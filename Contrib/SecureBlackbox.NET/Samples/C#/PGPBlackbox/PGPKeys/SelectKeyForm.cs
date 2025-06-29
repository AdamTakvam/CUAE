using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SBPGPKeys;

namespace PGPKeysDemo
{
	/// <summary>
	/// Summary description for SelectKeyForm.
	/// </summary>
	public class frmSelectKey : System.Windows.Forms.Form
	{
		public System.Windows.Forms.ListView lvKeys;
		private System.Windows.Forms.Label lSelectKey;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ColumnHeader chUserID;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSelectKey()
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
			this.lvKeys = new System.Windows.Forms.ListView();
			this.chUserID = new System.Windows.Forms.ColumnHeader();
			this.lSelectKey = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lvKeys
			// 
			this.lvKeys.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					 this.chUserID});
			this.lvKeys.FullRowSelect = true;
			this.lvKeys.Location = new System.Drawing.Point(8, 24);
			this.lvKeys.Name = "lvKeys";
			this.lvKeys.Size = new System.Drawing.Size(352, 200);
			this.lvKeys.TabIndex = 0;
			this.lvKeys.View = System.Windows.Forms.View.Details;
			// 
			// chUserID
			// 
			this.chUserID.Text = "User";
			this.chUserID.Width = 300;
			// 
			// lSelectKey
			// 
			this.lSelectKey.Location = new System.Drawing.Point(8, 8);
			this.lSelectKey.Name = "lSelectKey";
			this.lSelectKey.Size = new System.Drawing.Size(352, 16);
			this.lSelectKey.TabIndex = 1;
			this.lSelectKey.Text = "Please select a key:";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(104, 240);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(184, 240);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			// 
			// frmSelectKey
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(370, 279);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lSelectKey);
			this.Controls.Add(this.lvKeys);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmSelectKey";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Key selection";
			this.ResumeLayout(false);

		}
		#endregion

		public void Init(TElPGPKeyring keys, ImageList imgs)
		{
			int i;
			ListViewItem item;

			lvKeys.Items.Clear();
			lvKeys.SmallImageList = imgs;
			for(i = 0; i < keys.SecretCount; i++) 
			{
				item = lvKeys.Items.Add(GetDefaultUserID(keys.get_SecretKeys(i).PublicKey));
                item.Tag = keys.get_SecretKeys(i);
				if ((keys.get_SecretKeys(i).PublicKeyAlgorithm == SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA) ||
					(keys.get_SecretKeys(i).PublicKeyAlgorithm == SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA_SIGN) ||
					(keys.get_SecretKeys(i).PublicKeyAlgorithm == SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA_ENCRYPT)) 
				{
					item.ImageIndex = 1;
				} 
				else 
				{
					item.ImageIndex = 0;
				}
			}

		}

		private string GetDefaultUserID(TElPGPPublicKey key)
		{
			string result;
			if (key.UserIDCount > 0) 
			{
				result = key.get_UserIDs(0).Name;
			} 
			else 
			{
				result = "No name";
			}
			return result;
		}

	}
}
