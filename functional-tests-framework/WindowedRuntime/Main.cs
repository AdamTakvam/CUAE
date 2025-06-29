using System;
using System.Threading;

using Metreos.Utilities;
using Metreos.Samoa.FunctionalTestRuntime;

namespace WindowedRuntime
{
	/// <summary> Creates a Windowed app with which to control the test framework </summary>
	public class MainEntry
	{
        private static ManualResetEvent testToolClosed = new ManualResetEvent(false);

        [STAThread]
        static void Main(string[] args) 
        {
            CommandLineArguments parser = new CommandLineArguments(args);

            using(FunctionalTestMain testFramework = new FunctionalTestMain(parser))
            {
                TestTool windowedTool = new TestTool(testFramework);
                windowedTool.Disposed += new EventHandler(TestTool_Disposed);

                testFramework.Initialize();
                windowedTool.ShowDialog();
            
                testFramework.Dispose();
            }
        }

        private static void TestTool_Disposed(object sender, EventArgs args)
        {
            testToolClosed.Set();
        }
	}
}
