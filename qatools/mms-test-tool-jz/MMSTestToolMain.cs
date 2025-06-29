using System;
using System.IO;
using System.Collections;
using Metreos.Utilities;

using Metreos.MMSTestTool.Parser;
using Metreos.MMSTestTool.Sessions;
using Metreos.MMSTestTool.Commands;

using CommonAST				= antlr.CommonAST;
using AST					= antlr.collections.AST;
using RecognitionException	= antlr.RecognitionException;
using TokenStreamException	= antlr.TokenStreamException;


namespace Metreos.MMSTestTool
{
	/// <summary>
	/// The entry point class for the MMSTestTool utility. It creates a new instance of the Parser, 
	/// the SessionManager.
	/// </summary>
	class MMSTestToolMain
	{
        /// <summary>
        /// Prints the help dialogue.
        /// </summary>
        private static void PrintHelp()
        {
            Console.WriteLine("Help:");
            Console.WriteLine("Supported parameters are:");
            Console.WriteLine(Constants.CL_PREFIX + Constants.CL_PARAM_HELP + " Prints this message.\n");
            Console.WriteLine(Constants.CL_PREFIX + Constants.CL_PARAM_DEF + "=filename Path specification for the XML Command definitions file. Try the \"Command Basics\" directory of the project.\n");
            Console.WriteLine(Constants.CL_PREFIX + Constants.CL_PARAM_FILE + "=filename Path specification to the file containing your test fixture. Try the \"Test Scripts\" directory of the project.\n");
            System.Environment.Exit(0);
        }

        private static void PrintMissingParam(string param)
        {
            Console.WriteLine("Missing required parameter: " + param);
        }

        /// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main(string[] args)
		{
			CommandLineArguments arguments = new CommandLineArguments(args);
            if (arguments.IsParamPresent(Constants.CL_PARAM_HELP) || args.Length == 0)
                MMSTestToolMain.PrintHelp();

            string commandDefitionFilename;
            string testFilename;
            
            if ((commandDefitionFilename = arguments.GetSingleParam(Constants.CL_PARAM_DEF)) == string.Empty)
            {
                PrintMissingParam(Constants.CL_PARAM_DEF);
                System.Environment.Exit(0);
            }
            if ((testFilename = arguments.GetSingleParam(Constants.CL_PARAM_FILE)) == string.Empty)
            {
                PrintMissingParam(Constants.CL_PARAM_FILE);
                System.Environment.Exit(0);
            }

            try 
            {
                MMSScriptLexer lexer = null;

                lexer = new MMSScriptLexer(new StreamReader(new FileStream(testFilename, FileMode.Open, FileAccess.Read)));
                //lexer = new MMSScriptLexer(new StreamReader(new FileStream("test1.mmt", FileMode.Open, FileAccess.Read)));
                MMSScriptParser parser = new MMSScriptParser(lexer);
                
                Console.WriteLine(Constants.CL_XML_READ_START);
                CommandDescriptionContainerHandler.ReadXmlDescription(commandDefitionFilename);
                Console.WriteLine(Constants.CL_XML_READ_DONE);

                Console.WriteLine(Constants.PARSER_EXEC_BEGIN);
                parser.fixture();
                Console.WriteLine(Constants.PARSER_EXEC_END);

                CommonAST t = (CommonAST)parser.getAST();
                ArrayList fixtureList = parser.GetFixtures();

                //CommandDescriptionContainerHandler.ReadXmlDescription(@"C:\work\MMSTestTool\MMSTool\MMSTool\Command Basics\commandDefinitions.xml");
                Console.WriteLine(Constants.SM_EXEC_BEGIN);
                SessionManager sessionManager = new SessionManager("Metreos.MMSTestTool.Messaging.XmlMMSMessageFactory","Metreos.MMSTestTool.TransportLayer.MSMQTransport");
                SessionManager.ServerConnect("10","5");
                SessionManager.AddSession("Session1",fixtureList,1,10);
            } 
            //exceptions need to be un-fudged, and try/catch blocks need to be more atomic 
            catch(TokenStreamException e) 
            {
                Console.Error.WriteLine("TokenStreamException exception: " + e.Message);
            }
            catch(RecognitionException e) 
            {
                Console.Error.WriteLine("RecognitionException exception: " + e.Message);
            }
            catch(antlr.ANTLRException e)
            {
                Console.Error.WriteLine("antlr Exception: " + e.Message);
            }
            catch
            {
                Console.Error.WriteLine("Exception occured");
            }
		}
	}
}