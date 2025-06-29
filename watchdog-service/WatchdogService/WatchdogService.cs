using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Configuration;

using Metreos.LogSinks;
using Metreos.Configuration;
using Metreos.WatchdogService;
using Metreos.Interfaces;
using Metreos.Utilities;
using Metreos.LoggingFramework;

namespace Metreos.WatchdogService
{
	public class WatchdogService : System.ServiceProcess.ServiceBase
	{
		private System.ComponentModel.Container components = null;
	
		private readonly Watchdog watchdog;

		public WatchdogService()
		{
			// This call is required by the Windows.Forms Component Designer.
			InitializeComponent();   
        
            this.watchdog = new Watchdog();
		}

		// The main entry point for the process
		static void Main()
		{
			System.ServiceProcess.ServiceBase[] ServicesToRun;
	
			// More than one user Service may run within the same process. To add
			// another service to this process, change the following line to
			// create a second service object. For example,
			//
			//   ServicesToRun = new System.ServiceProcess.ServiceBase[] {new Service1(), new MySecondUserService()};
			//
			ServicesToRun = new System.ServiceProcess.ServiceBase[] { new WatchdogService() };

			System.ServiceProcess.ServiceBase.Run(ServicesToRun);
		}

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			this.ServiceName = "Cisco UAE WatchDog";
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
            EventLogSink els = new EventLogSink(EventLog, TraceLevel.Warning);

			watchdog.Start();
		}
 
		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
			watchdog.Dispose();
		}
    }
}
