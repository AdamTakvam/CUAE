using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace TinyProcessor
{
	/// <summary>
	/// Used to query password 
	/// </summary>
	public class frmRequestPassword : System.Windows.Forms.Form
	{
		public System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.Button btnOK;
		public System.Windows.Forms.Label lPrompt;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmRequestPassword()
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
		/// Require user password. In a case of empty password retuen ""
		/// </summary>
		/// <param name="Prompt">Prompt to password form</param>
		/// <returns></returns>
		public static string RequestPassword(string Prompt) 
		{
			frmRequestPassword dlg = new frmRequestPassword();
			dlg.lPrompt.Text = Prompt;
			if (dlg.ShowDialog() == DialogResult.OK) 
				return dlg.tbPassword.Text;
			else 
				return "";
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
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.lPrompt = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tbPassword
			// 
			this.tbPassword.Location = new System.Drawing.Point(8, 24);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.PasswordChar = '*';
			this.tbPassword.Size = new System.Drawing.Size(304, 21);
			this.tbPassword.TabIndex = 0;
			this.tbPassword.Text = "";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(80, 56);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			// 
			// lPrompt
			// 
			this.lPrompt.Location = new System.Drawing.Point(8, 8);
			this.lPrompt.Name = "lPrompt";
			this.lPrompt.Size = new System.Drawing.Size(304, 16);
			this.lPrompt.TabIndex = 2;
			this.lPrompt.Text = "Please enter password";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(160, 56);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			// 
			// frmRequestPassword
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(320, 93);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lPrompt);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.tbPassword);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmRequestPassword";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Password request";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
