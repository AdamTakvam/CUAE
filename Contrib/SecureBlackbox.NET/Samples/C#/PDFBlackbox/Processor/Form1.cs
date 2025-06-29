using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using SBPDF;
using SBPDFSecurity;
using SBX509;
using SBCustomCertStorage;

namespace TinyProcessor
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private TElPDFDocument Document;
		private TElMemoryCertStorage CertStorage;
		private TElX509Certificate Cert;
		private System.Windows.Forms.Label lSelectSource;
		private System.Windows.Forms.TextBox tbSource;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.OpenFileDialog openDialogPDF;
		private System.Windows.Forms.OpenFileDialog openDialogCert;
		private System.Windows.Forms.SaveFileDialog saveDialogPDF;
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
			// SecureBlackbox initialization
			// Calling SetLicenseKey
			SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"));

			// the following two functions *must* be called before PDF functionality is used
			SBPDF.Unit.Initialize();
			SBPDFSecurity.Unit.Initialize();
			// creating objects
			Document = new TElPDFDocument();
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
			this.lSelectSource = new System.Windows.Forms.Label();
			this.tbSource = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.openDialogPDF = new System.Windows.Forms.OpenFileDialog();
			this.openDialogCert = new System.Windows.Forms.OpenFileDialog();
			this.saveDialogPDF = new System.Windows.Forms.SaveFileDialog();
			this.SuspendLayout();
			// 
			// lSelectSource
			// 
			this.lSelectSource.Location = new System.Drawing.Point(8, 8);
			this.lSelectSource.Name = "lSelectSource";
			this.lSelectSource.Size = new System.Drawing.Size(280, 16);
			this.lSelectSource.TabIndex = 0;
			this.lSelectSource.Text = "Please select the PDF document:";
			// 
			// tbSource
			// 
			this.tbSource.Location = new System.Drawing.Point(8, 24);
			this.tbSource.Name = "tbSource";
			this.tbSource.Size = new System.Drawing.Size(280, 21);
			this.tbSource.TabIndex = 1;
			this.tbSource.Text = "";
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(296, 24);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.TabIndex = 2;
			this.btnBrowse.Text = "Browse...";
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(112, 56);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(192, 56);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// openDialogPDF
			// 
			this.openDialogPDF.Filter = "PDF documents (*.pdf)|*.pdf|All files (*.*)|*.*";
			this.openDialogPDF.InitialDirectory = ".";
			// 
			// openDialogCert
			// 
			this.openDialogCert.Filter = "PKCS#12 files (*.pfx)|*.pfx|All files (*.*)|*.*";
			this.openDialogCert.InitialDirectory = ".";
			this.openDialogCert.Title = "Please select a certificate to decrypt the document";
			// 
			// saveDialogPDF
			// 
			this.saveDialogPDF.Filter = "PDF document (*.pdf)|*.pdf|All files (*.*)|*.*";
			this.saveDialogPDF.InitialDirectory = ".";
			this.saveDialogPDF.Title = "Please select a location where to save the decrypted file";
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(378, 87);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.tbSource);
			this.Controls.Add(this.lSelectSource);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Tiny PDF Processor";
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

		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
			if (openDialogPDF.ShowDialog() == DialogResult.OK) 
                tbSource.Text = openDialogPDF.FileName;
		}

		/// <summary>
		/// Request certificate from user into internal variable Cert
		/// </summary>
		/// <returns>True if certificate was loaded</returns>
		private bool RequestCertificate() 
		{
			if (openDialogCert.ShowDialog() == DialogResult.OK) 
			{
				try 
				{
					int R = 0;
					FileStream F = new FileStream(openDialogCert.FileName, FileMode.Open);
					try 
					{
						String Pass = frmRequestPassword.RequestPassword("Password is needed to decrypt the certificate:");
						R = Cert.LoadFromStreamPFX(F, Pass, 0);
					}
					finally { F.Close();}
					if (R != 0) 
						MessageBox.Show("Failed to load certificate, error " + R.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					else 
						return(true);
				} 
				catch(Exception ex) 
				{
					MessageBox.Show("Failed to read certificate " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			return false;
		}

		private bool DisplayEncryptionInfo() 
		{
			frmEncryptionProps dlg = new frmEncryptionProps();
			dlg.Init(Document);
			return (dlg.ShowDialog() == DialogResult.OK);
		}

		private void DisplaySignaturesInfo() 
		{
            frmSigProps dlg = new frmSigProps();
			dlg.Init(Document);
			dlg.ShowDialog();
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			string TempPath = Path.GetTempFileName();
			bool DocumentChanged = false;
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
						// checking if the document is secured
						if ((!Document.Encrypted) && (!Document.Signed)) 
						{
                            MessageBox.Show("Document is neither encrypted nor signed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
							return;
						}
                        // checking if the document is encrypted and trying to decrypt it
						if (Document.Encrypted) 
						{
							if (DisplayEncryptionInfo())
							{
								// decrypting the document
								if (Document.EncryptionHandler is TElPDFPasswordSecurityHandler) 
								{
                                    string pass = frmRequestPassword.RequestPassword("Please supply a password to decrypt the document:");
									// trying the supplied password
									bool PasswordValid = false;
									TElPDFPasswordSecurityHandler PasswordHandler = 
										(TElPDFPasswordSecurityHandler)(Document.EncryptionHandler);
									// Check for owner password
									PasswordHandler.OwnerPassword = pass;
									if (PasswordHandler.IsOwnerPasswordValid()) 
										PasswordValid = true;
									else 
									{
										// Check for user password
										PasswordHandler.UserPassword = pass;
										if (PasswordHandler.IsUserPasswordValid()) PasswordValid = true;
									}
									if (PasswordValid) 
									{
										Document.Decrypt();
										DocumentChanged = true;
										MessageBox.Show("The document was successfully decrypted", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
									} 
									else 
                                        MessageBox.Show("Invalid password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
								} 
								else if (Document.EncryptionHandler is TElPDFPublicKeySecurityHandler) 
								{
									// requesting certificate from user
									if (RequestCertificate()) 
									{
                                        CertStorage.Clear();
                                        CertStorage.Add(Cert, true);
						                ((TElPDFPublicKeySecurityHandler)(Document.EncryptionHandler)).CertStorage = CertStorage;
                                        Document.Decrypt();
										DocumentChanged = true;
										MessageBox.Show("The document was successfully decrypted", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
									} 
									else 
										MessageBox.Show("No certificate for decryption found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
								} 
								else 
								{
									MessageBox.Show("The document is encrypted with unsupported security handler", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
								}
							}
						}
                        // displaying signatures for signed documents
						if (Document.Signed) DisplaySignaturesInfo();
					} 
					finally 
					{
						// closing the document
						Document.Close(DocumentChanged);
					}
				} 
				finally {	F.Close();	}
			} 
			catch(Exception ex) 
			{
				MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				DocumentChanged = false;
			}
			// if the document was changed (e.g, decrypted), moving the temporary file to the place
			// of destination file
			if (DocumentChanged) 
			{
				if (saveDialogPDF.ShowDialog() == DialogResult.OK) 
				{
					File.Copy(TempPath, saveDialogPDF.FileName, true);
					MessageBox.Show("The decrypted document was successfully saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			} 
			// deleting temporary file
            File.Delete(TempPath);
			this.Close();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}
}
