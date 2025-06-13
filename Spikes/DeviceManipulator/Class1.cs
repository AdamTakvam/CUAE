using System;
using Metreos.AxlSoap413;

namespace DeviceManipulator
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
            if(args.Length != 5)
            {
                Console.WriteLine(@"

rmphone ccmIp ccmUser ccmPass maxAXLWriteRate searchString
----------------------------------------------------------------
All arguments are mandatory.  
-- maxAXLWriteRate = 20 by default on most CCMs, 60 is upper safe limit.  
    Only put 60 if configured as so on CCM. If you put higher than configured
    on CCM you will have failed deletes.
-- searchString = string to delete on.  Is SQL-specific. 
    For instance, this will delete most every phone:
    'SEP%'  (no quotes)");

            }

            string ccmIp = args[0];
            string ccmUser = args[1];
            string ccmPass = args[2];
            int maxAxlWriteRate = int.Parse(args[3]);
            string searchString = args[4];

            AXLAPIService service = new AXLAPIService(ccmIp, ccmUser, ccmPass);
			DeviceManipulator ma = new DeviceManipulator(service);
            
            bool success = ma.RemoveAllDevices(maxAxlWriteRate, searchString);
        
            Console.WriteLine(success);
        }
	}
}
