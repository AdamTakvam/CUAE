using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PGPFilesDemo
{
	/// <summary>
	/// Summary description for PassRequestForm.
	/// </summary>
	public class frmPassRequest : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lPrompt;
		public System.Windows.Forms.TextBox tbPassphrase;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lKeyInfo;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmPassRequest()
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
			this.tbPassphrase = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lKeyInfo = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lPrompt
			// 
			this.lPrompt.Location = new System.Drawing.Point(8, 8);
			this.lPrompt.Name = "lPrompt";
			this.lPrompt.Size = new System.Drawing.Size(264, 16);
			this.lPrompt.TabIndex = 0;
			this.lPrompt.Text = "Passphrase is needed for secret key:";
			this.lPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tbPassphrase
			// 
			this.tbPassphrase.Location = new System.Drawing.Point(8, 48);
			this.tbPassphrase.Name = "tbPassphrase";
			this.tbPassphrase.PasswordChar = '*';
			this.tbPassphrase.Size = new System.Drawing.Size(344, 21);
			this.tbPassphrase.TabIndex = 1;
			this.tbPassphrase.Text = "";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(104, 80);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(184, 80);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			// 
			// lKeyInfo
			// 
			this.lKeyInfo.Location = new System.Drawing.Point(8, 24);
			this.lKeyInfo.Name = "lKeyInfo";
			this.lKeyInfo.Size = new System.Drawing.Size(344, 16);
			this.lKeyInfo.TabIndex = 4;
			// 
			// frmPassRequest
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(362, 111);
			this.Controls.Add(this.lKeyInfo);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.tbPassphrase);
			this.Controls.Add(this.lPrompt);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmPassRequest";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Passphrase request";
			this.ResumeLayout(false);

		}
		#endregion

		public void Init(SBPGPKeys.TElPGPCustomSecretKey key)
		{
			string userName;
			if (key != null) 
			{
				if (key is SBPGPKeys.TElPGPSecretKey) 
				{
					if (((SBPGPKeys.TElPGPSecretKey)key).PublicKey.UserIDCount > 0) 
					{
						userName = ((SBPGPKeys.TElPGPSecretKey)key).PublicKey.get_UserIDs(0).Name;
					} 
					else 
					{
						userName = "<no name>";
					}
				} 
				else 
				{
					userName = "Subkey";
				}
				lPrompt.Text = "Passphrase is needed for secret key:";
				lKeyInfo.Text = userName + " (ID=0x" + SBPGPUtils.Unit.KeyID2Str(key.KeyID(), true) + ")";
			} 
			else 
			{
				lPrompt.Text = "Passphrase is needed to decrypt the message";
				lKeyInfo.Text = "";
			}
		}
	}
}
