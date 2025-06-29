using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Metreos.Utilities;
using Metreos.Configuration;


namespace BackupTool
{
    class Program
    {
        static int Main(string[] args)
        {
            Metreos.Interfaces.IConsoleApps.PrintHeaderText("Backup Tool");

            if (args.Length > 0)
            {
                if (args[0] == "-h")
                {
                    UsageHelp();
                    return 0;
                }
            }

            int final = 0;

            // Get database connection credentials
            Hashtable configs = Metreos.Configuration.AppConfig.GetTable();
            string dbHost = configs["DatabaseHostname"].ToString();
            ushort dbPort = ushort.Parse(configs["DatabasePort"].ToString());
            string dbName = configs["DatabaseName"].ToString();
            string dbUser = configs["DatabaseUsername"].ToString();
            string dbPass = configs["DatabasePassword"].ToString();

            BehaviorCore.Config.DEFAULT_DB_HOST = dbHost;
            BehaviorCore.Config.DEFAULT_DB_PORT = dbPort.ToString();
            BehaviorCore.Config.DEFAULT_DB_NAME = dbName;
            BehaviorCore.Config.DEFAULT_DB_USER = dbUser;
            BehaviorCore.Config.DEFAULT_DB_PASSWORD = dbPass;

            // Create the database object
            string dsn = Database.FormatDSN(dbName, dbHost, dbPort, dbUser, dbPass, true);
            IDbConnection db = Database.CreateConnection(Database.DbType.mysql, dsn);
            db.Open();

            using (BehaviorCore.Behavior core = new BehaviorCore.Behavior())
            {
                core.DbConnection = db;
                try
                {
                    if (core.CreateBackup(args))
                    {
                        Console.WriteLine();
                        Console.WriteLine(String.Format("The backup was a success!  It can be located in {0}.", core.DestinationPath));
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("The backup was aborted by the user.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine("ERROR: The backup failed due to the following exception -");
                    Console.WriteLine(ex.Message);
                    final = -1;
                }
            }

            db.Close();
            return final;
        }

        static void UsageHelp()
        {
            Console.WriteLine("Usage: cuae-backup [-h] [+<database_name> ...]");
            Console.WriteLine("     -h                  displays the usage help");
            Console.WriteLine("     +<database_name>    additional database to backup");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("     To do a basic backup -");
            Console.WriteLine("         cuae-backup");
            Console.WriteLine("     To do a backup that includes databases 'dbone' and 'dbtwo' -");
            Console.WriteLine("         cuae-backup +dbone +dbtwo");
        }
    }
}
