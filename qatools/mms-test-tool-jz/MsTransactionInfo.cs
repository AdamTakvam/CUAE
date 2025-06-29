using System;
using Metreos.MMSTestTool.Sessions;


namespace Metreos.MMSTestTool.Transactions
{
	/// <summary>
	/// A standard media server transcation.
	/// </summary>
    public class MsTransactionInfo
    {
        public MsTransactionInfo()
        {
            // Generate a new transaction ID to correlate responses with.
            Id = TransactionIdFactory.GetTransactionId().ToString();
        }
		
        /// <summary>
        /// Transaction ID for this transaction. Its just a sequence number.
        /// </summary>
        public string Id;

        /// <summary>
        /// The ID of the server processing this transaction.
        /// </summary>
        public uint serverId;

        /// <summary>
        /// Indicates whether this transaction is for a server
        /// connect message.   
        /// </summary>
        public bool isApplicationServerConnect = false;

        /// <summary>
        /// indicates whether this transaction is for a server
        /// disconnect message. 
        /// </summary>
        public bool isApplicationServerDisconnect = false;

        /// <summary>
        /// Holds the current status of a transaction
        /// </summary>
        public TRANSACTION_RESULT status = TRANSACTION_RESULT.NOT_STARTED;

        /// <summary>
        /// Holds the current status of final assert checks, 
        /// </summary>
        public ASSERT_RESULT finalAssertResult = ASSERT_RESULT.NOT_PERFORMED; 

        /// <summary>
        /// Enumerations describing the results of a transaction
        /// </summary>
        public enum TRANSACTION_RESULT
        {
            NOT_STARTED, EXECUTING, FAILED, SUCCEEDED,
            RECEIVED_COMMAND_NOT_FOUND, RESULT_CODE_NOT_FOUND,  
            PROVISIONAL_RECEIVED, COMMAND_TIMEOUT, 
            PROVISIONAL_ASSERT_FAILED, PROVISIONAL_ASSERT_NOT_MATCHED, PROVISIONAL_ASSERT_OK, DUPLICATE_PROVISIONAL,
        }

        /// <summary>
        /// enumerations describing the results of assert checks
        /// </summary>
        public enum ASSERT_RESULT
        {
            NOT_PERFORMED,
            PROVISIONAL_ASSERT_FAILED, PROVISIONAL_ASSERT_NOT_MATCHED, PROVISIONAL_ASSERT_OK,
            FINAL_ASSERT_FAILED, FINAL_ASSERT_NOT_MATCHED, FINAL_ASSERT_OK,
            ASSERT_NOT_MATCHED,	ASSERT_FAILED, ASSERTS_OK
        }
    }


	/// <summary>
	/// Asynchronous media server transaction. Commands that use transactions
	/// of this type expect the media server to issue both a provisional and
	/// final response. The final response comes from the media server
	/// as an unsolicited event.
	/// </summary>
	internal class MsAsyncTransactionInfo : MsTransactionInfo
	{
		public MsAsyncTransactionInfo() : base()
		{}

		/// <summary>
		/// Indicates whether a provisional response has been received
		/// for this command.
		/// </summary>
		public bool provisionalReceived = false;

        /// <summary>
        /// Holds the current status of final assert checks, 
        /// </summary>
        public ASSERT_RESULT provisionalAssertResult = ASSERT_RESULT.NOT_PERFORMED; 
        
        /// <summary>
		/// User-specific data used to correlate the async response 
		/// to the original action.
		/// </summary>
		public string state;
	}
}
