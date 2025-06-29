using System;
using System.Diagnostics;
using System.Collections;

using Metreos.Core;
using Metreos.Core.ConfigData;
using Metreos.Core.ConfigData.CallRouteGroups;
using Metreos.Utilities;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.Messaging.MediaCaps;

namespace Metreos.AppServer.TelephonyManager
{
    internal delegate CrgData GetCrgDelegate(string appName, string partName);

	internal sealed class ActionHandler
	{
        internal GetCrgDelegate GetCrg;

        private readonly LogWriter log;
        private readonly IConfigUtility configUtility;
        private readonly MessageGenerator msgGen;

        private readonly MessageQueueWriter routerQ;
        public MessageQueueWriter RouterQ { get { return routerQ; } }

        private readonly MessageQueueWriter tmQ;

        public ActionHandler(MessageGenerator msgGen, IConfigUtility config, LogWriter log)
        {
            this.log = log;
            this.configUtility = config;
            this.msgGen = msgGen;

            this.routerQ = MessageQueueFactory.GetQueueWriter(IConfig.CoreComponentNames.ROUTER);
            if(routerQ == null) 
                throw new Exception("Could not locate the Router's message queue");

            this.tmQ = MessageQueueFactory.GetQueueWriter(IConfig.CoreComponentNames.TEL_MANAGER);
            if(tmQ == null) 
                throw new Exception("Could not locate the Telephony Manager's message queue");
        }

        #region Action/Event/Response Handling

        public bool ForwardEventToApp(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            // Send the most recently handled event to the app
            EventMessage eMsg = cInfo.LastHandledEvent;
            if(eMsg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Cannot forward event: No events on stack.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            eMsg.Source = IConfig.CoreComponentNames.TEL_MANAGER;
            eMsg.SourceQueue = msgGen.LocalQ;
            routerQ.PostMessage(eMsg);
            return true;
        }

        public bool ForwardResponseToApp(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            // Send the most recently handled response to the app
            ResponseMessage rMsg = cInfo.LastHandledResponse;
            if(rMsg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Cannot forward response: No responses on stack.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            // Add supplimentary fields
            if( rMsg.InResponseTo.EndsWith(ICallControl.Actions.Suffix.ANSWER_CALL) ||
                rMsg.InResponseTo.EndsWith(ICallControl.Actions.Suffix.BARGE))
            {
                rMsg.AddField(IMediaControl.Fields.CONNECTION_ID, cInfo.ConnectionId);
                rMsg.AddField(IMediaControl.Fields.CONFERENCE_ID, cInfo.ConferenceId);
                rMsg.AddField(IMediaControl.Fields.MMS_ID, cInfo.MmsId);
                rMsg.AddField(IMediaControl.Fields.TX_IP, cInfo.TxAddr == null ? "" : cInfo.TxAddr.Address.ToString());
                rMsg.AddField(IMediaControl.Fields.TX_PORT, cInfo.TxAddr == null ? 0 : cInfo.TxAddr.Port);
                rMsg.AddField(IMediaControl.Fields.TX_CODEC, cInfo.TxCodec.ToString());
                rMsg.AddField(IMediaControl.Fields.TX_FRAMESIZE, cInfo.TxFramesize);
                rMsg.AddField(IMediaControl.Fields.RX_IP, cInfo.RxAddr == null ? "" : cInfo.RxAddr.Address.ToString());
                rMsg.AddField(IMediaControl.Fields.RX_PORT, cInfo.RxAddr == null ? 0 : cInfo.RxAddr.Port);
                rMsg.AddField(IMediaControl.Fields.RX_CODEC, cInfo.RxCodec.ToString());
                rMsg.AddField(IMediaControl.Fields.RX_FRAMESIZE, cInfo.RxFramesize);
            }
            else if(rMsg.InResponseTo.EndsWith(ICallControl.Actions.Suffix.ACCEPT_CALL))
            {
                rMsg.AddField(IMediaControl.Fields.CONNECTION_ID, cInfo.ConnectionId);
                rMsg.AddField(IMediaControl.Fields.MMS_ID, cInfo.MmsId);
                rMsg.AddField(IMediaControl.Fields.RX_IP, cInfo.RxAddr == null ? "" : cInfo.RxAddr.Address.ToString());
                rMsg.AddField(IMediaControl.Fields.RX_PORT, cInfo.RxAddr == null ? 0 : cInfo.RxAddr.Port);
            }

            rMsg.Source = IConfig.CoreComponentNames.TEL_MANAGER;
            routerQ.PostMessage(rMsg);
            return true;
        }

        public bool ForwardActionToProvider(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            // Send the most recently handled action to its provider
            ActionMessage aMsg = cInfo.LastHandledAction;
            if(aMsg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Cannot forward action: No actions on stack.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            // If this is a MakeCall, build an OutboundCallInfo object and pass that to the provider.
            if(aMsg.MessageId == ICallControl.Actions.MAKE_CALL)
            {
                if(!cInfo.CallIdSpecified)
                {
                    log.Write(TraceLevel.Error, "[{0}:{1}] An ID was not generated for this call: {2}",
                        cInfo.MapName, cInfo.CurrStateId.ToString(), aMsg);
                    return false;
                }

                if(GetCrg == null)
                    throw new ApplicationException("ActionHandler.GetCrg delegate is null");

                CrgData crgData = GetCrg(aMsg.AppName, aMsg.PartitionName);

                if(crgData == null || crgData.Members == null || crgData.Members.Length == 0)
                {
                    log.Write(TraceLevel.Error, "No Call Route Group configured for: {0}->{1}",
                        aMsg.AppName, aMsg.PartitionName);
                    return false;
                }

                string to = Convert.ToString(aMsg[ICallControl.Fields.TO]);
                string from = Convert.ToString(aMsg[ICallControl.Fields.FROM]);
                string displayName = Convert.ToString(aMsg[ICallControl.Fields.DISPLAY_NAME]);

                OutboundCallInfo callInfo = new OutboundCallInfo(cInfo.CallId, aMsg.AppName, aMsg.PartitionName,
                    crgData.Members, to, from, displayName, cInfo.LocalMediaCaps, cInfo.RxAddr, cInfo.PeerCallId != 0);

                aMsg.AddField(ICallControl.Fields.OUTBOUND_CALLINFO, callInfo);
            }
            else if(aMsg.MessageId == ICallControl.Actions.BARGE)
            {
                Assertion.Check(cInfo.CallIdSpecified, "An ID was not generated for this call: " + aMsg);
                aMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.CallId);
            }
            else if(aMsg.MessageId == ICallControl.Actions.ACCEPT_CALL)
            {
                aMsg.AddField(ICallControl.Fields.PEER_CALL_ID, cInfo.PeerCallId);
            }
            else if(aMsg.MessageId == ICallControl.Actions.REJECT_CALL ||
                aMsg.MessageId == ICallControl.Actions.REDIRECT ||
                aMsg.MessageId == ICallControl.Actions.END_CONS_XFER ||
                aMsg.MessageId == ICallControl.Actions.BLIND_XFER)
            {
                cInfo.CallTerminated = true;
            }

            // Make a copy. Don't jack with the original message.          
            ActionMessage msgCopy = new ActionMessage(aMsg);
            msgCopy.Destination = cInfo.ProviderNamespace;
            msgCopy.Source = IConfig.CoreComponentNames.TEL_MANAGER;
            msgCopy.SourceQueue = msgGen.LocalQ;

            // Translate action namespace so it is routed to the proper provider
            Assertion.Check(cInfo.ProviderNamespace != null, "Provider namespace not set");
            msgCopy.MessageId = msgCopy.MessageId.Replace(ICallControl.NAMESPACE, cInfo.ProviderNamespace);

            log.Write(TraceLevel.Verbose, "Forwarding {0} action to provider", msgCopy.MessageId);

            routerQ.PostMessage(msgCopy);
            return true;
        }

        public bool SendActionFailureToApp(CallInfo cInfo)
        {
            FinalizeAction(cInfo);
            ActionMessage msg = cInfo.LastHandledAction;
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Cannot send failure response because no actions have been processed.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            msg.SendResponse(IApp.VALUE_FAILURE);
            return true;
        }

        public bool SendMakeCallCompleteToApp(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            if(cInfo.CallEstablished == false)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Cannot send MakeCall_Complete for a call which has not been established.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            EventMessage msg = msgGen.CreateMakeCallComplete(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create MakeCall_Complete event.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            routerQ.PostMessage(msg);
            return true;
        }

        public bool SendMakeCallFailedToApp(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            EventMessage msg = msgGen.CreateMakeCallFailed(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create MakeCall_Failed event.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            log.Write(TraceLevel.Warning, "Outbound call attempt to '{0}' failed. Reason: {1}", cInfo.RemoteParty, cInfo.EndReason);

            routerQ.PostMessage(msg);
            return true;
        }

        public bool SendHangupToApp(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            if(cInfo.EndReason == null || cInfo.EndReason == String.Empty)
            {
                if(cInfo.CallEstablished)
                    cInfo.EndReason = ICallControl.EndReason.InternalError.ToString();
                else
                    cInfo.EndReason = ICallControl.EndReason.Ringout.ToString();
            }

            EventMessage msg = msgGen.CreateHangupEvent(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create Hangup event.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            routerQ.PostMessage(msg);
            return true;
        }

        public bool SendStartTxToApp(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            EventMessage msg = msgGen.CreateStartTx(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create StartTx event.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            routerQ.PostMessage(msg);
            return true;
        }

        public bool SendStartRxToApp(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            EventMessage msg = msgGen.CreateStartRx(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create StartRx event.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            routerQ.PostMessage(msg);
            return true;
        }

        public bool SendStopTxToApp(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            EventMessage msg = msgGen.CreateStopTx(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create StopTx event.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            routerQ.PostMessage(msg);
            return true;
        }

        #endregion

        #region Call Control actions

        public bool HangupCall(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            ActionMessage msg = msgGen.CreateHangupAction(cInfo, false);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create Hangup action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            cInfo.CallTerminated = true;

            routerQ.PostMessage(msg);
            return true;
        }

        public bool RejectCall(CallInfo cInfo)
        {
            return RejectCall(cInfo, null);
        }

        public bool RejectCall(CallInfo cInfo, string provNamespace)
        {
            FinalizeAction(cInfo);

            if(provNamespace == null) 
            {
                provNamespace = cInfo.ProviderNamespace;
            }

            ActionMessage msg = msgGen.CreateRejectCallAction(cInfo, false);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create RejectCall action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            cInfo.CallTerminated = true;

            routerQ.PostMessage(msg);
            return true;
        }

        public bool AcceptCall(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            ActionMessage msg = msgGen.CreateAcceptCallAction(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create AcceptCall action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            routerQ.PostMessage(msg);
            return true;
        }

        public bool HoldCall(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            ActionMessage msg = msgGen.CreateHoldAction(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create Hold action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            routerQ.PostMessage(msg);
            return true;
        }

        public bool ResumeCall(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            ActionMessage msg = msgGen.CreateResumeAction(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create Resume action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            routerQ.PostMessage(msg);
            return true;
        }

        #endregion

        #region Media Actions

        public bool GetMediaCaps(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            ActionMessage msg = msgGen.CreateGetMediaCapsAction(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create GetMediaCaps action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            routerQ.PostMessage(msg);
            return true;
        }

        public bool ReserveConnection(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            ActionMessage msg = msgGen.CreateReserveConnectionAction(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create ReserveMedia action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            routerQ.PostMessage(msg);
            return true;
        }

        public bool CreateConnection(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            ActionMessage msg = msgGen.CreateCreateConnectionAction(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create CreateConnection action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            routerQ.PostMessage(msg);
            return true;
        }

        public bool ModifyConnection(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            ActionMessage msg = msgGen.CreateModifyConnectionAction(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create CreateConnection action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            routerQ.PostMessage(msg);
            return true;
        }

        public bool StopMediaOperation(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            ActionMessage msg = msgGen.CreateStopMediaAction(cInfo, false);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create StopMediaOperation action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            routerQ.PostMessage(msg);
            return true;
        }

        public bool DeleteConnection(CallInfo cInfo, bool useDummyId)
        {
            if(useDummyId == false)
                FinalizeAction(cInfo);

            if(cInfo.ConnectionId == 0)
                return true;

            // If we fail to create a DeleteConnection, just drop it quietly,
            //   because the this is probably just a duplicate due to overzealous 
            //   sandboxing efforts.
            ActionMessage msg = msgGen.CreateDeleteConnectionAction(cInfo, useDummyId);
            if(msg != null)
            {
                log.Write(TraceLevel.Info, "Deleting connection: {0}:{1}", cInfo.CallId, cInfo.ConnectionId);
                routerQ.PostMessage(msg);
                
                // Set connection ID to null since it is now invalid
                cInfo.ConnectionId = 0;
            }

            return true;
        }

        public bool CreateConference(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            ActionMessage msg = msgGen.CreateCreateConferenceAction(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create CreateConference action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            routerQ.PostMessage(msg);
            return true;
        }

        public bool JoinConference(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            ActionMessage msg = msgGen.CreateJoinConferenceAction(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create JoinConference action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            routerQ.PostMessage(msg);
            return true;
        }

        public bool SetMedia(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            ActionMessage msg = msgGen.CreateSetMediaAction(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create SetMedia action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            routerQ.PostMessage(msg);
            return true;
        }

        public bool SelectTxCodec(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            if(cInfo.LocalMediaCaps == null || cInfo.LocalMediaCaps.Count == 0)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Cannot select TX codec. No local media info available",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            log.Write(TraceLevel.Verbose, "Beginning Tx codec selection ({0}).\nLocal caps:\n{1}\nRemote caps:\n{2}",
                cInfo.CallId, cInfo.LocalMediaCaps != null ? cInfo.LocalMediaCaps.ToString() : "<null>", 
                cInfo.RemoteMediaCaps != null ? cInfo.RemoteMediaCaps.ToString() : "<null>");

            // Get preferred codec info from CRG cache
            CrgData crgData = GetCrg(cInfo.AppName, cInfo.PartitionName);
            if(crgData == null || crgData.PartitionInfo == null || 
                crgData.PartitionInfo.PreferredCodec == IMediaControl.Codecs.Unspecified ||
                crgData.PartitionInfo.PreferredFramesize == 0)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Could not retrieve preferred codec for {2}->{3}",
                    cInfo.MapName, cInfo.CurrStateId.ToString(), cInfo.AppName, cInfo.PartitionName);
                return false;
            }

            uint negFrame = 0;
            if(cInfo.RemoteMediaCaps != null)
            {
                // Look for preferred codec in remote caps set
                uint[] rFrames = cInfo.RemoteMediaCaps[crgData.PartitionInfo.PreferredCodec];
				if(rFrames != null)
				{
					// Look for preferred codec & framesize in local caps set
					uint[] lFrames = cInfo.LocalMediaCaps[crgData.PartitionInfo.PreferredCodec];
					if(lFrames != null)
					{
						ArrayList lFramesArray = new ArrayList(lFrames);
						ArrayList rFramesArray = new ArrayList(rFrames);

						if (lFramesArray.Contains(crgData.PartitionInfo.PreferredFramesize) &&
							rFramesArray.Contains(crgData.PartitionInfo.PreferredFramesize))
						{
							cInfo.TxCodec = crgData.PartitionInfo.PreferredCodec;
							cInfo.TxFramesize = crgData.PartitionInfo.PreferredFramesize;

							LogSelectedTxCodec(cInfo);
							return true;
						}
						else if(cInfo.LocalMediaCaps.MatchCaps(crgData.PartitionInfo.PreferredCodec, 
                            rFramesArray, out negFrame))
						{
							// We couldn't get the preferred framesize, 
							//  so just pick the first common one.
							cInfo.TxCodec = crgData.PartitionInfo.PreferredCodec;
							cInfo.TxFramesize = negFrame;

							LogSelectedTxCodec(cInfo);
							return true;
						}
					}
				}

				// If we didn't get our preference, pick the first remote capability we have
                foreach(DictionaryEntry de in cInfo.RemoteMediaCaps)
                {
                    IMediaControl.Codecs codec = (IMediaControl.Codecs) de.Key;
                    ArrayList framesizes = de.Value as ArrayList;
                
                    if(framesizes == null || framesizes.Count == 0)
                    {
                        log.Write(TraceLevel.Warning, "No framesizes specified in remote media caps for codec ({0}): {1}",
                            cInfo.CallId, codec.ToString());
                        continue;
                    }

                    if(cInfo.LocalMediaCaps.MatchCaps(codec, framesizes, out negFrame))
                    {
                        cInfo.TxCodec = codec;
                        cInfo.TxFramesize = negFrame;

                        LogSelectedTxCodec(cInfo);
                        return true;
                    }
                }
            }
            else if(cInfo.LocalMediaCaps.MatchCaps(cInfo.RxCodec, new uint[] { cInfo.RxFramesize }, out negFrame))
            {
                // We don't have any remote caps, but we have an Rx codec. 
                // So if we support that codec, let's use it for Tx also to make life easy.
                cInfo.TxCodec = cInfo.RxCodec;
                cInfo.TxFramesize = cInfo.RxFramesize;

                LogSelectedTxCodec(cInfo);
                return true;
            }
            else if(cInfo.RxCodec == IMediaControl.Codecs.Unspecified ||
                cInfo.RxCodec == 0)
            {
                // We have nothing to go on, so pick our preferred set if possible
                if(cInfo.LocalMediaCaps.MatchCaps(crgData.PartitionInfo.PreferredCodec, 
                    new uint[] { crgData.PartitionInfo.PreferredFramesize }, out negFrame))
                {
                    cInfo.TxCodec = crgData.PartitionInfo.PreferredCodec;
                    cInfo.TxFramesize = crgData.PartitionInfo.PreferredFramesize;

                    LogSelectedTxCodec(cInfo);
                    return true;
                }
                else
                {
                    // We really have nothing, just pick anything blindly and hope  :/
                    foreach(DictionaryEntry de in cInfo.LocalMediaCaps)
                    {
                        IMediaControl.Codecs codec = (IMediaControl.Codecs) de.Key;
                        ArrayList framesizes = de.Value as ArrayList;

                        if(framesizes == null || framesizes.Count == 0)
                            continue;

                        cInfo.TxCodec = codec;
                        cInfo.TxFramesize = (uint)framesizes[0];

                        LogSelectedTxCodec(cInfo);
                        return true;
                    }
                }
            }
            
            log.Write(TraceLevel.Error, "Could not negotiate capabilities for call: {0}\nLocal Caps:\n{1}\nRemote Caps:\n{2}",
                cInfo.CallId, cInfo.LocalMediaCaps.ToString(), 
                cInfo.RemoteMediaCaps != null ? cInfo.RemoteMediaCaps.ToString() : "<null>");
            return false;
        }

        public bool AssumePreferredTxCodec(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            // Get preferred codec info from CRG cache
            CrgData crgData = GetCrg(cInfo.AppName, cInfo.PartitionName);
            if(crgData == null || crgData.PartitionInfo == null || 
                crgData.PartitionInfo.PreferredCodec == IMediaControl.Codecs.Unspecified ||
                crgData.PartitionInfo.PreferredFramesize == 0)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Could not retrieve preferred codec for {2}->{3}",
                    cInfo.MapName, cInfo.CurrStateId.ToString(), cInfo.AppName, cInfo.PartitionName);
                return false;
            }

            log.Write(TraceLevel.Info, "Assuming preferred codec is Tx codec ({0}): {1}:{2}",
                cInfo.CallId, crgData.PartitionInfo.PreferredCodec.ToString(),
                crgData.PartitionInfo.PreferredFramesize.ToString());

            cInfo.TxCodec = crgData.PartitionInfo.PreferredCodec;
            cInfo.TxFramesize = crgData.PartitionInfo.PreferredFramesize;

            return true;
        }

        private void LogSelectedTxCodec(CallInfo cInfo)
        {
            log.Write(TraceLevel.Verbose, "Selected Tx codec ({0}): {1}:{2}",
                cInfo.CallId, cInfo.TxCodec.ToString(), cInfo.TxFramesize.ToString());
        }

        #endregion

        #region Peer-To-Peer actions

        public bool SendBridgeSuccessToApp(CallInfo cInfo, CallInfo peerInfo)
        {
            FinalizeAction(cInfo);

            // Gotta have a peer
            if(peerInfo == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Attempting to send BridgeCalls success: Ne peer call found",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            // Get action to create response
            ActionMessage bridgeMsg = cInfo.LastHandledAction;
            if(bridgeMsg.MessageId != ICallControl.Actions.BRIDGE_CALLS)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Attempting to send BridgeCalls success: Last handled action is: {2}",
                    cInfo.MapName, cInfo.CurrStateId.ToString(), bridgeMsg.MessageId);
                return false;
            }

            ArrayList fields = new ArrayList();
            fields.Add(new Field(IMediaControl.Fields.CONNECTION_ID, cInfo.ConnectionId));
            fields.Add(new Field(IMediaControl.Fields.PEER_CONN_ID, peerInfo.ConnectionId));
            fields.Add(new Field(IMediaControl.Fields.CONFERENCE_ID, cInfo.ConferenceId));
            fields.Add(new Field(IMediaControl.Fields.RX_IP, cInfo.RxAddr.Address.ToString()));

            bridgeMsg.SendResponse(IApp.VALUE_SUCCESS, fields, true);
            return true;
        }

        public bool SendUnbridgeSuccessToApp(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            // Get action to create response
            ActionMessage msg = cInfo.LastHandledAction;
            if(msg.MessageId != ICallControl.Actions.UNBRIDGE_CALLS)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Attempting to send UnbridgeCalls success: Last handled action is: {2}",
                    cInfo.MapName, cInfo.CurrStateId.ToString(), msg.MessageId);
                return false;
            }

            msg.SendResponse(IApp.VALUE_SUCCESS);
            return true;
        }

        public bool ForwardResponseToPeer(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            // Send the most recently handled response to the app
            ResponseMessage rMsg = cInfo.LastHandledResponse;
            if(rMsg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Cannot forward response: No responses on stack.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            // Adjust the call ID for proper routing
            rMsg.RemoveField(ICallControl.Fields.CALL_ID);
            rMsg.AddField(ICallControl.Fields.CALL_ID, cInfo.PeerCallId);

            tmQ.PostMessage(rMsg);
            return true;
        }

        public bool SendActionSuccessToPeer(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            ActionMessage msg = cInfo.LastHandledAction;
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Cannot send failure response because no actions have been processed.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            // Adjust the call ID for proper routing
            msg.RemoveField(ICallControl.Fields.CALL_ID);
            
            ArrayList fields = new ArrayList();
            fields.Add(new Field(ICallControl.Fields.CALL_ID, cInfo.PeerCallId));

            msg.SendResponse(IApp.VALUE_SUCCESS, fields, false);
            return true;
        }

        public bool SendActionFailureToPeer(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            ActionMessage msg = cInfo.LastHandledAction;
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Cannot send failure response because no actions have been processed.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            // Adjust the call ID for proper routing
            msg.RemoveField(ICallControl.Fields.CALL_ID);
            
            ArrayList fields = new ArrayList();
            fields.Add(new Field(ICallControl.Fields.CALL_ID, cInfo.PeerCallId));

            msg.SendResponse(IApp.VALUE_FAILURE, fields, false);
            return true;
        }

        public bool SetPeerMedia(CallInfo cInfo, CallInfo peerInfo)
        {
            FinalizeAction(cInfo);

            if(cInfo.PeerCallId == 0)
            {
                log.Write(TraceLevel.Error, "Cannot set peer media for call '{0}'. No peer specified", cInfo.CallId);
                return false;
            }

            // Only sync peer media if we are not bridged
            if(cInfo.ConnectionId == 0)
            {
                if(SyncPeerMedia(cInfo, peerInfo, true) == false)
                    return false;
            }

            ActionMessage msg = msgGen.CreateSetMediaAction(peerInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create P2P SetMedia action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            // This will enable the response to be routed back to this call object
            msg.AddField(ICallControl.Fields.PEER_CALL_ID, cInfo.CallId);

            // Set this flag because the action ID in the response
            //   will not match the current state ID of this call.
            //   This flag instructs the engine to relax that requirement.
            cInfo.WaitingForPeerResponse = true;

            // Send this action directly to peer's call in the provider
            routerQ.PostMessage(msg);
            return true;
        }

        public bool UseMohMedia(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            ActionMessage msg = msgGen.CreateUseMohMediaAction(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create UseMusicOnHoldMedia action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            routerQ.PostMessage(msg);
            return true;
        }

        public bool JoinPeerConference(CallInfo cInfo, CallInfo peerInfo)
        {
            FinalizeAction(cInfo);

            if(peerInfo == null)
            {
                log.Write(TraceLevel.Error, "Cannot create bridge '{0}'. No peer found", cInfo.CallId);
                return false;
            }

            if(peerInfo.ConferenceId == 0)
            {
                log.Write(TraceLevel.Error, "Cannot create bridge '{0}'. Peer has not created a conference", cInfo.CallId);
                return false;
            }

            // All we have to do here is add the current connection to
            //  the conference created by the peer
            cInfo.ConferenceId = peerInfo.ConferenceId;

            ActionMessage msg = msgGen.CreateJoinConferenceAction(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create JoinConference action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            routerQ.PostMessage(msg);
            return true;
        }

        public bool CreatePeerConference(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            if(cInfo.PeerCallId == 0)
            {
                log.Write(TraceLevel.Error, "Cannot create peer conference '{0}'. No peer specified", cInfo.CallId);
                return false;
            }

            ActionMessage msg = msgGen.CreateP2PCreateConferenceAction(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create P2P CreateConference action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            // Set this flag because the action ID in the response
            //   will not match the current state ID of the peer.
            //   This flag instructs the engine to relax that requirement.
            cInfo.WaitingForPeerResponse = true;

            tmQ.PostMessage(msg);
            return true;
        }

        public bool DeletePeerConnection(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            if(cInfo.PeerCallId == 0)
            {
                log.Write(TraceLevel.Error, "Cannot delete peer connection '{0}'. No peer specified", cInfo.CallId);
                return false;
            }

            ActionMessage msg = msgGen.CreateP2PDeleteConnectionAction(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create P2P DeleteConnection action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            // Set this flag because the action ID in the response
            //   will not match the current state ID of the peer.
            //   This flag instructs the engine to relax that requirement.
            cInfo.WaitingForPeerResponse = true;

            tmQ.PostMessage(msg);
            return true;
        }

        public bool AcceptPeerCall(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            if(cInfo.PeerCallId == 0)
            {
                log.Write(TraceLevel.Error, "Cannot accept peer call '{0}'. No peer specified", cInfo.CallId);
                return false;
            }

            ActionMessage msg = msgGen.CreateP2PAcceptCallAction(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create P2P AcceptCall action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            // Set this flag because the action ID in the response
            //   will not match the current state ID of the peer.
            //   This flag instructs the engine to relax that requirement.
            cInfo.WaitingForPeerResponse = true;

            tmQ.PostMessage(msg);
            return true;
        }

        public bool AnswerPeerCall(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            if(cInfo.PeerCallId == 0)
            {
                log.Write(TraceLevel.Error, "Cannot answer peer call '{0}'. No peer specified", cInfo.CallId);
                return false;
            }

            ActionMessage msg = msgGen.CreateP2PAnswerCallAction(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create P2P AnswerCall action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            cInfo.WaitingForPeerResponse = true;

            tmQ.PostMessage(msg);
            return true;
        }

        public bool HoldPeerCall(CallInfo cInfo, CallInfo peerInfo)
        {
            FinalizeAction(cInfo);

            if(cInfo.PeerCallId == 0)
            {
                log.Write(TraceLevel.Error, "Cannot hold peer call '{0}'. No peer specified", cInfo.CallId);
                return false;
            }

            ActionMessage msg = msgGen.CreateP2PHoldAction(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create P2P Hold action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            cInfo.WaitingForPeerResponse = true;

            tmQ.PostMessage(msg);
            return true;
        }

        public bool ResumePeerCall(CallInfo cInfo, CallInfo peerInfo)
        {
            FinalizeAction(cInfo);

            if(cInfo.PeerCallId == 0)
            {
                log.Write(TraceLevel.Error, "Cannot resume peer call '{0}'. No peer specified", cInfo.CallId);
                return false;
            }

            ActionMessage msg = msgGen.CreateP2PResumeAction(cInfo);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create P2P Resume action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            cInfo.WaitingForPeerResponse = true;

            tmQ.PostMessage(msg);
            return true;
        }

        public bool HangupPeerCall(CallInfo cInfo, CallInfo peerInfo)
        {
            FinalizeAction(cInfo);

            cInfo.WaitingForPeerResponse = true;

            if(cInfo.PeerCallId == 0 || peerInfo == null || peerInfo.CallTerminated)
            {
                // Post phoney success response to self
                tmQ.PostMessage(msgGen.CreateResponse(cInfo, true, ICallControl.Actions.HANGUP, cInfo.CallId));
                return true;
            }

            ActionMessage msg = msgGen.CreateHangupAction(cInfo, false, true);
            if(msg == null)
            {
                log.Write(TraceLevel.Error, "[{0}:{1}] Not enough information available to create P2P Hangup action.",
                    cInfo.MapName, cInfo.CurrStateId.ToString());
                return false;
            }

            tmQ.PostMessage(msg);
            return true;
        }

        internal bool ClearPeerMediaInfo(CallInfo cInfo, CallInfo peerInfo)
        {
            FinalizeAction(cInfo);

            if(cInfo.PeerCallId == 0 || peerInfo == null || peerInfo.CallTerminated)
            {
                // Post phoney failure response to self
                tmQ.PostMessage(msgGen.CreateResponse(cInfo, false, Actions.ClearPeerMediaInfo.ToString(), cInfo.CallId));
                return true;
            }

            peerInfo.ClearMediaInfo();

            tmQ.PostMessage(msgGen.CreateResponse(cInfo, true, Actions.ClearPeerMediaInfo.ToString(), cInfo.CallId));
            return true;
        }

        #endregion

        #region Internal Actions (no messages sent)

        public bool ClearMediaInfo(CallInfo cInfo)
        {
            FinalizeAction(cInfo);

            cInfo.ClearMediaInfo();
            return true;
        }

        public bool SyncPeerMedia(CallInfo cInfo, CallInfo peerInfo, bool internalCall)
        {
            if(!internalCall)
                FinalizeAction(cInfo);

            if(peerInfo == null)
            {
                log.Write(TraceLevel.Error, "Cannot link peer media for call '{0}'. Peer call ID '{1}' is invalid",
                    cInfo.CallId, cInfo.PeerCallId);
                return false;
            }

            // Ensure that peer call is linked to this call
            peerInfo.PeerCallId = cInfo.CallId;

            // Link for DTMF proxying
            cInfo.DtmfCallId = peerInfo.CallId;
            peerInfo.DtmfCallId = cInfo.CallId;

            // Exchange receive address
            cInfo.RxAddr = peerInfo.TxAddr;
            cInfo.RxControlAddr = peerInfo.TxControlAddr;
            peerInfo.RxAddr = cInfo.TxAddr;
            peerInfo.RxControlAddr = cInfo.TxControlAddr;

            // Exchange transmit codec
            cInfo.TxCodec = peerInfo.RxCodec;
            cInfo.TxFramesize = peerInfo.RxFramesize;
            peerInfo.TxCodec = cInfo.RxCodec;
            peerInfo.TxFramesize = cInfo.RxFramesize;

            // Caps
            if(peerInfo.RemoteMediaCaps != null)
                cInfo.LocalMediaCaps = peerInfo.RemoteMediaCaps;
            else if(peerInfo.RxCodec != IMediaControl.Codecs.Unspecified)
                cInfo.LocalMediaCaps = new MediaCapsField(peerInfo.RxCodec, peerInfo.RxFramesize);

            if(cInfo.RemoteMediaCaps != null)
                peerInfo.LocalMediaCaps = cInfo.RemoteMediaCaps;
            else if(cInfo.RxCodec != IMediaControl.Codecs.Unspecified)
                peerInfo.LocalMediaCaps = new MediaCapsField(cInfo.RxCodec, cInfo.RxFramesize);

            return true;
        }

        #endregion

        public void FinalizeAction(CallInfo cInfo)
        {
            if(log.LogLevel == TraceLevel.Verbose)
            {
                string shortRoutingGuid = cInfo.RoutingGuid.Substring(24, 12);
                if(cInfo.CallIdSpecified == false)
                {
                    log.Write(TraceLevel.Verbose, "Executing state: {0}:{1} ({2})", 
                        shortRoutingGuid, cInfo.CurrStateId, cInfo.CurrentState.action.ToString());
                }
                else
                {
                    log.Write(TraceLevel.Verbose, "Executing state: {0}:{1}:{2} ({3})", 
                        shortRoutingGuid, cInfo.CallId, cInfo.CurrStateId, cInfo.CurrentState.action.ToString());
                }
            }

            cInfo.StateHistory.Push(cInfo.CurrStateId);
        }
	}
}
