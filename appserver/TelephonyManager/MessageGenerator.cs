using System;
using System.Net;

using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.Messaging.MediaCaps;

namespace Metreos.AppServer.TelephonyManager
{
	public class MessageGenerator
	{
        private const string DUMMY_ACTION_ID    = "AUTO_GENERATED";

        private MessageQueueWriter localQ;
        public MessageQueueWriter LocalQ { get { return localQ; } }

        private MessageUtility eventMsgUtil;
        private MessageUtility actionMsgUtil;

        public MessageGenerator()
        {
            this.localQ = MessageQueueFactory.GetQueueWriter(IConfig.CoreComponentNames.TEL_MANAGER);
            this.eventMsgUtil = new MessageUtility(IConfig.CoreComponentNames.TEL_MANAGER, IConfig.ComponentType.Provider, localQ);
            this.actionMsgUtil = new MessageUtility(IConfig.CoreComponentNames.TEL_MANAGER, IConfig.ComponentType.Application, localQ);
        }

        #region Events

        internal EventMessage CreateIncomingCallEvent(CallInfo cInfo)
        {
            if(cInfo.RoutingGuid == null)
                return null;

            EventMessage evtMsg = 
                eventMsgUtil.CreateEventMessage(ICallControl.Events.INCOMING_CALL, EventMessage.EventType.Triggering, cInfo.RoutingGuid);
            evtMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);
            return evtMsg;
        }

        internal EventMessage CreateHangupEvent(CallInfo cInfo)
        {
            if(cInfo.RoutingGuid == null)
                return null;

            EventMessage evtMsg = 
                eventMsgUtil.CreateEventMessage(ICallControl.Events.REMOTE_HANGUP, EventMessage.EventType.NonTriggering, cInfo.RoutingGuid);
            evtMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);
            evtMsg.AddField(ICallControl.Fields.END_REASON, cInfo.EndReason);
            return evtMsg;
        }

        internal EventMessage CreateMakeCallComplete(CallInfo cInfo)
        {
            if((cInfo.RoutingGuid == null) || (cInfo.RemoteParty == null))
                return null;

            EventMessage evtMsg = 
                eventMsgUtil.CreateEventMessage(ICallControl.Callbacks.MAKECALL_COMPLETE, EventMessage.EventType.AsyncCallback, cInfo.RoutingGuid);
            evtMsg.UserData = cInfo.UserData;

            evtMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);
            evtMsg.AddField(IMediaControl.Fields.MMS_ID, cInfo.MmsId);
            evtMsg.AddField(ICallControl.Fields.TO, cInfo.RemoteParty);
            evtMsg.AddField(ICallControl.Fields.FROM, cInfo.LocalParty);
            evtMsg.AddField(IMediaControl.Fields.CONNECTION_ID, cInfo.ConnectionId);
            evtMsg.AddField(IMediaControl.Fields.CONFERENCE_ID, cInfo.ConferenceId);

            if(cInfo.TxCodec != IMediaControl.Codecs.Unspecified && cInfo.TxFramesize != 0)
            { 
                evtMsg.AddField(IMediaControl.Fields.TX_CODEC, cInfo.TxCodec); 
                evtMsg.AddField(IMediaControl.Fields.TX_FRAMESIZE, cInfo.TxFramesize);
            }

            if(cInfo.TxAddr != null)
            {
                evtMsg.AddField(IMediaControl.Fields.TX_IP, cInfo.TxAddr.Address.ToString());
                evtMsg.AddField(IMediaControl.Fields.TX_PORT, cInfo.TxAddr.Port.ToString());
            }
            return evtMsg;
        }

        internal EventMessage CreateMakeCallFailed(CallInfo cInfo)
        {
            if(cInfo.RoutingGuid == null)
                return null;

            EventMessage evtMsg = 
                eventMsgUtil.CreateEventMessage(ICallControl.Callbacks.MAKECALL_FAILED, EventMessage.EventType.AsyncCallback, cInfo.RoutingGuid);
            evtMsg.UserData = cInfo.UserData;

            evtMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);
            evtMsg.AddField(ICallControl.Fields.TO, cInfo.RemoteParty);
            evtMsg.AddField(ICallControl.Fields.FROM, cInfo.LocalParty);
            evtMsg.AddField(ICallControl.Fields.END_REASON, cInfo.EndReason);
            return evtMsg;
        }

        internal EventMessage CreateStartTx(CallInfo cInfo)
        {
            if(cInfo.RoutingGuid == null)
                return null;

            EventMessage evtMsg = 
                eventMsgUtil.CreateEventMessage(ICallControl.Events.START_TX, EventMessage.EventType.NonTriggering, cInfo.RoutingGuid);
            evtMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);
            evtMsg.AddField(IMediaControl.Fields.MMS_ID, cInfo.MmsId);
            evtMsg.AddField(IMediaControl.Fields.CONNECTION_ID, cInfo.ConnectionId);

            if(cInfo.TxCodec != IMediaControl.Codecs.Unspecified && cInfo.TxFramesize != 0)
            { 
                evtMsg.AddField(IMediaControl.Fields.TX_CODEC, cInfo.TxCodec); 
                evtMsg.AddField(IMediaControl.Fields.TX_FRAMESIZE, cInfo.TxFramesize);
            }

            if(cInfo.TxAddr != null)
            {
                evtMsg.AddField(IMediaControl.Fields.TX_IP, cInfo.TxAddr.Address.ToString());
                evtMsg.AddField(IMediaControl.Fields.TX_PORT, cInfo.TxAddr.Port.ToString());
            }
            return evtMsg;
        }

        internal EventMessage CreateStartRx(CallInfo cInfo)
        {
            if(cInfo.RoutingGuid == null)
                return null;

            EventMessage evtMsg = 
                eventMsgUtil.CreateEventMessage(ICallControl.Events.START_RX, EventMessage.EventType.NonTriggering, cInfo.RoutingGuid);
            evtMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);
            evtMsg.AddField(IMediaControl.Fields.MMS_ID, cInfo.MmsId);
            evtMsg.AddField(IMediaControl.Fields.CONNECTION_ID, cInfo.ConnectionId);

            if(cInfo.RxCodec != IMediaControl.Codecs.Unspecified && cInfo.RxFramesize != 0)
            { 
                evtMsg.AddField(IMediaControl.Fields.RX_CODEC, cInfo.RxCodec); 
                evtMsg.AddField(IMediaControl.Fields.RX_FRAMESIZE, cInfo.RxFramesize);
            }

            if(cInfo.RxAddr != null)
            {
                evtMsg.AddField(IMediaControl.Fields.RX_IP, cInfo.RxAddr.Address.ToString());
                evtMsg.AddField(IMediaControl.Fields.RX_PORT, cInfo.RxAddr.Port.ToString());
            }
            return evtMsg;
        }

        internal EventMessage CreateStopTx(CallInfo cInfo)
        {
            if(cInfo.RoutingGuid == null) 
                return null;

            EventMessage evtMsg = 
                eventMsgUtil.CreateEventMessage(ICallControl.Events.STOP_TX, EventMessage.EventType.NonTriggering, cInfo.RoutingGuid);
            evtMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);
            evtMsg.AddField(IMediaControl.Fields.MMS_ID, cInfo.MmsId);
            evtMsg.AddField(IMediaControl.Fields.CONNECTION_ID, cInfo.ConnectionId);
            return evtMsg;
        }

        internal EventMessage CreateGotDigitsEvent(CallInfo cInfo, string digits)
        {
            if(cInfo.RoutingGuid == null)
                return null;

            EventMessage evtMsg = 
                eventMsgUtil.CreateEventMessage(ICallControl.Events.GOT_DIGITS, EventMessage.EventType.NonTriggering, cInfo.RoutingGuid);
            evtMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);
            evtMsg.AddField(ICallControl.Fields.DIGITS, digits);
            return evtMsg;
        }

        #endregion

        #region Actions

        #region Call Control

        internal ActionMessage CreateHangupAction(CallInfo cInfo, bool useDummyActionId)
        {
            return CreateHangupAction(cInfo, useDummyActionId, false);
        }

        internal ActionMessage CreateHangupAction(CallInfo cInfo, bool useDummyActionId, bool p2p)
        {
            if((cInfo.RoutingGuid == null) || (cInfo.ProviderNamespace == null)) 
                return null;

            string actionGuid;
            if(useDummyActionId)
                actionGuid = ActionGuid.Create(cInfo.RoutingGuid, DUMMY_ACTION_ID);
            else
                actionGuid = ActionGuid.Create(cInfo.RoutingGuid, cInfo.CurrStateId.ToString());

            string actionName;
            if(p2p)
                actionName = ICallControl.Actions.HANGUP;
            else
                actionName = cInfo.ProviderNamespace + ICallControl.Actions.Suffix.HANGUP;

            ActionMessage aMsg = actionMsgUtil.CreateActionMessage(actionName, actionGuid, 
                cInfo.AppName, null, cInfo.PartitionName);
            aMsg.AddField(ICallControl.Fields.CALL_ID, p2p ? cInfo.PeerCallId : cInfo.CallId);
            return aMsg;
        }

        internal ActionMessage CreateRejectCallAction(CallInfo cInfo, bool useDummyActionId)
        {
            if((cInfo.RoutingGuid == null) || (cInfo.ProviderNamespace == null)) 
                return null;

            string actionGuid;
            if(useDummyActionId)
                actionGuid = ActionGuid.Create(cInfo.RoutingGuid, DUMMY_ACTION_ID);
            else
                actionGuid = ActionGuid.Create(cInfo.RoutingGuid, cInfo.CurrStateId.ToString());

            string actionName = cInfo.ProviderNamespace + ICallControl.Actions.Suffix.REJECT_CALL;

            ActionMessage aMsg = actionMsgUtil.CreateActionMessage(actionName, actionGuid,
                cInfo.AppName, null, cInfo.PartitionName);
            aMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);
            return aMsg;
        }

        internal ActionMessage CreateAcceptCallAction(CallInfo cInfo)
        {
            if((cInfo.RoutingGuid == null) || (cInfo.ProviderNamespace == null)) 
                return null;

            string actionGuid = ActionGuid.Create(cInfo.RoutingGuid, cInfo.CurrStateId.ToString());
            string actionName = cInfo.ProviderNamespace + ICallControl.Actions.Suffix.ACCEPT_CALL;

            ActionMessage aMsg = actionMsgUtil.CreateActionMessage(actionName, actionGuid,
                cInfo.AppName, null, cInfo.PartitionName);

            aMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);
            aMsg.AddField(ICallControl.Fields.PEER_CALL_ID, cInfo.PeerCallId);
            return aMsg;
        }

        internal ActionMessage CreateP2PAcceptCallAction(CallInfo cInfo)
        {
            if((cInfo.RoutingGuid == null) || (cInfo.ProviderNamespace == null)) 
                return null;

            string actionGuid = ActionGuid.Create(cInfo.RoutingGuid, cInfo.CurrStateId.ToString());

            ActionMessage aMsg = actionMsgUtil.CreateActionMessage(ICallControl.Actions.ACCEPT_CALL, actionGuid,
                cInfo.AppName, null, cInfo.PartitionName);

            aMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.PeerCallId);
            return aMsg;
        }

        internal ActionMessage CreateP2PAnswerCallAction(CallInfo cInfo)
        {
            if((cInfo.RoutingGuid == null) || (cInfo.PeerCallId == 0)) 
                return null;

            string actionGuid = ActionGuid.Create(cInfo.RoutingGuid, cInfo.CurrStateId.ToString());
            string actionName = ICallControl.Actions.ANSWER_CALL;
            
            ActionMessage aMsg = actionMsgUtil.CreateActionMessage(actionName, actionGuid,
                cInfo.AppName, null, cInfo.PartitionName);

            aMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.PeerCallId);
            return aMsg;
        }

        internal ActionMessage CreateP2PHoldAction(CallInfo cInfo)
        {
            if((cInfo.RoutingGuid == null) || (cInfo.PeerCallId == 0)) 
                return null;

            string actionGuid = ActionGuid.Create(cInfo.RoutingGuid, cInfo.CurrStateId.ToString());
            string actionName = ICallControl.Actions.HOLD;
            
            ActionMessage aMsg = actionMsgUtil.CreateActionMessage(actionName, actionGuid,
                cInfo.AppName, null, cInfo.PartitionName);

            aMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.PeerCallId);
            return aMsg;
        }

        internal ActionMessage CreateP2PResumeAction(CallInfo cInfo)
        {
            if((cInfo.RoutingGuid == null) || (cInfo.PeerCallId == 0)) 
                return null;

            string actionGuid = ActionGuid.Create(cInfo.RoutingGuid, cInfo.CurrStateId.ToString());
            string actionName = ICallControl.Actions.RESUME;
            
            ActionMessage aMsg = actionMsgUtil.CreateActionMessage(actionName, actionGuid,
                cInfo.AppName, null, cInfo.PartitionName);

            aMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.PeerCallId);
            return aMsg;
        }

        internal ActionMessage CreateSetMediaAction(CallInfo cInfo)
        {
            if((cInfo.RoutingGuid == null) || (cInfo.ProviderNamespace == null)) 
                return null;

            string actionGuid = ActionGuid.Create(cInfo.RoutingGuid, cInfo.CurrStateId.ToString());
            string actionName = cInfo.ProviderNamespace + ICallControl.Actions.Suffix.SET_MEDIA;

            ActionMessage aMsg = actionMsgUtil.CreateActionMessage(actionName, actionGuid,
                cInfo.AppName, null, cInfo.PartitionName);
            
            aMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);

            if(cInfo.RxAddr != null) 
            { 
                aMsg.AddField(IMediaControl.Fields.RX_IP, cInfo.RxAddr.Address.ToString()); 
                aMsg.AddField(IMediaControl.Fields.RX_PORT, cInfo.RxAddr.Port);
            }
            if(cInfo.RxControlAddr != null)
            {
                aMsg.AddField(IMediaControl.Fields.RX_CONTROL_IP, cInfo.RxControlAddr.Address.ToString());
                aMsg.AddField(IMediaControl.Fields.RX_CONTROL_PORT, cInfo.RxControlAddr.Port);
            }
            if(cInfo.RxCodec != IMediaControl.Codecs.Unspecified && cInfo.RxFramesize != 0)
            {
                aMsg.AddField(IMediaControl.Fields.RX_CODEC, cInfo.RxCodec);
                aMsg.AddField(IMediaControl.Fields.RX_FRAMESIZE, cInfo.RxFramesize);
            }
            if(cInfo.TxCodec != IMediaControl.Codecs.Unspecified && cInfo.TxFramesize != 0)
            {
                aMsg.AddField(IMediaControl.Fields.TX_CODEC, cInfo.TxCodec);
                aMsg.AddField(IMediaControl.Fields.TX_FRAMESIZE, cInfo.TxFramesize);
            }
            if(cInfo.LocalMediaCaps != null)
                aMsg.AddField(ICallControl.Fields.LOCAL_MEDIA_CAPS, cInfo.LocalMediaCaps);
            return aMsg;
        }

        internal ActionMessage CreateHoldAction(CallInfo cInfo)
        {
            if((cInfo.RoutingGuid == null) || (cInfo.ProviderNamespace == null))
                return null;

            string actionGuid = ActionGuid.Create(cInfo.RoutingGuid, cInfo.CurrStateId.ToString());
            string actionName = cInfo.ProviderNamespace + ICallControl.Actions.Suffix.HOLD;

            ActionMessage aMsg = actionMsgUtil.CreateActionMessage(actionName, actionGuid, 
                cInfo.AppName, null, cInfo.PartitionName);
            aMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);
            return aMsg;
        }

        internal ActionMessage CreateResumeAction(CallInfo cInfo)
        {
            if((cInfo.RoutingGuid == null) || (cInfo.ProviderNamespace == null)) 
                return null;

            string actionGuid = ActionGuid.Create(cInfo.RoutingGuid, cInfo.CurrStateId.ToString());
            string actionName = cInfo.ProviderNamespace + ICallControl.Actions.Suffix.RESUME;

            ActionMessage aMsg = actionMsgUtil.CreateActionMessage(actionName, actionGuid,
                cInfo.AppName, null, cInfo.PartitionName);
            
            aMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);

            if(cInfo.RxAddr != null) 
            { 
                aMsg.AddField(IMediaControl.Fields.RX_IP, cInfo.RxAddr.Address.ToString()); 
                aMsg.AddField(IMediaControl.Fields.RX_PORT, cInfo.RxAddr.Port);
            }
            if(cInfo.RxControlAddr != null)
            {
                aMsg.AddField(IMediaControl.Fields.RX_CONTROL_IP, cInfo.RxControlAddr.Address.ToString());
                aMsg.AddField(IMediaControl.Fields.RX_CONTROL_PORT, cInfo.RxControlAddr.Port);
            }
            return aMsg;
        }

        internal ActionMessage CreateUseMohMediaAction(CallInfo cInfo)
        {
            if((cInfo.RoutingGuid == null) || (cInfo.ProviderNamespace == null))
                return null;

            string actionGuid = ActionGuid.Create(cInfo.RoutingGuid, cInfo.CurrStateId.ToString());
            string actionName = cInfo.ProviderNamespace + ICallControl.Actions.Suffix.USE_MOH_MEDIA;

            ActionMessage aMsg = actionMsgUtil.CreateActionMessage(actionName, actionGuid, 
                cInfo.AppName, null, cInfo.PartitionName);
            aMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);
            return aMsg;
        }

        internal ActionMessage CreateSendUserInput(CallInfo cInfo, string digits)
        {
            if((cInfo.RoutingGuid == null) || (cInfo.ProviderNamespace == null))
                return null;

            string actionGuid = ActionGuid.Create(cInfo.RoutingGuid, DUMMY_ACTION_ID);
            string actionName = cInfo.ProviderNamespace + ICallControl.Actions.Suffix.SEND_USER_INPUT;

            ActionMessage aMsg = actionMsgUtil.CreateActionMessage(actionName, actionGuid, 
                cInfo.AppName, null, cInfo.PartitionName);
            aMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);
            aMsg.AddField(ICallControl.Fields.DIGITS, digits);
            return aMsg;
        }

        #endregion

        #region Media Control

        internal ActionMessage CreateGetMediaCapsAction(CallInfo cInfo)
        {
            if(cInfo.RoutingGuid == null) { return null; }

            string actionGuid = ActionGuid.Create(cInfo.RoutingGuid, cInfo.CurrStateId.ToString());

            ActionMessage aMsg = actionMsgUtil.CreateActionMessage(IMediaControl.Actions.GET_MEDIA_CAPS, 
                actionGuid, cInfo.AppName, null, cInfo.PartitionName);
            aMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);
            return aMsg;
        }

        internal ActionMessage CreateReserveConnectionAction(CallInfo cInfo)
        {
            if(cInfo.RoutingGuid == null)
                return null;

            string actionGuid = ActionGuid.Create(cInfo.RoutingGuid, cInfo.CurrStateId.ToString());

            ActionMessage aMsg = actionMsgUtil.CreateActionMessage(IMediaControl.Actions.RESERVE_CONNECTION, 
                actionGuid, cInfo.AppName, null, cInfo.PartitionName);
            aMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);
            
			if(cInfo.MmsId > 0)
				aMsg.AddField(IMediaControl.Fields.MMS_ID, cInfo.MmsId);

            if(cInfo.TxCodec != IMediaControl.Codecs.Unspecified)
                aMsg.AddField(IMediaControl.Fields.TX_CODEC, cInfo.TxCodec);
            if(cInfo.RxCodec != IMediaControl.Codecs.Unspecified)
                aMsg.AddField(IMediaControl.Fields.RX_CODEC, cInfo.RxCodec);

            return aMsg;
        }

        internal ActionMessage CreateCreateConnectionAction(CallInfo cInfo)
        {
            return CreateConnectionAction(cInfo, false);
        }

        internal ActionMessage CreateModifyConnectionAction(CallInfo cInfo)
        {
            return CreateConnectionAction(cInfo, true);
        }

        private ActionMessage CreateConnectionAction(CallInfo cInfo, bool modify)
        {
            if(cInfo.RoutingGuid == null) 
                return null;

            string actionGuid = ActionGuid.Create(cInfo.RoutingGuid, cInfo.CurrStateId.ToString());
            string actionName = modify ? IMediaControl.Actions.MODIFY_CONNECTION : IMediaControl.Actions.CREATE_CONNECTION;

            ActionMessage aMsg = actionMsgUtil.CreateActionMessage(actionName, 
                actionGuid, cInfo.AppName, null, cInfo.PartitionName);
            aMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);
            aMsg.AddField(IMediaControl.Fields.CONNECTION_ID, cInfo.ConnectionId);
            
            if(cInfo.TxAddr != null)
            {
                aMsg.AddField(IMediaControl.Fields.TX_IP, cInfo.TxAddr.Address.ToString());
                aMsg.AddField(IMediaControl.Fields.TX_PORT, cInfo.TxAddr.Port);
            }

            if(cInfo.TxCodec != IMediaControl.Codecs.Unspecified && cInfo.TxFramesize != 0)
            {
                aMsg.AddField(IMediaControl.Fields.TX_CODEC, cInfo.TxCodec.ToString());
                aMsg.AddField(IMediaControl.Fields.TX_FRAMESIZE, cInfo.TxFramesize);
            }

            if(cInfo.RxCodec != IMediaControl.Codecs.Unspecified && cInfo.RxFramesize != 0)
            {
                aMsg.AddField(IMediaControl.Fields.RX_CODEC, cInfo.RxCodec.ToString());
                aMsg.AddField(IMediaControl.Fields.RX_FRAMESIZE, cInfo.RxFramesize);
            }

            if(cInfo.TxControlAddr != null && cInfo.TxControlAddr.Address != null)
            {
                aMsg.AddField(IMediaControl.Fields.TX_CONTROL_IP, cInfo.TxControlAddr.Address.ToString());
                aMsg.AddField(IMediaControl.Fields.TX_CONTROL_PORT, cInfo.TxControlAddr.Port);
            }

            return aMsg;
        }

        internal ActionMessage CreateDeleteConnectionAction(CallInfo cInfo, bool useDummyActionId)
        {
            if((cInfo.RoutingGuid == null) || (cInfo.ConnectionId == 0)) 
                return null;

            string actionGuid;
            if(useDummyActionId)
                actionGuid = ActionGuid.Create(cInfo.RoutingGuid, DUMMY_ACTION_ID);
            else
                actionGuid = ActionGuid.Create(cInfo.RoutingGuid, cInfo.CurrStateId.ToString());

            ActionMessage aMsg = actionMsgUtil.CreateActionMessage(IMediaControl.Actions.DELETE_CONNECTION, 
                actionGuid, cInfo.AppName, null, cInfo.PartitionName);
            aMsg.AddField(IMediaControl.Fields.CONNECTION_ID, cInfo.ConnectionId);
            aMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);

            return aMsg;
        }

        internal ActionMessage CreateCreateConferenceAction(CallInfo cInfo)
        {
            if(cInfo.RoutingGuid == null) 
                return null;

            ActionMessage aMsg = CreateConnectionAction(cInfo, false);
            aMsg.MessageId = IMediaControl.Actions.CREATE_CONFERENCE;
            
            aMsg.AddField(IMediaControl.Fields.CONFERENCE_ID, cInfo.ConferenceId);
            aMsg.AddField(IMediaControl.Fields.TONE_JOIN, false);
			aMsg.AddField(IMediaControl.Fields.HAIRPIN, cInfo.Hairpin);

            return aMsg;
        }

        internal ActionMessage CreateJoinConferenceAction(CallInfo cInfo)
        {
            if((cInfo.RoutingGuid == null) || (cInfo.ConferenceId == 0)) 
                return null;

            ActionMessage aMsg = CreateConnectionAction(cInfo, false);
            aMsg.MessageId = IMediaControl.Actions.JOIN_CONFERENCE;

            aMsg.AddField(IMediaControl.Fields.CONFERENCE_ID, cInfo.ConferenceId);
			aMsg.AddField(IMediaControl.Fields.HAIRPIN, cInfo.Hairpin);

            return aMsg;
        }

        internal ActionMessage CreateSendDigitsAction(CallInfo cInfo, string digits, bool useDummyActionId)
        {
            if((cInfo.RoutingGuid == null) || (cInfo.ConnectionId == 0)) 
                return null;

            string actionGuid;
            if(useDummyActionId)
                actionGuid = ActionGuid.Create(cInfo.RoutingGuid, DUMMY_ACTION_ID);
            else
                actionGuid = ActionGuid.Create(cInfo.RoutingGuid, cInfo.CurrStateId.ToString());

            ActionMessage aMsg = actionMsgUtil.CreateActionMessage(IMediaControl.Actions.SEND_DIGITS, 
                actionGuid, cInfo.AppName, null, cInfo.PartitionName);
            aMsg.AddField(IMediaControl.Fields.CONNECTION_ID, cInfo.ConnectionId);
            aMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);
            aMsg.AddField(IMediaControl.Fields.DIGITS, digits);
            return aMsg;
        }

        internal ActionMessage CreateStopMediaAction(CallInfo cInfo, bool useDummyActionId)
        {
            if((cInfo.RoutingGuid == null) || (cInfo.ConnectionId == 0)) 
                return null;
            
            string actionGuid;
            if(useDummyActionId)
                actionGuid = ActionGuid.Create(cInfo.RoutingGuid, DUMMY_ACTION_ID);
            else
                actionGuid = ActionGuid.Create(cInfo.RoutingGuid, cInfo.CurrStateId.ToString());

            ActionMessage aMsg = actionMsgUtil.CreateActionMessage(IMediaControl.Actions.STOP_MEDIA, 
                actionGuid, cInfo.AppName, null, cInfo.PartitionName);
            aMsg.AddField(IMediaControl.Fields.CONNECTION_ID, cInfo.ConnectionId);
            aMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);
            aMsg.AddField(IMediaControl.Fields.BLOCK, true);
            return aMsg;
        }
        #endregion

        #region P2P Internal

        internal ActionMessage CreateP2PCreateConferenceAction(CallInfo cInfo)
        {
            if((cInfo.RoutingGuid == null) || (cInfo.PeerCallId == 0)) 
                return null;

            string actionGuid = ActionGuid.Create(cInfo.RoutingGuid, cInfo.CurrStateId.ToString());
            string actionName = ICallControl.Actions.P2P.CreateConference;
            
            ActionMessage aMsg = actionMsgUtil.CreateActionMessage(actionName, actionGuid,
                cInfo.AppName, null, cInfo.PartitionName);

            aMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.PeerCallId);
			aMsg.AddField(IMediaControl.Fields.HAIRPIN, true);
            return aMsg;
        }

        internal ActionMessage CreateP2PDeleteConnectionAction(CallInfo cInfo)
        {
            if((cInfo.RoutingGuid == null) || (cInfo.PeerCallId == 0)) 
                return null;

            string actionGuid = ActionGuid.Create(cInfo.RoutingGuid, cInfo.CurrStateId.ToString());
            string actionName = ICallControl.Actions.P2P.DeleteConnection;
            
            ActionMessage aMsg = actionMsgUtil.CreateActionMessage(actionName, actionGuid,
                cInfo.AppName, null, cInfo.PartitionName);

            aMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.PeerCallId);
            return aMsg;
        }

        #endregion

        #region Special

        public ActionMessage CreateNoHandler(EventMessage msg, bool sessionActive)
        {
            string actionGuid = msg.RoutingGuid + ".0";
            ActionMessage noHandler = actionMsgUtil.CreateActionMessage(IActions.NoHandler, actionGuid);
            noHandler.AddField(IActions.Fields.InnerMsg, msg);
            noHandler.AddField(IActions.Fields.SessionActive, sessionActive);
            return noHandler;
        }
        
        #endregion

        #endregion

        #region Helper Methods

        internal static bool IsDummyResponse(ResponseMessage rMsg)
        {
            if(rMsg == null)
                return false;

            return ActionGuid.GetActionId(rMsg.InResponseToActionGuid) == DUMMY_ACTION_ID;
        }

        internal ResponseMessage CreateResponse(CallInfo cInfo, bool success, string actionName, long callId)
        {
            string actionGuid = ActionGuid.Create(cInfo.RoutingGuid, cInfo.CurrStateId.ToString());

            ResponseMessage msg = new ResponseMessage(actionGuid);
            msg.MessageId = success ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
            msg.InResponseTo = actionName;

            if(callId != 0)
                msg.AddField(ICallControl.Fields.CALL_ID, callId);

            return msg;
        }

        #endregion
	}
}
