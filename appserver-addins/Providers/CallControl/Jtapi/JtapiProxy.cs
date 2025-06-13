using System;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Collections;

using Metreos.Core.IPC;
using Metreos.Interfaces;
using Metreos.Utilities;
using Metreos.Core.Sockets;
using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;
using Metreos.Core.IPC.Flatmaps;
using Metreos.Messaging.MediaCaps;

using Utils=Metreos.Utilities;

namespace Metreos.CallControl.JTapi
{
    public delegate MediaCapsField GetCodecsDelegate();
    public delegate DeviceInfo[] GetDevicesDelegate(string name);

    public delegate void NameDelegate(string name);
    public delegate void OnErrorDelegate(string deviceName, string stackCallId, JTapiProxy.FailReason reason, string message, 
        JTapiProxy.MsgType msgType, JTapiProxy.MsgField msgField);
    public delegate void OnStatusUpdateDelegate(string deviceName, string dn, IConfig.Status status);
    public delegate void OnCallInitiatedDelegate(string stackCallId, string deviceName, string from);
    public delegate void OnCallInactiveDelegate(string deviceName, string stackCallId, bool inUse);
    public delegate void OnCallEstablishedDelegate(string stackCallId, string from, string to, string originalTo);
    public delegate void OnMediaEstablishedDelegate(string stackCallId, IMediaControl.Codecs codec, uint framesize, string txIP, ushort txPort);
    public delegate void OnIncomingCallDelegate(string stackCallId, string deviceName, string to, string from, string originalTo);
    public delegate void OnHangupDelegate(string stackCallId, string cause);
    public delegate void CallNoticeDelegate(string stackCallId);
    public delegate void OnGotDigitsDelegate(string stackCallId, string digits, uint source);

	/// <summary>Flatmap IPC abstraction layer</summary>
	public class JTapiProxy : IDisposable
	{
        private delegate void ProcessMessageDelegate(int messageType, FlatmapList flatmap);

        #region Constants

        private abstract class Consts
        {
            public const int IpcWriteTimeout    = 5;    // seconds
            public const int IpcConnectTimeout  = 120;  // seconds
            public const int NumThreads         = 3;
            public const int MaxThreads         = 10;
            public const string PoolName        = "JTAPI ThreadPool";
            public const string ConnectorThreadName = "JTAPI service connect thread";
        }

        #endregion

        #region Message definitions

        public enum MsgType
        {
            Error               = 0,	// Stack  -> Provider  
            Register            = 1,	// Stack <-  Provider
            Unregister          = 2,	// Stack <-  Provider
            StatusUpdate		= 3,    // Stack  -> Provider
            RegisterMediaCaps   = 4,    // Stack <-  Provider
            UnregisterMediaCaps = 5,    // Stack <-  Provider
            IncomingCall        = 20,	// Stack  -> Provider
            MakeCall            = 21,	// Stack <-  Provider
            AnswerCall			= 22,	// Stack <-  Provider
            RejectCall          = 24,	// Stack <-  Provider
            Hangup              = 25,	// Stack <-> Provider
            AcceptCall          = 26,   // Stack <-  Provider
            Redirect            = 27,   // Stack <-  Provider
            BlindTransfer       = 28,   // Stack <-  Provider
            Conference          = 29,   // Stack <-  Provider
            CallEstablished     = 31,	// Stack  -> Provider
            MakeCallAck         = 33,   // Stack  -> Provider
            CallHeld            = 34,   // Stack  -> Provider
            InitiatedCall       = 35,   // Stack  -> Provider
            CallInUse           = 37,   // Stack  -> Provider
            Answered            = 45,   // Stack  -> Provider
            SetMedia            = 46,   // Stack <-  Provider
            UseMohMedia         = 47,   // Stack <-  Provider
            Hold                = 48,   // Stack <-  Provider
            Resume              = 49,   // Stack <-  Provider
            SetLogLevel         = 50,   // Stack <-  Provider
            SendUserInput       = 96,   // Stack <-  Provider
            GotDigits           = 97,   // Stack  -> Provider
            MediaEstablished    = 98,	// Stack  -> Provider
            Ringing             = 99    // Stack  -> Provider
        }

        public enum MsgField
        {
            FailReason          = 0,
            Status				= 1,	// Init fields
            CtiManager			= 2,
            Username			= 3,
            Password			= 4,
            DeviceName          = 5,
            DeviceType			= 6,
            MessageType         = 7,
            TxIP                = 21,	// Media fields
            TxPort              = 22,
            RxIP                = 23,
            RxPort              = 24,
            Codec				= 25,
            Framesize			= 26,
            CallId              = 40,	// Call fields
			To					= 41,
            From                = 42,
            OriginalTo          = 43,
            Cause               = 46,
            VolatileCallId      = 47,
            FromCallId          = 48,
            ToCallId            = 49,
            MessageField        = 50,
            Digits              = 51,
            Args                = 52,
            CallControlCause    = 53,
            Source              = 54,
            IsThirdParty        = 55,
            Level               = 56,   // Log Level
            Message             = 99
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
            MissingField        = 13,
            InvalidDestination  = 15,
            PlatformException   = 16
        }

        public enum Status 
        {
            DeviceOffline	= 0,
            DeviceOnline	= 1
        }

        #endregion

        private readonly string name;
        private readonly LogWriter log;
        private readonly IpcFlatmapClient ipcClient;
        private readonly Utils.ThreadPool threadPool;

        private MediaCapsField mediaCaps = null;
        private volatile bool stackInitialized = false;

        // Internal data delegates
        public NameDelegate onServiceGone;
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
        public OnCallEstablishedDelegate onAnswered;
        public OnHangupDelegate onHangup;
        public OnMediaEstablishedDelegate onMediaEstablished;
        public OnCallInitiatedDelegate onCallInitiated;
        public OnGotDigitsDelegate onGotDigits;

        // ThreadPool callback delegates
        private readonly ProcessMessageDelegate processMessageAsync;
        
        #region Construction/Startup/Shutdown/Cleanup
		public JTapiProxy(string name, LogWriter log)
		{
            Assertion.Check(name != null, "null name passed into JTapiProxy constructor");
            Assertion.Check(log != null, "null log passed into JTapiProxy constructor");
            
            this.name = name;
            this.log = log;

            this.processMessageAsync = new ProcessMessageDelegate(ProcessMessageAsync);

            this.threadPool = new Utils.ThreadPool(Consts.NumThreads, Consts.MaxThreads, Consts.PoolName);
            this.threadPool.MessageLogged += new LogDelegate(log.Write);

            this.ipcClient = new IpcFlatmapClient();
			this.ipcClient.onConnect = new OnConnectDelegate(ipcClient_onConnect);
            this.ipcClient.onFlatmapMessageReceived = new OnFlatmapMessageReceivedDelegate(ipcClient_OnMessageReceived);
			this.ipcClient.onClose = new OnCloseDelegate(ipcClient_onConnectionClosed);
		}

        public bool Startup(IPEndPoint ipEndpoint)
        {
            if(ipEndpoint == null || ipEndpoint.Address == null || ipEndpoint.Port == 0)
                return false;

            if(this.threadPool == null)
                throw new ObjectDisposedException(typeof(JTapiProxy).Name);

            if(this.threadPool.IsStarted == false)              
                this.threadPool.Start();

            if(ipcClient == null)
                throw new ObjectDisposedException(typeof(JTapiProxy).Name);

			ipcClient.RemoteEp = ipEndpoint;
			ipcClient.Start();
			return true;
        }

        public void Shutdown()
        {
            if(threadPool != null)
                threadPool.Stop();

            if(ipcClient != null)
                ipcClient.Close();
        }

        public void Dispose()
        {
            ipcClient.Dispose();
            threadPool.Stop();
        }

        private void InitializeService()
        {
            if(ipcClient == null) 
                return;

            if(mediaCaps == null)
            {
                Assertion.Check(this.onGetCodecs != null, "onGetCodecs delegate not hooked in JTAPI provider");
                mediaCaps = onGetCodecs();

                if(mediaCaps == null)
                {
                    log.Write(TraceLevel.Error, "JTAPI provider was unable to obtain codec list");
                    return;
                }
            }

            // Send config data to service
            if(SendRegister(mediaCaps) == false)
            {
                log.Write(TraceLevel.Error, "Failed to initialize JTAPI service v{0}", name);
                return;
            }

            Assertion.Check(this.onGetDevices != null, "onGetDevices delegate not hooked in JTAPI provider");
            ICollection devices = onGetDevices(name);

            if(devices != null && devices.Count > 0)
            {
                lock(devices)
                {
                    foreach(DeviceInfo dInfo in devices)
                    {
                        if(SendRegister(dInfo) == false)
                        {
                            log.Write(TraceLevel.Error, "Failed to register device '{0}' with the JTAPI service v{1}",
                                dInfo.Name, name);
                        }
                        else
                        {
                            log.Write(TraceLevel.Info, "Registering device '{0}' with the JTAPI service v{1}", 
                                dInfo.Name, name);
                        }
                    }
                }
            }
            else
            {
                log.Write(TraceLevel.Info, "No CTI devices configured for v{0}.", name);
            }

            this.stackInitialized = true;
        }

        #endregion

        #region IPC Client events

        private void ipcClient_onConnect(IpcClient c, bool reconnect)
        {
            log.Write(TraceLevel.Info, "Connected to JTAPI service v{0} successfully", name);
			whineAboutClose = true;
            InitializeService();
        }

		private bool whineAboutClose = true;        // Gee, can you tell which code is Scott's?  ;)

        private void ipcClient_onConnectionClosed(IpcClient c, Exception e)
        {
            Assertion.Check(onServiceGone != null, "onServiceGone delegate not hooked in JTapiProxy");

			if(whineAboutClose)
			{
				log.Write(TraceLevel.Warning, "Lost connection to JTAPI service v{0}: {1}",
					name, e != null ? e.Message : "(no msg)" );
				whineAboutClose = false;

                onServiceGone(name);
			}
        }

        private void ipcClient_OnMessageReceived(IpcFlatmapClient client, int messageType, FlatmapList flatmap)
        {            
            threadPool.PostRequest(processMessageAsync, new object[] { messageType, flatmap });
        }

        private void ProcessMessageAsync(int messageType, FlatmapList flatmap)
        {
            log.Write(TraceLevel.Verbose, "Got {0}({1}) message from v{2}.", 
                messageType, ((MsgType)messageType).ToString(), name);

            foreach(uint fieldKey in flatmap.Keys)
            {
                object fieldValueObj = flatmap.Find(fieldKey, 1).dataValue;
                string fieldValue = fieldValueObj == null ? "<null>" : fieldValueObj.ToString();
                
                log.Write(TraceLevel.Verbose, "Field: {0}({1}) = {2}", 
                    fieldKey, ((MsgField)fieldKey).ToString(), fieldValue);
            }

            switch((MsgType)messageType)
            {
                case MsgType.Error: 
                    ProcessError(flatmap);
                    break;
                case MsgType.MakeCallAck:
                    ProcessMakeCallAck(flatmap);
                    break;
                case MsgType.StatusUpdate:
                    ProcessStatusUpdate(flatmap);
                    break;
                case MsgType.InitiatedCall:
                    ProcessCallInitiated(flatmap);
                    break;
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
                case MsgType.CallInUse:
                    ProcessCallInactive(flatmap, true);
                    break;
                case MsgType.GotDigits:
                    ProcessGotDigits(flatmap);
                    break;
                case MsgType.Ringing:
                    ProcessRinging(flatmap);
                    break;
                default:
                    log.Write(TraceLevel.Error, 
                        "Received unknown message type '{0}' from JTAPI service v{1}", messageType, name);
                    break;
            }
        }

        private void ProcessError(FlatmapList flatmap)
        {
            Assertion.Check(onError != null, "onError delegate not hooked in JTAPI proxy");
            
            string deviceName = null;
            try { deviceName = Convert.ToString(flatmap.Find((uint)MsgField.DeviceName, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received failure message from JTAPI service v{0} with no device name specified.", name);
                return;
            }

            int failReason = 0;
            try { failReason = Convert.ToInt32(flatmap.Find((uint)MsgField.FailReason, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received failure message from JTAPI service v{0} with no reason specified.", name);
                return;
            }

            string message = null;
            if(flatmap.Contains((uint)MsgField.Message))
            {
                message = Convert.ToString(flatmap.Find((uint)MsgField.Message, 1).dataValue);
            }

            int msgType = 0;
            if(flatmap.Contains((uint)MsgField.MessageType))
            {
                msgType = Convert.ToInt32(flatmap.Find((uint)MsgField.MessageType, 1).dataValue);
            }

            int msgField = 0;
            if(flatmap.Contains((uint)MsgField.MessageField))
            {
                msgField = Convert.ToInt32(flatmap.Find((uint)MsgField.MessageField, 1).dataValue);
            }

            // Try 9,000,000 different ways to get the call ID
            string stackCallId = Convert.ToString(flatmap.Find((uint)MsgField.CallId, 1).dataValue);
            if(stackCallId == null || stackCallId == String.Empty)
            {
                stackCallId = Convert.ToString(flatmap.Find((uint)MsgField.FromCallId, 1).dataValue);
            }
            if((stackCallId == null || stackCallId == String.Empty) && flatmap.Contains((uint)MsgField.Args))
            {
                try
                {
                    byte[] args = flatmap.Find((uint)MsgField.Args, 1).dataValue as byte[];
                    if(args != null)
                    {
                        FlatmapList origMsg = new FlatmapList(args);
                        stackCallId = Convert.ToString(origMsg.Find((uint)MsgField.CallId, 1).dataValue);
                        if(stackCallId == null || stackCallId == String.Empty)
                        {
                            stackCallId = Convert.ToString(origMsg.Find((uint)MsgField.FromCallId, 1).dataValue);
                        }
                    }
                }
                catch {}
            }

            onError(deviceName, stackCallId, (FailReason)failReason, message, (MsgType)msgType, (MsgField)msgField);
        }

        private void ProcessMakeCallAck(FlatmapList flatmap)
        {
            Assertion.Check(onMakeCallAck != null, "onMakeCallAck delegate not hooked in JTAPI proxy");

            string stackCallId = Convert.ToString(flatmap.Find((uint)MsgField.CallId, 1).dataValue);
            if(stackCallId == null || stackCallId == String.Empty)
            {
                stackCallId = Convert.ToString(flatmap.Find((uint)MsgField.FromCallId, 1).dataValue);
            }

            onMakeCallAck(stackCallId);
        }

        private void ProcessStatusUpdate(FlatmapList flatmap)
        {
            Assertion.Check(onStatusUpdate != null, "onStatusUpdate delegate not hooked in JTAPI proxy");

            Status jtapiStatus;
            try { jtapiStatus = (Status) Convert.ToUInt32(flatmap.Find((uint)MsgField.Status, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received UpdateStatus message from JTAPI service v{0} with no status specified.", name);
                return;
            }

            string deviceName = null;
            try { deviceName = Convert.ToString(flatmap.Find((uint)MsgField.DeviceName, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received UpdateStatus message from JTAPI service v{0} with no device name specified.", name);
                return;
            }

            string dn = null;
            try { dn = Convert.ToString(flatmap.Find((uint)MsgField.From, 1).dataValue); }
            catch {}

            if(dn == null)
                log.Write(TraceLevel.Warning, "Received UpdateStatus message from JTAPI service v{0} with no directory number specified.", name);

            IConfig.Status status = ConvertStatus(jtapiStatus);

            onStatusUpdate(deviceName, dn, status);
        }

        private void ProcessIncomingCall(FlatmapList flatmap)
        {
            Assertion.Check(onIncomingCall != null, "onIncomingCall delegate not hooked in JTAPI proxy");

            string stackCallId, deviceName, to, from, originalTo;

            try { stackCallId = Convert.ToString(flatmap.Find((uint)MsgField.CallId, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received IncomingCall message from JTAPI service v{0} with no interim call ID specified.", name);
                return;
            }

            try { deviceName = Convert.ToString(flatmap.Find((uint)MsgField.DeviceName, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received IncomingCall message from JTAPI service v{0} with no device name specified.", name);
                return;
            }

            try { to = Convert.ToString(flatmap.Find((uint)MsgField.To, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received IncomingCall message from JTAPI service v{0} with no destination address specified.", name);
                return;
            }

            try { from = Convert.ToString(flatmap.Find((uint)MsgField.From, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received IncomingCall message from JTAPI service v{0} with no source address specified.", name);
                return;
            }

            if(flatmap.Contains((uint)MsgField.OriginalTo))
                originalTo = Convert.ToString(flatmap.Find((uint)MsgField.OriginalTo, 1).dataValue);
            else
                originalTo = to;

            onIncomingCall(stackCallId, deviceName, to, from, originalTo);
        }

        private void ProcessRinging(FlatmapList flatmap)
        {
            Assertion.Check(onRinging != null, "onRinging delegate not hooked in JTAPI proxy");

            string stackCallId;
            try { stackCallId = Convert.ToString(flatmap.Find((uint)MsgField.CallId, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received Ringing message from JTAPI service v{0} with no call ID specified.", name);
                return;
            }

            onRinging(stackCallId);
        }

        private void ProcessCallEstablished(FlatmapList flatmap)
        {
            Assertion.Check(onCallEstablished != null, "onCallEstablished delegate not hooked in JTAPI proxy");

            string stackCallId, from, to, originalTo;
            try { stackCallId = Convert.ToString(flatmap.Find((uint)MsgField.CallId, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received CallEstablished message from JTAPI service v{0} with no call ID specified.", name);
                return;
            }

            try { from = Convert.ToString(flatmap.Find((uint)MsgField.From, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received CallEstablished message from JTAPI service v{0} with no source address specified.", name);
                return;
            }

            try { to = Convert.ToString(flatmap.Find((uint)MsgField.To, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received CallEstablished message from JTAPI service v{0} with no destination address specified.", name);
                return;
            }

            try { originalTo = Convert.ToString(flatmap.Find((uint)MsgField.OriginalTo, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received CallEstablished message from JTAPI service v{0} with no original destination address specified.", name);
                return;
            }

            onCallEstablished(stackCallId, from, to, originalTo);
        }

        private void ProcessAnswered(FlatmapList flatmap)
        {
            Assertion.Check(onAnswered != null, "onAnswered delegate not hooked in JTAPI proxy");

            string stackCallId, from, to, originalTo;
            try { stackCallId = Convert.ToString(flatmap.Find((uint)MsgField.CallId, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received Answered message from JTAPI service v{0} with no call ID specified.", name);
                return;
            }

            try { from = Convert.ToString(flatmap.Find((uint)MsgField.From, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received Answered message from JTAPI service v{0} with no source address specified.", name);
                return;
            }

            try { to = Convert.ToString(flatmap.Find((uint)MsgField.To, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received Answered message from JTAPI service v{0} with no destination address specified.", name);
                return;
            }

            try { originalTo = Convert.ToString(flatmap.Find((uint)MsgField.OriginalTo, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received Answered message from JTAPI service v{0} with no original destination address specified.", name);
                return;
            }

            onAnswered(stackCallId, from, to, originalTo);
        }

        private void ProcessCallInitiated(FlatmapList flatmap)
        {
            Assertion.Check(onCallInitiated != null, "onCallInitiated delegate not hooked in JTAPI proxy");

            string stackCallId, deviceName, to, from;

            try { stackCallId = Convert.ToString(flatmap.Find((uint)MsgField.CallId, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received CallInitiated message from JTAPI service v{0} with no interim call ID specified.", name);
                return;
            }

            try { deviceName = Convert.ToString(flatmap.Find((uint)MsgField.DeviceName, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received CallInitiated message from JTAPI service v{0} with no device name specified.", name);
                return;
            }

            try { to = Convert.ToString(flatmap.Find((uint)MsgField.To, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received CallInitiated message from JTAPI service v{0} with no destination address specified.", name);
                return;
            }

            try { from = Convert.ToString(flatmap.Find((uint)MsgField.From, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received CallInitiated message from JTAPI service v{0} with no source address specified.", name);
                return;
            }

            onCallInitiated(stackCallId, deviceName, from);
        }

        private void ProcessHangup(FlatmapList flatmap)
        {
            Assertion.Check(onHangup != null, "onHangup delegate not hooked in JTAPI proxy");

            string stackCallId, cause;
            try { stackCallId = Convert.ToString(flatmap.Find((uint)MsgField.CallId, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received Hangup message from JTAPI service v{0} with no call ID specified.", name);
                return;
            }

            try { cause = Convert.ToString(flatmap.Find((uint)MsgField.Cause, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received Hangup message from JTAPI service v{0} with no cause specified.", name);
                return;
            }

            onHangup(stackCallId, cause);
        }

        private void ProcessMediaEstablished(FlatmapList flatmap)
        {
            Assertion.Check(onMediaEstablished != null, "onMediaEstablished delegate not hooked in JTAPI proxy");

            string stackCallId = null;
            try { stackCallId = Convert.ToString(flatmap.Find((uint)MsgField.CallId, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received MediaEstablished message from JTAPI service v{0} with no call ID specified.", name);
                return;
            }

            IMediaControl.Codecs codec;
            try { codec = (IMediaControl.Codecs) Convert.ToUInt32(flatmap.Find((uint)MsgField.Codec, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received MediaEstablished message from JTAPI service v{0} with no codec specified.", name);
                return;
            }

            uint framesize;
            try { framesize = Convert.ToUInt32(flatmap.Find((uint)MsgField.Framesize, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received MediaEstablished message from JTAPI service v{0} with no framesize specified.", name);
                return;
            }

            string txIP;
            try { txIP = Convert.ToString(flatmap.Find((uint)MsgField.TxIP, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received MediaEstablished message from JTAPI service v{0} with no Tx IP specified.", name);
                return;
            }

            ushort txPort;
            try { txPort = Convert.ToUInt16(flatmap.Find((uint)MsgField.TxPort, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received MediaEstablished message from JTAPI service v{0} with no Tx port specified.", name);
                return;
            }

            onMediaEstablished(stackCallId, codec, framesize, txIP, txPort);
        }

        private void ProcessCallInactive(FlatmapList flatmap, bool inUse)
        {
            Assertion.Check(onCallInactive != null, "onCallInactive delegate not hooked in JTAPI proxy");

            string stackCallId, deviceName;

            try { stackCallId = Convert.ToString(flatmap.Find((uint)MsgField.CallId, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received {0} message from JTAPI service v{1} with no call ID specified.",
                    inUse ? MsgType.CallInUse.ToString() : MsgType.CallHeld.ToString(), name);
                return;
            }

            try { deviceName = Convert.ToString(flatmap.Find((uint)MsgField.DeviceName, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received {0} message from JTAPI service v{1} with no device name specified.",
                    inUse ? MsgType.CallInUse.ToString() : MsgType.CallHeld.ToString(), name);
                return;
            }

            onCallInactive(deviceName, stackCallId, inUse);
        }

        private void ProcessGotDigits(FlatmapList flatmap)
        {
            Assertion.Check(onGotDigits != null, "onGotDigits delegate not hooked in JTAPI proxy");

            string stackCallId, digits;
            uint source = 0;

            try { stackCallId = Convert.ToString(flatmap.Find((uint)MsgField.CallId, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received GotDigits message from JTAPI service v{0} with no call ID specified.", name);
                return;
            }

            try { digits = Convert.ToString(flatmap.Find((uint)MsgField.Digits, 1).dataValue); }
            catch
            {
                log.Write(TraceLevel.Error, "Received GotDigits message from JTAPI service v{0} with no digits specified.", name);
                return;
            }

            if(flatmap.Contains((uint)MsgField.Source))
            {
                try { source = Convert.ToUInt32(flatmap.Find((uint)MsgField.Source, 1).dataValue); }
                catch
                {
                    log.Write(TraceLevel.Error, "Received GotDigits message from JTAPI service v{0} with invalid DigitSource specified: {1}", name, flatmap.Find((uint)MsgField.Source, 1));
                    return;
                }
            }

            onGotDigits(stackCallId, digits, source);
        }

        #endregion

        #region Service message senders

        public bool SendMakeCall(string stackCallId, string to, string fromLine, string deviceName, IConfig.DeviceType deviceType, string rxIP, int rxPort)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.FromCallId, stackCallId);
            flatmap.Add((int)MsgField.To, to);
            flatmap.Add((int)MsgField.DeviceName, deviceName);
            flatmap.Add((int)MsgField.DeviceType, (int)deviceType);

            if(fromLine != null && fromLine != String.Empty)
            {
                try 
                { 
                    uint.Parse(fromLine); 
                    flatmap.Add((int)MsgField.From, fromLine);
                }
                catch
                {
                    log.Write(TraceLevel.Warning, "Ignoring invalid field (MakeCall.From = '{0}'). JTAPI requires a valid line address in this field", fromLine);
                }
            }

            if(rxIP != null)    { flatmap.Add((int)MsgField.RxIP, rxIP); }
            if(rxPort != 0)     { flatmap.Add((int)MsgField.RxPort, rxPort); }

            if(ipcClient.Write((int)MsgType.MakeCall, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with JTAPI service v{0} while placing a call", name);
                return false;
			}

			log.Write(TraceLevel.Verbose,
				"Sent MakeCall. stackCallId:{0}, to:{1}, deviceName:{2}, deviceType:{3}, fromLine:{4}, rxIP:{5}, rxPort:{6}",
				stackCallId, to, deviceName, deviceType, fromLine, rxIP, rxPort);
            return true;
        }

        public bool SendSetMedia(string stackCallId, string rxIP, int rxPort)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.CallId, stackCallId);
            flatmap.Add((int)MsgField.RxIP, rxIP != null ? rxIP : "");
            flatmap.Add((int)MsgField.RxPort, rxPort);

            if(ipcClient.Write((int)MsgType.SetMedia, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with JTAPI service v{0} while setting media params.", name);
                return false;
            }

			log.Write(TraceLevel.Verbose, "Sent SetMedia. stackCallId:{0}, rxIP:{1}, rxPort:{2}",
				stackCallId, rxIP, rxPort);
            return true;
        }

        public bool SendHold(string stackCallId)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.CallId, stackCallId);

            if(ipcClient.Write((int)MsgType.Hold, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with JTAPI service v{0} while holding a call.", name);
                return false;
            }

            log.Write(TraceLevel.Verbose, "Sent Hold. stackCallId:{0}", stackCallId);
            return true;
        }

        public bool SendResume(string stackCallId, string rxIP, uint rxPort)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.CallId, stackCallId);
            flatmap.Add((int)MsgField.RxIP, rxIP != null ? rxIP : "");
            flatmap.Add((int)MsgField.RxPort, rxPort);

            if(ipcClient.Write((int)MsgType.Resume, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with JTAPI service v{0} while resuming a call.", name);
                return false;
            }

            log.Write(TraceLevel.Verbose, "Sent SetMedia. stackCallId:{0}, rxIP:{1}, rxPort:{2}",
                stackCallId, rxIP, rxPort);
            return true;
        }

        public bool SendUseMohMedia(string stackCallId)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.CallId, stackCallId);

            if(ipcClient.Write((int)MsgType.UseMohMedia, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with JTAPI service v{0} while setting media params", name);
                return false;
            }

            log.Write(TraceLevel.Verbose, "Sent UseMohMedia. stackCallId:{0}", stackCallId);
			return true;
        }

        public bool SendAcceptCall(string stackCallId)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.CallId, stackCallId);

            if(ipcClient.Write((int)MsgType.AcceptCall, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with JTAPI service v{0} while rejecting a call", name);
                return false;
			}

			log.Write(TraceLevel.Verbose, "Sent AcceptCall. stackCallId:{0}", stackCallId);
            return true;
        }

        public bool SendRedirect(string stackCallId, string to)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.CallId, stackCallId);
            flatmap.Add((int)MsgField.To, to);

            if(ipcClient.Write((int)MsgType.Redirect, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with JTAPI service v{0} while redirecting a call", name);
                return false;
            }

            log.Write(TraceLevel.Verbose, "Sent Redirect. stackCallId:{0}, to:{1}", stackCallId, to);
            return true;
        }

        public bool SendAnswerCall(string stackCallId)
        {
            return SendAnswerCall(stackCallId, null, 0);
        }

        public bool SendAnswerCall(string stackCallId, string rxIP, int rxPort)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.CallId, stackCallId);
            
            if(rxIP != null && rxPort > 0)
            {
                flatmap.Add((int)MsgField.RxIP, rxIP);
                flatmap.Add((int)MsgField.RxPort, rxPort);
            }

            if(ipcClient.Write((int)MsgType.AnswerCall, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with JTAPI service v{0} while answering a call", name);
                return false;
			}

			log.Write(TraceLevel.Verbose, "Sent AnswerCall. stackCallId:{0}, rxIP:{1}, rxPort:{2}",
				stackCallId, rxIP, rxPort);
            return true;
        }

        public bool SendRejectCall(string stackCallId)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.CallId, stackCallId);

            if(ipcClient.Write((int)MsgType.RejectCall, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with JTAPI service v{0} while rejecting a call", name);
                return false;
			}

			log.Write(TraceLevel.Verbose, "Sent RejectCall. stackCallId:{0}", stackCallId);
            return true;
        }

        public bool SendBlindTransfer(string stackCallId, string to)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.CallId, stackCallId);
            flatmap.Add((int)MsgField.To, to);

            if(ipcClient.Write((int)MsgType.BlindTransfer, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with JTAPI service v{0} while blind transfering a call", name);
                return false;
			}

			log.Write(TraceLevel.Verbose, "Sent BlindTransfer. stackCallId:{0}, to:{1}", stackCallId, to);
            return true;
        }

        public bool SendConference(string stackCallId, string vStackCallId)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.CallId, stackCallId);
            flatmap.Add((int)MsgField.VolatileCallId, vStackCallId);

            if(ipcClient.Write((int)MsgType.Conference, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with JTAPI service v{0} while conferencing a call", name);
                return false;
            }

            log.Write(TraceLevel.Verbose, "Sent Conference. stackCallId:{0}, vStackCallId:{1}",
                stackCallId, vStackCallId);
            return true;
        }

        public void SendHangup(string stackCallId)
        {
            if(this.stackInitialized == false)
                return;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.CallId, stackCallId);

            if(ipcClient.Write((int)MsgType.Hangup, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with JTAPI service v{0} while terminating a call", name);
				return;
			}

			log.Write(TraceLevel.Verbose, "Sent Hangup. stackCallId:{0}", stackCallId);
        }

        public bool SendUserInput(string stackCallId, string digits)
        {
            if(this.stackInitialized == false)
                return false;

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.CallId, stackCallId);
            flatmap.Add((int)MsgField.Digits, digits);

            if(ipcClient.Write((int)MsgType.SendUserInput, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with JTAPI service v{0} while sending digits to a call", name);
                return false;
			}

			log.Write(TraceLevel.Verbose, "Sent SendUserInput. stackCallId:{0}", stackCallId);
            return true;
        }

        private bool SendRegister(MediaCapsField caps)
        {
            IDictionaryEnumerator de = (IDictionaryEnumerator) caps.GetEnumerator();
            while(de.MoveNext())
            {
                IMediaControl.Codecs codec = (IMediaControl.Codecs)de.Key;
                ArrayList framesizes = (ArrayList)de.Value;

                FlatmapList flatmap = new FlatmapList();
                flatmap.Add((uint)MsgField.Codec, (int)codec);

                foreach(uint framesize in framesizes)
                {
                    flatmap.Add((uint)MsgField.Framesize, framesize);
                }

                if(ipcClient.Write((int)MsgType.RegisterMediaCaps, flatmap, Consts.IpcWriteTimeout) == false)
                {
                    log.Write(TraceLevel.Error, "Failed to communicate with JTAPI service v{0} while registering media capabilities", name);
                    this.stackInitialized = false;
                    return false;
                }
            }
            return true;
        }

        public bool SendRegister(DeviceInfo dInfo)
        {
            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((uint)MsgField.DeviceName, dInfo.Name);
            flatmap.Add((uint)MsgField.DeviceType, (int)dInfo.Type);
            flatmap.Add((uint)MsgField.Username, dInfo.Username);
            flatmap.Add((uint)MsgField.Password, dInfo.Password);
            flatmap.Add((uint)MsgField.CtiManager, dInfo.ServerAddrs[0].ToString());

            if(dInfo.Type == IConfig.DeviceType.CtiMonitored)
                flatmap.Add((uint) MsgField.IsThirdParty, 1);
            else
                flatmap.Add((uint) MsgField.IsThirdParty, 0);

            if(dInfo.ServerAddrs.Length > 1)
            {
                flatmap.Add((uint)MsgField.CtiManager, dInfo.ServerAddrs[1].ToString());
            }

            return ipcClient.Write((int)MsgType.Register, flatmap, Consts.IpcWriteTimeout);
        }

        public bool SendUnregister(DeviceInfo dInfo)
        {
            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((uint)MsgField.DeviceName, dInfo.Name);
            flatmap.Add((uint)MsgField.DeviceType, (int)dInfo.Type);

            return ipcClient.Write((int)MsgType.Unregister, flatmap, Consts.IpcWriteTimeout);
        }

        public bool SendSetLogLevel(int logLevel)
        {
            log.Write(TraceLevel.Info, "Setting log level for JTAPI service v{0} to level: {1}", name, logLevel);

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((uint) MsgField.Level, logLevel);

            return ipcClient.Write((int) MsgType.SetLogLevel, flatmap, Consts.IpcWriteTimeout);
        }

        #endregion

        #region Private helper methods
        
        IConfig.Status ConvertStatus(Status jtapiStatus)
        {
            IConfig.Status status = IConfig.Status.Unspecified;

            switch(jtapiStatus)
            {
                case Status.DeviceOnline:
                    status = IConfig.Status.Enabled_Running;
                    break;
                case Status.DeviceOffline:
                    status = IConfig.Status.Enabled_Stopped;
                    break;
            }

            return status;
        }

        #endregion
    }
}
