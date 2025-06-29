using System;
using System.Data;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;
using System.Windows.Forms;

using OutgoingCallTest = Metreos.TestBank.Provider.Provider.MultiOutgoingCallControlPanel;

namespace Metreos.FunctionalTests.Standard.CallControl
{
    /// <summary></summary>
    [FunctionalTestImpl(IsAutomated=false)]
    public class MultiOutgoingCallControlPanel : FunctionalTestBase
    {
        public ListBox callList;
        public const string callControlType = "Call Route Group";
        public const string makeCallDn = "Make Call To";
        public const string blindXferDn = "Blind Tranfer To";
        public const string hangupButton = "Hangup";
        public const string playButton = "Play";
        public const string makeCallButton = "Make Call";
        public const string blindXferButton = "Blind Transfer";
        public const string endButton = "End Test";

		public const string WaitForMediaOption = "Test Wait For Media";
		public const string WaitForMediaState = "Wait For Media State";
        private bool WaitForMedia;
        private string WaitForMediaStr; 

        private string routingGuid;
        private TestTextInputData callRouteGroupField;
        private TestControl control;
        private TestTextInputData makeCallField;
        private TestTextInputData blindXferField;
        private TestUserEvent makeCallEvent;
        private TestUserEvent blindEvent; 
        private TestUserEvent playEvent;
        private TestUserEvent hangupEvent;
        private TestBooleanInputData waitForMediaChoice;
        private TestOptionsInputData mediaChoice;
        private AutoResetEvent endTest;


		// Creates and initializes a new ArrayList.
		ArrayList waitForMediaAL = new ArrayList(new string[] {"TxRx","Tx","Rx","None"});

        public MultiOutgoingCallControlPanel () : base(typeof( MultiOutgoingCallControlPanel ))
        {
            this.endTest = new AutoResetEvent(false);
            this.callList = new ListBox();
            this.callList.Height = 100;

  			this.Instructions = "For the Call Route Group field, enter 'H323', 'SCCP', 'CTI' or 'SIP'.\n\nYou must enter the 'Make Call To', and 'Blind Transfer To' fields *before* you start the test";

        }

        public override bool Execute()
        {

            updateCallRouteGroup(OutgoingCallTest.Name, (Constants.CallRouteGroupTypes) Enum.Parse(typeof(Constants.CallRouteGroupTypes), input[callControlType] as string, true));
            updateMediaRouteGroup(OutgoingCallTest.Name, Constants.DefaultMediaGroup);

            ManagementCommunicator.Instance.RefreshApplicationConfiguration(OutgoingCallTest.Name);
            
            TriggerScript( OutgoingCallTest.script1.FullName );

			WaitForMedia = (bool) input[WaitForMediaOption];
            WaitForMediaStr = input[WaitForMediaState] as string;

            if(!WaitForSignal( OutgoingCallTest.script1.S_Started.FullName , 60) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive a start ack from application.  Giving up.");
                return false;
            }
          
            this.StartListening();

            endTest.WaitOne();
            this.StopListening();

            return true;
        }

        public override ArrayList GetRequiredUserInput()
        {
            control = new TestControl("My Control", "Seth", "list", callList); 
            callRouteGroupField = 
                new TestTextInputData(callControlType, "Enter Call Route Group Name-- H323 | CTI | SCCP | SIP", callControlType, "", 80); 
            makeCallField  = 
                new TestTextInputData(makeCallDn, "Make a call to which number?", makeCallDn, "", 80); 
            blindXferField = 
                new TestTextInputData(blindXferDn, "Blind Xfer to which number?", blindXferDn, "", 80); 
            makeCallEvent = 
                new TestUserEvent(makeCallButton, "Make Call Now!", makeCallButton, makeCallButton, new CommonTypes.AsyncUserInputCallback(OnMakeCallEvent));
            playEvent = 
                new TestUserEvent(playButton, "Play Music Now!", playButton, playButton, new CommonTypes.AsyncUserInputCallback(OnPlayEvent));
            blindEvent = 
                new TestUserEvent(blindXferButton, "Blind Xfer Now!", blindXferButton, blindXferButton, new CommonTypes.AsyncUserInputCallback(OnBlindEvent));
            hangupEvent = 
                new TestUserEvent(hangupButton, "Hangup Now!", hangupButton, hangupButton, new CommonTypes.AsyncUserInputCallback(OnHangupEvent));
            waitForMediaChoice = 
                new TestBooleanInputData(WaitForMediaOption, "Test Wait For Media?", WaitForMediaOption, false);
			mediaChoice = 
                new TestOptionsInputData(WaitForMediaState, "Select a Media State", WaitForMediaState, waitForMediaAL);
            TestUserEvent endEvent = new TestUserEvent(endButton, "End the test", endButton, endButton, new CommonTypes.AsyncUserInputCallback(OnEndEvent));

            ArrayList inputs = new ArrayList();
            inputs.Add(control);
            inputs.Add(callRouteGroupField);
            inputs.Add(makeCallField);
            inputs.Add(blindXferField);
            inputs.Add(makeCallEvent);
            inputs.Add(blindEvent);
            inputs.Add(playEvent);
            inputs.Add(hangupEvent);
            inputs.Add(waitForMediaChoice);
			inputs.Add(mediaChoice);
            inputs.Add(endEvent);

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

            if(success && message.IndexOf("completed") != -1)
            {
                log.Write(TraceLevel.Info, "Made call successfully");
                callList.Items.Add(im["callId"] as string);
            }
            else if(!success)
            {
                log.Write(TraceLevel.Error, "Call not completed");
            }
        }

        public void OnConsultResponse(ActionMessage im)
        {
            bool success = (bool) im["success"];

            if(success)
            {
                callList.Items.Remove(im["callId"] as String);
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
                callList.Items.Remove(im["callId"] as String);
                log.Write(System.Diagnostics.TraceLevel.Info, "Blind Transfer succeeded");
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
            callList.Items.Remove(im["callId"] as string);
        }

        #endregion

        #region User Events

        public bool OnMakeCallEvent(string name, string @value)
        {
            Hashtable fields = new Hashtable();
            fields["to"] = makeCallField.TextBox.Text;
			
            fields["WaitForMediaState"] = WaitForMediaStr;
            fields["testWaitForMedia"] = WaitForMedia.ToString();

            SendEvent( OutgoingCallTest.script1.E_MakeCall.FullName, routingGuid, fields);
            return true;
        }

        public bool OnBlindEvent(string name, string @value)
        {
            string selectedCallId;
            if(ItemSelected(out selectedCallId))
            {
                Hashtable fields = new Hashtable();
                fields["to"] = blindXferField.TextBox.Text;
                fields["callId"] = selectedCallId;
                SendEvent( OutgoingCallTest.script1.E_Blind.FullName, routingGuid, fields);
            }
            else
            {
                log.Write(TraceLevel.Error, "No call selected");
            }

            return true;
        }

        public bool OnPlayEvent(string name, string @value)
        {
            string selectedCallId;
            if(ItemSelected(out selectedCallId))
            {
                Hashtable fields = new Hashtable();
                fields["callId"] = selectedCallId;
                SendEvent( OutgoingCallTest.script1.E_Play.FullName, routingGuid, fields);
            }
            else
            {
                log.Write(TraceLevel.Error, "No call selected");
            }

            return true;
        }

        public bool OnHangupEvent(string name, string @value)
        {
            string selectedCallId;
            if(ItemSelected(out selectedCallId))
            {
                Hashtable fields = new Hashtable();
                fields["callId"] = selectedCallId;
                SendEvent( OutgoingCallTest.script1.E_Hangup.FullName, routingGuid, fields);
                callList.Items.Remove(selectedCallId);
            }
            else
            {
                log.Write(TraceLevel.Error, "No call selected");
            }

            return true;
        }

        public bool OnEndEvent(string name, string @value)
        {
            // Hang up all outstanding callls
            foreach(string call in callList.Items)
            {
                Hashtable fields = new Hashtable();
                fields["callId"] = call;
                if(routingGuid != null)
                {
                    SendEvent(OutgoingCallTest.script1.E_Hangup.FullName, routingGuid, fields);
                }
            }

            if(routingGuid != null)
            {
                SendEvent(OutgoingCallTest.script1.E_End.FullName, routingGuid);
            }

            routingGuid = null;
            callList.Items.Clear();
            endTest.Set();
            return true;
        }

        private bool ItemSelected(out string selectedCallId)
        {
            selectedCallId = null;
            selectedCallId = callList.SelectedItem as string;

            return callList.SelectedItem != null;
        }

        #endregion

        public override void Initialize()
        {
            // Hang up all outstanding callls
            foreach(string call in callList.Items)
            {
                Hashtable fields = new Hashtable();
                fields["callId"] = call;
                if(routingGuid != null)
                {
                    SendEvent(OutgoingCallTest.script1.E_Hangup.FullName, routingGuid, fields);
                }
            }

            if(routingGuid != null)
            {
                SendEvent(OutgoingCallTest.script1.E_End.FullName, routingGuid);
            }

            routingGuid = null;
            callList.Items.Clear();
            this.StopListening();
        }

        public override void Cleanup()
        {
            // Hang up all outstanding callls
            foreach(string call in callList.Items)
            {
                Hashtable fields = new Hashtable();
                fields["callId"] = call;
                if(routingGuid != null)
                {
                    SendEvent(OutgoingCallTest.script1.E_Hangup.FullName, routingGuid, fields);
                }
            }

            if(routingGuid != null)
            {
                SendEvent(OutgoingCallTest.script1.E_End.FullName, routingGuid);
            }

            routingGuid = null;
            callList.Items.Clear();
            this.StopListening();
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
                                          new CallbackLink( OutgoingCallTest.script1.S_Play.FullName,
                                          new FunctionalTestSignalDelegate(OnPlayResponse)), 
                                          new CallbackLink( OutgoingCallTest.script1.S_Hold.FullName,
                                          new FunctionalTestSignalDelegate(OnMediaChangedEvent)), 
                                          new CallbackLink( OutgoingCallTest.script1.S_Hangup.FullName,
                                          new FunctionalTestSignalDelegate(OnHangup)) };
        }
    } 
}
