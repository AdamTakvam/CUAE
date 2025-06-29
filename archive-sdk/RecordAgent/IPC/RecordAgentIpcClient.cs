using System;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Collections;

using Metreos.Core.IPC;
using Metreos.Core.IPC.Flatmaps;

namespace Metreos.RecordAgent
{
    public enum IPCState
    {
        Disconnected = 0,
        Connecting,
        Connected
    }

    public delegate void OnNewCallDelegate(uint callIdentifier, CallData callData);
    public delegate void OnCallStatusUpdateDelegate(uint callIdentifier, CallData callData);
    public delegate void OnStartRecordAckDelegate(uint callIdentifier, uint resultCode);
    public delegate void OnStopRecordAckDelegate(uint callIdentifier, uint resultCode, string fileName);
    public delegate void OnCallRemovedDelegate(uint callIdentifier, string fileName);
    public delegate void OnHeartbeatDelegate();

    public delegate void NullDelegate();

    /// <summary>Client class for communicating with the PCapService</summary>
    public class RecordAgentIpcClient
    {
        private IpcFlatmapClient ipcClient;

        public event NullDelegate onServiceDown;
        public event NullDelegate onServiceUp;
        public event OnNewCallDelegate onNewCall;
        public event OnCallStatusUpdateDelegate onCallStatusUpdate;
        public event OnStartRecordAckDelegate onStartRecordAck;
        public event OnStopRecordAckDelegate onStopRecordAck;
        public event OnCallRemovedDelegate onCallRemoved;
        public event OnHeartbeatDelegate onHeartbeat;

        private IPCState state = IPCState.Disconnected;
        public IPCState State { get { return state; } }

        #region Construction/Open/Close
        /// <summary>Default constructor</summary>
        public RecordAgentIpcClient(IPEndPoint ipEndpoint)
        {
            ipcClient = new IpcFlatmapClient(ipEndpoint);
            ipcClient.onClose +=new OnCloseDelegate(ipcClient_onConnectionClosed);
            ipcClient.onConnect += new OnConnectDelegate(ipcClient_onReconnect);
            ipcClient.onFlatmapMessageReceived += new OnFlatmapMessageReceivedDelegate(ipcClient_OnMessageReceived);
        }

        /// <summary>Open an IPC connection to the pcap-service.</summary>
        /// <param name="ipEndpoint">Address of the pcap-service.</param>
        /// <returns>True if connected, false otherwise.</returns>
        public void Startup()
        {
            if(ipcClient == null)
                throw new ObjectDisposedException(typeof(RecordAgentIpcClient).Name);

            state = IPCState.Connecting;

            if(ThreadPool.QueueUserWorkItem(new WaitCallback(ConnectThread), null) == false)
                throw new Metreos.Core.StartupFailedException("Could not start RecordAgentIpcClient connect thread");
        }

        private void ConnectThread(object threadState)
        {
            while(state == IPCState.Connecting && ipcClient != null)
            {
                if(ipcClient.Open())
                {
                    state = IPCState.Connected;
                    SendRecordConfigData();

                    if(onServiceUp != null)
                        onServiceUp();
                }
                else
                {
                    if(onServiceDown != null)
                        onServiceDown();
                }
            }
        }

        /// <summary>Close the IPC connection to the pcap-service.</summary>
        public void Close()
        {
            ipcClient.Close();
            ipcClient.Dispose();

            state = IPCState.Disconnected;
        }
        #endregion // Construction/Open/Close

        #region IPC Client Callbacks
        /// <summary>The connection to the pcap-service has been closed.</summary>
        private void ipcClient_onConnectionClosed(IpcClient c, Exception e)
        {
            state = IPCState.Disconnected;
            if(onServiceDown != null)
                onServiceDown();
        }


        /// <summary>A message has been received from pcap-service</summary>
        /// <param name="messageType">Type of message received.</param>
        /// <param name="flatmap">Flatmap containing message parameters.</param>
        private void ipcClient_OnMessageReceived(IpcFlatmapClient ipcClient, int messageType, FlatmapList flatmap)
        {
            switch((uint)messageType)
            {
                case Messages.NEW_CALL:
                    HandleNewCallMessage(flatmap);
                    break;

                case Messages.CALL_STATUS_UPDATE:
                    HandleCallStatusUpdateMessage(flatmap);
                    break;

                case Messages.HEART_BEAT:
                    HandleHeartbeatMessage(flatmap);
                    break;

                case Messages.START_RECORD_ACK:
                    HandleStartRecordAckMessage(flatmap);
                    break;

                case Messages.STOP_RECORD_ACK:
                    HandleStopRecordAckMessage(flatmap);
                    break;

                case Messages.CALL_REMOVED:
                    HandleCallRemovedMessage(flatmap);
                    break;

                default:
                    break;
            }
        }

        /// <summary>The connection to the pcap-service has been restored.</summary>
        private void ipcClient_onReconnect(IpcClient c, bool reconnect)
        {
            if (reconnect)
            {
                SendRecordConfigData();
                if(onServiceUp != null)
                    onServiceUp();
            }
        }
        #endregion //IPC Client Callbacks

        #region Message Senders
        // Request to start recording a call from the beginning of the call
        // Agent needs to provide:
        // 1) Call identifier for recording call
        public bool SendStartRecord(uint callIdentifier)
        {
            FlatmapList flatmap = new FlatmapList();
            flatmap.Add(Params.IDENTIFIER, callIdentifier);         // call identifier
            return this.ipcClient.Write(Messages.START_RECORD, flatmap);
        }

        // Request to start recording a call from now
        // Agent needs to provide:
        // 1) Call identifier for recording call
        public bool SendStartRecordNow(uint callIdentifier)
        {
            FlatmapList flatmap = new FlatmapList();
            flatmap.Add(Params.IDENTIFIER, callIdentifier);         // call identifier
            return this.ipcClient.Write(Messages.START_RECORD_NOW, flatmap);
        }

        // Request to stop recording a call
        // Agent needs to provide:
        // 1) Call identifier for recording call
        public bool SendStopRecord(uint callIdentifier)
        {
            FlatmapList flatmap = new FlatmapList();
            flatmap.Add(Params.IDENTIFIER, callIdentifier);         // call identifier
            return this.ipcClient.Write(Messages.STOP_RECORD, flatmap);
        }

        // Send record config data to pcap-service
        public bool SendRecordConfigData()
        {
            return true;
        }

        // Send heartbeat to pcap-service
        public bool SendHeartbeat()
        {
            return true;
        }
        #endregion // Message Senders

        #region Message Handlers
        // Received new call notification from pcap-service
        private void HandleNewCallMessage(FlatmapList flatmap)
        {
            // pcap-service sends back
            // * Call identifier
            // * Call Type, 1 = inbound, 2 = outbound
            // * Caller DN, Name
            // * Callee DN, Name 
               
            uint callIdentifier = 0;
            if(flatmap.Contains(Params.IDENTIFIER))
                callIdentifier = Convert.ToUInt32(flatmap.Find(Params.IDENTIFIER, 1).dataValue);

            uint callType = 0;
            if(flatmap.Contains(Params.CALL_TYPE))
                callType = Convert.ToUInt32(flatmap.Find(Params.CALL_TYPE, 1).dataValue);

            // call state is connected for a new call 

            string callerDN = null;
            callerDN = flatmap.Find(Params.CALLER_DN, 1).dataValue as String;

            string calleeDN = null;
            calleeDN = flatmap.Find(Params.CALLEE_DN, 1).dataValue as String;

            string callerName = null;
            callerName = flatmap.Find(Params.CALLER_NAME, 1).dataValue as String;

            string calleeName = null;
            calleeName = flatmap.Find(Params.CALLEE_NAME, 1).dataValue as String;

            if (this.onNewCall != null)
            {
                CallData callData = new CallData(callIdentifier, CallState.CONNECTED, callType, callerDN, callerName, calleeDN, calleeName);
                onNewCall(callIdentifier, callData);
            }
        }

        // Received call status update from pcap-service
        private void HandleCallStatusUpdateMessage(FlatmapList flatmap)
        {
            // pcap-service sends back
            // * Call identifier
            // * Call Type, 1 = inbound, 2 = outbound
            // * Call state
            // * Caller DN, Name
            // * Callee DN, Name 
               
            uint callIdentifier = 0;
            if(flatmap.Contains(Params.IDENTIFIER))
                callIdentifier = Convert.ToUInt32(flatmap.Find(Params.IDENTIFIER, 1).dataValue);

            uint callType = 0;
            if(flatmap.Contains(Params.CALL_TYPE))
                callType = Convert.ToUInt32(flatmap.Find(Params.CALL_TYPE, 1).dataValue);

            uint callState = 0;
            if(flatmap.Contains(Params.CALL_STATE))
                callState = Convert.ToUInt32(flatmap.Find(Params.CALL_STATE, 1).dataValue);

            string callerDN = null;
            callerDN = flatmap.Find(Params.CALLER_DN, 1).dataValue as String;

            string calleeDN = null;
            calleeDN = flatmap.Find(Params.CALLEE_DN, 1).dataValue as String;

            string callerName = null;
            callerName = flatmap.Find(Params.CALLER_NAME, 1).dataValue as String;

            string calleeName = null;
            calleeName = flatmap.Find(Params.CALLEE_NAME, 1).dataValue as String;

            if (this.onCallStatusUpdate != null)
            {
                CallData callData = new CallData(callIdentifier, callState, callType, callerDN, callerName, calleeDN, calleeName);
                onCallStatusUpdate(callIdentifier, callData);
            }
        }

        private void HandleStartRecordAckMessage(FlatmapList flatmap)
        {
            uint resultCode = 0;
            if(flatmap.Contains(Params.RESULT_CODE))
                resultCode = Convert.ToUInt32(flatmap.Find(Params.RESULT_CODE, 1).dataValue);

            uint callIdentifier = 0;
            if(flatmap.Contains(Params.IDENTIFIER))
                callIdentifier = Convert.ToUInt32(flatmap.Find(Params.IDENTIFIER, 1).dataValue);

            if (this.onStartRecordAck != null)
                onStartRecordAck(callIdentifier, resultCode);
        }

        private void HandleStopRecordAckMessage(FlatmapList flatmap)
        {
            uint resultCode = 0;
            if(flatmap.Contains(Params.RESULT_CODE))
                resultCode = Convert.ToUInt32(flatmap.Find(Params.RESULT_CODE, 1).dataValue);

            uint callIdentifier = 0;
            if(flatmap.Contains(Params.IDENTIFIER))
                callIdentifier = Convert.ToUInt32(flatmap.Find(Params.IDENTIFIER, 1).dataValue);

            string fileName = null;
            fileName = flatmap.Find(Params.RECORD_FILE, 1).dataValue as String;

            if (this.onStopRecordAck != null)
                onStopRecordAck(callIdentifier, resultCode, fileName);
        }

        private void HandleCallRemovedMessage(FlatmapList flatmap)
        {
            uint callIdentifier = 0;
            if(flatmap.Contains(Params.IDENTIFIER))
                callIdentifier = Convert.ToUInt32(flatmap.Find(Params.IDENTIFIER, 1).dataValue);

            string fileName = null;
            fileName = flatmap.Find(Params.RECORD_FILE, 1).dataValue as String;

            if (this.onCallRemoved != null)
                onCallRemoved(callIdentifier, fileName);
        }

        private void HandleHeartbeatMessage(FlatmapList flatmap)
        {

        }
        #endregion // Message Handlers
    }
}
