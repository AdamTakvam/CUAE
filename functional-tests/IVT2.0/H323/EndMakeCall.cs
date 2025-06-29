using System;
using System.Data;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using EndMakeCallTest = Metreos.TestBank.IVT.IVT.MakeCall;

namespace Metreos.FunctionalTests.IVT2._0.CallControl
{
    /// <summary>Make an inernal phone call</summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class EndMakeCall : FunctionalTestBase
    {
        public const string callControlType = "Call Route Group";
        public const string to = "to";
        public const string from = "from";

        private bool makeCallSuccess;
        private bool makeCallComplete;
        private string routingGuid;

        public EndMakeCall() : base(typeof( EndMakeCall ))
        {
            this.Instructions = "For the Call Route Group field, enter 'H323' or 'CTI'.\n\nYou must enter the 'Make Call To', 'Consult Transfer To' and 'Blind Transfer To' fields *before* you start the test";
        }

        public override void Initialize()
        {
            makeCallSuccess = false;
            makeCallComplete = false;
        }

        public override void Cleanup()
        {
            makeCallSuccess = false;
            makeCallComplete = false;
        }

        public override bool Execute()
        {
            updateCallRouteGroup(EndMakeCallTest.Name, (Constants.CallRouteGroupTypes) Enum.Parse(typeof(Constants.CallRouteGroupTypes), input[callControlType] as string, true));
            updateMediaRouteGroup(EndMakeCallTest.Name, Constants.DefaultMediaGroup);

            ManagementCommunicator.Instance.RefreshApplicationConfiguration(EndMakeCallTest.Name);

            Hashtable args = new Hashtable();
            args["to"] = input[to];
            args["from"] = input[from];

            TriggerScript( EndMakeCallTest.script1.FullName, args );

            if(!WaitForSignal( EndMakeCallTest.script1.S_Trigger.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive response from the application.");
                return false;
            }
            
            if(!makeCallSuccess)
            {
                return false;
            }

            if(!WaitForSignal( EndMakeCallTest.script1.S_MakeCallComplete.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive response from the application after making the call");
                return false;
            }

            if(!makeCallComplete)
            {
                return false;
            }

            if(!WaitForSignal( EndMakeCallTest.script1.S_Hangup.FullName, 20 ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Hangup not received by user");
                SendEvent( EndMakeCallTest.script1.E_Hangup.FullName, routingGuid );
            }

            // Recalling

            TriggerScript( EndMakeCallTest.script1.FullName, args );

            if(!WaitForSignal( EndMakeCallTest.script1.S_Trigger.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive response from the application.");
                return false;
            }
            
            if(!makeCallSuccess)
            {
                return false;
            }

            if(!WaitForSignal( EndMakeCallTest.script1.S_MakeCallComplete.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive response from the application after making the call");
                return false;
            }

            if(!makeCallComplete)
            {
                return false;
            }

            if(!WaitForSignal( EndMakeCallTest.script1.S_Hangup.FullName, 20 ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Hangup not received by user");
                SendEvent( EndMakeCallTest.script1.E_Hangup.FullName, routingGuid );
            }

            return true;
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData callRouteGroupField = new TestTextInputData(callControlType, "Enter Call Route Group Name-- H323 | CTI", callControlType, "", 80); 
            TestTextInputData toField = new TestTextInputData("To Number", 
                "The number to call.", to, 80);
            TestTextInputData fromField = new TestTextInputData("From Number", 
                "The number that this call is coming from.", from, 80);
            ArrayList inputs = new ArrayList();
            inputs.Add(callRouteGroupField);
            inputs.Add(toField);
            inputs.Add(fromField);
            return inputs;
        }

        public void MakeCallMade(ActionMessage im)
        {
            routingGuid = im.RoutingGuid;
            makeCallSuccess = (bool) im["success"];

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
            makeCallComplete = (bool) im["success"];
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

        public override string[] GetRequiredTests()
        {
            return new string[] { EndMakeCallTest.FullName };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( EndMakeCallTest.script1.S_Trigger.FullName , 
                                          new FunctionalTestSignalDelegate(MakeCallMade)),
                                          
                                          new CallbackLink( EndMakeCallTest.script1.S_MakeCallComplete.FullName,
                                          new FunctionalTestSignalDelegate(MakeCallComplete)),

                                          new CallbackLink( EndMakeCallTest.script1.S_Hangup.FullName , 
                                          null) };
        }
    } 
}
