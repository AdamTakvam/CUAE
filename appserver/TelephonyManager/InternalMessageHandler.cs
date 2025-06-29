using System;
using System.Net;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Utilities;
using Metreos.Messaging;
using Metreos.LoggingFramework;
using Metreos.Core.ConfigData;
using Metreos.Core.ConfigData.CallRouteGroups;

using Metreos.Configuration;

namespace Metreos.AppServer.TelephonyManager
{
    internal delegate void CallDelegate(CallInfo cInfo);
    internal delegate object LockDelegate();
    internal delegate CallInfo CallQueryDelegate(string routingGuid, long callId);
    internal delegate CallInfo MediaCallQueryDelegate(string routingGuid, uint connectionId);
    internal delegate bool StateMapDelegate(CallInfo cInfo, string mapName);
    internal delegate void TerminateCallDelegate(CallInfo cInfo, bool fatalError);
    internal delegate StateMap StateMapQueryDelegate(string mapName);
    internal delegate ArrayList GetArrayDelegate();

	/// <summary>
	/// High-capacity handler for action/event/response messages
	/// </summary>
	public class InternalMessageHandler
	{
        #region Constants

        public abstract class Consts
        {
            public const int NumThreads     = 3;
            public const int MaxThreads     = 10;
            public const string PoolName    = "TelephonyManager ThreadPool";
        }
        #endregion

        #region Delegates

        internal CallDelegate AddNewCall;
        internal LockDelegate GetNewCallsLock;
        internal GetArrayDelegate GetNewCalls;
        internal CallQueryDelegate GetCallInfo;
        internal MediaCallQueryDelegate GetMediaCallInfo;
        internal StateMapDelegate ChangeStateMap;
        internal TerminateCallDelegate TerminateCall;
        internal StateMapQueryDelegate GetStateMap;

        #endregion

        // References to Component-Global objects
        private readonly LogWriter log;
        private readonly MessageGenerator msgGen;
        private readonly ActionHandler actionHandler;
        private readonly CallIdFactory callIdFactory;
        private readonly IConfigUtility configUtility;

        /// <summary>Event name -> Map name</summary>
        private readonly StringDictionary triggers;

        /// <summary>CC protocol (IConfig.ComponentType) -> provider namespace (string)</summary>
        private readonly Hashtable namespaces;

        /// <summary>Call route group cache</summary>
        internal readonly CrgCache crgCache;

        private readonly ArrayList callsBeingServiced;

		internal InternalMessageHandler(MessageGenerator msgGen, ActionHandler actionHandler,
            IConfigUtility config, LogWriter log)
		{
            Assertion.Check(msgGen != null, "TelephonyManager: State engine msgGen is null");
            Assertion.Check(actionHandler != null, "TelephonyManager: State engine actionHandler is null");
            Assertion.Check(config != null, "TelephonyManager: State engine configUtility is null");
            Assertion.Check(log != null, "TelephonyManager: State engine log is null");

            this.msgGen = msgGen;
            this.actionHandler = actionHandler;
            this.configUtility = config;
            this.log = log;

            this.callIdFactory = CallIdFactory.Instance;
            this.triggers = new StringDictionary();
            this.namespaces = new Hashtable();
            this.crgCache = new CrgCache((Config)configUtility, log);

            this.callsBeingServiced = ArrayList.Synchronized(new ArrayList());
		}

        #region Startup Methods

        /// <summary>Only called during initial startup</summary>
        public void AddTrigger(string eventName, string scriptName)
        {
            if(triggers.ContainsKey(eventName))
                throw new StartupFailedException("Two handlers loaded for trigger: " + eventName);

            triggers.Add(eventName, scriptName);
        }

        /// <summary>Only called during initial startup</summary>
        public void RegisterNamespace(IConfig.ComponentType protocol, string pNamespace)
        {
            if(protocol == IConfig.ComponentType.Unspecified || pNamespace == null)
                return;

            string exNamespace = namespaces[protocol] as string;
            if(exNamespace != null)
            {
                log.Write(TraceLevel.Warning, "A provider with namespace '{0}' has already registered to handle protocol: {1}",
                    exNamespace, protocol.ToString());
            }
            else
                namespaces[protocol] = pNamespace;
        }

        public void UnregisterNamespace(StringCollection nss)
        {
            if(nss == null)
                return;

            ArrayList deadProtocols = new ArrayList();

            foreach(DictionaryEntry de in namespaces)
            {
                IConfig.ComponentType protocol = (IConfig.ComponentType) de.Key;
                string ns = de.Value as string;

                // I'm not sure why the namespaces PM maintains are always lower case  :S
                if(nss.Contains(ns.ToLower()))
                {
                    log.Write(TraceLevel.Info, "Unregistering '{0}' namespace: {1}", protocol.ToString(), ns);
                    deadProtocols.Add(protocol);
                }
            }

            foreach(IConfig.ComponentType protocol in deadProtocols)
            {
                namespaces.Remove(protocol);
            }
        }

        #endregion

        #region Message Queueing
        public void EnqueueMessage(InternalMessage msg)
        {
            Assertion.Check(AddNewCall != null, "TelephonyManager: State engine not started");
            Assertion.Check(GetNewCallsLock != null, "TelephonyManager: State engine not started");
            Assertion.Check(GetNewCalls != null, "TelephonyManager: State engine not started");
            Assertion.Check(GetCallInfo != null, "TelephonyManager: State engine not started");
            Assertion.Check(ChangeStateMap != null, "TelephonyManager: State engine not started");
            Assertion.Check(TerminateCall != null, "TelephonyManager: State engine not started");
            Assertion.Check(GetStateMap != null, "TelephonyManager: State engine not started");

            if(msg.MessageId == IActions.NoHandler)
            {
                HandleNoHandler(msg);
                return;
            }
            else if(msg.RoutingGuid == null)
            {
                log.Write(TraceLevel.Error, "Received message with no routing GUID:\n" + msg);
                return;
            }

            if(MessageGenerator.IsDummyResponse(msg as ResponseMessage))
            {
                // This is in response to an action we created in a failure/shutdown scenario, 
                //   so just drop it
                return;
            }

            // Handle the case where this message is a 
            //  response to an action performed on a peer call
            if(EnqueuePeerResponse(msg))
                return;

            // Try to get call info object
            CallInfo cInfo = null;
            long callId = 0;
            object callIdObj = msg[ICallControl.Fields.CALL_ID];
            if(callIdObj != null)
            {
                try { callId = Convert.ToInt64(callIdObj); }
                catch {}

                if(callId == 0)
                {
                    log.Write(TraceLevel.Error, "Invalid call ID in message: '{0}'\n{1}", 
                        callIdObj != null ? callIdObj.ToString() : "(null)", msg);
                    if(msg is ActionMessage)
                        ((ActionMessage)msg).SendResponse(IApp.VALUE_FAILURE);
                    return;
                }

                cInfo = GetCallInfo(msg.RoutingGuid, callId);

                if(cInfo == null)
                {
                    // Check to see if an in-call action or event was received 
                    //   before its call object has finished being initialized
                    cInfo = FindNewCall(msg.RoutingGuid, callId);
                    if(cInfo != null)
                    {
                        log.Write(TraceLevel.Info, "Handling {0}: {1}", msg is ActionMessage ? "action" : "event", msg.MessageId);
                        EnqueueNonTriggeringMessage(cInfo, msg);
                        return;
                    }
                }
            }
            else if(msg is EventMessage)   // no call ID
            {
                // Is it a media event?
                uint connectionId = Convert.ToUInt32(msg[IMediaControl.Fields.CONNECTION_ID]);
                if(connectionId != 0)
                    cInfo = GetMediaCallInfo(msg.RoutingGuid, connectionId);
            }

			if(cInfo == null)
			{
				string mapName = this.triggers[msg.MessageId];
				if(mapName != null)
				{
					if(msg is ActionMessage)
					{
                        if (msg.MessageId == ICallControl.Actions.MAKE_CALL)
                        {
                            // Catch invalid call attempt early
                            string to = Convert.ToString(msg[ICallControl.Fields.TO]);
                            if(to == null || to == String.Empty)
                            {
                                log.Write(TraceLevel.Error, "Failed to initiate outbound call with no destination specified:\n" + msg);
                                ((ActionMessage) msg).SendResponse(IApp.VALUE_FAILURE);
                            }
                            else
                            {
                                cInfo = CreateNewCall(msg as ActionMessage);
                                if(cInfo != null)
                                {
                                    log.Write(TraceLevel.Info, "Initiating new outbound call ({0}) from '{1}' to '{2}'",
                                        cInfo.CallId, cInfo.LocalParty, cInfo.RemoteParty);
                                    AddNewCall(cInfo);
                                }
                                else
                                {
                                    log.Write(TraceLevel.Error, "Failed to initiate outbound call:\n" + msg);
                                    ((ActionMessage) msg).SendResponse(IApp.VALUE_FAILURE);
                                }
                            }
                        }
                        else if(msg.MessageId == ICallControl.Actions.BARGE)
                        {
                            cInfo = CreateNewCall(msg as ActionMessage);
                            if(cInfo != null)
                            {
                                log.Write(TraceLevel.Info, "Initiating new barge call ({0}) on line: {1}",
                                    cInfo.CallId, cInfo.LocalParty);
                                AddNewCall(cInfo);
                            }
                            else
                            {
                                log.Write(TraceLevel.Error, "Failed to initiate barge call");
                                ((ActionMessage)msg).SendResponse(IApp.VALUE_FAILURE);
                            }
                        }
                        else if(msg.MessageId == ICallControl.Actions.HANGUP)
                        {
                            ((ActionMessage)msg).SendResponse(IApp.VALUE_SUCCESS);
                        }
                        else
                        {
                            log.Write(TraceLevel.Warning, "Received call service action for defunct call ({0}):\n{1}", 
                                callId, (log.LogLevel == TraceLevel.Verbose ? msg.ToString() : msg.MessageId));
                            ((ActionMessage)msg).SendResponse(IApp.VALUE_FAILURE);
                        }
					}
					else if(msg is EventMessage)
					{
						if(msg.MessageId == ICallControl.Events.INCOMING_CALL)
						{
							cInfo = CreateNewCall(msg as EventMessage);
							if(cInfo != null)
							{
								log.Write(TraceLevel.Info, "Processing new inbound call ({0}) from '{1}' to '{2}'",
									cInfo.CallId, cInfo.RemoteParty, cInfo.LocalParty);
								AddNewCall(cInfo);
							}
							else
							{
								log.Write(TraceLevel.Error, "Failed to handle new inbound call:\n" + msg);
								SendNoHandler((EventMessage)msg, false);
							}
						}
						else
						{
                            if(msg.MessageId != ICallControl.Events.REMOTE_HANGUP)
                            {
                                log.Write(TraceLevel.Warning, "Received call service event for defunct call ({0}):\n{1}",
                                    callId, msg);
                            }

                            SendNoHandler((EventMessage)msg, false);
						}
					}
				}
				else if(msg is ResponseMessage)
				{
					log.Write(TraceLevel.Info, "Ignoring orphan {0} response for call '{1}' from: {2}", 
						msg.MessageId, callId, msg.Source);
				}
				else if(msg.MessageId != IActions.NoHandler) 
				{
					// Log this and send error if it's not a response or NoHandler
					log.Write(TraceLevel.Warning, "Received message for defunct call: {0}", 
						log.LogLevel == TraceLevel.Verbose ? "\n" + msg : msg.MessageId);
					if(msg is ActionMessage)
						((ActionMessage)msg).SendResponse(IApp.VALUE_FAILURE);
					else if(msg is EventMessage)
						SendNoHandler((EventMessage)msg, false);
				}
			}
			else 
			{
				lock(cInfo.StateLock)
				{
					if(cInfo.State == CallState.Idle)  // The call is active, but no maps are running
					{
						string mapName = this.triggers[msg.MessageId];
                        if(mapName != null)
                        {
                            if(msg is ActionMessage)
                            {
                                cInfo.EnqueueMessage(msg, true);
                                if(ChangeStateMap(cInfo, mapName) == false)
                                {
                                    log.Write(TraceLevel.Error, "Failed to locate script for {0} action (callId={1})", 
                                        msg.MessageId, callId);
                                    ((ActionMessage)msg).SendResponse(IApp.VALUE_FAILURE);
                                }
                                else
                                    log.Write(TraceLevel.Info, "Handling action ({0}): {1}", cInfo.CallId, msg.MessageId);

                                // Support Unbridge creating a peer relationship
                                if(msg.MessageId == ICallControl.Actions.UNBRIDGE_CALLS)
                                {
                                    if(CreatePeerRelationship(cInfo) == false)
                                    {
                                        log.Write(TraceLevel.Error, "Failed to create call-peer relationship (callId={0}, peerCallId={1})",
                                            callId, cInfo.PeerCallId);
                                        ((ActionMessage)msg).SendResponse(IApp.VALUE_FAILURE);
                                    }
                                }
                            }
                            else if(msg is EventMessage)
                            {
                                cInfo.EnqueueMessage(msg, true);
                                if(ChangeStateMap(cInfo, mapName) == false)
                                {
                                    log.Write(TraceLevel.Error, "Failed to locate script for {0} event (callId={1})", 
                                        msg.MessageId, callId);
                                    SendNoHandler((EventMessage)msg, true);
                                }
                                else
                                    log.Write(TraceLevel.Info, "Handling event ({0}): {1}", callId, msg.MessageId);
                            }
                        }
                        else if(msg.MessageId == ICallControl.Events.GOT_DIGITS ||
                            msg.MessageId == IMediaControl.Events.GotMediaDigits)
                        {
                            HandleGotDigits(cInfo, msg as EventMessage);
                        }
                        else if(msg.MessageId == ICallControl.Events.PROV_HAIRPIN)
                        {
                            HandleProviderHairpin(cInfo, msg as EventMessage);
                        }
                        else
                        {
                            if(msg is ActionMessage)
                            {
                                log.Write(TraceLevel.Warning, "No context in which to handle action ({0}): {1}",
                                    callId, msg.MessageId);
                                ((ActionMessage)msg).SendResponse(IApp.VALUE_FAILURE);
                            }
                            else if(msg is EventMessage)
                            {
                                log.Write(TraceLevel.Warning, "No context in which to handle event ({0}): {1}",
                                    callId, msg.MessageId);
                                SendNoHandler((EventMessage)msg, true);
                            }
                            else
                                log.Write(TraceLevel.Info, "Ignoring orphan {0} response for call '{1}' from: {2}", 
                                    msg.MessageId, callId, msg.Source);
                        }
					}
					else  // A state map is currently running
					{
                        if(msg is ActionMessage)
                        {
                            log.Write(TraceLevel.Info, "Enqueuing action ({0}): {1}", callId, msg.MessageId);
                            EnqueueNonTriggeringMessage(cInfo, msg);
                        }
                        else if(msg is EventMessage)
                        {
                            log.Write(TraceLevel.Info, "Enqueuing event ({0}): {1}", callId, msg.MessageId);
                            EnqueueNonTriggeringMessage(cInfo, msg);
                        }
                        else if(msg is ResponseMessage)
                        {
                            ResponseMessage rMsg = msg as ResponseMessage;
                            if(rMsg.MessageId == IApp.VALUE_SUCCESS)
                                log.Write(TraceLevel.Verbose, "Enqueuing '{0}' ({1}) response from {2}", msg.MessageId, callId, msg.Source);
                            else if(rMsg.InResponseTo != IMediaControl.Actions.STOP_MEDIA)
                                log.Write(TraceLevel.Warning, "Enqueuing '{0}' ({1}) response from {2}", msg.MessageId, callId, msg.Source);

                            EnqueueNonTriggeringMessage(cInfo, msg);
                        }
                        else
                        {
                            log.Write(TraceLevel.Error, "Received unknown message type:\n" + msg);
                        }
					} 
				}
			}
        }

        private void EnqueueNonTriggeringMessage(CallInfo cInfo, InternalMessage msg)
        {
            if(msg is EventMessage && msg.MessageId == ICallControl.Events.GOT_DIGITS)
            {
                HandleGotDigits(cInfo, msg as EventMessage);
                return;
            }

            ActionMessage aMsg = msg as ActionMessage;
            if(aMsg != null)
            {
                if(cInfo.ProviderNamespace == null)
                {
                    cInfo.ProviderNamespace = DetermineProviderNamespace(aMsg.AppName, aMsg.PartitionName);
                    
                    if(cInfo.ProviderNamespace == null)
                    {
                        if(cInfo.CallIdSpecified)
                        {
                            log.Write(TraceLevel.Warning, "Rejecting call '{0}' due to configuration problem (cannot determine call control provider)", cInfo.CallId);
                            actionHandler.RejectCall(cInfo, Namespace.GetNamespace(msg.MessageId));
                        }

                        aMsg.SendResponse(IApp.VALUE_FAILURE);
                        TerminateCall(cInfo, true);
                        return;
                    }
                }

                if(msg.MessageId == ICallControl.Actions.ANSWER_CALL)
                {
                    CreateDtmfProxyRelationship(cInfo, msg);
                }
            }

            cInfo.EnqueueMessage(msg);
        }

        private bool EnqueuePeerResponse(InternalMessage msg)
        {
            ResponseMessage rMsg = msg as ResponseMessage;
            if(rMsg == null)
                return false;

            CallInfo cInfo = null;
            try
            {
                long callId = Convert.ToInt64(rMsg[ICallControl.Fields.PEER_CALL_ID]);

                if(callId == 0)
                    return false;

                // Note: Could be optimized if peer routing GUID were preserved.
                cInfo = GetCallInfo(null, callId);  
            }
            catch { return false; }

            if(cInfo != null && cInfo.State != CallState.Idle)
            {
                log.Write(TraceLevel.Verbose, "Enqueuing peer response ({0}): {1}", cInfo.CallId, msg.MessageId);
                cInfo.EnqueueMessage(msg);
                return true;
            }
            return false;
        }
        #endregion

        #region Helpers

        private void HandleNoHandler(InternalMessage msg)
        {
            EventMessage originalEvent = msg[IActions.Fields.InnerMsg] as EventMessage;
            if(originalEvent == null)
            {
                log.Write(TraceLevel.Warning, "Received 'NoHandler' notification with no associated event");
                return;
            }

            if(originalEvent.MessageId == ICallControl.Events.INCOMING_CALL)
            {
				long callId = Convert.ToInt32(originalEvent[ICallControl.Fields.CALL_ID]);
				if(callId == 0)
				{
					log.Write(TraceLevel.Warning, "Received a 'NoHandler' notification for an event with no or invalid call ID:\n" + originalEvent);
					return;
				}

                CallInfo cInfo = GetCallInfo(originalEvent.RoutingGuid, callId);
                if(cInfo == null) { return; }

                log.Write(TraceLevel.Info, "No handler registered for incoming call {0}. Terminating.", cInfo.CallId);
                this.TerminateCall(cInfo, true);
            }
        }

        private CallInfo CreateNewCall(EventMessage msg)
        {
            string mapName = triggers[msg.MessageId];
            Assertion.Check(mapName != null, "Event handling mechanism is all jacked up. Can't handle:\n" + msg);
            Assertion.Check(GetStateMap(mapName) != null, "Event handling mechanism is all jacked up. No map for: " + mapName);

            CallInfo cInfo = new CallInfo(GetStateMap(mapName), mapName, log, msg);

            cInfo.ProviderNamespace = msg[ICallControl.Fields.SOURCE_NS] as string;
            Assertion.Check(cInfo.ProviderNamespace != null, "Provider sent triggering event without a source namespace specified");

			cInfo.CallId = Convert.ToInt64(msg[ICallControl.Fields.CALL_ID]);
			if(cInfo.CallId == 0)
			{
				log.Write(TraceLevel.Error, "Received message with no or invalid call ID:\n" + msg);
				return null;
			}

            log.Write(TraceLevel.Info, "Loading {0} state map to handle {1} event",
                mapName, msg.MessageId);

            return cInfo;
        }

        private CallInfo CreateNewCall(ActionMessage msg)
        {
            string mapName = triggers[msg.MessageId];
            Assertion.Check(mapName != null, "Action handling mechanism is all jacked up. Can't handle:\n" + msg);
            Assertion.Check(GetStateMap(mapName) != null, "Action handling mechanism is all jacked up. No map for: " + mapName);

            CallInfo cInfo = new CallInfo(GetStateMap(mapName), mapName, log, msg);
    
            if(msg.Contains(ICallControl.Fields.CALL_ID) == false)
                cInfo.CallId = callIdFactory.GenerateCallId();
            else
                cInfo.CallId = Convert.ToInt64(msg[ICallControl.Fields.CALL_ID]);

            CreateDtmfProxyRelationship(cInfo, msg);

            log.Write(TraceLevel.Info, "Loading {0} state map to handle {1} action",
                mapName, msg.MessageId);

            cInfo.ProviderNamespace = DetermineProviderNamespace(msg.AppName, msg.PartitionName);
            return cInfo.ProviderNamespace != null ? cInfo : null;
        }

        private void CreateDtmfProxyRelationship(CallInfo cInfo, InternalMessage msg)
        {
            Assertion.Check(cInfo.CallId != 0, "Call ID is zero while creating DTMF proxy relationship");

            if(msg.Contains(ICallControl.Fields.DTMF_CALL_ID))
            {
                cInfo.DtmfCallId = Convert.ToInt64(msg[ICallControl.Fields.DTMF_CALL_ID]);
                
                CallInfo proxyInfo = GetCallInfo(null, cInfo.DtmfCallId);
                if(proxyInfo != null)
                {
                    proxyInfo.DtmfCallId = cInfo.CallId;
                }
                else
                {
                    log.Write(TraceLevel.Warning, "Invalid DTMF proxy call ID '{0}' in {1} action",
                        cInfo.DtmfCallId, msg.MessageId);
                    cInfo.DtmfCallId = 0;
                }
            }
        }

        private CallInfo FindNewCall(string routingGuid, long callId)
        {
            lock(GetNewCallsLock())
            {
                foreach(CallInfo cInfo in GetNewCalls())
                {
                    if(cInfo.RoutingGuid == routingGuid || cInfo.CallId == callId)
                    {
                        return cInfo;
                    }
                }
            }
            return null;
        }

        private void HandleGotDigits(CallInfo cInfo, EventMessage msg)
        {
            string digits = msg[ICallControl.Fields.DIGITS] as string;
            if(digits == null)
            {
                log.Write(TraceLevel.Warning, "Received GotDigits event with no digits");
                return;
            }

            if(cInfo.ConnectionId > 0)
            {
                // Proxy digits to media server for accumulation
                ActionMessage aMsg = msgGen.CreateSendDigitsAction(cInfo, digits, true);
                actionHandler.RouterQ.PostMessage(aMsg);
            }

            // Proxy digits to another call leg, if configured
            CallInfo destCallInfo = null;
            if(cInfo.DtmfCallId != 0)
            {
                destCallInfo = GetCallInfo(null, cInfo.DtmfCallId);
            }
            else if(cInfo.PeerCallId != 0)
            {
                destCallInfo = GetCallInfo(cInfo.RoutingGuid, cInfo.PeerCallId);
            }

            if(destCallInfo != null)
            {
                log.Write(TraceLevel.Verbose, "Proxying digit(s) '{0}' from call '{1}' to '{2}'",
                    digits, cInfo.CallId, cInfo.DtmfCallId);

                ActionMessage aMsg = msgGen.CreateSendUserInput(destCallInfo, digits);
                actionHandler.RouterQ.PostMessage(aMsg);
            }

            // Send event to app, in case there's something more they wish to do
            EventMessage eMsg = msgGen.CreateGotDigitsEvent(cInfo, digits);
            actionHandler.RouterQ.PostMessage(eMsg);
        }

        private void HandleProviderHairpin(CallInfo cInfo, EventMessage msg)
        {
            ActionMessage action = msg[ICallControl.Fields.HAIRPIN_ACTION] as ActionMessage;
            if(action == null)
            {
                log.Write(TraceLevel.Warning, "Received Provider Hairpin event with no action to hairpin");
                return;
            }

            // Proxy digits to another call leg, if configured
            if(cInfo.PeerCallId == 0)
            {
                log.Write(TraceLevel.Info, "Cannot hairpin action '{0}' because call '{1}' has no peer",
                    action.MessageId, cInfo.CallId);
                return;
            }

            CallInfo destCallInfo = GetCallInfo(cInfo.RoutingGuid, cInfo.PeerCallId);

            if(destCallInfo == null)
            {
                log.Write(TraceLevel.Warning, "Cannot hairpin action '{0}' because the peer for call '{1}' has ended",
                    action.MessageId, cInfo.CallId);
                return;
            }

            action.AddField(ICallControl.Fields.CALL_ID, destCallInfo.CallId);

            log.Write(TraceLevel.Verbose, "Hairpinning action '{0}' from call '{1}' to '{2}'",
                action.MessageId, cInfo.CallId, cInfo.PeerCallId);

            actionHandler.RouterQ.PostMessage(action);
        }

        private bool CreatePeerRelationship(CallInfo cInfo)
        {
            if(cInfo.PeerCallId != 0)
            {
                CallInfo peerInfo = GetCallInfo(null, cInfo.PeerCallId);
                if(peerInfo == null)
                    return false;
            
                peerInfo.PeerCallId = cInfo.CallId;
            }
            return true;
        }

        private void SendNoHandler(EventMessage msg, bool sessionActive)
        {
            if(msg.SourceQueue != null && !msg.SuppressNoHandler)
            {
                ActionMessage noHandler = msgGen.CreateNoHandler(msg, sessionActive);
                msg.SourceQueue.PostMessage(noHandler);
            }
        }

        private string DetermineProviderNamespace(string appName, string partName)
        {
            IConfig.ComponentType groupType = this.crgCache.GetCrgType(appName, partName);

            if(groupType == IConfig.ComponentType.Unspecified)
            {
                log.Write(TraceLevel.Warning, "Configuration error: Call route group is empty or misconfigured for: {0}->{1}",
                    appName, partName);
                return null;
            }

            string provNamespace = namespaces[groupType] as string;
                    
            if(provNamespace == null)
            {
                log.Write(TraceLevel.Warning, "Application {0}:{1} cannot use CallControl services, no provider is installed of type: {2}",
                    appName, partName, groupType.ToString());
            }

            return provNamespace;
        }
        #endregion
	}
}
