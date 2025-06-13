using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LocalUdpSendPort
{
	public class Wall
	{
		public Wall(int sourcePort, int destinationPort)
		{
			this.destinationPort = destinationPort;
			this.sourcePort = sourcePort;
			isReady = false;

			thread = new Thread(new ThreadStart(InNOut));
			thread.Start();
		}

		private int destinationPort;
		private int sourcePort;

		private Socket socket;

		private Thread thread;

		private bool isReady;

		public bool IsReady
		{
			get
			{
				return isReady;
			}
		}

		public void Stop()
		{
			thread.Abort();
			socket.Close();
		}

		private void InNOut()
		{
			byte[] buffer = new byte[500];
#region Receive and SendTo on same port
			socket = new Socket(AddressFamily.InterNetwork,
				SocketType.Dgram, ProtocolType.Udp);

			IPEndPoint destination =
				new IPEndPoint(Dns.GetHostByName(Dns.GetHostName()).AddressList[0],
				destinationPort);
			socket.Bind(new IPEndPoint(IPAddress.Any, sourcePort));

			while (true)
			{
				try
				{
					isReady = true;
					int bytesRead = socket.Receive(buffer);
					if (bytesRead > 0)
					{
						socket.SendTo(buffer, bytesRead, 0, destination);
					}
#endregion
				}
				catch (ThreadAbortException)
				{
					Thread.ResetAbort();

					break;
				}
				catch (SocketException e)
				{
					// If not simply because thread is terminating, log error.
					if (e.ErrorCode != 10004)
					{
						Console.WriteLine("error while echoing from port {0} to port {1}: {2}",
							((IPEndPoint)socket.LocalEndPoint).Port,
							((IPEndPoint)socket.RemoteEndPoint).Port, e.ErrorCode);
					}

					break;
				}
				catch (Exception e)
				{
					Console.WriteLine("error while echoing from port {0} to port {1}: {2}",
						((IPEndPoint)socket.LocalEndPoint).Port,
						((IPEndPoint)socket.RemoteEndPoint).Port, e);

					break;
				}
			}
		}
	}
}
