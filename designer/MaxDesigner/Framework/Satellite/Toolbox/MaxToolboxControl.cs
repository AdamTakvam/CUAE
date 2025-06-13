//
// MaxToolboxControl.cs
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
    public class MaxToolboxControl: UserControl
    {
        public  enum  TabStyle { Thick, Thin }
        public  const TabStyle tabStyle = TabStyle.Thin;
        public  const bool IsTimedItemPaint = true;
        public  const int  vPadTabTop = 2;  // vertical space above text
        
        // The following vPadTabBot must be set to 2 if tab style is Thick.
        // If we do it with a static initialization test as in:
        // public  const int  vPadTabBot = tabStyle == TabStyle.Thick? 2: 1;
        // we get a compiler warning which I don't know how to squash.
        public const int vPadTabBot = 1;       

        public  const int  vPadTot = vPadTabTop + vPadTabBot;
        public  const int  vTabGap = 1;      // vertical space between tabs
        public  const int  cxItemBorder = 3; // icon horizontal indent
        public  const int  itemHeight = 20;  // height of a toolbox item
        public  int   CyTab    { get { return Font.Height + vPadTot; } }
        public  int   CyTabTot { get { return CyTab + vTabGap;       } }
        private const int editboxVertDifferential = (-2);
        private const int editboxHorzDifferential = (-8);

        public  const int scrollbarWidth = 17 - 2;  // 17 is windows default
        public  const int scrollbarThumbSize = 3;   // less than 3 makes thumb a nub
        public  const int cxBorderRight = 2;        // vertical between control & frame  

        public  const int vsDockingTitleGrayscaleValue = 204;
        public  const int vsGroupHeaderGrayscaleValue  = 198;
        public  const int vsGroupHeaderGrayscaleValue2 = 225;

        public static int ToRGB(double d)
        {
            int n = Convert.ToInt32(d);
            if (n < 0) n = 0; else if (n > 255) n = 255;
            return n;
        }

        public static Color AdjustColor(Color c, double pct)
        {
            double fr = Convert.ToDouble(c.R);  double fg  = Convert.ToDouble(c.G);
            double fb = Convert.ToDouble(c.B);   
            fr *= pct; fg *= pct; fb *= pct;
            int r = ToRGB(fr); int g = ToRGB(fg); int b = ToRGB(fb);
            return Color.FromArgb((int)c.A,r,g,b);
        }

        public  Color VsTabColor            { get { return AdjustColor(SystemColors.Control, 0.97); } } 
        public  Color NormalBorderColor     { get { return AdjustColor(SystemColors.ControlDark, 1.175); } }
        public  SolidBrush VsTabSolidBrush  { get { return new SolidBrush(VsTabColor); } }        
        public  SolidBrush disabledTextBrush{ get { return new SolidBrush(ControlPaint.LightLight(SystemColors.ControlText)); } }


        public MaxToolboxControl()
        {
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            this.AllowDrop = this.ResizeRedraw = true;
            CreateTabControl();                  
            CreateOverlay();
            CreateScrollbar();
            this.menu = new MaxToolboxMenu(this);
        }


        private void CreateTabControl()
        {
            toolboxTabAreaControl = new MaxToolboxTabAreaControl();
            toolboxTabAreaControl.ToolboxControl = this;      
            toolboxTabs = new ArrayList();
            Controls.Add(toolboxTabAreaControl);
        }


        private void CreateScrollbar()
        {
            scrollbar = new VScrollBar();
            scrollbar.Width = MaxToolboxControl.scrollbarWidth;
            scrollbar.LargeChange = MaxToolboxControl.scrollbarThumbSize;
            scrollbar.SmallChange = 1;
            scrollbar.Scroll += new ScrollEventHandler(OnScrolled);
            scrollbar.Minimum = 0;
            Controls.Add(scrollbar);
        }


        private void CreateOverlay()
        {
            rtb = new TextBox(); 
            rtb.Visible = false;
            rtb.BorderStyle = BorderStyle.None;      
            Controls.Add(rtb);
        }


        private void SetCurrentTabScrollInfo(bool refresh)
        {
            if (openTab == null) return;   
            int count = openTab.Items.Count;          
            scrollbar.Maximum = count;
            scrollbar.Value = count < openTab.ScrollIndex? count: openTab.ScrollIndex;
            if (refresh) this.RefreshEx(1); 
        }


        public MaxToolboxTab SetOpenTab(MaxToolboxTab tab, bool refresh)
        {
            MaxToolboxTab oldtab = openTab;
            if (oldtab != null) oldtab.ScrollIndex = scrollbar.Value; 
            openTab = tab;
            SetCurrentTabScrollInfo(refresh);
            return oldtab;
        }


        public MaxToolboxTab OpenTab 
        {
            get { return openTab;}
            set 
            {   if (openTab != null) openTab.ScrollIndex = scrollbar.Value;       
                openTab = value;
                SetCurrentTabScrollInfo(true);
            }
        }


        public MaxToolboxTab FindTabByTool(MaxTool tool)    
        {
            foreach(MaxToolboxTab tab in this.Tabs)
            foreach(MaxToolboxItem item in tab.Items)
            {
                MaxTool thistool = item.Tag as MaxTool;
                if  (thistool != null && thistool == tool) return tab;    
            }

            return null;
        }


        public MaxToolboxTab FindTabByName(string name)    
        {
            foreach(MaxToolboxTab tab in this.Tabs) if (tab.Name.Equals(name)) return tab;
            return null;
        }


        public MaxToolboxItem FindItemByTool(MaxToolboxTab tab, MaxTool tool)  
        {
            foreach(MaxToolboxItem item in tab.Items)
            {
                MaxTool thistool = item.Tag as MaxTool;
                if  (thistool != null && thistool == tool) return item;    
            }

            return null;
        }


        public MaxToolboxItem FindItemByName(MaxToolboxTab tab, string name)  
        {
            foreach(MaxToolboxItem item in tab.Items)
            {
                MaxTool thistool = item.Tag as MaxTool;
                if (thistool != null && thistool.Name == name) return item;    
            }

            return null;
        }
    

        public void CancelAnyOngoingTabRename()
        {
            isNewToolboxTab = rtb.Visible = false;
            tabNowRenaming = null;                        
        }
    

        public void EnsureVisible(MaxToolboxItem thisItem)
        {
            int itemNdx  = openTab.Items.IndexOf(thisItem);

            if (itemNdx != -1) 
            {
                if (itemNdx < scrollbar.Value) 
                    scrollbar.Value = itemNdx;
                else 
                {   int lastndx = (toolboxTabAreaControl.Height - (itemHeight - 5)) / itemHeight;
                    if (itemNdx > scrollbar.Value + lastndx) 
                        scrollbar.Value = itemNdx - lastndx;
                }

                if (openTab != null) openTab.ScrollIndex = scrollbar.Value;
                toolboxTabAreaControl.RefreshEx(7);
            }

            this.HandleItemFocus(thisItem);  
        }


        public void HandleItemFocus(object sender, MouseEventArgs e)  
        {
            MaxPropertyWindow.Instance.Clear(this);

            MaxToolboxTab tab = OpenTab;
            if  (tab == null)  return;  
            MaxToolboxItem item = tab.GetItemAt(e.X, e.Y);
            this.HandleItemFocus(item); 
        }


        public void HandleItemFocus(MaxToolboxItem item)  
        {
            if  (item == null) return;
            MaxTool tool = item.Tag as MaxTool;
            if  (tool == null) return;
           
            PmProxy.ShowProperties(tool, tool.PmObjectType);
        }


        private const int cmdkeyResultChange  = 1;
        private const int cmdkeyResultEnsure  = 2;
        private const int cmdkeyResultOpenTab = 4;
        private const int cmdkeyResultBoth    = cmdkeyResultChange | cmdkeyResultEnsure;


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (base.ProcessCmdKey(ref msg, keyData)) return true;
            if (openTab == null) return false;

            int  newndx, maxndx, result = 0;
            int  itemcount  = openTab.Items.Count;
            int  itemndx    = openTab.Items.IndexOf(openTab.ItemPicked);      
            bool isRenaming = tabNowRenaming != null;
            bool isOkToScroll = itemcount > 0 && !isRenaming;
      
            switch (keyData) 
            {
               case Keys.Return:

                    if (tabNowRenaming == null) break;                                 
                    if (this.ValidateTabName())     
                    {   tabNowRenaming.Name = rtb.Text;
                        this.CancelAnyOngoingTabRename();
                    }
                    else
                    if  (isNewToolboxTab)                  
                         this.CancelNewTabRequest();                
                    else this.CancelAnyOngoingTabRename();                                                     
                    break;


                case Keys.Escape:

                    if (tabNowRenaming != null)             
                    {   
                        if  (isNewToolboxTab) 
                             this.CancelNewTabRequest();                
                        else this.CancelAnyOngoingTabRename();  
                    }
                    
                    if (this.Deselect()) result = cmdkeyResultChange;                                                            
                    break;


               case Keys.Down:
               case Keys.Up:

                    if (isOkToScroll) 
                    {
                        if  (openTab.ItemPicked == null) 
                             openTab.ItemPicked = openTab.Items[0] as MaxToolboxItem;
                        else
                        {    openTab.ItemPicked = keyData == Keys.Down?
                             openTab.Items[Math.Min(itemcount - 1, itemndx + 1)] as MaxToolboxItem:
                             openTab.Items[Math.Max(0, itemndx - 1)] as MaxToolboxItem;
                        }
               
                        openTab.SelectedItem = null;
                        result = cmdkeyResultBoth;                       
                    }
                    break;


               case Keys.PageDown:
               case Keys.PageUp:

                    if (isOkToScroll) 
                    {   
                        int lgChg = Math.Max(itemHeight, toolboxTabAreaControl.Height / itemHeight);

                        newndx = keyData == Keys.PageDown?
                            Math.Min(itemcount - 1, itemndx + lgChg):
                            Math.Max(0, itemndx - lgChg);

                        openTab.ItemPicked = openTab.Items[newndx] as MaxToolboxItem;
                        result = cmdkeyResultBoth;
                    }
                    break;


               case Keys.Home:
               case Keys.End:

                    if (isOkToScroll) 
                    {
                        newndx = keyData == Keys.Home? 0: itemcount > 0? itemcount: 0;
                        openTab.ItemPicked = openTab.Items[newndx] as MaxToolboxItem; 
                        result = cmdkeyResultBoth;
                    }
                    break;


               case Keys.Control | Keys.Up:    // Ctrl-UpArrow moves up a tab

                    newndx  = Tabs.IndexOf(OpenTab) - 1;
                    if (newndx < 0) newndx = 0;
                    OpenTab = Tabs[newndx] as MaxToolboxTab; 
                    result  = cmdkeyResultChange | cmdkeyResultOpenTab;
                    break;


               case Keys.Control | Keys.Down:  // Ctrl-DownArrow moves down a tab

                    maxndx  = Tabs.Count - 1; 
                    newndx  = Tabs.IndexOf(OpenTab) + 1;
                    newndx  = newndx < maxndx? newndx: maxndx;                   
                    OpenTab = Tabs[newndx] as MaxToolboxTab;
                    result  = cmdkeyResultChange | cmdkeyResultOpenTab;
                    break;

               default: return false; // indicate shortcut key not handled
            }

            if (0 != (result & cmdkeyResultChange))
            {   
                if (IsTimedItemPaint && ((result & cmdkeyResultOpenTab) != 0))
                {
                    // Instead of refresh, we'll fake a mouse up in order to trigger paint delay
                    Point p = Control.MousePosition;
                    mouseDownTab = openTab;
                    this.OnMouseUp(new MouseEventArgs(MouseButtons.Left, 1, p.X, p.Y, 0));
                }
                else this.RefreshEx(2);

                if (0 != (result & cmdkeyResultEnsure)) 
                    this.EnsureVisible(openTab.ItemPicked);
            }

            return true; // indicate shortcut key handled
        }


        /// <summary>Cancel current tab add operation and reset state</summary>
        private void CancelNewTabRequest()
        {
            Tabs.RemoveAt(Tabs.Count - 1);
            tabNowRenaming = null;
            rtb.Visible = isNewToolboxTab = false;
            RefreshEx(3);
        }


        /// <summary>Deselect selected item</summary> 
        public bool Deselect()
        {   
            foreach(MaxToolboxItem item in openTab.Items)  
                if (item.ToolboxItemState != ToolboxEntryState.Normal)  
                {   item.ToolboxItemState  = ToolboxEntryState.Normal;
                    MaxPropertyWindow.Instance.Clear(this);
                    return true;
                }

            return false;
        }


        /// <summary>Ensure tab name is reasonable and not taken</summary>   
        private bool ValidateTabName()
        {
            string tentative = rtb.Text;
            if (!Utl.ValidateRawStringInput(tentative)) return false;

            foreach(MaxToolboxTab tab in this.Tabs) 
                if (tab != tabNowRenaming && Utl.CompareCaseInsensitive(tab.Name, tentative)) return false;
                       
            return true; 
        }
    

        /// <summary>Begin re/naming tab. System key input triggers end edit</summary>
        public void Rename(MaxToolboxTab tab)
        {
            this.tabNowRenaming = tab;
            Point xy = this.TabXY(tabNowRenaming);
            xy.Offset(3,1);            
            rtb.Location = xy;
            rtb.Width    = Width + editboxHorzDifferential;
            rtb.Height   = Font.Height + editboxVertDifferential;
            rtb.Text     = tabNowRenaming.Name;
            rtb.Visible  = true;
            rtb.Focus();
        }
    

    
        public Point TabXY(MaxToolboxTab whichTab)
        {
            int curtab = 0, priorY = 0;
            int itemdelta = Font.Height + vPadTot + 1, count = toolboxTabs.Count;
      
            for (int i=0; i < count; ++i) 
            {
                MaxToolboxTab tab = toolboxTabs[i] as MaxToolboxTab;

                // Utl.Trace(i + " " + tab.Name);
        
                int y = i * itemdelta;       
                if (tab == whichTab) return new Point(0, y);
         
                curtab = i;
                priorY = y + Font.Height + vPadTot;
                if (tab == openTab) break;         
            }
            
            for (int i = count - 1; i > curtab; --i) 
            {
                MaxToolboxTab tab = toolboxTabs[i] as MaxToolboxTab;
                int revndx = count - i;
        
                int y = Height - revndx * itemdelta;        
                if (y < priorY + itemdelta) break;
        
                if (tab == whichTab) return new Point(0, y);        
            }
      
            return new Point(-1, -1);
        }


        public MaxToolboxTab GetTabAt(Point p)  { return GetTabAt(p.X, p.Y); }
    

        public MaxToolboxTab GetTabAt(int x, int y)
        {
            int priorY = 0, nextY = 0, curtab = 0;
            int cyTab = Font.Height + vPadTot;
            int itemdelta = cyTab + 1, count = toolboxTabs.Count;

            for (int i=0; i < count; ++i) 
            {
                MaxToolboxTab tab = toolboxTabs[i] as MaxToolboxTab;        
                nextY  = i * itemdelta;        
                priorY = nextY + cyTab;        
                if (y >= nextY && y <= priorY) return tab;

                curtab = i;
                if (tab == openTab) break;        
            }
      
            for (int i = count - 1; i > curtab; --i) 
            {
                MaxToolboxTab tab = toolboxTabs[i] as MaxToolboxTab;
                int revndx = count - i;
        
                nextY = Height - revndx * itemdelta;        
                if (nextY < priorY) break;
                if (y >= nextY && y <= nextY + cyTab) return tab;
            }

            return null;
        }
   
    

        public int GetTabIndexAt(int x, int y)
        {
            int priorY = 0, nextY = 0, curtab = 0;
            int cyTab = Font.Height + vPadTot;
            int itemdelta = cyTab + 1, count = toolboxTabs.Count;

            for (int i = 0; i < count; ++i) 
            {
                MaxToolboxTab tab = toolboxTabs[i] as MaxToolboxTab;
        
                nextY = i * itemdelta;        
                priorY = nextY + cyTab;        
                if (y >= nextY && y <= priorY) return i;

                curtab = i;
                if (tab == openTab) break;
            }
      
            for (int i = count - 1; i > curtab; --i) 
            {
                MaxToolboxTab tab = toolboxTabs[i] as MaxToolboxTab;
                int revndx = count - i;
        
                nextY = Height - revndx * itemdelta;        
                if (nextY < priorY + itemdelta) break;
                if (y >= nextY && y <= nextY + cyTab)  return i;
            }

            return -1;
        }
    


        public static DragDropEffects GetDragDropEffect(DragEventArgs e)    
        {
            return (e.AllowedEffect & (DragDropEffects.Move | DragDropEffects.Copy)) > 0?           
                    DragDropEffects.Move: DragDropEffects.None;
        }



        protected override void OnDragEnter(DragEventArgs e)
        {
            CancelAnyOngoingTabRename();
      
            base.OnDragEnter(e);           
      
            if  (e.Data.GetDataPresent(typeof(MaxToolboxItem)))
                 e.Effect = DragDropEffects.Copy;      
            else 
            if  (e.Data.GetDataPresent(typeof(MaxToolboxTab))) 
            {
                MaxToolboxTab tab = (MaxToolboxTab)e.Data.GetData(typeof(MaxToolboxTab));

                if  (Tabs.Contains(tab)) 
                {
                     this.DraggingTab = tab;
                     e.Effect = GetDragDropEffect(e);
                } 
                else e.Effect = DragDropEffects.None;         
            }          
            else e.Effect = DragDropEffects.None;       
        }
    

        protected override void OnDragLeave(EventArgs e)
        { 
            base.OnDragLeave(e);
            this.DraggingTab = null;
            CancelDragActivity(openTab);
            RefreshEx(4);
        }
    

        protected override void OnDragDrop(DragEventArgs e)
        {
            Point p = PointToClient(new Point(e.X, e.Y)); 
            Point q = mouseDownXY;                        

            // ensure small mouse movements during tab click not interpreted as start drag
            int  dx = Math.Abs(p.X - q.X), dy = Math.Abs(p.Y - q.Y); 
            if  (dx < Config.minPixelsInToolboxDrag && dy < Config.minPixelsInToolboxDrag) 
                 this.OnMouseUp(new MouseEventArgs(MouseButtons.Left, 1, q.X, q.Y, 0));
            else
            {    base.OnDragDrop(e);
                 SetDragTab(null);
                 RefreshEx(5); 
            }              
        }
    

        public void CancelDragActivity(MaxToolboxTab tab)
        {
            foreach(MaxToolboxItem item in tab.Items)  
                if (item.ToolboxItemState == ToolboxEntryState.Dragging)  
                    item.ToolboxItemState  = ToolboxEntryState.Normal;
        }



        void SetDragTab(MaxToolboxTab tab)
        {
            this.DraggingTab = tab;
            RefreshEx(6);
        }


    
        protected override void OnDragOver(DragEventArgs e)
        {
            CancelAnyOngoingTabRename();
            base.OnDragOver(e);      
            Point p = PointToClient(new Point(e.X, e.Y));

            if (e.Data.GetDataPresent(typeof(MaxToolboxItem))) 
            {
                CancelDragActivity(openTab);
                MaxToolboxTab tab = GetTabAt(p.X, p.Y);

                if (tab != null && tab != this.DraggingTab) SetDragTab(tab);

                e.Effect = this.DraggingTab == null? DragDropEffects.None: DragDropEffects.Move;             
            }            
            else 
            if (e.Data.GetDataPresent(typeof(MaxToolboxTab))) 
            {
                int  tabIndex  = GetTabIndexAt(p.X, p.Y);
                if  (tabIndex != -1) 
                {
                    MaxToolboxTab tab = this.DraggingTab;
                    Tabs.Remove(tab);
                    Tabs.Insert(tabIndex, tab);
                    RefreshEx(7);
                }
                e.Effect = DragDropEffects.Move;
            }
        }

    
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button != MouseButtons.Left || mouseDownTab == null) return;  
            int tabndx = -1;

            for (int i = 0; i < toolboxTabs.Count; ++i) 
            {
                MaxToolboxTab thistab = toolboxTabs[i] as MaxToolboxTab;
                if (thistab != null && thistab.ToolboxTabState == ToolboxTabState.Selected) 
                {
                    tabndx = i;
                    break;
                }
            }
    
            if (tabndx == -1) return;
            this.DraggingTab = Tabs[tabndx] as MaxToolboxTab;
            DoDragDrop(this.DraggingTab, DragDropEffects.All);
            RefreshEx(8);
        }
    

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e); // force mouse wheel 1 unit
            if  (!scrollbar.Visible) return;  
            int  scrolldir = e.Delta < 0? -1: e.Delta > 0? 1: 0;
            int  newscrollval = scrollbar.Value - scrolldir;  
            if  (newscrollval > scrollbar.Maximum) newscrollval = scrollbar.Maximum;
            if  (newscrollval < scrollbar.Minimum) newscrollval = scrollbar.Minimum;
            scrollbar.Value = newscrollval;
            OnScrolled(null, null);       
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            switch(e.Button)                 
            {
               case MouseButtons.Left:
                    this.mouseDownXY.X = e.X; this.mouseDownXY.Y = e.Y;   

                    MaxToolboxTab tab = GetTabAt(e.X, e.Y);
                    if  (tab == null) break;
              
                    mouseDownTab = tab;
                    tab.ToolboxTabState = ToolboxTabState.Selected;    

                    if  (activateTabOnMouseDown)   
                         OnMouseUp(e);             
                    else RefreshEx(9);  // refresh to show selection state
                    break;

               case MouseButtons.Right:       
                    this.rightClickXY.X = e.X; this.rightClickXY.Y = e.Y;  
                    this.contextTab = this.GetTabAt(this.rightClickXY);
                    break;
            }     
        }


        public void RefreshEx(int n)
        {
            // Utl.Trace("REFRESH from " + n);  
            Refresh(); 
        }


        private const int slowItemPaintDelay = 52;
        private const int normItemPaintDelay = 32;
        private const int fastItemPaintDelay = 24;


        private void DoTimedItemPaint()
        {
            if (openTab == null) return;
            int count = openTab.Items.Count;
            int delay = count < 4? slowItemPaintDelay: count < 8? normItemPaintDelay: fastItemPaintDelay;
            Rectangle itemarearect = new Rectangle
              (0, 0, toolboxTabAreaControl.Width, toolboxTabAreaControl.Height);

            Graphics g = toolboxTabAreaControl.CreateGraphics();

            for(int i = 1; i < count; i++)     // Paint items 2-n
            {
                System.Threading.Thread.Sleep(delay); 
                try  { openTab.DrawTabItem(g, this.Font, itemarearect, i); }
                catch{ }
            }

            g.Dispose();
            IsTabOpening = false;
        }



        protected override void OnMouseUp(MouseEventArgs e)
        {    
            switch(e.Button)    
            {  
               case MouseButtons.Left:

                    if (mouseDownTab != null)       
                    {     
                        // we know if user clicked on current
                        // tab by openTab == mouseDownTab   
                                              
                        SetOpenTab(mouseDownTab, false);
                        mouseDownTab.ToolboxTabState = ToolboxTabState.Normal;
                        mouseDownTab = null;

                        // when user clicks on tab, even if current tab,
                        // we clear properties and clear tool selection
                        MaxPropertyWindow.Instance.Clear(this);  
                        openTab.ItemPicked = null; 
                        IsTabOpening = true;
                        // RefreshEx(10);
                    }
              
                    CancelAnyOngoingTabRename();
                    RefreshEx(11);

                    if  (!activateTabOnMouseDown)   
                         base.OnMouseUp(e);

                    if (MaxToolboxControl.IsTimedItemPaint && IsTabOpening) 
                        this.DoTimedItemPaint();
                    break;

               case MouseButtons.Right: 

                    this.menu.PopTabContextMenu();
                    break;
            }
        }


    
        protected override void OnPaint(PaintEventArgs e)
        {
            int opentab = 0, priorY = 0, count = toolboxTabs.Count;
            int cyTab = this.CyTab, actbottom = this.Height; 
            Graphics g = e.Graphics;
      
            for (int i = 0; i < count; ++i) 
            {
                MaxToolboxTab tab = toolboxTabs[i] as MaxToolboxTab;

                int y = i * CyTabTot;
                tab.RenderTab(g, Font, new Point(0, y), Width);
                priorY = y + cyTab;
        
                opentab = i;
                if (tab == openTab) break;         
            }
      
            for (int i = count - 1; i > opentab; --i) 
            {
                MaxToolboxTab tab = toolboxTabs[i] as MaxToolboxTab;
                int revndx = count - i;
        
                int y = Height - (revndx * CyTabTot);        
                if (y < priorY + CyTabTot) break;
        
                actbottom = y;
                tab.RenderTab(g, Font, new Point(0, y), Width);
            }
      
            if (openTab == null) return;

            int priorBottom = actbottom - priorY;
            bool b = scrollbar.Maximum > priorBottom / itemHeight || scrollbar.Value != 0;
            scrollbar.Visible = b;
            int cxScrollbar = scrollbar.Visible? scrollbarWidth: 0;
            
            int tabItemsAreaWidth = this.Width - cxScrollbar - cxBorderRight;

            Rectangle activeTabItemsRect = new Rectangle(0, priorY, tabItemsAreaWidth, priorBottom);

            toolboxTabAreaControl.Bounds = activeTabItemsRect;
            scrollbar.LargeChange = 3;                
            scrollbar.Location = new Point(tabItemsAreaWidth, priorY);
            scrollbar.Height = activeTabItemsRect.Height;
        }



        void OnScrolled(object sender, ScrollEventArgs e)
        {
            if (openTab != null) openTab.ScrollIndex = scrollbar.Value;
            toolboxTabAreaControl.RefreshEx(1);
        }
    


        public MaxToolboxTab DraggingTab 
        {
            get { return dragTab; }
            set 
            {   if (dragTab != null)  dragTab.ToolboxTabState = ToolboxTabState.Normal;           
                dragTab = value;
                if (dragTab != null)  dragTab.ToolboxTabState = ToolboxTabState.Dragging;
            }
        }


        public static System.Drawing.Color VsTitleBarColor(int redval)
        {
            double d   = Convert.ToDouble(redval);
            int grnval = Convert.ToInt32(d *= 0.96);
            int bluval = Convert.ToInt32(d *  0.96);
            return Color.FromArgb(redval, grnval, bluval);
        }


        public static System.Drawing.Color VsToolboxTabrColor(int redval)
        {
            double d   = Convert.ToDouble(redval);
            int grnval = Convert.ToInt32(d *= 0.985);
            int bluval = Convert.ToInt32(d *  0.935);
            return Color.FromArgb(redval, grnval, bluval);
        }

        protected MaxToolboxTab openTab  = null;
        protected MaxToolboxTab contextTab = null;            
        protected MaxToolboxTab tabNowRenaming = null;
        public    MaxToolboxTab dragTab = null;
        public    MaxToolboxTab mouseDownTab = null; 
        public    MaxToolboxTab ContextTab { get { return contextTab; } set { contextTab = value; } }  

        protected MaxToolboxTabAreaControl toolboxTabAreaControl = null;
        protected ArrayList toolboxTabs = null; 
        protected VScrollBar scrollbar  = null;  
        protected TextBox rtb = null; 
        protected Point mouseDownXY   = new Point(0,0);   
        protected Point rightClickXY  = new Point(0,0);  
        protected bool  isNewToolboxTab  = false;  
        public  static bool IsTabOpening = false;   
        private static bool activateTabOnMouseDown = false;                
        public  ArrayList Tabs  { get { return toolboxTabs; } }
        public  MaxToolboxMenu menu;         

    }   // class MaxToolboxControl
  



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxDropObject
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxDropObject: System.Windows.Forms.IDataObject
    {
        ArrayList dataObjects = new ArrayList();

        public object GetData(string format)
        {
            return GetData(format, true);
        }
    
        public object GetData(System.Type format)
        {
            foreach (object o in dataObjects) 
            {
                if (o.GetType() == format)  
                    return o;         
            }
            return null;
        }
    
        public object GetData(string str, bool autoConvert)
        {
            foreach (object x in dataObjects)              
                 if (x is Metreos.Max.Core.Tool.MaxTool) return x;           
             
            return null;
        }
    
        public bool GetDataPresent(string format)
        {
            return GetDataPresent(format, true);
        }
    
        public bool GetDataPresent(System.Type format)
        {
            return GetData(format) != null;
        }
    
        public bool GetDataPresent(string format, bool autoConvert)
        {
            // Note this is called when 
            // Object x = data.GetData(Const.toolboxDropObjectClassType);
            // is invoked. "format" parameter has value "FileDrop"
            return GetData(format, autoConvert) != null;
        }
    
        public string[] GetFormats()
        {
            return null;
        }

        public string[] GetFormats(bool autoConvert)
        {                        
            return new string[] {Const.toolboxDropObjectClassString};
        }
    
        public void SetData(object data)
        {
            dataObjects.Add(data);
        }
    
        public void SetData(string format, object data) { }
        public void SetData(System.Type format, object data) { }
        public void SetData(string format, bool autoConvert, object data) { }
    }
}
