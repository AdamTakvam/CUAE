using System;

using Metreos.Core.IPC;

namespace Metreos.Core.IPC.Flatmaps
{
	/// <summary>
	/// Summary description for UDPFlatmapServer.
	/// </summary>
	public class UDPFlatmapServer : UDPServer
	{
		public delegate void OnMessageReceivedDelegate(string receiveInterface, int messageType, FlatmapList message);
		public event OnMessageReceivedDelegate OnMessageReceived;

		public UDPFlatmapServer(bool broadcast, string broadcastIP, ushort port, bool loopback) 
            : base(broadcast, broadcastIP, port, loopback)
		{
		}

		protected override void OnPayloadReceived(string receiveInterface, byte[] payload)
		{
			try 
			{
				FlatmapList parmMap = new FlatmapList(payload);
				// (Do this so we can extract message type from header extension in flatmap.)
				HeaderExtension headerExtension = new HeaderExtension(parmMap.HeaderExtension);

				// If callback provided, pass the payload off to consumer to process.
				if (OnMessageReceived != null)
				{
					OnMessageReceived(receiveInterface, headerExtension.messageType, parmMap);
				}
			}
			catch
			{
			}
		}
	}
}
