using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SBXMLSec;

namespace SimpleEncryptor
{
	/// <summary>
	/// Summary description for EncForm.
	/// </summary>
	public class EncForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public EncForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			EncryptedDataType = SBXMLSec.Unit.xedtElement;
			EncryptionMethod = SBXMLSec.Unit.xem3DES;
			KeyEncryptionType = SBXMLSec.Unit.xetKeyWrap;
			KeyTransportMethod = SBXMLSec.Unit.xktRSA15;
			KeyWrapMethod = SBXMLSec.Unit.xwm3DES;
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
			this.gbGeneralEnc = new System.Windows.Forms.GroupBox();
			this.sbExternalFile = new System.Windows.Forms.Button();
			this.edExternalFile = new System.Windows.Forms.TextBox();
			this.lbExternalFile = new System.Windows.Forms.Label();
			this.edMimeType = new System.Windows.Forms.TextBox();
			this.lbMimeType = new System.Windows.Forms.Label();
			this.cmbEncryptionMethod = new System.Windows.Forms.ComboBox();
			this.cmbEncryptedDataType = new System.Windows.Forms.ComboBox();
			this.lbEncryptionMethod = new System.Windows.Forms.Label();
			this.lbEnryptedDataType = new System.Windows.Forms.Label();
			this.cbEncryptKey = new System.Windows.Forms.CheckBox();
			this.gbKEK = new System.Windows.Forms.GroupBox();
			this.cmbKeyWrap = new System.Windows.Forms.ComboBox();
			this.lbKeyWrap = new System.Windows.Forms.Label();
			this.cmbKeyTransport = new System.Windows.Forms.ComboBox();
			this.lbKeyTransport = new System.Windows.Forms.Label();
			this.rgKEK = new System.Windows.Forms.GroupBox();
			this.rbKeyWrap = new System.Windows.Forms.RadioButton();
			this.rbKeyTransport = new System.Windows.Forms.RadioButton();
			this.gbKeyInfo = new System.Windows.Forms.GroupBox();
			this.sbKeyFile = new System.Windows.Forms.Button();
			this.edPassphrase = new System.Windows.Forms.TextBox();
			this.lbPassphrase = new System.Windows.Forms.Label();
			this.edKeyFile = new System.Windows.Forms.TextBox();
			this.lbKeyFile = new System.Windows.Forms.Label();
			this.edKeyName = new System.Windows.Forms.TextBox();
			this.lbKeyName = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
			this.dlgSave = new System.Windows.Forms.SaveFileDialog();
			this.gbGeneralEnc.SuspendLayout();
			this.gbKEK.SuspendLayout();
			this.rgKEK.SuspendLayout();
			this.gbKeyInfo.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbGeneralEnc
			// 
			this.gbGeneralEnc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.gbGeneralEnc.Controls.Add(this.sbExternalFile);
			this.gbGeneralEnc.Controls.Add(this.edExternalFile);
			this.gbGeneralEnc.Controls.Add(this.lbExternalFile);
			this.gbGeneralEnc.Controls.Add(this.edMimeType);
			this.gbGeneralEnc.Controls.Add(this.lbMimeType);
			this.gbGeneralEnc.Controls.Add(this.cmbEncryptionMethod);
			this.gbGeneralEnc.Controls.Add(this.cmbEncryptedDataType);
			this.gbGeneralEnc.Controls.Add(this.lbEncryptionMethod);
			this.gbGeneralEnc.Controls.Add(this.lbEnryptedDataType);
			this.gbGeneralEnc.Controls.Add(this.cbEncryptKey);
			this.gbGeneralEnc.Location = new System.Drawing.Point(8, 8);
			this.gbGeneralEnc.Name = "gbGeneralEnc";
			this.gbGeneralEnc.Size = new System.Drawing.Size(272, 160);
			this.gbGeneralEnc.TabIndex = 0;
			this.gbGeneralEnc.TabStop = false;
			this.gbGeneralEnc.Text = "General";
			// 
			// sbExternalFile
			// 
			this.sbExternalFile.Location = new System.Drawing.Point(238, 129);
			this.sbExternalFile.Name = "sbExternalFile";
			this.sbExternalFile.Size = new System.Drawing.Size(23, 22);
			this.sbExternalFile.TabIndex = 9;
			this.sbExternalFile.Text = "...";
			this.sbExternalFile.Click += new System.EventHandler(this.sbExternalFile_Click);
			// 
			// edExternalFile
			// 
			this.edExternalFile.Location = new System.Drawing.Point(88, 129);
			this.edExternalFile.Name = "edExternalFile";
			this.edExternalFile.Size = new System.Drawing.Size(144, 20);
			this.edExternalFile.TabIndex = 8;
			this.edExternalFile.Text = "";
			// 
			// lbExternalFile
			// 
			this.lbExternalFile.Location = new System.Drawing.Point(16, 133);
			this.lbExternalFile.Name = "lbExternalFile";
			this.lbExternalFile.Size = new System.Drawing.Size(70, 16);
			this.lbExternalFile.TabIndex = 7;
			this.lbExternalFile.Text = "External file:";
			// 
			// edMimeType
			// 
			this.edMimeType.Location = new System.Drawing.Point(136, 102);
			this.edMimeType.Name = "edMimeType";
			this.edMimeType.Size = new System.Drawing.Size(125, 20);
			this.edMimeType.TabIndex = 6;
			this.edMimeType.Text = "";
			// 
			// lbMimeType
			// 
			this.lbMimeType.Location = new System.Drawing.Point(16, 106);
			this.lbMimeType.Name = "lbMimeType";
			this.lbMimeType.Size = new System.Drawing.Size(100, 16);
			this.lbMimeType.TabIndex = 5;
			this.lbMimeType.Text = "Mime Type:";
			// 
			// cmbEncryptionMethod
			// 
			this.cmbEncryptionMethod.Items.AddRange(new object[] {
																	 "3DES",
																	 "AES",
																	 "Camellia",
																	 "DES",
																	 "RC4"});
			this.cmbEncryptionMethod.Location = new System.Drawing.Point(136, 75);
			this.cmbEncryptionMethod.Name = "cmbEncryptionMethod";
			this.cmbEncryptionMethod.Size = new System.Drawing.Size(125, 21);
			this.cmbEncryptionMethod.TabIndex = 4;
			// 
			// cmbEncryptedDataType
			// 
			this.cmbEncryptedDataType.Items.AddRange(new object[] {
																	  "Element",
																	  "Content",
																	  "External File"});
			this.cmbEncryptedDataType.Location = new System.Drawing.Point(136, 50);
			this.cmbEncryptedDataType.Name = "cmbEncryptedDataType";
			this.cmbEncryptedDataType.Size = new System.Drawing.Size(125, 21);
			this.cmbEncryptedDataType.TabIndex = 3;
			this.cmbEncryptedDataType.SelectedIndexChanged += new System.EventHandler(this.cmbEncryptedDataType_SelectedIndexChanged);
			// 
			// lbEncryptionMethod
			// 
			this.lbEncryptionMethod.Location = new System.Drawing.Point(16, 79);
			this.lbEncryptionMethod.Name = "lbEncryptionMethod";
			this.lbEncryptionMethod.Size = new System.Drawing.Size(100, 16);
			this.lbEncryptionMethod.TabIndex = 2;
			this.lbEncryptionMethod.Text = "Encryption method";
			// 
			// lbEnryptedDataType
			// 
			this.lbEnryptedDataType.Location = new System.Drawing.Point(16, 54);
			this.lbEnryptedDataType.Name = "lbEnryptedDataType";
			this.lbEnryptedDataType.Size = new System.Drawing.Size(112, 16);
			this.lbEnryptedDataType.TabIndex = 1;
			this.lbEnryptedDataType.Text = "Enrypted Data Type:";
			// 
			// cbEncryptKey
			// 
			this.cbEncryptKey.Location = new System.Drawing.Point(16, 24);
			this.cbEncryptKey.Name = "cbEncryptKey";
			this.cbEncryptKey.TabIndex = 0;
			this.cbEncryptKey.Text = "Encrypt Key";
			this.cbEncryptKey.CheckedChanged += new System.EventHandler(this.cbEncryptKey_CheckedChanged);
			// 
			// gbKEK
			// 
			this.gbKEK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.gbKEK.Controls.Add(this.cmbKeyWrap);
			this.gbKEK.Controls.Add(this.lbKeyWrap);
			this.gbKEK.Controls.Add(this.cmbKeyTransport);
			this.gbKEK.Controls.Add(this.lbKeyTransport);
			this.gbKEK.Controls.Add(this.rgKEK);
			this.gbKEK.Location = new System.Drawing.Point(8, 180);
			this.gbKEK.Name = "gbKEK";
			this.gbKEK.Size = new System.Drawing.Size(272, 160);
			this.gbKEK.TabIndex = 1;
			this.gbKEK.TabStop = false;
			this.gbKEK.Text = "Key Encryption Key (KEK)";
			// 
			// cmbKeyWrap
			// 
			this.cmbKeyWrap.Items.AddRange(new object[] {
															"3DES",
															"AES 128 bit",
															"AES 192 bit",
															"AES 256 bit"});
			this.cmbKeyWrap.Location = new System.Drawing.Point(136, 130);
			this.cmbKeyWrap.Name = "cmbKeyWrap";
			this.cmbKeyWrap.Size = new System.Drawing.Size(125, 21);
			this.cmbKeyWrap.TabIndex = 4;
			// 
			// lbKeyWrap
			// 
			this.lbKeyWrap.Location = new System.Drawing.Point(16, 134);
			this.lbKeyWrap.Name = "lbKeyWrap";
			this.lbKeyWrap.Size = new System.Drawing.Size(88, 16);
			this.lbKeyWrap.TabIndex = 3;
			this.lbKeyWrap.Text = "Key Wrap:";
			// 
			// cmbKeyTransport
			// 
			this.cmbKeyTransport.Items.AddRange(new object[] {
																 "RSA v1.5",
																 "RSA-OAEP"});
			this.cmbKeyTransport.Location = new System.Drawing.Point(136, 104);
			this.cmbKeyTransport.Name = "cmbKeyTransport";
			this.cmbKeyTransport.Size = new System.Drawing.Size(125, 21);
			this.cmbKeyTransport.TabIndex = 2;
			// 
			// lbKeyTransport
			// 
			this.lbKeyTransport.Location = new System.Drawing.Point(16, 107);
			this.lbKeyTransport.Name = "lbKeyTransport";
			this.lbKeyTransport.Size = new System.Drawing.Size(88, 16);
			this.lbKeyTransport.TabIndex = 1;
			this.lbKeyTransport.Text = "Key Transport:";
			// 
			// rgKEK
			// 
			this.rgKEK.Controls.Add(this.rbKeyWrap);
			this.rgKEK.Controls.Add(this.rbKeyTransport);
			this.rgKEK.Location = new System.Drawing.Point(16, 27);
			this.rgKEK.Name = "rgKEK";
			this.rgKEK.Size = new System.Drawing.Size(245, 69);
			this.rgKEK.TabIndex = 0;
			this.rgKEK.TabStop = false;
			this.rgKEK.Text = "KEK type:";
			// 
			// rbKeyWrap
			// 
			this.rbKeyWrap.Location = new System.Drawing.Point(16, 40);
			this.rbKeyWrap.Name = "rbKeyWrap";
			this.rbKeyWrap.TabIndex = 1;
			this.rbKeyWrap.Text = "Key Wrap";
			this.rbKeyWrap.CheckedChanged += new System.EventHandler(this.rbKeyWrap_CheckedChanged);
			// 
			// rbKeyTransport
			// 
			this.rbKeyTransport.Location = new System.Drawing.Point(16, 16);
			this.rbKeyTransport.Name = "rbKeyTransport";
			this.rbKeyTransport.TabIndex = 0;
			this.rbKeyTransport.Text = "Key Transport";
			this.rbKeyTransport.CheckedChanged += new System.EventHandler(this.rbKeyTransport_CheckedChanged);
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
			this.gbKeyInfo.Location = new System.Drawing.Point(8, 344);
			this.gbKeyInfo.Name = "gbKeyInfo";
			this.gbKeyInfo.Size = new System.Drawing.Size(272, 112);
			this.gbKeyInfo.TabIndex = 2;
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
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(120, 464);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(200, 464);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			// 
			// EncForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(288, 494);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.gbKeyInfo);
			this.Controls.Add(this.gbKEK);
			this.Controls.Add(this.gbGeneralEnc);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "EncForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Options";
			this.VisibleChanged += new System.EventHandler(this.EncForm_VisibleChanged);
			this.gbGeneralEnc.ResumeLayout(false);
			this.gbKEK.ResumeLayout(false);
			this.rgKEK.ResumeLayout(false);
			this.gbKeyInfo.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.GroupBox gbGeneralEnc;
		private System.Windows.Forms.CheckBox cbEncryptKey;
		private System.Windows.Forms.Label lbEnryptedDataType;
		private System.Windows.Forms.Label lbEncryptionMethod;
		private System.Windows.Forms.ComboBox cmbEncryptedDataType;
		private System.Windows.Forms.ComboBox cmbEncryptionMethod;
		private System.Windows.Forms.Label lbMimeType;
		private System.Windows.Forms.TextBox edMimeType;
		private System.Windows.Forms.Label lbExternalFile;
		private System.Windows.Forms.TextBox edExternalFile;
		private System.Windows.Forms.Button sbExternalFile;
		private System.Windows.Forms.GroupBox gbKEK;
		private System.Windows.Forms.GroupBox rgKEK;
		private System.Windows.Forms.Label lbKeyTransport;
		private System.Windows.Forms.ComboBox cmbKeyTransport;
		private System.Windows.Forms.ComboBox cmbKeyWrap;
		private System.Windows.Forms.Label lbKeyWrap;
		private System.Windows.Forms.GroupBox gbKeyInfo;
		private System.Windows.Forms.Label lbKeyName;
		private System.Windows.Forms.TextBox edKeyName;
		private System.Windows.Forms.Label lbKeyFile;
		private System.Windows.Forms.Label lbPassphrase;
		private System.Windows.Forms.TextBox edKeyFile;
		private System.Windows.Forms.TextBox edPassphrase;
		private System.Windows.Forms.Button sbKeyFile;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.RadioButton rbKeyTransport;
		private System.Windows.Forms.RadioButton rbKeyWrap;
		private System.Windows.Forms.OpenFileDialog dlgOpen;
		private System.Windows.Forms.SaveFileDialog dlgSave;

		public bool LockOpt;

		private void rbKeyTransport_CheckedChanged(object sender, System.EventArgs e)
		{
			rbKeyWrap.Checked = !rbKeyTransport.Checked;
			UpdateOpt();
		}

		private void rbKeyWrap_CheckedChanged(object sender, System.EventArgs e)
		{
			rbKeyTransport.Checked = !rbKeyWrap.Checked;
			UpdateOpt();
		}

		public bool EncryptKey 
		{
			get 
			{
				return cbEncryptKey.Checked;
			}
			set
			{
				cbEncryptKey.Checked = value;
			}
		}

		public short EncryptedDataType
		{
			get
			{
				switch (cmbEncryptedDataType.SelectedIndex)
				{
					case 1: return SBXMLSec.Unit.xedtContent;
					case 2: return SBXMLSec.Unit.xedtExternal;
					default: return SBXMLSec.Unit.xedtElement;
				}
			}
			set
			{
				switch (value)
				{
					case SBXMLSec.Unit.xedtElement: 
					{
						cmbEncryptedDataType.SelectedIndex = 0;
						break;
					}
					case SBXMLSec.Unit.xedtContent: 
					{
						cmbEncryptedDataType.SelectedIndex = 1;
						break;
					}
					case SBXMLSec.Unit.xedtExternal: 
					{
						cmbEncryptedDataType.SelectedIndex = 2;
						break;
					}
				}
			}
		}

		public short EncryptionMethod
		{
			get
			{
				switch (cmbEncryptionMethod.SelectedIndex)
				{
					case 1: return SBXMLSec.Unit.xemAES;
					case 2: return SBXMLSec.Unit.xemCamellia;
					case 3: return SBXMLSec.Unit.xemDES;
					case 4: return SBXMLSec.Unit.xemRC4;
					default: return SBXMLSec.Unit.xem3DES;
				}
			}
			set
			{
				switch (value)
				{
					case SBXMLSec.Unit.xem3DES: 
					{
						cmbEncryptionMethod.SelectedIndex = 0;
						break;
					}
					case SBXMLSec.Unit.xemAES: 
					{
						cmbEncryptionMethod.SelectedIndex = 1;
						break;
					}
					case SBXMLSec.Unit.xemCamellia: 
					{
						cmbEncryptionMethod.SelectedIndex = 2;
						break;
					}
					case SBXMLSec.Unit.xemDES: 
					{
						cmbEncryptionMethod.SelectedIndex = 3;
						break;
					}
					case SBXMLSec.Unit.xemRC4: 
					{
						cmbEncryptionMethod.SelectedIndex = 4;
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

		public short KeyEncryptionType
		{
			get
			{
				if (rbKeyWrap.Checked)
					return SBXMLSec.Unit.xetKeyWrap;
				else
					return SBXMLSec.Unit.xetKeyTransport;
			}
			set
			{
				if (value == SBXMLSec.Unit.xetKeyTransport)
					rbKeyTransport.Checked = true;
				else
					rbKeyWrap.Checked = true;
			}
		}

		public short KeyTransportMethod
		{
			get
			{
				if (cmbKeyTransport.SelectedIndex == 0)
					return SBXMLSec.Unit.xktRSA15;
				else
					return SBXMLSec.Unit.xktRSAOAEP;
			}
			set
			{
				if (value == SBXMLSec.Unit.xktRSA15)
					cmbKeyTransport.SelectedIndex = 0;
				else
					cmbKeyTransport.SelectedIndex = 1;
			}
		}
	
		public short KeyWrapMethod
		{
			get
			{
				switch (cmbKeyWrap.SelectedIndex)
				{
					case 1: return SBXMLSec.Unit.xwmAES128;
					case 2: return SBXMLSec.Unit.xwmAES192;
					case 3: return SBXMLSec.Unit.xwmAES256;
					default: return SBXMLSec.Unit.xwm3DES;
				}
			}
			set
			{
				switch (value)
				{
					case SBXMLSec.Unit.xwm3DES: 
					{
						cmbKeyWrap.SelectedIndex = 0;
						break;
					}
					case SBXMLSec.Unit.xwmAES128: 
					{
						cmbKeyWrap.SelectedIndex = 1;
						break;
					}
					case SBXMLSec.Unit.xwmAES192: 
					{
						cmbKeyWrap.SelectedIndex = 2;
						break;
					}
					case SBXMLSec.Unit.xwmAES256: 
					{
						cmbKeyWrap.SelectedIndex = 3;
						break;
					}
				}
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

		public string MimeType
		{
			get
			{
				return edMimeType.Text;
			}
			set
			{
				edMimeType.Text = value;
			}
		}

		public string ExternalFile
		{
			get
			{
				return edExternalFile.Text;
			}
			set
			{
				edExternalFile.Text = value;
			}
		}

		public void UpdateOpt()
		{
			rgKEK.Enabled = cbEncryptKey.Checked;
			cmbKeyTransport.Enabled = cbEncryptKey.Checked;
			lbKeyTransport.Enabled = cmbKeyTransport.Enabled;
			cmbKeyWrap.Enabled = cbEncryptKey.Checked;
			lbKeyWrap.Enabled = cmbKeyWrap.Enabled;
			cmbKeyTransport.Enabled = cbEncryptKey.Checked && rbKeyTransport.Checked;
			lbKeyTransport.Enabled = cmbKeyTransport.Enabled;
			cmbKeyWrap.Enabled = cbEncryptKey.Checked && rbKeyWrap.Checked;
			lbKeyWrap.Enabled = cmbKeyWrap.Enabled;
			edPassphrase.Enabled = cmbKeyTransport.Enabled;
			lbPassphrase.Enabled = edPassphrase.Enabled;
			edMimeType.Enabled = (cmbEncryptedDataType.SelectedIndex == 2);
			lbMimeType.Enabled = edMimeType.Enabled;
			edExternalFile.Enabled = (cmbEncryptedDataType.SelectedIndex == 2);
			lbExternalFile.Enabled = edExternalFile.Enabled;
			sbExternalFile.Enabled = edExternalFile.Enabled;
		}

		private void cbEncryptKey_CheckedChanged(object sender, System.EventArgs e)
		{
			UpdateOpt();
		}

		private void cmbEncryptedDataType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			UpdateOpt();
		}

		private void EncForm_VisibleChanged(object sender, System.EventArgs e)
		{
			UpdateOpt();
		}

		private void sbKeyFile_Click(object sender, System.EventArgs e)
		{
			dlgOpen.FileName = edKeyFile.Text;
			if (dlgOpen.ShowDialog() == DialogResult.OK)
				edKeyFile.Text = dlgOpen.FileName;
		}

		private void sbExternalFile_Click(object sender, System.EventArgs e)
		{
			if (LockOpt)
			{
				dlgSave.FileName = edExternalFile.Text;
				if (dlgSave.ShowDialog() == DialogResult.OK)
					edExternalFile.Text = dlgSave.FileName;
			}
			else
			{
				dlgOpen.FileName = edExternalFile.Text;
				if (dlgOpen.ShowDialog() == DialogResult.OK)
					edExternalFile.Text = dlgOpen.FileName;
			}
		}
	}
}
