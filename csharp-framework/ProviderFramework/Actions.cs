using System;
using System.Collections;

using Metreos.Utilities;
using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;

namespace Metreos.ProviderFramework
{
	public abstract class ActionBase
	{
        protected ActionMessage actionMsg;
        protected string providerName;

        public string Name { get { return actionMsg.MessageId; } }
        
        public string RoutingGuid { get { return ActionGuid.GetRoutingGuid(actionMsg.ActionGuid); } }

        public string Guid { get { return actionMsg.ActionGuid; } }
        
        public ActionMessage InnerMessage { get { return actionMsg; } }

		protected ActionBase(string providerName, ActionMessage actionMsg)
	    {
            if(providerName == null)
            {
                throw new ArgumentException("Argument cannot be null", "providerName");
            }
            if(actionMsg == null)
            {
                throw new ArgumentException("Argument cannot be null", "actionMsg");
            }

            if(!actionMsg.IsComplete)
            {
                throw new ArgumentException("Action message is incomplete", "actionMsg");
            }

            this.actionMsg = actionMsg;
            this.providerName = providerName;
		}

        public void SendResponse(bool success)
        {
            ArrayList dummy = null;
            SendResponse(success, dummy);
        }

        public void SendResponse(bool success, Field field)
        {
            ArrayList fields = new ArrayList();
            if(field == null)
            {
                SendResponse(success, fields);
            }
            else
            {
                fields.Add(field);
                SendResponse(success, fields);
            }
        }

        public void SendResponse(bool success, ArrayList fields)
        {
            string response = success ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
            actionMsg.SendResponse(response, fields, true);
        }
	}

    public class SyncAction : ActionBase
    {
        public SyncAction(string providerName, ActionMessage actionMsg)
            : base(providerName, actionMsg)
        {}
    }

    public class AsyncAction : ActionBase
    {
        public AsyncAction(string providerName, ActionMessage actionMsg)
            : base(providerName, actionMsg)
        {
        }

        public EventMessage CreateAsyncCallback(bool asyncSuccess)
        {
            return asyncSuccess ? 
                CreateAsyncCallback(IApp.RESULT_COMPLETE) : 
                CreateAsyncCallback(IApp.RESULT_FAILED);
        }

        public EventMessage CreateAsyncCallback(string asyncResult)
        {
            EventMessage msg = new EventMessage(EventMessage.EventType.AsyncCallback, ActionGuid.GetRoutingGuid(actionMsg.ActionGuid));
            msg.MessageId = String.Format("{0}_{1}", actionMsg.MessageId, asyncResult);
            msg.SourceType = IConfig.ComponentType.Provider;
            msg.Source = providerName;
            msg.UserData = InnerMessage.UserData;
            msg.AppName = InnerMessage.AppName;
            msg.ScriptName = InnerMessage.ScriptName;
            msg.PartitionName = InnerMessage.PartitionName;
            return msg;
        }
    }
}
