using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace TestApp
{
    public class SimpleMessageQueueProvider : IMessageQueueProvider
    {
        private Queue queue;
        private ManualResetEvent messageReady;

        public SimpleMessageQueueProvider()
        {
            queue = new Queue();
            messageReady = new ManualResetEvent(false);
        }


        public void Send(InternalMessage message)
        {
            lock(queue.SyncRoot)
            { 
                queue.Enqueue(message);
                messageReady.Set();
            }
        }


        public bool Receive(System.TimeSpan timeout, out InternalMessage message)
        {
            bool gotMessage = messageReady.WaitOne(timeout, false);

            if(gotMessage == true)
            {
                lock(queue.SyncRoot)
                {
                    message = (InternalMessage)queue.Dequeue();
                    
                    if(queue.Count == 0)
                    {
                        messageReady.Reset();
                    }
                }
            }
            else
            {
                message = null;
            }

            return gotMessage;
        }


        public string GetQueueId()
        {
            return "";
        }


        public void Purge()
        {
            lock(queue.SyncRoot)
            {
                queue.Clear();
                messageReady.Reset();
            }
        }


        public void ReleaseResources()
        {}


        public void Delete()
        {}


        public MessageQueueWriter GetMessageQueueWriter()
        {
            return new MessageQueueWriter(this);
        }
	}
}
