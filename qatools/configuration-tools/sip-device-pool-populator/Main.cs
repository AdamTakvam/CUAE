using System;
using System.Collections;
using Metreos.AxlSoap413;
using Metreos.Utilities;

namespace AROneExtern
{
	class Class1
	{
        public const string baseDnArg = "d";
        public const string devicePoolArg = "p";
        public const string numPhonesArg = "n";

        public static string Usage = String.Format(@"
Arguments:
Name        Arg       Default
-----------------------------
<BASE DN>   {0}         -
<DEV POOL>  {1}         -
<# PHONES>  {2}         -
",
            baseDnArg,
            devicePoolArg,
            numPhonesArg);

		/// <summary></summary>
		[STAThread]
		static void Main(string[] args)
		{
            CommandLineArguments parser = new CommandLineArguments(args);
            
            if(args.Length == 0)
            {
                Console.WriteLine(Usage);
                return;
            }

            int baseDn = int.Parse(GetArgument(parser, baseDnArg, null));
            string devicePool = GetArgument(parser, devicePoolArg, null);
            int numPhones = int.Parse(GetArgument(parser, numPhonesArg, null));

            Worker worker = new Worker();
            bool success = worker.Generate(baseDn, numPhones, devicePool);
        
            Console.WriteLine(success);
        }

        public static string GetArgument(CommandLineArguments parser, string arg, string defaultValue)
        {
            string foundValue;
            if(!parser.IsParamPresent(arg) && defaultValue == null)
            {
                Console.WriteLine("\nRequired Parameter '{0}' not present!\n", arg); 
                throw new ApplicationException("Stopping execution!");
            }
            else if(!parser.IsParamPresent(arg))
            {
                Console.WriteLine("Using default value '{0}' for parameter '{1}'", defaultValue, arg);
                foundValue = defaultValue;
            }
            else
            {
                foundValue = parser.GetSingleParam(arg);
            }

            return foundValue;
        }
	}
}
