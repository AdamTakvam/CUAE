using System;
using System.IO;
using System.Collections;
using Metreos.AxlSoap413;
using Metreos.Utilities;

namespace devicecreator
{
    class Class1
    {
        public const string mysqlUserArg = "u";
        public const string mysqlPassArg = "p";
		public const string usersArg = "users";
		public const string outFileArg = "o";
		public const string outFileDefault = "results.txt";

        public static string Usage = String.Format(@"
Arguments:
Name        Arg       Default
-----------------------------
<USER FILE> {0}         -
<MYSQLUSER> {1}			-
<MYSQLPASS> {2}         -
<OUT FILE>  {3}         {4}", 
			usersArg,
			mysqlUserArg,
			mysqlPassArg,
			outFileArg, outFileDefault);

        [STAThread]
        static void Main(string[] args)
        {
            CommandLineArguments parser = new CommandLineArguments(args);
            
            if(args.Length == 0)
            {
                Console.WriteLine(Usage);
                return;
            }

			string usersFile = GetArgument(parser, usersArg, null);
			string mysqlUser = GetArgument(parser, mysqlUserArg, null); 
			string mysqlPass = GetArgument(parser, mysqlPassArg, null);
			string outFile = GetArgument(parser, outFileArg, outFileDefault);

			ArrayList users = null;

			CheckUsers(usersFile, out users);

			CheckDupsInUsers(users);

            Console.WriteLine("\nStarting...\n\n");
			Worker worker = new Worker();
            bool success = worker.Generate(users, mysqlUser, mysqlPass, outFile);
        
            Console.WriteLine(success);
        }

		public static void CheckDupsInUsers(ArrayList users)
		{
			bool notFoundDup = true;
			for(int i = 0; i < users.Count; i++)
			{
				string xpid					= (users[i] as string[])[3];

				for(int j = 0; j < users.Count; j++)
				{
					if(i != j)
					{
						string otherXpid		= (users[j] as string[])[3];

						if(String.Compare(xpid, otherXpid, true) == 0)
						{
							notFoundDup &= false;
							Console.WriteLine("Duplicate username: {0}", xpid);
						}
					}
				}
			}

			for(int i = 0; i < users.Count; i++)
			{
				string phoneDeviceName		= (users[i] as string[])[8];

				for(int j = 0; j < users.Count; j++)
				{
					if(i != j)
					{
						string otherPhoneDeviceName		= (users[j] as string[])[8];

						if(String.Compare(phoneDeviceName, otherPhoneDeviceName, true) == 0)
						{
							notFoundDup &= false;
							Console.WriteLine("Duplicate devicename: {0}", phoneDeviceName);
						}
					}
				}
			}

			for(int i = 0; i < users.Count; i++)
			{
				string accountCode			= (users[i] as string[])[5];

				for(int j = 0; j < users.Count; j++)
				{
					if(i != j)
					{
						string otherAccountCode		= (users[j] as string[])[5];

						if(String.Compare(accountCode, otherAccountCode, true) == 0)
						{
							notFoundDup &= false;
							Console.WriteLine("Duplicate Account Code: {0}", accountCode);
						}
					}
				}
			}

			if(!notFoundDup)
			{
				Environment.Exit(-1);
			}

		}

		public static void CheckUsers(string userFile, out ArrayList users)
		{
			//Firstname, lastname, email, xpid, password=lehman, AccountCode = DN, Pin = DN, Primary Desk Phone, Device Name = the user Desk SEPxxxxxxxxxxxx, FindmeNumber name = AR Dummy, 11111, Disable

			//Wai, Lee, wlee4@lehman.com. Wlee4, lehman, 66984, 66984, Primary Desk Phone, SEP00036BC38221, AR Dummy, 11111, Disable, US/Eastern
			// Michael, Call, mcall@lehman.com. Mcall, lehman, 66001, 66001, Primary Desk Phone, SEP00036BC38229. AR Dummy, 11111, Disable, US/Eastern 


			users = new ArrayList();
			if(!File.Exists(userFile))
			{
				Console.WriteLine("User File does not exist at {0}.", userFile);
				Environment.Exit(-1);
			}
	
			using(FileStream stream = File.OpenRead(userFile))
			{
				using(StreamReader reader = new StreamReader(stream))
				{
					// read first line
					//reader.ReadLine();
						
					int count = 0;
					while(reader.Peek() > -1)
					{
						count++;
						string item = reader.ReadLine();

						if(item.IndexOf(",") > -1)
						{
							string[] bits = item.Split(new char[] {','});

							if(bits != null && bits.Length == 13)
							{
								// Validate a value exists for all fields
								string firstname			= ParseFieldAndValidate(bits, 0, "First Name", count);
								string lastname				= ParseFieldAndValidate(bits, 1, "Last Name", count);
								string email				= ParseFieldAndValidate(bits, 2, "Email Address", count);
								string xpid					= ParseFieldAndValidate(bits, 3, "XPID", count);
								string password				= ParseFieldAndValidate(bits, 4, "Password", count);
								string accountCode			= ParseFieldAndValidate(bits, 5, "Account Code", count);
								string pin					= ParseFieldAndValidate(bits, 6, "Pin", count);
								string phoneDisplayName		= ParseFieldAndValidate(bits, 7, "Phone Display Name", count);
								string phoneDeviceName		= ParseFieldAndValidate(bits, 8, "Phone Device Name", count);
								string findMeDisplayName	= ParseFieldAndValidate(bits, 9, "FindMe Display Name", count);
								string findMeNumber			= ParseFieldAndValidate(bits, 10, "FindMe Number", count);
								string findMeState			= ParseFieldAndValidate(bits, 11, "FindMe State", count);
								string timeZone				= ParseFieldAndValidate(bits, 12, "Timezone", count);

								// Validate numeric fields
								ValidateNumeric(accountCode, 5, "Account Code", count);
								ValidateNumeric(pin, 6, "Pin", count);
								ValidateNumeric(findMeNumber, 10, "FindMe Number", count);

								// Validate that the end field is always the phrase 'Disable'
								if(String.Compare("Disable", findMeState, true) != 0)
								{
									Console.WriteLine("FindMe State not set to 'Disable' at row index: " + count);
									Environment.Exit(-1);
								}

								// Validate that account code == pin
								if(String.Compare(accountCode, pin, true) != 0)
								{
									Console.WriteLine("Pin and AccountCode are not equivalent at row index: " + count);
									Environment.Exit(-1);
								}

								// Validate wel-formed Phone Devicename by checking for prepended SEP
								if(phoneDeviceName.StartsWith("SEP") == false)
								{
									Console.WriteLine("Phone Device Name is not prepended 'SEP' at row index: " + count);
									Environment.Exit(-1);
								}

								// Validate wel-formed Phone Devicename by checking for hexidecimal only
								string phoneDeviceNameHex = phoneDeviceName.Substring(3);

								try
								{
									long.Parse(phoneDeviceNameHex, System.Globalization.NumberStyles.HexNumber); 
								}
								catch
								{
									Console.WriteLine("Phone Device Name is not hexidecimal at row index: " + count);
									Environment.Exit(-1);
								}


								// all checks out--add row
								users.Add(new string[] {
														   firstname,
														   lastname,
														   email,
														   xpid,
														   password,
														   accountCode,
														   pin,
														   phoneDisplayName,
														   phoneDeviceName,
														   findMeDisplayName,
														   findMeNumber,
														   findMeState,
														   timeZone
													   });
							}
						}
						else 
						{
							Console.WriteLine("One or more rows do not have 13 columns");
							Environment.Exit(-1);
						}
					}
				}
			}
		}

		public static string ParseFieldAndValidate(string[] bits, int fieldIndex, string fieldDisplayName, int rowIndex)
		{
			string field = bits[fieldIndex];

			if(field != null)
			{
				field = field.Trim();
				if(field == String.Empty)
				{
					Console.WriteLine("Unable to parse field.  FieldName: {0}, FieldIndex: {1}, RowIndex: {2}", fieldDisplayName, fieldIndex, rowIndex);
					Environment.Exit(-1);
				}
			}

			return field;
		}

		public static void ValidateNumeric(string fieldValue, int fieldIndex, string fieldDisplayName, int rowIndex)
		{
			try
			{
				int.Parse(fieldValue);
			}
			catch
			{
				Console.WriteLine("Unable to parse field as numeric.  FieldName: {0}, FieldIndex: {1}, RowIndex: {2}", fieldDisplayName, fieldIndex, rowIndex);
				Environment.Exit(-1);
			}
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
