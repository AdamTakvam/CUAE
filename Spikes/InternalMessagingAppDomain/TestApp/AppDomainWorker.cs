using System;
using System.Collections;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestApp
{
	public class AppDomainWorker : MarshalByRefObject
	{
        private SimpleMessageQueueProvider queue;
        public Thread t;

        public ManualResetEvent ProcessMessages = new ManualResetEvent(false);
        public ManualResetEvent ThreadDone = new ManualResetEvent(false);

		public AppDomainWorker()
		{
            t = new Thread(new ThreadStart(this.ThreadWorker));
            queue = new SimpleMessageQueueProvider();
		}

        public void Go()
        {
            t.Start();
        }

        public void PostMessage(InternalMessage msg)
        {
            queue.Send(msg);
        }

        public void ThreadWorker()
        {
            bool gotMessage = false;
            InternalMessage msg = new InternalMessage();

            bool finished = false;

            while(finished == false)
            {
                gotMessage = queue.Receive(new TimeSpan(0, 0, 0, 30, 0), out msg);

                if(gotMessage == true)
                {
                    if(msg.MessageId == "QUIT")
                    {
                        msg.MessageId = "QUITACK";
                        msg.sourceQueueWriter.PostMessage(msg);

                        finished = true;
                    }
                    else if(msg.MessageId == "SpeedTest")
                    {
                        msg.MessageId = "SpeedTestAck";
                        msg.sourceQueueWriter.PostMessage(msg);
                    }
                }
            }

            ThreadDone.Set();
        }
	}
}
