using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using Crownwood.Magic.Common;
using Crownwood.Magic.Controls;
using Crownwood.Magic.Docking;
using Metreos.Max.Core; 
using Metreos.Max.Drawing; 
using Metreos.Max.GlobalEvents;
using Metreos.Max.Core.Package;
using Metreos.Max.Core.NativeType;
using Metreos.Max.Resources.Images;
using Metreos.Max.Framework;
using Northwoods.Go;



namespace Metreos.Max.Manager
{
    public delegate void MaxTabEventHandler (object sender, MaxTabEventArgs e);
    public delegate void MaxNodeEventHandler(object sender, MaxLocalNodeEventArgs e);

    public interface IMaxViewType
    {
        bool IsNew     { get; set; }
        int  SaveCount { get; }
    }


    /// <summary>Tab and canvas manager</summary>
    public class MaxManager: Crownwood.Magic.Controls.TabControl
    {
        #region singleton
        private MaxManager() {}
        private static MaxManager instance;
  
        public  static MaxManager Instance
        {
                get 
          {
                  if  (instance == null)
            {
                    instance = new MaxManager();
                instance.Init();
            }
              return instance;
          }
        }

        #if (false)
        private void InitializeComponent()
        {
        // 
        // _closeButton
        // 
        this._closeButton.Location = new System.Drawing.Point(230, 17);
        this._closeButton.Name = "_closeButton";
        // 
        // _leftArrow
        // 
        this._leftArrow.Location = new System.Drawing.Point(127, 17);
        this._leftArrow.Name = "_leftArrow";
        // 
        // _rightArrow
        // 
        this._rightArrow.Location = new System.Drawing.Point(17, 17);
        this._rightArrow.Name = "_rightArrow";
        // 
        // MaxManager
        // 
        this.TabStop = true;
        }
        #endif
        #endregion

        private void Init()
        {
            this.RegisterCallbacks();
            this.project = MaxProject.Instance;      
            this.Appearance = VisualAppearance.MultiDocument;  
            this.Style = VisualStyle.IDE;   
            this.Dock  = DockStyle.Fill;      
            this.IDEPixelBorder = true; 
            this.lockedTabIndex = -1;
            this.ImageList = MaxImageIndex.Instance.FrameworkImages16x16.Imagelist;

            this.packages  = MaxPackages.Instance;
            this.nativeTypePackages = MaxNativeTypes.Instance;
        }

        private MaxProject  project; 
        public  MaxProject  Project  { get { return project;  } }

        private MaxPackages packages;
        public  MaxPackages Packages { get { return packages; } }

        private MaxNativeTypes nativeTypePackages;
        public  MaxNativeTypes NativeTypePackages { get { return nativeTypePackages; } }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Events
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Fires project activity event into the global event layer</summary>
        public event GlobalEvents.MaxTabActivityHandler       RaiseTabActivity;
        public event GlobalEvents.MaxStatusBarOutputHandler   RaiseStatusBarActivity;
        public event GlobalEvents.MaxOutputWindowHandler      RaiseOutputWindowActivity;
        public event GlobalEvents.MaxToolboxActivityHandler   RaiseToolboxWindowDataReady;
        public event GlobalEvents.MaxFrameworkActivityHandler RaiseFrameworkActivity;
        public event GlobalEvents.MaxMenuOutputHandler        RaiseMenuActivity;

        private void RegisterCallbacks()
        {
            this.ClosePressed      += new EventHandler(OnTabCloseClicked);
            this.SelectionChanging += new EventHandler(OnTabFrameChanging);
            this.SelectionChanged  += new EventHandler(OnTabFrameChanged); 

            this.RaiseTabActivity            += OutboundHandlers.TabActivityProxy;
            this.RaiseMenuActivity           += OutboundHandlers.MenuOutputProxy; 
            this.RaiseFrameworkActivity      += OutboundHandlers.FrameworkActivityCallback;
            this.RaiseStatusBarActivity      += OutboundHandlers.StatusBarOutputProxy;
            this.RaiseOutputWindowActivity   += OutboundHandlers.OutputWindowProxy;
            this.RaiseToolboxWindowDataReady += OutboundHandlers.ToolboxDataReadyProxy;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Public methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


        /// <summary>Request received from framework to start Max</summary>
        public void Start(MaxFrameworkEventArgs e)
        {
            int packagecount = this.LoadFrameworkPackages();

            // Turn event around indicating whether packages were loaded successfully
            e.MaxEventType = MaxFrameworkEventArgs.MaxEventTypes.Started;

            e.Result = packagecount == 0? 
                MaxFrameworkEventArgs.MaxResults.Error: 
                MaxFrameworkEventArgs.MaxResults.OK;

            project.IdeDirty = true;   // Force ide write

            RaiseFrameworkActivity(this, e);
        }


        /// <summary> Clears and reloads the packages contained in the framework </summary>
        public int ReloadFrameworkPackages()
        {
            return LoadFrameworkPackages();
        }


        /// <summary>Load/deserialize samoa action/event packages and native types packages
        /// </summary>
        /// <returns> The number of action/event packages loaded</returns>
        private int LoadFrameworkPackages()
        {
            LoadTypesPackages();

            return LoadActionEventPackages();
        }


        ///<summary> Loads framework native type packages </summary>
        private void LoadTypesPackages()
        {
            int packagecount = MaxNativeTypes.Instance.Load();
            this.SignalStatusBarActivity(Const.LoadedTypesPackagesMessage(packagecount));
        }


        private int LoadActionEventPackages()
        {
            int  packagecount = this.packages.Load();
            this.SignalStatusBarActivity(Const.LoadedPackagesMessage(packagecount));
            return packagecount;
        }


        /// <summary>Add a tab frame hosting indicated canvas to tab control</summary>
        public Crownwood.Magic.Controls.TabPage AddTab(string tabName, MaxTabContent canvas)
        {
            return this.AddTab(tabName, canvas, -1) ; 
        }


        /// <summary>Add a tab frame hosting indicated canvas to tab control</summary>
        public Crownwood.Magic.Controls.TabPage AddTab(string tabName, MaxTabContent canvas, int img)
        {
            MaxTabEventHandler  tabEventHandler  = new MaxTabEventHandler (this.OnTabEvent);
            MaxNodeEventHandler nodeEventHandler = new MaxNodeEventHandler(this.OnNodeEvent);
            canvas.TabEvent  += tabEventHandler; 
            canvas.NodeEvent += nodeEventHandler;  

            Crownwood.Magic.Controls.TabPage tabpage 
                = this.TabPages.Add(new Crownwood.Magic.Controls.TabPage(tabName, canvas));

            // tabpage.ImageIndex = Config.ShowTabIcons? img: -1;
            tabpage.ImageIndex = img;       // Uses tab control's image list
            return tabpage;
        }


        /// <summary>Navigate to a tab frame, reopening the frame if necessary</summary>
        public Crownwood.Magic.Controls.TabPage GoToTab(string tabName)
        {                    
            if (MaxProject.View.IsClear) return null; 
            MaxCanvas canvas = null;  

            Crownwood.Magic.Controls.TabPage tabpage = this.TabPages[tabName];  

            if (tabpage == null)                  // Had tab frame been closed?
            {                                  
                if  (tabName == MaxProject.CurrentViewName)
                {        
                    switch(MaxProject.CurrentViewType)
                    {
                       case ViewTypes.App:
                            tabpage = MaxProject.CurrentApp.AppTree.TabPage;    
                            break;
                       case ViewTypes.Database:                     
                       case ViewTypes.Installer:
                       case ViewTypes.Locales:
                            tabpage = MaxProject.View.CurrentTab;
                            break;
                    }                       
                                           
                    if (tabpage != null) this.TabPages.Insert(0, tabpage); 
                }
                else
                {
                     switch(MaxProject.CurrentViewType)
                     {
                        case ViewTypes.App:
                             canvas = MaxProject.CurrentApp.Canvases[tabName] as MaxCanvas;    
                             break;
                        case ViewTypes.Database:                        
                        case ViewTypes.Installer:
                        case ViewTypes.Locales:
                             break;
                     } 
   
                    if (canvas != null && canvas.TabPage != null)  
                    {
                        tabpage = canvas.TabPage;    // Reopen function tab                   
                        this.TabPages.Add(tabpage);
                    }
                }
            }

            if  (tabpage == null) return null;

            this.SelectedTab = tabpage;
                                                     // Notify framework explorer
            this.SignalTabActivity(MaxCanvasTabEventArgs.MaxEventTypes.GoTo, tabpage);

            return tabpage;
        }


        /// <summary>Remove specified tab frame from tab control</summary>
        public void RemoveTab(string tabName)
        {
            Crownwood.Magic.Controls.TabPage tabpage = this.TabPages[tabName];
            this.RemoveTab(tabpage);
        }


        /// <summary>Remove specified tab frame from tab control</summary>
        public void RemoveTab(Crownwood.Magic.Controls.TabPage tabpage)
        {
            if  (tabpage == null) return;   

            this.TabPages.Remove(tabpage);
                                                  // Notify framework explorer
            this.SignalTabActivity(MaxCanvasTabEventArgs.MaxEventTypes.Close, tabpage);
            project.MarkViewDirty();
        }


        /// <summary>Clear all tab frames from tab control</summary>
        public void ClearTabs()
        {
            this.TabPages.Clear();
                                                  // Notify framework explorer
            this.SignalTabActivity(MaxCanvasTabEventArgs.MaxEventTypes.Clear);
        }   


        /// <summary>Navigate to tab previously viewed</summary>
        public void GoToPriorTab()
        {
            if (this.priorTab != null) this.GoToTab(this.priorTab.Title);
        }

                                             
        /// <summary>Invoked by event layer on framework request for toolbox content</summary>
        public void OnToolGroupsRequest(MaxToolboxEventArgs e)
        {
            // Turn it around, attaching the collection of packages previously loaded
            e.MaxEventType = MaxToolboxEventArgs.MaxEventTypes.ToolGroupReply;
            e.Payload = this.packages;
            RaiseToolboxWindowDataReady(this, e);
        }


        /// <summary>Fired by canvases to indicate tab change should occur</summary>
        public void OnTabEvent(object sender, MaxTabEventArgs e)
        {
            switch(e.eventtype)
            {
               case MaxTabEventArgs.TabEventType.DoubleClick:
               case MaxTabEventArgs.TabEventType.New:
                    this.OnTabEventDoubleClick(e);
                    break;

               case MaxTabEventArgs.TabEventType.Rename:
                    this.OnTabEventRename(e);
                    break;
            }
        }


        /// <summary>Fired by canvases to indicate node change should occur</summary>
        public void OnNodeEvent(object sender, MaxLocalNodeEventArgs e)
        {
            switch(e.eventtype)
            {
               case MaxLocalNodeEventArgs.NodeEventType.Rename:
                    MaxProject.CurrentApp.OnNodeRenamed(e.Node);     
                    break;
            }
        }


        /// <summary>Handle node double click triggering a tab change</summary>
        public void OnTabEventDoubleClick(MaxTabEventArgs e)
        {
            MaxFunctionCanvas canvas = null;
            bool isNewCanvas = false;
            Crownwood.Magic.Controls.TabPage tabpage = this.TabPages[e.TabName];

            if  (tabpage == null)  
            {
                // If tab does not exist, create canvas now.
                canvas = MaxProject.CurrentApp.OnNewFunction(e.TabName);    

                if  (canvas != null)  
                {    // Get canvas reference to app's function node
                    canvas.AppCanvasNode = e.nodeInfo != null? 
                        e.nodeInfo.node as MaxFunctionNode: null;
                    tabpage = canvas.TabPage;  
                    isNewCanvas = true;
                }        
            }
         
            if (tabpage != null && !isNewCanvas && !e.suppressTabSwitch)  
                this.GoToTab(e.TabName);
        }


        /// <summary>Handle node text change triggering a tab change</summary>
        public void OnTabEventRename(MaxTabEventArgs e) 
        {
            Crownwood.Magic.Controls.TabPage tabpage = this.TabPages[e.OldName]; 
            if  (tabpage == null) return;

            int  index = this.TabPages.IndexOf(tabpage);
            if   (index == -1) return;

            this.TabPages.Remove(tabpage);        // Rename the tab

            tabpage.Title = e.TabName;            // If already exists
            if (!this.IsTab(e.TabName))           // ... don't duplicate  
                this.TabPages.Insert(index, tabpage);
                                                  // Rename the canvas
            project.RenameCanvas(e.OldName, e.TabName);                                                   
                                                  // Notify framework of rename
            MaxCanvasTabEventArgs args = new MaxCanvasTabEventArgs
                (MaxCanvasTabEventArgs.MaxEventTypes.Rename, e.TabName, e.OldName, e.id); 

            RaiseTabActivity(this, args);
        }


        /// <summary>Handle node delete triggering a tab change</summary>
        public void OnTabEventRemove(MaxTabEventArgs e)
        {
            Crownwood.Magic.Controls.TabPage tabpage = this.TabPages[e.TabName]; 
            if  (tabpage == null) return;

            int  index = this.TabPages.IndexOf(tabpage);
            if   (index == -1) return;

            this.TabPages.Remove(tabpage);        

            MaxCanvasTabEventArgs args = new MaxCanvasTabEventArgs
                (MaxCanvasTabEventArgs.MaxEventTypes.Remove, e.TabName, e.TabName, e.id); 

            RaiseTabActivity(this, args);
        }


        /// <summary>Invoked as a tab close X is clicked</summary>
        public void OnTabCloseClicked(object o, EventArgs e)
        {            
            Crownwood.Magic.Controls.TabPage tabpage = this.SelectedTab;
            if  (tabpage == null) return;
            MaxCanvas canvas = tabpage.Control as MaxCanvas; 

            // If canvas disallows tab switch, disallow close and lock tab 
            if  (canvas != null && !canvas.CanLeaveTab())  
                this.lockedTabIndex = this.SelectedIndex; 
            else 
            {
                this.RemoveTab(tabpage);

                if  (this.TabPages.Count < 1) 
                {
                    this.ShowClose = false;
                    this.RaiseMenuActivity(this, CannotSelectAllEventArgs);
                }
            }
        }  


        /// <summary>Fired by tab control on a pending tab switch</summary>
        public void OnTabFrameChanging(object o, EventArgs e)
        {  
            this.lockedTabIndex = -1;
            if  (this.SelectedIndex < 0) return;

            Crownwood.Magic.Controls.TabPage tabpage = this.SelectedTab;
            MaxCanvas canvas = tabpage.Control as MaxCanvas; 

            // If canvas disallows tab switch, lock current tab frame
            if  (canvas != null && !canvas.CanLeaveTab())  
                 this.lockedTabIndex = this.SelectedIndex; 
            else this.priorTab = tabpage;
        }


        /// <summary>Fired by tab control after a tab switch</summary>
        public void OnTabFrameChanged(object o, EventArgs e)
        {  
            int tabindex = this.SelectedIndex; 
            if (tabindex < 0) 
            {
                    this.RaiseMenuActivity(this, CannotSelectAllEventArgs);
                return;
            }

            if  (this.lockedTabIndex != -1)
            { 
                this.SelectedIndex = lockedTabIndex;
                return;
            }

            Crownwood.Magic.Controls.TabPage tabpage = this.SelectedTab;
            if (tabpage == null) return;
                                                  // Notify framework explorer
            this.SignalTabActivity(MaxCanvasTabEventArgs.MaxEventTypes.GoTo, tabpage);

            switch(MaxProject.CurrentViewType)
            {
               case ViewTypes.Database:                        
               case ViewTypes.Installer:
               case ViewTypes.Locales:
               case ViewTypes.Media:
               case ViewTypes.None:
                    this.ShowClose = false;
                    this.RaiseMenuActivity(this, CannotSelectAllEventArgs);
                    this.RaiseMenuActivity(this, IsNotFunctionCanvasEventArgs);
                    break;

               case ViewTypes.App:
                    this.ShowClose = true;
                    this.RaiseMenuActivity(this, CanSelectAllEventArgs);
                    this.RaiseMenuActivity(this, IsFunctionCanvasEventArgs);
                    break;
            }

            MaxTabContent content = tabpage.Control as MaxTabContent;
            if  (content != null) 
                 content.OnTabActivated();

            // Have not yet figured out how to set focus to the current page
            // such that a subsequent shortcut key, such as ctrl-tab, will
            // be recognized. It is currently necessary to click the mouse
            // on the canvas in order to so set focus. Spy++ shows a confusing
            // chain of focus events.
                                                    
            Framework.Satellite.Property.PmProxy.PropertyWindow.Clear(null); 
      
            MaxCanvas canvas = tabpage.Control as MaxCanvas;   
            if (canvas == null) return; 
            canvas.ShowGrid(MaxCanvas.IsGridShown);  
            canvas.View.Focus();

            // When returning to a prior tab, if selection remains, show properties 1227
            MaxSelectableObject primarySelection = canvas.PrimarySelection();
            if (primarySelection != null)  
                Framework.Satellite.Property.PmProxy.ShowProperties
                  (primarySelection, primarySelection.PmObjectType);

            // Visual Studio Note: VS will have to host the managed overview in a
            // separate interop hierarchy. Therefore, we can continue to communicate 
            // directly with the overview, as we do here.
            Framework.Satellite.Overview.MaxOverviewWindow.Instance.ShowOverview(canvas.View);
        }


        /// <summary>Notification from tab control after instantiation</summary>
        public void OnTabControlCreated(Crownwood.Magic.Controls.TabControl tabControl)
        { 
            tabControl.PositionTop = true;
            tabControl.Appearance  = VisualAppearance.MultiForm;
        }


        /// <summary>Determine if a tab exists with specified name</summary>
        public bool IsTab(string s)
        {
            Crownwood.Magic.Controls.TabPage tabpage = this.TabPages[s]; 
            return tabpage != null;
        }


        /// <summary>Return the content of the active tab</summary>
        public MaxTabContent CurrentTabContent()
        {
            Crownwood.Magic.Controls.TabPage currentTab = this.SelectedTab;
            return (currentTab == null)? null: currentTab.Control as MaxTabContent;      
        }


        /// <summary>Handle request to save IDE state</summary>
        public void OnSaveIdeRequest()
        {
            project.SaveIDE(true);
        }


        /// <summary>Return the current application tree</summary>
        public MaxAppTree AppTree()
        {
            MaxAppTree tree      = null;
            MaxAppTree appcanvas = null;
            Crownwood.Magic.Controls.TabPage tabpage = null;
            MaxApp app = MaxProject.CurrentApp; 
            if (app       != null) appcanvas  = app.AppCanvas;
            if (appcanvas != null) tabpage = appcanvas.TabPage;
            if (tabpage   != null) tree    = tabpage.Control as MaxAppTree;
            return tree;
        }


        /// <summary>Navigate view to canvas containing supplied node ID</summary>
        public void NavigateToNode(long nodeID, bool select)
        {
            MaxApp app = MaxProject.CurrentApp;
            if (app != null) NavigateToNode(app.AppName, nodeID);
        }


        /// <summary>Navigate view to canvas containing supplied node ID</summary>
        public void NavigateToNode(string appname, long nodeID)
        {  
            MaxNodeInfo info = this.FindNode(appname, nodeID); 
            if (info != null)  this.NavigateToNode(info.node, true);
        }


        /// <summary>Navigate view to canvas containing supplied node</summary>
        public void NavigateToNode(IMaxNode node, bool select)
        {  
            MaxFunctionCanvas canvas = null;
            if (node != null) canvas = node.Canvas as MaxFunctionCanvas;
            if (canvas == null) return;

            this.GoToTab(canvas.CanvasName);      // Navigate to function tab

            MaxView view = canvas.View;     

            if (select)      
            {
                view.Selection.Clear();           // Select node
                view.Selection.Select(node as GoObject);
            }
                                                  // Scroll view to node
            view.ScrollRectangleToVisible((node as GoObject).Bounds);  
        }


        /// <summary>Find canvas and node for supplied node ID</summary>
        public MaxNodeInfo FindNode(long nodeID)
        {
            MaxApp app = MaxProject.CurrentApp;
            return app == null? null: FindNode(app.AppName, nodeID);
        }


        /// <summary>Find canvas and node for supplied node ID</summary>
        public MaxNodeInfo FindNode(string appname, long nodeID)
        {
            if (MaxProject.View.ViewType != ViewTypes.App) return null;
            if (MaxProject.View.Name != appname) return null;
            IMaxNode  foundnode   = null;
            MaxCanvas foundcanvas = null;

            MaxApp app = MaxProject.CurrentApp;

            foreach(object a in app.Canvases.Values) 
            {     
                MaxFunctionCanvas canvas = a as MaxFunctionCanvas; 
                if (canvas == null) continue; 

                foreach(GoObject x in canvas.View.Document)
                {
                    IMaxNode thisnode = x as IMaxNode; 
                    if (thisnode == null || thisnode.NodeID != nodeID) continue;

                    foundcanvas = canvas;
                    foundnode   = thisnode;  
                    break;
                }
            }

            return new MaxNodeInfo(foundcanvas, foundnode);
        }


        /// <summary>Determine existence of function with specified name</summary>
        public static bool FunctionExists(string name)
        {
            if (name != null)
                foreach(object a in MaxProject.CurrentApp.Canvases.Values) 
                {     
                    MaxFunctionCanvas canvas = a as MaxFunctionCanvas; 
                    if (canvas != null && canvas.CanvasName == name) return true;
                }
            return false;
        }


        public void SetFocusCanvas()
        {
            MaxCanvas canvas = null;
            Crownwood.Magic.Controls.TabPage tabpage = this.SelectedTab;
            if (tabpage != null) canvas = tabpage.Control as MaxCanvas; 
            if (canvas  != null) canvas.View.Focus();
        }


        public void SetFocusTray()
        {
            MaxFunctionCanvas canvas = null;
            Crownwood.Magic.Controls.TabPage tabpage = this.SelectedTab;
            if (tabpage != null) canvas = tabpage.Control as MaxFunctionCanvas; 
            if (canvas  != null) canvas.Tray.Focus();
        }


        /// <summary>Fire text message up to framework for display</summary>
        public void Trace(string text)
        {
            SignalOutputWindowActivity(text);
        }


        /// <summary>Fire text message up to framework for display</summary>
        public void SignalFrameworkTextMessage(string text, bool toOutput, bool toStatusBar)
        {
            if  (toOutput)    SignalOutputWindowActivity(text);
            if  (toStatusBar) SignalStatusBarActivity   (text);
        }


        /// <summary>Fire tab activity up to framework</summary>
        public void SignalTabActivity(MaxCanvasTabEventArgs.MaxEventTypes type)
        {   
            MaxCanvasTabEventArgs args = new MaxCanvasTabEventArgs(type);     
            RaiseTabActivity(this, args);
        }


        /// <summary>Fire tab activity up to framework</summary>
        public void SignalTabActivity
        ( MaxCanvasTabEventArgs.MaxEventTypes type, Crownwood.Magic.Controls.TabPage tabpage)
        {
            if (tabpage == null) return;
            MaxCanvasTabEventArgs args = new MaxCanvasTabEventArgs(type, tabpage.Title, tabpage);     
            RaiseTabActivity(this,args);
        }


        /// <summary>Fire output window message up to framework</summary>
        public void SignalOutputWindowActivity(string text)
        {
            RaiseOutputWindowActivity(this, new MaxOutputWindowEventArgs(text));
        }


        /// <summary>Fire status bar text up to framework</summary>
        public void SignalStatusBarActivity(string text)
        {
            RaiseStatusBarActivity(this, new MaxStatusBarOutputEventArgs(text));
        }

        protected static MaxMenuOutputEventArgs CanSelectAllEventArgs 
            = new MaxMenuOutputEventArgs
            (MaxMenuOutputEventArgs.MaxEventTypes.CanSelectAll, true);
        protected static MaxMenuOutputEventArgs CannotSelectAllEventArgs 
            = new MaxMenuOutputEventArgs
            (MaxMenuOutputEventArgs.MaxEventTypes.CanSelectAll, false);
        protected static MaxMenuOutputEventArgs IsFunctionCanvasEventArgs 
            = new MaxMenuOutputEventArgs
            (MaxMenuOutputEventArgs.MaxEventTypes.IsFunctionCanvas, true);
        protected static MaxMenuOutputEventArgs IsNotFunctionCanvasEventArgs 
            = new MaxMenuOutputEventArgs
            (MaxMenuOutputEventArgs.MaxEventTypes.IsFunctionCanvas, false);

        /// <summary>Global flag indicating script is currently loading</summary>
        protected static bool deserializing;
        public    static bool Deserializing 
        {
            get { return deserializing; }   
            set 
            {
                deserializing = value;
                Metreos.Max.Framework.Satellite.Property.MaxPropertiesManager.Deserializing = value;
            } 
        }

        /// <summary>Global flag for various on the fly upgrades</summary>
        protected static bool upgrading;
        public    static bool Upgrading 
        { get { return upgrading; } set { upgrading = value; } }  
    
        protected static bool pasting;
        public    static bool Pasting 
        { get { return pasting;  }  set { pasting = value; } }

        protected static bool shuttingdown;
        public    static bool ShuttingDown 
        { get { return shuttingdown;  }  set { shuttingdown = value; } }
                                         
        private int lockedTabIndex;         // Prevent switch away from this tab
        private Crownwood.Magic.Controls.TabPage priorTab;

        private Crownwood.Magic.Docking.DockingManager dockmgr;
        public  Crownwood.Magic.Docking.DockingManager DockMgr
        { get { return dockmgr;  }  set { dockmgr = value; } }   
    } // class MaxManager



    public class MaxTabEventArgs: EventArgs
    {
        public enum TabEventType {None, New, DoubleClick, Delete, Rename}
        public readonly TabEventType eventtype;
        public readonly string  TabName;
        public readonly string  OldName;
        public readonly long    id;
        public MaxNodeInfo  nodeInfo;
        public bool suppressTabSwitch;

        public readonly MaxCanvas.CanvasTypes canvasType;
    
        public MaxTabEventArgs(string s, MaxCanvas.CanvasTypes ct) 
        {
            TabName = s; canvasType = ct; eventtype = TabEventType.DoubleClick; 
        }

        public MaxTabEventArgs(MaxNodeInfo info) 
        {
            eventtype  = TabEventType.New; TabName = info.node.NodeName; 
            canvasType = info.canvas.CanvasType; 
        }

        public MaxTabEventArgs(string s, MaxCanvas.CanvasTypes ct, TabEventType et) 
        {
            TabName = s; canvasType = ct; eventtype = et; 
        }

        public MaxTabEventArgs(string sn, string so, long id, MaxCanvas.CanvasTypes ct) 
        {
            TabName = sn; OldName = so; canvasType = ct; this.id = id; eventtype = TabEventType.Rename; 
        }
    } 


    public class MaxLocalNodeEventArgs: EventArgs
    {
        public enum NodeEventType {None, Delete, Rename}
        public readonly NodeEventType eventtype;
        public readonly string    NodeName;
        public readonly string    OldName;
        public readonly long      id;
        public readonly IMaxNode  Node;

        public MaxLocalNodeEventArgs(string oldname, string newname, IMaxNode node) 
        {
            NodeName  = newname; OldName = oldname; Node = node; 
            eventtype = NodeEventType.Rename;  id = node.NodeID;      
        }
    } 


    public class MaxNodeInfo
    {
        public readonly MaxCanvas canvas;
        public readonly IMaxNode  node;
        public MaxNodeInfo(MaxCanvas c, IMaxNode n) { canvas = c; node = n; }
    }
 
} // namespace
