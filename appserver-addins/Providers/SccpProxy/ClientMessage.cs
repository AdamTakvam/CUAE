using System;
using System.Net;

using Metreos.Utilities;

namespace Metreos.Providers.SccpProxy
{
	/// <summary>
	/// This class represents a message that is sent from an SCCP client to a CCM.
	/// </summary>
	public class ClientMessage : Message
	{
		/// <summary>
		/// Client-message constructor.
		/// </summary>
		/// <param name="rawMessage">Byte array containing raw, binary SCCP
		/// message starting with the packet-length field.</param>
		/// <param name="fromRemote">Address of SCCP client that sent the message.</param>
		/// <param name="fromLocal">Address on localhost where SCCP client sent this message.</param>
		public ClientMessage(byte[] rawMessage, IPEndPoint fromRemote,
			IPEndPoint fromLocal, QueueProcessor qp, Connection connection)
			: base(rawMessage, fromRemote, fromLocal, qp, connection)
		{
		}

		/// <summary>
		/// Address that uniquely identifes the connection over which this
		/// message was sent by an SCCP client to the proxy.
		/// </summary>
		/// <remarks>
		/// This property has the value of the client's remote address.
		/// </remarks>
		public override IPEndPoint FromUniqueAddress
		{
			get
			{
				return FromRemote;
			}
		}

		/// <summary>
		/// Address that uniquely identifes the connection over which this
		/// message is to be sent from the proxy to a CCM.
		/// </summary>
		/// <remarks>
		/// This property has the value of the address on the local host from
		/// which messages are sent to a CCM.
		/// </remarks>
		public override IPEndPoint ToUniqueAddress
		{
			get
			{
				return ToLocal;
			}
		}
	}
}
