using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Metreos.Max.Resources.Images;
using Microsoft.Win32;

 
namespace Metreos.Max.Framework.ToolsOptions
{
    /// <summary>Tool/Options (global config) dialog</summary>
    public class MaxOptionsDlg: Form
    {
        #region dialog controls

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;  
        private MaxToolsOptionsTreeView tree; 
        #endregion 

        protected UserControl tabdlg;
                        
        public MaxOptionsDlg()
        {      
            this.InitializeComponent();

            tree = new MaxToolsOptionsTreeView(this);
            tree.Size     = treeWH;
            tree.Location = treeXY;                
            tree.TabIndex = 0;
            tree.BorderStyle = BorderStyle.FixedSingle;
            tree.BackColor   = Color.White; // Const.ColorMaxBackground;
            this.Controls.Add(tree);  

            this.Text = Const.toolsOptionsDlgTitle;
        }


        /// <summary>Notification from tree that a selection was made</summary>
        public void OnTabSelection(TreeNode x)
        {
            MaxOptionsItemTreeNode node = x as MaxOptionsItemTreeNode;
            if (node == null) return;

            if (this.tabdlg != null) 
            {   
                this.tabdlg.Dispose();
                this.Controls.Remove(this.tabdlg);
                this.tabdlg = null;
            }

            if  (node == itnGeneral)           
                 this.tabdlg = new MaxOptsGeneral();  
            else if (node == itnGraphs)     
                 this.tabdlg = new MaxOptsCanvas(); 
            else if (node == itnBuild)  
                 this.tabdlg = new MaxOptsBuild();  
            else if (node == itnAppServer) 
                 this.tabdlg = new MaxOptsServer();
            else if (node == itnDebugger) 
                 this.tabdlg = new MaxOptsDebug(); 

            if  (this.tabdlg == null) return;

            this.tabdlg.Location = tabXY;
            ((IMaxToolsOptions)this.tabdlg).OkButton = this.btnOK;

            this.Controls.Add(this.tabdlg); 
        } 


        /// <summary>Handle button</summary>
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


        /// <summary>Post-construction, pre-display</summary>
        private void MaxOptionsDlg_Load(object sender, System.EventArgs e)
        {
            itnGeneral   = new MaxOptionsItemTreeNode(Const.toolsOptionsTabNameGeneral);
            itnGraphs    = new MaxOptionsItemTreeNode(Const.toolsOptionsTabNameGraphs);
            itnBuild     = new MaxOptionsItemTreeNode(Const.toolsOptionsTabNameBuild);
            itnAppServer = new MaxOptionsItemTreeNode(Const.toolsOptionsTabNameAppServer);
            itnDebugger  = new MaxOptionsItemTreeNode(Const.toolsOptionsTabNameDebugger);

            tree.Nodes.Add(itnGeneral);
            tree.Nodes.Add(itnAppServer);
            tree.Nodes.Add(itnGraphs);
            tree.Nodes.Add(itnBuild);
            tree.Nodes.Add(itnDebugger);

            tree.SelectedNode = itnGeneral;
            this.OnTabSelection(itnGeneral);
        }


        /// <summary>Handle OK click. forwarding event to tab dialog</summary>
        private bool OnOK()
        {      
            bool canCloseTab = ((IMaxToolsOptions)tabdlg).OnOK();
            if  (canCloseTab) this.DialogResult = DialogResult.OK;
            return canCloseTab;
        }


        /// <summary>Paint overlays etc</summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int x1 = tree.Right + 16;
            int y1 = tree.Bottom - 1;
            int x2 = this.btnCancel.Right;
            g.DrawLine(SystemPens.Highlight,x1,y1,x2,y1);
        }     

        protected static readonly Point treeXY = new Point(9,8);
        protected static readonly Size  treeWH = new Size(162,296);
        protected static readonly Size  tabWH  = Const.toolsOptionsControlSize; 
        protected static readonly Point tabXY  
            = new Point(treeXY.X + treeWH.Width + 16, treeXY.Y);

        protected MaxOptionsItemTreeNode itnGeneral;
        protected MaxOptionsItemTreeNode itnAppServer;
        protected MaxOptionsItemTreeNode itnGraphs;
        protected MaxOptionsItemTreeNode itnBuild;
        protected MaxOptionsItemTreeNode itnDebugger;

        #region Initialization
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MaxOptionsDlg));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(507, 315);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.OnClick);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(417, 315);
            this.btnOK.Name = "btnOK";
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.OnClick);
            // 
            // MaxOptionsDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(591, 348);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(599, 382);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(599, 382);
            this.Name = "MaxOptionsDlg";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.MaxOptionsDlg_Load);
            this.ResumeLayout(false);

        }
        #endregion

    } // class MaxOptionsDlg



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // IMaxToolsOptions
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    interface IMaxToolsOptions
    {
        bool   OnOK();
        Button OkButton { set; }
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxToolsOptionsTreeView
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    ///<summary>Selection tree occupying the left part of the options dialog</summary>
    public class MaxToolsOptionsTreeView: TreeView
    {
        private MaxOptionsDlg parent;
        private bool latestIsKey;

        public MaxToolsOptionsTreeView(MaxOptionsDlg parent)
        {
            this.parent = parent;
            this.Name   = "tree";
            this.TabIndex = 1;
            this.ShowPlusMinus = this.ShowRootLines = false;

            this.ImageList = MaxImageIndex.Instance.FrameworkImages16x16.Imagelist; 
        }

        ///<summary>Effect selection on mouse down rather than wait for mouse up</summary>                               
        protected override void OnMouseDown(MouseEventArgs e)
        {
            TreeNode node = e.Button == MouseButtons.Left? this.GetNodeAt(e.X,e.Y): null;
            if (node != null) 
            {   
                this.SelectedNode = node; 
                parent.OnTabSelection(node); 
                this.latestIsKey = false;           
            }
            base.OnMouseDown(e);
        }

        ///<summary>Register selection on arrow key or first-letter</summary>
        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            // If a mouse down made the latest selection. then we do not want to
            // respond to the selection event, since we already pre-selected the
            // tree node. If instead, a key event occurred since the last mouse
            // down, then we must respond to the selection event, if we wish to 
            // handle up/down arrow and first-letter tree node selection.

            if (this.latestIsKey) parent.OnTabSelection(e.Node);
            base.OnBeforeSelect (e);
        }

        ///<summary>Set indicator so we know to respond to selection event </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            this.latestIsKey = true;
            base.OnKeyDown (e);
        }
    } // class MaxToolsOptionsTreeView



    ///<summary>A folder node in the tree</summary>
    public class MaxOptionsFolderTreeNode: TreeNode
    {
        public MaxOptionsFolderTreeNode(string name): base(name)
        {           
            this.ImageIndex         = MaxImageIndex.framework16x16IndexFolderClose;
            this.SelectedImageIndex = MaxImageIndex.framework16x16IndexFolderOpen;
        }
    }


    ///<summary>A detail node in the tree</summary>
    public class MaxOptionsItemTreeNode: TreeNode
    {
        public MaxOptionsItemTreeNode(string name): base(name)
        {           
            this.ImageIndex         = MaxImageIndex.framework16x16IndexTransparent;
            this.SelectedImageIndex = MaxImageIndex.framework16x16IndexTreeArrow;
        }
    }

}   // namespace
