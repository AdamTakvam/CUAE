using System;
using System.Net;

using Metreos.LoggingFramework;

namespace Metreos.SccpStack
{
//	public delegate void MessageHandler(Connection connection, Message message);

	/// <summary>
	/// This class represents a high-level call-control-based connection.
	/// </summary>
	public class ClientConnection
	{
#if false
		public event MessageHandler AlertingEvent;
		public event MessageHandler ConnectEvent;
		public event MessageHandler ConnectAckEvent;
		public event MessageHandler ProceedingEvent;
		public event MessageHandler OpenReceiveRequestEvent;
		public event MessageHandler RegisteredEvent;
		public event MessageHandler ReleaseEvent;
		public event MessageHandler ReleaseCompleteEvent;
		public event MessageHandler SetupEvent;
		public event MessageHandler SetupAckEvent;
		public event MessageHandler StartTransmitEvent;
		public event MessageHandler UnregisteredEvent;
#endif

		/// <summary>
		/// "Send" message into the stack.
		/// </summary>
		/// <param name="message">Message to send.</param>
		public void Send(Message message)
		{
		}
	}
}
