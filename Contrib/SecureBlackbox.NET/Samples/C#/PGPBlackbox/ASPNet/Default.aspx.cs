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
using SBPGPKeys;
using SBPGP;

namespace PGPDemo
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.DropDownList cbPublicKey;
		protected System.Web.UI.WebControls.DropDownList cbSecretKey;
		protected System.Web.UI.HtmlControls.HtmlInputButton submit;
		protected System.Web.UI.WebControls.Label lblMessage;

		// object for storing a set of public and private keys
		private TElPGPKeyring keyring;
		// object for storing a public key that will be used in PGP operation
		private TElPGPKeyring publicKey;
		// object for storing a secret key that will be used in PGP operation
		private TElPGPKeyring secretKey;
		// an error message string
		private string errormsg = "";
		// link to a result file
		private string link = "";
		// output stream object
		private Stream outputStream;
	
		// loads a set of keys from a keyring file
		private void LoadKeyring()
		{
            keyring = new TElPGPKeyring();
			string path = HttpRuntime.AppDomainAppPath;
			keyring.Load(path + "keys\\pubring.pkr", path + "keys\\secring.skr", true);
		}

		// encrypts the source stream using chosen public key
		private bool Encrypt(Stream inF, Stream outF) 
		{
			TElPGPWriter writer = new TElPGPWriter();
			try 
			{
				writer.EncryptingKeys = publicKey;
				writer.UseNewFeatures = false;
				writer.UseOldPackets = true;
				writer.EncryptionType = SBPGP.TSBPGPEncryptionType.etPublicKey;
				writer.Timestamp = DateTime.Now;
				writer.Encrypt(inF, outF, 0);
			} 
			catch(Exception ex) 
			{
                errormsg = ex.Message;
				return false;
			}
            return true;
		}

		// signs the source stream using chosen secret key
		private bool Sign(Stream inF, Stream outF) 
		{
			if (!secretKey.get_SecretKeys(0).PassphraseValid()) 
			{
				errormsg = "Secret key passphrase is not valid";
				return false;
			}
			TElPGPWriter writer = new TElPGPWriter();
			try 
			{
				writer.SigningKeys = secretKey;
				writer.UseNewFeatures = false;
				writer.UseOldPackets = false;
				writer.Timestamp = DateTime.Now;
				writer.Sign(inF, outF, false, 0);
			} 
			catch(Exception ex) 
			{
				errormsg = ex.Message;
				return false;
			}
			return true;
		}

		// encrypts and signs the source stream using chosen secret and public keys
		private bool EncryptAndSign(Stream inF, Stream outF)
		{
			if (!secretKey.get_SecretKeys(0).PassphraseValid()) 
			{
				errormsg = "Secret key passphrase is not valid";
				return false;
			}
			TElPGPWriter writer = new TElPGPWriter();
			try 
			{
				writer.SigningKeys = secretKey;
				writer.EncryptingKeys = publicKey;
				writer.UseNewFeatures = true;
				writer.UseOldPackets = false;
				writer.Timestamp = DateTime.Now;
				writer.EncryptAndSign(inF, outF, 0);
			} 
			catch(Exception ex) 
			{
				errormsg = ex.Message;
				return false;
			}
			return true;
		}

		// processes the stream and decrypts it if it is encrypted
		private bool ProcessPGP(Stream inF, Stream outF)
		{
			TElPGPReader reader = new TElPGPReader();
			try
			{
				reader.DecryptingKeys = secretKey;
				reader.VerifyingKeys = publicKey;
				outputStream = outF;
				reader.OnCreateOutputStream += new TSBPGPCreateOutputStreamEvent(reader_OnCreateOutputStream);
				reader.DecryptAndVerify(inF, 0);
			} 
			catch(Exception ex) 
			{
				errormsg = ex.Message;
				return false;
			}
			return true;
		}

		// performs a PGP operation basing on chosen options
		private bool PerformPGPOperation() 
		{
			bool result = false;
			
			// loading selected keys
			publicKey = new TElPGPKeyring();
			secretKey = new TElPGPKeyring();
			int pubIndex, secIndex;
			try 
			{
				pubIndex = System.Convert.ToInt32(Request.Params["cbPublicKey"]);
				secIndex = System.Convert.ToInt32(Request.Params["cbSecretKey"]);
			} 
			catch(Exception ex) 
			{
                errormsg = "Invalid public or secret key index";
				return false;
			}
			publicKey.AddPublicKey(keyring.get_PublicKeys(pubIndex));
			secretKey.AddSecretKey(keyring.get_SecretKeys(secIndex));
			secretKey.get_SecretKeys(0).Passphrase = Request.Params["password"];
			
			// processing uploaded file
			HttpFileCollection files = Request.Files;
			if (files.AllKeys.Length <= 0)
			{
				errormsg = "No files uploaded";
                return false;
			}
			HttpPostedFile f;
			f = files[0];
			if ((f.InputStream == null) || (f.FileName == null) || (f.FileName.Length <= 0))
			{
                errormsg = "Cannot access uploaded file";
				return false;
			}

			// creating output file
			string op = Request.Params["operation"];
			string fname = "";
			if (op != "4") 
			{
				fname = System.IO.Path.GetFileName(f.FileName) + ".pgp";
			} 
			else 
			{
				fname = "f" + Environment.TickCount.ToString() + ".bin";
			}
			link = HttpRuntime.AppDomainAppVirtualPath + "/files/" + fname;
			fname = HttpRuntime.AppDomainAppPath + "files\\" + fname;
			FileStream outF = new FileStream(fname, FileMode.Create);
			try 
			{
				// reading operation code
				if (op == "1") 
				{
					result = Encrypt(f.InputStream, outF);
				} 
				else if (op == "2") 
				{
					result = Sign(f.InputStream, outF);
				} 
				else if (op == "3") 
				{
					result = EncryptAndSign(f.InputStream, outF);
				} 
				else if (op == "4") 
				{
					result = ProcessPGP(f.InputStream, outF);
				} 
				else 
				{
					errormsg = "Unsupported PGP operation";
				}
			}
			finally 
			{
				outF.Close();
			}
			return result;
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// setting a license key
			SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"));
			
			// loading keys
			LoadKeyring();
            
			// reading parameters
			if (Request.Params["submit"] != null) 
			{
				// processing uploaded file
				if (PerformPGPOperation()) 
				{
					lblMessage.Text = "<i>The document was successfully processed</i>. <a href='" + link + "'>Download</a>";
					return;
				} 
				else 
				{
					lblMessage.Text = "<font color='#ff0000'>PGP operation failed: " + errormsg + "</font>";
				}
			} 
			else 
			{
				lblMessage.Text = "<i>Please complete and submit the form below:</i>";
			}
			
			// Adding keys to combo box controls
			string user;
			cbPublicKey.Items.Clear();
			for (int i = 0; i < keyring.PublicCount; i++) 
			{
				if (keyring.get_PublicKeys(i).UserIDCount > 0) 
				{
					user = keyring.get_PublicKeys(i).get_UserIDs(0).Name;
				} 
				else 
				{
					user = "<Untitled>";
				}
				System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem(user, i.ToString());
				cbPublicKey.Items.Add(item);
			}
			cbSecretKey.Items.Clear();
			for (int i = 0; i < keyring.SecretCount; i++) 
			{
				if ((keyring.get_SecretKeys(i).PublicKey != null) && (keyring.get_SecretKeys(i).PublicKey.UserIDCount > 0))
				{
					user = keyring.get_SecretKeys(i).PublicKey.get_UserIDs(0).Name;
				} 
				else 
				{
					user = "<Untitled>";
				}
				System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem(user, i.ToString());
				cbSecretKey.Items.Add(item);
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

		private void reader_OnCreateOutputStream(object Sender, string Filename, DateTime TimeStamp, ref Stream Stream, ref bool FreeOnExit)
		{
            Stream = outputStream;
			FreeOnExit = false;
		}
	}
}
