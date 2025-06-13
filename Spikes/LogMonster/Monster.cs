using System;
using System.IO;
using System.Collections.Specialized;

namespace LogMonster
{
	class Class1
	{
        [STAThread]
        static void Main(string[] args)
        {
            Class1 c = new Class1();
            c.Go();
        }

        private StringCollection openCalls;

        public Class1()
        {
            this.openCalls = new StringCollection();
        }

        public void Go()
        {
            DirectoryInfo dInfo = new DirectoryInfo("X:\\build\\appserver\\logs");
            if(!dInfo.Exists)
            {
                Console.WriteLine("Invalid directory");
                return;
            }

            int totalCalls = 0;

            foreach(FileInfo fInfo in dInfo.GetFiles())
            {
                Console.WriteLine("Inspecting: " + fInfo.Name);

                try
                {
                    StreamReader sr = new StreamReader(fInfo.FullName);
                    if(sr == null)
                        continue;

                    string line = null;
                    while((line = sr.ReadLine()) != null)
                    {
                        string callId = GetCallStart(line);
                        if(callId != null)
                        {
                            totalCalls++;
                            openCalls.Add(callId.Trim());
                        }
                        else
                        {
                            callId = GetCallEnd(line);
                            if(callId != null)
                            {
                                openCalls.Remove(callId.Trim());
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("Error reading {0}: {1}", fInfo.Name, e.Message);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Lost Calls:");
            Console.WriteLine("-----------");

            foreach(string callId in openCalls)
            {
                Console.WriteLine(callId);
            }

            Console.WriteLine();
            Console.WriteLine("Total number of calls: " + totalCalls);
            Console.WriteLine("Number of lost calls: " + openCalls.Count);
            Console.WriteLine();
            Console.WriteLine("Press <enter> to quit");
            Console.ReadLine();
        }

        private string GetCallStart(string line)
        {
            int p = line.IndexOf("outbound call");
            if(p == -1)
            {
                p = line.IndexOf("inbound call");
                if(p != -1)
                    p += 14;
            }
            else
            {
                p += 15;
            }

            if(p != -1)
            {
                int q = line.IndexOf(")", p);
                if(q != -1)
                    return line.Substring(p, q-p);
            }
            return null;
        }

        private string GetCallEnd(string line)
        {
            int p = line.IndexOf("has ended");
            if(p != -1)
            {
                int q = p;
                p = line.LastIndexOf(":");
                if(p != -1)
                {
                    p++;q--;
                    return line.Substring(p, q-p);
                }
            }
            return null;
        }
	}
}
