using System;
using System.Threading;
using System.Diagnostics;
using System.Collections;

using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Messaging.MediaCaps;

using Metreos.Samoa.FunctionalTestFramework;

using IncomingCallEventTest = Metreos.TestBank.Provider.Provider.IncomingCallEvent;

namespace Metreos.FunctionalTests.TestControlProvider.IncomingCallEvent
{
	/// <summary>Installs an application, and waits on one signal.</summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
	public class IncomingCall_SIP : FunctionalTestBase
	{
        protected long CallId               = 0;
        protected const string To           = "7000";
        protected const string From         = "7500";
        private const int loopCount         = 100;
        private const int rigourousTimeout  = 5;

        private AutoResetEvent callAnswered;
        private AutoResetEvent hungup;

		public IncomingCall_SIP() : base(typeof( IncomingCall_SIP ))
        {
            this.timeout = 3000;
            this.callAnswered = new AutoResetEvent(false);
            this.hungup = new AutoResetEvent(false);
        }

        public override bool Execute()
        {
            Thread.Sleep(1000);

            this.ccpClient.SendIncomingCall(CallId, To, From, To);

            Thread.Sleep(100);

            MediaCapsField caps = new MediaCapsField();
            caps.Add(IMediaControl.Codecs.G711u, 20, 30);
            this.ccpClient.SendGotCapabilities(CallId, "10.1.10.155", 3998, "10.1.10.155", 3999, caps);

            if(callAnswered.WaitOne(15000, false) == false)
            {
                this.log.Write(System.Diagnostics.TraceLevel.Info, "Error: Call was not answered");
                return false;
            }

            Thread.Sleep(100);

            ccpClient.SendCallEstablished(CallId, To, From);

            Thread.Sleep(100);

            ccpClient.SendMediaEstablished(CallId, "10.1.10.155", 3998, "10.1.10.155", 3999, IMediaControl.Codecs.G711u, 20, IMediaControl.Codecs.G711u, 20);

            if(hungup.WaitOne(15000, false) == false)
            {
                this.log.Write(System.Diagnostics.TraceLevel.Info, "Error: Call was not hung up");
                return false;
            }

            Thread.Sleep(500);

            return true;
        }

        public override void Initialize()
        {
        }

        public override void Cleanup()
        {
        }

        protected override bool SetMediaReceived(long callId, string rxIp, uint rxPort, string rxControlIp, 
            uint rxControlPort, IMediaControl.Codecs rxCodec, uint rxFramesize, IMediaControl.Codecs txCodec, uint txFramesize)
        {
            this.log.Write(System.Diagnostics.TraceLevel.Info, String.Format("Received SetMedia action (rxIP={0}, rxPort={1}, rxCtrlIP={2}, rxCtrlPort={3}, rxCodec={4}, rxFrame={5}, txCodec={6}, txFrame={7})",
                rxIp, rxPort, rxControlIp, rxControlPort, rxCodec, rxFramesize, txCodec, txFramesize));
            return true;
        }

        protected override bool AcceptCallReceived(long callId)
        {
            this.log.Write(System.Diagnostics.TraceLevel.Info, "Received AcceptCall action");
            return true;
        }

        protected override bool AnswerCallReceived(long callId)
        {
            this.log.Write(System.Diagnostics.TraceLevel.Info, "Received AnswerCall action");
            this.callAnswered.Set();
            return true;
        }

        protected override bool RejectCallReceived(long callId)
        {
            this.log.Write(System.Diagnostics.TraceLevel.Info, "Received RejectCall action");
            return true;
        }

        protected override bool HangupCallReceived(long callId)
        {
            this.log.Write(System.Diagnostics.TraceLevel.Info, "Received Hangup action");
            this.hungup.Set();
            return true;
        }

        protected override bool MakeCallReceived(long callId, string to, string from)
        {
            this.CallId = callId;

            this.log.Write(System.Diagnostics.TraceLevel.Info, "Received MakeCall action");
            return true;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( IncomingCallEventTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( IncomingCallEventTest.script1.S_Simple.FullName , null )};
        }
	} 
}