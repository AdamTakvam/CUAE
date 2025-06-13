using System;
using Metreos.AxlSoap413;

namespace H323SharedLineCreator
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
            string ccmIP = args[0];
            string ccmUsername = args[1];
            string ccmPassword = args[2];
            string h323PhoneName = args[3];
            string devicenameStart = args[4];
            string devicenameCount = args[5];
            
//            string ccmIP = "10.1.14.25";
//            string ccmUsername = "Administrator";
//            string ccmPassword = "metreos";

			// Add phone device
            Metreos.AxlSoap413.AXLAPIService service = new AXLAPIService(ccmIP, ccmUsername, ccmPassword);

            H323PhoneManipulator mani = new H323PhoneManipulator(service, h323PhoneName);

            bool success = mani.ShareLineWithDeviceRange(devicenameStart, devicenameCount);
            
            Console.WriteLine(success);
        }

        
	}
}
