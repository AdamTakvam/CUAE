using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Metreos.Max.Core;

 

namespace Metreos.Max.Framework.ToolsOptions
{
    /// <summary>Tools/Options "Build" tab</summary>
    public class MaxOptsServer: UserControl, IMaxToolsOptions
    {
        #region dialog controls
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ComboBox comboFolder;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.ComboBox fwverCombo;  
        private System.Windows.Forms.TextBox asIP1;
        private System.Windows.Forms.TextBox asIP2;
        private System.Windows.Forms.TextBox asIP3;
        private System.Windows.Forms.TextBox asPort;
        private System.Windows.Forms.TextBox asIP4;
        private System.Windows.Forms.TextBox tbPass;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.GroupBox gpIpPort;
        private System.Windows.Forms.GroupBox gbAdmin;    
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelPass; 
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;  
        private System.Windows.Forms.Label label5; 
        private System.Windows.Forms.Label label0;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox asTime;
        private System.Windows.Forms.Label label1; 
        #endregion

        public MaxOptsServer()
        {
            InitializeComponent();
            this.Size = Const.toolsOptionsControlSize;
        }

        /// <summary>Initialize controls</summary>
        private void MaxOptsServer_Load(object sender, System.EventArgs e)
        {
            asIP1.Tag = 0; asIP2.Tag = 1;  asIP3.Tag = 2; asIP4.Tag = 3;
            ipctrl[0] = asIP1; ipctrl[1] = asIP2; ipctrl[2] = asIP3; ipctrl[3] = asIP4;
            bool badUser, badPass, badIP, badPort, badTime;
  
            this.caption = Const.PromptFrameworkFolderLocation;
                                            // Get current port & IP
            string regPort = Config.AppServerPort;
            appServerPort  = regPort == null || regPort.Length == 0? 
                Config.DefaultAppServerPort: regPort;
            badPort = Utl.atoi(appServerPort) <= 0;
            this.asPort.Text = badPort? Const.szero: appServerPort;

            string timeOut = Config.SshTimeOut;
            sshTimeOut = timeOut == null || timeOut.Length == 0 ?
                Config.DefaultSshTimeOut : timeOut;
            badTime = Utl.atoi(sshTimeOut) <= 0;
            this.asTime.Text = badTime ? Config.DefaultSshTimeOut : sshTimeOut;

            string regIP = Config.AppServerIP;           
            badIP = regIP == null || regIP.Length == 0;   
            appServerIP = badIP? Const.ipzero: regIP;    
         
            this.IpStringToDialog();
                                          
            this.usernamePersisted = Config.AppServerAdminUser;
            this.userpassPersisted = Config.AppServerAdminPass;
            badUser = usernamePersisted == null || usernamePersisted.Length == 0;
            badPass = userpassPersisted == null || userpassPersisted.Length == 0;
            this.tbUserName.Text = badUser? String.Empty: usernamePersisted;
            this.tbPass.Text     = badPass? String.Empty: Const.PasswordAsterisks;
            this.isPasswordAsterisks = !badPass;

                                            // Get current framework dir
            regFrameworkFolder   = Config.FrameworkDirectory;
                                            
            this.frameworkFolder = Const.DefaultLocalLibrariesFolder;
                                             
            string folder = regFrameworkFolder;  

            if  (folder == null) 
                 folder  = this.frameworkFolder;
            else                                 
            if  (folder != this.frameworkFolder)  // Populate fw dir choices
                 comboFolder.Items.Add(this.frameworkFolder);

            comboFolder.SelectedIndex = comboFolder.Items.Add(folder);

            this.PopulateFrameworkVersion();      // Load and set fw version
            this.configuredFrameworkVersion = Config.FrameworkVersion;
            this.fwverCombo.SelectedItem = configuredFrameworkVersion;

            if (badUser) errmask |= 0x1;            
            if (badPass) errmask |= 0x2;
            if (badIP)   errmask |= 0x4; 
            if (badPort) errmask |= 0x8;
            if (badTime) errmask |= 0x16;

            if ((errmask & 0x1) != 0) 
            {   this.tbUserName.SelectAll();
                this.tbUserName.Focus();
            }
            else
            if ((errmask & 0x2) != 0) 
            {   this.tbPass.SelectAll();
                this.tbPass.Focus();
            }
            else
            if ((errmask & 0x4) != 0) 
            {   this.asIP1.SelectAll();
                this.asIP1.Focus();
            }
            else
            if ((errmask & 0x8) != 0) 
            {   this.asPort.SelectAll();
                this.asPort.Focus();
            }
            else
            if ((errmask & 0x16) != 0)
            {   this.asTime.SelectAll();
                this.asTime.Focus();
            }
        }         


        /// <summary>Persist values on parent OK button click</summary>
        public bool OnOK()
        {
            string folder = (string)comboFolder.Text; 
            btnOK.Enabled = false; 
            this.errmask = 0;

            if (!this.EditUsernameInput())
                return ShowErrorDialog(0x1);

            if (!this.EditPasswordInput())   
                return ShowErrorDialog(0x2);

            if (!EditIpInput()) 
                return ShowErrorDialog(0x4);  

            if (!EditPortInput()) 
                return ShowErrorDialog(0x8);

            if (!EditTimeInput())
                return ShowErrorDialog(0x16);

            if (!Directory.Exists(folder)) return false;

            btnOK.Enabled = true;  
            this.frameworkFolder = folder;  
       
            if (this.frameworkFolder != this.regFrameworkFolder) 
                Config.FrameworkDirectory = this.frameworkFolder;                    

            if (this.tbUserName.Text.Length > 0)
                Config.AppServerAdminUser = this.tbUserName.Text;

                                            // Encode and persist password
            if (this.tbPass.Text.Length > 0 && !this.isPasswordAsterisks)  
                Config.AdminPassword = this.tbPass.Text;

            if (this.port != String.Empty && Utl.atoi(this.port) > 0)
                Config.AppServerPort = this.port;

            if (this.time != String.Empty && Utl.atoi(this.time) > 0)
                Config.SshTimeOut = this.time;

            if (this.fwverCombo.Text != this.configuredFrameworkVersion)
                Config.FrameworkVersion = this.fwverCombo.Text;

            this.SetNewIP();

            return true; 
        }


        public bool ShowErrorDialog(int which)
        {
            string s = null;
            btnOK.Enabled = false;

            switch(which)
            {
               case 0x1: 
                    s = Const.badUsername;  
                    this.tbUserName.SelectAll();
                    this.tbUserName.Focus();
                    break;
               case 0x2: 
                    s = Const.badPassword; 
                    this.tbPass.SelectAll();
                    this.tbPass.Focus();
                    break;
               case 0x4: 
                    s = Const.badIpAddr;
                    this.IpSelectAll(); 
                    this.asIP1.Focus();
                    break;
               case 0x8: 
                    s = Const.badPort; 
                    this.asPort.Focus();
                    break;
                case 0x16:
                    s = Const.badTime;
                    this.asTime.Focus();
                    break;
            }

            if (s != null)
                MessageBox.Show(Manager.MaxManager.Instance, s,
                    Const.badEntryDlgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation); 

            return false;
        }



        /// <summary>Handle Browse button click</summary>
        private void OnBrowseButtonClick(object sender, System.EventArgs e)
        {
            string folder = new MaxFolderBrowser().ShowDialog(this.caption);
            if  (!Utl.ValidateMaxPath(folder)) return;

            int  ndx = comboFolder.FindStringExact(folder);
            if  (ndx == -1)
                 ndx = comboFolder.Items.Add(folder);

            comboFolder.SelectedIndex = ndx;  
            btnOK.Focus();
        }


        /// <summary>Populate dialog IP chunks from IP string</summary>
        private void IpStringToDialog()
        {
            string s = this.appServerIP;

            for(int i=0; i < 4; i++)
            {
                TextBox box = ipctrl[i];
                int  dotpos = s.IndexOf(Const.dot);

                if  (dotpos == -1)
                     box.Text = s;
                else
                {    box.Text = s.Substring(0, dotpos);
                     s = s.Substring(dotpos+1);
                }
            }
        }


        /// <summary>Set app server IP from dialog entries</summary>
        private void SetNewIP()
        {
            bool isEntry = true;
            for(int i=0; i < 4; i++) if (ipctrl[i].Text == String.Empty) isEntry = false;
            if (!isEntry) return;  

            System.Text.StringBuilder newIP = new System.Text.StringBuilder();                                          

            newIP.Append(ipctrl[0].Text);
            for(int i=1; i < 4; i++) { newIP.Append(Const.dot); newIP.Append(ipctrl[i].Text); }

            Config.AppServerIP = newIP.ToString(); // Set registry
        }


        /// <summary>Select all IP address text</summary>
        private void IpSelectAll()
        {  
            ipctrl[0].SelectAll();       
        }  


        /// <summary>Validate app server IP input</summary>
        private bool EditIpInput()
        {
            bool gap = false, err = false;
            int  sum = 0;

            for(int i=0; i < 4; i++) 
            {  
                string ipchunk = ipctrl[i].Text;
                if (ipchunk == String.Empty) 
                    gap = true;
                else 
                {   int val = Utl.atoi(ipchunk);
                    if((i==0 || i==3)&& (val==0))
                        err = true;
                    if (val > 254) 
                        err = true;
                    else
                    {   sum += val;               
                        ipctrl[i].Text = val.ToString(); // Nix LZs
                    }
                }
            }

            return !gap && !err && sum > 0;
        }


        /// <summary>Validate app server port input</summary>
        private bool EditPortInput()
        {
            int val = Utl.atoi(port); 
            port = val.ToString(); // Nix leading zeros      
            return val != 0;
        }

        /// <summary>Validate SSH Time out input</summary>
        private bool EditTimeInput()
        {
            int val = Utl.atoi(time);
            time = val.ToString();
            return val != 0;
        }

        /// <summary>Validate app server admin name input</summary>
        private bool EditUsernameInput()
        {
            return Utl.EditUsername(this.tbUserName.Text);         
        }


        /// <summary>Validate app server password input</summary>
        private bool EditPasswordInput()
        {
            if (this.isPasswordAsterisks) return true;
            return Utl.EditPassword(this.tbPass.Text); 
        }



        /// <summary>Reenable OK button on framework folder input</summary>
        private void OnTextInput(object sender, System.EventArgs e)
        {
            btnOK.Enabled = true;
        }


        /// <summary>Load framework versions choices to combobox</summary>
        private void PopulateFrameworkVersion()
        {
            string frameworkDir = (string)comboFolder.Text; 
            if (!Directory.Exists(frameworkDir)) return;

            string[] subdirs = Directory.GetDirectories(frameworkDir);
            if (subdirs.Length == 0) return;

            this.fwverCombo.Items.Clear();
            int i=-1;
      
            foreach(string subdir in subdirs)       
            {   this.fwverCombo.Items.Add(Utl.GetLastDirectory(subdir)); 
                i++;
            }  

            this.fwverCombo.SelectedIndex = i >= 0? i: 0;         
        }


        /// <summary>Load framework versions choices combobox on drop</summary>
        private void OnFrameworkVersionDropdown(object sender, System.EventArgs e)
        {
            this.PopulateFrameworkVersion();
        }


        private void MaxOptionsDlg_Load(object sender, System.EventArgs e)
        {
    
        } 


        /// <summary>Edit IP address chunk entry</summary>
        private void OnIpSegmentTextChanged(object sender, System.EventArgs e)
        {
            btnOK.Enabled = true;
            TextBox text = sender as TextBox;
            bool ok = false;
            int   i = (int)text.Tag;
            int   n = 0;
            try { n = System.Convert.ToInt32(text.Text); ok = n >= 0; } 
            catch { }
            if  (ok || text.Text == Const.emptystr) 
                 this.ipseg[i] = text.Text;
            else text.Text = this.ipseg[i];
        }


        /// <summary>Edit port number entry</summary>
        private void OnPortTextChanged(object sender, System.EventArgs e)
        {
            btnOK.Enabled = true;
            errmask &= ~(uint)0x8;
            TextBox text = sender as TextBox;
            bool ok = false;
            int   n = 0;
            try { n = System.Convert.ToInt32(text.Text); ok = n >= 0; } 
            catch { }
            if  (ok || text.Text == Const.emptystr) 
                 this.port = text.Text;
            else text.Text = this.port;
        }

        /// <summary>Edit SSH Time Out Entry</summary>
        private void OnTimeTextChanged(object sender, System.EventArgs e)
        {
            btnOK.Enabled = true;
            errmask &= ~(uint)0x16;
            TextBox text = sender as TextBox;
            bool ok = false;
            int n = 0;
            try { n = System.Convert.ToInt32(text.Text); ok = n >= 0; }
            catch { }
            if (ok || text.Text == Const.emptystr)
                this.time = text.Text;
            else
                text.Text = this.time;
        }

        private void OnPasswordMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.isPasswordAsterisks) return;
            this.tbPass.Text = String.Empty;
            errmask &= ~(uint)0x2;
        }


        private void OnPasswordTextChanged(object sender, System.EventArgs e)
        {
            btnOK.Enabled = true;
            this.isPasswordAsterisks = false;
            errmask &= ~(uint)0x2;
        }


        private void OnUserNameTextChanged(object sender, System.EventArgs e)
        {
            btnOK.Enabled = true;
            errmask &= ~(uint)0x1;
        }


        private void OnUserNameMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            errmask &= ~(uint)0x1;
        }


        private void OnIP1MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            errmask &= ~(uint)0x4;
        }
    

        /// <summary>Subdialog to browse for folder</summary>
        public class MaxFolderBrowser: System.Windows.Forms.Design.FolderNameEditor
        {
            private string path;
            public  string Path  { get { return path; } set { path = value; } }

            public string ShowDialog(string caption)
            {
                FolderBrowser fb = new FolderBrowser();
                fb.Description   = caption;  
    
                DialogResult result = fb.ShowDialog();

                this.path  = result == DialogResult.OK? fb.DirectoryPath: null;
                return this.path;
            }
        } // inner class MaxFolderBrowser


        private Button btnOK;
        public  Button OkButton { set { btnOK = value; } }
        private string regFrameworkFolder;
        private string frameworkFolder;
        private string appServerIP;
        private string appServerPort;
        private string sshTimeOut;
        private string configuredFrameworkVersion;
        private string caption;
        private string usernamePersisted;
        private string userpassPersisted;
        private string port = Const.emptystr;
        private string time = Const.emptystr;
        private string[] ipseg
            = new string[]  {Const.emptystr, Const.emptystr, Const.emptystr, Const.emptystr};
        private TextBox[] ipctrl = new TextBox[4];
        private bool isPasswordAsterisks;
        private uint errmask = 0;

        private System.ComponentModel.Container components = null;

        #region Component Designer generated code
		
        private void InitializeComponent()
        {
            this.comboFolder = new System.Windows.Forms.ComboBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label0 = new System.Windows.Forms.Label();
            this.gpIpPort = new System.Windows.Forms.GroupBox();
            this.asIP4 = new System.Windows.Forms.TextBox();
            this.asIP3 = new System.Windows.Forms.TextBox();
            this.asIP2 = new System.Windows.Forms.TextBox();
            this.asPort = new System.Windows.Forms.TextBox();
            this.asIP1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.fwverCombo = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.gbAdmin = new System.Windows.Forms.GroupBox();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.labelPass = new System.Windows.Forms.Label();
            this.labelUser = new System.Windows.Forms.Label();
            this.tbPass = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label7 = new System.Windows.Forms.Label();
            this.asTime = new System.Windows.Forms.TextBox();
            this.gpIpPort.SuspendLayout();
            this.gbAdmin.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboFolder
            // 
            this.comboFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboFolder.Location = new System.Drawing.Point(1, 17);
            this.comboFolder.Name = "comboFolder";
            this.comboFolder.Size = new System.Drawing.Size(389, 21);
            this.comboFolder.TabIndex = 4;
            this.comboFolder.TextChanged += new System.EventHandler(this.OnTextInput);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBrowse.Location = new System.Drawing.Point(316, 44);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 5;
            this.btnBrowse.Text = "Browse ...";
            this.btnBrowse.Click += new System.EventHandler(this.OnBrowseButtonClick);
            // 
            // label0
            // 
            this.label0.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label0.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label0.Location = new System.Drawing.Point(1, 1);
            this.label0.Name = "label0";
            this.label0.Size = new System.Drawing.Size(120, 16);
            this.label0.TabIndex = 0;
            this.label0.Text = "Framework Directory";
            // 
            // gpIpPort
            // 
            this.gpIpPort.Controls.Add(this.asIP4);
            this.gpIpPort.Controls.Add(this.asIP3);
            this.gpIpPort.Controls.Add(this.asIP2);
            this.gpIpPort.Controls.Add(this.asPort);
            this.gpIpPort.Controls.Add(this.asIP1);
            this.gpIpPort.Controls.Add(this.label8);
            this.gpIpPort.Controls.Add(this.label6);
            this.gpIpPort.Controls.Add(this.label1);
            this.gpIpPort.Controls.Add(this.label2);
            this.gpIpPort.Controls.Add(this.label3);
            this.gpIpPort.Controls.Add(this.label5);
            this.gpIpPort.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gpIpPort.Location = new System.Drawing.Point(1, 82);
            this.gpIpPort.Name = "gpIpPort";
            this.gpIpPort.Size = new System.Drawing.Size(224, 61);
            this.gpIpPort.TabIndex = 0;
            this.gpIpPort.TabStop = false;
            this.gpIpPort.Text = "Application Server Network Address";
            // 
            // asIP4
            // 
            this.asIP4.Location = new System.Drawing.Point(127, 30);
            this.asIP4.MaxLength = 8;
            this.asIP4.Name = "asIP4";
            this.asIP4.Size = new System.Drawing.Size(25, 20);
            this.asIP4.TabIndex = 10;
            this.asIP4.WordWrap = false;
            this.asIP4.TextChanged += new System.EventHandler(this.OnIpSegmentTextChanged);
            // 
            // asIP3
            // 
            this.asIP3.Location = new System.Drawing.Point(88, 30);
            this.asIP3.MaxLength = 7;
            this.asIP3.Name = "asIP3";
            this.asIP3.Size = new System.Drawing.Size(25, 20);
            this.asIP3.TabIndex = 9;
            this.asIP3.WordWrap = false;
            this.asIP3.TextChanged += new System.EventHandler(this.OnIpSegmentTextChanged);
            // 
            // asIP2
            // 
            this.asIP2.Location = new System.Drawing.Point(48, 30);
            this.asIP2.MaxLength = 6;
            this.asIP2.Name = "asIP2";
            this.asIP2.Size = new System.Drawing.Size(25, 20);
            this.asIP2.TabIndex = 8;
            this.asIP2.WordWrap = false;
            this.asIP2.TextChanged += new System.EventHandler(this.OnIpSegmentTextChanged);
            // 
            // asPort
            // 
            this.asPort.Location = new System.Drawing.Point(180, 30);
            this.asPort.MaxLength = 9;
            this.asPort.Name = "asPort";
            this.asPort.Size = new System.Drawing.Size(36, 20);
            this.asPort.TabIndex = 11;
            this.asPort.WordWrap = false;
            this.asPort.TextChanged += new System.EventHandler(this.OnPortTextChanged);
            // 
            // asIP1
            // 
            this.asIP1.Location = new System.Drawing.Point(8, 30);
            this.asIP1.MaxLength = 3;
            this.asIP1.Name = "asIP1";
            this.asIP1.Size = new System.Drawing.Size(25, 20);
            this.asIP1.TabIndex = 7;
            this.asIP1.WordWrap = false;
            this.asIP1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnIP1MouseDown);
            this.asIP1.TextChanged += new System.EventHandler(this.OnIpSegmentTextChanged);
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label8.Location = new System.Drawing.Point(114, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(8, 24);
            this.label8.TabIndex = 0;
            this.label8.Text = ".";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label6.Location = new System.Drawing.Point(1185, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(4, 16);
            this.label6.TabIndex = 19;
            this.label6.Text = ".";
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(9, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP";
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(180, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Port";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label3.Location = new System.Drawing.Point(34, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(8, 22);
            this.label3.TabIndex = 0;
            this.label3.Text = ".";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label5.Location = new System.Drawing.Point(74, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(8, 24);
            this.label5.TabIndex = 0;
            this.label5.Text = ".";
            // 
            // fwverCombo
            // 
            this.fwverCombo.Location = new System.Drawing.Point(104, 44);
            this.fwverCombo.Name = "fwverCombo";
            this.fwverCombo.Size = new System.Drawing.Size(60, 21);
            this.fwverCombo.TabIndex = 6;
            this.fwverCombo.DropDown += new System.EventHandler(this.OnFrameworkVersionDropdown);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(0, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Framework Version";
            // 
            // gbAdmin
            // 
            this.gbAdmin.Controls.Add(this.tbUserName);
            this.gbAdmin.Controls.Add(this.labelPass);
            this.gbAdmin.Controls.Add(this.labelUser);
            this.gbAdmin.Controls.Add(this.tbPass);
            this.gbAdmin.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbAdmin.Location = new System.Drawing.Point(1, 189);
            this.gbAdmin.Name = "gbAdmin";
            this.gbAdmin.Size = new System.Drawing.Size(298, 61);
            this.gbAdmin.TabIndex = 0;
            this.gbAdmin.TabStop = false;
            this.gbAdmin.Text = "Administrator";
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(8, 30);
            this.tbUserName.MaxLength = 60;
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(176, 20);
            this.tbUserName.TabIndex = 12;
            this.tbUserName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnUserNameMouseDown);
            this.tbUserName.TextChanged += new System.EventHandler(this.OnUserNameTextChanged);
            // 
            // labelPass
            // 
            this.labelPass.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelPass.Location = new System.Drawing.Point(194, 15);
            this.labelPass.Name = "labelPass";
            this.labelPass.Size = new System.Drawing.Size(64, 15);
            this.labelPass.TabIndex = 0;
            this.labelPass.Text = "Password";
            // 
            // labelUser
            // 
            this.labelUser.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelUser.Location = new System.Drawing.Point(10, 15);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(64, 15);
            this.labelUser.TabIndex = 0;
            this.labelUser.Text = "User Name";
            // 
            // tbPass
            // 
            this.tbPass.Location = new System.Drawing.Point(192, 30);
            this.tbPass.MaxLength = 20;
            this.tbPass.Name = "tbPass";
            this.tbPass.PasswordChar = '*';
            this.tbPass.Size = new System.Drawing.Size(98, 20);
            this.tbPass.TabIndex = 13;
            this.tbPass.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnPasswordMouseDown);
            this.tbPass.TextChanged += new System.EventHandler(this.OnPasswordTextChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(1, 157);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(131, 17);
            this.label7.TabIndex = 7;
            this.label7.Text = "SSH Timeout (seconds)";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // asTime
            // 
            this.asTime.Location = new System.Drawing.Point(127, 154);
            this.asTime.MaxLength = 9;
            this.asTime.Name = "asTime";
            this.asTime.Size = new System.Drawing.Size(36, 20);
            this.asTime.TabIndex = 12;
            this.asTime.WordWrap = false;
            this.asTime.TextChanged += new System.EventHandler(this.OnTimeTextChanged);
            // 
            // MaxOptsServer
            // 
            this.Controls.Add(this.asTime);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.gbAdmin);
            this.Controls.Add(this.fwverCombo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.gpIpPort);
            this.Controls.Add(this.comboFolder);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label0);
            this.Name = "MaxOptsServer";
            this.Size = new System.Drawing.Size(395, 287);
            this.Load += new System.EventHandler(this.MaxOptsServer_Load);
            this.gpIpPort.ResumeLayout(false);
            this.gpIpPort.PerformLayout();
            this.gbAdmin.ResumeLayout(false);
            this.gbAdmin.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        protected override void Dispose( bool disposing )
        {
            if (disposing && components != null) components.Dispose();				
            base.Dispose(disposing);
        }
        #endregion

        private void label7_Click(object sender, EventArgs e)
        {

        }

    } // class MaxOptsServer:
}   // namespace
