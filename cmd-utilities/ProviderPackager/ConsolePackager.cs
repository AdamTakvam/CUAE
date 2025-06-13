using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Specialized;

using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.ProviderPackagerCore;
using Metreos.ProviderPackagerCore.Xml;

namespace Metreos.ProviderPackager
{
    class ConsolePackager
    {
        static void Main(string[] args)
        {
            IConsoleApps.PrintHeaderText("Provider Packaging Tool");

            CommandLineArguments clargs = new CommandLineArguments(args);

            if(clargs.IsParamPresent(Parameters.Help.HelpParam))
            {
                PrintHelp();
                return;
            }

            StringCollection saps = clargs.GetStandAloneParameters();
            if(saps == null || saps.Count != 1)
            {
                PrintHelp();
                Environment.ExitCode = 1;
                return;
            }

            Packager packager;
            try { packager = new Packager(saps[0]); }
            catch(Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                Environment.ExitCode = 1;
                return;
            }

            // Single params
            packager.OutputFile = clargs.GetSingleParam(Parameters.Output);
            
            // Service Manifest
            string smFile = clargs.GetSingleParam(Parameters.ServManifest);
            if(smFile != null && smFile != String.Empty)
            {
                try { packager.SetServiceManifest(smFile); }
                catch(Exception e)
                {
                    Console.WriteLine("Error processing service manifest: " + e.Message);
                    Environment.ExitCode = 1;
                    return;
                }
            }

            // Doc Manifest
            string dmFile = clargs.GetSingleParam(Parameters.DocManifest);
            if(dmFile != null && dmFile != String.Empty)
            {
                try { packager.SetDocManifest(dmFile); }
                catch(Exception e)
                {
                    Console.WriteLine("Error processing document manifest: " + e.Message);
                    Environment.ExitCode = 1;
                    return;
                }
            }

            // Services
            string[] servFiles = clargs[Parameters.ServiceFile];
            if(servFiles != null)
            {
                foreach(string file in servFiles)
                {
                    if(!packager.AddServiceFile(file))
                    {
                        Console.WriteLine("Error: Could not find file: " + file);
                        Environment.ExitCode = 1;
                        return;
                    }
                }
            }

            // References
            string[] refFiles = clargs[Parameters.RefFile];
            if(refFiles != null)
            {
                foreach(string file in refFiles)
                {
                    if(!packager.AddReferenceFile(file))
                    {
                        Console.WriteLine("Error: Could not find file: " + file);
                        Environment.ExitCode = 1;
                        return;
                    }
                }
            }

            // Service references
            string[] srFiles = clargs[Parameters.ServRefFile];
            if(srFiles != null)
            {
                foreach(string file in srFiles)
                {
                    if(!packager.AddServiceReferenceFile(file))
                    {
                        Console.WriteLine("Error: Could not find file: " + file);
                        Environment.ExitCode = 1;
                        return;
                    }
                }
            }

            // Resources
            string[] resFiles = clargs[Parameters.ResFile];
            if(resFiles != null)
            {
                foreach(string file in resFiles)
                {
                    if(!packager.AddResourceFile(file))
                    {
                        Console.WriteLine("Error: Could not find file: " + file);
                        Environment.ExitCode = 1;
                        return;
                    }
                }
            }

            // Docs
            string[] docFiles = clargs[Parameters.DocFile];
            if(docFiles != null)
            {
                foreach(string file in docFiles)
                {
                    if(!packager.AddDocFile(file))
                    {
                        Console.WriteLine("Error: Could not find file: " + file);
                        Environment.ExitCode = 1;
                        return;
                    }
                }
            }

            // Web
            string[] webFiles = clargs[Parameters.WebFile];
            if(webFiles != null)
            {
                foreach(string file in webFiles)
                {
                    if(!packager.AddWebFile(file))
                    {
                        Console.WriteLine("Error: Could not find file: " + file);
                        Environment.ExitCode = 1;
                        return;
                    }
                }
            }

            try { packager.GeneratePackage(); }
            catch(Exception e)
            {
                Console.WriteLine("Error: " + e);
                Environment.ExitCode = 1;
                return;
            }

            Console.WriteLine("File created: " + packager.OutputFile);
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Usage: mcp.exe {0} [options]", Parameters.Help.ProvFile);
            Console.WriteLine();
            Console.WriteLine("Required Parameters:");
            Console.WriteLine("  {0,-20} Name of the main provider file (.dll)", Parameters.Help.ProvFile);
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  {0,-20} Print this help screen", Parameters.Help.HelpParam);
            Console.WriteLine("  {0,-20} Fully-qualified output file name (.mcp)", Parameters.Help.Output);
            Console.WriteLine("  {0,-20} Code file referenced by the provider", Parameters.Help.RefFile);
            Console.WriteLine("  {0,-20} Miscellaneous resource file", Parameters.Help.ResFile);
            Console.WriteLine("  {0,-20} Web administration file", Parameters.Help.WebFile);
            Console.WriteLine("  {0,-20} Document file", Parameters.Help.DocFile);
            Console.WriteLine("  {0,-20} Document manifest XML", Parameters.Help.DocManifest);
            Console.WriteLine("  {0,-20} Windows service executable", Parameters.Help.ServiceFile);
            Console.WriteLine("  {0,-20} Code file referenced by Windows service", Parameters.Help.ServRefFile);
            Console.WriteLine("  {0,-20} Service manifest XML", Parameters.Help.ServManifest);
            Console.WriteLine();
            Console.WriteLine("Notes:");
            Console.WriteLine("  Look in this directory for sample manifest files.");

            try { GenerateManifestSamples(); }
            catch(Exception e)
            {
                Console.WriteLine();
                Console.WriteLine("Error generating manifest example files: " + e.Message);
            }
        }

        private static void GenerateManifestSamples()
        {
            const string ServiceManFilename = "ServiceManifestExample.xml";
            const string DocManFilename = "DocManifestExample.xml";

            if(!File.Exists(ServiceManFilename))
            {
                // -- Service Manifest
                ServiceManifestType sMan = new ServiceManifestType();
                sMan.Service = new ServiceType[1];
                sMan.Service[0] = new ServiceType();
                sMan.Service[0].Filename = "myservice.exe";
                sMan.Service[0].Name = "MyService";
                sMan.Service[0].DisplayName = "My Awesome Service";
                sMan.Service[0].Description = "My Awesome Description";
                sMan.Service[0].Username = "Administrator";
                sMan.Service[0].Password = "AdminPass";
                sMan.Service[0].Argument = new string[2];
                sMan.Service[0].Argument[0] = "-debug";
                sMan.Service[0].Argument[1] = "port:80";

                const string sHelpStr = "\r\n<!-- Only 'Filename' and 'Name' attributes are required. Everything else is optional. -->";

                // Generate manifest file
                XmlSerializer serializer = new XmlSerializer(typeof(ServiceManifestType));
                using(FileStream fStream = File.OpenWrite(ServiceManFilename))
                {
                    serializer.Serialize(fStream, sMan);

                    byte[] buffer = System.Text.Encoding.Default.GetBytes(sHelpStr);
                    fStream.Write(buffer, 0, buffer.Length);
                    fStream.Flush();
                }
            }

            if(!File.Exists(DocManFilename))
            {
                // -- Document Manifest
                DocumentsType dMan = new DocumentsType();
                dMan.Documents = new DocumentType[1];
                dMan.Documents[0] = new DocumentType();
                dMan.Documents[0].Filename = "readme.txt";
                dMan.Documents[0].DisplayName = "Read Me";
                dMan.Documents[0].Description = "Provider usage info.";

                const string dHelpStr = "\r\n<!-- Only the 'Filename' attribute is required. Everything else is optional. -->";

                // Generate manifest file
                XmlSerializer serializer = new XmlSerializer(typeof(DocumentsType));
                using(FileStream fStream = File.OpenWrite(DocManFilename))
                {
                    serializer.Serialize(fStream, dMan);

                    byte[] buffer = System.Text.Encoding.Default.GetBytes(dHelpStr);
                    fStream.Write(buffer, 0, buffer.Length);
                    fStream.Flush();
                }
            }
        }
    }
}
