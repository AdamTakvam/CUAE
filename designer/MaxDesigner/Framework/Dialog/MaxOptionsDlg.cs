using System;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Microsoft.Win32;



namespace Metreos.Max.Framework
{
    /// <summary>Tool/Options dialog</summary>
    public class MaxOptionsDlg: Form
    {
        #region dialog controls
        private System.Windows.Forms.ComboBox comboFolder;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label0;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox asIP1;
        private System.Windows.Forms.TextBox asIP2;
        private System.Windows.Forms.TextBox asIP3;
        private System.Windows.Forms.TextBox asIP4;
        private System.Windows.Forms.TextBox asPort;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.ComponentModel.Container components = null;
        #endregion

        private string frameworkFolder;
        public string  FrameworkFolder { get { return frameworkFolder;} }
        private string appServerIP;
        public string  AppServerIP     { get { return appServerIP;    } }
        private string appServerPort;
        public string  AppServerPort   { get { return appServerPort;  } }

        private string registryFrameworkFolder;
        private string registryAppServerIP;
        private string registryAppServerPort;

        private string caption;
        private string port = Const.emptystr;
        private string[]  ipseg
            = new string[]  {Const.emptystr, Const.emptystr, Const.emptystr, Const.emptystr};
        private TextBox[] ipctrl = new TextBox[4];


        public MaxOptionsDlg(string defaultFolder)
        {     
            InitializeComponent(); 
            asIP1.Tag = 0; asIP2.Tag = 1;  asIP3.Tag = 2; asIP4.Tag = 3;
            ipctrl[0] = asIP1; ipctrl[1] = asIP2; ipctrl[2] = asIP3; ipctrl[3] = asIP4;
  
            this.caption = Const.PromptFrameworkFolderLocation;
                                            // Get current port
            registryAppServerPort = Config.AppServerPort;
            appServerPort = registryAppServerPort == null || registryAppServerPort.Length == 0? 
                Config.DefaultAppServerPort: registryAppServerPort;

                                            // Get current IP
            registryAppServerIP = Config.AppServerIP;
            appServerIP = registryAppServerIP == null || registryAppServerIP.Length == 0? 
                Config.DefaultAppServerIP: registryAppServerIP;

            this.asPort.Text = appServerPort; 
         
            this.IpStringToDialog();
                                            // Get current framework dir
            registryFrameworkFolder = Config.FrameworkDirectory;
                                            
            this.frameworkFolder = defaultFolder == null? 
                Const.DefaultLocalLibrariesFolder: defaultFolder;
                                             
            string folder = registryFrameworkFolder;  

            if  (folder == null) 
                 folder  = this.frameworkFolder;
            else
            if  (folder != this.frameworkFolder)
                 comboFolder.Items.Add(this.frameworkFolder);

            comboFolder.SelectedIndex = comboFolder.Items.Add(folder);
        }


        /// <summary>Actions on button click</summary>
        private void OnClick(object sender, System.EventArgs e)
        {
            Button button = (Button) sender;

            DialogResult result
                = button == btnOK?     DialogResult.OK:
                  button == btnCancel? DialogResult.Cancel:
                  DialogResult.None; 
      
            switch(result)
            {
               case DialogResult.None:                          
                    if  (this.OnBrowseButton())  
                         btnOK.Focus();                   
                    break;

               case DialogResult.OK:                
                    if  (this.OnOK()) 
                         this.DialogResult = DialogResult.OK;                             
                    break;

               case DialogResult.Cancel:            
                    this.DialogResult = result;
                    break;
            }    
        }      


        /// <summary>Handle OK button click</summary>
        private bool OnOK()
        {
            string folder = (string)comboFolder.SelectedItem; 
            btnOK.Enabled = false; 

            if (!Utl.ValidateMaxPath(folder) || !EditIpInput() || !EditPortInput())  
                return false;  

            btnOK.Enabled = true;  
            this.frameworkFolder = folder;  
                                            // Set registry values
            if  (this.frameworkFolder != this.registryFrameworkFolder) 
                 Config.FrameworkDirectory = this.frameworkFolder;                    

            this.SetNewIP();

            if  (this.port != Const.emptystr)
                 Config.AppServerPort = this.port;

            return true; 
        }


        /// <summary>Handle Browse button click</summary>
        private bool OnBrowseButton()
        {
            string folder = new MaxFolderBrowser().ShowDialog(this.caption);
            if  (!Utl.ValidateMaxPath(folder)) return false;

            int  ndx = comboFolder.FindStringExact(folder);
            if  (ndx == -1)
                 ndx = comboFolder.Items.Add(folder);

            comboFolder.SelectedIndex = ndx;  
            return true;
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
                {
                    box.Text = s.Substring(0, dotpos);
                    s = s.Substring(dotpos+1);
                }
            }
        }


        /// <summary>Set app server IP from dialog entries</summary>
        private void SetNewIP()
        {
            bool isEntry = true;
            for(int i=0; i < 4; i++) if (ipctrl[i].Text == Const.emptystr) isEntry = false;
            if  (!isEntry) return;                                            

            string newIP = ipctrl[0].Text;
            for(int i=1; i < 4; i++) newIP += Const.dot + ipctrl[i].Text;

            Config.AppServerIP = newIP;           // Set registry
        }


        /// <summary>Validate samoa IP input</summary>
        private bool EditIpInput()
        {
            bool gap = false, err = false;
            int  sum = 0;

            for(int i=0; i < 4; i++) 
            {  
                string ipchunk = ipctrl[i].Text;
                if (ipchunk == Const.emptystr) 
                    gap = true;
                else 
                {   int  val = Utl.atoi(ipchunk);
                    if  (val > 255) 
                         err = true;
                    else
                    {   sum += val;               
                        ipctrl[i].Text = val.ToString(); // Nix LZ
                    }
                }
            }

            return !gap && !err && sum > 0;
        }


        /// <summary>Validate samoa port input</summary>
        private bool EditPortInput()
        {
            if (port == Const.emptystr) return true; // No entry
            int val = Utl.atoi(port); 
            port = val.ToString(); // Nix leading zeros      
            return val != 0;
        }


        /// <summary>Reenable OK button on user input</summary>
        private void OnTextInput(object sender, System.EventArgs e)
        {
            btnOK.Enabled = true;
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
            TextBox text = sender as TextBox;
            bool ok = false;
            int   n = 0;
            try { n = System.Convert.ToInt32(text.Text); ok = n >= 0; } 
            catch { }
            if  (ok || text.Text == Const.emptystr) 
                this.port = text.Text;
            else text.Text = this.port;
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

        #region Windows Form Designer generated code
    
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MaxOptionsDlg));
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.comboFolder = new System.Windows.Forms.ComboBox();
            this.label0 = new System.Windows.Forms.Label();
            this.asIP1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.asIP2 = new System.Windows.Forms.TextBox();
            this.asIP3 = new System.Windows.Forms.TextBox();
            this.asPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.asIP4 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBrowse.Location = new System.Drawing.Point(368, 30);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse ...";
            this.btnBrowse.Click += new System.EventHandler(this.OnClick);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(280, 120);
            this.btnOK.Name = "btnOK";
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.OnClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(368, 120);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.OnClick);
            // 
            // comboFolder
            // 
            this.comboFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.comboFolder.Location = new System.Drawing.Point(8, 31);
            this.comboFolder.Name = "comboFolder";
            this.comboFolder.Size = new System.Drawing.Size(352, 21);
            this.comboFolder.TabIndex = 2;
            this.comboFolder.TextChanged += new System.EventHandler(this.OnTextInput);
            // 
            // label0
            // 
            this.label0.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label0.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label0.Location = new System.Drawing.Point(8, 15);
            this.label0.Name = "label0";
            this.label0.Size = new System.Drawing.Size(120, 16);
            this.label0.TabIndex = 0;
            this.label0.Text = "Framework Directory";
            // 
            // asIP1
            // 
            this.asIP1.Location = new System.Drawing.Point(8, 33);
            this.asIP1.MaxLength = 3;
            this.asIP1.Name = "asIP1";
            this.asIP1.Size = new System.Drawing.Size(25, 20);
            this.asIP1.TabIndex = 3;
            this.asIP1.Text = "";
            this.asIP1.WordWrap = false;
            this.asIP1.TextChanged += new System.EventHandler(this.OnIpSegmentTextChanged);
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(8, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "IP";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.asIP1);
            this.groupBox1.Controls.Add(this.asIP2);
            this.groupBox1.Controls.Add(this.asIP3);
            this.groupBox1.Controls.Add(this.asPort);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.asIP4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(8, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(226, 69);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Application Server Network Address";
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label8.Location = new System.Drawing.Point(114, 31);
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
            // asIP2
            // 
            this.asIP2.Location = new System.Drawing.Point(48, 33);
            this.asIP2.MaxLength = 3;
            this.asIP2.Name = "asIP2";
            this.asIP2.Size = new System.Drawing.Size(25, 20);
            this.asIP2.TabIndex = 4;
            this.asIP2.Text = "";
            this.asIP2.WordWrap = false;
            this.asIP2.TextChanged += new System.EventHandler(this.OnIpSegmentTextChanged);
            // 
            // asIP3
            // 
            this.asIP3.Location = new System.Drawing.Point(88, 33);
            this.asIP3.MaxLength = 3;
            this.asIP3.Name = "asIP3";
            this.asIP3.Size = new System.Drawing.Size(25, 20);
            this.asIP3.TabIndex = 5;
            this.asIP3.Text = "";
            this.asIP3.WordWrap = false;
            this.asIP3.TextChanged += new System.EventHandler(this.OnIpSegmentTextChanged);
            // 
            // asPort
            // 
            this.asPort.Location = new System.Drawing.Point(180, 33);
            this.asPort.MaxLength = 5;
            this.asPort.Name = "asPort";
            this.asPort.Size = new System.Drawing.Size(36, 20);
            this.asPort.TabIndex = 7;
            this.asPort.Text = "";
            this.asPort.WordWrap = false;
            this.asPort.TextChanged += new System.EventHandler(this.OnPortTextChanged);
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(180, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "Port";
            // 
            // asIP4
            // 
            this.asIP4.Location = new System.Drawing.Point(127, 33);
            this.asIP4.MaxLength = 3;
            this.asIP4.Name = "asIP4";
            this.asIP4.Size = new System.Drawing.Size(25, 20);
            this.asIP4.TabIndex = 6;
            this.asIP4.Text = "";
            this.asIP4.WordWrap = false;
            this.asIP4.TextChanged += new System.EventHandler(this.OnIpSegmentTextChanged);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label3.Location = new System.Drawing.Point(34, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(8, 22);
            this.label3.TabIndex = 0;
            this.label3.Text = ".";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label5.Location = new System.Drawing.Point(74, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(8, 24);
            this.label5.TabIndex = 0;
            this.label5.Text = ".";
            // 
            // MaxOptionsDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(452, 156);
            this.Controls.Add(this.comboFolder);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label0);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(640, 190);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(460, 190);
            this.Name = "MaxOptionsDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.MaxOptionsDlg_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();      
            base.Dispose(disposing);
        }
        #endregion

    } // class MaxOptionsDlg 

}   // namespace
