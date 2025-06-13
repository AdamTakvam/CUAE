using System;
using System.Collections;
using System.Diagnostics;
using System.Threading; 

using Metreos.Core;
using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;
using Metreos.Utilities;
// for ICiscoDeviceList
using Metreos.Interfaces;

using nsoftware.IPWorks;

namespace Metreos.Providers.CiscoDeviceListX
{
    /// <summary>
    /// SNMP reader for accessing device information from a CCM 5.0 cluster
    /// using the SNMP v2c protocol and a community string.
    /// </summary>
    internal sealed class SNMPReader : DeviceListXReader
    {
        //Snmpmgr response error index that indicates that no error had occured
        private const int ERROR_INDEX_NO_ERROR				        = 0;

        // Maximum numbers of objects per response packet
        private const int MAX_REPEATERS                             = 64;
		
        // Phone entry object indeces
        private const int INDEX_CCM_PHONE_NAME						= 1;
        private const int INDEX_CCM_PHONE_INET_ADDRESS				= 2;
        private const int INDEX_CCM_PHONE_DESCRIPTION				= 3;
        private const int INDEX_CCM_PHONE_DEVICE_POOL_INDEX			= 4;
        private const int INDEX_CCM_PHONE_STATUS					= 5;
        private const int INDEX_CCM_PHONE_PRODUCT_TYPE_INDEX		= 6;
		
        // Device pool object indeces
        private const int INDEX_CCM_DEVICE_POOL_NAME                = 1;

        // Device type object indeces
        private const int INDEX_CCM_PRODUCT_TYPE					= 1;
        private const int INDEX_CCM_PRODUCT_NAME					= 2;

        // number of above INDEX_CCM_PHONE_* parameters
        private const int PHONE_ENTRY_OUTBOUND_OBJ_COUNT			= 6;

        // number of product type parameters
        private const int PRODUCT_TYPE_OUTBOUND_OBJ_COUNT           = 1;

        // number of device pool parameters
        private const int DEVICE_POOL_OUTBOUND_OBJ_COUNT            = 1;

        // Default SNMP community to use in case none was specified on the cluster.
        private const string DEFAULT_SNMP_COMMUNITY                 = "public";

        // Snmpgmgr ForceLocalPort property name and the value that we want to assign to it.
        private const string SNMPMGR_FORCELOCALPORT                 = "ForceLocalPort";
        private const string SNMPMGR_FORCELOCALPORT_VALUE           = "False";

        // default value for the lastReceivedOidIndex field.
        private const int DEFAULT_LAST_READ_INDEX                   = 0;

        // OID identifiers for required SNMP objects
        private abstract class CcmMibOid
        {
            public const string ccmPhoneEntry				= "1.3.6.1.4.1.9.9.156.1.2.1.1";
            public const string ccmPhoneProductTypeIndex	= "1.3.6.1.4.1.9.9.156.1.2.1.1.18";
            public const string ccmPhoneDescription			= "1.3.6.1.4.1.9.9.156.1.2.1.1.4";
            public const string ccmPhoneStatus				= "1.3.6.1.4.1.9.9.156.1.2.1.1.7";
            public const string ccmPhoneDevicePoolIndex		= "1.3.6.1.4.1.9.9.156.1.2.1.1.13";
            public const string ccmPhoneInetAddress			= "1.3.6.1.4.1.9.9.156.1.2.1.1.15";
            public const string ccmPhoneName				= "1.3.6.1.4.1.9.9.156.1.2.1.1.20";

            public const string ccmDevicePoolEntry          = "1.3.6.1.4.1.9.9.156.1.1.7.1";
            public const string ccmDevicePoolName           = "1.3.6.1.4.1.9.9.156.1.1.7.1.2";

            public const string	ccmProductTypeEntry			= "1.3.6.1.4.1.9.9.156.1.1.8.1";
            public const string ccmProductType				= "1.3.6.1.4.1.9.9.156.1.1.8.1.2";
            public const string ccmProductName				= "1.3.6.1.4.1.9.9.156.1.1.8.1.3";
        }

        /// <summary>
        /// Enums that describe the status of a phone.
        /// </summary>
        private enum PhoneStatus
        {
            Unknown               = 1,
            Registered            = 2,
            Unregistered          = 3,
            Rejected              = 4,
            PartiallyRegistered   = 5
        }

        // SNMP Manager object
        private Snmpmgr snmpManager;

        // when set to true, we keep requesting more data
        // when there is no more data to retrieve (we start getting children
        // outside our sub-tree of interest)
        private volatile bool readOperationComplete;

        // event used to interlaces sends in receives (send-wait for response-response)
        private AutoResetEvent responseReceivedEvent = new AutoResetEvent(true);

        // SendGetCommand* delegate to use inside the generic ReadExecuter method
        private delegate bool SendGetCommandDelegate(int lastReadIndex);
        
        // tracks the last-received oid index so that it can be used in the next send.
        private volatile int lastReceivedOidIndex;

        // synchronization object for use by OnResponse
        private object responseLock = new object();

        // Hashtable used to temporarily hold retrieved devices 
        private Hashtable devices;

        // Mapping between devicePoolIndex and devicePoolName
        // and corresponding boolean flag used to indicate that the device pool
        // table was retrieved successfully (if set to true)
        private Hashtable devicePoolMap;
        private bool devicePoolTableRetrieved = false;

        // Mapping between product type index and product type enum value 
        // and corresponding boolean flag used to indicate that the product type
        // table was retrieved successfully (if set to true)
        private Hashtable productTypeMap;
        private bool productTypeTableRetrieved = false;

        public SNMPReader(LogWriter log, CallManagerCluster cluster) : base(log, cluster)
        {
            devices = new Hashtable();
            devicePoolMap = new Hashtable();
            devicePoolMap[string.Intern("0")] = "Unknown";
            productTypeMap = new Hashtable();
        }

        internal override bool Initialize()
        {
            snmpManager = new Snmpmgr();
            snmpManager.RemoteHost = cluster.PublisherIP.ToString();

            // The following line disables the "ForceLocalPort" property that by default (?) insisted
            // on using 161 as the local port, resulting in a socket-in-use exception.
            snmpManager.Config(string.Format("{0}={1}", SNMPMGR_FORCELOCALPORT, SNMPMGR_FORCELOCALPORT_VALUE));
            snmpManager.SNMPVersion = SnmpmgrSNMPVersions.snmpverV2c;
            snmpManager.Community = cluster.SnmpCommunity;
            
            lastReceivedOidIndex = DEFAULT_LAST_READ_INDEX;
            responseReceivedEvent.Set();

            if (snmpManager.Community == null || snmpManager.Community == string.Empty)
                snmpManager.Community = DEFAULT_SNMP_COMMUNITY;

            try { System.Net.IPAddress.Parse(snmpManager.RemoteHost); }
            catch
            {
                log.Write(TraceLevel.Warning, "Invalid IP Address specified for cluster '{0}'", cluster.Name);
                return false;
            }
            
            return true;
        }

        // Sets the callbacks for the read, setting the responseHandler to one of the OnResponse* handlers,
        // depending on what table is being read.
        private void RegisterCallbacks(Snmpmgr.OnResponseHandler responseHandler)
        {
            snmpManager.OnResponse += responseHandler; //responseHandler;
            
            snmpManager.OnError += new Snmpmgr.OnErrorHandler(this.OnError);

            snmpManager.OnBadPacket += new Snmpmgr.OnBadPacketHandler(this.OnBadPacket);
        }

        /// <summary>
        /// Retrieves a list of devices from the CallManager
        /// </summary>
        /// <returns>true if the retrieval succeeded, false otherwise.</returns>
        internal override bool RetrieveDeviceList(ref bool shutdownFlag)
        {
            bool success = true;

            // Read data from the publisher
            if (! shutdownFlag)
                PerformProductTypeReadOperation(ref shutdownFlag);
            else
            {
                log.Write(TraceLevel.Info, "Shutdown signaled, exiting.");
                return false;
            }

            if (! shutdownFlag)
                PerformDevicePoolReadOperation(ref shutdownFlag);
            else
            {
                log.Write(TraceLevel.Info, "Shutdown signaled, exiting.");
                return false;
            }
            
            if (! shutdownFlag)
                success = PerformDeviceInfoReadOperation(ref shutdownFlag);
            else
            {
                log.Write(TraceLevel.Info, "Shutdown signaled, exiting.");
                return false;
            }
            
            
            //device read failed
            lock (devices.SyncRoot)
            {
                if (! success)
                {
                    devices.Clear();
                }
                else if (devices.Count == 0)
                    success = false;
                else
                {
                    base.deviceList.Items = new DeviceListDevice[devices.Count];
                    int i = 0;
                    foreach (DeviceListDevice device in devices.Values)
                    {
                        if (shutdownFlag)
                        {
                            success = false;
                            break;
                        }
                        base.deviceList.Items[i++] = device;
                    }
                    base.deviceList.Error = null;
                }
            }
            
            return success;
        }

        /// <summary>
        /// Performs a synchronized read of data from a CCM SNMP data table. Generic method used by more
        /// specific (per-table) reader methods.
        /// </summary>
        /// <param name="shutdownFlag">thread shutdown flag</param>
        /// <param name="sendGetCommand">delegate to a specific (per-table) SendGet command</param>
        /// <returns></returns>
        private bool ExecuteRead(ref bool shutdownFlag, SendGetCommandDelegate sendGetCommand)
        {
            bool success = true;
            
            // ensure that the SNMP Manager gets disposed when we're done with it.
            using (snmpManager)
            {
                try
                {
                    // wait for the begin read event.
                    while (responseReceivedEvent.WaitOne())
                    {
                        // if shutdown flag is set, exit while loop
                        if(shutdownFlag)
                        {
                            break;
                        }

                        // zero out the object fields
                        snmpManager.Reset();

                        // when a response is received and the read is deemed complete, this boolean will be set
                        // and the loop will exit.
                        if (readOperationComplete)
                        {
                            break;
                        }
                        
                        // Execute the SendGetCommand specified by the sendGetCommand delegate
                        sendGetCommand(lastReceivedOidIndex);
                    }
                }
                catch
                {
                    success = false;
                    // rethrow exception to lead the particular PerformXYZReadOperation handle it in its own way
                    throw;
                }                
            }
            
            return success;
        }

        private bool PerformProductTypeReadOperation(ref bool shutdownFlag)
        {
            bool success = true;

            // ensure that none of the other reads proceed until this read operation is complete.
            // event gets Set() by async callback when read is complete. 
//            orderEnforceEvent.WaitOne();

            // reset the snmp manager
            Initialize();

            // ensure that the SNMP Manager gets disposed when we're done with it.
            using (snmpManager)
            {    
                // re-register the needed callbacks.
                RegisterCallbacks(new Snmpmgr.OnResponseHandler(this.OnResponseProductType));

                bool found = false;
                if(cluster.Subscribers == null || cluster.Subscribers.Length == 0)
                {
                    // initialize sets remote address to cluster publisher IP
                    found = true;
                }
                else
                {
                    foreach (CallManagerSubscriber sub in cluster.Subscribers)
                    {   
                        try { System.Net.IPAddress.Parse(sub.SubscriberIP.ToString()); }
                        catch
                        {
                            log.Write(TraceLevel.Warning, "Invalid IP Address specified for subscriber '{0}'", sub.Name);
                            continue;
                        }

                        snmpManager.RemoteHost = sub.SubscriberIP.ToString();
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    readOperationComplete = false;
                    try
                    {
                        success = ExecuteRead(ref shutdownFlag, new SendGetCommandDelegate(this.SendGetProductTypeCommand));
                    }
                    catch (Exception e)
                    {
                        log.Write(TraceLevel.Warning, "An error occured while retrieving product type entry table. Aborting read. Error message: '{0}'", e.Message);
                        success = false;
                    }
                }
                else
                {
                    log.Write(TraceLevel.Warning, "Could not find a valid subscriber to read product type information.");
                    success = false;
                }

                return success;
            }
        }

        private bool PerformDeviceInfoReadOperation(ref bool shutdownFlag)
        {
            bool success = true;

            // Reset the SnmpMgr for the new read.
            Initialize();


            // re-register the needed callbacks.
            RegisterCallbacks(new Snmpmgr.OnResponseHandler(this.OnResponse));

            if(cluster.Subscribers == null || cluster.Subscribers.Length == 0)
            {
                using (snmpManager)
                {
                    log.Write(TraceLevel.Warning, "No subscribers configured for cluster '{0}'; retrieving data from publisher.", cluster.Name);
                    readOperationComplete = false;
                
                    try
                    {
                        success = ExecuteRead(ref shutdownFlag, new SendGetCommandDelegate(this.SendGetDeviceCommand));
                    }
                    catch
                    {
                        log.Write(TraceLevel.Warning, "An error occured while retrieving device information table. Aborting read.");
                        return false;
                    }

                    if (success)
                        log.Write(TraceLevel.Verbose, "Refresh of data from publisher '{0}' succeeded.", cluster.Name);
                    else
                        log.Write(TraceLevel.Warning, "Refresh of data from publisher '{0}' failed.", cluster.Name);
                } //using 
            } // if
            else
            {
                foreach (CallManagerSubscriber sub in cluster.Subscribers)
                {
                    if ( ! this.Initialize())
                        continue;
                    
                    using (snmpManager)
                    {
                        bool readSuccess = false;
                        // re-register the needed callbacks.
                        RegisterCallbacks(new Snmpmgr.OnResponseHandler(this.OnResponse));

                        this.snmpManager.RemoteHost = sub.SubscriberIP.ToString();
                
                        log.Write(TraceLevel.Verbose, "Refresh of data from subscriber '{0}' starting.", sub.Name);
                        
                        readOperationComplete = false;
                        
                        try
                        {
                            readSuccess = ExecuteRead(ref shutdownFlag, new SendGetCommandDelegate(this.SendGetDeviceCommand));
                        }
                        catch
                        {
                            // ignore exception, handled in if statement below.
                        }
                        
                        if (readSuccess)
                            log.Write(TraceLevel.Verbose, "Refresh of data from subscriber '{0}' succeeded.", sub.Name);
                        else
                            log.Write(TraceLevel.Warning, "Refresh of data from subscriber '{0}' failed.", sub.Name);

                        // how to handle partial failures?
                        success &= readSuccess;
                    } //using 
                } //foreach 
            } // outmost else
            
            return success;
        } // PerformDeviceInfoReadOperation

        /// <summary>
        /// Reads the DLX device pool data from the CallManager currently bound to snmpManager
        /// </summary>
        /// <returns></returns>
        private bool PerformDevicePoolReadOperation(ref bool shutdownFlag)
        {
            bool success = false;

            // reset the snmp manager
            Initialize();

            // ensure that the SNMP Manager gets disposed when we're done with it.
            using (snmpManager)
            {    
                // re-register the needed callbacks.
                RegisterCallbacks(new Snmpmgr.OnResponseHandler(this.OnResponseDevicePool));

                bool found = false;
                if(cluster.Subscribers == null || cluster.Subscribers.Length == 0)
                {
                    // initialize sets remote address to cluster publisher IP
                    found = true;
                }
                else
                {
                    foreach (CallManagerSubscriber sub in cluster.Subscribers)
                    {   
                        try { System.Net.IPAddress.Parse(sub.SubscriberIP.ToString()); }
                        catch
                        {
                            log.Write(TraceLevel.Warning, "Invalid IP Address specified for subscriber '{0}'", sub.Name);
                            continue;
                        }

                        snmpManager.RemoteHost = sub.SubscriberIP.ToString();
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    readOperationComplete = false;
                    try
                    {
                        success = ExecuteRead(ref shutdownFlag, new SendGetCommandDelegate(this.SendGetDevicePoolCommand));
                    }
                    catch (Exception e)
                    {
                        log.Write(TraceLevel.Warning, "An error occured while retrieving device pool entry table. Aborting read. Error message: '{0}'", e.Message);
                        success = false;
                    }
                }
                else
                {
                    log.Write(TraceLevel.Warning, "Could not find a valid subscriber to read product type information.");
                    success = false;
                }

                return success;
            }
        }


        // Formats and OID by appending an index to a base OID
        private string FormatOid(string prefix, int objCount)
        {
            return string.Format("{0}.{1}", prefix, objCount);
        }

        // formats a received byte array to one of a proper size for the BitConverter.To*(byte[], int) functions
        /// <summary>
        /// Formats a passed in byte array into a byte array in a format that is acceptable to the BitConverter.To*Int*(byte[] int)
        /// functions.
        /// </summary>
        /// <param name="byteArr">A byte array to format.</param>
        /// <returns>A byte array that is either null, or of length == 2 or length == 4</returns>
        private byte[] FormatByteArray(byte[] byteArr)
        {
            uint outputArraySize = 0;
            
            if (byteArr.Length == 0)
                return null;
            else if (byteArr.Length == 1 || byteArr.Length == 2)
                outputArraySize = 2;
            else if (byteArr.Length == 3 || byteArr.Length == 4)
                outputArraySize = 4;
            else 
                return null;

            byte[] formattedArray = new byte[outputArraySize];
            formattedArray.Initialize();
            byteArr.CopyTo(formattedArray, 0);
            
            return formattedArray;
        }

        /// <summary>
        /// Concatenates all the elements of a byte array together, inserting a
        /// separator between each element, and returns the resulting string.
        /// </summary>
        /// <param name="byteArr">Byte array of elements to concatenate.</param>
        /// <param name="separator">The string that will be used to separate elements of byteArr from one another.</param>
        /// <param name="separateLast">If true, the resulting string will end with the separator, otherwise it will end with the last element of byteArr</param>
        /// <returns>The concatenated string.</returns>
        private string ByteArrToSplitString(byte[] byteArr, string separator, bool separateLast)
        {
            if (byteArr == null || byteArr.Length == 0)
                return null;

            string splitString = string.Empty;
            foreach (byte b in byteArr)
            {
                splitString += b.ToString() + separator;
            }

            if (separateLast == false)
                splitString = splitString.Remove(splitString.Length - separator.Length, separator.Length);

            return splitString;
        }


        /// <summary>
        /// Separates the OID base prefix and the index of the child in the subtree
        /// </summary>
        /// <param name="oid">Oid to split</param>
        /// <param name="prefix">phoneTableEntry OID prefix</param>
        /// <param name="index">index of the node</param>
        private void SplitOidAndIndex(string oid, out string prefix, out string index)
        {
            int lastDotIndex = oid.LastIndexOf(".");
            prefix = oid.Substring(0, lastDotIndex);
            if (lastDotIndex >= oid.Length)
                index = "0";
            else
                index = oid.Substring(lastDotIndex + 1);
        }

        
        /// <summary>
        /// Sends the SNMP packet that requests device information
        /// </summary>
        /// <param name="initialObject">Value that is appended to the end of the base oid to indicate where to begin the read</param>
        /// <returns>success or failure of send</returns>
        private bool SendGetDeviceCommand(int lastReadIndex)
        {
            // The number of fields that we're requesting
            snmpManager.ObjCount = PHONE_ENTRY_OUTBOUND_OBJ_COUNT;

            snmpManager.ObjId[INDEX_CCM_PHONE_NAME] = FormatOid(CcmMibOid.ccmPhoneName, lastReadIndex);
            snmpManager.ObjId[INDEX_CCM_PHONE_INET_ADDRESS] = FormatOid(CcmMibOid.ccmPhoneInetAddress, lastReadIndex);
            snmpManager.ObjId[INDEX_CCM_PHONE_DESCRIPTION] = FormatOid(CcmMibOid.ccmPhoneDescription, lastReadIndex);
            snmpManager.ObjId[INDEX_CCM_PHONE_DEVICE_POOL_INDEX] = FormatOid(CcmMibOid.ccmPhoneDevicePoolIndex, lastReadIndex);
            snmpManager.ObjId[INDEX_CCM_PHONE_STATUS] = FormatOid(CcmMibOid.ccmPhoneStatus, lastReadIndex);
            snmpManager.ObjId[INDEX_CCM_PHONE_PRODUCT_TYPE_INDEX] = FormatOid(CcmMibOid.ccmPhoneProductTypeIndex, lastReadIndex);

            // try/catch block is not necessary here if the function is only being called from RetrieveDeviceInformation,
            // as that will handle any exceptions that may occur during the read.
            snmpManager.SendGetBulkRequest(0, MAX_REPEATERS);

            return true;
        } // SendGetDeviceCommand

        /// <summary>
        /// Sends the SNMP packet that requests device information
        /// </summary>
        /// <param name="initialObject">Value that is appended to the end of the base oid to indicate where to begin the read</param>
        /// <returns>success or failure of send</returns>
        private bool SendGetProductTypeCommand(int lastReadIndex)
        {
            // The number of fields that we're requesting
            snmpManager.ObjCount = PRODUCT_TYPE_OUTBOUND_OBJ_COUNT;

            snmpManager.ObjId[INDEX_CCM_PRODUCT_TYPE] = FormatOid(CcmMibOid.ccmProductType, lastReadIndex);

            // try/catch block is not necessary here if the function is only being called from RetrieveDeviceInformation,
            // as that will handle any exceptions that may occur during the read.
            snmpManager.SendGetBulkRequest(0, MAX_REPEATERS);

            return true;
        } // SendGetProductTypeCommand

        private bool SendGetDevicePoolCommand(int lastReadIndex)
        {
            // The number of fields that we're requesting
            snmpManager.ObjCount = DEVICE_POOL_OUTBOUND_OBJ_COUNT;

            snmpManager.ObjId[INDEX_CCM_DEVICE_POOL_NAME] = FormatOid(CcmMibOid.ccmDevicePoolName, lastReadIndex);

            // try/catch block is not necessary here if the function is only being called from RetrieveDeviceInformation,
            // as that will handle any exceptions that may occur during the read.
            snmpManager.SendGetBulkRequest(0, MAX_REPEATERS);

            return true;
        } // SendGetDevicePoolCommand

        #region Event Handlers for the SNMP Manager
        private void OnBadPacket(object sender, SnmpmgrBadPacketEventArgs e)
        {
            log.Write(TraceLevel.Warning, "A bad packet was received while performing SNMP read. Error code: {0}", e.ErrorCode);
            throw new Exception(e.ErrorDescription);
        }


        private void OnError(object sender, SnmpmgrErrorEventArgs e)
        {
            log.Write(TraceLevel.Warning, "An error occured while performing SNMP read. Error code: {0}", e.ErrorCode);
            throw new Exception(e.Description);
        }

        
        private void OnResponseDevicePool(object sender, SnmpmgrResponseEventArgs e)
        {
            lock (responseLock)
            {
                if (e.ErrorIndex != ERROR_INDEX_NO_ERROR)
                {
                    readOperationComplete = true;
                    log.Write(TraceLevel.Warning, "SNMP Response returned error: {0}", e.ErrorDescription);
                    throw new Exception(e.ErrorDescription);
                }
                else
                {
                    string responseOid = null;
                    string oidIndex = null;

                    for (int i = 1; i <= snmpManager.ObjCount; i++)
                    {
                        string oidBase;

                        responseOid = snmpManager.ObjId[i] as string;
                        SplitOidAndIndex(responseOid, out oidBase, out oidIndex);
    					
                        switch (oidBase)
                        {
                            case CcmMibOid.ccmDevicePoolName: 
                            {
                                int index = int.Parse(oidIndex);
                                string poolName = snmpManager.ObjValue[i] as String;
                                devicePoolMap[string.Intern(index.ToString())] = poolName;
                                break;
                            }
                            default :
                            {
                                // since we received a node that is not in our tree of interest
                                // stop requesting more data.
                                readOperationComplete = true;
                                break;
                            }
                        } //switch
                    } // for

                    if (! readOperationComplete)
                    {
                        try { lastReceivedOidIndex = Int32.Parse(oidIndex); }
                        catch
                        {
                            log.Write(TraceLevel.Warning, "SNMP Reader was unable to parse the last received OID index.");
                            readOperationComplete = true;
                        }
                    } // if readOperationComplete
                    else if (devicePoolMap.Count > 0)
                        devicePoolTableRetrieved = true; // flag used to indicate that the device pool was retrieved successfully

                } //else

                responseReceivedEvent.Set();
            } //lock
        } // OnResponseDevicePool


        private void OnResponseProductType(object sender, SnmpmgrResponseEventArgs e)
        {
            lock (responseLock)
            {
                if (e.ErrorIndex != ERROR_INDEX_NO_ERROR)
                {
                    readOperationComplete = true;
                    log.Write(TraceLevel.Warning, "SNMP Response returned error: {0}", e.ErrorDescription);
                    throw new Exception(e.ErrorDescription);
                }
                else
                {
                    string responseOid = null;
                    string oidIndex = null;

                    for (int i = 1; i <= snmpManager.ObjCount; i++)
                    {
                        string oidBase;
                        responseOid = snmpManager.ObjId[i] as string;
                        SplitOidAndIndex(responseOid, out oidBase, out oidIndex);
                        lock (productTypeMap.SyncRoot)
                        {
                            switch (oidBase)
                            {
                                case CcmMibOid.ccmProductType : 
                                {
                                    productTypeMap[oidIndex] = snmpManager.ObjValue[i] as string;
                                    break;
                                }
                                default :
                                {
                                    // since we received a node that is not in our tree of interest
                                    // stop requesting more data.
                                    readOperationComplete = true;
                                    break;
                                }
                            } //switch
                        } //lock
                    } //for

                    if (! readOperationComplete)
                    {
                        try { lastReceivedOidIndex = Int32.Parse(oidIndex); }
                        catch
                        {
                            log.Write(TraceLevel.Warning, "SNMP Reader was unable to parse the last received OID index.");
                            readOperationComplete = true;
                        }
                    } // if readOperationComplete
                    else if (productTypeMap.Count > 0)
                        productTypeTableRetrieved = true; // flag used to indicate that theproduct table was retrieved successfully

                } //else

                responseReceivedEvent.Set();
            }
        }

        private void OnResponse(object sender, SnmpmgrResponseEventArgs e)
        {
            lock (responseLock)
            {
                if (e.ErrorIndex != ERROR_INDEX_NO_ERROR)
                {
                    readOperationComplete = true;
                    log.Write(TraceLevel.Warning, "SNMP Response returned error: {0}", e.ErrorDescription);
                    throw new Exception(e.ErrorDescription);
                }
                else
                {
                    string responseOid = null;
                    DeviceListDevice device;
                    string oidIndex = null;

                    for (int i = 1; i <= snmpManager.ObjCount; i++)
                    {
                        string oidBase;
                        bool addDeviceToList = false;

                        responseOid = snmpManager.ObjId[i] as string;
                        SplitOidAndIndex(responseOid, out oidBase, out oidIndex);
						
                        lock (devices.SyncRoot)
                        {
                            if (devices.ContainsKey(string.Intern(oidIndex)))
                                device = devices[string.Intern(oidIndex)] as DeviceListDevice;
                            else
                            {
                                addDeviceToList = true;
                                device = new DeviceListDevice();
                                // Temporarily set CSS to string.Empty until we figure out how to retrieve the CSS properly.
                                device.css = string.Empty;
                                device.ccmIP = cluster.PublisherIP.ToString();
                            }
                        }

                        switch (oidBase)
                        {
                            case CcmMibOid.ccmPhoneDescription : 
                            {
                                device.description = snmpManager.ObjValue[i] as string;
                                break;
                            }
                            case CcmMibOid.ccmPhoneDevicePoolIndex :
                            {
                                // retrieve pool name from devicepool map
                                if (devicePoolTableRetrieved)
                                {
                                    try
                                    {
                                        string devicePoolIndex = snmpManager.ObjValue[i] as string;
                                        device.pool = devicePoolMap[string.Intern(devicePoolIndex)] as string;
                                    }
                                    catch {device.pool = null; }
                                }
                                else
                                    device.pool = null;

                                break;
                            }
                            case CcmMibOid.ccmPhoneInetAddress :
                            {
                                // Format the byte array into a proper format.
                                byte[] formated = FormatByteArray(snmpManager.ObjValueB[i]);
                                device.ip = ByteArrToSplitString(formated, ".", false);
                                break;
                            }
                            case CcmMibOid.ccmPhoneName :
                            {
                                device.name = snmpManager.ObjValue[i] as string;
                                break;
                            }
                            case CcmMibOid.ccmPhoneProductTypeIndex :
                            {
                                // retrieve pool name from devicepool map
                                if (productTypeTableRetrieved)
                                {
                                    string productTypeIndex = snmpManager.ObjValue[i] as string;
                                    try { device.type = productTypeMap[string.Intern(productTypeIndex)] as string; }
                                    catch {device.type = null; }
                                }
                                else
                                    device.type = null;
                                
                                break;
                            }
                            case CcmMibOid.ccmPhoneStatus :
                            {
                                // REFACTOR: DLX for 4.1 and 5.0 enums for device status do not overlap
                                // as per the IP Services developer notes and the SNMP MIB
                                // so we need to perform a tiny little translation here to 
                                // map the values to the old. In the future, a more rugged system for ensuring
                                // data context consistency should be implemented.
                                PhoneStatus status = PhoneStatus.Unknown;
                                try
                                {
                                    status = (PhoneStatus) Enum.Parse(typeof(PhoneStatus), snmpManager.ObjValue[i] as string, true);
                                }
                                catch
                                {
                                    log.Write(TraceLevel.Info, "Unable to determine type for DLX device.");
                                }

                                switch (status)
                                {
                                    case PhoneStatus.Registered : device.status = ICiscoDeviceList.STATUS_REGISTERED; break;
                                    case PhoneStatus.Unregistered :
                                    case PhoneStatus.Rejected :
                                    case PhoneStatus.PartiallyRegistered : device.status = ICiscoDeviceList.STATUS_FOUND_AND_UNREGISTERED; break;
                                    default: device.status = ICiscoDeviceList.STATUS_NONE; break;
                                }

                                break;
                            }
                            default :
                            {
                                // since we received a node that is not in our tree of interest
                                // stop requesting more data.
                                readOperationComplete = true;
                                addDeviceToList = false;
                                break;
                            }
                        }

                        if (addDeviceToList)
                        {
                            devices.Add(string.Intern(oidIndex), device);
                        }
                    }

                    if (! readOperationComplete)
                    {
                        try { lastReceivedOidIndex = Int32.Parse(oidIndex); }
                        catch
                        {
                            log.Write(TraceLevel.Warning, "SNMP Reader was unable to parse the last received OID index.");
                            readOperationComplete = true;
                        }
                    } // if readOperationComplete
                }

                responseReceivedEvent.Set();
            }
        }
        #endregion
    } // SNMPReader
} //namespace
