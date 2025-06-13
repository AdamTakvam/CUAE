using System;
using System.Collections;

namespace Metreos.Messaging
{
    [Serializable]
	public class CommandMessage : InternalMessage
	{
		public CommandMessage()
		{
		}

        public bool SendResponse(string response)
        {
            return SendResponse(response, null, false);
        }

        public bool SendResponse(string response, ArrayList fields, bool cleanupQueueWriter)
        {
            ResponseMessage msg = new ResponseMessage(null);
            msg.InResponseTo = this.MessageId;
            msg.Destination = this.Source;
            msg.MessageId = response;
            msg.Source = this.Destination;

            // Preserve original command fields for correlation purposes
            foreach(Field field in base.Fields)
            {
                msg.AddField(field.Name, field.Value);
            }

            // Add additional response fields
            if(fields != null)
            {
                foreach(Field field in fields)
                {
                    msg.AddField(field.Name, field.Value);
                }
            }

            try
            {
                sourceQueue.PostMessage(msg);
            }
            catch(Exception)
            {
                return false;
            }

            if(cleanupQueueWriter)
            {
                sourceQueue.Dispose();
                sourceQueue = null;
            }

            return true;
        }

        public override string ToString()
        {
            return ToString("CommandMessage", null);
        }
	}
}
