using System;
using System.Net;
using System.Messaging;
using System.Threading;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;

using Utils=Metreos.Utilities;

namespace Metreos.MediaControl
{
    public class MediaServerInfoMSMQ : MediaServerInfo
    {
        private abstract class Consts
        {
            // MSMQ stuff
            public const string MediaServerQueueName    = "Private$\\metreos-mediaserver";
            public const string ReceiveQueueIdPrefix    = "Metreos-MediaServerManager-";
            public const string ReceiveQueuePathPrefix  = ".\\Private$\\";

            // Threadpool stuff
            public const int NumThreads         = 3;
            public const int MaxThreads         = 10;
            public const string PoolName        = "Media Control ThreadPool";
        }

        /// <summary>
        /// The queue that the remote media server is listening on.
        /// </summary>
        public MessageQueue MediaServerTransmitQueue { get { return mediaServerTransmitQueue; } }
        private MessageQueue mediaServerTransmitQueue;

        /// <summary>Serializes incoming MSMQ responses</summary>
        internal Utils.QueueProcessor qProcessor;

        /// <summary>Delegate for dispatching messages to threadpool</summary>
        internal static Utils.QueueProcessorDelegate qpDelegate;

        /// <summary>Name portion of receive queue path</summary>
        private static string receiveQueueId;

        /// <summary>The receive queue path of the media server manager.</summary>
        private static string receiveQueuePath;

        /// <summary>Thread for reading from our MSMQ message queue.</summary>
        private static Thread receiveQueueReadThread;

        /// <summary>The receive queue that the media server is talking to.</summary>
        private static MessageQueue receiveQueue;

        /// <summary>Pool of threads to handle incoming MSMQ messages</summary>
        private static Metreos.Utilities.ThreadPool threadPool;

        /// <summary>Lets the MSMQ receive thread exit gracefully</summary>
        private static volatile bool shuttingDown = false;

        /// <summary>MSMQ media server info constructor</summary>
        /// <remarks>Always invoke this in a try/catch!</remarks>
        public MediaServerInfoMSMQ(string name, uint id, IPAddress addr, LogWriter log)
            : base(name, id, addr, log)
        {
            // Make sure static receive queue is created and thread is running
            StartThreadPool();

            if(qpDelegate == null)
                qpDelegate = new Utils.QueueProcessorDelegate(QueueProcessorCallback);

            this.qProcessor = new Utils.QueueProcessor(
                new Utils.QueueProcessorExceptionDelegate(QueueProcessorException), threadPool);

            CreateReceiveQueue();
            StartReceiveThread();

            // Create transmit queue
            string fullTxQueueName = String.Format("FormatName:DIRECT=TCP:{0}\\{1}", 
                addr.ToString(), Consts.MediaServerQueueName);

            mediaServerTransmitQueue = new MessageQueue(fullTxQueueName);
        }

        private void QueueProcessorException(Utils.QueueProcessor qp, object data, Exception e)
        {
            log.Write(TraceLevel.Error, "Exception caught while processing MMS response: {0}", e.Message);
        }

        /// <summary>Shim for HandleMediaServerMessageDelegate</summary>
        internal static void QueueProcessorCallback(Utils.QueueProcessor qp, object data)
        {
            object[] objs = data as Object[];
            if(objs == null && objs.Length != 2)
                return;

            uint serverId = Convert.ToUInt32(objs[0]);
            MediaServerMessage msg = objs[1] as MediaServerMessage;

            handleMediaServerMessage(serverId, msg);
        }

        #region Static receive thread

        private static void StartReceiveThread()
        {
            // There's only one receive thread/queue for all media server connections
            if(receiveQueueReadThread != null && receiveQueueReadThread.IsAlive)
                return;

            receiveQueueReadThread = new Thread(new ThreadStart(ReceiveQueueReadThread));
            receiveQueueReadThread.Name = "MSM Receive Queue Read Thread";
            receiveQueueReadThread.IsBackground = true;
            receiveQueueReadThread.Start();
        }

        /// <summary>
        /// Executes a loop to receive messages from the MSMQ receive queue.
        /// </summary>
        /// <remarks>The thread will exit if shuttingDown == true.  To kick the
        /// receive out of its blocking state, remove the message queue from 
        /// the system.</remarks>
        private static void ReceiveQueueReadThread()
        {
            Message msmqMsg = null;
            MediaServerMessage msg;

            if (receiveQueue == null)
            {
                Debug.Fail("Receive Queue is invalid, MSMQ must start before App Server.");
                return;
            }

            //Console.WriteLine("MSMQ Receive thread beginning");

            while(shuttingDown == false)
            {
                try
                {
                    msmqMsg = receiveQueue.Receive();
                    Assertion.Check(msmqMsg.Body is MediaServerMessage, "Received message body is not a media server message");
                    msg = msmqMsg.Body as MediaServerMessage;

                    HandleMediaServerMessageAsync(msg);
                }
                catch//(Exception e)
                {
                    //Console.WriteLine("ReceiveQueueReadThread caught: " + e.Message);
                }
                finally
                {
                    if(msmqMsg != null)
                    {
                        msmqMsg.Dispose();
                        msmqMsg = null;
                    }
                }
            }

            //Console.WriteLine("MSMQ Receive thread exiting");
        }

        /// <summary>Uses the threadpool to handle media server messages</summary>
        /// <param name="msg">The media server message to handle.</param>
        private void HandleMediaServerMessageAsync(MediaServerMessage msg)
        {
            // Try hard to determine server ID
            // Note: IPC doesn't have this problem  :-/
            uint serverId = 0;
            if(msg.GetUInt32(IMediaServer.Fields.ServerId, out serverId) == false)
            {
                uint connId = 0;
                if(msg.GetUInt32(IMediaServer.Fields.ConnectionId, out connId))
                {
                    serverId = IMediaControl.GetMmsId(connId);
                }
            }

            MediaServerInfoMSMQ msInfo = MediaServerManager.mediaServers[serverId] as MediaServerInfoMSMQ;
            if(msInfo == null)
                return;

            if(msg != null && handleMediaServerMessage != null)
                msInfo.qProcessor.Enqueue(qpDelegate, new object[] { serverId, msg });
        }

        private static void StartThreadPool()
        {
            // We only do this once
            if(threadPool != null)
                return;

            threadPool = new Metreos.Utilities.ThreadPool(Consts.NumThreads, Consts.MaxThreads, Consts.PoolName);
            threadPool.Start();
        }

        /// <summary>Creates the MSMQ receive message queue.</summary>
        /// <remarks>If the queue exists, it is removed and re-created.</remarks>
        private static void CreateReceiveQueue()
        {
            // There's only one receive thread/queue for all media server connections
            if(receiveQueue != null)
                return;

            if(receiveQueuePath == null)
            {
                receiveQueueId = Consts.ReceiveQueueIdPrefix + Environment.MachineName;
                receiveQueuePath = Consts.ReceiveQueuePathPrefix + receiveQueueId;
            }

            if(MessageQueue.Exists(receiveQueuePath) == true)
            {
                receiveQueue = new MessageQueue(receiveQueuePath);
                receiveQueue.Purge();
            }
            else
            {
                receiveQueue = MessageQueue.Create(receiveQueuePath);
            }

            Assertion.Check(receiveQueue != null, "receiveQueue is not null in CreateReceiveQueue()");
            receiveQueue.Label = "Metreos Media Server Manager Receive Queue";
            receiveQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(MediaServerMessage) });
        }

        /// <summary>Purge and remove the MSMQ receive message queue.</summary>
        private static void RemoveReceiveQueue()
        {
            if(receiveQueue == null) { return; }

            try
            {
                receiveQueue.Purge();                         
                receiveQueue.Close();
                receiveQueue.Dispose();

                if(MessageQueue.Exists(receiveQueue.Path))
                {
                    // Delete the queue from MSMQ
                    MessageQueue.Delete(receiveQueue.Path);  
                }

                MessageQueue.ClearConnectionCache();
            }
            catch {}
        }

        public static void Shutdown()
        {
            shuttingDown = true;

            if(receiveQueueReadThread != null)
            {
                RemoveReceiveQueue();

                if(receiveQueueReadThread.Join(500) == false)
                    receiveQueueReadThread.Abort();
                
                receiveQueueReadThread = null;
            }

            if(threadPool != null)
            {
                threadPool.Stop();
                threadPool = null;
            }
        }

        #endregion

        /// <summary>Sends a message to the media server</summary>
        /// <param name="commandMsg">Message to send</param>
        /// <param name="isInitConnect">Is this an initial connect attempt?</param>
        /// <remarks>Watch for exceptions!</remarks>
        public override void SendCommand(MediaServerMessage commandMsg, bool isInitConnect)
        {
            if(isInitConnect)
            {
                commandMsg.AddField(IMediaServer.Fields.MachineName, Environment.MachineName);
                commandMsg.AddField(IMediaServer.Fields.QueueName, receiveQueueId);
            }

            lock(this.mediaServerTransmitQueue)
            {
                this.mediaServerTransmitQueue.Send(commandMsg);
            }
        }

        public override void Dispose()
        {
            if(this.mediaServerTransmitQueue != null)
            {
                this.mediaServerTransmitQueue.Close();
                this.mediaServerTransmitQueue.Dispose();
                this.mediaServerTransmitQueue = null;
            }

            if(qProcessor != null)
            {
                this.qProcessor.Stop(true);
                this.qProcessor = null;
            }
        }
    }
}
