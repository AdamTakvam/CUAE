using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;

using Metreos.Messaging;

namespace TaskQueueTest
{
    public abstract class PrimaryTaskBase : MarshalByRefObject, IDisposable
    {
        private Thread taskThread;
        protected MessageQueue taskQueue;
        protected const int QUEUE_RECEIVE_TIMEOUT_SECS = 300;

        protected System.TimeSpan queueTimeout;

        protected bool autoSignalThreadShutdown = true;

        protected AutoResetEvent taskShutdownComplete;

        protected const int FORCE_SHUTDOWN_TIMEOUT_SECS = 5000;

        protected volatile bool shutdownRequested = false;

        public PrimaryTaskBase(string name) 
        {
            this.taskShutdownComplete = new AutoResetEvent(false);

            this.taskQueue = new MessageQueue();

            this.queueTimeout = new System.TimeSpan(0, 0, 0, QUEUE_RECEIVE_TIMEOUT_SECS);

            taskThread = new Thread(new ThreadStart(Run));
            taskThread.IsBackground = true;
            taskThread.Name = name + " message pump";
            taskThread.Start();
        }

        public virtual void Dispose()
        {
            taskQueue.Dispose();
        }

        protected abstract bool HandleMessage(InternalMessage message);

        protected void Run()
        {
            InternalMessage im;
            bool messageReceived;

            int threadId = Thread.CurrentThread.ManagedThreadId;

            while(shutdownRequested == false)
            {
                // Receive a message from our queue. Block until a message arrives or
                // a timeout occurs.
                messageReceived = taskQueue.Receive(queueTimeout, out im);

                if(messageReceived && im != null)
                {
                    this.HandleMessage(im);
                }
            }

            this.taskShutdownComplete.Set();
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
