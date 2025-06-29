using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Reflection;

using Metreos.Core;
using Metreos.AppServer.CommonRuntime;

namespace Metreos.AppServer.ServiceRuntime
{
    public class MetreosAppServerService : System.ServiceProcess.ServiceBase
    {
        private const int STARTUP_TIMEOUT   = 240000;
        private const int SHUTDOWN_TIMEOUT  = 240000;

        private ApplicationServer appServer;

        private System.Threading.ManualResetEvent started;
        private System.Threading.ManualResetEvent shutdown;

        private System.ComponentModel.Container components = null;

        public MetreosAppServerService()
        {
            // This call is required by the Windows.Forms Component Designer.
            InitializeComponent();
        }

        // The main entry point for the process
        static void Main()
        {
            System.ServiceProcess.ServiceBase[] ServicesToRun;
	
            // More than one user Service may run within the same process. To add
            // another service to this process, change the following line to
            // create a second service object. For example,
            //
            //   ServicesToRun = New System.ServiceProcess.ServiceBase[] {new Service1(), new MySecondUserService()};
            //
            ServicesToRun = new System.ServiceProcess.ServiceBase[] { new MetreosAppServerService() };

            System.ServiceProcess.ServiceBase.Run(ServicesToRun);
        }

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // MetreosAppServerService
            // 
            this.CanShutdown = true;
            this.ServiceName = "MetreosAppServerService";

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if (components != null) 
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        /// <summary>
        /// Set things in motion so your service can do its work.
        /// </summary>
        protected override void OnStart(string[] args)
        {
            started = new System.Threading.ManualResetEvent(false);
            shutdown = new System.Threading.ManualResetEvent(false);

            string assemblyFile = Assembly.GetExecutingAssembly().Location;
            DirectoryInfo assemblyDir = Directory.GetParent(assemblyFile);

            // Set the working directory to the directory in which the assembly
            // resides. This way, we can avoid any problems with relative paths.
            Directory.SetCurrentDirectory(assemblyDir.FullName);

            try
            {
                appServer = ApplicationServer.Instance;
                appServer.StartLogger(TraceLevel.Off, this.EventLog);
            }
            catch(Exception e)
            {
                throw new ApplicationException(e.Message + ". " +
                    "Odds are that your database connection settings in AppServerService.exe.config are wrong or the database has not been properly initialized");
            }

            appServer.startupComplete += new CommonRuntime.StartupCompleteDelegate(this.StartupCompleteCallback);
            appServer.shutdownComplete += new CommonRuntime.ShutdownCompleteDelegate(this.ShutdownCompleteCallback);
            appServer.startupProgress += new CommonRuntime.StartupProgressDelegate(this.StartupProgressCallback);
            appServer.shutdownProgress += new CommonRuntime.ShutdownProgressDelegate(this.ShutdownProgressCallback);

            if(!appServer.Startup())
                throw new ApplicationException("Failed to start internal components. Re-installation is recommended.");
        }
 
        /// <summary>
        /// Stop this service.
        /// </summary>
        protected override void OnStop()
        {
            appServer.Shutdown();
            appServer.StopLogger();
            appServer.Dispose();

            this.EventLog.Close();
        }

        protected override void OnShutdown()
        {
            OnStop();
        }

        private void StartupCompleteCallback()
        {
            started.Set();
        }

        private void ShutdownCompleteCallback()
        {
            shutdown.Set();
        }

        private void StartupProgressCallback(string progressMessage)
        {
            try { this.EventLog.WriteEntry("Samoa Service (starting): " + progressMessage); }
            catch {}
        }

        private void ShutdownProgressCallback(string progressMessage)
        {
            try { this.EventLog.WriteEntry("Application Server Service (shutdown): " + progressMessage); }
            catch {}
        }
    }
}
