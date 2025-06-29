using System;
using System.Diagnostics;

using Metreos.Samoa;
using Metreos.Samoa.Interfaces;

namespace Metreos.Samoa.Core.Tests
{
    #region Mock Objects

    public class MockTaskOne : Core.PrimaryTaskBase
    {
        public System.Threading.ManualResetEvent startupCalledEvent;
        public System.Threading.ManualResetEvent shutdownCalledEvent;
        public System.Threading.ManualResetEvent handleMessageCalledEvent;

        public MockTaskOne(string taskName) : base(taskName, TraceLevel.Info)
        {
            this.startupCalledEvent = new System.Threading.ManualResetEvent(false);
            this.shutdownCalledEvent = new System.Threading.ManualResetEvent(false);
            this.handleMessageCalledEvent = new System.Threading.ManualResetEvent(false);
        }

		protected override void RefreshConfiguration() {}

        protected override bool HandleMessage(Core.InternalMessage im)
        {
            switch(im.MessageId)
            {
                case "Core.Test.MockMessage":
                    handleMessageCalledEvent.Set();
                    return true;
            }

            return false;
        }

        protected override void OnStartup()
        {
            startupCalledEvent.Set();
        }

        protected override void OnShutdown()
        {
            shutdownCalledEvent.Set();
        }
    }


    public class MockTaskTwo : Core.PrimaryTaskBase
    {
        public System.Threading.ManualResetEvent startupCalledEvent;

        public MockTaskTwo(string taskName) : base(taskName, TraceLevel.Info)
        {
            this.startupCalledEvent = new System.Threading.ManualResetEvent(false);
        }

		protected override void RefreshConfiguration() {}

        protected override bool HandleMessage(Core.InternalMessage im)
        {
            return false;
        }

        protected override void OnStartup()
        {
            startupCalledEvent.Set();

            throw new Core.StartupFailedException("OnStartup failed");
        }

        protected override void OnShutdown()
        {
        }
    }


    public class MockTaskThree : Core.PrimaryTaskBase
    {
        public System.Threading.ManualResetEvent startupCalledEvent;
        public System.Threading.ManualResetEvent shutdownCalledEvent;

        public MockTaskThree(string taskName) : base(taskName, TraceLevel.Info)
        {
            this.startupCalledEvent = new System.Threading.ManualResetEvent(false);
            this.shutdownCalledEvent = new System.Threading.ManualResetEvent(false);
        }

		protected override void RefreshConfiguration() {}

        public new void ForceShutdown()
        {
            this.shutdownRequested = true;
        }

        protected override bool HandleMessage(Core.InternalMessage im)
        {
            return false;
        }

        protected override void OnStartup()
        {
            startupCalledEvent.Set();
        }

        protected override void OnShutdown()
        {
            shutdownCalledEvent.Set();
            throw new Core.ShutdownFailedException("OnShutdown failed");
        }
    }


    public class MockTaskFour : Core.PrimaryTaskBase
    {
        public System.Threading.ManualResetEvent startupCalledEvent;
        public System.Threading.ManualResetEvent shutdownCalledEvent;

        public MockTaskFour(string taskName) : base(taskName, TraceLevel.Info)
        {
            this.startupCalledEvent = new System.Threading.ManualResetEvent(false);
            this.shutdownCalledEvent = new System.Threading.ManualResetEvent(false);
        }

		protected override void RefreshConfiguration() {}

        protected override bool HandleMessage(Core.InternalMessage im)
        {
            return false;
        }

        protected override void OnStartup()
        {
            startupCalledEvent.Set();
        }

        protected override void OnShutdown()
        {
            shutdownCalledEvent.Set();
        }
    }


    public class MockTaskFive : Core.PrimaryTaskBase
    {
        public System.Threading.ManualResetEvent startupCalledEvent;
        public System.Threading.ManualResetEvent shutdownCalledEvent;

        public bool SignalShutdown
        {
            set { this.shutdownRequested = value; }
        }

        public MockTaskFive(string taskName) : base(taskName, TraceLevel.Info)
        {
            this.autoSignalThreadShutdown = false;

            this.startupCalledEvent = new System.Threading.ManualResetEvent(false);
            this.shutdownCalledEvent = new System.Threading.ManualResetEvent(false);
        }

		protected override void RefreshConfiguration() {}

        protected override bool HandleMessage(Core.InternalMessage im)
        {
            return false;
        }

        protected override void OnStartup()
        {
            startupCalledEvent.Set();
        }

        protected override void OnShutdown()
        {
            shutdownCalledEvent.Set();
        }
    }


    public class MockTaskSix : Core.PrimaryTaskBase
    {
        public System.Threading.ManualResetEvent startupCalledEvent;
        public System.Threading.ManualResetEvent shutdownCalledEvent;

        public bool SignalShutdown
        {
            set { this.shutdownRequested = value; }
        }

        public bool AutoSendStartupComplete
        {
            set { this.autoSendStartupComplete = value; }
        }

        public bool AutoSendShutdownComplete
        {
            set { this.autoSendShutdownComplete = value; }
        }

        public MockTaskSix(string taskName) : base(taskName, TraceLevel.Info)
        {
            this.autoSignalThreadShutdown = false;

            this.startupCalledEvent = new System.Threading.ManualResetEvent(false);
            this.shutdownCalledEvent = new System.Threading.ManualResetEvent(false);
        }

		protected override void RefreshConfiguration() {}

        protected override bool HandleMessage(Core.InternalMessage im)
        {
            return false;
        }

        protected override void OnStartup()
        {
            startupCalledEvent.Set();
        }

        protected override void OnShutdown()
        {
            shutdownCalledEvent.Set();
        }

        public void SendStartupComplete()
        {
            InternalMessage im = new InternalMessage();

            im.MessageId = ITask.MSG_STARTUP_COMPLETE;
            im.Source = this.taskName;

            MessageQueueWriter mqw = new MessageQueueWriter(new MsmqMessageQueueProvider(this.startupReceivedFromQueueId));
            mqw.PostMessage(im);

            mqw.Cleanup();

            im = null;
            mqw = null;
        }

        public void SendShutdownComplete()
        {
            InternalMessage im = new InternalMessage();

            im.MessageId = ITask.MSG_SHUTDOWN_COMPLETE;
            im.Source = this.taskName;

            MessageQueueWriter mqw = new MessageQueueWriter(new MsmqMessageQueueProvider(this.shutdownReceivedFromQueueId));
            mqw.PostMessage(im);

            mqw.Cleanup();

            im = null;
            mqw = null;
        }
    }


    public class TestTaskPurgeQueueMockTask : Metreos.Samoa.Core.PrimaryTaskBase
    {
        public TestTaskPurgeQueueMockTask() : base("TestTaskPurgeQueueMockTask", TraceLevel.Info)
        {
        }

        protected override void Run()
        {
            // do nothing.
        }

		protected override void RefreshConfiguration() {}

        protected override bool HandleMessage(Core.InternalMessage im)
        {
            return false;
        }

        protected override void OnStartup()
        {}

        protected override void OnShutdown()
        {}
    }


    #endregion

    /// <summary>
    /// Unit tests for Metreos.Samoa.Core.PrimaryTaskBase
    /// </summary>
    public class PrimaryTaskBaseTest
    {
        private MockTaskOne task1;
        private MockTaskTwo task2;
        private MockTaskThree task3;
        private MockTaskFour task4;
        private MockTaskFive task5;
        private MockTaskSix task6;

        public PrimaryTaskBaseTest()
        {
            task1 = new MockTaskOne("MockTaskOne");
            task2 = new MockTaskTwo("MockTaskTwo");
            task3 = new MockTaskThree("MockTaskThree");
            task4 = new MockTaskFour("MockTaskFour");
            task5 = new MockTaskFive("MockTaskFive");
            task6 = new MockTaskSix("MockTaskSix");
        }

        [csUnit.FixtureTearDown]
        public void FixtureTearDown()
        {
            if(task1.TaskStatus == Core.PrimaryTaskBase.TaskStatusType.STARTED)
            {
                System.Threading.Thread.Sleep(1000);

                if(task1.TaskStatus == Core.PrimaryTaskBase.TaskStatusType.STARTED)
                {
                    InternalMessage im = new InternalMessage();
                    im.MessageId = ITask.MSG_SHUTDOWN;
                    im.Source = "testCleanup()";
                    task1.PostMessage(im);

                    System.Threading.Thread.Sleep(3000);
                }
            }

            if(task2.TaskStatus == Core.PrimaryTaskBase.TaskStatusType.STARTED)
            {
                System.Threading.Thread.Sleep(1000);

                if(task2.TaskStatus == Core.PrimaryTaskBase.TaskStatusType.STARTED)
                {
                    InternalMessage im = new InternalMessage();
                    im.MessageId = ITask.MSG_SHUTDOWN;
                    im.Source = "testCleanup()";
                    task2.PostMessage(im);

                    System.Threading.Thread.Sleep(3000);
                }
            }
            
            if(task3.TaskStatus == Core.PrimaryTaskBase.TaskStatusType.STARTED)
            {   
                task3.ForceShutdown();
                System.Threading.Thread.Sleep(50);
            }

            if(task4.TaskStatus == Core.PrimaryTaskBase.TaskStatusType.STARTED)
            {
                System.Threading.Thread.Sleep(1000);

                if(task4.TaskStatus == Core.PrimaryTaskBase.TaskStatusType.STARTED)
                {
                    InternalMessage im = new InternalMessage();
                    im.MessageId = ITask.MSG_SHUTDOWN;
                    im.Source = "testCleanup()";
                    task4.PostMessage(im);

                    System.Threading.Thread.Sleep(3000);
                }
            }

            if(task5.TaskStatus == Core.PrimaryTaskBase.TaskStatusType.STARTED)
            {
                System.Threading.Thread.Sleep(1000);

                if(task5.TaskStatus == Core.PrimaryTaskBase.TaskStatusType.STARTED)
                {
                    task5.SignalShutdown = true;

                    System.Threading.Thread.Sleep(3000);
                }
            }

            if(task6.TaskStatus == Core.PrimaryTaskBase.TaskStatusType.STARTED)
            {
                System.Threading.Thread.Sleep(1000);

                if(task6.TaskStatus == Core.PrimaryTaskBase.TaskStatusType.STARTED)
                {
                    task6.SignalShutdown = true;

                    System.Threading.Thread.Sleep(3000);
                }
            }

            task1.Cleanup();
            task2.Cleanup();
            task3.Cleanup();
            task4.Cleanup();
            task5.Cleanup();
            task6.Cleanup();

            task1 = null;
            task2 = null;
            task3 = null;
            task4 = null;
            task5 = null;
            task6 = null;
        }

        public void testTaskStatusOnInstantiation()
        {
            csUnit.Assert.Equals(Core.PrimaryTaskBase.TaskStatusType.SHUTDOWN, task1.TaskStatus);
        }

        public void testTaskPurgeQueue()
        {
            bool messageReceived = false;
            TestTaskPurgeQueueMockTask task = new TestTaskPurgeQueueMockTask();
            Metreos.Samoa.Core.MessageQueue q = new Metreos.Samoa.Core.MessageQueue(
                new MsmqMessageQueueProvider(task.QueueId));
            
            InternalMessage im = new InternalMessage();

            im.MessageId = "Blah";
            im.Source = "testTaskPurgeQueue()";

            q.Send(im);                                     // Fill the queue up with some
            q.Send(im);                                     // messages. These should all go
            q.Send(im);                                     // away when task2 is created.
            q.Send(im);
            q.Send(im);

            TestTaskPurgeQueueMockTask task2 = new TestTaskPurgeQueueMockTask();

            messageReceived = q.Receive(System.TimeSpan.Zero, out im);

            csUnit.Assert.False(messageReceived);
            
            task.Cleanup();
            q.Cleanup();

            task = null;
            task2 = null;
            q = null;
        }

        public void testStartupSuccess()
        {
            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.SHUTDOWN, task1.TaskStatus);

            Core.MessageQueue q = new Core.MessageQueue(new MsmqMessageQueueProvider("testStartupSuccess"));

            bool signalReceived;

            InternalMessage im = new InternalMessage();

            im.MessageId = ITask.MSG_STARTUP;
            im.Source = "testStartupSuccess()";
            im.SourceQueue = "testStartupSuccess";

            task1.PostMessage(im);

            signalReceived = task1.startupCalledEvent.WaitOne(5000, false);

            System.Threading.Thread.Sleep(250);

            im = null;

            csUnit.Assert.True(signalReceived);
            csUnit.Assert.Equals(task1.TaskStatus, Core.PrimaryTaskBase.TaskStatusType.STARTED);

            csUnit.Assert.True(q.Receive(out im));
            csUnit.Assert.Equals(ITask.MSG_STARTUP_COMPLETE, im.MessageId);

            im = null;

            q.Cleanup();
        }

        public void testSetLogLevel()
        {
            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.STARTED, task1.TaskStatus);

            InternalMessage im = new InternalMessage();

            im.MessageId = ITask.MSG_SET_LOG_LEVEL;
            im.Source = "testSetLogLevel";
            im.AddField(new Field(ITask.FIELD_LOG_LEVEL, System.Diagnostics.TraceLevel.Off.ToString()));

            task1.PostMessage(im);

            System.Threading.Thread.Sleep(50);

            csUnit.Assert.Equals(System.Diagnostics.TraceLevel.Off, task1.LogLevel);

            im = null;

            im = new InternalMessage();

            im.MessageId = ITask.MSG_SET_LOG_LEVEL;
            im.Source = "testSetLogLevel";
            im.AddField(new Field(ITask.FIELD_LOG_LEVEL, System.Diagnostics.TraceLevel.Verbose.ToString()));

            task1.PostMessage(im);

            System.Threading.Thread.Sleep(50);

            csUnit.Assert.Equals(System.Diagnostics.TraceLevel.Verbose, task1.LogLevel);

            im = null;

            im = new InternalMessage();

            im.MessageId = ITask.MSG_SET_LOG_LEVEL;
            im.Source = "testSetLogLevel";
            im.AddField(new Field(ITask.FIELD_LOG_LEVEL, System.Diagnostics.TraceLevel.Info.ToString()));

            task1.PostMessage(im);

            System.Threading.Thread.Sleep(50);

            csUnit.Assert.Equals(System.Diagnostics.TraceLevel.Info, task1.LogLevel);

            im = null;

            im = new InternalMessage();

            im.MessageId = ITask.MSG_SET_LOG_LEVEL;
            im.Source = "testSetLogLevel";
            im.AddField(new Field(ITask.FIELD_LOG_LEVEL, System.Diagnostics.TraceLevel.Warning.ToString()));

            task1.PostMessage(im);

            System.Threading.Thread.Sleep(50);

            csUnit.Assert.Equals(System.Diagnostics.TraceLevel.Warning, task1.LogLevel);

            im = null;

            im = new InternalMessage();

            im.MessageId = ITask.MSG_SET_LOG_LEVEL;
            im.Source = "testSetLogLevel";
            im.AddField(new Field(ITask.FIELD_LOG_LEVEL, System.Diagnostics.TraceLevel.Error.ToString()));

            task1.PostMessage(im);

            System.Threading.Thread.Sleep(50);

            csUnit.Assert.Equals(System.Diagnostics.TraceLevel.Error, task1.LogLevel);

            im = null;
        }

        public void testStartupFail()
        {
            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.SHUTDOWN, task2.TaskStatus);

            Core.MessageQueue q = new Core.MessageQueue(new MsmqMessageQueueProvider("testStartupFail"));

            bool signalReceived;

            InternalMessage im = new InternalMessage();

            im.MessageId = ITask.MSG_STARTUP;
            im.Source = "testStartupFail()";
            im.SourceQueue = "testStartupFail";

            task2.PostMessage(im);

            signalReceived = task2.startupCalledEvent.WaitOne(5000, false);

            System.Threading.Thread.Sleep(500);

            im = null;

            string reason;
            
            csUnit.Assert.True(signalReceived);
            csUnit.Assert.Equals(Core.PrimaryTaskBase.TaskStatusType.SHUTDOWN, task2.TaskStatus);

            csUnit.Assert.True(q.Receive(out im));
            csUnit.Assert.Equals(ITask.MSG_STARTUP_FAILED, im.MessageId);
            
            csUnit.Assert.True(im.GetFieldByName(ITask.FIELD_FAIL_REASON, out reason));
            csUnit.Assert.NotEquals("", reason);
            csUnit.Assert.Equals("OnStartup failed", reason);

            im = null;

            q.Cleanup();
        }

        public void testStartupNoSourceQueue()
        {
            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.SHUTDOWN, task4.TaskStatus);

            Core.MessageQueue q = new Core.MessageQueue(new MsmqMessageQueueProvider("testStartupNoSourceQueue"));

            bool signalReceived;

            InternalMessage im = new InternalMessage();

            im.MessageId = ITask.MSG_STARTUP;
            im.Source = "testStartupNoSourceQueue";

            task4.PostMessage(im);

            signalReceived = task4.startupCalledEvent.WaitOne(5000, false);

            System.Threading.Thread.Sleep(250);

            im = null;

            csUnit.Assert.True(signalReceived);
            csUnit.Assert.Equals(task4.TaskStatus, Core.PrimaryTaskBase.TaskStatusType.STARTED);

            csUnit.Assert.False(q.Receive(out im));
            csUnit.Assert.Null(im);

            im = null;

            q.Cleanup();
        }

        public void testHandleMessage()
        {
            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.STARTED, task1.TaskStatus);

            bool signalReceived;

            InternalMessage im = new InternalMessage();

            im.MessageId = "Core.Test.MockMessage";
            im.Source = "testHandleMessage()";

            task1.PostMessage(im);
            
            signalReceived = task1.handleMessageCalledEvent.WaitOne(5000, false);
            
            csUnit.Assert.True(signalReceived);

            im = null;
        }

        public void testShutdownSuccess()
        {
            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.STARTED, task1.TaskStatus);

            Core.MessageQueue q = new Core.MessageQueue(new MsmqMessageQueueProvider("testShutdownSuccess"));

            bool signalReceived;

            InternalMessage im = new InternalMessage();
            
            im.MessageId = ITask.MSG_SHUTDOWN;
            im.Source = "testShutdownSuccess()";
            im.SourceQueue = "testShutdownSuccess";

            task1.PostMessage(im);

            signalReceived = task1.shutdownCalledEvent.WaitOne(5000, false);

            System.Threading.Thread.Sleep(250);

            im = null;        

            csUnit.Assert.True(signalReceived);
            csUnit.Assert.Equals(task1.TaskStatus, Core.PrimaryTaskBase.TaskStatusType.SHUTDOWN);

            csUnit.Assert.True(q.Receive(out im));
            csUnit.Assert.Equals(ITask.MSG_SHUTDOWN_COMPLETE, im.MessageId);

            im = null;

            q.Cleanup();
        }

        public void testShutdownFail()
        {
            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.SHUTDOWN, task2.TaskStatus);

            Core.MessageQueue q = new Core.MessageQueue(new MsmqMessageQueueProvider("testShutdownFail"));

            bool signalReceived;

            InternalMessage im = new InternalMessage();

            im.MessageId = ITask.MSG_STARTUP;
            im.Source = "testShutdownFail()";

            task3.PostMessage(im);

            signalReceived = task3.startupCalledEvent.WaitOne(5000, false);

            csUnit.Assert.True(signalReceived);

            im = null;
            signalReceived = false;

            im = new InternalMessage();

            im.MessageId = ITask.MSG_SHUTDOWN;
            im.Source = "testShutdownFail()";
            im.SourceQueue = "testShutdownFail";

            task3.PostMessage(im);

            signalReceived = task3.shutdownCalledEvent.WaitOne(5000, false);

            System.Threading.Thread.Sleep(500);

            im = null;

            string reason;
            
            csUnit.Assert.True(signalReceived);
            csUnit.Assert.Equals(Core.PrimaryTaskBase.TaskStatusType.STARTED, task3.TaskStatus);

            csUnit.Assert.True(q.Receive(out im));
            csUnit.Assert.Equals(ITask.MSG_SHUTDOWN_FAILED, im.MessageId);
            
            csUnit.Assert.True(im.GetFieldByName(ITask.FIELD_FAIL_REASON, out reason));
            csUnit.Assert.NotEquals("", reason);
            csUnit.Assert.Equals("OnShutdown failed", reason);

            im = null;
            reason = null;

            q.Cleanup();
        }

        public void testShutdownNoSourceQueue()
        {
            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.STARTED, task4.TaskStatus);

            Core.MessageQueue q = new Core.MessageQueue(new MsmqMessageQueueProvider("testShutdownNoSourceQueue"));

            bool signalReceived;

            InternalMessage im = new InternalMessage();
            
            im.MessageId = ITask.MSG_SHUTDOWN;
            im.Source = "testShutdownNoSourceQueue";

            task4.PostMessage(im);

            signalReceived = task4.shutdownCalledEvent.WaitOne(5000, false);

            System.Threading.Thread.Sleep(250);

            im = null;        

            csUnit.Assert.True(signalReceived);
            csUnit.Assert.Equals(task4.TaskStatus, Core.PrimaryTaskBase.TaskStatusType.SHUTDOWN);

            csUnit.Assert.False(q.Receive(out im));
            csUnit.Assert.Null(im);

            im = null;

            q.Cleanup();
        }

        public void testAutoSignalShutdown()
        {
            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.SHUTDOWN, task5.TaskStatus);
            csUnit.Assert.True(task5.IsThreadAlive);
                
            InternalMessage im = new InternalMessage();

            im.MessageId = ITask.MSG_STARTUP;

            task5.PostMessage(im);

            System.Threading.Thread.Sleep(50);

            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.STARTED, task5.TaskStatus);
            csUnit.Assert.True(task5.IsThreadAlive);

            im.MessageId = ITask.MSG_SHUTDOWN;

            task5.PostMessage(im);

            System.Threading.Thread.Sleep(50);

            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.SHUTDOWN, task5.TaskStatus);
            csUnit.Assert.True(task5.IsThreadAlive);

            task5.SignalShutdown = true;

            task5.PostMessage(im);                          // This is just to pull it out of its Receive() wait.
            System.Threading.Thread.Sleep(50);

            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.SHUTDOWN, task5.TaskStatus);
            csUnit.Assert.False(task5.IsThreadAlive);
        }

        [csUnit.Test]
        public void testAutoSendStartupComplete()
        {
            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.SHUTDOWN, task6.TaskStatus);
            csUnit.Assert.True(task6.IsThreadAlive);

            task6.AutoSendStartupComplete = false;
            task6.AutoSendShutdownComplete = true;

            MessageQueue q = new MessageQueue(new MsmqMessageQueueProvider("testAutoSendStartupComplete"));

            InternalMessage im = new InternalMessage();

            im.MessageId = ITask.MSG_STARTUP;
            im.Source = "testAutoSendStartupComplete";
            im.SourceQueue = "testAutoSendStartupComplete";

            task6.PostMessage(im);

            System.Threading.Thread.Sleep(50);

            im = null;

            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.STARTED, task6.TaskStatus);
            csUnit.Assert.True(task6.IsThreadAlive);
            csUnit.Assert.False(q.Receive(out im));
            csUnit.Assert.Null(im);

            task6.SendStartupComplete();

            System.Threading.Thread.Sleep(50);

            csUnit.Assert.True(q.Receive(out im));
            csUnit.Assert.NotNull(im);
            csUnit.Assert.Equals(task6.Name, im.Source);
            csUnit.Assert.Equals(ITask.MSG_STARTUP_COMPLETE, im.MessageId);

            im = new InternalMessage();

            im.MessageId = ITask.MSG_SHUTDOWN;
            im.Source = "testAutoSendStartupComplete";
            im.SourceQueue = "testAutoSendStartupComplete";

            task6.PostMessage(im);

            System.Threading.Thread.Sleep(250);

            im = null;

            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.SHUTDOWN, task6.TaskStatus);
            csUnit.Assert.True(task6.IsThreadAlive);
            csUnit.Assert.True(q.Receive(out im));
            csUnit.Assert.NotNull(im);
            csUnit.Assert.Equals(ITask.MSG_SHUTDOWN_COMPLETE, im.MessageId);

            task6.SignalShutdown = true;
            task6.PostMessage(im);                          // This is just to pull it out of its Receive() wait.

            System.Threading.Thread.Sleep(50);

            csUnit.Assert.False(task6.IsThreadAlive);

            q.Cleanup();
        }

        [csUnit.Test]
        public void testAutoSendShutdownComplete()
        {
            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.SHUTDOWN, task6.TaskStatus);
            
            if(task6.IsThreadAlive == false)
            {
                task6.Cleanup();

                task6 = null;

                task6 = new MockTaskSix("testAutoSendShutdownComplete-MockTaskSix");
            }

            task6.AutoSendStartupComplete = true;
            task6.AutoSendShutdownComplete = false;

            MessageQueue q = new MessageQueue(new MsmqMessageQueueProvider("testAutoSendShutdownComplete"));

            InternalMessage im = new InternalMessage();

            im.MessageId = ITask.MSG_STARTUP;
            im.Source = "testAutoSendShutdownComplete";
            im.SourceQueue = "testAutoSendShutdownComplete";

            task6.PostMessage(im);

            System.Threading.Thread.Sleep(350);

            im = null;

            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.STARTED, task6.TaskStatus);
            csUnit.Assert.True(task6.IsThreadAlive);
            csUnit.Assert.True(q.Receive(out im));
            csUnit.Assert.NotNull(im);
            csUnit.Assert.Equals(ITask.MSG_STARTUP_COMPLETE, im.MessageId);

            im = new InternalMessage();

            im.MessageId = ITask.MSG_SHUTDOWN;
            im.Source = "testAutoSendShutdownComplete";
            im.SourceQueue = "testAutoSendShutdownComplete";

            task6.PostMessage(im);

            System.Threading.Thread.Sleep(500);

            im = null;

            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.SHUTDOWN, task6.TaskStatus);
            csUnit.Assert.True(task6.IsThreadAlive);
            csUnit.Assert.False(q.Receive(out im));
            csUnit.Assert.Null(im);

            task6.SendShutdownComplete();

            System.Threading.Thread.Sleep(250);

            csUnit.Assert.True(q.Receive(out im));
            csUnit.Assert.NotNull(im);
            csUnit.Assert.Equals(task6.Name, im.Source);
            csUnit.Assert.Equals(ITask.MSG_SHUTDOWN_COMPLETE, im.MessageId);

            task6.SignalShutdown = true;
            
            task6.PostMessage(im);                          // This is just to pull it out of its Receive() wait.
                                                            // Should never need to be done in "real life".

            System.Threading.Thread.Sleep(50);

            csUnit.Assert.False(task6.IsThreadAlive);

            q.Cleanup();
        }
    }
}