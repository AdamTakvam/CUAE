using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PubKeyCliDemo
{
	/// <summary>
	/// Summary description for ConnPropsForm.
	/// </summary>
	public class frmConnProps : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbConnProps;
		private System.Windows.Forms.Label lblHost;
		private System.Windows.Forms.Label lblPort;
		public System.Windows.Forms.TextBox tbHost;
		public System.Windows.Forms.TextBox tbPort;
		private System.Windows.Forms.Label lblUsername;
		private System.Windows.Forms.Label lblPassword;
		public System.Windows.Forms.TextBox tbPassword;
		public System.Windows.Forms.TextBox tbUsername;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
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
			this.lblHost = new System.Windows.Forms.Label();
			this.lblPort = new System.Windows.Forms.Label();
			this.tbHost = new System.Windows.Forms.TextBox();
			this.tbPort = new System.Windows.Forms.TextBox();
			this.lblUsername = new System.Windows.Forms.Label();
			this.lblPassword = new System.Windows.Forms.Label();
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.tbUsername = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.gbConnProps.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbConnProps
			// 
			this.gbConnProps.Controls.Add(this.tbUsername);
			this.gbConnProps.Controls.Add(this.tbPassword);
			this.gbConnProps.Controls.Add(this.lblPassword);
			this.gbConnProps.Controls.Add(this.lblUsername);
			this.gbConnProps.Controls.Add(this.tbPort);
			this.gbConnProps.Controls.Add(this.tbHost);
			this.gbConnProps.Controls.Add(this.lblPort);
			this.gbConnProps.Controls.Add(this.lblHost);
			this.gbConnProps.Location = new System.Drawing.Point(8, 8);
			this.gbConnProps.Name = "gbConnProps";
			this.gbConnProps.Size = new System.Drawing.Size(296, 176);
			this.gbConnProps.TabIndex = 0;
			this.gbConnProps.TabStop = false;
			this.gbConnProps.Text = "Connection properties";
			// 
			// lblHost
			// 
			this.lblHost.Location = new System.Drawing.Point(16, 24);
			this.lblHost.Name = "lblHost";
			this.lblHost.Size = new System.Drawing.Size(112, 16);
			this.lblHost.TabIndex = 0;
			this.lblHost.Text = "Host:";
			// 
			// lblPort
			// 
			this.lblPort.Location = new System.Drawing.Point(224, 24);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(48, 16);
			this.lblPort.TabIndex = 1;
			this.lblPort.Text = "Port:";
			// 
			// tbHost
			// 
			this.tbHost.Location = new System.Drawing.Point(16, 40);
			this.tbHost.Name = "tbHost";
			this.tbHost.Size = new System.Drawing.Size(200, 21);
			this.tbHost.TabIndex = 0;
			this.tbHost.Text = "192.168.0.1";
			// 
			// tbPort
			// 
			this.tbPort.Location = new System.Drawing.Point(224, 40);
			this.tbPort.Name = "tbPort";
			this.tbPort.Size = new System.Drawing.Size(56, 21);
			this.tbPort.TabIndex = 1;
			this.tbPort.Text = "22";
			// 
			// lblUsername
			// 
			this.lblUsername.Location = new System.Drawing.Point(16, 72);
			this.lblUsername.Name = "lblUsername";
			this.lblUsername.Size = new System.Drawing.Size(120, 16);
			this.lblUsername.TabIndex = 4;
			this.lblUsername.Text = "Username:";
			// 
			// lblPassword
			// 
			this.lblPassword.Location = new System.Drawing.Point(16, 120);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(100, 16);
			this.lblPassword.TabIndex = 5;
			this.lblPassword.Text = "Password:";
			// 
			// tbPassword
			// 
			this.tbPassword.Location = new System.Drawing.Point(16, 136);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.PasswordChar = '*';
			this.tbPassword.Size = new System.Drawing.Size(144, 21);
			this.tbPassword.TabIndex = 3;
			this.tbPassword.Text = "";
			// 
			// tbUsername
			// 
			this.tbUsername.Location = new System.Drawing.Point(16, 88);
			this.tbUsername.Name = "tbUsername";
			this.tbUsername.Size = new System.Drawing.Size(144, 21);
			this.tbUsername.TabIndex = 2;
			this.tbUsername.Text = "";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(80, 192);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(160, 192);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			// 
			// frmConnProps
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(314, 231);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.gbConnProps);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmConnProps";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Connection properties";
			this.gbConnProps.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
