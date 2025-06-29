using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
using System.Xml.Serialization;

using Metreos.Utilities;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;
using Metreos.ProviderFramework;
using Metreos.ProviderPackagerCore;
using Metreos.Utilities.Collections;
using Metreos.ProviderPackagerCore.Xml;

using Metreos.Configuration;
using Metreos.AppServer.ProviderManager.Collections;

namespace Metreos.AppServer.ProviderManager
{
    public sealed class ProviderManager : Loggable
    {
        private abstract class Consts
        {
            public abstract class Defaults
            {
                public const uint StartupTimeout     = 30000;
                public const uint ShutdownTimeout    = 10000;
            }

            public const uint PingInterval       = 30000;
        }

        private readonly Config configUtility;
        private MessageQueueWriter tmQ;

        private readonly ProviderInitManager initManager;
        private readonly ServiceManager serviceManager;

        private readonly ProviderTable providerTable;
        private readonly PingInfoCollection providerPings;

        private readonly MessageQueueWriter routerQ;
        private readonly MessageUtility messageUtility;

        private readonly DirectoryInfo deployDir;
        private readonly DirectoryInfo providerDir;

        /// <summary>Keeps track of the dll file names for each provider</summary>
        /// <remarks>Provider name (string) -> FileInfo (object)</remarks>
        private readonly Hashtable providerFiles;

        private CommandMessage refreshProvidersCmd;
        private readonly TallyCollection refreshTally;

        private Timer providerPinger;

        /// <summary>Stats/Alarms client connection</summary>
        private Metreos.Stats.StatsClient statsClient;

        private uint startupTimeout;
        private uint shutdownTimeout;

        #region Construct/Startup/Shutdown/Dispose
        public ProviderManager()
            : base(Config.ProviderManager.LogLevel, "PM")
        {
            this.configUtility = Config.Instance;

            this.providerDir = Config.ProviderDir;
            if(providerDir == null)
                throw new Metreos.Core.StartupFailedException("No provider directory configured.");

            this.deployDir = Config.AppDeployDir;
            if(deployDir == null)
                throw new Metreos.Core.StartupFailedException("No deploy directory configured.");

            this.routerQ = MessageQueueFactory.GetQueueWriter(IConfig.CoreComponentNames.ROUTER);
            if(this.routerQ == null)
                throw new Metreos.Core.StartupFailedException("Cannot get a writer for the Router queue");

            this.initManager = new ProviderInitManager(log);
            this.serviceManager = new ServiceManager(log);

            this.statsClient = Metreos.Stats.StatsClient.Instance;

            this.providerPings = new PingInfoCollection();
            this.providerTable = new ProviderTable();
            this.providerFiles = new Hashtable();
            this.refreshTally = new TallyCollection();
            this.messageUtility = new MessageUtility(IConfig.CoreComponentNames.PROV_MANAGER, 
                IConfig.ComponentType.Core, routerQ);
        }

        public void Startup()
        {
            RefreshConfiguration(IConfig.CoreComponentNames.PROV_MANAGER);

            // Grab a writer for the TelephonyManager
            //   so we can forward CC registrations to it
            this.tmQ = MessageQueueFactory.GetQueueWriter(IConfig.CoreComponentNames.TEL_MANAGER);
            Assertion.Check(tmQ != null, "Cannot get a writer for the Telephony Manager queue");

            // Adjust for legacy provider placement
            MoveLegacyProviderFiles();

            // Get handles to provider files
            DirectoryInfo[] pDirs = this.providerDir.GetDirectories();
            if(pDirs != null)
            {
                foreach(DirectoryInfo pDir in pDirs)
                {
                    // Leverage the fact that the provider directory
                    //  must be named the same as the .dll
                    FileInfo assembly = new FileInfo(Path.Combine(pDir.FullName, pDir.Name + IPackager.ProvFileExt));
                    if(assembly.Exists)
                        LoadProvider(assembly);
                    else
                        log.Write(TraceLevel.Warning, "Provider directory '{0}' exists with no provider implementation", pDir.Name);
                }
            }

            if(this.providerTable.Count == 0)
            {
                log.Write(TraceLevel.Warning, "Startup complete, no providers loaded.");
            }
            else if(this.providerTable.Count == 1)
            {
                log.Write(TraceLevel.Info, "1 provider started.");
            }
            else
            {
                log.Write(TraceLevel.Info, "{0} providers started.", this.providerTable.Count.ToString());
            }

            // Set the component status
            configUtility.UpdateStatus(IConfig.ComponentType.Core, 
                IConfig.CoreComponentNames.PROV_MANAGER, 
                IConfig.Status.Enabled_Running);

            // Start the provider maintenance timer
            providerPinger = 
                new Timer(new TimerCallback(PingProviders), null, Consts.PingInterval, Consts.PingInterval);
        }

        public void Shutdown()
        {
            if(providerPinger != null)
            {
                providerPinger.Dispose();
                providerPinger = null;
            }

            if(providerTable.Count == 0)
            {
                // Ok, shutdown is complete because we have no providers to shutdown.
                log.Write(TraceLevel.Info, "Shutdown complete.");
                return;
            }

            // Send shutdown messages to providers
            ArrayList provTemp = new ArrayList();
            lock(providerTable.SyncRoot)
            {
                // Create temp collection because main provider table will be 
                //  modified by UnloadProvider()
                foreach(ProviderInfo pInfo in providerTable)
                {
                    provTemp.Add(pInfo);
                }
            }

            foreach(ProviderInfo pInfo in provTemp)
            {
                UnloadProvider(pInfo, false, true);
            }

            log.Write(TraceLevel.Info, "All providers shutdown");

            providerTable.Clear();
        }

        public void RefreshConfiguration(string proxy)
        {
            if(proxy == IConfig.CoreComponentNames.PROV_MANAGER)
            {
                this.log.LogLevel = Config.ProviderManager.LogLevel;
                
                this.startupTimeout = Config.ProviderManager.StartupTimeout;
                if(this.startupTimeout == 0)
                    this.startupTimeout = Consts.Defaults.StartupTimeout;

                this.shutdownTimeout = Config.ProviderManager.ShutdownTimeout;
                if(this.shutdownTimeout == 0)
                    this.shutdownTimeout = Consts.Defaults.ShutdownTimeout;
            }
            else
            {
                ProviderInfo pInfo = providerTable[proxy];

                if(pInfo != null)
                {
                    // Tell the specified provider to reload config
                    CommandMessage refreshMsg = 
                        messageUtility.CreateCommandMessage(pInfo.Name, ICommands.REFRESH_CONFIG);
                    pInfo.ProviderQ.PostMessage(refreshMsg);
                }
                else
                {
                    log.Write(TraceLevel.Warning, "Received RefreshConfiguration message for unknown provider: " + proxy);
                }
            }
        }

        public override void Dispose()
        {
            providerTable.Clear();

            if(providerPinger != null)
            {
                providerPinger.Dispose();
                providerPinger = null;
            }

            initManager.Dispose();
        }
        #endregion

        #region Load/Unload Providers
        
        /// <summary>
        /// Loads a provider from an assembly file.
        /// </summary>
        /// <remarks>
        /// Only one provider per assembly is supported. If multiple providers 
        /// are found in an assembly, only the first one encountered will be loaded.
        /// </remarks>
        /// <param name="assemblyFileName">Filename of the assembly containing the provider.</param>
        private bool LoadProvider(FileInfo assemblyFile)
        {
            string failReason;
            return LoadProvider(assemblyFile, false, out failReason);
        }

        private bool LoadProvider(FileInfo assemblyFile, bool ignoreStatus, out string failReason)
        {
            failReason = null;

            if(assemblyFile == null)
            {
                failReason = "LoadProvider() called with a null assemblyFile.";
                log.Write(TraceLevel.Error, failReason);
                return false;
            }

            ProviderInfo pInfo = new ProviderInfo(log, assemblyFile);

            bool disabled;
            if(pInfo.CreateProvider(routerQ, out disabled, out failReason) == false)
                return false;

            if(ignoreStatus || !disabled)
            {
                if(!pInfo.InitializeProvider(out failReason))
                    return false;

                this.providerTable.Add(pInfo);
                this.providerFiles[pInfo.Name] = assemblyFile;

                // Send startup message to provider
                if(initManager.Startup(pInfo, this.startupTimeout) == false)
                {
                    pInfo.DestroyProvider(true);
                    providerTable.Remove(pInfo.Name);

                    failReason = "Provider failed to start";
                    return false;
                }

                pInfo.Status = IConfig.Status.Enabled_Running;
            }
            return true;
        }

        private void UnloadProvider(string providerName)
        {
            ProviderInfo pInfo = providerTable[providerName];
            if(pInfo == null) { return; }

            UnloadProvider(pInfo, false, false);
        }

        private void UnloadProvider(ProviderInfo pInfo, bool violent)
        {
            UnloadProvider(pInfo, violent, false);
        }

        private void UnloadProvider(ProviderInfo pInfo, bool violent, bool shuttingDown)
        {
            if(!shuttingDown)
                UnregisterProvider(pInfo.Name);

            if(pInfo.Status == IConfig.Status.Enabled_Running)
            {
                // Shutdown provider
                initManager.Shutdown(pInfo, this.shutdownTimeout);
            }

            // Destroy it (unloads the appDomain)
            pInfo.DestroyProvider(violent);

            if(pInfo.Name != null)
                providerTable.Remove(pInfo.Name);
        }

        private void UnregisterProvider(string providerName)
        {
            StringCollection nss = providerTable.GetNamespaces(providerName);
            if(nss != null)
            {
                // Tell TM to unregister namespace(s)
                CommandMessage unregMsg = messageUtility.CreateCommandMessage(
                    IConfig.CoreComponentNames.TEL_MANAGER,
                    ICommands.UNREGISTER_PROV_NAMESPACE);

                unregMsg.AddField(ICommands.Fields.PROVIDER_NAMESPACE, nss);
                tmQ.PostMessage(unregMsg);
            }

            // Remove from local table
            providerTable.RemoveNamespaces(providerName);
        }

        private bool ReloadProvider(string providerName)
        {
            ProviderInfo pInfo = providerTable[providerName];
            if(pInfo == null) 
            {
                log.Write(TraceLevel.Error, "Failed to reload unknown provider: " + providerName);
                return false; 
            }

            FileInfo assemblyFile = pInfo.AssemblyFile;

            // Trigger alarm
            statsClient.TriggerAlarm(IConfig.Severity.Yellow, IStats.AlarmCodes.AppServer.ProviderReloaded,
                IStats.AlarmCodes.AppServer.Descriptions.ProviderReloaded, providerName);

            UnloadProvider(providerName);

            if(LoadProvider(assemblyFile) == false)
            {
                log.Write(TraceLevel.Error, "Failed to reload provider '{0}' from '{1}'.",
                    providerName, assemblyFile.Name);
                pInfo.DestroyProvider();
                providerTable.Remove(providerName);

                // Trigger alarm
                statsClient.TriggerAlarm(IConfig.Severity.Red, IStats.AlarmCodes.AppServer.ProviderLoadFailure,
                    IStats.AlarmCodes.AppServer.Descriptions.ProviderLoadFailure, providerName);
                return false;
            }

            log.Write(TraceLevel.Info, "'{0}' provider restarted successfully.", providerName);
            return true;
        }

        private void MoveLegacyProviderFiles()
        {
            FileInfo[] files = this.providerDir.GetFiles("*.dll");
            if(files != null)
            {
                foreach(FileInfo file in files)
                {
                    string dirPath = file.FullName.Replace(file.Extension, "");
                    if(Directory.Exists(dirPath))
                    {
                        try { file.Delete(); }
                        catch
                        {
                            log.Write(TraceLevel.Warning, "Failed to remove legacy provider file: " + file.FullName);
                        }
                    }
                    else
                    {
                        try 
                        { 
                            DirectoryInfo pDir = Directory.CreateDirectory(dirPath); 
                            FileInfo pdbFile = new FileInfo(file.FullName.Replace(file.Extension, IPackager.DebugFileExt));
                            if(pdbFile.Exists)
                                pdbFile.MoveTo(Path.Combine(dirPath, pdbFile.Name));
                            file.MoveTo(Path.Combine(dirPath, file.Name));

                            // Create standard provider directories
                            pDir.CreateSubdirectory(IConfig.ProvDirectoryNames.Resources);
                            pDir.CreateSubdirectory(IConfig.ProvDirectoryNames.Service);
                            pDir.CreateSubdirectory(IConfig.ProvDirectoryNames.Docs);
                            pDir.CreateSubdirectory(IConfig.ProvDirectoryNames.Web);
                        }
                        catch(Exception e)
                        {
                            log.Write(TraceLevel.Warning, "Failed to create directory for legacy provider file '{0}': {1}", dirPath, e.Message);
                        }
                    }
                }
            }
        }
        #endregion

        #region Load/Unload Services

        private bool LoadProviderServices(FileInfo providerFile, out string failReason)
        {
            failReason = null;

            DirectoryInfo dir = providerFile.Directory;
            DirectoryInfo[] dirs = dir.GetDirectories(IConfig.ProvDirectoryNames.Service);
            if(dirs == null || dirs.Length != 1)
            {
                log.Write(TraceLevel.Verbose, "No services found for provider: " + providerFile.Name);
                return true;
            }

            dir = dirs[0]; // Service directory
            FileInfo[] files = dir.GetFiles();
            if(files == null || files.Length == 0)
            {
                log.Write(TraceLevel.Verbose, "No services found for provider: " + providerFile.Name);
                return true;
            }

            // Open manifest file
            string manFilePath = Path.Combine(dir.FullName, IPackager.ManifestFile);
            if(!File.Exists(manFilePath))
            {
                failReason = "No service manifest found in: " + dir.FullName;
                log.Write(TraceLevel.Error, failReason);
                return false;
            }

            ServiceManifestType manifest = null;
            try
            {
                FileStream fStream = File.OpenRead(manFilePath);
                XmlSerializer serializer = new XmlSerializer(typeof(ServiceManifestType));
                manifest = (ServiceManifestType) serializer.Deserialize(fStream);
            }
            catch(Exception e)
            {
                failReason = "Failed to read service manifest: " + e.Message;
                log.Write(TraceLevel.Error, failReason);
                return false;
            }

            if(manifest.Service == null)
            {
                log.Write(TraceLevel.Verbose, "Found empty service manifest for: " + providerFile.Name);
                return true;
            }

            // Load services
            foreach(ServiceType serviceData in manifest.Service)
            {
                FileInfo serviceFile = new FileInfo(Path.Combine(dir.FullName, serviceData.Filename));
                if(!serviceFile.Exists)
                {
                    failReason = "Unable to find service specified in manifest: " + serviceFile.Name;
                    log.Write(TraceLevel.Error, failReason);
                    return false;
                }

                if(serviceManager.InstallService(serviceFile, serviceData.Name, serviceData.DisplayName, 
                    serviceData.Username, serviceData.Password, out failReason))
                {
                    try 
                    {
                        serviceManager.StartService(serviceData.Name, serviceData.Argument);
                        log.Write(TraceLevel.Info, "Service '{0}' started successfully", serviceData.Name);
                    }
                    catch(Exception e)
                    {
                        log.Write(TraceLevel.Warning, "Service '{0}' failed to start: {1}", serviceData.Name, e.Message);
                    }

                    if(!configUtility.AddService(serviceData.Name, serviceData.DisplayName, serviceData.Description))
                        log.Write(TraceLevel.Warning, "Failed to add service '{0}' to configuration database", serviceData.DisplayName);
                }
                else
                {
                    log.Write(TraceLevel.Error, failReason);
                }
            }
            return true;
        }

        private bool UnloadProviderServices(DirectoryInfo provDir, out string failReason)
        {
            failReason = null;

            DirectoryInfo[] dirs = provDir.GetDirectories(IConfig.ProvDirectoryNames.Service);
            if(dirs == null || dirs.Length != 1)
                return true;

            DirectoryInfo serviceDir = dirs[0];
            FileInfo[] files = serviceDir.GetFiles(IPackager.ManifestFile);
            if(files == null || files.Length != 1)
                return true;

            // Open manifest file
            ServiceManifestType manifest = null;
            FileInfo manFile = files[0];
            try
            {
                using(FileStream fStream = manFile.OpenRead())
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ServiceManifestType));
                    manifest = (ServiceManifestType) serializer.Deserialize(fStream);
                }
            }
            catch(Exception e)
            {
                failReason = String.Format("Failed to open service manifest ({0}): {1}", manFile.FullName, e.Message);
                log.Write(TraceLevel.Error, failReason);
                return false;
            }

            // Iterate over the services in the manifest
            if(manifest != null && manifest.Service != null)
            {                
                foreach(ServiceType sData in manifest.Service)
                {
                    try { serviceManager.StopService(sData.Name); }
                    catch(Exception e)
                    {
                        failReason = String.Format("Failed to stop service '{0}': {1}", sData.Name, e.Message);
                        log.Write(TraceLevel.Error, failReason);
                        continue;
                    }

                    log.Write(TraceLevel.Info, "Service '{0}' stopped successfully", sData.Name);

                    if(!serviceManager.UninstallService(sData.Name, out failReason))
                        log.Write(TraceLevel.Error, failReason);
                    else
                        configUtility.RemoveService(sData.DisplayName);
                }
            }

            return failReason == null;
        }
        #endregion

        #region Handle Commands
        public bool HandleCommandMessage(CommandMessage msg)
        {
            string failReason = null;
            ArrayList fields = new ArrayList();

            switch(msg.MessageId)
            {
                case ICommands.DISABLE_PROVIDER:
                    if(OnDisableProvider(msg, out failReason))
                        msg.SendResponse(IApp.VALUE_SUCCESS);
                    else
                    {
                        fields.Add(new Field(ICommands.Fields.FAIL_REASON, failReason));
                        msg.SendResponse(IApp.VALUE_FAILURE, fields, true);
                    }
                    return true;
                case ICommands.ENABLE_PROVIDER:
                    if(OnEnableProvider(msg, out failReason))
                        msg.SendResponse(IApp.VALUE_SUCCESS);
                    else
                    {
                        fields.Add(new Field(ICommands.Fields.FAIL_REASON, failReason));
                        msg.SendResponse(IApp.VALUE_FAILURE, fields, true);
                    }
                    return true;
                case ICommands.INSTALL_PROVIDER:
                    if(OnInstallProvider(msg, out failReason))
                        msg.SendResponse(IApp.VALUE_SUCCESS);
                    else
                    {
                        fields.Add(new Field(ICommands.Fields.FAIL_REASON, failReason));
                        msg.SendResponse(IApp.VALUE_FAILURE, fields, true);
                    }
                    return true;
                case ICommands.REGISTER_PROV_NAMESPACE:
                    OnRegisterNamespace(msg);
                    return true;
                case ICommands.RELOAD_PROVIDER:
                    if(OnReloadProvider(msg, out failReason))
                        msg.SendResponse(IApp.VALUE_SUCCESS);
                    else
                    {
                        fields.Add(new Field(ICommands.Fields.FAIL_REASON, failReason));
                        msg.SendResponse(IApp.VALUE_FAILURE, fields, true);
                    }
                    return true;
                case ICommands.UNINSTALL_PROVIDER:
                    if(OnUninstallProvider(msg, out failReason))
                        msg.SendResponse(IApp.VALUE_SUCCESS);
                    else
                    {
                        fields.Add(new Field(ICommands.Fields.FAIL_REASON, failReason));
                        msg.SendResponse(IApp.VALUE_FAILURE, fields, true);
                    }
                    return true;
                case ICommands.UNHANDLED_EXCEPTION:
                    HandleProviderException(msg);
                    return true;
                case ICommands.REFRESH_PROVIDERS:
                    OnRefreshProviders(msg);
                    return true;
                case ICommands.PRINT_DIAGS:
                    OnPrintDiags();
                    return true;
            }
            return false;
        }

        private bool OnDisableProvider(CommandMessage msg, out string failReason)
        {
            failReason = null;

            string providerName = msg[ICommands.Fields.PROVIDER_NAME] as string;
            if(providerName == null)
            {
                failReason = "No provider name specified in Disable request";
                log.Write(TraceLevel.Error, failReason);
                return false;
            }
            
            ProviderInfo pInfo = providerTable[providerName];
            if(pInfo == null)
            {
                log.Write(TraceLevel.Info, "Disabling unloaded provider: " + providerName);
                configUtility.UpdateStatus(IConfig.ComponentType.Provider, providerName, IConfig.Status.Disabled);
                return true;
            }

            UnloadProvider(pInfo, false);

            pInfo.Status = IConfig.Status.Disabled;
            return true;
        }

        private bool OnEnableProvider(CommandMessage msg, out string failReason)
        {
            string providerName = msg[ICommands.Fields.PROVIDER_NAME] as string;
            if(providerName == null)
            {
                failReason = "No provider name specified in Enable request";
                log.Write(TraceLevel.Error, failReason);
                return false;
            }

            FileInfo providerFile = this.providerFiles[providerName] as FileInfo;
            if(providerFile == null)
            {
                failReason = "Cannot enable provider which is not installed: " + providerName;
                log.Write(TraceLevel.Error, failReason);
                return false;
            }

            // Disabled providers should not be in the provider table
            ProviderInfo pInfo = providerTable[providerName];
            if(pInfo != null)
            {
                // Provider is already loaded
                if(pInfo.Status == IConfig.Status.Enabled_Running)
                {
                    log.Write(TraceLevel.Warning, "Provider already enabled: " + providerName);
                    failReason = null;
                    return true;
                }
                else
                {
                    UnloadProvider(providerName);
                }
            }

            // Mark provider enabled, LoadProvider will set to Running after init complete.
            configUtility.UpdateStatus(IConfig.ComponentType.Provider, providerName, IConfig.Status.Enabled_Stopped);

            if(LoadProvider(providerFile, true, out failReason) == false)
            {
                failReason = "Failed to enable provider: " + providerName;
                log.Write(TraceLevel.Error, failReason);
                return false;
            }

            // Refetch provider info
            pInfo = providerTable[providerName];
            if(pInfo == null)
            {
                failReason = "Failed to enable provider: " + providerName;
                log.Write(TraceLevel.Error, failReason);
                return false;
            }

            failReason = null;
            return true;
        }

        private void OnRegisterNamespace(CommandMessage msg)
        {
            if(msg.SourceType != IConfig.ComponentType.Provider)
            {
                log.Write(TraceLevel.Error, "Only a provider can register an action namespace. Request ignored.");
                DebugLog.MethodExit();
                return;
            }

            string providerNamespace = msg[ICommands.Fields.PROVIDER_NAMESPACE] as string;
            if(providerNamespace == null)
            {
                log.Write(TraceLevel.Warning, "RegisterProviderNamespace received without a " + ICommands.Fields.PROVIDER_NAMESPACE +
                    " field. No namespace has been registered.");
                DebugLog.MethodExit();
                return;
            }

            providerTable.RegisterNamespace(providerNamespace, msg.Source);

            // If this is a CallControl provider, 
            //   forward the registration info along the Telephony Manager
            if(msg.Contains(ICommands.Fields.CC_PROTOCOL))
            {
                tmQ.PostMessage(msg);
            }
        }

        private bool OnReloadProvider(CommandMessage msg, out string failReason)
        {
            string providerName = msg[ICommands.Fields.PROVIDER_NAME] as string;

            if(providerName == null)
            {
                failReason = "No provider name specified in reload provider request.";
                log.Write(TraceLevel.Error, failReason);
                return false;
            }

            if(ReloadProvider(providerName) == false)
            {
                failReason = "Provider failed to reload";
                return false;
            }

            log.Write(TraceLevel.Info, "Provider '{0}' reloaded successfully", providerName);
            failReason = null;
            return true;
        }

        private bool OnInstallProvider(CommandMessage msg, out string failReason)
        {
            failReason = null;
            string providerName = msg[ICommands.Fields.PROVIDER_NAME] as String;
            if(providerName == null)
            {
                failReason = "No provider name specified in install provider request";
                log.Write(TraceLevel.Error, failReason);
                return false;
            }

            FileInfo providerFile = new FileInfo(Path.Combine(this.deployDir.FullName, providerName));

            // Check if it's a package or a lone dll
            if(providerFile.Extension == IPackager.PackFileExt)
            {
                FileInfo packageFile = providerFile;
                try
                {
                    providerFile = Packager.Extract(packageFile, providerDir);
                }
                catch(Exception e)
                {
                    failReason = "Failed to extract provider package: " + e.Message;
                    log.Write(TraceLevel.Error, failReason);

                    // Trigger alarm
                    statsClient.TriggerAlarm(IConfig.Severity.Red, IStats.AlarmCodes.AppServer.ProviderLoadFailure,
                        IStats.AlarmCodes.AppServer.Descriptions.ProviderLoadFailure, packageFile);
                    return false;
                }
                finally
                {
                    try { packageFile.Delete(); }
                    catch { }
                }

                if(!LoadProviderServices(providerFile, out failReason))
                {
                    // Trigger alarm
                    statsClient.TriggerAlarm(IConfig.Severity.Red, IStats.AlarmCodes.AppServer.ProviderLoadFailure,
                        IStats.AlarmCodes.AppServer.Descriptions.ProviderLoadFailure, packageFile);
                    return false;
                }
            }
            else
            {
                string provDirName = providerFile.Name.Replace(providerFile.Extension, "");
                DirectoryInfo targetDir = new DirectoryInfo(Path.Combine(this.providerDir.FullName, provDirName));
                if(!targetDir.Exists)
                {
                    failReason = "The provider directory was not found: " + targetDir.FullName;
                    log.Write(TraceLevel.Error, failReason);

                    // Trigger alarm
                    statsClient.TriggerAlarm(IConfig.Severity.Red, IStats.AlarmCodes.AppServer.ProviderLoadFailure,
                        IStats.AlarmCodes.AppServer.Descriptions.ProviderLoadFailure, providerFile.Name);
                    return false;
                }

                providerFile = new FileInfo(Path.Combine(targetDir.FullName, providerFile.Name));

                if(providerFile.Exists == false)
                {
                    failReason = "The provider file was not found: " + providerFile.FullName;
                    log.Write(TraceLevel.Error, failReason);

                    // Trigger alarm
                    statsClient.TriggerAlarm(IConfig.Severity.Red, IStats.AlarmCodes.AppServer.ProviderLoadFailure,
                        IStats.AlarmCodes.AppServer.Descriptions.ProviderLoadFailure, providerFile.Name);
                    return false;
                }
            }

            if(LoadProvider(providerFile, true, out failReason) == false)
            {
                try { providerFile.Delete(); }
                catch {}

                // Trigger alarm
                statsClient.TriggerAlarm(IConfig.Severity.Red, IStats.AlarmCodes.AppServer.ProviderLoadFailure,
                    IStats.AlarmCodes.AppServer.Descriptions.ProviderLoadFailure, providerFile.Name);
                return false;
            }
            return true;
        }

        private bool OnUninstallProvider(CommandMessage msg, out string failReason)
        {
            failReason = null;

            string providerName = msg[ICommands.Fields.PROVIDER_NAME] as string;

            if(providerName == null)
            {
                failReason = "No provider name specified in uninstall provider request.";
                log.Write(TraceLevel.Error, failReason);
                return false;
            }

            UnloadProvider(providerName);
            
            FileInfo providerFile = this.providerFiles[providerName] as FileInfo;
            if(providerFile != null)
            {
                string dirName = providerFile.Name.Replace(providerFile.Extension, "");
                DirectoryInfo provDir = new DirectoryInfo(Path.Combine(this.providerDir.FullName, dirName));
                if(provDir.Exists)
                {
                    log.Write(TraceLevel.Info, "Uninstalling provider services: " + providerName);

                    if(!UnloadProviderServices(provDir, out failReason))
                        return false;

                    log.Write(TraceLevel.Info, "Deleting provider files: " + providerName);

                    try { provDir.Delete(true); }
                    catch(Exception e)
                    {
                        failReason = String.Format("Failed to delete provider '{0}' files: {1}", providerName, e.Message);
                        log.Write(TraceLevel.Error, failReason);
                        return false;
                    }
                }
            }

            configUtility.RemoveComponent(IConfig.ComponentType.Provider, providerName);

            this.providerFiles.Remove(providerName);
            return true;
        }

        private void OnRefreshProviders(CommandMessage cmd)
        {
            if(this.refreshProvidersCmd != null)
            {
                log.Write(TraceLevel.Error, "Received RefreshProviders command while another is in progress");
                cmd.SendResponse(IApp.VALUE_FAILURE);
                return;
            }

            this.refreshProvidersCmd = cmd;

            // The refresh tally isn't updated as providers are installed/uninstalled
            // Since this isn't a common or time-critical task, just rebuild the list here.
            this.refreshTally.Clear();

            foreach(ProviderInfo pInfo in this.providerTable)
            {
                refreshTally.AddItem(pInfo.Name);

                log.Write(TraceLevel.Verbose, "Refreshing: " + pInfo.Name);

                CommandMessage refreshMsg = 
                    messageUtility.CreateCommandMessage(pInfo.Name, ICommands.REFRESH_CONFIG);

                pInfo.ProviderQ.PostMessage(refreshMsg);
            }
        }

        private void OnPrintDiags()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("[Provider Manager Diagnostics]\r\n");
            if(providerTable.Count > 0)
            {
                foreach(ProviderInfo pInfo in providerTable)
                {
                    sb.AppendFormat("{0}: {1}", pInfo.Name, pInfo.Status);
                    sb.AppendLine();

                    foreach(string ns in providerTable.GetNamespaces(pInfo.Name))
                    {
                        sb.AppendFormat("  - {0}", ns);
                        sb.AppendLine();
                    }
                }
            }
            else
            {
                sb.AppendLine("<No providers installed>");
            }

            log.ForceWrite(TraceLevel.Info, sb.ToString());
        }

        #endregion

        #region Handle Responses
        public void HandleResponseMessage(ResponseMessage msg)
        {
            switch(msg.MessageId)
            {
                case IResponses.PONG:
                    OnPong(msg);
                    break;
                case IResponses.REFRESH_COMPLETE:
                    if(this.refreshProvidersCmd != null)
                    {
                        log.Write(TraceLevel.Verbose, "Provider {0} finished refreshing", msg.Source);

                        this.refreshTally.Check(msg.Source);
                        if(this.refreshTally.AllChecked)
                        {
                            this.refreshProvidersCmd.SendResponse(IResponses.REFRESH_COMPLETE);
                            this.refreshProvidersCmd = null;
                        }
                    }
                    break;
            }
        }

        private void OnPong(ResponseMessage msg)
        {
            providerPings.Pong(msg.Source);
            log.Write(TraceLevel.Verbose, "Got ping response from: " + msg.Source);
        }
        #endregion

        #region Action Routing
        public void RouteAction(ActionMessage msg)
        {
            ProviderInfo pInfo = providerTable.GetByNs(msg.Destination);

            if(pInfo == null || pInfo.ProviderQ == null)
            {
                log.Write(TraceLevel.Warning, "No provider available to handle action: " + msg.MessageId);
                msg.SendResponse(IApp.VALUE_FAILURE);
                return;
            }
            long time = HPTimer.Now();
            pInfo.ProviderQ.PostMessage(msg);
            time = HPTimer.MillisSince(time);
            if(time > 32)
                log.Write(TraceLevel.Warning, "DIAG: RouteAction: time={0}, msg={1}, src={2}, contents follow:\n{3}", 
                    time, msg.MessageId, msg.Source, msg.ToString());
        }
        #endregion 

        #region Provider Runtime Management

        private void HandleProviderException(CommandMessage cmd)
        {
            string providerName = cmd.Source;

            // It's important to lookup the ProviderInfo object here.
            // Trying to handle the exception in the ProviderInfo object
            //   and having it pass itself up in a subsequent event does not work.
            ProviderInfo pInfo = this.providerTable[providerName];
            
            if(pInfo == null)
            {
                log.Write(TraceLevel.Error, "Exception thrown by unknown provider: {0}", providerName);
            }
            else if(pInfo.Name == null)
            {
                log.Write(TraceLevel.Error, "Provider creation failed for: {0}", pInfo.AssemblyFile.Name);
                UnloadProvider(pInfo, true);
            }
            else
            {
                if(pInfo.Status != IConfig.Status.Enabled_Running)
                {
                    // Tell ProviderInitManager to stop waiting for a startup response
                    pInfo.StartupAborted = true;
                    UnloadProvider(pInfo, true);
                }
                else
                {
                    log.Write(TraceLevel.Warning, "Attempting to reload provider: {0}", pInfo.Name);
                    ReloadProvider(pInfo.Name);
                    providerPings.Reset(pInfo.Name);
                }
            }            
        }

        public void PingProviders(object state)
        {
            StringCollection lostProviders = new StringCollection();

            lock(providerTable.SyncRoot)
            {
                foreach(ProviderInfo pInfo in providerTable)
                {
                    if(pInfo.ProviderQ != null)
                    {
                        if(providerPings.Ping(pInfo.Name) == false)
                        {
                            float cpuLoad = PerfMonCounter.GetValue(PerfCounterType.CPU_Load);
                            log.Write(TraceLevel.Warning, "Provider '{0}' failed to respond to ping attempts (CPU={1}%). Reloading...", 
                                pInfo.Name, cpuLoad.ToString());
                            lostProviders.Add(pInfo.Name);
                        }
                        else
                        {
                            log.Write(TraceLevel.Verbose, "Pinging provider: " + pInfo.Name);
                            ActionMessage pingMsg = messageUtility.CreateActionMessage(IActions.Ping, "InternalAction");
                            pingMsg.Destination = pInfo.Name;
                            pInfo.ProviderQ.PostMessage(pingMsg);
                        }
                    }
                    else
                    {
                        log.Write(TraceLevel.Warning, "Message queue writer is null for: " + pInfo.Name);
                    }
                }

                foreach(string providerName in lostProviders)
                {
                    ReloadProvider(providerName);
                    providerPings.Reset(providerName);
                }
            }
        }
        #endregion
    }
}
