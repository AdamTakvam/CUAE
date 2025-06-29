using System;
using System.Data;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using AnswerCallTest = Metreos.TestBank.IVT.IVT.AnswerCall;

namespace Metreos.FunctionalTests.IVT2._0.CallControl
{
    /// <summary>Make an inernal phone call</summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class AnswerCall : FunctionalTestBase
    {
        private bool answerCallSuccess;

        public AnswerCall() : base(typeof( AnswerCall ))
        {
            
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
            instructionLine("Call the Application Server");

            if(!WaitForSignal( AnswerCallTest.script1.S_AnsweredCall.FullName, 60) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a call.");
                return false;
            }
            
            if(!answerCallSuccess)
            {
                return false;
            }

            if(!WaitForSignal( AnswerCallTest.script1.S_Hangup.FullName, 20 ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive user hangup");
                return false;
            }
            else
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The call was hung up");
            }

            return true;
        }

        public void AnswerCallComplete(ActionMessage im)
        {
            answerCallSuccess = (bool) im["success"];

            if(answerCallSuccess)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The connection was established");
            }
            else
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The call was unsuccessfully answered");
            }
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( AnswerCallTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( AnswerCallTest.script1.S_AnsweredCall.FullName,
                                          new FunctionalTestSignalDelegate(AnswerCallComplete)),  
                                          new CallbackLink( AnswerCallTest.script1.S_Hangup.FullName , 
                                          null) };
        }
    } 
}
