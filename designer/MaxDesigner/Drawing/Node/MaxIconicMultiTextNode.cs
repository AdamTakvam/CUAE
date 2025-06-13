using System;
using System.Xml;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Metreos.Max.Core;
using Metreos.Max.Manager;
using Metreos.Max.Drawing;
using Metreos.Max.Core.Tool;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework.Satellite.Property;
using Northwoods.Go;

 

namespace Metreos.Max.Drawing
{
    /// <summary>An iconic node with a child multitext node</summary> 
    /// <remarks>Requires Create() invocation after construction. 
    /// Properties are maintained for the primary (iconic) node only.</remarks> 
    /// 
    public class MaxIconicMultiTextNode: GoNode, IMaxNode, IMaxActionNode
    {        
        private NodeTypes nodeType;             
        private string    nodeName;
        private string    fullName;
        private MaxCanvas canvas;
        private MaxTool   tool;
        private long      nodeID;
        private ArrayList linkLabelChoices;
        private MaxAnnotationNode annotation = null;
        public  MaxAnnotationNode Annotation { get { return annotation; } set { annotation = value; } }
        public  virtual bool CanAnnotate()   { return annotation == null; }
        private PropertyDescriptorCollection properties;
        public  Framework.Satellite.Property.DataTypes.Type pmObjectType;

        protected ParentSubnode pnode;
        protected ChildSubnode  cnode;
        protected GoPort        port;
        public    ChildSubnode  Cnode { get { return cnode; } }
        public    ParentSubnode Pnode { get { return pnode; } }

   
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Construction
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        /// <summary>Ctor for initial instantiation when default ctor required</summary>
        public MaxIconicMultiTextNode() 
        { 
            this.nodeType = NodeTypes.Group; 
            this.menu = new MaxNodeMenu(this);
        }


        public MaxIconicMultiTextNode(NodeTypes type, MaxCanvas canvas) 
        {
            this.nodeID = Const.Instance.NextNodeID;    
            this.Init(type, canvas); 
        }


        public MaxIconicMultiTextNode(NodeTypes type, MaxCanvas canvas, long ID) 
        {       
            this.nodeID = ID;
            this.Init(type, canvas);
        }


        private void Init(NodeTypes type, MaxCanvas canvas)
        {
            this.nodeType = type; this.canvas = canvas;
            if  (this.canvas == null) throw new ArgumentNullException(); 
            this.menu = new MaxNodeMenu(this);
        }


        /// <summary>Configure the multinode</summary>    
        public void Create(MaxTool tool, string text, string[] subtext, object[] tags)      
        {
            this.tool = tool;  
      
            switch(tool.ToolType)
            {
                case MaxTool.ToolTypes.Event:
                     this.pmObjectType = Framework.Satellite.Property.DataTypes.Type.EventInstance;
                     break;
                case MaxTool.ToolTypes.Action:
                     this.pmObjectType = Framework.Satellite.Property.DataTypes.Type.ActionInstance;
                     break;
                default:
                     this.pmObjectType = tool.PmObjectType; 
                     break;
            }
      
            this.CreateProperties(this.Tool.PackageToolDefinition);

            this.nodeName  = tool.Name == null? 
                 Const.defaultNodeName: Utl.StripQualifiers(tool.Name);  
            this.Resizable = false;  

            pnode = new ParentSubnode(this);
            pnode.Location = new PointF(0,0);         

            pnode.Initialize(tool.ImagesLg.Imagelist, tool.ImageIndexLg, tool.DisplayName);
                                             
            cnode = new ChildSubnode(this);

            for (int i = 0; i < subtext.Length; i++)      
                cnode.AddItem(subtext[i], tags[i]);   

            this.Add(pnode);             
            this.Add(cnode);

            port = new CustomPort(this);  
            this.Add(port);

            LinkLabelChangeCallback = new MaxLinkLabelChanged(OnLinkLabelChanged);

            PropertyGrid.Core.MaxPmAction pma 
                = this.Tool.PackageToolDefinition as PropertyGrid.Core.MaxPmAction;

            string[] currentChoices = 
                (pma == null || pma.ReturnValue == null || pma.ReturnValue.Values == null)?
                null: pma.ReturnValue.Values; 

            this.linkLabelChoices = Utl.MakeLinkLabelChoices(currentChoices);   
        } 


        /// <summary>Configure the multinode</summary>    
        public void Create(MaxTool tool, string text, string subtext, object tags)      
        {
            this.Create(tool, text, new string[] { subtext }, new object[] { tags } );
        } 


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Events
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public delegate void MultiTextNodeTextChanged(object sender, MultiTextNodeEventArgs e); 
        public event         MultiTextNodeTextChanged RaiseTextChanged;
        public               MaxLinkLabelChanged      LinkLabelChangeCallback;


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // ParentSubnode
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        /// <summary>The parent iconic node in the group</summary>       
        public class ParentSubnode: GoIconicNode
        {
            public ParentSubnode(MaxIconicMultiTextNode node)
            {
                this.parent    = node;
                this.DragsNode = true; 
                this.Selectable= false;
            }

            protected override GoPort CreatePort() { return null; }

            #if (false)
            public override bool OnDoubleClick(GoInputEventArgs evt, GoView view)
            {
                return parent.OnParentSubnodeDoubleClick(evt, view);
            } 
            #endif

            private MaxIconicMultiTextNode parent;
        }  


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // ChildSubnode
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        /// <summary>The child list node in the group</summary>
        public class ChildSubnode: GoListGroup
        {
            public ChildSubnode(MaxIconicMultiTextNode node)
            {
                this.parent     = node;
                this.DragsNode  = true;          
                this.Deletable  = false;
                this.Selectable = false;  
                this.Resizable  = true;        
                this.Spacing    = 0;
                this.Corner     = new SizeF(2F,2F);
                this.Alignment  = GoObject.MiddleLeft;        
                this.TopLeftMargin     = new SizeF(3F,0F);
                this.BottomRightMargin = new SizeF(5F,1F);
                this.ResizesRealtime   = false; 
                this.BorderPen = new Pen(Const.ColorNodeItemBorder, 1);      
                this.Brush = new SolidBrush(Const.ColorNodeItemBack);
            }


            /// <summary>Add an entry to the list</summary>
            public void AddItem(string text, object itemTag)
            {       
                ChildSubnodeLabel label = new ChildSubnodeLabel(this.parent, text);
                label.Tag = itemTag;        // app tree node corresponding to item 
       
                this.Add(label);
            }

            /// <summary>Return the i'th label</summary>
            public ChildSubnodeLabel Get(int index)
            {
                return(this.Count < index || index < 0)? null: this[index] as ChildSubnodeLabel;
            }

            /// <summary>Return position of string s in collection, or -1</summary>
            public int IndexOf(string s)
            {  
                int i = 0;
                if (s != null)
                    foreach(ChildSubnodeLabel lab in this)         
                        if (lab.Text == s) return i; else i++;         
                return -1;
            }

            /// <summary>Get tag associated with the list item matching supplied string</summary>
            public object GetTagData(string itemdata)
            {
                ChildSubnodeLabel label = this.Get(this.IndexOf(itemdata));
                return label == null? null: label.Tag;
            }

            /// <summary>Set tag associated with the list item matching supplied string</summary>
            public bool SetTagData(string itemdata, object newtag)
            {
                ChildSubnodeLabel label = this.Get(this.IndexOf(itemdata));
                if (label != null) label.Tag = newtag;
                return label != null;
            }        

            /// <summary>Delete subnode only when parent node is selection</summary>
            public override bool CanDelete()
            {
                return parent.Canvas.View.Selection.Primary == parent.pnode;
            }

            #if (false)
            public override bool OnDoubleClick(GoInputEventArgs evt, GoView view)
            {
                return parent.OnChildSubnodeDoubleClick(evt, view);
            } 
            #endif

            public override void AddSelectionHandles(GoSelection sel, GoObject selectedObj) { }

            private MaxIconicMultiTextNode parent;
        } 


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // ChildSubnodeLabel
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        /// <summary>Represents a text string displayed in the multitext box</summary>
        public class ChildSubnodeLabel: GoText 
        {
            public ChildSubnodeLabel(MaxIconicMultiTextNode parent, string text) 
            {       
                this.parentnode  = parent;  
                string label = text == null? Const.blank: text;
                this.Text    = label;
                this.FontSize  = this.FontSize - 1;
                this.Italic    = true;
                this.Editable  = true;  
                this.Alignment = GoObject.MiddleLeft;
                this.TextColor = Const.ColorNodeItemText;
                this.DragsNode = this.AutoResizes  = true;
                this.Wrapping  = this.AutoRescales = false;        
                this.BackgroundOpaqueWhenSelected  = false;
            } 
                                          
            public override void Changed
            ( int subhint, int oI, object ov, RectangleF or, int nI, object nv, RectangleF nr) 
            {   
                switch(subhint)
                {
                    case GoText.ChangedText:       
                         this.OnTextChanged(ov as string, nv as string);                 
                         break;
                } 
              
                base.Changed(subhint, oI, ov, or, nI, nv, nr);
            }


            /// <summary>Edit text change and fire name changed event</summary>
            public virtual void OnTextChanged(string oldtext, string newtext)
            {
                if  (this.undoingChange)
                     this.undoingChange = false;    // Intercept recursive call
                else
                if  (parentnode.CanChangeText(oldtext, newtext, this.tag))
                     parentnode.OnTextChanged(oldtext, newtext, this.tag);
                else 
                {
                    this.undoingChange = true; 
                    this.Text = oldtext;           // Results in recursive call                   
                }        
            }


            /// <summary>Show item as selected and pop context menu for item</summary>
            public override bool OnContextClick(GoInputEventArgs evt, GoView view)
            {
                this.Bordered = true;        
                parentnode.menu.PopContextMenu(this);
                this.Bordered = false;
                return true;
            }

            public override bool CanDelete() { return false; }

            public override void AddSelectionHandles(GoSelection sel, GoObject selectedObj) { }     

            protected bool undoingChange;
            protected MaxIconicMultiTextNode parentnode;
            public    MaxIconicMultiTextNode Parentnode { get { return parentnode; } }
            protected object tag;          // Should be a MaxAppTreeNodeEVxEH
            public    object Tag { get { return tag; } set { tag = value; } }
        }  
     

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // CustomPort
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        /// <summary>Override node's port so we can hook some methods</summary>
        protected class CustomPort: GoPort  
        {
            public CustomPort(MaxIconicMultiTextNode parent)
            {
                this.parent = parent;
                this.Brush  = null;
                this.Pen    = null;
                this.Size = Config.LargePorts? Const.portSizeLarge: Const.portSizeNormal;  
                this.FromSpot = this.ToSpot = NoSpot;
                this.PortObject = parent;
                this.IsValidDuplicateLinks = true;
                this.IsValidSelfNode = false;

                if  (Config.VisiblePorts)   
                {
                    // Since we now show ports on mouse hover, this is disabled
                    #if(false)
                    this.Style = GoPortStyle.Ellipse;
                    this.Brush = Const.portBrush;
                    this.Pen   = Const.portPen;
                    #endif
                }
                else this.Style = GoPortStyle.None;
            }

            public override bool CanLinkFrom()
            {
                return !MaxProject.Instance.IsMultipleNodeSelection(parent.Canvas.View, parent);
            }

            public override bool CanLinkTo()
            {
                return !MaxProject.Instance.IsMultipleNodeSelection(parent.Canvas.View, parent);
            }

            protected MaxIconicMultiTextNode parent;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        /// <summary>Callback for link label change event</summary>
        public void OnLinkLabelChanged(Object sender, string text, string oldtext)   
        { 
        }


        /// <summary>Derivative overrides to edit and confirm text change</summary>
        public virtual bool CanChangeText(string oldtext, string newtext, object tag)
        {      
            return (newtext != null && newtext.Length > 0 && !newtext.StartsWith(Const.blank));
        }


        /// <summary>Derivative overrides to catch a text change after the fact</summary>
        public virtual void OnTextChanged(string oldtext, string newtext, object tag)
        {   
            // This event is caught in MaxFunctionCanvas.OnAsyncActionHandlerNameChanged
            // but if OnTextChanged is overridden by inheritor, the event is not fired
            // unless inheritor chooses to do so.
            this.FireTextChanged(new MultiTextNodeEventArgs(oldtext, newtext, tag));     
        }


        protected override void OnBoundsChanged(RectangleF oldBounds)
        {
            if (this.Annotation != null)
                MaxIconicNode.MoveAnnotationRelative(oldBounds, this.Bounds, this.Annotation);

            base.OnBoundsChanged (oldBounds);
        }


        /// <summary>Show port on mouse over. Port is unshown in MaxFunctionCanvas mouse over</summary>       
        public override bool OnMouseOver(GoInputEventArgs evt, GoView view)
        {
            if (Config.VisiblePorts)             
                this.ShowPort(true);
            
            // Start annotation delay timer if node is annotated
            MaxCanvas.annotationState.OnMouseOverNode(this); 

            return false;
        }


        /// <summary>Do housekeeping for a re-delete of this node</summary>
        public bool CanRedoDelete()
        {
            bool result = false;
            try{ result = this.CanDeleteEx(true); } 
            catch(Exception x) { ShowCouldNotGetRefCountMsg(x.Message); }             
            return result;
        }


        /// <summary>Return indication as to whether this node can be deleted</summary>
        public override bool CanDelete()
        {
            bool result = false;
            try{ result = this.CanDeleteEx(false); } 
            catch(Exception x) { ShowCouldNotGetRefCountMsg(x.Message); }            
            return result;
        }


        /// <summary>Return indication as to whether this node can be deleted</summary>
        public virtual bool CanDeleteEx(bool isUndoRedo)
        {
            // We actually do the deletion of the referenced canvases here, which
            // we probably should not; however we don't have a good spot in mind
            // from which to do the deletion, so we'll bite the bullet for now.

            // Here we determine which of the async event handlers registered with
            // this action handle other instances of the event, and which are 
            // registered only with this action.

            // This is rewritten for v08, however it is a quick port from v07 so   
            // there are probably cleaner ways of accomplishing what we do here,  

            MaxApp     app = MaxProject.CurrentApp;
            MaxAppTree appTree = MaxManager.Instance.AppTree();
            ArrayList  singlyReferencedHandlers   = new ArrayList();
            ArrayList  multiplyReferencedHandlers = new ArrayList(); 

            foreach(ChildSubnodeLabel label in this.cnode)  
            {   
                string funcname = label.Text; 
 
                MaxAppTreeNodeFunc  treeNodeFun = label.Tag   as MaxAppTreeNodeFunc;
                if (treeNodeFun == null) return false;     
                MaxAppTreeNodeEVxEH treeNodeEvh = treeNodeFun as MaxAppTreeNodeEVxEH;

                int[] counts = RemainingReferencesCounts(this, funcname);
                bool isSingleRecursiveReference = counts[1] == 0 && counts[0] == 1;
                bool isOnlyRecursiveReferences  = counts[1] == 0 && counts[0]  > 0;
                bool isThisNodeCallsThisFunctionRecursively = IsNodeRecursiveCall(this, funcname);
                bool isDeletingFinalRecursiveReference = counts[1] == 0 && counts[0] == 0
                  && isThisNodeCallsThisFunctionRecursively;

                // If all remaining references to this particular function are recursive;
                // that is, they call or call back to that function from within the function,
                // we display a warning dialog (with Cancel option), informing that once
                // the *current node* (presumably *not* self-referential) is deleted, the
                // only remaining references to the function will be self-referential,
                // making the function unreachable. We additionally state that when they
                // want to delete this function, they must re-add an external reference,
                // at least temporarily, in order to do so.  
             
                // if (isOnlyRecursiveReferences)                 
                //     if (DialogResult.OK != this.ShowOrphanHandlerDlg(this, funcname))
                //        return false;                              

                int refcount = treeNodeFun.CanvasNodeCallActions.Count +
                   (treeNodeEvh == null? 0: treeNodeEvh.References.Count);

                if (refcount == 1) 
                {                               
                    singlyReferencedHandlers.Add(label);

                    // If an event handler function itself refers to other functions
                    // in the form of calls or async actions, the async action will not
                    // be able to be deleted until user removes those references.
                    // The exception is, if the last remaining reference is a recursive
                    // reference; that is, the node being deleted calls or calls back to
                    // this function, then we permit deletion of the node and the handler.
                                      
                    MaxFunctionCanvas canvas = app.Canvases[funcname] as MaxFunctionCanvas;

                    if (canvas != null && !canvas.CanDelete() && !isDeletingFinalRecursiveReference)
                    {
                        MaxIconicMultiTextNode recursiveNode  
                            = (counts[0] == 0 && counts[1] == 0 && isThisNodeCallsThisFunctionRecursively)?
                               this: null;

                        InformHandlerCannotDelete(this.NodeName, funcname, recursiveNode);
                        return false;
                    }
                }
                else multiplyReferencedHandlers.Add(label);
            }
    
            // Here we display all the handlers which will be deleted and solicit 
            // confirmation - if we don't get confirmation we cannot delete this node

            if (singlyReferencedHandlers.Count > 0)
            {
                string handlers = null; 
                string standaloneFunction = null; 
                int i = 0;

                foreach(ChildSubnodeLabel label in singlyReferencedHandlers) 
                    if  (label.Tag is MaxAppTreeNodeEVxEH)
                         handlers += ((++i).ToString() + Const.dotb + label.Text + Const.newline); 
                    else standaloneFunction = label.Text;     

                DialogResult result;
                                     
                if  (isUndoRedo)    // We only confirm a delete once, not on a redo
                     result = DialogResult.OK;
                else                 
                if  (standaloneFunction != null)
                     result = this.GetDeleteFunctionConfirmation(standaloneFunction);
                else result = this.GetDeleteHandlersConfirmation(handlers); 
                    
                if  (result != DialogResult.OK) return false;

                // If we got confirmation we can delete all this action's handlers   
                foreach(ChildSubnodeLabel label in singlyReferencedHandlers)    
                {
                    app.OnRemoveFunction(label.Text);        
                         
                    if  (standaloneFunction != null) 
                         appTree.RemoveFunction(standaloneFunction, true, false);
                    else appTree.RemoveHandler (label.Tag as MaxAppTreeNodeEVxEH);           
                }
            }

            // Any handler with multiple references is OK, but we must remove 
            // our reference in the handler's app tree entry, which we do here. 

            foreach(ChildSubnodeLabel label in multiplyReferencedHandlers) 
            {  
                MaxAppTreeNodeFunc  fnNode = label.Tag as MaxAppTreeNodeFunc;
                MaxAppTreeNodeEVxEH ehNode = fnNode    as MaxAppTreeNodeEVxEH;

                // We don't check whether we're a call or an async action, we try both
                fnNode.RemoveCallAction(this.NodeID);
                if (ehNode != null) ehNode.RemoveReference(this.NodeID);
            }
        
            // MaxFunctionCanvas fcanvas = this.canvas as MaxFunctionCanvas;
            // if (fcanvas != null) fcanvas.HoverNode = null;  // 20060831

            return true;
        }


        /// <summary>Count complex nodes of same type which are self- and nonself-referential</summary>
        public static int[] RemainingReferencesCounts(MaxIconicMultiTextNode thisnode, string funcname)
        {
            // Determine if all MaxIconicMultiTextNodes remaining in the script which are not thisnode, 
            // and which have the same functionality (i.e. the *name* of the node is the same 
            // (this.NodeName == CallFunction, MakeCall, etc.), reside within their own callback or call 
            // (funcname). Return counts of those nodes which do (i.e. are self-referential, they call
            // the same function they reside in), and those which do not (are not self-referential).             
            int[] counts = new int[2];  
            MaxApp app = MaxProject.CurrentApp; if (thisnode == null || app == null) return counts;

            // Utl.Trace("\nthis instance " + Utl.snid(thisnode) + Const.blank 
            //    + thisnode.NodeName + Const.blank + thisnode.Canvas.CanvasName);  

            foreach(object content in app.Canvases.Values)
            {
                MaxFunctionCanvas canvas = content as MaxFunctionCanvas; 
                if (canvas == null) continue;        
                GoDocument doc = canvas.View.Document; 

                foreach(GoObject x in doc)
                {
                    MaxIconicMultiTextNode mimtnode = x as MaxIconicMultiTextNode;
                    if  (mimtnode == null) continue;
                    // Utl.Trace("examining " + Utl.snid(mimtnode) + Const.blank + mimtnode.NodeName
                    //      + Const.blank + mimtnode.Canvas.CanvasName); 
 
                    if  (mimtnode.NodeID == thisnode.NodeID) continue;
                    if  (mimtnode.Canvas.CanvasName != funcname) continue;

                    bool hit = IsNodeRecursiveCall(mimtnode, funcname);                       
                    //  Utl.Trace(hit? " -- self ref in " + funcname: " -- not self ref");

                    if  (hit)  
                         counts[0]++; // self-referential count
                    else counts[1]++; // external ref count
                }
            }

            return counts;
        }


        /// <summary>Determine if node calls its own function</summary>
        public static bool IsNodeRecursiveCall(MaxIconicMultiTextNode node, string funcname)
        {
            if (node != null && funcname != null)  
                foreach(ChildSubnodeLabel label in node.Cnode)              
                    if (label.Text == funcname) return true;
                
            return false;
        }


        /// <summary>Actions on node losing keyboard focus</summary>
        public override void OnLostSelection(GoSelection selection)
        {
            base.OnLostSelection(selection);
        }


        /// <summary>Position node elements relative to group</summary>
        public override void LayoutChildren(GoObject childchanged)
        {
            // Note that when snap to grid is in effect, vertical links may be
            // misaligned among nodes of this derivation, for the reason that
            // the nodes differ in width based upon text, and the node bounds
            // is what is used to calculate snap to grid. If we say that the
            // port object is the port itself, the links line up, but they 
            // then pass over everything but the port. It appears that what is
            // needed is to snap to grid based upon the node icon; however it
            // is not clear whether we can override that calculation.

            if  (this.Initializing) return;
            float childX   = pnode.Location.X -(cnode.Width  / 2);
            float childY   = pnode.Location.Y + pnode.Height - 10;
            cnode.Location = new PointF(childX, childY);

            if  (this.port != null)
            {    // Adjust port location so links line up with regular iconic nodes
                RectangleF rcIcon = pnode.Image.Bounds;
                port.Left = rcIcon.Left + (rcIcon.Width  / 3) - 2;
                port.Top  = rcIcon.Top  + (rcIcon.Height / 3) + 1;
            }
        }


        /// <summary>Return index of the supplied string, or -1</summary>
        public int IndexOf(string s)
        {
            return cnode.IndexOf(s);
        }


        /// <summary>Open an editor on the i'th list item</summary>
        public void BeginEdit(int index)
        {
            ChildSubnodeLabel label = cnode.Get(index); 
            if (label != null && label.Editable) label.DoBeginEdit(this.Canvas.View);
        }


        /// <summary>Get tag for list item[i]</summary>
        public object GetTagForItem(string s)
        {
            int i = cnode.IndexOf(s);
            if (i < 0) return null;
            ChildSubnodeLabel label = cnode.Get(i);
            return label == null? null: label.Tag;
        }


        /// <summary>Set list item[i] tag -- returns old tag value</summary>
        public object SetTagForItem(string s, object newtag)
        {  
            ChildSubnodeLabel label = cnode.Get(cnode.IndexOf(s));
            if (label == null) return null;
            object oldtag = label.Tag;
            label.Tag = newtag;
            return oldtag;
        }


        /// <summary>Set or clear breakpoint on this node</summary>
        public void ToggleBreakpoint()
        {
            breakpointState = breakpointState != BreakpointStates.Off?
                BreakpointStates.Off: BreakpointStates.Set;
            this.InvalidateViews();
        }


        /// <summary>Enable or disable breakpoint on this node</summary>
        public void EnableBreakpoint(bool enable)
        {
            breakpointState = enable? BreakpointStates.Set: BreakpointStates.Disabled;  
            this.InvalidateViews();
        }


        /// <summary>Enable or disable breakpoint on this node</summary>
        public void ToggleBreakpointEnabled()
        {
            breakpointState = breakpointState >= BreakpointStates.Set?
                BreakpointStates.Disabled: BreakpointStates.Set;  
            this.InvalidateViews();
        }


        /// <summary>Allow for selection halo in invalidated region</summary>
        public override RectangleF ExpandPaintBounds(RectangleF rect, GoView view)
        {
            RectangleF bounds = base.ExpandPaintBounds (rect, view);
            if (!this.isAtBreak) return bounds;

            bounds.Inflate(5,5);
            return bounds;
        }


        public override void Paint(Graphics g, GoView view)
        {
            base.Paint(g, view);

            if (breakpointState != BreakpointStates.Off) 
                MaxActionNode.DrawBreakpoint
                    (g, breakpointState, Utl.Midpoint(this.pnode.Image.Bounds));

            if (this.isAtBreak) this.DrawBreakHalo(g);
        }


        /// <summary>Indicate graphically that this node is at a break</summary>
        public void DrawBreakHalo(Graphics g)
        {
            g.DrawRectangle(MaxSelection.debugBreakHaloPen, Rectangle.Round(this.Bounds));
        }


        protected bool isAtBreak;
        public    bool IsAtBreak 
        {
            get { return isAtBreak; }  
            set { isAtBreak = value;} 
        }


        protected BreakpointStates breakpointState;
        public    BreakpointStates BreakpointState 
        {
            get { return breakpointState; }  
            set { breakpointState = value;} 
        }


        /// <summary>Set tag associated with the list item matching supplied string</summary>
        public bool SetItemTag(string itemdata, object newtag)
        {
            return this.Cnode.SetTagData(itemdata, newtag);
        }

        /// <summary>Get tag associated with the list item matching supplied string</summary>
        public object GetItemTag(string itemdata)
        {
            return this.Cnode.GetTagData(itemdata);
        }        


        /// <summary>Show "Handler(s) and canvas(es) xxx, yyy, will be removed ...</summary>
        protected DialogResult GetDeleteHandlersConfirmation(string handlers)
        {
            return MessageBox.Show(Const.DeleteCanvasesMessage(handlers),
                Const.handlerDeleteDlgTitle, MessageBoxButtons.OKCancel, 
                MessageBoxIcon.Warning);
        }


        /// <summary>Show "Function xxx will be removed ...</summary>
        protected DialogResult GetDeleteFunctionConfirmation(string funcname)
        {
            return MessageBox.Show(Const.DeleteFunctionMessage(funcname),
                Const.functionDeleteDlgTitle, MessageBoxButtons.OKCancel, 
                MessageBoxIcon.Warning);
        }


        /// <summary>Show "Action canot be removed ... handler refers to other functions</summary>
        public static void InformHandlerCannotDelete
        ( string actionname, string funcname, MaxIconicMultiTextNode thisnode)
        {              
            MessageBox.Show(Const.CantDeleteHandlerMsg(actionname, funcname, thisnode), 
                Const.handlerDeleteDlgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        /// <summary>Show "Function has no remaining references. ... Delete?</summary
        public static DialogResult ConfirmDeleteOrphanFunction(string name, int nodecount)
        {   
            MaxProject.CancelMouse();  
            return MessageBox.Show(Manager.MaxManager.Instance,
                Const.deleteOrphanedFunctionMsg(name, nodecount), 
                Const.functionDeleteDlgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        } 


        /// <summary>Show message on failure to determine reference count for handler deletion</summary
        public static bool ShowCouldNotGetRefCountMsg(string exception)
        {             
            MessageBox.Show(Const.CouldNotDetermineRefMsg(exception),
              Const.CouldNotDetermineRefDlgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return false;
        }


        #region ShowOrphanHandlerDlg
        #if(false)
        /// <summary>Show message indicating deleting last non-recursive reference to function</summary
        public DialogResult ShowOrphanHandlerDlg(IMaxNode deletenode, string funcname)
        {
            return MessageBox.Show(MaxManager.Instance, Const.OrphanHanderMsg(deletenode, funcname),
                Const.OprphaningFuncWarning, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
        }
        #endif
        #endregion

        public class MultiTextNodeEventArgs: EventArgs
        {
            public enum EventTypes { None, TextChanged }
            public EventTypes eventType;
            public string OldName, NewName;
            public object Tag;
            public MultiTextNodeEventArgs(String oldname, string newname, object tag)
            {
                OldName = oldname; NewName = newname; Tag = tag; eventType = EventTypes.TextChanged;
            }
        }

        public void FireTextChanged(MultiTextNodeEventArgs e) 
        {                         
            if (null != RaiseTextChanged) RaiseTextChanged(this, e);
        }

        public override bool OnContextClick(GoInputEventArgs evt, GoView view)
        {
            menu.PopContextMenu(this);
            return true;
        }

        private static readonly float minX = Config.minNodeXf - Const.MultiTextOffsetX;
        private static readonly float minY = Config.minNodeYf - Const.MultiTextOffsetY;


        /// <summary>constrain nodes to positive coordinates</summary>
        public override PointF ComputeMove(PointF origLoc, PointF newLoc)
        {
            return this.canvas.View.ComputeMove(this, origLoc, newLoc, minX, minY); 
        }

        private IMaxLink deletedLink;
        protected MaxNodeMenu menu;

        public enum ChangeStates 
        {
            None, OldNoneNewNonex, OldNoneNewExist, OldSingleNewNonex, OldSingleNewExist, 
            OldMultiNewNonex, OldMultiNewExist, OldExistNewPlacehold 
        }

        public enum DeleteStates { None, Single, Multi }
        private long container;
        public  long Container   { get { return container; } set { container = value; } }

        #region IMaxNode Members

        public string     NodeName  { get { return nodeName;  } set { nodeName = value; } }
        public string     FullName  { get { return fullName;  } set { fullName = value; } }
        public long       NodeID    { get { return nodeID;    } set { nodeID = value;   } }
        public PointF     NodeLoc   { get { return Location;  } set { Location = value; } }
        public RectangleF NodeBounds{ get { return Bounds;    } }
        public NodeTypes  NodeType  { get { return nodeType;  } }
        public IGoPort    NodePort  { get { return this.port; } }
        public MaxCanvas  Canvas    { get { return canvas;    } }
        public MaxTool    Tool      { get { return tool;      } }
        public string GroupName 
        {
            get { return tool.ToolGroup == null? null: tool.ToolGroup.GroupName; } 
        }   
     

        /// <summary>Invoked after a link from this node has been drawn</summary>
        public virtual void OnLinkCreated(IMaxLink link)    
        {
            MaxActionNode.ConfigureLink(link, this.linkLabelChoices);   
        }


        public virtual void OnLinkDeleted(IMaxLink link)
        {                                       // Link is deleting itself, so
            this.deletedLink = link;            // we stage the deletion
            link.Canvas.OnPropertiesFocus(null);
            this.Document.LinksLayer.Remove(link as GoObject);
        }

        public virtual void CanDeleteLink(object sender, CancelEventArgs e) { e.Cancel = true; }

        public virtual void OnPropertiesChanged(MaxProperty[] properties) { }

        public virtual void ShowPort(bool isShowing)
        {
            if  (port == null) return;

            GoDocument doc = this.Canvas.View.Document;            
            bool oldskips = doc.SkipsUndoManager;
            doc.SkipsUndoManager = true; 

            if  (isShowing)
            {
                 port.Style = GoPortStyle.Ellipse;
                 port.Brush = Const.portBrush;
                 port.Pen   = Const.portPen;
                (this.canvas as MaxFunctionCanvas).HoverNode = this;
            }
            else port.Style = GoPortStyle.None;

            doc.SkipsUndoManager = oldskips;
             
            this.InvalidateViews();
        }
    
        #endregion

        #region MaxSelectableObject Members

        public PropertyDescriptorCollection MaxProperties { get { return this.properties; } }

        /// <summary>Ask properties manager to create this object's properties</summary>                
        public PropertyDescriptorCollection CreateProperties(PropertyGrid.Core.PackageElement pe) 
        {
            MaxPropertiesManager propertiesManager = PmProxy.PropertiesManager;

            CreatePropertiesArgs args = new
                CreatePropertiesArgs(this, pe, this.PmObjectType);

            this.properties = propertiesManager.ConstructProperties(args);
            return this.properties;
        } 

        public Framework.Satellite.Property.DataTypes.Type PmObjectType {get{return pmObjectType;}}

        public void OnPropertiesChangeRaised(MaxProperty[] properties) 
        { 
            this.OnPropertiesChanged(properties);
        }  

        #endregion

        #region MaxObject Members

        public Metreos.Max.Core.ObjectTypes MaxObjectType { get { return ObjectTypes.Node; } }

        public string ObjectDisplayName { get { return this.nodeName; } }

        public void MaxSerialize(XmlTextWriter writer)
        {
            MaxNodeSerializer serializer = MaxNodeSerializer.Instance;

            writer.WriteStartElement(Const.xmlEltNode); // <node>

            serializer.WriteCommonAttibutesA(this, writer);

            serializer.WriteIconicMultiTextNodeSpecificAttributes(this, writer);

            serializer.WriteCommonAttibutesB(this, writer);

            serializer.WriteIconicMultiTextNodeItems(this.cnode, writer);

            serializer.WriteAnnotation(this, writer);

            serializer.WriteLinks(this, this.NodePort, writer);

            serializer.WriteProperties(this, writer);

            writer.WriteEndElement(); // </node>
        }

        #endregion

        #region ICustomTypeDescriptor Members

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }
      
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }
      
        EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }
      
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }
      
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
      
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }
      
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        { 
            return GetProperties();
        }
      
        public PropertyDescriptorCollection GetProperties()
        {
            return this.MaxProperties;         
        }
      
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }
      
        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }
      
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }
      
        public string GetClassName()
        {
            TypeDescriptor.GetClassName(this, true);
            return null;
        }

        #endregion
    } // class MaxIconicMultiTextNode

}  // namespace
