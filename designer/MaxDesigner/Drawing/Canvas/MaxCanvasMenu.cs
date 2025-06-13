using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Framework;
using Metreos.Max.Manager;
using Metreos.Max.Core;
using Metreos.Max.Drawing;
using Metreos.Max.GlobalEvents;
using Metreos.Max.Framework.Satellite.Property;
using Crownwood.Magic.Menus;



namespace Metreos.Max.Drawing
{
    /// <summary>Canvas background context menus</summary>
    public class MaxCanvasMenu
    {
        public MaxCanvasMenu()
        {
            InitMenu();
        }

        public void PopFunctionCanvasContextMenu(MouseEventArgs e)
        {
            PopupMenu contextmenu = new PopupMenu();
            contextmenu.MenuCommands.Add(menuTray);
            contextmenu.MenuCommands.Add(menuSep1);
            contextmenu.MenuCommands.Add(menuCanv);     
            contextmenu.MenuCommands.Add(menuZoom);
            contextmenu.MenuCommands.Add(menuGrid); 
            contextmenu.MenuCommands.Add(menuSep1);
            contextmenu.MenuCommands.Add(menuProp);

            menuGrid.Checked = MaxCanvas.IsGridShown;
     
            menuTray.Checked = canvas.VtrayManager.TrayState 
                == MaxVariablesManager.TrayStates.Shown; 

            contextmenu.TrackPopup(canvas.View.PointToScreen(new Point(e.X,e.Y)));
        }


        protected void InitMenu()
        {
            menuViewZoomIn    = new MenuCommand(Const.menuViewZoomIn,  
                                new EventHandler(OnMenuZoomIn));
            menuViewZoomOut   = new MenuCommand(Const.menuViewZoomOut, 
                                new EventHandler(OnMenuZoomOut));
            menuViewZoomNorm  = new MenuCommand(Const.menuViewZoomNorm,
                                new EventHandler(OnMenuZoomNormal)); 

            menuZoom  = new MenuCommand(Const.menuViewZoom); 
            menuZoom.MenuCommands.Add(menuViewZoomIn);
            menuZoom.MenuCommands.Add(menuViewZoomOut);
            menuZoom.MenuCommands.Add(menuViewZoomNorm);
 
            menuGrid  = new MenuCommand(Const.menuViewGrid, 
                        new EventHandler(OnMenuGrid));  
            menuCanv  = new MenuCommand(Const.menuViewCanvas, 
                        new EventHandler(OnMenuCanvasOnly)); 
            menuSep1  = new MenuCommand(Const.dash);  
            menuTray  = new MenuCommand(Const.menuViewTray, 
                        new EventHandler(OnMenuTray));  
            menuProp  = new MenuCommand(Const.menuGenericProperties, 
                        new EventHandler(OnMenuProp)); 
        }


        public void OnMenuZoomIn(object sender, EventArgs e)
        {
            MaxProject.Instance.OnZoomRequest(1);
        }


        public void OnMenuZoomOut(object sender, EventArgs e)
        {
            MaxProject.Instance.OnZoomRequest(-1);
        }


        public void OnMenuZoomNormal(object sender, EventArgs e)
        {
            MaxProject.Instance.OnZoomRequest(0);
        }


        public void OnMenuGrid(object sender, EventArgs e)
        {
            MaxProject.Instance.OnGridRequest(MaxCanvas.IsGridShown? 0: 1);
        }


        public void OnMenuCanvasOnly(object sender, EventArgs e)
        {
            menuCanv.Checked = !menuCanv.Checked;

            if  (menuCanv.Checked)  
                 MaxManager.Instance.DockMgr.HideAllContents();
            else MaxManager.Instance.DockMgr.ShowAllContents(); 
        }


        public void OnMenuTray(object sender, EventArgs e)
        {
            menuTray.Checked = !menuTray.Checked;
            canvas.VtrayManager.Show(menuTray.Checked);

            canvas.Project.SignalMenuActivity(new MaxMenuOutputEventArgs
                (MaxMenuOutputEventArgs.MaxEventTypes.IsTrayShown, menuTray.Checked)); 
        }


        public void OnMenuProp(object sender, EventArgs e)
        {
            MaxApp app = MaxProject.CurrentApp; if (app == null) return;
            Max.Framework.Satellite.Property.PmProxy.ShowProperties(app, app.PmObjectType);  
            MaxProject.Instance.ShowPropertiesWindow();     
        }

        protected MaxFunctionCanvas canvas;
        public    MaxFunctionCanvas Canvas { set { canvas = value; } }

        protected MenuCommand menuZoom;
        protected MenuCommand menuGrid;
        protected MenuCommand menuCanv;
        protected MenuCommand menuTray;
        protected MenuCommand menuProp;
        protected MenuCommand menuSep1;

        protected MenuCommand menuViewZoomIn; 
        protected MenuCommand menuViewZoomOut; 
        protected MenuCommand menuViewZoomNorm; 
      
    } // class MaxCanvasMenu
}   // namespace