using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Data;

using Metreos.Utilities;

namespace Metreos.ClickToTalk.StandaloneClient
{
	/// <summary>
	///     Main client form
	/// </summary>
	public class StandaloneClient : System.Windows.Forms.Form
	{
        private const string configPath = "c2tconfig.xml";

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.ListBox toCall;
        private System.Windows.Forms.TextBox newNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button callButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.CheckBox recordCall;
        private System.Windows.Forms.StatusBar statusBar1;
		private System.ComponentModel.Container components = null;

		public StandaloneClient()
		{
			InitializeComponent();

			InitializeForm();
        }

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.toCall = new System.Windows.Forms.ListBox();
            this.newNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.recordCall = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.clearButton = new System.Windows.Forms.Button();
            this.callButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                      this.menuItem2,
                                                                                      this.menuItem1});
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                      this.menuItem3});
            this.menuItem2.Text = "File";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 0;
            this.menuItem3.Text = "Close";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                      this.menuItem4});
            this.menuItem1.Text = "Options";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 0;
            this.menuItem4.Text = "Settings";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // toCall
            // 
            this.toCall.Location = new System.Drawing.Point(24, 96);
            this.toCall.Name = "toCall";
            this.toCall.Size = new System.Drawing.Size(208, 95);
            this.toCall.TabIndex = 0;
            this.toCall.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toCall_KeyDown);
            this.toCall.SelectedIndexChanged += new System.EventHandler(this.toCall_SelectedIndexChanged);
            // 
            // newNumber
            // 
            this.newNumber.Location = new System.Drawing.Point(24, 48);
            this.newNumber.Name = "newNumber";
            this.newNumber.Size = new System.Drawing.Size(208, 20);
            this.newNumber.TabIndex = 1;
            this.newNumber.Text = "[Enter Number]";
            this.newNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.newNumber_KeyDown);
            this.newNumber.Click += new System.EventHandler(this.newNumber_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Enter a new number to call, and press enter";
            // 
            // recordCall
            // 
            this.recordCall.Location = new System.Drawing.Point(24, 24);
            this.recordCall.Name = "recordCall";
            this.recordCall.TabIndex = 3;
            this.recordCall.Text = "Record this Call";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.clearButton);
            this.groupBox1.Controls.Add(this.callButton);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.toCall);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.newNumber);
            this.groupBox1.Location = new System.Drawing.Point(16, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(256, 232);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Define Call Group";
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(160, 200);
            this.clearButton.Name = "clearButton";
            this.clearButton.TabIndex = 5;
            this.clearButton.Text = "Clear Group";
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // callButton
            // 
            this.callButton.Location = new System.Drawing.Point(24, 200);
            this.callButton.Name = "callButton";
            this.callButton.TabIndex = 4;
            this.callButton.Text = "Call";
            this.callButton.Click += new System.EventHandler(this.callButton_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 72);
            this.label2.Name = "label2";
            this.label2.TabIndex = 3;
            this.label2.Text = "Numbers to Call";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.recordCall);
            this.groupBox2.Location = new System.Drawing.Point(16, 248);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(256, 56);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Call Options";
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 307);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(288, 22);
            this.statusBar1.TabIndex = 6;
            this.statusBar1.Text = "Ready";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(288, 329);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "Click-To-Talk Client";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		[STAThread]
		static void Main() 
		{
			Application.Run(new StandaloneClient());
		}

        private void InitializeForm()
        {
            SerializedConfiguration config = LoadConfig();
        
            if(config == null)
            {
                callButton.Enabled = false;
                statusBar1.Text =  "Configure the client first. Go to Options -> Settings";
                return;
            }
            else
            {
                callButton.Enabled = true;
                statusBar1.Text = "Ready";
            }

            if(config.alwaysRecord)
            {
                recordCall.Enabled = false;
                recordCall.Checked = true;
            }
            else
            {
                recordCall.Enabled = true;
                recordCall.Checked = false;
            }
        }

        private void menuItem3_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void menuItem4_Click(object sender, System.EventArgs e)
        {
            Configuration configurationDialog = new Configuration();
            SerializedConfiguration config = LoadConfig();

            if(config != null)
            {
                configurationDialog.Username = config.username;
                configurationDialog.Password = config.password;
                configurationDialog.Email    = config.email;
                configurationDialog.ApplicationServerIp = config.applicationServerIp;
                configurationDialog.ApplicationServerPort = config.applicationServerPort;
                configurationDialog.AlwaysRecord = config.alwaysRecord;
            }

            configurationDialog.ShowDialog(this);

            switch(configurationDialog.DialogResult)
            {
                case DialogResult.OK:
                    SaveConfig(
                        configurationDialog.Username,
                        configurationDialog.Password,
                        configurationDialog.Email,
                        configurationDialog.ApplicationServerIp,
                        configurationDialog.ApplicationServerPort,
                        configurationDialog.AlwaysRecord);

                    InitializeForm();
                    break;

                case DialogResult.Cancel:
                    break;
            }
        }

        private void SaveConfig(
            string username,
            string password,
            string email,
            string appServerIp,
            int    appServerPort,
            bool   alwaysRecord)
        {
            SerializedConfiguration config = new SerializedConfiguration();
            config.username = username;
            config.password = password;
            config.email    = email;
            config.applicationServerIp = appServerIp;
            config.applicationServerPort = appServerPort;
            config.alwaysRecord = alwaysRecord;

            FileStream configFile = File.Open(configPath, FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(SerializedConfiguration));
            serializer.Serialize(configFile, config);
            configFile.Close();
        }

        private SerializedConfiguration LoadConfig()
        {
            if(File.Exists(configPath))
            {
                FileStream stream = null;
                try
                {
                    stream = File.Open(configPath, FileMode.Open);
                    XmlSerializer deserializer = new XmlSerializer(typeof(SerializedConfiguration));
                    SerializedConfiguration config = deserializer.Deserialize(stream) as SerializedConfiguration;
                    return config;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    if(stream != null)
                    {
                        stream.Close();
                    }
                }
            }
            else
            {
                return null;
            }
        }

        private void newNumber_Click(object sender, EventArgs e)
        {
            newNumber.Text = String.Empty;
        }

        private void newNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                string additionalNumber = newNumber.Text;
                if(additionalNumber == String.Empty || additionalNumber == null)
                {
                    return;
                }

                toCall.Items.Add(newNumber.Text);
                newNumber.Text = String.Empty;
            }
        }

        private void clearButton_Click(object sender, System.EventArgs e)
        {
            toCall.Items.Clear();
        }

        private void toCall_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        
        }

        private void toCall_KeyDown(object sender, KeyEventArgs e)
        {
            if(toCall.SelectedItem == null) return;

            if(e.KeyCode == Keys.Delete)
            {
                toCall.SelectedIndex = -1;
                toCall.Items.Remove(toCall.SelectedItem);
            }
        }

        public calleeType[] GetAllToCall()
        {
            ArrayList toCallItems = new ArrayList();
            int i = 0;
            foreach(string item in toCall.Items)
            {
                calleeType newCallee = new calleeType();
                newCallee.name = String.Format("Callee {0}", i++);
                newCallee.Value = item;
                toCallItems.Add(newCallee);
            }

            return (calleeType[]) toCallItems.ToArray(typeof(calleeType));
        }



        private void callButton_Click(object sender, System.EventArgs e)
        {
            SerializedConfiguration config = LoadConfig();

            if(config == null)
            {
                statusBar1.Text = "Not configured";
                return;
            }

            string initiateUrl = String.Format(
                "http://{0}:{1}/click-to-talk/initiateCall", 
                config.applicationServerIp, 
                config.applicationServerPort);

            initCallType call = new initCallType();
            call.callee = GetAllToCall();

            if(call.callee.Length == 0)
            {
                statusBar1.Text = "No phone numbers defined in the call group";
                return;
            }

            call.email = config.email;
            call.password = config.password;
            call.username = config.username;
            call.record = config.alwaysRecord || recordCall.Checked;
            
            string response;
            UrlStatus status = Web.UpXmlDownStringTransaction(
                initiateUrl, 
                call,  
                out response); 

            if(status == UrlStatus.CommunicationError || response == "error")
            {
                statusBar1.Text = "Unable to initiate the call(s)";
            }
            else if(status != UrlStatus.Success)
            {
                statusBar1.Text = "Unable to communicate with the Application Server";
            }
            else
            {
                statusBar1.Text = "Call(s) initiated";
            }
        }
    }
}
