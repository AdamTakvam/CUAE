using System.Diagnostics;

namespace Metreos.Providers.SccpProxy
{
	/// <summary>
	/// This simple class provides a unified interface to the application.
	/// </summary>
	public class AppFacade
	{
		public AppFacade(Proxy proxy)
		{
			this.proxy = proxy;
		}

		private Proxy proxy;

		/// <summary>
		/// Send Register message to the app.
		/// </summary>
		/// <param name="tag">Tag that uniquely identifies this message in the
		/// pendingMessages table. The app will later have to return it to us
		/// so we can remove the message from the table.</param>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="ipAddress">IP address taken from the Register message.</param>
		/// <param name="sid">"Skinny IDentifier," a.k.a., device name. This is
		/// what app uses to decide where to route this device's messages.</param>
		public void SendRegister(int tag, Message message, string ipAddress, string sid)
		{
//			Message.Type.Register;
			string fromIpAddress = message.From.Address.ToString();
			int toPort = message.From.Port;
		}

		/// <summary>
		/// Send OpenReceiveChannelAck message to the app.
		/// </summary>
		/// <param name="tag">Tag that uniquely identifies this message in the
		/// pendingMessages table. The app will later have to return it to us
		/// so we can remove the message from the table.</param>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="ipAddress">IP address taken from the OpenReceiveChannelAck message.</param>
		/// <param name="port">Port taken from the OpenReceiveChannelAck message.</param>
		public void SendOpenReceiveChannelAck(int tag, Message message, string ipAddress, int port)
		{
//			Message.Type.OpenReceiveChannelAck;
			string fromIpAddress = message.From.Address.ToString();
			int toPort = message.From.Port;
		}

		/// <summary>
		/// Send StartMediaTransmission message to the app.
		/// </summary>
		/// <param name="tag">Tag that uniquely identifies this message in the
		/// pendingMessages table. The app will later have to return it to us
		/// so we can remove the message from the table.</param>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="ipAddress">IP address taken from the StartMediaTransmission message.</param>
		/// <param name="port">Port taken from the StartMediaTransmission message.</param>
		public void SendStartMediaTransmission(int tag, Message message, string ipAddress, int port)
		{
//			Message.Type.StartMediaTransmission;
			string fromIpAddress = message.From.Address.ToString();
			int toPort = message.From.Port;
		}

		/// <summary>
		/// Send StartMulticastMediaReceptionmessage to the app.
		/// </summary>
		/// <param name="tag">Tag that uniquely identifies this message in the
		/// pendingMessages table. The app will later have to return it to us
		/// so we can remove the message from the table.</param>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="ipAddress">IP address taken from the StartMulticastMediaReceptionmessage.</param>
		/// <param name="port">Port taken from the StartMulticastMediaReceptionmessage.</param>
		public void SendStartMulticastMediaReception(int tag, Message message, string ipAddress, int port)
		{
//			Message.Type.StartMulticastMediaReception;
			string fromIpAddress = message.From.Address.ToString();
			int toPort = message.From.Port;
		}

		/// <summary>
		/// Send StartMulticastMediaTransmissionmessage to the app.
		/// </summary>
		/// <param name="tag">Tag that uniquely identifies this message in the
		/// pendingMessages table. The app will later have to return it to us
		/// so we can remove the message from the table.</param>
		/// <param name="message">Message whose info we are sending to app.</param>
		/// <param name="ipAddress">IP address taken from the StartMulticastMediaTransmissionmessage.</param>
		/// <param name="port">Port taken from the StartMulticastMediaTransmissionmessage.</param>
		public void SendStartMulticastMediaTransmission(int tag, Message message, string ipAddress, int port)
		{
//			Message.Type.StartMulticastMediaTransmission;
			string fromIpAddress = message.From.Address.ToString();
			int toPort = message.From.Port;
		}

		/// <summary>
		/// Send "simple" message (no parameters other than message type) to the app.
		/// </summary>
		/// <param name="tag">Tag that uniquely identifies this message in the
		/// pendingMessages table. The app will later have to return it to us
		/// so we can remove the message from the table.</param>
		/// <param name="message">Message whose info we are sending to app.</param>
		public void SendSimpleMessage(int tag, Message message)
		{
			Message.Type messageType = message.MessageType;
			string fromIpAddress = message.From.Address.ToString();
			int toPort = message.From.Port;
		}

		/// <summary>
		/// Send non-Skinny, connection-failure message to the app. This tells
		/// the app that it should consider the registration terminated.
		/// </summary>
		/// <param name="sid">Skinny id of the device whose connection failed.</param>
		public void SendConnectionFailure(string sid)
		{
			Debug.Assert(sid != null && sid.Length > 0, "Invalid Skinny identifier");

			if (sid == null)
			{
				sid = "";
			}
		}
	}
}
