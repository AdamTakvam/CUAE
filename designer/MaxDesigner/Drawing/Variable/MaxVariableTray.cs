using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Framework;
using Metreos.Max.Manager;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Drawing;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework.Satellite.Property;
using Crownwood.Magic.Menus;
using Northwoods.Go;

 
namespace Metreos.Max.Drawing
{
    /// <summary>Function variables tray</summary>
    public class MaxVariableTray: UserControl 
    {
        protected VarTrayListView     listview;
        protected MaxFunctionCanvas   canvas;
        protected MaxVariablesManager manager;
        protected Splitter split;
        protected Timer    timer;

        public MaxVariableTray(MaxFunctionCanvas canvas) 
        {
            this.listview = new VarTrayListView(this);
            this.Controls.Add(listview); 
            this.AllowDrop = true;
            this.canvas  = canvas; 

            this.split = new VarTraySplitter();
            this.Dock  = split.Dock = DockStyle.Bottom; 
            this.canvas.TrayFrame = split;

            this.timer = new Timer();
            this.timer.Tick += new EventHandler(OnTimeExpired);
        }


        /// <summary>Post-instantiation initialization</summary>
        public void Create(MaxVariablesManager mgr)
        {
            this.manager = mgr;
            this.Height  = mgr.TrayHeight;  
      
            canvas.View.Controls.Add(split);
            canvas.View.Controls.Add(this); 

            split.Height = 1; 
            split.BorderStyle = BorderStyle.Fixed3D;  
            split.BackColor   = Const.ColorMaxBackground;
        } 


        /// <summary>Show or hide the tray</summary>
        public void Show(bool b)
        {
            heightDelta = Bounds.Height / showDeltas;

            if  (b)
            {
                if (animate) split.SplitPosition = this.Height = currHeight = heightDelta;

                split.Show();
                this.Show();

                if (animate) this.ShowAnimated();
            }
            else
            {
                this.Hide();
                split.Hide();
            }
        }


        /// <summary>Animate display of tray</summary>
        public void ShowAnimated()
        {
            // We're currently not using this
            timer.Interval = showTimeMs / showDeltas;
            showState = 1;
            timer.Start();
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Tray content manipulation and enumeration
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Add a dummy node to empty list indicating empty tray</summary>
        public void AddPlaceholder()
        {
            ListViewItem item = new ListViewItem(Const.variablesTrayText);
            item.ForeColor = Const.ExtralightSlateGray;
            listview.Items.Add(item);
        } 


        /// <summary>Remove dummy from list</summary>
        public void RemovePlaceholder()
        {
            if (listview.Items.Count > 0 && listview.Items[0].Text == Const.variablesTrayText)
                listview.Items.RemoveAt(0);
        } 


        /// <summary>Add a variable to the tray</summary>
        public ListViewItem AddItem(MaxRecumbentVariableNode maxnode, int index) 
        { 
            // Each variables tray item is a ListViewItem member of the tray ListView.
            // Each such item hosts a MaxRecumbentVariableNode which is not a member
            // of the document, and is referenced only for properties purposes. The
            // variable node itself references the ListViewItem to faciliate name changes.

            if (maxnode == null) return null;
            this.RemovePlaceholder();
  
            // The .Add here throws an out of range exception. related to listview 
            // column width. Not sure how to prevent the exception without clearing
            // and reloading the listview. Update 6/27/05 this appears to be a .net
            // framework bug, for which a hotfix is available. See MSKB 899326.
            // We'll wait for the service pack rather than install the fix.  

            ListViewItem item = new ListViewItem(maxnode.NodeName, index);
            item.Tag = maxnode;                   // Set mututal references
            maxnode.UserObject = item;
      
            try   { listview.Items.Add(item); }
            catch { } // ArgumentOutOfRangeException
            
//            this.tooltip = new ToolTip();
//            this.tooltip.InitialDelay = 500;
//            this.tooltip.ReshowDelay = 100;
//            this.tooltip.AutoPopDelay = 5000;
//            this.tooltip.SetToolTip(this, "text of this tooltip\r\nsecond line of text");

            item.EnsureVisible();
            MaxProject.Instance.MarkViewDirty();
            return item;
        }


        /// <summary>Add a variable to the tray</summary>
        public ListViewItem AddItem(MaxRecumbentVariableNode maxnode) 
        { 
            return this.AddItem(maxnode, MaxImageIndex.stockTool16x16IndexVariable); 
        }


        /// <summary>Remove a variable from the tray</summary>
        public void RemoveItem(ListViewItem item) 
        { 
            listview.Items.Remove(item);

            PmProxy.PropertyWindow.Clear(this);

            if (listview.Items.Count == 0)
                this.AddPlaceholder();
        }


        /// <summary>Reload the tray, adjusting slot size to longest variable name</summary>
        public void Reload()
        {
            ListViewItem[] items = new ListViewItem[listview.Items.Count];
            listview.Items.CopyTo(items,0);
            listview.Clear();
            foreach(ListViewItem item in items) listview.Items.Add(item);
        }


        /// <summary>Return array of variable nodes in tray</summary>
        public ArrayList Contents()
        {
            ArrayList a = new ArrayList();

            foreach(ListViewItem item in listview.Items) 
                if (item.Tag is MaxRecumbentVariableNode) 
                {
                   (item.Tag as GoObject).Location = Const.point00;
                    a.Add(item.Tag);
                }

            return a;
        }


        public bool IsFocused { get { return listview.Focused; } }
        public int  Selected  { get { return listview.SelectedItems.Count; } }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Drag/drop
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Indicate via cursor if droppable object is over tray</summary>
        protected override void OnDragOver(DragEventArgs e)
        {              
            Object toolboxObj = e.Data.GetData(Const.toolboxDropObjectClassType);                                             
            Object canvasObj  = e.Data.GetData(Const.canvasDropObjectClassString);

            e.Effect = toolboxObj is MaxVariableTool || canvasObj is GoSelection?
                DragDropEffects.Copy: DragDropEffects.None;      
        }


        /// <summary>Handle drop of an object onto tray</summary>
        protected override void OnDragDrop(DragEventArgs e)
        {
            MaxVariableTool varTool = null;
            MaxRecumbentVariableNode varNode = null;

            Object toolboxObj = e.Data.GetData(Const.toolboxDropObjectClassType);
            Object canvasObj  = e.Data.GetData(Const.canvasDropObjectClassString);

            if  (toolboxObj != null) 
                 varTool = toolboxObj as MaxVariableTool;
            else
            if  (canvasObj != null && canvasObj is GoSelection) 
                 varNode = ((GoSelection)canvasObj).First as MaxRecumbentVariableNode;

            if   (varTool != null) this.DropFromToolbox();
            else
            if   (varNode != null) this.DropFromCanvas(varNode);
        }


        /// <summary>Handle drop of an object dragged from toolbox onto tray</summary>
        public ListViewItem DropFromToolbox()
        {
            this.RemovePlaceholder();

            MaxRecumbentVariableNode node = MaxStockTools.NewMaxVariableNode
                (this.canvas, Framework.Satellite.Property.DataTypes.Type.LocalVariable);
         
            node.Location = Const.point00;        // Indicates tray member
 
            ListViewItem item = this.AddItem(node);

            item.BeginEdit();                     // Open edit session on name

            return item;
        }


        /// <summary>Handle drop of an object dragged from canvas onto tray</summary>
        public ListViewItem DropFromCanvas(MaxRecumbentVariableNode node)
        {
            this.RemovePlaceholder();
            MaxDocument doc = canvas.View.Document as MaxDocument;

            doc.SkipsUndoManager = true;

            doc.Remove(node); 
     
            doc.SkipsUndoManager = false;

            node.Location = Const.point00;        // Indicates tray member
      
            return this.AddItem(node);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Events
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Act on tray animation timer</summary>
        private void OnTimeExpired(Object sender, EventArgs args)
        {
            int newHeight = currHeight += heightDelta;

            if (showState++ >= showDeltas)
                timer.Stop();

            else split.SplitPosition = this.Height = Math.Min(newHeight, Bounds.Height);
        } 


        protected override void OnResize(EventArgs e)    
        {
            base.OnResize(e);
            if (manager != null) manager.TrayHeight = this.Height;
        }


        /// <summary>Handle shortcut keys</summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (Keys.Delete == keyData)
            {
                // Ask for user confirmation to thwart errant 'delete' key hit

                // We must retrieve and store the current variable in this case because we are
                // not able to relying on the EditDelete() overload... prompting for variable
                // delete confirmation steals focus from the variable tray.
                ListViewItem currentVar = SelectedVariable();

                if (currentVar != null && Utl.ShowRemoveVariableDlg(currentVar.Text))
                {
                    EditDelete(currentVar);
                }
            }

            return base.ProcessCmdKey (ref msg, keyData);
        }


        /// <summary> Determines the currently selected variable, and deletes it </summary>
        public void EditDelete()
        {
            ListViewItem item = SelectedVariable();

            if  (item != null)  
            {
                this.RemoveItem(item);           
                MaxProject.Instance.MarkViewDirty();
            }
        }


        /// <summary> Removes the specified variable </summary>
        protected void EditDelete(ListViewItem item)
        {
            if  (item != null)  
            {
                this.RemoveItem(item);           
                MaxProject.Instance.MarkViewDirty();
            }
        }


        /// <summary> Retrieves the currently selected variable </summary>
        protected ListViewItem SelectedVariable()
        {
            return listview.SelectedItems.Count == 0? null:
                   listview.SelectedItems[0];
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Context menu
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected void OnMenuDelete(object sender, EventArgs e)
        {
            ListViewItem item = (sender as MenuCommand).Tag as ListViewItem;
            this.RemoveItem(item);

            MaxProject.Instance.MarkViewDirty();
        }



        protected void OnMenuRename(object sender, EventArgs e)
        {
            ListViewItem item = (sender as MenuCommand).Tag as ListViewItem;
            item.BeginEdit();
        }



        protected void OnMenuProperties(object sender, EventArgs e)
        {
            MaxProject.Instance.ShowPropertiesWindow();
        }



        protected void OnMenuAddNewItem(object sender, EventArgs e)
        {
            this.DropFromToolbox();
        }



        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Properties
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public void SetColumnWidth(int n) { listview.ColumnHeader.Width = n; }

        private int showState, currHeight, heightDelta;
        private const int  showDeltas = 5;
        private const int  showTimeMs = 150;
        private bool  animate = false;

        public static readonly Color trayBackground = Color.FromArgb(238,239,238);  
    

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // List view
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>ListView control hosting a function's variables</summary>
        protected class VarTrayListView: ListView
        {
            public VarTrayListView(MaxVariableTray mvt)
            {
                this.tray = mvt;
                this.View = System.Windows.Forms.View.List;
                this.Dock = DockStyle.Fill;     
                this.BorderStyle = BorderStyle.None;
                this.BackColor = MaxVariableTray.trayBackground; 
                this.Sorting   = SortOrder.None; 
                this.LabelEdit = true;
                //this.tooltip   = new ToolTip();

                this.SmallImageList = MaxImageIndex.Instance.StockToolImages16x16.Imagelist;
                this.Font = new Font
                    (Config.canvasFont.FontFamily, Config.trayFontSize, FontStyle.Regular); 
         
                colhdr = new ColumnHeader();
                colhdr.Text  = Const.emptystr;
                colhdr.Width = -1; 
                this.Columns.Add(colhdr);

                this.CreateMenuContent();
            }

            ColumnHeader colhdr;
            public ColumnHeader ColumnHeader { get { return colhdr; } }


            /// <summary>Show context for tray variable</summary>
            protected void PopContextMenu(MouseEventArgs e, ListViewItem item)
            {
                PopupMenu contextmenu = new PopupMenu();
                mcvRename.Tag = mcvDelete.Tag = mcvProps.Tag = item;
                contextmenu.MenuCommands.AddRange
                    (new MenuCommand[] { mcvRename, mcvDelete, separator, mcvProps } );

                contextmenu.TrackPopup(this.PointToScreen(new Point(e.X, e.Y)));
            }


            /// <summary>Show context for tray</summary>
            protected void PopContextMenu(MouseEventArgs e)
            {
                PopupMenu contextmenu = new PopupMenu();
                contextmenu.MenuCommands.Add(mcvAddItem);

                contextmenu.TrackPopup(this.PointToScreen(new Point(e.X, e.Y)));
            }


            /// <summary>Validate variable name change</summary>
            protected override void OnAfterLabelEdit(LabelEditEventArgs e)
            {     
                ListViewItem item = Items[e.Item];
                MaxRecumbentVariableNode varnode = item.Tag as MaxRecumbentVariableNode;
                e.CancelEdit = e.Label == null || varnode == null;

                if (!e.CancelEdit)     
                {    
                    e.CancelEdit = !tray.canvas.CanNameNode(NodeTypes.Variable, e.Label);

                    if (e.CancelEdit) 
                        Utl.ShowCannotRenameNodeDialog(false);
                    else 
                    {
                        item.Text = varnode.Text = e.Label; 
                        Utl.SetProperty(varnode.MaxProperties, Const.PmVariableName, e.Label);
                        tray.Reload();            // ... to readjust column size
                        item.EnsureVisible();
                        MaxProject.Instance.MarkViewDirty();
                    }
                }
            }


            /// <summary>Handle context click</summary>
            protected override void OnMouseUp(MouseEventArgs e)
            {
                if  (e.Button == MouseButtons.Right) 
                {
                    ListViewItem item = GetItemAt(e.X, e.Y);
            
                    if  (item == null || item.Tag == null)  
                         this.PopContextMenu(e);
                    else this.PopContextMenu(e, item);
                }

                base.OnMouseUp (e);
            }


            /// <summary>Handle double click anywhere in tray</summary>
            protected override void OnDoubleClick(EventArgs e)
            {
                ListViewItem item = this.SelectedItems.Count == 0? null:
                     this.SelectedItems[0];
                if  (item == null) 
                     base.OnDoubleClick(e);
                else item.BeginEdit();     
            }

        
            #if(false)
            protected override void OnMouseMove(MouseEventArgs e)
            {
                base.OnMouseMove(e);
                Point p  = Control.MousePosition;
                Point pt = this.PointToClient(p); 
                ListViewItem item = this.GetItemAt(pt.X, pt.Y);

                if (item == null) return;                               
                
                this.tooltip = new ToolTip();
                this.tooltip.InitialDelay = 500;
                this.tooltip.ReshowDelay  = 500;
                this.tooltip.AutoPopDelay = 2000;
                this.tooltip.ShowAlways   = true;
                this.tooltip.SetToolTip(this, "variable " + item.Text + "\nsecond line of text");
                this.tooltip = null;                
            }
            #endif


            /// <summary>Handle tab among variables</summary>
            protected override void OnSelectedIndexChanged(EventArgs e)
            {
                base.OnSelectedIndexChanged(e);

                ListViewItem item = this.SelectedItems.Count == 0? null:
                    this.SelectedItems[0];
                if (item == null) return;

                MaxRecumbentVariableNode varnode = item.Tag as MaxRecumbentVariableNode;
                if  (varnode == null) 
                     this.SelectedItems.Clear();    // Don't permit select placeholder       
                else                                // First clear canvas selection
                {
                    GoSelection selection = this.tray.canvas.View.Selection;
                    if (selection != null) selection.Clear();
                    // Then act on tray selection
                    PmProxy.ShowProperties(varnode, varnode.PmObjectType); 
                }
            }


            /// <summary>Clear variable selection when focus leaves tray</summary>
            protected override void OnLostFocus(EventArgs e)
            {
                base.OnLostFocus(e);
                this.SelectedItems.Clear();
            }


            #if(false)
            protected override void OnMouseMove(MouseEventArgs e)
            {
                // We only handle mouse move if we're tracking mouse movement entering,
                // leaving, or within tray, i,e, autohide, autoshow. If we implement
                // neither of these features, then we only must handle the drag events
                // to determine when a varible is dragged into the tray.
                tray.manager.OnMouseEnter(e);
            }
            #endif


            protected void CreateMenuContent()
            {
                mcvDelete  = new MenuCommand(Const.menuGenericDelete,    
                             new EventHandler(tray.OnMenuDelete));
                mcvRename  = new MenuCommand(Const.menuGenericRename,    
                             new EventHandler(tray.OnMenuRename));
                mcvProps   = new MenuCommand(Const.menuGenericProperties,    
                             new EventHandler(tray.OnMenuProperties));
                mcvAddItem = new MenuCommand(Const.menuGenericAddNewItem,    
                             new EventHandler(tray.OnMenuAddNewItem));
                separator  = new MenuCommand(Const.dash);
            }
            
            // private ToolTip tooltip;
            private MaxVariableTray tray;
            private MenuCommand mcvProps, mcvRename, mcvDelete, mcvAddItem, separator;

        } // inner class VarTrayListView


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Splitter
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected class VarTraySplitter: Splitter
        {
            // Occasionally the splitter seems to render at a different height than the
            // height specified, e.g. two pixels instead of one. We work around this by
            // specifying the splitter background to be that of the canvas, and specifi-
            // cally drawing a thin line on the splitter.
            protected static Pen pen;
     
            protected override void WndProc(ref Message msg)
            {
                switch(msg.Msg)
                {
                   case Const.WM_PAINT:

                        base.WndProc(ref msg);
                        Graphics g = this.CreateGraphics();
                        g.DrawLine(pen, 0, 0, this.Right, 0);
                        g.Dispose();                          
                        return;       
                }

                base.WndProc(ref msg);
            }

            public VarTraySplitter() 
            { 
                if (pen == null) pen = new Pen(Color.LightSlateGray, 0.5F); 
            }
        } // inner class VarTraySplitter

    } // class MaxVariableTray

}   // namespace
