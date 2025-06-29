using System;
using System.Net;
using System.Diagnostics;

using Metreos.Core.IPC;

namespace Metreos.Core.IPC.Flatmaps
{
	/// <summary>
	/// Delegate for callback into consumer when a complete payload has been
	/// received.
	/// </summary>
	public delegate void OnFlatmapMessageReceivedDelegate(IpcFlatmapClient ipcClient,
		int messageType, FlatmapList flatmap);

	public class IpcFlatmapClient : IpcClient
	{
        public OnFlatmapMessageReceivedDelegate onFlatmapMessageReceived;

		public IpcFlatmapClient( IPEndPoint remoteEp, int writeQueueLength, int delay, bool unused )
			: base( remoteEp, writeQueueLength, delay )
		{
			Init();
		}

		public IpcFlatmapClient( int writeQueueLength, int delay, bool unused )
			: base( null, writeQueueLength, delay )
		{
			Init();
		}

		public IpcFlatmapClient( IPEndPoint endPoint )
			: base( endPoint )
		{
			Init();
		}

		public IpcFlatmapClient()
			: base()
		{
			Init();
		}

		private void Init()
		{
			onPacketReceived += new OnPacketReceivedDelegate( PacketReceived );
		}

		/// <summary>
        /// Write message to IPC server. Wait forever if error.
        /// </summary>
        /// <param name="messageType">Message type.</param>
        /// <param name="flatmap">Flatmap containing message parameters (but
        /// not the message type).</param>
        /// <returns>Whether the write succeeded.</returns>
        public bool Write(int messageType, FlatmapList flatmap)
        {
            return Write(messageType, flatmap, -1);
        }

        /// <summary>
        /// Write message to IPC server. Wait forever specified time.
        /// </summary>
        /// <param name="messageType">Message type.</param>
        /// <param name="flatmap">Flatmap containing message parameters (but
        /// not the message type).</param>
        /// <param name="timeout">Maximum number of seconds to wait attempting
        /// to write message.</param>
        /// <returns>Whether the write succeeded.</returns>
        public bool Write(int messageType, FlatmapList flatmap, int timeout)
        {
            HeaderExtension headerExtension = new HeaderExtension(messageType);

            // Convert flatmap and header extension to a single byte array.
            byte[] payload = flatmap.ToFlatmap(headerExtension.ToArray());

            return Write(NO_FLAG, payload);
        }

        /// <summary>
        /// Called when there is an IPC payload to process, which is a consumer message.
        /// </summary>
        /// <param name="payload">Payload just received.</param>
        
		protected void PacketReceived(IpcClient ipcClient, Packet p)
		{
			FlatmapList parmMap = new FlatmapList(p.buf);

			// (Do this so we can extract message type from header
			// extension in flatmap.)
			HeaderExtension headerExtension =
				new HeaderExtension(parmMap.HeaderExtension);

			onFlatmapMessageReceived(this, headerExtension.messageType, parmMap);
		}
	}
}
