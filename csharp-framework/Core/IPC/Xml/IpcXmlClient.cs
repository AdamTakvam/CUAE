using System;
using System.Net;

namespace Metreos.Core.IPC.Xml
{
	public delegate void OnXmlMessageReceivedDelegate(IpcXmlClient ipcClient, string message);

	public class IpcXmlClient : IpcClient
	{
		public IpcXmlClient( IPEndPoint remoteEp, int writeQueueLength, int delay, bool unused )
			: base( remoteEp, writeQueueLength, delay )
		{
			Init();
		}

		public IpcXmlClient( int writeQueueLength, int delay, bool unused )
			: base( null, writeQueueLength, delay )
		{
			Init();
		}

		public IpcXmlClient( IPEndPoint remoteEp )
			: base( remoteEp )
		{
			Init();
		}

		public IpcXmlClient()
			: base()
		{
			Init();
		}

		private void Init()
		{
			onPacketReceived = new OnPacketReceivedDelegate( PacketReceived );
		}

		public OnXmlMessageReceivedDelegate onXmlMessageReceived;

        public bool Write(string xmlStr)
        {
            return base.Write((int)IpcConsts.PayloadType.XML, System.Text.Encoding.UTF8.GetBytes(xmlStr));
        }

        private void PacketReceived(IpcClient ipcClient, Packet p)
        {
            onXmlMessageReceived(this, System.Text.Encoding.UTF8.GetString(p.buf));
        }
	}
}
