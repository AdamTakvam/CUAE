using System;
using System.Windows.Forms;

namespace SSHServer.NET
{
    /// <summary>
    /// Responsible for retrieving password from user
    /// </summary>
	public class frmGetPwd : Form
    {
        public PictureBox imgKeys = null;
        public TextBox edPassword = null;
        public Label lblPassword = null;
        public Button btnOk = null;
		private System.Windows.Forms.ToolTip ttMain;
		public System.Windows.Forms.Label lblConfirmPassword;
		public System.Windows.Forms.TextBox edConfirmPassword;
        public Button btnCancel = null;
        public frmGetPwd()
        {
            InitializeComponent();
        }

		private System.ComponentModel.IContainer components;

#region Windows Form Designer generated code
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmGetPwd));
			this.ttMain = new System.Windows.Forms.ToolTip(this.components);
			this.edPassword = new System.Windows.Forms.TextBox();
			this.edConfirmPassword = new System.Windows.Forms.TextBox();
			this.imgKeys = new System.Windows.Forms.PictureBox();
			this.lblPassword = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblConfirmPassword = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// edPassword
			// 
			this.edPassword.Location = new System.Drawing.Point(48, 18);
			this.edPassword.Name = "edPassword";
			this.edPassword.PasswordChar = '*';
			this.edPassword.Size = new System.Drawing.Size(200, 20);
			this.edPassword.TabIndex = 1;
			this.edPassword.Text = "";
			this.ttMain.SetToolTip(this.edPassword, "Enter your password for private key here");
			// 
			// edConfirmPassword
			// 
			this.edConfirmPassword.Location = new System.Drawing.Point(48, 56);
			this.edConfirmPassword.Name = "edConfirmPassword";
			this.edConfirmPassword.PasswordChar = '*';
			this.edConfirmPassword.Size = new System.Drawing.Size(200, 20);
			this.edConfirmPassword.TabIndex = 3;
			this.edConfirmPassword.Text = "";
			this.ttMain.SetToolTip(this.edConfirmPassword, "Enter your password for private key here");
			this.edConfirmPassword.TextChanged += new System.EventHandler(this.edConfirmPassword_TextChanged);
			// 
			// imgKeys
			// 
			this.imgKeys.Image = ((System.Drawing.Image)(resources.GetObject("imgKeys.Image")));
			this.imgKeys.Location = new System.Drawing.Point(8, 24);
			this.imgKeys.Name = "imgKeys";
			this.imgKeys.Size = new System.Drawing.Size(32, 32);
			this.imgKeys.TabIndex = 0;
			this.imgKeys.TabStop = false;
			// 
			// lblPassword
			// 
			this.lblPassword.Location = new System.Drawing.Point(55, 1);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(153, 13);
			this.lblPassword.TabIndex = 0;
			this.lblPassword.Text = "Enter new password for user :";
			// 
			// btnOk
			// 
			this.btnOk.BackColor = System.Drawing.SystemColors.Control;
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Enabled = false;
			this.btnOk.Location = new System.Drawing.Point(20, 84);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(47, 21);
			this.btnOk.TabIndex = 4;
			this.btnOk.Text = "O&k";
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(196, 84);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(47, 21);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "&Cancel";
			// 
			// lblConfirmPassword
			// 
			this.lblConfirmPassword.Location = new System.Drawing.Point(56, 40);
			this.lblConfirmPassword.Name = "lblConfirmPassword";
			this.lblConfirmPassword.Size = new System.Drawing.Size(145, 13);
			this.lblConfirmPassword.TabIndex = 2;
			this.lblConfirmPassword.Text = "Password confirmation :";
			// 
			// frmGetPwd
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoScroll = true;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(258, 112);
			this.Controls.Add(this.lblConfirmPassword);
			this.Controls.Add(this.edConfirmPassword);
			this.Controls.Add(this.edPassword);
			this.Controls.Add(this.imgKeys);
			this.Controls.Add(this.lblPassword);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.KeyPreview = true;
			this.Location = new System.Drawing.Point(320, 225);
			this.Name = "frmGetPwd";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Set password";
			this.ttMain.SetToolTip(this, "Enter password for user authentication");
			this.Activated += new System.EventHandler(this.FormActivate);
			this.ResumeLayout(false);

		}
#endregion

        /// <summary>
        /// Used to retrieve a password from user
        /// </summary>
        /// <param name="Pwd">requested password</param>
        /// <returns>true if the password was provided by user</returns>
        public static bool GetPassword(ref string Pwd)
        {
	        frmGetPwd frm = new frmGetPwd();
			if (frm == null) 
			{
				return false;
			}
			if (frm.ShowDialog() == DialogResult.OK)
            {
                Pwd = frm.edPassword.Text;
				frm.Dispose();
				return true;
            }
			else 
			{
				frm.Dispose(); 
				return false;
			}
        }

        public void FormActivate(System.Object Sender, System.EventArgs _e1)
        {
            edPassword.Text = "";
			edConfirmPassword.Text = "";
			edPassword.Focus();
        }

		private void edConfirmPassword_TextChanged(object sender, System.EventArgs e)
		{
			btnOk.Enabled = (edPassword.Text.Equals(edConfirmPassword.Text)
				&& (edPassword.Text.Length > 0));
		}
    } 
}
