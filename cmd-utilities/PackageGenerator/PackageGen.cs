using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Specialized;

using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.PackageGeneratorCore;

namespace Metreos.PackageGenerator
{
    /// <summary>Command-line utility for generating Action/Event XML packages</summary>
	public sealed class PackageGen
	{
		[STAThread]
		static void Main(string[] args)
		{
            // Print copyright info
            IConsoleApps.PrintHeaderText("Action/Event Package Generator");

            CommandLineArguments clargs = new CommandLineArguments(args);

            if(clargs.IsParamPresent(Parameters.PARAM_HELP))
            {
                PrintHelp();
                return;
            }

            if(clargs.IsParamPresent(Parameters.PARAM_DEBUG))
            {
                Console.WriteLine("Attach the debugger and press Enter to continue");
                Console.ReadLine();
            }

            if(clargs.IsParamPresent(Parameters.PARAM_SOURCE) == false)
            {
                Console.WriteLine("You must specify a source file.");
                PrintHelp();
                return;
            }
            string srcFile = clargs.GetSingleParam(Parameters.PARAM_SOURCE);
            System.Diagnostics.Debug.Assert(srcFile != null, "No source file specified");
            
            string dest = null;
            if(clargs.IsParamPresent(Parameters.PARAM_DESTINATION) == false)
            {
                dest = ".";
            }
            else
            {
                dest = clargs.GetSingleParam(Parameters.PARAM_DESTINATION);
            }

            if(!Directory.Exists(dest))
            {
                Console.WriteLine("Destination output directory {0} not found. Please create this directory.", dest);
                return;
            }

            bool overwrite = false;
            if(clargs.IsParamPresent(Parameters.PARAM_OVERWRITE))
            {
                overwrite = true;
            }

            FileInfo file = new FileInfo(srcFile);

            if(file == null)
            {
                Console.WriteLine("Could not open {0} for reading", srcFile);
                return;
            }

            if(file.Extension != ".dll")
            {
                Console.WriteLine("Invalid source file. Please specify a valid .NET assembly (.dll).");
                return;
            }

            if(!file.Exists)
            {
                Console.WriteLine("Source file {0} does not exist", file.FullName);
                return;
            }

            // Preload additional assemblies specified
            string[] references = clargs[Parameters.PARAM_REF];

            // Add additional unmanaged dependency search paths to %PATH%
            string[] depPaths = clargs[Parameters.PARAM_SEARCH];
            string oldSystemPath = null;

            if(depPaths != null) 
            {
                oldSystemPath = System.Environment.GetEnvironmentVariable("Path");
                string systemPath = oldSystemPath;

                if(systemPath == null)
                {
                    Console.WriteLine("System configuration error: Could not load PATH environment variable");
                    return;
                }

                int p = -1;
                for(int i=0; i<depPaths.Length; i++)
                {
                    p = systemPath.IndexOf(depPaths[i]);

                    if(p == -1)
                    {
                        systemPath = String.Format("{0};{1}", systemPath, depPaths[i]);
                    }
                }

                // Add dependency path(s) to system path
                bool success = false;
                try
                {
                    success = Win32.SetEnvironmentVariableEx("Path", systemPath);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Error setting PATH environment variable: " + e.Message);
                    return;
                }

                if(!success)
                {
                    Console.WriteLine("Error setting PATH environment variable");
                    return;
                }
            }

            // Parse the assembly
            XmlGenerator xmlGen = new XmlGenerator(file, references);
            xmlGen.LogWriter = new LogWriteDelegate(WriteMessage);
            
            if(xmlGen.Parse())
            {

                dest = Path.Combine(dest, file.Name);
                dest = dest.Replace(".dll", ".xml");
                
                FileInfo destFile = new FileInfo(dest);
                
                if(destFile.Exists && !overwrite)
                {
                    Console.Write("File {0} exists, do you wish to overwrite (y/n)? ", destFile.FullName);
                    string response = Console.ReadLine();
                    if(response == null || response.ToLower().StartsWith("n"))
                    {
                        WriteMessage(TraceLevel.Error, "Write operation cancelled.");
                        return;
                    }
                }

                if(xmlGen.WriteFile(destFile))
                {
                    Console.WriteLine("File written: {0}", dest);
                }
                else
                {
                    Console.WriteLine("XML creation failed.");
                }
            }

            if(oldSystemPath != null)
            {
                Win32.SetEnvironmentVariableEx("Path", oldSystemPath);
            }
		}

        private static void WriteMessage(TraceLevel level, string message)
        {
            Console.WriteLine("{0}: {1}", level.ToString(), message);
        }

        static void PrintHelp()
        {
            Console.WriteLine();
            Console.WriteLine("Usage: PGEN.EXE [{0}] {1} [{2}] [{3}] [{4}]", 
                Parameters.PARAM_HELP_HELP, Parameters.PARAM_SOURCE_HELP, 
                Parameters.PARAM_DESTINATION_HELP, Parameters.PARAM_SEARCH_HELP,
                Parameters.PARAM_REF_HELP);
            Console.WriteLine();
            Console.WriteLine("Required Parameters:");
            Console.WriteLine("  {0,-20} File name of the source assembly", Parameters.PARAM_SOURCE_HELP);
            Console.WriteLine();
            Console.WriteLine("Optional Parameters:");
            Console.WriteLine("  {0,-20} Path for output, if not current directory", Parameters.PARAM_DESTINATION_HELP);
            Console.WriteLine("  {0,-20} Search path for unmanaged DLL dependencies", Parameters.PARAM_SEARCH_HELP);
            Console.WriteLine("  {0,-20} .NET assemblies referenced by the src file.  Absolute path required.", Parameters.PARAM_REF_HELP);
            Console.WriteLine("  {0,-20} Overwrite without prompting", Parameters.PARAM_OVERWRITE_HELP);
            Console.WriteLine("  {0,-20} Print this help screen", Parameters.PARAM_HELP_HELP);
            Console.WriteLine();
            Console.WriteLine("Note:");
            Console.WriteLine("  The output file will have the same name as the input file except it will");
            Console.WriteLine("  have a .xml extension");
        }
	}
}
