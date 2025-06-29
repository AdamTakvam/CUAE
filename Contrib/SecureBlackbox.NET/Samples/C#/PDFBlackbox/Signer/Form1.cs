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
using SBWinCertStorage;
using SBX509;
using SBHTTPSClient;
using SBTSPCommon;
using SBTSPClient;
using SBHTTPTSPClient;

namespace TinySigner
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private TElPDFDocument Document;
		private TElPDFPublicKeySecurityHandler PublicKeyHandler;
		private TElMemoryCertStorage CertStorage;
		private TElWinCertStorage SystemStore;
		private TElX509Certificate Cert;
		private TElHTTPSClient HTTPClient;
		private TElHTTPTSPClient TSPClient;

		private System.Windows.Forms.Label lSource;
		private System.Windows.Forms.Label lDest;
		private System.Windows.Forms.Button btnBrowseSource;
		private System.Windows.Forms.Button btnBrowseDest;
		private System.Windows.Forms.Button btnSign;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox gbSigProps;
		private System.Windows.Forms.Label lCertPassword;
		private System.Windows.Forms.TextBox tbCert;
		private System.Windows.Forms.TextBox tbCertPassword;
		private System.Windows.Forms.Button btnBrowseCert;
		private System.Windows.Forms.Label lSignatureType;
		private System.Windows.Forms.ComboBox cbSignatureType;
		private System.Windows.Forms.Label lAuthor;
		private System.Windows.Forms.TextBox tbAuthor;
		private System.Windows.Forms.Label lReason;
		private System.Windows.Forms.ComboBox cbReason;
		private System.Windows.Forms.OpenFileDialog openDialogPDF;
		private System.Windows.Forms.OpenFileDialog openDialogCert;
		private System.Windows.Forms.TextBox tbSource;
		private System.Windows.Forms.TextBox tbDest;
		private System.Windows.Forms.SaveFileDialog saveDialogPDF;
		private System.Windows.Forms.CheckBox cbTimestamp;
		private System.Windows.Forms.TextBox tbTimestampServer;
		private System.Windows.Forms.RadioButton rbUseCertFile;
		private System.Windows.Forms.RadioButton rbUseSystemStore;
		private System.Windows.Forms.ComboBox cbSystemCerts;
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

			// Both initialization function *must* be called before using PDFBlackbox:
			SBPDF.Unit.Initialize();
			SBPDFSecurity.Unit.Initialize();

			Document = new TElPDFDocument();
			PublicKeyHandler = new TElPDFPublicKeySecurityHandler();
			CertStorage = new TElMemoryCertStorage();
			Cert = new TElX509Certificate();
			HTTPClient = new TElHTTPSClient();
			TSPClient = new TElHTTPTSPClient();
			SystemStore = new TElWinCertStorage();
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
			this.lSource = new System.Windows.Forms.Label();
			this.lDest = new System.Windows.Forms.Label();
			this.tbSource = new System.Windows.Forms.TextBox();
			this.tbDest = new System.Windows.Forms.TextBox();
			this.btnBrowseSource = new System.Windows.Forms.Button();
			this.btnBrowseDest = new System.Windows.Forms.Button();
			this.gbSigProps = new System.Windows.Forms.GroupBox();
			this.tbTimestampServer = new System.Windows.Forms.TextBox();
			this.cbTimestamp = new System.Windows.Forms.CheckBox();
			this.cbReason = new System.Windows.Forms.ComboBox();
			this.lReason = new System.Windows.Forms.Label();
			this.tbAuthor = new System.Windows.Forms.TextBox();
			this.lAuthor = new System.Windows.Forms.Label();
			this.cbSignatureType = new System.Windows.Forms.ComboBox();
			this.lSignatureType = new System.Windows.Forms.Label();
			this.btnBrowseCert = new System.Windows.Forms.Button();
			this.tbCertPassword = new System.Windows.Forms.TextBox();
			this.tbCert = new System.Windows.Forms.TextBox();
			this.lCertPassword = new System.Windows.Forms.Label();
			this.btnSign = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.openDialogPDF = new System.Windows.Forms.OpenFileDialog();
			this.openDialogCert = new System.Windows.Forms.OpenFileDialog();
			this.saveDialogPDF = new System.Windows.Forms.SaveFileDialog();
			this.rbUseCertFile = new System.Windows.Forms.RadioButton();
			this.rbUseSystemStore = new System.Windows.Forms.RadioButton();
			this.cbSystemCerts = new System.Windows.Forms.ComboBox();
			this.gbSigProps.SuspendLayout();
			this.SuspendLayout();
			// 
			// lSource
			// 
			this.lSource.Location = new System.Drawing.Point(8, 8);
			this.lSource.Name = "lSource";
			this.lSource.Size = new System.Drawing.Size(100, 16);
			this.lSource.TabIndex = 0;
			this.lSource.Text = "Source PDF file:";
			// 
			// lDest
			// 
			this.lDest.Location = new System.Drawing.Point(8, 56);
			this.lDest.Name = "lDest";
			this.lDest.Size = new System.Drawing.Size(136, 16);
			this.lDest.TabIndex = 1;
			this.lDest.Text = "Destination PDF file:";
			// 
			// tbSource
			// 
			this.tbSource.Location = new System.Drawing.Point(8, 24);
			this.tbSource.Name = "tbSource";
			this.tbSource.Size = new System.Drawing.Size(296, 21);
			this.tbSource.TabIndex = 2;
			this.tbSource.Text = "";
			// 
			// tbDest
			// 
			this.tbDest.Location = new System.Drawing.Point(8, 72);
			this.tbDest.Name = "tbDest";
			this.tbDest.Size = new System.Drawing.Size(296, 21);
			this.tbDest.TabIndex = 3;
			this.tbDest.Text = "";
			// 
			// btnBrowseSource
			// 
			this.btnBrowseSource.Location = new System.Drawing.Point(312, 24);
			this.btnBrowseSource.Name = "btnBrowseSource";
			this.btnBrowseSource.TabIndex = 4;
			this.btnBrowseSource.Text = "Browse...";
			this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click);
			// 
			// btnBrowseDest
			// 
			this.btnBrowseDest.Location = new System.Drawing.Point(312, 72);
			this.btnBrowseDest.Name = "btnBrowseDest";
			this.btnBrowseDest.TabIndex = 5;
			this.btnBrowseDest.Text = "Browse...";
			this.btnBrowseDest.Click += new System.EventHandler(this.btnBrowseDest_Click);
			// 
			// gbSigProps
			// 
			this.gbSigProps.Controls.Add(this.cbSystemCerts);
			this.gbSigProps.Controls.Add(this.rbUseSystemStore);
			this.gbSigProps.Controls.Add(this.rbUseCertFile);
			this.gbSigProps.Controls.Add(this.tbTimestampServer);
			this.gbSigProps.Controls.Add(this.cbTimestamp);
			this.gbSigProps.Controls.Add(this.cbReason);
			this.gbSigProps.Controls.Add(this.lReason);
			this.gbSigProps.Controls.Add(this.tbAuthor);
			this.gbSigProps.Controls.Add(this.lAuthor);
			this.gbSigProps.Controls.Add(this.cbSignatureType);
			this.gbSigProps.Controls.Add(this.lSignatureType);
			this.gbSigProps.Controls.Add(this.btnBrowseCert);
			this.gbSigProps.Controls.Add(this.tbCertPassword);
			this.gbSigProps.Controls.Add(this.tbCert);
			this.gbSigProps.Controls.Add(this.lCertPassword);
			this.gbSigProps.Location = new System.Drawing.Point(8, 104);
			this.gbSigProps.Name = "gbSigProps";
			this.gbSigProps.Size = new System.Drawing.Size(376, 392);
			this.gbSigProps.TabIndex = 6;
			this.gbSigProps.TabStop = false;
			this.gbSigProps.Text = "Signature properties";
			// 
			// tbTimestampServer
			// 
			this.tbTimestampServer.Location = new System.Drawing.Point(32, 352);
			this.tbTimestampServer.Name = "tbTimestampServer";
			this.tbTimestampServer.Size = new System.Drawing.Size(328, 21);
			this.tbTimestampServer.TabIndex = 12;
			this.tbTimestampServer.Text = "http://";
			// 
			// cbTimestamp
			// 
			this.cbTimestamp.Location = new System.Drawing.Point(16, 328);
			this.cbTimestamp.Name = "cbTimestamp";
			this.cbTimestamp.Size = new System.Drawing.Size(336, 24);
			this.cbTimestamp.TabIndex = 11;
			this.cbTimestamp.Text = "Request a timestamp from TSA server:";
			// 
			// cbReason
			// 
			this.cbReason.Items.AddRange(new object[] {
														  "I am the author of this document",
														  "I agree to the terms defined by placement of my signature on this document",
														  "I have reviewed this document",
														  "I attest to the accuracy and integrity of this document",
														  "I am approving this document"});
			this.cbReason.Location = new System.Drawing.Point(16, 296);
			this.cbReason.Name = "cbReason";
			this.cbReason.Size = new System.Drawing.Size(344, 21);
			this.cbReason.TabIndex = 10;
			this.cbReason.Text = "<none>";
			// 
			// lReason
			// 
			this.lReason.Location = new System.Drawing.Point(16, 280);
			this.lReason.Name = "lReason";
			this.lReason.Size = new System.Drawing.Size(160, 16);
			this.lReason.TabIndex = 9;
			this.lReason.Text = "Reason for signing:";
			// 
			// tbAuthor
			// 
			this.tbAuthor.Location = new System.Drawing.Point(16, 248);
			this.tbAuthor.Name = "tbAuthor";
			this.tbAuthor.Size = new System.Drawing.Size(272, 21);
			this.tbAuthor.TabIndex = 8;
			this.tbAuthor.Text = "";
			// 
			// lAuthor
			// 
			this.lAuthor.Location = new System.Drawing.Point(16, 232);
			this.lAuthor.Name = "lAuthor";
			this.lAuthor.Size = new System.Drawing.Size(100, 16);
			this.lAuthor.TabIndex = 7;
			this.lAuthor.Text = "Author\'s name:";
			// 
			// cbSignatureType
			// 
			this.cbSignatureType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSignatureType.Items.AddRange(new object[] {
																 "Invisible document signature",
																 "Visible document signature",
																 "Certification (MDP) signature"});
			this.cbSignatureType.Location = new System.Drawing.Point(16, 200);
			this.cbSignatureType.Name = "cbSignatureType";
			this.cbSignatureType.Size = new System.Drawing.Size(344, 21);
			this.cbSignatureType.TabIndex = 6;
			// 
			// lSignatureType
			// 
			this.lSignatureType.Location = new System.Drawing.Point(16, 184);
			this.lSignatureType.Name = "lSignatureType";
			this.lSignatureType.Size = new System.Drawing.Size(160, 16);
			this.lSignatureType.TabIndex = 5;
			this.lSignatureType.Text = "Signature type:";
			// 
			// btnBrowseCert
			// 
			this.btnBrowseCert.Location = new System.Drawing.Point(288, 48);
			this.btnBrowseCert.Name = "btnBrowseCert";
			this.btnBrowseCert.TabIndex = 4;
			this.btnBrowseCert.Text = "Browse...";
			this.btnBrowseCert.Click += new System.EventHandler(this.btnBrowseCert_Click);
			// 
			// tbCertPassword
			// 
			this.tbCertPassword.Location = new System.Drawing.Point(32, 96);
			this.tbCertPassword.Name = "tbCertPassword";
			this.tbCertPassword.PasswordChar = '*';
			this.tbCertPassword.Size = new System.Drawing.Size(120, 21);
			this.tbCertPassword.TabIndex = 2;
			this.tbCertPassword.Text = "";
			// 
			// tbCert
			// 
			this.tbCert.Location = new System.Drawing.Point(32, 48);
			this.tbCert.Name = "tbCert";
			this.tbCert.Size = new System.Drawing.Size(248, 21);
			this.tbCert.TabIndex = 1;
			this.tbCert.Text = "";
			// 
			// lCertPassword
			// 
			this.lCertPassword.Location = new System.Drawing.Point(32, 80);
			this.lCertPassword.Name = "lCertPassword";
			this.lCertPassword.Size = new System.Drawing.Size(160, 16);
			this.lCertPassword.TabIndex = 0;
			this.lCertPassword.Text = "Certificate password:";
			// 
			// btnSign
			// 
			this.btnSign.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnSign.Location = new System.Drawing.Point(120, 504);
			this.btnSign.Name = "btnSign";
			this.btnSign.TabIndex = 7;
			this.btnSign.Text = "Sign";
			this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(200, 504);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// openDialogPDF
			// 
			this.openDialogPDF.Filter = "PDF document (*.pdf)|*.pdf|All files (*.*)|*.*";
			this.openDialogPDF.InitialDirectory = ".";
			// 
			// openDialogCert
			// 
			this.openDialogCert.Filter = "PKCS#12 certificate (*.pfx)|*.pfx|All files (*.*)|*.*";
			this.openDialogCert.InitialDirectory = ".";
			// 
			// saveDialogPDF
			// 
			this.saveDialogPDF.Filter = "PDF document (*.pdf)|*.pdf|All files (*.*)|*.*";
			this.saveDialogPDF.InitialDirectory = ".";
			// 
			// rbUseCertFile
			// 
			this.rbUseCertFile.Checked = true;
			this.rbUseCertFile.Location = new System.Drawing.Point(16, 24);
			this.rbUseCertFile.Name = "rbUseCertFile";
			this.rbUseCertFile.Size = new System.Drawing.Size(328, 24);
			this.rbUseCertFile.TabIndex = 13;
			this.rbUseCertFile.TabStop = true;
			this.rbUseCertFile.Text = "Use certificate from file:";
			// 
			// rbUseSystemStore
			// 
			this.rbUseSystemStore.Location = new System.Drawing.Point(16, 128);
			this.rbUseSystemStore.Name = "rbUseSystemStore";
			this.rbUseSystemStore.Size = new System.Drawing.Size(336, 24);
			this.rbUseSystemStore.TabIndex = 14;
			this.rbUseSystemStore.Text = "Use certificate from system certificate store:";
			// 
			// cbSystemCerts
			// 
			this.cbSystemCerts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSystemCerts.Location = new System.Drawing.Point(32, 152);
			this.cbSystemCerts.Name = "cbSystemCerts";
			this.cbSystemCerts.Size = new System.Drawing.Size(328, 21);
			this.cbSystemCerts.TabIndex = 15;
			// 
			// frmMain
			// 
			this.AcceptButton = this.btnSign;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(392, 535);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSign);
			this.Controls.Add(this.gbSigProps);
			this.Controls.Add(this.btnBrowseDest);
			this.Controls.Add(this.btnBrowseSource);
			this.Controls.Add(this.tbDest);
			this.Controls.Add(this.tbSource);
			this.Controls.Add(this.lDest);
			this.Controls.Add(this.lSource);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmMain";
			this.Text = "Tiny PDF signer";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.gbSigProps.ResumeLayout(false);
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
			if (openDialogPDF.ShowDialog() == DialogResult.OK) 
                tbSource.Text = openDialogPDF.FileName;
		}

		private void btnBrowseDest_Click(object sender, System.EventArgs e)
		{
			if (saveDialogPDF.ShowDialog() == DialogResult.OK) 
                tbDest.Text = saveDialogPDF.FileName;
		}

		private void btnBrowseCert_Click(object sender, System.EventArgs e)
		{
			if (openDialogCert.ShowDialog() == DialogResult.OK) 
                tbCert.Text = openDialogCert.FileName;
		}

		private void btnSign_Click(object sender, System.EventArgs e)
		{
			string TempPath = Path.GetTempFileName();
			bool Success = false;

			if ((rbUseSystemStore.Checked) && (cbSystemCerts.SelectedIndex < 0)) 
			{
                MessageBox.Show("Please select a signing certificate");
				return;
			}

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
							MessageBox.Show("The document is encrypted and cannot be signed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
						// adding the signature and setting up property values
						int index = Document.AddSignature();
                        TElPDFSignature Sig = Document.get_Signatures(index);
                        Sig.Handler = PublicKeyHandler;
                        Sig.AuthorName = tbAuthor.Text;
						Sig.SigningTime = DateTime.Now;
						if (String.Compare(cbReason.Text, "<none>") != 0) 
							Sig.Reason = cbReason.Text;
						else 
		                    Sig.Reason = "";
						// configuring signature type
						switch(cbSignatureType.SelectedIndex) 
						{
							case 0:
								Sig.Invisible = true;
								break;
							case 1:
								Sig.Invisible = false;
								break;
							case 2:
								Sig.Invisible = false;
								Sig.SignatureType = SBPDF.Unit.stMDP;
								break;
						}
						// loading certificate
						if (rbUseCertFile.Checked) 
						{
							FileStream CertF = new FileStream(tbCert.Text, FileMode.Open);
							try 
							{
								int CertFormat = TElX509Certificate.DetectCertFileFormat(CertF);
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
							finally {	CertF.Close();	}
						} 
						else 
						{
							Cert = SystemStore.get_Certificates(cbSystemCerts.SelectedIndex);
						}
						// adding certificate to certificate storage
						CertStorage.Clear();
						CertStorage.Add(Cert, true);
						PublicKeyHandler.CertStorage = CertStorage;
						PublicKeyHandler.SignatureType = SBPDFSecurity.TSBPDFPublicKeySignatureType.pstPKCS7SHA1;
						PublicKeyHandler.CustomName = "Adobe.PPKMS";
						// configuring timestamping settings
						if (cbTimestamp.Checked) 
						{
							TSPClient.HTTPClient = HTTPClient;
							TSPClient.HashAlgorithm = SBConstants.Unit.SB_ALGORITHM_DGST_SHA1;
							TSPClient.URL = tbTimestampServer.Text;
							PublicKeyHandler.TSPClient = TSPClient;
						}
						// allowing to save the document
						Success = true;
					} 
					finally 
					{
						// closing the document
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
				MessageBox.Show("Signing process successfully finished", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
			} 
			else 
				MessageBox.Show("Signing failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			// deleting the temporary file
			File.Delete(TempPath);
			this.Close();
		}

		private void frmMain_Load(object sender, System.EventArgs e)
		{
			cbSignatureType.SelectedIndex = 0;
			RefreshSystemCertificateList();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void RefreshSystemCertificateList() 
		{
			cbSystemCerts.Items.Clear();
            SystemStore.SystemStores.BeginUpdate();
			try 
			{
				SystemStore.SystemStores.Clear();
				SystemStore.SystemStores.Add("MY");
			} 
			finally 
			{
				SystemStore.SystemStores.EndUpdate();
			}
			for (int i = 0; i < SystemStore.Count; i++) 
			{
                cbSystemCerts.Items.Add(SystemStore.get_Certificates(i).SubjectName.CommonName);
			}
		}
	}
}
