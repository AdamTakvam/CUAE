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
    public class IncomingCall_Load_Remote : FunctionalTestBase
    {
        protected const string To           = "7000";
        protected const string From         = "7500";
        private const int rigourousTimeout  = 5;
        private const uint NumCalls         = 20;

        private AutoResetEvent complete;
        private volatile bool error = false;

        public IncomingCall_Load_Remote() : base(typeof( IncomingCall_Load_Remote ))
        {
            this.complete = new AutoResetEvent(false);
            this.timeout = 60000;
        }

        public override bool Execute()
        {
            error = false;

            ccpClient.SendIncomingCall(0, "To", "From", "To", true);

            log.Write(System.Diagnostics.TraceLevel.Info, "Sent start signal to test CallControl provider");

            if(this.complete.WaitOne(timeout, false) == false)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Test timed out");
            }

            return !error;
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
                rxIp == null ? "<null>" : rxIp, rxPort, 
                rxControlIp == null ? "<null>" : rxControlIp, 
                rxControlPort, rxCodec, rxFramesize, txCodec, txFramesize));

            return false;
        }

        protected override bool AcceptCallReceived(long callId)
        {
            this.log.Write(System.Diagnostics.TraceLevel.Info, "Received AcceptCall action");
            return false;
        }

        protected override bool AnswerCallReceived(long callId)
        {
            this.log.Write(System.Diagnostics.TraceLevel.Info, "-- Test calls completed successfully --");

            this.error = false;
            this.complete.Set();

            return true;
        }

        protected override bool RejectCallReceived(long callId)
        {
            this.log.Write(System.Diagnostics.TraceLevel.Info, "-- Test failed --");

            this.error = true;
            this.complete.Set();

            return true;
        }

        protected override bool HangupCallReceived(long callId)
        {
            this.log.Write(System.Diagnostics.TraceLevel.Info, "Received Hangup action");
            return false;
        }

        protected override bool MakeCallReceived(long callId, string to, string from)
        {
            this.log.Write(System.Diagnostics.TraceLevel.Info, "Received MakeCall action");
            return false;
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