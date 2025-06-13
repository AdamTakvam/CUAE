using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Crownwood.Magic.Menus;     
using Metreos.Max.Core;          
using Metreos.Max.Core.Tool;     
using Metreos.Max.Manager;       
using Metreos.Max.Framework.Satellite.Property;



namespace Metreos.Max.Framework.Satellite.Toolbox
{
    public class MaxToolboxMenu
    {
        MaxToolboxWindow toolbox;  

        public MaxToolboxMenu(MaxToolboxControl ctrl)
        {			
            toolbox = ctrl as MaxToolboxWindow;
        }


        public void PopTabContextMenu()    
        {
            MaxToolboxTab tab = toolbox.ContextTab;

            PopupMenu popup = new PopupMenu();
            MenuCommand mc1 = new MenuCommand(Const.menuTabDelete, new EventHandler(OnDeleteTab));
            mc1.Enabled = tab.OkToDelete;

            MenuCommand mc2 = new MenuCommand(Const.menuTabRename, new EventHandler(OnRenameTab));
            MenuCommand mc3 = new MenuCommand(Const.dash);
            MenuCommand mc4 = new MenuCommand(Const.menuTabAddRem, new EventHandler(OnAddRemoveItems));
      
            MenuCommand mc5 = new MenuCommand(Const.menuTabAdd,    new EventHandler(OnAddTab));
            MenuCommand mc6 = new MenuCommand(Const.menuTabShowAll,new EventHandler(OnShowAllTabs));
            MenuCommand mc7 = new MenuCommand(Const.dash);
            MenuCommand mc8 = new MenuCommand(Const.menuTabMoveUp, new EventHandler(OnMoveUp));
            MenuCommand mc9 = new MenuCommand(Const.menuTabMoveDn, new EventHandler(OnMoveDown));

            mc4.Enabled = mc5.Enabled = toolbox.Main.ProjectExists;
            mc6.Visible = false;

            mc1.Visible = mc2.Visible = mc3.Visible 
                = toolbox.Main.ProjectExists && toolbox.ContextTab.OkToDelete;
      
            MenuCommand[] mcs = new MenuCommand[] { mc1, mc2, mc3, mc4, mc5 }; // mc6, mc7, mc8, mc9 };

            popup.MenuCommands.AddRange(mcs);

            Point pt  = Control.MousePosition;  
     
            popup.TrackPopup(pt);
        }


        protected void OnDeleteTab(object sender, EventArgs e)
        {
            if (toolbox.ContextTab != null) toolbox.RemoveToolboxTab();
        }


        protected void OnRenameTab(object sender, EventArgs e)
        {     
            if (toolbox.ContextTab != null) 
                toolbox.Rename(toolbox.ContextTab);
        }


        public void OnAddRemoveItems(object sender, EventArgs e)
        {
            MaxToolboxTab tab = e == null? toolbox.OpenTab: toolbox.ContextTab;
            if (tab == null) return;
       
            new Max.Framework.MaxCustomizeDlg(toolbox, tab).ShowDialog();
        }


        protected void OnAddTab(object sender, EventArgs e)
        {
            toolbox.AddToolboxTab();
        }


        protected void OnShowAllTabs(object sender, EventArgs e)
        {
        }


        protected void OnMoveUp(object sender, EventArgs e)
        {
            int i = toolbox.Tabs.IndexOf(toolbox.ContextTab); if (i < 1) return;
            toolbox.Tabs[i]   = toolbox.Tabs[i-1];
            toolbox.Tabs[i-1] = toolbox.ContextTab;
            toolbox.Refresh();
        }


        protected void OnMoveDown(object sender, EventArgs e)
        {
            int i = toolbox.Tabs.IndexOf(toolbox.ContextTab);
            if (i < 0 || i > toolbox.Tabs.Count) return;      
            toolbox.Tabs[i]   = toolbox.Tabs[i+1];
            toolbox.Tabs[i+1] = toolbox.ContextTab;
            toolbox.Refresh();
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        // Item context menu
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public void PopItemContextMenu(MaxToolboxItem item)      
        {     
            MaxTool tool = item.Tag as MaxTool;
            bool canDelete = tool != null && tool.Deletable;

            PopupMenu popup = new PopupMenu();
            MenuCommand mc1 = new MenuCommand(Const.menuToolDelete, new EventHandler(OnItemDelete));
            MenuCommand mc3 = new MenuCommand(Const.dash);
            MenuCommand mc4 = new MenuCommand(Const.menuToolAddRem, new EventHandler(OnItemAddRemove));

            mc1.Tag = mc3.Tag = mc4.Tag = item;

            mc3.Enabled = mc4.Enabled = toolbox.Main.ProjectExists;

            mc1.Enabled = canDelete  && toolbox.Main.ProjectExists;

            MenuCommand[] mcs = new MenuCommand[] { mc1, mc3, mc4 };

            popup.MenuCommands.AddRange(mcs);

            Point pt = Control.MousePosition;  
     
            popup.TrackPopup(pt);
        }


        /// <summary>Remove selected tool from toolbox control and internal tool group</summary>
        protected void OnItemDelete(object sender, EventArgs e)
        {
            MaxTool tool  = null;
            MaxToolboxItem item = null;
            MenuCommand mc = sender as MenuCommand;
            if (mc    != null) item = mc.Tag as MaxToolboxItem;
            if (item  != null) tool = item.Tag as MaxTool;
            if (tool  != null  && tool.Deletable) 
                toolbox.RemoveToolFromGroup(tool);
        }
   

        /// <summary>Add/Remove Items selected from item menu</summary>
        protected void OnItemAddRemove(object sender, EventArgs e)
        {
            this.OnAddRemoveItems(sender, e);
        }

    } // class MaxToolboxMenu
}   // namespace
