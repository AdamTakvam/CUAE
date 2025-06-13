//
// MaxNewFileDlg.cs
// New project dialog
//
using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;



namespace Metreos.Max.Framework
{
    /// <summary>New file dialog (since .NET does not provide one)</summary>
    /// <remarks>This has evolved into the new *project* file dialog</remarks>
    public class MaxNewFileDlg: Form
    {
        #region dialog controls
        private System.Windows.Forms.TextBox  txtProjName;
        private System.Windows.Forms.ComboBox comboFolder;
        private System.Windows.Forms.Label    label1;
        private System.Windows.Forms.Label    label2;
        private System.Windows.Forms.Label    txtMessageArea;
        private System.Windows.Forms.Button   btnBrowse;
        private System.Windows.Forms.Button   btnOK;
        private System.Windows.Forms.Button   btnCancel;  
        private System.ComponentModel.Container components = null;
        #endregion

        private string projectName;
        private string projectFolder;
  
        public  string ProjectName   { get { return projectName;   } }
        public  string ProjectFolder { get { return projectFolder; } }
        public  enum   Options { FixupRecentFilePath };
        private bool   isFixupRecentFile;


        public MaxNewFileDlg()
        {  
            Init();
        }

        public MaxNewFileDlg(Options option)
        {  
            this.isFixupRecentFile = option == Options.FixupRecentFilePath;
            Init();
        }

        private void Init()
        {
            InitializeComponent(); 

            string folder = GetRecentFolder();
            comboFolder.SelectedIndex = comboFolder.Items.Add(folder);

            int sequence = this.isFixupRecentFile?

                Utl.GetUniqueDirectorySequencer(folder, Const.defaultProjectFilename, 0):

                Utl.GetUniqueFilenameSequencer
                (folder, Const.defaultProjectFilename, Const.maxProjectFileExtension, 0);

            this.txtProjName.Text = Const.defaultProjectFilename + sequence;  
           
            this.txtMessageArea.Font = new Font("Microsoft Sans Serif", 8.25F, 
                FontStyle.Bold, GraphicsUnit.Point, ((Byte)(0)));

            this.txtMessageArea.ForeColor = Const.DialogMessageBlue;
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
                    this.OnBrowseButton(); 
                    break;

               case DialogResult.OK:                
                    this.OnOK();                       
                    break;

               case DialogResult.Cancel:            
                    this.DialogResult = result;
                    break;
            }     
        }      



        /// <summary>Handle OK button click</summary>
        private bool OnOK()
        {
            string fname  = txtProjName.Text;      
            string folder = comboFolder.Text;

            bool isPathOK = this.ValidateProjectPath(folder);
            bool isNameOK = this.ValidateProjectName(fname);
      
            if (isPathOK && isNameOK)
            {    
                this.DialogResult  = DialogResult.OK;
                this.projectName   = fname;
                this.projectFolder = folder;
                btnOK.Focus();
            }
            else 
            {   btnOK.Enabled = false;
                this.ShowMessage(true);
            }

            return btnOK.Enabled;
        }


        /// <summary>Validate name as entered</summary>
        private bool ValidateProjectName(string name)
        {
            bool result = Utl.ValidateMaxName(name);
            if (!result) this.SetMessage(Const.badProjectNameMsg);
            return result;
        }


        /// <summary>Validate path as entered</summary>
        private bool ValidateProjectPath(string folder)
        {
            bool result = Utl.ValidateMaxPath(folder);
            if (!result) this.SetMessage(Const.badProjectPathMsg);
            return result;
        }


        /// <summary>Handle Browse button click</summary>
        private bool OnBrowseButton()      
        {
            string folder = new MaxFolderBrowser().ShowDialog();
            if  (!Utl.ValidateMaxPath(folder)) return false;

            int  ndx = comboFolder.FindStringExact(folder);
            if  (ndx == -1)
                 ndx = comboFolder.Items.Add(folder);

            comboFolder.SelectedIndex = ndx;  
            return true;
        } 


        /// <summary>Reenable OK button on text input</summary>
        private void OnTextInput(object sender, System.EventArgs e)
        {
            btnOK.Enabled = true;
            this.ShowMessage(false);
        }


        /// <summary>Get start directory from recent file list</summary>
        /// <remarks>With Visual Studio styled project logic, as used in Max,
        /// a project is assigned its own directory, which is created by the IDE
        /// using the same name as the project file. This directory is however
        /// not specified on the New File dialog path string. Then, the recent
        /// files list manager adds this path to its recent files list. Now,
        /// ordinarily a New File prompt will strip the filespec from the most
        /// recent file path, and use this as the start directory. However, 
        /// since an extra directory now exists on the path, if we were to
        /// do this, we would end up placing the next new project inside the
        /// directory of the previous project. To remedy the situation, we 
        /// strip the deepest directory from the file path, when asked to do so.
        ///</remarks>
        public string GetRecentFolder()
        {
            MaxRecentFileList recent = Config.RecentFiles;
            if  (recent.First == null)
                 recent.Load();
 
            string mostRecentFilePath  = recent.First;

            string mostRecentDirectory 
                = mostRecentFilePath == null?  Const.blank:
                  this.isFixupRecentFile?  
                        Utl.StripLastDirectory(mostRecentFilePath, false):
                        Utl.StripPathFilespec(mostRecentFilePath);

            return mostRecentDirectory;
        }


        /// <summary>Set keyboard focus to file name entry</summary>
        private void MaxNewFileDlg_Load(object sender, System.EventArgs e)
        {      
            this.txtProjName.Focus();
            this.txtProjName.TabIndex = 0;
            this.txtProjName.KeyDown += new KeyEventHandler(txtProjName_KeyDown);
        } 


        /// <summary>Interpret Enter key as OK</summary>
        private void txtProjName_KeyDown(object sender, KeyEventArgs e)
        { 
            if (e.KeyData == Keys.Enter) 
                this.btnOK.PerformClick();
        }


        /// <summary>Show or hide the message area</summary>
        private void ShowMessage(bool b)
        {
            this.txtMessageArea.Visible = b;
        }


        /// <summary>Show supplied message in the message area</summary>
        private void ShowMessage(string s)
        {
            this.txtMessageArea.Text = s;
            this.txtMessageArea.Visible = true;
        }


        /// <summary>Set message in the message area</summary>
        private void SetMessage(string s)
        {
            this.txtMessageArea.Text = s;
        }


        /// <summary>Subdialog to browse for folder</summary>
        public class MaxFolderBrowser: System.Windows.Forms.Design.FolderNameEditor
        {
            private string path;
            public  string Path  { get { return path; } set { path = value; } }

            public string ShowDialog()
            {
                FolderBrowser fb = new FolderBrowser();
                fb.Description   = Const.ChooseFolder;
                // public string Description { get; set; }
                // public string DirectoryPath { get; }
                // public FolderBrowserFolder StartLocation { get; set; }

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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MaxNewFileDlg));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtProjName = new System.Windows.Forms.TextBox();
            this.comboFolder = new System.Windows.Forms.ComboBox();
            this.txtMessageArea = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(8, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Project Name";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Project Location";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBrowse.Location = new System.Drawing.Point(344, 48);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "Browse ...";
            this.btnBrowse.Click += new System.EventHandler(this.OnClick);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(256, 96);
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
            this.btnCancel.Location = new System.Drawing.Point(344, 96);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.OnClick);
            // 
            // txtProjName
            // 
            this.txtProjName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProjName.Location = new System.Drawing.Point(96, 16);
            this.txtProjName.Name = "txtProjName";
            this.txtProjName.Size = new System.Drawing.Size(322, 20);
            this.txtProjName.TabIndex = 7;
            this.txtProjName.Text = "";
            this.txtProjName.TextChanged += new System.EventHandler(this.OnTextInput);
            // 
            // comboFolder
            // 
            this.comboFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.comboFolder.Location = new System.Drawing.Point(96, 48);
            this.comboFolder.Name = "comboFolder";
            this.comboFolder.Size = new System.Drawing.Size(240, 21);
            this.comboFolder.TabIndex = 8;
            this.comboFolder.TextChanged += new System.EventHandler(this.OnTextInput);
            // 
            // txtMessageArea
            // 
            this.txtMessageArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessageArea.Location = new System.Drawing.Point(8, 76);
            this.txtMessageArea.Name = "txtMessageArea";
            this.txtMessageArea.Size = new System.Drawing.Size(408, 16);
            this.txtMessageArea.TabIndex = 0;
            // 
            // MaxNewFileDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(429, 131);
            this.Controls.Add(this.txtMessageArea);
            this.Controls.Add(this.comboFolder);
            this.Controls.Add(this.txtProjName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(640, 165);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(437, 165);
            this.Name = "MaxNewFileDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Application Designer Project";
            this.Load += new System.EventHandler(this.MaxNewFileDlg_Load);
            this.ResumeLayout(false);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();      
            base.Dispose(disposing);
        }
        #endregion

    }   // class MaxNewFileDlg 

}     // namespace
