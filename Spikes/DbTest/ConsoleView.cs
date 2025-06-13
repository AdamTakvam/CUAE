using System;
using System.Text;
using System.Data;
using System.Diagnostics;

using ByteFX.Data.MySqlClient;

using Metreos.Interfaces;
using Metreos.AppServer.Configuration;

namespace DbTest
{
	class ConsoleView
	{
		[STAThread]
		static void Main(string[] args)
		{
            ConsoleView cv = new ConsoleView();
            cv.Go();
		}

        private const string HOST       = "localhost";
        private const string PORT       = "3306";
        private const string DB_NAME    = "MCE";
        private const string USERNAME   = "root";
        private const string PASSWORD   = "";
        
        public ConsoleView()
        {
        }

        public void Go()
        {
            //"DATABASE=SiteTracker;Driver=mysql;SERVER=localhost;UID=hamu;PWD
            //=naptra;PORT=3306;OPTION=131072;STMT=;"
            StringBuilder dsn = new StringBuilder("DATABASE=");
            dsn.Append(DB_NAME);
            dsn.Append(";SERVER=");
            dsn.Append(HOST);
            dsn.Append(";PORT=");
            dsn.Append(PORT);
            dsn.Append(";UID=");
            dsn.Append(USERNAME);
            dsn.Append(";PWD=");
            dsn.Append(PASSWORD);

            using(IDbConnection db = new MySqlConnection(dsn.ToString()))
            {
                db.Open();

                Config config = Config.Instance;

                IConfig.ComponentInfo cInfo = new IConfig.ComponentInfo();
                cInfo.name = "MyApp1";
                cInfo.type = IConfig.ComponentType.Application;
                cInfo.status = IConfig.Status.Enabled_Running;

                if(config.AddComponent(cInfo) == false)
                {
                    Console.WriteLine("Add component failed");
                    db.Close();
                    return;
                }

                IConfig.ConfigEntry entry = new IConfig.ConfigEntry("logLevel", "Error", "", IConfig.StandardFormat.String);
            
                if(config.AddEntry(IConfig.ComponentType.Application, "MyApp1", entry, null, null) == false)
                {
                    Console.WriteLine("Add entry failed");
                    db.Close();
                    return;
                }

                IConfig.ConfigEntry gotEntry = config.GetEntry(IConfig.ComponentType.Application, "MyApp1", "logLevel", null, null);

                Console.WriteLine("Press <Enter> to continue");
                Console.ReadLine();

                db.Close();
            }
        }
	}
}
