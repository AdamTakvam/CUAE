using System;
using System.Threading;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Metreos.MmsTester.Core;
using Metreos.MmsTester.Interfaces;
using Metreos.MmsTester.Conduit;
using Metreos.Samoa.Core;
using Metreos.MmsTester.VisualInterfaceFramework;
using Metreos.MmsTester.VisualInterfaceManager;
using Metreos.MmsTester.MediaServerFramework;

namespace Metreos.MmsTester.VisualInterfaceManager
{
	/// <summary>
	/// Provides a windows form shell for all visual interfaces loaded at runtime
	/// </summary>
	public class VisualInterfaceShell : System.Windows.Forms.Form
	{
        // User variables
        private Thread mainTestThread;
        private AutoResetEvent are;
        private Conduit.Conduit conduit;
        private IConduit.VisualDelegate startupComplete = new IConduit.VisualDelegate( StartUpComplete );

        // Registered Events

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.StatusBar statusBar1;
        private System.Windows.Forms.Panel settingsPanel;
        private System.Windows.Forms.Label settingsLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox mediaServer0_networkName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox mediaServer0_queueName;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel connectionPanel;
        private System.Windows.Forms.Panel commandPanel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage simpleCommandsTab;
        private System.Windows.Forms.Panel simpleCommands_mediaServer0_panel;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.TabControl visualizations;
        private System.Windows.Forms.TabPage experimentalSimpleCommands;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.ListBox listBox3;
        private System.Data.SqlClient.SqlCommand sqlCommand1;
        private System.Windows.Forms.Button button16;

        private VisualInterfaceProvider provider;
		public VisualInterfaceShell(VisualInterfaceProvider provider, Conduit.Conduit conduit, ref AutoResetEvent are)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.provider = provider;
            this.conduit = conduit;
            this.are = are;
         
            mainTestThread = new Thread(new ThreadStart( TestMain ));
		}

        void TestMain()
        {
        }
        // REFACTOR:  read config file to determine which visual interfaces to load
        public bool StartVisualInterfaces()
        {
            return false;
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

            are.Set();
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.mediaServer0_queueName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.mediaServer0_networkName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.settingsLabel = new System.Windows.Forms.Label();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.commandPanel = new System.Windows.Forms.Panel();
            this.visualizations = new System.Windows.Forms.TabControl();
            this.simpleCommandsTab = new System.Windows.Forms.TabPage();
            this.button16 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.simpleCommands_mediaServer0_panel = new System.Windows.Forms.Panel();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.button15 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.experimentalSimpleCommands = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.connectionPanel = new System.Windows.Forms.Panel();
            this.button14 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.sqlCommand1 = new System.Data.SqlClient.SqlCommand();
            this.settingsPanel.SuspendLayout();
            this.controlPanel.SuspendLayout();
            this.commandPanel.SuspendLayout();
            this.visualizations.SuspendLayout();
            this.simpleCommandsTab.SuspendLayout();
            this.panel1.SuspendLayout();
            this.simpleCommands_mediaServer0_panel.SuspendLayout();
            this.connectionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                      this.menuItem1,
                                                                                      this.menuItem2,
                                                                                      this.menuItem3});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                      this.menuItem4,
                                                                                      this.menuItem5,
                                                                                      this.menuItem6,
                                                                                      this.menuItem7});
            this.menuItem1.Text = "File";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 0;
            this.menuItem4.Text = "Start Test";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 1;
            this.menuItem5.Text = "Stop Test";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 2;
            this.menuItem6.Text = "Open Log";
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 3;
            this.menuItem7.Text = "Exit";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "Settings";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.Text = "Help";
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 681);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(972, 22);
            this.statusBar1.TabIndex = 0;
            this.statusBar1.Text = "statusBar1";
            // 
            // settingsPanel
            // 
            this.settingsPanel.BackColor = System.Drawing.Color.IndianRed;
            this.settingsPanel.Controls.Add(this.mediaServer0_queueName);
            this.settingsPanel.Controls.Add(this.label3);
            this.settingsPanel.Controls.Add(this.label2);
            this.settingsPanel.Controls.Add(this.mediaServer0_networkName);
            this.settingsPanel.Controls.Add(this.label1);
            this.settingsPanel.Controls.Add(this.settingsLabel);
            this.settingsPanel.Location = new System.Drawing.Point(8, 0);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(952, 688);
            this.settingsPanel.TabIndex = 1;
            // 
            // mediaServer0_queueName
            // 
            this.mediaServer0_queueName.Location = new System.Drawing.Point(136, 136);
            this.mediaServer0_queueName.Name = "mediaServer0_queueName";
            this.mediaServer0_queueName.Size = new System.Drawing.Size(104, 20);
            this.mediaServer0_queueName.TabIndex = 5;
            this.mediaServer0_queueName.Text = "metreos-mediaserver";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(136, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Queue Name";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(32, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Network Name";
            // 
            // mediaServer0_networkName
            // 
            this.mediaServer0_networkName.Location = new System.Drawing.Point(32, 136);
            this.mediaServer0_networkName.Name = "mediaServer0_networkName";
            this.mediaServer0_networkName.Size = new System.Drawing.Size(72, 20);
            this.mediaServer0_networkName.TabIndex = 2;
            this.mediaServer0_networkName.Text = "BART";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "MediaServer One";
            // 
            // settingsLabel
            // 
            this.settingsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.settingsLabel.Location = new System.Drawing.Point(8, 8);
            this.settingsLabel.Name = "settingsLabel";
            this.settingsLabel.Size = new System.Drawing.Size(88, 32);
            this.settingsLabel.TabIndex = 0;
            this.settingsLabel.Text = "Settings";
            // 
            // controlPanel
            // 
            this.controlPanel.BackColor = System.Drawing.Color.Lavender;
            this.controlPanel.Controls.Add(this.label4);
            this.controlPanel.Controls.Add(this.button2);
            this.controlPanel.Controls.Add(this.button1);
            this.controlPanel.Controls.Add(this.commandPanel);
            this.controlPanel.Controls.Add(this.connectionPanel);
            this.controlPanel.Location = new System.Drawing.Point(8, 0);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(952, 680);
            this.controlPanel.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(320, 40);
            this.label4.TabIndex = 2;
            this.label4.Text = "Metreos Media Server Test Tool";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(136, 88);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 32);
            this.button2.TabIndex = 1;
            this.button2.Text = "Commands";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(24, 88);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 32);
            this.button1.TabIndex = 0;
            this.button1.Text = "Connect";
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // commandPanel
            // 
            this.commandPanel.BackColor = System.Drawing.Color.CornflowerBlue;
            this.commandPanel.Controls.Add(this.visualizations);
            this.commandPanel.Controls.Add(this.label6);
            this.commandPanel.Location = new System.Drawing.Point(8, 152);
            this.commandPanel.Name = "commandPanel";
            this.commandPanel.Size = new System.Drawing.Size(936, 520);
            this.commandPanel.TabIndex = 4;
            this.commandPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.commandPanel_Paint);
            // 
            // visualizations
            // 
            this.visualizations.Controls.Add(this.simpleCommandsTab);
            this.visualizations.Controls.Add(this.experimentalSimpleCommands);
            this.visualizations.Location = new System.Drawing.Point(8, 56);
            this.visualizations.Name = "visualizations";
            this.visualizations.SelectedIndex = 0;
            this.visualizations.Size = new System.Drawing.Size(920, 456);
            this.visualizations.TabIndex = 1;
            // 
            // simpleCommandsTab
            // 
            this.simpleCommandsTab.Controls.Add(this.button16);
            this.simpleCommandsTab.Controls.Add(this.panel1);
            this.simpleCommandsTab.Controls.Add(this.button8);
            this.simpleCommandsTab.Location = new System.Drawing.Point(4, 22);
            this.simpleCommandsTab.Name = "simpleCommandsTab";
            this.simpleCommandsTab.Size = new System.Drawing.Size(912, 430);
            this.simpleCommandsTab.TabIndex = 0;
            this.simpleCommandsTab.Text = "Simple Commands";
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(240, 8);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(80, 32);
            this.button16.TabIndex = 4;
            this.button16.Text = "Refresh";
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Thistle;
            this.panel1.Controls.Add(this.simpleCommands_mediaServer0_panel);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Location = new System.Drawing.Point(8, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(896, 376);
            this.panel1.TabIndex = 3;
            // 
            // simpleCommands_mediaServer0_panel
            // 
            this.simpleCommands_mediaServer0_panel.AutoScroll = true;
            this.simpleCommands_mediaServer0_panel.BackColor = System.Drawing.Color.Plum;
            this.simpleCommands_mediaServer0_panel.Controls.Add(this.listBox3);
            this.simpleCommands_mediaServer0_panel.Controls.Add(this.button15);
            this.simpleCommands_mediaServer0_panel.Controls.Add(this.listView1);
            this.simpleCommands_mediaServer0_panel.Controls.Add(this.listBox2);
            this.simpleCommands_mediaServer0_panel.Controls.Add(this.button7);
            this.simpleCommands_mediaServer0_panel.Controls.Add(this.button6);
            this.simpleCommands_mediaServer0_panel.Controls.Add(this.button5);
            this.simpleCommands_mediaServer0_panel.Controls.Add(this.button4);
            this.simpleCommands_mediaServer0_panel.Location = new System.Drawing.Point(8, 24);
            this.simpleCommands_mediaServer0_panel.Name = "simpleCommands_mediaServer0_panel";
            this.simpleCommands_mediaServer0_panel.Size = new System.Drawing.Size(880, 344);
            this.simpleCommands_mediaServer0_panel.TabIndex = 0;
            // 
            // listBox3
            // 
            this.listBox3.Location = new System.Drawing.Point(200, 8);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(40, 316);
            this.listBox3.TabIndex = 8;
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(56, 48);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(80, 32);
            this.button15.TabIndex = 7;
            this.button15.Text = "Full-Connect";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                        this.columnHeader1,
                                                                                        this.columnHeader2,
                                                                                        this.columnHeader3,
                                                                                        this.columnHeader4,
                                                                                        this.columnHeader5,
                                                                                        this.columnHeader6});
            this.listView1.Location = new System.Drawing.Point(248, 8);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(624, 320);
            this.listView1.TabIndex = 6;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "#";
            this.columnHeader1.Width = 22;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Connected";
            this.columnHeader2.Width = 66;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "In Conference";
            this.columnHeader3.Width = 84;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Played to";
            this.columnHeader4.Width = 56;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Recording";
            this.columnHeader5.Width = 72;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Muted";
            this.columnHeader6.Width = 46;
            // 
            // listBox2
            // 
            this.listBox2.Items.AddRange(new object[] {
                                                          "1",
                                                          "2",
                                                          "3",
                                                          "4",
                                                          "5",
                                                          "6",
                                                          "7",
                                                          "8",
                                                          "9",
                                                          "10",
                                                          "11",
                                                          "12",
                                                          "13",
                                                          "14",
                                                          "15",
                                                          "16",
                                                          "17",
                                                          "18",
                                                          "19",
                                                          "20",
                                                          "21",
                                                          "22",
                                                          "23",
                                                          "24",
                                                          "25",
                                                          "26",
                                                          "27",
                                                          "28",
                                                          "29",
                                                          "30",
                                                          "31",
                                                          "32"});
            this.listBox2.Location = new System.Drawing.Point(8, 8);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(40, 316);
            this.listBox2.TabIndex = 5;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(56, 168);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(72, 32);
            this.button7.TabIndex = 4;
            this.button7.Text = "Disconnect";
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(56, 128);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(44, 32);
            this.button6.TabIndex = 3;
            this.button6.Text = "Play";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(56, 88);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(140, 32);
            this.button5.TabIndex = 2;
            this.button5.Text = "Connect to Conference";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(56, 8);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(80, 32);
            this.button4.TabIndex = 1;
            this.button4.Text = "Half-Connect";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(8, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(104, 24);
            this.button3.TabIndex = 1;
            this.button3.Text = "Media Server One";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(8, 8);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(128, 32);
            this.button8.TabIndex = 2;
            this.button8.Text = "Application Server Emulator 1";
            // 
            // experimentalSimpleCommands
            // 
            this.experimentalSimpleCommands.Location = new System.Drawing.Point(4, 22);
            this.experimentalSimpleCommands.Name = "experimentalSimpleCommands";
            this.experimentalSimpleCommands.Size = new System.Drawing.Size(912, 430);
            this.experimentalSimpleCommands.TabIndex = 1;
            this.experimentalSimpleCommands.Text = "Experimental Simple Commands";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label6.Location = new System.Drawing.Point(8, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(136, 24);
            this.label6.TabIndex = 0;
            this.label6.Text = "Control Panel";
            // 
            // connectionPanel
            // 
            this.connectionPanel.BackColor = System.Drawing.Color.PaleTurquoise;
            this.connectionPanel.Controls.Add(this.button14);
            this.connectionPanel.Controls.Add(this.button13);
            this.connectionPanel.Controls.Add(this.button12);
            this.connectionPanel.Controls.Add(this.button11);
            this.connectionPanel.Controls.Add(this.button10);
            this.connectionPanel.Controls.Add(this.button9);
            this.connectionPanel.Controls.Add(this.label13);
            this.connectionPanel.Controls.Add(this.label12);
            this.connectionPanel.Controls.Add(this.label11);
            this.connectionPanel.Controls.Add(this.listBox1);
            this.connectionPanel.Controls.Add(this.textBox2);
            this.connectionPanel.Controls.Add(this.textBox1);
            this.connectionPanel.Controls.Add(this.label10);
            this.connectionPanel.Controls.Add(this.label9);
            this.connectionPanel.Controls.Add(this.label8);
            this.connectionPanel.Controls.Add(this.label5);
            this.connectionPanel.Location = new System.Drawing.Point(8, 160);
            this.connectionPanel.Name = "connectionPanel";
            this.connectionPanel.Size = new System.Drawing.Size(936, 512);
            this.connectionPanel.TabIndex = 3;
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(200, 216);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(24, 24);
            this.button14.TabIndex = 16;
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(200, 176);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(24, 24);
            this.button13.TabIndex = 15;
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(200, 136);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(24, 24);
            this.button12.TabIndex = 14;
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(200, 96);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(24, 24);
            this.button11.TabIndex = 13;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(200, 56);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(24, 24);
            this.button10.TabIndex = 12;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(168, 8);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(80, 24);
            this.button9.TabIndex = 11;
            this.button9.Text = "Initialize test";
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(64, 72);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(64, 16);
            this.label13.TabIndex = 10;
            this.label13.Text = "Amount";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(128, 176);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(64, 16);
            this.label12.TabIndex = 9;
            this.label12.Text = "Amount";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(128, 200);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 32);
            this.label11.TabIndex = 8;
            this.label11.Text = "Adapter Type";
            // 
            // listBox1
            // 
            this.listBox1.Items.AddRange(new object[] {
                                                          "MmsMqAdapter"});
            this.listBox1.Location = new System.Drawing.Point(8, 200);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(112, 56);
            this.listBox1.TabIndex = 7;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(8, 176);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(112, 20);
            this.textBox2.TabIndex = 6;
            this.textBox2.Text = "1";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(8, 72);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(48, 20);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "1";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(8, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 24);
            this.label10.TabIndex = 4;
            this.label10.Text = "Media Servers";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(8, 144);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(112, 32);
            this.label9.TabIndex = 3;
            this.label9.Text = "Application Server Emulator";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 120);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 16);
            this.label8.TabIndex = 2;
            this.label8.Text = "Clients";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label5.Location = new System.Drawing.Point(8, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(144, 32);
            this.label5.TabIndex = 1;
            this.label5.Text = "Test Initialization";
            // 
            // VisualInterfaceShell
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(972, 703);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.settingsPanel);
            this.Menu = this.mainMenu1;
            this.Name = "VisualInterfaceShell";
            this.Text = "VisualInterfaceManager";
            this.Load += new System.EventHandler(this.VisualInterfaceShell_Load);
            this.settingsPanel.ResumeLayout(false);
            this.controlPanel.ResumeLayout(false);
            this.commandPanel.ResumeLayout(false);
            this.visualizations.ResumeLayout(false);
            this.simpleCommandsTab.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.simpleCommands_mediaServer0_panel.ResumeLayout(false);
            this.connectionPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

        private void button1_Click(object sender, System.EventArgs e)
        {
        
        }

        private void VisualInterfaceShell_Load(object sender, System.EventArgs e)
        {
        
        }

        private void dataGrid1_Navigate(object sender, System.Windows.Forms.NavigateEventArgs ne)
        {
        
        }

        private void label7_Click(object sender, System.EventArgs e)
        {
        
        }

        private void button9_Click(object sender, System.EventArgs e)
        {
            InternalMessage im = new InternalMessage();

            
            im.AddField(IMessaging.ADAPTER_DISPLAY_NAME, IAdapterTypes.MMS_MQ_ADAPTER_DISPLAY_NAME);
            im.AddField(IMessaging.MEDIA_SERVER_HANDLE, "0");
            im.AddField(IMessaging.ADAPTER_GUID, "0");
            im.AddField(IMessaging.CLIENT_DISPLAY_NAME, IClientTypes.AS_EMULATOR_DISPLAY_NAME);
            im.AddField(IMessaging.CLIENT_GUID, "0");
            im.AddField(IMessaging.TEST_MACHINE_NAME, IMessaging.MACHINE_NAME);
            im.AddField(IMessaging.TEST_QUEUE_NAME, IMessaging.TEST_TOOL_NAME + "_client_0");
            im.AddField(MmsProtocol.FIELD_MS_TRANSACTION_ID, TransactionIdFactory.GetTransactionId().ToString());

            // Create the adapter
            im.MessageId = IMessaging.INITIALIZE_ADAPTER;

            if(!conduit.SendSystemCommand(im, startupComplete))          
            {
                button10.BackColor = Color.Red;
                return;
            }
               
            button10.BackColor = Color.Green;

            // Start the adapter
            im.MessageId = IMessaging.START_ADAPTER;
            im.RemoveField(MmsProtocol.FIELD_MS_TRANSACTION_ID);
            im.AddField(MmsProtocol.FIELD_MS_TRANSACTION_ID, TransactionIdFactory.GetTransactionId().ToString());

            if(!conduit.SendSystemCommand(im, startupComplete))          
            {
                button11.BackColor = Color.Red;
                return;
            }

            button11.BackColor = Color.Green;

            // Start client
            im.MessageId = IMessaging.START_CLIENT;
            im.RemoveField(MmsProtocol.FIELD_MS_TRANSACTION_ID);
            im.AddField(MmsProtocol.FIELD_MS_TRANSACTION_ID, TransactionIdFactory.GetTransactionId().ToString());
        
            if(!conduit.SendSystemCommand(im, startupComplete))          
            {
                button12.BackColor = Color.Red;
                return;
            }

            button12.BackColor = Color.Green;

            // Give the client the adapter
            im.MessageId = IMessaging.LINK_ADAPTER_TO_CLIENT;
            im.RemoveField(MmsProtocol.FIELD_MS_TRANSACTION_ID);
            im.AddField(MmsProtocol.FIELD_MS_TRANSACTION_ID, TransactionIdFactory.GetTransactionId().ToString());

            if(!conduit.SendSystemCommand(im, startupComplete))          
            {
                button13.BackColor = Color.Red;
                return;
            }

            button13.BackColor = Color.Green;

            // Connect to the media server
            im.MessageId = IMessaging.CONNECT_TO_MEDIASERVER;
            im.RemoveField(MmsProtocol.FIELD_MS_TRANSACTION_ID);
            im.AddField(MmsProtocol.FIELD_MS_TRANSACTION_ID, TransactionIdFactory.GetTransactionId().ToString());

            if(!conduit.SendMediaServerCommand(im, startupComplete))
            {
                button14.BackColor = Color.Red;
                return;
            }

            button14.BackColor = Color.Green;

        }

        #region Registered Events

        public static bool StartUpComplete(InternalMessage im)
        {
            if(im.MessageId == IMessaging.SUCCESS)
            {
                return true;
            }
            if(im.MessageId == IMessaging.FAILURE)
            {
                return false;
            }

            return false;
        }

        #endregion Registered Events

        private void commandPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, System.EventArgs e)
        {
            connectionPanel.Visible = true;
            commandPanel.Visible = false;
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            connectionPanel.Visible = false;
            commandPanel.Visible = true;
        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            InternalMessage im = new InternalMessage();

            im.MessageId = MmsProtocol.MSG_MS_CONNECT;

            im.AddField(MmsProtocol.FIELD_MS_PORT, "0");
            im.AddField(MmsProtocol.FIELD_MS_IP_ADDRESS, "0");
            im.AddField(MmsProtocol.FIELD_MS_SESSION_TIMEOUT, ResourcePool.SESSION_TIMEOUT.ToString());
            im.AddField(MmsProtocol.FIELD_MS_COMMAND_TIMEOUT, ResourcePool.COMMAND_TIMEOUT.ToString());

            // Client information
            im.AddField(IMessaging.CLIENT_DISPLAY_NAME, IClientTypes.AS_EMULATOR_DISPLAY_NAME);
            im.AddField(IMessaging.CLIENT_GUID, "0");

            im.AddField(IMessaging.MEDIA_SERVER_HANDLE, "0");

            im.AddField(MmsProtocol.FIELD_MS_TRANSACTION_ID, TransactionIdFactory.GetTransactionId().ToString());

            if(!conduit.SendMediaServerCommand(im, startupComplete))          
            {
                button4.BackColor = Color.Red;
                return;
            }
               
            button4.BackColor = Color.Green;
        }

        private void button7_Click(object sender, System.EventArgs e)
        {
            InternalMessage im = new InternalMessage();

            im.MessageId = MmsProtocol.MSG_MS_DISCONNECT;

            im.AddField(MmsProtocol.FIELD_MS_CONNECTION_ID, "1");

            // Client information
            im.AddField(IMessaging.CLIENT_DISPLAY_NAME, IClientTypes.AS_EMULATOR_DISPLAY_NAME);
            im.AddField(IMessaging.CLIENT_GUID, "0");

            im.AddField(IMessaging.MEDIA_SERVER_HANDLE, "0");

            im.AddField(MmsProtocol.FIELD_MS_TRANSACTION_ID, TransactionIdFactory.GetTransactionId().ToString());

            if(!conduit.SendMediaServerCommand(im, startupComplete))          
            {
                button7.BackColor = Color.Red;
                return;
            }
               
            button7.BackColor = Color.Green;
        }

        private void button16_Click(object sender, System.EventArgs e)
        {
            InternalMessage im  = new InternalMessage();

            im.MessageId = IMessaging.REQUEST_MEDIA_SERVER_INFO;
            
            // Client information
            im.AddField(IMessaging.CLIENT_DISPLAY_NAME, IClientTypes.AS_EMULATOR_DISPLAY_NAME);
            im.AddField(IMessaging.CLIENT_GUID, "0");

            im.AddField(IMessaging.MEDIA_SERVER_HANDLE, "0");

            conduit.RequestMediaServerTable(im, new IConduit.VisualDelegate( FullRefresh ));
            
        }

        public bool FullRefresh(InternalMessage im)
        {
            RefreshFromFullMediaServerInfo(im);

            return true;
        }

        private void listView1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        
        }

        private void RefreshFromFullMediaServerInfo(InternalMessage im)
        {
            string numConnections;
            im.GetFieldByName(IMessaging.NUM_CONNECTIONS, out numConnections);

            System.Windows.Forms.ListViewItem[] items = new System.Windows.Forms.ListViewItem[Int32.Parse(numConnections)];

            for(int i = 0; i < Int32.Parse(numConnections); i++)
            {
                string stringToParse;
                
                im.GetFieldByName(IMessaging.CONNECTION_NUM + i, out stringToParse);
                string[] results = stringToParse.Split(';');

                if (results.Length == 5)
                {
                    items[i] = new System.Windows.Forms.ListViewItem(new string[] {
                                                                                      i.ToString(),
                                                                                      results[0],
                                                                                      results[1],
                                                                                      results[2],
                                                                                      results[3],
                                                                                      results[4]}, -1);
                }
            }

            listView1.Items.Clear();

            this.listView1.Items.AddRange(items);
        }
	}
}
