using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using SBX509;
using SBPDF;
using SBPDFSecurity;
using SBCustomCertStorage;

namespace PDFDemo
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public class MainForm : System.Web.UI.Page
	{
		// declaring class variables
		private TElX509Certificate cert = null;
		private string certpass = "";
		private string msg = "";
		private string link = "";
		private bool error = false;
		
		protected System.Web.UI.WebControls.Label lblPrompt;
	
		// loads the uploaded certificate into cert object if possible
		private bool LoadCertificate() 
		{
			bool result = false;
			HttpFileCollection files = Request.Files;
			HttpPostedFile f = null;
			for (int i = 0; i < files.AllKeys.Length; i++) 
			{
				if (files.AllKeys[i] == "cert") 
				{
					f = files[i];
				}
			}
			if ((f != null) && (f.FileName.Length > 0))
			{
				certpass = Request.Params["certpass"];
				long oldpos = f.InputStream.Position;
				cert = new TElX509Certificate();
				int format = TElX509Certificate.DetectCertFileFormat(f.InputStream);
				f.InputStream.Position = oldpos;
				int r = 0;
				switch(format) 
				{
					case SBX509.Unit.cfDER:
						try 
						{
							cert.LoadFromStream(f.InputStream, 0);
							result = true;
						} 
						catch(Exception ex) 
						{
							error = true;
							msg = "Failed to load certificate: " + ex.Message;
						}
						break;
					case SBX509.Unit.cfPEM:
						r = cert.LoadFromStreamPEM(f.InputStream, certpass, 0);
						if (r != 0) 
						{
							error = true;
							msg = "PEM read error " + r.ToString();
						} 
						else 
						{
                            result = true;
						}
						break;
					case SBX509.Unit.cfPFX:
						r = cert.LoadFromStreamPFX(f.InputStream, certpass, 0);
						if (r != 0) 
						{
							error = true;
							msg = "PFX read error " + r.ToString();
						} 
						else 
						{
							result = true;
						}
						break;
					default:
						error = true;
						msg = "Unsupported certificate format";
						break;
				}
			}
			if (error) 
			{
				cert = null;
			}
            return result;
		}

		// creates a copy of uploaded document.
		// all the operations are performed over this copy.
		private Stream LoadDocument() 
		{
			Stream result = null;
			HttpFileCollection files = Request.Files;
			HttpPostedFile f = null;
			for (int i = 0; i < files.AllKeys.Length; i++) 
			{
				if (files.AllKeys[i] == "document") 
				{
					f = files[i];
				}
			}
			if ((f != null) && (f.FileName.Length > 0))
			{
				string fname = Environment.TickCount.ToString() + ".pdf";
				link = HttpRuntime.AppDomainAppVirtualPath + "/files/" + fname;
				fname = HttpRuntime.AppDomainAppPath + "files\\" + fname;
				FileStream outputStream = new FileStream(fname, FileMode.Create, FileAccess.ReadWrite);
				byte[] buffer = new byte[4096];
				while (f.InputStream.Position < f.InputStream.Length) 
				{
					int len = f.InputStream.Read(buffer, 0, buffer.Length);
                    outputStream.Write(buffer, 0, len);
				}
				outputStream.Position = 0;
				result = outputStream;
			}
			return result;
		}

		// encrypts the document using the uploaded certificate
		private void EncryptPublicKey(Stream stream) 
		{
			TElPDFDocument document = new TElPDFDocument();
			TElPDFPublicKeySecurityHandler handler = new TElPDFPublicKeySecurityHandler();
			try 
			{
				document.Open(stream);
                document.EncryptionHandler = handler;
				int groupIndex = handler.AddRecipientGroup();
				handler.get_RecipientGroups(groupIndex).AddRecipient(cert);
				handler.StreamEncryptionAlgorithm = SBConstants.Unit.SB_ALGORITHM_CNT_RC4;
				handler.StreamEncryptionKeyBits = 128;
				handler.StringEncryptionAlgorithm = handler.StreamEncryptionAlgorithm;
				handler.StringEncryptionKeyBits = handler.StreamEncryptionKeyBits;
				document.Encrypt();
				document.Close(true);
			} 
			catch(Exception ex) 
			{
                error = true;
				msg = "Document processing error: " + ex.Message;
			}
		}

		// encrypts the document using password
		private void EncryptPassword(Stream stream)
		{
			TElPDFDocument document = new TElPDFDocument();
			TElPDFPasswordSecurityHandler handler = new TElPDFPasswordSecurityHandler();
			string pass = Request.Params["docpass"];
			try 
			{
				document.Open(stream);
				document.EncryptionHandler = handler;
				handler.OwnerPassword = pass;
				handler.UserPassword = pass;
				handler.StreamEncryptionAlgorithm = SBConstants.Unit.SB_ALGORITHM_CNT_RC4;
				handler.StreamEncryptionKeyBits = 128;
				handler.StringEncryptionAlgorithm = handler.StreamEncryptionAlgorithm;
				handler.StringEncryptionKeyBits = handler.StreamEncryptionKeyBits;
				handler.Permissions.EnableAll();
				document.Encrypt();
				document.Close(true);
			} 
			catch(Exception ex) 
			{
				error = true;
				msg = "Document processing error: " + ex.Message;
			}
		}

		// signs the document using the uploaded certificate
		private void Sign(Stream stream)
		{
			TElPDFDocument document = new TElPDFDocument();
			TElPDFPublicKeySecurityHandler handler = new TElPDFPublicKeySecurityHandler();
			TElMemoryCertStorage storage = new TElMemoryCertStorage();
			storage.Add(cert, true);
			try 
			{
				document.Open(stream);
				int sigIndex = document.AddSignature();
				TElPDFSignature sig = document.get_Signatures(sigIndex);
				sig.Handler = handler;
				handler.CertStorage = storage;
				handler.SignatureType = SBPDFSecurity.TSBPDFPublicKeySignatureType.pstPKCS7SHA1;
				sig.SignatureType = SBPDF.Unit.stDocument;
				sig.AuthorName = "EldoS PDFBlackbox";
				document.Close(true);
			} 
			catch(Exception ex) 
			{
				error = true;
				msg = "Document processing error: " + ex.Message;
			}
		}

		// certifies (creates MDP signature) the document using the uploaded certificate
		private void Certify(Stream stream)
		{
			TElPDFDocument document = new TElPDFDocument();
			TElPDFPublicKeySecurityHandler handler = new TElPDFPublicKeySecurityHandler();
			TElMemoryCertStorage storage = new TElMemoryCertStorage();
			storage.Add(cert, true);
			try 
			{
				document.Open(stream);
				int sigIndex = document.AddSignature();
				TElPDFSignature sig = document.get_Signatures(sigIndex);
				sig.Handler = handler;
				handler.CertStorage = storage;
				handler.SignatureType = SBPDFSecurity.TSBPDFPublicKeySignatureType.pstPKCS7SHA1;
				sig.SignatureType = SBPDF.Unit.stMDP;
				sig.AuthorName = "EldoS PDFBlackbox";
				document.Close(true);
			} 
			catch(Exception ex) 
			{
				error = true;
				msg = "Document processing error: " + ex.Message;
			}
		}

		// decrypts the document or verifies its signatures
		private void DecryptAndVerify(Stream stream) 
		{
			TElPDFDocument document = new TElPDFDocument();
			TElMemoryCertStorage storage = new TElMemoryCertStorage();
			if (cert != null) 
			{
				storage.Add(cert, true);
			}
			try 
			{
				document.Open(stream);
				if (document.Encrypted) 
				{
					if (document.EncryptionHandler is TElPDFPasswordSecurityHandler) 
					{
						string pass = Request.Params["docpass"];
						TElPDFPasswordSecurityHandler handler = (TElPDFPasswordSecurityHandler)document.EncryptionHandler;
						handler.UserPassword = pass;
						handler.OwnerPassword = pass;
						document.Decrypt();
						msg = "Document was successfully decrypted with password. " + "<a href=\"" + link + "\">Download</a>.";
					} 
					else if (document.EncryptionHandler is TElPDFPublicKeySecurityHandler) 
					{
						TElPDFPublicKeySecurityHandler handler = (TElPDFPublicKeySecurityHandler)document.EncryptionHandler;
						handler.CertStorage = storage;
						document.Decrypt();
						msg = "Document was successfully decrypted with certificate. " + "<a href=\"" + link + "\">Download</a>.";
					} 
					else 
					{
						error = true;
						msg = "Unsupported security handler type";
					}
				} 
				else if (document.Signed) 
				{
					int valid = 0;
					int invalid = 0;
					for (int i = 0; i < document.SignatureCount; i++) 
					{
						if (document.get_Signatures(i).Validate()) 
						{
							valid++;
						} 
						else 
						{
                            invalid++;
						}
					}
					msg = "Document is signed. Validation results: " + valid.ToString() + " valid signatures, " + invalid.ToString() + " invalid signatures";
				} 
				else 
				{
                    msg = "Document is neither encrypted nor signed";
				}
				document.Close(true);
			} 
			catch(Exception ex) 
			{
				error = true;
				msg = "Document processing error: " + ex.Message;
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			// setting a license key
			SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"));

			// initializing PDF library
			SBPDF.Unit.Initialize();
			SBPDFSecurity.Unit.Initialize();

			// reading parameters
			if (Request.Params["submit"] != null) 
			{
                string op = Request.Params["cbOperation"];
				Stream doc = LoadDocument();
				if (doc != null) 
				{
					if (op == "1") 
					{
						EncryptPassword(doc);
					} 
					else if (op == "2") 
					{
						if (LoadCertificate()) 
						{
							EncryptPublicKey(doc);
						} 
					} 
					else if (op == "3") 
					{
						if (LoadCertificate()) 
						{
							Sign(doc);
						}
					} 
					else if (op == "4") 
					{
						if (LoadCertificate()) 
						{
							Certify(doc);	
						}
					} 
					else if (op == "5") 
					{	
						LoadCertificate();
						error = false;
						DecryptAndVerify(doc);                        
					}
					doc.Close();
					if ((!error) && (!(op == "5")))
					{
						msg = "The document was processed successfully. <a href=\"" + link + "\">Download</a>";
					}
				} 
				else 
				{
					msg = "No document specified";
					error = true;
				}
				if (error) 
				{
                    lblPrompt.Text = "<font color='#ff0000'>Error: " + msg + "</div>";
				} 
				else 
				{
					lblPrompt.Text = msg;
				}
			} 
			else 
			{
				lblPrompt.Text = "Please complete and submit the form below.";
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
