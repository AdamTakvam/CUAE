using System;
using System.Net;
using System.Threading;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Core.IPC;
using Metreos.Core.IPC.Flatmaps;
using Metreos.Core.Sockets;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Utilities;

using Metreos.Configuration;

namespace Metreos.AppServer.Clustering
{
	/// <summary>Manages the IPC connection to other servers in the cluster</summary>
	public class ClusterInterface : PrimaryTaskBase
	{
        #region Constants

        private abstract class Consts
        {
            public const ushort ListenPort      = 9090;
            public const int NumTimerThreads    = 2;
            public const int MaxTimerThreads    = 2;

            public abstract class Messages
            {
                public const int Heartbeat      = 1;
                public const int HeartbeatAck   = 2;
                public const int Disconnect     = 3;
            }

            public abstract class Fields
            {
                public const int HeartbeatInterval  = 1;
                public const int FailoverState      = 2;
            }

            public abstract class Flatmaps
            {
                public static FlatmapList Heartbeat  = new FlatmapList();
                public static FlatmapList Disconnect = new FlatmapList();
            }
        }
        #endregion

        // Server-side stuff
        private IpcFlatmapServer ipcServer;
        private TimerHandle heartbeatTimer;
        private TimeSpan startupSyncTimeout;
        private IPAddress standbyAddr;
        private readonly ManualResetEvent clientConnected;
        private TimeSpan heartbeatInterval = TimeSpan.MinValue;
        private int socketId = -1;
        
        // Client-side stuff
        private IpcFlatmapClient ipcClient;
        private TimerHandle missedHeartbeatTimer;
        private TimeSpan incomingHbInterval;
        private TimeSpan maxHbInterval;
        private bool parentConnected = false;
        
        // Misc
        private readonly TimerManager timers;
        private bool started = false;

        // Children
        private readonly FailoverManager failoverManager;

        #region Construction/Startup/Refresh/Shutdown

		public ClusterInterface()
            : base(IConfig.ComponentType.Core, 
                IConfig.CoreComponentNames.CLUSTER_INTERFACE, 
                IConfig.CoreComponentNames.CLUSTER_INTERFACE, 
                Config.ClusterInterface.LogLevel,
                Config.Instance)
		{
            this.failoverManager = new FailoverManager(base.log, base.taskQueue);
            this.timers = new TimerManager(IConfig.CoreComponentNames.CLUSTER_INTERFACE,
                null, null, Consts.NumTimerThreads, Consts.MaxTimerThreads);
            this.clientConnected = new ManualResetEvent(false);
		}

        protected override void OnStartup()
        {
            this.started = true;
            RefreshConfiguration(null);

            this.failoverManager.Startup();

            if(ipcServer != null && startupSyncTimeout.Seconds > 0)
            {
                if(this.clientConnected.WaitOne(this.startupSyncTimeout, false) == false)
                {
                    log.Write(TraceLevel.Warning, "Unable to synch with standby at {0} within {1}s",
                        this.standbyAddr.ToString(), this.startupSyncTimeout.Seconds);
                }
                else
                {
                    log.Write(TraceLevel.Info, "Connected to standby ({0}). Status = {1}",
                        this.standbyAddr.ToString(), Config.ParentFailoverStatus.ToString());
                }
            }
            else
            {
                log.Write(TraceLevel.Warning, "This server is not configured with a hot-standby");
            }
        }

        protected override void RefreshConfiguration(string proxy)
        {
            if(!started)
                return;

            // Fetch startupSyncTimeout from DB
            this.startupSyncTimeout = new TimeSpan(0, 0, Config.ClusterInterface.StartupSyncTimeout);

            // Fetch parentAddr from DB
            IPAddress parentAddr = Config.ClusterInterface.ParentAddr;

            // Fetch clientAddr from DB
            IPAddress newStandbyAddr = Config.ClusterInterface.StandbyAddr;

            // Fetch incomingHbInterval from DB
            this.incomingHbInterval = new TimeSpan(0, 0, Config.ClusterInterface.HeartbeatInterval);

            // Fetch maxNumMissedHbs from DB
            int maxNumMissedHbs = Config.ClusterInterface.MaxMissedHeartbeats;
            this.maxHbInterval = new TimeSpan(0, 0, maxNumMissedHbs * incomingHbInterval.Seconds);

            if(ipcClient != null && !parentAddr.Equals(ipcClient.RemoteEp.Address))
            {
                log.Write(TraceLevel.Info, "Disconnecting from cluster parent: " + ipcClient.RemoteEp.Address);

                this.parentConnected = false;

                this.ipcClient.Close();
                this.ipcClient.Dispose();
                this.ipcClient = null;
            }
            
            if(this.ipcClient == null && !parentAddr.Equals(IPAddress.None))
            {
                log.Write(TraceLevel.Info, "Attempting to connect to cluster parent: " + parentAddr);

                this.ipcClient = CreateIpcClient(parentAddr);
                this.ipcClient.Start();
            }

            if(ipcServer != null && socketId >= 0 && !newStandbyAddr.Equals(this.standbyAddr))
            {
                log.Write(TraceLevel.Warning, "Disconnecting cluster standby: " + this.standbyAddr);
                ipcServer.Write(this.socketId, Consts.Messages.Disconnect, Consts.Flatmaps.Disconnect);
                ipcServer.CloseConnection(socketId);
            }

            if(!newStandbyAddr.Equals(IPAddress.None))
            {
                log.Write(TraceLevel.Info, "Accepting cluster standby connections from: " + newStandbyAddr);

                if(ipcServer == null)
                {
                    this.ipcServer = CreateIpcServer();
                    this.ipcServer.Start();
                }
            }
            else if(ipcServer != null)
            {
                log.Write(TraceLevel.Warning, "No longer accepting cluster standby connections");
                ipcServer.Stop();
                ipcServer.Dispose();
                ipcServer = null;
            }

            this.standbyAddr = newStandbyAddr;
        }

        protected override void OnShutdown()
        {
            if(this.ipcClient != null)
            {
                this.ipcClient.Close();
                this.ipcClient.Dispose();
                this.ipcClient = null;
            }

            if(this.ipcServer != null)
            {
                this.ipcServer.Stop();
                this.ipcServer.Dispose();
                this.ipcServer = null;
            }

            this.timers.Shutdown();
        }

        protected override TraceLevel GetLogLevel()
        {
            return Config.ClusterInterface.LogLevel;
        }

        #endregion

        #region Internal message handler

        protected override bool HandleMessage(InternalMessage message)
        {
            CommandMessage cMsg = message as CommandMessage;
            if(cMsg != null)
            {
                switch(cMsg.MessageId)
                {
                    case ICommands.PRINT_DIAGS:
                        HandlePrintDiags();
                        return true;
                }
                return false;
            }

            ResponseMessage rMsg = message as ResponseMessage;
            if(rMsg != null)
            {
                switch(rMsg.InResponseTo)
                {
                    case ICommands.REFRESH_PROVIDERS:
                        this.failoverManager.HandleRefreshResponse();
                        return true;
                }
            }

            return false;
        }
        #endregion

        #region AppServer command handlers

        private void HandlePrintDiags()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("[Clustering Diagnostics]\r\n");
            sb.Append("Connected to parent : ");
            sb.Append(this.parentConnected ? "True" : "False");
            sb.Append("\r\n");
            sb.Append("Connected to standby: ");
            sb.Append(this.socketId >= 0 ? "True" : "False");
            sb.Append("\r\n");
            sb.Append(failoverManager.GetDiagMsg());

            log.ForceWrite(TraceLevel.Info, sb.ToString());
        }
        #endregion

        #region Client callbacks

        private void OnClientClosed(IpcClient client, Exception e)
        {
            if(this.parentConnected)
            {
                if(e == null)
                    log.Write(TraceLevel.Warning, "Cluster parent disconnected");
                else
                    log.Write(TraceLevel.Warning, "Cluster parent connection lost: " + e.Message);

                if(this.missedHeartbeatTimer != null)
                    this.missedHeartbeatTimer.Cancel();

                this.parentConnected = false;
                this.failoverManager.HandleParentConnectionFailure();
            }
        }

        private void OnClientConnected(IpcClient client, bool reconnect)
        {
            // Note: We're not fully connected until a heartbeat is received
        }

        private void ClientHandleMessage(IpcFlatmapClient client, int messageType, FlatmapList message)
        {
            switch(messageType)
            {
                case Consts.Messages.Heartbeat:
                    OnHeartbeat(message);
                    break;
                case Consts.Messages.Disconnect:
                    OnDisconnect();
                    break;
                default:
                    log.Write(TraceLevel.Warning, "Received unknown message from cluster parent: {0}",
                        messageType);
                    break;
            }
        }

        private void OnDisconnect()
        {
            log.Write(TraceLevel.Warning, "The cluster parent has permanently severed the cluster link. " +
                "This server is no longer a standby");

            if(this.missedHeartbeatTimer != null)
                this.missedHeartbeatTimer.Cancel();

            this.parentConnected = false;
            
            if(this.ipcClient != null)
            {
                this.ipcClient.Close();
                this.ipcClient.Dispose();
                this.ipcClient = null;
            }

            Config.ClusterInterface.RemoveParentAddr();
        }

        private void OnHeartbeat(FlatmapList message)
        {
            if(!this.parentConnected)
            {
                log.Write(TraceLevel.Info, "Cluster parent connected: {0}",
                    this.ipcClient.RemoteEp.Address.ToString());

                this.parentConnected = true;
                this.failoverManager.HandleParentConnectionRestored();
            }
            else
            {
                log.Write(TraceLevel.Verbose, "Cluster heartbeat received");
            }

            SendHeartbeatAck();

            if(this.missedHeartbeatTimer == null)
                this.missedHeartbeatTimer = timers.Add(Convert.ToInt64(this.maxHbInterval.TotalMilliseconds), 
                    new WakeupDelegate(OnHeartbeatTimeout));
            else
                this.missedHeartbeatTimer.Reschedule(Convert.ToInt64(this.maxHbInterval.TotalMilliseconds));
        }

        private void SendHeartbeatAck()
        {
            FlatmapList hbAck = new FlatmapList();
            hbAck.Add((int)Consts.Fields.FailoverState, (int)Config.StandbyFailoverStatus);
            hbAck.Add((int)Consts.Fields.HeartbeatInterval, this.incomingHbInterval.Seconds);
            
            this.ipcClient.Write(Consts.Messages.HeartbeatAck, hbAck);
        }

        private long OnHeartbeatTimeout(TimerHandle timer, object state)
        {
            log.Write(TraceLevel.Error, "Missed too many heartbeats from cluster parent. Assuming failure.");

            this.parentConnected = false;
            this.failoverManager.HandleParentConnectionFailure();

            if(this.ipcClient != null)
            {
                log.Write(TraceLevel.Verbose, "Attempting to reconnect to cluster parent: {0}",
                    ipcClient.RemoteEp.Address.ToString());

                this.ipcClient.Close();
                this.ipcClient.Start();
            }

            return 0;
        }
        #endregion

        #region Server callbacks

        private void OnServerClosed(int socketId)
        {
            log.Write(TraceLevel.Warning, "Cluster standby disconnected");

            if(this.heartbeatTimer != null)
            {
                this.heartbeatTimer.Cancel();
                this.heartbeatTimer = null;
            }

            this.socketId = -1;
            this.heartbeatInterval = TimeSpan.MinValue;
            this.clientConnected.Reset();
        }

        private void OnServerConnected(int sId, string remoteHost)
        {
            // Rip off port portion of remote host address
            int colonIndex = remoteHost.IndexOf(":");
            if(colonIndex > -1)
                remoteHost = remoteHost.Substring(0, colonIndex);

            IPAddress incomingClient = null;
            try { incomingClient = IPAddress.Parse(remoteHost); }
            catch 
            {
                this.ipcServer.CloseConnection(sId);
                log.Write(TraceLevel.Warning, "Rejecting standby connection from: {0} (Could not determine IP address)",
                    remoteHost);
                return;
            }

            if(incomingClient.Equals(standbyAddr))
            {
                if(this.socketId > -1)
                {
                    log.Write(TraceLevel.Warning, "Cluster standby has opened a new connection. Closing old one");
                    this.ipcServer.CloseConnection(this.socketId);
                }
                else
                {
                    log.Write(TraceLevel.Info, "Cluster standby connected: {0}", incomingClient.ToString());
                }

                this.socketId = sId;

                // We're still not *really* connected yet. We need to perform one ping transaction first.
                if(this.ipcServer.Write(socketId, Consts.Messages.Heartbeat, Consts.Flatmaps.Heartbeat) == false)
                {
                    log.Write(TraceLevel.Error, "Failed to send heartbeat to cluster standby: {0}",
                        incomingClient.ToString());
                }
            }
            else
            {
                this.ipcServer.CloseConnection(sId);

                log.Write(TraceLevel.Warning, "A standby connect was attempted by an unauthorized entity: {0}",
                    incomingClient.ToString());
            }
        }

        private void ServerHandleMessage(int socketId, string receiveInterface, int messageType, FlatmapList message)
        {
            switch(messageType)
            {
                case Consts.Messages.HeartbeatAck:
                    OnHeartbeatAck(message);
                    break;
                default:
                    log.Write(TraceLevel.Warning, "Received unknown message from cluster standby: {0}",
                        messageType);
                    break;
            }
        }

        private void OnHeartbeatAck(FlatmapList message)
        {
            // Get failover state
            int failoverStatus = Convert.ToInt32(message.Find(Consts.Fields.FailoverState, 1).dataValue);
            this.failoverManager.SetParentStatus((IConfig.FailoverStatus)failoverStatus);

            // Get ping interval
            int hbInterval = Convert.ToInt32(message.Find(Consts.Fields.HeartbeatInterval, 1).dataValue);
            if(hbInterval == 0 && this.heartbeatInterval == TimeSpan.MinValue)
            {
                log.Write(TraceLevel.Error, "Missing heartbeat interval in initial heartbeat ACK");
                return;
            }
            
            if(hbInterval > 0 && hbInterval != this.heartbeatInterval.Seconds)
            {
                this.heartbeatInterval = new TimeSpan(0, 0, hbInterval);
                
                if(this.heartbeatTimer == null)                
                    this.heartbeatTimer = this.timers.Add(Convert.ToInt64(heartbeatInterval.TotalMilliseconds), 
                        new WakeupDelegate(SendHeartbeat));
                else
                    this.heartbeatTimer.Reschedule(Convert.ToInt64(heartbeatInterval.TotalMilliseconds));
            }

            log.Write(TraceLevel.Verbose, "Cluster heartbeat ACK received: state={0}, interval={1}",
                ((IConfig.FailoverStatus)failoverStatus).ToString(), hbInterval);

            this.clientConnected.Set();
        }

        private long SendHeartbeat(TimerHandle timer, object state)
        {
            if(this.ipcServer.Write(socketId, Consts.Messages.Heartbeat, Consts.Flatmaps.Heartbeat) == false)
            {
                log.Write(TraceLevel.Error, "Failed to send heartbeat to cluster standby: {0}",
                    standbyAddr.ToString());
                return 0;
            }
            
            log.Write(TraceLevel.Verbose, "Sending cluster heartbeat");
            return Convert.ToInt64(heartbeatInterval.TotalMilliseconds);
        }
        #endregion

        #region Helpers

        private IpcFlatmapClient CreateIpcClient(IPAddress parentAddr)
        {
            IpcFlatmapClient client = null;
            if(parentAddr != null)
            {
                IPEndPoint parentEP = new IPEndPoint(parentAddr, Consts.ListenPort);
                client = new IpcFlatmapClient(parentEP);
            }
            else
            {
                client = new IpcFlatmapClient();
            }

            client.onClose += new OnCloseDelegate(OnClientClosed);
            client.onConnect += new OnConnectDelegate(OnClientConnected);
            client.onFlatmapMessageReceived += new OnFlatmapMessageReceivedDelegate(ClientHandleMessage);
            return client;
        }

        private IpcFlatmapServer CreateIpcServer()
        {
            IpcFlatmapServer server = new IpcFlatmapServer(IConfig.CoreComponentNames.CLUSTER_INTERFACE,
                Consts.ListenPort, false, TraceLevel.Warning);

            server.OnCloseConnection += new CloseConnectionDelegate(OnServerClosed); 
            server.OnNewConnection += new NewConnectionDelegate(OnServerConnected);
            server.OnMessageReceived += new IpcFlatmapServer.OnMessageReceivedDelegate(ServerHandleMessage);
            return server;
        }

        #endregion
    }
}
