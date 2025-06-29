using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

using Metreos.MMSTestTool.TransportLayer;
using Metreos.MMSTestTool.Commands;
using Metreos.MMSTestTool.Transactions;
using Metreos.MMSTestTool.Messaging;

namespace Metreos.MMSTestTool.Sessions
{
	/// <summary>
	/// Session Manager is basically a static class that handles communication between the various components of the
	/// MMSTestTool. It provides hooks for all the various components like transport objects, sessions, UI, etc to 
	/// connect to. Methods and the variables defined in here are pretty much all static, and this will eventually be implemented as a singleton.
	/// Events are used to trigger various functionality. 
	/// </summary>
	public class SessionManager
	{
	
        #region Properties
        public static bool IsConnected
        {
            get { return msInfo.connectedToServer; }
            set { msInfo.connectedToServer = value; }
        }
        		
        public static int HeartbeatTimeout
        {
            get { return msInfo.msHeartbeatTimeoutValue; } 
            set { msInfo.msHeartbeatTimeoutValue = value; }
        }


        public static int MediaServerConnectTimeout
        {
            get { return msInfo.msConnectTimeoutValue; }
            set { msInfo.msConnectTimeoutValue = value; }
        }
        #endregion

        #region Variable Declarations
		public static TimerCallback onMsHeartbeatTimeout;
		public static TimerCallback onMsConnectTimeout;

		public delegate void OutputDelegate(MMSSession session, string output);
		//event that the GUI can subscribe to. 
		public static OutputDelegate outputEvent;	
	
		//Event and delegate pair for signaling when server connection is obtained
		public delegate void executionDelegate(object sender, ExecutionEventArgs args);
		public static event executionDelegate startEvent;
        public static event executionDelegate stopEvent;
		
        //Event and delegate pair for sending commands to the mms
        public delegate void SendDelegate(object sender, SendEventArgs dataToSend);
        public static event SendDelegate sendEvent;

		//Media server object that holds all information relevant to the MMS
		public static MediaServerInfo msInfo = new MediaServerInfo();

		//an array list to keep track of all the sessions managed by this manager;
		private static ArrayList sessions;

		//transport layer responsible for transporting the message to the MMS
		//in the format constructed by the msgFactory inside the tranport object. 
		private static Transport transport;
		
		//holds the different sessions, key is the mms transaction id 
		private static Hashtable pendingTransactions;

        private static Hashtable sessionCorrelator;

		private static object serverConnectLock = new object();
        private static object receiveLock = new object();

		//hardcoded for now.
		public static string mediaServerIpAddress = "127.0.0.1";

        private static ManualResetEvent connectionEvent = new ManualResetEvent(false);
		
		//parameter names and values are stored in an array, where the name of the parameter is at index 0 and
		//it's value at index 1
		private const int PARAM_NAME_INDEX = 0;
		private const int PARAM_VALUE_INDEX = 1;
		#endregion

        #region Helper class definitions
        public class SendEventArgs : EventArgs
        {
            public MsTransactionInfo transaction;
            public MMSSession session;
            public TestFixture fixture;
            public ScriptExecuter executer;
            public Command command;
            public SendEventArgs()
            {}
        }

        public class ExecutionEventArgs : EventArgs
        {
            public bool execute;
        }
        #endregion

        #region Constructors
		/// <summary>
		/// Constructor for the Session Manager.
		/// </summary>
		/// <param name="factoryType">Fully-qualified name of the message factory class</param>
		/// <param name="transportType">Fully-qualified name of the class handling the sending and receiving of all messages</param>
        public SessionManager(string factoryType, string transportType)
		{
			//possibly change appdomain
			try
			{
				pendingTransactions = new Hashtable();
                sessionCorrelator = new Hashtable();

				sessions = new ArrayList();

				//create a new transport object of the type specified at runtime
				Assembly assembly = Assembly.GetEntryAssembly();
				//Console.WriteLine(assembly.FullName);
				string fullyQualifiedName = typeof(MSMQTransport).FullName;
				transport = assembly.CreateInstance(fullyQualifiedName, true, BindingFlags.CreateInstance, null, new object[] { factoryType }, System.Globalization.CultureInfo.CurrentCulture, null) as Transport;
				
				//add the SessionManager.ReceiveResponse method to the Transport.ReceiveEvent so that we're notified
				//every time a message comes in
				transport.ReceiveEvent += new Transport.ReceiveMessageHandler(ReceiveResponse);

                msInfo.useClientId = true;
			}
			catch (System.Exception e)
			{
				if (outputEvent != null)
                    outputEvent(null,e.ToString());
			}
		}
        #endregion

		#region Sending commands section
		/// <summary>
		/// Used to send Commands to the transport layer
		/// </summary>
		/// <param name="session"></param>
		/// <param name="command"></param>
		/// <returns></returns>
		public static string SendCommand(SendEventArgs args)
		{
			//Debug.Assert(msInfo.connectedToMediaServer, "WARNING: Attempted to send message without an established MMS connection");
			
			AddPendingTransaction(args.transaction.Id, args);
            transport.SendMessage(args.transaction, args.command);
			
			return args.transaction.Id;
		}

        private static void SessionManager_sendEvent(object sender, SendEventArgs dataToSend)
        {
            sessionCorrelator.Add(dataToSend.transaction.Id, dataToSend);
            SendCommand(dataToSend);
        }
        #endregion

        #region Receiving commands section
        /// <summary>
        /// This method handles messages received from the transport object, and forwards them to the appropriate session.
        /// </summary>
        /// <param name="message"></param>
        public static void ReceiveResponse(ParameterContainer message)
        {
            Debug.Assert(message != null,"WARNING: null message received from transport provider");
            string resultCode = GetFieldByName(IMediaServer.FIELD_MS_RESULT_CODE, message);								
            string messageId = GetFieldByName(IMediaServer.FIELD_MS_MESSAGE_ID, message);

            // Check to see if the message is a heartbeat
            if (string.Compare(messageId,IMediaServer.MSG_MS_HEARTBEAT) == 0)
            {
                string heartTransId = GetFieldByName(IMediaServer.FIELD_MS_TRANSACTION_ID,message);
                if (heartTransId != null)
                    PopPendingTransaction(heartTransId);

                OnMsHeartbeat(message);
                return;
            }
		
            // retrieve transactionId from the message
            string transId = GetFieldByName(IMediaServer.FIELD_MS_TRANSACTION_ID, message);
            Debug.Assert(transId != null, "WARNING: Transaction Id was not found");
            Debug.Assert(pendingTransactions.ContainsKey(transId),"WARNING: Received transaction id: " + transId + " not found in pending transactions table");

            //remove the appropriate SendEventArgs from the pendingTransactions table
            SendEventArgs args = PopPendingTransaction(transId);

            //if this is the response to a server connect...
            if (args.transaction.isApplicationServerConnect)
            {
                Debug.Assert(resultCode != null, "WARNING: Received message does not contain a result code!");

                if (resultCode.Equals(IMediaServer.MS_RESULT_OK))
                    OnConnect(message);
                else
                {
                    AddPendingTransaction(transId, args);
                    //ServerConnect();
                }
                return;
            }
            else if (args.transaction.isApplicationServerDisconnect)
            {
                if (resultCode.Equals(IMediaServer.MS_RESULT_OK))
                    OnDisconnect();
                //do nothing for now, eventually error outputelse 
                return;
            }

            //if the transaction we're dealing with is asynchronous...
            if (args.transaction is MsAsyncTransactionInfo)
            {
                MsAsyncTransactionInfo asyncTransaction = args.transaction as MsAsyncTransactionInfo;
                if (!asyncTransaction.provisionalReceived)
                    AddPendingTransaction(transId, args);
            }
            
            args.session.ReceiveResponse(args, message);
            //transaction.session.ReceiveResponse(transaction, message);
        }

		private static string GetFieldByName(string fieldName, ParameterContainer message)
		{
			foreach (ParameterField field in message)
			{
				if (string.Compare(fieldName, field.Name, true) == 0)
					return field.Value;
				
			}

			return null;
		}
		#endregion

        //Ghetto rigged to work right now, needs to be made proper. 
        #region Deal with heartbeats
        /// <summary>Handle heartbeat messages from media servers.</summary>
        /// <remarks>
        /// Heartbeat messages must be sent by media servers at specified
        /// intervals.  When received, a heartbeat message causes a reset
        /// of the heratbeat timeout timer.
        /// </remarks>
        /// <param name="heartbeatMsg">The heartbeat message.</param>
        private static void OnMsHeartbeat(ParameterContainer heartbeatMsg)
        {
            Console.WriteLine("Session Manager received a heartbeat");
            Debug.Assert(msInfo.msHeartbeatTimeout != null, "msInfo.msHeartbeatTimeout is null");
            msInfo.msHeartbeatTimeout.Change(HeartbeatTimeout, HeartbeatTimeout);
            SendMsHeartbeatAck(heartbeatMsg);
            //UpdateMediaServerStatistics(heartbeatMsg);
            //}
            //else
        {
            /*log.Write(TraceLevel.Info, 
                    "Received a media server heartbeat from server with ID {0}, but we are not connected.", 
                    serverId);*/
        }

            //msInfo = null;

            //DebugLog.MethodExit();
        }

        private static void OnMsHeartbeatTimeout(object state)
        {
            Console.WriteLine("Heartbeat timed out!");
			
            /*
            DebugLog.MethodEnter();

            if(this.shuttingDown == false)
            {
                MediaServerInfo msInfo = state as MediaServerInfo;

                if(msInfo == null)
                {
                    log.Write(TraceLevel.Error, "No MediaServerInfo passed into OnMsHeartbeatTimeout()");
                    DebugLog.MethodExit();
                    return;
                }

                StopHeartbeatTimer(msInfo);

                log.Write(TraceLevel.Warning, "Lost connection to media server {0}.", 
                    msInfo.mediaServerIpAddress);

                SessionManager.IsConnected = false;
                ConnectToMediaServer(msInfo);
            }

            DebugLog.MethodExit();*/
        }


        /// <summary>
        /// Acknowledges a heartbeat message received from the media server.
        /// </summary>
        /// <param name="heartbeatMsg">The heartbeat message.</param>
        private static void SendMsHeartbeatAck(ParameterContainer heartbeatMsg)
        {
            string heartbeatId = null;

            if((heartbeatId = GetFieldByName(IMediaServer.FIELD_MS_HEARTBEAT_ID, heartbeatMsg)) == null)
            {
                /*log.Write(TraceLevel.Warning, 
                    "No heartbeat ID in media server heardbeat message. Can not acknowledge");*/
            }
            else
            {
                // heartbeatId was found in the heartbeat message. Lets acknowledge it so the
                // media server knows we are still alive.

                Console.WriteLine("Session Manager responding to heartbeat");
                Debug.Assert(heartbeatId != null);

                // REFACTOR: Do we want to create a transaction for heartbeat ACKs ??
                MsTransactionInfo transaction = new MsTransactionInfo();
                if (msInfo.useServerId)
                    transaction.serverId = uint.Parse(msInfo.serverId);

                lock(SessionManager.pendingTransactions.SyncRoot)
                {
                    //eventually create a HeartbeatEvent
                    AddPendingTransaction(transaction.Id, null);
                }

                //MediaServerMessage heartbeatAckMsg = this.CreateMediaServerMessage(IMediaServer.MSG_MS_HEARTBEAT);
                //heartbeatAckMsg.AddField(new Field(IMediaServer.FIELD_MS_HEARTBEAT_ID, heartbeatId));

                transport.SendHeartbeat(transaction, heartbeatId);
			
            }
        }

        /// <summary>
        /// Updates the internal statistics maintained by the provider
        /// that indicate how many resources a media server has available.
        /// </summary>
        /// <param name="heartbeatMsg">The heartbeat message containing the statistics.</param>
        /*private static void UpdateMediaServerStatistics(MediaServerMessage heartbeatMsg)
        {
            Debug.Assert(msInfo != null);
            Debug.Assert(heartbeatMsg != null);

            string[] resources;

            heartbeatMsg.GetFieldsByName(IMediaServer.FIELD_MS_MEDIA_RES_PAYLOAD, out resources);

            if(resources == null)
            {
                return;
            }

            msInfo.resources.LoadResources(resources);
            resourceManager.UpdateMediaServerResourceInfo(msInfo);
        }*/

        /// <summary>
        /// Reset the heart beat timer for a particular media server.
        /// </summary>
        /// <param name="msInfo">The MediaServerInfo object for the media server to reset
        /// the timer on.</param>
        private static void ResetHeartbeatTimer(MediaServerInfo msInfo)
        {
            StopHeartbeatTimer(msInfo);

            msInfo.msHeartbeatTimeout = new System.Threading.Timer(
                onMsHeartbeatTimeout,
                msInfo,
                HeartbeatTimeout,
                HeartbeatTimeout);
        }


        /// <summary>
        /// Stop the heart beat timer for a particular media server.
        /// </summary>
        /// <param name="msInfo">The MediaServerInfo object for the media server to stop
        /// the timer on.</param>
        private static void StopHeartbeatTimer(MediaServerInfo msInfo)
        {
            if(msInfo.msHeartbeatTimeout != null)
            {
                try
                {
                    msInfo.msHeartbeatTimeout.Dispose();
                    msInfo.msHeartbeatTimeout = null;
                }
                catch(ObjectDisposedException)
                {}
            }
        }
        #endregion

		#region pendingTransaction manipulation
		/// <summary>
		/// Adds a transaction-SendEventsArg pair to the pendingTransactions table, using the transactionId as hash.
		/// </summary>
		/// <param name="sessionScript"></param>
		/// <returns></returns>
		private static void AddPendingTransaction(string transactionId, SendEventArgs args)
		{
			lock (pendingTransactions.SyncRoot)
			{
				Debug.Assert(transactionId != string.Empty, "WARNING: Attempted to add a transaction without a transactionId");
				Debug.Assert(!pendingTransactions.ContainsKey(transactionId),"WARNING: Failed to add duplicate entry into pending transactions table");
				pendingTransactions.Add(transactionId,args);
			}
		}
		
		/// <summary>
		/// Looks for the transaction-session pair associated with the transactionId specified, removes it from the
		/// pending transactions table and returns the removed object.
		/// </summary>
		/// <param name="transactionId"></param>
		/// <returns></returns>
		private static SendEventArgs PopPendingTransaction(string transactionId)
		{
			lock (pendingTransactions.SyncRoot)
			{
				Debug.Assert(pendingTransactions.ContainsKey(transactionId), "WARNING: Attempt to remove a transaction that was not in the pending table");
				SendEventArgs args = (SendEventArgs)pendingTransactions[transactionId];
				pendingTransactions.Remove(transactionId);
				return args;
			}
		}
		#endregion

        #region Session Manager Methods handling MMS connects/disconnects
        //establishes connection to the MMS
		public static void ServerConnect(string heartBeatInterval, string heartBeatSkew)
		{
			lock(serverConnectLock)
			{
				if (!SessionManager.IsConnected)
				{
					
                    MsTransactionInfo transaction = new MsTransactionInfo();
					transaction.isApplicationServerConnect = true;

					HeartbeatTimeout = (int.Parse(heartBeatInterval) + int.Parse(heartBeatSkew)) * 1000;
					onMsHeartbeatTimeout = new System.Threading.TimerCallback(OnMsHeartbeatTimeout);
					ResetHeartbeatTimer(msInfo);

					//this.onMsConnectTimeout = new System.Threading.TimerCallback(this.OnMsConnectTimeout);
                    SendEventArgs args = new SendEventArgs();
                    args.transaction = transaction;
					AddPendingTransaction(transaction.Id, args);
    				
                    //Need to allow the session to handle this, but for now just leaving this here so that it works.
                    //Sessions also need to perform their own reset
                    transport.Initialize();
                    
                    transport.ServerConnect(transaction);
				}
			}
		}

        private static void OnConnect(ParameterContainer message)
        {
            //don't need both, refactor
            transport.Connected = true;
            SessionManager.IsConnected = true;
									
            msInfo.clientId = GetFieldByName(IMediaServer.FIELD_MS_CLIENT_ID, message);
            msInfo.serverId = GetFieldByName(IMediaServer.FIELD_MS_SERVER_ID, message);

            connectionEvent.Set();
        }

        public static void ServerDisconnect()
        {
            lock (serverConnectLock)
            {
                MsTransactionInfo transaction = new MsTransactionInfo();
                transaction.isApplicationServerDisconnect = true;
                ResetHeartbeatTimer(msInfo);

                SendEventArgs args = new SendEventArgs();
                args.transaction = transaction;

                AddPendingTransaction(transaction.Id, args);
                transport.ServerDisconnect(transaction);
            }
        }

        private static void OnDisconnect()
        {
            SessionManager.IsConnected = false;
            transport.ShutDown();
            Console.WriteLine(Constants.SM_EXEC_END);
            //System.Environment.Exit(0);
        }
        #endregion

        public static void StartExecution()
        {
            if (!IsConnected)
            {
                //change to not be static
                ServerConnect("10", "10");
                connectionEvent.WaitOne();
                if (startEvent != null)
                {
                    ExecutionEventArgs args = new ExecutionEventArgs();
                    args.execute = true;
                    startEvent(null, args);
                }
            }
            else
            {
                if (startEvent != null)
                {
                    ExecutionEventArgs args = new ExecutionEventArgs();
                    args.execute = true;
                    startEvent(null, args);
                }
            }
        }

        #region Methods dealing with sessions
        //creates a new session
		public static bool AddSession(string name, ArrayList fixtures, int numOfInstances, int commandTimeoutMsecs)
		{
            try
            {
                //hardcoded number of instances, change
                MMSSession session = new MMSSession(name, fixtures, numOfInstances, commandTimeoutMsecs);
                sessions.Add(session);
                session.sendEvent += new SendDelegate(SessionManager_sendEvent);

                //there may be a potential synchronization issue here... unlikely.
                startEvent += new executionDelegate(session.ProcessExecution);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                /*if (outputEvent != null)
                    outputEvent(null,e.Message);*/
                return false;
            }

			return true;
        }
    }
    #endregion

}