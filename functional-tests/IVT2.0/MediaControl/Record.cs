using System;
using System.Data;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using MakeCall = Metreos.TestBank.IVT.IVT.MakeCall;

namespace Metreos.FunctionalTests.IVT2._0.MediaControl
{
    /// <summary>Make an inernal phone call</summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class RecordCallee : FunctionalTestBase
    {
        public const string to = "to";

        private bool makeCallSuccess;
        private bool makeCallComplete;
        private bool recordSuccess;
        private bool playSuccess;
        private string routingGuid;

        public RecordCallee() : base(typeof( RecordCallee ))
        {
            
        }

        public override void Initialize()
        {
            makeCallSuccess = false;
            makeCallComplete = false;
            recordSuccess = false;
            playSuccess = false;
            routingGuid = null;
        }

        public override void Cleanup()
        {
            makeCallSuccess = false;
            makeCallComplete = false;
            recordSuccess = false;
            playSuccess = false;
            routingGuid = null;
        }

        public override bool Execute()
        {
            updateCallRouteGroup(MakeCall.Name, Constants.CallRouteGroupTypes.H323);
            updateMediaRouteGroup(MakeCall.Name, Constants.DefaultMediaGroup);

            ManagementCommunicator.Instance.RefreshApplicationConfiguration(MakeCall.Name);

            Hashtable args = new Hashtable();
            args["to"] = input[to];

            TriggerScript( MakeCall.script1.FullName, args );

            if(!WaitForSignal( MakeCall.script1.S_Trigger.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive response from the application.");
                return false;
            }
            
            if(!makeCallSuccess)
            {
                return false;
            }

            if(!WaitForSignal( MakeCall.script1.S_MakeCallComplete.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive response from the application after making the call");
                return false;
            }

            if(!makeCallComplete)
            {
                return false;
            }

            SendEvent(MakeCall.script1.E_Record.FullName, routingGuid);

            if(!WaitForSignal( MakeCall.script1.S_Record.FullName , 15 ))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Record complete not received");
                return false;
            }

            if(!recordSuccess)
            {
                return false;
            }

            if(!WaitForSignal( MakeCall.script1.S_Play.FullName, 15 ))
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Play complete not received");
            }

            return true;
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData toField = new TestTextInputData("To Number", 
                "The number to call.", to, 80);
            ArrayList inputs = new ArrayList();
            inputs.Add(toField);
            return inputs;
        }

        public void MakeCallMade(ActionMessage im)
        {
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

        public void PlayComplete(ActionMessage im)
        {
            playSuccess = (bool) im["success"];

            if(!playSuccess)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The play failed to complete");
            }
            else
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The play of the recording completed");
            }
        }

        public void RecordComplete(ActionMessage im)
        {
            recordSuccess = (bool) im["success"];

            if(!recordSuccess)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The record failed to complete");
            }
            else
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The record completed");
            }
        }

        public void MakeCallComplete(ActionMessage im)
        {
            routingGuid = im.RoutingGuid;
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
            return new string[] { ( MakeCall.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( MakeCall.script1.S_Trigger.FullName , 
                                          new FunctionalTestSignalDelegate(MakeCallMade)),
                                          
                                          new CallbackLink( MakeCall.script1.S_MakeCallComplete.FullName,
                                          new FunctionalTestSignalDelegate(MakeCallComplete)),

                                          new CallbackLink( MakeCall.script1.S_Play.FullName,
                                          new FunctionalTestSignalDelegate(PlayComplete)),

                                          new CallbackLink( MakeCall.script1.S_Record.FullName,
                                          new FunctionalTestSignalDelegate(RecordComplete)),

                                          new CallbackLink( MakeCall.script1.S_Hangup.FullName , 
                                          null) };
        }
    } 
}
