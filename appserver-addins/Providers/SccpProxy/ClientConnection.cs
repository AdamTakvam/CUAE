using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

using Metreos.Core.IPC;
using Metreos.Utilities;
using Metreos.Utilities.Selectors;

namespace Metreos.Providers.SccpProxy
{
	/// <summary>
	/// This class represents a server connection--a connection between the
	/// proxy and an SCCP client.
	/// </summary>
	public class ClientConnection : Connection
	{
		/// <summary>
		/// Constructor that initializes member variables and creates a new
		/// thread to read on this server connection.
		/// </summary>
		/// <param name="socket">TCP socket on which to do reads.</param>
		/// <param name="processIncomingPacket">Delegate to process incoming
		/// packets.</param>
		/// <param name="socketFailure">Delegate to handle socket failure.</param>
		/// <param name="provider">Refers back to provider in order to access
		/// methods in its base class.</param>
		/// <param name="selector">The selector to use with the sockets we create.</param>
		public ClientConnection(Metreos.Utilities.Selectors.SelectionKey socket,
			QueueProcessor processIncomingPacket,
			QueueProcessorDelegate iqpd,
			OnSocketFailureDelegate socketFailure,
			SccpProxyProvider provider,
			SelectorBase selector)
			: base(socket, processIncomingPacket, iqpd, socketFailure, provider, selector)
		{
		}

		/// <summary>
		/// Address that uniquely identifes this server connection (between
		/// proxy and SCCP clients) by having the value of the client's remote
		/// address.
		/// </summary>
		public override IPEndPoint UniqueAddress { get { return RemoteAddress; } }

		/// <summary>
		/// Message constructor when sent from an SCCP client.
		/// </summary>
		/// <param name="rawMessage">Byte array containing raw, binary SCCP
		/// message starting with the packet-length field.</param>
		/// <param name="fromRemote">Address from where SCCP client sent this message.</param>
		/// <param name="fromLocal">Address on localhost where SCCP client sent this message.</param>
		protected override Message CreateMessage(byte[] rawMessage,
			IPEndPoint fromRemote, IPEndPoint fromLocal)
		{
			return new ClientMessage(rawMessage, fromRemote, fromLocal, IncomingQueueProcessor, this);
		}

		/// <summary>
		/// Method to invoke to report (or not) that no counterpart has been
		/// found for the connection over which the specified message was sent.
		/// </summary>
		/// <remarks>
		/// If no sid has been assigned, we haven't gotten a response back from
		/// the app for our Register, so it's normal to encounter
		/// no-counterpart. In that case, we only make an Info log entry;
		/// otherwise, a Warning.
		/// </remarks>
		/// <param name="message">Message sent over connection for which we are
		/// looking for counterpart.</param>
		public override void ReportNoCounterpartOnProxy(Message message)
		{
			provider.LogWrite(TraceLevel.Info,
				"{0}: no counterpart; cannot proxy {1}",
				this, message);
		}

		/// <summary>
		/// Return string representing this connection.
		/// </summary>
		/// <returns>String representing this connection.</returns>
		public override string ToString()
		{
			return String.Format( "ClCo {0} -> {1} ({2})",
				RemoteAddress, LocalAddress, MySession );
		}
	}
}
