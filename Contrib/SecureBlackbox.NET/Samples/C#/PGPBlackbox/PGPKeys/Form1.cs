using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using SBPGPKeys;

namespace PGPKeysDemo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private TElPGPKeyring keyring;
		private string pubKeyringFile;
		private string secKeyringFile;
		private System.Windows.Forms.ToolBar tbToolbar;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem mnuFile;
		private System.Windows.Forms.MenuItem mnuOpen;
		private System.Windows.Forms.MenuItem mnuSave;
		private System.Windows.Forms.MenuItem mnuBreak;
		private System.Windows.Forms.MenuItem mnuQuit;
		private System.Windows.Forms.MenuItem mnuHelp;
		private System.Windows.Forms.MenuItem mnuAbout;
		private System.Windows.Forms.ToolBarButton tbGenerate;
		private System.Windows.Forms.ToolBarButton tbDelim1;
		private System.Windows.Forms.ToolBarButton tbAddKey;
		private System.Windows.Forms.ToolBarButton tbRemoveKey;
		private System.Windows.Forms.StatusBarPanel sbpMain;
		private System.Windows.Forms.Panel pItemInfo;
		private System.Windows.Forms.TreeView tvKeyring;
		private System.Windows.Forms.ToolBarButton tbLoadKeyring;
		private System.Windows.Forms.ToolBarButton tbSaveKeyring;
		private System.Windows.Forms.ToolBarButton tbNewKeyring;
		private System.Windows.Forms.ToolBarButton tbDelim2;
		private System.Windows.Forms.ToolBarButton tbDelim3;
		private System.Windows.Forms.ImageList imgTreeView;
		private System.Windows.Forms.Panel pKeyInfo;
		private System.Windows.Forms.Label lKeyID;
		private System.Windows.Forms.Label lKeyFP;
		private System.Windows.Forms.Label lKeyAlgorithm;
		private System.Windows.Forms.Label lTimestamp;
		private System.Windows.Forms.Label lExpires;
		private System.Windows.Forms.Label lTrust;
		private System.Windows.Forms.Panel pUserInfo;
		private System.Windows.Forms.PictureBox picUser;
		private System.Windows.Forms.Label lUserName;
		private System.Windows.Forms.Panel pSigInfo;
		private System.Windows.Forms.Label lSigner;
		private System.Windows.Forms.Label lSigCreated;
		private System.Windows.Forms.Label lValidity;
		private System.Windows.Forms.Label lSigType;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.ToolBarButton tbExportKey;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.ToolBarButton tbDelim4;
		private System.Windows.Forms.ToolBarButton tbSign;
		private System.Windows.Forms.ToolBarButton tbRevoke;
		private System.Windows.Forms.ImageList imgToolbar;
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
			keyring = new TElPGPKeyring();
			HideAllInfoPanels();
			Status("Application started");
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
			this.tbToolbar = new System.Windows.Forms.ToolBar();
			this.tbNewKeyring = new System.Windows.Forms.ToolBarButton();
			this.tbDelim1 = new System.Windows.Forms.ToolBarButton();
			this.tbLoadKeyring = new System.Windows.Forms.ToolBarButton();
			this.tbSaveKeyring = new System.Windows.Forms.ToolBarButton();
			this.tbDelim2 = new System.Windows.Forms.ToolBarButton();
			this.tbGenerate = new System.Windows.Forms.ToolBarButton();
			this.tbDelim3 = new System.Windows.Forms.ToolBarButton();
			this.tbAddKey = new System.Windows.Forms.ToolBarButton();
			this.tbRemoveKey = new System.Windows.Forms.ToolBarButton();
			this.tbExportKey = new System.Windows.Forms.ToolBarButton();
			this.tbDelim4 = new System.Windows.Forms.ToolBarButton();
			this.tbSign = new System.Windows.Forms.ToolBarButton();
			this.tbRevoke = new System.Windows.Forms.ToolBarButton();
			this.imgToolbar = new System.Windows.Forms.ImageList(this.components);
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.mnuFile = new System.Windows.Forms.MenuItem();
			this.mnuOpen = new System.Windows.Forms.MenuItem();
			this.mnuSave = new System.Windows.Forms.MenuItem();
			this.mnuBreak = new System.Windows.Forms.MenuItem();
			this.mnuQuit = new System.Windows.Forms.MenuItem();
			this.mnuHelp = new System.Windows.Forms.MenuItem();
			this.mnuAbout = new System.Windows.Forms.MenuItem();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.sbpMain = new System.Windows.Forms.StatusBarPanel();
			this.pItemInfo = new System.Windows.Forms.Panel();
			this.pSigInfo = new System.Windows.Forms.Panel();
			this.lSigType = new System.Windows.Forms.Label();
			this.lValidity = new System.Windows.Forms.Label();
			this.lSigCreated = new System.Windows.Forms.Label();
			this.lSigner = new System.Windows.Forms.Label();
			this.pUserInfo = new System.Windows.Forms.Panel();
			this.lUserName = new System.Windows.Forms.Label();
			this.picUser = new System.Windows.Forms.PictureBox();
			this.pKeyInfo = new System.Windows.Forms.Panel();
			this.lTrust = new System.Windows.Forms.Label();
			this.lExpires = new System.Windows.Forms.Label();
			this.lTimestamp = new System.Windows.Forms.Label();
			this.lKeyAlgorithm = new System.Windows.Forms.Label();
			this.lKeyFP = new System.Windows.Forms.Label();
			this.lKeyID = new System.Windows.Forms.Label();
			this.tvKeyring = new System.Windows.Forms.TreeView();
			this.imgTreeView = new System.Windows.Forms.ImageList(this.components);
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.sbpMain)).BeginInit();
			this.pItemInfo.SuspendLayout();
			this.pSigInfo.SuspendLayout();
			this.pUserInfo.SuspendLayout();
			this.pKeyInfo.SuspendLayout();
			this.SuspendLayout();
			// 
			// tbToolbar
			// 
			this.tbToolbar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						 this.tbNewKeyring,
																						 this.tbDelim1,
																						 this.tbLoadKeyring,
																						 this.tbSaveKeyring,
																						 this.tbDelim2,
																						 this.tbGenerate,
																						 this.tbDelim3,
																						 this.tbAddKey,
																						 this.tbRemoveKey,
																						 this.tbExportKey,
																						 this.tbDelim4,
																						 this.tbSign,
																						 this.tbRevoke});
			this.tbToolbar.DropDownArrows = true;
			this.tbToolbar.ImageList = this.imgToolbar;
			this.tbToolbar.Location = new System.Drawing.Point(0, 0);
			this.tbToolbar.Name = "tbToolbar";
			this.tbToolbar.ShowToolTips = true;
			this.tbToolbar.Size = new System.Drawing.Size(680, 28);
			this.tbToolbar.TabIndex = 0;
			this.tbToolbar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbToolbar_ButtonClick);
			// 
			// tbNewKeyring
			// 
			this.tbNewKeyring.ImageIndex = 0;
			this.tbNewKeyring.ToolTipText = "New keyring";
			// 
			// tbDelim1
			// 
			this.tbDelim1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tbLoadKeyring
			// 
			this.tbLoadKeyring.ImageIndex = 1;
			this.tbLoadKeyring.ToolTipText = "Load keyring";
			// 
			// tbSaveKeyring
			// 
			this.tbSaveKeyring.ImageIndex = 2;
			this.tbSaveKeyring.ToolTipText = "Save keyring";
			// 
			// tbDelim2
			// 
			this.tbDelim2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tbGenerate
			// 
			this.tbGenerate.ImageIndex = 3;
			this.tbGenerate.ToolTipText = "Generate a key";
			// 
			// tbDelim3
			// 
			this.tbDelim3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tbAddKey
			// 
			this.tbAddKey.ImageIndex = 4;
			this.tbAddKey.ToolTipText = "Import a key";
			// 
			// tbRemoveKey
			// 
			this.tbRemoveKey.ImageIndex = 5;
			this.tbRemoveKey.ToolTipText = "Remove a key from keyring";
			// 
			// tbExportKey
			// 
			this.tbExportKey.ImageIndex = 6;
			this.tbExportKey.ToolTipText = "Export a key";
			// 
			// tbDelim4
			// 
			this.tbDelim4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tbSign
			// 
			this.tbSign.ImageIndex = 7;
			this.tbSign.ToolTipText = "Sign selected";
			// 
			// tbRevoke
			// 
			this.tbRevoke.ImageIndex = 8;
			this.tbRevoke.ToolTipText = "Revoke selected";
			// 
			// imgToolbar
			// 
			this.imgToolbar.ImageSize = new System.Drawing.Size(16, 16);
			this.imgToolbar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgToolbar.ImageStream")));
			this.imgToolbar.TransparentColor = System.Drawing.Color.Blue;
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuFile,
																					 this.mnuHelp});
			// 
			// mnuFile
			// 
			this.mnuFile.Index = 0;
			this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuOpen,
																					this.mnuSave,
																					this.mnuBreak,
																					this.mnuQuit});
			this.mnuFile.Text = "File";
			// 
			// mnuOpen
			// 
			this.mnuOpen.Index = 0;
			this.mnuOpen.Text = "Load keyring...";
			this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
			// 
			// mnuSave
			// 
			this.mnuSave.Index = 1;
			this.mnuSave.Text = "Save keyring...";
			this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
			// 
			// mnuBreak
			// 
			this.mnuBreak.Index = 2;
			this.mnuBreak.Text = "-";
			// 
			// mnuQuit
			// 
			this.mnuQuit.Index = 3;
			this.mnuQuit.Text = "Quit";
			this.mnuQuit.Click += new System.EventHandler(this.mnuQuit_Click);
			// 
			// mnuHelp
			// 
			this.mnuHelp.Index = 1;
			this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuAbout});
			this.mnuHelp.Text = "Help";
			// 
			// mnuAbout
			// 
			this.mnuAbout.Index = 0;
			this.mnuAbout.Text = "About...";
			this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 443);
			this.statusBar.Name = "statusBar";
			this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						 this.sbpMain});
			this.statusBar.ShowPanels = true;
			this.statusBar.Size = new System.Drawing.Size(680, 22);
			this.statusBar.TabIndex = 1;
			this.statusBar.Text = "StatusBar";
			// 
			// sbpMain
			// 
			this.sbpMain.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.sbpMain.Width = 664;
			// 
			// pItemInfo
			// 
			this.pItemInfo.Controls.Add(this.pSigInfo);
			this.pItemInfo.Controls.Add(this.pUserInfo);
			this.pItemInfo.Controls.Add(this.pKeyInfo);
			this.pItemInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pItemInfo.Location = new System.Drawing.Point(0, 323);
			this.pItemInfo.Name = "pItemInfo";
			this.pItemInfo.Size = new System.Drawing.Size(680, 120);
			this.pItemInfo.TabIndex = 2;
			// 
			// pSigInfo
			// 
			this.pSigInfo.Controls.Add(this.lSigType);
			this.pSigInfo.Controls.Add(this.lValidity);
			this.pSigInfo.Controls.Add(this.lSigCreated);
			this.pSigInfo.Controls.Add(this.lSigner);
			this.pSigInfo.Location = new System.Drawing.Point(32, 8);
			this.pSigInfo.Name = "pSigInfo";
			this.pSigInfo.Size = new System.Drawing.Size(112, 100);
			this.pSigInfo.TabIndex = 2;
			// 
			// lSigType
			// 
			this.lSigType.Location = new System.Drawing.Point(16, 16);
			this.lSigType.Name = "lSigType";
			this.lSigType.Size = new System.Drawing.Size(312, 16);
			this.lSigType.TabIndex = 3;
			this.lSigType.Text = "Type:";
			// 
			// lValidity
			// 
			this.lValidity.Location = new System.Drawing.Point(16, 88);
			this.lValidity.Name = "lValidity";
			this.lValidity.Size = new System.Drawing.Size(320, 16);
			this.lValidity.TabIndex = 2;
			this.lValidity.Text = "Validity:";
			// 
			// lSigCreated
			// 
			this.lSigCreated.Location = new System.Drawing.Point(16, 64);
			this.lSigCreated.Name = "lSigCreated";
			this.lSigCreated.Size = new System.Drawing.Size(320, 16);
			this.lSigCreated.TabIndex = 1;
			this.lSigCreated.Text = "Created:";
			// 
			// lSigner
			// 
			this.lSigner.Location = new System.Drawing.Point(16, 40);
			this.lSigner.Name = "lSigner";
			this.lSigner.Size = new System.Drawing.Size(320, 16);
			this.lSigner.TabIndex = 0;
			this.lSigner.Text = "Signer:";
			// 
			// pUserInfo
			// 
			this.pUserInfo.Controls.Add(this.lUserName);
			this.pUserInfo.Controls.Add(this.picUser);
			this.pUserInfo.Location = new System.Drawing.Point(464, 0);
			this.pUserInfo.Name = "pUserInfo";
			this.pUserInfo.Size = new System.Drawing.Size(136, 120);
			this.pUserInfo.TabIndex = 1;
			// 
			// lUserName
			// 
			this.lUserName.Location = new System.Drawing.Point(16, 16);
			this.lUserName.Name = "lUserName";
			this.lUserName.Size = new System.Drawing.Size(608, 16);
			this.lUserName.TabIndex = 1;
			this.lUserName.Text = "User name:";
			// 
			// picUser
			// 
			this.picUser.Location = new System.Drawing.Point(8, 8);
			this.picUser.Name = "picUser";
			this.picUser.Size = new System.Drawing.Size(112, 104);
			this.picUser.TabIndex = 0;
			this.picUser.TabStop = false;
			// 
			// pKeyInfo
			// 
			this.pKeyInfo.Controls.Add(this.lTrust);
			this.pKeyInfo.Controls.Add(this.lExpires);
			this.pKeyInfo.Controls.Add(this.lTimestamp);
			this.pKeyInfo.Controls.Add(this.lKeyAlgorithm);
			this.pKeyInfo.Controls.Add(this.lKeyFP);
			this.pKeyInfo.Controls.Add(this.lKeyID);
			this.pKeyInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pKeyInfo.Location = new System.Drawing.Point(0, 0);
			this.pKeyInfo.Name = "pKeyInfo";
			this.pKeyInfo.Size = new System.Drawing.Size(680, 120);
			this.pKeyInfo.TabIndex = 0;
			// 
			// lTrust
			// 
			this.lTrust.Location = new System.Drawing.Point(304, 64);
			this.lTrust.Name = "lTrust";
			this.lTrust.Size = new System.Drawing.Size(264, 16);
			this.lTrust.TabIndex = 6;
			this.lTrust.Text = "Trust:";
			// 
			// lExpires
			// 
			this.lExpires.Location = new System.Drawing.Point(304, 40);
			this.lExpires.Name = "lExpires";
			this.lExpires.Size = new System.Drawing.Size(264, 16);
			this.lExpires.TabIndex = 5;
			this.lExpires.Text = "Expires:";
			// 
			// lTimestamp
			// 
			this.lTimestamp.Location = new System.Drawing.Point(304, 16);
			this.lTimestamp.Name = "lTimestamp";
			this.lTimestamp.Size = new System.Drawing.Size(264, 16);
			this.lTimestamp.TabIndex = 4;
			this.lTimestamp.Text = "Created:";
			// 
			// lKeyAlgorithm
			// 
			this.lKeyAlgorithm.Location = new System.Drawing.Point(16, 16);
			this.lKeyAlgorithm.Name = "lKeyAlgorithm";
			this.lKeyAlgorithm.Size = new System.Drawing.Size(280, 16);
			this.lKeyAlgorithm.TabIndex = 2;
			this.lKeyAlgorithm.Text = "Algorithm:";
			// 
			// lKeyFP
			// 
			this.lKeyFP.Location = new System.Drawing.Point(16, 64);
			this.lKeyFP.Name = "lKeyFP";
			this.lKeyFP.Size = new System.Drawing.Size(280, 16);
			this.lKeyFP.TabIndex = 1;
			this.lKeyFP.Text = "KeyFP: ";
			// 
			// lKeyID
			// 
			this.lKeyID.Location = new System.Drawing.Point(16, 40);
			this.lKeyID.Name = "lKeyID";
			this.lKeyID.Size = new System.Drawing.Size(280, 16);
			this.lKeyID.TabIndex = 0;
			this.lKeyID.Text = "KeyID:";
			// 
			// tvKeyring
			// 
			this.tvKeyring.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvKeyring.ImageList = this.imgTreeView;
			this.tvKeyring.Location = new System.Drawing.Point(0, 28);
			this.tvKeyring.Name = "tvKeyring";
			this.tvKeyring.Size = new System.Drawing.Size(680, 295);
			this.tvKeyring.TabIndex = 3;
			this.tvKeyring.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvKeyring_AfterSelect);
			// 
			// imgTreeView
			// 
			this.imgTreeView.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.imgTreeView.ImageSize = new System.Drawing.Size(16, 16);
			this.imgTreeView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgTreeView.ImageStream")));
			this.imgTreeView.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// openFileDialog
			// 
			this.openFileDialog.Title = "Open key file";
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.Title = "Save key file";
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(680, 465);
			this.Controls.Add(this.tvKeyring);
			this.Controls.Add(this.pItemInfo);
			this.Controls.Add(this.statusBar);
			this.Controls.Add(this.tbToolbar);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.Menu = this.mainMenu;
			this.Name = "frmMain";
			this.Text = "PGPKeys Demo Application";
			((System.ComponentModel.ISupportInitialize)(this.sbpMain)).EndInit();
			this.pItemInfo.ResumeLayout(false);
			this.pSigInfo.ResumeLayout(false);
			this.pUserInfo.ResumeLayout(false);
			this.pKeyInfo.ResumeLayout(false);
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

		private void tbToolbar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == tbNewKeyring) 
			{
				NewKeyring();
			} 
			else if (e.Button == tbLoadKeyring) 
			{
				LoadKeyring();
			} 
			else if (e.Button == tbSaveKeyring) 
			{
				SaveKeyring();
			} 
			else if (e.Button == tbGenerate) 
			{
				GenerateKey();
			}
			else if (e.Button == tbAddKey) 
			{
				AddKey();
			} 
			else if (e.Button == tbRemoveKey) 
			{
				RemoveKey();
			} 
			else if (e.Button == tbExportKey)
			{
				ExportKey();
			}
			else if (e.Button == tbSign) 
			{
				Sign();
			} 
			else if (e.Button == tbRevoke) 
			{
				Revoke();
			}
		}

		private void NewKeyring() 
		{
			if (MessageBox.Show("Are you sure you want to create a new keyring?\nAll unsaved information will be LOST!", 
				"New keyring", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) 
			{
				pubKeyringFile = "";
				secKeyringFile = "";
				keyring.Clear();
				HideAllInfoPanels();
				Utils.RedrawKeyring(tvKeyring, keyring);
				Status("New keyring created");
			}
		}

		private void LoadKeyring()
		{
			TElPGPKeyring tempKeyring = new TElPGPKeyring();
			frmLoadSaveKeyring dlg = new frmLoadSaveKeyring();
			dlg.OpenDialog = true;
			dlg.Text = "Load keyring";
			if (dlg.ShowDialog() == DialogResult.OK) 
			{
				try 
				{
					tempKeyring.Load(dlg.tbPublicKeyring.Text, dlg.tbSecretKeyring.Text, true);
				} 
				catch(Exception ex) 
				{
					MessageBox.Show(ex.Message, "Keyring error", MessageBoxButtons.OK,
						MessageBoxIcon.Error);
					Status("Failed to load keyring");
					return;
				}
				HideAllInfoPanels();
				pubKeyringFile = dlg.tbPublicKeyring.Text;
				secKeyringFile = dlg.tbSecretKeyring.Text;
				keyring.Clear();
				tempKeyring.ExportTo(keyring);
				Utils.RedrawKeyring(tvKeyring, keyring);
				Status("Keyring loaded");
			}
		}

		private void SaveKeyring()
		{
			frmLoadSaveKeyring dlg = new frmLoadSaveKeyring();
			dlg.OpenDialog = false;
			dlg.Text = "Save keyring";
			dlg.tbPublicKeyring.Text = pubKeyringFile;
			dlg.tbSecretKeyring.Text = secKeyringFile;
			if (dlg.ShowDialog() == DialogResult.OK) 
			{
				try 
				{
					keyring.Save(dlg.tbPublicKeyring.Text, dlg.tbSecretKeyring.Text, false);
					Status("Keyring saved");
				} 
				catch(Exception ex) 
				{
					MessageBox.Show(ex.Message, "Keyring error", MessageBoxButtons.OK,
						MessageBoxIcon.Error);
					Status("Failed to save keyring");
				}
			}
		}

		private void GenerateKey()
		{
			TElPGPSecretKey key = new TElPGPSecretKey();
			frmGenerateKey dlg = new frmGenerateKey();
			dlg.SecretKey = key;
			dlg.ShowDialog(); 
			if (dlg.Success) 
			{
				keyring.AddSecretKey(key);
				Utils.RedrawKeyring(tvKeyring, keyring);
			}
			Status("New key was added to keyring");
		}

		private void AddKey()
		{
			TElPGPKeyring tempKeyring = new TElPGPKeyring();
			frmImportKey dlg = new frmImportKey();
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				try 
				{
					tempKeyring.Load(openFileDialog.FileName, "", true);
				} 
				catch(Exception ex) 
				{
					MessageBox.Show(ex.Message, "Unable to load key", MessageBoxButtons.OK,
						MessageBoxIcon.Error);
					Status("Failed to import key");
					return;
				}
				dlg.Init(tempKeyring, imgTreeView);
				if (dlg.ShowDialog() == DialogResult.OK) 
				{
					tempKeyring.ExportTo(keyring);
					Utils.RedrawKeyring(tvKeyring, keyring);
				}
			}
			Status(tempKeyring.PublicCount.ToString() + " key(s) successfully imported");
		}

		private void RemoveKey()
		{
			TElPGPPublicKey key;
			if ((tvKeyring.SelectedNode != null) && (tvKeyring.SelectedNode.Tag is TElPGPPublicKey))
			{
				key = (TElPGPPublicKey)(tvKeyring.SelectedNode.Tag);
				if (MessageBox.Show("Are you sure you want to remove the key (" + Utils.GetDefaultUserID(key) +
					")?", "Remove key", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) 
				{
					if (key.SecretKey != null) 
					{
						if (MessageBox.Show("The key you want to remove is SECRET! Are you still sure?", "Remove key", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) 
						{
							return;
						}
					}
					keyring.RemovePublicKey(key, true);
					Utils.RedrawKeyring(tvKeyring, keyring);
					Status("Key was successfully removed");
				}
			}
		}

		private void ExportKey()
		{
			TElPGPPublicKey key;
			if ((tvKeyring.SelectedNode != null) && (tvKeyring.SelectedNode.Tag is TElPGPPublicKey))
			{
				key = (TElPGPPublicKey)(tvKeyring.SelectedNode.Tag);
				if (saveFileDialog.ShowDialog() == DialogResult.OK) 
				{
					key.SaveToFile(saveFileDialog.FileName, true);
					Status("Key saved");
				}
			}
		}

		private void About()
		{
			frmAbout dlg = new frmAbout();
			dlg.ShowDialog();
		}

		private void ExitApp()
		{
			this.Close();
		}

		private void Sign()
		{
			frmSelectKey dlg = new frmSelectKey();
			TElPGPKeyring keys = new TElPGPKeyring();
			TElPGPSignature sig;
			TreeNode newNode;
			int i;

			if ((tvKeyring.SelectedNode != null) && (tvKeyring.SelectedNode.Tag != null)) 
			{
				for(i = 0; i < keyring.SecretCount; i++) 
				{
					keys.AddSecretKey(keyring.get_SecretKeys(i));
				}
				dlg.Init(keys, imgTreeView);
				if (tvKeyring.SelectedNode.Tag is TElPGPCustomUser) 
				{
					if ((dlg.ShowDialog() == DialogResult.OK) && (dlg.lvKeys.SelectedItems.Count > 0))
					{
						if ((tvKeyring.SelectedNode.Parent != null) && (tvKeyring.SelectedNode.Parent.Tag is TElPGPPublicKey)) 
						{
							sig = SignUser((TElPGPCustomUser)(tvKeyring.SelectedNode.Tag), 
								(TElPGPPublicKey)(tvKeyring.SelectedNode.Parent.Tag), 
								(TElPGPSecretKey)(dlg.lvKeys.SelectedItems[0].Tag));
							if (sig != null) 
							{
                                ((TElPGPCustomUser)(tvKeyring.SelectedNode.Tag)).AddSignature(sig);
								newNode = tvKeyring.SelectedNode.Nodes.Add("Signature");
								newNode.Tag = sig;
								Status("Signed successfully");
							}
						}
					} 
				}
				else 
				{
					MessageBox.Show("Only User information may be signed", 
						"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void Revoke()
		{
			frmSelectKey dlg = new frmSelectKey();
			TElPGPKeyring keys = new TElPGPKeyring();
			TElPGPSignature sig;
			TreeNode newNode;
			int i;

			if ((tvKeyring.SelectedNode != null) && (tvKeyring.SelectedNode.Tag != null)) 
			{
				for(i = 0; i < keyring.SecretCount; i++) 
				{
					keys.AddSecretKey(keyring.get_SecretKeys(i));
				}
				dlg.Init(keys, imgTreeView);
				if ((tvKeyring.SelectedNode.Tag is TElPGPCustomUser) &&
					(tvKeyring.SelectedNode.Parent != null) &&
					(tvKeyring.SelectedNode.Parent.Tag is TElPGPPublicKey))
				{
					if ((dlg.ShowDialog() == DialogResult.OK) && (dlg.lvKeys.SelectedItems.Count > 0))
					{
						sig = RevokeUser((TElPGPCustomUser)(tvKeyring.SelectedNode.Tag), 
							(TElPGPPublicKey)(tvKeyring.SelectedNode.Parent.Tag), 
							(TElPGPSecretKey)(dlg.lvKeys.SelectedItems[0].Tag));
						if (sig != null) 
						{
							((TElPGPCustomUser)(tvKeyring.SelectedNode.Tag)).AddSignature(sig);
							newNode = tvKeyring.SelectedNode.Nodes.Add("Revocation");
							newNode.Tag = sig;
							Status("Revoked successfully");
						}
					} 
				}
				else 
				{
					MessageBox.Show("Only User signature information may be revoked", 
						"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
        
		private void Status(string S)
		{
			statusBar.Panels[0].Text = S;
		}

		private TElPGPSignature SignUser(TElPGPCustomUser user, TElPGPCustomPublicKey userKey,
			TElPGPSecretKey signingKey) 
		{
			TElPGPSignature sig = new TElPGPSignature();
			sig.CreationTime = DateTime.Now;
			try 
			{
				signingKey.Sign((TElPGPPublicKey)userKey, user, sig, SBPGPKeys.Unit.ctGeneric);
			} 
			catch(Exception ex) 
			{
				// Exception. Possibly, passphrase is needed.
				signingKey.Passphrase = RequestPassphrase(signingKey.PublicKey);
				try 
				{
					signingKey.Sign((TElPGPPublicKey)userKey, user, sig, SBPGPKeys.Unit.ctGeneric);
				} 
				catch(Exception exin) 
				{
					MessageBox.Show(exin.Message, "Signing error", MessageBoxButtons.OK,
						MessageBoxIcon.Error);
					return null;
				}
			}
			return sig;
		}

		private TElPGPSignature RevokeUser(TElPGPCustomUser user, TElPGPCustomPublicKey userKey,
			TElPGPSecretKey signingKey) 
		{
			TElPGPSignature sig = new TElPGPSignature();
			sig.CreationTime = DateTime.Now;
			try 
			{
				signingKey.Revoke((TElPGPPublicKey)userKey, user, sig, null);
			} 
			catch(Exception ex) 
			{
				// Exception. Possibly, passphrase is needed.
				signingKey.Passphrase = RequestPassphrase(signingKey.PublicKey);
				try 
				{
					signingKey.Revoke((TElPGPPublicKey)userKey, user, sig, null);
				} 
				catch(Exception exin) 
				{
					MessageBox.Show(exin.Message, "Signing error", MessageBoxButtons.OK,
						MessageBoxIcon.Error);
					return null;
				}
			}
			return sig;
		}

		private string RequestPassphrase(TElPGPPublicKey key)
		{
			frmPassphraseRequest dlg = new frmPassphraseRequest();
			dlg.lKeyID.Text = Utils.GetDefaultUserID(key) + " (" + SBPGPUtils.Unit.KeyID2Str(key.KeyID(), true) + ")";
			dlg.ShowDialog();
            return dlg.tbPassphrase.Text;
		}

		private void HideAllInfoPanels()
		{
			pKeyInfo.Visible = false;
			pUserInfo.Visible = false;
			pSigInfo.Visible = false;
		}

		private void EnableView(Panel p)
		{
			p.Dock = DockStyle.Fill;
			p.Visible = true;
		}

		private void DrawPublicKeyProps(TElPGPCustomPublicKey key)
		{
			HideAllInfoPanels();
			this.lKeyAlgorithm.Text = "Algorithm: " + SBPGPUtils.Unit.PKAlg2Str(key.PublicKeyAlgorithm) + " (" + key.BitsInKey.ToString() + " bits)";
			this.lKeyID.Text = "KeyID: " + SBPGPUtils.Unit.KeyID2Str(key.KeyID(), false);
			this.lKeyFP.Text = "KeyFP: " + SBPGPUtils.Unit.KeyFP2Str(key.KeyFP());
			this.lTimestamp.Text = "Created: " + key.Timestamp.ToShortDateString() + " " + key.Timestamp.ToShortTimeString();
			if (key.Expires == 0) 
			{
				this.lExpires.Text = "Expires: NEVER";
			} 
			else 
			{
				this.lExpires.Text = "Expires: " + key.Timestamp.AddDays(key.Expires).ToString();
			}
			EnableView(pKeyInfo);
		}

		private void DrawUserIDProps(TElPGPUserID user)
		{
			HideAllInfoPanels();
			picUser.Visible = false;
			lUserName.Visible = true;
			lUserName.Text = "User name: " + user.Name;
			EnableView(pUserInfo);
		}

		private void DrawUserAttrProps(TElPGPUserAttr user)
		{
			MemoryStream strm = new MemoryStream();
			HideAllInfoPanels();
			picUser.Visible = true;
			lUserName.Visible = false;
			strm.Write(user.get_Images(0).JpegData, 0, user.get_Images(0).JpegData.Length);
			strm.Position = 0;
			picUser.Image = System.Drawing.Image.FromStream(strm);
			EnableView(pUserInfo);
		}

		private void DrawSignatureProps(TElPGPSignature sig, TElPGPCustomUser user,
			TElPGPCustomPublicKey userKey)
		{
			string validity = "Unable to verify";
			TElPGPCustomPublicKey key = null;
			HideAllInfoPanels();
			keyring.FindPublicKeyByID(sig.SignerKeyID(), ref key, 0);
			
			if (key != null) 
			{
				if (key is TElPGPPublicKey) 
				{
					lSigner.Text = "Signer: " + Utils.GetDefaultUserID((TElPGPPublicKey)key);
					if (user != null) 
					{
						try 
						{
							if (sig.IsUserRevocation()) 
							{
								if (key.RevocationVerify(userKey, user, sig)) 
								{
									validity = "Valid";
								} 
								else 
								{
									validity = "INVALID";
								}
							} 
							else 
							{
								if (key.Verify(userKey, user, sig)) 
								{
									validity = "Valid";
								} 
								else 
								{
									validity = "INVALID";
								}
							}
						} 
						catch (Exception ex) 
						{
							validity = ex.Message;
						}
					} 
					else 
					{
						validity = "UserID not found";
					}
				}
				else 
				{
					lSigner.Text = "Signer: Unknown signer";
				}				
			}  
			else 
			{
				lSigner.Text = "Signer: Unknown signer";
			}
			lSigCreated.Text = sig.CreationTime.ToString();
			lValidity.Text = "Validity: " + validity;
			if (sig.IsUserRevocation()) 
			{
				lSigType.Text = "Type: User revocation";
			} 
			else 
			{
				lSigType.Text = "Type: Certification signature";
			}
			EnableView(pSigInfo);
		}

		private void DrawSignatureProps(TElPGPSignature sig, TElPGPPublicSubkey subkey,
			TElPGPCustomPublicKey userKey) 
		{
			string validity = "Unable to verify";
			HideAllInfoPanels();
			
			lSigner.Text = "Signer: " + Utils.GetDefaultUserID((TElPGPPublicKey)userKey);
			if (subkey != null) 
			{
				try 
				{
					if (sig.IsSubkeyRevocation()) 
					{
						if (userKey.RevocationVerify(subkey, sig)) 
						{
							validity = "Valid";
						} 
						else 
						{
							validity = "INVALID";
						}
					} 
					else 
					{
						if (userKey.Verify(subkey, sig)) 
						{
							validity = "Valid";
						} 
						else 
						{
							validity = "INVALID";
						}
					}
				} 
				catch (Exception ex) 
				{
					validity = ex.Message;
				}
			} 
			else 
			{
				validity = "Subkey not found";
			}
			lSigCreated.Text = sig.CreationTime.ToString();
			lValidity.Text = "Validity: " + validity;
			if (sig.IsSubkeyRevocation()) 
			{
				lSigType.Text = "Type: Subkey revocation";
			} 
			else 
			{
				lSigType.Text = "Type: Subkey binding signature";
			}
			EnableView(pSigInfo);
		}

		private void tvKeyring_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (e.Node.Tag is TElPGPCustomPublicKey) 
			{
				DrawPublicKeyProps((TElPGPCustomPublicKey)e.Node.Tag);
			} 
			else if (e.Node.Tag is TElPGPUserID) 
			{
				DrawUserIDProps((TElPGPUserID)e.Node.Tag);
			} 
			else if (e.Node.Tag is TElPGPUserAttr) 
			{
				DrawUserAttrProps((TElPGPUserAttr)e.Node.Tag);
			} 
			else if (e.Node.Tag is TElPGPSignature)
			{
				if ((e.Node.Parent != null) && (e.Node.Parent.Tag is TElPGPCustomUser) &&
					(e.Node.Parent.Parent != null) && (e.Node.Parent.Parent.Tag is TElPGPCustomPublicKey)) 
				{
					DrawSignatureProps((TElPGPSignature)e.Node.Tag, (TElPGPCustomUser)e.Node.Parent.Tag,
						(TElPGPCustomPublicKey)e.Node.Parent.Parent.Tag);
				} 
				else if ((e.Node.Parent != null) && (e.Node.Parent.Tag is TElPGPPublicSubkey) &&
					(e.Node.Parent.Parent != null) && (e.Node.Parent.Parent.Tag is TElPGPCustomPublicKey)) 
				{
                    DrawSignatureProps((TElPGPSignature)e.Node.Tag, (TElPGPPublicSubkey)e.Node.Parent.Tag,
						(TElPGPCustomPublicKey)e.Node.Parent.Parent.Tag);
				}
				else 
				{
					DrawSignatureProps((TElPGPSignature)e.Node.Tag, (TElPGPCustomUser)null, null);
				}
			}
		}

		private void mnuOpen_Click(object sender, System.EventArgs e)
		{
			LoadKeyring();
		}

		private void mnuSave_Click(object sender, System.EventArgs e)
		{
			SaveKeyring();
		}

		private void mnuQuit_Click(object sender, System.EventArgs e)
		{
			ExitApp();
		}

		private void mnuAbout_Click(object sender, System.EventArgs e)
		{
			About();
		}


	}
}
