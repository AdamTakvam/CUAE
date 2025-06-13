using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Northwoods.Go;
using Crownwood.Magic.Menus;
using Metreos.Max.Core;
using Metreos.Max.Resources.Images;
using Metreos.Max.GlobalEvents;



namespace Metreos.Max.Framework.Satellite.Explorer
{
    ///<summary>Explorer tree view</summary>
    public class MaxExplorerTreeView: TreeView
    {
        public   MaxExplorerWindow parent;
        private  MaxExplorerMenu   menu;

        public   static  Point   mouseXY = new Point(0,0);
        private  static  bool    projectNodeSelected;
        private  static  int     leftmargin, lastmargin, scrollaction;
        private  static  int     unit, units, width, height;
        private  static  string  priorName     = Const.emptystr;
        private  static  string  projectString = Const.emptystr;
        private  static  Bitmap  bitmapUnselected = new Bitmap(1,1);
        private  static  Bitmap  bitmapSelected   = new Bitmap(1,1);
        private  static  Bitmap  bitmapProjectNode;
        private  static  PointF  textXY  = Const.ExplorerProjectNodeTextPos;
        private  static  PointF  imageXY = Const.ExplorerProjectNodeIconPos;
        private  ToolTip tooltip;


        public MaxExplorerTreeView(MaxExplorerWindow parent)
        {
            this.parent = parent;
            this.menu   = new MaxExplorerMenu(this);

            this.ImageList  = MaxImageIndex.Instance.FrameworkImages16x16.Imagelist;
            this.ImageIndex = this.SelectedImageIndex = 0;
            this.HideSelection = false;

            this.tooltip = new ToolTip();
        }


        ///<summary>Hook the TreeView's window procedure</summary>
        protected override void WndProc(ref Message msg)
        {
            switch(msg.Msg)
            {
               case Const.WM_PAINT:

                    // Paint the project header overlay in the explorer tree, if indeed a
                    // project is currently open and the top node of the tree is visible. 
                    // We in fact paint over the placeholder node inserted on project open.
                    // The base TreeView WM_PAINT must be called prior to ours, since it 
                    // invalidates the tree view's client area -- jld

                    base.WndProc(ref msg);

                    if (!this.IsProjectNodeVisible()) return;                                                                                

                    string projectName = MaxMain.ProjectName;

                    Graphics g = this.CreateGraphics();

                    // We create the project header node text only when necessary. 
                    // We facilitate mouse selection by assigning blank text to  
                    // the underlying "real" node, of roughly the same length.

                    if (projectName != priorName)
                    {
                        priorName = projectName;                 
                        projectString = Const.ExplorerProjectString(projectName);  

                        int strlen = projectString.Length; 
                        parent.ProjectNode.Text = "".PadRight(strlen, ' ');   

                        this.PreDrawProjectNodes(projectString);
                    }            

                    bitmapProjectNode = projectNodeSelected? bitmapSelected: bitmapUnselected;

                    // When horizontal scroll position has changed, calculate the  
                    // portion of header node bitmap which will be painted 

                    if (leftmargin != lastmargin)
                    {
                        unit  = this.Width / 100; if (unit < 1) unit = 1;
                        units = unit * leftmargin;   
                        lastmargin = leftmargin;
                    }

                    width = bitmapProjectNode.Width - units;

                    // Paint the header node or portion thereof

                    g.DrawImage(bitmapProjectNode, new Rectangle(0, 0, width, height),
                        units, 0, width, height, GraphicsUnit.Pixel);                        

                    g.Dispose();  
                        
                    return;            

               case Const.WM_HSCROLL:

                    // On horizontal scrolling, we must invalidate the window region 
                    // in which our owner-drawn project header node exists. We also. 
                    // must identify the current scroll position, in order that the
                    // header node may be scrolled with the rest of the window.
            
                    leftmargin   = Utl.GetScrollPos(this.Handle, SB_HORZ);  
                    scrollaction = (int)msg.WParam & 0xff;
             
                    this.InvalidateProjectNode();
                    break;
            }

            base.WndProc(ref msg);
        }


        ///<summary>Make a pair of bitmaps representing project node selection states</summary>
        public void PreDrawProjectNodes(string text)
        {
            Bitmap  bm = new Bitmap(ImageList.Images[0]);  
            Graphics g = Graphics.FromImage(bm);
            SizeF textWH = g.MeasureString(text, SystemInformation.MenuFont); 
            Size  nodeWH = new Size(Utl.ftoi(textWH.Width) + 16 + 4, 16);

            bitmapUnselected.Dispose();
            bitmapUnselected = new Bitmap(bm, nodeWH);        
            g = Graphics.FromImage(bitmapUnselected);
            g.Clear(Color.Transparent);      

            g.DrawImage(ImageList.Images[MaxImageIndex.framework16x16IndexProject], imageXY);
            g.DrawString(text, SystemInformation.MenuFont, SystemBrushes.WindowText,textXY); 

            bitmapSelected.Dispose();
            bitmapSelected = bitmapUnselected.Clone() as Bitmap;
            g = Graphics.FromImage(bitmapSelected);
            g.Clear(Color.Transparent);

            g.FillRectangle(SystemBrushes.Highlight, 
                textXY.X, textXY.Y, textWH.Width, textWH.Height + 1); 
            g.DrawImage(ImageList.Images[MaxImageIndex.framework16x16IndexProject], imageXY); 
            g.DrawString(text, SystemInformation.MenuFont, SystemBrushes.HighlightText,textXY); 
            
            height = bitmapSelected.Height;       
            g.Dispose();  
        }


        ///<summary>Actions on double click of tree node</summary>
        protected override void OnDoubleClick(EventArgs e)
        {
            Point pt = this.PointToClient(Control.MousePosition); 
            MaxTreeNode node = this.GetMaxTreeNodeAt(pt);
            if  (node == null) return;

            switch(node.NodeType)
            {
               case MaxTreeNode.NodeTypes.App:  
                    if (MaxMain.View.IsApp && MaxMain.View.Name == node.NodeName) break;
                    menu.mcsGoTo.Tag = node;
                    parent.OnMenuAppGoTo(menu.mcsGoTo, null);
                    break;

               case MaxTreeNode.NodeTypes.Installer: 
                    if (MaxMain.View.IsInstaller && MaxMain.View.Name == node.NodeName) break;          
                    menu.mciGoTo.Tag = node;
                    parent.OnMenuInstallerGoTo(menu.mciGoTo, null);
                    break;

                case MaxTreeNode.NodeTypes.Locales:
                    if (MaxMain.View.IsLocales && MaxMain.View.Name == node.NodeName) break;
                    menu.mciGoTo.Tag = node;
                    parent.OnMenuLocalesGoTo(menu.mciGoTo, null);
                    break;

               case MaxTreeNode.NodeTypes.DbScript: 
                    if (MaxMain.View.IsDatabase && MaxMain.View.Name == node.NodeName)  break;          
                    menu.mcdGoTo.Tag = node;
                    parent.OnMenuDatabaseGoTo(menu.mcdGoTo, null);
                    break;

               case MaxTreeNode.NodeTypes.VrResx:
                    if (MaxMain.View.IsVrResx && MaxMain.View.Name == node.NodeName)  break; 
                    menu.mcvGoTo.Tag = node;
                    parent.OnMenuVrResxGoTo(menu.mcvGoTo, null);
                    break;

               case MaxTreeNode.NodeTypes.Canvas:
                    this.OnGoToCanvas(node);
                    break;

               default:
                    parent.Maxmain.SignalTabChangeRequest(node.NodeName);
                    break;
            }
    
            base.OnDoubleClick (e);
        }


        ///<summary>Handle double click (GoTo) of a canvas node</summary>
        protected void OnGoToCanvas(MaxTreeNode node)
        {
          string clickedCanvasName = node.NodeName;
          string clickedScriptName = node.Parent.Text;
          MaxMain.prompted.CanvasName = null;
          MaxMain main = parent.Maxmain;
               
          bool isRequestedCanvasInThisProject 
            = (main.CurrentViewType == MaxMain.ViewTypes.App 
            && clickedScriptName != null
            && clickedScriptName == MaxMain.View.CurrentAppName);

          if (isRequestedCanvasInThisProject)
          {
              parent.Maxmain.SignalTabChangeRequest(clickedCanvasName);
              return;
          }

          main.Dialog.OnOpenScriptRequest(clickedScriptName, clickedCanvasName); 

          parent.Maxmain.SignalTabChangeRequest(clickedCanvasName);
        }


        ///<summary>Pop node context menu</summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Right)  
            { 
                this.SelectedNode = this.GetMaxTreeNodeAt(new Point(e.X, e.Y));;

                menu.PopContextMenu();
            }
        }


        ///<summary>Get tree node at context menu coordinates</summary>
        public MaxTreeNode GetMaxTreeNodeAtMouse()
        {
            return this.GetMaxTreeNodeAt(mouseXY);      
        }


        ///<summary>Get tree node at specified client coordinates</summary>
        public MaxTreeNode GetMaxTreeNodeAt(Point pt)
        {
            TreeNode x  = this.GetNodeAt(pt);
            return   x as MaxTreeNode;
        }


        ///<summary>Identify whether project node selected on mouse down</summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            mouseXY.X = e.X; mouseXY.Y = e.Y;
            TreeNode node = this.GetNodeAt(mouseXY); 
            this.SelectedNode   = node;
            projectNodeSelected = node is MaxProjectTreeNode;

            base.OnMouseDown(e);
        }


        ///<summary>Ensure project node scroll and repaint on wheel scroll</summary>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            this.InvalidateProjectNode();  
            leftmargin = Utl.GetScrollPos(this.Handle, SB_HORZ);  
            base.OnMouseWheel (e);
        }


        ///<summary>Show tooltip for nodes with possibly lengthy text</summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove (e);  
            MaxExplorerWindow.MediaInfo info = null;
            TreeNode node = this.GetNodeAt(e.X, e.Y);  
            if (node != null) info = node.Tag as MaxExplorerWindow.MediaInfo;  
     
            string tiptext = 
               (info == null || !(node is MaxReferenceTreeNode))? Const.emptystr: 
                info.invalid? info.path: node.Text;

            this.tooltip.SetToolTip(this, tiptext);   
        }


        ///<summary>Identify whether project node selected on on arrow key select</summary>
        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            projectNodeSelected = e.Node is MaxProjectTreeNode;

            this.InvalidateProjectNode();
            base.OnBeforeSelect (e);
        }


        ///<summary>Ensure project node gets a repaint</summary>
        protected void InvalidateProjectNode()
        {
            if (this.IsProjectNodeVisible()) 
                this.Invalidate(new Rectangle(0, 0, this.Width, 16));
        }


        ///<summary>Determine if the tree's topmost node is currently visible</summary>
        private bool IsProjectNodeVisible()
        {
            return parent != null &&              // visible if ...
                parent.ProjectNode  != null && // UI is initialized, and ...
                MaxMain.ProjectPath != null && // a project is open, and ...
                parent.ProjectNode.IsVisible;  // top node scrolled in view  
        }


        /// <summary>Trap and handle ctrl-tab, delete</summary>
        protected override bool IsInputKey(Keys keyData)  // 401
        {
            if ((keyData & Keys.KeyCode) == Keys.Tab && (keyData & Keys.Modifiers) == Keys.Control)         
                parent.Maxmain.SignalTabToggleRequest();
            else
            if ((keyData & Keys.KeyCode) == Keys.Delete) 
                this.OnEditDelete();
       
            return base.IsInputKey (keyData);
        }


        /// <summary>Handle delete key, edit menu delete</summary>
        public void OnEditDelete()
        {
            parent.OnDeleteKey(this.GetNodeAt(mouseXY) as MaxTreeNode);
        }


        private void InitializeComponent()
        {
        }

        #if(false)
        protected override void OnCreateControl()
        {
            System.IO.Stream stream = null;
            Assembly assemb  = Assembly.GetAssembly(this.GetType());  
            if  (assemb != null)    
                stream  = assemb.GetManifestResourceStream(Const.AppIconNamespacePath);
            if  (stream != null)  appIcon = new Icon(stream);

            base.OnCreateControl();
        }
        #endif

        #region wm_scroll
        private  const int SB_HORZ          = 0;
        private  const int SB_VERT          = 1;

        private  const int SB_LINEUP        = 0;
        private  const int SB_LINELEFT      = 0;
        private  const int SB_LINEDOWN      = 1;
        private  const int SB_LINERIGHT     = 1;
        private  const int SB_PAGEUP        = 2;
        private  const int SB_PAGELEFT      = 2;
        private  const int SB_PAGEDOWN      = 3;
        private  const int SB_PAGERIGHT     = 3;
        private  const int SB_THUMBPOSITION = 4;
        private  const int SB_THUMBTRACK    = 5;
        private  const int SB_TOP           = 6;
        private  const int SB_LEFT          = 6;
        private  const int SB_BOTTOM        = 7;
        private  const int SB_RIGHT         = 7;
        private  const int SB_ENDSCROLL     = 8;

        //             switch(scrollaction)
        //             {
        //               case SB_THUMBPOSITION:                    
        //               case SB_THUMBTRACK:
        //                    leftmargin = ((int)msg.WParam & 0x7fff0000) >> 16; 
        //                    break;
        //                
        //               case SB_LINELEFT:  break;
        //               case SB_LINERIGHT: break;
        //               case SB_PAGELEFT:  break;
        //               case SB_PAGERIGHT: break;
        //               case SB_LEFT:      break;
        //               case SB_RIGHT:     break;
        //               case SB_ENDSCROLL: break;
        //               default: break; 
        //             }
        #endregion

    } // class MaxTreeView

} // namespace
