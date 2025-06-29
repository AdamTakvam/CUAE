using System;
using System.Diagnostics;
using System.Messaging;
using System.Text;

using Metreos.LoggingFramework;

namespace Metreos.Messaging
{
    /// <summary>
    /// Provider for Metreos.Samoa.Core.MessageQueue that utilizes
    /// Microsoft Message Queue, encompassed in System.Messaging.
    /// </summary>
    public class MsmqMessageQueueProvider : Loggable, IMessageQueueProvider
    {
        // REFACTOR Do not automatically insert "Metreos-" into the queue ID.

        /// <summary>
        /// Token that is prepended to all queueIds.
        /// </summary>
        private const string METREOS_TOKEN = "Metreos-";

        /// <summary>
        /// Token that is used to specify a private queue in MSMQ.
        /// </summary>
        private const string PRIVATE_QUEUE_TOKEN = "Private$";

		/// <summary>
		/// List of valid state values
		/// </summary>
		private enum State { Started, Shutdown }

		/// <summary>
		/// State variable
		/// </summary>
		private State state;

        /// <summary>
        /// The complete path for this message queue.
        /// </summary>
        /// <example>
        /// REMOTE: FormatName:Direct=OS:192.168.1.1\Private$\Metreos-SomeRemoteQueue
        /// LOCAL: .\Private$\Metreos-SomeLocalQueue
        /// </example>
        private string completeQueuePath;

        /// <summary>
        /// Gets the complete queue path for the message queue.
        /// </summary>
        public string QueuePath
        {
            get { return completeQueuePath; }
        }

        /// <summary>
        /// The queue id. This is the last part of the queue path.
        /// </summary>
        /// <example>
        /// In the following, "SomeRemoteQueue" is the queueId:
        ///     FormatName:Direct=OS:192.168.1.1\Private$\Metreos-SomeRemoteQueue
        /// </example>
        private string queueId;

        /// <summary>
        /// The IP address of the machine that we are connecting to to
        /// access the queue. This is always set to "." if the 
        /// queue is not flagged as remote.
        /// </summary>
        private string machineIpAddress;

        /// <summary>
        /// Is the queue that this provider is using remote?
        /// </summary>
        private bool remoteQueue;

        /// <summary>
        /// Gets whether the queue is remote.
        /// </summary>
        public bool IsRemoteQueue
        {
            get { return remoteQueue; }
        }
       
        /// <summary>
        /// The MSMQ object.
        /// </summary>
        private System.Messaging.MessageQueue queue;
        
        private object sendLock;

        public MsmqMessageQueueProvider(string queueId) : this(".", false, queueId)
        {}


        public MsmqMessageQueueProvider(string machineIpAddress, bool remoteQ, string queueId) 
            : base(TraceLevel.Verbose, "MsmqMessageQueueProvider")
        {
            Debug.Assert(queueId != null, "queueId is null.");
            Debug.Assert(queueId != "", "queueId is empty.");

            sendLock = new object();
            
            if(IsCompleteQueuePath(queueId))
            {
                ParseCompleteQueuePath(queueId);
            }
            
            if((this.queueId == null) || (this.queueId == "") ||
               (this.machineIpAddress == null) || (this.machineIpAddress == ""))
            {
                this.remoteQueue = remoteQ;

                if(machineIpAddress == null)
                {
                    machineIpAddress = ".";
                }

                this.machineIpAddress = machineIpAddress;
                this.queueId = queueId;
            }

            Initialize();
        }


        /// <summary>
        /// Initialize this message queue for use.
        /// </summary>
        /// <param name="queueId">The queue ID. If this queue does not exist, it will be created.</param>
        public void Initialize()
        {
			state = State.Started;

            BuildCompleteQueuePath();

            try
            {
                if(this.remoteQueue == false)
                {
                    if(System.Messaging.MessageQueue.Exists(this.completeQueuePath) == true)
                    {
                        this.queue = new System.Messaging.MessageQueue(this.completeQueuePath);
                    }
                    else
                    {
                        this.queue = System.Messaging.MessageQueue.Create(this.completeQueuePath);
                    }
                }
                else
                {
                    this.queue = new System.Messaging.MessageQueue(this.completeQueuePath);
                }

                this.queue.Formatter = new XmlMessageFormatter(new Type[] {typeof(InternalMessage)}); 
                
                if(this.remoteQueue == false)
                {
                    // REFACTOR Add journaling based on a config option.
                    // this.queue.UseJournalQueue = true;
                }
            }
            catch(MessageQueueException e)
            {
                log.Write(TraceLevel.Error, "MessageQueueException caught (Initialize):\n" + e.ToString());
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Unknown exception caught (Initialize):\n" + e.ToString());
            }
        }


        /// <summary>
        /// Determines whether the queue path that is passed in is a complete
        /// path. If it is, it should be parsed into its individual elements.
        /// </summary>
        /// <param name="queuePath">The queue path string to check.</param>
        /// <returns>True if queuePath contains a complete path.</returns>
        private bool IsCompleteQueuePath(string queuePath)
        {
            if(queuePath.IndexOf("/") > 0)
            {
                // We have \'s in the queuePath, so assume it is "complete"

                return true;
            }

            return false;
        }


        /// <summary>
        /// Parse a complete queue path into its individual elements.
        /// </summary>
        /// <param name="queuePath">The complete queue path to parse.</param>
        private void ParseCompleteQueuePath(string queuePath)
        {
            // Very, very, very crude test as to whether this is a remote queue.
            if(queuePath.ToLower().StartsWith("formatname:direct=os"))
            {
                this.remoteQueue = true;
            }

            if(remoteQueue)
            {
                queuePath = queuePath.Substring(queuePath.LastIndexOf(':') + 1);
            }

            // pathBits should contain 3 elements:
            //   0 -> IP address
            //   1 -> "Private$" token
            //   2 -> Queue id
            string[] pathBits = queuePath.Split('/');

            if(pathBits.Length != 3)
            {
                log.Write(TraceLevel.Warning, "ParseCompleteQueuePath encountered an invalid queue path: " + queuePath);
                return;
            }

            this.machineIpAddress = pathBits[0];

            this.queueId = pathBits[2];
        }


        /// <summary>
        /// Build the complete queue path from the memeber variables.
        /// </summary>
        private void BuildCompleteQueuePath()
        {
            StringBuilder qPath = new StringBuilder();
            
            if(this.remoteQueue == true)
            {
                qPath.Append("FormatName:DIRECT=OS:");
            }

            qPath.Append(this.machineIpAddress);
            qPath.Append("/");

            qPath.Append(PRIVATE_QUEUE_TOKEN);
            qPath.Append("/");

            if(queueId.StartsWith(METREOS_TOKEN) == false)
            {
                qPath.Append(METREOS_TOKEN);
            }

            qPath.Append(this.queueId);

            this.completeQueuePath = qPath.ToString();
        }


        /// <summary>
        /// Send a message to this queue.
        /// </summary>
        /// <param name="message">The message to be sent. Will be serialized using XML.</param>
        public void Send(InternalMessage message)
        {
            Debug.Assert(message != null, "Attempt to send a null message object");
            Debug.Assert(this.queue != null, "Internal message queue is null");
            
            try
            {
                lock(sendLock)
                {
                    this.queue.Send(message, System.Messaging.MessageQueueTransactionType.None);
                }
            }
            catch(MessageQueueException e)
            {
                if( (e.MessageQueueErrorCode == MessageQueueErrorCode.QueueDeleted) ||
                    (e.MessageQueueErrorCode == MessageQueueErrorCode.QueueNotAvailable) ||
                    (e.MessageQueueErrorCode == MessageQueueErrorCode.QueueNotFound))
                {
                    log.Write(TraceLevel.Verbose, 
                        "MessageQueue Send() failed: {0}. {1}",
                        e.Message,
                        e.MessageQueueErrorCode);
                }
                else
                {
                    log.Write(TraceLevel.Error,
                        "MessageQueue Send() failed: {0}. {1}",
                        e.Message,
                        e.MessageQueueErrorCode);
                }
            }
        }


        /// <summary>
        /// Receive a message off of this queue.
        /// </summary>
        public bool Receive(System.TimeSpan timeout, out InternalMessage message)
        {
            Debug.Assert(this.queue != null, "Internal message queue is null");
            Debug.Assert(this.queue.QueueName.ToLower() != "metreos-", "The queue does not have a complete name.");

            System.Messaging.Message msmqMessage = null;

            try
            {
                msmqMessage = this.queue.Receive(timeout);

                Debug.Assert(msmqMessage.Body is InternalMessage, "Received message body is not an internal message");
                
                message = msmqMessage.Body as InternalMessage;

                Debug.Assert(message != null, "Internal message object is null");
            }
            catch(MessageQueueException e)
            {
                message = null;

                if(e.MessageQueueErrorCode != MessageQueueErrorCode.IOTimeout)
                {
                    log.WriteIf(state != State.Shutdown, 
                        TraceLevel.Error, 
                        "MessageQueueException caught: {0}. ErrorCode: {1}",
                        e.Message,
                        e.MessageQueueErrorCode);
                }

                return false;
            }
            catch(InvalidOperationException e)
            {
                message = null;

                log.WriteIf(state != State.Shutdown, 
                    TraceLevel.Error, 
                    "InvalidOperationException caught (Receive): {0}",
                    e.Message);

                return false;
            }
            finally
            {
                if(msmqMessage != null)
                {
                    msmqMessage.Dispose();
                    msmqMessage = null;
                }
            }
            
            return true;
        }


        /// <summary>
        /// Retrieve the friendly queue name.
        /// </summary>
        /// <returns>String containing the friendly queue name.</returns>
        public string GetQueueId()
        {
            return this.queueId;
        }

        public int GetQueueLength()
        {
            return 0;
        }

        public void Purge()
        {
            try
            {
                this.queue.Purge();
            }
            catch(MessageQueueException e)
            {
                log.Write(TraceLevel.Error, "MessageQueueException caught (Purge): {0}. {1}",
                    e.Message, e.MessageQueueErrorCode);
            }
        }

        
        /// <summary>
        /// Release the resources that this queue is using. This does not 
        /// delete the queue from the system.
        /// </summary>
        public void ReleaseResources()
        {
            try
            {
                this.queue.Close();
            }
            catch(MessageQueueException e) 
            {
                log.Write(TraceLevel.Error, "MessageQueueException caught (ReleaseResources): {0}. {1}",
                    e.Message, e.MessageQueueErrorCode);
            }
        }


        /// <summary>
        /// Delete this queue and release all resources.
        /// </summary>
        public void Delete()
        {
			state = State.Shutdown;

            try
            {
                this.queue.Purge();                         // Purge the queue of messages
                this.ReleaseResources();

                if(System.Messaging.MessageQueue.Exists(this.queue.Path))
                {
                    System.Messaging.MessageQueue.Delete(this.queue.Path);  // Delete the queue from MSMQ
                }

                System.Messaging.MessageQueue.ClearConnectionCache();
            }
            catch(MessageQueueException e)
            {
                if( (e.MessageQueueErrorCode == MessageQueueErrorCode.QueueNotFound) ||
                    (e.MessageQueueErrorCode == MessageQueueErrorCode.QueueDeleted))
                {
                    // The queue is already gone.
                    log.Write(TraceLevel.Warning, "The queue can not be deleted because it was not found");
                }
                else
                {
                    log.Write(TraceLevel.Error, "MessageQueueException caught (Delete): {0}. {1}",
                        e.Message, e.MessageQueueErrorCode);
                }
            }
        }


        public MessageQueueWriter GetMessageQueueWriter()
        {
            return new MessageQueueWriter(this);
        }
    }
}
