using System;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Core;
using Metreos.Core.ConfigData;
using Metreos.Core.ConfigData.CallRouteGroups;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.LoggingFramework;
using Metreos.ProviderFramework;
using Metreos.Messaging.MediaCaps;

namespace Metreos.CallControl.H323
{
    [ProviderDecl("H323 Call Control Provider")]
	public class H323Provider : CallControlProviderBase
	{
        #region Constants
        internal abstract class Consts
        {
			public const string DisplayName         = "H.323 Provider";

            public abstract class ConfigEntries
            {
                public const string ListenPort          = "Port";
                public const string EnableDebug         = "EnableStackDebugging";
                public const string DisableFastStart    = "DisableFastStart";		// Not exposed currently
                public const string DisableH245Tunneling= "DisableH245Tunneling";	// Not exposed currently
                public const string DisableH245InSetup  = "DisableH245InSetup";		// Not exposed currently
                public const string DebugLevel          = "StackDebuggingLogLevel";
                public const string DebugFilename       = "StackDebuggingLogFile";
                public const string H245RangeMin        = "H245RangeMin";
                public const string H245RangeMax		= "H245RangeMax";
				public const string MaxPendingCalls		= "MaxPendingCalls";
                public const string TcpConnectTimeout   = "TcpConnectTimeout";
                public const string ServiceLogLevel     = "ServiceLogLevel";

				public abstract class DisplayNames
				{
					public const string ListenPort			= "Listen Port";
					public const string DisableFastStart    = "Disable Fast Start";
					public const string DisableH245Tunneling= "Disable H.245 Tunneling";
					public const string DisableH245InSetup  = "Disable H.245 In Setup";
					public const string H245RangeMin        = "H.245 Range (min)";
					public const string H245RangeMax		= "H.245 Range (max)";
					public const string MaxPendingCalls		= "Max Pending Calls";
					public const string EnableDebug         = "Enable Stack Debugging";
					public const string DebugLevel          = "Stack Debugging Log Level";
					public const string DebugFilename       = "Stack Debugging Log File";
                    public const string TcpConnectTimeout   = "TCP Connect Timeout";
                    public const string ServiceLogLevel     = "H323 Service Log Level";
				}

				public abstract class Descriptions
				{
					public const string ListenPort			= "Port on which the stack should listen for incoming H.225 requests";
					public const string DisableFastStart    = "Disable Fast Start";
					public const string DisableH245Tunneling= "Disable H.245 Tunneling";
					public const string DisableH245InSetup  = "Disable H.245 In Setup";
					public const string H245RangeMin        = "H.245 port range (min)";
					public const string H245RangeMax		= "H.245 port range (max)";
					public const string MaxPendingCalls		= "Maximum number of pending calls allowed before stack starts auto-rejecting";
					public const string EnableDebug         = "Causes stack to write logs to a file directly, instead of via Metreos Log Server";
					public const string DebugLevel          = "Detail level of stack log messages. 0=Errors-only, 5=Verbose (if stack debugging is enabled)";
					public const string DebugFilename       = "Name of log file to create (if stack debugging is enabled)";
                    public const string TcpConnectTimeout   = "Number of seconds to attempt to contact a gateway before giving up. A lower number ensures faster failover";
                    public const string ServiceLogLevel     = "Detail level of service log messages. 0=Off, 1=Error, 2=Warning, 3=Info, 4=Verbose";
				}

				public abstract class Bounds
				{
					public const int ListenPortMin		    = 1024;
					public const int ListenPortMax		    = short.MaxValue;
					public const int H245RangeMinMin        = 1024;
					public const int H245RangeMinMax        = short.MaxValue;
					public const int H245RangeMaxMin	    = 1024;
					public const int H245RangeMaxMax	    = short.MaxValue;
					public const int MaxPendingCallsMin	    = 100;
					public const int MaxPendingCallsMax	    = 1000;
					public const int DebugLevelMin		    = 0;
					public const int DebugLevelMax		    = 5;
                    public const int TcpConnectTimeoutMin   = 1;
                    public const int TcpConnectTimeoutMax   = 10;
                    public const int ServiceLogLevelMin     = 0;
                    public const int ServiceLogLevelMax     = 4;
				}
			}

			public abstract class DefaultValues
			{
				public const bool EnableDebug           = false;
				public const int DebugLevel             = 3;
				public const string DebugFilename       = "H323StackLog.txt";
				public const bool DisableFastStart      = true;   
				public const bool DisableH245Tunneling  = true;
				public const bool DisableH245InSetup    = true;
				public const uint ListenPort            = 1720;
				public const uint H245RangeMin          = 10000;
				public const uint H245RangeMax          = 11000;
				public const uint MaxPendingCalls		= 100;
                public const int TcpConnectTimeout      = 2;
                public const int ServiceLogLevel        = 2;    // Warning
			}

			public const string StackHost           = "127.0.0.1";
            public const int StackIpcPort           = 8500;
            public const int ShutdownTimeoutMs      = 5000;

            public const int MorgueSize             = 100;

            
        }
        #endregion

        /// <summary>Reference to H.323 proxy via the H.323 flatmap proxy (proxies
        /// between us and the IPC client)</summary>
        private readonly H323IpcClient proxy;

		/// <summary>Map of AppServer call IDs to stack call IDs</summary>
        private readonly CallIdMap callMap;

		/// <summary>Table of pending outbound call metadata (for failover)</summary>
		private readonly PendingCallTable pendingCalls;

		private bool started = false;

        #region Construction/Initialization/Startup/Shutdown

		public H323Provider(IConfigUtility configUtility, ICallIdFactory callIdFactory)
            : base(typeof(H323Provider), Consts.DisplayName,
            IConfig.ComponentType.H323_Gateway, configUtility, callIdFactory)
		{
            this.callMap = new CallIdMap(Consts.MorgueSize);
			this.pendingCalls = new PendingCallTable();

            this.proxy = new H323IpcClient();
            this.proxy.Log = log;
            this.proxy.onServiceDown += new NullDelegate(OnServiceDown);
            this.proxy.onIncomingCall += new OnIncomingCallDelegate(OnIncomingCall);
            this.proxy.onMakeCallAck += new OnMakeCallAckDelegate(OnMakeCallAck);
            this.proxy.onCallCleared += new OnCallClearedDelegate(OnEndCall);
            this.proxy.onCallEstablished += new OnCallEstablishedDelegate(OnCallEstablished);
            this.proxy.onGotCapabilities += new OnGotCapabilitiesDelegate(OnGotCapabilities);
            this.proxy.onMediaEstablished += new OnMediaDelegate(OnMediaEstablished);
            this.proxy.onMediaChanged += new OnMediaDelegate(OnMediaChanged);
            this.proxy.onGotDigits += new OnGotDigitsDelegate(OnGotDigits);
		}

        protected override bool DeclareConfig(out ConfigEntry[] configItems, out Extension[] extensions)
        {
            configItems = new ConfigEntry[9];

            configItems[0] = new ConfigEntry(Consts.ConfigEntries.ListenPort, Consts.ConfigEntries.DisplayNames.ListenPort, 
				Consts.DefaultValues.ListenPort, Consts.ConfigEntries.Descriptions.ListenPort, 
				Consts.ConfigEntries.Bounds.ListenPortMin, Consts.ConfigEntries.Bounds.ListenPortMax, true);
			configItems[1] = new ConfigEntry(Consts.ConfigEntries.MaxPendingCalls, Consts.ConfigEntries.DisplayNames.MaxPendingCalls,
				Consts.DefaultValues.MaxPendingCalls, Consts.ConfigEntries.Descriptions.MaxPendingCalls,
				Consts.ConfigEntries.Bounds.MaxPendingCallsMin, Consts.ConfigEntries.Bounds.MaxPendingCallsMax, true);
            configItems[2] = new ConfigEntry(Consts.ConfigEntries.H245RangeMin, Consts.ConfigEntries.DisplayNames.H245RangeMin, 
				Consts.DefaultValues.H245RangeMin, Consts.ConfigEntries.Descriptions.H245RangeMin,
                Consts.ConfigEntries.Bounds.H245RangeMinMin, Consts.ConfigEntries.Bounds.H245RangeMinMax, true);
            configItems[3] = new ConfigEntry(Consts.ConfigEntries.H245RangeMax, Consts.ConfigEntries.DisplayNames.H245RangeMax, 
				Consts.DefaultValues.H245RangeMax, Consts.ConfigEntries.Descriptions.H245RangeMax,
                Consts.ConfigEntries.Bounds.H245RangeMaxMin, Consts.ConfigEntries.Bounds.H245RangeMaxMax, true);
            configItems[4] = new ConfigEntry(Consts.ConfigEntries.EnableDebug, Consts.ConfigEntries.DisplayNames.EnableDebug, 
				Consts.DefaultValues.EnableDebug, Consts.ConfigEntries.Descriptions.EnableDebug,
                IConfig.StandardFormat.Bool, true);
            configItems[5] = new ConfigEntry(Consts.ConfigEntries.DebugLevel, Consts.ConfigEntries.DisplayNames.DebugLevel,
                Consts.DefaultValues.DebugLevel, Consts.ConfigEntries.Descriptions.DebugLevel, 
				Consts.ConfigEntries.Bounds.DebugLevelMin, Consts.ConfigEntries.Bounds.DebugLevelMax, true);
            configItems[6] = new ConfigEntry(Consts.ConfigEntries.DebugFilename, Consts.ConfigEntries.DisplayNames.DebugFilename, 
				Consts.DefaultValues.DebugFilename, Consts.ConfigEntries.Descriptions.DebugFilename, 
				IConfig.StandardFormat.String, true);
            configItems[7] = new ConfigEntry(Consts.ConfigEntries.TcpConnectTimeout, Consts.ConfigEntries.DisplayNames.TcpConnectTimeout, 
                Consts.DefaultValues.TcpConnectTimeout, Consts.ConfigEntries.Descriptions.TcpConnectTimeout,
                Consts.ConfigEntries.Bounds.TcpConnectTimeoutMin, Consts.ConfigEntries.Bounds.TcpConnectTimeoutMax, true);
            configItems[8] = new ConfigEntry(Consts.ConfigEntries.ServiceLogLevel, Consts.ConfigEntries.DisplayNames.ServiceLogLevel, 
                Consts.DefaultValues.ServiceLogLevel, Consts.ConfigEntries.Descriptions.ServiceLogLevel,
                Consts.ConfigEntries.Bounds.ServiceLogLevelMin, Consts.ConfigEntries.Bounds.ServiceLogLevelMax, true);

            // No extensions
            extensions = null;

            return true;
        }

        protected override void RefreshConfiguration()
        {   
            // Get parameters
            proxy.ListenPort = Convert.ToUInt32(GetConfigValue(Consts.ConfigEntries.ListenPort));
			proxy.MaxPendingCalls = Convert.ToUInt32(GetConfigValue(Consts.ConfigEntries.MaxPendingCalls));
            proxy.H245RangeMin = Convert.ToUInt32(GetConfigValue(Consts.ConfigEntries.H245RangeMin));
            proxy.H245RangeMax = Convert.ToUInt32(GetConfigValue(Consts.ConfigEntries.H245RangeMax));
			proxy.EnableDebug = Convert.ToBoolean(GetConfigValue(Consts.ConfigEntries.EnableDebug));
			proxy.DebugLevel = Convert.ToUInt32(GetConfigValue(Consts.ConfigEntries.DebugLevel));
			proxy.DebugFilename = Convert.ToString(GetConfigValue(Consts.ConfigEntries.DebugFilename));
            proxy.TcpConnectTimeout = Convert.ToUInt32(GetConfigValue(Consts.ConfigEntries.TcpConnectTimeout));
            proxy.ServiceLogLevel = Convert.ToUInt32(GetConfigValue(Consts.ConfigEntries.ServiceLogLevel));
            //            proxy.DisableFastStart = Convert.ToBoolean(GetConfigValue(Consts.ConfigEntries.DisableFastStart));
			//            proxy.DisableH245Tunneling = Convert.ToBoolean(GetConfigValue(Consts.ConfigEntries.DisableH245Tunneling));
			//            proxy.DisableH245InSetup = Convert.ToBoolean(GetConfigValue(Consts.ConfigEntries.DisableH245InSetup));

			// Sanity check
            if (proxy.H245RangeMin < Consts.ConfigEntries.Bounds.H245RangeMinMin || 
				proxy.H245RangeMin > Consts.ConfigEntries.Bounds.H245RangeMinMax)
            {
                log.Write(TraceLevel.Warning, "Invalid Config: H245 Range Min = {0}. Resetting to default: {1}",
                    proxy.H245RangeMin, Consts.DefaultValues.H245RangeMin);
                proxy.H245RangeMin = Consts.DefaultValues.H245RangeMin;
            }
            if (proxy.H245RangeMax < Consts.ConfigEntries.Bounds.H245RangeMaxMin || 
				proxy.H245RangeMax > Consts.ConfigEntries.Bounds.H245RangeMaxMax)
            {
                log.Write(TraceLevel.Warning, "Invalid Config: H245 Range Max = {0}. Resetting to default: {1}",
                    proxy.H245RangeMax, Consts.DefaultValues.H245RangeMax);
                proxy.H245RangeMax = Consts.DefaultValues.H245RangeMax;
            }
            if(proxy.H245RangeMin > proxy.H245RangeMax)
            {
                log.Write(TraceLevel.Warning, "Invalid Config: H245 Range Min greater than H245 Range Max. Resetting to default: {0} - {1}",
                    Consts.DefaultValues.H245RangeMin, Consts.DefaultValues.H245RangeMax);
                proxy.H245RangeMin = Consts.DefaultValues.H245RangeMin;
                proxy.H245RangeMax = Consts.DefaultValues.H245RangeMax;
            }

			if (started)
	            StartStack(true);
        }

        protected override void OnStartup()
		{
			started = true;
            StartStack(false);

            // TODO: Wait for startup ACK

            RegisterNamespace();
        }

        protected override void OnShutdown()
        {
			started = false;

            if(proxy.State == ProxyState.Started)
            {
                HangupAllCalls();
                proxy.SendH323StopStackMessage();
            }

            proxy.Close();
        }

        private void HangupAllCalls()
        {
            lock(callMap.SyncRoot)
            {
                foreach(DictionaryEntry de in callMap)
                {
                    string stackCallId = de.Value as string;
                    if(stackCallId != null)
                        proxy.SendHangupMessage(stackCallId);
                }
                callMap.Clear();
				pendingCalls.Clear();
            }
        }

        private void OnServiceDown()
        {
			lock(callMap.SyncRoot)
			{
				foreach(DictionaryEntry de in callMap)
				{
					long callId = (long)de.Key;
					this.SendHangup(callId, ICallControl.EndReason.InternalError);
				}
				callMap.Clear();
				pendingCalls.Clear();
			}
        }

        #endregion
        
        #region Action Handlers

        protected override bool HandleAcceptCall(long callId, bool p2p)
        {
            string stackCallId = callMap[callId];
            if(stackCallId == null || stackCallId == String.Empty)
            {
                if(callMap.IsRecentlyEnded(callId))
                    log.Write(TraceLevel.Info, "An attempt has been made to accept a call which has already ended: " + callId);
                else
                    log.Write(TraceLevel.Warning, "Received AcceptCall action for unknown call: " + callId);

                return false;
            }

            proxy.SendAcceptMessage(stackCallId, "" /* DisplayName ?? */);
            return true;
        }

        protected override bool HandleAnswerCall(long callId, string displayName)
        {
            log.Write(TraceLevel.Info, "Display Name: {0}", displayName.Length > 0 ? displayName : "None Specified");

            string stackCallId = callMap[callId];
            if(stackCallId == null || stackCallId == String.Empty)
            {
                if(callMap.IsRecentlyEnded(callId))
                    log.Write(TraceLevel.Info, "An attempt has been made to answer a call which has already ended: " + callId);
                else
                    log.Write(TraceLevel.Warning, "Received AnswerCall action for unknown call: " + callId);
                return false;
            }

            proxy.SendAnswerMessage(stackCallId, displayName);
            return true;
        }

        protected override bool HandleRedirectCall(long callId, string to)
        {
            log.Write(TraceLevel.Error, "H.323 does not support Redirect");
            return false;
        }

        protected override bool HandleHangup(long callId)
        {
			this.pendingCalls.Remove(callId);

            string stackCallId = callMap[callId];
            if(stackCallId != null && stackCallId != String.Empty)
            {
                proxy.SendHangupMessage(stackCallId);
				this.callMap.Remove(callId);
            }
            return true;
        }

        protected override bool HandleMakeCall(OutboundCallInfo callInfo)
        {
            if(callInfo.IsPeerToPeer == true)
            {
                log.Write(TraceLevel.Error, "Call {0} failed: Peer-to-peer media is not supported in H.323",
                    callInfo.CallId);
                return false;
            }

			return MakeCall(callInfo, true);
        }

		internal bool MakeCall(OutboundCallInfo callInfo, bool initial)
		{
            if(callInfo == null)
            {
                log.Write(TraceLevel.Error, "Internal error: Outbound call metadata is null");
                return false;
            }

            H323GW gw = callInfo.RouteGroup.GetNextMember() as H323GW;

            if(gw != null && gw.Address != null)
            {
				string fullTo = String.Format("{0}@{1}", callInfo.To, gw.Address);
                string rxIP = null;
                uint rxPort = 0;

                if(callInfo.RxAddr != null)
                {
                    rxIP = callInfo.RxAddr.Address.ToString();
                    rxPort = (uint)callInfo.RxAddr.Port;
                }

                if(proxy.SendMakeCallMessage(callInfo.CallId, fullTo, callInfo.From, callInfo.DisplayName, 
                    rxIP, rxPort, callInfo.Caps))
                {
                    if(initial)
                        log.Write(TraceLevel.Info, "Placing call {0} to: {1}", callInfo.CallId, fullTo);
					
                    this.pendingCalls.Add(callInfo);
                    return true;
                }
                else
                    log.Write(TraceLevel.Error, "Call {0} to {1} failed: Cannot connect to H.323 service.", 
                        callInfo.CallId, fullTo);
			}
			else if(initial)
				log.Write(TraceLevel.Error, "No servers in CRG for call {0} to {1}", callInfo.CallId, callInfo.To);
			else
				log.Write(TraceLevel.Warning, "No more servers in CRG to failover to for call {0} to {1}", 
					callInfo.CallId, callInfo.To);

			return false;
		}

        protected override bool HandleRejectCall(long callId)
        {
            string stackCallId = callMap[callId];
            if(stackCallId == null || stackCallId == String.Empty)
            {
                if(callMap.IsRecentlyEnded(callId) == false)
                    log.Write(TraceLevel.Warning, "Received RejectCall action for unknown call: " + callId);
                return false;
            }

            proxy.SendRejectMessage(stackCallId);
            return true;
        }

        protected override bool HandleSetMedia(long callId, string rxIP, uint rxPort, string rxControlIP, 
            uint rxControlPort, Metreos.Interfaces.IMediaControl.Codecs rxCodec, uint rxFramesize, 
            Metreos.Interfaces.IMediaControl.Codecs txCodec, uint txFramesize, MediaCapsField caps)
        {
			log.Write(TraceLevel.Info,
				"SetMedia({0},rx:{1}:{2},tx:{3},{4},{{{5}}})",
				callId, rxIP, rxPort, txCodec, txFramesize,
				caps == null ? "" : caps.ToString().Replace('\n', ',').Replace(" ", ""));

            string stackCallId = callMap[callId];
            if(stackCallId == null || stackCallId == String.Empty)
            {
                if(callMap.IsRecentlyEnded(callId))
                    return true;

                log.Write(TraceLevel.Warning, "Received SetMedia action for unknown call: " + callId);
                return false;
            }

            proxy.SendSetMediaMessage(stackCallId, rxIP, rxPort, txCodec.ToString(), txFramesize, caps);
            return true;
        }

        protected override bool HandleHold(long callId)
        {
            string stackCallId = callMap[callId];
            if(stackCallId == null || stackCallId == String.Empty)
            {
                if(callMap.IsRecentlyEnded(callId))
                    log.Write(TraceLevel.Info, "An attempt has been made to hold a call which has already ended: " + callId);
                else
                    log.Write(TraceLevel.Warning, "Received Hold action for unknown call: " + callId);
                return false;
            }

            proxy.SendHoldMessage(stackCallId);
            return true;
        }

        protected override bool HandleResume(long callId, string rxIP, uint rxPort, string rxControlIP, uint rxControlPort)
        {
            string stackCallId = callMap[callId];
            if(stackCallId == null || stackCallId == String.Empty)
            {
                if(callMap.IsRecentlyEnded(callId))
                    log.Write(TraceLevel.Info, "An attempt has been made to resume a call which has already ended: " + callId);
                else
                    log.Write(TraceLevel.Warning, "Received Resume action for unknown call: " + callId);
                return false;
            }

            proxy.SendResumeMessage(stackCallId, rxIP, rxPort);
            return true;
        }

        protected override bool HandleUseMohMedia(long callId)
        {
            string stackCallId = callMap[callId];
            if(stackCallId == null || stackCallId == String.Empty)
            {
                if(callMap.IsRecentlyEnded(callId))
                    return true;

                log.Write(TraceLevel.Warning, "Received UseMohMedia action for unknown call: " + callId);
                return false;
            }

            return true;
        }

        protected override void HandleNoHandler(ActionMessage noHandlerAction, EventMessage originalEvent)
        {
            log.Write(TraceLevel.Warning, "{0} event was not handled", originalEvent.MessageId); 

            if(originalEvent.MessageId == ICallControl.Events.INCOMING_CALL)
            {
                long callId = Convert.ToInt64(originalEvent[ICallControl.Fields.CALL_ID]);
                proxy.SendRejectMessage(callMap[callId]);
                callMap.Remove(callId);
            }
        }

        protected override bool HandleSendUserInput(long callId, string digits)
        {
            string stackCallId = callMap[callId];
            if(stackCallId == null || stackCallId == String.Empty)
            {
                if(callMap.IsRecentlyEnded(callId))
                    log.Write(TraceLevel.Info, "An attempt has been made to send digits on a call which has already ended: " + callId);
                else
                    log.Write(TraceLevel.Warning, "Received SendUserInput action for unknown call: " + callId);
                return false;
            }

            proxy.SendUserInputMessage(stackCallId, digits);
            return true;
        }


        #endregion

        #region Event Senders
        private void OnIncomingCall(string stackCallId, string from, string to, string displayName)
        {
            long callId = this.callIdFactory.GenerateCallId();
            callMap.Add(callId, stackCallId);

            log.Write(TraceLevel.Info, "Incoming call: {0} = {1}", callId, stackCallId);

            base.SendIncomingCall(callId, stackCallId, to, from, to, true, displayName);
        }

        private void OnGotCapabilities(string stackCallId, MediaCapsField caps)
        {
            long callId = callMap[stackCallId];
            if(callId == 0)
            {
                if(callMap.IsRecentlyEnded(stackCallId) == false)
                {
                    log.Write(TraceLevel.Warning, "Received GotCapabilities event for unknown call: " + stackCallId);
                    proxy.SendHangupMessage(stackCallId);
                }
                return;
            }

            base.SendGotCapabilities(callId, caps);
        }

        private void OnMakeCallAck(long callId, string stackCallId, bool fatalError)
        {
			if(fatalError)
			{
				this.pendingCalls.Remove(callId);
				base.SendCallSetupFailed(callId, ICallControl.EndReason.InternalError);
			}
			else
			{
				callMap.Add(callId, stackCallId);
			}

            log.Write(TraceLevel.Info, "Initiated call: {0} = {1}", callId, stackCallId);
        }

        private void OnCallEstablished(string stackCallId, string to, string from)
        {
            long callId = callMap[stackCallId];
            if(callId == 0)
            {
                if(callMap.IsRecentlyEnded(stackCallId) == false)
                {
                    log.Write(TraceLevel.Warning, "Received CallEstablished event for unknown call: " + stackCallId);
                    proxy.SendHangupMessage(stackCallId);
                }
                return;
            }

			this.pendingCalls.Remove(callId);

            base.SendCallEstablished(callId, to, from);
        }

        private void OnMediaEstablished(string stackCallId, uint direction, string txIP, 
            uint txPort, IMediaControl.Codecs rxCodec, uint rxFramesize)
        {
            long callId = callMap[stackCallId];
            if(callId == 0)
            {
                if(callMap.IsRecentlyEnded(stackCallId) == false)
                {
                    log.Write(TraceLevel.Warning, "Received MediaEstablished event for unknown call: " + stackCallId);
                    proxy.SendHangupMessage(stackCallId);
                }
                return;
            }

            log.Write(TraceLevel.Verbose,
                "OnMediaEstablished({0}=>{1},{2}{3},tx:{4}:{5},rx:{6},{7})",
                stackCallId, callId,
                (direction & Direction.Receive) > 0 ? "Receive" : "",
                (direction & Direction.Transmit) > 0 ? "Transmit" : "",
                txIP, txPort, rxCodec.ToString(), rxFramesize);

            base.SendMediaEstablished(callId, txIP, txPort, txIP, txPort, rxCodec, rxFramesize, 
                IMediaControl.Codecs.Unspecified, 0);
        }

        private void OnMediaChanged(string stackCallId, uint direction, string txIP, 
            uint txPort, IMediaControl.Codecs rxCodec, uint rxFramesize)
        {
            long callId = callMap[stackCallId];
            if(callId == 0)
            {
                if(callMap.IsRecentlyEnded(stackCallId) == false)
                {
                    log.Write(TraceLevel.Warning, "Received MediaChanged event for unknown call: " + stackCallId);
                    proxy.SendHangupMessage(stackCallId);
                }
                return;
            }

            // Log this
            if((direction & Direction.Receive) > 0)
                log.Write(TraceLevel.Info, "Media Changed: Rx Codec={0}:{1}", rxCodec.ToString(), rxFramesize);
            if((direction & Direction.Transmit) > 0)
                log.Write(TraceLevel.Info, "Media Changed: Tx Addr={0}:{1}", txIP, txPort);

            base.SendMediaChanged(callId, txIP, txPort, txIP, txPort, rxCodec, rxFramesize, 
                IMediaControl.Codecs.Unspecified, 0);       
        }

        private void OnGotDigits(string stackCallId, string digits)
        {
            long callId = callMap[stackCallId];
            if(callId == 0)
            {
                if(callMap.IsRecentlyEnded(stackCallId) == false)
                {
                    log.Write(TraceLevel.Warning, "Received GotDigits event for unknown call: " + stackCallId);
                    proxy.SendHangupMessage(stackCallId);
                }
                return;
            }

            base.SendGotDigits(callId, digits);
        }

        private void OnEndCall(string stackCallId, ICallControl.EndReason reason)
        {
            long callId = callMap[stackCallId];
            if(callId > 0)
            {
				// InternalError indicates that we should attempt failover
				//   if this is a pending outbound call
                bool failingOver = false;
				if(reason == ICallControl.EndReason.InternalError)
				{
					OutboundCallInfo callInfo = this.pendingCalls[callId];
					if(callInfo != null)
					{
                        if(MakeCall(callInfo, false))
                            failingOver = true;
					}
				}
                
                this.callMap.Remove(callId);
                this.pendingCalls.Remove(callId);

                if(!failingOver)
                    base.SendHangup(callId, reason);
            }
        }

        #endregion

        #region Private helper methods

        private void StartStack(bool refresh)
        {
            switch(proxy.State)
            {
                case ProxyState.Connecting:
                    proxy.Close();
                    IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(Consts.StackHost), Consts.StackIpcPort);
                    proxy.Startup(ipe);
                    break;
                case ProxyState.Disconnected:
                    IPEndPoint ipe2 = new IPEndPoint(IPAddress.Parse(Consts.StackHost), Consts.StackIpcPort); 
                    proxy.Startup(ipe2);
                    break;
                case ProxyState.Connected:
                    proxy.SendH323StartStackMessage();
                    break;
                case ProxyState.Started:
                    if(refresh)
                    {
                        proxy.SendH323StopStackMessage();
                        proxy.SendH323StartStackMessage();
                    }
                    break;
            }
        }

        #endregion
    }
}
