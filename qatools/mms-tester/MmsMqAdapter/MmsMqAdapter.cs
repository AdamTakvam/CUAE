using System;
using System.Diagnostics;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Samoa.Core;
using Metreos.Samoa.Interfaces;

using Metreos.MmsTester.Interfaces;
using Metreos.MmsTester.AdapterFramework;

namespace Metreos.MmsTester.Custom.Adapters
{
    /// <summary>
    /// Summary description for MmsMqAdapter.
    /// </summary>
    [Adapter("Message Queuing")]
    public class MmsMqAdapter : AdapterBase
    {
        /// <summary>
        /// Timeout used in calls to this task's queue's Receive() method.
        /// </summary>
        protected const int QUEUE_RECEIVE_TIMEOUT_SECS = 300;

        protected const int NUM_TIMES_ATTEMPT_TO_CONNECT = 50;
        protected const int WAIT_TO_REATTEMPT_TO_CONNECT = 1000; //milliseconds

        /// <summary>
        /// The queue ID for this task.
        /// </summary>
        protected string taskQueueId;

        /// <summary>
        /// Running list of all outstanding responses yet to come back from the MediaServer
        /// </summary>
        protected Hashtable pendingResponses;
        /// <summary>
        /// Synchronized version of pendingResponses
        /// </summary>
        protected Hashtable PendingResponses;

        private MessageQueueWriter mediaServerWriter;
        private string mediaServerMachineName;
        private string mediaServerMessageQueueName;
        private MessageQueue taskQueue;
        private TimeSpan queueTimeout;
        private bool stopReceivingMessages;

		public MmsMqAdapter(string taskName) : base(taskName)
		{
			stopReceivingMessages = true;
            displayName = "Message Queuing";

            pendingResponses = new Hashtable();
            PendingResponses = Hashtable.Synchronized(pendingResponses);

            taskQueue = new MessageQueue(new MsmqMessageQueueProvider(this.taskQueueId));

            // Make sure our queue is empty.
            taskQueue.Purge();                              

            // Get our timeout object ready. This is used repeatedly inside Run(), so
            // lets only build it once.
            this.queueTimeout = new System.TimeSpan(0, 0, 0, QUEUE_RECEIVE_TIMEOUT_SECS);

            
            
		}

        public override bool Initialize(object mediaServerHandle)
        {
            StringCollection mediaServerInformation = (StringCollection) mediaServerHandle;

            mediaServerMachineName = mediaServerInformation[0];
            mediaServerMessageQueueName = mediaServerInformation[1];

            // Create message queue to attach to the Media Server
            for(int i = 0; i < NUM_TIMES_ATTEMPT_TO_CONNECT; i++)
            {
                try
                {
                    this.mediaServerWriter = new MessageQueueWriter(
                        new MsmqMessageQueueProvider( mediaServerMachineName, true, mediaServerMessageQueueName));

                    break;
                }
                catch
                {
                    log.Write(TraceLevel.Warning, "Failed to connect to the Metreos Media Server.  Trying again in " + WAIT_TO_REATTEMPT_TO_CONNECT + " milliseconds");
                }
                System.Threading.Thread.Sleep(WAIT_TO_REATTEMPT_TO_CONNECT);
            }

            stopReceivingMessages = false;

            base.Start();

            return true;
        }

        public override void Cleanup()
        {
            PendingResponses.Clear();

            this.mediaServerWriter.Cleanup();

            base.Cleanup();
        }

        public override bool Send(InternalMessage im, IAdapterTypes.IncomingResponseDelegate incomingResponseCallback)
        {   
            PushPendingRequest(im, incomingResponseCallback);

            try
            {
                mediaServerWriter.PostMessage(im);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "The MmsMqAdapter failed to send to the MMS.  Exception caught was:\n" + e.ToString());
                return false;
            }

            return true;
        }

        protected override void Run()
        {
            InternalMessage im;
            bool messageReceived;

            while(stopReceivingMessages == false)
            {
                // Receive a message from our queue. Block until a message arrives or
                // a timeout occurs.
                messageReceived = taskQueue.Receive(queueTimeout, out im);

                if(messageReceived)
                {
                    if(this.HandleMessage(im) == false)
                    {
                        this.DefaultMessageHandler(im);
                    }
                }
                else
                {
                    // REFACTOR: log the queue timeout and maybe do something intelligent

                    // We probably need to do a keep alive here. Every task needs to report
                    // into a central object and receive an ACK to their keep alive. If they 
                    // don't get an ACK, then we're in panic mode.

                    // Maybe a better approach here is to call an abstract method to be
                    // imeplemented by classes that derive from Task, as each would want
                    // to handle timeout's in different ways.
                }
            }

            this.Cleanup();
        }

        /// <summary>
        /// To be implemented by the derived class. When messages arrive from
        /// the queue they are passed to HandleMessage.
        /// </summary>
        /// <param name="message">The message to handle.</param>
        /// <returns>True if message was handled, false if message was not handled.</returns>
        private bool HandleMessage(InternalMessage message)
        {
            IAdapterTypes.IncomingResponseDelegate incomingResponseCallback;

            if(PopPendingRequest( message, out incomingResponseCallback))
            {
                incomingResponseCallback(message);
            }
            else
            {
                return false;
            }

            

            return true;
        }

        private void DefaultMessageHandler(InternalMessage im)
        {
        }

        private bool PopPendingRequest( InternalMessage im, out IAdapterTypes.IncomingResponseDelegate incomingResponseCallback)
        {
            incomingResponseCallback = null;

            string transactionId;
            im.GetFieldByName(IMmsProtocol.FIELD_MS_TRANSACTION_ID, out transactionId);


            if(PendingResponses.Contains(transactionId))
            {
                incomingResponseCallback = (IAdapterTypes.IncomingResponseDelegate) PendingResponses[transactionId];
                PendingResponses.Remove(transactionId);
                return true;
            }
            else
            {
                return false;
            }
            
        }

        private bool PushPendingRequest(InternalMessage im, IAdapterTypes.IncomingResponseDelegate incomingResponseCallback)
        {
            string transactionId;
            im.GetFieldByName(IMmsProtocol.FIELD_MS_TRANSACTION_ID, out transactionId);

            if(!PendingResponses.Contains(transactionId))
            {
                PendingResponses[transactionId] = incomingResponseCallback;
                return true;
            }
            else
            {
                log.Write(TraceLevel.Error, "A duplicate transactionId was sent to the MmsMqAdapter. Ignoring request to send.");
                return false;
            }
        }

        public override void SignalShutdown()
        {
            stopReceivingMessages = true;
        }

        // Unimplemented
        public override bool Restart(object mediaServerHandle)
        {
            return false;
        }
	}
}
