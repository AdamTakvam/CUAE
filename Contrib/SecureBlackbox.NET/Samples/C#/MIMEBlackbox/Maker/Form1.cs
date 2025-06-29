using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

// EldoS MIME Assembly

using SBMIMEUtils;
using SBMIME;
using SBSMIMECore;
using SBMIMEStream;
using SBPGPMIME;
//using SBMIMEUUE;

// EldoS SecureBlackbox Assembly

using SBConstants;
using SBCustomCertStorage;
using SBX509;
using SBRDN;
using SBPGPKeys;

namespace MimeMaker
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private TElPGPKeyring EncryptingKeys;
        private TElPGPKeyring SigningKeys;

		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem_File;
		private System.Windows.Forms.MenuItem menuItem_Help;
		private System.Windows.Forms.MenuItem menuItem_About;
		private System.Windows.Forms.MenuItem menuItem_Assemble;
		private System.Windows.Forms.MenuItem menuItem_AppExit;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.OpenFileDialog openFileDialog_certificate;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage_Options;
        private System.Windows.Forms.TabPage tabPage_part_text;
        private System.Windows.Forms.TabPage tabPage_part_html;
        private System.Windows.Forms.TabPage tabPage_part_attachments;
        private System.Windows.Forms.TextBox textBox_to;
        private System.Windows.Forms.TextBox textBox_from;
        private System.Windows.Forms.Label label_to;
        private System.Windows.Forms.Label label_from;
        private System.Windows.Forms.TextBox textBox_subject;
        private System.Windows.Forms.Label label_subject;
        private System.Windows.Forms.GroupBox groupBox_smime;
        private System.Windows.Forms.CheckBox checkBox_encode;
        private System.Windows.Forms.Label label_encode_options;
        private System.Windows.Forms.CheckBox checkBox_sign_clear_format;
        private System.Windows.Forms.Label label_sign_options;
        private System.Windows.Forms.CheckBox checkBox_sign;
        private System.Windows.Forms.Button button_load_certificate;
        private System.Windows.Forms.Label label_certificate;
        private System.Windows.Forms.TextBox textBox_certificate;
        private System.Windows.Forms.CheckBox checkBox_SignRootHeader;
        private System.Windows.Forms.TextBox textBox_part_plain_text;
        private System.Windows.Forms.CheckBox checkBox_part_plain_text;
        private System.Windows.Forms.CheckBox checkBox_part_html;
        private System.Windows.Forms.TextBox textBox_part_html;
        private System.Windows.Forms.CheckBox checkBox_part_attachments;
        private System.Windows.Forms.CheckBox checkBox_attach_as_uue;
        private System.Windows.Forms.ListBox listBox_attachments;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_attach_add;
        private System.Windows.Forms.Button button_attach_del;
        private System.Windows.Forms.OpenFileDialog openFileDialog_Attachment;
		private System.Windows.Forms.GroupBox groupBox_pgpMime;
		private System.Windows.Forms.Label label_PubKeyring;
		private System.Windows.Forms.Label label_SecKeyring;
		private System.Windows.Forms.Button button_load_pubring;
		private System.Windows.Forms.Button button_load_secring;
		private System.Windows.Forms.GroupBox groupBox_Security;
		private System.Windows.Forms.RadioButton rbDoNotUseSecurity;
		private System.Windows.Forms.RadioButton rbUseSMIME;
		private System.Windows.Forms.RadioButton rbUsePGPMIME;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBox_recipient_certificate;
		private System.Windows.Forms.Button button_load_recipient_certificate;
		private System.Windows.Forms.TextBox textBox_SecKeyring;
		private System.Windows.Forms.TextBox textBox_PubKeyring;
		private System.Windows.Forms.OpenFileDialog openKeyringDialog;
		private SBCustomCertStorage.TElMemoryCertStorage SenderCertStorage;
		private SBCustomCertStorage.TElMemoryCertStorage RecipientCertStorage;
        private System.ComponentModel.IContainer components;

		public Form1()
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

		static Form1()
		{	
			// MIME Box Library Initialization: 
			SBMIME.Unit.Initialize();
			SBSMIMECore.Unit.Initialize();
			SBPGPMIME.Unit.Initialize();
		}

		private void Init()
		{
			this.rbDoNotUseSecurity.Checked = true;
			this.groupBox_pgpMime.Enabled = false;
			this.groupBox_smime.Enabled = false;
			this.label_encode_options.Enabled = false;
			this.label_sign_options.Enabled = false;
			this.checkBox_encode.Enabled = false;
			this.checkBox_sign.Enabled = false;
			this.checkBox_sign_clear_format.Enabled = false;
			this.checkBox_SignRootHeader.Enabled = false;
			EncryptingKeys = new TElPGPKeyring();
			SigningKeys = new TElPGPKeyring();
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
			this.SenderCertStorage = new SBCustomCertStorage.TElMemoryCertStorage();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem_File = new System.Windows.Forms.MenuItem();
			this.menuItem_Assemble = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem_AppExit = new System.Windows.Forms.MenuItem();
			this.menuItem_Help = new System.Windows.Forms.MenuItem();
			this.menuItem_About = new System.Windows.Forms.MenuItem();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.openFileDialog_certificate = new System.Windows.Forms.OpenFileDialog();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPage_Options = new System.Windows.Forms.TabPage();
			this.groupBox_Security = new System.Windows.Forms.GroupBox();
			this.rbUsePGPMIME = new System.Windows.Forms.RadioButton();
			this.rbUseSMIME = new System.Windows.Forms.RadioButton();
			this.rbDoNotUseSecurity = new System.Windows.Forms.RadioButton();
			this.label_sign_options = new System.Windows.Forms.Label();
			this.checkBox_sign = new System.Windows.Forms.CheckBox();
			this.checkBox_sign_clear_format = new System.Windows.Forms.CheckBox();
			this.checkBox_SignRootHeader = new System.Windows.Forms.CheckBox();
			this.label_encode_options = new System.Windows.Forms.Label();
			this.checkBox_encode = new System.Windows.Forms.CheckBox();
			this.groupBox_pgpMime = new System.Windows.Forms.GroupBox();
			this.button_load_secring = new System.Windows.Forms.Button();
			this.button_load_pubring = new System.Windows.Forms.Button();
			this.textBox_SecKeyring = new System.Windows.Forms.TextBox();
			this.label_SecKeyring = new System.Windows.Forms.Label();
			this.textBox_PubKeyring = new System.Windows.Forms.TextBox();
			this.label_PubKeyring = new System.Windows.Forms.Label();
			this.groupBox_smime = new System.Windows.Forms.GroupBox();
			this.button_load_recipient_certificate = new System.Windows.Forms.Button();
			this.textBox_recipient_certificate = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.button_load_certificate = new System.Windows.Forms.Button();
			this.label_certificate = new System.Windows.Forms.Label();
			this.textBox_certificate = new System.Windows.Forms.TextBox();
			this.textBox_to = new System.Windows.Forms.TextBox();
			this.textBox_from = new System.Windows.Forms.TextBox();
			this.label_to = new System.Windows.Forms.Label();
			this.label_from = new System.Windows.Forms.Label();
			this.textBox_subject = new System.Windows.Forms.TextBox();
			this.label_subject = new System.Windows.Forms.Label();
			this.tabPage_part_text = new System.Windows.Forms.TabPage();
			this.checkBox_part_plain_text = new System.Windows.Forms.CheckBox();
			this.textBox_part_plain_text = new System.Windows.Forms.TextBox();
			this.tabPage_part_html = new System.Windows.Forms.TabPage();
			this.textBox_part_html = new System.Windows.Forms.TextBox();
			this.checkBox_part_html = new System.Windows.Forms.CheckBox();
			this.tabPage_part_attachments = new System.Windows.Forms.TabPage();
			this.label1 = new System.Windows.Forms.Label();
			this.button_attach_del = new System.Windows.Forms.Button();
			this.button_attach_add = new System.Windows.Forms.Button();
			this.listBox_attachments = new System.Windows.Forms.ListBox();
			this.checkBox_attach_as_uue = new System.Windows.Forms.CheckBox();
			this.checkBox_part_attachments = new System.Windows.Forms.CheckBox();
			this.openFileDialog_Attachment = new System.Windows.Forms.OpenFileDialog();
			this.openKeyringDialog = new System.Windows.Forms.OpenFileDialog();
			this.RecipientCertStorage = new SBCustomCertStorage.TElMemoryCertStorage();
			this.tabControl.SuspendLayout();
			this.tabPage_Options.SuspendLayout();
			this.groupBox_Security.SuspendLayout();
			this.groupBox_pgpMime.SuspendLayout();
			this.groupBox_smime.SuspendLayout();
			this.tabPage_part_text.SuspendLayout();
			this.tabPage_part_html.SuspendLayout();
			this.tabPage_part_attachments.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem_File,
																					  this.menuItem_Help});
			// 
			// menuItem_File
			// 
			this.menuItem_File.Index = 0;
			this.menuItem_File.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.menuItem_Assemble,
																						  this.menuItem5,
																						  this.menuItem_AppExit});
			this.menuItem_File.Text = "File";
			// 
			// menuItem_Assemble
			// 
			this.menuItem_Assemble.Index = 0;
			this.menuItem_Assemble.Shortcut = System.Windows.Forms.Shortcut.F9;
			this.menuItem_Assemble.Text = "Assemble And Save";
			this.menuItem_Assemble.Click += new System.EventHandler(this.menuItem_Assemble_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 1;
			this.menuItem5.Text = "-";
			// 
			// menuItem_AppExit
			// 
			this.menuItem_AppExit.Index = 2;
			this.menuItem_AppExit.Shortcut = System.Windows.Forms.Shortcut.F10;
			this.menuItem_AppExit.Text = "Exit";
			this.menuItem_AppExit.Click += new System.EventHandler(this.menuItem_AppExit_Click);
			// 
			// menuItem_Help
			// 
			this.menuItem_Help.Index = 1;
			this.menuItem_Help.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.menuItem_About});
			this.menuItem_Help.Text = "Help";
			// 
			// menuItem_About
			// 
			this.menuItem_About.Index = 0;
			this.menuItem_About.Text = "About";
			this.menuItem_About.Click += new System.EventHandler(this.menuItem_About_Click);
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.DefaultExt = "eml";
			this.saveFileDialog.FileName = "Assembled.Message.eml";
			this.saveFileDialog.Filter = "Mail (.eml)|*.eml|All Files (*.*)|*.*";
			this.saveFileDialog.InitialDirectory = ".";
			// 
			// openFileDialog_certificate
			// 
			this.openFileDialog_certificate.Filter = "Certificates (*.pfx;*.p7b;*.cer;*.bin)|*.pfx;*.p7b;*.cer;*.bin|All Files (*.*)|*." +
				"*";
			this.openFileDialog_certificate.InitialDirectory = ".";
			this.openFileDialog_certificate.Title = "Load Certificate From File";
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabPage_Options);
			this.tabControl.Controls.Add(this.tabPage_part_text);
			this.tabControl.Controls.Add(this.tabPage_part_html);
			this.tabControl.Controls.Add(this.tabPage_part_attachments);
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(608, 401);
			this.tabControl.TabIndex = 7;
			// 
			// tabPage_Options
			// 
			this.tabPage_Options.Controls.Add(this.groupBox_Security);
			this.tabPage_Options.Controls.Add(this.groupBox_pgpMime);
			this.tabPage_Options.Controls.Add(this.groupBox_smime);
			this.tabPage_Options.Controls.Add(this.textBox_to);
			this.tabPage_Options.Controls.Add(this.textBox_from);
			this.tabPage_Options.Controls.Add(this.label_to);
			this.tabPage_Options.Controls.Add(this.label_from);
			this.tabPage_Options.Controls.Add(this.textBox_subject);
			this.tabPage_Options.Controls.Add(this.label_subject);
			this.tabPage_Options.Location = new System.Drawing.Point(4, 22);
			this.tabPage_Options.Name = "tabPage_Options";
			this.tabPage_Options.Size = new System.Drawing.Size(600, 375);
			this.tabPage_Options.TabIndex = 0;
			this.tabPage_Options.Text = "Assemble Options";
			// 
			// groupBox_Security
			// 
			this.groupBox_Security.Controls.Add(this.rbUsePGPMIME);
			this.groupBox_Security.Controls.Add(this.rbUseSMIME);
			this.groupBox_Security.Controls.Add(this.rbDoNotUseSecurity);
			this.groupBox_Security.Controls.Add(this.label_sign_options);
			this.groupBox_Security.Controls.Add(this.checkBox_sign);
			this.groupBox_Security.Controls.Add(this.checkBox_sign_clear_format);
			this.groupBox_Security.Controls.Add(this.checkBox_SignRootHeader);
			this.groupBox_Security.Controls.Add(this.label_encode_options);
			this.groupBox_Security.Controls.Add(this.checkBox_encode);
			this.groupBox_Security.Location = new System.Drawing.Point(8, 104);
			this.groupBox_Security.Name = "groupBox_Security";
			this.groupBox_Security.Size = new System.Drawing.Size(280, 260);
			this.groupBox_Security.TabIndex = 14;
			this.groupBox_Security.TabStop = false;
			this.groupBox_Security.Text = "Security settings";
			this.groupBox_Security.Enter += new System.EventHandler(this.groupBox_Security_Enter);
			// 
			// rbUsePGPMIME
			// 
			this.rbUsePGPMIME.Location = new System.Drawing.Point(20, 88);
			this.rbUsePGPMIME.Name = "rbUsePGPMIME";
			this.rbUsePGPMIME.Size = new System.Drawing.Size(232, 24);
			this.rbUsePGPMIME.TabIndex = 13;
			this.rbUsePGPMIME.Text = "Use PGP/MIME security features";
			this.rbUsePGPMIME.CheckedChanged += new System.EventHandler(this.rbUsePGPMIME_CheckedChanged);
			// 
			// rbUseSMIME
			// 
			this.rbUseSMIME.Location = new System.Drawing.Point(20, 60);
			this.rbUseSMIME.Name = "rbUseSMIME";
			this.rbUseSMIME.Size = new System.Drawing.Size(232, 24);
			this.rbUseSMIME.TabIndex = 12;
			this.rbUseSMIME.Text = "Use S/MIME security features";
			this.rbUseSMIME.CheckedChanged += new System.EventHandler(this.rbUseSMIME_CheckedChanged);
			// 
			// rbDoNotUseSecurity
			// 
			this.rbDoNotUseSecurity.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbDoNotUseSecurity.Checked = true;
			this.rbDoNotUseSecurity.Location = new System.Drawing.Point(20, 28);
			this.rbDoNotUseSecurity.Name = "rbDoNotUseSecurity";
			this.rbDoNotUseSecurity.Size = new System.Drawing.Size(208, 28);
			this.rbDoNotUseSecurity.TabIndex = 11;
			this.rbDoNotUseSecurity.TabStop = true;
			this.rbDoNotUseSecurity.Text = "Do not use security features for this message";
			this.rbDoNotUseSecurity.CheckedChanged += new System.EventHandler(this.rbDoNotUseSecurity_CheckedChanged);
			// 
			// label_sign_options
			// 
			this.label_sign_options.Location = new System.Drawing.Point(20, 128);
			this.label_sign_options.Name = "label_sign_options";
			this.label_sign_options.Size = new System.Drawing.Size(100, 16);
			this.label_sign_options.TabIndex = 4;
			this.label_sign_options.Text = "Sign Options:";
			this.label_sign_options.Click += new System.EventHandler(this.label_sign_options_Click);
			// 
			// checkBox_sign
			// 
			this.checkBox_sign.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkBox_sign.Location = new System.Drawing.Point(44, 148);
			this.checkBox_sign.Name = "checkBox_sign";
			this.checkBox_sign.Size = new System.Drawing.Size(88, 20);
			this.checkBox_sign.TabIndex = 3;
			this.checkBox_sign.Text = "Sign Message";
			this.checkBox_sign.CheckedChanged += new System.EventHandler(this.checkBox_sign_CheckedChanged);
			// 
			// checkBox_sign_clear_format
			// 
			this.checkBox_sign_clear_format.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkBox_sign_clear_format.Location = new System.Drawing.Point(140, 148);
			this.checkBox_sign_clear_format.Name = "checkBox_sign_clear_format";
			this.checkBox_sign_clear_format.Size = new System.Drawing.Size(128, 20);
			this.checkBox_sign_clear_format.TabIndex = 5;
			this.checkBox_sign_clear_format.Text = "Sign in Clear Format";
			this.checkBox_sign_clear_format.CheckedChanged += new System.EventHandler(this.checkBox_sign_clear_format_CheckedChanged);
			// 
			// checkBox_SignRootHeader
			// 
			this.checkBox_SignRootHeader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkBox_SignRootHeader.Location = new System.Drawing.Point(44, 172);
			this.checkBox_SignRootHeader.Name = "checkBox_SignRootHeader";
			this.checkBox_SignRootHeader.Size = new System.Drawing.Size(108, 20);
			this.checkBox_SignRootHeader.TabIndex = 10;
			this.checkBox_SignRootHeader.Text = "Sign Root Headers";
			this.checkBox_SignRootHeader.CheckedChanged += new System.EventHandler(this.checkBox_SignRootHeader_CheckedChanged);
			// 
			// label_encode_options
			// 
			this.label_encode_options.Location = new System.Drawing.Point(20, 200);
			this.label_encode_options.Name = "label_encode_options";
			this.label_encode_options.Size = new System.Drawing.Size(100, 16);
			this.label_encode_options.TabIndex = 6;
			this.label_encode_options.Text = "Encryption Options:";
			this.label_encode_options.Click += new System.EventHandler(this.label_encode_options_Click);
			// 
			// checkBox_encode
			// 
			this.checkBox_encode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkBox_encode.Location = new System.Drawing.Point(40, 220);
			this.checkBox_encode.Name = "checkBox_encode";
			this.checkBox_encode.Size = new System.Drawing.Size(112, 16);
			this.checkBox_encode.TabIndex = 7;
			this.checkBox_encode.Text = "Encrypt Message";
			this.checkBox_encode.CheckedChanged += new System.EventHandler(this.checkBox_encode_CheckedChanged);
			// 
			// groupBox_pgpMime
			// 
			this.groupBox_pgpMime.Controls.Add(this.button_load_secring);
			this.groupBox_pgpMime.Controls.Add(this.button_load_pubring);
			this.groupBox_pgpMime.Controls.Add(this.textBox_SecKeyring);
			this.groupBox_pgpMime.Controls.Add(this.label_SecKeyring);
			this.groupBox_pgpMime.Controls.Add(this.textBox_PubKeyring);
			this.groupBox_pgpMime.Controls.Add(this.label_PubKeyring);
			this.groupBox_pgpMime.Location = new System.Drawing.Point(296, 104);
			this.groupBox_pgpMime.Name = "groupBox_pgpMime";
			this.groupBox_pgpMime.Size = new System.Drawing.Size(292, 128);
			this.groupBox_pgpMime.TabIndex = 13;
			this.groupBox_pgpMime.TabStop = false;
			this.groupBox_pgpMime.Text = "PGP/MIME options";
			// 
			// button_load_secring
			// 
			this.button_load_secring.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button_load_secring.Location = new System.Drawing.Point(256, 92);
			this.button_load_secring.Name = "button_load_secring";
			this.button_load_secring.Size = new System.Drawing.Size(24, 23);
			this.button_load_secring.TabIndex = 5;
			this.button_load_secring.Text = "...";
			this.button_load_secring.Click += new System.EventHandler(this.button_load_secring_Click);
			// 
			// button_load_pubring
			// 
			this.button_load_pubring.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button_load_pubring.Location = new System.Drawing.Point(256, 44);
			this.button_load_pubring.Name = "button_load_pubring";
			this.button_load_pubring.Size = new System.Drawing.Size(24, 23);
			this.button_load_pubring.TabIndex = 4;
			this.button_load_pubring.Text = "...";
			this.button_load_pubring.Click += new System.EventHandler(this.button_load_pubring_Click);
			// 
			// textBox_SecKeyring
			// 
			this.textBox_SecKeyring.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox_SecKeyring.Location = new System.Drawing.Point(12, 92);
			this.textBox_SecKeyring.Name = "textBox_SecKeyring";
			this.textBox_SecKeyring.Size = new System.Drawing.Size(240, 21);
			this.textBox_SecKeyring.TabIndex = 3;
			this.textBox_SecKeyring.Text = "";
			// 
			// label_SecKeyring
			// 
			this.label_SecKeyring.Location = new System.Drawing.Point(12, 72);
			this.label_SecKeyring.Name = "label_SecKeyring";
			this.label_SecKeyring.Size = new System.Drawing.Size(204, 16);
			this.label_SecKeyring.TabIndex = 2;
			this.label_SecKeyring.Text = "Secret keyring file:";
			// 
			// textBox_PubKeyring
			// 
			this.textBox_PubKeyring.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox_PubKeyring.Location = new System.Drawing.Point(12, 44);
			this.textBox_PubKeyring.Name = "textBox_PubKeyring";
			this.textBox_PubKeyring.Size = new System.Drawing.Size(240, 21);
			this.textBox_PubKeyring.TabIndex = 1;
			this.textBox_PubKeyring.Text = "";
			// 
			// label_PubKeyring
			// 
			this.label_PubKeyring.Location = new System.Drawing.Point(12, 24);
			this.label_PubKeyring.Name = "label_PubKeyring";
			this.label_PubKeyring.Size = new System.Drawing.Size(164, 16);
			this.label_PubKeyring.TabIndex = 0;
			this.label_PubKeyring.Text = "Public keyring file:";
			// 
			// groupBox_smime
			// 
			this.groupBox_smime.Controls.Add(this.button_load_recipient_certificate);
			this.groupBox_smime.Controls.Add(this.textBox_recipient_certificate);
			this.groupBox_smime.Controls.Add(this.label2);
			this.groupBox_smime.Controls.Add(this.button_load_certificate);
			this.groupBox_smime.Controls.Add(this.label_certificate);
			this.groupBox_smime.Controls.Add(this.textBox_certificate);
			this.groupBox_smime.Location = new System.Drawing.Point(296, 236);
			this.groupBox_smime.Name = "groupBox_smime";
			this.groupBox_smime.Size = new System.Drawing.Size(292, 128);
			this.groupBox_smime.TabIndex = 12;
			this.groupBox_smime.TabStop = false;
			this.groupBox_smime.Text = "S/MIME options";
			// 
			// button_load_recipient_certificate
			// 
			this.button_load_recipient_certificate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button_load_recipient_certificate.Location = new System.Drawing.Point(256, 84);
			this.button_load_recipient_certificate.Name = "button_load_recipient_certificate";
			this.button_load_recipient_certificate.Size = new System.Drawing.Size(24, 23);
			this.button_load_recipient_certificate.TabIndex = 5;
			this.button_load_recipient_certificate.Text = "...";
			this.button_load_recipient_certificate.Click += new System.EventHandler(this.button_load_recipient_certificate_Click);
			// 
			// textBox_recipient_certificate
			// 
			this.textBox_recipient_certificate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox_recipient_certificate.Location = new System.Drawing.Point(12, 84);
			this.textBox_recipient_certificate.Name = "textBox_recipient_certificate";
			this.textBox_recipient_certificate.Size = new System.Drawing.Size(240, 21);
			this.textBox_recipient_certificate.TabIndex = 4;
			this.textBox_recipient_certificate.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 68);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(176, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Recipients certificate:";
			// 
			// button_load_certificate
			// 
			this.button_load_certificate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button_load_certificate.Location = new System.Drawing.Point(256, 40);
			this.button_load_certificate.Name = "button_load_certificate";
			this.button_load_certificate.Size = new System.Drawing.Size(24, 23);
			this.button_load_certificate.TabIndex = 2;
			this.button_load_certificate.Text = "...";
			this.button_load_certificate.Click += new System.EventHandler(this.button_load_certificate_Click);
			// 
			// label_certificate
			// 
			this.label_certificate.Location = new System.Drawing.Point(12, 24);
			this.label_certificate.Name = "label_certificate";
			this.label_certificate.Size = new System.Drawing.Size(100, 16);
			this.label_certificate.TabIndex = 1;
			this.label_certificate.Text = "Senders certificate:";
			// 
			// textBox_certificate
			// 
			this.textBox_certificate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox_certificate.Location = new System.Drawing.Point(12, 40);
			this.textBox_certificate.Name = "textBox_certificate";
			this.textBox_certificate.Size = new System.Drawing.Size(240, 21);
			this.textBox_certificate.TabIndex = 0;
			this.textBox_certificate.Text = "";
			// 
			// textBox_to
			// 
			this.textBox_to.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox_to.Location = new System.Drawing.Point(72, 68);
			this.textBox_to.Name = "textBox_to";
			this.textBox_to.Size = new System.Drawing.Size(512, 21);
			this.textBox_to.TabIndex = 11;
			this.textBox_to.Text = "demos@localhost";
			// 
			// textBox_from
			// 
			this.textBox_from.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox_from.Location = new System.Drawing.Point(72, 40);
			this.textBox_from.Name = "textBox_from";
			this.textBox_from.Size = new System.Drawing.Size(512, 21);
			this.textBox_from.TabIndex = 10;
			this.textBox_from.Text = "demos@eldos.org";
			// 
			// label_to
			// 
			this.label_to.BackColor = System.Drawing.SystemColors.ControlDark;
			this.label_to.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.label_to.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.label_to.Location = new System.Drawing.Point(8, 66);
			this.label_to.Name = "label_to";
			this.label_to.Size = new System.Drawing.Size(60, 23);
			this.label_to.TabIndex = 9;
			this.label_to.Text = "To :";
			this.label_to.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label_from
			// 
			this.label_from.BackColor = System.Drawing.SystemColors.ControlDark;
			this.label_from.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.label_from.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.label_from.Location = new System.Drawing.Point(8, 40);
			this.label_from.Name = "label_from";
			this.label_from.Size = new System.Drawing.Size(60, 23);
			this.label_from.TabIndex = 8;
			this.label_from.Text = "From :";
			this.label_from.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// textBox_subject
			// 
			this.textBox_subject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox_subject.Location = new System.Drawing.Point(72, 12);
			this.textBox_subject.Name = "textBox_subject";
			this.textBox_subject.Size = new System.Drawing.Size(512, 21);
			this.textBox_subject.TabIndex = 7;
			this.textBox_subject.Text = "EldoS MimeBlackbox Example.";
			// 
			// label_subject
			// 
			this.label_subject.BackColor = System.Drawing.SystemColors.ControlDark;
			this.label_subject.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.label_subject.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.label_subject.Location = new System.Drawing.Point(8, 12);
			this.label_subject.Name = "label_subject";
			this.label_subject.Size = new System.Drawing.Size(60, 23);
			this.label_subject.TabIndex = 6;
			this.label_subject.Text = "Subject :";
			this.label_subject.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tabPage_part_text
			// 
			this.tabPage_part_text.Controls.Add(this.checkBox_part_plain_text);
			this.tabPage_part_text.Controls.Add(this.textBox_part_plain_text);
			this.tabPage_part_text.Location = new System.Drawing.Point(4, 22);
			this.tabPage_part_text.Name = "tabPage_part_text";
			this.tabPage_part_text.Size = new System.Drawing.Size(600, 375);
			this.tabPage_part_text.TabIndex = 1;
			this.tabPage_part_text.Text = "Text Part";
			// 
			// checkBox_part_plain_text
			// 
			this.checkBox_part_plain_text.Checked = true;
			this.checkBox_part_plain_text.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox_part_plain_text.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkBox_part_plain_text.Location = new System.Drawing.Point(8, 4);
			this.checkBox_part_plain_text.Name = "checkBox_part_plain_text";
			this.checkBox_part_plain_text.TabIndex = 1;
			this.checkBox_part_plain_text.Text = "Use this part";
			// 
			// textBox_part_plain_text
			// 
			this.textBox_part_plain_text.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBox_part_plain_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox_part_plain_text.Location = new System.Drawing.Point(4, 32);
			this.textBox_part_plain_text.Multiline = true;
			this.textBox_part_plain_text.Name = "textBox_part_plain_text";
			this.textBox_part_plain_text.Size = new System.Drawing.Size(592, 340);
			this.textBox_part_plain_text.TabIndex = 0;
			this.textBox_part_plain_text.Text = "Plaint text part.";
			// 
			// tabPage_part_html
			// 
			this.tabPage_part_html.Controls.Add(this.textBox_part_html);
			this.tabPage_part_html.Controls.Add(this.checkBox_part_html);
			this.tabPage_part_html.Location = new System.Drawing.Point(4, 22);
			this.tabPage_part_html.Name = "tabPage_part_html";
			this.tabPage_part_html.Size = new System.Drawing.Size(600, 375);
			this.tabPage_part_html.TabIndex = 2;
			this.tabPage_part_html.Text = "HTML Part";
			// 
			// textBox_part_html
			// 
			this.textBox_part_html.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBox_part_html.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox_part_html.Location = new System.Drawing.Point(4, 36);
			this.textBox_part_html.Multiline = true;
			this.textBox_part_html.Name = "textBox_part_html";
			this.textBox_part_html.Size = new System.Drawing.Size(592, 336);
			this.textBox_part_html.TabIndex = 1;
			this.textBox_part_html.Text = "<HTML>\r\n  <BR>Html code part.<BR>\r\n  <HR>\r\n</HTML>";
			// 
			// checkBox_part_html
			// 
			this.checkBox_part_html.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkBox_part_html.Location = new System.Drawing.Point(4, 8);
			this.checkBox_part_html.Name = "checkBox_part_html";
			this.checkBox_part_html.Size = new System.Drawing.Size(108, 24);
			this.checkBox_part_html.TabIndex = 0;
			this.checkBox_part_html.Text = "Use this part";
			// 
			// tabPage_part_attachments
			// 
			this.tabPage_part_attachments.Controls.Add(this.label1);
			this.tabPage_part_attachments.Controls.Add(this.button_attach_del);
			this.tabPage_part_attachments.Controls.Add(this.button_attach_add);
			this.tabPage_part_attachments.Controls.Add(this.listBox_attachments);
			this.tabPage_part_attachments.Controls.Add(this.checkBox_attach_as_uue);
			this.tabPage_part_attachments.Controls.Add(this.checkBox_part_attachments);
			this.tabPage_part_attachments.Location = new System.Drawing.Point(4, 22);
			this.tabPage_part_attachments.Name = "tabPage_part_attachments";
			this.tabPage_part_attachments.Size = new System.Drawing.Size(600, 375);
			this.tabPage_part_attachments.TabIndex = 3;
			this.tabPage_part_attachments.Text = "Attachments";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(440, 132);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(148, 68);
			this.label1.TabIndex = 5;
			this.label1.Text = "- Warning: Not recomended make multipart message with UUE attachments.";
			// 
			// button_attach_del
			// 
			this.button_attach_del.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button_attach_del.Enabled = false;
			this.button_attach_del.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button_attach_del.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
			this.button_attach_del.Location = new System.Drawing.Point(436, 64);
			this.button_attach_del.Name = "button_attach_del";
			this.button_attach_del.Size = new System.Drawing.Size(156, 23);
			this.button_attach_del.TabIndex = 4;
			this.button_attach_del.Text = "Remove Attachment";
			this.button_attach_del.Click += new System.EventHandler(this.button_attach_del_Click);
			// 
			// button_attach_add
			// 
			this.button_attach_add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button_attach_add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button_attach_add.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
			this.button_attach_add.Location = new System.Drawing.Point(436, 32);
			this.button_attach_add.Name = "button_attach_add";
			this.button_attach_add.Size = new System.Drawing.Size(156, 23);
			this.button_attach_add.TabIndex = 3;
			this.button_attach_add.Text = "Add Attachment";
			this.button_attach_add.Click += new System.EventHandler(this.button_attach_add_Click);
			// 
			// listBox_attachments
			// 
			this.listBox_attachments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.listBox_attachments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.listBox_attachments.HorizontalScrollbar = true;
			this.listBox_attachments.Location = new System.Drawing.Point(8, 36);
			this.listBox_attachments.Name = "listBox_attachments";
			this.listBox_attachments.Size = new System.Drawing.Size(400, 327);
			this.listBox_attachments.TabIndex = 2;
			// 
			// checkBox_attach_as_uue
			// 
			this.checkBox_attach_as_uue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBox_attach_as_uue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkBox_attach_as_uue.Location = new System.Drawing.Point(440, 96);
			this.checkBox_attach_as_uue.Name = "checkBox_attach_as_uue";
			this.checkBox_attach_as_uue.TabIndex = 1;
			this.checkBox_attach_as_uue.Text = "Attach as UUE";
			// 
			// checkBox_part_attachments
			// 
			this.checkBox_part_attachments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkBox_part_attachments.Location = new System.Drawing.Point(4, 8);
			this.checkBox_part_attachments.Name = "checkBox_part_attachments";
			this.checkBox_part_attachments.TabIndex = 0;
			this.checkBox_part_attachments.Text = "Attach Files";
			// 
			// openFileDialog_Attachment
			// 
			this.openFileDialog_Attachment.Filter = "All Files (*.*)|*.*";
			// 
			// openKeyringDialog
			// 
			this.openKeyringDialog.Filter = "PGP Keyrings (*.asc, *.pgp, *.gpg, *.pkr, *.skr)|*.asc;*.gpg;*.pgp;*.pkr;*.skr|Al" +
				"l files (*.*)|*.*";
			this.openKeyringDialog.InitialDirectory = ".";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(608, 401);
			this.Controls.Add(this.tabControl);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.Text = "MimeMaker";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.tabControl.ResumeLayout(false);
			this.tabPage_Options.ResumeLayout(false);
			this.groupBox_Security.ResumeLayout(false);
			this.groupBox_pgpMime.ResumeLayout(false);
			this.groupBox_smime.ResumeLayout(false);
			this.tabPage_part_text.ResumeLayout(false);
			this.tabPage_part_html.ResumeLayout(false);
			this.tabPage_part_attachments.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"));
			Application.Run(new Form1());
		}

		private void menuItem_AppExit_Click(object sender, System.EventArgs e)
		{
			if ( Application.AllowQuit )
				Application.Exit();
		}

		private void menuItem_Assemble_Click(object sender, System.EventArgs e)
		{
			int iPartCount = 0;
			if (checkBox_part_plain_text.Checked)
				iPartCount++;
			if (checkBox_part_html.Checked )
                iPartCount++;
            if ( checkBox_part_attachments.Checked && (listBox_attachments.Items.Count > 0) )
				iPartCount++;
			if ( iPartCount == 0 )
                throw new Exception("Warning: Please define any mime part !");
            if ( saveFileDialog.ShowDialog() != DialogResult.OK )
                return;

              TElMessage msg = null;
              TElPlainTextPart mpt = null;
              TElMultiPartList mmp = null;
              TElMessagePart mpc = null;

              try {

                if (iPartCount == 1)
                { // Make one part message
                  msg = new TElMessage(false, SBMIME.Unit.cXMailerDefaultFieldValue);
                  if ( checkBox_part_plain_text.Checked ) {
                    mpt = new TElPlainTextPart(msg, mmp);
                    msg.SetMainPart(mpt, false);
                    mpt.SetText( textBox_part_plain_text.Text );
                  } else
                  if (checkBox_part_html.Checked ) {
                    mpt = new TElPlainTextPart(msg, mmp);
                    msg.SetMainPart(mpt, false);
                    mpt.SetContentSubtype("html", true);
                    mpt.SetText( textBox_part_html.Text );
                  }
                  else
                  if ( checkBox_part_attachments.Checked && (listBox_attachments.Items.Count > 0) ) {
					{
                      for ( int i = 0; i < listBox_attachments.Items.Count; i++ )
                        msg.AttachFile(""/* == application/octet-stream'*/, (string) listBox_attachments.Items[i] );
                    }
                  }
                }
                else
                { // Make multiparts message
                  msg = new TElMessage(false/*== do not create default main part*/, SBMIME.Unit.cXMailerDefaultFieldValue);
                  mmp = new TElMultiPartList(msg, null);
                  msg.SetMainPart(mmp, false);

                  if ( checkBox_part_plain_text.Checked )
                  {
                    mpt = new TElPlainTextPart(msg, mmp);
                    mmp.AddPart(mpt);
                    mpt.SetText( textBox_part_plain_text.Text );
                  }
                  if (checkBox_part_html.Checked )
                  {
                    mpt = new TElPlainTextPart(msg, mmp);
                    mmp.AddPart(mpt);
                    mpt.SetContentSubtype("html", true);
                    mpt.SetText( textBox_part_html.Text );
                  }
                  if ( checkBox_part_attachments.Checked && (listBox_attachments.Items.Count > 0) )
                  {
                    {
                      for ( int i = 0; i < listBox_attachments.Items.Count; i++ ) {
                        string fileName = (string) listBox_attachments.Items[i];
                        // 1)
                        // msg.AttachFile(""/* == application/octet-stream'*/, fileName );
                        // or alternative (manual):
                        // 2)
                        mpc = new TElMessagePart(msg, mmp);
                        mmp.AddPart(mpc);
                        string s = SBMIMEUtils.Unit.ExtractWideFileName( fileName );
                        TElMessageHeaderField headerField = mpc.Header.AddField("Content-Type", "application/octet-stream", true);
                        headerField.AddParam("name", s);
                        headerField = mpc.Header.AddField("Content-Disposition", "attachment", true);
                        headerField.AddParam("filename", s);
                        mpc.Header.AddField("Content-Transfer-Encoding", "base64", true);
                        TAnsiStringStream file_data = new TAnsiStringStream();
                        file_data.LoadFromFile( fileName );
                        mpc.SetData(file_data, (int)file_data.Length, false); // stream controled in mpc
                        file_data = null;
                      }//of: for i
                    }
                  }//of: if ( checkBox_part_attachments.Checked && (listBox_attachments.Items.Count > 0) )

                }//of: if (iPartCount == 1)

                msg.From.AddAddress("From e-mail: "+textBox_from.Text, textBox_from.Text);
                msg.To_.AddAddress("To e-mail: " + textBox_to.Text, textBox_to.Text);
                msg.SetSubject(textBox_subject.Text);
                msg.SetDate( System.DateTime.Now );
                msg.MessageID = TElMessage.GenerateMessageID();

				  if (rbUseSMIME.Checked) 
				  {
					  if ( checkBox_sign.Checked || checkBox_encode.Checked ) 
					  {
						  TElMessagePartHandlerSMime smime = new TElMessagePartHandlerSMime(null);
						  msg.MainPart.MessagePartHandler = smime;

						  if ( checkBox_sign.Checked ) 
						  {
							  smime.EncoderSigned = true;
							  smime.EncoderSignOnlyClearFormat = checkBox_sign_clear_format.Checked;
							  smime.EncoderSignRootHeader = checkBox_SignRootHeader.Checked; // - allow sign some fields in header (subject,... ) except (From, To, Sender, Reply-To)
							  smime.EncoderMicalg = "sha1";
						  }
						  if ( checkBox_encode.Checked ) 
						  {
							  smime.EncoderCrypted = true;
							  smime.EncoderCryptBitsInKey = 128;
							  smime.EncoderCryptAlgorithm = SBConstants.Unit.SB_ALGORITHM_CNT_3DES;
						  }
						  smime.EncoderSignCertStorage = SenderCertStorage;
						  smime.EncoderCryptCertStorage = RecipientCertStorage;
					  } 
				  }

				  if (rbUsePGPMIME.Checked) 
				  {
					  if ( checkBox_sign.Checked || checkBox_encode.Checked )
					  {
						  TElMessagePartHandlerPGPMime pgpmime = new TElMessagePartHandlerPGPMime(null);
						  msg.MainPart.MessagePartHandler = pgpmime;

						  if (checkBox_sign.Checked) 
						  {
							  pgpmime.SigningKeys = SigningKeys;
							  pgpmime.Sign = true;
						  }
						  if (checkBox_encode.Checked) 
						  {
							  pgpmime.EncryptingKeys = EncryptingKeys;
							  pgpmime.Encrypt = true;
						  }
						  pgpmime.OnKeyPassphrase += new SBPGPStreams.TSBPGPKeyPassphraseEvent(OnKeyPassphrase);
					  }
				  }

                TAnsiStringStream sm = new TAnsiStringStream();

                int res = msg.AssembleMessage(sm,
                  // Charset of message:
                  "utf-8",
                  // HeaderEncoding
                  SBMIME.TElHeaderEncoding.heBase64, //  variants:  he8bit  | heQuotedPrintable  | heBase64
                  // BodyEncoding
                  "base64", //  variants:   '8bit' | 'quoted-printable' |  'base64'
                  // AttachEncoding
                  "base64", //  variants:   '8bit' | 'quoted-printable' |  'base64'
                  false
                );

                if ( (res == SBMIME.Unit.EL_OK) || (res == SBMIME.Unit.EL_WARNING) )
                  sm.SaveToFile(saveFileDialog.FileName);
                else
                  throw new ElMimeException("ERROR: Assembling message return error code: "+res.ToString()+"\"");

              }//of: try
              finally {
                if ( msg != null) {
                  IDisposable id = msg as IDisposable;
                  if ( id  != null)
                    id.Dispose();
                }
              }


            }

		private void OnKeyPassphrase(object sender, SBPGPKeys.TElPGPCustomSecretKey key, ref string Passphrase,
			ref bool Cancel) 
		{
			frmRequestPassword dlg = new frmRequestPassword();
			dlg.lPrompt.Text = "Please enter password for key 0x" + SBPGPUtils.Unit.KeyID2Str(key.KeyID(), true);
			if (dlg.ShowDialog() == DialogResult.OK) 
			{
				Cancel = false;
				Passphrase = dlg.tbPassword.Text;
			} 
			else 
			{
				Cancel = true;
			}
		}

		private void menuItem_About_Click(object sender, System.EventArgs e)
		{
          System.Windows.Forms.MessageBox.Show(
            "EldoS MIMEBlackbox Demo Application.\r\n\r\n"+
            "  Mime Viewer, version: 2005.03.20\r\n" +
            "  (" + SBMIME.Unit.cXMailerDefaultFieldValue + ")\r\n\r\n" +
            "    Home page: http://www.secureblackbox.com"
          );
		}

		private void button_load_certificate_Click(object sender, System.EventArgs e)
		{
			if ( openFileDialog_certificate.ShowDialog() == DialogResult.OK )
 			{
				frmRequestPassword dlg = new frmRequestPassword();
				if (dlg.ShowDialog(this) == DialogResult.OK) 
				{
					SenderCertStorage.Clear();
					Boolean bOK = ( SBSMIMECore.Unit.AddToStorageCertificateFromFile(
						// Global SMIME Storage
						SenderCertStorage,
						// file certificate:
						openFileDialog_certificate.FileName,
						// certificate password:
						dlg.tbPassword.Text) );
					if ( bOK )
					{
						TElX509Certificate cer = (TElX509Certificate)
							SenderCertStorage.CertificateList[ SenderCertStorage.Count-1 ];
						ArrayList arList = new ArrayList();
						cer.SubjectRDN.GetValuesByOID(SBUtils.Unit.SB_CERT_OID_EMAIL, arList);
						bOK = arList.Count > 0;
						if ( bOK )
						{
							// Convert UTF8 to unicode string:
							string wsFrom = System.Text.Encoding.UTF8.GetString( (byte []) arList[0] );
							bOK = wsFrom.Length > 0;
							if ( bOK )
							{
								textBox_certificate.Text = openFileDialog_certificate.FileName;
								textBox_from.Text = wsFrom;
							}
						}
					}
					if ( ! bOK ) 
					{
						throw new Exception("Cannot load certificate or certificate is bad");
					}
				}
			}//of : OpenFile
		}

        private void button_attach_add_Click(object sender, System.EventArgs e)
        {
            if ( openFileDialog_Attachment.ShowDialog() == DialogResult.OK )
            {
              int idx = listBox_attachments.Items.IndexOf(openFileDialog_Attachment.FileName);
              if ( idx < 0 ){
                listBox_attachments.Items.Add( openFileDialog_Attachment.FileName );
                if ( listBox_attachments.SelectedIndex < 0 )
                  listBox_attachments.SelectedIndex = listBox_attachments.Items.Count-1;

                button_attach_del.Enabled =  true;
              }
 			}
        }

        private void button_attach_del_Click(object sender, System.EventArgs e)
        {
          int idx = listBox_attachments.SelectedIndex;
          if ( idx >= 0 ) {
            listBox_attachments.Items.RemoveAt( idx );
            if ( ( idx > 0 ) && ( idx < listBox_attachments.Items.Count ) )
              listBox_attachments.SelectedIndex = idx;
            else if ( listBox_attachments.Items.Count > 0 )
              listBox_attachments.SelectedIndex = idx -1 ;

            button_attach_del.Enabled =  listBox_attachments.Items.Count > 0;
          }
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
          textBox_subject.Text = SBMIME.Unit.cXMailerDefaultFieldValue;
        }

		private void checkBox_sign_clear_format_CheckedChanged(object sender, System.EventArgs e)
		{
		
		}

		private void label_sign_options_Click(object sender, System.EventArgs e)
		{
		
		}

		private void checkBox_sign_CheckedChanged(object sender, System.EventArgs e)
		{
		
		}

		private void checkBox_SignRootHeader_CheckedChanged(object sender, System.EventArgs e)
		{
		
		}

		private void groupBox_Security_Enter(object sender, System.EventArgs e)
		{
		
		}

		private void checkBox_encode_CheckedChanged(object sender, System.EventArgs e)
		{
		
		}

		private void label_encode_options_Click(object sender, System.EventArgs e)
		{
		
		}

		private void rbDoNotUseSecurity_CheckedChanged(object sender, System.EventArgs e)
		{
			if (rbDoNotUseSecurity.Checked) 
			{
				this.groupBox_pgpMime.Enabled = false;
				this.groupBox_smime.Enabled = false;
				this.label_encode_options.Enabled = false;
				this.label_sign_options.Enabled = false;
				this.checkBox_encode.Enabled = false;
				this.checkBox_sign.Enabled = false;
				this.checkBox_sign_clear_format.Enabled = false;
				this.checkBox_SignRootHeader.Enabled = false;
			}
		}

		private void rbUseSMIME_CheckedChanged(object sender, System.EventArgs e)
		{
			if (rbUseSMIME.Checked) 
			{
				this.groupBox_pgpMime.Enabled = false;
				this.groupBox_smime.Enabled = true;
				this.label_encode_options.Enabled = true;
				this.label_sign_options.Enabled = true;
				this.checkBox_encode.Enabled = true;
				this.checkBox_sign.Enabled = true;
				this.checkBox_sign_clear_format.Enabled = true;
				this.checkBox_SignRootHeader.Enabled = true;
			}
		}

		private void rbUsePGPMIME_CheckedChanged(object sender, System.EventArgs e)
		{
			if (rbUsePGPMIME.Checked) 
			{
				this.groupBox_pgpMime.Enabled = true;
				this.groupBox_smime.Enabled = false;
				this.label_encode_options.Enabled = true;
				this.label_sign_options.Enabled = true;
				this.checkBox_encode.Enabled = true;
				this.checkBox_sign.Enabled = true;
				this.checkBox_sign_clear_format.Enabled = true;
				this.checkBox_SignRootHeader.Enabled = true;
			}
		}

		private void button_load_recipient_certificate_Click(object sender, System.EventArgs e)
		{
			if ( openFileDialog_certificate.ShowDialog() == DialogResult.OK )
			{
				frmRequestPassword dlg = new frmRequestPassword();
				if (dlg.ShowDialog(this) == DialogResult.OK) 
				{
					RecipientCertStorage.Clear();
					Boolean bOK = ( SBSMIMECore.Unit.AddToStorageCertificateFromFile(
						// Global SMIME Storage
						RecipientCertStorage,
						// file certificate:
						openFileDialog_certificate.FileName,
						// certificate password:
						dlg.tbPassword.Text) );
					if ( bOK )
					{
						TElX509Certificate cer = (TElX509Certificate)
							RecipientCertStorage.CertificateList[ RecipientCertStorage.Count-1 ];
						ArrayList arList = new ArrayList();
						cer.SubjectRDN.GetValuesByOID(SBUtils.Unit.SB_CERT_OID_EMAIL, arList);
						bOK = arList.Count > 0;
						if ( bOK )
						{
							// Convert UTF8 to unicode string:
							string wsFrom = System.Text.Encoding.UTF8.GetString( (byte []) arList[0] );
							bOK = wsFrom.Length > 0;
							if ( bOK )
							{
								textBox_recipient_certificate.Text = openFileDialog_certificate.FileName;
								textBox_to.Text = wsFrom;
							}
						}
					}
					if ( ! bOK ) 
					{
						throw new Exception("Cannot load certificate or certificate is bad");
					}
				}
			}//of : OpenFile
	
		}

		private void button_load_pubring_Click(object sender, System.EventArgs e)
		{
			if (openKeyringDialog.ShowDialog() == DialogResult.OK) 
			{
				try 
				{
					EncryptingKeys.Load("", openKeyringDialog.FileName, true);
				} 
				catch(System.Exception ex) 
				{
					MessageBox.Show("Unable to load keyring: " + ex.Message);
					return;
				}
				if (EncryptingKeys.PublicCount < 1) 
				{
					MessageBox.Show("Public key(s) not found");
					return;
				} 
				else 
				{
					if (EncryptingKeys.get_PublicKeys(0).UserIDCount > 0) 
					{
						textBox_to.Text = EncryptingKeys.get_PublicKeys(0).get_UserIDs(0).Name;
					}
					textBox_PubKeyring.Text = openKeyringDialog.FileName;
				}
			}
		}

		private void button_load_secring_Click(object sender, System.EventArgs e)
		{
			if (openKeyringDialog.ShowDialog() == DialogResult.OK) 
			{
				try 
				{
					SigningKeys.Load(openKeyringDialog.FileName, "", true);
				} 
				catch(System.Exception ex) 
				{
                    MessageBox.Show("Unable to load keyring: " + ex.Message);
					return;
				}
				if (SigningKeys.SecretCount < 1) 
				{
					MessageBox.Show("Secret key(s) not found");
					return;
				} 
				else 
				{
					if (SigningKeys.get_SecretKeys(0).PublicKey.UserIDCount > 0) 
					{
						textBox_from.Text = SigningKeys.get_SecretKeys(0).PublicKey.get_UserIDs(0).Name;
					}
					textBox_SecKeyring.Text = openKeyringDialog.FileName;
				}
			}
		}
	}
}
