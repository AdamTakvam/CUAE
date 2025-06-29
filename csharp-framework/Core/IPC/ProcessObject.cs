using System;
using System.Collections;
using System.Threading;

namespace Metreos.Core.IPC
{
	public delegate void ProcessObjectDelegate(Object obj);

    /// <summary>
    /// This class runs its own, separate, persistent thread to process
    /// objects in series, not in parallel.
    /// </summary>
    public class ProcessObject
    {
        public ProcessObjectDelegate processObject;

        /// <summary>
        /// Queue that holds objects that delegate processes.
        /// </summary>
        Queue queue;

        /// <summary>
        /// The thread within which the object is processed.
        /// </summary>
        private Thread thread;

        /// <summary>
        /// Shutdown flag
        /// </summary>
        private volatile bool shutdown;

        private AutoResetEvent newObject;

		/// <summary>
		/// Maximum number of objects to process in a single burst.
		/// </summary>
		private volatile int maxBurst;

		/// <summary>
		/// Maximum number of objects to process in a single burst.
		/// </summary>
		/// <remarks>Consumer can change this at any time.</remarks>
		public int MaxBurst { set { maxBurst = value; } }

		/// <summary>
		/// Delay between bursts.
		/// </summary>
		private volatile int interBurstDelayMs;

		/// <summary>
		/// Delay between bursts.
		/// </summary>
		/// <remarks>Consumer can change this at any time.</remarks>
		public int InterBurstDelayMs { set { interBurstDelayMs = value; } }

        /// <summary>
        /// Constructor for processing objects, defaulting to not breaking
        /// up into bursts.
        /// </summary>
        public ProcessObject() : this(int.MaxValue, 0) { }

		/// <summary>
		/// Constructor for processing objects.
		/// </summary>
		/// <param name="maxBurst">Maximum number of objects to process in
		/// a single burst.</param>
		/// <param name="interBurstDelayMs">Delay between bursts.</param>
		public ProcessObject(int maxBurst, int interBurstDelayMs)
		{
			queue = new Queue();
			thread = new Thread(new ThreadStart(ProcessObjectThread));
			thread.Name = "IpcClient.ProcessObject";
			thread.IsBackground = true;
			shutdown = false;
			processObject = null;
			newObject = new AutoResetEvent(false);

			this.maxBurst = maxBurst;
			this.interBurstDelayMs = interBurstDelayMs;
		}

        /// <summary>
        /// If not already running, start thread.
        /// </summary>
        public void Start()
        {
            if (!thread.IsAlive)
            {
                thread.Start();
            }
        }

        /// <summary>
        /// Stop thread if running.
        /// </summary>
        /// <remarks>The thread should terminate either due to the
        /// shutdown flag being set or the connection being closed.</remarks>
        public void Stop()
        {
            // Tell thread to terminate and give it a chance to do so
            shutdown = true;
            newObject.Set();

            // If still not dead, kill it.
            if (this.thread.IsAlive && !this.thread.Join(ThreadTimeoutMSecs))
            {
                // Try real hard to terminate the thread right now.
                this.thread.Abort();
            }

            queue.Clear();	// Remove any objects we didn't have a chance to process.
        }

		public const int ThreadTimeoutMSecs = 250;

        /// <summary>
        /// Submit an object for processing.
        /// </summary>
        /// <param name="obj">Object to process.</param>
        /// <returns>
        /// Whether the object was successfully submitted for processing,
        /// e.g., because thread is still active.
        /// </returns>
        public bool Submit(Object obj)
        {
            bool submitted = false;

            if (thread.IsAlive)
            {
                Enqueue(obj);
                newObject.Set();
                submitted = true;
            }

            return submitted;
        }

        /// <summary>
        /// Thread to process objects as they are submitted.
        /// </summary>
        /// <remarks>
        /// Queue was necessary because otherwise we had submissions that
        /// came in before we process them--we couldn't keep up. We
        /// shouldn't have to have a queue but couldn't figure out why we
        /// weren't keeping up, so we used this work-around.
        /// </remarks>
        private void ProcessObjectThread()
        {
            do
            {
                // (I moved this processing loop to _before_ the Wait()
                // in case objects were Submit()ed before thread got
                // started.)
                Object obj;
				int i = maxBurst;
                while (Dequeue(out obj))
                {
                    // Make sure consumer provided callback(s).
                    if (processObject != null)
                    {
                        processObject(obj);
                    }

					if (--i <= 0)
					{
						Thread.Sleep(interBurstDelayMs);
						i = maxBurst;
					}

					if (shutdown)
					{
						break;
					}
				}

                newObject.WaitOne();
            }
            while (!shutdown);
        }

        /// <summary>
        /// Enqueue object onto object queue with thread safety.
        /// </summary>
        /// <param name="obj">Object to enqueue.</param>
        private void Enqueue(Object obj)
        {
            lock (queue)
            {
                queue.Enqueue(obj);
            }
        }

        /// <summary>
        /// Dequeue an object from the object queue with thread safety.
        /// </summary>
        /// <remarks>
        /// A separate return value is used to indicate whether an object
        /// was dequeued rather than having object = null to indicate this
        /// because a valid object on the queue could in fact be null.
        /// </remarks>
        /// <param name="obj">Dequeued object.</param>
        /// <returns>Whether an object was dequeued.</returns>
        private bool Dequeue(out Object obj)
        {
            bool dequeued;

            lock (queue)
            {
                if (queue.Count > 0)
                {
                    obj = queue.Dequeue();
                    dequeued = true;
                }
                else
                {
                    obj = null;
                    dequeued = false;
                }
            }

            return dequeued;
        }
    }
}
