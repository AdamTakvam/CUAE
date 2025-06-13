using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LocalUdpSendPort
{
	public class Ear
	{
		public Ear(int sourcePort)
		{
			this.sourcePort = sourcePort;
			isReady = false;

			thread = new Thread(new ThreadStart(In));
			thread.Start();
		}

		private int sourcePort;

		private Socket socketIn;

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
			isReady = false;
			thread.Abort();
			socketIn.Close();
		}

		private void In()
		{
			socketIn = new Socket(AddressFamily.InterNetwork,
				SocketType.Dgram, ProtocolType.Udp);

			socketIn.Bind(new IPEndPoint(IPAddress.Any, sourcePort));

			byte[] buffer = new byte[500];

			while (true)
			{
				try
				{
					isReady = true;
					EndPoint sendingAddress = new IPEndPoint(IPAddress.Any, 0);
					int bytesRead = socketIn.ReceiveFrom(buffer, ref sendingAddress);
					if (bytesRead > 0)
					{
						Console.WriteLine("{0}: {1}",
							((IPEndPoint)sendingAddress).Port,
							StringByteArrayToString(buffer, 0, buffer.Length));
					}
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
						Console.WriteLine("error while hearing on port {0}: {1}",
							((IPEndPoint)socketIn.LocalEndPoint).Port,
							e.ErrorCode);
					}

					break;
				}
				catch (Exception e)
				{
					Console.WriteLine("error while hearing on port {0}: {1}",
						((IPEndPoint)socketIn.LocalEndPoint).Port, e);

					break;
				}
			}
		}

		private static string StringByteArrayToString(byte[] buffer, int offset, int length)
		{
			string str = (new ASCIIEncoding()).GetString(buffer, offset, length);
			int index = str.IndexOf('\0');
			if (index >= 0)
			{
				str = str.Substring(0, index);
			}

			return str;
		}
	}
}
