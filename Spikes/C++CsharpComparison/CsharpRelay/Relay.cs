using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CsharpRelay
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Relay
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if (args.Length < 3)
			{
				Console.WriteLine("Invalid command-line arguments");
			}
			else
			{
				addressOut = new IPEndPoint(IPAddress.Parse(args[1]), Convert.ToInt32(args[2]));
				addressIn = new IPEndPoint(IPAddress.Any, Convert.ToInt32(args[0]));
				Console.WriteLine("Relaying from {0} to {1}",
					addressIn.ToString(), addressOut.ToString());

				socketOut = new Socket(AddressFamily.InterNetwork,
					SocketType.Dgram, ProtocolType.Udp);
				socketIn = new Socket(AddressFamily.InterNetwork,
					SocketType.Dgram, ProtocolType.Udp);

				thread = new Thread(new ThreadStart(RelayData));
				thread.Start();
			}
		}

		private static Socket socketOut;
		private static Socket socketIn;
		private static Thread thread;
		private static IPEndPoint addressOut;
		private static IPEndPoint addressIn;

		private static void RelayData()
		{
			socketOut.Connect(addressOut);
			socketIn.Bind(addressIn);

			byte[] buffer = new byte[1024];

			while (true)
			{
				int bytesRead = socketIn.Receive(buffer);
				if (bytesRead > 0)
				{
//					Console.WriteLine("{0}", BitConverter.ToInt32(buffer, 0));
					socketOut.Send(buffer, 0, bytesRead, 0);
				}
			}
		}
	}
}
