using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.Net.Sockets;
using SBCustomCertStorage;
using SBSimpleSSL;

namespace SSLClientDemo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		const int recvBufferSize = 16384;
		private bool bUseSSL2;
		private bool bUseSSL3;
		private bool bUseTLS1;
		private bool bUseTLS11;
		private bool bUseOnlyExportable;
		private bool bAllowAnonymous;
		private bool bSSLEnabled;
		//private bool bSimpleMode;
		private TElMemoryCertStorage clientCerts;
		private string host;
		private int port;

		private int certIndex;

		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem mnuConnection;
		private System.Windows.Forms.MenuItem mnuConnect;
		private System.Windows.Forms.MenuItem mnuDisconnect;
		private System.Windows.Forms.MenuItem mnuSeparator;
		private System.Windows.Forms.MenuItem mnuExit;
		private System.Windows.Forms.MenuItem mnuTools;
		private System.Windows.Forms.MenuItem mnuSSLOptions;
		private System.Windows.Forms.MenuItem mnuHelp;
		private System.Windows.Forms.MenuItem mnuAbout;
		private System.Windows.Forms.ToolBar toolBar;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.ToolBarButton btnConnect;
		private System.Windows.Forms.ToolBarButton btnDisconnect;
		private System.Windows.Forms.ToolBarButton btnSeparator;
		private System.Windows.Forms.ToolBarButton btnSSLOptions;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		private SBSimpleSSL.TElSimpleSSLClient SecureClient;
		private System.Windows.Forms.TextBox tbData;
		private System.Windows.Forms.ImageList imageList;
		private System.ComponentModel.IContainer components;

		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			InitializeApp();
		}

		protected void InitializeApp()
		{
			bUseSSL2 = false;
			bUseSSL3 = true;
			bUseTLS1 = true;
			bUseTLS11 = true;
			bUseOnlyExportable = false;
			bAllowAnonymous = false;
            clientCerts = new TElMemoryCertStorage();
			SBUtils.Unit.SetLicenseKey("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D");
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmMain));
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.mnuConnection = new System.Windows.Forms.MenuItem();
			this.mnuConnect = new System.Windows.Forms.MenuItem();
			this.mnuDisconnect = new System.Windows.Forms.MenuItem();
			this.mnuSeparator = new System.Windows.Forms.MenuItem();
			this.mnuExit = new System.Windows.Forms.MenuItem();
			this.mnuTools = new System.Windows.Forms.MenuItem();
			this.mnuSSLOptions = new System.Windows.Forms.MenuItem();
			this.mnuHelp = new System.Windows.Forms.MenuItem();
			this.mnuAbout = new System.Windows.Forms.MenuItem();
			this.toolBar = new System.Windows.Forms.ToolBar();
			this.btnConnect = new System.Windows.Forms.ToolBarButton();
			this.btnDisconnect = new System.Windows.Forms.ToolBarButton();
			this.btnSeparator = new System.Windows.Forms.ToolBarButton();
			this.btnSSLOptions = new System.Windows.Forms.ToolBarButton();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
			this.SecureClient = new SBSimpleSSL.TElSimpleSSLClient();
			this.tbData = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuConnection,
																					 this.mnuTools,
																					 this.mnuHelp});
			// 
			// mnuConnection
			// 
			this.mnuConnection.Index = 0;
			this.mnuConnection.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.mnuConnect,
																						  this.mnuDisconnect,
																						  this.mnuSeparator,
																						  this.mnuExit});
			this.mnuConnection.Text = "Connection";
			// 
			// mnuConnect
			// 
			this.mnuConnect.Index = 0;
			this.mnuConnect.Text = "Connect...";
			this.mnuConnect.Click += new System.EventHandler(this.mnuConnect_Click);
			// 
			// mnuDisconnect
			// 
			this.mnuDisconnect.Index = 1;
			this.mnuDisconnect.Text = "Disconnect";
			this.mnuDisconnect.Click += new System.EventHandler(this.mnuDisconnect_Click);
			// 
			// mnuSeparator
			// 
			this.mnuSeparator.Index = 2;
			this.mnuSeparator.Text = "-";
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 3;
			this.mnuExit.Text = "Exit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// mnuTools
			// 
			this.mnuTools.Index = 1;
			this.mnuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuSSLOptions});
			this.mnuTools.Text = "Tools";
			// 
			// mnuSSLOptions
			// 
			this.mnuSSLOptions.Index = 0;
			this.mnuSSLOptions.Text = "SSL options...";
			this.mnuSSLOptions.Click += new System.EventHandler(this.mnuSSLOptions_Click);
			// 
			// mnuHelp
			// 
			this.mnuHelp.Index = 2;
			this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuAbout});
			this.mnuHelp.Text = "Help";
			// 
			// mnuAbout
			// 
			this.mnuAbout.Index = 0;
			this.mnuAbout.Text = "About...";
			this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
			// 
			// toolBar
			// 
			this.toolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																					   this.btnConnect,
																					   this.btnDisconnect,
																					   this.btnSeparator,
																					   this.btnSSLOptions});
			this.toolBar.DropDownArrows = true;
			this.toolBar.ImageList = this.imageList;
			this.toolBar.Location = new System.Drawing.Point(0, 0);
			this.toolBar.Name = "toolBar";
			this.toolBar.ShowToolTips = true;
			this.toolBar.Size = new System.Drawing.Size(568, 28);
			this.toolBar.TabIndex = 0;
			this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
			// 
			// btnConnect
			// 
			this.btnConnect.ImageIndex = 0;
			this.btnConnect.ToolTipText = "Connect";
			// 
			// btnDisconnect
			// 
			this.btnDisconnect.ImageIndex = 1;
			this.btnDisconnect.ToolTipText = "Disconnect";
			// 
			// btnSeparator
			// 
			this.btnSeparator.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// btnSSLOptions
			// 
			this.btnSSLOptions.ImageIndex = 2;
			this.btnSSLOptions.ToolTipText = "SSL options";
			// 
			// imageList
			// 
			this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Yellow;
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 391);
			this.statusBar.Name = "statusBar";
			this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						 this.statusBarPanel1});
			this.statusBar.ShowPanels = true;
			this.statusBar.Size = new System.Drawing.Size(568, 22);
			this.statusBar.TabIndex = 1;
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel1.Width = 552;
			// 
			// SecureClient
			// 
			this.SecureClient.Address = "";
			this.SecureClient.CertStorage = null;
			this.SecureClient.Enabled = true;
			this.SecureClient.Port = 443;
			this.SecureClient.SocksAuthentication = 0;
			this.SecureClient.SocksPassword = "";
			this.SecureClient.SocksResolveAddress = false;
			this.SecureClient.SocksServer = null;
			this.SecureClient.SocksUserCode = "";
			this.SecureClient.UseInternalSocket = true;
			this.SecureClient.UseSocks = false;
			this.SecureClient.UseWebTunneling = false;
			this.SecureClient.WebTunnelAddress = null;
			this.SecureClient.WebTunnelPassword = null;
			this.SecureClient.WebTunnelPort = 3128;
			this.SecureClient.WebTunnelUserId = null;
			this.SecureClient.OnCertificateValidate += new SBSSLCommon.TSBCertificateValidateEvent(this.SecureClient_OnCertificateValidate);
			this.SecureClient.OnCloseConnection += new SBClient.TSBCloseConnectionEvent(this.SecureClient_OnCloseConnection);
			this.SecureClient.OnCertificateNeededEx += new SBClient.TSBCertificateNeededExEvent(this.SecureClient_OnCertificateNeededEx);
			// 
			// tbData
			// 
			this.tbData.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbData.Location = new System.Drawing.Point(0, 28);
			this.tbData.Multiline = true;
			this.tbData.Name = "tbData";
			this.tbData.ReadOnly = true;
			this.tbData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbData.Size = new System.Drawing.Size(568, 363);
			this.tbData.TabIndex = 2;
			this.tbData.Text = "";
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(568, 413);
			this.Controls.Add(this.tbData);
			this.Controls.Add(this.statusBar);
			this.Controls.Add(this.toolBar);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.Menu = this.mainMenu;
			this.Name = "frmMain";
			this.Text = "SSLClientDemo";
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new frmMain());
		}

		private void Connect()
		{
			int i, index;
			frmConnProps dlg = new frmConnProps();
			if (dlg.ShowDialog() == DialogResult.OK) 
			{
				SetupSSLOptions();
				tbData.Text = "";
				host = dlg.tbHost.Text;
				port = System.Convert.ToInt32(dlg.tbPort.Text, 10);
				bSSLEnabled = dlg.cbSSLEnabled.Checked;
				Status("Resolving host " + host + "...");
				System.Net.EndPoint ep = new System.Net.IPEndPoint(Dns.Resolve(dlg.tbHost.Text).AddressList[0], port);
				Status("Connecting to " + host + "...");
				certIndex = -1;
				for(i = 0; i < clientCerts.ChainCount; i++) {
					index = clientCerts.get_Chains(i);
					if (clientCerts.get_Certificates(index).PrivateKeyExists) 
					{
                        certIndex = index;
						break;
					}
				} 
				SecureClient.Address = host;
				SecureClient.Port = port;
				SecureClient.Enabled = bSSLEnabled;
				DoRequest();				
			}
		}

		private void DoRequest()
		{
			string s;
			byte[] Buffer;
			SecureClient.Open();
			if (SecureClient.Active)
			{
				Status("Sending request ...");
				string request = "GET / HTTP/1.1\nHost:" + host + "\nUser-Agent: EldoS SSLBlackbox (.NET edition)\nConnection: close\n\n";
				Buffer = SBUtils.Unit.BytesOfString(request);
				SecureClient.SendData(Buffer, 0, Buffer.Length);
			}
			
			int ToRead;
			while (SecureClient.Active)
			{
				try
				{
					ToRead = 16384;
					Buffer = new byte[ToRead];		
					SecureClient.ReceiveData(ref Buffer, ref ToRead);
					s = SBUtils.Unit.StringOfBytes(Buffer);
					//s = s.Replace("\r", "");
					tbData.Text = tbData.Text + s;
				}
				catch (Exception )
				{
					break;
				}
			}
			Disconnect();
		}

		private void Disconnect()
		{
			SecureClient.Close(true);
			Status("");
		}

		private void ExitApp()
		{
			this.Close();
		}

		private void SSLOptions()
		{
            frmSSLOptions dlg = new frmSSLOptions();
			PutSSLOptionsToDialog(dlg);
			if (dlg.ShowDialog() == DialogResult.OK) 
			{
				GetSSLOptionsFromDialog(dlg);
			}
		}

		private void About()
		{
            frmAbout dlg = new frmAbout();
			dlg.ShowDialog();
		}

		private void PutSSLOptionsToDialog(frmSSLOptions dlg)
		{
			dlg.SetCertificates(clientCerts);
			dlg.cbSSL2.Checked = bUseSSL2;
			dlg.cbSSL3.Checked = bUseSSL3;
			dlg.cbTLS1.Checked = bUseTLS1;
			dlg.cbTLS11.Checked = bUseTLS11;
			dlg.cbAllowAnon.Checked = bAllowAnonymous;
			dlg.cbExportableOnly.Checked = bUseOnlyExportable;
		}

		private void GetSSLOptionsFromDialog(frmSSLOptions dlg)
		{
			dlg.GetCertificates(clientCerts);
			bUseSSL2 = dlg.cbSSL2.Checked;
			bUseSSL3 = dlg.cbSSL3.Checked;
			bUseTLS1 = dlg.cbTLS1.Checked;
			bUseTLS11 = dlg.cbTLS11.Checked;
			bAllowAnonymous = dlg.cbAllowAnon.Checked;
			bUseOnlyExportable = dlg.cbExportableOnly.Checked;
		}

		private void SetupSSLOptions()
		{
			int i;
			SecureClient.Versions = 0;
			if (this.bUseSSL2) 
			{
				SecureClient.Versions = (short) ((byte) SecureClient.Versions | (byte)SBConstants.Unit.sbSSL2);
			}
			if (this.bUseSSL3) 
			{
				SecureClient.Versions = (short) ((byte) SecureClient.Versions | (byte)SBConstants.Unit.sbSSL3);
			}
			if (this.bUseTLS1) 
			{
				SecureClient.Versions = (short) ((byte) SecureClient.Versions | (byte)SBConstants.Unit.sbTLS1);
			}
			if (this.bUseTLS11) 
			{
				SecureClient.Versions = (short) ((byte) SecureClient.Versions | (byte)SBConstants.Unit.sbTLS11);
			}
			
			for(i = SBConstants.Unit.SB_SUITE_FIRST; i <= SBConstants.Unit.SB_SUITE_LAST; i++) 
			{
                SecureClient.set_CipherSuites((short)i, false);
			}
			
			if (this.bUseOnlyExportable) 
			{
				SecureClient.set_CipherSuites(SBConstants.Unit.SB_SUITE_RSA_DES_SHA_EXPORT, true);
				SecureClient.set_CipherSuites(SBConstants.Unit.SB_SUITE_RSA_RC2_MD5_EXPORT, true);
				SecureClient.set_CipherSuites(SBConstants.Unit.SB_SUITE_RSA_RC4_MD5_EXPORT, true);
				SecureClient.set_CipherSuites(SBConstants.Unit.SB_SUITE_DHE_RSA_DES_SHA_EXPORT, true);
				SecureClient.set_CipherSuites(SBConstants.Unit.SB_SUITE_DH_DSS_DES_SHA_EXPORT, true);
				SecureClient.set_CipherSuites(SBConstants.Unit.SB_SUITE_DH_RSA_DES_SHA_EXPORT, true);
				SecureClient.set_CipherSuites(SBConstants.Unit.SB_SUITE_DHE_DSS_DES_SHA_EXPORT, true);
				if (this.bAllowAnonymous) 
				{
					SecureClient.set_CipherSuites(SBConstants.Unit.SB_SUITE_DH_ANON_DES_SHA_EXPORT, true);
					SecureClient.set_CipherSuites(SBConstants.Unit.SB_SUITE_DH_ANON_RC4_MD5_EXPORT, true);
				}
			} 
			else 
			{
				for(i = SBConstants.Unit.SB_SUITE_FIRST; i <= SBConstants.Unit.SB_SUITE_LAST; i++) 
				{
					SecureClient.set_CipherSuites((short)i, true);
				}
				if (!this.bAllowAnonymous) 
				{
					SecureClient.set_CipherSuites(SBConstants.Unit.SB_SUITE_DH_ANON_3DES_SHA, false);
					SecureClient.set_CipherSuites(SBConstants.Unit.SB_SUITE_DH_ANON_AES128_SHA, false);
					SecureClient.set_CipherSuites(SBConstants.Unit.SB_SUITE_DH_ANON_AES256_SHA, false);
					SecureClient.set_CipherSuites(SBConstants.Unit.SB_SUITE_DH_ANON_DES_SHA, false);
					SecureClient.set_CipherSuites(SBConstants.Unit.SB_SUITE_DH_ANON_DES_SHA_EXPORT, false);
					SecureClient.set_CipherSuites(SBConstants.Unit.SB_SUITE_DH_ANON_RC4_MD5, false);
					SecureClient.set_CipherSuites(SBConstants.Unit.SB_SUITE_DH_ANON_RC4_MD5_EXPORT, false);
				}
			}
		}

		private void Status(string s) 
		{
			statusBar.Panels[0].Text = s;
		}

		private void mnuConnect_Click(object sender, System.EventArgs e)
		{
			Connect();
		}

		private void mnuDisconnect_Click(object sender, System.EventArgs e)
		{
			Disconnect();
		}

		private void mnuExit_Click(object sender, System.EventArgs e)
		{
			ExitApp();
		}

		private void mnuSSLOptions_Click(object sender, System.EventArgs e)
		{
			SSLOptions();
		}

		private void mnuAbout_Click(object sender, System.EventArgs e)
		{
			About();
		}

		private void SecureClient_OnCertificateNeededEx(object Sender, ref SBX509.TElX509Certificate Certificate)
		{
			if (certIndex >= 0) 
			{
				Certificate = clientCerts.get_Certificates(certIndex);
				certIndex = clientCerts.GetIssuerCertificate(Certificate);
			} 
			else 
			{
				Certificate = null;
			}
		}

		private void SecureClient_OnCertificateValidate(object Sender, SBX509.TElX509Certificate Certificate, ref bool Validate)
		{
			Status("Certificate received");
			Validate = true;
			// NEVER do this in real life since this makes security void. 
			// Instead validate the certificate as described on http://www.eldos.com/sbb/articles/1966.php
		}

		private void SecureClient_OnCloseConnection(object Sender, SBClient.TSBCloseReason CloseReason)
		{
			Status("SSL connection closed");
		}

		private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == btnConnect) 
			{
				Connect();
			} 
			else if (e.Button == btnDisconnect) 
			{
				Disconnect();
			} 
			else if (e.Button == btnSSLOptions) 
			{
				SSLOptions();
			} 
		}

	}
}
