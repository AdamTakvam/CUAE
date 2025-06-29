using System;
using System.Messaging;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

using Metreos.MMSTestTool.Messaging;
using Metreos.MMSTestTool.Sessions;
using Metreos.MMSTestTool.Commands;
using Metreos.MMSTestTool.Transactions;



namespace Metreos.MMSTestTool.TransportLayer
{
	/// <summary>
	/// Summary description for MSMQTransport.
	/// </summary>
	public class MSMQTransport : Transport
	{
        //Make these not ghetto, and configurable from the MMS
		#region Queue declarations
		/// <summary>
		/// Queue used for sending messages to the MMS
		/// </summary>
		private MessageQueue sendQueue;
		
		/// <summary>
		/// The default queue name that the media server is 
		/// listening on for messages.
		/// </summary>
		// REFACTOR: Config variable.
		private const string mediaServerQueueName = "Private$\\metreos-mediaserver";
		
		/// <summary>
		/// The send queue path of the media server tester
		/// </summary>
		private string SEND_QUEUE_PATH = "FormatName:DIRECT=TCP:" 
					+ SessionManager.mediaServerIpAddress + "\\"+ mediaServerQueueName;



		/// <summary>
		/// The queue on which messages are received
		/// </summary>
		private MessageQueue receiveQueue;
		
		//public event Transport.ReceiveMessageHandler ReceiveEvent;

		private const string RECEIVE_QUEUE_ID = "mediaserver-test";
		/// <summary>
		/// The receive queue path of the media server manager.
		/// </summary>
		private const string RECEIVE_QUEUE_PATH = ".\\Private$\\" + RECEIVE_QUEUE_ID;


		/// <summary>
		/// Thread for reading from our MSMQ message queue.
		/// </summary>
		private Thread receiveQueueReadThread;
		#endregion
		
		public MSMQTransport(string factoryType) : base(factoryType)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			string fullyQualifiedName = "Metreos.MMSTestTool.Messaging.XmlMMSMessageFactory";
			msgFactory = assembly.CreateInstance(fullyQualifiedName, true, BindingFlags.CreateInstance, null, null, System.Globalization.CultureInfo.CurrentCulture, null) as MMSMessageFactory;
		}

        #region Queue creation
        /// <summary>
        /// Creates the MSMQ receive message queue.
        /// </summary>
        /// <remarks>If the queue exists, it is removed and re-created.</remarks>
        private void CreateReceiveQueue()
        {
            try
            {
                if(System.Messaging.MessageQueue.Exists(RECEIVE_QUEUE_PATH))
                {
                    this.receiveQueue = new System.Messaging.MessageQueue(RECEIVE_QUEUE_PATH);
                    this.receiveQueue.Purge();
                }
                else
                {
                    this.receiveQueue = System.Messaging.MessageQueue.Create(RECEIVE_QUEUE_PATH);
                }

                Debug.Assert(this.receiveQueue != null);
                this.receiveQueue.Label = "Metreos Media Server Tester Receive Queue";
                this.receiveQueue.Formatter = new System.Messaging.XmlMessageFormatter(new Type[] { typeof(MediaServerMessage) });
            }
            catch(System.Messaging.MessageQueueException e)
            {
                //log.Write(TraceLevel.Error, "System.Messaging.MessageQueueException caught: " + e.Message);
            }
            catch(Exception e)
            {
                //log.Write(TraceLevel.Error, "Unknown exception caught: " + e.Message);
            }
        }

        //NEED TO DELETE IF IT ALREADY EXISTS!
        /// <summary>
        /// Creates a MSMQ message queue for communicating with a media server.
        /// </summary>
        /// <returns>A MSMQ SysMessaging.MessageQueue object configured to communicate with
        /// the media server.</returns>
        private void CreateSendQueue()
        {
            try
            {
                if (MessageQueue.Exists(mediaServerQueueName))
                {
                    MessageQueue.Delete(mediaServerQueueName);
                }
                this.sendQueue = new System.Messaging.MessageQueue(SEND_QUEUE_PATH);
                //this.sendQueue.Label = "Metreos Media Server Tester Send Queue";
                //this.sendQueue.Purge();
            }
            catch(Exception e)
            {
                /*log.Write(TraceLevel.Error, 
                    "Failed creating media server transmit message queue: {0}. {1}",
                    SEND_QUEUE_PATH, e.Message);*/
            }

        }
        #endregion

        #region ShutDown() and Initialize()
        public override bool ShutDown()
        {
            try
            {
                run = false;
                //receiveQueueReadThread.Join(); //why isn't this working properly?
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool Initialize()
        {
            try
            {
                run = true;
                CreateReceiveQueue();
                CreateSendQueue();
                receiveQueueReadThread = new Thread(new ThreadStart(ReceiveQueueReadThread));
                receiveQueueReadThread.Start();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Server Connect and disconnect
		//Oy. Needs configuration... 
        public override void ServerConnect(MsTransactionInfo transaction)
			//string heartbeatInterval, string heartbeatPayload, string transId, string serverId)
		{
			MediaServerMessage message = null;
			lock(this.initLock)
			{
				if (!this.Connected)
				{
					string heartbeatInterval = SessionManager.msInfo.heartbeatInterval;
					string heartbeatPayload	 = SessionManager.msInfo.heartbeatPayload;
					string serverId			 = SessionManager.msInfo.serverId;

					message = msgFactory.CreateServerConnectMessage(heartbeatInterval, heartbeatPayload, transaction.Id, serverId);
				    message.AddField(IMediaServer.FIELD_MS_QUEUE_NAME, RECEIVE_QUEUE_ID);
		
					try
					{
						System.Messaging.Message msmqMsg = new Message(message);
						sendQueue.Send(msmqMsg);
					}
					catch (System.Exception e)
					{
						
					}

					//this.Connected = true;
				}
			}
		}

        public override void ServerDisconnect(MsTransactionInfo transaction)
        {
            lock (this.initLock)
            {
                if (this.connected)
                {
                    try
                    {
                        System.Messaging.Message msmqMsg = new Message(msgFactory.CreateServerDisconnectMessage(transaction.Id));
                        sendQueue.Send(msmqMsg);
                    }
                    catch (System.Exception e)
                    {
						Console.WriteLine(e.Message);
                    }
                }
            }
        }
		#endregion


		/// <summary>
		/// Decodes MultiMediaServer message into it's constituent fields, then sends it off
		/// to be raised as an event. 
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public override void HandleMediaServerMessage(MediaServerMessage message)
		{
			base.DoReceiveEvent(this.msgFactory.DecodeMessage(message));
        }
		
		
		#region Sending and receiving messages
		/// <summary>
		/// Executes a loop to receive messages from the MSMQ receive queue.
		/// </summary>
		/// <remarks>The thread will exit if run == false.  To kick the
		/// receive out of its blocking state, remove the message queue from 
		/// the system.</remarks>
		private void ReceiveQueueReadThread()
		{
			System.Messaging.Message msmqMsg;
			MediaServerMessage msg; 
			//log.Write(TraceLevel.Info, "Receive thread beginning");
			
			while(this.run == true)
			{
				try
				{
                    msmqMsg = this.receiveQueue.Receive();
                    Debug.Assert(msmqMsg.Body is MediaServerMessage, "Received message body is not a media server message");
                    msmqMsg.Dispose();
                    msg = msmqMsg.Body as MediaServerMessage;

                    if(msg != null)
                    {
                        HandleMediaServerMessage(msg);
                        msg = null;
                    }
				}
				catch //sure would be good if this did something at some point. soon.
				{
                }
			}
		}

        public override bool SendMessage(MsTransactionInfo transaction, Command command)
		{
			MediaServerMessage messageToSend = msgFactory.CreateMessage(transaction, command);
			
			try
			{
				System.Messaging.Message msmqMsg = new Message(messageToSend);
				sendQueue.Send(msmqMsg);
			}
			catch (System.Exception e)
			{
				return false;
			}

			return true;			
		}

		public override bool SendHeartbeat(MsTransactionInfo transaction, string heartbeatId)
		{
			MediaServerMessage heartbeatMessage = msgFactory.CreateHeartbeatMessage(transaction,heartbeatId);
            			
			try
			{
				System.Messaging.Message msmqMsg = new Message(heartbeatMessage);
				sendQueue.Send(msmqMsg);
			}
			catch (System.Exception e)
			{
				return false;
			}

			return true;
		}

		#endregion
	}
}
