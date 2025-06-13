using System;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Collections;

using Metreos.Core.IPC;
using Metreos.Core.IPC.Flatmaps;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.Messaging.MediaCaps;

namespace Metreos.Providers.PCapService
{
    public enum IPCState
    {
        Disconnected = 0,
        Connecting,
        Connected
    }

    // Container class for active call data
    public class MonitoredCallData
    {
        private uint resultCode;
        public uint ResultCode { get { return resultCode; } set { resultCode = value; } }

        private uint callIdentifier;
        public uint CallIdentifier { get { return callIdentifier; } set { callIdentifier = value; } }

        private uint rtpPort1;
        public uint RtpPort1 { get { return rtpPort1; } set { rtpPort1 = value; } }

        private uint rtpPort2;
        public uint RtpPort2 { get { return rtpPort2; } set { rtpPort2 = value; } }

        private uint callType;
        public uint CallType { get { return callType; } set { callType = value; } }

        private string callerDN;
        public string CallerDN { get { return callerDN; } set { callerDN = value; } }

        private string calleeDN;
        public string CalleeDN { get { return calleeDN; } set { calleeDN = value; } }

        private string phoneIp;
        public string PhoneIp { get { return phoneIp; } set { phoneIp = value; } }
    }

    public delegate void OnActiveCallListDelegate(uint transactionId, Hashtable calldata);              // List of active calls
    public delegate void OnMonitorCallRequestAckDelegate(uint transactionId, MonitoredCallData data);   // Ack on monitor call request 
    public delegate void OnStopMonitorCallRequestAckDelegate(uint rc, uint callIdentifier);             // Ack on stop monitor call request
    public delegate void OnCallEstablishedDelegate();                                                   // Call gets established
    public delegate void NullDelegate();

    /// <summary>Client class for communicating with the PCapService</summary>
    public class PCapServiceIpcClient
    {
        public abstract class Consts
        {
            public const int IpcConnectTimeout      = 120;  // seconds
            public const string ConnectorThreadName = "PCap service connect thread";
        }

        private IpcFlatmapClient ipcClient;

        public event NullDelegate onServiceDown;
        //public event OnCallEstablishedDelegate onCallEstablished;
        public event OnActiveCallListDelegate onActiveCallList;
        public event OnMonitorCallRequestAckDelegate onMonitorCallRequestAck;
        public event OnStopMonitorCallRequestAckDelegate onStopMonitorCallRequestAck; 

        private IPCState state = IPCState.Disconnected;
        public IPCState State { get { return state; } }

        private LogWriter log;
        public LogWriter Log { set { log = value; } }

        private uint portbaseLWM;
        public uint PortbaseLWM { set { portbaseLWM = value; } }

        private uint portbaseHWM;
        public uint PortbaseHWM { set { portbaseHWM = value; } }

        private uint portStep;
        public uint PortStep { set { portStep = value; } }

        private uint maxMonitorTime;
        public uint MaxMonitorTime { set { maxMonitorTime = value; } }

        private uint logLevel;
        public uint LogLevel { set { logLevel = value; } }

        #region Construction/Open/Close
        /// <summary>Default constructor</summary>
        public PCapServiceIpcClient()
        {
            ipcClient = new IpcFlatmapClient();
			ipcClient.onConnect          = new OnConnectDelegate(ipcClient_onConnect);
			ipcClient.onFlatmapMessageReceived  = new OnFlatmapMessageReceivedDelegate(ipcClient_OnMessageReceived);
			ipcClient.onClose            = new OnCloseDelegate(ipcClient_onConnectionClosed);
        }

        /// <summary>Open an IPC connection to the pcap-service.</summary>
        /// <param name="ipEndpoint">Address of the pcap-service.</param>
        /// <returns>True if connected, false otherwise.</returns>
        public void Startup(IPEndPoint ipEndpoint)
        {
            if(ipcClient == null)
                throw new ObjectDisposedException(typeof(PCapServiceProvider).Name);

            if(ipEndpoint == null || ipEndpoint.Address == null)
                throw new Metreos.Core.StartupFailedException("Invalid pcap-service process address in PCapServiceIpcClient.Startup()");

            state = IPCState.Connecting;
			active = true;
            ipcClient.RemoteEp = ipEndpoint;
			ipcClient.Start();
        }

        /// <summary>Close the IPC connection to the pcap-service.</summary>
        public void Close()
        {
			active = false;
            ipcClient.Close();
        }

		private bool active;

        #endregion // Construction/Open/Close

        #region IPC Client Callbacks

        /// <summary>The initial connection attempt to the pcap-service has succeeded.</summary>
        private void ipcClient_onConnect(IpcClient c, bool reconnect)
        {
			whineAboutClose = true;
            log.Write(TraceLevel.Info, "Connected to pcap-service successfully");
            state = IPCState.Connected;
            SendConfigData();
        }

		private bool whineAboutClose = true;

        /// <summary>The connection to the pcap-service has been closed.</summary>
        private void ipcClient_onConnectionClosed(IpcClient c, Exception e)
        {
			if (whineAboutClose)
			{
				log.Write(TraceLevel.Warning, "Lost connection to pcap-service service: {0}",
					e != null ? e.Message : "(no msg)");
				whineAboutClose = false;
			}

            state = IPCState.Disconnected;

            if(onServiceDown != null)
                onServiceDown();

			if (active)
				state = IPCState.Connecting;
        }

        /// <summary>A message has been received from pcap-service</summary>
        /// <param name="messageType">Type of message received.</param>
        /// <param name="flatmap">Flatmap containing message parameters.</param>
        private void ipcClient_OnMessageReceived(IpcFlatmapClient client, int messageType, FlatmapList flatmap)
        {
            switch((uint)messageType)
            {
                case Messages.ACTIVE_CALL_LIST:
                    HandleActiveCallListMessage(flatmap);
                    break;

                case Messages.MONITOR_CALL_ACK:
                    HandleMonitorCallRequestAckMessage(flatmap);
                    break;

                case Messages.STOP_MONITOR_CALL_ACK:
                    HandleStopMonitorCallRequestAckMessage(flatmap);
                    break;

                default:
                    break;
            }
        }

        #endregion //IPC Client Callbacks

        #region Message Senders
        // Request list of active calls
        public bool SendRequestActiveCallListMessage(uint transactionId)
        {
            FlatmapList flatmap = new FlatmapList();
            flatmap.Add(Params.TRANSACTION_ID, transactionId);
            return this.ipcClient.Write(Messages.ACTIVE_CALL_LIST, flatmap);
        }

        // Request to monitor an active call
        // Provider needs to provide:
        // 1) Call identifier for selected call
        // 2) Media Server IP address
        // 3) Media Server RTP receiving port 1
        // 4) Media Server RTP receiving port 2
        public bool SendMonitorCallRequestMessage(uint transactionId, uint callIdentifier, string mmsIpAddress, uint mmsPort1, uint mmsPort2)
        {
            FlatmapList flatmap = new FlatmapList();
            flatmap.Add(Params.IDENTIFIER, callIdentifier);         // selected call identifier
            flatmap.Add(Params.MONITORED_IP, mmsIpAddress);         // media server ip
            flatmap.Add(Params.MONITORED_PORT, mmsPort1);           // media server Rx port for RTP stream 1
            flatmap.Add(Params.MONITORED_PORT, mmsPort2);           // media server Rx port for RTP stream 2
            flatmap.Add(Params.TRANSACTION_ID, transactionId);
            return this.ipcClient.Write(Messages.MONITOR_CALL, flatmap);
        }

        // Request to stop monitoring a call
        // Provider needs to provide:
        // 1) Call identifier for monitored call
        public bool SendStopMonitorCallRequestMessage(uint callIdentifier)
        {
            FlatmapList flatmap = new FlatmapList();
            flatmap.Add(Params.IDENTIFIER, callIdentifier);         // call identifier
            return this.ipcClient.Write(Messages.STOP_MONITOR_CALL, flatmap);
        }

        // Send config data to pcap-service
        // Provider needs to provide:
        // 1) Portbase LWM
        // 2) Portbase HWM
        // 3) Portbase Steps
        public bool SendConfigData()
        {
            FlatmapList flatmap = new FlatmapList();            
            flatmap.Add(Params.PORTBASE_LWM, portbaseLWM);
            flatmap.Add(Params.PORTBASE_HWM, portbaseHWM);
            flatmap.Add(Params.PORT_STEP, portStep);
            flatmap.Add(Params.MAX_MONITOR_TIME, maxMonitorTime);
            flatmap.Add(Params.LOG_LEVEL, logLevel);

            return this.ipcClient.Write(Messages.CONFIG_DATA, flatmap);
        }
        #endregion // Message Senders

        #region Message Handlers
        // Received active call list from pcap-service
        private void HandleActiveCallListMessage(FlatmapList flatmap)
        {
            // pcap-service sends back
            // 1) Transaction ID
            // 2) Number of active calls
            // 3) For each active call, there is call data in the following format:
            //      call_identifier&dn@ip

            uint transactionId = 0;
            if(flatmap.Contains(Params.TRANSACTION_ID))
                transactionId = Convert.ToUInt32(flatmap.Find(Params.TRANSACTION_ID, 1).dataValue);

            uint numEntries = 0;
            if(flatmap.Contains(Params.NUM_ENTRIES))
                numEntries = Convert.ToUInt32(flatmap.Find(Params.NUM_ENTRIES, 1).dataValue);

            log.Write(TraceLevel.Info, "Number of active calls is {0}", numEntries);

            Hashtable calls = new Hashtable();

            for (int i=1; i<=numEntries; i++)
            {
                string callData = null;
                callData = flatmap.Find(Params.CALL_DATA, i).dataValue as String;

                string [] data = callData.Split(new char[] {'&'}, 2);
                if (data != null && data.Length == 2)
                {
                    uint callIdentifier = Convert.ToUInt32(data[0].Trim());

                    string dn_ip = data[1].Trim(); 

                    if (dn_ip != null && dn_ip.Length > 0)
                    {
                        if (!calls.ContainsKey(dn_ip))
                            calls.Add(dn_ip, callIdentifier);
                    }
            
                    log.Write(TraceLevel.Verbose, "Active Call [{0}] is {1}, DN = {2}", i, callIdentifier, dn_ip);
                }
            }

            if (this.onActiveCallList != null)
                this.onActiveCallList(transactionId, calls);
        }

        // Received monitor call request ACK from pcap-service
        private void HandleMonitorCallRequestAckMessage(FlatmapList flatmap)
        {
            // pcap-service sends back
            // 1) result code, 0 = SUCCESS, 1= FAILURE
            // 2) Tx port for RTP stream 1
            // 3) Tx port for RTP stream 2
            // 4) Call identifier
            // 5) transaction id
            // 6) Call Type, 1 = inbound, 2 = outbound
            // 7) Caller DN
            // 8) Callee DN
            // 9) Monitored Phone IP
            uint resultCode = 1;
            if(flatmap.Contains(Params.RESULT_CODE))
                resultCode = Convert.ToUInt32(flatmap.Find(Params.RESULT_CODE, 1).dataValue);

            uint pcapPort1 = 0;
            if(flatmap.Contains(Params.MONITORED_PORT))
                pcapPort1 = Convert.ToUInt32(flatmap.Find(Params.MONITORED_PORT, 1).dataValue);

            uint pcapPort2 = 0;
            if(flatmap.Contains(Params.MONITORED_PORT))
                pcapPort2 = Convert.ToUInt32(flatmap.Find(Params.MONITORED_PORT, 2).dataValue);

            uint callIdentifier = 0;
            if(flatmap.Contains(Params.IDENTIFIER))
                callIdentifier = Convert.ToUInt32(flatmap.Find(Params.IDENTIFIER, 1).dataValue);

            uint transactionId = 0;
            if(flatmap.Contains(Params.TRANSACTION_ID))
                transactionId = Convert.ToUInt32(flatmap.Find(Params.TRANSACTION_ID, 1).dataValue);

            uint callType = 0;
            if(flatmap.Contains(Params.CALL_TYPE))
                callType = Convert.ToUInt32(flatmap.Find(Params.CALL_TYPE, 1).dataValue);

            string callerDN = null;
            callerDN = flatmap.Find(Params.CALLER_DN, 1).dataValue as String;

            string calleeDN = null;
            calleeDN = flatmap.Find(Params.CALLEE_DN, 1).dataValue as String;

            string phoneIp = null;
            phoneIp = flatmap.Find(Params.PHONE_IP, 1).dataValue as String;

            log.Write(TraceLevel.Info, "Monitor call {0} returns {1}, RTP Port1 = {2}, RTP Port2 = {3}, Call Type = {4}, Caller DN = {5}, Callee DN = {6}, Phone IP = {7}", 
                        callIdentifier, resultCode, pcapPort1, pcapPort2, callType, callerDN, calleeDN, phoneIp);            

            if (this.onMonitorCallRequestAck != null)
            {
                MonitoredCallData data = new MonitoredCallData();
                data.ResultCode = resultCode;
                data.RtpPort1 = pcapPort1;
                data.RtpPort2 = pcapPort2;
                data.CallType = callType;
                data.CallerDN = callerDN;
                data.CalleeDN = calleeDN;
                data.PhoneIp = phoneIp;
                this.onMonitorCallRequestAck(transactionId, data);
            }
        }

        // Received stop monitor call request ACK from pcap-service
        private void HandleStopMonitorCallRequestAckMessage(FlatmapList flatmap)
        {
            // pcap-service sends back
            // 1) result code, 0 = SUCCESS, 1= FAILURE
            uint resultCode = 1;
            if(flatmap.Contains(Params.RESULT_CODE))
                resultCode = Convert.ToUInt32(flatmap.Find(Params.RESULT_CODE, 1).dataValue);

            uint callIdentifier = 0;
            if(flatmap.Contains(Params.IDENTIFIER))
                callIdentifier = Convert.ToUInt32(flatmap.Find(Params.IDENTIFIER, 1).dataValue);

            log.Write(TraceLevel.Info, "Stop monitor call {0} returns {1}", callIdentifier, resultCode);
            
            if (this.onStopMonitorCallRequestAck != null)
                this.onStopMonitorCallRequestAck(resultCode, callIdentifier);
        }
        #endregion // Message Handlers
    }
}
