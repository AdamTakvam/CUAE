using System;
using System.Collections;

using Metreos.MMSTestTool.Transactions;
using Metreos.MMSTestTool.Commands;


namespace Metreos.MMSTestTool.Messaging
{
	/// <summary>
	/// Summary description for MMSMessageFactory.
	/// </summary>
	public abstract class MMSMessageFactory
	{
		public MMSMessageFactory()
		{
		}

		/// <summary>
		/// Creates a Server connect message, used in establishing a session connection.
		/// </summary>
		/// <param name="heartbeatInterval"></param>
		/// <param name="heartbeatPayload"></param>
		/// <param name="transId"></param>
		/// <param name="serverId"></param>
		/// <returns></returns>
        public abstract MediaServerMessage CreateServerConnectMessage(string heartbeatInterval, string heartbeatPayload, string transId,  string serverId);
		
        /// <summary>
        /// Creates a server disconnect message, used in disconnecting a session
        /// </summary>
        /// <param name="transId"></param>
        /// <returns></returns>
        public abstract MediaServerMessage CreateServerDisconnectMessage(string transId);

        /// <summary>
        /// Create a heartbeat message to send to the MMS
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="heartbeatId"></param>
        /// <returns></returns>
        public abstract MediaServerMessage CreateHeartbeatMessage(MsTransactionInfo transaction, string heartbeatId);
		
        /// <summary>
        /// Converts a message obtained from the MMS into a ParameterContainer object
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public abstract ParameterContainer DecodeMessage(MediaServerMessage message);

        /// <summary>
        /// Creates a message to send to the MMS
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="message"></param>
        /// <returns></returns>
		public abstract MediaServerMessage CreateMessage(MsTransactionInfo transaction, Command message);
        	
	}
}
