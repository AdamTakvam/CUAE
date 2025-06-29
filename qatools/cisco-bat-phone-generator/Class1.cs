using System;
using System.IO;
using System.Collections;

namespace CiscoBatPhoneGenerator
{
	class Class1
	{
        class Consts
        {
            public const string NumLines = "NUMBER OF LINES";
            public const string MacAddress = "MAC ADDRESS";
            public const string Description = "DESCRIPTION";
            public const string DirNum = "DIRECTORY NUMBER";
        }

        private static StreamWriter writer;
        private static ArrayList numbers = null;

        [STAThread]
        static void Main(string[] args)
        {
            int baseMacPos  = 0;
            int descripPos  = 1;
            int baseDnPos   = 2;
            int skipPos     = args.Length == 5 ? -1 : 3;
            int numPhonePos = args.Length == 5 ? 3 : 4;
            int outPos      = args.Length == 5 ? 4 : 5;

            if(args.Length != 5 && args.Length != 6)
            {
                Console.WriteLine("Usage: cbpg.exe <Base MAC Address> <Description> <Base DN OR Filepath> <Increment Amount[Optional]> <Number of Phones> <outputFile>");
                return;
            }

            if(args[0].Length != 12)
            {
                Console.WriteLine("Error: '{0}' is not a valid MAC address.\nMust of of the form: 'AABBCCDDEEFF'", args[0]);
                return;
            }

            // BasePnPos is now an either/or behavior. If one passes in a filename, then 
            // we will attempt to parse this file for directory numbers as returned by BAT phone report

            if(File.Exists(args[baseDnPos]))
            {
                try
                {
                    // Open up and parse file to determine if it is a phone report
                    numbers = ParseFile(args[baseDnPos]);
                }
                catch
                {
                    Console.WriteLine("Error: '{0}' could not be read", args[baseDnPos]);
                    return;
                }
            }
            else
            {
                try
                {
                    uint.Parse(args[baseDnPos]);
                }
                catch
                {
                    Console.WriteLine("Error: '{0}' is not a valid base DN.\nMust be a non-zero integer.", args[baseDnPos]);
                    return;
                }
            }

            try
            {
                uint.Parse(args[numPhonePos]);
            }
            catch
            {
                Console.WriteLine("Error: '{0}' is not a valid number of phones.\nMust be a non-zero integer.", args[numPhonePos]);
                return;
            }        

            try
            {
                writer = new StreamWriter(args[outPos], false);
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: Failed to open the output file '{0}'\nException: {1}", args[outPos], e.ToString());
                return;
            }

            int skipAmount = 1;
            if(skipPos != -1)
            {
                skipAmount = int.Parse(args[skipPos]);
            }

            WriteHeader();

            if(numbers != null) // this is operating in file-based mode
            {
                WritePhoneBatch(1, args[baseMacPos], args[descripPos], numbers);
            }
            else
            {
                WritePhoneBatch(1, args[baseMacPos], args[descripPos], int.Parse(args[baseDnPos]), int.Parse(args[numPhonePos]), skipAmount);
            }
            writer.Close();
		}

        static void WriteHeader()
        {
            writer.WriteLine("{0},{1},{2},{3}", Consts.NumLines, Consts.MacAddress, Consts.Description, Consts.DirNum);
        }

        static void WritePhoneBatch(int numLines, string startMacAddress, string description, int startDirNum, int numPhones, int incrementAmount)
        {
            long curMacAddress = Convert.ToInt64(startMacAddress, 16);
            int curDirNum = startDirNum;
            
            for(int i = 0; i < numPhones; i++)
            {
                WritePhone(numLines, 
                    curMacAddress++,
                    String.Format("{0} {1}", description, curDirNum), 
                    curDirNum);

                curDirNum = incrementAmount + curDirNum; 
            }
        }

        static void WritePhoneBatch(int numLines, string startMacAddress, string description, ArrayList foundNumbers)
        {
            long curMacAddress = Convert.ToInt64(startMacAddress, 16);
            
            foreach(string number in foundNumbers)
            {
                WritePhone(numLines, 
                    curMacAddress++,
                    description, 
                    int.Parse(number));
            }
        }

        static void WritePhone(int numLines, long macAddress, string description, int dirNum)
        {
            writer.WriteLine("{0},{1:X12},{2},{3}", numLines, macAddress, description, dirNum);
        }

        static ArrayList ParseFile(string path)
        {
            ArrayList list = new ArrayList();
            using(FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                StreamReader reader = new StreamReader(stream);
                
                while(reader.Peek() > -1)
                {
                    string line = reader.ReadLine();
                    if(line != null) line = line.Trim();
                    try
                    {
                        int.Parse(line);
                        list.Add(line);
                    }
                    catch {}
                }
            }

            if(list.Count == 0) 
            {
                return null;
            }
            else
            {
                return list;
            }
        }
	}
}
