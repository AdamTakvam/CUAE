using System;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Metreos.Max.Drawing;
using Metreos.Max.Manager;
using Microsoft.Win32;



namespace Metreos.Max.Framework
{
    /// <summary>Dialog to display, and from which to navigate to, function references</summary>
    public class MaxFunctionRefsDlg: Form
    {
        #region dialog controls
        private System.Windows.Forms.ListBox listbox;
        private System.Windows.Forms.Button  btnOK;
        private System.Windows.Forms.Button  btnGoTo;
        private System.Windows.Forms.Label   labTitle;
        private System.ComponentModel.Container components = null;
        #endregion
  
        protected ListboxEntry Selection { get { return listbox.SelectedItem as ListboxEntry; } }
        protected MaxAppTreeNodeFunc treenode;

        protected class ListboxEntry
        {
            public ListboxEntry(MaxIconicMultiTextNode node) { action = node; }
            public MaxIconicMultiTextNode action;
            public string Text { get { return action.Canvas.CanvasName; } }
            public override string ToString() { return Text; }
        }
   
    
        public MaxFunctionRefsDlg(MaxAppTreeNodeFunc treenode, bool calls)
        {     
            InitializeComponent(); 
            this.treenode = treenode;  
            this.btnGoTo.Enabled = false;    
            listbox.Click       += new EventHandler(listbox_Click);
            listbox.DoubleClick += new EventHandler(listbox_DoubleClick);
  
            if  (calls)
                 this.LoadCalls();
            else this.LoadReferences();   
        }


        /// <summary>Populate listbox with calls to selected function</summary>
        protected void LoadCalls()
        {
            this.Text = Const.ShowCallsDlgTitle;
            this.labTitle.Text = Const.ShowCallsToLabel + treenode.GetFunctionName();

            foreach(object x in treenode.CanvasNodeCallActions)
            {
                MaxCallNode callnode = x as MaxCallNode; 
                if (callnode != null) listbox.Items.Add(new ListboxEntry(callnode));
            }
        }


        /// <summary>Populate listbox with references to selected function</summary>
        protected void LoadReferences()
        {
            this.Text = Const.ShowReferencesDlgTitle;
            this.labTitle.Text = Const.ActionsReferencingLabel + treenode.GetFunctionName();
            MaxAppTreeNodeEVxEH evhnode = treenode as MaxAppTreeNodeEVxEH;
            if (evhnode == null) return;

            foreach(object x in evhnode.References)
            {
                MaxAppTreeEvhRef refx = x as MaxAppTreeEvhRef; if (refx == null) continue;
                listbox.Items.Add(new ListboxEntry(refx.action));
            }
        }


        /// <summary>Handle button click</summary>
        private void OnClick(object sender, System.EventArgs e)
        {
            Button button = (Button) sender;

            DialogResult result = button == btnOK? DialogResult.OK: DialogResult.None; 
      
            switch(result)
            {
               case DialogResult.None:  
                    this.GoToReference();                                     
                    this.btnOK.Focus();                   
                    break;

               case DialogResult.OK:                          
                    this.DialogResult = DialogResult.OK;                               
                    break;
            }    
        } 


        /// <summary>Navigate to canvas node referenced</summary>
        private void GoToReference()
        {
            ListboxEntry selection = this.Selection; 
            if (selection != null && selection.action != null)  
                MaxManager.Instance.NavigateToNode(selection.action.NodeID, true);
        }     


        /// <summary>Handle single click</summary>
        private void listbox_Click(object sender, EventArgs e)
        {
            this.btnGoTo.Enabled = this.Selection != null;
        }


        /// <summary>Handle double click</summary>
        private void listbox_DoubleClick(object sender, EventArgs e)
        {
            this.GoToReference(); 
            this.btnOK.Focus();   
        }


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MaxFunctionRefsDlg));
            this.btnOK = new System.Windows.Forms.Button();
            this.listbox = new System.Windows.Forms.ListBox();
            this.labTitle = new System.Windows.Forms.Label();
            this.btnGoTo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(310, 128);
            this.btnOK.Name = "btnOK";
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.OnClick);
            // 
            // listbox
            // 
            this.listbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.listbox.Location = new System.Drawing.Point(8, 32);
            this.listbox.Name = "listbox";
            this.listbox.Size = new System.Drawing.Size(376, 82);
            this.listbox.TabIndex = 0;
            // 
            // labTitle
            // 
            this.labTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.labTitle.Location = new System.Drawing.Point(8, 12);
            this.labTitle.Name = "labTitle";
            this.labTitle.Size = new System.Drawing.Size(376, 16);
            this.labTitle.TabIndex = 6;
            // 
            // btnGoTo
            // 
            this.btnGoTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGoTo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnGoTo.Location = new System.Drawing.Point(220, 128);
            this.btnGoTo.Name = "btnGoTo";
            this.btnGoTo.TabIndex = 7;
            this.btnGoTo.Text = "Go To";
            this.btnGoTo.Click += new System.EventHandler(this.OnClick);
            // 
            // MaxFunctionRefsDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(392, 158);
            this.Controls.Add(this.btnGoTo);
            this.Controls.Add(this.labTitle);
            this.Controls.Add(this.listbox);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 192);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 192);
            this.Name = "MaxFunctionRefsDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();      
            base.Dispose(disposing);
        }
        #endregion

    }  // class MaxFunctionRefsDlgs 

}  // namespace
