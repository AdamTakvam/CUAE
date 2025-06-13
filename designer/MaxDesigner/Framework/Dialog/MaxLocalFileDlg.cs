using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;



namespace Metreos.Max.Framework
{
    /// <summary>Dialog to prompt for name of new file in specified directory</summary>
    public class MaxLocalFileDlg: Form
    {
        #region dialog controls
        private Label  textBoxLabel;
        private System.Windows.Forms.TextBox  txtFilename;
        private System.Windows.Forms.Button   btnOK;
        private System.Windows.Forms.Button   btnCancel;  
        private System.ComponentModel.Container components = null;
        #endregion

        private string filename;
        private string defaultname;
        private string fileDescription;
        private string fileExt;
        private string folder;
        private string fullpath;
        private bool   mayExist;
  
        public  string FileName { get { return filename; } }
        public  string FilePath { get { return fullpath; } }
 
        /// <summary>Constructor</summary>
        /// <param name="dir">Directory path, no trailing slash</param>
        /// <param name="defaultName">Name to show in the prompt</param>
        /// <param name="ext">File extension of filetype, including dot</param>
        /// <param name="filedesc">Description of filetype + space + single quote</param>
        /// <param name="prompt">Label to appear above file name text box</param>
        /// <param name="caption">Dialog title</param>
        /// <param name="mayExist">Indication whether file may exist or not</param>
        public MaxLocalFileDlg(string dir, string defaultName, string ext, 
        string filedesc, string prompt, string caption, bool mayExist)
        {  
            InitializeComponent(); 

            this.folder      = dir;
            this.defaultname = defaultName;
            this.fileExt     = ext;
            this.mayExist    = mayExist;
            this.fileDescription = filedesc;
            if (caption != null) this.Text = caption;
            if (prompt  != null) this.textBoxLabel.Text = prompt;
            if (this.defaultname != null) txtFilename.Text = this.defaultname;
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
            string fname = txtFilename.Text;      
      
            if (Utl.ValidateMaxName(fname))
            {    
                this.DialogResult = DialogResult.OK;
                this.filename = fname;
                btnOK.Focus();
            }
            else btnOK.Enabled = false;

            return btnOK.Enabled;
        }


        /// <summary>Validate path and existence</summary>
        private bool ValidateFile(object sender, System.EventArgs e)
        {
            this.fullpath = this.folder + Const.bslash + this.filename + this.fileExt;
            if  (!Utl.ValidateMaxPath(fullpath)) return false;

            // If file exists, if caller indicated existence is OK, show the
            // " ... Do you want to replace it?" dialog, otherwise the no-option
            // " xxx already exists in the project directory" dialog.
            if (File.Exists(fullpath))       
                return this.mayExist?
                    Utl.ShowAlreadyExistsDialog(fullpath, this.Text) == DialogResult.Yes:
                    Utl.ShowNameExistsDlg(this.filename,  this.fileDescription, this.Text);
            else return true;
        }


        /// <summary>Reenable OK button on text input</summary>
        private void OnTextInput(object sender, System.EventArgs e)
        {
            btnOK.Enabled = true;
        }


        /// <summary>Set keyboard focus to file name entry</summary>
        private void MaxLocalFileDlg_Load(object sender, System.EventArgs e)
        {      
            this.txtFilename.Focus();
            this.txtFilename.TabIndex = 0;
            this.txtFilename.KeyDown += new KeyEventHandler(txtFilename_KeyDown);
        } 


        /// <summary>Interpret Enter key as OK/summary>
        private void txtFilename_KeyDown(object sender, KeyEventArgs e)
        { 
            if (e.KeyData == Keys.Enter) 
                this.btnOK.PerformClick();
        }


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MaxLocalFileDlg));
            this.textBoxLabel = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxLabel
            // 
            this.textBoxLabel.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.textBoxLabel.Location = new System.Drawing.Point(8, 16);
            this.textBoxLabel.Name = "textBoxLabel";
            this.textBoxLabel.Size = new System.Drawing.Size(352, 16);
            this.textBoxLabel.TabIndex = 0;
            this.textBoxLabel.Text = "File Name";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(200, 71);
            this.btnOK.Name = "btnOK";
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.OnClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(290, 71);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.OnClick);
            // 
            // txtFilename
            // 
            this.txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilename.Location = new System.Drawing.Point(8, 32);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.Size = new System.Drawing.Size(356, 20);
            this.txtFilename.TabIndex = 1;
            this.txtFilename.Text = "";
            this.txtFilename.TextChanged += new System.EventHandler(this.OnTextInput);
            // 
            // MaxLocalFileDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(372, 106);
            this.Controls.Add(this.txtFilename);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.textBoxLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(640, 140);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(380, 140);
            this.Name = "MaxLocalFileDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New File";
            this.Load += new System.EventHandler(this.MaxLocalFileDlg_Load);
            this.ResumeLayout(false);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();      
            base.Dispose(disposing);
        }
        #endregion

    }   // class MaxLocalFileDlg 

}     // namespace
