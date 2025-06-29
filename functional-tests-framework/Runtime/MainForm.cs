using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using Metreos.Samoa.FunctionalTestFramework;

namespace Metreos.Samoa.FunctionalTestRuntime
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		public System.Windows.Forms.TreeView Explorer{ get{ return explorer; }}
		public System.Windows.Forms.StatusBar Status{ get{ return statusBar1; }}

		private ArrayList masterTreeNodeCollection;
		private IFunctionalTestConduit conduit;
		private TestInputControl userInput;
		private bool testExecuting;
		private bool automatedTestsExecuting;
		private bool connected;
		private string status;

		private object outputLock;
		private object instructionLock;
		private object testSettingsFileLock;

		private System.Windows.Forms.TreeView explorer;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.RichTextBox outputBox;
		private System.Windows.Forms.RichTextBox promptBox;
		private System.Windows.Forms.RichTextBox instructionsBox;
		private System.Windows.Forms.RichTextBox descriptionBox;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.Windows.Forms.MenuItem menuItem17;
		private System.Windows.Forms.MenuItem menuItem18;
		private System.Windows.Forms.MenuItem menuItem19;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem20;
		private System.Windows.Forms.MenuItem menuItem21;
		private System.Windows.Forms.Panel testGuiPanel;

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.explorer = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.outputBox = new System.Windows.Forms.RichTextBox();
            this.promptBox = new System.Windows.Forms.RichTextBox();
            this.testGuiPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem20 = new System.Windows.Forms.MenuItem();
            this.menuItem21 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuItem18 = new System.Windows.Forms.MenuItem();
            this.menuItem19 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.menuItem16 = new System.Windows.Forms.MenuItem();
            this.menuItem17 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.instructionsBox = new System.Windows.Forms.RichTextBox();
            this.descriptionBox = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.testGuiPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // explorer
            // 
            this.explorer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left)));
            this.explorer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.explorer.ImageList = this.imageList1;
            this.explorer.Location = new System.Drawing.Point(0, 47);
            this.explorer.Name = "explorer";
            this.explorer.Size = new System.Drawing.Size(224, 513);
            this.explorer.TabIndex = 0;
            this.explorer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.explorer_AfterSelect);
            this.explorer.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.explorer_BeforeSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // outputBox
            // 
            this.outputBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.outputBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.outputBox.HideSelection = false;
            this.outputBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.outputBox.Location = new System.Drawing.Point(0, 24);
            this.outputBox.Name = "outputBox";
            this.outputBox.ReadOnly = true;
            this.outputBox.Size = new System.Drawing.Size(512, 104);
            this.outputBox.TabIndex = 0;
            this.outputBox.Text = "";
            this.outputBox.TextChanged += new System.EventHandler(this.outputBox_TextChanged);
            // 
            // promptBox
            // 
            this.promptBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.promptBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.promptBox.HideSelection = false;
            this.promptBox.Location = new System.Drawing.Point(0, 152);
            this.promptBox.Name = "promptBox";
            this.promptBox.ReadOnly = true;
            this.promptBox.Size = new System.Drawing.Size(512, 120);
            this.promptBox.TabIndex = 1;
            this.promptBox.Text = "";
            this.promptBox.TextChanged += new System.EventHandler(this.outputBox_TextChanged);
            // 
            // testGuiPanel
            // 
            this.testGuiPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.testGuiPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.testGuiPanel.Controls.Add(this.label2);
            this.testGuiPanel.Location = new System.Drawing.Point(232, 16);
            this.testGuiPanel.Name = "testGuiPanel";
            this.testGuiPanel.Size = new System.Drawing.Size(520, 240);
            this.testGuiPanel.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label2.Location = new System.Drawing.Point(-1, -1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(520, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "No Test Selected";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusBar1
            // 
            this.statusBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.statusBar1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusBar1.Location = new System.Drawing.Point(0, 584);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(760, 16);
            this.statusBar1.TabIndex = 2;
            this.statusBar1.Text = "statusBar1";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(0, 560);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(760, 16);
            this.progressBar1.TabIndex = 3;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                      this.menuItem1,
                                                                                      this.menuItem10,
                                                                                      this.menuItem4,
                                                                                      this.menuItem3});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                      this.menuItem7,
                                                                                      this.menuItem2});
            this.menuItem1.Text = "File";
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 0;
            this.menuItem7.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                      this.menuItem6,
                                                                                      this.menuItem20,
                                                                                      this.menuItem21});
            this.menuItem7.Text = "Run All Tests";
            this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 0;
            this.menuItem6.Text = "All Tests";
            this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // menuItem20
            // 
            this.menuItem20.Index = 1;
            this.menuItem20.Text = "All Automated Tests";
            // 
            // menuItem21
            // 
            this.menuItem21.Index = 2;
            this.menuItem21.Text = "All Unautomated Tests";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "Close";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 1;
            this.menuItem10.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                       this.menuItem11,
                                                                                       this.menuItem18,
                                                                                       this.menuItem19});
            this.menuItem10.Text = "View";
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 0;
            this.menuItem11.Text = "All Tests";
            // 
            // menuItem18
            // 
            this.menuItem18.Index = 1;
            this.menuItem18.Text = "Automated Only";
            // 
            // menuItem19
            // 
            this.menuItem19.Index = 2;
            this.menuItem19.Text = "Unautomated Only";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                      this.menuItem5,
                                                                                      this.menuItem12});
            this.menuItem4.Text = "Settings";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 0;
            this.menuItem5.Text = "Global Settings";
            this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 1;
            this.menuItem12.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                       this.menuItem15,
                                                                                       this.menuItem16,
                                                                                       this.menuItem17});
            this.menuItem12.Text = "Reset Settings";
            this.menuItem12.Click += new System.EventHandler(this.menuItem12_Click);
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 0;
            this.menuItem15.Text = "Reset Global Settings";
            this.menuItem15.Click += new System.EventHandler(this.menuItem15_Click);
            // 
            // menuItem16
            // 
            this.menuItem16.Index = 1;
            this.menuItem16.Text = "Individual Test Settings (Permanently)";
            this.menuItem16.Click += new System.EventHandler(this.menuItem16_Click);
            // 
            // menuItem17
            // 
            this.menuItem17.Index = 2;
            this.menuItem17.Text = "Individual Test Settings (Temporarily)";
            this.menuItem17.Click += new System.EventHandler(this.menuItem17_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 3;
            this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                      this.menuItem14,
                                                                                      this.menuItem13});
            this.menuItem3.Text = "Help";
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 0;
            this.menuItem14.Text = "Understanding the GUI";
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 1;
            this.menuItem13.Text = "About";
            // 
            // menuItem9
            // 
            this.menuItem9.Index = -1;
            this.menuItem9.Text = "";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(163, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 24);
            this.button1.TabIndex = 4;
            this.button1.Text = "Go";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(0, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 32);
            this.label1.TabIndex = 6;
            this.label1.Text = "Choose a test(s)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(232, 264);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(520, 304);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.instructionsBox);
            this.tabPage1.Controls.Add(this.descriptionBox);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(512, 275);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Initialization";
            // 
            // instructionsBox
            // 
            this.instructionsBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.instructionsBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.instructionsBox.HideSelection = false;
            this.instructionsBox.Location = new System.Drawing.Point(0, 152);
            this.instructionsBox.Name = "instructionsBox";
            this.instructionsBox.ReadOnly = true;
            this.instructionsBox.Size = new System.Drawing.Size(512, 120);
            this.instructionsBox.TabIndex = 3;
            this.instructionsBox.Text = "";
            this.instructionsBox.TextChanged += new System.EventHandler(this.outputBox_TextChanged);
            // 
            // descriptionBox
            // 
            this.descriptionBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.descriptionBox.HideSelection = false;
            this.descriptionBox.Location = new System.Drawing.Point(0, 24);
            this.descriptionBox.Name = "descriptionBox";
            this.descriptionBox.ReadOnly = true;
            this.descriptionBox.Size = new System.Drawing.Size(512, 104);
            this.descriptionBox.TabIndex = 2;
            this.descriptionBox.Text = "";
            this.descriptionBox.TextChanged += new System.EventHandler(this.outputBox_TextChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label6.Location = new System.Drawing.Point(0, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(512, 24);
            this.label6.TabIndex = 1;
            this.label6.Text = "Instructions";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label525
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(512, 24);
            this.label5.TabIndex = 0;
            this.label5.Text = "Description";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.outputBox);
            this.tabPage2.Controls.Add(this.promptBox);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(512, 275);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Runtime";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(512, 24);
            this.label4.TabIndex = 8;
            this.label4.Text = "User Prompt";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(512, 24);
            this.label3.TabIndex = 7;
            this.label3.Text = "Test Output";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(760, 601);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.explorer);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.testGuiPanel);
            this.Menu = this.mainMenu1;
            this.Name = "MainForm";
            this.Text = "Functional Tests";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.testGuiPanel.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

		private System.ComponentModel.IContainer components;

		public MainForm(IFunctionalTestConduit conduit)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.conduit = conduit;
			conduit.OutputLine += new CommonTypes.OutputLine(OutputLine);
			conduit.StatusUpdate += new CommonTypes.StatusUpdate(StatusUpdate);
			conduit.InstructionLine += new CommonTypes.InstructionLine(InstructionLine);
			conduit.AddNewTest += new CommonTypes.AddTest(AddNode);
			conduit.UpdateConnectedStatus += new CommonTypes.ServerConnectionStatus(UpdateStatus);
			conduit.ResetProgress += new CommonTypes.ResetProgressMeter(ResetMeter);
			conduit.AdvanceProgress += new CommonTypes.AdvanceProgressMeter(AdvanceMeter);
			conduit.FrameworkLoaded += new CommonTypes.LoadDone(LoadDone);
			conduit.TestEnded += new CommonTypes.TestEndedDelegate(TestDone);
            conduit.TestNowAbortable += new CommonTypes.TestAbortable(TestIsAbortable);
			
			userInput = new TestInputControl(new ArrayList(), new CommonTypes.UpdateTestSettings(UpdateTestSettings));
            
			userInput.Size = new System.Drawing.Size(518, 214);
			userInput.Location = new System.Drawing.Point(0, 24);
			userInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
            
			this.testGuiPanel.SuspendLayout();
			this.testGuiPanel.Controls.Add(userInput);
			this.testGuiPanel.ResumeLayout();

			masterTreeNodeCollection = new ArrayList();

			testExecuting = false;
			automatedTestsExecuting = false;

			outputLock = new object();
			instructionLock = new object();
			testSettingsFileLock = new object();
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

            conduit.UnloadFramework();

			base.Dispose( disposing );
		}

		private void menuItem5_Click(object sender, System.EventArgs e)
		{
			SamoaSettings settingsForm = new SamoaSettings(conduit.Settings, conduit.TestSettingsFolder);

			settingsForm.ShowDialog(this);

			if(settingsForm.Changed)
			{
				conduit.ReconnectToServer();
			}

			if(!settingsForm.IsDisposed)
			{
				settingsForm.Dispose();
			}

			settingsForm = null;
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			string testName = null;
			
			if(explorer.SelectedNode != null)
			{	
                try
                {
                    testName = (explorer.SelectedNode.Tag as object[])[3] as string;
                }
                catch
                {
                    return;
                }
			}
			else
			{
				return;
			}

			if(!testExecuting)
			{
				ExecuteTest(testName);
			}
			else
			{
				AbortTest(testName);
			}
		}

        private void ExecuteTest(string testName)
        {
            button1.Enabled = false;
            button1.Text = Constants.ABORT_TEST;

            testExecuting = true;

            testGuiPanel.Tag = explorer.SelectedNode;

            this.outputBox.Text = "";
            this.promptBox.Text = "";

            this.tabControl1.SelectedTab = this.tabPage2;

            this.conduit.StartTest(testName, userInput.Values);  
        }

		private void TestDone(string testName, bool testSuccessful, bool ignoreSuccess)
		{
			bool foundTest = false;
			
            if(conduit.TestSettings.tests != null)
            {
                foreach(testType test in conduit.TestSettings.tests)
                {
                    if(test.name == testName)
                    {
                        test.success = testSuccessful;
                        foundTest = true;
                        break;
                    }
                }
            }

			if(!foundTest)
			{
				// Test couldn't be found, so create it.
				if(conduit.TestSettings.tests != null)
				{
					// Couldn't find test, must create it
					testType[] newTests = new testType[conduit.TestSettings.tests.Length + 1];

					conduit.TestSettings.tests.CopyTo(newTests, 0);

					testType newTest = new testType();
					newTest.success = testSuccessful;
					newTest.name = testName;

					newTests[newTests.Length - 1] = newTest;

					conduit.TestSettings.tests = newTests;
				}
				else
				{
					// First test, must create it
					testType[] newTests = new testType[1];

					testType newTest = new testType();
					newTest.success = testSuccessful;
					newTest.name = testName;

					newTests[0] = newTest;

					conduit.TestSettings.tests = newTests;
				} 
			}   
            
			ThreadPool.QueueUserWorkItem(new WaitCallback(SaveTestSettings), conduit.Settings.TestSettings);

            if(!ignoreSuccess)
            {
                if(testSuccessful)
                {
                    this.OutputLine(testName + " succeeded.");

                    if(testGuiPanel.Tag != null)
                    {
                        TreeNode node = testGuiPanel.Tag as TreeNode;

                        node.ImageIndex = Constants.previousTestSuccess;
                        node.SelectedImageIndex = Constants.go;

                        //UpdateGroupIcon(node.Parent);
                    
                        explorer.Refresh();                    
                    }
                }
                else
                {
                    this.OutputLine(testName + " failed.");

                    if(testGuiPanel.Tag != null)
                    {
                        TreeNode node = testGuiPanel.Tag as TreeNode;

                        node.ImageIndex = Constants.previousTestFailure;
                        node.SelectedImageIndex = Constants.go;

                        //UpdateGroupIcon(node.Parent);

                        explorer.Refresh();
                    }
                }
            }

			testExecuting = false;

            button1.Enabled = true;
			button1.Text = "Go";

            // Flip back to first tap
             //this.tabControl1.SelectedTab = this.tabPage1;
		}

		private void OutputLine(string line)
		{
			lock(outputLock)
			{
				outputBox.Text = outputBox.Text + "\r\n" + line; //AppendText();
			}
		}

		private void InstructionLine(string line)
		{
			lock(instructionLock)
			{
				promptBox.AppendText("\r\n" + line);
			}
		}

		private void StatusUpdate(string status)
		{
			if(status != null)
			{
				this.status = status;
				this.statusBar1.Text = connected ? "Connected to " + conduit.Settings.SamoaIP + ":  "  + status : "Not Connected:  " + status;
			}
			else
				this.statusBar1.Text = connected ? "Connected to " + conduit.Settings.SamoaIP + ":  "  + this.status  : "Not Connected:  " + this.status;
		}

        #region Initializing Logic of the Tree View

        private void AddNode(int baseNameSpaceLength, string fullTestName, ArrayList inputData, Hashtable configValues, bool firstTime, bool previousSuccess, string description, string instructions)
        {
            string[] spaces = fullTestName.Split(new char[] { '.' });

            // Parse last space from the namespace for the test name
            string testName = spaces[spaces.Length - 1];

            int initialImage = InitializeImage(firstTime, previousSuccess);
            
            foreach(TestInputDataBase input in inputData)
            {
                input.TestName = testName;
            }
          
            
            IterateThroughGroups(explorer.Nodes, fullTestName, spaces,
                baseNameSpaceLength, initialImage, inputData, description, instructions, configValues);
        }

        private void IterateThroughGroups(TreeNodeCollection groupNodes, string fullTestName, string[] spaces, int index, int initialImage, ArrayList inputData, string description, string instructions, Hashtable configValues)
        {
            string currentSpace = spaces[index];

            // If this is the last space, then the actual test has been found.
            if(index == spaces.Length - 1)
            {
                TreeNode node = CreateTestNode(currentSpace, fullTestName, initialImage, configValues, inputData, description, instructions);
                groupNodes.Add(node);
                return;
            }
            else
            {
                // Is just a group, so create group as needed, or simply navigate into the group
                
                foreach(TreeNode groupNode in groupNodes)
                {
                    // The node exists, so navigate into the next level.
                    if(currentSpace == groupNode.Text)
                    {
                        IterateThroughGroups(groupNode.Nodes, fullTestName, spaces, index + 1, initialImage, inputData, description, instructions, configValues);
                        return;
                    }
                }

                // Unable to find the group, so it must be created.
                TreeNode newGroupNode = new TreeNode(currentSpace, 
                    Constants.group, 
                    Constants.group);

                groupNodes.Add(newGroupNode);

                IterateThroughGroups(newGroupNode.Nodes, fullTestName, spaces, index + 1, initialImage, inputData, description, instructions, configValues);
            }
        }

        private TreeNode CreateTestNode(string testName, string fullTestName, int initialImage, Hashtable configValues, ArrayList inputData, string description, string instructions)
        {
            TreeNode node = new TreeNode(testName, initialImage, Constants.go);
					
            IDictionaryEnumerator enumerator = configValues.GetEnumerator();
            while(enumerator.MoveNext())
            {
                string configName = enumerator.Key as string;
                string defaultValue = enumerator.Value as string;

                foreach(TestInputDataBase input in inputData)
                {
                    input.TestName = testName;

                    if(configName == input.Variable)
                    {
                        input.SavedDefaultValue = defaultValue;
                    }
                }
            }
					
            object[] staticData = new object[] { inputData != null ? inputData : new ArrayList(), description, instructions, fullTestName };
            node.Tag = staticData;

            return node;
        }

        #endregion 

		private void UpdateStatus(bool status)
		{
			if(status)
			{
				connected = true;
				this.StatusUpdate(null);
			}
			else
			{
				connected = false;
				this.StatusUpdate(null);
			}
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			OutputLine("Connecting to Application Server at " + conduit.Settings.SamoaIP + ':' + conduit.Settings.SamoaPort);

			conduit.ConnectServer.BeginInvoke(new System.AsyncCallback(ConnectDone), null);
		}
    
		private void ConnectDone(IAsyncResult result)
		{
			AsyncResult aResult = result as AsyncResult;

			CommonTypes.ConnectServer temp = aResult.AsyncDelegate as CommonTypes.ConnectServer;

			bool connectStatus = temp.EndInvoke(result);
		}

        #region User Interaction

        /// <summary>Consumes event if a test is executing</summary>
        private void explorer_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if(testExecuting || automatedTestsExecuting)
            {
                e.Cancel = true;
            }
        }

		private void explorer_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if(explorer.SelectedNode.Tag != null)
			{
				object[] staticData = explorer.SelectedNode.Tag as object[];
                
				if(!testExecuting && !automatedTestsExecuting)
				{
					userInput.Tag = explorer.SelectedNode.Text;
					userInput.Reset(staticData[0] as ArrayList);
					userInput.PerformLayout();
					descriptionBox.Text = staticData[1] as string;
					instructionsBox.Text = staticData[2] as string;
                
					label2.Text = explorer.SelectedNode.Text;
					label2.Tag = explorer.SelectedNode;
				}
			}
		}

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabPage1.Refresh();
            tabPage2.Refresh();
            promptBox.Refresh();
            outputBox.Refresh();
            instructionsBox.Refresh();
            descriptionBox.Refresh();
        }

        private void menuItem16_Click(object sender, System.EventArgs e)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(TestSettings));

            FileInfo info = new FileInfo(conduit.TestSettingsFolder + "\\" + conduit.Settings.TestSettings);

            if(info.Exists)
            {
                try
                {
                    info.Delete();
                }
                catch
                {
                    
                }
            }

            conduit.TestSettings = new TestSettings();

            FileStream stream = null;

            try
            {
                stream = info.Create();
                serializer.Serialize(stream, conduit.TestSettings);
                stream.Close();
                stream = null;
            }
            catch
            {
                if(stream != null)
                    stream.Close();

                stream = null;
            }

            ResetNodes();
        }

        private void menuItem15_Click(object sender, System.EventArgs e)
        {
            DialogResult result = MessageBox.Show("This will revert the Functional Tester to the defaults of the global settings. ", "Reset Global Settings", MessageBoxButtons.YesNo);

            if(result == DialogResult.No)
                return;

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Settings));

            FileInfo info = new FileInfo(conduit.TestSettingsFolder + "\\" + "Settings.txt");

            if(info.Exists)
            {
                try
                {
                    info.Delete();
                }
                catch
                {
                    
                }
            }

            conduit.Settings = new Settings();

            
            FileStream stream = null;

            try
            {
                stream = info.Create();
                serializer.Serialize(stream, conduit.Settings);
                stream.Close();
                stream = null;
            }
            catch
            {
                if(stream != null)
                    stream.Close();

                stream = null;
            }

            conduit.InitializeGlobalSettings();
          
        }

        private void menuItem7_Click(object sender, System.EventArgs e)
        {
        
        }

        
        private void menuItem6_Click(object sender, System.EventArgs e)
        {
            // Execute All Tests
            
        }

        private void outputBox_TextChanged(object sender, EventArgs e)
        {
            RichTextBox box = sender as RichTextBox;
            box.Focus();
            box.SelectionStart = box.TextLength;         
        }

        private void menuItem9_Click(object sender, System.EventArgs e)
        {
            OutputLine("Connecting to Application Server at " + conduit.Settings.SamoaIP + ':' + conduit.Settings.SamoaPort);
            
            conduit.ConnectServer.BeginInvoke(new System.AsyncCallback(ConnectDone), null);
        }

        private void menuItem17_Click(object sender, System.EventArgs e)
        {
            ResetNodes();
        }

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			this.Dispose(true);
		}
        
        private void menuItem10_Click(object sender, System.EventArgs e)
        {
		
        }

        private void menuItem11_Click(object sender, System.EventArgs e)
        {
		
        }

        private void menuItem12_Click(object sender, System.EventArgs e)
        {
		
        }

        #endregion 

		private void ResetMeter(int maximum)
		{
			this.progressBar1.Value = 0;
			this.progressBar1.Maximum = maximum;
		}

		private void AdvanceMeter(int value_)
		{
			this.progressBar1.Increment(value_);
		}

		private void MainForm_Load(object sender, System.EventArgs e)
		{
        
		}

		private void LoadDone()
		{
            // REFACTOR: this is not fully correct
            //UpdateAllGroupIcons(explorer.Nodes);

            explorer.ExpandAll();
		}

        private void UpdateAllGroupIcons(TreeNodeCollection nodes)
        {
//            foreach(TreeNode node in nodes)
//            {
//                // Is a group node.
//                if(node.Nodes.Count > 0)
//                {
//                    UpdateGroupIcon(node);
//                    UpdateAllGroupIcons(node.Nodes);
//                }
//            }
        }

		private void UpdateGroupIcon(TreeNode groupNode)
		{
//			bool allSuccess = true;
//
//			foreach(TreeNode testNode in groupNode.Nodes)
//			{
//				 failure
//				if(testNode.ImageIndex == Constants.previousTestFailure)
//				{
//					groupNode.ImageIndex = Constants.previousTestFailure;
//					groupNode.SelectedImageIndex = Constants.previousTestFailure;
//					return;
//				}
//				else if(testNode.ImageIndex == Constants.unknownTestSuccess)
//				{
//					allSuccess = false;
//				}
//			}
//
//			if(allSuccess)
//			{
//				groupNode.ImageIndex = Constants.previousTestSuccess;
//				groupNode.SelectedImageIndex = Constants.previousTestSuccess;
//			}
//			else
//			{
//				groupNode.ImageIndex = Constants.unknownTestSuccess;
//				groupNode.SelectedImageIndex = Constants.unknownTestSuccess;
//			}
		}

		private void UpdateTestSettings(string testName, string variableName, string variableValue)
		{
			try
			{
				if(conduit.TestSettings.tests != null)
				{
					foreach(testType test in conduit.TestSettings.tests)
					{
						if(testName == test.name)
						{
							if(test.variables != null)
							{
								foreach(variableType variable in test.variables)
								{
									if(variableName == variable.name)
									{
										variable.Value = variableValue;
										return;
									}
								}

								// Couldn't find variable, must create it.
								variableType[] newVariables = new variableType[test.variables.Length + 1];

								test.variables.CopyTo(newVariables, 0);

								variableType newVar = new variableType();
								newVar.name = variableName;
								newVar.Value = variableValue;
								newVariables[newVariables.Length - 1] = newVar;

								test.variables = newVariables;
								return;
							}
							else
							{
								// Couldn't find variable, must create it.
								variableType[] newVariables = new variableType[1];

								variableType newVar = new variableType();
								newVar.name = variableName;
								newVar.Value = variableValue;
								newVariables[0] = newVar;

								test.variables = newVariables;   
								return;
							}
						}
					}

					// Couldn't find test, must create it
					testType[] newTests = new testType[conduit.TestSettings.tests.Length + 1];

					conduit.TestSettings.tests.CopyTo(newTests, 0);

					testType newTest = new testType();
					newTest.success = false;
					newTest.name = testName;

					variableType[] newVariables2 = new variableType[1];

					variableType newVar2 = new variableType();
					newVar2.name = variableName;
					newVar2.Value = variableValue;
					newVariables2[0] = newVar2;
               
					newTest.variables = newVariables2;

					newTests[newTests.Length - 1] = newTest;

					conduit.TestSettings.tests = newTests;
					return;

				}
				else
				{
					// First test, must create it
					testType[] newTests = new testType[1];

					testType newTest = new testType();
					newTest.success = false;
					newTest.name = testName;

					variableType[] newVariables = new variableType[1];

					variableType newVar = new variableType();
					newVar.name = variableName;
					newVar.Value = variableValue;
					newVariables[0] = newVar;
               
					newTest.variables = newVariables;

					newTests[0] = newTest;

					conduit.TestSettings.tests = newTests;
					return;
				}
			}
			finally
			{
				// Save TestSettings
				SaveTestSettings(conduit.Settings.TestSettings);
			}
		}

		private void SaveTestSettings(object testSettingsFileName)
		{
			System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(TestSettings));

			FileStream stream = null;

			lock(testSettingsFileLock)
			{
				try
				{
					FileInfo fileInfo = new FileInfo(conduit.TestSettingsFolder + "\\" + testSettingsFileName as string);
					stream = fileInfo.Open(FileMode.Create);
					serializer.Serialize(stream, conduit.TestSettings);
				}
				catch
				{
					OutputLine("Unable to save recently changed settings.");
				}
				finally
				{
					if(stream != null)
					{
						stream.Close();
					}
				}
			}
		}

		private void ResetNodes()
		{
			foreach(TreeNode groupNode in masterTreeNodeCollection)
			{
				groupNode.ImageIndex = 2;
				groupNode.SelectedImageIndex = 2;

				foreach(TreeNode testNode in groupNode.Nodes)
				{
					testNode.ImageIndex = 2;
					testNode.SelectedImageIndex = 2;

					object[] obj = testNode.Tag as object[];
					ArrayList inputs = obj[0] as ArrayList;

					foreach(TestInputDataBase input in inputs)
					{
						input.SavedDefaultValue = "";
					}
				}
			}
		}

		private void AbortTest(string testName)
		{
			conduit.AbortTest(testName);
		}

        private void TestIsAbortable()
        {
            button1.Enabled = true;
        }

        #region Miscellanous Refactored Clutter

        private int InitializeImage(bool firstTime, bool previousSuccess)
        {
            if(firstTime)
            {
                return Constants.unknownTestSuccess;
            }
            else if(previousSuccess)
            {
                return Constants.previousTestSuccess;
            }
            else
            {
                return Constants.previousTestFailure;
            }
        }

        #endregion
    }
}
