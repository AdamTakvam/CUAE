using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Text;

namespace LocalUdpSendPort
{
	public class Mouth
	{
		public Mouth(int destinationPort)
		{
			socketOut = new Socket(AddressFamily.InterNetwork,
				SocketType.Dgram, ProtocolType.Udp);
			socketOut.Connect(new IPEndPoint(
				IPAddress.Parse(Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString()),
				destinationPort));
		}

		private Socket socketOut;

		public void Stop()
		{
			socketOut.Close();
		}

		public void Shout(string words)
		{
			byte[] buffer = new byte[words.Length + 1];
			Array.Copy(Encoding.ASCII.GetBytes(words), buffer, words.Length);
			buffer[words.Length] = 0;

			try
			{
				socketOut.Send(buffer);
			}
			catch (Exception e)
			{
				Console.WriteLine("error while shouting to port {0}: {1}",
					((IPEndPoint)socketOut.RemoteEndPoint).Port, e);
			}
		}
	}
}
