using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SimpleSftpDemo
{
	/// <summary>
	/// Summary description for ConnPropsForm.
	/// </summary>
	public class frmConnProps : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbConnProps;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lHost;
		private System.Windows.Forms.Label lusername;
		private System.Windows.Forms.Label lPassword;
		public System.Windows.Forms.TextBox tbHost;
		public System.Windows.Forms.TextBox tbUsername;
		public System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.Button cmdPrivateKey;
		public System.Windows.Forms.TextBox txtPrivateKey;
		private System.Windows.Forms.Label lbPrivateKey;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
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
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.tbUsername = new System.Windows.Forms.TextBox();
			this.tbHost = new System.Windows.Forms.TextBox();
			this.lPassword = new System.Windows.Forms.Label();
			this.lusername = new System.Windows.Forms.Label();
			this.lHost = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.cmdPrivateKey = new System.Windows.Forms.Button();
			this.txtPrivateKey = new System.Windows.Forms.TextBox();
			this.lbPrivateKey = new System.Windows.Forms.Label();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.gbConnProps.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbConnProps
			// 
			this.gbConnProps.Controls.Add(this.cmdPrivateKey);
			this.gbConnProps.Controls.Add(this.txtPrivateKey);
			this.gbConnProps.Controls.Add(this.lbPrivateKey);
			this.gbConnProps.Controls.Add(this.tbPassword);
			this.gbConnProps.Controls.Add(this.tbUsername);
			this.gbConnProps.Controls.Add(this.tbHost);
			this.gbConnProps.Controls.Add(this.lPassword);
			this.gbConnProps.Controls.Add(this.lusername);
			this.gbConnProps.Controls.Add(this.lHost);
			this.gbConnProps.Location = new System.Drawing.Point(8, 8);
			this.gbConnProps.Name = "gbConnProps";
			this.gbConnProps.Size = new System.Drawing.Size(336, 192);
			this.gbConnProps.TabIndex = 0;
			this.gbConnProps.TabStop = false;
			this.gbConnProps.Text = "Connection properties";
			// 
			// tbPassword
			// 
			this.tbPassword.Location = new System.Drawing.Point(176, 96);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.PasswordChar = '*';
			this.tbPassword.Size = new System.Drawing.Size(144, 21);
			this.tbPassword.TabIndex = 5;
			this.tbPassword.Text = "";
			// 
			// tbUsername
			// 
			this.tbUsername.Location = new System.Drawing.Point(16, 96);
			this.tbUsername.Name = "tbUsername";
			this.tbUsername.Size = new System.Drawing.Size(152, 21);
			this.tbUsername.TabIndex = 4;
			this.tbUsername.Text = "user";
			// 
			// tbHost
			// 
			this.tbHost.Location = new System.Drawing.Point(16, 40);
			this.tbHost.Name = "tbHost";
			this.tbHost.Size = new System.Drawing.Size(304, 21);
			this.tbHost.TabIndex = 3;
			this.tbHost.Text = "192.168.0.1";
			// 
			// lPassword
			// 
			this.lPassword.Location = new System.Drawing.Point(176, 80);
			this.lPassword.Name = "lPassword";
			this.lPassword.Size = new System.Drawing.Size(100, 16);
			this.lPassword.TabIndex = 2;
			this.lPassword.Text = "Password";
			// 
			// lusername
			// 
			this.lusername.Location = new System.Drawing.Point(16, 80);
			this.lusername.Name = "lusername";
			this.lusername.Size = new System.Drawing.Size(100, 16);
			this.lusername.TabIndex = 1;
			this.lusername.Text = "Username";
			// 
			// lHost
			// 
			this.lHost.Location = new System.Drawing.Point(16, 24);
			this.lHost.Name = "lHost";
			this.lHost.Size = new System.Drawing.Size(100, 16);
			this.lHost.TabIndex = 0;
			this.lHost.Text = "Host";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(192, 208);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(272, 208);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			// 
			// cmdPrivateKey
			// 
			this.cmdPrivateKey.Location = new System.Drawing.Point(272, 152);
			this.cmdPrivateKey.Name = "cmdPrivateKey";
			this.cmdPrivateKey.Size = new System.Drawing.Size(24, 23);
			this.cmdPrivateKey.TabIndex = 12;
			this.cmdPrivateKey.Text = "...";
			this.cmdPrivateKey.Click += new System.EventHandler(this.cmdPrivateKey_Click);
			// 
			// txtPrivateKey
			// 
			this.txtPrivateKey.Location = new System.Drawing.Point(16, 152);
			this.txtPrivateKey.Name = "txtPrivateKey";
			this.txtPrivateKey.Size = new System.Drawing.Size(248, 21);
			this.txtPrivateKey.TabIndex = 11;
			this.txtPrivateKey.Text = "";
			// 
			// lbPrivateKey
			// 
			this.lbPrivateKey.Location = new System.Drawing.Point(16, 136);
			this.lbPrivateKey.Name = "lbPrivateKey";
			this.lbPrivateKey.Size = new System.Drawing.Size(264, 16);
			this.lbPrivateKey.TabIndex = 10;
			this.lbPrivateKey.Text = "Private key file for PUBLICKEY authentication type";
			// 
			// frmConnProps
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(352, 245);
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
