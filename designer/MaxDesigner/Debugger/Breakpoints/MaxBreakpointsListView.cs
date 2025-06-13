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
    ///<summary>Debugger breakpoints list view</summary>
    public class MaxBreakpointsListView: MaxDebugListView
    {
        public  MaxBreakpointsWindow parent;
        private const int NumCols = 3;


        public MaxBreakpointsListView(MaxBreakpointsWindow parent): base(NumCols)
        {
            this.parent = parent;
            this.CheckBoxes = true;
        }


        /// <summary>Populate dialog listview</summary>
        public override void Create()
        {
            base.Create();
            int wI = this.Width / 4, wA = this.Width / 3, wF = 1000;

            this.Columns.Add(Const.DbgActionNameColHdr, wA, HorizontalAlignment.Left);
            this.Columns.Add(Const.DbgActionIdColHdr,   wI, HorizontalAlignment.Left);
            this.Columns.Add(Const.DbgFuncNameColHdr,   wF, HorizontalAlignment.Left);
        }


        /// <summary>Add an entry to list</summary>
        public void Add(string function, string action, long actionID, bool check)
        {
            if (this.Find(function, action, actionID) != null) return;
            ListViewItem item = new ListViewItem(action);
            item.SubItems.Add(actionID.ToString());
            item.SubItems.Add(function);
            item.Tag = new ItemData(function, action, actionID);  
            item.Checked = check;
 
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


        /// <summary>Return breakpoints list</summary>
        public ItemData[] Contents()
        {
            ItemData[] data = new ItemData[this.Items.Count];
            int i=0;
            foreach(ListViewItem item in this.Items)
                    data[i++] = item.Tag as ItemData;
            return data;
        }


        /// <summary>Check an entry in list</summary>
        public void Check(string function, string action, long actionID, bool check)
        {
            ListViewItem item = this.Find(function, action, actionID);
            if  (item != null)  item.Checked = check;
        }


        /// <summary>Handle listitem checkbox event</summary>
        protected override void OnItemCheck(ItemCheckEventArgs e)
        {
            base.OnItemCheck(e);
            if (e.Index >= this.Items.Count) return;
            ListViewItem item = this.Items[e.Index];
            ItemData data = item.Tag as ItemData;
            if (data != null) parent.OnBreakpointChecked(data.id, e);
        }
     

        /// <summary>Hook the listbox window procedure</summary>
        protected override void WndProc(ref Message msg)
        {
            switch(msg.Msg)
            {
                case Const.WM_LBUTTONDBLCLK:
                    // A ListViewItem displays the odd behavior of sending a check event
                    // to its checkbox on a double click. Here we prevent that activity.
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

            if  (item == null)
                 this.PopWindowContextMenu();
            else this.PopItemContextMenu(item);      
        }


        ///<summary>Pop list item context menu</summary>
        public void PopItemContextMenu(ListViewItem item)
        {
            PopupMenu contextmenu = new PopupMenu();
            mciGoTo.Tag = mciRemove.Tag = mciEnable.Tag = mciDisable.Tag = item; 
            contextmenu.MenuCommands.Add(mciGoTo); 
            contextmenu.MenuCommands.Add(separator);
            contextmenu.MenuCommands.Add(mciRemove);

            if  (item.Checked)
                 contextmenu.MenuCommands.Add(mciDisable);
            else contextmenu.MenuCommands.Add(mciEnable);

            MenuCommand selection = contextmenu.TrackPopup(Control.MousePosition);
        }


        ///<summary>Pop window background context menu</summary>
        public void PopWindowContextMenu()
        {
            PopupMenu contextmenu = new PopupMenu();
            mcwClearAll.Tag = mcwDisableAll.Tag = null; 
            contextmenu.MenuCommands.Add(mcwClearAll); 
            contextmenu.MenuCommands.Add(mcwDisableAll);
            MenuCommand selection = contextmenu.TrackPopup(Control.MousePosition);
        }


        /// <summary>Sort list column when header clicked</summary>
        protected override void OnColumnClick(object sender, ColumnClickEventArgs e)
        {   
            if (e.Column == 1) return; // Don't sort on ID 
            base.OnColumnClick(sender, e);
        }


        protected override void InitMenus()
        {
            mciGoTo    = new MenuCommand(Const.menuGenericGoTo, 
                         new EventHandler(parent.OnBreakpointGoTo));
            mciRemove  = new MenuCommand(Const.menuNodeRemoveBkpt, 
                         new EventHandler(parent.OnBreakpointRemove));
            mciEnable  = new MenuCommand(Const.menuNodeEnableBkpt, 
                         new EventHandler(parent.OnBreakpointEnable));
            mciDisable = new MenuCommand(Const.menuNodeDisableBkpt, 
                         new EventHandler(parent.OnBreakpointDisable));
   
            MaxDebugUtil debugger = MaxDebugger.Instance.Util;
            mcwClearAll   = new MenuCommand(Const.menuDebugClearBkpts, 
                            new EventHandler(debugger.OnMenuClearBreakpoints));
            mcwDisableAll = new MenuCommand(Const.menuDebugDisableBkpts, 
                            new EventHandler(debugger.OnMenuDisableBreakpoints));   
        }

        private MenuCommand mciGoTo;
        private MenuCommand mciRemove;
        private MenuCommand mciEnable;
        private MenuCommand mciDisable;
   
        private MenuCommand mcwClearAll;
        private MenuCommand mcwDisableAll;

    } // class MaxBreakpointsListView

} // namespace
