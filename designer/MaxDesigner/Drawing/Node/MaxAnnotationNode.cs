//
// MaxAnnotationNode
//
using System;
using System.Xml;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Framework;
using Metreos.Max.Drawing;
using Metreos.Max.Manager;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework.Satellite.Property;
using PropertyGrid.Core;                                                                                
using Northwoods.Go;



namespace Metreos.Max.Drawing
{
	/// <summary></summary>
	public class MaxAnnotationNode: GoBalloon, IMaxNode 
	{
        private NodeTypes nodeType;
        private string    nodeName;
        private string    fullName;
        private MaxCanvas canvas;
        private MaxTool   tool;   
        private long      nodeID;
        private IMaxNode  parent;
        public  IMaxNode  AnnotationHostNode { get { return parent; } }
        public  MaxAnnotationNode Annotation { get { return null; } set { } }
        public  virtual bool CanAnnotate()   { return false; }

        // vate static Color ColorCafeAuLait = Color.FromArgb(243,239,222);
        // vate static Color ColorPowderBlue = Color.FromArgb(246,251,255);
        // vate static SolidBrush bkgndbrush = new SolidBrush(ColorPowderBlue);
        private static SolidBrush bkgndbrush = new SolidBrush(Color.PapayaWhip);       

        private PropertyDescriptorCollection properties;
        private Metreos.Max.Framework.Satellite.Property.DataTypes.Type pmObjectType;
       

        /// <summary>Create a new annotation node</summary>       
        public MaxAnnotationNode(MaxCanvas canvas, MaxTool tool, string text, long id, IMaxNode parent) 
        {   
            // If node ID is passed nonzero, we are recreating (deserializing) a previous annotation
            this.nodeID = id > 0? id: Const.Instance.NextNodeID;   
            this.Init(canvas, tool, parent, text);
        }


        public static MaxAnnotationNode CreateAnnotation
        ( MaxCanvas canvas, MaxTool tool, string text, long id, IMaxNode parent)
        {
            MaxAnnotationNode node = new MaxAnnotationNode(canvas, tool, text, id, parent);
            MaxCanvas.annotationState.OnNewAnnotation(parent);
            return node;
        }

 
        /// <summary>Common construction initialization</summary>
        private void Init(MaxCanvas canvas, MaxTool tool, IMaxNode parent, string text)
        {
            this.isInitialized = false;
            this.tool      = tool;  
            this.canvas    = canvas;
            if (this.tool == null || this.canvas == null) throw new ArgumentNullException();
            this.nodeName  = null;
            this.nodeType  = NodeTypes.Annotation;

            CustomLabel label = new CustomLabel(this);
            label.Font     = SystemInformation.MenuFont;   
            label.FontSize = 8.5F;             
            label.Text     = text;
            this.Label = label;
            this.Label.AddObserver(this); 
             
            this.menu = new MaxNodeMenu(this); 

           (this.Background as GoPolygon).Brush = bkgndbrush;  

            this.parent = parent;   
            this.container = parent.Container;     

            this.SetBalloonAnchor(parent);

            if (parent != null)
            {   
                parent.Annotation = this;

                // Move the annotation outside the parent node in order to show some of the
                // balloon pointy part. Place it below and to the right, since these are the
                // directions in which the canvas expands.
                PointF pt = parent.NodeLoc;
                pt.X += MaxAnnotationNode.initialXoffset;
                pt.Y += MaxAnnotationNode.initialYoffset;

                if (parent is MaxIconicMultiTextNode)
                {
                    // Location is at different relative spot for multitext node types
                    pt.X += MaxAnnotationNode.additionalXoffset;
                    pt.Y += MaxAnnotationNode.additionalYoffset;
                }

                this.Location = pt;
            }

            this.pmObjectType = tool.PmObjectType; 
            // this.CreateProperties(this.Tool.PackageToolDefinition);
            this.isInitialized = true;
        } 


        public void Show(bool isShowing)
        {
            this.Visible = isShowing;
        }


        public void Show()
        {
            this.Visible = true;
        }


        public void Hide()
        {
            this.Visible = false;
        }

        
        public override bool Visible     
        {
            get
            {
                return base.Visible;
            }
            set
            {   // If we are undo/redo-ing we hide the recreated annotation  
                GoUndoManager undoManager = this.canvas.View.Document.UndoManager;
                bool isUndoRedo = undoManager.IsRedoing || undoManager.IsUndoing; 
                bool isVisible  = isUndoRedo? false: value;                           
                base.Visible = isVisible;
            }
        }             


        /// <summary>Set object to which balloon points to</summary>
        public void SetBalloonAnchor(IMaxNode parent) 
        {
            GoObject anchor = parent as GoObject;

            // A node can have a large bounds rect, since it includes everything including text.
            // We don't want our annotation balloon pointing at blue sky, so tell it to point
            // into just the icon portion of the parent node.

            if  (parent is MaxIconicNode)
                 anchor = (parent as MaxIconicNode).Icon;
            else
            if  (parent is MaxIconicMultiTextNode)
                 anchor = (parent as MaxIconicMultiTextNode).Pnode.Icon;

            this.Anchor = anchor;
        } 


        public override bool OnMouseOver(GoInputEventArgs evt, GoView view)
        {
            MaxCanvas.annotationState.OnMouseOverAnnotation(this);
            return base.OnMouseOver (evt, view);
        }
      
        
        /// <summary>Handle properties changed raised by grid</summary>
        protected void OnPropertiesChanged(MaxProperty[] properties)
        {
  
        }

      
        #region OnGotSelection
        #if(false)
        /// <summary>Actions on node gaining keyboard focus</summary>
        public override void OnGotSelection(GoSelection selection)   
        {
            // PmProxy.ShowProperties(this, this.PmObjectType); 
            base.OnGotSelection (selection);
           
            // Do we want to *remove* the annotation from the selection here?
            // No, because we need to be selected to drag and start edit,
            // we simply do not want to *appear* selected. 
        }
        #endif
        #endregion


        /// <summary>Ensure no focus rect is drawn when we are selected</summary>                 
        public override GoObject SelectionObject { get { return null; } }        


        /// <summary>Open a text box to edit the annotation text</summary>
        public void OpenEditSession()
        {
            this.Label.DoBeginEdit(this.parent.Canvas.View);
        }


        // Pop a context menu for the annotation itself
        public override bool OnContextClick(GoInputEventArgs evt, GoView view)
        {
            menu.PopContextMenu(this);    
            return true;
        }

                                                              
        /// <summary>Catch annotation text change</summary>   
        protected override void OnObservedChanged(GoObject observed, int subhint, 
            int oI, object oldVal, RectangleF oR, int nI, object newVal, RectangleF nR)
        {
            if (observed is GoText && newVal is string)
                this.OnAnnotationTextChanged(observed as GoText, newVal as string, oldVal as string);
            
            base.OnObservedChanged(observed, subhint, oI, oldVal, oR, nI, newVal, nR);
        }


        /// <summary>Monitor changes to annotation text</summary>
        protected void OnAnnotationTextChanged(GoText label, string newtext, string oldtext)
        {   
           if (isInitialized && newtext.TrimEnd() == String.Empty)
               MaxCanvas.annotationState.DeleteAnnotation(this.parent);                 
        }

        
        private static readonly float minX = Config.minNodeXf - Const.CommentOffsetX;
        private static readonly float minY = Config.minNodeYf - Const.CommentOffsetY;

        private const int initialXoffset = 80;
        private const int initialYoffset = 24;
        private const int additionalXoffset = 56;  // For multitext nodes
        private const int additionalYoffset = 16;   


        /// <summary>constrain node to positive coordinates</summary>
        public override PointF ComputeMove(PointF origLoc, PointF newLoc)
        {
            return this.canvas.View.ComputeMove(this, origLoc, newLoc, minX, minY); 
        }

        #region CustomLabel   
                   
        /// <summary>Override annotation text object to hook some of its methods</summary>           
        public class CustomLabel: GoText     
        {
            public CustomLabel(MaxAnnotationNode parent) 
            { 
                this.Editable = this.Multiline = true; 
                this.parent = parent;
                this.parentParent = parent.AnnotationHostNode;
            }

            private MaxAnnotationNode parent;
            private IMaxNode parentParent;
           
            /// <summary>Actions on edit session started</summary>
            public override GoControl CreateEditor(GoView view)
            {
                MaxCanvas.annotationState.BeginEdit(this.parentParent);
                return base.CreateEditor (view);
            }


            /// <summary>Actions on edit session ended</summary>
            public override string ComputeEdit(string oldtext, string newtext)
            {
                MaxCanvas.annotationState.EndEdit(this.parentParent);
                return base.ComputeEdit (oldtext, newtext);
            }  

            /// <summary>Set no selection object (so no focus rect)</summary>
            public override GoObject SelectionObject { get { return null; } }
        }
        
        #endregion  // CustomLabel

        private MaxNodeMenu menu;
        private bool isInitialized;
        private long container;
        public  long Container { get { return container; } set { container = value; } }

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
            // Lose the body of this method since we never have properties
            MaxPropertiesManager propertiesManager = PmProxy.PropertiesManager;

            CreatePropertiesArgs args = new
            CreatePropertiesArgs(this, pe, this.PmObjectType);

            this.properties = propertiesManager.ConstructProperties(args);
            return this.properties;
        } 

        public Metreos.Max.Framework.Satellite.Property.DataTypes.Type PmObjectType { get { return pmObjectType; } }

        // Lose the body here since we never have properties
        public void OnPropertiesChangeRaised(MaxProperty[] props) { this.OnPropertiesChanged(props); }  

        #endregion

        #region MaxObject Members

        public Metreos.Max.Core.ObjectTypes MaxObjectType { get { return ObjectTypes.Node; } }

        public string ObjectDisplayName { get { return Const.AnnotObjectDisplayName; } }

        public void MaxSerialize(XmlTextWriter writer)
        {
            // The annotation is serialized as part of the node, not separately, so we do nothing here
            // MaxNodeSerializer serializer = MaxNodeSerializer.Instance;
            // writer.WriteStartElement(Const.xmlEltNode); // <node>

            // serializer.WriteCommonAttibutesA(this, writer);

            // serializer.WriteAnnotationNodeSpecificAttributes(this, writer);  

            // serializer.WriteCommonAttibutesB(this, writer);

            // writer.WriteEndElement(); // </node>
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
	} // class MaxAnnotationNode
}   // namespace
