using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

using SBXMLCore;
using SBXMLDefs;
using SBXMLSec;
using SBXMLEnc;

using SBPGPKeys;
using SBX509;

namespace SimpleEncryptor
{
	/// <summary>
	/// Summary description for frmMain.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lbXMLFile;
		private System.Windows.Forms.TextBox edXMLFile;
		private System.Windows.Forms.Button sbBrowseXMLFile;
		private System.Windows.Forms.TreeView tvXML;
		private System.Windows.Forms.Button btnLoadXML;
		private System.Windows.Forms.Button btnSaveXML;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Button btnEncrypt;
		private System.Windows.Forms.Button btnDecrypt;
		private System.Windows.Forms.Label lbNodeType;
		private System.Windows.Forms.Label lbNamespaceURI;
		private System.Windows.Forms.TextBox mmXML;
		private System.Windows.Forms.Label dlbNodeType;
		private System.Windows.Forms.Label dlbNamespaceURI;
		private System.Windows.Forms.OpenFileDialog dlgOpenXML;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private TElXMLDOMDocument FXMLDocument = null;

		private EncForm frmEnc = null;

		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			frmEnc = new EncForm();

			// Calling SetLicenseKey
			SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0566A0892842A9A7E2957B21B3D81A1C6EDB361CFEC2CCCA088A386D658ED927B01744E8DFBC590717631F42F840ED63C0322DDA17AA712010211551FFCD6042AB769E2D8FE083C15338C99902232783FAB30AA65EEBEE98338B8FCBC1AFE342DF79686C1E1587E6E3EACCBF9DA720F12BA80C66CE2191BDE832BB59AB459236B12FC1EFFC0FDDB198869B2E95FC5C5593FE6D69FEA95AC03E97D4F78C948C85AD18E5589A7E827E7D09AB04FEB7C69C0AA7ED2530F8AEE623BCE705D4F39E1644CF22872C3425C2A260234AE3410F32642FE0683781FC6833F5A5BA7306488BCE4F7D13D91E892DAE4C908D92415BF05F61A380CB8CB796F047334B58FE79FA"));

			FXMLDocument = new TElXMLDOMDocument();
			UpdateXML();
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
					FXMLDocument.Dispose();
					frmEnc.Dispose();
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
			this.lbXMLFile = new System.Windows.Forms.Label();
			this.edXMLFile = new System.Windows.Forms.TextBox();
			this.sbBrowseXMLFile = new System.Windows.Forms.Button();
			this.tvXML = new System.Windows.Forms.TreeView();
			this.btnLoadXML = new System.Windows.Forms.Button();
			this.btnSaveXML = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnClear = new System.Windows.Forms.Button();
			this.btnEncrypt = new System.Windows.Forms.Button();
			this.btnDecrypt = new System.Windows.Forms.Button();
			this.lbNodeType = new System.Windows.Forms.Label();
			this.lbNamespaceURI = new System.Windows.Forms.Label();
			this.mmXML = new System.Windows.Forms.TextBox();
			this.dlgOpenXML = new System.Windows.Forms.OpenFileDialog();
			this.dlbNodeType = new System.Windows.Forms.Label();
			this.dlbNamespaceURI = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lbXMLFile
			// 
			this.lbXMLFile.Location = new System.Drawing.Point(8, 17);
			this.lbXMLFile.Name = "lbXMLFile";
			this.lbXMLFile.Size = new System.Drawing.Size(48, 13);
			this.lbXMLFile.TabIndex = 0;
			this.lbXMLFile.Text = "XML file:";
			// 
			// edXMLFile
			// 
			this.edXMLFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.edXMLFile.Location = new System.Drawing.Point(64, 14);
			this.edXMLFile.Name = "edXMLFile";
			this.edXMLFile.Size = new System.Drawing.Size(330, 20);
			this.edXMLFile.TabIndex = 1;
			this.edXMLFile.Text = "";
			// 
			// sbBrowseXMLFile
			// 
			this.sbBrowseXMLFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.sbBrowseXMLFile.Location = new System.Drawing.Point(400, 12);
			this.sbBrowseXMLFile.Name = "sbBrowseXMLFile";
			this.sbBrowseXMLFile.Size = new System.Drawing.Size(23, 22);
			this.sbBrowseXMLFile.TabIndex = 2;
			this.sbBrowseXMLFile.Text = "...";
			this.sbBrowseXMLFile.Click += new System.EventHandler(this.sbBrowseXMLFile_Click);
			// 
			// tvXML
			// 
			this.tvXML.ImageIndex = -1;
			this.tvXML.Location = new System.Drawing.Point(9, 49);
			this.tvXML.Name = "tvXML";
			this.tvXML.SelectedImageIndex = -1;
			this.tvXML.Size = new System.Drawing.Size(333, 343);
			this.tvXML.TabIndex = 3;
			this.tvXML.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvXML_AfterSelect);
			// 
			// btnLoadXML
			// 
			this.btnLoadXML.Location = new System.Drawing.Point(350, 56);
			this.btnLoadXML.Name = "btnLoadXML";
			this.btnLoadXML.TabIndex = 4;
			this.btnLoadXML.Text = "Load XML";
			this.btnLoadXML.Click += new System.EventHandler(this.btnLoadXML_Click);
			// 
			// btnSaveXML
			// 
			this.btnSaveXML.Location = new System.Drawing.Point(350, 88);
			this.btnSaveXML.Name = "btnSaveXML";
			this.btnSaveXML.TabIndex = 5;
			this.btnSaveXML.Text = "Save XML";
			this.btnSaveXML.Click += new System.EventHandler(this.btnSaveXML_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(350, 160);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 6;
			this.btnDelete.Text = "Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnClear
			// 
			this.btnClear.Location = new System.Drawing.Point(350, 192);
			this.btnClear.Name = "btnClear";
			this.btnClear.TabIndex = 7;
			this.btnClear.Text = "Clear";
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// btnEncrypt
			// 
			this.btnEncrypt.Location = new System.Drawing.Point(350, 336);
			this.btnEncrypt.Name = "btnEncrypt";
			this.btnEncrypt.TabIndex = 8;
			this.btnEncrypt.Text = "Encrypt";
			this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
			// 
			// btnDecrypt
			// 
			this.btnDecrypt.Location = new System.Drawing.Point(350, 368);
			this.btnDecrypt.Name = "btnDecrypt";
			this.btnDecrypt.TabIndex = 9;
			this.btnDecrypt.Text = "Decrypt";
			this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
			// 
			// lbNodeType
			// 
			this.lbNodeType.Location = new System.Drawing.Point(8, 400);
			this.lbNodeType.Name = "lbNodeType";
			this.lbNodeType.Size = new System.Drawing.Size(64, 14);
			this.lbNodeType.TabIndex = 10;
			this.lbNodeType.Text = "Node type:";
			// 
			// lbNamespaceURI
			// 
			this.lbNamespaceURI.Location = new System.Drawing.Point(8, 416);
			this.lbNamespaceURI.Name = "lbNamespaceURI";
			this.lbNamespaceURI.Size = new System.Drawing.Size(96, 14);
			this.lbNamespaceURI.TabIndex = 11;
			this.lbNamespaceURI.Text = "Namespace URI:";
			// 
			// mmXML
			// 
			this.mmXML.Location = new System.Drawing.Point(8, 432);
			this.mmXML.Multiline = true;
			this.mmXML.Name = "mmXML";
			this.mmXML.Size = new System.Drawing.Size(416, 128);
			this.mmXML.TabIndex = 12;
			this.mmXML.Text = "";
			// 
			// dlbNodeType
			// 
			this.dlbNodeType.Location = new System.Drawing.Point(104, 400);
			this.dlbNodeType.Name = "dlbNodeType";
			this.dlbNodeType.Size = new System.Drawing.Size(184, 14);
			this.dlbNodeType.TabIndex = 13;
			this.dlbNodeType.Text = "none";
			// 
			// dlbNamespaceURI
			// 
			this.dlbNamespaceURI.Location = new System.Drawing.Point(104, 416);
			this.dlbNamespaceURI.Name = "dlbNamespaceURI";
			this.dlbNamespaceURI.Size = new System.Drawing.Size(184, 14);
			this.dlbNamespaceURI.TabIndex = 14;
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(432, 566);
			this.Controls.Add(this.dlbNamespaceURI);
			this.Controls.Add(this.dlbNodeType);
			this.Controls.Add(this.mmXML);
			this.Controls.Add(this.edXMLFile);
			this.Controls.Add(this.lbNamespaceURI);
			this.Controls.Add(this.lbNodeType);
			this.Controls.Add(this.btnDecrypt);
			this.Controls.Add(this.btnEncrypt);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnSaveXML);
			this.Controls.Add(this.btnLoadXML);
			this.Controls.Add(this.tvXML);
			this.Controls.Add(this.sbBrowseXMLFile);
			this.Controls.Add(this.lbXMLFile);
			this.Name = "frmMain";
			this.Text = "Simple Encryptor";
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

		private TreeNode UpdateXML_AddNode(TreeNode Sibling, TElXMLDOMNode Node)
		{
			TElXMLDOMNode T;
			TreeNode AttrNode;
			TreeNode Result;
			TElXMLDOMNamedNodeMap Attributes;
			int i;
			string s;
			if (Node is TElXMLDOMDocument)
				s = ((TElXMLDOMDocument)Node).LocalName;
			else if (Node is TElXMLDOMElement) 
				s = ((TElXMLDOMElement)Node).NodeName;
			else if (Node is TElXMLDOMAttr) 
				s = ((TElXMLDOMAttr)Node).NodeName;
			else
				s = Node.NodeName;

			Result = new TreeNode(s);
			Result.Tag = Node;
			if (Sibling != null)
				Sibling.Nodes.Add(Result);
			else
				tvXML.Nodes.Add(Result);

			if (Node is TElXMLDOMElement)
			{
				Attributes = ((TElXMLDOMElement)Node).Attributes;
				if ((Attributes != null) && (Attributes.Length > 0))
				{
					AttrNode = Result;
					for (i = 0; i < Attributes.Length; i++)
						UpdateXML_AddNode(AttrNode, Attributes[i]);
				}
			}

			if (Node is TElXMLDOMAttr)
				return Result;

			T = Node.FirstChild;
			while (T != null)
			{
				if ((T is TElXMLDOMElement) ||
					(T is TElXMLDOMAttr) ||
					((T is TElXMLDOMText) && (((TElXMLDOMText)T).NodeValue.Trim().Length > 0)))
					UpdateXML_AddNode(Result, T);

				T = T.NextSibling;
			}

			return Result;
		}

		public void UpdateXML()
		{
			mmXML.Clear();
			tvXML.BeginUpdate();
			try
			{
				tvXML.Nodes.Clear();
				UpdateXML_AddNode(null, FXMLDocument);
				tvXML.Nodes[0].Expand();
			}
			finally 
			{
				tvXML.EndUpdate();
			}
		}

		private void sbBrowseXMLFile_Click(object sender, System.EventArgs e)
		{
			dlgOpenXML.InitialDirectory = Application.StartupPath + "\\..\\..\\Samples";
			dlgOpenXML.FileName = edXMLFile.Text;
			if (dlgOpenXML.ShowDialog() == DialogResult.OK)
				edXMLFile.Text = dlgOpenXML.FileName;		
		}

		private void btnLoadXML_Click(object sender, System.EventArgs e)
		{
			FileStream F = null;
			try
			{
				F = new FileStream(edXMLFile.Text, FileMode.Open, FileAccess.Read);
				FXMLDocument.LoadFromStream(F);
			}
			catch (Exception E)
			{
				MessageBox.Show("Error: " + E.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (F != null)
				F.Close();

			UpdateXML();
		}

		private void tvXML_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			TElXMLDOMNode N;
			string s, nt;
			if ((tvXML.SelectedNode != null) &&
				(tvXML.SelectedNode.Tag != null))
			{
				N = (TElXMLDOMNode)tvXML.SelectedNode.Tag;
				if (N is TElXMLDOMAttr)
					s = ((TElXMLDOMAttr)N).NodeValue;
				else
					s = N.OuterXML;

				dlbNamespaceURI.Text = N.NamespaceURI;
				mmXML.Text = s.Replace("\n", "\r\n");
				if (N is TElXMLDOMAttr)
					nt = "Attribute";
				else
					if (N is TElXMLDOMElement)
				{
					if ((N.ParentNode != null) &&
						!(N.ParentNode is TElXMLDOMDocument))
						nt = "Element";
					else
						nt = "Root element";
				}
				else
					if (N is TElXMLDOMText) 
					nt = "Text";
				else
					if (N is TElXMLDOMComment)
					nt = "Comment";
				else
					if (N is TElXMLDOMDocument)
					nt = "Document";
				else
					nt = "Unknown";

				dlbNodeType.Text = nt;
			}
			else
			{
				mmXML.Text = "";
				dlbNodeType.Text = "None";
				dlbNamespaceURI.Text = "";
			}		
		}

		private void btnClear_Click(object sender, System.EventArgs e)
		{
			FXMLDocument.Dispose();
			FXMLDocument = new TElXMLDOMDocument();
			UpdateXML();
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			TElXMLDOMNode N;
			if ((tvXML.SelectedNode != null) &&
				(tvXML.SelectedNode.Tag != null)) 
			{
				N = (TElXMLDOMNode)tvXML.SelectedNode.Tag;
				if ((N is TElXMLDOMElement) ||
					(N is TElXMLDOMText))
				{
					if (N.ParentNode != null) 
						N.ParentNode.RemoveChild(N);
					else
						FXMLDocument.RemoveChild(N);
				}
				else
					if ((N is TElXMLDOMAttr) && (tvXML.SelectedNode.Parent != null)
					&& (tvXML.SelectedNode.Parent.Tag != null) &&
					((TElXMLDOMNode)tvXML.SelectedNode.Parent.Tag is TElXMLDOMElement))
					((TElXMLDOMElement)tvXML.SelectedNode.Parent.Tag).RemoveAttributeNode((TElXMLDOMAttr)N);
			}

			UpdateXML();
		}

		private void btnSaveXML_Click(object sender, System.EventArgs e)
		{
			FileStream F = null;
			try
			{
				F = new FileStream(edXMLFile.Text, FileMode.Create, FileAccess.ReadWrite);
				FXMLDocument.SaveToStream(F, SBXMLDefs.Unit.xcmNone, "");
			}
			catch (Exception E)
			{
				MessageBox.Show("Error: " + E.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (F != null)
				F.Close();		
		}

		private void LoadCertificate(FileStream F, string Password, TElXMLKeyInfoX509Data X509KeyData)
		{
			int CertFormat;
			X509KeyData.Certificate = new TElX509Certificate();
			try
			{
				CertFormat = TElX509Certificate.DetectCertFileFormat(F);
				F.Position = 0;

				switch (CertFormat)
				{
					case SBX509.Unit.cfDER: 
					{
						X509KeyData.Certificate.LoadFromStream(F, 0);
						break;
					}
					case SBX509.Unit.cfPEM: 
					{
						X509KeyData.Certificate.LoadFromStreamPEM(F, Password, 0);
						break;
					}
					case SBX509.Unit.cfPFX: 
					{
						X509KeyData.Certificate.LoadFromStreamPFX(F, Password, 0);
						break;
					}
					default:
					{
						X509KeyData.Certificate.Dispose();
						X509KeyData.Certificate = null;
						break;
					}
				}
			}
			catch
			{
				X509KeyData.Certificate.Dispose();
				X509KeyData.Certificate = null;
			}
		}

		private void btnEncrypt_Click(object sender, System.EventArgs e)
		{
			TElXMLEncryptor Encryptor;
			TElXMLKeyInfoSymmetricData SymKeyData, SymKEKData = null;
			TElXMLKeyInfoRSAData RSAKeyData = null;
			TElXMLKeyInfoX509Data X509KeyData = null;
			TElXMLKeyInfoPGPData PGPKeyData = null;
			FileStream F;
			TElXMLDOMNode Node, EncNode;
			byte[] Buf;
			int i;

			SymKeyData = null;
			SymKEKData = null;
			RSAKeyData = null;
			X509KeyData = null;
			PGPKeyData = null;
			frmEnc.LockOpt = false;
			if (frmEnc.ShowDialog() == DialogResult.OK)
			{
				Encryptor = new TElXMLEncryptor();
				try
				{
					Encryptor.EncryptKey = frmEnc.EncryptKey;
					Encryptor.EncryptionMethod = frmEnc.EncryptionMethod;
					Encryptor.KeyName = frmEnc.KeyName;
					Encryptor.EncryptedDataType = frmEnc.EncryptedDataType;
					if (Encryptor.EncryptKey)
					{
						Encryptor.KeyEncryptionType = frmEnc.KeyEncryptionType;
						Encryptor.KeyTransportMethod = frmEnc.KeyTransportMethod;
						Encryptor.KeyWrapMethod = frmEnc.KeyWrapMethod;

						SymKeyData = new TElXMLKeyInfoSymmetricData(true);
						// generate random Key & IV
						switch (Encryptor.EncryptionMethod)
						{
							case SBXMLSec.Unit.xem3DES:
							{
								SymKeyData.Key.Generate(SBDES.Unit.T3DESKeySize * 8);
								SymKeyData.Key.GenerateIV(8 * 8);
								break;
							}

							case SBXMLSec.Unit.xemAES:
							{
								SymKeyData.Key.Generate(32 * 8);
								SymKeyData.Key.GenerateIV(16 * 8);
								break;
							}

							case SBXMLSec.Unit.xemCamellia:
							{
								SymKeyData.Key.Generate(32 * 8); // 16, 24, 32
								SymKeyData.Key.GenerateIV(16 * 8);
								break;
							}

							case SBXMLSec.Unit.xemDES:
							{
								SymKeyData.Key.Generate(SBDES.Unit.TDESKeySize * 8);
								SymKeyData.Key.GenerateIV(8 * 8);
								break;
							}

							case SBXMLSec.Unit.xemRC4:
							{
								SymKeyData.Key.Generate(16 * 8); // 1..32
								break;
							}
						}

						Encryptor.KeyData = SymKeyData;

						if (Encryptor.KeyEncryptionType == SBXMLSec.Unit.xetKeyWrap)
						{
							SymKEKData = new TElXMLKeyInfoSymmetricData(true);

							try
							{
								F = new FileStream(frmEnc.KeyFile, FileMode.Open, FileAccess.Read);
							}
							catch (Exception E)
							{
								MessageBox.Show("Error: " + E.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
							}

							try
							{
								SymKEKData.Key.Load(F, 0);
							}
							finally
							{
								F.Close();
							}

							Encryptor.KeyEncryptionKeyData = SymKEKData;
						}
						else
						{ // xetKeyTransport
							RSAKeyData = new TElXMLKeyInfoRSAData(true);
							RSAKeyData.RSAKeyMaterial.Passphrase = frmEnc.Passphrase;
							X509KeyData = new TElXMLKeyInfoX509Data(true);
							PGPKeyData = new TElXMLKeyInfoPGPData(true);

							try
							{
								F = new FileStream(frmEnc.KeyFile, FileMode.Open, FileAccess.Read);
							}
							catch (Exception E)
							{
								MessageBox.Show("Error: " + E.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
							}

							try 
							{
								RSAKeyData.RSAKeyMaterial.LoadPublic(F, 0);
							}
							catch {}								 

							if (!RSAKeyData.RSAKeyMaterial.PublicKey)
							{
								F.Position = 0;
								try
								{
									RSAKeyData.RSAKeyMaterial.LoadSecret(F, 0);
								}
								catch {}
									 
							}

							if (!RSAKeyData.RSAKeyMaterial.PublicKey)
							{
								F.Position = 0;
								LoadCertificate(F, frmEnc.Passphrase, X509KeyData);
							}

							if (!RSAKeyData.RSAKeyMaterial.PublicKey &&
								(X509KeyData.Certificate == null))
							{
								F.Position = 0;
								PGPKeyData.PublicKey = new TElPGPPublicKey();
								try 
								{
									PGPKeyData.PublicKey.LoadFromStream(F);
								}
								catch
								{
									PGPKeyData.PublicKey.Dispose();
									PGPKeyData.PublicKey = null;
								}

								if (PGPKeyData.PublicKey == null)
								{
									F.Position = 0;
									PGPKeyData.SecretKey = new TElPGPSecretKey();
									PGPKeyData.SecretKey.Passphrase = frmEnc.Passphrase;
									try
									{
										PGPKeyData.SecretKey.LoadFromStream(F);
									}
									catch
									{
										PGPKeyData.SecretKey = null;
									}
								}
							}

							F.Close();

							if (RSAKeyData.RSAKeyMaterial.PublicKey)
								Encryptor.KeyEncryptionKeyData = RSAKeyData;
							else
								if (X509KeyData.Certificate != null)
								Encryptor.KeyEncryptionKeyData = X509KeyData;
																																																   
							else
								if ((PGPKeyData.PublicKey != null) ||
								(PGPKeyData.SecretKey != null))
								Encryptor.KeyEncryptionKeyData = PGPKeyData;																																					 
							else
							{
								MessageBox.Show("Key not loaded.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
							}
						}
					}
					else
					{
						SymKeyData = new TElXMLKeyInfoSymmetricData(true);

						try
						{
							F = new FileStream(frmEnc.KeyFile, FileMode.Open, FileAccess.Read);
						}
						catch (Exception E)
						{
							MessageBox.Show("Error: " + E.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}

						try
						{
							SymKeyData.Key.Load(F, 0);
						}
						finally
						{
							F.Close();
						}

						// generate random IV
						if ((Encryptor.EncryptionMethod == SBXMLSec.Unit.xem3DES) ||
							(Encryptor.EncryptionMethod == SBXMLSec.Unit.xemDES))
							SymKeyData.Key.GenerateIV(8 * 8);
						else
							SymKeyData.Key.GenerateIV(16 * 8);

						Encryptor.KeyData = SymKeyData;
					}

					if (Encryptor.EncryptedDataType == SBXMLSec.Unit.xedtExternal)
					{
						Encryptor.MimeType = frmEnc.MimeType;

						try
						{
							F = new FileStream(frmEnc.ExternalFile, FileMode.Open, FileAccess.Read);
						}
						catch (Exception E)
						{
							MessageBox.Show("Error: " + E.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}

						try
						{
							Buf = new byte[F.Length];
							if (F.Length > 0)
								F.Read(Buf, 0, Buf.Length);
						}
						finally 
						{
							F.Close();
						}

						Encryptor.Encrypt(Buf);

						FXMLDocument.Dispose();
						FXMLDocument = new TElXMLDOMDocument();

						try
						{
							EncNode = Encryptor.Save(FXMLDocument);
						}
						catch (Exception E)
						{
							MessageBox.Show(string.Format("Encrypted data saving failed. ({0})", E.Message), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}

						FXMLDocument.AppendChild(EncNode);
						UpdateXML();
					}
					else
					{
						if ((tvXML.SelectedNode == null) || (tvXML.SelectedNode.Tag == null))
						{
							MessageBox.Show("Please, select node for encryption.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
							return;
						}

						Node = (TElXMLDOMNode)tvXML.SelectedNode.Tag;

						Encryptor.Encrypt(Node);

						try
						{
							EncNode = Encryptor.Save(FXMLDocument);
						}
						catch (Exception E)
						{
							MessageBox.Show(string.Format("Encrypted data saving failed. ({0})", E.Message), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}

						if (Encryptor.EncryptedDataType == SBXMLSec.Unit.xedtElement)
							Node.ParentNode.ReplaceChild(EncNode, Node);
						else // xedtContent
						{
							if (Node is TElXMLDOMElement)
							{
								while (Node.LastChild != null)
								{
									Node.RemoveChild(Node.LastChild);
								}

								Node.AppendChild(EncNode);
							}
							else
								Node.ParentNode.ReplaceChild(EncNode, Node);
						}
						UpdateXML();
					}
				}
				finally
				{
					Encryptor.Dispose();
					if (X509KeyData != null)
						X509KeyData.Dispose();
					if (PGPKeyData != null)
						PGPKeyData.Dispose();
				}	
			}
		}

		private void btnDecrypt_Click(object sender, System.EventArgs e)
		{
			TElXMLDecryptor Decryptor;
			TElXMLKeyInfoSymmetricData SymKeyData, SymKEKData = null;
			TElXMLKeyInfoRSAData RSAKeyData = null;
			TElXMLKeyInfoX509Data X509KeyData = null;
			TElXMLKeyInfoPGPData PGPKeyData = null;
			FileStream F;
			TElXMLDOMNode Node, T;
			int i;
			if ((tvXML.SelectedNode != null) && (tvXML.SelectedNode.Tag != null))
				Node = (TElXMLDOMNode)tvXML.SelectedNode.Tag;
			else
				Node = (TElXMLDOMNode)FXMLDocument;

			while ((Node is TElXMLDOMElement) &&
				(((TElXMLDOMElement)Node).LocalName != "EncryptedData") &&
				(Node.ParentNode != null))
				Node = Node.ParentNode;

			if ((Node is TElXMLDOMElement) &&
				(((TElXMLDOMElement)Node).LocalName == "EncryptedData") &&
				(Node.ParentNode != null) &&
				(Node.ParentNode is TElXMLDOMDocument))
				Node = Node.ParentNode;

			if (Node is TElXMLDOMDocument)
				T = Node.FirstChild;
			else
				T = Node;

			if (!(T is TElXMLDOMElement) ||
				(((TElXMLDOMElement)T).LocalName != "EncryptedData"))
			{
				MessageBox.Show("Please, select EncryptedData element or Document for decryption.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			Decryptor = new TElXMLDecryptor();
			try
			{
				try
				{
					if (Node is TElXMLDOMDocument)
						Decryptor.Load((TElXMLDOMDocument)Node);
					else
						Decryptor.Load((TElXMLDOMElement)Node);
				}
				catch (Exception E)
				{
					MessageBox.Show(string.Format("Encrypted data loading failed. ({0})", E.Message), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				frmEnc.EncryptKey = Decryptor.EncryptKey;
				frmEnc.EncryptedDataType = Decryptor.EncryptedDataType;
				frmEnc.EncryptionMethod = Decryptor.EncryptionMethod;
				frmEnc.KeyEncryptionType = Decryptor.KeyEncryptionType;
				frmEnc.KeyTransportMethod = Decryptor.KeyTransportMethod;
				frmEnc.KeyWrapMethod = Decryptor.KeyWrapMethod;
				frmEnc.KeyName = Decryptor.KeyName;
				frmEnc.MimeType = Decryptor.MimeType;
				frmEnc.LockOpt = true;
				frmEnc.UpdateOpt();

				while (true)
				{
					if (frmEnc.ShowDialog() == DialogResult.OK)
					{
						if (Decryptor.EncryptKey)
						{
							if (Decryptor.KeyEncryptionType == SBXMLSec.Unit.xetKeyWrap)
							{
								SymKEKData = new TElXMLKeyInfoSymmetricData(true);

								try
								{
									F = new FileStream(frmEnc.KeyFile, FileMode.Open, FileAccess.Read);
								}
								catch (Exception E)
								{
									MessageBox.Show("Error: " + E.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
									return;
								}

								try
								{
									SymKEKData.Key.Load(F, 0);
								}
								finally
								{
									F.Close();
								}

								Decryptor.KeyEncryptionKeyData = SymKEKData;
							}
							else
							{
								if (X509KeyData != null)
									X509KeyData.Dispose();
								if (PGPKeyData != null)
									PGPKeyData.Dispose();
								RSAKeyData = new TElXMLKeyInfoRSAData(true);
								RSAKeyData.RSAKeyMaterial.Passphrase = frmEnc.Passphrase;
								X509KeyData = new TElXMLKeyInfoX509Data(true);
								PGPKeyData = new TElXMLKeyInfoPGPData(true);

								try
								{
									F = new FileStream(frmEnc.KeyFile, FileMode.Open, FileAccess.Read);
								}
								catch (Exception E)
								{
									MessageBox.Show("Error: " + E.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
									return;
								}

								try
								{
									RSAKeyData.RSAKeyMaterial.LoadSecret(F, 0);
								}
								catch {}
            
								if (!RSAKeyData.RSAKeyMaterial.SecretKey)
								{
									F.Position = 0;
									LoadCertificate(F, frmEnc.Passphrase, X509KeyData);
								}

								if (!RSAKeyData.RSAKeyMaterial.PublicKey && (X509KeyData.Certificate == null))
								{
									F.Position = 0;
									PGPKeyData.SecretKey = new TElPGPSecretKey();
									PGPKeyData.SecretKey.Passphrase = frmEnc.Passphrase;
									try
									{
										PGPKeyData.SecretKey.LoadFromStream(F);
									}
									catch
									{
										PGPKeyData.SecretKey = null;
									}
								}

								F.Close();

								if (RSAKeyData.RSAKeyMaterial.SecretKey)
									Decryptor.KeyEncryptionKeyData = RSAKeyData;
								else
									if (X509KeyData.Certificate != null)
									Decryptor.KeyEncryptionKeyData = X509KeyData;
																																											  
								else
									if (PGPKeyData.SecretKey != null)
									Decryptor.KeyEncryptionKeyData = PGPKeyData;
																							 
								else
								{
									MessageBox.Show("Key not loaded.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
									return;
								}
							}
						}
						else
						{
							SymKeyData = new TElXMLKeyInfoSymmetricData(true);

							try
							{
								F = new FileStream(frmEnc.KeyFile, FileMode.Open, FileAccess.Read);
							}
							catch (Exception E)
							{
								MessageBox.Show("Error: " + E.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
							}

							try
							{
								SymKeyData.Key.Load(F, 0);
							}
							finally
							{
								F.Close();
							}

							Decryptor.KeyData = SymKeyData;
						}

						i = Decryptor.Decrypt(FXMLDocument);
						if (i != SBXMLEnc.Unit.SB_XML_ENC_ERROR_OK)
						{
							if ((!Decryptor.EncryptKey && (i == SBXMLEnc.Unit.SB_XML_ENC_ERROR_INVALID_KEY)) ||
								(Decryptor.EncryptKey && (i == SBXMLEnc.Unit.SB_XML_ENC_ERROR_INVALID_KEK)))
								MessageBox.Show("Decryption failed. Bad key or data is corrupted.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
							else
							{
								MessageBox.Show(string.Format("Decryption failed. Error code: 0x{0:x}", i), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
							}

							continue;
						}

						if (Decryptor.EncryptedDataType == SBXMLSec.Unit.xedtExternal)
						{
							try
							{
								F = new FileStream(frmEnc.ExternalFile, FileMode.Create, FileAccess.ReadWrite);
							}
							catch (Exception E)
							{
								MessageBox.Show("Error: " + E.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
							}

							try
							{
								if (Decryptor.DecryptedData.Length > 0)
									F.Write(Decryptor.DecryptedData, 0, Decryptor.DecryptedData.Length);
							}
							finally
							{
								F.Close();
							}

							MessageBox.Show("Data saved successfully.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
						else
						{
							if (Decryptor.DecryptedNode != null)
							{
								if (Node is TElXMLDOMDocument)
									FXMLDocument.ReplaceChild(Decryptor.DecryptedNode, FXMLDocument.FirstChild);
								else
									Node.ParentNode.ReplaceChild(Decryptor.DecryptedNode, Node);

								Decryptor.DecryptedNode = null;
							}
							else
								if ((Decryptor.EncryptedDataType == SBXMLSec.Unit.xedtContent) &&
								(Decryptor.DecryptedNodeList != null))
							{
								T = Node.ParentNode;
								for (i = 0; i < Decryptor.DecryptedNodeList.Length; i++)
									T.InsertBefore(Decryptor.DecryptedNodeList.get_Item(i).CloneNode(true), Node);

								T.RemoveChild(Node);
							}

							UpdateXML();
						}

						break;
					}
					else
						break;
				}
			}
			finally
			{
				Decryptor.Dispose();
				if (X509KeyData != null)
					X509KeyData.Dispose();
				if (PGPKeyData != null)
					PGPKeyData.Dispose();
			}
		}
	}
}
