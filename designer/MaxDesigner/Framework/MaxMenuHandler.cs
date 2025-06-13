using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.Magic.Docking;
using Crownwood.Magic.Menus;
using Metreos.Max.GlobalEvents;
using Metreos.Max.Debugging;
using Metreos.Max.Core;
using Metreos.Max.Framework.Satellite.Property;



namespace Metreos.Max.Framework
{
    /// <summary>Menu first-chance handlers</summary>
    public class MaxMenuHandlers
    {
        public MaxMenuHandlers(MaxMain maxmain, MaxMenu menu)
        {
            main = maxmain;
            mm   = menu;
            RaiseMenuActivity += InboundHandlers.MenuActivityCallback;
            RaiseUserInput    += InboundHandlers.UserInputCallback;
        }

        private MaxMain main;
        private MaxMenu mm;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Events
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Fires inbound menu activity event</summary>
        public event GlobalEvents.MaxMenuActivityHandler RaiseMenuActivity;
        /// <summary>Fires inbound user input event</summary>
        public event GlobalEvents.MaxUserInputHandler    RaiseUserInput;

  
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Handlers
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>User selected a recent file entry from submenu</summary>
        public void OnRecentFileSelected(object sender, EventArgs e)
        {      
            MenuCommand menuCommand = (MenuCommand)sender;
            string path = menuCommand.Text;

            if (!System.IO.File.Exists(path)) 
            {
                Utl.ShowBadProjectFileDlg(path);
                Config.RecentFiles.Remove(path);
                return;
            }

            if (main.ProjectExists)          
                main.RequestPersistIDE(true);   

            MaxUserInputEventArgs args = main.Dialog.InitiateOpenProjectSequence(path); 
            if (args == null) return;      

            RaiseUserInput(this, args);
        }


        /// <summary>Invoked prior to display of File menu</summary>
        public void OnPopFileMenu(object sender, EventArgs e)
        {
            MaxMenu.menuFileClose.Visible  
                = MaxMenu.menuFileAddScript.Visible = MaxMenu.menuFileAddResource.Visible  
                = MaxMenu.menuFileAddInstal.Visible = MaxMenu.menuFileCloseFile.Visible  
                = MaxMenu.menuFileSep1.Visible      = MaxMenu.menuFileSave.Visible    
                = MaxMenu.menuFileSaveAs.Visible    = MaxMenu.menuFileSaveAll.Visible  
                = MaxMenu.menuFileSep2.Visible      = MaxMenu.menuFilePrint.Visible   
                = MaxMenu.menuFilePreview.Visible   = MaxMenu.menuFilePageSet.Visible   
                = MaxMenu.menuFileAddLocales.Visible
                = main.ProjectExists;

            MaxMenu.menuFileSaveAs.Visible = MaxMenu.menuFileSaveAll.Visible = false;  // for now

            MaxMenu.menuFileAddInstal.Enabled = main.ProjectExists && !main.InstallerPresent;
            
            MaxMenu.menuFileAddLocales.Enabled = main.ProjectExists && !main.LocalesPresent;

            MaxMenu.menuFileSave.Enabled = main.ProjectDirty;
        }


        /// <summary>Invoked prior to display of File/Recent Files menu</summary>
        public void OnPopFileRecentFilesMenu(object sender, EventArgs e)
        {
            mm.CreateRecentFilesSubmenu();
        }
        

        public void OnFileNew(object sender, EventArgs e)
        {
            MaxUserInputEventArgs inputEventArgs = main.Dialog.PromptNewProject();
            bool didUserCancel = (inputEventArgs == null);

            if (!didUserCancel) 
            { 
                RaiseUserInput(this, inputEventArgs);

                // Pop new script dialog after a blank project is opened 20060914
                if (main.ProjectExists && !Config.SuppressNewScriptDialog)  
                    this.OnFileAddScriptNew(null, null);
            }
        }


        public void OnFileOpen(object sender, EventArgs e)
        {
            MaxUserInputEventArgs inputEventArgs = main.Dialog.PromptOpenProject();

            if  (inputEventArgs != null) RaiseUserInput(this, inputEventArgs);
        }


        /// <summary>Invoked on File/Close Project selected</summary>
        public void OnFileClose(object sender, EventArgs e)
        {
            MaxUserInputEventArgs args = main.BeforeCloseProject();
            if (args != null) RaiseUserInput(this, args);                                                                
        }


        public void OnFileAddScriptNew(object sender, EventArgs e)
        { 
            MaxUserInputEventArgs inputEventArgs = main.PromptNewScript();  

            if (inputEventArgs != null)  
                RaiseUserInput(this, inputEventArgs);                                                                      
        }


        public void OnFileAddScriptExisting(object sender, EventArgs e)
        { 
            MaxUserInputEventArgs inputEventArgs = main.PromptExistingScript();  

            if (inputEventArgs != null)  
                RaiseUserInput(this, inputEventArgs);                                                                      
        }


        #region OnFileOpenScript
        #if(false)
        public void OnFileOpenScript(object sender, EventArgs e)
        {
        // This is not currently hooked up to the main menu, however we do invoke
        // from explorer context menu. This will be invoked eventually also from
        // somewhere on the file menu, not yet determined.
        MaxUserInputEventArgs inputEventArgs = main.PromptOpenScript(); 

        RaiseUserInput(this, inputEventArgs);                                                                       
        }
        #endif
        #endregion


        /// <summary>Invoked prior to display of File/Add Script</summary>
        public void OnPopAddResourceMenu(object sender, EventArgs e)
        {
            
        }

        /// <summary>Invoked prior to display of File/Add Script</summary>
        public void OnPopAddScriptMenu(object sender, EventArgs e)
        {
            // We can lose this -- moved everything to OnPopFileMenu
        }

        /// <summary>Convenience method to fire user input with event type only</summary>
        public void SignalUserInput(MaxUserInputEventArgs.MaxEventTypes type)
        {
            RaiseUserInput(this, new MaxUserInputEventArgs(type, null));
        }


        /// <summary>Convenience method to fire user input with event type and string</summary>
        public void SignalUserInput(MaxUserInputEventArgs.MaxEventTypes type, string s)
        {
            RaiseUserInput(this, new MaxUserInputEventArgs(type, s));
        }


        /// <summary>Convenience method to fire user input with event type and int</summary>
        public void SignalUserInput(MaxUserInputEventArgs.MaxEventTypes type, int n)
        {
            RaiseUserInput(this, new MaxUserInputEventArgs(type, n));
        }


        public void OnFileAddInstallerNew(object sender, EventArgs e)
        {
            MaxUserInputEventArgs args = main.PromptNewInstaller();  

            if (args != null) RaiseUserInput(this, args);  
        }


        public void OnFileAddInstallerExisting(object sender, EventArgs e)
        {
            MaxUserInputEventArgs args = main.PromptExistingInstaller();

            if (args != null) RaiseUserInput(this, args);
        }


        public void OnFileAddLocalesNew(object sender, EventArgs e)
        {
            MaxUserInputEventArgs args = main.PromptNewLocales();

            if (args != null) RaiseUserInput(this, args);
        }


        public void OnFileAddLocalesExisting(object sender, EventArgs e)
        {
            MaxUserInputEventArgs args = main.PromptExistingLocales();

            if (args != null) RaiseUserInput(this, args);
        }


        public void OnFileAddDbScriptNew(object sender, EventArgs e)
        {
            MaxUserInputEventArgs args = main.PromptNewDatabase();

            if (args != null) RaiseUserInput(this, args);
        }


        public void OnFileAddDbScriptExisting(object sender, EventArgs e)
        {
            MaxUserInputEventArgs args = main.PromptExistingDatabase();

            if (args != null) RaiseUserInput(this, args);                                                            
        }


        public void OnFileAddAudioExisting(object sender, EventArgs e)
        {
            main.OnAddMediaFile();
        }

                                            // 0120
        public void OnFileAddVrResxExisting(object sender, EventArgs e)
        {
            main.OnAddVoiceRecResource();
        }

                                            // 0120
        public void OnFileAddTtsTextExisting(object sender, EventArgs e)
        {
            // MaxUserInputEventArgs args = main.PromptExistingDatabase();

            // if (args != null) RaiseUserInput(this, args);                                                            
        }

                                            // 0120
        public void OnFileAddTtsTextNew(object sender, EventArgs e)
        {
            // MaxUserInputEventArgs args = main.PromptExistingDatabase();

            // if (args != null) RaiseUserInput(this, args);                                                            
        }


        public void OnFileCloseFile(object sender, EventArgs e)
        {
            SignalUserInput(MaxUserInputEventArgs.MaxEventTypes.CloseFile);
        }


        public void OnFileSave(object sender, EventArgs e)
        { 
            // Invoked only from menu or ctrl-s, since no force option

            if (!main.ProjectDirty) return;   // Reject ctrl+s when not dirty (329)

            MaxUserInputEventArgs args = main.BeforeSaveProject(); 
            if (args != null) RaiseUserInput(this, args);
        }


        public void OnFileSaveAs(object sender, EventArgs e)
        {
            MaxUserInputEventArgs args = main.Dialog.PromptSaveProject(); 

            if (args != null) RaiseUserInput(this, args);
        }


        public void OnFileSaveAll(object sender, EventArgs e)
        {

        }


        public void OnFilePageSetup(object sender, EventArgs e)
        {
            SignalUserInput(MaxUserInputEventArgs.MaxEventTypes.PageSet);
        }


        public void OnFilePrint(object sender, EventArgs e)
        {
            SignalUserInput(MaxUserInputEventArgs.MaxEventTypes.Print);
        }


        public void OnFilePrintPreview(object sender, EventArgs e)
        {
            SignalUserInput(MaxUserInputEventArgs.MaxEventTypes.PrintPreview);
        }


        public void OnFileExit(object sender, EventArgs e)
        { 
            main.Close();
        }


        /// <summary>Invoked prior to display of Edit menu</summary>
        public void OnPopEditMenu(object sender, EventArgs e)
        {
            if (!main.ProjectExists)
            {
                MaxMenu.menuEditUndo.Enabled  = MaxMenu.menuEditRedo.Enabled   = 
                MaxMenu.menuEditCut.Enabled   = MaxMenu.menuEditCopy.Enabled   = 
                MaxMenu.menuEditPaste.Enabled = MaxMenu.menuEditDelete.Enabled =  
                MaxMenu.menuEditSelectAll.Enabled = MaxMenu.menuEditGoTo.Enabled = false; 
                return;
            }
   
            MaxMenu.menuEditUndo.Enabled      = mp.CanUndo;
            MaxMenu.menuEditRedo.Enabled      = mp.CanRedo;
            MaxMenu.menuEditCut.Enabled       = mp.CanDelete;
            MaxMenu.menuEditCopy.Enabled      = mp.CanCopy;
            MaxMenu.menuEditPaste.Enabled     = mp.CanPaste;
            MaxMenu.menuEditSelectAll.Enabled = mp.CanSelectAll || IsSelectableSatelliteItemFocused();
            MaxMenu.menuEditDelete.Enabled    = mp.CanDelete    || IsDeletableSatelliteItemFocused();          
        }

        public static void EnableEditShortcuts()
        {
            MaxMenu.menuEditUndo.Enabled  = MaxMenu.menuEditRedo.Enabled    = 
            MaxMenu.menuEditCut.Enabled   = MaxMenu.menuEditCopy.Enabled    =  
            MaxMenu.menuEditPaste.Enabled = MaxMenu.menuEditDelete.Enabled  = 
            MaxMenu.menuEditSelectAll.Enabled = true;         
        }

        public static void DisableEditShortcuts()
        {
            MaxMenu.menuEditUndo.Enabled  = MaxMenu.menuEditRedo.Enabled    = 
            MaxMenu.menuEditCut.Enabled   = MaxMenu.menuEditCopy.Enabled    =  
            MaxMenu.menuEditPaste.Enabled = MaxMenu.menuEditDelete.Enabled  = 
            MaxMenu.menuEditSelectAll.Enabled = false;             
        }


        private bool IsDeletableSatelliteItemFocused()
        {
            if (main.Explorer.ContainsFocus)       
                return main.Explorer.IsDeletableItemSelected();

            if (main.Toolbox.ContainsFocus)
                return main.Toolbox.IsDeletableItemSelected();
       
            return false;
        }


        /// <summary>Determine if can select all in current context</summary>
        private bool IsSelectableSatelliteItemFocused()
        {
            return (main.Output.ContainsFocus || MaxDebugger.Instance.ConsoleControl.ContainsFocus);               
        }


        public void OnEditUndo(object sender, EventArgs e)
        { 
            RaiseMenuActivity(this, ArgsUndo); 
        }


        public void OnEditRedo(object sender, EventArgs e)
        { 
            RaiseMenuActivity(this, ArgsRedo); 
        }


        public void OnEditCut(object sender, EventArgs e)
        { 
            RaiseMenuActivity(this, ArgsCut); 
        }


        public void OnEditCopy(object sender, EventArgs e)
        { 
            if  (main.Output.ContainsFocus)
                 main.Output.Copy();
 
            else RaiseMenuActivity(this, ArgsCopy);
        }


        public void OnEditPaste(object sender, EventArgs e)
        { 
            RaiseMenuActivity(this, ArgsPaste); 
        }


        /// <summary>Handle Del key or Edit/Delete</summary>
        public void OnEditDelete(object sender, EventArgs e)
        { 
            // Del key seems to be caught here prior to focused window. If a satellite
            // has focus and can handle a Delete action, let that control handle it.
            // Otherwise forward delete to Max

            if  (main.Explorer.ContainsFocus)
                 main.Explorer.Tree.OnEditDelete();
            else
            if  (main.Toolbox.ContainsFocus) 
                 main.Toolbox.OnEditDelete();
            else
            if  (main.PropertyWindowControl.ContainsFocus) 
            {
            }
            else RaiseMenuActivity(this, ArgsDelete); 
        }


        public void OnEditSelectAll(object sender, EventArgs e)
        { 
            if  (main.Output.ContainsFocus)
                 main.Output.SelectAll();
            else RaiseMenuActivity(this, ArgsSelectAll); 
        }

        public void OnEditGoTo(object sender, EventArgs e)
        { 
            main.Output.OnMenuGoToNode();       
        }


        /// <summary>Invoked prior to display of View menu</summary>
        public void OnPopViewMenu(object sender, EventArgs e)
        {
            #if(false)
            bool isAppTreeTabActive = main.ProjectExists 
                && main.CurrentViewType == MaxMain.ViewTypes.App 
                && main.CurrentTab      == main.CurrentViewName;
            bool isFunctionTabActive = !isAppTreeTabActive;
            MaxMenu.menuViewZoom.Enabled = MaxMenu.menuViewGrid.Enabled = isFunctionTabActive;
            #endif

            MaxMenu.menuViewGrid.Visible = MaxMenu.menuViewZoom.Visible =
            MaxMenu.menuViewTray.Visible = MaxMenu.menuViewSep1.Visible = 
            MaxMenu.menuViewSep2.Visible = mp.IsFunctionCanvas;

            if (mp.IsFunctionCanvas)
                MaxMenu.menuViewTray.Checked = mp.IsTrayShown;         
        }


        public void OnViewZoomIn(object sender, EventArgs e)
        { 
            SignalUserInput(MaxUserInputEventArgs.MaxEventTypes.Zoom, 1);         
        }


        public void OnViewZoomOut(object sender, EventArgs e)
        { 
            SignalUserInput(MaxUserInputEventArgs.MaxEventTypes.Zoom, -1);         
        }


        public void OnViewZoomNormal(object sender, EventArgs e)
        { 
            SignalUserInput(MaxUserInputEventArgs.MaxEventTypes.Zoom, 0);       
        }


        public void OnViewGrid(object sender, EventArgs e)
        {        
            MaxMenu.menuViewGrid.Checked = !MaxMenu.menuViewGrid.Checked;
            int arg = MaxMenu.menuViewGrid.Checked? 1: 0;

            SignalUserInput(MaxUserInputEventArgs.MaxEventTypes.Grid, arg);    
        }
 

        public void OnViewCanvas(object sender, EventArgs e)
        { 
            MaxMenu.menuViewCanvas.Checked = !MaxMenu.menuViewCanvas.Checked;

            if  (MaxMenu.menuViewCanvas.Checked)  
                 MaxMain.DockMgr.HideAllContents();
            else MaxMain.DockMgr.ShowAllContents();     
        } 


        public void OnViewTray(object sender, EventArgs e)
        { 
            if (!this.IsFunctionCanvasActive()) return;
            bool isShowing = !main.TrayShown;
            MaxMenu.menuViewTray.Checked = mp.IsTrayShown = isShowing;
            int arg = isShowing? 1: 0;

            SignalUserInput(MaxUserInputEventArgs.MaxEventTypes.Tray, arg);               
        } 
   
  
        public void OnViewProperties(object sender, EventArgs e)
        { 
            main.ShowPropertiesWindow(false); 
        }


        public void OnViewToolbox(object sender, EventArgs e)
        { 
            main.ShowToolboxWindow(false);
        }


        public void OnViewExplorer(object sender, EventArgs e)
        {  
            main.ShowExplorerWindow(false);
        }


        public void OnViewOutput(object sender, EventArgs e)
        { 
            main.ShowOutputWindow();  
        } 


        public void OnViewOverview(object sender, EventArgs e)
        { 
            // main.MaxToggleWindow(main.OverviewWindow);  
            main.ShowOverviewWindow();          
        }


        public void OnViewFocusCanvas(object sender, EventArgs e)
        { 
            if (main.ProjectExists) 
                SignalUserInput(MaxUserInputEventArgs.MaxEventTypes.FocusCanvas);           
        }


        public void OnViewFocusTray(object sender, EventArgs e)
        { 
            if (!this.IsFunctionCanvasActive()) return;
            if (!main.TrayShown) this.OnViewTray(null, null);
            SignalUserInput(MaxUserInputEventArgs.MaxEventTypes.FocusTray);           
        }


        public void OnViewFocusProperties(object sender, EventArgs e)
        { 
            main.ShowPropertiesWindow(true);
        }


        public void OnViewFocusExplorer(object sender, EventArgs e)
        { 
            main.ShowExplorerWindow(true);
        }


        public void OnViewFocusToolbox(object sender, EventArgs e)
        { 
            main.ShowToolboxWindow(true);
        }


        public void OnPopProjectMenu(MenuCommand x)
        {
            MaxMenu.menuFileAddScript.Visible = MaxMenu.menuFileAddResource.Visible = 
            MaxMenu.menuProjectSep1.Enabled   = MaxMenu.menuProjectProperties.Enabled 
            = main.ProjectExists;

            MaxMenu.menuProjectAddReference.Visible = 
            MaxMenu.menuProjectAddWebService.Visible = MaxMenu.menuProjectSep2.Visible
            = main.ProjectExists;

            MaxMenu.menuFileAddInstal.Enabled = main.ProjectExists && !main.InstallerPresent;

            MaxMenu.menuFileAddLocales.Enabled = main.ProjectExists && !main.LocalesPresent;
        }


        public void OnProjectProperties(object sender, EventArgs e)
        { 
            PmProxy.ShowProperties
               (Max.Manager.MaxProject.Instance,
                Max.Manager.MaxProject.Instance.PmObjectType); 

            main.ShowPropertiesWindow(false); 
        }


        /// <summary>Invoked on Add Reference menu selection</summary>
        public void OnProjectAddReference(object sender, EventArgs e)
        { 
            main.Explorer.OnProjectAddReference();
        }


        /// <summary>Invoked on Add Web Service menu selection</summary>
        public void OnProjectAddWebService(object sender, EventArgs e)
        { 
            Max.Framework.Dialog.WebService.MaxWebServiceWizard wizard = new
                Max.Framework.Dialog.WebService.MaxWebServiceWizard(main);

            wizard.ShowDialog();
        }


        /// <summary>Invoked prior to display of Build menu</summary>
        public void OnPopBuildMenu(object sender, EventArgs e)
        {
            MaxMenu.menuBuildBuildProject.Enabled = MaxMenu.menuBuildDeploy.Enabled       
                = main.ProjectExists && !MaxDebugger.Instance.Debugging;
        }


        /// <summary>Invoked prior to display of Debug menu</summary>
        public void OnPopDebugMenu(MenuCommand x)
        {     
            MaxDebugger debugger = MaxDebugger.Instance;

            MaxMenu.menuDebugSep1.Visible     = MaxMenu.menuDebugStart.Visible = 
            MaxMenu.menuDebugStop.Visible     = MaxMenu.menuDebugStepInto.Visible = 
            MaxMenu.menuDebugStepOver.Visible = MaxMenu.menuDebugToggleBkpt.Visible = 
            MaxMenu.menuDebugClearBkpts.Visible = // MaxMenu.menuDebugNewBkpt.Visible  = 
            MaxMenu.menuDebugDisableBkpts.Visible 
                = main.ProjectExists && debugger.CurrentFunctionCanvas != null;

            MaxMenu.menuDebugStart.Text = Const.menuDebugStart;

            if (main.ProjectExists) 
            {          
                if  (debugger.Debugging)
                {
                     if  (debugger.DebugBreak)
                          MaxMenu.menuDebugStart.Text = Const.menuDebugContinue; 
                     else MaxMenu.menuDebugStart.Visible = false;
                }

                MaxMenu.menuDebugToggleBkpt.Enabled = debugger.IsActionNodeSelected();

                MaxMenu.menuDebugClearBkpts.Visible = MaxMenu.menuDebugDisableBkpts.Visible
                    = debugger.BreakpointCount() > 0;

                MaxMenu.menuDebugStop.Visible = MaxMenu.menuDebugStepOver.Enabled 
                    = debugger.Debugging;

                MaxMenu.menuDebugStepInto.Enabled = debugger.IsBreakAtCallFunction();
            }
                                  
            bool streaming = debugger.ConsoleControl.Streaming;

            MaxMenu.menuDebugStartConsole.Visible =!streaming;
            MaxMenu.menuDebugStopConsole.Visible  = streaming;
        }


        /// <summary>Invoked prior to display of Debug/Windows menu</summary>
        public void OnPopDebugWindowsMenu(MenuCommand x)
        {
            MaxMenu.menuDebugWindowsWatch.Enabled = MaxMenu.menuDebugWindowsCalls.Enabled 
                = MaxDebugger.Instance.Debugging;   
        }


        /// <summary>Invoked on Build selected from Build menu</summary>
        /// <remarks>We are building from source files, rather than from raw
        /// graphs, so we no longer fire the build event in to Max. 
        /// We have left the code in place here to do so however, which is
        /// why we continue to handle the event args, intercepting the
        /// build args and redirecting the request back to main.</remarks>
        public void OnBuildBuildProject(object sender, EventArgs e)
        { 
            if (!main.ProjectExists) return;

            if(!MaxMain.autobuild)
                MaxPropertyWindow.Instance.CheckPropertyGrid();

            MaxUserInputEventArgs args = main.OnBuildProjectRequest();

            #if(false)
            if (args != null) RaiseUserInput(this, args);
            #else

            if  (args == null) return;

            switch(args.MaxEventType)
            {
               case MaxUserInputEventArgs.MaxEventTypes.SaveProject:
                    RaiseUserInput(this, args);
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.Build:
                    MaxMain.OnBuildProject();
                    break;
            }
       
            #endif
        }


        public void OnBuildDeployProject(object sender, EventArgs e)
        { 
            if (!main.ProjectExists) return;

            MaxUserInputEventArgs args = main.OnDeployProjectRequest();

            if (args == null) return;

            switch(args.MaxEventType)
            {
               case MaxUserInputEventArgs.MaxEventTypes.SaveProject:

                    RaiseUserInput(this, args);
                    args = main.OnDeployProjectRequest();
                
                    // Build failed. User can deploy if old .mca file is present, but they  
                    // should be aware they are not deploying the build they just attempted 
                    if (args.MaxEventType != MaxUserInputEventArgs.MaxEventTypes.Deploy) 
                    {
                        // Dialog to user indicating that proceeding will deploy an old .mca file.
                        // JIM TODO: please implement a dialog.    
                    }
                    else main.OnDeployProject();
               
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.Build:

                    MaxMain.OnBuildProject();

                    args = main.OnDeployProjectRequest();
                    // Build failed. User can deploy if old .mca file is present, but they  
                    // should be aware they are not deploying the build they just attempted 
                    if (args.MaxEventType != MaxUserInputEventArgs.MaxEventTypes.Deploy) 
                    {
                        // Same dialog from above.
                    }
                    else main.OnDeployProject();
               
                    break;

               case MaxUserInputEventArgs.MaxEventTypes.Deploy:
                    main.OnDeployProject();
                    break;
            }
        }


        /// <summary>Invoked prior to display of Tools menu</summary>
        public void OnPopToolsMenu(object sender, EventArgs e)
        {     
            MaxMenu.menuToolsAddTab.Enabled = MaxMenu.menuToolsAddRemoveItems.Enabled 
                = main.ProjectExists;
        }


        public void OnToolsAddToolboxTab(object sender, EventArgs e)
        { 
            main.ShowToolboxWindow(false);
            main.Toolbox.AddToolboxTab();
        }


        public void OnToolsAddRemoveItems(object sender, EventArgs e)
        { 
            main.ShowToolboxWindow(false);
            main.Toolbox.menu.OnAddRemoveItems(null, null); 
        }


        public void OnToolsTest(object sender, EventArgs e)
        {   
            RaiseUserInput(this, new MaxUserInputEventArgs
                (MaxUserInputEventArgs.MaxEventTypes.Test, null));    
        }


        public void OnToolsOptions(object sender, EventArgs e)   
        { 
            immediateChanges.SetBefore(); 

            new Max.Framework.ToolsOptions.MaxOptionsDlg().ShowDialog();

            this.AfterToolsOptions();
        }


        /// <summary>Invoked after display of tools/options dialog</summary>
        private void AfterToolsOptions()   
        {
            if (immediateChanges.waitMotionChanged) 
                RaiseMenuActivity(this, new MaxMenuEventArgs
                    (MaxMenuEventArgs.MaxEventTypes.OptWaitMotion, Config.WaitForPortMotion)); 

            if (immediateChanges.largePortsChanged) 
                RaiseMenuActivity(this, new MaxMenuEventArgs
                    (MaxMenuEventArgs.MaxEventTypes.OptLargePorts, Config.LargePorts));  

            if (immediateChanges.visiblePortsChanged)  
                RaiseMenuActivity(this, new MaxMenuEventArgs
                    (MaxMenuEventArgs.MaxEventTypes.OptVisiblePorts, Config.VisiblePorts));
        }


        /// <summary>Invoked prior to display of Window menu</summary>
        public void OnPopWindowMenu(Crownwood.Magic.Menus.MenuCommand c)  
        {
            MaxMenu.menuWindow.MenuCommands.Clear();

            mm.CreateDynamicWindowMenu();
        }


        /// <summary>Invoked after display of Window menu</summary>
        public void OnDismissWindowMenu(Crownwood.Magic.Menus.MenuCommand c)
        {
            MaxMenu.menuWindow.MenuCommands.Clear();

            // Re-insert placeholder item so that next pop event knows to fire
            MaxMenu.menuWindow.MenuCommands.Add(MaxMenu.menuWindowSep1);
        }


        /// <summary>Invoked when a tab name is selected from the Window menu</summary>
        public void OnWindowWindowSelected(object sender, EventArgs e)
        {
            string tabname = ((MenuCommand)sender).Text;

            main.SignalTabChangeRequest(tabname);
        }


        public void OnWindowCloseAll(object sender, EventArgs e)
        { 
            main.CloseAllTabs(); 
        }


        public void OnWindowWindows(object sender, EventArgs e)
        { 
       
        }


        public void OnHelpAbout(object sender, EventArgs e)
        { 
            MessageBox.Show(Const.AboutMsg, "About " + Const.productName);     
        }


        /// <summary>Docking satellite title bar context menu selection "Dockable"</summary>
        public void OnDockingContextDockable(object sender, EventArgs e)
        { 
       
        }


        /// <summary>Docking satellite title bar context menu selection "Hide"</summary>
        public void OnDockingContextHide(object sender, EventArgs e)
        { 
            MenuCommand mc = sender as MenuCommand; if (mc == null) return;
            Content c = mc.Tag as Content; if (c == null) return;
            main.MaxToggleWindow(c);
        }


        /// <summary>Docking satellite title bar context menu selection "Floating"</summary>
        public void OnDockingContextFloating(object sender, EventArgs e)
        { 

        }


        /// <summary>Docking satellite title bar context menu selection "Auto Hide"</summary>
        public void OnDockingContextAutoHide(object sender, EventArgs e)
        { 
            MenuCommand mc = sender as MenuCommand; if (mc == null) return;
            Content c = mc.Tag as Content; if (c == null) return;
            MaxMain.DockMgr.ToggleContentAutoHide(c);
        }


        /// <summary>Determine if active tab exists and is a function</summary>
        protected bool IsFunctionCanvasActive()
        {
            // This can and should be simplified once we remove the framework to max async layer
            return main.ProjectExists && main.CurrentViewType == MaxMain.ViewTypes.App
                && main.CurrentViewName == MaxMain.View.CurrentAppName;
        }


        public class MenuPredicates
        {
            public bool CanUndo, CanRedo, CanDelete, CanCopy, CanPaste, 
                CanSelectAll, IsTrayShown, IsFunctionCanvas;
        }


        public class OptionsChanges    
        {
            public bool waitMotionBefore,largePortsBefore, visiblePortsBefore;

            public void SetBefore()
            {
                this.waitMotionBefore   = Config.WaitForPortMotion;
                this.largePortsBefore   = Config.LargePorts;
                this.visiblePortsBefore = Config.VisiblePorts;
            }

            public bool waitMotionChanged  { get { return waitMotionBefore != Config.WaitForPortMotion;} }
            public bool largePortsChanged  { get { return largePortsBefore   != Config.LargePorts;  } }
            public bool visiblePortsChanged{ get { return visiblePortsBefore != Config.VisiblePorts;} }
        }

        private MenuPredicates mp = new MenuPredicates();
        private OptionsChanges immediateChanges = new OptionsChanges();
        public void SetMenuPredicates(MenuPredicates mp) { this.mp = mp; }

        #region static event args
        private static MaxMenuEventArgs ArgsUndo  = new MaxMenuEventArgs(MaxMenuEventArgs.MaxEventTypes.EditUndo);
        private static MaxMenuEventArgs ArgsRedo  = new MaxMenuEventArgs(MaxMenuEventArgs.MaxEventTypes.EditRedo);
        private static MaxMenuEventArgs ArgsCut   = new MaxMenuEventArgs(MaxMenuEventArgs.MaxEventTypes.EditCut);
        private static MaxMenuEventArgs ArgsCopy  = new MaxMenuEventArgs(MaxMenuEventArgs.MaxEventTypes.EditCopy);
        private static MaxMenuEventArgs ArgsPaste = new MaxMenuEventArgs(MaxMenuEventArgs.MaxEventTypes.EditPaste);
        private static MaxMenuEventArgs ArgsDelete= new MaxMenuEventArgs(MaxMenuEventArgs.MaxEventTypes.EditDelete);
        private static MaxMenuEventArgs ArgsSelectAll = new MaxMenuEventArgs(MaxMenuEventArgs.MaxEventTypes.EditSelectAll);
        #endregion

    } // class MaxMenuHandlers
}   // namespace
