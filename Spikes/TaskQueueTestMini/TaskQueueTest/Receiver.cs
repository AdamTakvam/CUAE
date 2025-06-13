using System;
using System.Threading;

using Metreos.Messaging;

namespace TaskQueueTest 
{
    public class Receiver : PrimaryTaskBase
    {
        private readonly MessageQueueWriter parentQ;

        public Receiver(MessageQueueWriter parentQ)
            : base("Receiver")
        {
            this.parentQ = parentQ;
        }

        protected override bool HandleMessage(InternalMessage message)
        {
            parentQ.PostMessage(message);
            return true;
        }

        public MessageQueueWriter GetQueueWriter()
        {
            return taskQueue.GetWriter();
        }
    }
}
