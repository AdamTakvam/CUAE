using System;
using System.Net;
using System.Threading;

namespace csipctest
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
            CsIpcClient client = new CsIpcClient();
            while (true)
            {
                if (client.State == CsIpcClient.IPCState.Disconnected)
                {
                    IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9530); 
                    client.Start(ipe);
                }
                Console.WriteLine("Enter 'q' to quit");
                string userInput = Console.ReadLine();
                if(0 == String.Compare(userInput, "q", true))
                {
                    break;
                }
            }
            client.Stop();
		}
	}
}
