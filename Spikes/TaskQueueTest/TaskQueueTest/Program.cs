using System;
using System.Threading;
using System.Reflection;
using System.Collections;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.Messaging.MediaCaps;
using Metreos.Utilities;
using Metreos.Configuration;
using Metreos.Utilities.Collections;

namespace TaskQueueTest
{
    class Tester : PrimaryTaskBase
    {
        private const bool UseAppDomain = true;

        static void Main(string[] args)
        {
            Tester t = new Tester(UseAppDomain);

            Console.WriteLine("Test started. Press 'q' to quit.");

            string input = "";

            while(input != "q")
            {
                input = Console.ReadLine();
                Console.WriteLine("Elapsed time: " + t.TestTime);
            }

            t.Stop();
        }

        private const int NumSenders = 10;

        private readonly ThreadInfo[] senderThreads;
        private readonly PrimaryTask receiver;
        private readonly MessageQueueWriter receiverQ;
        private volatile bool shutdown = false;

        private const int MessageDelay  = 25;  //ms

        public const int AlarmProcTime    = 50;    // ms
        private readonly DateTime testStartTime;
        private long numMsgsSent = 0;
        private long numMsgsReceived = 0;
        private long totalProcTime = 0;
        private long procTimeHW = 0;
        private readonly BoundedCollection receivedIds;

        public TimeSpan TestTime { get { return DateTime.Now - testStartTime; } }

        public Tester(bool useAppDomain)
            : base(IConfig.ComponentType.Core, "ParentTask", "", Config.Instance)
        {
            receivedIds = new BoundedCollection(20);

            this.testStartTime = DateTime.Now;

            if(useAppDomain)
            {
                AppDomain childDomain = AppDomain.CreateDomain("PrimaryTask Domain");
                this.receiver = (PrimaryTask) childDomain.CreateInstanceFromAndUnwrap("TaskQueueTest.exe", typeof(PrimaryTask).FullName, true,
                    BindingFlags.CreateInstance, null, new object[] { taskQueue.GetWriter() }, null, null, null);
            }
            else
            {
                this.receiver = new PrimaryTask(taskQueue.GetWriter());
            }

            this.receiverQ = receiver.GetQueueWriter();

            this.senderThreads = new ThreadInfo[NumSenders];

            for(int i=0; i<NumSenders; i++)
            {
                ThreadInfo tInfo = new ThreadInfo();
                tInfo.thread = new Thread(new ParameterizedThreadStart(SendMessages));
                tInfo.thread.IsBackground = true;
                tInfo.thread.Name = "SenderThread: " + i;

                senderThreads[i] = tInfo;

                tInfo.thread.Start(i);

                Thread.Sleep(100);  //stagger
            }
        }

        public void Stop()
        {
            shutdown = true;

            Console.WriteLine("Total test time: {0}", TestTime);
            Console.WriteLine("Messages sent: {0}", numMsgsSent);
            Console.WriteLine("Messages received: {0}", numMsgsReceived);
            Console.WriteLine("Messages completion: {0}%", (numMsgsReceived / (double)numMsgsSent) * 100);

            double avgProcTime = numMsgsReceived > 0 ? (totalProcTime / (double) numMsgsReceived) : 0;
            Console.WriteLine("Avg processing time: {0}ms", avgProcTime);
            Console.WriteLine("Processing time high-water: {0}ms", procTimeHW);

            foreach(ThreadInfo tInfo in senderThreads)
            {
                if(!tInfo.thread.Join(20))
                    tInfo.thread.Abort();
            }  
        }

        private void SendMessages(object state)
        {
            int threadId = Convert.ToInt32(state);
            ThreadInfo tInfo = senderThreads[threadId];

            while(!shutdown)
            {
                MediaCapsField mcf = new MediaCapsField();
                mcf.Add(IMediaControl.Codecs.G711u, 10, 20, 30);
                mcf.Add(IMediaControl.Codecs.G711a, 10, 20, 30);
                mcf.Add(IMediaControl.Codecs.G729, 20, 30, 60);
                mcf.Add(IMediaControl.Codecs.G723, 10, 30, 60);

                lock(tInfo.responseLock)
                {
                    tInfo.currMsgId = Interlocked.Increment(ref numMsgsSent);

                    CommandMessage msg = new CommandMessage();
                    msg.MessageId = tInfo.currMsgId.ToString();
                    msg.AddField("ThreadId", threadId);
                    msg.AddField("Time", HPTimer.Now());
                    msg.AddField("MediaCaps", mcf);

                    receiverQ.PostMessage(msg);

                    if(!Monitor.Wait(tInfo.responseLock, 1000))
                        Console.WriteLine("Timed out waiting for response. (thread={0}, msg={1})", threadId, tInfo.currMsgId);
                }
            }
        }

        protected override bool HandleMessage(InternalMessage message)
        {
            numMsgsReceived++;

            long sendTime = Convert.ToInt64(message["Time"]);
            long procTime = HPTimer.MillisSince(sendTime);
            totalProcTime += procTime;

            if(procTime > procTimeHW)
                procTimeHW = procTime;

            if(procTime > AlarmProcTime)
            {
                Console.WriteLine("{0} Time to process message: {1}ms", DateTime.Now, procTime);
            }

            int threadId = Convert.ToInt32(message["ThreadId"]);
            ThreadInfo tInfo = senderThreads[threadId];
            if(tInfo == null)
            {
                Console.WriteLine("Received message with invalid thread ID: " + message);
                return true;
            }

            lock(tInfo.responseLock)
            {
                long msgId = Convert.ToInt64(message.MessageId);
                if(msgId == tInfo.currMsgId)
                    Monitor.Pulse(tInfo.responseLock);
                else
                    Console.WriteLine("Received response out of order. Expected {0}: {1}", tInfo.currMsgId, message);
            }

            return true;
        }

        protected override void RefreshConfiguration(string proxy)
        { }

        protected override void OnStartup()
        { }

        protected override void OnShutdown()
        { }

        public MessageQueue GetQueueWriter()
        {
            return this.taskQueue;
        }

        internal class ThreadInfo
        {
            public long currMsgId;
            public readonly object responseLock;

            public Thread thread;

            public ThreadInfo()
            {
                this.responseLock = new object();
            }
        }
    }
}
