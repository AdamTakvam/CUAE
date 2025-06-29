using System;

using Metreos.Core.IPC;

namespace Metreos.Core.IPC.Flatmaps
{
	/// <summary>
	/// UDP based flatmap client.
	/// </summary>
	public class UDPFlatmapClient : UDPClient
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public UDPFlatmapClient(bool broadcast, string server, ushort port) : base(broadcast, server, port)
		{
		}

		/// <summary>
		/// Write message to UDP based IPC server.
		/// </summary>
		/// <param name="messageType">Message type.</param>
		/// <param name="flatmap">Flatmap containing message parameters (but
		/// not the message type).</param>
		/// <returns>Whether the write succeeded.</returns>
		public bool Write(int messageType, FlatmapList flatmap)
		{
			HeaderExtension headerExtension = new HeaderExtension(messageType);

			// Convert flatmap and header extension to a single byte array.
			byte[] payload = flatmap.ToFlatmap(headerExtension.ToArray());

			return Write(payload);
		}
	}
}
