using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SimpleSSHDemo
{
	/// <summary>
	/// Summary description for ConnPropsForm.
	/// </summary>
	public class frmConnProps : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lUsername;
		public System.Windows.Forms.TextBox tbHost;
		private System.Windows.Forms.Label lHost;
		public System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.Label lPassword;
		public System.Windows.Forms.TextBox tbUser;
		public System.Windows.Forms.CheckBox cbSSH1;
		public System.Windows.Forms.CheckBox cbSSH2;
		public System.Windows.Forms.CheckBox cbCompress;
		private System.Windows.Forms.GroupBox gbConnProps;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Label lbPrivateKey;
		public System.Windows.Forms.TextBox txtPrivateKey;
		private System.Windows.Forms.Button cmdPrivateKey;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmConnProps()
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
			this.gbConnProps = new System.Windows.Forms.GroupBox();
			this.cbCompress = new System.Windows.Forms.CheckBox();
			this.cbSSH2 = new System.Windows.Forms.CheckBox();
			this.cbSSH1 = new System.Windows.Forms.CheckBox();
			this.tbUser = new System.Windows.Forms.TextBox();
			this.lPassword = new System.Windows.Forms.Label();
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.lHost = new System.Windows.Forms.Label();
			this.tbHost = new System.Windows.Forms.TextBox();
			this.lUsername = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.lbPrivateKey = new System.Windows.Forms.Label();
			this.txtPrivateKey = new System.Windows.Forms.TextBox();
			this.cmdPrivateKey = new System.Windows.Forms.Button();
			this.gbConnProps.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbConnProps
			// 
			this.gbConnProps.Controls.Add(this.cmdPrivateKey);
			this.gbConnProps.Controls.Add(this.txtPrivateKey);
			this.gbConnProps.Controls.Add(this.lbPrivateKey);
			this.gbConnProps.Controls.Add(this.cbCompress);
			this.gbConnProps.Controls.Add(this.cbSSH2);
			this.gbConnProps.Controls.Add(this.cbSSH1);
			this.gbConnProps.Controls.Add(this.tbUser);
			this.gbConnProps.Controls.Add(this.lPassword);
			this.gbConnProps.Controls.Add(this.tbPassword);
			this.gbConnProps.Controls.Add(this.lHost);
			this.gbConnProps.Controls.Add(this.tbHost);
			this.gbConnProps.Controls.Add(this.lUsername);
			this.gbConnProps.Location = new System.Drawing.Point(8, 8);
			this.gbConnProps.Name = "gbConnProps";
			this.gbConnProps.Size = new System.Drawing.Size(320, 216);
			this.gbConnProps.TabIndex = 0;
			this.gbConnProps.TabStop = false;
			this.gbConnProps.Text = "Connection properties";
			// 
			// cbCompress
			// 
			this.cbCompress.Location = new System.Drawing.Point(184, 136);
			this.cbCompress.Name = "cbCompress";
			this.cbCompress.TabIndex = 6;
			this.cbCompress.Text = "Compress traffic";
			// 
			// cbSSH2
			// 
			this.cbSSH2.Checked = true;
			this.cbSSH2.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbSSH2.Location = new System.Drawing.Point(184, 112);
			this.cbSSH2.Name = "cbSSH2";
			this.cbSSH2.Size = new System.Drawing.Size(64, 24);
			this.cbSSH2.TabIndex = 5;
			this.cbSSH2.Text = "SSHv2";
			// 
			// cbSSH1
			// 
			this.cbSSH1.Checked = true;
			this.cbSSH1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbSSH1.Location = new System.Drawing.Point(184, 88);
			this.cbSSH1.Name = "cbSSH1";
			this.cbSSH1.Size = new System.Drawing.Size(56, 24);
			this.cbSSH1.TabIndex = 4;
			this.cbSSH1.Text = "SSHv1";
			// 
			// tbUser
			// 
			this.tbUser.Location = new System.Drawing.Point(16, 88);
			this.tbUser.Name = "tbUser";
			this.tbUser.Size = new System.Drawing.Size(136, 21);
			this.tbUser.TabIndex = 2;
			this.tbUser.Text = "user";
			// 
			// lPassword
			// 
			this.lPassword.Location = new System.Drawing.Point(16, 120);
			this.lPassword.Name = "lPassword";
			this.lPassword.Size = new System.Drawing.Size(100, 16);
			this.lPassword.TabIndex = 4;
			this.lPassword.Text = "Password";
			// 
			// tbPassword
			// 
			this.tbPassword.Location = new System.Drawing.Point(16, 136);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.PasswordChar = '*';
			this.tbPassword.Size = new System.Drawing.Size(136, 21);
			this.tbPassword.TabIndex = 3;
			this.tbPassword.Text = "";
			// 
			// lHost
			// 
			this.lHost.Location = new System.Drawing.Point(16, 24);
			this.lHost.Name = "lHost";
			this.lHost.Size = new System.Drawing.Size(100, 16);
			this.lHost.TabIndex = 2;
			this.lHost.Text = "Host";
			// 
			// tbHost
			// 
			this.tbHost.Location = new System.Drawing.Point(16, 40);
			this.tbHost.Name = "tbHost";
			this.tbHost.Size = new System.Drawing.Size(288, 21);
			this.tbHost.TabIndex = 1;
			this.tbHost.Text = "192.168.0.1";
			// 
			// lUsername
			// 
			this.lUsername.Location = new System.Drawing.Point(16, 72);
			this.lUsername.Name = "lUsername";
			this.lUsername.Size = new System.Drawing.Size(100, 16);
			this.lUsername.TabIndex = 0;
			this.lUsername.Text = "Username";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(168, 232);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 7;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(248, 232);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			// 
			// lbPrivateKey
			// 
			this.lbPrivateKey.Location = new System.Drawing.Point(14, 166);
			this.lbPrivateKey.Name = "lbPrivateKey";
			this.lbPrivateKey.Size = new System.Drawing.Size(264, 16);
			this.lbPrivateKey.TabIndex = 7;
			this.lbPrivateKey.Text = "Private key file for PUBLICKEY authentication type";
			// 
			// txtPrivateKey
			// 
			this.txtPrivateKey.Location = new System.Drawing.Point(14, 182);
			this.txtPrivateKey.Name = "txtPrivateKey";
			this.txtPrivateKey.Size = new System.Drawing.Size(248, 21);
			this.txtPrivateKey.TabIndex = 8;
			this.txtPrivateKey.Text = "";
			// 
			// cmdPrivateKey
			// 
			this.cmdPrivateKey.Location = new System.Drawing.Point(270, 182);
			this.cmdPrivateKey.Name = "cmdPrivateKey";
			this.cmdPrivateKey.Size = new System.Drawing.Size(24, 23);
			this.cmdPrivateKey.TabIndex = 9;
			this.cmdPrivateKey.Text = "...";
			this.cmdPrivateKey.Click += new System.EventHandler(this.cmdPrivateKey_Click);
			// 
			// frmConnProps
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(336, 264);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.gbConnProps);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmConnProps";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Connection properties";
			this.gbConnProps.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void cmdPrivateKey_Click(object sender, System.EventArgs e)
		{
			if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
			{
				txtPrivateKey.Text = openFileDialog1.FileName;
			}
		}
	}
}
