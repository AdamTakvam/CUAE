using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Metreos.Core;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.AppServer.CommonRuntime;

[assembly:ClassInterface(ClassInterfaceType.AutoDual)]
namespace Metreos.AppServer.ConsoleRuntime
{
    public sealed class ConsoleMain
    {
        private abstract class Consts
        {
            public abstract class Params
            {
                public const string Help        = "h";
                public const string NoPrompt    = "n";
                public const string Log         = "l";
            }

            public abstract class ParamHelp
            {
                public const string Help        = "-" + Params.Help;
                public const string NoPrompt    = "-" + Params.NoPrompt;
                public const string Log         = "-" + Params.Log + ":<TraceLevel>";
            }

            public abstract class Defaults
            {
                public const bool NoPrompt      = false;
                public const TraceLevel Log     = TraceLevel.Verbose;
            }
        }

        private ApplicationServer appServer;

        public ConsoleMain(bool promptOnStartup, TraceLevel logLevel)
        {
            // Output our standard header that includes the application name, version, and copyright
            IConsoleApps.PrintHeaderText("Application Server");

            string choice;
            if(promptOnStartup == true)
            {
                Console.WriteLine("Press \"Enter\" to start the server. Type 'q' to quit.");
                choice = Console.ReadLine();

                if(choice.ToLower() == "q")
                {
                    Console.WriteLine();
                    return;
                }
            }

            Console.WriteLine("Application Server startup beginning:");

            try
            {
                appServer = ApplicationServer.Instance;
                appServer.StartLogger(logLevel, null);
            }
            catch(Exception e)
            {
                Console.WriteLine("Server could not be started: " + Exceptions.FormatException(e));
                Console.WriteLine("Odds are that your database connection settings in AppServer.exe.config are wrong or the database has not been properly initialized");
                return;
            }

            appServer.startupComplete += new CommonRuntime.StartupCompleteDelegate(this.StartupCompleteCallback);
            appServer.shutdownComplete += new CommonRuntime.ShutdownCompleteDelegate(this.ShutdownCompleteCallback);
            appServer.startupProgress += new CommonRuntime.StartupProgressDelegate(this.StartupProgressCallback);
            appServer.shutdownProgress += new CommonRuntime.ShutdownProgressDelegate(this.ShutdownProgressCallback);
            
            bool startupOk = appServer.Startup();

            if(startupOk == true)
            {
                Console.WriteLine();
                Console.WriteLine("- Application Server started -");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("- Application Server startup timed out - Exiting - ");
                Console.WriteLine();

                appServer.Dispose();
                appServer = null;

                return;
            }

            choice = "";

            while(choice.ToLower() != "q")
            {
                Console.WriteLine("Press \"q\" to terminate.");
                choice = Console.ReadLine();
            }

            Console.WriteLine();
            Console.WriteLine("Application Server shutdown beginning:");

            appServer.Shutdown();

            appServer.StopLogger();
            appServer.Dispose();
            appServer = null;
        }


        private void StartupCompleteCallback()
        {
        }

        private void ShutdownCompleteCallback()
        {
        }

        private void StartupProgressCallback(string progressMessage)
        {
            Console.WriteLine("  --> {0}", progressMessage);
        }

        private void ShutdownProgressCallback(string progressMessage)
        {
            Console.WriteLine("  --> {0}", progressMessage);
        }

        [STAThread]
        static void Main(string[] args)
        {
            CommandLineArguments clargs = new CommandLineArguments(args);

            if(clargs.IsParamPresent(Consts.Params.Help))
            {
                PrintHelp();
                return;
            }

            bool promptOnStartup = !clargs.IsParamPresent(Consts.Params.NoPrompt);

            TraceLevel logLevel = Consts.Defaults.Log;

            if(clargs.IsParamPresent(Consts.Params.Log))
            {
                string logLevelStr = clargs.GetSingleParam(Consts.Params.Log);
                try { logLevel = (TraceLevel)Enum.Parse(typeof(TraceLevel), logLevelStr, true); }
                catch 
                {
                    PrintHelp();
                    return;
                }
            }

            ConsoleMain cm = new ConsoleMain(promptOnStartup, logLevel);

            cm = null;
        }

        static void PrintHelp()
        {
            Console.WriteLine();
            Console.WriteLine("Usage: AppServer.exe [{0}] [{1}] [{2}]", 
                Consts.ParamHelp.Help, Consts.ParamHelp.Log, Consts.ParamHelp.NoPrompt);
            Console.WriteLine();
            Console.WriteLine("Required Parameters:");
            Console.WriteLine("  <None>");
            Console.WriteLine();
            Console.WriteLine("Optional Parameters:");
            Console.WriteLine("  {0,-18} Console output log level\n\t\t\tMust be a valid System.Diagnostics.TraceLevel value", Consts.ParamHelp.Log);
            Console.WriteLine("  {0,-18} Supresses prompt for user to press a key", Consts.ParamHelp.NoPrompt);
            Console.WriteLine("  {0,-18} Print this help screen", Consts.ParamHelp.Help);
        }
    }
}
