using System;
using System.Diagnostics;
using System.Configuration;

using Metreos.LogSinks;
using Metreos.Interfaces;
using Metreos.Configuration;
using Metreos.LoggingFramework;

namespace Metreos.SftpServer
{
	/// <summary>Console facade for SFTP server</summary>
	class SftpConsole
	{
        #region Main

		[STAThread]
		static void Main(string[] args)
		{
            IConsoleApps.PrintHeaderText("Secure FTP Server");

            TraceLevel level = TraceLevel.Info;
            if(args != null && args.Length > 0)
            {
                try { level = (TraceLevel) Enum.Parse(typeof(TraceLevel), args[0], true); }
                catch
                {
                    System.Console.WriteLine("Warning: Invalid log level specified. Using \"Info\".");
                }
            }

            SftpConsole c = new SftpConsole(level);
            if(!c.Start())
            {
                System.Console.WriteLine("Error: app.config file settings are invalid or corrupt");
                return;
            }

            do
            {
                System.Console.WriteLine("Press 'q' to quit");
            }
            while(System.Console.ReadLine().ToLower() != "q");

            c.Stop();
		}
        #endregion

        private readonly Server sftpServer;

        public SftpConsole(TraceLevel level)
        {
            DBHelper config = CreateDbHelper();

            if(config == null)
                throw new ApplicationException("Server cannot start: Database connection information missing in app.config");

            ConsoleLoggerSink cls = new ConsoleLoggerSink(level);

            this.sftpServer = new Server(config, GetListenPort());
        }

        public bool Start()
        {
            return this.sftpServer.Start();
        }

        public void Stop()
        {
            this.sftpServer.Stop();
            this.sftpServer.Dispose();
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
                    Console.WriteLine("No database port specified in app.config file");
                    return null;
                }

                ushort dbPort = Convert.ToUInt16(dbPortStr);
        
                return new DBHelper(dbName, dbHost, dbPort, dbUsername, dbPassword, mediaUser, grammarUser);
            }
            catch(Exception e)
            {
                Console.WriteLine("App.config file corrupt: " + e.Message);
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
