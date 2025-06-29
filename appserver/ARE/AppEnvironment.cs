using System;
using System.Threading;
using System.Diagnostics;

using Metreos.Configuration;
using Metreos.LoggingFramework;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Utilities;

namespace Metreos.AppServer.ARE
{
    public delegate void AppExceptionDelegate(string appName, UnhandledExceptionEventArgs e);

	public sealed class AppEnvironment : Loggable
	{
        public const string DisplayName = "ApplicationRuntimeEnvironment";

        public static AppMetaData AppMetaData;

        private Config configUtility;

        private MessageUtility messageUtility;
        private MessageQueueWriter routerQ;
        private Repository repository;
        private SchedulerTask schedulerTask;
        private bool initialized = false;

        public event AppExceptionDelegate UnhandledException;

		public AppEnvironment(Config configUtility)
            : base(TraceLevel.Warning, DisplayName)
		{
            this.configUtility = configUtility;

            TraceLevel level = (TraceLevel)configUtility.GetEntryValue(IConfig.ComponentType.Core, 
                IConfig.CoreComponentNames.ARE, IConfig.Entries.Names.LOG_LEVEL);
            base.SetLogLevel(level);

            messageUtility = new MessageUtility(IConfig.CoreComponentNames.ARE, IConfig.ComponentType.Core, null);
            
            repository = new Repository(log);
            schedulerTask = new SchedulerTask(repository, level);

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandler);
		}

        public void RefreshConfiguration()
        {
            // Refresh the scheduler task
            CommandMessage refreshMsg = 
                messageUtility.CreateCommandMessage(IConfig.CoreComponentNames.ARE, ICommands.REFRESH_CONFIG);

            schedulerTask.PostMessage(refreshMsg);
        }

        public bool Initialize(AppMetaData metaData, MessageQueueWriter routerQ, MessageQueueWriter telManQ, Logger logger)
        {
            Assertion.Check(metaData != null, "Cannot initialize app environment with null metadata");
            Assertion.Check(routerQ != null, "Cannot initialize app environment with null router queue");
            Assertion.Check(logger != null, "Cannot initialize app environment with null logger");

            AppEnvironment.AppMetaData = metaData;
            this.routerQ = routerQ;

            Trace.Listeners.Clear();
            Trace.Listeners.Add(logger);

            // Initialize repository
            MessageQueueWriter areQ = MessageQueueFactory.GetQueueWriter(schedulerTask.QueueId);
            if(!repository.Initialize(areQ, routerQ))
                return false;

            // Set the metadata on the SchedulerTask
            schedulerTask.Initialize(routerQ, telManQ);

            // Register scripts with the Router
            repository.RegisterMasterScripts();

            // Startup the scheduler task
            CommandMessage startupMsg = 
                messageUtility.CreateCommandMessage(IConfig.CoreComponentNames.ARE, ICommands.STARTUP);

            schedulerTask.PostMessage(startupMsg);

            // Wait for schedulerTask startup to complete
            schedulerTask.startupComplete.WaitOne();

            initialized = true;
            return true;
        }

		public void PostMessage(InternalMessage msg)
		{
			schedulerTask.PostMessage(msg);
		}

        public void Shutdown()
        {
            if(initialized == false) { return; }
            initialized = false;

            // Shutdown the scheduler task
            if(schedulerTask != null)
            {
                CommandMessage shutdownMsg = 
                    messageUtility.CreateCommandMessage(IConfig.CoreComponentNames.ARE, ICommands.SHUTDOWN);
                schedulerTask.PostMessage(shutdownMsg);

                if(schedulerTask.shutdownComplete.WaitOne(5000, false) == false)
                {
                    try
                    {
                        schedulerTask.ForceShutdown();
                    }
                    catch {}
                }
                schedulerTask = null;
            }

            // Give the thread pool another few milliseconds to terminate threads
            Thread.Sleep(100);

            // Clear the repository
            if(repository != null)
            {
                repository.Clear();
                repository = null;
            }
        }

        private void ExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ie = e.ExceptionObject as Exception;
            string exStr = Exceptions.FormatException(ie);

            if(ie is System.Threading.ThreadAbortException)
            {
                log.Write(TraceLevel.Verbose, exStr);
            }
            else if(ie is System.AppDomainUnloadedException)
            {
                log.Write(TraceLevel.Verbose, exStr);
            }
            else if(UnhandledException != null)
            {
                UnhandledException(AppMetaData.Name, e);
            }
        }

        public string GetDiagMessage()
        {
            return this.schedulerTask.GetDiagMessage();
        }
	}
}
