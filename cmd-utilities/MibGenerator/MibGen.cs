using System;
using System.IO;

using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Configuration;

namespace Metreos.MibGen
{
    public class Program
    {
        [STAThread]
		static void Main(string[] args)
		{
            IConsoleApps.PrintHeaderText("MIB File Generator");

            CommandLineArguments clargs = new CommandLineArguments(args);

            if(clargs.IsParamPresent(Parameters.HelpParam))
            {
                PrintHelp();
                return;
            }

            string target = clargs.GetSingleParam(Parameters.Target);
            if(target == null || target == String.Empty)
                target = AppDomain.CurrentDomain.BaseDirectory;

            try
            {
                MibGenerator.GenerateMIB(target);
                Console.WriteLine("MIB file written: " + Path.Combine(target, MibGenerator.Consts.MibFilename));
            }
            catch(Exception e)
            {
                Console.WriteLine("Failed to generate MIB: " + e.Message);
            }
		}

        private static void PrintHelp()
        {
            Console.WriteLine();
            Console.WriteLine("Usage: mibgen.exe [{0}] {1}", 
                Parameters.Help.HelpParam, Parameters.Help.Target);
            Console.WriteLine();
            Console.WriteLine("Required Parameters:");
            Console.WriteLine("  None");
            Console.WriteLine();
            Console.WriteLine("Optional Parameters:");
            Console.WriteLine("  {0,-20} Target directory for new MIB file", Parameters.Help.Target);
            Console.WriteLine("  {0,-20} Print this help screen", Parameters.Help.HelpParam);
        }
    }
}
