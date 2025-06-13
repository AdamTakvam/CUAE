using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;
using Crownwood.Magic.Menus;
using Metreos.Max.GlobalEvents;
using Metreos.Max.Framework.Satellite;
using Metreos.Max.Core;
using Metreos.Max.Debugging;
using Metreos.Max.Framework.Satellite.Explorer;

                                          

namespace Metreos.Max.Framework
{
    /// <summary>Creation of menus and handling of selections</summary>
    public sealed class MaxMenu
    {
        public MaxMenu(MaxMain main)
        {
            this.main  = main;
            this.mainx = main.MainX;
            this.mh = new MaxMenuHandlers(main, this);
        }

        private MaxMain main;
        private MaxMainUtil mainx;
        private MaxMenuHandlers mh;
        public  MaxMenuHandlers Handlers { get { return mh; } }

        public static MenuCommand menuFile;     
        public static MenuCommand menuEdit;      
        public static MenuCommand menuView;      
        public static MenuCommand menuProject;  
        public static MenuCommand menuBuild;
        public static MenuCommand menuDebug; 
        public static MenuCommand menuTools;   
        public static MenuCommand menuWindow;   
        public static MenuCommand menuHelp;
  
        public static MenuCommand menuFileNew; 
        public static MenuCommand menuFileOpen;  
        public static MenuCommand menuFileClose;   
        public static MenuCommand menuFileAddScript;
        public static MenuCommand menuFileAddInstal;
        public static MenuCommand menuFileAddLocales;
                                                           // 0120
        // public static MenuCommand menuFileAddDbScript; // 0120
        // public static MenuCommand menuFileAddMedia;    // 0120
        public static MenuCommand menuFileAddResource;    // 0120 new
                                                          // 0120
        public static MenuCommand menuFileCloseFile;  
        public static MenuCommand menuFileSep1;  
        public static MenuCommand menuFileSave; 
        public static MenuCommand menuFileSaveAs; 
        public static MenuCommand menuFileSaveAll;    
        public static MenuCommand menuFileSep2;
        public static MenuCommand menuFilePrint;
        public static MenuCommand menuFilePreview;
        public static MenuCommand menuFilePageSet;
        public static MenuCommand menuFileSep3;
        public static MenuCommand menuFileRecent;
        public static MenuCommand menuFileSep4;
        public static MenuCommand menuFileExit;

        public static MenuCommand menuFileAddScriptNew;
        public static MenuCommand menuFileAddScriptExist;
        public static MenuCommand menuFileAddInstalNew;
        public static MenuCommand menuFileAddInstalExist;
        public static MenuCommand menuFileAddLocalesNew;
        public static MenuCommand menuFileAddLocalesExist;

        public static MenuCommand menuFileAddResxDatabase; // 0210 new
        public static MenuCommand menuFileAddResxMedia;    // 0210 new
        public static MenuCommand menuFileAddResxVoiceRec; // 0210 new
        public static MenuCommand menuFileAddTtsText;      // 0210 new
        #if(false)
        public static MenuCommand menuFileAddResxTtsText;
        public static MenuCommand menuFileAddResxHtml;
        public static MenuCommand menuFileAddResxImage;
        public static MenuCommand menuFileAddResxText;
        #endif

        public static MenuCommand menuFileAddDbScriptNew;
        public static MenuCommand menuFileAddDbScriptExist;
        public static MenuCommand menuFileAddAudioExist;
        public static MenuCommand menuFileAddTtsTextExist; // 0210 new
        public static MenuCommand menuFileAddTtsTextNew;   // 0210 new

        public static MenuCommand menuEditUndo; 
        public static MenuCommand menuEditRedo; 
        public static MenuCommand menuEditSep1;   
        public static MenuCommand menuEditCut;
        public static MenuCommand menuEditCopy;  
        public static MenuCommand menuEditPaste; 
        public static MenuCommand menuEditDelete; 
        public static MenuCommand menuEditSep2; 
        public static MenuCommand menuEditSelectAll;
        public static MenuCommand menuEditSep3; 
        public static MenuCommand menuEditGoTo; 

        public static MenuCommand menuViewZoom; 
        public static MenuCommand menuViewSep1; 
        public static MenuCommand menuViewGrid;
        public static MenuCommand menuViewSep2; 
        public static MenuCommand menuViewCanvas; 
        public static MenuCommand menuViewTray;
        public static MenuCommand menuViewSep3;
        public static MenuCommand menuViewProperties; 
        public static MenuCommand menuViewToolbox; 
        public static MenuCommand menuViewExplorer; 
        public static MenuCommand menuViewOutput; 
        public static MenuCommand menuViewOverview;
 
        public static MenuCommand menuViewFocusCanvas; 
        public static MenuCommand menuViewFocusTray; 
        public static MenuCommand menuViewFocusProps; 
        public static MenuCommand menuViewFocusExplor;
        public static MenuCommand menuViewFocusTools;
        public static MenuCommand menuViewShowConsole;
        public static MenuCommand menuViewShowBkpts;
        public static MenuCommand menuViewShowStack;
        public static MenuCommand menuViewShowWatch;

        public static MenuCommand menuViewZoomZoomIn;
        public static MenuCommand menuViewZoomZoomOut;
        public static MenuCommand menuViewZoomZoomNormal;

        public static MenuCommand menuProjectSep1;
        public static MenuCommand menuProjectSep2;
        public static MenuCommand menuProjectAddReference;
        public static MenuCommand menuProjectAddWebService;
        public static MenuCommand menuProjectProperties;

        public static MenuCommand menuBuildBuildProject;
        public static MenuCommand menuBuildDeploy;

        public static MenuCommand menuDebugWindows;
        public static MenuCommand menuDebugSep1;
        public static MenuCommand menuDebugStart;
        public static MenuCommand menuDebugStop;
        public static MenuCommand menuDebugBreak;
        public static MenuCommand menuDebugStepInto;
        public static MenuCommand menuDebugStepOver;
        public static MenuCommand menuDebugToggleBkpt;
        public static MenuCommand menuDebugNewBkpt;
        public static MenuCommand menuDebugClearBkpts;
        public static MenuCommand menuDebugDisableBkpts;
        public static MenuCommand menuDebugStartConsole;
        public static MenuCommand menuDebugStopConsole;

        public static MenuCommand menuDebugWindowsCon;
        public static MenuCommand menuDebugWindowsBkpts;
        public static MenuCommand menuDebugWindowsWatch;
        public static MenuCommand menuDebugWindowsCalls;
        public static MenuCommand menuDebugWindowsSep1;

        public static MenuCommand menuToolsAddTab; 
        public static MenuCommand menuToolsAddRemoveItems;    
        public static MenuCommand menuToolsSep1; 
        public static MenuCommand menuToolsOptions;
        public static MenuCommand menuToolsTest;
 
        public static MenuCommand menuWindowCloseAll;
        public static MenuCommand menuWindowSep1;  
        public static MenuCommand menuWindowWindows;  
        public static MenuCommand menuWindowSep2;   

        public static MenuCommand menuHelpAbout; 


        /// <summary>Create main menu and attach handlers</summary>
        public Crownwood.Magic.Menus.MenuControl Create()
        {
            Crownwood.Magic.Menus.MenuControl topMenu = new Crownwood.Magic.Menus.MenuControl();
            topMenu.Style     = main.Style;
            topMenu.Dock      = DockStyle.Top;
            topMenu.MultiLine = false;

                                            // Top menu
            menuFile   = new MenuCommand(Const.menuFile);     
            menuEdit   = new MenuCommand(Const.menuEdit);      
            menuView   = new MenuCommand(Const.menuView);      
            menuProject= new MenuCommand(Const.menuProject);  
            menuBuild  = new MenuCommand(Const.menuBuild); 
            menuDebug  = new MenuCommand(Const.menuDebug);   
            menuTools  = new MenuCommand(Const.menuTools);   
            menuWindow = new MenuCommand(Const.menuWindow);   
            menuHelp   = new MenuCommand(Const.menuHelp); 
    
            topMenu.MenuCommands.AddRange(new MenuCommand[]
            {
                menuFile,  menuEdit,  menuView, menuProject, menuBuild, 
                menuDebug, menuTools, menuWindow, menuHelp
            });


                                            // File menu 
            menuFileNew   = new MenuCommand(Const.menuFileNew,   new EventHandler(mh.OnFileNew));
            menuFileNew.Update += new EventHandler(mh.OnPopFileMenu);
            menuFileOpen  = new MenuCommand(Const.menuFileOpen,  new EventHandler(mh.OnFileOpen));
            menuFileClose = new MenuCommand(Const.menuFileClose, new EventHandler(mh.OnFileClose));
            menuFileSep1  = new MenuCommand(Const.dash);

            menuFileAddScript = new MenuCommand(Const.menuFileAddScript);
                                            // Add Script submenu       
            menuFileAddScript.Update += new EventHandler(mh.OnPopAddScriptMenu); 
            menuFileAddScriptNew   = new MenuCommand(Const.menuFileAddScriptNew, 
                                     new EventHandler(mh.OnFileAddScriptNew));
            menuFileAddScriptExist = new MenuCommand(Const.menuFileAddScriptExist, 
                                     new EventHandler(mh.OnFileAddScriptExisting));
            menuFileAddScript.MenuCommands.AddRange( new MenuCommand[]
            {
                menuFileAddScriptNew, menuFileAddScriptExist
            } );


            menuFileAddInstal      = new MenuCommand(Const.menuFileAddInstal);
                                            // Add Installer submenu
            menuFileAddInstalNew   = new MenuCommand(Const.menuFileAddInstalNew, 
                                     new EventHandler(mh.OnFileAddInstallerNew));
            menuFileAddInstalExist = new MenuCommand(Const.menuFileAddInstalExist, 
                                     new EventHandler(mh.OnFileAddInstallerExisting));
            menuFileAddInstal.MenuCommands.AddRange( new MenuCommand[]
            {
                menuFileAddInstalNew, menuFileAddInstalExist 
            } );


            menuFileAddLocales = new MenuCommand(Const.menuFileAddLocales);
                                            // Add Installer submenu
            menuFileAddLocalesNew = new MenuCommand(Const.menuFileAddLocalesNew,
                                     new EventHandler(mh.OnFileAddLocalesNew));
            menuFileAddLocalesExist = new MenuCommand(Const.menuFileAddLocalesExist,
                                     new EventHandler(mh.OnFileAddLocalesExisting));
            menuFileAddLocales.MenuCommands.AddRange(new MenuCommand[]
            {
                menuFileAddLocalesNew, menuFileAddLocalesExist 
            });
                    
                         
            menuFileAddResource = new MenuCommand(Const.menuFileAddResource);
                                            // Add Resource submenu 0120      
            menuFileAddResource.Update += new EventHandler(mh.OnPopAddResourceMenu); 

            menuFileAddResxDatabase  = new MenuCommand(Const.menuFileAddDbScript);
                                            // Add Database submenu
            menuFileAddDbScriptNew   = new MenuCommand(Const.menuFileAddDbScriptNew, 
                                       new EventHandler(mh.OnFileAddDbScriptNew));
            menuFileAddDbScriptExist = new MenuCommand(Const.menuFileAddDbScriptExist, 
                                       new EventHandler(mh.OnFileAddDbScriptExisting));
            menuFileAddResxDatabase.MenuCommands.AddRange( new MenuCommand[]
            {
                menuFileAddDbScriptNew, menuFileAddDbScriptExist 
            } );

            menuFileAddResxMedia     = new MenuCommand(Const.menuFileAddMedia);
                                            // Add Media submenu
            menuFileAddAudioExist    = new MenuCommand(Const.menuFileAddMediaExist, 
                                       new EventHandler(mh.OnFileAddAudioExisting));
            menuFileAddResxMedia.MenuCommands.Add(menuFileAddAudioExist);
            menuFileAddResxVoiceRec  = new MenuCommand(Const.menuFileAddVrResx, // 0120
                                       new EventHandler(mh.OnFileAddVrResxExisting));
            #if(false)
            menuFileAddTtsText       = new MenuCommand(Const.menuFileAddTtsText);
            menuFileAddTtsTextNew    = new MenuCommand(Const.menuFileAddTtsTextNew, 
                                       new EventHandler(mh.OnFileAddTtsTextNew));
            menuFileAddTtsTextExist  = new MenuCommand(Const.menuFileAddTtsTextExist, 
                                       new EventHandler(mh.OnFileAddTtsTextExisting));
            menuFileAddTtsText.MenuCommands.AddRange( new MenuCommand[]
            {
                menuFileAddTtsTextNew, menuFileAddTtsTextExist 
            } );
            #endif

            menuFileAddAudioExist    = new MenuCommand(Const.menuFileAddMediaExist, 
                new EventHandler(mh.OnFileAddAudioExisting));

            menuFileAddResource.MenuCommands.AddRange( new MenuCommand[]
            {
                menuFileAddResxDatabase, menuFileAddResxMedia, menuFileAddResxVoiceRec
                // , menuFileAddTtsText // remove comment to include TTS text on main menus
            } );
     
            menuFileCloseFile = new MenuCommand(Const.menuFileCloseFile, 
                new EventHandler(mh.OnFileCloseFile));

            // After a new project is created we can't get a response to the save
            // shortcut until we've done a save as, so we'll work around this by
            // assigning both the same shortcut and handler, and hiding save as 
            menuFileSave      = new MenuCommand(Const.menuFileSave,   Shortcut.CtrlS, 
                                new EventHandler(mh.OnFileSave));
            menuFileSaveAs    = new MenuCommand(Const.menuFileSaveAs, Shortcut.CtrlS,
                                new EventHandler(mh.OnFileSave));
            menuFileSaveAll   = new MenuCommand(Const.menuFileSaveAll,
                                new EventHandler(mh.OnFileSaveAll));
            menuFileSep2      = new MenuCommand(Const.dash);
            menuFilePageSet   = new MenuCommand(Const.menuFileSetup , 
                                new EventHandler(mh.OnFilePageSetup));
            menuFilePrint     = new MenuCommand(Const.menuFilePrint,  Shortcut.CtrlP,
                                new EventHandler(mh.OnFilePrint));
            menuFilePreview   = new MenuCommand(Const.menuFilePreview,
                                new EventHandler(mh.OnFilePrintPreview));
            menuFileSep3      = new MenuCommand(Const.dash);
            menuFileRecent    = new MenuCommand(Const.menuFileRecent);
            menuFileRecent.Update += new EventHandler(mh.OnPopFileRecentFilesMenu);
            menuFileSep4      = new MenuCommand(Const.dash);
            menuFileExit      = new MenuCommand(Const.menuFileExit,   
                                new EventHandler(mh.OnFileExit));

            menuFile.MenuCommands.AddRange( new MenuCommand[]
            {
                menuFileNew,       menuFileOpen,      menuFileClose,      menuFileSep1, 
                menuFileAddScript, menuFileAddInstal, menuFileAddLocales, menuFileAddResource, 
                menuFileCloseFile, menuFileSep1,  
                menuFileSave,      menuFileSaveAs,    menuFileSaveAll,    menuFileSep2,   
                menuFilePageSet,   menuFilePreview,   menuFilePrint,      menuFileSep3,      
                menuFileRecent,    menuFileSep4,      menuFileExit
            } );

            menuFilePrint.Enabled   = menuFilePreview.Enabled = menuFilePageSet.Enabled =true;
            menuFileCloseFile.Enabled = false; 
            menuFileSaveAll.Visible = false; 


            // Edit menu 
            // Note that menu shortcuts, being keyboard hooks, trump GoDiagram and
            // docking window shortcut key handling. We have therefore not specifi-
            // cally assigned shortcuts Ctrl+C, Ctrl+V, and Del, since: 
            // (a) we need these key combos in explorer, properties, output, etc; 
            // (b) there is no way (currently to display the shortcut text (in the
            // normal right-justified manner) without actually assigning a shortcut; 
            // (c) windows which need these shortcuts handle them themselves. (Note
            // that we can change Crownwood source code to, say recognize a tab char
            // in the menu text, and display text after tab as shortcut; however this
            // is extreme if we are going to lose the Crownwood framework as seems
            // likely. Other shorcuts left in do not interfere with other windows.
      
            menuEditUndo      = new MenuCommand(Const.menuEditUndo,   Shortcut.CtrlZ, 
                                new EventHandler(mh.OnEditUndo));
            menuEditUndo.Update += new EventHandler(mh.OnPopEditMenu);
            menuEditRedo      = new MenuCommand(Const.menuEditRedo,   Shortcut.CtrlY, 
                                new EventHandler(mh.OnEditRedo));
            menuEditSep1      = new MenuCommand(Const.dash);
            menuEditCut       = new MenuCommand(Const.menuEditCut,    Shortcut.CtrlX, 
                                new EventHandler(mh.OnEditCut)); 
            menuEditCopy      = new MenuCommand(Const.menuEditCopy,   Shortcut.CtrlC, 
                                new EventHandler(mh.OnEditCopy));
            menuEditPaste     = new MenuCommand(Const.menuEditPaste,  Shortcut.CtrlV, 
                                new EventHandler(mh.OnEditPaste));
            menuEditDelete    = new MenuCommand(Const.menuGenericDelete, Shortcut.Del,
                                new EventHandler(mh.OnEditDelete));
            menuEditSep2      = new MenuCommand(Const.dash);
            menuEditSelectAll = new MenuCommand(Const.menuEditSelAll,  Shortcut.CtrlA,
                                new EventHandler(mh.OnEditSelectAll));
            menuEditSep3      = new MenuCommand(Const.dash);
            menuEditGoTo      = new MenuCommand(Const.menuGenericGoTo, Shortcut.CtrlG,
                                new EventHandler(mh.OnEditGoTo));

            menuEdit.MenuCommands.AddRange( new MenuCommand[]
            {
                menuEditUndo, menuEditRedo,  menuEditSep1,   menuEditCut,      
                menuEditCopy, menuEditPaste, menuEditDelete, menuEditSep2, 
                menuEditSelectAll, menuEditSep3, menuEditGoTo 
            } );


                                            // View menu 
            menuViewZoom        = new MenuCommand(Const.menuViewZoom); 
            menuViewZoom.Update   += new EventHandler(mh.OnPopViewMenu); 
            this.CreateZoomSubmenu();
            menuViewSep1        = new MenuCommand(Const.dash);
            menuViewGrid        = new MenuCommand(Const.menuViewGrid,    Shortcut.CtrlShiftG,
                                  new EventHandler(mh.OnViewGrid));     
            menuViewSep2        = new MenuCommand(Const.dash);
            menuViewCanvas      = new MenuCommand(Const.menuViewCanvas,  Shortcut.CtrlShiftC,
                                  new EventHandler(mh.OnViewCanvas));
            menuViewTray        = new MenuCommand(Const.menuViewTray,    Shortcut.Alt2,
                                  new EventHandler(mh.OnViewTray));
            menuViewSep3        = new MenuCommand(Const.dash);
            menuViewProperties  = new MenuCommand(Const.menuViewProps,   Shortcut.F4,
                                  new EventHandler(mh.OnViewProperties));
            menuViewToolbox     = new MenuCommand(Const.menuViewToolbox, Shortcut.CtrlShiftX,
                                  new EventHandler(mh.OnViewToolbox));
            menuViewExplorer    = new MenuCommand(Const.menuViewExplorer,Shortcut.CtrlShiftL,
                                  new EventHandler(mh.OnViewExplorer));
            menuViewOutput      = new MenuCommand(Const.menuViewOutput,  Shortcut.CtrlShiftO,
                                  new EventHandler(mh.OnViewOutput));
            menuViewOverview    = new MenuCommand(Const.menuViewOverview,Shortcut.CtrlShiftW,
                                  new EventHandler(mh.OnViewOverview));   
                                            // Hidden items - shortcut use only
            menuViewFocusCanvas = new MenuCommand(Const.emptystr, Shortcut.Alt1,
                                  new EventHandler(mh.OnViewFocusCanvas));    
            menuViewFocusTray   = new MenuCommand(Const.emptystr, Shortcut.Alt3,
                                  new EventHandler(mh.OnViewFocusTray));  
            menuViewFocusProps  = new MenuCommand(Const.emptystr, Shortcut.Alt4,
                                  new EventHandler(mh.OnViewFocusProperties));
            menuViewFocusExplor = new MenuCommand(Const.emptystr, Shortcut.Alt5,
                                  new EventHandler(mh.OnViewFocusExplorer));
            menuViewFocusTools  = new MenuCommand(Const.emptystr, Shortcut.Alt6,
                                  new EventHandler(mh.OnViewFocusToolbox));

            MaxDebugUtil debugger = MaxDebugger.Instance.Util;

            menuViewShowConsole = new MenuCommand(Const.emptystr, Shortcut.Alt7,
                                  new EventHandler(debugger.OnViewShortcutConsole));
            menuViewShowBkpts   = new MenuCommand(Const.emptystr, Shortcut.Alt8,
                                  new EventHandler(debugger.OnViewShortcutBreakpoints));
            menuViewShowStack   = new MenuCommand(Const.emptystr, Shortcut.Alt9,
                                  new EventHandler(debugger.OnViewShortcutCallStack));
            menuViewShowWatch   = new MenuCommand(Const.emptystr, Shortcut.Alt0,
                                  new EventHandler(debugger.OnViewShortcutWatchWindow));     

            menuViewFocusCanvas.Visible = menuViewFocusTray.Visible   = 
            menuViewFocusProps.Visible  = menuViewFocusExplor.Visible = 
            menuViewFocusTools.Visible  = false;  

            menuViewShowConsole.Visible = menuViewShowBkpts.Visible = 
            menuViewShowStack.Visible   = menuViewShowWatch.Visible = false;   
      
            menuView.MenuCommands.AddRange(new MenuCommand[]
            {
                menuViewZoom, menuViewSep1,   menuViewGrid, menuViewSep2, 
                menuViewTray, menuViewCanvas, menuViewSep3, menuViewProperties, 
                menuViewToolbox,  menuViewExplorer, menuViewOutput, 
                menuViewOverview, 
                menuViewFocusCanvas, menuViewFocusTray, menuViewFocusProps, 
                menuViewFocusExplor, menuViewFocusTools,
                menuViewShowConsole, menuViewShowBkpts,
                menuViewShowStack,   menuViewShowWatch
            } );


                                            // Project menu 
            menuProjectProperties    = new MenuCommand(Const.menuProjectProperties, 
                                       new EventHandler(mh.OnProjectProperties));
            menuProjectAddReference  = new MenuCommand(Const.menuProjectAddReference, 
                                       new EventHandler(mh.OnProjectAddReference));
            menuProjectAddWebService = new MenuCommand(Const.menuProjectAddWebService, 
                                       new EventHandler(mh.OnProjectAddWebService));
            menuProjectSep1          = new MenuCommand(Const.dash);
            menuProjectSep2          = new MenuCommand(Const.dash);

            menuProject.PopupStart  += new CommandHandler(mh.OnPopProjectMenu); 

            menuProject.MenuCommands.AddRange( new MenuCommand[]
            {
                menuFileAddScript, menuFileAddInstal, menuFileAddLocales, menuFileAddResource, // 0120
                menuProjectSep1, menuProjectAddReference, menuProjectAddWebService,
                menuProjectSep2, menuProjectProperties,
            } );


                                            // Build menu 
            menuBuildBuildProject = new MenuCommand(Const.menuBuildBuildProject, Shortcut.CtrlShiftB,
                                    new EventHandler(mh.OnBuildBuildProject));
            menuBuildBuildProject.Update += new EventHandler(mh.OnPopBuildMenu); 

            menuBuildDeploy       = new MenuCommand(Const.menuBuildDeploy, Shortcut.CtrlShiftD,
                                    new EventHandler(mh.OnBuildDeployProject));

            menuBuild.MenuCommands.AddRange( new MenuCommand[]
            {
                menuBuildBuildProject, menuBuildDeploy
            } );


                                            // Debug menu 
            menuDebugWindows      = new MenuCommand(Const.menuDebugWindows);
      
            menuDebugWindowsCon   = new MenuCommand(Const.menuViewConsole, 
                                    new EventHandler(debugger.OnMenuWindowsConsole));
            menuDebugWindowsBkpts = new MenuCommand(Const.menuDebugWindowsBkpts, 
                                    new EventHandler(debugger.OnMenuWindowsBreakpoints));
            menuDebugWindowsWatch = new MenuCommand(Const.menuDebugWindowsWatch, 
                                    new EventHandler(debugger.OnMenuWindowsWatch));
            menuDebugWindowsCalls = new MenuCommand(Const.menuDebugWindowsCalls, 
                                    new EventHandler(debugger.OnMenuWindowsCallStack));
            menuDebugWindowsSep1  = new MenuCommand(Const.dash);

            menuDebug.PopupStart += new CommandHandler(mh.OnPopDebugMenu); 

            menuDebugWindowsWatch.Visible = menuDebugWindowsCalls.Visible = 
            menuDebugWindowsSep1.Visible  = false;

            menuDebugWindows.MenuCommands.Add(menuDebugWindowsCon);
            menuDebugWindows.MenuCommands.Add(menuDebugWindowsBkpts);
            menuDebugWindows.MenuCommands.Add(menuDebugWindowsSep1);
            menuDebugWindows.MenuCommands.Add(menuDebugWindowsWatch);
            menuDebugWindows.MenuCommands.Add(menuDebugWindowsCalls); 

            menuDebugWindows.PopupStart += new CommandHandler(mh.OnPopDebugWindowsMenu); 

            menuDebugSep1     = new MenuCommand(Const.dash);
            menuDebugStart    = new MenuCommand(Const.menuDebugStart, Shortcut.F5,
                                new EventHandler(debugger.OnMenuStartDebugging));
            menuDebugStop     = new MenuCommand(Const.menuDebugStop,  Shortcut.ShiftF5,
                                new EventHandler(debugger.OnMenuStopDebugging));

            menuDebugStepOver = new MenuCommand(Const.menuDebugStepOver, Shortcut.F11,
                                new EventHandler(debugger.OnMenuStepOver));
            menuDebugStepInto = new MenuCommand(Const.menuDebugStepInto, Shortcut.ShiftF11,
                                new EventHandler(debugger.OnMenuStepInto));
            //nuDebugNewBkpt  = new MenuCommand(Const.menuDebugNewBkpt,
            //                  new EventHandler(debugger.OnMenuNewBreakpoint));
            menuDebugToggleBkpt = new MenuCommand(Const.menuDebugToggleBkpt, Shortcut.F9,
                                  new EventHandler(debugger.OnMenuToggleBreakpoint));
            menuDebugClearBkpts = new MenuCommand(Const.menuDebugClearBkpts,
                                  new EventHandler(debugger.OnMenuClearBreakpoints));
            menuDebugDisableBkpts = new MenuCommand(Const.menuDebugDisableBkpts,
                                    new EventHandler(debugger.OnMenuDisableBreakpoints));
            menuDebugStartConsole = new MenuCommand(Const.menuDebugStartConsole,
                                    new EventHandler(debugger.OnMenuStartConsole));
            menuDebugStopConsole  = new MenuCommand(Const.menuDebugStopConsole,
                                    new EventHandler(debugger.OnMenuStopConsole));

            menuDebugStop.Visible = menuDebugStopConsole.Visible = false;

            menuDebug.MenuCommands.AddRange( new MenuCommand[]
            {   menuDebugWindows,    menuDebugSep1, 
                menuDebugStart,      menuDebugStop,       menuDebugSep1,       
                menuDebugStepInto,   menuDebugStepOver,   menuDebugSep1,
                menuDebugToggleBkpt, menuDebugClearBkpts, menuDebugDisableBkpts,
                menuDebugSep1,       menuDebugStartConsole, menuDebugStopConsole
            } );

      
                                            // Tools menu 
            menuToolsAddTab   = new MenuCommand(Const.menuToolsAddTab, 
                                new EventHandler(mh.OnToolsAddToolboxTab));
            menuToolsAddRemoveItems = new MenuCommand(Const.menuToolsAddRemove, 
                                new EventHandler(mh.OnToolsAddRemoveItems));
            menuToolsSep1     = new MenuCommand(Const.dash);
            menuToolsTest     = new MenuCommand(Const.menuToolsTest,
                                new EventHandler(mh.OnToolsTest));
            menuToolsOptions  = new MenuCommand(Const.menuToolsOptions, 
                                new EventHandler(mh.OnToolsOptions));
            menuToolsOptions.Update += new EventHandler(mh.OnPopToolsMenu);

            menuToolsTest.Visible = Config.EnableTestMenu;
      
            menuTools.MenuCommands.AddRange( new MenuCommand[]
            {   menuToolsAddTab, menuToolsAddRemoveItems, 
                menuToolsSep1, menuToolsTest, menuToolsOptions  
            } );

                       
                                            // Window menu 
            menuWindowCloseAll = new MenuCommand(Const.menuWindowCloseAll, 
                                 new EventHandler(mh.OnWindowCloseAll));
            menuWindowSep1     = new MenuCommand(Const.dash);
            menuWindowWindows  = new MenuCommand(Const.menuWindowWindows,  
                                 new EventHandler(mh.OnWindowWindows));
            menuWindowSep2     = new MenuCommand(Const.dash);
            menuWindow.PopupStart += new Crownwood.Magic.Menus.CommandHandler(mh.OnPopWindowMenu); 
            menuWindow.PopupEnd   += new Crownwood.Magic.Menus.CommandHandler(mh.OnDismissWindowMenu);
            // Non-empty menu required for PopupStart event to fire
            menuWindow.MenuCommands.Add(menuWindowSep1);
      

            // Help menu 
            menuHelpAbout = new MenuCommand(Const.menuHelpAbout, new EventHandler(mh.OnHelpAbout));

            menuHelp.MenuCommands.AddRange( new MenuCommand[]
            {
                menuHelpAbout
            } );


            return topMenu;

        } // Create()

    

        /// <summary>Create submenu with each recent project as a menu item</summary>
        public void CreateRecentFilesSubmenu()
        {
            string[] recentFiles = null;
            int  recentFileCount = Config.RecentFiles.First == null? 
                Config.RecentFiles.Load(): Config.RecentFiles.Count;

            if  (recentFileCount > 0)  
                 recentFiles = Config.RecentFiles.Contents();

            if  (recentFiles == null || recentFiles.Length == 0)
            {
                menuFileRecent.Enabled = false;
                return;
            }

            ArrayList commands = new ArrayList(recentFiles.Length);

            for(int i=0; i < recentFileCount; i++)
                commands.Add(new MenuCommand(recentFiles[i], 
                             new EventHandler(mh.OnRecentFileSelected)));
       
            MenuCommand[] menuCommands = new MenuCommand[commands.Count];
            commands.ToArray().CopyTo(menuCommands,0);

            menuFileRecent.MenuCommands.Clear();
            menuFileRecent.MenuCommands.AddRange(menuCommands);         
        }



        /// <summary>Build Window menu and append window list to it</summary>
        public void CreateDynamicWindowMenu()
        {
            MaxMenu.menuWindowCloseAll.Enabled = MaxMenu.menuWindowWindows.Enabled 
                = main.ProjectExists;

            menuWindow.MenuCommands.AddRange( new MenuCommand[]
            {
                menuWindowCloseAll, menuWindowSep1  
            } );

            this.AppendWindowsToWindowMenu();

            menuWindow.MenuCommands.Add(menuWindowWindows); 
            MaxMenu.menuWindowWindows.Visible = false; // until we code the dialog       
        }


        /// <summary>Append window list to Window menu</summary>    
        private void AppendWindowsToWindowMenu()
        {
            if (main.Explorer  == null || main.CurrentViewName == null) return;
 
            MaxTreeNode viewnode = main.Explorer.FindByViewName
                (main.CurrentViewType, main.CurrentViewName);
            if (viewnode == null) return;

            if (viewnode.isSingleton())
            {   // A singleton treenode has no "canvas" nodes, it *is* the tab
                MenuCommand menuentry = new MenuCommand(viewnode.NodeName, 
                    new EventHandler(mh.OnWindowWindowSelected));
                menuWindow.MenuCommands.Add(menuentry); 

                MaxExplorerWindow.AppInfo info = viewnode.Tag as MaxExplorerWindow.AppInfo;
                menuentry.Checked = info == null? true: info.isShown;
            }
            else foreach(MaxTreeNode node in viewnode.Nodes)     
            {
                MaxExplorerWindow.CanvasInfo info = node.Tag as MaxExplorerWindow.CanvasInfo;
                if (info == null) continue;

                MenuCommand menuentry = new MenuCommand(info.canvasName, 
                                        new EventHandler(mh.OnWindowWindowSelected));
                menuentry.Checked = info.isShown;
                menuWindow.MenuCommands.Add(menuentry); 
            } 
        }


        /// <summary>Create submenu for View/Zoom</summary>
        private void CreateZoomSubmenu()
        {    
            menuViewZoomZoomIn     = new MenuCommand(Const.menuViewZoomIn,  
                                     new EventHandler(mh.OnViewZoomIn));
            menuViewZoomZoomOut    = new MenuCommand(Const.menuViewZoomOut, 
                                     new EventHandler(mh.OnViewZoomOut));
            menuViewZoomZoomNormal = new MenuCommand(Const.menuViewZoomNorm,
                                     new EventHandler(mh.OnViewZoomNormal));
       
            MenuCommand[] menuCommands = new MenuCommand[] 
            {
                menuViewZoomZoomIn, menuViewZoomZoomOut, menuViewZoomZoomNormal
            };      
      
            menuViewZoom.MenuCommands.AddRange(menuCommands);         
        }


        /// <summary>Show context menu on satellite window title bar</summary>
        public void OnDockingContextMenu(PopupMenu pm)
        {      
            // First determine which docking window is under the mouse
            // This does not work if the window is floating. We need to tweak
            // the "under XY" code appropriately.

            Content satelliteContent = mainx.SatelliteUnderXY(Control.MousePosition);
            if  (satelliteContent == null) return;
            MaxSatelliteWindow sw = satelliteContent.Control as MaxSatelliteWindow;
            if  (sw == null) return;
        
            MenuCommand mcA = new MenuCommand(Const.menuDockDock, 
                              new EventHandler(mh.OnDockingContextDockable));
            MenuCommand mcB = new MenuCommand(Const.menuDockHide,     
                              new EventHandler(mh.OnDockingContextHide));
            MenuCommand mcC = new MenuCommand(Const.menuDockFloat, 
                              new EventHandler(mh.OnDockingContextFloating));
            MenuCommand mcD = new MenuCommand(Const.menuDockAuto,
                              new EventHandler(mh.OnDockingContextAutoHide));

            mcA.Checked = true;
            mcD.Checked = satelliteContent.AutoHidePanel != null;
            mcA.Tag = mcB.Tag = mcC.Tag = mcD.Tag = satelliteContent;

            // We need to figure out how to undock and redock content programatically
            // by examining the source code. There are no methods provided to do this 

            mcA.Enabled = mcC.Enabled = false;

            pm.MenuCommands.Clear();        // Clear Crownwood default context menu
            pm.MenuCommands.Insert(0, mcA); // and replace it with our own
            pm.MenuCommands.Insert(1, mcB);
            pm.MenuCommands.Insert(2, mcC);
            pm.MenuCommands.Insert(3, mcD);

            // pm.MenuCommands.Remove(pm.MenuCommands["Show All"]);
            // pm.MenuCommands.Remove(pm.MenuCommands["Hide All"]);
        }

    } // class MaxMenu

}   // namespace
