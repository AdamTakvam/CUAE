using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SBCustomCertStorage;

namespace ElSecureChat.Client
{
	public enum TSelectCertMode {Unknown, ClientCert, ServerCert};

	/// <summary>
	/// Summary description for SelectCertForm.
	/// </summary>
	public class SelectCertForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lbSelectCertificates;
		private System.Windows.Forms.ListBox lbxCertificates;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnRemoveCertificate;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnLoadStorage;
		private System.Windows.Forms.Button btnSaveStorage;
		private System.Windows.Forms.Button btnAddCertificate;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private System.Windows.Forms.OpenFileDialog OpenDlg;
		private System.Windows.Forms.SaveFileDialog SaveDlg;

		private TElMemoryCertStorage FCertStorage;
		private TSelectCertMode FMode;

		public SelectCertForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			FCertStorage = new TElMemoryCertStorage();
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
			this.lbSelectCertificates = new System.Windows.Forms.Label();
			this.lbxCertificates = new System.Windows.Forms.ListBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnAddCertificate = new System.Windows.Forms.Button();
			this.btnRemoveCertificate = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnLoadStorage = new System.Windows.Forms.Button();
			this.btnSaveStorage = new System.Windows.Forms.Button();
			this.OpenDlg = new System.Windows.Forms.OpenFileDialog();
			this.SaveDlg = new System.Windows.Forms.SaveFileDialog();
			this.SuspendLayout();
			// 
			// lbSelectCertificates
			// 
			this.lbSelectCertificates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lbSelectCertificates.Location = new System.Drawing.Point(8, 8);
			this.lbSelectCertificates.Name = "lbSelectCertificates";
			this.lbSelectCertificates.Size = new System.Drawing.Size(456, 33);
			this.lbSelectCertificates.TabIndex = 0;
			this.lbSelectCertificates.Text = "Please, choose certificates.";
			// 
			// lbxCertificates
			// 
			this.lbxCertificates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lbxCertificates.Location = new System.Drawing.Point(8, 48);
			this.lbxCertificates.Name = "lbxCertificates";
			this.lbxCertificates.Size = new System.Drawing.Size(336, 160);
			this.lbxCertificates.TabIndex = 1;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Location = new System.Drawing.Point(8, 216);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(456, 4);
			this.panel1.TabIndex = 2;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(296, 224);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(384, 224);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnAddCertificate
			// 
			this.btnAddCertificate.Location = new System.Drawing.Point(352, 48);
			this.btnAddCertificate.Name = "btnAddCertificate";
			this.btnAddCertificate.Size = new System.Drawing.Size(112, 23);
			this.btnAddCertificate.TabIndex = 5;
			this.btnAddCertificate.Text = "Add certificate";
			this.btnAddCertificate.Click += new System.EventHandler(this.btnAddCertificate_Click);
			// 
			// btnRemoveCertificate
			// 
			this.btnRemoveCertificate.Location = new System.Drawing.Point(352, 80);
			this.btnRemoveCertificate.Name = "btnRemoveCertificate";
			this.btnRemoveCertificate.Size = new System.Drawing.Size(112, 23);
			this.btnRemoveCertificate.TabIndex = 6;
			this.btnRemoveCertificate.Text = "Remove certificate";
			this.btnRemoveCertificate.Click += new System.EventHandler(this.btnRemoveCertificate_Click);
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel2.Location = new System.Drawing.Point(352, 112);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(112, 4);
			this.panel2.TabIndex = 7;
			// 
			// btnLoadStorage
			// 
			this.btnLoadStorage.Location = new System.Drawing.Point(352, 120);
			this.btnLoadStorage.Name = "btnLoadStorage";
			this.btnLoadStorage.Size = new System.Drawing.Size(112, 23);
			this.btnLoadStorage.TabIndex = 8;
			this.btnLoadStorage.Text = "Load Storage";
			this.btnLoadStorage.Click += new System.EventHandler(this.btnLoadStorage_Click);
			// 
			// btnSaveStorage
			// 
			this.btnSaveStorage.Location = new System.Drawing.Point(352, 152);
			this.btnSaveStorage.Name = "btnSaveStorage";
			this.btnSaveStorage.Size = new System.Drawing.Size(112, 23);
			this.btnSaveStorage.TabIndex = 9;
			this.btnSaveStorage.Text = "Save Storage";
			this.btnSaveStorage.Click += new System.EventHandler(this.btnSaveStorage_Click);
			// 
			// SelectCertForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(472, 254);
			this.Controls.Add(this.btnSaveStorage);
			this.Controls.Add(this.btnLoadStorage);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.btnRemoveCertificate);
			this.Controls.Add(this.btnAddCertificate);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.lbxCertificates);
			this.Controls.Add(this.lbSelectCertificates);
			this.Name = "SelectCertForm";
			this.Text = "Select Certificates";
			this.ResumeLayout(false);

		}
		#endregion

		public static void CheckSBB(int iErrorCode, string sErrorMessage)
		{
			if (iErrorCode != 0)
				throw new Exception(sErrorMessage + ". Error code: '" + iErrorCode.ToString() + "'.");
		}

		private const string sDefCertPswdInCustStorage = "{37907B5C-B309-4AE4-AFD2-2EAE948EADA2}";

		public static void LoadStorage(string sFileName, TElCustomCertStorage CertStorage)
		{
			CertStorage.Clear();
			if (!File.Exists(sFileName))
				return;

			FileStream fs = new FileStream(sFileName, FileMode.Open);
			try
			{
				CheckSBB(
					CertStorage.LoadFromStreamPFX(fs, sDefCertPswdInCustStorage, 0),
					"Cannot load certificates from file storage: '" + sFileName + "'"
					);
			}
			finally
			{
				fs.Close();
			}
		}

		public static void SaveStorage(string sFileName, TElCustomCertStorage CertStorage)
		{
			FileStream fs = new FileStream(sFileName, FileMode.Create);
			try
			{
				int iError = CertStorage.SaveToStreamPFX(fs, sDefCertPswdInCustStorage,
					SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_3DES, 
					SBConstants.Unit.SB_ALGORITHM_PBE_SHA1_3DES);

				if (iError != 0)
					CheckSBB(iError, "SaveToStreamPFX failed to save the storage");
			}
			finally
			{
				fs.Close();
			}
		}

		private void UpdateCertificatesList()
		{
			lbxCertificates.BeginUpdate();
			lbxCertificates.Items.Clear();
			string s, t;
			for (int i = 0; i < FCertStorage.Count; i++)
			{
				s = fRDN.GetOIDValue(FCertStorage.get_Certificates(i).SubjectRDN, SBUtils.Unit.SB_CERT_OID_COMMON_NAME);
				if (s == "")
					s = fRDN.GetOIDValue(FCertStorage.get_Certificates(i).SubjectRDN, SBUtils.Unit.SB_CERT_OID_ORGANIZATION);

				if (s == "")
					s = "<unknown>";

				t = fRDN.GetOIDValue(FCertStorage.get_Certificates(i).IssuerRDN, SBUtils.Unit.SB_CERT_OID_COMMON_NAME);
				if (t == "")
					t = fRDN.GetOIDValue(FCertStorage.get_Certificates(i).IssuerRDN, SBUtils.Unit.SB_CERT_OID_ORGANIZATION);

				if (t == "")
					t = "<unknown>";

				lbxCertificates.Items.Add(s + " (" + t + ")");
			}

			lbxCertificates.EndUpdate();
		}

		private string RequestPassphrase()
		{
			StringQueryDlg passwdDlg = new StringQueryDlg(true);
			passwdDlg.Text = "Enter password";
			passwdDlg.Description = "Please, enter passphrase:";
			string sPwd = "";
			if (passwdDlg.ShowDialog(this) == DialogResult.OK)
				sPwd = passwdDlg.TextBox;

			passwdDlg.Dispose();
			return sPwd;
		}

		public void GetStorage(ref TElMemoryCertStorage Value)
		{
			if (Value == null)
				Value = new TElMemoryCertStorage();
			else
				Value.Clear();

			FCertStorage.ExportTo(Value);
		}

		public void SetStorage(TElMemoryCertStorage Value)
		{
			FCertStorage.Clear();
			if (Value != null)
				Value.ExportTo(FCertStorage);

			UpdateCertificatesList();
		}

		public void SetMode(TSelectCertMode Value)
		{
			FMode = Value;
			if (FMode == TSelectCertMode.ClientCert)
				lbSelectCertificates.Text = "Please, choose client-side certificate or certificate chain.\r\nThe server has client authentication enabled.";
			else if (FMode == TSelectCertMode.ServerCert)
				lbSelectCertificates.Text = "Please, choose server certificates.";
			else
				lbSelectCertificates.Text = "Please, choose certificates.";
		}

		private void btnAddCertificate_Click(object sender, System.EventArgs e)
		{
			bool KeyLoaded = false;
			OpenDlg.FileName = "";
			OpenDlg.Title = "Select certificate file";
			OpenDlg.Filter = "PEM-encoded certificate (*.pem)|*.pem|DER-encoded certificate (*.cer)|*.cer|PFX-encoded certificate (*.pfx)|*.pfx";
			if (OpenDlg.ShowDialog(this) != DialogResult.OK)
				return;

			FileStream F = new FileStream(OpenDlg.FileName, FileMode.Open);
			byte [] Buf = new byte[F.Length];
			F.Read(Buf, 0, (int)F.Length);
			F.Close();

			int Res = 0;
			SBX509.TElX509Certificate Cert = new SBX509.TElX509Certificate(null);
			if (OpenDlg.FilterIndex == 3)
				Res = Cert.LoadFromBufferPFX(Buf, RequestPassphrase());
			else if (OpenDlg.FilterIndex == 1)
				Res = Cert.LoadFromBufferPEM(Buf, "");
			else if (OpenDlg.FilterIndex == 2)
				Cert.LoadFromBuffer(Buf);
			else
				Res = -1;

			if ((Res != 0) || (Cert.CertificateSize == 0))
			{				
				MessageBox.Show("Error loading the certificate", "SSL Sample", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			int Sz = 0;
			Buf = null;
			Cert.SaveKeyToBuffer(ref Buf, ref Sz);

			if (Sz == 0)
			{
				OpenDlg.Title = "Select the corresponding private key file";
				OpenDlg.Filter = "PEM-encoded key (*.pem)|*.PEM|DER-encoded key (*.key)|*.key";
				if (OpenDlg.ShowDialog(this) == DialogResult.OK)
				{
					F = new FileStream(OpenDlg.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
					Buf = new byte[F.Length];
					F.Read(Buf, 0, (int)F.Length);
					F.Close();

					if (OpenDlg.FilterIndex == 1)
						Cert.LoadKeyFromBufferPEM(Buf, RequestPassphrase());
					else
						Cert.LoadKeyFromBuffer(Buf);

					KeyLoaded = true;
				}
			}
			else
				KeyLoaded = true;

			if (!KeyLoaded)
				MessageBox.Show("Private key was not loaded. Certificate added without private key.", "SSL Sample", MessageBoxButtons.OK, MessageBoxIcon.Error);

			if (!FCertStorage.IsPresent(Cert))
				FCertStorage.Add(Cert, true);
    
			UpdateCertificatesList();
		}

		private void btnRemoveCertificate_Click(object sender, System.EventArgs e)
		{
			if (lbxCertificates.SelectedIndex >= 0)
			{
				FCertStorage.Remove(lbxCertificates.SelectedIndex);
				UpdateCertificatesList();
			}
		}

		private void btnLoadStorage_Click(object sender, System.EventArgs e)
		{
			OpenDlg.Filter = "Users Certificates Storage (*.ucs)|*.ucs|All Files (*.*)|*.*";
			OpenDlg.FilterIndex = 0;
			OpenDlg.Title = "Load Storage";
			OpenDlg.FileName = "";
			if (OpenDlg.ShowDialog(this) == DialogResult.OK)
			{
				LoadStorage(OpenDlg.FileName, FCertStorage);
				UpdateCertificatesList();
			}
		}

		private void btnSaveStorage_Click(object sender, System.EventArgs e)
		{
			SaveDlg.Filter = "Users Certificates Storage (*.ucs)|*.ucs|All Files (*.*)|*.*";
			SaveDlg.FilterIndex = 0;
			SaveDlg.DefaultExt = ".ucs";
			SaveDlg.Title = "Save Storage";
			SaveDlg.FileName = "";
			if (SaveDlg.ShowDialog(this) == DialogResult.OK)
				SaveStorage(SaveDlg.FileName, FCertStorage);	
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			Close();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			Close();	
		}
	}

	public class fRDN
	{
		public static string GetOIDValue(SBRDN.TElRelativeDistinguishedName RDN, byte[] S)
		{
			string t = "";
			int iCount = RDN.Count;
			for (int i = 0; i < iCount; i++)
			{
				if (SBUtils.Unit.CompareContent(RDN.get_OIDs(i), S))
				{
					if (t != "")
						t += " / ";

					t += SBUtils.Unit.UTF8ToStr(RDN.get_Values(i));
				}
			}

			return t;
		}
	}
}
