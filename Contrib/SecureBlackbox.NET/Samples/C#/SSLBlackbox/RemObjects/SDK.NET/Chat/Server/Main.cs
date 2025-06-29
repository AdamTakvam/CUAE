using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using SBUtils;
using SBX509;
using SBCustomCertStorage;
using SSLConnection;
using RemObjects.InternetPack;

namespace ChatServer
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
		private RemObjects.SDK.PoweredByButton poweredByButton1;
		private RemObjects.SDK.Server.MemorySessionManager memorySessionManager;
		private RemObjects.SDK.Server.MemoryMessageQueueManager memoryMessageQueueManager;
		private RemObjects.SDK.BinMessage binMessage;
		private RemObjects.SDK.Server.IpHttpServerChannel serverChannel;
		private RemObjects.SDK.Server.EventSinkManager eventSinkManager;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"));
			SSLServerConnectionFactory factory = new SSLServerConnectionFactory();
			serverChannel.HttpServer.ConnectionFactory = factory;

			// assign the event handler for validating client-side certificates
			factory.OnCertificateValidate += new SBServer.TSBCertificateValidateEvent(factory_OnCertificateValidate);
			//factory.Passthrough = true;

			// setup server-side certificate
			factory.CertStorage = new TElMemoryCertStorage();

			string fn = "";
			if (File.Exists("cert.pfx"))
				fn = "cert.pfx";
			else if (File.Exists("../../cert.pfx"))
				fn = "../../cert.pfx";
				
			if (fn != "")
			{
				FileStream stream = new FileStream(fn, FileMode.Open);
				byte[] buffer = new byte[stream.Length];
				stream.Read(buffer, 0, buffer.Length);
				factory.CertStorage.LoadFromBufferPFX(buffer, "password");
				stream.Close();
			}
		}

		private void factory_OnCertificateValidate(object Sender, SBX509.TElX509Certificate X509Certificate, ref bool Validate)
		{
			Validate = true;
			// NEVER do this in real life since this makes security void. 
			// Instead validate the certificate as described on http://www.eldos.com/sbb/articles/1966.php
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.memorySessionManager = new RemObjects.SDK.Server.MemorySessionManager(this.components);
			this.memoryMessageQueueManager = new RemObjects.SDK.Server.MemoryMessageQueueManager();
			this.binMessage = new RemObjects.SDK.BinMessage();
			this.eventSinkManager = new RemObjects.SDK.Server.EventSinkManager();
			this.poweredByButton1 = new RemObjects.SDK.PoweredByButton();
			this.serverChannel = new RemObjects.SDK.Server.IpHttpServerChannel(this.components);
			((System.ComponentModel.ISupportInitialize)(this.serverChannel)).BeginInit();
			this.SuspendLayout();
			// 
			// eventSinkManager
			// 
			this.eventSinkManager.Message = this.binMessage;
			// 
			// poweredByButton1
			// 
			this.poweredByButton1.ApplicationType = RemObjects.SDK.ApplicationType.Server;
			this.poweredByButton1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.poweredByButton1.Location = new System.Drawing.Point(62, 18);
			this.poweredByButton1.Name = "poweredByButton1";
			this.poweredByButton1.Size = new System.Drawing.Size(212, 48);
			this.poweredByButton1.TabIndex = 0;
			this.poweredByButton1.Text = "poweredByButton1";
			// 
			// serverChannel
			// 
			this.serverChannel.Active = true;
			this.serverChannel.Dispatchers.Add(new RemObjects.SDK.Server.MessageDispatcher("bin", this.binMessage));
			// 
			// xserverChannel.HttpServer
			// 
			this.serverChannel.HttpServer.Port = 8099;
			this.serverChannel.HttpServer.ServerName = "RemObjects SDK for .NET HTTP Server/3.0";
			this.serverChannel.HttpServer.ValidateRequests = false;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(336, 85);
			this.Controls.Add(this.poweredByButton1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "RemObjects SDK for .NET - Chat Server";
			((System.ComponentModel.ISupportInitialize)(this.serverChannel)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}
	}
}
