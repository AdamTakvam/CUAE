using System;
using System.Collections;
using Metreos.AxlSoap413;
using Metreos.Utilities;

namespace AROneExtern
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
    class Class1
    {
        public const string sepStartArg = "d";
        public const string linePreArg = "l";
        public const string findMePreArg = "f";
        public const string incrementArg = "i";
        public const string numPhonesArg = "n";
        public const string ccmIpArg = "ccm";
        public const string ccmUserArg = "u";
        public const string ccmPassArg = "p";
        public const string maxAxlWriteArg = "a";
        public const string ccmUserDefault = "Administrator";
        public const string ccmPassDefault = "metreos";
        public const string linePreDefault = "";
        public const string findMePreDefault = "";
        public const string incrementDefault = "1";
        public const string maxAxlWriteDefault = "60";

        public static string Usage = String.Format(@"
Arguments:
Name        Arg       Default
-----------------------------
<SEP START> {0}
<LINE PRE>  {1}         {2}
<FDME PRE>  {3}         {4}
<# PHONES>  {5}         -
<CCM IP>    {6}
<CCM USER>  {7}         {8}
<CCM PASS>  {9}         {10}
<MAX AXL>   {11}         {12}
",
            sepStartArg,
            linePreArg, "''",
            findMePreArg, "''",
            numPhonesArg,
            ccmIpArg, 
            ccmUserArg, ccmUserDefault,
            ccmPassArg, ccmPassDefault,
            maxAxlWriteArg, maxAxlWriteDefault
            );

		[STAThread]
		static void Main(string[] args)
		{

            CommandLineArguments parser = new CommandLineArguments(args);
            
            if(args.Length == 0)
            {
                Console.WriteLine(Usage);
                return;
            }

            string deviceStartString = GetArgument(parser, sepStartArg, null);
            string linePrefix = GetArgument(parser, linePreArg, linePreDefault);
            string findMePrefix = GetArgument(parser, findMePreArg, findMePreDefault);
            int numPhones = int.Parse(GetArgument(parser, numPhonesArg, null));
            string ccmIp = GetArgument(parser, ccmIpArg, null);
            string ccmUser = GetArgument(parser, ccmUserArg, ccmUserDefault);
            string ccmPass = GetArgument(parser, ccmPassArg, ccmPassDefault);
            int maxAxlWrite = int.Parse(GetArgument(parser, maxAxlWriteArg, maxAxlWriteDefault));

            if(deviceStartString.StartsWith("SEP"))
            {
                deviceStartString = deviceStartString.Substring(3);
            }

            long deviceStart = long.Parse(deviceStartString, System.Globalization.NumberStyles.HexNumber); 


            AXLAPIService service = new AXLAPIService(ccmIp, ccmUser, ccmPass);

            Worker worker = new Worker(service, maxAxlWrite);
            bool success = worker.Generate(deviceStart, numPhones, linePrefix, findMePrefix);
        
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
