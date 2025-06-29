using System;
using System.Data;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using OutgoingCallTest = Metreos.TestBank.Provider.Provider.OutgoingCallControlPanel;

namespace Metreos.FunctionalTests.Standard.CallControl
{
    /// <summary></summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class OutgoingCallControlPanel : FunctionalTestBase
    {
        public const string callControlType = "Call Route Group";
        public const string makeCallDn = "Make Call To";
		//4/19/06 WKY: This test is not supported at this time
        //public const string consultXferDn = "Consult Transfer To";
        public const string blindXferDn = "Blind Tranfer To";
        public const string hangupButton = "Hangup";
        public const string playButton = "Play";
        //public const string consultXferButton = "Consult Transfer";
        public const string blindXferButton = "Blind Transfer";

		//5/23/06 WKY: SMA-1128
		public const string WaitForMediaOption = "Test Wait For Media";
		public const string WaitForMediaState = "Wait For Media State";
        private bool WaitForMedia=true;
        private string WaitForMediaStr;  //todo: find a better name
		private string WaitForMediaField = "Wait For Media State";
		//private string MediaChoice = "Wait For Media State";

        private string routingGuid;
        private string makeCallTo;
        //private string consultXferTo;
        private string blindXferTo;
        private volatile bool done;
        private bool makeCallSuccess;

		// Creates and initializes a new ArrayList.
		ArrayList waitForMediaAL = new ArrayList(new string[] {"TxRx","Tx","Rx","None"});

        public OutgoingCallControlPanel () : base(typeof( OutgoingCallControlPanel ))
        {
			this.Instructions = "For the Call Route Group field, enter 'H323', 'SCCP', 'CTI' or 'SIP'.\n\nYou must enter the 'Make Call To', and 'Blind Transfer To' fields *before* you start the test";
        }

        public override bool Execute()
        {
            updateCallRouteGroup(OutgoingCallTest.Name, (Constants.CallRouteGroupTypes) Enum.Parse(typeof(Constants.CallRouteGroupTypes), input[callControlType] as string, true));
            updateMediaRouteGroup(OutgoingCallTest.Name, Constants.DefaultMediaGroup);

            ManagementCommunicator.Instance.RefreshApplicationConfiguration(OutgoingCallTest.Name);
            
            TriggerScript( OutgoingCallTest.script1.FullName );

            makeCallTo = input[makeCallDn] as string;
            //consultXferTo = input[consultXferDn] as String;
            blindXferTo = input[blindXferDn] as String;

			WaitForMedia = (bool) input[WaitForMediaOption];
			WaitForMediaStr = input[WaitForMediaField] as String;
            //WaitForMediaStr = input[MediaChoice] as string;

            if(!WaitForSignal( OutgoingCallTest.script1.S_Started.FullName , 60) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a start ack from application.  Giving up.");
                return false;
            }
          
            Hashtable fields = new Hashtable();
            fields["to"] = makeCallTo;
			
            fields["WaitForMediaState"] = WaitForMediaStr;
			fields["testWaitForMedia"] = WaitForMedia.ToString();

            SendEvent( OutgoingCallTest.script1.E_MakeCall.FullName, routingGuid, fields);

            if(!WaitForSignal( OutgoingCallTest.script1.S_MakeCall.FullName , 60) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a makecall ack from application.  Giving up.");
                return false;
            }
          

            if(!makeCallSuccess)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Call could not be made.  Assuming failure.");
                return false;
            }

            // Now we wait until hangup from our end or remote end

            while(!done)
            {
                WaitForSignal( String.Empty, 1);
            }

            return true;
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData callRouteGroupField = new TestTextInputData(callControlType, "Enter Call Route Group Name-- H323 | CTI | SCCP | SIP", callControlType, "", 80); 
            TestTextInputData makeCallField  = new TestTextInputData(makeCallDn, "Make a call to which number?", makeCallDn, "", 80); 
            //TestTextInputData consultXferField = new TestTextInputData(consultXferDn, "Consult Xfer to which number?", consultXferDn, "", 80); 
            TestTextInputData blindXferField = new TestTextInputData(blindXferDn, "Blind Xfer to which number?", blindXferDn, "", 80); 
            //TestUserEvent consultEvent = new TestUserEvent(consultXferButton, "Consult Xfer Now!", consultXferButton, consultXferButton, new CommonTypes.AsyncUserInputCallback(OnConsultEvent));
            TestUserEvent blindEvent = new TestUserEvent(blindXferButton, "Blind Xfer Now!", blindXferButton, blindXferButton, new CommonTypes.AsyncUserInputCallback(OnBlindEvent));
            TestUserEvent playEvent = new TestUserEvent(playButton, "Play Music Now!", playButton, playButton, new CommonTypes.AsyncUserInputCallback(OnPlayEvent));
            TestUserEvent hangupEvent = new TestUserEvent(hangupButton, "Hangup Now!", hangupButton, hangupButton, new CommonTypes.AsyncUserInputCallback(OnHangupEvent));

			TestBooleanInputData WaitForMediaChoice = new TestBooleanInputData(WaitForMediaOption, "Test Wait For Media?", WaitForMediaOption, false);			
			TestTextInputData WaitForMediaField  = new TestTextInputData(WaitForMediaState, "Enter A Wait For Media State-- TxRx | Rx | Tx | None", WaitForMediaState, "", 80); 
			//TestOptionsInputData MediaChoice = new TestOptionsInputData(WaitForMediaState, "Select a Media State", WaitForMediaState, waitForMediaAL);

            ArrayList inputs = new ArrayList();
            inputs.Add(callRouteGroupField);
            inputs.Add(makeCallField);
            //inputs.Add(consultXferField);
            inputs.Add(blindXferField);
            //inputs.Add(consultEvent);
            inputs.Add(blindEvent);
            inputs.Add(playEvent);
            inputs.Add(hangupEvent);

            inputs.Add(WaitForMediaChoice);
			inputs.Add(WaitForMediaField);

			//inputs.Add(MediaChoice);

            return inputs;
        }

        #region Test Events

        public void OnStarted(ActionMessage im)
        {
           routingGuid = im.RoutingGuid;
        }

        public void OnMakeCall(ActionMessage im)
        {
            bool success = (bool) im["success"];
            string message = im["message"] as string;

            log.Write(System.Diagnostics.TraceLevel.Info, message);
            makeCallSuccess = success;
            done = !makeCallSuccess;
        }

        public void OnConsultResponse(ActionMessage im)
        {
            bool success = (bool) im["success"];

            if(success)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Consult Transfer succeeded");
            }
            else
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Consult Transfer failed");
            }
        }

        public void OnBlindResponse(ActionMessage im)
        {
            bool success = (bool) im["success"];

            if(success)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Blind Transfer succeeded");
				SendEvent( OutgoingCallTest.script1.E_Hangup.FullName, routingGuid);
				Thread.Sleep(500);
				done = true;
            }
            else
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Blind Transfer failed");
            }
        }

        public void OnPlayResponse(ActionMessage im)
        {
            string message = im["message"] as string;
            bool success = (bool) im["success"];

            log.Write(System.Diagnostics.TraceLevel.Info, message);
        }

        public void OnMediaChangedEvent(ActionMessage im)
        {
            string message = im["message"] as string;

            log.Write(System.Diagnostics.TraceLevel.Info, message);
        }

        public void OnHangup(ActionMessage im)
        {
            log.Write(System.Diagnostics.TraceLevel.Info, "Far end hungup.  Ending test.");
            done = true;
        }

        #endregion

        #region User Events

      /*
		public bool OnConsultEvent(string name, string @value)
        {
            Hashtable fields = new Hashtable();
            fields["to"] = consultXferTo;
            SendEvent( OutgoingCallTest.script1.E_Consult.FullName, routingGuid, fields);
            return true;
        }
      */
        public bool OnBlindEvent(string name, string @value)
        {
            Hashtable fields = new Hashtable();
            fields["to"] = blindXferTo;
            SendEvent( OutgoingCallTest.script1.E_Blind.FullName, routingGuid, fields);
            return true;
        }

        public bool OnPlayEvent(string name, string @value)
        {
            SendEvent( OutgoingCallTest.script1.E_Play.FullName, routingGuid);
            return true;
        }

        public bool OnHangupEvent(string name, string @value)
        {
            SendEvent( OutgoingCallTest.script1.E_Hangup.FullName, routingGuid);
            Thread.Sleep(500);
            done = true;
            return true;
        }

        #endregion

        public override void Initialize()
        {
            routingGuid = null;
            makeCallSuccess = false;
            done = false;
            makeCallTo = null;
            //consultXferTo = null;
            blindXferTo = null;
        }

        public override void Cleanup()
        {
            routingGuid = null;
            makeCallSuccess = false;
            done = false;
            makeCallTo = null;
            //consultXferTo = null;
            blindXferTo = null;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( OutgoingCallTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
              new CallbackLink( OutgoingCallTest.script1.S_Started.FullName, 
              new FunctionalTestSignalDelegate(OnStarted)),
              new CallbackLink( OutgoingCallTest.script1.S_MakeCall.FullName, 
              new FunctionalTestSignalDelegate(OnMakeCall)),
              new CallbackLink( OutgoingCallTest.script1.S_Blind.FullName,
              new FunctionalTestSignalDelegate(OnBlindResponse)), 
              //new CallbackLink( OutgoingCallTest.script1.S_Consult.FullName,
              //new FunctionalTestSignalDelegate(OnConsultResponse)), 
              new CallbackLink( OutgoingCallTest.script1.S_Play.FullName,
              new FunctionalTestSignalDelegate(OnPlayResponse)), 
              new CallbackLink( OutgoingCallTest.script1.S_Hold.FullName,
              new FunctionalTestSignalDelegate(OnMediaChangedEvent)), 
              new CallbackLink( OutgoingCallTest.script1.S_Hangup.FullName,
              new FunctionalTestSignalDelegate(OnHangup)) };
        }
    } 
}
