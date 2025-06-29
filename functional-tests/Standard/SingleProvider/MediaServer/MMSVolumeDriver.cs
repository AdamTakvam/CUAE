/*
 * Purpose: 
 * This script is used to create a UI to support the MAX application MMSVolumeDriver.
 * The application is trigered on an IncommingCall event. The script has a life span of
 * 6 minutes max and during this time the script is listening for one of two events:
 * S_Failure, S_HangUp. Nothing out of the ordinary, this is just a driver for an
 * application to which the user must listen to the WAV file that is played and
 * verify the test condition.
 * 
 * Author: Warren Yetman
 * Created: 2/13/06
 * 
 * Dependencies: MAX application MMSVolumeDriver
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

using MyTest = Metreos.TestBank.Provider.Provider.MMSVolumeDriver;

namespace Metreos.FunctionalTests.SingleProvider.MediaServer
{
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class MMSVolumeDriver : FunctionalTestBase
    {
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
        private bool success;

        public MMSVolumeDriver () : base(typeof( MMSVolumeDriver ))
        {
			this.Instructions = "Note: Setting the toggle value to true will override the mode setting.";
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData callRouteGroupField = new TestTextInputData(callControlType, "Enter Call Route Group Name-- H323 | CTI", callControlType, "H323", 80); 
            TestTextInputData toField = new TestTextInputData(to, "The number to call.", to, 80);

			TestBooleanInputData modeOption = new TestBooleanInputData(modeOptionChoice, "default=Adjust(true)", modeOptionChoice, true);
            TestBooleanInputData typeOption = new TestBooleanInputData(typeOptionChoice, "default=Speed(true)", typeOptionChoice, true);
			TestBooleanInputData scaleOption = new TestBooleanInputData(scaleOptionChoice, "default=Absolute(true)", scaleOptionChoice, true);
			TestBooleanInputData toggleOption = new TestBooleanInputData(toggleOptionChoice, "default=False", toggleOptionChoice, false);
            TestTextInputData tvalueField = new TestTextInputData(tvalueChoice, "default=0", tvalueChoice, 20);

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

        public override bool Execute()
        {
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
			else 
			{
				testScale="absolute";
			}
			args["testScale"] = testScale;

            TriggerScript( MyTest.script1.FullName, args );
			
			try
			{
				if( !WaitForSignal( null, 1200) )
				{
					log.Write(System.Diagnostics.TraceLevel.Info, "Failed to receive a signal.");
					return false;
				}
			}
			catch (System.Threading.ThreadAbortException) 
			{
				log.Write(TraceLevel.Info, "The thread was aborted.");
				return false;
			}
			catch(Exception e)
			{
				log.Write(TraceLevel.Info, "Exception was thrown but handled: " + e.Message);
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
/*
 * TODO: 
 * Need to incorporate additional code to handle abort requests
 * 
 * 
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
*/
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
                new CallbackLink( MyTest.script1.S_HangUp.FullName, new FunctionalTestSignalDelegate( Hangup ))
            };
        }
    } 
}
