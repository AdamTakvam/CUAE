using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using SBUtils;
using SBSftpCommon;
using SBSSHConstants;
using SBSimpleSftp;
using SBSSHKeyStorage;

namespace SimpleSftpDemo
{

	/// <summary>
	/// ElSimpleSftpClient main form
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ToolBar tbToolbar;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.ToolBarButton btnConnect;
		private System.Windows.Forms.ToolBarButton btnDisconnect;
		private System.Windows.Forms.ToolBarButton btnDelim1;
		private System.Windows.Forms.ToolBarButton btnRename;
		private System.Windows.Forms.ToolBarButton btnMakeDir;
		private System.Windows.Forms.ToolBarButton btnDelete;
		private System.Windows.Forms.ToolBarButton btnDelim2;
		private System.Windows.Forms.ToolBarButton btnDownload;
		private System.Windows.Forms.ToolBarButton btnUpload;
		private System.Windows.Forms.ToolBarButton btnDelim3;
		private System.Windows.Forms.ToolBarButton btnRefresh;
		private System.Windows.Forms.ListView lvLog;
		private System.Windows.Forms.Splitter splitter;
		private System.Windows.Forms.Panel pPath;
		private System.Windows.Forms.ListView lvFiles;
		private System.Windows.Forms.ColumnHeader chFilename;
		private System.Windows.Forms.ColumnHeader chSize;
		private System.Windows.Forms.ColumnHeader chModified;
		private System.Windows.Forms.ColumnHeader chOwner;
		private System.Windows.Forms.ColumnHeader chRights;
		private System.Windows.Forms.ColumnHeader chTime;
		private System.Windows.Forms.ColumnHeader chEvent;
		private System.Windows.Forms.MenuItem mnuConnection;
		private System.Windows.Forms.MenuItem mnuConnect;
		private System.Windows.Forms.MenuItem mnuDisconnect;
		private System.Windows.Forms.MenuItem mnuExit;
		private System.Windows.Forms.MenuItem mnuHelp;
		private System.Windows.Forms.MenuItem mnuAbout;
		private SBSimpleSftp.TElSimpleSFTPClient SftpClient;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label lPath;
		private System.Windows.Forms.ImageList imgToolbar;
		private System.Windows.Forms.ImageList imgFiles;
		private System.Windows.Forms.ImageList imgLog;
		private System.Windows.Forms.PictureBox pbDirectory;
		private string currentDir;
		private TElSSHMemoryKeyStorage KeyStorage;

		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			Initialize();
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
			this.btnConnect = new System.Windows.Forms.ToolBarButton();
			this.btnDisconnect = new System.Windows.Forms.ToolBarButton();
			this.btnDelim1 = new System.Windows.Forms.ToolBarButton();
			this.btnRename = new System.Windows.Forms.ToolBarButton();
			this.btnMakeDir = new System.Windows.Forms.ToolBarButton();
			this.btnDelete = new System.Windows.Forms.ToolBarButton();
			this.btnDelim2 = new System.Windows.Forms.ToolBarButton();
			this.btnDownload = new System.Windows.Forms.ToolBarButton();
			this.btnUpload = new System.Windows.Forms.ToolBarButton();
			this.btnDelim3 = new System.Windows.Forms.ToolBarButton();
			this.btnRefresh = new System.Windows.Forms.ToolBarButton();
			this.imgToolbar = new System.Windows.Forms.ImageList(this.components);
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.mnuConnection = new System.Windows.Forms.MenuItem();
			this.mnuConnect = new System.Windows.Forms.MenuItem();
			this.mnuDisconnect = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.mnuExit = new System.Windows.Forms.MenuItem();
			this.mnuHelp = new System.Windows.Forms.MenuItem();
			this.mnuAbout = new System.Windows.Forms.MenuItem();
			this.lvLog = new System.Windows.Forms.ListView();
			this.chTime = new System.Windows.Forms.ColumnHeader();
			this.chEvent = new System.Windows.Forms.ColumnHeader();
			this.imgLog = new System.Windows.Forms.ImageList(this.components);
			this.splitter = new System.Windows.Forms.Splitter();
			this.pPath = new System.Windows.Forms.Panel();
			this.pbDirectory = new System.Windows.Forms.PictureBox();
			this.lPath = new System.Windows.Forms.Label();
			this.lvFiles = new System.Windows.Forms.ListView();
			this.chFilename = new System.Windows.Forms.ColumnHeader();
			this.chSize = new System.Windows.Forms.ColumnHeader();
			this.chModified = new System.Windows.Forms.ColumnHeader();
			this.chOwner = new System.Windows.Forms.ColumnHeader();
			this.chRights = new System.Windows.Forms.ColumnHeader();
			this.imgFiles = new System.Windows.Forms.ImageList(this.components);
			this.SftpClient = new SBSimpleSftp.TElSimpleSFTPClient();
			this.pPath.SuspendLayout();
			this.SuspendLayout();
			// 
			// tbToolbar
			// 
			this.tbToolbar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.tbToolbar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
				this.btnConnect,
				this.btnDisconnect,
				this.btnDelim1,
				this.btnRename,
				this.btnMakeDir,
				this.btnDelete,
				this.btnDelim2,
				this.btnDownload,
				this.btnUpload,
				this.btnDelim3,
				this.btnRefresh});
			this.tbToolbar.DropDownArrows = true;
			this.tbToolbar.ImageList = this.imgToolbar;
			this.tbToolbar.Location = new System.Drawing.Point(0, 0);
			this.tbToolbar.Name = "tbToolbar";
			this.tbToolbar.ShowToolTips = true;
			this.tbToolbar.Size = new System.Drawing.Size(712, 28);
			this.tbToolbar.TabIndex = 0;
			this.tbToolbar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbToolbar_ButtonClick);
			// 
			// btnConnect
			// 
			this.btnConnect.ImageIndex = 0;
			this.btnConnect.ToolTipText = "Connect";
			// 
			// btnDisconnect
			// 
			this.btnDisconnect.ImageIndex = 1;
			this.btnDisconnect.ToolTipText = "Disconnect";
			// 
			// btnDelim1
			// 
			this.btnDelim1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// btnRename
			// 
			this.btnRename.ImageIndex = 2;
			this.btnRename.ToolTipText = "Rename";
			// 
			// btnMakeDir
			// 
			this.btnMakeDir.ImageIndex = 3;
			this.btnMakeDir.ToolTipText = "Make Directory";
			// 
			// btnDelete
			// 
			this.btnDelete.ImageIndex = 4;
			this.btnDelete.ToolTipText = "Delete selected";
			// 
			// btnDelim2
			// 
			this.btnDelim2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// btnDownload
			// 
			this.btnDownload.ImageIndex = 5;
			this.btnDownload.ToolTipText = "Download";
			// 
			// btnUpload
			// 
			this.btnUpload.ImageIndex = 6;
			this.btnUpload.ToolTipText = "Upload";
			// 
			// btnDelim3
			// 
			this.btnDelim3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// btnRefresh
			// 
			this.btnRefresh.ImageIndex = 7;
			this.btnRefresh.ToolTipText = "Refresh";
			// 
			// imgToolbar
			// 
			this.imgToolbar.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.imgToolbar.ImageSize = new System.Drawing.Size(16, 16);
			this.imgToolbar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgToolbar.ImageStream")));
			this.imgToolbar.TransparentColor = System.Drawing.Color.Yellow;
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuConnection,
																					 this.mnuHelp});
			// 
			// mnuConnection
			// 
			this.mnuConnection.Index = 0;
			this.mnuConnection.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.mnuConnect,
																						  this.mnuDisconnect,
																						  this.menuItem4,
																						  this.mnuExit});
			this.mnuConnection.Text = "Connection";
			// 
			// mnuConnect
			// 
			this.mnuConnect.Index = 0;
			this.mnuConnect.Text = "Connect...";
			this.mnuConnect.Click += new System.EventHandler(this.mnuConnect_Click);
			// 
			// mnuDisconnect
			// 
			this.mnuDisconnect.Index = 1;
			this.mnuDisconnect.Text = "Disconnect";
			this.mnuDisconnect.Click += new System.EventHandler(this.mnuDisconnect_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 2;
			this.menuItem4.Text = "-";
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 3;
			this.mnuExit.Text = "Exit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
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
			// lvLog
			// 
			this.lvLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					this.chTime,
																					this.chEvent});
			this.lvLog.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lvLog.FullRowSelect = true;
			this.lvLog.Location = new System.Drawing.Point(0, 369);
			this.lvLog.Name = "lvLog";
			this.lvLog.Size = new System.Drawing.Size(712, 112);
			this.lvLog.SmallImageList = this.imgLog;
			this.lvLog.TabIndex = 1;
			this.lvLog.View = System.Windows.Forms.View.Details;
			// 
			// chTime
			// 
			this.chTime.Text = "Time";
			this.chTime.Width = 120;
			// 
			// chEvent
			// 
			this.chEvent.Text = "Event";
			this.chEvent.Width = 400;
			// 
			// imgLog
			// 
			this.imgLog.ImageSize = new System.Drawing.Size(16, 16);
			this.imgLog.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLog.ImageStream")));
			this.imgLog.TransparentColor = System.Drawing.Color.White;
			// 
			// splitter
			// 
			this.splitter.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter.Location = new System.Drawing.Point(0, 366);
			this.splitter.Name = "splitter";
			this.splitter.Size = new System.Drawing.Size(712, 3);
			this.splitter.TabIndex = 2;
			this.splitter.TabStop = false;
			// 
			// pPath
			// 
			this.pPath.Controls.Add(this.pbDirectory);
			this.pPath.Controls.Add(this.lPath);
			this.pPath.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pPath.Location = new System.Drawing.Point(0, 342);
			this.pPath.Name = "pPath";
			this.pPath.Size = new System.Drawing.Size(712, 24);
			this.pPath.TabIndex = 3;
			// 
			// pbDirectory
			// 
			this.pbDirectory.BackColor = System.Drawing.SystemColors.Control;
			this.pbDirectory.Image = ((System.Drawing.Image)(resources.GetObject("pbDirectory.Image")));
			this.pbDirectory.Location = new System.Drawing.Point(4, 5);
			this.pbDirectory.Name = "pbDirectory";
			this.pbDirectory.Size = new System.Drawing.Size(16, 16);
			this.pbDirectory.TabIndex = 1;
			this.pbDirectory.TabStop = false;
			// 
			// lPath
			// 
			this.lPath.Location = new System.Drawing.Point(24, 6);
			this.lPath.Name = "lPath";
			this.lPath.Size = new System.Drawing.Size(608, 16);
			this.lPath.TabIndex = 0;
			// 
			// lvFiles
			// 
			this.lvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					  this.chFilename,
																					  this.chSize,
																					  this.chModified,
																					  this.chOwner,
																					  this.chRights});
			this.lvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvFiles.FullRowSelect = true;
			this.lvFiles.Location = new System.Drawing.Point(0, 28);
			this.lvFiles.MultiSelect = false;
			this.lvFiles.Name = "lvFiles";
			this.lvFiles.Size = new System.Drawing.Size(712, 314);
			this.lvFiles.SmallImageList = this.imgFiles;
			this.lvFiles.TabIndex = 4;
			this.lvFiles.View = System.Windows.Forms.View.Details;
			this.lvFiles.DoubleClick += new System.EventHandler(this.lvFiles_DoubleClick);
			// 
			// chFilename
			// 
			this.chFilename.Text = "Filename";
			this.chFilename.Width = 250;
			// 
			// chSize
			// 
			this.chSize.Text = "Size";
			this.chSize.Width = 80;
			// 
			// chModified
			// 
			this.chModified.Text = "Modified";
			this.chModified.Width = 100;
			// 
			// chOwner
			// 
			this.chOwner.Text = "Owner";
			// 
			// chRights
			// 
			this.chRights.Text = "Rights";
			this.chRights.Width = 120;
			// 
			// imgFiles
			// 
			this.imgFiles.ImageSize = new System.Drawing.Size(16, 16);
			this.imgFiles.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgFiles.ImageStream")));
			this.imgFiles.TransparentColor = System.Drawing.Color.White;
			// 
			// SftpClient
			// 
			this.SftpClient.ClientHostname = "";
			this.SftpClient.ClientUsername = "";
			this.SftpClient.CompressionLevel = 6;
			this.SftpClient.ForceCompression = false;
			this.SftpClient.KeyStorage = null;
			this.SftpClient.NewLineConvention = null;
			this.SftpClient.Password = "";
			this.SftpClient.SFTPExt = null;
			this.SftpClient.SoftwareName = "EldoS.SSHBlackbox.3";
			this.SftpClient.Username = "";
			this.SftpClient.Versions = ((short)(28));
			this.SftpClient.OnKeyValidate += new SBSSHCommon.TSSHKeyValidateEvent(this.SftpClient_OnKeyValidate);
			this.SftpClient.OnAuthenticationSuccess += new SBUtils.TNotifyEvent(this.SftpClient_OnAuthenticationSuccess);
			this.SftpClient.OnAuthenticationKeyboard +=new SBSSHCommon.TSSHAuthenticationKeyboardEvent(SftpClient_OnAuthenticationKeyboard);
			this.SftpClient.OnCloseConnection += new SBSSHCommon.TSSHCloseConnectionEvent(this.SftpClient_OnCloseConnection);
			this.SftpClient.OnError += new SBSSHCommon.TSSHErrorEvent(this.SftpClient_OnError);
			this.SftpClient.MessageLoop += new SBSftpCommon.TSBSftpMessageLoopEvent(this.SftpClient_MessageLoop);
			this.SftpClient.OnAuthenticationFailed += new SBSSHCommon.TSSHAuthenticationFailedEvent(this.SftpClient_OnAuthenticationFailed);
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(712, 481);
			this.Controls.Add(this.lvFiles);
			this.Controls.Add(this.pPath);
			this.Controls.Add(this.splitter);
			this.Controls.Add(this.lvLog);
			this.Controls.Add(this.tbToolbar);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.Menu = this.mainMenu;
			this.Name = "frmMain";
			this.Text = "ElSimpleSftpClient demo application";
			this.pPath.ResumeLayout(false);
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

		private void Initialize()
		{
			SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("0375F798E0FE4841F7BE6108C9380091070D223C95716FD7E54DC5AFAD2E1D9967B9D98111447F1EE1D63240089ECB469970710E9F35931A84012A4B0D6AA9A357353EF5B0B4B8C98A994347D93E3076EC5BEA10EB95A531D8FCFAC2295ECD59A495BE4E450C52AC431A274BE4B0B3122C7FF0E02FB9FD0CEB4094B98D290BC55BFD63A36D0EB355098F4B5A84FC333F1A8B932DBB86EFBAC8AD563ECA18851934F129370919B8683B88075A6980D1B0F850761DBFBBE44DAA788355CBF4C6B682EF8FF3894BD6D469A40E6DC8433C30FC6D8CBEB59DBA0E8DAEBA6DDFC04C8F78BD81385973AEE50C652F66A91888A37FCA08DF1CD730C4E55B6700DD1BB14D"));

			KeyStorage = new TElSSHMemoryKeyStorage(this);
			SftpClient.KeyStorage = KeyStorage;
		}

		private frmInputDialog CreateInputDialog(string Caption, string Prompt, string Default) 
		{
			frmInputDialog dlg = new frmInputDialog();
			dlg.Text = Caption;
			dlg.lPrompt.Text = Prompt;
			dlg.tbResponse.Text = Default;
			return dlg;
		}

		private string InputDialog(string Caption, string Prompt, string Default) 
		{
			frmInputDialog dlg = CreateInputDialog(Caption, Prompt, Default);
			if (dlg.ShowDialog(this) == DialogResult.OK) 
			{
				return dlg.tbResponse.Text;
			} 
			else 
			{
				return "";
			}
		}

		private void Connect()
		{
			frmConnProps dlg = new frmConnProps();
			if (SftpClient.Active) 
			{
				System.Windows.Forms.MessageBox.Show(this, "Already connected");
				return;
			}
			if (dlg.ShowDialog(this) == DialogResult.OK) 
			{
				SftpClient.Username = dlg.tbUsername.Text;
				SftpClient.Password = dlg.tbPassword.Text;
				SftpClient.Address = dlg.tbHost.Text;
				SftpClient.Port = 22;

				KeyStorage.Clear();
				TElSSHKey Key = new TElSSHKey();
				if ((dlg.txtPrivateKey.Text.Length != 0) && (Key.LoadPrivateKey(dlg.txtPrivateKey.Text, "") == 0))
				{
					KeyStorage.Add(Key);
					SftpClient.AuthenticationTypes = SftpClient.AuthenticationTypes | SBSSHConstants.Unit.SSH_AUTH_TYPE_PUBLICKEY;
				}
				else
					SftpClient.AuthenticationTypes = SftpClient.AuthenticationTypes & (~SBSSHConstants.Unit.SSH_AUTH_TYPE_PUBLICKEY);	

				Log("Connecting to " + dlg.tbHost.Text, false);
				try 
				{
					SftpClient.Open();
				} 
				catch(Exception ex) 
				{
					Log("SFTP connection failed with message [" + ex.Message + "]", true);
					return;
				}
				Log("SFTP connection established", false);
				currentDir = ".";
				RefreshData();
			};
		}

		private void Disconnect()
		{
			Log("Disconnecting", false);
			if (SftpClient.Active) 
			{
				SftpClient.Close(true);
			}
		}

		private void Rename()
		{
			string NewName;
			if (SftpClient.Active) 
			{
				if ((lvFiles.SelectedItems.Count > 0) && (lvFiles.SelectedItems[0].Tag != null)) 
				{
					NewName = InputDialog("Rename", "Please enter the new name for " +
						((TElSftpFileInfo)lvFiles.SelectedItems[0].Tag).Name, "");
					if (NewName.Length > 0) 
					{
						Log("Renaming " + ((TElSftpFileInfo)lvFiles.SelectedItems[0].Tag).Name + " to " + NewName, false);
						try 
						{
							SftpClient.RenameFile(currentDir + "/" + ((TElSftpFileInfo)lvFiles.SelectedItems[0].Tag).Name,
								currentDir + "/" + NewName);
						} 
						catch (Exception ex) 
						{
							Log("Failed to rename file '" + ((TElSftpFileInfo)lvFiles.SelectedItems[0].Tag).Name + "' to '" +
								NewName + "', " + ex.Message, true);
						}
						RefreshData();
					}
				}
			}
		}

		private void MakeDir()
		{
			if (SftpClient.Active) 
			{
				string DirName = InputDialog("Make Directory", "Please specify the name for new directory", "");
				if (DirName.Length > 0) 
				{
					Log("Creating directory " + DirName, false);
					try 
					{
						SftpClient.MakeDirectory(currentDir + "/" + DirName, null);
					} 
					catch(Exception ex) 
					{
						Log("Failed to create directory '" + DirName + "', " + ex.Message, true);
					}
					RefreshData();
				}
			}
		}

		private void Delete()
		{
			TElSftpFileInfo info;
			if (SftpClient.Active) 
			{
				if ((lvFiles.SelectedItems.Count > 0) && (lvFiles.SelectedItems[0].Tag != null)) 
				{
					info = (TElSftpFileInfo)lvFiles.SelectedItems[0].Tag;
					if (System.Windows.Forms.MessageBox.Show(this, "Please confirm that you want to delete " + 
						info.Name, "Delete item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) 
						== DialogResult.Yes) 
					{
						Log("Removing item " + info.Name, false);
						try 
						{
							if (info.Attributes.Directory) 
							{
								SftpClient.RemoveDirectory(currentDir + "/" + info.Name);
							} 
							else 
							{
								SftpClient.RemoveFile(currentDir + "/" + info.Name);
							}
						} 
						catch(Exception ex) 
						{
							Log("Failed to delete " + info.Name + ", " + ex.Message, true);
						}
						RefreshData();
					}
				}
			}
		}

		private void Download()
		{
			SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
			TElSftpFileInfo info;
			System.IO.FileStream stream;
			byte[] fileHandle;
			long processed, size;
			int read;
			byte[] buf = new byte[65280];
			frmProgress dlgProgress = new frmProgress();

			if ((SftpClient.Active) && (lvFiles.SelectedItems.Count > 0) && 
				(lvFiles.SelectedItems[0].Tag != null)) 
			{
				info = (TElSftpFileInfo)lvFiles.SelectedItems[0].Tag;
				dlg.FileName = info.Name;
				if (dlg.ShowDialog(this) == DialogResult.OK) 
				{
					try 
					{
						stream = new System.IO.FileStream(dlg.FileName, FileMode.Create);
					} 
					catch(Exception ex) 
					{
						Log("Failed to create local file " + dlg.FileName, true);
						return;
					}
					Log("Downloading file " + info.Name, false);
					try 
					{
                        fileHandle = SftpClient.OpenFile(currentDir + "/" + info.Name, 
							SBSftpCommon.Unit.fmRead, null);
					} 
					catch(Exception ex) 
					{
						stream.Close();
						Log("Failed to open file " + info.Name + ", " + ex.Message, true);
						return;
					}
					processed = 0;
					size = info.Attributes.Size;
					dlgProgress.Text = "Download";
					dlgProgress.lSourceFilename.Text = currentDir + "/" + info.Name;
					dlgProgress.lDestFilename.Text = dlg.FileName;
					dlgProgress.lProgress.Text = "0 / " + size.ToString();
					dlgProgress.pbProgress.Value = 0;
					dlgProgress.Show();
					try 
					{
						try 
						{
							while ((processed < size) && (!dlgProgress.Canceled)) 
							{
								read = SftpClient.Read(fileHandle, processed, buf, 0, buf.Length);
								stream.Write(buf, 0, read);
								processed += read;
								dlgProgress.pbProgress.Value = (int)(processed * 100 / size);
								dlgProgress.lProgress.Text = processed.ToString() + " / " + size.ToString();
							}
						} 
						finally 
						{
							dlgProgress.Hide();
							stream.Close();
							SftpClient.CloseHandle(fileHandle);
							Log("Download finished", false);
						}
					} 
					catch(Exception ex) 
					{
						Log("Error during download, " + ex.Message, true);
					}
				}				
			}
		}

		private void Upload()
		{
			System.IO.FileStream stream;
			byte[] fileHandle;
			string shortName;
			byte[] buf = new byte[65280];
			long processed, size;
			int read;
			System.Windows.Forms.OpenFileDialog dlg = new OpenFileDialog();
			frmProgress dlgProgress = new frmProgress();

			if (SftpClient.Active) 
			{
				if (dlg.ShowDialog(this) == DialogResult.OK) 
				{
					try 
					{
						stream = new System.IO.FileStream(dlg.FileName, FileMode.Open);
					} 
					catch(Exception ex) 
					{
						Log("Failed to open source file: " + ex.Message, true);
						return;
					}
					Log("Uploading file " + dlg.FileName, false);
					shortName = System.IO.Path.GetFileName(dlg.FileName);
					try 
					{
						fileHandle = SftpClient.CreateFile(currentDir + "/" + shortName);
					} 
					catch(Exception ex) 
					{
						Log("Failed to create remote file: " + ex.Message, true);
						stream.Close();
						return;
					}
                    processed = 0;
					size = stream.Length;
					dlgProgress.Text = "Upload";
					dlgProgress.lSourceFilename.Text = dlg.FileName;
					dlgProgress.lDestFilename.Text = currentDir + "/" + shortName;
					dlgProgress.lProgress.Text = "0 / " + size.ToString();
					dlgProgress.pbProgress.Value = 0;
					dlgProgress.Show();
					try 
					{
						try 
						{
							while ((processed < size) && (!dlgProgress.Canceled)) 
							{
								read = stream.Read(buf, 0, buf.Length);
								SftpClient.Write(fileHandle, processed, buf, 0, read);
								processed += read;
								dlgProgress.pbProgress.Value = (int)(processed * 100 / size);
								dlgProgress.lProgress.Text = processed.ToString() + " / " + size.ToString();
							}
						} 
						finally 
						{
							dlgProgress.Hide();
							stream.Close();
							SftpClient.CloseHandle(fileHandle);
							Log("Upload finished", false);
						}
					} 
					catch(Exception ex) 
					{
						Log("Error during upload, " + ex.Message, true);
					}
					RefreshData();
				}
			}
		}

		private void RefreshData()
		{
			byte[] dirHandle;
			ArrayList dirList = new ArrayList();
			TElSftpFileInfo info;
			ListViewItem item;
			FileInfoComparer comparer = new FileInfoComparer();

			ClearFileList();
			if (!SftpClient.Active) 
			{
				return; 
			}
			try 
			{
				currentDir = SftpClient.RequestAbsolutePath(currentDir);
			} 
			catch (Exception ex) 
			{
				currentDir = ".";
			}
			lPath.Text = currentDir;
			Log("Retrieving file list", false);
			try 
			{
				dirHandle = SftpClient.OpenDirectory(currentDir);
				try 
				{
					SftpClient.ReadDirectory(dirHandle, dirList);
					dirList.Sort(comparer);
					for(int i = 0; i < dirList.Count; i++) 
					{
						info = new TElSftpFileInfo();
						((TElSftpFileInfo)dirList[i]).CopyTo(info);
						item = lvFiles.Items.Add(info.Name);
						item.Tag = info;
						if (!info.Attributes.Directory) 
						{
							item.SubItems.Add(info.Attributes.Size.ToString());
							item.ImageIndex = 1;
						} 
						else 
						{
							item.SubItems.Add("");
							item.ImageIndex = 0;
						}
						item.SubItems.Add(info.Attributes.MTime.ToShortDateString() + " " + 
							info.Attributes.MTime.ToShortTimeString());
						item.SubItems.Add(SBUtils.Unit.StringOfBytes(info.Attributes.Owner));
						item.SubItems.Add(FormatRights(info.Attributes));
					}
				} 
				finally 
				{
					SftpClient.CloseHandle(dirHandle);
				}
			} 
			catch(Exception ex) 
			{
				Log("Failed to retrieve file list", true);
				return;
			}
		}

		private void ChangeDir()
		{
			TElSftpFileInfo info;
			byte[] dirHandle;
			if ((SftpClient.Active) && (lvFiles.SelectedItems.Count > 0) && 
				(lvFiles.SelectedItems[0].Tag != null)) 
			{
				info = (TElSftpFileInfo)lvFiles.SelectedItems[0].Tag;
				if (info.Attributes.Directory) 
				{
					Log("Changing directory to " + info.Name, false);
					try 
					{
						dirHandle = SftpClient.OpenDirectory(currentDir + "/" + info.Name);
					} 
					catch(Exception ex) 
					{
						Log("Unable to change directory: " + ex.Message, true);
						return;
					}
					try 
					{
						SftpClient.CloseHandle(dirHandle);
						currentDir = SftpClient.RequestAbsolutePath(currentDir + "/" + info.Name);
					} 
					catch(Exception ex) 
					{
						Log("Unexpected error: " + ex.Message, true);
					}
					RefreshData();
				}
			}
		}

		private void Log(string S, bool IsError) 
		{
			ListViewItem item = lvLog.Items.Add(DateTime.Now.ToLongTimeString());
			if (IsError) 
			{
				item.ImageIndex = 1;
			} 
			else 
			{
				item.ImageIndex = 0;
			}
			item.SubItems.Add(S);
		}

		private void ClearFileList()
		{
			lvFiles.Items.Clear();
		}

		private string FormatRights(TElSftpFileAttributes attributes)
		{
			string res = "";
			if (attributes.Directory) 
			{
				res = res + "d";
			}
			if (attributes.UserRead) 
			{
				res = res + "r";
			} 
			else 
			{
				res = res + "-";
			}
			if (attributes.UserWrite) 
			{
				res = res + "w";
			}
			else 
			{
				res = res + "-";
			}
			if (attributes.UserExecute) 
			{
				res = res + "x";
			}
			else 
			{
				res = res + "-";
			}
			if (attributes.GroupRead) 
			{
				res = res + 'r';
			}
			else 
			{
				res = res + '-';
			}
			if (attributes.GroupWrite) 
			{
				res = res + 'w';
			}
			else 
			{
				res = res + '-';
			}
			if (attributes.GroupExecute) 
			{
				res = res + 'x';
			}
			else 
			{
				res = res + '-';
			}
			if (attributes.OtherRead) 
			{
				res = res + 'r';
			}
			else 
			{
				res = res + '-';
			}
			if (attributes.OtherWrite) 
			{
				res = res + 'w';
			}
			else 
			{
				res = res + '-';
			}
			if (attributes.OtherExecute) 
			{
				res = res + 'x';
			}
			else 
			{
				res = res + '-';
			}
			return res;
		}

		private void tbToolbar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == btnConnect) 
			{
				Connect();
			} 
			else if (e.Button == btnDisconnect) 
			{
				Disconnect();
			} 
			else if (e.Button == btnRename) 
			{
				Rename();
			} 
			else if (e.Button == btnMakeDir) 
			{
				MakeDir();
			} 
			else if (e.Button == btnDelete) 
			{
				Delete();
			} 
			else if (e.Button == btnDownload) 
			{
				Download();
			} 
			else if (e.Button == btnUpload) 
			{
				Upload();
			} 
			else if (e.Button == btnRefresh) 
			{
				RefreshData();
			}
		}

		private void SftpClient_OnAuthenticationFailed(object Sender, int AuthenticationType)
		{
			Log("Authentication type [" + AuthenticationType.ToString() + "] failed", true);
		}

		private void SftpClient_OnAuthenticationSuccess(object Sender)
		{
			Log("Authentication succeeded", false);
		}

		private void SftpClient_OnCloseConnection(object Sender)
		{
			Log("SFTP connection closed", false);
		}

		private void SftpClient_OnError(object Sender, int ErrorCode)
		{
			Log("Error " + ErrorCode.ToString(), true);
		}

		private void SftpClient_OnKeyValidate(object Sender, SBSSHKeyStorage.TElSSHKey ServerKey, ref bool Validate)
		{
			Log("Server key [" + SBUtils.Unit.DigestToStr128(ServerKey.FingerprintMD5, true) + "] received", false);
			Validate = true;
		}

		private bool SftpClient_MessageLoop()
		{
			Application.DoEvents();
			return true;
		}

		private void lvFiles_DoubleClick(object sender, System.EventArgs e)
		{
			ChangeDir();
		}

		private void mnuConnect_Click(object sender, System.EventArgs e)
		{
			Connect();
		}

		private void mnuDisconnect_Click(object sender, System.EventArgs e)
		{
			Disconnect();
		}

		private void mnuExit_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void mnuAbout_Click(object sender, System.EventArgs e)
		{
			frmAbout dlg = new frmAbout();
			dlg.ShowDialog(this);
		}

		private void SftpClient_OnAuthenticationKeyboard(object Sender, SBStringList.TElStringList Prompts, bool[] Echo, SBStringList.TElStringList Responses)
		{
			Responses.Clear();
			for (int i = 0; i < Prompts.Count; i++)
			{
				string Response = "";				
				if (PromptForm.Prompt(Prompts[i], Echo[i], ref Response))
					Responses.Add(Response);
				else
					Responses.Add("");
			}
		}
	}

	public class FileInfoComparer : System.Collections.IComparer
	{
		public int Compare(object x, object y) 
		{
			TElSftpFileInfo infox, infoy;
			int ret;
			
			infox = (TElSftpFileInfo)x;
			infoy = (TElSftpFileInfo)y;

			if (!((infox.Attributes.Directory) ^ (infoy.Attributes.Directory))) 
			{
				ret = infox.Name.CompareTo(infoy.Name);
			} 
			else 
			{
				if (infox.Attributes.Directory) 
				{
					ret = -1;
				} 
				else 
				{
					ret = 1;
				}
			}			
			return ret;
		}
	}

}
