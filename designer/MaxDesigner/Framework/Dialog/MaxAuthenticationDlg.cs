//
// MaxAuthenticationDlg
//
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;




namespace Metreos.Max.Framework
{
    /// <summary>Dialog from which to enter username/password</summary>
    public class MaxAuthenticationDlg: Form
    {
        #region dialog controls
        private System.Windows.Forms.Button  btnOK;
        private System.Windows.Forms.Button  btnCancel;
        private System.Windows.Forms.GroupBox gbAdmin;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.Label labelPass;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.TextBox tbPass;
        private System.Windows.Forms.GroupBox gpIpPort;
        private System.Windows.Forms.TextBox asIP4;
        private System.Windows.Forms.TextBox asIP3;
        private System.Windows.Forms.TextBox asIP2;
        private System.Windows.Forms.TextBox asPort;
        private System.Windows.Forms.TextBox asIP1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label textcontent;
        private System.ComponentModel.Container components = null;
        #endregion


        public MaxAuthenticationDlg(string content)
        {     
            InitializeComponent(); 
            this.textcontent.Text = content != null? content: Const.DefaultAuthItemsMsg;
        }


        private void MaxAuthenticationDlg_Load(object sender, System.EventArgs e)
        {
            asIP1.Tag = 0; asIP2.Tag = 1;  asIP3.Tag = 2; asIP4.Tag = 3;
            ipctrl[0] = asIP1; ipctrl[1] = asIP2; ipctrl[2] = asIP3; ipctrl[3] = asIP4;
            bool badUser, badPass, badIP;
  
            // Get current port & IP
            string regPort = Config.AppServerPort;
            appServerPort  = regPort == null || regPort.Length == 0? 
                Config.DefaultAppServerPort: regPort;
            this.asPort.Text = appServerPort; 
            this.asPort.ReadOnly = true;
      
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

            if (badUser) errmask |= 0x1;            
            if (badPass) errmask |= 0x2;
            if (badIP)   errmask |= 0x4;  
        } 


        protected override void OnActivated(EventArgs e)
        {     
            if ((errmask & 0x1) != 0) 
            {   this.tbUserName.SelectAll();
                this.tbUserName.Focus();
            }
            else
            if ((errmask & 0x2) != 0) 
            {  this.tbPass.SelectAll();
               this.tbPass.Focus();
            }
            else
            if ((errmask & 0x4) != 0) 
            {   this.asIP1.SelectAll();
                this.asIP1.Focus();
            }

            base.OnActivated (e);
        }
      

        private void OnClick(object sender, System.EventArgs e)
        {
            Button button = (Button) sender;

            DialogResult result
              = button == btnOK? DialogResult.OK:
                button == btnCancel? DialogResult.Cancel:
                DialogResult.None; 
      
            switch(result)
            {
               case DialogResult.None:                                    
                    btnOK.Focus();                   
                    break;

               case DialogResult.OK:                
                    this.DialogResult = this.OnOK()? result: DialogResult.None;                             
                    break;

               case DialogResult.Cancel:            
                    this.DialogResult = result;
                    break;
            }    
        }          


        /// <summary>Edit and persist values on OK button click</summary>
        public bool OnOK()
        {
            this.errmask = 0;

            if (!this.EditUsernameInput())
                return ShowErrorDialog(0x1);

            if (!this.EditPasswordInput())   
                return ShowErrorDialog(0x2);

            if (!EditIpInput()) 
                return ShowErrorDialog(0x4);  

            if (!EditPortInput()) 
                 return ShowErrorDialog(0x8); 

            if (this.tbUserName.Text.Length > 0)
                Config.AppServerAdminUser = this.tbUserName.Text;
                                            // Encode and persist password
            if (this.tbPass.Text.Length > 0 && !this.isPasswordAsterisks)  
                Config.AdminPassword = this.tbPass.Text;

            this.SetNewIP();

            if (this.port != String.Empty && Utl.atoi(this.port) > 0)
                Config.AppServerPort = this.port; 

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
            }

            if (s != null)
                MessageBox.Show(Manager.MaxManager.Instance, s,
                  Const.badEntryDlgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation); 

            return false;
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



        /// <summary>Validate app server IP input</summary>
        private bool EditIpInput()
        {
            bool gap = false, err = false;
            int  sum = 0;

            for(int i=0; i < 4; i++) 
            {  
                string ipchunk = ipctrl[i].Text;
                if  (ipchunk == String.Empty) 
                     gap = true;
                else 
                {   int val = Utl.atoi(ipchunk);
                    if (val > 255) 
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


        /// <summary>Populate dialog IP chunks from IP string</summary>
        private void IpStringToDialog()
        {
            string[] s = this.appServerIP.Split(new char[] { Const.cdot } );
            if (s.Length > 3)   
                for(int i=0; i < 4; i++) ipctrl[i].Text = s[i];               
        }  


        /// <summary>Select all IP address text</summary>
        private void IpSelectAll()
        {  
           ipctrl[0].SelectAll();       
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
            TextBox text = sender as TextBox;
            bool ok = false;
            int   n = 0;
            try { n = System.Convert.ToInt32(text.Text); ok = n >= 0; } 
            catch { }
            if  (ok || text.Text == String.Empty) 
                 this.port = text.Text;
            else text.Text = this.port;
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

        private string appServerIP;
        private string appServerPort;
        private string usernamePersisted;
        private string userpassPersisted;
        private string port = String.Empty;
        private string[] ipseg
            = new string[]  { String.Empty, String.Empty, String.Empty, String.Empty };
        private TextBox[] ipctrl = new TextBox[4];
        private bool isPasswordAsterisks;
        private uint errmask = 0;


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MaxAuthenticationDlg));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbAdmin = new System.Windows.Forms.GroupBox();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.labelPass = new System.Windows.Forms.Label();
            this.labelUser = new System.Windows.Forms.Label();
            this.tbPass = new System.Windows.Forms.TextBox();
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
            this.textcontent = new System.Windows.Forms.Label();
            this.gbAdmin.SuspendLayout();
            this.gpIpPort.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(142, 198);
            this.btnOK.Name = "btnOK";
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.OnClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(232, 198);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.OnClick);
            // 
            // gbAdmin
            // 
            this.gbAdmin.Controls.Add(this.tbUserName);
            this.gbAdmin.Controls.Add(this.labelPass);
            this.gbAdmin.Controls.Add(this.labelUser);
            this.gbAdmin.Controls.Add(this.tbPass);
            this.gbAdmin.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbAdmin.Location = new System.Drawing.Point(8, 46);
            this.gbAdmin.Name = "gbAdmin";
            this.gbAdmin.Size = new System.Drawing.Size(298, 61);
            this.gbAdmin.TabIndex = 7;
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
            this.tbUserName.Text = "";
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
            this.tbPass.Text = "";
            this.tbPass.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnPasswordMouseDown);
            this.tbPass.TextChanged += new System.EventHandler(this.OnPasswordTextChanged);
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
            this.gpIpPort.Location = new System.Drawing.Point(8, 120);
            this.gpIpPort.Name = "gpIpPort";
            this.gpIpPort.Size = new System.Drawing.Size(224, 61);
            this.gpIpPort.TabIndex = 8;
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
            this.asIP4.Text = "";
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
            this.asIP3.Text = "";
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
            this.asIP2.Text = "";
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
            this.asPort.Text = "";
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
            this.asIP1.Text = "";
            this.asIP1.WordWrap = false;
            this.asIP1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnIP1MouseDown);
            this.asIP1.TextChanged += new System.EventHandler(this.OnIpSegmentTextChanged);
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label8.Location = new System.Drawing.Point(114, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(8, 24);
            this.label8.TabIndex = 0;
            this.label8.Text = ".";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
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
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label3.Location = new System.Drawing.Point(34, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(8, 22);
            this.label3.TabIndex = 0;
            this.label3.Text = ".";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label5.Location = new System.Drawing.Point(74, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(8, 24);
            this.label5.TabIndex = 0;
            this.label5.Text = ".";
            // 
            // textcontent
            // 
            this.textcontent.Location = new System.Drawing.Point(10, 10);
            this.textcontent.Name = "textcontent";
            this.textcontent.Size = new System.Drawing.Size(296, 28);
            this.textcontent.TabIndex = 9;
            // 
            // MaxAuthenticationDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(314, 228);
            this.Controls.Add(this.textcontent);
            this.Controls.Add(this.gpIpPort);
            this.Controls.Add(this.gbAdmin);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(322, 262);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(322, 262);
            this.Name = "MaxAuthenticationDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Server Authentication";
            this.Load += new System.EventHandler(this.MaxAuthenticationDlg_Load);
            this.gbAdmin.ResumeLayout(false);
            this.gpIpPort.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();      
            base.Dispose(disposing);
        }
        #endregion
   

    }   // class MaxAuthenticationDlg

}  // namespace
