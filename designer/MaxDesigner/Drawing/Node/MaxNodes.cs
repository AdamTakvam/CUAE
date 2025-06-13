using System;
using System.Xml;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Manager;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework.Satellite.Property;
using PropertyGrid.Core;
using Northwoods.Go;



namespace Metreos.Max.Drawing
{
    public delegate void MaxLinkLabelChanged(object sender, string oldtext, string newtext); 


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxActionNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>An iconic node representing a specific action</summary>
    public class MaxActionNode: MaxIconicNode, IMaxActionNode
    {
        public MaxActionNode(MaxCanvas canvas, MaxTool tool): 
        base (NodeTypes.Action, canvas, tool) 
        {
            InitAction();
        }


        public MaxActionNode(MaxCanvas canvas, MaxTool tool, long id):
        base (NodeTypes.Action, canvas, tool, id)  
        {       
            InitAction();
        } 


        /// <summary>Common construction initialization</summary>
        protected void InitAction()
        {
            this.pmObjectType = this.GetPropertiesType();
            this.CreateProperties(this.Tool.PackageToolDefinition);

            this.Port.IsValidDuplicateLinks = true;  // mult links per port   

            LinkLabelChangeCallback = new MaxLinkLabelChanged(OnLinkLabelChanged);

            MaxPmAction pma = this.Tool.PackageToolDefinition as MaxPmAction;

            string[] currentChoices = 
               (pma == null || pma.ReturnValue == null || pma.ReturnValue.Values == null)?
                null: pma.ReturnValue.Values; 

            this.linkLabelChoices = Utl.MakeLinkLabelChoices(currentChoices);   
        }


        /// <summary>Return properties manager-specific type of this node</summary>
        protected virtual DataTypes.Type GetPropertiesType()
        {
            return Framework.Satellite.Property.DataTypes.Type.ActionInstance;
        }


        /// <summary>Disallow outbound link from final action</summary>
        public override bool CanLinkFrom()  
        { 
            // This prevents accidentally beginning a link gesture when dragging a selection
            if (MaxProject.Instance.IsMultipleNodeSelection(this.Canvas.View, this)) return false;
       
            MaxPmAction pmaction = this.Tool.PackageToolDefinition as MaxPmAction;
            return pmaction == null? true: !pmaction.Final;
        }  


        /// <summary>Disallow link to multiple selection</summary>
        public override bool CanLinkTo()
        {
            // This prevents accidentally beginning a link gesture when dragging a selection.
            // However it also prevents a legitimate attempt to link to a node which is a
            // member of a multipled selection. It remains to be seen which is the lesser evil.
            return !MaxProject.Instance.IsMultipleNodeSelection(this.Canvas.View, this);
        }


        /// <summary>Get array of names of async events this action must be prepared for</summary>
        public string[] GetAsyncHandler()
        {              
            MaxActionTool tool = this.Tool as MaxActionTool;
            return tool == null? null: tool.PmAction == null? null: tool.PmAction.AsyncCallbacks; 
        }


        /// <summary>Invoked after a link from this node has been drawn</summary>
        public override void OnLinkCreated(IMaxLink link)    
        {
            MaxActionNode.ConfigureLink(link, this.linkLabelChoices);      
        }


        public static void ConfigureLink(IMaxLink link, ArrayList choices)
        {
            MaxActionLink alink = link as MaxActionLink;
            if  (alink == null) return;
            string labelText = Const.initialActionLinkLabelText;

            if (MaxManager.Deserializing)
            {    // When deserializing, the link label has been set temporarily as text
                GoText t = alink.MidLabel as GoText;   
                if (t != null) labelText = t.Text;   
            }        

            alink.MidLabel = new MaxActionLinkLabel(alink, true, choices, labelText); 
      
            MaxLabeledLink.CalculateCurvature(alink);  
        }


        /// <summary>Callback for link label change event</summary>
        public MaxLinkLabelChanged LinkLabelChangeCallback;


        /// <summary>Callback for link label change event</summary>
        public void OnLinkLabelChanged(Object sender, string text, string oldtext)   
        { 
            // MaxActionLink is sender
            // Here we can check the new text against text that has been used
            // MessageBox.Show("label changed from " + oldtext + " to " + text);

            // Note that the dropdown contents are assigned by Go at the time the
            // link label is *created*, not at dropdown time. This means we cannot
            // in this manner subtract entries already used from the available
            // choices.

            // If we wish to validate text *prior* to change, we will override
            // GoText.ComputeEdit (to set or prevent set of text), and/or 
            // GoText.DoEdit (to do more than setting the text)
        }


        protected override void OnPropertiesChanged(MaxProperty[] properties)
        {
            foreach(MaxProperty x in properties)
            {
                ActionParameterProperty property = x as ActionParameterProperty;
                if  (property == null || !property.IsChanged) continue;

                switch(property.Name)
                {
                   case Const.PmFunctionName:
                        this.OnFunctionNamePropertyChanged(property);
                        break;
                }  
            }      
        }


        /// <summary>Handle function name change via property grid</summary>
        protected void OnFunctionNamePropertyChanged(ActionParameterProperty property)
        {      
            string newname  = property.Value    as string;
            string oldname  = property.OldValue as string;
            if  (oldname == newname) return;
            MaxAppTree apptree = MaxManager.Instance.AppTree();
            bool isNewFunction = (oldname == null || oldname.Length == 0); 
            bool ok = false;

            if (!Utl.IsValidFunctionName(newname))
                 Utl.ShowCannotRenameNodeDialog(true);
            else
            if (isNewFunction)              // Adding new function
            {                               // Function already exists?
                if (apptree.FindFunctionReferences(newname).Count > 0) return;

                // Add new standalone function at root level
                MaxAppTreeNode root = apptree.Tree.EventsAndFunctionsRoot;
                MaxCanvas canvas    = apptree.AppCanvas;

                ok = apptree.AddFunction(newname, canvas, root);             
            }
            else
            if (!apptree.AppCanvas.CanNameNode(NodeTypes.Function, newname))
                Utl.ShowCannotRenameNodeDialog(true);
            else                            // Renaming existing function              
            if (apptree.RenameFunction(null, oldname, newname) != null)
                ok = true;
                                            // Revert to old name if unsuccessful
            if (!ok) property.Value = oldname; 
        }  


        /// <summary>Set or clear breakpoint on this node</summary>
        public void ToggleBreakpoint()
        {
            breakpointState = breakpointState != BreakpointStates.Off?
                BreakpointStates.Off: BreakpointStates.Set;
            this.InvalidateViews();
        }


        /// <summary>Enable or disable breakpoint on this node</summary>
        public void ToggleBreakpointEnabled()
        {
            breakpointState = breakpointState == BreakpointStates.Set?
                BreakpointStates.Disabled: BreakpointStates.Set;  
            this.InvalidateViews();
        }


        /// <summary>Enable or disable breakpoint on this node</summary>
        public void EnableBreakpoint(bool enable)
        {
            breakpointState = enable? BreakpointStates.Set: BreakpointStates.Disabled;  
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


        /// <summary>Also paint breakpoint indicator and selection halo if necessary</summary>
        public override void Paint(Graphics g, GoView view)
        {
            base.Paint(g, view);

            if (this.breakpointState != BreakpointStates.Off) 
                DrawBreakpoint(g, breakpointState, this.Location);

            if (this.isAtBreak) this.DrawBreakHalo(g);
        }


        /// <summary>Indicate graphically that breakpoint is set on this node</summary>
        public static void DrawBreakpoint(Graphics g, BreakpointStates state, PointF location)
        {
            if (state == BreakpointStates.Off) return;

            Brush brush = state == BreakpointStates.Set? Brushes.Red: Brushes.Silver;
            Pen   pen   = state == BreakpointStates.Set? Pens.Black:  Pens.Maroon;
                                  
            Point pt = new Point(Utl.ftoi(location.X) + Const.breakpointIconOffsetX, 
                                 Utl.ftoi(location.Y) + Const.breakpointIconOffsetY);     
            Rectangle rect = new Rectangle(pt, new Size(6,6));

            g.DrawEllipse(pen, rect);
            rect.Inflate(-1,-1);
            g.FillEllipse(brush, rect);
        }


        /// <summary>Indicate graphically that this node is at a break</summary>
        public void DrawBreakHalo(Graphics g)
        {
            g.DrawRectangle(MaxSelection.debugBreakHaloPen, Rectangle.Round(this.Bounds));
        }


        protected BreakpointStates breakpointState;
        public    BreakpointStates BreakpointState 
        {
            get { return breakpointState; }  
            set { breakpointState = value;} 
        }

        protected bool isAtBreak;
        public    bool IsAtBreak 
        {
            get { return isAtBreak; }  
            set { isAtBreak = value;} 
        }

        protected ArrayList linkLabelChoices;
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxCodeNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>An iconic node representing a specific action</summary>
    public class MaxCodeNode: MaxActionNode
    {
        public MaxCodeNode(MaxCanvas canvas, MaxTool tool): base (canvas, tool) 
        {
            InitAction();
        }


        public MaxCodeNode(MaxCanvas canvas, MaxTool tool, long id): base (canvas, tool, id)  
        {       
            InitAction();
        } 


        /// <summary>Determine type of node representing this node for MaxPropMan </summary>
        protected override DataTypes.Type GetPropertiesType()
        {
            return Framework.Satellite.Property.DataTypes.Type.Code;
        }


        /// <summary>Checks that the custom user code has changed</summary>
        /// <remarks>Not Implemented</remarks>
        protected override void OnPropertiesChanged(MaxProperty[] properties)
        {
            foreach(MaxProperty x in properties)
            {
                CodeProperty property = x as CodeProperty;
                if  (property == null || !property.IsChanged) continue;

                switch(property.Name)
                {
                    case DataTypes.USER_CODE:
                        // TODO:  add code which monitors Code changing for this node
                        break;
                }  
            }      
        }
    }



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxEventNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>An iconic node representing a specific event</summary>
    public class MaxEventNode: MaxIconicNode
    {
        public MaxEventNode(MaxCanvas canvas, MaxTool tool): 
        base (NodeTypes.Event, canvas, tool) 
        {
            InitEvent();     
        }

        public MaxEventNode(MaxCanvas canvas, MaxTool tool, long id):
        base (NodeTypes.Event, canvas, tool, id)  
        {       
            InitEvent(); 
        } 

        private void InitEvent()
        {
            this.Label.Editable = false;   
            this.pmObjectType = Framework.Satellite.Property.DataTypes.Type.EventInstance;
            this.CreateProperties(this.Tool.PackageToolDefinition);
        }

        public override bool CanLinkTo()    { return false; }
     
        public override bool CanLinkFrom()  { return this.Port.LinksCount == 0; }  

        protected bool isProjectTrigger;
        public bool IsProjectTrigger 
        { 
            get { return isProjectTrigger; } 
            set { isProjectTrigger = value; }
        }
    }  
  


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxFunctionNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>An iconic node representing a specific user-defined function</summary>
    public class MaxFunctionNode: MaxIconicNode
    {
        public MaxFunctionNode(MaxCanvas canvas, MaxTool tool): 
        base (NodeTypes.Function, canvas, tool)
        {
            InitFunction();     
        }


        public MaxFunctionNode(NodeTypes type, MaxCanvas canvas, MaxTool tool, long id):
        base (NodeTypes.Function, canvas, tool, id)  
        {       
            InitFunction();
        } 


        private void InitFunction()
        {
            this.Label.Editable = true;
            string newname = this.MakeUniqueNodeName();
            this.NodeName  = this.Label.Text = newname;

            this.Label.AddObserver(this);   // Monitor label changes
  
            this.CreateProperties(this.Tool.PackageToolDefinition);
        }


        public string MakeUniqueNodeName()
        {
            string name = null;
            while(true)
            {       
                name = MakeNodeName();
                if  (this.Canvas.CanNameNode(NodeTypes.Function, name)) break;
            }

            return name;
        }


        public static string MakeNodeName()
        {
            return Const.defaultFunctionName + MaxProject.CurrentApp.FunctionSequence; 
        }


        public override bool OnDoubleClick(GoInputEventArgs evt, GoView view)
        {
            this.Canvas.FireTabEvent(new MaxTabEventArgs(this.NodeName, MaxCanvas.CanvasTypes.Function));
            return true;
        }


        /// <summary>Return indication as to whether this node can be deleted</summary>
        public override bool CanDelete()
        {
            // If the function node has a matching handler, we will inform user
            // that canvas will be deleted, and solicit confirmation

            if (MaxProject.CurrentApp.Canvases[this.NodeName] == null) return true;  

            DialogResult result = MessageBox.Show(Const.DeleteCanvasMessage(this.NodeName),
                Const.canvasDeleteDlgTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            return result == DialogResult.OK;
        }



        /// <summary>Catch node label text change</summary>
        protected override void OnObservedChanged(GoObject observed, int subhint, 
        int oI, object oldVal, RectangleF oR, int nI, object newVal, RectangleF nR)
        {
            if  (observed is GoText && newVal is string)
                this.OnNodeNameChanged(MaxCanvas.CanvasTypes.Function, NodeTypes.Function, 
                    observed as GoText, newVal as string, oldVal as string);

            base.OnObservedChanged(observed, subhint, oI, oldVal, oR, nI, newVal, nR);
        }


        /// <summary>Handle properties changed raised by grid</summary>
        protected override void OnPropertiesChanged(MaxProperty[] properties)
        {
            foreach(MaxProperty property in properties)
            {
                if  (property == null || !property.IsChanged) continue;

                switch(property.Name)
                {
                   case Const.PmFunctionName:
                        this.OnFunctionNamePropertyChanged(property);
                        break;
                }  
            }      
        }


        /// <summary>Handle function name change via property grid</summary>
        protected void OnFunctionNamePropertyChanged(MaxProperty property)
        {      
            string newname  = property.Value    as string;
            string oldname  = property.OldValue as string;
            if  (oldname == newname) return;
            MaxAppTree apptree = MaxManager.Instance.AppTree();
            bool ok = false;

            if  (!Utl.IsValidFunctionName(newname)           
                || !apptree.AppCanvas.CanNameNode(NodeTypes.Function, newname)) 
                Utl.ShowCannotRenameNodeDialog(true);
            else                                           
            if  (apptree.RenameFunction(null, oldname, newname) != null)
                ok = true;
       
            if (!ok) property.Value = oldname;     
        } 

        public enum CurrentActions { None, BenignRemove }  // 1009
        protected   CurrentActions currentAction;
        public      CurrentActions CurrentAction 
        { get { return currentAction; } set { currentAction = value; } }

    }  // class MaxFunctionNode



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxVariableNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>An iconic node representing a specific user-defined variable</summary>
    public class MaxVariableNode: MaxIconicNode
    {
        public MaxVariableNode(MaxCanvas canvas, MaxTool tool): 
        base (NodeTypes.Variable, canvas, tool) 
        {
            InitVariable(); 
        }

        public MaxVariableNode(MaxCanvas canvas, MaxTool tool, long id):
        base (NodeTypes.Variable, canvas, tool, id)  
        {       
            InitVariable();
        }

        private void InitVariable()
        {
            this.Label.Editable = true;
            string newname = this.MakeUniqueNodeName();
            this.NodeName  = this.Label.Text = newname;
         
            this.Port.IsValidFrom = this.Port.IsValidTo = false;

            this.Label.AddObserver(this);         // Monitor label changes  

            this.CreateProperties(this.Tool.PackageToolDefinition);
        } 

        public string MakeUniqueNodeName()
        {
            string name = null;
            while(true)
            {       
                name = MakeNodeName();
                if  (this.Canvas.CanNameNode(NodeTypes.Variable, name)) break;
            }

            return name;
        }


        public static string MakeNodeName()
        {
            return Const.defaultVariableName + MaxProject.CurrentApp.VariableSequence; 
        }


        /// <summary>Catch node label text change</summary>
        protected override void OnObservedChanged(GoObject observed, int subhint, 
        int oI, object oldVal, RectangleF oR, int nI, object newVal, RectangleF nR)
        {
            if  (observed is GoText)
                this.OnNodeNameChanged(MaxCanvas.CanvasTypes.None, NodeTypes.Variable, 
                    observed as GoText, newVal as string, oldVal as string);

            base.OnObservedChanged(observed, subhint, oI, oldVal, oR, nI, newVal, nR);
        }


        /// <summary>Notify framework of node name change</summary>
        public override void FireNodeNameChangeEvent
            ( MaxCanvas.CanvasTypes canvasType, string oldname, string newname)
        {
            MaxLocalNodeEventArgs args = new MaxLocalNodeEventArgs(oldname, newname, this);

            this.Canvas.FireNodeEvent(args); // Notify framework
        }


        /// <summary>Handle properties changed raised by grid</summary>
        protected override void OnPropertiesChanged(MaxProperty[] properties)
        {
            foreach(MaxProperty property in properties)
            {
                if  (property == null || !property.IsChanged) continue;

                switch(property.Name)
                {
                   case Const.PmVariableName:
                        this.OnVariableNamePropertyChanged(property);
                        break;
                }  
            }      
        }


        /// <summary>Handle variable name change via property grid</summary>
        protected void OnVariableNamePropertyChanged(MaxProperty property)
        {      
      
        } 

    }  // class MaxVariableNode



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxStartNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>An iconic node representing a function entry point</summary>
    public class MaxStartNode: MaxIconicNode
    {
        public MaxStartNode(MaxCanvas canvas, MaxTool tool): base(NodeTypes.Start, canvas, tool) 
        {
            InitStart();     
        }

        public MaxStartNode(NodeTypes type, MaxCanvas canvas, MaxTool tool, long id):
        base (NodeTypes.Start, canvas, tool, id)  
        {       
            InitStart();
        } 

        private void InitStart()
        {
            this.Deletable = this.Copyable = false;

            // this.SkipsUndoManager = true;  // removed 20060828

            this.CreateProperties(this.Tool.PackageToolDefinition);
        } 

        public override bool CanLinkFrom() { return this.Port.CanLinkFrom(); }
        public override bool CanLinkTo()   { return false; }

        protected override void OnBoundsChanged(RectangleF oldBounds)
        {
            if (this.Annotation != null)
                MaxIconicNode.MoveAnnotationRelative(oldBounds, this.Bounds, this.Annotation);

            base.OnBoundsChanged (oldBounds);
        }

        protected override GoPort CreatePort()
        {
            return new CustomPort(this);  
        }

        /// <summary>Override node's port so we can hook some methods</summary>
        protected class CustomPort: GoPort
        {
            public CustomPort(MaxStartNode parent)
            {
                this.parent = parent; 
                this.Brush  = null;
                this.Pen    = null; 
                this.Size   = Const.portSizeNormal;  
                this.FromSpot   = this.ToSpot = NoSpot;
                this.PortObject = parent;
                this.IsValidDuplicateLinks = this.IsValidSelfNode = false;

                if (Config.VisiblePorts)   
                {
                    #if(false) // we now show ports only on mouse over
                    this.Style = GoPortStyle.Ellipse;
                    this.Brush = Const.portBrush;
                    this.Pen   = Const.portPen;
                    #endif
                }
                else this.Style = GoPortStyle.None;
         
                this.Size = Config.LargePorts? Const.portSizeLarge: Const.portSizeNormal;  
            }

    
            /// <summary>On link change, rotate node's image to match link direction</summary>
            public override void OnLinkChanged
            (IGoLink l, int subhint, int oi, object ov, RectangleF or, int ni, object nv, RectangleF nr)
            {
                base.OnLinkChanged(l, subhint, oi, ov, or, ni, nv, nr);
                if (subhint != GoStroke.ChangedAllPoints) return;                          

                CompassDir linkDirection = CompassDir.East;
                PointF linkEndPoint = this.GetFromLinkPoint(Utl.FirstLink(this));

                int x    = Utl.ftoi(linkEndPoint.X),  y = Utl.ftoi(linkEndPoint.Y);
                int top  = Utl.ftoi(parent.Top), bottom = Utl.ftoi(parent.Bottom);
                int left = Utl.ftoi(parent.Left), right = Utl.ftoi(parent.Right);
                int horzMiddle = left + 16;

                if  (x >= right && y > top)  linkDirection = CompassDir.East;
                else
                if  (x == horzMiddle && y >= bottom) linkDirection = CompassDir.South;
                else
                if  (x <= left && y > top)   linkDirection = CompassDir.West;
                else 
                if  (x == horzMiddle && y <= top) linkDirection = CompassDir.North; 

                parent.Image.Index = MaxImageIndex.stockTool32x32IndexStartE + (int)linkDirection;
            }

            public override bool CanLinkFrom() { return this.LinksCount == 0; }
            public override bool CanLinkTo()   { return false; }

            protected MaxStartNode parent;
            protected enum CompassDir { East, South, West, North };
        }
    }  


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxCommentNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>A user-editable comment node</summary>
    public class MaxCommentNode: GoComment, IMaxNode
    {
        private NodeTypes nodeType;
        private string    nodeName;
        private string    fullName;
        private MaxCanvas canvas;
        private MaxTool   tool;   
        private long      nodeID;
        private PropertyDescriptorCollection properties;
        private Framework.Satellite.Property.DataTypes.Type pmObjectType;

        /// <summary>Create a new comment node</summary>
        public MaxCommentNode(MaxCanvas canvas, MaxTool tool, string text)
        {
            this.nodeID = Const.Instance.NextNodeID; 
            this.Init(canvas, tool, text);   
        }

        /// <summary>Recreate a comment node</summary>
        public MaxCommentNode(MaxCanvas canvas, MaxTool tool, string text, long id) 
        {       
            this.nodeID = id;      
            this.Init(canvas, tool, text);
        }

        private void Init(MaxCanvas canvas, MaxTool tool, string text)
        {
            this.tool       = tool;  
            this.canvas     = canvas;
            if (this.tool  == null || this.canvas == null) throw new ArgumentNullException();
            this.nodeName   = null;
            this.nodeType   = NodeTypes.Comment;
            GoText label    = this.Label;
            label.Font      = SystemInformation.MenuFont;   
            label.FontSize  = 8.5F;  
            this.Label.Text = text; 
            this.menu = new MaxNodeMenu(this);

            this.pmObjectType = tool.PmObjectType; 
            this.CreateProperties(this.Tool.PackageToolDefinition);
        }


        /// <summary>Set text property on change of comment text</summary>
        public override void Changed
        ( int subhint, int oI, object ov, RectangleF or, int nI, object nv, RectangleF nr) 
        {   
            switch(subhint)
            {
               case GoText.ChangedText: 
                    Utl.SetProperty(this.MaxProperties, Const.PmCommentText, nv as string);       
                    break;
            } 
              
            base.Changed(subhint, oI, ov, or, nI, nv, nr);
        }


        /// <summary>Handle properties changed raised by grid</summary>
        protected void OnPropertiesChanged(MaxProperty[] properties)
        {
            foreach(MaxProperty property in properties)
            {
                if  (property == null || !property.IsChanged) continue;

                switch(property.Name)
                {
                   case Const.PmCommentText:
                        this.Text = property.Value as string;
                        break;
                }  
            }      
        }


        /// <summary>Actions on node gaining keyboard focus</summary>
        public override void OnGotSelection(GoSelection selection)   
        {
            PmProxy.ShowProperties(this, this.PmObjectType); 
            base.OnGotSelection (selection);
        }


        public override bool OnContextClick(GoInputEventArgs evt, GoView view)
        {
            menu.PopContextMenu(this);
            return true;
        }


        private static readonly float minX = Config.minNodeXf - Const.CommentOffsetX;
        private static readonly float minY = Config.minNodeYf - Const.CommentOffsetY;


        /// <summary>constrain node to positive coordinates</summary>
        public override PointF ComputeMove(PointF origLoc, PointF newLoc)
        {
            return this.canvas.View.ComputeMove(this, origLoc, newLoc, minX, minY); 
        }

        protected MaxNodeMenu menu;
        private long container;
        public  long Container { get { return container; } set { container = value; } }
        public  MaxAnnotationNode Annotation { get { return null; } set { } }
        public virtual bool CanAnnotate()    { return false; }

        #region IMaxNode Members

        public string     NodeName  { get { return nodeName;  } set { nodeName = value; } }
        public string     FullName  { get { return fullName;  } set { fullName = value; } }
        public long       NodeID    { get { return nodeID;    } set { nodeID = value;   } }
        public PointF     NodeLoc   { get { return Location;  } set { Location = value; } }
        public RectangleF NodeBounds{ get { return Bounds;    } }
        public NodeTypes  NodeType  { get { return nodeType;  } }
        public IGoPort    NodePort  { get { return null;      } }
        public MaxCanvas  Canvas    { get { return canvas;    } }
        public MaxTool    Tool      { get { return tool;      } }
        public string     GroupName 
        {
            get { return tool.ToolGroup == null? null: tool.ToolGroup.GroupName; } 
        } 

        public virtual void OnLinkCreated(IMaxLink link) { }
     
        public virtual void OnLinkDeleted(IMaxLink link) { }  

        public virtual void CanDeleteLink(object sender, CancelEventArgs e)  { }

        public virtual void ShowPort(bool isShowing) { }

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

        public void OnPropertiesChangeRaised(MaxProperty[] props) { this.OnPropertiesChanged(props); }  

        #endregion

        #region MaxObject Members

        public Metreos.Max.Core.ObjectTypes MaxObjectType { get { return ObjectTypes.Node; } }

        public string ObjectDisplayName { get { return Const.CommentObjectDisplayName; } }
   
        public void MaxSerialize(XmlTextWriter writer)
        {
            MaxNodeSerializer serializer = MaxNodeSerializer.Instance;
            writer.WriteStartElement(Const.xmlEltNode); // <node>

            serializer.WriteCommonAttibutesA(this, writer);

            serializer.WriteCommentNodeSpecificAttributes(this, writer);

            serializer.WriteCommonAttibutesB(this, writer);

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

    } // class MaxCommentNode



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxLabelNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>A "GoTo" label node</summary>
    public class MaxLabelNode: GoBasicNode, IMaxNode
    {
        private NodeTypes nodeType;
        private string    nodeName;
        private string    fullName;
        private MaxCanvas canvas;
        private MaxTool   tool;   
        private long      nodeID;
        private MaxAnnotationNode annotation = null;
        public  MaxAnnotationNode Annotation 
        { get { return annotation; } set { annotation = value; } 
        }
        public  virtual bool CanAnnotate() { return annotation == null; }
        private PropertyDescriptorCollection properties;
        private Framework.Satellite.Property.DataTypes.Type pmObjectType;

        public MaxLabelNode(MaxCanvas canvas)
        {
            this.nodeType = NodeTypes.Label;
            this.nodeName = "Label" + ++nodeno;
            this.canvas   = canvas;
            this.tool     = MaxStockTools.Instance.LabelTool;
            this.nodeID   = Const.Instance.NextNodeID; 
      
            this.Label = new CustomLabel(this, "?");
            this.Brush = new SolidBrush(Color.FloralWhite);   
            this.Pen   = new Pen(Color.LightSlateGray, 1); 
            this.Size  = new SizeF(19F, 19F); 
            this.menu  = new MaxNodeMenu(this);
            this.Label.Editable   = true;
            this.Label.Selectable = false;
            this.LabelSpot = GoObject.Middle;

            this.pmObjectType = tool.PmObjectType; 
            this.CreateProperties(null);
        }

        protected override GoPort CreatePort()
        {
            return new CustomPort(this);  
        }


        protected override void OnBoundsChanged(RectangleF oldBounds)
        {
            if (this.Annotation != null)
                MaxIconicNode.MoveAnnotationRelative(oldBounds, this.Bounds, this.Annotation);

            base.OnBoundsChanged (oldBounds);
        }


        private class CustomLabel: GoText
        {
            public CustomLabel(MaxLabelNode parent, string text) 
            {
                this.parent = parent;   
                this.Font   = Config.canvasFont;  
                this.FontSize  = Config.canvasFontSize;
                this.TextColor = Config.canvasTextColor;
                this.Bold   = true;
                this.Text   = text;
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


            public virtual void OnTextChanged(string oldtext, string newtext)
            {
                if (this.undoingChange)
                    this.undoingChange = false;    // Intercept recursive call
                else
                if (this.Text.Length > 1)
                {        
                    this.undoingChange = true;     
                    this.Text = this.Text.Substring(0,1);
                }     
                else 
                if (this.Text.Length == 0)
                {
                    this.undoingChange = true; 
                    this.Text = "?";
                } 

                Utl.SetProperty(parent.MaxProperties, Const.PmLabelName, this.Text);      
            } 

            protected bool undoingChange;
            private MaxLabelNode parent;
        }


        private class CustomPort: GoPort
        {
            public CustomPort(MaxLabelNode parent)
            {
                this.parent = parent;  
                this.Style  = GoPortStyle.None;
                this.Size   = Const.portSizeNormal;  
                this.FromSpot = this.ToSpot = NoSpot;
                this.PortObject = parent;
                this.IsValidDuplicateLinks  = this.IsValidSelfNode = false;
            }

            public override bool CanLinkFrom() { return this.LinksCount == 0; }
            public override bool CanLinkTo()   { return this.LinksCount == 0; }
            private MaxLabelNode parent;
        }


        public override bool OnMouseOver(GoInputEventArgs evt, GoView view)
        {           
            // Start annotation delay timer if node is annotated
            MaxCanvas.annotationState.OnMouseOverNode(this); 

            return false;
        }


        /// <summary>Handle properties changed raised by grid</summary>
        protected void OnPropertiesChanged(MaxProperty[] properties)
        {
            foreach(MaxProperty property in properties)
            {
                if  (property == null || !property.IsChanged) continue;

                switch(property.Name)
                {
                   case Const.PmLabelName:
                        this.Text = property.Value as string; // fires text changed
                        break;
                }  
            }      
        }


        /// <summary>Actions on node gaining keyboard focus</summary>
        public override void OnGotSelection(GoSelection selection)   
        {
            PmProxy.ShowProperties(this, this.PmObjectType); 
            base.OnGotSelection (selection);
        }


        public override bool OnContextClick(GoInputEventArgs evt, GoView view)
        {
            menu.PopContextMenu(this);
            return true;
        }


        /// <summary>constrain nodes to positive coordinates</summary>
        public override PointF ComputeMove(PointF origLoc, PointF newLoc)
        {
            return this.canvas.View.ComputeMove(this, origLoc, newLoc); 
        }


        private static int nodeno = 0;
        protected MaxNodeMenu menu;
        private long container;
        public  long Container { get { return container; } set { container = value; } }

        #region MaxObject Members

        public Metreos.Max.Core.ObjectTypes MaxObjectType { get { return ObjectTypes.Node; } }

        public string ObjectDisplayName { get { return Const.LabelObjectDisplayName; } }
    
        public void MaxSerialize(XmlTextWriter writer)  
        {
            writer.WriteStartElement(Const.xmlEltNode); // <node>
            MaxNodeSerializer serializer = MaxNodeSerializer.Instance;

            serializer.WriteCommonAttibutesA(this, writer);

            serializer.WriteLabelNodeSpecificAttributes(this, writer);

            serializer.WriteCommonAttibutesB(this, writer);

            serializer.WriteAnnotation(this, writer);

            serializer.WriteLinks(this, this.Port, writer);

            writer.WriteEndElement(); // </node>
        }
   
        #endregion

        #region IMaxNode Members

        public string    NodeName   { get { return nodeName;  } set { nodeName = value; } }
        public string    FullName   { get { return fullName;  } set { fullName = value; } }
        public long      NodeID     { get { return nodeID;    } set { nodeID = value;   } }
        public PointF    NodeLoc    { get { return Location;  } set { Location = value; } }
        public RectangleF NodeBounds{ get { return Bounds;    } }
        public NodeTypes NodeType   { get { return nodeType;  } }
        public IGoPort   NodePort   { get { return this.Port; } }
        public MaxCanvas Canvas     { get { return canvas;    } }
        public MaxTool   Tool       { get { return tool;      } }
        public string    GroupName 
        {
            get { return tool.ToolGroup == null? null: tool.ToolGroup.GroupName; } 
        } 

        public virtual void OnLinkCreated(IMaxLink link) { }  
        public virtual void OnLinkDeleted(IMaxLink link) { link.Canvas.OnPropertiesFocus(null); }  
        public virtual void CanDeleteLink(object sender, CancelEventArgs e)  { }
        public virtual void ShowPort(bool isShowing) { }

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

        public Framework.Satellite.Property.DataTypes.Type PmObjectType { get { return pmObjectType; } }

        public void OnPropertiesChangeRaised(MaxProperty[] props) { this.OnPropertiesChanged(props); }  

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
    }



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxTriggerTargetNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>A drop target for the application trigger</summary>
    public class MaxTriggerTargetNode: MaxPictureNode
    {
        private static readonly string imageName = "triggering.bmp";
        private static readonly Size   imageSize = new Size(120,120);
        public  static readonly Point  DropPoint = new Point(60,60);

        public MaxTriggerTargetNode(): base(imageName, imageSize, null) 
        {
            this.Port.IsValidFrom = this.Port.IsValidTo = false;
            this.Selectable = false;
        }
    }  
 
} // namespace
 
