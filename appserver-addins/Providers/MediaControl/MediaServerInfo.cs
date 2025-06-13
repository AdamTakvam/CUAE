using System;
using System.Net;
using System.Messaging;
using System.Threading;
using System.Diagnostics;
using System.Collections;

using Metreos.Stats;
using Metreos.Interfaces;
using Metreos.LoggingFramework;

namespace Metreos.MediaControl
{
    public delegate void MsInfoDelegate(MediaServerInfo msInfo);
    public delegate void HandleMmsMessageDelegate(uint serverId, MediaServerMessage msg);

    public abstract class MediaServerInfo : IDisposable
    {
		#region Boring inner classes

		// Contains the only valid values for the ConnectionType config entry
		internal abstract class ConnectionType
		{
			public const string MSMQ        = "MSMQ";
			public const string IPC         = "IPC";
		}

		// I told you they were boring, but you just had to click anyway, didn't you?  ;)

		internal sealed class AlarmGuidData
		{
			private string serverDown;
			public string ServerDown 
			{
				get { return serverDown; }
				set { serverDown = value; }
			}
		}
		#endregion

		#region Properties

        /// <summary>Delegate for threadpool callback</summary>
        public static HandleMmsMessageDelegate HandleMediaServerMessage 
        {
            set { handleMediaServerMessage = value; }
        }
        protected static HandleMmsMessageDelegate handleMediaServerMessage;

        /// <summary>Connect timer callback method</summary>
        public static MsInfoDelegate ConnectToMediaServer
        {
            set { connectToMediaServer = value; }
        }
        protected static MsInfoDelegate connectToMediaServer;

		protected LogWriter log;
        protected StatsClient statsClient;

        public MediaServerResources Resources { get { return resources; } }
        protected MediaServerResources resources;

		internal AlarmGuidData AlarmGuids { get { return alarmGuids; } }
		private readonly AlarmGuidData alarmGuids;

        public virtual string ConnectionTypeStr
        {
			get { return this is MediaServerInfoIPC ? ConnectionType.IPC : ConnectionType.MSMQ; }
        }

        /// <summary>
        /// True if we have established a connection to the media server.
        /// </summary>
        public virtual bool ConnectedToMediaServer 
        { 
            get { return connectedToMediaServer; } 
            set 
            { 
                connectedToMediaServer = value; 
                if(value == true)
                {
                    mmsConnectAttempts = 0;
                    lastConnectAttemptTransactionId = 0;
                }
            }
        }
        protected volatile bool connectedToMediaServer = false;

        public object ConnectLock { get { return connectLock; } }
        protected readonly object connectLock = new object();

        public bool ConnectPending { get { return connectPending; } }
        protected bool connectPending = false;

        /// <summary>Flag set when final disconnect is sent to MMS</summary>
        public bool DisconnectRequested 
        { 
            get { return disconnectRequested; } 
            set { disconnectRequested = value; }
        }
        protected volatile bool disconnectRequested = false;

        /// <summary>
        /// The number of attempts the provider has made to connect to the
        /// media server.
        /// </summary>
        public uint MmsConnectAttempts
        {
            get { return mmsConnectAttempts; }
            set { mmsConnectAttempts = value; }
        }
        protected uint mmsConnectAttempts;

        /// <summary>The transaction ID of the last connect attempt</summary>
        public uint LastConnectAttemptTransactionId
        {
            get { return lastConnectAttemptTransactionId; }
            set { lastConnectAttemptTransactionId = value; }
        }
        protected uint lastConnectAttemptTransactionId;

        /// <summary>
        /// The ID returned to us by the media server that is used
        /// to identify our unique connection to it.
        /// </summary>
        public string ClientId
        {
            get { return clientId; }
            set { clientId = value; }
        }
        protected string clientId;

        /// <summary>
        /// The server ID that has been assigned to this media server.
        /// Valid server IDs are 1-255.  If operating in single media
        /// server mode, the ID of 0 should be used.
        /// </summary>
        public uint ServerId { get { return serverId; } }
        protected readonly uint serverId;

        /// <summary>The IP address of the media server.</summary>
        public IPAddress Address { get { return address; } }
        protected IPAddress address;

        /// <summary>The friendly name of the media server.</summary>
        public string MediaServerName { get { return mediaServerName; } }
        protected string mediaServerName;

        /// <summary>
        /// A table of transactions that are currently waiting for responses from the media server.
        /// </summary>
        protected readonly TransactionCollection transactions;
        protected uint heartbeatTransId = 0;

		#endregion

		#region Constructor/Close

        protected MediaServerInfo(string name, uint id, IPAddress addr, LogWriter log, StatsClient statsClient)
        {
            this.mediaServerName = name;
            this.serverId = id;
            this.address = addr;
			this.log = log;
            this.statsClient = statsClient;

			this.alarmGuids = new AlarmGuidData();
            this.resources = new MediaServerResources();
            this.transactions = new TransactionCollection();
        }

        /// <summary>Handles failure case for all pending transactions</summary>
        /// <remarks>Subclasses must call the base class if they overload this method</remarks>
        public virtual void Close()
        {
			connectedToMediaServer = false;
			clientId = null;
			mmsConnectAttempts = 0;
			lastConnectAttemptTransactionId = 0;
			resources.Zero();
            connectPending = false;

            lock(transactions.SyncRoot)
            {
                foreach(DictionaryEntry de in transactions)
                {
                    TransactionInfo trans = de.Value as TransactionInfo;
					if(trans != null)
					{
						trans.Complete();
						MediaControlProvider.HandleFatalError(trans.Action, IMediaControl.ResultCodes.NotConnected);
					}
                }

                transactions.Clear();
            }

			SetServerDownAlarm();
        }
		#endregion

		#region Alarm helpers

		public void SetServerDownAlarm()
		{
			if(this.alarmGuids.ServerDown == null && !disconnectRequested)
			{
				// Set an alarm
                this.alarmGuids.ServerDown = statsClient.TriggerAlarm(IConfig.Severity.Red, IStats.AlarmCodes.General.MediaServerUnavailable,
                    IStats.AlarmCodes.General.Descriptions.MediaServerUnavailable, mediaServerName, address.ToString());
			}
		}

		public void ClearServerDownAlarm()
		{
            if(this.alarmGuids.ServerDown != null)
            {
                statsClient.ClearAlarm(this.alarmGuids.ServerDown);
                this.alarmGuids.ServerDown = null;
            }
		}
		#endregion

        #region Transaction methods

        internal void AddTransaction(TransactionInfo trans)
        {
            if(trans is HeartbeatTransactionInfo)
                this.heartbeatTransId = trans.ID;
            else if(trans is InitConnectTransactionInfo)
                this.connectPending = true;

            transactions.Add(trans);
        }

        internal TransactionInfo GetTransaction(uint transId)
        {
            return transactions[transId];
        }

        internal TransactionInfo TakeTransaction(uint transId)
        {
            TransactionInfo trans =  transactions.Take(transId);
            if(trans is InitConnectTransactionInfo)
                this.connectPending = false;
            return trans;
        }

        internal TransactionInfo TakeHeartbeatTransaction()
        {
            return transactions.Take(this.heartbeatTransId);
        }

        internal void RemoveTransaction(uint transId)
        {
            TakeTransaction(transId);
        }

        internal string GetDiagnosticMessage()
        {
            // Format:
            // "sId    tId=       ttId    tSid  tType   msMsg  Created    Due   \r\n"
            // "--- ---------- ---------- ---- ------- ------- -------- --------\r\n"

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            lock(transactions.SyncRoot)
            {
                foreach(DictionaryEntry de in transactions)
                {
                    sb.Append(serverId.ToString().PadRight(4));

                    uint tId = Convert.ToUInt32(de.Key);
                    TransactionInfo trans = de.Value as TransactionInfo;
                    
                    sb.Append(tId.ToString().PadRight(11));

                    if(trans == null)
                    {
                        sb.Append("(null)\r\n");
                        continue;
                    }

                    sb.Append(trans.ID.ToString().PadRight(11));
                    sb.Append(trans.ServerId.ToString().PadRight(5));
                    sb.Append(trans.GetTypeStr(7).PadRight(8));
                    
                    if(trans.Action != null && trans.Action.MediaServerMessage != null)
                    {
                        string msg = trans.Action.MediaServerMessage.MessageId;
                        if(msg.Length > 7)
                            msg = msg.Substring(0, 7);
                        sb.Append(msg.PadRight(8));
                    }
                    else
                        sb.Append("(none)  ");

                    sb.Append(trans.CreatedStr.PadRight(9));
                    sb.Append(trans.DueTimeStr);
                    sb.Append("\r\n");
                }
            }
            return sb.ToString();
        }

        #endregion

        #region Abstract methods

        public abstract void SendCommand(MediaServerMessage commandMsg, bool isInitConnect);

        public abstract void Dispose();

        #endregion
    }
}
