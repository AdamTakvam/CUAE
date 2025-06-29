using System;
using Metreos.MMSTestTool.Transactions;
using Metreos.MMSTestTool.Commands;
using Metreos.MMSTestTool.Messaging;

namespace Metreos.MMSTestTool.TransportLayer
{
	/// <summary>
	/// Interface defining the methods that all all children of Transport must implement
	/// </summary>
	interface ITransport
	{
		/// <summary>
		/// Establishes the connection to the MMS server, only executed once. 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="transId"></param>
		/// <returns></returns>
		void ServerConnect(string transId);
		
		/// <summary>
		/// Decides what to do with a received MMS message, used for pre-processing (if any) before handing it off
		/// to the session manager
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		void HandleMediaServerMessage(MediaServerMessage message);

		bool SendMessage(CommandBase command);
	}

}
