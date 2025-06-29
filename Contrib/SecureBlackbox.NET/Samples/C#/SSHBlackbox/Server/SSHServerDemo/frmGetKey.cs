using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SSHServer.NET
{
	/// <summary>
	/// Responsible for retrieving public key information from user
	/// </summary>
	public class frmGetKey : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblPrompt;
		private System.Windows.Forms.TextBox edKey;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmGetKey()
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

		public static bool GetKey(ref string key) 
		{
			frmGetKey frm = new frmGetKey();
			if (frm == null) 
			{
				return false;
			}
			if (frm.ShowDialog() == DialogResult.OK)
			{
				key = frm.edKey.Text;
				frm.Dispose();
				return true;
			}
			else 
			{
				frm.Dispose(); 
				return false;
			}
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblPrompt = new System.Windows.Forms.Label();
			this.edKey = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(64, 160);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(144, 160);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			// 
			// lblPrompt
			// 
			this.lblPrompt.Location = new System.Drawing.Point(8, 8);
			this.lblPrompt.Name = "lblPrompt";
			this.lblPrompt.Size = new System.Drawing.Size(240, 16);
			this.lblPrompt.TabIndex = 3;
			this.lblPrompt.Text = "Please insert your public key:";
			// 
			// edKey
			// 
			this.edKey.Location = new System.Drawing.Point(8, 24);
			this.edKey.Multiline = true;
			this.edKey.Name = "edKey";
			this.edKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.edKey.Size = new System.Drawing.Size(256, 128);
			this.edKey.TabIndex = 4;
			this.edKey.Text = "";
			// 
			// frmGetKey
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(274, 191);
			this.Controls.Add(this.edKey);
			this.Controls.Add(this.lblPrompt);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmGetKey";
			this.Text = "Set private key";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
