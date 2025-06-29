using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Threading;

using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.Messaging.MediaCaps;
using Metreos.Core.ConfigData.CallRouteGroups;
using Metreos.Core.ConfigData;
using Metreos.Core.IPC;
using Metreos.Core;
using Metreos.ProviderFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.TestCallControl.Communication;

namespace Metreos.Providers.TestCallControl
{
    public delegate bool SetMediaReceivedDelegate(long callId, string rxIp, uint rxPort, string rxControlIp, 
        uint rxControlPort, IMediaControl.Codecs rxCodec, uint rxFramesize, IMediaControl.Codecs txCodec, uint txFramesize);
    public delegate bool AcceptCallReceivedDelegate(long callId);
    public delegate bool AnswerCallReceivedDelegate(long callId);
    public delegate bool RejectCallReceivedDelegate(long callId);
    public delegate bool HangupCallReceivedDelegate(long callId);
    public delegate bool MakeCallReceivedDelegate(long callId, string to, string from, string displayName, MediaCapsField mediaCaps);

	/// <summary> 
	///     A call control provider which proxies information to and from
	///     the Functional Test Framework
    /// </summary>
    [ProviderDecl("TestCallControl Provider")]
    [PackageDecl("Metreos.Providers.TestCallControl", "Test provider to serve as a tester for the Call Control/Telephony Manager code")]
	public class CallControlProvider : CallControlProviderBase 
	{
        public const TraceLevel logLevel    = TraceLevel.Info;
        public const string ProviderName    = "TestCallControl";
        public const string Namespace       = "Metreos.Providers.TestCC";
        public const string Description     = "A thin implementation of the Call Control Base to test Call Control";

        public const string AcceptCall      = "AcceptCallMessage";
        public const string AnswerCall      = "AnswerCall";
        public const string HangupCall      = "HangupCall";
        public const string MakeCall        = "MakeCall";
        public const string RejectCall      = "RejectCall";
        public const string SetMedia        = "SetMedia";
		public const string HandleRedirect  = "HandleRedirectCall";

        // Load tester stuff
        bool loadTestMode = false;
        IncomingCall_H323_Load loadTester;
        public SetMediaReceivedDelegate OnSetMediaReceived;
        public AcceptCallReceivedDelegate OnAcceptCallReceived;
        public AnswerCallReceivedDelegate OnAnswerCallReceived;
        public RejectCallReceivedDelegate OnRejectCallReceived;
        public HangupCallReceivedDelegate OnHangupCallReceived;
        public MakeCallReceivedDelegate OnMakeCallReceived;

        protected AutoResetEvent acceptCallBlock;
        protected AutoResetEvent answerCallBlock;
        protected AutoResetEvent hangupBlock;
        protected AutoResetEvent makeCallBlock;
        protected AutoResetEvent rejectCallBlock;
        protected AutoResetEvent setMediaBlock;

        protected Hashtable outstandingMessages;
        protected ServerIpcInterface server;

        public CallControlProvider(IConfigUtility config, ICallIdFactory callIdFactory) :
            base(typeof(CallControlProvider), "CallControl Test Provider", IConfig.ComponentType.Test, 
            config, callIdFactory)
		{
            this.loadTester = new IncomingCall_H323_Load(this);

            this.outstandingMessages            = new Hashtable();
            this.server = new ServerIpcInterface(ICallControlTest.ListenPort);
            this.server.CallEstablishedRequest += new SendCallEstablishedDelegate(CallEstablishedRequest);
            this.server.CallSetupFailedRequest += new SendCallSetupFailedDelegate(CallSetupFailedRequest);
            this.server.GotCapabilitiesRequest += new SendGotCapabilitiesDelegate(GotCapabilitiesRequest);
            this.server.GotDigitsRequest += new SendGotDigitsDelegate(GotDigitsRequest);
            this.server.HangupRequest += new SendHangupDelegate(HangupRequest);
            this.server.IncomingCallRequest += new SendIncomingCallDelegate(IncomingCallRequest);
            this.server.MediaEstablishedRequest += new SendMediaEstablishedDelegate(MediaEstablishedRequest);
            this.server.MediaChangedRequest += new SendMediaChangedDelegate(MediaChangedRequest);
        }

        protected override bool DeclareConfig(out ConfigEntry[] configItems, out Extension[] extensions)
        {
            configItems = null;
            extensions = null;
            return true;
        }

        protected override void HandleNoHandler(ActionMessage noHandlerAction, EventMessage originalEvent)
        {
            return;
        }

        protected override void OnStartup()
        {
            this.server.Start();
            this.RegisterNamespace();
        }

        protected override void OnShutdown()
        {
            this.server.Stop();
        }

        protected override void RefreshConfiguration()
        {
            
        }

        protected override bool HandleAcceptCall(long callId, bool p2p)
        {
            if(this.loadTestMode == true)
            {
                return loadTester.AcceptCallReceived(callId);
            }
            else
            {
                return server.SendAcceptCall(callId);
            }
        }

        protected override bool HandleAnswerCall(long callId, string displayName)
        {
            if(this.loadTestMode == true)
            {
                return loadTester.AnswerCallReceived(callId);
            }
            else
            {
                return server.SendAnswerCall(callId);
            }
        }

        protected override bool HandleHangup(long callId)
        {
            if(this.loadTestMode == true)
            {
                return loadTester.HangupCallReceived(callId);
            }
            else
            {
                return server.SendHangup(callId);
            }
        }

        protected override bool HandleMakeCall(OutboundCallInfo callInfo)
        {
            // ignoring p2p
            if(this.loadTestMode == true)
            {
                return loadTester.MakeCallReceived(callInfo.CallId, callInfo.To, callInfo.From, 
                    callInfo.DisplayName, callInfo.Caps);
            }
            else
            {
                return server.SendMakeCall(callInfo.CallId, callInfo.To, callInfo.From, 
                    callInfo.DisplayName, callInfo.Caps);
            }
        }

        protected override bool HandleRejectCall(long callId)
        {
            if(this.loadTestMode == true)
            {
                return loadTester.RejectCallReceived(callId);
            }
            else
            {                
                return server.SendRejectCall(callId);
            }
        }

        protected override bool HandleSetMedia(
            long callId, string rxIp, 
            uint rxPort, string rxControlIp, 
            uint rxControlPort, IMediaControl.Codecs rxCodec, 
            uint rxFramesize, IMediaControl.Codecs txCodec, uint txFramesize,
            MediaCapsField caps)
        {
            if(this.loadTestMode == true)
            {
                return loadTester.SetMediaReceived(callId, rxIp, rxPort, rxControlIp, rxControlPort, rxCodec, rxFramesize, txCodec, txFramesize);
            }
            else
            {
                return server.SendSetMedia(callId, rxIp, rxPort, rxControlIp, rxControlPort, rxCodec, rxFramesize, txCodec, txFramesize);
            }
        }        
  
        protected override bool HandleHold(long callId)
        {
            // TODO IMPLEMENT
            return false;
        }

        protected override bool HandleResume(long callId, string rxIP, uint rxPort, string rxControlIP, uint rxControlPort)
        {
            // TODO IMPLEMENT
            return false;
        }


		protected override bool HandleRedirectCall(
			long callId, string to)
		{
			// TODO IMPLEMENT
			return false;
		}        

        public void CallEstablishedRequest(int transactionId, long callId, string to, string from)
        {
            SendCallEstablished(callId, to, from);
        }

        public void CallSetupFailedRequest(int transactionId, long callId, string reason)
        {
            SendCallSetupFailed(callId, (ICallControl.EndReason) Enum.Parse(typeof(ICallControl.EndReason), reason, true));
        }

        public void GotCapabilitiesRequest(int transactionId, long callId, string txIp, uint txPort, string txControlIp, uint txControlPort,
            Metreos.Messaging.MediaCaps.MediaCapsField field)
        {
            SendGotCapabilities(callId, field);
        }

        public void GotDigitsRequest(int transactionId, long callId, string digits)
        {
            SendGotDigits(callId, digits);
        }

        public void HangupRequest(int transactionId, long callId)
        {
            SendHangup(callId);
        }

        public void IncomingCallRequest(int transactionId, long callId, string to, string from, 
            string originalTo, bool loadTest, bool negCaps)
        {
            if(this.loadTestMode == false && loadTest == true)
            {
                log.Write(TraceLevel.Info, "Got start signal. Beginning test");

                this.loadTestMode = true;
                if(loadTester.Execute() == false)
                {
                    log.Write(TraceLevel.Error, "Load test failed");
                    server.SendRejectCall(callId);
                }
                else
                {
                    log.Write(TraceLevel.Info, "Load test succeeded");
                    server.SendAnswerCall(callId);
                }
                this.loadTestMode = false;
            }
            else
            {
                SendIncomingCall(callId, callId.ToString(), to, from, originalTo, negCaps, from);
            }
        }

        public void MediaEstablishedRequest(int transactionId, long callId, string txIp, uint txPort, string txControlIp, 
            uint txControlPort, uint rxCodec, uint rxFramesize, uint txCodec, uint txFramesize)
        {
            SendMediaEstablished(callId, txIp, txPort, txControlIp, txControlPort, (IMediaControl.Codecs)rxCodec, rxFramesize, (IMediaControl.Codecs)txCodec, txFramesize);
        }

        private void MediaChangedRequest(int transactionId, long callId, string txIP, uint txPort)
        {
            SendMediaChanged(callId, txIP, txPort);
        }
    }
}
