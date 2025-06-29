using System;
using System.Collections;
using Metreos.AxlSoap413;
using Metreos.Utilities;

namespace devicecreator
{
    class Class1
    {
        public const string ccmIpArg = "ccmip";
        public const string ccmUserArg = "u";
        public const string ccmPassArg = "p";
        public const string baseMacArg = "m";
        public const string baseDnArg = "d";
        public const string incrementArg = "i";
        public const string numPhonesArg = "n";
        public const string descriptionArg = "s";
        public const string maxAxlWriteArg = "a";
        public const string cssArg = "css";
        public const string partitionArg = "part";
        public const string templateArg = "t";
        public const string deviceProfileRangeArg = "dp";
        public const string ccmUserDefault = "Administrator";
        public const string ccmPassDefault = "metreos";
        public const string incrementDefault = "1";
        public const string maxAxlWriteDefault = "20";
        public const string descriptionDefault = "";
        public const string cssDefault = "Unrestricted";
        public const string partDefault = "Unrestricted";
        public const string templateDefault = "Standard 7960";

        public static string Usage = String.Format(@"
Arguments:
Name        Arg       Default
-----------------------------
<CCM IP>    {0}     -
<CCM USER>  {1}         {2}
<CCM PASS>  {3}         {4}
<BASE MAC>  {5}         -
<BASE DN>   {6}         -
<INCR>      {7}         {8}
<# PHONES>  {9}         -
<DESC>      {10}         {11}
<CSS>       {12}       {13}
<PART>      {14}      {15}
<TEMPLATE>  {16}         {17}
<AXL WRITE> {18}         {19}
<DP RANGE>  {20}        <NONE>", 
            ccmIpArg,
            ccmUserArg, ccmUserDefault,
            ccmPassArg, ccmPassDefault,
            baseMacArg,
            baseDnArg,
            incrementArg, incrementDefault,
            numPhonesArg,
            descriptionArg, "''",
            cssArg, cssDefault,
            partitionArg, partDefault,
            templateArg, templateDefault,
            maxAxlWriteArg, maxAxlWriteDefault,
            deviceProfileRangeArg);

        [STAThread]
        static void Main(string[] args)
        {
            CommandLineArguments parser = new CommandLineArguments(args);
            
            if(args.Length == 0)
            {
                Console.WriteLine(Usage);
                return;
            }

            string ccmIp = GetArgument(parser, ccmIpArg, null);
            string ccmUser = GetArgument(parser, ccmUserArg, ccmUserDefault);
            string ccmPass = GetArgument(parser, ccmPassArg, ccmPassDefault);
            string baseMac = GetArgument(parser, baseMacArg, null);
            int baseDn = int.Parse(GetArgument(parser, baseDnArg, null));
            string description = GetArgument(parser, descriptionArg, descriptionDefault);
            int increment = int.Parse(GetArgument(parser, incrementArg, incrementDefault));
            int numPhones = int.Parse(GetArgument(parser, numPhonesArg, null));
            int maxAxlWrite = int.Parse(GetArgument(parser, maxAxlWriteArg, maxAxlWriteDefault));
            string css = GetArgument(parser, cssArg, cssDefault);
            string partition = GetArgument(parser, partitionArg, partDefault);
            string template = GetArgument(parser, templateArg, templateDefault);
            string deviceProfileRange = GetArgument(parser, deviceProfileRangeArg, "NONE");

            if(deviceProfileRange.StartsWith("SEP"))
            {
                deviceProfileRange = deviceProfileRange.Substring(3);
            }

            long deviceProfileStart = 0;
            if(deviceProfileRange != "NONE")
            {
                deviceProfileStart = long.Parse(deviceProfileRange, System.Globalization.NumberStyles.HexNumber);
            }

            if(baseMac.StartsWith("SEP"))
            {
                baseMac = baseMac.Substring(3);
            }

            long baseMacLong = long.Parse(baseMac, System.Globalization.NumberStyles.HexNumber); 

            AXLAPIService service = new AXLAPIService(ccmIp, ccmUser, ccmPass);

            Console.WriteLine("\nStarting...\n\n");
            Worker worker = new Worker(service, maxAxlWrite);
            bool success = worker.Generate(baseMacLong, baseDn, numPhones, description, increment, css, partition, template, deviceProfileStart);
        
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
