using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using SBPDF;
using SBPDFSecurity;
using SBCustomCertStorage;
using SBX509;
using SBConstants;

namespace TinyEncryptor
{
	/// <summary>
	/// Tiny PDF encryptor
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnBrowseSource;
		private System.Windows.Forms.Button btnBrowseDest;
		private System.Windows.Forms.Label lSource;
		private System.Windows.Forms.Label lDest;
		private System.Windows.Forms.TextBox tbSource;
		private System.Windows.Forms.TextBox tbDest;
		private System.Windows.Forms.GroupBox gbEncProps;
		private System.Windows.Forms.Label lEncryptionAlgorithm;
		private System.Windows.Forms.ComboBox cbEncryptionAlgorithm;
		private System.Windows.Forms.CheckBox cbEncryptMetadata;
		private System.Windows.Forms.RadioButton rbPasswordEncryption;
		private System.Windows.Forms.RadioButton rbPublicKeyEncryption;
		private System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.Label lCertificate;
		private System.Windows.Forms.Button btnBrowseCert;
		private System.Windows.Forms.TextBox tbCert;
		private System.Windows.Forms.Label lCertPassword;
		private System.Windows.Forms.TextBox tbCertPassword;
		private System.Windows.Forms.Button btnEncrypt;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.OpenFileDialog openPDFDialog;
		private System.Windows.Forms.SaveFileDialog savePDFDialog;
		private System.Windows.Forms.OpenFileDialog openCertDialog;
		// SecureBlackbox objects
		private TElPDFDocument Document;
		private TElPDFPasswordSecurityHandler PasswordHandler;
		private TElPDFPublicKeySecurityHandler PublicKeyHandler;
		private TElMemoryCertStorage CertStorage;
		private TElX509Certificate Cert;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			// Calling SetLicenseKey
			SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"));
			
			// Both these functions *must* be called before using PDFBlackbox
			SBPDF.Unit.Initialize();
			SBPDFSecurity.Unit.Initialize();

			Document = new TElPDFDocument();
			PasswordHandler = new TElPDFPasswordSecurityHandler();
			PublicKeyHandler = new TElPDFPublicKeySecurityHandler();
			CertStorage = new TElMemoryCertStorage();
			Cert = new TElX509Certificate();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			this.tbSource = new System.Windows.Forms.TextBox();
			this.tbDest = new System.Windows.Forms.TextBox();
			this.btnBrowseSource = new System.Windows.Forms.Button();
			this.btnBrowseDest = new System.Windows.Forms.Button();
			this.lSource = new System.Windows.Forms.Label();
			this.lDest = new System.Windows.Forms.Label();
			this.gbEncProps = new System.Windows.Forms.GroupBox();
			this.tbCertPassword = new System.Windows.Forms.TextBox();
			this.btnBrowseCert = new System.Windows.Forms.Button();
			this.lCertPassword = new System.Windows.Forms.Label();
			this.lCertificate = new System.Windows.Forms.Label();
			this.tbCert = new System.Windows.Forms.TextBox();
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.rbPublicKeyEncryption = new System.Windows.Forms.RadioButton();
			this.rbPasswordEncryption = new System.Windows.Forms.RadioButton();
			this.cbEncryptMetadata = new System.Windows.Forms.CheckBox();
			this.cbEncryptionAlgorithm = new System.Windows.Forms.ComboBox();
			this.lEncryptionAlgorithm = new System.Windows.Forms.Label();
			this.btnEncrypt = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.openPDFDialog = new System.Windows.Forms.OpenFileDialog();
			this.savePDFDialog = new System.Windows.Forms.SaveFileDialog();
			this.openCertDialog = new System.Windows.Forms.OpenFileDialog();
			this.gbEncProps.SuspendLayout();
			this.SuspendLayout();
			// 
			// tbSource
			// 
			this.tbSource.Location = new System.Drawing.Point(8, 24);
			this.tbSource.Name = "tbSource";
			this.tbSource.Size = new System.Drawing.Size(288, 21);
			this.tbSource.TabIndex = 0;
			this.tbSource.Text = "";
			// 
			// tbDest
			// 
			this.tbDest.Location = new System.Drawing.Point(8, 72);
			this.tbDest.Name = "tbDest";
			this.tbDest.Size = new System.Drawing.Size(288, 21);
			this.tbDest.TabIndex = 1;
			this.tbDest.Text = "";
			// 
			// btnBrowseSource
			// 
			this.btnBrowseSource.Location = new System.Drawing.Point(304, 24);
			this.btnBrowseSource.Name = "btnBrowseSource";
			this.btnBrowseSource.TabIndex = 2;
			this.btnBrowseSource.Text = "Browse...";
			this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click);
			// 
			// btnBrowseDest
			// 
			this.btnBrowseDest.Location = new System.Drawing.Point(304, 72);
			this.btnBrowseDest.Name = "btnBrowseDest";
			this.btnBrowseDest.TabIndex = 3;
			this.btnBrowseDest.Text = "Browse...";
			this.btnBrowseDest.Click += new System.EventHandler(this.btnBrowseDest_Click);
			// 
			// lSource
			// 
			this.lSource.Location = new System.Drawing.Point(8, 8);
			this.lSource.Name = "lSource";
			this.lSource.Size = new System.Drawing.Size(256, 16);
			this.lSource.TabIndex = 4;
			this.lSource.Text = "Source PDF file:";
			// 
			// lDest
			// 
			this.lDest.Location = new System.Drawing.Point(8, 56);
			this.lDest.Name = "lDest";
			this.lDest.Size = new System.Drawing.Size(264, 16);
			this.lDest.TabIndex = 5;
			this.lDest.Text = "Destination PDF file:";
			// 
			// gbEncProps
			// 
			this.gbEncProps.Controls.Add(this.tbCertPassword);
			this.gbEncProps.Controls.Add(this.btnBrowseCert);
			this.gbEncProps.Controls.Add(this.lCertPassword);
			this.gbEncProps.Controls.Add(this.lCertificate);
			this.gbEncProps.Controls.Add(this.tbCert);
			this.gbEncProps.Controls.Add(this.tbPassword);
			this.gbEncProps.Controls.Add(this.rbPublicKeyEncryption);
			this.gbEncProps.Controls.Add(this.rbPasswordEncryption);
			this.gbEncProps.Controls.Add(this.cbEncryptMetadata);
			this.gbEncProps.Controls.Add(this.cbEncryptionAlgorithm);
			this.gbEncProps.Controls.Add(this.lEncryptionAlgorithm);
			this.gbEncProps.Location = new System.Drawing.Point(8, 104);
			this.gbEncProps.Name = "gbEncProps";
			this.gbEncProps.Size = new System.Drawing.Size(368, 264);
			this.gbEncProps.TabIndex = 6;
			this.gbEncProps.TabStop = false;
			this.gbEncProps.Text = "Encryption properties";
			// 
			// tbCertPassword
			// 
			this.tbCertPassword.Enabled = false;
			this.tbCertPassword.Location = new System.Drawing.Point(40, 224);
			this.tbCertPassword.Name = "tbCertPassword";
			this.tbCertPassword.PasswordChar = '*';
			this.tbCertPassword.Size = new System.Drawing.Size(128, 21);
			this.tbCertPassword.TabIndex = 11;
			this.tbCertPassword.Text = "";
			// 
			// btnBrowseCert
			// 
			this.btnBrowseCert.Enabled = false;
			this.btnBrowseCert.Location = new System.Drawing.Point(280, 176);
			this.btnBrowseCert.Name = "btnBrowseCert";
			this.btnBrowseCert.TabIndex = 10;
			this.btnBrowseCert.Text = "Browse...";
			this.btnBrowseCert.Click += new System.EventHandler(this.btnBrowseCert_Click);
			// 
			// lCertPassword
			// 
			this.lCertPassword.Enabled = false;
			this.lCertPassword.Location = new System.Drawing.Point(40, 208);
			this.lCertPassword.Name = "lCertPassword";
			this.lCertPassword.Size = new System.Drawing.Size(224, 16);
			this.lCertPassword.TabIndex = 8;
			this.lCertPassword.Text = "Certificate password:";
			// 
			// lCertificate
			// 
			this.lCertificate.Enabled = false;
			this.lCertificate.Location = new System.Drawing.Point(40, 160);
			this.lCertificate.Name = "lCertificate";
			this.lCertificate.Size = new System.Drawing.Size(272, 16);
			this.lCertificate.TabIndex = 7;
			this.lCertificate.Text = "Encryption certificate:";
			// 
			// tbCert
			// 
			this.tbCert.Enabled = false;
			this.tbCert.Location = new System.Drawing.Point(40, 176);
			this.tbCert.Name = "tbCert";
			this.tbCert.Size = new System.Drawing.Size(232, 21);
			this.tbCert.TabIndex = 6;
			this.tbCert.Text = "";
			// 
			// tbPassword
			// 
			this.tbPassword.Location = new System.Drawing.Point(40, 96);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.PasswordChar = '*';
			this.tbPassword.Size = new System.Drawing.Size(128, 21);
			this.tbPassword.TabIndex = 5;
			this.tbPassword.Text = "";
			// 
			// rbPublicKeyEncryption
			// 
			this.rbPublicKeyEncryption.Location = new System.Drawing.Point(16, 136);
			this.rbPublicKeyEncryption.Name = "rbPublicKeyEncryption";
			this.rbPublicKeyEncryption.Size = new System.Drawing.Size(304, 24);
			this.rbPublicKeyEncryption.TabIndex = 4;
			this.rbPublicKeyEncryption.Text = "Public key encryption";
			this.rbPublicKeyEncryption.CheckedChanged += new System.EventHandler(this.rbPasswordEncryption_CheckedChanged);
			// 
			// rbPasswordEncryption
			// 
			this.rbPasswordEncryption.Checked = true;
			this.rbPasswordEncryption.Location = new System.Drawing.Point(16, 72);
			this.rbPasswordEncryption.Name = "rbPasswordEncryption";
			this.rbPasswordEncryption.Size = new System.Drawing.Size(272, 24);
			this.rbPasswordEncryption.TabIndex = 3;
			this.rbPasswordEncryption.TabStop = true;
			this.rbPasswordEncryption.Text = "Password encryption";
			this.rbPasswordEncryption.CheckedChanged += new System.EventHandler(this.rbPasswordEncryption_CheckedChanged);
			// 
			// cbEncryptMetadata
			// 
			this.cbEncryptMetadata.Checked = true;
			this.cbEncryptMetadata.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEncryptMetadata.Location = new System.Drawing.Point(184, 40);
			this.cbEncryptMetadata.Name = "cbEncryptMetadata";
			this.cbEncryptMetadata.Size = new System.Drawing.Size(168, 24);
			this.cbEncryptMetadata.TabIndex = 2;
			this.cbEncryptMetadata.Text = "Encrypt document metadata";
			// 
			// cbEncryptionAlgorithm
			// 
			this.cbEncryptionAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbEncryptionAlgorithm.Items.AddRange(new object[] {
																	   "RC4/40 bits (Acrobat 4)",
																	   "RC4/128 bits (Acrobat 5)",
																	   "AES/128 bits (Acrobat 6, 7)"});
			this.cbEncryptionAlgorithm.Location = new System.Drawing.Point(16, 40);
			this.cbEncryptionAlgorithm.Name = "cbEncryptionAlgorithm";
			this.cbEncryptionAlgorithm.Size = new System.Drawing.Size(152, 21);
			this.cbEncryptionAlgorithm.TabIndex = 1;
			// 
			// lEncryptionAlgorithm
			// 
			this.lEncryptionAlgorithm.Location = new System.Drawing.Point(16, 24);
			this.lEncryptionAlgorithm.Name = "lEncryptionAlgorithm";
			this.lEncryptionAlgorithm.Size = new System.Drawing.Size(144, 16);
			this.lEncryptionAlgorithm.TabIndex = 0;
			this.lEncryptionAlgorithm.Text = "Encryption algorithm:";
			// 
			// btnEncrypt
			// 
			this.btnEncrypt.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnEncrypt.Location = new System.Drawing.Point(120, 376);
			this.btnEncrypt.Name = "btnEncrypt";
			this.btnEncrypt.TabIndex = 7;
			this.btnEncrypt.Text = "Encrypt";
			this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(200, 376);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// openPDFDialog
			// 
			this.openPDFDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
			this.openPDFDialog.InitialDirectory = ".";
			// 
			// savePDFDialog
			// 
			this.savePDFDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
			this.savePDFDialog.InitialDirectory = ".";
			// 
			// openCertDialog
			// 
			this.openCertDialog.Filter = "Raw X.509 certificate (*.cer, *.csr, *.crt)|*.CER;*.CSR;*.CRT|Base64-encoded X.50" +
				"9 certificate (*.pem)|*.PEM|PKCS#12 certificate (*.pfx, *.p12)|*.PFX; *.P12|All " +
				"files (*.*)|*.*";
			this.openCertDialog.InitialDirectory = ".";
			// 
			// frmMain
			// 
			this.AcceptButton = this.btnEncrypt;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(384, 413);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnEncrypt);
			this.Controls.Add(this.gbEncProps);
			this.Controls.Add(this.lDest);
			this.Controls.Add(this.lSource);
			this.Controls.Add(this.btnBrowseDest);
			this.Controls.Add(this.btnBrowseSource);
			this.Controls.Add(this.tbDest);
			this.Controls.Add(this.tbSource);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmMain";
			this.Text = "Tiny PDF Encryptor";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.gbEncProps.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new frmMain());
		}

		private void btnBrowseSource_Click(object sender, System.EventArgs e)
		{
			if (openPDFDialog.ShowDialog() == DialogResult.OK) 
                tbSource.Text = openPDFDialog.FileName;
		}

		private void btnBrowseDest_Click(object sender, System.EventArgs e)
		{
			if (savePDFDialog.ShowDialog() == DialogResult.OK) 
                tbDest.Text = savePDFDialog.FileName;
		}

		private void btnBrowseCert_Click(object sender, System.EventArgs e)
		{
			if (openCertDialog.ShowDialog() == DialogResult.OK) 
                tbCert.Text = openCertDialog.FileName;
		}

		private void rbPasswordEncryption_CheckedChanged(object sender, System.EventArgs e)
		{
			tbPassword.Enabled = rbPasswordEncryption.Checked;
			tbCert.Enabled = !rbPasswordEncryption.Checked;
			tbCertPassword.Enabled = !rbPasswordEncryption.Checked;
			btnBrowseCert.Enabled = !rbPasswordEncryption.Checked;
			lCertificate.Enabled = !rbPasswordEncryption.Checked;
			lCertPassword.Enabled = !rbPasswordEncryption.Checked;
		}

		private void btnEncrypt_Click(object sender, System.EventArgs e)
		{
			bool Success = false;
			TElPDFSecurityHandler CurrHandler;
			int Alg;
			int KeySize;
			int CertFormat;
			int GroupIndex;
			string TempPath = Path.GetTempFileName();

			try 
			{
				// creating a temporary file copy
				System.IO.File.Copy(tbSource.Text, TempPath, true);
				// opening the temporary file
				FileStream F = new FileStream(TempPath, FileMode.Open, FileAccess.ReadWrite);
				try 
				{
                    Document.Open(F);
					try 
					{
						// checking if the document is already encrypted
						if (Document.Encrypted) 
						{
							MessageBox.Show("The document is already encrypted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
						}
						// checking if the document is signed
						if (Document.Signed) 
						{
                            MessageBox.Show("The document contains a digital signature and cannot be encrypted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
						// setting up property values
						if (rbPasswordEncryption.Checked) 
							CurrHandler = PasswordHandler;
						else 
                            CurrHandler = PublicKeyHandler;
						// the following encryption handler assignment must be executed before
						// other handler properties are assigned, since encryption handler
						// has to access document properties during its work.
						Document.EncryptionHandler = CurrHandler;
						CurrHandler.EncryptMetadata = cbEncryptMetadata.Checked;
						switch(cbEncryptionAlgorithm.SelectedIndex) 
						{
							case 0 :
								Alg = SBConstants.Unit.SB_ALGORITHM_CNT_RC4;
								KeySize = 40;
								break;
							case 1 :
								Alg = SBConstants.Unit.SB_ALGORITHM_CNT_RC4;
								KeySize = 128;
								break;
							case 2 :
								Alg = SBConstants.Unit.SB_ALGORITHM_CNT_AES128;
								// the key size for this cipher is always 128 bits, so we may
								// omit the assignment in this point. However, just to calm the
					            // compiler we assing the 0 value to KeySize variable.
								KeySize = 0;
								break;
							default:
								// normally, we should not reach this point, so just making the
								// compiler silent.
								MessageBox.Show("Unsupported algorithm", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
						}
						// PDF specification allows to use different ciphers for streams and
						// strings contained in a document. However, using the same value for
						// both string and stream encryption allows to achieve greater compatibility
						// with other implementations
						CurrHandler.StreamEncryptionAlgorithm = Alg;
						CurrHandler.StringEncryptionAlgorithm = Alg;
						CurrHandler.StreamEncryptionKeyBits = KeySize;
						CurrHandler.StringEncryptionKeyBits = KeySize;
						if (CurrHandler is TElPDFPasswordSecurityHandler) 
						{
							PasswordHandler.UserPassword = tbPassword.Text;
							PasswordHandler.OwnerPassword = tbPassword.Text;
							PasswordHandler.Permissions.EnableAll();
						} 
						else 
						{
							// loading certificate
							FileStream CertF = new FileStream(tbCert.Text, FileMode.Open);
							try 
							{
								CertFormat = TElX509Certificate.DetectCertFileFormat(CertF);
								CertF.Position = 0;
								switch (CertFormat) 
								{
									case SBX509.Unit.cfDER : 
										Cert.LoadFromStream(CertF, 0);
										break;
									case SBX509.Unit.cfPEM : 
										Cert.LoadFromStreamPEM(CertF, tbCertPassword.Text, 0);
										break;
									case SBX509.Unit.cfPFX : 
										Cert.LoadFromStreamPFX(CertF, tbCertPassword.Text, 0);
										break;
									default:
										MessageBox.Show("Failed to load certificate", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
										return;
								} 
							}
							finally 
							{
								CertF.Close();
							}
							// creating recipient group
							GroupIndex = PublicKeyHandler.AddRecipientGroup();
							// adding recipient certificate to group
							PublicKeyHandler.get_RecipientGroups(GroupIndex).AddRecipient(Cert);
						}
						// encrypting the document
						Document.Encrypt();
						// allowing to save the document
						Success = true;
					} 
					finally 
					{
                        Document.Close(Success);
					}
				} 
				finally 
				{
                    F.Close();
				}
			} 
			catch(Exception ex) 
			{
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Success = false;
			}
			// if encryption process succeeded, moving the temporary file to the place 
			// of destination file
			if (Success) 
			{
				File.Copy(TempPath, tbDest.Text, true);
				MessageBox.Show("Encryption process successfully finished", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
			} 
			else 
			{
				MessageBox.Show("Encryption failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			// deleting the temporary file
			File.Delete(TempPath);
			this.Close();
		}

		private void frmMain_Load(object sender, System.EventArgs e)
		{
			cbEncryptionAlgorithm.SelectedIndex = 0;
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}
}
