using System;
using System.Diagnostics;

namespace Metreos.Samoa.Core.Tests
{
    public class TestRunMockTask : Metreos.Samoa.Core.TaskBase
    {
        public System.Threading.ManualResetEvent runCalledEvent;

        public TestRunMockTask() : base("TestRunMockTask", TraceLevel.Info)
        {
            runCalledEvent = new System.Threading.ManualResetEvent(false);
        }

        protected override void Run()
        {
            runCalledEvent.Set();
        }
    }
    
    public class TaskBaseTest
    {
        public TaskBaseTest()
        {
        }

        public void testRun()
        {
            bool runCalled = false;

            TestRunMockTask task = new TestRunMockTask();

            task.Start();

            runCalled = task.runCalledEvent.WaitOne(5000, false);   // Wait 5 seconds for the event to fire

            csUnit.Assert.True(runCalled);

            task.Cleanup();

            task = null;
        }
    }
}
