using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Metreos.Utilities;
using Metreos.ApplicationSuite.Storage;
using Metreos.ApplicationSuite.Actions;
using ReturnValues = Metreos.ApplicationSuite.Storage.SessionRecords.WriteSessionStartResultValues;

namespace ApplicationSuiteTestTool
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class TestTool : System.Windows.Forms.Form
	{
        private IDbConnection connection;
        private Metreos.LoggingFramework.LogWriter log;
        private string applicationName = "testtool";
        private string partitionName = "not used";
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox loginAccountCode;
        private System.Windows.Forms.TextBox loginPin;
        private System.Windows.Forms.Button phoneValidate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label loginUserId;
        private System.Windows.Forms.Label AuthRecordId;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label loginAuthRecordId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox loginOriginNumber;
        private System.Windows.Forms.Label loginAuthResult;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox loginIp;
        private System.Windows.Forms.Label loginResult;
        private System.Windows.Forms.Label loginAuthRecordId2;
        private System.Windows.Forms.Label loginUserId2;
        private System.Windows.Forms.TextBox loginPassword;
        private System.Windows.Forms.TextBox loginUsername;
        private System.Windows.Forms.Button webLogin;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox deviceUserId;
        private System.Windows.Forms.Label UserId;
        private System.Windows.Forms.Button primaryDeviceButton;
        private System.Windows.Forms.Label devicePrimaryMac;
        private System.Windows.Forms.Button allDeviceButton;
        private System.Windows.Forms.ListBox devicesAll;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox callStartUserId;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox callStartSessionId;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox callStartTo;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox callEndRecordId;
        private System.Windows.Forms.Label CallRecordId;
        private System.Windows.Forms.TextBox callEndEndReason;
        private System.Windows.Forms.Label laoeu;
        private System.Windows.Forms.Button callStart;
        private System.Windows.Forms.Button callStop;
        private System.Windows.Forms.TextBox callStartFrom;
        private System.Windows.Forms.Label callStartResult;
        private System.Windows.Forms.Label callEndResult;
        private System.Windows.Forms.Label callStartRecordId;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox startSessionAuthRec;
        private System.Windows.Forms.Label startSessionRecordId;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Button startSessionButton;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox stopSessionRecordId;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Button stopSessionButton;
        private System.Windows.Forms.Label stopSessionResult;
        private System.Windows.Forms.TextBox callStartAuthRecordId;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView configsList;
        private System.Windows.Forms.ColumnHeader ConfigName;
        private System.Windows.Forms.ColumnHeader ConfigValue;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TextBox intercomUserIdTextBox;
        private System.Windows.Forms.ListView intercomResultsListView;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.TextBox intercomGroupsIdTextBox;
        private System.Windows.Forms.Label intercomGroupsIsEnabledLabel;
        private System.Windows.Forms.Label intercomGroupsIsPrivateLabel;
        private System.Windows.Forms.Label intercomGroupsIsTalkbackEnabledLabel;
        private System.Windows.Forms.Label intercomGroupsNameLabel;
        private System.Windows.Forms.Button RetrieveAllIntercomGroupsForUserButton;
        private System.Windows.Forms.Button RetrieveIntercomGroupButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TestTool()
		{
			InitializeComponent();

            connection = Database.CreateConnection(Database.DbType.mysql, Database.FormatDSN(
                "application_suite", 
                "localhost", 
                3306, 
                "root", 
                "metreos", 
                true));

            log = new Metreos.LoggingFramework.LogWriter(TraceLevel.Verbose, "testtool");

            connection.Open();

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
                connection.Close();
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
            this.loginAccountCode = new System.Windows.Forms.TextBox();
            this.loginPin = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.loginOriginNumber = new System.Windows.Forms.TextBox();
            this.loginAuthResult = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.loginAuthRecordId = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.AuthRecordId = new System.Windows.Forms.Label();
            this.loginUserId = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.phoneValidate = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.loginIp = new System.Windows.Forms.TextBox();
            this.loginResult = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.loginAuthRecordId2 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.loginUserId2 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.webLogin = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.loginPassword = new System.Windows.Forms.TextBox();
            this.loginUsername = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.devicesAll = new System.Windows.Forms.ListBox();
            this.allDeviceButton = new System.Windows.Forms.Button();
            this.devicePrimaryMac = new System.Windows.Forms.Label();
            this.primaryDeviceButton = new System.Windows.Forms.Button();
            this.UserId = new System.Windows.Forms.Label();
            this.deviceUserId = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label28 = new System.Windows.Forms.Label();
            this.callStartAuthRecordId = new System.Windows.Forms.TextBox();
            this.callStartRecordId = new System.Windows.Forms.Label();
            this.callEndResult = new System.Windows.Forms.Label();
            this.callStartResult = new System.Windows.Forms.Label();
            this.callStop = new System.Windows.Forms.Button();
            this.laoeu = new System.Windows.Forms.Label();
            this.callEndEndReason = new System.Windows.Forms.TextBox();
            this.CallRecordId = new System.Windows.Forms.Label();
            this.callEndRecordId = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.callStart = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.callStartFrom = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.callStartTo = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.callStartSessionId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.callStartUserId = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.stopSessionResult = new System.Windows.Forms.Label();
            this.stopSessionButton = new System.Windows.Forms.Button();
            this.label27 = new System.Windows.Forms.Label();
            this.stopSessionRecordId = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.startSessionButton = new System.Windows.Forms.Button();
            this.label26 = new System.Windows.Forms.Label();
            this.startSessionRecordId = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.startSessionAuthRec = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.configsList = new System.Windows.Forms.ListView();
            this.ConfigName = new System.Windows.Forms.ColumnHeader();
            this.ConfigValue = new System.Windows.Forms.ColumnHeader();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.RetrieveAllIntercomGroupsForUserButton = new System.Windows.Forms.Button();
            this.intercomResultsListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.intercomUserIdTextBox = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.intercomGroupsIdTextBox = new System.Windows.Forms.TextBox();
            this.intercomGroupsIsEnabledLabel = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.intercomGroupsNameLabel = new System.Windows.Forms.Label();
            this.intercomGroupsIsPrivateLabel = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.intercomGroupsIsTalkbackEnabledLabel = new System.Windows.Forms.Label();
            this.RetrieveIntercomGroupButton = new System.Windows.Forms.Button();
            this.label41 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // loginAccountCode
            // 
            this.loginAccountCode.Location = new System.Drawing.Point(8, 16);
            this.loginAccountCode.Name = "loginAccountCode";
            this.loginAccountCode.Size = new System.Drawing.Size(72, 20);
            this.loginAccountCode.TabIndex = 0;
            this.loginAccountCode.Text = "44";
            // 
            // loginPin
            // 
            this.loginPin.Location = new System.Drawing.Point(8, 40);
            this.loginPin.Name = "loginPin";
            this.loginPin.Size = new System.Drawing.Size(72, 20);
            this.loginPin.TabIndex = 1;
            this.loginPin.Text = "123";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.loginOriginNumber);
            this.groupBox1.Controls.Add(this.loginAuthResult);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.loginAuthRecordId);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.AuthRecordId);
            this.groupBox1.Controls.Add(this.loginUserId);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.phoneValidate);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.loginPin);
            this.groupBox1.Controls.Add(this.loginAccountCode);
            this.groupBox1.Location = new System.Drawing.Point(8, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 264);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Phone Login Authentication";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(80, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 23);
            this.label8.TabIndex = 14;
            this.label8.Text = "Directory Number";
            // 
            // loginOriginNumber
            // 
            this.loginOriginNumber.Location = new System.Drawing.Point(8, 64);
            this.loginOriginNumber.Name = "loginOriginNumber";
            this.loginOriginNumber.Size = new System.Drawing.Size(72, 20);
            this.loginOriginNumber.TabIndex = 13;
            this.loginOriginNumber.Text = "4774";
            // 
            // loginAuthResult
            // 
            this.loginAuthResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.loginAuthResult.Location = new System.Drawing.Point(8, 224);
            this.loginAuthResult.Name = "loginAuthResult";
            this.loginAuthResult.Size = new System.Drawing.Size(144, 23);
            this.loginAuthResult.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(160, 224);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 23);
            this.label6.TabIndex = 11;
            this.label6.Text = "Login Result";
            // 
            // loginAuthRecordId
            // 
            this.loginAuthRecordId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.loginAuthRecordId.Location = new System.Drawing.Point(8, 192);
            this.loginAuthRecordId.Name = "loginAuthRecordId";
            this.loginAuthRecordId.Size = new System.Drawing.Size(72, 23);
            this.loginAuthRecordId.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(88, 160);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 23);
            this.label7.TabIndex = 9;
            this.label7.Text = "UserId";
            // 
            // AuthRecordId
            // 
            this.AuthRecordId.Location = new System.Drawing.Point(88, 192);
            this.AuthRecordId.Name = "AuthRecordId";
            this.AuthRecordId.Size = new System.Drawing.Size(80, 23);
            this.AuthRecordId.TabIndex = 8;
            this.AuthRecordId.Text = "AuthRecordId";
            // 
            // loginUserId
            // 
            this.loginUserId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.loginUserId.Location = new System.Drawing.Point(8, 160);
            this.loginUserId.Name = "loginUserId";
            this.loginUserId.Size = new System.Drawing.Size(72, 23);
            this.loginUserId.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 136);
            this.label4.Name = "label4";
            this.label4.TabIndex = 6;
            this.label4.Text = "Results";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // phoneValidate
            // 
            this.phoneValidate.Location = new System.Drawing.Point(8, 104);
            this.phoneValidate.Name = "phoneValidate";
            this.phoneValidate.TabIndex = 5;
            this.phoneValidate.Text = "Validate";
            this.phoneValidate.Click += new System.EventHandler(this.phoneValidate_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(80, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "Pin";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(80, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "AccountCode";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.loginIp);
            this.groupBox2.Controls.Add(this.loginResult);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.loginAuthRecordId2);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.loginUserId2);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.webLogin);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.loginPassword);
            this.groupBox2.Controls.Add(this.loginUsername);
            this.groupBox2.Location = new System.Drawing.Point(272, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(256, 264);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Web Login Authentication";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(88, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 23);
            this.label1.TabIndex = 28;
            this.label1.Text = "IP Address";
            // 
            // loginIp
            // 
            this.loginIp.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.loginIp.Location = new System.Drawing.Point(8, 65);
            this.loginIp.Name = "loginIp";
            this.loginIp.Size = new System.Drawing.Size(80, 20);
            this.loginIp.TabIndex = 27;
            this.loginIp.Text = "192.168.1.150";
            // 
            // loginResult
            // 
            this.loginResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.loginResult.Location = new System.Drawing.Point(12, 225);
            this.loginResult.Name = "loginResult";
            this.loginResult.Size = new System.Drawing.Size(144, 23);
            this.loginResult.TabIndex = 26;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(164, 225);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 23);
            this.label9.TabIndex = 25;
            this.label9.Text = "Login Result";
            // 
            // loginAuthRecordId2
            // 
            this.loginAuthRecordId2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.loginAuthRecordId2.Location = new System.Drawing.Point(12, 193);
            this.loginAuthRecordId2.Name = "loginAuthRecordId2";
            this.loginAuthRecordId2.Size = new System.Drawing.Size(72, 23);
            this.loginAuthRecordId2.TabIndex = 24;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(92, 161);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 23);
            this.label11.TabIndex = 23;
            this.label11.Text = "UserId";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(92, 193);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 23);
            this.label12.TabIndex = 22;
            this.label12.Text = "AuthRecordId";
            // 
            // loginUserId2
            // 
            this.loginUserId2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.loginUserId2.Location = new System.Drawing.Point(12, 161);
            this.loginUserId2.Name = "loginUserId2";
            this.loginUserId2.Size = new System.Drawing.Size(72, 23);
            this.loginUserId2.TabIndex = 21;
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label14.Location = new System.Drawing.Point(12, 137);
            this.label14.Name = "label14";
            this.label14.TabIndex = 20;
            this.label14.Text = "Results";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // webLogin
            // 
            this.webLogin.Location = new System.Drawing.Point(12, 105);
            this.webLogin.Name = "webLogin";
            this.webLogin.TabIndex = 19;
            this.webLogin.Text = "Validate";
            this.webLogin.Click += new System.EventHandler(this.webLogin_Click);
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(80, 41);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(64, 23);
            this.label15.TabIndex = 18;
            this.label15.Text = "Password";
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(80, 17);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(72, 23);
            this.label16.TabIndex = 17;
            this.label16.Text = "Username";
            // 
            // loginPassword
            // 
            this.loginPassword.Location = new System.Drawing.Point(8, 41);
            this.loginPassword.Name = "loginPassword";
            this.loginPassword.Size = new System.Drawing.Size(72, 20);
            this.loginPassword.TabIndex = 16;
            this.loginPassword.Text = "metreos";
            // 
            // loginUsername
            // 
            this.loginUsername.Location = new System.Drawing.Point(8, 17);
            this.loginUsername.Name = "loginUsername";
            this.loginUsername.Size = new System.Drawing.Size(72, 20);
            this.loginUsername.TabIndex = 15;
            this.loginUsername.Text = "achaney";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.devicesAll);
            this.groupBox3.Controls.Add(this.allDeviceButton);
            this.groupBox3.Controls.Add(this.devicePrimaryMac);
            this.groupBox3.Controls.Add(this.primaryDeviceButton);
            this.groupBox3.Controls.Add(this.UserId);
            this.groupBox3.Controls.Add(this.deviceUserId);
            this.groupBox3.Location = new System.Drawing.Point(8, 296);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(184, 216);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Device Association";
            // 
            // devicesAll
            // 
            this.devicesAll.Location = new System.Drawing.Point(8, 144);
            this.devicesAll.Name = "devicesAll";
            this.devicesAll.Size = new System.Drawing.Size(160, 56);
            this.devicesAll.TabIndex = 5;
            // 
            // allDeviceButton
            // 
            this.allDeviceButton.Location = new System.Drawing.Point(8, 112);
            this.allDeviceButton.Name = "allDeviceButton";
            this.allDeviceButton.Size = new System.Drawing.Size(160, 23);
            this.allDeviceButton.TabIndex = 4;
            this.allDeviceButton.Text = "Retrieve All Devices";
            this.allDeviceButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // devicePrimaryMac
            // 
            this.devicePrimaryMac.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.devicePrimaryMac.Location = new System.Drawing.Point(8, 80);
            this.devicePrimaryMac.Name = "devicePrimaryMac";
            this.devicePrimaryMac.Size = new System.Drawing.Size(160, 23);
            this.devicePrimaryMac.TabIndex = 3;
            // 
            // primaryDeviceButton
            // 
            this.primaryDeviceButton.Location = new System.Drawing.Point(8, 48);
            this.primaryDeviceButton.Name = "primaryDeviceButton";
            this.primaryDeviceButton.Size = new System.Drawing.Size(160, 23);
            this.primaryDeviceButton.TabIndex = 2;
            this.primaryDeviceButton.Text = "Retrieve Primary Device";
            this.primaryDeviceButton.Click += new System.EventHandler(this.primaryDeviceButton_Click);
            // 
            // UserId
            // 
            this.UserId.Location = new System.Drawing.Point(64, 24);
            this.UserId.Name = "UserId";
            this.UserId.Size = new System.Drawing.Size(40, 23);
            this.UserId.TabIndex = 1;
            this.UserId.Text = "UserId";
            // 
            // deviceUserId
            // 
            this.deviceUserId.Location = new System.Drawing.Point(8, 24);
            this.deviceUserId.Name = "deviceUserId";
            this.deviceUserId.Size = new System.Drawing.Size(48, 20);
            this.deviceUserId.TabIndex = 0;
            this.deviceUserId.Text = "2";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label28);
            this.groupBox4.Controls.Add(this.callStartAuthRecordId);
            this.groupBox4.Controls.Add(this.callStartRecordId);
            this.groupBox4.Controls.Add(this.callEndResult);
            this.groupBox4.Controls.Add(this.callStartResult);
            this.groupBox4.Controls.Add(this.callStop);
            this.groupBox4.Controls.Add(this.laoeu);
            this.groupBox4.Controls.Add(this.callEndEndReason);
            this.groupBox4.Controls.Add(this.CallRecordId);
            this.groupBox4.Controls.Add(this.callEndRecordId);
            this.groupBox4.Controls.Add(this.label21);
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Controls.Add(this.callStart);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.callStartFrom);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.callStartTo);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.callStartSessionId);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.callStartUserId);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Location = new System.Drawing.Point(208, 296);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(232, 384);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Call Record Start/Stop";
            this.groupBox4.Enter += new System.EventHandler(this.groupBox4_Enter);
            // 
            // label28
            // 
            this.label28.Location = new System.Drawing.Point(80, 144);
            this.label28.Name = "label28";
            this.label28.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label28.Size = new System.Drawing.Size(136, 23);
            this.label28.TabIndex = 42;
            this.label28.Text = "Auth Rec Id (0 for none)";
            // 
            // callStartAuthRecordId
            // 
            this.callStartAuthRecordId.Location = new System.Drawing.Point(8, 144);
            this.callStartAuthRecordId.Name = "callStartAuthRecordId";
            this.callStartAuthRecordId.Size = new System.Drawing.Size(72, 20);
            this.callStartAuthRecordId.TabIndex = 41;
            this.callStartAuthRecordId.Text = "0";
            // 
            // callStartRecordId
            // 
            this.callStartRecordId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.callStartRecordId.Location = new System.Drawing.Point(8, 200);
            this.callStartRecordId.Name = "callStartRecordId";
            this.callStartRecordId.Size = new System.Drawing.Size(72, 23);
            this.callStartRecordId.TabIndex = 40;
            // 
            // callEndResult
            // 
            this.callEndResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.callEndResult.Location = new System.Drawing.Point(144, 272);
            this.callEndResult.Name = "callEndResult";
            this.callEndResult.Size = new System.Drawing.Size(80, 23);
            this.callEndResult.TabIndex = 39;
            // 
            // callStartResult
            // 
            this.callStartResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.callStartResult.Location = new System.Drawing.Point(136, 24);
            this.callStartResult.Name = "callStartResult";
            this.callStartResult.Size = new System.Drawing.Size(80, 23);
            this.callStartResult.TabIndex = 38;
            // 
            // callStop
            // 
            this.callStop.Location = new System.Drawing.Point(8, 352);
            this.callStop.Name = "callStop";
            this.callStop.Size = new System.Drawing.Size(136, 23);
            this.callStop.TabIndex = 37;
            this.callStop.Text = "Stop Call Record";
            this.callStop.Click += new System.EventHandler(this.callStop_Click);
            // 
            // laoeu
            // 
            this.laoeu.Location = new System.Drawing.Point(112, 328);
            this.laoeu.Name = "laoeu";
            this.laoeu.Size = new System.Drawing.Size(104, 32);
            this.laoeu.TabIndex = 36;
            this.laoeu.Text = "End Reason (1, 2, 4, 8, 16, 65536 )";
            // 
            // callEndEndReason
            // 
            this.callEndEndReason.Location = new System.Drawing.Point(8, 328);
            this.callEndEndReason.Name = "callEndEndReason";
            this.callEndEndReason.TabIndex = 35;
            this.callEndEndReason.Text = "1";
            // 
            // CallRecordId
            // 
            this.CallRecordId.Location = new System.Drawing.Point(112, 304);
            this.CallRecordId.Name = "CallRecordId";
            this.CallRecordId.Size = new System.Drawing.Size(88, 23);
            this.CallRecordId.TabIndex = 34;
            this.CallRecordId.Text = "Call Record Id";
            // 
            // callEndRecordId
            // 
            this.callEndRecordId.Location = new System.Drawing.Point(8, 304);
            this.callEndRecordId.Name = "callEndRecordId";
            this.callEndRecordId.TabIndex = 33;
            this.callEndRecordId.Text = "";
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(8, 272);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(136, 23);
            this.label21.TabIndex = 32;
            this.label21.Text = "Write Call Record End";
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(16, 24);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(136, 23);
            this.label20.TabIndex = 31;
            this.label20.Text = "Write Call Record Start";
            // 
            // callStart
            // 
            this.callStart.Location = new System.Drawing.Point(8, 232);
            this.callStart.Name = "callStart";
            this.callStart.Size = new System.Drawing.Size(136, 23);
            this.callStart.TabIndex = 30;
            this.callStart.Text = "Start Call Record";
            this.callStart.Click += new System.EventHandler(this.callStart_Click);
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(80, 120);
            this.label18.Name = "label18";
            this.label18.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label18.Size = new System.Drawing.Size(136, 23);
            this.label18.TabIndex = 27;
            this.label18.Text = "From";
            // 
            // callStartFrom
            // 
            this.callStartFrom.Location = new System.Drawing.Point(8, 120);
            this.callStartFrom.Name = "callStartFrom";
            this.callStartFrom.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.callStartFrom.Size = new System.Drawing.Size(72, 20);
            this.callStartFrom.TabIndex = 26;
            this.callStartFrom.Text = "8000";
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(80, 200);
            this.label17.Name = "label17";
            this.label17.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label17.Size = new System.Drawing.Size(104, 23);
            this.label17.TabIndex = 25;
            this.label17.Text = "Call Record Id";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(80, 96);
            this.label13.Name = "label13";
            this.label13.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label13.Size = new System.Drawing.Size(136, 23);
            this.label13.TabIndex = 23;
            this.label13.Text = "To";
            // 
            // callStartTo
            // 
            this.callStartTo.Location = new System.Drawing.Point(8, 96);
            this.callStartTo.Name = "callStartTo";
            this.callStartTo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.callStartTo.Size = new System.Drawing.Size(72, 20);
            this.callStartTo.TabIndex = 22;
            this.callStartTo.Text = "6000";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(80, 72);
            this.label10.Name = "label10";
            this.label10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label10.Size = new System.Drawing.Size(136, 23);
            this.label10.TabIndex = 21;
            this.label10.Text = "SessionId ( 0 for none)";
            // 
            // callStartSessionId
            // 
            this.callStartSessionId.Location = new System.Drawing.Point(8, 72);
            this.callStartSessionId.Name = "callStartSessionId";
            this.callStartSessionId.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.callStartSessionId.Size = new System.Drawing.Size(72, 20);
            this.callStartSessionId.TabIndex = 20;
            this.callStartSessionId.Text = "0";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(80, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 23);
            this.label5.TabIndex = 19;
            this.label5.Text = "UserId";
            // 
            // callStartUserId
            // 
            this.callStartUserId.Location = new System.Drawing.Point(8, 48);
            this.callStartUserId.Name = "callStartUserId";
            this.callStartUserId.Size = new System.Drawing.Size(72, 20);
            this.callStartUserId.TabIndex = 18;
            this.callStartUserId.Text = "1";
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label19.Location = new System.Drawing.Point(8, 176);
            this.label19.Name = "label19";
            this.label19.TabIndex = 29;
            this.label19.Text = "Results";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.stopSessionResult);
            this.groupBox5.Controls.Add(this.stopSessionButton);
            this.groupBox5.Controls.Add(this.label27);
            this.groupBox5.Controls.Add(this.stopSessionRecordId);
            this.groupBox5.Controls.Add(this.label24);
            this.groupBox5.Controls.Add(this.startSessionButton);
            this.groupBox5.Controls.Add(this.label26);
            this.groupBox5.Controls.Add(this.startSessionRecordId);
            this.groupBox5.Controls.Add(this.label22);
            this.groupBox5.Controls.Add(this.label23);
            this.groupBox5.Controls.Add(this.startSessionAuthRec);
            this.groupBox5.Controls.Add(this.label25);
            this.groupBox5.Location = new System.Drawing.Point(456, 296);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(232, 352);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Session Record Start/Stop";
            // 
            // stopSessionResult
            // 
            this.stopSessionResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.stopSessionResult.Location = new System.Drawing.Point(16, 264);
            this.stopSessionResult.Name = "stopSessionResult";
            this.stopSessionResult.TabIndex = 50;
            // 
            // stopSessionButton
            // 
            this.stopSessionButton.Location = new System.Drawing.Point(16, 232);
            this.stopSessionButton.Name = "stopSessionButton";
            this.stopSessionButton.Size = new System.Drawing.Size(128, 23);
            this.stopSessionButton.TabIndex = 49;
            this.stopSessionButton.Text = "Stop Session Record";
            this.stopSessionButton.Click += new System.EventHandler(this.stopSessionButton_Click);
            // 
            // label27
            // 
            this.label27.Location = new System.Drawing.Point(120, 200);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(104, 23);
            this.label27.TabIndex = 48;
            this.label27.Text = "Session Record Id";
            // 
            // stopSessionRecordId
            // 
            this.stopSessionRecordId.Location = new System.Drawing.Point(16, 200);
            this.stopSessionRecordId.Name = "stopSessionRecordId";
            this.stopSessionRecordId.TabIndex = 47;
            this.stopSessionRecordId.Text = "";
            // 
            // label24
            // 
            this.label24.Location = new System.Drawing.Point(16, 176);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(152, 23);
            this.label24.TabIndex = 46;
            this.label24.Text = "Write Session Record Stop";
            // 
            // startSessionButton
            // 
            this.startSessionButton.Location = new System.Drawing.Point(16, 136);
            this.startSessionButton.Name = "startSessionButton";
            this.startSessionButton.Size = new System.Drawing.Size(128, 23);
            this.startSessionButton.TabIndex = 45;
            this.startSessionButton.Text = "Start Session Record";
            this.startSessionButton.Click += new System.EventHandler(this.startSessionButton_Click);
            // 
            // label26
            // 
            this.label26.Location = new System.Drawing.Point(104, 104);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(72, 23);
            this.label26.TabIndex = 44;
            this.label26.Text = "Session Id";
            // 
            // startSessionRecordId
            // 
            this.startSessionRecordId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.startSessionRecordId.Location = new System.Drawing.Point(16, 104);
            this.startSessionRecordId.Name = "startSessionRecordId";
            this.startSessionRecordId.Size = new System.Drawing.Size(80, 23);
            this.startSessionRecordId.TabIndex = 43;
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(16, 24);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(152, 23);
            this.label22.TabIndex = 0;
            this.label22.Text = "Write Session Record Start";
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(88, 48);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(80, 23);
            this.label23.TabIndex = 42;
            this.label23.Text = "Auth Record Id";
            // 
            // startSessionAuthRec
            // 
            this.startSessionAuthRec.Location = new System.Drawing.Point(16, 48);
            this.startSessionAuthRec.Name = "startSessionAuthRec";
            this.startSessionAuthRec.Size = new System.Drawing.Size(72, 20);
            this.startSessionAuthRec.TabIndex = 41;
            this.startSessionAuthRec.Text = "1";
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label25.Location = new System.Drawing.Point(16, 72);
            this.label25.Name = "label25";
            this.label25.TabIndex = 41;
            this.label25.Text = "Results";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.configsList);
            this.groupBox6.Controls.Add(this.button1);
            this.groupBox6.Location = new System.Drawing.Point(536, 16);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(224, 264);
            this.groupBox6.TabIndex = 7;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Configuration Values";
            // 
            // configsList
            // 
            this.configsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                          this.ConfigName,
                                                                                          this.ConfigValue});
            this.configsList.Location = new System.Drawing.Point(8, 64);
            this.configsList.Name = "configsList";
            this.configsList.Size = new System.Drawing.Size(208, 192);
            this.configsList.TabIndex = 1;
            this.configsList.View = System.Windows.Forms.View.Details;
            // 
            // ConfigName
            // 
            this.ConfigName.Text = "Name";
            this.ConfigName.Width = 101;
            // 
            // ConfigValue
            // 
            this.ConfigValue.Text = "Value";
            this.ConfigValue.Width = 103;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(64, 24);
            this.button1.Name = "button1";
            this.button1.TabIndex = 0;
            this.button1.Text = "Retrieve All";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.RetrieveIntercomGroupButton);
            this.groupBox7.Controls.Add(this.intercomGroupsIsPrivateLabel);
            this.groupBox7.Controls.Add(this.label38);
            this.groupBox7.Controls.Add(this.label39);
            this.groupBox7.Controls.Add(this.intercomGroupsIsTalkbackEnabledLabel);
            this.groupBox7.Controls.Add(this.label30);
            this.groupBox7.Controls.Add(this.intercomGroupsIdTextBox);
            this.groupBox7.Controls.Add(this.label29);
            this.groupBox7.Controls.Add(this.RetrieveAllIntercomGroupsForUserButton);
            this.groupBox7.Controls.Add(this.intercomResultsListView);
            this.groupBox7.Controls.Add(this.label34);
            this.groupBox7.Controls.Add(this.label35);
            this.groupBox7.Controls.Add(this.intercomUserIdTextBox);
            this.groupBox7.Controls.Add(this.intercomGroupsIsEnabledLabel);
            this.groupBox7.Controls.Add(this.label32);
            this.groupBox7.Controls.Add(this.label33);
            this.groupBox7.Controls.Add(this.intercomGroupsNameLabel);
            this.groupBox7.Controls.Add(this.label41);
            this.groupBox7.Location = new System.Drawing.Point(704, 296);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(224, 392);
            this.groupBox7.TabIndex = 51;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Intercom Groups";
            // 
            // RetrieveAllIntercomGroupsForUserButton
            // 
            this.RetrieveAllIntercomGroupsForUserButton.Location = new System.Drawing.Point(16, 80);
            this.RetrieveAllIntercomGroupsForUserButton.Name = "RetrieveAllIntercomGroupsForUserButton";
            this.RetrieveAllIntercomGroupsForUserButton.TabIndex = 44;
            this.RetrieveAllIntercomGroupsForUserButton.Text = "Retrieve All";
            this.RetrieveAllIntercomGroupsForUserButton.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // intercomResultsListView
            // 
            this.intercomResultsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                                      this.columnHeader1});
            this.intercomResultsListView.Location = new System.Drawing.Point(112, 48);
            this.intercomResultsListView.Name = "intercomResultsListView";
            this.intercomResultsListView.Size = new System.Drawing.Size(96, 80);
            this.intercomResultsListView.TabIndex = 43;
            this.intercomResultsListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "IntercomGroupId";
            this.columnHeader1.Width = 91;
            // 
            // label34
            // 
            this.label34.Location = new System.Drawing.Point(16, 24);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(152, 23);
            this.label34.TabIndex = 0;
            this.label34.Text = "GetIntercomGroupsForUser";
            // 
            // label35
            // 
            this.label35.Location = new System.Drawing.Point(64, 48);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(40, 23);
            this.label35.TabIndex = 42;
            this.label35.Text = "UserId";
            // 
            // intercomUserIdTextBox
            // 
            this.intercomUserIdTextBox.Location = new System.Drawing.Point(16, 48);
            this.intercomUserIdTextBox.Name = "intercomUserIdTextBox";
            this.intercomUserIdTextBox.Size = new System.Drawing.Size(40, 20);
            this.intercomUserIdTextBox.TabIndex = 41;
            this.intercomUserIdTextBox.Text = "1";
            // 
            // label29
            // 
            this.label29.Location = new System.Drawing.Point(16, 136);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(152, 23);
            this.label29.TabIndex = 45;
            this.label29.Text = "GetIntercomGroup";
            // 
            // label30
            // 
            this.label30.Location = new System.Drawing.Point(64, 168);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(96, 23);
            this.label30.TabIndex = 47;
            this.label30.Text = "IntercomGroupsId";
            // 
            // intercomGroupsIdTextBox
            // 
            this.intercomGroupsIdTextBox.Location = new System.Drawing.Point(16, 168);
            this.intercomGroupsIdTextBox.Name = "intercomGroupsIdTextBox";
            this.intercomGroupsIdTextBox.Size = new System.Drawing.Size(40, 20);
            this.intercomGroupsIdTextBox.TabIndex = 46;
            this.intercomGroupsIdTextBox.Text = "1";
            // 
            // intercomGroupsIsEnabledLabel
            // 
            this.intercomGroupsIsEnabledLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.intercomGroupsIsEnabledLabel.Location = new System.Drawing.Point(16, 296);
            this.intercomGroupsIsEnabledLabel.Name = "intercomGroupsIsEnabledLabel";
            this.intercomGroupsIsEnabledLabel.Size = new System.Drawing.Size(72, 23);
            this.intercomGroupsIsEnabledLabel.TabIndex = 32;
            // 
            // label32
            // 
            this.label32.Location = new System.Drawing.Point(88, 264);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(72, 23);
            this.label32.TabIndex = 31;
            this.label32.Text = "Name";
            // 
            // label33
            // 
            this.label33.Location = new System.Drawing.Point(88, 296);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(80, 23);
            this.label33.TabIndex = 30;
            this.label33.Text = "IsEnabled";
            // 
            // intercomGroupsNameLabel
            // 
            this.intercomGroupsNameLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.intercomGroupsNameLabel.Location = new System.Drawing.Point(16, 264);
            this.intercomGroupsNameLabel.Name = "intercomGroupsNameLabel";
            this.intercomGroupsNameLabel.Size = new System.Drawing.Size(72, 23);
            this.intercomGroupsNameLabel.TabIndex = 29;
            // 
            // intercomGroupsIsPrivateLabel
            // 
            this.intercomGroupsIsPrivateLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.intercomGroupsIsPrivateLabel.Location = new System.Drawing.Point(16, 360);
            this.intercomGroupsIsPrivateLabel.Name = "intercomGroupsIsPrivateLabel";
            this.intercomGroupsIsPrivateLabel.Size = new System.Drawing.Size(72, 23);
            this.intercomGroupsIsPrivateLabel.TabIndex = 51;
            // 
            // label38
            // 
            this.label38.Location = new System.Drawing.Point(88, 328);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(104, 23);
            this.label38.TabIndex = 50;
            this.label38.Text = "IsTalkbackEnabled";
            // 
            // label39
            // 
            this.label39.Location = new System.Drawing.Point(88, 360);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(80, 23);
            this.label39.TabIndex = 49;
            this.label39.Text = "Private";
            // 
            // intercomGroupsIsTalkbackEnabledLabel
            // 
            this.intercomGroupsIsTalkbackEnabledLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.intercomGroupsIsTalkbackEnabledLabel.Location = new System.Drawing.Point(16, 328);
            this.intercomGroupsIsTalkbackEnabledLabel.Name = "intercomGroupsIsTalkbackEnabledLabel";
            this.intercomGroupsIsTalkbackEnabledLabel.Size = new System.Drawing.Size(72, 23);
            this.intercomGroupsIsTalkbackEnabledLabel.TabIndex = 48;
            // 
            // RetrieveIntercomGroupButton
            // 
            this.RetrieveIntercomGroupButton.Location = new System.Drawing.Point(16, 200);
            this.RetrieveIntercomGroupButton.Name = "RetrieveIntercomGroupButton";
            this.RetrieveIntercomGroupButton.TabIndex = 52;
            this.RetrieveIntercomGroupButton.Text = "Retrieve";
            this.RetrieveIntercomGroupButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // label41
            // 
            this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label41.Location = new System.Drawing.Point(16, 232);
            this.label41.Name = "label41";
            this.label41.TabIndex = 51;
            this.label41.Text = "Results";
            this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TestTool
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(984, 750);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox7);
            this.Name = "TestTool";
            this.Text = "Application Suite Database Test Tool";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new TestTool());
		}

        private void phoneValidate_Click(object sender, System.EventArgs e)
        {
            Users userDbAccess = new Users(connection, log, applicationName, partitionName, 
                DbTable.DetermineAllowWrite(new Hashtable()));
            
            uint userId;
            AuthenticationResult result;
            uint authRecordId;
            bool phoneChangeRequired;

            userDbAccess.ValidatePhoneLogin(
                int.Parse(loginAccountCode.Text), 
                int.Parse(loginPin.Text),
                loginOriginNumber.Text,
                out userId,
                out result,
                out authRecordId, 
                out phoneChangeRequired);

            loginUserId.Text = userId.ToString();
            loginAuthResult.Text = result.ToString();
            loginAuthRecordId.Text = authRecordId.ToString();
        }

        private void webLogin_Click(object sender, System.EventArgs e)
        {
            Users userDbAccess = new Users(connection, log, applicationName, partitionName, 
                true);
            
            uint userId;
            AuthenticationResult result;
            uint authRecordId;

            userDbAccess.ValidateWebLogin(
                loginUsername.Text, 
                loginPassword.Text, 
                loginIp.Text,
                out userId,
                out result,
                out authRecordId);

            loginUserId2.Text = userId.ToString();
            loginResult.Text = result.ToString();
            loginAuthRecordId2.Text = authRecordId.ToString();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            Users userDbAccess = new Users(connection, log, applicationName, partitionName, true);
            
            string[] macAddresses = userDbAccess.GetAllDevices(uint.Parse(deviceUserId.Text));

            if(macAddresses== null)
            {
                devicesAll.Items.Clear();
                devicesAll.Items.Add("N/A");
            }
            else
            {
                devicesAll.Items.Clear();
                devicesAll.Items.AddRange(macAddresses);
            }

        }

        private void primaryDeviceButton_Click(object sender, System.EventArgs e)
        {
            Users userDbAccess = new Users(connection, log, applicationName, partitionName, true);
            
            string macAddress = userDbAccess.GetPrimaryDevice(int.Parse(deviceUserId.Text));

            if(macAddress == null)
            {
                devicePrimaryMac.Text = "N/A";
            }
            else
            {
                devicePrimaryMac.Text = macAddress;
            }
        }

        private void callStart_Click(object sender, System.EventArgs e)
        {
            CallRecords callRecords = new CallRecords(connection, log, applicationName, partitionName,
                true);
            
            uint callRecordId;

            bool success = callRecords.WriteStart(
                uint.Parse(callStartUserId.Text), 
                uint.Parse(callStartSessionId.Text),
                callStartFrom.Text,
                callStartTo.Text,
                log.LogName,
                uint.Parse(callStartAuthRecordId.Text),
                out callRecordId);

            if(success) callStartResult.Text = "Success";
            else        callStartResult.Text = "Failure";

            callStartRecordId.Text = callRecordId.ToString();
            callEndRecordId.Text = callRecordId.ToString();
        }

        private void callStop_Click(object sender, System.EventArgs e)
        {
            CallRecords callRecords = new CallRecords(connection, log, applicationName, partitionName, 
                true);
            
            bool success = callRecords.WriteStop(
                int.Parse(callEndRecordId.Text),
                (EndReason)int.Parse(callEndEndReason.Text));

            if(success) callEndResult.Text = "Success";
            else        callEndResult.Text = "Failure";
        }

        private void startSessionButton_Click(object sender, System.EventArgs e)
        {
            SessionRecords sessionRecords = new SessionRecords(connection, log, applicationName, partitionName, 
                true);

            uint sessionId;
            ReturnValues success = sessionRecords.WriteCallSessionStart(uint.Parse(startSessionAuthRec.Text), out sessionId);
            startSessionRecordId.Text = sessionId.ToString();
            stopSessionRecordId.Text = sessionId.ToString();
        }

        private void stopSessionButton_Click(object sender, System.EventArgs e)
        {
            SessionRecords sessionRecords = new SessionRecords(connection, log, applicationName, partitionName, true);
            bool success = sessionRecords.WriteCallSessionStop(uint.Parse(stopSessionRecordId.Text));

            if(success)     stopSessionResult.Text = "success";
            else            stopSessionResult.Text = "failure";
        }

        private void groupBox4_Enter(object sender, System.EventArgs e)
        {
        
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Config config = new Config(connection, log, applicationName, partitionName,
                true);
            string[] configs = Enum.GetNames(typeof(ConfigurationName));

            configsList.Items.Clear();
            foreach(string configurationField in configs)
            {
                if(configurationField == ConfigurationName.Unspecified.ToString()) { continue; }
                string configValue = config.GetConfigValue((ConfigurationName) Enum.Parse(typeof(ConfigurationName), configurationField, false));

                if(configValue == null)
                {
                    configsList.Items.Add(new ListViewItem(new string[] { configurationField, "NOT FOUND" }));
                }
                else
                {
                    configsList.Items.Add(new ListViewItem(new string[] { configurationField, configValue }));
                }
            }
        }

        private void button2_Click_1(object sender, System.EventArgs e)
        {
            IntercomMembers intercom = new IntercomMembers(connection, log, applicationName, partitionName,
                true);

            uint userId;
            try   { userId = uint.Parse(intercomUserIdTextBox.Text); } 
            catch { userId = 1;}

            string[] intercomGroupIds;
            intercomGroupIds = intercom.GetIntercomGroupsForUser(userId);

            intercomResultsListView.Items.Clear();
            if(intercomGroupIds != null)
            {
                foreach(string id in intercomGroupIds)
                {
                    intercomResultsListView.Items.Add(id);
                }
            }
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            IntercomGroups intercom = new IntercomGroups(connection, log, applicationName, partitionName,
                true);

            uint intercomGroupsId;
            try   { intercomGroupsId = uint.Parse(intercomGroupsIdTextBox.Text); } 
            catch { intercomGroupsId = 1;}

            string name;
            bool isEnabled, isTalkbackEnabled, isPrivate;

            intercom.GetIntercomGroup(intercomGroupsId, out name, out isEnabled, out isTalkbackEnabled, out isPrivate);

            intercomGroupsNameLabel.Text = name;
            intercomGroupsIsEnabledLabel.Text = isEnabled.ToString();
            intercomGroupsIsTalkbackEnabledLabel.Text = isTalkbackEnabled.ToString();
            intercomGroupsIsPrivateLabel.Text = isPrivate.ToString();
        }

	}
}
