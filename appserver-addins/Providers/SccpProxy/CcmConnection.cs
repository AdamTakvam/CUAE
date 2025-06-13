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
	/// This class represents a client connection--a connection between the
	/// proxy and a CCM.
	/// </summary>
	public class CcmConnection : Connection
	{
		/// <summary>
		/// Constructor that initializes member variables and creates a new
		/// thread to read on this client connection.
		/// </summary>
		/// <param name="socket">TCP socket on which to do reads.</param>
		/// <param name="processIncomingPacket">Delegate to process incoming
		/// packets.</param>
		/// <param name="socketFailure">Delegate to handle socket failure.</param>
		/// <param name="provider">Refers back to provider in order to access
		/// methods in its base class.</param>
		/// <param name="selector">The selector to use with the sockets we create.</param>
		public CcmConnection(Metreos.Utilities.Selectors.SelectionKey key,
			QueueProcessor processIncomingPacket,
			QueueProcessorDelegate iqpd,
			OnSocketFailureDelegate socketFailure,
			SccpProxyProvider provider,
			SelectorBase selector)
			: base(key, processIncomingPacket, iqpd, socketFailure, provider, selector)
		{
		}

		/// <summary>
		/// Address that uniquely identifes this client connection (between
		/// proxy and CCMs) by having the value of the proxy's local address.
		/// </summary>
		public override IPEndPoint UniqueAddress
		{
			get
			{
				return LocalAddress;
			}
		}

		/// <summary>
		/// Message constructor when sent from a CCM.
		/// </summary>
		/// <param name="rawMessage">Byte array containing raw, binary SCCP
		/// message starting with the packet-length field.</param>
		/// <param name="fromRemote">Address from where CCM sent this message.</param>
		/// <param name="fromLocal">Address on localhost where CCM sent this message.</param>
		protected override Message CreateMessage(byte[] rawMessage,
			IPEndPoint fromRemote, IPEndPoint fromLocal)
		{
			return new CcmMessage(rawMessage, fromRemote, fromLocal, IncomingQueueProcessor, this);
		}

		/// <summary>
		/// Method to invoke to report that no counterpart has been found for
		/// the connection over which the specified message was sent.
		/// </summary>
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
			return String.Format( "CcmCo {0} -> {1} ({2})",
				LocalAddress, RemoteAddress, MySession );
		}
	}
}
