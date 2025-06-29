using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using SBSSHKeyStorage;

namespace SSHKeys
{
	/// <summary>
	/// Main application form
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ToolBar tbTop;
		private System.Windows.Forms.ToolBarButton tbGenerate;
		private System.Windows.Forms.ToolBarButton tbSavePrivate;
		private System.Windows.Forms.ToolBarButton tbSavePublic;
		private System.Windows.Forms.ToolBarButton tbLoadPrivate;
		private System.Windows.Forms.ToolBarButton tbLoadPublic;
		private System.Windows.Forms.ToolBarButton tbExit;
		private System.Windows.Forms.RadioButton rbDSS;
		private System.Windows.Forms.RadioButton rbRSA;
		private System.Windows.Forms.GroupBox rgAlgorithm;
		public System.Windows.Forms.Label lblPrivateKey;
		public System.Windows.Forms.Label lblPublicKey;
		public System.Windows.Forms.TextBox memPrivateKey;
		public System.Windows.Forms.TextBox memPublicKey;
		public System.Windows.Forms.StatusBar sbStatus;
		public System.Windows.Forms.Panel pnlTop;
		public System.Windows.Forms.Label lblKeyLen;
		public System.Windows.Forms.Label lblSubject;
		public System.Windows.Forms.Label lblComment;
		public System.Windows.Forms.ComboBox cbKeyLen;
		public System.Windows.Forms.TextBox edtSubject;
		public System.Windows.Forms.TextBox edtComment;
		private System.Windows.Forms.Splitter splBottom;
		private System.Windows.Forms.RadioButton rbOpenSSH;
		private System.Windows.Forms.RadioButton rbIETF;
		private System.Windows.Forms.RadioButton rbPutty;
		private System.Windows.Forms.RadioButton rbX509;
		private System.Windows.Forms.GroupBox gbKeyFormat;
		private System.ComponentModel.IContainer components;
		// Components
		private SBSSHKeyStorage.TElSSHKey FKey = new TElSSHKey();
		private System.Windows.Forms.OpenFileDialog odKeys;
		private System.Windows.Forms.SaveFileDialog sdKeys;

		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			// Initialization
			cbKeyLen.SelectedIndex = 2;
			SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"));
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
			this.tbTop = new System.Windows.Forms.ToolBar();
			this.tbGenerate = new System.Windows.Forms.ToolBarButton();
			this.tbSavePrivate = new System.Windows.Forms.ToolBarButton();
			this.tbSavePublic = new System.Windows.Forms.ToolBarButton();
			this.tbLoadPrivate = new System.Windows.Forms.ToolBarButton();
			this.tbLoadPublic = new System.Windows.Forms.ToolBarButton();
			this.tbExit = new System.Windows.Forms.ToolBarButton();
			this.rgAlgorithm = new System.Windows.Forms.GroupBox();
			this.rbRSA = new System.Windows.Forms.RadioButton();
			this.rbDSS = new System.Windows.Forms.RadioButton();
			this.gbKeyFormat = new System.Windows.Forms.GroupBox();
			this.rbX509 = new System.Windows.Forms.RadioButton();
			this.rbPutty = new System.Windows.Forms.RadioButton();
			this.rbOpenSSH = new System.Windows.Forms.RadioButton();
			this.rbIETF = new System.Windows.Forms.RadioButton();
			this.lblPrivateKey = new System.Windows.Forms.Label();
			this.lblPublicKey = new System.Windows.Forms.Label();
			this.memPrivateKey = new System.Windows.Forms.TextBox();
			this.memPublicKey = new System.Windows.Forms.TextBox();
			this.sbStatus = new System.Windows.Forms.StatusBar();
			this.pnlTop = new System.Windows.Forms.Panel();
			this.lblKeyLen = new System.Windows.Forms.Label();
			this.lblSubject = new System.Windows.Forms.Label();
			this.lblComment = new System.Windows.Forms.Label();
			this.cbKeyLen = new System.Windows.Forms.ComboBox();
			this.edtSubject = new System.Windows.Forms.TextBox();
			this.edtComment = new System.Windows.Forms.TextBox();
			this.splBottom = new System.Windows.Forms.Splitter();
			this.odKeys = new System.Windows.Forms.OpenFileDialog();
			this.sdKeys = new System.Windows.Forms.SaveFileDialog();
			this.rgAlgorithm.SuspendLayout();
			this.gbKeyFormat.SuspendLayout();
			this.pnlTop.SuspendLayout();
			this.SuspendLayout();
			// 
			// tbTop
			// 
			this.tbTop.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.tbTop.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																					 this.tbGenerate,
																					 this.tbSavePrivate,
																					 this.tbSavePublic,
																					 this.tbLoadPrivate,
																					 this.tbLoadPublic,
																					 this.tbExit});
			this.tbTop.ButtonSize = new System.Drawing.Size(40, 22);
			this.tbTop.Cursor = System.Windows.Forms.Cursors.Default;
			this.tbTop.DropDownArrows = true;
			this.tbTop.Location = new System.Drawing.Point(0, 0);
			this.tbTop.Name = "tbTop";
			this.tbTop.ShowToolTips = true;
			this.tbTop.Size = new System.Drawing.Size(680, 28);
			this.tbTop.TabIndex = 0;
			this.tbTop.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.tbTop.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbTop_ButtonClick);
			// 
			// tbGenerate
			// 
			this.tbGenerate.Text = "&Generate key";
			this.tbGenerate.ToolTipText = "Press to generate new key";
			// 
			// tbSavePrivate
			// 
			this.tbSavePrivate.Enabled = false;
			this.tbSavePrivate.Text = "Save &private key";
			this.tbSavePrivate.ToolTipText = "Press to save generated private key";
			// 
			// tbSavePublic
			// 
			this.tbSavePublic.Enabled = false;
			this.tbSavePublic.Text = "Save pub&lic key";
			this.tbSavePublic.ToolTipText = "Press to save generated public key";
			// 
			// tbLoadPrivate
			// 
			this.tbLoadPrivate.Text = "Load private key";
			// 
			// tbLoadPublic
			// 
			this.tbLoadPublic.Text = "Load public key";
			// 
			// tbExit
			// 
			this.tbExit.Text = "E&xit";
			this.tbExit.ToolTipText = "Exit from demo";
			// 
			// rgAlgorithm
			// 
			this.rgAlgorithm.Controls.Add(this.rbRSA);
			this.rgAlgorithm.Controls.Add(this.rbDSS);
			this.rgAlgorithm.Dock = System.Windows.Forms.DockStyle.Top;
			this.rgAlgorithm.Location = new System.Drawing.Point(0, 28);
			this.rgAlgorithm.Name = "rgAlgorithm";
			this.rgAlgorithm.Size = new System.Drawing.Size(680, 36);
			this.rgAlgorithm.TabIndex = 1;
			this.rgAlgorithm.TabStop = false;
			this.rgAlgorithm.Text = "&Algorithm";
			// 
			// rbRSA
			// 
			this.rbRSA.Checked = true;
			this.rbRSA.Location = new System.Drawing.Point(40, 16);
			this.rbRSA.Name = "rbRSA";
			this.rbRSA.Size = new System.Drawing.Size(104, 16);
			this.rbRSA.TabIndex = 0;
			this.rbRSA.TabStop = true;
			this.rbRSA.Text = "RSA";
			// 
			// rbDSS
			// 
			this.rbDSS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.rbDSS.Location = new System.Drawing.Point(504, 16);
			this.rbDSS.Name = "rbDSS";
			this.rbDSS.Size = new System.Drawing.Size(104, 16);
			this.rbDSS.TabIndex = 0;
			this.rbDSS.Text = "DSS";
			// 
			// gbKeyFormat
			// 
			this.gbKeyFormat.Controls.Add(this.rbX509);
			this.gbKeyFormat.Controls.Add(this.rbPutty);
			this.gbKeyFormat.Controls.Add(this.rbOpenSSH);
			this.gbKeyFormat.Controls.Add(this.rbIETF);
			this.gbKeyFormat.Dock = System.Windows.Forms.DockStyle.Top;
			this.gbKeyFormat.Location = new System.Drawing.Point(0, 64);
			this.gbKeyFormat.Name = "gbKeyFormat";
			this.gbKeyFormat.Size = new System.Drawing.Size(680, 36);
			this.gbKeyFormat.TabIndex = 3;
			this.gbKeyFormat.TabStop = false;
			this.gbKeyFormat.Text = "Key &format";
			// 
			// rbX509
			// 
			this.rbX509.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.rbX509.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rbX509.Location = new System.Drawing.Point(600, 16);
			this.rbX509.Name = "rbX509";
			this.rbX509.Size = new System.Drawing.Size(56, 16);
			this.rbX509.TabIndex = 2;
			this.rbX509.Text = "X.509";
			this.rbX509.CheckedChanged += new System.EventHandler(this.rbPutty_CheckedChanged);
			// 
			// rbPutty
			// 
			this.rbPutty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.rbPutty.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rbPutty.Location = new System.Drawing.Point(408, 16);
			this.rbPutty.Name = "rbPutty";
			this.rbPutty.Size = new System.Drawing.Size(64, 16);
			this.rbPutty.TabIndex = 1;
			this.rbPutty.Text = "PuTTY";
			this.rbPutty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.rbPutty.CheckedChanged += new System.EventHandler(this.rbPutty_CheckedChanged);
			// 
			// rbOpenSSH
			// 
			this.rbOpenSSH.Checked = true;
			this.rbOpenSSH.Location = new System.Drawing.Point(16, 16);
			this.rbOpenSSH.Name = "rbOpenSSH";
			this.rbOpenSSH.Size = new System.Drawing.Size(104, 16);
			this.rbOpenSSH.TabIndex = 0;
			this.rbOpenSSH.TabStop = true;
			this.rbOpenSSH.Text = "OpenSSH";
			this.rbOpenSSH.CheckedChanged += new System.EventHandler(this.rbPutty_CheckedChanged);
			// 
			// rbIETF
			// 
			this.rbIETF.Location = new System.Drawing.Point(208, 16);
			this.rbIETF.Name = "rbIETF";
			this.rbIETF.Size = new System.Drawing.Size(56, 16);
			this.rbIETF.TabIndex = 0;
			this.rbIETF.Text = "IETF";
			this.rbIETF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.rbIETF.CheckedChanged += new System.EventHandler(this.rbPutty_CheckedChanged);
			// 
			// lblPrivateKey
			// 
			this.lblPrivateKey.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblPrivateKey.Location = new System.Drawing.Point(0, 155);
			this.lblPrivateKey.Name = "lblPrivateKey";
			this.lblPrivateKey.Size = new System.Drawing.Size(680, 13);
			this.lblPrivateKey.TabIndex = 4;
			this.lblPrivateKey.Text = "Private key";
			this.lblPrivateKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblPublicKey
			// 
			this.lblPublicKey.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblPublicKey.Location = new System.Drawing.Point(0, 298);
			this.lblPublicKey.Name = "lblPublicKey";
			this.lblPublicKey.Size = new System.Drawing.Size(680, 13);
			this.lblPublicKey.TabIndex = 8;
			this.lblPublicKey.Text = "Public key";
			this.lblPublicKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// memPrivateKey
			// 
			this.memPrivateKey.AcceptsReturn = true;
			this.memPrivateKey.AutoSize = false;
			this.memPrivateKey.Dock = System.Windows.Forms.DockStyle.Fill;
			this.memPrivateKey.Location = new System.Drawing.Point(0, 168);
			this.memPrivateKey.Multiline = true;
			this.memPrivateKey.Name = "memPrivateKey";
			this.memPrivateKey.Size = new System.Drawing.Size(680, 266);
			this.memPrivateKey.TabIndex = 5;
			this.memPrivateKey.Text = "Press Generate key to create a new key or load from the file";
			// 
			// memPublicKey
			// 
			this.memPublicKey.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.memPublicKey.Location = new System.Drawing.Point(0, 314);
			this.memPublicKey.Multiline = true;
			this.memPublicKey.Name = "memPublicKey";
			this.memPublicKey.Size = new System.Drawing.Size(680, 120);
			this.memPublicKey.TabIndex = 8;
			this.memPublicKey.Text = "Press Generate key to create a new key or load from the file";
			// 
			// sbStatus
			// 
			this.sbStatus.Location = new System.Drawing.Point(0, 434);
			this.sbStatus.Name = "sbStatus";
			this.sbStatus.Size = new System.Drawing.Size(680, 19);
			this.sbStatus.TabIndex = 9;
			// 
			// pnlTop
			// 
			this.pnlTop.BackColor = System.Drawing.SystemColors.Control;
			this.pnlTop.Controls.Add(this.lblKeyLen);
			this.pnlTop.Controls.Add(this.lblSubject);
			this.pnlTop.Controls.Add(this.lblComment);
			this.pnlTop.Controls.Add(this.cbKeyLen);
			this.pnlTop.Controls.Add(this.edtSubject);
			this.pnlTop.Controls.Add(this.edtComment);
			this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTop.Location = new System.Drawing.Point(0, 100);
			this.pnlTop.Name = "pnlTop";
			this.pnlTop.Size = new System.Drawing.Size(680, 55);
			this.pnlTop.TabIndex = 3;
			// 
			// lblKeyLen
			// 
			this.lblKeyLen.Location = new System.Drawing.Point(5, 8);
			this.lblKeyLen.Name = "lblKeyLen";
			this.lblKeyLen.Size = new System.Drawing.Size(91, 13);
			this.lblKeyLen.TabIndex = 0;
			this.lblKeyLen.Text = "Key length in bits";
			// 
			// lblSubject
			// 
			this.lblSubject.Location = new System.Drawing.Point(176, 8);
			this.lblSubject.Name = "lblSubject";
			this.lblSubject.Size = new System.Drawing.Size(104, 13);
			this.lblSubject.TabIndex = 1;
			this.lblSubject.Text = "Subject (IETF only)";
			// 
			// lblComment
			// 
			this.lblComment.Location = new System.Drawing.Point(5, 32);
			this.lblComment.Name = "lblComment";
			this.lblComment.Size = new System.Drawing.Size(59, 13);
			this.lblComment.TabIndex = 2;
			this.lblComment.Text = "Comment";
			// 
			// cbKeyLen
			// 
			this.cbKeyLen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbKeyLen.ItemHeight = 13;
			this.cbKeyLen.Items.AddRange(new object[] {
														  "128",
														  "256",
														  "512",
														  "1024",
														  "2048",
														  "4096"});
			this.cbKeyLen.Location = new System.Drawing.Point(104, 5);
			this.cbKeyLen.Name = "cbKeyLen";
			this.cbKeyLen.Size = new System.Drawing.Size(60, 21);
			this.cbKeyLen.TabIndex = 0;
			// 
			// edtSubject
			// 
			this.edtSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.edtSubject.BackColor = System.Drawing.SystemColors.GrayText;
			this.edtSubject.Enabled = false;
			this.edtSubject.Location = new System.Drawing.Point(288, 5);
			this.edtSubject.Name = "edtSubject";
			this.edtSubject.Size = new System.Drawing.Size(384, 20);
			this.edtSubject.TabIndex = 1;
			this.edtSubject.Text = "";
			// 
			// edtComment
			// 
			this.edtComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.edtComment.Location = new System.Drawing.Point(104, 29);
			this.edtComment.Name = "edtComment";
			this.edtComment.Size = new System.Drawing.Size(568, 20);
			this.edtComment.TabIndex = 2;
			this.edtComment.Text = "";
			// 
			// splBottom
			// 
			this.splBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splBottom.Location = new System.Drawing.Point(0, 311);
			this.splBottom.Name = "splBottom";
			this.splBottom.Size = new System.Drawing.Size(680, 3);
			this.splBottom.TabIndex = 7;
			this.splBottom.TabStop = false;
			// 
			// odKeys
			// 
			this.odKeys.AddExtension = false;
			// 
			// sdKeys
			// 
			this.sdKeys.AddExtension = false;
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(680, 453);
			this.Controls.Add(this.lblPublicKey);
			this.Controls.Add(this.splBottom);
			this.Controls.Add(this.memPublicKey);
			this.Controls.Add(this.memPrivateKey);
			this.Controls.Add(this.lblPrivateKey);
			this.Controls.Add(this.sbStatus);
			this.Controls.Add(this.pnlTop);
			this.Controls.Add(this.gbKeyFormat);
			this.Controls.Add(this.rgAlgorithm);
			this.Controls.Add(this.tbTop);
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SSH keys demo";
			this.rgAlgorithm.ResumeLayout(false);
			this.gbKeyFormat.ResumeLayout(false);
			this.pnlTop.ResumeLayout(false);
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

		#region Helper functions
		/// <summary>
		/// Put status into status bar and update form
		/// </summary>
		/// <param name="AStatus"></param>
		private void SetStatus(String AStatus)
		{
			sbStatus.Text = AStatus;
			this.Update();
		}

		/// <summary>
		/// Show status after call to Generate, Load, etc..
		/// </summary>
		/// <param name="AStatus"></param>
		private void ShowStatus(int AStatus)
		{
			switch(AStatus)
			{
				case (0): 
				{
					SetStatus("OK");
					break;
				}
				case(SBSSHKeyStorage.Unit.SB_ERROR_SSH_KEYS_INVALID_PUBLIC_KEY) :
				{
					SetStatus("SB_ERROR_SSH_KEYS_INVALID_PUBLIC_KEY");
					break;
				}
				case(SBSSHKeyStorage.Unit.SB_ERROR_SSH_KEYS_INVALID_PRIVATE_KEY) :
				{
					SetStatus("SB_ERROR_SSH_KEYS_INVALID_PRIVATE_KEY");
					break;
				}
				case(SBSSHKeyStorage.Unit.SB_ERROR_SSH_KEYS_FILE_READ_ERROR) :
				{
					SetStatus("SB_ERROR_SSH_KEYS_FILE_READ_ERROR");
					break;
				}
				case(SBSSHKeyStorage.Unit.SB_ERROR_SSH_KEYS_FILE_WRITE_ERROR) :
				{
					SetStatus("SB_ERROR_SSH_KEYS_FILE_WRITE_ERROR");
					break;
				}
				case(SBSSHKeyStorage.Unit.SB_ERROR_SSH_KEYS_UNSUPPORTED_ALGORITHM) :
				{
					SetStatus("SB_ERROR_SSH_KEYS_UNSUPPORTED_ALGORITHM");
					break;
				}
				case(SBSSHKeyStorage.Unit.SB_ERROR_SSH_KEYS_INTERNAL_ERROR) :
				{
					SetStatus("SB_ERROR_SSH_KEYS_INTERNAL_ERROR");
					break;
				}
				case(SBSSHKeyStorage.Unit.SB_ERROR_SSH_KEYS_BUFFER_TOO_SMALL) :
				{
					SetStatus("SB_ERROR_SSH_KEYS_BUFFER_TOO_SMALL");
					break;
				}
				case(SBSSHKeyStorage.Unit.SB_ERROR_SSH_KEYS_NO_PRIVATE_KEY) :
				{
					SetStatus("SB_ERROR_SSH_KEYS_NO_PRIVATE_KEY");
					break;
				}
				case(SBSSHKeyStorage.Unit.SB_ERROR_SSH_KEYS_INVALID_PASSPHRASE) :
				{
					SetStatus("SB_ERROR_SSH_KEYS_INVALID_PASSPHRASE");
					break;
				}
				case(SBSSHKeyStorage.Unit.SB_ERROR_SSH_KEYS_UNSUPPORTED_PEM_ALGORITHM) :
				{
					SetStatus("SB_ERROR_SSH_KEYS_UNSUPPORTED_PEM_ALGORITHM");
					break;
				}
			}
		}

		/// <summary>
		/// Show generated keys into 
		/// </summary>
		private void ShowKeys()
		{
			try
			{
				byte[] key = new byte[16384];
				int KeyLen = 0;
				FKey.SavePrivateKey(ref key,ref KeyLen,"test");
				key = new byte[KeyLen];
				FKey.SavePrivateKey(ref key,ref KeyLen,"");
				ASCIIEncoding encoder = new ASCIIEncoding();
				memPrivateKey.Text = encoder.GetString(key).Replace("\n","\r\n");
				KeyLen=0;
				FKey.SavePublicKey(ref key,ref KeyLen);
				key = new byte[KeyLen];
				FKey.SavePublicKey(ref key,ref KeyLen);
				memPublicKey.Text = encoder.GetString(key).Replace("\n","\r\n");
				edtSubject.Text = FKey.Subject;
				edtComment.Text = FKey.Comment;
				key=null;
			} 
			catch(Exception e) {this.SetStatus("Error saving key " + e.Message);}
		}

		/// <summary>
		/// Allow saving key in a case of generation
		/// </summary>
		private void AllowKeySaving()
		{
			tbSavePrivate.Enabled = true;
			tbSavePublic.Enabled = true;
		}

		#endregion

		/// <summary>
		/// Generate new key
		/// </summary>
		private void GenerateKey()
		{
			// Generation algorithm
			if (rbRSA.Checked) FKey.Algorithm=SBSSHKeyStorage.Unit.ALGORITHM_RSA;
				else FKey.Algorithm=SBSSHKeyStorage.Unit.ALGORITHM_DSS;
			// Key format
			if (rbOpenSSH.Checked) FKey.KeyFormat=TSBSSHKeyFormat.kfOpenSSH;
			if (rbIETF.Checked) FKey.KeyFormat=TSBSSHKeyFormat.kfIETF;
			if (rbPutty.Checked) FKey.KeyFormat=TSBSSHKeyFormat.kfPuTTY;
			if (rbX509.Checked) FKey.KeyFormat=TSBSSHKeyFormat.kfX509;
			//
			FKey.Comment = edtComment.Text;
			FKey.Subject = edtSubject.Text;
			int Bits = 0;
			try
			{
				Bits = int.Parse(cbKeyLen.Text);
			}
			catch(Exception) {SetStatus("Invalid key length"); return;}
			SetStatus("Please wait...Generating key...");
			int Status = FKey.Generate(FKey.Algorithm,Bits);
			ShowStatus(Status);
			if (Status!=0) return;
			ShowKeys();
			AllowKeySaving();
		}

		private void tbTop_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			switch (tbTop.Buttons.IndexOf(e.Button))
			{
				case 0 : {GenerateKey(); break;}
				case 1 : 
				{
					sdKeys.Title = "Save private key";
					if (sdKeys.ShowDialog()!= DialogResult.OK) return;
					String pwd = "";
					TfrmGetPassword.GetPassword(ref pwd);
					FKey.SavePrivateKey(sdKeys.FileName,pwd);
					break;
				}
				case 2 : 
				{
					sdKeys.Title = "Save public key";
					if (sdKeys.ShowDialog()!= DialogResult.OK) return;
					FKey.SavePublicKey(sdKeys.FileName);
					break;
				}
				case 3 :
				{
					odKeys.Title = "Open private key";
					if (odKeys.ShowDialog()!= DialogResult.OK) return;
					int Status = FKey.LoadPrivateKey(odKeys.FileName,"");
					if (Status != 0)
					{
						String pwd = "";
						TfrmGetPassword.GetPassword(ref pwd);
						Status = FKey.LoadPrivateKey(odKeys.FileName,pwd);
					}
					ShowStatus(Status);
					if (Status==0) ShowKeys();
					break;
				}
				case 4 :
				{
					odKeys.Title = "Open public key";
					if (odKeys.ShowDialog()!= DialogResult.OK) return;
					int Status = FKey.LoadPublicKey(odKeys.FileName);
					ShowStatus(Status);
					if (Status==0) ShowKeys();
					break;
				}
				case 5 : 
				{ 
					Close(); 
					break;
				}
			}
		}

		private void rbPutty_CheckedChanged(object sender, System.EventArgs e)
		{
			if (rbIETF.Checked)
			{
				edtSubject.Enabled = true;
				edtSubject.BackColor = SystemColors.Window;
			} 
			else
			{
				edtSubject.Enabled = false;
				edtSubject.BackColor = SystemColors.GrayText;
			}
		}


	}
}
