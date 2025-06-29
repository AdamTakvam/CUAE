using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

namespace SSHServer.NET
{
	/// <summary>
	/// Implements server configuration UI
	/// </summary>
	public class frmSettings : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox edIP;
		private System.Windows.Forms.Label lblIPAddress;
		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.TextBox edPort;
		private System.Windows.Forms.CheckBox cbForceCompression;
		private System.Windows.Forms.GroupBox gbAllowedSubsystem;
		private System.Windows.Forms.CheckBox cbAlowSFTP;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Label lblHostKey;
		public System.Windows.Forms.TextBox memHostKey;
		private System.Windows.Forms.Button btnGenerateNewKey;
		private System.Windows.Forms.ImageList ilUser;
		private System.Windows.Forms.Button btnAddUser;
		private System.Windows.Forms.Button btnRemoveUser;
		private System.Windows.Forms.Button btnEditUser;
		private System.Windows.Forms.ListView lvUsers;
		private System.Windows.Forms.ColumnHeader chUserName;
		private System.Windows.Forms.ColumnHeader chAuthentication;
		private System.Windows.Forms.GroupBox gbSettings;
		private System.Windows.Forms.GroupBox gbUsers;
		private System.ComponentModel.IContainer components;

		public frmSettings()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			FillUsers();
			memHostKey.Text = Globals.Settings.ServerKey;
			edIP.Text = Globals.Settings.ServerHost;
			edPort.Text = Globals.Settings.ServerPort.ToString();
			cbForceCompression.Checked = Globals.Settings.ForceCompression;
		}

		// sets up users list view according to user accounts settings
		private void FillUsers()
		{
			lvUsers.Items.Clear();
			for (int i = 0; i < Globals.Settings.Users.Count; i++)
			{
				UserInfo user = (UserInfo)Globals.Settings.Users[i];
				ListViewItem li = lvUsers.Items.Add(user.Name);
				li.SubItems.Add(user.GetAuthType());
				li.ImageIndex = 2;
				li.Tag = i;
			}		
		}
        
		// forces settings update 
		public static void ChangeSettings()
		{
			frmSettings frm = new frmSettings();
			frm.BringToFront();
			if (frm.ShowDialog() == DialogResult.OK)
			{
				try
				{
					Globals.Settings.SetHostKey(frm.memHostKey.Text);
					Globals.Settings.ServerHost = frm.edIP.Text;
					Globals.Settings.ServerPort = int.Parse(frm.edPort.Text);
					Globals.Settings.ForceCompression = frm.cbForceCompression.Checked;
					Globals.Settings.SaveSettings();
				}
				catch(Exception exc)
				{
					Logger.Log("ChangeSettings : " + exc.Message,true);
				}
			}
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmSettings));
			this.gbSettings = new System.Windows.Forms.GroupBox();
			this.btnGenerateNewKey = new System.Windows.Forms.Button();
			this.memHostKey = new System.Windows.Forms.TextBox();
			this.lblHostKey = new System.Windows.Forms.Label();
			this.cbForceCompression = new System.Windows.Forms.CheckBox();
			this.lblPort = new System.Windows.Forms.Label();
			this.edPort = new System.Windows.Forms.TextBox();
			this.lblIPAddress = new System.Windows.Forms.Label();
			this.edIP = new System.Windows.Forms.TextBox();
			this.gbAllowedSubsystem = new System.Windows.Forms.GroupBox();
			this.cbAlowSFTP = new System.Windows.Forms.CheckBox();
			this.gbUsers = new System.Windows.Forms.GroupBox();
			this.lvUsers = new System.Windows.Forms.ListView();
			this.chUserName = new System.Windows.Forms.ColumnHeader();
			this.chAuthentication = new System.Windows.Forms.ColumnHeader();
			this.ilUser = new System.Windows.Forms.ImageList(this.components);
			this.btnEditUser = new System.Windows.Forms.Button();
			this.btnRemoveUser = new System.Windows.Forms.Button();
			this.btnAddUser = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.gbSettings.SuspendLayout();
			this.gbAllowedSubsystem.SuspendLayout();
			this.gbUsers.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbSettings
			// 
			this.gbSettings.Controls.Add(this.btnGenerateNewKey);
			this.gbSettings.Controls.Add(this.memHostKey);
			this.gbSettings.Controls.Add(this.lblHostKey);
			this.gbSettings.Controls.Add(this.cbForceCompression);
			this.gbSettings.Controls.Add(this.lblPort);
			this.gbSettings.Controls.Add(this.edPort);
			this.gbSettings.Controls.Add(this.lblIPAddress);
			this.gbSettings.Controls.Add(this.edIP);
			this.gbSettings.Location = new System.Drawing.Point(0, 0);
			this.gbSettings.Name = "gbSettings";
			this.gbSettings.Size = new System.Drawing.Size(472, 152);
			this.gbSettings.TabIndex = 0;
			this.gbSettings.TabStop = false;
			this.gbSettings.Text = "Server settings";
			// 
			// btnGenerateNewKey
			// 
			this.btnGenerateNewKey.Location = new System.Drawing.Point(344, 40);
			this.btnGenerateNewKey.Name = "btnGenerateNewKey";
			this.btnGenerateNewKey.Size = new System.Drawing.Size(120, 20);
			this.btnGenerateNewKey.TabIndex = 7;
			this.btnGenerateNewKey.Text = "Generate new key...";
			this.btnGenerateNewKey.Click += new System.EventHandler(this.btnGenerateNewKey_Click);
			// 
			// memHostKey
			// 
			this.memHostKey.AcceptsReturn = true;
			this.memHostKey.AutoSize = false;
			this.memHostKey.Location = new System.Drawing.Point(4, 65);
			this.memHostKey.Multiline = true;
			this.memHostKey.Name = "memHostKey";
			this.memHostKey.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.memHostKey.Size = new System.Drawing.Size(460, 79);
			this.memHostKey.TabIndex = 6;
			this.memHostKey.Text = "";
			// 
			// lblHostKey
			// 
			this.lblHostKey.Location = new System.Drawing.Point(8, 45);
			this.lblHostKey.Name = "lblHostKey";
			this.lblHostKey.Size = new System.Drawing.Size(100, 16);
			this.lblHostKey.TabIndex = 5;
			this.lblHostKey.Text = "Host Key :";
			// 
			// cbForceCompression
			// 
			this.cbForceCompression.Location = new System.Drawing.Point(256, 17);
			this.cbForceCompression.Name = "cbForceCompression";
			this.cbForceCompression.Size = new System.Drawing.Size(160, 16);
			this.cbForceCompression.TabIndex = 4;
			this.cbForceCompression.Text = "Force packet compression";
			// 
			// lblPort
			// 
			this.lblPort.Location = new System.Drawing.Point(161, 21);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(32, 16);
			this.lblPort.TabIndex = 3;
			this.lblPort.Text = "Port :";
			// 
			// edPort
			// 
			this.edPort.Location = new System.Drawing.Point(200, 16);
			this.edPort.Name = "edPort";
			this.edPort.Size = new System.Drawing.Size(40, 20);
			this.edPort.TabIndex = 2;
			this.edPort.Text = "22";
			// 
			// lblIPAddress
			// 
			this.lblIPAddress.Location = new System.Drawing.Point(9, 21);
			this.lblIPAddress.Name = "lblIPAddress";
			this.lblIPAddress.Size = new System.Drawing.Size(72, 16);
			this.lblIPAddress.TabIndex = 1;
			this.lblIPAddress.Text = "IP Address :";
			// 
			// edIP
			// 
			this.edIP.Location = new System.Drawing.Point(80, 18);
			this.edIP.Name = "edIP";
			this.edIP.Size = new System.Drawing.Size(72, 20);
			this.edIP.TabIndex = 0;
			this.edIP.Text = "127.0.0.1";
			// 
			// gbAllowedSubsystem
			// 
			this.gbAllowedSubsystem.Controls.Add(this.cbAlowSFTP);
			this.gbAllowedSubsystem.Location = new System.Drawing.Point(0, 152);
			this.gbAllowedSubsystem.Name = "gbAllowedSubsystem";
			this.gbAllowedSubsystem.Size = new System.Drawing.Size(472, 40);
			this.gbAllowedSubsystem.TabIndex = 1;
			this.gbAllowedSubsystem.TabStop = false;
			this.gbAllowedSubsystem.Text = "Allowed subsystem";
			// 
			// cbAlowSFTP
			// 
			this.cbAlowSFTP.Checked = true;
			this.cbAlowSFTP.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbAlowSFTP.Enabled = false;
			this.cbAlowSFTP.Location = new System.Drawing.Point(8, 16);
			this.cbAlowSFTP.Name = "cbAlowSFTP";
			this.cbAlowSFTP.Size = new System.Drawing.Size(160, 16);
			this.cbAlowSFTP.TabIndex = 4;
			this.cbAlowSFTP.Text = "SFTP";
			// 
			// gbUsers
			// 
			this.gbUsers.Controls.Add(this.lvUsers);
			this.gbUsers.Controls.Add(this.btnEditUser);
			this.gbUsers.Controls.Add(this.btnRemoveUser);
			this.gbUsers.Controls.Add(this.btnAddUser);
			this.gbUsers.Location = new System.Drawing.Point(0, 194);
			this.gbUsers.Name = "gbUsers";
			this.gbUsers.Size = new System.Drawing.Size(472, 118);
			this.gbUsers.TabIndex = 5;
			this.gbUsers.TabStop = false;
			this.gbUsers.Text = "Authorised users";
			// 
			// lvUsers
			// 
			this.lvUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					  this.chUserName,
																					  this.chAuthentication});
			this.lvUsers.FullRowSelect = true;
			this.lvUsers.GridLines = true;
			this.lvUsers.LargeImageList = this.ilUser;
			this.lvUsers.Location = new System.Drawing.Point(6, 15);
			this.lvUsers.Name = "lvUsers";
			this.lvUsers.Size = new System.Drawing.Size(360, 96);
			this.lvUsers.SmallImageList = this.ilUser;
			this.lvUsers.TabIndex = 3;
			this.lvUsers.View = System.Windows.Forms.View.Details;
			this.lvUsers.ItemActivate += new System.EventHandler(this.lvUsers_ItemActivate);
			this.lvUsers.SelectedIndexChanged += new System.EventHandler(this.lvUsers_SelectedIndexChanged);
			// 
			// chUserName
			// 
			this.chUserName.Text = "Username";
			this.chUserName.Width = 120;
			// 
			// chAuthentication
			// 
			this.chAuthentication.Text = "Authentication types";
			this.chAuthentication.Width = 220;
			// 
			// ilUser
			// 
			this.ilUser.ImageSize = new System.Drawing.Size(16, 16);
			this.ilUser.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilUser.ImageStream")));
			this.ilUser.TransparentColor = System.Drawing.Color.Fuchsia;
			// 
			// btnEditUser
			// 
			this.btnEditUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnEditUser.ImageIndex = 3;
			this.btnEditUser.ImageList = this.ilUser;
			this.btnEditUser.Location = new System.Drawing.Point(376, 80);
			this.btnEditUser.Name = "btnEditUser";
			this.btnEditUser.Size = new System.Drawing.Size(88, 23);
			this.btnEditUser.TabIndex = 2;
			this.btnEditUser.Text = "Edit...";
			this.btnEditUser.Click += new System.EventHandler(this.btnEditUser_Click);
			// 
			// btnRemoveUser
			// 
			this.btnRemoveUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnRemoveUser.ImageIndex = 1;
			this.btnRemoveUser.ImageList = this.ilUser;
			this.btnRemoveUser.Location = new System.Drawing.Point(376, 48);
			this.btnRemoveUser.Name = "btnRemoveUser";
			this.btnRemoveUser.Size = new System.Drawing.Size(88, 23);
			this.btnRemoveUser.TabIndex = 1;
			this.btnRemoveUser.Text = "Remove";
			this.btnRemoveUser.Click += new System.EventHandler(this.btnRemoveUser_Click);
			// 
			// btnAddUser
			// 
			this.btnAddUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnAddUser.ImageIndex = 0;
			this.btnAddUser.ImageList = this.ilUser;
			this.btnAddUser.Location = new System.Drawing.Point(376, 16);
			this.btnAddUser.Name = "btnAddUser";
			this.btnAddUser.Size = new System.Drawing.Size(88, 23);
			this.btnAddUser.TabIndex = 0;
			this.btnAddUser.Text = "Add";
			this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(392, 314);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(64, 24);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(320, 314);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(64, 24);
			this.btnOk.TabIndex = 6;
			this.btnOk.Text = "Ok";
			// 
			// frmSettings
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(472, 344);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.gbAllowedSubsystem);
			this.Controls.Add(this.gbSettings);
			this.Controls.Add(this.gbUsers);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmSettings";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Server settings";
			this.gbSettings.ResumeLayout(false);
			this.gbAllowedSubsystem.ResumeLayout(false);
			this.gbUsers.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnGenerateNewKey_Click(object sender, System.EventArgs e)
		{
			if (MessageBox.Show("Do you want to generate a new host key for the server?",
				"Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				SBSSHKeyStorage.TElSSHKey Key = new SBSSHKeyStorage.TElSSHKey();
				Key.Generate(SBSSHKeyStorage.Unit.ALGORITHM_RSA, 1024);
				byte[] SaveKey = null;
				int SaveLen = 0;
				Key.SavePrivateKey(ref SaveKey, ref SaveLen, "");
				SaveKey = new byte[SaveLen];
				Key.SavePrivateKey(ref SaveKey, ref SaveLen, "");
				memHostKey.Text = SBUtils.Unit.StringOfBytes(SaveKey);//new ASCIIEncoding().GetString(SaveKey).Replace("\n","\r\n");
				Globals.Settings.ServerKey = memHostKey.Text;
			}
		}

		// returns the index of currently selected user 
		private int GetUserNumber()
		{
			try
			{
				return int.Parse(lvUsers.FocusedItem.Tag.ToString());
			}
			catch(Exception) 
			{
				return -1;
			}
		}

		private void btnEditUser_Click(object sender, System.EventArgs e)
		{
			if (Globals.Settings.EditUser(GetUserNumber())) FillUsers();
		}

		private void btnRemoveUser_Click(object sender, System.EventArgs e)
		{
			if (Globals.Settings.RemoveUser(GetUserNumber())) FillUsers();
		}

		private void btnAddUser_Click(object sender, System.EventArgs e)
		{
			if (Globals.Settings.AddUser()) 
			{
				Globals.Settings.EditUser(Globals.Settings.Users.Count - 1);
			}
			FillUsers();
		}

		private void lvUsers_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			bool AllowEdit = (lvUsers.FocusedItem != null) && (lvUsers.Focused);
			btnRemoveUser.Enabled = AllowEdit;
			btnEditUser.Enabled = AllowEdit;
		}

		private void lvUsers_ItemActivate(object sender, System.EventArgs e)
		{
			bool AllowEdit = (lvUsers.FocusedItem != null) && (lvUsers.Focused);
			btnRemoveUser.Enabled = AllowEdit;
			btnEditUser.Enabled = AllowEdit;			
		}
	}
}
