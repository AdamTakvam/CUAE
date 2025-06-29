using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

using Metreos.Utilities;
using Metreos.Configuration;


namespace RestoreTool
{
    class Program
    {
        static int Main(string[] args)
        {

            Metreos.Interfaces.IConsoleApps.PrintHeaderText("Restore Tool");

            if (args.Length == 0)
            {
                UsageHelp();
                return -1;
            }
            else if (args[0] == "-h")
            {
                UsageHelp();
                return 0;
            }

            string package_file = args[0];

            if (!File.Exists(package_file))
            {
                Console.WriteLine();
                Console.WriteLine("ERROR: Could not find the package file.");
                return -1;
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

            Console.WriteLine("IMPORTANT NOTICE: Before restoring the system, it is recommended that CUAE");
            Console.WriteLine("and all related services are stopped.");
            Console.WriteLine("Would you like to continue? [Y/N]");
            char response = Console.ReadKey(true).KeyChar;
            if (response == 'Y' || response == 'y')
            {
                using (BehaviorCore.Behavior core = new BehaviorCore.Behavior())
                {
                    core.DbConnection = db;
                    try
                    {
                        if (core.Restore(package_file))
                        {
                            Console.WriteLine();
                            Console.WriteLine("The restore was successful!");
                            Console.WriteLine("If the package came from a previous version of the CUAE, then you");
                            Console.WriteLine("will also want to run the CUAE Database Tool (cuae-dbtool.exe).");
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("The restore was aborted by the user.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine();
                        Console.WriteLine("ERROR: The restore failed due to the following exception -");
                        Console.WriteLine(ex.Message);
                        final = -1;
                    }
                }
            }

            db.Close();
            return final;
        }

        static void UsageHelp()
        {
            Console.WriteLine("Usage: cuae-restore [-h] <package_file>");
            Console.WriteLine("     -h                 display usage help");
            Console.WriteLine("     <package_file>     a CUAE backup package from which to restore");
            Console.WriteLine();
            Console.WriteLine("Example:");
            Console.WriteLine("     To perform a restore from a backup package named MYBACKUP.CUAE -");
            Console.WriteLine("          cuae_restore MYBACKUP.CUAE");
        }
    }
}
