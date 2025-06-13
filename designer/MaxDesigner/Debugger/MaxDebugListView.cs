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
    ///<summary>Debugger window list view base class</summary>
    public class MaxDebugListView: ListView
    {
        private ToolTip tooltip;
        protected int MaxCols = 1;    
        protected SortOrder[] sortInfo;


        public MaxDebugListView(int maxcols)
        {
            this.Name = "list";
            this.Dock = DockStyle.Fill;
            this.TabIndex = 0;
            this.tooltip  = new ToolTip();
            this.MultiSelect = false;
            this.Size = new Size(800, 200);
            this.View = System.Windows.Forms.View.Details;
            this.ColumnClick += new ColumnClickEventHandler(this.OnColumnClick);

            MaxCols  = maxcols;
            sortInfo = new SortOrder[maxcols];
        }


        /// <summary>Populate dialog listview</summary>
        public virtual void Create()
        {
            this.InitMenus();
        }


        /// <summary>Add column headings</summary>
        public virtual void CreateColumns()
        {    
        }


        /// <summary>Sort list column when header clicked</summary>
        protected virtual void OnColumnClick(object sender, ColumnClickEventArgs e)
        {   
            SortOrder so = sortInfo[e.Column] == SortOrder.Ascending? 
                SortOrder.Descending: SortOrder.Ascending;
            sortInfo[e.Column] = so;        // Toggle sort order
  
            this.ListViewItemSorter = new Comparer(e.Column, so);
            this.Sort();
        }


        /// <summary>Comparison object for listview column sort</summary>
        class Comparer: IComparer 
        {
            private int colno;
            private SortOrder sortOrder;

            public Comparer(int column, SortOrder order) 
            {
                colno = column; sortOrder = order;
            }

            public int Compare(object a, object b) 
            {
                int result = String.Compare
                    (((ListViewItem)a).SubItems[colno].Text, 
                     ((ListViewItem)b).SubItems[colno].Text);

                return sortOrder == SortOrder.Ascending? result: 0 - result;
            }
        }   
 

        protected virtual void InitMenus()
        {      
        }

        public static MenuCommand separator = new MenuCommand(Const.dash);

    } // class MaxDebugListView

} // namespace
