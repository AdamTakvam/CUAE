using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using SBSimpleSSH;
using SBSSHConstants;
using SBSSHCommon;
using SBSSHKeyStorage;
using SBUtils;
using System.Net;
using System.Net.Sockets;

namespace SimpleSSHDemo
{
	/// <summary>
	/// SimpleSSHDemo main form
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private SBSimpleSSH.TElSimpleSSHClient client;
		private TElSSHMemoryKeyStorage KeyStorage;
		private System.Windows.Forms.ColumnHeader chTime;
		private System.Windows.Forms.ColumnHeader chEvent;
		private System.Windows.Forms.ListView lvLog;

		private System.Windows.Forms.MainMenu mnuMain;
		private System.Windows.Forms.ToolBar tbToolbar;
		private System.Windows.Forms.ToolBarButton btnConnect;
		private System.Windows.Forms.ToolBarButton btnDisconnect;
		private System.Windows.Forms.ImageList imgToolbar;
		private System.Windows.Forms.MenuItem mnuConnection;
		private System.Windows.Forms.MenuItem mnuConnect;
		private System.Windows.Forms.MenuItem mnuDisconnect;
		private System.Windows.Forms.MenuItem mnuBreak;
		private System.Windows.Forms.MenuItem mnuExit;
		private System.Windows.Forms.MenuItem mnuHelp;
		private System.Windows.Forms.MenuItem mnuAbout;
		private System.Windows.Forms.Splitter splitter;
		private System.Windows.Forms.Panel pSend;
		private System.Windows.Forms.TextBox tbSend;
		private System.Windows.Forms.Button btnSend;
		private System.Windows.Forms.Panel pView;
		private System.Windows.Forms.TextBox tbView;
		private System.Windows.Forms.ImageList imgListLog;
		private System.Windows.Forms.Timer timer;
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
			Initialize();
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
			this.client = new SBSimpleSSH.TElSimpleSSHClient();
			this.lvLog = new System.Windows.Forms.ListView();
			this.chTime = new System.Windows.Forms.ColumnHeader();
			this.chEvent = new System.Windows.Forms.ColumnHeader();
			this.imgListLog = new System.Windows.Forms.ImageList(this.components);
			this.tbToolbar = new System.Windows.Forms.ToolBar();
			this.btnConnect = new System.Windows.Forms.ToolBarButton();
			this.btnDisconnect = new System.Windows.Forms.ToolBarButton();
			this.imgToolbar = new System.Windows.Forms.ImageList(this.components);
			this.mnuMain = new System.Windows.Forms.MainMenu();
			this.mnuConnection = new System.Windows.Forms.MenuItem();
			this.mnuConnect = new System.Windows.Forms.MenuItem();
			this.mnuDisconnect = new System.Windows.Forms.MenuItem();
			this.mnuBreak = new System.Windows.Forms.MenuItem();
			this.mnuExit = new System.Windows.Forms.MenuItem();
			this.mnuHelp = new System.Windows.Forms.MenuItem();
			this.mnuAbout = new System.Windows.Forms.MenuItem();
			this.splitter = new System.Windows.Forms.Splitter();
			this.pSend = new System.Windows.Forms.Panel();
			this.btnSend = new System.Windows.Forms.Button();
			this.tbSend = new System.Windows.Forms.TextBox();
			this.pView = new System.Windows.Forms.Panel();
			this.tbView = new System.Windows.Forms.TextBox();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.pSend.SuspendLayout();
			this.pView.SuspendLayout();
			this.SuspendLayout();
			// 
			// client
			// 
			this.client.Address = "";
			this.client.AuthenticationTypes = 22;
			this.client.ClientHostname = "";
			this.client.ClientUsername = "";
			this.client.Command = "";
			this.client.CompressionLevel = 6;
			this.client.ForceCompression = false;
			this.client.KeyStorage = null;
			this.client.Password = "";
			this.client.SocksAuthentication = 0;
			this.client.SocksPassword = null;
			this.client.SocksPort = 0;
			this.client.SocksResolveAddress = false;
			this.client.SocksServer = null;
			this.client.SocksUserCode = null;
			this.client.SocksVersion = 0;
			this.client.SoftwareName = "EldoS.SSHBlackbox.3";
			this.client.TerminalInfo = null;
			this.client.UseInternalSocket = true;
			this.client.Username = "";
			this.client.UseSocks = false;
			this.client.UseWebTunneling = false;
			this.client.Versions = ((short)(0));
			this.client.WebTunnelAddress = null;
			this.client.WebTunnelPassword = null;
			this.client.WebTunnelPort = 0;
			this.client.WebTunnelUserId = null;
			this.client.OnKeyValidate += new SBSSHCommon.TSSHKeyValidateEvent(this.client_OnKeyValidate);
			this.client.OnAuthenticationSuccess += new SBUtils.TNotifyEvent(this.client_OnAuthenticationSuccess);
			this.client.OnAuthenticationKeyboard += new SBSSHCommon.TSSHAuthenticationKeyboardEvent(this.client_OnAuthenticationKeyboard);
			this.client.OnCloseConnection += new SBSSHCommon.TSSHCloseConnectionEvent(this.client_OnCloseConnection);
			this.client.OnError += new SBSSHCommon.TSSHErrorEvent(this.client_OnError);
			this.client.OnAuthenticationFailed += new SBSSHCommon.TSSHAuthenticationFailedEvent(this.client_OnAuthenticationFailed);
			// 
			// lvLog
			// 
			this.lvLog.Alignment = System.Windows.Forms.ListViewAlignment.Default;
			this.lvLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					this.chTime,
																					this.chEvent});
			this.lvLog.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lvLog.FullRowSelect = true;
			this.lvLog.Location = new System.Drawing.Point(0, 429);
			this.lvLog.Name = "lvLog";
			this.lvLog.Size = new System.Drawing.Size(688, 104);
			this.lvLog.SmallImageList = this.imgListLog;
			this.lvLog.TabIndex = 2;
			this.lvLog.View = System.Windows.Forms.View.Details;
			// 
			// chTime
			// 
			this.chTime.Text = "Time";
			this.chTime.Width = 80;
			// 
			// chEvent
			// 
			this.chEvent.Text = "Event";
			this.chEvent.Width = 250;
			// 
			// imgListLog
			// 
			this.imgListLog.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.imgListLog.ImageSize = new System.Drawing.Size(16, 16);
			this.imgListLog.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListLog.ImageStream")));
			this.imgListLog.TransparentColor = System.Drawing.Color.White;
			// 
			// tbToolbar
			// 
			this.tbToolbar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.tbToolbar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						 this.btnConnect,
																						 this.btnDisconnect});
			this.tbToolbar.DropDownArrows = true;
			this.tbToolbar.ImageList = this.imgToolbar;
			this.tbToolbar.Location = new System.Drawing.Point(0, 0);
			this.tbToolbar.Name = "tbToolbar";
			this.tbToolbar.ShowToolTips = true;
			this.tbToolbar.Size = new System.Drawing.Size(688, 28);
			this.tbToolbar.TabIndex = 6;
			this.tbToolbar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbToolbar_ButtonClick);
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
			// imgToolbar
			// 
			this.imgToolbar.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.imgToolbar.ImageSize = new System.Drawing.Size(16, 16);
			this.imgToolbar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgToolbar.ImageStream")));
			this.imgToolbar.TransparentColor = System.Drawing.Color.Yellow;
			// 
			// mnuMain
			// 
			this.mnuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuConnection,
																					this.mnuHelp});
			// 
			// mnuConnection
			// 
			this.mnuConnection.Index = 0;
			this.mnuConnection.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.mnuConnect,
																						  this.mnuDisconnect,
																						  this.mnuBreak,
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
			// mnuBreak
			// 
			this.mnuBreak.Index = 2;
			this.mnuBreak.Text = "-";
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 3;
			this.mnuExit.Text = "Exit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// mnuHelp
			// 
			this.mnuHelp.Index = 1;
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
			// splitter
			// 
			this.splitter.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter.Location = new System.Drawing.Point(0, 426);
			this.splitter.Name = "splitter";
			this.splitter.Size = new System.Drawing.Size(688, 3);
			this.splitter.TabIndex = 7;
			this.splitter.TabStop = false;
			// 
			// pSend
			// 
			this.pSend.Controls.Add(this.btnSend);
			this.pSend.Controls.Add(this.tbSend);
			this.pSend.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pSend.Location = new System.Drawing.Point(0, 394);
			this.pSend.Name = "pSend";
			this.pSend.Size = new System.Drawing.Size(688, 32);
			this.pSend.TabIndex = 8;
			// 
			// btnSend
			// 
			this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.btnSend.Image = ((System.Drawing.Image)(resources.GetObject("btnSend.Image")));
			this.btnSend.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnSend.Location = new System.Drawing.Point(624, 8);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(56, 23);
			this.btnSend.TabIndex = 1;
			this.btnSend.Text = "Send";
			this.btnSend.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			// 
			// tbSend
			// 
			this.tbSend.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tbSend.Location = new System.Drawing.Point(64, 8);
			this.tbSend.Name = "tbSend";
			this.tbSend.Size = new System.Drawing.Size(552, 21);
			this.tbSend.TabIndex = 0;
			this.tbSend.Text = "";
			// 
			// pView
			// 
			this.pView.Controls.Add(this.tbView);
			this.pView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pView.Location = new System.Drawing.Point(0, 28);
			this.pView.Name = "pView";
			this.pView.Size = new System.Drawing.Size(688, 366);
			this.pView.TabIndex = 9;
			// 
			// tbView
			// 
			this.tbView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbView.Location = new System.Drawing.Point(0, 0);
			this.tbView.Multiline = true;
			this.tbView.Name = "tbView";
			this.tbView.ReadOnly = true;
			this.tbView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbView.Size = new System.Drawing.Size(688, 366);
			this.tbView.TabIndex = 0;
			this.tbView.Text = "";
			// 
			// timer
			// 
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(688, 533);
			this.Controls.Add(this.pView);
			this.Controls.Add(this.pSend);
			this.Controls.Add(this.splitter);
			this.Controls.Add(this.tbToolbar);
			this.Controls.Add(this.lvLog);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.Menu = this.mnuMain;
			this.Name = "frmMain";
			this.Text = "SimpleSSHClient Demo Application";
			this.pSend.ResumeLayout(false);
			this.pView.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void Initialize() 
		{
			SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"));

			KeyStorage = new TElSSHMemoryKeyStorage(this);
			client.KeyStorage = KeyStorage;
		}

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
			frmConnProps dlg = new frmConnProps();

			if (client.Active) 
			{
				MessageBox.Show("Already connected");
				return;
			}

			if (dlg.ShowDialog(this) == DialogResult.OK) 
			{
				client.Address = dlg.tbHost.Text;
				client.Port = 22;
				client.Username = dlg.tbUser.Text;
				client.Password = dlg.tbPassword.Text;
				client.Versions = 0;
				if (dlg.cbSSH1.Checked) 
				{
					client.Versions += SBSSHCommon.Unit.sbSSH1;
				}
				if (dlg.cbSSH2.Checked) 
				{
					client.Versions += SBSSHCommon.Unit.sbSSH2;
				}
				client.ForceCompression = dlg.cbCompress.Checked;
				
				KeyStorage.Clear();
				TElSSHKey Key = new TElSSHKey();
				if ((dlg.txtPrivateKey.Text != "") && (Key.LoadPrivateKey(dlg.txtPrivateKey.Text, "") == 0))
				{
					KeyStorage.Add(Key);
					client.AuthenticationTypes = client.AuthenticationTypes | SBSSHConstants.Unit.SSH_AUTH_TYPE_PUBLICKEY;
				}
				else
					client.AuthenticationTypes = client.AuthenticationTypes & (~SBSSHConstants.Unit.SSH_AUTH_TYPE_PUBLICKEY);

				Log("Connecting to " + dlg.tbHost.Text + "...", false);
				try 
				{
					client.Open();
					timer.Enabled = true;
				} 
				catch(Exception ex) 
				{
					Log("SSH connection failed", true);
					return;
				}

				Log("SSH connection established", false);				
			}
		}

		private void Disconnect()
		{
			if (client.Active) 
			{
				Log("Disconnecting", false);
				client.Close();
			}
		}

		private void Send(string Data) 
		{
			byte[] encoded = System.Text.Encoding.UTF8.GetBytes(Data + "\x0d\x0a");
			client.SendData(encoded, encoded.Length);
		}

		private void Log(string S, bool error) 
		{
			int imgIndex;
			string[] items = new string[2];
			items[0] = DateTime.Now.ToShortTimeString();
			items[1] = S;
			if (error) 
			{
				imgIndex = 1;
			} 
			else 
			{
				imgIndex = 0;
			}

			ListViewItem item = new ListViewItem(items, imgIndex);
			lvLog.Items.Add(item);
		}

		private void client_OnAuthenticationFailed(object Sender, int AuthenticationType)
		{
			timer.Enabled = false;
			Log("Authentication type (" + AuthenticationType.ToString() + ") failed", true); 
		}

		private void client_OnAuthenticationSuccess(object Sender)
		{
			Log("Authentication succeeded", false);
		}

		private void client_OnCloseConnection(object Sender)
		{
			timer.Enabled = false;
			Log("SSH connection closed", false);
		}

		private void client_OnError(object Sender, int ErrorCode)
		{
			Log("SSH error " + ErrorCode.ToString(), true);
		}

		private void client_OnKeyValidate(object Sender, SBSSHKeyStorage.TElSSHKey ServerKey, ref bool Validate)
		{
			Log("Server key received, fingerprint " + SBUtils.Unit.DigestToStr128(ServerKey.FingerprintMD5, true), false);
		}

		private void btnSend_Click(object sender, System.EventArgs e)
		{
			Send(tbSend.Text);
			tbSend.Text = "";
		}

		private void tbToolbar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == btnConnect) 
			{
				Connect();
			} 
			else if (e.Button == btnDisconnect) 
			{
				Disconnect();
			}
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
			this.Close();
		}

		private void mnuAbout_Click(object sender, System.EventArgs e)
		{
			frmAbout dlg = new frmAbout();
			dlg.ShowDialog(this);
		}

		private void timer_Tick(object sender, System.EventArgs e)
		{
			byte[] data = new byte[65280];
			byte[] dataErr = new byte[65280];
			int dataLen, dataErrLen;
			
			dataLen = data.Length;
			dataErrLen = dataErr.Length;
			client.ReceiveData(ref data, ref dataLen, ref dataErr, ref dataErrLen);
			tbView.Text = tbView.Text + System.Text.Encoding.UTF8.GetString(data, 0, dataLen);
		}

		private void client_OnAuthenticationKeyboard(object Sender, SBStringList.TElStringList Prompts, bool[] Echo, SBStringList.TElStringList Responses)
		{
			Responses.Clear();
			for (int i = 0; i < Prompts.Count; i++)
			{
				string Response = "";				
				if (PromptForm.Prompt(Prompts[i], Echo[i], ref Response))
					Responses.Add(Response);
				else
					Responses.Add("");
			}
		
		}
	}
}
