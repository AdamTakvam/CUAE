using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using Microsoft.Win32;

namespace Metreos.RecordAgent
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class FormMain : System.Windows.Forms.Form
	{
        private abstract class Consts
        {
            public const string CONFIG_SALESFORCE			= "salesforce";
        }

        private abstract class DefaultValues
        {
            public const string DEFAULT_SALESFORCE         = "0";
        }

        private System.Windows.Forms.NotifyIcon notifyIconMain;
        private System.Windows.Forms.ContextMenu contextMenuMain;
        private System.Windows.Forms.MenuItem menuItemExit;
        private System.Windows.Forms.MenuItem menuItemOpen;
        private System.ComponentModel.IContainer components;

        private const int WM_QUERYENDSESSION = 0x11;
        private bool systemShutdown = false;
        private FormWindowState lastWindowState = FormWindowState.Normal;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageActivaCalls;
        private System.Windows.Forms.TabPage tabPageCallList;
        private UIComponents.XPPanelGroup xpPanelGroup1;
        private System.Windows.Forms.LinkLabel linkLabel_StartNow1;
        private System.Windows.Forms.LinkLabel linkLabel_Start1;
        private System.Windows.Forms.LinkLabel linkLabel_Stop1;
        private System.Windows.Forms.LinkLabel linkLabel_Note1;
        private System.Windows.Forms.Label label_Status1;
        private System.Windows.Forms.Label label_Timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.LinkLabel linkLabel4;
        private ActiveCallPanel xpPanel1;
        private ActiveCallPanel xpPanel2;
        private ActiveCallPanel xpPanel3;
        private System.Windows.Forms.Label label_Timer2;
        private System.Windows.Forms.Label label_Status2;
        private System.Windows.Forms.LinkLabel linkLabel_Note2;
        private System.Windows.Forms.LinkLabel linkLabel_Stop2;
        private System.Windows.Forms.LinkLabel linkLabel_StartNow2;
        private System.Windows.Forms.LinkLabel linkLabel_Start2;
        private System.Windows.Forms.Label label_Status3;
        private System.Windows.Forms.LinkLabel linkLabel_Note3;
        private System.Windows.Forms.LinkLabel linkLabel_Stop3;
        private System.Windows.Forms.LinkLabel linkLabel_StartNow3;
        private System.Windows.Forms.LinkLabel linkLabel_Start3;
        private System.Windows.Forms.Label label_Timer3;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem menuItem12;
        private System.Windows.Forms.MenuItem menuItem14;
        private System.Windows.Forms.MenuItem menuItem19;
        private System.Windows.Forms.MenuItem menuItemCallSeperator;
        private System.Windows.Forms.MenuItem menuItemCall3;
        private System.Windows.Forms.MenuItem menuItem_Start3;
        private System.Windows.Forms.MenuItem menuItem_StartNow3;
        private System.Windows.Forms.MenuItem menuItem_Stop3;
        private System.Windows.Forms.MenuItem menuItem_TakeNote3;
        private System.Windows.Forms.MenuItem menuItemCall2;
        private System.Windows.Forms.MenuItem menuItem_Start2;
        private System.Windows.Forms.MenuItem menuItem_StartNow2;
        private System.Windows.Forms.MenuItem menuItem_Stop2;
        private System.Windows.Forms.MenuItem menuItem_TakeNote2;
        private System.Windows.Forms.MenuItem menuItemCall1;
        private System.Windows.Forms.MenuItem menuItem_Start1;
        private System.Windows.Forms.MenuItem menuItem_StartNow1;
        private System.Windows.Forms.MenuItem menuItem_Stop1;
        private System.Windows.Forms.MenuItem menuItem21;
        private System.Windows.Forms.MenuItem menuItem_TakeNote1;

        private Agent agent;
        private UIComponents.ImageSet imageSetCallState;
        private System.Windows.Forms.ImageList imageListTray;
        private Hashtable ConversationTable;
        private ServiceManager serviceManager;
        private System.Windows.Forms.MenuItem menuItemSalesForce;
        private System.Windows.Forms.MenuItem menuItem1;
        private RecordList recordList1;
        private SforceManager sforceManager;

		public FormMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            ConversationTable = new Hashtable();

            serviceManager = ServiceManager.Instance;

            sforceManager = SforceManager.Instance;

            agent = new Agent();            
            agent.IpcClient.onServiceUp += new NullDelegate(IpcClient_onServiceUp);
            agent.IpcClient.onServiceDown += new NullDelegate(IpcClient_onServiceDown);
            agent.IpcClient.onNewCall += new OnNewCallDelegate(agent_onNewCall);
            agent.IpcClient.onCallStatusUpdate += new OnCallStatusUpdateDelegate(agent_onCallStatusUpdate);
            agent.onCallStatusUpdated += new Metreos.RecordAgent.Agent.OnCallStatusUpdated(agent_onCallStatusUpdated);
            agent.onNewCallReceived += new Metreos.RecordAgent.Agent.OnNewCallReceived(agent_onNewCallReceived);
            agent.onRecordFromNotifier += new Metreos.RecordAgent.Agent.OnRecordFromNotifierDelegate(agent_onRecordFromNotifier);

            xpPanel1.onPanelCallStateUpdate += new OnPanelCallStateUpdateDelegate(panel_onPanelCallStateUpdate);
            xpPanel1.onPanelCallTimeUpdate += new OnPanelCallTimeUpdateDelegate(panel_onPanelCallTimeUpdate);
            xpPanel1.onPanelCallInfoUpdate += new OnPanelCallInfoUpdateDelegate(panel_onPanelCallInfoUpdate);
            xpPanel2.onPanelCallStateUpdate += new OnPanelCallStateUpdateDelegate(panel_onPanelCallStateUpdate);
            xpPanel2.onPanelCallTimeUpdate += new OnPanelCallTimeUpdateDelegate(panel_onPanelCallTimeUpdate);
            xpPanel2.onPanelCallInfoUpdate += new OnPanelCallInfoUpdateDelegate(panel_onPanelCallInfoUpdate);
            xpPanel3.onPanelCallStateUpdate += new OnPanelCallStateUpdateDelegate(panel_onPanelCallStateUpdate);
            xpPanel3.onPanelCallTimeUpdate += new OnPanelCallTimeUpdateDelegate(panel_onPanelCallTimeUpdate);
            xpPanel3.onPanelCallInfoUpdate += new OnPanelCallInfoUpdateDelegate(panel_onPanelCallInfoUpdate);
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
                ConversationTable.Clear();
                notifyIconMain.Icon.Dispose ();
                notifyIconMain.Dispose(); 

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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FormMain));
            this.notifyIconMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuMain = new System.Windows.Forms.ContextMenu();
            this.menuItemCall1 = new System.Windows.Forms.MenuItem();
            this.menuItem_Start1 = new System.Windows.Forms.MenuItem();
            this.menuItem_StartNow1 = new System.Windows.Forms.MenuItem();
            this.menuItem19 = new System.Windows.Forms.MenuItem();
            this.menuItem_Stop1 = new System.Windows.Forms.MenuItem();
            this.menuItem21 = new System.Windows.Forms.MenuItem();
            this.menuItem_TakeNote1 = new System.Windows.Forms.MenuItem();
            this.menuItemCall2 = new System.Windows.Forms.MenuItem();
            this.menuItem_Start2 = new System.Windows.Forms.MenuItem();
            this.menuItem_StartNow2 = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.menuItem_Stop2 = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.menuItem_TakeNote2 = new System.Windows.Forms.MenuItem();
            this.menuItemCall3 = new System.Windows.Forms.MenuItem();
            this.menuItem_Start3 = new System.Windows.Forms.MenuItem();
            this.menuItem_StartNow3 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem_Stop3 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem_TakeNote3 = new System.Windows.Forms.MenuItem();
            this.menuItemCallSeperator = new System.Windows.Forms.MenuItem();
            this.menuItemOpen = new System.Windows.Forms.MenuItem();
            this.menuItemSalesForce = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageActivaCalls = new System.Windows.Forms.TabPage();
            this.xpPanelGroup1 = new UIComponents.XPPanelGroup();
            this.xpPanel3 = new ActiveCallPanel(104);
            this.label_Timer3 = new System.Windows.Forms.Label();
            this.label_Status3 = new System.Windows.Forms.Label();
            this.linkLabel_Note3 = new System.Windows.Forms.LinkLabel();
            this.linkLabel_Stop3 = new System.Windows.Forms.LinkLabel();
            this.linkLabel_StartNow3 = new System.Windows.Forms.LinkLabel();
            this.linkLabel_Start3 = new System.Windows.Forms.LinkLabel();
            this.imageSetCallState = new UIComponents.ImageSet();
            this.xpPanel2 = new ActiveCallPanel(104);
            this.label_Timer2 = new System.Windows.Forms.Label();
            this.label_Status2 = new System.Windows.Forms.Label();
            this.linkLabel_Note2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel_Stop2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel_StartNow2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel_Start2 = new System.Windows.Forms.LinkLabel();
            this.xpPanel1 = new ActiveCallPanel(104);
            this.label_Timer1 = new System.Windows.Forms.Label();
            this.label_Status1 = new System.Windows.Forms.Label();
            this.linkLabel_Note1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel_Stop1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel_StartNow1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel_Start1 = new System.Windows.Forms.LinkLabel();
            this.tabPageCallList = new System.Windows.Forms.TabPage();
            this.recordList1 = new Metreos.RecordAgent.RecordList();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.imageListTray = new System.Windows.Forms.ImageList(this.components);
            this.tabControlMain.SuspendLayout();
            this.tabPageActivaCalls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xpPanelGroup1)).BeginInit();
            this.xpPanelGroup1.SuspendLayout();
            this.xpPanel3.SuspendLayout();
            this.xpPanel2.SuspendLayout();
            this.xpPanel1.SuspendLayout();
            this.tabPageCallList.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIconMain
            // 
            this.notifyIconMain.ContextMenu = this.contextMenuMain;
            this.notifyIconMain.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconMain.Icon")));
            this.notifyIconMain.Text = "Metreos Record Agent";
            this.notifyIconMain.Visible = true;
            this.notifyIconMain.DoubleClick += new System.EventHandler(this.notifyIconMain_DoubleClick);
            // 
            // contextMenuMain
            // 
            this.contextMenuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                            this.menuItemCall1,
                                                                                            this.menuItemCall2,
                                                                                            this.menuItemCall3,
                                                                                            this.menuItemCallSeperator,
                                                                                            this.menuItemOpen,
                                                                                            this.menuItemSalesForce,
                                                                                            this.menuItem1,
                                                                                            this.menuItemExit});
            // 
            // menuItemCall1
            // 
            this.menuItemCall1.Index = 0;
            this.menuItemCall1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                          this.menuItem_Start1,
                                                                                          this.menuItem_StartNow1,
                                                                                          this.menuItem19,
                                                                                          this.menuItem_Stop1,
                                                                                          this.menuItem21,
                                                                                          this.menuItem_TakeNote1});
            this.menuItemCall1.Text = "Call 1";
            this.menuItemCall1.Visible = false;
            // 
            // menuItem_Start1
            // 
            this.menuItem_Start1.Index = 0;
            this.menuItem_Start1.Text = "Start Recording";
            this.menuItem_Start1.Click += new System.EventHandler(this.StartRecord);
            // 
            // menuItem_StartNow1
            // 
            this.menuItem_StartNow1.Index = 1;
            this.menuItem_StartNow1.Text = "Start Recording Now";
            this.menuItem_StartNow1.Click += new System.EventHandler(this.StartRecordNow);
            // 
            // menuItem19
            // 
            this.menuItem19.Index = 2;
            this.menuItem19.Text = "-";
            // 
            // menuItem_Stop1
            // 
            this.menuItem_Stop1.Index = 3;
            this.menuItem_Stop1.Text = "Stop Recording";
            this.menuItem_Stop1.Click += new System.EventHandler(this.StopRecord);
            // 
            // menuItem21
            // 
            this.menuItem21.Index = 4;
            this.menuItem21.Text = "-";
            // 
            // menuItem_TakeNote1
            // 
            this.menuItem_TakeNote1.Index = 5;
            this.menuItem_TakeNote1.Text = "Attach Notes";
            this.menuItem_TakeNote1.Click += new System.EventHandler(this.TakeNote);
            // 
            // menuItemCall2
            // 
            this.menuItemCall2.Index = 1;
            this.menuItemCall2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                          this.menuItem_Start2,
                                                                                          this.menuItem_StartNow2,
                                                                                          this.menuItem12,
                                                                                          this.menuItem_Stop2,
                                                                                          this.menuItem14,
                                                                                          this.menuItem_TakeNote2});
            this.menuItemCall2.Text = "Call 2";
            this.menuItemCall2.Visible = false;
            // 
            // menuItem_Start2
            // 
            this.menuItem_Start2.Index = 0;
            this.menuItem_Start2.Text = "Start Recording";
            this.menuItem_Start2.Click += new System.EventHandler(this.StartRecord);
            // 
            // menuItem_StartNow2
            // 
            this.menuItem_StartNow2.Index = 1;
            this.menuItem_StartNow2.Text = "Start Recording Now";
            this.menuItem_StartNow2.Click += new System.EventHandler(this.StartRecordNow);
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 2;
            this.menuItem12.Text = "-";
            // 
            // menuItem_Stop2
            // 
            this.menuItem_Stop2.Index = 3;
            this.menuItem_Stop2.Text = "Stop Recording";
            this.menuItem_Stop2.Click += new System.EventHandler(this.StopRecord);
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 4;
            this.menuItem14.Text = "-";
            // 
            // menuItem_TakeNote2
            // 
            this.menuItem_TakeNote2.Index = 5;
            this.menuItem_TakeNote2.Text = "Attach Notes";
            this.menuItem_TakeNote2.Click += new System.EventHandler(this.TakeNote);
            // 
            // menuItemCall3
            // 
            this.menuItemCall3.Index = 2;
            this.menuItemCall3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                          this.menuItem_Start3,
                                                                                          this.menuItem_StartNow3,
                                                                                          this.menuItem5,
                                                                                          this.menuItem_Stop3,
                                                                                          this.menuItem7,
                                                                                          this.menuItem_TakeNote3});
            this.menuItemCall3.Text = "Call 3";
            this.menuItemCall3.Visible = false;
            // 
            // menuItem_Start3
            // 
            this.menuItem_Start3.Index = 0;
            this.menuItem_Start3.Text = "Start Recording";
            this.menuItem_Start3.Click += new System.EventHandler(this.StartRecord);
            // 
            // menuItem_StartNow3
            // 
            this.menuItem_StartNow3.Index = 1;
            this.menuItem_StartNow3.Text = "Start Recording Now";
            this.menuItem_StartNow3.Click += new System.EventHandler(this.StartRecordNow);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 2;
            this.menuItem5.Text = "-";
            // 
            // menuItem_Stop3
            // 
            this.menuItem_Stop3.Index = 3;
            this.menuItem_Stop3.Text = "Stop Recording";
            this.menuItem_Stop3.Click += new System.EventHandler(this.StopRecord);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 4;
            this.menuItem7.Text = "-";
            // 
            // menuItem_TakeNote3
            // 
            this.menuItem_TakeNote3.Index = 5;
            this.menuItem_TakeNote3.Text = "Attach Notes";
            this.menuItem_TakeNote3.Click += new System.EventHandler(this.TakeNote);
            // 
            // menuItemCallSeperator
            // 
            this.menuItemCallSeperator.Index = 3;
            this.menuItemCallSeperator.Text = "-";
            this.menuItemCallSeperator.Visible = false;
            // 
            // menuItemOpen
            // 
            this.menuItemOpen.DefaultItem = true;
            this.menuItemOpen.Index = 4;
            this.menuItemOpen.Text = "Open Metreos Record Agent";
            this.menuItemOpen.Click += new System.EventHandler(this.menuItemOpen_Click);
            // 
            // menuItemSalesForce
            // 
            this.menuItemSalesForce.Index = 5;
            this.menuItemSalesForce.Text = "salesforce.com";
            this.menuItemSalesForce.Visible = false;
            this.menuItemSalesForce.Click += new System.EventHandler(this.menuItemSalesForce_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 6;
            this.menuItem1.Text = "-";
            // 
            // menuItemExit
            // 
            this.menuItemExit.Index = 7;
            this.menuItemExit.Text = "Exit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageActivaCalls);
            this.tabControlMain.Controls.Add(this.tabPageCallList);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(488, 376);
            this.tabControlMain.TabIndex = 1;
            // 
            // tabPageActivaCalls
            // 
            this.tabPageActivaCalls.Controls.Add(this.xpPanelGroup1);
            this.tabPageActivaCalls.Location = new System.Drawing.Point(4, 22);
            this.tabPageActivaCalls.Name = "tabPageActivaCalls";
            this.tabPageActivaCalls.Size = new System.Drawing.Size(480, 350);
            this.tabPageActivaCalls.TabIndex = 0;
            this.tabPageActivaCalls.Text = "Active Calls";
            this.tabPageActivaCalls.ToolTipText = "Active Calls";
            // 
            // xpPanelGroup1
            // 
            this.xpPanelGroup1.AutoScroll = true;
            this.xpPanelGroup1.BackColor = System.Drawing.Color.Transparent;
            this.xpPanelGroup1.Controls.Add(this.xpPanel3);
            this.xpPanelGroup1.Controls.Add(this.xpPanel2);
            this.xpPanelGroup1.Controls.Add(this.xpPanel1);
            this.xpPanelGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xpPanelGroup1.Location = new System.Drawing.Point(0, 0);
            this.xpPanelGroup1.Name = "xpPanelGroup1";
            this.xpPanelGroup1.PanelGradient = ((UIComponents.GradientColor)(resources.GetObject("xpPanelGroup1.PanelGradient")));
            this.xpPanelGroup1.Size = new System.Drawing.Size(480, 350);
            this.xpPanelGroup1.TabIndex = 1;
            // 
            // xpPanel3
            // 
            this.xpPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.xpPanel3.BackColor = System.Drawing.Color.Transparent;
            this.xpPanel3.Caption = "Call 3";
            this.xpPanel3.CaptionCornerType = UIComponents.CornerType.Top;
            this.xpPanel3.CaptionGradient.End = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(213)), ((System.Byte)(247)));
            this.xpPanel3.CaptionGradient.Start = System.Drawing.Color.White;
            this.xpPanel3.CaptionGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.xpPanel3.CaptionUnderline = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(255)));
            this.xpPanel3.CollapsedGlyphs.ImageSet = null;
            this.xpPanel3.Controls.Add(this.label_Timer3);
            this.xpPanel3.Controls.Add(this.label_Status3);
            this.xpPanel3.Controls.Add(this.linkLabel_Note3);
            this.xpPanel3.Controls.Add(this.linkLabel_Stop3);
            this.xpPanel3.Controls.Add(this.linkLabel_StartNow3);
            this.xpPanel3.Controls.Add(this.linkLabel_Start3);
            this.xpPanel3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.xpPanel3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.xpPanel3.HorzAlignment = System.Drawing.StringAlignment.Near;
            this.xpPanel3.ImageItems.ImageSet = this.imageSetCallState;
            this.xpPanel3.ImageItems.Normal = 0;
            this.xpPanel3.Location = new System.Drawing.Point(8, 232);
            this.xpPanel3.Name = "xpPanel3";
            this.xpPanel3.PanelGradient.End = System.Drawing.Color.FromArgb(((System.Byte)(214)), ((System.Byte)(223)), ((System.Byte)(247)));
            this.xpPanel3.PanelGradient.Start = System.Drawing.Color.FromArgb(((System.Byte)(214)), ((System.Byte)(223)), ((System.Byte)(247)));
            this.xpPanel3.PanelGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.xpPanel3.Size = new System.Drawing.Size(464, 104);
            this.xpPanel3.TabIndex = 2;
            this.xpPanel3.TextColors.Foreground = System.Drawing.Color.FromArgb(((System.Byte)(33)), ((System.Byte)(93)), ((System.Byte)(198)));
            this.xpPanel3.TextHighlightColors.Foreground = System.Drawing.Color.FromArgb(((System.Byte)(66)), ((System.Byte)(142)), ((System.Byte)(255)));
            this.xpPanel3.VertAlignment = System.Drawing.StringAlignment.Center;
            this.xpPanel3.Visible = false;
            this.xpPanel3.XPPanelStyle = UIComponents.XPPanelStyle.WindowsXP;
            // 
            // label_Timer3
            // 
            this.label_Timer3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label_Timer3.Location = new System.Drawing.Point(328, 72);
            this.label_Timer3.Name = "label_Timer3";
            this.label_Timer3.TabIndex = 5;
            this.label_Timer3.Text = "00:00";
            this.label_Timer3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Status3
            // 
            this.label_Status3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label_Status3.Location = new System.Drawing.Point(328, 40);
            this.label_Status3.Name = "label_Status3";
            this.label_Status3.TabIndex = 4;
            this.label_Status3.Text = "Connected";
            this.label_Status3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // linkLabel_Note3
            // 
            this.linkLabel_Note3.ActiveLinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkLabel_Note3.DisabledLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(22)), ((System.Byte)(33)), ((System.Byte)(84)));
            this.linkLabel_Note3.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel_Note3.Image")));
            this.linkLabel_Note3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel_Note3.Location = new System.Drawing.Point(176, 72);
            this.linkLabel_Note3.Name = "linkLabel_Note3";
            this.linkLabel_Note3.Size = new System.Drawing.Size(120, 23);
            this.linkLabel_Note3.TabIndex = 3;
            this.linkLabel_Note3.TabStop = true;
            this.linkLabel_Note3.Text = "Attach Notes";
            this.linkLabel_Note3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel_Note3.Click += new System.EventHandler(this.TakeNote);
            // 
            // linkLabel_Stop3
            // 
            this.linkLabel_Stop3.ActiveLinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkLabel_Stop3.DisabledLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(22)), ((System.Byte)(33)), ((System.Byte)(84)));
            this.linkLabel_Stop3.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel_Stop3.Image")));
            this.linkLabel_Stop3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel_Stop3.Location = new System.Drawing.Point(176, 40);
            this.linkLabel_Stop3.Name = "linkLabel_Stop3";
            this.linkLabel_Stop3.Size = new System.Drawing.Size(144, 23);
            this.linkLabel_Stop3.TabIndex = 2;
            this.linkLabel_Stop3.TabStop = true;
            this.linkLabel_Stop3.Text = "Stop Recording";
            this.linkLabel_Stop3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel_Stop3.Click += new System.EventHandler(this.StopRecord);
            // 
            // linkLabel_StartNow3
            // 
            this.linkLabel_StartNow3.ActiveLinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkLabel_StartNow3.DisabledLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(22)), ((System.Byte)(33)), ((System.Byte)(84)));
            this.linkLabel_StartNow3.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel_StartNow3.Image")));
            this.linkLabel_StartNow3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel_StartNow3.Location = new System.Drawing.Point(8, 72);
            this.linkLabel_StartNow3.Name = "linkLabel_StartNow3";
            this.linkLabel_StartNow3.Size = new System.Drawing.Size(168, 23);
            this.linkLabel_StartNow3.TabIndex = 1;
            this.linkLabel_StartNow3.TabStop = true;
            this.linkLabel_StartNow3.Text = "Start Recording Now";
            this.linkLabel_StartNow3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel_StartNow3.Click += new System.EventHandler(this.StartRecordNow);
            // 
            // linkLabel_Start3
            // 
            this.linkLabel_Start3.ActiveLinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkLabel_Start3.DisabledLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(22)), ((System.Byte)(33)), ((System.Byte)(84)));
            this.linkLabel_Start3.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel_Start3.Image")));
            this.linkLabel_Start3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel_Start3.Location = new System.Drawing.Point(8, 40);
            this.linkLabel_Start3.Name = "linkLabel_Start3";
            this.linkLabel_Start3.Size = new System.Drawing.Size(144, 23);
            this.linkLabel_Start3.TabIndex = 0;
            this.linkLabel_Start3.TabStop = true;
            this.linkLabel_Start3.Text = "Start Recording";
            this.linkLabel_Start3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel_Start3.Click += new System.EventHandler(this.StartRecord);
            // 
            // imageSetCallState
            // 
            this.imageSetCallState.Images.AddRange(new System.Drawing.Image[] {
                                                                                  ((System.Drawing.Image)(resources.GetObject("resource"))),
                                                                                  ((System.Drawing.Image)(resources.GetObject("resource1"))),
                                                                                  ((System.Drawing.Image)(resources.GetObject("resource2")))});
            this.imageSetCallState.Size = new System.Drawing.Size(32, 32);
            this.imageSetCallState.TransparentColor = System.Drawing.Color.Empty;
            // 
            // xpPanel2
            // 
            this.xpPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.xpPanel2.BackColor = System.Drawing.Color.Transparent;
            this.xpPanel2.Caption = "Call 2";
            this.xpPanel2.CaptionCornerType = UIComponents.CornerType.Top;
            this.xpPanel2.CaptionGradient.End = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(213)), ((System.Byte)(247)));
            this.xpPanel2.CaptionGradient.Start = System.Drawing.Color.White;
            this.xpPanel2.CaptionGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.xpPanel2.CaptionUnderline = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(255)));
            this.xpPanel2.CollapsedGlyphs.ImageSet = null;
            this.xpPanel2.Controls.Add(this.label_Timer2);
            this.xpPanel2.Controls.Add(this.label_Status2);
            this.xpPanel2.Controls.Add(this.linkLabel_Note2);
            this.xpPanel2.Controls.Add(this.linkLabel_Stop2);
            this.xpPanel2.Controls.Add(this.linkLabel_StartNow2);
            this.xpPanel2.Controls.Add(this.linkLabel_Start2);
            this.xpPanel2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.xpPanel2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.xpPanel2.HorzAlignment = System.Drawing.StringAlignment.Near;
            this.xpPanel2.ImageItems.ImageSet = this.imageSetCallState;
            this.xpPanel2.ImageItems.Normal = 0;
            this.xpPanel2.Location = new System.Drawing.Point(8, 120);
            this.xpPanel2.Name = "xpPanel2";
            this.xpPanel2.PanelGradient.End = System.Drawing.Color.FromArgb(((System.Byte)(214)), ((System.Byte)(223)), ((System.Byte)(247)));
            this.xpPanel2.PanelGradient.Start = System.Drawing.Color.FromArgb(((System.Byte)(214)), ((System.Byte)(223)), ((System.Byte)(247)));
            this.xpPanel2.PanelGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.xpPanel2.Size = new System.Drawing.Size(464, 104);
            this.xpPanel2.TabIndex = 1;
            this.xpPanel2.TextColors.Foreground = System.Drawing.Color.FromArgb(((System.Byte)(33)), ((System.Byte)(93)), ((System.Byte)(198)));
            this.xpPanel2.TextHighlightColors.Foreground = System.Drawing.Color.FromArgb(((System.Byte)(66)), ((System.Byte)(142)), ((System.Byte)(255)));
            this.xpPanel2.VertAlignment = System.Drawing.StringAlignment.Center;
            this.xpPanel2.Visible = false;
            this.xpPanel2.XPPanelStyle = UIComponents.XPPanelStyle.WindowsXP;
            // 
            // label_Timer2
            // 
            this.label_Timer2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label_Timer2.Location = new System.Drawing.Point(328, 72);
            this.label_Timer2.Name = "label_Timer2";
            this.label_Timer2.TabIndex = 5;
            this.label_Timer2.Text = "00:00";
            this.label_Timer2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Status2
            // 
            this.label_Status2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label_Status2.Location = new System.Drawing.Point(328, 40);
            this.label_Status2.Name = "label_Status2";
            this.label_Status2.TabIndex = 4;
            this.label_Status2.Text = "Connected";
            this.label_Status2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // linkLabel_Note2
            // 
            this.linkLabel_Note2.ActiveLinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkLabel_Note2.DisabledLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(22)), ((System.Byte)(33)), ((System.Byte)(84)));
            this.linkLabel_Note2.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel_Note2.Image")));
            this.linkLabel_Note2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel_Note2.Location = new System.Drawing.Point(176, 72);
            this.linkLabel_Note2.Name = "linkLabel_Note2";
            this.linkLabel_Note2.Size = new System.Drawing.Size(120, 23);
            this.linkLabel_Note2.TabIndex = 3;
            this.linkLabel_Note2.TabStop = true;
            this.linkLabel_Note2.Text = "Attach Notes";
            this.linkLabel_Note2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel_Note2.Click += new System.EventHandler(this.TakeNote);
            // 
            // linkLabel_Stop2
            // 
            this.linkLabel_Stop2.ActiveLinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkLabel_Stop2.DisabledLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(22)), ((System.Byte)(33)), ((System.Byte)(84)));
            this.linkLabel_Stop2.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel_Stop2.Image")));
            this.linkLabel_Stop2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel_Stop2.Location = new System.Drawing.Point(176, 40);
            this.linkLabel_Stop2.Name = "linkLabel_Stop2";
            this.linkLabel_Stop2.Size = new System.Drawing.Size(144, 23);
            this.linkLabel_Stop2.TabIndex = 2;
            this.linkLabel_Stop2.TabStop = true;
            this.linkLabel_Stop2.Text = "Stop Recording";
            this.linkLabel_Stop2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel_Stop2.Click += new System.EventHandler(this.StopRecord);
            // 
            // linkLabel_StartNow2
            // 
            this.linkLabel_StartNow2.ActiveLinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkLabel_StartNow2.DisabledLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(22)), ((System.Byte)(33)), ((System.Byte)(84)));
            this.linkLabel_StartNow2.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel_StartNow2.Image")));
            this.linkLabel_StartNow2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel_StartNow2.Location = new System.Drawing.Point(8, 72);
            this.linkLabel_StartNow2.Name = "linkLabel_StartNow2";
            this.linkLabel_StartNow2.Size = new System.Drawing.Size(168, 23);
            this.linkLabel_StartNow2.TabIndex = 1;
            this.linkLabel_StartNow2.TabStop = true;
            this.linkLabel_StartNow2.Text = "Start Recording Now";
            this.linkLabel_StartNow2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel_StartNow2.Click += new System.EventHandler(this.StartRecordNow);
            // 
            // linkLabel_Start2
            // 
            this.linkLabel_Start2.ActiveLinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkLabel_Start2.DisabledLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(22)), ((System.Byte)(33)), ((System.Byte)(84)));
            this.linkLabel_Start2.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel_Start2.Image")));
            this.linkLabel_Start2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel_Start2.Location = new System.Drawing.Point(8, 40);
            this.linkLabel_Start2.Name = "linkLabel_Start2";
            this.linkLabel_Start2.Size = new System.Drawing.Size(144, 23);
            this.linkLabel_Start2.TabIndex = 0;
            this.linkLabel_Start2.TabStop = true;
            this.linkLabel_Start2.Text = "Start Recording";
            this.linkLabel_Start2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel_Start2.Click += new System.EventHandler(this.StartRecord);
            // 
            // xpPanel1
            // 
            this.xpPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.xpPanel1.BackColor = System.Drawing.Color.Transparent;
            this.xpPanel1.Caption = "Call 1";
            this.xpPanel1.CaptionCornerType = UIComponents.CornerType.Top;
            this.xpPanel1.CaptionGradient.End = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(213)), ((System.Byte)(247)));
            this.xpPanel1.CaptionGradient.Start = System.Drawing.Color.White;
            this.xpPanel1.CaptionGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.xpPanel1.CaptionUnderline = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(255)));
            this.xpPanel1.CollapsedGlyphs.ImageSet = null;
            this.xpPanel1.Controls.Add(this.label_Timer1);
            this.xpPanel1.Controls.Add(this.label_Status1);
            this.xpPanel1.Controls.Add(this.linkLabel_Note1);
            this.xpPanel1.Controls.Add(this.linkLabel_Stop1);
            this.xpPanel1.Controls.Add(this.linkLabel_StartNow1);
            this.xpPanel1.Controls.Add(this.linkLabel_Start1);
            this.xpPanel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.xpPanel1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.xpPanel1.HorzAlignment = System.Drawing.StringAlignment.Near;
            this.xpPanel1.ImageItems.ImageSet = this.imageSetCallState;
            this.xpPanel1.ImageItems.Normal = 0;
            this.xpPanel1.Location = new System.Drawing.Point(8, 8);
            this.xpPanel1.Name = "xpPanel1";
            this.xpPanel1.PanelGradient.End = System.Drawing.Color.FromArgb(((System.Byte)(214)), ((System.Byte)(223)), ((System.Byte)(247)));
            this.xpPanel1.PanelGradient.Start = System.Drawing.Color.FromArgb(((System.Byte)(214)), ((System.Byte)(223)), ((System.Byte)(247)));
            this.xpPanel1.PanelGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.xpPanel1.Size = new System.Drawing.Size(464, 104);
            this.xpPanel1.TabIndex = 0;
            this.xpPanel1.TextColors.Foreground = System.Drawing.Color.FromArgb(((System.Byte)(33)), ((System.Byte)(93)), ((System.Byte)(198)));
            this.xpPanel1.TextHighlightColors.Foreground = System.Drawing.Color.FromArgb(((System.Byte)(66)), ((System.Byte)(142)), ((System.Byte)(255)));
            this.xpPanel1.VertAlignment = System.Drawing.StringAlignment.Center;
            this.xpPanel1.Visible = false;
            this.xpPanel1.XPPanelStyle = UIComponents.XPPanelStyle.WindowsXP;
            // 
            // label_Timer1
            // 
            this.label_Timer1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label_Timer1.Location = new System.Drawing.Point(328, 72);
            this.label_Timer1.Name = "label_Timer1";
            this.label_Timer1.TabIndex = 5;
            this.label_Timer1.Text = "00:00";
            this.label_Timer1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Status1
            // 
            this.label_Status1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label_Status1.Location = new System.Drawing.Point(328, 40);
            this.label_Status1.Name = "label_Status1";
            this.label_Status1.TabIndex = 4;
            this.label_Status1.Text = "Connected";
            this.label_Status1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // linkLabel_Note1
            // 
            this.linkLabel_Note1.ActiveLinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkLabel_Note1.DisabledLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(22)), ((System.Byte)(33)), ((System.Byte)(84)));
            this.linkLabel_Note1.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel_Note1.Image")));
            this.linkLabel_Note1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel_Note1.Location = new System.Drawing.Point(176, 72);
            this.linkLabel_Note1.Name = "linkLabel_Note1";
            this.linkLabel_Note1.Size = new System.Drawing.Size(120, 23);
            this.linkLabel_Note1.TabIndex = 3;
            this.linkLabel_Note1.TabStop = true;
            this.linkLabel_Note1.Text = "Attach Notes";
            this.linkLabel_Note1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel_Note1.Click += new System.EventHandler(this.TakeNote);
            // 
            // linkLabel_Stop1
            // 
            this.linkLabel_Stop1.ActiveLinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkLabel_Stop1.DisabledLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(22)), ((System.Byte)(33)), ((System.Byte)(84)));
            this.linkLabel_Stop1.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel_Stop1.Image")));
            this.linkLabel_Stop1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel_Stop1.Location = new System.Drawing.Point(176, 40);
            this.linkLabel_Stop1.Name = "linkLabel_Stop1";
            this.linkLabel_Stop1.Size = new System.Drawing.Size(144, 23);
            this.linkLabel_Stop1.TabIndex = 2;
            this.linkLabel_Stop1.TabStop = true;
            this.linkLabel_Stop1.Text = "Stop Recording";
            this.linkLabel_Stop1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel_Stop1.Click += new System.EventHandler(this.StopRecord);
            // 
            // linkLabel_StartNow1
            // 
            this.linkLabel_StartNow1.ActiveLinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkLabel_StartNow1.DisabledLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(22)), ((System.Byte)(33)), ((System.Byte)(84)));
            this.linkLabel_StartNow1.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel_StartNow1.Image")));
            this.linkLabel_StartNow1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel_StartNow1.Location = new System.Drawing.Point(8, 72);
            this.linkLabel_StartNow1.Name = "linkLabel_StartNow1";
            this.linkLabel_StartNow1.Size = new System.Drawing.Size(168, 23);
            this.linkLabel_StartNow1.TabIndex = 1;
            this.linkLabel_StartNow1.TabStop = true;
            this.linkLabel_StartNow1.Text = "Start Recording Now";
            this.linkLabel_StartNow1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel_StartNow1.Click += new System.EventHandler(this.StartRecordNow);
            // 
            // linkLabel_Start1
            // 
            this.linkLabel_Start1.ActiveLinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkLabel_Start1.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel_Start1.DisabledLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(22)), ((System.Byte)(33)), ((System.Byte)(84)));
            this.linkLabel_Start1.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel_Start1.Image")));
            this.linkLabel_Start1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel_Start1.Location = new System.Drawing.Point(8, 40);
            this.linkLabel_Start1.Name = "linkLabel_Start1";
            this.linkLabel_Start1.Size = new System.Drawing.Size(144, 23);
            this.linkLabel_Start1.TabIndex = 0;
            this.linkLabel_Start1.TabStop = true;
            this.linkLabel_Start1.Text = "Start Recording";
            this.linkLabel_Start1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel_Start1.Click += new System.EventHandler(this.StartRecord);
            // 
            // tabPageCallList
            // 
            this.tabPageCallList.Controls.Add(this.recordList1);
            this.tabPageCallList.Location = new System.Drawing.Point(4, 22);
            this.tabPageCallList.Name = "tabPageCallList";
            this.tabPageCallList.Size = new System.Drawing.Size(480, 350);
            this.tabPageCallList.TabIndex = 1;
            this.tabPageCallList.Text = "Recorded Calls";
            this.tabPageCallList.ToolTipText = "Recorded Calls";
            // 
            // recordList1
            // 
            this.recordList1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.recordList1.Cursor = System.Windows.Forms.Cursors.Default;
            this.recordList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recordList1.Location = new System.Drawing.Point(0, 0);
            this.recordList1.Name = "recordList1";
            this.recordList1.Size = new System.Drawing.Size(480, 350);
            this.recordList1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(328, 72);
            this.label1.Name = "label1";
            this.label1.TabIndex = 5;
            this.label1.Text = "00:00";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(328, 40);
            this.label2.Name = "label2";
            this.label2.TabIndex = 4;
            this.label2.Text = "Connected";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // linkLabel1
            // 
            this.linkLabel1.ActiveLinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkLabel1.DisabledLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(22)), ((System.Byte)(33)), ((System.Byte)(84)));
            this.linkLabel1.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel1.Image")));
            this.linkLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel1.Location = new System.Drawing.Point(176, 72);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(112, 23);
            this.linkLabel1.TabIndex = 3;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Take Note";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabel2
            // 
            this.linkLabel2.ActiveLinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkLabel2.DisabledLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(22)), ((System.Byte)(33)), ((System.Byte)(84)));
            this.linkLabel2.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel2.Image")));
            this.linkLabel2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel2.Location = new System.Drawing.Point(176, 40);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(120, 23);
            this.linkLabel2.TabIndex = 2;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Stop Recording";
            this.linkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabel3
            // 
            this.linkLabel3.ActiveLinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkLabel3.DisabledLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(22)), ((System.Byte)(33)), ((System.Byte)(84)));
            this.linkLabel3.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel3.Image")));
            this.linkLabel3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel3.Location = new System.Drawing.Point(8, 72);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(144, 23);
            this.linkLabel3.TabIndex = 1;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "Start Recording Now";
            this.linkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabel4
            // 
            this.linkLabel4.ActiveLinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkLabel4.DisabledLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(22)), ((System.Byte)(33)), ((System.Byte)(84)));
            this.linkLabel4.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel4.Image")));
            this.linkLabel4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel4.Location = new System.Drawing.Point(8, 40);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(120, 23);
            this.linkLabel4.TabIndex = 0;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "Start Recording";
            this.linkLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imageListTray
            // 
            this.imageListTray.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListTray.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTray.ImageStream")));
            this.imageListTray.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // FormMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(488, 376);
            this.Controls.Add(this.tabControlMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Metreos Record Agent";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FormMain_Closing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageActivaCalls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xpPanelGroup1)).EndInit();
            this.xpPanelGroup1.ResumeLayout(false);
            this.xpPanel3.ResumeLayout(false);
            this.xpPanel2.ResumeLayout(false);
            this.xpPanel1.ResumeLayout(false);
            this.tabPageCallList.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
            RegistryKey OurKey = Registry.LocalMachine;
            OurKey = OurKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths", false);
            string license = OurKey.GetValue("Components").ToString();
            OurKey.Close();
            string [] keys = license.Split('-');

            try
            {
                if (keys != null && keys.Length == 5)
                {
                    string yearkey = keys[3];
                    if (yearkey == null)
                    {
                        MessageBox.Show("Your trial license has expired!\nPlease contact Metreos Corporation for purchase information.", "Metreos Record Agent",
                            MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;     // malformed, expire it
                    }

                    if (!yearkey.EndsWith("E"))     // if ends with E, this is purchased license
                    {
                        int duration = Convert.ToInt32(yearkey.Substring(3, 1)) * 30;
                        yearkey = "20" + yearkey.Substring(1, 2);
                        int year = Convert.ToInt32(yearkey);

                        string daykey = keys[1];
                        if (daykey == null)
                        {
                            MessageBox.Show("Your trial license has expired!\nPlease contact Metreos Corporation for purchase information.", "Metreos Record Agent",
                                MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;     // malformed, expire it
                        }
                        String delim = "CF";
                        daykey = daykey.Trim(delim.ToCharArray());
                        int day = Convert.ToInt32(daykey);
                        day -= 51;

                        string monthkey = keys[2];
                        if (monthkey == null)
                        {
                            MessageBox.Show("Your trial license has expired!\nPlease contact Metreos Corporation for purchase information.", "Metreos Record Agent",
                                MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;     // malformed, expire it
                        }
                        delim = "BD";
                        monthkey = monthkey.Trim(delim.ToCharArray());
                        int month = Convert.ToInt32(monthkey);
                        month -= 68;

                        DateTime startDate = new DateTime(year, month, day);
                        if (DateTime.Now > startDate.AddDays(duration))
                        {
                            MessageBox.Show("Your trial license has expired!\nPlease contact Metreos Corporation for purchase information.", "Metreos Record Agent",
                                            MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;     // expired
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Your trial license has expired!\nPlease contact Metreos Corporation for purchase information.", "Metreos Record Agent",
                MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            Process aProcess = Process.GetCurrentProcess();
            string aProcName = aProcess.ProcessName;
			
            if (Process.GetProcessesByName(aProcName).Length > 1)
            {
                Application.ExitThread();
                return;
            }

            Application.EnableVisualStyles(); // Enable XP styles
            Application.DoEvents();           // Work around MS ShowDialog() bug
			Application.Run(new FormMain());
		}

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_QUERYENDSESSION)
                systemShutdown = true;
            base.WndProc(ref m);
        }

        private void FormMain_Load(object sender, System.EventArgs e)
        {
            serviceManager.StartService();
            notifyIconMain.Icon = GetIconFromImageList(imageListTray, 0);
            agent.Start();

            xpPanelGroup1.ShowNoItems(true);

            string sfFlag = DefaultValues.DEFAULT_SALESFORCE;
            sfFlag = ConfigurationSettings.AppSettings[Consts.CONFIG_SALESFORCE];
            menuItemSalesForce.Visible = (sfFlag.CompareTo(DefaultValues.DEFAULT_SALESFORCE) == 0) ? false : true;
        }

        #region Main Form Window Management
        /// <summary>
        /// When user double click on system tray icon, restore the original window state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIconMain_DoubleClick(object sender, System.EventArgs e)
        {
            Show();
            this.ShowInTaskbar = true;
            WindowState = lastWindowState;               
        }

        /// <summary>
        /// Minimize window to system tray when try to close it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(systemShutdown == true)
            {
                e.Cancel = false;
                serviceManager.StopService();
            }
            else
            {
                lastWindowState = this.WindowState;
                e.Cancel = true;
                Hide();
            }
        }
        #endregion

        #region Tray icon context menu handlers
        private void menuItemExit_Click(object sender, System.EventArgs e)
        {
            agent.Stop();
            notifyIconMain.Visible = false;
            serviceManager.StopService();
            Application.Exit();
        }

        private void menuItemOpen_Click(object sender, System.EventArgs e)
        {
            Show();            
            this.ShowInTaskbar = true;
            WindowState = lastWindowState;               
        }
        #endregion

        private void agent_onNewCall(uint callIdentifier, CallData callData)
        {
            OnNewCallDelegate del = new OnNewCallDelegate(agent.OnNewCall);
            this.BeginInvoke(del, new object[] {callIdentifier, callData});
        }

        private void agent_onCallStatusUpdate(uint callIdentifier, CallData callData)
        {
            notifyIconMain.Icon = GetIconFromImageList(imageListTray, 2);
            OnCallStatusUpdateDelegate del = new OnCallStatusUpdateDelegate(agent.OnCallStatusUpdate);
            this.BeginInvoke(del, new object[] {callIdentifier, callData});
        }

        private void agent_onNewCallReceived(uint callIdentifier, ActiveCall call)
        {
            if (!this.xpPanel1.IsActiveCall)
            {
                this.xpPanel1.AssignCall(call.CallData);
                this.menuItemCall1.Visible = true;
                this.menuItemCallSeperator.Visible = true;
            }
            else if (!this.xpPanel2.IsActiveCall)
            {
                this.xpPanel2.AssignCall(call.CallData);
                this.menuItemCall2.Visible = true;
                this.menuItemCallSeperator.Visible = true;
            }
            else if (!this.xpPanel3.IsActiveCall)
            {
                this.xpPanel3.AssignCall(call.CallData);
                this.menuItemCall3.Visible = true;
                this.menuItemCallSeperator.Visible = true;
            }

            xpPanelGroup1.ShowNoItems(false);
        }

        private void agent_onCallStatusUpdated(uint callIdentifier, ActiveCall call)
        {
            // Propergate to panels
            if (this.xpPanel1.IsActiveCall && this.xpPanel1.ActiveCall.CallData.CallIdentifier == callIdentifier)
                this.xpPanel1.UpdateCallStatus(call.CallData);
            else if (this.xpPanel2.IsActiveCall && this.xpPanel2.ActiveCall.CallData.CallIdentifier == callIdentifier)
                this.xpPanel2.UpdateCallStatus(call.CallData);
            else if (this.xpPanel3.IsActiveCall && this.xpPanel3.ActiveCall.CallData.CallIdentifier == callIdentifier)
                this.xpPanel3.UpdateCallStatus(call.CallData);

            if (call.CallData.CallState == CallState.ONHOOK)
            {
                agent.RemoveCall(callIdentifier);

                if (!xpPanel1.IsActiveCall && !xpPanel2.IsActiveCall && !xpPanel3.IsActiveCall)
                    xpPanelGroup1.ShowNoItems(true);
            }
        }

        private void agent_onRecordFromNotifier(uint callIdentifier)
        {
            if (this.xpPanel1.IsActiveCall && this.xpPanel1.ActiveCall.CallData.CallIdentifier == callIdentifier)
            {
                this.linkLabel_StartNow1.Enabled = false;
                this.linkLabel_Start1.Enabled = false;
                this.linkLabel_Stop1.Enabled = true;

                this.menuItem_Start1.Enabled = false;
                this.menuItem_StartNow1.Enabled = false;
                this.menuItem_Stop1.Enabled = true;

                xpPanel1.ImageItems.Normal = 2;
                xpPanel1.Invalidate();
            }
            else if (this.xpPanel2.IsActiveCall && this.xpPanel2.ActiveCall.CallData.CallIdentifier == callIdentifier)
            {
                this.linkLabel_StartNow2.Enabled = false;
                this.linkLabel_Start2.Enabled = false;
                this.linkLabel_Stop2.Enabled = true;

                this.menuItem_Start2.Enabled = false;
                this.menuItem_StartNow2.Enabled = false;
                this.menuItem_Stop2.Enabled = true;

                xpPanel2.ImageItems.Normal = 2;
                xpPanel2.Invalidate();
            }
            else if (this.xpPanel3.IsActiveCall && this.xpPanel3.ActiveCall.CallData.CallIdentifier == callIdentifier)
            {
                this.linkLabel_StartNow3.Enabled = false;
                this.linkLabel_Start3.Enabled = false;
                this.linkLabel_Stop3.Enabled = true;

                this.menuItem_Start3.Enabled = false;
                this.menuItem_StartNow3.Enabled = false;
                this.menuItem_Stop3.Enabled = true;

                xpPanel3.ImageItems.Normal = 2;
                xpPanel3.Invalidate();
            }
        }

        private void StartRecord(object sender, System.EventArgs e)
        {
            if (sender.Equals(this.linkLabel_Start1) || sender.Equals(this.menuItem_Start1))
            {
                this.linkLabel_StartNow1.Enabled = false;
                this.linkLabel_Start1.Enabled = false;
                this.linkLabel_Stop1.Enabled = true;

                this.menuItem_Start1.Enabled = false;
                this.menuItem_StartNow1.Enabled = false;
                this.menuItem_Stop1.Enabled = true;

                xpPanel1.ImageItems.Normal = 2;
                xpPanel1.Invalidate();

                agent.OnStartRecord(this.xpPanel1.ActiveCall.CallData.CallIdentifier);
            }
            else if (sender.Equals(this.linkLabel_Start2) || sender.Equals(this.menuItem_Start2))
            {
                this.linkLabel_StartNow2.Enabled = false;
                this.linkLabel_Start2.Enabled = false;
                this.linkLabel_Stop2.Enabled = true;

                this.menuItem_Start2.Enabled = false;
                this.menuItem_StartNow2.Enabled = false;
                this.menuItem_Stop2.Enabled = true;

                xpPanel2.ImageItems.Normal = 2;
                xpPanel2.Invalidate();

                agent.OnStartRecord(this.xpPanel2.ActiveCall.CallData.CallIdentifier);
            }
            else if (sender.Equals(this.linkLabel_Start3) || sender.Equals(this.menuItem_Start3))
            {
                this.linkLabel_StartNow3.Enabled = false;
                this.linkLabel_Start3.Enabled = false;
                this.linkLabel_Stop3.Enabled = true;

                this.menuItem_Start3.Enabled = false;
                this.menuItem_StartNow3.Enabled = false;
                this.menuItem_Stop3.Enabled = true;

                xpPanel3.ImageItems.Normal = 2;
                xpPanel3.Invalidate();

                agent.OnStartRecord(this.xpPanel3.ActiveCall.CallData.CallIdentifier);
            }
        }

        private void StartRecordNow(object sender, System.EventArgs e)
        {
            if (sender.Equals(this.linkLabel_StartNow1) || sender.Equals(this.menuItem_StartNow1))
            {
                this.linkLabel_StartNow1.Enabled = false;
                this.linkLabel_Start1.Enabled = false;
                this.linkLabel_Stop1.Enabled = true;

                this.menuItem_Start1.Enabled = false;
                this.menuItem_StartNow1.Enabled = false;
                this.menuItem_Stop1.Enabled = true;

                xpPanel1.ImageItems.Normal = 2;
                xpPanel1.Invalidate();

                agent.OnStartRecordNow(this.xpPanel1.ActiveCall.CallData.CallIdentifier);
            }
            else if (sender.Equals(this.linkLabel_StartNow2) || sender.Equals(this.menuItem_StartNow2))
            {
                this.linkLabel_StartNow2.Enabled = false;
                this.linkLabel_Start2.Enabled = false;
                this.linkLabel_Stop2.Enabled = true;

                this.menuItem_Start2.Enabled = false;
                this.menuItem_StartNow2.Enabled = false;
                this.menuItem_Stop2.Enabled = true;

                xpPanel2.ImageItems.Normal = 2;
                xpPanel2.Invalidate();

                agent.OnStartRecordNow(this.xpPanel2.ActiveCall.CallData.CallIdentifier);
            }
            else if (sender.Equals(this.linkLabel_StartNow3) || sender.Equals(this.menuItem_StartNow3))
            {
                this.linkLabel_StartNow3.Enabled = false;
                this.linkLabel_Start3.Enabled = false;
                this.linkLabel_Stop3.Enabled = true;

                this.menuItem_Start3.Enabled = false;
                this.menuItem_StartNow3.Enabled = false;
                this.menuItem_Stop3.Enabled = true;

                xpPanel3.ImageItems.Normal = 2;
                xpPanel3.Invalidate();

                agent.OnStartRecordNow(this.xpPanel3.ActiveCall.CallData.CallIdentifier);
            }
        }

        private void StopRecord(object sender, System.EventArgs e)
        {
            if (sender.Equals(this.linkLabel_Stop1) || sender.Equals(this.menuItem_Stop1))
            {
                this.linkLabel_StartNow1.Enabled = true;
                this.linkLabel_Start1.Enabled = true;
                this.linkLabel_Stop1.Enabled = false;

                this.menuItem_Start1.Enabled = true;
                this.menuItem_StartNow1.Enabled = true;
                this.menuItem_Stop1.Enabled = false;

                if (xpPanel1.ActiveCall.CallData.CallState == CallState.CONNECTED)
                    xpPanel1.ImageItems.Normal = 0;
                else if (xpPanel1.ActiveCall.CallData.CallState == CallState.HOLD)
                    xpPanel1.ImageItems.Normal = 1;
                xpPanel1.Invalidate();

                agent.OnStopRecord(this.xpPanel1.ActiveCall.CallData.CallIdentifier);
            }
            else if (sender.Equals(this.linkLabel_Stop2) || sender.Equals(this.menuItem_Stop2))
            {
                this.linkLabel_StartNow2.Enabled = true;
                this.linkLabel_Start2.Enabled = true;
                this.linkLabel_Stop2.Enabled = false;

                this.menuItem_Start2.Enabled = true;
                this.menuItem_StartNow2.Enabled = true;
                this.menuItem_Stop2.Enabled = false;

                if (xpPanel2.ActiveCall.CallData.CallState == CallState.CONNECTED)
                    xpPanel2.ImageItems.Normal = 0;
                else if (xpPanel2.ActiveCall.CallData.CallState == CallState.HOLD)
                    xpPanel2.ImageItems.Normal = 1;
                xpPanel2.Invalidate();

                agent.OnStopRecord(this.xpPanel2.ActiveCall.CallData.CallIdentifier);
            }
            else if (sender.Equals(this.linkLabel_Stop3) || sender.Equals(this.menuItem_Stop3))
            {
                this.linkLabel_StartNow3.Enabled = true;
                this.linkLabel_Start3.Enabled = true;
                this.linkLabel_Stop3.Enabled = false;

                this.menuItem_Start3.Enabled = true;
                this.menuItem_StartNow3.Enabled = true;
                this.menuItem_Stop3.Enabled = false;

                if (xpPanel3.ActiveCall.CallData.CallState == CallState.CONNECTED)
                    xpPanel3.ImageItems.Normal = 0;
                else if (xpPanel3.ActiveCall.CallData.CallState == CallState.HOLD)
                    xpPanel3.ImageItems.Normal = 1;
                xpPanel3.Invalidate();

                agent.OnStopRecord(this.xpPanel3.ActiveCall.CallData.CallIdentifier);
            }
        }

        private void TakeNote(object sender, System.EventArgs e)
        {
            uint callIdentifier = 0;
            int ticks = 0;
            string title = "";
            MetaDataForm conv = null;
            if (sender.Equals(this.linkLabel_Note1) || sender.Equals(this.menuItem_TakeNote1))
            {
                callIdentifier = xpPanel1.ActiveCall.CallData.CallIdentifier;
                ticks = xpPanel1.Ticks;
                title = xpPanel1.Caption;
            }
            else if (sender.Equals(this.linkLabel_Note2) || sender.Equals(this.menuItem_TakeNote2))
            {
                callIdentifier = xpPanel2.ActiveCall.CallData.CallIdentifier;
                ticks = xpPanel2.Ticks;
                title = xpPanel2.Caption;
            }
            else if (sender.Equals(this.linkLabel_Note3) || sender.Equals(this.menuItem_TakeNote3))
            {
                callIdentifier = xpPanel3.ActiveCall.CallData.CallIdentifier;
                ticks = xpPanel3.Ticks;
                title = xpPanel3.Caption;
            }     
   
            if (callIdentifier == 0)
                return;

            lock(ConversationTable.SyncRoot)
            {
                if (ConversationTable.ContainsKey(callIdentifier))
                {
                    conv = ConversationTable[callIdentifier] as MetaDataForm;
                    if (conv != null)
                    {
                        if (conv.IsDisposed)
                        {
                            conv = new MetaDataForm(callIdentifier, title, ticks);
                            ConversationTable[callIdentifier] = conv;
                            conv.Show();
                        }
                        else
                        {
                            if (conv.WindowState == FormWindowState.Minimized)
                                conv.WindowState = FormWindowState.Normal;
                            conv.BringToFront();
                        }
                    }
                }
                else
                {
                    conv = new MetaDataForm(callIdentifier, title, ticks);
                    ConversationTable.Add(callIdentifier, conv);
                    conv.Show();
                }
            }
        }

        private void panel_onPanelCallStateUpdate(object sender, uint callState)
        {
            string state = "";
            string remotePartyDN = "";

            switch(callState)
            {
                case CallState.CONNECTED:
                    state = "Connected";
                    if (sender.Equals(xpPanel1))
                    {
                        if (agent.IsRecording(xpPanel1.ActiveCall.CallData.CallIdentifier))
                            xpPanel1.ImageItems.Normal = 2;
                        else
                            xpPanel1.ImageItems.Normal = 0;
                        remotePartyDN = xpPanel1.ActiveCall.CallData.CallType == CallType.INBOUND_CALL ? xpPanel1.ActiveCall.CallData.CallerDN : xpPanel1.ActiveCall.CallData.CalleeDN;
                        xpPanel1.Invalidate();
                    }
                    else if (sender.Equals(xpPanel2))
                    {
                        if (agent.IsRecording(xpPanel2.ActiveCall.CallData.CallIdentifier))
                            xpPanel2.ImageItems.Normal = 2;
                        else
                            xpPanel2.ImageItems.Normal = 0;
                        remotePartyDN = xpPanel2.ActiveCall.CallData.CallType == CallType.INBOUND_CALL ? xpPanel2.ActiveCall.CallData.CallerDN : xpPanel2.ActiveCall.CallData.CalleeDN;
                        xpPanel2.Invalidate();
                    }
                    else if (sender.Equals(xpPanel3))
                    {
                        if (agent.IsRecording(xpPanel3.ActiveCall.CallData.CallIdentifier))
                            xpPanel3.ImageItems.Normal = 2;
                        else
                            xpPanel3.ImageItems.Normal = 0;
                        remotePartyDN = xpPanel3.ActiveCall.CallData.CallType == CallType.INBOUND_CALL ? xpPanel3.ActiveCall.CallData.CallerDN : xpPanel3.ActiveCall.CallData.CalleeDN;
                        xpPanel3.Invalidate();
                    }
                    if (remotePartyDN.Length > 0 && sforceManager.IsEnabled)
                        sforceManager.DoSearchByPhoneNumber(remotePartyDN);
                    break;

                case CallState.HOLD:
                    state = "Hold";
                    if (sender.Equals(xpPanel1))
                    {
                        xpPanel1.ImageItems.Normal = 1;
                        xpPanel1.Invalidate();
                    }
                    else if (sender.Equals(xpPanel2))
                    {
                        xpPanel2.ImageItems.Normal = 1;
                        xpPanel2.Invalidate();
                    }
                    else if (sender.Equals(xpPanel3))
                    {
                        xpPanel3.ImageItems.Normal = 1;
                        xpPanel3.Invalidate();
                    }
                    break;

                case CallState.PRE_ONHOOK:
                case CallState.ONHOOK:
                    if (sender.Equals(xpPanel1))
                    {
                        this.menuItemCall1.Visible = false;
                        this.menuItem_Start1.Enabled = true;
                        this.menuItem_StartNow1.Enabled = true;
                        this.menuItem_Stop1.Enabled = true;
                        this.menuItem_TakeNote1.Enabled = true;

                        this.linkLabel_StartNow1.Enabled = true;
                        this.linkLabel_Start1.Enabled = true;
                        this.linkLabel_Stop1.Enabled = false;
                    }
                    else if (sender.Equals(xpPanel2))
                    {
                        this.menuItemCall2.Visible = false;
                        this.menuItem_Start2.Enabled = true;
                        this.menuItem_StartNow2.Enabled = true;
                        this.menuItem_Stop2.Enabled = true;
                        this.menuItem_TakeNote2.Enabled = true;

                        this.linkLabel_StartNow2.Enabled = true;
                        this.linkLabel_Start2.Enabled = true;
                        this.linkLabel_Stop2.Enabled = false;
                    }
                    else if (sender.Equals(xpPanel3))
                    {
                        this.menuItemCall3.Visible = false;
                        this.menuItem_Start3.Enabled = true;
                        this.menuItem_StartNow3.Enabled = true;
                        this.menuItem_Stop3.Enabled = true;
                        this.menuItem_TakeNote3.Enabled = true;

                        this.linkLabel_StartNow3.Enabled = true;
                        this.linkLabel_Start3.Enabled = true;
                        this.linkLabel_Stop3.Enabled = false;
                    }

                    if (!this.xpPanel1.IsActiveCall && !this.xpPanel2.IsActiveCall && !this.xpPanel3.IsActiveCall)
                        this.menuItemCallSeperator.Visible = false;

                    break;

                default:
                    return;
            }

            if (state.Length == 0)
                return;

            if (sender.Equals(xpPanel1))
            {
                label_Status1.Text = state;
            }
            else if (sender.Equals(xpPanel2))
            {
                label_Status2.Text = state;
            }
            else if (sender.Equals(xpPanel3))
            {
                label_Status3.Text = state;
            }
        }

        private void panel_onPanelCallTimeUpdate(object sender, string callTime)
        {
            if (sender.Equals(xpPanel1))
            {
                label_Timer1.Text = callTime;
            }
            else if (sender.Equals(xpPanel2))
            {
                label_Timer2.Text = callTime;
            }
            else if (sender.Equals(xpPanel3))
            {
                label_Timer3.Text = callTime;
            }
        }

        private void panel_onPanelCallInfoUpdate(object sender, string callInfo)
        {
            if (sender.Equals(xpPanel1))
            {
                xpPanel1.Caption = callInfo;
                menuItemCall1.Text = callInfo;
            }
            else if (sender.Equals(xpPanel2))
            {
                xpPanel2.Caption = callInfo;
                menuItemCall2.Text = callInfo;
            }
            else if (sender.Equals(xpPanel3))
            {
                xpPanel3.Caption = callInfo;
                menuItemCall3.Text = callInfo;
            }
        }

        private void IpcClient_onServiceUp()
        {
            notifyIconMain.Icon = GetIconFromImageList(imageListTray, 1);
        }

        private void IpcClient_onServiceDown()
        {
            notifyIconMain.Icon = GetIconFromImageList(imageListTray, 0);
        }

        private Icon GetIconFromImageList(ImageList imgList, int position) 
        { 
            return Icon.FromHandle(((Bitmap)imgList.Images[position]).GetHicon()); 
        }

        private void menuItemSalesForce_Click(object sender, System.EventArgs e)
        {
            if (menuItemSalesForce.Checked == true)
            {
                menuItemSalesForce.Checked = false;
                sforceManager.IsEnabled = false;
            }
            else
            {
                SforceLogin sfLogin = new SforceLogin();
                if (sfLogin.ShowDialog().Equals(DialogResult.OK))
                {
                    // login success
                    menuItemSalesForce.Checked = true;
                    sforceManager.IsEnabled = true;
                } 
                else
                {
                    // login failed
                    menuItemSalesForce.Checked = false;
                    sforceManager.IsEnabled = false;
                }      
                sfLogin.Close();
            }
        }
    }
}
