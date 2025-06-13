using System;
using System.Configuration;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace Metreos.Messaging
{
    /// <summary>
    /// A message queue provider that uses a System.Collections.Queue as its data store.
    /// </summary>
    public class MessageQueue : IDisposable
    {
        private readonly Queue queue;
        private readonly ManualResetEvent messageReady;

        public MessageQueue()
        {
            this.queue = new Queue();   // Note, queue is not synchronized because we lock on SyncRoot when we access it
            this.messageReady = new ManualResetEvent(false);
        }

        // Place a message onto this queue
        public void Send(InternalMessage message)
        {
            lock(queue.SyncRoot)
            {
                queue.Enqueue(message);
            }

            messageReady.Set();
        }


        // Receive a message from this queue
        public bool Receive(System.TimeSpan timeout, out InternalMessage message)
        {
            message = null;
            bool gotMessage = messageReady.WaitOne(timeout, false);

            if(gotMessage == true)
            {
                lock(queue.SyncRoot)
                {
                    if(queue.Count != 0) // Check for possible race condition with Purge()
                        message = (InternalMessage) queue.Dequeue();
                   
                    if(queue.Count == 0) // No more messages? Then reset messageReady.
                        messageReady.Reset();
                }
            }

            return message == null ? false : true;
        }

        public MessageQueueWriter GetWriter()
        {
            return new MessageQueueWriter(this);
        }

        public void Purge()
        {
            lock(queue.SyncRoot)
            {
                queue.Clear();
                messageReady.Reset(); // Reset messageReady inside of the lock to make sure we don't have a race w/ Send
            }
        }

        public void Dispose()
        {
            Purge();
        }
	}
}
