using System;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Collections;

using Metreos.Core.IPC;
using Metreos.Core.IPC.Flatmaps;
using Metreos.Utilities;
using Metreos.Core.Sockets;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.Messaging.MediaCaps;

namespace Metreos.CallControl.H323
{
    public delegate void NullDelegate();
    public delegate void OnIncomingCallDelegate(string callId, string from, string to, string displayName);
    public delegate void OnCallClearedDelegate(string callId, ICallControl.EndReason reason);
    public delegate void OnGotCapabilitiesDelegate(string callId, MediaCapsField caps);
    public delegate void OnCallEstablishedDelegate(string callId, string to, string from);
    public delegate void OnGotDigitsDelegate(string callId, string digits);
    public delegate void OnMakeCallAckDelegate(long callId, string stackCallId, bool fatalError);
    public delegate void OnMediaDelegate(string callId, uint direction, string txIP, 
        uint txPort, IMediaControl.Codecs rxCodec, uint rxFramesize);

    public enum ProxyState
    {
        Disconnected = 0,
        Connecting,
        Connected,
        Started
    }

    /// <summary>Client class for communicating with the H.323
    /// stack service.</summary>
    public class H323IpcClient
    {
        private IpcFlatmapClient ipcClient;

        public event NullDelegate onServiceDown;
        public event OnIncomingCallDelegate onIncomingCall;
        public event OnMakeCallAckDelegate onMakeCallAck;
        public event OnCallClearedDelegate onCallCleared;
        public event OnGotCapabilitiesDelegate onGotCapabilities;
        public event OnCallEstablishedDelegate onCallEstablished;
        public event OnMediaDelegate onMediaEstablished;
        public event OnMediaDelegate onMediaChanged;
        public event OnGotDigitsDelegate onGotDigits;

        private ProxyState state = ProxyState.Disconnected;
        public ProxyState State { get { return state; } }

        private LogWriter log;
        public LogWriter Log { set { log = value; } }

        // Transaction ID (int) -> AppServer call ID (long)
        private Hashtable pendingMakeCalls;

        private int nextTid = 1;

        #region H.323 Stack Parameters

        public abstract class Consts
        {
            public const int MakeCallAckTimeout     = 3000; // milliseconds
            public const int IpcConnectTimeout      = 120;  // seconds
            public const string ConnectorThreadName = "H.323 service connect thread";
        }
        
        private bool enableDebug = H323Provider.Consts.DefaultValues.EnableDebug;
        public  bool EnableDebug { set { enableDebug = value; } }

        private uint debugLevel = H323Provider.Consts.DefaultValues.DebugLevel;
        public  uint DebugLevel { set { debugLevel = value; } }

        private string debugFilename = H323Provider.Consts.DefaultValues.DebugFilename;
        public  string DebugFilename { set { debugFilename = value; } }

        private bool disableFastStart = H323Provider.Consts.DefaultValues.DisableFastStart;
        public  bool DisableFastStart { set { disableFastStart = value; } }
        
        private bool disableH245Tun = H323Provider.Consts.DefaultValues.DisableH245Tunneling;
        public  bool DisableH245Tunneling { set { disableH245Tun = value; } }
        
        private bool disableH245InSetup = H323Provider.Consts.DefaultValues.DisableH245InSetup;
        public  bool DisableH245InSetup { set { disableH245InSetup = value; } }

        private uint listenPort = H323Provider.Consts.DefaultValues.ListenPort;
        public  uint ListenPort { set { listenPort = value; } }

		private uint maxPendingCalls = H323Provider.Consts.DefaultValues.MaxPendingCalls;
		public uint MaxPendingCalls { set { maxPendingCalls = value; } }

        private uint tcpConnectTimeout = H323Provider.Consts.DefaultValues.TcpConnectTimeout;
        public uint TcpConnectTimeout { set { tcpConnectTimeout = value; } }

        private uint serviceLogLevel = H323Provider.Consts.DefaultValues.ServiceLogLevel;
        public uint ServiceLogLevel { set { serviceLogLevel = value; } }

		private uint h245RangeMin = H323Provider.Consts.DefaultValues.H245RangeMin;
		public  uint H245RangeMin
		{
			get { return h245RangeMin; }
			set { h245RangeMin = value; }
		}

		private uint h245RangeMax = H323Provider.Consts.DefaultValues.H245RangeMin;
		public  uint H245RangeMax
		{
			get { return h245RangeMax; }
			set { h245RangeMax = value; }
		}

        #endregion

        #region Construction/Open/Close

        public H323IpcClient()
        {
            this.pendingMakeCalls = Hashtable.Synchronized(new Hashtable());

            this.ipcClient = new IpcFlatmapClient();
			this.ipcClient.onConnect          = new OnConnectDelegate(ipcClient_onConnect);
			this.ipcClient.onFlatmapMessageReceived = new OnFlatmapMessageReceivedDelegate(ipcClient_OnMessageReceived);
			this.ipcClient.onClose            = new OnCloseDelegate(ipcClient_onConnectionClosed);
        }

        /// <summary>Open an IPC connection to the H.323 service.</summary>
        /// <param name="ipEndpoint">Address of the H.323 service.</param>
        /// <returns>True if connected, false otherwise.</returns>
        public void Startup(IPEndPoint ipEndpoint)
        {
            if(ipcClient == null)
                throw new ObjectDisposedException(typeof(H323Provider).Name);

            if(ipEndpoint == null || ipEndpoint.Address == null)
                throw new Metreos.Core.StartupFailedException("Invalid H323 process address in H323IpcClient.Startup()");

            state = ProxyState.Connecting;
			active = true;
			ipcClient.RemoteEp = ipEndpoint;
			ipcClient.Start();
        }

        /// <summary>Close the IPC connection to the H.323 service.</summary>
        public void Close()
        {
			active = false;
            ipcClient.Close();
            pendingMakeCalls.Clear();
        }

		private bool active;

        #endregion

        #region IPC Client Callbacks

        /// <summary>The initial connection attempt to the H.323 service has succeeded.</summary>
        private void ipcClient_onConnect(IpcClient c, bool reconnect)
        {
			whineAboutClose = true;
			if (reconnect)
			{
				if(log != null)
					log.Write(TraceLevel.Info, "Reconnected to H.323 service");
				state = ProxyState.Connected;
				SendH323StartStackMessage();
				return;
			}
            log.Write(TraceLevel.Info, "Connected to H.323 service successfully");
            state = ProxyState.Connected;
            SendH323StartStackMessage();
        }

		private bool whineAboutClose = true;

        /// <summary>The connection to the H.323 service has been closed.</summary>
        private void ipcClient_onConnectionClosed(IpcClient c, Exception e)
        {
			if (whineAboutClose)
			{
				if(log != null)
					log.Write(TraceLevel.Warning, "Lost connection to H.323 service at {0}: {1}",
						ipcClient.RemoteEp,
						e != null ? e.Message : "(no msg)");
				whineAboutClose = false;
			}

            state = ProxyState.Disconnected;

            if(onServiceDown != null)
                onServiceDown();
			
            if (active)
				state = ProxyState.Connecting;
        }

        /// <summary>A message has been received from the H.323 service</summary>
        /// <param name="messageType">Type of message received.</param>
        /// <param name="flatmap">Flatmap containing message parameters.</param>
        private void ipcClient_OnMessageReceived(IpcFlatmapClient client, int messageType, FlatmapList flatmap)
        {
			try
			{
				switch((uint)messageType)
				{
					case Messages.IncomingCall:
						HandleIncomingCallMsg(flatmap);
						break;
					case Messages.GotCapabilities:
						HandleGotCapabilitiesMsg(flatmap);
						break;
					case Messages.CallEstablished:
						HandleCallEstablishedMsg(flatmap);
						break;
					case Messages.MediaEstablished:
						HandleMediaEstablishedMsg(flatmap);
						break;
					case Messages.MediaChanged:
						HandleMediaChangedMsg(flatmap);
						break;
					case Messages.GotDigits:
						HandleGotDigitsMsg(flatmap);
						break;
					case Messages.CallCleared:
						HandleCallClearedMsg(flatmap);
						break;
					case Messages.MakeCallAck:
						HandleMakeCallAckMsg(flatmap);
						break;
					case Messages.StartH323StackAck:
						HandleStartH323StackAckMsg(flatmap);
						break;
				}
			}
			catch(Exception e)
			{
				log.Write(TraceLevel.Error, "Exception thrown while handing H.323 stack message (Type={0}):\n{1}",
					messageType, e);
			}
        }

        #endregion

        #region Message Senders

        /// <summary>Start the H.323 stack.</summary>
        public void SendH323StartStackMessage()
        {
            FlatmapList startStackMsg = new FlatmapList();

            startStackMsg.Add(Params.EnableDebug, Convert.ToUInt32(enableDebug));
            startStackMsg.Add(Params.DebugLevel, debugLevel);
            startStackMsg.Add(Params.DebugFilename, debugFilename);
            startStackMsg.Add(Params.DisableFastStart, Convert.ToUInt32(disableFastStart));
            startStackMsg.Add(Params.DisableH245Tunneling, Convert.ToUInt32(disableH245Tun));
            startStackMsg.Add(Params.DisableH245InSetup, Convert.ToUInt32(disableH245InSetup));
            startStackMsg.Add(Params.ListenPort, listenPort);
            startStackMsg.Add(Params.H245PortBase, h245RangeMin);
            startStackMsg.Add(Params.H245PortMax, h245RangeMax);
			startStackMsg.Add(Params.MaxPendingCalls, maxPendingCalls);
            startStackMsg.Add(Params.TcpConnectTimeout, tcpConnectTimeout);
            startStackMsg.Add(Params.TransactionId, GenerateTransactionId());
            startStackMsg.Add(Params.ServiceLogLevel, serviceLogLevel);

            ipcClient.Write(Messages.StartH323Stack, startStackMsg);
        }


        /// <summary>Stop the H.323 stack.</summary>
        public void SendH323StopStackMessage()
        {
            FlatmapList stopStackMsg = new FlatmapList();
            stopStackMsg.Add(Params.TransactionId, GenerateTransactionId());

            if(ipcClient.Write(Messages.StopH323Stack, stopStackMsg))
                state = ProxyState.Connected;
        }

        public bool SendMakeCallMessage(long callId, string to, string from, 
            string displayName, string rxIp, uint rxPort, MediaCapsField caps)
        {
            FlatmapList msg = new FlatmapList();

            msg.Add(Params.CalledPartyNumber, to);
            msg.Add(Params.CallingPartyNumber, from);
            msg.Add(Params.DisplayName, displayName);

            if(rxIp != null && rxPort > 0)
            {
                msg.Add(Params.RxIp, rxIp);
                msg.Add(Params.RxPort, rxPort);
            }

            int transId = GenerateTransactionId();
            msg.Add(Params.TransactionId, transId);

            this.pendingMakeCalls[transId] = callId;

            AddMediaCapsToFlatMap(msg, caps);

            return ipcClient.Write(Messages.MakeCall, msg);
        }

        /// <summary>Send an accept call message to the H.323 stack.</summary>
        /// <param name="callId">Call identifier.</param>
        /// <param name="displayName">Display name for accepting party.</param>
        public void SendAcceptMessage(string callId, string displayName)
        {
            FlatmapList acceptMsg = new FlatmapList();
            
            acceptMsg.Add(Params.CallId, callId);
            acceptMsg.Add(Params.ShouldAcceptCall, 1);
            acceptMsg.Add(Params.DisplayName, displayName);

            ipcClient.Write(Messages.Accept, acceptMsg);
        }

        /// <summary>Send a reject call message to the H.323 stack.</summary>
        /// <param name="callId">Call identifier.</param>
        public void SendRejectMessage(string callId)
        {
            FlatmapList acceptMsg = new FlatmapList();
            
            acceptMsg.Add(Params.CallId, callId);
            acceptMsg.Add(Params.ShouldAcceptCall, 0);

            ipcClient.Write(Messages.Accept, acceptMsg);
        }

        /// <summary>Issues a set media command for a given call.</summary>
        /// <param name="callId">Call identifier to set media on.</param>
        /// <param name="rxIp">The IP address to receive media on. Use null for none.</param>
        /// <param name="rxPort">The port to receive media on. Use 0 for none.</param>
        /// <param name="txCodec">The transmit codec. Use null for none.</param>
        /// <param name="txFs">The transmit framesize. Use 0 for none.</param>
        /// <param name="localCaps">The local media capabilities. Use null for none.</param>
        public void SendSetMediaMessage(string callId, string rxIp, uint rxPort, 
            string txCodec, uint txFs, MediaCapsField localCaps)
        {
            if(callId == null || callId.Length == 0)
            {
                if(log != null)
                    log.Write(TraceLevel.Warning, "No callId in SendSetMediaMessage()");

                return;
            }

            FlatmapList setMediaMsg = new FlatmapList();
            
            setMediaMsg.Add(Params.CallId, callId);

            if(rxIp != null && rxIp.Length > 0)
                setMediaMsg.Add(Params.RxIp, rxIp);

            if(rxPort != 0)
                setMediaMsg.Add(Params.RxPort, rxPort);

            if(txCodec != null && txCodec.Length > 0)
                setMediaMsg.Add(Params.TxCodec, txCodec);

            if(txFs != 0)
                setMediaMsg.Add(Params.TxFramesize, txFs);

            if(localCaps != null)
                AddMediaCapsToFlatMap(setMediaMsg, localCaps);
            
            ipcClient.Write(Messages.SetMedia, setMediaMsg);
        }

        /// <summary>Answer a pending call.</summary>
        /// <param name="callId">Call identifier to answer.</param>
        /// <param name="displayName">The display name for this call.</param>
        public void SendAnswerMessage(string callId, string displayName)
        {
            FlatmapList answerMsg = new FlatmapList();
            answerMsg.Add(Params.CallId, callId);
            answerMsg.Add(Params.DisplayName, displayName);
            ipcClient.Write(Messages.Answer, answerMsg);
        }

        /// <summary>Send a reject call message to the H.323 stack.</summary>
        /// <param name="callId">Call identifier.</param>
        public void SendHoldMessage(string callId)
        {
            FlatmapList msg = new FlatmapList();
            msg.Add(Params.CallId, callId);

            ipcClient.Write(Messages.Hold, msg);
        }

        /// <summary>Issues a resume command for a given call.</summary>
        /// <param name="callId">Call identifier to set media on.</param>
        /// <param name="rxIp">The IP address to receive media on. Use null for none.</param>
        /// <param name="rxPort">The port to receive media on. Use 0 for none.</param>
        public void SendResumeMessage(string callId, string rxIp, uint rxPort)
        {
            if(callId == null || callId.Length == 0)
            {
                if(log != null)
                    log.Write(TraceLevel.Warning, "No callId in SendResumeMessage()");

                return;
            }

            FlatmapList msg = new FlatmapList();
            
            msg.Add(Params.CallId, callId);

            if(rxIp != null && rxIp.Length > 0)
                msg.Add(Params.RxIp, rxIp);

            if(rxPort != 0)
                msg.Add(Params.RxPort, rxPort);
            
            ipcClient.Write(Messages.Resume, msg);
        }

        public void SendUserInputMessage(string callId, string digits)
        {
            FlatmapList msg = new FlatmapList();
            msg.Add(Params.CallId, callId);
            msg.Add(Params.Digits, digits);
            ipcClient.Write(Messages.SendUserInput, msg);
        }

        /// <summary>Hangup an existing call.</summary>
        /// <param name="callId">Call identifier to hang up.</param>
        public void SendHangupMessage(string callId)
        {
            FlatmapList hangupMsg = new FlatmapList();
            hangupMsg.Add(Params.CallId, callId);
            ipcClient.Write(Messages.Hangup, hangupMsg);
        }

        #endregion

        #region Message Handlers

        /// <summary>Handle an incoming call event from the H.323 service.</summary>
        /// <param name="flatmap">Incoming call event parameters.</param>
        private void HandleIncomingCallMsg(FlatmapList flatmap)
        {
            Assertion.Check(onIncomingCall != null, "onIncomingCall delegate not hooked in H323IpcClient");

            string callId       = flatmap.Find(Params.CallId, 1).dataValue as String;
            string from         = flatmap.Find(Params.CallingPartyNumber, 1).dataValue as String;
            string to           = flatmap.Find(Params.CalledPartyNumber, 1).dataValue as String;
            string displayName  = flatmap.Find(Params.CallingPartyAlias, 1).dataValue as String;

            this.onIncomingCall(callId, from, to, displayName);
        }

        /// <summary>Handle a got capabilities event from the H.323 service.</summary>
        /// <param name="flatmap">The got capabilities event parameters.</param>
        private void HandleGotCapabilitiesMsg(FlatmapList flatmap)
        {
            Assertion.Check(onGotCapabilities != null, "onGotCapabilities delegate not hooked in H323IpcClient");

            string callId = flatmap.Find(Params.CallId, 1).dataValue as String;
            MediaCapsField caps = GetMediaCapsFromFlatmap(flatmap);

            this.onGotCapabilities(callId, caps);
        }

        /// <summary>Handle a call established message from the H.323 service.</summary>
        /// <param name="flatmap">The call established event parameters.</param>
        private void HandleCallEstablishedMsg(FlatmapList flatmap)
        {
            Assertion.Check(onCallEstablished != null, "onCallEstablished delegate not hooked in H323IpcClient");

            string callId = flatmap.Find(Params.CallId, 1).dataValue as String;
            string from   = flatmap.Find(Params.CallingPartyNumber, 1).dataValue as String;
            string to     = flatmap.Find(Params.CalledPartyNumber, 1).dataValue as String;

            this.onCallEstablished(callId, to, from);
        }

        /// <summary>Handle a media established message from the H.323 service.</summary>
        /// <param name="flatmap">The media established event parameters.</param>
        private void HandleMediaEstablishedMsg(FlatmapList flatmap)
        {
            Assertion.Check(onMediaEstablished != null, "onMediaEstablished delegate not hooked in H323IpcClient");

            string callId = Convert.ToString(flatmap.Find(Params.CallId, 1).dataValue);
            uint dir = Convert.ToUInt32(flatmap.Find(Params.Direction, 1).dataValue);

            IMediaControl.Codecs rxCodec;
            uint rxFramesize;
            H323Codecs h323RxCodec;
            
            if(GetRxCodec_TEMP(flatmap, out h323RxCodec, out rxCodec, out rxFramesize))
            {
                ConvertMediaCap(h323RxCodec, out rxCodec, out rxFramesize);
            }

            string txIP = Convert.ToString(flatmap.Find(Params.TxIp, 1).dataValue);
            uint txPort = Convert.ToUInt32(flatmap.Find(Params.TxPort, 1).dataValue);

            if(txIP == String.Empty)
                txIP = null;

            this.onMediaEstablished(callId, dir, txIP, txPort, rxCodec, rxFramesize);
        }

        /// <summary>Handle a media changed message from the H.323 service.</summary>
        /// <param name="flatmap">The media changed event parameters.</param>
        private void HandleMediaChangedMsg(FlatmapList flatmap)
        {
            Assertion.Check(onMediaChanged != null, "onMediaChanged delegate not hooked in H323IpcClient");
            
            string callId = flatmap.Find(Params.CallId, 1).dataValue as String;
            uint dir = Convert.ToUInt32(flatmap.Find(Params.Direction, 1).dataValue);

            IMediaControl.Codecs rxCodec;
            uint rxFramesize;
            H323Codecs h323RxCodec;

            if(GetRxCodec_TEMP(flatmap, out h323RxCodec, out rxCodec, out rxFramesize))
            {
                if(!ConvertMediaCap(h323RxCodec, out rxCodec, out rxFramesize) && dir == Direction.Receive)
                {
                    // Mask out close Rx channel messages
                    return;
                }
            }
            else if(dir == Direction.Receive && rxCodec == IMediaControl.Codecs.Unspecified)
            {
                // Mask out close Rx channel messages
                return;
            }

            string txIP = Convert.ToString(flatmap.Find(Params.TxIp, 1).dataValue);
            uint txPort = Convert.ToUInt32(flatmap.Find(Params.TxPort, 1).dataValue);

            this.onMediaChanged(callId, dir, txIP, txPort, rxCodec, rxFramesize);
        }

        /// <summary>
        /// Annoying method used until the stack is updated to use numbers for codecs
        /// </summary>
        /// <returns>Indicates the value found is an H323Codec value</returns>
        private bool GetRxCodec_TEMP(FlatmapList flatmap, out H323Codecs h323RxCodec, 
            out IMediaControl.Codecs rxCodec, out uint framesize)
        {
            h323RxCodec = H323Codecs.Unspecified;
            rxCodec = IMediaControl.Codecs.Unspecified;
            framesize = 0;

            try
            {
                h323RxCodec = (H323Codecs)Convert.ToUInt32(flatmap.Find(Params.RxCodec, 1).dataValue);
                return true;
            }
            catch
            {
                string rxCodecStr = flatmap.Find(Params.RxCodec, 1).dataValue as String;
                if(rxCodecStr != null)
                {
                    rxCodec = (IMediaControl.Codecs)
                        Enum.Parse(typeof(IMediaControl.Codecs), rxCodecStr, true);
                    framesize = Convert.ToUInt32(flatmap.Find(Params.RxFramesize, 1).dataValue);
                }
            }
            return false;
        }

        private void HandleGotDigitsMsg(FlatmapList flatmap)
        {
            Assertion.Check(onGotDigits != null, "onGotDigits delegate not hooked in H323IpcClient");
            
            string callId = Convert.ToString(flatmap.Find(Params.CallId, 1).dataValue);
            string digits = Convert.ToString(flatmap.Find(Params.Digits, 1).dataValue);

            this.onGotDigits(callId, digits);
        }

        /// <summary>Handle a call cleared message from the H.323 service.</summary>
        /// <param name="flatmap">The call cleared event parameters.</param>
        private void HandleCallClearedMsg(FlatmapList flatmap)
        {
            Assertion.Check(onCallCleared != null, "onCallCleared delegate not hooked in H323IpcClient");

            string callId   = Convert.ToString(flatmap.Find(Params.CallId, 1).dataValue);
            int endReason	= Convert.ToInt32(flatmap.Find(Params.CallEndReason, 1).dataValue);

            this.onCallCleared(callId, (ICallControl.EndReason)endReason);
        }

        private void HandleMakeCallAckMsg(FlatmapList flatmap)
        {
            string stackCallId = Convert.ToString(flatmap.Find(Params.CallId, 1).dataValue);
            int transId = Convert.ToInt32(flatmap.Find(Params.TransactionId, 1).dataValue);

			bool error = false;
			int errorCode = Convert.ToInt32(flatmap.Find(Params.ResultCode, 1).dataValue);
			if(errorCode != ResultCodes.Success)
				error = true;
				
            long callId = Convert.ToInt64(this.pendingMakeCalls[transId]);
            this.pendingMakeCalls.Remove(transId);

            this.onMakeCallAck(callId, stackCallId, error);
        }

        private void HandleStartH323StackAckMsg(FlatmapList flatmap)
        {
            state = ProxyState.Started;
        }

        #endregion

        #region Helpers

        private int GenerateTransactionId()
        {
            return Interlocked.Increment(ref nextTid); 
        }

        private MediaCapsField GetMediaCapsFromFlatmap(FlatmapList map)
        {
            if(map == null)
                return null;

            MediaCapsField caps = new MediaCapsField();
            for(int i=0; i<map.Count; i++)
            {
                Flatmap.MapEntry entry = map.GetAt(i);
                if(entry.key == Params.MediaCaps)
                {
                    IMediaControl.Codecs codec;
                    uint framesize = 0;

                    H323Codecs h323Codec = (H323Codecs)Convert.ToUInt32(entry.dataValue);
                    if(ConvertMediaCap(h323Codec, out codec, out framesize))
                    {
                        caps.Add(codec, framesize);
                    }
                }
            }

            return caps;
        }

        private bool ConvertMediaCap(H323Codecs h323Codec, out IMediaControl.Codecs asCodec, out uint framesize)
        {
            asCodec = IMediaControl.Codecs.Unspecified;
            framesize = 0;

            switch(h323Codec)
            {
                case H323Codecs.G711u30:
                    asCodec = IMediaControl.Codecs.G711u;
                    framesize = 30;
                    return true;
                case H323Codecs.G711u20:
                    asCodec = IMediaControl.Codecs.G711u;
                    framesize = 20;
                    return true;
                case H323Codecs.G711u10:
                    asCodec = IMediaControl.Codecs.G711u;
                    framesize = 10;
                    return true;
                case H323Codecs.G711a30:
                    asCodec = IMediaControl.Codecs.G711a;
                    framesize = 30;
                    return true;
                case H323Codecs.G711a20:
                    asCodec = IMediaControl.Codecs.G711a;
                    framesize = 20;
                    return true;
                case H323Codecs.G711a10:
                    asCodec = IMediaControl.Codecs.G711a;
                    framesize = 10;
                    return true;
                case H323Codecs.G729x40:
                    asCodec = IMediaControl.Codecs.G729;
                    framesize = 40;
                    return true;
                case H323Codecs.G729x30:
                    asCodec = IMediaControl.Codecs.G729;
                    framesize = 30;
                    return true;
                case H323Codecs.G729x20:
                    asCodec = IMediaControl.Codecs.G729;
                    framesize = 20;
                    return true;
                case H323Codecs.G723x30:
                    asCodec = IMediaControl.Codecs.G723;
                    framesize = 30;
                    return true;
                case H323Codecs.G723x60:
                    asCodec = IMediaControl.Codecs.G723;
                    framesize = 60;
                    return true;
            }
            return false;
        }

        /// <summary>Adds low-bitrate coder info to flatmap if present in the provided caps set.</summary>
        /// <param name="map">The map to add entries to.</param>
        /// <param name="caps">The MediaCapsField object to add to the flatmap.</param>
        private void AddMediaCapsToFlatMap(FlatmapList map, MediaCapsField caps)
        {
            if(map == null || caps == null)
                return;

            uint[] fss;

            #region G.711u
            if(caps.Contains(IMediaControl.Codecs.G711u))
            {
                fss = caps.GetFramesizes(IMediaControl.Codecs.G711u);
                foreach(uint fs in fss)
                {
                    switch(fs)
                    {
                        case 10:
                            map.Add(Params.MediaCaps, H323Codecs.G711u10);
                            break;

                        case 20:
                            map.Add(Params.MediaCaps, H323Codecs.G711u20);
                            break;

                        case 30:
                            map.Add(Params.MediaCaps, H323Codecs.G711u30);
                            break;
                    }
                }
            }
            #endregion

            #region G.711a
            if(caps.Contains(IMediaControl.Codecs.G711a))
            {
                fss = caps.GetFramesizes(IMediaControl.Codecs.G711a);
                foreach(uint fs in fss)
                {
                    switch(fs)
                    {
                        case 10:
                            map.Add(Params.MediaCaps, H323Codecs.G711a10);
                            break;

                        case 20:
                            map.Add(Params.MediaCaps, H323Codecs.G711a20);
                            break;

                        case 30:
                            map.Add(Params.MediaCaps, H323Codecs.G711a30);
                            break;
                    }
                }
            }
            #endregion

            #region G.729a
            if(caps.Contains(IMediaControl.Codecs.G729))
            {
                fss = caps.GetFramesizes(IMediaControl.Codecs.G729);
                foreach(uint fs in fss)
                {
                    switch(fs)
                    {
                        case 20:
                            map.Add(Params.MediaCaps, H323Codecs.G729x20);
                            break;

                        case 30:
                            map.Add(Params.MediaCaps, H323Codecs.G729x30);
                            break;

                        case 40:
                            map.Add(Params.MediaCaps, H323Codecs.G729x40);
                            break;
                    }
                }
            }
            #endregion

            #region G.723.1
//            if(caps.Contains(IMediaControl.Codecs.G723))
//            {
//                fss = caps.GetFramesizes(IMediaControl.Codecs.G723);
//                foreach(uint fs in fss)
//                {
//                    switch(fs)
//                    {
//                        case 30:
//                            map.Add(Params.MediaCaps, H323Codecs.G723x30);
//                            break;
//
//                        case 60:
//                            map.Add(Params.MediaCaps, H323Codecs.G723x60);
//                            break;
//                    }
//                }
//            }
            #endregion
        }

        #endregion
    }
}
