using System;
using System.Net;

using Metreos.Utilities;

namespace Metreos.Providers.SccpProxy
{
	/// <summary>
	/// This class represents a message that is sent from a CCM to an SCCP client.
	/// </summary>
	public class CcmMessage : Message
	{
		/// <summary>
		/// CCM-message constructor.
		/// </summary>
		/// <param name="rawMessage">Byte array containing raw, binary SCCP
		/// message starting with the packet-length field.</param>
		/// <param name="fromRemote">Address of CCM that sent the message.</param>
		/// <param name="fromLocal">Address on localhost where CCM sent this message.</param>
		public CcmMessage(byte[] rawMessage, IPEndPoint fromRemote,
			IPEndPoint fromLocal, QueueProcessor qp, Connection connection)
			: base(rawMessage, fromRemote, fromLocal, qp, connection)
		{
		}

		/// <summary>
		/// Address that uniquely identifes the connection over which this
		/// message was sent by a CCM to the proxy.
		/// </summary>
		/// <remarks>
		/// This property has the value of the proxy's local address.
		/// </remarks>
		public override IPEndPoint FromUniqueAddress
		{
			get
			{
				return FromLocal;
			}
		}

		/// <summary>
		/// Address that uniquely identifes the connection over which this
		/// message is to be sent from the proxy to an SCCP client.
		/// </summary>
		/// <remarks>
		/// This property has the value of the address on the client's remote\
		/// address.
		/// </remarks>
		public override IPEndPoint ToUniqueAddress
		{
			get
			{
				return ToRemote;
			}
		}
	}
}
