using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Manager;
using Metreos.Max.GlobalEvents;
using Metreos.Max.Resources.Images;
using Crownwood.Magic.Menus;

           

namespace Metreos.Max.Drawing
{
    /// <summary>Tree control for the application tree view</summary>
    /// <remarks>Does not need to do much lookup, since references
    /// to tree nodes are cached by their owner canvases and actions,  
    /// and modifications to the tree are made via those references</remarks>
    public class MaxAppTreeView: TreeView
    {
        public  static Point   mouseXY = new Point(0,0);
        private bool inEditMode;
        private MaxAppTreeMenu menu;
        private MaxAppTree     appTree;
        private MaxAppTreeNode EventsFunctionsFolderNode;
        private MaxAppTreeNode VariableFolderNode;
        private System.ComponentModel.Container components = null;
        public  MaxAppTree     AppTree                { get { return appTree; } }
        public  MaxAppTreeMenu Menu                   { get { return menu;    } }
        public  MaxAppTreeNode EventsAndFunctionsRoot { get { return EventsFunctionsFolderNode; } }
        public  MaxAppTreeNode VariablesRoot          { get { return VariableFolderNode;} }

       
        public MaxAppTreeView(MaxAppTree appTree)
        {     
            InitializeComponent();          // See comments at MaxCanvas ctor
            this.Size = new Size(this.Size.Width - 7, this.Size.Height - 3);
            this.BorderStyle = BorderStyle.None;
            this.Location    = Const.AppTreeOffset;
            this.ItemHeight  = Const.AppTreeItemHeight;
            this.LabelEdit   = true;
            this.HideSelection = false;

            this.ImageList   = MaxImageIndex.Instance.StockToolImages16x16.Imagelist;
            this.ImageIndex  = this.SelectedImageIndex = 0;
            this.menu        = new MaxAppTreeMenu(this);
            this.appTree     = appTree;
            this.tooltip     = new ToolTip();
            this.inEditMode  = false;
            this.EventsFunctionsFolderNode = new MaxAppTreeNodeFolder(Const.AppTreeRootFolderName);
            this.VariableFolderNode = new MaxAppTreeNodeFolder(Const.AppTreeVariableFolderName);
            this.BeforeLabelEdit   += new NodeLabelEditEventHandler(OnBeforeLabelEdit);
            this.RaiseMenuActivity += OutboundHandlers.MenuOutputProxy; 
        }


        /// <summary>Add event/handler node to tree</summary>
        public MaxAppTreeNode AddFunctionWithHandler
        ( TreeNode root, MaxFunctionNode func, MaxEventNode evt, string qualifiedEventName)
        {
            // 20060906 we ordinarily pass either the event node, or the event name
            MaxAppTreeNodeEVxEH node = null;

            if (Const.IsPriorVersion08(MaxProjectSerializer.serializedVersionF))
            {                               // This block deprecated 1016
                node = new MaxAppTreeNodeEVxEH(func, evt);

                node.Add(root);
            }
            else                                   
            {
                string funcname = func.NodeName;
                MaxAppTreeNodeEVxEH existingTreeNodeEvh = null;

                MaxAppTreeNodeFunc  existingTreeNodeFunc  
                    = this.appTree.GetFirstEntryFor(funcname) as MaxAppTreeNodeFunc;

                // If a function of this name exists but is a standalone function,
                // we will promote that function to handler status. Any other means of
                // handling this situation involves complexity of presenting user with
                // choices of whether to rename, dialogs for accepting new names, error
                // checking, etc -- this is the simplest way to go for a rare occurrence.
                // If for some reason use does not want to use the called function as
                // the event handler, he can recreate another call and copy the existing
                // logic into it. 20060906

                if  (existingTreeNodeFunc is MaxAppTreeNodeEVxEH)
                     existingTreeNodeEvh = existingTreeNodeFunc as MaxAppTreeNodeEVxEH;
                else 
                if  (existingTreeNodeFunc != null)
                {                           // 20060906        
                     existingTreeNodeEvh = appTree.PromoteCalledFunctionToHandler
                         (existingTreeNodeFunc, qualifiedEventName);

                     // For each call to the function, modify the call's tree refererence
                     appTree.ModifyCallReferences(funcname, existingTreeNodeEvh);
                }                                    

                if  (existingTreeNodeEvh == null)
                {                              
                    node = new MaxAppTreeNodeEVxEH(func, evt);

                    node.Add(root);         // Add to tree

                    // Note that references to the async action represented by the treenode
                    // are added to the treenode when the async action node is constructed

                    // 0117 quasikludge fixup - treenode object should determine its own functype
                    if (node.IsUnsolicited) node.FuncType = MaxAppTreeNodeFunc.Functypes.Unsolicited;
                }
                else node = existingTreeNodeEvh;
            }

            return node;
        }


        /// <summary>Add event/handler node to tree</summary>
        public MaxAppTreeNode AddFunctionWithHandler(TreeNode root, 
            MaxFunctionNode func, MaxEventNode evt, MaxAsyncActionNode action)
        {
            MaxAppTreeNode node = new MaxAppTreeNodeEVxEH(func, evt, action);

            node.Add(root);

            return node;
        }


        /// <summary>Remove event/handler node from tree</summary>
        public bool RemoveNode(TreeNode root, MaxAppTreeNode node)
        {
            if (node != null && root.Nodes.Count > 0 && node.Parent != null) 
                root.Nodes.Remove(node);  
            return node != null;
        }


        /// <summary>Add called function node to tree</summary>
        public MaxAppTreeNode AddFunction(TreeNode root, MaxFunctionNode func)
        {
            MaxAppTreeNode node = new MaxAppTreeNodeFunc(func);

            node.Add(root);

            return node;
        }


        /// <summary>Add variable node to tree given canvas node</summary>
        public MaxAppTreeNodeVar AddVariable(MaxRecumbentVariableNode var, long nodeID)
        {
            MaxAppTreeNodeVar node = new MaxAppTreeNodeVar(var);
            node.Pnode = this.VariableFolderNode;
            if  (nodeID != 0) node.NodeID = nodeID; // Deserializing

            node.Add(this.VariableFolderNode);

            return node;
        }


        /// <summary>Remove variable node from tree given canvas node</summary>
        public bool RemoveVariable(MaxVariableNode var)
        {
            MaxAppTreeNode node = this.FindByVariable(var);
            if (node != null) this.Nodes.Remove(node);
            return node != null;
        }


        /// <summary>Remove variable node from tree given node</summary>
        public bool RemoveVariable(TreeNode root, MaxAppTreeNode node)
        {
            if (node != null) this.VariableFolderNode.Nodes.Remove(node);
            return node != null;
        }



        /// <summary>Rename variable node</summary>
        public bool RenameVariable(MaxVariableNode var)
        {
            MaxAppTreeNode node = this.FindByVariable(var);
            if (node != null) node.Text = var.NodeName;
            return node != null;   
        }
    

        /// <summary>Return tree node hosting supplied variable</summary>
        public MaxAppTreeNode FindByVariable(IMaxNode var)
        {
            foreach(MaxAppTreeNode x in this.VariableFolderNode.Nodes)
            {
                MaxAppTreeNodeVar node = x as MaxAppTreeNodeVar;  
                if  (node != null && node.CanvasNodeVariable.NodeID == var.NodeID) return node;
            }
             
            return null;
        }


        ///<summary>Pop node context menu</summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            mouseXY.X = e.X; mouseXY.Y = e.Y;

            if (e.Button == MouseButtons.Right) 
            {               
                this.SelectedNode = this.GetAppTreeNodeAt(new Point(e.X, e.Y));

                menu.PopContextMenu();
            }
              
            base.OnMouseUp(e);
        }


        ///<summary>Get tree node at context menu coordinates</summary>
        public MaxAppTreeNode GetMaxTreeNodeAtMouse()
        {
            return this.GetAppTreeNodeAt(mouseXY);      
        }


        ///<summary>Get tree node at specified client coordinates</summary>
        public MaxAppTreeNode GetAppTreeNodeAt(Point pt)
        {
            TreeNode x  = this.GetNodeAt(pt);
            return   x as MaxAppTreeNode;
        }


        ///<summary>Indicate via cursor if one of our toolbox objects is dragged in</summary>
        private void MaxAppTreeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(Const.toolboxDropObjectClassType)?
                DragDropEffects.Copy: DragDropEffects.None;
        }


        ///<summary>Handle drop of a variable or event from the toolbox</summary>
        private void MaxAppTreeView_DragDrop(object sender, DragEventArgs e)
        {
            IDataObject dropdata = e.Data;                                    
            Object droppedObj = dropdata.GetData(Const.toolboxDropObjectClassType);

            MaxVariableTool vtool = droppedObj as MaxVariableTool; 
            MaxEventTool    etool = droppedObj as MaxEventTool;  

            if  (vtool != null) this.OnDropVariable(vtool);
            else
            if  (etool != null) this.OnDropEvent(etool); 

            MaxProject.Instance.MarkViewDirty();
        }


        ///<summary>Handle drop of a variable tool from the toolbox</summary>
        public void OnDropVariable(MaxVariableTool tool)
        {
            MaxAppTreeNodeVar varnode = this.CreateVariableNode(tool, null, 0, 0);         

            if  (varnode != null) 
            {
                 this.VariableFolderNode.Expand();
                 varnode.BeginEdit();
            }
        }


        ///<summary>Handle creation or recreation of a variable node</summary>
        public MaxAppTreeNodeVar CreateVariableNode
            ( MaxVariableTool tool, string name, long treeNodeID, long varNodeID)
        {
            MaxRecumbentVariableNode maxnode = new MaxRecumbentVariableNode(appTree.AppCanvas, tool, 
                Max.Framework.Satellite.Property.DataTypes.Type.GlobalVariable);

            if  (name == null)   name = maxnode.NodeName;        // Drag/dropped name
      
            if  (name != null)   maxnode.NodeName = name;        // Dropped or deserialized
            if  (varNodeID != 0) maxnode.NodeID   = varNodeID;   // Deserialized

            this.appTree.AppCanvas.View.Document.Add(maxnode);

            MaxAppTreeNodeVar vnode = this.AddVariable(maxnode, treeNodeID);

            return vnode;
        }


        ///<summary>Handle drop of an event tool from the toolbox</summary>
        private void OnDropEvent(MaxEventTool tool)
        {
            // This assumes that each instance of an unsolicited event must have 
            // a uniquely-named handler function.

            if (! tool.IsUnsolicitedEvent()) return;
            string handlername = Utl.MakeUniqueHandlerName(tool.Name, appTree.AppCanvas);
       
            MaxAppTree.HandlerInfo info = appTree.AddEventWithHandler
                (tool.FullQualName, handlername, 0, this.EventsAndFunctionsRoot);

            this.EventsAndFunctionsRoot.Expand();
        }


        ///<summary>Post-node-label-edit handler</summary>
        private void MaxAppTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            inEditMode = false;

            if  (e.Label == null) { e.CancelEdit= true; return; }
            MaxAppTreeNodeVar   vnode = e.Node as MaxAppTreeNodeVar;
            MaxAppTreeNodeEVxEH hnode = e.Node as MaxAppTreeNodeEVxEH;
            MaxAppTreeNodeFunc  fnode = e.Node as MaxAppTreeNodeFunc;
      
            if (vnode != null) this.AfterVariableLabelEdit(vnode, e);    else
            if (hnode != null) this.AfterEventHandlerLabelEdit(hnode,e); else
            if (fnode != null) this.AfterFunctionLabelEdit(fnode, e);  
        }


        ///<summary>Post-variable-name-edit handler</summary>
        private void AfterVariableLabelEdit(MaxAppTreeNodeVar node, NodeLabelEditEventArgs e)
        {    
            inEditMode = false;

            e.CancelEdit = true;
            IMaxNode maxnode = node.CanvasNodeVariable as IMaxNode;
            if  (maxnode == null || e.Label == null) return;   
                         
            if  (maxnode.Canvas.CanNameNode(NodeTypes.Variable, e.Label))
            {
                e.CancelEdit = false;
                node.EndEdit(false);
                appTree.RenameVariable(node, node.Text, e.Label);   
                this.SelectedNode = node; // 1227     
            }
            else Utl.ShowCannotRenameNodeDialog(false); 
        }


        ///<summary>Post-function-name-edit handler</summary>
        private void AfterFunctionLabelEdit(MaxAppTreeNodeFunc node, NodeLabelEditEventArgs e)
        {     
            inEditMode = false;

            e.CancelEdit = true;

            if (Config.EnableCallFunctionNode)
            {
                MaxCallNode maxnode = node.First; if (maxnode == null) return;
                bool result = maxnode.TryFunctionNameChange(node.Text, e.Label, node);

                // If function name change was successful, the treenode has been deleted,
                // having been replaced with another, and we ended the treenode edit 
                // session, to dismiss the edit box, in apptree.RemoveFunction
                if  (result) 
                     e.CancelEdit = false;
                else Utl.ShowCannotRenameNodeDialog(true);  
            }
            else
            {
                MaxFunctionNode maxnode = node.CanvasNodeFunction as MaxFunctionNode;
                if  (maxnode == null) return;      
    
                if (maxnode.Canvas.CanNameNode(NodeTypes.Function, e.Label))
                {
                    e.CancelEdit = false;
                    node.EndEdit(false);
                    appTree.RenameFunction(node, node.Text, e.Label);     
                }
                else Utl.ShowCannotRenameNodeDialog(true); 
            }
        }


        ///<summary>Post-handler-name-edit handler</summary>
        private void AfterEventHandlerLabelEdit(MaxAppTreeNodeEVxEH node, NodeLabelEditEventArgs e)
        {     
            // We must cancel the edit, even when the handler name change is valid,
            // in order to be able to replace the edited text with text of our own 
            // choosing, which we do in the RenameHandler call.

            e.CancelEdit = true;
            MaxFunctionNode maxnode = node.CanvasNodeFunction as MaxFunctionNode;
            if  (maxnode == null) return;      
    
            string newHandlerName = this.CanChangeHandlerTo(node, maxnode, e.Label);

            if  (newHandlerName == null)      
                 Utl.ShowCannotRenameNodeDialog(true);

            else appTree.RenameHandler(node, maxnode.NodeName, newHandlerName);
        }


        ///<summary>Edit new handler name entered by user</summary>
        private string CanChangeHandlerTo(MaxAppTreeNodeEVxEH node, MaxFunctionNode maxnode, string newname)
        {      
            // For now we edit name in place, although this can be confusing. 
            // The tree node label is of the form EventName: HandlerName, and 
            // when we open up the node for edit, of course the entire string
            // is editable. The user may replace the entire string with a new
            // handler name, or may just edit the handler part of the existing
            // string. We may wish to provide a custom editor in place of this
            // scheme when we have time.

            if  (newname == null || newname.Length == 0) return null;
            int  colonpos = newname.IndexOf(Const.colon);
            string s = colonpos >= 0? newname.Substring(colonpos+1): newname;
            string handlername = s.Trim();

            return maxnode.Canvas.CanNameNode(NodeTypes.Function, handlername)? handlername: null;
        }


        ///<summary>Actions on mouse button down</summary>
        private void MaxAppTreeView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            #if(false)
            // This was code utilized when we used this event to clear stale properties,
            // prior to 205-b in which we resumed showing properties by default on focus
            MaxAppTreeNode propertiesNode = menu.CurrentPropertiesNode;
            if  (propertiesNode == null) return;

            mouseXY.X = e.X; mouseXY.Y = e.Y;
            MaxAppTreeNode clickednode = this.GetAppTreeNodeAt(mouseXY); 
            if  (clickednode == propertiesNode) return;
              
            Framework.Satellite.Property.PmProxy.PropertyWindow.Clear(this); 
            menu.CurrentPropertiesNode = null;  
            #endif

            MaxAppTreeNode propertiesNode = menu.CurrentPropertiesNode;
      
            mouseXY.X = e.X; mouseXY.Y = e.Y;
            MaxAppTreeNode clickednode = this.GetAppTreeNodeAt(mouseXY); 

            this.OnFocused(clickednode);      
        }


        ///<summary>Actions on focus received</summary>
        public void OnFocused(MaxAppTreeNode clickednode)
        {    
            if  (clickednode == null)        
                 Framework.Satellite.Property.PmProxy.PropertyWindow.Clear(this); 

            else switch(clickednode.NodeType)
            {
                case MaxAppTreeNode.NodeTypes.Folder:
                     menu.OnFolderProperties(clickednode as MaxAppTreeNodeFolder);            
                     break;

                case MaxAppTreeNode.NodeTypes.Function:

                     switch(clickednode.Subtype)
                     {
                        case MaxAppTreeNode.Subtypes.EventAndHandler:
                             menu.OnEventProperties(clickednode as MaxAppTreeNodeEVxEH);
                             break;

                        default: // Function props not implemented so clear props window
                             Framework.Satellite.Property.PmProxy.PropertyWindow.Clear(this);
                             menu.OnFunctionProperties(clickednode as MaxAppTreeNodeFunc);
                             break;
                     }                  
                     break;

                case MaxAppTreeNode.NodeTypes.Variable:
                     menu.OnVariableProperties(clickednode as MaxAppTreeNodeVar);
                     break;
            }
        }


        /// <summary>Trap and handle ctrl-tab, delete</summary>
        protected override bool IsInputKey(Keys keyData)
        {
            if ((keyData & Keys.KeyCode) == Keys.Tab && (keyData & Keys.Modifiers) == Keys.Control)         
                 MaxManager.Instance.GoToPriorTab();
            else
            if ((keyData & Keys.KeyCode) == Keys.Delete) 
                // We will see delete key here only if it is not hooked as a menu 
                // shortcut, in which case delete will be forwarded from framework 
                // via MaxAppTree.OnEditDelete(), this.EditDelete()
                this.EditDelete();    
                                
            return base.IsInputKey (keyData);
        }



        /// <summary>Indicate if tree node selected, if any, can be deleted</summary>
        public bool IsDeletableItemSelected()
        {
            MaxAppTreeNode node = this.SelectedNode as MaxAppTreeNode;
            if (node == null) return false;
            if (node.NodeType == MaxAppTreeNode.NodeTypes.Variable) return true;
            if (node.NodeType != MaxAppTreeNode.NodeTypes.Function) return false;
            // Only called function can be deleted from app tree
            return node.Subtype == MaxAppTreeNode.Subtypes.None; 
        }


        /// <summary>Show properties on selected node</summary>
        protected void ShowSelectedProperties()
        {     
            MaxAppTreeNode node = this.SelectedNode as MaxAppTreeNode;           
            if  (node is MaxAppTreeNodeEVxEH)
                 menu.OnEventProperties   (node as MaxAppTreeNodeEVxEH);
            else if (node is MaxAppTreeNodeVar)
                 menu.OnVariableProperties(node as MaxAppTreeNodeVar);
            else if (node is MaxAppTreeNodeFunc)
                 menu.OnFunctionProperties(node as MaxAppTreeNodeFunc);
        }


        /// <summary>Handle forwarded delete key or Edit/Delete from framework</summary>
        public void EditDelete()
        {
            // If a function or variable is being currently edited,
            // we don't want to remove any nodes at this time
            if(inEditMode) return;

            MaxAppTreeNode node = this.SelectedNode as MaxAppTreeNode;
            if (node != null) menu.OnDeleteKey(node);
        }


        /// <summary>Notify framework of edit actions available on current selection</summary>
        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            base.OnAfterSelect(e);

            if (this.IsDeletableItemSelected() && !this.CanDelete)
            {
                this.CanDelete = true;
                RaiseMenuActivity(this, CanDeleteEventArgs);
            }
            else 
            if (this.CanDelete)
            {
                this.CanDelete = false;
                RaiseMenuActivity(this, CannotDeleteEventArgs);
            }
    
            this.ShowSelectedProperties();
        }


        ///<summary>Disallow in-place editing of certain nodes</summary>
        private void OnBeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            inEditMode = true;

            if ((e.Node is MaxAppTreeNodeFolder)        
             || (e.Node is MaxAppTreeNodeEVxEH     
                 && !menu.IsUnsolicitedEvent(e.Node as MaxAppTreeNodeEVxEH)))      
                 e.CancelEdit = true;       
        }


        ///<summary>Show tooltip for selected nodes</summary>
        protected override void OnMouseMove(MouseEventArgs e)   
        {
            base.OnMouseMove(e);  

            #region app tree tooltip debugging
            #if(false)

            MaxAppTreeNodeFunc node = this.GetNodeAt(e.X, e.Y) as MaxAppTreeNodeFunc; 
     
            string tiptext = node == null? Const.emptystr: 
                node.NodeID.ToString() + Const.blank + node.GetFunctionName();

            if (node != null)
            {
                int calls = node.CanvasNodeCallActions == null? 0: 
                    node.CanvasNodeCallActions.Count;
                if (calls > 0) tiptext = tiptext + ": calls " + calls;

                MaxAppTreeNodeEVxEH evhnode = node as MaxAppTreeNodeEVxEH; 
          
                int refs  = evhnode == null || evhnode.References == null? 0: 
                    evhnode.References.Count;
                if (evhnode != null) tiptext = tiptext + ": references " + refs;
            }
       
            this.tooltip.SetToolTip(this, tiptext); 

            #endif 
            #endregion
        }


        /// <summary>Notify framework no edit actions are available here</summary>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus (e);
            if  (this.CanDelete)
            {
                 this.CanDelete = false;
                 RaiseMenuActivity(this, CannotDeleteEventArgs);
            }
        }

        protected ToolTip tooltip;
        protected bool    CanDelete;
        private MaxAppTreeNodeEVxEH pendingTreeNode; // Tree node pending some action
        public  MaxAppTreeNodeEVxEH PendingTreeNode 
        { get { return pendingTreeNode; } set { pendingTreeNode = value; } }

        public    event   GlobalEvents.MaxMenuOutputHandler RaiseMenuActivity;
        protected static  MaxMenuOutputEventArgs CanDeleteEventArgs     = new MaxMenuOutputEventArgs
            (MaxMenuOutputEventArgs.MaxEventTypes.CanDelete, true);
        protected static  MaxMenuOutputEventArgs CannotDeleteEventArgs  = new MaxMenuOutputEventArgs
            (MaxMenuOutputEventArgs.MaxEventTypes.CanDelete, false);

        #region Windows Form Designer generated code
   
        private void InitializeComponent()
        {
            // 
            // MaxAppTreeView
            // 
            this.AllowDrop = true;
            this.Size = new System.Drawing.Size(300, 300);
            this.Text = "Form1";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MaxAppTreeView_MouseDown);
            this.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.MaxAppTreeView_AfterLabelEdit);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MaxAppTreeView_DragEnter);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MaxAppTreeView_DragDrop);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }
        #endregion

    } // class MaxAppTreeView:
}   // namespace

