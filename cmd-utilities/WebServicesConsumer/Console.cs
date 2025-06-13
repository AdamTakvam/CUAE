using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Interfaces;
using Metreos.Utilities;
using Metreos.WebServicesConsumerCore;

namespace Metreos.WebServicesConsumer
{
    /// <summary> Console interface for the WSDL to Metreos Assemblies Tool </summary>
    class MainEntry
	{
        #region Usage Message
        protected static string nameDelimiter = "n";
        protected static string outputDelimiter = "o";
        protected static string frameworkDelimiter = "d";
        protected static string versionDelimiter = "v";
        protected static string overwriteDelimiter = "f";
        protected const string usage = @"This utility will generate any assemblies needed within the
Application Runtime Enviroment given a WSDL file to consume.

MWS.EXE -f <WSDL file path> <XSD file> <XSD file...> -n:<name>  
        -:o<output directory> -:d<framework directory> -:v<version>

Required parameters

<WSDL file path> 
    The path to the WSDL file to process.

<XSD file>
    A fully qualified path, or relative to the WSDL file, 
    to any additional XSD files needed by the WSDL file.

<name> 
    A name of your choosing to provide a basis for the assembly names 
    and namespaces generated this tool.  Should only contain alphanumeric 
    characters; cannot start with a numeral.

-:d<framework directory>
    The fully qualified path to the Metreos 'framework' directory.

-:v<version>
    The version of the Metreos Framework to use.

Optional parameters

-:o<output directory>
    The directory in which the assemblies will be created.  If none is 
    specified, then the current directory is used.  

-f:<overwrite flag>
    Overwrites any code or assemblies in place for a web service of the 
    same name. Default is to not overwrite
";

        #endregion

        [STAThread]
        static void Main(string[] args)
        {
            string wsdlpath     = null;
            string wsdlname     = null;
            string outputDir    = null;
            string frameworkDir = null;
            string version      = null;
            string[] xsds         = null;
            bool overwrite      = false;

            IConsoleApps.PrintHeaderText("WSDL Consumer Tool");

            if(!ParseCommandLineArgs(args, out wsdlname, out wsdlpath, out xsds, out outputDir, out frameworkDir, out version, out overwrite))
            {
                Console.WriteLine(usage);
                return;
            }

            Console.Write(FormatVerboseStartInfo(wsdlname, wsdlpath, outputDir, frameworkDir, version, overwrite));

            using(MetreosWsdlConsumer consumer = new MetreosWsdlConsumer(wsdlname, wsdlpath, outputDir, Path.Combine(outputDir, "mws.log"), frameworkDir, version, xsds))
            {
                try
                {
                    consumer.Format(overwrite);

                    if(consumer.References == null || consumer.References.Length == 0)
                    {
                        Console.WriteLine("No assemblies were generated for the WSDL file provided.");
                    }
                    else
                    {
                        Console.WriteLine("The following assemblies were created:" + System.Environment.NewLine);
                        foreach(string reference in consumer.References)
                        {
                            Console.WriteLine(reference);
                        }
                    }
                }
                catch(WsdlConvertException e)
                {
                    Console.WriteLine(e.Message);
                    if(e.InnerException != null && e.InnerException is CompileException)
                    {
                        CompileException ce = e.InnerException as CompileException;
                        Console.WriteLine(ce.Message);
                        Console.WriteLine("Code compilation errors as follows:" + System.Environment.NewLine);
                        Console.WriteLine(ce.FormatCompilerErrors());
                    }
                }
                catch(WsdlParseException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch(DuplicateNameException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch(NativeTypeAssembleException e)
                {
                    Console.WriteLine(e.Message);
                    if(e.InnerException != null && e.InnerException is CompileException)
                    {
                        CompileException ce = e.InnerException as CompileException;
                        Console.WriteLine(ce.Message);
                        Console.WriteLine("Code compilation errors as follows:" + System.Environment.NewLine);
                        Console.WriteLine(ce.FormatCompilerErrors());
                    }
                }
                catch(NativeActionAssembleException e)
                {
                    Console.WriteLine(e.Message);
                    if(e.InnerException != null && e.InnerException is CompileException)
                    {
                        CompileException ce = e.InnerException as CompileException;
                        Console.WriteLine(ce.Message);
                        Console.WriteLine("Code compilation errors as follows:" + System.Environment.NewLine);
                        Console.WriteLine(ce.FormatCompilerErrors());
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("A generic error occurred: " + e.Message + 
                        System.Environment.NewLine + "Source: " + e.Source +
                        System.Environment.NewLine + "StackTrace: " + e.StackTrace);
                }
            }
        }

        private static bool ParseCommandLineArgs(string[] args, 
            out string wsdlname, 
            out string wsdlpath,
            out string[] xsds,
            out string outputDir, 
            out string frameworkDir,
            out string version,
            out bool overwrite)
        { 
            wsdlname     = null;
            wsdlpath     = null;
            xsds         = null;
            outputDir    = null;
            frameworkDir = null;
            version      = null;
            overwrite    = false;

            if(args == null || args.Length == 0)
            {
                return false;
            }

            CommandLineArguments commandParser = new CommandLineArguments(args);

            StringCollection standAloneParameters = commandParser.GetStandAloneParameters();

            if(standAloneParameters == null || standAloneParameters.Count == 0)
            {
                Console.WriteLine("<WSDL file path> not specified.");
                return false;
            }

            ArrayList xsdFiles = new ArrayList();

            foreach(string param in standAloneParameters)
            {
                if(Path.GetExtension(param) == ".wsdl")
                {
                    wsdlpath = param;
                }
                else if(Path.GetExtension(param) == ".xsd")
                {
                    xsdFiles.Add(param);
                }
            }
            
            if(wsdlpath == null)
            {
                Console.WriteLine("<WSDL file path> not specified.");
                return false;
            }

            if(xsdFiles.Count > 0)
            {
                xsds = xsdFiles.ToArray(typeof(string)) as string[];
            }
    
            wsdlname = commandParser.GetSingleParam(nameDelimiter);
            if(wsdlname == null || wsdlname == String.Empty)
            {
                Console.WriteLine("<name> not specified.");
                return false;
            }

            frameworkDir = commandParser.GetSingleParam(frameworkDelimiter);
            if(frameworkDir == null || frameworkDir == String.Empty)
            {
                Console.WriteLine("<framework directory> not specified.");
                return false;
            }

            version = commandParser.GetSingleParam(versionDelimiter);
            if(version == null || version == String.Empty)
            {
                Console.WriteLine("<version> not specified)");
                return false;
            }

            overwrite = commandParser.IsParamPresent(overwriteDelimiter);

            // Framework/version check
            DirectoryInfo frameworkWithVersionDir = new DirectoryInfo(Path.Combine(frameworkDir, version));
            if(!frameworkWithVersionDir.Exists)
            {
                Console.WriteLine("Framework directory {0} with version {1} not found", frameworkDir, version );
                return false;
            }

            outputDir = commandParser.GetSingleParam(outputDelimiter);
            if(outputDir == null || outputDir == String.Empty)
            {
                outputDir = System.Environment.CurrentDirectory;
            }

            return true;
        }

        private static string FormatVerboseStartInfo(
            string wsdlname, 
            string wsdlpath, 
            string outputDir, 
            string frameworkDir,
            string version,
            bool   overwrite)
        {
            return String.Format(@"
Using the following values:

<WSDL file path>
    {0}
<name>   
    {1}
<framework directory>   
    {2}
<version>
    {3}
<output directory>
    {4}
<overwrite>
    {5}

", new object[] { wsdlpath, wsdlname, frameworkDir, version, outputDir, overwrite });

        }
	}
}
