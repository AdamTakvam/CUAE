using System;

namespace TestApp
{
    public class MessageQueueWriter : MarshalByRefObject
    {
        private IMessageQueueProvider queue;

        public MessageQueueWriter(IMessageQueueProvider provider)
        {
            queue = provider;
        }

        public void PostMessage(InternalMessage im)
        {
            queue.Send(im);
        }

        public void Cleanup()
        {
            queue.ReleaseResources();
        }

        #region MarshalByRefObject

        public override object InitializeLifetimeService()
        {
            return null;
        }

        #endregion
    }
}
