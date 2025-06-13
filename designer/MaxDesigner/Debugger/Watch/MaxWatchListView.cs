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
  ///<summary>Debugger call stack list view</summary>
  public class MaxWatchListView: MaxDebugListView
  {
    public  MaxWatchWindowOld parent;
    private const int numCols = 5;


    public MaxWatchListView(MaxWatchWindowOld parent): base(numCols)
    {
      this.parent = parent;
      this.Name   = "watchlist";
    }


    /// <summary>Populate dialog listview</summary>
    public override void Create()
    {
      base.Create();
                           
      this.CreateColumns(); 
    }


    /// <summary>Add column headings</summary>
    public override void CreateColumns()
    {
      int wS = 48, wF = 140, wN = 128, wT = 160, wV = 1000;

      this.Columns.Add(Const.DbgVarScopeColHdr, wS, HorizontalAlignment.Left);
      this.Columns.Add(Const.DbgFuncNameColHdr, wF, HorizontalAlignment.Left);
      this.Columns.Add(Const.DbgVarNameColHdr,  wN, HorizontalAlignment.Left);
      this.Columns.Add(Const.DbgVarTypeColHdr,  wT, HorizontalAlignment.Left);
      this.Columns.Add(Const.DbgVarValueColHdr, wV, HorizontalAlignment.Left);
    }


    /// <summary>Add an entry to list</summary>
    public void Add(string func, string name, string type, string val)
    {
      if (this.Find(func, name) != null) return;
      ListViewItem item = new ListViewItem(func == null? Const.scopeGlobal: Const.scopeLocal);
      item.SubItems.Add(func);
      item.SubItems.Add(name);
      item.SubItems.Add(type);
      item.SubItems.Add(val);
      item.Tag = new ItemData(func, name, type, val);  
 
      this.Items.Add(item);  
    }


    /// <summary>Remove an entry from list</summary>
    public void Remove(string function, string name, string type, string val)
    {
      ListViewItem item = this.Find(function, name); 
      if  (item != null)  this.Items.Remove(item);
    }


    /// <summary>Find specified item in list</summary>
    public ListViewItem Find(string func, string name)
    {
      foreach(ListViewItem item in this.Items)
      {
        ItemData x = item.Tag as ItemData;
        if (x != null && x.func == func && x.name == name) return item;
      }

      return null;
    }


    /// <summary>Sort list column when header clicked</summary>
    protected override void OnColumnClick(object sender, ColumnClickEventArgs e)
    {   
      if (e.Column < 3)                     // Don't sort on type or value
         base.OnColumnClick(sender, e);
    }


    /// <summary>List item tag data</summary>
    public class ItemData
    {
      public ItemData(string f, string n, string t, string v) 
      { func = f; name = n; type = t; val = v; 
      }
      public bool global { get { return func == null; } }
      public string func;
      public string name;
      public string type;
      public string val;
    } 
        
  } // class MaxWatchListView

} // namespace
