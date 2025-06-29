using System;
using System.Net;
using System.Diagnostics;
using System.Collections;

using Metreos.Interfaces;
using Metreos.Utilities;
using Metreos.Messaging;
using Metreos.Messaging.MediaCaps;
using Metreos.LoggingFramework;

namespace Metreos.AppServer.TelephonyManager
{
    [Flags]
    public enum CallState : ushort
    {
        Idle            = 0x00,
        WaitForEvent    = 0x01,
        WaitForAction   = 0x02,
        WaitForResponse = 0x04,
        CheckForData    = 0x08,
        WaitForTimeout  = 0x10,
        HasDefault      = 0x20,
		Error           = 0x40
    }

    internal sealed class CallInfo : IDisposable
    {
        #region The Unholy Legion of Properties

        private string appName = null;
        public string AppName
        {
            get { return appName; }
            set { appName = value; }
        }

        private string partitionName = null;
        public string PartitionName
        {
            get { return partitionName; }
            set { partitionName = value; }
        }

        private string routingGuid = null;
        public string RoutingGuid
        {
            get { return routingGuid; }
            set { routingGuid = value; }
        }

        private bool callIdSpecified = false;
        public bool CallIdSpecified { get {return callIdSpecified; } }

        private long callId;
        public long CallId
        {
            get { return callId; }
            set 
            {
                callIdSpecified = true;
                callId = value; 
            }
        }

        private bool callEstablished = false;
        public bool CallEstablished { get { return callEstablished; } }
        
        private bool callTerminated = false;
        public bool CallTerminated 
        { 
            get { return callTerminated; } 
            set { callTerminated = value; }
        }

        private string remoteParty = null;
		public string RemoteParty { get { return remoteParty; } }

		private string localParty = null;
		public string LocalParty { get { return localParty; } }

		private bool negCaps = false;
		public bool NegCaps { get { return negCaps; } }

        private long peerCallId = 0;
        public long PeerCallId 
        { 
            get { return peerCallId; } 
            set { peerCallId = value; }
        }

        private bool waitingForPeerResponse = false;
        public bool WaitingForPeerResponse
        {
            get { return waitingForPeerResponse; }
            set { waitingForPeerResponse = value; }
        }

        private long dtmfCallId = 0;
        public long DtmfCallId 
        { 
            get { return dtmfCallId; } 
            set { dtmfCallId = value; }
        }

        private uint mmsId = 0;
        public uint MmsId { get { return mmsId; } }

        private uint connectionId = 0;
        public uint ConnectionId 
        { 
            get { return connectionId; } 
            set { connectionId = value; }
        }

        private bool conference = false;
        public bool Conference { get { return conference; } }

        private uint conferenceId = 0;
        public uint ConferenceId 
        { 
            get { return conferenceId; } 
            set { conferenceId = value; }
        }

		private bool hairpin = IMediaControl.Fields.DefaultValues.HAIRPIN;
		public bool Hairpin { get { return hairpin; } }

        private string providerNamespace = null;
        public string ProviderNamespace
        {
            get { return providerNamespace; }
            set { providerNamespace = value; }
        }

        private MediaCapsField localMediaCaps = null;
        public MediaCapsField LocalMediaCaps
        {
            get { return localMediaCaps; }
            set { localMediaCaps = value; }
        }

        private MediaCapsField remoteMediaCaps = null;
        public MediaCapsField RemoteMediaCaps { get { return remoteMediaCaps; } }

        private IPEndPoint txAddr = null;
        public IPEndPoint TxAddr { get { return txAddr; } }

        private IPEndPoint txControlAddr = null;
        public IPEndPoint TxControlAddr { get { return GetTxControlAddr(); } }

        private IMediaControl.Codecs txCodec = IMediaControl.Codecs.Unspecified;
        public IMediaControl.Codecs TxCodec
        {
            get { return txCodec; }
            set { txCodec = value; }
        }

        private uint txFramesize = 0;
        public uint TxFramesize
        {
            get { return txFramesize; }
            set { txFramesize = value; }
        }

        private IPEndPoint rxAddr = null;
        public IPEndPoint RxAddr 
        { 
            get { return rxAddr; } 
            set { rxAddr = value; }
        }

        private IPEndPoint rxControlAddr = null;
        public IPEndPoint RxControlAddr
        {
            get { return GetRxControlAddr(); }
            set { rxControlAddr = value; }
        }

        private IMediaControl.Codecs rxCodec = IMediaControl.Codecs.Unspecified;
        public IMediaControl.Codecs RxCodec 
        { 
            get { return rxCodec; } 
            set { rxCodec = value; }
        }

        private uint rxFramesize = 0;
        public uint RxFramesize 
        { 
            get { return rxFramesize; } 
            set { rxFramesize = value; }
        }

        private WaitMedia waitForMedia = WaitMedia.TxRx;
        public WaitMedia WaitForMedia { get { return waitForMedia; } }

        private object userData;
        public object UserData { get { return userData; } }

        private string endReason = ICallControl.EndReason.Normal.ToString();
        public string EndReason 
        { 
            get { return endReason; } 
            set { endReason = value; }
        }

        public CallState State
        {
            get
            {
				StateDescription desc;
				lock(stateLock)
				{
					desc = CurrentState;
					if(desc == null)
					{
						log.Write(TraceLevel.Error, "Internal Error, Current state is not valid. ({0}:{1})",
							mapName, CurrStateId);
						return CallState.Error;
					}
				}

                if (desc.action == Actions.EndScript ||
                    desc.action == Actions.EndCall)
                    return CallState.Idle;

				CallState currState = 0;
				if(desc.actionNextStates != null)
                    currState |= CallState.WaitForAction;
                if(desc.eventNextStates != null)
                    currState |= CallState.WaitForEvent;
                if(desc.responseNextStates != null)
                    currState |= CallState.WaitForResponse;
                if(desc.dataNextStates != null)
                    currState |= CallState.CheckForData;
                if(desc.TimeoutNextState != null)
                    currState |= CallState.WaitForTimeout;
                if(desc.defaultNextState != null)
                    currState |= CallState.HasDefault;
                if(desc.action == Actions.EndScript || desc.action == Actions.EndCall)
                    currState |= CallState.HasDefault;
                return currState;
            }
        }

        private long created;
        public long Created { get { return created; } }

        private long currStateTimeout;
        public long CurrStateTimeout { get { return currStateTimeout; } }

        public long CurrStateExecTime 
        { 
            set 
            { 
                currStateTimeout = HPTimer.AddTime(value, CurrentState.Timeout.Minutes, 
                    CurrentState.Timeout.Seconds, CurrentState.Timeout.Milliseconds);  
            } 
        }

        private object stateLock = new object();
        public object StateLock { get { return stateLock; } }

        private StateMap stateMap = null;
        public StateMap CurrStateMap 
        { 
            get { return stateMap; }
            set { stateMap = value; } 
        }

        private volatile uint currStateId;
        public uint CurrStateId 
        { 
            get { return currStateId; } 
            set { currStateId = value; }
        }

		public StateDescription CurrentState
		{
			get { lock(this.StateLock) { return stateMap[currStateId]; } }
		}

        private string mapName;  // For logging purposes only
        public string MapName 
        { 
            get { return mapName; } 
            set { mapName = value; }
        }

        private bool fatalError = false;
        public bool FatalError
        {
            get { return fatalError; }
            set { fatalError = value; }
        }

        private Stack stateHistory;         // Executed state IDs
        public Stack StateHistory { get { return stateHistory; } }

        public bool Running 
        { 
            get 
            { 
                if(stateHistory.Count == 0) { return true; }
                uint lastStateId = Convert.ToUInt32(stateHistory.Peek());
                if(lastStateId != currStateId) { return true; }
                return false;
            } 
        }

        public ResponseMessage LastHandledResponse 
        { 
            get { return handledResponses.Count > 0 ? handledResponses.Peek() as ResponseMessage : null; } 
        }

        public ActionMessage LastHandledAction 
        { 
            get { return handledActions.Count > 0 ? handledActions.Peek() as ActionMessage : null; } 
        }

        public EventMessage LastHandledEvent
        { 
            get { return handledEvents.Count > 0 ? handledEvents.Peek() as EventMessage : null; } 
        }

        #endregion

        #region Private Members/Constructor/Dispose

        private LogWriter log;

        private Queue incomingResponses;
        private ArrayList incomingActions;   
        private ArrayList incomingEvents;
 
        // Note: It doesn't seem really necessary to keep old handled stuff around, so
        //   we may want to convert these over to single members (e.g. lastHandledResponse)
        //   at some point in the future to save a bit of memory.
        private Stack handledResponses;
        private Stack handledActions;
        private Stack handledEvents;

		public CallInfo(StateMap map, string mapName, LogWriter log, InternalMessage trigMsg)
		{
            this.log = log;
            this.currStateId = map.FirstStateId;
            this.stateMap = map;
            this.mapName = mapName;

            this.incomingResponses = Queue.Synchronized(new Queue());
            this.incomingActions = ArrayList.Synchronized(new ArrayList());   
            this.incomingEvents = ArrayList.Synchronized(new ArrayList());

            this.handledResponses = Stack.Synchronized(new Stack());
            this.handledActions = Stack.Synchronized(new Stack());
            this.handledEvents = Stack.Synchronized(new Stack());

            this.stateHistory = Stack.Synchronized(new Stack());
            this.created = HPTimer.Now();

            EnqueueMessage(trigMsg, true);
		}

        public void Dispose()
        {
            if(stateHistory.Count > 0 && log.LogLevel == TraceLevel.Verbose)
            {
                lock(stateHistory.SyncRoot)
                {
                    string statePath = stateHistory.Pop().ToString();
                    foreach(uint stateId in stateHistory)
                    {
                        statePath = stateId + ", " + statePath;
                    }
                
                    log.Write(TraceLevel.Verbose, "Call '{0}' ended. State path = {1}", CallId, statePath);
                    stateHistory.Clear();
                }
            }
        }
        #endregion

        #region Public Utility Methods

        public void EnqueueMessage(InternalMessage msg)
        {
            EnqueueMessage(msg, false);
        }

        public void EnqueueMessage(InternalMessage msg, bool processNow)
        {
            if(msg is ResponseMessage)
            {
                if(processNow)
                    ProcessResponse(msg as ResponseMessage);
                else
                    incomingResponses.Enqueue(msg);
            }
            else if(msg is ActionMessage)
            {
                if(processNow)
                    ProcessAction(msg as ActionMessage);
                else
                    incomingActions.Add(msg);
            }
            else if(msg is EventMessage)
            {
                if(processNow)
                    ProcessEvent(msg as EventMessage);
                else
                    incomingEvents.Add(msg);
            }
        }

        public ResponseMessage GetIncomingResponse()
        {
            ResponseMessage msg = null;
            
            lock(this.incomingResponses.SyncRoot)
            {
                for(int i=0; i<incomingResponses.Count; i++)
                {
                    msg = incomingResponses.Dequeue() as ResponseMessage;
                    if(msg == null) { return null; }

                    string actionId = ActionGuid.GetActionId(msg.InResponseToActionGuid);
                    Assertion.Check(actionId != null, "Encountered incoming response with no action ID:\n" + msg);

                    if(CurrentState.action == Actions.ForwardActionToProvider)
                    {
                        ActionMessage actionMsg = handledActions.Peek() as ActionMessage;
                        if(actionMsg.ActionGuid == msg.InResponseToActionGuid)
                        {
                            ProcessResponse(msg);
                            return msg;
                        }
                    }
                    else if(actionId == currStateId.ToString() || waitingForPeerResponse)
                    {
                        waitingForPeerResponse = false;
                        ProcessResponse(msg);
                        return msg;
                    }
                    
                    log.Write(TraceLevel.Warning, "Discarding response ({0}):\n{1}", callId, msg);
                }
            }
            return null;
        } 

        public ActionMessage GetIncomingAction(out string nextStateId)
        {
            // Find the first incoming action which appears on the handler list for this state
            nextStateId = null;

            lock(this.incomingActions.SyncRoot)
            {
                foreach(ActionMessage inActionMsg in incomingActions)
                {
                    foreach(string actionName in CurrentState.actionNextStates.Keys)
                    {
                        if(inActionMsg.MessageId.ToLower().IndexOf(actionName.ToLower()) != -1)
                        {
                            nextStateId = CurrentState.actionNextStates[actionName] as String;
                            incomingActions.Remove(inActionMsg);
                            ProcessAction(inActionMsg);
                            return inActionMsg;
                        }
                    }
                }
            }
            return null;
        }

        public EventMessage GetIncomingEvent(out string nextStateId)
        {
            // Find the first incoming action which appears on the handler list for this state
            nextStateId = null;

            lock(this.incomingEvents.SyncRoot)
            {
                foreach(EventMessage inEventMsg in incomingEvents)
                {
                    foreach(string eventName in CurrentState.eventNextStates.Keys)
                    {
                        if(inEventMsg.MessageId.ToLower().IndexOf(eventName.ToLower()) != -1)
                        {
                            nextStateId = CurrentState.eventNextStates[eventName] as String;
                            incomingEvents.Remove(inEventMsg);
                            ProcessEvent(inEventMsg);
                            return inEventMsg;
                        }
                    }
                }
            }

            return null;
        }

        public ActionMessage PopIncomingAction()
        {
            if(incomingActions.Count == 0)
                return null;

            ActionMessage action = incomingActions[0] as ActionMessage;
            incomingActions.RemoveAt(0);
            return action;
        }

        public EventMessage PopIncomingEvent()
        {
            if(incomingEvents.Count == 0)
                return null;

            EventMessage ev = incomingEvents[0] as EventMessage;
            incomingEvents.RemoveAt(0);
            return ev;
        }

        public bool MatchCriteria(DataCriteria crit)
        {
            switch(crit.fieldName)
            {
                case DataFields.callId:
                    if(crit.comparator == Comparators.Equal) { return callId == Convert.ToInt64(crit.Value); }
                    else { return callId != Convert.ToInt64(crit.Value); }
                case DataFields.peerCallId:
                    if(crit.comparator == Comparators.Equal) { return peerCallId == Convert.ToInt64(crit.Value); }
                    else { return peerCallId != Convert.ToInt64(crit.Value); }
                case DataFields.mmsId:
                    if(crit.comparator == Comparators.Equal) { return mmsId == Convert.ToUInt32(crit.Value); }
                    else { return mmsId != Convert.ToUInt32(crit.Value); }
                case DataFields.connectionId:
                    if(crit.comparator == Comparators.Equal) { return connectionId == Convert.ToUInt64(crit.Value); }
                    else { return connectionId != Convert.ToUInt64(crit.Value); }
                case DataFields.conference:
                    if(crit.comparator == Comparators.Equal) { return conference == Convert.ToBoolean(crit.Value); }
                    else { return conference != Convert.ToBoolean(crit.Value); }
                case DataFields.conferenceId:
                    if(crit.comparator == Comparators.Equal) { return conferenceId == Convert.ToUInt64(crit.Value); }
                    else { return conferenceId != Convert.ToUInt64(crit.Value); }
                case DataFields.localMediaCaps:
                    if(crit.comparator == Comparators.Equal) { return localMediaCaps == crit.Value as MediaCapsField; }
                    else { return localMediaCaps != crit.Value as MediaCapsField; }
                case DataFields.remoteMediaCaps:
                    if(crit.comparator == Comparators.Equal) { return remoteMediaCaps == crit.Value as MediaCapsField; }
                    else { return remoteMediaCaps != crit.Value as MediaCapsField; }
                case DataFields.txAddr:
                    if(crit.comparator == Comparators.Equal) { return txAddr == crit.Value as IPEndPoint; }
                    else { return txAddr != crit.Value as IPEndPoint; }
                case DataFields.txCodec:
                    if(crit.comparator == Comparators.Equal) { return txCodec == IMediaControl.Codecs.Unspecified; }
                    else { return txCodec != IMediaControl.Codecs.Unspecified; }
                case DataFields.txFramesize:
                    if(crit.comparator == Comparators.Equal) { return txFramesize == Convert.ToUInt32(crit.Value); }
                    else { return txFramesize != Convert.ToUInt32(crit.Value); }
                case DataFields.rxAddr:
                    if(crit.comparator == Comparators.Equal) { return rxAddr == crit.Value as IPEndPoint; }
                    else { return rxAddr != crit.Value as IPEndPoint; }
                case DataFields.rxCodec:
                    if(crit.comparator == Comparators.Equal) { return rxCodec == IMediaControl.Codecs.Unspecified; }
                    else { return rxCodec != IMediaControl.Codecs.Unspecified; }
                case DataFields.rxFramesize:
                    if(crit.comparator == Comparators.Equal) { return rxFramesize == Convert.ToUInt32(crit.Value); }
					else { return rxFramesize != Convert.ToUInt32(crit.Value); }
				case DataFields.waitForMedia:
					if(crit.comparator == Comparators.Equal) { return waitForMedia == (WaitMedia)crit.Value; }
					else { return waitForMedia != (WaitMedia)crit.Value; }
				case DataFields.negCaps:
					if(crit.comparator == Comparators.Equal) { return negCaps == Convert.ToBoolean(crit.Value); }
					else { return negCaps != Convert.ToBoolean(crit.Value); }
            }
            return false;
        }

        public void ClearMediaInfo()
        {
            rxAddr = null;
            rxControlAddr = null;
            txAddr = null;
            txControlAddr = null;
        }

        #endregion

        #region Private Helper Methods

        private void ProcessResponse(ResponseMessage msg)
        {
            if(msg.InResponseTo != IMediaControl.Actions.DELETE_CONNECTION)
                ProcessMessage(msg);

            if(!msg.InResponseTo.EndsWith(ICallControl.Actions.Suffix.SET_MEDIA) && 
               !msg.InResponseTo.StartsWith(IMediaControl.NAMESPACE))
            {
                handledResponses.Push(msg);
            }
        }

        private void ProcessAction(ActionMessage msg)
        {
            switch(msg.MessageId)
            {
                case ICallControl.Actions.MAKE_CALL:
                    this.remoteParty = Convert.ToString(msg[ICallControl.Fields.TO]);
                    this.localParty = Convert.ToString(msg[ICallControl.Fields.FROM]);
                    this.userData = msg.UserData;
                    break;                    
                case ICallControl.Actions.HANGUP:
                    this.callTerminated = true;
                    break;
            }

            // Process action-specific info
            if(this.appName == null) { this.appName = msg.AppName; }
            if(this.partitionName == null) { this.partitionName = msg.PartitionName; }

            ProcessMessage(msg);
            handledActions.Push(msg);
        }

        private void ProcessEvent(EventMessage msg)
        {
            switch(msg.MessageId)
            {
                case ICallControl.Events.INCOMING_CALL:
                    this.localParty = Convert.ToString(msg[ICallControl.Fields.TO]);
                    this.remoteParty = Convert.ToString(msg[ICallControl.Fields.FROM]);
                    break;
                case ICallControl.Events.CALL_ESTABLISHED:
                    this.callEstablished = true;
                    break;
                case ICallControl.Events.REMOTE_HANGUP:
                    this.callTerminated = true;
                    break;
            }

            ProcessMessage(msg);
            handledEvents.Push(msg);
        }

        private void ProcessMessage(InternalMessage msg)
        {
            try
            {
                if(this.routingGuid == null) 
                    this.routingGuid = msg.RoutingGuid;
                if(msg.Contains(IMediaControl.Fields.CONNECTION_ID)) 
                    this.connectionId = Convert.ToUInt32(msg[IMediaControl.Fields.CONNECTION_ID]);
                if(msg.Contains(IMediaControl.Fields.CONFERENCE_ID))
                    this.conferenceId = Convert.ToUInt32(msg[IMediaControl.Fields.CONFERENCE_ID]);
                if(msg.Contains(IMediaControl.Fields.LOCAL_MEDIA_CAPS)) 
                    this.localMediaCaps = msg[IMediaControl.Fields.LOCAL_MEDIA_CAPS] as MediaCapsField;
                if(msg.Contains(IMediaControl.Fields.MMS_ID)) 
                    this.mmsId = Convert.ToUInt32(msg[IMediaControl.Fields.MMS_ID]);
                if(msg.Contains(ICallControl.Fields.PEER_CALL_ID) && this.peerCallId == 0) 
                    this.peerCallId = Convert.ToInt64(msg[ICallControl.Fields.PEER_CALL_ID]);
                if(msg.Contains(ICallControl.Fields.REMOTE_MEDIA_CAPS)) 
                    this.remoteMediaCaps = msg[ICallControl.Fields.REMOTE_MEDIA_CAPS] as MediaCapsField;
                if(msg.Contains(IMediaControl.Fields.RX_FRAMESIZE)) 
                    this.rxFramesize = Convert.ToUInt32(msg[IMediaControl.Fields.RX_FRAMESIZE]);
                if(msg.Contains(IMediaControl.Fields.TX_FRAMESIZE)) 
                    this.txFramesize = Convert.ToUInt32(msg[IMediaControl.Fields.TX_FRAMESIZE]);
                if(msg.Contains(ICallControl.Fields.END_REASON))
                    this.endReason = Convert.ToString(msg[ICallControl.Fields.END_REASON]);
                if(msg.Contains(ICallControl.Fields.NEG_CAPS))
                    this.negCaps = Convert.ToBoolean(msg[ICallControl.Fields.NEG_CAPS]);
                if(msg.Contains(IMediaControl.Fields.HAIRPIN))
                    this.hairpin = Convert.ToBoolean(msg[IMediaControl.Fields.HAIRPIN]);

                if(msg.Contains(IMediaControl.Fields.RX_CODEC))
                    this.rxCodec = (IMediaControl.Codecs) Convert.ToUInt32(msg[IMediaControl.Fields.RX_CODEC]);
                if(msg.Contains(IMediaControl.Fields.TX_CODEC))
                    this.txCodec = (IMediaControl.Codecs) Convert.ToUInt32(msg[IMediaControl.Fields.TX_CODEC]);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Received message with one or more invalid fields:\n" + msg);
                return;
            }

            if(msg.Contains(IMediaControl.Fields.RX_IP) && msg.Contains(IMediaControl.Fields.RX_PORT)) 
            {
                string rxIP = Convert.ToString(msg[IMediaControl.Fields.RX_IP]);
                uint rxPort = Convert.ToUInt32(msg[IMediaControl.Fields.RX_PORT]);
                
                if((rxIP != null) && (rxPort != 0))
                {
                    try { this.rxAddr = new IPEndPoint(IPAddress.Parse(rxIP), (int)rxPort); }
                    catch
                    {
                        log.Write(TraceLevel.Warning, "Received invalid local media address '{0}:{1}' in:\n{2}",
                            rxIP, rxPort.ToString(), msg.ToString());
                    }
                }
            }

            if(msg.Contains(IMediaControl.Fields.TX_IP) && msg.Contains(IMediaControl.Fields.TX_PORT)) 
            {
                string txIP = Convert.ToString(msg[IMediaControl.Fields.TX_IP]);
                uint txPort = Convert.ToUInt32(msg[IMediaControl.Fields.TX_PORT]);
                
                if((txIP != null) && (txPort != 0))
                {
                    try { this.txAddr = new IPEndPoint(IPAddress.Parse(txIP), (int)txPort); }
                    catch
                    {
                        log.Write(TraceLevel.Warning, "Received invalid remote media address '{0}:{1}' in:\n{2}",
                            txIP, txPort.ToString(), msg.ToString());
                    }
                }
                else
                {
                    this.txAddr = null;
                }
            }

            if(msg.Contains(ICallControl.Fields.CONFERENCE))
            {
                try { this.conference = Convert.ToBoolean(msg[ICallControl.Fields.CONFERENCE]); }
                catch 
                {
                    log.Write(TraceLevel.Warning, "Invalid value specified for 'Conference' field:\n" + msg);
                }
            }

            if(msg.Contains(ICallControl.Fields.WAIT_MEDIA))
            {
                try 
                {
                    this.waitForMedia = (WaitMedia) Enum.Parse(typeof(WaitMedia), 
                          Convert.ToString(msg[ICallControl.Fields.WAIT_MEDIA]), true); 
                }
                catch
                {
                    log.Write(TraceLevel.Warning, "Invalid value specified for 'WaitForMedia' field:\n" + msg);
                }
            }

			// Set MMS ID if conference or connection ID was set
			if(this.mmsId == 0)
			{
				if(this.conferenceId > 0)
					this.mmsId = IMediaControl.GetMmsId(this.conferenceId);
				else if(this.connectionId > 0)
					this.mmsId = IMediaControl.GetMmsId(this.connectionId);
			}

            // Log media stuff (don't waste cycles if not logging verbose)
            if(log.LogLevel == TraceLevel.Verbose)
            {
                if(msg.MessageId == ICallControl.Events.GOT_CAPABILITIES)
                {
                    if(remoteMediaCaps != null && remoteMediaCaps.Count > 0)
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder("GotCapabilities (");
                        sb.Append(callId);
                        sb.Append("):\r\n");

                        foreach(DictionaryEntry de in remoteMediaCaps)
                        {
                            IMediaControl.Codecs codec = IMediaControl.Codecs.Unspecified;
                            try { codec = (IMediaControl.Codecs) de.Key; }
                            catch { continue; }

                            ArrayList frames = de.Value as ArrayList;

                            sb.Append(codec.ToString());
                            sb.Append(": ");

                            if(frames != null && frames.Count > 0)
                            {
                                foreach(uint frame in frames)
                                {
                                    sb.Append(frame);
                                    sb.Append(", ");
                                }

                                sb.Remove(sb.Length-2, 2);  // remove trailing comma
                                sb.Append("\r\n");
                            }
                            else
                            {
                                sb.Append("<none>\r\n");
                            }
                        }

                        log.Write(TraceLevel.Verbose, sb.ToString().Trim());
                    }
                    else
                    {
                        log.Write(TraceLevel.Verbose, "GotCapabilities ({0}): <null>", callId);
                    }
                }
                else if (msg.MessageId == ICallControl.Events.MEDIA_ESTABLISHED ||
                    msg.MessageId == ICallControl.Events.MEDIA_CHANGED)
                {
                    log.Write(TraceLevel.Verbose, "{0} ({1}): Tx Addr={2}, RxAddr={3}, TxCodec={4}:{5}, RxCodec={6}:{7}.", 
                        Namespace.GetName(msg.MessageId), callId, 
                        txAddr != null ? txAddr.ToString() : "<null>", 
                        rxAddr != null ? rxAddr.ToString() : "<null>", 
                        txCodec.ToString(), txFramesize, rxCodec.ToString(), rxFramesize);
                }
            }
        }

        private IPEndPoint GetTxControlAddr()
        {
            if(txControlAddr != null)
                return txControlAddr;
            else if(txAddr != null)
            {
                try { return new IPEndPoint(txAddr.Address, txAddr.Port+1); }
                catch
                {
                    log.Write(TraceLevel.Warning, "Invalid Tx control address: {0}:{1}", txAddr.Address, txAddr.Port);
                    return null;
                }
            }
            else
                return null;
        }

        private IPEndPoint GetRxControlAddr()
        {
            if(rxControlAddr != null) 
                return rxControlAddr;
            else if(rxAddr != null)
            {
                try { return new IPEndPoint(rxAddr.Address, rxAddr.Port+1); }
                catch
                {
                    log.Write(TraceLevel.Warning, "Invalid Tx control address: {0}:{1}", txAddr.Address, txAddr.Port);
                    return null;
                }
            }
            else
                return null;
        }
        #endregion
	}
}
