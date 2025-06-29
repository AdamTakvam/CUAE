using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Core;
using Metreos.Stats;
using Metreos.LogSinks;
using Metreos.Utilities;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.Utilities.Collections;

using Metreos.AppServer.Clustering;
using Metreos.AppServer.Management;
using Metreos.AppServer.EventRouter;
using Metreos.Configuration;
using Metreos.AppServer.TelephonyManager;
using Metreos.AppServer.ApplicationManager;

namespace Metreos.AppServer.CommonRuntime
{
    /// <summary>
    /// Invoked after the application server has completed its startup procedures.
    /// </summary>
    public delegate void StartupCompleteDelegate();

    // REFACTOR: Modify this to send a IConfig.CoreComponentNames and IConfig.Status
    public delegate void StartupProgressDelegate(string progressMessage);

    /// <summary>
    /// Invoked after the application server has completed its shutdown procedures.
    /// </summary>
    public delegate void ShutdownCompleteDelegate();

    // REFACTOR: Modify this to send a IConfig.CoreComponentNames and IConfig.Status
    public delegate void ShutdownProgressDelegate(string progressMessage);

    /// <summary>
    /// ApplicationServer is the bootloader for the process.
    /// It is intended that any number of runtime facades will use this
    /// class to implement a runtime for the server.
    /// </summary>
    /// 
    /// <remarks>
    /// ApplicationServer is a thread-safe singleton. Only one may exist at any
    /// time. Users receive an instance to ApplicationServer through the
    /// Instance property.
    /// 
    /// ApplicationServer will create instances of every primary core 
    /// component, initialize them, and manage their execution.
    /// Startup, shutdown, etc is all managed by ApplicationServer.
    /// </remarks> 
    /// 
    /// <example>
    ///     <code>
    ///     ApplicationServer appServer = Metreos.AppServer.CommonRuntime.ApplicationServer.Instance;
    ///     appServer.Start();
    ///     ... do something useful ...
    ///     appServer.Shutdown();
    ///     </code>
    /// </example>
    public sealed class ApplicationServer : PrimaryTaskBase
    {
        private const int STARTUP_TIMEOUT   = 240000;
        private const int SHUTDOWN_TIMEOUT  = 30000;
        private const string ServiceName    = "AppServer";

        private readonly ManualResetEvent started;
        private readonly ManualResetEvent shutdown;

        private readonly Logger logger;
        private readonly LogSinkCollection logSinks;

        private readonly new Config configUtility;
        private readonly StatsClient statsClient;

        private readonly Router router;
        private readonly AppManager appManager;
        private readonly MediaManager mediaManager;
        private readonly TelManager telManager;
        private readonly ManagementInterface management;
        private readonly ClusterInterface cluster;

        private readonly TallyCollection localTaskStatus;

        public event StartupCompleteDelegate startupComplete;
        public event ShutdownCompleteDelegate shutdownComplete;

        public event StartupProgressDelegate startupProgress;
        public event ShutdownProgressDelegate shutdownProgress;

        #region Singleton interface
        
        private static volatile ApplicationServer instance = null;
        private static Object newInstanceSync = new Object();

        public static ApplicationServer Instance
        {
            get
            {
                if(instance == null)                                // Have we already initialized?
                {
                    lock(newInstanceSync)                           // Grab the instance lock
                    {
                        if(instance == null)                        // Verify one more time
                        {                                           // Create the singleton instance
                            instance = new ApplicationServer();
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Private constructor. Used for the singleton implementation.
        /// </summary>
        private ApplicationServer() 
            : base(IConfig.ComponentType.Core, 
            IConfig.CoreComponentNames.APP_SERVER,
            IConfig.CoreComponentNames.APP_SERVER,
            TraceLevel.Info,
            Config.Instance)
        {
            this.configUtility = Config.Instance;
            this.statsClient = StatsClient.Instance;

            if(!this.configUtility.Test())
                throw new ConfigurationException("Configuration database not found");

            this.log.LogLevel = Config.ApplicationServer.LogLevel;

            // Register the default unhandled exception handler
            AppDomain.CurrentDomain.UnhandledException += 
                new UnhandledExceptionEventHandler(UnhandledExceptionHandler);

            this.started = new System.Threading.ManualResetEvent(false);
            this.shutdown = new System.Threading.ManualResetEvent(false);

            this.autoSignalThreadShutdown = false;

            this.logger = Logger.Instance;
            this.logger.DiagLoggerQueue = IsLoggerQueueDiagEnabled();

            this.logSinks = new LogSinkCollection();

            // Instantiate primary components
            this.router = new Router();
            this.mediaManager = new MediaManager();
            this.appManager = new AppManager();
            this.telManager = new TelManager();
            this.management = new ManagementInterface();
            this.cluster = new ClusterInterface();

            this.localTaskStatus = new TallyCollection();
            localTaskStatus.AddItem(IConfig.CoreComponentNames.ROUTER);
            localTaskStatus.AddItem(IConfig.CoreComponentNames.APP_MANAGER);
            localTaskStatus.AddItem(IConfig.CoreComponentNames.MEDIA_MANAGER);
            localTaskStatus.AddItem(IConfig.CoreComponentNames.TEL_MANAGER);
            localTaskStatus.AddItem(IConfig.CoreComponentNames.MANAGEMENT);
            localTaskStatus.AddItem(IConfig.CoreComponentNames.CLUSTER_INTERFACE);
        }
        #endregion

        #region PrimaryTaskBase overrides

        protected override bool HandleMessage(InternalMessage im)
        {
            Assertion.Check(this.router != null, "Router reference is null");
            Assertion.Check(this.appManager != null, "AppManager reference is null");
            Assertion.Check(this.management != null, "IPC Manager reference is null");

            switch(im.MessageId)
            {
                case IResponses.STARTUP_COMPLETE:
                    OnStartupComplete(im);
                    return true;
                case IResponses.SHUTDOWN_COMPLETE:
                    OnShutdownComplete(im);
                    return true;
                case IResponses.STARTUP_FAILED:
                    OnStartupFailed(im);
                    return true;
                case IResponses.SHUTDOWN_FAILED:
                    OnShutdownComplete(im);
                    return true;
            }

            return false;
        }

        protected override TraceLevel GetLogLevel()
        {
            return Config.ApplicationServer.LogLevel;
        }

        protected override void RefreshConfiguration(string proxy)
        {
            if(proxy == IConfig.CoreComponentNames.LOGGER)
            {
                logger.DiagLoggerQueue = IsLoggerQueueDiagEnabled();
                logger.RefreshConfiguration();
            }
            else if(proxy != null)
            {
                log.Write(TraceLevel.Warning, "Received proxy message for an unreachable component: " + proxy);
            }
        }

        public override void Dispose()
        {
            management.Dispose();
            appManager.Dispose();
            router.Dispose();
            logger.Dispose();
            statsClient.Dispose();

            base.Dispose();

            ApplicationServer.instance = null;
        }
        #endregion

        #region Startup

        /// <summary>Start the application server.</summary>
        public bool Startup()
        {
			#if DEBUG
				log.Write(TraceLevel.Info, "Application Server is running in DEBUG mode");
			#endif

            // Adjust Windows scheduler resolution
            Win32.timeBeginPeriod(1);

            // Verify that the framework can be found
            if(configUtility.FrameworkDir == null)
            {
                log.Write(TraceLevel.Error, "Framework directory cannot be located.");
                return false;
            }

            try { ClearShadowCacheDir(); }
            catch(Exception e)
            {
                log.Write(TraceLevel.Warning, "Failed to delete old shadow copied files: " + e.Message);
            }

            CommandMessage startupMessage = CreateCommandMessage(this.Name, ICommands.STARTUP);

            // Post the startup message to ourselves to begin application server startup.
            this.PostMessage(startupMessage);               

            return started.WaitOne(STARTUP_TIMEOUT, false);
        }

        /// <summary>
        /// Deletes the contents of the directory used for shadow-copying 
        /// application and provider assemblies.
        /// </summary>
        private static void ClearShadowCacheDir()
        {
            DirectoryInfo dInfo = Config.CacheDir;
            
            if(dInfo.Exists)
                dInfo.Delete(true);
            
            dInfo.Create();
        }

        /// <summary>
        /// This method will post startup messages to each 
        /// primary component. Upon doing this, each component will begin 
        /// their startup procedure and be ready for execution.
        /// </summary>
        protected override void OnStartup()
        {
            Assertion.Check(this.router != null, "Router reference is null");
            Assertion.Check(this.appManager != null, "AppManager reference is null");
            Assertion.Check(this.telManager != null, "TelManager reference is null");
            Assertion.Check(this.management != null, "IPC Manager reference is null");

            // Post startup message to cluster interface first. 
            // After that, the others will be started.
            CommandMessage startupMessage = 
                this.CreateCommandMessage(IConfig.CoreComponentNames.CLUSTER_INTERFACE, ICommands.STARTUP);
            cluster.PostMessage(startupMessage);
        }

        private void OnStartupComplete(InternalMessage im)
        {
            Assertion.Check(im.Source != "", "Received a STARTUP_COMPLETE without a source");

            startupProgress(im.Source + " started successfully");
            
            localTaskStatus.Check(im.Source);

            if(this.localTaskStatus.AllChecked)
            {
                started.Set();
                startupComplete();
            }
            else if(im.Source == IConfig.CoreComponentNames.CLUSTER_INTERFACE)
            {
                CommandMessage startupMessage = 
                    this.CreateCommandMessage(IConfig.CoreComponentNames.TEL_MANAGER, ICommands.STARTUP);
                telManager.PostMessage(startupMessage);

                startupMessage = 
                    this.CreateCommandMessage(IConfig.CoreComponentNames.ROUTER, ICommands.STARTUP);
                router.PostMessage(startupMessage);

                startupMessage = 
                    this.CreateCommandMessage(IConfig.CoreComponentNames.MEDIA_MANAGER, ICommands.STARTUP);
                mediaManager.PostMessage(startupMessage);

                startupMessage = 
                    this.CreateCommandMessage(IConfig.CoreComponentNames.MANAGEMENT, ICommands.STARTUP);
                management.PostMessage(startupMessage);
            }
            else if(localTaskStatus.GetUncheckedNames().Count == 1)
            {
                // Wait until everything else is ready before loading apps
                CommandMessage startupMessage = 
                    this.CreateCommandMessage(IConfig.CoreComponentNames.APP_MANAGER, ICommands.STARTUP);
                appManager.PostMessage(startupMessage);
            }
        }

        private void OnStartupFailed(InternalMessage im)
        {
            // Set a flag so watchdog won't restart us
            configUtility.SetUserStoppedFlag(true);

            // Trigger alarm
            statsClient.TriggerAlarm(IConfig.Severity.Red, IStats.AlarmCodes.AppServer.StartFailure,
                IStats.AlarmCodes.AppServer.Descriptions.StartFailure);

            // PrimaryTaskBase already printed an error, so just take us down...
            Process.GetCurrentProcess().Kill();
        }
        #endregion

        #region Shutdown
        /// <summary>
        /// Shutdown the application server.
        /// </summary>
        public void Shutdown()
        {
            CommandMessage shutdownMessage = this.CreateCommandMessage(this.Name, ICommands.SHUTDOWN);

            // Post the shutdown message to ourselves to begin application server shutdown.
            this.PostMessage(shutdownMessage);              

            // Reset Windows scheduler resolution
            Win32.timeEndPeriod(1);
                                                            
            if(shutdown.WaitOne(SHUTDOWN_TIMEOUT, false))
            {
                log.Write(TraceLevel.Info, "Application Server stopped gracefully");
            }
            else
            {
                log.Write(TraceLevel.Error, "Application Server shutdown timed out. Exiting forcefully...");
                
                // Commit suicide
                Process me = Process.GetCurrentProcess();
                me.Kill();
            }
        }   

        /// <summary>
        /// This method will post shutdown messages to each 
        /// primary component. Upon doing this, each component will 
        /// begin their shutdown procedure.
        /// </summary>
        protected override void OnShutdown()
        {
            Assertion.Check(this.router != null, "Router reference is null");
            Assertion.Check(this.appManager != null, "AppManager reference is null");
            Assertion.Check(this.management != null, "IPC Manager reference is null");

            this.localTaskStatus.UncheckAll();

            // Post shutdown messages to each of the primary components.
            CommandMessage shutdownMessage = 
                this.CreateCommandMessage(IConfig.CoreComponentNames.CLUSTER_INTERFACE, ICommands.SHUTDOWN);
            shutdownMessage.AddField("shutdownImmediately", "false");
            cluster.PostMessage(shutdownMessage);

            shutdownMessage = 
                this.CreateCommandMessage(IConfig.CoreComponentNames.MANAGEMENT, ICommands.SHUTDOWN);
            shutdownMessage.AddField("shutdownImmediately", "false");
            management.PostMessage(shutdownMessage);

            shutdownMessage = 
                this.CreateCommandMessage(IConfig.CoreComponentNames.ROUTER, ICommands.SHUTDOWN);
            shutdownMessage.AddField("shutdownImmediately", "false");
            router.PostMessage(shutdownMessage);            

            shutdownMessage = 
                this.CreateCommandMessage(IConfig.CoreComponentNames.APP_MANAGER, ICommands.SHUTDOWN);
            shutdownMessage.AddField("shutdownImmediately", "false");
            appManager.PostMessage(shutdownMessage);

            shutdownMessage = 
                this.CreateCommandMessage(IConfig.CoreComponentNames.MEDIA_MANAGER, ICommands.SHUTDOWN);
            shutdownMessage.AddField("shutdownImmediately", "false");
            mediaManager.PostMessage(shutdownMessage);

            shutdownMessage = 
                this.CreateCommandMessage(IConfig.CoreComponentNames.TEL_MANAGER, ICommands.SHUTDOWN);
            shutdownMessage.AddField("shutdownImmediately", "false");
            telManager.PostMessage(shutdownMessage);
        }

        private void OnShutdownComplete(InternalMessage im)
        {
            Assertion.Check(im.Source != "", "Received a SHUTDOWN_COMPLETE without a source");

            shutdownProgress(im.Source + " shutdown successfully");
            
            this.localTaskStatus.Check(im.Source);

            if(this.localTaskStatus.AllChecked)
            {
                shutdownRequested = true;
                shutdown.Set();
                shutdownComplete();
            }
        }
        #endregion

        #region Unhandled Exception handler

        public void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
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
            else if(Exceptions.Compare(ie, configUtility.LastChildDomainException))
            { 
                // Do nothing
            }
            else
            {
                // Log it
                log.Write(TraceLevel.Error, "The Application Server engine has encountered unhandled exception: {0}", exStr);

                // Trigger an alarm
                if(ie is OutOfMemoryException)
                    statsClient.TriggerAlarm(IConfig.Severity.Red, IStats.AlarmCodes.General.OutOfMemory,
                        IStats.AlarmCodes.General.Descriptions.OutOfMemory, Name);
                else
                    statsClient.TriggerAlarm(IConfig.Severity.Red, IStats.AlarmCodes.AppServer.UnexpectedShutdown,
                        IStats.AlarmCodes.AppServer.Descriptions.UnexpectedShutdown);

                // Wait a moment for the log to flush
                Thread.Sleep(500);

                // Clear the flag so watchdog will restart us
                configUtility.SetUserStoppedFlag(false);

                // Reset Windows scheduler resolution
                Win32.timeEndPeriod(1);
                
                // Commit seppuku and let Watchdog resurrect us
                Process.GetCurrentProcess().Kill();
            }
        }
        #endregion

        #region Logging

        private TraceLevel consoleLogLevel;

        public void StartLogger(TraceLevel consoleLevel, EventLog eventLog)
        {
            this.consoleLogLevel = consoleLevel;

            // Set logger status to "Enabled_Running"
            configUtility.UpdateStatus(IConfig.ComponentType.Core, IConfig.CoreComponentNames.LOGGER, IConfig.Status.Enabled_Running);

            LogServerSink lss = new LogServerSink(ServiceName, Config.LogService.ListenPort, GetLogServerSinkLevel());
            lss.GetLogLevel = new LogLevelQueryDelegate(GetLogServerSinkLevel);
            logSinks.Add(lss);

            TcpLoggerSink tls = new TcpLoggerSink(Config.Logger.TcpLoggerPort, configUtility, GetTcpLogLevel());
            tls.GetLogLevel = new LogLevelQueryDelegate(GetTcpLogLevel);
            logSinks.Add(tls);

            if(consoleLogLevel != TraceLevel.Off)
            {
                ConsoleLoggerSink cls = new ConsoleLoggerSink(GetConsoleLogLevel());
                cls.GetLogLevel = new LogLevelQueryDelegate(GetConsoleLogLevel);
                logSinks.Add(cls);
            }

            if(eventLog != null)
            {
                EventLogSink els = new EventLogSink(eventLog, GetEventLogLevel());
                els.GetLogLevel = new LogLevelQueryDelegate(GetEventLogLevel);
                logSinks.Add(els);
            }
        }

        public void StopLogger()
        {
            // Set logger status to "Enabled_Stopped"
            configUtility.UpdateStatus(IConfig.ComponentType.Core, IConfig.CoreComponentNames.LOGGER, IConfig.Status.Enabled_Stopped);

            // Just make sure the logger is shutdown, otherwise it may hang
            logger.Dispose();

            foreach(LoggerSinkBase sink in logSinks)
            {
                sink.Dispose();
            }
        }

        private TraceLevel GetLogServerSinkLevel()
        {
            return Config.Logger.LogServerLevel;
        }

        private TraceLevel GetTcpLogLevel()
        {
            return TraceLevel.Verbose;
        }

        private TraceLevel GetConsoleLogLevel()
        {
            return consoleLogLevel;
        }

        private TraceLevel GetEventLogLevel()
        {
            return TraceLevel.Error;
        }

        /// <summary>
        /// Check the configuration database to see if the logger's queue diagnostics
        /// are enabled.
        /// </summary>
        /// <returns>True if diagnostics are enabled, false otherwise.</returns>
        private bool IsLoggerQueueDiagEnabled()
        {
            bool enableLoggerQDiag = Convert.ToBoolean(configUtility.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.LOGGER, 
                IConfig.Entries.Names.LOG_ENABLE_Q_DIAGS));

            return enableLoggerQDiag;
        }
        #endregion
    }
}