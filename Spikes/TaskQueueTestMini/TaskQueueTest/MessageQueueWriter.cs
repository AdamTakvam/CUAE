using System;
using System.Diagnostics;

namespace Metreos.Messaging
{
    public sealed class MessageQueueWriter : MarshalByRefObject
    {
        private MessageQueue queue;

        internal MessageQueueWriter(MessageQueue provider)
        {
            this.queue = provider;
        }

        public void PostMessage(InternalMessage im)
        {
            queue.Send(im);
        }

        #region MarshalByRefObject

        public override object InitializeLifetimeService()
        {
            return null;
        }

        #endregion
    }
}
