using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Northwoods.Go;
using Crownwood.Magic.Menus;
using Metreos.Max.Core;
using Metreos.Max.Resources.Images;
using Metreos.Max.GlobalEvents;


          
namespace Metreos.Max.Debugging
{
  ///<summary>Debugger CallStack list view</summary>
  public class MaxCallStackListView: MaxDebugListView
  {
    public  MaxCallStackWindow parent;
    private const int NumCols = 3;


    public MaxCallStackListView(MaxCallStackWindow parent): base(NumCols)
    {
      this.parent = parent;
    }


    /// <summary>Populate dialog listview</summary>
    public override void Create()
    {
      base.Create();
      int wI = this.Width / 4, wA = this.Width / 3, wF = 1000;

      this.Columns.Add(Const.DbgActionNameColHdr, wA, HorizontalAlignment.Left);
      this.Columns.Add(Const.DbgActionIdColHdr,   wI, HorizontalAlignment.Left);
      this.Columns.Add(Const.DbgFuncNameColHdr,   wF, HorizontalAlignment.Left);

      this.Add("DoSomething", "MakeCall", 632262852930781961); 
      this.Add("DoSomething", "CallFunction", 632262852930781962); 
      this.Add("DoSomethingElse", "Hangup", 632262961928593969); 
    }


    /// <summary>Add an entry to list</summary>
    public void Add(string function, string action, long actionID)
    {
      if (this.Find(function, action, actionID) != null) return;
      ListViewItem item = new ListViewItem(action);
      item.SubItems.Add(actionID.ToString());
      item.SubItems.Add(function);
      item.Tag = new ItemData(function, action, actionID);  
 
      this.Items.Add(item);  
    }


    /// <summary>Remove an entry from list</summary>
    public void Remove(string function, string action, long actionID)
    {
      ListViewItem item = this.Find(function, action, actionID);
      if  (item != null)  this.Items.Remove(item);
    }


    /// <summary>Find specified item in list</summary>
    public ListViewItem Find(string function, string action, long actionID)
    {
      foreach(ListViewItem item in this.Items)
      {
        ItemData x = item.Tag as ItemData;
        if (x != null && x.function == function && x.action == action && x.id == actionID) 
            return item;
      }

      return null;
    }


    /// <summary>Hook the listbox window procedure</summary>
    protected override void WndProc(ref Message msg)
    {
      switch(msg.Msg)
      {
        case Const.WM_LBUTTONDBLCLK:
          // Here we ensure that the only side effects of a double click
          // are those that we wish to occur
          this.OnDoubleClick((int)msg.LParam);                                 
          return;       
      }

      base.WndProc(ref msg);
    }


    /// <summary>Act on list item double click</summary>
    protected void OnDoubleClick(int xy) // (EventArgs e)
    {
      int x = xy & 0xffff, y = (xy & 0x0fff0000) >> 16;
      ItemData data = null;
      ListViewItem item = this.GetItemAt(x,y);
      if (item != null) data = item.Tag as ItemData;
      if (data != null) parent.OnListItemDoubleClick(data.id);      
    }


    /// <summary>Sort list column when header clicked</summary>
    protected override void OnColumnClick(object sender, ColumnClickEventArgs e)
    {   
      // Don't sort on any columns, since call stack is ordered by definition 
    }


    /// <summary>List item tag data</summary>
    public class ItemData
    {
      public ItemData(string f, string a, long i) { function=f; action=a; id=i; }
      public string function;
      public string action;
      public long   id;
    } 


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Context menus
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    protected override void OnMouseUp(MouseEventArgs e)
    {
      base.OnMouseUp(e);
      if (e.Button != MouseButtons.Right) return;

      ListViewItem item = this.GetItemAt(e.X, e.Y);

      if (item != null) this.PopItemContextMenu(item);      
    }


    ///<summary>Pop list item context menu</summary>
    public void PopItemContextMenu(ListViewItem item)
    {
      PopupMenu contextmenu = new PopupMenu();
      mciGoTo.Tag = item; 
      contextmenu.MenuCommands.Add(mciGoTo); 

      MenuCommand selection = contextmenu.TrackPopup(Control.MousePosition);
    }


    protected override void InitMenus()
    {
      mciGoTo = new MenuCommand(Const.menuGenericGoTo, 
                new EventHandler(parent.OnBreakpointGoTo));  
    }

    private MenuCommand mciGoTo;

  } // class MaxCallStackListView

} // namespace
