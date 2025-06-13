using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.ServiceProcess;

using Metreos.LogSinks;
using Metreos.Interfaces;
using Metreos.Configuration;
using Metreos.LoggingFramework;

namespace Metreos.SftpServer
{
	public class SftpServerService : System.ServiceProcess.ServiceBase
	{
        private const string AppName    = "SftpServer";

		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.Container components = null;

        private readonly Server sftpServer;

		public SftpServerService()
		{
			// This call is required by the Windows.Forms Component Designer.
			InitializeComponent();

            DBHelper config = CreateDbHelper();

            if(config == null)
                throw new Exception("Server cannot start: Database connection information missing in app.config");

            this.sftpServer = new Server(config, GetListenPort());
		}

		// The main entry point for the process
		static void Main()
		{
			System.ServiceProcess.ServiceBase[] ServicesToRun;
	
			// More than one user Service may run within the same process. To add
			// another service to this process, change the following line to
			// create a second service object. For example,
			//
			//   ServicesToRun = new System.ServiceProcess.ServiceBase[] {new Service1(), new MySecondUserService()};
			//
			ServicesToRun = new System.ServiceProcess.ServiceBase[] { new SftpServerService() };

			System.ServiceProcess.ServiceBase.Run(ServicesToRun);
		}

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			this.ServiceName = "SftpServerService";
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			    components.Dispose();

            base.Dispose(disposing);
		}

		/// <summary>
		/// Set things in motion so your service can do its work.
		/// </summary>
		protected override void OnStart(string[] args)
		{
            TraceLevel logLevel = TraceLevel.Info;
            if(args != null && args.Length > 0)
            {
                try { logLevel = (TraceLevel) Enum.Parse(typeof(TraceLevel), args[0], true); }
                catch {}
            }

            EventLogSink els = new EventLogSink(EventLog, TraceLevel.Warning);

            if(!this.sftpServer.Start())
                throw new ApplicationException("Failed to initialize server. Verify that the settings in SftpServerService.exe.config are correct");
		}
 
		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
            this.sftpServer.Stop();
		}

        private DBHelper CreateDbHelper()
        {
            try
            {
                string dbName = AppConfig.GetEntry(IConfig.ConfigFileSettings.DB_NAME);
                string dbHost = AppConfig.GetEntry(IConfig.ConfigFileSettings.DB_HOST);
                string dbPortStr = AppConfig.GetEntry(IConfig.ConfigFileSettings.DB_PORT);
                string dbUsername = AppConfig.GetEntry(IConfig.ConfigFileSettings.DB_USERNAME);
                string dbPassword = AppConfig.GetEntry(IConfig.ConfigFileSettings.DB_PASSWORD);
                string mediaUser = AppConfig.GetEntry(IConfig.ConfigFileSettings.SFTP_MEDIA_USER);
                string grammarUser = AppConfig.GetEntry(IConfig.ConfigFileSettings.SFTP_GRAMMAR_USER);

                if(dbPortStr == null)
                {
                    EventLog.WriteEntry(ServiceName, "No database port specified in app.config file", EventLogEntryType.Error);
                    return null;
                }

                ushort dbPort = Convert.ToUInt16(dbPortStr);
        
                return new DBHelper(dbName, dbHost, dbPort, dbUsername, dbPassword, mediaUser, grammarUser);
            }
            catch(Exception e)
            {
                EventLog.WriteEntry(ServiceName, "App.config file corrupt: " + e.Message, EventLogEntryType.Error);
                return null;
            }
        }

        private int GetListenPort()
        {
            string portStr = AppConfig.GetEntry(IConfig.ConfigFileSettings.SFTP_LISTEN_PORT);
            if(portStr != null)
                return Convert.ToInt32(portStr);
            return 0;
        }
	}
}
