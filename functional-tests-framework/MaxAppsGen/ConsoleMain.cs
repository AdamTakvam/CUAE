using System;

namespace Metreos.Samoa.FunctionalTestFramework
{

   
	class ConsoleMain
	{
		[STAThread]
		static void Main(string[] args)
		{
            if(args == null)
            {
                Console.WriteLine(usage);
            }

            if(args.Length < 4 || args.Length > 6)
            {
                Console.WriteLine(usage);
            }

            try
            {
                string maxAppsFolder;

                MaxAppsFileOptions options = ParseArgs(args, out maxAppsFolder);

                MaxAppsGenerator.GenerateMaxAppsFile(options, maxAppsFolder);
            }		
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());

                Console.WriteLine("");

                Console.WriteLine(usage);
            }
        }
            
        private static MaxAppsFileOptions ParseArgs(string[] args, out string baseFolder)
        {
            baseFolder = null;

            MaxAppsFileOptions options = new MaxAppsFileOptions();

            foreach(string arg in args)
            {
                string param = arg.Substring(0, 3);

                switch(param)
                {
                    case "-b:":
                        baseFolder = arg.Substring(3);
                        break;

                    case "-n:":
                        options.useNamespace = arg.Substring(3);
                        break;
 
                    case "-f:":
                        options.outputFileFolder = arg.Substring(3);
                        break;

                    case "-o:":
                        options.outputFileName = arg.Substring(3);
                        break;

                    case "-t:":
                        options.topLevelName = arg.Substring(3);
                        break;

                    case "-p:":
                        options.pause = true;
                        break;

                    default:
                        throw new Exception("");
                }
            }

            return options;
        }

        #region Usage

        private const string usage = @"
Usage:
----------------------------------------------------------------------
-b:     [Base folder of Max Apps]                       (required)
-n:     [Namespace]                                     (required)
-f:     [OutputFolder]                                  (not required)
-o:     [OutputFileName]                                (required)
-t:     [Top level class name]                          (required)
-p:     [Leave blank: causes Console.Read]              (not required)

";
        #endregion
	}
}
