using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PGPKeysDemo
{
	/// <summary>
	/// Summary description for PassphraseRequestForm.
	/// </summary>
	public class frmPassphraseRequest : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lPrompt;
		public System.Windows.Forms.Label lKeyID;
		public System.Windows.Forms.TextBox tbPassphrase;
		private System.Windows.Forms.Button btnOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmPassphraseRequest()
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
			this.lPrompt = new System.Windows.Forms.Label();
			this.lKeyID = new System.Windows.Forms.Label();
			this.tbPassphrase = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lPrompt
			// 
			this.lPrompt.Location = new System.Drawing.Point(8, 8);
			this.lPrompt.Name = "lPrompt";
			this.lPrompt.Size = new System.Drawing.Size(248, 16);
			this.lPrompt.TabIndex = 0;
			this.lPrompt.Text = "Please enter passphrase for the following key:";
			// 
			// lKeyID
			// 
			this.lKeyID.Location = new System.Drawing.Point(8, 24);
			this.lKeyID.Name = "lKeyID";
			this.lKeyID.Size = new System.Drawing.Size(312, 16);
			this.lKeyID.TabIndex = 1;
			// 
			// tbPassphrase
			// 
			this.tbPassphrase.Location = new System.Drawing.Point(8, 48);
			this.tbPassphrase.Name = "tbPassphrase";
			this.tbPassphrase.PasswordChar = '*';
			this.tbPassphrase.Size = new System.Drawing.Size(304, 21);
			this.tbPassphrase.TabIndex = 2;
			this.tbPassphrase.Text = "";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(120, 80);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "OK";
			// 
			// frmPassphraseRequest
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btnOK;
			this.ClientSize = new System.Drawing.Size(322, 111);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.tbPassphrase);
			this.Controls.Add(this.lKeyID);
			this.Controls.Add(this.lPrompt);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmPassphraseRequest";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Passphrase request";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
