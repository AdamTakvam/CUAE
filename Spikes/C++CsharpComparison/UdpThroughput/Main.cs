using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Metreos.Utilities;

namespace UdpThroughput
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class MainClass
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if (args.Length < 2 ||
				(args[1] != "C++" && args[1] != "C#" && args[1] != "loopback"))
			{
				Console.WriteLine("Invalid command-line arguments.");
				Console.WriteLine("UdpThroughput <pps> <relay>");
				Console.WriteLine("<pps>   packets per second; 0-1000, >1000 means no throttling");
				Console.WriteLine("<relay> \"C++\", \"C#\", or \"loopback\"");
			}
			else
			{
				int packetsPerSecond = Convert.ToInt32(args[0]);
				if (packetsPerSecond > 1000)
				{
					interpacketDelayMs = 0;
				}
				else
				{
					interpacketDelayMs = 1000 / packetsPerSecond;
				}
				myIpAddress = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
				if (args[1] != "loopback")
				{
					// processController object must be instantiated before thread is
					// started that starts up the stack-server process.
					System.IO.FileInfo procFile = new System.IO.FileInfo(
						args[1] == "C++" ? Consts.CppProcessPath : Consts.CsharpProcessPath);
					if (!procFile.Exists)
					{
						throw new ConfigurationException("Process cannot be located.");
					}
					processController = new ProcessController(procFile);
					string processArgs = Consts.relayReceivePort.ToString() + " " +
						myIpAddress + " " + Consts.relaySendPort.ToString();
					processController.Start(processArgs, false, Consts.Window);
				}
				socketOut = new Socket(AddressFamily.InterNetwork,
					SocketType.Dgram, ProtocolType.Udp);
				socketIn = new Socket(AddressFamily.InterNetwork,
					SocketType.Dgram, ProtocolType.Udp);
				addressTo = new IPEndPoint(IPAddress.Parse(myIpAddress),
					Consts.relayReceivePort);
				addressFrom = new IPEndPoint(IPAddress.Any,
					args[1] == "loopback" ? Consts.relayReceivePort : Consts.relaySendPort);
				Console.WriteLine("Sending to {0} receiving from {1}",
					addressTo.ToString(), addressFrom.ToString());

				threadOut = new Thread(new ThreadStart(Send));
				threadOut.Start();
				threadIn = new Thread(new ThreadStart(Receive));
				threadIn.Start();

				Console.WriteLine("{0} process selected", args[1]);
				while (true)
				{
					Console.WriteLine("Press q and Enter to terminate test");
					if (Console.ReadLine() == "q")
					{
						Console.WriteLine("Shutting down. Please wait...");
						break;
					}
				}

				threadIn.Abort();
				threadOut.Abort();

				socketIn.Close();
				socketOut.Close();

				if (args[1] != "loopback")
				{
					processController.Stop(0);
				}
			}
		}

		private abstract class Consts
		{
			public const int packetsPerSample = 100000;
			public const int packetSize = 180;
			public const int relayReceivePort = 3000;
			public const int relaySendPort = 3002;
			public const string CppProcessPath = @"..\..\..\debug\C++Relay.exe";
			public const string CsharpProcessPath = @"..\..\..\CsharpRelay\bin\Debug\CsharpRelay.exe";
			public const bool Window = true;
		}

		private static string myIpAddress;

		private static ProcessController processController;

		private static Socket socketOut;
		private static Socket socketIn;
		private static Thread threadOut;
		private static Thread threadIn;
		private static IPEndPoint addressTo;
		private static IPEndPoint addressFrom;
		private static int interpacketDelayMs;

		private static void Send()
		{
			socketOut.Connect(addressTo);

			byte[] buffer = new byte[Consts.packetSize];

			// Wait for process and receive thread to get started up.
			Thread.Sleep(5000);

			int i = 0;
			while (true)
			{
				Array.Copy(BitConverter.GetBytes(i), 0, buffer, 0, 4);
				try
				{
					socketOut.Send(buffer);
				}
				catch (ThreadAbortException)
				{
					// (Do nothing.)
				}
				catch (Exception e)
				{
					Console.WriteLine("socket send error: {0}", e.ToString());
				}

				if (interpacketDelayMs != 0)
				{
					Thread.Sleep(interpacketDelayMs);
				}

				++i;
			}
		}

		private static void Receive()
		{
			socketIn.Bind(addressFrom);

			byte[] buffer = new byte[Consts.packetSize];
			int errors = 0;
			int packetsRead = 0;

			int i = 0;
			DateTime startTime = DateTime.Now;
			while (true)
			{
				try
				{
					int bytesRead = socketIn.Receive(buffer);
					if (bytesRead < 4)
					{
						Console.WriteLine("Too-small packet read ({0} bytes); ignored",
							bytesRead.ToString());
					}
					else
					{
						int j = BitConverter.ToInt32(buffer, 0);
						if (j != i)
						{
							Console.WriteLine("expected {0}, received {1}",
								i.ToString(), j.ToString());
							i = j;
							++errors;
						}

						if (packetsRead == Consts.packetsPerSample)
						{
							TimeSpan elapsedTime = DateTime.Now - startTime;
							Console.WriteLine("{0} usecs/packet",
								elapsedTime.Ticks * 10 / Consts.packetsPerSample);
							startTime = DateTime.Now;
							packetsRead = 0;
						}

						++i;
						++packetsRead;
					}
				}
				catch (ThreadAbortException)
				{
					// (Do nothing.)
				}
				catch (Exception e)
				{
					Console.WriteLine("socket receive error: {0}", e.ToString());
				}
			}
		}
	}
}
