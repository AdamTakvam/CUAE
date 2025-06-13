using System;
using System.Configuration;

using Metreos.Core;
using Metreos.LogServer;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Configuration;

namespace Metreos.LogServer
{
	/// <summary> Console Runtime for the Log Server </summary>
	class LogConsole
	{
		private const string numberFilesSymbol = "f";
		private const string numberLinesSymbol = "n";
        private const string portSymbol = "p";
        private const string logDirectorySymbol = "d";

        [STAThread]
		static void Main(string[] args)
		{
            IConsoleApps.PrintHeaderText("Log Server");

            Console.WriteLine("Press \"Enter\" to start the service. Type 'q' to quit.");

            string input = Console.ReadLine();
            if(input == "q")
            {
                Console.WriteLine();
                return;
            }

            using(LogServer logServer = new LogServer())
            {
                // Read config data from database
                ReadConfigDataFromDatabase(logServer);

                // Read config data from config file
                ReadConfigDataFromConfigFile(logServer);

                // Check the command args, if perhaps we need to override some values
                CommandLineArguments parser = new CommandLineArguments(args);

                Console.WriteLine("Log Path: " + logServer.LocalPath);

                logServer.Start();

                Console.WriteLine("Service started");
                Console.WriteLine();

                while(input != "q")
                {
                    Console.WriteLine("Press \"q\" to terminate.");
                    input = Console.ReadLine();
                }

                logServer.Stop();
            }
        }

        /// <summary> 
        /// Read configuration data from application config file
        /// </summary>
        private static void ReadConfigDataFromConfigFile(LogServer logServer)
        {
            logServer.LocalPath = ConfigurationManager.AppSettings[IServerLog.CONFIG_LOGROOTFOLDER];
        }

        /// <summary> 
        /// Read configuration data from command line
        /// </summary>
        private static void ReadConfigDataFromCommandLine(LogServer logServer, CommandLineArguments parser)
        {
            if(parser.IsParamPresent(numberLinesSymbol))
            {
                try
                {
                    uint numLines = uint.Parse(parser.GetSingleParam(numberLinesSymbol));
                    logServer.NumberLines = numLines;
                }
                catch {}
            }

            if(parser.IsParamPresent(numberFilesSymbol))
            {
                try
                {
                    uint numFiles = uint.Parse(parser.GetSingleParam(numberFilesSymbol));
                    logServer.NumberFiles = numFiles;
                }
                catch {}
            }
            
            if(parser.IsParamPresent(portSymbol))
            {
                try
                {
                    ushort portNumber = ushort.Parse(parser.GetSingleParam(portSymbol));
                    logServer.Port = portNumber;
                }
                catch {}
            }

            if(parser.IsParamPresent(logDirectorySymbol))
            {
                try
                {
                    string logDirectory = parser.GetSingleParam(logDirectorySymbol);
                    logServer.LocalPath = logDirectory;
                }
                catch {}
            }
        }

        /// <summary> 
        /// Read configuration data from database
        /// </summary>
        private static void ReadConfigDataFromDatabase(LogServer logServer)
        {
            uint u = Config.LogService.NumFiles;
            if (u != 0)
                logServer.NumberFiles = u;

            u = Config.LogService.NumLinesPerFile;
            if (u != 0)
                logServer.NumberLines = u;

            ushort us = Config.LogService.ListenPort;
            if (us != 0)
                logServer.Port = us;

            string fp = Config.LogService.FilePath;
            if (fp != null && fp != String.Empty)
                logServer.LocalPath = fp;
        }
	}
}
