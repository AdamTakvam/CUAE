using System;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.AppArchiveCore;
using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;
using Metreos.AppArchiveCore.Xml;
using Metreos.ApplicationFramework;
using Metreos.ApplicationFramework.Collections;

using Metreos.AppServer.ARE;
using Metreos.Configuration;
using Metreos.AppServer.ApplicationManager.Collections;

namespace Metreos.AppServer.ApplicationManager
{
	public sealed class AppManager : PrimaryTaskBase
	{
        private readonly string fwRootDir;
        private readonly string appRootDir;
        private new Config configUtility;

        /// <summary>Handle to media manager component</summary>
        private MessageQueueWriter mediaManagerQ;

        /// <summary>Collection of application info objects</summary>
        private AppInfoCollection apps;

        /// <summary>Component which handles remote application debugging</summary>
		private DebugServer debugServer;

        /// <summary>Setting permitting or restricting application installation.</summary>
        private bool appInstallEnabled;

        /// <summary>Default culture to use for all newly-installed apps</summary>
        private System.Globalization.CultureInfo defaultCulture;

        /// <summary>Stats/Alarms client connection</summary>
        private Metreos.Stats.StatsClient statsClient;

		public AppManager()
            : base(IConfig.ComponentType.Core, 
                IConfig.CoreComponentNames.APP_MANAGER,
                IConfig.CoreComponentNames.APP_MANAGER,
                Config.AppManager.LogLevel,
                Config.Instance)
		{
            this.configUtility = Config.Instance;
            this.appRootDir = Config.ApplicationDir.FullName;
            this.fwRootDir = configUtility.FrameworkDir.ToString();

            this.apps = new AppInfoCollection();
            this.appInstallEnabled = true;

            this.statsClient = Metreos.Stats.StatsClient.Instance;

            AppInfo.UnhandledException = new AppExceptionDelegate(ExceptionHandler);

			this.debugServer = new DebugServer(Config.AppManager.LogLevel);
            this.debugServer.handleStartDebugging = new DebugStateDelegate(HandleStartDebugging);
			this.debugServer.handleSetBreakpoint = new BreakpointDelegate(HandleSetBreakpoint);
            this.debugServer.handleGetBreakpoints = new DebugStateDelegate(HandleGetBreakpoints);
            this.debugServer.handleClearBreakpoint = new BreakpointDelegate(HandleClearBreakpoint);
			this.debugServer.handleStopDebugging = new DebugStateDelegate(HandleStopDebugging);
            this.debugServer.handleBreak = new DebugStateDelegate(HandleBreak);
			this.debugServer.handleRun = new ExecuteActionDelegate(HandleRun);
            this.debugServer.handleStepInto = new ExecuteActionDelegate(HandleStepInto);
            this.debugServer.handleStepOver = new ExecuteActionDelegate(HandleStepOver);
            this.debugServer.handleUpdateValue = new UpdateValueDelegate(HandleUpdateValue);
		}

        #region Startup/Shutdown/Refresh
        protected override System.Diagnostics.TraceLevel GetLogLevel()
        {
            return Config.AppManager.LogLevel;
        }

        protected override void OnStartup()
        {
            this.mediaManagerQ = MessageQueueFactory.GetQueueWriter(IConfig.CoreComponentNames.MEDIA_MANAGER);
            if(mediaManagerQ == null)
                throw new StartupFailedException("Failed to acquire queue write for MediaManager");

            RefreshConfiguration(null);

            LoadApplicationsFromFileSystem();
			this.debugServer.Start((ushort)Config.AppManager.DebugListenPort);
        }

        protected override void RefreshConfiguration(string proxy)
        {
            if(proxy != null)
            {
                switch(proxy)
                {
                    case IConfig.CoreComponentNames.ARE:
                        RefreshConfigurationOnAllApps();
                        break;
                    default:
                        AppInfo appInfo = apps[proxy];
                        if(appInfo == null) 
                        {
                            log.Write(TraceLevel.Warning, "Received refresh request for an application which is not installed: " + proxy);
                            break;
                        }

                        appInfo.RefreshConfiguration();
                        break;                        
                }
            }
            else
            {
                CommandMessage refreshMsg = messageUtility.CreateCommandMessage(IConfig.CoreComponentNames.MEDIA_MANAGER,
                    ICommands.REFRESH_CONFIG);
                this.mediaManagerQ.PostMessage(refreshMsg);

                // Get default locale config
                string locale = null;
                try
                {
                    locale = 
                        configUtility.GetEntryValue(IConfig.ComponentType.Core, this.Name,
                        IConfig.Entries.Names.DEFAULT_LOCALE) as string;
                    this.defaultCulture = new System.Globalization.CultureInfo(locale);
                }
                catch
                {
                    log.Write(TraceLevel.Warning, "Default locale is invalid: " + locale);
                    this.defaultCulture = new System.Globalization.CultureInfo("en-US");
                }
            }
        }

        private void RefreshConfigurationOnAllApps()
        {
            AppInfo appInfo = null;
            IDictionaryEnumerator de = apps.GetEnumerator();
            while(de.MoveNext())
            {
                appInfo = (AppInfo) de.Value;
     			appInfo.RefreshConfiguration();
            }
        }

        protected override void OnShutdown()
        {
			this.debugServer.Stop();

            log.Write(TraceLevel.Info, "Unloading applications...");
            UnloadAllAppDomains();
        }
        #endregion

        #region Internal Message Handler
        protected override bool HandleMessage(InternalMessage message)
        {
			CommandMessage cMsg = message as CommandMessage;
			if(cMsg != null)
			{
                string failReason;
				switch(cMsg.MessageId)
				{
                    case ICommands.UNINSTALL_APP:
                    {
                        if(OnUninstallApplication(cMsg, out failReason))
                        {
                            cMsg.SendResponse(IApp.VALUE_SUCCESS, null, false);

                            // Tell MediaManager do it can clean up
                            mediaManagerQ.PostMessage(cMsg);
                        }
                        else
                        {
                            ArrayList fields = new ArrayList();
                            fields.Add(new Field(ICommands.Fields.FAIL_REASON, failReason));
                            cMsg.SendResponse(IApp.VALUE_FAILURE, fields, false);
                        }
                        return true;
                    }
                    case ICommands.INSTALL_APP:
                    {
                        string filename = cMsg[ICommands.Fields.FILENAME] as string;
                        if(filename == null)
                        {
                            log.Write(TraceLevel.Error, "Received InstallApplication command with no app filename:\n" + cMsg);
                            return true;
                        }
                        
                        if(HandleInstallApplicationCommand(filename, out failReason))
                        {
                            cMsg.SendResponse(IApp.VALUE_SUCCESS, null, false);
                        }
                        else
                        {
                            ArrayList fields = new ArrayList();
                            fields.Add(new Field(ICommands.Fields.FAIL_REASON, failReason));
                            cMsg.SendResponse(IApp.VALUE_FAILURE, fields, false);
                        }
                        return true;
                    }
                    case ICommands.UPDATE_APP:
                    {
                        string filename = cMsg[ICommands.Fields.FILENAME] as string;
                        if(filename == null)
                        {
                            log.Write(TraceLevel.Error, "Received UpdateApplication command with no app filename:\n" + cMsg);
                            return true;
                        }

                        string appName = cMsg[ICommands.Fields.APP_NAME] as string;
                        
                        if(HandleUpdateApplicationCommand(filename, appName, out failReason))
                        {
                            cMsg.SendResponse(IApp.VALUE_SUCCESS, null, false);
                        }
                        else
                        {
                            ArrayList fields = new ArrayList();
                            fields.Add(new Field(ICommands.Fields.FAIL_REASON, failReason));
                            cMsg.SendResponse(IApp.VALUE_FAILURE, fields, false);
                        }
                        return true;
                    }
                    case ICommands.RELOAD_APP:
                    {
                        if((OnReloadApplication(cMsg) == true) && (cMsg.SourceQueue != null))
                        {
                            cMsg.SendResponse(IApp.VALUE_SUCCESS, null, false);
                        }
                        else if(cMsg.SourceQueue != null)
                        {
                            ArrayList fields = new ArrayList();
                            fields.Add(new Field(ICommands.Fields.FAIL_REASON, "Could not reload application"));
                            cMsg.SendResponse(IApp.VALUE_FAILURE, fields, false);
                        }
                        return true;
                    }
                    case ICommands.ENABLE_APP_INSTALL:
                    {
                        this.appInstallEnabled = true;
                        return true;
                    }
                    case ICommands.DISABLE_APP_INSTALL:
                    {
                        this.appInstallEnabled = false;
                        return true;
                    }
                    case ICommands.HIT_BREAKPOINT:
                    case ICommands.STOP_DEBUGGING:
                    {
                        debugServer.SendCommand(cMsg);
                        return true;
                    }
                    case ICommands.PRINT_DIAGS:
                    {
                        HandlePrintDiags(cMsg.Destination);
                        return true;
                    }
				}
                return false;
			}
			
			ResponseMessage respMsg = message as ResponseMessage;
			if(respMsg != null)
			{
				switch(respMsg.InResponseTo)
				{
					case ICommands.SET_BREAKPOINT:
						SendSetBreakpointResp(respMsg);
						return true;

					case ICommands.STOP_DEBUGGING:
                    case ICommands.START_DEBUGGING:
						SendSimpleResp(respMsg);
						return true;

                    case ICommands.BREAK:
                    case ICommands.RUN:
					case ICommands.EXEC_ACTION:
						SendDetailedResp(respMsg);
						return true;
				}
			}

            return false;
        }

        /// <summary>Prints diagnosic report for all installed applications</summary>
        /// <param name="destination">Not used at this time</param>
        private void HandlePrintDiags(string destination)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("[Application Diagnostics]\r\n");

            if(apps.Count > 0)
            {
                foreach(DictionaryEntry de in apps)
                {
                    AppInfo aInfo = de.Value as AppInfo;
                    if(aInfo != null)
                    {
                        sb.Append(aInfo.GetDiagMessage());
                        sb.Append("\r\n\r\n");
                    }
                }
            }
            else
            {
                sb.Append("* No applications installed *");
            }

            log.ForceWrite(TraceLevel.Info, sb.ToString());
        }

        #endregion

        #region Application Installation

        /// <summary>
        /// Loads all properly configured and enabled applications from the 
        /// filesystem. Applications must be configured in the Samoa OAM 
        /// database to be loaded. Before loading, all applications awaiting
        /// deployment are installed.
        /// </summary>
        private void LoadApplicationsFromFileSystem()
        {
            ComponentInfo[] databaseApps = null;        // Applications configured in database.
            DirectoryInfo[] appsOnFilesystem = null;            // Applications from the file system.
            DirectoryInfo[] filesystemAppVersions = null;       // Application versions from the file system.
            ComponentInfo filesystemAppInfo = null;     // Info for each application from file system.

            // Deploy any applications that are waiting in the deploy directory.
            InstallApplicationsWaitingForDeployment();

            // Populate our database structure with all of the applications that are currently
            // configured in Samoa's OAM database.
            databaseApps = configUtility.GetComponents(IConfig.ComponentType.Application);
            
            try
            {
                // Populate our file system structure with all of the applications currently
                // located on the filesystem within the application directory.
                DirectoryInfo appDir = new DirectoryInfo(appRootDir);
                appsOnFilesystem = appDir.GetDirectories();
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Can not get applications from filesystem: {0}", e.Message);
                return;
            }

            // Component info for filesystem apps.
            ArrayList filesystemAppInfoList = new ArrayList();

            // Scan directory for applications.
            Assertion.Check(appsOnFilesystem != null, "appsOnFilesystem is null");
            int appsOnFileSystemCount = appsOnFilesystem.Length;
            for(int i = 0; i < appsOnFileSystemCount; i++)
            {
                // Grab all of the available application versions from the filesystem.
                filesystemAppVersions = appsOnFilesystem[i].GetDirectories();
                
                int filesystemAppVersionsCount = filesystemAppVersions.Length;
                for(int x = 0; x < filesystemAppVersionsCount; x++)
                {
                    // Build a component info object for each version.
                    filesystemAppInfo = new ComponentInfo();
                    filesystemAppInfo.name = appsOnFilesystem[i].Name;
                    filesystemAppInfo.version = filesystemAppVersions[x].Name;
                    filesystemAppInfoList.Add(filesystemAppInfo);
                }
            }

            int databaseAppsCount = 0;

            // Compare what was found on the filesystem to what is configured in
            // the MCE database and add orphan apps to the "not configured" list
            ArrayList orphanFileSystemApps = new ArrayList();
            for(int i = 0; i < filesystemAppInfoList.Count; i++)
            {
                filesystemAppInfo = (ComponentInfo) filesystemAppInfoList[i];

                bool found = false;

                databaseAppsCount = (databaseApps != null) ? databaseApps.Length : 0;
                for(int x = 0; x < databaseAppsCount; x++)
                {
                    if( (databaseApps[x].name == filesystemAppInfo.name) &&
                        (databaseApps[x].version == filesystemAppInfo.version))
                    {
                        // Database and filesystem match for this app name and version.
                        found = true;
                        break;
                    }
                }
                
                if(found == false)
                {
                    orphanFileSystemApps.Add(filesystemAppInfo);
                }
            }
            
            // At this point, fileAppInfoList contains those applications which are
            // on the filesystem but not configured within the database. Toss some
            // warning messages to indicate this.
            for(int i = 0; i < orphanFileSystemApps.Count; i++)
            {
                filesystemAppInfo = (ComponentInfo) orphanFileSystemApps[i];

                log.Write(TraceLevel.Warning, 
                    "Application {0} version {1} found on filesystem but not configured in database", 
                    filesystemAppInfo.name, filesystemAppInfo.version);
            }

            // Locate enabled apps and start them.
            databaseAppsCount = (databaseApps != null) ? databaseApps.Length : 0;
            for(int i = 0; i < databaseAppsCount; i++)
            {
                if( (databaseApps[i].status != IConfig.Status.Disabled_Error) &&
                    (databaseApps[i].status != IConfig.Status.Unspecified))
                {
                    // Load the application.
                    string errorMsg;
                    if(!LoadApplication(databaseApps[i].name, databaseApps[i].version, out errorMsg))
                    {
                        if(errorMsg != null)
                            log.Write(TraceLevel.Error, errorMsg);
                        else
                            log.Write(TraceLevel.Error, "Failed to load application: " + databaseApps[i].name);
                    }
                }
                else
                {
                    log.Write(TraceLevel.Info, 
                        "Not loading application {0} version {1} because its status is {2}", 
                        databaseApps[i].name, databaseApps[i].version, databaseApps[i].status);
                }
            }
        }

        private bool LoadApplication(string appName, string appVersion, out string errorMsg)
        {
            errorMsg = null;

            // All applications start disabled
            configUtility.UpdateStatus(IConfig.ComponentType.Application, appName, IConfig.Status.Disabled);

            // Verify that the directories exist for those apps
            string appDirStr = null;
            string scriptsDirStr = null;
            string dbDirStr = null;

            DirectoryInfo applicationDir = null;
            DirectoryInfo scriptsDir = null;
            DirectoryInfo dbDir = null;

            try
            {
                appDirStr = Path.Combine(appRootDir, appName);
                appDirStr = Path.Combine(appDirStr, appVersion);
                scriptsDirStr = Path.Combine(appDirStr, IConfig.AppDirectoryNames.SCRIPTS);
                dbDirStr = Path.Combine(appDirStr, "Databases");  
                
                scriptsDir = new DirectoryInfo(scriptsDirStr);
                applicationDir = new DirectoryInfo(appDirStr);
                dbDir = new DirectoryInfo(dbDirStr);
            }
            catch(Exception e)
            {
                errorMsg = String.Format("Unable to inspect application directory for application {0} version {1}. {2}",
                    appName, appVersion, e.Message);
                return false;
            }

            // Throw error if any app cannot be found and mark app Disabled_Error.
            if(applicationDir.Exists == false)
            {
                errorMsg = String.Format("Application '{0}' version {1} installed but cannot be found on filesystem",
                    appName, appVersion);

                configUtility.UpdateStatus(IConfig.ComponentType.Application, appName, IConfig.Status.Disabled_Error);
                return false;
            }

            if(scriptsDir.Exists == false)
            {
                errorMsg = String.Format("Application {0} version {1} scripts directory not found on filesystem.",
                    appName, appVersion);
                log.Write(TraceLevel.Warning, errorMsg);

                configUtility.UpdateStatus(IConfig.ComponentType.Application, appName, IConfig.Status.Disabled_Error);
                return false;
            }

            // Load the manifest from the application directory.
            string manifestPath = Path.Combine(applicationDir.FullName, IAppPackager.DEFAULT_MANIFEST_FILENAME);
            manifestType manifest = 
                ApplicationPackage.LoadManifestFromFile(manifestPath);

            if(manifest == null)
            {
                errorMsg = String.Format("Unable to load manifest for {0} version {1}.", appName, appVersion);
                log.Write(TraceLevel.Error, errorMsg);

                // If the manifest load failed, mark this application "Disabled_Error".
                configUtility.UpdateStatus(IConfig.ComponentType.Application, appName, IConfig.Status.Disabled_Error);
                return false;
            }

            // Load the String Table from the application directory and store in database.
            string stPath = Path.Combine(applicationDir.FullName, IAppPackager.DEFAULT_LOCALES_FILENAME);
            if(File.Exists(stPath))
                StoreStringTable(stPath, appName);

            // Get application script names and internal database names
            List<FileInfo> scriptXmlFiles;
            FileInfo[] dbFiles;
            List<string> dbNames = new List<string>();
            try 
            { 
                scriptXmlFiles = new List<FileInfo>(scriptsDir.GetFiles("*.xml")); 
                dbFiles = dbDir.GetFiles("*.sql");

                if(dbFiles != null)
                {
                    foreach(FileInfo dbFile in dbFiles)
                    {
                        dbNames.Add(Path.GetFileNameWithoutExtension(dbFile.Name));
                    }
                }
            }
            catch(Exception e)
            {
                errorMsg = String.Format("Can not access specific application directory {0}. {1}.",
                    scriptsDir.FullName, e.Message);
                log.Write(TraceLevel.Error, errorMsg);

                // Directory access errors means we need to mark this application "Disabled_Error".
                configUtility.UpdateStatus(IConfig.ComponentType.Application, appName, IConfig.Status.Disabled_Error);
                return false;
            }

            Assertion.Check(scriptXmlFiles != null, "scriptXmlFiles is null");

            // Prepare metadata structure for new app
            AppInfo appInfo = new AppInfo(manifest.summary.name, manifest.summary.version, 
                manifest.summary.frameworkVersion, dbNames, scriptXmlFiles, log);            

            // Create the AppDomain for the application
            if(appInfo.LoadApplication(fwRootDir) == false)
            {
                // Application failed to load.
                errorMsg = String.Format("Application {0} failed to load.", appName);
                configUtility.UpdateStatus(IConfig.ComponentType.Application, appName, IConfig.Status.Disabled_Error);
                return false;
            }

            if(apps.Add(appInfo) == false)
            {
                errorMsg = String.Format("Internal Error: App '{0}' already exists in app table", Name);
                return false;
            }

            log.Write(TraceLevel.Info, "Application {0} loaded successfully.", appName);
            return true;
        }

        private void InstallApplicationsWaitingForDeployment()
        {
            // Set the filter to '*.mca'
            string fileFilter = "*" + IAppPackager.DEFAULT_APP_PACKAGE_EXTENSION;

            DirectoryInfo deployDir = Config.AppDeployDir;

            FileInfo[] apps = deployDir.GetFiles(fileFilter);

            manifestType manifest;

            foreach(FileInfo app in apps)
            {
                InstallApplication(app.FullName, out manifest);
            }
        }

        private bool InstallApplication(string appFilename, out manifestType manifest)
        {
            string errorMsg;
            return InstallApplication(appFilename, out manifest, out errorMsg);
        }

        private bool InstallApplication(string appFilename, out manifestType manifest, out string errorMsg)
        {
            // Outer try/finally block so we can cleanup after ourselves
            // no matter where we exit this method.
            string tempDir = null;
            try
            {
                if(PrepTempInstallDir(ref appFilename, out manifest, out tempDir, out errorMsg) == false)
                    return false;

                if(IsApplicationAlreadyInstalled(manifest.summary.name, manifest.summary.version) == true)
                {
                    errorMsg = String.Format("Unable to install application {0} version {1}. The application is already installed.",
                        manifest.summary.name, manifest.summary.version);
                    log.Write(TraceLevel.Error, errorMsg);
                    return false;
                }

                DirectoryInfo realAppDir = new DirectoryInfo(tempDir);

                // Find the applications directory: <tempdir>\<appname>\<version>\
                while(realAppDir.GetDirectories().Length == 1)
                {
                    // Change to the directory's single sub-directory.
                    realAppDir = new DirectoryInfo(realAppDir.GetDirectories()[0].FullName);
                }

                // Create databases and apply SQL scripts
				if(CreateApplicationDatabases(realAppDir, out errorMsg) == false)
					return false;
                
				// Execute the application's installer
				if(ProcessApplicationInstaller(manifest, realAppDir, false, out errorMsg) == false)
                    return false;
				
				realAppDir = realAppDir.Parent;
				string installDirLocation = Path.Combine(Config.ApplicationDir.FullName, realAppDir.Name);

				try
				{
					if(Directory.Exists(installDirLocation) == true)
					{
						log.Write(TraceLevel.Info, "Removing stale application files from {0}", installDirLocation);

						// These must be stale files because the application 
						// installation check above failed. So lets just remove them.
						Directory.Delete(installDirLocation, true);
					}

					// Move the application's files into place into the "Applications" directory.
					realAppDir.MoveTo(installDirLocation);
				}
				catch(Exception e)
				{
                    errorMsg = "Error while installing application files: "+ e.Message;
					log.Write(TraceLevel.Error, errorMsg);
					return false;
				}

                // Queue media files for provisioning (best effort)
                DirectoryInfo mediaDir = new DirectoryInfo(installDirLocation);
                mediaDir = mediaDir.GetDirectories()[0];
                ProvisionMediaFiles(mediaDir, manifest.summary.name);
            }
            finally
            {
                // Cleanup the application archive file and temporary directory
                // if they still exist.
                try
                {
                    if(File.Exists(appFilename))
                    {
                        // Cleanup the application archive currently in the deploy directory.
                        File.Delete(appFilename);
                    }
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Warning, 
                        "Can not delete application archive {0} from deploy directory. {1}",
                        appFilename, e.Message);
                }
            
                try
                {
                    if(Directory.Exists(tempDir))
                    {
                        // Cleanup the temporary directory that was created
                        // during the deployment process
                        Directory.Delete(tempDir, true);
                    }
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Warning, 
                        "Can not delete temporary directory {0}. {1}", 
                        tempDir, e.Message);
                }
            }
            return true;
        }

        private bool HandleInstallApplicationCommand(string filename, out string failReason)
        {
            manifestType manifest;
            failReason = null;

            if(appInstallEnabled == false)
            {
                failReason = String.Format("Could not install '{0}'. Application installation/uninstallation features are disabled", filename);
                log.Write(TraceLevel.Warning, failReason);
                return false;
            }

            bool success = InstallApplication(filename, out manifest, out failReason);
            if(success)
            {
                success = LoadApplication(manifest.summary.name, manifest.summary.version, out failReason);
            }

            if(success == false)
            {
                Assertion.Check(failReason != null, "The app install failed, but no reason was given");

                log.Write(TraceLevel.Error, "Application '{0}' installation failed: {1}",
                    manifest != null ? manifest.summary.name : filename, failReason);

                // Trigger alarm
                statsClient.TriggerAlarm(IConfig.Severity.Red, IStats.AlarmCodes.AppServer.AppLoadFailure,
                    IStats.AlarmCodes.AppServer.Descriptions.AppLoadFailure, filename);
                
            }
            else
            {
                log.Write(TraceLevel.Info, "Application '{0}' installed successfully.",
                    manifest != null ? manifest.summary.name : filename);
            }

            return success;
        }
        #endregion

        #region Application Updating

        /// <summary>Updates an application to a new version</summary>
        /// <remarks>
        /// What we're doing here is basically uninstalling and then reinstalling the app 
        ///   without ditching the existing configuration values and partitions.
        /// </remarks>
        private bool HandleUpdateApplicationCommand(string appFilename, string appName, out string errorMsg)
        {
            errorMsg = null;

            if(appInstallEnabled == false)
            {
                errorMsg = String.Format("Could not update '{0}'. Application installation/uninstallation features are disabled",
                    appFilename);
                log.Write(TraceLevel.Warning, errorMsg);
                return false;
            }

            string tempDir = null;
            manifestType manifest = null;
            try
            {
                if(PrepTempInstallDir(ref appFilename, out manifest, out tempDir, out errorMsg) == false)
                    return false;

                // Verify that this is in fact a new version of the inteded app
                //   and not something else entirely.
                if(String.Compare(manifest.summary.name, appName, true) != 0)
                {
                    errorMsg = String.Format("Cannot update application '{0}' with the archive for '{1}'",
                        appName, manifest.summary.name);
                    log.Write(TraceLevel.Error, errorMsg);
                    return false;
                }

                if(UninstallApplication(manifest.summary.name, true, out errorMsg) == false)
                {
                    log.Write(TraceLevel.Warning, "Uninstallation of existing application has failed." +
                        "Upgrade will attempt to continue.");
                }

                DirectoryInfo realAppDir = new DirectoryInfo(tempDir);

                // Find the applications directory: <tempdir>\<appname>\<version>\
                while(realAppDir.GetDirectories().Length == 1)
                {
                    // Change to the directory's single sub-directory.
                    realAppDir = new DirectoryInfo(realAppDir.GetDirectories()[0].FullName);
                }

                // Create databases and apply SQL scripts
                if(CreateApplicationDatabases(realAppDir, out errorMsg) == false)
                    return false;

                // Execute the application's installer
                if(ProcessApplicationInstaller(manifest, realAppDir, true, out errorMsg) == false)
                    return false;

                realAppDir = realAppDir.Parent;
                string installDirLocation = Path.Combine(Config.ApplicationDir.FullName, realAppDir.Name);

                try
                {
                    if(Directory.Exists(installDirLocation) == true)
                    {
                        log.Write(TraceLevel.Warning, "Removing stale application files from: " + installDirLocation);
                        Directory.Delete(installDirLocation, true);
                    }

                    // Move the application's files into place into the "Applications" directory.
                    realAppDir.MoveTo(installDirLocation);
                }
                catch(Exception e)
                {
                    errorMsg = "Error while updating application files: "+ e.Message;
                    log.Write(TraceLevel.Error, errorMsg);
                    return false;
                }

                // Queue media files for provisioning (best effort)
                DirectoryInfo mediaDir = new DirectoryInfo(installDirLocation);
                mediaDir = mediaDir.GetDirectories()[0];
                ProvisionMediaFiles(mediaDir, manifest.summary.name);
            }
            finally
            {
                // Cleanup the application archive file and temporary directory
                // if they still exist.
                try
                {
                    if(File.Exists(appFilename))
                    {
                        // Cleanup the application archive currently in the deploy directory.
                        File.Delete(appFilename);
                    }
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Warning, "Can not delete application archive {0} from deploy directory. {1}",
                        appFilename, e.Message);
                }

                try
                {
                    if(Directory.Exists(tempDir))
                    {
                        // Cleanup the temporary directory that was created
                        // during the deployment process
                        Directory.Delete(tempDir, true);
                    }
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Warning, "Can not delete temporary directory {0}. {1}",
                        tempDir, e.Message);
                }
            }

            if(LoadApplication(manifest.summary.name, manifest.summary.version, out errorMsg) == false)
                return false;

            return true;
        }
        #endregion

        #region Application installer file processing

        /// <summary>
        /// Locates the application intaller for an MCE application. If no installer
        /// is located within the archive an empty installer is created for processing.
        /// </summary>
        /// <param name="manifest">The manifest of the application being installed.</param>
        /// <param name="appDir">The directory where the application has been extracted to.</param>
        /// <returns>True if successfull, false otherwise.</returns>
        private bool ProcessApplicationInstaller(manifestType manifest, DirectoryInfo appDir, bool update, 
            out string errorMsg)
        {
            Assertion.Check(manifest != null, "manifest passed to ProcessApplicationInstaller() can not be null");
            Assertion.Check(appDir != null, "appDir passed to ProcessApplicationInstaller() can not be null");
            Assertion.Check(appDir.Exists, "appDir passed to ProcessApplicationInstaller() does not exist");
            errorMsg = null;

            FileInfo installerFile = null;
            installType installer = null;

            string installerFilePath = 
                Path.Combine(appDir.FullName, manifest.summary.name + IAppPackager.INSTALLER_FILE_EXTENSION);

            // Search for an installer that is formatted 
            // according to: <ApplicationName>.installer
            if(File.Exists(installerFilePath) == false)
            {
                // No installer found according to the above
                // formatting convention. Lets check for a
                // deprecated standard in which the installer
                // was named "INSTALLER.xml".
                installerFilePath = 
                    Path.Combine(appDir.FullName, IAppPackager.DEFAULT_INSTALLER_FILENAME);

                if(File.Exists(installerFilePath) == false)
                {
                    // No installer found, there must not be one
                    // in the archive.
                    installerFilePath = null;
                }
            }

            if(installerFilePath != null)
            {
                try
                {
                    // Grab the installer from the location we
                    // checked above. We do this as an added
                    // sanity check.
                    installerFile = new FileInfo(installerFilePath);
                }
                catch(Exception e)
                {
                    errorMsg = "Error while getting the application installer file: " + e.Message;
                    log.Write(TraceLevel.Error, errorMsg);
                    return false;
                }

                // Load the installer up into object form.
                installer = ApplicationPackage.LoadApplicationInstallerFromFile(installerFile.FullName);

                if(installer == null)
                {
                    errorMsg = "Unable to load application installer file from disk.";
                    log.Write(TraceLevel.Error, errorMsg);
                    return false;
                }
            }
            else
            {
                // No installer located inside the application package. Create
                // an empty one with no configuration information.
                installer = new installType();
            }

            // Execute the installer and return the result.
            if(update)
                return UpdateInstaller(installer, manifest, out errorMsg);
            else
                return ExecuteInstaller(installer, manifest, out errorMsg);
        }

        /// <summary>
        /// Updates the application configuration information for a particular application
        /// without deleting or overwriting existing values.
        /// </summary>
        /// <param name="installer">The installer to execute.</param>
        /// <param name="manifest">The manifest for the application being installed.</param>
        /// <returns>True if successful, false otherwise.</returns>
        private bool UpdateInstaller(installType installer, manifestType manifest, out string errorMsg)
        {
            Assertion.Check(installer != null, "installer passed to ExecuteInstaller() is null");
            Assertion.Check(manifest != null, "manifest passed to ExecuteInstaller() is null");
            Assertion.Check(manifest.summary != null, "manifest.summary is null");
            errorMsg = null;

            ComponentInfo cInfo = configUtility.GetComponentInfo(IConfig.ComponentType.Application, manifest.summary.name);
            if(cInfo == null)
            {
                log.Write(TraceLevel.Warning, "No existing configuration information found for application '{0}'. " + 
                    "Proceeding with clean install.", manifest.summary.name);
                return ExecuteInstaller(installer, manifest, out errorMsg);
            }

            cInfo.displayName = manifest.summary.displayName;
            cInfo.version = manifest.summary.version;
            cInfo.description = manifest.summary.description;
            cInfo.author = manifest.summary.author;
            cInfo.copyright = manifest.summary.copyright;
            cInfo.status = IConfig.Status.Disabled;

            // Update the component
            if(configUtility.UpdateComponent(IConfig.ComponentType.Application, cInfo.name, cInfo.displayName,
                cInfo.version, cInfo.description, cInfo.author, cInfo.copyright) == false)
            {
                log.Write(TraceLevel.Warning, "Error while updating component information for application '{0}' " + 
                    "Proceeding with clean install.", cInfo.name);
                return ExecuteInstaller(installer, manifest, out errorMsg);
            }

            // Update configuration data
            if(installer.configuration != null)
            {
                foreach(configurationType configSection in installer.configuration)
                {
                    if(configSection.configValue == null) { continue; }

                    // Keep track of which config entries are staying, 
                    //   so we can prune the ones not present in the update
                    ArrayList remainingConfigs = new ArrayList();

                    foreach(configValueType config in configSection.configValue)
                    {
                        remainingConfigs.Add(config.name);

                        if(configUtility.UpdateEntryMeta(IConfig.ComponentType.Application, manifest.summary.name, 
                            config.name, config.displayName, config.description, config.minValue, config.minValueSpecified,
                            config.maxValue, config.maxValueSpecified, config.readOnly, config.readOnlySpecified,
                            config.required, config.requiredSpecified) == false)
                        {
                            // If updating failed, it's reasonable to assume this is a new value
                            ConfigEntry configEntry = CreateConfigEntry(config, manifest.summary.name, out errorMsg);
                            if(configEntry == null)
                                break;

                            // Add the new config value to the OAM database.
                            if(configUtility.AddEntry(IConfig.ComponentType.Application, manifest.summary.name, 
                                configEntry) == false)
                            {
                                // Something went terribly awry
                                errorMsg = String.Format("Error while executing application installer for {0} version {1}. Can not add config value {2}.",
                                    manifest.summary.name, manifest.summary.version, configEntry.name);
                                log.Write(TraceLevel.Error, errorMsg);
                            }
                        }
                    }

                    // Remove entries not present in update package
                    IDictionary hash = configUtility.GetEntries(IConfig.ComponentType.Application, manifest.summary.name, null);
                    if(hash != null && hash.Count > 0)
                    {
                        foreach(string valueName in hash.Keys)
                        {
                            if(!remainingConfigs.Contains(valueName))
                            {
                                configUtility.RemoveEntry(IConfig.ComponentType.Application, manifest.summary.name, valueName, null);
                            }
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Executes the application installer for a particular application.
        /// By executing the installer configuration entires are made into
        /// Samoa's OAM database.
        /// </summary>
        /// <param name="installer">The installer to execute.</param>
        /// <param name="manifest">The manifest for the application being installed.</param>
        /// <returns>True if successful, false otherwise.</returns>
        private bool ExecuteInstaller(installType installer, manifestType manifest, out string errorMsg)
        {
            Assertion.Check(installer != null, "installer passed to ExecuteInstaller() is null");
            Assertion.Check(manifest != null, "manifest passed to ExecuteInstaller() is null");
            Assertion.Check(manifest.summary != null, "manifest.summary is null");
            errorMsg = null;

            ComponentInfo cInfo = new ComponentInfo();

            cInfo.name = manifest.summary.name;
            cInfo.displayName = manifest.summary.displayName;
            cInfo.version = manifest.summary.version;
            cInfo.description = manifest.summary.description;
            cInfo.author = manifest.summary.author;
            cInfo.copyright = manifest.summary.copyright;
            cInfo.type = IConfig.ComponentType.Application;
            cInfo.status = IConfig.Status.Disabled;

            cInfo.groups = new ComponentGroup[1];
            cInfo.groups[0] = new ComponentGroup();
            cInfo.groups[0].ID = Database.ComponentGroupIds.APPLICATIONS;

            // Add the component
            if(configUtility.AddComponent(cInfo, defaultCulture.IetfLanguageTag) == false)
            {
                errorMsg = String.Format("Error while executing application installer for {0} version {1}. Can not add component in database.", 
                    manifest.summary.name, manifest.summary.version);  
                log.Write(TraceLevel.Error, errorMsg); 
                return false;
            }

            // Insert configuration data
            if(installer.configuration != null)
            {
                foreach(configurationType configSection in installer.configuration)
                {
                    if(configSection.configValue == null) { continue; }

                    foreach(configValueType config in configSection.configValue)
                    {
                        ConfigEntry configEntry = CreateConfigEntry(config, manifest.summary.name, out errorMsg);
                        if(configEntry == null)
                            break;

                        // Add the new config value to the OAM database.
                        if(configUtility.AddEntry(IConfig.ComponentType.Application, manifest.summary.name, configEntry) == false)
                        {
                            // Failure occured, therefore write a log message and attempt to roll back
                            // the installation.
                            errorMsg = String.Format("Error while executing application installer for {0} version {1}. Can not add config value {2}.",
                                manifest.summary.name, manifest.summary.version, configEntry.name);
                            log.Write(TraceLevel.Error, errorMsg);
                        }
                    }
                }

                if(errorMsg != null)
                {
                    // Rollback involves removing the component that we just added to
                    // the OAM database. This will also remove any config values that were
                    // inserted.
                    configUtility.RemoveComponent(IConfig.ComponentType.Application, manifest.summary.name);
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region Application Reloading

        private bool OnReloadApplication(InternalMessage msg)
        {
            string appName = msg.Destination;
            if(appName == null)
            {
                log.Write(TraceLevel.Error, "No application name specified in " + 
                    ICommands.RELOAD_APP + "request.");
                return false;
            }
            return ReloadApplication(appName);
        }


        private bool ReloadApplication(string appName)
        {
            AppInfo appInfo = apps[appName];

            if(appInfo == null)
            {
                log.Write(TraceLevel.Error, "Could not locate application for reloading: " +
                    appName);
                return false;
            }

            log.Write(TraceLevel.Info, "Reloading application: " + appName);

            // Trigger alarm
            statsClient.TriggerAlarm(IConfig.Severity.Yellow, IStats.AlarmCodes.AppServer.AppReloaded,
                IStats.AlarmCodes.AppServer.Descriptions.AppReloaded, appName);

            appInfo.UnloadApplication();

            if(appInfo.LoadApplication(fwRootDir) == false)
            {
                log.Write(TraceLevel.Error, "INTERNAL ERROR: Unable to recreate application type: " + appName);

                // Trigger alarm
                statsClient.TriggerAlarm(IConfig.Severity.Red, IStats.AlarmCodes.AppServer.AppLoadFailure,
                    IStats.AlarmCodes.AppServer.Descriptions.AppLoadFailure, appName);

                return false;
            }

            return true;
        }

        #endregion

        #region Application Uninstallation

        private bool OnUninstallApplication(InternalMessage msg, out string errorMsg)
        {
            errorMsg = null;
            Assertion.Check(msg != null, "Internal message object is null on application uninstall");

            string appName = msg[ICommands.Fields.APP_NAME] as string;
            if(appName == null)
            {
                errorMsg = "No application name specified in uninstall application request.";
                log.Write(TraceLevel.Error, errorMsg);
                return false;
            }

            if(appInstallEnabled == false)
            {
                errorMsg = String.Format("Could not uninstall '{0}'. Application installation/uninstallation features are disabled", appName);
                log.Write(TraceLevel.Warning, errorMsg);
                return false;
            }

            if(UninstallApplication(appName, out errorMsg) == false)
            {
                log.Write(TraceLevel.Error, errorMsg);
                return false;
            }

            return true;
        }

        private bool UninstallApplication(string appName, out string errorMsg)
        {
            return UninstallApplication(appName, false, out errorMsg);
        }

        private bool UninstallApplication(string appName, bool preserveConfig, out string errorMsg)
        {
            errorMsg = null;

            log.Write(TraceLevel.Info, "Uninstalling application {0}", appName);

            bool isSuccess = true;

            ComponentInfo appInfo = configUtility.GetComponentInfo(IConfig.ComponentType.Application, appName);

            if(appInfo != null)
            {
                if((appInfo.status == IConfig.Status.Enabled_Stopped) ||
                (appInfo.status == IConfig.Status.Enabled_Running))
                {
                    errorMsg = String.Format("Unable to uninstall application. Application status is currently {0} and must be {1}. Disable the application first.",
                        appInfo.status, IConfig.Status.Disabled);
                    log.Write(TraceLevel.Error, errorMsg);
                    return false;
                }

                if((apps.Contains(appName) == false) &&
                ((appInfo.status == IConfig.Status.Disabled) || (appInfo.status == IConfig.Status.Disabled_Error)))
                {
                    log.Write(TraceLevel.Info, "Uninstalling currently not loaded application {0}.", appName);
                }
                else
                {
                    isSuccess = UnloadApplication(appName);
                }

                if(preserveConfig == false)
                    isSuccess &= configUtility.RemoveComponent(IConfig.ComponentType.Application, appName);

                if(isSuccess == false)
                    log.Write(TraceLevel.Error, "Unable to fully uninstall application {0}", appName);
            }

            string installedLocation = Path.Combine(Config.ApplicationDir.FullName, appName);

            try
            {
                if(Directory.Exists(installedLocation))
                    Directory.Delete(installedLocation, true);
            }
            catch(Exception e)
            {
                errorMsg = String.Format("Unable to remove {0}'s application files from filesystem. {1}",
                    appName, e.Message);
                log.Write(TraceLevel.Error, errorMsg);
                return false;
            }

            if(isSuccess == true)
            {
                log.Write(TraceLevel.Info, "Application {0} uninstalled successfully.", appName);
            }

            return isSuccess;
        }


        private bool UnloadApplication(string appName)
        {
            AppInfo appInfo = apps[appName];

            if(appInfo != null)
            {
                log.Write(TraceLevel.Info, "Unloading application: {0}", appName);

                // Clean it up
                appInfo.UnloadApplication();

                apps.Remove(appName);
            }

            return true;
        }
        #endregion

        #region Helper methods

        private void ProvisionMediaFiles(DirectoryInfo appDir, string appName)
        {
            CommandMessage msg = messageUtility.CreateCommandMessage(IConfig.CoreComponentNames.MEDIA_MANAGER,
                IMediaManager.Commands.ProvisionMedia);
            msg.AddField(IMediaManager.Fields.AppName, appName);
            msg.AddField(IMediaManager.Fields.MediaDir, appDir);

            this.mediaManagerQ.PostMessage(msg);
        }

        private bool PrepTempInstallDir(ref string appFilename, out manifestType manifest, out string tempDir,
            out string errorMsg)
        {
            manifest = null;
            errorMsg = null;

            FileInfo appFileInfo = new FileInfo(Path.Combine(Config.AppDeployDir.FullName, appFilename));
            appFilename = appFileInfo.FullName;

            log.Write(TraceLevel.Info, "Installing application from package {0}", appFileInfo.Name);

            tempDir = Path.Combine(Config.AppDeployDir.FullName,
                DirectoryUtilities.CreateTemporaryDirectoryName());

            // Make sure the application package has not disappeared on us.
            if(appFileInfo.Exists == false)
            {
                errorMsg = String.Format("Application package {0} does not exist in deployment directory",
                    appFilename);
                log.Write(TraceLevel.Error, errorMsg);
                return false;
            }

            try
            {
                // Create a temporary directory that we will extract the
                // application archive into.
                Directory.CreateDirectory(tempDir);
            }
            catch(Exception e)
            {
                errorMsg = String.Format("Can not create temporary application deploy directory. {0}",
                    e.Message);
                log.Write(TraceLevel.Error, errorMsg);
                return false;
            }

            AppPackagerOptions options = new AppPackagerOptions();
            options.filename = appFilename;
            options.outputDirectory = tempDir;

            try
            {
                // Package extraction includes validation of the manifest against
                // the extracted files.
                manifest = AppPackager.ExtractPackage(options);
            }
            catch(PackagerException e)
            {
                Directory.Delete(tempDir, true);

                errorMsg = String.Format("Can not extract application package {0}. Error: {1}.",
                    appFileInfo.Name, e.ErrorType);
                log.Write(TraceLevel.Error, errorMsg);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Stores the localization string table XML in the database as an XML blob.
        /// The app will read and parse the data when it is loaded or on RefreshConfiguration()
        /// </summary>
        /// <param name="stPath">Full path to string table XML file</param>
        /// <param name="appName">Application name</param>
        private void StoreStringTable(string stPath, string appName)
        {
            try
            {
                StreamReader sr = File.OpenText(stPath);

                ConfigEntry cEntry = new ConfigEntry(IConfig.Entries.Names.STRING_TABLE, sr.ReadToEnd(), null, IConfig.StandardFormat.String, false);
                configUtility.AddEntry(IConfig.ComponentType.Application, appName, cEntry, false);

                sr.Close();
            }
            catch { }
        }

        private bool IsApplicationAlreadyInstalled(string appName, string appVersion)
        {
            Assertion.Check(appName != null, "App name is null");
            Assertion.Check(appVersion != null, "App version is null");
            Assertion.Check(appVersion != "", "App version is empty string");

            ComponentInfo appInfo = configUtility.GetComponentInfo(IConfig.ComponentType.Application, appName);

            if(appInfo == null)
            {
                return false;
            }
            else if(appInfo.status == IConfig.Status.Disabled_Error)
            {
                return false;
            }

            string appDirStr = Path.Combine(Config.ApplicationDir.FullName, appName);
            appDirStr = Path.Combine(appDirStr, appVersion);

            DirectoryInfo appDir = new DirectoryInfo(appDirStr);

            // If the application doesn't exist on the file system then it isn't installed.
            if(appDir.Exists == false)
            {
                return false;
            }

            return true;
        }

        private bool CreateApplicationDatabases(DirectoryInfo appDir, out string errorMsg)
        {
            Assertion.Check(appDir != null, "appDir passed to CreateApplicationDatabases() can not be null");
            Assertion.Check(appDir.Exists, "appDir passed to CreateApplicationDatabases() does not exist");
            errorMsg = null;

            string dbDirStr = Path.Combine(appDir.FullName, IConfig.AppDirectoryNames.DATABASES);

            DirectoryInfo dbDir = new DirectoryInfo(dbDirStr);
            if(!dbDir.Exists)
            {
                errorMsg = "Application package does not contain a database directory.";
                log.Write(TraceLevel.Error, errorMsg);
                return false;
            }

            foreach(FileInfo sqlFile in dbDir.GetFiles("*.sql"))
            {
                string dbName = sqlFile.Name.Replace(".sql", "");

                if(configUtility.DatabaseExists(dbName))
                {
                    configUtility.DatabaseDrop(dbName);
                }

                StreamReader sqlReader = null;
                try
                {
                    configUtility.DatabaseCreate(dbName);

                    using(IDbConnection appDb = configUtility.DatabaseConnect(dbName))
                    {
                        sqlReader = new StreamReader(sqlFile.FullName);

                        using(IDbCommand command = appDb.CreateCommand())
                        {
                            command.CommandText = sqlReader.ReadToEnd();
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch(Exception e)
                {
                    errorMsg = "Error while initializing application database. " + Exceptions.FormatException(e);
                    log.Write(TraceLevel.Error, errorMsg);
                    return false;
                }
                finally
                {
                    if(sqlReader != null)
                        sqlReader.Close();
                }
            }
            return true;
        }

        private ConfigEntry CreateConfigEntry(configValueType config, string appName, out string errorMsg)
        {
            errorMsg = null;

            if(config == null || config.name.Length == 0)
            {
                errorMsg = "Invalid entry in installer file";
                return null;
            }

            ConfigEntry configEntry = null;

            bool required = config.requiredSpecified ? config.required : ConfigEntry.Defaults.Required;

            if(config.EnumItem == null)
            {
                IConfig.StandardFormat formatName;

                if(DetermineStandardFormat(config.format, out formatName) == false)
                {
                    errorMsg = String.Format("Invalid standard config value format '{0}' for value '{1}' encountered while installing: {2}",
                        config.format, config.name, appName);
                    log.Write(TraceLevel.Error, errorMsg);
                    return null;
                }

                try
                {
                    configEntry = new ConfigEntry(config.name, config.displayName, config.defaultValue,
                        config.description, formatName, required);
                }
                catch(ArgumentException e)
                {
                    errorMsg = "Failed to add configuration entry to Application Server database: " + e.Message;
                    log.Write(TraceLevel.Error, errorMsg);
                    return null;
                }
            }
            else
            {
                StringCollection enumValues = new StringCollection();
                enumValues.AddRange(config.EnumItem);

                try
                {
                    FormatType format = new FormatType(config.format, null, enumValues);
                    configEntry = new ConfigEntry(config.name, config.displayName, config.defaultValue, config.description, format, required);
                }
                catch(Exception e)
                {
                    errorMsg = String.Format("Failed to install config value '{0}': {1}",
                        config.name, e.Message);
                    log.Write(TraceLevel.Error, errorMsg);
                    return null;
                }
            }

            configEntry.displayName = config.displayName;
            configEntry.minValue = config.minValue;
            configEntry.maxValue = config.maxValue;
            configEntry.readOnly = config.readOnlySpecified ? config.readOnly : ConfigEntry.Defaults.ReadOnly;

            return configEntry;
        }

        private bool DetermineStandardFormat(string formatName, out IConfig.StandardFormat format)
        {
            format = IConfig.StandardFormat.String;

            if(formatName == null || formatName == String.Empty)
                return false;

            // Translate common types to our database format types
            switch(formatName.ToLower())
            {
                case "integer":
                case "int":
                case "uint":
                case "double":
                case "long":
                case "ulong":
                case "short":
                case "ushort":
                    format = IConfig.StandardFormat.Number;
                    break;
                case "boolean":
                    format = IConfig.StandardFormat.Bool;
                    break;
                case "arraylist":
                    format = IConfig.StandardFormat.Array;
                    break;
                default:
                    try
                    {
                        format = (IConfig.StandardFormat) Enum.Parse(typeof(IConfig.StandardFormat),
                            formatName, true);
                    }
                    catch { return false; }
                    break;
            }

            return true;
        }

        /// <summary>Unloads all of the application script domains.</summary>
        private void UnloadAllAppDomains()
        {
            AppInfo appInfo;
            IDictionaryEnumerator de = apps.GetEnumerator();
            while(de.MoveNext())
            {
                appInfo = de.Value as AppInfo;

                try { appInfo.UnloadApplication(); }
                catch(Exception) { }
            }

            apps.Clear();
        }
        #endregion

        #region Application Runtime Management

        private void ExceptionHandler(string appName, UnhandledExceptionEventArgs e)
        {
            string eMessage = "<No Exception Text>";
            if(e != null && e.ExceptionObject != null)
            {
                eMessage = Exceptions.FormatException(e.ExceptionObject as Exception);
                configUtility.LastChildDomainException = (Exception) e.ExceptionObject;
            }

            AppInfo appInfo = apps[appName];

            if(!apps.Contains(appName))
            {
                log.Write(TraceLevel.Error, "Exception thrown by unknown application '{0}': {1}", appName, eMessage);
            }
            else
            {
                log.Write(TraceLevel.Error, "Application '{0}' threw an exception: {1}", appName, eMessage);
                log.Write(TraceLevel.Warning, "Attempting to reload application: {0}", appName);
                ReloadApplication(appName);
            }
        }
        #endregion

        #region Debug command message proxying

        #region Incoming Commands

        internal bool HandleStartDebugging(string appName, string scriptName, string transId, out string failReason)
        {
            failReason = String.Empty;

            AppInfo appInfo = apps[appName];
            if(appInfo == null) 
            {
                failReason = String.Format("Application '{0}' is not installed on this server", appName);
                log.Write(TraceLevel.Warning, failReason);
                return false; 
            }

            CommandMessage msg = CreateCommandMessage(scriptName, ICommands.START_DEBUGGING);
            msg.AddField(ICommands.Fields.TRANS_ID, transId);

            appInfo.PostMessage(msg);
            return true;
        }

		internal bool HandleSetBreakpoint(string appName, string scriptName, string actionId, string transId, out string failReason)
		{
			failReason = String.Empty;

			AppInfo appInfo = apps[appName];
			if(appInfo == null) 
			{
				failReason = String.Format("Application '{0}' is not installed on this server", appName);
				log.Write(TraceLevel.Warning, failReason);
				return false; 
			}

			CommandMessage msg = CreateCommandMessage(scriptName, ICommands.SET_BREAKPOINT);
			msg.AddField(ICommands.Fields.DEBUG_ACTION_ID, actionId);
			msg.AddField(ICommands.Fields.TRANS_ID, transId);

			appInfo.PostMessage(msg);
			return true;
		}

        internal bool HandleClearBreakpoint(string appName, string scriptName, string actionId, string transId, out string failReason)
        {
            failReason = String.Empty;

            AppInfo appInfo = apps[appName];
            if(appInfo == null) 
            {
                failReason = String.Format("Application '{0}' is not installed on this server", appName);
                log.Write(TraceLevel.Warning, failReason);
                return false; 
            }

            CommandMessage msg = CreateCommandMessage(scriptName, ICommands.CLEAR_BREAKPOINT);
            msg.AddField(ICommands.Fields.DEBUG_ACTION_ID, actionId);
            msg.AddField(ICommands.Fields.TRANS_ID, transId);

            appInfo.PostMessage(msg);
            return true;
        }

        internal bool HandleGetBreakpoints(string appName, string scriptName, string transId, out string failReason)
        {
            failReason = String.Empty;

            AppInfo appInfo = apps[appName];
            if(appInfo == null) 
            {
                failReason = String.Format("Application '{0}' is not installed on this server", appName);
                log.Write(TraceLevel.Warning, failReason);
                return false; 
            }

            CommandMessage msg = CreateCommandMessage(scriptName, ICommands.GET_BREAKPOINTS);
            msg.AddField(ICommands.Fields.TRANS_ID, transId);

            appInfo.PostMessage(msg);
            return true;
        }

        internal bool HandleBreak(string appName, string scriptName, string transId, out string failReason)
        {
            failReason = String.Empty;

            AppInfo appInfo = apps[appName];
            if(appInfo == null) 
            {
                failReason = String.Format("Application '{0}' is not installed on this server", appName);
                log.Write(TraceLevel.Warning, failReason);
                return false; 
            }

            CommandMessage msg = CreateCommandMessage(scriptName, ICommands.BREAK);
            msg.AddField(ICommands.Fields.TRANS_ID, transId);

            appInfo.PostMessage(msg);
            return true;
        }

		internal bool HandleStopDebugging(string appName, string scriptName, string transId, out string failReason)
		{
			failReason = String.Empty;

			AppInfo appInfo = apps[appName];
			if(appInfo == null) 
			{
                // Let it slide
				return true; 
			}

			CommandMessage msg = CreateCommandMessage(scriptName, ICommands.STOP_DEBUGGING);
			msg.AddField(ICommands.Fields.TRANS_ID, transId);

			appInfo.PostMessage(msg);
			return true;
		}

		internal bool HandleRun(string appName, string scriptName, string actionId, string transId, out string failReason)
		{
			failReason = String.Empty;

			AppInfo appInfo = apps[appName];
			if(appInfo == null) 
			{
				failReason = String.Format("Application '{0}' is not installed on this server", appName);
				log.Write(TraceLevel.Warning, failReason);
				return false; 
			}

			CommandMessage msg = CreateCommandMessage(scriptName, ICommands.RUN);
			msg.AddField(ICommands.Fields.DEBUG_ACTION_ID, actionId);
			msg.AddField(ICommands.Fields.TRANS_ID, transId);

			appInfo.PostMessage(msg);
			return true;
		}

        internal bool HandleStepInto(string appName, string scriptName, string actionId, string transId, out string failReason)
        {
            failReason = String.Empty;

            AppInfo appInfo = apps[appName];
            if(appInfo == null) 
            {
                failReason = String.Format("Application '{0}' is not installed on this server", appName);
                log.Write(TraceLevel.Warning, failReason);
                return false; 
            }

            CommandMessage msg = CreateCommandMessage(scriptName, ICommands.EXEC_ACTION);
            msg.AddField(ICommands.Fields.DEBUG_ACTION_ID, actionId);
            msg.AddField(ICommands.Fields.TRANS_ID, transId);
            msg.AddField(ICommands.Fields.STEP_INTO, true);

            appInfo.PostMessage(msg);
            return true;
        }

        internal bool HandleStepOver(string appName, string scriptName, string actionId, string transId, out string failReason)
        {
            failReason = String.Empty;

            AppInfo appInfo = apps[appName];
            if(appInfo == null) 
            {
                failReason = String.Format("Application '{0}' is not installed on this server", appName);
                log.Write(TraceLevel.Warning, failReason);
                return false; 
            }

            CommandMessage msg = CreateCommandMessage(scriptName, ICommands.EXEC_ACTION);
            msg.AddField(ICommands.Fields.DEBUG_ACTION_ID, actionId);
            msg.AddField(ICommands.Fields.TRANS_ID, transId);
            msg.AddField(ICommands.Fields.STEP_INTO, false);

            appInfo.PostMessage(msg);
            return true;
        }

        internal bool HandleUpdateValue(string appName, string scriptName, Hashtable funcVars, Hashtable scriptVars, string transId, out string failReason)
        {
            failReason = String.Empty;

            AppInfo appInfo = apps[appName];
            if(appInfo == null) 
            {
                failReason = String.Format("Application '{0}' is not installed on this server", appName);
                log.Write(TraceLevel.Warning, failReason);
                return false; 
            }

            // Get variable name and value
            string varName = null;
            object varValue = null;

            if(funcVars != null)
            {
                if(funcVars.Count > 1)
                {
                    failReason = "Only one variable value can be changed at a time";
                    return false;
                }
                else if(funcVars.Count == 1)
                {
                    IDictionaryEnumerator de = funcVars.GetEnumerator();
                    de.MoveNext();

                    varName = de.Key as string;
                    varValue = de.Value;

                    if(varName == null)
                    {
                        failReason = "No variable name specified";
                        return false;
                    }
                }
            }
            else if(scriptVars != null)
            {
                if(scriptVars.Count > 1)
                {
                    failReason = "Only one variable value can be changed at a time";
                    return false;
                }
                else if(scriptVars.Count == 1)
                {
                    IDictionaryEnumerator de = scriptVars.GetEnumerator();
                    de.MoveNext();

                    varName = de.Key as string;
                    varValue = de.Value;

                    if(varName == null)
                    {
                        failReason = "No variable name specified";
                        return false;
                    }
                }
            }

            if(varName == null)
            {
                failReason = "No variable specified for updating";
                return false;
            }

            CommandMessage cMsg = CreateCommandMessage(scriptName, ICommands.UPDATE_VALUE);
            cMsg.AddField(ICommands.Fields.TRANS_ID, transId);
            cMsg.AddField(ICommands.Fields.VAR_NAME, varName);
            cMsg.AddField(ICommands.Fields.VAR_VALUE, varValue);

            appInfo.PostMessage(cMsg);
            return true;
        }

        #endregion

        #region Outbound Responses

		private void SendSetBreakpointResp(ResponseMessage respMsg)
		{
			string transId = respMsg[ICommands.Fields.TRANS_ID] as string;
			Assertion.Check(transId != null, "Transaction ID is null in SetBreakpoint response");

			string currAction = respMsg[ICommands.Fields.DEBUG_ACTION_ID] as string;

			if(respMsg.MessageId == IApp.VALUE_SUCCESS)
			{
				Assertion.Check(currAction != null, "Current action is null in SetBreakpoint success response");
				debugServer.SendSuccessResponse(transId, currAction);
			}
			else
			{
				debugServer.SendFailureResponse(transId, respMsg[ICommands.Fields.FAIL_REASON] as string);
			}
		}

		private void SendSimpleResp(ResponseMessage respMsg)
		{
			string transId = respMsg[ICommands.Fields.TRANS_ID] as string;
			Assertion.Check(transId != null, "Transaction ID is null in Simple response");

			if(respMsg.MessageId == IApp.VALUE_SUCCESS)
			{
				debugServer.SendSuccessResponse(transId, null);
			}
			else
			{
				debugServer.SendFailureResponse(transId, respMsg[ICommands.Fields.FAIL_REASON] as string);
			}
		}

		private void SendDetailedResp(ResponseMessage respMsg)
		{
			string transId = respMsg[ICommands.Fields.TRANS_ID] as string;
			Assertion.Check(transId != null, "Transaction ID is null in Detailed response");

			if(respMsg.MessageId == IApp.VALUE_SUCCESS)
			{
				string nextAction = respMsg[ICommands.Fields.DEBUG_ACTION_ID] as string;
				Assertion.Check(nextAction != null, "Next action ID is null in Detailed success response");

				string resultStr = respMsg[ICommands.Fields.ACTION_RESULT] as string;
				Assertion.Check(nextAction != null, "Result is null in Detailed success response");

				Hashtable funcVars = respMsg[ICommands.Fields.FUNCTION_VARS] as Hashtable;
				Assertion.Check(funcVars != null, "Function variable collection is null in Detailed success response");

				Hashtable scriptVars = respMsg[ICommands.Fields.SCRIPT_VARS] as Hashtable;
				Assertion.Check(scriptVars != null, "Script variable collection is null in Detailed success response");

				SessionData sData = respMsg[ICommands.Fields.SESSION_DATA] as SessionData;
				Assertion.Check(sData != null, "SessionData is null in Detailed success response");

				Stack callStack = respMsg[ICommands.Fields.ACTION_STACK] as Stack;

				debugServer.SendResponse(true, resultStr, null, transId, nextAction, funcVars, scriptVars, sData, callStack);
			}
			else
			{
				debugServer.SendFailureResponse(transId, respMsg[ICommands.Fields.FAIL_REASON] as string);
			}
		}
        #endregion

		#endregion
	}
}
