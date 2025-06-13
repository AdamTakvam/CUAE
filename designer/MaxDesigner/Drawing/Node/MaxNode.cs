using System;
using System.Xml;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using Metreos.Max.Core;
using Metreos.Max.Manager;
using Metreos.Max.Core.Tool;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework.Satellite.Property;
using Northwoods.Go;



namespace Metreos.Max.Drawing
{
    public enum NodeTypes
    { 
        None, Function, Action, Event, Comment, Annotation, Variable, Label, Start, Loop, Group 
    }

    public enum BreakpointStates { Off, Disabled, Set }


    // - - - - - - - -- - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // IMaxNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>Implemented by all graph node types hosted by MaxCanvas</summary>
    public interface IMaxNode: MaxSelectableObject
    {
        string     NodeName   { get; set; } 
        string     FullName   { get; set; }
        long       NodeID     { get; set; } 
        long       Container  { get; set; }  
        PointF     NodeLoc    { get; set; } 
        RectangleF NodeBounds { get; }         
        NodeTypes  NodeType   { get; }
        IGoPort    NodePort   { get; }
        string     GroupName  { get; }
        MaxCanvas  Canvas     { get; }
        MaxTool    Tool       { get; }
        MaxAnnotationNode Annotation { get; set; }   
        void       OnLinkCreated(IMaxLink link);
        void       OnLinkDeleted(IMaxLink link);
        void       CanDeleteLink(object sender, CancelEventArgs e);
        bool       CanAnnotate();
        void       ShowPort(bool isShowing);
    }

    /// <summary>Implemented by complex graph node types hosted by MaxCanvas</summary>
    public interface IComplexMaxNode: IMaxNode
    {
        IEnumerable MaxNodes { get; }
    }


    /// <summary>Implemented by all action node types hosted by MaxCanvas</summary>
    public interface IMaxActionNode: IMaxNode
    {
        BreakpointStates BreakpointState  { get; set; }
        bool IsAtBreak                    { get; set; }
        void ToggleBreakpoint();
        void ToggleBreakpointEnabled();
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxIconicNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxIconicNode: GoIconicNode, IMaxNode
    {
        private NodeTypes nodeType;
        private string    nodeName;
        private string    fullName;
        private MaxCanvas canvas;
        private MaxTool   tool;
        private long      nodeID;
        private PropertyDescriptorCollection properties;
        public  Framework.Satellite.Property.DataTypes.Type pmObjectType;


        public MaxIconicNode(NodeTypes type, MaxCanvas canvas, MaxTool tool) 
        {
            this.nodeID = Const.Instance.NextNodeID; 
 
            if (type != NodeTypes.None)  
                this.Init(type, canvas, tool); 
        }


        public MaxIconicNode
        (NodeTypes type, MaxCanvas canvas, MaxTool tool, long ID) 
        {       
            this.nodeID = ID;

            if (type != NodeTypes.None)  
                this.Init(type, canvas, tool);
        }


        protected void Init(NodeTypes type, MaxCanvas canvas, MaxTool tool)
        {
            this.LocalInit(type, canvas, tool);
            // GoIconicNode initialization
            this.Initialize(tool.ImagesLg.Imagelist, tool.ImageIndexLg, tool.DisplayName); 
        }


        protected void LocalInit(NodeTypes type, MaxCanvas canvas, MaxTool tool)
        {
            this.nodeType = type; this.canvas = canvas; this.tool = tool;
            if (this.tool == null || this.canvas == null) throw new ArgumentNullException();                                           

            this.nodeName = tool.Name == null? 
                Const.defaultNodeName: Utl.StripQualifiers(tool.Name);
            this.pmObjectType = tool.PmObjectType;// May override this later

            this.Resizable = false;

            this.menu = new MaxNodeMenu(this);
        }


        /// <summary>Intercept normal port creation</summary>
        protected override GoPort CreatePort()
        { 
            return new CustomPort(this); 
        }


        #if(false)
        /// <summary>Show entire node selected, to match multitext node behavior</summary>
        public override GoObject SelectionObject { get { return this; } }
        #endif


        /// <summary>Actions on node gaining keyboard focus</summary>
        public override void OnGotSelection(GoSelection selection)   
        {
            PmProxy.ShowProperties(this, this.PmObjectType); 
            base.OnGotSelection (selection);
        }


        /// <summary>Actions on node losing keyboard focus</summary>
        public override void OnLostSelection(GoSelection selection)
        {
            base.OnLostSelection(selection);
        }


        /// <summary>Rename node and notify framework of name change</summary>
        protected void OnNodeNameChanged(MaxCanvas.CanvasTypes canvasType, 
        NodeTypes nodeType, GoText label, string newname, string oldname)
        {   
            // Invoked as user changes name of function or variable node in
            // place. We check that the name is not already assigned, and
            // if OK, notify framework to reflect change. 

            IMaxNode node = label.Parent as IMaxNode;
            if  (node == null || newname == null || oldname == null) return;

            if (this.isRevertingToPreviousName)   // Intercept recursive call
            {
                this.isRevertingToPreviousName = false;
                return;
            }

            if (!this.Canvas.CanNameNode(nodeType, newname))
            {
                Utl.ShowCannotRenameNodeDialog(false);
                this.isRevertingToPreviousName = true;
                label.Text = oldname;               // Results in recursion
                return;
            }         
             
            node.NodeName = newname;  

            this.FireNodeNameChangeEvent(canvasType, oldname, newname);
        }


        /// <summary>Notify framework of node name change</summary>
        public virtual void FireNodeNameChangeEvent
        ( MaxCanvas.CanvasTypes canvasType, string oldname, string newname)
        {
            MaxTabEventArgs args = new MaxTabEventArgs(newname, oldname, this.NodeID, canvasType); 

            this.Canvas.FireTabEvent(args);        
        }


        public override bool OnContextClick(GoInputEventArgs evt, GoView view)
        {
            menu.PopContextMenu(this);
            return true;
        }


        public virtual bool CanLinkFrom()  { return true; }

        public virtual bool CanLinkTo()    { return true; }


        /// <summary>constrain nodes to positive coordinates</summary>
        public override PointF ComputeMove(PointF origLoc, PointF newLoc)
        {
            return this.canvas.View.ComputeMove(this, origLoc, newLoc); 
        }


        /// <summary>Move annotation relative to its parent</summary>
        public static void MoveAnnotationRelative
        ( RectangleF oldBounds, RectangleF newbounds, MaxAnnotationNode annotation)
        {          
            if (annotation == null) return;
            float xdiff = newbounds.X - oldBounds.X;
            float ydiff = newbounds.Y - oldBounds.Y;
            RectangleF annBounds = annotation.Bounds;
            annBounds.X += xdiff;
            annBounds.Y += ydiff;
            annotation.Bounds = annBounds;                
        }


        protected override void OnBoundsChanged(RectangleF oldBounds)
        {
            if (this.Annotation != null)
                MaxIconicNode.MoveAnnotationRelative(oldBounds, this.Bounds, this.Annotation);

            base.OnBoundsChanged (oldBounds);
        }


        protected virtual void OnPropertiesChanged(MaxProperty[] properties) { }


        /// <summary>Show port on mouse over. Port is unshown in MaxFunctionCanvas mouse over</summary>       
        public override bool OnMouseOver(GoInputEventArgs evt, GoView view)
        {
            if (Config.VisiblePorts)
                this.ShowPort(true);

            // Start annotation delay timer if node is annotated
            MaxCanvas.annotationState.OnMouseOverNode(this); 

            return false;
        }


        /// <summary>Port override to monitor linking behavior</summary>
        private class CustomPort: GoPort
        {
            public CustomPort(MaxIconicNode parent)  
            { 
                this.parent = parent; 
                this.Brush  = null;
                this.Pen    = null; 

                if (Config.VisiblePorts && !(parent is MaxRecumbentVariableNode))   
                {
                    #if(false) // we now show ports on mouse over
                    this.Style = GoPortStyle.Ellipse;
                    this.Brush = Const.portBrush;
                    this.Pen   = Const.portPen;
                    #endif
                }
                else this.Style = GoPortStyle.None;
         
                this.Size = Config.LargePorts? Const.portSizeLarge: Const.portSizeNormal;  

                this.FromSpot = this.ToSpot = NoSpot;
                this.PortObject = parent;
                this.IsValidSelfNode = false;                                             
            }

            public override bool CanLinkFrom() { return parent.CanLinkFrom(); }  
            public override bool CanLinkTo()   { return parent.CanLinkTo();   }  
            private MaxIconicNode parent;
        }
      
        protected MaxNodeMenu menu;
        private IMaxLink deletedLink;
        private bool isRevertingToPreviousName;
        private long container;
        private MaxAnnotationNode annotation = null;
        public  long Container               { get { return container;  } set { container = value;  } }
        public  MaxAnnotationNode Annotation { get { return annotation; } set { annotation = value; } }

        #region IMaxNode Members

        public string     NodeName  { get { return nodeName;  } set { nodeName = value; } }
        public string     FullName  { get { return fullName;  } set { fullName = value; } }
        public long       NodeID    { get { return nodeID;    } set { nodeID   = value; } }
        public PointF     NodeLoc   { get { return Location;  } set { Location = value; } }
        public RectangleF NodeBounds{ get { return Bounds;    } }
        public NodeTypes  NodeType  { get { return nodeType;  } }
        public IGoPort    NodePort  { get { return this.Port; } }
        public MaxCanvas  Canvas    { get { return canvas;    } }
        public MaxTool    Tool      { get { return tool;      } }

        public string GroupName 
        {
            get { return tool.ToolGroup == null? null: tool.ToolGroup.GroupName; } 
        }   

        public virtual void OnLinkCreated(IMaxLink link) { }
     
        public virtual void OnLinkDeleted(IMaxLink link) 
        {                                   // Link is deleting itself, so
            this.deletedLink = link;        // we stage the deletion
            if (link.Canvas != null) link.Canvas.OnPropertiesFocus(null);
            this.Document.LinksLayer.Remove(link as GoObject);
        }

        public virtual void CanDeleteLink(object sender, CancelEventArgs e)  { }

        public virtual bool CanAnnotate() { return annotation == null; }

        public virtual void ShowPort(bool isShowing)
        {
            GoPort port = this.NodePort as GoPort; 
            if (port == null) return; 

            GoDocument doc = this.Canvas.View.Document;
            bool oldskips  = doc.SkipsUndoManager;
            doc.SkipsUndoManager = true; 
 
            GoPortStyle oldstyle = port.Style;               
            
            if  (isShowing && this.CanLinkFrom())
            {
                 port.Style = GoPortStyle.Ellipse;
                 port.Brush = Const.portBrush;
                 port.Pen   = Const.portPen;
                (this.canvas as MaxFunctionCanvas).HoverNode = this;
            }
            else port.Style = GoPortStyle.None;

            doc.SkipsUndoManager = oldskips;
             
            if  (port.Style != oldstyle)
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

            this.properties = propertiesManager.ConstructProperties(args);   // 319
            return this.properties;
        } 

        public Framework.Satellite.Property.DataTypes.Type PmObjectType { get { return pmObjectType; } }

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

            serializer.WriteIconicNodeSpecificAttributes(this, writer);

            serializer.WriteCommonAttibutesB(this, writer);

            serializer.WriteAnnotation(this, writer);

            serializer.WriteLinks(this, this.Port, writer);

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

      
    } // class MaxIconicNode



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxConfigurableIconicNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>Iconic node whose icon size and label position is configurable</summary>
    public class MaxConfigurableIconicNode: MaxIconicNode 
    {
        public MaxConfigurableIconicNode(NodeTypes type, MaxCanvas canvas, MaxTool tool, 
        MaxNodeConfigInfo config): base(NodeTypes.None, canvas, tool)  
        { 
            this.Init(type, canvas, tool, config);      
        }


        public MaxConfigurableIconicNode(NodeTypes type, MaxCanvas canvas, MaxTool tool, 
        long id, MaxNodeConfigInfo config): base(NodeTypes.None, canvas, tool) 
        { 
            this.Init(type, canvas, tool, config);      
        }


        protected void Init(NodeTypes type, MaxCanvas canvas, MaxTool tool, MaxNodeConfigInfo config)
        {
            base.LocalInit(type, canvas, tool);  

            ImageList imageList;
            int       imageIndex;

            switch(config.imageType)
            {
               case MaxNodeConfigInfo.ImageTypes.Small:
                    imageList  = tool.ImagesSm.Imagelist;
                    imageIndex = config.customImageIndex > 0? config.customImageIndex: tool.ImageIndexSm;
                    break;

               case MaxNodeConfigInfo.ImageTypes.Custom:
                    imageList  = config.customImage;
                    imageIndex = config.customImageIndex;
                    break;
 
               default:  
                    imageList  = tool.ImagesSm.Imagelist;
                    imageIndex = tool.ImageIndexSm;
                    break;
            }

            // GoIconicNode initialization
            this.Initialize(imageList, imageIndex, this.NodeName);  

            if (config.iconSize != Const.size0x0)
                this.Icon.Size   = config.iconSize;

            this.Label.Editable   = config.labelEditable;
            this.Label.Italic     = config.labelItalic;
            this.Label.FontSize  += config.labelFontDelta;

            if  (config.labelSpot > 0)            // Repo label relative to icon?
            {
                PointF labelPoint = this.Icon.GetSpotLocation(config.relativeIconSpot);
                labelPoint.X += config.labelOffset.X;
                labelPoint.Y += config.labelOffset.Y;
                this.DraggableLabel = true;      // Hack to permit label repo
                this.Label.SetSpotLocation(GoObject.MiddleLeft, labelPoint);
                this.DraggableLabel = false;
            }
        } // Init()

    }   // class MaxConfigurableIconicNode


 
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxPictureNode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxPictureNode: GoIconicNode
    {
        public MaxPictureNode(MaxImageList images, int index, string label) 
        {
            this.Initialize(images.Imagelist, index, label); 
        }

        public MaxPictureNode(string bitmapname, System.Drawing.Size size, string label) 
        {
            MaxEmbeddedBitmapResource resx = new 
                MaxEmbeddedBitmapResource(imagePathPrefix + bitmapname, size);
            if  (resx == null) return;

            this.imageList = resx.LoadBitmapStrip(MaxBitmapResource.point00);
            if  (imageList == null) return;

            this.Initialize(this.imageList.Imagelist, 0, label); 
        }

        private MaxImageList imageList;
        public  MaxImageList Images  { get { return imageList; } }
        private static readonly string imagePathPrefix = "MaxDesigner.Resources.Images.";
    }

} // namespace
 
