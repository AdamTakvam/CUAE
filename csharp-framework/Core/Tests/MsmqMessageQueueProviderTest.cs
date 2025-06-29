using System;

namespace Metreos.Samoa.Core.Tests
{
    /// <summary>
    /// Unit tests for the Microsoft Message Queue provider,
    /// Metreos.Samoa.Core.MsmqMessageQueueProvider.
    /// </summary>
    public class MsmqMessageQueueProviderTest
    {
        public MsmqMessageQueueProviderTest()
        {}

        /// <summary>
        /// Test the basic send and receive functionality. Verify 
        /// sent messages are equal to received messages.
        /// </summary>
        [csUnit.Test]
        public void testSendAndReceive()
        {
            MessageQueue queue = new MessageQueue(new MsmqMessageQueueProvider("testSendAndReceive"));

            InternalMessage m = new InternalMessage();
            
            m.MessageId = "id";
            m.Source = "source";
            m.AddField(new Field("name", "value"));

            queue.Send(m);

            MessageQueue queue2 = new MessageQueue(new MsmqMessageQueueProvider("testSendAndReceive"));

            InternalMessage m2;

            queue2.Receive(out m2);

            csUnit.Assert.True(m.MessageId == m2.MessageId);
            csUnit.Assert.True(m.Source == m2.Source);
            csUnit.Assert.True(m2.Fields.GetLength(0) == 1);
            csUnit.Assert.True(m.Fields[0].Name == m2.Fields[0].Name);
            csUnit.Assert.True(m.Fields[0].Value == m2.Fields[0].Value);
            csUnit.Assert.True(m2.Fields[0].Value == "value");

            queue.Cleanup();

            m = null;
            m2 = null;
        }

        /// <summary>
        /// Test sending and receiving a modest number of messages (500). Verify
        /// all messages are equal to what was being sent.
        /// </summary>
        [csUnit.Test]
        public void testMultipleSendAndReceive()
        {
            MessageQueue queue = new MessageQueue(new MsmqMessageQueueProvider("testMultipleSendAndReceive"));

            InternalMessage[] m = new InternalMessage[500];
            InternalMessage m2;

            for(int i = 0; i < 500; i++)
            {
                m[i] = new InternalMessage();
                m[i].MessageId = i.ToString();
                m[i].Source = "source" + i.ToString();
                m[i].AddField(new Field("name1", "value1"));
                m[i].AddField(new Field("name2", "value2"));
                m[i].AddField(new Field("name3", "value3"));
            }

            for(int i = 0; i < 500; i++)
            {
                queue.Send(m[i]);
            }

            for(int i = 0; i < 500; i++)
            {
                queue.Receive(out m2);

                csUnit.Assert.True(m[i].MessageId == m2.MessageId);
                csUnit.Assert.True(m[i].Source == m2.Source);
                csUnit.Assert.True(m[i].Fields.GetLength(0) == m2.Fields.GetLength(0));
                csUnit.Assert.True(m[i].Fields[0].Name == m2.Fields[0].Name);
                csUnit.Assert.True(m[i].Fields[0].Value == m2.Fields[0].Value);
                csUnit.Assert.True(m[i].Fields[1].Name == m2.Fields[1].Name);
                csUnit.Assert.True(m[i].Fields[1].Value == m2.Fields[1].Value);
                csUnit.Assert.True(m[i].Fields[2].Name == m2.Fields[2].Name);
                csUnit.Assert.True(m[i].Fields[2].Value == m2.Fields[2].Value);
            }

            queue.Cleanup();
        }

        [csUnit.Ignore("Not a test fixture")]
        private sealed class ProducerConsumerScenarioHelper
        {
            private Core.MessageQueue producerQ;
            private Core.MessageQueue consumerQ;

            public System.Threading.ManualResetEvent producerDone;
            public System.Threading.ManualResetEvent consumerDone;
            
            public int messagesRead;
            public int messagesWritten;
            
            public ProducerConsumerScenarioHelper()
            {
                messagesRead = messagesWritten = 0;

                producerQ = new Core.MessageQueue(new Core.MsmqMessageQueueProvider("ProdConsTestQ"));
                consumerQ = new Core.MessageQueue(new Core.MsmqMessageQueueProvider("ProdConsTestQ"));
            }

            public void Producer(Object state)
            {
                InternalMessage im;
                int numMessagesToSend = (int)state;

                // Produce and send to the queue.
                for(int i = 0; i < numMessagesToSend; i++)
                {
                    im = new InternalMessage();

                    im.MessageId = i.ToString();
                    im.Source = "Metreos.ProducerConsumerTestScenario";
                    
                    im.AddField(new Core.Field("name", i.ToString()));
                   
                    producerQ.Send(im);

                    messagesWritten++;
                }
   
                producerDone.Set();
            }

            public void Consumer(Object state)
            {
                Core.InternalMessage im;

                // Read messages from the queue. We're blocking for approximately
                // 100 milliseconds on each receive to give the producer time to 
                // churn the messages out.
                while(consumerQ.Receive(new System.TimeSpan(0, 0, 0, 0, 50), out im))
                {
                    messagesRead++;
                }

                consumerDone.Set();
            }

            public void Cleanup()
            {
                producerQ.Cleanup();
                consumerQ.Cleanup();
            }
        }

        
        /// <summary>
        /// Test scenario that will spawn two threads to read and write from the same
        /// message queue.
        /// </summary>
        [csUnit.Test]
        public void testProducerConsumerScenario()
        {
            int numMsgs = 1000;

            ProducerConsumerScenarioHelper pc = new ProducerConsumerScenarioHelper();

            System.Threading.ManualResetEvent producerDone = new System.Threading.ManualResetEvent(false);
            System.Threading.ManualResetEvent consumerDone = new System.Threading.ManualResetEvent(false);

            // The events we're going to watch for completion.
            pc.producerDone = producerDone;
            pc.consumerDone = consumerDone;

            // Spool up our work items on the CLR thread pool.
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(pc.Producer), numMsgs);
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(pc.Consumer));

            // Wait for the finished events to be fired by each queue. These will timeout after
            // approximately 10 seconds each.
            producerDone.WaitOne(new System.TimeSpan(0, 0, 10), true);
            consumerDone.WaitOne(new System.TimeSpan(0, 0, 10), true);

            csUnit.Assert.True(pc.messagesRead > 0);
            csUnit.Assert.True(pc.messagesWritten > 0);
            csUnit.Assert.True(pc.messagesRead == pc.messagesWritten);

            pc.Cleanup();
        }

        [csUnit.Test]
        public void testCreateQueueThatAlreadyExists()
        {
            Core.MessageQueue q1 = new Core.MessageQueue(new Core.MsmqMessageQueueProvider("testCreateQueueThatAlreadyExists"));
            Core.MessageQueue q2 = new Core.MessageQueue(new Core.MsmqMessageQueueProvider("testCreateQueueThatAlreadyExists"));

            Core.InternalMessage im = new Core.InternalMessage();
            im.MessageId = "Blah";

            for(int i = 0; i < 50; i++)
            {
                q1.Send(im);
                csUnit.Assert.True(q2.Receive(out im));
            }

            Core.MessageQueue q3 = new Core.MessageQueue(new Core.MsmqMessageQueueProvider("testCreateQueueThatAlreadyExists"));

            for(int i = 0; i < 50; i++)
            {
                q1.Send(im);
                csUnit.Assert.True(q3.Receive(out im));
            }

            q1.Cleanup();
            q2.Cleanup();
            q3.Cleanup();

            q1 = q2 = q3 = null;
            im = null;
        }

        [csUnit.Test]
        public void testReleaseResources()
        {
            Core.MessageQueue q1 = new Core.MessageQueue(new Core.MsmqMessageQueueProvider("testReleaseResources"));
            Core.MessageQueue q2 = new Core.MessageQueue(new Core.MsmqMessageQueueProvider("testReleaseResources"));

            Core.InternalMessage im = new Core.InternalMessage();
            im.MessageId = "testReleaseResources";
            im.Source = "blah";

            q1.Send(im);

            q1.ReleaseResources();

            im = null;

            csUnit.Assert.True(q2.Receive(out im));
            csUnit.Assert.Equals("testReleaseResources", im.MessageId);

            q2.ReleaseResources();
            q2.Cleanup();
        }

        [csUnit.Test]
        public void testReleaseResourcesAfterCleanup()
        {
            bool caughtException = true;

            Core.MessageQueue q1 = new Core.MessageQueue(new Core.MsmqMessageQueueProvider("testReleaseResourcesAfterCleanup"));
            Core.MessageQueue q2 = new Core.MessageQueue(new Core.MsmqMessageQueueProvider("testReleaseResourcesAfterCleanup"));

            try
            {
                q1.Cleanup();
                q2.ReleaseResources();
            }
            catch(Exception)
            {
                caughtException = false;
            }
            
            csUnit.Assert.True(caughtException);
        }

        [csUnit.Test]
        public void testRemoteQueuePathIsBuiltProperly()
        {
            MsmqMessageQueueProvider qp = new MsmqMessageQueueProvider("127.0.0.1", true, "TestId");

            csUnit.Assert.Equals("FormatName:DIRECT=OS:127.0.0.1\\Private$\\Metreos-TestId", qp.QueuePath);
            csUnit.Assert.True(qp.IsRemoteQueue);

            qp.Delete();
        }

        public void testParseCompleteQueuePath()
        {
            MsmqMessageQueueProvider qp = new MsmqMessageQueueProvider("FormatName:DIRECT=OS:127.0.0.1\\Private$\\Metreos-Blah");

            csUnit.Assert.Equals("FormatName:DIRECT=OS:127.0.0.1\\Private$\\Metreos-Blah", qp.QueuePath);
            csUnit.Assert.True(qp.IsRemoteQueue);

            qp.Delete();

            qp = null;
            qp = new MsmqMessageQueueProvider(".\\Private$\\Metreos-Blah");

            csUnit.Assert.Equals(".\\Private$\\Metreos-Blah", qp.QueuePath);
            csUnit.Assert.False(qp.IsRemoteQueue);

            qp.Delete();
        }
    }
}