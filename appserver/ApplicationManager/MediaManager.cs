using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

using Metreos.Core;
using Metreos.Core.ConfigData;
using Metreos.Interfaces;
using Metreos.Utilities;
using Metreos.Messaging;
using Metreos.LoggingFramework;
using Metreos.AppServer.ApplicationManager.Collections;

using Metreos.Configuration;

namespace Metreos.AppServer.ApplicationManager
{
    #region MediaProvInfo

    /// <summary>A container class for media provisioning data sent to internal thread</summary>
    internal class MediaProvInfo
    {
        public readonly string appName;

        public readonly MediaFileCollection audioFiles;
        public readonly MediaFileCollection grammarFiles;

        public readonly MediaProvStatus provStatus;

        public int NumFiles { get { return audioFiles.Count + grammarFiles.Count; } }

        /// <summary>Aggregates handles to all media files for an application</summary>
        /// <param name="appDir">AppServer\Applications\%appName%\%appVersion%\</param>
        /// <param name="appName">Application name</param>
        public MediaProvInfo(DirectoryInfo appDir, string appName)
        {
            Assertion.Check(appDir != null, "Cannot create MediaProvInfo with null appDir");

            this.appName = appName;
            this.provStatus = new MediaProvStatus();

            // Get audio file handles
            DirectoryInfo[] mediaDirs = appDir.GetDirectories(IConfig.AppDirectoryNames.MEDIA_FILES);
            if(mediaDirs != null && mediaDirs.Length == 1)
                this.audioFiles = new MediaFileCollection(mediaDirs[0]);
            else
                this.audioFiles = new MediaFileCollection(null);

            // Get grammar file handles
            mediaDirs = appDir.GetDirectories(IConfig.AppDirectoryNames.VOICE_REC_FILES);
            if(mediaDirs != null && mediaDirs.Length == 1)
                this.grammarFiles = new MediaFileCollection(mediaDirs[0]);
            else
                this.grammarFiles = new MediaFileCollection(null);
        }
    }
    #endregion

    #region MediaProvStatus

    internal class MediaProvStatus
    {
        /// <summary>Keeps track of last provisioning error per MMS</summary>
        private Hashtable errors;
        public Hashtable Errors { get { return errors; } }

        public int numMediaServers;
        public int numFiles;
        public int numDeployed;

        public MediaProvStatus()
        {
            this.numDeployed = 0;
            this.numFiles = 0;
            this.numMediaServers = 0;

            this.errors = Hashtable.Synchronized(new Hashtable());
        }

        public float GetProgress()
        {
            if(numMediaServers == 0 || numFiles == 0)
                return 1;

            return (float) numDeployed / (numFiles * numMediaServers);
        }
    }
    #endregion

    /// <summary>
    /// Runs in its own thread and provisions media to all configured 
    /// media servers via MediaInfo messages on its queue.
    /// </summary>
    public class MediaManager : PrimaryTaskBase
    {
        #region Constants

        internal abstract class Consts
        {
            // These will probably need to become config items
            public const string MediaUsername   = "media";
            public const string GrammarUsername = "grammars";
            public const int SftpServerPort     = 22;

            public const int JoinTimeout        = 2000;         // ms
            public const string ErrorKey        = "Error";      // Reserved entry in errors table
            public const string SnapshotApp     = "_Snapshot_"; // App name used when populating a new MMS

            public const uint MediaEntryMetaID  = 39;

            public abstract class Defaults
            {
                public const string MmsPassword = "metreos";
                public const string Locale      = "en-US";
            }
        }

        private enum MediaType
        {
            Audio,
            Grammar
        }
        #endregion

        /// <summary>appName (string) -> MediaProvInfo</summary>
        private Hashtable appMediaInfo;

        /// <summary>Media provisioning thread</summary>
        private Thread provThread;

        /// <summary>Queue for communicating with media provisioning thread</summary>
        /// <remarks>Contains MediaProvInfo objects only</remarks>
        private Queue queue;

        /// <summary>Indicates that media provisioning thread should exit</summary>
        private bool shutdown = false;

        private readonly ConfigEntry MediaEntry;

        /// <summary>Stats/Alarms client connection</summary>
        private Metreos.Stats.StatsClient statsClient;

        #region Construction/Startup/Refresh/Shutdown

        public MediaManager()
            : base(IConfig.ComponentType.Core,
                IConfig.CoreComponentNames.MEDIA_MANAGER,
                IConfig.CoreComponentNames.MEDIA_MANAGER,
                Config.AppManager.LogLevel,
                Config.Instance)
        {
            this.appMediaInfo = Hashtable.Synchronized(new Hashtable());
            this.queue = Queue.Synchronized(new Queue());

            this.provThread = new Thread(new ThreadStart(ProvisionMediaThread));
            this.provThread.Name = "Media Provisioning thread";
            this.provThread.IsBackground = true;

            this.statsClient = Metreos.Stats.StatsClient.Instance;

            this.MediaEntry = new ConfigEntry(IConfig.Entries.Names.HAS_MEDIA, true,
                null, IConfig.StandardFormat.Bool, false);
            this.MediaEntry.metaID = Consts.MediaEntryMetaID;
        }

        protected override void OnStartup()
        {
            this.provThread.Start();
        }

        protected override void OnShutdown()
        {
            this.shutdown = true;
            lock(this.queue.SyncRoot)
            {
                Monitor.Pulse(this.queue.SyncRoot);
            }

            if(this.provThread.Join(Consts.JoinTimeout) == false)
                this.provThread.Abort();
        }

        protected override void RefreshConfiguration(string proxy)
        {
            // For now, we're piggy-backing our log level on AppManager's
            this.log.LogLevel = Config.AppManager.LogLevel;

            // Learn about currently installed application media
            if(appMediaInfo.Count == 0)
                InitializeAppMediaInfo();

            // Deploy media to any new media servers
            ComponentInfo[] mediaServers = configUtility.GetComponents(IConfig.ComponentType.MediaServer);
            if(mediaServers == null) { return; }

            bool success = true;

            foreach(ComponentInfo mediaServer in mediaServers)
            {
                bool hasMedia = configUtility.GetEntryValue(IConfig.ComponentType.MediaServer,
                    mediaServer.name, IConfig.Entries.Names.HAS_MEDIA) != null;
                if(!hasMedia)
                {
                    lock(appMediaInfo.SyncRoot)
                    {
                        foreach(MediaProvInfo mInfo in appMediaInfo.Values)
                        {
                            // Set up stats (not that it really matters)
                            mInfo.provStatus.numDeployed = 0;
                            mInfo.provStatus.numMediaServers = 1;
                            mInfo.provStatus.numFiles = mInfo.NumFiles;
                            
                            log.Write(TraceLevel.Info, "Deploying media for application '{0}' to new media server: {1}", 
                                mInfo.appName, mediaServer.name);

                            success &= ProvisionMedia(mInfo, mediaServer);
                        }
                    }
                }
            }

            if(success)
                log.Write(TraceLevel.Info, "All media servers are up to date");
        }

        private void InitializeAppMediaInfo()
        {
            DirectoryInfo appRootDir = Config.ApplicationDir;
            foreach(DirectoryInfo appDir in appRootDir.GetDirectories())
            {
                string appName = appDir.Name;
                
                // Go into version directory
                DirectoryInfo[] dirs = appDir.GetDirectories();
                if(dirs != null)
                {
                    double version = 0;
                    DirectoryInfo versionDir = null;
                    foreach(DirectoryInfo vDir in dirs)
                    {
                        try
                        {
                            double currVersion = double.Parse(vDir.Name);
                            if(currVersion > version)
                            {
                                version = currVersion;
                                versionDir = vDir;
                            }
                        }
                        catch {}
                    }
                    
                    if(versionDir != null)
                        appMediaInfo[appName] = new MediaProvInfo(versionDir, appName);
                }
            }
        }

        protected override TraceLevel GetLogLevel()
        {
            return Config.AppManager.LogLevel;
        }
        #endregion

        #region Message handlers

        protected override bool HandleMessage(InternalMessage message)
        {
            if(message is CommandMessage)
            {
                switch(message.MessageId)
                {
                    case IMediaManager.Commands.ProvisionMedia:
                        HandleProvisionMedia(message as CommandMessage);
                        return true;
                    case IMediaManager.Commands.GetStatus:
                        HandleGetStatus(message as CommandMessage);
                        return true;
                    case ICommands.UNINSTALL_APP:
                        HandleAppUninstalled(message as CommandMessage);
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates an async request for the media files in the specified 
        /// application directory to be sent to all media servers.
        /// </summary>
        private void HandleProvisionMedia(CommandMessage message)
        {
            string appName = message[IMediaManager.Fields.AppName] as string;
            DirectoryInfo mediaDir = message[IMediaManager.Fields.MediaDir] as DirectoryInfo;

            if(mediaDir == null || mediaDir.Exists == false || appName == null || appName == String.Empty)
            {
                log.Write(TraceLevel.Error, "Internal error: Cannot provision media files.");
                return;
            }

            lock(this.queue.SyncRoot)
            {
                this.queue.Enqueue(new MediaProvInfo(mediaDir, appName));
                Monitor.Pulse(this.queue.SyncRoot);
            }
        }

        private void HandleGetStatus(CommandMessage message)
        {
            string errorStr = null;
            ArrayList fields = new ArrayList();

            string appName = message[IMediaManager.Fields.AppName] as string;
            if(appName == null || appName == String.Empty)
            {
                errorStr = "No application name specified in provisioning status request";
                fields.Add(new Field(IMediaManager.Fields.Error, errorStr));
                message.SendResponse(IApp.VALUE_FAILURE, fields, true);
                return;
            }

            MediaProvInfo mInfo = this.appMediaInfo[appName] as MediaProvInfo;
            if(mInfo == null)
            {
                errorStr = "Received provisioning status request for unknown application: " + appName;
                log.Write(TraceLevel.Warning, errorStr);
                fields.Add(new Field(IMediaManager.Fields.Error, errorStr));
                message.SendResponse(IApp.VALUE_FAILURE, fields, true);
                return;
            }

            fields.Add(new Field(IMediaManager.Fields.Progress, mInfo.provStatus.GetProgress()));

            lock(mInfo.provStatus.Errors.SyncRoot)
            {
                foreach(DictionaryEntry de in mInfo.provStatus.Errors)
                {
                    string mmsName = de.Key as String;
                    string msg = de.Value as String;

                    if(mmsName == null || mmsName == String.Empty || msg == null || msg == String.Empty)
                        errorStr = "Unknown error";
                    else
                        errorStr = String.Format("{0}: {1}", mmsName, msg);

                    fields.Add(new Field(IMediaManager.Fields.Error, errorStr));
                }
            }

            message.SendResponse(IApp.VALUE_SUCCESS, fields, true);
        }

        private void HandleAppUninstalled(CommandMessage message)
        {
            string appName = message[IMediaManager.Fields.AppName] as string;
            if(appName == null || appName == String.Empty)
            {
                message.SendResponse(IApp.VALUE_FAILURE);
                return;
            }

            this.appMediaInfo.Remove(appName);
        }
        #endregion

        #region Media provisioning thread

        /// <summary>Sends files to all configured media servers</summary>
        private void ProvisionMediaThread()
        {
            while(shutdown == false)
            {
                if(this.queue.Count > 0)
                {
                    MediaProvInfo mInfo = this.queue.Dequeue() as MediaProvInfo;
                    appMediaInfo[mInfo.appName] = mInfo;

                    ProvisionMedia(mInfo);
                }

                lock(this.queue.SyncRoot)
                {
                    Monitor.Wait(this.queue.SyncRoot);
                }
            }
        }

        private bool ProvisionMedia(MediaProvInfo mInfo)
        {
            if(mInfo == null)
            {
                log.Write(TraceLevel.Error, "Internal Error: mInfo is null in MediaProvisioner.ProvisionMedia()");
                return false;
            }

            if(mInfo.NumFiles == 0)
                return true;

            // If there are media files and no media servers, show warning.
            ComponentInfo[] mediaServers = configUtility.GetComponents(IConfig.ComponentType.MediaServer);
            if(mediaServers == null)
            {
                log.Write(TraceLevel.Warning, "Cannot provision media files for application, no media servers configured");
                return true;
            }

            // Set up stats
            mInfo.provStatus.numDeployed = 0;
            mInfo.provStatus.numMediaServers = mediaServers.Length;
            mInfo.provStatus.numFiles = mInfo.NumFiles;

            bool success = true;
            foreach(ComponentInfo mediaServer in mediaServers)
            {
                success &= ProvisionMedia(mInfo, mediaServer);
            }

            // Just to make sure (paranoia)
            mInfo.provStatus.numDeployed = mInfo.NumFiles * mediaServers.Length;

            if(success)
                log.Write(TraceLevel.Info, "All media files for application '{0}' have been deployed", mInfo.appName);

            return success;
        }

        private bool ProvisionMedia(MediaProvInfo mInfo, ComponentInfo mediaServer)
        {
            bool success = true;

            foreach(string locale in mInfo.audioFiles.GetLocales())
            {
                // Don't bother deploying media to a server that's already errored-out
                if(!mInfo.provStatus.Errors.Contains(mediaServer.name))
                {
                    success &= ProvisionMedia(mInfo, mediaServer, MediaType.Audio, locale);
                }
            }

            foreach(string locale in mInfo.grammarFiles.GetLocales())
            {
                FileInfo[] grammarFiles = mInfo.grammarFiles.GetFiles(locale);

                if(!mInfo.provStatus.Errors.Contains(mediaServer.name))
                {
                    success &= ProvisionMedia(mInfo, mediaServer, MediaType.Grammar, locale);
                }
            }

            if(success)
                log.Write(TraceLevel.Verbose, "All files have been deployed for application '{0}' to: {1}", mInfo.appName, mediaServer.name);

            return success;
        }

        private bool ProvisionMedia(MediaProvInfo mInfo, ComponentInfo mediaServer, MediaType type, string locale)
        {
            Assertion.Check(mediaServer != null, "No media server specified in ProvisionMedia");

            FileInfo[] mediaFiles;
            if(type == MediaType.Audio)
                mediaFiles = mInfo.audioFiles.GetFiles(locale);
            else
                mediaFiles = mInfo.grammarFiles.GetFiles(locale);

            if(mediaFiles == null || mediaFiles.Length == 0)
                return true;

            string errorMsg = null;

            IPAddress msAddr = configUtility.GetEntryValue(IConfig.ComponentType.MediaServer,
                mediaServer.name, IConfig.Entries.Names.ADDRESS) as System.Net.IPAddress;

            string password = configUtility.GetEntryValue(IConfig.ComponentType.MediaServer,
                mediaServer.name, IConfig.Entries.Names.PASSWORD) as string;

            // temp
            if(password == null)
            {
                log.Write(TraceLevel.Warning, "No MMS password configured for '{0}'. Using default", mediaServer.name);
                password = Consts.Defaults.MmsPassword;
            }

            if(msAddr == null)
            {
                errorMsg = String.Format("Media Server '{0}' is not fully configured", mediaServer.name);
                log.Write(TraceLevel.Warning, errorMsg);
                mInfo.provStatus.Errors[mediaServer.name] = errorMsg;
                return false;
            }

            log.Write(TraceLevel.Verbose, "Connecting to media server: " + mediaServer.name);

            // MD5 Hash the password
            password = Security.EncryptPassword(password);

            using(MmsCommunicator mmsClient = new MmsCommunicator())
            {
                // Connect to media server
                string username = null;
                if(type == MediaType.Audio)
                    username = Consts.MediaUsername;
                else if(type == MediaType.Grammar)
                    username = Consts.GrammarUsername;

                if(!mmsClient.Connect(msAddr.ToString(), Consts.SftpServerPort, username, password,
                    mInfo.appName, locale, out errorMsg))
                {
                    errorMsg = String.Format("Could not contact media server '{0}' for media provisioning: {1}",
                        msAddr.ToString(), errorMsg);
                    log.Write(TraceLevel.Warning, errorMsg);
                    mInfo.provStatus.Errors[mediaServer.name] = errorMsg;
                    mmsClient.Disconnect();

                    // Trigger alarm
                    statsClient.TriggerAlarm(IConfig.Severity.Red, IStats.AlarmCodes.AppServer.MediaDeployFailure,
                        IStats.AlarmCodes.AppServer.Descriptions.MediaDeployFailure, mInfo.appName, mediaServer.name);

                    return false;
                }

                bool alarmTriggered = false;
                foreach(FileInfo mediaFile in mediaFiles)
                {
                    log.Write(TraceLevel.Verbose, "Deploying {0} file '{1}' to: {2}",
                        type == MediaType.Audio ? "audio" : "grammar", mediaFile.Name, mediaServer.name);

                    if(mmsClient.ProvisionMedia(mediaFile, out errorMsg) == false)
                    {
                        errorMsg = String.Format("Failed to upload file '{0}' to media server ({1}): {2}",
                            mediaFile.Name, mediaServer.name, errorMsg);
                        log.Write(TraceLevel.Warning, errorMsg);
                        mInfo.provStatus.Errors[mediaServer.name] = errorMsg;

                        if(!alarmTriggered)
                        {
                            // Trigger alarm
                            statsClient.TriggerAlarm(IConfig.Severity.Red, IStats.AlarmCodes.AppServer.MediaDeployFailure,
                                IStats.AlarmCodes.AppServer.Descriptions.MediaDeployFailure, mInfo.appName, mediaServer.name);
                            alarmTriggered = true;
                        }
                    }

                    mInfo.provStatus.numDeployed++;
                }

                // Success! Mark it provisioned.
                if(errorMsg == null)
                    configUtility.AddEntry(IConfig.ComponentType.MediaServer, mediaServer.name, MediaEntry);

                mmsClient.Disconnect();
            }
            return errorMsg == null;
        }
        #endregion
    }
}