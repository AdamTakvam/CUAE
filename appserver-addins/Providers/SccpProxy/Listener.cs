using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

using Metreos.Core.IPC;
using Metreos.Utilities;
using Metreos.Utilities.Selectors;

namespace Metreos.Providers.SccpProxy
{
	/// <summary>
	/// This class represents the passive, server side of the proxy, listening
	/// for SCCP clients to initiate a connection with us, as proxy for the
	/// CCMs. Once the connection is established, however, the connection
	/// class takes over.
	/// </summary>
	public class Listener
	{
		/// <summary>
		/// Simple server constructor.
		/// </summary>
		/// <param name="tp">thread pool to use to construct queue processor</param>
		/// <param name="iqpd">action to perform processing of incoming message</param>
		/// <param name="connections">Table that contains TCP connections with
		/// CCMs and SCCP clients.</param>
		/// <param name="socketFailure">Delegate to handle socket failure.</param>
		/// <param name="provider">Refers back to provider in order to access
		/// methods in its base class.</param>
		/// <param name="selector">The selector to use with the sockets we accept.</param>
		public Listener(Metreos.Utilities.ThreadPool tp, QueueProcessorDelegate iqpd,
			Connections connections,
			OnSocketFailureDelegate socketFailure,
			SccpProxyProvider provider,
			SelectorBase selector)
		{
			this.tp = tp;
			this.iqpd = iqpd;
			this.connections = connections;
			this.socketFailure = socketFailure;
			this.provider = provider;
			this.selector = selector;
		}

		private Metreos.Utilities.ThreadPool tp;
		
		private QueueProcessorDelegate iqpd;

		/// <summary>
		/// The selector to use with the sockets we accept.
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
		/// Thread that listen for incoming TCP connections from SCCP clients
		/// </summary>
		//private Thread thread;

		/// <summary>
		/// Whether the server is active--listening for connections.
		/// </summary>
		private bool isActive;

		/// <summary>
		/// Mask for IP addresses of remote entities whose connections we accept.
		/// </summary>
		private int ipAddressAcceptMask;

		/// <summary>
		/// Write-only property for setting the mask for IP addresses of remote
		/// entities whose connections we accept.
		/// </summary>
		public int IpAddressAcceptMask
		{
			set
			{
				ipAddressAcceptMask = value;
			}
		}

		/// <summary>
		/// Mask for IP addresses of remote entities whose connections we deny.
		/// </summary>
		private int ipAddressDenyMask;

		/// <summary>
		/// Write-only property for setting the mask for IP addresses of remote
		/// entities whose connections we deny.
		/// </summary>
		public int IpAddressDenyMask
		{
			set
			{
				ipAddressDenyMask = value;
			}
		}

		/// <summary>
		/// Start thread that listens for new connections.
		/// </summary>
		public void Start()
		{
			lock (this)	// So (re)starting and stopping don't occur at same time
			{
				if (!isActive)
				{
					MakeListener();
					isActive = true;
				}
			}
		}

		/// <summary>
		/// Property whose value is the port that we listen to for
		/// new, incoming SCCP sessions.
		/// </summary>
		public int ListenPortNumber
		{
			// Return the number of the port to which the TCP listener is
			// currently listening. If there is no TCP listener, return 0.
			get
			{
				if (listener != null)
					return ((IPEndPoint)listener.Socket.LocalEndPoint).Port;
				
				return 0;
			}

			// If we're active and specified port number is different than what
			// we're currently using, restart with the new port number.
			set
			{
				provider.LogWrite(TraceLevel.Info,
					"Lsn: changing listenPortNumber from {0} to {1}: isActive {2}",
					listenPortNumber, value, isActive);

				lock (this)	// So (re)starting and stopping don't occur at same time
				{
					if (isActive && value != listenPortNumber)
					{
						Stop();
						listenPortNumber = value;
						Start();
					}
					else if (!isActive)
					{
						listenPortNumber = value;
					}
				}
			}
		}

		private int listenPortNumber;

		private void MakeListener()
		{
			provider.LogWrite(TraceLevel.Info,
				"Lsn: starting listener on port {0}",
				listenPortNumber);
			
			IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, listenPortNumber);

			Socket xlistener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			xlistener.Bind(endPoint);
			xlistener.Listen(250);
			xlistener.Blocking = false;
			
			listener = selector.Register(xlistener, null, 
				new Metreos.Utilities.Selectors.SelectedDelegate(ListenerSelected),
				null);
			listener.WantsAccept = true;
		}

		private Metreos.Utilities.Selectors.SelectionKey listener;

		/// <summary>
		/// Stop the thread that listens for new connections.
		/// </summary>
		public void Stop()
		{
			lock (this)	// So (re)starting and stopping don't occur at same time
			{
				provider.LogWrite(TraceLevel.Info,
					"Lsn: stopping listener on port {0}",
					listenPortNumber );
				listener.Close();
				listener = null;
				isActive = false;
			}
		}

		private void ListenerSelected(Metreos.Utilities.Selectors.SelectionKey key)
		{
			if (key.IsSelectedForError)
			{
				// this probably means the listener has been closed.
				// so we don't really need to do anything.
				provider.LogWrite(TraceLevel.Error,
					"Lsn: {0}: selected for error",
					key );
				key.Close();
				return;
			}

			if (key.IsSelectedForAccept)
			{
				try
				{
					MakeSocket( key.Accept() );
				}
				catch ( SocketException e )
				{
					if (e.ErrorCode != 10035)
						throw e;
				}
				return;
			}

			provider.LogWrite(TraceLevel.Error,
				"Lsn: {0}: selected for what I dunno",
				key);
		}

		private void MakeSocket(Socket socket)
		{
			//socket.Blocking = true;

			IPEndPoint endPoint = (IPEndPoint) socket.RemoteEndPoint;
			int ipAddress = BitConverter.ToInt32(
				endPoint.Address.GetAddressBytes(), 0);

			if ((ipAddress & ipAddressAcceptMask) != ipAddress)
			{
				provider.LogWrite(TraceLevel.Warning,
					"Lsn: {0} does not match accept mask; ignored",
					endPoint);
				socket.Close();
				return;
			}

			if ((ipAddress & ipAddressDenyMask) == ipAddress)
			{
				provider.LogWrite(TraceLevel.Warning,
					"Lsn: {0} matches deny mask; ignored",
					endPoint);
				socket.Close();
				return;
			}

			lock (connections)
			{
				if (connections.IsRoomAvailable())
				{
					// Create connection, add to connection
					// list, then start connection.

					Connection connection =
						new ClientConnection(selector.Register(socket),
						new QueueProcessor( null, tp ), iqpd, socketFailure,
						provider, selector);
					
					connections.Add(connection);
					connection.Start();
					return;
				}
			}

			provider.LogWrite(TraceLevel.Warning,
				"Lsn: {0} rejected; too many connections ({1} is the limit)",
				endPoint, connections.MaxConnections);
			socket.Close();
		}

		/// <summary>
		/// When someone connects on a socket, add an entry to the connections
		/// table, and spawn a thread to read packets on that socket.
		/// </summary>
		/// <remarks>
		/// The number of connections is limited to the value of the
		/// MaxConnected constant.
		/// </remarks>
//		private void WaitingForClient(SelectionKey key, int eventMask)
//		{
//			try
//			{
//				provider.LogWrite(TraceLevel.Info, "Lsn: {0} <-",
//					tcpListener.LocalEndpoint);
//
//				tcpListener.Start();
//
//				isActive = true;
//
//				while (true)
//				{
//					// Accept will block until someone connects
//					Socket socket = tcpListener.AcceptSocket();
//
//					// If the socket connected, the IP address matches accept
//					// mask but does not match deny mask, and we have not
//					// reached our connection limit, create connection, add it
//					// to the connections table, and start the connection (its
//					// read thread).
//					//
//					// (The socket.Connected test seems redundant, but I've
//					// seen it used elsewhere, so I'm just being extra
//					// careful.)
//					if (socket.Connected)
		//					{
		//						int ipAddress = BitConverter.ToInt32(
		//							((IPEndPoint)socket.RemoteEndPoint).Address.GetAddressBytes(), 0);
		//						if ((ipAddress & ipAddressAcceptMask) == ipAddress)
		//						{
		//							if ((ipAddress & ipAddressDenyMask) != ipAddress)
		//							{
		//								bool added = false;
		//								lock (connections)
		//								{
		//									if (connections.IsFewerThan(maxConnected))
		//									{
		//										// Create connection, add to connection
		//										// list, then start connection.
		//										Connection connection =
		//											new ClientConnection(socket,
		//											processIncomingPacket, socketFailure,
		//											provider);
		//										connections.Add(connection);
		//										connection.Start();
		//
		//										added = true;
		//									}
		//								}
		//
		//								if (!added)
		//								{
		//									socket.Close();
		//									provider.LogWrite(TraceLevel.Warning,
		//										"Lsn: too many connections ({0} is the limit); ignored",
		//										maxConnected);
		//								}
		//							}
		//							else
		//							{
		//								socket.Close();
		//								provider.LogWrite(TraceLevel.Warning,
		//									"Lsn: {0} matches deny mask; ignored",
		//									((IPEndPoint)socket.RemoteEndPoint).Address);
		//							}
		//						}
		//						else
		//						{
		//							socket.Close();
		//							provider.LogWrite(TraceLevel.Warning,
		//								"Lsn: {0} does not match accept mask; ignored",
		//								((IPEndPoint)socket.RemoteEndPoint).Address);
		//						}
//					}
//					else
//					{
//						socket.Close();
//						provider.LogWrite(TraceLevel.Warning,
//							"Lsn: connection failed on {0}; ignored",
//							tcpListener.LocalEndpoint);
//					}
//				}
//			}
//			catch(ThreadAbortException)
//			{
//				// Do nothing.
//			}
//			catch(Exception e)
//			{
//				provider.LogWrite(TraceLevel.Error, "Lsn: {0}", e);
//			}
//		}
	}
}
