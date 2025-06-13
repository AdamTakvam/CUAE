//
// MaxToolboxTab.cs
//
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections;
using Metreos.Max.Core;



namespace Metreos.Max.Framework.Satellite.Toolbox
{
    public enum  ToolboxTabState { Normal, Selected, Dragging } 
  
    public class MaxToolboxTab
    {
        public MaxToolboxTab(MaxToolboxControl toolboxControl, string name) 
        {
            this.name = name;
            this.toolboxControl = toolboxControl; 
            items = new MaxToolboxItemArray();
        }

        private MaxToolboxItemArray items = null;
        private MaxToolboxItem selectedItem = null;
        private MaxToolboxItem itemPicked   = null;
        private MaxToolboxControl toolboxControl;   
        public  MaxToolboxItemArray Items  { get { return items; } } 

        public  Color SelectedBorderColor      
        { get 
          {   Color c = SystemColors.ActiveCaption;
              if  (c.R == c.G && c.R == c.B)
                   return Color.FromArgb(160,176,233);
              else return ControlPaint.LightLight(c); 
          } 
        }

    
        public MaxToolboxItem SelectedItem 
        {   get { return selectedItem; }
            set { SetSelection(value as MaxToolboxItem); }            
        }
        
        public MaxToolboxItem ItemPicked 
        {   get { return itemPicked; }
            set { SetPick(value as MaxToolboxItem); }  
        }


        private void SetSelection(MaxToolboxItem item)
        {
            if (IsNewSelection())  
                SetItemState(selectedItem, ToolboxEntryState.Normal); 
            selectedItem = item;
            if (IsNewSelection()) 
                SetItemState(selectedItem, ToolboxEntryState.Selected);  
        }


        private void SetPick(MaxToolboxItem item)
        {            
            SetItemState(itemPicked, ToolboxEntryState.Normal);     
            itemPicked = item;
            SetItemState(itemPicked, ToolboxEntryState.Committed);
        }



        private void SetItemState(MaxToolboxItem item, ToolboxEntryState newstate)
        {
           if (item != null) item.ToolboxItemState = newstate;
        }



        private bool IsNewSelection()
        {
           return selectedItem != null && selectedItem != itemPicked;
        }



        public void RenderItems(Graphics g, Font f, Rectangle rect)
        {       
            // If staggered paint, draw only the first item here 
            if  (Items.Count == 0) { }
            else              
            if  (MaxToolboxControl.IsTimedItemPaint && MaxToolboxControl.IsTabOpening)   
                 this.DrawTabItem(g, f, rect, 0); 

            else for (int i = 0; i + ScrollIndex < Items.Count; ++i) 
                      if (!this.DrawTabItem(g, f, rect, i)) break;  
        }



        public bool DrawTabItem(Graphics g, Font f, Rectangle tabsArea, int i)  
        {
            // Utl.Trace("DrawTabItem " + i);  
            int cyTotal = i * MaxToolboxControl.itemHeight;
            if (tabsArea.Height < cyTotal) return false;

            MaxToolboxItem item = (MaxToolboxItem)Items[this.ScrollIndex + i];

            Rectangle rcitem = new Rectangle(tabsArea.X, tabsArea.Y + cyTotal, 
                tabsArea.Width, MaxToolboxControl.itemHeight);
          
            item.Render(g, f, rcitem);

            return true;
        }
     
        

        public void RenderTab(Graphics g, Font font, Point p, int width)
        {
            // Utl.Trace("RenderTab " + p.ToString());  
            Rectangle  nr = new Rectangle (0, p.Y,     
                       width -  MaxToolboxControl.cxBorderRight, 
                       toolboxControl.CyTab);  

            RectangleF rf = new RectangleF(1+1, p.Y + MaxToolboxControl.vPadTabTop, 
                       width - (MaxToolboxControl.cxBorderRight + 1), 
                       font.Height + MaxToolboxControl.vPadTabTop);
            rf.Y--; rf.Height--; // rf.X--; 

            switch(this.toolboxTabState) 
            {
               case ToolboxTabState.Normal:

                    g.FillRectangle(toolboxControl.VsTabSolidBrush, nr);  

                    ControlPaint.DrawBorder(g, nr, 
                       toolboxControl.NormalBorderColor,
                       ButtonBorderStyle.Solid);
                                      
                    g.DrawString(name, font, SystemBrushes.ControlText, rf);          
                    break;


               case ToolboxTabState.Selected:

                    g.FillRectangle(toolboxControl.VsTabSolidBrush, nr); 

                    ControlPaint.DrawBorder(g, nr, 
                                 SelectedBorderColor, 
                                 ButtonBorderStyle.Solid);

                    g.DrawString(name, font, SystemBrushes.ControlText, rf);
                    break;


               case ToolboxTabState.Dragging:

                    g.FillRectangle(toolboxControl.VsTabSolidBrush, nr); 
 
                    ControlPaint.DrawBorder(g, nr, 
                                 SelectedBorderColor, 
                                 ButtonBorderStyle.Solid);

                    g.DrawString(name, font, SystemBrushes.ControlText, rf);
                    break;
            }
        }
    

        public Point ItemLocation(MaxToolboxItem whichItem)
        {
            for (int i = 0; i < Items.Count; ++i) 
            {
                MaxToolboxItem item = (MaxToolboxItem)Items[i];
                if (item == whichItem)  
                    return new Point(0, i * MaxToolboxControl.itemHeight);         
            }

            return new Point(-1, -1);
        }
    

        public MaxToolboxItem GetItemAt(int x, int y)
        {
            int ndx = ScrollIndex + y / MaxToolboxControl.itemHeight;
            return (ndx >= 0 && ndx < Items.Count)? (MaxToolboxItem)Items[ndx]: null;
        }
    

        public MaxToolboxItem GetItemAt(Point pos)
        {
            return GetItemAt(pos.X, pos.Y);
        }  


        public class MaxToolboxItemArray: ArrayList
        {
            public MaxToolboxItem Get(int index)
            {
                return (MaxToolboxItem)this[index]; 
            }


            public MaxToolboxItem Get(string name)
            {    
                foreach(MaxToolboxItem item in this) if (item.Name == name) return item;
                return null;
            }

      
            public int IndexOfDraggedItem() 
            {       
                for (int i = 0; i < Count; ++i)  
                     if (Get(i).ToolboxItemState == ToolboxEntryState.Dragging) return i;          
                return -1;                 
            }



            public MaxToolboxItem Add(string name, object content)
            {
                return Add(name, content, -1);
            }
      


            public MaxToolboxItem Add(string name, object content, int imageIndex)
            {                                                   
                MaxToolboxItem item = MaxToolboxItem.NewToolboxEntry(name, content, null);  
                Add(item);
                return item;
            }
      

            public MaxToolboxItem Insert(int index, string name, object content, int imageIndex)
            {                                                  
                MaxToolboxItem item = MaxToolboxItem.NewToolboxEntry(name, content, null);   
                Insert(index, item);
                return item;
            }
        }

        public ToolboxTabState ToolboxTabState 
        { get { return toolboxTabState; } set { toolboxTabState = value; } 
        }
        public string Name {get  { return name; } set { name = value; } }   
        public int  ScrollIndex  { get { return scrollIndex; } set { scrollIndex  = value; } }    
        public bool OkToDelete   { get { return okToDelete;}   set { okToDelete   = value; } }
        private int scrollIndex  = 0;
        private string name = null;
        private ToolboxTabState toolboxTabState;
        public  bool okToDelete = true; 
    }
}
