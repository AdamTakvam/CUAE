using System;
using System.Data;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using MakeCall = Metreos.TestBank.IVT.IVT.MakeCall;

namespace Metreos.FunctionalTests.IVT2._0
{
    /// <summary>Make an inernal phone call</summary>
    [FunctionalTestImpl(IsAutomated=true)]
    public class MakeCallExternal : FunctionalTestBase
    {

        public const string to = "to";

        private bool makeCallSuccess;
        private bool makeCallComplete;

        public MakeCallExternal() : base(typeof( MakeCallExternal ))
        {
            
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
            Hashtable args = new Hashtable();
            args["to"] = input[to];

            TriggerScript( MakeCall.script1.FullName, args );

            if(!WaitForSignal( MakeCall.script1.S_Trigger.FullName ) )
            {
                outputLine("Did not receive response from the application.");
                return false;
            }
            
            if(!makeCallSuccess)
            {
                return false;
            }

            if(!WaitForSignal( MakeCall.script1.S_MakeCallComplete.FullName ) )
            {
                outputLine("Did not receive response from the application after making the call");
                return false;
            }

            if(!makeCallComplete)
            {
                return false;
            }

            if(!WaitForSignal( MakeCall.script1.S_Hangup.FullName ) )
            {
                outputLine("Hangup received");
                return false;
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
                outputLine("The call is now outbound");
            }
            else
            {
                outputLine("The call could not be made");
            }
        }

        public void MakeCallComplete(ActionMessage im)
        {
            makeCallComplete = (bool) im["success"];
            string failReason = im["reason"] as string;

            if(makeCallComplete)
            {
                outputLine("The call was successfully completed");
            }
            else
            {
                outputLine(String.Format("The call did not successfully complete.  EndReason was {0}", failReason));
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

                                          new CallbackLink( MakeCall.script1.S_Hangup.FullName , 
                                          null) };
        }
    } 
}
