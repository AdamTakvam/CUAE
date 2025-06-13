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
  /// <summary>The child event list node in a MaxFunctionEventsNode group</summary>
  public class EventList: GoListGroup
  {
    public EventList(MaxFunctionEventsNode parent)
    {
      this.parent     = parent;
      this.items      = new ArrayList();
      this.DragsNode  = true;          
      this.Deletable  = false;
      this.Selectable = false;  
      this.Resizable  = true;        
      this.Spacing    = 1;      
      this.Alignment  = GoObject.MiddleLeft;        
      this.TopLeftMargin     = new SizeF(3F,0F);
      this.BottomRightMargin = new SizeF(5F,1F);
      this.ResizesRealtime   = false; 
      this.BorderPen = null;       
      this.Brush = Brushes.Transparent;  
    }

    protected ArrayList items;
    public    ArrayList Items { get { return items; } }


    /// <summary>Add an event entry to the list</summary>
    public EventListItem AddItem(string qualifiedEventName)
    {      
      MaxTool tool = MaxManager.Instance.Packages.FindByToolName(qualifiedEventName);
      if  (tool == null) return null;

      EventListItem listEntry = new EventListItem(this, tool);   

      this.Add(listEntry);
      // if (this.FindByName(qualifiedEventName) != null) MessageBox.Show("duplicate");                 
      this.items.Add(listEntry);

      return listEntry;
    }


    /// <summary>Remove an event entry from the list</summary>
    public void RemoveItem(long nodeID)
    {
      EventListItem item = this.FindByNodeID(nodeID);
      if  (item != null)
           this.items.Remove(item);         
    }


    /// <summary>Add or remove empty slots following this one</summary>
    /// <remarks>Propagates backward to event lists earlier in chain</remarks>
    public void Normalize(EventListItem entry, int count)
    {
      int  index  = this.IndexOf(entry);
      if  (index == -1 || count == 0) return;

      if  (count < 0)
           this.BubbleUp  (index, count);
      else this.BubbleDown(index, count);

      parent.Render();

      // Tell whatever event our host node is the handler FOR, to normalize ITS list
      EventListItem parentEvent = this.parent.HandlerFor as EventListItem;
      if  (parentEvent != null)
           parentEvent.NormalizeList(count);
    }
      

    /// <summary>Move a block from slotindex+1 to end down newSlotCount slots</summary>
    public void BubbleDown(int slotindex, int newSlotCount)
    {
      newSlotCount++;                     // Appearance requires one extra slot   
      int lastindex = this.Count - 1, i, j;
      int firstEmptySlot = slotindex + 1;

      // 1. Save off entries to be displaced
      int savedItemsCount   = Math.Min(newSlotCount, lastindex - slotindex); 
      GoObject[] savedItems = new GoObject[savedItemsCount];
        
      for(i = firstEmptySlot, j=0;  i < firstEmptySlot + savedItemsCount; i++, j++)
          savedItems[j] = this[i] as GoObject;

      // 2. Insert newSlotCount slots following slotindex
      GoObject placeholder = new NullItem(), lastplaceholder = placeholder;
      this.InsertAfter(this[slotindex], placeholder);

      for(i = firstEmptySlot; i < slotindex + newSlotCount; i++)
      {
          placeholder = new NullItem();
          this.InsertAfter(lastplaceholder, placeholder);
          lastplaceholder = placeholder;
      }

      // 3. Insert the saved list slots following the empty slots created above
      GoObject currentObject = lastplaceholder;
 
      for(i = 0; i <  savedItemsCount; i++) 
      {
          GoObject savedObject = savedItems[i];
          EventListItem item = savedObject as EventListItem;
          MaxBasicLink  link = this.SalvageLink(item, null);

          this.InsertAfter(currentObject, savedObject); 
          currentObject = savedObject;

          // When we move the event list entry, the link becomes detached
          // and we cannot relink it for some reason. For now, we create a
          // new link, but we should find out why this occurs and fix it
          if  (item != null && link != null)  
               parent.CreateLink(item, link.ToNode as MaxIconicNode);
      }       
    }  // BubbleDown()


    /// <summary>Move a block from index+count+1 to end up n slots</summary>
    public void BubbleUp(int index, int count)
    {
      for(int i = index + count + 1; i < this.Count; i++)
          this[i - count] = this[i];

      // No doubt we'll have to recreate the link here as well

      for(int i = index + count + 1; i < this.Count; i++)
          this.RemoveAt(i);
    } 


    /// <summary>Link gets lost when shift event in list; save and replace it</summary>
    /// <remarks>Called twice, first time to save link, second to restore link</remarks>
    public MaxBasicLink SalvageLink(EventListItem item, MaxBasicLink xlink)
    {
      MaxBasicLink link = null;
      if  (xlink == null && item != null) // First time      
           link = Utl.FirstLink(item.Port) as MaxBasicLink; 
      else link = xlink;

      if  (xlink != null && link != null) // Second time
      {
           link.Location = new PointF(link.Location.X, item.Location.Y);
           try { parent.Add(link); } catch { }
      }

      return link;
    }


    /// <summary>Find maximum width of list items/summary>
    public float MaxWidth()
    {
      float max = 0;
      foreach(object o in this)
      {
          EventListItem item = o as EventListItem;
          if (item != null && item.Label != null && item.Label.Width > max) 
              max = item.Label.Width;
      } 
      return max;   
    }


    /// <summary>Delete subnode only when parent node is selection</summary>
    public override bool CanDelete()
    {
      return parent.Canvas.View.Selection.Primary == parent.Pnode;
    }

    // <summary>Return event in list matching supplied node ID</summary>
    public EventListItem FindByNodeID(long id)
    {
      foreach(object x in items)
      {
        EventListItem item = x as EventListItem;
        if  (item != null && item.NodeID == id) return item;
      }
      return null;
    }


    // <summary>Return first event in list matching supplied name</summary>
    public EventListItem FindByName(string name)
    {
      string unqualifiedName = Utl.StripQualifiers(name);
      foreach(object x in items)
      {
        EventListItem item = x as EventListItem;
        if  (item != null && item.NodeName == unqualifiedName) return item;
      }
      return null;
    }

    public override void AddSelectionHandles(GoSelection sel, GoObject selectedObj) { }

    private MaxFunctionEventsNode parent;
    public  MaxFunctionEventsNode HostNode { get { return parent; } }

  } // class EventList




  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  // EventListItem
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

  /// <summary>A slot entry in the event list</summary>
  public class EventListItem: MaxRecumbentEventNode 
  {
    public EventListItem(EventList parent, MaxTool tool): 
      base(parent.HostNode.Canvas, tool)
    {  
      this.list = parent;
      this.CanLinkIn  = this.CanLinkOut = false; 
      this.Selectable = false;  
      this.DragsNode  = true;
      this.Label.TextColor = Color.FromArgb(96,96,100);
    }


    /// <summary>Move port to end of label</summary>
    public override void LayoutChildren(GoObject childchanged)
    {
      base.LayoutChildren(childchanged);
      if  (Port != null && Label != null)
           Port.Location = new PointF(Port.Location.X + Label.Width + 8, Port.Location.Y);        
    }


    /// <summary>Callback from handler to adjust list</summary>
    public void NormalizeList(int entrycount) 
    { 
      this.list.Normalize(this, entrycount); 
    }


    /// <summary>Return one and only link</summary>
    public MaxBasicLink Link { get { return Utl.FirstLink(this.Port) as MaxBasicLink; } }

    /// <summary>Disallow link delete</summary>
    public override void CanDeleteLink(object sender, CancelEventArgs e) { e.Cancel = true; }

    private EventList list;                // This event's host list
    private MaxFunctionEventsNode handler; // This event's handler's group node
    public  MaxFunctionEventsNode Handler { get{return handler;} set{handler = value; }}

  } // class EventListItem


  /// <summary>A placeholder event list item</summary>
  public class NullItem: GoObject { }  

} // namespace
