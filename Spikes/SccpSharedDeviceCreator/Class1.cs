using System;
using Metreos.AxlSoap413;

namespace SharedLineCreator
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
            string ccmIP                = args[0];
            string ccmUsername          = args[1];
            string ccmPassword          = args[2];
            string devicenameToShare    = args[3];
            string devicenameBaseFrom   = args[4];
            string devicenameCount      = args[5];
            
//            string ccmIP = "10.1.14.25";
//            string ccmUsername = "Administrator";
//            string ccmPassword = "metreos";

			// Add phone device
            Metreos.AxlSoap413.AXLAPIService service = new AXLAPIService(ccmIP, ccmUsername, ccmPassword);

            SccpPhoneManipulator mani = new SccpPhoneManipulator(service);

            bool success = mani.ShareLineWithDeviceRange(devicenameToShare, devicenameBaseFrom, devicenameCount);
            
            Console.WriteLine(success);
        }        
	}
}
