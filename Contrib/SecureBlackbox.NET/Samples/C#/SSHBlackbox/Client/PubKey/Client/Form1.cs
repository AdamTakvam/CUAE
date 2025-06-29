using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.Net.Sockets;
using SBUtils;
using SBSSHPubKeyCommon;
using SBSSHPubKeyClient;
using SBSSHCommon;
using SBSSHClient;
using SBSSHKeyStorage;

namespace PubKeyCliDemo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private enum RequestType {rtAdd, rtRemove, rtList, rtNone};

		private TElSSHClient client;
		private TElSSHTunnelList tunnelList;
		private TElSubsystemSSHTunnel tunnel;
		private TElSSHPublicKeyClient publicKeyClient;
		private System.Net.Sockets.Socket socket;
		private byte[] recvBuffer = new byte[16384];
		private int recvBufferIndex;
		private int recvBufferSize;
		private RequestType lastRequestType;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem mnuConnection;
		private System.Windows.Forms.MenuItem mnuConnect;
		private System.Windows.Forms.MenuItem mnuDisconnect;
		private System.Windows.Forms.MenuItem mnuBreak;
		private System.Windows.Forms.MenuItem mnuQuit;
		private System.Windows.Forms.MenuItem mnuHelp;
		private System.Windows.Forms.MenuItem mnuAbout;
		private System.Windows.Forms.ToolBar toolBar;
		private System.Windows.Forms.ToolBarButton tbConnect;
		private System.Windows.Forms.ToolBarButton tbDisconnect;
		private System.Windows.Forms.ToolBarButton tbBreak;
		private System.Windows.Forms.ToolBarButton tbAddKey;
		private System.Windows.Forms.ToolBarButton tbRemoveKey;
		private System.Windows.Forms.ListView lvLog;
		private System.Windows.Forms.ListView lvKeys;
		private System.Windows.Forms.ColumnHeader chTitle;
		private System.Windows.Forms.ColumnHeader chAlgorithm;
		private System.Windows.Forms.ColumnHeader chBits;
		private System.Windows.Forms.ColumnHeader chFingerprint;
		private System.Windows.Forms.ColumnHeader chTime;
		private System.Windows.Forms.ColumnHeader chEvent;
		private System.Windows.Forms.ImageList imgListView;
		private System.Windows.Forms.ImageList imgToolbar;
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
			InitializeSecureBlackbox();
			CreateComponents();
		}

		private void InitializeSecureBlackbox() 
		{
			SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("BWagiShCqafilXshs9gaHG7bNhz+wszKCIo4bWWO2SewF0To37xZBxdjH0L4QO1jwDIt2heqcSAQIRVR/81gQqt2ni2P4IPBUzjJmQIjJ4P6swqmXuvumDOLj8vBr+NC33lobB4Vh+bj6sy/nacg8SuoDGbOIZG96DK7WatFkjaxL8Hv/A/dsZiGmy6V/FxVk/5taf6pWsA+l9T3jJSMha0Y5ViafoJ+fQmrBP63xpwKp+0lMPiu5iO85wXU854WRM8ihyw0JcKiYCNK40EPMmQv4Gg3gfxoM/WlunMGSIvOT30T2R6JLa5MkI2SQVvwX2GjgMuMt5bwRzNLWP55+g=="));
		}

		private void CreateComponents() 
		{
			// creating components
            client = new TElSSHClient();
			tunnelList = new TElSSHTunnelList();
			tunnel = new TElSubsystemSSHTunnel();
			publicKeyClient = new TElSSHPublicKeyClient();
			// setting up SSH client component
			client.Versions = SBSSHCommon.Unit.sbSSH2;
			client.OnError += new TSSHErrorEvent(client_OnError);
			client.OnAuthenticationFailed += new TSSHAuthenticationFailedEvent(client_OnAuthenticationFailed);
			client.OnAuthenticationSuccess += new TNotifyEvent(client_OnAuthenticationSuccess);
			client.OnAuthenticationKeyboard += new TSSHAuthenticationKeyboardEvent(client_OnAuthenticationKeyboard);
			client.OnCloseConnection += new TSSHCloseConnectionEvent(client_OnCloseConnection);
			client.OnSend += new TSSHSendEvent(client_OnSend);
			client.OnReceive += new TSSHReceiveEvent(client_OnReceive);
			client.OnKeyValidate +=new TSSHKeyValidateEvent(client_OnKeyValidate);
			// setting up tunnel list
			tunnelList = new TElSSHTunnelList();
			client.TunnelList = tunnelList;
			// setting up subsystem tunnel
			tunnel = new TElSubsystemSSHTunnel();
			tunnel.TunnelList = tunnelList;
			// initializing public-key subsystem client
			publicKeyClient.Tunnel = tunnel;
			publicKeyClient.OnOpenConnection +=new TNotifyEvent(publicKeyClient_OnOpenConnection);
			publicKeyClient.OnCloseConnection +=new TNotifyEvent(publicKeyClient_OnCloseConnection);
			publicKeyClient.OnError +=new TSBSSHPublicKeyErrorEvent(publicKeyClient_OnError);
			publicKeyClient.OnStatus +=new TSBSSHPublicKeyStatusEvent(publicKeyClient_OnStatus);
			publicKeyClient.OnPublicKey += new TSBSSHPublicKeyPublicKeyEvent(publicKeyClient_OnPublicKey);
			publicKeyClient.Subsystem = "publickey@vandyke.com";
			// initializing socket
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
				ProtocolType.Tcp);
			socket.Blocking = false;
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
			this.mnuBreak = new System.Windows.Forms.MenuItem();
			this.mnuQuit = new System.Windows.Forms.MenuItem();
			this.mnuHelp = new System.Windows.Forms.MenuItem();
			this.mnuAbout = new System.Windows.Forms.MenuItem();
			this.toolBar = new System.Windows.Forms.ToolBar();
			this.tbConnect = new System.Windows.Forms.ToolBarButton();
			this.tbDisconnect = new System.Windows.Forms.ToolBarButton();
			this.tbBreak = new System.Windows.Forms.ToolBarButton();
			this.tbAddKey = new System.Windows.Forms.ToolBarButton();
			this.tbRemoveKey = new System.Windows.Forms.ToolBarButton();
			this.lvLog = new System.Windows.Forms.ListView();
			this.chTime = new System.Windows.Forms.ColumnHeader();
			this.chEvent = new System.Windows.Forms.ColumnHeader();
			this.lvKeys = new System.Windows.Forms.ListView();
			this.chTitle = new System.Windows.Forms.ColumnHeader();
			this.chAlgorithm = new System.Windows.Forms.ColumnHeader();
			this.chBits = new System.Windows.Forms.ColumnHeader();
			this.chFingerprint = new System.Windows.Forms.ColumnHeader();
			this.imgListView = new System.Windows.Forms.ImageList(this.components);
			this.imgToolbar = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
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
																						  this.mnuQuit});
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
			// mnuQuit
			// 
			this.mnuQuit.Index = 3;
			this.mnuQuit.Text = "Quit";
			this.mnuQuit.Click += new System.EventHandler(this.mnuQuit_Click);
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
			// toolBar
			// 
			this.toolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																					   this.tbConnect,
																					   this.tbDisconnect,
																					   this.tbBreak,
																					   this.tbAddKey,
																					   this.tbRemoveKey});
			this.toolBar.DropDownArrows = true;
			this.toolBar.ImageList = this.imgToolbar;
			this.toolBar.Location = new System.Drawing.Point(0, 0);
			this.toolBar.Name = "toolBar";
			this.toolBar.ShowToolTips = true;
			this.toolBar.Size = new System.Drawing.Size(562, 28);
			this.toolBar.TabIndex = 0;
			this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
			// 
			// tbConnect
			// 
			this.tbConnect.ImageIndex = 0;
			this.tbConnect.ToolTipText = "Connect";
			// 
			// tbDisconnect
			// 
			this.tbDisconnect.ImageIndex = 1;
			this.tbDisconnect.ToolTipText = "Disconnect";
			// 
			// tbBreak
			// 
			this.tbBreak.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tbAddKey
			// 
			this.tbAddKey.ImageIndex = 2;
			this.tbAddKey.ToolTipText = "Add key";
			// 
			// tbRemoveKey
			// 
			this.tbRemoveKey.ImageIndex = 3;
			this.tbRemoveKey.ToolTipText = "Remove key";
			// 
			// lvLog
			// 
			this.lvLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					this.chTime,
																					this.chEvent});
			this.lvLog.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lvLog.FullRowSelect = true;
			this.lvLog.Location = new System.Drawing.Point(0, 266);
			this.lvLog.Name = "lvLog";
			this.lvLog.Size = new System.Drawing.Size(562, 97);
			this.lvLog.TabIndex = 1;
			this.lvLog.View = System.Windows.Forms.View.Details;
			// 
			// chTime
			// 
			this.chTime.Text = "Time";
			this.chTime.Width = 78;
			// 
			// chEvent
			// 
			this.chEvent.Text = "Event";
			this.chEvent.Width = 420;
			// 
			// lvKeys
			// 
			this.lvKeys.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					 this.chTitle,
																					 this.chAlgorithm,
																					 this.chBits,
																					 this.chFingerprint});
			this.lvKeys.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvKeys.FullRowSelect = true;
			this.lvKeys.Location = new System.Drawing.Point(0, 28);
			this.lvKeys.Name = "lvKeys";
			this.lvKeys.Size = new System.Drawing.Size(562, 238);
			this.lvKeys.TabIndex = 2;
			this.lvKeys.View = System.Windows.Forms.View.Details;
			// 
			// chTitle
			// 
			this.chTitle.Text = "Title";
			this.chTitle.Width = 125;
			// 
			// chAlgorithm
			// 
			this.chAlgorithm.Text = "Algorithm";
			// 
			// chBits
			// 
			this.chBits.Text = "Bits";
			// 
			// chFingerprint
			// 
			this.chFingerprint.Text = "Fingerprint";
			this.chFingerprint.Width = 218;
			// 
			// imgListView
			// 
			this.imgListView.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.imgListView.ImageSize = new System.Drawing.Size(16, 16);
			this.imgListView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListView.ImageStream")));
			this.imgListView.TransparentColor = System.Drawing.Color.White;
			// 
			// imgToolbar
			// 
			this.imgToolbar.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
			this.imgToolbar.ImageSize = new System.Drawing.Size(16, 16);
			this.imgToolbar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgToolbar.ImageStream")));
			this.imgToolbar.TransparentColor = System.Drawing.Color.White;
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(562, 363);
			this.Controls.Add(this.lvKeys);
			this.Controls.Add(this.lvLog);
			this.Controls.Add(this.toolBar);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Menu = this.mainMenu;
			this.Name = "frmMain";
			this.Text = "SSH Public key subsystem demo (client)";
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

		delegate void LogCallback(string text);

		private void Log(string S) 
		{
			if (this.lvLog.InvokeRequired) 
			{
				LogCallback d = new LogCallback(Log);
				this.Invoke(d, new object[] { S });
			} 
			else 
			{
				ListViewItem item = new ListViewItem();
				item.Text = DateTime.Now.ToShortTimeString();
				item.SubItems.Add(S);
				lvLog.Items.Add(item);
			}
		}

		private void Connect() 
		{
			if (client.Active)
				return;
            frmConnProps dlg = new frmConnProps();
			if (dlg.ShowDialog() == DialogResult.OK) 
			{
				lastRequestType = RequestType.rtNone;
				try 
				{
					client.UserName = dlg.tbUsername.Text;
					client.Password = dlg.tbPassword.Text;
					IPAddress addr = Dns.Resolve(dlg.tbHost.Text).AddressList[0];
					int port = System.Convert.ToInt32(dlg.tbPort.Text, 10);
					IPEndPoint ep = new IPEndPoint(addr, port);
					socket.BeginConnect(ep, new AsyncCallback(socket_OnConnect), null);
				} 
				catch(Exception ex) 
				{
					Log("Connect failed: " + ex.Message);
				}
			}
		}

		private void Disconnect() 
		{
			if (!publicKeyClient.Active) 
				return;
			client.Close(true);
		}

		private void AddKey()
		{
			if (!publicKeyClient.Active) 
				return;
			frmAddKey dlg = new frmAddKey();
			if (dlg.ShowDialog() == DialogResult.OK) 
			{
				TElSSHKey key = new TElSSHKey();
				int r = key.LoadPublicKey(dlg.tbLocation.Text);
				if (r != 0) 
				{
					MessageBox.Show("Failed to load key, error " + r.ToString(), "Error",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				} 
				else 
				{
					lastRequestType = RequestType.rtAdd;
					publicKeyClient.Add(key, null, dlg.cbOverwrite.Checked);
				}
			}
		}

		private void RemoveKey() 
		{
			if (!publicKeyClient.Active) 
				return;
			if (lvKeys.SelectedItems.Count < 1)
				return;
			TElSSHKey key = (TElSSHKey)(lvKeys.SelectedItems[0].Tag);
			lastRequestType = RequestType.rtRemove;
            publicKeyClient.Remove(key);
		}

		private void ExitApp() 
		{
			Close();
		}

		private void About() 
		{
			frmAbout dlg = new frmAbout();
			dlg.ShowDialog();
		}

		private void RefreshList() 
		{
			ClearPublicKeyList();
			lastRequestType = RequestType.rtList;
			publicKeyClient.List();
		}

		private void AddPublicKeyToList(TElSSHKey key) 
		{
            TElSSHKey keyCopy = new TElSSHKey();
			try
			{
				key.Copy(ref keyCopy);
			} 
			catch(Exception ex)
			{
				return;
			}
			ListViewItem item = new ListViewItem();
			item.Tag = keyCopy;
			string cmt = keyCopy.Comment.Replace("\n", " ");
			cmt = cmt.Replace("\r", " ");
			cmt = cmt.Trim();
			if (cmt.Length > 0) 
			{
				item.Text = cmt;
			} 
			else 
			{
				item.Text = "<untitled>";
			}
			if (keyCopy.Algorithm == SBSSHKeyStorage.Unit.ALGORITHM_RSA) 
			{
				item.SubItems.Add("RSA");
			} 
			else if (keyCopy.Algorithm == SBSSHKeyStorage.Unit.ALGORITHM_DSS) 
			{
				item.SubItems.Add("DSS");
			} 
			else 
			{
				item.SubItems.Add("Unknown");
			}
			item.SubItems.Add(keyCopy.Bits.ToString());
			item.SubItems.Add(SBUtils.Unit.DigestToStr128(keyCopy.FingerprintMD5, true));
            lvKeys.Items.Add(item);		
		}

		private void ClearPublicKeyList() 
		{
			lvKeys.Items.Clear();
		}

		private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == tbConnect) 
			{
				Connect();
			} 
			else if (e.Button == tbDisconnect) 
			{
				Disconnect();
			} 
			else if (e.Button == tbAddKey) 
			{
				AddKey();
			} 
			else if (e.Button == tbRemoveKey) 
			{
				RemoveKey();
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

		private void mnuQuit_Click(object sender, System.EventArgs e)
		{
			ExitApp();
		}

		private void mnuAbout_Click(object sender, System.EventArgs e)
		{
			About();
		}

		private void client_OnError(object Sender, int ErrorCode)
		{
            Log("SSH error " + ErrorCode.ToString());
		}

		private void client_OnAuthenticationFailed(object Sender, int AuthenticationType)
		{
            Log("SSH authentication type " + AuthenticationType.ToString() + " failed");
		}

		private void client_OnAuthenticationSuccess(object Sender)
		{
            Log("SSH authentication succeeded");
		}

		private void client_OnCloseConnection(object Sender)
		{
            Log("SSH connection closed");
		}

		private void client_OnSend(object Sender, byte[] Buffer)
		{
			try 
			{
				socket.BeginSend(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(socket_OnSend), null);
			} 
			catch(Exception ex) 
			{
				Log("Send failed: " + ex.Message);
			}
		}

		private void client_OnReceive(object Sender, ref byte[] Buffer, int MaxSize, out int Written)
		{
			Written = Math.Min(recvBufferSize - recvBufferIndex, MaxSize);
			System.Array.Copy(recvBuffer, recvBufferIndex, Buffer, 0, Written);
			recvBufferIndex += Written;
		}

		private void client_OnKeyValidate(object Sender, SBSSHKeyStorage.TElSSHKey ServerKey, ref bool Validate)
		{
            Log("Server host key received: " + SBUtils.Unit.DigestToStr128(ServerKey.FingerprintMD5, true));
            Validate = true;
		}

		private void publicKeyClient_OnOpenConnection(object Sender)
		{
            Log("Public key subsystem opened");
			RefreshList();
		}

		private void publicKeyClient_OnCloseConnection(object Sender)
		{
			Log("Public key subsystem closed");
		}

		private void publicKeyClient_OnError(object Sender, int ErrorCode, string Comment)
		{
            Log("Public key subsystem error " + ErrorCode.ToString() + " (" + Comment + ")");
		}

		private void publicKeyClient_OnStatus(object Sender, int Status, string Descr, string Tag)
		{
            Log("Public key subsystem status: " + Status.ToString() + " (" + Descr + ")");
			if ((lastRequestType == RequestType.rtAdd) || (lastRequestType == RequestType.rtRemove)) 
			{
				RefreshList();
			}
		}

		private void publicKeyClient_OnPublicKey(object Sender, SBSSHKeyStorage.TElSSHKey Key, TElSSHPublicKeyAttributes Attributes)
		{
			for (int i = 0; i < Attributes.Count; i++) 
			{
				if (String.Equals(Attributes.get_Names(i), "comment")) 
				{
					Key.Comment = Attributes.get_Values(i);
					break;
				}
			}
            AddPublicKeyToList(Key);            
		}

		private void socket_OnConnect(IAsyncResult ar) 
		{
			try 
			{
				socket.EndConnect(ar);
				client.Open();
				recvBufferIndex = 0;
				socket.BeginReceive(recvBuffer, 0, recvBuffer.Length, SocketFlags.None,
					new AsyncCallback(socket_OnReceive), null);
			} 
			catch(Exception ex) 
			{
                Log("Connect failed: " + ex.Message);
			}
		}

		private void socket_OnSend(IAsyncResult ar) 
		{
			try 
			{
				socket.EndSend(ar);
			} 
			catch(Exception ex) 
			{
				Log("Send failed: " + ex.Message);
			}
		}

		private void socket_OnReceive(IAsyncResult ar) 
		{
			try 
			{
				int received = socket.EndReceive(ar);
				recvBufferIndex = 0;
				recvBufferSize = received;
				while(recvBufferIndex < recvBufferSize) 
				{
                    client.DataAvailable();
				}
				socket.BeginReceive(recvBuffer, 0, recvBuffer.Length, SocketFlags.None,
					new AsyncCallback(socket_OnReceive), null);
			} 
			catch(Exception ex) 
			{
                Log("Receive failed: " + ex.Message);
			}
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
