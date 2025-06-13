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
  public class MaxConsoleListView: ListView
  {
    public  MaxConsoleWindow parent;
    private ToolTip tooltip;


    public MaxConsoleListView(MaxConsoleWindow parent)
    {
      this.parent = parent;
      this.CheckBoxes = true;
      this.Dock       = DockStyle.Fill;
      this.Location   = new Point(0, 0);
      this.TabIndex   = 0;
      this.tooltip     = new ToolTip();
    }


    /// <summary>Populate dialog listview</summary>
    private void Create()
    {
      int wF = this.Width / 4, wA = this.Width / 2, wI = this.Width / 4;

      this.Columns.Add(Const.DbgFuncNameColHdr,   wF, HorizontalAlignment.Left);
      this.Columns.Add(Const.DbgActionNameColHdr, wA, HorizontalAlignment.Left);
      this.Columns.Add(Const.DbgActionIdColHdr,   wI, HorizontalAlignment.Left);
      
      ListViewItem x = new ListViewItem("DoSomething");
      x.SubItems.Add("ActionOne");
      x.SubItems.Add("01234567890123456789");
      x.Tag = null;   
      this.Items.Add(x);  

      ListViewItem y = new ListViewItem("DoSomething");
      y.SubItems.Add("ActionTwo");
      y.SubItems.Add("98765432109876543210");
      y.Tag = null;   
      this.Items.Add(y);    

      this.Items[0].Selected = true;  
    }
        
  } // class MaxConsoleListView

} // namespace
