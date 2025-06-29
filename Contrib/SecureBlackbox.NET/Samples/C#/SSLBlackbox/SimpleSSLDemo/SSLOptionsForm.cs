using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using SBCustomCertStorage;
using SBRDN;
using SBUtils;
using SBX509;

namespace SSLClientDemo
{
	/// <summary>
	/// Summary description for SSLOptionsForm.
	/// </summary>
	public class frmSSLOptions : System.Windows.Forms.Form
	{
		private TElMemoryCertStorage certs;
		private System.Windows.Forms.GroupBox gbEncryptionOptions;
		public System.Windows.Forms.CheckBox cbExportableOnly;
		public System.Windows.Forms.CheckBox cbAllowAnon;
		public System.Windows.Forms.CheckBox cbSSL2;
		public System.Windows.Forms.CheckBox cbSSL3;
		public System.Windows.Forms.CheckBox cbTLS1;
		private System.Windows.Forms.GroupBox gbCertificates;
		private System.Windows.Forms.ListView lvCerts;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.ColumnHeader chSubject;
		private System.Windows.Forms.ColumnHeader chIssuer;
		private System.Windows.Forms.ColumnHeader chAlgorithm;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		public System.Windows.Forms.CheckBox cbTLS11;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSSLOptions()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			Init();
		}

		protected void Init()
		{
			certs = new TElMemoryCertStorage();
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
			this.gbEncryptionOptions = new System.Windows.Forms.GroupBox();
			this.cbTLS1 = new System.Windows.Forms.CheckBox();
			this.cbSSL3 = new System.Windows.Forms.CheckBox();
			this.cbSSL2 = new System.Windows.Forms.CheckBox();
			this.cbAllowAnon = new System.Windows.Forms.CheckBox();
			this.cbExportableOnly = new System.Windows.Forms.CheckBox();
			this.gbCertificates = new System.Windows.Forms.GroupBox();
			this.btnRemove = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.lvCerts = new System.Windows.Forms.ListView();
			this.chSubject = new System.Windows.Forms.ColumnHeader();
			this.chIssuer = new System.Windows.Forms.ColumnHeader();
			this.chAlgorithm = new System.Windows.Forms.ColumnHeader();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.cbTLS11 = new System.Windows.Forms.CheckBox();
			this.gbEncryptionOptions.SuspendLayout();
			this.gbCertificates.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbEncryptionOptions
			// 
			this.gbEncryptionOptions.Controls.Add(this.cbTLS11);
			this.gbEncryptionOptions.Controls.Add(this.cbTLS1);
			this.gbEncryptionOptions.Controls.Add(this.cbSSL3);
			this.gbEncryptionOptions.Controls.Add(this.cbSSL2);
			this.gbEncryptionOptions.Controls.Add(this.cbAllowAnon);
			this.gbEncryptionOptions.Controls.Add(this.cbExportableOnly);
			this.gbEncryptionOptions.Location = new System.Drawing.Point(8, 8);
			this.gbEncryptionOptions.Name = "gbEncryptionOptions";
			this.gbEncryptionOptions.Size = new System.Drawing.Size(360, 144);
			this.gbEncryptionOptions.TabIndex = 0;
			this.gbEncryptionOptions.TabStop = false;
			this.gbEncryptionOptions.Text = "Encryption options";
			// 
			// cbTLS1
			// 
			this.cbTLS1.Checked = true;
			this.cbTLS1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbTLS1.Location = new System.Drawing.Point(160, 24);
			this.cbTLS1.Name = "cbTLS1";
			this.cbTLS1.Size = new System.Drawing.Size(56, 24);
			this.cbTLS1.TabIndex = 4;
			this.cbTLS1.Text = "TLS1";
			// 
			// cbSSL3
			// 
			this.cbSSL3.Checked = true;
			this.cbSSL3.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbSSL3.Location = new System.Drawing.Point(88, 24);
			this.cbSSL3.Name = "cbSSL3";
			this.cbSSL3.TabIndex = 3;
			this.cbSSL3.Text = "SSL3";
			// 
			// cbSSL2
			// 
			this.cbSSL2.Location = new System.Drawing.Point(16, 24);
			this.cbSSL2.Name = "cbSSL2";
			this.cbSSL2.Size = new System.Drawing.Size(80, 24);
			this.cbSSL2.TabIndex = 2;
			this.cbSSL2.Text = "SSL2";
			// 
			// cbAllowAnon
			// 
			this.cbAllowAnon.Location = new System.Drawing.Point(16, 104);
			this.cbAllowAnon.Name = "cbAllowAnon";
			this.cbAllowAnon.Size = new System.Drawing.Size(200, 24);
			this.cbAllowAnon.TabIndex = 1;
			this.cbAllowAnon.Text = "Allow anonymous sessions";
			// 
			// cbExportableOnly
			// 
			this.cbExportableOnly.Location = new System.Drawing.Point(16, 72);
			this.cbExportableOnly.Name = "cbExportableOnly";
			this.cbExportableOnly.Size = new System.Drawing.Size(184, 24);
			this.cbExportableOnly.TabIndex = 0;
			this.cbExportableOnly.Text = "Use only exportable ciphersuites";
			// 
			// gbCertificates
			// 
			this.gbCertificates.Controls.Add(this.btnRemove);
			this.gbCertificates.Controls.Add(this.btnAdd);
			this.gbCertificates.Controls.Add(this.lvCerts);
			this.gbCertificates.Location = new System.Drawing.Point(8, 160);
			this.gbCertificates.Name = "gbCertificates";
			this.gbCertificates.Size = new System.Drawing.Size(360, 176);
			this.gbCertificates.TabIndex = 1;
			this.gbCertificates.TabStop = false;
			this.gbCertificates.Text = "Certificates";
			// 
			// btnRemove
			// 
			this.btnRemove.Location = new System.Drawing.Point(280, 56);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(72, 23);
			this.btnRemove.TabIndex = 2;
			this.btnRemove.Text = "Remove";
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(280, 24);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(72, 23);
			this.btnAdd.TabIndex = 1;
			this.btnAdd.Text = "Add...";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// lvCerts
			// 
			this.lvCerts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					  this.chSubject,
																					  this.chIssuer,
																					  this.chAlgorithm});
			this.lvCerts.FullRowSelect = true;
			this.lvCerts.Location = new System.Drawing.Point(16, 24);
			this.lvCerts.Name = "lvCerts";
			this.lvCerts.Size = new System.Drawing.Size(256, 136);
			this.lvCerts.TabIndex = 0;
			this.lvCerts.View = System.Windows.Forms.View.Details;
			// 
			// chSubject
			// 
			this.chSubject.Text = "Subject";
			this.chSubject.Width = 91;
			// 
			// chIssuer
			// 
			this.chIssuer.Text = "Issuer";
			this.chIssuer.Width = 85;
			// 
			// chAlgorithm
			// 
			this.chAlgorithm.Text = "Algorithm";
			this.chAlgorithm.Width = 59;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(208, 344);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(288, 344);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "PKCS#12 certificates (*.pfx)|*.pfx|PEM-formatted certificates (*.pem)|*.pem|Plain" +
				" certificates (*.cer)|*.cer";
			// 
			// cbTLS11
			// 
			this.cbTLS11.Checked = true;
			this.cbTLS11.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbTLS11.Location = new System.Drawing.Point(232, 24);
			this.cbTLS11.Name = "cbTLS11";
			this.cbTLS11.Size = new System.Drawing.Size(56, 24);
			this.cbTLS11.TabIndex = 5;
			this.cbTLS11.Text = "TLS1.1";
			// 
			// frmSSLOptions
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(378, 383);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.gbCertificates);
			this.Controls.Add(this.gbEncryptionOptions);
			this.Font = new System.Drawing.Font("Tahoma", 8F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmSSLOptions";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "SSL options";
			this.gbEncryptionOptions.ResumeLayout(false);
			this.gbCertificates.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private string FormatRDN(TElRelativeDistinguishedName rdn) 
		{
			string s = "";
			int i;
			for (i = 0; i < rdn.Count; i++) 
			{
				if (SBUtils.Unit.CompareMem(SBUtils.Unit.SB_CERT_OID_COMMON_NAME, rdn.get_OIDs(i))) 
				{
					s = s + "CN=" + SBUtils.Unit.StringOfBytes(rdn.get_Values(i)) + "; ";
				} 
				else if (SBUtils.Unit.CompareMem(SBUtils.Unit.SB_CERT_OID_EMAIL, rdn.get_OIDs(i)))
				{
					s = s + "E=" + SBUtils.Unit.StringOfBytes(rdn.get_Values(i)) + "; ";
				}
				else if (SBUtils.Unit.CompareMem(SBUtils.Unit.SB_CERT_OID_COUNTRY, rdn.get_OIDs(i)))
				{
					s = s + "C=" + SBUtils.Unit.StringOfBytes(rdn.get_Values(i)) + "; ";
				}
				else if (SBUtils.Unit.CompareMem(SBUtils.Unit.SB_CERT_OID_ORGANIZATION, rdn.get_OIDs(i)))
				{
					s = s + "O=" + SBUtils.Unit.StringOfBytes(rdn.get_Values(i)) + "; ";
				}
				else if (SBUtils.Unit.CompareMem(SBUtils.Unit.SB_CERT_OID_STATE_OR_PROVINCE, rdn.get_OIDs(i)))
				{
					s = s + "SP=" + SBUtils.Unit.StringOfBytes(rdn.get_Values(i)) + "; ";
				}
			}
			return s;
		}

		void OutputCertificates()
		{
			int i;
			lvCerts.Items.Clear();
			for (i = 0; i < certs.Count; i++) 
			{
				ListViewItem item = new ListViewItem();
				item.Text = FormatRDN(certs.get_Certificates(i).SubjectRDN);
				item.SubItems.Add(FormatRDN(certs.get_Certificates(i).IssuerRDN));
				lvCerts.Items.Add(item);
			}
		}

		public void SetCertificates(TElMemoryCertStorage str)
		{
			certs.Clear();
			str.ExportTo(certs);
			OutputCertificates();
		}

		public void GetCertificates(TElMemoryCertStorage str)
		{
			str.Clear();
			certs.ExportTo(str);
		}

		private string RequestPassword()
		{
			frmRequestPassword dlg = new frmRequestPassword();
			if (dlg.ShowDialog() == DialogResult.OK) 
			{
				return dlg.tbInput.Text;
			} 
			else 
			{
				return "";
			}
		}
		
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			System.IO.FileStream f;
			int r;
			if (openFileDialog.ShowDialog() == DialogResult.OK) 
			{
				f = new System.IO.FileStream(openFileDialog.FileName, FileMode.Open);
				try
				{
					TElX509Certificate cert = new TElX509Certificate();
					if (openFileDialog.FilterIndex == 1) 
					{
						r = cert.LoadFromStreamPFX(f, RequestPassword(), 0);
					} 
					else if (openFileDialog.FilterIndex == 2) 
					{
						r = cert.LoadFromStreamPEM(f, RequestPassword(), 0);
					} 
					else if (openFileDialog.FilterIndex == 3) 
					{
						try 
						{
							cert.LoadFromStream(f, 0);
							r = 0;
						} 
						catch 
						{
							r = -1;
						}
					}
					else 
					{
						r = -1;
					}
					if (r != 0) 
					{
						MessageBox.Show("Cannot load certificate, error " + r.ToString());
					} 
					else 
					{
						certs.Add(cert, true);
						OutputCertificates();
					}
				} 
				finally 
				{
					f.Close();
				}
			}
		}

		private void btnRemove_Click(object sender, System.EventArgs e)
		{
			if (lvCerts.SelectedIndices.Count > 0) 
			{
                certs.Remove(lvCerts.SelectedIndices[0]);
				OutputCertificates();
			}
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			int i;
			bool found = false;
			if (certs.ChainCount != 0) 
			{
				for (i = 0; i < certs.ChainCount; i++ ) 
				{
					if (certs.get_Certificates(certs.get_Chains(i)).PrivateKeyExists) 
					{
						found = true;
						break;
					}
				}
				if (!found) 
				{
					MessageBox.Show("WARNING: At least one certificate must have a corresponding private key");
				}
			}
		}
	}
}
