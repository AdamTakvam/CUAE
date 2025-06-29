using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PGPFilesDemo
{
	/// <summary>
	/// Summary description for SignaturesForm.
	/// </summary>
	public class frmSignatures : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListView lvSignatures;
		private System.Windows.Forms.ColumnHeader chSigner;
		private System.Windows.Forms.ColumnHeader chValidity;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSignatures()
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
			this.lvSignatures = new System.Windows.Forms.ListView();
			this.chSigner = new System.Windows.Forms.ColumnHeader();
			this.chValidity = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// lvSignatures
			// 
			this.lvSignatures.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						   this.chSigner,
																						   this.chValidity});
			this.lvSignatures.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvSignatures.FullRowSelect = true;
			this.lvSignatures.Location = new System.Drawing.Point(0, 0);
			this.lvSignatures.Name = "lvSignatures";
			this.lvSignatures.Size = new System.Drawing.Size(434, 119);
			this.lvSignatures.TabIndex = 0;
			this.lvSignatures.View = System.Windows.Forms.View.Details;
			// 
			// chSigner
			// 
			this.chSigner.Text = "Signer";
			this.chSigner.Width = 200;
			// 
			// chValidity
			// 
			this.chValidity.Text = "Validity";
			this.chValidity.Width = 200;
			// 
			// frmSignatures
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(434, 119);
			this.Controls.Add(this.lvSignatures);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmSignatures";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Signatures";
			this.ResumeLayout(false);

		}
		#endregion

		public void Init(SBPGPKeys.TElPGPSignature[] Signatures, SBPGPStreams.TSBPGPSignatureValidity[] Validities, SBPGPKeys.TElPGPKeyring keyring)
		{
			int i, index;
			ListViewItem item;
			SBPGPKeys.TElPGPCustomPublicKey key = null;
			SBPGPKeys.TElPGPPublicKey mainKey;
			string userID, sigVal;
			
			lvSignatures.Items.Clear();
			for (i = 0; i < Signatures.Length; i++) 
			{
				item = lvSignatures.Items.Add("");
				index = keyring.FindPublicKeyByID(Signatures[i].SignerKeyID(), ref key, 0);
				if (key != null) 
				{
					if (key is SBPGPKeys.TElPGPPublicKey) 
					{
						mainKey = (SBPGPKeys.TElPGPPublicKey)key;
					} 
					else 
					{
						// retrieving supkey...
						mainKey = null;
					}
					if (mainKey != null)
					{
						if (mainKey.UserIDCount > 0) 
						{
							userID = mainKey.get_UserIDs(0).Name;
						} 
						else 
						{
							userID = "No name";
						}
					} 
					else 
					{
						userID = "Unknown Key";
					}
				} 
				else 
				{
					userID = "Unknown Key";
				}
				item.Text = userID;
				switch(Validities[i]) 
				{
					case SBPGPStreams.TSBPGPSignatureValidity.svCorrupted :
						sigVal = "Corrupted";
						break;
					case SBPGPStreams.TSBPGPSignatureValidity.svNoKey :
						sigVal = "Signing key not found, unable to verify";
						break;
					case SBPGPStreams.TSBPGPSignatureValidity.svUnknownAlgorithm :
						sigVal = "Unknown signing algorithm";
						break;
					case SBPGPStreams.TSBPGPSignatureValidity.svValid :
						sigVal = "Valid";
						break;
					default:
						sigVal = "Unknown reason";
						break;
				}
				item.SubItems.Add(sigVal);
			}
		}
	}
}
