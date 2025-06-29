using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SimpleFTPSDemo
{
	/// <summary>
	/// Summary description for ConnPropsForm.
	/// </summary>
	public class ConnPropsForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox gbProps;
		private System.Windows.Forms.Label label1;
		internal System.Windows.Forms.TextBox editHost;
		private System.Windows.Forms.Label label2;
		internal System.Windows.Forms.TextBox editUsername;
		private System.Windows.Forms.Label label3;
		internal System.Windows.Forms.TextBox editPassword;
		private System.Windows.Forms.Label label4;
		internal System.Windows.Forms.CheckBox cbUseSSL;
		internal System.Windows.Forms.ComboBox comboAuthCmd;
		private System.Windows.Forms.Label label5;
		internal System.Windows.Forms.CheckBox cbSSL2;
		internal System.Windows.Forms.CheckBox cbSSL3;
		internal System.Windows.Forms.CheckBox cbTLS1;
		internal System.Windows.Forms.CheckBox cbTLS11;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button btnBrowse;
		internal System.Windows.Forms.TextBox editCertPassword;
		private System.Windows.Forms.Label lblCertPassword;
		internal System.Windows.Forms.CheckBox cbImplicit;
		internal System.Windows.Forms.CheckBox cbPassive;
		internal System.Windows.Forms.CheckBox cbClear;
		internal System.Windows.Forms.TextBox editCert;
		internal System.Windows.Forms.NumericUpDown editPort;
		private System.Windows.Forms.OpenFileDialog dlgOpen;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ConnPropsForm()
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
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.gbProps = new System.Windows.Forms.GroupBox();
			this.editCert = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.cbTLS11 = new System.Windows.Forms.CheckBox();
			this.cbTLS1 = new System.Windows.Forms.CheckBox();
			this.cbSSL3 = new System.Windows.Forms.CheckBox();
			this.cbSSL2 = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.comboAuthCmd = new System.Windows.Forms.ComboBox();
			this.cbClear = new System.Windows.Forms.CheckBox();
			this.cbUseSSL = new System.Windows.Forms.CheckBox();
			this.editPassword = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.editUsername = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.editHost = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.editCertPassword = new System.Windows.Forms.TextBox();
			this.lblCertPassword = new System.Windows.Forms.Label();
			this.cbImplicit = new System.Windows.Forms.CheckBox();
			this.cbPassive = new System.Windows.Forms.CheckBox();
			this.editPort = new System.Windows.Forms.NumericUpDown();
			this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
			this.gbProps.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.editPort)).BeginInit();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(160, 312);
			this.btnOk.Name = "btnOk";
			this.btnOk.TabIndex = 0;
			this.btnOk.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(240, 312);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			// 
			// gbProps
			// 
			this.gbProps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.gbProps.Controls.Add(this.editPort);
			this.gbProps.Controls.Add(this.cbPassive);
			this.gbProps.Controls.Add(this.cbImplicit);
			this.gbProps.Controls.Add(this.editCertPassword);
			this.gbProps.Controls.Add(this.lblCertPassword);
			this.gbProps.Controls.Add(this.btnBrowse);
			this.gbProps.Controls.Add(this.editCert);
			this.gbProps.Controls.Add(this.label6);
			this.gbProps.Controls.Add(this.cbTLS11);
			this.gbProps.Controls.Add(this.cbTLS1);
			this.gbProps.Controls.Add(this.cbSSL3);
			this.gbProps.Controls.Add(this.cbSSL2);
			this.gbProps.Controls.Add(this.label5);
			this.gbProps.Controls.Add(this.comboAuthCmd);
			this.gbProps.Controls.Add(this.cbClear);
			this.gbProps.Controls.Add(this.cbUseSSL);
			this.gbProps.Controls.Add(this.editPassword);
			this.gbProps.Controls.Add(this.label4);
			this.gbProps.Controls.Add(this.editUsername);
			this.gbProps.Controls.Add(this.label3);
			this.gbProps.Controls.Add(this.label2);
			this.gbProps.Controls.Add(this.editHost);
			this.gbProps.Controls.Add(this.label1);
			this.gbProps.Location = new System.Drawing.Point(8, 8);
			this.gbProps.Name = "gbProps";
			this.gbProps.Size = new System.Drawing.Size(304, 296);
			this.gbProps.TabIndex = 2;
			this.gbProps.TabStop = false;
			this.gbProps.Text = "Connection properties";
			// 
			// editCert
			// 
			this.editCert.Location = new System.Drawing.Point(16, 208);
			this.editCert.Name = "editCert";
			this.editCert.Size = new System.Drawing.Size(248, 20);
			this.editCert.TabIndex = 17;
			this.editCert.Text = "";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 192);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(208, 16);
			this.label6.TabIndex = 16;
			this.label6.Text = "Use certificate (PFX format assumed)";
			// 
			// cbTLS11
			// 
			this.cbTLS11.Checked = true;
			this.cbTLS11.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbTLS11.Location = new System.Drawing.Point(232, 160);
			this.cbTLS11.Name = "cbTLS11";
			this.cbTLS11.Size = new System.Drawing.Size(64, 24);
			this.cbTLS11.TabIndex = 15;
			this.cbTLS11.Text = "TLS 1.1";
			// 
			// cbTLS1
			// 
			this.cbTLS1.Checked = true;
			this.cbTLS1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbTLS1.Location = new System.Drawing.Point(160, 160);
			this.cbTLS1.Name = "cbTLS1";
			this.cbTLS1.Size = new System.Drawing.Size(56, 24);
			this.cbTLS1.TabIndex = 14;
			this.cbTLS1.Text = "TLS 1";
			// 
			// cbSSL3
			// 
			this.cbSSL3.Checked = true;
			this.cbSSL3.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbSSL3.Location = new System.Drawing.Point(88, 160);
			this.cbSSL3.Name = "cbSSL3";
			this.cbSSL3.Size = new System.Drawing.Size(56, 24);
			this.cbSSL3.TabIndex = 13;
			this.cbSSL3.Text = "SSL3";
			// 
			// cbSSL2
			// 
			this.cbSSL2.Location = new System.Drawing.Point(16, 160);
			this.cbSSL2.Name = "cbSSL2";
			this.cbSSL2.Size = new System.Drawing.Size(56, 24);
			this.cbSSL2.TabIndex = 12;
			this.cbSSL2.Text = "SSL2";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(16, 136);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(83, 16);
			this.label5.TabIndex = 11;
			this.label5.Text = "Auth command:";
			// 
			// comboAuthCmd
			// 
			this.comboAuthCmd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAuthCmd.Items.AddRange(new object[] {
															  "Autodetect",
															  "AUTH TLS",
															  "AUTH SSL",
															  "AUTH TLS-P",
															  "AUTH TLS-C"});
			this.comboAuthCmd.Location = new System.Drawing.Point(104, 132);
			this.comboAuthCmd.Name = "comboAuthCmd";
			this.comboAuthCmd.Size = new System.Drawing.Size(121, 21);
			this.comboAuthCmd.TabIndex = 10;
			// 
			// cbClear
			// 
			this.cbClear.Location = new System.Drawing.Point(152, 104);
			this.cbClear.Name = "cbClear";
			this.cbClear.Size = new System.Drawing.Size(144, 24);
			this.cbClear.TabIndex = 9;
			this.cbClear.Text = "Use clear data channel";
			// 
			// cbUseSSL
			// 
			this.cbUseSSL.Location = new System.Drawing.Point(16, 104);
			this.cbUseSSL.Name = "cbUseSSL";
			this.cbUseSSL.TabIndex = 8;
			this.cbUseSSL.Text = "Use SSL/TLS";
			// 
			// editPassword
			// 
			this.editPassword.Location = new System.Drawing.Point(200, 80);
			this.editPassword.Name = "editPassword";
			this.editPassword.Size = new System.Drawing.Size(96, 20);
			this.editPassword.TabIndex = 7;
			this.editPassword.Text = "";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(200, 64);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(54, 16);
			this.label4.TabIndex = 6;
			this.label4.Text = "Password";
			// 
			// editUsername
			// 
			this.editUsername.Location = new System.Drawing.Point(16, 80);
			this.editUsername.Name = "editUsername";
			this.editUsername.TabIndex = 5;
			this.editUsername.Text = "anonymous";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(16, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 16);
			this.label3.TabIndex = 4;
			this.label3.Text = "Username";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(224, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(25, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "Port";
			// 
			// editHost
			// 
			this.editHost.Location = new System.Drawing.Point(16, 40);
			this.editHost.Name = "editHost";
			this.editHost.Size = new System.Drawing.Size(200, 20);
			this.editHost.TabIndex = 1;
			this.editHost.Text = "localhost";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(27, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Host";
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(264, 208);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(24, 23);
			this.btnBrowse.TabIndex = 18;
			this.btnBrowse.Text = "...";
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// editCertPassword
			// 
			this.editCertPassword.Location = new System.Drawing.Point(72, 240);
			this.editCertPassword.Name = "editCertPassword";
			this.editCertPassword.Size = new System.Drawing.Size(96, 20);
			this.editCertPassword.TabIndex = 20;
			this.editCertPassword.Text = "";
			// 
			// lblCertPassword
			// 
			this.lblCertPassword.AutoSize = true;
			this.lblCertPassword.Location = new System.Drawing.Point(16, 240);
			this.lblCertPassword.Name = "lblCertPassword";
			this.lblCertPassword.Size = new System.Drawing.Size(57, 16);
			this.lblCertPassword.TabIndex = 19;
			this.lblCertPassword.Text = "Password:";
			// 
			// cbImplicit
			// 
			this.cbImplicit.Location = new System.Drawing.Point(16, 272);
			this.cbImplicit.Name = "cbImplicit";
			this.cbImplicit.Size = new System.Drawing.Size(120, 16);
			this.cbImplicit.TabIndex = 21;
			this.cbImplicit.Text = "Use implicit SSL";
			// 
			// cbPassive
			// 
			this.cbPassive.Location = new System.Drawing.Point(160, 272);
			this.cbPassive.Name = "cbPassive";
			this.cbPassive.Size = new System.Drawing.Size(128, 16);
			this.cbPassive.TabIndex = 22;
			this.cbPassive.Text = "Passive FTP mode";
			// 
			// editPort
			// 
			this.editPort.Location = new System.Drawing.Point(224, 40);
			this.editPort.Maximum = new System.Decimal(new int[] {
																	 65535,
																	 0,
																	 0,
																	 0});
			this.editPort.Minimum = new System.Decimal(new int[] {
																	 1,
																	 0,
																	 0,
																	 0});
			this.editPort.Name = "editPort";
			this.editPort.Size = new System.Drawing.Size(72, 20);
			this.editPort.TabIndex = 23;
			this.editPort.Value = new System.Decimal(new int[] {
																   21,
																   0,
																   0,
																   0});
			// 
			// dlgOpen
			// 
			this.dlgOpen.Filter = "Certificates in PFX format|*.pkcs12;*.pfx";
			this.dlgOpen.Title = "Select certificate";
			// 
			// ConnPropsForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(320, 343);
			this.Controls.Add(this.gbProps);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConnPropsForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Connection properties";
			this.gbProps.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.editPort)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
			if (dlgOpen.ShowDialog() == DialogResult.OK) 
			{
				editCert.Text = dlgOpen.FileName;
			}
		}
	}
}
