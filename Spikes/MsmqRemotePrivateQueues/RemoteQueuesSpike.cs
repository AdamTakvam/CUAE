using System;
using System.Messaging;

namespace MsmqRemotePrivateQueues
{
    class RemoteQueuesSpike
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Current DateTime is: {0}", System.DateTime.Now.ToString());

            System.Messaging.MessageQueue q = new System.Messaging.MessageQueue();

            string remoteIp;
            string queueName;
            string fullQueuePath;

            Console.Write("Enter the IP address of the remote machine: ");
            remoteIp = Console.ReadLine();

            Console.Write("Enter the queue name to write to: ");
            queueName = Console.ReadLine();

            fullQueuePath = @"FormatName:DIRECT=OS:" + remoteIp + @"\private$\" + queueName;

            Console.WriteLine("Attempting to write a test message to: {0}", fullQueuePath);

            q.Path = fullQueuePath;

            Metreos.Samoa.Core.InternalMessage im = new Metreos.Samoa.Core.InternalMessage();
            im.MessageId = "connect";

            System.Console.WriteLine(im.ToString());

            q.Send(im, System.Messaging.MessageQueueTransactionType.None);

            q.Close();
        }
    }
}
