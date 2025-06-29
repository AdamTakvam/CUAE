using System;
using System.Collections;

namespace Metreos.Messaging
{
	public abstract class MessageQueueFactory
	{
        private static Hashtable queues = new Hashtable();

        public static MessageQueueWriter GetQueueWriter(string name)
        {
            MessageQueue queue = null;

            lock(queues.SyncRoot)
            {
                queue = queues[name] as MessageQueue;
            }

            if(queue == null) { return null; }

            return queue.GetWriter();
        }

        public static bool RegisterQueue(string name, MessageQueue queue)
        {
            try
            {
                lock(queues.SyncRoot)
                {
                    queues[name] = queue;
                }
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }
	}
}
