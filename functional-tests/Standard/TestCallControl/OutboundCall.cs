using System;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using MakeCallActionTest = Metreos.TestBank.Provider.Provider.MakeCallAction;

namespace Metreos.FunctionalTests.TestControlProvider.IncomingCallEvent
{
	/// <summary>Installs an application, and waits on one signal.</summary>
	[Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
	public class MakeCallAction : FunctionalTestBase
	{
        protected long CallId           = 0;
        protected const string To       = "1002";
        protected const string From     = "1000";
        private const int loopCount = 100;
        private const int rigourousTimeout = 5;

        protected AutoResetEvent gotMakeCall;
        protected AutoResetEvent hungUp;
        protected bool signallingFailure;
        protected bool success;

		public MakeCallAction() : base(typeof( MakeCallAction ))
        {
            this.timeout = 10000;
            gotMakeCall = new AutoResetEvent(false);
            hungUp = new AutoResetEvent(false);
            signallingFailure = false;
            success = false;
        }

        public override bool Execute()
        {
            TriggerScript(MakeCallActionTest.script1.FullName);

            bool gotMakeCallActionSignal = gotMakeCall.WaitOne(timeout, false);
            if(gotMakeCallActionSignal == false)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a MakeCall action");
                return false;
            }

            Thread.Sleep(100);
            
            ccpClient.SendGotCapabilities(CallId);

            Thread.Sleep(500);

            if(!WaitForSignal( MakeCallActionTest.script1.S_Simple.FullName, rigourousTimeout ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a compare-to-callId result signal from after the MakeCall action");
                return false;
            }

            if(!success)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The callId specified in the MakeCall provider action did not route back to the application");
                return false;
            }

            ccpClient.SendCallEstablished(CallId, To, From);

            Thread.Sleep(100);

            ccpClient.SendMediaEstablished(CallId, "10.1.10.155", 3998, "10.1.10.155", 3999, IMediaControl.Codecs.G711u, 20, IMediaControl.Codecs.G711u, 20);

            if(!WaitForSignal( MakeCallActionTest.script1.S_Simple.FullName, rigourousTimeout ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The MakeCall_Complete event not signaled");
            }

            if(hungUp.WaitOne(15000, false) == false)
            {
                this.log.Write(System.Diagnostics.TraceLevel.Info, "Error: Call was not hung up");
                return false;
            }

            Thread.Sleep(500);

            return true;
        }

        protected void SimpleMessage(ActionMessage action)
        {
            success = true;
        }
        
        protected void FailedMessage(ActionMessage action)
        {
            success = false;
        }

        protected override bool MakeCallReceived(long callId, string to, string from)
        {
            if(to != To || from != From)
            {
                signallingFailure = true;
            }

            this.CallId = callId;

            gotMakeCall.Set();

            return true;
        }

        protected override bool SetMediaReceived(long callId, string rxIp, uint rxPort, string rxControlIp, 
            uint rxControlPort, IMediaControl.Codecs rxCodec, uint rxFramesize, IMediaControl.Codecs txCodec, uint txFramesize)
        {
            this.log.Write(System.Diagnostics.TraceLevel.Info, String.Format("Received SetMedia action (rxIP={0}, rxPort={1}, rxCtrlIP={2}, rxCtrlPort={3}, rxCodec={4}, rxFrame={5}, txCodec={6}, txFrame={7})",
                rxIp, rxPort, rxControlIp, rxControlPort, rxCodec, rxFramesize, txCodec, txFramesize));
            return true;
        }

        protected override bool HangupCallReceived(long callId)
        {
            this.log.Write(System.Diagnostics.TraceLevel.Info, "Received Hangup action");
            this.hungUp.Set();
            return true;
        }

        public override void Initialize()
        {
            signallingFailure = false;
        }

        public override void Cleanup()
        {
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( MakeCallActionTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { new CallbackLink( MakeCallActionTest.script1.S_Simple.FullName , new FunctionalTestSignalDelegate(SimpleMessage)),
                                          new CallbackLink( MakeCallActionTest.script1.S_Failed.FullName , new FunctionalTestSignalDelegate(FailedMessage))};
        }
	} 
}
