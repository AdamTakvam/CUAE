using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;

namespace AlarmAgentService
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
		private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
		private System.ServiceProcess.ServiceInstaller serviceInstaller;

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
			components = new System.ComponentModel.Container();

			this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
			this.serviceInstaller = new System.ServiceProcess.ServiceInstaller();
			// 
			// serviceProcessInstaller1
			// 
			this.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
			this.serviceProcessInstaller.Password = null;
			this.serviceProcessInstaller.Username = null;
			// 
			// serviceInstaller1
			// 
			this.serviceInstaller.DisplayName = "Cisco UAE Alarm Server";
			this.serviceInstaller.ServiceName = "MetreosAlarmServer";
			this.serviceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
			// 
			// ProjectInstaller
			// 
			this.Installers.AddRange(new System.Configuration.Install.Installer[] {	  this.serviceProcessInstaller,
																					  this.serviceInstaller});
		}
		#endregion
	}
}
