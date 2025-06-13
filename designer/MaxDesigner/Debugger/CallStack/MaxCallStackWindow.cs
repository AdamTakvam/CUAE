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
    ///<summary>Debugger call stack window frame</summary>
    public class MaxCallStackWindow: Form, MaxSatelliteWindow
    {
        private MaxCallStackListView list;    
        public  MaxCallStackListView List { get { return list; } }
        private System.ComponentModel.Container components = null;


        public MaxCallStackWindow()
        {
            InitializeComponent();
        }


        /// <summary>Post-construction initialization</summary>
        public void Init()
        {
            this.list.Create();
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        //  Context menu handlers
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                        
        /// <summary>Invoked on Go To selected from context menu</summary>
        public void OnBreakpointGoTo(object sender, EventArgs e)
        {       
            MenuCommand mc = sender as MenuCommand;
            ListViewItem item = mc.Tag as ListViewItem;
            MaxCallStackListView.ItemData data = item.Tag as MaxCallStackListView.ItemData;

            this.OnListItemDoubleClick(data.id); 
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        //  Other methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Clear all entries from list</summary>
        public void Clear()
        {
            this.list.Items.Clear();     
        }


        /// <summary>Actions on double click of a call stack entry</summary>
        public void OnListItemDoubleClick(long nodeID)
        {
            MaxManager.Instance.NavigateToNode(nodeID, true);
        }
                                          

        private void InitializeComponent()
        {
            this.SuspendLayout();  
            this.Name = this.Text = Const.CallStackWindowTitle;
            this.list = new MaxCallStackListView(this);
            this.Controls.Add(this.list);
            this.ResumeLayout(false);
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
            get { return MaxMenu.menuDebugWindowsCalls; } 
        }
        #endregion

    } // class MaxCallStackWindow

}  // namespace


