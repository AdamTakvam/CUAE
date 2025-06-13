//
// MaxToolboxItem.cs
// Toolbox tool object
//
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Metreos.Max.Drawing;
using Metreos.Max.Framework;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Resources.Images;
using Crownwood.Magic.Menus;



namespace Metreos.Max.Framework.Satellite.Toolbox
{
   
    public class MaxToolboxWindow: MaxToolboxControl, MaxSatelliteWindow  
    {
        public static MaxToolboxWindow ToolboxControl;
    
        protected ArrayList toolGroups;
        public    ArrayList ToolGroups { get { return toolGroups;   } }

        private   MaxMain   main;
        public    MaxMain   Main { get { return main; } }

    
        public MaxToolboxWindow(MaxMain main)     
        {
            this.main = main;
            ToolboxControl = this;
        }


        public MaxToolboxWindow(MaxMain main, ArrayList maxToolGroups)     
        {
            this.main = main;
            ToolboxControl = this;
            this.LoadToolbox(maxToolGroups);
        }
    

        public MaxToolGroup FindByGroupName(string name)   
        {
            foreach(MaxToolGroup toolGroup in this.toolGroups)
                if (toolGroup.GroupName == name) return toolGroup;
            return null;
        }


        public MaxToolGroup FindGroupByTool(MaxTool tool)  
        {
            foreach(MaxToolGroup toolGroup in this.toolGroups)
                if (toolGroup.Tools.Contains(tool)) return toolGroup;
            return null;
        }


        public void LoadToolbox(ArrayList maxToolGroups)     
        {
            this.toolGroups = maxToolGroups;

            toolboxTabAreaControl.MouseUp   += new MouseEventHandler(SetItemContextMenu);
            toolboxTabAreaControl.MouseDown += new MouseEventHandler(HandleItemFocus);    

            foreach(MaxToolGroup toolGroup in maxToolGroups)
            { 
                MaxToolboxTab tab = this.AddToolboxTab(toolGroup.GroupText);

                foreach (MaxTool tool in toolGroup.Tools) 
                {
                    string name = Utl.StripQualifiers(tool.Name);                  

                    // Note that tool is saved in the toolbox item's Tag member
                    MaxToolboxItem item = MaxToolboxItem.NewToolboxEntry  
                           (tool.DisplayName, tool, tool.ImagesSm[tool.ImageIndexSm]);
                    if (item == null) continue;

                    tab.Items.Add(item);   
                }               

                this.OpenTab = tab;               // Default to last group 
            }
        }   


        public void AddToolboxTab()   
        {
            MaxToolboxTab tab = new MaxToolboxTab(this, this.GetNewTabDefaultName());

            Tabs.Add(tab);

            MarkProjectDirty();
            this.isNewToolboxTab = true;
            this.Rename(tab); // see MaxToolboxControl
            Refresh();
        }  


        public MaxToolboxTab AddToolboxTab(string text)
        {
            MaxToolboxTab tab = new MaxToolboxTab(this, text);
            tab.OkToDelete = tab.Name != Const.AppComponentsTabName;
            Tabs.Add(tab);
            this.Refresh();  
            return tab;
        }


        /// <summary>Remove a toolbox tab, its local tool group, and all its tools</summary>
        public bool RemoveToolboxTab(string tabname)   
        {
            MaxToolboxTab tab = this.FindTabByName(tabname);
            return (tab == null || !tab.OkToDelete)? false: this.RemoveToolboxTab(tab);
        }


        /// <summary>Remove a toolbox tab, its local tool group, and all its tools</summary>
        public void RemoveToolboxTab()   
        {
            // We currently do not remove the dll reference from the project if the
            // last tool using it is deleted. At some point we'll want to add that
            // logic. We'll need to run thru this.toolGroups and check if any tools
            // still refer to the dll, and if not, we can ask explorer to remove
            // the reference. 

            MaxToolboxTab tab = this.ContextTab == null? this.OpenTab: this.ContextTab;
            if  (tab == null || !tab.OkToDelete) return;

            MaxToolGroup toolgroup = this.FindByGroupName(tab.Name); 

            DialogResult result = (tab.Items.Count == 0)? DialogResult.OK: ShowRemoveTabDlg(tab);
            if (result != DialogResult.OK) return;
       
            this.RemoveToolboxTab(tab);
        } 


        /// <summary> Removes a toolbox tab, its local tool group, all its tools</summary>
        /// <param name="toolgroup">The toolgroup which corresponds to the tab to remove</param>
        public void RemoveToolboxTab(MaxToolGroup toolgroup)
        {
            MaxToolboxTab tab = this.FindTabByName(toolgroup.GroupName);

            if (this.toolboxTabs.Contains(tab))
            {
                this.SetNewOpenTab(tab);
                this.toolboxTabs.Remove(tab);
            }
                
            if (this.toolGroups.Contains(toolgroup))
                this.toolGroups.Remove(toolgroup);
        }


        /// <summary>Removes a toolbox tab, its local tool group, all its tools</summary>
        protected bool RemoveToolboxTab(MaxToolboxTab tab)
        {
            if (tab == null) return false;
 
            if (this.toolboxTabs.Contains(tab))
            {
                this.SetNewOpenTab(tab);
                this.toolboxTabs.Remove(tab);

                // if (this.openTab != null) this.toolboxTabAreaControl.Invalidate();
            }

            MaxToolGroup toolgroup = this.FindByGroupName(tab.Name);

            if (toolgroup != null && this.toolGroups.Contains(toolgroup))
                this.toolGroups.Remove(toolgroup);

            this.Refresh();  
            MarkProjectDirty();
            return true;
        }


        /// <summary>Set tab which will be current tab after a tab deletion</summary>
        protected void SetNewOpenTab(MaxToolboxTab tab)
        {
            if (tab == this.openTab)
            {
                int  tabndx = Tabs.IndexOf(tab);
                if  (tabndx > 0)  
                     this.openTab = toolboxTabs[tabndx - 1] as MaxToolboxTab;
                else                     
                if  (tabndx < toolboxTabs.Count - 1)  
                     this.openTab = toolboxTabs[tabndx + 1] as MaxToolboxTab;
                else this.openTab = null;       
            }
        }


        /// <summary>Get a default name for the tab of format "New Tab 1"</summary>
        private string GetNewTabDefaultName()   
        {
            string proposed;
            int sequencer = 0;

            while(true)
            {
                bool exists = false;
                proposed = Const.NewToolboxTabTextPrefix + ++sequencer;
        
                foreach(MaxToolboxTab tab in this.Tabs) 
                {                   
                    exists = tab.Name == proposed;
                    if (exists) break;                 
                }

                if (!exists) break;;
            }

            return proposed;
        }


        /// <summary>Remove a tool from its toolbox group and from its toolbox tab</summary>
        public bool RemoveToolFromGroup(MaxTool tool)
        {
            MaxToolGroup group;
            return RemoveToolFromGroup(tool, out group);
        }


        /// <summary>Remove a tool from its toolbox and from its toolbox tab</summary>
        public bool RemoveToolFromGroup(MaxTool tool, out MaxToolGroup group)
        {
            // Remove tool from toolbox toolgroup. We of course do not remove the
            // toolgroup if made empty, since toolgroups correspond to toolbox tabs,
            // and an empty tab is OK.

            group = this.FindGroupByTool(tool);
            if (group == null) return false;

            group.Tools.Remove(tool); 

            // Now remove tool from toolbox control
            MaxToolboxTab tab = this.FindTabByTool(tool);
            if (tab == null) return false;

            MaxToolboxItem toolboxitem = this.FindItemByTool(tab, tool);
            if (toolboxitem != null) 
            {
                tab.Items.Remove(toolboxitem);
                tool.Displayed = false;
            }

            return true;
        }


        /// <summary>Create a minimal toolbox</summary>
        public MaxToolboxTab CreateDefaultToolbox()    
        {
            MaxToolboxTab tab = new MaxToolboxTab(this, Const.AppComponentsTabName);
            this.Tabs.Add(tab);

            MaxToolGroup toolgroup     
                = new MaxToolGroup(Const.stockPackageName, Const.stockPackageName);

            this.toolGroups = new ArrayList();
            this.ToolGroups.Add(toolgroup);

            return tab;
        }   


        #if(false)
        public void OnItemDoubleClick(object sender, EventArgs e)
        {
            // This is called (sidbars+1) times when double click in background of toolbox
            // MessageBox.Show("MaxToolboxWindow double click");

            if (mainWindow.ActiveContentWindow == null) return;
             
            string text = OpenTab.SelectedItem.Tag.ToString();
              
            mainWindow.ActiveContentWindow.IEditable.ClipboardHandler.Delete(this, null);                                  
        }
        #endif
    
        
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Tab context menu
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        void SetItemContextMenu(object sender, MouseEventArgs e)
        {
            // shows context for an individual tool
            CancelAnyOngoingTabRename();

            if (e.Button == MouseButtons.Right) 
            {
                this.contextTab  = this.OpenTab; 
                this.DraggingTab = OpenTab;  
                Refresh();                      
                this.DraggingTab = null;        
            }           
        }   



        /// <summary>Handle delete key, edit menu delete</summary>
        public void OnEditDelete()
        {
            Object x = this.SelectedObject();
            if  (x is MaxToolboxTab) 
                 this.RemoveToolboxTab(); 
            else
            if  (x is MaxToolboxItem)  
            {
                DialogResult result = DialogResult.None;
                MaxTool tool = (x as MaxToolboxItem).Tag as MaxTool;             
                if  (tool != null)
                     result = ShowRemoveToolDlg(tool.Name);

                if  (result == DialogResult.OK)
                    this.RemoveToolFromGroup(tool);
            }      
        }



        /// <summary>Indicate if toolbox tab or item selected is deletable</summary>
        public bool IsDeletableItemSelected()
        {
            return this.SelectedObject() != null;
        }



        /// <summary>Return toolbox object currently selected, if any</summary>
        public Object SelectedObject()
        {
            MaxToolboxTab tab = this.ContextTab == null? this.OpenTab: this.ContextTab;
            MaxToolboxItem item = tab == null? null: tab.ItemPicked;
            MaxTool tool = item == null? null: item.Tag as MaxTool;
            return tool == null? tab as Object: item as Object;
        }



        public void MarkProjectDirty()
        {
            MaxMain.IdeDirty = true;
            main.OnProjectDirty(true);
        }


                
        public void MaxSerialize(XmlTextWriter writer)
        {     
            MaxToolboxHelper.Instance.Serialize(this, writer);      
        }


        /// <summary>Show "Tab xxx and n tools will be removed from toolbox"</summary>
        public static DialogResult ShowRemoveTabDlg(MaxToolboxTab tab)   
        {
            return MessageBox.Show(Manager.MaxManager.Instance,
                Const.DeleteToolboxTabMsg(tab.Name, tab.Items.Count), Const.removeTabDlgTitle,
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }


        /// <summary>Show "Tool xxx will be removed from toolbox"</summary>
        public static DialogResult ShowRemoveToolDlg(string name)   
        {
            return MessageBox.Show(Manager.MaxManager.Instance,
                Const.DeleteToolboxItemMsg(name), Const.removeToolDlgTitle,
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }
    

        #region MaxSatelliteWindow Members

        public Metreos.Max.Framework.Satellite.SatelliteTypes SatelliteType
        {
            get { return SatelliteTypes.Toolbox; }
        }

        public Crownwood.Magic.Menus.MenuCommand ViewMenuItem 
        { 
            get { return MaxMenu.menuViewToolbox; } 
        }

        #endregion

    }  // class MaxToolboxWindow
}    // namespace
