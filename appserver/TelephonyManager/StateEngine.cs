using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Core;
using Metreos.Stats;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;

using Metreos.Configuration;

namespace Metreos.AppServer.TelephonyManager
{
    internal delegate int ScalarQueryDelegate();

    internal sealed class StateEngine
    {
        private abstract class Consts
        {
            public const string ScriptExtension     = ".tms";
            public const string ScriptFileFilter    = "*" + ScriptExtension;
            public const int ShutdownTimeout        = 5000;
        }

        internal ScalarQueryDelegate GetQueueSize;

        private LogWriter log;
        private readonly IConfigUtility configUtility;
        private readonly CallIdFactory callIdFactory;
        private readonly StatsClient statsClient;

        private volatile bool shutdown = false;
        private readonly Thread schedulerThread;

        private readonly CallTable callTable;
        private readonly ArrayList defunctCalls;         // CallInfo objects
        private readonly Hashtable stateMaps;            // Map name -> StateMap

        private MessageGenerator msgGen;
        private ActionHandler actionHandler;
        private InternalMessageHandler msgHandler;

        

        private readonly ArrayList newCalls;    // CallInfo objects requiring script execution

        private bool enableSandbox = false;
        public bool EnableSandbox { set { enableSandbox = value; } }

		private bool enableDiags = false;
		public bool EnableDiags { set { enableDiags = value; } }

        #region Construction/Start/Dispose

        public StateEngine(IConfigUtility configUtility)
        {
            this.configUtility = configUtility;
            this.callIdFactory = CallIdFactory.Instance;
            this.statsClient = StatsClient.Instance;

            this.newCalls = ArrayList.Synchronized(new ArrayList());
            
            this.callTable = new CallTable();
            this.defunctCalls = ArrayList.Synchronized(new ArrayList());
            this.stateMaps = Hashtable.Synchronized(new Hashtable());

            this.schedulerThread = new Thread(new ThreadStart(Run));
            this.schedulerThread.IsBackground = true;
            this.schedulerThread.Name = "Telephony Manager Execution Engine";
            this.schedulerThread.Priority = ThreadPriority.AboveNormal;
        }

        public bool Start(string scriptDir, LogWriter log)
        {
            if(log == null) { return false; }
            this.log = log;

            this.msgGen = new MessageGenerator();

            try 
            { 
                this.actionHandler = new ActionHandler(msgGen, configUtility, log);

                this.msgHandler = new InternalMessageHandler(msgGen, actionHandler, configUtility, log);
                this.msgHandler.AddNewCall = new CallDelegate(AddNewCall);
                this.msgHandler.GetNewCallsLock = new LockDelegate(GetNewCallsLock);
                this.msgHandler.GetNewCalls = new GetArrayDelegate(GetNewCalls);
                this.msgHandler.GetCallInfo = new CallQueryDelegate(GetCallInfo);
                this.msgHandler.GetMediaCallInfo = new MediaCallQueryDelegate(GetMediaCallInfo);
                this.msgHandler.ChangeStateMap = new StateMapDelegate(ChangeStateMap);
                this.msgHandler.TerminateCall = new TerminateCallDelegate(TerminateCall);
                this.msgHandler.GetStateMap = new StateMapQueryDelegate(GetStateMap);

                this.actionHandler.GetCrg = new GetCrgDelegate(msgHandler.crgCache.GetData);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                return false;
            }

            DirectoryInfo dInfo = new DirectoryInfo(scriptDir);
            if(dInfo.Exists == false)
            {
                log.Write(TraceLevel.Error,
                    "AppServer installation corrupt: Telephony Manager scripts directory ({0}) does not exist.",
                    scriptDir);
                return false;
            }

            foreach(FileInfo scriptFile in dInfo.GetFiles(Consts.ScriptFileFilter))
            {
                try
                {
                    FileStream fStream = scriptFile.OpenRead();
                    StreamReader reader = new StreamReader(fStream);
                    string scriptStr = reader.ReadToEnd();

                    string eventName;
                    string scriptName = scriptFile.Name.Replace(Consts.ScriptExtension, "");
                    StateMap map = new StateMap();

                    if(map.LoadScript(scriptStr, scriptName, log, out eventName) == false)
                        return false;

                    stateMaps[scriptName] = map;

                    if(eventName != null)
                    {
                        msgHandler.AddTrigger(eventName, scriptName);
                    }
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, 
                        "Unable to load TelephonyManager script '{0}': {1}", scriptFile.Name, e.Message);
                    return false;
                }
            }

            if(stateMaps.Count == 0)
            {
                log.Write(TraceLevel.Error, "No TelephonyManager scripts found");
                return false;
            }

            // Set num calls to zero
            statsClient.SetStatistic(IStats.Statistics.ActiveCalls, 0);

            schedulerThread.Start();
            return true;
        }

        /// <summary>Stops state engine.</summary>
        /// <remarks> Restart not supported.</remarks>
        public void Stop()
        {
            shutdown = true;

            if(schedulerThread != null)
            {                
                if(schedulerThread.Join(Consts.ShutdownTimeout) == false)
                {
                    schedulerThread.Abort();
                }
            }

            EndAllCalls();

            // Set num calls to zero
            statsClient.SetStatistic(IStats.Statistics.ActiveCalls, 0);
        }

        #endregion

        #region Scheduler Thread

        private void Run()
        {
			long startTime = 0;
			long lastLogTime = HPTimer.Now();

            while(shutdown == false)
            {
                Thread.Sleep(1);   // Throttle

				ArrayList activeCalls = callTable.GetActiveCalls();

				if(this.enableDiags && HPTimer.SecondsSince(lastLogTime) > 5)
				{
                    System.Text.StringBuilder sb = new System.Text.StringBuilder(CreateStandardDiagMessage());

                    if(startTime > 0)
                    {
                        sb.Append("\r\nTime to service call action: ");
                        sb.Append(HPTimer.MillisSince(startTime));
                        sb.Append("ms");

                        log.Write(TraceLevel.Info, sb.ToString());
                    }

					lastLogTime = HPTimer.Now();
				}

				startTime = HPTimer.Now();

                foreach(CallInfo cInfo in activeCalls)
                {
                    if(cInfo.Running)
                    {
                        ExecuteCurrentAction(cInfo);
                        continue;
                    }

                    lock(cInfo.StateLock)
                    {
                        // State determination is not trivial, so pass it around
                        //   instead of recomputing it each time.
                        ushort state = (ushort)cInfo.State;
						if(state != (ushort)CallState.Idle)
						{
							if(CheckForResponse(cInfo, state)) { continue; }
							else if(CheckForAction(cInfo, state)) { continue; }
							else if(CheckForEvent(cInfo, state)) { continue; }
							else if(CheckForTimeout(cInfo, state)) { continue; }
							else if (CheckForData(cInfo, state)) { continue; }
							else { CheckForDefault(cInfo, state); }
						}
                    }
                }

                RemoveDefunctCalls();

                // Check for new incoming calls
                lock(newCalls.SyncRoot)
                {
                    if(newCalls.Count > 0)
                    {
                        foreach(CallInfo cInfo in newCalls)
                        {
                            callTable.AddCall(cInfo);
                        }
                        newCalls.Clear();

                        // Update call statistic
                        statsClient.SetStatistic(IStats.Statistics.ActiveCalls, callTable.Count);
                    }
                } 
            }
        }

        private void RemoveDefunctCalls()
        {
            if(defunctCalls.Count == 0) { return; }

            ArrayList removedCalls = new ArrayList();

            lock(defunctCalls.SyncRoot)
            {
                foreach(CallInfo cInfo in defunctCalls)
                {
                    // This check prevents an exiting script from causing the state map to end prematurely.
                    if(cInfo.FatalError || cInfo.CurrentState.action == Actions.EndCall)
                    {
                        callTable.RemoveCall(cInfo.RoutingGuid, cInfo.CallId);

                        // Clear outstanding calls and connections
                        if(cInfo.CallIdSpecified && cInfo.CallTerminated == false)
                        {
                            if(cInfo.CallEstablished)
                            {
                                ActionMessage msg = msgGen.CreateHangupAction(cInfo, true);
                                actionHandler.RouterQ.PostMessage(msg);
                            }
                            else
                            {
                                ActionMessage msg = msgGen.CreateRejectCallAction(cInfo, true);
                                actionHandler.RouterQ.PostMessage(msg);
                            }
                        }

                        if(cInfo.ConnectionId > 0)
                            actionHandler.DeleteConnection(cInfo, true);

                        cInfo.Dispose();

                        removedCalls.Add(cInfo);
                    }
                }
            }

            if(removedCalls.Count > 0)
            {
                // Update statistic
                statsClient.SetStatistic(IStats.Statistics.ActiveCalls, callTable.Count);

                foreach(CallInfo cInfo in removedCalls)
                {
                    defunctCalls.Remove(cInfo);
                }
                removedCalls.Clear();
            }
        }

        private void ExecuteCurrentAction(CallInfo cInfo)
        {
            cInfo.CurrStateExecTime = HPTimer.Now();

            switch(cInfo.CurrentState.action)
            {
                case Actions.ForwardEventToApp:
                    if(!actionHandler.ForwardEventToApp(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.ForwardResponseToApp:
                    if(!actionHandler.ForwardResponseToApp(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.ForwardActionToProvider:
                    if(!actionHandler.ForwardActionToProvider(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.SendActionFailureToApp:
                    if(!actionHandler.SendActionFailureToApp(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.SendMakeCallCompleteToApp:
                    if(!actionHandler.SendMakeCallCompleteToApp(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.SendMakeCallFailedToApp:
                    if(!actionHandler.SendMakeCallFailedToApp(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.SendBridgeSuccessToApp:
                    if(!actionHandler.SendBridgeSuccessToApp(cInfo, GetPeerInfo(cInfo))) { TerminateCall(cInfo, true); }
                    break;
                case Actions.SendUnbridgeSuccessToApp:
                    if(!actionHandler.SendUnbridgeSuccessToApp(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.SendHangupToApp:
                    if(!actionHandler.SendHangupToApp(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.SendStartTxToApp:
                    if(!actionHandler.SendStartTxToApp(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.SendStartRxToApp:
                    if(!actionHandler.SendStartRxToApp(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.SendStopTxToApp:
                    if(!actionHandler.SendStopTxToApp(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.HangupCall:
                    if(!actionHandler.HangupCall(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.RejectCall:
                    if(!actionHandler.RejectCall(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.AcceptCall:
                    if(!actionHandler.AcceptCall(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.HoldCall:
                    if(!actionHandler.HoldCall(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.ResumeCall:
                    if(!actionHandler.ResumeCall(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.GetMediaCaps:
                    if(!actionHandler.GetMediaCaps(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.ReserveConnection:
                    if(!actionHandler.ReserveConnection(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.CreateConnection:
                    if(!actionHandler.CreateConnection(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.ModifyConnection:
                    if(!actionHandler.ModifyConnection(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.DeleteConnection:
                    if(!actionHandler.DeleteConnection(cInfo, false)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.CreateConference:
                    if(!actionHandler.CreateConference(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.JoinConference:
                    if(!actionHandler.JoinConference(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.StopMediaOperation:
                    if(!actionHandler.StopMediaOperation(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.SetMedia:
                    if(!actionHandler.SetMedia(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.ClearMediaInfo:
                    if(!actionHandler.ClearMediaInfo(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.SelectTxCodec:
                    if(!actionHandler.SelectTxCodec(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.AssumePreferredTxCodec:
                    if(!actionHandler.AssumePreferredTxCodec(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.SyncPeerMedia:
                    if(!actionHandler.SyncPeerMedia(cInfo, GetPeerInfo(cInfo), false)) { TerminateCall(cInfo, true); }
                    break;  
                case Actions.ClearPeerMediaInfo:
                    if(!actionHandler.ClearPeerMediaInfo(cInfo, GetPeerInfo(cInfo))) { TerminateCall(cInfo, true); }
                    break; 
                case Actions.ForwardResponseToPeer:
                    if(!actionHandler.ForwardResponseToPeer(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.SendActionSuccessToPeer:
                    if(!actionHandler.SendActionSuccessToPeer(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.SendActionFailureToPeer:
                    if(!actionHandler.SendActionFailureToPeer(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.AcceptPeerCall:
                    if(!actionHandler.AcceptPeerCall(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.AnswerPeerCall:
                    if(!actionHandler.AnswerPeerCall(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.HoldPeerCall:
                    if(!actionHandler.HoldPeerCall(cInfo, GetPeerInfo(cInfo))) { TerminateCall(cInfo, true); }
                    break;
                case Actions.ResumePeerCall:
                    if(!actionHandler.ResumePeerCall(cInfo, GetPeerInfo(cInfo))) { TerminateCall(cInfo, true); }
                    break;
                case Actions.JoinPeerConference:
                    if(!actionHandler.JoinPeerConference(cInfo, GetPeerInfo(cInfo))) { TerminateCall(cInfo, true); }
                    break;
                case Actions.CreatePeerConference:
                    if(!actionHandler.CreatePeerConference(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.DeletePeerConnection:
                    if(!actionHandler.DeletePeerConnection(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.SetPeerMedia:
                    if(!actionHandler.SetPeerMedia(cInfo, GetPeerInfo(cInfo))) { TerminateCall(cInfo, true); }
                    break;
                case Actions.UseMohMedia:
                    if(!actionHandler.UseMohMedia(cInfo)) { TerminateCall(cInfo, true); }
                    break;
                case Actions.HangupPeerCall:
                    if(!actionHandler.HangupPeerCall(cInfo, GetPeerInfo(cInfo))) { TerminateCall(cInfo, true); }
                    break;
                case Actions.Wait:
                    actionHandler.FinalizeAction(cInfo);
                    break;
                case Actions.EndScript:
                    lock(cInfo.StateLock)  // This ensures events don't get mishandled
                    {
                        actionHandler.FinalizeAction(cInfo);
                        HandleEndScript(cInfo);
                    }
                    break;
                case Actions.EndCall:
                    actionHandler.FinalizeAction(cInfo);
                    TerminateCall(cInfo, false);
                    break;
            }
        }

        private CallInfo GetPeerInfo(CallInfo cInfo)
        {
            if(cInfo.PeerCallId == 0)
            {
                log.Write(TraceLevel.Error, "No peer specified for: " + cInfo.CallId);
                return null;
            }

            if(cInfo.PeerCallId == cInfo.CallId)
            {
                log.Write(TraceLevel.Error, "Call references itself as peer: " + cInfo.CallId);
                return null;
            }

            CallInfo peerInfo = callTable.GetCallInfo(cInfo.RoutingGuid, cInfo.PeerCallId);
            if(peerInfo == null)
            {
                log.Write(TraceLevel.Warning, "Cannot locate peer for call '{0}'. Peer call ID '{1}' is invalid",
                    cInfo.CallId, cInfo.PeerCallId);
            }

            return peerInfo;
        }

        private bool CheckForResponse(CallInfo cInfo, ushort state)
        {
            if((state & (ushort)CallState.WaitForResponse) != 0)
            {
                ResponseMessage rMsg = cInfo.GetIncomingResponse();
                if(rMsg == null) { return false; }

                string nextStateId = cInfo.CurrentState.responseNextStates[rMsg.MessageId.ToLower()] as string;
                if(nextStateId == null)
                {
                    nextStateId = cInfo.CurrentState.responseNextStates["*"] as string;
                }

                if(nextStateId != null)
                {
                    MoveToNextState(cInfo, nextStateId);
                }
                else
                {
                    log.Write(TraceLevel.Error, "No next state defined for response '{0}' in {1}:{2}",
                        rMsg.MessageId, cInfo.MapName, cInfo.CurrStateId);
                    TerminateCall(cInfo, true);
                }
                return true;
            }
            return false;
        }

        private bool CheckForAction(CallInfo cInfo, ushort state)
        {
            if((state & (ushort)CallState.WaitForAction) != 0)
            {
                string nextStateId;
                if(cInfo.GetIncomingAction(out nextStateId) == null) { return false; }
                MoveToNextState(cInfo, nextStateId);
                return true;
            }
            return false;
        }

        private bool CheckForEvent(CallInfo cInfo, ushort state)
        {
            if((state & (ushort)CallState.WaitForEvent) != 0)
            {
                string nextStateId;
                if(cInfo.GetIncomingEvent(out nextStateId) == null) { return false; }
                MoveToNextState(cInfo, nextStateId);
                return true;
            }
            return false;
        }

        private bool CheckForTimeout(CallInfo cInfo, ushort state)
        {
            if((state & (ushort)CallState.WaitForTimeout) != 0)
            {
                if(HPTimer.Now() > cInfo.CurrStateTimeout)
                {
                    if(cInfo.CurrentState.TimeoutNextState == null)
                    {
                        log.Write(TraceLevel.Error, "Internal Error: Timeout declared, but no next state specified. ({0}:{1})",
                            cInfo.MapName, cInfo.CurrStateId);
                        return false;
                    }

                    if((state | (ushort)CallState.WaitForTimeout) != (ushort)CallState.WaitForTimeout)
                        log.Write(TraceLevel.Warning, "Acting on timeout condition ({0}:{1}:{2}) [q={3},cpu={4}%,mem={5}KB]", 
                            cInfo.CallId, cInfo.MapName, cInfo.CurrStateId, GetQueueSize(), 
                            PerfMonCounter.GetValue(PerfCounterType.CPU_Load),
                            PerfMonCounter.GetMemoryUsage());

                    MoveToNextState(cInfo, cInfo.CurrentState.TimeoutNextState);
                    return true;
                }
            }
            return false;
        }

        private bool CheckForData(CallInfo cInfo, ushort state)
        {
            if((state & (ushort)CallState.CheckForData) != 0)
            {
                foreach(DictionaryEntry de in cInfo.CurrentState.dataNextStates)
                {
                    DataCriteria crit = de.Key as DataCriteria;
                    string nextStateId = de.Value as string;
                    bool success = false;

                    // Early media is a special case because the value comes from app config
                    //   instead of call state/data.
                    if(crit.fieldName == DataFields.earlyMedia)
                    {
                        CrgData data = msgHandler.crgCache[cInfo.AppName, cInfo.PartitionName];
                        if(data == null || data.PartitionInfo == null)
                        {
                            // More like an assertion, really.
                            log.Write(TraceLevel.Error, "No call route group configurated for {2}->{3}",
                                cInfo.MapName, cInfo.CurrStateId, cInfo.AppName, cInfo.PartitionName);
                            continue;
                        }

                        success = data.PartitionInfo.earlyMedia == Convert.ToBoolean(crit.Value);
                    }
                    else
                    {
                        success = cInfo.MatchCriteria(crit);
                    }

                    if(success)
                    {
                        MoveToNextState(cInfo, nextStateId);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckForDefault(CallInfo cInfo, ushort state)
        {
            if((state &= (ushort)CallState.HasDefault) != 0)
            {
                MoveToNextState(cInfo, cInfo.CurrentState.defaultNextState);
                return true;
            }
            return false;
        }

        private void MoveToNextState(CallInfo cInfo, string nextStateId)
        {
            if(nextStateId.IndexOf("[") != -1)
            {
                // Switch to new map
                nextStateId = nextStateId.Trim('[',']');
                if(ChangeStateMap(cInfo, nextStateId) == false)
                {
                    log.Write(TraceLevel.Error, "No map '{0}' found for next state in {1}:{2}",
                        nextStateId, cInfo.MapName, cInfo.CurrStateId);
                    TerminateCall(cInfo, true);
                }
            }
            else if(cInfo.CurrStateMap.Contains(Convert.ToUInt32(nextStateId)))
            {
                cInfo.CurrStateId = Convert.ToUInt32(nextStateId);
            }
            else
            {
                log.Write(TraceLevel.Error, "No next state '{0}' defined in map {1} from state {2}",
                    nextStateId, cInfo.MapName, cInfo.CurrStateId);
                TerminateCall(cInfo, true);
            }
        }

        private void HandleEndScript(CallInfo cInfo)
        {
            log.Write(TraceLevel.Verbose, "Script ended for call '{0}'. Waiting for call service request...", cInfo.CallId);

			callTable.SetInactive(cInfo.CallId);

            // Recycle unhandled actions and events.
            ActionMessage action = cInfo.PopIncomingAction();
			if(action != null)
			{
				this.EnqueueMessage(action);
			}
			else
			{
				EventMessage ev = cInfo.PopIncomingEvent();
				if(ev != null)
					this.EnqueueMessage(ev);
			}
        }

        private void TerminateCall(CallInfo cInfo, bool fatalError, bool sandbox)
        {
            lock(defunctCalls.SyncRoot)
            {
                if(defunctCalls.Contains(cInfo) == false)
                {
                    callTable.SetInactive(cInfo.CallId);

                    cInfo.FatalError = fatalError || sandbox;

                    if(fatalError)
                        log.Write(TraceLevel.Info, "Call {0}:{1} has ended due to an error", cInfo.RoutingGuid, cInfo.CallId);
                    else if(sandbox)
                        log.Write(TraceLevel.Info, "Call {0}:{1} has ended due to call sandboxing", cInfo.RoutingGuid, cInfo.CallId);
                    else
                        log.Write(TraceLevel.Info, "Call {0}:{1} has ended due to normal call termination", cInfo.RoutingGuid, cInfo.CallId);

                    defunctCalls.Add(cInfo);
                }
            }
        }

        #endregion

        #region Utility Methods

        internal void EnqueueMessage(InternalMessage msg)
        {
            msgHandler.EnqueueMessage(msg);
        }

        internal void RegisterNamespace(IConfig.ComponentType protocol, string pNamespace)
        {
            msgHandler.RegisterNamespace(protocol, pNamespace);
        }

        internal void UnregisterNamespace(StringCollection nss)
        {
            msgHandler.UnregisterNamespace(nss);
        }

        internal bool ChangeActionGuid(string oldGuid, string newGuid)
        {
            return callTable.ChangeActionGuid(oldGuid, newGuid);
        }

        /// <summary>Handles the Metreos.ApplicationControl.EndScript action</summary>
        internal void HandleEndScript(string routingGuid, string scriptName)
        {
            if(enableSandbox)
            {
                log.Write(TraceLevel.Info, "Clearing all calls and connections for {0}:{1}", scriptName, routingGuid);

                ArrayList calls = callTable.GetCalls(routingGuid);
                if(calls == null) { return; }

                // Lock defunctCalls here to avoid deadlocking with engine thread.
                lock(defunctCalls.SyncRoot)
                {
                    lock(calls.SyncRoot)
                    {
                        foreach(CallInfo cInfo in calls)
                        {
                            TerminateCall(cInfo, false, true);
                        }
                    }
                }
            }
        }

        internal void ClearCallTable()
        {
            foreach(CallInfo cInfo in callTable.GetAllCalls())
                log.Write(TraceLevel.Warning, "Manually clearing call state for call: " + cInfo.CallId);

            callTable.Clear();
        }

        internal void PrintDiags()
        {
            log.ForceWrite(TraceLevel.Info, CreateStandardDiagMessage());
        }

        private string CreateStandardDiagMessage()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("[Telephony Manager Diagnostics]\r\n");
            sb.Append("Total number of active calls: ");
            sb.AppendLine(callTable.Count.ToString());
            sb.Append("Number of scripts with active calls: ");
            sb.AppendLine(callTable.NumGuids.ToString());
            sb.Append("Number of calls in a transient state: ");
            sb.AppendLine(callTable.NumActiveCalls.ToString());

            if(callTable.Count > 0)
            {
                sb.AppendLine("Call state history:");

                foreach(CallInfo cInfo in callTable.GetAllCalls())
                {
                    sb.AppendFormat("{0}: ", cInfo.CallId);
                    foreach(uint stateId in cInfo.StateHistory)
                    {
                        sb.AppendFormat("{0}, ", stateId);
                    }
                    sb.Remove(sb.Length-2, 2);  // remove trailing comma

                    if(cInfo.MapName != null)
                        sb.AppendFormat(" [{0}]", cInfo.MapName);
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        internal void EndAllCalls()
        {
            ArrayList calls = callTable.GetAllCalls();
            if(calls == null) { return; }

            foreach(CallInfo cInfo in calls)
            {
                if(!shutdown)
                    log.Write(TraceLevel.Info, "Manually terminating call: " + cInfo.CallId);

                if(cInfo.CallIdSpecified)
                {
                    ActionMessage msg = msgGen.CreateHangupAction(cInfo, true);
                    actionHandler.RouterQ.PostMessage(msg);
                }

                if(cInfo.ConnectionId > 0)
                    actionHandler.DeleteConnection(cInfo, true);
            }

            callTable.Clear();
        }

        internal void ClearCrgCache()
        {
            log.Write(TraceLevel.Info, "Clearing CRG cache");
            this.msgHandler.crgCache.Clear();
        }

        #endregion

        #region MessageHandler delegates

        internal void AddNewCall(CallInfo cInfo)
        {
            this.newCalls.Add(cInfo);
        }

        internal object GetNewCallsLock()
        {
            return newCalls.SyncRoot;
        }

        /// <summary>
        /// It is assumed that the caller has requested the new calls lock already
        /// </summary>
        internal ArrayList GetNewCalls()
        {
            return this.newCalls;
        }

        internal CallInfo GetMediaCallInfo(string routingGuid, uint connectionId)
        {
            if(routingGuid != null)
                return callTable.GetMediaCallInfo(routingGuid, connectionId);
            else
                return callTable.GetMediaCallInfo(connectionId);
        }

        internal CallInfo GetCallInfo(string routingGuid, long callId)
        {
            if(routingGuid != null)
                return callTable.GetCallInfo(routingGuid, callId);
            else
                return callTable.GetCallInfo(callId);
        }

        internal StateMap GetStateMap(string mapName)
        {
            return this.stateMaps[mapName] as StateMap;
        }

        /// <summary>Changes the active state map for a call</summary>
        /// <remarks>Used from both the scheduler and message queueing threads</remarks>
        internal bool ChangeStateMap(CallInfo cInfo, string mapName)
        {
            Assertion.Check(cInfo != null, "cInfo is null in ChangeStateMap");

            StateMap newMap = stateMaps[mapName] as StateMap;
            if(newMap == null) { return false; }

            lock(cInfo.StateLock)
            {
                cInfo.CurrStateMap = newMap;
                cInfo.MapName = mapName;
                cInfo.CurrStateId = newMap.FirstStateId;
				cInfo.StateHistory.Push((uint)0);

				log.Write(TraceLevel.Info, "Call '{0}' switching to state map: {1}", cInfo.CallId, mapName);
            }

			callTable.SetActive(cInfo);
            return true;
        }

        /// <summary>Indirectly removes call from activeCalls list</summary>
        /// <remarks>Used from both the scheduler and message queueing threads</remarks>
        internal void TerminateCall(CallInfo cInfo, bool fatalError)
        {
            TerminateCall(cInfo, fatalError, false);
        }

        #endregion
    }
}
