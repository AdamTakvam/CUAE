using System;
using System.Collections;
using Metreos.AxlSoap413;
using Metreos.Utilities;

namespace AROneExtern
{
	class Class1
	{
        public const string ccmIpArg = "ccmIp";
        public const string ccmUserArg = "ccmUser";
        public const string ccmPassArg = "ccmPass";
        public const string maxAxlWriteArg = "x";
        public const string ccmUserDefault = "Administrator";
        public const string maxAxlWriteDefault = "20";

        public static string Usage = String.Format(@"
This tool will query the Metreos Application Suite database for user accounts,
and perform the necessary configurations in ccmadmin and mceadmin to enable the 
account for ActiveRelay with a SCCP-Shared line.

User accounts without an associated device will be ignored.
This tool must be run on the Metreos Communications Environment server.

Arguments:
Name        Arg       Default
-----------------------------
<CCM IP>    {0}         -
<CCM USER>  {1}         {2}
<CCM PASS>  {3}         -
<AXL SPEED> {5}         {6}
",
            ccmIpArg,
            ccmUserArg, ccmUserDefault,
            ccmPassArg,
            maxAxlWriteArg, maxAxlWriteDefault);

		[STAThread]
		static void Main(string[] args)
		{
            CommandLineArguments parser = new CommandLineArguments(args);
            
            if(args.Length == 0)
            {
                Console.WriteLine(Usage);
                return;
            }

            string ccmIp    = GetArgument(parser, ccmIpArg, null);
            string ccmUser  = GetArgument(parser, ccmUserArg, ccmUserDefault);
            string ccmPass  = GetArgument(parser, ccmPassArg, null);
            int axlSpeed    = 0;
            try
            {
                axlSpeed    = int.Parse(GetArgument(parser, maxAxlWriteArg, maxAxlWriteDefault));
            }
            catch{ Console.WriteLine("The AXL SPEED parameter must be an integer.  Giving up."); }

            Worker worker = new Worker(ccmIp, ccmUser, ccmPass, axlSpeed);
            bool success = worker.Generate();
        
            Console.WriteLine(success ? "Success" : "Failure");
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
