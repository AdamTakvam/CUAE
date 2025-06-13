using System;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Microsoft.Win32;



namespace Metreos.Max.Framework
{
    /// <summary>Dialog from which to choose one or more strings</summary>
    public class MaxChooserDlg: Form
    {
        #region dialog controls
        private System.Windows.Forms.ListBox listbox;
        private System.Windows.Forms.Button  btnOK;
        private System.Windows.Forms.Button  btnCancel;
        private System.ComponentModel.Container components = null;
        #endregion
        public  string  Selection  { get { return listbox.SelectedItem as string;  } }
        public  ListBox.SelectedObjectCollection Selections { get { return listbox.SelectedItems; } }
   
    
        public MaxChooserDlg(string caption, string[] content, bool multiselect)
        {     
            InitializeComponent(); 
            this.listbox.SelectionMode = multiselect? SelectionMode.MultiSimple: SelectionMode.One;
            this.Text = caption;         
            listbox.Click       += new EventHandler(listbox_Click);
            listbox.DoubleClick += new EventHandler(listbox_DoubleClick);
  
            this.LoadContent(content);    
        }


        protected void LoadContent(string[] content)
        {
            foreach(string s in content) listbox.Items.Add(s);
        }


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MaxChooserDlg));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.listbox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(220, 122);
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
            this.btnCancel.Location = new System.Drawing.Point(310, 122);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.OnClick);
            // 
            // listbox
            // 
            this.listbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.listbox.Location = new System.Drawing.Point(8, 12);
            this.listbox.Name = "listbox";
            this.listbox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listbox.Size = new System.Drawing.Size(376, 95);
            this.listbox.TabIndex = 0;
            // 
            // MaxChooserDlg
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(392, 158);
            this.Controls.Add(this.listbox);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 192);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 192);
            this.Name = "MaxChooserDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chooser";
            this.ResumeLayout(false);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();      
            base.Dispose(disposing);
        }
        #endregion


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
                    if  (listbox.SelectedIndex == -1)
                        this.btnOK.Enabled = false;
                    else this.DialogResult = DialogResult.OK;                               
                    break;

               case DialogResult.Cancel:            
                    this.DialogResult = result;
                    break;
            }    
        }      


        private void listbox_Click(object sender, EventArgs e)
        {
            this.btnOK.Enabled = true;
        }

        private void listbox_DoubleClick(object sender, EventArgs e)
        {
            if  (listbox.SelectedIndex >= 0) this.DialogResult = DialogResult.OK;  
        }
    }   

}  // namespace
