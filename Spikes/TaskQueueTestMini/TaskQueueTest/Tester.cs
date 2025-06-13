using System;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using Metreos.Messaging;

namespace TaskQueueTest
{
    public class Tester : PrimaryTaskBase
    {
        public const bool UseAppDomain              = true;
        public const string ShortTimestampFormat    = "HH:mm:ss.fff";
        public const int NumSenders                 = 10;
        public const int MessageDelay               = 25;    // millis
        public const int AlarmProcTime              = 1000;  // millis
       
        #region Main
        public static void Main(string[] args)
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
        #endregion

        private readonly ThreadInfo[] senderThreads;
        private readonly Receiver receiver;
        private readonly MessageQueueWriter receiverQ;
        private readonly DateTime testStartTime;
        private volatile bool shutdown = false;
        private long numMsgsSent     = 0;
        private long numMsgsReceived = 0;
        private long totalProcTime   = 0;

        public TimeSpan TestTime { get { return DateTime.Now - testStartTime; } }

        public Tester(bool useAppDomain)
            : base("Tester")
        {
            this.testStartTime = DateTime.Now;

            if(useAppDomain)
            {
                AppDomain childDomain = AppDomain.CreateDomain("Receiver Domain");
                this.receiver = (Receiver) childDomain.CreateInstanceFromAndUnwrap("TaskQueueTest.exe", typeof(Receiver).FullName, true,
                    BindingFlags.CreateInstance, null, new object[] { taskQueue.GetWriter() }, null, null, null);
            }
            else
            {
                this.receiver = new Receiver(taskQueue.GetWriter());
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
            object[] intArray = new object[] { "string", 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };  // Used in various tests below

            while(!shutdown)
            {
                lock(tInfo.responseLock)
                {
                    tInfo.currMsgId = Interlocked.Increment(ref numMsgsSent);

                    InternalMessage msg = new InternalMessage();
                    msg.MessageId = tInfo.currMsgId.ToString();
                    msg.Time = HPTimer.Now();
                    //msg.AddField("time", HPTimer.Now());
                    msg.ThreadId = threadId;
                    //msg.AddField("threadId", threadId);


                    // --- Bad configurations ---

                    // Bad #1
                    // Uncomment this code to get "bad" behavior
                    //
                    //for(int i = 0; i < 100; i++)
                    //{
                    //    msg.fieldValues.Add(intArray);
                    //}

                    // Bad #2
                    // Uncomment this code to get "bad" behavior
                    //
                    //Hashtable y = new Hashtable();
                    //for(int i=0; i<100; i++)
                    //{
                    //    msg.fieldIndex.Add(i, intArray);
                    //}
                    
                    // Bad #3
                    // Uncomment this code to get "bad" behavior. Sometimes this takes
                    // longer to show the bad behavior. Previously we thought this was ok
                    // but it failed this morning after 3 minutes. We normally let the test
                    // run 2.5 minutes because we almost always see the problem within 1
                    // minute. But this one was faking us out.
                    //
                    // This one seems to crash after about 3 minutes every time.
                    //
                    //for(int i=0; i<100; i++)
                    //{
                    //    msg.AddField(i.ToString(), i); // BAD
                    //}

                    // Bad #4
                    // Uncomment this code to get "bad" behavior. Before, we were reusing
                    // the same object [] and it was not causing the problem. If we make 
                    // a new array each time, the problem happens.
                    //
                    for(int i = 0; i < msg.objArray.Length; i++)
                    {
                        msg.objArray[i] = new object[] { "string", 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    }

                    // --- Good configurations ---
                    // There are no good configurations... :(

                    receiverQ.PostMessage(msg);

                    if(!Monitor.Wait(tInfo.responseLock, 1000))
                        Console.WriteLine("Timed out waiting for response. (thread={0}, msg={1})", threadId, tInfo.currMsgId);
                }
            }
        }

        protected override bool HandleMessage(InternalMessage message)
        {
            numMsgsReceived++;

            int threadId = Convert.ToInt32(message.ThreadId);
            //int threadId = Convert.ToInt32(message.GetField("threadId"));
            ThreadInfo tInfo = senderThreads[threadId];
           
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
