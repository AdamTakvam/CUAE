using System;
using System.Collections;
using Metreos.AxlSoap413;
using Metreos.Utilities;

namespace AROneExtern
{
	class Class1
	{
        public const string baseDnArg = "d";
        public const string linePreArg = "l";
        public const string findMePreArg = "f";
        public const string incrementArg = "i";
        public const string numPhonesArg = "n";
        public const string ccmUserDefault = "Administrator";
        public const string ccmPassDefault = "metreos";
        public const string linePreDefault = "";
        public const string findMePreDefault = "";
        public const string incrementDefault = "1";
        public const string maxAxlWriteDefault = "20";

        public static string Usage = String.Format(@"
Arguments:
Name        Arg       Default
-----------------------------
<BASE DN>   {0}         -
<LINE PRE>  {1}         {2}
<FDME PRE>  {3}         {4}
<INCR>      {5}         {6}
<# PHONES>  {7}         -
",
            baseDnArg,
            linePreArg, "''",
            findMePreArg, "''",
            incrementArg, incrementDefault,
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
            string linePrefix = GetArgument(parser, linePreArg, linePreDefault);
            string findMePrefix = GetArgument(parser, findMePreArg, findMePreDefault);
            int increment = int.Parse(GetArgument(parser, incrementArg, incrementDefault));
            int numPhones = int.Parse(GetArgument(parser, numPhonesArg, null));

            Worker worker = new Worker();
            bool success = worker.Generate(baseDn, numPhones, linePrefix, findMePrefix, increment, "127.0.0.1");
        
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
