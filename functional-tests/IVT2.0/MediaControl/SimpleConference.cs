using System;
using System.Data;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using SimpleConferenceTest = Metreos.TestBank.IVT.IVT.SimpleConference;

namespace Metreos.FunctionalTests.IVT2._0.MediaControl
{
    /// <summary>Make an inernal phone call</summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class SimpleConference : FunctionalTestBase
    {
        public const string to1 = "to1";
        public const string to2 = "to2";
        public const string to3 = "to3";
        public const string to4 = "to4";

        private bool makeCallComplete;

        public SimpleConference () : base(typeof( SimpleConference ))
        {
            
        }

        public override void Initialize()
        {
            makeCallComplete = true;
        }

        public override void Cleanup()
        {
            makeCallComplete = true;
        }

        public override bool Execute()
        {
            updateCallRouteGroup(SimpleConferenceTest.Name, Constants.CallRouteGroupTypes.H323);
            updateMediaRouteGroup(SimpleConferenceTest.Name, Constants.DefaultMediaGroup);

            ManagementCommunicator.Instance.RefreshApplicationConfiguration(SimpleConferenceTest.Name);

            Hashtable args = new Hashtable();
            args[to1] = input[to1];
            args[to2] = input[to2];
            args[to3] = input[to3];
            args[to4] = input[to4];

            TriggerScript( SimpleConferenceTest.script1.FullName, args );

            for(int i = 0; i < 4; i++)
            {
                if(!WaitForSignal( SimpleConferenceTest.script1.S_MakeCallComplete.FullName ) )
                {
                    log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a make call complete message.");
                    return false;
                }
            }

            if(!makeCallComplete)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "All calls did not complete");
                return false;
            }

            if(!WaitForSignal ( SimpleConferenceTest.script1.S_AllHangup.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "All callers did not hangup.  Ending test.");
            }

            return true;
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData to1Field = new TestTextInputData("To Number 1", 
                "The 1st number to call.", to1, 80);
            TestTextInputData to2Field = new TestTextInputData("To Number 2", 
                "The 2nd number to call.", to2, 80);
            TestTextInputData to3Field = new TestTextInputData("To Number 3", 
                "The 3rd number to call.", to3, 80);
            TestTextInputData to4Field = new TestTextInputData("To Number 4", 
                "The 4th number to call.", to4, 80);
            ArrayList inputs = new ArrayList();
            inputs.Add(to1Field);
            inputs.Add(to2Field);
            inputs.Add(to3Field);
            inputs.Add(to4Field);
            return inputs;
        }

        public void MakeCallComplete(ActionMessage im)
        {
            makeCallComplete &= (bool) im["success"];
            string failReason = im["reason"] as string;

            if(makeCallComplete)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "A call was successfully completed");
            }
            else
            {
                log.Write(System.Diagnostics.TraceLevel.Info, String.Format("A call did not successfully complete.  EndReason was {0}", failReason));
            }
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( SimpleConferenceTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( SimpleConferenceTest.script1.S_MakeCallComplete.FullName, 
                                          new FunctionalTestSignalDelegate(MakeCallComplete)),
                                          
                                          new CallbackLink( SimpleConferenceTest.script1.S_AllHangup.FullName,
                                          null) };
        }
    } 
}
