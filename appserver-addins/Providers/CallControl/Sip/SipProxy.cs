using System;
using System.Text;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.IO;

using Metreos.Core.IPC;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Core.Sockets;
using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;
using Metreos.Core.IPC.Flatmaps;
using Metreos.Messaging.MediaCaps;

using Utils=Metreos.Utilities;

namespace Metreos.CallControl.Sip
{
    public delegate MediaCapsField GetCodecsDelegate();
    public delegate Core.ConfigData.SipDeviceInfo[] GetDevicesDelegate();

    public delegate void VoidDelegate();
    public delegate void OnErrorDelegate(string dn, long cid, SipProxy.FailReason reason, string message, 
        SipProxy.MsgType msgType, SipProxy.MsgField msgField);
    public delegate void OnStatusUpdateDelegate(string dn, string device, IConfig.Status status);
    public delegate void OnCallInitiatedDelegate(long cid, string dn, string to, string from);
    public delegate void OnCallInactiveDelegate(long cid, string stackCallId, bool inUse);
    public delegate void OnCallEstablishedDelegate(long cid);
    public delegate void OnMediaEstablishedDelegate(long cid, IMediaControl.Codecs codec, uint framesize, string txIP, ushort txPort);
	public delegate void OnCallAnsweredDelegate(long cid, string stackCallId, string from, string to, string originalTo, string txIp, int txPort, MediaCapsField caps);
	public delegate void OnReInviteDelegate(long cid, string txIp, int txPort, MediaCapsField caps, bool mediaActive, bool isAnswer);
	public delegate void OnIncomingCallDelegate(string stackCallId, string dn, string to, string from, string originalTo, string txIp, int txPort, MediaCapsField caps);
    public delegate void OnHangupDelegate(long cid, string stackCallId, string cause);
    public delegate void CallNoticeDelegate(long cid, string stackCallId);
    public delegate void OnGotDigitsDelegate(long cid, string digits, uint source);
	public delegate void OnGotCapabilitiesDelegate(long cid, string txIp, int txPort, MediaCapsField caps);
	public delegate void OnRequestDirectoryNumberDelegate(SipDeviceInfo di);
	public delegate void OnResetDirectoryNumberDelegate(string dn);

	/// <summary>Flatmap IPC abstraction layer</summary>
	public class SipProxy : IDisposable
	{
		private delegate void ProcessMessageDelegate(int messageType, FlatmapList flatmap);

		#region Constants

		private abstract class Consts
		{
			public const int IpcWriteTimeout    = 5;    // seconds
			public const int IpcConnectTimeout  = 120;  // seconds
			public const int NumThreads         = 3;
			public const int MaxThreads         = 10;
			public const string PoolName        = "Sip ThreadPool";
			public const string ConnectorThreadName = "Sip service connect thread";
			public const int MinRegistrationPort	= 1024;
			public const int MaxRegistrationPort	= 65535;
			public const int SipTrunkPort			= 6050;
		}

		#endregion

		#region Message definitions



		public enum MsgType
		{
			Error				= 0,    // Causes message loop to exit
			Quit                = 1,    // Causes message loop to exit
			InternalInit        = 2,    // 1-15 should not be user-handled 
			Ping                = 16,   // Generic thread ping
			PingBack            = 17,   // Generic thread acknowledge
			Start               = 18,   // Start the runtime
			Stop                = 19,   // Stop the runtime

			StartStack			= 129,  // IPC: Start the stack
			StopStack			= 130,  // IPC: Stop the stack
			StartStackAck		= 131,
			StopStackAck		= 132,
      
			//messages from the stack
			IncomingCall		= 140,
			CallEstablished		= 141,
			CallCleared			= 142,
			MediaEstablished    = 143,
			GotDigits			= 144,
			GotCapabilities     = 145,
			MediaChanged        = 146,
			MakeCallAck         = 148,
			StatusUpdate		= 149,
			Answered			= 150,
			ReInvite			= 151,
			ReInviteAnswer		= 152,

			//messages from the application server
			Accept				= 200,
			Answer              = 201,
			SetMedia            = 202,
			Hangup              = 203,
			MakeCall            = 204,
			SendUserInput       = 205,
			RegisterDevices     = 206,	
			UnregisterDevices   = 207,	

			CallInUse			= 208,	
			CallHeld			= 209,
			Hold				= 210,
			Resume				= 211,
			UseMohMedia			= 212,
			Redirect			= 213,
			BlindTransfer		= 214,
			Conference			= 215,
			Reject				= 216,
			ResetDirectoryNumber= 217,
			ParameterChanged	= 218
		}

		public enum MsgField
		{
			ResultCode          = 0,
			ResultMsg			= 1,	
			CallId				= 2,
			DeviceName          = 3,
			DeviceType			= 4,
			MessageType         = 5,
			StackCallId			= 6,
			TxIp                = 7,	
			TxPort              = 8,
			TxCodec				= 9,
			TxFrameSize			= 10,
			RxIp                = 11,
			RxPort              = 12,
			RxCodec				= 13,
			RxFramesize			= 14,
			Digits				= 15,
			DisplayName			= 16,
			CallEndReason		= 17,
			MediaCaps			= 18,
			Direction			= 19,
			From				= 20,
			To					= 21,
			UserName			= 22,
			Password			= 23,
			MaxPendingCalls		= 24,
			EnableDebug			= 25,
			DebugLevel			= 26,
			DebugFilename		= 27,
			ListenPort			= 28,
			TransactionId		= 29,
			OriginalTo			= 30,
			Registrars			= 31,
			ProxyServer			= 32,
			DomainName			= 33,
			Status				= 34,
			MinRegistrationPort	= 35,
			MaxRegistrationPort	= 36,
			DefaultFrom			= 37,
			MediaOption			= 38,
			SipTrunkIp			= 39,
			SipTrunkPort		= 40,
			ServiceLogLevel		= 41,
			DirectoryNumber		= 42,
			MediaActive			= 43,
			LogTimingStat		= 44
		}

		public enum FailReason 
		{
			InvalidDeviceName	= 2,
			InvalidDeviceType   = 3,
			UnknownMessageType  = 4,
			InvalidDN           = 6,
			NoProvider          = 7,
			GeneralFailure      = 9,
			CallIdUnknown       = 10,
			CodecNotSupported   = 12,
			MissingField        = 13
		}

		public enum Status 
		{
			DeviceUnregistered		= 0,
			DeviceRegistered		= 1,
			DeviceFailedToRegister	= 2
		}

		public enum ResultCodes 
		{
			Success			= 0,
			Failure			= 1
		}

		public enum MediaOption
		{
			sendrecv		= 0,
			sendonly		= 1,
			recvonly		= 2
		}

		#endregion

		private LogWriter log;
		private IpcFlatmapClient ipcClient;
		private Utils.ThreadPool threadPool;

		private volatile bool connected = false;
		public bool Connected
		{
			get { lock(this) { return connected; } }
		}

		private volatile bool stackInitialized = false;

		private ManualResetEvent configDataReady = new ManualResetEvent(false);
		public ManualResetEvent ConfigDataReady
		{
			get { return configDataReady; }
		}

		//the default value to populate From field for outbound calls
		private string defaultFromOutboundNumber = null;
		public string DefaultFromOutboundNumber
		{
			get { return defaultFromOutboundNumber; }
			set { defaultFromOutboundNumber = value; }
		}

		//IP addr for trunk use. It should match the value in CCM.
		private string sipTrunkIp = null;
		public string SipTrunkIp
		{
			get { return sipTrunkIp; }
			set { sipTrunkIp = value; }
		}

		//Trunk port number. It should match the value in CCM.
		private int sipTrunkPort = Consts.SipTrunkPort;
		public int SipTrunkPort
		{
			get { return sipTrunkPort; }
			set { sipTrunkPort = value; }
		}

		//the port range for registering with SIP server
		private int minRegistartionPort = Consts.MinRegistrationPort;
		public int MinRegistrationPort
		{
			get { return minRegistartionPort; }
			set { minRegistartionPort = value; }
		}

		private int maxRegistartionPort = Consts.MaxRegistrationPort;
		public int MaxRegistrationPort
		{
			get { return maxRegistartionPort; }
			set { maxRegistartionPort = value; }
		}

		//Log level for sip stack
		private int serviceLogLevel;
		public int ServiceLogLevel
		{
			get { return serviceLogLevel; }
			set { serviceLogLevel = value; }
		}

		private bool logTimingStat;
		public bool LogTimgingStat
		{
			get { return logTimingStat; }
			set { logTimingStat = value; }
		}

        // Internal data delegates
        public VoidDelegate onServiceGone;
        public GetCodecsDelegate onGetCodecs;
        public GetDevicesDelegate onGetDevices;

        // Incoming message-handler delegates
        public OnErrorDelegate onError;
        public OnStatusUpdateDelegate onStatusUpdate;
        public OnIncomingCallDelegate onIncomingCall;
        public CallNoticeDelegate onRinging;
        public CallNoticeDelegate onMakeCallAck;
        public OnCallInactiveDelegate onCallInactive;
        public OnCallEstablishedDelegate onCallEstablished;
        public OnCallAnsweredDelegate onAnswered;
        public OnHangupDelegate onHangup;
        public OnMediaEstablishedDelegate onMediaEstablished;
        public OnCallInitiatedDelegate onCallInitiated;
        public OnGotDigitsDelegate onGotDigits;
		public OnGotCapabilitiesDelegate onGotCapabilities;
		public OnReInviteDelegate onReInvite;
		public OnRequestDirectoryNumberDelegate onRequestDirectoryNumber;
		public OnResetDirectoryNumberDelegate onResetDirectoryNumber;

        // ThreadPool callback delegates
        private ProcessMessageDelegate processMessageAsync;
        
        #region Construction/Startup/Shutdown/Cleanup

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="log">where the logs go</param>
		public SipProxy(LogWriter log)
		{
            Assertion.Check(log != null, "null log passed into SipProxy constructor");
            
            this.log = log;
			lock(this)
			{
				this.connected = false;
			}

            this.processMessageAsync = new ProcessMessageDelegate(ProcessMessageAsync);

            this.threadPool = new Utils.ThreadPool(Consts.NumThreads, Consts.MaxThreads, Consts.PoolName);
            this.threadPool.MessageLogged += new LogDelegate(log.Write);

            this.ipcClient = new IpcFlatmapClient();
			this.ipcClient.onConnect = new OnConnectDelegate(ipcClient_onConnect);
            this.ipcClient.onFlatmapMessageReceived = new OnFlatmapMessageReceivedDelegate(ipcClient_OnMessageReceived);
			this.ipcClient.onClose = new OnCloseDelegate(ipcClient_onConnectionClosed);
		}

		/// <summary>
		/// Proxy start up
		/// </summary>
		/// <param name="ipEndpoint">the other end of pipe</param>
		/// <returns></returns>
        public bool Startup(IPEndPoint ipEndpoint)
        {
            if(ipEndpoint == null || ipEndpoint.Address == null || ipEndpoint.Port == 0)
                return false;

            if(this.threadPool == null)
                throw new ObjectDisposedException(typeof(SipProxy).Name);

            if(this.threadPool.IsStarted == false)              
                this.threadPool.Start();

            if(ipcClient == null)
                throw new ObjectDisposedException(typeof(SipProxy).Name);

			ipcClient.RemoteEp = ipEndpoint;
			ipcClient.Start();
			return true;
        }

		/// <summary>
		/// It shuts down the proxy to stack and stops the thread pool.
		/// </summary>
        public void Shutdown()
        {
            if(threadPool != null)
                threadPool.Stop();

            if(ipcClient != null)
                ipcClient.Close();
        }

		/// <summary>
		/// It frees the resources.
		/// </summary>
        public void Dispose()
        {
            if(ipcClient != null)
            {
                ipcClient.Dispose();
                ipcClient = null;
            }

            if(threadPool != null)
            {
                threadPool.Stop();
                threadPool = null;
            }
        }

		/// <summary>
		/// It sends startup request to stack.
		/// </summary>
        private void InitializeService()
        {
            if(ipcClient == null) 
                return;

            Assertion.Check(this.onGetDevices != null, "onGetDevices delegate not hooked in Sip provider");
            
			SendStartStack();
        }

        #endregion

        #region IPC Client events

		/// <summary>
		/// Callback function for successful connection to stack
		/// </summary>
		/// <param name="c">the ipc connection</param>
		/// <param name="reconnect">indicates whethere its a reconnect or not</param>
        private void ipcClient_onConnect(IpcClient c, bool reconnect)
        {
            log.Write(TraceLevel.Info, "Connected to Sip service successfully");
			whineAboutClose = true;
			lock(this)
			{
				connected = true;
			}
			
			//wait for config data to be ready before sending data to service
			ConfigDataReady.WaitOne();
			InitializeService();
        }

		private bool whineAboutClose = true;

		/// <summary>
		/// Callback function for connection close to stack
		/// </summary>
		/// <param name="c">the connection</param>
		/// <param name="e">the exception generated by the close</param>
        private void ipcClient_onConnectionClosed(IpcClient c, Exception e)
        {
			if (whineAboutClose)
			{
				log.Write(TraceLevel.Warning, "Lost connection to Sip service: {0}",
					e != null ? e.Message : "(no msg)" );
				lock(this)
				{
					connected = false;
				}
				whineAboutClose = false;
			}

			if (onServiceGone != null)
	            onServiceGone();
        }

		/// <summary>
		/// Callback function for arrival of messages from stack
		/// </summary>
		/// <param name="client">the connection</param>
		/// <param name="messageType">type of the message arrived</param>
		/// <param name="flatmap">the message itself</param>
        private void ipcClient_OnMessageReceived(IpcFlatmapClient client, int messageType, FlatmapList flatmap)
        {
            log.Write(TraceLevel.Verbose, "Got {0}({1}) message.", messageType, ((MsgType)messageType).ToString());
            
            threadPool.PostRequest(processMessageAsync, new object[] { messageType, flatmap });
        }

		/// <summary>
		/// The main switch for the messages from stack
		/// </summary>
		/// <param name="messageType">type of the message to be proccessed</param>
		/// <param name="flatmap">the message itself</param>
        private void ProcessMessageAsync(int messageType, FlatmapList flatmap)
        {
			//dump out the message content for debugging 
            foreach(uint fieldKey in flatmap.Keys)
            {
                object fieldValueObj = flatmap.Find(fieldKey, 1).dataValue;
                string fieldValue = fieldValueObj == null ? "<null>" : fieldValueObj.ToString();
                log.Write(TraceLevel.Verbose, "Field: {0}({1}) = {2}", fieldKey, ((MsgField)fieldKey).ToString(), fieldValue);
            }

            switch((MsgType)messageType)
            {
                case MsgType.Error: 
                    ProcessError(flatmap);
                    break;

				case MsgType.StartStackAck:
					ProcessStartStackAck(flatmap);
					break;

                case MsgType.MakeCallAck:
                    ProcessMakeCallAck(flatmap);
                    break;

                case MsgType.StatusUpdate:
                    ProcessStatusUpdate(flatmap);
                    break;

 //               case MsgType.InitiatedCall:
 //                   ProcessCallInitiated(flatmap);
 //                   break;

                case MsgType.IncomingCall:
                    ProcessIncomingCall(flatmap);
                    break;

                case MsgType.CallEstablished:
                    ProcessCallEstablished(flatmap);
                    break;

                case MsgType.Answered:
                    ProcessAnswered(flatmap);
                    break;

                case MsgType.Hangup:
                    ProcessHangup(flatmap);
                    break;

                case MsgType.MediaEstablished:
                    ProcessMediaEstablished(flatmap);
                    break;

                case MsgType.CallHeld:
                    ProcessCallInactive(flatmap, false);
                    break;

//                case MsgType.CallInUse:
//                    ProcessCallInactive(flatmap, true);
//                    break;

                case MsgType.GotDigits:
                    ProcessGotDigits(flatmap);
                    break;

				case MsgType.GotCapabilities:
					ProcessGotCapabilities(flatmap);
					break;

				case MsgType.ReInvite:
					ProcessReInvite(flatmap, false);
					break;

				case MsgType.ReInviteAnswer:
					ProcessReInvite(flatmap, true);
					break;

				case MsgType.ResetDirectoryNumber:
					ProcessResetDN(flatmap);
					break;

//                case MsgType.Ringing:
//                    ProcessRinging(flatmap);
 //                   break;
                default:
                    log.Write(TraceLevel.Error, 
                        "Received unknown message type '{0}' from Sip service", messageType);
                    break;
            }
        }

		/// <summary>
		/// It handles error message from stack
		/// </summary>
		/// <param name="flatmap">the message</param>
        private void ProcessError(FlatmapList flatmap)
        {
            Assertion.Check(onError != null, "onError delegate not hooked in Sip proxy");
            
            string dn = null;
            try { dn= Convert.ToString(flatmap.Find((uint)MsgField.DirectoryNumber, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received failure message from Sip service with no directory number specified.");
                return;
            }

            int failReason = 0;

			string message = null;
			try
			{
				message = Convert.ToString(flatmap.Find((uint) MsgField.ResultMsg, 1).dataValue);
			}
			catch
			{
				log.Write(TraceLevel.Error, "Received failure message from Sip service with no reason specified.");
				return;
			}

            int msgType = 0;
            int msgField = 0;

            long cid = Convert.ToUInt32(flatmap.Find((uint)MsgField.CallId, 1).dataValue);
            onError(dn, cid, (FailReason)failReason, message, (MsgType)msgType, (MsgField)msgField);
        }

		/// <summary>
		/// It handles StartStack response from stack
		/// </summary>
		/// <param name="flatmap"></param>
		private void ProcessStartStackAck(FlatmapList flatmap)
		{
			Assertion.Check(onError != null, "onError delegate not hooked in Sip proxy");

			ResultCodes resultCode = ResultCodes.Failure;
			try
			{
				resultCode = (ResultCodes) Convert.ToInt32(flatmap.Find((uint)MsgField.ResultCode, 1).dataValue);
			}
			catch
			{
				log.Write(TraceLevel.Error, "Received StartStackAck message from Sip service with no ResultCode field.");
				return;
			}

			string message = null;
			if (resultCode == ResultCodes.Failure)	//something went wrong starting up
			{
				try
				{
					message = Convert.ToString(flatmap.Find((uint) MsgField.ResultMsg, 1).dataValue);
				}
				catch
				{
					log.Write(TraceLevel.Error, "Received failure message from Sip service with no reason specified.");
					return;
				}

				int msgType = 0;
				int msgField = 0;
				int failedReason = 0;

				onError("", 0, (FailReason)failedReason, message, (MsgType) msgType, (MsgField)msgField);
			}
			else //the stack started
			{
				//start processing
				ICollection devices = onGetDevices();
				if(devices != null && devices.Count > 0)
				{
					lock(devices)
					{
						foreach(SipDeviceInfo dInfo in devices)
						{
							//only register Ietf Sip device and the Cisco devices with DN
							if (dInfo.Type == IConfig.DeviceType.IetfSip ||
								(dInfo.Type == IConfig.DeviceType.CiscoSip &&
								dInfo.DirectoryNumber != null &&
								dInfo.DirectoryNumber.Length > 0 &&
								!dInfo.DirectoryNumber.Equals("0")) )	//dont register SipTrunk
							{
								if(SendRegister(dInfo) == false)
								{
									log.Write(TraceLevel.Error, "Failed to register device '{0}' with the Sip service",
										dInfo.Name);
								}
								else
								{
									log.Write(TraceLevel.Info, "Registering device '{0}' with the Sip service", dInfo.Name);
								}
							}
							//For those Cisco devices without DN, request DN from CCM
							else if (dInfo.Type == IConfig.DeviceType.CiscoSip &&
								(dInfo.DirectoryNumber == null || dInfo.DirectoryNumber.Length == 0 || dInfo.DirectoryNumber.Equals("0")))
							{
								//a cisco sip phone without dn, request it
								onRequestDirectoryNumber(dInfo);
							}//there is no need to register Sip trunk
							else if (dInfo.Type == IConfig.DeviceType.SipTrunk)	//simply mark it online
								onStatusUpdate(dInfo.Key, dInfo.Key, IConfig.Status.Enabled_Running);
						}
					}
				}
				else
				{
					log.Write(TraceLevel.Warning, "No SIP devices configured. SIP services are unavailable.");
				}

				this.stackInitialized = true;
			}
		}

		/// <summary>
		/// It handles MakeCall response from stack
		/// </summary>
		/// <param name="flatmap">the message content</param>
        private void ProcessMakeCallAck(FlatmapList flatmap)
        {
            Assertion.Check(onMakeCallAck != null, "onMakeCallAck delegate not hooked in Sip proxy");

            uint callId;
			try
			{
				callId = Convert.ToUInt32(flatmap.Find((uint)MsgField.CallId, 1).dataValue);
			}
			catch
			{
				log.Write(TraceLevel.Error, "Received MakeCallAck message from Sip service with no call ID specified.");
				return;
			}

			string stackCallId;
			try
			{
				stackCallId = Convert.ToString(flatmap.Find((uint)MsgField.StackCallId, 1).dataValue);
			}
			catch
			{
				log.Write(TraceLevel.Error, "Received MakeCallAck message from Sip service with no stack call ID specified.");
				return;
			}

            onMakeCallAck(callId, stackCallId);
        }

		/// <summary>
		/// It handles device registration status update from stack
		/// </summary>
		/// <param name="flatmap">message content</param>
        private void ProcessStatusUpdate(FlatmapList flatmap)
        {
            Assertion.Check(onStatusUpdate != null, "onStatusUpdate delegate not hooked in Sip proxy");

            Status SipStatus;
            try { SipStatus = (Status) Convert.ToUInt32(flatmap.Find((uint)MsgField.Status, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received UpdateStatus message from Sip service with no status specified.");
                return;
            }

            string dn = null;
            try { dn = Convert.ToString(flatmap.Find((uint)MsgField.DirectoryNumber, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received UpdateStatus message from Sip service with no directory number specified.");
                return;
            }

			string device = null;
			try
			{
				device = Convert.ToString(flatmap.Find((uint)MsgField.DeviceName, 1).dataValue);
			}
			catch
			{
				//from non-cisco sip server
				device = null;
			}

            IConfig.Status status = ConvertStatus(SipStatus);

            onStatusUpdate(dn, device, status);
        }

		/// <summary>
		/// It handles IncomingCall from stack
		/// </summary>
		/// <param name="flatmap">message content</param>
        private void ProcessIncomingCall(FlatmapList flatmap)
        {
            Assertion.Check(onIncomingCall != null, "onIncomingCall delegate not hooked in Sip proxy");

            string stackCallId, dn, to, from, originalTo;
			string txIp = null;
			int txPort = 0;

            try { stackCallId = Convert.ToString(flatmap.Find((uint)MsgField.StackCallId, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received IncomingCall message from Sip service with no stack call ID specified.");
                return;
            }

            try { dn = Convert.ToString(flatmap.Find((uint)MsgField.DirectoryNumber, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received IncomingCall message from Sip service with no directory number specified.");
                return;
            }

            try { to = Convert.ToString(flatmap.Find((uint)MsgField.To, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received IncomingCall message from Sip service with no destination address specified.");
                return;
            }

            try { from = Convert.ToString(flatmap.Find((uint)MsgField.From, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received IncomingCall message from Sip service with no source address specified.");
                return;
            }

            if(flatmap.Contains((uint)MsgField.OriginalTo))
                originalTo = Convert.ToString(flatmap.Find((uint)MsgField.OriginalTo, 1).dataValue);
            else
                originalTo = to;

			MediaCapsField caps = null;

			//check to see if the incomingCall request comes with media info
			if (flatmap.Contains((uint)MsgField.TxIp))
			{
				txIp = Convert.ToString(flatmap.Find((uint)MsgField.TxIp, 1).dataValue);

				try { txPort = Convert.ToInt32(flatmap.Find((uint)MsgField.TxPort, 1).dataValue); }
				catch
				{
					log.Write(TraceLevel.Error, "Received IncomingCall message from Sip service with no or bad TxPort specified.");
					return;
				}
				caps = new MediaCapsField();
				if (ReadMediaCapsFromFlatMap(flatmap, caps) <= 0)
					caps = null;
			}
            onIncomingCall(stackCallId, dn, to, from, originalTo, txIp, txPort, caps);
        }

		/// <summary>
		/// It handles ReInvite from stack
		/// </summary>
		/// <param name="flatmap">message content</param>
		/// <param name="isAnswer">true if this is an answer to an earlier reinvite</param>
		private void ProcessReInvite(FlatmapList flatmap, bool isAnswer)
		{
			string stackCallId, dn, to, from, originalTo;
			string txIp = null;
			int txPort = 0;
			long cid = 0;

			try { cid = Convert.ToUInt32(flatmap.Find((uint)MsgField.CallId, 1).dataValue); }
			catch
			{
				log.Write(TraceLevel.Error, "Received Answered message from Sip service with no call ID specified.");
				return;
			}

			try { stackCallId = Convert.ToString(flatmap.Find((uint)MsgField.StackCallId, 1).dataValue); }
			catch
			{
				log.Write(TraceLevel.Error, "Received IncomingCall message from Sip service with no stack call ID specified.");
				return;
			}

			try { dn = Convert.ToString(flatmap.Find((uint)MsgField.DirectoryNumber, 1).dataValue); }
			catch
			{
				log.Write(TraceLevel.Error, "Received IncomingCall message from Sip service with no directory number specified.");
				return;
			}

			try { to = Convert.ToString(flatmap.Find((uint)MsgField.To, 1).dataValue); }
			catch
			{
				log.Write(TraceLevel.Error, "Received IncomingCall message from Sip service with no destination address specified.");
				return;
			}

			try { from = Convert.ToString(flatmap.Find((uint)MsgField.From, 1).dataValue); }
			catch
			{
				log.Write(TraceLevel.Error, "Received IncomingCall message from Sip service with no source address specified.");
				return;
			}

			if(flatmap.Contains((uint)MsgField.OriginalTo))
				originalTo = Convert.ToString(flatmap.Find((uint)MsgField.OriginalTo, 1).dataValue);
			else
				originalTo = to;

			MediaCapsField caps = null;

			//check to see if the incomingCall request comes with media info
			if (flatmap.Contains((uint)MsgField.TxIp))
			{
				txIp = Convert.ToString(flatmap.Find((uint)MsgField.TxIp, 1).dataValue);

				try { txPort = Convert.ToInt32(flatmap.Find((uint)MsgField.TxPort, 1).dataValue); }
				catch
				{
					log.Write(TraceLevel.Error, "Received IncomingCall message from Sip service with no or bad TxPort specified.");
					return;
				}
				caps = new MediaCapsField();
				if (ReadMediaCapsFromFlatMap(flatmap, caps) <= 0)
					caps = null;
			}

			//mediaActive==false signals Hold
			int mediaActive = 1;
			if (flatmap.Contains((uint)MsgField.MediaActive))
			{
				mediaActive = Convert.ToInt32(flatmap.Find((uint)MsgField.MediaActive, 1).dataValue);
			}

			onReInvite(cid, txIp, txPort, caps, mediaActive!=0, isAnswer);
		}

		/// <summary>
		/// It handles directory number change by call manager.
		/// </summary>
		/// <param name="flatmap">the message</param>
		private void ProcessResetDN(FlatmapList flatmap)
		{
			string dn;
			try { dn = Convert.ToString(flatmap.Find((uint)MsgField.DirectoryNumber, 1).dataValue); }
			catch
			{
				log.Write(TraceLevel.Error, "Received ResetDirectoryNumber message from Sip service with no Directory Number specified.");
				return;
			}
			
			onResetDirectoryNumber(dn);
		}
		
		/// <summary>
		/// It handles ringing notification from stack
		/// </summary>
		/// <param name="flatmap">the message</param>
		private void ProcessRinging(FlatmapList flatmap)
        {
            Assertion.Check(onRinging != null, "onRinging delegate not hooked in Sip proxy");

            long cid;
            try { cid= Convert.ToUInt32(flatmap.Find((uint)MsgField.CallId, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received Ringing message from Sip service with no call ID specified.");
                return;
            }

            onRinging(cid, null);
        }

		/// <summary>
		/// It handles CallEstablished notification from stack.
		/// </summary>
		/// <param name="flatmap">the message</param>
        private void ProcessCallEstablished(FlatmapList flatmap)
        {
            Assertion.Check(onCallEstablished != null, "onCallEstablished delegate not hooked in Sip proxy");

			long cid;
            try { cid = Convert.ToUInt32(flatmap.Find((uint)MsgField.CallId, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received CallEstablished message from Sip service with no call ID specified.");
                return;
            }

            onCallEstablished(cid);
        }

		/// <summary>
		/// It handles CallAnswered notification from stack.
		/// </summary>
		/// <param name="flatmap">the message</param>
        private void ProcessAnswered(FlatmapList flatmap)
        {
            Assertion.Check(onAnswered != null, "onAnswered delegate not hooked in Sip proxy");

			long cid;
			string stackCallId;
            string from, to, originalTo;
            try { cid = Convert.ToUInt32(flatmap.Find((uint)MsgField.CallId, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received Answered message from Sip service with no call ID specified.");
                return;
            }

			try { stackCallId = Convert.ToString(flatmap.Find((uint)MsgField.StackCallId, 1).dataValue); }
			catch
			{
				log.Write(TraceLevel.Error, "Received Answered message from Sip service with no stackCallId specified.");
				return;
			}

            try { from = Convert.ToString(flatmap.Find((uint)MsgField.From, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received Answered message from Sip service with no source address specified.");
                return;
            }

            try { to = Convert.ToString(flatmap.Find((uint)MsgField.To, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received Answered message from Sip service with no destination address specified.");
                return;
            }

            try { originalTo = Convert.ToString(flatmap.Find((uint)MsgField.OriginalTo, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received Answered message from Sip service with no original destination address specified.");
                return;
            }

			MediaCapsField caps = null;
			string txIp = null;
			int txPort = 0;

			//check to see if the incomingCall request comes with media info
			if (flatmap.Contains((uint)MsgField.TxIp))
			{
				txIp = Convert.ToString(flatmap.Find((uint)MsgField.TxIp, 1).dataValue);

				try { txPort = Convert.ToInt32(flatmap.Find((uint)MsgField.TxPort, 1).dataValue); }
				catch
				{
					log.Write(TraceLevel.Error, "Received IncomingCall message from Sip service with no or bad TxPort specified.");
					return;
				}
				caps = new MediaCapsField();
				if (ReadMediaCapsFromFlatMap(flatmap, caps) <= 0)
					caps = null;
			}
			onAnswered(cid, stackCallId, from, to, originalTo, txIp, txPort, caps);
        }

		/// <summary>
		/// It handles CallInitiated notification from stack.
		/// </summary>
		/// <param name="flatmap">the message</param>
        private void ProcessCallInitiated(FlatmapList flatmap)
        {
            Assertion.Check(onCallInitiated != null, "onCallInitiated delegate not hooked in Sip proxy");

			long cid;
            string dn, to, from;

            try { cid = Convert.ToUInt32(flatmap.Find((uint)MsgField.CallId, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received CallInitiated message from Sip service with no interim call ID specified.");
                return;
            }

            try { dn = Convert.ToString(flatmap.Find((uint)MsgField.DirectoryNumber, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received CallInitiated message from Sip service with no directory number specified.");
                return;
            }

            try { to = Convert.ToString(flatmap.Find((uint)MsgField.To, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received CallInitiated message from Sip service with no destination address specified.");
                return;
            }

            try { from = Convert.ToString(flatmap.Find((uint)MsgField.From, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received CallInitiated message from Sip service with no source address specified.");
                return;
            }

            onCallInitiated(cid, dn, to, from);
        }

		/// <summary>
		///Handles Call Hangup notification.
		/// </summary>
		/// <param name="flatmap">callId and stackCallId need to be present</param>
        private void ProcessHangup(FlatmapList flatmap)
        {
            Assertion.Check(onHangup != null, "onHangup delegate not hooked in Sip proxy");

			long cid;
            string cause;
			string stackCallId;
            try { cid = Convert.ToUInt32(flatmap.Find((uint)MsgField.CallId, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received Hangup message from Sip service with no call ID specified.");
                return;
            }

			try { stackCallId = Convert.ToString(flatmap.Find((uint)MsgField.StackCallId, 1).dataValue); }
			catch
			{
				log.Write(TraceLevel.Error, "Received IncomingCall message from Sip service with no stack call ID specified.");
				return;
			}

			try { cause = Convert.ToString(flatmap.Find((uint)MsgField.ResultMsg, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received Hangup message from Sip service with no cause specified.");
                return;
            }

            onHangup(cid, stackCallId, cause);
        }

		/// <summary>
		/// Handles CallEstablished notification
		/// </summary>
		/// <param name="flatmap"></param>

        private void ProcessMediaEstablished(FlatmapList flatmap)
        {
            Assertion.Check(onMediaEstablished != null, "onMediaEstablished delegate not hooked in Sip proxy");

            uint cid;
            try { cid = Convert.ToUInt32(flatmap.Find((uint)MsgField.CallId, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received MediaEstablished message from Sip service with no call ID specified.");
                return;
            }

            IMediaControl.Codecs codec;
			CodecPayloadType payloadType;
            try { 
				payloadType = (CodecPayloadType) Convert.ToUInt32(flatmap.Find((uint)MsgField.TxCodec, 1).dataValue); 
				codec = FromCodecPayloadType(payloadType);
			}
            catch
            {
                log.Write(TraceLevel.Error, "Received MediaEstablished message from Sip service with no codec specified.");
                return;
            }

            uint framesize;
            try { framesize = Convert.ToUInt32(flatmap.Find((uint)MsgField.TxFrameSize, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received MediaEstablished message from Sip service with no framesize specified.");
                return;
            }

            string txIP;
            try { txIP = Convert.ToString(flatmap.Find((uint)MsgField.TxIp, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received MediaEstablished message from Sip service with no Tx IP specified.");
                return;
            }

            ushort txPort;
            try { txPort = Convert.ToUInt16(flatmap.Find((uint)MsgField.TxPort, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received MediaEstablished message from Sip service with no Tx port specified.");
                return;
            }

            onMediaEstablished(cid, codec, framesize, txIP, txPort);
        }

		/// <summary>
		/// Handles CallInactive notification from stack
		/// </summary>
		/// <param name="flatmap"></param>
		/// <param name="inUse"></param>
        private void ProcessCallInactive(FlatmapList flatmap, bool inUse)
        {
            Assertion.Check(onCallInactive != null, "onCallInactive delegate not hooked in Sip proxy");

			uint cid;
            string dn;

            try { cid = Convert.ToUInt32(flatmap.Find((uint)MsgField.CallId, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received {0} message from Sip service with no call ID specified.",
                    inUse ? MsgType.CallInUse.ToString() : MsgType.CallHeld.ToString());
                return;
            }

            try { dn = Convert.ToString(flatmap.Find((uint)MsgField.DirectoryNumber, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received {0} message from Sip service with no directory number specified.",
                    inUse ? MsgType.CallInUse.ToString() : MsgType.CallHeld.ToString());
                return;
            }

            onCallInactive(cid, dn, inUse);
        }

		/// <summary>
		/// Handles GotDigits notification from stack
		/// </summary>
		/// <param name="flatmap"></param>
        private void ProcessGotDigits(FlatmapList flatmap)
        {
            Assertion.Check(onGotDigits != null, "onGotDigits delegate not hooked in Sip proxy");

			uint cid;
            string digits;
            uint source = (uint) SipProvider.Source.Remote;

            try { cid = Convert.ToUInt32(flatmap.Find((uint)MsgField.CallId, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received GotDigits message from Sip service with no call ID specified.");
                return;
            }

            try { digits = Convert.ToString(flatmap.Find((uint)MsgField.Digits, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received GotDigits message from Sip service with no digits specified.");
                return;
            }

/*            if(flatmap.Contains((uint)MsgField.Source))
            {
                try { source = Convert.ToUInt32(flatmap.Find((uint)MsgField.Source, 1).dataValue); }
                catch
                {
                    log.Write(TraceLevel.Error, "Received GotDigits message from Sip service with invalid DigitSource specified: " + flatmap.Find((uint)MsgField.Source, 1));
                    return;
                }
            }
*/
            onGotDigits(cid, digits, source);
        }

		/// <summary>
		/// Handles GotCapabilities notification
		/// </summary>
		/// <param name="flatmap"></param>
		private void ProcessGotCapabilities(FlatmapList flatmap)
		{
			MediaCapsField caps = null;
			uint cid;
			string txIp = null;
			int txPort = 0;

			try { cid = Convert.ToUInt32(flatmap.Find((uint)MsgField.CallId, 1).dataValue); }
			catch
			{
				log.Write(TraceLevel.Error, "Received GotCapabilities message from Sip service with no call ID specified.");
				return;
			}
			
			try { txIp = Convert.ToString(flatmap.Find((uint)MsgField.TxIp, 1).dataValue); }
			catch
			{
				log.Write(TraceLevel.Error, "Received GotCapabilities message from Sip service with no or bad TxIp specified.");
				return;
			}

			try { txPort = Convert.ToInt32(flatmap.Find((uint)MsgField.TxPort, 1).dataValue); }
			catch
			{
				log.Write(TraceLevel.Error, "Received GotCapabilities message from Sip service with no or bad TxPort specified.");
				return;
			}

			caps = new MediaCapsField();
			if (ReadMediaCapsFromFlatMap(flatmap, caps) <= 0)
			{
				log.Write(TraceLevel.Error, "Received GotCapabilities message from Sip service with no or bad MediaCaps specified.");
				return;
			}

			onGotCapabilities(cid, txIp, txPort, caps);
		}

        #endregion

        #region Service message senders

		/// <summary>
		/// It sends MakeCall message to stack to make an outbound call.
		/// </summary>
		/// <param name="ci">Call related information</param>
		/// <param name="dvi">Device information to be used for this outbound call</param>
		/// <param name="caps">Media info for the outbound call</param>
		/// <returns>true if MakeCall message is sent successfully to the stack. false otherwise.</returns>
		public bool SendMakeCall(CallInfo ci, Core.ConfigData.SipDeviceInfo dvi, MediaCapsField caps)
        {
            if(this.stackInitialized == false)
                return false;

			StringBuilder sb = new StringBuilder(64);
			
			FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.CallId, ci.CallId);
			
			sb.AppendFormat("sip:{0}@{1}", ci.To, dvi.DomainName);
            flatmap.Add((int)MsgField.To, sb.ToString()); //"sip:" + ci.To+"@"+ dvi.DomainName);

			sb.Length = 0;
			sb.AppendFormat("sip:{0}@{1}", dvi.DirectoryNumber, dvi.DomainName);
			flatmap.Add((int)MsgField.DirectoryNumber, sb.ToString());	//"sip:" + dvi.Name + "@" + dvi.DomainName); //WORKAROUND FOR CCM Key);
			
			sb.Length = 0;
			if (ci.From != null && ci.From.Length>0)
				sb.AppendFormat("{0} <sip:{1}@{2}>", ci.From, ci.From, dvi.DomainName);
			else 
				sb.AppendFormat("<sip:{0}@{1}>", DefaultFromOutboundNumber, dvi.DomainName);

			flatmap.Add((int)MsgField.From, sb.ToString());	//ci.From!=null ? ci.From + "<sip:" + DefaultFromOutboundNumber "@" + dvi.DomainName);

            flatmap.Add((int)MsgField.DeviceType, (int)dvi.Type);
			for(int i = 0; i < dvi.ServerAddrs.Length; i++)
				flatmap.Add((int)MsgField.Registrars, dvi.ServerAddrs[i].ToString());
			if (dvi.Type == IConfig.DeviceType.CiscoSip)
			{
				Metreos.Core.ConfigData.SipDeviceInfo sipdvi = (Metreos.Core.ConfigData.SipDeviceInfo) dvi;
				if (sipdvi.ProxyAddr != null)
					flatmap.Add((int)MsgField.ProxyServer, sipdvi.ProxyAddr.ToString());
			}

            if(ci.RxIp != null)    { flatmap.Add((int)MsgField.RxIp, ci.RxIp); }
            if(ci.RxPort != 0)     { flatmap.Add((int)MsgField.RxPort, ci.RxPort); }

			//now add the mediacaps
			if (caps != null)
				AddMediaCapsToFlatMap(flatmap, caps);

            if(ipcClient.Write((int)MsgType.MakeCall, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with Sip service while placing a call");
                return false;
			}

			log.Write(TraceLevel.Verbose,
				"Sent MakeCall. stackCallId:{0}, to:{1}, dn:{2}, deviceType:{3}, from:{4}, rxIP:{5}, rxPort:{6}",
				ci.CallId, ci.To, dvi.Key, dvi.Type, ci.From, ci.RxIp, ci.RxPort);
            return true;
        }

		/// <summary>
		/// It sends SetMedia message to the stack.
		/// </summary>
		/// <param name="cid">call id which the media info is for</param>
		/// <param name="stackCallId">stack call id for the call</param>
		/// <param name="rxIP">the IP for receiving media</param>
		/// <param name="rxPort">the port for receiving media</param>
		/// <param name="rxCodec">the codec for receiving media</param>
		/// <param name="rxFramesize">frame size for receiving media</param>
		/// <param name="caps">supported media capabilities</param>
		/// <returns>true if the message is sent successfully to the stack</returns>
        public bool SendSetMedia(long cid, string stackCallId, string rxIP, int rxPort, 
								IMediaControl.Codecs rxCodec, uint rxFramesize,
								MediaCapsField caps)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.CallId, cid);
			flatmap.Add((int)MsgField.StackCallId, stackCallId);
            flatmap.Add((int)MsgField.RxIp, rxIP != null ? rxIP : "");
            flatmap.Add((int)MsgField.RxPort, rxPort);
			if (rxCodec != IMediaControl.Codecs.Unspecified)
			{
				flatmap.Add((int)MsgField.RxCodec, (int)ToCodecPayloadType(rxCodec));
				flatmap.Add((int)MsgField.RxFramesize, rxFramesize);
			}

			if (caps != null)
				AddMediaCapsToFlatMap(flatmap, caps);

            if(ipcClient.Write((int)MsgType.SetMedia, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with Sip service while setting media params");
                return false;
            }

			log.Write(TraceLevel.Verbose, "Sent SetMedia. stackCallId:{0}, rxIP:{1}, rxPort:{2}",
				cid, rxIP==null ? "null" : rxIP, rxPort);
            return true;
        }

		/// <summary>
		/// It sends Hold request to stack.
		/// </summary>
		/// <param name="cid">the call that's to be held</param>
		/// <param name="stackCallId">the stack call id for the call</param>
		/// <returns>true if the message is sent successfully</returns>
        public bool SendHold(long cid, string stackCallId)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.CallId, cid);
			flatmap.Add((int)MsgField.StackCallId, stackCallId);

            if(ipcClient.Write((int)MsgType.Hold, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with Sip service while holding a call");
                return false;
            }

            log.Write(TraceLevel.Verbose, "Sent Hold. stackCallId:{0}", cid);
            return true;
        }

		/// <summary>
		/// It sends resume request to the stack.
		/// </summary>
		/// <param name="cid">identifies the call to be resumed</param>
		/// <param name="stackCallId">call id from stack</param>
		/// <param name="rxIP">the receiving IP for media</param>
		/// <param name="rxPort">the receiving port for the media</param>
		/// <returns>true if the message is sent successfully</returns>
        public bool SendResume(long cid, string stackCallId, string rxIP, uint rxPort)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
			flatmap.Add((int)MsgField.CallId, cid);
            flatmap.Add((int)MsgField.StackCallId, stackCallId);
            flatmap.Add((int)MsgField.RxIp, rxIP != null ? rxIP : "");
            flatmap.Add((int)MsgField.RxPort, rxPort);

            if(ipcClient.Write((int)MsgType.Resume, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with Sip service while resuming a call");
                return false;
            }

            log.Write(TraceLevel.Verbose, "Sent SendResume. stackCallId:{0}, rxIP:{1}, rxPort:{2}",
                cid, rxIP, rxPort);
            return true;
        }

		/// <summary>
		/// It sends media on hold notification to stack
		/// </summary>
		/// <param name="cid">identifies the call</param>
		/// <param name="stackCallId">stack call id for the call</param>
		/// <returns>true if the message is sent successfully</returns>
        public bool SendUseMohMedia(long cid, string stackCallId)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.CallId, cid);
			flatmap.Add((int)MsgField.StackCallId, stackCallId);

            if(ipcClient.Write((int)MsgType.UseMohMedia, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with Sip service while setting media params");
                return false;
            }

            log.Write(TraceLevel.Verbose, "Sent UseMohMedia. stackCallId:{0}", cid);
			return true;
        }

		/// <summary>
		/// It asks the stack to accept call.
		/// </summary>
		/// <param name="cid">identifies the call</param>
		/// <param name="stackCallId">the stack call id for the call</param>
		/// <returns>true if the message is sent successfully</returns>
        public bool SendAcceptCall(long cid, string stackCallId)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.CallId, cid);
			flatmap.Add((int)MsgField.StackCallId, stackCallId);

            if(ipcClient.Write((int)MsgType.Accept, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with Sip service while rejecting a call");
                return false;
			}

			log.Write(TraceLevel.Verbose, "Sent AcceptCall. callId:{0} stackCallId:{1}", cid, stackCallId);
            return true;
        }

		/// <summary>
		/// It sends Redirect request to the stack.
		/// </summary>
		/// <param name="stackCallId">stack call id for the call to be redirected</param>
		/// <param name="to">where the call should be redirected</param>
		/// <param name="domainName">the domain name for the destination</param>
		/// <returns>true if the message is sent successfully</returns>
        public bool SendRedirect(string stackCallId, string to, string domainName)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.StackCallId, stackCallId);

			StringBuilder sb = new StringBuilder(64);
			//CCM sends incorrect domainname in this case. hard-code it before getting the fix.
			sb.AppendFormat("sip:{0}@{1}", to, domainName); //"10.1.14.33");//domainName); 
			flatmap.Add((int)MsgField.To, sb.ToString()); //"sip:" + ci.To+"@"+ dvi.DomainName);

            if(ipcClient.Write((int)MsgType.Redirect, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with Sip service while redirecting a call");
                return false;
            }

            log.Write(TraceLevel.Verbose, "Sent Redirect. CallId:{0}, to:{1}", stackCallId, to);
            return true;
        }

		/// <summary>
		/// It asks the stack to answer the incoming call.
		/// </summary>
		/// <param name="cid">identifies the call to be answered</param>
		/// <param name="stackCallId">stack call id for the call</param>
		/// <param name="rxIP">IP for receiving media</param>
		/// <param name="rxPort">port for receiving media</param>
		/// <param name="rxCodec">codec for receiving media</param>
		/// <param name="rxFramesize">frame size for receiving media</param>
		/// <returns>identifies the call to be answered</returns>
        public bool SendAnswerCall(long cid, string stackCallId, string rxIP, int rxPort, 
									IMediaControl.Codecs rxCodec, uint rxFramesize,
									MediaCapsField caps)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();

			flatmap.Add((int)MsgField.CallId, cid);
			flatmap.Add((int)MsgField.StackCallId, stackCallId);
			flatmap.Add((int)MsgField.RxIp, rxIP != null ? rxIP : "");
			flatmap.Add((int)MsgField.RxPort, rxPort);
			if (rxCodec != IMediaControl.Codecs.Unspecified)
			{
				flatmap.Add((int)MsgField.RxCodec, (int)ToCodecPayloadType(rxCodec));
				flatmap.Add((int)MsgField.RxFramesize, rxFramesize);
			}
			if (caps != null)
				AddMediaCapsToFlatMap(flatmap, caps);

            if(ipcClient.Write((int)MsgType.Answer, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with Sip service while answering a call");
                return false;
			}

			log.Write(TraceLevel.Verbose, "Sent AnswerCall. stackCallId:{0}, rxIP:{1}, rxPort:{2}",
				cid, rxIP, rxPort);
            return true;
        }

		/// <summary>
		/// It asks the stack to reject the call.
		/// </summary>
		/// <param name="stackCallId">identifies the call to be rejected</param>
		/// <returns>identifies the call to be answered</returns>
        public bool SendRejectCall(string stackCallId)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.StackCallId, stackCallId);

            if(ipcClient.Write((int)MsgType.Reject, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with Sip service while rejecting a call");
                return false;
			}

			log.Write(TraceLevel.Verbose, "Sent RejectCall. stackCallId:{0}", stackCallId);
            return true;
        }

		/// <summary>
		/// It requests the stack to do a blind transfer for the call.
		/// </summary>
		/// <param name="stackCallId">identifies the call to be blind transferred</param>
		/// <param name="to">where the call to be transferred to</param>
		/// <param name="domainName">the domainname for the destination</param>
		/// <returns>identifies the call to be answered</returns>
        public bool SendBlindTransfer(string stackCallId, string to, string domainName)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.StackCallId, stackCallId);

			StringBuilder sb = new StringBuilder(64);
			sb.AppendFormat("sip:{0}@{1}", to, domainName);
			flatmap.Add((int)MsgField.To, sb.ToString()); //"sip:" + ci.To+"@"+ dvi.DomainName);
			
            if(ipcClient.Write((int)MsgType.BlindTransfer, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with Sip service while blind transfering a call");
                return false;
			}

			log.Write(TraceLevel.Verbose, "Sent BlindTransfer. stackCallId:{0}, to:{1}", stackCallId, to);
            return true;
        }

		/// <summary>
		/// It requests the stack to conference
		/// </summary>
		/// <param name="stackCallId">identifies the call to be conferenced</param>
		/// <param name="vStackCallId"></param>
		/// <returns>identifies the call to be answered</returns>
        public bool SendConference(string stackCallId, string vStackCallId)
        {
			Assertion.Check(false, "False?");
/*            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.CallId, stackCallId);
            flatmap.Add((int)MsgField.VolatileCallId, vStackCallId);

            if(ipcClient.Write((int)MsgType.Conference, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with Sip service while conferencing a call");
                return false;
            }

            log.Write(TraceLevel.Verbose, "Sent Conference. stackCallId:{0}, vStackCallId:{1}",
                stackCallId, vStackCallId);
  */          return true;
        }

		/// <summary>
		/// It asks the stack to hangup the call
		/// </summary>
		/// <param name="stackCallId">identifies the call to be hung up</param>
        public void SendHangup(string stackCallId)
        {
            if(this.stackInitialized == false)
                return;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.StackCallId, stackCallId);

            if(ipcClient.Write((int)MsgType.Hangup, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with Sip service while terminating a call");
				return;
			}

			log.Write(TraceLevel.Verbose, "Sent Hangup. stackCallId:{0}", stackCallId);
        }

		/// <summary>
		/// It sends the user input to the stack
		/// </summary>
		/// <param name="stackCallId">identifies the call</param>
		/// <param name="digits">the digits to be sent</param>
		/// <returns>identifies the call to be answered</returns>
        public bool SendUserInput(string stackCallId, string digits)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.StackCallId, stackCallId);
            flatmap.Add((int)MsgField.Digits, digits);

            if(ipcClient.Write((int)MsgType.SendUserInput, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with Sip service while sending digits to a call");
                return false;
			}

			log.Write(TraceLevel.Verbose, "Sent SendUserInput. stackCallId:{0}", stackCallId);
            return true;
        }

		/// <summary>
		/// It sends registration request to the stack
		/// </summary>
		/// <param name="dInfo">the device to be registered</param>
		/// <returns>identifies the call to be answered</returns>
        public bool SendRegister(Core.ConfigData.SipDeviceInfo dInfo)
        {
            return ipcClient.Write((int)MsgType.RegisterDevices, CreateFlatmapToRegister(dInfo), Consts.IpcWriteTimeout);
        }

		/// <summary>
		/// It sends unregistration request to the stack
		/// </summary>
		/// <param name="dInfo">the device to be unregistered</param>
		/// <returns>identifies the call to be answered</returns>
        public bool SendUnregister(Core.ConfigData.SipDeviceInfo dInfo)
        {
            return ipcClient.Write((int)MsgType.UnregisterDevices, CreateFlatmapToRegister(dInfo), Consts.IpcWriteTimeout);
        }

		/// <summary>
		/// It creates the flatmap for the device to register/unregister
		/// </summary>
		/// <param name="dInfo">the device to be (un)registered</param>
		/// <returns>the flatmap for the registration</returns>
		private FlatmapList CreateFlatmapToRegister(Core.ConfigData.SipDeviceInfo dInfo)
		{
			FlatmapList flatmap = new FlatmapList();
			flatmap.Add((uint)MsgField.UserName, dInfo.Username);
			flatmap.Add((uint)MsgField.Password, dInfo.Password);
			StringBuilder sb =  new StringBuilder(64);
			sb.AppendFormat("{0}@{1}", dInfo.DirectoryNumber, dInfo.DomainName);
			flatmap.Add((uint)MsgField.DirectoryNumber, sb.ToString());
			flatmap.Add((uint)MsgField.DeviceName, dInfo.Name);
			flatmap.Add((uint)MsgField.DeviceType, (int)dInfo.Type);
			flatmap.Add((uint)MsgField.DomainName, dInfo.DomainName);

			for(int i = 0; i < dInfo.ServerAddrs.Length; i++)
			{
				flatmap.Add((uint)MsgField.Registrars, dInfo.ServerAddrs[i].ToString());
			}
			
			if (dInfo.ProxyAddr != null)
			{
				flatmap.Add((uint)MsgField.ProxyServer, dInfo.ProxyAddr);
			}

			return flatmap;
		}

		/// <summary>
		/// It sends StartStack request to stack.
		/// </summary>
		/// <returns>identifies the call to be answered</returns>
		public bool SendStartStack()
		{
			FlatmapList flatmap = new FlatmapList();
			flatmap.Add((int)MsgField.DefaultFrom, defaultFromOutboundNumber);
			flatmap.Add((int)MsgField.MinRegistrationPort, minRegistartionPort);
			flatmap.Add((int)MsgField.MaxRegistrationPort, maxRegistartionPort);
			flatmap.Add((int)MsgField.SipTrunkIp, sipTrunkIp);
			flatmap.Add((int)MsgField.SipTrunkPort, sipTrunkPort);
			flatmap.Add((int)MsgField.ServiceLogLevel, serviceLogLevel);
			flatmap.Add((int)MsgField.LogTimingStat, (int) (logTimingStat ? 1 : 0));

			return ipcClient.Write((int)MsgType.StartStack, flatmap, Consts.IpcWriteTimeout);
		}

		/// <summary>
		/// It notifies the stack about parameter change.
		/// 
		/// </summary>
		/// 
		/// <returns>true if the message is sent</returns>
		public bool SendParameterChanged()
		{
			FlatmapList flatmap = new FlatmapList();
			flatmap.Add((int)MsgField.DefaultFrom, defaultFromOutboundNumber);
			flatmap.Add((int)MsgField.MinRegistrationPort, minRegistartionPort);
			flatmap.Add((int)MsgField.MaxRegistrationPort, maxRegistartionPort);
			flatmap.Add((int)MsgField.SipTrunkIp, sipTrunkIp);
			flatmap.Add((int)MsgField.SipTrunkPort, sipTrunkPort);
			flatmap.Add((int)MsgField.ServiceLogLevel, serviceLogLevel);
			flatmap.Add((int)MsgField.LogTimingStat, (int) (logTimingStat ? 1 : 0));

			return ipcClient.Write((int)MsgType.ParameterChanged, flatmap, Consts.IpcWriteTimeout);
		}

		#endregion

        #region Private helper methods
        
		/// <summary>
		/// It converts stack status to provider status for the device
		/// </summary>
		/// <param name="SipStatus">the stack status</param>
		/// <returns>the provider status</returns>
        IConfig.Status ConvertStatus(Status SipStatus)
        {
            IConfig.Status status = IConfig.Status.Unspecified;

            switch(SipStatus)
            {
                case Status.DeviceRegistered:
                    status = IConfig.Status.Enabled_Running;
                    break;
                case Status.DeviceUnregistered:
                    status = IConfig.Status.Enabled_Stopped;
                    break;

				case Status.DeviceFailedToRegister:
					status = IConfig.Status.Disabled_Error;
					break;

				default:
					break;
            }

            return status;
        }

		/// <summary>Adds low-bitrate coder info to flatmap if present in the provided caps set.</summary>
		/// <param name="map">The map to add entries to.</param>
		/// <param name="caps">The MediaCapsField object to add to the flatmap.</param>
		private void AddMediaCapsToFlatMap(FlatmapList map, MediaCapsField caps)
		{
			StringBuilder sb = new StringBuilder();
			IEnumerator ie = caps.GetEnumerator();
			while (ie.MoveNext())
			{
				IMediaControl.Codecs codec = (IMediaControl.Codecs) ((DictionaryEntry) ie.Current).Key;
				uint[] fms = caps.GetFramesizes(codec);
				sb.AppendFormat("{0}", (int) ToCodecPayloadType(codec));

				foreach(uint s in fms)
				{
					sb.AppendFormat(" {0}", s);
				}

				map.Add((uint) MsgField.MediaCaps, sb.ToString());
				sb.Remove(0, sb.Length);
			}
		}

		/// <summary>
		/// It parses the flatmap for media information from stack.
		/// </summary>
		/// <param name="map">the message from stack</param>
		/// <param name="caps">the parsed media cap information</param>
		/// <returns>number of media caps in the message</returns>
		private int ReadMediaCapsFromFlatMap(FlatmapList map, MediaCapsField caps)
		{
			int count = 0;
			int i = 0;
			while (map.Contains((uint) MsgField.MediaCaps, i++))
			{
				Flatmap.MapEntry entry;
				entry = map.Find((uint) MsgField.MediaCaps, i);
				string buf = Convert.ToString(entry.dataValue);
				if (buf.Length == 0)
					continue;

				string[] tok = buf.Split();
				if (tok.Length > 1)	//good input
				{
					try
					{
						//first token is the payload type code
						CodecPayloadType payloadType = (CodecPayloadType) Convert.ToUInt32(tok[0]);
						//the rest are the framesizes
						int toki = 1;
						uint[] frms = new uint[tok.Length-1];
						while(toki < tok.Length)
						{
							frms[toki-1] = Convert.ToUInt32(tok[toki]);
							++toki;
						}
						caps.Add(FromCodecPayloadType(payloadType), frms);
						++count;
					}
					catch
					{
						//invalid input, just ignore it
							log.Write(TraceLevel.Error, "Invalid message field MediaCaps from sip stack: {0}", buf);
					}

				}
				else
				{
					//just ignore it
					log.Write(TraceLevel.Error, "Invalid message field MediaCaps from sip stack: {0}", buf);
				}
			}

			return count;
		}

		/// <summary>
		/// It converts provider media codec code to standard Paylod Type
		/// </summary>
		/// <param name="codec">provider media codec to be converted</param>
		/// <returns>the standard Payload Type</returns>
		private CodecPayloadType ToCodecPayloadType(IMediaControl.Codecs codec)
		{
			CodecPayloadType payloadType;
			switch(codec)
			{
				case IMediaControl.Codecs.G711u:
					payloadType = CodecPayloadType.G711u;
					break;

				case IMediaControl.Codecs.G711a:
					payloadType = CodecPayloadType.G711a;
					break;

				case IMediaControl.Codecs.G723:
					payloadType = CodecPayloadType.G723;
					break;

				case IMediaControl.Codecs.G729:
					payloadType = CodecPayloadType.G729;
					break;

				default:
					payloadType = CodecPayloadType.Unspecified;
					break;
			}

			return payloadType;
		}

		/// <summary>
		/// It converts Payload Type to provider media codec type
		/// </summary>
		/// <param name="payloadType">Payload type to be converted</param>
		/// <returns>media codec type</returns>
		private IMediaControl.Codecs FromCodecPayloadType(CodecPayloadType payloadType)
		{
			IMediaControl.Codecs codec;
			switch(payloadType)
			{
				case CodecPayloadType.G711a:
					codec = IMediaControl.Codecs.G711a;
					break;

				case CodecPayloadType.G711u:
					codec = IMediaControl.Codecs.G711u;
					break;

				case CodecPayloadType.G723:
					codec = IMediaControl.Codecs.G723;
					break;

				case CodecPayloadType.G729:
					codec = IMediaControl.Codecs.G729;
					break;

				default:
					codec = IMediaControl.Codecs.Unspecified;
					break;
			}

			return codec;

		}

		#endregion
    }
}
