using System;
using System.IO;
using System.Web;
using System.Net;
using System.Collections;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.Samoa.FunctionalTestFramework;

using BasicTTSTest = Metreos.TestBank.Provider.Provider.BasicTTS;

namespace Metreos.FunctionalTests.SingleProvider.MediaServer.TTS
{
    /// <summary></summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    [QaTest(Id="TESTCASE-MMS-1014")]
    public class TTSTTSTTS : FunctionalTestBase
    {
        public const string callControlType = "Call Route Group";
        public const string to = "to";
        private string routingGuid;
        private bool success;
        private bool hungup;
        public TTSTTSTTS () : base(typeof( TTSTTSTTS ))
        {
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData callRouteGroupField = new TestTextInputData(callControlType, "Enter Call Route Group Name-- H323 | CTI", callControlType, "H323", 80); 
            TestTextInputData toField = new TestTextInputData("To Number", 
                "The number to call.", to, 80);
            ArrayList inputs = new ArrayList();
            inputs.Add(callRouteGroupField);
            inputs.Add(toField);
            return inputs;
        }

        public override bool Execute()
        {
            updateCallRouteGroup(BasicTTSTest.Name, (Constants.CallRouteGroupTypes) Enum.Parse(typeof(Constants.CallRouteGroupTypes), input[callControlType] as string, true));
            updateMediaRouteGroup(BasicTTSTest.Name, Constants.DefaultMediaGroup);

            ManagementCommunicator.Instance.RefreshApplicationConfiguration(BasicTTSTest.Name);

            Hashtable args = new Hashtable();
            args["to"] = input[to];

            TriggerScript( BasicTTSTest.script1.FullName, args );

            if( !WaitForSignal( BasicTTSTest.script1.S_MadeCall.FullName ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Failed to receive a make call complete message.  Cannot continue.");
                return true;
            }

            if(hungup)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Remote party hungup.  Cannot continue.");
                return true;
            }

            if(!success)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The call did not complete successfully.  Cannot continue.");
                return true;
            }       

            log.Write(System.Diagnostics.TraceLevel.Info, "Executing play");
            Hashtable parameters = new Hashtable();
            parameters["play1"] = "if this is first.";
            parameters["play2"] = "this is second.";
            parameters["play3"] = "and this is third, this test succeeds.";

            SendEvent(BasicTTSTest.script1.E_Play.FullName, routingGuid, parameters);
            
            if( !WaitForSignal( BasicTTSTest.script1.S_PlayComplete.FullName, 120 ) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Failed to receive a play complete message.  Cannot continue.");
                return true;
            }

            if(hungup)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Remote party hungup.  Cannot continue.");
                return true;
            }

            if(!success)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The play did not complete successfully.");
                return false;
            }

            return true;
        }

        private void PlayComplete(ActionMessage im)
        {
            success &= (bool) im["success"];

            if(success)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The play was successful.");
            }
            else
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The play was not successful.");
            }
        }

        private void MadeCall(ActionMessage im)
        {
            routingGuid = im.RoutingGuid;
            success &= (bool) im["success"];

            if(success)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The call was successful.");
            }
            else
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "The call was not successful.");
            }
        }

        private void Hangup(ActionMessage im)
        {
            hungup = true;
            log.Write(System.Diagnostics.TraceLevel.Info, "The remote party hungup");
        }


        public override string[] GetRequiredTests()
        {
            return new string[] { ( BasicTTSTest.FullName ) };
        }

        public override void Initialize()
        {
            success = true;
            hungup = false;
            routingGuid = null;
        }

        public override void Cleanup()
        {
            success = true;
            hungup = false;
            routingGuid = null;
        }


        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] 
            { 
                new CallbackLink( BasicTTSTest.script1.S_MadeCall.FullName , new FunctionalTestSignalDelegate( MadeCall )),
                new CallbackLink( BasicTTSTest.script1.S_PlayComplete.FullName, new FunctionalTestSignalDelegate( PlayComplete )),
                new CallbackLink( BasicTTSTest.script1.S_Hangup.FullName, new FunctionalTestSignalDelegate( Hangup ))
            };
        }
    } 
}
