using System;

using Metreos.Utilities;
using Metreos.Interfaces;

namespace Metreos.CypherUtil
{
	/// <summary>
	/// Command-line utility for encrypting and decrypting strings.
	/// </summary>
	class Cypher
	{
		[STAThread]
		static void Main(string[] args)
		{
            IConsoleApps.PrintHeaderText("Cypher Utility");

            CommandLineArguments clargs = new CommandLineArguments(args);

            if(clargs.IsParamPresent(ICypher.PARAM_HELP))
            {
                PrintHelp();
                return;
            }

            if(clargs.IsParamPresent(ICypher.PARAM_DEBUG))
            {
                Console.WriteLine("Attach the debugger and press Enter to continue");
                Console.ReadLine();
            }

            bool verify = clargs.IsParamPresent(ICypher.PARAM_VERIFY);

            string key = clargs.GetSingleParam(ICypher.PARAM_KEY);
            key = key == String.Empty ? null : key;

            if(key == null)
            {
                PrintHelp("No key specified");
                return;
            }

            if(key.Length != 8 && key.Length != 12 && key.Length != 16) 
            {
                PrintHelp("Key must be of 8, 12, or 16 characters");
                return;
            }

            string encryptStr = clargs.GetSingleParam(ICypher.PARAM_ENCRYPT);
            string decryptStr = clargs.GetSingleParam(ICypher.PARAM_DECRYPT);

            encryptStr = encryptStr == String.Empty ? null : encryptStr;
            decryptStr = decryptStr == String.Empty ? null : decryptStr;

            if(encryptStr == null && decryptStr == null)
            {
                PrintHelp("No operation specified");
                return;
            }

            if(encryptStr != null && decryptStr != null)
            {
                PrintHelp("You cannot specify both encrypt and decrypt parameters");
                return;
            }

            if(encryptStr != null)
            {
                string encB64Str = Security.Aes.EncryptB64(encryptStr, key);
                Console.WriteLine("Result: " + encB64Str);

                // Sanity check
                if(verify)
                    Console.WriteLine("Verify: " + Security.Aes.DecryptB64(encB64Str, key));
            }
            else
            {
                Console.WriteLine("Result: " + Security.Aes.DecryptB64(decryptStr, key));
            }
		}

        public static void PrintHelp()
        {
            PrintHelp(null);
        }

        public static void PrintHelp(string error)
        {            
            if(error != null)
            {
                Console.WriteLine("Error: " + error);
                Console.WriteLine();
            }

            Console.WriteLine("Usage: CYPHER.EXE [{0}] [{1} | {2}] {3} [{4}]", 
                ICypher.PARAM_HELP_HELP, ICypher.PARAM_ENCRYPT_HELP, ICypher.PARAM_DECRYPT_HELP,
                ICypher.PARAM_KEY_HELP, ICypher.PARAM_VERIFY_HELP);
            Console.WriteLine();
            Console.WriteLine("Required Parameters:");
            Console.WriteLine("  {0,-20} String to encrypt", ICypher.PARAM_ENCRYPT_HELP);
            Console.WriteLine("  {0,-20} String to decrypt (Base64 encoded)", ICypher.PARAM_DECRYPT_HELP);
            Console.WriteLine("  {0,-20} \"Secret\" key", ICypher.PARAM_KEY_HELP);
            Console.WriteLine();
            Console.WriteLine("Optional Parameters:");
            Console.WriteLine("  {0,-20} Print this help screen", ICypher.PARAM_HELP_HELP);
            Console.WriteLine("  {0,-20} Decrypts newly-encrypted value to verify cypher integrity", ICypher.PARAM_VERIFY_HELP);
            Console.WriteLine();
            Console.WriteLine("Note:");
            Console.WriteLine("  The result of the operation will be a Base64-encoded string written to stdout.");
        }
	}
}
