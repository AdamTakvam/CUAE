using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Metreos.Max.Manager; 
using Metreos.Max.GlobalEvents; 
using Metreos.Max.Core.Package;
using Metreos.Max.Resources.XML;
using Metreos.Max.Drawing;


namespace Metreos.Max.Framework
{
    /// <summary>MaxMain input dialog presentation and handling of dialog input</summary>
    public class MaxMainDialog
    {
        private MaxMain     main;
        private MaxMainUtil thisx;
        public  enum SaveActions { None, Cancel, Save, Nosave, Exit }


        public MaxMainDialog(MaxMain main)
        {
            this.main  = main; 
            this.thisx = main.MainX;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Project 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Prompt for project name and directory</summary>
        /// <remarks>A new folder is created for the project, in the specified
        /// directory, named the same as the project file, ala Visual Studio</remarks>
        public MaxUserInputEventArgs PromptNewProject()
        {     
            MaxNewFileDlg dlgNewProject 
                = new MaxNewFileDlg(MaxNewFileDlg.Options.FixupRecentFilePath);

            if (DialogResult.OK != dlgNewProject.ShowDialog()) return null;

            string name = Utl.StripFileExtension(dlgNewProject.ProjectName);
            string path = dlgNewProject.ProjectFolder + Const.bslash + name;

            if (!thisx.ValidateProjectDirectory(name, path, dlgNewProject.Text)) 
                return null;

            // If a project is open, we request a close of the project, continuing 
            // with the new project once the reply is received from the close request. 
            // If a project does not currently exist, we complete the new project now.

            MaxUserInputEventArgs args = main.ProjectExists?

                main.CloseImplicit(name, path, true):
            
                new MaxUserInputEventArgs
                   (MaxUserInputEventArgs.MaxEventTypes.NewProject, name, path);

            return args;
        }


        /// <summary>Invoked prior open project round trip</summary>
        public MaxUserInputEventArgs PromptOpenProject()     
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title       = Const.ProjectOpenDlgTitle;
            dlg.DefaultExt  = Const.maxProjectFileExtension;
            dlg.FileName    = Const.emptystr;
            dlg.Filter      = Const.MaxProjectFilter;
            dlg.FilterIndex = 0 ;
            dlg.RestoreDirectory = true ;

            if (dlg.ShowDialog() != DialogResult.OK) return null;      
 
            return InitiateOpenProjectSequence(dlg.FileName);
        }


        /// <summary>Invoked prior open project round trip, after project name in hand</summary>
        public MaxUserInputEventArgs InitiateOpenProjectSequence(string path) 
        {
            // If a project is open, we request a close of the project, continuing  
            // with the project open once the reply is received from the close request. 
            // If a project does not currently exist, we finish opening the project now.

            if (path == null || !File.Exists(path)) return null;
            MaxUserInputEventArgs args = null;

            if  (main.ProjectExists) 
                 args = main.CloseImplicit(Path.GetFileNameWithoutExtension(path), path, false);
            else
            {    main.BeforeOpenProject(path);

                 args = new MaxUserInputEventArgs
                     (MaxUserInputEventArgs.MaxEventTypes.OpenProject, path);
            }
  
            return args;
        }


        /// <summary>Prompt for project name and directory</summary>
        public MaxUserInputEventArgs PromptSaveProject()      
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title       = Const.ProjectSaveDlgTitle;
            dlg.DefaultExt  = Const.maxProjectFileExtension;
            dlg.FileName    = MaxMain.ProjectFilename != null? 
                MaxMain.ProjectFilename: Const.defaultProjectName; 
            dlg.Filter      = Const.MaxProjectFilter;
            dlg.FilterIndex = 0 ;
            dlg.RestoreDirectory = true;
            dlg.InitialDirectory = MaxMain.ProjectPath;

            if (dlg.ShowDialog() != DialogResult.OK) return null;   

            #region ifDemoProject
            #if(false)
            // Ensure we do not clobber whatever project we are using to demo Max
            if (-1 != dlg.FileName.ToLower().IndexOf(Const.MetreosDemoProjectFileName.ToLower()))
            {
                MessageBox.Show("Cannot overwrite demo project", Const.ProjectSaveDlgTitle);  
                return null;
            } 
            #endif
            #endregion

            main.RequestPersistIDE(true);

            return new MaxUserInputEventArgs
                (MaxUserInputEventArgs.MaxEventTypes.SaveProject, dlg.FileName);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Application script 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Pop New Application dialog, returning app and trigger names</summary>
        public bool PromptNewApplication()
        {
            // We formerly serialized triggers to project file in order to avoid the
            // async framework/max boundary. Now that we're integrated, we can simply
            // search the installed packages for triggers. 
            string[] triggers = MaxPackages.Instance.GetTriggers(); // thisx.GetTriggersList();
            if (triggers == null || triggers.Length == 0) return Utl.ShowNoTriggersDlg();

            string defaultname = this.GetUniqueDefaultAppName(MaxMain.ProjectFolder);
                                             
            MaxNewAppDlg newAppDlg = new MaxNewAppDlg
                (defaultname, MaxProject.ProjectFolder, triggers);

            if (DialogResult.OK != newAppDlg.ShowDialog()) return false;

            MaxMain.prompted.AppName = newAppDlg.AppName;
            MaxMain.prompted.Trigger = newAppDlg.Trigger;

            return true;
        }


        /// <summary>Pop Add Existing Application dialog, returning app name</summary>
        public string PromptAddExistingApplication()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title       = Const.AppAddExistDlgTitle;
            dlg.DefaultExt  = Const.maxScriptFileExtension;
            dlg.FileName    = null;
            dlg.Filter      = Const.MaxScriptFilter;
            dlg.FilterIndex = 0;
            dlg.InitialDirectory = MaxMain.ProjectFolder;
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog()!= DialogResult.OK) return null;

            MaxMain.prompted.AppName = dlg.FileName;

            return dlg.FileName;                
        }

                           
        /// <summary>Actions on new script menu selection</summary>    
        public MaxUserInputEventArgs OnNewScriptRequest()      
        {
            MaxUserInputEventArgs args = null;

            switch(this.GetCurrentViewDisposalAction())
            {
               case SaveActions.None:
                    // There was no existing app to save or close, so we
                    // fire off a new script request immediately

                    args = new MaxUserInputEventArgs
                        (MaxUserInputEventArgs.MaxEventTypes.AddNewScript, 
                         MaxMain.prompted.AppName, MaxMain.prompted.Trigger);
                    break;

               case SaveActions.Save:
                    // User asked to save current script prior to adding a new
                    // script, so we'll fire off a request to max to do a save. When
                    // we get the return event from the save, we'll continue with 
                    // the New Script request.

                    main.AsyncState = MaxMain.AsyncStates.SavingPriorNewApp; 
                    
                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.SaveProject, null);
                    break;

               case SaveActions.Nosave:
                    // User did not want to save current script prior to adding
                    // a new script, so we'll fire off a request to max to close
                    // the script. When we get the return event from the close, 
                    // we'll continue with the New Script request.

                    main.AsyncState = MaxMain.AsyncStates.ClosingPriorNewApp;

                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.CloseFile, null);
                    break;                               
                             
               case SaveActions.Cancel:
                    // User canceled the New Script request
                    break;
            }

            return args;
        }

                            
        /// <summary>Actions on existing script menu selection</summary>    
        public MaxUserInputEventArgs OnExistingScriptRequest(string appfilePath, string trigger)      
        {
            if  (appfilePath == null) return null;
            MaxUserInputEventArgs args = null;

            switch(this.GetCurrentViewDisposalAction())
            {
               case SaveActions.None:
                    // There was no existing app to save or close, so we
                    // fire off a max add existing script request immediately

                    args = new MaxUserInputEventArgs
                       (MaxUserInputEventArgs.MaxEventTypes.AddExistingScript, 
                        appfilePath, trigger);
                    break;

               case SaveActions.Save:
                    // User asked to save current script prior to adding another
                    // script, so we'll fire off a request to max to do a save. When
                    // When we get the return event from the save, we'll continue  
                    // with the Add Existing Script request.

                    main.AsyncState = MaxMain.AsyncStates.SavingPriorAddExistingApp; 
                     
                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.SaveProject, null);
                    break;

               case SaveActions.Nosave:
                    // User did not want to save current script prior to adding 
                    // another script, so we'll fire off a request to max to close
                    // the script. When we get the return event from the close, 
                    // we'll continue with the Add Existing Script request.

                    main.AsyncState = MaxMain.AsyncStates.ClosingPriorAddExistingApp;
   
                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.CloseFile, null);
                    break;                               
                              
               case SaveActions.Cancel:
                    // User canceled the Add Existing Script request
                    break;
            }

            return args;
        }


        /// <summary>App open request from explorer context menu</summary>
        public void OnOpenScriptRequest(string appname, string canvasname)
        {
            MaxUserInputEventArgs args = null;           

            switch(this.GetCurrentViewDisposalAction())
            {
               case SaveActions.None:
                    // There was no existing app to save or close, so we
                    // fire off an open script request immediately

                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.OpenScript, appname, canvasname);
                    break;

               case SaveActions.Save:
                    // User asked to save existing script prior to opening another
                    // script, so we'll fire off a request to max to do a save. When
                    // we get the return event from the save, we'll continue with 
                    // the Open Script request.

                    main.AsyncState = MaxMain.AsyncStates.SavingPriorOpenApp;  
                    MaxMain.prompted.CanvasName = canvasname;

                    MaxMain.prompted.AppName = appname;  
                  
                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.SaveProject, null);
                    break;

               case SaveActions.Nosave:

                    MaxMain.prompted.AppName = appname; 
                    MaxMain.prompted.CanvasName = canvasname;

                    if (main.CurrentViewType == MaxMain.ViewTypes.None)
                    {
                        // There was no tab currently open, so nothing to close
                        main.ContinueOpenApp();
                    }
                    else
                    {
                        // User did not want to save existing script prior to opening
                        // a new script, so we'll fire off a request to max to close
                        // the script. When we get the return event from the close, 
                        // we'll continue with the Open Script request.

                        main.AsyncState = MaxMain.AsyncStates.ClosingPriorOpenApp;
                         
                        args = new MaxUserInputEventArgs
                                (MaxUserInputEventArgs.MaxEventTypes.CloseFile, null);
                    }
                    break;                               
                             
               case SaveActions.Cancel:
                    // User canceled the Open Script request
                    break;
            }

            if (args != null) main.SignalUserInput(args);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Installer script 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>There is no new installer prompt, but keeps pattern intact</summary>
        public bool PromptAddNewInstaller()
        {
            MaxMain.prompted.InstallerName = Config.DefaultInstallerName;

            return true;
        }


        /// <summary>Pop Add Existing Application dialog, returning app name</summary>
        public string PromptAddExistingInstaller()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title       = Const.InstalAddExistDlgTitle;
            dlg.DefaultExt  = Const.maxInstallerFileExtension;
            dlg.FileName    = null;
            dlg.Filter      = Const.MaxInstallerFilter;
            dlg.FilterIndex = 0;
            dlg.InitialDirectory = Utl.GetInstallerFileFolder(MaxMain.ProjectPath);
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog()!= DialogResult.OK) return null;

            MaxMain.prompted.InstallerPath = dlg.FileName;
            MaxMain.prompted.InstallerName = Path.GetFileNameWithoutExtension(dlg.FileName);

            return dlg.FileName;  
        }


        /// <summary>Actions on new installer menu selection</summary>    
        public MaxUserInputEventArgs OnNewInstallerRequest()      
        {
            MaxUserInputEventArgs args = null;

            switch(this.GetCurrentViewDisposalAction())
            {
               case SaveActions.None:
                    args = new MaxUserInputEventArgs
                        (MaxUserInputEventArgs.MaxEventTypes.AddNewInstaller,  
                         MaxMain.prompted.InstallerName);
                    break;

               case SaveActions.Save:             
                    main.AsyncState = MaxMain.AsyncStates.SavingPriorAddNewInstaller; 
                    
                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.SaveProject, null);
                    break;

               case SaveActions.Nosave:             
                    main.AsyncState = MaxMain.AsyncStates.ClosingPriorAddNewInstaller;

                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.CloseFile, null);
                    break;                               
                              
               case SaveActions.Cancel:
                    break;
            }

            return args;
        }


        /// <summary>Actions on existing installer menu selection</summary>    
        public MaxUserInputEventArgs OnExistingInstallerRequest(string installerFilePath)      
        {
            if (installerFilePath == null) return null;
            MaxUserInputEventArgs args = null;

            switch(this.GetCurrentViewDisposalAction())
            {
               case SaveActions.None:
                    args = new MaxUserInputEventArgs
                       (MaxUserInputEventArgs.MaxEventTypes.AddExistingInstaller, 
                        installerFilePath);
                    break;

               case SaveActions.Save:
                    main.AsyncState = MaxMain.AsyncStates.SavingPriorAddExistingInstaller; 
                      
                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.SaveProject, null);
                    break;

               case SaveActions.Nosave:
                    main.AsyncState = MaxMain.AsyncStates.ClosingPriorAddExistingInstaller;
    
                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.CloseFile, null);
                    break;                               
                              
               case SaveActions.Cancel:
                    break;
            }

            return args;
        }


        /// <summary>Installer open request from explorer context menu</summary>
        public void OnOpenInstallerRequest(string installername)
        {
            MaxUserInputEventArgs args = null;

            switch(this.GetCurrentViewDisposalAction())
            {
               case SaveActions.None:
                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.OpenInstaller, installername);
                    break;

               case SaveActions.Save:
                    main.AsyncState = MaxMain.AsyncStates.SavingPriorOpenInstaller;  
                    MaxMain.prompted.InstallerName = installername;  
                  
                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.SaveProject, null);
                    break;

               case SaveActions.Nosave:
                    main.AsyncState = MaxMain.AsyncStates.ClosingPriorOpenInstaller;
                    MaxMain.prompted.InstallerName = installername; 
                      
                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.CloseFile, null);
                    break;                               
                              
               case SaveActions.Cancel:
                    break;
            }

            if (args != null) main.SignalUserInput(args);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Locales script 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>There is no new locales prompt, but keeps pattern intact</summary>
        public bool PromptAddNewLocales()
        {
            MaxMain.prompted.LocalesName = Config.DefaultLocalesName;

            return true;
        }


        /// <summary>Pop Add Existing Application dialog, returning app name</summary>
        public string PromptAddExistingLocales()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = Const.LocalesAddExistDlgTitle;
            dlg.DefaultExt = Const.maxLocalesFileExtension;
            dlg.FileName = null;
            dlg.Filter = Const.MaxLocalesFilter;
            dlg.FilterIndex = 0;
            dlg.InitialDirectory = Utl.GetLocalesFileFolder(MaxMain.ProjectPath);
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() != DialogResult.OK) return null;

            MaxMain.prompted.LocalesPath = dlg.FileName;
            MaxMain.prompted.LocalesName = Path.GetFileNameWithoutExtension(dlg.FileName);

            return dlg.FileName;
        }


        /// <summary>Actions on new locales menu selection</summary>    
        public MaxUserInputEventArgs OnNewLocalesRequest()
        {
            MaxUserInputEventArgs args = null;

            switch (this.GetCurrentViewDisposalAction())
            {
                case SaveActions.None:
                    args = new MaxUserInputEventArgs
                        (MaxUserInputEventArgs.MaxEventTypes.AddNewLocales,
                         MaxMain.prompted.LocalesName);
                    break;

                case SaveActions.Save:
                    main.AsyncState = MaxMain.AsyncStates.SavingPriorAddNewLocales;

                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.SaveProject, null);
                    break;

                case SaveActions.Nosave:
                    main.AsyncState = MaxMain.AsyncStates.ClosingPriorAddNewLocales;

                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.CloseFile, null);
                    break;

                case SaveActions.Cancel:
                    break;
            }

            return args;
        }


        /// <summary>Actions on existing locales menu selection</summary>    
        public MaxUserInputEventArgs OnExistingLocalesRequest(string localesFilePath)
        {
            if (localesFilePath == null) return null;
            MaxUserInputEventArgs args = null;

            switch (this.GetCurrentViewDisposalAction())
            {
                case SaveActions.None:
                    args = new MaxUserInputEventArgs
                       (MaxUserInputEventArgs.MaxEventTypes.AddExistingLocales,
                        localesFilePath);
                    break;

                case SaveActions.Save:
                    main.AsyncState = MaxMain.AsyncStates.SavingPriorAddExistingLocales;

                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.SaveProject, null);
                    break;

                case SaveActions.Nosave:
                    main.AsyncState = MaxMain.AsyncStates.ClosingPriorAddExistingLocales;

                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.CloseFile, null);
                    break;

                case SaveActions.Cancel:
                    break;
            }

            return args;
        }


        /// <summary>Locales open request from explorer context menu</summary>
        public void OnOpenLocalesRequest(string localesname)
        {
            MaxUserInputEventArgs args = null;

            switch (this.GetCurrentViewDisposalAction())
            {
                case SaveActions.None:
                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.OpenLocales, localesname);
                    break;

                case SaveActions.Save:
                    main.AsyncState = MaxMain.AsyncStates.SavingPriorOpenLocales;
                    MaxMain.prompted.LocalesName = localesname;

                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.SaveProject, null);
                    break;

                case SaveActions.Nosave:
                    main.AsyncState = MaxMain.AsyncStates.ClosingPriorOpenLocales;
                    MaxMain.prompted.LocalesName = localesname;

                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.CloseFile, null);
                    break;

                case SaveActions.Cancel:
                    break;
            }

            if (args != null) main.SignalUserInput(args);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Database script 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Pop New Application dialog, returning app and trigger names</summary>
        public bool PromptAddNewDatabase()
        { 
            string defaultname = this.GetUniqueDefaultDatabaseName(MaxMain.ProjectPath);
                                         
            while(true)
            {
                MaxLocalFileDlg dlg = new MaxLocalFileDlg(MaxMain.ProjectFolder, defaultname, 
                    Const.maxDatabaseFileExtension, Const.DatabaseScriptDescr, 
                    Const.DatabaseScriptPrompt, Const.NewDatabaseDlgTitle, true); 

                if (DialogResult.OK != dlg.ShowDialog()) return false;

                // Validate that file isn't in use.
                if( !File.Exists(Utl.GetDatabaseFilePath(MaxMain.ProjectPath, dlg.FileName)) &&
                    !MaxProject.Instance.Databases.Contains(dlg.FileName)) 
                {
                    MaxMain.prompted.DatabaseName = dlg.FileName;
                    return true;
                }
            }
        }


        /// <summary>Pop Add Existing Application dialog, returning app name</summary>
        public string PromptAddExistingDatabase()
        {
            while(true)
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Title       = Const.DbaseAddExistDlgTitle;
                dlg.DefaultExt  = Const.maxDatabaseFileExtension;
                dlg.FileName    = null;
                dlg.Filter      = Const.MaxDatabaseFilter;
                dlg.FilterIndex = 0;
                dlg.InitialDirectory = MaxMain.ProjectFolder;
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog()!= DialogResult.OK) return null; // User escape

                string dbname = Path.GetFileNameWithoutExtension(dlg.FileName);

                if (MaxProject.Instance.Databases.Contains(dbname))
                {            
                    // Inform user of duplicate filename and repop file browser.
                    MessageBox.Show(
                        Manager.MaxManager.Instance, 
                        Const.DuplicateDatabaseMessage(dbname),
                        Const.DuplicateDatabaseTitle);
                    continue; // Can't point to a database of same name (same file name without ext)  
                }

                MaxMain.prompted.DatabasePath = dlg.FileName;
                MaxMain.prompted.DatabaseName = dbname;

                return dlg.FileName;

                // MSC: The following line of code can not ever occur anymore, with the above check.
                // Explanation:  This line of code used to ask for overwrite.  
                // However, because we allow the addition of existing databases that stay in the directory 
                // that it was already in, it doesn't make sense to ask for overwrite.  
                // This used to make sense when all SQL scripts were copied to the max project directory
                
                // if (main.MainX.DuplicateFileHandled(dlg.FileName, Path.GetFileName(dlg.FileName))) 
                //     return null;
            }

        }


        /// <summary>Actions on new database menu selection</summary>    
        public MaxUserInputEventArgs OnNewDatabaseRequest()      
        {
            MaxUserInputEventArgs args = null;

            switch(this.GetCurrentViewDisposalAction())
            {
               case SaveActions.None:
                    args = new MaxUserInputEventArgs
                        (MaxUserInputEventArgs.MaxEventTypes.AddNewDatabase,  
                         MaxMain.prompted.DatabaseName);
                    break;

               case SaveActions.Save:             
                    main.AsyncState = MaxMain.AsyncStates.SavingPriorAddNewDatabase; 
                    
                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.SaveProject, null);
                    break;

               case SaveActions.Nosave:             
                    main.AsyncState = MaxMain.AsyncStates.ClosingPriorAddNewDatabase;

                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.CloseFile, null);
                    break;                               
                              
               case SaveActions.Cancel:
                    break;
            }

            return args;
        }



        /// <summary>Actions on existing database menu selection</summary>    
        public MaxUserInputEventArgs OnExistingDatabaseRequest(string databaseFilePath)      
        {
            if (databaseFilePath == null) return null;
            MaxUserInputEventArgs args = null;

            switch(this.GetCurrentViewDisposalAction())
            {
               case SaveActions.None:
                    args = new MaxUserInputEventArgs
                        (MaxUserInputEventArgs.MaxEventTypes.AddExistingDatabase, 
                         databaseFilePath);
                    break;

               case SaveActions.Save:
                    main.AsyncState = MaxMain.AsyncStates.SavingPriorAddExistingDatabase; 
                      
                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.SaveProject, null);
                    break;

               case SaveActions.Nosave:
                    main.AsyncState = MaxMain.AsyncStates.ClosingPriorAddExistingDatabase;
    
                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.CloseFile, null);
                    break;                               
                              
               case SaveActions.Cancel:
                    break;
            }

            return args;
        }


        /// <summary>Database open request from explorer context menu</summary>
        public void OnOpenDatabaseRequest(string databasename)
        {
            MaxUserInputEventArgs args = null;

            switch(this.GetCurrentViewDisposalAction())
            {
               case SaveActions.None:
                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.OpenDatabase, databasename);
                    break;

               case SaveActions.Save:
                    main.AsyncState = MaxMain.AsyncStates.SavingPriorOpenDatabase;  
                    MaxMain.prompted.DatabaseName = databasename;  
                  
                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.SaveProject, null);
                    break;

               case SaveActions.Nosave:
                    main.AsyncState = MaxMain.AsyncStates.ClosingPriorOpenDatabase;
                    MaxMain.prompted.DatabaseName = databasename; 
                      
                    args = new MaxUserInputEventArgs
                              (MaxUserInputEventArgs.MaxEventTypes.CloseFile, null);
                    break;                               
                              
               case SaveActions.Cancel:
                    break;
            }

            if  (args != null) main.SignalUserInput(args);
        }


        /// <summary>Pop Add Existing Media File dialog, returning media file path</summary>
        public bool PromptAddAudioFile(CultureInfo forceCulture, out string selectedMediaFilePath, out CultureInfo selectedCulture)
        {
            selectedMediaFilePath = null;
            selectedCulture = null;
            bool choiceMade = false;

            MediaFileLocaleDlg dlg = new MediaFileLocaleDlg();
            dlg.ForceCulture = forceCulture;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                choiceMade = true;
                selectedCulture = dlg.SelectedCulture;
                selectedMediaFilePath = dlg.SelectedPath;
                MaxMain.prompted.AudioFilePath = dlg.SelectedPath;
                MaxMain.prompted.AudioFileLocale = dlg.SelectedCulture;
            }

            return choiceMade;        
        }


        /// <summary>Pop Add existing voice recognition resource dialog, returning file path</summary>
        public string PromptAddVoiceRecResource()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title       = Const.AddVrResourceDlgTitle;
            dlg.DefaultExt  = null;
            dlg.FileName    = null;
            dlg.Filter      = Const.MaxVrResourceFilter;
            dlg.FilterIndex = 0;
            dlg.InitialDirectory = MaxMain.ProjectFolder;
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() != DialogResult.OK) return null;

            MaxMain.prompted.VrResxFilePath = dlg.FileName;

            return dlg.FileName;                
        }


        /// <summary>Voice rec resource open request from explorer context menu</summary>
        public void OnOpenVrResxRequest(string filename)
        {
            MaxUserInputEventArgs args = null;

            switch(this.GetCurrentViewDisposalAction())
            {
                case SaveActions.None:
                     args = new MaxUserInputEventArgs
                           (MaxUserInputEventArgs.MaxEventTypes.OpenVrResx, filename);
                     break;

                case SaveActions.Save:
                     main.AsyncState = MaxMain.AsyncStates.SavingPriorOpenVrResx;  
                     MaxMain.prompted.VrResxName = filename;  
                  
                     args = new MaxUserInputEventArgs
                           (MaxUserInputEventArgs.MaxEventTypes.SaveProject, null);
                     break;

                case SaveActions.Nosave:
                     main.AsyncState = MaxMain.AsyncStates.ClosingPriorOpenVrResx;
                     MaxMain.prompted.VrResxName = filename; 
                      
                     args = new MaxUserInputEventArgs
                           (MaxUserInputEventArgs.MaxEventTypes.CloseFile, null);
                     break;                               
                              
                case SaveActions.Cancel:
                     break;
            }

            if  (args != null) main.SignalUserInput(args);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Reference 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Pop Add Reference dialog, returning library path</summary>
        public string PromptAddReference()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title       = Const.AddReferenceDlgTitle;
            dlg.DefaultExt  = null;
            dlg.FileName    = null;
            dlg.Filter      = Const.MaxReferenceFilter;
            dlg.FilterIndex = 0;
            dlg.InitialDirectory = MaxMain.ProjectFolder;
            dlg.RestoreDirectory = true;

            return dlg.ShowDialog() == DialogResult.OK? dlg.FileName: null;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Support 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Determine how user wishes to dispose of current view,
        ///  if any, prior to opening a new view</summary>
        public SaveActions GetCurrentViewDisposalAction()
        {
            // This happens when VR resx is dirty, but we're rewriting
            if (main.CurrentViewName == null) return SaveActions.Nosave; 
            SaveActions action = SaveActions.None;

            if (MaxMain.View.Dirty)
            {
                switch(Utl.PromptSaveChangesTo(main.CurrentViewName, Const.AppSaveDlgTitle))
                {
                    case DialogResult.Yes:    action = SaveActions.Save;   break;
                    case DialogResult.No:     action = SaveActions.Nosave; break;
                    case DialogResult.Cancel: action = SaveActions.Cancel; break;
                }
            }
            else                               
            if  (main.CurrentViewType == MaxMain.ViewTypes.None 
              || main.CurrentViewName == MaxMain.ProjectName)
                // Guards against anomalous situation where a view was not opened
                // for whatever reason. This makes it possible to open another view.
                 action = SaveActions.None;
            else
            if  (main.CurrentViewName != null)
                 action = SaveActions.Nosave;

            return action;
        }


        /// <summary>Get a unique db name of the form "database1"</summary>
        private string GetUniqueDefaultDatabaseName(string path)
        {
            return Const.defaultDatabaseName + Utl.GetUniqueFilenameSequencer
                  (Utl.GetDbDirectoryPath(path), Const.defaultDatabaseName, Const.maxDatabaseFileExtension, 0);
        }


        /// <summary>Get a unique app name of the form "script1"</summary>
        private string GetUniqueDefaultAppName(string path)
        {
            return Const.defaultAppName + Utl.GetUniqueFilenameSequencer
                  (path, Const.defaultAppName, Const.maxScriptFileExtension, 0);
        }

    } // class MaxMainDialog

}   // namespace
