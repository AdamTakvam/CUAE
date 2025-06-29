using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Metreos.Samoa.FunctionalTestFramework;

namespace Metreos.Samoa.FunctionalTestRuntime
{
	/// <summary>
	/// Summary description for SamoaSettings.
	/// </summary>
	public class SamoaSettings : System.Windows.Forms.Form
	{
        public bool Changed{ get{ return changed; }}

		private string settingsPath;
        private Settings settings;

        private bool changed;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox samoaIp;
        private System.Windows.Forms.TextBox samoaPort;
        private System.Windows.Forms.TextBox testPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox frameWorkDir;
        private System.Windows.Forms.Label frameWorkDirLabel;
        private System.Windows.Forms.TextBox testDllDir;
        private System.Windows.Forms.Label testDllDirLabel;
        private System.Windows.Forms.TextBox pollTimes;
        private System.Windows.Forms.Label pollTimesLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox compiledTestDir;
        private System.Windows.Forms.TextBox phoneStartRange;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox phoneEndRange;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox callManagerIp;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RadioButton neverUninstall;
        private System.Windows.Forms.RadioButton alwaysUninstall;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SamoaSettings(Settings settings, string settingsPath)
		{
			this.settings = settings;
			this.settingsPath = settingsPath;
			
			InitializeComponent();

			LoadSettings();

            changed = false;
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
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.samoaIp = new System.Windows.Forms.TextBox();
            this.samoaPort = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.testPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.frameWorkDir = new System.Windows.Forms.TextBox();
            this.frameWorkDirLabel = new System.Windows.Forms.Label();
            this.testDllDir = new System.Windows.Forms.TextBox();
            this.testDllDirLabel = new System.Windows.Forms.Label();
            this.pollTimes = new System.Windows.Forms.TextBox();
            this.pollTimesLabel = new System.Windows.Forms.Label();
            this.compiledTestDir = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.phoneStartRange = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.phoneEndRange = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.callManagerIp = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.neverUninstall = new System.Windows.Forms.RadioButton();
            this.alwaysUninstall = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 8);
            this.label1.Name = "label1";
            this.label1.TabIndex = 0;
            this.label1.Text = "Samoa IP (delim ;)";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(0, 32);
            this.label2.Name = "label2";
            this.label2.TabIndex = 1;
            this.label2.Text = "OAM Port";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(40, 360);
            this.button1.Name = "button1";
            this.button1.TabIndex = 12;
            this.button1.Text = "Save";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // samoaIp
            // 
            this.samoaIp.Location = new System.Drawing.Point(120, 8);
            this.samoaIp.Name = "samoaIp";
            this.samoaIp.Size = new System.Drawing.Size(128, 20);
            this.samoaIp.TabIndex = 2;
            this.samoaIp.Text = "255.255.255.255";
            this.samoaIp.TextChanged += new System.EventHandler(this.Any_TextChanged);
            // 
            // samoaPort
            // 
            this.samoaPort.Location = new System.Drawing.Point(120, 32);
            this.samoaPort.Name = "samoaPort";
            this.samoaPort.Size = new System.Drawing.Size(128, 20);
            this.samoaPort.TabIndex = 3;
            this.samoaPort.Text = "4280";
            this.samoaPort.TextChanged += new System.EventHandler(this.Any_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(128, 360);
            this.button2.Name = "button2";
            this.button2.TabIndex = 13;
            this.button2.Text = "Cancel";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // testPort
            // 
            this.testPort.Location = new System.Drawing.Point(120, 56);
            this.testPort.Name = "testPort";
            this.testPort.Size = new System.Drawing.Size(128, 20);
            this.testPort.TabIndex = 4;
            this.testPort.Text = "8998";
            this.testPort.TextChanged += new System.EventHandler(this.Any_TextChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(0, 56);
            this.label3.Name = "label3";
            this.label3.TabIndex = 10;
            this.label3.Text = "Test Port";
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(120, 80);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(128, 20);
            this.username.TabIndex = 5;
            this.username.Text = "administrator";
            this.username.TextChanged += new System.EventHandler(this.Any_TextChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(0, 104);
            this.label5.Name = "label5";
            this.label5.TabIndex = 14;
            this.label5.Text = "MCE Password";
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(120, 104);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(128, 20);
            this.password.TabIndex = 6;
            this.password.Text = "password";
            this.password.TextChanged += new System.EventHandler(this.Any_TextChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(0, 80);
            this.label4.Name = "label4";
            this.label4.TabIndex = 12;
            this.label4.Text = "MCE Username";
            // 
            // frameWorkDir
            // 
            this.frameWorkDir.Location = new System.Drawing.Point(120, 128);
            this.frameWorkDir.Name = "frameWorkDir";
            this.frameWorkDir.Size = new System.Drawing.Size(128, 20);
            this.frameWorkDir.TabIndex = 7;
            this.frameWorkDir.Text = "x:\\build\\Framework\\1.0";
            this.frameWorkDir.TextChanged += new System.EventHandler(this.Any_TextChanged);
            // 
            // frameWorkDirLabel
            // 
            this.frameWorkDirLabel.Location = new System.Drawing.Point(0, 128);
            this.frameWorkDirLabel.Name = "frameWorkDirLabel";
            this.frameWorkDirLabel.Size = new System.Drawing.Size(120, 23);
            this.frameWorkDirLabel.TabIndex = 19;
            this.frameWorkDirLabel.Text = "Framework Directory";
            // 
            // testDllDir
            // 
            this.testDllDir.Location = new System.Drawing.Point(120, 152);
            this.testDllDir.Name = "testDllDir";
            this.testDllDir.Size = new System.Drawing.Size(128, 20);
            this.testDllDir.TabIndex = 9;
            this.testDllDir.Text = "x:\\Build\\FunctionalTest\\FunctionalTests";
            this.testDllDir.TextChanged += new System.EventHandler(this.Any_TextChanged);
            // 
            // testDllDirLabel
            // 
            this.testDllDirLabel.Location = new System.Drawing.Point(0, 152);
            this.testDllDirLabel.Name = "testDllDirLabel";
            this.testDllDirLabel.Size = new System.Drawing.Size(120, 23);
            this.testDllDirLabel.TabIndex = 23;
            this.testDllDirLabel.Text = "Test Dlls Directory";
            // 
            // pollTimes
            // 
            this.pollTimes.Location = new System.Drawing.Point(120, 176);
            this.pollTimes.Name = "pollTimes";
            this.pollTimes.Size = new System.Drawing.Size(128, 20);
            this.pollTimes.TabIndex = 10;
            this.pollTimes.Text = "60";
            this.pollTimes.TextChanged += new System.EventHandler(this.Any_TextChanged);
            // 
            // pollTimesLabel
            // 
            this.pollTimesLabel.Location = new System.Drawing.Point(0, 176);
            this.pollTimesLabel.Name = "pollTimesLabel";
            this.pollTimesLabel.Size = new System.Drawing.Size(120, 23);
            this.pollTimesLabel.TabIndex = 23;
            this.pollTimesLabel.Text = "Poll # for Load";
            // 
            // compiledTestDir
            // 
            this.compiledTestDir.Location = new System.Drawing.Point(120, 200);
            this.compiledTestDir.Name = "compiledTestDir";
            this.compiledTestDir.Size = new System.Drawing.Size(128, 20);
            this.compiledTestDir.TabIndex = 11;
            this.compiledTestDir.Text = "x:\\build\\FunctionalTest\\CompiledTests";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(0, 200);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(128, 23);
            this.label7.TabIndex = 26;
            this.label7.Text = "Compiled Test Base Dir";
            // 
            // phoneStartRange
            // 
            this.phoneStartRange.Location = new System.Drawing.Point(120, 224);
            this.phoneStartRange.Name = "phoneStartRange";
            this.phoneStartRange.Size = new System.Drawing.Size(128, 20);
            this.phoneStartRange.TabIndex = 27;
            this.phoneStartRange.Text = "69000";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(0, 224);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(128, 23);
            this.label6.TabIndex = 28;
            this.label6.Text = "Phone Start Range";
            // 
            // phoneEndRange
            // 
            this.phoneEndRange.Location = new System.Drawing.Point(120, 248);
            this.phoneEndRange.Name = "phoneEndRange";
            this.phoneEndRange.Size = new System.Drawing.Size(128, 20);
            this.phoneEndRange.TabIndex = 29;
            this.phoneEndRange.Text = "69999";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(0, 248);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(128, 23);
            this.label8.TabIndex = 30;
            this.label8.Text = "Phone End Range";
            // 
            // callManagerIp
            // 
            this.callManagerIp.Location = new System.Drawing.Point(120, 272);
            this.callManagerIp.Name = "callManagerIp";
            this.callManagerIp.Size = new System.Drawing.Size(128, 20);
            this.callManagerIp.TabIndex = 31;
            this.callManagerIp.Text = "192.168.1.250";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(0, 272);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(128, 23);
            this.label9.TabIndex = 32;
            this.label9.Text = "Call Manager IP";
            // 
            // neverUninstall
            // 
            this.neverUninstall.Location = new System.Drawing.Point(64, 304);
            this.neverUninstall.Name = "neverUninstall";
            this.neverUninstall.Size = new System.Drawing.Size(176, 24);
            this.neverUninstall.TabIndex = 33;
            this.neverUninstall.Text = "Never Uninstall Applications";
            this.neverUninstall.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // alwaysUninstall
            // 
            this.alwaysUninstall.Location = new System.Drawing.Point(64, 328);
            this.alwaysUninstall.Name = "alwaysUninstall";
            this.alwaysUninstall.Size = new System.Drawing.Size(176, 24);
            this.alwaysUninstall.TabIndex = 34;
            this.alwaysUninstall.Text = "Always Uninstall Applications";
            this.alwaysUninstall.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SamoaSettings
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(248, 398);
            this.Controls.Add(this.alwaysUninstall);
            this.Controls.Add(this.neverUninstall);
            this.Controls.Add(this.callManagerIp);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.phoneEndRange);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.phoneStartRange);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.compiledTestDir);
            this.Controls.Add(this.pollTimes);
            this.Controls.Add(this.testDllDir);
            this.Controls.Add(this.frameWorkDir);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.testPort);
            this.Controls.Add(this.samoaPort);
            this.Controls.Add(this.samoaIp);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pollTimesLabel);
            this.Controls.Add(this.testDllDirLabel);
            this.Controls.Add(this.frameWorkDirLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SamoaSettings";
            this.Text = "Samoa IP (delim ;)";
            this.ResumeLayout(false);

        }
		#endregion

        public void LoadSettings()
        {
            samoaIp.Text = settings.AppServerIps != null ? String.Join(";", settings.AppServerIps) : "";
            samoaPort.Text = settings.SamoaPort;
            testPort.Text = settings.TestPort;
            username.Text = settings.Username;
            password.Text = settings.Password;
            testDllDir.Text = settings.DllFolder;
            pollTimes.Text = settings.PollTimes;
            frameWorkDir.Text = settings.FrameworkDir;
            compiledTestDir.Text = settings.CompiledMaxTestsDir; 
            phoneStartRange.Text = settings.PhoneStartRange.ToString();
            phoneEndRange.Text = settings.PhoneEndRange.ToString();
            callManagerIp.Text = settings.CallManagerIp;
            neverUninstall.Checked = settings.NeverUninstallExistingApps;
        }

        private void Save()
        {     
            string samoaIpText = samoaIp.Text;
            string[] appServerIps = samoaIpText.Split(new char[] {';'});
            settings.AppServerIps = appServerIps;
            settings.SamoaPort = samoaPort.Text;
            settings.TestPort = testPort.Text;
            settings.Username = username.Text;
            settings.Password = password.Text;   
            settings.DllFolder = testDllDir.Text;
            settings.PollTimes = pollTimes.Text;
            settings.FrameworkDir = frameWorkDir.Text;
            settings.CompiledMaxTestsDir = compiledTestDir.Text;
            settings.CallManagerIp = callManagerIp.Text;
            settings.PhoneStartRange = int.Parse(phoneStartRange.Text);
            settings.PhoneEndRange = int.Parse(phoneEndRange.Text);
            settings.NeverUninstallExistingApps = neverUninstall.Checked;
            FileInfo info = new FileInfo(settingsPath + Path.DirectorySeparatorChar + "Settings.txt");

            if(info.Exists)
                info.Delete();
            
            FileStream stream = info.Create();
            
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Settings));

            serializer.Serialize(stream,  settings);

            stream.Close();
            stream = null;            
            info = null;
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            this.Dispose(true);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Save();

            Dispose(true);
        }

        private void Any_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }
    }
}
