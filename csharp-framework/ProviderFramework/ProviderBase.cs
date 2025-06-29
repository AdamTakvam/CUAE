using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Diagnostics;
using System.Threading;

using Metreos.Core;
using Metreos.Core.ConfigData;
using Metreos.Utilities;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.LoggingFramework;

namespace Metreos.ProviderFramework
{
    public delegate void HandleMessageDelegate(ActionBase action);

    /// <summary>
    /// Base class for all providers. Extends PrimaryTaskBase and adds
    /// functionality to make it easier to implement.
    /// </summary>
    public abstract class ProviderBase : PrimaryTaskBase, IProvider
    {
		#region Constants

		internal abstract class Consts
		{
			public abstract class Defaults
			{
				public const bool AsyncRefresh	= false;
			}
		}
		#endregion

        /// <summary> Holds a hash of message ID to instances of HandleMessageDelegate.</summary>
        protected Hashtable messageCallbacks;

        /// <summary>All actions which are not specifically registered for and are not 
        /// internally reserved are sent to this method.</summary>
        protected HandleMessageDelegate defaultHandler = null;

        protected MessageQueueWriter palWriter;

        protected string providerNamespace;

        protected ComponentInfo componentInfo;

		/// <summary>
		/// Indicates that the RefreshConfiguration() should be executed in a separate thread.
		/// </summary>
		private readonly bool asyncRefresh;

		/// <summary>Thread on which RefreshConfiguration() will be called if provider elects</summary>
		private Thread asyncRefreshThread = null;

		/// <summary>
		/// Indicates that a RefreshConfiguration request was received while an asynch 
		/// RefreshConfiguration was running.
		/// </summary>
		private bool refreshPending = false;

        #region Protected properties

        protected DirectoryInfo DocsDir { get { return GetProviderDir(IConfig.ProvDirectoryNames.Docs); } }
        protected DirectoryInfo ResourceDir { get { return GetProviderDir(IConfig.ProvDirectoryNames.Resources); } }
        protected DirectoryInfo ServiceDir { get { return GetProviderDir(IConfig.ProvDirectoryNames.Service); } }
        protected DirectoryInfo WebDir { get { return GetProviderDir(IConfig.ProvDirectoryNames.Web); } }

        #endregion

        #region Constuction/Initialization/Startup

        protected ProviderBase(Type providerType, string displayName, IConfigUtility config)
			: this(providerType, displayName, config, Consts.Defaults.AsyncRefresh) {}

        protected ProviderBase(Type providerType, string displayName, IConfigUtility config, bool asyncRefresh) 
            : base(IConfig.ComponentType.Provider, providerType.Name, displayName, config)
        {
			this.asyncRefresh = asyncRefresh;

            this.providerNamespace = Namespace.GetNamespace(providerType.FullName);

            messageCallbacks = new Hashtable();

            // Initialize static component information
            componentInfo = new ComponentInfo();
            componentInfo.name = this.Name;
            componentInfo.displayName = displayName;
            componentInfo.type = IConfig.ComponentType.Provider;
            componentInfo.status = IConfig.Status.Enabled_Stopped;

            // Load dynamic component info from assembly metadata
            GetAssemblyData(providerType, out componentInfo.version, out componentInfo.description, 
                out componentInfo.author, out componentInfo.copyright);
            
            componentInfo.groups = new ComponentGroup[1];
            componentInfo.groups[0] = new ComponentGroup();
            componentInfo.groups[0].ID = Database.ComponentGroupIds.PROVIDERS;

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandler);
        }

        /// <summary>
        /// Extracts the provider assembly metadata (AssemblyInfo.cs) to be stored 
        ///   in the DB with the provider config
        /// </summary>
        private void GetAssemblyData(Type pType, out string version, out string description, out string author, 
            out string copyright)
        {
            version = null;
            description = null;
            author = null;
            copyright = null;

            object[] attrs = pType.Assembly.GetCustomAttributes(true);
            
            if(attrs == null || attrs.Length == 0)
                return;

            foreach(object attr in attrs)
            {
                if(attr is AssemblyVersionAttribute)
                    version = ((AssemblyVersionAttribute)attr).Version;
                else if(attr is AssemblyDescriptionAttribute)
                    description = ((AssemblyDescriptionAttribute)attr).Description;
                else if(attr is AssemblyCompanyAttribute)
                    author = ((AssemblyCompanyAttribute)attr).Company;
                else if(attr is AssemblyCopyrightAttribute)
                    copyright = ((AssemblyCopyrightAttribute)attr).Copyright;
            }

            if(version == null)
                version = pType.Assembly.GetName().Version.ToString();

            log.Write(TraceLevel.Verbose, "Assembly metadata: Version='{1}', Description='{2}', Author='{3}', Copyright='{4}'",
                this.Name, version, description, author, copyright);
        }

        private void SetupProvider()
        {
            ComponentInfo cInfo = configUtility.GetComponentInfo(IConfig.ComponentType.Provider, componentInfo.name);

            if(cInfo == null)
            {
                if(!configUtility.AddComponent(componentInfo, null))
                    throw new ConfigurationException("Failed to add component: " + this.Name);
            }
            else
            {
                if(!configUtility.UpdateComponent(IConfig.ComponentType.Provider, componentInfo.name, componentInfo.displayName,
                    componentInfo.version, componentInfo.description, componentInfo.author, componentInfo.copyright))
                {
                    throw new ConfigurationException("Failed to update component: " + this.Name);
                }
            }
        }

        public object GetMessageQueue()
        {
            return this.taskQueue;
        }

        public void SetRouterQueueWriter(object writer)
        {
            Debug.Assert(writer != null, "Setting null Abstract queue writer on provider");
            Debug.Assert(writer is MessageQueueWriter, "You must pass a MessageQueueWriter object to SetRouterQueueWriter");

            palWriter = writer as MessageQueueWriter;
        }

        public override void Dispose()
        {
			this.refreshPending = false;
            
            if(this.asyncRefreshThread != null)
			    this.asyncRefreshThread.Abort();

            if(palWriter != null)
                palWriter.Dispose();

            messageCallbacks.Clear();

            base.Dispose();
        }

        /// <summary>[Deprecated] Provided for legacy compatibility only. Override Dispose() instead.</summary>
        public virtual void Cleanup()
        {
            Dispose();
        }

        public string GetName()
        {
            return this.Name;
        }

        public bool InitializeProvider(Logger logger)
        {
            // Initialize trace listers for this AppDomain
            Trace.Listeners.Clear();
            Trace.Listeners.Add(logger);

            // Create the component
            SetupProvider();

            // Add logLevel to config
            ConfigEntry logEntry = new ConfigEntry(IConfig.Entries.Names.LOG_LEVEL, 
                IConfig.Entries.DisplayNames.LOG_LEVEL, TraceLevel.Info, 
                IConfig.Entries.Descriptions.LOG_LEVEL, IConfig.StandardFormat.TraceLevel, true);
            configUtility.AddEntry(IConfig.ComponentType.Provider, Name, logEntry, false);

            // Set the log level
            base.RefreshConfiguration(GetLogLevel());

            // Call Initialize() on provider
            ConfigEntry[] configItems;
            Extension[] extensions;
            bool initResult = this.Initialize(out configItems, out extensions);

            AddConfigItems(configItems);
            AddExtensions(extensions);

            // Finish initializing provider
            RefreshConfiguration(null);

            return initResult;
        }

        protected override TraceLevel GetLogLevel()
        {
            return (TraceLevel)configUtility.GetEntryValue(IConfig.ComponentType.Provider, Name, 
                IConfig.Entries.Names.LOG_LEVEL);
        }

        /// <summary>
        /// Make sure this object has an infinite lifetime in terms of the remote garbage
        /// collector.
        /// </summary>
        /// <returns>Always returns null.</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
        #endregion

        #region RefreshConfiguration

        protected override void RefreshConfiguration(string proxy)
        {
            // Rather unsightly hack for some rotting code in ProviderFactory that we can't change right now.
            IConfig.Status status = TaskStatus; // DB dip
            if(status != IConfig.Status.Enabled_Running &&
				status != IConfig.Status.Enabled_Stopped)
                return;

            if(this.shutdownRequested)
                return;

            if(asyncRefresh)
            {
                if(asyncRefreshThread != null && asyncRefreshThread.IsAlive)
                {
                    this.refreshPending = true;
                }
                else
                {
                    asyncRefreshThread = new Thread(new ThreadStart(AsyncRefreshConfiguration));
                    asyncRefreshThread.Start();
                }
            }
            else
            {
                RefreshConfiguration();
            }
        }

        private void AsyncRefreshConfiguration()
        {
            do
            {
                refreshPending = false;
                RefreshConfiguration();
            }
            while(refreshPending);
        }
        #endregion

        #region Provider overrides
        /// <summary>
        /// Initialize the provider.
        /// </summary>
        /// <returns>true if successfull, false otherwise.</returns>
        protected abstract bool Initialize(out ConfigEntry[] configItems, out Extension[] extensions);

        /// <summary>
        /// Refresh provider configuration
        /// </summary>
        protected abstract void RefreshConfiguration();

        /// <summary>
        /// Indicates that no application was available to service an event
        /// </summary>
        /// <param name="noHandlerAction">The NoHandler action (which may contain reason or other metadata)</param>
        /// <param name="originalEvent">The event which was not handed</param>
        protected abstract void HandleNoHandler(ActionMessage noHandlerAction, EventMessage originalEvent);

        /// <summary>
        /// Called when this provider is started by the PAL.Abstractor component.
        /// Override this method if custom startup code is required.
        /// </summary>
        /// 
        /// <remarks>
        /// To indicate startup failure, throw a Core.StartupFailedException.
        /// </remarks>
        protected override void OnStartup()
        {}


        /// <summary>
        /// Called when this provider is shutdown by the PAL.Abstractor component.
        /// Override this method if custom shutdown code is required.
        /// </summary>
        /// 
        /// <remarks>
        /// To indicate startup failure, throw a Core.ShutdownFailedException.
        /// </remarks>
        protected override void OnShutdown()
        {}
        #endregion

        #region Unhandled exception handler

        /// <summary>Propogates unhandled exceptions to the ProviderManager</summary>
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
            else
            {
                if(log != null)
                {
                    log.Write(TraceLevel.Error, "Provider '{0}' threw an unhandled exception: {1}",
                        base.Name, Exceptions.FormatException(ie));
                }

                // Save exception so main exception handler doesn't take the server down
                configUtility.LastChildDomainException = ie;

                // Send message to Provider Manager
                CommandMessage msg = CreateCommandMessage(IConfig.CoreComponentNames.PROV_MANAGER, ICommands.UNHANDLED_EXCEPTION);
                msg.SourceQueue = null;  // suppress response
                this.palWriter.PostMessage(msg);
            }
        }
        #endregion

        #region Provider helper methods

        public void RegisterNamespace()
        {
            Debug.Assert(palWriter != null, "palWriter is null");

            CommandMessage registerNamespaceMsg = 
                this.CreateCommandMessage(IConfig.CoreComponentNames.PROV_MANAGER, ICommands.REGISTER_PROV_NAMESPACE);
            registerNamespaceMsg.AddField(ICommands.Fields.PROVIDER_NAMESPACE, providerNamespace);
            
            this.palWriter.PostMessage(registerNamespaceMsg);
        }

        protected object GetConfigValue(string valueName)
        {
            return configUtility.GetEntryValue(IConfig.ComponentType.Provider, componentInfo.name, valueName);
        }
        #endregion

        #region Message pump

        /// <summary>Handle a message sent to this provider.</summary>
        /// <remarks>
        /// Operates off the basis that there are entries within the messageCallbacks
        /// hash table that are delegates to functions to handle processing for
        /// messages that arrive on the queue.
        /// </remarks>
        /// <param name="im">Message received from the queue.</param>
        /// <returns>True if the message was handled, false otherwise.</returns>
        protected sealed override bool HandleMessage(InternalMessage im)
        {
            ActionMessage actionMsg = im as ActionMessage;
            if(actionMsg != null)
            {
                if(actionMsg.MessageId == IActions.NoHandler)
                {
                    HandleNoHandler(actionMsg, actionMsg[IActions.Fields.InnerMsg] as EventMessage);
					return true;
                }

                HandleMessageDelegate callbackFoundInHash = 
                    messageCallbacks[actionMsg.MessageId] as HandleMessageDelegate;

                if(callbackFoundInHash == null)
                {
                    if(ReservedActionHandler(actionMsg) == false)
                    {
                        if(this.defaultHandler != null)
                        {
                            callbackFoundInHash = this.defaultHandler;
                        }
                        else
                        {
                            log.Write(TraceLevel.Warning, 
                                "Action: " + actionMsg.MessageId + " received with appropriate namespace, but no message handler is registered.");
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
        
                ActionBase action = null;
                if(actionMsg.Type == ActionMessage.ActionType.Asynchronous)
                {
                    action = new AsyncAction(this.componentInfo.name, actionMsg);
                }
                else
                {
                    action = new SyncAction(this.componentInfo.name, actionMsg);
                }

                try
                {
                    callbackFoundInHash(action);
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Exception caught inside provider message callback: " +
						Metreos.Utilities.Exceptions.FormatException(e));
                }
                return false;
            }
            return false;
        }

        private bool ReservedActionHandler(ActionMessage msg)
        {
            if(msg.MessageId == IActions.Ping)
            {
                msg.SendResponse(IResponses.PONG);
                return true;
            }

            return false;
        }
        #endregion

        #region Private helper methods

        private DirectoryInfo GetProviderDir(string dirName)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dirName);
            return new DirectoryInfo(path);
        }

        private void AddExtensions(Extension[] exts)
        {
            configUtility.RemoveExtensions(Name);

            if(exts == null)
                return;

            foreach(Extension ext in exts)
            {
                configUtility.AddExtension(componentInfo.name, ext);
            }
        }

        private void AddConfigItems(ConfigEntry[] configs)
        {
            // Load existing config values into memory and then wipe the old entries from the DB
            // config entry name -> ConfigEntry object
            IDictionary oldEntries = configUtility.GetEntries(IConfig.ComponentType.Provider, Name, null);
            if(oldEntries != null)
            {
                foreach(string entryName in oldEntries.Keys)
                {
                    // The log level config should never be removed.
                    // Remove all others
                    if(entryName != IConfig.Entries.Names.LOG_LEVEL)
                        configUtility.RemoveEntry(IConfig.ComponentType.Provider, Name, entryName, null);
                }
            }

            if(configs == null)
                return;

            foreach(ConfigEntry cEntry in configs)
            {
                if(cEntry == null)
                    continue;

                // Preserve old value, if present
                ConfigEntry oldEntry = oldEntries[cEntry.name] as ConfigEntry;
                if(oldEntry != null)
                    cEntry.Value = oldEntry.Value;

                configUtility.AddEntry(IConfig.ComponentType.Provider, componentInfo.name, cEntry, true);
            }
        }
        #endregion
    }
}
