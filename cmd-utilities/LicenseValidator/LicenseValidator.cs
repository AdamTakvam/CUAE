using System;
using System.IO;
using Metreos.LicensingFramework;

namespace LicenseValidator
{
    public abstract class Constants
    {
        public const string NO_SUCH_FILE_ERROR = "The file does not exist.";
    }

    class LicenseValidator
    {
        static void PrintHelp()
        {
            return;
        }
 
        static void Main(string[] args)
        {
            int returnCode = -1;

            if (args.Length == 0)
            {
                PrintHelp();
                Environment.Exit(1);
            }

            foreach (string licFile in args)
            {
                try
                {
                    if (!File.Exists(licFile))
                    {
                        Console.WriteLine("{0} : {1}", licFile, Constants.NO_SUCH_FILE_ERROR);
                        Environment.Exit(1);
                    }
                    else
                    {
                        returnCode = LicenseUtilities.ValidateLicenseFile(licFile);
                    }
                }
                catch
                { }
            }

            if (returnCode == 0)
            {
                Console.WriteLine("The license file is valid.");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("{0} The license file is not valid.",returnCode);
                Environment.Exit(1);
            }
        }
    }
}

