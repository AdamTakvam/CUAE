using System;
using System.Xml;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Manager;
using Metreos.Max.Drawing;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework.Satellite.Property;
using Northwoods.Go;
using Crownwood.Magic.Menus;



namespace Metreos.Max.Drawing
{
  public class OldMaxLoopContainerNode: GoGroup, IMaxNode
  {
    private NodeTypes nodeType;
    private string    nodeName;
    private string    fullName;
    private MaxCanvas canvas;
    private MaxTool   tool;   
    private long      nodeID;
    private PropertyDescriptorCollection properties;
    private Framework.Satellite.Property.DataTypes.Type pmObjectType;

    protected Frame   frame;
    protected Xlabel  label;
    private const int minWidth = 60, minHeight = 60;
    public string Text  { get { return label.Text;} set { label.Text = value;} }
    public Xlabel Label { get { return label;     } set { label = value;     } }


    public OldMaxLoopContainerNode(MaxCanvas canvas, SizeF initialSize, string text)
    {
      this.tool     = MaxStockTools.Instance.LoopTool;
      this.nodeName = tool.Name + ++nodecount;
      this.nodeType = NodeTypes.Loop;
      this.canvas   = canvas;
      this.nodeID   = Const.Instance.NextNodeID;

      this.pmObjectType = tool.PmObjectType;
      this.CreateProperties(null);
      if  (text == null) text = Const.DefaultLoopContainerText;

      this.frame = new Frame (this, 3, Const.ExtralightSlateGray, initialSize);
      this.label = new Xlabel(text,-2, true, false, Color.DarkSlateGray);

      this.Resizable = true;

      this.Add(frame);
      this.Add(label);

      this.menu = new MaxNodeMenu(this);
    }


    public class Frame: GoRoundedRectangle
    {
      public Frame(OldMaxLoopContainerNode parent, int width, Color color, SizeF initialSize)
      {
        this.parent     = parent;
        this.Resizable  = true;
        this.DragsNode  = true;
        this.Selectable = false; // !! 
        SizeF size      = new SizeF(Math.Max(initialSize.Width, minWidth), 
                                    Math.Max(initialSize.Height,minHeight));
        this.Bounds     = new RectangleF(new PointF(0,0), size);
        Pen pen         = new Pen(color, width);
        pen.DashStyle   = System.Drawing.Drawing2D.DashStyle.Dash;
        this.Pen = pen;
      }
       
      private OldMaxLoopContainerNode parent;
    }
  

    public class Xlabel: GoText
    { 
      public Xlabel(string text, int relativeFontSize, bool italic, bool border, Color color)
      {
        this.FontSize  += relativeFontSize;
        this.Italic     = italic;
        this.Text       = text;
        this.Bordered   = border;
        this.Editable   = false;
        this.TextColor  = color;
        this.DragsNode  = true;
        this.Selectable = false;
      }
    }


    /// <summary>Return array of all nodes contained by the loop node</summary>
    /// <remarks>If the midpoint of the node's icon is within the bounds of
    /// the loop frame, the node is considered to be within the loop</remarks>
    public IMaxNode[] Contents()
    {
      ArrayList contents = new ArrayList();

      foreach(GoObject x in this.canvas.View.Document) 
      {
        IMaxNode thisnode   = x as IMaxNode; if (thisnode == null) continue;

        RectangleF rect     = Utl.GetNodeIconBounds(thisnode); 

        PointF nodeMidpoint = Utl.Midpoint(rect);
        
        if  (this.Bounds.Contains(nodeMidpoint))         
             contents.Add(thisnode);       
      } 

      if  (contents.Count == 0) return null;
       
      IMaxNode[] nodes = new IMaxNode[contents.Count];
      contents.ToArray().CopyTo(nodes,0); 
      return nodes;
    }


    /// <summary>Arrange position of loop node components relative to group</summary>
    public override void LayoutChildren(GoObject childchanged)
    {
      if  (label == null)  return;
      RectangleF gbounds = this.Bounds;
      RectangleF fbounds = frame.Bounds;
  
      label.Location  = new PointF(fbounds.Left + 4F, fbounds.Top + 1F);

      if  (fbounds.Right < label.Bounds.Right + 8F)
           frame.Size = new SizeF(label.Width + 12F, frame.Height);

      if  (frame.Height  < minHeight)
           frame.Size = new SizeF(frame.Width, minHeight);
    }


    /// <summary>Show properties for the loop node</summary>
    private void OnMenuProperties(object sender, EventArgs e)
    {
      PmProxy.ShowProperties(this, this.PmObjectType); 
    }


    /// <summary>Select all children in the group when group is selected</summary>
    public override void OnGotSelection(GoSelection sel)
    {
      base.OnGotSelection (sel);

      IMaxNode[] contents = this.Contents(); if (contents == null) return;

      foreach(IMaxNode node in contents)
              this.canvas.View.Selection.Add(node as GoObject);
    }


    /// <summary>Clear stale properties grid content</summary>
    public override void OnLostSelection(GoSelection selection)
    {
      this.canvas.OnPropertiesFocus(null);   

      base.OnLostSelection(selection);
    }


    /// <summary>Handle a changed property arrival via property grid</summary>
    protected void OnPropertiesChanged(MaxProperty[] properties)
    {
      foreach(MaxProperty property in properties)
      {
        if  (property == null || !property.IsChanged) continue;

        switch(property.Name)
        {
          case Const.PmLoopCountName:
               this.OnLoopLimitPropertyChanged(property);
               break;
        }  
      }      
    }


    /// <summary>Handle loop limit change via property grid</summary>
    protected void OnLoopLimitPropertyChanged(MaxProperty property)
    {          
      // 1/1/04 This code is complete to date, but could change once
      // the "Count" property is modified to reflect the variable and
      // csharp loop types. Lose this comment when that occurs.
      LoopTypeProperty loopTypeProperty 
            = property.ChildrenProperties[DataTypes.LOOP_TYPE_NAME] as LoopTypeProperty; 

      DataTypes.LoopType loopType 
           = loopTypeProperty == null? DataTypes.LoopType.Integer: loopTypeProperty.Value;

      switch(loopType)
      {
        case DataTypes.LoopType.Integer:
             int  inewval  = Utl.atoi(property.Value    as string);
             int  ioldval  = Utl.atoi(property.OldValue as string);
             if  (inewval == ioldval)  { }
             else 
             if  (inewval > 0)
                  this.Text = Const.LoopContainerTextPrefix + inewval
                            + Const.LoopContainerTextSuffixConstant;
             else property.Value = property.OldValue;  
             break;  

        case DataTypes.LoopType.Variable:
             this.Text = Const.LoopContainerTextPrefix  
                       + Const.LoopContainerTextSuffixVariable;
             break;

        case DataTypes.LoopType.CSharp:
             this.Text = Const.LoopContainerTextPrefix  
                       + Const.LoopContainerTextSuffixExpression;
             break;
      }            
    }


    public override bool OnContextClick(GoInputEventArgs evt, GoView view)
    {
      menu.PopContextMenu(this);
      return true;
    }


    private static readonly float minX = Config.minNodeXf - Const.LoopOffsetX;
    private static readonly float minY = Config.minNodeYf - Const.LoopOffsetY;


    /// <summary>constrain node to positive coordinates</summary>
    public override PointF ComputeMove(PointF origLoc, PointF newLoc)
    {
      return this.canvas.View.ComputeMove(this, origLoc, newLoc, minX, minY); 
    }


    protected MaxNodeMenu menu;
    private static int  nodecount = 0;
    private static MenuCommand mcSeparator = new MenuCommand(Const.dash);
    private long container;
    public  long Container { get { return container; } set { container = value; } }

    #region IMaxNode Members

    public string    NodeName  { get { return nodeName;} set { nodeName = value; } }
    public string    FullName  { get { return fullName;} set { fullName = value; } }
    public long      NodeID    { get { return nodeID;  } set { nodeID = value;   } }
    public NodeTypes NodeType  { get { return nodeType;} }
    public IGoPort   NodePort  { get { return null;    } }
    public MaxCanvas Canvas    { get { return canvas;  } }
    public MaxTool   Tool      { get { return tool;    } }

    public string    GroupName 
    {
      get { return tool.ToolGroup == null? null: tool.ToolGroup.GroupName; } 
    }   
    public virtual void OnLinkCreated(IMaxLink link) { }  
    public virtual void OnLinkDeleted(IMaxLink link) { }
    public virtual void CanDeleteLink(object sender, CancelEventArgs e)  { }

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

    public void OnPropertiesChangeRaised(MaxProperty[] props) { this.OnPropertiesChanged (props);  }  

    #endregion

    #region MaxObject Members

    public Metreos.Max.Core.ObjectTypes MaxObjectType { get { return ObjectTypes.Node; } }     

    public void MaxSerialize(XmlTextWriter writer)   
    {
      MaxNodeSerializer serializer = MaxNodeSerializer.Instance;

      writer.WriteStartElement(Const.xmlEltNode); // <node>

      serializer.WriteCommonAttibutesA(this, writer);

      serializer.WriteLoopNodeSpecificAttributes(this, writer);

      serializer.WriteCommonAttibutesB(this, writer);

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
  } // class MaxToolContainerNode

}   // namespace
