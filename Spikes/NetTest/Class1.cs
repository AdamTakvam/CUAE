using System;
using System.Net;

using Metreos.Utilities;

namespace NetTest
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
            DateTime startTime = DateTime.Now;

            IPAddress[] addrs = IpUtility.GetIPAddresses();

            foreach(IPAddress addr in addrs)
            {
                Console.WriteLine("Local address: " + addr);
            }

            Console.WriteLine("\nDone: {0} ms", DateTime.Now.Subtract(startTime).TotalMilliseconds);
            Console.ReadLine();
		}
	}
}
