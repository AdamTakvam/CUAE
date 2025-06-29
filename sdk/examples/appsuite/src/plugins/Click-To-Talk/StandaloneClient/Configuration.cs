using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization;

using Metreos.Utilities;

namespace Metreos.ClickToTalk.StandaloneClient
{
	/// <summary>
	///     Alters configuration settings for the test client
	/// </summary>
	public class Configuration : System.Windows.Forms.Form
	{   
        public  string Username { get { return username.Text; } set { username.Text = value; } }
        public  string Password { get { return password.Text; } set { password.Text = value; } }
        public  string Email { get { return userEmailAddress.Text; } set { userEmailAddress.Text = value; } }
        public  bool   AlwaysRecord { get { return alwaysRecordCalls.Checked; } set { alwaysRecordCalls.Checked = value; } }
        public  string ApplicationServerIp { get { return applicationServerIp.Text; } set { applicationServerIp.Text = value; } }
        public  int    ApplicationServerPort { get { return int.Parse(applicationServerPort.Text); } set { applicationServerPort.Text = value.ToString(); } }

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox applicationServerIp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox applicationServerPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox userEmailAddress;
        private System.Windows.Forms.CheckBox alwaysRecordCalls;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button validateButton;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.StatusBar statusBar1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Configuration()
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.alwaysRecordCalls = new System.Windows.Forms.CheckBox();
            this.userEmailAddress = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.applicationServerPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.applicationServerIp = new System.Windows.Forms.TextBox();
            this.validateButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.password);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.username);
            this.groupBox1.Controls.Add(this.alwaysRecordCalls);
            this.groupBox1.Controls.Add(this.userEmailAddress);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(16, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(336, 136);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Settings";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 50);
            this.label2.Name = "label2";
            this.label2.TabIndex = 3;
            this.label2.Text = "Password:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(192, 50);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(128, 20);
            this.password.TabIndex = 2;
            this.password.Text = "";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 20);
            this.label1.Name = "label1";
            this.label1.TabIndex = 1;
            this.label1.Text = "Username:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(192, 20);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(128, 20);
            this.username.TabIndex = 0;
            this.username.Text = "";
            // 
            // alwaysRecordCalls
            // 
            this.alwaysRecordCalls.Location = new System.Drawing.Point(192, 104);
            this.alwaysRecordCalls.Name = "alwaysRecordCalls";
            this.alwaysRecordCalls.Size = new System.Drawing.Size(136, 24);
            this.alwaysRecordCalls.TabIndex = 10;
            this.alwaysRecordCalls.Text = "Always Record Calls";
            // 
            // userEmailAddress
            // 
            this.userEmailAddress.Location = new System.Drawing.Point(192, 82);
            this.userEmailAddress.Name = "userEmailAddress";
            this.userEmailAddress.Size = new System.Drawing.Size(128, 20);
            this.userEmailAddress.TabIndex = 8;
            this.userEmailAddress.Text = "";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(160, 23);
            this.label5.TabIndex = 9;
            this.label5.Text = "Email Address:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.applicationServerPort);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.applicationServerIp);
            this.groupBox2.Controls.Add(this.validateButton);
            this.groupBox2.Location = new System.Drawing.Point(16, 168);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(336, 117);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Server Settings";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 23);
            this.label4.TabIndex = 7;
            this.label4.Text = "Application Server HTTP Port:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // applicationServerPort
            // 
            this.applicationServerPort.Location = new System.Drawing.Point(192, 56);
            this.applicationServerPort.Name = "applicationServerPort";
            this.applicationServerPort.Size = new System.Drawing.Size(128, 20);
            this.applicationServerPort.TabIndex = 6;
            this.applicationServerPort.Text = "8000";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Application Server IP:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // applicationServerIp
            // 
            this.applicationServerIp.Location = new System.Drawing.Point(192, 24);
            this.applicationServerIp.Name = "applicationServerIp";
            this.applicationServerIp.Size = new System.Drawing.Size(128, 20);
            this.applicationServerIp.TabIndex = 4;
            this.applicationServerIp.Text = "";
            // 
            // validateButton
            // 
            this.validateButton.Location = new System.Drawing.Point(246, 84);
            this.validateButton.Name = "validateButton";
            this.validateButton.TabIndex = 6;
            this.validateButton.Text = "Validate";
            this.validateButton.Click += new System.EventHandler(this.validateButton_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(192, 296);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(72, 23);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(280, 296);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(72, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 320);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(368, 22);
            this.statusBar1.TabIndex = 6;
            this.statusBar1.Text = "Ready";
            // 
            // Configuration
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(368, 342);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "Configuration";
            this.Text = "Configuration";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

        private void okButton_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void validateButton_Click(object sender, System.EventArgs e)
        {
            string validateUrl = String.Format(
                "http://{0}:{1}/click-to-talk/validate", applicationServerIp.Text, applicationServerPort.Text);
            
            initCallType validateData = new initCallType();

            validateData.username = username.Text;
            validateData.password = password.Text;
            
            statusBar1.Text = "Attempting to validate...";

            string response;
            UrlStatus status = Web.UpXmlDownStringTransaction(validateUrl, validateData, out response);

            if(status == UrlStatus.CommunicationError)
            {
                statusBar1.Text = "Username and/or password is incorrect";
            }
            else if(status != UrlStatus.Success)
            {
                statusBar1.Text = "Unable to communicate with the Application Server";
            }
            else
            {
                statusBar1.Text = "Valid";                    
            }
        }
	}
}
