using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace SSHServer.NET
{
	/// <summary>
	/// Implements SSH server demo main form
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		public System.Windows.Forms.ToolBar tbTop;
		private System.Windows.Forms.ToolBarButton tbStart;
		private System.Windows.Forms.ToolBarButton tbStop;
		private System.Windows.Forms.MainMenu mmMain;
		private System.Windows.Forms.ImageList ilToolbar;
		private System.Windows.Forms.ListView lvConnections;
		private System.Windows.Forms.ListView lvLog;
		private System.Windows.Forms.MenuItem miServer;
		private System.Windows.Forms.MenuItem miStart;
		private System.Windows.Forms.MenuItem miStop;
		private System.Windows.Forms.MenuItem miSeparator1;
		private System.Windows.Forms.MenuItem miExit;
		private System.Windows.Forms.MenuItem miHelp;
		private System.Windows.Forms.MenuItem miAbout;
		private System.Windows.Forms.ToolBarButton tbSeparator;
		private System.Windows.Forms.ToolBarButton tbServerSettings;
		private System.Windows.Forms.ToolBarButton tbSeparator2;
		private System.Windows.Forms.ColumnHeader chRemoteHost;
		private System.Windows.Forms.ColumnHeader chUser;
		private System.Windows.Forms.ColumnHeader chTunnels;
		private System.Windows.Forms.ColumnHeader chTime;
		private System.Windows.Forms.ColumnHeader chEvent;
		private System.Windows.Forms.ToolBarButton tbExit;
		private System.Windows.Forms.ImageList ilLog;
		private System.Windows.Forms.Splitter splitter;
		private System.Windows.Forms.ColumnHeader chStartTime;
		private System.Windows.Forms.ImageList ilConnections;
		private System.Windows.Forms.ColumnHeader chSoftware;
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
		}

		/// <summary>
		/// Adds log record to log window
		/// </summary>
		/// <param name="Message">log string</param>
		/// <param name="ErrorFlag">true if the message contains information about an error</param>
		public void AddLogEvent(string Message, bool ErrorFlag)
		{
            ListViewItem lvi = new ListViewItem(System.DateTime.Now.ToShortDateString() + " " +
                System.DateTime.Now.ToLongTimeString());
			lvi.SubItems.Add(Message);
			if (ErrorFlag) 
			{
				lvi.ImageIndex = 1; 
			}
			else 
			{
				lvi.ImageIndex = 0;
			}
            Utils.ListViewAddItem(this, lvLog, lvi);
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
			this.mmMain = new System.Windows.Forms.MainMenu();
			this.miServer = new System.Windows.Forms.MenuItem();
			this.miStart = new System.Windows.Forms.MenuItem();
			this.miStop = new System.Windows.Forms.MenuItem();
			this.miSeparator1 = new System.Windows.Forms.MenuItem();
			this.miExit = new System.Windows.Forms.MenuItem();
			this.miHelp = new System.Windows.Forms.MenuItem();
			this.miAbout = new System.Windows.Forms.MenuItem();
			this.ilToolbar = new System.Windows.Forms.ImageList(this.components);
			this.tbTop = new System.Windows.Forms.ToolBar();
			this.tbStart = new System.Windows.Forms.ToolBarButton();
			this.tbStop = new System.Windows.Forms.ToolBarButton();
			this.tbSeparator = new System.Windows.Forms.ToolBarButton();
			this.tbServerSettings = new System.Windows.Forms.ToolBarButton();
			this.tbSeparator2 = new System.Windows.Forms.ToolBarButton();
			this.tbExit = new System.Windows.Forms.ToolBarButton();
			this.lvConnections = new System.Windows.Forms.ListView();
			this.chRemoteHost = new System.Windows.Forms.ColumnHeader();
			this.chUser = new System.Windows.Forms.ColumnHeader();
			this.chTunnels = new System.Windows.Forms.ColumnHeader();
			this.lvLog = new System.Windows.Forms.ListView();
			this.chTime = new System.Windows.Forms.ColumnHeader();
			this.chEvent = new System.Windows.Forms.ColumnHeader();
			this.ilLog = new System.Windows.Forms.ImageList(this.components);
			this.splitter = new System.Windows.Forms.Splitter();
			this.chStartTime = new System.Windows.Forms.ColumnHeader();
			this.ilConnections = new System.Windows.Forms.ImageList(this.components);
			this.chSoftware = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// mmMain
			// 
			this.mmMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				   this.miServer,
																				   this.miHelp});
			// 
			// miServer
			// 
			this.miServer.Index = 0;
			this.miServer.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.miStart,
																					 this.miStop,
																					 this.miSeparator1,
																					 this.miExit});
			this.miServer.Text = "Server";
			// 
			// miStart
			// 
			this.miStart.Index = 0;
			this.miStart.Text = "&Start";
			// 
			// miStop
			// 
			this.miStop.Index = 1;
			this.miStop.Text = "S&top";
			// 
			// miSeparator1
			// 
			this.miSeparator1.Index = 2;
			this.miSeparator1.Text = "-";
			// 
			// miExit
			// 
			this.miExit.Index = 3;
			this.miExit.Text = "E&xit";
			this.miExit.Click += new System.EventHandler(this.miExit_Click);
			// 
			// miHelp
			// 
			this.miHelp.Index = 1;
			this.miHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				   this.miAbout});
			this.miHelp.Text = "Help";
			// 
			// miAbout
			// 
			this.miAbout.Index = 0;
			this.miAbout.Text = "About...";
			this.miAbout.Click += new System.EventHandler(this.miAbout_Click);
			// 
			// ilToolbar
			// 
			this.ilToolbar.ColorDepth = System.Windows.Forms.ColorDepth.Depth4Bit;
			this.ilToolbar.ImageSize = new System.Drawing.Size(16, 16);
			this.ilToolbar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilToolbar.ImageStream")));
			this.ilToolbar.TransparentColor = System.Drawing.Color.Fuchsia;
			// 
			// tbTop
			// 
			this.tbTop.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.tbTop.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																					 this.tbStart,
																					 this.tbStop,
																					 this.tbSeparator,
																					 this.tbServerSettings,
																					 this.tbSeparator2,
																					 this.tbExit});
			this.tbTop.Divider = false;
			this.tbTop.DropDownArrows = true;
			this.tbTop.ImageList = this.ilToolbar;
			this.tbTop.Location = new System.Drawing.Point(0, 0);
			this.tbTop.Name = "tbTop";
			this.tbTop.ShowToolTips = true;
			this.tbTop.Size = new System.Drawing.Size(584, 40);
			this.tbTop.TabIndex = 0;
			this.tbTop.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbTop_ButtonClick);
			// 
			// tbStart
			// 
			this.tbStart.ImageIndex = 0;
			this.tbStart.Text = "Start";
			// 
			// tbStop
			// 
			this.tbStop.Enabled = false;
			this.tbStop.ImageIndex = 1;
			this.tbStop.Text = "Stop";
			// 
			// tbSeparator
			// 
			this.tbSeparator.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tbServerSettings
			// 
			this.tbServerSettings.ImageIndex = 2;
			this.tbServerSettings.Text = "Settings";
			// 
			// tbSeparator2
			// 
			this.tbSeparator2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tbExit
			// 
			this.tbExit.ImageIndex = 3;
			this.tbExit.Text = "Exit";
			// 
			// lvConnections
			// 
			this.lvConnections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.chRemoteHost,
																							this.chUser,
																							this.chTunnels,
																							this.chStartTime,
																							this.chSoftware});
			this.lvConnections.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvConnections.GridLines = true;
			this.lvConnections.Location = new System.Drawing.Point(0, 40);
			this.lvConnections.Name = "lvConnections";
			this.lvConnections.Size = new System.Drawing.Size(584, 374);
			this.lvConnections.SmallImageList = this.ilConnections;
			this.lvConnections.TabIndex = 1;
			this.lvConnections.View = System.Windows.Forms.View.Details;
			// 
			// chRemoteHost
			// 
			this.chRemoteHost.Text = "Remote host";
			this.chRemoteHost.Width = 200;
			// 
			// chUser
			// 
			this.chUser.Text = "User";
			this.chUser.Width = 140;
			// 
			// chTunnels
			// 
			this.chTunnels.Text = "Tunnels";
			this.chTunnels.Width = 100;
			// 
			// lvLog
			// 
			this.lvLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					this.chTime,
																					this.chEvent});
			this.lvLog.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lvLog.GridLines = true;
			this.lvLog.Location = new System.Drawing.Point(0, 278);
			this.lvLog.Name = "lvLog";
			this.lvLog.Size = new System.Drawing.Size(584, 136);
			this.lvLog.SmallImageList = this.ilLog;
			this.lvLog.TabIndex = 3;
			this.lvLog.View = System.Windows.Forms.View.Details;
			// 
			// chTime
			// 
			this.chTime.Text = "Time";
			this.chTime.Width = 120;
			// 
			// chEvent
			// 
			this.chEvent.Text = "Event";
			this.chEvent.Width = 430;
			// 
			// ilLog
			// 
			this.ilLog.ColorDepth = System.Windows.Forms.ColorDepth.Depth4Bit;
			this.ilLog.ImageSize = new System.Drawing.Size(16, 16);
			this.ilLog.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilLog.ImageStream")));
			this.ilLog.TransparentColor = System.Drawing.Color.Fuchsia;
			// 
			// splitter
			// 
			this.splitter.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter.Location = new System.Drawing.Point(0, 275);
			this.splitter.Name = "splitter";
			this.splitter.Size = new System.Drawing.Size(584, 3);
			this.splitter.TabIndex = 4;
			this.splitter.TabStop = false;
			// 
			// chStartTime
			// 
			this.chStartTime.Text = "Start time";
			this.chStartTime.Width = 100;
			// 
			// ilConnections
			// 
			this.ilConnections.ImageSize = new System.Drawing.Size(16, 16);
			this.ilConnections.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilConnections.ImageStream")));
			this.ilConnections.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// chSoftware
			// 
			this.chSoftware.Text = "Client software";
			this.chSoftware.Width = 110;
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(584, 414);
			this.Controls.Add(this.splitter);
			this.Controls.Add(this.lvLog);
			this.Controls.Add(this.lvConnections);
			this.Controls.Add(this.tbTop);
			this.Menu = this.mmMain;
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SSHServer .NET demo";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			// Calling SetLicenseKey
			SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("BWagiShCqafilXshs9gaHG7bNhz+wszKCIo4bWWO2SewF0To37xZBxdjH0L4QO1jwDIt2heqcSAQIRVR/81gQqt2ni2P4IPBUzjJmQIjJ4P6swqmXuvumDOLj8vBr+NC33lobB4Vh+bj6sy/nacg8SuoDGbOIZG96DK7WatFkjaxL8Hv/A/dsZiGmy6V/FxVk/5taf6pWsA+l9T3jJSMha0Y5ViafoJ+fQmrBP63xpwKp+0lMPiu5iO85wXU854WRM8ihyw0JcKiYCNK40EPMmQv4Gg3gfxoM/WlunMGSIvOT30T2R6JLa5MkI2SQVvwX2GjgMuMt5bwRzNLWP55+g=="));
			Globals.main = new frmMain();
			if (Globals.Settings.Empty == true) 
			{
				MessageBox.Show("Welcome to the ElSSHServer Demo Application!\n" + 
					"Thank you for your interest in our products.\n\n" +
					"First of all, you need to create at least one user account.\n\n" + 
					"After clicking the OK button you will be redirected to the\n" +
					"User settings window.",
					"SSH Server demo",MessageBoxButtons.OK, MessageBoxIcon.Information);
				frmSettings.ChangeSettings();
			}
			Globals.SessionStarted += new SSHServer.NET.Globals.SessionStartedHandler(Globals_SessionStarted);
			Globals.SessionClosed += new SSHServer.NET.Globals.SessionClosedHandler(Globals_SessionClosed);
			Globals.SessionInfoChanged += new SSHServer.NET.Globals.SessionInfoChangedHandler(Globals_SessionInfoChanged);
			Application.Run(Globals.main);
			if (Globals.ServerStarted) 
			{
				Globals.StopSSHListener();
			}
			Globals.main = null;
		}

		private void miAbout_Click(object sender, System.EventArgs e)
		{
			frmAbout.ShowAboutBox();
		}

		private void tbTop_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			switch (tbTop.Buttons.IndexOf(e.Button))
			{
				case 0 : 
				{
					Globals.StartSSHListener();
					break;
				}
				case 1:
				{
					Globals.StopSSHListener();
					break;
				}
				case 3 :
				{
					frmSettings.ChangeSettings();
					break;
				}
				case 5 :
				{
					this.Close();
					break;
				}
			}
		}

		private void miExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private static void Globals_SessionStarted(SSHSession session)
		{
            Logger.Log("SSH session started");
		}

		private static void Globals_SessionClosed(SSHSession session)
		{
			Logger.Log("SSH session closed");
			ListViewItem item = (ListViewItem)session.Data;
			if (item != null) 
			{
                Globals.main.lvConnections.Items.Remove(item);
			}
		}

		private static void Globals_SessionInfoChanged(SSHSession session)
		{
			ListViewItem item;
			if (session.Data == null) 
			{
				item = new ListViewItem();
				item.ImageIndex = 0;
				session.Data = item;
                Utils.ListViewAddItem(Globals.main, Globals.main.lvConnections, item);
			} 
			else 
			{
                item = (ListViewItem)session.Data;
			}
            Utils.ListViewItemClearSubItems(Globals.main, Globals.main.lvConnections, item);
            Utils.ListViewItemAddSubItem(Globals.main, Globals.main.lvConnections, item, session.Username);
            Utils.ListViewItemAddSubItem(Globals.main, Globals.main.lvConnections, item, session.Status);
            Utils.ListViewItemAddSubItem(Globals.main, Globals.main.lvConnections, item, session.StartTime.ToLongTimeString());
            Utils.ListViewItemAddSubItem(Globals.main, Globals.main.lvConnections, item, session.ClientSoftware);
            Utils.ListViewItemSetText(Globals.main, Globals.main.lvConnections, item, session.Host); 
		}
	}
}
