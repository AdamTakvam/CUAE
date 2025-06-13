using System;
using System.Messaging;

namespace MsmqPurgeQueues
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    class Class1
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            System.Messaging.MessageQueue[] queues = System.Messaging.MessageQueue.GetPrivateQueuesByMachine(".");

            Console.WriteLine("Deleting {0} queue(s)", queues.Length - 4);

            for(int i = 4; i < queues.Length; i++)
            {
                System.Messaging.MessageQueue.Delete(queues[i].Path);
            }
        }
    }
}
