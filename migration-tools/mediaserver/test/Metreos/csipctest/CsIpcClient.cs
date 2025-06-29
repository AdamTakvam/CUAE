using System;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

using Metreos.Core.IPC.Flatmaps;
using Metreos.Interfaces;

namespace csipctest
{
	/// <summary>
	/// Summary description for CsIpcClient.
	/// </summary>
	public class CsIpcClient
	{
        public enum IPCState
        {
            Disconnected = 0,
            Connecting,
            Connected
        }

        public abstract class Consts
        {
            public const int IpcConnectTimeout      = 120;  // seconds
            public const string ConnectorThreadName = "mms ipc server connect thread";
            public const int MsgTypeId		        = 1001;
            public const int MsgBodyId		        = 100;
        }

        private IpcFlatmapClient ipcClient;
        private XmlSerializer xmlDeserializer;

        private IPCState state = IPCState.Disconnected;
        public IPCState State { get { return state; } }

        private int transId = 0;
        private string serverId = "0";
        private string clientId = "0";

		public CsIpcClient()
		{      
            ipcClient = new IpcFlatmapClient();
            this.xmlDeserializer = new XmlSerializer(typeof(MediaServerMessage));

            ipcClient.onConnect += new Metreos.Core.IPC.OnConnectDelegate(ipcClient_onConnect);
            ipcClient.onConnectFailed += new Metreos.Core.IPC.OnConnectFailedDelegate(ipcClient_onConnectFailed);
            ipcClient.onConnectionClosed += new Metreos.Core.Sockets.ClientCloseConnectionDelegate(ipcClient_onConnectionClosed);
            ipcClient.OnMessageReceived += new Metreos.Core.IPC.Flatmaps.IpcFlatmapClient.OnMessageReceivedDelegate(ipcClient_OnMessageReceived);
            ipcClient.onReconnect += new Metreos.Core.IPC.OnReconnectDelegate(ipcClient_onReconnect);
        }

        public void Start(IPEndPoint ipEndpoint)
        {
            if(ipcClient == null)
                throw new ObjectDisposedException(typeof(CsIpcClient).Name);

            if(ipEndpoint == null || ipEndpoint.Address == null)
                throw new Metreos.Core.StartupFailedException("Invalid mms server ipc address");

            state = IPCState.Connecting;

            if(ipcClient.OpenAsync( ipEndpoint.Address.ToString(), ipEndpoint.Port, Consts.IpcConnectTimeout,
                                    Consts.ConnectorThreadName) == false)
            {
                throw new Metreos.Core.StartupFailedException("Could not start ipc test connect thread");
            }

        }

        public void Stop()
        {
            ipcClient.Close();
            ipcClient.Dispose();

            state = IPCState.Disconnected;
            Console.WriteLine("IPC Client Stopped.");
        }

        private void ipcClient_onConnect()
        {
            state = IPCState.Connected;
            Console.WriteLine("IPC Client Connected.");
            SendServerConnect();
        }

        private void ipcClient_onConnectFailed()
        {
            state = IPCState.Disconnected;
            Console.WriteLine("IPC Client failed, re-opening async....");
            ipcClient.ReopenAsync();
        }

        private void ipcClient_onConnectionClosed()
        {
            Console.WriteLine("IPC Client Disconnected.");
            state = IPCState.Disconnected;
        }

        private void ipcClient_onReconnect()
        {
            state = IPCState.Connected;
            Console.WriteLine("IPC Client Reconnected.");
            SendServerConnect();
        }

        private void ipcClient_OnMessageReceived(int messageType, FlatmapList map)
        {
            if (messageType == Consts.MsgTypeId && map.Find(Consts.MsgBodyId, 1).dataValue != null)
            {
                string sBody = map.Find(Consts.MsgBodyId, 1).dataValue.ToString();
                XmlTextReader xtr = new XmlTextReader(new System.IO.StringReader(sBody)); 
                MediaServerMessage msg = (MediaServerMessage)xmlDeserializer.Deserialize(xtr);

                string cid;
                if(msg.GetFieldByName(IMediaServer.FIELD_MS_CLIENT_ID, out cid) == true)
                    this.clientId = cid; 

                if(msg != null)
                {
                    if (msg.MessageId == IMediaServer.MSG_MS_HEARTBEAT)
                    {
                        string heartbeatId = null;

                        if(msg.GetFieldByName(IMediaServer.FIELD_MS_HEARTBEAT_ID, out heartbeatId) == false)
                        {
                            Console.WriteLine("Failed to retrieve heartbeat id.");
                        }
                        else
                        {
                            MediaServerMessage ackMsg = new MediaServerMessage();
                            ackMsg.MessageId = IMediaServer.MSG_MS_HEARTBEAT;
                            ackMsg.AddField(new Field(IMediaServer.FIELD_MS_HEARTBEAT_ID, heartbeatId));

                            ackMsg.AddField(new Field(IMediaServer.FIELD_MS_TRANSACTION_ID, transId.ToString()));
                            ackMsg.AddField(new Field(IMediaServer.FIELD_MS_SERVER_ID, serverId));
        
                            if (ackMsg.IsFieldPresent(IMediaServer.FIELD_MS_CLIENT_ID) == false)
                                ackMsg.AddField(new Field(IMediaServer.FIELD_MS_CLIENT_ID, clientId));

                            try
                            {
                                FlatmapList mapx = new FlatmapList();
                                mapx.Add(Consts.MsgBodyId, ackMsg.ToString());

                                if (State == IPCState.Connected && ipcClient.Write(Consts.MsgTypeId, mapx, 0))
                                {
                                    Console.WriteLine("Sent ACK - {0}", transId);
                                    transId++;
                                }
                            }
                            catch(Exception e) 
                            { 
                                Console.WriteLine("Failed to send heartbeat ack, error is {0}", e.ToString());
                            }

                            ackMsg = null;
                        }
                    }
                }
            }
        }

        public void SendServerConnect()
        {
            MediaServerMessage msg = new MediaServerMessage();
            msg.MessageId = IMediaServer.MSG_MS_CONNECT;
            msg.AddField(new Field(IMediaServer.FIELD_MS_TRANSACTION_ID, transId.ToString()));
            msg.AddField(new Field(IMediaServer.FIELD_MS_HEARTBEAT_INTERVAL, "1"));
            msg.AddField(new Field(IMediaServer.FIELD_MS_HEARTBEAT_PAYLOAD, IMediaServer.FIELD_MS_MEDIA_RES_PAYLOAD));
            msg.AddField(new Field(IMediaServer.FIELD_MS_SERVER_ID, serverId));
            msg.AddField(new Field(IMediaServer.FIELD_MS_CLIENT_ID, clientId));

            try
            {
                FlatmapList mapx = new FlatmapList();
                mapx.Add(Consts.MsgBodyId, msg.ToString());

                if (this.ipcClient.Write(Consts.MsgTypeId, mapx, 0))
                {
                    Console.WriteLine("Sent CONNECT - {0}", transId);
                    transId++;
                }
            }
            catch(Exception e) 
            { 
                Console.WriteLine("Failed to send server connect, error is {0}", e.ToString());
            }

            msg = null;
        }
    }
}
