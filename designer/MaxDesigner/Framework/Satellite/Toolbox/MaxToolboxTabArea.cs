//
// MaxToolboxTabArea.cs
//
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using Crownwood.Magic.Menus;     
using Metreos.Max.Core;          
using Metreos.Max.Core.Tool;     
using Metreos.Max.Manager;       
using Metreos.Max.Framework.Satellite.Property;



namespace Metreos.Max.Framework.Satellite.Toolbox
{ 
    public class MaxToolboxTabAreaControl: UserControl
    {
        MaxToolboxControl toolboxControl = null;
        // private ToolTip tooltip;  

        public MaxToolboxControl ToolboxControl 
        {
            get { return toolboxControl;  } 
            set { toolboxControl = value; } 
        }

        public MaxToolboxTabAreaControl()
        {
            ResizeRedraw = AllowDrop = true;

            // tooltip = new ToolTip();
            // tooltip.InitialDelay = 750;
            // tooltip.AutoPopDelay = 5000;
            // tooltip.ReshowDelay  = 500;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.CacheText, true);
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            if (toolboxControl == null || toolboxControl.OpenTab == null) return; 

            Rectangle thisrect = new Rectangle(0, 0, Width, Height);

            toolboxControl.OpenTab.RenderItems(e.Graphics, Font, thisrect); 
        }



        public void RefreshEx(int n)
        {
            // Utl.Trace("TBA REFRESH FROM " + n);
            Refresh();
        }

      

        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);         
            toolboxControl.DraggingTab = null;
            toolboxControl.CancelDragActivity(toolboxControl.OpenTab);
            RefreshEx(2);
        }


        void CancelDragActivity(MaxToolboxTab tab)
        {
            foreach (MaxToolboxItem item in tab.Items) 
            {
                if (item.ToolboxItemState == ToolboxEntryState.Dragging)  
                    item.ToolboxItemState  = ToolboxEntryState.Normal;
            }
        }


        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);
            toolboxControl.CancelAnyOngoingTabRename();
            if (!e.Data.GetDataPresent(typeof(MaxToolboxItem))) return;
            Point p = PointToClient(new Point(e.X, e.Y));        
        
            // Drag moved item inside the activeTabItemsRect                    
            MaxToolboxItem item = toolboxControl.OpenTab.GetItemAt(p.X, p.Y);
            if (item == null) 
            {
                toolboxControl.CancelDragActivity(toolboxControl.OpenTab);
                toolboxControl.RefreshEx(12);
            } 
            else
                if (item != toolboxControl.OpenTab.ItemPicked) 
            {
                if (item.ToolboxItemState != ToolboxEntryState.Dragging) 
                {
                    toolboxControl.CancelDragActivity(toolboxControl.OpenTab);
                    item.ToolboxItemState = ToolboxEntryState.Dragging;
                    toolboxControl.RefreshEx(13);
                }
            } 
            else 
            {
                toolboxControl.CancelDragActivity(toolboxControl.OpenTab);
                toolboxControl.RefreshEx(14);
            }

            e.Effect = MaxToolboxControl.GetDragDropEffect(e);                               
        }


        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (toolboxControl.OpenTab == null) return;
            MaxToolboxItem oldSelectedItem = toolboxControl.OpenTab.SelectedItem;  
            toolboxControl.OpenTab.SelectedItem = null;
            // tooltip.SetToolTip(this, Const.emptystr);
            if (oldSelectedItem != null) Refresh();
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (toolboxControl.OpenTab == null) return;  
            MaxToolboxItem thisItem = toolboxControl.OpenTab.GetItemAt(e.X, e.Y);

            switch(e.Button)
            {
               case MouseButtons.Left: 
                     
                     if (thisItem != null) 
                     {   // Controls drag state of toolbox item  
                         if (thisItem.Tag is MaxTool && (thisItem.Tag as MaxTool).Disabled) break; 
                         // toolboxControl.DraggingTab = toolboxControl.OpenTab;  
                         MaxDropObject dataObject = new MaxDropObject();
                         dataObject.SetData(thisItem.Tag);
                         dataObject.SetData(thisItem);
                         DoDragDrop(dataObject, DragDropEffects.All); // observed invalid cast 
                         RefreshEx(3);
                     }
                     
                    break;

               case MouseButtons.Right:  // right click selects
               default:
                        
                    MaxToolboxItem oldSelectedItem = toolboxControl.OpenTab.SelectedItem;
                    toolboxControl.OpenTab.SelectedItem = null;

                    if (thisItem != null)  
                        toolboxControl.OpenTab.SelectedItem = thisItem;

                    if (oldSelectedItem != toolboxControl.OpenTab.SelectedItem)  
                        toolboxControl.RefreshEx(15);

                    break;
            }     
        }



        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (toolboxControl == null || toolboxControl.OpenTab == null) return;  
            toolboxControl.OpenTab.ItemPicked = toolboxControl.OpenTab.SelectedItem;
            RefreshEx(4);
        }



        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (toolboxControl == null || toolboxControl.OpenTab == null)  
            {
                base.OnMouseUp(e);                 
                return;
            }

            toolboxControl.CancelAnyOngoingTabRename();
            RefreshEx(5);        

            switch(e.Button)                     
            {  
                case MouseButtons.Right:  

                    MaxToolboxItem item = toolboxControl.OpenTab.GetItemAt(e.X, e.Y);
                    if (item == null) break;
                    // Note -- the fact that we also set ItemPicked here means that  
                    // the right click also selects the tool. This is not classic  
                    // UI behavior, but is for now the lesser of evils.
                    toolboxControl.OpenTab.SelectedItem = toolboxControl.OpenTab.ItemPicked = item; 
                    toolboxControl.ContextTab = toolboxControl.OpenTab;
                    RefreshEx(6);  

                    toolboxControl.menu.PopItemContextMenu(item);        
                    break;
            }

            base.OnMouseUp(e);
        }

    } // class MaxToolboxTabAreaControl
	
	
} // namespace
