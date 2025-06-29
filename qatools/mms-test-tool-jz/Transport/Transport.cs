using System;
using System.Collections;

using Metreos.MMSTestTool.Commands;
using Metreos.MMSTestTool.Messaging;
using Metreos.MMSTestTool.Sessions;
using Metreos.MMSTestTool.Transactions;

namespace Metreos.MMSTestTool.TransportLayer
{
	/// <summary>
	/// Summary description for TransportManager.
	/// </summary>
	public abstract class Transport 
	{
		public class TransportException : System.ApplicationException
		{
			public TransportException() : base("Transport Exception occured...") {}

			public TransportException(string message) : base(message) {}
		}

        /// <summary>
		/// Property used to change the "connected" status of the transport object. 
		/// </summary>
		public bool Connected
		{
			get
			{
				return connected;
			}
			set
			{
				connected = value;
			}
		}

        #region Variable declarations
        public event ReceiveMessageHandler ReceiveEvent;
		public delegate void ReceiveMessageHandler(ParameterContainer messageList);	
		
		//the Factory that builds messages in desired format and outputs a string array;
		protected MMSMessageFactory msgFactory;

		//And object used for locking
		protected Object initLock = new Object();
		
		protected bool connected = false;
		protected volatile bool run = false;
        #endregion

		/// <summary>
		/// Creates a message factory of the specified type. Needs a try/catch block.
		/// </summary>
		/// <param name="factoryType"></param>
        public Transport(string factoryType)	
		{
			lock(initLock)
			{
				if (msgFactory != null)
				{
					msgFactory = (MMSMessageFactory)AppDomain.CurrentDomain.CreateInstanceAndUnwrap(AppDomain.CurrentDomain.ToString(),factoryType);
				}
			}
		}
		
		/// <summary>
		/// Raises the ReceiveEvent event. Needs to be modified to match proper event(Object,EventArgs) format.
		/// </summary>
		/// <param name="message"></param>
        public void DoReceiveEvent(ParameterContainer message)
		{
			if (this.ReceiveEvent != null)
			{
				this.ReceiveEvent(message);
			}
		}

		/// <summary>
		/// Session server connect
		/// </summary>
		/// <param name="transaction"></param>
		public abstract void ServerConnect(MsTransactionInfo transaction);

        /// <summary>
        /// Session server disconnect
        /// </summary>
        /// <param name="transaction"></param>
        public abstract void ServerDisconnect(MsTransactionInfo transaction);

		/// <summary>
		/// Deals with messages received from the MMS
		/// </summary>
		/// <param name="message"></param>
        public abstract void HandleMediaServerMessage(MediaServerMessage message);

		/// <summary>
		/// Sends a message to the MMS
		/// </summary>
		/// <param name="transaction"></param>
		/// <param name="command"></param>
		/// <returns></returns>
        public abstract bool SendMessage(MsTransactionInfo transaction, Command command);

		/// <summary>
		/// Sends a heartbeat to the MMS
		/// </summary>
		/// <param name="transaction"></param>
		/// <param name="heartbeatId"></param>
		/// <returns></returns>
        public abstract bool SendHeartbeat(MsTransactionInfo transaction, string heartbeatId);

        /// <summary>
        /// Initializes whatever resources may be needed for communicating with the MMS. Should probably be in an interface instead
        /// </summary>
        /// <returns></returns>
        public abstract bool Initialize();

        /// <summary>
        /// resets whatever resources are necessary. Needs to be called when a new fixture starts running?
        /// </summary>
        /// <returns></returns>
        public abstract bool ShutDown();
	}
}
