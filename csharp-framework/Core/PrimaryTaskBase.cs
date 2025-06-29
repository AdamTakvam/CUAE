using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;

using Metreos.LoggingFramework;
using Metreos.Messaging;
using Metreos.Interfaces;

namespace Metreos.Core
{
    #region PrimaryTaskBase Exceptions

    /// <summary>
    /// Indicates a failure of custom OnStartup() logic.
    /// </summary>
    public class StartupFailedException : ApplicationException
    {
        public StartupFailedException() : base("No reason given")
        {}

        public StartupFailedException(string message) : base(message)
        {}

        public StartupFailedException(string message, Exception inner) : base(message, inner)
        {}
    }

    /// <summary>
    /// Indicates a failure of custom OnShutdown() logic.
    /// </summary>
    public class ShutdownFailedException : ApplicationException
    {
        public ShutdownFailedException() : base("No reason given")
        {}

        public ShutdownFailedException(string message) : base(message)
        {}

        public ShutdownFailedException(string message, Exception inner) : base(message, inner)
        {}
    }

    #endregion

    /// <summary>
    /// Abstract base class for each of the primary Samoa components.
    /// Extends TaskBase and implements additional queue handling 
    /// logic as well as defining an interface for primary Samoa components.
    /// </summary>
    public abstract class PrimaryTaskBase : TaskBase
    {
        /// <summary>
        /// This primary task's message queue.
        /// </summary>
        protected MessageQueue taskQueue;
 
        /// <summary>
        /// Timeout used in calls to this task's queue's Receive() method.
        /// </summary>
        protected const int QUEUE_RECEIVE_TIMEOUT_SECS = 300;

        /// <summary>
        /// Timespan object representing QUEUE_TIMEOUT_SECS. Reused in Run().
        /// </summary>
        protected System.TimeSpan queueTimeout;

        /// <summary>
        /// Indicates whether to automatically signal this primary task's user defined
        /// Run() loop to exit, thus causing this task's thread to exit.
        /// </summary>
        protected bool autoSignalThreadShutdown = true;

        /// <summary>
        /// Event used to signal that the shutdown of this primary task is complete.
        /// </summary>
        protected AutoResetEvent taskShutdownComplete;

        /// <summary>
        /// Time, in seconds, that ForceShutdown() will wait for this primary
        /// task to shutdown.
        /// </summary>
        protected const int FORCE_SHUTDOWN_TIMEOUT_SECS = 5000;

        /// <summary>
        /// Indicates whether this primary task has been asked to shutdown its thread
        /// and exit.
        /// </summary>
        protected volatile bool shutdownRequested = false;

        /// <summary>
        /// Provides helper methods which create well-formed action/event/response messages.
        /// </summary>
        protected MessageUtility messageUtility;

        /// <summary>
        /// Interface for getting and setting configuration values
        /// </summary>
        protected IConfigUtility configUtility;

		/// <summary>
        /// Get the status of this primary task.
        /// </summary>
        public IConfig.Status TaskStatus
        {
            get { return configUtility.GetStatus(Type, Name); }
        }

        /// <summary>
        /// Get the queue ID of this primary task.
        /// </summary>
        public string QueueId
        {
            get { return this.Name; }
        }

        public PrimaryTaskBase(IConfig.ComponentType type, string taskName, string displayName, IConfigUtility config) 
            : this(type, taskName, displayName, TraceLevel.Verbose, false, config) {}

        public PrimaryTaskBase(IConfig.ComponentType type, string taskName, string displayName, TraceLevel logLevel, IConfigUtility config) 
            : this(type, taskName, displayName, logLevel, true, config) {}

        /// <summary>
        /// Primary constructor. During instantiation the task's thread will kick off
        /// and begin waiting for messages to arrive on the queue.
        /// </summary>
        /// <param name="taskName">The friendly name of this task.</param>
        public PrimaryTaskBase(IConfig.ComponentType type, string taskName, string displayName, TraceLevel logLevel, bool logLevelSpecified, IConfigUtility config) 
            : base(type, taskName, 
                displayName != null ? displayName : taskName, 
                logLevelSpecified ? logLevel : TraceLevel.Warning)
        {
            this.configUtility = config;

            if(logLevelSpecified == false)
            {
                try { base.log.LogLevel = GetLogLevel(); }
                catch {}
            }

            this.taskShutdownComplete = new AutoResetEvent(false);

            taskQueue = new MessageQueue(this.Name, configUtility.MessageQueueProvider, log);
            Debug.Assert(taskQueue != null, "Queue could not be created of type: " + configUtility.MessageQueueProvider);

            MessageQueueFactory.RegisterQueue(this.Name, taskQueue);

            // Make sure our queue is empty.
            taskQueue.Purge();                              

            // Get our timeout object ready. This is used repeatedly inside Run(), so
            // lets only build it once.
            this.queueTimeout = new System.TimeSpan(0, 0, 0, QUEUE_RECEIVE_TIMEOUT_SECS);

            if( type != IConfig.ComponentType.Provider && 
                Name != IConfig.CoreComponentNames.APP_SERVER) 
            {
                configUtility.UpdateStatus(Type, Name, IConfig.Status.Enabled_Stopped);
            }

            messageUtility = new MessageUtility(taskName, type, this.taskQueue.GetWriter());

            // Kick off thread execution.
            base.Start();
        }

        /// <summary>
        /// Cleanup this primary task's resources.
        /// </summary>
        /// <remarks>
        /// If the task has not been shutdown, the primary task will be forced to 
        /// shutdown.
        /// </remarks>
        public override void Dispose()
        {
            // This is bogus.
            if(this.TaskStatus == IConfig.Status.Enabled_Running)
            {
                this.ForceShutdown();
            }

            taskQueue.Dispose();

            base.Dispose();
        }

        /// <summary>
        /// Force this primary task to shutdown by sending it a MSG_SHUTDOWN
        /// and waiting a period of time, defined by FORCE_SHUTDOWN_TIMEOUT_SECS.
        /// If the task is not shutdown within that period of time, it will
        /// be aborted by the parent class' (Task) Cleanup() method.
        /// </summary>
        public void ForceShutdown()
        {
			this.autoSignalThreadShutdown = true;

            InternalMessage im = new CommandMessage();
            im.MessageId = ICommands.SHUTDOWN;

            this.PostMessage(im);

             // Block for 5 seconds while we shutdown.
            this.taskShutdownComplete.WaitOne(FORCE_SHUTDOWN_TIMEOUT_SECS, false);  
        }

        /// <summary>
        /// To be implemented by the derived class. Any global configuration vlues
        /// should be re-initialized here.
        /// </summary>
        protected abstract void RefreshConfiguration(string proxy);

        protected virtual TraceLevel GetLogLevel()
        {
            return TraceLevel.Warning;
        }

        /// <summary>
        /// To be implemented by the derived class. Any task specific startup logic
        /// should be implemented here.
        /// </summary>
        protected abstract void OnStartup();

        /// <summary>
        /// To be implemented by the derived class. Any task specific shutdown logic
        /// should be implemented here.
        /// </summary>
        protected abstract void OnShutdown();

        /// <summary>
        /// To be implemented by the derived class. When messages arrive from
        /// the queue they are passed to HandleMessage.
        /// </summary>
        /// <param name="message">The message to handle.</param>
        /// <returns>True if message was handled, false if message was not handled.</returns>
        protected abstract bool HandleMessage(InternalMessage message);

        /// <summary>
        /// Creates an object used to carry an event through the application server
        /// </summary>
        /// <param name="eventName">Fully-qualified name of the event</param>
        /// <returns>An InternalMessage-derived object which can be used in a PostMessage() method</returns>
        protected EventMessage CreateEventMessage(string eventName, EventMessage.EventType eventType, string routingGuid)
        {
            return messageUtility.CreateEventMessage(eventName, eventType, routingGuid);
        }

        /// <summary>
        /// For internal use only
        /// </summary>
        protected CommandMessage CreateCommandMessage(string to, string command)
        {
            return messageUtility.CreateCommandMessage(to, command);
        }

        /// <summary>
        /// Empty the queue associated with this task.
        /// </summary>
        public void FlushQueue()
        {
            this.taskQueue.Purge();
        }


        /// <summary>
        /// Post a message to this primary task.
        /// </summary>
        /// <param name="im">InternalMessage containing the message data to post.</param>
        public void PostMessage(InternalMessage im)
        {
            taskQueue.Send(im);
        }


        /// <summary>
        /// Primary loop for the task. Essentially just receive a message, handle it,
        /// and repeat. This method will exit when a message with MessageId "Task.Shutdown"
        /// arrives in the queue.
        /// </summary>
        protected override void Run()
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
                    if(this.HandleMessage(im) == false)
                    {
                        this.DefaultMessageHandler(im);
                    }
                }
            }

            this.taskShutdownComplete.Set();
        }


        #region Message Handling Logic

        /// <summary>
        /// Default message handler. This handler is invoked if the derived
        /// classes HandleMessage method returns false.
        /// </summary>
        /// <param name="im">The message that was received.</param>
        private void DefaultMessageHandler(InternalMessage im)
        {
            DebugLog.MethodEnter();

            CommandMessage cmdMsg = im as CommandMessage;
            if(cmdMsg != null)
            {
                switch(im.MessageId)
                {
                    case ICommands.STARTUP:   
                        base.RefreshConfiguration(GetLogLevel());
                        this.HandleStartupMessage(cmdMsg);          
                        break;

                    case ICommands.SHUTDOWN:                          
                        this.HandleShutdownMessage(cmdMsg);
                        break;

                    case ICommands.SET_LOG_LEVEL:
                        this.HandleSetLogLevelMessage(cmdMsg);
                        break;

                    case ICommands.REFRESH_CONFIG:
                        base.RefreshConfiguration(GetLogLevel());
                        this.RefreshConfiguration(cmdMsg.Destination != Name ? cmdMsg.Destination : null);
                        cmdMsg.SendResponse(IResponses.REFRESH_COMPLETE);
                        break;
                }
            }

            DebugLog.MethodExit();
        }
        

        /// <summary>
        /// Start the task.
        /// </summary>
        /// <param name="im">The startup message.</param>
        private void HandleStartupMessage(CommandMessage im)
        {
            DebugLog.MethodEnter();

            try
            {
                this.OnStartup();

                configUtility.UpdateStatus(Type, Name, IConfig.Status.Enabled_Running);

                if(im.Source != this.Name)
                {
                    im.SendResponse(IResponses.STARTUP_COMPLETE);
                }
            }
            catch(StartupFailedException e)
            {
                log.Write(TraceLevel.Warning, "Startup failed. Exception is: \n" + e.ToString() + "\n");

                if(im.Source != this.Name)
                {
                    ArrayList responseFields = new ArrayList();
                    responseFields.Add(new Field(ICommands.Fields.FAIL_REASON, e.Message));
                    im.SendResponse(IResponses.STARTUP_FAILED);
                }
            }

            DebugLog.MethodExit();
        }


        private void HandleShutdownMessage(CommandMessage im)
        {
            DebugLog.MethodEnter();

            try
            {
                configUtility.UpdateStatus(Type, Name, IConfig.Status.Enabled_Stopped);

                this.OnShutdown();

                if(autoSignalThreadShutdown)
                {
                    shutdownRequested = true;
                }

                if(im.Source != this.Name)
                {
                    im.SendResponse(IResponses.SHUTDOWN_COMPLETE);
                }
            }
            catch(ShutdownFailedException e)
            {
                log.Write(TraceLevel.Warning, "Shutdown failed. Exception is: \n" + e.ToString() + "\n");

                if(im.Source != this.Name)
                {
                    ArrayList responseFields = new ArrayList();
                    responseFields.Add(new Field(ICommands.Fields.FAIL_REASON, e.Message));
                    im.SendResponse(IResponses.SHUTDOWN_FAILED);
                }
            }

            DebugLog.MethodExit();
        }


        private void HandleSetLogLevelMessage(InternalMessage im)
        {
            DebugLog.MethodEnter();

            string newLogLevel = im[ICommands.Fields.LOG_LEVEL] as string;

            if(newLogLevel == null)
            {
                log.Write(TraceLevel.Info, "SetLogLevel from " + im.Source + " failed because " + ICommands.Fields.LOG_LEVEL + 
                    " was not present in the message.");
                DebugLog.MethodExit();
                return;
            }

            try
            {
                log.LogLevel = (TraceLevel)System.Enum.Parse(typeof(TraceLevel), newLogLevel, true);
            }
            catch(ArgumentException)
            {
                log.Write(TraceLevel.Info, "SetLogLevel from " + im.Source + " failed. An ArgumentException was thrown " +
                                            "by System.Enum.Parse(). Verify the validity of the new log level.");
            }

            DebugLog.MethodExit();
        }
        
        #endregion
    }
}
