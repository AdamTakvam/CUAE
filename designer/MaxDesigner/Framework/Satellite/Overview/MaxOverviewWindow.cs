using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Northwoods.Go;
using Metreos.Max.Core;
using Metreos.Max.Drawing;
using Crownwood.Magic.Menus;



namespace Metreos.Max.Framework.Satellite.Overview
{
    /// <summary>Docking window containing panable overview of current canvas</summary>  
    public class MaxOverviewWindow: Form, MaxSatelliteWindow
    {
        #region singleton
        private MaxOverviewWindow() {}
        private static MaxOverviewWindow instance;
        public  static MaxOverviewWindow Instance
        {
            get 
            {
                if(instance == null)
                {
                    instance = new MaxOverviewWindow();
                    instance.Init();
                }
                return instance;
            }
        }
        #endregion

        private void Init()
        {      
            InitializeComponent();
            this.oview = new GoOverview();
            this.oview.BackColor = SystemColors.Control;
            this.oview.Dock = DockStyle.Fill;
            this.Controls.Add(this.oview);
            this.oview.MouseUp += new MouseEventHandler(this.OnOverviewMouseUp);
                                 
            if (!Config.EnableOverviewDragZoom)  // Disable drag/zoom
                this.oview.ReplaceMouseTool(typeof(GoToolZooming), null);
        }
          
        private System.ComponentModel.Container components = null;
        private GoOverview oview;


        public void ShowOverview(MaxView view)
        {
            this.oview.BackColor = SystemColors.Window;   
            this.oview.Observed  = view;
            this.Show();
        }


        public void ClearOverview()
        {
            this.oview.BackColor = SystemColors.Window;
            this.oview.Observed  = null;
        }


        public void CloseOverview()
        {
            this.oview.BackColor = SystemColors.Control;
            this.oview.Observed  = null;
        }


        private void OnOverviewMouseUp(object sender, MouseEventArgs e)
        {
            switch(e.Button)                     
            {  
               case MouseButtons.Right:  
                    PopupMenu contextmenu = new PopupMenu();
                    contextmenu.MenuCommands.Add(MaxMenu.menuViewZoom);             
                    contextmenu.TrackPopup(Control.MousePosition);
                    return;
            }

            base.OnMouseUp (e);
        }

 
        protected override void Dispose( bool disposing )
        {
            if (disposing) if (components != null) components.Dispose();
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            // 
            // MaxOverview
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MaxOverview";
            this.Text = "";

        }
        #endregion

        #region MaxSatelliteWindow Members

        public Metreos.Max.Framework.Satellite.SatelliteTypes SatelliteType
        {
            get { return SatelliteTypes.Overview; }
        }

        public Crownwood.Magic.Menus.MenuCommand ViewMenuItem 
        { 
            get { return MaxMenu.menuViewOverview; } 
        }

        #endregion

    } // class MaxOverview

}   // namespace
