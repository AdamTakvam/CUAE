using System;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.Messaging;
using System.Threading;
using System.Diagnostics;

using Metreos.Stats;
using Metreos.Core.IPC;
using Metreos.Core.IPC.Flatmaps;
using Metreos.Core.Sockets;
using Metreos.Interfaces;
using Metreos.LoggingFramework;

namespace Metreos.MediaControl
{
    public class MediaServerInfoIPC : MediaServerInfo
    {
        private abstract class Consts
        {
            public const int DefaultIpcPort         = 9530;
            public const string ConnectorThreadName = "MediaServer IPC connect thread";
            public const int MsgTypeId		        = 1001;
            public const int MsgBodyId		        = 100;
        }

        /// <summary>Flatmap IPC client to communicate with media server</summary>
        public IpcFlatmapClient FlatmapClient { get { return flatmapClient; } }
        private readonly IpcFlatmapClient flatmapClient;

        /// <summary>Flatmap IPC communication port</summary>
        private int ipcPort;

        /// <summary>Indicate that IPC client is connected to IPC server</summary>
        private bool connectedToIPCServer = false;

        /// <summary>XML message deserializer</summary>
        private readonly XmlSerializer xmlDeserializer;

        /// <summary>
        /// Indicate that the IPC socket connection is established
        ///   and a connect message transaction has completed
        /// </summary>
        public override bool ConnectedToMediaServer 
        { 
            get { return connectedToIPCServer && connectedToMediaServer; } 
        }

        public MediaServerInfoIPC(string name, uint id, IPAddress addr, uint port, LogWriter log, StatsClient statsClient)
            : base(name, id, addr, log, statsClient)
        {
            if(addr == null)
            {
                throw new ArgumentException("No address configured for media server: " + 
                    base.mediaServerName);
            }

            this.xmlDeserializer = new XmlSerializer(typeof(MediaServerMessage));

            this.ipcPort = port > 1024 ? (int)port : Consts.DefaultIpcPort;

            this.flatmapClient = new IpcFlatmapClient(new IPEndPoint(addr, ipcPort));
            this.flatmapClient.onClose            = new OnCloseDelegate(OnConnectionClosed);
            this.flatmapClient.onConnect          = new OnConnectDelegate(OnConnect);
            this.flatmapClient.onFlatmapMessageReceived  = new OnFlatmapMessageReceivedDelegate(OnMessageReceived);
            this.flatmapClient.Start();
        }

        /// <summary>Sends a message to the media server</summary>
        /// <param name="commandMsg">Message to send</param>
        /// <param name="isInitConnect">Is this an initial connect attempt?</param>
        /// <remarks>Watch for exceptions!</remarks>
        public override void SendCommand(MediaServerMessage commandMsg, bool isInitConnect)
        {
            if(commandMsg == null)
                return;

            FlatmapList map = new FlatmapList();
            map.Add(Consts.MsgBodyId, commandMsg.ToString());

            if(!connectedToIPCServer)
                throw new Exception("IPC cannot send message: Not connected to server");

            if(!this.flatmapClient.Write(Consts.MsgTypeId, map, 0))
                throw new Exception("Failed to write to IPC connection");
        }

        /// <summary>Post received Flatmap IPC message to handler</summary>
        /// <param name="messageType"></param>
        /// <param name="flatmap"></param>
        private void OnMessageReceived(IpcFlatmapClient client, int messageType, FlatmapList map)
        {
            if (messageType == Consts.MsgTypeId &&
                map.Find(Consts.MsgBodyId, 1).dataValue != null)
            {
                string sBody = map.Find(Consts.MsgBodyId, 1).dataValue.ToString();
                XmlTextReader xtr = new XmlTextReader(new System.IO.StringReader(sBody));
                
                long time = Metreos.Utilities.HPTimer.Now();
                MediaServerMessage msg = (MediaServerMessage)xmlDeserializer.Deserialize(xtr);
                time = Metreos.Utilities.HPTimer.MillisSince(time);

                if(time > 128)
                    log.Write(TraceLevel.Warning, "DIAG: OnMessageReceived: time={0}, data content follows:\n{1}", time, sBody);

                if(msg != null && handleMediaServerMessage != null)
                {
                    handleMediaServerMessage(serverId, msg);
                }
            }
        }

        private void OnConnect(IpcClient c, bool reconnect)
        {
            if(connectToMediaServer == null)
            {
                Debug.Fail("connectToMediaServer delegate not hooked in MediaServerInfoIPC");
                return;
            }

            connectedToIPCServer = true;

            connectToMediaServer(this);
        }

        /// <summary>Receiving a connection closed message from IPC server</summary>
        private void OnConnectionClosed(IpcClient c, Exception e)
        {
            if(connectedToIPCServer && e != null)
            {
                log.Write(TraceLevel.Warning, "IPC connection to {0} ({1}) lost: {2}",
                    base.MediaServerName, base.Address.ToString(), e.Message);
            }

            connectedToIPCServer = false;
            
			base.Close();
        }

        public override void Close()
        {
            base.Close();

			try { flatmapClient.Close(); }
			catch {}
        }

		public override void Dispose()
		{
		}
    }
}
