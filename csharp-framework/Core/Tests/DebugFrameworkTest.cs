#define TrackMethodCalls    // Make sure this is always enabled when we test this class

using System;

namespace Metreos.Samoa.Core.Tests
{
    public class DebugFrameworkTestListener : System.Diagnostics.TraceListener
    {
        public string message;

        public override void Write(string message)
        {
            WriteLine(message);
        }

        public override void WriteLine(string message)
        {
            this.message = message;
        }
    }

    public class DebugFrameworkTest
    {
        private DebugFrameworkTestListener listener;

        public DebugFrameworkTest()
        {
            listener = new DebugFrameworkTestListener();

            System.Diagnostics.Trace.Listeners.Clear();
            System.Diagnostics.Trace.Listeners.Add(listener);
        }

        [csUnit.FixtureTearDown]
        public void FixtureTearDown()
        {
            listener = null;
        }

        public void testMethodEnter()
        {
            Metreos.Samoa.Core.DebugFramework.MethodEnter();

            System.Threading.Thread.Sleep(5);

            csUnit.Assert.NotNull(listener.message);
            csUnit.Assert.NotEquals("", listener.message);
            csUnit.Assert.True(listener.message.IndexOf(DebugFramework.METHODCALL + ": ENTERING") > 0);
            csUnit.Assert.True(listener.message.IndexOf("DebugFrameworkTest.testMethodEnter") > 0);
        }

        public void testMethodExit()
        {
            Metreos.Samoa.Core.DebugFramework.MethodExit();

            System.Threading.Thread.Sleep(5);

            csUnit.Assert.NotNull(listener.message);
            csUnit.Assert.NotEquals("", listener.message);
            csUnit.Assert.True(listener.message.IndexOf(DebugFramework.METHODCALL + ": EXITING") > 0);
            csUnit.Assert.True(listener.message.IndexOf("DebugFrameworkTest.testMethodExit") > 0);
        }

        public void testMethodEnterWithUserText()
        {
            Metreos.Samoa.Core.DebugFramework.MethodEnter("User Text");

            System.Threading.Thread.Sleep(5);

            csUnit.Assert.NotNull(listener.message);
            csUnit.Assert.NotEquals("", listener.message);
            csUnit.Assert.True(listener.message.IndexOf(DebugFramework.METHODCALL + ": ENTERING") > 0);
            csUnit.Assert.True(listener.message.IndexOf("DebugFrameworkTest.testMethodEnterWithUserText") > 0);
            csUnit.Assert.True(listener.message.IndexOf("(User Text)") > 0);
        }

        public void testMethodExitWithUserText()
        {
            Metreos.Samoa.Core.DebugFramework.MethodExit("User Text");

            System.Threading.Thread.Sleep(5);

            csUnit.Assert.NotNull(listener.message);
            csUnit.Assert.NotEquals("", listener.message);
            csUnit.Assert.True(listener.message.IndexOf(DebugFramework.METHODCALL + ": EXITING") > 0);
            csUnit.Assert.True(listener.message.IndexOf("DebugFrameworkTest.testMethodExitWithUserText") > 0);
            csUnit.Assert.True(listener.message.IndexOf("(User Text)") > 0);
        }
    }
}
