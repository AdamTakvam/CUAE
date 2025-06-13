using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.ServiceProcess;
using System.Configuration;

using Metreos.LogServer;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Configuration;

namespace Metreos.LogServer
{
	public class LogServerService : System.ServiceProcess.ServiceBase
	{
		// command line options:
		// -n:x		-> x lines per file
		// -f:x		-> x files per application folder
		// -p:x		-> port x
		// -d:path	-> path is log parent folder
		private const string numberLinesSymbol = "n";				// number of lines
		private const string numberFilesSymbol = "f";				// number of files
		private const string portSymbol = "p";						// port number
		private const string logDirectorySymbol = "d";				// log directory

		private System.ComponentModel.Container components = null;
		private LogServer logServer = null;

		public LogServerService()
		{
			// This call is required by the Windows.Forms Component Designer.
			InitializeComponent();
		}

		// The main entry point for the process
		static void Main()
		{
			System.ServiceProcess.ServiceBase[] ServicesToRun;	
			ServicesToRun = new System.ServiceProcess.ServiceBase[] { new LogServerService() };
			System.ServiceProcess.ServiceBase.Run(ServicesToRun);
		}

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			this.ServiceName = "MetreosLogServer";
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Set things in motion so your service can do its work.
		/// </summary>
		protected override void OnStart(string[] args)
		{
			// Create the log server
			logServer = new LogServer();

			// Read config data from database
			ReadConfigDataFromDatabase(logServer);

			// Read config data from config file
			ReadConfigDataFromConfigFile(logServer);

			// Check the command args, if perhaps we need to override some values
			CommandLineArguments parser = new CommandLineArguments(args);

			// Read config data from command line
			ReadConfigDataFromCommandLine(logServer, parser);

			logServer.Start();
		}
 
		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
			logServer.Stop();
			logServer = null;
		}

		/// <summary> 
		/// Handle shutdown event
		/// </summary>
		protected override void OnShutdown()
		{
			OnStop();
		}

		/// <summary> 
		/// Read configuration data from application config file
		/// </summary>
		private void ReadConfigDataFromConfigFile(LogServer logServer)
		{
			logServer.LocalPath = ConfigurationManager.AppSettings[IServerLog.CONFIG_LOGROOTFOLDER];
        }

		/// <summary> 
		/// Read configuration data from command line
		/// </summary>
		private void ReadConfigDataFromCommandLine(LogServer logServer, CommandLineArguments parser)
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
		private void ReadConfigDataFromDatabase(LogServer logServer)
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
