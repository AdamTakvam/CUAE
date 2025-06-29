using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using SBSSHForwarding;

namespace LocalPortForwardDemo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel pLeft;
		private System.Windows.Forms.Panel pRight;
		private System.Windows.Forms.Panel pLog;
		private System.Windows.Forms.Splitter splitter;
		private System.Windows.Forms.ListView lvConnections;
		private System.Windows.Forms.ListView lvLog;
		private System.Windows.Forms.ColumnHeader chTime;
		private System.Windows.Forms.ColumnHeader chEvent;
		private System.Windows.Forms.ColumnHeader chHost;
		private System.Windows.Forms.ColumnHeader chIncomingSocket;
		private System.Windows.Forms.ColumnHeader chOutgoingSocket;
		private System.Windows.Forms.ColumnHeader chIncomingChannel;
		private System.Windows.Forms.ColumnHeader chOutgoingChannel;
		private System.Windows.Forms.ColumnHeader chSocketState;
		private System.Windows.Forms.ColumnHeader chChannelState;
		private System.Windows.Forms.Label lblConnSettings;
		private System.Windows.Forms.Label lblHost;
		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.TextBox tbHost;
		private System.Windows.Forms.TextBox tbPort;
		private System.Windows.Forms.Label lblUsername;
		private System.Windows.Forms.TextBox tbUsername;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.Label lblForwardingSettings;
		private System.Windows.Forms.Label lblForwardedPort;
		private System.Windows.Forms.TextBox tbForwardedPort;
		private System.Windows.Forms.Label lblDestination;
		private System.Windows.Forms.TextBox tbDestHost;
		private System.Windows.Forms.TextBox tbDestPort;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.ImageList imgLog;
		private System.Windows.Forms.ImageList imgConns;
		private SBSSHForwarding.TElSSHRemotePortForwarding forwarding;
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
			SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"));
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
			this.pLeft = new System.Windows.Forms.Panel();
			this.btnStart = new System.Windows.Forms.Button();
			this.tbDestPort = new System.Windows.Forms.TextBox();
			this.tbDestHost = new System.Windows.Forms.TextBox();
			this.lblDestination = new System.Windows.Forms.Label();
			this.tbForwardedPort = new System.Windows.Forms.TextBox();
			this.lblForwardedPort = new System.Windows.Forms.Label();
			this.lblForwardingSettings = new System.Windows.Forms.Label();
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.lblPassword = new System.Windows.Forms.Label();
			this.tbUsername = new System.Windows.Forms.TextBox();
			this.lblUsername = new System.Windows.Forms.Label();
			this.tbPort = new System.Windows.Forms.TextBox();
			this.lblPort = new System.Windows.Forms.Label();
			this.tbHost = new System.Windows.Forms.TextBox();
			this.lblHost = new System.Windows.Forms.Label();
			this.lblConnSettings = new System.Windows.Forms.Label();
			this.pRight = new System.Windows.Forms.Panel();
			this.lvConnections = new System.Windows.Forms.ListView();
			this.chHost = new System.Windows.Forms.ColumnHeader();
			this.chIncomingSocket = new System.Windows.Forms.ColumnHeader();
			this.chOutgoingSocket = new System.Windows.Forms.ColumnHeader();
			this.chIncomingChannel = new System.Windows.Forms.ColumnHeader();
			this.chOutgoingChannel = new System.Windows.Forms.ColumnHeader();
			this.chSocketState = new System.Windows.Forms.ColumnHeader();
			this.chChannelState = new System.Windows.Forms.ColumnHeader();
			this.imgConns = new System.Windows.Forms.ImageList(this.components);
			this.splitter = new System.Windows.Forms.Splitter();
			this.pLog = new System.Windows.Forms.Panel();
			this.lvLog = new System.Windows.Forms.ListView();
			this.chTime = new System.Windows.Forms.ColumnHeader();
			this.chEvent = new System.Windows.Forms.ColumnHeader();
			this.imgLog = new System.Windows.Forms.ImageList(this.components);
			this.forwarding = new SBSSHForwarding.TElSSHRemotePortForwarding();
			this.pLeft.SuspendLayout();
			this.pRight.SuspendLayout();
			this.pLog.SuspendLayout();
			this.SuspendLayout();
			// 
			// pLeft
			// 
			this.pLeft.Controls.Add(this.btnStart);
			this.pLeft.Controls.Add(this.tbDestPort);
			this.pLeft.Controls.Add(this.tbDestHost);
			this.pLeft.Controls.Add(this.lblDestination);
			this.pLeft.Controls.Add(this.tbForwardedPort);
			this.pLeft.Controls.Add(this.lblForwardedPort);
			this.pLeft.Controls.Add(this.lblForwardingSettings);
			this.pLeft.Controls.Add(this.tbPassword);
			this.pLeft.Controls.Add(this.lblPassword);
			this.pLeft.Controls.Add(this.tbUsername);
			this.pLeft.Controls.Add(this.lblUsername);
			this.pLeft.Controls.Add(this.tbPort);
			this.pLeft.Controls.Add(this.lblPort);
			this.pLeft.Controls.Add(this.tbHost);
			this.pLeft.Controls.Add(this.lblHost);
			this.pLeft.Controls.Add(this.lblConnSettings);
			this.pLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.pLeft.Location = new System.Drawing.Point(0, 0);
			this.pLeft.Name = "pLeft";
			this.pLeft.Size = new System.Drawing.Size(216, 437);
			this.pLeft.TabIndex = 0;
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(72, 312);
			this.btnStart.Name = "btnStart";
			this.btnStart.TabIndex = 15;
			this.btnStart.Text = "Start";
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// tbDestPort
			// 
			this.tbDestPort.Location = new System.Drawing.Point(160, 272);
			this.tbDestPort.Name = "tbDestPort";
			this.tbDestPort.Size = new System.Drawing.Size(48, 21);
			this.tbDestPort.TabIndex = 14;
			this.tbDestPort.Text = "5001";
			// 
			// tbDestHost
			// 
			this.tbDestHost.Location = new System.Drawing.Point(8, 272);
			this.tbDestHost.Name = "tbDestHost";
			this.tbDestHost.Size = new System.Drawing.Size(144, 21);
			this.tbDestHost.TabIndex = 13;
			this.tbDestHost.Text = "192.168.0.2";
			// 
			// lblDestination
			// 
			this.lblDestination.Location = new System.Drawing.Point(8, 256);
			this.lblDestination.Name = "lblDestination";
			this.lblDestination.Size = new System.Drawing.Size(200, 16);
			this.lblDestination.TabIndex = 12;
			this.lblDestination.Text = "Destination host and port:";
			// 
			// tbForwardedPort
			// 
			this.tbForwardedPort.Location = new System.Drawing.Point(8, 224);
			this.tbForwardedPort.Name = "tbForwardedPort";
			this.tbForwardedPort.Size = new System.Drawing.Size(48, 21);
			this.tbForwardedPort.TabIndex = 11;
			this.tbForwardedPort.Text = "5001";
			// 
			// lblForwardedPort
			// 
			this.lblForwardedPort.Location = new System.Drawing.Point(8, 208);
			this.lblForwardedPort.Name = "lblForwardedPort";
			this.lblForwardedPort.Size = new System.Drawing.Size(144, 16);
			this.lblForwardedPort.TabIndex = 10;
			this.lblForwardedPort.Text = "Forwarded port:";
			// 
			// lblForwardingSettings
			// 
			this.lblForwardingSettings.BackColor = System.Drawing.SystemColors.ControlDark;
			this.lblForwardingSettings.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.lblForwardingSettings.Location = new System.Drawing.Point(8, 184);
			this.lblForwardingSettings.Name = "lblForwardingSettings";
			this.lblForwardingSettings.Size = new System.Drawing.Size(200, 16);
			this.lblForwardingSettings.TabIndex = 9;
			this.lblForwardingSettings.Text = "Forwarding settings";
			this.lblForwardingSettings.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tbPassword
			// 
			this.tbPassword.Location = new System.Drawing.Point(8, 144);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.PasswordChar = '*';
			this.tbPassword.Size = new System.Drawing.Size(120, 21);
			this.tbPassword.TabIndex = 8;
			this.tbPassword.Text = "";
			// 
			// lblPassword
			// 
			this.lblPassword.Location = new System.Drawing.Point(8, 128);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(104, 16);
			this.lblPassword.TabIndex = 7;
			this.lblPassword.Text = "Password:";
			// 
			// tbUsername
			// 
			this.tbUsername.Location = new System.Drawing.Point(8, 96);
			this.tbUsername.Name = "tbUsername";
			this.tbUsername.Size = new System.Drawing.Size(120, 21);
			this.tbUsername.TabIndex = 6;
			this.tbUsername.Text = "user";
			// 
			// lblUsername
			// 
			this.lblUsername.Location = new System.Drawing.Point(8, 80);
			this.lblUsername.Name = "lblUsername";
			this.lblUsername.Size = new System.Drawing.Size(96, 16);
			this.lblUsername.TabIndex = 5;
			this.lblUsername.Text = "Username:";
			// 
			// tbPort
			// 
			this.tbPort.Location = new System.Drawing.Point(160, 48);
			this.tbPort.Name = "tbPort";
			this.tbPort.Size = new System.Drawing.Size(48, 21);
			this.tbPort.TabIndex = 4;
			this.tbPort.Text = "22";
			// 
			// lblPort
			// 
			this.lblPort.Location = new System.Drawing.Point(160, 32);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(48, 16);
			this.lblPort.TabIndex = 3;
			this.lblPort.Text = "Port:";
			// 
			// tbHost
			// 
			this.tbHost.Location = new System.Drawing.Point(8, 48);
			this.tbHost.Name = "tbHost";
			this.tbHost.Size = new System.Drawing.Size(144, 21);
			this.tbHost.TabIndex = 2;
			this.tbHost.Text = "192.168.0.1";
			// 
			// lblHost
			// 
			this.lblHost.Location = new System.Drawing.Point(8, 32);
			this.lblHost.Name = "lblHost";
			this.lblHost.Size = new System.Drawing.Size(104, 16);
			this.lblHost.TabIndex = 1;
			this.lblHost.Text = "Host:";
			// 
			// lblConnSettings
			// 
			this.lblConnSettings.BackColor = System.Drawing.SystemColors.ControlDark;
			this.lblConnSettings.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.lblConnSettings.Location = new System.Drawing.Point(8, 8);
			this.lblConnSettings.Name = "lblConnSettings";
			this.lblConnSettings.Size = new System.Drawing.Size(200, 16);
			this.lblConnSettings.TabIndex = 0;
			this.lblConnSettings.Text = "SSH connection settings";
			this.lblConnSettings.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pRight
			// 
			this.pRight.Controls.Add(this.lvConnections);
			this.pRight.Controls.Add(this.splitter);
			this.pRight.Controls.Add(this.pLog);
			this.pRight.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pRight.Location = new System.Drawing.Point(216, 0);
			this.pRight.Name = "pRight";
			this.pRight.Size = new System.Drawing.Size(600, 437);
			this.pRight.TabIndex = 1;
			// 
			// lvConnections
			// 
			this.lvConnections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.chHost,
																							this.chIncomingSocket,
																							this.chOutgoingSocket,
																							this.chIncomingChannel,
																							this.chOutgoingChannel,
																							this.chSocketState,
																							this.chChannelState});
			this.lvConnections.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvConnections.FullRowSelect = true;
			this.lvConnections.Location = new System.Drawing.Point(0, 0);
			this.lvConnections.MultiSelect = false;
			this.lvConnections.Name = "lvConnections";
			this.lvConnections.Size = new System.Drawing.Size(600, 334);
			this.lvConnections.SmallImageList = this.imgConns;
			this.lvConnections.TabIndex = 2;
			this.lvConnections.View = System.Windows.Forms.View.Details;
			// 
			// chHost
			// 
			this.chHost.Text = "Host";
			this.chHost.Width = 93;
			// 
			// chIncomingSocket
			// 
			this.chIncomingSocket.Text = "Incoming (socket)";
			this.chIncomingSocket.Width = 84;
			// 
			// chOutgoingSocket
			// 
			this.chOutgoingSocket.Text = "Outgoing (socket)";
			this.chOutgoingSocket.Width = 84;
			// 
			// chIncomingChannel
			// 
			this.chIncomingChannel.Text = "Incoming (channel)";
			this.chIncomingChannel.Width = 80;
			// 
			// chOutgoingChannel
			// 
			this.chOutgoingChannel.Text = "Outgoing (channel)";
			this.chOutgoingChannel.Width = 82;
			// 
			// chSocketState
			// 
			this.chSocketState.Text = "Socket state";
			this.chSocketState.Width = 71;
			// 
			// chChannelState
			// 
			this.chChannelState.Text = "Channel state";
			this.chChannelState.Width = 75;
			// 
			// imgConns
			// 
			this.imgConns.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imgConns.ImageSize = new System.Drawing.Size(16, 16);
			this.imgConns.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgConns.ImageStream")));
			this.imgConns.TransparentColor = System.Drawing.Color.Yellow;
			// 
			// splitter
			// 
			this.splitter.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter.Location = new System.Drawing.Point(0, 334);
			this.splitter.Name = "splitter";
			this.splitter.Size = new System.Drawing.Size(600, 3);
			this.splitter.TabIndex = 1;
			this.splitter.TabStop = false;
			// 
			// pLog
			// 
			this.pLog.Controls.Add(this.lvLog);
			this.pLog.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pLog.Location = new System.Drawing.Point(0, 337);
			this.pLog.Name = "pLog";
			this.pLog.Size = new System.Drawing.Size(600, 100);
			this.pLog.TabIndex = 0;
			// 
			// lvLog
			// 
			this.lvLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					this.chTime,
																					this.chEvent});
			this.lvLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvLog.FullRowSelect = true;
			this.lvLog.Location = new System.Drawing.Point(0, 0);
			this.lvLog.MultiSelect = false;
			this.lvLog.Name = "lvLog";
			this.lvLog.Size = new System.Drawing.Size(600, 100);
			this.lvLog.SmallImageList = this.imgLog;
			this.lvLog.TabIndex = 0;
			this.lvLog.View = System.Windows.Forms.View.Details;
			// 
			// chTime
			// 
			this.chTime.Text = "Time";
			this.chTime.Width = 84;
			// 
			// chEvent
			// 
			this.chEvent.Text = "Event";
			this.chEvent.Width = 483;
			// 
			// imgLog
			// 
			this.imgLog.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imgLog.ImageSize = new System.Drawing.Size(16, 16);
			this.imgLog.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLog.ImageStream")));
			this.imgLog.TransparentColor = System.Drawing.Color.Yellow;
			// 
			// forwarding
			// 
			this.forwarding.Address = null;
			this.forwarding.ClientHostname = null;
			this.forwarding.ClientUsername = null;
			this.forwarding.CompressionLevel = 0;
			this.forwarding.DestHost = null;
			this.forwarding.DestPort = 0;
			this.forwarding.ForceCompression = false;
			this.forwarding.ForwardedHost = "";
			this.forwarding.ForwardedPort = 0;
			this.forwarding.KeyStorage = null;
			this.forwarding.Password = null;
			this.forwarding.Port = 0;
			this.forwarding.SocksAuthentication = 0;
			this.forwarding.SocksPassword = null;
			this.forwarding.SocksPort = 0;
			this.forwarding.SocksResolveAddress = false;
			this.forwarding.SocksServer = null;
			this.forwarding.SocksUserCode = null;
			this.forwarding.SocksVersion = 0;
			this.forwarding.SoftwareName = "SSHBlackbox.4";
			this.forwarding.SynchronizeGUI = true;
			this.forwarding.UseProxySettingsForForwardedConnections = false;
			this.forwarding.Username = null;
			this.forwarding.UseSocks = false;
			this.forwarding.UseWebTunneling = false;
			this.forwarding.Versions = ((short)(2));
			this.forwarding.WebTunnelAddress = null;
			this.forwarding.WebTunnelPassword = null;
			this.forwarding.WebTunnelPort = 0;
			this.forwarding.WebTunnelUserId = null;
			this.forwarding.OnKeyValidate += new SBSSHCommon.TSSHKeyValidateEvent(this.forwarding_OnKeyValidate);
			this.forwarding.OnAuthenticationFailed += new SBSSHCommon.TSSHAuthenticationFailedEvent(this.forwarding_OnAuthenticationFailed);
			this.forwarding.OnConnectionOpen += new SBSSHForwarding.TSBSSHConnectionEvent(this.forwarding_OnConnectionOpen);
			this.forwarding.OnClose += new SBUtils.TNotifyEvent(this.forwarding_OnClose);
			this.forwarding.OnConnectionError += new SBSSHForwarding.TSBSSHConnectionErrorEvent(this.forwarding_OnConnectionError);
			this.forwarding.OnConnectionClose += new SBSSHForwarding.TSBSSHConnectionEvent(this.forwarding_OnConnectionClose);
			this.forwarding.OnAuthenticationSuccess += new SBUtils.TNotifyEvent(this.forwarding_OnAuthenticationSuccess);
			this.forwarding.OnError += new SBSSHCommon.TSSHErrorEvent(this.forwarding_OnError);
			this.forwarding.OnConnectionChange += new SBSSHForwarding.TSBSSHConnectionEvent(this.forwarding_OnConnectionChange);
			this.forwarding.OnOpen += new SBUtils.TNotifyEvent(this.forwarding_OnOpen);
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(816, 437);
			this.Controls.Add(this.pRight);
			this.Controls.Add(this.pLeft);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.Name = "frmMain";
			this.Text = "SSH remote port forwarding demo";
			this.Closed += new System.EventHandler(this.frmMain_Closed);
			this.pLeft.ResumeLayout(false);
			this.pRight.ResumeLayout(false);
			this.pLog.ResumeLayout(false);
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

        delegate void LogFunc(string s, bool error);

		private void Log(string s, bool error) 
		{
            if (lvLog.InvokeRequired)
            {
                LogFunc d = new LogFunc(Log);
                Invoke(d, new object[] { s, error });
            } 
            else
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = DateTime.Now.ToShortTimeString();
                lvi.SubItems.Add(s);
                if (!error)
                {
                    lvi.ImageIndex = 0;
                }
                else
                {
                    lvi.ImageIndex = 1;
                }
                lvLog.Items.Add(lvi);
            }
		}

        delegate void RefreshConnectionFunc(TElSSHForwardedConnection conn);

		private void RefreshConnection(TElSSHForwardedConnection conn)
		{
            if (lvConnections.InvokeRequired)
            {
                RefreshConnectionFunc d = new RefreshConnectionFunc(RefreshConnection);
                Invoke(d, new object[] { conn });
            }
            else
            {
                string s = "";
                ListViewItem lvi = (ListViewItem)conn.Data;
                lvi.SubItems.Clear();
                if (conn.Socket != null)
                {
                    lvi.Text = conn.Socket.Address;
                }
                else
                {
                    lvi.Text = "N/A";
                }
                lvi.SubItems.Add(conn.ReceivedFromSocket.ToString());
                lvi.SubItems.Add(conn.SentToSocket.ToString());
                lvi.SubItems.Add(conn.ReceivedFromChannel.ToString());
                lvi.SubItems.Add(conn.SentToChannel.ToString());
                switch (conn.SocketState)
                {
                    case TSBSSHForwardingSocketState.fssEstablishing:
                        s = "Establishing";
                        break;
                    case TSBSSHForwardingSocketState.fssActive:
                        s = "Active";
                        break;
                    case TSBSSHForwardingSocketState.fssClosing:
                        s = "Closing";
                        break;
                    case TSBSSHForwardingSocketState.fssClosed:
                        s = "Closed";
                        break;
                    default:
                        s = "";
                        break;
                }
                lvi.SubItems.Add(s);
                switch (conn.ChannelState)
                {
                    case TSBSSHForwardingChannelState.fcsEstablishing:
                        s = "Establishing";
                        break;
                    case TSBSSHForwardingChannelState.fcsActive:
                        s = "Active";
                        break;
                    case TSBSSHForwardingChannelState.fcsClosing:
                        s = "Closing";
                        break;
                    case TSBSSHForwardingChannelState.fcsClosed:
                        s = "Closed";
                        break;
                    default:
                        s = "";
                        break;
                }
                lvi.SubItems.Add(s);
                if ((conn.ChannelState == TSBSSHForwardingChannelState.fcsActive) &&
                    (conn.SocketState == TSBSSHForwardingSocketState.fssActive))
                {
                    lvi.ImageIndex = 1;
                }
                else if ((conn.ChannelState == TSBSSHForwardingChannelState.fcsEstablishing) ||
                    (conn.SocketState == TSBSSHForwardingSocketState.fssEstablishing))
                {
                    lvi.ImageIndex = 0;
                }
                else
                {
                    lvi.ImageIndex = 2;
                }
            }
		}

        delegate void RemoveConnectionFunc(ListViewItem lvi);

		private void RemoveConnection(ListViewItem lvi) 
		{
            if (lvConnections.InvokeRequired)
            {
                RemoveConnectionFunc d = new RemoveConnectionFunc(RemoveConnection);
                Invoke(d, new object[] { lvi });
            }
            else
            {
                lvConnections.Items.Remove(lvi);
            }
		}

        delegate void AddConnectionFunc(TElSSHForwardedConnection conn);

		private void AddConnection(TElSSHForwardedConnection conn) 
		{
            if (lvConnections.InvokeRequired)
            {
                AddConnectionFunc d = new AddConnectionFunc(AddConnection);
                Invoke(d, new object[] { conn });
            }
            else
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Tag = conn;
                conn.Data = lvi;
                lvConnections.Items.Add(lvi);
                RefreshConnection(conn);
            }
		}

		private void forwarding_OnAuthenticationFailed(object Sender, int AuthenticationType)
		{
			Log("Authentication [" + AuthenticationType.ToString() + "] failed", true);
		}

		private void forwarding_OnAuthenticationSuccess(object Sender)
		{
			Log("Authentication succeeded", false);
		}

		private void forwarding_OnClose(object Sender)
		{
			Log("SSH session closed", false);
			btnStart.Text = "Start";
		}

		private void forwarding_OnConnectionChange(object Sender, SBSSHForwarding.TElSSHForwardedConnection Conn)
		{
			RefreshConnection(Conn);
		}

		private void forwarding_OnConnectionClose(object Sender, SBSSHForwarding.TElSSHForwardedConnection Conn)
		{
			Log("Secure channel closed", false);
			RemoveConnection((ListViewItem)Conn.Data);
		}

		private void forwarding_OnConnectionError(object Sender, SBSSHForwarding.TElSSHForwardedConnection Conn, int ErrorCode)
		{
			Log("Secure channel error [" + ErrorCode.ToString() + "]", true);
		}

		private void forwarding_OnConnectionOpen(object Sender, SBSSHForwarding.TElSSHForwardedConnection Conn)
		{
			Log("Secure channel opened", false);
			AddConnection(Conn);
		}

		private void forwarding_OnError(object Sender, int ErrorCode)
		{
			Log("SSH error [" + ErrorCode.ToString() + "]", true);
		}

		private void forwarding_OnKeyValidate(object Sender, SBSSHKeyStorage.TElSSHKey ServerKey, ref bool Validate)
		{
			string s = SBUtils.Unit.DigestToStr128(ServerKey.FingerprintMD5, true);
			Log("Server key received [" + s + "]", false);
			Validate = true;
		}

		private void forwarding_OnOpen(object Sender)
		{
			Log("SSH session opened", false);
		}

		private void btnStart_Click(object sender, System.EventArgs e)
		{
			if (!forwarding.Active) 
			{
				forwarding.Address = tbHost.Text;
				forwarding.Port = Convert.ToInt32(tbPort.Text, 10);
				forwarding.ForwardedHost = "";
				forwarding.ForwardedPort = Convert.ToInt32(tbForwardedPort.Text, 10);
				forwarding.DestHost = tbDestHost.Text;
				forwarding.DestPort = Convert.ToInt32(tbDestPort.Text, 10);
				forwarding.Username = tbUsername.Text;
				forwarding.Password = tbPassword.Text;
				forwarding.Open();
				btnStart.Text = "Stop";
			} 
			else 
			{
				forwarding.Close();
			}
		}

		private void frmMain_Closed(object sender, System.EventArgs e)
		{
			if (forwarding.Active) 
			{
                forwarding.Close();
			}
		}
	}
}
