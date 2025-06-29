using System;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using ContinuousCallsTest = Metreos.TestBank.Provider.Provider.ContinuousCalls;

namespace Metreos.FunctionalTests.SingleProvider.H323.Stress
{
    /// <summary></summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class ContinuousCalls : FunctionalTestBase
    {
        private const string number = "number";
        private const string callManagerIp = "callManager";
        private const string callPeriodName = "callPeriod";
        private const string callDurationName = "callDuration";
        private const string receive = "receive";
        private const string stop = "stop";
        private string routingGuid;
        private volatile bool testOver;
        private volatile int receivedCallCount;
        private volatile int madeCallCount;

        public ContinuousCalls() : base(typeof( ContinuousCalls ))
        {

        }

        public override bool Execute()
        {
            bool receiverMode = (bool) input[receive];
            string numberToDial = input[number] as string;
            string callManagerIpAddress = input[callManagerIp] as string;
            int callDuration = GetNumber(input[callDurationName] as string, 3000);
            int callPeriod = GetNumber(input[callPeriodName] as string, 1000);

            if(receiverMode)
            {
                while(!testOver)
                {
                    if( !WaitForSignal( ContinuousCallsTest.script2.S_ReceivedCall.FullName, 60 ) )
                    {
                        log.Write(System.Diagnostics.TraceLevel.Info, "Timeout in waiting on call received signal.");
                    }
                }
            }
            else
            {
                Hashtable fields = new Hashtable();
                fields["to"] = numberToDial + '@' + callManagerIpAddress;
                fields["callPeriod"] = callPeriod.ToString();
                fields["callDuration"] = callDuration.ToString();

                TriggerScript(ContinuousCallsTest.script1.FullName, fields);

                if( !WaitForSignal( ContinuousCallsTest.script1.S_Trigger.FullName ) )
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive onload signal");
                    return false;
                }

                while(!testOver)
                {
                    if( !WaitForSignal( ContinuousCallsTest.script1.S_MadeCall.FullName, 60 ) )
                    {
                        log.Write(System.Diagnostics.TraceLevel.Info, "Timeout in waiting on call made signal.");
                    }
                }
            }

            if(!receiverMode)
            {
                SendEvent( ContinuousCallsTest.script1.E_StopTimer.FullName, routingGuid );
            }

            return true;
        }


        private int GetNumber(string callPeriodString, int defaultAmount)
        {      
            try
            {
                return Int32.Parse(callPeriodString);
            }
            catch
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Unable to parse value. Defaulting to " + defaultAmount + ".");
                return defaultAmount;
            }
        }

        private void CallMade(ActionMessage im)
        {
            madeCallCount++;

            if(madeCallCount % 10000 == 0)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Made " + madeCallCount + " calls.");
            }
        }

        private void CallReceived(ActionMessage im)
        {
            receivedCallCount++;

            if(receivedCallCount % 10000 == 0)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Received " + madeCallCount + " calls.");
            }
        }

        private void GetRoutingGuid(ActionMessage im)
        {
            routingGuid = ActionGuid.GetRoutingGuid(im.ActionGuid);  
        }

        public override ArrayList GetRequiredUserInput()
        {
            ArrayList inputs = new ArrayList();
            
            TestTextInputData numberToCall = new TestTextInputData("Enter the number to call", "This number will be dialed by the test", number, "1001", 40);
            TestTextInputData callManagerIpAddress = new TestTextInputData("Enter CallManager IP", "This Ip Address will be used to create the 'to' field.", callManagerIp, "192.168.1.251", 120);
            TestTextInputData callPeriod = new TestTextInputData("Period between calls (ms)", "Amount of milliseconds between calls.", callPeriodName, "1000", 120);
            TestTextInputData callDuration = new TestTextInputData("Duration of call (ms)", "Amount of milliseconds for call talk time.", callDurationName, "3000", 120);
            TestBooleanInputData receiverMode = new TestBooleanInputData("Receiver mode?", "True is receiver mode, false is caller mode.", receive, true);
            TestUserEvent stopEvent = new TestUserEvent("Stop receiving/calling", "Stops the test from calling or receiving", stop, "Stop", new CommonTypes.AsyncUserInputCallback(EndTest));
            
            inputs.Add(numberToCall);
            inputs.Add(callManagerIpAddress);
            inputs.Add(callPeriod);
            inputs.Add(callDuration);
            inputs.Add(receiverMode);
            inputs.Add(stopEvent);

            return inputs;
            
        }

        private bool EndTest(string varName, string Value)
        {
            testOver = true;
            return true;
        }

        public override void Initialize()
        {
            routingGuid = null;
            testOver = false;
            receivedCallCount = 0;
        }

        public override void Cleanup()
        {
            routingGuid = null;
            testOver = false;
            receivedCallCount = 0;
        }


        public override string[] GetRequiredTests()
        {
            return new string[] { ( ContinuousCallsTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] 
            { 
                new CallbackLink( ContinuousCallsTest.script1.S_Trigger.FullName ,
                new FunctionalTestSignalDelegate(GetRoutingGuid)),
                
                new CallbackLink( ContinuousCallsTest.script1.S_MadeCall.FullName ,
                new FunctionalTestSignalDelegate(CallMade)),

                new CallbackLink( ContinuousCallsTest.script2.S_ReceivedCall.FullName ,
                new FunctionalTestSignalDelegate(CallReceived))
            };
        }
    } 
}
