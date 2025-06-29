using System;
using System.Diagnostics;

namespace Metreos.Core.IPC.Xml
{
	public class IpcXmlServer : IpcServer
	{
        public delegate void OnMessageReceivedDelegate(int socketId, string receiveInterface, string message);

        public event OnMessageReceivedDelegate OnMessageReceived;

		public IpcXmlServer(string taskname, ushort listenPort, bool loopback, TraceLevel logLevel)
            : base(taskname, listenPort, loopback, logLevel)
		{
		}

        public bool Write(int socketId, string xmlStr)
        {
            byte[] xmlBytes = System.Text.Encoding.UTF8.GetBytes(xmlStr);

            return base.Write(socketId, xmlBytes);
        }

        public bool WriteToAllSockets(string xmlStr)
        {
            byte[] xmlBytes = System.Text.Encoding.UTF8.GetBytes(xmlStr);

            return base.WriteToAllSockets(xmlBytes);
        }

        protected override void OnPayloadReceived(int socketId, string receiveInterface, byte[] payload)
        {
            string xmlStr = System.Text.Encoding.UTF8.GetString(payload);

            if(OnMessageReceived != null)
            {
                OnMessageReceived(socketId, receiveInterface, xmlStr);
            }
        }

	}
}
