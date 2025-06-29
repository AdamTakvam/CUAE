using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

using SBPGPKeys;
using SBPGPConstants;
using SBPGPUtils;

namespace MimeViewer
{
	/// <summary>
	/// Summary description for MimeViewer_OptionsPGPMime.
	/// </summary>
	public class MimeViewer_OptionsPGPMime: MimeViewer.MimeViewer_PlugControl
	{
		private System.Windows.Forms.Panel pRight;
		private System.Windows.Forms.Panel pClient;
		private System.Windows.Forms.ImageList ImageList;
		private System.Windows.Forms.OpenFileDialog OpenDialog;
		private System.Windows.Forms.SaveFileDialog SaveDialog;
		private System.Windows.Forms.Label lbHeaderKeys;
		private System.Windows.Forms.TreeView tvKeys;
		private System.Windows.Forms.Button btnAddKey;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Button btnSaveKey;
		private System.Windows.Forms.Button btnRemoveKey;
		private System.Windows.Forms.Panel pKeyInfo;
		private System.Windows.Forms.Label lbKeyID;
		private System.Windows.Forms.Label lbKeyFP;
		private System.Windows.Forms.Label lbAlgorithm;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label Bevel;

		public static SBPGPKeys.TElPGPKeyring Keyring = null;

		public MimeViewer_OptionsPGPMime(System.ComponentModel.IContainer container)
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			container.Add(this);
			InitializeComponent();
			Init();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public MimeViewer_OptionsPGPMime()
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			InitializeComponent();
			Init();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		private void Init()
		{
			if (Keyring == null)
			{
				Keyring = new TElPGPKeyring();
				try
				{
					Keyring.Load("EldoS MIMEBlackbox Demo.asc", "", true);
				}
				catch
				{
					Keyring.Load("..\\..\\EldoS MIMEBlackbox Demo.asc", "", true);
				}
				RepaintKeyring();
			}
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


		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MimeViewer_OptionsPGPMime));
			this.pRight = new System.Windows.Forms.Panel();
			this.Bevel = new System.Windows.Forms.Label();
			this.btnRemoveKey = new System.Windows.Forms.Button();
			this.btnSaveKey = new System.Windows.Forms.Button();
			this.btnClear = new System.Windows.Forms.Button();
			this.btnAddKey = new System.Windows.Forms.Button();
			this.pClient = new System.Windows.Forms.Panel();
			this.pKeyInfo = new System.Windows.Forms.Panel();
			this.lbAlgorithm = new System.Windows.Forms.Label();
			this.lbKeyFP = new System.Windows.Forms.Label();
			this.lbKeyID = new System.Windows.Forms.Label();
			this.tvKeys = new System.Windows.Forms.TreeView();
			this.ImageList = new System.Windows.Forms.ImageList(this.components);
			this.lbHeaderKeys = new System.Windows.Forms.Label();
			this.OpenDialog = new System.Windows.Forms.OpenFileDialog();
			this.SaveDialog = new System.Windows.Forms.SaveFileDialog();
			this.pRight.SuspendLayout();
			this.pClient.SuspendLayout();
			this.pKeyInfo.SuspendLayout();
			this.SuspendLayout();
			// 
			// pRight
			// 
			this.pRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pRight.Controls.Add(this.Bevel);
			this.pRight.Controls.Add(this.btnRemoveKey);
			this.pRight.Controls.Add(this.btnSaveKey);
			this.pRight.Controls.Add(this.btnClear);
			this.pRight.Controls.Add(this.btnAddKey);
			this.pRight.Location = new System.Drawing.Point(703, 0);
			this.pRight.Name = "pRight";
			this.pRight.Size = new System.Drawing.Size(130, 413);
			this.pRight.TabIndex = 1;
			// 
			// Bevel
			// 
			this.Bevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Bevel.Location = new System.Drawing.Point(8, 112);
			this.Bevel.Name = "Bevel";
			this.Bevel.Size = new System.Drawing.Size(112, 3);
			this.Bevel.TabIndex = 7;
			// 
			// btnRemoveKey
			// 
			this.btnRemoveKey.Location = new System.Drawing.Point(4, 48);
			this.btnRemoveKey.Name = "btnRemoveKey";
			this.btnRemoveKey.Size = new System.Drawing.Size(120, 25);
			this.btnRemoveKey.TabIndex = 3;
			this.btnRemoveKey.Text = "Remove Key";
			this.btnRemoveKey.Click += new System.EventHandler(this.btnRemoveKey_Click);
			// 
			// btnSaveKey
			// 
			this.btnSaveKey.Location = new System.Drawing.Point(4, 80);
			this.btnSaveKey.Name = "btnSaveKey";
			this.btnSaveKey.Size = new System.Drawing.Size(120, 25);
			this.btnSaveKey.TabIndex = 2;
			this.btnSaveKey.Text = "Save Key";
			this.btnSaveKey.Click += new System.EventHandler(this.btnSaveKey_Click);
			// 
			// btnClear
			// 
			this.btnClear.Location = new System.Drawing.Point(4, 120);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(120, 25);
			this.btnClear.TabIndex = 1;
			this.btnClear.Text = "Clear Keyring";
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// btnAddKey
			// 
			this.btnAddKey.Location = new System.Drawing.Point(4, 16);
			this.btnAddKey.Name = "btnAddKey";
			this.btnAddKey.Size = new System.Drawing.Size(120, 25);
			this.btnAddKey.TabIndex = 0;
			this.btnAddKey.Text = "Add Key";
			this.btnAddKey.Click += new System.EventHandler(this.btnAddKey_Click);
			// 
			// pClient
			// 
			this.pClient.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pClient.Controls.Add(this.pKeyInfo);
			this.pClient.Controls.Add(this.tvKeys);
			this.pClient.Controls.Add(this.lbHeaderKeys);
			this.pClient.Location = new System.Drawing.Point(0, 0);
			this.pClient.Name = "pClient";
			this.pClient.Size = new System.Drawing.Size(702, 413);
			this.pClient.TabIndex = 2;
			// 
			// pKeyInfo
			// 
			this.pKeyInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pKeyInfo.Controls.Add(this.lbAlgorithm);
			this.pKeyInfo.Controls.Add(this.lbKeyFP);
			this.pKeyInfo.Controls.Add(this.lbKeyID);
			this.pKeyInfo.Location = new System.Drawing.Point(0, 321);
			this.pKeyInfo.Name = "pKeyInfo";
			this.pKeyInfo.Size = new System.Drawing.Size(702, 88);
			this.pKeyInfo.TabIndex = 2;
			// 
			// lbAlgorithm
			// 
			this.lbAlgorithm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lbAlgorithm.Location = new System.Drawing.Point(16, 64);
			this.lbAlgorithm.Name = "lbAlgorithm";
			this.lbAlgorithm.Size = new System.Drawing.Size(664, 13);
			this.lbAlgorithm.TabIndex = 2;
			this.lbAlgorithm.Text = "Algorithm:";
			// 
			// lbKeyFP
			// 
			this.lbKeyFP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lbKeyFP.Location = new System.Drawing.Point(16, 40);
			this.lbKeyFP.Name = "lbKeyFP";
			this.lbKeyFP.Size = new System.Drawing.Size(664, 13);
			this.lbKeyFP.TabIndex = 1;
			this.lbKeyFP.Text = "Key FP:";
			// 
			// lbKeyID
			// 
			this.lbKeyID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lbKeyID.Location = new System.Drawing.Point(16, 16);
			this.lbKeyID.Name = "lbKeyID";
			this.lbKeyID.Size = new System.Drawing.Size(664, 13);
			this.lbKeyID.TabIndex = 0;
			this.lbKeyID.Text = "Key ID:";
			// 
			// tvKeys
			// 
			this.tvKeys.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tvKeys.ImageList = this.ImageList;
			this.tvKeys.Location = new System.Drawing.Point(4, 24);
			this.tvKeys.Name = "tvKeys";
			this.tvKeys.Size = new System.Drawing.Size(694, 297);
			this.tvKeys.TabIndex = 1;
			this.tvKeys.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvKeys_AfterSelect);
			// 
			// ImageList
			// 
			this.ImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList.ImageStream")));
			this.ImageList.TransparentColor = System.Drawing.Color.Fuchsia;
			// 
			// lbHeaderKeys
			// 
			this.lbHeaderKeys.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lbHeaderKeys.Location = new System.Drawing.Point(0, 0);
			this.lbHeaderKeys.Name = "lbHeaderKeys";
			this.lbHeaderKeys.Size = new System.Drawing.Size(702, 23);
			this.lbHeaderKeys.TabIndex = 0;
			this.lbHeaderKeys.Text = "PGP Keys";
			this.lbHeaderKeys.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// OpenDialog
			// 
			this.OpenDialog.Filter = "PGP keys (*.asc, *.pkr, *.skr, *.gpg)|*.asc;*.pkr;*.skr;*.gpg;*.pgp";
			// 
			// MimeViewer_OptionsPGPMime
			// 
			this.Controls.Add(this.pClient);
			this.Controls.Add(this.pRight);
			this.Name = "MimeViewer_OptionsPGPMime";
			this.Size = new System.Drawing.Size(833, 413);
			this.pRight.ResumeLayout(false);
			this.pClient.ResumeLayout(false);
			this.pKeyInfo.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnAddKey_Click(object sender, System.EventArgs e)
		{
			TElPGPKeyring TempKeyring;
			if (OpenDialog.ShowDialog() == DialogResult.OK)
			{
				TempKeyring = new TElPGPKeyring();
				try
				{
					TempKeyring.Load(OpenDialog.FileName, "", true);
					TempKeyring.ExportTo(Keyring);
				}
				finally
				{
					TempKeyring.Dispose();
				}
				RepaintKeyring();
			}
		}		

		private void btnRemoveKey_Click(object sender, System.EventArgs e)
		{
			if (MessageBox.Show("Are you sure you wish to remove this key from keyring?", "MIME Viewer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
				return;

			if ((tvKeys.SelectedNode != null) && (tvKeys.SelectedNode.Tag != null) && (tvKeys.SelectedNode.Tag is TElPGPPublicKey))
			{
				if (((TElPGPPublicKey)tvKeys.SelectedNode.Tag).SecretKey != null) 
					Keyring.RemoveSecretKey(((TElPGPPublicKey)tvKeys.SelectedNode.Tag).SecretKey);
				else
					Keyring.RemovePublicKey(((TElPGPPublicKey)tvKeys.SelectedNode.Tag), false);

				RepaintKeyring();
			}
		}

		private void btnSaveKey_Click(object sender, System.EventArgs e)
		{
			if ((tvKeys.SelectedNode != null) && (tvKeys.SelectedNode.Tag != null) && (tvKeys.SelectedNode.Tag is TElPGPPublicKey))
			{
				if (SaveDialog.ShowDialog() != DialogResult.OK) 
					return;

				if (((TElPGPPublicKey)tvKeys.SelectedNode.Tag).SecretKey != null) 
					((TElPGPPublicKey)tvKeys.SelectedNode.Tag).SecretKey.SaveToFile(SaveDialog.FileName, false);
				else
					((TElPGPPublicKey)tvKeys.SelectedNode.Tag).SaveToFile(SaveDialog.FileName, false);

				MessageBox.Show("Key successfully saved", "MIME Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}		
		}

		private void btnClear_Click(object sender, System.EventArgs e)
		{
			Keyring.Clear();
			RepaintKeyring();
		}

		private void RepaintKeyring()
		{
			tvKeys.BeginUpdate();
			try
			{
				tvKeys.Nodes.Clear();

				int i, j, k;
				string S;
				TreeNode Node, SubNode, SigNode;

				for (i = 0; i < Keyring.PublicCount; i++)
				{
					if (Keyring.get_PublicKeys(i).UserIDCount > 0)
						S = Keyring.get_PublicKeys(i).get_UserIDs(0).Name;
					else
						S = "<unnamed key>";

					Node = tvKeys.Nodes.Add(S);
					Node.Tag = Keyring.get_PublicKeys(i);
					if ((Keyring.get_PublicKeys(i).PublicKeyAlgorithm & (SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_DSA + SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_ELGAMAL)) > 0)
						Node.ImageIndex = 0;
					else
						Node.ImageIndex = 1;

					Node.SelectedImageIndex = Node.ImageIndex;

					for (j = 0; j < Keyring.get_PublicKeys(i).UserIDCount; j++)
					{
						SubNode = Node.Nodes.Add(Keyring.get_PublicKeys(i).get_UserIDs(j).Name);
						SubNode.Tag = Keyring.get_PublicKeys(i).get_UserIDs(j);
						SubNode.ImageIndex = 2;
						SubNode.SelectedImageIndex = 2;
						for (k = 0; k < Keyring.get_PublicKeys(i).get_UserIDs(j).SignatureCount; k++)
						{
							SigNode = SubNode.Nodes.Add("signature");
							SigNode.Tag = Keyring.get_PublicKeys(i).get_UserIDs(j).get_Signatures(k);
							SigNode.ImageIndex = 3;
							SigNode.SelectedImageIndex = 3;
						}
					}

					for (j = 0; j < Keyring.get_PublicKeys(i).SubkeyCount; j++)
					{
						SubNode = Node.Nodes.Add("Subkey");
						SubNode.Tag = Keyring.get_PublicKeys(i).get_Subkeys(j);
						SubNode.ImageIndex = Node.ImageIndex;
						SubNode.SelectedImageIndex = Node.ImageIndex;
						for (k = 0; k < Keyring.get_PublicKeys(i).get_Subkeys(j).SignatureCount; k++)
						{
							SigNode = SubNode.Nodes.Add("signature");
							SigNode.Tag = Keyring.get_PublicKeys(i).get_Subkeys(j).get_Signatures(k);
							SigNode.ImageIndex = 3;
							SigNode.SelectedImageIndex = 3;
						}
					}
				}
			}
			finally
			{
				tvKeys.EndUpdate();
			}
		}

		private void tvKeys_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if ((e.Node != null) && (e.Node.Tag != null))
			{
				if ((e.Node.Tag is TElPGPPublicKey) || (e.Node.Tag is TElPGPPublicSubkey))
				{
					TElPGPCustomPublicKey PubKey = (TElPGPCustomPublicKey)e.Node.Tag;
					lbKeyID.Text = "Key ID: " + SBPGPUtils.Unit.KeyID2Str(PubKey.KeyID(), false);
					lbKeyFP.Text = "Key FP: " + SBPGPUtils.Unit.KeyFP2Str(PubKey.KeyFP());
					lbAlgorithm.Text = "Algorithm: " + SBPGPUtils.Unit.PKAlg2Str(PubKey.PublicKeyAlgorithm) +
						" (" + PubKey.BitsInKey.ToString() + " bits)";
					return;
				}
				else if (e.Node.Tag is TElPGPSignature)
				{
					lbKeyID.Text = "Signing Key ID: " + SBPGPUtils.Unit.KeyID2Str(((TElPGPSignature)e.Node.Tag).SignerKeyID(), false);
					lbKeyFP.Text = "";
					lbAlgorithm.Text = "";
					return;
				}
			}

			lbKeyID.Text = "";
			lbKeyFP.Text = "";
			lbAlgorithm.Text = "";
		}

		public override string GetCaption()
		{
			return "PGP/MIME Options";
		}

	}
}
