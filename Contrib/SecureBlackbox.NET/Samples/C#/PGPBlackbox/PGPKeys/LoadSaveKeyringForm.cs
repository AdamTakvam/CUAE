using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PGPKeysDemo
{
	/// <summary>
	/// Summary description for LoadSaveKeyringForm.
	/// </summary>
	public class frmLoadSaveKeyring : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lPublicKeyring;
		private System.Windows.Forms.Label lSecretKeyring;
		public System.Windows.Forms.TextBox tbPublicKeyring;
		public System.Windows.Forms.TextBox tbSecretKeyring;
		private System.Windows.Forms.Button btnBrowsePublic;
		private System.Windows.Forms.Button btnBrowseSecret;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		public bool OpenDialog = false;

		public frmLoadSaveKeyring()
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
			this.lPublicKeyring = new System.Windows.Forms.Label();
			this.lSecretKeyring = new System.Windows.Forms.Label();
			this.tbPublicKeyring = new System.Windows.Forms.TextBox();
			this.tbSecretKeyring = new System.Windows.Forms.TextBox();
			this.btnBrowsePublic = new System.Windows.Forms.Button();
			this.btnBrowseSecret = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.SuspendLayout();
			// 
			// lPublicKeyring
			// 
			this.lPublicKeyring.Location = new System.Drawing.Point(16, 16);
			this.lPublicKeyring.Name = "lPublicKeyring";
			this.lPublicKeyring.Size = new System.Drawing.Size(100, 16);
			this.lPublicKeyring.TabIndex = 0;
			this.lPublicKeyring.Text = "Public keyring:";
			// 
			// lSecretKeyring
			// 
			this.lSecretKeyring.Location = new System.Drawing.Point(16, 64);
			this.lSecretKeyring.Name = "lSecretKeyring";
			this.lSecretKeyring.Size = new System.Drawing.Size(100, 16);
			this.lSecretKeyring.TabIndex = 1;
			this.lSecretKeyring.Text = "Secret keyring:";
			// 
			// tbPublicKeyring
			// 
			this.tbPublicKeyring.Location = new System.Drawing.Point(16, 32);
			this.tbPublicKeyring.Name = "tbPublicKeyring";
			this.tbPublicKeyring.Size = new System.Drawing.Size(312, 21);
			this.tbPublicKeyring.TabIndex = 2;
			this.tbPublicKeyring.Text = "";
			// 
			// tbSecretKeyring
			// 
			this.tbSecretKeyring.Location = new System.Drawing.Point(16, 80);
			this.tbSecretKeyring.Name = "tbSecretKeyring";
			this.tbSecretKeyring.Size = new System.Drawing.Size(312, 21);
			this.tbSecretKeyring.TabIndex = 3;
			this.tbSecretKeyring.Text = "";
			// 
			// btnBrowsePublic
			// 
			this.btnBrowsePublic.Location = new System.Drawing.Point(328, 32);
			this.btnBrowsePublic.Name = "btnBrowsePublic";
			this.btnBrowsePublic.TabIndex = 4;
			this.btnBrowsePublic.Text = "Browse...";
			this.btnBrowsePublic.Click += new System.EventHandler(this.btnBrowsePublic_Click);
			// 
			// btnBrowseSecret
			// 
			this.btnBrowseSecret.Location = new System.Drawing.Point(328, 80);
			this.btnBrowseSecret.Name = "btnBrowseSecret";
			this.btnBrowseSecret.TabIndex = 5;
			this.btnBrowseSecret.Text = "Browse...";
			this.btnBrowseSecret.Click += new System.EventHandler(this.btnBrowseSecret_Click);
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(136, 120);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 6;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(216, 120);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			// 
			// frmLoadSaveKeyring
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(416, 157);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnBrowseSecret);
			this.Controls.Add(this.btnBrowsePublic);
			this.Controls.Add(this.tbSecretKeyring);
			this.Controls.Add(this.tbPublicKeyring);
			this.Controls.Add(this.lSecretKeyring);
			this.Controls.Add(this.lPublicKeyring);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmLoadSaveKeyring";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Load Keyring";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnBrowsePublic_Click(object sender, System.EventArgs e)
		{
			if (OpenDialog) 
			{
				if (openFileDialog.ShowDialog() == DialogResult.OK) 
				{
					tbPublicKeyring.Text = openFileDialog.FileName;
				}
			} 
			else 
			{
				if (saveFileDialog.ShowDialog() == DialogResult.OK) 
				{
					tbPublicKeyring.Text = saveFileDialog.FileName;
				}
			}
		}

		private void btnBrowseSecret_Click(object sender, System.EventArgs e)
		{
			if (OpenDialog) 
			{
				if (openFileDialog.ShowDialog() == DialogResult.OK) 
				{
					tbSecretKeyring.Text = openFileDialog.FileName;
				}
			} 
			else 
			{
				if (saveFileDialog.ShowDialog() == DialogResult.OK) 
				{
					tbSecretKeyring.Text = saveFileDialog.FileName;
				}
			}
		}
	}
}
