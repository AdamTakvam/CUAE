using System;
using System.Collections.Generic;
using System.Text;
using Metreos.Utilities;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.DocGeneratorCore;

namespace Metreos.DocGenerator
{
    class Program
    {
        private static string USAGE = String.Format(@"

 USAGE INSTRUCTIONS
-{0}:     [Required] The name of resulting file 
-{1}:  [Required] The display name to use in the TOC/front page 
-{2}:       [Required] Input Directory to search for XML Packages 
-{3}:      [Optional] Output Directory to place generated content 
-{4}:     [Optional] Path to XSLT file to use for transformation 

 Example:  /> docgen.exe -name:mydocs -display:""Company A's Docs"" 
                         -in:c:\mypgenxml -out:c:\mydocxml
",
            PARAM_BOOKNAME,
            PARAM_BOOKDISPLAYNAME,
            PARAM_INPUTDIR,
            PARAM_OUTPUTDIR,
            PARAM_XSLTPATH
            );
      
        private const string PARAM_INPUTDIR = "in";
        private const string PARAM_OUTPUTDIR = "out";
        private const string PARAM_BOOKNAME = "name";
        private const string PARAM_BOOKDISPLAYNAME = "display";
        private const string PARAM_XSLTPATH = "xslt";
        private const string PARAM_GLOSSARYPATH = "glossary";

        static void Main(string[] args)
        {
            // Print copyright info
            IConsoleApps.PrintHeaderText("Package Documentation Generator");

            CommandLineArguments parser = new CommandLineArguments(args);

            Metreos.DocGeneratorCore.DocGen generator = new Metreos.DocGeneratorCore.DocGen();
            generator.Logged += new Metreos.DocGeneratorCore.DocGen.Log(generator_Logged);

            string inDir = null;
            string outDir = null;
            string bookName = null;
            string bookDisplayname = null;
            string glossaryPath = null;
            string xsltPath;

            if (!parser.IsParamPresent(PARAM_INPUTDIR))
            {
                Console.WriteLine(" USAGE ERROR: '{0}' argument is missing.", PARAM_INPUTDIR);
                Console.WriteLine(" SUGGESTION:  The input directory must be specified.");
                Console.WriteLine(USAGE);
                return;
            }
            else
            {
                inDir = parser.GetSingleParam(PARAM_INPUTDIR);
            }

            if (!parser.IsParamPresent(PARAM_OUTPUTDIR))
            {
                outDir = System.Environment.CurrentDirectory;
            }
            else
            {
                outDir = parser.GetSingleParam(PARAM_OUTPUTDIR);
            }

            if (!parser.IsParamPresent(PARAM_BOOKNAME))
            {
                Console.WriteLine(" USAGE ERROR: '{0}' argument is missing.", PARAM_BOOKNAME);
                Console.WriteLine(" SUGGESTION:  The book name must be specified.");
                Console.WriteLine(USAGE);
                return;
            }
            else
            {
                bookName = parser.GetSingleParam(PARAM_BOOKNAME);
            }

            if (!parser.IsParamPresent(PARAM_BOOKDISPLAYNAME))
            {
                Console.WriteLine(" USAGE ERROR: '{0}' argument is missing.", PARAM_BOOKDISPLAYNAME);
                Console.WriteLine(" SUGGESTION:  The book display name must be specified.");
                Console.WriteLine(USAGE);
                return;
            }
            else
            {
                bookDisplayname = parser.GetSingleParam(PARAM_BOOKDISPLAYNAME);
            }

            if (!parser.IsParamPresent(PARAM_XSLTPATH))
            {
                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                xsltPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(exePath), "XSLT\\package2docbook.xsl");
            }
            else
            {
                xsltPath = parser.GetSingleParam(PARAM_XSLTPATH);
            }

            if(!parser.IsParamPresent(PARAM_GLOSSARYPATH))
            {
                glossaryPath = null;
            }
            else
            {
                glossaryPath = parser.GetSingleParam(PARAM_GLOSSARYPATH);
            }

            try
            {
                generator.Generate(inDir, outDir, xsltPath, bookName, bookDisplayname, glossaryPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error in docbook transalation!  {0}", e);
            }
        }

        static void generator_Logged(System.Diagnostics.TraceLevel level, string message)
        {
            Console.WriteLine("{0}: {1}", level, message);
        }
    }
}
