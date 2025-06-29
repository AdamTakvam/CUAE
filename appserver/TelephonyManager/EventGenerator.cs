using System;

using Metreos.Messaging;
using Metreos.Interfaces;

namespace Metreos.AppServer.TelephonyManager
{
	public class AppEventGenerator
	{
        MessageUtility msgUtil;

        public AppEventGenerator()
        {
            msgUtil = new MessageUtility(IConfig.CoreComponentNames.TEL_MANAGER, IConfig.ComponentType.Provider, null);
        }

        public EventMessage CreateIncomingCall(string routingGuid, string callId)
        {
            if((routingGuid == null) || (callId == null)) { return null; }

            EventMessage evtMsg = 
                msgUtil.CreateEventMessage(ICallControl.Events.INCOMING_CALL, EventMessage.EventType.Triggering, routingGuid);
            evtMsg.AddField(ICallControl.Fields.CALL_ID, callId);
            return evtMsg;
        }

        public EventMessage CreateHangup(string routingGuid, string callId)
        {
            if((routingGuid == null) || (callId == null)) { return null; }

            EventMessage evtMsg = 
                msgUtil.CreateEventMessage(ICallControl.Events.HANGUP, EventMessage.EventType.NonTriggering, routingGuid);
            evtMsg.AddField(ICallControl.Fields.CALL_ID, callId);
            return evtMsg;
        }
	}
}
