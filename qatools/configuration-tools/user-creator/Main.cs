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
        public const string numPhonesArg = "n";
        public const string userPasswordArg = "pass";
        public const string userPinArg = "pin";
        public const string assocDpArg = "assoc";
        public const string maxAxlWriteArg = "a";
        public const string userPasswordDefault = "metreos";
        public const string userPinDefault = "343434";
        public const string ccmUserDefault = "Administrator";
        public const string ccmPassDefault = "metreos";
        public const string assocDpDefault = "y";
        public const string maxAxlWriteDefault = "20";
        public const string descriptionDefault = "";

        public static string Usage = String.Format(@"
Arguments:
Name        Arg       Default
-----------------------------
<CCM IP>    {0}     -
<CCM USER>  {1}         {2}
<CCM PASS>  {3}         {4}
<BASE MAC>  {5}         -
<# USERS>   {6}         -
<PASSWORD>  {7}      {8}
<PIN>       {9}       {10}
<ASSOC DP>  {11}       {12}
<AXL WRITE> {13}         {14}", 
            ccmIpArg,
            ccmUserArg, ccmUserDefault,
            ccmPassArg, ccmPassDefault,
            baseMacArg,
            numPhonesArg,
            userPasswordArg, userPasswordDefault,
            userPinArg, userPinDefault,
            assocDpArg, assocDpDefault,
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

            string ccmIp = GetArgument(parser, ccmIpArg, null);
            string ccmUser = GetArgument(parser, ccmUserArg, ccmUserDefault);
            string ccmPass = GetArgument(parser, ccmPassArg, ccmPassDefault);
            string baseMac = GetArgument(parser, baseMacArg, null);
            int numPhones = int.Parse(GetArgument(parser, numPhonesArg, null));
            int maxAxlWrite = int.Parse(GetArgument(parser, maxAxlWriteArg, maxAxlWriteDefault));
            string password = GetArgument(parser, userPasswordArg, userPasswordDefault);
            string pin = GetArgument(parser, userPinArg, userPinDefault);
            string assocDp = GetArgument(parser, assocDpArg, assocDpDefault);

            if(baseMac.StartsWith("SEP"))
            {
                baseMac = baseMac.Substring(3);
            }

            long baseMacLong = long.Parse(baseMac, System.Globalization.NumberStyles.HexNumber); 

            AXLAPIService service = new AXLAPIService(ccmIp, ccmUser, ccmPass);

            Console.WriteLine("\nStarting...\n\n");
            Worker worker = new Worker(service, maxAxlWrite);
            bool success = worker.Generate(baseMacLong, numPhones, password, pin, assocDp.ToLower() == "y" || assocDp.ToLower() == "yes");
        
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
