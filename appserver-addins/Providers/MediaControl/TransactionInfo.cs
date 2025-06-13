using System;
using System.Collections;

using Metreos.Utilities;
using Metreos.ProviderFramework;

namespace Metreos.MediaControl
{
    /// <summary>Base class for media server transcations.</summary>
    internal abstract class TransactionInfo
    {
        public abstract class Consts
        {
            public const int TimeoutMSecs = 5000;
            public const long NanosPerSec = 1000000000;
        }

        /// <summary>The action that caused this media server transaction.</summary>
        public MsAction Action { get { return action; } }
        private readonly MsAction action;

        /// <summary>
        /// Transaction ID for this transaction. Its just a sequence number.
        /// </summary>
        public uint ID { get { return id; } }
        private readonly uint id;

        /// <summary>The ID of the server processing this transaction.</summary>
        public uint ServerId 
        { 
            get { return serverId; } 
            set { serverId = value; }
        }
        protected uint serverId;

        public bool IsInitConnect { get { return this is InitConnectTransactionInfo; } }

        private readonly TimerHandle timer;
        public string DueTimeStr { get { return String.Format("{0}s", timer.DueTime / Consts.NanosPerSec); } }

        public long Created { get { return created; } }
        private readonly long created;
        public string CreatedStr { get { return String.Format("{0}s", created / Consts.NanosPerSec); } }

        public TransactionInfo(MsAction action, long timeout)
        {
            this.created = HPTimer.Now();
            this.id = TransactionIdFactory.GenerateId();

            this.action = action;
            this.timer = MediaServerManager.TimerManager.Add(timeout, this);
        }

        public void Complete()
        {
            this.timer.Cancel();
        }

        public string GetTypeStr(int maxLength)
        {
            string tType = "Unknown";
            if(action != null)
                tType = Metreos.Utilities.Namespace.GetName(
                    action.OriginalAction.InnerMessage.MessageId);
            else if(IsInitConnect)
                tType = "IC";
            else if(this is HeartbeatTransactionInfo)
                tType = "HB";

            // Restrict length
            if(tType.Length > maxLength)
                tType = tType.Substring(0, maxLength);

            return tType;
        }
    }

    /// <summary>Synchronous media server transaction</summary>
    internal class SyncTransactionInfo : TransactionInfo
    {
        /// <summary>
        /// Indicates that this transaction is either creating or modifying a conference
        /// </summary>
        public bool IsConferenceConnect { get { return conference; } }
        private readonly bool conference = false;

		/// <summary>
		/// Collection of IDs (uint) of servers which failed to service this transaction
		/// </summary>
		public ArrayList FailedServers { get { return failedServers; } }
		private readonly ArrayList failedServers;

        /// <summary>A transaction which expects only a single response</summary>
        /// <param name="action">The action which caused this transaction</param>
        /// <param name="timeout">The number of milliseconds to wait for an MMS response</param>
        /// <param name="conference">
        /// Indicates that this transaction is either creating or modifying a conference
        /// </param>
        public SyncTransactionInfo(MsAction action, long timeout, bool conference) 
            : base(action, timeout) 
        {
            this.conference = conference;

			this.failedServers = new ArrayList();
        }
    }

    /// <summary>
    /// Asynchronous media server transaction. Commands that use transactions
    /// of this type expect the media server to issue both a provisional and
    /// final response. The final response comes from the media server
    /// as an unsolicited event.
    /// </summary>
    internal class AsyncTransactionInfo : TransactionInfo
    {
        /// <summary>
        /// Indicates that this transaction is either creating or modifying a conference
        /// </summary>
        public bool IsConferenceConnect { get { return conference; } }
        private readonly bool conference = false;

        /// <summary>User-defined async state information</summary>
        public string State { get { return state; } }
        private readonly string state = null;

        /// <summary>
        /// Indicates whether a provisional response has been received
        /// for this command.
        /// </summary>
        public bool ProvisionalReceived
        {
            get { return provisionalReceived; }
            set { provisionalReceived = value; }
        }
        private bool provisionalReceived = false;

        /// <summary>A transaction which will receive two responses</summary>
        /// <param name="action">The action which caused this transaction</param>
        /// <param name="timeout">The number of milliseconds to wait for an MMS response</param>
        /// /// <param name="conference">
        /// Indicates that this transaction is either creating or modifying a conference
        /// </param>
        public AsyncTransactionInfo(MsAction action, long timeout, bool conference, string state) 
            : base(action, timeout) 
        {
            this.conference = conference;
            this.state = state;
        }
    }

    /// <summary>Initial connect transaction</summary>
    internal sealed class InitConnectTransactionInfo : TransactionInfo
    {
        /// <summary>An initial connect transaction</summary>
        /// <param name="serverId">The ID of the server we're trying to connect to</param>
        /// <param name="timeout">The number of milliseconds to wait for an MMS response</param>
        public InitConnectTransactionInfo(uint serverId, long timeout) 
            : base(null, timeout) 
        {
            base.serverId = serverId;
        }
    }

    /// <summary>Heartbeat transaction</summary>
    internal sealed class HeartbeatTransactionInfo : TransactionInfo
    {
        /// <summary>An initial connect transaction</summary>
        /// <param name="serverId">The ID of the server we're trying to connect to</param>
        /// <param name="timeout">The number of milliseconds to wait for an MMS response</param>
        public HeartbeatTransactionInfo(uint serverId, long timeout) 
            : base(null, timeout) 
        {
            base.serverId = serverId;
        }
    }
}
