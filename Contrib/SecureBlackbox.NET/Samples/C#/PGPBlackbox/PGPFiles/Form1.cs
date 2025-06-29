using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace PGPFilesDemo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private const System.Int16 STATE_SELECT_OPERATION			= 0;
		private const System.Int16 STATE_PROTECT_SELECT_KEYRING		= 1;
		private const System.Int16 STATE_PROTECT_SELECT_SOURCE		= 2;
		private const System.Int16 STATE_PROTECT_SELECT_OPERATION	= 3;
		private const System.Int16 STATE_PROTECT_SELECT_RECIPIENTS	= 4;
		private const System.Int16 STATE_PROTECT_SELECT_SIGNERS		= 5;
		private const System.Int16 STATE_PROTECT_SELECT_DESTINATION	= 6;
		private const System.Int16 STATE_PROTECT_PROGRESS			= 7;
		private const System.Int16 STATE_PROTECT_FINISH				= 8;
		private const System.Int16 STATE_DECRYPT_SELECT_KEYRING		= 11;
		private const System.Int16 STATE_DECRYPT_SELECT_SOURCE		= 12;
		private const System.Int16 STATE_DECRYPT_PROGRESS			= 13;
		private const System.Int16 STATE_DECRYPT_FINISH				= 14;
		private const System.Int16 STATE_FINISH						= 255;
		private const System.Int16 STATE_INVALID					= -1;
		private System.Int16 state;
		private string source;
		private SBPGPKeys.TElPGPSignature[] sigs;
		private SBPGPStreams.TSBPGPSignatureValidity[] vals;

		private System.Windows.Forms.Panel pHints;
		private System.Windows.Forms.Label lStage;
		private System.Windows.Forms.Label lStageComment;
		private System.Windows.Forms.Panel pMain;
		private System.Windows.Forms.Panel pNavigation;
		private System.Windows.Forms.Panel pClient;
		private System.Windows.Forms.Panel pOperationSelect;
		private System.Windows.Forms.RadioButton rbProtect;
		private System.Windows.Forms.RadioButton rbUnprotect;
		private System.Windows.Forms.Label lIWantTo;
		private System.Windows.Forms.GroupBox gbSeparator;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnBack;
		private System.Windows.Forms.Label lPubKeyring;
		private System.Windows.Forms.Label lSecKeyring;
		private System.Windows.Forms.TextBox tbPubKeyring;
		private System.Windows.Forms.TextBox tbSecKeyring;
		private System.Windows.Forms.Button btnBrowsePub;
		private System.Windows.Forms.Button btnBrowseSec;
		private System.Windows.Forms.Panel pFileSelect;
		private System.Windows.Forms.Label lPrompt;
		private System.Windows.Forms.TextBox tbFile;
		private System.Windows.Forms.Button btnBrowseFile;
		private System.Windows.Forms.CheckBox cbEncrypt;
		private System.Windows.Forms.CheckBox cbSign;
		private System.Windows.Forms.Label lEncryptionAlg;
		private System.Windows.Forms.ComboBox cbEncryptionAlg;
		private System.Windows.Forms.Panel pEncOps;
		private System.Windows.Forms.Panel pKeyringSelect;
		private System.Windows.Forms.CheckBox cbUseConvEnc;
		private System.Windows.Forms.Label lProtLevel;
		private System.Windows.Forms.ComboBox cbProtLevel;
		private System.Windows.Forms.CheckBox cbTextInput;
		private System.Windows.Forms.CheckBox cbCompress;
		private System.Windows.Forms.CheckBox cbUseNewFeatures;
		private System.Windows.Forms.Panel pUserSelect;
		private System.Windows.Forms.ListView lvKeys;
		private System.Windows.Forms.ColumnHeader chUserID;
		private System.Windows.Forms.ColumnHeader chAlgorithm;
		private System.Windows.Forms.ColumnHeader chBits;
		private System.Windows.Forms.Label lPassphrase;
		private System.Windows.Forms.Label lPassphraseConfirmation;
		private System.Windows.Forms.TextBox tbPassphrase;
		private System.Windows.Forms.TextBox tbPassphraseConf;
		private SBPGPKeys.TElPGPKeyring keyring;
		private SBPGP.TElPGPReader pgpReader;
		private SBPGP.TElPGPWriter pgpWriter;
		private SBPGPKeys.TElPGPKeyring pubKeyring;
		private SBPGPKeys.TElPGPKeyring secKeyring;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.OpenFileDialog openKeyringDialog;
		private System.Windows.Forms.Panel pProgress;
		private System.Windows.Forms.Label lProcessingFile;
		private System.Windows.Forms.ProgressBar pbProgress;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel pFinish;
		private System.Windows.Forms.Label lFinished;
		private System.Windows.Forms.Label lErrorComment;
		private System.Windows.Forms.ImageList imgKeys;
		private System.Windows.Forms.Button btnSignatures;
		private System.Windows.Forms.Label lFileSelectComment;
		private System.ComponentModel.IContainer components;

		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			ChangeState(STATE_SELECT_OPERATION);
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmMain));
			this.pHints = new System.Windows.Forms.Panel();
			this.lStageComment = new System.Windows.Forms.Label();
			this.lStage = new System.Windows.Forms.Label();
			this.pMain = new System.Windows.Forms.Panel();
			this.pClient = new System.Windows.Forms.Panel();
			this.pEncOps = new System.Windows.Forms.Panel();
			this.cbUseNewFeatures = new System.Windows.Forms.CheckBox();
			this.cbCompress = new System.Windows.Forms.CheckBox();
			this.cbTextInput = new System.Windows.Forms.CheckBox();
			this.cbProtLevel = new System.Windows.Forms.ComboBox();
			this.lProtLevel = new System.Windows.Forms.Label();
			this.cbUseConvEnc = new System.Windows.Forms.CheckBox();
			this.cbEncryptionAlg = new System.Windows.Forms.ComboBox();
			this.lEncryptionAlg = new System.Windows.Forms.Label();
			this.cbSign = new System.Windows.Forms.CheckBox();
			this.cbEncrypt = new System.Windows.Forms.CheckBox();
			this.pFinish = new System.Windows.Forms.Panel();
			this.btnSignatures = new System.Windows.Forms.Button();
			this.lErrorComment = new System.Windows.Forms.Label();
			this.lFinished = new System.Windows.Forms.Label();
			this.pProgress = new System.Windows.Forms.Panel();
			this.pbProgress = new System.Windows.Forms.ProgressBar();
			this.lProcessingFile = new System.Windows.Forms.Label();
			this.pUserSelect = new System.Windows.Forms.Panel();
			this.tbPassphraseConf = new System.Windows.Forms.TextBox();
			this.tbPassphrase = new System.Windows.Forms.TextBox();
			this.lPassphraseConfirmation = new System.Windows.Forms.Label();
			this.lPassphrase = new System.Windows.Forms.Label();
			this.lvKeys = new System.Windows.Forms.ListView();
			this.chUserID = new System.Windows.Forms.ColumnHeader();
			this.chAlgorithm = new System.Windows.Forms.ColumnHeader();
			this.chBits = new System.Windows.Forms.ColumnHeader();
			this.imgKeys = new System.Windows.Forms.ImageList(this.components);
			this.pFileSelect = new System.Windows.Forms.Panel();
			this.btnBrowseFile = new System.Windows.Forms.Button();
			this.tbFile = new System.Windows.Forms.TextBox();
			this.lPrompt = new System.Windows.Forms.Label();
			this.pOperationSelect = new System.Windows.Forms.Panel();
			this.lIWantTo = new System.Windows.Forms.Label();
			this.rbUnprotect = new System.Windows.Forms.RadioButton();
			this.rbProtect = new System.Windows.Forms.RadioButton();
			this.pKeyringSelect = new System.Windows.Forms.Panel();
			this.btnBrowseSec = new System.Windows.Forms.Button();
			this.btnBrowsePub = new System.Windows.Forms.Button();
			this.tbSecKeyring = new System.Windows.Forms.TextBox();
			this.tbPubKeyring = new System.Windows.Forms.TextBox();
			this.lSecKeyring = new System.Windows.Forms.Label();
			this.lPubKeyring = new System.Windows.Forms.Label();
			this.pNavigation = new System.Windows.Forms.Panel();
			this.btnBack = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnNext = new System.Windows.Forms.Button();
			this.gbSeparator = new System.Windows.Forms.GroupBox();
			this.keyring = new SBPGPKeys.TElPGPKeyring();
			this.pgpReader = new SBPGP.TElPGPReader();
			this.pgpWriter = new SBPGP.TElPGPWriter();
			this.pubKeyring = new SBPGPKeys.TElPGPKeyring();
			this.secKeyring = new SBPGPKeys.TElPGPKeyring();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.openKeyringDialog = new System.Windows.Forms.OpenFileDialog();
			this.lFileSelectComment = new System.Windows.Forms.Label();
			this.pHints.SuspendLayout();
			this.pMain.SuspendLayout();
			this.pClient.SuspendLayout();
			this.pEncOps.SuspendLayout();
			this.pFinish.SuspendLayout();
			this.pProgress.SuspendLayout();
			this.pUserSelect.SuspendLayout();
			this.pFileSelect.SuspendLayout();
			this.pOperationSelect.SuspendLayout();
			this.pKeyringSelect.SuspendLayout();
			this.pNavigation.SuspendLayout();
			this.SuspendLayout();
			// 
			// pHints
			// 
			this.pHints.BackColor = System.Drawing.SystemColors.Info;
			this.pHints.Controls.Add(this.lStageComment);
			this.pHints.Controls.Add(this.lStage);
			this.pHints.Dock = System.Windows.Forms.DockStyle.Top;
			this.pHints.Location = new System.Drawing.Point(0, 0);
			this.pHints.Name = "pHints";
			this.pHints.Size = new System.Drawing.Size(528, 48);
			this.pHints.TabIndex = 1;
			// 
			// lStageComment
			// 
			this.lStageComment.Location = new System.Drawing.Point(8, 24);
			this.lStageComment.Name = "lStageComment";
			this.lStageComment.Size = new System.Drawing.Size(496, 16);
			this.lStageComment.TabIndex = 1;
			this.lStageComment.Text = "Choose desired operation";
			// 
			// lStage
			// 
			this.lStage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.lStage.Location = new System.Drawing.Point(8, 8);
			this.lStage.Name = "lStage";
			this.lStage.Size = new System.Drawing.Size(496, 16);
			this.lStage.TabIndex = 0;
			this.lStage.Text = "Stage 1 of 5";
			// 
			// pMain
			// 
			this.pMain.Controls.Add(this.pClient);
			this.pMain.Controls.Add(this.pNavigation);
			this.pMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pMain.Location = new System.Drawing.Point(0, 48);
			this.pMain.Name = "pMain";
			this.pMain.Size = new System.Drawing.Size(528, 309);
			this.pMain.TabIndex = 2;
			// 
			// pClient
			// 
			this.pClient.Controls.Add(this.pFileSelect);
			this.pClient.Controls.Add(this.pEncOps);
			this.pClient.Controls.Add(this.pFinish);
			this.pClient.Controls.Add(this.pProgress);
			this.pClient.Controls.Add(this.pUserSelect);
			this.pClient.Controls.Add(this.pOperationSelect);
			this.pClient.Controls.Add(this.pKeyringSelect);
			this.pClient.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pClient.Location = new System.Drawing.Point(0, 0);
			this.pClient.Name = "pClient";
			this.pClient.Size = new System.Drawing.Size(528, 253);
			this.pClient.TabIndex = 1;
			// 
			// pEncOps
			// 
			this.pEncOps.Controls.Add(this.cbUseNewFeatures);
			this.pEncOps.Controls.Add(this.cbCompress);
			this.pEncOps.Controls.Add(this.cbTextInput);
			this.pEncOps.Controls.Add(this.cbProtLevel);
			this.pEncOps.Controls.Add(this.lProtLevel);
			this.pEncOps.Controls.Add(this.cbUseConvEnc);
			this.pEncOps.Controls.Add(this.cbEncryptionAlg);
			this.pEncOps.Controls.Add(this.lEncryptionAlg);
			this.pEncOps.Controls.Add(this.cbSign);
			this.pEncOps.Controls.Add(this.cbEncrypt);
			this.pEncOps.Location = new System.Drawing.Point(360, 24);
			this.pEncOps.Name = "pEncOps";
			this.pEncOps.TabIndex = 3;
			// 
			// cbUseNewFeatures
			// 
			this.cbUseNewFeatures.Location = new System.Drawing.Point(40, 200);
			this.cbUseNewFeatures.Name = "cbUseNewFeatures";
			this.cbUseNewFeatures.Size = new System.Drawing.Size(112, 24);
			this.cbUseNewFeatures.TabIndex = 9;
			this.cbUseNewFeatures.Text = "Use new features";
			// 
			// cbCompress
			// 
			this.cbCompress.Enabled = false;
			this.cbCompress.Location = new System.Drawing.Point(64, 112);
			this.cbCompress.Name = "cbCompress";
			this.cbCompress.Size = new System.Drawing.Size(280, 24);
			this.cbCompress.TabIndex = 8;
			this.cbCompress.Text = "Compress data before encryption";
			// 
			// cbTextInput
			// 
			this.cbTextInput.Enabled = false;
			this.cbTextInput.Location = new System.Drawing.Point(64, 168);
			this.cbTextInput.Name = "cbTextInput";
			this.cbTextInput.Size = new System.Drawing.Size(216, 24);
			this.cbTextInput.TabIndex = 7;
			this.cbTextInput.Text = "Treat input as text";
			// 
			// cbProtLevel
			// 
			this.cbProtLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbProtLevel.Enabled = false;
			this.cbProtLevel.Items.AddRange(new object[] {
															 "Low",
															 "Normal",
															 "High"});
			this.cbProtLevel.Location = new System.Drawing.Point(184, 56);
			this.cbProtLevel.Name = "cbProtLevel";
			this.cbProtLevel.Size = new System.Drawing.Size(104, 21);
			this.cbProtLevel.TabIndex = 6;
			// 
			// lProtLevel
			// 
			this.lProtLevel.Enabled = false;
			this.lProtLevel.Location = new System.Drawing.Point(184, 40);
			this.lProtLevel.Name = "lProtLevel";
			this.lProtLevel.Size = new System.Drawing.Size(208, 16);
			this.lProtLevel.TabIndex = 5;
			this.lProtLevel.Text = "Protection level";
			// 
			// cbUseConvEnc
			// 
			this.cbUseConvEnc.Enabled = false;
			this.cbUseConvEnc.Location = new System.Drawing.Point(64, 88);
			this.cbUseConvEnc.Name = "cbUseConvEnc";
			this.cbUseConvEnc.Size = new System.Drawing.Size(280, 24);
			this.cbUseConvEnc.TabIndex = 4;
			this.cbUseConvEnc.Text = "Use conventional (passphrase) encryption";
			// 
			// cbEncryptionAlg
			// 
			this.cbEncryptionAlg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbEncryptionAlg.Enabled = false;
			this.cbEncryptionAlg.Items.AddRange(new object[] {
																 "CAST5",
																 "3DES",
																 "AES128",
																 "AES256"});
			this.cbEncryptionAlg.Location = new System.Drawing.Point(64, 56);
			this.cbEncryptionAlg.Name = "cbEncryptionAlg";
			this.cbEncryptionAlg.Size = new System.Drawing.Size(104, 21);
			this.cbEncryptionAlg.TabIndex = 3;
			// 
			// lEncryptionAlg
			// 
			this.lEncryptionAlg.Enabled = false;
			this.lEncryptionAlg.Location = new System.Drawing.Point(64, 40);
			this.lEncryptionAlg.Name = "lEncryptionAlg";
			this.lEncryptionAlg.Size = new System.Drawing.Size(200, 16);
			this.lEncryptionAlg.TabIndex = 2;
			this.lEncryptionAlg.Text = "Encryption algorithm:";
			// 
			// cbSign
			// 
			this.cbSign.Location = new System.Drawing.Point(40, 144);
			this.cbSign.Name = "cbSign";
			this.cbSign.TabIndex = 1;
			this.cbSign.Text = "Sign source";
			this.cbSign.CheckedChanged += new System.EventHandler(this.cbSign_CheckedChanged);
			// 
			// cbEncrypt
			// 
			this.cbEncrypt.Location = new System.Drawing.Point(40, 16);
			this.cbEncrypt.Name = "cbEncrypt";
			this.cbEncrypt.TabIndex = 0;
			this.cbEncrypt.Text = "Encrypt source";
			this.cbEncrypt.CheckedChanged += new System.EventHandler(this.cbEncrypt_CheckedChanged);
			// 
			// pFinish
			// 
			this.pFinish.Controls.Add(this.btnSignatures);
			this.pFinish.Controls.Add(this.lErrorComment);
			this.pFinish.Controls.Add(this.lFinished);
			this.pFinish.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pFinish.Location = new System.Drawing.Point(0, 0);
			this.pFinish.Name = "pFinish";
			this.pFinish.Size = new System.Drawing.Size(528, 253);
			this.pFinish.TabIndex = 6;
			// 
			// btnSignatures
			// 
			this.btnSignatures.Enabled = false;
			this.btnSignatures.Location = new System.Drawing.Point(424, 216);
			this.btnSignatures.Name = "btnSignatures";
			this.btnSignatures.Size = new System.Drawing.Size(88, 23);
			this.btnSignatures.TabIndex = 2;
			this.btnSignatures.Text = "Signatures...";
			this.btnSignatures.Visible = false;
			this.btnSignatures.Click += new System.EventHandler(this.btnSignatures_Click);
			// 
			// lErrorComment
			// 
			this.lErrorComment.Location = new System.Drawing.Point(8, 120);
			this.lErrorComment.Name = "lErrorComment";
			this.lErrorComment.Size = new System.Drawing.Size(512, 40);
			this.lErrorComment.TabIndex = 1;
			this.lErrorComment.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lFinished
			// 
			this.lFinished.Location = new System.Drawing.Point(8, 104);
			this.lFinished.Name = "lFinished";
			this.lFinished.Size = new System.Drawing.Size(512, 16);
			this.lFinished.TabIndex = 0;
			this.lFinished.Text = "Operation successfully finished";
			this.lFinished.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// pProgress
			// 
			this.pProgress.Controls.Add(this.pbProgress);
			this.pProgress.Controls.Add(this.lProcessingFile);
			this.pProgress.Location = new System.Drawing.Point(160, 8);
			this.pProgress.Name = "pProgress";
			this.pProgress.TabIndex = 5;
			// 
			// pbProgress
			// 
			this.pbProgress.Location = new System.Drawing.Point(96, 88);
			this.pbProgress.Name = "pbProgress";
			this.pbProgress.Size = new System.Drawing.Size(336, 23);
			this.pbProgress.TabIndex = 1;
			// 
			// lProcessingFile
			// 
			this.lProcessingFile.Location = new System.Drawing.Point(96, 72);
			this.lProcessingFile.Name = "lProcessingFile";
			this.lProcessingFile.Size = new System.Drawing.Size(100, 16);
			this.lProcessingFile.TabIndex = 0;
			this.lProcessingFile.Text = "Processing a file...";
			// 
			// pUserSelect
			// 
			this.pUserSelect.Controls.Add(this.tbPassphraseConf);
			this.pUserSelect.Controls.Add(this.tbPassphrase);
			this.pUserSelect.Controls.Add(this.lPassphraseConfirmation);
			this.pUserSelect.Controls.Add(this.lPassphrase);
			this.pUserSelect.Controls.Add(this.lvKeys);
			this.pUserSelect.Location = new System.Drawing.Point(232, 8);
			this.pUserSelect.Name = "pUserSelect";
			this.pUserSelect.TabIndex = 4;
			// 
			// tbPassphraseConf
			// 
			this.tbPassphraseConf.Location = new System.Drawing.Point(24, 216);
			this.tbPassphraseConf.Name = "tbPassphraseConf";
			this.tbPassphraseConf.PasswordChar = '*';
			this.tbPassphraseConf.Size = new System.Drawing.Size(480, 21);
			this.tbPassphraseConf.TabIndex = 4;
			this.tbPassphraseConf.Text = "";
			// 
			// tbPassphrase
			// 
			this.tbPassphrase.Location = new System.Drawing.Point(24, 168);
			this.tbPassphrase.Name = "tbPassphrase";
			this.tbPassphrase.PasswordChar = '*';
			this.tbPassphrase.Size = new System.Drawing.Size(480, 21);
			this.tbPassphrase.TabIndex = 3;
			this.tbPassphrase.Text = "";
			// 
			// lPassphraseConfirmation
			// 
			this.lPassphraseConfirmation.Location = new System.Drawing.Point(24, 200);
			this.lPassphraseConfirmation.Name = "lPassphraseConfirmation";
			this.lPassphraseConfirmation.Size = new System.Drawing.Size(216, 16);
			this.lPassphraseConfirmation.TabIndex = 2;
			this.lPassphraseConfirmation.Text = "Passphrase confirmation";
			// 
			// lPassphrase
			// 
			this.lPassphrase.Location = new System.Drawing.Point(24, 152);
			this.lPassphrase.Name = "lPassphrase";
			this.lPassphrase.Size = new System.Drawing.Size(208, 16);
			this.lPassphrase.TabIndex = 1;
			this.lPassphrase.Text = "Passphrase for conventional encryption";
			// 
			// lvKeys
			// 
			this.lvKeys.CheckBoxes = true;
			this.lvKeys.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					 this.chUserID,
																					 this.chAlgorithm,
																					 this.chBits});
			this.lvKeys.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.lvKeys.FullRowSelect = true;
			this.lvKeys.Location = new System.Drawing.Point(24, 24);
			this.lvKeys.Name = "lvKeys";
			this.lvKeys.Size = new System.Drawing.Size(480, 112);
			this.lvKeys.SmallImageList = this.imgKeys;
			this.lvKeys.TabIndex = 0;
			this.lvKeys.View = System.Windows.Forms.View.Details;
			// 
			// chUserID
			// 
			this.chUserID.Text = "User";
			this.chUserID.Width = 300;
			// 
			// chAlgorithm
			// 
			this.chAlgorithm.Text = "Key";
			this.chAlgorithm.Width = 70;
			// 
			// chBits
			// 
			this.chBits.Text = "Bits";
			this.chBits.Width = 70;
			// 
			// imgKeys
			// 
			this.imgKeys.ImageSize = new System.Drawing.Size(16, 16);
			this.imgKeys.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgKeys.ImageStream")));
			this.imgKeys.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// pFileSelect
			// 
			this.pFileSelect.Controls.Add(this.lFileSelectComment);
			this.pFileSelect.Controls.Add(this.btnBrowseFile);
			this.pFileSelect.Controls.Add(this.tbFile);
			this.pFileSelect.Controls.Add(this.lPrompt);
			this.pFileSelect.Location = new System.Drawing.Point(16, 24);
			this.pFileSelect.Name = "pFileSelect";
			this.pFileSelect.Size = new System.Drawing.Size(488, 216);
			this.pFileSelect.TabIndex = 2;
			// 
			// btnBrowseFile
			// 
			this.btnBrowseFile.Location = new System.Drawing.Point(400, 88);
			this.btnBrowseFile.Name = "btnBrowseFile";
			this.btnBrowseFile.TabIndex = 2;
			this.btnBrowseFile.Text = "Browse...";
			this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
			// 
			// tbFile
			// 
			this.tbFile.Location = new System.Drawing.Point(48, 88);
			this.tbFile.Name = "tbFile";
			this.tbFile.Size = new System.Drawing.Size(352, 21);
			this.tbFile.TabIndex = 1;
			this.tbFile.Text = "";
			// 
			// lPrompt
			// 
			this.lPrompt.Location = new System.Drawing.Point(48, 72);
			this.lPrompt.Name = "lPrompt";
			this.lPrompt.Size = new System.Drawing.Size(312, 16);
			this.lPrompt.TabIndex = 0;
			this.lPrompt.Text = "Please select the source file:";
			// 
			// pOperationSelect
			// 
			this.pOperationSelect.Controls.Add(this.lIWantTo);
			this.pOperationSelect.Controls.Add(this.rbUnprotect);
			this.pOperationSelect.Controls.Add(this.rbProtect);
			this.pOperationSelect.Location = new System.Drawing.Point(288, 144);
			this.pOperationSelect.Name = "pOperationSelect";
			this.pOperationSelect.Size = new System.Drawing.Size(296, 285);
			this.pOperationSelect.TabIndex = 0;
			// 
			// lIWantTo
			// 
			this.lIWantTo.Location = new System.Drawing.Point(136, 64);
			this.lIWantTo.Name = "lIWantTo";
			this.lIWantTo.Size = new System.Drawing.Size(100, 16);
			this.lIWantTo.TabIndex = 2;
			this.lIWantTo.Text = "I want to...";
			// 
			// rbUnprotect
			// 
			this.rbUnprotect.Location = new System.Drawing.Point(152, 120);
			this.rbUnprotect.Name = "rbUnprotect";
			this.rbUnprotect.Size = new System.Drawing.Size(312, 24);
			this.rbUnprotect.TabIndex = 1;
			this.rbUnprotect.Text = "Process the PGP-protected file";
			// 
			// rbProtect
			// 
			this.rbProtect.Checked = true;
			this.rbProtect.Location = new System.Drawing.Point(152, 88);
			this.rbProtect.Name = "rbProtect";
			this.rbProtect.Size = new System.Drawing.Size(312, 24);
			this.rbProtect.TabIndex = 0;
			this.rbProtect.TabStop = true;
			this.rbProtect.Text = "Protect the existing file using PGP functionality";
			// 
			// pKeyringSelect
			// 
			this.pKeyringSelect.Controls.Add(this.btnBrowseSec);
			this.pKeyringSelect.Controls.Add(this.btnBrowsePub);
			this.pKeyringSelect.Controls.Add(this.tbSecKeyring);
			this.pKeyringSelect.Controls.Add(this.tbPubKeyring);
			this.pKeyringSelect.Controls.Add(this.lSecKeyring);
			this.pKeyringSelect.Controls.Add(this.lPubKeyring);
			this.pKeyringSelect.Location = new System.Drawing.Point(112, 32);
			this.pKeyringSelect.Name = "pKeyringSelect";
			this.pKeyringSelect.Size = new System.Drawing.Size(224, 293);
			this.pKeyringSelect.TabIndex = 1;
			// 
			// btnBrowseSec
			// 
			this.btnBrowseSec.Location = new System.Drawing.Point(400, 136);
			this.btnBrowseSec.Name = "btnBrowseSec";
			this.btnBrowseSec.TabIndex = 5;
			this.btnBrowseSec.Text = "Browse...";
			this.btnBrowseSec.Click += new System.EventHandler(this.btnBrowseSec_Click);
			// 
			// btnBrowsePub
			// 
			this.btnBrowsePub.Location = new System.Drawing.Point(400, 80);
			this.btnBrowsePub.Name = "btnBrowsePub";
			this.btnBrowsePub.TabIndex = 4;
			this.btnBrowsePub.Text = "Browse...";
			this.btnBrowsePub.Click += new System.EventHandler(this.btnBrowsePub_Click);
			// 
			// tbSecKeyring
			// 
			this.tbSecKeyring.Location = new System.Drawing.Point(48, 136);
			this.tbSecKeyring.Name = "tbSecKeyring";
			this.tbSecKeyring.Size = new System.Drawing.Size(352, 21);
			this.tbSecKeyring.TabIndex = 3;
			this.tbSecKeyring.Text = "";
			// 
			// tbPubKeyring
			// 
			this.tbPubKeyring.Location = new System.Drawing.Point(48, 80);
			this.tbPubKeyring.Name = "tbPubKeyring";
			this.tbPubKeyring.Size = new System.Drawing.Size(352, 21);
			this.tbPubKeyring.TabIndex = 2;
			this.tbPubKeyring.Text = "";
			// 
			// lSecKeyring
			// 
			this.lSecKeyring.Location = new System.Drawing.Point(48, 120);
			this.lSecKeyring.Name = "lSecKeyring";
			this.lSecKeyring.Size = new System.Drawing.Size(100, 16);
			this.lSecKeyring.TabIndex = 1;
			this.lSecKeyring.Text = "Secret keyring file:";
			// 
			// lPubKeyring
			// 
			this.lPubKeyring.Location = new System.Drawing.Point(48, 64);
			this.lPubKeyring.Name = "lPubKeyring";
			this.lPubKeyring.Size = new System.Drawing.Size(128, 16);
			this.lPubKeyring.TabIndex = 0;
			this.lPubKeyring.Text = "Public keyring file:";
			// 
			// pNavigation
			// 
			this.pNavigation.Controls.Add(this.btnBack);
			this.pNavigation.Controls.Add(this.btnCancel);
			this.pNavigation.Controls.Add(this.btnNext);
			this.pNavigation.Controls.Add(this.gbSeparator);
			this.pNavigation.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pNavigation.Location = new System.Drawing.Point(0, 253);
			this.pNavigation.Name = "pNavigation";
			this.pNavigation.Size = new System.Drawing.Size(528, 56);
			this.pNavigation.TabIndex = 0;
			// 
			// btnBack
			// 
			this.btnBack.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnBack.Location = new System.Drawing.Point(264, 16);
			this.btnBack.Name = "btnBack";
			this.btnBack.TabIndex = 4;
			this.btnBack.Text = "< Back";
			this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(440, 16);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnNext
			// 
			this.btnNext.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnNext.Location = new System.Drawing.Point(344, 16);
			this.btnNext.Name = "btnNext";
			this.btnNext.TabIndex = 2;
			this.btnNext.Text = "Next >";
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// gbSeparator
			// 
			this.gbSeparator.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.gbSeparator.Location = new System.Drawing.Point(-8, -32);
			this.gbSeparator.Name = "gbSeparator";
			this.gbSeparator.Size = new System.Drawing.Size(568, 40);
			this.gbSeparator.TabIndex = 1;
			this.gbSeparator.TabStop = false;
			// 
			// keyring
			// 
			this.keyring.SaveSecretKeySignatures = false;
			// 
			// pgpReader
			// 
			this.pgpReader.DecryptingKeys = null;
			this.pgpReader.VerifyingKeys = null;
			this.pgpReader.OnPassphrase += new SBPGPStreams.TSBPGPPassphraseEvent(this.pgpReader_OnPassphrase);
			this.pgpReader.OnProgress += new SBPGP.TSBPGPProgressEvent(this.pgpReader_OnProgress);
			this.pgpReader.OnCreateOutputStream += new SBPGP.TSBPGPCreateOutputStreamEvent(this.pgpReader_OnCreateOutputStream);
			this.pgpReader.OnSignatures += new SBPGPStreams.TSBPGPSignaturesEvent(this.pgpReader_OnSignatures);
			this.pgpReader.OnKeyPassphrase += new SBPGPStreams.TSBPGPKeyPassphraseEvent(this.pgpReader_OnKeyPassphrase);
			// 
			// pgpWriter
			// 
			this.pgpWriter.Armor = false;
			this.pgpWriter.ArmorBoundary = null;
			this.pgpWriter.Compress = false;
			this.pgpWriter.CompressionLevel = 9;
			this.pgpWriter.EncryptingKeys = null;
			this.pgpWriter.EncryptionType = SBPGP.TSBPGPEncryptionType.etPublicKey;
			this.pgpWriter.Filename = null;
			this.pgpWriter.InputIsText = false;
			this.pgpWriter.Protection = SBPGPConstants.TSBPGPProtectionType.ptNormal;
			this.pgpWriter.SignBufferingMethod = SBPGP.TSBPGPSignBufferingMethod.sbmRewind;
			this.pgpWriter.SigningKeys = null;
			this.pgpWriter.TextCompatibilityMode = true;
			this.pgpWriter.Timestamp = new System.DateTime(((long)(0)));
			this.pgpWriter.UseNewFeatures = true;
			this.pgpWriter.UseOldPackets = false;
			this.pgpWriter.OnProgress += new SBPGP.TSBPGPProgressEvent(this.pgpWriter_OnProgress);
			this.pgpWriter.OnKeyPassphrase += new SBPGPStreams.TSBPGPKeyPassphraseEvent(this.pgpWriter_OnKeyPassphrase);
			// 
			// pubKeyring
			// 
			this.pubKeyring.SaveSecretKeySignatures = false;
			// 
			// secKeyring
			// 
			this.secKeyring.SaveSecretKeySignatures = false;
			// 
			// openKeyringDialog
			// 
			this.openKeyringDialog.Filter = "PGP Keyring Files (*.pkr, *.skr, *.pgp, *.gpg, *.asc)|*.PKR;*.SKR;*.PGP;*.GPG;*.A" +
				"SC";
			// 
			// lFileSelectComment
			// 
			this.lFileSelectComment.Location = new System.Drawing.Point(48, 136);
			this.lFileSelectComment.Name = "lFileSelectComment";
			this.lFileSelectComment.Size = new System.Drawing.Size(424, 64);
			this.lFileSelectComment.TabIndex = 3;
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(528, 357);
			this.Controls.Add(this.pMain);
			this.Controls.Add(this.pHints);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmMain";
			this.Text = "PGPFiles Demo Application";
			this.pHints.ResumeLayout(false);
			this.pMain.ResumeLayout(false);
			this.pClient.ResumeLayout(false);
			this.pEncOps.ResumeLayout(false);
			this.pFinish.ResumeLayout(false);
			this.pProgress.ResumeLayout(false);
			this.pUserSelect.ResumeLayout(false);
			this.pFileSelect.ResumeLayout(false);
			this.pOperationSelect.ResumeLayout(false);
			this.pKeyringSelect.ResumeLayout(false);
			this.pNavigation.ResumeLayout(false);
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
			Application.Run(new frmMain());
		}

		private System.Int16 GetPrevState(System.Int16 state)
		{
			System.Int16 result;
			switch(state) 
			{
				case STATE_SELECT_OPERATION : 
					result = STATE_INVALID;
					break;
				case STATE_PROTECT_SELECT_KEYRING :
					result = STATE_SELECT_OPERATION;
					break;
				case STATE_PROTECT_SELECT_SOURCE :
					result = STATE_PROTECT_SELECT_KEYRING;
					break;
				case STATE_PROTECT_SELECT_OPERATION:
					result = STATE_PROTECT_SELECT_SOURCE;
					break;
				case STATE_PROTECT_SELECT_DESTINATION:
					if (cbSign.Checked) 
					{
						result = STATE_PROTECT_SELECT_SIGNERS;
					} 
					else 
					{
						result = STATE_PROTECT_SELECT_RECIPIENTS;
					}
					break;
				case STATE_PROTECT_SELECT_RECIPIENTS:
					result = STATE_PROTECT_SELECT_OPERATION;
					break;
				case STATE_PROTECT_SELECT_SIGNERS:
					if (cbEncrypt.Checked) 
					{
						result = STATE_PROTECT_SELECT_RECIPIENTS;
					} 
					else 
					{
						result = STATE_PROTECT_SELECT_OPERATION;
					}
					break;
				case STATE_DECRYPT_SELECT_KEYRING:
					result = STATE_SELECT_OPERATION;
					break;
				case STATE_DECRYPT_SELECT_SOURCE:
					result = STATE_DECRYPT_SELECT_KEYRING;
					break;
				default:
					result = STATE_INVALID;
					break;
			}
			return result;
		}

		private void ChangeState(System.Int16 nextState)
		{
			this.pEncOps.Visible = false;
			this.pFileSelect.Visible = false;
			this.pOperationSelect.Visible = false;
			this.pKeyringSelect.Visible = false;
			this.pUserSelect.Visible = false;
			this.pProgress.Visible = false;
			this.pFinish.Visible = false;
			switch (nextState) 
			{
				case STATE_SELECT_OPERATION:
					SetCaption("Preparatory stage", "Choose desired operation");
					EnableButtons(false, true);
					EnableView(this.pOperationSelect);
					break;
				case STATE_PROTECT_SELECT_KEYRING:
					SetCaption("Step 1 of 6", "Select keyring files");
					EnableButtons(true, true);
					EnableView(this.pKeyringSelect);
					break;
				case STATE_PROTECT_SELECT_SOURCE:
					SetCaption("Step 2 of 6", "Select file to protect");
					lFileSelectComment.Text = "";
					EnableButtons(true, true);
					EnableView(this.pFileSelect);
					break;
				case STATE_PROTECT_SELECT_OPERATION:
					SetCaption("Step 3 of 6", "Select protection options");
					EnableButtons(true, true);
					this.cbProtLevel.SelectedIndex = 0;
					this.cbEncryptionAlg.SelectedIndex = 0;

					if (keyring.SecretCount == 0) 
					{
						this.cbSign.Checked = false;
						this.cbSign.Enabled = false;
					} 
					else 
					{
                        this.cbSign.Enabled = true;
					}

					if (keyring.PublicCount == 0)
					{
                        this.cbEncrypt.Checked = false;
						this.cbEncrypt.Enabled = false;
					} 
					else 
					{
						this.cbEncrypt.Enabled = true;
					}

					EnableView(this.pEncOps);
					break;
				case STATE_PROTECT_SELECT_RECIPIENTS:
					SetCaption("Step 4 of 6", "Select recipients");
					EnableButtons(true, true);
					KeysToList(true);
					this.tbPassphrase.Visible = this.cbUseConvEnc.Checked;
					this.tbPassphraseConf.Visible = this.cbUseConvEnc.Checked;
					this.lPassphrase.Visible = this.cbUseConvEnc.Checked;
					this.lPassphraseConfirmation.Visible = this.cbUseConvEnc.Checked;
					EnableView(this.pUserSelect);
					break;
				case STATE_PROTECT_SELECT_SIGNERS:
					SetCaption("Step 4 of 6", "Select signers");
					EnableButtons(true, true);
					KeysToList(false);
					this.tbPassphrase.Visible = false;
					this.tbPassphraseConf.Visible = false;
					this.lPassphrase.Visible = false;
					this.lPassphraseConfirmation.Visible = false;
					EnableView(this.pUserSelect);
					break;
				case STATE_PROTECT_SELECT_DESTINATION:
					SetCaption("Step 5 of 6", "Select destination file");
					lFileSelectComment.Text = "";
					this.tbFile.Text = source + ".pgp";
					EnableButtons(true, true);
					EnableView(this.pFileSelect);
					break;
				case STATE_PROTECT_PROGRESS:
					SetCaption("Step 6 of 6", "Protecting file");
					EnableButtons(false, false);
					this.pbProgress.Value = 0;
					this.lProcessingFile.Visible = true;
					this.pbProgress.Visible = true;
					this.lErrorComment.Text = "";
					this.lFinished.Text = "Operation successfully finished";
					EnableView(this.pProgress);
					try 
					{
						ProtectFile(source, this.tbFile.Text);
					} 
					catch (Exception ex) 
					{
						lErrorComment.Text = ex.Message;
						lFinished.Text = "ERROR";
					}
					this.pbProgress.Visible = false;
					this.lProcessingFile.Visible = false;
					ChangeState(STATE_PROTECT_FINISH);
					break;
				case STATE_PROTECT_FINISH:
					SetCaption("End of Work", "Protection finished");
					EnableButtons(false, false);
					EnableView(pFinish);
					btnSignatures.Visible = false;
					btnCancel.Text = "Finish";
					break;
				case STATE_DECRYPT_SELECT_KEYRING:
					SetCaption("Step 1 of 3", "Select keyring files");
					EnableButtons(true, true);
					EnableView(this.pKeyringSelect);
					break;
				case STATE_DECRYPT_SELECT_SOURCE:
					SetCaption("Step 2 of 3", "Select PGP-protected file");
					if (keyring.SecretCount == 0) 
					{
						lFileSelectComment.Text = "Your keyring does not contain private keys. You will not be able to decrypt encrypted files.";
					} 
					else 
					{
                        lFileSelectComment.Text = "";
					}
					EnableButtons(true, true);
					EnableView(this.pFileSelect);
					break;
				case STATE_DECRYPT_PROGRESS:
					SetCaption("Step 3 of 3", "Extracting protected data");
					EnableButtons(false, false);
					this.pbProgress.Value = 0;
					this.lProcessingFile.Visible = true;
					this.pbProgress.Visible = true;
					this.lErrorComment.Text = "";
					this.lFinished.Text = "Operation successfully finished";
					EnableView(this.pProgress);
					try 
					{
						DecryptFile(source);
					} 
					catch (Exception ex) 
					{
						lErrorComment.Text = ex.Message;
						lFinished.Text = "ERROR";
					}
					
					this.pbProgress.Visible = false;
					this.lProcessingFile.Visible = false;
					ChangeState(STATE_DECRYPT_FINISH);
					break;
				case STATE_DECRYPT_FINISH:
					SetCaption("End of Work", "Decryption finished");
					EnableButtons(false, false);
					EnableView(pFinish);
					btnSignatures.Visible = true;
					btnSignatures.Enabled = ((sigs != null) && (sigs.Length > 0)); 
					btnCancel.Text = "Finish";
					break;
			}
			this.state = nextState;
		}

		private void SetCaption(string Stage, string Comment) 
		{
			this.lStage.Text = Stage;
			this.lStageComment.Text = Comment;
		}

		private void EnableButtons(bool Back, bool Next) 
		{
			this.btnBack.Enabled = Back;
			this.btnNext.Enabled = Next;
		}

		private void EnableView(Panel p)
		{
			p.Dock = DockStyle.Fill;
			p.Visible = true;
		}

		private void KeysToList(bool PublicKeys) 
		{
			int i;
			string name, alg;
			ListViewItem item;
			lvKeys.Items.Clear();
			if (PublicKeys) 
			{
				for(i = 0; i < keyring.PublicCount; i++) 
				{
					if (keyring.get_PublicKeys(i).UserIDCount > 0) 
					{
						name = keyring.get_PublicKeys(i).get_UserIDs(0).Name;
					} 
					else 
					{
						name = "<no name>";
					}
					item = lvKeys.Items.Add(name);
					alg = SBPGPUtils.Unit.PKAlg2Str(keyring.get_PublicKeys(i).PublicKeyAlgorithm);
					item.SubItems.Add(alg);
					item.SubItems.Add(keyring.get_PublicKeys(i).BitsInKey.ToString());
					if ((keyring.get_PublicKeys(i).PublicKeyAlgorithm == SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA) ||
						(keyring.get_PublicKeys(i).PublicKeyAlgorithm == SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA_ENCRYPT) ||
						(keyring.get_PublicKeys(i).PublicKeyAlgorithm == SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA_SIGN)) 
					{
						item.ImageIndex = 1;
					} 
					else 
					{
						item.ImageIndex = 0;
					}
					item.Tag = keyring.get_PublicKeys(i);
				}
			} 
			else 
			{
				for(i = 0; i < keyring.SecretCount; i++) 
				{
					if (keyring.get_SecretKeys(i).PublicKey.UserIDCount > 0) 
					{
						name = keyring.get_SecretKeys(i).PublicKey.get_UserIDs(0).Name;
					} 
					else 
					{
						name = "<no name>";
					}
					item = lvKeys.Items.Add(name);
					alg = SBPGPUtils.Unit.PKAlg2Str(keyring.get_SecretKeys(i).PublicKeyAlgorithm);
					item.SubItems.Add(alg);
					item.SubItems.Add(keyring.get_SecretKeys(i).BitsInKey.ToString());
					if ((keyring.get_SecretKeys(i).PublicKeyAlgorithm == SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA) ||
						(keyring.get_SecretKeys(i).PublicKeyAlgorithm == SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA_ENCRYPT) ||
						(keyring.get_SecretKeys(i).PublicKeyAlgorithm == SBPGPConstants.Unit.SB_PGP_ALGORITHM_PK_RSA_SIGN)) 
					{
						item.ImageIndex = 1;
					} 
					else 
					{
						item.ImageIndex = 0;
					}
					item.Tag = keyring.get_SecretKeys(i);
				}
			}
		}

		private void Back() 
		{
			ChangeState(GetPrevState(state));
		}

		private void Next()
		{
			int i;
			switch(state) 
			{
				case STATE_SELECT_OPERATION:
					if (rbProtect.Checked) 
					{
						ChangeState(STATE_PROTECT_SELECT_KEYRING);
					} 
					else 
					{
						ChangeState(STATE_DECRYPT_SELECT_KEYRING);
					}
					break;

				case STATE_PROTECT_SELECT_KEYRING:
				case STATE_DECRYPT_SELECT_KEYRING:
					if (!System.IO.File.Exists(this.tbPubKeyring.Text)) 
					{
						MessageBox.Show("Keyring file '" + this.tbPubKeyring.Text + "' not found", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
					} 
					/*
					else if (!System.IO.File.Exists(this.tbSecKeyring.Text)) 
					{
						MessageBox.Show("Keyring file '" + this.tbSecKeyring.Text + "' not found", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
					} 
                    */
					else 
					{
						try 
						{
							keyring.Load(this.tbPubKeyring.Text, this.tbSecKeyring.Text, true);
						} 
						catch (Exception ex) 
						{
							MessageBox.Show("Failed to load keyring: " + ex.Message, "Keyring error", MessageBoxButtons.OK, MessageBoxIcon.Error);
							break;
						}
						if (state == STATE_PROTECT_SELECT_KEYRING) 
						{
							ChangeState(STATE_PROTECT_SELECT_SOURCE);
						} 
						else 
						{
							ChangeState(STATE_DECRYPT_SELECT_SOURCE);
						}
					}
					break;
				case STATE_PROTECT_SELECT_SOURCE:
					if (!System.IO.File.Exists(this.tbFile.Text)) 
					{
						MessageBox.Show("Source file '" + this.tbFile.Text + "' not found", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
					} 
					else 
					{
						source = this.tbFile.Text;
						ChangeState(STATE_PROTECT_SELECT_OPERATION);
					}
					break;
				case STATE_PROTECT_SELECT_OPERATION:
					if (!(cbEncrypt.Checked | cbSign.Checked)) 
					{	
						MessageBox.Show("Please select protection operation", "Operation not selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
					} 
					else 
					{
						if (cbEncrypt.Checked) 
						{
							ChangeState(STATE_PROTECT_SELECT_RECIPIENTS);
						} 
						else 
						{
							ChangeState(STATE_PROTECT_SELECT_SIGNERS);
						}
					}
					break;
				case STATE_PROTECT_SELECT_RECIPIENTS:
					pubKeyring.Clear();
					for (i = 0; i < lvKeys.Items.Count; i++) 
					{
						if (lvKeys.Items[i].Checked) 
						{
							pubKeyring.AddPublicKey((SBPGPKeys.TElPGPPublicKey)lvKeys.Items[i].Tag);
						}
					}
					if ((pubKeyring.PublicCount == 0) && (!cbUseConvEnc.Checked))
					{
						MessageBox.Show("At least one recipient key must be selected", "No selected keys", MessageBoxButtons.OK, MessageBoxIcon.Error);
					} 
					else 
					{
						if (cbSign.Checked) 
						{
							ChangeState(STATE_PROTECT_SELECT_SIGNERS);
						} 
						else 
						{
							ChangeState(STATE_PROTECT_SELECT_DESTINATION);
						}
					}
					break;
				case STATE_PROTECT_SELECT_SIGNERS:
					secKeyring.Clear();
					for (i = 0; i < lvKeys.Items.Count; i++) 
					{
						if (lvKeys.Items[i].Checked) 
						{
							secKeyring.AddSecretKey((SBPGPKeys.TElPGPSecretKey)lvKeys.Items[i].Tag);
						}
					}
					if (secKeyring.PublicCount == 0) 
					{
						MessageBox.Show("At least one signer's key must be selected", "No selected keys", MessageBoxButtons.OK, MessageBoxIcon.Error);
					} 
					else 
					{
						ChangeState(STATE_PROTECT_SELECT_DESTINATION);
					}
					break;
				case STATE_PROTECT_SELECT_DESTINATION:
					ChangeState(STATE_PROTECT_PROGRESS);
					break;
				case STATE_DECRYPT_SELECT_SOURCE:
					if (!System.IO.File.Exists(this.tbFile.Text)) 
					{
						MessageBox.Show("Source file '" + this.tbFile.Text + "' not found", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
					} 
					else 
					{
						source = this.tbFile.Text;
						ChangeState(STATE_DECRYPT_PROGRESS);
					}
					break;
			}
		}

		private void ProtectFile(string SourceFile, string DestFile) 
		{
			System.IO.FileStream inF, outF;
			System.IO.FileInfo info;

			pgpWriter.Armor = true;
			pgpWriter.ArmorHeaders.Clear();
			pgpWriter.ArmorHeaders.Add("Version: EldoS PGPBlackbox (.NET edition)");
			pgpWriter.ArmorBoundary = "PGP MESSAGE";
			pgpWriter.Compress = cbCompress.Checked;
			pgpWriter.EncryptingKeys = pubKeyring;
			pgpWriter.SigningKeys = secKeyring;
			if ((cbUseConvEnc.Checked) && (pubKeyring.PublicCount > 0)) 
			{
				pgpWriter.EncryptionType = SBPGP.TSBPGPEncryptionType.etBoth;
			} 
			else if ((cbUseConvEnc.Checked) && (pubKeyring.PublicCount == 0)) 
			{
				pgpWriter.EncryptionType = SBPGP.TSBPGPEncryptionType.etPassphrase;
			} 
			else 
			{
				pgpWriter.EncryptionType = SBPGP.TSBPGPEncryptionType.etPublicKey;
			}
			info = new System.IO.FileInfo(SourceFile);
			pgpWriter.Filename = info.Name;
			pgpWriter.InputIsText = cbTextInput.Checked;
			pgpWriter.Passphrases.Clear();
			pgpWriter.Passphrases.Add(tbPassphrase.Text);
			switch(cbProtLevel.SelectedIndex) {
				case 0 : 
					pgpWriter.Protection = SBPGPConstants.TSBPGPProtectionType.ptLow;
					break;
				case 1 :
					pgpWriter.Protection = SBPGPConstants.TSBPGPProtectionType.ptNormal;
					break;
				default:
					pgpWriter.Protection = SBPGPConstants.TSBPGPProtectionType.ptHigh;
					break;
			}
			pgpWriter.SignBufferingMethod = SBPGP.TSBPGPSignBufferingMethod.sbmRewind;
			switch (this.cbEncryptionAlg.SelectedIndex) 
			{
				case 0 : 
					pgpWriter.SymmetricKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_SK_CAST5;
					break;
				case 1 :
					pgpWriter.SymmetricKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_SK_3DES;
					break;
				case 2 :
					pgpWriter.SymmetricKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_SK_AES128;
					break;
				default :
					pgpWriter.SymmetricKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_SK_AES256;
					break;
			}
			pgpWriter.Timestamp = DateTime.Now;
			pgpWriter.UseNewFeatures = cbUseNewFeatures.Checked;
			pgpWriter.UseOldPackets = false;

			inF = new System.IO.FileStream(source, FileMode.Open);
			try 
			{
				outF = new System.IO.FileStream(this.tbFile.Text, FileMode.Create);
				try 
				{
					if ((!cbEncrypt.Checked) && (cbSign.Checked) && (cbTextInput.Checked))
					{
						pgpWriter.ClearTextSign(inF, outF, 0);
					} 
					else if ((cbEncrypt.Checked) && (cbSign.Checked)) 
					{
						pgpWriter.EncryptAndSign(inF, outF, 0);
					} 
					else if ((cbEncrypt.Checked) && (!cbSign.Checked))
					{
						pgpWriter.Encrypt(inF, outF, 0);
					} 
					else 
					{
						pgpWriter.Sign(inF, outF, false, 0);
					}
				} 
				finally 
				{
					outF.Close();
				}
			} 
			finally 
			{
				inF.Close();
			}
		}

		private void DecryptFile(string SourceFile)
		{
			System.IO.FileStream inF;

			pgpReader.DecryptingKeys = keyring;
			pgpReader.VerifyingKeys = keyring;

			inF = new System.IO.FileStream(SourceFile, FileMode.Open);
			try 
			{
				pgpReader.DecryptAndVerify(inF, 0);
			} 
			finally 
			{
				inF.Close();
			}
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
			return result;
		}

		private void btnBack_Click(object sender, System.EventArgs e)
		{
			Back();
		}

		private void btnNext_Click(object sender, System.EventArgs e)
		{
			Next();
		}

		private void cbEncrypt_CheckedChanged(object sender, System.EventArgs e)
		{
			this.cbCompress.Enabled = cbEncrypt.Checked;
			this.cbEncryptionAlg.Enabled = cbEncrypt.Checked;
			this.cbProtLevel.Enabled = cbEncrypt.Checked;
			this.cbUseConvEnc.Enabled = cbEncrypt.Checked;
			this.lProtLevel.Enabled = cbEncrypt.Checked;
			this.lEncryptionAlg.Enabled = cbEncrypt.Checked;
		}

		private void cbSign_CheckedChanged(object sender, System.EventArgs e)
		{
			this.cbTextInput.Enabled = cbSign.Checked;
		}

		private void btnBrowseFile_Click(object sender, System.EventArgs e)
		{
			if ((state == STATE_PROTECT_SELECT_SOURCE) ||
				(state == STATE_DECRYPT_SELECT_SOURCE)) 
			{
				if (openFileDialog.ShowDialog() == DialogResult.OK) 
				{
					this.tbFile.Text = openFileDialog.FileName;
				}
			} 
			else if (state == STATE_PROTECT_SELECT_DESTINATION)
			{
				if (saveFileDialog.ShowDialog() == DialogResult.OK)
				{
					this.tbFile.Text = saveFileDialog.FileName;
				}
			}
		}

		private void pgpWriter_OnKeyPassphrase(object Sender, SBPGPKeys.TElPGPCustomSecretKey Key, ref string Passphrase, ref bool Cancel)
		{
			string pass;
			pass = RequestKeyPassphrase(Key, ref Cancel);
			Passphrase = pass;
		}

		private void pgpWriter_OnProgress(object Sender, int Processed, int Total, ref bool Cancel)
		{
			if (Total != 0) 
			{
				pbProgress.Value = (int)((System.Double)Processed * 100 / (System.Double)Total);
			}
			System.Windows.Forms.Application.DoEvents();
		}

		private void btnBrowsePub_Click(object sender, System.EventArgs e)
		{
			openKeyringDialog.Title = "Select public keyring file";
			if (openKeyringDialog.ShowDialog() == DialogResult.OK) 
			{
				this.tbPubKeyring.Text = openKeyringDialog.FileName;
			}
		}

		private void btnBrowseSec_Click(object sender, System.EventArgs e)
		{
			openKeyringDialog.Title = "Select secret keyring file";
			if (openKeyringDialog.ShowDialog() == DialogResult.OK) 
			{
				this.tbSecKeyring.Text = openKeyringDialog.FileName;
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void pgpReader_OnKeyPassphrase(object Sender, SBPGPKeys.TElPGPCustomSecretKey Key, ref string Passphrase, ref bool Cancel)
		{
			string pass;
			pass = RequestKeyPassphrase(Key, ref Cancel);
			Passphrase = pass;
		}

		private void pgpReader_OnPassphrase(object Sender, ref string Passphrase, ref bool Cancel)
		{
			string pass;
			pass = RequestKeyPassphrase(null, ref Cancel);
			Passphrase = pass;
		}

		private void pgpReader_OnProgress(object Sender, int Processed, int Total, ref bool Cancel)
		{
			if (Total != 0) 
			{
				pbProgress.Value = (int)((System.Double)Processed * 100 / (System.Double)Total);
			} 
			System.Windows.Forms.Application.DoEvents();
		}

		private void pgpReader_OnCreateOutputStream(object Sender, string Filename, System.DateTime TimeStamp, ref System.IO.Stream Stream, ref bool FreeOnExit)
		{
			/*
			 * The '_CONSOLE' filename should be handled in different way 
			 * (for-your-eyes-only message), but we do not consider this case here.
			 */
			saveFileDialog.FileName = Filename;
			if (saveFileDialog.ShowDialog() == DialogResult.OK) 
			{
				Stream = new System.IO.FileStream(saveFileDialog.FileName, FileMode.Create);
			} 
			else 
			{
				Stream = new System.IO.MemoryStream();
			}
			FreeOnExit = true;
		}

		private void pgpReader_OnSignatures(object Sender, SBPGPKeys.TElPGPSignature[] Signatures, SBPGPStreams.TSBPGPSignatureValidity[] Validities)
		{
			int i;
			SBPGPKeys.TElPGPSignature sig;
			sigs = new SBPGPKeys.TElPGPSignature[Signatures.Length];
			vals = new SBPGPStreams.TSBPGPSignatureValidity[Signatures.Length];
			for(i = 0; i < Signatures.Length; i++) 
			{
				sig = new SBPGPKeys.TElPGPSignature();
				sig.Assign(Signatures[i]);
				sigs[i] = sig;
				vals[i] = Validities[i];
			}
		}

		private void btnSignatures_Click(object sender, System.EventArgs e)
		{
			frmSignatures dlg = new frmSignatures();
			dlg.Init(this.sigs, this.vals, keyring);
			dlg.ShowDialog();
		}
	}
}
