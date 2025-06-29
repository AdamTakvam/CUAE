using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SimpleSigner
{
	/// <summary>
	/// Summary description for SignForm.
	/// </summary>
	public class SignForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.GroupBox gbKeyInfo;
		private System.Windows.Forms.Button sbKeyFile;
		private System.Windows.Forms.TextBox edPassphrase;
		private System.Windows.Forms.Label lbPassphrase;
		private System.Windows.Forms.TextBox edKeyFile;
		private System.Windows.Forms.Label lbKeyFile;
		private System.Windows.Forms.TextBox edKeyName;
		private System.Windows.Forms.Label lbKeyName;
		private System.Windows.Forms.GroupBox gbGeneralEnc;
		private System.Windows.Forms.ComboBox cmbSignatureType;
		private System.Windows.Forms.Label lbCanonMethod;
		private System.Windows.Forms.Label lbSignatureType;
		private System.Windows.Forms.GroupBox rgSignatureMethodType;
		private System.Windows.Forms.RadioButton rbHMACMethod;
		private System.Windows.Forms.RadioButton rbSignatureMethod;
		private System.Windows.Forms.Label lbHMACMethod;
		private System.Windows.Forms.ComboBox cmbHMACMethod;
		private System.Windows.Forms.Label lbSignatureMethod;
		private System.Windows.Forms.ComboBox cmbSignatureMethod;
		private System.Windows.Forms.Button btnReferences;
		private System.Windows.Forms.OpenFileDialog dlgOpen;
		private System.Windows.Forms.ComboBox cmbCanonMethod;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public ReferencesForm frmReferences;

		public SignForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			frmReferences = new ReferencesForm();

			CanonicalizationMethod = SBXMLDefs.Unit.xcmCanon;
			SignatureType = SBXMLSec.Unit.xstEnveloped;
			SignatureMethodType = SBXMLSec.Unit.xmtSig;
			HMACMethod = SBXMLSec.Unit.xmmHMAC_SHA1;
			SigMethod = SBXMLSec.Unit.xsmRSA_SHA1;
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
					frmReferences.Dispose();
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
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.gbKeyInfo = new System.Windows.Forms.GroupBox();
			this.sbKeyFile = new System.Windows.Forms.Button();
			this.edPassphrase = new System.Windows.Forms.TextBox();
			this.lbPassphrase = new System.Windows.Forms.Label();
			this.edKeyFile = new System.Windows.Forms.TextBox();
			this.lbKeyFile = new System.Windows.Forms.Label();
			this.edKeyName = new System.Windows.Forms.TextBox();
			this.lbKeyName = new System.Windows.Forms.Label();
			this.gbGeneralEnc = new System.Windows.Forms.GroupBox();
			this.cmbSignatureMethod = new System.Windows.Forms.ComboBox();
			this.lbSignatureMethod = new System.Windows.Forms.Label();
			this.cmbHMACMethod = new System.Windows.Forms.ComboBox();
			this.lbHMACMethod = new System.Windows.Forms.Label();
			this.rgSignatureMethodType = new System.Windows.Forms.GroupBox();
			this.rbSignatureMethod = new System.Windows.Forms.RadioButton();
			this.rbHMACMethod = new System.Windows.Forms.RadioButton();
			this.cmbCanonMethod = new System.Windows.Forms.ComboBox();
			this.cmbSignatureType = new System.Windows.Forms.ComboBox();
			this.lbCanonMethod = new System.Windows.Forms.Label();
			this.lbSignatureType = new System.Windows.Forms.Label();
			this.btnReferences = new System.Windows.Forms.Button();
			this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
			this.gbKeyInfo.SuspendLayout();
			this.gbGeneralEnc.SuspendLayout();
			this.rgSignatureMethodType.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(200, 360);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(120, 360);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 6;
			this.btnOK.Text = "OK";
			// 
			// gbKeyInfo
			// 
			this.gbKeyInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.gbKeyInfo.Controls.Add(this.sbKeyFile);
			this.gbKeyInfo.Controls.Add(this.edPassphrase);
			this.gbKeyInfo.Controls.Add(this.lbPassphrase);
			this.gbKeyInfo.Controls.Add(this.edKeyFile);
			this.gbKeyInfo.Controls.Add(this.lbKeyFile);
			this.gbKeyInfo.Controls.Add(this.edKeyName);
			this.gbKeyInfo.Controls.Add(this.lbKeyName);
			this.gbKeyInfo.Location = new System.Drawing.Point(8, 240);
			this.gbKeyInfo.Name = "gbKeyInfo";
			this.gbKeyInfo.Size = new System.Drawing.Size(272, 112);
			this.gbKeyInfo.TabIndex = 5;
			this.gbKeyInfo.TabStop = false;
			this.gbKeyInfo.Text = "Key Info";
			// 
			// sbKeyFile
			// 
			this.sbKeyFile.Location = new System.Drawing.Point(238, 49);
			this.sbKeyFile.Name = "sbKeyFile";
			this.sbKeyFile.Size = new System.Drawing.Size(23, 22);
			this.sbKeyFile.TabIndex = 10;
			this.sbKeyFile.Text = "...";
			this.sbKeyFile.Click += new System.EventHandler(this.sbKeyFile_Click);
			// 
			// edPassphrase
			// 
			this.edPassphrase.Location = new System.Drawing.Point(80, 78);
			this.edPassphrase.Name = "edPassphrase";
			this.edPassphrase.Size = new System.Drawing.Size(180, 20);
			this.edPassphrase.TabIndex = 5;
			this.edPassphrase.Text = "";
			// 
			// lbPassphrase
			// 
			this.lbPassphrase.Location = new System.Drawing.Point(16, 82);
			this.lbPassphrase.Name = "lbPassphrase";
			this.lbPassphrase.Size = new System.Drawing.Size(64, 16);
			this.lbPassphrase.TabIndex = 4;
			this.lbPassphrase.Text = "Passphrase:";
			// 
			// edKeyFile
			// 
			this.edKeyFile.Location = new System.Drawing.Point(80, 49);
			this.edKeyFile.Name = "edKeyFile";
			this.edKeyFile.Size = new System.Drawing.Size(152, 20);
			this.edKeyFile.TabIndex = 3;
			this.edKeyFile.Text = "";
			// 
			// lbKeyFile
			// 
			this.lbKeyFile.Location = new System.Drawing.Point(16, 53);
			this.lbKeyFile.Name = "lbKeyFile";
			this.lbKeyFile.Size = new System.Drawing.Size(64, 16);
			this.lbKeyFile.TabIndex = 2;
			this.lbKeyFile.Text = "Key File:";
			// 
			// edKeyName
			// 
			this.edKeyName.Location = new System.Drawing.Point(80, 20);
			this.edKeyName.Name = "edKeyName";
			this.edKeyName.Size = new System.Drawing.Size(180, 20);
			this.edKeyName.TabIndex = 1;
			this.edKeyName.Text = "";
			// 
			// lbKeyName
			// 
			this.lbKeyName.Location = new System.Drawing.Point(16, 24);
			this.lbKeyName.Name = "lbKeyName";
			this.lbKeyName.Size = new System.Drawing.Size(64, 16);
			this.lbKeyName.TabIndex = 0;
			this.lbKeyName.Text = "Key Name:";
			// 
			// gbGeneralEnc
			// 
			this.gbGeneralEnc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.gbGeneralEnc.Controls.Add(this.cmbSignatureMethod);
			this.gbGeneralEnc.Controls.Add(this.lbSignatureMethod);
			this.gbGeneralEnc.Controls.Add(this.cmbHMACMethod);
			this.gbGeneralEnc.Controls.Add(this.lbHMACMethod);
			this.gbGeneralEnc.Controls.Add(this.rgSignatureMethodType);
			this.gbGeneralEnc.Controls.Add(this.cmbCanonMethod);
			this.gbGeneralEnc.Controls.Add(this.cmbSignatureType);
			this.gbGeneralEnc.Controls.Add(this.lbCanonMethod);
			this.gbGeneralEnc.Controls.Add(this.lbSignatureType);
			this.gbGeneralEnc.Location = new System.Drawing.Point(8, 8);
			this.gbGeneralEnc.Name = "gbGeneralEnc";
			this.gbGeneralEnc.Size = new System.Drawing.Size(272, 224);
			this.gbGeneralEnc.TabIndex = 8;
			this.gbGeneralEnc.TabStop = false;
			this.gbGeneralEnc.Text = "General";
			// 
			// cmbSignatureMethod
			// 
			this.cmbSignatureMethod.Items.AddRange(new object[] {
																	"DSS",
																	"RSA SHA1",
																	"RSA MD5",
																	"RSA SHA256",
																	"RSA SHA384",
																	"RSA SHA512",
																	"RSA RIPEMD160"});
			this.cmbSignatureMethod.Location = new System.Drawing.Point(136, 188);
			this.cmbSignatureMethod.Name = "cmbSignatureMethod";
			this.cmbSignatureMethod.Size = new System.Drawing.Size(125, 21);
			this.cmbSignatureMethod.TabIndex = 9;
			// 
			// lbSignatureMethod
			// 
			this.lbSignatureMethod.Location = new System.Drawing.Point(16, 192);
			this.lbSignatureMethod.Name = "lbSignatureMethod";
			this.lbSignatureMethod.Size = new System.Drawing.Size(100, 16);
			this.lbSignatureMethod.TabIndex = 8;
			this.lbSignatureMethod.Text = "Signature Method:";
			// 
			// cmbHMACMethod
			// 
			this.cmbHMACMethod.Items.AddRange(new object[] {
															   "MD5",
															   "SHA1",
															   "SHA224",
															   "SHA256",
															   "SHA384",
															   "SHA512",
															   "RIPEMD160"});
			this.cmbHMACMethod.Location = new System.Drawing.Point(136, 159);
			this.cmbHMACMethod.Name = "cmbHMACMethod";
			this.cmbHMACMethod.Size = new System.Drawing.Size(125, 21);
			this.cmbHMACMethod.TabIndex = 7;
			// 
			// lbHMACMethod
			// 
			this.lbHMACMethod.Location = new System.Drawing.Point(16, 163);
			this.lbHMACMethod.Name = "lbHMACMethod";
			this.lbHMACMethod.Size = new System.Drawing.Size(100, 16);
			this.lbHMACMethod.TabIndex = 6;
			this.lbHMACMethod.Text = "HMAC Method:";
			// 
			// rgSignatureMethodType
			// 
			this.rgSignatureMethodType.Controls.Add(this.rbSignatureMethod);
			this.rgSignatureMethodType.Controls.Add(this.rbHMACMethod);
			this.rgSignatureMethodType.Location = new System.Drawing.Point(12, 81);
			this.rgSignatureMethodType.Name = "rgSignatureMethodType";
			this.rgSignatureMethodType.Size = new System.Drawing.Size(251, 68);
			this.rgSignatureMethodType.TabIndex = 5;
			this.rgSignatureMethodType.TabStop = false;
			this.rgSignatureMethodType.Text = "Signature method type:";
			// 
			// rbSignatureMethod
			// 
			this.rbSignatureMethod.Location = new System.Drawing.Point(16, 40);
			this.rbSignatureMethod.Name = "rbSignatureMethod";
			this.rbSignatureMethod.Size = new System.Drawing.Size(160, 24);
			this.rbSignatureMethod.TabIndex = 1;
			this.rbSignatureMethod.Text = "Signature method";
			this.rbSignatureMethod.CheckedChanged += new System.EventHandler(this.rbSignatureMethod_CheckedChanged);
			// 
			// rbHMACMethod
			// 
			this.rbHMACMethod.Location = new System.Drawing.Point(16, 16);
			this.rbHMACMethod.Name = "rbHMACMethod";
			this.rbHMACMethod.Size = new System.Drawing.Size(152, 24);
			this.rbHMACMethod.TabIndex = 0;
			this.rbHMACMethod.Text = "HMAC method";
			this.rbHMACMethod.CheckedChanged += new System.EventHandler(this.rbHMACMethod_CheckedChanged);
			// 
			// cmbCanonMethod
			// 
			this.cmbCanonMethod.Items.AddRange(new object[] {
																"Canonical",
																"Canonical +comments",
																"Minimal"});
			this.cmbCanonMethod.Location = new System.Drawing.Point(136, 48);
			this.cmbCanonMethod.Name = "cmbCanonMethod";
			this.cmbCanonMethod.Size = new System.Drawing.Size(125, 21);
			this.cmbCanonMethod.TabIndex = 4;
			// 
			// cmbSignatureType
			// 
			this.cmbSignatureType.Items.AddRange(new object[] {
																  "Enveloped",
																  "Enveloping",
																  "Detached"});
			this.cmbSignatureType.Location = new System.Drawing.Point(136, 16);
			this.cmbSignatureType.Name = "cmbSignatureType";
			this.cmbSignatureType.Size = new System.Drawing.Size(125, 21);
			this.cmbSignatureType.TabIndex = 3;
			// 
			// lbCanonMethod
			// 
			this.lbCanonMethod.Location = new System.Drawing.Point(16, 46);
			this.lbCanonMethod.Name = "lbCanonMethod";
			this.lbCanonMethod.Size = new System.Drawing.Size(100, 32);
			this.lbCanonMethod.TabIndex = 2;
			this.lbCanonMethod.Text = "Canonicalization method";
			// 
			// lbSignatureType
			// 
			this.lbSignatureType.Location = new System.Drawing.Point(16, 24);
			this.lbSignatureType.Name = "lbSignatureType";
			this.lbSignatureType.Size = new System.Drawing.Size(112, 16);
			this.lbSignatureType.TabIndex = 1;
			this.lbSignatureType.Text = "Signature Type:";
			// 
			// btnReferences
			// 
			this.btnReferences.Location = new System.Drawing.Point(16, 360);
			this.btnReferences.Name = "btnReferences";
			this.btnReferences.TabIndex = 9;
			this.btnReferences.Text = "References";
			this.btnReferences.Click += new System.EventHandler(this.btnReferences_Click);
			// 
			// SignForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(286, 392);
			this.Controls.Add(this.btnReferences);
			this.Controls.Add(this.gbGeneralEnc);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.gbKeyInfo);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "SignForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Signature Options";
			this.VisibleChanged += new System.EventHandler(this.SignForm_VisibleChanged);
			this.gbKeyInfo.ResumeLayout(false);
			this.gbGeneralEnc.ResumeLayout(false);
			this.rgSignatureMethodType.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void sbKeyFile_Click(object sender, System.EventArgs e)
		{
			dlgOpen.FileName = edKeyFile.Text;
			if (dlgOpen.ShowDialog() == DialogResult.OK)
				edKeyFile.Text = dlgOpen.FileName;
		}

		private void btnReferences_Click(object sender, System.EventArgs e)
		{
			frmReferences.ShowDialog();
		}

		private void SignForm_VisibleChanged(object sender, System.EventArgs e)
		{
			UpdateOpt();
		}

		public short CanonicalizationMethod
		{
			get
			{
				switch (cmbCanonMethod.SelectedIndex)
				{
					case 0: return SBXMLDefs.Unit.xcmCanon;
					case 1: return SBXMLDefs.Unit.xcmCanonComment;
					case 2: return SBXMLDefs.Unit.xcmMinCanon;
					default: return SBXMLDefs.Unit.xcmCanon;
				}
			}
			set
			{
				switch (value)
				{
					case SBXMLDefs.Unit.xcmCanon: 
					{
						cmbCanonMethod.SelectedIndex = 0;
						break;
					}
					case SBXMLDefs.Unit.xcmCanonComment: 
					{
						cmbCanonMethod.SelectedIndex = 1;
						break;
					}
					case SBXMLDefs.Unit.xcmMinCanon: 
					{
						cmbCanonMethod.SelectedIndex = 2;
						break;
					}
					default:
					{
						cmbCanonMethod.SelectedIndex = 0;
						break;
					}
				}
			}
		}

		public short SignatureType
		{
			get
			{
				switch (cmbSignatureType.SelectedIndex)
				{
					case 1: return SBXMLSec.Unit.xstEnveloping;
					case 2: return SBXMLSec.Unit.xstDetached;
					default: return SBXMLSec.Unit.xstEnveloped;
				}
			}
			set
			{
				switch (value)
				{
					case SBXMLSec.Unit.xstDetached: 
					{
						cmbSignatureType.SelectedIndex = 2;
						break;
					}
					case SBXMLSec.Unit.xstEnveloping: 
					{
						cmbSignatureType.SelectedIndex = 1;
						break;
					}
					case SBXMLSec.Unit.xstEnveloped: 
					{
						cmbSignatureType.SelectedIndex = 0;
						break;
					}
				}
			}
		}

		public short SignatureMethodType
		{
			get
			{
				if (rbHMACMethod.Checked)
					return SBXMLSec.Unit.xmtMAC;
				else
					return SBXMLSec.Unit.xmtSig;

			}
			set
			{
				if (value == SBXMLSec.Unit.xmtMAC)
					rbHMACMethod.Checked = true;
				else
					rbSignatureMethod.Checked = true;
			}
		}

		public short SigMethod
		{
			get 
			{
				switch (cmbSignatureMethod.SelectedIndex)
				{
					case 0: return SBXMLSec.Unit.xsmDSS;
					case 1: return SBXMLSec.Unit.xsmRSA_SHA1;
					case 2: return SBXMLSec.Unit.xsmRSA_MD5;
					case 3: return SBXMLSec.Unit.xsmRSA_SHA256;
					case 4: return SBXMLSec.Unit.xsmRSA_SHA384;
					case 5: return SBXMLSec.Unit.xsmRSA_SHA512;
					case 6: return SBXMLSec.Unit.xsmRSA_RIPEMD160;
					default: return SBXMLSec.Unit.xsmRSA_SHA1;
				}
			}
			set
			{
				switch (value)
				{
					case SBXMLSec.Unit.xsmDSS: 
					{
						cmbSignatureMethod.SelectedIndex = 0;
						break;
					}
					case SBXMLSec.Unit.xsmRSA_SHA1: 
					{
						cmbSignatureMethod.SelectedIndex = 1;
						break;
					}
					case SBXMLSec.Unit.xsmRSA_MD5: 
					{
						cmbSignatureMethod.SelectedIndex = 2;
						break;
					}
					case SBXMLSec.Unit.xsmRSA_SHA256: 
					{
						cmbSignatureMethod.SelectedIndex = 3;
						break;
					}
					case SBXMLSec.Unit.xsmRSA_SHA384: 
					{
						cmbSignatureMethod.SelectedIndex = 4;
						break;
					}
					case SBXMLSec.Unit.xsmRSA_SHA512: 
					{
						cmbSignatureMethod.SelectedIndex = 5;
						break;
					}
					case SBXMLSec.Unit.xsmRSA_RIPEMD160: 
					{
						cmbSignatureMethod.SelectedIndex = 6;
						break;
					}
				}
			}
		}

		public short HMACMethod 
		{
			get 
			{
				switch (cmbHMACMethod.SelectedIndex)
				{
					case 0: return SBXMLSec.Unit.xmmHMAC_MD5;
					case 1: return SBXMLSec.Unit.xmmHMAC_SHA1;
					case 2: return SBXMLSec.Unit.xmmHMAC_SHA224;
					case 3: return SBXMLSec.Unit.xmmHMAC_SHA256;
					case 4: return SBXMLSec.Unit.xmmHMAC_SHA384;
					case 5: return SBXMLSec.Unit.xmmHMAC_SHA512;
					case 6: return SBXMLSec.Unit.xmmHMAC_RIPEMD160;
					default: return SBXMLSec.Unit.xmmHMAC_SHA1;
				}
			}
			set
			{
				switch (value)
				{
					case SBXMLSec.Unit.xmmHMAC_MD5: 
					{
						cmbHMACMethod.SelectedIndex = 0;
						break;
					}
					case SBXMLSec.Unit.xmmHMAC_SHA1: 
					{
						cmbHMACMethod.SelectedIndex = 1;
						break;
					}
					case SBXMLSec.Unit.xmmHMAC_SHA224: 
					{
						cmbHMACMethod.SelectedIndex = 2;
						break;
					}
					case SBXMLSec.Unit.xmmHMAC_SHA256: 
					{
						cmbHMACMethod.SelectedIndex = 3;
						break;
					}
					case SBXMLSec.Unit.xmmHMAC_SHA384: 
					{
						cmbHMACMethod.SelectedIndex = 4;
						break;
					}
					case SBXMLSec.Unit.xmmHMAC_SHA512: 
					{
						cmbHMACMethod.SelectedIndex = 5;
						break;
					}
					case SBXMLSec.Unit.xmmHMAC_RIPEMD160: 
					{
						cmbHMACMethod.SelectedIndex = 6;
						break;
					}
				}
			}
		}

		public string KeyName
		{
			get
			{
				return edKeyName.Text;
			}
			set
			{
				edKeyName.Text = value;
			}
		}

		public string KeyFile
		{
			get
			{
				return edKeyFile.Text;
			}
			set
			{
				edKeyFile.Text = value;
			}
		}

		public string Passphrase
		{
			get
			{
				return edPassphrase.Text;
			}
			set
			{
				edPassphrase.Text = value;
			}
		}

		public void UpdateOpt()
		{
			cmbHMACMethod.Enabled = rbHMACMethod.Checked;
			lbHMACMethod.Enabled = cmbHMACMethod.Enabled;
			cmbSignatureMethod.Enabled = rbSignatureMethod.Checked;
			lbSignatureMethod.Enabled = cmbSignatureMethod.Enabled;
			edPassphrase.Enabled = cmbSignatureMethod.Enabled;
			lbPassphrase.Enabled = edPassphrase.Enabled;
		}

		private void rbHMACMethod_CheckedChanged(object sender, System.EventArgs e)
		{
			UpdateOpt();
		}

		private void rbSignatureMethod_CheckedChanged(object sender, System.EventArgs e)
		{
			UpdateOpt();
		}
	}
}
