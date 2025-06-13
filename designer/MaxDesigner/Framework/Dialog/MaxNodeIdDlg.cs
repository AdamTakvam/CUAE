using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;



namespace Metreos.Max.Framework
{
    /// <summary>Dialog to prompt for a node ID</summary>
    public class MaxNodeIdDlg: Form
    {
        #region dialog controls

        private Button   btnOK;
        private Button   btnCancel;  
        private Label    labelNodeId;
        private TextBox  txtNodeID;
        private System.ComponentModel.Container components = null;
        #endregion

        private Point xy;
        private long  nodeID;
        public  long  NodeID { get { return nodeID; } }
      
        public MaxNodeIdDlg(Point pt)
        {  
            InitializeComponent(); 
            this.xy = pt;
        }   


        protected override void OnActivated(EventArgs e)
        {
            if  (xy.X == 0 && xy.Y == 0)  
                 this.Location = Control.MousePosition;
            else this.Location = new Point(xy.X, xy.Y - (this.Height / 2));
       
            base.OnActivated (e);
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


        private bool OnOK()
        {
            string snodeID = txtNodeID.Text;   
            long   nodeID  = Utl.atol(snodeID);  
      
            if  (nodeID == 0)
                 btnOK.Enabled = false;
            else
            {
                this.DialogResult = DialogResult.OK;
                this.nodeID = nodeID;
            }    

            return btnOK.Enabled;
        }


        private void OnTextInput(object sender, System.EventArgs e)
        {
            btnOK.Enabled = true;
        }


        private void MaxNodeIdDlg_Load(object sender, System.EventArgs e)
        {      
            this.txtNodeID.Focus();
            this.txtNodeID.TabIndex = 0;
        } 

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MaxNodeIdDlg));
            this.labelNodeId = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtNodeID = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelNodeId
            // 
            this.labelNodeId.BackColor = System.Drawing.SystemColors.Control;
            this.labelNodeId.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelNodeId.Location = new System.Drawing.Point(8, 5);
            this.labelNodeId.Name = "labelNodeId";
            this.labelNodeId.Size = new System.Drawing.Size(352, 15);
            this.labelNodeId.TabIndex = 0;
            this.labelNodeId.Text = "Enter or paste node ID";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(167, 47);
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
            this.btnCancel.Location = new System.Drawing.Point(252, 47);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.OnClick);
            // 
            // txtNodeID
            // 
            this.txtNodeID.Location = new System.Drawing.Point(8, 21);
            this.txtNodeID.Name = "txtNodeID";
            this.txtNodeID.Size = new System.Drawing.Size(319, 20);
            this.txtNodeID.TabIndex = 1;
            this.txtNodeID.Text = "";
            this.txtNodeID.TextChanged += new System.EventHandler(this.OnTextInput);
            // 
            // MaxNodeIdDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(334, 76);
            this.Controls.Add(this.txtNodeID);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.labelNodeId);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(340, 100);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(340, 100);
            this.Name = "MaxNodeIdDlg";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Go To Node";
            this.Load += new System.EventHandler(this.MaxNodeIdDlg_Load);
            this.ResumeLayout(false);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();      
            base.Dispose(disposing);
        }
        #endregion

    }   // class MaxNodeIdDlg 
}     // namespace
