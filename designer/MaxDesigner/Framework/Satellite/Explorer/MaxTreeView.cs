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
  public class MaxTreeView: TreeView
  {
    protected MaxExplorerWindow parent;
    private   static Point mouseXY = new Point(0,0);

    MenuCommand mccClose;
    MenuCommand mccOpen;
    MenuCommand mccDelete;
    MenuCommand mccRename;
    MenuCommand mccGoTo;
    MenuCommand mcnGoTo;
    MenuCommand mccProps;
        
    static MenuCommand mcnDelete = new MenuCommand("&Delete",
           new EventHandler(MaxExplorerWindow.OnMenuNodeDelete));
    static MenuCommand mcnRename = new MenuCommand("&Rename",
           new EventHandler(MaxExplorerWindow.OnMenuNodeRename));
    static MenuCommand mcnProps  = new MenuCommand("&Properties",
           new EventHandler(MaxExplorerWindow.OnMenuNodeProperties));
    static MenuCommand separator = new MenuCommand(Const.dash);


    public MaxTreeView(MaxExplorerWindow parent)
    {
      this.parent = parent;
      this.AfterCheck += new TreeViewEventHandler(this.OnAfterCheck);

      this.ImageList  = MaxImageIndex.Instance.StockToolImages16x16.Imagelist;
      this.ImageIndex = this.SelectedImageIndex = 0;

      mccClose  = new MenuCommand("&Close",      new EventHandler(parent.OnMenuCanvasOpenClose));
      mccOpen   = new MenuCommand("&Open",       new EventHandler(parent.OnMenuCanvasOpenClose));
      mccDelete = new MenuCommand("&Delete",     new EventHandler(parent.OnMenuCanvasDelete));
      mccRename = new MenuCommand("&Rename",     new EventHandler(parent.OnMenuCanvasRename));
      mccGoTo   = new MenuCommand("&Go To",      new EventHandler(parent.OnMenuCanvasGoTo));
      mcnGoTo   = new MenuCommand("&Go To",      new EventHandler(parent.OnMenuNodeGoTo));
      mccProps  = new MenuCommand("&Properties", new EventHandler(parent.OnMenuCanvasProperties));
    }


    ///<summary>Actions on double click of tree node</summary>
    protected override void OnDoubleClick(EventArgs e)
    {
      Point pt = this.PointToClient(Control.MousePosition);     
      MaxTreeNode node = this.GetMaxTreeNodeAt(pt);
      if  (node == null) return;

      switch(node.NodeType)
      {
        case MaxTreeNode.NodeTypes.Canvas:
             parent.Maxmain.SignalTabChangeRequest(node.NodeName);
             break;

        default:
             parent.Maxmain.SignalTabChangeRequest(node.NodeName);
             break;
      }
    
      base.OnDoubleClick (e);
    }


    ///<summary>Pop node context menu</summary>
    protected override void OnMouseUp(MouseEventArgs e)
    {
      mouseXY.X = e.X; mouseXY.Y = e.Y;

      if  (e.Button == MouseButtons.Right)  
           this.PopContextMenu();
              
      base.OnMouseUp(e);
    }


    ///<summary>Pop node context menu</summary>
    private void PopContextMenu()
    {
      MaxTreeNode node = this.GetMaxTreeNodeAt(mouseXY);
      if  (node == null) return;

      PopupMenu contextmenu = new PopupMenu();

      switch(node.NodeType)
      {
        case MaxTreeNode.NodeTypes.Node:
             this.MakeNodeContextMenu(contextmenu, node); 
             break;

        case MaxTreeNode.NodeTypes.Canvas:
             this.MakeCanvasContextMenu(contextmenu, node);                       
             break;

        default: return;
      }
     
      MenuCommand selection = contextmenu.TrackPopup(this.PointToScreen(mouseXY));
    }


    ///<summary>Construct the context menu for a node node</summary>
    private void MakeNodeContextMenu(PopupMenu contextmenu, MaxTreeNode node)
    {
      MaxExplorerWindow.ToolInfo info = node.Tag as MaxExplorerWindow.ToolInfo; 

      MaxTreeNode canvasNode = info.toolType == Max.Drawing.NodeTypes.Function?
                  parent.FindByCanvasName(node.NodeName): null;

      MaxExplorerWindow.CanvasInfo canvasInfo = canvasNode == null? null: 
                        canvasNode.Tag as MaxExplorerWindow.CanvasInfo;

      bool isNodeOnActiveCanvas = canvasInfo != null && canvasInfo.isActive;
       
      mcnRename.Enabled = false; // Until we do the BeginEdit code
      mcnDelete.Enabled = false; // Until we do the delete code
      mcnGoTo.Visible   = info.toolType == Max.Drawing.NodeTypes.Function && !isNodeOnActiveCanvas;
      mcnGoTo.Tag       = info;  // Pass node info in tag for now

      contextmenu.MenuCommands.AddRange
        (new MenuCommand[] {mcnGoTo, mcnRename, mcnDelete, separator, mcnProps});
    }


    ///<summary>Construct the context menu for a canvas node</summary>
    private void MakeCanvasContextMenu(PopupMenu contextmenu, MaxTreeNode node)
    {
      MaxExplorerWindow.CanvasInfo info = parent.GetCanvasInfoAtMouse();
      if  (info == null) return;

      mccOpen.Visible   = false; // !info.isShown; Go To does double duty
      mccClose.Visible  =  info.isShown;
      mccGoTo.Visible   = !info.isActive;
      mccDelete.Visible = !node.isPrimaryCanvas();  

      mccDelete.Enabled = mccRename.Enabled = !node.isPrimaryCanvas();
      mccDelete.Enabled = false; // until we do the delete code   
      mccProps.Enabled  = false; // until we define these properties    

      contextmenu.MenuCommands.AddRange(new MenuCommand[] 
      {
        mccOpen, mccGoTo, mccClose, mccRename, mccDelete, separator, mccProps
      });
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


    ///<summary>Actions after node check box (if any) is checked</summary>
    protected void OnAfterCheck(object sender, TreeViewEventArgs e)
    {
      if (e.Action != TreeViewAction.Unknown)
          e.Node.Checked = false;
    }

  } // class MaxTreeView

} // namespace
