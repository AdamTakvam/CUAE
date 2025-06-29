using System;
using System.Diagnostics;

using Metreos.Interfaces;
using Metreos.Utilities;

namespace Metreos.Messaging
{
	/// <summary>
	/// Helps create well-formed InternalMessage objects
	/// </summary>
	public class MessageUtility
	{
        private string name;
        private IConfig.ComponentType type;
        private MessageQueueWriter queueWriter;

		public MessageUtility(string name, IConfig.ComponentType type, MessageQueueWriter queueWriter)
		{
            if(name == null)
            {
                throw new ArgumentException("Cannot create MessageUtility with no source name");
            }

            this.name = name;
            this.type = type;
            this.queueWriter = queueWriter;
		}

        /// <summary>
        /// Creates an object used to carry an event through the application server
        /// </summary>
        /// <param name="eventName">Fully-qualified name of the event</param>
        /// <returns>An InternalMessage-derived object which can be used in a PostMessage() method</returns>
        public EventMessage CreateEventMessage(string eventName, EventMessage.EventType eventType, string routingGuid)
        {
            EventMessage msg = new EventMessage(eventType, routingGuid);
            return PopulateInternalMessage(msg, IConfig.CoreComponentNames.ROUTER, eventName) as EventMessage;
        }

        /// <summary>
        /// For internal use only
        /// </summary>
        public ActionMessage CreateActionMessage(string actionName, string actionGuid)
        {
            return CreateActionMessage(actionName, actionGuid, String.Empty, String.Empty, String.Empty);
        }

        /// <summary>
        /// For internal use only
        /// </summary>
        public ActionMessage CreateActionMessage(string actionName, string actionGuid, string appName, string scriptName, string partitionName)
        {
            Debug.Assert(actionName != null, "Cannot create action with null action name");
            Debug.Assert(actionGuid != null, "Cannot create action with null action GUID");

            ActionMessage msg = new ActionMessage(actionGuid);
            msg = PopulateInternalMessage(msg, Namespace.GetNamespace(actionName), actionName) as ActionMessage;
            msg.AppName = appName;
            msg.ScriptName = scriptName;
            msg.PartitionName = partitionName;
            return msg;
        }

        /// <summary>
        /// For internal use only
        /// </summary>
        public CommandMessage CreateCommandMessage(string to, string command)
        {
            CommandMessage msg = new CommandMessage();
            return PopulateInternalMessage(msg, to, command) as CommandMessage;
        }

        private InternalMessage PopulateInternalMessage(InternalMessage msg, string to, string messageId)
        {
            msg.Destination = to;
            msg.MessageId = messageId;
            msg.Source = this.name;
            msg.SourceType = this.type;
            msg.SourceQueue = this.queueWriter;

            return msg;
        }
	}
}
