using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.Runtime.Serialization;

namespace RemoteConsole
{
	class ConsoleViewer
	{
		[STAThread]
		static void Main(string[] args)
		{
			Console.WriteLine("Metreos Communications Environment Remote Console");
			Console.WriteLine("Copyright (C) Metreos Corporation 2004. All Rights Reserved.");
			Console.WriteLine();

			if(args.Length == 2)
			{
				int port = 0;
				try
				{
					port = int.Parse(args[1]);
				}
				catch {}

				if(port > 1024)
				{
					ConsoleViewer cv = new ConsoleViewer();
					cv.Start(args[0], port);
					return;
				}
			}
            
			Console.WriteLine("Error: You must specify an IPaddress and port to connect to");
			Console.WriteLine();
			Console.WriteLine("  Client <IP address> <port>");
		}

		private TcpClient client;
		private NetworkStream stream;
		private IFormatter formatter;
		private volatile bool shutdown = false;

		public ConsoleViewer()
		{
			formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
		}

		public void Start(string ipAddress, int remotePort)
		{
			IPAddress remoteIP;
			try
			{
				remoteIP = IPAddress.Parse(ipAddress);
			}
			catch
			{
				Console.WriteLine("Invalid IP Address: " + ipAddress);
				return;
			}

			client = new TcpClient();
			IPEndPoint remoteEP = new IPEndPoint(remoteIP, remotePort);
            
			try
			{
				client.Connect(remoteEP);
				stream = client.GetStream();
			}
			catch(Exception e)
			{
				Console.WriteLine("Cannot connect to: " + remoteEP);
				Console.WriteLine("Error: " + e.Message);
				return;
			}

			Thread listenThread = new Thread(new ThreadStart(ListenThread));
			listenThread.Start();

			Console.WriteLine("<Connected>");
			Console.WriteLine();
			Console.WriteLine("Press <Enter> to quit");
			
			while(Console.ReadLine() != "") {}

			shutdown = true;
			listenThread.Join();
		}

		private void ListenThread()
		{
			while(shutdown == false)
			{
				object dataObj = null;

				if(stream.DataAvailable)
				{
					lock(stream)
					{
						try
						{
							dataObj = formatter.Deserialize(stream);
						}
						catch
						{
							Console.WriteLine("-- Could not read data from network --");
						}
						stream.Flush();
					}

					if(dataObj == null) { continue; }

					if(dataObj is string)
					{
						Console.WriteLine(dataObj);
					}

					dataObj = null;
				}
				else
				{
					Thread.Sleep(20);
				}
			}
		}
	}
}
