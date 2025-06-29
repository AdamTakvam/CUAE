using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SSHServer.NET
{
	/// <summary>
	/// Implements user configuration UI
	/// </summary>
	public class frmUser : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblUserName;
		private System.Windows.Forms.TextBox edUserName;
		private System.Windows.Forms.GroupBox gbAuthentication;
		private System.Windows.Forms.CheckBox cbPassword;
		private System.Windows.Forms.CheckBox cbPublicKey;
		private System.Windows.Forms.CheckBox cbKeyboard;
		private System.Windows.Forms.Button btnSetPassword;
		private System.Windows.Forms.RadioButton rbAny;
		private System.Windows.Forms.RadioButton rbAll;
		private System.Windows.Forms.Button btnSetPublicKey;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private string Password = "";
		private string PublicKey = "";

		public frmUser()
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

		// displays user settings form and changes user parameters if needed
		public static bool EditUserParameters(ref UserInfo user)
		{
			frmUser frm = new frmUser();
			if (frm == null) 
			{
				return false;
			}
			frm.InitDialog(user);
			if (frm.ShowDialog() == DialogResult.OK)
			{
				if (frm.Password.Length > 0) 
				{
					user.SetPassword(frm.Password);
				}
				if (frm.PublicKey.Length > 0) 
				{
					if (!user.SetPublicKey(frm.PublicKey)) 
					{
						MessageBox.Show("Failed to set private key: bad private key");
					}
				}
				user.AuthAll = frm.rbAll.Checked;
				user.Name = frm.edUserName.Text;
				int Auth = 0;
				if (frm.cbKeyboard.Checked) 
					Auth |= SBSSHConstants.Unit.SSH_AUTH_TYPE_KEYBOARD;
				if (frm.cbPassword.Checked) 
					Auth |= SBSSHConstants.Unit.SSH_AUTH_TYPE_PASSWORD;
				if (frm.cbPublicKey.Checked) 
					Auth |= SBSSHConstants.Unit.SSH_AUTH_TYPE_PUBLICKEY;
				user.AuthTypes = Auth;
				frm.Dispose();
				return true;
			}
			else {
				frm.Dispose(); 
				return false;
			}
		}

		// initialize dialog controls
		private void InitDialog(UserInfo user)
		{
			edUserName.Text = user.Name;
			cbPassword.Checked = (user.AuthTypes & SBSSHConstants.Unit.SSH_AUTH_TYPE_PASSWORD)!=0;
			cbPublicKey.Checked = (user.AuthTypes & SBSSHConstants.Unit.SSH_AUTH_TYPE_PUBLICKEY)!=0;
			cbKeyboard.Checked = (user.AuthTypes & SBSSHConstants.Unit.SSH_AUTH_TYPE_KEYBOARD)!=0;
			rbAll.Checked = user.AuthAll;
			rbAny.Checked = !rbAll.Checked;
			Password = "";
			PublicKey = "";
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblUserName = new System.Windows.Forms.Label();
			this.edUserName = new System.Windows.Forms.TextBox();
			this.gbAuthentication = new System.Windows.Forms.GroupBox();
			this.btnSetPublicKey = new System.Windows.Forms.Button();
			this.rbAll = new System.Windows.Forms.RadioButton();
			this.rbAny = new System.Windows.Forms.RadioButton();
			this.btnSetPassword = new System.Windows.Forms.Button();
			this.cbKeyboard = new System.Windows.Forms.CheckBox();
			this.cbPublicKey = new System.Windows.Forms.CheckBox();
			this.cbPassword = new System.Windows.Forms.CheckBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.gbAuthentication.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblUserName
			// 
			this.lblUserName.Location = new System.Drawing.Point(8, 8);
			this.lblUserName.Name = "lblUserName";
			this.lblUserName.Size = new System.Drawing.Size(72, 16);
			this.lblUserName.TabIndex = 0;
			this.lblUserName.Text = "User name : ";
			// 
			// edUserName
			// 
			this.edUserName.Location = new System.Drawing.Point(88, 6);
			this.edUserName.Name = "edUserName";
			this.edUserName.Size = new System.Drawing.Size(232, 20);
			this.edUserName.TabIndex = 1;
			this.edUserName.Text = "";
			// 
			// gbAuthentication
			// 
			this.gbAuthentication.Controls.Add(this.btnSetPublicKey);
			this.gbAuthentication.Controls.Add(this.rbAll);
			this.gbAuthentication.Controls.Add(this.rbAny);
			this.gbAuthentication.Controls.Add(this.btnSetPassword);
			this.gbAuthentication.Controls.Add(this.cbKeyboard);
			this.gbAuthentication.Controls.Add(this.cbPublicKey);
			this.gbAuthentication.Controls.Add(this.cbPassword);
			this.gbAuthentication.Location = new System.Drawing.Point(8, 32);
			this.gbAuthentication.Name = "gbAuthentication";
			this.gbAuthentication.Size = new System.Drawing.Size(352, 152);
			this.gbAuthentication.TabIndex = 2;
			this.gbAuthentication.TabStop = false;
			this.gbAuthentication.Text = "Allowed authentications:";
			// 
			// btnSetPublicKey
			// 
			this.btnSetPublicKey.Location = new System.Drawing.Point(248, 45);
			this.btnSetPublicKey.Name = "btnSetPublicKey";
			this.btnSetPublicKey.Size = new System.Drawing.Size(96, 24);
			this.btnSetPublicKey.TabIndex = 6;
			this.btnSetPublicKey.Text = "Set public key...";
			this.btnSetPublicKey.Click += new System.EventHandler(this.btnSetPublicKey_Click);
			// 
			// rbAll
			// 
			this.rbAll.Location = new System.Drawing.Point(5, 125);
			this.rbAll.Name = "rbAll";
			this.rbAll.Size = new System.Drawing.Size(339, 18);
			this.rbAll.TabIndex = 5;
			this.rbAll.Text = "The user must authenticate using ALL of the above methods";
			// 
			// rbAny
			// 
			this.rbAny.Location = new System.Drawing.Point(5, 103);
			this.rbAny.Name = "rbAny";
			this.rbAny.Size = new System.Drawing.Size(339, 16);
			this.rbAny.TabIndex = 4;
			this.rbAny.Text = "The user must authenticate using ANY of the above methods";
			this.rbAny.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			// 
			// btnSetPassword
			// 
			this.btnSetPassword.Location = new System.Drawing.Point(248, 15);
			this.btnSetPassword.Name = "btnSetPassword";
			this.btnSetPassword.Size = new System.Drawing.Size(96, 23);
			this.btnSetPassword.TabIndex = 3;
			this.btnSetPassword.Text = "Set password...";
			this.btnSetPassword.Click += new System.EventHandler(this.btnSetPassword_Click);
			// 
			// cbKeyboard
			// 
			this.cbKeyboard.Location = new System.Drawing.Point(8, 76);
			this.cbKeyboard.Name = "cbKeyboard";
			this.cbKeyboard.Size = new System.Drawing.Size(240, 18);
			this.cbKeyboard.TabIndex = 2;
			this.cbKeyboard.Text = "Allow keyboard-interactive authentication";
			// 
			// cbPublicKey
			// 
			this.cbPublicKey.Location = new System.Drawing.Point(8, 48);
			this.cbPublicKey.Name = "cbPublicKey";
			this.cbPublicKey.Size = new System.Drawing.Size(184, 18);
			this.cbPublicKey.TabIndex = 1;
			this.cbPublicKey.Text = "Allow public-key authentication";
			// 
			// cbPassword
			// 
			this.cbPassword.Location = new System.Drawing.Point(9, 19);
			this.cbPassword.Name = "cbPassword";
			this.cbPassword.Size = new System.Drawing.Size(183, 16);
			this.cbPassword.TabIndex = 0;
			this.cbPassword.Text = "Allow password authentication";
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(216, 192);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(64, 24);
			this.btnOk.TabIndex = 3;
			this.btnOk.Text = "Ok";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(288, 192);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(64, 24);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			// 
			// frmUser
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(368, 222);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.gbAuthentication);
			this.Controls.Add(this.edUserName);
			this.Controls.Add(this.lblUserName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmUser";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "User settings";
			this.gbAuthentication.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnSetPassword_Click(object sender, System.EventArgs e)
		{
			frmGetPwd.GetPassword(ref Password);
		}

		private void btnSetPublicKey_Click(object sender, System.EventArgs e)
		{
            frmGetKey.GetKey(ref PublicKey);
		}
	}
}
