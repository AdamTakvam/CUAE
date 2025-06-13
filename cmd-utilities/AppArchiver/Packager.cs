using System;
using System.Collections.Specialized;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.AppArchiveCore;

namespace Metreos.AppArchiveGenerator
{
    class Packager
    {
        [STAThread]
        static void Main(string[] args)
        {
            IConsoleApps.PrintHeaderText("Application Packaging Tool");

            CommandLineArguments clargs = new CommandLineArguments(args);

            AppPackagerOptions opts = new AppPackagerOptions();

            bool createPackage = clargs.IsParamPresent(Parameters.CREATE_PACKAGE);
            bool extractPackage = clargs.IsParamPresent(Parameters.EXTRACT_PACKAGE);
            bool debugMode = clargs.IsParamPresent(Parameters.DEBUG_MODE);

            if(debugMode == true)
            {
                Console.Write("Debug mode enabled. Press enter to continue...");
                Console.ReadLine();
                Console.WriteLine();
            }

            if(((createPackage) && (extractPackage)) == true)
            {
                Console.WriteLine("Invalid commany line option. -{0} and -{1} can not be specified at the same time.", 
                    Parameters.CREATE_PACKAGE, Parameters.EXTRACT_PACKAGE);
                PrintUsage();
                return;
            }

            if(((createPackage) || (extractPackage)) == false)
            {
                Console.WriteLine("Invalid command line option. You must specify either -{0} or -{1}.", 
                    Parameters.CREATE_PACKAGE, Parameters.EXTRACT_PACKAGE);
                PrintUsage();
                return;
            }

            if(clargs.GetStandAloneParameters().Count != 1)
            {
                Console.WriteLine("Invalid command line option. Exactly one output package name must be specified.");
                PrintUsage();
                return;
            }

            opts.appXmlFiles                = clargs[Parameters.APP_XML_FILE];
            opts.nativeTypeSearchDirs       = clargs[Parameters.NATIVE_TYPE_SEARCH_DIR];
            opts.nativeActionSearchDirs     = clargs[Parameters.NATIVE_ACTION_SEARCH_DIR];
            opts.dbCreateScripts            = clargs[Parameters.DB_CREATE_SCRIPT];
			opts.mediaFiles					= clargs[Parameters.MEDIA_FILE];
            opts.installerXmlFile           = clargs.GetSingleParam(Parameters.INSTALLER_XML_FILE);
            opts.frameworkDirName           = clargs.GetSingleParam(Parameters.FRAMEWORK_DIR);
            opts.outputDirectory            = clargs.GetSingleParam(Parameters.OUTPUT_DIRECTORY);
            
            opts.appVersion                 = clargs.GetSingleParam(Parameters.APP_VERSION);
            opts.appAuthor                  = clargs.GetSingleParam(Parameters.APP_AUTHOR);
            opts.appCompany                 = clargs.GetSingleParam(Parameters.APP_COMPANY);
            opts.appTrademark               = clargs.GetSingleParam(Parameters.APP_TRADEMARK);
            opts.appDisplayName             = clargs.GetSingleParam(Parameters.APP_DISPLAY_NAME);
            opts.appDescription             = clargs.GetSingleParam(Parameters.APP_DESCRIPTION);
            opts.appCopyright               = clargs.GetSingleParam(Parameters.APP_COPYRIGHT);
            
            opts.printUsage                 = clargs.IsParamPresent(Parameters.PRINT_USAGE);
            opts.recursiveDirSearch         = clargs.IsParamPresent(Parameters.RECURSIVE_DIRECTORY_SEARCH);
            opts.verbose                    = clargs.IsParamPresent(Parameters.VERBOSE);

            opts.filename                   = clargs.GetStandAloneParameters()[0];

            try
            {
                if(createPackage == true)
                {
                    if(opts.ValidateCreate() == false)
                    {
                        PrintUsage();
                    }
                    else
                    {
                        try
                        {
                            AppPackager.BuildPackage(opts, Console.Out);
                        }
                        catch(PackagerException e)
                        {
                            PrintErrors(e, opts.verbose);   
                            Console.WriteLine(
                                "Failed to package application '{0}'.", 
                                opts.filename);
                            return;
                        }

                        if(opts.verbose) { Console.WriteLine(); }
                        Console.WriteLine("Application '{0}' successfully packaged.", opts.filename);
                    }
                }
                else if(extractPackage == true)
                {
                    if(opts.ValidateExtract() == false)
                    {
                        PrintUsage();
                    }
                    else
                    {
                        AppPackager.ExtractPackage(opts, Console.Out);

                        Console.WriteLine("Application '{0}' successfully extracted.", opts.filename);
                    }
                }                    
            }
            catch(PackagerException e)
            {
                PrintErrors(e, opts.verbose);
            }
        }


        /// <summary>
        /// Prints the normal usage/help text to the console.
        /// </summary>
        static void PrintUsage()
        {
            Console.WriteLine();
            Console.WriteLine("Creating packages usage:");
            Console.WriteLine("MCA.EXE {0} {1} [{1} ...] {2} [options] <packageName>", 
                Parameters.CREATE_PACKAGE_HELP, Parameters.APP_XML_FILE_HELP, 
                Parameters.FRAMEWORK_DIR_HELP);
            Console.WriteLine();
            Console.WriteLine("Extracing packages usage:");
            Console.WriteLine("MCA.EXE {0} [{1}] [{2}] <packageName>", 
                Parameters.EXTRACT_PACKAGE_HELP, Parameters.OUTPUT_DIRECTORY_HELP, Parameters.VERBOSE_HELP);
            Console.WriteLine();

            Console.WriteLine("Command line parameter format: <optionName>:<optionValue>");
            Console.WriteLine("The value may be surrounded in quotes if it includes spaces.");
            Console.WriteLine();

            Console.WriteLine("Actions:");
            Console.WriteLine("  {0,-27} Create a new Metreos application package.", Parameters.CREATE_PACKAGE_HELP);
            Console.WriteLine("  {0,-27} Extract an existing Metreos application package.", Parameters.EXTRACT_PACKAGE_HELP);
            Console.WriteLine();

            Console.WriteLine("Required parameters for creating a package:");
            Console.WriteLine("  {0,-27} One or more application scripts to process.", Parameters.APP_XML_FILE_HELP);
            Console.WriteLine("  {0,-27} Metreos framework directory.", Parameters.FRAMEWORK_DIR_HELP);
            Console.WriteLine();

            Console.WriteLine("Optional parameters:");
            Console.WriteLine("  {0,-27} Application installer XML script.", Parameters.INSTALLER_XML_FILE_HELP);
            Console.WriteLine("  {0,-27} One or more database creation SQL scripts.", Parameters.DB_CREATE_SCRIPT_HELP);
			Console.WriteLine("  {0,-27} One or more media files.", Parameters.MEDIA_FILE_HELP);
            Console.WriteLine("  {0,-27} Directory in which to place command output.", Parameters.OUTPUT_DIRECTORY_HELP);
            Console.WriteLine("  {0,-27} One or more native type search directories.", Parameters.NATIVE_TYPE_SEARCH_DIR_HELP);
            Console.WriteLine("  {0,-27} One or more native action search directories.", Parameters.NATIVE_ACTION_SEARCH_DIR_HELP);
            Console.WriteLine("  {0,-27} Recursively search directories for assemblies.", Parameters.RECURSIVE_DIRECTORY_SEARCH_HELP);
            Console.WriteLine("  {0,-27} Enable verbose output.", Parameters.VERBOSE_HELP);
            Console.WriteLine("  {0,-27} This help text.", Parameters.PRINT_USAGE_HELP);
            Console.WriteLine();

            Console.WriteLine("Optional meta-data parameters:");
            Console.WriteLine("  {0,-27} Application version.", Parameters.APP_VERSION_HELP);
            Console.WriteLine("  {0,-27} Application display name.", Parameters.APP_DISPLAY_NAME_HELP);
            Console.WriteLine("  {0,-27} Application author.", Parameters.APP_AUTHOR_HELP);
            Console.WriteLine("  {0,-27} Application company.", Parameters.APP_COMPANY_HELP);
            Console.WriteLine("  {0,-27} Application copyright.", Parameters.APP_COPYRIGHT_HELP);
            Console.WriteLine("  {0,-27} Application description.", Parameters.APP_DESCRIPTION_HELP);
            Console.WriteLine();
        }

        
        static void PrintErrors(PackagerException error, bool verbose)
        {
            Debug.Assert(error != null, "PackagerException in PrintErrors() can not null");
            
            if(verbose)
            {
                Console.WriteLine();
                Console.WriteLine("[Errors]");
            }

            Console.WriteLine("Error Type: {0}", error.ErrorType);
            Console.WriteLine();

            if(error.ErrorMessages != null)
            {
                foreach(string errorMsg in error.ErrorMessages)
                {
                    Console.WriteLine(errorMsg);
                }
            }
            else
            {
                Console.WriteLine("None, errors collection is null");
            }

            Console.WriteLine();
        }
    }
}
