using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;


using Metreos.Core;
using WebMessage = Metreos.Common.Mec;

namespace Metreos.Mec.StressApp
{
    /// <summary>
    /// A console application to pound on the MEC
    /// </summary>
    public class MecStressTest : Form, IMecTester
    {
        StressTesting.Settings settings;
        public RandomizedConference2 test;

        private Regex numbersOnly = new Regex("^[0-9]+$");
        private Regex lettersOnly = new Regex("^[^0-9]+$");

		public IStressTesting.CreateConferenceDelegate createCallback;
		public IStressTesting.EndConferenceDelegate endCallback;
		public IStressTesting.JoinLocationDelegate joinCallback;
		public IStressTesting.KickLocationDelegate kickCallback;
		public IStressTesting.MuteLocationDelegate muteCallback;
        public IStressTesting.TerminateAllDelegate terminateAllCallback;

        public TestBase currentTest;
        public AutoResetEvent testIsDone = new AutoResetEvent(false);
        public DateTime testStartedAtThisTime;
        public const int MAXIMUM_INTENSITY = 21;
        public bool startRandomizedConference;
        public int randomizedConference_Selected;
        public Thread mainTestThread;
        public System.Timers.Timer clockThread;
        public MessageHandler debugMessageHandler;
        public MessageHandler errorMessageHandler;
        public Hashtable locationGuidTable;
        public Hashtable LocationGuidTable;
        public ArrayList orderItemsTable;
        public ArrayList OrderItemsTable;
        public object listViewLock;

        public int pendingConferences;
        public int liveConferences;
        public int totalConferences;
        public int failedConferences;
        public int pendingConnections;
        public int liveConnections;
        public int totalConnections;
        public int failedConnections;
        public int pendingKicks;
        public int totalKicks;
        public int failedKicks;

        public object pendingConferencesLock;
        public object liveConferencesLock;
        public object totalConferencesLock;
        public object failedConferencesLock;
        public object pendingConnectionsLock;
        public object liveConnectionsLock;
        public object totalConnectionsLock;
        public object failedConnectionsLock;
        public object pendingKicksLock;
        public object totalKicksLock;
        public object failedKicksLock;
        public object verboseInfoLock;
        public object errorInfoLock;

        public int PendingConferences
        {
            set
            {
                lock(pendingConferencesLock)
                {
                    pendingConferences = value;

                    if(pendingConferences < 0)
                    {
                        pendingConferences = 0;
                    }

                    label13.Text = value.ToString();

                }
            }

            get
            {
                lock(pendingConferencesLock)
                {
                    return pendingConferences;
                }
            }
        }

        public int LiveConferences
        {
            set
            {
                lock(liveConferencesLock)
                {
                    liveConferences = value;

                    if(liveConferences < 0)
                    {
                        liveConferences = 0;
                    }

                    label12.Text = value.ToString();
                }
            }

            get
            {
                lock(liveConferencesLock)
                {
                    return liveConferences;
                }
            }
        }

        public int TotalConferences
        {
            set
            {
                lock(totalConferencesLock)
                {
                    totalConferences = value;

                    if(totalConferences < 0)
                    {
                        totalConferences = 0;
                    }

                    label11.Text = value.ToString();
                }
            }

            get
            {
                lock(totalConferencesLock)
                {
                    return totalConferences;
                }
            }
        }
    
        public int FailedConferences
        {
            set
            {
                lock(failedConferencesLock)
                {
                    failedConferences = value;

                    if(failedConferences < 0)
                    {
                        failedConferences = 0;
                    }

                    label8.Text = value.ToString();
                }
            }

            get
            {
                lock(failedConferencesLock)
                {
                    return failedConferences;
                }
            }
        }
    
        public int PendingConnections
        {
            set
            {
                lock(pendingConnectionsLock)
                {
                    pendingConnections = value;

                    if(pendingConnections < 0)
                    {
                        pendingConnections = 0;
                    }

                    label10.Text = value.ToString();
                }
            }

            get
            {
                lock(pendingConnectionsLock)
                {
                    return pendingConnections;
                }
            }
        }

        public int LiveConnections
        {
            set
            {
                lock(liveConnectionsLock)
                {
                    liveConnections = value;

                    if(liveConnections < 0)
                    {
                        liveConnections = 0;
                    }

                    label2.Text = value.ToString();
                }
            }

            get
            {
                lock(liveConnectionsLock)
                {
                    return liveConnections;
                }
            }
        }
    
        public int TotalConnections
        {
            set
            {
                lock(totalConnectionsLock)
                {
                    totalConnections = value;

                    if(totalConnections < 0)
                    {
                        totalConnections = 0;
                    }

                    label3.Text = value.ToString();
                }
            }

            get
            {
                lock(totalConnectionsLock)
                {
                    return totalConnections;
                }
            }
        }

        public int FailedConnections
        {
            set
            {
                lock(failedConnectionsLock)
                {
                    failedConnections = value;

                    if(failedConnections < 0)
                    {
                        failedConnections = 0;
                    }
                    label16.Text = value.ToString();
                }
            }

            get
            {
                lock(failedConnectionsLock)
                {
                    return failedConnections;
                }
            }
        }

        public int PendingKicks
        {
            set
            {
                lock(pendingKicksLock)
                {
                    pendingKicks = value;

                    if(pendingKicks < 0)
                    {
                        pendingKicks = 0;
                    }

                    label23.Text = value.ToString();
                }
            }

            get
            {
                lock(pendingKicksLock)
                {
                    return pendingKicks;
                }
            }
        }

        public int TotalKicks
        {
            set
            {
                lock(totalKicksLock)
                {
                    totalKicks = value;

                    if(totalKicks < 0)
                    {
                        totalKicks = 0;
                    }

                    label17.Text = value.ToString();
                }
            }

            get
            {
                lock(totalKicksLock)
                {
                    return totalKicks;
                }
            }
        }

        public int FailedKicks
        {
            set
            {
                lock(failedKicksLock)
                {
                    failedKicks = value;

                    if(failedKicks < 0)
                    {
                        failedKicks = 0;
                    }

                    label14.Text = value.ToString();
                }
            }

            get
            {
                lock(failedKicksLock)
                {
                    return failedKicks;
                }
            }
        }

        public string VerboseInfo
        {
            set
            {
                lock(verboseInfoLock)
                {
                    richTextBox1.AppendText(value + "\n");
                    richTextBox1.ScrollToCaret();
                }
            }
        }

        public string ErrorInfo
        {
            set
            {
                lock(errorInfoLock)
                {
                    richTextBox2.AppendText(DateTime.Now.ToString() + ":  " + value + "\n");
                    richTextBox2.ScrollToCaret();
                }
            }
        }

        public System.Windows.Forms.MenuItem menuItem1;
        public System.Windows.Forms.MenuItem menuItem2;
        public System.Windows.Forms.MenuItem menuItem3;
        public System.Windows.Forms.MenuItem menuItem4;
        public System.Windows.Forms.MenuItem menuItem5;
        public System.Windows.Forms.MenuItem menuItem6;
        public System.Windows.Forms.MenuItem menuItem7;
        public System.Windows.Forms.MainMenu mainMenu1;
        public System.Windows.Forms.StatusBar statusBar1;
        public System.Windows.Forms.LinkLabel metreosLink;
        public System.Windows.Forms.MenuItem menuItem8;
        public System.Windows.Forms.MenuItem menuItem9;
        public System.Windows.Forms.MenuItem menuItem10;
        public System.Windows.Forms.Button executeButton;
        public System.Windows.Forms.MenuItem menuItem11;
        public System.Windows.Forms.Panel testOutput_Dashboard_Panel;
        private System.Diagnostics.PerformanceCounter performanceCounter1;
        public System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.MenuItem menuItem13;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader conferenceId;
        private System.Windows.Forms.ColumnHeader locationId;
        private System.Windows.Forms.ColumnHeader status;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ColumnHeader phoneNumber;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        public System.Windows.Forms.Button button6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label timeLabel;
        public System.ComponentModel.IContainer components;

        public MecStressTest()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            settings = new StressTesting.Settings();
            randomizedConference_Selected = 1;
            locationGuidTable = new Hashtable();
            LocationGuidTable = new Hashtable();
            orderItemsTable = new ArrayList();
            OrderItemsTable = new ArrayList();
            listViewLock = new object();
            mainTestThread = new Thread(new ThreadStart(this.RunTestThread));
            clockThread = new System.Timers.Timer(1000);
            clockThread.Elapsed += new System.Timers.ElapsedEventHandler(UpdateClock);
            
            pendingConferences = 0;
            liveConferences = 0;
            totalConferences = 0;
            failedConferences = 0;
            pendingConnections = 0;
            liveConnections = 0;
            totalConnections = 0;
            failedConnections = 0;
            pendingKicks = 0;
            totalKicks = 0;
            failedKicks = 0;

            pendingConferencesLock = new object();
            liveConferencesLock = new object();
            totalConferencesLock = new object();
            failedConferencesLock = new object();
            pendingConnectionsLock = new object();
            liveConnectionsLock = new object();
            totalConnectionsLock = new object();
            failedConnectionsLock = new object();
            pendingKicksLock = new object();
            totalKicksLock = new object();
            failedKicksLock = new object();
            verboseInfoLock = new object();
            errorInfoLock = new object();

            clockThread.Interval = 1000;
            test = new RandomizedConference2((IMecTester)this, settings);
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

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        public void InitializeComponent()
        {
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.metreosLink = new System.Windows.Forms.LinkLabel();
            this.testOutput_Dashboard_Panel = new System.Windows.Forms.Panel();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.executeButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.conferenceId = new System.Windows.Forms.ColumnHeader();
            this.locationId = new System.Windows.Forms.ColumnHeader();
            this.status = new System.Windows.Forms.ColumnHeader();
            this.phoneNumber = new System.Windows.Forms.ColumnHeader();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.performanceCounter1 = new System.Diagnostics.PerformanceCounter();
            this.label25 = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
            this.testOutput_Dashboard_Panel.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounter1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                      this.menuItem2,
                                                                                      this.menuItem11,
                                                                                      this.menuItem3});
            this.menuItem1.Text = "Commands";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "Start";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 1;
            this.menuItem11.Text = "Stop";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.Text = "Exit";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                      this.menuItem5,
                                                                                      this.menuItem7,
                                                                                      this.menuItem6});
            this.menuItem4.Text = "Help";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 0;
            this.menuItem5.Text = "User Manual";
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 1;
            this.menuItem7.Text = "Metreos";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 2;
            this.menuItem6.Text = "Version";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                      this.menuItem1,
                                                                                      this.menuItem8,
                                                                                      this.menuItem4});
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 1;
            this.menuItem8.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                      this.menuItem9,
                                                                                      this.menuItem10,
                                                                                      this.menuItem13});
            this.menuItem8.Text = "Settings";
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 0;
            this.menuItem9.Text = "Global";
            this.menuItem9.Click += new System.EventHandler(this.menuItem9_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 1;
            this.menuItem10.Text = "SimClient";
            this.menuItem10.Click += new System.EventHandler(this.menuItem10_Click);
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 2;
            this.menuItem13.Text = "Test Settings";
            this.menuItem13.Click += new System.EventHandler(this.menuItem13_Click);
            // 
            // statusBar1
            // 
            this.statusBar1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.statusBar1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusBar1.Location = new System.Drawing.Point(0, 568);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(640, 24);
            this.statusBar1.TabIndex = 8;
            this.statusBar1.Text = "Initialized";
            // 
            // metreosLink
            // 
            this.metreosLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.metreosLink.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.metreosLink.Location = new System.Drawing.Point(480, 568);
            this.metreosLink.Name = "metreosLink";
            this.metreosLink.Size = new System.Drawing.Size(144, 24);
            this.metreosLink.TabIndex = 13;
            this.metreosLink.TabStop = true;
            this.metreosLink.Text = "Metreos Communications";
            this.metreosLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // testOutput_Dashboard_Panel
            // 
            this.testOutput_Dashboard_Panel.Controls.Add(this.groupBox7);
            this.testOutput_Dashboard_Panel.Controls.Add(this.groupBox6);
            this.testOutput_Dashboard_Panel.Controls.Add(this.groupBox5);
            this.testOutput_Dashboard_Panel.Controls.Add(this.groupBox2);
            this.testOutput_Dashboard_Panel.Controls.Add(this.groupBox1);
            this.testOutput_Dashboard_Panel.Controls.Add(this.groupBox4);
            this.testOutput_Dashboard_Panel.Controls.Add(this.groupBox3);
            this.testOutput_Dashboard_Panel.Location = new System.Drawing.Point(0, 0);
            this.testOutput_Dashboard_Panel.Name = "testOutput_Dashboard_Panel";
            this.testOutput_Dashboard_Panel.Size = new System.Drawing.Size(640, 576);
            this.testOutput_Dashboard_Panel.TabIndex = 18;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.executeButton);
            this.groupBox7.Controls.Add(this.stopButton);
            this.groupBox7.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox7.Location = new System.Drawing.Point(360, 8);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(128, 56);
            this.groupBox7.TabIndex = 62;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Auto Test Control";
            // 
            // executeButton
            // 
            this.executeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.executeButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.executeButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.executeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.executeButton.Location = new System.Drawing.Point(16, 21);
            this.executeButton.Name = "executeButton";
            this.executeButton.Size = new System.Drawing.Size(40, 24);
            this.executeButton.TabIndex = 25;
            this.executeButton.Text = "Start";
            this.executeButton.Click += new System.EventHandler(this.executeButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.stopButton.Location = new System.Drawing.Point(72, 21);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(40, 24);
            this.stopButton.TabIndex = 25;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label15);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.richTextBox2);
            this.groupBox6.Controls.Add(this.richTextBox1);
            this.groupBox6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox6.Location = new System.Drawing.Point(360, 296);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(272, 264);
            this.groupBox6.TabIndex = 61;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Logging";
            // 
            // label15
            // 
            this.label15.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label15.Location = new System.Drawing.Point(232, 240);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(32, 16);
            this.label15.TabIndex = 3;
            this.label15.Text = "Error";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(216, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Verbose";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // richTextBox2
            // 
            this.richTextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.richTextBox2.HideSelection = false;
            this.richTextBox2.Location = new System.Drawing.Point(8, 136);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(256, 120);
            this.richTextBox2.TabIndex = 1;
            this.richTextBox2.Text = "";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.richTextBox1.HideSelection = false;
            this.richTextBox1.Location = new System.Drawing.Point(8, 16);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(256, 112);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.listView1);
            this.groupBox5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox5.Location = new System.Drawing.Point(8, 8);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(336, 552);
            this.groupBox5.TabIndex = 60;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Conference Detail View";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                        this.conferenceId,
                                                                                        this.locationId,
                                                                                        this.status,
                                                                                        this.phoneNumber});
            this.listView1.Location = new System.Drawing.Point(8, 16);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(320, 528);
            this.listView1.TabIndex = 47;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // conferenceId
            // 
            this.conferenceId.Text = "Conference Id";
            this.conferenceId.Width = 84;
            // 
            // locationId
            // 
            this.locationId.Text = "Location Id";
            this.locationId.Width = 72;
            // 
            // status
            // 
            this.status.Text = "Status";
            this.status.Width = 72;
            // 
            // phoneNumber
            // 
            this.phoneNumber.Text = "Phone #";
            this.phoneNumber.Width = 98;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button6);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(360, 72);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(128, 216);
            this.groupBox2.TabIndex = 58;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Conference Control";
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button3.Location = new System.Drawing.Point(16, 21);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(96, 24);
            this.button3.TabIndex = 46;
            this.button3.Text = "Create Conf";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button5.Location = new System.Drawing.Point(16, 53);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(96, 24);
            this.button5.TabIndex = 55;
            this.button5.Text = "End Conf";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(16, 85);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 24);
            this.button1.TabIndex = 44;
            this.button1.Text = "Join Location";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Location = new System.Drawing.Point(16, 117);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(96, 24);
            this.button2.TabIndex = 45;
            this.button2.Text = "Kick Location";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button4.Location = new System.Drawing.Point(16, 149);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(96, 24);
            this.button4.TabIndex = 54;
            this.button4.Text = "Mute Location";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.button6.Location = new System.Drawing.Point(16, 181);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(96, 24);
            this.button6.TabIndex = 62;
            this.button6.Text = "Terminate All";
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(528, 112);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(104, 88);
            this.groupBox1.TabIndex = 57;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connections";
            // 
            // label18
            // 
            this.label18.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label18.Location = new System.Drawing.Point(8, 64);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(48, 20);
            this.label18.TabIndex = 9;
            this.label18.Text = "Failed";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label16
            // 
            this.label16.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label16.Location = new System.Drawing.Point(56, 64);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(40, 20);
            this.label16.TabIndex = 8;
            this.label16.Text = "0";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label3.Location = new System.Drawing.Point(56, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "0";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(56, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "0";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label10.Location = new System.Drawing.Point(56, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 20);
            this.label10.TabIndex = 5;
            this.label10.Text = "0";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label7.Location = new System.Drawing.Point(8, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 20);
            this.label7.TabIndex = 4;
            this.label7.Text = "Total";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label6.Location = new System.Drawing.Point(8, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 20);
            this.label6.TabIndex = 2;
            this.label6.Text = "Live";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label5.Location = new System.Drawing.Point(8, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "Pending";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.label23);
            this.groupBox4.Controls.Add(this.label24);
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox4.Location = new System.Drawing.Point(528, 216);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(104, 72);
            this.groupBox4.TabIndex = 59;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Kicks";
            // 
            // label14
            // 
            this.label14.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label14.Location = new System.Drawing.Point(56, 48);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 20);
            this.label14.TabIndex = 20;
            this.label14.Text = "0";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label9.Location = new System.Drawing.Point(8, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 20);
            this.label9.TabIndex = 21;
            this.label9.Text = "Failed";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label17
            // 
            this.label17.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label17.Location = new System.Drawing.Point(56, 32);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(40, 20);
            this.label17.TabIndex = 19;
            this.label17.Text = "0";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            this.label19.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label19.Location = new System.Drawing.Point(8, 32);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(48, 20);
            this.label19.TabIndex = 18;
            this.label19.Text = "Total";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label23
            // 
            this.label23.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label23.Location = new System.Drawing.Point(56, 16);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(40, 20);
            this.label23.TabIndex = 23;
            this.label23.Text = "0";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label24
            // 
            this.label24.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label24.Location = new System.Drawing.Point(8, 16);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(48, 20);
            this.label24.TabIndex = 22;
            this.label24.Text = "Pending";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.label21);
            this.groupBox3.Controls.Add(this.label22);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Location = new System.Drawing.Point(528, 8);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(104, 88);
            this.groupBox3.TabIndex = 59;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Conferences";
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label4.Location = new System.Drawing.Point(8, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 20);
            this.label4.TabIndex = 17;
            this.label4.Text = "Failed";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label8.Location = new System.Drawing.Point(56, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 20);
            this.label8.TabIndex = 16;
            this.label8.Text = "0";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label11.Location = new System.Drawing.Point(56, 48);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 20);
            this.label11.TabIndex = 15;
            this.label11.Text = "0";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label12.Location = new System.Drawing.Point(56, 32);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(40, 20);
            this.label12.TabIndex = 14;
            this.label12.Text = "0";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label13.Location = new System.Drawing.Point(56, 16);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(40, 20);
            this.label13.TabIndex = 13;
            this.label13.Text = "0";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label20
            // 
            this.label20.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label20.Location = new System.Drawing.Point(8, 48);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(48, 20);
            this.label20.TabIndex = 12;
            this.label20.Text = "Total";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label21
            // 
            this.label21.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label21.Location = new System.Drawing.Point(8, 32);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(48, 20);
            this.label21.TabIndex = 11;
            this.label21.Text = "Live";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label22
            // 
            this.label22.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label22.Location = new System.Drawing.Point(8, 16);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(48, 20);
            this.label22.TabIndex = 10;
            this.label22.Text = "Pending";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // performanceCounter1
            // 
            this.performanceCounter1.CategoryName = "Process";
            this.performanceCounter1.CounterName = "% Processor Time";
            // 
            // label25
            // 
            this.label25.Location = new System.Drawing.Point(353, 574);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(80, 16);
            this.label25.TabIndex = 19;
            this.label25.Text = "Length of Test:";
            // 
            // timeLabel
            // 
            this.timeLabel.Location = new System.Drawing.Point(432, 574);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(48, 16);
            this.timeLabel.TabIndex = 20;
            this.timeLabel.Text = "0:00:00";
            // 
            // MecStressTest
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(640, 593);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.metreosLink);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.testOutput_Dashboard_Panel);
            this.Menu = this.mainMenu1;
            this.Name = "MecStressTest";
            this.Text = "MEC Stress Test Control Center";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MecStressTest_Closing);
            this.testOutput_Dashboard_Panel.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounter1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion


        

        public void oneConference_Go_CheckedChanged(object sender, System.EventArgs e)
        {
        
        }

        public void oneConference_OneSizeSettings_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
        
        }

        public void executeButton_Click(object sender, System.EventArgs e)
        {
            ExecuteButtonPressed();
        }

        private void dashBoard_executeButton_Click(object sender, System.EventArgs e)
        {
            ExecuteButtonPressed();            
        }

        public void stopButton_Click(object sender, System.EventArgs e)
        {
            this.stopButton.Enabled = false;
            this.executeButton.Enabled = false;
            if(currentTest != null)
            {
                currentTest.EndTest();
            }
        }    

        public void UpdateClock(object o, System.Timers.ElapsedEventArgs args)
        {
            // Update the clock every second or so
            TimeSpan amountOfElapsedTime = DateTime.Now.Subtract(this.testStartedAtThisTime);
            timeLabel.Text = amountOfElapsedTime.Hours.ToString() + ':' + amountOfElapsedTime.Minutes.ToString() + ':' + amountOfElapsedTime.Seconds.ToString();
        }
        public void RunTestThread()
        {
            // GUI and debugging info
            statusBar1.Text = "Running...";
            this.richTextBox1.AppendText("\n\nInitializing MEC-Stress Test.");
            testStartedAtThisTime = DateTime.Now;
            clockThread.Start();


            // Run One Conference

            // Grab all checked boxes, and establish which tests are to be run


            // Run Randomized Conference
          
            UpdateVerboseInfo("\nBeginning Randomized Conference test.");
            
            this.RandomizedConference_SkewedRandom();

            UpdateButtonsAfterTest();

            UpdateVerboseInfo("\nEnding Randomized Conference Test.");
            UpdateVerboseInfo("\nAll Tests completed.");

            statusBar1.Text = "Done";
        }

        public static void Main()
        {
            // Subscribe to thread (unhandled) exception events
            ThreadExceptionHandler handler = 
                new ThreadExceptionHandler();

            Application.ThreadException += 
                new ThreadExceptionEventHandler(
                handler.Application_ThreadException);

            Application.EnableVisualStyles(); 
            
            Application.DoEvents();


            Application.Run( new MecStressTest());          
        }

        public void menuItem2_Click(object sender, System.EventArgs e)
        {
            executeButton_Click(sender,e);
        }

        public void menuItem3_Click(object sender, System.EventArgs e)
        {
            Dispose(true);
        }

		public void RandomizedConference_SkewedRandom()
		{
			currentTest = new RandomizedConference2((IMecTester)this, settings);
			currentTest.Start();
		}

        private void ExecuteButtonPressed()
        {
            mainTestThread = null;
            mainTestThread = new Thread(new ThreadStart(this.RunTestThread));
            this.mainTestThread.Start();
            this.executeButton.Enabled = false;
            this.stopButton.Enabled = true;
            this.testOutput_Dashboard_Panel.Visible = true;
        }

        private void UpdateButtonsAfterTest()
        {
            mainTestThread = null;
            this.executeButton.Enabled = true;
            this.stopButton.Enabled = false;
        }

        private void MecStressTest_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
        }

        private void menuItem9_Click(object sender, System.EventArgs e)
        {
            StressTesting.GlobalSettings globalSettings = new StressTesting.GlobalSettings(settings);
            globalSettings.ShowDialog(this);
        }

        private void menuItem10_Click(object sender, System.EventArgs e)
        {
            StressTesting.CallerSettings callerSettings = new StressTesting.CallerSettings(settings);
            callerSettings.ShowDialog(this);
        }


        private void menuItem13_Click(object sender, System.EventArgs e)
        {
            TestSettings testSettings = new TestSettings(settings);
            testSettings.ShowDialog(this);
        } 
        #region IMecTester Members

        public void NewConference(string conferenceGuid, string locationGuid, string status, string phoneNumber)
        {
            lock(listViewLock)
            {
                // record location of connection in listView - 0 root and vice versa
                LocationGuidTable[locationGuid] = listView1.Items.Count;
                OrderItemsTable.Add(locationGuid);
                LocationGuidTable[conferenceGuid] = listView1.Items.Count + 1;
                OrderItemsTable.Add(conferenceGuid);
                string spacerGuid = DateTime.Now.ToString();
                OrderItemsTable.Add(spacerGuid);
                LocationGuidTable[spacerGuid] = listView1.Items.Count + 2;

                ListViewItem item = new ListViewItem( new string[] {"-", "-", status, phoneNumber}, -1, System.Drawing.Color.Empty, System.Drawing.Color.LightGreen, null);

                listView1.Items.Add(item);

                // Creation of the ending conference-stat bar
                ListViewItem itemConf = new ListViewItem( new string[] {"Live: 0", "Joining: 1", "initializing", "Kicking: 0"}, 1, System.Drawing.Color.Empty, System.Drawing.Color.FromArgb(0, 197, 0 ), null);
               
                listView1.Items.Add(itemConf);

                // Creation of the whitebar located in-between conferences
                ListViewItem itemWhiteBar = new ListViewItem( new string[] {"", "", "", ""}, 1);

                listView1.Items.Add(itemWhiteBar);

                listView1.Refresh();
            }
        }

        public void NewLocation(string conferenceGuid, string locationGuid, string conferenceId, string status, string phoneNumber)
        {
            
            lock(listViewLock)
            {

                bool foundConference = false;
                // find last spot in Items that are of similiar conference type
                for(int i = 0; i < listView1.Items.Count; i++)
                {
                    ListViewItem item = listView1.Items[i];

                    // find the conference this new location belongs too.
                    if(conferenceId == item.SubItems[0].Text)
                    {
                        foundConference = true;
                    }

                    // checks that we have found the conference group, and either the group has changed,
                    if(foundConference == true && ( conferenceId != item.SubItems[0].Text) )
                    {
                        LocationGuidTable[locationGuid] = i;

                        OrderItemsTable.Insert(i, locationGuid);

                        // correct tables
                        for(int j = i + 1; j < OrderItemsTable.Count;j++)
                        {
                            string locationGuidToIncrement = (string) OrderItemsTable[j];

                            int location = (int) LocationGuidTable[locationGuidToIncrement];

                            LocationGuidTable[locationGuidToIncrement] = location + 1;
                        }

                        string conferenceToEnter;

                        if(conferenceId != null)
                        {
                            conferenceToEnter = conferenceId;
                        }
                        else
                        {
                            conferenceToEnter = "-";
                        }

                        ListViewItem itemToInsert = new ListViewItem( new string[] {conferenceToEnter, "-", status, phoneNumber}, -1, System.Drawing.Color.Empty, System.Drawing.Color.LightGreen, null);
                        
                        listView1.Items.Insert(i, itemToInsert);

                        listView1.Refresh();

                        break;
                    }

                    // or its the last item
                    // can't happen anymore, or shouldn't
                    if(foundConference == true && ( i == listView1.Items.Count - 1) )
                    {
                        LocationGuidTable[locationGuid] = i + 1;

                        ListViewItem itemToAdd = new ListViewItem( new string[] {"-", "-", status, phoneNumber}, -1, System.Drawing.Color.Empty, System.Drawing.Color.LightGreen, null);

                        listView1.Items.Add(itemToAdd);

                        OrderItemsTable.Add(locationGuid);
                        listView1.Refresh();

                        break;
                    }
                }
            }
        }

        public void UpdateLocation(string conferenceGuid, string locationGuid, string conferenceId, string locationId, string status, string phoneNumber)
        {
            lock(listViewLock)
            {
                int i = (int) LocationGuidTable[locationGuid];

                if((string)OrderItemsTable[i] != locationGuid)
                {
                    MessageBox.Show("ListView out of order");
                }

                ListViewItem item;

                switch(status)
                {
                    case "kicking":
                        item = new ListViewItem( new string[] {"-", "-", status, phoneNumber}, -1, System.Drawing.Color.Empty, System.Drawing.Color.LightPink, null);
                        break;
                    case "online":
                        item = new ListViewItem(new string[] {conferenceId, locationId, status, phoneNumber});
                        break;
                    case IStressTesting.MUTING:
                        item = new ListViewItem( new string[] {"-", "-", status, phoneNumber}, -1, System.Drawing.Color.Empty, System.Drawing.Color.LightPink, null);
                        item.BackColor = System.Drawing.Color.LightPink;
                        item.ForeColor = System.Drawing.Color.Empty; 
                        break;
                    default:
                        item = new ListViewItem(new string[] {conferenceId, locationId, status, phoneNumber});
                        break;
                }

                listView1.Items[i] = item;

                listView1.Refresh();
            }
        }

        public void UpdateEstablishedLocation(string conferenceGuid, string locationGuid, string conferenceId, string locationId, string status)
        {
            lock(listViewLock)
            {
                int i = (int) LocationGuidTable[locationGuid];

                if((string)OrderItemsTable[i] != locationGuid)
                {
                    MessageBox.Show("ListView out of order");
                }

                string phoneNumber = listView1.Items[i].SubItems[3].Text;

                ListViewItem item;

                switch(status)
                {
                    case "kicking":
                        item = new ListViewItem( new string[] {conferenceId, locationId, status, phoneNumber}, -1,  System.Drawing.Color.Empty, System.Drawing.Color.LightPink, null);
                        break;
                    case IStressTesting.MUTING:
                        item = new ListViewItem( new string[] {conferenceId, locationId, status, phoneNumber}, -1, System.Drawing.Color.Empty, System.Drawing.Color.GhostWhite, null);
                        break;
                    case IStressTesting.MUTED:
                        item = new ListViewItem( new string[] {conferenceId, locationId, status, phoneNumber}, -1, System.Drawing.Color.Empty, System.Drawing.Color.LightGray, null);
                        break;
                    case "lost":
                        item = new ListViewItem( new string[] {conferenceId, locationId, status, phoneNumber}, -1, System.Drawing.Color.Empty, System.Drawing.Color.FromArgb(255,255,0), null);
                        break;
                    default:
                        item = new ListViewItem(new string[] {conferenceId, locationId, status, phoneNumber});
                        break;
                }

                listView1.Items[i] = item;

                listView1.Refresh();
            }
        }

        public void UpdateConference(string conferenceGuid, int liveConnections, int pendingJoins, string status, int pendingKicks)
        {

            lock(listViewLock)
            {
                int i = (int) LocationGuidTable[conferenceGuid];

                if((string)OrderItemsTable[i] != conferenceGuid)
                {
                    MessageBox.Show("ListView out of order");
                    return;
                }

                ListViewItem item;

                switch(status)
                {
                    case "lost":
                        item = new ListViewItem( new string[] {"Live: " + liveConnections , "Joining: " + pendingJoins, status , "Kicking: " + pendingKicks}, -1, System.Drawing.Color.Empty, System.Drawing.Color.FromArgb(255,255,0), null);
                        break;
                    case "online":
                        item = new ListViewItem( new string[] {"Live: " + liveConnections , "Joining: " + pendingJoins, status , "Kicking: " + pendingKicks});
                        break;
                    case "ended":
                        item = new ListViewItem( new string[] {"Live: " + liveConnections , "Joining: " + pendingJoins, status , "Kicking: " + pendingKicks}, -1, System.Drawing.Color.Empty, System.Drawing.Color.FromArgb(197,0,0), null);
                        break;
                    default:
                        item = new ListViewItem(new string[] {"Live: " + liveConnections , "Joining: " + pendingJoins, status , "Kicking: " + pendingKicks});
                        break;
                }

                listView1.Items[i] = item;

                listView1.Refresh();
            }
        }
        public void RemoveLocation(string conferenceGuid, string locationGuid)
        {
            Debug.Assert(locationGuid != null, "locationGuid is null");
            lock(listViewLock)
            {
                int i;
                if(LocationGuidTable.Contains(locationGuid))
                {
                    i = (int) LocationGuidTable[locationGuid];
                }
                else
                {
                    UpdateErrorInfo("Unable to remove pending connection from ListView");
                    return;
                }

                listView1.Items[i].Remove();

                // update the both collections
                LocationGuidTable.Remove(locationGuid);
                OrderItemsTable.RemoveAt(i);

                // update the numbering in all above numbers
                for(int j = i; j < OrderItemsTable.Count; j++)
                {
                    string locationGuidToDecrement = (string) OrderItemsTable[j];

                    int locationOfItem = (int) LocationGuidTable[locationGuidToDecrement];

                    LocationGuidTable[locationGuidToDecrement] = locationOfItem - 1;

                }

                listView1.Refresh();
            }
        }

        public void RemoveConference(string conferenceGuid)
        {
            Debug.Assert(conferenceGuid != null, "conferenceGuid is null");

            lock(listViewLock)
            {

                int i;
                if(LocationGuidTable.Contains(conferenceGuid))
                {
                    i = (int) LocationGuidTable[conferenceGuid];
                }
                else
                {
                    UpdateErrorInfo("Unable to remove conference bar from ListView");
                    return;
                }

                listView1.Items[i].Remove();
                listView1.Items[i].Remove();

                // update both collections
                LocationGuidTable.Remove(conferenceGuid);
                OrderItemsTable.RemoveAt(i);

                // also remove the white spacer located between conferences
                string spacerGuid  = (string) OrderItemsTable[i];

                LocationGuidTable.Remove(spacerGuid);
                OrderItemsTable.RemoveAt(i);

                // update the numbering in all above numbers
                for(int j = i; j < OrderItemsTable.Count; j++)
                {
                    string locationGuidToDecrement = (string) OrderItemsTable[j];

                    int locationOfItem = (int) LocationGuidTable[locationGuidToDecrement];

                    LocationGuidTable[locationGuidToDecrement] = locationOfItem - 2;

                }

                listView1.Refresh();
            }
        }

        public void UpdatePendingConferences(int pendingConferences)
        {
            PendingConferences = pendingConferences + PendingConferences;
        }

        public void UpdateLiveConferences(int liveConferences)
        {
            LiveConferences = liveConferences + LiveConferences;

            if(liveConferences > 0)
            {
                UpdateTotalConferences(liveConferences);
            }
        }

        public void UpdateTotalConferences(int totalConferences)
        {
           TotalConferences = totalConferences + TotalConferences;
        }

        public void UpdateFailedConferences(int failedConferences)
        {
            FailedConferences = failedConferences + FailedConferences;
        }

        public void UpdatePendingConnections(int pendingConnections)
        {
            PendingConnections = pendingConnections + PendingConnections;
        }

        public void UpdateLiveConnections(int liveConnections)
        {
            LiveConnections = liveConnections + LiveConnections;

            if(liveConnections > 0)
            {
                UpdateTotalConnections(liveConnections);
            }
        }

        public void UpdateTotalConnections(int totalConnections)
        {
            TotalConnections = totalConnections + TotalConnections;
        }

        public void UpdateFailedConnections(int failedConnections)
        {
            FailedConnections = failedConnections + FailedConnections;
        }

        public void UpdatePendingKicks(int pendingKicks)
        {
            PendingKicks = pendingKicks + PendingKicks;

            if(pendingKicks > 0)
            {
                UpdateTotalKicks(pendingKicks);
            }
        }

        public void UpdateTotalKicks(int totalKicks)
        {
            TotalKicks = totalKicks + TotalKicks;
        }

        public void UpdateFailedKicks(int failedKicks)
        {
            FailedKicks = failedKicks + FailedKicks;
        }

        public void UpdateVerboseInfo(string info)
        {
            VerboseInfo = info;
        }

        public void UpdateErrorInfo(string info)
        {
           ErrorInfo = info;
        }

		public void RegisterCreateConference(IStressTesting.CreateConferenceDelegate callback)
		{
			this.createCallback = callback;
		}

		public void RegisterEndConference(IStressTesting.EndConferenceDelegate callback)
		{
			this.endCallback = callback;
		}

		public void RegisterJoinLocation(IStressTesting.JoinLocationDelegate callback)
		{
			this.joinCallback = callback;
		}

		public void RegisterKickLocation(IStressTesting.KickLocationDelegate callback)
		{
			this.kickCallback = callback;
		}

		public void RegisterMuteLocation(IStressTesting.MuteLocationDelegate callback)
		{
			this.muteCallback = callback;
		}

        public void RegisterTerminateAll(IStressTesting.TerminateAllDelegate callback)
        {
            this.terminateAllCallback = callback;
        }

		public void CreateConference(string phoneNumber, bool allowRandom)
		{
			createCallback.BeginInvoke(phoneNumber, allowRandom,  null, null);
		}

		public void EndConference(string conferenceId)
		{
			endCallback.BeginInvoke(conferenceId, null, null);
		}

		public void JoinLocation(string conferenceId, string phoneNumber)
		{
			joinCallback.BeginInvoke(conferenceId, phoneNumber, null, null);
		}

		public void KickLocation(string locationGuid, string conferenceId, string locationId)
		{
			kickCallback.BeginInvoke(locationGuid, conferenceId, locationId, null, null);
		}

		public void MuteLocation(string locationGuid, string conferenceId, string locationId)
		{
			muteCallback.BeginInvoke(locationGuid, conferenceId, locationId, null, null);
		}

        public void TerminateAll()
        {
            terminateAllCallback.BeginInvoke(null,null);
        }

        #endregion

		private void button3_Click(object sender, System.EventArgs e)
		{
            CreateConferenceWindow window = new CreateConferenceWindow();
				
            DialogResult result = window.ShowDialog(this);

            if(result == DialogResult.OK)
            {
                if(window.useAutoChecked || window.phoneNumberText == "")
                {
                    this.CreateConference("-1", window.allowRandomChecked);
                }
                else
                {
                    try
                    {
                        this.CreateConference(window.phoneNumberText, window.allowRandomChecked);
                    }
                    catch
                    {
                        MessageBox.Show("Ignored Create Location:  invalid phoneNumber");
                    }
                }
            }

            window.Close();
		}

		private void button5_Click(object sender, System.EventArgs e)
		{
            lock(listViewLock)
            {
                bool anySelected = false;
                ArrayList itemsSelected = new ArrayList();
                // grab which conference is selected from listview

                int numberItemsSelected = listView1.SelectedItems.Count;

                if(numberItemsSelected > 0)
                {
                    anySelected = true;
                }

                if(anySelected)
                {
                    for(int i = 0; i < numberItemsSelected; i++)
                    {
                        EndConference(listView1.SelectedItems[i].SubItems[0].Text);
                    }
                }
            }
		}

		private void button1_Click(object sender, System.EventArgs e)
	    {
            if(listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a conference");
            }
            else
            {
                string conferenceId = listView1.SelectedItems[0].SubItems[0].Text;


                JoinLocationWindow window = new JoinLocationWindow("Join");
			
                DialogResult result = window.ShowDialog(this);

                if(result == DialogResult.OK)
                {
                    if(window.useAuto || window.phoneNumber == "")
                    {
                        this.JoinLocation(conferenceId, "-1");
                    }
                    else
                    {
                        try
                        {
                            this.JoinLocation(conferenceId, window.phoneNumber);
                        }
                        catch
                        {
                            MessageBox.Show("Ignored Join Location:  invalid phoneNumber");
                        }
                    }
                }

                window.Close();
            }
       
		}

        private void button2_Click(object sender, System.EventArgs e)
        {
            lock(listViewLock)
            {
                if(listView1.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select a location");
                }
                else
                {
                    string locationId = listView1.SelectedItems[0].SubItems[1].Text;
                    string conferenceId = listView1.SelectedItems[0].SubItems[0].Text;
                
                    string locationGuid = (string) this.OrderItemsTable[listView1.SelectedItems[0].Index];

                    this.KickLocation(locationGuid, conferenceId, locationId);
                }
            }
        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            lock(listViewLock)
            {
                if(listView1.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select a location");
                }
                else
                {
                    string locationId = listView1.SelectedItems[0].SubItems[1].Text;
                    string conferenceId = listView1.SelectedItems[0].SubItems[0].Text;

                    string locationGuid = (string) OrderItemsTable[listView1.SelectedItems[0].Index];

                    this.MuteLocation(locationGuid, conferenceId, locationId);
                }
            }
        }

        private void button6_Click(object sender, System.EventArgs e)
        {
            this.TerminateAll();
        }

        
        internal class ThreadExceptionHandler
         {
             /// 
             /// Handles the thread exception.
             /// 
             public void Application_ThreadException(
                 object sender, ThreadExceptionEventArgs e)
             {
                 try
                 {
                     // Exit the program if the user clicks Abort.
                     DialogResult result = ShowThreadExceptionDialog(
                         e.Exception);

                     if (result == DialogResult.Abort) 
                         Application.Exit();
                 }
                 catch
                 {
                     // Fatal error, terminate program
                     try
                     {
                         MessageBox.Show("Fatal Error", 
                             "Fatal Error",
                             MessageBoxButtons.OK, 
                             MessageBoxIcon.Stop);
                     }
                     finally
                     {
                         Application.Exit();
                     }
                 }
             }

             /// 
             /// Creates and displays the error message.
             /// 
             private DialogResult ShowThreadExceptionDialog(Exception ex) 
             {
                 string errorMessage= 
                     "Unhandled Exception:\n\n" +
                     ex.Message + "\n\n" + 
                     ex.GetType() + 
                     "\n\nStack Trace:\n" + 
                     ex.StackTrace;

                 return MessageBox.Show(errorMessage, 
                     "Application Error", 
                     MessageBoxButtons.AbortRetryIgnore, 
                     MessageBoxIcon.Stop);
             }
         } // End ThreadExceptionHandler

	}
}
