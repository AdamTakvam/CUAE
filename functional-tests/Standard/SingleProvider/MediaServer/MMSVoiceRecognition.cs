/*
 * Purpose: 
 * This script is used to create a UI to support the MAX application 
 * 
 * Author: Warren Yetman
 * Created: 2/22/06
 * 
 * Dependencies: MAX application MMSVoiceRecognition
 */ 
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

using MyTest = Metreos.TestBank.Provider.Provider.MMSVoiceRecognition;

namespace Metreos.FunctionalTests.SingleProvider.MediaServer
{
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class MMSVoiceRecognition : FunctionalTestBase
    {
        /*
		public const string callControlType = "Call Route Group";
        public const string to = "to";
		public const string modeOptionChoice = "Mode option: Adjust/Constant";
		public const string typeOptionChoice = "Type option: Volume/Speed";
		public const string scaleOptionChoice = "Scale option: Absolute/Relative";
		public const string toggleOptionChoice = "Toggle option: On/Off, default=Off";
		public const string tvalueChoice = "Toggle Value 0-4";

		private bool mode;
		private bool type;
		private bool scale;
		private bool toggle;
		private int tvalue;
		private string testScale="absolute";
		*/
        private bool success;

        public MMSVoiceRecognition () : base(typeof( MMSVoiceRecognition ))
        {
			this.Instructions = "Note: 1. TTS Needs to Be Installed and Running\nYou will need to call the Appliance to trigger the App....";
        }

		/*
        public override ArrayList GetRequiredUserInput()
        {
            
			TestTextInputData callRouteGroupField = new TestTextInputData(callControlType, "Enter Call Route Group Name-- H323 | CTI", callControlType, "H323", 80); 
            TestTextInputData toField = new TestTextInputData(to, "The number to call.", to, 80);

			TestBooleanInputData modeOption = new TestBooleanInputData(modeOptionChoice, "default=Adjust(true)", modeOptionChoice, true);
            TestBooleanInputData typeOption = new TestBooleanInputData(typeOptionChoice, "default=Speed(true)", typeOptionChoice, true);
			TestBooleanInputData scaleOption = new TestBooleanInputData(scaleOptionChoice, "default=Absolute(true)", scaleOptionChoice, true);
			TestBooleanInputData toggleOption = new TestBooleanInputData(toggleOptionChoice, "default=False", toggleOptionChoice, false);
            TestTextInputData tvalueField = new TestTextInputData(tvalueChoice, "", tvalueChoice, 20);

            ArrayList inputs = new ArrayList();
            inputs.Add(callRouteGroupField);
			inputs.Add(toField);
			inputs.Add(modeOption);
            inputs.Add(typeOption);
            inputs.Add(scaleOption);
			inputs.Add(toggleOption);
            inputs.Add(tvalueField);

			return inputs;
		    
        }
        */
		
        public override bool Execute()
        {
            /*
			updateCallRouteGroup(MyTest.Name, (Constants.CallRouteGroupTypes) Enum.Parse(typeof(Constants.CallRouteGroupTypes), input[callControlType] as string, true));
            updateMediaRouteGroup(MyTest.Name, Constants.DefaultMediaGroup);

            ManagementCommunicator.Instance.RefreshApplicationConfiguration(MyTest.Name);

            mode = (bool) input[modeOptionChoice];
            type = (bool) input[typeOptionChoice];
            scale = (bool) input[scaleOptionChoice];
            toggle = (bool) input[toggleOptionChoice];
            tvalue = Convert.ToInt32(input[tvalueChoice]);  //

            Hashtable args = new Hashtable();
            args["to"] = input[to];
			args["mode"] = mode.ToString();
			args["type"] = type.ToString();
			args["scale"] = scale.ToString();
			args["toggle"] = toggle.ToString();
			args["tvalue"] = tvalue.ToString();
            
			if (!scale)
			{
				testScale="relative";
			} 
			args["testScale"] = testScale;

            TriggerScript( MyTest.script1.FullName, args );
            */
			
			/* wait for a recieved call signal */
			if( !WaitForSignal( null, 30) )
			{
				log.Write(System.Diagnostics.TraceLevel.Info, "Failed to receive a signal.");
				return false;
			}

			/* wait for a hangup signal */
			if( !WaitForSignal( null, 1200) )
			{
				log.Write(System.Diagnostics.TraceLevel.Info, "Failed to receive a signal.");
				return false;
			}

            return success;
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

		private void Receive(ActionMessage im)
		{
			log.Write(TraceLevel.Info, "Received a Call.");
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
                new CallbackLink( MyTest.script1.S_HangUp.FullName, new FunctionalTestSignalDelegate( Hangup )),
				new CallbackLink( MyTest.script1.S_ReceiveCall.FullName, new FunctionalTestSignalDelegate( Receive ))
            };
        }
    } 
}
