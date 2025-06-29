using System;
using System.IO;
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
        public const string maxAxlWriteArg = "a";
		public const string descriptionArg = "s";
		public const string outFileArg = "o";
		public const string poolMaxSizeArg = "x";
		public const string usersExclusionArg = "users";
		public const string devicesArg = "devices";
		public const string cssArg = "css";
		public const string partitionArg = "part";
		public const string templateArg = "t";
		public const string readOnlyArg = "r";
        public const string ccmUserDefault = "Administrator";
        public const string ccmPassDefault = "metreos";
        public const string maxAxlWriteDefault = "20";
		public const string outFileDefault = "results.txt";
		public const string descriptionDefault = "METREOS SPECIAL";
		public const string cssDefault = "Unrestricted";
		public const string templateDefault = "Standard 7960";
		public const string poolMaxSizeDefault = "625";
		public const string readOnlyDefault = "true";
		public const string userPromptedArg = "prompt";
		public const string userPromptedDefault = "true";

        public static string Usage = String.Format(@"
Arguments:
Name        Arg       Default
-----------------------------
<CCM IP>    {0}     -
<CCM USER>  {1}         {2}
<CCM PASS>  {3}         {4}
<BASE MAC>  {5}         -
<USERSFILE> {6}         -
<DEVFILE>   {7}         -
<READONLY>  {8}         {9}
<POOLSIZE>  {10}        {11}
<DESC>      {12}        {13}
<CSS>       {14}        {15}
<TEMPLATE>  {16}        {17}
<AXL WRITE> {18}        {19}
<OUT FILE>  {20}        {21}
<PROMPT?>   {22}        {23}", 
            ccmIpArg,
            ccmUserArg, ccmUserDefault,
            ccmPassArg, ccmPassDefault,
            baseMacArg,
			usersExclusionArg,
			devicesArg,
			readOnlyArg, readOnlyDefault,
			poolMaxSizeArg, poolMaxSizeDefault,
			descriptionArg, descriptionDefault,
			cssArg, cssDefault,
			templateArg, templateDefault,
            maxAxlWriteArg, maxAxlWriteDefault,
			outFileArg, outFileDefault,
			userPromptedArg, userPromptedDefault);

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
            string[] baseMacs = GetArguments(parser, baseMacArg, null);
			string usersFile = GetArgument(parser, usersExclusionArg, "");
			string devicesFile = GetArgument(parser, devicesArg, null);
			string description = GetArgument(parser, descriptionArg, descriptionDefault);
			string css = GetArgument(parser, cssArg, cssDefault);
			string template = GetArgument(parser, templateArg, templateDefault);
			int maxAxlWrite = int.Parse(GetArgument(parser, maxAxlWriteArg, maxAxlWriteDefault));
			int poolMaxSize = int.Parse(GetArgument(parser, poolMaxSizeArg, poolMaxSizeDefault));
			bool readOnly = bool.Parse(GetArgument(parser, readOnlyArg, readOnlyDefault));
			bool prompt = bool.Parse(GetArgument(parser, userPromptedArg, userPromptedDefault));
			string outFile = GetArgument(parser, outFileArg, outFileDefault);

			for(int i = 0; i < baseMacs.Length; i++)
			{
				string baseMac = baseMacs[i];
				if(baseMac.StartsWith("SEP"))
				{
					baseMac = baseMac.Substring(3, 6);
				}
				baseMacs[i] = baseMac;
			}
            
			ArrayList users = null;
			ArrayList devices = null;

			if(parser.IsParamPresent(usersExclusionArg))
			{
				if(!CheckUsers(usersFile, out users))
				{
					Console.WriteLine("Users Exclusion File invalid.  Exiting...");
					return;
				}
			}
			else
			{
				Console.WriteLine("No users file specified.  All users configured in appsuiteadmin will by synchronized.");
				users = new ArrayList();
			}


			if(parser.IsParamPresent(devicesArg))
			{
				if(!CheckDevices(devicesFile, out devices))
				{
					Console.WriteLine("Devices File invalid.  Exiting...");
					return;
				}

				if(devices.Count == 0)
				{
					Console.WriteLine("No devices found in Devices File.  Only appsuiteadmin database will be used.");
				}
			}
			else
			{
				Console.WriteLine("No devices file specified.  Only appsuiteadmin database will be used.");
			}

            AXLAPIService service = new AXLAPIService(ccmIp, ccmUser, ccmPass);

            Console.WriteLine("\nStarting...\n\n");
            Worker worker = new Worker(service, maxAxlWrite);
            bool success = worker.Generate(baseMacs, users, devices, description, css, template, outFile, poolMaxSize, readOnly, prompt);
        
            Console.WriteLine(success);
        }

		public static bool CheckUsers(string userFile, out ArrayList users)
		{
			users = new ArrayList();
			if(!File.Exists(userFile))
			{
				Console.WriteLine("User File does not exist.");
				return false;
			}
	
			using(FileStream stream = File.OpenRead(userFile))
			{
				using(StreamReader reader = new StreamReader(stream))
				{
					// read first line
					reader.ReadLine();
						
					while(reader.Peek() > -1)
					{
						string item = reader.ReadLine();

						if(item.IndexOf(",") > -1 || item.IndexOf("\t") > -1)
						{
							string[] bits = item.Split(new char[] {','});

							if(bits.Length == 7)
							{
								string user = bits[0];
								if(user != String.Empty)
								{
									users.Add(user);
								}
							}
						}
						else if(item != null && item != String.Empty)
						{
							item = item.Trim();
							if(item != String.Empty)
							{
								users.Add(item);
							}
						}
					}
				}
			}

			return true;
		}

		public static bool CheckDevices(string deviceFile, out ArrayList devices)
		{
			devices = new ArrayList();
			if(!File.Exists(deviceFile))
			{
				Console.WriteLine("Device File does not exist.");
				return false;
			}
	
			using(FileStream stream = File.OpenRead(deviceFile))
			{
				using(StreamReader reader = new StreamReader(stream))
				{
					while(reader.Peek() > -1)
					{
						string item = reader.ReadLine();

						if(item.StartsWith("username"))
						{
							continue;
						}


						if(item != null && item != String.Empty)
						{
							string[] bits = item.Split(new char[] {',', '\t'});
							if(bits.Length == 4)
							{
								string user = bits[0];
								string device = bits[1];
								string first = bits[2];
								string last = bits[3];
								if(user != null && user != string.Empty)
								{
									user = user.Trim();
								}

								if(user != String.Empty)
								{
									if(device != null && device != String.Empty)
									{
										device = device.Trim();
										devices.Add(new string[] {user, device, first, last});
									}
									else
									{
										Console.WriteLine("No device associated with user {0}", user);
										continue;
									}
								}
							}
							else if(bits.Length == 7)
							{
								string user = bits[0];
								string device = bits[1];
								string first = bits[5];
								string last = bits[6];
								if(user != null && user != string.Empty)
								{
									user = user.Trim();
								}

								if(user != String.Empty)
								{
									if(device != null && device != String.Empty)
									{
										device = device.Trim();
										devices.Add(new string[] {user, device, first, last});
									}
									else
									{
										Console.WriteLine("No device associated with user {0}", user);
										continue;
									}
								}
							}
							else
							{
								Console.WriteLine("Skipping line '{0}'", item);
							}
						}
					}
				}
			}

			return true;
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

		public static string[] GetArguments(CommandLineArguments parser, string arg, string defaultValue)
		{
			string[] foundValue;
			if(!parser.IsParamPresent(arg) && defaultValue == null)
			{
				Console.WriteLine("\nRequired Parameter '{0}' not present!\n", arg); 
				throw new ApplicationException("Stopping execution!");
			}
			else if(!parser.IsParamPresent(arg))
			{
				Console.WriteLine("Using default value '{0}' for parameter '{1}'", defaultValue, arg);
				foundValue = new string[] {defaultValue};
			}
			else
			{
				foundValue = parser[arg];
			}

			return foundValue;
		}
    }
}
