using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using SBSSHConstants;
using SBSSHCommon;
using SBSftpCommon;
using SBSSHClient;
using SBStringList;
using SBSftp;

namespace Sftp
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private static int FILE_BLOCK_SIZE				= 65280;
		private static int STATE_OPEN_DIRECTORY_SENT	= 1;
		private static int STATE_READ_DIRECTORY_SENT	= 2;
		private static int STATE_CHANGE_DIR				= 3;
		private static int STATE_MAKE_DIR				= 4;
		private static int STATE_RENAME					= 5;
		private static int STATE_REMOVE					= 6;
		private static int STATE_DOWNLOAD_OPEN			= 7;
		private static int STATE_DOWNLOAD_RECEIVE		= 8;
		private static int STATE_UPLOAD_OPEN			= 9;
		private static int STATE_UPLOAD_SEND			= 10;
		private static int STATE_CLOSE_HANDLE			= 11;

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox editHost;
		private System.Windows.Forms.TextBox editUserName;
		private System.Windows.Forms.TextBox editPassword;
		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.TextBox editPath;
		private System.Windows.Forms.Button btnUpdateFileInfo;
		private System.Windows.Forms.Button btnMkDir;
		private System.Windows.Forms.Button btnRename;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnGetFile;
		private System.Windows.Forms.Button btnPutFile;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ColumnHeader columnHeaderName;
		private System.Windows.Forms.ColumnHeader columnHeaderSize;
		private System.Windows.Forms.ColumnHeader columnHeaderPermissions;

		private Socket scktClient;
		private TElSSHClient sshClient;
		private TElSSHTunnelList tunnelList;
		private TElSubsystemSSHTunnel sftpTunnel; 
		private TElSftpClient sftpClient;

		private int state;
		private ArrayList currentFileList = new ArrayList();
		private byte[] currentHandle;
		private long currentFileSize = 0;
		private String currentDir;
		private String relDir;
		private System.Windows.Forms.TextBox editInfo;

		private byte[] receiveBuf = new byte[8192];
		private int receiveLen = 0;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;

		private FileStream currentFile;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.TextBox memoLog;
		private System.Windows.Forms.ListView lvFiles;

		private ProgressWindow progress;
 
		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Init();
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.editPassword = new System.Windows.Forms.TextBox();
            this.editUserName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.editHost = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.editPath = new System.Windows.Forms.TextBox();
            this.lvFiles = new System.Windows.Forms.ListView();
            this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderSize = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderPermissions = new System.Windows.Forms.ColumnHeader();
            this.editInfo = new System.Windows.Forms.TextBox();
            this.btnMkDir = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnGetFile = new System.Windows.Forms.Button();
            this.btnPutFile = new System.Windows.Forms.Button();
            this.btnUpdateFileInfo = new System.Windows.Forms.Button();
            this.memoLog = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnConnect);
            this.groupBox1.Controls.Add(this.editPassword);
            this.groupBox1.Controls.Add(this.editUserName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.editHost);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(5, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(475, 80);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection properties";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(56, 48);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(128, 23);
            this.btnConnect.TabIndex = 5;
            this.btnConnect.Text = "Connect";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // editPassword
            // 
            this.editPassword.Location = new System.Drawing.Point(312, 48);
            this.editPassword.Name = "editPassword";
            this.editPassword.PasswordChar = '*';
            this.editPassword.Size = new System.Drawing.Size(152, 21);
            this.editPassword.TabIndex = 4;
            // 
            // editUserName
            // 
            this.editUserName.Location = new System.Drawing.Point(312, 16);
            this.editUserName.Name = "editUserName";
            this.editUserName.Size = new System.Drawing.Size(152, 21);
            this.editUserName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(235, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "User Name:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Host:";
            // 
            // editHost
            // 
            this.editHost.Location = new System.Drawing.Point(48, 16);
            this.editHost.Name = "editHost";
            this.editHost.Size = new System.Drawing.Size(152, 21);
            this.editHost.TabIndex = 0;
            this.editHost.Text = "192.168.0.1";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(235, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Password:";
            // 
            // editPath
            // 
            this.editPath.BackColor = System.Drawing.SystemColors.Window;
            this.editPath.Location = new System.Drawing.Point(8, 88);
            this.editPath.Name = "editPath";
            this.editPath.ReadOnly = true;
            this.editPath.Size = new System.Drawing.Size(472, 21);
            this.editPath.TabIndex = 1;
            // 
            // lvFiles
            // 
            this.lvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderSize,
            this.columnHeaderPermissions});
            this.lvFiles.FullRowSelect = true;
            this.lvFiles.GridLines = true;
            this.lvFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvFiles.HideSelection = false;
            this.lvFiles.Location = new System.Drawing.Point(8, 112);
            this.lvFiles.MultiSelect = false;
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.Size = new System.Drawing.Size(472, 240);
            this.lvFiles.TabIndex = 2;
            this.lvFiles.View = System.Windows.Forms.View.Details;
            this.lvFiles.DoubleClick += new System.EventHandler(this.lvFiles_DoubleClick);
            this.lvFiles.SelectedIndexChanged += new System.EventHandler(this.lvFiles_SelectedIndexChanged);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 212;
            // 
            // columnHeaderSize
            // 
            this.columnHeaderSize.Text = "Size";
            this.columnHeaderSize.Width = 79;
            // 
            // columnHeaderPermissions
            // 
            this.columnHeaderPermissions.Text = "Permissions";
            this.columnHeaderPermissions.Width = 156;
            // 
            // editInfo
            // 
            this.editInfo.BackColor = System.Drawing.SystemColors.Window;
            this.editInfo.Location = new System.Drawing.Point(8, 360);
            this.editInfo.Name = "editInfo";
            this.editInfo.ReadOnly = true;
            this.editInfo.Size = new System.Drawing.Size(472, 21);
            this.editInfo.TabIndex = 3;
            // 
            // btnMkDir
            // 
            this.btnMkDir.Location = new System.Drawing.Point(8, 384);
            this.btnMkDir.Name = "btnMkDir";
            this.btnMkDir.Size = new System.Drawing.Size(75, 23);
            this.btnMkDir.TabIndex = 4;
            this.btnMkDir.Text = "MkDir";
            this.btnMkDir.Click += new System.EventHandler(this.btnMkDir_Click);
            // 
            // btnRename
            // 
            this.btnRename.Location = new System.Drawing.Point(87, 384);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(75, 23);
            this.btnRename.TabIndex = 5;
            this.btnRename.Text = "Rename";
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(167, 384);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnGetFile
            // 
            this.btnGetFile.Location = new System.Drawing.Point(247, 384);
            this.btnGetFile.Name = "btnGetFile";
            this.btnGetFile.Size = new System.Drawing.Size(75, 23);
            this.btnGetFile.TabIndex = 7;
            this.btnGetFile.Text = "Get File";
            this.btnGetFile.Click += new System.EventHandler(this.btnGetFile_Click);
            // 
            // btnPutFile
            // 
            this.btnPutFile.Location = new System.Drawing.Point(327, 384);
            this.btnPutFile.Name = "btnPutFile";
            this.btnPutFile.Size = new System.Drawing.Size(75, 23);
            this.btnPutFile.TabIndex = 8;
            this.btnPutFile.Text = "Put File";
            this.btnPutFile.Click += new System.EventHandler(this.btnPutFile_Click);
            // 
            // btnUpdateFileInfo
            // 
            this.btnUpdateFileInfo.Location = new System.Drawing.Point(407, 384);
            this.btnUpdateFileInfo.Name = "btnUpdateFileInfo";
            this.btnUpdateFileInfo.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateFileInfo.TabIndex = 9;
            this.btnUpdateFileInfo.Text = "Update Info";
            this.btnUpdateFileInfo.Click += new System.EventHandler(this.btnUpdateFileInfo_Click);
            // 
            // memoLog
            // 
            this.memoLog.BackColor = System.Drawing.SystemColors.Window;
            this.memoLog.Location = new System.Drawing.Point(8, 416);
            this.memoLog.Multiline = true;
            this.memoLog.Name = "memoLog";
            this.memoLog.ReadOnly = true;
            this.memoLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.memoLog.Size = new System.Drawing.Size(472, 64);
            this.memoLog.TabIndex = 10;
            // 
            // frmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(490, 487);
            this.Controls.Add(this.memoLog);
            this.Controls.Add(this.btnUpdateFileInfo);
            this.Controls.Add(this.btnPutFile);
            this.Controls.Add(this.btnGetFile);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.btnMkDir);
            this.Controls.Add(this.editInfo);
            this.Controls.Add(this.lvFiles);
            this.Controls.Add(this.editPath);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sftp demo";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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

		private void Init()
		{
			btnConnect.Tag = false;

			sshClient = new TElSSHClient();
			sshClient.Versions = SBSSHCommon.Unit.sbSSH2;
			sshClient.OnSend += new SBSSHCommon.TSSHSendEvent(sshClient_OnSend);
			sshClient.OnReceive += new SBSSHCommon.TSSHReceiveEvent(sshClient_OnReceive);
			sshClient.OnOpenConnection += new SBSSHCommon.TSSHOpenConnectionEvent(sshClient_OnOpenConnection);
			sshClient.OnCloseConnection += new SBSSHCommon.TSSHCloseConnectionEvent(sshClient_OnCloseConnection);
			sshClient.OnAuthenticationSuccess += new SBUtils.TNotifyEvent(sshClient_OnAuthenticationSuccess);
			sshClient.OnAuthenticationFailed += new SBSSHCommon.TSSHAuthenticationFailedEvent(sshClient_OnAuthenticationFailed);
			sshClient.OnAuthenticationKeyboard +=new TSSHAuthenticationKeyboardEvent(sshClient_OnAuthenticationKeyboard);
			tunnelList = new TElSSHTunnelList();
			sftpTunnel = new TElSubsystemSSHTunnel();
			sftpClient = new TElSftpClient();
			sftpTunnel.TunnelList = tunnelList;
			sshClient.TunnelList = tunnelList;
			sftpClient.Tunnel = sftpTunnel;
			sftpClient.OnOpenConnection += new SBUtils.TNotifyEvent(sftpClient_OnOpenConnection);
			sftpClient.OnCloseConnection += new SBUtils.TNotifyEvent(sftpClient_OnCloseConnection);
			sftpClient.OnOpenFile += new TSBSftpFileOpenEvent(sftpClient_OnOpenFile);
			sftpClient.OnError += new TSBSftpErrorEvent(sftpClient_OnError);
			sftpClient.OnSuccess += new TSBSftpSuccessEvent(sftpClient_OnSuccess);
			sftpClient.OnDirectoryListing += new TSBSftpDirectoryListingEvent(sftpClient_OnDirectoryListing);
			sftpClient.OnData += new TSBSftpDataEvent(sftpClient_OnData);
			sftpClient.OnAbsolutePath += new TSBSftpAbsolutePathEvent(sftpClient_OnAbsolutePath);
			sftpClient.OnFileAttributes += new TSBSftpFileAttributesEvent(sftpClient_OnFileAttributes);
		}

		#region High-level routines
		
		private void CloseConnection()
		{
			if (scktClient != null && scktClient.Connected)
			{
				LogMessage("TCP connection closed.");
				scktClient.Close();
				scktClient = null;
				ListViewClear(lvFiles);
				SetEditText("", editPath);
                SetEditText("", editInfo);
				if (currentFile != null)
				{
					try
					{
						currentFile.Close();
					}
					catch(Exception ex)
					{}
					currentFile = null;
				}
			}
			
			btnConnect.Tag = false;
			btnConnectSetText("Connect");
		}

		private void CloseCurrentHandle()
		{
			if (scktClient == null || !scktClient.Connected)
			{
				LogMessage("Error: not connected.");
				return;
			}
			LogMessage("Closing active handle.");
			sftpClient.CloseHandle(currentHandle);
		}

		private bool RequestAbsolutePath(String path)
		{
			return sftpClient.RequestAbsolutePath(path);
		}

		private void OutputFileList()
		{
			ListViewItem[] buf = new ListViewItem[currentFileList.Count];
			ListViewItem tmp;
			for (int i = 0; i < currentFileList.Count; i++)
			{
				tmp = new ListViewItem();
				
				TElSftpFileInfo fi = (TElSftpFileInfo)currentFileList[i];
				tmp.Tag = fi;
				tmp.Text = fi.Name;
				tmp.SubItems.Add(Convert.ToString(fi.Attributes.Size, 10));
				tmp.SubItems.Add(WritePermissions(fi.Attributes));
				buf[i] = tmp;
			}

            ListViewClear(lvFiles);
            ListViewAddRange(buf, lvFiles);
		}

		private void WriteNextBlockToFile()
		{
			if (scktClient == null || !scktClient.Connected)
			{
				LogMessage("Error: not connected.");
				return;
			}
            
			if (currentFile.Position >= currentFileSize)
			{
				state = STATE_CLOSE_HANDLE;
				if (progress != null)
					progress.CloseDialog();
				CloseCurrentHandle();
				return;
			}

			byte[] buf = new byte[FILE_BLOCK_SIZE];
			int transferred;
	
			transferred = currentFile.Read(buf, 0, (int)FILE_BLOCK_SIZE);
			byte[] wb = null;
			if (transferred < FILE_BLOCK_SIZE)
			{
				wb = new byte[transferred];
				Array.Copy(buf, 0, wb, 0, transferred);
			}
			else
				wb = buf;
			buf = null;
			sftpClient.Write(currentHandle, currentFile.Position - transferred, wb);
		}

		private void BuildFileList(String path)
		{
			if (scktClient == null || !scktClient.Connected)
			{
				LogMessage("Error: not connected.");
				return;
			}
			currentFileList.Clear();
			LogMessage("Opening directory " + path);
			sftpClient.OpenDirectory(path);
			state = STATE_OPEN_DIRECTORY_SENT;
		}

		protected void LogMessage(String text)
		{
			String time = DateTime.Now.ToString();
			StringBuilder sb = new StringBuilder(time.Length + 2 + text.Length + 2);
			if (memoLog.Text.Length > 0)
				sb.Append("\r\n");
			sb.Append(time);
			sb.Append(": ");
			sb.Append(text);
			memoLogAppendText(sb.ToString());
		}
		private void ChangeDir(String dir)
		{
			LogMessage("Trying to change directory to " + dir);
			relDir = dir;
			state = STATE_CHANGE_DIR;
			sftpClient.OpenDirectory(currentDir + '/' + dir);
		}

		private void MakeDir(String dir)
		{
			if (scktClient == null || !scktClient.Connected)
			{
				LogMessage("Error: not connected.");
				return;
			}

			LogMessage("Creating directory " + dir);
			TElSftpFileAttributes attrs = new TElSftpFileAttributes();
			state = STATE_MAKE_DIR;
			sftpClient.MakeDirectory(dir, attrs);
		}

		private void RenameFile(String oldName, String newName)
		{
			if (scktClient == null || !scktClient.Connected)
			{
				LogMessage("Error: not connected.");
				return;
			}
			
			LogMessage(String.Format("Renaming \"{0}\" to \"{1}\"", 
				oldName, newName));
			state = STATE_RENAME;
			sftpClient.RenameFile(currentDir + '/' + oldName, 
				currentDir + '/' + newName);
		}

		private void DeleteDir(String name)
		{
			if (scktClient == null || !scktClient.Connected)
			{
				LogMessage("Error: not connected.");
				return;
			}
			LogMessage("Removing directory " + name);
			state = STATE_REMOVE;
			sftpClient.RemoveDirectory(currentDir + '/' + name);
		}         

		private void DeleteFile(String name)
		{
			if (scktClient == null || !scktClient.Connected)
			{
				LogMessage("Error: not connected.");
				return;
			}
			LogMessage("Removing file " + name);
			state = STATE_REMOVE;
			sftpClient.RemoveFile(currentDir + '/' + name);
		}


		private void DownloadFile(TElSftpFileInfo info, String localName)
		{
			if (scktClient == null || !scktClient.Connected)
			{
				LogMessage("Error: not connected.");
				return;
			}
            
			try
			{
				currentFile = new FileStream(localName, FileMode.Create);
				currentFileSize = info.Attributes.Size;
			}
			catch(Exception ex)
			{
				currentFileSize = 0;
				LogMessage(String.Format("Failed to download the file: \"{0}\", errror: \"{1}\"", info.Name, ex.Message));
				return;
			}
		
			LogMessage("Starting file download " + info.Name);
			state = STATE_DOWNLOAD_OPEN;
			sftpClient.OpenFile(
				currentDir + '/' + info.Name,
				SBSftpCommon.Unit.fmRead, null);
		}

		private void UploadFile(String localFile)
		{
			if (scktClient == null || !scktClient.Connected)
			{
				LogMessage("Error: not connected.");
				return;
			}
			
			String fName = "";
			try
			{
				FileInfo fi = new FileInfo(localFile);
				fName = fi.Name;
				currentFileSize = fi.Length;
				currentFile = File.OpenRead(localFile);
			}
			catch(Exception ex)
			{
				ShowErrorMessage(ex);
				return;
			}

			LogMessage(String.Format("Starting file upload, \"{0}\"", localFile));
			state = STATE_UPLOAD_OPEN;
			sftpClient.CreateFile(currentDir + '/' + fName);
		}

		protected void ShowErrorMessage(Exception ex)
		{
			Console.WriteLine(ex.StackTrace);
			MessageBox.Show(ex.Message, this.Text, 
				MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		#endregion

		#region Multi-threaded GUI access handling
		
		delegate void SetTextCallback(string text);
        delegate void SetEditTextCallback(string text, TextBox edit);
        delegate void SetEnabledCallback(bool value);
        delegate void ListViewAddRangeCallback(ListViewItem[] items, ListView lv);
        delegate void ListViewClearCallback(ListView lv);

		private void btnConnectSetText(string s) 
		{
			if (this.btnConnect.InvokeRequired) 
			{
				SetTextCallback d = new SetTextCallback(btnConnectSetText);
				this.Invoke(d, new object[] { s });
			} 
			else 
			{
				this.btnConnect.Text = s;
			}
		}

		private void SetEditText(string s, TextBox edit) 
		{
			if (edit.InvokeRequired) 
			{
				SetEditTextCallback d = new SetEditTextCallback(SetEditText);
				this.Invoke(d, new object[] { s, edit });
			} 
			else 
			{
				edit.Text = s;
			}
		}

		private void memoLogAppendText(string s)
		{
			if (this.memoLog.InvokeRequired) 
			{
				SetTextCallback d = new SetTextCallback(memoLogAppendText);
				this.Invoke(d, new object[] { s });
			} 
			else 
			{
				this.memoLog.AppendText(s);
			}
		}

        private void btnConnectSetEnabled(bool value)
        {
            if (this.btnConnect.InvokeRequired)
            {
                SetEnabledCallback d = new SetEnabledCallback(btnConnectSetEnabled);
                this.Invoke(d, new object[] { value });
            }
            else
            {
                this.btnConnect.Enabled = value;
            }
        }

        private void ListViewAddRange(ListViewItem[] items, ListView lv)
        {
            if (lv.InvokeRequired)
            {
                ListViewAddRangeCallback d = new ListViewAddRangeCallback(ListViewAddRange);
                this.Invoke(d, new object[] { items, lv });
            }
            else
            {
                lv.Items.AddRange(items);
            }
        }

        private void ListViewClear(ListView lv)
        {
            if (lv.InvokeRequired)
            {
                ListViewClearCallback d = new ListViewClearCallback(ListViewClear);
                this.Invoke(d, new object[] { lv });
            }
            else
            {
                lv.Items.Clear();
            }
        }

		#endregion

		#region Control event handlers

		private void btnConnect_Click(object sender, System.EventArgs e)
		{
			if ((bool)btnConnect.Tag == true)
			{
				CloseConnection();
			}
			else
			{
                scktClient = new Socket(AddressFamily.InterNetwork, 
					SocketType.Stream, ProtocolType.Tcp);
				try
				{
					IPAddress hostadd = Dns.Resolve(editHost.Text).AddressList[0];
					IPEndPoint EPhost = new IPEndPoint(hostadd, 22);
					scktClient.BeginConnect(EPhost, 
						new AsyncCallback(OnSocketConnectCallback), null);
				}
				catch(Exception ex)
				{
					ShowErrorMessage(ex);
					return;
				}

				btnConnectSetEnabled(false);
			}
  		}

		private void btnUpdateFileInfo_Click(object sender, System.EventArgs e)
		{
			if (scktClient == null || !scktClient.Connected)
			{
				LogMessage("Error: not connected.");
				return;
			}

			if (lvFiles.SelectedItems.Count == 0)
			{
				LogMessage("File is not selected.");
				return;
			}

			TElSftpFileInfo info = (TElSftpFileInfo)lvFiles.SelectedItems[0].Tag;
			LogMessage(String.Format("Requesting  download, \"{0}\"", info.Name));
			sftpClient.RequestAttributes(currentDir + '/' + info.Name, false);
		}

		private void btnPutFile_Click(object sender, System.EventArgs e)
		{
			if (scktClient == null || !scktClient.Connected)
			{
				LogMessage("Error: not connected.");
				return;
			}

			openFileDialog1.Filter = "All files (*.*)|*";
			if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
				UploadFile(openFileDialog1.FileName);
		}

		private void btnGetFile_Click(object sender, System.EventArgs e)
		{
			if (scktClient == null || !scktClient.Connected)
			{
				LogMessage("Error: not connected.");
				return;
			}

			if (lvFiles.SelectedItems.Count == 0)
			{
				LogMessage("File is not selected.");
				return;
			}

			TElSftpFileInfo info = (TElSftpFileInfo)lvFiles.SelectedItems[0].Tag;
			if (info.Attributes.Directory)
			{
				LogMessage("Can not download directory. Please choose ordinary file.");
				return;
			}
                
			saveFileDialog1.Filter = "All files (*.*)|*";
			saveFileDialog1.FileName = info.Name;
			if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
				DownloadFile(info, saveFileDialog1.FileName);
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if (scktClient == null || !scktClient.Connected)
			{
				LogMessage("Error: not connected.");
				return;
			}

			if (lvFiles.SelectedItems.Count == 0)
			{
				LogMessage("File is not selected.");
				return;
			}

			if (MessageBox.Show("Do you really want to remove this item?", 
				this.Text, MessageBoxButtons.YesNo, 
				MessageBoxIcon.Question) == DialogResult.No)
				return;

			TElSftpFileInfo info = (TElSftpFileInfo)lvFiles.SelectedItems[0].Tag;
			if (info.Attributes.Directory)
				DeleteDir(info.Name);
			else
				DeleteFile(info.Name);
		}

		private void btnRename_Click(object sender, System.EventArgs e)
		{
			if (scktClient == null || !scktClient.Connected)
			{
				LogMessage("Error: not connected.");
				return;
			}

			if (lvFiles.SelectedItems.Count == 0)
			{
				LogMessage("File is not selected.");
				return;
			}

			TElSftpFileInfo info = (TElSftpFileInfo)lvFiles.SelectedItems[0].Tag;
				
			StringQueryDlg sqd = new StringQueryDlg(false);
			sqd.Text = "Rename...";
			sqd.Description = "Enter NEW name:";
			if (sqd.ShowDialog(this) != DialogResult.OK)
				return;
			if (sqd.TextBox.Length == 0)
			{
				LogMessage("Invalid filename, renaming cancelled.");
				return;
			}

			RenameFile(info.Name, sqd.TextBox);
		}

		private void lvFiles_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lvFiles.SelectedItems.Count > 0)
			{
				TElSftpFileInfo info = (TElSftpFileInfo)lvFiles.SelectedItems[0].Tag;
				editInfo.Text = info.LongName;
			}
			else
				editInfo.Text = "";
		}

		private void btnMkDir_Click(object sender, System.EventArgs e)
		{
			if (scktClient == null || !scktClient.Connected)
			{
				LogMessage("Error: not connected.");
				return;
			}

			StringQueryDlg sqd = new StringQueryDlg(false);
			sqd.Text = "New directory...";
			sqd.Description = "Enter directory name:";
			if (sqd.ShowDialog(this) != DialogResult.OK)
				return;
			if (sqd.TextBox.Length == 0)
			{
				LogMessage("Invalid directory name, creation cancelled.");
				return;
			}
			MakeDir(sqd.TextBox);
		}

		private void lvFiles_DoubleClick(object sender, System.EventArgs e)
		{
			if (scktClient == null || !scktClient.Connected)
			{
				LogMessage("Error: not connected");
				return;
			}

			if (lvFiles.SelectedItems.Count > 0)
			{
				TElSftpFileInfo info = (TElSftpFileInfo)lvFiles.SelectedItems[0].Tag;
				if (info.Attributes.Directory)
				{
					editInfo.Text = "";
					ChangeDir(info.Name);
				}
			}
		}

		#endregion 

		#region Asynchronous socket callbacks

		private void OnSocketConnectCallback(IAsyncResult asyncResult)
		{
			btnConnectSetEnabled(true); 
			try
			{
				scktClient.EndConnect(asyncResult);
				LogMessage("TCP connection opened.");
			}
			catch(Exception ex)
			{
				scktClient.Close();
				scktClient = null;
				ShowErrorMessage(ex);
				return;
			}
			btnConnect.Tag = true;
			btnConnectSetText("Disconnect");            
			sshClient.UserName = editUserName.Text;
			sshClient.Password = editPassword.Text;
			sshClient.Open();
			scktClient.BeginReceive(receiveBuf, 0, receiveBuf.Length, 0,
				new AsyncCallback(OnSocketReceiveCallback), scktClient);
		}

		private void OnSocketReceiveCallback(IAsyncResult ar)
		{
			try
			{
				if (scktClient != null && scktClient.Connected)
				{
					receiveLen = scktClient.EndReceive(ar);
					while (receiveLen > 0)
					{
						sshClient.DataAvailable();
					}
					scktClient.BeginReceive(receiveBuf, 0, receiveBuf.Length, 0,
						new AsyncCallback(OnSocketReceiveCallback), scktClient);
				}
			}
			catch(Exception ex)
			{
				if (scktClient != null && scktClient.Connected)
				{
					ShowErrorMessage(ex);
					CloseConnection();
				}
			}
		}

		#endregion

		#region ElSSHClient callbacks

		private void sshClient_OnSend(object Sender, byte[] Buffer)
		{
			try
			{
				scktClient.Send(Buffer, 0, Buffer.Length, 0);
			}
			catch(Exception ex)
			{
				if (scktClient != null && scktClient.Connected)
				{
					ShowErrorMessage(ex);
					CloseConnection();
				}
			}
		}

		private void sshClient_OnReceive(object Sender, ref byte[] Buffer, int MaxSize, out int Written)
		{
			Written = Math.Min(MaxSize, receiveLen);
			if (Written > 0) 
			{
                Array.Copy(receiveBuf, 0, Buffer, 0, Written);
				Array.Copy(receiveBuf, Written, receiveBuf, 0, receiveLen - Written);
				receiveLen -= Written;
			}
		}

		private void sshClient_OnOpenConnection(object Sender)
		{
			LogMessage("SSH Connection started.");
		}

		private void sshClient_OnCloseConnection(object Sender)
		{
			LogMessage("SSH Connection closed.");
		}

		private void sshClient_OnAuthenticationSuccess(object Sender)
		{
			LogMessage("Authentication succeeded.");
		}

		private void sshClient_OnAuthenticationFailed(object Sender, int AuthenticationType)
		{
			LogMessage("Authentication failed. Unknown user or Invalid password.");
		}

		#endregion 

		#region ElSftpClient callbacks

		private void sftpClient_OnOpenConnection(object Sender)
		{
			LogMessage("Sftp connection started.");
			currentDir = ".";
			SetEditText(currentDir, editPath);
			BuildFileList(".");
		}

		private void sftpClient_OnCloseConnection(object Sender)
		{
			LogMessage("Sftp connection closed.");
			CloseConnection();
		}

		private void sftpClient_OnOpenFile(object Sender, byte[] Handle)
		{
			if (state == STATE_OPEN_DIRECTORY_SENT)
			{
				LogMessage("Directory opened.");
				currentHandle = Handle;
				sftpClient.ReadDirectory(currentHandle);
				state = STATE_READ_DIRECTORY_SENT;
			}
			else if (state == STATE_CHANGE_DIR)
			{
				sftpClient.CloseHandle(Handle);
			}
			else if (state == STATE_DOWNLOAD_OPEN)
			{
				currentHandle = Handle;
				ShowProgressWindow();
				sftpClient.Read(Handle, currentFile.Position, FILE_BLOCK_SIZE);
				state = STATE_DOWNLOAD_RECEIVE;
			}
			else if (state == STATE_UPLOAD_OPEN)
			{
				currentHandle = Handle;
				ShowProgressWindow();
				WriteNextBlockToFile();
				state = STATE_UPLOAD_SEND;
			}
		}

		private void sftpClient_OnError(object Sender, int ErrorCode, string Comment)
		{
			if (state == STATE_READ_DIRECTORY_SENT &&
				ErrorCode == SBSftpCommon.Unit.SSH_ERROR_EOF)
			{
				LogMessage("File list received.");
				CloseCurrentHandle();
				OutputFileList();
			}
			else if (state == STATE_DOWNLOAD_RECEIVE && 
				ErrorCode == SBSftpCommon.Unit.SSH_ERROR_EOF)
			{
				LogMessage("File received.");
				if (currentFile != null)
				{
					currentFile.Close();
					currentFile = null;
				}
			}
            else
                LogMessage(String.Format("Error {0} with comment \"{1}\".", Convert.ToString(ErrorCode, 10), Comment));  
		}

		private void sftpClient_OnSuccess(object Sender, string Comment)
		{
			if (state == STATE_CHANGE_DIR)
			{
				LogMessage(String.Format("Operation succeeded with comment \"{0}\"", Comment));
				RequestAbsolutePath(currentDir + '/' + relDir + '/');
			}
			else if (state == STATE_MAKE_DIR || 
				state == STATE_RENAME ||
				state == STATE_REMOVE)
			{
				LogMessage(String.Format("Operation succeeded with comment \"{0}\"", Comment));
				BuildFileList(currentDir);
			}
			else if (state == STATE_UPLOAD_SEND)
			{
				if (progress != null)
					SetProgressBarValue((Int32)(100 * currentFile.Position / currentFileSize));
				WriteNextBlockToFile();
			}
			else if (state == STATE_CLOSE_HANDLE)
			{
				if (currentFile != null)
				{
					currentFile.Close();
					currentFile = null;
				}
				BuildFileList(currentDir);
			}
		}

		private void sftpClient_OnDirectoryListing(object Sender, TElSftpFileInfo[] Listing)
		{
			TElSftpFileInfo fileInfo;
			if (state == STATE_READ_DIRECTORY_SENT)
			{
				for (int i = 0; i < Listing.Length; i++)
				{
					fileInfo = new TElSftpFileInfo();
					Listing[i].CopyTo(fileInfo);
					currentFileList.Add(fileInfo);
				}
				sftpClient.ReadDirectory(currentHandle);
			}
		}

		private void sftpClient_OnData(object Sender, byte[] Buffer)
		{
			if (currentFile == null)
				return;
			if (state == STATE_DOWNLOAD_RECEIVE)
			{
				currentFile.Write(Buffer, 0, Buffer.Length);
				if (currentFile.Position >= currentFileSize)
				{
					currentFile.Close();
					currentFile = null;
					CloseProgressWindow();
					LogMessage("File received");
					CloseCurrentHandle();
				}
				else
				{
					sftpClient.Read(currentHandle, currentFile.Position, FILE_BLOCK_SIZE);
					SetProgressBarValue((Int32)(100 * currentFile.Position / currentFileSize));
				}
			}
		}

		private void sftpClient_OnAbsolutePath(object Sender, string Path)
		{
			currentDir = Path;
			BuildFileList(currentDir);
            SetEditText(Path, editPath);
		}

		private void sftpClient_OnFileAttributes(object Sender, TElSftpFileAttributes Attributes)
		{
			ListViewItem lvi = lvFiles.SelectedItems[0];
			TElSftpFileInfo info = (TElSftpFileInfo)lvi.Tag;
			if ((Attributes.IncludedAttributes & SBSftpCommon.Unit.saSize) > 0)
				info.Attributes.Size = Attributes.Size;
			if ((Attributes.IncludedAttributes & SBSftpCommon.Unit.saUID) > 0)
				info.Attributes.UID = Attributes.UID;
			if ((Attributes.IncludedAttributes & SBSftpCommon.Unit.saGID) > 0)
				info.Attributes.GID = Attributes.GID;
			if ((Attributes.IncludedAttributes & SBSftpCommon.Unit.saATime) > 0)
				info.Attributes.ATime = Attributes.ATime;
			if ((Attributes.IncludedAttributes & SBSftpCommon.Unit.saMTime) > 0)
				info.Attributes.MTime = Attributes.MTime;
			if ((Attributes.IncludedAttributes & SBSftpCommon.Unit.saExtended) > 0)
			{
				for(int i = 0; i < Attributes.ExtendedAttributeCount; i++) 
				{
					int j = info.Attributes.AddExtendedAttribute();
					info.Attributes.get_ExtendedAttributes(j).ExtType = Attributes.get_ExtendedAttributes(i).ExtType;
					info.Attributes.get_ExtendedAttributes(j).ExtData = Attributes.get_ExtendedAttributes(i).ExtData;
				}
			}
            
			info.Attributes.Directory = Attributes.Directory;
			if ((Attributes.IncludedAttributes & SBSftpCommon.Unit.saPermissions) > 0)
			{
				info.Attributes.UserRead = Attributes.UserRead;
				info.Attributes.UserWrite = Attributes.UserWrite;
				info.Attributes.UserExecute = Attributes.UserExecute;
				info.Attributes.GroupRead = Attributes.GroupRead;
				info.Attributes.GroupWrite = Attributes.GroupWrite;
				info.Attributes.GroupExecute = Attributes.GroupExecute;
				info.Attributes.OtherRead = Attributes.OtherRead;
				info.Attributes.OtherWrite = Attributes.OtherWrite;
				info.Attributes.OtherExecute = Attributes.OtherExecute;
			}

			lvi.SubItems.Clear();
			lvi.Text = info.Name;
			lvi.SubItems.Add(Convert.ToString(info.Attributes.Size, 10));
			lvi.SubItems.Add(WritePermissions(info.Attributes));
		}
		#endregion

		#region Auxiliary routines

		private String WritePermissions(TElSftpFileAttributes attributes)
		{
			StringBuilder sb = new StringBuilder();
			if (attributes.Directory)
				sb.Append('d');
			if (attributes.UserRead)
				sb.Append('r');
			else
				sb.Append('-');
			if (attributes.UserWrite)
				sb.Append('w');
			else
				sb.Append('-');
			if (attributes.UserExecute)
				sb.Append('x');
			else
				sb.Append('-');
			if (attributes.GroupRead)
				sb.Append('r');
			else
				sb.Append('-');
			if (attributes.GroupWrite)
				sb.Append('w');
			else
				sb.Append('-');
			if (attributes.GroupExecute)
				sb.Append('x');
			else
				sb.Append('-');
			if (attributes.OtherRead)
				sb.Append('r');
			else
				sb.Append('-');
			if (attributes.OtherWrite)
				sb.Append('w');
			else
				sb.Append('-');
			if (attributes.OtherExecute)
				sb.Append('x');
			else
				sb.Append('-');

			return sb.ToString();
		}

		private void ShowProgressWindow()
		{
			if (progress != null)
				progress.CloseDialog();
			progress = ProgressWindow.ShowDialog(this, 
                new ProgressWindow.OnAbortingDialog(OnClosingProgressDialog));
		}

		private void CloseProgressWindow()
		{
			if (progress != null)
			{
				progress.CloseDialog();
				progress = null;
			}
		}

		private void SetProgressBarValue(Int32 val)
		{
			if (progress != null)
				progress.ProgressBarValue = val;
		}

		public void OnClosingProgressDialog(System.ComponentModel.CancelEventArgs e)
		{
			CloseCurrentHandle();
			currentFile.Close();
			currentFile = null;
			CloseProgressWindow();
			LogMessage("Transfer aborted");
		}

		#endregion

		private void sshClient_OnAuthenticationKeyboard(object Sender, TElStringList Prompts, bool[] Echo, TElStringList Responses)
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
}
