using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Collections;

using Metreos.Core;
using Metreos.Core.ConfigData;
using Metreos.Interfaces;
using Metreos.Utilities;
using Metreos.LoggingFramework;

using Metreos.AppServer.Configuration;

namespace Metreos.AppServer.ApplicationManager
{
    /// <summary>A container class for MediaProvisioner queue items</summary>
    internal class MediaProvInfo
    {
        public DirectoryInfo appDir;
        public string appName;

        public MediaProvInfo(DirectoryInfo appDir, string appName)
        {
            this.appDir = appDir;
            this.appName = appName;
        }
    }

	/// <summary>
	/// Runs in its own thread and provisions media to all configured 
	/// media servers via MediaInfo messages on its queue.
	/// </summary>
	public class MediaProvisioner
	{
        // These will probably need to become config items
        private const string MMS_USERNAME   = "media";
        private const string MMS_PASSWORD   = "metreos";

        private const int JOIN_TIMEOUT      = 2000;     // ms

        private LogWriter log;
        private Config configUtility;

        /// <summary>Marshalls media provisioning requests to the internal thread</summary>
        private Queue queue;

        /// <summary>The provisioning thread</summary>
        private Thread provThread;

        /// <summary>Indicates whether the thread should exit</summary>
        private bool shutdown = false;

		public MediaProvisioner(LogWriter log, Config configUtility)
		{
            this.log = log;
            this.configUtility = configUtility;

            this.queue = Queue.Synchronized(new Queue());

            this.provThread = new Thread(new ThreadStart(ProvisionMediaThread));
            this.provThread.Name = "AppManager Media Provisioning thread";
            this.provThread.IsBackground = true;
		}

        public void Start()
        {
            this.provThread.Start();
        }

        public void Stop()
        {
            this.shutdown = true;
            if(this.provThread.Join(JOIN_TIMEOUT) == false)
                this.provThread.Abort();
        }

        /// <summary>
        /// Creates an async request for the media files in the specified 
        /// application directory to be sent to all media servers.
        /// </summary>
        /// <param name="appDir">[AppServerDir]\Applications\[AppName]\[Version]</param>
        /// <param name="appName">The application name</param>
        public void ProvisionFiles(DirectoryInfo appDir, string appName)
        {
            if(appDir == null || appName == null || appName == String.Empty)
            {
                log.Write(TraceLevel.Error, "Internal error: Cannot provision media files.");
                return;
            }

            this.queue.Enqueue(new MediaProvInfo(appDir, appName));
        }

        /// <summary>Sends files to all configured media servers</summary>
        private void ProvisionMediaThread()
        {
            while(shutdown == false)
            {
                if(this.queue.Count == 0)
                {
                    Thread.Sleep(1);
                    continue;
                }

                MediaProvInfo mInfo = this.queue.Dequeue() as MediaProvInfo;
                if(mInfo == null)
                {
                    log.Write(TraceLevel.Warning, "Internal error detected in MediaProvisioner message queue");
                    continue;
                }

                ProvisionMedia(mInfo);
            }
        }

        private void ProvisionMedia(MediaProvInfo mInfo)
        {
            if(mInfo == null)
            {
                log.Write(TraceLevel.Error, "Internal Error: mInfo is null in MediaProvisioner.ProvisionMedia()");
                return;
            }

            string mediaDirStr = Path.Combine(mInfo.appDir.FullName, IConfig.AppDirectoryNames.MEDIA_FILES);

            DirectoryInfo mediaDir = new DirectoryInfo(mediaDirStr);
            if(!mediaDir.Exists)
            {
                log.Write(TraceLevel.Error, "Application package does not contain a MediaFiles directory.");
                return;
            }

            FileInfo[] mediaFiles = mediaDir.GetFiles();
            if(mediaFiles.Length == 0) { return; }

            // If there are media files and no media servers, show warning.
            ComponentInfo[] mediaServers = configUtility.GetComponents(IConfig.ComponentType.MediaServer);
            if(mediaServers == null)
            {
                log.Write(TraceLevel.Warning, "Cannot provision media files for application, no media servers configured");
                return;
            }

            using(MmsCommunicator mmsClient = new MmsCommunicator())
            {
                foreach(ComponentInfo mediaServer in mediaServers)
                {
                    IPAddress msAddr = configUtility.GetEntryValue(IConfig.ComponentType.MediaServer, 
                        mediaServer.name, Database.ConfigEntryNames.ADDRESS) as System.Net.IPAddress;

                    if(msAddr == null)
                    {
                        log.Write(TraceLevel.Warning, "Media Server '{0}' is not fully configured", mediaServer.name);
                        continue;
                    }

                    log.Write(TraceLevel.Verbose, "Connecting to media server: " + msAddr.ToString());

                    // Connect to media server
                    string errorMsg = null;
                    if(mmsClient.Connect(msAddr.ToString(), 0 /* default */, MMS_USERNAME, MMS_PASSWORD, 
                        mInfo.appName, out errorMsg) == false)
                    {
                        log.Write(TraceLevel.Warning, "Could not contact media server '{0}' for media provisioning: {1}",
                            msAddr.ToString(), errorMsg);
                        continue; 
                    }

                    foreach(FileInfo mediaFile in mediaFiles)
                    {
                        log.Write(TraceLevel.Verbose, "Deploying media file: " + mediaFile.Name);

                        if(mmsClient.ProvisionMedia(mediaFile, out errorMsg) == false)
                        {
                            log.Write(TraceLevel.Error, "Failed to upload media file '{0}' to media server ({1}): {2}",
                                mediaFile.Name, msAddr.ToString(), errorMsg);
                        }
                    }

                    mmsClient.Disconnect();
                }
            }

            log.Write(TraceLevel.Info, "All media files for application '{0}' have been deployed", mInfo.appName);
        }
	}
}
