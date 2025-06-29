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
			string ccmIp = args[0];
            string ccmUser = args[1];
            string ccmPass = args[2];
            
            string deviceStartString = args[3];
            int deviceCount = int.Parse(args[4]);
            string accountPrefix = args[5];
            int maxAxlWrite = int.Parse(args[6]);

            ArrayList mces = new ArrayList();
            for(int i = 7; i < args.Length; i++)
            {
                mces.Add(args[i]);
            }

            if(deviceStartString.StartsWith("SEP"))
            {
                deviceStartString = deviceStartString.Substring(3);
            }

            long deviceStart = long.Parse(deviceStartString, System.Globalization.NumberStyles.HexNumber); 


            AXLAPIService service = new AXLAPIService(ccmIp, ccmUser, ccmPass);

            Worker worker = new Worker(service, maxAxlWrite);
            bool success = worker.Generate(deviceStart, deviceCount, accountPrefix, mces);
        
            Console.WriteLine(success);
        }
	}
}
