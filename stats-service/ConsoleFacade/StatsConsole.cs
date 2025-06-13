using System;
using System.Threading;
using System.Diagnostics; 

using Metreos.Core;
using Metreos.Stats;
using Metreos.LogSinks;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.Utilities;

namespace Metreos.Stats
{
    /// <summary>Console facade for the stats service</summary>
	class StatsConsole
	{           
        [STAThread]
        static void Main(string[] args)
        {
            IConsoleApps.PrintHeaderText("Statistics Service");

            Console.WriteLine("Press \"Enter\" to start the service. Type 'q' to quit.");

            string input = Console.ReadLine();
            if(input == "q")
            {
                Console.WriteLine();
                return;
            }

            ConsoleLoggerSink consSink = new ConsoleLoggerSink(TraceLevel.Verbose);

            using(StatsServer server = new StatsServer())
            {
                // Get test param: StatsServer -test
                CommandLineArguments clargs = new CommandLineArguments(args);
                if(clargs.IsParamPresent("test"))
                {
                    server.StartTestData();
                }

                Console.WriteLine("Service started");
                Console.WriteLine();

                while(input != "q")
                {
                    Console.WriteLine("Press \"q\" to terminate.");
                    input = Console.ReadLine();

                    switch(input)
                    {
                        case "p":
                            server.PrintStats();
                            break;
                        case "g1":
                            server.TestGenerateGraph(new TimeSpan(0, 1, 0));
                            break;
                        case "g2":
                            server.TestGenerateGraph(new TimeSpan(0, 10, 0));
                            break;
                        case "g3":
                            server.TestGenerateGraph(new TimeSpan(0, 30, 0));
                            break;
                        case "g4":
                            server.TestGenerateGraph(IStats.MgmtListener.Commands.Interval.Hour);
                            break;
                        case "g5":
                            server.TestGenerateGraph(IStats.MgmtListener.Commands.Interval.SixHour);
                            break;
                        case "g6":
                            server.TestGenerateGraph(IStats.MgmtListener.Commands.Interval.TwelveHour);
                            break;
                        case "g7":
                            server.TestGenerateGraph(IStats.MgmtListener.Commands.Interval.Day);
                            break;
                        case "g8":
                            server.TestGenerateGraph(IStats.MgmtListener.Commands.Interval.Week);
                            break;
                        case "g9":
                            server.TestGenerateGraph(IStats.MgmtListener.Commands.Interval.Month);
                            break;
                        case "g10":
                            server.TestGenerateGraph(IStats.MgmtListener.Commands.Interval.Year);
                            break;
                        case "x":
                            System.Environment.SetEnvironmentVariable("TZ", "");
                            break;
                    }
                }

                server.StopTestData();
            }        
		}
	}
}
