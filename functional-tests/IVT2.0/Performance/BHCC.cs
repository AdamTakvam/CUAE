using System;
using System.Data;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using AnswerCallTest = Metreos.TestBank.IVT.IVT.AnswerCall;

namespace Metreos.FunctionalTests.IVT2._0.Performance
{
    /// <summary>Make an inernal phone call</summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class BHCC : FunctionalTestBase
    {
        private const string maxCallsField = "Max Calls";

        private int callCountSuccess;
        private int callCountFailure;
        private int simultCount;
        private bool answerCallSuccess;
        private int maxSimult;

        public BHCC() : base(typeof( BHCC ))
        {
            
        }

        public override void Initialize()
        {
            maxSimult = 0;
            callCountSuccess = 0;
            callCountFailure = 0;
            simultCount = 0;
            answerCallSuccess = true;
        }

        public override void Cleanup()
        {
            maxSimult = 0;
            callCountSuccess = 0;
            callCountFailure = 0;
            simultCount = 0;
            answerCallSuccess = true;
        }

        public override bool Execute()
        {  
            instructionLine("Start SimClient");
            while(true)
            {
                if(!WaitForSignal( AnswerCallTest.script1.S_AnsweredCall.FullName, 60) )
                {
                    int totalCalls = callCountSuccess + callCountFailure;
                    log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a answer call response");
                    log.Write(System.Diagnostics.TraceLevel.Info, totalCalls + " calls received");
                    log.Write(System.Diagnostics.TraceLevel.Info, maxSimult + " simultaneous calls");
                    break;
                }
            }

            int totalCalls2 = callCountSuccess + callCountFailure;
            log.Write(System.Diagnostics.TraceLevel.Info, totalCalls2 + " calls received");
            log.Write(System.Diagnostics.TraceLevel.Info, maxSimult + " simultaneous calls");

            return true;
        }

        public void AnswerCallComplete(ActionMessage im)
        {
            bool success = (bool) im["success"];
            answerCallSuccess &= success;
            simultCount++;
            if(simultCount > maxSimult)
            {
                maxSimult = simultCount;
            }

            if(success)
            {
                callCountSuccess++;
                if(callCountSuccess % 25 == 0)
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, callCountSuccess + " calls answered successfully");
                }
            }
            else
            {
                callCountFailure++;
                log.Write(System.Diagnostics.TraceLevel.Info, callCountFailure + " calls not answered successfully");
            }
        }

        public void Hangup(ActionMessage im)
        {
            simultCount--;
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
                                          new FunctionalTestSignalDelegate(Hangup)) };
        }
    } 
}
