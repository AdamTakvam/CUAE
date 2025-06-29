using System;
using System.Diagnostics;

using Metreos.Core.IPC;

namespace Metreos.Core.IPC.Flatmaps
{
	public class IpcFlatmapServer : IpcServer
	{
        public delegate void OnMessageReceivedDelegate(int socketId, string receiveInterface, 
            int messageType, FlatmapList message);

        public event OnMessageReceivedDelegate OnMessageReceived;

		public IpcFlatmapServer(string taskname, ushort listenPort, bool loopback, TraceLevel logLevel)
            : base(taskname, listenPort, loopback, logLevel)
		{
		}

        public bool Write(int socketId, int messageType, FlatmapList flatmap)
        {
            HeaderExtension headerExtension = new HeaderExtension(messageType);

            byte[] headerExtensionAsArray = headerExtension.ToArray();
            headerExtension = null;

            // Convert flatmap and header extension to a single byte array.
            int totalFlatmapLength = flatmap.BinaryFlatmapLength(headerExtensionAsArray.Length);
            byte[] payload = new byte[totalFlatmapLength];

            // Convert payload length to a byte array.
            payload = flatmap.ToFlatmap(headerExtensionAsArray);

            return Write(socketId, payload);
        }

        protected override void OnPayloadReceived(int socketId, string receiveInterface, byte[] payload)
        {
            FlatmapList parmMap = new FlatmapList(payload);

            // (Do this so we can extract message type from header extension
            // in flatmap.)
            HeaderExtension headerExtension = new HeaderExtension(parmMap.HeaderExtension);

            // If callback provided, pass the payload off to consumer
            // to process.
            if (OnMessageReceived != null)
            {
                OnMessageReceived(socketId, receiveInterface, headerExtension.messageType, parmMap);
            }
        }
	}
}
