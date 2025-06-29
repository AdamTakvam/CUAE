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
    [FunctionalTestImpl(IsAutomated=false)]
    public class ActiveRelay1 : FunctionalTestBase
    {
        public const string callControlType = "Call Route Group";
        public const string bhcaInput = "BHCA";
        public const string talkTimeInput = "Talk Time";
        public const string testTimeInput = "Test Time";
        public const string mysqlUserInput = "MySQL Username";
        public const string mysqlPassInput = "MySQL Password";
        public const string mysqlIpInput   = "MySQL Host IP Address";
        public const string useMoreServers  = "Use Two Servers for CallGen";
        public const string minAnsInput    = "Minimum Answer Time";
        public const string maxAnsInput    = "Maximum Answer Time";
        public const string minConfInput   = "Minimum Confirmation Time";
        public const string maxConfInput   = "Maximum Confirmation Time";

        public int callerServer;
        public int receiverServer;
        public int minAnsTime;
        public int maxAnsTime;
        public int minConfTime;
        public int maxConfTime;
        private Hashtable makeCallFields;
        private Hashtable acceptCallFields;
        private Hashtable fromNumberLookup;
        private Hashtable callGenIdToArCall;
        private Hashtable fromNumberToArCall;
        private Hashtable callGenIdToRoutingGuid;
        private Hashtable emptyHashtable;
        private Hashtable callIdToArCall;
        private ActiveRelayProgress controller;
        private static string mapPrepend = "1"; 
        private CallGenerator generator;
        private int callCount;

        public ActiveRelay1 () : base(typeof( ActiveRelay1 ))
        {
            this.callCount              = 0;
            this.Instructions           = "Push start";
            this.makeCallFields         = new Hashtable();
            this.acceptCallFields       = new Hashtable();
            this.emptyHashtable         = new Hashtable();
            this.fromNumberLookup       = new Hashtable();
            this.callIdToArCall         = new Hashtable();
            this.callGenIdToArCall      = new Hashtable();
            this.fromNumberToArCall     = new Hashtable();
            this.callGenIdToRoutingGuid = new Hashtable();
            this.controller             = new ActiveRelayProgress();
        }

        public override bool Execute()
        {
            if(Convert.ToBoolean(input[useMoreServers]))
            {
                callerServer = 0;
                receiverServer = 1;
            }
            else
            {
                callerServer = 0;
                receiverServer = 0;
            }

            minAnsTime = Convert.ToInt32(input[minAnsInput]);
            maxAnsTime = Convert.ToInt32(input[maxAnsInput]);
            minConfTime = Convert.ToInt32(input[minConfInput]);
            maxConfTime = Convert.ToInt32(input[maxConfInput]);

            updateMediaRouteGroup(MakeCallTest.Name, Constants.DefaultMediaGroup);
            updateMediaRouteGroup(AnswerCallTest.Name, Constants.DefaultMediaGroup);

            updateCallRouteGroup(MakeCallTest.Name, (Constants.CallRouteGroupTypes) Enum.Parse(typeof(Constants.CallRouteGroupTypes), 
                input[callControlType] as string, true));
            
            ManagementCommunicator.Instance.RefreshApplicationConfiguration(MakeCallTest.Name);
            ManagementCommunicator.Instance.RefreshApplicationConfiguration(AnswerCallTest.Name);

            generator = new CallGenerator();
            generator.CallRequested += new Metreos.FunctionalTests.App.CallGenerator.CallRequest(CallRequested);
            generator.EndCallRequested += new Metreos.FunctionalTests.App.CallGenerator.EndCallRequest(EndCallRequested);
            
            StartListening();

            ActiveRelayUserImporter userImporter = new ActiveRelayUserImporter();

            ArrayList arUsers = null;
            try
            {
                arUsers = userImporter.DownloadUsers(input[mysqlUserInput] as String, input[mysqlPassInput] as String, 
                    input[mysqlIpInput] as String);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Unable to download user information.  " + Exceptions.FormatException(e));
                return false;
            }

            // Create map of users 
            CreateFromUserLookup(arUsers, mapPrepend);

            generator.BHCAGen(arUsers, int.Parse(input[bhcaInput] as string), int.Parse(input[talkTimeInput] as string),
                int.Parse(input[testTimeInput] as string), mapPrepend);
            generator.WaitForTestEnd();

            log.Write(TraceLevel.Info, controller.Report());

            StopListening();
            return true;
        }

        private void CreateFromUserLookup(ArrayList users, string mapPrepend)
        {
            fromNumberLookup.Clear();
            foreach(ActiveRelayUser user in users)
            {
                string fullFrom = (mapPrepend + user.extension);
                fullFrom = String.Intern(fullFrom);
                fromNumberLookup[fullFrom] = user;   
            }
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData callRouteGroupField = new TestTextInputData(callControlType, "Enter Call Route Group Name-- H323 | CTI",
                callControlType, "", 80); 
            TestTextInputData bhcaField = new TestTextInputData(bhcaInput, "BHCA", bhcaInput, "600", 80); 
            TestTextInputData talkTimeField = new TestTextInputData(talkTimeInput, "Talk Time for calls in ms", talkTimeInput, "5000", 80); 
            TestTextInputData testTimeField = new TestTextInputData(testTimeInput, "Test Time in s", testTimeInput, "60", 80); 
            TestTextInputData mysqlUserField = new TestTextInputData(mysqlUserInput, "MySQL Username", mysqlUserInput, "root2", 80); 
            TestTextInputData mysqlPassField = new TestTextInputData(mysqlPassInput, "MySQL Password", mysqlPassInput, "metreos", 80); 
            TestTextInputData mysqlIpField  = new TestTextInputData(mysqlIpInput, "The IP address of the MySQL server", mysqlIpInput, 
                "10.1.14.118", 80); 
            TestTextInputData minAnsTime  = new TestTextInputData(minAnsInput, "Minimum Amount of Time to Answer", minAnsInput, 
                "1", 80); 
            TestTextInputData maxAnsTime  = new TestTextInputData(maxAnsInput, "Maximum Amount of Time to Answer", maxAnsInput, 
                "1", 80); 
            TestTextInputData minConfTime  = new TestTextInputData(minConfInput, "Minimum Amount of Time to Confirm", minConfInput, 
                "1", 80); 
            TestTextInputData maxConfTime  = new TestTextInputData(maxConfInput, "Maximum Amount of Time to Confirm", maxConfInput, 
                "1", 80); 
            TestBooleanInputData useTwoServers = new TestBooleanInputData(useMoreServers, "Use Two Servers", useMoreServers, false);

            ArrayList inputs = new ArrayList();
            inputs.Add(callRouteGroupField);
            inputs.Add(bhcaField);
            inputs.Add(minAnsTime);
            inputs.Add(maxAnsTime);
            inputs.Add(minConfTime);
            inputs.Add(maxConfTime);
            inputs.Add(useTwoServers);
            inputs.Add(talkTimeField);
            inputs.Add(testTimeField);
            inputs.Add(mysqlUserField);
            inputs.Add(mysqlPassField);
            inputs.Add(mysqlIpField);

            return inputs;
        }
        
        private void CallRequested(long id, string from, string to, ActiveRelayUser user)
        {
            log.Write(TraceLevel.Info, "Making call: ID {0} FROM {1} TO {2}", id, from, to);

            if(++callCount % 10 == 0)
            {
                log.Write(TraceLevel.Info, "! " + controller.Report()); 
            }

            // Trigger an instance of the MakeCall application
            lock(makeCallFields.SyncRoot)
            {
                makeCallFields.Clear();
                makeCallFields["to"] = to;
                makeCallFields["from"] = from;
                makeCallFields["id"] = id;

                ActiveRelayCall call = controller.NewCall(id, user, minAnsTime, maxAnsTime, minConfTime, maxConfTime);
                call.AnswerCallRequest += new AnswerCall(AnswerCallRequest);
                call.ConfirmCallRequest += new ConfirmCall(ConfirmCallRequest);
                call.CallCompleted += new CallCompletion(CallCompleted);

                lock(callGenIdToArCall.SyncRoot)
                {
                    callGenIdToArCall[id] = call;
                }

                lock(fromNumberToArCall.SyncRoot)
                {
                    fromNumberToArCall[from] = call;
                }
                
                log.Write(TraceLevel.Info, controller.CurrentReport());

                TriggerScript(callerServer, MakeCallTest.script1.FullName, makeCallFields);
            }
        }

        private void EndCallRequested(long id)
        {
            lock(callGenIdToArCall.SyncRoot)
            {
                if(callGenIdToArCall.Contains(id))
                {
                    ActiveRelayCall call = callGenIdToArCall[id] as ActiveRelayCall;
                    if(call.OutboundCallCompleted(true))
                    {
                        generator.CallStarted(call.id);
                    }
                    callGenIdToArCall.Remove(id);
                }
            }

            lock(callGenIdToRoutingGuid.SyncRoot)
            {
                if(callGenIdToRoutingGuid.Contains(id))
                {
                    string routingGuid = callGenIdToRoutingGuid[id] as String; 
                    callGenIdToRoutingGuid.Remove(id);
                    SendEvent(callerServer, MakeCallTest.script1.E_Hangup.FullName, routingGuid, emptyHashtable);
                    log.Write(TraceLevel.Info, "Ending call: ID {0}", id);
                }
            }
        }

        public void OnMakeCallAttempt(ActionMessage im)
        {
            string routingGuid = im.RoutingGuid;
            string callId = null;
            long id = -1;
            bool success = (bool)im["success"];
            id = (long) im["id"];
            if(success) 
            {
                callId = (string) im["callId"]; 
                callGenIdToRoutingGuid[id] = routingGuid; 
                log.Write(TraceLevel.Verbose, "MakeCall ATTEMPT {0} ID={1} CALLID={2}", "SUCCESS", id, callId);
            }
            else
            {
                controller.totalInboundFailedCalls++;

                lock(callGenIdToArCall.SyncRoot)
                {
                    if(callGenIdToArCall.Contains(id))
                    {
                        ActiveRelayCall call = callGenIdToArCall[id] as ActiveRelayCall;
                        if(call.OutboundCallCompleted(false))
                        {
                            generator.CallStarted(call.id);
                        }
                        callGenIdToArCall.Remove(id);
                    }
                }
                lock(callGenIdToRoutingGuid.SyncRoot)
                {
                    if(callGenIdToRoutingGuid.Contains(id))
                    {
                        callGenIdToRoutingGuid.Remove(id);
                    }
                }
                log.Write(TraceLevel.Error, "MakeCall ATTEMPT {0} ID={1} CALLID={2}", "FAILURE", id, callId);
                log.Write(TraceLevel.Error, "FAILED-INBOUND {0}", controller.totalInboundFailedCalls);
            }
        }

        public void OnMakeCallComplete(ActionMessage im)
        {
            string routingGuid = im.RoutingGuid;
            bool success = (bool) im["success"];
            string callId = (string) im["callId"];
            long id = (long) im["id"];
            string endReason = null;

            lock(callGenIdToArCall.SyncRoot)
            {
                if(callGenIdToArCall.Contains(id))
                {
                    ActiveRelayCall call = callGenIdToArCall[id] as ActiveRelayCall;
                    if(call.OutboundCallCompleted(true))
                    {
                        generator.CallStarted(call.id);
                    }
                    callGenIdToArCall.Remove(id);
                }
            }

            if(success)
            {
                log.Write(TraceLevel.Verbose, "MakeCall COMPLETE {0} CALLID={1} ENDREASON={2}", "SUCCESS",  callId, "NORMAL");
            }
            else
            {
                controller.totalInboundFailedCalls++;
                endReason = (string) im["endReason"];
                log.Write(TraceLevel.Error, "MakeCall COMPLETE {0} CALLID={1} ENDREASON={2}", "FAILURE",  callId, endReason);
                log.Write(TraceLevel.Error, "FAILED-INBOUND {0}", controller.totalInboundFailedCalls);
                lock(callGenIdToRoutingGuid.SyncRoot)
                {
                    if(callGenIdToRoutingGuid.Contains(id))
                    {
                        callGenIdToRoutingGuid.Remove(id);
                    }
                }
            }
        }

        public void OnMakeCallHangup(ActionMessage im)
        {
            string routingGuid = im.RoutingGuid;
            string callId = (string) im["callId"];
            long id = (long) im["id"];

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

            // take off prepend
            //from = from.Substring(mapPrepend.Length);

            ActiveRelayCall call = fromNumberToArCall[from] as ActiveRelayCall;

            callIdToArCall[callId] = new object[] { call, to };

            call.FindMeCallReceived(to, routingGuid, callId);
        }

        public void OnAnswerCallAck(ActionMessage im)
        {
            string routingGuid = im.RoutingGuid;
            string callId = (string) im["callId"];
            bool success = (bool) im["success"];

            object[] stuff = callIdToArCall[callId] as object[];

            ActiveRelayCall call = stuff[0] as ActiveRelayCall;
            string to = stuff[1] as string;
            callIdToArCall.Remove(callId);

            if(call.FindMeCallCompleted(success, to))
            {
                generator.CallStarted(call.id);
            }

            if(success)
            {
                log.Write(TraceLevel.Verbose, "AnswerCall ANSWER_ACK {0} CALLID={1}", "SUCCESS", callId);
            }
            else
            {
                controller.totalOutboundFailedCalls++;
                log.Write(TraceLevel.Error, "AnswerCall ANSWER_ACK {0} CALLID={1}", "FAILURE", callId);
                log.Write(TraceLevel.Error, "FAILED-OUTBOUND {0}", controller.totalOutboundFailedCalls);
            }
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
            this.controller = new ActiveRelayProgress();
            this.fromNumberLookup.Clear();
            this.fromNumberToArCall.Clear();
            this.callGenIdToRoutingGuid.Clear();
            this.callGenIdToArCall.Clear();
            this.callIdToArCall.Clear();
            this.emptyHashtable.Clear();
            if(generator != null)
            {
                generator.Stop(120000);
                generator = null;
            }
            this.callCount = 0;
        }

        public override void Cleanup()
        {
            this.controller = new ActiveRelayProgress();
            this.fromNumberLookup.Clear();
            this.fromNumberToArCall.Clear();
            this.callGenIdToRoutingGuid.Clear();
            this.callGenIdToArCall.Clear();
            this.callIdToArCall.Clear();
            this.emptyHashtable.Clear();
            if(generator != null)
            {
                generator.Stop(120000);
                generator = null;
            }
            this.callCount = 0;
        }

        /// <summary> 
        ///     This is fired by the ActiveRelayCall object... if it fires,
        ///     the test knows its time to tell the application to answer the call</summary>
        /// <param name="id"> The BHCAGen-based callIDs </param>
        private void AnswerCallRequest(string appId, string callId)
        {
            log.Write(TraceLevel.Info, "ANSWERING CALLID={0}", callId);
            SendEvent(receiverServer, AnswerCallTest.script1.E_AnswerCallAck.FullName, appId, emptyHashtable);
        }

        private void ConfirmCallRequest(string appId, string callId)
        {
            log.Write(TraceLevel.Info, "CONFIRMING CALLID={0}", callId);
            SendEvent(receiverServer, AnswerCallTest.script1.E_SendConfirmDigit.FullName, appId, emptyHashtable);
        }

        private void CallCompleted(ActiveRelayCall call, CallCompletion callCompleteDelegate, CallCompleteReason reason)
        {
            // Call completed!  So, lets clean up what we can
            
            lock(fromNumberToArCall.SyncRoot)
            {
                if(fromNumberToArCall.Contains(call.user.extension))
                {
                    fromNumberToArCall.Remove(call.user.extension);
                }
            }

            call.CallCompleted -= callCompleteDelegate;
            log.Write(TraceLevel.Info, "CALL SEQUENCE COMPLETE: {0}", reason.ToString());
        }

        public override bool StopRequest(int timeout)
        {
            bool stopped = true;
            if(generator != null)
            {
                stopped &= generator.Stop(120000);
                log.Write(TraceLevel.Info, "Call Generator stopped");
                log.Write(TraceLevel.Info, "Waiting for all outstanding calls to complete.");
                stopped &= controller.Wait(120000);
            }
            return stopped;
        }
    } 
}