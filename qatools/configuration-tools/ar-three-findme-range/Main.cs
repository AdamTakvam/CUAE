using System;
using System.Collections;
using Metreos.AxlSoap413;

namespace AROneExtern
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{         
            string deviceStartString = args[0];
            int deviceCount = int.Parse(args[1]);
            string accountPrefix = args[2];
            string findmePrefix = args[3];
            int maxAxlWrite = int.Parse(args[4]);

            int deviceStart = int.Parse(deviceStartString); 
        
            Worker worker = new Worker(maxAxlWrite);
            bool success = worker.Generate(deviceStart, deviceCount, accountPrefix == "NONE" ? "" : accountPrefix, findmePrefix == "NONE" ? "" : findmePrefix);
        
            Console.WriteLine(success);
        }
	}
}
