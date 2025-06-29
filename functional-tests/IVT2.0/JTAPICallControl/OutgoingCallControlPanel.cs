using System;
using System.Data;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;

using JTapiOutgoingCallControlPanelCallTest = Metreos.TestBank.IVT.IVT.JTapiOutgoingCallControlPanel;

namespace Metreos.FunctionalTests.IVT2._0.JtapiCallControl
{
	/// <summary>Make an inernal phone call</summary>
	[FunctionalTestImpl(IsAutomated=false)]
	public class JTapiOutgoingCallControlPanel : FunctionalTestBase
	{
		private const string to = "to";
		private const string from = "from";
		private const string deviceName = "devicename";
		private const string hangup = "Hang Up";

		private bool makeCallSuccess;
		private string routingGuid;
		//private AutoResetEvent are;


		public const string blindXferDn = "Blind Tranfer To";
		public const string blindXferButton = "Blind Transfer";
		public const string conferenceButton = "Conference";
		public const string conferXferDn = "Conference With";

		private string blindXferTo;
		private string conferXferWith;
		private int totalCalls;
		private string routingGuid1;
		private string CallID;

		private Hashtable g_fields = new Hashtable();

		//private AutoResetEvent are;
		private volatile bool done;
		private volatile bool OutgoingCallReceived;

		private string version = "V 5";
        

		public JTapiOutgoingCallControlPanel() : base(typeof( JTapiOutgoingCallControlPanel ))
		{
			//are = new AutoResetEvent(false);
			this.Instructions = version + " You must enter the 'Blind Transfer To field and Conference With numbers *before* you start the test";
		}

		//"[GO]" runs Initialize()
		public override void Initialize()
		{
			makeCallSuccess = false;
			blindXferTo = null;
			conferXferWith = null;
			OutgoingCallReceived=false;
			done = false;
			routingGuid1=string.Empty;
			routingGuid=string.Empty;
			CallID=string.Empty;
			totalCalls=0;
		}

		//"[Abort]" or at program end runs Cleanup()
		public override void Cleanup()
		{
			makeCallSuccess = false;
			blindXferTo = null;
			conferXferWith = null;
			done = false;
			OutgoingCallReceived=false;
			routingGuid1=string.Empty;
			routingGuid=string.Empty;
			CallID=string.Empty;
			totalCalls=0;
		}

		public override bool Execute()
		{  

            blindXferTo = input[blindXferDn] as String;
            conferXferWith = input[conferXferDn] as String;

			Hashtable args = new Hashtable();
			args[to] = input[to];
			args[from] = input[from];
			args[deviceName] = input[deviceName];

			log.Write(System.Diagnostics.TraceLevel.Info, "Calling " + input[to] + " " + DateTime.Now);

			TriggerScript(JTapiOutgoingCallControlPanelCallTest.script1.FullName, args);

			if(!WaitForSignal( JTapiOutgoingCallControlPanelCallTest.script1.S_MakeCallComplete.FullName, 10 ) )
			{
				log.Write(System.Diagnostics.TraceLevel.Info, "Never received response from the application for JTapiOutgoingCallControlPanel completion");
				return false;
			}

			// Verify the expected signal type.
			if (OutgoingCallReceived)
			{
				Hashtable fields = new Hashtable();
				fields["totalCalls"] = totalCalls;

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
			TestTextInputData toField = new TestTextInputData("To Number", 
				"The number to call.", to, 80);
			TestTextInputData fromField = new TestTextInputData("From Number", 
				"The number that the call is coming from.", from, 80);
			TestTextInputData deviceNameField = new TestTextInputData("Device Name ", 
				"The device to call from.", deviceName, 160);
            
			TestTextInputData blindXferField = new TestTextInputData(blindXferDn, "Blind Xfer to which number?", blindXferDn, "", 80); 
			TestTextInputData confXferField = new TestTextInputData(conferXferDn, "Conference with which number?", conferXferDn, "", 80); 			
			TestUserEvent blindEvent = new TestUserEvent(blindXferButton, "Blind Xfer Now!", blindXferButton, blindXferButton, new CommonTypes.AsyncUserInputCallback(OnBlindEvent));
			TestUserEvent conferenceEvent = new TestUserEvent(conferenceButton, "Conference Now!", conferenceButton, conferenceButton, new CommonTypes.AsyncUserInputCallback(OnConferenceEvent));						
			TestUserEvent hangupEvent = new TestUserEvent(hangup, "Hangup Now!", hangup, hangup, new CommonTypes.AsyncUserInputCallback(OnHangupEvent));
						
			ArrayList inputs = new ArrayList();
			inputs.Add(toField);
			inputs.Add(fromField);
			inputs.Add(deviceNameField);
			inputs.Add(blindXferField);
			inputs.Add(blindEvent);
			inputs.Add(confXferField);
			inputs.Add(conferenceEvent);
			inputs.Add(hangupEvent);

			return inputs;

		}

		#region Test Events


		public void OnMakeCallComplete(ActionMessage im)
		{   
			makeCallSuccess = (bool) im["success"];

			if (!OutgoingCallReceived) 
			{
				OutgoingCallReceived=true;
			}
			//For this test we are only expecting to ever get 2 incoming calls.
			if (routingGuid1=="") 
			{ 
				totalCalls=totalCalls+1;
				//retain the first script pointer
				routingGuid1 = im.RoutingGuid;
				routingGuid=im.RoutingGuid;
				log.Write(System.Diagnostics.TraceLevel.Info, "DEBUG: MakeCallComplete routingGuid: " + routingGuid + " - " + DateTime.Now); 
			} 
			else
				if (!routingGuid1.Equals(im.RoutingGuid)) 
			{
				totalCalls=totalCalls+1;

				Hashtable fields = new Hashtable();
				fields["totalCalls"] = totalCalls;

				SendEvent( JTapiOutgoingCallControlPanelCallTest.script1.E_UpdateGlobals.FullName, routingGuid1, fields);
				SendEvent( JTapiOutgoingCallControlPanelCallTest.script1.E_UpdateGlobals.FullName, im.RoutingGuid, fields);

				fields["reject"] = false;
				log.Write(System.Diagnostics.TraceLevel.Info, "DEBUG: 2nd Outgoing call, totalCalls: " + totalCalls + " - " + DateTime.Now);
				SendEvent( JTapiOutgoingCallControlPanelCallTest.script1.E_OutgoingCall.FullName, im.RoutingGuid, fields);
			}         
		}


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

		#endregion

        #region User Events

		public bool HangupEvent(string name, string @value)
		{
			log.Write(System.Diagnostics.TraceLevel.Verbose, "** DEBUG: HangupEvent");
			return true;
		}

		public bool OnHangupEvent(string name, string @value)
		{
			log.Write(System.Diagnostics.TraceLevel.Verbose, "** DEBUG: OnHangupEvent");
			SendEvent( JTapiOutgoingCallControlPanelCallTest.script1.E_Hangup.FullName, routingGuid1);
			Thread.Sleep(500);
			done = true;
			return true;
		}

		#endregion

		public bool OnBlindEvent(string name, string @value)
		{
			Hashtable fields = new Hashtable();
			fields["to"] = blindXferTo;
			log.Write(System.Diagnostics.TraceLevel.Verbose, "DEBUG: FTF sending E_Blind" + " - " + DateTime.Now);
			SendEvent( JTapiOutgoingCallControlPanelCallTest.script1.E_Blind.FullName, routingGuid1, fields);
			return true;
		}

		public bool OnConferenceEvent(string name, string @value)
		{
			g_fields["with"] = conferXferWith;
            //Now we make a second call from the app
			log.Write(System.Diagnostics.TraceLevel.Verbose, "DEBUG: FTF sending E_Conference with " + conferXferWith + "  - " + DateTime.Now);
            SendEvent( JTapiOutgoingCallControlPanelCallTest.script1.E_Conference.FullName, routingGuid1, g_fields);
            return true;
		}

		public void OnHangup(ActionMessage im)
		{
			log.Write(System.Diagnostics.TraceLevel.Info, "Received Signal Far end hungup.  Ending test." + " - " + DateTime.Now);
			done = true;
		}

		public override string[] GetRequiredTests()
		{
			return new string[] { JTapiOutgoingCallControlPanelCallTest.FullName };
		}

		public override CallbackLink[] GetCallbacks()
		{
			return new CallbackLink[] { 
			    new CallbackLink( JTapiOutgoingCallControlPanelCallTest.script1.S_MakeCallComplete.FullName,
				new FunctionalTestSignalDelegate(OnMakeCallComplete)),
	            new CallbackLink( JTapiOutgoingCallControlPanelCallTest.script1.S_CallActive.FullName,
				new FunctionalTestSignalDelegate(OnCallActive)),
				new CallbackLink( JTapiOutgoingCallControlPanelCallTest.script1.S_BlindSuccess.FullName,
				new FunctionalTestSignalDelegate(OnBlindResponse)),
				new CallbackLink( JTapiOutgoingCallControlPanelCallTest.script1.S_BlindFail.FullName,
				new FunctionalTestSignalDelegate(OnBlindResponse)),
				new CallbackLink( JTapiOutgoingCallControlPanelCallTest.script1.S_ConferenceSuccess.FullName,
				new FunctionalTestSignalDelegate(OnConferenceResponse)),
				new CallbackLink( JTapiOutgoingCallControlPanelCallTest.script1.S_ConferenceFail.FullName,
				new FunctionalTestSignalDelegate(OnConferenceResponse)),
				new CallbackLink( JTapiOutgoingCallControlPanelCallTest.script1.S_Hangup.FullName,
				new FunctionalTestSignalDelegate(OnHangup)) 													  
		   };                                     
		}
	} 
}
