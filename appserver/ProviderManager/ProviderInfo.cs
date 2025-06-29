using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ProviderFramework;
using Metreos.Messaging;
using Metreos.Utilities;

using Metreos.Configuration;

namespace Metreos.AppServer.ProviderManager
{
    /// <summary>
    /// This class keeps track of the AppDomain information for a single provider.
    /// It is responsible for creating and destroying the domain, but not sending
    ///   startup and shutdown messages.
    /// </summary>
    [Serializable]
	internal sealed class ProviderInfo
	{
        public abstract class Consts
        {
            public const string ProviderFrameworkFilename   = "Metreos.ProviderFramework.dll";
            public const string ProviderFactoryTypeName     = "Metreos.ProviderFramework.ProviderFactory";
        }

        private Config configUtility;
        private LogWriter log;

        private string name;
        public string Name { get { return name; } }

        private FileInfo assemblyFile;
        public FileInfo AssemblyFile { get { return assemblyFile; } }
        
        private IProvider provider;
        public IProvider Provider { get { return provider; } }

        private AppDomain appDomain;
        public AppDomain AppDomain { get { return appDomain; } }

        private bool startupAborted = false;
        public bool StartupAborted 
        { 
            get { return startupAborted; }
            set { startupAborted = value; }
        }

        public IConfig.Status Status
        {
            get { return configUtility.GetStatus(IConfig.ComponentType.Provider, Name); }
            set { configUtility.UpdateStatus(IConfig.ComponentType.Provider, Name, value); }
        }

        private MessageQueueWriter providerQ;
        public MessageQueueWriter ProviderQ { get { return providerQ; } }

        public bool Running { get { return this.Status == IConfig.Status.Enabled_Running; } }

        public ProviderInfo(LogWriter log, FileInfo assemblyFile)
        {
            this.log = log;
            this.configUtility = Config.Instance;
            this.assemblyFile = assemblyFile;

            // Adjust status in case AppServer terminated badly
            if(Status == IConfig.Status.Enabled_Running)
                Status = IConfig.Status.Enabled_Stopped;
        }

        public bool CreateProvider(MessageQueueWriter routerQ)
        {
            bool disabled;
            string failReason;
            return CreateProvider(routerQ, out disabled, out failReason);
        }

        public bool CreateProvider(MessageQueueWriter routerQ, out bool disabled, out string failReason)
        {
            disabled = false;
            failReason = null;

            AssemblyName assemblyName = new AssemblyName();
            assemblyName.CodeBase = assemblyFile.FullName;
            assemblyName.Name = assemblyFile.Name.Replace(".dll", "");

            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = assemblyFile.DirectoryName;
            setup.ApplicationName = assemblyName.Name;

            // Configure shadow copying
            // Shadow copy directory = CachePath + ProviderName;
            setup.ShadowCopyFiles = "true";
            setup.CachePath = Config.CacheDir.FullName;

            setup.PrivateBinPath = String.Format("{0};{1}", 
                AppDomain.CurrentDomain.RelativeSearchPath,
                assemblyFile.DirectoryName);

            try
            {
                this.appDomain = AppDomain.CreateDomain("Provider-" + assemblyName.Name, null, setup);

                string fwFullName = Path.Combine(configUtility.FrameworkVersionDir.FullName, IConfig.FwDirectoryNames.CORE);
                fwFullName = Path.Combine(fwFullName, Consts.ProviderFrameworkFilename);
                ProviderFactory factory = (ProviderFactory) appDomain.CreateInstanceFromAndUnwrap(fwFullName, 
                    Consts.ProviderFactoryTypeName);

                // Call Control providers need a reference to the call ID factory. All others do not.
                object[] ccpArgs = new object[] { Config.Instance, CallIdFactory.Instance };
                object[] args = new object[] { Config.Instance };
                
                this.provider = factory.Create(assemblyName, ccpArgs, args);
                this.provider.SetRouterQueueWriter(routerQ);
                this.name = provider.GetName();

                // Now that we finally have the real name of the provider,
                //   check to see if it is disabled
                IConfig.Status pStatus = configUtility.GetStatus(IConfig.ComponentType.Provider, name);
                if( pStatus == IConfig.Status.Disabled ||
                    pStatus == IConfig.Status.Disabled_Error)
                {
                    disabled = true;
                    return true;  // Not an error case
                }
            }
            catch(Exception e)
            {
                failReason = String.Format("Unable to create provider from {0}: {1}", assemblyFile.Name, Exceptions.FormatException(e));
                log.Write(TraceLevel.Error, failReason);
                DestroyProvider();
                return false;
            }
            return true;
        }

        public bool InitializeProvider(out string failReason)
        {
            failReason = null;

            try
            {
                MessageQueueFactory.RegisterQueue(this.Name, this.provider.GetMessageQueue() as MessageQueue);
                this.providerQ = MessageQueueFactory.GetQueueWriter(this.name);

                if(!provider.InitializeProvider(Logger.Instance))
                {
                    failReason = "Unable to initialize provider: " + name;
                    return false;
                }
            }
            catch(Exception e)
            {
                failReason = String.Format("Unable to initialize provider '{0}': {1}", name, e.Message);
                log.Write(TraceLevel.Error, failReason);
                DestroyProvider();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Search an assembly for valid provider implementations.
        /// </summary>
        /// <param name="a">The assembly to search.</param>
        /// <param name="typeNames">(out) The type names of valid provider implementations.</param>
        /// <returns></returns>
        private string GetProviderTypeName(Assembly a)
        {
            Assertion.Check(a != null, "GetProviderTypeName: Assembly is null");

            foreach(System.Type t in a.GetTypes())
            {
                if(t.IsClass == true)
                {                    
                    foreach(System.Attribute attr in t.GetCustomAttributes(false))
                    {
                        if(attr is ProviderDeclAttribute)
                        {
                            return t.FullName;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>Gracefully cleans up provider and unloads its AppDomain</summary>
        public void DestroyProvider()
        {
            DestroyProvider(false);
        }

        /// <summary>Cleans up provider and unloads its AppDomain</summary>
        /// <param name="violent">Indicates that provider Dispose() should not be called</param>
        public void DestroyProvider(bool violent)
        {
            if(this.Status == IConfig.Status.Enabled_Running)
                this.Status = IConfig.Status.Enabled_Stopped;

            if(providerQ != null)
            {
                providerQ.Dispose();
                providerQ = null;
            }

            if(!violent && provider != null)
            {
                try { provider.Dispose(); }
                catch {}

                provider = null;
            }

            assemblyFile = null;

            if(appDomain != null)
            {
                try
                {
                    AppDomain.Unload(appDomain);
                }
                catch(CannotUnloadAppDomainException e)
                {
                    log.Write(TraceLevel.Warning, "Could not unload AppDomain for '{0}': {1}", Name, e.Message);
                }
                catch(Exception) {}

                appDomain = null;
            }
        }
	}
}
