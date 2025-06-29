using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SBPGPKeys;

namespace PGPKeysDemo
{
	/// <summary>
	/// Summary description for ImportKeyForm.
	/// </summary>
	public class frmImportKey : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lHint;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		public System.Windows.Forms.TreeView tvKeys;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmImportKey()
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
			this.tvKeys = new System.Windows.Forms.TreeView();
			this.lHint = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tvKeys
			// 
			this.tvKeys.ImageIndex = -1;
			this.tvKeys.Location = new System.Drawing.Point(8, 24);
			this.tvKeys.Name = "tvKeys";
			this.tvKeys.SelectedImageIndex = -1;
			this.tvKeys.Size = new System.Drawing.Size(312, 176);
			this.tvKeys.TabIndex = 0;
			// 
			// lHint
			// 
			this.lHint.Location = new System.Drawing.Point(8, 8);
			this.lHint.Name = "lHint";
			this.lHint.Size = new System.Drawing.Size(248, 16);
			this.lHint.TabIndex = 1;
			this.lHint.Text = "The following keys will be imported:";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(88, 208);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(168, 208);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			// 
			// frmImportKey
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(330, 247);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lHint);
			this.Controls.Add(this.tvKeys);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmImportKey";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Import key";
			this.ResumeLayout(false);

		}
		#endregion

		public void Init(TElPGPKeyring keyring, ImageList imgList) 
		{
			tvKeys.ImageList = imgList;
			Utils.RedrawKeyring(tvKeys, keyring);
		}
	}
}
