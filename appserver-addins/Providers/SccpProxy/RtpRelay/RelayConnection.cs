using System;
using System.Net;
using System.Threading;
using System.Collections;
using System.Diagnostics;

using Metreos.LoggingFramework;
using Metreos.Core.IPC;
using Metreos.Core.IPC.Flatmaps;
using Metreos.Utilities;

namespace Metreos.Providers.SccpProxy.RtpRelay
{
    public delegate void ConnectionStatusDelegate(RelayConnection conn);

	/// <summary>Represents the connection to the RTP Relay server</summary>
    public class RelayConnection
    {
        public const int ConnectTimeout = 30;  // 30 seconds

        public ConnectionStatusDelegate OnConnected;
        public ConnectionStatusDelegate OnDisconnected;

        public int NumRelays { get { return rtpRelays.Count; } }

        /// <summary>Used for routing responses</summary>
        /// <remarks>RelayObjectId (int) -> Relay (object)</remarks>
        private IDictionary rtpRelays;
        private int lastRelayObjectId = 0;

        /// <summary>The connection to the RTP relay server</summary>
        protected IpcFlatmapClient ipcClient;

		public int ConnectionId
		{
			get { return connectionId; }
		}

        private int connectionId;

		/// <summary>
		/// Returns a number which quantifies the desireability of using
		/// this relay connection for the next relay. A lower value means
		/// more desireable. The most useful value to return is an
		/// estimate of the total number of relays carried by the service,
		/// not just the ones created by this connection, or perhaps the
		/// total cpu load on the server hosting the service.
		/// </summary>
		public int Desireability
		{
			get { return rtpRelays.Count; }
		}

        private LogWriter log;

        protected IPEndPoint relayIpcEP;
        public IPAddress RelayAddress { get { return relayIpcEP != null ? relayIpcEP.Address : null; } }

        /// <summary>Flag used during config refresh</summary>
        private bool confirmed = true;
        public bool Confirmed 
        {
            get { return confirmed; }
            set { confirmed = value; }
        }

		public RelayConnection(int connectionId, LogWriter log, IPEndPoint ipcAddr, RelayManager relayManager)
		{
            this.log = log;
            this.connectionId = connectionId;
			this.relayIpcEP = ipcAddr;
			this.relayManager = relayManager;

            this.rtpRelays = ReportingDict.Wrap( "RelayConnection.rtpRelays", Hashtable.Synchronized(new Hashtable()) );
            
            this.ipcClient = new IpcFlatmapClient(relayIpcEP);
			this.ipcClient.onConnect = new OnConnectDelegate(OnConnect);
			this.ipcClient.onFlatmapMessageReceived = new OnFlatmapMessageReceivedDelegate(HandleResponse);
			this.ipcClient.onClose = new OnCloseDelegate(OnConnectionClosed);
		}

		private RelayManager relayManager;

		public void Connect()
		{
			//Console.WriteLine( "relay starting {0}", Environment.StackTrace );
			ipcClient.Start();
		}

        public void Close()
        {
            ipcClient.Close();
        }

        public Relay CreateRelay(IPEndPoint remoteEP, int addrIndice)
        {
            int relayObjectId = Interlocked.Increment(ref lastRelayObjectId);
            Relay relay = new Relay(connectionId, relayObjectId, log, relayManager);

            this.rtpRelays[relayObjectId] = relay;

            log.Write(TraceLevel.Info, "Starting relay channel '{0}' on connection: {1}", relayObjectId, connectionId);

            if(relay.Start(remoteEP, addrIndice) == false)
            {
                this.rtpRelays.Remove(relayObjectId);
                log.Write(TraceLevel.Warning, "Connection '{0}' failed to create relay", this.connectionId);
                return null;
            }

            log.Write(TraceLevel.Info, "NumRelays = {0}", rtpRelays.Count);

            return relay;
        }

        public void SendMessage(RelayMsg msg)
        {
            FlatmapList flatmap = new FlatmapList();

            // Add relay object Id as request Id in order to route the response back
            flatmap.Add(Convert.ToUInt32(MsgApi.Fields.requestId), msg.RelayObjectId);

            foreach(DictionaryEntry de in msg.Fields)
            {
                if(de.Value != null)
                    flatmap.Add(Convert.ToUInt32(de.Key), de.Value);
            }
            
            if(ipcClient.Write((int)msg.Type, flatmap) == false)
            {
                log.Write(TraceLevel.Error, "Failed to send message to relay server:\n{0}", msg);
            }
        }

        private void HandleResponse(IpcFlatmapClient client, int messageType, FlatmapList flatmap)
        {
            int relayObjectId = Convert.ToInt32(flatmap.Find((uint)MsgApi.Fields.requestId, 1).dataValue);

            if((MsgApi.MsgTypes)messageType == MsgApi.MsgTypes.StopResp)
            {
                // This relay has asked to be stopped, remove it from the table
                this.rtpRelays.Remove(relayObjectId);
            }
            else
            {
                // Get relay object
                Relay relay = this.rtpRelays[relayObjectId] as Relay;
                if(relay == null)
                {
                    log.Write(TraceLevel.Error, "Received response for unknown relay channel: {0}", relayObjectId);
                    return;
                }
                relay.ProcessResponse(flatmap);            
            }
        }

        private void OnConnect(IpcClient c, bool reconnect)
		{
			whineAboutClose = true;

			log.Write(TraceLevel.Info, "Established connection to RTP relay {0} at {1}",
				connectionId, relayIpcEP);

            if(this.OnConnected != null)
                OnConnected(this);
		}

		private bool whineAboutClose = true;

		private void OnConnectionClosed(IpcClient c, Exception e)
		{
			if (whineAboutClose)
			{
				log.Write(TraceLevel.Warning, "Lost connection to RTP relay {0} at {1}: {2}",
					connectionId, relayIpcEP, e != null ? e.Message : "(no msg)");
				whineAboutClose = false;
			}

			if(this.OnDisconnected != null)
				OnDisconnected(this);
		}
    }
}
