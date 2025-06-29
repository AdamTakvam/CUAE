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
using SBXMLSig;
using SBXMLTransform;

using SBPGPKeys;
using SBX509;

namespace SimpleSigner
{
	/// <summary>
	/// Summary description for frmMain.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lbXMLFile;
		private System.Windows.Forms.TextBox edXMLFile;
		private System.Windows.Forms.Button sbBrowseXMLFile;
		private System.Windows.Forms.TreeView tvXML;
		private System.Windows.Forms.Button btnLoadXML;
		private System.Windows.Forms.Button btnSaveXML;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnClear;
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
		private System.Windows.Forms.Button btnSign;
		private System.Windows.Forms.Button btnVerify;
		private System.Windows.Forms.Button btnRemove;

		private TElXMLDOMDocument FXMLDocument = null;
		private SignForm frmSign = null;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			frmSign = new SignForm();

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
					frmSign.Dispose();
					FXMLDocument.Dispose();
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
			this.btnSign = new System.Windows.Forms.Button();
			this.btnVerify = new System.Windows.Forms.Button();
			this.lbNodeType = new System.Windows.Forms.Label();
			this.lbNamespaceURI = new System.Windows.Forms.Label();
			this.mmXML = new System.Windows.Forms.TextBox();
			this.dlgOpenXML = new System.Windows.Forms.OpenFileDialog();
			this.dlbNodeType = new System.Windows.Forms.Label();
			this.dlbNamespaceURI = new System.Windows.Forms.Label();
			this.btnRemove = new System.Windows.Forms.Button();
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
			// btnSign
			// 
			this.btnSign.Location = new System.Drawing.Point(350, 300);
			this.btnSign.Name = "btnSign";
			this.btnSign.TabIndex = 8;
			this.btnSign.Text = "Sign";
			this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
			// 
			// btnVerify
			// 
			this.btnVerify.Location = new System.Drawing.Point(350, 332);
			this.btnVerify.Name = "btnVerify";
			this.btnVerify.TabIndex = 9;
			this.btnVerify.Text = "Verify";
			this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
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
			// btnRemove
			// 
			this.btnRemove.Location = new System.Drawing.Point(350, 368);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.TabIndex = 15;
			this.btnRemove.Text = "Remove";
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(432, 566);
			this.Controls.Add(this.btnRemove);
			this.Controls.Add(this.dlbNamespaceURI);
			this.Controls.Add(this.dlbNodeType);
			this.Controls.Add(this.mmXML);
			this.Controls.Add(this.edXMLFile);
			this.Controls.Add(this.lbNamespaceURI);
			this.Controls.Add(this.lbNodeType);
			this.Controls.Add(this.btnVerify);
			this.Controls.Add(this.btnSign);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnSaveXML);
			this.Controls.Add(this.btnLoadXML);
			this.Controls.Add(this.tvXML);
			this.Controls.Add(this.sbBrowseXMLFile);
			this.Controls.Add(this.lbXMLFile);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "MainForm";
			this.Text = "Simple Signer";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
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

		private void btnSign_Click(object sender, System.EventArgs e)
		{
			TElXMLSigner Signer;
			TElXMLKeyInfoHMACData HMACKeyData = null;
			TElXMLKeyInfoRSAData RSAKeyData = null;
			TElXMLKeyInfoX509Data X509KeyData = null;
			TElXMLKeyInfoPGPData PGPKeyData = null;
			FileStream F;
			TElXMLDOMNode SigNode;
			byte[] Buf;
			TElXMLReference Ref;
			TElXMLReferenceList Refs;
			int i;

			Refs = new TElXMLReferenceList();
			if ((tvXML.SelectedNode != null) && (tvXML.SelectedNode.Tag != null))
			{
				Ref = new TElXMLReference();
				Ref.DigestMethod = SBXMLSec.Unit.xdmSHA1;
				Ref.URINode = (TElXMLDOMNode)tvXML.SelectedNode.Tag;
				Ref.URI = ((TElXMLDOMNode)tvXML.SelectedNode.Tag).LocalName;
				Ref.TransformChain.Add(new TElXMLEnvelopedSignatureTransform());
				Refs.Add(Ref);
			}

			frmSign.frmReferences.References = Refs;
			frmSign.frmReferences.Verify = false;

			if (frmSign.ShowDialog() == DialogResult.OK)
			{
				Signer = new TElXMLSigner();
				try
				{
					Signer.SignatureType = frmSign.SignatureType;
					Signer.CanonicalizationMethod = frmSign.CanonicalizationMethod;
					Signer.SignatureMethodType = frmSign.SignatureMethodType;
					Signer.SignatureMethod = frmSign.SigMethod;
					Signer.MACMethod = frmSign.HMACMethod;
					Signer.References = Refs;
					Signer.KeyName = frmSign.KeyName;

					if (Signer.SignatureMethodType == SBXMLSec.Unit.xmtMAC)
					{
						HMACKeyData = new TElXMLKeyInfoHMACData(true);

						try
						{
							F = new FileStream(frmSign.KeyFile, FileMode.Open, FileAccess.Read);
						}
						catch (Exception E)
						{
							MessageBox.Show("Error: " + E.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}

						Buf = new byte[F.Length];
						if (F.Length > 0)
							F.Read(Buf, 0, (int)F.Length);

						F.Close();
						HMACKeyData.Key.Key = Buf;
						Signer.KeyData = HMACKeyData;
					}
					else
					{
						RSAKeyData = new TElXMLKeyInfoRSAData(true);
						RSAKeyData.RSAKeyMaterial.Passphrase = frmSign.Passphrase;
						X509KeyData = new TElXMLKeyInfoX509Data(true);
						PGPKeyData = new TElXMLKeyInfoPGPData(true);

						try
						{
							F = new FileStream(frmSign.KeyFile, FileMode.Open, FileAccess.Read);
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
							LoadCertificate(F, frmSign.Passphrase, X509KeyData);
						}

						if (!RSAKeyData.RSAKeyMaterial.PublicKey &&
							(X509KeyData.Certificate == null))
						{
							F.Position = 0;
							PGPKeyData.SecretKey = new TElPGPSecretKey();
							PGPKeyData.SecretKey.Passphrase = frmSign.Passphrase;
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
							Signer.KeyData = RSAKeyData;
						else
							if (X509KeyData.Certificate != null)
							Signer.KeyData = X509KeyData;
						else
							if (PGPKeyData.SecretKey != null)
							Signer.KeyData = PGPKeyData;
						else
						{
							MessageBox.Show("Key not loaded.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
					}

					Signer.UpdateReferencesDigest();

					if (Signer.SignatureType == SBXMLSec.Unit.xstDetached)
					{
						Signer.Sign();

						FXMLDocument.Dispose();
						try
						{
							SigNode = null;
							Signer.Save(ref SigNode);
							FXMLDocument = SigNode.OwnerDocument;
						}
						catch (Exception E)
						{
							FXMLDocument = new TElXMLDOMDocument();
							MessageBox.Show(string.Format("Signed data saving failed. ({0})", E.Message), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
					}
					else
					{
						if ((tvXML.SelectedNode == null) ||
							(tvXML.SelectedNode.Tag == null))
						{
							MessageBox.Show("Please, select node for signing.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
							return;
						}

						Signer.Sign();

						SigNode = (TElXMLDOMNode)tvXML.SelectedNode.Tag;
						try
						{
							Signer.Save(ref SigNode);
						}
						catch (Exception E)
						{
							MessageBox.Show(string.Format("Signed data saving failed. ({0})", E.Message), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
					}

					UpdateXML();
				}
				finally
				{
					Signer.Dispose();
					if (X509KeyData != null)
						X509KeyData.Dispose();
					if (PGPKeyData != null)
						PGPKeyData.Dispose();
				}
			}
		}

		private void btnVerify_Click(object sender, System.EventArgs e)
		{
			TElXMLVerifier Verifier;
			TElXMLKeyInfoHMACData HMACKeyData = null;
			TElXMLKeyInfoRSAData RSAKeyData = null;
			TElXMLKeyInfoX509Data X509KeyData = null;
			TElXMLKeyInfoPGPData PGPKeyData = null;
			FileStream F;
			byte[] Buf;
			TElXMLDOMNode Node, T;

			if ((tvXML.SelectedNode != null) && (tvXML.SelectedNode.Tag != null))
				Node = (TElXMLDOMNode)tvXML.SelectedNode.Tag;
			else
				Node = (TElXMLDOMNode)FXMLDocument;

			while ((Node is TElXMLDOMElement) &&
				(((TElXMLDOMElement)Node).LocalName != "Signature") &&
				(Node.ParentNode != null))
				Node = Node.ParentNode;

			if ((Node is TElXMLDOMElement) &&
				(((TElXMLDOMElement)Node).LocalName == "Signature") &&
				(Node.ParentNode != null) &&
				(Node.ParentNode is TElXMLDOMDocument))
				Node = Node.ParentNode;

			if (Node is TElXMLDOMDocument)
				T = Node.FirstChild;
			else
				T = Node;

			if (!(T is TElXMLDOMElement) ||
				((((TElXMLDOMElement)T).LocalName != "Signature") &&
				(SBXMLSec.Unit.FindChildElementSig((TElXMLDOMElement)T, "Signature") == null)))
			{
				MessageBox.Show("Please, select Signature element for verifying.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			Verifier = new TElXMLVerifier();
			try
			{
				try
				{
					Verifier.Load((TElXMLDOMElement)T);
				}
				catch (Exception E)
				{
					MessageBox.Show(string.Format("Signature data loading failed. ({0})", E.Message), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				frmSign.frmReferences.References = Verifier.References;
				frmSign.frmReferences.Verify = true;

				if (!Verifier.ValidateSignature())
				{
					// KeyInfo doesn't contain correct public key for the signature
					// or data corrupted

					frmSign.CanonicalizationMethod = Verifier.CanonicalizationMethod;
					frmSign.SignatureType = Verifier.SignatureType;
					frmSign.SignatureMethodType = Verifier.SignatureMethodType;
					frmSign.SigMethod = Verifier.SignatureMethod;
					frmSign.HMACMethod = Verifier.MACMethod;
					frmSign.KeyName = Verifier.KeyName;

					if (frmSign.ShowDialog() == DialogResult.OK)
					{
						frmSign.SignatureType = Verifier.SignatureType;

						if (Verifier.SignatureMethodType == SBXMLSec.Unit.xmtMAC)
						{
							HMACKeyData = new TElXMLKeyInfoHMACData(true);

							try
							{
								F = new FileStream(frmSign.KeyFile, FileMode.Open, FileAccess.Read);
							}
							catch (Exception E)
							{
								MessageBox.Show("Error: " + E.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
							}

							Buf = new byte[F.Length];
							if (F.Length > 0)
								F.Read(Buf, 0, (int)F.Length);

							F.Close();
							HMACKeyData.Key.Key = Buf;
							Verifier.HMACKey = HMACKeyData;
						}
						else
						{
							RSAKeyData = new TElXMLKeyInfoRSAData(true);
							RSAKeyData.RSAKeyMaterial.Passphrase = frmSign.Passphrase;
							X509KeyData = new TElXMLKeyInfoX509Data(true);
							PGPKeyData = new TElXMLKeyInfoPGPData(true);

							try
							{
								F = new FileStream(frmSign.KeyFile, FileMode.Open, FileAccess.Read);
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
								LoadCertificate(F, frmSign.Passphrase, X509KeyData);
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
									PGPKeyData.SecretKey.Passphrase = frmSign.Passphrase;
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
								Verifier.KeyData = RSAKeyData;
							else
								if (X509KeyData.Certificate != null)
								Verifier.KeyData = X509KeyData;
							else
								if ((PGPKeyData.PublicKey != null) || (PGPKeyData.SecretKey != null))
								Verifier.KeyData = PGPKeyData;
							else
							{
								MessageBox.Show("Key not loaded.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
							}
						}

						if (Verifier.ValidateSignature())
							MessageBox.Show("Signature validated successfully.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
						else
							MessageBox.Show("Signature is invalid", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
				else
				{
					if (MessageBox.Show("Signature validated successfully.\r\nDo you want to validate references?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
						frmSign.frmReferences.ShowDialog();
				}				
			}
			finally
			{
				Verifier.Dispose();
				if (X509KeyData != null)
					X509KeyData.Dispose();
				if (PGPKeyData != null)
					PGPKeyData.Dispose();
			}
		}

		private void btnRemove_Click(object sender, System.EventArgs e)
		{
			TElXMLVerifier Verifier;
			TElXMLDOMNode Node, T;
			if ((tvXML.SelectedNode != null) && (tvXML.SelectedNode.Tag != null))
				Node = (TElXMLDOMNode)tvXML.SelectedNode.Tag;
			else
				Node = (TElXMLDOMNode)FXMLDocument;

			while ((Node is TElXMLDOMElement) &&
				(((TElXMLDOMElement)Node).LocalName != "Signature") &&
				(Node.ParentNode != null))
				Node = Node.ParentNode;

			if ((Node is TElXMLDOMElement) &&
				(((TElXMLDOMElement)Node).LocalName == "Signature") &&
				(Node.ParentNode != null) &&
				(Node.ParentNode is TElXMLDOMDocument))
				Node = Node.ParentNode;

			if (Node is TElXMLDOMDocument)
				T = Node.FirstChild;
			else
				T = Node;

			if (!(T is TElXMLDOMElement) ||
				((((TElXMLDOMElement)T).LocalName != "Signature") &&
				(SBXMLSec.Unit.FindChildElementSig((TElXMLDOMElement)T, "Signature")) == null))
			{
				MessageBox.Show("Please, select Signature element for verifying.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			Verifier = new TElXMLVerifier();
			try
			{
				try
				{
					Verifier.Load((TElXMLDOMElement)T);
				}
				catch (Exception E)
				{
					MessageBox.Show(string.Format("Signature data loading failed. ({0})", E.Message), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				Verifier.SignatureType = SBXMLSec.Unit.xstEnveloped;
				T.ParentNode.ReplaceChild(Verifier.RemoveSignature(), T);
			}
			finally
			{
				Verifier.Dispose();
			}

			UpdateXML();
		}

	}
}
