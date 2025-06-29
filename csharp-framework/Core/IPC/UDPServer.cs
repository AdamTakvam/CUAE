using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Metreos.Core.IPC
{
	/// <summary>
	/// Summary description for UDPServer.
	/// </summary>
	public abstract class UDPServer : IDisposable
	{
		private Socket serverSocket;
		private IPEndPoint ep;
		private ushort port;
        private bool loopback;
		private const int BUFFER_SIZE = 1024;
		private byte[] buffer;
		public Thread serverThread;
		private volatile bool serverStopped;
		private readonly object syncRoot;
		private bool broadcast;
		private string broadcastIP;

		protected abstract void OnPayloadReceived(string receiveInterface, byte[] payload);

		// Multicast IP addresses are within the Class D range of 224.0.0.0-239.255.255.255 
		protected UDPServer(bool broadcast, string broadcastIP, ushort port, bool loopback)
		{
			this.broadcast = broadcast;
			this.broadcastIP = broadcastIP;
			this.port = port;
            this.loopback = loopback;
			this.syncRoot = new object();
			this.serverStopped = false;
		}

		/// <summary>
		/// Start server thread
		/// </summary>
		public void Start()
		{
			serverThread = new Thread(new ThreadStart(StartReceiveFromClients));
            serverThread.Name = "UDP Server receive thread";
            serverThread.IsBackground = true;
			serverThread.Start();			
		}

		/// <summary>
		/// Stop server thread
		/// </summary>
		public void Stop()
		{
			lock(syncRoot)
			{
				serverStopped = true;
			}
		}
		
		/// <summary>
		/// Verify if thread is still alive
		/// </summary>
		public bool IsServerStopped
		{
			get
			{
				lock(syncRoot) { return this.serverStopped; }
			}
		}

		/// <summary>
		/// Worker for server thread
		/// </summary>
		public void StartReceiveFromClients()
		{
			serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

			if (broadcast)
			{
				serverSocket.SetSocketOption(SocketOptionLevel.Socket,
					SocketOptionName.ReuseAddress,
					1);

                IPAddress _interface = this.loopback ? IPAddress.Loopback : IPAddress.Any;
				ep = new IPEndPoint(_interface, this.port);
				serverSocket.Bind(ep);

				IPAddress ip = IPAddress.Parse(this.broadcastIP);
				
				serverSocket.SetSocketOption(SocketOptionLevel.IP,
					SocketOptionName.AddMembership,
					new MulticastOption(ip, IPAddress.Any));
			}
			else
			{
				ep = new IPEndPoint(IPAddress.Any, this.port);
				serverSocket.Bind(ep);
			}

			EndPoint tempRemoteEP = (EndPoint)ep;
			while (!IsServerStopped)
			{
				int recv = 0;
				buffer = new byte[BUFFER_SIZE];
				recv = serverSocket.ReceiveFrom(buffer, ref tempRemoteEP);

				string receiveInterface;

				if (recv > 0)
				{
					if (tempRemoteEP != null)
					{
						SocketAddress sa = tempRemoteEP.Serialize();
						receiveInterface = sa[4] + "." + sa[5] + "." + sa[6] + "." + sa[7];
					} 
					else
					{
						receiveInterface = "127.0.0.1";
					}

					// Received data
					try
					{
						byte[] data = new byte[recv];
						Array.Copy(buffer, 0, data, 0, recv);
						OnPayloadReceived(receiveInterface, data);
					}
					catch
					{
						serverSocket.Close();
						serverSocket = null;
						break;
					}

					buffer = null;
				}
			}
		}
	
		public void Dispose()
		{
			if (serverThread.IsAlive)
			{
				serverThread.Abort();
				serverThread = null;
			}

			if (serverSocket != null)
			{
				serverSocket.Close();
				serverSocket = null;
			}
		}
	}
}
