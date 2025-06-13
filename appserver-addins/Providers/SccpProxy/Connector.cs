using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Diagnostics;

using Metreos.Core.IPC;
using Metreos.Utilities;
using Metreos.Utilities.Selectors;

namespace Metreos.Providers.SccpProxy
{
	/// <summary>
	/// This class represents the act of initiating a connection with a CCM.
	/// Once the connection is established, however, the connection class takes
	/// over.
	/// </summary>
	public class Connector
	{
		/// <summary>
		/// Simple client constructor.
		/// </summary>
		/// <param name="processIncomingPacket">Delegate to process incoming
		/// packets.</param>
		/// <param name="connections">Table that contains TCP connections with
		/// CCMs and SCCP clients.</param>
		/// <param name="socketFailure">Delegate to handle socket failure.</param>
		/// <param name="provider">Refers back to provider in order to access
		/// methods in its base class.</param>
		/// <param name="selector">The selector to use with the sockets we create.</param>
		public Connector(QueueProcessorDelegate iqpd,
			Connections connections,
			OnSocketFailureDelegate socketFailure,
			SccpProxyProvider provider,
			SelectorBase selector)
		{
			this.iqpd = iqpd;
			this.connections = connections;
			this.socketFailure = socketFailure;
			this.provider = provider;
			this.selector = selector;
		}
		
		private QueueProcessorDelegate iqpd;

		/// <summary>
		/// The selector to use with the sockets we create.
		/// </summary>
		private SelectorBase selector;

		/// <summary>
		/// Refers back to provider in order to access methods in its base class.
		/// </summary>
		SccpProxyProvider provider;

		/// <summary>
		/// TCP connections with CCMs and SCCP clients.
		/// </summary>
		private Connections connections;

		/// <summary>
		/// Delegate to invoke upon socket failure.
		/// </summary>
		private OnSocketFailureDelegate socketFailure;

		/// <summary>
		/// This method blocks until connected on the connection object's
		/// socket.
		/// </summary>
		/// <param name="toRemote">Address of CCM to connect to.</param>
		/// <returns>Connection object if connection established; null otherwise.</returns>
		public void Connect(OnConnectDoneDelegate connectDone,
			Message message, Session session, Connection fromConnection)
		{
			Socket socket = new Socket(AddressFamily.InterNetwork,
				SocketType.Stream, ProtocolType.Tcp);

			try
			{
				socket.Bind(new IPEndPoint( IPAddress.Any, 0 ));
				socket.Blocking = false;				
				provider.LogWrite(TraceLevel.Verbose,
					"Cnctr: connecting to {0} for {1}", message.ToRemote, fromConnection );

				try
				{
					socket.Connect(message.ToRemote);
				}
				catch ( SocketException e )
				{
					if (e.ErrorCode != 10035)
						throw e;
				}

				provider.LogWrite(TraceLevel.Verbose,
					"Cnctr: connect started to {0} for {1}",
					message.ToRemote, fromConnection );

				selector.Register(socket, new ConnectInfo(connectDone, message, session, fromConnection),
					new Metreos.Utilities.Selectors.SelectedDelegate(SelectedForConnect), null,
					false, true, false, false);
			}
			catch
			{
				socket.Close();
				provider.LogWrite(TraceLevel.Warning,
					"Cnctr: connection on {0} for {1} failed",
					message.ToRemote, fromConnection);
				connectDone(message, session, fromConnection, null);
			}
		}

		private void SelectedForConnect(Metreos.Utilities.Selectors.SelectionKey key)
		{
			ConnectInfo info = (ConnectInfo) key.Data;

			if (key.IsSelectedForError)
			{
				provider.LogWrite(TraceLevel.Warning,
					"Cnctr: connection on {0} for {1} failed",
					info.message.ToRemote, info.fromConnection);
				info.connectDone(info.message, info.session, info.fromConnection, null);
				key.Close();
				return;
			}

			Assertion.Check(key.IsSelectedForConnect,
				"SelectedForConnect: selected for what i dunno");

			key.WantsConnect = false;

			provider.LogWrite(TraceLevel.Info, "Cnctr: connected {0} to {1} for {2}",
				key.Socket.LocalEndPoint, key.Socket.RemoteEndPoint, info.fromConnection );

			CcmConnection connection = new CcmConnection(key,
				info.fromConnection.IncomingQueueProcessor, iqpd, socketFailure, provider, selector);
			
			connections.Add(connection);
			connection.Start();
			info.connectDone(info.message, info.session, info.fromConnection, connection);
		}
	}

	internal class ConnectInfo
	{
		public ConnectInfo(OnConnectDoneDelegate connectDone,
			Message message, Session session, Connection fromConnection)
		{
			this.connectDone = connectDone;
			this.message = message;
			this.session = session;
			this.fromConnection = fromConnection;
		}

		public OnConnectDoneDelegate connectDone;
		
		public Message message;
		
		public Session session;
		
		public Connection fromConnection;
	}
}
