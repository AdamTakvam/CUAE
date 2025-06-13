using System;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Microsoft.Win32;



namespace Metreos.Max.Framework
{
    /// <summary>Dialog from which to specify app script and trigger names</summary>
    public class MaxNewAppDlg: Form
    {
        #region dialog controls
        private System.Windows.Forms.ListBox listbox;
        private System.Windows.Forms.Button  btnOK;
        private System.Windows.Forms.Button  btnCancel;
        private System.Windows.Forms.Label   label2;
        private System.Windows.Forms.Label   label3;
        private System.Windows.Forms.TextBox triggerName;
        private System.Windows.Forms.TextBox appName;
        private System.ComponentModel.Container components = null;
        #endregion

        public  string AppName { get { return appName.Text;    } }
        public  string Trigger { get { return triggerName.Text;} }  
        private string projectfolder; 
   
    
        public MaxNewAppDlg(string defaultname, string projectdir, string[] triggers)
        {     
            InitializeComponent(); 
            this.Text          = Const.NewAppDlgTitle;
            this.appName.Text  = defaultname;
            this.projectfolder = projectdir;

            this.listbox.SelectionMode = SelectionMode.One;
            listbox.GotFocus +=new EventHandler(listbox_GotFocus);
            listbox.DoubleClick += new EventHandler(listbox_DoubleClick);
  
            foreach(string s in triggers) listbox.Items.Add(s);    
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
                    btnOK.Focus();                   
                    break;

               case DialogResult.OK:                
                    if (!this.ValidateScriptName(appName.Text) 
                     || !this.isTriggerSelected || this.IsScriptNameTriggerConflict(true))
                         this.btnOK.Enabled = false;
                    else this.DialogResult  = DialogResult.OK;                               
                    break;

               case DialogResult.Cancel:            
                    this.DialogResult = result;
                    break;
            }    
        } 


        /// <summary>Determine if script has same name as default triggering function/summary>
        private bool IsScriptNameTriggerConflict(bool notify)
        {
            string defaultTriggerFunctionName = Utl.MakeHandlerName(triggerName.Text);
            bool IsConflict = String.Compare(defaultTriggerFunctionName, appName.Text, true) == 0;
            if  (IsConflict && notify) Utl.ShowInvalidNameDlg(appName.Text);
            return IsConflict;
        }     


        /// <summary>Interpret double click as OK/summary>
        private void listbox_DoubleClick(object sender, EventArgs e)
        {
            if (listbox.SelectedIndex >= 0) this.btnOK.PerformClick();  
        }


        /// <summary>Track selection</summary>
        private void listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.triggerName.Text  = listbox.SelectedItem as string;
            this.isTriggerSelected = this.btnOK.Enabled = true; 
        }


        /// <summary>Handle selection via tab</summary>
        private void listbox_GotFocus(object sender, EventArgs e)
        {
            if (listbox.SelectedIndex < 0) listbox.SelectedIndex = 0;
        }


        /// <summary>Interpret Enter key as OK/summary>
        private void txtProjName_KeyDown(object sender, KeyEventArgs e)
        { 
            this.btnOK.Enabled = true; 
            if (e.KeyData == Keys.Enter) this.btnOK.PerformClick();
        }


        /// <summary>Edit app name string input</summary>
        private bool ValidateScriptName(string name)
        {
            if (!Utl.ValidateMaxName(name)) return Utl.ShowInvalidNameDlg(name);

            string path = projectfolder + Const.bslash + name + Const.maxScriptFileExtension;

            if (File.Exists(path)) return Utl.ShowNameExistsDlg(name, null, this.Text);

            return true;
        }  


        private void MaxNewAppDlg_Load(object sender, System.EventArgs e)
        { 
            this.appName.Focus();
            this.appName.TabIndex = 0;
            this.appName.KeyDown += new KeyEventHandler(txtProjName_KeyDown);   
            this.listbox.SelectedIndexChanged += new EventHandler(listbox_SelectedIndexChanged);
        }


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MaxNewAppDlg));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.listbox = new System.Windows.Forms.ListBox();
            this.triggerName = new System.Windows.Forms.TextBox();
            this.appName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(220, 218);
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
            this.btnCancel.Location = new System.Drawing.Point(309, 218);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.OnClick);
            // 
            // listbox
            // 
            this.listbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.listbox.Location = new System.Drawing.Point(8, 108);
            this.listbox.Name = "listbox";
            this.listbox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listbox.Size = new System.Drawing.Size(376, 95);
            this.listbox.TabIndex = 0;
            // 
            // triggerName
            // 
            this.triggerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.triggerName.Location = new System.Drawing.Point(9, 82);
            this.triggerName.Name = "triggerName";
            this.triggerName.ReadOnly = true;
            this.triggerName.Size = new System.Drawing.Size(375, 20);
            this.triggerName.TabIndex = 8;
            this.triggerName.Text = "";
            // 
            // appName
            // 
            this.appName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.appName.Location = new System.Drawing.Point(8, 30);
            this.appName.Name = "appName";
            this.appName.Size = new System.Drawing.Size(376, 20);
            this.appName.TabIndex = 9;
            this.appName.Text = "";
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(8, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 19);
            this.label2.TabIndex = 10;
            this.label2.Text = "Application Script Name";
            // 
            // label3
            // 
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label3.Location = new System.Drawing.Point(8, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 19);
            this.label3.TabIndex = 11;
            this.label3.Text = "Select Triggering Event";
            // 
            // MaxNewAppDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(392, 252);
            this.Controls.Add(this.appName);
            this.Controls.Add(this.triggerName);
            this.Controls.Add(this.listbox);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 286);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 286);
            this.Name = "MaxNewAppDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Application Script";
            this.Load += new System.EventHandler(this.MaxNewAppDlg_Load);
            this.ResumeLayout(false);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();      
            base.Dispose(disposing);
        }
        #endregion

        private bool isTriggerSelected;

    } // class MaxNewAppDlg  

}  // namespace
