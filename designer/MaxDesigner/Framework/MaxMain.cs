//
// MaxMain
//
using System;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Collections;
using System.Runtime.InteropServices;
using Crownwood.Magic.Common;
using Crownwood.Magic.Controls;
using Crownwood.Magic.Docking;
using Crownwood.Magic.Menus;
using Metreos.Max.Core;
using Metreos.Max.Core.Tool;
using Metreos.Max.Core.Package;
using Metreos.Max.Core.NativeType;
using Metreos.Max.Manager; 
using Metreos.Max.Drawing;
using Metreos.Max.Debugging; 
using Metreos.Max.GlobalEvents; 
using Metreos.Max.Framework.Satellite;
using Metreos.Max.Framework.Satellite.Output;
using Metreos.Max.Framework.Satellite.Explorer;
using Metreos.Max.Framework.Satellite.Overview;
using Metreos.Max.Framework.Satellite.Property;
using Metreos.Max.Framework.Satellite.Toolbox;
using Metreos.Max.Resources.Images;
using Metreos.Max.Resources.XML;
using Metreos.AppArchiveCore;
using Northwoods.Go;
                                           
 

namespace Metreos.Max.Framework
{
    /// <summary>IDE: main frame window, menus, tab frame, and docking manager</summary>
    public class MaxMain: Form
    {
        [STAThread]
        public static int Main(string[] args)
        {    
            MaxMain.messageWriter = new MaxMessageWriter(null);
                                             
            int cmdlineErrors = MaxCommandLine.Parse(args);  
            if (cmdlineErrors > 0) return Const.retcodeCmdLineError;
            if (MaxMain.autobuild) return MaxMain.Autobuild();
                         
            Application.EnableVisualStyles(); // Enable XP styles
            Application.DoEvents();           // Work around MS ShowDialog() bug
            // Workaround for "NullReferenceException occurred in Unknown Module" 
            Debug.WriteLine(Const.emptystr);  // Workaround for .net null ref excp
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolveHelper);

            Application.Run(new MaxMain());       

            return maxretcode;
        }



        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Properties
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    
        private MaxManager        tabs;
        private static DockingManager  dockmgr;
        public  static DockingManager  DockMgr  { get { return dockmgr;  } }
        private StatusBar         statusBar;
        public  StatusBar         StatusBar     { get { return statusBar;} }
        private VisualStyle       style;
        public  VisualStyle       Style         { get { return style;    } }

        private static StatusBarPanel statusMsg;
        public  static StatusBarPanel StatusMsg { get { return statusMsg;} }

        private MaxMenu           mainMenu;
        public  MaxMenu           MainMenu      { get { return mainMenu; } }

        private static Content    propertyWindow;
        public  static Content    PropertyWindow        { get { return propertyWindow;} }
        private MaxPropertyWindow propertyWindowControl;
        public  MaxPropertyWindow PropertyWindowControl { get { return propertyWindowControl; } }

        private Content           toolboxWindow;
        public  Content           ToolboxWindow         { get { return toolboxWindow; } }
        private MaxToolboxWindow  toolbox;
        public  MaxToolboxWindow  Toolbox               { get { return toolbox;       } }

        private static Content    outputWindow;
        public  static Content    OutputWindow          { get { return outputWindow;  } }
        private MaxOutputWindow   outputWindowControl;
        public  MaxOutputWindow   OutputWindowControl   { get { return outputWindowControl; } }
        public  MaxOutputWindow   Output                { get { return outputWindowControl; } }

        private Content           explorerWindow;
        public  Content           ExplorerWindow        { get { return explorerWindow; } }
        private MaxExplorerWindow explorer;
        public  MaxExplorerWindow Explorer              { get { return explorer; } }
        private static MaxExplorerWindow xplorer;         
        public  static MaxExplorerWindow Xplorer        { get { return xplorer;  } } 
        // Need a static reference to explorer due to integration. Used a second reference
        // to avoid changing the many instance references to main.Explorer for the present.

        private Content           overviewWindow;
        public  Content           OverviewWindow        { get { return overviewWindow; } }
        private MaxOverviewWindow overview;
        public  MaxOverviewWindow Overview              { get { return overview; } }

        public  static Timer statusBarTimer = new Timer();
        private AsyncStates  asyncState;
        public  AsyncStates  AsyncState{ get { return asyncState; } set { asyncState = value; } }

        public  static string ProjectName     { get { return projectFilename; } }
        public  static string ProjectFilename { get { return projectFilename; } }
        public  static string ProjectFolder   { get { return projectFolder;   } }
        public  static string ProjectPath     { get { return projectPath; } }
        public  bool   ProjectDirty    { get { return projectDirty;} }
        public  bool   ProjectExists   { get { return projectPath != null; } }
        public  bool   ProjectBuilt    { get { return built;    } set { built    = value; } }
        public  static bool   IdeDirty { get { return ideDirty; } set { ideDirty = value; } }
        private static string projectFilename = null;
        private static string projectFolder   = null;
        private static string projectPath     = null;
        private static bool   projectDirty    = false;
        private static bool   ideDirty        = false;
        private static bool   built           = false;
        private static int    hHook           = 0;
    
        private static CurrentView view;
        public  static CurrentView View  { get { return view;      } }
        public  string CurrentViewName   { get { return view.Name; } }
        public  string CurrentTab        { get { return currentTab;} }
        private string currentTab = null;
        public  ViewTypes CurrentViewType{ get { return view.ViewType;} }

        public  MaxMainUtil   MainX      { get { return thisx; } }
        public  MaxMainDialog Dialog     { get { return thisi; } }
        private MaxMainDialog thisi;
        private MaxMainUtil   thisx;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Events
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Fires inbound toolbox data request event</summary>
        public event GlobalEvents.MaxToolboxActivityHandler    RaiseToolboxDataRequest;
        /// <summary>Fires inbound tab change request event</summary>
        public event GlobalEvents.MaxTabActivityHandler        RaiseTabChangeRequest; 
        /// <summary>Fires inbound tab close request event</summary>
        public event GlobalEvents.MaxTabActivityHandler        RaiseTabCloseRequest; 
        /// <summary>Fires inbound Max start request event</summary>
        public event GlobalEvents.MaxFrameworkActivityHandler  RaiseFrameworkEvent;
        /// <summary>Fires inbound properties activity event</summary>
        public event GlobalEvents.MaxPropertiesActivityHandler RaisePropertiesEvent;
        /// <summary>Fires inbound user input event</summary>
        public event GlobalEvents.MaxUserInputHandler          RaiseUserInput;
        public void  SignalUserInput(MaxUserInputEventArgs e) {RaiseUserInput(this, e);}

        private static MaxCanvasTabEventArgs tabChangeArgs  
            = new MaxCanvasTabEventArgs(MaxCanvasTabEventArgs.MaxEventTypes.GoTo);
        private static MaxCanvasTabEventArgs tabToggleArgs  
            = new MaxCanvasTabEventArgs(MaxCanvasTabEventArgs.MaxEventTypes.Toggle);
        private static MaxCanvasTabEventArgs tabCloseArgs  
            = new MaxCanvasTabEventArgs(MaxCanvasTabEventArgs.MaxEventTypes.Close);
        private static MaxPropertiesEventArgs propertiesChangedArgs  
            = new MaxPropertiesEventArgs(MaxPropertiesEventArgs.MaxEventTypes.Changed);


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Methods 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public MaxMain()
        {
            OutboundHandlers.Set(this);
            this.style = VisualStyle.IDE;  
            this.thisx = new MaxMainUtil(this); 
            this.thisi = new MaxMainDialog(this);
            view       = new CurrentView(); view.main = this;
            this.WindowState = FormWindowState.Minimized;  // Hide while IDE painting       
            InitializeComponent();
            InitializeIDE(); 

            RaiseToolboxDataRequest += InboundHandlers.ToolboxActivityCallback;
            RaiseTabChangeRequest   += InboundHandlers.TabRequestCallback;
            RaiseTabCloseRequest    += InboundHandlers.TabRequestCallback;
            RaiseFrameworkEvent     += InboundHandlers.FrameworkRequestCallback;
            RaisePropertiesEvent    += InboundHandlers.PropertiesActivityCallback;
            RaiseUserInput          += InboundHandlers.UserInputCallback;
            Application.ApplicationExit += new EventHandler(OnMaxExit);       

            // Inform Max manager that the IDE is complete, so Max can start
            RaiseFrameworkEvent(this, new MaxFrameworkEventArgs
                (MaxFrameworkEventArgs.MaxEventTypes.Start));
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Startup methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Post-window-creation initialization</summary>
        protected override void OnLoad(EventArgs e)
        {      
            base.OnLoad (e);

            // Set up debugger windows tabbed region        
            MaxDebugger.Instance.Init(outputWindow);    

            // Open initial project if so requested or configured
            if (cmdlinePath != null)      
                this.OpenInitialProject(cmdlinePath[0]);
            else
            if (Config.LoadLastSavedOnStartup) 
                this.OpenInitialProject(null);

            // Show main window and restore window placement
            new MaxWindowPlacement().Restore(this); 

            // Force scroll to occur for load msgs
            Output.WriteLine(String.Empty);                                           
           
            isInitialOpen = false;
            isIdeLoaded   = true;

            MaxMain.SetKeyboardHook();

            this.OnIdeOpened();
        }


        /// <summary>Actions on IDE opened, visible, and idle</summary>
        protected void OnIdeOpened()
        {
            // Show user any missing packages, if any
            Utl.MissingPackagesProc(Utl.MissingPackageActions.Show); 

            // If IDE open with no project open, pop open project dialog 20060914
            MaxPackages pkgs = MaxPackages.Instance;
            int packagecount = pkgs.Packages == null? 0: pkgs.Packages.Count;
            if (!Config.SuppressNewProjectDialog && !this.ProjectExists && packagecount > 0) 
                this.mainMenu.Handlers.OnFileOpen(null,null);
        }


        /// <summary>Instantiate tab frame and docking windows</summary>
        protected void InitializeIDE()
        {
            // This is executed prior to OnLoad. Note that docking window content
            // content must exist prior to loading in a saved IDE configuration.

            // Release build static intitialization timing issues require our    
            // instantiation of some singletons in advance of reference to their  
            // statically intitialized properties. Of course, the non-framework
            // singleton instantiation will be moved elsewhere for the VS version.

            Config config  = Config.Instance;                                
            Const  consts  = Const.Instance;
            MaxManager  mm = MaxManager.Instance;  
            MaxProject  mp = MaxProject.Instance; 
            MaxDebugger md = MaxDebugger.Instance;       
            InboundHandlers ih = InboundHandlers.Instance;  
      
            this.tabs = MaxManager.Instance;  
            Controls.Add(this.tabs);
  
            SetStyle(ControlStyles.DoubleBuffer, true);      
            SetStyle(ControlStyles.UserPaint,    true);      
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                                            
            dockmgr = new DockingManager(this, this.style);
            tabs.DockMgr = dockmgr;
                                            // Dock mgr notifications
            dockmgr.ContextMenu       += new DockingManager.ContextMenuHandler(OnDockingContextMenu);
            dockmgr.ContentHiding     += new DockingManager.ContentHidingHandler(OnContentHiding);
            dockmgr.ContentHidden     += new DockingManager.ContentHandler(OnContentHidden);
            dockmgr.ContentShown      += new DockingManager.ContentHandler(OnContentShown);
            dockmgr.TabControlCreated += new DockingManager.TabControlCreatedHandler(tabs.OnTabControlCreated);

            dockmgr.InnerControl = this.tabs; 
            dockmgr.ResizeBarVector = -1;   // -1 is default
                                
            this.statusBar = new StatusBar();           
            this.statusBar.Dock = DockStyle.Bottom;
            this.statusBar.ShowPanels = true;
            statusBarTimer.Tick += new EventHandler(OnStatusBarTimeExpired);
                                             
            statusMsg = new StatusBarPanel(); // Single status bar panel  
            statusMsg.AutoSize = StatusBarPanelAutoSize.Spring;
            this.statusBar.Panels.Add(statusMsg);
            Controls.Add(this.statusBar);

            // We wait to create the main menu until now, since otherwise the 
            // docking windows would be permitted to usurp space from the menu bar
            this.mainMenu = md.Menu = new MaxMenu(this);
            MenuControl menu = this.mainMenu.Create();
            Controls.Add(menu);

            // Ensure docking occurs after menu control and status bar controls
            dockmgr.OuterControl = this.statusBar;

            // Create all satellites. The initial configuration will be overridden 
            // when we load a saved configuration.
            this.CreateSatelliteWindows();

            this.isIdeStarted = true;   

        } // InitializeIDE()


        /// <summary>Initial creation of docking satellite windows</summary>
        private void CreateSatelliteWindows()
        {
            // Note that Crownwood cannot do WindowContent with arbitrarily sized Content.
            // Each pane will be sized equally vertically. The edge opposite the dock can  
            // be expanded by altering the Content.DisplaySize, thus for example making
            // a left dock wider, but the size has no effect on the pane height in that
            // same WindowContent. The way around this is to adjust sizes with the mouse, 
            // save the configuration to a file, and restore the configuration on startup.

            CreatePropertyWindow();
            CreateToolboxWindow();
            WindowContent wcp = dockmgr.AddContentWithState
                (toolboxWindow, State.DockRight) as WindowContent;
            dockmgr.AddContentToZone(propertyWindow, wcp.ParentZone, 1);

            CreateOutputWindow();  
            dockmgr.AddContentWithState(outputWindow, State.DockBottom); 
                                            
            MaxDebugger.Instance.CreateDebugWindows      
                (outputWindow.ParentWindowContent as WindowContentTabbed, this);

            CreateOverviewWindow();                
            CreateExplorerWindow();  
              
            WindowContent wce = dockmgr.AddContentWithState
                (explorerWindow, State.DockLeft) as WindowContent;
                                            // Overview to bottom left
            dockmgr.AddContentToZone(overviewWindow, wce.ParentZone, 1); 
        }

        #region satellite windows instantiation
        /// <summary>Initial creation of property grid and its docking frame</summary>
        private void CreatePropertyWindow()
        {
            if (propertyWindow != null) return;

            propertyWindowControl = MaxPropertyWindow.Instance;
            propertyWindowControl.Create(this);
 
            propertyWindow = dockmgr.Contents.Add(propertyWindowControl, Const.PropertiesWindowTitle);
            SetDockingWindowState(propertyWindow);   

            propertyWindow.ImageList  = MaxImageIndex.Instance.StockToolImages16x16.Imagelist;
            propertyWindow.ImageIndex = MaxImageIndex.stockTool16x16IndexProperties;
        }


        /// <summary>Initial creation of overview window and its docking frame</summary>
        private void CreateOverviewWindow()
        {
            if (this.overviewWindow != null) return;
            overview = MaxOverviewWindow.Instance;
            overviewWindow = dockmgr.Contents.Add(overview, Const.OverviewWindowTitle);
            SetDockingWindowState(overviewWindow);
            overviewWindow.ImageList  = MaxImageIndex.Instance.StockToolImages16x16.Imagelist;
            overviewWindow.ImageIndex = MaxImageIndex.stockTool16x16IndexOverview;
        }


        /// <summary>Initial creation of output window and its docking frame</summary>
        private void CreateOutputWindow()
        {
            if (outputWindow != null) return;
            outputWindowControl = new MaxOutputWindow(this);
            outputWindow = dockmgr.Contents.Add(outputWindowControl, Const.OutputWindowTitle);
            outputWindow.PropertyChanged += new Content.PropChangeHandler
                (MaxDebugger.Instance.Util.OnContentPropertyChanged);
            SetDockingWindowState(outputWindow);
            MaxMain.messageWriter   = new MaxMessageWriter(outputWindowControl);
            outputWindow.ImageList  = MaxImageIndex.Instance.StockToolImages16x16.Imagelist;
            outputWindow.ImageIndex = MaxImageIndex.stockTool16x16IndexOutput;
        }


        /// <summary>Initial creation of explorer window and its docking frame</summary>
        private void CreateExplorerWindow()
        {
            if (this.explorerWindow != null) return;
            explorer = new MaxExplorerWindow(this);
            xplorer  = explorer;                  // Will be replaced asap 1009
            explorerWindow = dockmgr.Contents.Add(explorer, Const.ExplorerWindowTitle);
            SetDockingWindowState(explorerWindow);
            explorerWindow.ImageList  = MaxImageIndex.Instance.StockToolImages16x16.Imagelist;
            explorerWindow.ImageIndex = MaxImageIndex.stockTool16x16IndexExplorer;
        }


        /// <summary>Initial creation of toolbox window and its docking frame</summary>
        protected void CreateToolboxWindow()     
        {
            if (this.toolboxWindow != null) return;
            this.toolbox  = new MaxToolboxWindow(this);  
            toolboxWindow = dockmgr.Contents.Add(toolbox, Const.ToolboxWindowTitle);       
            // toolboxWindow.PropertyChanged 
            // += new Content.PropChangeHandler(OnContentPropertyChanged); 
               
            SetDockingWindowState (toolboxWindow);
            toolboxWindow.ImageList  = MaxImageIndex.Instance.StockToolImages16x16.Imagelist;
            toolboxWindow.ImageIndex = MaxImageIndex.stockTool16x16IndexToolbox;     
            MaxToolboxHelper.toolbox = this.toolbox;  
        }

        #region OnContentPropertyChanged
#if(false)
    private void OnContentPropertyChanged(Content obj, Content.Property prop)
    {
      Content.Property p = prop;
      Size s = obj.DisplaySize;
    }
#endif
        #endregion

        #endregion


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Project event pre-processing 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>On startup, open project specified or most recently saved</summary>    
        public void OpenInitialProject(string path)      
        {
            string projectpath = path == null? Config.RecentFiles.First: path;
            isInitialOpen = true;

            MaxUserInputEventArgs args 
                = thisi.InitiateOpenProjectSequence(projectpath);

            if (args != null) RaiseUserInput(this, args);
        }


        /// <summary>Prompt for script name, returning event to fire</summary>    
        public MaxUserInputEventArgs PromptNewScript()      
        {
            if (!thisi.PromptNewApplication()) return null;

            return thisi.OnNewScriptRequest();
        }


        /// <summary>Prompt for script name, returning event to fire</summary>    
        public MaxUserInputEventArgs PromptExistingScript()      
        {
            string appfilePath = thisi.PromptAddExistingApplication();
            if (appfilePath == null) return null;

            return thisx.ValidateExistingScriptFile(appfilePath)?            
                thisi.OnExistingScriptRequest(prompted.AppName, prompted.Trigger):
                null;
        }


        /// <summary>Invoked to solicit confirmation and remove app script</summary>
        public void PromptRemoveScript(string name)
        {
            if (DialogResult.OK == Utl.ShowRemoveFromProjectConfirmDlg(Const.AppLiteral, name)) 
            {        
                RaiseUserInput(this, new MaxUserInputEventArgs
                    (MaxUserInputEventArgs.MaxEventTypes.RemoveScript, name)); 

                if (view.IsApp && view.Name == name) view.Clear(); 
            } 
        }


        /// <summary>Prompt for installer name, returning event to fire</summary>    
        public MaxUserInputEventArgs PromptNewInstaller()      
        {
            if (!thisi.PromptAddNewInstaller()) return null;

            return thisi.OnNewInstallerRequest();
        }


        /// <summary>Prompt for installer name, returning event to fire</summary>    
        public MaxUserInputEventArgs PromptExistingInstaller()      
        {
            string installerPath = thisi.PromptAddExistingInstaller();
            if (installerPath == null) return null;

            return thisx.ValidateExistingInstallerFile (installerPath)?            
                thisi.OnExistingInstallerRequest(prompted.InstallerPath):
                null;
        }


        /// <summary>Invoked to solicit confirmation and remove installer</summary>
        public void PromptRemoveInstaller()
        {
            if (DialogResult.OK == Utl.ShowRemoveFromProjectConfirmDlg(Const.Installer, Const.emptystr)) 
            {        
                RaiseUserInput(this, new MaxUserInputEventArgs
                    (MaxUserInputEventArgs.MaxEventTypes.RemoveInstaller, null)); 

                if (view.IsInstaller && view.Name == Const.Installer) view.Clear(); 
            } 
        }



        /// <summary>Prompt for locales name, returning event to fire</summary>    
        public MaxUserInputEventArgs PromptNewLocales()
        {
            if (!thisi.PromptAddNewLocales()) return null;

            return thisi.OnNewLocalesRequest();
        }


        /// <summary>Prompt for locales name, returning event to fire</summary>    
        public MaxUserInputEventArgs PromptExistingLocales()
        {
            string localesPath = thisi.PromptAddExistingLocales();
            if (localesPath == null) return null;

            return thisx.ValidateExistingLocalesFile(localesPath) ?
                thisi.OnExistingLocalesRequest(prompted.LocalesPath) :
                null;
        }


        /// <summary>Invoked to solicit confirmation and remove locales</summary>
        public void PromptRemoveLocales()
        {
            if (DialogResult.OK == Utl.ShowRemoveFromProjectConfirmDlg(Const.Locales, Const.emptystr))
            {
                RaiseUserInput(this, new MaxUserInputEventArgs
                    (MaxUserInputEventArgs.MaxEventTypes.RemoveLocales, null));

                if (view.IsLocales && view.Name == Const.Locales) view.Clear();
            }
        }


        /// <summary>Prompt for database name, returning event to fire</summary>    
        public MaxUserInputEventArgs PromptNewDatabase()      
        {
            if (!thisi.PromptAddNewDatabase()) return null;

            return thisi.OnNewDatabaseRequest();
        }


        /// <summary>Prompt for database name, returning event to fire</summary>    
        public MaxUserInputEventArgs PromptExistingDatabase()      
        {
            string filePath = thisi.PromptAddExistingDatabase();
            if (filePath == null) return null;

            return thisi.OnExistingDatabaseRequest(filePath);
        }


        /// <summary>Invoked to solicit confirmation and remove database</summary>
        public void PromptRemoveDatabase(string name)
        {
            if (DialogResult.OK == Utl.ShowRemoveFromProjectConfirmDlg(Const.Database, name))         
            {
                RaiseUserInput(this, new MaxUserInputEventArgs
                    (MaxUserInputEventArgs.MaxEventTypes.RemoveDatabase, name));  

                if (view.IsDatabase && view.Name == name) view.Clear(); 
            } 
        }


        /// <summary>Invoked to solicit filename and add a media file to project</summary>
        public void OnAddMediaFile()
        {
            OnAddMediaFile(null);
        }


        /// <summary>Invoked to solicit filename and add a media file to project</summary>
        public void OnAddMediaFile(CultureInfo forceCulture)
        {
            string audioFilePath;
            CultureInfo audioFileLocale;

            bool choiceMade = thisi.PromptAddAudioFile(forceCulture, out audioFilePath, out audioFileLocale);
            if (!choiceMade) return;

            this.explorer.AddMedia(audioFilePath, audioFileLocale);

            // We fire inbound event only to ensure project dirty
            RaiseUserInput(this, new MaxUserInputEventArgs
                (MaxUserInputEventArgs.MaxEventTypes.AddExistingMediaFile, Const.emptystr, audioFilePath));
        }


        /// <summary>Invoked to solicit confirmation and remove a media file from project</summary>
        public void OnRemoveMediaFile(string filename, string locale)
        {
            DialogResult result = Utl.ShowRemoveFromProjectConfirmDlg(Const.MediaFile, filename);
            if (result != DialogResult.OK) return;

            this.explorer.RemoveMedia(filename, locale);

            // We fire inbound event only to ensure project dirty
            RaiseUserInput(this, new MaxUserInputEventArgs
                (MaxUserInputEventArgs.MaxEventTypes.RemoveMediaFile, Const.emptystr, filename));
        }


        /// <summary>Invoked to solicit filename and add a voice rec resource to project</summary>
        public void OnAddVoiceRecResource()
        {      
            string filePath = thisi.PromptAddVoiceRecResource();
            if (filePath == null) return;
       
            this.explorer.AddVoiceRecResource(filePath);

            // We fire inbound event only to ensure project dirty
            RaiseUserInput(this, new MaxUserInputEventArgs
                (MaxUserInputEventArgs.MaxEventTypes.AddExistingVrResxFile, Const.emptystr, filePath));
        }


        /// <summary>Invoked to solicit confirmation and remove a voice rec resource from project</summary>
        public void OnRemoveVoiceRecResource(string filename)
        {
            DialogResult result = Utl.ShowRemoveFromProjectConfirmDlg(Const.VrResxFile, filename);
            if (result != DialogResult.OK) return;

            this.explorer.RemoveVoiceRecResource(filename);

            // We fire inbound event only to ensure project dirty
            RaiseUserInput(this, new MaxUserInputEventArgs
                (MaxUserInputEventArgs.MaxEventTypes.RemoveVrResxFile, Const.emptystr, filename));
        }


        /// <summary>Build project, saving first if necessary</summary>
        public MaxUserInputEventArgs OnBuildProjectRequest()      
        {
            if  (!this.ProjectExists) return null;

            // If the project is dirty, we request a save of the project, continuing 
            // with the build request once the reply is received from the save request. 
            // If the project is not dirty, we instead continue with the build now.

            MaxUserInputEventArgs args = MaxMain.projectDirty?

                this.CloseImplicit():
           
                new MaxUserInputEventArgs
                   (MaxUserInputEventArgs.MaxEventTypes.Build, null);

            return args; 
        }


        /// <summary>Execute a command line build</summary>
        private static int Autobuild()
        {
            // Load packages
            MaxPackages.Instance.Load();

            for (int i = 0; i < MaxMain.cmdlinePath.Length; i++)
            {
                MaxMain.projectPath = MaxMain.cmdlinePath[i];
                MaxMain.projectFilename = Path.GetFileNameWithoutExtension(MaxMain.projectPath);
                MaxMain.projectFolder = Path.GetDirectoryName(MaxMain.projectPath);

                if (MaxMain.projectFolder == null || MaxMain.projectFolder.Length == 0)
                    MaxMain.projectFolder = ".\\";
                if (MaxMain.projectPath.Equals(MaxMain.projectFilename + ".max"))
                    MaxMain.projectPath = ".\\" + MaxMain.projectPath;

                // Load project packages
                MaxMain.LoadProjectSpecificPackages(MaxMain.ProjectPath);

                MaxMain.OnBuildProject();
            }
            return MaxMain.built? 0: Const.retcodeBuildError;
        }


        /// <summary>Deploy project, save and/or building if necessary</summary>
        public MaxUserInputEventArgs OnDeployProjectRequest()
        {
            if  (!this.ProjectExists) return null;

            if  (!thisx.GetAuthenticationInfo()) return null;  

            if  (this.ProjectDirty) return CloseImplicit();

            else 
            if (!this.ProjectBuilt) 
                 return new MaxUserInputEventArgs
                    (MaxUserInputEventArgs.MaxEventTypes.Build, null);

            else return new MaxUserInputEventArgs
                    (MaxUserInputEventArgs.MaxEventTypes.Deploy, null);
        }


        /// <summary>Actions prior to notifying max to open project</summary>
        public void BeforeOpenProject(string path)
        {
            string projectName = path.IndexOf(Const.dot) >= 0?
                Path.GetFileNameWithoutExtension(path): path;

            // Assign view a temporary name which will be replaced shortly
            view.Set(ViewTypes.App, projectName);  

            this.explorer.AddProjectPlaceholder(projectName); 

            // Using the per-project information found in the project file,
            // load the packages specifically relating to this opening project
            LoadProjectSpecificPackages(path);
        }


        /// <summary>Actions prior to explicit close of a project</summary>
        public MaxUserInputEventArgs BeforeCloseProject()
        {  
            // Menu File/CloseProject selected. We force an IDE state save    
            // to preserve toolbox and IDE state event if project not dirty.   
            this.RequestPersistIDE(true);
       
            int forceSave = MaxMain.projectDirty? 1: 0;
            MaxUserInputEventArgs args = new MaxUserInputEventArgs  
                (MaxUserInputEventArgs.MaxEventTypes.CloseProject, forceSave);

            return args;
        }


        /// <summary>Loads up the project-specific native types and action/event packages</summary>
        public static void LoadProjectSpecificPackages(string projectPath)
        {
            MaxToolboxHelper.Instance.ConfigureProjectTools(projectPath);

            MaxNativeTypes.Instance.LoadProjectTypes(projectPath);
        }


        /// <summary>Actions prior to explicit save of a project</summary>
        public MaxUserInputEventArgs BeforeSaveProject()
        {   
            this.RequestPersistIDE(true);
            return new MaxUserInputEventArgs  
                (MaxUserInputEventArgs.MaxEventTypes.SaveProject, MaxMain.projectPath);
        }


        /// <summary>Fire event requesting data to populate toolbox</summary>
        protected void SolicitToolboxContent()
        {   
            // NOTE we do NOT need to send the XML downstream, since want manager
            // to send back all tool groups, from which we will construct the
            // desired configuration by parsing the XML. 
  
            this.RaiseToolboxDataRequest(this,     
                new MaxToolboxEventArgs(MaxToolboxEventArgs.MaxEventTypes.ToolGroupRequest, null));
        }


        /// <summary>Invoked on Window menu command Close All</summary>
        public void CloseAllTabs()
        {                                                   
            if  (Explorer == null || this.CurrentViewName == null) return;

            MaxTreeNode viewnode = Explorer.FindByViewName(view.ViewType, view.Name); 
            if  (viewnode == null) return;

            foreach(MaxTreeNode node in viewnode.Nodes)       
            {                                        
                MaxExplorerWindow.CanvasInfo info = node.Tag as MaxExplorerWindow.CanvasInfo;
                if (info != null) this.SignalTabCloseRequest(info.canvasName);
            } 

            this.overview.ClearOverview();
        }


        /// <summary>Reqeuests change of active tab frame</summary> 
        public void SignalTabChangeRequest(string name)
        {
            tabChangeArgs.TabName = name;
            RaiseTabChangeRequest(this, tabChangeArgs);
        }


        /// <summary>Reqeuests revert to prior tab frame</summary> 
        public void SignalTabToggleRequest()
        {
            RaiseTabChangeRequest(this, tabToggleArgs);
        }


        /// <summary>Requests close of a tab frame</summary>
        public void SignalTabCloseRequest(string name)
        {
            tabCloseArgs.TabName = name;
            RaiseTabCloseRequest(this, tabCloseArgs);
        }


        /// <summary>Fires properties changed event inbound</summary>
        public void SignalPropertiesChanged
        ( Framework.Satellite.Property.MaxProperty[] changedProperties)  
        {            
            propertiesChangedArgs.PropertyDescriptors = changedProperties;                                     
            RaisePropertiesEvent(this, propertiesChangedArgs);   
        }


        /// <summary>Fires properties show request inbound</summary>
        public void SignalPropertiesShowRequest(string entityName, int requestType)
        {
            MaxUserInputEventArgs args = new MaxUserInputEventArgs
           (MaxUserInputEventArgs.MaxEventTypes.ShowProperties, entityName, requestType);

            RaiseUserInput(this, args);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Event post-processing 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Response received to Max start request</summary>
        public void OnMaxStarted(MaxFrameworkEventArgs e)
        {
            if  (e.Result == MaxFrameworkEventArgs.MaxResults.Error)
            {                                     // No packages
                MessageBox.Show(Const.NoPkgsMsg, Const.dialogTitle, 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                             
                MaxToolboxHelper.Instance.VerifyRequiredToolsPresent();
                toolbox.Refresh();                // 20060914
            }
                                   
            else this.SolicitToolboxContent();    // Request toolbox data  
        }


        /// <summary>Invoked after new project round trip</summary>
        public void OnNewProjectCreated(string path)
        {  
            MaxToolboxHelper.Instance.ConfigureMaxDefaultToolbox(toolbox);

            this.SetProjectInfo(path);
            this.explorer.AddProjectPlaceholder(MaxMain.projectFilename); 
 
            // We need to establish an initial IDE file in order that project save
            // requests will not result in IDE flicker. Flicker will be evident
            // once here, but not on saves. We may wish to create a default IDE
            // layout file and use that in place of serializing the layout here.
            dockmgr.SaveConfigToFile                
                (Utl.GetIdeFilePath(MaxMain.ProjectPath), Config.MaxEncoding);
        }


        /// <summary>Invoked after open project round trip</summary>
        public void OnProjectOpened(MaxProjectEventArgs e)
        {  
            if  (e.Result != MaxProjectEventArgs.MaxResults.OK) 
            {
                this.MarkProjectClosed(); 
                return;
            }
           
            this.SetProjectInfo(e.ProjectPath);
                                                                
            // An event for the current view may have been fired prior to this event.
            // If so, view.Name will contain the view name, and we will want to
            // revise our title bar accordingly 
            if  (view.Name != null) view.Set(view.Name);

            // Configure docking windows as saved with the project
            this.OnLoadMaxDockingConfiguration(); 
            this.ResumeLayout(); 

            // Unless to be displayed after a command line or last save open, 
            // show user missing packages noted during project load, if any.
            if (!isInitialOpen) Utl.MissingPackagesProc(Utl.MissingPackageActions.Show);       
        }


        /// <summary>Invoked after close project round trip</summary>
        public void OnProjectClosed(MaxProjectEventArgs e)
        {  
            if  (e.Result != MaxProjectEventArgs.MaxResults.OK) return;

            // If the project was not dirty, save IDE state in .max file regardless.
     
            switch(asyncState)
            {
                case AsyncStates.ClosingPriorOpen:
                case AsyncStates.ClosingPriorNew:
                    // The visual transition is too ugly when we serialize a Crownwood
                    // IDE after project close prior to an open. We'll simply stipulate 
                    // that docking state is not saved at this point. It will of course 
                    // be saved when the process is closed.  
                    break;
                default:
                    if (!wasProjectFileSaved) thisx.SaveProjectFile(false, false);
                    break;
            } 

            // Clear out project-specific using statements and other properties
            thisx.ClearProjectSpecificProperties();

            // After saving IDE, remove project specific information from the toolbox.
            thisx.UnloadProjectSpecificPackages();
            wasProjectFileSaved = false;  
            this.MarkProjectClosed();   

            // If we are waiting on closing an existing project, prior to
            // continuing with the pending command, such as opening another  
            // project, then continue where we left off  

            switch(asyncState)
            {
                case AsyncStates.ClosingPriorOpen:
                case AsyncStates.ClosingPriorNew:
                    this.ContinueNewOpenAfterSave();
                    break;
            } 
        }
                        

        /// <summary>Invoked after open script round trip</summary>
        public void OnViewOpened(MaxProjectEventArgs e)
        {  
            if  (e.Result == MaxProjectEventArgs.MaxResults.Error) return;

            this.OnViewDirty(false);

            Const.MakeMaxTitle(MaxMain.ProjectName, e.ScriptName);  
        }


        /// <summary>Invoked after close view round trip, e.g. close script</summary>
        public void OnViewClosed(MaxProjectEventArgs e)     
        {  
            if  (e.Result == MaxProjectEventArgs.MaxResults.Error) return;   

            // If current app was determined by max to be abandoned (never saved)
            // when closed, max has so indicated, and we will remove the app from
            // explorer, and delete the app file from project folder.
            // Note: we have e.ScriptName, e.ScriptPath if we need it

            if (e.Result == MaxProjectEventArgs.MaxResults.OkRemove)       
                view.Delete();
      
            this.overview.ClearOverview();
            view.Clear();  

            // If we are waiting on closing an existing app, prior to continuing 
            // with the pending command, such as opening another app, 
            // then continue where we left off  

            switch(asyncState)
            {
               case AsyncStates.ClosingPriorNewApp:
                    this.ContinueAddApp(false); 
                    break;
               case AsyncStates.ClosingPriorAddExistingApp:
                    this.ContinueAddApp(true);
                    break;
               case AsyncStates.ClosingPriorOpenApp:
                    this.ContinueOpenApp();
                    break;
               case AsyncStates.ClosingPriorAddNewInstaller:
                    this.ContinueAddInstaller(false);
                    break;
               case AsyncStates.ClosingPriorAddExistingInstaller:
                    this.ContinueAddInstaller(true);
                    break;
               case AsyncStates.ClosingPriorOpenInstaller:
                    this.ContinueOpenInstaller();
                    break;
                case AsyncStates.ClosingPriorAddNewLocales:
                    this.ContinueAddLocales(false);
                    break;
                case AsyncStates.ClosingPriorAddExistingLocales:
                    this.ContinueAddLocales(true);
                    break;
                case AsyncStates.ClosingPriorOpenLocales:
                    this.ContinueOpenLocales();
                    break;
               case AsyncStates.ClosingPriorAddNewDatabase:
                    this.ContinueAddDatabase(false);
                    break;
               case AsyncStates.ClosingPriorAddExistingDatabase:
                    this.ContinueAddDatabase(true);
                    break;
               case AsyncStates.ClosingPriorOpenDatabase:
                    this.ContinueOpenDatabase();
                    break;
               case AsyncStates.ClosingPriorOpenVrResx:
                    this.ContinueOpenVrResx();
                    break;
            }
        }
    

        /// <summary>Invoked after project Save/Save As round trip</summary>
        public void OnProjectSaved(MaxProjectEventArgs args)      
        {
            // Sequence of events: a) user selected Save As from menu; b) menu handler
            // fired Save As user input event; c) Event layer invoked MaxManager.SaveAs;
            // d) MaxManager invoked Project.SaveAs; e) MaxProject saved the app layout
            // to its .app file if necessary, and saved the toolbox layout to .tbx file
            // if necessary; f) Project fired a project activity event; g) Event layer
            // invoked this method, where we write the project file, including in it 
            // the .tbx layout, and references to the .app files.
      
            if  (args.Result == MaxProjectEventArgs.MaxResults.Error) return;
       
            thisx.SaveProjectFile(true, true); // Incremental .max file save

            Utl.WaitCursor(false);
            this.OnProjectDirty(false);

            switch(asyncState)
            {
               case AsyncStates.SavingPriorBuild:    
                    this.ContinueBuild();  
                    break;
               case AsyncStates.SavingPriorShutdown: 
                    this.ContinueShutdown(false);
                    break;
               case AsyncStates.SavingPriorNewApp:
                    this.ContinueAddApp(false);
                    break;
               case AsyncStates.SavingPriorOpenApp:
                    this.ContinueOpenApp();
                    break;
               case AsyncStates.SavingPriorAddExistingApp:
                    this.ContinueAddApp(true);
                    break;
               case AsyncStates.SavingPriorAddNewInstaller:
                    this.ContinueAddInstaller(false);
                    break;
               case AsyncStates.SavingPriorAddExistingInstaller:
                    this.ContinueAddInstaller(true);
                    break;
               case AsyncStates.SavingPriorOpenInstaller:
                    this.ContinueOpenInstaller();
                    break;
               case AsyncStates.SavingPriorAddNewDatabase:
                    this.ContinueAddDatabase(false);
                    break;
               case AsyncStates.SavingPriorAddExistingDatabase:
                    this.ContinueAddDatabase(true);
                    break;
               case AsyncStates.SavingPriorOpenDatabase:
                    this.ContinueOpenDatabase();
                    break;
               case AsyncStates.SavingPriorOpenVrResx:
                    this.ContinueOpenVrResx();
                    break;
            }    
        }


        /// <summary>Invoked to add a script to project (explorer) during app open</summary>
        public void OnAddScript(MaxProjectEventArgs e)
        {
            if  (e.Active) view.Set(ViewTypes.App, e.ScriptName);   
            this.explorer.AddApp(e.ScriptName, e.Active);
        }


        /// <summary>Invoked after an OpenScript round trip</summary>
        public void OnOpenScript(MaxProjectEventArgs e)
        {
            if (e.Active) view.Set(ViewTypes.App, e.ScriptName);
            this.OnViewOpened(e);
        }


        /// <summary>Invoked after Remove Script round trip</summary>
        public void OnRemoveScript(MaxProjectEventArgs e)
        {
            if (e.ScriptName == view.CurrentAppName)
                view.Clear();

            this.explorer.RemoveApp(e.ScriptName);
            this.OnProjectDirty(true);
        }


        /// <summary>Invoked to add an installer script to project (explorer)</summary>
        public void OnAddInstaller(MaxProjectEventArgs e)
        {   
            if (e.Active) view.Set(ViewTypes.Installer, e.ScriptName);
            this.explorer.AddInstaller(null); // Show "Installer" only
        }


        /// <summary>Invoked to register installer as current view</summary>
        public void OnOpenInstaller(MaxProjectEventArgs e)
        {   
            view.Set(ViewTypes.Installer, e.ScriptName);
            this.OnViewOpened(e);
        }


        /// <summary>Invoked to remove installer</summary>
        public void OnRemoveInstaller()
        {
            if (view.ViewType == ViewTypes.Installer) View.Clear();

            this.explorer.RemoveInstaller();
        }


        /// <summary>Invoked to add an locales script to project (explorer)</summary>
        public void OnAddLocales(MaxProjectEventArgs e)
        {
            if (e.Active) view.Set(ViewTypes.Locales, e.ScriptName);
            this.explorer.AddLocales(null); // Show "Locales" only
        }


        /// <summary>Invoked to register locales as current view</summary>
        public void OnOpenLocales(MaxProjectEventArgs e)
        {
            view.Set(ViewTypes.Locales, e.ScriptName);
            this.OnViewOpened(e);
        }


        /// <summary>Invoked to remove locales</summary>
        public void OnRemoveLocales()
        {
            if (view.ViewType == ViewTypes.Locales) View.Clear();
            this.explorer.RemoveLocales();
        }


        /// <summary>Invoked to add a database script to project (explorer)</summary>
        public void OnAddDatabase(MaxProjectEventArgs e)
        {      
            if (e.Active) view.Set(ViewTypes.Database, e.ScriptPath);  
            this.explorer.AddDatabase(e.ScriptPath);
        }


        /// <summary>Invoked to register database as current view</summary>
        public void OnOpenDatabase(MaxProjectEventArgs e)
        {   
            view.Set(ViewTypes.Database, e.ScriptName);
            this.OnViewOpened(e);
        }


        /// <summary>Invoked after Remove Database round trip</summary>
        public void OnRemoveDatabase(MaxProjectEventArgs e)
        {
            if (view.CurrentAppName == e.ScriptName && view.ViewType == ViewTypes.Database) 
                View.Clear();

            this.explorer.RemoveDatabase(e.ScriptName);
        }


        /// <summary>Invoked to add a voice rec resource to project (explorer)</summary>
        public void OnAddVoiceRecResource(MaxProjectEventArgs e)
        {      
            if (e.Active) view.Set(ViewTypes.VrResx, e.ScriptPath);  
            this.explorer.AddVoiceRecResource(e);
        }


        /// <summary>Invoked to register voice rec resource as current view</summary>
        public void OnOpenVoiceRecResource(MaxProjectEventArgs e)
        {   
            view.Set(ViewTypes.VrResx, e.ScriptPath);
            this.OnViewOpened(e);
        }


        /// <summary>Invoked after Remove Voice Rec Resource round trip</summary>
        public void OnRemoveVoiceRecResource(MaxProjectEventArgs e)
        {
            if (view.CurrentAppName == e.ScriptName && view.ViewType == ViewTypes.VrResx) 
                View.Clear();

            this.explorer.RemoveVoiceRecResource(e.ScriptName);
        }


        /// <summary>Implicit close of current project on new or open</summary>
        /// <remarks>When New or Open is selected while a project is currently
        /// open, we fire an event to close the current project. Once the async
        /// result is received elsewhere, the New or Open request will continue
        /// at ContinueNewOpenAfterSave() depending upon whether the save was 
        /// confirmed and was successful</remarks>
        public MaxUserInputEventArgs CloseImplicit(string nextName, string nextPath, bool isNew)
        { 
            asyncState = isNew? AsyncStates.ClosingPriorNew: AsyncStates.ClosingPriorOpen;

            pending.ProjectName = nextName;
            pending.ProjectPath = nextPath;

            return new MaxUserInputEventArgs(MaxUserInputEventArgs.MaxEventTypes.CloseProject, null);
        }


        /// <summary>Implicit close of current project on build request</summary>
        private MaxUserInputEventArgs CloseImplicit()
        {
            asyncState = AsyncStates.SavingPriorBuild; 

            return new MaxUserInputEventArgs
                (MaxUserInputEventArgs.MaxEventTypes.SaveProject, ProjectPath);
        }


        /// <summary>Set project state to no project open</summary>
        private void MarkProjectClosed()
        {
            thisx.CleanupProjectFolder();
            this.SetProjectInfo(null);
            this.overview.CloseOverview();
            this.explorer.Clear();
        }


        /// <summary>Continue with new or open after implicit save or close of project</summary>
        /// <remarks>When New or Open was selected while a project was currently
        /// open, we fired an event to close the current project. When the async
        /// result was received, and the save was confirmed and sucessful,
        /// the New or Open request continues here
        private void ContinueNewOpenAfterSave()
        {
            MaxUserInputEventArgs args = null;

            switch(asyncState)
            {
               case AsyncStates.ClosingPriorOpen:            
             
                    this.BeforeOpenProject(pending.ProjectPath);  
 
                    args = new MaxUserInputEventArgs 
                        (MaxUserInputEventArgs.MaxEventTypes.OpenProject, 
                            pending.ProjectPath);
                    break;

               case AsyncStates.ClosingPriorNew:

                    args = new MaxUserInputEventArgs
                        (MaxUserInputEventArgs.MaxEventTypes.NewProject, 
                            pending.ProjectName, pending.ProjectPath);
                    break;

            }     

            if  (args != null) RaiseUserInput(this, args);    

            asyncState = AsyncStates.None;
            pending.ProjectName = pending.ProjectPath = null;
        }


        /// <summary>Continue with new app request after save or close</summary>
        /// <remarks>When New Script or Open Script was selected while an app 
        /// was currently open, we fired an event to save or close the project,
        /// depending on dialog choice made by user. When the async result was 
        /// received, the New or Open Script request continues here
        private void ContinueAddApp(bool existing)
        {
            asyncState = AsyncStates.None;

            MaxUserInputEventArgs.MaxEventTypes eventType = existing?
                MaxUserInputEventArgs.MaxEventTypes.AddExistingScript:
                MaxUserInputEventArgs.MaxEventTypes.AddNewScript;

            MaxUserInputEventArgs args = new MaxUserInputEventArgs
                (eventType,prompted.AppName, prompted.Trigger);

            RaiseUserInput(this, args); 
        }


        /// <summary>Continue with open app request after save or close</summary>
        /// <remarks>See comments at ContinueNewApp</remarks>
        public void ContinueOpenApp()
        {
            asyncState = AsyncStates.None;

            view.Set(ViewTypes.App, prompted.AppName);  

            MaxUserInputEventArgs args = new MaxUserInputEventArgs
                (MaxUserInputEventArgs.MaxEventTypes.OpenScript, prompted.AppName, prompted.CanvasName);

            RaiseUserInput(this, args); 
        }


        /// <summary>Continue with Add Installer request after save or close</summary>
        private void ContinueAddInstaller(bool existing)
        {
            asyncState = AsyncStates.None;

            MaxUserInputEventArgs.MaxEventTypes eventType = existing?
                MaxUserInputEventArgs.MaxEventTypes.AddExistingInstaller:
                MaxUserInputEventArgs.MaxEventTypes.AddNewInstaller;

            MaxUserInputEventArgs args = new MaxUserInputEventArgs(eventType, prompted.InstallerPath);

            RaiseUserInput(this, args); 
        }


        /// <summary>Continue with open installer request after save or close</summary>
        private void ContinueOpenInstaller()
        {
            asyncState = AsyncStates.None;

            MaxUserInputEventArgs args = new MaxUserInputEventArgs
                (MaxUserInputEventArgs.MaxEventTypes.OpenInstaller, prompted.InstallerName);

            RaiseUserInput(this, args); 
        }


        /// <summary>Continue with Add Locales request after save or close</summary>
        private void ContinueAddLocales(bool existing)
        {
            asyncState = AsyncStates.None;

            MaxUserInputEventArgs.MaxEventTypes eventType = existing ?
                MaxUserInputEventArgs.MaxEventTypes.AddExistingLocales :
                MaxUserInputEventArgs.MaxEventTypes.AddNewLocales;

            MaxUserInputEventArgs args = new MaxUserInputEventArgs(eventType, prompted.LocalesPath);

            RaiseUserInput(this, args);
        }


        /// <summary>Continue with open installer request after save or close</summary>
        private void ContinueOpenLocales()
        {
            asyncState = AsyncStates.None;

            MaxUserInputEventArgs args = new MaxUserInputEventArgs
                (MaxUserInputEventArgs.MaxEventTypes.OpenLocales, prompted.LocalesPath);

            RaiseUserInput(this, args);
        }


        /// <summary>Continue with Add Database request after save or close</summary>
        private void ContinueAddDatabase(bool existing)
        {
            asyncState = AsyncStates.None;

            MaxUserInputEventArgs.MaxEventTypes eventType = existing?
                MaxUserInputEventArgs.MaxEventTypes.AddExistingDatabase:
                MaxUserInputEventArgs.MaxEventTypes.AddNewDatabase;

            MaxUserInputEventArgs args = new MaxUserInputEventArgs(eventType, existing ? prompted.DatabasePath : prompted.DatabaseName);

            RaiseUserInput(this, args); 
        }


        /// <summary>Continue with open database request after save or close</summary>
        private void ContinueOpenDatabase()
        {
            asyncState = AsyncStates.None;

            MaxUserInputEventArgs args = new MaxUserInputEventArgs
                (MaxUserInputEventArgs.MaxEventTypes.OpenDatabase, prompted.DatabaseName);

            RaiseUserInput(this, args); 
        }


        /// <summary>Continue with open voice resx request after save or close</summary>
        private void ContinueOpenVrResx()
        {
            asyncState = AsyncStates.None;

            MaxUserInputEventArgs args = new MaxUserInputEventArgs
                (MaxUserInputEventArgs.MaxEventTypes.OpenVrResx, prompted.VrResxName);

            RaiseUserInput(this, args); 
        }


        /// <summary>Continue with build after implicit save of project</summary>
        /// <remarks>See comments at ContinueNewOpenAfterSave</remarks>
        private void ContinueBuild()
        {
            asyncState = AsyncStates.None;

            OnBuildProject();
        }
  

        /// <summary>Build the project</summary>
        public static void OnBuildProject()
        {
            Utl.WaitCursor(true);
            string buildmsg = null;
            int errorcount, warningcount;
            MessageWriter.WriteLine(Const.newline);

            OutboundHandlers.OutputWindowProxy(new object(), 
                new MaxOutputWindowEventArgs(MaxOutputWindowEventArgs.MaxEventTypes.Clear));

            try  
            { 
                MaxMainUtil.BuildProject(out warningcount, out errorcount);

                buildmsg = Const.GetBuildCompleteMessage(errorcount, warningcount);

                MaxMain.built = (errorcount == 0);
            }
            catch(Exception x) 
            { 
                Utl.WriteExceptionToFile(Utl.GetObjDirectoryPath(ProjectPath, Const.BuildErrorsFilename), x);

                MessageWriter.WriteLine(Const.BuildErrorOccurred);
                MessageWriter.WriteLine(String.Empty);
                MessageWriter.WriteLine(x.Message);
          
                if (x is PackagerException)
                {
                    PackagerException pe = x as PackagerException;

                    if(pe.ErrorMessages != null)
                        foreach(string msg in pe.ErrorMessages)
                            MessageWriter.WriteLine(Const.PackagerMessage + msg);
                }

                MessageWriter.WriteLine(Const.MoreErrorInfo);
      
                buildmsg = Const.GetBuildCompleteMessage(-1, -1);
            }

            Utl.WaitCursor(false);
            MessageWriter.WriteLine(String.Empty);
            MessageWriter.WriteLine(buildmsg);
            MessageWriter.WriteLine(String.Empty);
            MessageWriter.WriteStatusBarText(buildmsg);
        }


        /// <summary>Deploy the project</summary>
        public void OnDeployProject()
        {
            if(thisx.DeployProject()) // Spawns an async delegate.
            {
                MessageWriter.WriteLine(Const.DeployInitiatedMsg);
                MessageWriter.WriteStatusBarText(Const.DeployInitiatedMsg);
            }
        }


        /// <summary>Notification from max that full save is in progress</summary>
        public void OnMaxSaving()
        {
            MaxMain.WriteStatusBarText(Const.projectSavingStatusMsg, 30000); 
            Utl.WaitCursor(true);
        }


        /// <summary>Continue with pending shutdown after max round trip</summary>
        public void OnMaxShutdown(MaxFrameworkEventArgs e)
        {
            if (asyncState == AsyncStates.SavingPriorShutdown)       
                this.ContinueShutdown(false);
        }
      

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Event notifications 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Notification from Max of a change in project state</summary>
        public void OnProjectActivity(MaxProjectEventArgs e)
        {
            switch(e.MaxEventType)
            {
               case MaxProjectEventArgs.MaxEventTypes.New:
                    this.OnNewProjectCreated(e.ProjectPath);
                    break;

               case MaxProjectEventArgs.MaxEventTypes.Open:
                    this.OnProjectOpened(e);
                    break;

               case MaxProjectEventArgs.MaxEventTypes.Close:
                    this.OnProjectClosed(e);
                    break;

               case MaxProjectEventArgs.MaxEventTypes.Save:
                    this.OnProjectSaved(e);
                    break;

               case MaxProjectEventArgs.MaxEventTypes.SaveAs:
                    this.OnProjectSaved(e);
                    break;

               case MaxProjectEventArgs.MaxEventTypes.Dirty:
                    this.OnProjectDirty(true);
                    break;

               case MaxProjectEventArgs.MaxEventTypes.NotDirty:
                    this.OnProjectDirty(false);
                    // We can lose this event and use the save event
                    break; 

               case MaxProjectEventArgs.MaxEventTypes.AppDirty:
                    this.OnViewDirty(true);
                    break;

               case MaxProjectEventArgs.MaxEventTypes.AppNotDirty:
                    this.OnViewDirty(false);            
                    // We can lose this event and use the save event
                    break; 

               case MaxProjectEventArgs.MaxEventTypes.Properties:
                    ShowPropertiesWindow(false);
                    break;

               case MaxProjectEventArgs.MaxEventTypes.AddScript:             
                    this.OnAddScript(e);
                    break;
 
               case MaxProjectEventArgs.MaxEventTypes.OpenScript:
                    this.OnOpenScript(e);
                    break; 

               case MaxProjectEventArgs.MaxEventTypes.CloseScript:
                    this.OnViewClosed(e);
                    break; 

               case MaxProjectEventArgs.MaxEventTypes.RemoveScript:
                    this.OnRemoveScript(e); 
                    break; 

               case MaxProjectEventArgs.MaxEventTypes.RenameScript:
                    view.Set(ViewTypes.App, e.NewName);
                    break; 

               case MaxProjectEventArgs.MaxEventTypes.AddInstaller:
                    this.OnAddInstaller(e);
                    break;

               case MaxProjectEventArgs.MaxEventTypes.OpenInstaller:
                    this.OnOpenInstaller(e);
                    break;

               case MaxProjectEventArgs.MaxEventTypes.CloseInstaller:
                    this.OnViewClosed(e);    
                    break;

               case MaxProjectEventArgs.MaxEventTypes.RemoveInstaller:
                    this.OnRemoveInstaller();
                    break;

                case MaxProjectEventArgs.MaxEventTypes.AddLocaleEd:
                    this.OnAddLocales(e);
                    break;

                case MaxProjectEventArgs.MaxEventTypes.OpenLocaleEd:
                    this.OnOpenLocales(e);
                    break;

                case MaxProjectEventArgs.MaxEventTypes.CloseLocaleEd:
                    this.OnViewClosed(e);
                    break;

                case MaxProjectEventArgs.MaxEventTypes.RemoveLocaleEd:
                    this.OnRemoveLocales();
                    break;

               case MaxProjectEventArgs.MaxEventTypes.AddDatabase:
                    this.OnAddDatabase(e);
                    break;

               case MaxProjectEventArgs.MaxEventTypes.OpenDatabase:
                    this.OnOpenDatabase(e);
                    break;

               case MaxProjectEventArgs.MaxEventTypes.RemoveDatabase:
                    this.OnRemoveDatabase(e);
                    break;

               case MaxProjectEventArgs.MaxEventTypes.CloseDatabase:
                    this.OnViewClosed(e);  
                    break;

               case MaxProjectEventArgs.MaxEventTypes.AddMedia:
                    this.explorer.AddMedia(e);
                    break;

               case MaxProjectEventArgs.MaxEventTypes.AddVrResx:
                    this.explorer.AddVoiceRecResource(e);
                    break;

               case MaxProjectEventArgs.MaxEventTypes.CloseVrResx:
                    this.OnViewClosed(e); 
                    break;

               case MaxProjectEventArgs.MaxEventTypes.AddReference:
                    this.explorer.AddReference(e);
                    break;
            }
        } 


        /// <summary>Invoked on canvas activity event</summary> 
        public void OnCanvasActivity(MaxCanvasEventArgs e)
        {
            Explorer.OnCanvasActivity(view.ViewType, e);
        }


        /// <summary>Invoked on canvas node activity event</summary> 
        public void OnCanvasNodeActivity(MaxNodeEventArgs e)
        {
            Explorer.OnCanvasNodeActivity(e);
        }


        /// <summary>Notification from Max to reflect tab close, add, remove, or navigation</summary>
        public void OnTabActivity(MaxCanvasTabEventArgs e)
        {
            if (Explorer == null) return;
            Crownwood.Magic.Controls.TabPage tabpage = e.TabPage;;
            MaxTabContent canvas = tabpage == null?  null: tabpage.Control as MaxTabContent;
            if  (canvas == null) return;
      
            switch(e.MaxEventType)
            {
               case MaxCanvasTabEventArgs.MaxEventTypes.Add:
                    break;                                   
                                                      
               case MaxCanvasTabEventArgs.MaxEventTypes.GoTo:
                    Explorer.SetCanvasActive(view.ViewType,  view.Name, e.TabName, true); 
                    Explorer.SelectCanvasNode(view.ViewType, view.Name, canvas.CanvasName); 
                    this.currentTab = e.TabName;
                    if  (e.TabName == view.Name) this.overview.ClearOverview();
                    break;

               case MaxCanvasTabEventArgs.MaxEventTypes.Close:   
                    // Mark canvas as not having a tab (for Window menu purposes)                 
                    Explorer.SetCanvasActive(view.ViewType, view.Name, e.TabName, false);  
                    break;

               case MaxCanvasTabEventArgs.MaxEventTypes.Clear:
                    Explorer.Clear();
                    this.currentTab = null;
                    break;

               case MaxCanvasTabEventArgs.MaxEventTypes.Rename:   
                    Explorer.RenameCanvas(view.ViewType, view.Name, e.TabName, e.OldName);
                    break;
            }
        }


        /// <summary>Invoked after explorer activity round trip</summary>
        public void OnExplorerActivity(MaxProjectEventArgs e)
        {
        }


        /// <summary>Invoked on receipt of project dirty event from max</summary>
        public void OnProjectDirty(bool isDirty)
        {
            MaxMain.projectDirty = isDirty; 
            if  (!isDirty) view.Dirty = false;
            this.Text  = isDirty? Const.AppTitleDirty: Const.AppTitleNormal;  
            MaxMain.built = false;
        }


        /// <summary>Invoked on receipt of view dirty event from max</summary>
        public void OnViewDirty(bool isDirty)
        {      
            if (isDirty) OnProjectDirty(isDirty); 
            view.Dirty = isDirty; 
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Docking satellite window handling 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Invoked on right click of any docking window title bar</summary>
        protected void OnDockingContextMenu(PopupMenu pm, CancelEventArgs cea)
        {       
            mainMenu.OnDockingContextMenu(pm);
        }


        /// <summary>Invoked when a docking window close box is clicked</summary>
        protected void OnContentHiding(Content c, CancelEventArgs cea)
        {
            // To prevent a docking window from closing we would set cea.Cancel true 
        }


        /// <summary>Invoked when a docking window is being hidden</summary>
        protected void OnContentHidden(Content c, EventArgs cea)
        {
            if  (!(c.Control is MaxSatelliteWindow)) return; 
            MaxSatelliteWindow closingWindow = c.Control as MaxSatelliteWindow;
            MenuCommand menu = closingWindow.ViewMenuItem;
            if  (menu != null) menu.Checked = false;
        }


        /// <summary>Invoked when a docking window is unhidden</summary>
        protected void OnContentShown(Content c, EventArgs cea)
        {
            if  (!(c.Control is MaxSatelliteWindow)) return;
            MaxSatelliteWindow openingWindow = c.Control as MaxSatelliteWindow;
            MenuCommand menu = openingWindow.ViewMenuItem;
            if  (menu == null) return;

            menu.Checked = true;
            MaxMenu.menuViewCanvas.Checked = false;
        }


        /// <summary>Ensure property window visible</summary>
        public void ShowPropertiesWindow(bool focus)
        {
            dockmgr.ShowContent(propertyWindow);
            dockmgr.BringAutoHideIntoView(propertyWindow);  
            //propertyWindow.BringToFront();
            if (focus) MaxPropertyWindow.Instance.Grid.Focus();
        }


        /// <summary>Ensure output window visible</summary>
        public void ShowOutputWindow()
        {
            dockmgr.ShowContent(outputWindow);
            dockmgr.BringAutoHideIntoView(outputWindow);  
            try
            {
                outputWindow.BringToFront();
            }
            catch { }
        }


        /// <summary>Ensure explorer window visible</summary>
        public void ShowExplorerWindow(bool focus)
        {
            dockmgr.ShowContent(explorerWindow);
            dockmgr.BringAutoHideIntoView(explorerWindow);  
            explorerWindow.BringToFront();
            if (focus) explorer.Tree.Focus();
        }


        /// <summary>Ensure toolbox window visible</summary>
        public void ShowToolboxWindow(bool focus)
        { 
            dockmgr.ShowContent(toolboxWindow);
            dockmgr.BringAutoHideIntoView(toolboxWindow);  
            toolboxWindow.BringToFront();
            if (focus) toolbox.Focus();
        }


        /// <summary>Ensure overview window visible</summary>
        public void ShowOverviewWindow()
        { 
            dockmgr.ShowContent(overviewWindow);
            dockmgr.BringAutoHideIntoView(overviewWindow);  
            overviewWindow.BringToFront();
        }
  

        /// <summary>Arrange docking windows per current saved configuration</summary>
        public bool OnLoadMaxDockingConfiguration()
        { 
            string configFilePath = thisx.GetSavedDockingConfiguration();
            if (configFilePath == null) return false;

            dockmgr.LoadConfigFromFile(configFilePath);
            this.ResumeLayout();            // Ensure IDE painted now
            return true;
        }


        /// <summary>Set state common to all satellite windows</summary>
        public static void SetDockingWindowState(Content c)
        {
            c.CaptionBar  = true;
            c.CloseButton = true;
        }


        /// <summary>Fire inbound event if IDE should be saved now</summary>
        public void RequestPersistIDE(bool force)
        {
            if (MaxMain.ideDirty || force)   
                RaiseFrameworkEvent(this, new MaxFrameworkEventArgs
                    (MaxFrameworkEventArgs.MaxEventTypes.IdeModified));

            MaxMain.ideDirty = MaxMain.wasProjectFileSaved = false;
        }


        /// <summary>Toggle visibility state of docking window</summary>
        public void MaxToggleWindow(Content c)
        {
            if  (c.Visible)
                 dockmgr.HideContent(c, true, false);
            else dockmgr.ShowContent(c);
        }

           
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Process shutdown handling 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public enum SaveActions { None, Cancel, Save, Nosave, Exit }


        /// <summary>Invoked prior to close of main window</summary>
        /// <remarks>When the main window is about to close, and we have not yet
        /// done so, we cancel the close of the main window, and fire an event in
        /// to max to save files if necessary and wrap up. Then, the next time we
        /// pass through here, we permit the main window to close.</remarks>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (!isIdeStarted) { base.OnClosing(e); return; }
            MaxMain.SaveActions action = SaveActions.None;

            if  (asyncState == AsyncStates.SavingPriorShutdown)    
                 action = SaveActions.Exit;        
            else
            if (MaxMain.projectDirty)
            {    
                switch(Utl.PromptSaveChangesTo(ProjectName, Const.maxShutdownDlgTitle))
                {
                    case DialogResult.Cancel: action = SaveActions.Cancel; break;
                    case DialogResult.Yes:    action = SaveActions.Save;   break;
                    case DialogResult.No:     action = SaveActions.Nosave; break;
                }
            }
            else action = SaveActions.Nosave;

            switch(action)
            {
               case SaveActions.Save:
                    // User asked to save the project. Before we can do that, we must
                    // cancel the close, fire off an event to max to save the view,
                    // and await the async reply, at which time we will save the 
                    // project file and continue with shutdown.
                    e.Cancel   = true;
                    this.RequestPersistIDE(true); 
                                              // Rrefresh the docking layout which
                    dockmgr.SaveConfigToFile  // gets copied to project file  
                        (Utl.GetIdeFilePath(MaxMain.ProjectPath), Config.MaxEncoding);

                    asyncState = AsyncStates.SavingPriorShutdown;

                    RaiseUserInput(this, new MaxUserInputEventArgs
                        (MaxUserInputEventArgs.MaxEventTypes.SaveProject, ProjectPath, 1));
                    break;

               case SaveActions.Nosave:
                    // User asked to not save the project. We still must cancel the
                    // close, fire off an event to max to wrap up, and await the
                    // async reply, at which time we will continue with shutdown.
                    e.Cancel   = true;
                    asyncState = AsyncStates.SavingPriorShutdown;

                    // We'll save the current IDE configuration regardless  
                    thisx.SaveProjectFile(false, false);  

                    RaiseUserInput(this, new MaxUserInputEventArgs
                        (MaxUserInputEventArgs.MaxEventTypes.Shutdown, null)); 
                    break;

               case SaveActions.Cancel:
                    // User canceled the shutdown request
                    e.Cancel = true;
                    break;

                case SaveActions.Exit:
                    // Second time through this method: we permit the close to complete
                    break;
            }    
      
            base.OnClosing (e);
        }


        /// <summary>Complete pending shutdown after a max save round trip</summary>
        /// <remarks>If we initiated a shutdown of the main window, and the project 
        /// was dirty, we will have canceled the close of the main window, and fired 
        /// an event in to max to close the app file and wrap up. Once the reply was
        /// received from max at OnProjectClosed or OnMaxShutdown, we arrive here,
        /// where we complete the close of the main window and resultant process 
        /// shutdown.</remarks>
        private void ContinueShutdown(bool saveProject)
        {
            if (saveProject) thisx.SaveProjectFile();

            this.Close();
        }


        /// <summary>Invoked prior to process exit</summary>
        private void OnMaxExit(object sender, EventArgs e)
        {
            MaxDebugger.Instance.Force();   // Cancel listener thread if any

            this.UnhookKeyboard(ref MaxMain.hHook);

            Config.RecentFiles.Save();             
                                             
            new MaxWindowPlacement().Save(this);

            thisx.CleanupProjectFolder(); 
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Other methods 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


        /// <summary>Establish or clear path and name of project, and title bar text</summary>
        public void SetProjectInfo(string path)
        {
            MaxMain.projectPath = path;
            if  (path == null){this.currentTab = null; view.Clear();}
            MaxMain.projectFilename = path == null? null: Utl.StripPathFolderPlusExtension(path);
            MaxMain.projectFolder   = path == null? null: Utl.StripPathFilespec(path);
            Const.MakeMaxTitle(MaxMain.projectFilename);

            // On deserialization, a view may have been opened prior to project open
            // round trip. When this is the case, ensure title bar is current.  
            if (!View.IsClear) Const.MakeMaxTitle(MaxMain.projectFilename, view.Name);  
            
            this.OnProjectDirty(false);
        }


        /// <summary>Track last non-maximized state</summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            if (isIdeLoaded && this.WindowState == FormWindowState.Normal) 
                RecentBounds = this.Bounds;

            base.OnSizeChanged (e);
        }


        /// <summary>Install keyboard hook</summary>
        public static void SetKeyboardHook()
        {
            MaxMain.kbHookCallback = new Utl.HookProc(MaxMain.OnSystemKey);

            int id = System.Threading.Thread.CurrentThread.ManagedThreadId;
            MaxMain.hHook = Utl.SetWindowsHookEx(Const.WH_KEYBOARD, kbHookCallback, IntPtr.Zero, id); 
        }


        /// <summary>Lose the keyboard hook</summary>
        private void UnhookKeyboard(ref int hHook)
        {
            if (hHook != 0)       
                Utl.UnhookWindowsEx(hHook);
       
            hHook = 0;       
        }


        /// <summary>Keyboard hook</summary>
        public static int OnSystemKey(int nCode, IntPtr wParam, int lParam)
        {
            // Virtual keycodes:
            // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/w98ddk/hh/w98ddk/keycnt_4fqw.asp
     
            if (nCode < 0)       
                return Utl.CallNextHookEx(hHook, nCode, wParam, lParam);              
       
            Utl.KeyInfo keyData = Utl.ParseKeyProcLParam(lParam);

            return Utl.CallNextHookEx(hHook, nCode, wParam, lParam);       
        }
    

        /// <summary>Actions on status bar text received</summary>
        public static void OnStatusBarText(string text)
        {
            WriteStatusBarText(text, 0);
        }


        /// <summary>Display text on status bar</summary>
        public static void WriteStatusBarText(string text, int intervalMS)
        {  
            statusBarTimer.Stop();
            statusMsg.Text = text;     
            statusBarTimer.Interval = intervalMS < 1?   Const.statusBarMessageDurationMs:
                intervalMS < 128? intervalMS * 1000: intervalMS;
            statusBarTimer.Start();
        }


        /// <summary>Clear status bar on timer expiration</summary>
        private static void OnStatusBarTimeExpired(Object sender, EventArgs args) 
        {
            statusBarTimer.Stop();
            statusMsg.Text = null;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Other properties 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public enum ViewTypes { None, App, Installer, Locales, Database, Media, VrResx }

        public enum AsyncStates 
        {
            None, 
            ClosingPriorOpen,   ClosingPriorNew,    
            SavingPriorBuild,   SavingPriorShutdown, 
            SavingPriorNewApp,  ClosingPriorNewApp, 
            SavingPriorOpenApp, ClosingPriorOpenApp,
            SavingPriorAddExistingApp,       ClosingPriorAddExistingApp,
            SavingPriorAddNewInstaller,      ClosingPriorAddNewInstaller, 
            SavingPriorOpenInstaller,        ClosingPriorOpenInstaller, 
            SavingPriorAddExistingInstaller, ClosingPriorAddExistingInstaller,
            SavingPriorAddNewLocales,        ClosingPriorAddNewLocales,
            SavingPriorOpenLocales,          ClosingPriorOpenLocales,
            SavingPriorAddExistingLocales,   ClosingPriorAddExistingLocales, 
            SavingPriorAddNewDatabase,       ClosingPriorAddNewDatabase, 
            SavingPriorAddExistingDatabase,  ClosingPriorAddExistingDatabase,
            SavingPriorOpenDatabase,         ClosingPriorOpenDatabase,
            SavingPriorOpenVrResx,           ClosingPriorOpenVrResx
        };

        public class Prompted 
        { 
            public string AppName, CanvasName, Trigger, InstallerName, InstallerPath;
            public string LocalesName, LocalesPath;
            public string DatabaseName, DatabasePath, AudioFilePath; 
            public string VrResxName, VrResxFilePath;
            public CultureInfo AudioFileLocale;
        }

        public class MaxMessageWriter
        {
            private MaxOutputWindow maxoutputwindow;
            public  MaxMessageWriter(MaxOutputWindow mopw) { maxoutputwindow = mopw; }

            public void WriteLine(string text)
            {
                if  (maxoutputwindow == null) System.Console.WriteLine(text);
                else maxoutputwindow.WriteLine(text);
            } 

            public void WriteStatusBarText(string text)
            {
                if  (maxoutputwindow != null) MaxMain.OnStatusBarText(text);
            } 
        }

        private static MaxMessageWriter messageWriter;
        public  static MaxMessageWriter MessageWriter { get { return messageWriter; } }

        public  class  Pending  { public string ProjectName, ProjectPath; }
        public  static Prompted prompted = new Prompted();
        public  static Pending  pending  = new Pending();
        private static Utl.HookProc kbHookCallback;
    
        public  bool   TrayShown;
        public  bool InstallerPresent { get { return explorer.InstallerPresent; } }
        public  bool LocalesPresent   { get { return explorer.LocalesPresent; } }
        public static Rectangle RecentBounds;
        private static int maxretcode = 0;
        public  static string[] cmdlinePath = null ;
        public  static bool isInitialOpen = false;
        public  static bool wasProjectFileSaved = false;
        public  static bool autobuild = false;
        private bool   isIdeStarted = false, isIdeLoaded = false;

        #region Windows Form Designer generated code
       
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MaxMain));
            // 
            // MaxMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(952, 606);
            this.DockPadding.Left = 1;
            this.DockPadding.Right = 1;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(250, 250);
            this.Name = "MaxMain";
            this.Text = "Cisco Unified Application Designer";

        }

        protected override void Dispose(bool disposing)
        {
            if  (disposing && AppDeployment.Initialized) 
                AppDeployment.Instance.Dispose();

            base.Dispose(disposing);
        }
        #endregion

        /// <summary>An assembly failed to resolve: let's see if we can find it 
        ///          in the current AppDomain, though not as fully qualified in name</summary>
        /// <remarks>This situation can very well occur when a user attempts 
        ///          to add a reference to the Explorer, and that reference
        ///          needs another assembly not yet added.
        ///          
        ///          This 'solution' checks for assemblies which appear to 
        ///          be the correct assembly, but the framework complained 
        ///          because they were not as fully qualified in name as the 
        ///          framework required at runtime when using some of the 
        ///          System.Reflection methods (like MethodInfo.GetCustomAttributes)
        /// </remarks>
        /// <returns>The already AppDomain-loaded assembly if it could be found</returns>
        private static System.Reflection.Assembly ResolveHelper(object sender, ResolveEventArgs args)
        {
            string assemblyName = args.Name;
            System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(System.Reflection.Assembly assembly in assemblies)
                 if(assembly.FullName.IndexOf(assemblyName) != -1)
                    return assembly;

            return null;
        }
    } // class MaxMain

} // namespace
