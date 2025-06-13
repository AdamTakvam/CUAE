using System;
using System.Xml;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework.Satellite.Property;
using Northwoods.Go;



namespace Metreos.Max.Drawing
{
    /// <summary>
    ///  A two-icon node with parent iconic subnode linked to child iconic subnode.
    ///  The child and link are movable but cannot be deleted separately. 
    ///  Selecting the parent node selects the group. 
    /// </summary> 
    public class MaxMultiIconNode: GoNode, IMaxNode
    {      
        // Requires a System.ComponentModel.CancelEventHandler delegate, handling   
        // the SelectionDeleting event in the view, which identifies our subnode 
        // types, permits or denies the deletion accordingly, and ensures all members 
        // of group are deleted with the group.

        // Differs from other node types in that it requires a separate Create
        // invocation after construction.

        // Properties are maintained for the primary node only

        public MaxMultiIconNode() { this.nodeType = NodeTypes.Group; }

        public MaxMultiIconNode(NodeTypes type, MaxCanvas canvas) 
        {
            this.nodeID = Const.Instance.NextNodeID;    
            this.Init(type, canvas); 
        }


        public MaxMultiIconNode(NodeTypes type, MaxCanvas canvas, long ID) 
        {       
            this.nodeID = ID;
            this.Init(type, canvas);
        }


        private void Init(NodeTypes type, MaxCanvas canvas)
        {
            this.nodeType = type; this.canvas = canvas;
            if  (this.canvas == null) throw new ArgumentNullException(); 
        }


        public void Render()
        {
            this.Link.RealLink.CalculateStroke();
        }


        private NodeTypes nodeType;             
        private string    nodeName;
        private string    fullName;
        private MaxCanvas canvas;
        private MaxTool   tool;
        private long      nodeID;
        private MaxAnnotationNode annotation = null;
        public  MaxAnnotationNode Annotation { get { return annotation; } set { annotation = value; } }
        public  virtual bool CanAnnotate()   { return annotation == null; }
        private PropertyDescriptorCollection properties;
        public  Framework.Satellite.Property.DataTypes.Type pmObjectType;


        /// <summary>Configure the multinode</summary>    
        public void Create(MaxTool tool, string txtA, MaxImageList imgB, 
            int imgIndexB, string txtB, string txtL, PointF offset)      
        {
            this.tool = tool;  
            this.Resizable = false;  

            this.pmObjectType = tool.PmObjectType; 
            this.CreateProperties(this.Tool.PackageToolDefinition);

            this.nodeName = tool.Name == null? Const.defaultNodeName: 
                Utl.StripQualifiers(tool.Name);  
   
            anode = new ParentSubnode(this);
            anode.Initialize(tool.ImagesLg.Imagelist, tool.ImageIndexLg, txtA);
            anode.Location = new PointF(0,0);  

            bnode = new ChildSubnode(this);
            bnode.Initialize(imgB.Imagelist, imgIndexB, txtB);
            bnode.Location = offset;
             
            link = new ParentChildLink(this, txtL);
            link.Node = this; 
            
            this.Add(link);
            this.Add(anode);
            this.Add(bnode);

            bnode.Port.IsValidFrom = bnode.Port.IsValidTo = false;

        } // Create() 


        /// <summary>Makes previously undeletable group members deletable</summary>
        public void RemoveGroup(GoView view)
        {
            bnode.Deletable = link.Deletable = true;
        } 


        public virtual bool OnParentSubnodeDoubleClick(GoInputEventArgs evt, GoView view)
        {
            return true; // MessageBox.Show("Double-click") != DialogResult.Abort;
        } 

        public virtual bool OnChildSubnodeDoubleClick(GoInputEventArgs evt, GoView view)
        {
            return true; // MessageBox.Show("Double-click") != DialogResult.Abort;
        }  
   

        /// <summary>The primary iconic node in the group</summary>       
        public class ParentSubnode: GoIconicNode
        {
            public ParentSubnode(MaxMultiIconNode node)
            {
                this.parent    = node;
                this.DragsNode = true;            // Can drag group by primary node
            }

            /// <summary>Adds child node to selection being deleted</summary>
            public void RemoveGroup(GoView view)
            {
                parent.RemoveGroup(view);
                view.Selection.Add(this.Parent);
            }

            public override bool OnDoubleClick(GoInputEventArgs evt, GoView view)
            {
                return parent.OnParentSubnodeDoubleClick(evt, view);
            } 

            private MaxMultiIconNode parent;
        } // class ParentSubnode


        /// <summary>The child iconic node in the group</summary>
        public class ChildSubnode: GoIconicNode
        {
            public ChildSubnode(MaxMultiIconNode node)
            {
                this.parent     = node;
                this.DragsNode  = false;          // Can drag child node independently
                this.Deletable  = false;
                this.Selectable = parent.canMoveChild;
            }

            protected override GoText CreateLabel(string name)
            {
                ChildSubnodeLabel label = new ChildSubnodeLabel(this.parent, name);
                label.TextColor = parent.childLabelColor;

                return label;
            }

            public override bool OnDoubleClick(GoInputEventArgs evt, GoView view)
            {
                return parent.OnChildSubnodeDoubleClick(evt, view);
            } 

            private MaxMultiIconNode parent;
        } // class ChildSubnode


        /// <summary>The link connecting the parent and child nodes</summary>
        public class ParentChildLink: MaxLabeledLink
        {
            public ParentChildLink(MaxMultiIconNode parent, string labelText)
            {   
                this.parent = parent;      
                this.CreateLabel(labelText); 
      
                this.Node = parent.anode as IMaxNode;

                this.Init();
            }

            public ParentChildLink() { this.Init();  }


            private void Init()
            {        
                this.FromPort    = parent.anode.Port;
                this.ToPort      = parent.bnode.Port;
                this.ToArrow     = this.ToArrowFilled = true;
                this.Pen         = new Pen(parent.linkColor, 2);  
                this.Brush       = new SolidBrush(parent.linkColor);                 
                this.Deletable   = this.Relinkable = this.Selectable = false; 
            }


            public void CreateLabel(string text)
            {
                GoText label     = new GoText();
                label.Text       = text;
                label.TextColor  = parent.childLabelColor;
                label.Selectable = false;
                this.SetLabel(label, LabelPositions.Mid);
            }

            MaxMultiIconNode parent;
        }  // class ParentChildLink



        /// <summary>Represents the text label displayed beneath the child node</summary>
        public class ChildSubnodeLabel: GoText 
        {
            public ChildSubnodeLabel(MaxMultiIconNode parent, string text) 
            {       
                this.parent    = parent;  
                string label   = text == null? Const.blank: text;
                this.Text      = this.originalText = label;
                this.Editable  = parent.canEditChildLabel;
                this.TextColor = parent.childLabelColor;
            } 


            public override void Changed
            ( int subhint, int oI, object ov, RectangleF or, int nI, object nv, RectangleF nr) 
            {   
                switch(subhint)
                {
                   case GoText.ChangedText:       
                        this.OnTextChanged();                 
                        break;

                   default:
                        base.Changed(subhint, oI, ov, or, nI, nv, nr);
                        break;
                }    
            }

            public virtual void OnTextChanged()
            {
                if  (this.Text.Length == 0 || this.Text.StartsWith(Const.blank))
                    this.Text = this.originalText;
                // else xxxxx; // Here, when editable, we will fire an event to notify
                // interested parties to change function node, canvas, and tab names

                this.originalText = this.Text;
            }

            public override bool CanDelete() { return false; }

            protected string           originalText;
            protected MaxMultiIconNode parent;

        } // class ChildSubnodeLabel

      
        protected ParentSubnode   anode;
        protected ChildSubnode    bnode;
        protected ParentChildLink link;
        public    ParentChildLink Link { get { return link; } }


        /// <summary>Actions on node losing keyboard focus</summary>
        public override void OnLostSelection(GoSelection selection)
        {
            this.canvas.OnPropertiesFocus(null);  // Clear properties grid

            base.OnLostSelection(selection);
        }

        private bool canEditChildLabel = true;    
        public  bool CanEditChildLabel 
        {
            get { return canEditChildLabel;  } 
            set { canEditChildLabel = bnode.Label.Editable = value; } 
        }
      
        private bool canMoveChild = true;
        public  bool CanMoveChild 
        {
            get { return canMoveChild;  } 
            set { canMoveChild = value; } 
        }

        private PointF childOffset = new PointF(92,24);
        public  PointF ChildOffset 
        {
            get { return childOffset;  } 
            set { childOffset = value; }
        }

        private Color childLabelColor = Const.linkColorLight;
        public  Color ChildLabelColor 
        {
            get { return childLabelColor;  } 
            set { childLabelColor = value; } 
        }

        private Color linkColor = Const.linkColorExtraLight;
        public  Color LinkColor 
        {
            get { return linkColor;  } 
            set { linkColor = value; } 
        }

        private long container;
        public  long Container { get { return container; } set { container = value; } }

        #region IMaxNode Members

        public string     NodeName  { get { return nodeName;  } set { nodeName = value; } }
        public string     FullName  { get { return fullName;  } set { fullName = value; } }
        public long       NodeID    { get { return nodeID;    } set { nodeID = value;   } }
        public PointF     NodeLoc   { get { return Location;  } set { Location = value; } }
        public RectangleF NodeBounds{ get { return Bounds;    } }
        public NodeTypes  NodeType  { get { return nodeType;  } }
        public IGoPort    NodePort  { get { return anode.Port;} }
        public MaxCanvas  Canvas    { get { return canvas;    } }
        public MaxTool    Tool      { get { return tool;      } }
        public string GroupName 
        {
            get { return tool.ToolGroup == null? null: tool.ToolGroup.GroupName; } 
        }   

        public virtual void OnLinkCreated(IMaxLink link) { }
     
        public virtual void OnLinkDeleted(IMaxLink link) { }  

        public virtual void CanDeleteLink(object sender, CancelEventArgs e) { e.Cancel = true; }

        public virtual void ShowPort(bool isShowing)     { }
    
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

        public void OnPropertiesChangeRaised(MaxProperty[] properties) { }  

        #endregion

        #region MaxObject Members

        public Metreos.Max.Core.ObjectTypes MaxObjectType { get { return ObjectTypes.Node; } }

        public string ObjectDisplayName { get { return this.nodeName; } }

        public void MaxSerialize(XmlTextWriter writer)
        {
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
    } // class MaxMultiIconNode

}  // namespace
