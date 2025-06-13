using System;
using System.Xml;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Metreos.Max.Manager;
using Metreos.Max.Core.Tool;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework.Satellite.Property;
using Northwoods.Go;
using Crownwood.Magic.Menus;



namespace Metreos.Max.Drawing
{
  /// <summary>
  ///  A complex node containing a parent function node in regular or list 
  ///  orientation, attached to a vertical child list of event nodes in side 
  ///  list orientation
  ///  </summary> 
  ///  <remarks>The function node is displayed with static link lines to each
  ///  event node, such as would appear in a tree control. The child 
  ///  information is not separately movable. List slots may be empty, in order
  ///  that one MaxFunctionEventsNode can be shown to line up with another 
  ///  displayed along side it, which also may have zero or more events in its 
  ///  own list, the initial list skipping one slot for each such event in the
  ///  second node.
  /// </remarks> 
	public class MaxFunctionEventsNode: GoNode, IComplexMaxNode
	{
    public    enum PrimaryNodeTypes { None, Standard, Recumbent }
    private   PrimaryNodeTypes primaryNodeType;
    private   NodeTypes  nodeType;             
    private   string     nodeName;
    private   string     fullName;
    private   MaxCanvas  canvas;
    private   MaxTool    tool;
    private   long       nodeID;
    private   PropertyDescriptorCollection properties;
    public    Framework.Satellite.Property.DataTypes.Type pmObjectType;

    private   MaxEnumerableNodes maxEnumerables;
    protected IMaxNode      handlerFor;     // The event we are the handler for
    public    IMaxNode      HandlerFor  { get { return handlerFor; } }
    public    GoPort        Port  { get { return pnode.Port; } }
    public    MaxIconicNode Pnode { get { return pnode; } }

    protected MaxIconicNode pnode;
    protected EventList     cnode;
    protected GoStroke      connectors;


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Construction
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

		public MaxFunctionEventsNode()
		{
      this.canvas   = null; // MaxProject.AppCanvas; 
			this.nodeType = NodeTypes.Group;
      this.primaryNodeType = PrimaryNodeTypes.Recumbent;
      this.Create();
		}


    public MaxFunctionEventsNode
    ( PrimaryNodeTypes type, MaxCanvas canvas, IMaxNode ev) 
    {
      this.nodeID = Const.Instance.NextNodeID;    
      this.Init(type, canvas, ev); 
      this.Create();
    }


    public MaxFunctionEventsNode
    ( PrimaryNodeTypes type, MaxCanvas canvas, IMaxNode ev, long ID) 
    {       
      this.nodeID = ID;
      this.Init(type, canvas, ev);
      this.Create();
    }


    public void Init(PrimaryNodeTypes type, MaxCanvas canvas, IMaxNode ev)
    {
      this.primaryNodeType = type;
      this.nodeType   = NodeTypes.Group; 
      this.canvas     = canvas;
      this.handlerFor = ev;
    }


    /// <summary>Configure the multinode</summary>    
    public void Create() 
    {
      if  (this.canvas == null) throw new ArgumentNullException(); 
      this.maxEnumerables = new MaxEnumerableNodes(this);

      this.tool = MaxStockTools.Instance.FunctionTool;
      this.Resizable = false;  

      this.pmObjectType = tool.PmObjectType; 
      this.CreateProperties(this.Tool.PackageToolDefinition);
      this.nodeName = Const.defaultNodeName;

      if  (this.primaryNodeType == PrimaryNodeTypes.Standard)
           this.pnode = new ParentNodeStandard (this);
      else this.pnode = new ParentNodeRecumbent(this); 

      this.cnode = new EventList(this);

      this.connectors = new GoStroke();
      connectors.Selectable = connectors.FromArrow = connectors.ToArrow = false;
      connectors.Pen = Pens.DarkGray;     
      connectors.Visible = false;

      this.Add(pnode);
      this.Add(cnode);;
      this.Add(connectors);
    } // Create() 


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // ParentSubnode
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    protected class ParentNodeStandard: MaxFunctionNode
    {
      public ParentNodeStandard(MaxFunctionEventsNode parent): 
      base(parent.canvas, parent.tool) 
      { 
        this.parent = parent; 
        this.DragsNode = true; 
      }

      public override void OnGotSelection(GoSelection selection)
      {     
        selection.RemoveAllSelectionHandles();
        selection.Add(parent);         
      }

      private MaxFunctionEventsNode parent;
    }


    protected class ParentNodeRecumbent: MaxRecumbentFunctionNode
    {
      public ParentNodeRecumbent(MaxFunctionEventsNode parent): 
      base(parent.canvas, parent.tool) 
      { 
        this.parent = parent;
        this.DragsNode = true;
      }

      public override void OnGotSelection(GoSelection selection)
      {                            
        selection.RemoveAllSelectionHandles();
        selection.Add(parent);
      }

      private MaxFunctionEventsNode parent;
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Methods
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>Add a list of events to the event list</summary>
    /// <remarks>Postpones parent list adjustment until all events added</remarks>
    public void Add(string[] eventNames)
    {
      this.isBatchRequest = true;

      foreach(string qualifiedEventName in eventNames)
              this.Add(qualifiedEventName);
                                           // Get event this handler handles
      EventListItem parent = this.handlerFor as EventListItem;
      if  (parent != null)                 // If another event list, adjust it
           parent.NormalizeList(eventNames.Length);

      this.isBatchRequest = false;
    }


    /// <summary>Add an event to the list</summary>
    public void Add(string qualifiedEventName)    
    {                                            // Add event to list
      EventListItem eventEntry = this.cnode.AddItem(qualifiedEventName);
      if  (eventEntry == null)  return;
                                                 // Create and add event handler
      PointF pt = new PointF(eventEntry.Location.X + cnode.Width + 8, eventEntry.Top);

      MaxFunctionEventsNode handlerNode = this.canvas.CreateFunctionEventsNode        
      (eventEntry, qualifiedEventName, pt, true);
         
      handlerNode.Render();
      eventEntry.Handler = handlerNode;     // Register handler with event
                                            // Add handler to canvas
      canvas.View.Document.Add(handlerNode as GoGroup);   

      this.CreateLink(eventEntry, handlerNode);    
                                            // Adjust parent list appearance
      EventListItem parent = this.handlerFor as EventListItem;
      if  (!this.isBatchRequest && parent != null)
            parent.NormalizeList(1);

      MaxTabEventArgs args = new            // Notify manager and framework
      MaxTabEventArgs(handlerNode.NodeName, MaxCanvas.CanvasTypes.Function);
      args.suppressTabSwitch = true; 
         
      canvas.FireTabEvent(args);   
    } 


    /// <summary>Create link from list event to its handler node</summary>
    public void CreateLink(EventListItem eventEntry, MaxFunctionEventsNode handlerNode)
    {
      if  (eventEntry == null || handlerNode == null) return;
      MaxBasicLink link = new MaxBasicLink(this.canvas, eventEntry);
      if  (link == null) return;

      eventEntry.CanLinkOut = true; 

      handlerNode.pnode.Port.IsValidTo = true;

      this.CreateLink(eventEntry as MaxIconicNode, handlerNode.pnode as MaxIconicNode);
   
      handlerNode.pnode.Port.IsValidTo = false;
      eventEntry.CanLinkOut = false;    
    }


    /// <summary>Create link from list event to its handler node</summary>
    public void CreateLink(MaxIconicNode eventEntry, MaxIconicNode handlerNode)
    {
      if  (eventEntry == null || handlerNode == null) return;
      MaxBasicLink link = new MaxBasicLink(this.canvas, eventEntry);
      if  (link == null) return;

      link.Node     = eventEntry;
      link.FromPort = eventEntry.Port;
      link.ToPort   = handlerNode.Port;
    
      this.RealizeLink(link, eventEntry.Port, handlerNode.Port);   
    }


    /// <summary>Configure and realize event to handler link</summary>
    public MaxBasicLink RealizeLink(MaxBasicLink link, GoPort fromPort, GoPort toPort)
    {
      link.ToArrow  = link.ToArrowFilled = true;
      Pen pen       = new Pen(Color.FromArgb(127,127,127), 1);
      pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
      link.Pen   = pen;         
      link.Brush = Brushes.DarkGray;
      link.ToArrowWidth -=4;  
      link.Deletable = false; link.Relinkable = true;                  
      link.CalculateStroke();

      this.Add(link);
      return link;
    }


    public void Remove(string[] eventNames)
    {
      this.isBatchRequest = true;

      foreach(string qualifiedEventName in eventNames)
              this.Remove(qualifiedEventName);

      EventListItem parent = this.handlerFor as EventListItem;
      if  (parent != null)
           parent.NormalizeList(0 - eventNames.Length);

      this.isBatchRequest = false;
    }


    // <summary>Remove an event from the list</summary>
    public void Remove(long nodeID)
    {
      this.cnode.RemoveItem(nodeID);
       
      EventListItem parent = this.handlerFor as EventListItem;
      if  (!this.isBatchRequest && parent != null)
            parent.NormalizeList(-1);
    } 


    /// <summary>Layout</summary>
    public void Render()
    {
      DrawConnectors();
      AlignHandlers();                      // TODO propagate
    }


    /// <summary>Return indication as to whether the group node can be deleted</summary>
    public override bool CanDelete()
    {
      return this.pnode.CanDelete();   
    }


    /// <summary>Actions on node losing keyboard focus</summary>
    public override void OnLostSelection(GoSelection selection)
    {
      this.canvas.OnPropertiesFocus(null);  // Clear properties grid

      base.OnLostSelection(selection);
    }


    /// <summary>Position list relative to primary node and draw connectors</summary>
    public override void LayoutChildren(GoObject childchanged)
    {    
      if  (childchanged is MaxIconicNode)
      {
           float leftanchor = childchanged is ParentNodeRecumbent? pnode.Left: pnode.Label.Left;

           this.cnode.Location = new PointF(leftanchor + 4 + LISTOFFSET, pnode.Label.Bottom+4);
               
           this.DrawConnectors();
      }

      else base.LayoutChildren (childchanged);
    }


    /// <summary>Draw treeview-style connectors from function to events</summary>
    protected void DrawConnectors()
    {
      if  (connectors == null) return;
      ArrayList items = cnode.Items;      
      connectors.Visible = items.Count != 0;
      if  (!connectors.Visible) return;

      connectors.ClearPoints();
      PointF startPoint = new PointF(cnode.Location.X - 7, cnode.Location.Y-4);
      connectors.AddPoint(startPoint);
      float x = startPoint.X;
      
      foreach(Object o in items)
      {
        EventListItem item = o as EventListItem; if (item == null) continue;
        PointF pt = item.Location;
        connectors.AddPoint(x, pt.Y);
        connectors.AddPoint(x+LISTOFFSET+2, pt.Y);
        connectors.AddPoint(x, pt.Y);       // Backtrack 
      }
    }


    /// <summary>Align each event's handler with the event</summary>
    /// <remarks>Propagates forward</remarks>
    public void AlignHandlers()
    {
      float width = cnode.MaxWidth();

      foreach(Object o in cnode.Items)
      {
        EventListItem item  = o as EventListItem; if (item == null) continue;
        item.Handler.Location = new PointF(item.Location.X + width + 32, item.Top);
        item.Link.CalculateStroke();        // Rerender link
        
        item.Handler.AlignHandlers();       // Propagate realignment
      }
    }


    public override bool OnContextClick(GoInputEventArgs evt, GoView view)
    {        
      PopupMenu popup = new PopupMenu();
      popup.MenuCommands.Add(new MenuCommand("Add 1",   new EventHandler(OnAdd1)));
      popup.MenuCommands.Add(new MenuCommand("Add 2",   new EventHandler(OnAdd2)));
      popup.MenuCommands.Add(new MenuCommand("Remove 1",new EventHandler(OnRem1)));
      popup.MenuCommands.Add(new MenuCommand("Remove 2",new EventHandler(OnRem2)));
      popup.TrackPopup(Control.MousePosition);
      return false;
    }

    public void OnAdd1(object x, EventArgs e) 
    {
      this.Add("Metreos.CallControl.CallEstablished");
      this.Render();
    }

    public void OnAdd2(object x, EventArgs e) 
    {  
      this.Add(new string[] { "Metreos.CallControl.CallEstablished",
                              "Metreos.CallControl.SignallingChange" } );
      this.Render();
    }

    public void OnRem1(object x, EventArgs e) { MessageBox.Show("Rem 1"); }
    public void OnRem2(object x, EventArgs e) { MessageBox.Show("Rem 2"); }


    /// <summary>Return enumerable helper satisfying IMaxComplexNode</summary>
    public IEnumerable MaxNodes { get { return maxEnumerables; } }

    public static readonly int LISTOFFSET = 5;


    /// <summary>Enumerable helper to iterate over the IMaxNodes in the group</summary>
    /// <remarks>Since the parent GoGroup already is IEnumerable, we use an 
    /// embedded IEnumerable helper to iterate over just the IMaxNodes.
    /// IMaxComplexNode specifies a MaxNodes property which returns an IEnumerable,
    /// so MaxFunctionEventsNode.MaxNodes returns an instance of this helper</remarks>
    /// <example>foreach(IMaxNode in maxComplexNode.MaxNodes {...}</example>
    /// 
    public class MaxEnumerableNodes: IEnumerable
    {
      public  MaxEnumerableNodes(MaxFunctionEventsNode parent) { this.parent = parent; }
      public  IEnumerator GetEnumerator() { return new NodesEnumerator(this.parent); }
      private MaxFunctionEventsNode parent;

      /// <summary>Iterator over each IMaxNode in the MaxFunctionEventsNode</summary>
      public class NodesEnumerator: IEnumerator
      {
        private int         index;
        private IMaxNode    node;
        private IEnumerator eventNodes;
        private MaxFunctionEventsNode parent;

        public NodesEnumerator(MaxFunctionEventsNode parent) 
        { 
          this.parent = parent;
          this.Reset(); 
        }

        public void   Reset() { this.index = -1; this.node = null; }
        public object Current { get { return node; } }      

        public bool MoveNext()
        {
          if  (++index == 0)                
          {                                 // First access returns
               node       = parent.pnode;   // the function node          
               eventNodes = parent.cnode.Items.GetEnumerator();
          }                                 // Subsequent return event nodes
          else node = eventNodes.MoveNext()? eventNodes.Current as IMaxNode: null;          

          return node != null;
        } // MoveNext()
      }   // class NodesEnumerator
    }     // class MaxEnumerableNodes


    #region IMaxNode Members

    public string  NodeName  
    { get { return nodeName;  } 
      set 
      {                                     // We register subnodes with explorer 
        nodeName = value;                   // individually so function needs a name 
        if (this.pnode != null) this.pnode.NodeName = value;
      } 
    }
    public string  FullName 
    { get { return fullName;  } 
      set 
      { fullName = value;                    
        if (this.pnode != null) this.pnode.FullName = value;
      } 
    } 
    public long      NodeID    { get { return nodeID;    } set { nodeID = value;   } }
    public NodeTypes NodeType  { get { return nodeType;  } }
    public IGoPort   NodePort  { get { return pnode.NodePort; } }
    public MaxCanvas Canvas    { get { return canvas;    } }
    public MaxTool   Tool      { get { return tool;      } }
    public string    GroupName 
    {
      get { return tool.ToolGroup == null? null: tool.ToolGroup.GroupName; } 
    } 

    public virtual void OnLinkCreated(IMaxLink link) 
    { 
      pnode.Port.IsValidFrom = pnode.Port.IsValidTo = false; 
    } 
    public virtual void OnLinkDeleted(IMaxLink link) { }  
    public virtual void CanDeleteLink(object sender, CancelEventArgs e) { e.Cancel = true; }  
     
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

    public Framework.Satellite.Property.DataTypes.Type PmObjectType { get { return pmObjectType; }}

    public void OnPropertiesChangeRaised(MaxProperty[] properties) { }  

    #endregion

    #region MaxObject Members

    public Metreos.Max.Core.ObjectTypes MaxObjectType  { get { return ObjectTypes.Group; } }
    
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

    private bool isBatchRequest;
  } // class MaxFunctionEventsNode
}   // namespace
