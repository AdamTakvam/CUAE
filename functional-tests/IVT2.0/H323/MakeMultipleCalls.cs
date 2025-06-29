using System;
using System.Data;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using MakeCall = Metreos.TestBank.IVT.IVT.MakeCall;

namespace Metreos.FunctionalTests.IVT2._0.CallControl
{
    /// <summary>Make an inernal phone call</summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class MakeMultipleCalls : FunctionalTestBase
    {
        public const string callControlType = "Call Route Group";
        public const string callNum = "callNum";
        public const string hangup = "Hang up";
        private bool makeCallSuccess;
        private bool makeCallComplete;
        private ArrayList routingGuids;
        private AutoResetEvent are;

        public MakeMultipleCalls() : base(typeof( MakeMultipleCalls ))
        {
            this.Instructions = "For the Call Route Group field, enter 'H323' or 'CTI'.\n\nYou must enter the 'Make Call To', 'Consult Transfer To' and 'Blind Transfer To' fields *before* you start the test";
            routingGuids = new ArrayList();
            are = new AutoResetEvent(false);
        }

        public override void Initialize()
        {
            makeCallSuccess = true;
            makeCallComplete = true;
            routingGuids.Clear();
        }

        public override void Cleanup()
        {
            makeCallSuccess = true;
            makeCallComplete = true;
            routingGuids.Clear();
        }

        public override bool Execute()
        {
            updateCallRouteGroup(MakeCall.Name, (Constants.CallRouteGroupTypes) Enum.Parse(typeof(Constants.CallRouteGroupTypes), input[callControlType] as string, true));
            updateMediaRouteGroup(MakeCall.Name, Constants.DefaultMediaGroup);

            ManagementCommunicator.Instance.RefreshApplicationConfiguration(MakeCall.Name);

            Hashtable args = new Hashtable();
            int numberOfCalls = Convert.ToInt32(input[callNum]);

            for(int i = 0; i < numberOfCalls; i++)
            {
                args.Clear();
                args["to"] = input[MakeInputName(i)];

                TriggerScript( MakeCall.script1.FullName, args );
            }

            for(int i = 0; i < numberOfCalls * 2; i++)
            {
                if(!WaitForSignal( MakeCall.script1.S_Trigger.FullName))
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a response from the application.");
                    return false;
                }
            }

            if(!are.WaitOne(20000, false))
            {
                foreach(string routingGuid in routingGuids)
                {
                    SendEvent( MakeCall.script1.E_Hangup.FullName, routingGuid);
                }
            }

            return makeCallComplete && makeCallSuccess;
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData callRouteGroupField = new TestTextInputData(callControlType, "Enter Call Route Group Name-- H323 | CTI", callControlType, "", 80); 
            TestTextInputData numCallInput = new TestTextInputData("Number of outbound calls", 
                "The number of calls to make.", callNum, 80);
          
            ArrayList inputs = new ArrayList();
            inputs.Add(callRouteGroupField);
            inputs.Add(numCallInput);
       
            for(int i = 0; i < 5; i++)
            {
                string inputName = MakeInputName(i);
                
                TestTextInputData toNumberInput = new TestTextInputData(inputName, inputName, inputName, String.Empty, 80);
                inputs.Add(toNumberInput);
            }
            TestUserEvent hangupPush = new TestUserEvent(hangup, "Press to hang up all phones.", hangup, hangup, new CommonTypes.AsyncUserInputCallback(HangupEvent));

            inputs.Add(hangupPush);
            return inputs;
        }

        public void MakeCallMade(ActionMessage im)
        {
            makeCallSuccess &= (bool) im["success"];

            if(makeCallSuccess)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The call is now outbound");
            }
            else
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The call could not be made");
            }
        }

        public void MakeCallComplete(ActionMessage im)
        {
            routingGuids.Add(im.RoutingGuid);
            makeCallComplete &= (bool) im["success"];
            string failReason = im["reason"] as string;

            if(makeCallComplete)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The call was successfully completed");
            }
            else
            {
                log.Write(System.Diagnostics.TraceLevel.Info, String.Format("The call did not successfully complete.  EndReason was {0}", failReason));
            }
        }

        public bool HangupEvent(string name, string @value)
        {
            foreach(string routingGuid in routingGuids)
            {
                SendEvent( MakeCall.script1.E_Hangup.FullName, routingGuid);
            }
            
            are.Set();

            return true;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( MakeCall.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( MakeCall.script1.S_Trigger.FullName , 
                                          new FunctionalTestSignalDelegate(MakeCallMade)),
                                          
                                          new CallbackLink( MakeCall.script1.S_MakeCallComplete.FullName,
                                          new FunctionalTestSignalDelegate(MakeCallComplete)),

                                          new CallbackLink( MakeCall.script1.S_Hangup.FullName , 
                                          null) };
        }

        public string MakeInputName(int i)
        {
            return "Number " + i;
        }
    } 
}
