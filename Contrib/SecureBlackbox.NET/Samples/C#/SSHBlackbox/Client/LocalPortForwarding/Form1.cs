using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace LocalPortForwarding
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private SshSession session = null;
		private System.Windows.Forms.Panel pConnProps;
		private System.Windows.Forms.Panel pLog;
		private System.Windows.Forms.Panel pConnList;
		private System.Windows.Forms.Label lblHost;
		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.TextBox tbHost;
		private System.Windows.Forms.TextBox tbPort;
		private System.Windows.Forms.Label lblUsername;
		private System.Windows.Forms.TextBox tbUsername;
		private System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.ListView lvConnections;
		private System.Windows.Forms.Label lblConnProps;
		private System.Windows.Forms.ListView lvLog;
		private System.Windows.Forms.Label lblForwardingProps;
		private System.Windows.Forms.Label lblLocalPort;
		private System.Windows.Forms.Label lblRemoteHost;
		private System.Windows.Forms.TextBox tbLocalPort;
		private System.Windows.Forms.TextBox tbRemoteHost;
		private System.Windows.Forms.TextBox tbRemotePort;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.ColumnHeader chTime;
		private System.Windows.Forms.ColumnHeader chEvent;
		private System.Windows.Forms.ColumnHeader chHost;
		private System.Windows.Forms.ColumnHeader chReceived;
		private System.Windows.Forms.ColumnHeader chSent;
		private System.Windows.Forms.ColumnHeader chInputState;
		private System.Windows.Forms.ColumnHeader chOutputState;
		private System.Windows.Forms.ImageList imgConnections;
		private System.Windows.Forms.ImageList imgLog;
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
			this.pConnProps = new System.Windows.Forms.Panel();
			this.btnStart = new System.Windows.Forms.Button();
			this.tbRemotePort = new System.Windows.Forms.TextBox();
			this.tbRemoteHost = new System.Windows.Forms.TextBox();
			this.tbLocalPort = new System.Windows.Forms.TextBox();
			this.lblRemoteHost = new System.Windows.Forms.Label();
			this.lblLocalPort = new System.Windows.Forms.Label();
			this.lblForwardingProps = new System.Windows.Forms.Label();
			this.lblConnProps = new System.Windows.Forms.Label();
			this.lblPassword = new System.Windows.Forms.Label();
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.tbUsername = new System.Windows.Forms.TextBox();
			this.lblUsername = new System.Windows.Forms.Label();
			this.tbPort = new System.Windows.Forms.TextBox();
			this.tbHost = new System.Windows.Forms.TextBox();
			this.lblPort = new System.Windows.Forms.Label();
			this.lblHost = new System.Windows.Forms.Label();
			this.pLog = new System.Windows.Forms.Panel();
			this.lvLog = new System.Windows.Forms.ListView();
			this.chTime = new System.Windows.Forms.ColumnHeader();
			this.chEvent = new System.Windows.Forms.ColumnHeader();
			this.imgLog = new System.Windows.Forms.ImageList(this.components);
			this.pConnList = new System.Windows.Forms.Panel();
			this.lvConnections = new System.Windows.Forms.ListView();
			this.chHost = new System.Windows.Forms.ColumnHeader();
			this.chReceived = new System.Windows.Forms.ColumnHeader();
			this.chSent = new System.Windows.Forms.ColumnHeader();
			this.chInputState = new System.Windows.Forms.ColumnHeader();
			this.chOutputState = new System.Windows.Forms.ColumnHeader();
			this.imgConnections = new System.Windows.Forms.ImageList(this.components);
			this.pConnProps.SuspendLayout();
			this.pLog.SuspendLayout();
			this.pConnList.SuspendLayout();
			this.SuspendLayout();
			// 
			// pConnProps
			// 
			this.pConnProps.Controls.Add(this.btnStart);
			this.pConnProps.Controls.Add(this.tbRemotePort);
			this.pConnProps.Controls.Add(this.tbRemoteHost);
			this.pConnProps.Controls.Add(this.tbLocalPort);
			this.pConnProps.Controls.Add(this.lblRemoteHost);
			this.pConnProps.Controls.Add(this.lblLocalPort);
			this.pConnProps.Controls.Add(this.lblForwardingProps);
			this.pConnProps.Controls.Add(this.lblConnProps);
			this.pConnProps.Controls.Add(this.lblPassword);
			this.pConnProps.Controls.Add(this.tbPassword);
			this.pConnProps.Controls.Add(this.tbUsername);
			this.pConnProps.Controls.Add(this.lblUsername);
			this.pConnProps.Controls.Add(this.tbPort);
			this.pConnProps.Controls.Add(this.tbHost);
			this.pConnProps.Controls.Add(this.lblPort);
			this.pConnProps.Controls.Add(this.lblHost);
			this.pConnProps.Dock = System.Windows.Forms.DockStyle.Left;
			this.pConnProps.Location = new System.Drawing.Point(0, 0);
			this.pConnProps.Name = "pConnProps";
			this.pConnProps.Size = new System.Drawing.Size(200, 557);
			this.pConnProps.TabIndex = 0;
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(56, 312);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(80, 23);
			this.btnStart.TabIndex = 16;
			this.btnStart.Text = "Start";
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// tbRemotePort
			// 
			this.tbRemotePort.Location = new System.Drawing.Point(144, 264);
			this.tbRemotePort.Name = "tbRemotePort";
			this.tbRemotePort.Size = new System.Drawing.Size(48, 21);
			this.tbRemotePort.TabIndex = 15;
			this.tbRemotePort.Text = "5001";
			// 
			// tbRemoteHost
			// 
			this.tbRemoteHost.Location = new System.Drawing.Point(8, 264);
			this.tbRemoteHost.Name = "tbRemoteHost";
			this.tbRemoteHost.Size = new System.Drawing.Size(128, 21);
			this.tbRemoteHost.TabIndex = 14;
			this.tbRemoteHost.Text = "127.0.0.1";
			// 
			// tbLocalPort
			// 
			this.tbLocalPort.Location = new System.Drawing.Point(8, 216);
			this.tbLocalPort.Name = "tbLocalPort";
			this.tbLocalPort.Size = new System.Drawing.Size(56, 21);
			this.tbLocalPort.TabIndex = 13;
			this.tbLocalPort.Text = "5001";
			// 
			// lblRemoteHost
			// 
			this.lblRemoteHost.Location = new System.Drawing.Point(8, 248);
			this.lblRemoteHost.Name = "lblRemoteHost";
			this.lblRemoteHost.Size = new System.Drawing.Size(144, 16);
			this.lblRemoteHost.TabIndex = 11;
			this.lblRemoteHost.Text = "Remote host and port:";
			// 
			// lblLocalPort
			// 
			this.lblLocalPort.Location = new System.Drawing.Point(8, 200);
			this.lblLocalPort.Name = "lblLocalPort";
			this.lblLocalPort.Size = new System.Drawing.Size(112, 16);
			this.lblLocalPort.TabIndex = 10;
			this.lblLocalPort.Text = "Local port:";
			// 
			// lblForwardingProps
			// 
			this.lblForwardingProps.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.lblForwardingProps.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.lblForwardingProps.Location = new System.Drawing.Point(8, 176);
			this.lblForwardingProps.Name = "lblForwardingProps";
			this.lblForwardingProps.Size = new System.Drawing.Size(184, 16);
			this.lblForwardingProps.TabIndex = 9;
			this.lblForwardingProps.Text = "Forwarding settings";
			this.lblForwardingProps.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblConnProps
			// 
			this.lblConnProps.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.lblConnProps.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.lblConnProps.Location = new System.Drawing.Point(8, 8);
			this.lblConnProps.Name = "lblConnProps";
			this.lblConnProps.Size = new System.Drawing.Size(184, 16);
			this.lblConnProps.TabIndex = 8;
			this.lblConnProps.Text = "SSH settings";
			this.lblConnProps.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblPassword
			// 
			this.lblPassword.Location = new System.Drawing.Point(8, 128);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(104, 16);
			this.lblPassword.TabIndex = 7;
			this.lblPassword.Text = "Password:";
			// 
			// tbPassword
			// 
			this.tbPassword.Location = new System.Drawing.Point(8, 144);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.PasswordChar = '*';
			this.tbPassword.Size = new System.Drawing.Size(112, 21);
			this.tbPassword.TabIndex = 6;
			this.tbPassword.Text = "";
			// 
			// tbUsername
			// 
			this.tbUsername.Location = new System.Drawing.Point(8, 96);
			this.tbUsername.Name = "tbUsername";
			this.tbUsername.Size = new System.Drawing.Size(112, 21);
			this.tbUsername.TabIndex = 5;
			this.tbUsername.Text = "user";
			// 
			// lblUsername
			// 
			this.lblUsername.Location = new System.Drawing.Point(8, 80);
			this.lblUsername.Name = "lblUsername";
			this.lblUsername.Size = new System.Drawing.Size(128, 16);
			this.lblUsername.TabIndex = 4;
			this.lblUsername.Text = "Username:";
			// 
			// tbPort
			// 
			this.tbPort.Location = new System.Drawing.Point(144, 48);
			this.tbPort.Name = "tbPort";
			this.tbPort.Size = new System.Drawing.Size(48, 21);
			this.tbPort.TabIndex = 3;
			this.tbPort.Text = "22";
			// 
			// tbHost
			// 
			this.tbHost.Location = new System.Drawing.Point(8, 48);
			this.tbHost.Name = "tbHost";
			this.tbHost.Size = new System.Drawing.Size(128, 21);
			this.tbHost.TabIndex = 2;
			this.tbHost.Text = "192.168.0.1";
			// 
			// lblPort
			// 
			this.lblPort.Location = new System.Drawing.Point(144, 32);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(48, 16);
			this.lblPort.TabIndex = 1;
			this.lblPort.Text = "Port:";
			// 
			// lblHost
			// 
			this.lblHost.Location = new System.Drawing.Point(8, 32);
			this.lblHost.Name = "lblHost";
			this.lblHost.Size = new System.Drawing.Size(120, 16);
			this.lblHost.TabIndex = 0;
			this.lblHost.Text = "Server address:";
			// 
			// pLog
			// 
			this.pLog.Controls.Add(this.lvLog);
			this.pLog.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pLog.Location = new System.Drawing.Point(200, 365);
			this.pLog.Name = "pLog";
			this.pLog.Size = new System.Drawing.Size(624, 192);
			this.pLog.TabIndex = 1;
			// 
			// lvLog
			// 
			this.lvLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					this.chTime,
																					this.chEvent});
			this.lvLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvLog.FullRowSelect = true;
			this.lvLog.Location = new System.Drawing.Point(0, 0);
			this.lvLog.Name = "lvLog";
			this.lvLog.Size = new System.Drawing.Size(624, 192);
			this.lvLog.SmallImageList = this.imgLog;
			this.lvLog.TabIndex = 0;
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
			this.chEvent.Width = 519;
			// 
			// imgLog
			// 
			this.imgLog.ImageSize = new System.Drawing.Size(16, 16);
			this.imgLog.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLog.ImageStream")));
			this.imgLog.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// pConnList
			// 
			this.pConnList.Controls.Add(this.lvConnections);
			this.pConnList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pConnList.Location = new System.Drawing.Point(200, 0);
			this.pConnList.Name = "pConnList";
			this.pConnList.Size = new System.Drawing.Size(624, 365);
			this.pConnList.TabIndex = 2;
			// 
			// lvConnections
			// 
			this.lvConnections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.chHost,
																							this.chReceived,
																							this.chSent,
																							this.chInputState,
																							this.chOutputState});
			this.lvConnections.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvConnections.FullRowSelect = true;
			this.lvConnections.Location = new System.Drawing.Point(0, 0);
			this.lvConnections.Name = "lvConnections";
			this.lvConnections.Size = new System.Drawing.Size(624, 365);
			this.lvConnections.SmallImageList = this.imgConnections;
			this.lvConnections.TabIndex = 0;
			this.lvConnections.View = System.Windows.Forms.View.Details;
			// 
			// chHost
			// 
			this.chHost.Text = "Host";
			this.chHost.Width = 168;
			// 
			// chReceived
			// 
			this.chReceived.Text = "Received";
			this.chReceived.Width = 80;
			// 
			// chSent
			// 
			this.chSent.Text = "Sent";
			this.chSent.Width = 80;
			// 
			// chInputState
			// 
			this.chInputState.Text = "Input state";
			this.chInputState.Width = 80;
			// 
			// chOutputState
			// 
			this.chOutputState.Text = "Output state";
			this.chOutputState.Width = 80;
			// 
			// imgConnections
			// 
			this.imgConnections.ImageSize = new System.Drawing.Size(16, 16);
			this.imgConnections.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgConnections.ImageStream")));
			this.imgConnections.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(824, 557);
			this.Controls.Add(this.pConnList);
			this.Controls.Add(this.pLog);
			this.Controls.Add(this.pConnProps);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.Name = "frmMain";
			this.Text = "SSH local port forwarding demo application";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmMain_Closing);
			this.pConnProps.ResumeLayout(false);
			this.pLog.ResumeLayout(false);
			this.pConnList.ResumeLayout(false);
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

		private void btnStart_Click(object sender, System.EventArgs e)
		{
			if (session == null) 
			{
				btnStart.Text = "Stop";
				session = new SshSession();
				session.SshHost = tbHost.Text;
				session.SshPort = Convert.ToInt32(tbPort.Text, 10);
				session.ForwardPort = Convert.ToInt32(tbLocalPort.Text, 10);
				session.ForwardToHost = tbRemoteHost.Text;
				session.ForwardToPort = Convert.ToInt32(tbRemotePort.Text);
				session.Username = tbUsername.Text;
				session.Password = tbPassword.Text;
				session.OnLog += new LocalPortForwarding.SshSession.LogEvent(session_OnLog);
				session.OnConnectionAdd += new LocalPortForwarding.SshSession.ConnectionEvent(session_OnConnectionAdd);
				session.OnConnectionChange += new LocalPortForwarding.SshSession.ConnectionEvent(session_OnConnectionChange);
				session.OnConnectionRemove += new LocalPortForwarding.SshSession.ConnectionEvent(session_OnConnectionRemove);
				session.Connect();
			} 
			else 
			{
				btnStart.Text = "Start";
				session.Disconnect();
				session = null;
			}
		}

		private void session_OnLog(object sender, string s, bool error)
		{
			ListViewItem lvi = new ListViewItem();
			lvi.Text = DateTime.Now.ToShortTimeString();
			lvi.SubItems.Add(s);
			if (error) 
			{
				lvi.ImageIndex = 1;
			} 
			else 
			{
				lvi.ImageIndex = 0;
			}
            lvLog.Items.Add(lvi);
		}

		private void RefreshConnectionInfo(ListViewItem lvi)
		{
			SshForwarding connection = (SshForwarding)lvi.Tag;
			lvi.SubItems.Clear();
			lvi.Text = connection.Host;
			lvi.SubItems.Add(connection.Received.ToString());
			lvi.SubItems.Add(connection.Sent.ToString());
			string s = "";
			switch(connection.InState) 
			{
				case SshForwardingInState.Active:
					s = "Active";
					break;
				case SshForwardingInState.Closed:
					s = "Closed";
					break;
				case SshForwardingInState.Closing:
					s = "Closing";
					break;
			}
			lvi.SubItems.Add(s);
			s = "";
			switch(connection.OutState)
			{
				case SshForwardingOutState.Active:
					s = "Active";
					break;
				case SshForwardingOutState.Closed:
					s = "Closed";
					break;
				case SshForwardingOutState.Closing:
					s = "Closing";
					break;
				case SshForwardingOutState.Establishing:
					s = "Establishing";
					break;
			}
			lvi.SubItems.Add(s);
			int imageindex = -1;
			if ((connection.InState == SshForwardingInState.Active) && (connection.OutState == SshForwardingOutState.Active))
			{
				imageindex = 1;
			} 
			else if (connection.OutState == SshForwardingOutState.Establishing) 
			{
				imageindex = 0;
			} 
			else 
			{
				imageindex = 3;
			}
            lvi.ImageIndex = imageindex;
			lvi.Tag = connection;
		}

		private void session_OnConnectionAdd(object sender, SshForwarding connection)
		{
            ListViewItem lvi = new ListViewItem();
			lvi.Tag = connection;
			RefreshConnectionInfo(lvi);
			lvConnections.Items.Add(lvi);
		}

		private void session_OnConnectionChange(object sender, SshForwarding connection)
		{
			for (int i = 0; i < lvConnections.Items.Count; i++) 
			{
				if (lvConnections.Items[i].Tag == connection) 
				{
                    RefreshConnectionInfo(lvConnections.Items[i]);
					break;
				}
			}
		}

		private void session_OnConnectionRemove(object sender, SshForwarding connection)
		{
			for (int i = 0; i < lvConnections.Items.Count; i++) 
			{
				if (lvConnections.Items[i].Tag == connection) 
				{
					lvConnections.Items.Remove(lvConnections.Items[i]);
					break;
				}
			}
		}

		private void frmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (session != null) 
			{
                session.Disconnect();
			}
		}
	}
}
