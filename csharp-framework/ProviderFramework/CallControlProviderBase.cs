using System;
using System.Diagnostics;
using System.Collections;

using Metreos.Core;
using Metreos.Core.ConfigData;
using Metreos.Core.ConfigData.CallRouteGroups;
using Metreos.Messaging;
using Metreos.Messaging.MediaCaps;
using Metreos.Interfaces;
using Metreos.Utilities;

namespace Metreos.ProviderFramework
{
    /// <summary>Base class for all providers which implement a call control protocol</summary>
    /// <remarks>Child class constructors must be of the form: 
    /// public ClassName(IConfigUtility configUtility, ICallIdFactory callIdFactory)</remarks>
	public abstract class CallControlProviderBase : ProviderBase
	{
        private IConfig.ComponentType primaryProtocolType;

        protected CallGuidMap routingGuids;
        protected ICallIdFactory callIdFactory;

		public CallControlProviderBase(Type providerType, string displayName, IConfig.ComponentType protocolType, 
			IConfigUtility config, ICallIdFactory callIdFactory)
			: this(providerType, displayName, protocolType, config, callIdFactory, ProviderBase.Consts.Defaults.AsyncRefresh) {}

        public CallControlProviderBase(Type providerType, string displayName, IConfig.ComponentType protocolType, 
            IConfigUtility config, ICallIdFactory callIdFactory, bool asyncRefresh)
            : base(providerType, displayName, config, asyncRefresh)
        {
            Debug.Assert(callIdFactory != null, "callIdFactory is null");

            int protType = (int)protocolType;
            if((protType < 100) || (protType > 149))
                throw new ArgumentException("Invalid call control protocol type: " + protocolType);

            this.callIdFactory = callIdFactory;
            this.primaryProtocolType = protocolType;
            this.routingGuids = new CallGuidMap();
        }

        protected sealed override bool Initialize(out ConfigEntry[] configItems, out Extension[] extensions)
        {
            messageCallbacks.Add(providerNamespace + ICallControl.Actions.Suffix.ACCEPT_CALL, 
                new HandleMessageDelegate(this.HandleAcceptCall));
            messageCallbacks.Add(providerNamespace + ICallControl.Actions.Suffix.ANSWER_CALL, 
                new HandleMessageDelegate(this.HandleAnswerCall));
            messageCallbacks.Add(providerNamespace + ICallControl.Actions.Suffix.HANGUP, 
                new HandleMessageDelegate(this.HandleHangup));
            messageCallbacks.Add(providerNamespace + ICallControl.Actions.Suffix.MAKE_CALL, 
                new HandleMessageDelegate(this.HandleMakeCall));
            messageCallbacks.Add(providerNamespace + ICallControl.Actions.Suffix.REJECT_CALL, 
                new HandleMessageDelegate(this.HandleRejectCall));
            messageCallbacks.Add(providerNamespace + ICallControl.Actions.Suffix.REDIRECT, 
                new HandleMessageDelegate(this.HandleRedirectCall));
            messageCallbacks.Add(providerNamespace + ICallControl.Actions.Suffix.SET_MEDIA, 
                new HandleMessageDelegate(this.HandleSetMedia));
            messageCallbacks.Add(providerNamespace + ICallControl.Actions.Suffix.HOLD,
                new HandleMessageDelegate(this.HandleHold));
            messageCallbacks.Add(providerNamespace + ICallControl.Actions.Suffix.RESUME,
                new HandleMessageDelegate(this.HandleResume));
            messageCallbacks.Add(providerNamespace + ICallControl.Actions.Suffix.USE_MOH_MEDIA,
                new HandleMessageDelegate(this.HandleUseMohMedia));

            // Supplementary services
            messageCallbacks.Add(providerNamespace + ICallControl.Actions.Suffix.SEND_USER_INPUT,
                new HandleMessageDelegate(this.HandleSendUserInput));
            messageCallbacks.Add(providerNamespace + ICallControl.Actions.Suffix.BLIND_XFER, 
                new HandleMessageDelegate(this.HandleBlindTransfer));
            messageCallbacks.Add(providerNamespace + ICallControl.Actions.Suffix.BEGIN_CONS_XFER, 
                new HandleMessageDelegate(this.HandleBeginConsTransfer));
            messageCallbacks.Add(providerNamespace + ICallControl.Actions.Suffix.END_CONS_XFER, 
                new HandleMessageDelegate(this.HandleEndConsTransfer));
            messageCallbacks.Add(providerNamespace + ICallControl.Actions.Suffix.BARGE,
                new HandleMessageDelegate(this.HandleBarge));

            // Default handler for hairpinned actions
            base.defaultHandler = new HandleMessageDelegate(this.HandleHairpinAction);

            return DeclareConfig(out configItems, out extensions);
        }

        protected abstract bool DeclareConfig(out ConfigEntry[] configItems, out Extension[] extensions);

        #region CallControl Action Handlers

        private void HandleAcceptCall(ActionBase actionBase)
        {
            long callId, peerId = 0;
            try
            {
                actionBase.InnerMessage.GetInt64(ICallControl.Fields.CALL_ID, true, out callId);
                actionBase.InnerMessage.GetInt64(ICallControl.Fields.PEER_CALL_ID, false, out peerId);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                actionBase.SendResponse(false);
                return;
            }

            // Do *not* attempt to use the value of peerId for anything!
            bool p2p = peerId != 0;

            if(HandleAcceptCall(callId, p2p))
            {
                actionBase.SendResponse(true, new Field(ICallControl.Fields.CALL_ID, callId));
            }
            else
            {
                actionBase.SendResponse(false, new Field(ICallControl.Fields.CALL_ID, callId));
            }
        }

        private void HandleRedirectCall(ActionBase actionBase)
        {
            long callId;
            string to;
            try
            {
                actionBase.InnerMessage.GetInt64(ICallControl.Fields.CALL_ID, true, out callId);
                actionBase.InnerMessage.GetString(ICallControl.Fields.TO, true, out to);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                actionBase.SendResponse(false);
                return;
            }

            if(HandleRedirectCall(callId, to))
            {
                actionBase.SendResponse(true, new Field(ICallControl.Fields.CALL_ID, callId));
            }
            else
            {
                actionBase.SendResponse(false, new Field(ICallControl.Fields.CALL_ID, callId));
            }
        }

        private void HandleAnswerCall(ActionBase actionBase)
        {
            string displayName = Convert.ToString(actionBase.InnerMessage[ICallControl.Fields.DISPLAY_NAME]);

            long callId;
            if(GetCallId(actionBase, out callId))
            {
                if(HandleAnswerCall(callId, displayName))
                {
                    actionBase.SendResponse(true, new Field(ICallControl.Fields.CALL_ID, callId));
                }
                else
                {
                    actionBase.SendResponse(false, new Field(ICallControl.Fields.CALL_ID, callId));
                }
                return;
            }

            actionBase.SendResponse(false);
        }

        private void HandleHangup(ActionBase actionBase)
        {
            long callId;
            if(GetCallId(actionBase, out callId))
            {
                if(HandleHangup(callId))
                {
                    actionBase.SendResponse(true, new Field(ICallControl.Fields.CALL_ID, callId));
                }
                else
                {
                    actionBase.SendResponse(false, new Field(ICallControl.Fields.CALL_ID, callId));
                }
                routingGuids.Remove(callId);
                return;
            }

            actionBase.SendResponse(false);
        }

        private void HandleRejectCall(ActionBase actionBase)
        {
            long callId;
            if(GetCallId(actionBase, out callId))
            {
                if(HandleRejectCall(callId))
                {
                    actionBase.SendResponse(true, new Field(ICallControl.Fields.CALL_ID, callId));
                }
                else
                {
                    actionBase.SendResponse(false, new Field(ICallControl.Fields.CALL_ID, callId));
                }
                return;
            }

            actionBase.SendResponse(false);
        }

        private void HandleMakeCall(ActionBase actionBase)
        {
            object callInfoObj = actionBase.InnerMessage[ICallControl.Fields.OUTBOUND_CALLINFO];
            OutboundCallInfo callInfo = callInfoObj as OutboundCallInfo;
            if(callInfo == null)
            {
                log.Write(TraceLevel.Error, "Received a MakeCall message without an OutboundCallInfo object");
                actionBase.SendResponse(false);
                return;
            }

            // Take note of call ID -> Routing GUID mapping
            routingGuids.Add(callInfo.CallId, actionBase.RoutingGuid);
           
            if(HandleMakeCall(callInfo))
            {
                actionBase.SendResponse(true, new Field(ICallControl.Fields.CALL_ID, callInfo.CallId));
            }
            else
            {
                actionBase.SendResponse(false, new Field(ICallControl.Fields.CALL_ID, callInfo.CallId));
            }
        }

        private void HandleSetMedia(ActionBase actionBase)
        {
            long callId;
            string rxIP=null, rxControlIP=null;
            uint rxPort=0, rxControlPort=0, rxFramesize=0, txFramesize=0, rxCodec=0, txCodec=0;
            try
            {
                actionBase.InnerMessage.GetInt64(ICallControl.Fields.CALL_ID, true, out callId);
                actionBase.InnerMessage.GetString(IMediaControl.Fields.RX_IP, false, out rxIP);
                actionBase.InnerMessage.GetString(IMediaControl.Fields.RX_CONTROL_IP, false, out rxControlIP);
                actionBase.InnerMessage.GetUInt32(IMediaControl.Fields.RX_PORT, false, out rxPort);
                actionBase.InnerMessage.GetUInt32(IMediaControl.Fields.RX_CONTROL_PORT, false, out rxControlPort);
                actionBase.InnerMessage.GetUInt32(IMediaControl.Fields.RX_FRAMESIZE, false, out rxFramesize);
                actionBase.InnerMessage.GetUInt32(IMediaControl.Fields.TX_FRAMESIZE, false, out txFramesize);
                actionBase.InnerMessage.GetUInt32(IMediaControl.Fields.RX_CODEC, false, out rxCodec);
                actionBase.InnerMessage.GetUInt32(IMediaControl.Fields.TX_CODEC, false, out txCodec);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                actionBase.SendResponse(false);
                return;
            }

            MediaCapsField caps = actionBase.InnerMessage[ICallControl.Fields.LOCAL_MEDIA_CAPS] as MediaCapsField;

            ArrayList fields = new ArrayList();
            fields.Add(new Field(ICallControl.Fields.CALL_ID, callId));

            // Save the peer call ID (if present) for TM routing purposes.
            object peerCallIdObj = actionBase.InnerMessage[ICallControl.Fields.PEER_CALL_ID];
            if(peerCallIdObj != null)
                fields.Add(new Field(ICallControl.Fields.PEER_CALL_ID, peerCallIdObj));

            if(HandleSetMedia(callId, rxIP, rxPort, rxControlIP, rxControlPort, (IMediaControl.Codecs) rxCodec, 
                rxFramesize, (IMediaControl.Codecs) txCodec, txFramesize, caps))
            {
                actionBase.SendResponse(true, fields);
            }
            else
            {
                actionBase.SendResponse(false, fields);
            }
        }

        private void HandleHold(ActionBase actionBase)
        {
            long callId;
            try
            {
                actionBase.InnerMessage.GetInt64(ICallControl.Fields.CALL_ID, true, out callId);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                actionBase.SendResponse(false);
                return;
            }

            if(HandleHold(callId))
            {
                actionBase.SendResponse(true, new Field(ICallControl.Fields.CALL_ID, callId));
            }
            else
            {
                actionBase.SendResponse(false, new Field(ICallControl.Fields.CALL_ID, callId));
            }
        }

        private void HandleResume(ActionBase actionBase)
        {
            long callId;
            string rxIP=null, rxControlIP=null;
            uint rxPort=0, rxControlPort=0;
            try
            {
                actionBase.InnerMessage.GetInt64(ICallControl.Fields.CALL_ID, true, out callId);
                actionBase.InnerMessage.GetString(IMediaControl.Fields.RX_IP, false, out rxIP);
                actionBase.InnerMessage.GetString(IMediaControl.Fields.RX_CONTROL_IP, false, out rxControlIP);
                actionBase.InnerMessage.GetUInt32(IMediaControl.Fields.RX_PORT, false, out rxPort);
                actionBase.InnerMessage.GetUInt32(IMediaControl.Fields.RX_CONTROL_PORT, false, out rxControlPort);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                actionBase.SendResponse(false);
                return;
            }

            if(HandleResume(callId, rxIP, rxPort, rxControlIP, rxControlPort))
            {
                actionBase.SendResponse(true, new Field(ICallControl.Fields.CALL_ID, callId));
            }
            else
            {
                actionBase.SendResponse(false, new Field(ICallControl.Fields.CALL_ID, callId));
            }
        }

        private void HandleUseMohMedia(ActionBase actionBase)
        {
            long callId;
            try
            {
                actionBase.InnerMessage.GetInt64(ICallControl.Fields.CALL_ID, true, out callId);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                actionBase.SendResponse(false);
                return;
            }

            if(HandleUseMohMedia(callId))
            {
                actionBase.SendResponse(true, new Field(ICallControl.Fields.CALL_ID, callId));
            }
            else
            {
                actionBase.SendResponse(false, new Field(ICallControl.Fields.CALL_ID, callId));
            }
        }

        private void HandleBlindTransfer(ActionBase actionBase)
        {
            long callId;
            string to;
            try
            {
                actionBase.InnerMessage.GetInt64(ICallControl.Fields.CALL_ID, true, out callId);
                actionBase.InnerMessage.GetString(ICallControl.Fields.TO, true, out to);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                actionBase.SendResponse(false);
                return;
            }

            if(HandleBlindTransfer(callId, to))
            {
                actionBase.SendResponse(true, new Field(ICallControl.Fields.CALL_ID, callId));
            }
            else
            {
                actionBase.SendResponse(false, new Field(ICallControl.Fields.CALL_ID, callId));
            }
        }

        private void HandleBeginConsTransfer(ActionBase actionBase)
        {
            long callId;
            string to;
            MediaCapsField caps;
            try
            {
                actionBase.InnerMessage.GetInt64(ICallControl.Fields.CALL_ID, true, out callId);
                actionBase.InnerMessage.GetString(ICallControl.Fields.TO, true, out to);

                caps = actionBase.InnerMessage[ICallControl.Fields.LOCAL_MEDIA_CAPS] as MediaCapsField;
                if(caps == null)
                    throw new ArgumentException(String.Format("Required parameter {0} not found in {1} message", 
                        ICallControl.Fields.LOCAL_MEDIA_CAPS, actionBase.InnerMessage.MessageId));
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                actionBase.SendResponse(false);
                return;
            }

            ArrayList fields = new ArrayList();
            fields.Add(new Field(ICallControl.Fields.CALL_ID, callId));

            long transCallId;
            string txIP;
            ushort txPort; 
            IMediaControl.Codecs codec; 
            int framesize;

            if(HandleBeginConsTransfer(callId, to, caps, out transCallId, out txIP, out txPort, out codec, out framesize))
            {
                fields.Add(new Field(ICallControl.Fields.TRANS_CALL_ID, transCallId));
                fields.Add(new Field(IMediaControl.Fields.TX_IP, txIP));
                fields.Add(new Field(IMediaControl.Fields.TX_PORT, txPort));
                fields.Add(new Field(IMediaControl.Fields.TX_CODEC, codec));
                fields.Add(new Field(IMediaControl.Fields.TX_FRAMESIZE, framesize));
                fields.Add(new Field(IMediaControl.Fields.RX_CODEC, codec));
                fields.Add(new Field(IMediaControl.Fields.RX_FRAMESIZE, framesize));
                actionBase.SendResponse(true, fields);
            }
            else
            {
                actionBase.SendResponse(false, fields);
            }
        }

        private void HandleEndConsTransfer(ActionBase actionBase)
        {
            long callId, transCallId;
            try
            {
                actionBase.InnerMessage.GetInt64(ICallControl.Fields.CALL_ID, true, out callId);
                actionBase.InnerMessage.GetInt64(ICallControl.Fields.TRANS_CALL_ID, true, out transCallId);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                actionBase.SendResponse(false);
                return;
            }

            if(HandleEndConsTransfer(callId, transCallId))
            {
                actionBase.SendResponse(true, new Field(ICallControl.Fields.CALL_ID, callId));
            }
            else
            {
                actionBase.SendResponse(false, new Field(ICallControl.Fields.CALL_ID, callId));
            }
        }

        private void HandleSendUserInput(ActionBase actionBase)
        {
            long callId;
            string digits;
            try
            {
                actionBase.InnerMessage.GetInt64(ICallControl.Fields.CALL_ID, true, out callId);
                actionBase.InnerMessage.GetString(ICallControl.Fields.DIGITS, true, out digits);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                actionBase.SendResponse(false);
                return;
            }

            if(HandleSendUserInput(callId, digits))
            {
                actionBase.SendResponse(true, new Field(ICallControl.Fields.CALL_ID, callId));
            }
            else
            {
                actionBase.SendResponse(false, new Field(ICallControl.Fields.CALL_ID, callId));
            }
        }

        private void HandleBarge(ActionBase actionBase)
        {
            long callId;
            string dn;
            try
            {
                actionBase.InnerMessage.GetInt64(ICallControl.Fields.CALL_ID, true, out callId);
                actionBase.InnerMessage.GetString(ICallControl.Fields.FROM, true, out dn);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                actionBase.SendResponse(false);
                return;
            }

			// Take note of call ID -> Routing GUID mapping
			routingGuids.Add(callId, actionBase.RoutingGuid);

            if(HandleBarge(callId, dn))
            {
                actionBase.SendResponse(true, new Field(ICallControl.Fields.CALL_ID, callId));
            }
            else
            {
                actionBase.SendResponse(false, new Field(ICallControl.Fields.CALL_ID, callId));
            }
        }

		private void HandleHairpinAction(ActionBase actionBase)
		{
			long callId;
			try
			{
				actionBase.InnerMessage.GetInt64(ICallControl.Fields.CALL_ID, true, out callId);
			}
			catch(Exception e)
			{
				log.Write(TraceLevel.Error, e.Message);
				actionBase.SendResponse(false);
				return;
			}

			// Take note of call ID -> Routing GUID mapping
			routingGuids.Add(callId, actionBase.RoutingGuid);

			if(HandleHairpinAction(actionBase, callId))
			{
				actionBase.SendResponse(true, new Field(ICallControl.Fields.CALL_ID, callId));
			}
			else
			{
				actionBase.SendResponse(false, new Field(ICallControl.Fields.CALL_ID, callId));
			}
		}

        private bool GetCallId(ActionBase actionBase, out long callId)
        {
            callId = 0;
            try { actionBase.InnerMessage.GetInt64(ICallControl.Fields.CALL_ID, true, out callId); }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                return false;
            }
            return true;
        }
        #endregion

        #region Abstract CallControl Action Handlers (Provider must implement)

        protected abstract bool HandleAcceptCall(long callId, bool p2p);
        protected abstract bool HandleRedirectCall(long callId, string to);
        protected abstract bool HandleAnswerCall(long callId, string displayName);
        protected abstract bool HandleHangup(long callId);
        protected abstract bool HandleMakeCall(OutboundCallInfo callInfo);
        protected abstract bool HandleRejectCall(long callId);
        protected abstract bool HandleSetMedia(long callId, string rxIP, uint rxPort, 
            string rxControlIP, uint rxControlPort, IMediaControl.Codecs rxCodec, uint rxFramesize, 
            IMediaControl.Codecs txCodec, uint txFramesize, MediaCapsField caps);
        protected abstract bool HandleHold(long callId);
        protected abstract bool HandleResume(long callId, string rxIP, uint rxPort, string rxControlIP, 
            uint rxControlPort);

        #endregion

        #region Virtual CallControl Action Handlers (Provider may implement)

        // Supplementary services (not required)
        protected virtual bool HandleSendUserInput(long callId, string digits)
        {
            log.Write(TraceLevel.Error, "{0} provider does not support out-of-band DTMF", this.Name);
            return false;
        }

        protected virtual bool HandleBlindTransfer(long callId, string to)
        {
            log.Write(TraceLevel.Error, "{0} provider does not support Blind Transfer", this.Name);
            return false;
        }

        protected virtual bool HandleBeginConsTransfer(long callId, string to, MediaCapsField caps, 
            out long transCallId, out string txIP, out ushort txPort, out IMediaControl.Codecs codec, 
            out int framesize)
        {
            transCallId = 0;
            txIP = null;
            txPort = 0; 
            codec = IMediaControl.Codecs.Unspecified;
            framesize = 0;

            log.Write(TraceLevel.Error, "{0} provider does not support Consultation Transfer", this.Name);
            return false;
        }

        protected virtual bool HandleEndConsTransfer(long callId, long transCallId)
        {
            log.Write(TraceLevel.Error, "{0} provider does not support Consultation Transfer", this.Name);
            return false;
        }

        protected virtual bool HandleUseMohMedia(long callId)
        {
            // Just smile and nod  :)
            return true;
        }

        protected virtual bool HandleBarge(long callId, string dn)
        {
            log.Write(TraceLevel.Error, "{0} provider does not support Barge", this.Name);
            return false;
        }

        protected virtual bool HandleHairpinAction(ActionBase action, long callId)
        {
            if(action != null && action.InnerMessage != null)
            {
                log.Write(TraceLevel.Error, "{0} provider does not support {1}", 
                    this.Name, action.InnerMessage.MessageId);
				return false;
            }

			return true;
        }

        #endregion

        #region Event senders

        protected void SendIncomingCall(long callId, string stackCallId, string to, string from, 
            string originalTo, bool negCaps, string displayName)
        {
            string routingGuid = System.Guid.NewGuid().ToString();
            routingGuids.Add(callId, routingGuid);

            EventMessage msg = messageUtility.CreateEventMessage(ICallControl.Events.INCOMING_CALL,
                EventMessage.EventType.Triggering, routingGuid);
            msg.AddField(ICallControl.Fields.SOURCE_NS, providerNamespace);
            msg.AddField(ICallControl.Fields.CALL_ID, callId);
            msg.AddField(ICallControl.Fields.STACK_TOKEN, stackCallId);
            msg.AddField(ICallControl.Fields.TO, to);
            msg.AddField(ICallControl.Fields.TO_ORIG, originalTo);
            msg.AddField(ICallControl.Fields.FROM, from);
			msg.AddField(ICallControl.Fields.NEG_CAPS, negCaps);
            msg.AddField(ICallControl.Fields.DISPLAY_NAME, displayName);
            palWriter.PostMessage(msg);
        }

        protected void SendHangup(long callId)
        {
            SendHangup(callId, ICallControl.EndReason.Normal);
        }

        protected void SendHangup(long callId, ICallControl.EndReason reason)
        {
            string routingGuid = routingGuids[callId];
            if(routingGuid != null)
            {
                EventMessage msg = messageUtility.CreateEventMessage(ICallControl.Events.REMOTE_HANGUP,
                    EventMessage.EventType.NonTriggering, routingGuid);
                msg.AddField(ICallControl.Fields.CALL_ID, callId);
                msg.AddField(ICallControl.Fields.END_REASON, reason.ToString());
                routingGuids.Remove(callId);
                palWriter.PostMessage(msg);
            }
        }

        protected void SendCallEstablished(long callId, string to, string from)
        {
            string routingGuid = routingGuids[callId];
			if(routingGuid != null)
			{
				EventMessage msg = messageUtility.CreateEventMessage(ICallControl.Events.CALL_ESTABLISHED,
					EventMessage.EventType.NonTriggering, routingGuid);
				msg.AddField(ICallControl.Fields.CALL_ID, callId);
				msg.AddField(ICallControl.Fields.TO, to);
				msg.AddField(ICallControl.Fields.FROM, from);
				palWriter.PostMessage(msg);
			}
			else
			{
				log.Write(TraceLevel.Error, "Provider attempted to send a CallEstablished message with an invalid call ID: " + callId);
			}
        }

        protected void SendCallChanged(long callId, string to, string from)
        {
            string routingGuid = routingGuids[callId];
			if(routingGuid != null)
			{
				EventMessage msg = messageUtility.CreateEventMessage(ICallControl.Events.CALL_CHANGED,
					EventMessage.EventType.NonTriggering, routingGuid);
				msg.AddField(ICallControl.Fields.CALL_ID, callId);
				msg.AddField(ICallControl.Fields.TO, to);
				msg.AddField(ICallControl.Fields.FROM, from);
				palWriter.PostMessage(msg);
			}
			else
			{
				log.Write(TraceLevel.Error, "Provider attempted to send a CallChanged message with an invalid call ID: " + callId);
			}
        }

        protected void SendCallSetupFailed(long callId, ICallControl.EndReason reason)
        {
            string routingGuid = routingGuids[callId];
			if(routingGuid != null)
			{
				EventMessage msg = messageUtility.CreateEventMessage(ICallControl.Events.CALL_SETUP_FAILED,
					EventMessage.EventType.NonTriggering, routingGuid);
				msg.AddField(ICallControl.Fields.CALL_ID, callId);
				msg.AddField(ICallControl.Fields.END_REASON, reason.ToString());
				palWriter.PostMessage(msg);
			}
			else
			{
				log.Write(TraceLevel.Error, "Provider attempted to send a CallSetupFailed message with an invalid call ID: " + callId);
			}
        }

        protected void SendGotCapabilities(long callId, MediaCapsField remoteMediaCaps)
        {
            string routingGuid = routingGuids[callId];
			if(routingGuid != null)
			{
				EventMessage msg = messageUtility.CreateEventMessage(ICallControl.Events.GOT_CAPABILITIES,
					EventMessage.EventType.NonTriggering, routingGuid);
				msg.AddField(ICallControl.Fields.CALL_ID, callId);

				if(remoteMediaCaps != null) { msg.AddField(ICallControl.Fields.REMOTE_MEDIA_CAPS, remoteMediaCaps); }
				palWriter.PostMessage(msg);
			}
			else
			{
				log.Write(TraceLevel.Error, "Provider attempted to send a GotCapabilities message with an invalid call ID: " + callId);
			}
        }

        protected void SendMediaEstablished(long callId, string txIP, uint txPort, string txControlIP, 
            uint txControlPort, IMediaControl.Codecs rxCodec, uint rxFramesize, IMediaControl.Codecs txCodec, uint txFramesize)
        {
            string routingGuid = routingGuids[callId];
			if(routingGuid != null)
			{
				EventMessage msg = messageUtility.CreateEventMessage(ICallControl.Events.MEDIA_ESTABLISHED,
					EventMessage.EventType.NonTriggering, routingGuid);
				msg.AddField(ICallControl.Fields.CALL_ID, callId);

				if(txIP != null) { msg.AddField(IMediaControl.Fields.TX_IP, txIP); }
				if(txPort != 0) { msg.AddField(IMediaControl.Fields.TX_PORT, txPort); }
				if(txControlIP != null) { msg.AddField(IMediaControl.Fields.TX_CONTROL_IP, txControlIP); }
				if(txControlPort != 0) { msg.AddField(IMediaControl.Fields.TX_CONTROL_PORT, txControlPort); }
				if(rxCodec != IMediaControl.Codecs.Unspecified)
					msg.AddField(IMediaControl.Fields.RX_CODEC, rxCodec);
				if(rxFramesize != 0) { msg.AddField(IMediaControl.Fields.RX_FRAMESIZE, rxFramesize); }
				if(txCodec != IMediaControl.Codecs.Unspecified)
					msg.AddField(IMediaControl.Fields.TX_CODEC, txCodec);
				if(txFramesize != 0) { msg.AddField(IMediaControl.Fields.TX_FRAMESIZE, txFramesize); }

				palWriter.PostMessage(msg);
			}
			else
			{
				log.Write(TraceLevel.Error, "Provider attempted to send a MediaEstablished message with an invalid call ID: " + callId);
			}
        }

        protected void SendMediaChanged(long callId, string txIP, uint txPort)
        {
            SendMediaChanged(callId, txIP, txPort, txIP, txPort+1, IMediaControl.Codecs.Unspecified, 0, 
                IMediaControl.Codecs.Unspecified, 0);
        }

        protected void SendMediaChanged(long callId, string txIP, uint txPort, string txControlIP, 
            uint txControlPort, IMediaControl.Codecs rxCodec, uint rxFramesize, IMediaControl.Codecs txCodec, uint txFramesize)
        {
            string routingGuid = routingGuids[callId];
			if(routingGuid != null)
			{
				EventMessage msg = messageUtility.CreateEventMessage(ICallControl.Events.MEDIA_CHANGED,
					EventMessage.EventType.NonTriggering, routingGuid);
				msg.AddField(ICallControl.Fields.CALL_ID, callId);

				// Tx address can be null
				msg.AddField(IMediaControl.Fields.TX_IP, txIP);
				msg.AddField(IMediaControl.Fields.TX_PORT, txPort);
				msg.AddField(IMediaControl.Fields.TX_CONTROL_IP, txControlIP);
				msg.AddField(IMediaControl.Fields.TX_CONTROL_PORT, txControlPort);

				if(rxCodec != IMediaControl.Codecs.Unspecified)
					msg.AddField(IMediaControl.Fields.RX_CODEC, rxCodec);
				if(rxFramesize != 0) { msg.AddField(IMediaControl.Fields.RX_FRAMESIZE, rxFramesize); }
				if(txCodec != IMediaControl.Codecs.Unspecified)
					msg.AddField(IMediaControl.Fields.TX_CODEC, txCodec);
				if(txFramesize != 0) { msg.AddField(IMediaControl.Fields.TX_FRAMESIZE, txFramesize); }

				palWriter.PostMessage(msg);
			}
			else
			{
				log.Write(TraceLevel.Error, "Provider attempted to send a MediaChanged message with an invalid call ID: " + callId);
			}
        }

        protected void SendRemoteHold(long callId)
        {
            string routingGuid = routingGuids[callId];
            if(routingGuid != null)
            {
                EventMessage msg = messageUtility.CreateEventMessage(ICallControl.Events.REMOTE_HOLD,
                    EventMessage.EventType.NonTriggering, routingGuid);
                msg.AddField(ICallControl.Fields.CALL_ID, callId);

                // Clear everything TM knows about the media for this call
                msg.AddField(IMediaControl.Fields.TX_IP, null);
                msg.AddField(IMediaControl.Fields.TX_PORT, 0);
                msg.AddField(IMediaControl.Fields.TX_CONTROL_IP, null);
                msg.AddField(IMediaControl.Fields.TX_CONTROL_PORT, 0);

                palWriter.PostMessage(msg);
            }
            else
            {
                log.Write(TraceLevel.Error, "Provider attempted to send a MediaChanged message with an invalid call ID: " + callId);
            }
        }

        protected void SendRemoteResume(long callId, string txIP, uint txPort, string txControlIP, uint txControlPort)
        {
            string routingGuid = routingGuids[callId];
            if(routingGuid != null)
            {
                EventMessage msg = messageUtility.CreateEventMessage(ICallControl.Events.REMOTE_RESUME,
                    EventMessage.EventType.NonTriggering, routingGuid);
                msg.AddField(ICallControl.Fields.CALL_ID, callId);

                // Tx address can be null
                msg.AddField(IMediaControl.Fields.TX_IP, txIP);
                msg.AddField(IMediaControl.Fields.TX_PORT, txPort);
                msg.AddField(IMediaControl.Fields.TX_CONTROL_IP, txControlIP);
                msg.AddField(IMediaControl.Fields.TX_CONTROL_PORT, txControlPort);

                palWriter.PostMessage(msg);
            }
            else
            {
                log.Write(TraceLevel.Error, "Provider attempted to send a MediaChanged message with an invalid call ID: " + callId);
            }
        }

        protected void SendGotDigits(long callId, string digits)
        {
            string routingGuid = routingGuids[callId];
			if(routingGuid != null)
			{
				EventMessage msg = messageUtility.CreateEventMessage(ICallControl.Events.GOT_DIGITS,
					EventMessage.EventType.NonTriggering, routingGuid);
				msg.AddField(ICallControl.Fields.CALL_ID, callId);
				msg.AddField(ICallControl.Fields.DIGITS, digits);
				palWriter.PostMessage(msg);
			}
			else
			{
				log.Write(TraceLevel.Error, "Provider attempted to send a GotDigits message with an invalid call ID: " + callId);
			}
        }

        /// <summary>
        /// This allows a provider to send non-call-event-related messaging 
        /// to the peer of the current call. 
        /// </summary>
        /// <remarks>
        /// For example, if a provider is handling a remote resume and for some reason 
        /// there are superfluous messages which must be passed back and forth which cannot 
        /// be derived simply from a resume action, then this allows one leg to tell the
        /// other what to do.
        /// 
        /// Using this method will not change the TM state or cause it to run any scripts.
        /// This method must not be used to circumvent TM for normal call events.
        /// 
        /// Generally, a message about the event should be sent to TM and then this method 
        /// called only for protocol-specific peculiarities.
        /// </remarks>
        /// <param name="callId">Call ID</param>
        /// <param name="actionName">The unqualified name of the action (suffix)</param>
        /// <param name="fields">Data to accompany the action</param>
        protected void SendHairpinAction(long callId, string actionName, Field[] fields)
        {
            string routingGuid = routingGuids[callId];
            if(routingGuid != null)
            {
                // Build action to hairpin
                if(!actionName.StartsWith("."))
                    actionName = "." + actionName;

                actionName = this.providerNamespace + actionName;

                ActionMessage aMsg = this.messageUtility.CreateActionMessage(actionName,
                    ActionGuid.Create(routingGuid, "0"));
                aMsg.Destination = this.providerNamespace;

				if (fields != null)
				{
					foreach(Field field in fields)
					{
						aMsg.AddField(field.Name, field.Value);
					}
				}

                // Build event to tunnel action
                EventMessage msg = messageUtility.CreateEventMessage(ICallControl.Events.PROV_HAIRPIN,
                    EventMessage.EventType.NonTriggering, routingGuid);
                msg.AddField(ICallControl.Fields.CALL_ID, callId);
                msg.AddField(ICallControl.Fields.HAIRPIN_ACTION, aMsg);
                palWriter.PostMessage(msg);
            }
            else
            {
                log.Write(TraceLevel.Error, "Provider attempted to send a Hairpin action with an invalid call ID: " + callId);
            }
        }

        #endregion

        #region Namespace Registration

        protected void RegisterSecondaryProtocolType(IConfig.ComponentType secondaryProtocolType)
        {
            int protType = (int)secondaryProtocolType;
            if((protType < 100) || (protType > 149))
                throw new ArgumentException("Invalid call control protocol type: " + secondaryProtocolType);

            RegisterNamespace(secondaryProtocolType);
        }

        protected void RegisterExtensionNamespace(string extNamespace)
        {
            Debug.Assert(palWriter != null, "palWriter is null");

            CommandMessage registerNamespaceMsg = 
                this.CreateCommandMessage(IConfig.CoreComponentNames.PROV_MANAGER, ICommands.REGISTER_PROV_NAMESPACE);
            registerNamespaceMsg.AddField(ICommands.Fields.PROVIDER_NAMESPACE, extNamespace);

            this.palWriter.PostMessage(registerNamespaceMsg);
        }

        new protected void RegisterNamespace()
        {
            RegisterNamespace(this.primaryProtocolType);
        }

        private void RegisterNamespace(IConfig.ComponentType protocolType)
        {
            Debug.Assert(palWriter != null, "palWriter is null");

            CommandMessage registerNamespaceMsg = 
                this.CreateCommandMessage(IConfig.CoreComponentNames.PROV_MANAGER, ICommands.REGISTER_PROV_NAMESPACE);
            registerNamespaceMsg.AddField(ICommands.Fields.PROVIDER_NAMESPACE, providerNamespace);
            registerNamespaceMsg.AddField(ICommands.Fields.CC_PROTOCOL, protocolType);

            this.palWriter.PostMessage(registerNamespaceMsg);
        }

        #endregion
	}
}
