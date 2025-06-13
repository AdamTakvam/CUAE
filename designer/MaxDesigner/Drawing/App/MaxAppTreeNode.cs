using System;
using System.Text;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Manager;
using Metreos.Max.Resources.Images;



namespace Metreos.Max.Drawing
{	
    ///<summary>A node in the application tree view</summary>
    public abstract class MaxAppTreeNode: TreeNode
    {
        public enum NodeTypes{ None, Folder, Function, Variable }
        public enum Subtypes { None, EventAndHandler, Reference }
        protected NodeTypes    nodetype;
        protected Subtypes     subtype;
        protected TreeNode     parentnode;
        protected long         nodeID;

        public NodeTypes NodeType{ get { return nodetype; } }
        public Subtypes  Subtype { get { return subtype;  } }
        public long      NodeID  { get { return nodeID;   }   set { nodeID = value;     } }
        public TreeNode  Pnode   { get { return parentnode; } set { parentnode = value; } } 

        public virtual void Add(TreeNode root)
        {
            this.parentnode = root;   
            this.nodeID = Const.Instance.NextNodeID;

            root.Nodes.Add(this);
        }
    }  


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    // MaxAppTreeNodeFunc
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    ///<summary>A function node in the application tree view</summary>
    ///<remarks>May be a standalone called function, or an event/handler pair</remarks>
    public class MaxAppTreeNodeFunc: MaxAppTreeNode
    {
        public MaxAppTreeNodeFunc(MaxFunctionNode canvasNode) 
        {
            this.nodetype   = MaxAppTreeNode.NodeTypes.Function;
            this.ImageIndex = this.SelectedImageIndex 
                = MaxImageIndex.stockTool16x16IndexFunction;
            this.functype   = Functypes.Called;   // 1016
            this.canvasNodeFunction = canvasNode;
            this.Text = canvasNodeFunction.NodeName;
            #region nodefont
            #if (false)
            Font font = new Font(SystemInformation.MenuFont, FontStyle.Italic);      
            this.NodeFont = font;
            #endif
            #endregion
        }

        public enum Functypes { None, Trigger, AsyncHandler, Called, Unsolicited }  
        protected   Functypes functype;         // 1016
        public      Functypes FuncType { get { return functype; } set { functype = value; } } 

        public virtual string GetFunctionName() { return this.Text; }

        protected MaxFunctionNode canvasNodeFunction;
        public    MaxFunctionNode CanvasNodeFunction{ get { return canvasNodeFunction; } }
   
        protected ArrayList canvasNodeCallActions = new ArrayList();
        public    ArrayList CanvasNodeCallActions   
        { get { return canvasNodeCallActions;} set { canvasNodeCallActions = value; } }

        public MaxCallNode First 
        {
            get
            {
                return canvasNodeCallActions.Count == 0? null: 
                canvasNodeCallActions[0] as MaxCallNode; 
            }    
        } 
     
        public void AddCallAction(MaxCallNode node)
        {
            if (!canvasNodeCallActions.Contains(node)) canvasNodeCallActions.Add(node); 
        }

        public void RemoveCallAction(MaxCallNode node)
        {
            if  (canvasNodeCallActions.Contains(node)) canvasNodeCallActions.Remove(node); 
        }

        public void RemoveCallAction(long nodeID)
        {
            object node = null;

            foreach(object x in canvasNodeCallActions)       
                if (x is MaxCallNode && (x as MaxCallNode).NodeID == nodeID) node = x;
       
            if (node != null) canvasNodeCallActions.Remove(node);
        }
    }  


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    // MaxAppTreeNodeEVxEH
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    ///<summary>An event plus handler node in the application tree view</summary>
    public class MaxAppTreeNodeEVxEH: MaxAppTreeNodeFunc
    {
        public MaxAppTreeNodeEVxEH(MaxFunctionNode func, MaxEventNode evnt): base(func)
        {
            if (func != null && evnt != null) this.Set(func, evnt);
            this.references = new ArrayList();     

            this.functype = Functypes.AsyncHandler;  
        }


        public MaxAppTreeNodeEVxEH                  
        ( MaxFunctionNode func, MaxEventNode evnt, bool unsolicited): base(func)
        {
            if (func != null && evnt != null) this.Set(func, evnt);
            this.references = new ArrayList();     
            this.functype = unsolicited? Functypes.Unsolicited: Functypes.AsyncHandler;
        }


        public MaxAppTreeNodeEVxEH(MaxFunctionNode func,  
        MaxEventNode evnt, MaxAsyncActionNode action): base(func)
        {                                        
            if  (func == null || evnt == null) return;
            this.Set(func, evnt);
            this.references = new ArrayList(); 
            if (action != null) this.AddReference(action);
            this.canvasNodeAction = action;        
        }


        protected void Set(MaxFunctionNode func, MaxEventNode evnt)
        {                                       
            this.subtype    = MaxAppTreeNode.Subtypes.EventAndHandler;
            this.ImageIndex = this.SelectedImageIndex = MaxImageIndex.stockTool16x16IndexEVxEH;
            this.canvasNodeEvent = evnt;                                           
            this.Text = MakeEventWithHandlerName(evnt, canvasNodeFunction.NodeName);
        }


        public static string MakeEventWithHandlerName(MaxEventNode evnt, string functionName)
        {                                        
            string separator = evnt.IsProjectTrigger?  Const.AppTreeTriggerSubstring: ": ";
            string eventName = evnt.Tool.Name;
            if (eventName == null || eventName.Length == 0)
                eventName = evnt.NodeName; 
            StringBuilder s = new StringBuilder(evnt.Tool.Package.Name);
            s.Append(Const.dot);
            s.Append(eventName);
            s.Append(separator);
            s.Append(functionName);
            return s.ToString();
        }


        public string MakeEventWithHandlerName(string functionname)
        {
            string x = this.Text;
            int    colonpos  = x.IndexOf(Const.colon);
            string eventpart = x.Substring(0, colonpos+2); 
            return eventpart + functionname;
        }


        public override string GetFunctionName()
        {
            int    colonpos    = this.Text.IndexOf(Const.colon);
            string handlerpart = this.Text.Substring(colonpos+2); 
            return handlerpart;
        }


        public string GetEventName()
        {
            int    colonpos  = this.Text.IndexOf(Const.colon);
            string eventpart = this.Text.Substring(0, colonpos); 
            int    spacepos  = eventpart.IndexOf(Const.blank);  // 1009
            if (spacepos >= 0) eventpart = eventpart.Substring(0, spacepos);
            return eventpart;
        }


        ///<summary>Replace this node's content with that from another node</summary>
        public void CopyFrom(MaxAppTreeNodeEVxEH newContent)
        {
            this.Text = newContent.Text;
            this.canvasNodeAction   = newContent.CanvasNodeAction;
            this.canvasNodeEvent    = newContent.CanvasNodeEvent;
            this.canvasNodeFunction = newContent.CanvasNodeFunction;
        }


        ///<summary>Add a reference to an async action which this node represents</summary>
        public bool AddReference(MaxAsyncActionNode action) 
        {                                       // 1016
            foreach(MaxAppTreeEvhRef refx in this.references)
                if (refx.action == action) return false;

            this.references.Add(new MaxAppTreeEvhRef(this, action));
            return true;
        }


        ///<summary>Add a reference to an async action which this node represents</summary>
        public bool AddReference(MaxAppTreeEvhRef refnode) 
        {                                       // 1016
            this.references.Add(refnode);
            return true;
        }


        //<summary>Remove a reference to an async action</summary>
        public bool RemoveReference(MaxAsyncActionNode action) 
        {
            return action == null? false: this.RemoveReference(action.NodeID);
        }


        //<summary>Remove a reference to an async action</summary>
        public bool RemoveReference(long nodeID) 
        {                
            MaxAppTreeEvhRef refToRemove = null;  // 1016

            foreach(MaxAppTreeEvhRef refx in this.references)
                if (refx.action != null && refx.action.NodeID == nodeID)
                {
                    refToRemove = refx;
                    break;
                } 

            if  (refToRemove != null) 
            {
                 this.references.Remove(refToRemove);
                 return true;
            }
            else return false;
        }

        protected MaxEventNode       canvasNodeEvent;  // Event node for properties only
        protected ArrayList          references;       // 1016
        protected MaxAsyncActionNode canvasNodeAction; // Action node which fires event
        public    MaxEventNode       CanvasNodeEvent  { get { return canvasNodeEvent;  } }
        public    ArrayList          References       
        { get { return references; } set { references = value; } }
        public    MaxAsyncActionNode CanvasNodeAction  // Deprecated 1016
        { get { return canvasNodeAction;} set { canvasNodeAction = value;} }
        public bool IsUnsolicited                      // Deprecated 1016
        { get { return canvasNodeAction == null && 
                (canvasNodeEvent == null || !canvasNodeEvent.IsProjectTrigger);
        }     }
        public bool IsUnsolicitedEvent                 // Deprecated 1016
        { get { return this.functype == Functypes.Unsolicited; } }
        public bool IsProjectTrigger   { get { return canvasNodeEvent.IsProjectTrigger;} }
    } 


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    // MaxAppTreeEvhRef
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    ///<summary>A reference to an event handler</summary>  
    ///<remarks>During tree deserialization the action ID is set. Immediately after
    ///a script is deserialized, the actionID is used to place this.action</remarks>
    public class MaxAppTreeEvhRef: MaxAppTreeNode
    {
        public MaxAppTreeEvhRef(MaxAppTreeNodeEVxEH refparent, MaxAsyncActionNode action)
        {
            this.refparent = refparent; 
            this.action    = action;
            this.nodeID    = Const.Instance.NextNodeID;
            this.Init();
        }

        /// <summary>Ctor used during tree deserialization</summary>
        public MaxAppTreeEvhRef(MaxAppTreeNodeEVxEH refparent, long nodeID, long actionID)
        {
            this.refparent = refparent; 
            // this.action    = action;
            this.nodeID    = nodeID;
            this.actionID  = actionID;  
            this.Init();
        }

        private void Init()
        {
            this.Pnode     = refparent.Pnode;
            this.nodetype  = refparent.NodeType;
            this.subtype   = Subtypes.Reference;
            if  (action != null) this.actionID = action.NodeID;
        }

        public override void Add(TreeNode root) {}  
        public long actionID;
        public MaxAsyncActionNode  action;
        public MaxAppTreeNodeEVxEH refparent;
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    // MaxAppTreeNodeVar
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    ///<summary>A global variable node in the application tree view</summary>
    public class MaxAppTreeNodeVar: MaxAppTreeNode
    {
        public MaxAppTreeNodeVar(MaxRecumbentVariableNode var)
        {
            this.nodetype   = MaxAppTreeNode.NodeTypes.Variable;
            this.ImageIndex = this.SelectedImageIndex   
                = MaxImageIndex.stockTool16x16IndexVariable;
            this.canvasNodeVariable = var;
            this.Text = canvasNodeVariable.NodeName;
                                            
            // We register for the node change event in order to update the tree
            // node name at the time the underlying variable node receives the
            // notification of name change via properties grid. Now that we're
            // doing this, we may be able to remove explicit tree node updates which
            // may now be redundant, so note to self to watch for this opportunity.
            // Note that ALL global variable nodes catch the event, so we must filter
            // for the node we want. Note that this event is raised on direct edit
            // of tree node, as well as via property grid. If there is a way to edit
            // the tree node directly after change via property grid, that would be
            // preferable to this broadcast approach.
            var.Canvas.NodeEvent += new Metreos.Max.Manager.MaxNodeEventHandler(OnNodeEvent);
        }

        /// <summary>Fired by canvases to indicate some node change is pending</summary>
        public void OnNodeEvent(object sender, MaxLocalNodeEventArgs e)
        {
            // We're only interested in changes to the application, not to function vars.
            MaxAppCanvas canvas = sender as MaxAppCanvas;
            if  (canvas == null) return;

            switch(e.eventtype)
            {
                case MaxLocalNodeEventArgs.NodeEventType.Rename:
                    // We would ideally like to centralize the tree node name updates
                    // As such, invoking apptree.RenameVariable (via this.TreeView) here 
                    // might be indicated, except that doing so would fire another node
                    // change event.     
                    if  (this.Text != e.OldName) break; // Filter for tree node we want       
                    this.Text = e.NodeName;             // Rename explorer node 
            
                    MaxProject.CurrentApp.OnNodeRenamed(this.CanvasNodeVariable);   
                    break;
            }
        }


        /// <summary>Returns the first subnode in a tree node</summary>
        public static TreeNode First(TreeNode node)
        {
            if  (node == null || node.Nodes.Count == 0) return null;
            IEnumerator i = node.Nodes.GetEnumerator();
            return i.MoveNext()? i.Current as TreeNode: null;
        } 


        protected IMaxNode canvasNodeVariable;
        public    IMaxNode CanvasNodeVariable { get { return canvasNodeVariable; } }
    }  


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    // MaxAppTreeNodeFolder
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    ///<summary>A folder node in the application tree view</summary>
    public class MaxAppTreeNodeFolder: MaxAppTreeNode
    {
        public MaxAppTreeNodeFolder(string text)
        {
            this.nodetype   = MaxAppTreeNode.NodeTypes.Folder;
            this.ImageIndex = this.SelectedImageIndex 
                = MaxImageIndex.stockTool16x16IndexVarFolder;
            this.Text = text;
        }
    } 

} // namespace