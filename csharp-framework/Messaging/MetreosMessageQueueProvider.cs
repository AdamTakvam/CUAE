using System;
using System.Configuration;
using System.Collections;
using System.Diagnostics;
using System.Threading;

using Metreos.Interfaces;
using Metreos.LoggingFramework;

namespace Metreos.Messaging
{
    /// <summary>
    /// A message queue provider that uses a System.Collections.Queue as its data store.
    /// </summary>
    public class MetreosMessageQueueProvider : IMessageQueueProvider, IDisposable
    {
        /// <summary>Number of messages in our queue before we start output warnings.</summary>
        private readonly int queueThreshold;

        private readonly Queue queue;
        private readonly ManualResetEvent messageReady;
        private readonly LogWriter log;

        public MetreosMessageQueueProvider(LogWriter log)
        {
            this.log = log;
            this.queue = new Queue();   // Note, queue is not synchronized because we lock on SyncRoot when we access it
            this.messageReady = new ManualResetEvent(false);
            queueThreshold = 25; // Default
            try
            {
                queueThreshold = Int32.Parse(ConfigurationManager.AppSettings.Get(IConfig.ConfigFileSettings.MQ_HIGHWATER_WARN_THRESHOLD));
            }
            catch { };
        }


        // Place a message onto this queue
        public void Send(InternalMessage message)
        {
            long enter, enqueue, set;

            enter = Metreos.Utilities.HPTimer.Now();
            lock(queue.SyncRoot)
            {
                enter = Metreos.Utilities.HPTimer.MillisSince(enter);

                enqueue = Metreos.Utilities.HPTimer.Now();
                queue.Enqueue(message);
                enqueue = Metreos.Utilities.HPTimer.MillisSince(enqueue);

                if(queue.Count > queueThreshold)
                    log.Write(TraceLevel.Warning, "DIAG: Queue High-Water Alert: count={0}, msg={1}, src={2}", 
                        queue.Count, message.MessageId, message.Source);
            }

            set = Metreos.Utilities.HPTimer.Now();
            messageReady.Set();
            set = Metreos.Utilities.HPTimer.MillisSince(set);

            if(enter > 32 || enqueue > 32 || set > 32)
                log.Write(TraceLevel.Warning, "DIAG: Send: enter={0}, enqueue={1}, set={2}, msg={3}, src={4}", 
                    enter, enqueue, set, message.MessageId, message.Source);
        }


        // Receive a message from this queue
        public bool Receive(System.TimeSpan timeout, out InternalMessage message)
        {
            long enter, dequeue, reset;

            message = null;
            bool gotMessage = messageReady.WaitOne(timeout, false);

            if(gotMessage == true)
            {
                enter = Metreos.Utilities.HPTimer.Now();
                lock(queue.SyncRoot)
                {
                    enter = Metreos.Utilities.HPTimer.MillisSince(enter);

                    dequeue = Metreos.Utilities.HPTimer.Now();
                    if(queue.Count != 0) // Check for possible race condition with Purge()
                        message = (InternalMessage) queue.Dequeue();
                    dequeue = Metreos.Utilities.HPTimer.MillisSince(dequeue);

                    reset = Metreos.Utilities.HPTimer.Now();
                    if(queue.Count == 0) // No more messages? Then reset messageReady.
                        messageReady.Reset();
                    reset = Metreos.Utilities.HPTimer.MillisSince(reset);
                }

                if(enter > 32 || dequeue > 32 || reset > 32)
                    log.Write(TraceLevel.Warning, "DIAG: Receive: enter={0}, dequeue={1}, reset={2}, msg={3}, src={4}",
                        enter, dequeue, reset,
                        message != null ? message.MessageId : "None",
                        message != null ? message.Source    : "None");
            }

            return message == null ? false : true;
        }


        public string GetQueueId()
        {
            return String.Empty;
        }


        public int GetQueueLength()
        {
            int length = 0;
            lock(queue.SyncRoot)
            {
                length = queue.Count;
            }
            return length;
        }


        public void Purge()
        {
            lock(queue.SyncRoot)
            {
                queue.Clear();
                messageReady.Reset(); // Reset messageReady inside of the lock to make sure we don't have a race w/ Send
            }
        }


        public void ReleaseResources()
        {}


        public void Delete()
        {}


        public MessageQueueWriter GetMessageQueueWriter()
        {
            return new MessageQueueWriter(this, log);
        }


        public void Dispose()
        {
            Purge();
        }
	}
}
