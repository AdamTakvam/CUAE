using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Metreos.Max.Manager;
using Metreos.Max.Framework;
using Metreos.Max.Core.Package;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework.Satellite.Property;
using Crownwood.Magic.Menus;
using Northwoods.Go;



namespace Metreos.Max.Drawing
{
    ///<summary>Helper class to manage app tree context menus</summary>
    public sealed class MaxAppTreeMenu
    {
        private MenuCommand mcxProps;
        private MenuCommand mcxReplaceTrigger;
        private MenuCommand mcxRenameHandler;
        private MenuCommand mcxReplaceEvent;
        private MenuCommand mcxSeparator;
        private MenuCommand mcxShowCalls;
        private MenuCommand mcxShowReferences;
        private MenuCommand mcxShowParameters;
        private MenuCommand mcxDelete;
        private MenuCommand mcxGoTo;

        private MenuCommand mcfProps;
        private MenuCommand mcfRename;
        private MenuCommand mcfDelete;
        private MenuCommand mcfGoTo;

        private MenuCommand mcvProps;
        private MenuCommand mcvRename;
        private MenuCommand mcvDelete;
        private MenuCommand mcvGoTo;

        private MenuCommand mcdProps;
        private MenuCommand mcdAddNewItem;

        private static MenuCommand separator = new MenuCommand(Const.dash);

        private MaxAppTreeNode currentPropertiesNode;
        public  MaxAppTreeNode CurrentPropertiesNode 
        { get { return currentPropertiesNode; } set { currentPropertiesNode = value; } }

        private MaxAppTreeView tree;


        public MaxAppTreeMenu(MaxAppTreeView tree)
        {
            this.tree = tree;

            InitMenus();
        }


        ///<summary>Pop node context menu</summary>
        public void PopContextMenu()
        {
            MaxAppTreeNode node = tree.GetAppTreeNodeAt(MaxAppTreeView.mouseXY);
            if  (node == null) return;

            PopupMenu contextmenu = new PopupMenu();

            switch(node.NodeType)
            {
                case MaxAppTreeNode.NodeTypes.Folder:
                     this.MakeFolderContexMenu(contextmenu, node);            
                     break;

                case MaxAppTreeNode.NodeTypes.Function:
              
                     switch(node.Subtype)
                     {
                        case MaxAppTreeNode.Subtypes.EventAndHandler:
                             this.MakeEventHandlerContexMenu(contextmenu, node as MaxAppTreeNodeEVxEH);
                             break;

                        default:
                             this.MakeFunctionContexMenu(contextmenu, node as MaxAppTreeNodeFunc);
                             break;
                     }                  
                     break;

                case MaxAppTreeNode.NodeTypes.Variable:
                     this.MakeVariableContexMenu(contextmenu, node as MaxAppTreeNodeVar);
                     break;

                default: return;
            }
     
            MenuCommand selection = contextmenu.TrackPopup(tree.PointToScreen(MaxAppTreeView.mouseXY));
        }


        ///<summary>Construct the context menu for an event/handler node</summary>
        private void MakeEventHandlerContexMenu(PopupMenu contextmenu, MaxAppTreeNodeEVxEH node)
        {   
            // Rename of event handler from the app tree node is currently restricted
            // to unsolicited events (that is, not events associated with async actions).
            // There is no other graphic representation of the unsolicited event other
            // than on the app tree, so of course any activity must occur from here.
            // Async events, however, can be renamed from the handler list under the 
            // async action node on the function canvas. We do not now have a means 
            // of reflecting a handler change to its associated action node, were the 
            // rename to be permitted from the app tree, so for now we simply disable 
            // the rename selection on the tree node context menu.

            // An unsolicited event can be identified in the app tree as one which  
            // exists at level one of the tree, but which is not the first node 
            // (since the trigger node is always the first node at level one)

            //  See this.isUnsolicitedEvent and this.IsTriggerEevent for logic

            mcxReplaceEvent.Visible   = false; // Not sure what benefit this would provide

            mcxRenameHandler.Visible  = !node.IsProjectTrigger;

            mcxDelete.Enabled = this.IsUnsolicitedEvent(node) || IsCalledFunction(node);

            mcxReplaceTrigger.Visible = node.IsProjectTrigger;
             
            mcxShowCalls.Enabled      = node.CanvasNodeCallActions.Count > 0;
            mcxShowReferences.Enabled = node.References.Count > 0;
            mcxShowParameters.Enabled = false;
   
            mcxRenameHandler.Tag = mcxReplaceEvent.Tag = mcxDelete.Tag = mcxGoTo.Tag 
                = mcxShowCalls.Tag = mcxShowReferences.Tag = mcxShowParameters.Tag 
                = mcxProps.Tag = node;

            contextmenu.MenuCommands.AddRange (new MenuCommand[] 
            {
                mcxGoTo, mcxSeparator, mcxShowCalls, mcxShowReferences, mcxShowParameters,
                separator, mcxRenameHandler, mcxReplaceTrigger, 
                mcxReplaceEvent, mcxDelete, separator, mcxProps
            });
        }


        ///<summary>Construct the context menu for a called function node</summary>
        private void MakeFunctionContexMenu(PopupMenu contextmenu, MaxAppTreeNodeFunc node)
        {    
            mcfRename.Tag = mcfDelete.Tag = mcfGoTo.Tag = mcxShowCalls.Tag 
                = mcxShowParameters.Tag = mcfProps.Tag = node;

            mcxShowCalls.Enabled = node.CanvasNodeCallActions.Count > 0;
            mcxShowParameters.Enabled = false;
   
            contextmenu.MenuCommands.AddRange (new MenuCommand[] 
            {
                mcfGoTo, separator, mcxShowCalls, mcxShowParameters, 
                separator, mcfRename, mcfDelete, separator, mcfProps
            });
        }


        ///<summary>Construct the context menu for a variable node</summary>
        private void MakeVariableContexMenu(PopupMenu contextmenu, MaxAppTreeNodeVar node)
        {   
            mcvRename.Tag = mcvDelete.Tag = mcvProps.Tag = node;
   
            contextmenu.MenuCommands.AddRange (new MenuCommand[] 
            { mcvRename, mcvDelete, separator, mcvProps});
        }


        ///<summary>Construct the context menu for a folder node</summary>
        private void MakeFolderContexMenu(PopupMenu contextmenu, MaxAppTreeNode node)
        {       
            MaxAppTreeNodeFolder foldernode = node as MaxAppTreeNodeFolder;
            if (foldernode == null) return;

            mcdProps.Enabled = false;  
            mcdProps.Tag = mcdAddNewItem.Tag = foldernode;

            mcdAddNewItem.Enabled = foldernode.Text.Equals(Const.AppTreeVariableFolderName);
     
            contextmenu.MenuCommands.AddRange (new MenuCommand[] 
            { mcdAddNewItem, separator, mcdProps});
        }


        private void InitMenus()
        {
            mcxReplaceTrigger = new MenuCommand(Const.menuAppTreeTriggerReplace,new EventHandler(OnReplaceTrigger));
            mcxRenameHandler  = new MenuCommand(Const.menuAppTreeHandlerRename, new EventHandler(OnRenameHandler));
            mcxReplaceEvent   = new MenuCommand(Const.menuAppTreeHandlerReplace,new EventHandler(OnReplaceEvent));
            mcxDelete         = new MenuCommand(Const.menuGenericDelete,        new EventHandler(OnDeleteHandler));
            mcxGoTo           = new MenuCommand(Const.menuAppTreeHandlerGoTo,   new EventHandler(OnGoToHandler));
            mcxSeparator      = new MenuCommand(Const.dash);
            mcxShowCalls      = new MenuCommand(Const.menuAppTreeFunctionCalls, new EventHandler(OnShowCalls));
            mcxShowReferences = new MenuCommand(Const.menuAppTreeFunctionRefs,  new EventHandler(OnShowReferences));
            mcxShowParameters = new MenuCommand(Const.menuAppTreeFunctionParams,new EventHandler(OnShowParameters));  
            mcxProps          = new MenuCommand(Const.menuGenericProperties,    new EventHandler(OnEventProperties));

            mcfRename = new MenuCommand(Const.menuGenericRename,         new EventHandler(OnRenameFunction));
            mcfDelete = new MenuCommand(Const.menuAppTreeFunctionDelete, new EventHandler(OnDeleteFunction));
            mcfGoTo   = new MenuCommand(Const.menuGenericGoTo,           new EventHandler(OnGoToFunction));
            mcfProps  = new MenuCommand(Const.menuGenericProperties,     new EventHandler(OnFunctionProperties));

            mcvRename = new MenuCommand(Const.menuGenericRename,         new EventHandler(OnRenameVariable));
            mcvDelete = new MenuCommand(Const.menuGenericDelete,         new EventHandler(OnDeleteVariable));
            mcvGoTo   = new MenuCommand(Const.menuGenericGoToDef,        new EventHandler(OnGoToVariable));
            mcvProps  = new MenuCommand(Const.menuGenericProperties,     new EventHandler(OnVariableProperties));

            mcdProps      = new MenuCommand(Const.menuGenericProperties, new EventHandler(OnFolderProperties));
            mcdAddNewItem = new MenuCommand(Const.menuGenericAddNewItem, new EventHandler(OnFolderAddNewItem));
        }


        ///<summary>Replace trigger (or rename trigger event handler)</summary>
        public void OnReplaceTrigger(object sender, EventArgs e)
        {
            string[] triggers = MaxPackages.Instance.GetTriggers();
      
            tree.EventsAndFunctionsRoot.Expand(); // Show app canvas; select trigger node
            MaxManager.Instance.GoToTab(MaxProject.CurrentApp.AppName);
            tree.SelectedNode = tree.EventsAndFunctionsRoot.Nodes[0];

            bool isChangeHandlerNameOnly = sender is Framework.Satellite.Explorer.MaxExplorerWindow;

            MaxNewTriggerDlg dlg = new MaxNewTriggerDlg(triggers, !isChangeHandlerNameOnly);
            if (DialogResult.OK != dlg.ShowDialog()) return;

            if (dlg.IsTriggerChanged())
            {
                tree.AppTree.ReplaceTrigger(dlg);
            }
            else
            if (dlg.IsTriggerHandlerNameChanged())
            {
                this.tree.AppTree.RenameFunction(dlg.OriginalTriggerNode, 
                    dlg.OriginalHandlerName, dlg.TriggerHandlerName);
            }
        }


        ///<summary>Open an editor to rename the handler part of the event/handler pair</summary>
        public void OnRenameHandler(object sender, EventArgs e)
        {
            MaxAppTreeNodeEVxEH node = null;
            MenuCommand mc = sender as MenuCommand;
            if (mc != null) node = mc.Tag as MaxAppTreeNodeEVxEH;
            this.OnRenameHandler(node);
        }


        ///<summary>Open an editor to rename the handler part of the event/handler pair</summary>
        public void OnRenameHandler(MaxAppTreeNodeEVxEH node)
        {
            if  (node == null) return;

            if  (this.IsUnsolicitedEvent(node)) 
                 node.BeginEdit(); 
             
            else this.RenameAsyncActionHandler(node);
        }


        ///<summary>Rename async handler by jumping to its async action 1007b</summary>
        public bool RenameAsyncActionHandler(MaxAppTreeNodeEVxEH treenode)
        {
            tree.PendingTreeNode = null;
                                            // Can't rename if multiple refs
            if (treenode.References.Count > 1) return Utl.ShowMultipleReferencesAlert(false);

            MaxAppTreeEvhRef refx = treenode.References[0] as MaxAppTreeEvhRef;
            if (refx == null || refx.action == null) return false;
                                            // Jump to canvas hosting async action
            MaxManager.Instance.NavigateToNode(refx.action.NodeID, true);
                                            // Find this name in handler list
            int i = refx.action.IndexOf(treenode.GetFunctionName());
            if (i < 0) return false; 

            // Once the edit is successfully completed, we will then delete this node
            // from the app tree if and only if a new node was added to the tree,
            // as opposed to being renamed in place.

            tree.PendingTreeNode = treenode;
            refx.action.BeginEdit(i);
            return true;
        }


        ///<summary>Not implemented</summary>
        public void OnReplaceEvent(object sender, EventArgs e)
        {

        }


        ///<summary>Pop dialog displaying actions calling selected function</summary>
        public void OnShowCalls(object sender, EventArgs e)
        {
            MaxAppTreeNodeFunc node = null;
            MenuCommand mc = sender as MenuCommand;
            if (mc != null) node = mc.Tag as MaxAppTreeNodeFunc; 
            if (node == null) return;
            MaxFunctionRefsDlg dlg = new MaxFunctionRefsDlg(node, true);
            dlg.ShowDialog();
        }


        ///<summary>Pop dialog displaying actions referencing selection handler</summary>
        public void OnShowReferences(object sender, EventArgs e)
        {
            MaxAppTreeNodeFunc node = null;
            MenuCommand mc = sender as MenuCommand;
            if (mc != null) node = mc.Tag as MaxAppTreeNodeFunc; 
            if (node == null) return;
            MaxFunctionRefsDlg dlg = new MaxFunctionRefsDlg(node, false);
            dlg.ShowDialog();
        }


        ///<summary>Pop dialog displaying parameters of this function</summary>
        public void OnShowParameters(object sender, EventArgs e)
        {

        }
    

        ///<summary>Indicate if event handler tree node references an unsolicited event</summary>
        public bool IsUnsolicitedEvent(MaxAppTreeNodeEVxEH node)
        {
            if (node == null) return false;

            // This formerly used level in tree. As of v08, Unsolicited is a specific subtype. 
            if (Const.IsPriorVersion08(MaxProjectSerializer.serializedVersionF))
                return node.Pnode == tree.EventsAndFunctionsRoot && node.Index > 0;

            return node.FuncType == MaxAppTreeNodeFunc.Functypes.Unsolicited;
        }


        ///<summary>Indicate if event handler tree node references the triggering event</summary>
        public static bool IsTriggerEvent(MaxAppTreeNodeEVxEH node) // Deprecated 1016
        {      
            return node != null && node.Text.IndexOf(Const.AppTreeTriggerSubstring) >= 0;  
        }


        ///<summary>Indicate if tree node references a called function</summary>
        public static bool IsCalledFunction(MaxAppTreeNode treenode)
        {
            MaxAppTreeNodeFunc  treeNodeFunc  = treenode as MaxAppTreeNodeFunc;
            MaxAppTreeNodeEVxEH treeNodeEVxEH = treenode as MaxAppTreeNodeEVxEH;         
            return treeNodeFunc != null && treeNodeEVxEH == null; 
        }


        ///<summary>Translate delete key to menu selection</summary>
        public void OnDeleteKey(MaxAppTreeNode node)
        {
            if (node == null) return;
            MaxAppTreeNodeVar   nodeVar  = node as MaxAppTreeNodeVar;
            MaxAppTreeNodeFunc  nodeFunc = node as MaxAppTreeNodeFunc;
            MaxAppTreeNodeEVxEH nodeEvh  = node as MaxAppTreeNodeEVxEH;

            MenuCommand mc = new MenuCommand(Const.emptystr);
            mc.Tag = node;

            if (nodeVar  != null) 
                this.OnDeleteVariable(mc, null); 
            else
            if (nodeEvh  != null && this.IsUnsolicitedEvent(nodeEvh)) 
                this.OnDeleteHandler (mc, null); 
            else
            if (nodeFunc != null) this.OnDeleteFunction(mc, null);
        }


        ///<summary>Delete selected from context menu</summary>
        ///<remarks>Currently menu item delete is enabled only for unsolicited 
        /// events, so that is what is handled here</remarks>
        public void OnDeleteHandler(object sender, EventArgs e)
        {
            MaxAppTreeNodeEVxEH node = null;
            MenuCommand mc = sender as MenuCommand;
            if (mc != null) node = mc.Tag as MaxAppTreeNodeEVxEH; 
            this.OnDeleteHandler(node); 
        }


        public void OnDeleteHandler(MaxAppTreeNodeEVxEH node)
        {
            MaxFunctionNode fnode = null;
            if  (node  != null) fnode = node.CanvasNodeFunction;
            if  (fnode == null) return;
            string fname = node.GetFunctionName();
            this.tree.AppTree.RemoveUnsolicitedEvent(fname);   
        }


        ///<summary>GoTo selected from context menu</summary>
        public void OnGoToHandler(object sender, EventArgs e)
        {
            MaxFunctionNode fnode = null;
            MaxAppTreeNodeEVxEH  node = null;
            MenuCommand mc = sender as MenuCommand;
            if  (mc    != null) node  = mc.Tag as MaxAppTreeNodeEVxEH;  
            if  (node  != null) fnode = node.CanvasNodeFunction;
            if  (fnode != null) this.tree.AppTree.Manager.GoToTab(fnode.NodeName);
        }


        ///<summary>Show properties for the event part of the event/handler pair</summary>
        public void OnEventProperties(object sender, EventArgs e)
        {
            MaxAppTreeNodeEVxEH node = null;
            MenuCommand mc = sender as MenuCommand;
            if  (mc    != null) node   = mc.Tag as MaxAppTreeNodeEVxEH;      
            this.OnEventProperties(node); 
            PmProxy.ShowPropertiesWindow(false);   
        }


        ///<summary>Show properties for the event part of the event/handler pair</summary>
        public void OnEventProperties(MaxAppTreeNodeEVxEH node)
        {     
            MaxEventNode enode = null;
            if  (node  != null) enode  = node.CanvasNodeEvent;
            if  (enode == null) return;
            this.currentPropertiesNode = node;
            PmProxy.ShowProperties(enode, enode.PmObjectType);               
        }


        ///<summary>Open an editor to rename a standalone (called) function</summary>
        public void OnRenameFunction(object sender, EventArgs e)
        {
            MaxAppTreeNodeFunc node = null;
            MenuCommand mc = sender as MenuCommand;
            if  (mc   != null) node = mc.Tag as MaxAppTreeNodeFunc;
            this.OnRenameFunction(node);
        }


        ///<summary>Open an editor to rename a standalone (called) function</summary>
        public void OnRenameFunction(MaxAppTreeNodeFunc node)
        { 
            if (node != null) node.BeginEdit(); 
        }


        public void OnDeleteFunction(object sender, EventArgs e)
        {
            MaxAppTreeNodeFunc  node = null;
            MenuCommand mc = sender as MenuCommand;
            if  (mc    != null) node = mc.Tag as MaxAppTreeNodeFunc;  
            if  (node  != null) 
                 this.tree.AppTree.RemoveFunction(node.Text, true, true);
        }


        ///<summary>Navigate to tab hosting indicated function</summary>
        public void OnGoToFunction(object sender, EventArgs e)
        {
            MaxAppTreeNodeFunc  node = null;
            MenuCommand mc = sender as MenuCommand;
            if  (mc    != null) node = mc.Tag as MaxAppTreeNodeFunc;  
            if  (node  != null) this.tree.AppTree.Manager.GoToTab(node.Text);
        }


        ///<summary>Show properties for indicated function</summary>
        public void OnFunctionProperties(object sender, EventArgs e)
        {
            MaxAppTreeNodeFunc node = null;
            MenuCommand mc = sender as MenuCommand;
            if  (mc != null) node = mc.Tag as MaxAppTreeNodeFunc;  
            this.OnFunctionProperties(node);
            PmProxy.ShowPropertiesWindow(false);   
        }


        ///<summary>Show properties for indicated function</summary>
        public void OnFunctionProperties(MaxAppTreeNodeFunc node)
        { 
            MaxFunctionNode maxnode = null;
            if (node    != null) maxnode = node.CanvasNodeFunction;
            if (maxnode != null)                                             
                PmProxy.ShowProperties  // 1006c this does not work
                    (maxnode, Framework.Satellite.Property.DataTypes.Type.Function);
        }


        ///<summary>Open an editor to rename a variable node and its underlying variable</summary>
        public void OnRenameVariable(object sender, EventArgs e)
        {
            MaxAppTreeNodeVar node = null;
            MenuCommand mc = sender as MenuCommand;
            if  (mc   != null) node = mc.Tag as MaxAppTreeNodeVar;
            if  (node == null) return;
            node.BeginEdit(); 
        }


        ///<summary>Delete a variable node and its underlying variable</summary>
        public void OnDeleteVariable(object sender, EventArgs e)
        {
            MaxAppTreeNodeVar node = null;
            MenuCommand mc = sender as MenuCommand;
            if  (mc   != null) node = mc.Tag as MaxAppTreeNodeVar;
            if  (node == null) return;
                                            // Confirm delete from user
            if(!Utl.ShowRemoveVariableDlg(node.CanvasNodeVariable.NodeName)) return;

            PmProxy.PropertyWindow.Clear(this); // 1227
                                            // Remove underlying MaxVariableNode
            node.CanvasNodeVariable.Canvas.View.Document.Remove(node.CanvasNodeVariable as GoObject);

            node.Remove();                  // Remove app tree node            
            MaxProject.Instance.MarkViewDirty();
        }


        public void OnGoToVariable(object sender, EventArgs e)
        {

        }


        ///<summary>Show properties for a variable</summary>
        public void OnVariableProperties(object sender, EventArgs e)
        {
            MaxAppTreeNodeVar node = null;
            MenuCommand mc = sender as MenuCommand;
            if  (mc   != null) node = mc.Tag as MaxAppTreeNodeVar;
            this.OnVariableProperties(node);
            PmProxy.ShowPropertiesWindow(false);
        }


        ///<summary>Show properties for a variable</summary>
        public void OnVariableProperties(MaxAppTreeNodeVar node)
        {   
            if (node == null) return;

            IMaxNode maxnode = node.CanvasNodeVariable as IMaxNode;
            if (maxnode == null) return;
            PmProxy.ShowProperties(maxnode, maxnode.PmObjectType); 
        }


        ///<summary>Add a global variable via context menu</summary>
        public void OnFolderAddNewItem(object sender, EventArgs e)
        {
            MaxAppTreeNodeFolder node = null;
            MenuCommand mc = sender as MenuCommand;
            if (mc   != null) node = mc.Tag as MaxAppTreeNodeFolder;   
            if (node == null) return;
            tree.OnDropVariable(Core.Tool.MaxStockTools.Instance.VariableTool); 
            MaxProject.Instance.MarkViewDirty();  
        }


        public void OnFolderProperties(object sender, EventArgs e)
        {

        }


        public void OnFolderProperties(MaxAppTreeNodeFolder node)
        {

        }
 
    }  // class MaxAppTreeMenu
}    // namespace
