using System;
using System.Data;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using JTapiAnswerCallTest = Metreos.TestBank.IVT.IVT.JTapiAnswerCall;

namespace Metreos.FunctionalTests.IVT2._0.JtapiCallControl
{
    /// <summary>Make an inernal phone call</summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class JTapiTerminateCall : FunctionalTestBase
    {
        private const string hangup = "Hang Up";

        private bool answerCallSuccess;
        private string routingGuid;
        private AutoResetEvent are;

        public JTapiTerminateCall() : base(typeof( JTapiTerminateCall ))
        {
            are = new AutoResetEvent(false);
        }

        public override void Initialize()
        {
            answerCallSuccess = false;
        }

        public override void Cleanup()
        {
            answerCallSuccess = false;
        }

        public override bool Execute()
        {  

            if(!WaitForSignal( JTapiAnswerCallTest.script1.S_AnswerCallComplete.FullName, 60 ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Never received response from the application for JTapiAnswerCall completion. Was the test call made?");
                return false;
            }

            if(!answerCallSuccess)
            {
                return false;
            }

            are.WaitOne();

            SendEvent( JTapiAnswerCallTest.script1.E_Hangup.FullName, routingGuid);


            return true;
        }

        public override ArrayList GetRequiredUserInput()
        {      
            ArrayList inputs = new ArrayList();
            TestUserEvent hangupPush = new TestUserEvent(hangup, "Press to hangup", hangup, hangup, new CommonTypes.AsyncUserInputCallback(HangupEvent));
            inputs.Add(hangupPush);
            return inputs;
        }

        public bool HangupEvent(string name, string @value)
        {
            are.Set();
            return true;
        }

        public void AnswerCallComplete(ActionMessage im)
        {
            routingGuid = im.RoutingGuid;

            answerCallSuccess = (bool) im["success"];
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { JTapiAnswerCallTest.FullName };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( JTapiAnswerCallTest.script1.S_AnswerCallComplete.FullName,
                                          new FunctionalTestSignalDelegate(AnswerCallComplete)) }; 
                                    
        }
    } 
}
