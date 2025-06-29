using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.IO;
using System.Threading;

using SBUtils;
using SBRDN;
using SBCustomCertStorage;
using SBX509;
using SBX509Ext;
using SBConstants;
using SBMessages;

using SBMIME;
using SBSMIMECore;
using SBPGPKeys;
using SBPGPConstants;
using SBPGPUtils;
using SBPGPMIME;


namespace SecureMail
{
	/// <summary>
	/// Summary description for SecureMailWin.
	/// </summary>
	public class SecureMailWin : System.Windows.Forms.Form
	{
		private const String cDemoVersion = "2005.04.18";
		private const String cXMailerDemoFieldValue = "EldoS ElMime Demos, version: " + cDemoVersion + " ( " + SBMIME.Unit.cXMailerDefaultFieldValue + " )";

		private const String sLF = "\r\n";
		private const System.Int16 ACTION_UNKNOWN			= 0;
		private const System.Int16 ACTION_SMIME_ENCRYPT		= 1;
		private const System.Int16 ACTION_SMIME_SIGN		= 2;
		private const System.Int16 ACTION_SMIME_DECRYPT		= 3;
		private const System.Int16 ACTION_SMIME_VERIFY		= 4;
		private const System.Int16 ACTION_PGPMIME_ENCRYPT	= 5;
		private const System.Int16 ACTION_PGPMIME_SIGN      = 6;
		private const System.Int16 ACTION_PGPMIME_DECRYPT   = 7;
		private const System.Int16 ACTION_PGPMIME_VERIFY    = 8;

		private const System.Int16 PAGE_DEFAULT             = 0;
		private const System.Int16 PAGE_SELECT_ACTION       = 1;
		private const System.Int16 PAGE_SELECT_FILES        = 2;
		private const System.Int16 PAGE_SELECT_CERTIFICATES = 3;
		private const System.Int16 PAGE_SELECT_ALGORITHM    = 4;
		private const System.Int16 PAGE_SELECT_KEYS         = 5;
		private const System.Int16 PAGE_CHECK_DATA          = 6;
		private const System.Int16 PAGE_PROCESS             = 7;

		private System.Int16 Action;
		private System.Int16 CurrentPage;
		private SBCustomCertStorage.TElMemoryCertStorage MemoryCertStorage;
		private SBPGPKeys.TElPGPKeyring Keyring;

		private SBPGPKeys.TElPGPKeyring SecretRing;
		private SBPGPKeys.TElPGPKeyring PublicRing;

		private System.Windows.Forms.PictureBox imgLogo;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnBack;
		private System.Windows.Forms.Label lbPGPMime;
		private System.Windows.Forms.Label lbSMime;
		private System.Windows.Forms.RadioButton rbSMimeVerify;
		private System.Windows.Forms.RadioButton rbSMimeDecrypt;
		private System.Windows.Forms.RadioButton rbSMimeSign;
		private System.Windows.Forms.RadioButton rbSMimeEncrypt;
		private System.Windows.Forms.Label lbActionToPerform;
		private System.Windows.Forms.TabPage tsSelectAction;
		private System.Windows.Forms.RadioButton rbPGPMimeVerify;
		private System.Windows.Forms.RadioButton rbPGPMimeDecrypt;
		private System.Windows.Forms.RadioButton rbPGPMimeSign;
		private System.Windows.Forms.RadioButton rbPGPMimeEncrypt;
		private System.Windows.Forms.GroupBox Bavel;
		private System.Windows.Forms.TabControl PageControl;
		private System.Windows.Forms.TabPage tsSelectCertificates;
		private System.Windows.Forms.TabPage tsSelectFiles;
		private System.Windows.Forms.TabPage tsSelectKey;
		private System.Windows.Forms.TabPage tsSelectKeys;
		private System.Windows.Forms.TabPage tsSignAlgorithm;
		private System.Windows.Forms.TabPage tsResult;
		private System.Windows.Forms.TabPage tsCheckData;
		private System.Windows.Forms.Label lbResult;
		private System.Windows.Forms.TextBox mmResult;
		private System.Windows.Forms.Label lbChooseAgorithm;
		private System.Windows.Forms.RadioButton rbDES;
		private System.Windows.Forms.RadioButton rbTripleDES;
		private System.Windows.Forms.RadioButton rbRC2;
		private System.Windows.Forms.RadioButton rbRC4_40;
		private System.Windows.Forms.RadioButton rbRC4_128;
		private System.Windows.Forms.RadioButton rbAES_128;
		private System.Windows.Forms.RadioButton rbAES_192;
		private System.Windows.Forms.RadioButton rbAES_256;
		private System.Windows.Forms.TabPage tsAlgorithm;
		private System.Windows.Forms.Label lbInfo;
		private System.Windows.Forms.TextBox mmInfo;
		private System.Windows.Forms.Button btnDoIt;
		private System.Windows.Forms.Label lbSelectCertificates;
		private System.Windows.Forms.ListBox lbxCertificates;
		private System.Windows.Forms.Button btnAddCertificate;
		private System.Windows.Forms.Button btnRemoveCertificate;
		private System.Windows.Forms.Label lbSelectFiles;
		private System.Windows.Forms.Label lbInputFile;
		private System.Windows.Forms.TextBox edInputFile;
		private System.Windows.Forms.Button sbInputFile;
		private System.Windows.Forms.Button sbOutputFile;
		private System.Windows.Forms.TextBox edOutputFile;
		private System.Windows.Forms.Label lbOutputFile;
		private System.Windows.Forms.Label lbSelectKey;
		private System.Windows.Forms.Label lbKeyring;
		private System.Windows.Forms.Button sbKeyring;
		private System.Windows.Forms.TextBox edKeyring;
		private System.Windows.Forms.Label lbChooseSignAlgorithm;
		private System.Windows.Forms.RadioButton rbMD5;
		private System.Windows.Forms.RadioButton rbSHA1;
		private System.Windows.Forms.Button btnRemoveKey;
		private System.Windows.Forms.Button btnAddKey;
		private System.Windows.Forms.Label lbSelectKeys;
		private System.Windows.Forms.TreeView tvKeys;
		private System.Windows.Forms.OpenFileDialog OpenDlg;
		private System.Windows.Forms.SaveFileDialog SaveDlg;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SecureMailWin()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Init();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		static SecureMailWin()
		{	
			// MIME Box Library Initialization: 
			SBMIME.Unit.Initialize();
			SBSMIMECore.Unit.Initialize();
			SBPGPMIME.Unit.Initialize();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SecureMailWin));
			this.imgLogo = new System.Windows.Forms.PictureBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnNext = new System.Windows.Forms.Button();
			this.btnBack = new System.Windows.Forms.Button();
			this.Bavel = new System.Windows.Forms.GroupBox();
			this.PageControl = new System.Windows.Forms.TabControl();
			this.tsSelectAction = new System.Windows.Forms.TabPage();
			this.rbPGPMimeVerify = new System.Windows.Forms.RadioButton();
			this.rbPGPMimeDecrypt = new System.Windows.Forms.RadioButton();
			this.rbPGPMimeSign = new System.Windows.Forms.RadioButton();
			this.rbPGPMimeEncrypt = new System.Windows.Forms.RadioButton();
			this.lbPGPMime = new System.Windows.Forms.Label();
			this.lbSMime = new System.Windows.Forms.Label();
			this.rbSMimeVerify = new System.Windows.Forms.RadioButton();
			this.rbSMimeDecrypt = new System.Windows.Forms.RadioButton();
			this.rbSMimeSign = new System.Windows.Forms.RadioButton();
			this.rbSMimeEncrypt = new System.Windows.Forms.RadioButton();
			this.lbActionToPerform = new System.Windows.Forms.Label();
			this.tsSelectCertificates = new System.Windows.Forms.TabPage();
			this.btnRemoveCertificate = new System.Windows.Forms.Button();
			this.btnAddCertificate = new System.Windows.Forms.Button();
			this.lbxCertificates = new System.Windows.Forms.ListBox();
			this.lbSelectCertificates = new System.Windows.Forms.Label();
			this.tsSelectFiles = new System.Windows.Forms.TabPage();
			this.sbOutputFile = new System.Windows.Forms.Button();
			this.edOutputFile = new System.Windows.Forms.TextBox();
			this.lbOutputFile = new System.Windows.Forms.Label();
			this.sbInputFile = new System.Windows.Forms.Button();
			this.edInputFile = new System.Windows.Forms.TextBox();
			this.lbInputFile = new System.Windows.Forms.Label();
			this.lbSelectFiles = new System.Windows.Forms.Label();
			this.tsSelectKey = new System.Windows.Forms.TabPage();
			this.sbKeyring = new System.Windows.Forms.Button();
			this.edKeyring = new System.Windows.Forms.TextBox();
			this.lbKeyring = new System.Windows.Forms.Label();
			this.lbSelectKey = new System.Windows.Forms.Label();
			this.tsSignAlgorithm = new System.Windows.Forms.TabPage();
			this.rbSHA1 = new System.Windows.Forms.RadioButton();
			this.rbMD5 = new System.Windows.Forms.RadioButton();
			this.lbChooseSignAlgorithm = new System.Windows.Forms.Label();
			this.tsSelectKeys = new System.Windows.Forms.TabPage();
			this.tvKeys = new System.Windows.Forms.TreeView();
			this.btnRemoveKey = new System.Windows.Forms.Button();
			this.btnAddKey = new System.Windows.Forms.Button();
			this.lbSelectKeys = new System.Windows.Forms.Label();
			this.tsAlgorithm = new System.Windows.Forms.TabPage();
			this.rbAES_256 = new System.Windows.Forms.RadioButton();
			this.rbAES_192 = new System.Windows.Forms.RadioButton();
			this.rbAES_128 = new System.Windows.Forms.RadioButton();
			this.rbRC4_128 = new System.Windows.Forms.RadioButton();
			this.rbRC4_40 = new System.Windows.Forms.RadioButton();
			this.rbRC2 = new System.Windows.Forms.RadioButton();
			this.rbTripleDES = new System.Windows.Forms.RadioButton();
			this.rbDES = new System.Windows.Forms.RadioButton();
			this.lbChooseAgorithm = new System.Windows.Forms.Label();
			this.tsResult = new System.Windows.Forms.TabPage();
			this.mmResult = new System.Windows.Forms.TextBox();
			this.lbResult = new System.Windows.Forms.Label();
			this.tsCheckData = new System.Windows.Forms.TabPage();
			this.btnDoIt = new System.Windows.Forms.Button();
			this.mmInfo = new System.Windows.Forms.TextBox();
			this.lbInfo = new System.Windows.Forms.Label();
			this.OpenDlg = new System.Windows.Forms.OpenFileDialog();
			this.SaveDlg = new System.Windows.Forms.SaveFileDialog();
			this.PageControl.SuspendLayout();
			this.tsSelectAction.SuspendLayout();
			this.tsSelectCertificates.SuspendLayout();
			this.tsSelectFiles.SuspendLayout();
			this.tsSelectKey.SuspendLayout();
			this.tsSignAlgorithm.SuspendLayout();
			this.tsSelectKeys.SuspendLayout();
			this.tsAlgorithm.SuspendLayout();
			this.tsResult.SuspendLayout();
			this.tsCheckData.SuspendLayout();
			this.SuspendLayout();
			// 
			// imgLogo
			// 
			this.imgLogo.BackColor = System.Drawing.Color.MidnightBlue;
			this.imgLogo.Image = ((System.Drawing.Image)(resources.GetObject("imgLogo.Image")));
			this.imgLogo.Location = new System.Drawing.Point(0, 0);
			this.imgLogo.Name = "imgLogo";
			this.imgLogo.Size = new System.Drawing.Size(129, 318);
			this.imgLogo.TabIndex = 7;
			this.imgLogo.TabStop = false;
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(352, 332);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 25);
			this.btnCancel.TabIndex = 16;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnNext
			// 
			this.btnNext.Location = new System.Drawing.Point(256, 332);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(75, 25);
			this.btnNext.TabIndex = 15;
			this.btnNext.Text = "Next >";
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnBack
			// 
			this.btnBack.Enabled = false;
			this.btnBack.Location = new System.Drawing.Point(176, 332);
			this.btnBack.Name = "btnBack";
			this.btnBack.Size = new System.Drawing.Size(75, 25);
			this.btnBack.TabIndex = 14;
			this.btnBack.Text = "< Back";
			this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
			// 
			// Bavel
			// 
			this.Bavel.ForeColor = System.Drawing.SystemColors.Control;
			this.Bavel.Location = new System.Drawing.Point(0, 318);
			this.Bavel.Name = "Bavel";
			this.Bavel.Size = new System.Drawing.Size(440, 3);
			this.Bavel.TabIndex = 12;
			this.Bavel.TabStop = false;
			// 
			// PageControl
			// 
			this.PageControl.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			this.PageControl.Controls.Add(this.tsSelectAction);
			this.PageControl.Controls.Add(this.tsSelectCertificates);
			this.PageControl.Controls.Add(this.tsSelectFiles);
			this.PageControl.Controls.Add(this.tsSelectKey);
			this.PageControl.Controls.Add(this.tsSignAlgorithm);
			this.PageControl.Controls.Add(this.tsSelectKeys);
			this.PageControl.Controls.Add(this.tsAlgorithm);
			this.PageControl.Controls.Add(this.tsResult);
			this.PageControl.Controls.Add(this.tsCheckData);
			this.PageControl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.PageControl.ItemSize = new System.Drawing.Size(0, 1);
			this.PageControl.Location = new System.Drawing.Point(129, 0);
			this.PageControl.Name = "PageControl";
			this.PageControl.SelectedIndex = 0;
			this.PageControl.Size = new System.Drawing.Size(308, 318);
			this.PageControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.PageControl.TabIndex = 11;
			// 
			// tsSelectAction
			// 
			this.tsSelectAction.Controls.Add(this.rbPGPMimeVerify);
			this.tsSelectAction.Controls.Add(this.rbPGPMimeDecrypt);
			this.tsSelectAction.Controls.Add(this.rbPGPMimeSign);
			this.tsSelectAction.Controls.Add(this.rbPGPMimeEncrypt);
			this.tsSelectAction.Controls.Add(this.lbPGPMime);
			this.tsSelectAction.Controls.Add(this.lbSMime);
			this.tsSelectAction.Controls.Add(this.rbSMimeVerify);
			this.tsSelectAction.Controls.Add(this.rbSMimeDecrypt);
			this.tsSelectAction.Controls.Add(this.rbSMimeSign);
			this.tsSelectAction.Controls.Add(this.rbSMimeEncrypt);
			this.tsSelectAction.Controls.Add(this.lbActionToPerform);
			this.tsSelectAction.Location = new System.Drawing.Point(4, 5);
			this.tsSelectAction.Name = "tsSelectAction";
			this.tsSelectAction.Size = new System.Drawing.Size(300, 309);
			this.tsSelectAction.TabIndex = 0;
			// 
			// rbPGPMimeVerify
			// 
			this.rbPGPMimeVerify.Location = new System.Drawing.Point(80, 272);
			this.rbPGPMimeVerify.Name = "rbPGPMimeVerify";
			this.rbPGPMimeVerify.Size = new System.Drawing.Size(152, 17);
			this.rbPGPMimeVerify.TabIndex = 21;
			this.rbPGPMimeVerify.Text = "Verify digital signature";
			// 
			// rbPGPMimeDecrypt
			// 
			this.rbPGPMimeDecrypt.Location = new System.Drawing.Point(80, 248);
			this.rbPGPMimeDecrypt.Name = "rbPGPMimeDecrypt";
			this.rbPGPMimeDecrypt.Size = new System.Drawing.Size(152, 17);
			this.rbPGPMimeDecrypt.TabIndex = 20;
			this.rbPGPMimeDecrypt.Text = "Decrypt message";
			// 
			// rbPGPMimeSign
			// 
			this.rbPGPMimeSign.Checked = true;
			this.rbPGPMimeSign.Location = new System.Drawing.Point(80, 200);
			this.rbPGPMimeSign.Name = "rbPGPMimeSign";
			this.rbPGPMimeSign.Size = new System.Drawing.Size(152, 17);
			this.rbPGPMimeSign.TabIndex = 19;
			this.rbPGPMimeSign.TabStop = true;
			this.rbPGPMimeSign.Text = "Digitally sign message";
			// 
			// rbPGPMimeEncrypt
			// 
			this.rbPGPMimeEncrypt.Location = new System.Drawing.Point(80, 224);
			this.rbPGPMimeEncrypt.Name = "rbPGPMimeEncrypt";
			this.rbPGPMimeEncrypt.Size = new System.Drawing.Size(152, 17);
			this.rbPGPMimeEncrypt.TabIndex = 18;
			this.rbPGPMimeEncrypt.Text = "Encrypt message";
			// 
			// lbPGPMime
			// 
			this.lbPGPMime.Location = new System.Drawing.Point(48, 176);
			this.lbPGPMime.Name = "lbPGPMime";
			this.lbPGPMime.Size = new System.Drawing.Size(160, 13);
			this.lbPGPMime.TabIndex = 17;
			this.lbPGPMime.Text = "Use PGP/Mime security:";
			// 
			// lbSMime
			// 
			this.lbSMime.Location = new System.Drawing.Point(48, 48);
			this.lbSMime.Name = "lbSMime";
			this.lbSMime.Size = new System.Drawing.Size(160, 13);
			this.lbSMime.TabIndex = 16;
			this.lbSMime.Text = "Use S/Mime security:";
			// 
			// rbSMimeVerify
			// 
			this.rbSMimeVerify.Location = new System.Drawing.Point(80, 144);
			this.rbSMimeVerify.Name = "rbSMimeVerify";
			this.rbSMimeVerify.Size = new System.Drawing.Size(152, 17);
			this.rbSMimeVerify.TabIndex = 15;
			this.rbSMimeVerify.Text = "Verify digital signature";
			// 
			// rbSMimeDecrypt
			// 
			this.rbSMimeDecrypt.Location = new System.Drawing.Point(80, 120);
			this.rbSMimeDecrypt.Name = "rbSMimeDecrypt";
			this.rbSMimeDecrypt.Size = new System.Drawing.Size(152, 17);
			this.rbSMimeDecrypt.TabIndex = 14;
			this.rbSMimeDecrypt.Text = "Decrypt message";
			// 
			// rbSMimeSign
			// 
			this.rbSMimeSign.Checked = true;
			this.rbSMimeSign.Location = new System.Drawing.Point(80, 72);
			this.rbSMimeSign.Name = "rbSMimeSign";
			this.rbSMimeSign.Size = new System.Drawing.Size(152, 17);
			this.rbSMimeSign.TabIndex = 13;
			this.rbSMimeSign.TabStop = true;
			this.rbSMimeSign.Text = "Digitally sign message";
			// 
			// rbSMimeEncrypt
			// 
			this.rbSMimeEncrypt.Location = new System.Drawing.Point(80, 96);
			this.rbSMimeEncrypt.Name = "rbSMimeEncrypt";
			this.rbSMimeEncrypt.Size = new System.Drawing.Size(152, 17);
			this.rbSMimeEncrypt.TabIndex = 12;
			this.rbSMimeEncrypt.Text = "Encrypt message";
			// 
			// lbActionToPerform
			// 
			this.lbActionToPerform.Location = new System.Drawing.Point(32, 16);
			this.lbActionToPerform.Name = "lbActionToPerform";
			this.lbActionToPerform.Size = new System.Drawing.Size(264, 13);
			this.lbActionToPerform.TabIndex = 11;
			this.lbActionToPerform.Text = "What kind of action would you like to perform?";
			this.lbActionToPerform.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tsSelectCertificates
			// 
			this.tsSelectCertificates.Controls.Add(this.btnRemoveCertificate);
			this.tsSelectCertificates.Controls.Add(this.btnAddCertificate);
			this.tsSelectCertificates.Controls.Add(this.lbxCertificates);
			this.tsSelectCertificates.Controls.Add(this.lbSelectCertificates);
			this.tsSelectCertificates.Location = new System.Drawing.Point(4, 5);
			this.tsSelectCertificates.Name = "tsSelectCertificates";
			this.tsSelectCertificates.Size = new System.Drawing.Size(300, 309);
			this.tsSelectCertificates.TabIndex = 1;
			// 
			// btnRemoveCertificate
			// 
			this.btnRemoveCertificate.Location = new System.Drawing.Point(136, 200);
			this.btnRemoveCertificate.Name = "btnRemoveCertificate";
			this.btnRemoveCertificate.Size = new System.Drawing.Size(115, 25);
			this.btnRemoveCertificate.TabIndex = 3;
			this.btnRemoveCertificate.Text = "Remove certificate";
			this.btnRemoveCertificate.Click += new System.EventHandler(this.btnRemoveCertificate_Click);
			// 
			// btnAddCertificate
			// 
			this.btnAddCertificate.Location = new System.Drawing.Point(32, 200);
			this.btnAddCertificate.Name = "btnAddCertificate";
			this.btnAddCertificate.Size = new System.Drawing.Size(97, 25);
			this.btnAddCertificate.TabIndex = 2;
			this.btnAddCertificate.Text = "Add certificate";
			this.btnAddCertificate.Click += new System.EventHandler(this.btnAddCertificate_Click);
			// 
			// lbxCertificates
			// 
			this.lbxCertificates.Location = new System.Drawing.Point(32, 80);
			this.lbxCertificates.Name = "lbxCertificates";
			this.lbxCertificates.Size = new System.Drawing.Size(240, 108);
			this.lbxCertificates.TabIndex = 1;
			// 
			// lbSelectCertificates
			// 
			this.lbSelectCertificates.Location = new System.Drawing.Point(32, 8);
			this.lbSelectCertificates.Name = "lbSelectCertificates";
			this.lbSelectCertificates.Size = new System.Drawing.Size(240, 65);
			this.lbSelectCertificates.TabIndex = 0;
			this.lbSelectCertificates.Text = "Please choose certificates, which may be used to decrypt encrypted message";
			// 
			// tsSelectFiles
			// 
			this.tsSelectFiles.Controls.Add(this.sbOutputFile);
			this.tsSelectFiles.Controls.Add(this.edOutputFile);
			this.tsSelectFiles.Controls.Add(this.lbOutputFile);
			this.tsSelectFiles.Controls.Add(this.sbInputFile);
			this.tsSelectFiles.Controls.Add(this.edInputFile);
			this.tsSelectFiles.Controls.Add(this.lbInputFile);
			this.tsSelectFiles.Controls.Add(this.lbSelectFiles);
			this.tsSelectFiles.Location = new System.Drawing.Point(4, 5);
			this.tsSelectFiles.Name = "tsSelectFiles";
			this.tsSelectFiles.Size = new System.Drawing.Size(300, 309);
			this.tsSelectFiles.TabIndex = 2;
			// 
			// sbOutputFile
			// 
			this.sbOutputFile.Location = new System.Drawing.Point(248, 144);
			this.sbOutputFile.Name = "sbOutputFile";
			this.sbOutputFile.Size = new System.Drawing.Size(22, 23);
			this.sbOutputFile.TabIndex = 6;
			this.sbOutputFile.Text = "...";
			this.sbOutputFile.Click += new System.EventHandler(this.sbOutputFile_Click);
			// 
			// edOutputFile
			// 
			this.edOutputFile.Location = new System.Drawing.Point(40, 144);
			this.edOutputFile.Name = "edOutputFile";
			this.edOutputFile.Size = new System.Drawing.Size(200, 20);
			this.edOutputFile.TabIndex = 5;
			this.edOutputFile.Text = "";
			// 
			// lbOutputFile
			// 
			this.lbOutputFile.Location = new System.Drawing.Point(40, 128);
			this.lbOutputFile.Name = "lbOutputFile";
			this.lbOutputFile.Size = new System.Drawing.Size(100, 13);
			this.lbOutputFile.TabIndex = 4;
			this.lbOutputFile.Text = "Output file";
			// 
			// sbInputFile
			// 
			this.sbInputFile.Location = new System.Drawing.Point(248, 88);
			this.sbInputFile.Name = "sbInputFile";
			this.sbInputFile.Size = new System.Drawing.Size(22, 23);
			this.sbInputFile.TabIndex = 3;
			this.sbInputFile.Text = "...";
			this.sbInputFile.Click += new System.EventHandler(this.sbInputFile_Click);
			// 
			// edInputFile
			// 
			this.edInputFile.Location = new System.Drawing.Point(40, 88);
			this.edInputFile.Name = "edInputFile";
			this.edInputFile.Size = new System.Drawing.Size(200, 20);
			this.edInputFile.TabIndex = 2;
			this.edInputFile.Text = "";
			// 
			// lbInputFile
			// 
			this.lbInputFile.Location = new System.Drawing.Point(40, 72);
			this.lbInputFile.Name = "lbInputFile";
			this.lbInputFile.Size = new System.Drawing.Size(100, 13);
			this.lbInputFile.TabIndex = 1;
			this.lbInputFile.Text = "Input file";
			// 
			// lbSelectFiles
			// 
			this.lbSelectFiles.Location = new System.Drawing.Point(32, 24);
			this.lbSelectFiles.Name = "lbSelectFiles";
			this.lbSelectFiles.Size = new System.Drawing.Size(220, 26);
			this.lbSelectFiles.TabIndex = 0;
			this.lbSelectFiles.Text = "Please select message file to encrypt and file where to write encrypted data";
			// 
			// tsSelectKey
			// 
			this.tsSelectKey.Controls.Add(this.sbKeyring);
			this.tsSelectKey.Controls.Add(this.edKeyring);
			this.tsSelectKey.Controls.Add(this.lbKeyring);
			this.tsSelectKey.Controls.Add(this.lbSelectKey);
			this.tsSelectKey.Location = new System.Drawing.Point(4, 5);
			this.tsSelectKey.Name = "tsSelectKey";
			this.tsSelectKey.Size = new System.Drawing.Size(300, 309);
			this.tsSelectKey.TabIndex = 3;
			// 
			// sbKeyring
			// 
			this.sbKeyring.Location = new System.Drawing.Point(240, 72);
			this.sbKeyring.Name = "sbKeyring";
			this.sbKeyring.Size = new System.Drawing.Size(22, 23);
			this.sbKeyring.TabIndex = 5;
			this.sbKeyring.Text = "...";
			this.sbKeyring.Click += new System.EventHandler(this.sbKeyring_Click);
			// 
			// edKeyring
			// 
			this.edKeyring.Location = new System.Drawing.Point(32, 72);
			this.edKeyring.Name = "edKeyring";
			this.edKeyring.Size = new System.Drawing.Size(200, 20);
			this.edKeyring.TabIndex = 4;
			this.edKeyring.Text = "";
			// 
			// lbKeyring
			// 
			this.lbKeyring.Location = new System.Drawing.Point(32, 56);
			this.lbKeyring.Name = "lbKeyring";
			this.lbKeyring.Size = new System.Drawing.Size(100, 13);
			this.lbKeyring.TabIndex = 1;
			this.lbKeyring.Text = "Public keyring:";
			// 
			// lbSelectKey
			// 
			this.lbSelectKey.Location = new System.Drawing.Point(32, 8);
			this.lbSelectKey.Name = "lbSelectKey";
			this.lbSelectKey.Size = new System.Drawing.Size(240, 39);
			this.lbSelectKey.TabIndex = 0;
			this.lbSelectKey.Text = "Please choose PGP key, which should be used to encrypt message";
			// 
			// tsSignAlgorithm
			// 
			this.tsSignAlgorithm.Controls.Add(this.rbSHA1);
			this.tsSignAlgorithm.Controls.Add(this.rbMD5);
			this.tsSignAlgorithm.Controls.Add(this.lbChooseSignAlgorithm);
			this.tsSignAlgorithm.Location = new System.Drawing.Point(4, 5);
			this.tsSignAlgorithm.Name = "tsSignAlgorithm";
			this.tsSignAlgorithm.Size = new System.Drawing.Size(300, 309);
			this.tsSignAlgorithm.TabIndex = 5;
			// 
			// rbSHA1
			// 
			this.rbSHA1.Location = new System.Drawing.Point(96, 104);
			this.rbSHA1.Name = "rbSHA1";
			this.rbSHA1.Size = new System.Drawing.Size(104, 17);
			this.rbSHA1.TabIndex = 2;
			this.rbSHA1.Text = "SHA1";
			// 
			// rbMD5
			// 
			this.rbMD5.Checked = true;
			this.rbMD5.Location = new System.Drawing.Point(96, 80);
			this.rbMD5.Name = "rbMD5";
			this.rbMD5.Size = new System.Drawing.Size(104, 17);
			this.rbMD5.TabIndex = 1;
			this.rbMD5.TabStop = true;
			this.rbMD5.Text = "MD5";
			// 
			// lbChooseSignAlgorithm
			// 
			this.lbChooseSignAlgorithm.Location = new System.Drawing.Point(32, 24);
			this.lbChooseSignAlgorithm.Name = "lbChooseSignAlgorithm";
			this.lbChooseSignAlgorithm.Size = new System.Drawing.Size(240, 39);
			this.lbChooseSignAlgorithm.TabIndex = 0;
			this.lbChooseSignAlgorithm.Text = "Please choose hash function which should be used to calculate message digest on i" +
				"nput data";
			// 
			// tsSelectKeys
			// 
			this.tsSelectKeys.Controls.Add(this.tvKeys);
			this.tsSelectKeys.Controls.Add(this.btnRemoveKey);
			this.tsSelectKeys.Controls.Add(this.btnAddKey);
			this.tsSelectKeys.Controls.Add(this.lbSelectKeys);
			this.tsSelectKeys.Location = new System.Drawing.Point(4, 5);
			this.tsSelectKeys.Name = "tsSelectKeys";
			this.tsSelectKeys.Size = new System.Drawing.Size(300, 309);
			this.tsSelectKeys.TabIndex = 4;
			// 
			// tvKeys
			// 
			this.tvKeys.ImageIndex = -1;
			this.tvKeys.Location = new System.Drawing.Point(32, 72);
			this.tvKeys.Name = "tvKeys";
			this.tvKeys.SelectedImageIndex = -1;
			this.tvKeys.Size = new System.Drawing.Size(240, 112);
			this.tvKeys.TabIndex = 8;
			// 
			// btnRemoveKey
			// 
			this.btnRemoveKey.Location = new System.Drawing.Point(136, 192);
			this.btnRemoveKey.Name = "btnRemoveKey";
			this.btnRemoveKey.Size = new System.Drawing.Size(97, 25);
			this.btnRemoveKey.TabIndex = 7;
			this.btnRemoveKey.Text = "Remove Key";
			this.btnRemoveKey.Click += new System.EventHandler(this.btnRemoveKey_Click);
			// 
			// btnAddKey
			// 
			this.btnAddKey.Location = new System.Drawing.Point(32, 192);
			this.btnAddKey.Name = "btnAddKey";
			this.btnAddKey.Size = new System.Drawing.Size(97, 25);
			this.btnAddKey.TabIndex = 6;
			this.btnAddKey.Text = "Add Key";
			this.btnAddKey.Click += new System.EventHandler(this.btnAddKey_Click);
			// 
			// lbSelectKeys
			// 
			this.lbSelectKeys.Location = new System.Drawing.Point(32, 8);
			this.lbSelectKeys.Name = "lbSelectKeys";
			this.lbSelectKeys.Size = new System.Drawing.Size(240, 57);
			this.lbSelectKeys.TabIndex = 4;
			this.lbSelectKeys.Text = "Please choose PGP keys, which may be used to decrypt encrypted message";
			// 
			// tsAlgorithm
			// 
			this.tsAlgorithm.Controls.Add(this.rbAES_256);
			this.tsAlgorithm.Controls.Add(this.rbAES_192);
			this.tsAlgorithm.Controls.Add(this.rbAES_128);
			this.tsAlgorithm.Controls.Add(this.rbRC4_128);
			this.tsAlgorithm.Controls.Add(this.rbRC4_40);
			this.tsAlgorithm.Controls.Add(this.rbRC2);
			this.tsAlgorithm.Controls.Add(this.rbTripleDES);
			this.tsAlgorithm.Controls.Add(this.rbDES);
			this.tsAlgorithm.Controls.Add(this.lbChooseAgorithm);
			this.tsAlgorithm.Location = new System.Drawing.Point(4, 5);
			this.tsAlgorithm.Name = "tsAlgorithm";
			this.tsAlgorithm.Size = new System.Drawing.Size(300, 309);
			this.tsAlgorithm.TabIndex = 6;
			// 
			// rbAES_256
			// 
			this.rbAES_256.Location = new System.Drawing.Point(72, 232);
			this.rbAES_256.Name = "rbAES_256";
			this.rbAES_256.Size = new System.Drawing.Size(130, 17);
			this.rbAES_256.TabIndex = 8;
			this.rbAES_256.Text = "AES (256 bits)";
			// 
			// rbAES_192
			// 
			this.rbAES_192.Location = new System.Drawing.Point(72, 208);
			this.rbAES_192.Name = "rbAES_192";
			this.rbAES_192.Size = new System.Drawing.Size(130, 17);
			this.rbAES_192.TabIndex = 7;
			this.rbAES_192.Text = "AES (192 bits)";
			// 
			// rbAES_128
			// 
			this.rbAES_128.Location = new System.Drawing.Point(72, 184);
			this.rbAES_128.Name = "rbAES_128";
			this.rbAES_128.Size = new System.Drawing.Size(130, 17);
			this.rbAES_128.TabIndex = 6;
			this.rbAES_128.Text = "AES (128 bits)";
			// 
			// rbRC4_128
			// 
			this.rbRC4_128.Location = new System.Drawing.Point(72, 160);
			this.rbRC4_128.Name = "rbRC4_128";
			this.rbRC4_128.Size = new System.Drawing.Size(130, 17);
			this.rbRC4_128.TabIndex = 5;
			this.rbRC4_128.Text = "RC4 (128 bits)";
			// 
			// rbRC4_40
			// 
			this.rbRC4_40.Location = new System.Drawing.Point(72, 136);
			this.rbRC4_40.Name = "rbRC4_40";
			this.rbRC4_40.Size = new System.Drawing.Size(130, 17);
			this.rbRC4_40.TabIndex = 4;
			this.rbRC4_40.Text = "RC4 (40 bits)";
			// 
			// rbRC2
			// 
			this.rbRC2.Location = new System.Drawing.Point(72, 112);
			this.rbRC2.Name = "rbRC2";
			this.rbRC2.Size = new System.Drawing.Size(130, 17);
			this.rbRC2.TabIndex = 3;
			this.rbRC2.Text = "RC2 (128 bits)";
			// 
			// rbTripleDES
			// 
			this.rbTripleDES.Location = new System.Drawing.Point(72, 88);
			this.rbTripleDES.Name = "rbTripleDES";
			this.rbTripleDES.Size = new System.Drawing.Size(130, 17);
			this.rbTripleDES.TabIndex = 2;
			this.rbTripleDES.Text = "Triple DES (168 bits)";
			// 
			// rbDES
			// 
			this.rbDES.Location = new System.Drawing.Point(72, 64);
			this.rbDES.Name = "rbDES";
			this.rbDES.Size = new System.Drawing.Size(130, 17);
			this.rbDES.TabIndex = 1;
			this.rbDES.Text = "DES (56 bits)";
			// 
			// lbChooseAgorithm
			// 
			this.lbChooseAgorithm.Location = new System.Drawing.Point(32, 24);
			this.lbChooseAgorithm.Name = "lbChooseAgorithm";
			this.lbChooseAgorithm.Size = new System.Drawing.Size(256, 26);
			this.lbChooseAgorithm.TabIndex = 0;
			this.lbChooseAgorithm.Text = "Please choose encryption algorithm which should be used to encrypt data";
			// 
			// tsResult
			// 
			this.tsResult.Controls.Add(this.mmResult);
			this.tsResult.Controls.Add(this.lbResult);
			this.tsResult.Location = new System.Drawing.Point(4, 5);
			this.tsResult.Name = "tsResult";
			this.tsResult.Size = new System.Drawing.Size(300, 309);
			this.tsResult.TabIndex = 7;
			// 
			// mmResult
			// 
			this.mmResult.Location = new System.Drawing.Point(32, 56);
			this.mmResult.Multiline = true;
			this.mmResult.Name = "mmResult";
			this.mmResult.Size = new System.Drawing.Size(240, 169);
			this.mmResult.TabIndex = 1;
			this.mmResult.Text = "";
			// 
			// lbResult
			// 
			this.lbResult.Location = new System.Drawing.Point(32, 24);
			this.lbResult.Name = "lbResult";
			this.lbResult.Size = new System.Drawing.Size(100, 13);
			this.lbResult.TabIndex = 0;
			this.lbResult.Text = "Encryption results:";
			// 
			// tsCheckData
			// 
			this.tsCheckData.Controls.Add(this.btnDoIt);
			this.tsCheckData.Controls.Add(this.mmInfo);
			this.tsCheckData.Controls.Add(this.lbInfo);
			this.tsCheckData.Location = new System.Drawing.Point(4, 5);
			this.tsCheckData.Name = "tsCheckData";
			this.tsCheckData.Size = new System.Drawing.Size(300, 309);
			this.tsCheckData.TabIndex = 8;
			// 
			// btnDoIt
			// 
			this.btnDoIt.Location = new System.Drawing.Point(112, 232);
			this.btnDoIt.Name = "btnDoIt";
			this.btnDoIt.Size = new System.Drawing.Size(75, 25);
			this.btnDoIt.TabIndex = 2;
			this.btnDoIt.Text = "Encrypt";
			this.btnDoIt.Click += new System.EventHandler(this.btnDoIt_Click);
			// 
			// mmInfo
			// 
			this.mmInfo.Location = new System.Drawing.Point(32, 64);
			this.mmInfo.Multiline = true;
			this.mmInfo.Name = "mmInfo";
			this.mmInfo.Size = new System.Drawing.Size(240, 145);
			this.mmInfo.TabIndex = 1;
			this.mmInfo.Text = "";
			// 
			// lbInfo
			// 
			this.lbInfo.Location = new System.Drawing.Point(32, 24);
			this.lbInfo.Name = "lbInfo";
			this.lbInfo.Size = new System.Drawing.Size(240, 26);
			this.lbInfo.TabIndex = 0;
			this.lbInfo.Text = "Ready to start encryption. Please check all the parameters to be valid";
			// 
			// SecureMailWin
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(440, 366);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnNext);
			this.Controls.Add(this.btnBack);
			this.Controls.Add(this.Bavel);
			this.Controls.Add(this.imgLogo);
			this.Controls.Add(this.PageControl);
			this.Name = "SecureMailWin";
			this.Text = "Secure Mail";
			this.PageControl.ResumeLayout(false);
			this.tsSelectAction.ResumeLayout(false);
			this.tsSelectCertificates.ResumeLayout(false);
			this.tsSelectFiles.ResumeLayout(false);
			this.tsSelectKey.ResumeLayout(false);
			this.tsSignAlgorithm.ResumeLayout(false);
			this.tsSelectKeys.ResumeLayout(false);
			this.tsAlgorithm.ResumeLayout(false);
			this.tsResult.ResumeLayout(false);
			this.tsCheckData.ResumeLayout(false);
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
			Application.Run(new SecureMailWin());
		}

		private void Init()
		{
			MemoryCertStorage = new SBCustomCertStorage.TElMemoryCertStorage();

			Keyring = new SBPGPKeys.TElPGPKeyring();
			SecretRing = new SBPGPKeys.TElPGPKeyring();
			PublicRing = new SBPGPKeys.TElPGPKeyring();

			SetPage(PAGE_DEFAULT);
		}

		public void SetPage(System.Int16 Page)
		{
			switch (Page) 
			{
				case PAGE_SELECT_ACTION:
				{					
					PageControl.SelectedTab = tsSelectAction;
					break;
				}

				case PAGE_SELECT_FILES:
				{
					switch (Action)
					{
						case ACTION_SMIME_ENCRYPT:
						case ACTION_PGPMIME_ENCRYPT:
							lbSelectFiles.Text = "Please select message file to encrypt and file where to write encrypted data";
							break;

						case ACTION_SMIME_DECRYPT:
						case ACTION_PGPMIME_DECRYPT:
							lbSelectFiles.Text = "Please select input (encrypted) file and file where to write decrypted data";
							break;

						case ACTION_SMIME_SIGN:
						case ACTION_PGPMIME_SIGN:
							lbSelectFiles.Text = "Please select file to sign and file where to write signed data";
							break;

						case ACTION_SMIME_VERIFY:
						case ACTION_PGPMIME_VERIFY:
							lbSelectFiles.Text = "Please select file with a signed message";
							break;
					}

					bool b = ((Action != ACTION_SMIME_VERIFY) && (Action != ACTION_PGPMIME_VERIFY));
					edOutputFile.Visible = b;
					lbOutputFile.Visible = b;
					sbOutputFile.Visible = b;
					PageControl.SelectedTab = tsSelectFiles;
					break;
				}
    
				case PAGE_SELECT_CERTIFICATES:
				{
					switch (Action)
					{
						case ACTION_SMIME_ENCRYPT:
							lbSelectCertificates.Text = "Please choose certificates which should be used to encrypt message";
							break;

						case ACTION_SMIME_DECRYPT:
							lbSelectCertificates.Text = "Please select certificates which may be used to decrypt message. Each certificate should be loaded with corresponding private key";
							break;

						case ACTION_SMIME_SIGN:
							lbSelectCertificates.Text = "Please choose certificates which should be used to sign the file. At least one certificate must be loaded with corresponding private key";
							break;

						case ACTION_SMIME_VERIFY:
							lbSelectCertificates.Text = "Please select certificates which may be used to verify digital signature. Note, that in most cases signer's certificates are included in signed message, so you may leave certificate list empty";
							break;
					}

					PageControl.SelectedTab = tsSelectCertificates;
					break;
				}

				case PAGE_SELECT_ALGORITHM:
				{
					switch (Action)
					{
						case ACTION_SMIME_ENCRYPT:
							PageControl.SelectedTab = tsAlgorithm;
							break;

						case ACTION_SMIME_SIGN:
							PageControl.SelectedTab = tsSignAlgorithm;
							break;
					}
					break;
				}

				case PAGE_SELECT_KEYS:
				{
					switch (Action)
					{
						case ACTION_PGPMIME_ENCRYPT:
							lbSelectKey.Text = "Please choose PGP public key which should be used to encrypt message";
							lbKeyring.Text = "Public keyring:";
							break;

						case ACTION_PGPMIME_DECRYPT:
							lbSelectKeys.Text = "Please select PGP secret keys which may be used to decrypt message";
							break;

						case ACTION_PGPMIME_SIGN:
							lbSelectKey.Text = "Please choose PGP secret key which should be used to sign the file";
							lbKeyring.Text = "Secret keyring:";
							break;

						case ACTION_PGPMIME_VERIFY:
							lbSelectKeys.Text = "Please select PGP public keys which may be used to verify digital signature.";
							break;
					}

					if ((Action == ACTION_PGPMIME_ENCRYPT) ||
						(Action == ACTION_PGPMIME_SIGN))
						PageControl.SelectedTab = tsSelectKey;
					else
						PageControl.SelectedTab = tsSelectKeys;

					break;
				}

				case PAGE_CHECK_DATA:
				{
					StringBuilder sb = new StringBuilder(0);
					
					switch (Action)
					{
						case ACTION_SMIME_ENCRYPT:
						case ACTION_PGPMIME_ENCRYPT:
							lbInfo.Text = "Ready to start encryption. Please check all the parameters to be valid";
							btnDoIt.Text = "Encrypt";

							mmInfo.Clear();
							sb.Length = 0;
							sb.Append("File to encrypt: " + edInputFile.Text + sLF + sLF);
							sb.Append("File to write encrypted data: " + edOutputFile.Text + sLF + sLF);
							if (Action == ACTION_SMIME_ENCRYPT)
							{
								sb.Append("Certificates: " + sLF);
								sb.Append(WriteCertificateInfo(MemoryCertStorage));
								sb.Append("Algorithm: " + GetAlgorithmName() + sLF);
							}
							else
							{
								sb.Append("Keys: " + sLF);
								sb.Append(WriteKeyringInfo(PublicRing));
							}

							mmInfo.Text = sb.ToString();
							break;

						case ACTION_SMIME_SIGN:
						case ACTION_PGPMIME_SIGN:
							lbInfo.Text = "Ready to start signing. Please check that all signing options are correct.";
							btnDoIt.Text = "Sign";

							mmInfo.Clear();
							sb.Length = 0;
							sb.Append("File to sign: " + edInputFile.Text + sLF + sLF);
							sb.Append("File to write signed data: " + edOutputFile.Text + sLF + sLF);
							if (Action == ACTION_SMIME_SIGN)
							{
								sb.Append("Certificates: " + sLF);
								sb.Append(WriteCertificateInfo(MemoryCertStorage));
								sb.Append("Algorithm: " + GetSignAlgorithm() + sLF);
							}												
							else
							{
								sb.Append("Keys: " + sLF);
								sb.Append(WriteKeyringInfo(SecretRing));
							}
							mmInfo.Text = sb.ToString();
							break;

						case ACTION_SMIME_DECRYPT:
						case ACTION_PGPMIME_DECRYPT:
							lbInfo.Text = "Ready to start decryption. Please check that all decryption options are correct.";
							btnDoIt.Text = "Decrypt";

							mmInfo.Clear();
							sb.Length = 0;
							sb.Append("File to decrypt: " + edInputFile.Text + sLF + sLF);
							sb.Append("File to write decrypted data: " + edOutputFile.Text + sLF + sLF);
							if (Action == ACTION_SMIME_DECRYPT)
							{
								sb.Append("Certificates: " + sLF);
								sb.Append(WriteCertificateInfo(MemoryCertStorage));
							}
							else
							{
								sb.Append("Keys: " + sLF);
								sb.Append(WriteKeyringInfo(Keyring));
							}
							mmInfo.Text = sb.ToString();
							break;

						case ACTION_SMIME_VERIFY:
						case ACTION_PGPMIME_VERIFY:
							lbInfo.Text = "Ready to start verifying. Please check that all options are correct.";
							btnDoIt.Text = "Verify";

							mmInfo.Clear();
							sb.Length = 0;
							sb.Append("File to verify: " + edInputFile.Text + sLF + sLF);
							if (Action == ACTION_SMIME_VERIFY)
							{
								sb.Append("Certificates: " + sLF);
								sb.Append(WriteCertificateInfo(MemoryCertStorage));
							}
							else
							{
								sb.Append("Keys: " + sLF);
								sb.Append(WriteKeyringInfo(Keyring));
							}
							mmInfo.Text = sb.ToString();
							break;
					}

					mmInfo.SelectionLength = 0;
					PageControl.SelectedTab = tsCheckData;
					break;
				}

				case PAGE_PROCESS:
				{
					switch (Action)
					{
						case ACTION_SMIME_ENCRYPT:
						case ACTION_PGPMIME_ENCRYPT:
							lbResult.Text = "Encryption results:";
							break;

						case ACTION_SMIME_SIGN: 
						case ACTION_PGPMIME_SIGN:
							lbResult.Text = "Signing results:";
							break;

						case ACTION_SMIME_DECRYPT:
						case ACTION_PGPMIME_DECRYPT:
							lbResult.Text = "Decryption results:";
							break;

						case ACTION_SMIME_VERIFY:
						case ACTION_PGPMIME_VERIFY:
							lbResult.Text = "Verifying results:";
							break;

						default:
							lbResult.Text = "Results:";
							break;
					}

					mmResult.Text = "Processing...";
					mmResult.SelectionLength = 0;

					PageControl.SelectedTab = tsResult;
					this.Cursor = Cursors.WaitCursor;;
					break;
				}

				default:
					PageControl.SelectedTab = tsSelectAction;
					Page = PAGE_SELECT_ACTION;
					break;
			}

			btnBack.Enabled = (Page != PAGE_SELECT_ACTION) && (Page != PAGE_PROCESS);
			btnNext.Enabled = (Page != PAGE_CHECK_DATA);

			if (Page == PAGE_PROCESS)
			{
				btnNext.Text = "New Task";
				btnCancel.Text = "Finish";
			} 
			else 
			{
				btnNext.Text = "Next >";
				btnCancel.Text = "Cancel";
			}

			CurrentPage = Page;
		}

		private string GetPublicKeyNames(SBPGPKeys.TElPGPPublicKey Key)
		{
			int i;
			if (Key == null)
				return "";

			string Result = "";
			for (i = 0; i < Key.UserIDCount; i++)
				if (Key.get_UserIDs(i).Name != "") 
				{
					if (Result != "")
						Result = Result + ", ";

					Result = Result + Key.get_UserIDs(i).Name;
				}

			return Result;
		}

		private string WriteKeyringInfo(SBPGPKeys.TElPGPKeyring Keyring)
		{
			int i, j;
			string Result = "";

			Result = "Secret Keys:" + sLF;
			for (i = 0; i < Keyring.SecretCount; i++)
			{
				Result = Result + "Key #" + (i + 1).ToString() + ":" + sLF;
				Result = Result + " Names: " + GetPublicKeyNames(Keyring.get_SecretKeys(i).PublicKey) + sLF;
				Result = Result + " KeyID: " + SBPGPUtils.Unit.KeyID2Str(Keyring.get_SecretKeys(i).KeyID(), true) + sLF;
				Result = Result + " KeyFP: " + SBPGPUtils.Unit.KeyFP2Str(Keyring.get_SecretKeys(i).KeyFP()) + sLF;

				if (Keyring.get_SecretKeys(i).SubkeyCount > 0)
				{
					for (j = 0; j < Keyring.get_SecretKeys(i).SubkeyCount; j++)
					{
						Result = Result + " KeyID: " + SBPGPUtils.Unit.KeyID2Str(Keyring.get_SecretKeys(i).get_Subkeys(j).KeyID(), true) + sLF;
						Result = Result + " KeyFP: " + SBPGPUtils.Unit.KeyFP2Str(Keyring.get_SecretKeys(i).get_Subkeys(j).KeyFP()) + sLF;
					}
				}
			}

			Result = Result + sLF + "Public Keys:" + sLF;
			for (i = 0; i < Keyring.PublicCount; i++)
			{
				Result = Result + "Key #" + (i + 1).ToString() + ":" + sLF;
				Result = Result + " Names: " + GetPublicKeyNames(Keyring.get_PublicKeys(i)) + sLF;
				Result = Result + " KeyID: " + SBPGPUtils.Unit.KeyID2Str(Keyring.get_PublicKeys(i).KeyID(), true) + sLF;
				Result = Result + " KeyFP: " + SBPGPUtils.Unit.KeyFP2Str(Keyring.get_PublicKeys(i).KeyFP()) + sLF;

				if (Keyring.get_PublicKeys(i).SubkeyCount > 0)
				{
					for (j = 0; j < Keyring.get_PublicKeys(i).SubkeyCount; j++)
					{
						Result = Result + " KeyID: " + SBPGPUtils.Unit.KeyID2Str(Keyring.get_PublicKeys(i).get_Subkeys(j).KeyID(), true) + sLF;
						Result = Result + " KeyFP: " + SBPGPUtils.Unit.KeyFP2Str(Keyring.get_PublicKeys(i).get_Subkeys(j).KeyFP()) + sLF;
					}
				}
			}

			return Result;
		}

		private string WriteCertificateInfo(SBCustomCertStorage.TElCustomCertStorage Storage)
		{
			SBX509.TElX509Certificate Cert;
			int i, j;
			byte[] Buf;
			int Sz;
			string Result = "";

			for (i = 0; i < Storage.Count; i++)
			{
				Cert = Storage.get_Certificates(i);
				Result = Result + "Certificate #" + (i + 1).ToString() + ":" + sLF;
				Result = Result + "Issuer:" + sLF;
				for (j = 0; j < Cert.IssuerRDN.Count; j++)
					Result = Result + fRDN.GetStringByOID(Cert.IssuerRDN.get_OIDs(j)) + '=' + SBUtils.Unit.UTF8ToStr(Cert.IssuerRDN.get_Values(j)) + sLF;

				Result = Result + "Subject:" + sLF;
				for (j = 0; j < Cert.SubjectRDN.Count; j++)
					Result = Result + fRDN.GetStringByOID(Cert.SubjectRDN.get_OIDs(j)) + '=' + SBUtils.Unit.UTF8ToStr(Cert.SubjectRDN.get_Values(j)) + sLF;

				Sz = 0;
				Buf = null;
				Cert.SaveKeyToBuffer(ref Buf, ref Sz);
				if (Sz > 0)
					Result = Result + "Private key available";
				else
					Result = Result + "Private key is not available";

				Result = Result + sLF + sLF;
			}

			return Result;
		}

		private void Back()
		{
			System.Int16 NewPage;
			if (PageControl.TabIndex < 0)
			{
				SetPage(PAGE_DEFAULT);
				return;
			}

			NewPage = PAGE_DEFAULT;
			switch (CurrentPage)
			{
				case PAGE_SELECT_FILES: 
					NewPage = PAGE_SELECT_ACTION;
					break;

				case PAGE_SELECT_CERTIFICATES: 
					NewPage = PAGE_SELECT_FILES;
					break;

				case PAGE_SELECT_ALGORITHM: 
					NewPage = PAGE_SELECT_CERTIFICATES;
					break;

				case PAGE_SELECT_KEYS: 
					NewPage = PAGE_SELECT_FILES;
					break;

				case PAGE_CHECK_DATA:
				{
					switch (Action)
					{
						case ACTION_SMIME_ENCRYPT:
						case ACTION_SMIME_SIGN:
							NewPage = PAGE_SELECT_ALGORITHM;
							break;

						case ACTION_SMIME_DECRYPT:
						case ACTION_SMIME_VERIFY:
							NewPage = PAGE_SELECT_CERTIFICATES;
							break;

						default:
							NewPage = PAGE_SELECT_KEYS;
							break;
					}

					break;
				}

				case PAGE_PROCESS: 
					NewPage = PAGE_CHECK_DATA;
					break;
			}

			SetPage(NewPage);
		}

		private void Next()
		{
			if (PageControl.TabIndex < 0)
			{
				SetPage(PAGE_DEFAULT);
				return;
			}

			if ((CurrentPage == PAGE_SELECT_ACTION) &&
				((rbSMimeEncrypt.Checked && (Action != ACTION_SMIME_ENCRYPT)) ||
				(rbSMimeDecrypt.Checked && (Action != ACTION_SMIME_DECRYPT)) ||
				(rbSMimeSign.Checked && (Action != ACTION_SMIME_SIGN)) ||
				(rbSMimeVerify.Checked && (Action != ACTION_SMIME_VERIFY)) ||
				(rbPGPMimeEncrypt.Checked && (Action != ACTION_PGPMIME_ENCRYPT)) ||
				(rbPGPMimeDecrypt.Checked && (Action != ACTION_PGPMIME_DECRYPT)) ||
				(rbPGPMimeSign.Checked && (Action != ACTION_PGPMIME_SIGN)) ||
				(rbPGPMimeVerify.Checked && (Action != ACTION_PGPMIME_VERIFY)) ) )
			{
				if (rbSMimeEncrypt.Checked) 
					Action = ACTION_SMIME_ENCRYPT;
				else if (rbSMimeDecrypt.Checked)
					Action = ACTION_SMIME_DECRYPT;
				else if (rbSMimeSign.Checked)
					Action = ACTION_SMIME_SIGN;
				else if (rbSMimeVerify.Checked)
					Action = ACTION_SMIME_VERIFY;
				else if (rbPGPMimeEncrypt.Checked)
					Action = ACTION_PGPMIME_ENCRYPT;
				else if (rbPGPMimeDecrypt.Checked)
					Action = ACTION_PGPMIME_DECRYPT;
				else if (rbPGPMimeSign.Checked)
					Action = ACTION_PGPMIME_SIGN;
				else if (rbPGPMimeVerify.Checked) 
					Action = ACTION_PGPMIME_VERIFY;
				else
					Action = ACTION_UNKNOWN;

				ClearData();
			}

			switch (Action)
			{
				case ACTION_SMIME_ENCRYPT: 
					SMimeEncryptNext();
					break;

				case ACTION_SMIME_DECRYPT: 
					SMimeDecryptNext();
					break;

				case ACTION_SMIME_SIGN: 
					SMimeSignNext(); 
					break;

				case ACTION_SMIME_VERIFY: 
					SMimeVerifyNext();
					break;

				case ACTION_PGPMIME_ENCRYPT: 
					PGPEncryptNext();
					break;

				case ACTION_PGPMIME_DECRYPT:
					PGPDecryptNext();
					break;

				case ACTION_PGPMIME_SIGN: 
					PGPSignNext();
					break;

				case ACTION_PGPMIME_VERIFY: 
					PGPVerifyNext();
					break;

				default:
					SetPage(PAGE_DEFAULT);
					break;
			}
		}


		private void btnNext_Click(object sender, System.EventArgs e)
		{
			Next();
		}

		private void btnBack_Click(object sender, System.EventArgs e)
		{
			Back();
		}
		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void ClearData()
		{
			edInputFile.Text = "";
			edOutputFile.Text = "";

			MemoryCertStorage.Clear();
			lbxCertificates.Items.Clear();

			rbTripleDES.Checked = true;
			rbSHA1.Checked = true;

			PublicRing.Clear();
			SecretRing.Clear();
			Keyring.Clear();
			edKeyring.Text = "";
			tvKeys.Nodes.Clear();

			mmInfo.Clear();
			mmResult.Clear();
		}

		private int GetAlgorithm()
		{
			if (rbDES.Checked)
				return SBConstants.Unit.SB_ALGORITHM_CNT_DES;
			if (rbTripleDES.Checked)
				return SBConstants.Unit.SB_ALGORITHM_CNT_3DES;
			if (rbRC2.Checked)
				return SBConstants.Unit.SB_ALGORITHM_CNT_RC2;
			if (rbRC4_40.Checked || rbRC4_128.Checked)
				return SBConstants.Unit.SB_ALGORITHM_CNT_RC4;
			if (rbAES_128.Checked)
				return SBConstants.Unit.SB_ALGORITHM_CNT_AES128;
			if (rbAES_192.Checked)
				return SBConstants.Unit.SB_ALGORITHM_CNT_AES192;
			if (rbAES_256.Checked)
				return SBConstants.Unit.SB_ALGORITHM_CNT_AES256;
																																																																						
			return SBConstants.Unit.SB_ALGORITHM_CNT_3DES;
		}

		private int GetAlgorithmBitsInKey()
		{
			// this is only for SB_ALGORITHM_CNT_RC2 or SB_ALGORITHM_CNT_RC4
			if (rbRC4_40.Checked)
				return 40;
			
			return 128;
		}

		private string GetAlgorithmName()
		{
			if (rbDES.Checked)
				return rbDES.Text;
			if (rbTripleDES.Checked)
				return rbTripleDES.Text;
			if (rbRC2.Checked)
				return rbRC2.Text;
			if (rbRC4_40.Checked)
				return rbRC4_40.Text;
			if (rbRC4_128.Checked)
				return rbRC4_128.Text;
			if (rbAES_128.Checked)
				return rbAES_128.Text;
			if (rbAES_192.Checked)
				return rbAES_192.Text;
			if (rbAES_256.Checked)
				return rbAES_256.Text;
																																																					   
			return rbTripleDES.Text;
		}

		private string GetSignAlgorithm()
		{
			if (rbMD5.Checked)
				return "MD5";
	
			return "SHA1";
		}

		private string RequestKeyPassphrase(SBPGPKeys.TElPGPCustomSecretKey key, ref bool Cancel) 
		{
			string result;
			frmPassRequest dlg = new frmPassRequest();
			dlg.Init(key);
			if (dlg.ShowDialog() == DialogResult.OK) 
			{
				result = dlg.tbPassphrase.Text;
				Cancel = false;
			} 
			else 
			{
				Cancel = true;
				result = "";
			}

			dlg.Dispose();
			return result;
		}

		private void PGPMIMEKeyPassphrase(object Sender, TElPGPCustomSecretKey Key, ref string Passphrase, ref bool Cancel)
		{
			Passphrase = RequestKeyPassphrase(Key, ref Cancel);
		}

		private string RequestPassphrase()
		{
			StringQueryDlg passwdDlg = new StringQueryDlg(true);
			passwdDlg.Text = "Enter password";
			passwdDlg.Description = "Please, enter passphrase:";
			string sPwd = "";
			if (passwdDlg.ShowDialog(this) == DialogResult.OK)
				sPwd = passwdDlg.TextBox;

			passwdDlg.Dispose();
			return sPwd;
		}

		private void SetResults(string Res)
		{
			Cursor = Cursors.Default;
			if (Res != "")
				mmResult.Text = Res;
			else
				mmResult.Text = "Finished. Unknown status.";

			mmResult.SelectionLength = 0;
		}

		private string PGPDecrypt(string InputFileName, string OutputFileName)
		{
			int Res = 0;
			string Result = "";

			SBMIME.TElMessage Msg = new SBMIME.TElMessage(cXMailerDemoFieldValue);

			FileStream Stream = new FileStream(InputFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			try
			{
				Res = Msg.ParseMessage(Stream, "", "",	SBMIME.Unit.mpoStoreStream | SBMIME.Unit.mpoLoadData | SBMIME.Unit.mpoCalcDataSize,	false, false, false);
			}
			catch (Exception E)
			{
				Result = E.Message;
				Res = SBMIME.Unit.EL_ERROR;
			}
	
			if ((Res == SBMIME.Unit.EL_OK) || (Res == SBMIME.Unit.EL_WARNING))
			{
				if ((Msg.MainPart == null) ||
					(Msg.MainPart.MessagePartHandler == null) ||
					(Msg.MainPart.IsActivatedMessagePartHandler))
				{
					Stream.Close();
					return "Mesage not encoded. No action done.";
				}

				if (Msg.MainPart.MessagePartHandler.IsError)
				{
					Result = Msg.MainPart.MessagePartHandler.ErrorText;
					Res = SBMIME.Unit.EL_ERROR;
				}
				else
				{
					if (Msg.MainPart.MessagePartHandler is TElMessagePartHandlerPGPMime)
					{
						SBPGPMIME.TElMessagePartHandlerPGPMime Handler = (SBPGPMIME.TElMessagePartHandlerPGPMime)Msg.MainPart.MessagePartHandler;
						Handler.DecryptingKeys = Keyring;
						Handler.OnKeyPassphrase += new SBPGPStreams.TSBPGPKeyPassphraseEvent(PGPMIMEKeyPassphrase);
						try
						{
							Res = Handler.Decode(true);
						}
						catch (Exception E)
						{
							Result = E.Message;
							Res = SBMIME.Unit.EL_ERROR;
						}
					}
					else
					{
						Result = "Unknown message handler.";
						Res = SBMIME.Unit.EL_ERROR;
					}
				}
			}
		
			Stream.Close();

			if ((Res != SBMIME.Unit.EL_OK) && (Res != SBMIME.Unit.EL_WARNING))
			{
				if (Result != "") 
					Result = "Message: \"" + Result + "\"";
				else
					if ((Res == SBMIME.Unit.EL_HANDLERR_ERROR) && (Msg.MainPart.MessagePartHandler != null))
					Result = "Message: \"" + Msg.MainPart.MessagePartHandler.ErrorText + "\"";

				Result = String.Format("Error parsing mime message \"{0}\".\r\nElMime error code: {1:D}\r\n{2}", InputFileName, Res, Result);
				return Result;
			}

			SBMIME.TElMessagePart MainPart = Msg.MainPart.MessagePartHandler.DecodedPart;
			Msg.MainPart.MessagePartHandler.DecodedPart = null;
			Msg.SetMainPart(MainPart, false);

			Stream = new FileStream(OutputFileName, FileMode.Create);
			try
			{
				Res = Msg.AssembleMessage(Stream,
					// Charset of message:
					"utf-8",
					// HeaderEncoding
					SBMIME.TElHeaderEncoding.heBase64, //  variants:  he8bit  | heQuotedPrintable  | heBase64
					// BodyEncoding
					"base64", //  variants:   '8bit' | 'quoted-printable' |  'base64'
					// AttachEncoding
					"base64",  //  variants:   '8bit' | 'quoted-printable' |  'base64'
					false
					);
			}
			catch (Exception E)
			{
				Result = E.Message;
				Res = SBMIME.Unit.EL_ERROR;
			}

			if ((Res == SBMIME.Unit.EL_OK) || (Res == SBMIME.Unit.EL_WARNING))
				Result = "Message decrypted and assembled OK";
			else
			{
				if (Result != "")
					Result = "Message: \"" + Result + "\"";
				else
					if ((Res == SBMIME.Unit.EL_HANDLERR_ERROR) && (Msg.MainPart.MessagePartHandler != null))
					Result = "Message: \"" + Msg.MainPart.MessagePartHandler.ErrorText + "\"";

				Result = String.Format("Failed to assemble a message.\r\nElMime error code: {0:D}\r\n{1}", Res, Result);
			}

			Stream.Close();
			return Result;
		}

		private void PGPDecryptNext()
		{
			System.Int16 NextPage = -1;
			switch (CurrentPage)
			{
				case PAGE_SELECT_ACTION:
					NextPage = PAGE_SELECT_FILES;
					break;
				
				case PAGE_SELECT_FILES:
					if ((edInputFile.Text == "") || (edOutputFile.Text == "")) 
						MessageBox.Show("You must select both input and output files", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
					else
						NextPage = PAGE_SELECT_KEYS;
					break;

				case PAGE_SELECT_KEYS:
					if (Keyring.SecretCount == 0)
						MessageBox.Show("No recipient secret keys selected. Please select one.", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
					else
						NextPage = PAGE_CHECK_DATA;
					break;

				case PAGE_CHECK_DATA:
					NextPage = PAGE_PROCESS;
					break;

				default:
					NextPage = PAGE_DEFAULT;
					break;
			}

			if (NextPage >= 0)
				SetPage(NextPage);

			if (NextPage == PAGE_PROCESS)
			{
				Application.DoEvents();
				SetResults( PGPDecrypt(edInputFile.Text, edOutputFile.Text) );
			}
		}

		private string PGPEncrypt(string InputFileName, string OutputFileName)
		{
			int Res = 0;
			string Result = "";

			SBMIME.TElMessage Msg = new SBMIME.TElMessage(cXMailerDemoFieldValue);

			FileStream Stream = new FileStream(InputFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			try
			{
				Res = Msg.ParseMessage(Stream, "", "",	SBMIME.Unit.mpoStoreStream | SBMIME.Unit.mpoLoadData | SBMIME.Unit.mpoCalcDataSize,	false, false, true);
			}
			catch (Exception E)
			{
				Result = E.Message;
				Res = SBMIME.Unit.EL_ERROR;
			}

			Stream.Close();
	
			if ((Res != SBMIME.Unit.EL_OK) && (Res != SBMIME.Unit.EL_WARNING))
			{
				if (Result != "")
					Result = "Message: \"" + Result + "\"";
				else if ((Res == SBMIME.Unit.EL_HANDLERR_ERROR) && (Msg.MainPart.MessagePartHandler != null))
					Result = "Message: \"" + Msg.MainPart.MessagePartHandler.ErrorText + "\"";

				Result = String.Format("Error parsing mime message {0}.\r\nElMime error code: {1:D}\r\n{2}", InputFileName, Res, Result);
				return Result;
			}

			SBPGPMIME.TElMessagePartHandlerPGPMime PGPMime = new SBPGPMIME.TElMessagePartHandlerPGPMime(null);
			Msg.MainPart.MessagePartHandler = PGPMime;

			PGPMime.EncryptingKeys = PublicRing;
			PGPMime.Encrypt = true;

			Stream = new FileStream(OutputFileName, FileMode.Create);
			try
			{
				Res = Msg.AssembleMessage(Stream,
					// Charset of message:
					"utf-8",
					// HeaderEncoding
					SBMIME.TElHeaderEncoding.heBase64, //  variants:  he8bit  | heQuotedPrintable  | heBase64
					// BodyEncoding
					"base64", //  variants:   '8bit' | 'quoted-printable' |  'base64'
					// AttachEncoding
					"base64",  //  variants:   '8bit' | 'quoted-printable' |  'base64'
					false
					);
			}
			catch (Exception E)
			{
				Result = E.Message;
				Res = SBMIME.Unit.EL_ERROR;
			}

			if ((Res == SBMIME.Unit.EL_OK) || (Res == SBMIME.Unit.EL_WARNING))
				Result = "Message encrypted and assembled OK";
			else
			{
				if (Result != "")
					Result = "Message: \"" + Result + "\"";
				else if (Res == SBMIME.Unit.EL_HANDLERR_ERROR)
					Result = "Message: \"" + PGPMime.ErrorText + "\"";

				Result = String.Format("Failed to assemble a message.\r\nElMime error code: {0:D}\r\n{1}", Res, Result);
			}
			Stream.Close();

			return Result;
		}

		private void PGPEncryptNext()
		{
			System.Int16 NextPage = -1;
			switch (CurrentPage)
			{
				case PAGE_SELECT_ACTION:
					NextPage = PAGE_SELECT_FILES;
					break;

				case PAGE_SELECT_FILES:
					if ((edInputFile.Text == "") || (edOutputFile.Text == ""))
						MessageBox.Show("You must select both input and output files", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
					else
						NextPage = PAGE_SELECT_KEYS;
					break;

				case PAGE_SELECT_KEYS:
					if (PublicRing.PublicCount == 0)
						MessageBox.Show("No public key selected. Please select one.", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
					else
						NextPage = PAGE_CHECK_DATA;
					break;

				case PAGE_CHECK_DATA:
					NextPage = PAGE_PROCESS;
					break;

				default:
					NextPage = PAGE_DEFAULT;
					break;
			}

			if (NextPage >= 0)
				SetPage(NextPage);

			if (NextPage == PAGE_PROCESS)
			{
				Application.DoEvents();
				SetResults( PGPEncrypt(edInputFile.Text, edOutputFile.Text) );
			}
		}

		private string PGPSign(string InputFileName, string OutputFileName)
		{
			int Res = 0;
			string Result = "";

			SBMIME.TElMessage Msg = new SBMIME.TElMessage(cXMailerDemoFieldValue);

			FileStream Stream = new FileStream(InputFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			try
			{
				Res = Msg.ParseMessage(Stream, "", "",	SBMIME.Unit.mpoStoreStream | SBMIME.Unit.mpoLoadData | SBMIME.Unit.mpoCalcDataSize,	false, false, true);
			}
			catch (Exception E)
			{
				Result = E.Message;
				Res = SBMIME.Unit.EL_ERROR;
			}

			Stream.Close();

			if ((Res != SBMIME.Unit.EL_OK) && (Res != SBMIME.Unit.EL_WARNING))
			{
				if (Result != "")
					Result = "Message: \"" + Result + "\"";
				else if ((Res == SBMIME.Unit.EL_HANDLERR_ERROR) && (Msg.MainPart.MessagePartHandler != null))
					Result = "Message: \"" + Msg.MainPart.MessagePartHandler.ErrorText + "\"";

				Result = String.Format("Error parsing mime message {0}.\r\nElMime error code: {1:D}\r\n{2}", InputFileName, Res, Result);
				return Result;
			}
			
			SBPGPMIME.TElMessagePartHandlerPGPMime PGPMime = new SBPGPMIME.TElMessagePartHandlerPGPMime(null);
			Msg.MainPart.MessagePartHandler = PGPMime;

			PGPMime.SigningKeys = SecretRing;
			PGPMime.OnKeyPassphrase += new SBPGPStreams.TSBPGPKeyPassphraseEvent(PGPMIMEKeyPassphrase);
			PGPMime.Sign = true;

			Stream = new FileStream(OutputFileName, FileMode.Create);
			try
			{
				Res = Msg.AssembleMessage(Stream,
					// Charset of message:
					"utf-8",
					// HeaderEncoding
					SBMIME.TElHeaderEncoding.heBase64, //  variants:  he8bit  | heQuotedPrintable  | heBase64
					// BodyEncoding
					"base64", //  variants:   '8bit' | 'quoted-printable' |  'base64'
					// AttachEncoding
					"base64",  //  variants:   '8bit' | 'quoted-printable' |  'base64'
					false
					);
			}
			catch (Exception E)
			{
				Result = E.Message;
				Res = SBMIME.Unit.EL_ERROR;
			}

			if ((Res == SBMIME.Unit.EL_OK) || (Res == SBMIME.Unit.EL_WARNING))
				Result = "Message signed and assembled OK";
			else
			{
				if (Result != "")
					Result = "Message: \"" + Result + "\"";
				else if (Res == SBMIME.Unit.EL_HANDLERR_ERROR)
					Result = "Message: \"" + PGPMime.ErrorText + "\"";

				Result = String.Format("Failed to assemble a message.\r\nElMime error code: {0:D}\r\n{1}", Res, Result);
			}

			Stream.Close();
			return Result;
		}	

		private void PGPSignNext()
		{
			System.Int16 NextPage = -1;
			switch (CurrentPage)
			{
				case PAGE_SELECT_ACTION:	
					NextPage = PAGE_SELECT_FILES;
					break;

				case PAGE_SELECT_FILES:
					if ((edInputFile.Text == "") || (edOutputFile.Text == ""))
						MessageBox.Show("You must select both input and output files", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
					else
						NextPage = PAGE_SELECT_KEYS;
					break;

				case PAGE_SELECT_KEYS:
					if (SecretRing.SecretCount == 0)
						MessageBox.Show("No secret key selected. Please select one.", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
					else
						NextPage = PAGE_CHECK_DATA;
					break;

				case PAGE_CHECK_DATA:
					NextPage = PAGE_PROCESS;
					break;

				default:
					NextPage = PAGE_DEFAULT;
					break;
			}

			if (NextPage >= 0)
				SetPage(NextPage);

			if (NextPage == PAGE_PROCESS)
			{
				Application.DoEvents();
				SetResults( PGPSign(edInputFile.Text, edOutputFile.Text) );
			}
		}

		private string PGPVerify(string InputFileName)
		{
			int Res = 0;
			string Result = "";

			SBMIME.TElMessage Msg = new SBMIME.TElMessage(cXMailerDemoFieldValue);

			FileStream Stream = new FileStream(InputFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			try
			{
				Res = Msg.ParseMessage(Stream, "", "",	SBMIME.Unit.mpoStoreStream | SBMIME.Unit.mpoLoadData | SBMIME.Unit.mpoCalcDataSize,	false, false, false);
			}
			catch (Exception E)
			{
				Result = E.Message;
				Res = SBMIME.Unit.EL_ERROR;
			}

			if ((Res == SBMIME.Unit.EL_OK) || (Res == SBMIME.Unit.EL_WARNING))
			{
				if ((Msg.MainPart == null) || (Msg.MainPart.MessagePartHandler == null) ||
					(Msg.MainPart.IsActivatedMessagePartHandler))
				{
					Stream.Close();
					return "Mesage not encoded. No action done.";
				}

				if (Msg.MainPart.MessagePartHandler.IsError)
				{
					Result = Msg.MainPart.MessagePartHandler.ErrorText;
					Res = SBMIME.Unit.EL_ERROR;
				}
				else
				{
					if (Msg.MainPart.MessagePartHandler is SBPGPMIME.TElMessagePartHandlerPGPMime)
					{
						SBPGPMIME.TElMessagePartHandlerPGPMime Handler = (SBPGPMIME.TElMessagePartHandlerPGPMime)Msg.MainPart.MessagePartHandler;
						Handler.VerifyingKeys = Keyring;
						try
						{
							Res = Msg.MainPart.MessagePartHandler.Decode(true);
						}
						catch (Exception E)
						{
							Result = E.Message;
							Res = SBMIME.Unit.EL_ERROR;
						}
					}
					else
					{
						Result = "Unknown message handler.";
						Res = SBMIME.Unit.EL_ERROR;
					}
				}
			}

			Stream.Close();

			if ((Res != SBMIME.Unit.EL_OK) && (Res != SBMIME.Unit.EL_WARNING))
			{
				if (Result != "")
					Result = "Message: \"" + Result + "\"";
				else if ((Res == SBMIME.Unit.EL_HANDLERR_ERROR) && (Msg.MainPart.MessagePartHandler != null))
					Result = "Message: \"" + Msg.MainPart.MessagePartHandler.ErrorText + "\"";

				Result = String.Format("Error parsing mime message {0}.\r\nElMime error code: {1:D}\r\n{2}", InputFileName, Res, Result);
			}
			else
			{
				if ((Res == SBMIME.Unit.EL_WARNING) && (Msg.MainPart.MessagePartHandler != null) &&
					(Msg.MainPart.MessagePartHandler.ErrorText != ""))
				{
					Result = "Message: \"" + Msg.MainPart.MessagePartHandler.ErrorText + "\"";
				}
				else
					Result = "Message verified OK";

				if ((Msg.MainPart.MessagePartHandler != null) &&
					(Msg.MainPart.MessagePartHandler is SBPGPMIME.TElMessagePartHandlerPGPMime))
				{
					Result += "\r\n\r\nSignature verified with:\r\n";
					SBPGPMIME.TElMessagePartHandlerPGPMime Handler = (SBPGPMIME.TElMessagePartHandlerPGPMime)Msg.MainPart.MessagePartHandler;
					Result += WriteKeyringInfo(Handler.VerifyingKeys);
				}
			}
			return Result;
		}

		private void PGPVerifyNext()
		{
			System.Int16 NextPage = -1;
			switch (CurrentPage)
			{
				case PAGE_SELECT_ACTION:	
					NextPage = PAGE_SELECT_FILES;
					break;

				case PAGE_SELECT_FILES:
					if (edInputFile.Text == "")
						MessageBox.Show("You must select input file", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
					else
						NextPage = PAGE_SELECT_KEYS;
					break;

				case PAGE_SELECT_KEYS:
					NextPage = PAGE_CHECK_DATA;
					break;

				case PAGE_CHECK_DATA:
					NextPage = PAGE_PROCESS;
					break;

				default:
					NextPage = PAGE_DEFAULT;
					break;
			}

			if (NextPage >= 0)
				SetPage(NextPage);

			if (NextPage == PAGE_PROCESS)
			{
				Application.DoEvents();
				SetResults( PGPVerify(edInputFile.Text) );
			}
		}
	
		private string SMimeDecrypt(string InputFileName, string OutputFileName)
		{
			int Res = 0;
			string Result = "";

			SBMIME.TElMessage Msg = new SBMIME.TElMessage(cXMailerDemoFieldValue);

			FileStream Stream = new FileStream(InputFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			try
			{
				Res = Msg.ParseMessage(Stream, "", "",	SBMIME.Unit.mpoStoreStream | SBMIME.Unit.mpoLoadData | SBMIME.Unit.mpoCalcDataSize,	false, false, false);
			}
			catch (Exception E)
			{
				Result = E.Message;
				Res = SBMIME.Unit.EL_ERROR;
			}

			if ((Res == SBMIME.Unit.EL_OK) || (Res == SBMIME.Unit.EL_WARNING))
			{
				if ((Msg.MainPart == null) ||
					(Msg.MainPart.MessagePartHandler == null) ||
					(Msg.MainPart.IsActivatedMessagePartHandler))
				{
					Stream.Close();
					return "Mesage not encoded. No action done.";
				}

				if (Msg.MainPart.MessagePartHandler.IsError)
				{
					Result = Msg.MainPart.MessagePartHandler.ErrorText;
					Res = SBMIME.Unit.EL_ERROR;
				}
				else
				{
					if (Msg.MainPart.MessagePartHandler is SBSMIMECore.TElMessagePartHandlerSMime) 
					{
					
						SBSMIMECore.TElMessagePartHandlerSMime SMime = (SBSMIMECore.TElMessagePartHandlerSMime)Msg.MainPart.MessagePartHandler;
						SMime.CertificatesStorage = MemoryCertStorage;

						try
						{
							Res = Msg.MainPart.MessagePartHandler.Decode(true);
						}
						catch (Exception E)
						{
							Result = E.Message;
							Res = SBMIME.Unit.EL_ERROR;
						}
					}
					else
					{
						Result = "Unknown message handler.";
						Res = SBMIME.Unit.EL_ERROR;
					}
				}
			}

			Stream.Close();

			if ((Res != SBMIME.Unit.EL_OK) && (Res != SBMIME.Unit.EL_WARNING))
			{
				if (Result != "")
					Result = "Message: \"" + Result + "\"";
				else if ((Res == SBMIME.Unit.EL_HANDLERR_ERROR) && (Msg.MainPart.MessagePartHandler != null))
					Result = "Message: \"" + Msg.MainPart.MessagePartHandler.ErrorText + "\"";

				Result = String.Format("Error parsing mime message {0}.\r\nElMime error code: {1:D}\r\n{2}", InputFileName, Res, Result);
				return Result;
			}

			SBMIME.TElMessagePart MainPart = Msg.MainPart.MessagePartHandler.DecodedPart;
			Msg.MainPart.MessagePartHandler.DecodedPart = null;
			Msg.SetMainPart(MainPart, false);

			Stream = new FileStream(OutputFileName, FileMode.Create);
			try
			{
				Res = Msg.AssembleMessage(Stream,
					// Charset of message:
					"utf-8",
					// HeaderEncoding
					SBMIME.TElHeaderEncoding.heBase64, //  variants:  he8bit  | heQuotedPrintable  | heBase64
					// BodyEncoding
					"base64", //  variants:   '8bit' | 'quoted-printable' |  'base64'
					// AttachEncoding
					"base64",  //  variants:   '8bit' | 'quoted-printable' |  'base64'
					false
					);
			}
			catch (Exception E)
			{
				Result = E.Message;
				Res = SBMIME.Unit.EL_ERROR;
			}

			if ((Res == SBMIME.Unit.EL_OK) || (Res == SBMIME.Unit.EL_WARNING))
				Result = "Message decrypted and assembled OK";
			else
			{
				if (Result != "")
					Result = "Message: \"" + Result + "\"";
				else
					if ((Res == SBMIME.Unit.EL_HANDLERR_ERROR) && (Msg.MainPart.MessagePartHandler != null))
					Result = "Message: \"" + Msg.MainPart.MessagePartHandler.ErrorText + "\"";

				Result = String.Format("Failed to assemble a message.\r\nElMime error code: {0:D}\r\n{1}", Res, Result);
			}

			Stream.Close();
			return Result;
		}			

		private void SMimeDecryptNext()
		{
			System.Int16 NextPage = -1;
			switch (CurrentPage)
			{
				case PAGE_SELECT_ACTION:
					NextPage = PAGE_SELECT_FILES;
					break;

				case PAGE_SELECT_FILES:
					if ((edInputFile.Text == "") || (edOutputFile.Text == ""))
						MessageBox.Show("You must select both input and output files", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
					else
						NextPage = PAGE_SELECT_CERTIFICATES;
					break;

				case PAGE_SELECT_CERTIFICATES:
					if (MemoryCertStorage.Count == 0) 
						MessageBox.Show("No recipient certificate selected. Please select one.", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
					else
						NextPage = PAGE_CHECK_DATA;
					break;

				case PAGE_CHECK_DATA:
					NextPage = PAGE_PROCESS;
					break;

				default:
					NextPage = PAGE_DEFAULT;
					break;
			}

			if (NextPage >= 0)
				SetPage(NextPage);

			if (NextPage == PAGE_PROCESS)
			{
				Application.DoEvents();
				SetResults( SMimeDecrypt(edInputFile.Text, edOutputFile.Text) );
			}
		}

		private string SMimeEncrypt(string InputFileName, string OutputFileName, int CryptAlgorithm, int CryptAlgorithmBitsInKey)
		{
			int Res = 0;
			string Result = "";

			SBMIME.TElMessage Msg = new SBMIME.TElMessage(cXMailerDemoFieldValue);

			FileStream Stream = new FileStream(InputFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			try
			{
				Res = Msg.ParseMessage(Stream, "", "",	SBMIME.Unit.mpoStoreStream | SBMIME.Unit.mpoLoadData | SBMIME.Unit.mpoCalcDataSize,	false, false, true);
			}
			catch (Exception E)
			{
				Result = E.Message;
				Res = SBMIME.Unit.EL_ERROR;
			}

			Stream.Close();

			if ((Res != SBMIME.Unit.EL_OK) && (Res != SBMIME.Unit.EL_WARNING))
			{
				if (Result != "")
					Result = "Message: \"" + Result + "\"";
				else if ((Res == SBMIME.Unit.EL_HANDLERR_ERROR) && (Msg.MainPart.MessagePartHandler != null))
					Result = "Message: \"" + Msg.MainPart.MessagePartHandler.ErrorText + "\"";

				Result = String.Format("Error parsing mime message {0}.\r\nElMime error code: {1:D}\r\n{2}", InputFileName, Res, Result);
				return Result;
			}

			SBSMIMECore.TElMessagePartHandlerSMime SMime = new SBSMIMECore.TElMessagePartHandlerSMime(null);
			SMime.EncoderCryptCertStorage = MemoryCertStorage;
			Msg.MainPart.MessagePartHandler = SMime;

			SMime.EncoderCrypted = true;
			SMime.EncoderCryptBitsInKey = CryptAlgorithmBitsInKey;
			SMime.EncoderCryptAlgorithm = CryptAlgorithm;

			Stream = new FileStream(OutputFileName, FileMode.Create);
			try
			{
				Res = Msg.AssembleMessage(Stream,
					// Charset of message:
					"utf-8",
					// HeaderEncoding
					SBMIME.TElHeaderEncoding.heBase64, //  variants:  he8bit  | heQuotedPrintable  | heBase64
					// BodyEncoding
					"base64", //  variants:   '8bit' | 'quoted-printable' |  'base64'
					// AttachEncoding
					"base64",  //  variants:   '8bit' | 'quoted-printable' |  'base64'
					false
					);
			}
			catch (Exception E)
			{
				Result = E.Message;
				Res = SBMIME.Unit.EL_ERROR;
			}

			if ((Res == SBMIME.Unit.EL_OK) || (Res == SBMIME.Unit.EL_WARNING))
				Result = "Message encrypted and assembled OK";
			else
			{
				if (Result != "")
					Result = "Message: \"" + Result + "\"";
				else
					if ((Res == SBMIME.Unit.EL_HANDLERR_ERROR) && (Msg.MainPart.MessagePartHandler != null))
					Result = "Message: \"" + Msg.MainPart.MessagePartHandler.ErrorText + "\"";

				Result = String.Format("Failed to assemble a message.\r\nElMime error code: {0:D}\r\n{1}", Res, Result);
			}

			Stream.Close();
			return Result;
		}			

		private void SMimeEncryptNext()
		{
			System.Int16 NextPage = -1;
			switch (CurrentPage)
			{
				case PAGE_SELECT_ACTION:
					NextPage = PAGE_SELECT_FILES;
					break;

				case PAGE_SELECT_FILES:
					if ((edInputFile.Text == "") || (edOutputFile.Text == ""))
						MessageBox.Show("You must select both input and output files", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
					else
						NextPage = PAGE_SELECT_CERTIFICATES;
					break;

				case PAGE_SELECT_CERTIFICATES:
					if (MemoryCertStorage.Count == 0)
						MessageBox.Show("MessageDlg('No recipient certificate selected. Please select one.", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
					else
						NextPage = PAGE_SELECT_ALGORITHM;
					break;

				case PAGE_SELECT_ALGORITHM:
					NextPage = PAGE_CHECK_DATA;
					break;

				case PAGE_CHECK_DATA:
					NextPage = PAGE_PROCESS;
					break;

				default:
					NextPage = PAGE_DEFAULT;
					break;
			}

			if (NextPage >= 0)
				SetPage(NextPage);

			if (NextPage == PAGE_PROCESS)
			{
				Application.DoEvents();
				SetResults( SMimeEncrypt(edInputFile.Text, edOutputFile.Text, GetAlgorithm(), GetAlgorithmBitsInKey()) );
			}
		}

		private string SMimeSign(string InputFileName, string OutputFileName, string SignAlgorithm)
		{
			int Res = 0;
			string Result = "";

			SBMIME.TElMessage Msg = new SBMIME.TElMessage(cXMailerDemoFieldValue);

			FileStream Stream = new FileStream(InputFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			try
			{
				Res = Msg.ParseMessage(Stream, "", "",	SBMIME.Unit.mpoStoreStream | SBMIME.Unit.mpoLoadData | SBMIME.Unit.mpoCalcDataSize,	false, false, true);
			}
			catch (Exception E)
			{
				Result = E.Message;
				Res = SBMIME.Unit.EL_ERROR;
			}

			Stream.Close();

			if ((Res != SBMIME.Unit.EL_OK) && (Res != SBMIME.Unit.EL_WARNING))
			{
				if (Result != "")
					Result = "Message: \"" + Result + "\"";
				else if ((Res == SBMIME.Unit.EL_HANDLERR_ERROR) && (Msg.MainPart.MessagePartHandler != null))
					Result = "Message: \"" + Msg.MainPart.MessagePartHandler.ErrorText + "\"";

				Result = String.Format("Error parsing mime message {0}.\r\nElMime error code: {1:D}\r\n{2}", InputFileName, Res, Result);
				return Result;
			}
			
			SBSMIMECore.TElMessagePartHandlerSMime SMime = new SBSMIMECore.TElMessagePartHandlerSMime(null);
			SMime.EncoderSignCertStorage = MemoryCertStorage;
			Msg.MainPart.MessagePartHandler = SMime;

			SMime.EncoderSigned = true;
			SMime.EncoderSignOnlyClearFormat = true;
			SMime.EncoderMicalg = SignAlgorithm;

			Stream = new FileStream(OutputFileName, FileMode.Create);
			try
			{
				Res = Msg.AssembleMessage(Stream,
					// Charset of message:
					"utf-8",
					// HeaderEncoding
					SBMIME.TElHeaderEncoding.heBase64, //  variants:  he8bit  | heQuotedPrintable  | heBase64
					// BodyEncoding
					"base64", //  variants:   '8bit' | 'quoted-printable' |  'base64'
					// AttachEncoding
					"base64",  //  variants:   '8bit' | 'quoted-printable' |  'base64'
					false
					);
			}
			catch (Exception E)
			{
				Result = E.Message;
				Res = SBMIME.Unit.EL_ERROR;
			}

			if ((Res == SBMIME.Unit.EL_OK) || (Res == SBMIME.Unit.EL_WARNING))
				Result = "Message signed and assembled OK";
			else
			{
				if (Result != "")
				{
					Result = "Message: \"" + Result + "\"";
					if (Result.IndexOf(((int)SBMessages.Unit.SB_MESSAGE_ERROR_NO_CERTIFICATE).ToString()) > 0)
						Result = Result + " (at least one certificate should be loaded with corresponding sender (from email for message should be equal to certificate email field or to SubjectAlternativeName))";
				}
				else
					if (Res == SBMIME.Unit.EL_HANDLERR_ERROR) 
					Result = "Message: \"" + SMime.ErrorText + "\"";

				Result = String.Format("Failed to assemble a message.\r\nElMime error code: {0:D}\r\n{1}", Res, Result);
			}

			Stream.Close();
			return Result;
		}

		private void SMimeSignNext()
		{
			System.Int16 NextPage = -1;
			switch (CurrentPage)
			{
				case PAGE_SELECT_ACTION:
					NextPage = PAGE_SELECT_FILES;
					break;

				case PAGE_SELECT_FILES:
					if ((edInputFile.Text == "") || (edOutputFile.Text == ""))
						MessageBox.Show("You must select both input and output files", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
					else
						NextPage = PAGE_SELECT_CERTIFICATES;
					break;

				case PAGE_SELECT_CERTIFICATES:
					bool Found = false;
					byte[] Buf = null;
					int Sz;
					for (int i = 0; i < MemoryCertStorage.Count; i++)
					{
						Sz = 0;
						MemoryCertStorage.get_Certificates(i).SaveKeyToBuffer(ref Buf, ref Sz);
						if (Sz > 0) 
						{
							Found = true;
							break;
						}
					}

					if (!Found)
						MessageBox.Show("At least one certificate should be loaded with corresponding private key", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
					else
						NextPage = PAGE_SELECT_ALGORITHM;
					break;
		
				case PAGE_SELECT_ALGORITHM:
					NextPage = PAGE_CHECK_DATA;
					break;

				case PAGE_CHECK_DATA:
					NextPage = PAGE_PROCESS;
					break;

				default:
					NextPage = PAGE_DEFAULT;
					break;
			}

			if (NextPage >= 0) 
				SetPage(NextPage);

			if (NextPage == PAGE_PROCESS)
			{
				Application.DoEvents();
				SetResults( SMimeSign(edInputFile.Text, edOutputFile.Text, GetSignAlgorithm()) );
			}
		}

		private string SMimeVerify(string InputFileName)
		{
			int Res = 0;
			string Result = "";

			SBMIME.TElMessage Msg = new SBMIME.TElMessage(cXMailerDemoFieldValue);

			FileStream Stream = new FileStream(InputFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			try
			{
				Res = Msg.ParseMessage(Stream, "", "",	SBMIME.Unit.mpoStoreStream | SBMIME.Unit.mpoLoadData | SBMIME.Unit.mpoCalcDataSize,	false, false, false);
			}
			catch (Exception E)
			{
				Result = E.Message;
				Res = SBMIME.Unit.EL_ERROR;
			}

			if ((Res == SBMIME.Unit.EL_OK) || (Res == SBMIME.Unit.EL_WARNING))
			{
				if ((Msg.MainPart == null) ||
					(Msg.MainPart.MessagePartHandler == null) ||
					(Msg.MainPart.IsActivatedMessagePartHandler))
				{
					Stream.Close();
					return "Mesage not encoded. No action done.";
				}

				if (Msg.MainPart.MessagePartHandler.IsError)
				{
					Result = Msg.MainPart.MessagePartHandler.ErrorText;
					Res = SBMIME.Unit.EL_ERROR;
				}
				else
				{
					if (Msg.MainPart.MessagePartHandler is SBSMIMECore.TElMessagePartHandlerSMime)
					{
						SBSMIMECore.TElMessagePartHandlerSMime Handler = (SBSMIMECore.TElMessagePartHandlerSMime)Msg.MainPart.MessagePartHandler;
						Handler.CertificatesStorage = MemoryCertStorage;

						try
						{
							Res = Msg.MainPart.MessagePartHandler.Decode(true);
						}
						catch (Exception E)
						{
							Result = E.Message;
							Res = SBMIME.Unit.EL_ERROR;
						}

						if (Handler.Errors != 0)
						{
							if (Result != "")
								Result = Result + "\r\n";

							Result = Result + "Errors: ";
							if ((SBSMIMECore.Unit.smeUnknown & Handler.Errors) != 0)
								Result = Result + "smeUnknown, ";

							if ((SBSMIMECore.Unit.smeSignaturePartNotFound & Handler.Errors) != 0)
								Result = Result + "smeSignaturePartNotFound, ";

							if ((SBSMIMECore.Unit.smeBodyPartNotFound & Handler.Errors) != 0)
								Result = Result + "smeBodyPartNotFound, ";

							if ((SBSMIMECore.Unit.smeInvalidSignature & Handler.Errors) != 0)
								Result = Result + "smeInvalidSignature, ";

							if ((SBSMIMECore.Unit.smeSigningCertificateMismatch & Handler.Errors) != 0)
								Result = Result + "smeSigningCertificateMismatch, ";

							if ((SBSMIMECore.Unit.smeEncryptingCertificateMismatch & Handler.Errors) != 0)
								Result = Result + "smeEncryptingCertificateMismatch, ";

							if ((SBSMIMECore.Unit.smeNoData & Handler.Errors) != 0)
								Result = Result + "smeNoData, ";

							Result.Remove(Result.Length - 3, 2);
						}
					}
					else
					{
						Result = "Unknown message handler.";
						Res = SBMIME.Unit.EL_ERROR;
					}
				}
			}

			Stream.Close();

			if ((Res != SBMIME.Unit.EL_OK) && (Res != SBMIME.Unit.EL_WARNING))
			{
				if (Result != "")
					Result = "Message: \"" + Result + "\"";
				else
					if ((Res == SBMIME.Unit.EL_HANDLERR_ERROR) && (Msg.MainPart.MessagePartHandler != null))
					Result = "Message: \"" + Msg.MainPart.MessagePartHandler.ErrorText + "\"";

				Result = String.Format("Error parsing mime message \"{0}\".\r\nElMime error code: {1:D}\r\n{2}", InputFileName, Res, Result);
			}
			else
			{
				Result = "Message verified OK";
				if ((Msg.MainPart.MessagePartHandler != null) &&
					(Msg.MainPart.MessagePartHandler is TElMessagePartHandlerSMime))
				{
					Result = Result + "\r\n\r\nSigned with:\r\n";
					SBSMIMECore.TElMessagePartHandlerSMime Handler = (SBSMIMECore.TElMessagePartHandlerSMime)Msg.MainPart.MessagePartHandler;
					Result = Result + WriteCertificateInfo(Handler.DecoderSignCertStorage);
				}
			}

			return Result;
		}

		private void SMimeVerifyNext()
		{
			System.Int16 NextPage = -1;
			switch (CurrentPage)
			{
				case PAGE_SELECT_ACTION:
					NextPage = PAGE_SELECT_FILES;
					break;

				case PAGE_SELECT_FILES:
					if (edInputFile.Text == "")
						MessageBox.Show("You must select input file", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
					else
						NextPage = PAGE_SELECT_CERTIFICATES;
					break;

				case PAGE_SELECT_CERTIFICATES:	
					NextPage = PAGE_CHECK_DATA;
					break;

				case PAGE_CHECK_DATA:
					NextPage = PAGE_PROCESS;
					break;

				default:
					NextPage = PAGE_DEFAULT;
					break;
			}

			if (NextPage >= 0)
				SetPage(NextPage);

			if (NextPage == PAGE_PROCESS)
			{
				Application.DoEvents();
				SetResults( SMimeVerify(edInputFile.Text) );
			}
		}

		private void sbInputFile_Click(object sender, System.EventArgs e)
		{
			OpenDlg.Title = "Select input file";
			OpenDlg.Filter = "Message files (*.eml)|*.eml|All files (*.*)|*.*";
			OpenDlg.FileName = edInputFile.Text;
			if (OpenDlg.ShowDialog(this) == DialogResult.OK)
				edInputFile.Text = OpenDlg.FileName;		
		}

		private void sbOutputFile_Click(object sender, System.EventArgs e)
		{
			SaveDlg.Title = "Select output file";
			SaveDlg.Filter = "Message files (*.eml)|*.eml|All files (*.*)|*.*";
			SaveDlg.FileName = edOutputFile.Text;
			if (SaveDlg.ShowDialog(this) == DialogResult.OK)
				edOutputFile.Text = SaveDlg.FileName;		
		}

		private void btnDoIt_Click(object sender, System.EventArgs e)
		{
			Next();
		}

		private void btnAddCertificate_Click(object sender, System.EventArgs e)
		{
			bool KeyLoaded = false;
			//bool Cancel = true;
			int index;
			OpenDlg.FileName = "";
			OpenDlg.Title = "Select certificate file";
			OpenDlg.Filter = "PEM-encoded certificate (*.pem)|*.pem|DER-encoded certificate (*.cer)|*.cer|PFX-encoded certificate (*.pfx)|*.pfx";
			if (OpenDlg.ShowDialog(this) != DialogResult.OK)
				return;
	
			FileStream F = new FileStream(OpenDlg.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			byte [] Buf = new byte[F.Length];
			F.Read(Buf, 0, (int)F.Length);
			F.Close();

			int Res = 0;
			SBX509.TElX509Certificate Cert = new SBX509.TElX509Certificate(null);
			if (OpenDlg.FilterIndex == 3)
				Res = Cert.LoadFromBufferPFX(Buf, RequestPassphrase());
			else if (OpenDlg.FilterIndex == 1)
				Res = Cert.LoadFromBufferPEM(Buf, "");
			else if (OpenDlg.FilterIndex == 2)
				Cert.LoadFromBuffer(Buf);
			else
				Res = -1;

			if ((Res != 0) || (Cert.CertificateSize == 0))
			{				
				MessageBox.Show("Error loading the certificate", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if ((Action == ACTION_SMIME_DECRYPT) || (Action == ACTION_SMIME_SIGN))
			{
				int Sz = 0;
				Buf = null;
				Cert.SaveKeyToBuffer(ref Buf, ref Sz);

				if (Sz == 0)
				{
					OpenDlg.Title = "Select the corresponding private key file";
					OpenDlg.Filter = "PEM-encoded key (*.pem)|*.PEM|DER-encoded key (*.key)|*.key";
					if (OpenDlg.ShowDialog(this) == DialogResult.OK)
					{
						F = new FileStream(OpenDlg.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
						Buf = new byte[F.Length];
						F.Read(Buf, 0, (int)F.Length);
						F.Close();

						if (OpenDlg.FilterIndex == 1)
							Cert.LoadKeyFromBufferPEM(Buf, RequestPassphrase());
						else
							Cert.LoadKeyFromBuffer(Buf);

						KeyLoaded = true;
					}
				}
				else
					KeyLoaded = true;
			}

			// certificate e-mail in UTF8
			string sFrom = fRDN.GetOIDValue(Cert.SubjectRDN, SBUtils.Unit.SB_CERT_OID_EMAIL);
			if (sFrom == "")
			{
				index = Cert.Extensions.SubjectAlternativeName.Content.FindNameByType(TSBGeneralName.gnRFC822Name, 0);
				if (index >= 0) 
					sFrom = Cert.Extensions.SubjectAlternativeName.Content.get_Names(index).RFC822Name;
				else
					MessageBox.Show("Warning: Certificate does not contain e-mail address.", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			if ((Action == ACTION_SMIME_DECRYPT) && (!KeyLoaded))
				MessageBox.Show("Private key was not loaded, certificate ignored", "Secure Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
			else
			{
				MemoryCertStorage.Add(Cert, true);
				UpdateCertificatesList();
			}
		}

		private void btnRemoveCertificate_Click(object sender, System.EventArgs e)
		{
			if (lbxCertificates.SelectedIndex >= 0)
			{
				MemoryCertStorage.Remove(lbxCertificates.SelectedIndex);
				UpdateCertificatesList();
			}
		}

		private void UpdateCertificatesList()
		{
			lbxCertificates.BeginUpdate();
			lbxCertificates.Items.Clear();
			string s;
			for (int i = 0; i < MemoryCertStorage.Count; i++)
			{
				s = fRDN.GetOIDValue(MemoryCertStorage.get_Certificates(i).SubjectRDN, SBUtils.Unit.SB_CERT_OID_COMMON_NAME);
				if (s == "")
					s = fRDN.GetOIDValue(MemoryCertStorage.get_Certificates(i).SubjectRDN, SBUtils.Unit.SB_CERT_OID_ORGANIZATION);

				if (s == "")
					s = "<unknown>";

				lbxCertificates.Items.Add(s);
			}

			lbxCertificates.EndUpdate();
		}

		private void sbKeyring_Click(object sender, System.EventArgs e)
		{
			OpenDlg.Title = "Select input file";
			OpenDlg.Filter = "PGP Keyring files (*.asc, *.pkr, *.skr, *.gpg, *.pgp)|*.asc;*.pkr;*.skr;*.gpg;*.pgp";
			OpenDlg.FileName = edKeyring.Text;
			if (OpenDlg.ShowDialog(this) == DialogResult.OK)
			{
				FileStream F = new FileStream(OpenDlg.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
				try
				{
					if (Action == ACTION_PGPMIME_ENCRYPT)
						PublicRing.Load(F, null, true);
					else if (Action == ACTION_PGPMIME_SIGN)
						SecretRing.Load(F, null, true);
				}				
				catch 
				{
					edKeyring.Text = "";
					return;
				}
				finally
				{
					F.Close();
				}
				edKeyring.Text = OpenDlg.FileName;
			}
		
		}

		private void btnAddKey_Click(object sender, System.EventArgs e)
		{			
			OpenDlg.Title = "Select input file";
			OpenDlg.Filter = "PGP Keyring files (*.asc, *.pkr, *.skr, *.gpg, *.pgp)|*.asc;*.pkr;*.skr;*.gpg;*.pgp";
			OpenDlg.FileName = "";
			if (OpenDlg.ShowDialog(this) == DialogResult.OK)
			{
				SBPGPKeys.TElPGPKeyring TempKeyring = new SBPGPKeys.TElPGPKeyring(null);
				TempKeyring.Load(OpenDlg.FileName, "", true);
				if ((Action == ACTION_PGPMIME_VERIFY) && (TempKeyring.PublicCount > 0))
					TempKeyring.ExportTo(Keyring);

				if ((Action == ACTION_PGPMIME_DECRYPT) && (TempKeyring.SecretCount > 0)) 
					TempKeyring.ExportTo(Keyring);
				
				UpdateKeysList();
			}
		}

		private void btnRemoveKey_Click(object sender, System.EventArgs e)
		{
			if ((tvKeys.SelectedNode != null) && (tvKeys.SelectedNode.Tag != null))
			{
				if (tvKeys.SelectedNode.Tag is SBPGPKeys.TElPGPPublicKey)
				{
					SBPGPKeys.TElPGPPublicKey Key = (SBPGPKeys.TElPGPPublicKey)tvKeys.SelectedNode.Tag;
					if (Key.SecretKey != null) 
						Keyring.RemoveSecretKey(Key.SecretKey);
					else
						Keyring.RemovePublicKey(Key, true);
				}
				else if (tvKeys.SelectedNode.Tag is SBPGPKeys.TElPGPSecretKey)
					Keyring.RemoveSecretKey((TElPGPSecretKey)tvKeys.SelectedNode.Tag);

				UpdateKeysList();
			}
		}

		private void UpdateKeysList()
		{
			string s;
			TreeNode Node;
			tvKeys.BeginUpdate();
			tvKeys.Nodes.Clear();
			if (Action == ACTION_PGPMIME_DECRYPT)
			{
				for (int i = 0; i < Keyring.SecretCount; i++)
				{
					s = GetPublicKeyNames(Keyring.get_SecretKeys(i).PublicKey);
					if (s != "")
						s = s + " (" + SBPGPUtils.Unit.KeyID2Str(Keyring.get_SecretKeys(i).KeyID(), true) + ")";
					else
						s = SBPGPUtils.Unit.KeyID2Str(Keyring.get_SecretKeys(i).KeyID(), true);

					if (s == "")
						s = "<unknown>";

					Node = tvKeys.Nodes.Add(s);
					Node.Tag = Keyring.get_SecretKeys(i);
				}
			}

			if (Action == ACTION_PGPMIME_VERIFY)
			{
				for (int i = 0; i < Keyring.PublicCount; i++)
				{
					s = GetPublicKeyNames(Keyring.get_PublicKeys(i));
					if (s != "")
						s = s + " (" + SBPGPUtils.Unit.KeyID2Str(Keyring.get_PublicKeys(i).KeyID(), true) + ")";
							else
						s = SBPGPUtils.Unit.KeyID2Str(Keyring.get_PublicKeys(i).KeyID(), true);

					if (s == "")
						s = "<unknown>";

					Node = tvKeys.Nodes.Add(s);
					Node.Tag = Keyring.get_PublicKeys(i);
				}
			}

			tvKeys.EndUpdate();
		}
	}

	public class fRDN
	{
		public static string GetStringByOID(byte[] S)
		{
			if (SBUtils.Unit.CompareContent(S, (byte[])SBUtils.Unit.SB_CERT_OID_COMMON_NAME))
				return "CommonName";
			else
				if (SBUtils.Unit.CompareContent(S, (byte[])SBUtils.Unit.SB_CERT_OID_COUNTRY))
				return "Country";
			else
				if (SBUtils.Unit.CompareContent(S, (byte[])SBUtils.Unit.SB_CERT_OID_LOCALITY))
				return "Locality";
			else
				if (SBUtils.Unit.CompareContent(S, (byte[])SBUtils.Unit.SB_CERT_OID_STATE_OR_PROVINCE))
				return "StateOrProvince";
			else
				if (SBUtils.Unit.CompareContent(S, (byte[])SBUtils.Unit.SB_CERT_OID_ORGANIZATION))
				return "Organization";
			else
				if (SBUtils.Unit.CompareContent(S, (byte[])SBUtils.Unit.SB_CERT_OID_ORGANIZATION_UNIT))
				return "OrganizationUnit";
			else
				if (SBUtils.Unit.CompareContent(S, (byte[])SBUtils.Unit.SB_CERT_OID_EMAIL))
				return "Email";
			else
				return "UnknownField";			
		}

		public static string GetOIDValue(SBRDN.TElRelativeDistinguishedName RDN, byte[] S)
		{
			string t = "";
			int iCount = RDN.Count;
			for (int i = 0; i < iCount; i++)
			{
				if (SBUtils.Unit.CompareContent(RDN.get_OIDs(i), S))
				{
					if (t != "")
						t += " / ";

					t += SBUtils.Unit.UTF8ToStr(RDN.get_Values(i));
				}
			}

			return t;
		}
	}
}
