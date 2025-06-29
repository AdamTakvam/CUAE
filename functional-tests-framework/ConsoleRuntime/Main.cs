using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Specialized;

using Metreos.LogSinks;
using Metreos.LoggingFramework;
using Metreos.Utilities;

using Metreos.Samoa.FunctionalTestRuntime;

using FTF=Metreos.Samoa.FunctionalTestFramework;

namespace ConsoleRuntime
{
	/// <summary> Entry holder of the FTF console</summary>
	class MainEntry
	{
        public const string listAllTests = "l";
        public const string listFullname = "f";
        public const string listDescription ="d";

        private static Thread readThread = null;
        private static bool stop = false;
        private static FunctionalTestMain main = null;

        [STAThread]
        static void Main(string[] args)
        {
            CommandLineArguments parser = new CommandLineArguments(args);
            using(main = new FunctionalTestMain(parser))
            {
                ConsoleLoggerSink console = new ConsoleLoggerSink(TraceLevel.Verbose);
                NonWindowedRuntime consoleRuntime = new NonWindowedRuntime(main);
                main.Initialize();
                
                if(parser.IsParamPresent(listAllTests))
                {
                    main.OutputTestNames(parser.IsParamPresent(listFullname), parser.IsParamPresent(listDescription));
                    return;
                }

                if(!main.EstablishRemoteConnection(
                    main.Settings.Username, main.Settings.Password))
                {
                    Console.WriteLine("Unable to connect to the Application Server");
                    consoleRuntime.Close();
                    return;
                }

                // Determine if indidivual tests have been specified, or if all should be run in bulk

                StringCollection tests = parser.GetStandAloneParameters();
                bool runIndividual = tests != null && tests.Count > 0;
                
                StartConsoleReader();

                if(runIndividual) 
                {
                    foreach(string test in tests)
                    {
                        main.RunTest(test);
                    }
                }
                else
                {
                    main.RunAllAutomatedTests();     // Run all tests
                }

                consoleRuntime.Close(); 
            }
		}

        private static void StartConsoleReader()
        {
            if(readThread == null)
            {
                readThread = new Thread(new ThreadStart(ConsoleRead));
                readThread.IsBackground = true;
                readThread.Start();
            }
        }

        private static void ConsoleRead()
        {
            while(!stop)
            {
                Console.WriteLine("Enter 's' to gracefully stop the current test");
                Console.WriteLine("Enter 'q' to gracefully stop the current test, and stop testing");

                string userEntry = Console.ReadLine();

                switch(userEntry)
                {
                    case "q":
                        // Stop current test, then quit
                        main.StopAll();
                        break;

                    case "s":
                        // Stop current test
                        main.StopTest();
                        break;
                }
            }
        }
	}
}
