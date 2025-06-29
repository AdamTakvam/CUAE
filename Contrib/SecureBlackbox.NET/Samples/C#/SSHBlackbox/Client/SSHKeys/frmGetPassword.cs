using System;
using System.Windows.Forms;
namespace SSHKeys
{
    /// <summary>
    /// Used to enter private key password
    /// </summary>
	public class TfrmGetPassword : Form
    {
        public PictureBox imgKeys = null;
        public TextBox edPassword = null;
        public Label lblPassword = null;
        public Button btnOk = null;
		private System.Windows.Forms.ToolTip ttMain;
        public Button btnCancel = null;
        public TfrmGetPassword()
        {
            InitializeComponent();
        }

		private System.ComponentModel.IContainer components;

#region Windows Form Designer generated code
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TfrmGetPassword));
			this.ttMain = new System.Windows.Forms.ToolTip(this.components);
			this.edPassword = new System.Windows.Forms.TextBox();
			this.imgKeys = new System.Windows.Forms.PictureBox();
			this.lblPassword = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// edPassword
			// 
			this.edPassword.Location = new System.Drawing.Point(48, 18);
			this.edPassword.Name = "edPassword";
			this.edPassword.PasswordChar = '*';
			this.edPassword.Size = new System.Drawing.Size(161, 20);
			this.edPassword.TabIndex = 0;
			this.edPassword.Text = "";
			this.ttMain.SetToolTip(this.edPassword, "Enter your password for private key here");
			// 
			// imgKeys
			// 
			this.imgKeys.Image = ((System.Drawing.Image)(resources.GetObject("imgKeys.Image")));
			this.imgKeys.Location = new System.Drawing.Point(8, 8);
			this.imgKeys.Name = "imgKeys";
			this.imgKeys.Size = new System.Drawing.Size(32, 32);
			this.imgKeys.TabIndex = 0;
			this.imgKeys.TabStop = false;
			// 
			// lblPassword
			// 
			this.lblPassword.Location = new System.Drawing.Point(55, 1);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(145, 13);
			this.lblPassword.TabIndex = 1;
			this.lblPassword.Text = "Enter private key password";
			// 
			// btnOk
			// 
			this.btnOk.BackColor = System.Drawing.SystemColors.Control;
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(16, 46);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(47, 21);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "O&k";
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(156, 46);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(47, 21);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "&Cancel";
			// 
			// TfrmGetPassword
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoScroll = true;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(217, 69);
			this.Controls.Add(this.imgKeys);
			this.Controls.Add(this.lblPassword);
			this.Controls.Add(this.edPassword);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.KeyPreview = true;
			this.Location = new System.Drawing.Point(320, 225);
			this.Name = "TfrmGetPassword";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Password";
			this.ttMain.SetToolTip(this, "Enter password for user authentication");
			this.Activated += new System.EventHandler(this.FormActivate);
			this.ResumeLayout(false);

		}
#endregion

        /// <summary>
        /// Used to get/set password for private key
        /// </summary>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public static bool GetPassword(ref string Pwd)
        {
	        TfrmGetPassword frm;
            frm = new TfrmGetPassword();
            if (frm == null) return false;
			if (frm.ShowDialog()==DialogResult.OK)
            {
                Pwd = frm.edPassword.Text;
				frm.Dispose();
				return true;
            }
			else {frm.Dispose(); return false;}
        }

        public void FormActivate(System.Object Sender, System.EventArgs _e1)
        {
            edPassword.Text = "";
			edPassword.Focus();
        }

    } 

}
