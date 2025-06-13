using System;

using Metreos.Utilities;

namespace CallMonitorReport
{
	/// <summary>
	/// Summary description for Reporter
	/// </summary>
	class ReportConsole
	{        
        private const string fromDateSymbol = "s";
        private const string toDateSymbol = "e";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
            // Check the command args, if perhaps we need to override some values
            CommandLineArguments parser = new CommandLineArguments(args);

            string fromDate = "", toDate = "";
            if(parser.IsParamPresent(fromDateSymbol))
            {
                try
                {
                    fromDate = parser.GetSingleParam(fromDateSymbol);
                }
                catch { }
            }

            if(parser.IsParamPresent(toDateSymbol))
            {
                try
                {
                    toDate = parser.GetSingleParam(toDateSymbol);
                }
                catch { }
            }

            bool toDateValid = IsDateValid(toDate);
            bool fromDateValid = IsDateValid(fromDate);

            if (!toDateValid && !fromDateValid)
            {
                PrintHelp();
                return;
            }

            if (!toDateValid)
            {
                if (toDate.Length == 0 && fromDateValid)
                {
                    toDate = null;
                }
                else
                {
                    Console.WriteLine("Error: Invalid command line options");
                    Console.WriteLine(" ");
                    PrintHelp();
                    return;
                }
            }

            if (!fromDateValid)
            {
                if (fromDate.Length == 0 && toDateValid)
                {
                    fromDate = null;
                }
                else
                {
                    Console.WriteLine("Error: Invalid command line options");
                    Console.WriteLine(" ");
                    PrintHelp();
                    return;
                }
            }

            Reporter rt = new Reporter();
            if (rt.ReadData(fromDate, toDate))
            {
                rt.WriteCsv();
                rt.WriteHtml();
            }
        }

        static private void PrintHelp()
        {
            Console.WriteLine("Metreos Call Monitor Reporting Tool");
            Console.WriteLine("(C) Copyright, 2003-2005 Metreos Corp.");
            Console.WriteLine("Usage: CallMonitorReport.exe -s:yyyy-mm-dd -e:yyyy-mm-dd");
            Console.WriteLine("-s:yyyy-mm-dd\tReport Start Date");
            Console.WriteLine("-e:yyyy-mm-dd\tReport End Date");
        }

        static private bool IsDateValid(string dateString)
        {
            try
            {
                DateTime.Parse(dateString);
                return true;
            }
            catch
            {
                return false;
            }
        }
	}
}
