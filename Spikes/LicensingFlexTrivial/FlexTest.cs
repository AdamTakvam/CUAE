using System;
using System.Collections.Generic;
using System.Text;

namespace LicensingFlexTrivial
{
    class FlexTest
    {
        static void Main(string[] args)
        {
            if(args.Length != 1)
            {
                PrintHelp();
                System.Environment.Exit(0);
            }

            switch(args[0])
            {
                case "-o":
                    Test1Feature1Line1Instance();
                    break;
                case "-m":
                    Test1Feature1LineMaxInstances();
                    break;
                case "-t":
                    Test1Feature1Line2part();
                    break;
                case "-2":
                    Test2Feature1Instance();
                    break;
                case "-n":
                    TestNoCheckIn();
                    break;
                default:
                    Test1Feature1Line1Instance();
                    break;
            }
        }

        public static void PrintHelp()
        {
            Console.WriteLine("Specify only one argument.\nCommand line args:");
            Console.WriteLine("-o : checks out one instance of one feature");
            Console.WriteLine("-m : checks out the maximum number allowed of one feature");
            Console.WriteLine("-t : perform a check out of x instances, followed by a checkout of y instances");
            Console.WriteLine("-2 : checks out one instance of each one of two features");
            Console.WriteLine("-n : performs a checkout, but not a checkin");
        }

        public static void Test1Feature1Line1Instance()
        {
            Console.WriteLine("Checking out one instance of one feature.");
            uint numInstances = CheckOut("Instances", "1.0", "@localhost");
            Console.WriteLine("Checked out instances:  {0}", numInstances);
            Console.WriteLine("Press 'Enter' to continue...");
            Console.ReadLine();
            Console.WriteLine("Calling 'lt_checkin()'");
            CheckIn();
        }

        public static void Test1Feature1LineMaxInstances()
        {
            Console.WriteLine("Checking out as many instances as allowed.");
            uint numInstances = CheckOut("Instances", "1.0", "@localhost", (uint) 10000);
            Console.WriteLine("Checked out instances:  {0}", numInstances);
            Console.WriteLine("Press 'Enter' to continue...");
            Console.ReadLine();
            Console.WriteLine("Calling 'lt_checkin()'");
            CheckIn();
        }

        public static void Test1Feature1Line2part()
        {
            Console.WriteLine("Two part checkout.");
            Console.Write("Number of instances for first checkout: ");
            string numFirst = Console.ReadLine();
            Console.Write("Number of instances for second checkout: ");
            string numSecond = Console.ReadLine();

            Console.WriteLine("Performing first checkout.");
            uint numInstances = CheckOut("Instances", "1.0", "@localhost", uint.Parse(numFirst));
            Console.WriteLine("Checked out instances:  {0}", numInstances);
            Console.WriteLine("Press 'Enter' to continue...");

            Console.ReadLine();
            Console.WriteLine("Performing second checkout.");
            uint numInstances2 = CheckOut("Instances", "1.0", "@localhost", uint.Parse(numSecond));
            Console.WriteLine("Checked out instances:  {0}", numInstances2);
            Console.WriteLine("Total number of instances checked out: {0}", numInstances+numInstances2);
            Console.WriteLine("Press 'Enter' to continue...");

            Console.ReadLine();
            Console.WriteLine("Calling 'lt_checkin()'");
            CheckIn();
        }

        public static void TestNoCheckIn()
        {
            Console.WriteLine("Testing to see what happens when we do a checkout w/ doing a checkin.");
            Console.Write("Number of instances for checkout: ");
            string numFirst = Console.ReadLine();

            Console.WriteLine("Performing first checkout.");
            uint numInstances = CheckOut("Instances", "1.0", "@localhost", uint.Parse(numFirst));
            Console.WriteLine("Checked out instances:  {0}", numInstances);
            Console.WriteLine("Press 'Enter' to continue...");
            Console.ReadLine();
            Console.WriteLine("Exiting without performing lt_checkin, check lmgrd log to see what happens.");
        }

        public static void Test2Feature1Instance()
        {
            Console.WriteLine("Checking out one instance of each one of two features");
            Console.WriteLine("Checking out feature 1...");
            uint numInstances = CheckOut("Instances", "1.0", "@localhost", (uint) 1);
            Console.WriteLine("Checked out instances of feature 1:  {0}", numInstances);
            Console.WriteLine("Press 'Enter' to continue...");
            Console.ReadLine();

            Console.WriteLine("Checking out feature 2...");
            uint numInstances2 = CheckOut("Mode", "1.0", "@localhost", (uint) 1);
            Console.WriteLine("Checked out instances of feature 2:  {0}", numInstances2);
            Console.WriteLine("Press 'Enter' to continue...");
            Console.ReadLine();

            Console.WriteLine("Calling 'lt_checkin()'");
            CheckIn();
        }

        public static uint CheckOut(string feature, string version, string licensePath)
        {
            return CheckOut(feature, version, licensePath, (uint) 1);
        }

        public static uint CheckOut(string feature, string version, string licensePath, uint numberToCheckOut)
        {
            int errorCode = 0;
            uint numberOfLicenses = 0;

            // error code of '0' designates success

            while((numberOfLicenses < numberToCheckOut) && ((errorCode = FLEXlm.lt_checkout(FLEXlm.LM_RESTRICTIVE, feature, version, licensePath)) == 0))
            {
                numberOfLicenses++;
            }

            if(numberOfLicenses == 0)
                Console.WriteLine("lt_checkout failed to check out any instances with error code: {0}", errorCode);

            return numberOfLicenses;
        }

        public static void CheckIn()
        {
            FLEXlm.lt_checkin();
        }
    }
}
