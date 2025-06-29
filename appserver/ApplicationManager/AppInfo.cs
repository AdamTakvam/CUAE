using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

using Metreos.Utilities;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Configuration;
using Metreos.LoggingFramework;

using Metreos.AppServer.ARE;

namespace Metreos.AppServer.ApplicationManager
{
	/// <summary>Information about and actions performed on an application</summary>
	internal sealed class AppInfo
	{
        #region Constants: AppDomain Setup Info

        private abstract class Consts
        {
            // Parameters for AppDomain creation
            public const string AssemblyName    = "Metreos.AppServer.ARE";
            public const string AppLoader       = "Metreos.AppServer.ARE.AppEnvironment";
        }

        #endregion

        public static AppExceptionDelegate UnhandledException;

        private LogWriter log;

        /// <summary>Basic data about this application</summary>
        private AppMetaData metaData;

        /// <summary>Reference to the primary class in the AppDomain</summary>
        private AppEnvironment appEnvironment = null;

        /// <summary>The AppDomain in which the application executes</summary>
        private AppDomain appDomain = null;

        public string Name { get { return metaData.Name; } }
        public string Version { get { return metaData.Version; } }
        public string FwVersion { get { return metaData.FwVersion; } }
        public List<string> Databases { get { return metaData.Databases; } }
        public List<FileInfo> ScriptFiles { get { return metaData.ScriptFiles; } }

        public AppInfo(string name, string version, string fwVersion, List<string> databases, 
            List<FileInfo> scriptFiles, LogWriter log)
        {
            Assertion.Check(scriptFiles != null, "Attempted to create AppInfo with null scriptFiles");
            Assertion.Check(version != null, "Attempted to create AppInfo with null version");
            Assertion.Check(fwVersion != null, "Attempted to create AppInfo with null framework version");
            Assertion.Check(name != null, "Attempted to create AppInfo with no name");

            this.log = log;

            this.metaData = new AppMetaData(name, version, fwVersion, databases, scriptFiles);
        }

        #region App Communication

        public void RefreshConfiguration()
        {
            if(appEnvironment != null)
                appEnvironment.RefreshConfiguration();
        }

        public void PostMessage(InternalMessage msg)
        {
            if(appEnvironment != null)
                appEnvironment.PostMessage(msg);
        }

        public string GetDiagMessage()
        {
            if(appEnvironment != null)
                return appEnvironment.GetDiagMessage();
            else
                return "(error)";
        }

        #endregion

        #region Application Loading/Unloading

        /// <summary>
        /// Creates a System.AppDomain for each individual application XML script.
        /// </summary>
        /// <param name="appInfo">Information about the application</param>
        /// <returns></returns>
        public bool LoadApplication(string fwRootDir)
        {
            // Prepare setup info for new AppDomain
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            
            // Configure shadow copying
            // Shadow copy directory = CachePath + ApplicationName;
            setup.ShadowCopyFiles = "true";
            setup.ApplicationName = Name;
            setup.CachePath = Config.CacheDir.FullName;

            // Set search paths for application-included code
            string fwLibDir = Path.Combine(fwRootDir, IConfig.FwDirectoryNames.CORE);
            string fwTypesDir = Path.Combine(fwRootDir, IConfig.FwDirectoryNames.TYPES);

            string appDir = Path.Combine(IConfig.AppServerDirectoryNames.APPS, Name);
            appDir = Path.Combine(appDir, Version);
            string libDir = Path.Combine(appDir, IConfig.AppDirectoryNames.LIBS);
            string actionDir = Path.Combine(appDir, IConfig.AppDirectoryNames.ACTIONS);
            string typesDir = Path.Combine(appDir, IConfig.AppDirectoryNames.TYPES);

            setup.PrivateBinPath = String.Format("{0};{1};{2};{3};{4};{5}", 
                AppDomain.CurrentDomain.RelativeSearchPath, 
                fwLibDir, fwTypesDir, libDir, actionDir, typesDir);

            MessageQueueWriter routerQueue = null;
            try
            {
                // AppDomain is named: "Application: <appName> (<guid>)"
                //   the guid prevents an AppDomain from being recreated with the same name 
                //   in the event that a previous installation failed to unload the domain.
                string domainName = String.Format("Application: {0} ({1})", Name, Guid.NewGuid().ToString());
                this.appDomain = AppDomain.CreateDomain(domainName, null, setup);

                this.appEnvironment = (AppEnvironment) this.appDomain.CreateInstanceAndUnwrap(
                    Consts.AssemblyName, Consts.AppLoader, true, System.Reflection.BindingFlags.CreateInstance, null, 
                    new object[] {Config.Instance}, System.Globalization.CultureInfo.CurrentCulture, null, null);
                
                this.appEnvironment.UnhandledException += AppInfo.UnhandledException;
                routerQueue = MessageQueueFactory.GetQueueWriter(IConfig.CoreComponentNames.ROUTER);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Warning, "Could not create application environment (probably a library version problem): {0}", 
                    Exceptions.FormatException(e));
                return false;
            }

            // Note: Marshalling a serializable data structure across an AppDomain boundary
            MessageQueueWriter telManQ = MessageQueueFactory.GetQueueWriter(IConfig.CoreComponentNames.TEL_MANAGER);
            if(this.appEnvironment.Initialize(this.metaData, routerQueue, telManQ, Logger.Instance) == false)
            {
                UnloadApplication();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Unloads an individual application script AppDomain.
        /// </summary>
        /// <param name="di">The domain information for the domain to be unloaded.</param>
        /// <param name="scriptGuid">
        /// GUID of the script to be unloaded. If null, the domain will still be unloaded
        /// but the domain will not be removed from scriptAppDomains.
        /// </param>
        /// <returns>True if successfull, false otherwise.</returns>
        public void UnloadApplication()
        {
            if(this.appEnvironment != null)
            {
                this.appEnvironment.Shutdown();
            }

            if(this.appDomain != null)
            {
                try
                {
                    AppDomain.Unload(this.appDomain);
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Warning, "Error occurred during AppDomain unload for '{0}': {1}", Name, e.Message);
                }
            }
        }
        #endregion
	}
}
