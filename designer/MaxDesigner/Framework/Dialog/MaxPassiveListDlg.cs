using System;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Microsoft.Win32;



namespace Metreos.Max.Framework
{
    /// <summary>Informative dialog presenting a read-only list with explanatory blurb</summary>
    public class MaxPassiveListDlg: Form
    {
        #region dialog controls
        private System.Windows.Forms.ListBox  listbox;
        private System.Windows.Forms.Button   btnOK;
        private System.Windows.Forms.GroupBox groupbox;
        private System.Windows.Forms.Label    blurb;
        private System.ComponentModel.Container components = null;
        #endregion
        private string[] content;
       
        public MaxPassiveListDlg(string caption, string blurb, string[] content, string title)
        {     
            InitializeComponent(); 
            if (caption != null) this.Text = caption; 
            if (title   != null) this.groupbox.Text = title;
            this.content = content;            
            this.blurb.Text = blurb;  
        }

        private void OnClick(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;                
        }     

        private void MaxPassiveListDlg_Load(object sender, System.EventArgs e)
        {
            foreach(string s in content) listbox.Items.Add(s);
        }

        #region Windows Form Designer generated code
    
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MaxPassiveListDlg));
            this.btnOK = new System.Windows.Forms.Button();
            this.listbox = new System.Windows.Forms.ListBox();
            this.groupbox = new System.Windows.Forms.GroupBox();
            this.blurb = new System.Windows.Forms.Label();
            this.groupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(310, 162);
            this.btnOK.Name = "btnOK";
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.OnClick);
            // 
            // listbox
            // 
            this.listbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.listbox.Location = new System.Drawing.Point(8, 12);
            this.listbox.Name = "listbox";
            this.listbox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listbox.Size = new System.Drawing.Size(376, 82);
            this.listbox.TabIndex = 0;
            // 
            // groupbox
            // 
            this.groupbox.Controls.Add(this.blurb);
            this.groupbox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupbox.Location = new System.Drawing.Point(8, 104);
            this.groupbox.Name = "groupbox";
            this.groupbox.Size = new System.Drawing.Size(294, 80);
            this.groupbox.TabIndex = 6;
            this.groupbox.TabStop = false;
            // 
            // blurb
            // 
            this.blurb.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.blurb.Location = new System.Drawing.Point(8, 14);
            this.blurb.Name = "blurb";
            this.blurb.Size = new System.Drawing.Size(284, 64);
            this.blurb.TabIndex = 0;
            // 
            // MaxPassiveListDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(392, 196);
            this.Controls.Add(this.groupbox);
            this.Controls.Add(this.listbox);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 230);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 230);
            this.Name = "MaxPassiveListDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Max Designer";
            this.Load += new System.EventHandler(this.MaxPassiveListDlg_Load);
            this.groupbox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();      
            base.Dispose(disposing);
        }
        #endregion  
    }   
}  // namespace
