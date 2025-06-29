using System;
using System.Threading;
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
    public class SimultaneousHangup : FunctionalTestBase
    {
        private const string callNum = "callNum";
        private const string hangup = "Hang up";
        private bool answerCallSuccess;
        private ArrayList routingGuids;
        private AutoResetEvent are;

        public SimultaneousHangup() : base(typeof( SimultaneousHangup ))
        {
            routingGuids = new ArrayList();
            are = new AutoResetEvent(false);
        }

        public override void Initialize()
        {
            answerCallSuccess = true;
            routingGuids.Clear();
        }

        public override void Cleanup()
        {
            answerCallSuccess = true;
            routingGuids.Clear();
        }

        public override bool Execute()
        {  
            instructionLine("Run SimClient");

            int numCalls = Convert.ToInt32(input[callNum]);

            for(int i = 0; i < numCalls; i++)
            {
                if(!WaitForSignal( AnswerCallTest.script1.S_AnsweredCall.FullName, 30) )
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a call.");
                    return false;
                }
            }
            
            if(!answerCallSuccess)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Not all calls were answered successfully");
                return false;
            }

            if(!are.WaitOne(10000, false))
            {
                foreach(string routingGuid in routingGuids)
                {
                    SendEvent( AnswerCallTest.script1.E_Hangup.FullName, routingGuid );
                }
            }

            return true;
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData numCallInput = new TestTextInputData("Number of inbound calls", 
                "The number of calls to answer.", callNum, 80);
            TestUserEvent hangupPush = new TestUserEvent(hangup, "Press to hang up all phones.", hangup, hangup, new CommonTypes.AsyncUserInputCallback(HangupEvent));

            ArrayList inputs = new ArrayList();
            inputs.Add(numCallInput);
            inputs.Add(hangupPush); 
            return inputs;
        }

        public void AnswerCallComplete(ActionMessage im)
        {
            routingGuids.Add(im.RoutingGuid);
            answerCallSuccess &= (bool) im["success"];

            if(answerCallSuccess)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Answered call");
            }
            else
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The call could not be answered");
            }
        }

        public bool HangupEvent(string name, string @value)
        {
            foreach(string routingGuid in routingGuids)
            {
                SendEvent( AnswerCallTest.script1.E_Hangup.FullName, routingGuid );
            }
            
            are.Set();

            return true;
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
