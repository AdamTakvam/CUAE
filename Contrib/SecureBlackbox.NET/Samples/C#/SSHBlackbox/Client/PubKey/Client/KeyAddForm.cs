using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PubKeyCliDemo
{
	/// <summary>
	/// Summary description for KeyAddForm.
	/// </summary>
	public class frmAddKey : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbAddKey;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblLocation;
		public System.Windows.Forms.TextBox tbLocation;
		private System.Windows.Forms.Button btnBrowse;
		public System.Windows.Forms.CheckBox cbOverwrite;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmAddKey()
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
			this.gbAddKey = new System.Windows.Forms.GroupBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblLocation = new System.Windows.Forms.Label();
			this.tbLocation = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.cbOverwrite = new System.Windows.Forms.CheckBox();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.gbAddKey.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbAddKey
			// 
			this.gbAddKey.Controls.Add(this.cbOverwrite);
			this.gbAddKey.Controls.Add(this.btnBrowse);
			this.gbAddKey.Controls.Add(this.tbLocation);
			this.gbAddKey.Controls.Add(this.lblLocation);
			this.gbAddKey.Location = new System.Drawing.Point(8, 8);
			this.gbAddKey.Name = "gbAddKey";
			this.gbAddKey.Size = new System.Drawing.Size(352, 104);
			this.gbAddKey.TabIndex = 0;
			this.gbAddKey.TabStop = false;
			this.gbAddKey.Text = "Add key";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(112, 120);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(192, 120);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			// 
			// lblLocation
			// 
			this.lblLocation.Location = new System.Drawing.Point(16, 24);
			this.lblLocation.Name = "lblLocation";
			this.lblLocation.Size = new System.Drawing.Size(136, 16);
			this.lblLocation.TabIndex = 0;
			this.lblLocation.Text = "Key location:";
			// 
			// tbLocation
			// 
			this.tbLocation.Location = new System.Drawing.Point(16, 40);
			this.tbLocation.Name = "tbLocation";
			this.tbLocation.Size = new System.Drawing.Size(240, 21);
			this.tbLocation.TabIndex = 1;
			this.tbLocation.Text = "";
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(264, 40);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.TabIndex = 2;
			this.btnBrowse.Text = "Browse...";
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// cbOverwrite
			// 
			this.cbOverwrite.Location = new System.Drawing.Point(16, 72);
			this.cbOverwrite.Name = "cbOverwrite";
			this.cbOverwrite.Size = new System.Drawing.Size(208, 24);
			this.cbOverwrite.TabIndex = 3;
			this.cbOverwrite.Text = "Overwrite if exists";
			// 
			// frmAddKey
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(370, 151);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.gbAddKey);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAddKey";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Add key";
			this.gbAddKey.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
			if (openFileDialog.ShowDialog() == DialogResult.OK) 
			{
				tbLocation.Text = openFileDialog.FileName;
			}
		}
	}
}
