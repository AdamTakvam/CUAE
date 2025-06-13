using System;
using System.Xml;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Metreos.Max.Framework.Satellite.Property;
using Crownwood.Magic.Menus;
using Northwoods.Go;



namespace Metreos.Max.Drawing
{
    public enum LinkTypes  { None, Basic, Labeled }
    public enum LinkStyles { None, Vector, Bevel, Bezier }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // IMaxLink
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>Implemented by all graph link types</summary>
    public interface IMaxLink: MaxSelectableObject
    { 
        LinkTypes     LinkType    { get; set; }
        LinkStyles    LinkStyle   { get; set; }
        bool          IsOrthogonal{ get; set; }
        int           LinkWidth   { get; set; }
        Color         LinkColor   { get; set; }
        MaxCanvas     Canvas      { get; set; }
        IMaxNode      Node        { get; set; }
        MaxLinkHelper Helper      { get; }
        // PropertyDescriptorCollection CustomProperties { get; }
    }



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxBasicLink
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>A regular link without a label</summary>
    public class MaxBasicLink: GoLink, IMaxLink
    {
        private  LinkTypes  linkType;
        private  LinkStyles linkStyle;
        private  bool       orthogonal;
        private  int        width;
        private  Color      color;
        private  MaxCanvas  canvas;
        private  IMaxNode   node;
        private  PropertyDescriptorCollection properties;
        private  MaxLinkHelper helper;

        public MaxBasicLink() 
        {
            this.Init();
        }


        public MaxBasicLink(MaxCanvas canvas) 
        {
            this.canvas = canvas;
            this.Init();
        }


        public MaxBasicLink(MaxCanvas canvas, IMaxNode node) 
        {
            this.SetHosts(canvas, node);       
            this.Init();
        }

        public MaxBasicLink(MaxCanvas canvas, IMaxNode node, LinkStyles linkStyle) 
        {
            this.SetHosts(canvas, node);
            this.linkStyle = linkStyle;
            this.Init();
        }


        private void Init()
        {
            this.linkType = LinkTypes.Basic;
            helper = new MaxLinkHelper(this, this as GoLink, this as GoStroke);
            helper.Init();
            this.Relinkable = Config.IsLinksRelinkable;
            this.CreateProperties(null);
        } 


        /// <summary>Set parent information for this link</summary>
        public void SetHosts(MaxCanvas canvas, IMaxNode node)
        {
            this.canvas = canvas;
            this.node   = node;
        }


        /// <summary>constrain links to positive coordinates</summary>
        public override PointF ComputeMove(PointF origLoc, PointF newLoc)
        {
            return this.canvas.View.ComputeMove(this, origLoc, newLoc); 
        }


        public override bool OnContextClick(GoInputEventArgs evt, GoView view)
        {
            this.helper.PopLinkContextMenu(); 
            return true;
        }


        public override void OnLostSelection(GoSelection sel)
        {
            PmProxy.PropertyWindow.Clear(this);
            base.OnLostSelection (sel);
        }


        #region IMaxLink Members

        public LinkTypes  LinkType  { get { return linkType;  } set { linkType = value;  } } 

        public LinkStyles LinkStyle { get { return linkStyle; } set { linkStyle = value; } }
   
        public bool  IsOrthogonal   { get { return orthogonal;} set { orthogonal = value;} }   

        public int   LinkWidth      { get { return width; } set { width = value; } }  

        public Color LinkColor      { get { return color; } set { color = value; } } 

        public MaxCanvas Canvas     { get { return canvas;} set { canvas = value;} } 

        public IMaxNode  Node       { get { return node;  } set { node = value;  } } 

        public MaxLinkHelper Helper { get { return helper;  } } 

        public PropertyDescriptorCollection CustomProperties { get { return properties; } }
    
        #endregion

        #region MaxSelectableObject Members

        public PropertyDescriptorCollection MaxProperties { get { return this.properties; } }

        /// <summary>Ask properties manager to create this object's properties</summary>                
        public PropertyDescriptorCollection CreateProperties(PropertyGrid.Core.PackageElement pe) 
        {
            //if  (pe == null) return null;
            MaxPropertiesManager propertiesManager = PmProxy.PropertiesManager;

            CreatePropertiesArgs args = new
                CreatePropertiesArgs(this, pe, this.PmObjectType);

            this.properties = propertiesManager.ConstructProperties(args);
            return this.properties;
        }   

        public Framework.Satellite.Property.DataTypes.Type PmObjectType 
        { get { return Framework.Satellite.Property.DataTypes.Type.Link; } }

        public void OnPropertiesChangeRaised(MaxProperty[] properties) { }  

        #endregion

        #region MaxObject Members

        public Metreos.Max.Core.ObjectTypes MaxObjectType { get { return ObjectTypes.Link; } }
   
        public void MaxSerialize(XmlTextWriter writer)
        {
        }

        public string ObjectDisplayName 
        { get { return Const.LinkObjectDisplayName + this.LinkStyle.ToString(); } 
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

    }



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxLabeledLink
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>A link with an object label</summary>
    public class MaxLabeledLink: GoLabeledLink, IMaxLink
    {
        public enum LabelPositions {None, From, Mid, To } 

        private  LinkTypes  linkType;
        private  LinkStyles linkStyle;
        private  bool       orthogonal;
        private  int        width;
        private  Color      color;
        private  MaxCanvas  canvas;
        private  IMaxNode   node;
        private  PropertyDescriptorCollection properties;
        private  MaxLinkHelper helper;


        public MaxLabeledLink() 
        {
            this.Init();
        }


        public MaxLabeledLink(MaxCanvas canvas) 
        {
            this.canvas = canvas;
            this.Init();
        }


        public MaxLabeledLink(MaxCanvas canvas, IMaxNode node, string labeltext) 
        {
            this.SetHosts(canvas, node, labeltext);       
            this.Init();
        }

        
        public MaxLabeledLink(MaxCanvas canvas, IMaxNode node, string labeltext, LinkStyles linkStyle) 
        {
            this.SetHosts(canvas, node, labeltext);
            this.linkStyle = linkStyle;
            this.Init();
        }


        private void Init()
        {
            this.linkType = LinkTypes.Labeled;
            helper = new MaxLinkHelper(this, this.RealLink as GoLink, this.RealLink as GoStroke);
            helper.Init();
            this.Relinkable = Config.IsLinksRelinkable;
            this.CreateProperties(null);
        } 


        /// <summary>Set parent information for this link</summary>
        public void SetHosts(MaxCanvas canvas, IMaxNode node, string labeltext)
        {
            this.canvas = canvas;
            this.node   = node;
            if (labeltext != null) 
                this.SetLabelText(labeltext, LabelPositions.Mid); 
        }


        /// <summary>Set a text label at specified position</summary>
        public void SetLabelText(string text, LabelPositions labelPos)
        {
            GoText label = new GoText();
            label.Text = text;
            this.SetLabel(label, labelPos);
        }


        /// <summary>Set an object label specified position</summary>
        public void SetLabel(GoObject label, LabelPositions labelPos)
        {
            switch(labelPos)
            {
                case LabelPositions.From: this.FromLabel = label; break;
                case LabelPositions.Mid:  this.MidLabel  = label; break;
                case LabelPositions.To:   this.ToLabel   = label; break;
            }
        }


        /// <summary>Determine amplitude of link's bezier curve</summary>
        public static void CalculateCurvature(MaxLabeledLink link)
        {
            // When multiple links between the same pair of nodes, 
            // attempt to ensure that no link is hidden by another 
            if  (link == null) return;
            int  linkcount  = link.Canvas.NumLinksBetween(link.FromPort, link.ToPort);
            bool isOddLink  = (linkcount & 1) != 0;          
            int  curviness  = isOddLink? 16 * linkcount: 0 - (16 * (linkcount - 1));
            link.Curviness  = curviness;  
            link.MidLabel.Movable = true;         
            link.RealLink.CalculateStroke();       
        }


        /// <summary>constrain links to positive coordinates</summary>
        public override PointF ComputeMove(PointF origLoc, PointF newLoc)
        {
            return this.canvas.View.ComputeMove(this, origLoc, newLoc); 
        }


        public override bool OnContextClick(GoInputEventArgs evt, GoView view)
        {
            this.helper.PopLinkContextMenu(); 
            return true;
        }


        public override void OnLostSelection(GoSelection sel)
        {
            PmProxy.PropertyWindow.Clear(this);
            base.OnLostSelection (sel);
        }


        #region IMaxLink Members

        public LinkTypes  LinkType  { get { return linkType;  } set { linkType = value;  } } 

        public LinkStyles LinkStyle { get { return linkStyle; } set { linkStyle = value; } }
   
        public bool  IsOrthogonal   { get { return orthogonal;} set { orthogonal = value;} }   

        public int   LinkWidth      { get { return width; } set { width = value; } }  

        public Color LinkColor      { get { return color; } set { color = value; } } 

        public MaxCanvas Canvas     { get { return canvas;} set { canvas = value;} } 

        public IMaxNode  Node       { get { return node;  } set { node = value;  } } 

        public MaxLinkHelper Helper { get { return helper;  } } 

        public PropertyDescriptorCollection CustomProperties { get { return properties; } }

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

        public Framework.Satellite.Property.DataTypes.Type PmObjectType 
        { get { return Framework.Satellite.Property.DataTypes.Type.Link; } }

        public void OnPropertiesChangeRaised(MaxProperty[] properties) { }  

        #endregion

        #region MaxObject Members

        public Metreos.Max.Core.ObjectTypes MaxObjectType { get { return ObjectTypes.Link; } }
   
        public void MaxSerialize(XmlTextWriter writer)
        {
        }

        public string ObjectDisplayName 
        {
           get { return Const.LinkObjectDisplayName + this.LinkStyle.ToString(); } 
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

    } // class MaxLabeledLink

}  // namespace


