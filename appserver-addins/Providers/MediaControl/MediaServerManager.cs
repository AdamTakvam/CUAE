using System;
using System.Net;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;

using Metreos.Stats;
using Metreos.Core;
using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;
using Metreos.ProviderFramework;
using Metreos.Interfaces;
using Metreos.Utilities;

namespace Metreos.MediaControl
{
    // Delegates for provider callbacks
    internal delegate void SendMsFailureResponseToAppDelegate(ActionBase action, string msResultCode, params string[] errorCodes);
    internal delegate void ForwardMsMessageToApplicationDelegate(TransactionInfo trans, MediaServerMessage msMsg, IMediaServer.Results resultCode);
    internal delegate void SendMsFinalAsyncResponseToAppDelegate(AsyncAction action, IMediaServer.Results resultCode, MediaServerMessage msMsg);
    internal delegate void SendDigitsEventDelegate(uint connectionId, string routingGuid, string digits);

    /// <summary>Facilitates all communication with media servers.</summary>
    /// <remarks>
    /// This class is solely concerned with clustering, load-balancing, and fault-tolerance
    ///   of a media server pool. Issues involving individual connections on a given
    ///   media server are left to that media server to handle. Thus, no connection
    ///   or conference state is maintained here.
    /// The data structure is as follows:
    ///   Media Servers collection (mmsID -> MediaServerInfo)
    ///     MediaServerInfo contains:
    ///     - Server metadata
    ///     - Connection state (transport-layer)
    ///     - Pending transactions collection (transId -> TransactionInfo)
    ///       TransactionInfo contains:
    ///       - Sync Transactions (connect, mute, unmute, etc)
    ///       - Async Transactoins (play, record, etc)
    ///       - Connect transactions (used to establish initial link to MMS)
    ///       - Heartbeat transactions (used to determine if a media server is gone (MSMQ))
    ///       Transactions contain:
    ///         - The action which triggered the transaction, if applicable
    ///         - The server ID
    ///         - The transaction ID
    ///         - A timer handle
    ///         When the timer fires, the transaction has expired and an error response is sent
    ///         (if an action is associated with the transaction) or some type of bookkeeping occurs 
    ///         if it's an internally-generated transaction.
    /// </remarks>
    public class MediaServerManager : Loggable 
    {
        private abstract class Consts
        {
            public const int StartupTimeout     = 10000;
            public const int ShutdownTimeout    = 3000;

            public abstract class Defaults
            {
                public const long TransactionTimeout = 5000;  // 5 secs
                public const int InitTimerThreads    = 3;
                public const int MaxTimerThreads     = 10;
            }
        }

        internal SendMsFailureResponseToAppDelegate SendMsFailureResponseToApp;
        internal ForwardMsMessageToApplicationDelegate ForwardMsMessageToApplication;
        internal SendMsFinalAsyncResponseToAppDelegate SendMsFinalAsyncResponseToApp;
        internal SendDigitsEventDelegate SendDigitsEvent;
       
        private readonly IConfigUtility configUtility;
        private readonly StatsClient statsClient;

        /// <summary>Indicates whether the media server manager has started.</summary>
        private bool managerStarted = false;

        /// <summary>Indicates to all threads that a shutdown has been requested</summary>
        private volatile bool shuttingDown = false;

        /// <summary>The requested timeout for heart beats.</summary>
        private int msHeartbeatRequestedInterval;

        /// <summary>The value used by the heartbeat timeout timer.</summary>
        private int msHeartbeatTimeoutValue;

        /// <summary>Collection of all configured media servers</summary>
        /// <remarks>Media server ID (uint) -> MediaServerInfo</remarks>
        internal readonly MediaServerCollection mediaServers;

        /// <summary>Media Resource Group cache</summary>
        /// <remarks>Used to minimize DB dips under load</remarks>
        private readonly MrgCache mrgCache;

        /// <summary>Premade heartbeat ACK</summary>
        private readonly MediaServerMessage heartbeatAckMsg;

        /// <summary>Single-threaded timer manager (doesn't use .NET threadpool)</summary>
        private static TimerManager timerManager;
        internal static TimerManager TimerManager { get { return timerManager; } }
        
        /// <summary>Delegate fired when a transaction timed out</summary>
        private readonly WakeupDelegate onTransactionTimeout;

        private int msConnectTimeoutValue;
        private bool diagInboundConnectMessages;
        private bool diagOutboundConnectMessages;
        private bool diagOutboundDisconnectMessages;
        private bool diagOutboundCommandMessages;
        private bool diagInboundResponseMessages;
        private bool diagResourceInfo;
        private bool diagServerSelection;
        private bool diagOutputTransactionTime;

        #region Set/Get Config Properties

        public int MediaServerConnectTimeout { set { this.msConnectTimeoutValue = value; } }

        public bool DiagInboundConnectMessages
        {
            get { return this.diagInboundConnectMessages; } 
            set { this.diagInboundConnectMessages = value; }
        }

        public bool DiagOutboundConnectMessages
        {
            get { return this.diagOutboundConnectMessages; } 
            set { this.diagOutboundConnectMessages = value; }
        }

        public bool DiagOutboundDisconnectMessages
        {
            get { return this.diagOutboundDisconnectMessages; } 
            set { this.diagOutboundDisconnectMessages = value; }
        }

        public bool DiagOutboundCommandMessages
        {
            get { return this.diagOutboundCommandMessages; } 
            set { this.diagOutboundCommandMessages = value; }
        }

        public bool DiagInboundResponseMessages
        {
            get { return this.diagInboundResponseMessages; } 
            set { this.diagInboundResponseMessages = value; }
        }

        public bool DiagResourceInfo
        {
            get { return this.diagResourceInfo; } 
            set { this.diagResourceInfo = value; }
        }

        public bool DiagServerSelection
        {
            get { return this.diagServerSelection; } 
            set { this.diagServerSelection = value; }
        }

        public bool DiagOutputTransactionTime
        {
            get { return this.diagOutputTransactionTime; }
            set { this.diagOutputTransactionTime = value; }
        }

        #endregion

        #region Construction/Initialization

        internal MediaServerManager(TraceLevel logLevel, IConfigUtility configUtility) 
            : base(logLevel, typeof(MediaControlProvider).Name)
        {
            this.configUtility = configUtility;
            this.statsClient = StatsClient.Instance;

            mediaServers = new MediaServerCollection();

            this.heartbeatAckMsg = new MediaServerMessage(IMediaServer.Messages.Heartbeat);

            // Timer call backs for our heart beat and connect timers.
            this.onTransactionTimeout = new WakeupDelegate(this.OnTransactionTimeout);

            timerManager = new TimerManager("Media server timers", onTransactionTimeout, null, 
                Consts.Defaults.InitTimerThreads, Consts.Defaults.MaxTimerThreads);

            this.mrgCache = new MrgCache(log, configUtility);

            MediaServerInfo.HandleMediaServerMessage = new HandleMmsMessageDelegate(HandleMediaServerMessage);
            MediaServerInfo.ConnectToMediaServer = new MsInfoDelegate(ConnectToMediaServer);
        }

        /// <summary>Sets heartbeat config values</summary>
        /// <param name="hbInterval">Heartbeat interval</param>
        /// <param name="hbSkew">Heartbeat skew</param>
        internal void SetHeartbeatConfig(int hbInterval, int hbSkew)
        {
            this.msHeartbeatRequestedInterval = hbInterval;
            this.msHeartbeatTimeoutValue = (hbInterval + hbSkew) * 1000;
        }

        /// <summary>
        /// Instruct the Media Server Manager to refresh its configuration.
        /// </summary>
        /// <remarks>
        /// This is a public interface to the kick off a refresh.  This is
        /// used by the Media Server Provider to instruct the manager to
        /// refresh.
        /// </remarks>
        internal void Refresh()
        {
            this.RefreshMediaServerList();
        }

        /// <summary>Instruct the MSM to start trying to connect to media servers</summary>
        internal void Startup()
        {
            this.managerStarted = true;

            lock(mediaServers.SyncRoot)
            {
                foreach(DictionaryEntry de in mediaServers)
                {
                    MediaServerInfo msInfo = de.Value as MediaServerInfo;
                    ConnectToMediaServer(msInfo);
                }
            }
        }

        /// <summary>Instruct the MSM to close connections and shutdown</summary>
        internal void Shutdown()
        {
            this.shuttingDown = true;

            lock(mediaServers.SyncRoot)
            {
                foreach(DictionaryEntry de in mediaServers)
                {
                    MediaServerInfo msInfo = de.Value as MediaServerInfo;
                    DisconnectFromMediaServer(msInfo);

                    configUtility.UpdateStatus(IConfig.ComponentType.MediaServer, msInfo.MediaServerName, IConfig.Status.Enabled_Stopped);
                }

                timerManager.Shutdown();

                mediaServers.Clear();
            }
        }

        public override void Dispose()
        {
            statsClient.Dispose();

            base.Dispose();
        }
        #endregion

        #region Media server connection methods
        /// <summary>
        /// Adds a media server to the media server manager.
        /// </summary>
        /// <param name="msComponent">The component information for the new media server.</param>
        /// <returns>A MediaServerInfo object for the new media server.  If the 
        /// manager fails to add the media server then null is returned.</returns>
        private MediaServerInfo AddMediaServer(ComponentInfo msComponent)
        {
            Assertion.Check(msComponent != null, "msComponent null in AddMediaServer()");

            // Retrieve the IP address of the media server
            System.Net.IPAddress ipAddress = configUtility.GetEntryValue(IConfig.ComponentType.MediaServer, 
                msComponent.name, IConfig.Entries.Names.ADDRESS) as System.Net.IPAddress;

            if(ipAddress == null)
            {
                log.Write(TraceLevel.Error,
                    "Media server '{0}' has an invalid IP address. Media server not added.",
                    msComponent.name);
                return null;
            }

            //
            // If there were more than one kind of media server connection,
            //   this is where the abstraction logic would go.
            //

            // Retrieve the connection type of the media server
            //string connectionType = configUtility.GetEntryValue(IConfig.ComponentType.MediaServer, 
            //    msComponent.name, IConfig.Entries.Names.CONNECTION_TYPE) as string;

            uint serverId = GenerateServerId();
            MediaServerInfo msInfo = null;

            // Retrieve IPC port of the media server
            uint port = Convert.ToUInt32(configUtility.GetEntryValue(IConfig.ComponentType.MediaServer,
                msComponent.name, IConfig.Entries.Names.LISTEN_PORT));

            try
            {
                msInfo = new MediaServerInfoIPC(msComponent.name, serverId, ipAddress, port, log, statsClient);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
            }

            Assertion.Check(mediaServers[serverId] == null, 
                "MediaServerInfo already contains nextServerId in AddMediaServer()");

            mediaServers.Add(msInfo);
            
            log.Write(TraceLevel.Info, "Adding media server {0} ({1})", msInfo.MediaServerName, msInfo.Address);

            return msInfo;
        }

        /// <summary>Establish a connection to a media server.</summary>
        /// <param name="msInfo">The MediaServerInfo object of the media server to
        /// connect to.</param>
        protected void ConnectToMediaServer(MediaServerInfo msInfo)
        {
			if(shuttingDown || msInfo == null || msInfo.ConnectedToMediaServer || msInfo.DisconnectRequested)
                return;

            // Ensures that two connect messages don't slip through
            lock(msInfo.ConnectLock)
            {
                if(msInfo.ConnectPending)
                {
                    log.Write(TraceLevel.Verbose, "Not sending initial connect to {0} ({1}) because another is pending",
                        msInfo.MediaServerName, msInfo.Address.ToString());
                    return;
                }

                // Output a log warning if for every 20 failed connect attempts.
                if((msInfo.MmsConnectAttempts % 20 == 0) && (msInfo.MmsConnectAttempts > 0))
                {
                    log.Write(TraceLevel.Info, "Unable to connect to media server {0} ({1}). Retrying...", 
                        msInfo.MediaServerName, msInfo.Address.ToString());
                }
            
                msInfo.MmsConnectAttempts++;

                TransactionInfo transaction = new InitConnectTransactionInfo(msInfo.ServerId, this.msConnectTimeoutValue);
                transaction.ServerId = msInfo.ServerId;
                msInfo.AddTransaction(transaction);

                MediaServerMessage connectMsg = new MediaServerMessage(IMediaServer.Messages.Connect);
                connectMsg.AddField(IMediaServer.Fields.HeartbeatInterval, this.msHeartbeatRequestedInterval.ToString());
                connectMsg.AddField(IMediaServer.Fields.HeartbeatPayload, IMediaServer.Fields.MediaResources);
                connectMsg.AddField(IMediaServer.Fields.ServerId, msInfo.ServerId.ToString());
                connectMsg.AddField(IMediaServer.Fields.TransactionId, transaction.ID.ToString());
				connectMsg.AddField(IMediaServer.Fields.ClientId, "0");
            
                // Check one more time to make sure we didn't get a shutdown request 
                // while we were processing the connect message.
                if(shuttingDown == false && !msInfo.ConnectedToMediaServer)
                {
                    log.WriteIf(diagOutboundConnectMessages, TraceLevel.Info, "Sending initial connect message to {0} ({1}):\n{2}",
                        msInfo.MediaServerName, msInfo.Address.ToString(), connectMsg.ToString());

                    try { msInfo.SendCommand(connectMsg, true); }
                    catch(Exception e)
                    {
                        log.WriteIf(diagOutboundConnectMessages, TraceLevel.Warning, 
                            "Failed to send initial connect: " + e.Message);

                        transaction.Complete();
                        msInfo.RemoveTransaction(transaction.ID);
                    }
                }
                else
                {
                    transaction.Complete();
                    msInfo.RemoveTransaction(transaction.ID);
                }
            }
        }

        /// <summary>Refresh the list of media servers.</summary>
        /// <remarks>
        /// If the media server manager is currently started, then maintenace
        /// will be performed on the media server list.  Otherwise, the 
        /// media server list will be constructed for the first time.
        /// </remarks>
        private void RefreshMediaServerList()
        {
            ComponentInfo[] mediaServers = 
                configUtility.GetComponents(IConfig.ComponentType.MediaServer);

            ArrayList prunedList = new ArrayList();

            if(mediaServers != null)
            {
                prunedList.AddRange(mediaServers);
                foreach(ComponentInfo cInfo in new ArrayList(prunedList))
                {
                    IConfig.Status status = configUtility.GetStatus(IConfig.ComponentType.MediaServer, cInfo.name);
                    if(status != IConfig.Status.Enabled_Stopped && status != IConfig.Status.Enabled_Running)
                        prunedList.Remove(cInfo);
                }
            }

            if(this.managerStarted == true)
            {
                RefreshMediaServerListMaintenance(prunedList);
            }
            else
            {
                BuildInitialMediaServerList(prunedList);
            }
        }

        /// <summary>
        /// Builds the initial list of media servers that the media server
        /// manager will attempt to connect to.
        /// </summary>
        /// <param name="mediaServers">Array of IConfig.ComponentInfo objects  
        /// containing the media servers to be connected to.</param>
        private void BuildInitialMediaServerList(ArrayList components)
        {
            Assertion.Check(mediaServers.Count == 0, "MediaServerInfo not empty in BuildInitialMediaServerList()");

            if(components != null)
            {
                foreach(ComponentInfo comp in components)
                {
                    AddMediaServer(comp);
                }
            }
        }

        private uint GenerateServerId()
        {
            // Search for a null entry in the MediaServerInfo hash
            lock(mediaServers.SyncRoot)
            {
                foreach(DictionaryEntry de in mediaServers)
                {
                    if(de.Value == null)
                    {
                        return Convert.ToUInt32(de.Key);
                    }
                }
            }
            return ServerIdFactory.GenerateId();
        }

        /// <summary>
        /// Performs maintenance on the media server list.  Maintenance
        /// on the media server list will only be performanced after the
        /// manager is started.
        /// </summary>
        /// <remarks>
        /// First, the media server manager determines which media servers
        /// have been removed from the system and disconnects from them. 
        /// It then determines which media servers are new and attempts to
        /// establish a connection to them.
        /// </remarks>
        protected void RefreshMediaServerListMaintenance(ArrayList components)
        {
            // Build list of DB media server addresses
            Hashtable componentAddrs = new Hashtable();
            if(components != null)
            {
                foreach(ComponentInfo comp in components)
                {
                    // Retrieve the IP address of the media server
                    System.Net.IPAddress storedAddress = configUtility.GetEntryValue(IConfig.ComponentType.MediaServer, 
                        comp.name, IConfig.Entries.Names.ADDRESS) as System.Net.IPAddress;

                    if(storedAddress == null)
                    {
                        log.Write(TraceLevel.Error, "No address found for media server: " + comp.name);
                        continue;
                    }

                    componentAddrs[comp.ID] = storedAddress;
                }
            }

            // Build a list of the media servers that are no longer present.
            ArrayList removedServerIds = new ArrayList();
            lock(mediaServers.SyncRoot)
            {
                foreach(DictionaryEntry de in mediaServers)
                {
                    MediaServerInfo msInfo = de.Value as MediaServerInfo;
                    if(msInfo == null)
                        continue;

                    bool found = false;

                    foreach(IPAddress addr in componentAddrs.Values)
                    {
                        if(msInfo.Address.Equals(addr))
                        {
                            found = true;
                            break;
                        }
                    }

                    if(found == false)
                        removedServerIds.Add(msInfo.ServerId);
                }
            }

            // Disconnect from the removed media servers.
            DisconnectFromRemovedMediaServers(removedServerIds);

            // Connect to all of the new media servers.
            if(components != null)
            {
                foreach(ComponentInfo comp in components)
                {
                    bool installed = false;

                    lock(mediaServers.SyncRoot)
                    {
                        foreach(DictionaryEntry de in mediaServers)
                        {
                            MediaServerInfo msInfo = de.Value as MediaServerInfo;
                            if(msInfo == null)
                                continue;

                            if(msInfo.Address.Equals(componentAddrs[comp.ID]))
                            {
                                installed = true;
                                break;
                            }
                        }
                    }

                    if(!installed)
                    {
                        MediaServerInfo msInfo = AddMediaServer(comp);

                        if(msInfo != null)
                            ConnectToMediaServer(msInfo);
                    }
                }
            }
        }

        /// <summary>
        /// Disconnects from a group of media servers that have been removed from
        /// the configured media server list.
        /// </summary>
        /// <param name="removedServerIds">An ArrayList of server IDs to disconnect from.</param>
        protected void DisconnectFromRemovedMediaServers(ArrayList removedServerIds)
        {
            Assertion.Check(removedServerIds != null, "removedServerIds is null in DisconnectFromRemovedMediaServers()");

            uint serverId = 0;

            for(int i = 0; i < removedServerIds.Count; i++)
            {
                serverId = (uint)removedServerIds[i];

                MediaServerInfo msInfo = mediaServers[serverId];
                Assertion.Check(msInfo != null, "msInfo is not null in DisconnectFromRemovedMediaServers()");

                log.Write(TraceLevel.Info, "Disconnecting from removed media server {0} ({1})", 
                    msInfo.MediaServerName, msInfo.Address.ToString());

                DisconnectFromMediaServer(msInfo);
            }
        }
        
        /// <summary>Disconnect from a single media server.</summary>
        /// <param name="msInfo">The MediaServerInfo object of the media server to disconnect from.</param>
        protected void DisconnectFromMediaServer(MediaServerInfo msInfo)
        {
            if(msInfo == null)
            {
                Debug.Fail("msInfo is null in DisconnectFromMediaServer()");
                return;
            }

			msInfo.DisconnectRequested = true;

			if(msInfo.ConnectedToMediaServer)
			{
				SyncTransactionInfo transaction = new SyncTransactionInfo(null, Consts.Defaults.TransactionTimeout, false);
				transaction.ServerId = msInfo.ServerId;
				msInfo.AddTransaction(transaction);

                MediaServerMessage disconnectMsg = new MediaServerMessage(IMediaServer.Messages.Disconnect);
				disconnectMsg.AddField(IMediaServer.Fields.ClientId, msInfo.ClientId);
				disconnectMsg.AddField(IMediaServer.Fields.ServerId, msInfo.ServerId.ToString());
				disconnectMsg.AddField(IMediaServer.Fields.TransactionId, transaction.ID.ToString());

				log.WriteIf(diagOutboundDisconnectMessages, TraceLevel.Info, "Sending final disconnect to {0} ({1})",
					msInfo.MediaServerName, msInfo.Address.ToString());

				try { msInfo.SendCommand(disconnectMsg, false); }
				catch
				{
					transaction.Complete();
				}
			}

            KillMsConnection(msInfo);
        }
        #endregion
        
        #region Message Sender

        internal bool SendCommandToMediaServer(MediaServerMessage commandMsg, TransactionInfo trans)
        {
            if(commandMsg == null)
            {
                Debug.Fail("SendCommandToMediaServer: commandMsg is null");
                return false;
            }
            if(trans == null)
            {
                Debug.Fail("SendCommandToMediaServer: transaction is null");
                return false;
            }

            // Save the message in the action object in case we have to retransmit it
            trans.Action.MediaServerMessage = commandMsg;
            
            MediaServerInfo msInfo = null;
            if(trans.ServerId > 0)
            {
                // Fetch msInfo from table
                msInfo = mediaServers[trans.ServerId];
                if(msInfo == null)
                {
                    log.Write(TraceLevel.Warning, "Could not send command to specified MMS ({0}):\n{1}",
                        trans.ServerId, commandMsg);
                    return false;
                }
            }
            else if(trans.Action != null && commandMsg.MessageId == IMediaServer.Messages.Connect)
            {
                // Dig out app name and partition name
                string appName = trans.Action.OriginalAction.InnerMessage.AppName;
                string partName = trans.Action.OriginalAction.InnerMessage.PartitionName;

                // Select best server from MRG
                MediaServerInfo[] servers = this.GetMediaServers(appName, partName, false);
                if(servers == null || servers.Length == 0)
                {
                    log.Write(TraceLevel.Error, "No media resource group configured for {0}:{1}", appName, partName);
                    MediaControlProvider.HandleFatalError(trans.Action, IMediaControl.ResultCodes.OperationTimeout);
                    return false;
                }
                
                bool success = FindBestMediaServer(trans.Action.TxCodec, trans.Action.RxCodec, servers, out msInfo);

                if(!success)
                {
                    // Try failover group
                    servers = this.GetMediaServers(appName, partName, true);
                    if(servers != null && servers.Length > 0)
                    {
                        success = FindBestMediaServer(trans.Action.TxCodec, trans.Action.RxCodec, servers, out msInfo);
                    }
                }

                if(!success)
                {
                    log.Write(TraceLevel.Warning, "Could not locate a suitable media server for action:\n" +
                        trans.Action.ToString());
                    return false;
                }

                trans.ServerId = msInfo.ServerId;
            }
            else
            {
                log.Write(TraceLevel.Error, "Cannot send command. Could not determine server ID from action:\n" + 
					trans.Action.OriginalAction.InnerMessage);
                return false;
            }

            // Store transaction
            msInfo.AddTransaction(trans);

            commandMsg.AddField(IMediaServer.Fields.TransactionId, trans.ID.ToString());
        
            if (commandMsg.ContainsField(IMediaServer.Fields.ClientId) == false)
            {
                commandMsg.AddField(IMediaServer.Fields.ClientId, msInfo.ClientId);
            }

            if( (diagOutboundCommandMessages) ||
                (diagOutboundConnectMessages && commandMsg.MessageId == IMediaServer.Messages.Connect) ||
                (diagOutboundDisconnectMessages && commandMsg.MessageId == IMediaServer.Messages.Disconnect))
            {
                log.Write(TraceLevel.Info, "Sending command to {0} ({1})\n{2}", msInfo.MediaServerName, 
                    msInfo.Address.ToString(), commandMsg.ToString());
            }

            try
            {
                msInfo.SendCommand(commandMsg, false);
            }
            catch(Exception e) 
            {
                trans.Complete();
                msInfo.RemoveTransaction(trans.ID);

				SyncTransactionInfo sTrans = trans as SyncTransactionInfo;
				if(sTrans != null)
				{
					sTrans.FailedServers.Add(msInfo.ServerId);
					return Failover(sTrans);
				}
			
				if(!msInfo.DisconnectRequested)
					PrintSendException(e, commandMsg);
				return false; 
			}

            log.Write(TraceLevel.Verbose, "{0} message sent successfully", commandMsg.MessageId);
            return true;
        }

		/// <summary>Try connecting to another media server if the first attempt failed</summary>
		/// <param name="trans">The failed, and defunct, transaction</param>
		/// <returns>success</returns>
		/// <remarks>
		/// If we failed to create a connection to a media server and the app did not specify a server ID,
		///   then we attempt to locate another server in the app's MRG to service the request.
		/// </remarks>
		private bool Failover(SyncTransactionInfo trans)
		{
			// Don't attempt to failover internal commands
			if(trans == null || trans.Action == null)
				return false;

			// A particular server is implied three ways. If any of these exist, we cannot fail over.
			if(trans.Action.MmsId > 0 || trans.Action.ConfId > 0 || trans.Action.ConnId > 0)
				return false;

			string appName = trans.Action.OriginalAction.InnerMessage.AppName;
			string partName = trans.Action.OriginalAction.InnerMessage.PartitionName;

			MediaServerInfo[] servers = GetMediaServers(appName, partName, false);
			servers = PruneServerList(servers, trans.FailedServers);

			MediaServerInfo newServer = null;
            if(!FindBestMediaServer(trans.Action.TxCodec, trans.Action.RxCodec, servers, out newServer))
            {
                // Try failover MRG
                servers = GetMediaServers(appName, partName, true);
                servers = PruneServerList(servers, trans.FailedServers);
                if(!FindBestMediaServer(trans.Action.TxCodec, trans.Action.RxCodec, servers, out newServer))
                    return false;
            }

			// Create a new transaction
			TransactionInfo newTrans = CreateTransaction(trans.Action, trans.IsConferenceConnect);

			// This ensures the command is sent to the specific server we want,
			//   but does not exclude this transaction from further failover attempts.
			newTrans.ServerId = newServer.ServerId;

            // Fix up the message so it's palatable to the new server
            trans.Action.MediaServerMessage.ChangeField(IMediaServer.Fields.ClientId, newServer.ClientId);
            trans.Action.MediaServerMessage.ChangeField(IMediaServer.Fields.TransactionId, newTrans.ID.ToString());

			// Send (is recursive in some cases)
			return SendCommandToMediaServer(trans.Action.MediaServerMessage, newTrans);
		}

		private MediaServerInfo[] PruneServerList(MediaServerInfo[] servers, ArrayList excludeIds)
		{
			// Make sure there's work to do
			if(servers == null || servers.Length == 0 || excludeIds == null || excludeIds.Count == 0)
				return servers;

			ArrayList prunedList = new ArrayList();
			foreach(MediaServerInfo msInfo in servers)
			{
				if(!excludeIds.Contains(msInfo.ServerId))
					prunedList.Add(msInfo);
			}

			MediaServerInfo[] prunedArray = new MediaServerInfo[prunedList.Count];
			prunedList.CopyTo(prunedArray);
			return prunedArray;
		}

        #endregion

        #region Transaction Methods

        internal TransactionInfo CreateTransaction(MsAction action, bool conference)
        {
            TransactionInfo trans = null;

            if(action.IsAsync)
                trans = new AsyncTransactionInfo(action, Consts.Defaults.TransactionTimeout, conference, action.State);
            else
                trans = new SyncTransactionInfo(action, Consts.Defaults.TransactionTimeout, conference);

            trans.ServerId = action.MmsId;
            return trans;
        }

		private bool CompleteTransaction(uint serverId, uint transId, out MediaServerInfo msInfo, 
			out TransactionInfo trans)
		{
			return CompleteTransaction(serverId, transId, false, out msInfo, out trans);
		}

        private bool CompleteTransaction(uint serverId, uint transId, bool keepInTable, out MediaServerInfo msInfo, 
            out TransactionInfo trans)
        {
            trans = null;

            msInfo = mediaServers[serverId];
            if(msInfo == null)
            {
                log.Write(TraceLevel.Warning, "Received a message from a media server which has since been removed: " + serverId);
                return false;
            }

			if(keepInTable)
				trans = msInfo.GetTransaction(transId);
			else
				trans = msInfo.TakeTransaction(transId);

            if(trans == null)
            {
                log.Write(TraceLevel.Warning, "Received response for defunct transaction: {0}->{1}",
                    serverId.ToString(), transId.ToString());
                return false;
            }

            trans.Complete();

            if(diagOutputTransactionTime)
            {
				string actionName = "Final Disconnect";
				if(trans.Action != null)
					actionName = Namespace.GetName(trans.Action.OriginalAction.InnerMessage.MessageId);
				else if(trans is InitConnectTransactionInfo)
					actionName = "Initial Connect";

                log.Write(TraceLevel.Info, "::STAT:: {0} {1}->{2}: {3}ms", 
                     actionName, serverId, transId, (HPTimer.Now() - trans.Created)/1000000);
            }

            return true;
        }

        #endregion

        #region Media Server Message Callbacks

        /// <summary>Handles individual media server messages.</summary>
        /// <param name="msg">The media server message to handle.</param>
        private void HandleMediaServerMessage(uint serverId, MediaServerMessage msg)
        {
            if(serverId == 0)
            {
                log.Write(TraceLevel.Error, "Received message with invalid serverId:\n" + msg);
                return;
            }

            switch(msg.MessageId)
            {
                case IMediaServer.Messages.Heartbeat:
                    OnMsHeartbeat(serverId, msg);
                    break;
                case IMediaServer.Messages.Connect:
                    OnMsConnect(serverId, msg);
                    break;
                case IMediaServer.Messages.Disconnect:
                    OnMsDisconnect(serverId, msg);
                    break;
                case IMediaServer.Messages.Play:
                    HandleAsyncMediaServerResponse(serverId, msg);
                    break;
                case IMediaServer.Messages.PlayTone:
                    HandleAsyncMediaServerResponse(serverId, msg);
                    break;
                case IMediaServer.Messages.VoiceRecognition:
                    HandleAsyncMediaServerResponse(serverId, msg);
                    break;
                case IMediaServer.Messages.Record:
                    HandleAsyncMediaServerResponse(serverId, msg);
                    break;
                case IMediaServer.Messages.ReceiveDigits:
                    HandleAsyncMediaServerResponse(serverId, msg);
                    break;
                case IMediaServer.Messages.SendDigits:
                    OnMsEventDefaultHandler(serverId, msg);
                    break;
                case IMediaServer.Messages.StopMediaOperation:
                    OnMsEventDefaultHandler(serverId, msg);
                    break;
                case IMediaServer.Messages.ConfereeSetAttribute:
                    OnMsEventDefaultHandler(serverId, msg);
                    break;
                case IMediaServer.Messages.MonitorCallState:
                    HandleAsyncMediaServerResponse(serverId, msg);
                    break;
                case IMediaServer.Messages.GotDigits:
                    OnMsGotDigits(serverId, msg);
                    break;
            }
        }

        /// <summary>Handle heartbeat messages from media servers.</summary>
        /// <remarks>
        /// Heartbeat messages must be sent by media servers at specified
        /// intervals.  When received, a heartbeat message causes a reset
        /// of the heratbeat timeout timer.
        /// </remarks>
        /// <param name="heartbeatMsg">The heartbeat message.</param>
        private void OnMsHeartbeat(uint serverId, MediaServerMessage heartbeatMsg)
        {
            if(shuttingDown)
                return;

            if(serverId == 0)
            {
                log.Write(TraceLevel.Error, "Received heartbeat message with server ID = 0");
                return;
            }

            // Grab the media server info object based on the server ID.
            MediaServerInfo msInfo = mediaServers[serverId];
            if(msInfo == null)
            {
                log.Write(TraceLevel.Error, "Heartbeat received from an unknown media server ({0}). Ignoring.", 
                    serverId);
                return;
            }

            // Get pending heartbeat transaction and cancel it
            TransactionInfo pendingTrans = msInfo.TakeHeartbeatTransaction();
            if(pendingTrans != null)
                pendingTrans.Complete();

            if(msInfo.ConnectedToMediaServer == false)
            {
                log.Write(TraceLevel.Info, 
                    "Received a media server heartbeat from server {0} ({1}), but we are not connected.", 
                    msInfo.MediaServerName, msInfo.Address.ToString());
                return;
            }

            TransactionInfo trans = new HeartbeatTransactionInfo(serverId, this.msHeartbeatTimeoutValue);
            msInfo.AddTransaction(trans);

            SendMsHeartbeatAck(msInfo, heartbeatMsg);
            UpdateMediaServerStatistics(msInfo, heartbeatMsg);
        }

        /// <summary>Handle connect responses from the media server.</summary>
        /// <remarks>
        /// Connect response messages come in two varieties. First, connect
        /// responses are sent as a result of the media server manager 
        /// establishing an initial relationship with the media server. 
        /// The second type of connect response is sent as a result of an
        /// application script establishing connections to media servers.
        /// </remarks>
        /// <param name="connectMsg">The connect response.</param>
        private void OnMsConnect(uint serverId, MediaServerMessage connectMsg)
        {
            uint transactionId = 0;
            IMediaServer.Results resultCode = IMediaServer.Results.Unknown;

            if(IsValidMsResponse(connectMsg, out resultCode, out transactionId) == false)
                return;

            MediaServerInfo msInfo = null;
            TransactionInfo transaction = null;

			if(CompleteTransaction(serverId, transactionId, out msInfo, out transaction) == false)
			{
				log.WriteIf(this.diagInboundResponseMessages, TraceLevel.Info, 
					"Received response out of context:\n" + connectMsg);
				return;
			}

            uint connId = 0;
            connectMsg.GetUInt32(IMediaServer.Fields.ConnectionId, out connId);

            uint confId = 0;
            if(connectMsg.GetUInt32(IMediaServer.Fields.ConferenceId, out confId) == false)

            LogResponseDetails(connectMsg, connId, 0, transaction, resultCode.ToString());

            if(diagInboundConnectMessages)
                log.Write(TraceLevel.Info, "Inbound connect from {0} ({1}):\n{2}",
                    msInfo.MediaServerName, msInfo.Address.ToString(), connectMsg.ToString());

			if(resultCode == IMediaServer.Results.OK)
			{
				// Get resource information
				if(UpdateMediaServerStatistics(msInfo, connectMsg) == false)
				{
					log.Write(TraceLevel.Warning, "No resource information was supplied in connect from {0} ({1})",
						msInfo.MediaServerName, msInfo.Address.ToString());
				}
			}

            // Is this a response to an initial connect?
            if(transaction.IsInitConnect)        
            {
                this.OnInitConnectAck(msInfo, connectMsg);
                return;
            }

            // Async connection handling
            AsyncTransactionInfo asyncTransaction = transaction as AsyncTransactionInfo;
            if(asyncTransaction != null)
            {
                if(SendMsFinalAsyncResponseToApp != null)
                {
                    SendMsFinalAsyncResponseToApp(asyncTransaction.Action.OriginalAction as AsyncAction, 
                        resultCode, connectMsg);
                }
                return;
            }

            SyncTransactionInfo connectTransaction = transaction as SyncTransactionInfo;

            if(connectTransaction == null)
            {
                Debug.Fail("Connect response is not in a SyncTransaction");
                return;
            }

            // If we have an OK result
            if(resultCode == IMediaServer.Results.OK)
            {
                UpdateMediaServerStatistics(msInfo, connectMsg);

                if(connId == 0)
                {
                    log.Write(TraceLevel.Error, "Media server connect response sent an OK result without a connectionId. Ignoring.");
                    return;
                }
                
                if (connectTransaction.IsConferenceConnect && confId == 0)
                {
                    log.Write(TraceLevel.Error, "Received a conference connect response without a conference ID.");

                    if(SendMsFailureResponseToApp != null)
                    {
                        SendMsFailureResponseToApp(transaction.Action.OriginalAction, resultCode.ToString(), 
                            IMediaServer.Errors.NoLocalPort);
                    }
                    return;
                }
            }
            else if(resultCode == IMediaServer.Results.ServerBusy ||
                resultCode == IMediaServer.Results.ResourceUnavailable)
            {
    			connectTransaction.FailedServers.Add(serverId);
                if(Failover(connectTransaction))
                {
                    string cmd = Namespace.GetName(connectTransaction.Action.OriginalAction.InnerMessage.MessageId);       
                    log.Write(TraceLevel.Warning, "{0} command failed on {1} ({2}). Attempting to fail over...",
                        cmd, msInfo.MediaServerName, msInfo.Address.ToString());
                }
                else
                {
                    string cmd = Namespace.GetName(connectTransaction.Action.OriginalAction.InnerMessage.MessageId);
                    log.Write(TraceLevel.Warning, 
                        "{0} command failed because {1} ({2}) does not have any available sessions and this transaction cannot fail over.",
                        cmd, msInfo.MediaServerName, msInfo.Address.ToString());
                }
				return;
            }
            else
            {
                // Received something else.
                log.Write(TraceLevel.Error, "Connect command failed {0} ({1}): cid={2} r={3}", 
                    msInfo.MediaServerName, msInfo.Address.ToString(), connId, resultCode);
            }
            
            // Forward the response to the application associated with this transaction.
            if(ForwardMsMessageToApplication != null)
                ForwardMsMessageToApplication(transaction, connectMsg, resultCode);
        }

        private void OnInitConnectAck(MediaServerInfo msInfo, MediaServerMessage connectResponse)
        {
            IMediaServer.Results resultCode = IMediaServer.Results.Unknown;
            string clientId;

            if(msInfo.ConnectedToMediaServer == true)
            {
                log.Write(TraceLevel.Warning, "Received an connection result from {0} ({1}) but we are already connected.",
                    msInfo.MediaServerName, msInfo.Address.ToString());
                return;
            }

            string resultCodeStr;
            if(connectResponse.GetString(IMediaServer.Fields.ResultCode, out resultCodeStr) == false)
            {
                log.Write(TraceLevel.Error, 
                    "Received a media server connect message without a resultCode from {0} ({1}). Ignoring.",
                    msInfo.MediaServerName, msInfo.Address.ToString());
                msInfo.SetServerDownAlarm();

                // Mark the media server in the DB as stopped
                configUtility.UpdateStatus(IConfig.ComponentType.MediaServer, msInfo.MediaServerName, IConfig.Status.Enabled_Stopped);

                ConnectToMediaServer(msInfo);
                return;
            }

            try
            {
                resultCode = (IMediaServer.Results) Convert.ToInt32(resultCodeStr);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Received invalid result code from media server: " + resultCodeStr); 
            }

            if(resultCode != IMediaServer.Results.OK)
            {
                log.Write(TraceLevel.Error, "Connection to media server {0} ({1}) was actively denied. Result code: {2}",
                    msInfo.MediaServerName, msInfo.Address.ToString(), resultCode);

                msInfo.SetServerDownAlarm();

                // Mark the media server in the DB as stopped
                configUtility.UpdateStatus(IConfig.ComponentType.MediaServer, msInfo.MediaServerName, IConfig.Status.Enabled_Stopped);

				ConnectToMediaServer(msInfo);
                return;
            }
            
            msInfo.ConnectedToMediaServer = true;

			msInfo.ClearServerDownAlarm();

            // Mark the media server in the DB as running
            configUtility.UpdateStatus(IConfig.ComponentType.MediaServer, msInfo.MediaServerName, IConfig.Status.Enabled_Running);

            log.Write(TraceLevel.Info, "Connected to {0} ({1}) via {2}.",
                msInfo.MediaServerName, msInfo.Address.ToString(), msInfo.ConnectionTypeStr);

            if(connectResponse.GetString(IMediaServer.Fields.ClientId, out clientId) == false)
            {
                log.Write(TraceLevel.Warning, "No clientId was returned by media server {0} ({1}).",
                    msInfo.MediaServerName, msInfo.Address.ToString());
            }
            else
            {
                msInfo.ClientId = clientId;
            }
        }
        
        private void OnMsDisconnect(uint serverId, MediaServerMessage disconnectResponse)
        {
            uint transactionId = 0;
            IMediaServer.Results resultCode = IMediaServer.Results.Unknown;

            if(IsValidMsResponse(disconnectResponse, out resultCode, out transactionId) == false)
                return;

            uint connId = 0;
            disconnectResponse.GetUInt32(IMediaServer.Fields.ConnectionId, out connId);

            uint confId = 0;
            if(disconnectResponse.GetUInt32(IMediaServer.Fields.ConferenceId, out confId) == false)

            // If the disconnect response does not contain a connection ID or
            // a conference ID then it is a response indicating that the link
            // with the media server has been terminated, not that an individual
            // connection or conference within the media server has been closed.
            if((connId == 0) && (confId == 0))
            {
                // Just drop it
                return;
            }

            MediaServerInfo msInfo = null;
            TransactionInfo transaction = null;

            if(!CompleteTransaction(serverId, transactionId, out msInfo, out transaction))
                return;

            LogResponseDetails(disconnectResponse, connId, 0, transaction, resultCode.ToString());

            if(resultCode == IMediaServer.Results.OK)
            {
                // Get resource information
                if(UpdateMediaServerStatistics(msInfo, disconnectResponse) == false)
                {
                    log.Write(TraceLevel.Warning, "No resource information was supplied in disconnect from {0} ({1})",
                        msInfo.MediaServerName, msInfo.Address.ToString());
                }
            }
            else
            {
                // Received something other than Results.OK
                log.Write(TraceLevel.Warning, "Received Non-OK result: {0}", resultCode);
            }

            if(ForwardMsMessageToApplication != null)
                ForwardMsMessageToApplication(transaction, disconnectResponse, resultCode);
        }

        private void OnFinalDisconnect(TransactionInfo transaction, IMediaServer.Results resultCode)
        {
            if(transaction == null)
                return;

            MediaServerInfo msInfo = mediaServers[transaction.ServerId] as MediaServerInfo;
            if(msInfo == null)
                return;

            if(resultCode != IMediaServer.Results.OK)
            {
                log.Write(TraceLevel.Warning, 
                    "Media server disconnect returned unknown result code {0} ({1}): {2}", 
                    msInfo.MediaServerName, msInfo.Address.ToString(), resultCode);
                return;
            }

            KillMsConnection(msInfo);

            log.Write(TraceLevel.Info, "Disconnected from {0} ({1}) gracefully.", 
                msInfo.MediaServerName, msInfo.Address.ToString());
        }

        /// <summary>
        /// This is a purely unsolicited media event. So it's handled differently from
        /// the other messages. The digits will be sent to TM and from there, TM will
        /// decide whether they should be added to the digit buffer, via SendDigits,
        /// or dropped.
        /// </summary>
        private void OnMsGotDigits(uint serverId, MediaServerMessage digitsMsg)
        {
            MediaServerInfo msInfo = mediaServers[serverId];
            if(msInfo == null)
            {
                log.Write(TraceLevel.Warning, "Received a message from a media server which has since been removed: " + serverId);
                return;
            }

            uint connId;
            if(!digitsMsg.GetUInt32(IMediaServer.Fields.ConnectionId, out connId))
            {
                log.Write(TraceLevel.Warning, "Received a digit event from '{0}' without a connection ID", msInfo.MediaServerName);
                return;
            }

            string routingGuid;
            if(!digitsMsg.GetString(ICommands.Fields.ROUTING_GUID, out routingGuid))
            {
                log.Write(TraceLevel.Warning, "Received a digit event from '{0}' without a routing GUID", msInfo.MediaServerName);
                return;
            }

            string digits;
            if(!digitsMsg.GetString(IMediaServer.Fields.Digits, out digits))
            {
                log.Write(TraceLevel.Warning, "Received a digit event from '{0}' without any digits", msInfo.MediaServerName);
                return;
            }

            if(SendDigitsEvent != null)
                SendDigitsEvent(connId, routingGuid, digits);
        }

        private void OnMsEventDefaultHandler(uint serverId, MediaServerMessage im)
        {
            uint transId = 0;
            IMediaServer.Results resultCode = IMediaServer.Results.Unknown;

            if(IsValidMsResponse(im, out resultCode, out transId) == false)
                return;

            MediaServerInfo msInfo = null;
            TransactionInfo transaction = null;
			if(!CompleteTransaction(serverId, transId, out msInfo, out transaction))
			{
				log.WriteIf(this.diagInboundResponseMessages, TraceLevel.Info, 
					"Received response out of context:\n" + im);
				return;
			}

            uint connId = 0;
            if(im.GetUInt32(IMediaServer.Fields.ConnectionId, out connId) == false)
            {
                log.Write(TraceLevel.Error, "Received a generic response without a connectionId.");
                return;
            }

            LogResponseDetails(im, connId, 0, transaction, resultCode.ToString());          

            if(resultCode != IMediaServer.Results.OK)
            {
                // Received something other than Results.OK
                log.Write(TraceLevel.Info, "Received Non-OK result from {0} ({1}): {2}",
                    msInfo.MediaServerName, msInfo.Address.ToString(), resultCode);
            }

            if(ForwardMsMessageToApplication != null)
                ForwardMsMessageToApplication(transaction, im, resultCode);
        }

        /// <summary>Handles a standard media server asynchronous response.</summary>
        /// <param name="im">The asynchronous response message.</param>
        /// <param name="finalResponseMessageId">
        /// The message ID to use if this response represents a "final" response
        /// from the media server for the transaction.
        /// </param>
        private void HandleAsyncMediaServerResponse(uint serverId, MediaServerMessage im)
        {
            uint transId = 0;
            IMediaServer.Results resultCode = IMediaServer.Results.Unknown;

            if(IsValidMsResponse(im, out resultCode, out transId) == false)
                return;

            MediaServerInfo msInfo = null;
            TransactionInfo transaction = null;

            if(resultCode == IMediaServer.Results.TransExecuting)
            {
                // This message is an async provisional response, so we only
                // need to update the transaction information, but we must
                // leave the transaction in our table.
				if(!CompleteTransaction(serverId, transId, true, out msInfo, out transaction))
				{
					log.Write(TraceLevel.Warning, "Received response out of context:\n" + im);
					return;
				}

                if(diagInboundResponseMessages)
                    log.Write(TraceLevel.Info, "Received provisional response:\n" + im.ToString());

				// Turn the transaction into an async transaction object.
				AsyncTransactionInfo asyncTransaction = transaction as AsyncTransactionInfo;

				if(asyncTransaction.ProvisionalReceived == true)
				{
					log.Write(TraceLevel.Warning, 
						"Received duplicate provisional responses for media server transaction");
				}
				else
				{
					asyncTransaction.ProvisionalReceived = true;
				}
            
				// Forward the provisional response as normal.
				if(ForwardMsMessageToApplication != null)
					ForwardMsMessageToApplication(transaction, im, resultCode);
            }
            else
            {
                // This is a final response, so lets pop it from the transaction
                // table and carry on.
				if(!CompleteTransaction(serverId, transId, false, out msInfo, out transaction))
				{
					log.Write(TraceLevel.Warning, "Received response out of context:\n" + im);
					return;
				}

                if(diagInboundResponseMessages)
                    log.Write(TraceLevel.Info, "Received final response:\n" + im.ToString());

				// Turn the transaction into an async transaction object.
				AsyncTransactionInfo asyncTransaction = transaction as AsyncTransactionInfo;

				if(asyncTransaction.State != null)
					im.AddField(IMediaControl.Fields.STATE, asyncTransaction.State);

				if(asyncTransaction.ProvisionalReceived == true)
				{
					// This is a final response, we need to generate an unsolicited event
					// for the application to handle.
					if(SendMsFinalAsyncResponseToApp != null)
					{
						SendMsFinalAsyncResponseToApp(transaction.Action.OriginalAction as AsyncAction, 
                            resultCode, im);
					}
				}
				else if(SendMsFailureResponseToApp != null)
				{
					SendMsFailureResponseToApp(asyncTransaction.Action.OriginalAction, resultCode.ToString());
				}
            }
        }

        #endregion

        #region Timer Callbacks

        private long OnTransactionTimeout(TimerHandle timerHandle, object state)
        {
            if(this.shuttingDown)
                return 0;

            TransactionInfo trans = state as TransactionInfo;
            if(state == null)
            {
                Debug.Fail("OnTransactionTimeout: state is not a transaction");
                return 0;
            }

            // Remove transaction
            MediaServerInfo msInfo = mediaServers[trans.ServerId];
            if(msInfo != null)
                msInfo.RemoveTransaction(trans.ID);

            if(trans is HeartbeatTransactionInfo)
                OnHeartbeatTimeout(msInfo);
            else if(trans is InitConnectTransactionInfo)
                OnInitConnectTimeout(msInfo);
            else
                OnActionTimeout(trans);

            return 0;
        }

        private void OnActionTimeout(TransactionInfo trans)
        {
			// Final disconect messages will come through here if we tear down the connection
			//  before the response comes in, so don't worry about them. (action will be null)
			if(trans.Action != null)
			{
				log.Write(TraceLevel.Warning, "Media control action timed out:\n" + trans.Action);
				MediaControlProvider.HandleFatalError(trans.Action, IMediaControl.ResultCodes.OperationTimeout);
			}
        }

        private void OnInitConnectTimeout(MediaServerInfo msInfo)
        {
            msInfo.SetServerDownAlarm();

            // Mark the media server in the DB as stopped
            configUtility.UpdateStatus(IConfig.ComponentType.MediaServer, msInfo.MediaServerName, IConfig.Status.Enabled_Stopped);

            ConnectToMediaServer(msInfo);
        }

        private void OnHeartbeatTimeout(MediaServerInfo msInfo)
        {
            if(msInfo == null)
                return;

            log.Write(TraceLevel.Warning, "Lost connection to media server {0} ({1})", 
                msInfo.MediaServerName, msInfo.Address.ToString());

            if(msInfo.DisconnectRequested)
            {
                // This could happen if the media server failed to respond
                //   to a disconnect command
                KillMsConnection(msInfo);
            }
            else
            {
                msInfo.ConnectedToMediaServer = false;
                ConnectToMediaServer(msInfo);
            }
        }

        #endregion

        #region Helpers

        internal void KillMsConnection(MediaServerInfo msInfo)
        {
            if(msInfo == null)
                return;

            if(!shuttingDown)
                mediaServers.Remove(msInfo.ServerId);

            msInfo.Close();
            msInfo.Dispose();
        }

        private bool IsValidMsResponse(MediaServerMessage response, out IMediaServer.Results resultCode, 
            out uint transId)
        {
            resultCode = IMediaServer.Results.Unknown;
            transId = 0;

            if(response == null)
                return false;

            bool valid = true;

            // Pull the result code from the message. Every response from the media server
            // must have a result code.
            string resultCodeStr;
            if(response.GetString(IMediaServer.Fields.ResultCode, out resultCodeStr) == false)
            {
                log.Write(TraceLevel.Warning, 
                    "Received a result from the media server without a resultCode. Assuming its an OK.");

                resultCode = IMediaServer.Results.OK;
            }

            try
            {
                resultCode = (IMediaServer.Results) Convert.ToInt32(resultCodeStr);
            }
            catch 
            {
                log.Write(TraceLevel.Warning, "Received invalid result code from media server: " + resultCodeStr);
            }

            // Pull the transaction ID from the message. Every response from the media server
            // must contain a transaction ID that maps to a request transaction initiated
            // from the media server provider.
            if (response.GetUInt32(IMediaServer.Fields.TransactionId, out transId) == false)
            {
                log.Write(TraceLevel.Error, "Received a result with no transactionId. Ignoring result.");
                valid = false;
            }

            return valid;
        }

        /// <summary>
        /// Acknowledges a heartbeat message received from the media server.
        /// </summary>
        /// <param name="heartbeatMsg">The heartbeat message.</param>
        private void SendMsHeartbeatAck(MediaServerInfo msInfo, MediaServerMessage heartbeatMsg)
        {
            string heartbeatId = null;
            if(heartbeatMsg.GetString(IMediaServer.Fields.HeartbeatId, out heartbeatId) == false)
            {
                log.Write(TraceLevel.Warning,
                    "Heartbeat from {0} ({1}) contained no heartbeat ID. Can not acknowledge.",
                    msInfo.MediaServerName, msInfo.Address.ToString());
                return;
            }

            this.heartbeatAckMsg.ChangeField(IMediaServer.Fields.HeartbeatId, heartbeatId);
			this.heartbeatAckMsg.ChangeField(IMediaServer.Fields.ClientId, msInfo.ClientId);

            try 
            { 
                msInfo.SendCommand(heartbeatAckMsg, false); 

                log.WriteIf(this.diagOutboundCommandMessages, TraceLevel.Info, "Sent heartbeat ACK to {0} ({1}):\n{2}",
                    msInfo.MediaServerName, msInfo.Address.ToString(), heartbeatAckMsg);
            }
            catch(Exception e)
            {
                if(!msInfo.DisconnectRequested)
                    PrintSendException(e, heartbeatAckMsg);
            }
        }

        /// <summary>
        /// Updates the internal statistics maintained by the provider
        /// that indicate how many resources a media server has available.
        /// </summary>
        /// <param name="heartbeatMsg">The heartbeat message containing the statistics.</param>
        private bool UpdateMediaServerStatistics(MediaServerInfo msInfo, MediaServerMessage msg)
        {
            if(msInfo == null || msg == null)
                return false;

            string[] resources;

            msg.GetFieldsByName(IMediaServer.Fields.MediaResources, out resources);

            if(resources == null)
                return false;

			// Take note of what we have now
			bool ipOut = msInfo.Resources.ipResAvail == 0;
			bool lbrOut = msInfo.Resources.lbrResAvail == 0;

			// Load up the new data
            msInfo.Resources.LoadResources(resources);

			// Log it
            log.WriteIf(diagResourceInfo, TraceLevel.Info,
                "Resources available for {0} ({1}): ip={2} v={3} c={4}", msInfo.MediaServerName, 
                msInfo.Address.ToString(), msInfo.Resources.ipResAvail, 
                msInfo.Resources.voxResAvail, msInfo.Resources.confResAvail);

            return true;
        }

        private void WriteInvalidResponseErrorLogMessage(string resultCode, string transactionId)
        {
            System.Text.StringBuilder logMessage = new System.Text.StringBuilder();
                    
            logMessage.Append("Received an invalid media server response. Data follows:\n");

            logMessage.Append("    resultCode: ");
            if(resultCode == null)
            {
                logMessage.Append("<null>");
            }
            else
            {
                logMessage.Append(resultCode);
            }

            logMessage.Append("    transactionId: ");
            if(transactionId == null)
            {
                logMessage.Append("<null>");
            }
            else
            {
                logMessage.Append(transactionId);
            }

            log.Write(TraceLevel.Error, logMessage.ToString());
        }

        /// <summary>Uses a simple algorithm to find the best media server
        /// for a new connection.</summary>
        /// <remarks>"Best" is determined by finding the media server with
        /// the most IP resources available which supports the
        /// specified codecs.</remarks>
        /// <returns>The server ID of the media server to send the
        /// connection to.</returns>
        private bool FindBestMediaServer(IMediaControl.Codecs txCodec, IMediaControl.Codecs rxCodec, 
            MediaServerInfo[] servers, out MediaServerInfo bestServer)
        {
            bestServer = null;

            if(servers == null || servers.Length == 0)
                return false;

            log.WriteIf(diagServerSelection, TraceLevel.Info, "Locating best media server for codecs: Rx={0}, Tx={1}",
                rxCodec.ToString(), txCodec.ToString());

            bool needLbr = false;
            if (txCodec == IMediaControl.Codecs.G723 ||
                txCodec == IMediaControl.Codecs.G729 ||
                rxCodec == IMediaControl.Codecs.G723 ||
                rxCodec == IMediaControl.Codecs.G729)
            {
                needLbr = true;
            }

            // Find best server
            int mostSessionsAvailable = 0;
            foreach(MediaServerInfo msInfo in servers)
            {
                if(msInfo == null)
                    continue;

                if(this.diagServerSelection)
                {
                    if(msInfo.ConnectedToMediaServer == false)
                    {
                        log.Write(TraceLevel.Info, "Inspecting MMS {0} ({1}): Not connected",
                            msInfo.MediaServerName, msInfo.Address.ToString());
                        continue;
                    }
                    else
                    {
                        log.Write(TraceLevel.Info, "Inspecting MMS {0} ({1}): ip={2}, lbr={3}",
                            msInfo.MediaServerName, msInfo.Address.ToString(),
                            msInfo.Resources.ipResAvail, msInfo.Resources.lbrResAvail);
                    }
                }

                if(needLbr)
                {
                    if(msInfo.Resources.lbrResAvail > mostSessionsAvailable)
                    {
                        bestServer = msInfo;
                        mostSessionsAvailable = msInfo.Resources.lbrResAvail;
                    }
                }
                else if(msInfo.Resources.ipResAvail > mostSessionsAvailable)
                {
                    bestServer = msInfo;
                    mostSessionsAvailable = msInfo.Resources.ipResAvail;
                }
            }

            if(bestServer != null)
            {
                log.WriteIf(diagServerSelection, TraceLevel.Info, "Found best media server: {0} ({1})",
                    bestServer.MediaServerName, bestServer.Address.ToString());
                return true;
            }

            log.WriteIf(diagServerSelection, TraceLevel.Info, "No media server could be found for the specified codec(s)");
            return false;
        }

        internal MediaServerInfo GetMediaServer(uint mmsId)
        {
            return mediaServers[mmsId];
        }

        internal MediaServerInfo[] GetMediaServers(string appName, string partName, bool failoverGroup)
        {
            MrgCache.CacheData cData = this.mrgCache.GetCacheData(appName, partName, diagServerSelection);
            if(cData == null)
            {
                log.Write(TraceLevel.Warning, "No media resource group configured for {0}:{1}",
                    appName, partName);
                return null;
            }

            IPAddress[] mmsAddrs = null;
            if(failoverGroup)
                mmsAddrs = cData.FailoverMmsAddrs;
            else
                mmsAddrs = cData.MmsAddrs;

            if(mmsAddrs == null)
                return null;

            MediaServerInfo[] servers = new MediaServerInfo[mmsAddrs.Length];

            for(int i=0; i<mmsAddrs.Length; i++)
            {
                servers[i] = mediaServers.GetByAddr(mmsAddrs[i]);
            }

            return servers;
        }

        private void LogResponseDetails(MediaServerMessage msg, uint connId, uint confId, 
            TransactionInfo transaction, string resultCode)
        {
            log.WriteIf(diagInboundResponseMessages, TraceLevel.Info,
                "res={0} a={1} r={2} tid={3} sid={4} connId={5} confId={6}",
                msg.MessageId, 
                transaction.Action == null ? "(none)" : Namespace.GetName(transaction.Action.OriginalAction.Name), 
                resultCode, transaction.ID, transaction.ServerId,
                connId == 0 ? "(none)" : connId.ToString(), 
                confId == 0 ? "(none)" : confId.ToString());
        }

        internal void PrintSendException(Exception e, MediaServerMessage msg)
        {
            if(e != null)
            {
                log.Write(TraceLevel.Error, "Failed to send message to media server [CPU={0}%, Mem={1}KB]: {2}",
                    PerfMonCounter.GetValue(PerfCounterType.CPU_Load), 
                    PerfMonCounter.GetMemoryUsage(), e.Message);
                log.Write(TraceLevel.Error, "Full exception:\n" + e);
            }

            if(msg != null)
                log.Write(TraceLevel.Error, "Failed message:\n" + msg);
        }

        internal void ClearMrgCache()
        {
            this.mrgCache.Clear();
        }

        internal void PrintServerTable()
        {
            log.ForceWrite(TraceLevel.Info, "Server Table:\n" + mediaServers);
        }

        internal void PrintDiags()
        {
            log.ForceWrite(TraceLevel.Info, "Engineering diagnostics:\n" + mediaServers.GetDiagnosticMessage());
        }

        #endregion
    }
}
