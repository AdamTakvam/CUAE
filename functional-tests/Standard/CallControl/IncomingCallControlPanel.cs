using System;
using System.Data;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using IncomingCallTest = Metreos.TestBank.Provider.Provider.IncomingCallControlPanel;

namespace Metreos.FunctionalTests.Standard.CallControl
{
    /// <summary>The objective of this test is to verify that the DeviceListX data can be queried by
    /// a supplied Device Name as the search key</summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class IncomingCallControlPanel : FunctionalTestBase
    {
        //Consult Transfer is ONLY supported by H323
		public const string consultXferDn = "Consult Transfer To";
        public const string blindXferDn = "Blind Tranfer To";
        public const string hangupButton = "Hangup";
        public const string rejectCallOption = "Reject";
        public const string playButton = "Play";
        public const string consultXferButton = "Consult Transfer";
        public const string blindXferButton = "Blind Transfer";
        public const string hairpinOption = "Hairpin";

		//5/23/06 WKY: SMA-1128
		public const string WaitForMediaOption = "Test Wait For Media";
		public const string WaitForMediaState = "Wait For Media State";
		private bool WaitForMedia;
		private string WaitForMediaStr;  //todo: find a better name
		private string WaitForMediaField = "Wait For Media State";

        private string routingGuid;
        private bool reject;
        //private bool hairpin;
        private string consultXferTo;
        private string blindXferTo;
        private volatile bool done;

		// Creates and initializes a new ArrayList.
		ArrayList waitForMediaAL = new ArrayList(new string[] {"TxRx","Tx","Rx","None"});

        public IncomingCallControlPanel () : base(typeof( IncomingCallControlPanel ))
        {
            this.Instructions = "You must enter the 'Consult Transfer To (ONLY for H323 calls)' and 'Blind Transfer To' fields *before* you start the test";
		}

        public override bool Execute()
        {
            updateMediaRouteGroup(IncomingCallTest.Name, Constants.DefaultMediaGroup);

            ManagementCommunicator.Instance.RefreshApplicationConfiguration(IncomingCallTest.Name);
            reject = (bool) input[rejectCallOption];
            //hairpin = (bool) input[hairpinOption];
            consultXferTo = input[consultXferDn] as String;
            blindXferTo = input[blindXferDn] as String;

			WaitForMedia = (bool) input[WaitForMediaOption];
			WaitForMediaStr = input[WaitForMediaField] as string;

            if(!WaitForSignal( IncomingCallTest.script1.S_IncomingCall.FullName , 60) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive an incoming call from user after a minute.  Giving up.");
                return false;
            }
          
            Hashtable fields = new Hashtable();
			/*
			 * 2/20/06: WKY| SFT-25 "FTF does not support Hairpin Connections"
			 * This feature is not available and will be initialized as false until
			 * FTF can support it.
			 */ 
			//fields["hairpin"] = hairpin.ToString();
			fields["hairpin"] = "false";
            fields["reject"] = reject.ToString();

			fields["WaitForMediaState"] = WaitForMediaStr;
			fields["testWaitForMedia"] = WaitForMedia.ToString();

            SendEvent( IncomingCallTest.script1.E_IncomingCall.FullName, routingGuid, fields);

            if(reject)
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Call should have been rejected.  Exiting test.");
                return true;
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
            //TestBooleanInputData hairpinOptionChoice = new TestBooleanInputData(hairpinOption, "Hear yourself", hairpinOption, true);
            TestBooleanInputData rejectChoice = new TestBooleanInputData(rejectCallOption, "Reject the call?", rejectCallOption, false);
            TestTextInputData consultXferField = new TestTextInputData(consultXferDn, "Consult Xfer to which number?", consultXferDn, "", 80); 
            TestTextInputData blindXferField = new TestTextInputData(blindXferDn, "Blind Xfer to which number?", blindXferDn, "", 80); 
            TestUserEvent consultEvent = new TestUserEvent(consultXferButton, "Consult Xfer Now!", consultXferButton, consultXferButton, new CommonTypes.AsyncUserInputCallback(OnConsultEvent));
            TestUserEvent blindEvent = new TestUserEvent(blindXferButton, "Blind Xfer Now!", blindXferButton, blindXferButton, new CommonTypes.AsyncUserInputCallback(OnBlindEvent));
            TestUserEvent playEvent = new TestUserEvent(playButton, "Play Music Now!", playButton, playButton, new CommonTypes.AsyncUserInputCallback(OnPlayEvent));
            TestUserEvent hangupEvent = new TestUserEvent(hangupButton, "Hangup Now!", hangupButton, hangupButton, new CommonTypes.AsyncUserInputCallback(OnHangupEvent));

			TestBooleanInputData WaitForMediaChoice = new TestBooleanInputData(WaitForMediaOption, "Test Wait For Media?", WaitForMediaOption, false);			
			TestTextInputData WaitForMediaField  = new TestTextInputData(WaitForMediaState, "Enter A Wait For Media State-- TxRx | Rx | Tx | None", WaitForMediaState, "", 80); 
			//TestOptionsInputData MediaChoice = new TestOptionsInputData(WaitForMediaState, "Select a Media State", WaitForMediaState, waitForMediaAL);

            ArrayList inputs = new ArrayList();
            //inputs.Add(hairpinOptionChoice);
            inputs.Add(rejectChoice);
            inputs.Add(consultXferField);
            inputs.Add(blindXferField);
            inputs.Add(consultEvent);
            inputs.Add(blindEvent);
            inputs.Add(playEvent);
            inputs.Add(hangupEvent);

			inputs.Add(WaitForMediaChoice);
			inputs.Add(WaitForMediaField);
			//inputs.Add(MediaChoice);

            return inputs;
        }

        #region Test Events

        public void OnIncomingCall(ActionMessage im)
        {
           routingGuid = im.RoutingGuid;
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
				SendEvent( IncomingCallTest.script1.E_Hangup.FullName, routingGuid);
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


        public bool OnConsultEvent(string name, string @value)
        {
            Hashtable fields = new Hashtable();
            fields["to"] = consultXferTo;
            SendEvent( IncomingCallTest.script1.E_Consult.FullName, routingGuid, fields);
            return true;
        }
 
        public bool OnBlindEvent(string name, string @value)
        {
            Hashtable fields = new Hashtable();
            fields["to"] = blindXferTo;
            SendEvent( IncomingCallTest.script1.E_Blind.FullName, routingGuid, fields);
            return true;
        }

        public bool OnPlayEvent(string name, string @value)
        {
            SendEvent( IncomingCallTest.script1.E_Play.FullName, routingGuid);
            return true;
        }

        public bool OnHangupEvent(string name, string @value)
        {
            SendEvent( IncomingCallTest.script1.E_Hangup.FullName, routingGuid);
            Thread.Sleep(500);
            done = true;
            return true;
        }

        #endregion

        public override void Initialize()
        {
            routingGuid = null;
            reject = false;
            done = false;
            consultXferTo = null;
            blindXferTo = null;
        }

        public override void Cleanup()
        {
            routingGuid = null;
            reject = false;
            done = false;
            consultXferTo = null;
            blindXferTo = null;
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( IncomingCallTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
              new CallbackLink( IncomingCallTest.script1.S_IncomingCall.FullName, 
              new FunctionalTestSignalDelegate(OnIncomingCall)),
              new CallbackLink( IncomingCallTest.script1.S_Blind.FullName,
              new FunctionalTestSignalDelegate(OnBlindResponse)), 
              new CallbackLink( IncomingCallTest.script1.S_Consult.FullName,
              new FunctionalTestSignalDelegate(OnConsultResponse)), 
              new CallbackLink( IncomingCallTest.script1.S_Play.FullName,
              new FunctionalTestSignalDelegate(OnPlayResponse)), 
              new CallbackLink( IncomingCallTest.script1.S_Hold.FullName,
              new FunctionalTestSignalDelegate(OnMediaChangedEvent)), 
              new CallbackLink( IncomingCallTest.script1.S_Hangup.FullName,
              new FunctionalTestSignalDelegate(OnHangup)) };
        }
    } 
}
