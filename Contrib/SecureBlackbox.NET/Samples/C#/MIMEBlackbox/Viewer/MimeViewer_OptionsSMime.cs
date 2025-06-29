using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Windows.Forms;

using SBCustomCertStorage;
using SBWinCertStorage;
using SBX509;
using SBPKCS12;
using SBMIMEStream;
using SBSMIMECore;

namespace MimeViewer
{
	/// <summary>
	/// Summary description for MimeViewer_OptionsSMime.
	/// </summary>
	public class MimeViewer_OptionsSMime: MimeViewer.MimeViewer_PlugControl
	{
		private System.Windows.Forms.Panel pCSTop;
		private System.Windows.Forms.CheckBox cbCustCert;
		private System.Windows.Forms.CheckBox cbWinCert;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Panel pCertOther;
		private System.Windows.Forms.Button btnAddCert;
		private System.Windows.Forms.Button btnDeleteCert;
		private System.Windows.Forms.Button btnSaveCert;
		private System.Windows.Forms.Button btnViewCert;
		private System.Windows.Forms.Panel pCSR;
		private System.Windows.Forms.Button btnLoadStorage;
		private System.Windows.Forms.Button btnSaveStorage;
		private System.Windows.Forms.OpenFileDialog OpenDialog;
		private System.Windows.Forms.SaveFileDialog SaveDialog;
		private System.Windows.Forms.ListView lvCert;

		public static bool UseWinCertStorage = true;
		public static bool UseUserCertStorage = true;
		public static TElFileCertStorage UserCertStorage = null;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.Label Bevel;
		public static TElMemoryCertStorage CurCertStorage = null;

		public MimeViewer_OptionsSMime()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			Init();

			// TODO: Add any initialization after the InitializeComponent call

		}

		private void Init()
		{
			UserCertStorage = new TElFileCertStorage();
			CurCertStorage = new TElMemoryCertStorage();

			cbCustCert_CheckedChanged(null, null);
			cbWinCert_CheckedChanged(null, null);

			// Load default user storage
			LoadStorage("CerStorageDef.ucs");
			LoadStorage("..\\..\\CerStorageDef.ucs");
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
			this.pCSTop = new System.Windows.Forms.Panel();
			this.cbWinCert = new System.Windows.Forms.CheckBox();
			this.cbCustCert = new System.Windows.Forms.CheckBox();
			this.pCertOther = new System.Windows.Forms.Panel();
			this.pCSR = new System.Windows.Forms.Panel();
			this.btnSaveStorage = new System.Windows.Forms.Button();
			this.btnLoadStorage = new System.Windows.Forms.Button();
			this.btnViewCert = new System.Windows.Forms.Button();
			this.btnSaveCert = new System.Windows.Forms.Button();
			this.btnDeleteCert = new System.Windows.Forms.Button();
			this.btnAddCert = new System.Windows.Forms.Button();
			this.lvCert = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.OpenDialog = new System.Windows.Forms.OpenFileDialog();
			this.SaveDialog = new System.Windows.Forms.SaveFileDialog();
			this.Bevel = new System.Windows.Forms.Label();
			this.pCSTop.SuspendLayout();
			this.pCertOther.SuspendLayout();
			this.pCSR.SuspendLayout();
			this.SuspendLayout();
			// 
			// pCSTop
			// 
			this.pCSTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pCSTop.Controls.Add(this.cbWinCert);
			this.pCSTop.Controls.Add(this.cbCustCert);
			this.pCSTop.Location = new System.Drawing.Point(0, 0);
			this.pCSTop.Name = "pCSTop";
			this.pCSTop.Size = new System.Drawing.Size(833, 26);
			this.pCSTop.TabIndex = 0;
			// 
			// cbWinCert
			// 
			this.cbWinCert.Checked = true;
			this.cbWinCert.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbWinCert.Location = new System.Drawing.Point(230, 5);
			this.cbWinCert.Name = "cbWinCert";
			this.cbWinCert.Size = new System.Drawing.Size(168, 17);
			this.cbWinCert.TabIndex = 1;
			this.cbWinCert.Text = "Use Windows certificates";
			this.cbWinCert.CheckedChanged += new System.EventHandler(this.cbWinCert_CheckedChanged);
			// 
			// cbCustCert
			// 
			this.cbCustCert.Checked = true;
			this.cbCustCert.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbCustCert.Location = new System.Drawing.Point(16, 4);
			this.cbCustCert.Name = "cbCustCert";
			this.cbCustCert.Size = new System.Drawing.Size(168, 17);
			this.cbCustCert.TabIndex = 0;
			this.cbCustCert.Text = "Use Custom certificates";
			this.cbCustCert.CheckedChanged += new System.EventHandler(this.cbCustCert_CheckedChanged);
			// 
			// pCertOther
			// 
			this.pCertOther.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pCertOther.Controls.Add(this.pCSR);
			this.pCertOther.Controls.Add(this.lvCert);
			this.pCertOther.Location = new System.Drawing.Point(0, 26);
			this.pCertOther.Name = "pCertOther";
			this.pCertOther.Size = new System.Drawing.Size(833, 390);
			this.pCertOther.TabIndex = 1;
			// 
			// pCSR
			// 
			this.pCSR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pCSR.Controls.Add(this.Bevel);
			this.pCSR.Controls.Add(this.btnSaveStorage);
			this.pCSR.Controls.Add(this.btnLoadStorage);
			this.pCSR.Controls.Add(this.btnViewCert);
			this.pCSR.Controls.Add(this.btnSaveCert);
			this.pCSR.Controls.Add(this.btnDeleteCert);
			this.pCSR.Controls.Add(this.btnAddCert);
			this.pCSR.Location = new System.Drawing.Point(701, 0);
			this.pCSR.Name = "pCSR";
			this.pCSR.Size = new System.Drawing.Size(132, 416);
			this.pCSR.TabIndex = 1;
			// 
			// btnSaveStorage
			// 
			this.btnSaveStorage.Location = new System.Drawing.Point(4, 168);
			this.btnSaveStorage.Name = "btnSaveStorage";
			this.btnSaveStorage.Size = new System.Drawing.Size(123, 25);
			this.btnSaveStorage.TabIndex = 5;
			this.btnSaveStorage.Text = "Save Storage";
			this.btnSaveStorage.Click += new System.EventHandler(this.btnSaveStorage_Click);
			// 
			// btnLoadStorage
			// 
			this.btnLoadStorage.Location = new System.Drawing.Point(4, 136);
			this.btnLoadStorage.Name = "btnLoadStorage";
			this.btnLoadStorage.Size = new System.Drawing.Size(123, 25);
			this.btnLoadStorage.TabIndex = 4;
			this.btnLoadStorage.Text = "Load Storage";
			this.btnLoadStorage.Click += new System.EventHandler(this.btnLoadStorage_Click);
			// 
			// btnViewCert
			// 
			this.btnViewCert.Location = new System.Drawing.Point(4, 96);
			this.btnViewCert.Name = "btnViewCert";
			this.btnViewCert.Size = new System.Drawing.Size(123, 25);
			this.btnViewCert.TabIndex = 3;
			this.btnViewCert.Text = "View Details";
			this.btnViewCert.Click += new System.EventHandler(this.btnViewCert_Click);
			// 
			// btnSaveCert
			// 
			this.btnSaveCert.Location = new System.Drawing.Point(4, 64);
			this.btnSaveCert.Name = "btnSaveCert";
			this.btnSaveCert.Size = new System.Drawing.Size(123, 25);
			this.btnSaveCert.TabIndex = 2;
			this.btnSaveCert.Text = "Save Certificate";
			this.btnSaveCert.Click += new System.EventHandler(this.btnSaveCert_Click);
			// 
			// btnDeleteCert
			// 
			this.btnDeleteCert.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnDeleteCert.Location = new System.Drawing.Point(4, 34);
			this.btnDeleteCert.Name = "btnDeleteCert";
			this.btnDeleteCert.Size = new System.Drawing.Size(123, 25);
			this.btnDeleteCert.TabIndex = 1;
			this.btnDeleteCert.Text = "Delete Certificate";
			this.btnDeleteCert.Click += new System.EventHandler(this.btnDeleteCert_Click);
			// 
			// btnAddCert
			// 
			this.btnAddCert.Location = new System.Drawing.Point(4, 4);
			this.btnAddCert.Name = "btnAddCert";
			this.btnAddCert.Size = new System.Drawing.Size(123, 25);
			this.btnAddCert.TabIndex = 0;
			this.btnAddCert.Text = "Add Certificate";
			this.btnAddCert.Click += new System.EventHandler(this.btnAddCert_Click);
			// 
			// lvCert
			// 
			this.lvCert.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lvCert.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					 this.columnHeader1});
			this.lvCert.FullRowSelect = true;
			this.lvCert.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvCert.Location = new System.Drawing.Point(4, 4);
			this.lvCert.MultiSelect = false;
			this.lvCert.Name = "lvCert";
			this.lvCert.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lvCert.Size = new System.Drawing.Size(692, 379);
			this.lvCert.TabIndex = 0;
			this.lvCert.View = System.Windows.Forms.View.List;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "User Certificates Storage:";
			this.columnHeader1.Width = -1;
			// 
			// Bevel
			// 
			this.Bevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Bevel.Location = new System.Drawing.Point(8, 128);
			this.Bevel.Name = "Bevel";
			this.Bevel.Size = new System.Drawing.Size(112, 3);
			this.Bevel.TabIndex = 6;
			// 
			// MimeViewer_OptionsSMime
			// 
			this.Controls.Add(this.pCertOther);
			this.Controls.Add(this.pCSTop);
			this.Name = "MimeViewer_OptionsSMime";
			this.Size = new System.Drawing.Size(833, 416);
			this.pCSTop.ResumeLayout(false);
			this.pCertOther.ResumeLayout(false);
			this.pCSR.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void cbCustCert_CheckedChanged(object sender, System.EventArgs e)
		{
			UseUserCertStorage = cbCustCert.Checked;		
		}

		private void cbWinCert_CheckedChanged(object sender, System.EventArgs e)
		{
			UseWinCertStorage = cbWinCert.Checked;
		}

		private void btnAddCert_Click(object sender, System.EventArgs e)
		{
			OpenDialog.Filter = "Certificates (*.pfx;*.p7b;*.cer;*.bin)|*.pfx;*.p7b;*.cer;*.bin|All Files (*.*)|*.*";
			OpenDialog.FilterIndex = 1;
			if (OpenDialog.ShowDialog() == DialogResult.OK)
			{
				StringQueryDlg passwdDlg = new StringQueryDlg(true);
				passwdDlg.Text = "Enter password";
				passwdDlg.Description = "Please, enter passphrase:";
				string sPwd = "";
				if (passwdDlg.ShowDialog(this) == DialogResult.OK)
					sPwd = passwdDlg.TextBox;

				passwdDlg.Dispose();

				TElX509Certificate cer = SBSMIMECore.Unit.LoadCertificateFromFile(OpenDialog.FileName, sPwd);

				if (cer == null)
					MessageBox.Show("Error loading the certificate", "MIME Viewer", MessageBoxButtons.OK, MessageBoxIcon.Error);
				else
				{
					int idx = UserCertStorage.IndexOf(cer);
					if ((idx >= 0) && (cer.PrivateKeyExists) && (!UserCertStorage.get_Certificates(idx).PrivateKeyExists))
					{
						UserCertStorage.Remove(idx);
						idx = -1;
					}

					if (idx < 0)
					{
						UserCertStorage.Add(cer, true);

						RepaintCertificates();
					}
					else
						MessageBox.Show("Certificate is already in the list.", "MIME Viewer", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

			}
		}

		private void btnDeleteCert_Click(object sender, System.EventArgs e)
		{
			if (lvCert.SelectedItems.Count <= 0)
				return;

			TElX509Certificate cer;
			int idx;
			int i;
			for (i = 0; i < lvCert.SelectedItems.Count; i++)
			{
				cer = (TElX509Certificate)lvCert.SelectedItems[i].Tag;
				if (cer != null) 
				{
					idx = UserCertStorage.IndexOf(cer);
					UserCertStorage.Remove(idx);
				}
			}

			RepaintCertificates();
		}

		private void btnSaveCert_Click(object sender, System.EventArgs e)
		{
			if ((lvCert.SelectedItems.Count <= 0) || (lvCert.SelectedItems[0].Tag == null))
				return;

			SaveDialog.Filter = "Certificates (*.bin)|*.bin|All Files (*.*)|*.*";
			SaveDialog.FilterIndex = 1;
			if (SaveDialog.ShowDialog() == DialogResult.OK)
			{
				TAnsiStringStream sm = new TAnsiStringStream();
				TElX509Certificate cer;
				try
				{
					cer = (TElX509Certificate)lvCert.SelectedItems[0].Tag;

					bool SavePrivateKey = true;
					string sPwd = "";
					if (cer.PrivateKeyExists)
					{
						StringQueryDlg passwdDlg = new StringQueryDlg(true);
						passwdDlg.Text = "Certificate Password";
						passwdDlg.Description = "Please Enter Password if it needed:";
						if (passwdDlg.ShowDialog(this) == DialogResult.OK)
							sPwd = passwdDlg.TextBox;
						else
							SavePrivateKey = false;

						passwdDlg.Dispose();
					}

					if (SavePrivateKey)
					{
						cer.SaveToStreamPFX(sm, sPwd, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_RC4_128, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_RC4_128);
						sm.SaveToFile(SaveDialog.FileName);
					}
					else
					{
						cer.SaveToStream(sm);
						sm.SaveToFile(SaveDialog.FileName);
						if (cer.PrivateKeyExists)
							MessageBox.Show("Saved without private key", "MIME Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}

				}
				finally
				{
					sm.Close();
				}
			}
		}

		private void btnViewCert_Click(object sender, System.EventArgs e)
		{
			if ((lvCert.SelectedItems.Count <= 0) || (lvCert.SelectedItems[0].Tag == null))
				return;

			MimeViewer_CertDetails CertInfo = new MimeViewer_CertDetails();
			try
			{
				TElX509Certificate cer = (TElX509Certificate)lvCert.SelectedItems[0].Tag;
				CertInfo.SetCertificate(cer);
				CertInfo.ShowDialog();
			}
			finally
			{
				CertInfo.Dispose();
			}
		}

		private void btnLoadStorage_Click(object sender, System.EventArgs e)
		{
			OpenDialog.Filter = "Users Certificates Storage (*.ucs)|*.ucs|All Files (*.*)|*.*";
			OpenDialog.FilterIndex = 1;
			if (OpenDialog.ShowDialog() == DialogResult.OK)
				LoadStorage(OpenDialog.FileName);
		}

		private void btnSaveStorage_Click(object sender, System.EventArgs e)
		{
			SaveDialog.Filter = "Users Certificates Storage (*.ucs)|*.ucs|All Files (*.*)|*.*";
			SaveDialog.FilterIndex = 1;
			if (SaveDialog.ShowDialog() == DialogResult.OK)
				SaveStorage(SaveDialog.FileName);
		}

		private const string sDefCertPswdInCustStorage = "{37907B5C-B309-4AE4-AFD2-2EAE948EADA2}";

		public void LoadStorage(string sFileName)
		{
			UserCertStorage = null;
			UserCertStorage = new TElFileCertStorage();

			FileInfo fi = new FileInfo(sFileName);
			if (!fi.Exists)
				return;

			TAnsiStringStream sm = new TAnsiStringStream();
			sm.LoadFromFile(sFileName);
			try
			{
				sm.Position = 0;
				CheckSBB(UserCertStorage.LoadFromBufferPFX(sm.Memory, sDefCertPswdInCustStorage),
						 "Cannot load certificates from file storage: '" + sFileName + "'");

				RepaintCertificates();
			}
			finally
			{
				sm.Close();
			}
		}

		public void SaveStorage(string sFileName)
		{
			TAnsiStringStream sm = new TAnsiStringStream();
			try
			{
				int iSize = 0;
				byte [] buffer = null;
				int iError = UserCertStorage.SaveToBufferPFX(ref buffer, ref iSize, sDefCertPswdInCustStorage, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_3DES, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_3DES);

				if ((iError != SBPKCS12.Unit.SB_PKCS12_ERROR_BUFFER_TOO_SMALL) || (iSize <= 0))
					CheckSBB(iError, "SaveToBufferPFX failed to save the storage");

				sm.Size = iSize;
				buffer = sm.Memory;
				CheckSBB(UserCertStorage.SaveToBufferPFX(ref buffer, ref iSize, sDefCertPswdInCustStorage, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_3DES, SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_3DES), "SaveToBufferPFX failed to save the storage");
				sm.Memory = buffer;
				sm.SaveToFile(sFileName);
			}
			finally
			{
				sm.Close();
			}
		}

		public static void CheckSBB(int iErrorCode, string sErrorMessage)
		{
			if (iErrorCode != 0)
				throw new Exception(sErrorMessage + ". Error code: '" + iErrorCode.ToString() + "'.");
		}

		private void RepaintCertificates()
		{
			lvCert.BeginUpdate();
			try
			{
				lvCert.Clear();

				int i;
				string s;
				TElX509Certificate cer;
				ListViewItem lv;
				for (i = 0; i < UserCertStorage.Count; i++)
				{
					cer = UserCertStorage.get_Certificates(i);
					s = fRDN.GetOIDValue(cer.SubjectRDN, SBUtils.Unit.SB_CERT_OID_COMMON_NAME) + " / " + 
						fRDN.GetOIDValue(cer.SubjectRDN, SBUtils.Unit.SB_CERT_OID_EMAIL);
					lv = lvCert.Items.Add(s);
					lv.Tag = cer;
				}
			}
			finally
			{
				lvCert.EndUpdate();
			}
		}

		public override string GetCaption()
		{
			return "SMIME Options";
		}

		public static void SMIMECollectCertificates()
		{
			TElWinCertStorage WinStorage;
			CurCertStorage.Clear();

			if (UseWinCertStorage)
			{
				WinStorage = new TElWinCertStorage();
				try
				{
					WinStorage.SystemStores.Text = "My";
					WinStorage.ExportTo(CurCertStorage);
				}
				finally
				{
					WinStorage.Dispose();
				}
			}

			if (UseUserCertStorage)
			{
				UserCertStorage.ExportTo(CurCertStorage);
			}
		}
	}
}
