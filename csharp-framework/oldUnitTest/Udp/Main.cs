using System;
using System.Net;
using System.Threading;
using System.Diagnostics;
using Metreos.Core.IPC;
using Metreos.Core.IPC.Flatmaps;

namespace Metreos.Framework.UnitTest.Udp
{
	/// <summary>
	/// Tester for UDP client/server and flatmap
	/// </summary>
	class Tester
	{
		public const int buffertime = 50;

		[STAThread]
		static void Main(string[] args)
		{
			Tester tester = new Tester();

			bool success = true;

			if(args.Length == 3 && args[0] == "c")
			{
				string udpServer = Convert.ToString(args[1]);
				ushort port = Convert.ToUInt16(args[2]);
				Console.WriteLine("Run client with server={0}, port={1}", udpServer, port);
				success = tester.TestClient(udpServer, port);
			}
			else if (args.Length == 2 && args[0] == "s")
			{
				ushort port = Convert.ToUInt16(args[1]);
				Console.WriteLine("Run client server test with port={0}", port);
				success = tester.TestClientServer(port);
			}
			else 
			{
				Console.WriteLine("Usage:");
				Console.WriteLine("udp c server port");
				Console.WriteLine("udp s port");
			}

			Console.WriteLine();
			Console.WriteLine("Press any key to end");
			Console.Read();
		}

		private UDPFlatmapClient client;
		private UDPFlatmapServer server;
		private UDPFlatmapServer server1;

		public Tester() {}

		public bool TestClient(string server, ushort port)
		{
			int testcount = 0;
			bool success = true;

			client = new UDPFlatmapClient(false, server, port);
			success &= Test1();
			testcount++;

			if(!success) 
			{
				Console.WriteLine(testcount + " failed");
			}

			success &= Test2();
			testcount++;

			if(!success) 
			{
				Console.WriteLine(testcount + " failed");
			}

			client.Dispose();

			return success;				
		}

		public bool TestClientServer(ushort port)
		{
			int testcount = 0;
			bool success = true;

			server = new UDPFlatmapServer(true, "224.5.6.7", port);
			server.OnMessageReceived += new Metreos.Core.IPC.Flatmaps.UDPFlatmapServer.OnMessageReceivedDelegate(server_OnMessageReceived);
			
			server.Start();

			server1 = new UDPFlatmapServer(true, "224.5.6.7", port);
			server1.OnMessageReceived += new Metreos.Core.IPC.Flatmaps.UDPFlatmapServer.OnMessageReceivedDelegate(server1_OnMessageReceived);
			
			server1.Start();


			client = new UDPFlatmapClient(true, "224.5.6.7", port);

			success &= Test1();
			testcount++;

			if(!success) 
			{
				Console.WriteLine(testcount + " failed");
			}

			success &= Test2();
			testcount++;

			if(!success) 
			{
				Console.WriteLine(testcount + " failed");
			}

			client.Dispose();

			server.Stop();
			server1.Stop();

			server.Dispose();
			server1.Dispose();

			return success;
		}

		public bool Test1()
		{
			Thread.Sleep(buffertime);
			FlatmapList message = new FlatmapList();
			message.Add(2001, 0);
			return client.Write(2000, message);
		}

		public bool Test2()
		{
			bool written = false;

			for(int i = 0; i < 100; i++)
			{
				Thread.Sleep(buffertime);

				FlatmapList message = new FlatmapList();
				message.Add(2001, i + 1);

				written = client.Write(2000, message);
				if (!written)
				{
					break;
				}
			}

			return written;
		}

		public void server_OnMessageReceived(string receiveInterface, int messageType, FlatmapList message)
		{
			Console.WriteLine("Server: Interface: " + receiveInterface);
			Console.WriteLine("Server: Message received");
			Console.WriteLine("Server: MessageType: " + messageType);
			Console.WriteLine("Server: Message Count: " + (uint) message.Find(2001, 1).dataValue);
			Console.WriteLine();
		}

		public void server1_OnMessageReceived(string receiveInterface, int messageType, FlatmapList message)
		{
			Console.WriteLine("Server1: Interface: " + receiveInterface);
			Console.WriteLine("Server1: Message received");
			Console.WriteLine("Server1: MessageType: " + messageType);
			Console.WriteLine("Server1: Message Count: " + (uint) message.Find(2001, 1).dataValue);
			Console.WriteLine();
		}
	}
}
