using System;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Microsoft.Win32;



namespace Metreos.Max.Framework
{
    /// <summary>Dialog to solicit a directory path</summary>
    public class MaxLocationDlg: Form
    {
        #region dialog controls

        private System.Windows.Forms.ComboBox comboFolder;
        private System.Windows.Forms.Button   btnBrowse;
        private System.Windows.Forms.Button   btnOK;
        private System.Windows.Forms.Button   btnCancel;  
        private System.Windows.Forms.Label    label;
        private System.ComponentModel.Container components = null;
        #endregion

        private string returnFolder;
        public string  ReturnFolder { get { return returnFolder; } }
        private string regItemKey;
        private string caption;
        private string defaultFolder;

        /// <summary>
        /// Constructor accepts prompt caption, and registry subkey under "Location"
        /// from which to read default folder path</summary>
        public MaxLocationDlg(string caption, string itemKey, string defaultFolder)
        {     
            InitializeComponent(); 
  
            this.caption    = this.label.Text = caption; 
            this.regItemKey = itemKey;
            this.defaultFolder = defaultFolder == null? 
                Const.DefaultLocalLibrariesFolder: defaultFolder;
     
            string folder = this.GetCurrentLocation();

            if  (folder == null) 
                 folder  = this.defaultFolder;
            else
            if  (folder != this.defaultFolder)
                 comboFolder.Items.Add(this.defaultFolder);

            comboFolder.SelectedIndex = comboFolder.Items.Add(folder);
        }


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

            if (Utl.ValidateMaxPath(folder))
            {    
                this.returnFolder = folder;
                this.Persist(folder);
                btnOK.Enabled = true;
            }
            else btnOK.Enabled = false; 

            return btnOK.Enabled; 
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


        /// <summary>Reenable OK button on user input</summary>
        private void OnTextInput(object sender, System.EventArgs e)
        {
            btnOK.Enabled = true;
        }


        /// <summary>Load folder from registry given key</summary>
        public string GetCurrentLocation()
        { 
            return Reg.GetStringValue(Const.RegistryLocationKey, this.regItemKey);
        }


        /// <summary>Save the new folder path to registry</summary>
        public bool Persist(string path)
        {
            return Reg.SetStringValue(Const.RegistryLocationKey, this.regItemKey, path);
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
        } // class MaxFolderBrowser


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MaxLocationDlg));
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.comboFolder = new System.Windows.Forms.ComboBox();
            this.label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBrowse.Location = new System.Drawing.Point(448, 40);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "Browse ...";
            this.btnBrowse.Click += new System.EventHandler(this.OnClick);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(361, 87);
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
            this.btnCancel.Location = new System.Drawing.Point(448, 87);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.OnClick);
            // 
            // comboFolder
            // 
            this.comboFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.comboFolder.Location = new System.Drawing.Point(8, 40);
            this.comboFolder.Name = "comboFolder";
            this.comboFolder.Size = new System.Drawing.Size(429, 21);
            this.comboFolder.TabIndex = 8;
            this.comboFolder.TextChanged += new System.EventHandler(this.OnTextInput);
            // 
            // label
            // 
            this.label.Location = new System.Drawing.Point(8, 18);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(408, 16);
            this.label.TabIndex = 3;
            this.label.Text = "Specify Location";
            // 
            // MaxLocationDlg
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(536, 122);
            this.Controls.Add(this.comboFolder);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(700, 156);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 156);
            this.Name = "MaxLocationDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "System Folder Location";
            this.ResumeLayout(false);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();      
            base.Dispose(disposing);
        }
        #endregion

    }   // class MaxNewFileDlg 

}   // namespace
