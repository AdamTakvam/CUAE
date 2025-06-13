using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using Northwoods.Go;
using Crownwood.Magic.Menus;
using Metreos.Max.Core;
using Metreos.Max.Manager;
using Metreos.Max.Drawing;
using Metreos.Max.Framework;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework.Satellite;



namespace Metreos.Max.Debugging
{
    ///<summary>Debugger breakpoints window frame</summary>
    public class MaxBreakpointsWindow: Form, MaxSatelliteWindow
    {
        private MaxBreakpointsListView list;    
        public  MaxBreakpointsListView List { get { return list; } }
        private System.ComponentModel.Container components = null;


        public MaxBreakpointsWindow()
        {
            InitializeComponent();
        }

        /// <summary>Post-construction initialization</summary>
        public void Init()
        {
            this.list.Create();
        }


        public void CheckAll(bool check)
        {
            foreach(ListViewItem item in this.List.Items) item.Checked = check;
        }


        public int Count { get { return this.List.Items.Count; } }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        //  Context menu handlers
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                        
        /// <summary>Invoked on Go To selected from context menu</summary>
        public void OnBreakpointGoTo(object sender, EventArgs e)
        {       
            MenuCommand mc = sender as MenuCommand;
            ListViewItem item = mc.Tag as ListViewItem;
            MaxBreakpointsListView.ItemData data = item.Tag as MaxBreakpointsListView.ItemData;

            this.OnListItemDoubleClick(data.id); 
        }


        /// <summary>Invoked on Remove Breakpoint selected from context menu</summary>
        public void OnBreakpointRemove(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            ListViewItem item = mc.Tag as ListViewItem;
            MaxBreakpointsListView.ItemData data = item.Tag as MaxBreakpointsListView.ItemData;

            Max.Manager.MaxNodeInfo info = MaxManager.Instance.FindNode(data.id);
            if (info != null && info.node != null)  
                MaxDebugger.Instance.Util.OnToggleBreakpoint(info.node);
        }


        /// <summary>Invoked on Enable Breakpoint selected from context menu</summary>
        public void OnBreakpointEnable(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            ListViewItem item = mc.Tag as ListViewItem;
            MaxBreakpointsListView.ItemData data = item.Tag as MaxBreakpointsListView.ItemData;
            this.ToggleEnabled(data.function, data.action, data.id);       
        }


        /// <summary>Invoked on Disable Breakpoint selected from context menu</summary>
        public void OnBreakpointDisable(object sender, EventArgs e)
        {
            this.OnBreakpointEnable(sender, e); 
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        //  Other methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                                 
 
        private void InitializeComponent()
        {
            this.SuspendLayout();  
            this.Name = this.Text = Const.BreakpointsWindowTitle;
            this.list = new MaxBreakpointsListView(this);
            this.Controls.Add(this.list);
            this.ResumeLayout(false);
        }


        /// <summary>Act on double click of a breakpoint list item</summary>
        public void OnListItemDoubleClick(long nodeID)
        {
            MaxManager.Instance.NavigateToNode(nodeID, true);
        }


        /// <summary>Toggle breakpoint on or off</summary>
        public bool ToggleBreakpoint(string function, string action, long actionID)
        {
            ListViewItem  item = list.Find(function, action, actionID);
            bool result = item == null;

            if  (result) 
                 list.Add(function, action, actionID, true);
            else list.Remove(function, action, actionID);

            return result;
        }


        /// <summary>Toggle breakpoint enabled state on or off</summary>
        public bool ToggleEnabled(string function, string action, long actionID)
        {
            ListViewItem  item = list.Find(function, action, actionID);
            bool result = item == null;

            if  (!result) 
                 item.Checked = !item.Checked;

            return result;
        }


        /// <summary>Act on breakpoint list item checkbox activity</summary>
        public void OnBreakpointChecked(long nodeID, ItemCheckEventArgs e)
        {
            Max.Manager.MaxNodeInfo info = MaxManager.Instance.FindNode(nodeID);
            if (info == null || info.node == null) return;

            Max.Drawing.MaxActionNode actionNode = info.node as Max.Drawing.MaxActionNode;
            MaxIconicMultiTextNode    multiNode  = info.node as MaxIconicMultiTextNode;

            if  (actionNode != null)
                 actionNode.EnableBreakpoint(e.NewValue == CheckState.Checked);
            else
            if  (multiNode  != null)
                 multiNode.EnableBreakpoint (e.NewValue == CheckState.Checked);
        }


        protected override void Dispose(bool disposing)
        {
            if  (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }  

        #region MaxSatelliteWindow Members

        public Metreos.Max.Framework.Satellite.SatelliteTypes SatelliteType
        {
            get{ return SatelliteTypes.Breakpoints; }
        }

        public Crownwood.Magic.Menus.MenuCommand ViewMenuItem 
        { 
            get { return MaxMenu.menuDebugWindowsBkpts; } 
        }
        #endregion

    } // class MaxBreakpointsWindow

}  // namespace


