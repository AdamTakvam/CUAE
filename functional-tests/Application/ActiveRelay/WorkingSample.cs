using System;
using System.Diagnostics;
using System.Data;
using System.Threading;
using System.Collections;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Samoa.FunctionalTestFramework;
using Metreos.FunctionalTests.App;

using AnswerCallTest = Metreos.TestBank.App.App.AnswerCall;
using MakeCallTest = Metreos.TestBank.App.App.MakeCall;

namespace Metreos.FunctionalTests.App.ActiveRelay
{
    public class ActiveRelaySample : FunctionalTestBase
    {
        public const string callControlType = "Call Route Group";

        public const int callerServer = 0;
        public const int receiverServer = 1;
        private Hashtable makeCallFields;
        private Hashtable acceptCallFields;

        public ActiveRelaySample() : base(typeof( ActiveRelaySample ))
        {
            this.Instructions = "Push start";
            this.makeCallFields = new Hashtable();
            this.acceptCallFields = new Hashtable();
        }

        public override bool Execute()
        {
            updateMediaRouteGroup(MakeCallTest.Name, Constants.DefaultMediaGroup);
            updateMediaRouteGroup(AnswerCallTest.Name, Constants.DefaultMediaGroup);

            //updateCallRouteGroup(MakeCallTest.Name, (Constants.CallRouteGroupTypes) Enum.Parse(typeof(Constants.CallRouteGroupTypes), input[callControlType] as string, true));
            
            ManagementCommunicator.Instance.RefreshApplicationConfiguration(MakeCallTest.Name);
            ManagementCommunicator.Instance.RefreshApplicationConfiguration(AnswerCallTest.Name);

            CallGenerator generator = new CallGenerator();
            generator.CallRequested += new Metreos.FunctionalTests.App.CallGenerator.CallRequest(CallRequested);
            generator.EndCallRequested += new Metreos.FunctionalTests.App.CallGenerator.EndCallRequest(EndCallRequested);
            
            string[] callers;
            string[] receivers;
            string path = null;
            bool loaded = LoadPhoneNumbers(path, out callers, out receivers);

            if(!loaded)
            {
                log.Write(TraceLevel.Error, "Unable to load the caller/receiver list list");
                return false;
            }

            callers = new string[] { "2000", "2001" };
            receivers = new string[] {"2002", "2003" };

            StartListening();
            generator.BHCAGen(callers, receivers, 2000, 5000, 60);
            generator.WaitForTestEnd();
            return true;
        }


        private bool LoadPhoneNumbers(string path, out string[] callers, out string[] receivers)
        {
            callers = null;
            receivers = null;

            return true;
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData callRouteGroupField = new TestTextInputData(callControlType, "Enter Call Route Group Name-- H323 | CTI", callControlType, "", 80); 

            ArrayList inputs = new ArrayList();
            inputs.Add(callRouteGroupField);
            return inputs;
        }
        
        private void CallRequested(long id, string from, string to, ActiveRelayUser user)
        {
            log.Write(TraceLevel.Info, "Making call: ID {0} FROM {1} TO {2}", id, from, to);
            
            // Trigger an instance of the MakeCall application
            lock(makeCallFields.SyncRoot)
            {
                makeCallFields.Clear();
                makeCallFields["to"] = to;
                makeCallFields["from"] = from;
                makeCallFields["id"] = id;
                TriggerScript(MakeCallTest.script1.FullName, makeCallFields);
            }
        }

        private void EndCallRequested(long id)
        {
            log.Write(TraceLevel.Info, "Ending call: ID {0}", id);
        }

        public void OnMakeCallAttempt(ActionMessage im)
        {
            string routingGuid = im.RoutingGuid;
            string callId = null;
            long id = -1;
            bool success = (bool)im["success"];
            id = (long) im["id"];
            if(success) { callId = (string) im["callId"]; }

            log.Write(TraceLevel.Verbose, "MakeCall ATTEMPT {0} ID={1} CALLID={2}", success ? "SUCCESS" : "FAILURE", id, callId);
        }

        public void OnMakeCallComplete(ActionMessage im)
        {
            string routingGuid = im.RoutingGuid;
            bool success = (bool) im["success"];
            string callId = (string) im["callId"];
            string endReason = null;
            if(!success) endReason = (string) im["endReason"];

            log.Write(TraceLevel.Verbose, "MakeCall COMPLETE {0} CALLID={1} ENDREASON={2}", success ? "SUCCESS" : "FAILURE",  callId, success ? "NORMAL": endReason);
        }

        public void OnMakeCallHangup(ActionMessage im)
        {
            string routingGuid = im.RoutingGuid;
            string callId = (string) im["callId"];

            log.Write(TraceLevel.Verbose, "MakeCall HANGUP CALLID={0}", callId);
        }

        public void OnIncomingCall(ActionMessage im)
        {
            string routingGuid = im.RoutingGuid;
            string callId = (string) im["callId"];
            string to = (string) im["to"];
            string from = (string) im["from"];
            //string originalTo = (string) im["originalTo"]; Does anyone really care? no.

            log.Write(TraceLevel.Verbose, "AnswerCall INCOMING CALLID={0} TO={1} FROM={2}", callId, to, from);
            
            lock(acceptCallFields.SyncRoot)
            {
                acceptCallFields.Clear();
                acceptCallFields["callId"] = callId;
                acceptCallFields["dtmfDelay"] = 0;

                SendEvent(AnswerCallTest.script1.E_AnswerCallAck.FullName, routingGuid, acceptCallFields);
            }
        }

        public void OnAnswerCallAck(ActionMessage im)
        {
            string routingGuid = im.RoutingGuid;
            string callId = (string) im["callId"];
            bool success = (bool) im["success"];

            log.Write(TraceLevel.Verbose, "AnswerCall ACCEPT_ACK {0} CALLID={1}", success ? "SUCCESS" : "FAILURE", callId);
        }

        public void OnAnswerCallHangup(ActionMessage im)
        {
            string routingGuid = im.RoutingGuid;
            string callId = (string) im["callId"];

            log.Write(TraceLevel.Verbose, "AnswerCall HANGUP CALLID={0}", callId);
        }


        public override string[] GetRequiredTests()
        {
            return new string[] { AnswerCallTest.FullName, MakeCallTest.FullName };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( MakeCallTest.script1.S_Hangup.FullName, 
                                          new FunctionalTestSignalDelegate( OnMakeCallHangup )),
                                          new CallbackLink( MakeCallTest.script1.S_MakeCall.FullName,
                                          new FunctionalTestSignalDelegate( OnMakeCallAttempt )),
                                          new CallbackLink( MakeCallTest.script1.S_MakeCallComplete.FullName,
                                          new FunctionalTestSignalDelegate( OnMakeCallComplete )),
                                            
                                          new CallbackLink( AnswerCallTest.script1.S_IncomingCall.FullName,
                                          new FunctionalTestSignalDelegate( OnIncomingCall )),
                                          new CallbackLink( AnswerCallTest.script1.S_AnswerCall.FullName,
                                          new FunctionalTestSignalDelegate( OnAnswerCallAck )),
                                          new CallbackLink( AnswerCallTest.script1.S_Hangup.FullName,
                                          new FunctionalTestSignalDelegate( OnAnswerCallHangup )) };

        }

        
        public override void Initialize()
        {
        }

        public override void Cleanup()
        {
        }
    } 
}