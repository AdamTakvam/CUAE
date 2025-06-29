using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using MigrationCore;

namespace MigrationTool
{
    class Program
    {
        static int Main(string[] args)
        {
            Metreos.Interfaces.IConsoleApps.PrintHeaderText("Database Tool");

            bool finalReturn = true;
            bool dryRun = false;
            bool migrationAttempt = false;

            if (args.Length == 0)
            {
                UsageHelp();
                return -1;
            }

            using(MigrationCore.Driver mainDriver = new MigrationCore.Driver())
            {
                try
                {

                    for (int x = 0; x < args.Length; x++)
                    {
                        switch (args[x].ToLower())
                        {
                            case "-v":
                                mainDriver.LogLevel = LogLevels.VERBOSE;
                                break;
                            case "-t":
                                dryRun = true;
                                mainDriver.UseDryRunDatabase();
                                break;
                            case "rollback":
                                migrationAttempt = true;
                                if (args.Length > x+2)
                                {
                                    if (ConfirmRun())
                                    {
                                        finalReturn = mainDriver.Rollback(args[x+1]);
                                        x++;
                                    }
                                }
                                else
                                {
                                    if (ConfirmRun())
                                        finalReturn = mainDriver.Rollback();
                                }
                                x = args.Length + 1;
                                break;
                            case "upgrade":
                                migrationAttempt = true;
                                if (args.Length > x+2)
                                {
                                    if (ConfirmRun())
                                    {
                                        finalReturn = mainDriver.Upgrade(args[x+1]);
                                        x++;
                                    }
                                }
                                else
                                {
                                    if (ConfirmRun())
                                        finalReturn = mainDriver.Upgrade();
                                }
                                x = args.Length + 1;
                                break;
                            default:
                                UsageHelp();
                                finalReturn = false;
                                x = args.Length + 1;
                                break;
                        }
                    }

                }
                catch (Exception ex)
                {

                    if (ex.Data.Contains(Configuration.DISPLAY_NO_EXCEPTION_KEY))
                    {
                        Console.WriteLine();
                        Console.WriteLine(ex.Message);
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("FATAL ERROR: The following exception was thrown:");
                        Console.WriteLine(ex.ToString());
                    }
                    finalReturn = false;

                }
                finally
                {
                    if (dryRun)
                    {
                        mainDriver.DropDryRunDatabase();
                        Console.WriteLine();
                        if (finalReturn)
                        {
                            Console.WriteLine("The trial run was successful.");
                        }
                        else
                        {
                            Console.WriteLine("The trial run failed.");
                        }
                    }
                    else
                    {
                        Console.WriteLine();
                        if (migrationAttempt)
                        {
                            if (finalReturn)
                            {
                                Console.WriteLine("The migration was successful.");
                            }
                            else
                            {
                                Console.WriteLine("The migration failed.");
                            }
                        }
                    }

                }
            }
            return finalReturn ? 0 : -1;
        }

        static void UsageHelp()
        {
            Console.WriteLine("Usage: cuae-dbtool [-v] [-t] upgrade|rollback <end_version>");
            Console.WriteLine("    -v               display actions verbosely");
            Console.WriteLine("    -t               do a trial run on a copy of the database");
            Console.WriteLine("    upgrade          upgrade the database to the latest version");
            Console.WriteLine("    rollback         roll back the database to version before the last upgrade");
            Console.WriteLine("    <end_version>    force an upgrade or roll back to a specific version");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("    Upgrade the database to the latest version - ");
            Console.WriteLine("        cuae-dbtool upgrade");
            Console.WriteLine("    Roll back the database to the last version - ");
            Console.WriteLine("        cuae-dbtool rollback");
            Console.WriteLine("    Upgrade the database to the latest version and show all actions - ");
            Console.WriteLine("        cuae-dbtool -v upgrade");
            Console.WriteLine("    Perform a trial run of an upgrade - ");
            Console.WriteLine("        cuae-dbtool -t upgrade");
            Console.WriteLine("    Force an upgrade to database version 42 -");
            Console.WriteLine("        cuae-dbtool upgrade 42");
            Console.WriteLine();
            Console.WriteLine("Tips:");
            Console.WriteLine("    Before performing a migration, a trial run should be done to verify it.");
            Console.WriteLine("    If any errors or issues come up during the trial run, contact support.");
            Console.WriteLine("    It is unlikely that migration to a specific version needs to be forced");
            Console.WriteLine("    unless support advises it.");

        }

        static bool ConfirmRun()
        {
            Console.WriteLine();
            Console.WriteLine("IMPORTANT NOTICE: Before migrating the database, it is recommended that CUAE");
            Console.WriteLine("and all related services are stopped.");
            Console.WriteLine("Would you like to continue? [Y/N]");
            char response = Console.ReadKey(true).KeyChar;
            return (response == 'Y' || response == 'y');
        }
    }
}
