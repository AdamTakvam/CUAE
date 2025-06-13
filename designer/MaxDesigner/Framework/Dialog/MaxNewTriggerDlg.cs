using System;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Metreos.Max.Manager;
using Metreos.Max.Drawing;
using Microsoft.Win32;



namespace Metreos.Max.Framework
{
    /// <summary>Dialog from which to specify replacement trigger names/summary>
    public class MaxNewTriggerDlg: Form
    {
        #region dialog controls
        private System.Windows.Forms.Button  btnOK;
        private System.Windows.Forms.Button  btnCancel;
        private System.Windows.Forms.ListBox listbox;
        private System.Windows.Forms.TextBox tbHandlerName;
        private System.Windows.Forms.Label   labHandlerName;
        private System.Windows.Forms.Label   labTriggerName;
        private System.Windows.Forms.TextBox tbTriggerName;
        private System.ComponentModel.Container components = null;
        #endregion

        public  string HandlerName { get { return tbHandlerName.Text; } }
        public  string TriggerName { get { return tbTriggerName.Text; } }  

    
        public MaxNewTriggerDlg(string[] triggers, bool allowTriggerChange)
        {     
            InitializeComponent(); 

            this.allowTriggerChange = allowTriggerChange;
            this.Text = allowTriggerChange? 
                Const.ReplaceTriggerDlgTitle: Const.RenameThandlerDlgTitle;

            this.listbox.SelectionMode = SelectionMode.One;
            listbox.GotFocus    += new EventHandler(listbox_GotFocus);
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
                    if  (IsTriggerHandlerNameChanged() && !this.ValidateHandlerName(tbHandlerName.Text))
                         this.btnOK.Enabled = false;
                    else this.DialogResult  = DialogResult.OK;                               
                    break;

               case DialogResult.Cancel:            
                    this.DialogResult = result;
                    break;
            }    
        }      


        /// <summary>Interpret double click as OK/summary>
        private void listbox_DoubleClick(object sender, EventArgs e)
        {
            if (listbox.SelectedIndex >= 0) this.btnOK.PerformClick();  
        }


        /// <summary>Track selection and construct default handler name</summary>
        private void listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listbox.SelectionMode == SelectionMode.None) return;
            this.tbTriggerName.Text = listbox.SelectedItem as string;
            this.btnOK.Enabled = true; 

            string trigger = listbox.SelectedItem as string;

            this.tbHandlerName.Text = trigger == this.originalTriggerEventName?
                this.originalTriggerHandlerName:
                Utl.MakeHandlerName(trigger);
        }


        /// <summary>Handle selection via tab</summary>
        private void listbox_GotFocus(object sender, EventArgs e)
        {
            if (listbox.SelectionMode == SelectionMode.None) return;
            if (listbox.SelectedIndex < 0) listbox.SelectedIndex = 0;
        }


        /// <summary>Handle handler name listbox text change</summary>
        private void tbHandlerName_TextChanged(object sender, System.EventArgs e)
        {
            this.btnOK.Enabled = true; 
        }


        /// <summary>Edit handler name string input</summary>
        private bool ValidateHandlerName(string name)
        {
            return this.appTree.CanNameHandler(null, name, name, false)? true: 
                Utl.ShowInvalidNameDlg(name);
        }     


        /// <summary>Post-construction initialization</summary>
        private void MaxNewTriggerDlg_Load(object sender, System.EventArgs e)
        { 
            this.listbox.Focus();
            this.listbox.TabIndex = 0;
            this.listbox.SelectedIndexChanged += new EventHandler(listbox_SelectedIndexChanged);

            this.appTree = MaxManager.Instance.AppTree();
            this.originalTriggerNode = this.appTree.TriggerNode;
            this.originalTriggerEventName = this.originalTriggerNode.GetEventName();
            this.originalTriggerHandlerName = this.originalTriggerNode.GetFunctionName();
 
            int i = 0;                            // Find current trigger in list
            foreach(string entry in this.listbox.Items) 
            {     
                if  (entry == this.originalTriggerEventName)
                     break;
                else i++;
            }

            listbox.SelectedIndex = i;            // Show current trigger as selected

            if  (this.allowTriggerChange)
                 this.listbox.Focus();
            else
            {
                this.tbHandlerName.Focus();
                this.listbox.SelectionMode = SelectionMode.None;
            }
        }


        /// <summary>Indicate if user has changed the trigger</summary>
        public bool IsTriggerChanged()
        {
            return this.tbTriggerName.Text != null && this.tbTriggerName.Text.Length > 0
                && this.tbTriggerName.Text != this.originalTriggerEventName;
        }


        /// <summary>Indicate if user has changed the triggerhandler name</summary>
        public bool IsTriggerHandlerNameChanged()
        {
            return this.tbHandlerName.Text != null && this.tbHandlerName.Text.Length > 0
                && this.tbHandlerName.Text != this.originalTriggerHandlerName;
        }


        /// <summary>User-selected trigger, if any</summary>
        public string TriggerEventName    { get { return this.tbTriggerName.Text; } }

        /// <summary>User-entered handler name, if any</summary>
        public string TriggerHandlerName  { get { return this.tbHandlerName.Text; } }

        /// <summary>Current app tree trigger node, before change</summary>
        public MaxAppTreeNodeEVxEH OriginalTriggerNode 
        { get { return this.originalTriggerNode;} }

        /// <summary>Current trigger handler name, before change</summary>
        public string OriginalHandlerName { get { return this.originalTriggerHandlerName;} }

        /// <summary>Current trigger name, before change</summary>
        public string OriginalEventName   { get { return this.originalTriggerEventName;  } }

        private bool allowTriggerChange;
        private MaxAppTree appTree;
        private string originalTriggerEventName;
        private string originalTriggerHandlerName;
        private MaxAppTreeNodeEVxEH originalTriggerNode;

        #region Windows Form Designer generated code
   
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MaxNewTriggerDlg));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.listbox = new System.Windows.Forms.ListBox();
            this.tbTriggerName = new System.Windows.Forms.TextBox();
            this.labTriggerName = new System.Windows.Forms.Label();
            this.tbHandlerName = new System.Windows.Forms.TextBox();
            this.labHandlerName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(222, 218);
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
            this.btnCancel.Location = new System.Drawing.Point(310, 218);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.OnClick);
            // 
            // listbox
            // 
            this.listbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.listbox.Location = new System.Drawing.Point(8, 56);
            this.listbox.Name = "listbox";
            this.listbox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listbox.Size = new System.Drawing.Size(376, 95);
            this.listbox.TabIndex = 0;
            // 
            // tbTriggerName
            // 
            this.tbTriggerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTriggerName.Location = new System.Drawing.Point(8, 30);
            this.tbTriggerName.Name = "tbTriggerName";
            this.tbTriggerName.ReadOnly = true;
            this.tbTriggerName.Size = new System.Drawing.Size(375, 20);
            this.tbTriggerName.TabIndex = 8;
            this.tbTriggerName.Text = "";
            // 
            // labTriggerName
            // 
            this.labTriggerName.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labTriggerName.Location = new System.Drawing.Point(8, 14);
            this.labTriggerName.Name = "labTriggerName";
            this.labTriggerName.Size = new System.Drawing.Size(136, 19);
            this.labTriggerName.TabIndex = 11;
            this.labTriggerName.Text = "Select Triggering Event";
            // 
            // tbHandlerName
            // 
            this.tbHandlerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.tbHandlerName.Location = new System.Drawing.Point(10, 178);
            this.tbHandlerName.Name = "tbHandlerName";
            this.tbHandlerName.Size = new System.Drawing.Size(376, 20);
            this.tbHandlerName.TabIndex = 1;
            this.tbHandlerName.Text = "";
            this.tbHandlerName.TextChanged += new System.EventHandler(this.tbHandlerName_TextChanged);
            // 
            // labHandlerName
            // 
            this.labHandlerName.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labHandlerName.Location = new System.Drawing.Point(10, 162);
            this.labHandlerName.Name = "labHandlerName";
            this.labHandlerName.Size = new System.Drawing.Size(112, 19);
            this.labHandlerName.TabIndex = 13;
            this.labHandlerName.Text = "Event Handfler Name";
            // 
            // MaxNewTriggerDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(392, 252);
            this.Controls.Add(this.tbHandlerName);
            this.Controls.Add(this.labHandlerName);
            this.Controls.Add(this.tbTriggerName);
            this.Controls.Add(this.listbox);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.labTriggerName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 286);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 286);
            this.Name = "MaxNewTriggerDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Replace Triggering Event";
            this.Load += new System.EventHandler(this.MaxNewTriggerDlg_Load);
            this.ResumeLayout(false);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();      
            base.Dispose(disposing);
        }
        #endregion

    } // class MaxNewTriggerDlg  

}  // namespace
