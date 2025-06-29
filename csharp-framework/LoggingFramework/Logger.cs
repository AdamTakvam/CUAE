using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace Metreos.LoggingFramework
{
    public delegate void LoggerWriteDelegate(DateTime timeStamp, TraceLevel errorLevel, string message);

    public delegate void LoggerRefreshDelegate();

    public sealed class Logger : DefaultTraceListener, IDisposable
    {   
        private const string MSG_LOGGER_SHUTDOWN = "Logger.Shutdown";

        private readonly Queue syncQueue;
        private readonly Thread thread;

        private bool diagLoggerQueue = false;
        public bool DiagLoggerQueue
        {
            set { diagLoggerQueue = value; }
            get { return diagLoggerQueue;  }
        }
        
        private readonly AutoResetEvent messagesWaiting;

        private volatile bool shutdownRequested = false;

        private event LoggerRefreshDelegate refreshLoggerSinks;

        public event LoggerWriteDelegate verboseMessageSink;
        public event LoggerWriteDelegate infoMessageSink;
        public event LoggerWriteDelegate warningMessageSink;
        public event LoggerWriteDelegate errorMessageSink;

        #region Singleton interface
        
        private static volatile Logger instance = null;
        private static Object newInstanceSync = new Object();

        public static Logger Instance
        {
            get
            {
                if(instance == null)                                // Have we already initialized?
                {
                    lock(newInstanceSync)                           // Grab the instance lock
                    {
                        if(instance == null)                        // Verify one more time
                        {
                            instance = new Logger();                // Create the singleton instance
                        }
                    }
                }

                return instance;
            }
        }

        #endregion

        private class LogMessage
        {
			public DateTime timeStamp;
            public TraceLevel errorLevel;
            public string message;

            public LogMessage(TraceLevel errorLevel, string message)
            {
				this.timeStamp = DateTime.Now;
                this.errorLevel = errorLevel;
                this.message = message;
            }
        }


        private Logger()
        {
            Trace.Listeners.Clear();
            Debug.Listeners.Clear();
            Trace.Listeners.Add(this);

            messagesWaiting = new AutoResetEvent(false);

            syncQueue = Queue.Synchronized(new Queue());

            thread = new Thread(new ThreadStart(this.Run));
            thread.IsBackground = true;
            thread.Name = "Logger";
            thread.Start();
        }


        public void RegisterLoggerSink(LoggerSinkBase sink)
        {
            refreshLoggerSinks += new LoggerRefreshDelegate(sink.RefreshConfiguration);
        }

        public void UnregisterLoggerSink(LoggerSinkBase sink)
        {
            refreshLoggerSinks -= new LoggerRefreshDelegate(sink.RefreshConfiguration);
        }

        public void RefreshConfiguration()
        {
			lock (deliverSync)
			{
				verboseMessageSink = null;
				infoMessageSink = null;
				warningMessageSink = null;
				errorMessageSink = null;

				if(refreshLoggerSinks != null)
				{
					refreshLoggerSinks();
				}
			}
        }

        public new void Dispose()
        {
            this.Close();

            if(thread.IsAlive)
            {
                // Post the logger thread a shutdown message.
                this.Write(MSG_LOGGER_SHUTDOWN, TraceLevel.Off.ToString());

                if(thread.Join(250) == false)
                    thread.Abort();
            }

			this.refreshLoggerSinks = null;
            this.verboseMessageSink = null;
            this.infoMessageSink = null;
            this.warningMessageSink = null;
            this.errorMessageSink = null;

            Logger.instance = null;

            base.Dispose();
        }

        private void Run()
        {            
            LogMessage logMessage;

            syncQueue.Enqueue(new LogMessage(TraceLevel.Info, "Logger thread beginning"));

            while(shutdownRequested == false)
            {
                messagesWaiting.WaitOne();

                while(syncQueue.Count > 0)
                {
                    logMessage = syncQueue.Dequeue() as LogMessage;
					if(logMessage == null)
						continue;
                    
                    if(diagLoggerQueue)
                    {
                        logMessage.message += String.Format(":{0}:{1}",
                            syncQueue.Count,
                            GC.GetGeneration(logMessage));
                    }

					lock (deliverSync)
					{
						switch(logMessage.errorLevel)
						{
							case TraceLevel.Verbose:
								if(verboseMessageSink != null)
								{
									verboseMessageSink(logMessage.timeStamp, logMessage.errorLevel, logMessage.message);
								}
								break;

							case TraceLevel.Info:
								if(infoMessageSink != null)
								{
									infoMessageSink(logMessage.timeStamp, logMessage.errorLevel, logMessage.message);
								}
								break;

							case TraceLevel.Warning:
								if(warningMessageSink != null)
								{
									warningMessageSink(logMessage.timeStamp, logMessage.errorLevel, logMessage.message);
								}
								break;

							case TraceLevel.Error:
								if(errorMessageSink != null)
								{
									errorMessageSink(logMessage.timeStamp, logMessage.errorLevel, logMessage.message);
								}
								break;

							case TraceLevel.Off:
								if(logMessage.message == MSG_LOGGER_SHUTDOWN)
								{
									syncQueue.Enqueue(new LogMessage(TraceLevel.Info, "Logger thread exiting"));
									messagesWaiting.Set();
									shutdownRequested = true;
								}
								break;
						}
					}

                    logMessage = null;
                }
            }
        }

		private readonly object deliverSync = new object();

        /// <summary>
        /// Make sure this object has an infinite lifetime in terms of the remote garbage
        /// collector.
        /// </summary>
        /// <returns>Always returns null.</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }

        #region TraceListener implementation

        public override void Write(string message, string category)
        {
            TraceLevel errorLevel;

            try
            {
                errorLevel = (TraceLevel)System.Enum.Parse(typeof(TraceLevel), category, true);
            }
            catch(ArgumentException)
            {
                errorLevel = TraceLevel.Error;
                syncQueue.Enqueue(new LogMessage(errorLevel, "Logger: Could not parse error level: " + category));
            }

            syncQueue.Enqueue(new LogMessage(errorLevel, message));

            messagesWaiting.Set();
        }


        public override void Write(object message, string category)
        {
            Write(message.ToString(), category);
        }


        public override void Write(object message)
        {
            syncQueue.Enqueue(new LogMessage(TraceLevel.Info, message.ToString()));
        }


        public override void Write(string message)
        {
            syncQueue.Enqueue(new LogMessage(TraceLevel.Info, message));
        }


        public override void WriteLine(string message)
        {
            Write(message);
        }


        public override void WriteLine(string message, string category)
        {
            Write(message, category);
        }


        public override void WriteLine(object message, string category)
        {
            Write(message, category);
        }


        public override void WriteLine(object message)
        {
            Write(message);
        }


        public override void Fail(string message)
        {
            base.Fail (message);
        }


        public override void Fail(string message, string detailMessage)
        {
            base.Fail (message, detailMessage);
        }


        #endregion
    }
}
