using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Method = Metreos.Utilities.SqlBuilder.Method;
using DevicesTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.Devices;
using DirNumTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.DirectoryNumbers;
using UsersTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.Users;

namespace Metreos.ApplicationSuite.Storage
{
    /// <summary>
    ///     Provides data access to the Devices table, and any information which stem from that
    /// </summary>
    public class Devices : DbTable
    {
        public Devices(IDbConnection connection, LogWriter log, string applicationName, string partitionName, bool allowWrite) :
            base(connection, log, applicationName, partitionName, allowWrite) {}

        public Devices(DbTable table) : base(table) { }

        #region ResultValues
        public enum GetDeviceReturnValues
        {
            success,
            NoDevice,
            failure
        }
        #endregion

        #region GetUserForDevice
        /// <summary>
        /// This method returns the user id of the user associated with this device 
        /// </summary>
        public bool GetUserForDevice(uint deviceId, out uint userId)
        {
            userId = 0;

            SqlBuilder builder = new SqlBuilder(Method.SELECT, DevicesTable.TableName);
            builder.fieldNames.Add(DevicesTable.UserId);
            builder.where[DevicesTable.Id] = deviceId;

            AdvancedReadResultContainer result = ExecuteEasyQuery(builder);

            if(result.result == ReadResult.Success)
            {
                DataTable table = result.results;

                if ( (table == null) || (table.Rows.Count == 0) )
                    return false;

                DataRow row = table.Rows[0];

                if ( Convert.IsDBNull(row[DevicesTable.UserId]) )
                    return false;

                userId = Convert.ToUInt32(row[DevicesTable.UserId]);
                return ( userId < SqlConstants.StandardPrimaryKeySeed ) ? false : true;
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetUserForDevice method, using device Id: '{0}'\n" +
                    "Error message: {1}", deviceId, result.e.Message);

                return false;
            }
        }
        #endregion
    
        #region GetUserByDeviceMac
        /// <summary>
        /// Returns the userId and device deviceId associated with the device possesing the specified mac
        /// If 'primary' is 'true' then the method will only succeed is the device is a primary device for the user.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="userId"></param>
        /// <param name="primary"></param>
        /// <returns></returns>
        public bool GetUserByDeviceMac(string mac, out uint userId, out UserStatus userStatus, out uint deviceId, bool primary)
        {
            userId = deviceId = 0;
            userStatus = UserStatus.Ok;

            if (mac == null)
                return false;

            // Select as_phone_devices.as_users_id, mac_address, status FROM as_phone_devices LEFT JOIN as_users USING(as_users_id) WHERE mac_address = '
            string query = String.Format("SELECT {0}.{1}, {2}, {3}, {4} FROM {0} LEFT JOIN {5} USING({6}) WHERE {2} = '{7}'",
                DevicesTable.TableName,
                DevicesTable.UserId,
                DevicesTable.MacAddress,
                UsersTable.Status,
                DevicesTable.Id,
                UsersTable.TableName,
                UsersTable.Id,
                mac);
            
            if (primary)
                query = String.Format("{0} AND {1} = 1", query, DevicesTable.IsPrimaryDevice);
            
            AdvancedReadResultContainer result = ExecuteEasyQuery(query);

            if(result.result == ReadResult.Success)
            {
                DataTable table = result.results;

                if ( (table == null) || (table.Rows.Count == 0) )
                    return false;
                if (primary && table.Rows.Count > 1)
                {
                    object[] msgArray = new object[3] { "Found multiple users using device with MAC: '", mac, "' as their primary device!" } ;
                    log.Write(TraceLevel.Warning, "{0}{1}{2}", msgArray);
                    return false;
                } 

                DataRow row = table.Rows[0];
                
                deviceId = Convert.ToUInt32(row[DevicesTable.Id]);
                userId = Convert.ToUInt32(row[DevicesTable.UserId]); 
                userStatus = (UserStatus) Convert.ToInt32(row[UsersTable.Status]);

                return ( (userId < SqlConstants.StandardPrimaryKeySeed) ? false : true);
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetUserByDeviceMac method, using MAC: '{0}'\n" +
                    "Error message: {1}", mac, result.e.Message);
                return false;
            }
        }
        #endregion

        #region GetDeviceIdByDeviceName
        /// <summary>
        /// Takes the device name of a device and inserts the device Id into device Id then returns true,
        /// returns false otherwise.
        /// </summary>
        public bool GetDeviceIdByDeviceName(string deviceName, out uint deviceId)
        {
            deviceId = 0;
            if (deviceName == null)
                return false;

            SqlBuilder builder = new SqlBuilder(Method.SELECT, DevicesTable.TableName);
            builder.fieldNames.Add(DevicesTable.Id);
            builder.where[DevicesTable.MacAddress] = deviceName;

            AdvancedReadResultContainer result = ExecuteEasyQuery(builder);

            if(result.result == ReadResult.Success)
            {
                DataTable table = result.results;

                if ( (table == null) || (table.Rows.Count == 0) )
                    return false;

                if (table.Rows.Count > 1)
                {
                    object[] msgArray = new object[3] { "Found multiple devices with device name: '", 
                                                          deviceName, "' !" } ;
                    log.Write(TraceLevel.Error, "{0}{1}{2}", msgArray);
                    return false;
                } 

                DataRow row = table.Rows[0];
                deviceId = Convert.ToUInt32(row[DevicesTable.Id]);
                return (deviceId < SqlConstants.StandardPrimaryKeySeed) ? false : true;
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetDirNumsForDevice method, using device Id: '{0}'\n" +
                    "Error message: {1}", deviceId, result.e.Message);
                return false;
            }
        }
        #endregion
        
        #region GetDirNumsForDevice
        /// <summary>
        /// Returns a StringCollection of directory numbers associated with a given deviceId
        /// If 'primary' is 'true', this method will return true and assign a string collection of length one,
        /// containing the primary numb1r, if there is one and only one such number. Otherwise, it will return false.
        /// If 'primary' is 'false', this method will return true and assign a list of all directory numbers
        /// associated with the device, or it will return false if no such numbers are found. 
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="userId"></param>
        /// <param name="primary"></param>
        /// <returns></returns>
        public bool GetDirNumsForDevice(uint deviceId, out StringCollection dirNumbers, bool primary)
        {
            dirNumbers = null;
            if (deviceId < SqlConstants.StandardPrimaryKeySeed)
                return false;

            SqlBuilder builder = new SqlBuilder(Method.SELECT, DirNumTable.TableName);
            builder.fieldNames.Add(DirNumTable.DirectoryNumber);
            builder.where[DirNumTable.DeviceId] = deviceId;
            if (primary)
                builder.where[DirNumTable.IsPrimaryNumber] = true;

            AdvancedReadResultContainer result = ExecuteEasyQuery(builder);

            if(result.result == ReadResult.Success)
            {
                DataTable table = result.results;

                if ( (table == null) || (table.Rows.Count == 0) )
                    return false;
                if (primary && table.Rows.Count > 1)
                {
                    object[] msgArray = new object[3] { "Found multiple primary directory numbers for device Id: '", 
                                                          deviceId, "' !" } ;
                    log.Write(TraceLevel.Error, "{0}{1}{2}", msgArray);
                    return false;
                } 

                dirNumbers = new StringCollection();
                foreach (DataRow row in table.Rows)
                {
                    if ( ! (Convert.IsDBNull(row[DirNumTable.DirectoryNumber])) )
                    {
                        string number = Convert.ToString(row[DirNumTable.DirectoryNumber]);
                        if (number != null)
                            dirNumbers.Add(number);
                    }
                }

                if (primary)
                    return (dirNumbers.Count == 1);

                return (dirNumbers.Count == 0) ? false : true;

            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetDirNumsForDevice method, using device Id: '{0}'\n" +
                    "Error message: {1}", deviceId, result.e.Message);
                return false;
            }
        }
        #endregion

        
        public static string Execute(
            string ipAddress, 
            bool both, 
            string g_toIpAddress, 
            string g_fromIpAddress,
            string perspective,
            bool g_toUserAwareOfRecord,
            bool g_fromUserAwareOfRecord,
            ref string textMessage,
            ref string softKeyName)
        {
            bool sendExecuteTo = g_toUserAwareOfRecord && g_toIpAddress != "NONE";
            bool sendExecuteFrom = g_fromUserAwareOfRecord && g_fromIpAddress != "NONE";
		
            // Strip off any port information from the ipAddress
            int portIndex = ipAddress.IndexOf(':');
            if(portIndex != -1)
            {
                ipAddress = ipAddress.Substring(0, portIndex);
            }		

            // softKeyName can be equal to "Start" or "Stop" or "NONE"

            if(softKeyName == "Stop" && perspective == "SELF")
                textMessage = "Recording initiated.";

            else if(softKeyName == "Stop" && perspective == "OTHER")
                textMessage = "The other end initiated recording.";

            else if(softKeyName == "Stop" && perspective == "NONE")
                textMessage= "Recording initiated.";

            else if(softKeyName == "Start" && perspective == "SELF")
                textMessage = "Recording stopped.";

            else if(softKeyName == "Start" && perspective == "OTHER")
                textMessage = "The other end stopped the recording.";

            else if(softKeyName == "Start" && perspective == "NONE")
                textMessage= "Recording enabled.";

            // No IpAddress specified, so send to both
            if(ipAddress == "NONE")
            {  
                if(!sendExecuteTo && !sendExecuteFrom)
                    return "SendExecuteBoth";

                else if(sendExecuteTo && !sendExecuteFrom)
                    return "SendExecuteToUser";

                else if(!sendExecuteTo && sendExecuteFrom)
                    return "SendExecuteFromUser";

                else
                    return "error";
            }
            else 
            {
                // Another way to think of "NONE" is the first run through the application
                if(perspective == "NONE")
                {
                    if(g_toIpAddress == ipAddress)
                    {
                        return "SendResponseToUser";
                    }
                    else if(g_fromIpAddress == ipAddress)
                    {
                        return "SendResponseFromUser";
                    }
                } 
                // Another way to think of "SELF" is a softkey initiated phone request
                else if(perspective == "SELF")
                {
                    if(g_toIpAddress == ipAddress)
                    {
                        if(g_fromUserAwareOfRecord)
                        {
                            return "SendResponseToUser_SendExecuteFromUser";
                        }
                        else
                        {
                            Console.WriteLine("HHAHAHAH");
                            return "SendResponseToUser";
                        }
                    }
                    else if(g_fromIpAddress == ipAddress)
                    {
                        if(g_toUserAwareOfRecord)
                        {
                            return "SendResponseFromUser_SendExecuteToUser";
                        }
                        else
                        {
                            return "SendResponseFromUser";
                        }
                    } 
                }
                // Another way to think of "OTHER" is SendExecute-initaited phone request
                else if(perspective == "OTHER")
                {
                    if(g_toIpAddress == ipAddress)
                    {
                        return "SendResponseToUser";
                    }
                    else if(g_fromIpAddress == ipAddress)
                    {
                        return "SendResponseFromUser";
                    }
                }             
            }

            return "error";
        }
    }
}
