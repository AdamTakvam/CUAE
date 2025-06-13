using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;

namespace Metreos.LogServer
{
	/// <summary>
	/// Summary description for ServiceInstaller.
	/// </summary>
	[RunInstaller(true)]
	public class ServiceInstaller : System.Configuration.Install.Installer
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
		private System.ServiceProcess.ServiceInstaller serviceInstaller1;

		public ServiceInstaller()
		{
			// This call is required by the Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.DisplayName = "Cisco UAE Log Server";
            this.serviceInstaller1.ServiceName = "MetreosLogServer";
            this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.serviceInstaller1.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller1_AfterInstall);
            // 
            // ServiceInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
                                                                                      this.serviceProcessInstaller1,
                                                                                      this.serviceInstaller1});

        }
		#endregion

        private void serviceInstaller1_AfterInstall(object sender, System.Configuration.Install.InstallEventArgs e)
        {
        
        }
	}
}
