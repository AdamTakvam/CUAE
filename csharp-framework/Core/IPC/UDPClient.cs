using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Metreos.Core.IPC
{
	/// <summary>
	/// Summary description for UDPClient.
	/// </summary>
	public abstract class UDPClient : IDisposable
	{
		private bool broadcast;
		private string server;				// if broadcast flag is on then this is broadcast IP
		private ushort port;				// port number
		private UdpClient client;			// UDP client
		private Socket bc;					// broadcast socket

		// Multicast IP addresses are within the Class D range of 224.0.0.0-239.255.255.255 
		protected UDPClient(bool broadcast, string server, ushort port)
		{
			this.broadcast = broadcast;
			this.server = server;
			this.port = port;
			
			try
			{
				if (broadcast)
				{
					bc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
					IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
					bc.Bind(ep);
					IPAddress ip = IPAddress.Parse(this.server);
					bc.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ip));
					bc.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 1);
					IPEndPoint ipep = new IPEndPoint(ip, this.port);
					bc.Connect(ipep);
				}
				else
				{
					client = new UdpClient(this.server, this.port);
				}
			}
			catch
			{
				ReConnect();
			}
		}

		public void Dispose()
		{
			if (bc != null)
			{
				bc.Close();
				bc = null;
			}

			if (client != null)
			{
				client.Close();
				client = null;
			}
		}

		protected bool Write(byte[] data)
		{
			if(data == null) { return false; }

			bool written = false;
			try
			{
				int nDataSent = 0;
				if (broadcast)
				{
					nDataSent = bc.Send(data, data.Length, SocketFlags.None);
				}
				else
				{
					nDataSent = client.Send(data, data.Length);
				}

				written	= nDataSent > 0 ? true : false ;
			}
			catch
			{
				if (bc != null)
				{
					bc.Close();
					bc = null;
				}

				if (client != null)
				{
					client.Close();
					client = null;
				}

				ReConnect();
			}

			return written;
		}

		private bool ReConnect()
		{
			bool connected = false;

			// try for a minute
			for (int i=0; i<60; i++)
			{
				Thread.Sleep(1000);		
				try
				{
					if (broadcast)
					{
						bc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
						IPEndPoint ep = new IPEndPoint(IPAddress.Any, this.port);
						bc.Bind(ep);
						IPAddress ip = IPAddress.Parse(this.server);
						bc.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ip));
						bc.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 1);
						IPEndPoint ipep = new IPEndPoint(ip, this.port);
						bc.Connect(ipep);
					}
					else
					{
						client = new UdpClient(this.server, this.port);
					}
					
					connected = true;

					break;
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}

			return connected;
		}
	}
}
