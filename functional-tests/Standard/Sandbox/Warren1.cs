using System;
using System.IO;
using System.Web;
using System.Net;
using System.Diagnostics;
using System.Collections;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.Samoa.FunctionalTestFramework;

using MyTest = Metreos.TestBank.ARE.ARE.Warren1;

namespace Metreos.FunctionalTests.Sandbox
{
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class Warren1 : FunctionalTestBase
    {
        public const string callControlType = "Call Route Group";
        public const string to = "to";
		//public const string mode = "mode";
		//public const string volume = "volume";
        private bool success;

        public Warren1 () : base(typeof( Warren1 ))
        {
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData callRouteGroupField = new TestTextInputData(callControlType, "Enter Call Route Group Name-- H323 | CTI", callControlType, "H323", 80); 
            TestTextInputData toField = new TestTextInputData("To Number", "The number to call.", to, 80);
			//TestTextInputData modeField = new TestTextInputData("Mode", "Mode of the test", mode, 80);
			//TestTextInputData volumeField = new TestTextInputData("Volume", "The number to call.", volume, 80);
            ArrayList inputs = new ArrayList();
            inputs.Add(callRouteGroupField);
			inputs.Add(toField);
			//inputs.Add(modeField);
			//inputs.Add(volumeField);
			return inputs;
        }

        public override bool Execute()
        {
            updateCallRouteGroup(MyTest.Name, (Constants.CallRouteGroupTypes) Enum.Parse(typeof(Constants.CallRouteGroupTypes), input[callControlType] as string, true));
            updateMediaRouteGroup(MyTest.Name, Constants.DefaultMediaGroup);

            ManagementCommunicator.Instance.RefreshApplicationConfiguration(MyTest.Name);

            
			//args["mode"] = input[mode];
			//args["volume"] = input[volume];

			// Note: This next line would work if the application was not waiting on an Incoming Call
            //TriggerScript( MyTest.script1.FullName, args );
            log.Write(System.Diagnostics.TraceLevel.Info, "*** Please Call The App Now ***");

			// - This test will wait for a signal. We are waiting for the S_ReceiveCall signal at the start
			// -- and the value of the phone number to call for a conference call will
			// -- be passed in as well.
			// --- Then that signal will be processed by the ReceiveCall method.
			// ---- The ReceiveCall method will send the AnswerCall event back to the script
			// ----- At the point when the application gets an OnRemoteHangUp event 
			// ----- the HangUp signal is sent to the test framework and the second WaitForSignal 
			// ----- will complete the test framework script.		
			 
            if( !WaitForSignal( null, 30) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Failed to receive a signal.");
                return false;
            }

			if( !WaitForSignal( null, 60) )
			{
				log.Write(System.Diagnostics.TraceLevel.Info, "Failed to receive a signal.");
				return false;
			}

            return success;
        }

		private void Receive(ActionMessage im)
		{
			log.Write(TraceLevel.Info, "Recieved Incoming Call.");
			success = false;

			Hashtable args = new Hashtable();
			args["to"] = this.input[to];

			SendEvent(MyTest.script1.E_AnswerCall.FullName, im.RoutingGuid, args);
		}

        private void Failure(ActionMessage im)
        {
			log.Write(TraceLevel.Error, "Failed.");
			success = false;
		}

		

        private void Hangup(ActionMessage im)
        {
			log.Write(TraceLevel.Info, "Received Hangup.");
			success = true;
        }


        public override string[] GetRequiredTests()
        {
            return new string[] { ( MyTest.FullName ) };
        }

        public override void Initialize()
        {
			success = false;
		}

        public override void Cleanup()
        {
			success = false;
        }


        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] 
            { 
                new CallbackLink( MyTest.script1.S_Failure.FullName , new FunctionalTestSignalDelegate( Failure )),
				new CallbackLink( MyTest.script1.S_ReceiveCall.FullName , new FunctionalTestSignalDelegate( Receive )),
                new CallbackLink( MyTest.script1.S_HangUp.FullName, new FunctionalTestSignalDelegate( Hangup ))
            };
        }
    } 
}
