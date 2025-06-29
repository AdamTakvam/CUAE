using System;
using System.Net;
using System.Collections;

namespace Metreos.RecordAgent
{
	/// <summary>
	/// Summary description for Agent.
	/// </summary>
	public class Agent
	{
        public delegate void OnNewCallReceived(uint callIdentifier, ActiveCall call);
        public delegate void OnCallStatusUpdated(uint callIdentifier, ActiveCall call);
        public delegate void OnRecordFromNotifierDelegate(uint callIdentiier);

        #region Constants
        private abstract class Consts
        {
            public const string IPC_SERVER       = "127.0.0.1";
            public const int IPC_PORT            = 8510;
        }
        #endregion

        private RecordAgentIpcClient ipcClient;         // IPC interface to communicate with pcap-service
        public RecordAgentIpcClient IpcClient { get { return ipcClient; } }
        private NotifierManager notifierManager;        // New call notify window manager
        static readonly object ipclock = new object();
        private Hashtable activeCalls;

        public event OnNewCallReceived onNewCallReceived;
        public event OnCallStatusUpdated onCallStatusUpdated;
        public event OnRecordFromNotifierDelegate onRecordFromNotifier;

        /// <summary>
        /// Constructor
        /// </summary>
		public Agent()
		{
            activeCalls = Hashtable.Synchronized(new Hashtable());
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(Consts.IPC_SERVER), Consts.IPC_PORT); 
            ipcClient = new RecordAgentIpcClient(ipe);
            //ipcClient.onServiceDown += new NullDelegate(OnServiceDown);
            ipcClient.onStartRecordAck += new OnStartRecordAckDelegate(OnStartRecordAck);
            ipcClient.onStopRecordAck += new OnStopRecordAckDelegate(OnStopRecordAck);
            ipcClient.onHeartbeat += new OnHeartbeatDelegate(OnHeartbeat);
            ipcClient.onCallRemoved += new OnCallRemovedDelegate(OnCallRemoved);

            notifierManager = NotifierManager.Instance;

            notifierManager.onStartRecord += new OnStartRecordDelegate(OnNotifierStartRecord);
            notifierManager.onStartRecordNow += new OnStartRecordNowDelegate(OnNotifierStartRecordNow);
        }

        #region Agent management
        /// <summary>
        /// Start Agent tasks
        /// </summary>
        public void Start()
        {
            StartIpcClient();
        }

        /// <summary>
        /// Stop agent tasks
        /// </summary>
        public void Stop()
        {
            if (ipcClient.State == IPCState.Connected)
                ipcClient.Close();

            if (activeCalls.Count > 0)
                activeCalls.Clear();
        }

        /// <summary>
        /// Refresh record configuration
        /// </summary>
        public void RefreshConfiguration()
        {
            StartIpcClient();
        }

        /// <summary>
        /// Helper function to manage IPC interface
        /// </summary>
        private void StartIpcClient()
        {
            if (ipcClient.State == IPCState.Disconnected)
            {
                IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(Consts.IPC_SERVER), Consts.IPC_PORT); 
                ipcClient.Startup();
            }
            else if (ipcClient.State == IPCState.Connected)
            {
                ipcClient.SendRecordConfigData();
            }
        }
        #endregion

        #region Event handlers
        public void OnCallStatusUpdate(uint callIdentifier, CallData callData)
        {
            if (!activeCalls.ContainsKey(callIdentifier))
                return;

            ActiveCall ac = null;
            lock(activeCalls.SyncRoot)
            {
                ac = activeCalls[callIdentifier] as ActiveCall;
                if (ac != null)
                    ac.UpdateCallStatus(callData, true);
            }

            if (onCallStatusUpdated != null)
                onCallStatusUpdated(callIdentifier, ac);
        }

        public void OnNewCall(uint callIdentifier, CallData callData)
        {
            ActiveCall ac = new ActiveCall(callData);
            lock(activeCalls.SyncRoot)
            {
                activeCalls.Add(callIdentifier, ac);
            }

            if (callData.CallType == CallType.INBOUND_CALL)
                notifierManager.AddNotifier(callIdentifier, true, callData.CallerName, callData.CallerDN, -1, -1);
            else if (callData.CallType == CallType.OUTBOUND_CALL)
                notifierManager.AddNotifier(callIdentifier, false, callData.CalleeName, callData.CalleeDN, -1, -1);

            if (onNewCallReceived != null)
                onNewCallReceived(callIdentifier, ac);
        }

        private void OnStartRecordAck(uint callIdentifier, uint resultCode)
        {
            if (resultCode != ResultCodes.SUCCESS)
                return;

            if (!activeCalls.ContainsKey(callIdentifier))
                return;

            lock(activeCalls.SyncRoot)
            {
                ActiveCall ac = activeCalls[callIdentifier] as ActiveCall;
                if (ac != null)
                    ac.StartRecording();
            }           
        }

        private void OnStopRecordAck(uint callIdentifier, uint resultCode, string fileName)
        {
            if (resultCode != ResultCodes.SUCCESS)
                return;

            if (!activeCalls.ContainsKey(callIdentifier))
                return;

            lock(activeCalls.SyncRoot)
            {
                ActiveCall ac = activeCalls[callIdentifier] as ActiveCall;
                if (ac != null)
                {
                    ac.AddAudioFile(fileName);
                    ac.StopRecording();
                }
            }           
        }

        private void OnCallRemoved(uint callIdentifier, string fileName)
        {
            lock(activeCalls.SyncRoot)
            {
                ActiveCall ac = activeCalls[callIdentifier] as ActiveCall;
                if (ac != null)
                {
                    ac.AddAudioFile(fileName);
                    ac.StopRecording();
                }
            }           
        }

        public void OnStartRecord(uint callIdentifier)
        {
            lock (ipclock)
            {
                ipcClient.SendStartRecord(callIdentifier);
            }
        }

        public void OnStartRecordNow(uint callIdentifier)
        {
            lock (ipclock)
            {
                ipcClient.SendStartRecordNow(callIdentifier);
            }
        }

        public void OnNotifierStartRecord(uint callIdentifier)
        {
            lock (ipclock)
            {
                ipcClient.SendStartRecord(callIdentifier);
            }

            if (this.onRecordFromNotifier != null)
                onRecordFromNotifier(callIdentifier);
        }

        public void OnNotifierStartRecordNow(uint callIdentifier)
        {
            lock (ipclock)
            {
                ipcClient.SendStartRecordNow(callIdentifier);
            }

            if (this.onRecordFromNotifier != null)
                onRecordFromNotifier(callIdentifier);
        }

        public void OnStopRecord(uint callIdentifier)
        {
            lock (ipclock)
            {
                ipcClient.SendStopRecord(callIdentifier);
            }
        }

        /*
        private void OnServiceDown()
        {

        }

        private void OnServiceUp()
        {

        }
        */

        private void OnHeartbeat()
        {

        }

        public void RemoveCall(uint callIdentifier)
        {
            lock(activeCalls.SyncRoot)
            {
                ActiveCall ac = activeCalls[callIdentifier] as ActiveCall;
                if (ac != null)
                {
                    ac.CreateRecord();
                    activeCalls.Remove(callIdentifier);
                }
            }           
        }

        public bool IsRecording(uint callIdentifier)
        {
            bool bIsRecording = false;
            lock(activeCalls.SyncRoot)
            {
                ActiveCall ac = activeCalls[callIdentifier] as ActiveCall;
                if (ac != null)
                {
                    bIsRecording = ac.IsRecording;
                }
            }           

            return bIsRecording;
        }
        #endregion
    }
}
