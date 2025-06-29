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
    [FunctionalTestImpl(IsAutomated=true)]
	public class MakeCallAction : FunctionalTestBase
	{
        protected const string CallId   = "2000";
        protected const string To       = "1002";
        protected const string From     = "1000";
        private const int loopCount = 100;
        private const int rigourousTimeout = 5;
        protected AutoResetEvent are;
        protected bool signallingFailure;
        protected bool success;

		public MakeCallAction() : base(typeof( MakeCallAction ))
        {
            this.timeout = 10000;
            are = new AutoResetEvent(false);
            signallingFailure = false;
            success = false;
        }

        public override bool Execute()
        {
            this.ccpClient.SendIncomingCall(CallId, To, From);

            bool noMakeCallActionSignal = are.WaitOne(timeout, false);
            if(noMakeCallActionSignal)
            {
                outputLine("Did not receive a signal from the Test Call Control Provider pertainting to a MakeCall action");
                return false;
            }
            
            if(!WaitForSignal( MakeCallActionTest.script1.S_Simple.FullName, rigourousTimeout ) )
            {
                outputLine("Did not receive a signal after the MakeCall attempted");
                return false;
            }

            if(!success)
            {
                outputLine("Did not receive a success signal from the MakeCall action");
            }

            if(!WaitForSignal( MakeCallActionTest.script1.S_Simple.FullName, rigourousTimeout ) )
            {
                outputLine("Did not receive a compare-to-callId result signal from after the MakeCall action");
                return false;
            }

            if(!success)
            {
                outputLine("The callId specified in the MakeCall provider action did not route back to the application");
                return false;
            }

            ccpClient.SendCallEstablished(CallId);

            if(!WaitForSignal( MakeCallActionTest.script1.S_Simple.FullName, rigourousTimeout ) )
            {
                outputLine("The MakeCall_Complete event not signaled");
            }

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

        protected void SimpleMessage(ActionMessage action)
        {
            success = true;
        }
        
        protected void FailedMessage(ActionMessage action)
        {
            success = false;
        }

        protected override bool MakeCallReceived(string to, string from, out string callId)
        {
            if(to != To || from != From)
            {
                signallingFailure = true;
            }

            callId = CallId;

            are.Set();

            return true;
        }

	} 
}
