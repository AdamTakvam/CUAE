using System;
using System.Data;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using JTapiIncomingCallControlPanelCallTest = Metreos.TestBank.IVT.IVT.JTapiIncomingCallControlPanel;

namespace Metreos.FunctionalTests.IVT2._0.JtapiCallControl
{
	/// <summary>JTapi Incoming Call Control Panel</summary>
	[FunctionalTestImpl(IsAutomated=false)]
	public class JTapiIncomingCallControlPanel : FunctionalTestBase
	{
		public const string blindXferDn = "Blind Tranfer To";
		public const string blindXferButton = "Blind Transfer";
		public const string conferenceButton = "Conference";
		public const string hangupButton = "Hangup";
        public const string rejectCallOption = "Reject";

		private bool reject;
		private string blindXferTo;
		private bool answerCallSuccess;
        private int totalCalls;
        private string routingGuid1;
		private string routingGuid;
        private string CallID;

		private Hashtable g_fields = new Hashtable();

		//private AutoResetEvent are;
		private volatile bool done;
		private volatile bool incomingCallReceived;

		private string version = "V 16";

		public JTapiIncomingCallControlPanel() : base(typeof( JTapiIncomingCallControlPanel ))
		{
			//are = new AutoResetEvent(false);			
			this.Instructions = version + " You must enter the 'Blind Transfer To field *before* you start the test";
		}

		public override bool Execute()
		{  
			log.Write(System.Diagnostics.TraceLevel.Info, "Call test phone " + DateTime.Now);
            reject = (bool) input[rejectCallOption];
			blindXferTo = input[blindXferDn] as String;

            log.Write(System.Diagnostics.TraceLevel.Info, version + " Waiting for incoming call " + DateTime.Now);

			// The WaitForSignal function traps all incoming signals and they are 
			// then categorized in the CallBack function. The fact that the actual signal is 
			// listed below is really for readability sake. If any signal comes down within 60 seconds the
			// code will not exit.
			if(!WaitForSignal( JTapiIncomingCallControlPanelCallTest.script1.S_IncomingCall.FullName, 60)) 
			{
				log.Write(System.Diagnostics.TraceLevel.Info, "Did not receive an incoming call from the user after a minute.  Giving up.");
				return false;
			}

			// Verify the expected signal type.
			if (incomingCallReceived)
			{
				log.Write(System.Diagnostics.TraceLevel.Verbose, "DEBUG: Received MAX Signal sending E_IncomingCall" + " - " + DateTime.Now);
            
				Hashtable fields = new Hashtable();
				fields["reject"] = reject.ToString();
				fields["totalCalls"] = totalCalls;

				SendEvent( JTapiIncomingCallControlPanelCallTest.script1.E_IncomingCall.FullName, routingGuid, fields);

				if(reject)
				{
					log.Write(System.Diagnostics.TraceLevel.Info, "Call should have been rejected.  Exiting test." + " - " + DateTime.Now);
					return true;
				}

				log.Write(System.Diagnostics.TraceLevel.Verbose, "DEBUG: waiting for s_hangup" + " - " + DateTime.Now);

				//
				// Now we are waiting for the next signal that we expect to be a hangup. The signal can not
				// be quantified until it enters the CallBack function and if a Hangup signal comes down
				// the boolean "done" will be set to true.
				//

				while(!done)
				{
					WaitForSignal( String.Empty, 1);
				}

				return true;
			} 
			else 
			{
              return false;
			}
		}

		public override ArrayList GetRequiredUserInput()
		{                  
			TestTextInputData blindXferField = new TestTextInputData(blindXferDn, "Blind Xfer to which number?", blindXferDn, "", 80); 
			TestUserEvent blindEvent = new TestUserEvent(blindXferButton, "Blind Xfer Now!", blindXferButton, blindXferButton, new CommonTypes.AsyncUserInputCallback(OnBlindEvent));
			TestUserEvent conferenceEvent = new TestUserEvent(conferenceButton, "Conference Now!", conferenceButton, conferenceButton, new CommonTypes.AsyncUserInputCallback(OnConferenceEvent));						
			TestUserEvent hangupEvent = new TestUserEvent(hangupButton, "Hangup Now!", hangupButton, hangupButton, new CommonTypes.AsyncUserInputCallback(OnHangupEvent));
			TestBooleanInputData rejectChoice = new TestBooleanInputData(rejectCallOption, "Reject the call?", rejectCallOption, false);

			ArrayList inputs = new ArrayList();

            inputs.Add(rejectChoice);
			inputs.Add(blindXferField);
			inputs.Add(blindEvent);
			inputs.Add(conferenceEvent);
			inputs.Add(hangupEvent);

			return inputs;
		}

		#region Test Events

		public void AnswerCallComplete(ActionMessage im)
		{
			routingGuid = im.RoutingGuid;

			answerCallSuccess = (bool) im["success"];
		}

		public void OnHangup(ActionMessage im)
		{
			log.Write(System.Diagnostics.TraceLevel.Info, "Received Signal Far end hungup.  Ending test." + " - " + DateTime.Now);
			done = true;
		}

		public void OnCallActive(ActionMessage im)
		{
			log.Write(System.Diagnostics.TraceLevel.Info, "DEBUG: MAX sent CallActive signal" + " " + im["CallId"].ToString());
			if (CallID=="") 
			{ 
				CallID=im["CallId"].ToString();
				g_fields["CallId1"] = CallID;
				log.Write(System.Diagnostics.TraceLevel.Info, "DEBUG: 1st Active call - " + CallID);
				g_fields["CallId2"]=string.Empty;
			} 
			else
				if (!CallID.Equals(im["CallId"])) 
			{
				g_fields["CallId2"] = im["CallId"]; 
				log.Write(System.Diagnostics.TraceLevel.Info, "DEBUG: 2nd Active call - " + im["CallId"].ToString());
				
			}         
		}

		public void OnIncomingCall(ActionMessage im)
		{   
			if (!incomingCallReceived) 
			{
              incomingCallReceived=true;
			}
			//For this test we are only expecting to ever get 2 incoming calls.
			if (routingGuid1=="") 
			{ 
              totalCalls=totalCalls+1;
              //log.Write(System.Diagnostics.TraceLevel.Info, "DEBUG: empty string");
			  //retain the first script pointer
			  routingGuid1 = im.RoutingGuid;
			  routingGuid=im.RoutingGuid;
              log.Write(System.Diagnostics.TraceLevel.Info, "DEBUG: OnIncomingCall routingGuid: " + routingGuid + " - " + DateTime.Now); 
			} else
				if (!routingGuid1.Equals(im.RoutingGuid)) {
				  totalCalls=totalCalls+1;

				  Hashtable fields = new Hashtable();
				  fields["totalCalls"] = totalCalls;

				  SendEvent( JTapiIncomingCallControlPanelCallTest.script1.E_UpdateGlobals.FullName, routingGuid1, fields);
				  SendEvent( JTapiIncomingCallControlPanelCallTest.script1.E_UpdateGlobals.FullName, im.RoutingGuid, fields);

				  fields["reject"] = false;
				  log.Write(System.Diagnostics.TraceLevel.Info, "DEBUG: 2nd Incoming call, totalCalls: " + totalCalls + " - " + DateTime.Now);
				  SendEvent( JTapiIncomingCallControlPanelCallTest.script1.E_IncomingCall.FullName, im.RoutingGuid, fields);
			}         
		}


		// What is interesting about this call back is that the app is sending a signal and
		// along with the signal it is sending a custom action parameter called "success" and that
		// variable represents a boolean true|false which is tested and processed accordingly.
		//
		public void OnBlindResponse(ActionMessage im)
		{
			bool success = (bool) im["success"];

			if(success)
			{
				log.Write(System.Diagnostics.TraceLevel.Info, "Received Signal Blind Transfer succeeded" + " - " + DateTime.Now);
			}
			else
			{
				log.Write(System.Diagnostics.TraceLevel.Info, "Received Signal Blind Transfer failed" + " - " + DateTime.Now);
			}
		}

		public void OnConferenceResponse(ActionMessage im)
		{
			bool success = (bool) im["success"];

			if(success)
			{
				log.Write(System.Diagnostics.TraceLevel.Info, "Received Signal Conference succeeded" + " - " + DateTime.Now);
			}
			else
			{
				log.Write(System.Diagnostics.TraceLevel.Info, "Received Signal Conference failed" + " - " + DateTime.Now);
			}
		}

		#endregion

		#region User Events

		public bool HangupEvent(string name, string @value)
		{
			log.Write(System.Diagnostics.TraceLevel.Verbose, "HangupEvent");
			return true;
		}

		public bool OnHangupEvent(string name, string @value)
		{
			SendEvent( JTapiIncomingCallControlPanelCallTest.script1.E_Hangup.FullName, routingGuid1);
			Thread.Sleep(500);
			done = true;
			return true;
		}

		public bool OnBlindEvent(string name, string @value)
		{
			Hashtable fields = new Hashtable();
			fields["to"] = blindXferTo;
            log.Write(System.Diagnostics.TraceLevel.Verbose, "DEBUG: FTF sending E_Blind" + " - " + DateTime.Now);
			SendEvent( JTapiIncomingCallControlPanelCallTest.script1.E_Blind.FullName, routingGuid1, fields);
			return true;
		}

		public bool OnConferenceEvent(string name, string @value)
		{
			if (g_fields["CallId2"].ToString()!="") 
			{ 
				//since we have 2 instances of the JTapiIncomingCall script running because there are 2 calls
				//on the same line, at this point, we will send back to the script the 2nd CallId for the
				//conference call.
				log.Write(System.Diagnostics.TraceLevel.Verbose, "DEBUG: FTF sending E_Conference" + " - " + DateTime.Now);
				SendEvent( JTapiIncomingCallControlPanelCallTest.script1.E_Conference.FullName, routingGuid1, g_fields);
				return true;
			} 
			else 
			{
               log.Write(System.Diagnostics.TraceLevel.Info, "ERROR: You do not have 2 active calls on the same JTapi monitored line." + " - " + DateTime.Now);		
               return false;
			}
		}

		#endregion

		//"[GO]" runs Initialize()
		public override void Initialize()
		{
			answerCallSuccess = false;
			blindXferTo = null;
			reject = false;
			incomingCallReceived=false;
			done = false;
			routingGuid1=string.Empty;
			routingGuid=string.Empty;
			CallID=string.Empty;
			totalCalls=0;
		}

		//"[Abort]" or at program end runs Cleanup()
		public override void Cleanup()
		{
			answerCallSuccess = false;
			blindXferTo = null;
			reject = false;
			done = false;
			incomingCallReceived=false;
			routingGuid1=string.Empty;
			routingGuid=string.Empty;
			CallID=string.Empty;
			totalCalls=0;
		}

		public override string[] GetRequiredTests()
		{
			return new string[] { JTapiIncomingCallControlPanelCallTest.FullName };
		}

		public override CallbackLink[] GetCallbacks()
		{
			return new CallbackLink[] { 
				new CallbackLink( JTapiIncomingCallControlPanelCallTest.script1.S_IncomingCall.FullName, 
                new FunctionalTestSignalDelegate(OnIncomingCall)),
				new CallbackLink( JTapiIncomingCallControlPanelCallTest.script1.S_AnswerCallComplete.FullName,
				new FunctionalTestSignalDelegate(AnswerCallComplete)), 
				new CallbackLink( JTapiIncomingCallControlPanelCallTest.script1.S_CallActive.FullName,
				new FunctionalTestSignalDelegate(OnCallActive)),
				new CallbackLink( JTapiIncomingCallControlPanelCallTest.script1.S_BlindSuccess.FullName,
				new FunctionalTestSignalDelegate(OnBlindResponse)),
				new CallbackLink( JTapiIncomingCallControlPanelCallTest.script1.S_BlindFail.FullName,
				new FunctionalTestSignalDelegate(OnBlindResponse)),
				new CallbackLink( JTapiIncomingCallControlPanelCallTest.script1.S_ConferenceSuccess.FullName,
				new FunctionalTestSignalDelegate(OnConferenceResponse)),
				new CallbackLink( JTapiIncomingCallControlPanelCallTest.script1.S_ConferenceFail.FullName,
				new FunctionalTestSignalDelegate(OnConferenceResponse)),
				new CallbackLink( JTapiIncomingCallControlPanelCallTest.script1.S_Hangup.FullName,
				new FunctionalTestSignalDelegate(OnHangup)) 
			};                                   
		}
	} 
}
