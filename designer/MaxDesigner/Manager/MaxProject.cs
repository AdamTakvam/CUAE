using System;  
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Drawing.Printing;
using Metreos.Max.Core;
using Metreos.Max.Drawing;
using Metreos.Max.Debugging;
using Metreos.Max.Framework;
using Metreos.Max.GlobalEvents;
using Metreos.Max.Core.Package;
using Metreos.Max.Core.NativeType;
using Metreos.Max.Core.Tool;
using Metreos.Max.Framework.Satellite.Property;
using Metreos.Max.Framework.Satellite.Toolbox;
using Northwoods.Go;
using CollectionsUtil = System.Collections.Specialized.CollectionsUtil;



namespace Metreos.Max.Manager
{
    /// <summary>Encapsulates a Max project. Hosts apps. Handles serialization.</summary>
  
    public class MaxProject: MaxSelectableObject
    {    
        #region singleton
        private MaxProject() {}
        private static MaxProject instance;
        public  static MaxProject Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new MaxProject();
                    instance.thisx = new MaxProjectUtil();
                    instance.Init();
                    instance.Clear();
                }
                return instance;
        }
        }
        #endregion

        private void Init()
        {
            SuspendLayoutEventArgs  = new MaxFrameworkEventArgs(MaxFrameworkEventArgs.MaxEventTypes.SuspendLayout);
            ResumeLayoutEventArgs   = new MaxFrameworkEventArgs(MaxFrameworkEventArgs.MaxEventTypes.ResumeLayout);
            ProjectDirtyEventArgs   = new MaxProjectEventArgs(MaxProjectEventArgs.MaxEventTypes.Dirty);
            ProjectNotDirtyEventArgs= new MaxProjectEventArgs(MaxProjectEventArgs.MaxEventTypes.NotDirty);
            ViewDirtyEventArgs      = new MaxProjectEventArgs(MaxProjectEventArgs.MaxEventTypes.AppDirty);
            ViewNotDirtyEventArgs   = new MaxProjectEventArgs(MaxProjectEventArgs.MaxEventTypes.AppNotDirty);
            PropertiesEventArgs     = new MaxProjectEventArgs(MaxProjectEventArgs.MaxEventTypes.Properties);
            SuspendLayoutPriorSaveEventArgs = new MaxFrameworkEventArgs(MaxFrameworkEventArgs.MaxEventTypes.SuspendLayout);
            SuspendLayoutPriorSaveEventArgs.Result = MaxFrameworkEventArgs.MaxResults.Saving;

            view = new MaxProjectView(this);
            serializer = new MaxProjectSerializer(this);

            this.ideDirty = true;

            RaiseProjectActivity   += OutboundHandlers.ProjectActivityCallback;
            RaiseFrameworkActivity += OutboundHandlers.FrameworkActivityCallback;
            RaiseNodeActivity      += OutboundHandlers.NodeActivityProxy;
            RaiseCanvasActivity    += OutboundHandlers.CanvasActivityProxy;
            RaiseMenuActivity      += OutboundHandlers.MenuOutputProxy;

            pmObjectType = Framework.Satellite.Property.DataTypes.Type.Project;   

            configSequence = 0;
            customConfigItems = new ArrayList();
        }
 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Properties
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private static string projectPath;
        public  static string ProjectPath  { get { return projectPath;  } }
        private static string projectName;
        public  static string ProjectName  { get { return projectName;  } }
        private static string projectFolder;
        public  static string ProjectFolder{ get { return projectFolder;} }
        private static string projectXmlVer;

        public ArrayList CustomConfigItems { get { return customConfigItems; } set { customConfigItems = value; } }
        private ArrayList customConfigItems;

        public  int  ConfigSequence  { get { return ++configSequence; } }
        private int  configSequence;

        private static MaxProjectView view;
        public  static MaxProjectView View { get { return view; } } 

        private MaxProjectUtil thisx;
        public  MaxProjectUtil Util{ get { return thisx;} }

        private Hashtable apps = new Hashtable();
        public  Hashtable Apps     { get { return apps; } }

        // The databases table derives keys from the filename of the sql file for the database.
        // In keeping with Windows case insensitivity, we make a case insensitive hashtable
        private Hashtable databases = CollectionsUtil.CreateCaseInsensitiveHashtable();
        public  Hashtable Databases{ get { return databases;} } 

        private bool dirty;
        public  bool Dirty   { get { return dirty;   } set { dirty = value;    } }
        private bool ideDirty;
        public  bool IdeDirty{ get { return ideDirty;} set { ideDirty = value; } }

        private MaxProjectSerializer serializer;
        public  MaxProjectSerializer Serializer { get { return serializer; } }

        public  static InstallerInfo installer = new InstallerInfo();
        public  static InstallerInfo Installer  { get { return installer;  } }

        public static LocalesInfo localeInfo = new LocalesInfo();
        public static LocalesInfo LocaleInfo { get { return localeInfo; } }

        private static MaxManager manager = MaxManager.Instance;
        private PropertyDescriptorCollection properties;
        private Framework.Satellite.Property.DataTypes.Type pmObjectType;
                                       

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Events
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Fires project activity event into the global event layer</summary>
        public event GlobalEvents.MaxProjectActivityHandler   RaiseProjectActivity;
        public event GlobalEvents.MaxFrameworkActivityHandler RaiseFrameworkActivity;
        public event GlobalEvents.MaxCanvasActivityHandler    RaiseCanvasActivity;
        public event GlobalEvents.MaxNodeActivityHandler      RaiseNodeActivity;
        public event GlobalEvents.MaxMenuOutputHandler        RaiseMenuActivity;
        public void  SignalCanvasActivity (MaxCanvasEventArgs args)    { RaiseCanvasActivity (this, args);}
        public void  SignalProjectActivity(MaxProjectEventArgs args)   { RaiseProjectActivity(this, args);}
        public void  SignalMenuActivity   (MaxMenuOutputEventArgs args){ RaiseMenuActivity   (this, args);}

        public static MaxFrameworkEventArgs SuspendLayoutEventArgs;
        public static MaxFrameworkEventArgs SuspendLayoutPriorSaveEventArgs;
        public static MaxFrameworkEventArgs ResumeLayoutEventArgs;
        public static MaxProjectEventArgs ProjectDirtyEventArgs;
        public static MaxProjectEventArgs ProjectNotDirtyEventArgs;
        public static MaxProjectEventArgs ViewDirtyEventArgs;
        public static MaxProjectEventArgs ViewNotDirtyEventArgs;
        public static MaxProjectEventArgs PropertiesEventArgs;


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // File menu methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Create a new project</summary>
        public bool New(string projectName, string folderPath)
        {
            if  (!this.CanOpenNew()) return false;

            RaiseFrameworkActivity(this, SuspendLayoutEventArgs);

            this.SetPathInfo(projectName, folderPath); 
            this.CreateProperties(null);
            this.SaveIDE(true);

            RaiseProjectActivity(this, new MaxProjectEventArgs(
                MaxProjectEventArgs.MaxEventTypes.New,
                MaxProjectEventArgs.MaxResults.OK, ProjectPath)); 

            RaiseFrameworkActivity(this, ResumeLayoutEventArgs);

            PmProxy.ShowProperties(this, pmObjectType);  

            MaxDebugger.Instance.OnProjectOpen();  

            return true;
        }


        /// <summary>Open a project</summary>
        public bool Open(string projectPath)
        {
            if (!this.CanOpenNew()) return false;

            RaiseFrameworkActivity(this, SuspendLayoutEventArgs);

            MaxProjectEventArgs eventArgs = new MaxProjectEventArgs(                                             
                MaxProjectEventArgs.MaxEventTypes.Open, projectPath);

            bool result = thisx.Load(projectPath);   
           
            eventArgs.Result = result? 
                MaxProjectEventArgs.MaxResults.OK: MaxProjectEventArgs.MaxResults.Error;
          
            RaiseProjectActivity(this, eventArgs);

            RaiseFrameworkActivity(this, ResumeLayoutEventArgs);

            // It is important to mark the view not dirty as the last step of
            // project open, since during deserialization, closing of tabs behind
            // the scenes marks the view dirty.
            this.MarkViewNotDirty(true);  

            if (result) 
            {
                this.Util.SaveIdeLayout(false);   // Ensure an IDE file exists 

                if (this.properties == null)
                    this.CreateProperties(null);

                PmProxy.ShowProperties(this, pmObjectType);

                MaxDebugger.Instance.OnProjectOpen();
            }     

            return result;
        }


        /// <summary>Close project</summary>
        public bool Close()
        {
            return this.Close(0);
        }


        /// <summary>Close project</summary>
        public bool Close(int forceSave)
        {
            installer.Close(false); 
      
            MaxDebugger.Instance.Force();

            MaxProjectEventArgs eventArgs = new MaxProjectEventArgs(                                             
                MaxProjectEventArgs.MaxEventTypes.Close, ProjectPath);

            if (forceSave == 1) this.dirty = true;

            AppDisposeInfo info = this.DisposeProject();

            if (info.isCanceled) 
                eventArgs.Result = MaxProjectEventArgs.MaxResults.Error;  
      
            RaiseProjectActivity(this, eventArgs);

            return eventArgs.Result == MaxProjectEventArgs.MaxResults.OK;
        }


        /// <summary>Save project</summary>
        public bool SaveAs(string path)
        {
            thisx.Backup(3);
                   
            ConditionalRaiseFrameworkActivity(this, SuspendLayoutPriorSaveEventArgs);

            bool result = view.Dirty? view.Save(): true;

            MaxProjectEventArgs eventArgs = new MaxProjectEventArgs(                                             
                MaxProjectEventArgs.MaxEventTypes.SaveAs, 
                MaxProjectEventArgs.MaxResults.Error, path, null);

            if (result)  
            {
                projectPath = path;
                projectName = Utl.StripPathFolderPlusExtension(path);
                this.MarkNotDirty(false);

                eventArgs.SetOK(); 
            }  

            RaiseProjectActivity(this, eventArgs);

            ConditionalRaiseFrameworkActivity(this, ResumeLayoutEventArgs);
     
            return result;
        } 


        /// <summary>Save project</summary>
        public bool Save()
        {
            return this.SaveAs(ProjectPath);    
        } 


        /// <summary>Save tools and toolbox</summary>
        public void SaveIDE(bool force)
        {
            if  (this.ideDirty || force) this.Util.SaveIdeLayout(true); 
            this.ideDirty = false;    
        } 


        /// <summary>Load project from supplied XML reader</summary>
        public bool Load(XmlTextReader reader)
        {      
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(reader);
            XmlNode root = xdoc.DocumentElement;
            if (root.Name != Const.xmlEltProject) return false; 

            MaxProject.projectXmlVer = Utl.XmlAttr(root, Const.xmlAttrVersion);

            // Ensure not trying to load a file version we no longer support
            if (!Utl.ValidateProjectFileVersion(projectXmlVer)) return false;

            MaxProject.projectName = Utl.XmlAttr(root, Const.xmlAttrName);
            if (projectName == null) return false;

            string currentView = Utl.XmlAttr(root, Const.xmlAttrCurrent);
            string viewType    = Utl.XmlAttr(root, Const.xmlAttrType);      

            return serializer.Deserialize(root, currentView, viewType);    
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Build 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Compile the project graphs</summary>
        /// <remarks>It is doubtful we will require this inner build, since it is
        /// anticipated that the project will be built entirely from source xml</remarks>
        public void Build()
        {
            #if(false)
            // If we do build here ...
            string buildfilePath = Config.BuildFilesDirectory(ProjectFolder) 
                + Const.bslash + ProjectName + Const.maxBuildFileExtension;

            // ... we would do the build here to buildfilePath

            // ... and fire back a built event 
            RaiseProjectActivity(this, new MaxProjectEventArgs
                (MaxProjectEventArgs.MaxEventTypes.Build, 
                MaxProjectEventArgs.MaxResults.OK, buildfilePath));
            #endif    
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // General view activity 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Close current view prior to opening another</summary>
        public void OnCloseView()        
        {                                      
            switch(view.ViewType)
            {
                case ViewTypes.App:       OnCloseScript   (view.Name); break;
                case ViewTypes.Installer: OnCloseInstaller(view.Name); break;
                case ViewTypes.Locales:   OnCloseLocales  (view.Name); break;
                case ViewTypes.Database:  OnCloseDatabase (view.Name); break;
                case ViewTypes.VrResx:    OnCloseVoiceRecResource(view.Name); break;
                default:
                {    manager.ClearTabs();  // jld 0405 kludge but rewriting anyway
                     view.Clear();      
                     RaiseProjectActivity(this, new MaxProjectEventArgs
                         (MaxProjectEventArgs.MaxEventTypes.CloseScript, null, null));
                }
                break;
            }
        }


        public void MarkIfAbandoned(MaxProjectEventArgs args)
        {
            // If a file has never been saved in its lifetime, indicate to framework
            // that the reference should be removed from explorer, and the file deleted
            bool isAbandoned = view.ViewObject.IsNew && view.ViewObject.SaveCount == 0;
            if  (isAbandoned) 
                 args.Result = MaxProjectEventArgs.MaxResults.OkRemove;
        }
    

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // App script activity 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Add a new application script to current project</summary>
        public void OnAddNewScript(string name, string trigger)
        {
            if  (name == null || trigger == null) return;
                                  
            OnCloseView();              // Close currently open view if any 

            MaxApp app = this.CreateApp(name, null, trigger);  
     
            if  (app.New())  
            {
                apps.Add(name, app.AppfilePath);
                MarkViewDirty(); 
            }
            else view.Clear();
        }

                                  
        /// <summary>Add an existing application script to current project</summary>
        public void OnAddExistingScript(string appfilepath, string trigger)
        {
            string appname = Path.GetFileNameWithoutExtension(appfilepath);

            OnCloseView();          
    
            MaxApp app = this.CreateApp(appname, appfilepath, trigger);   
                                            // Add app root to explorer
            MaxProjectEventArgs args = new MaxProjectEventArgs   
                (MaxProjectEventArgs.MaxEventTypes.AddScript, appname, appfilepath);
            args.Active = true;

            RaiseProjectActivity(this, args);         

            if (app.Open(appfilepath, true))  
            {
                apps.Add(appname, app.AppfilePath);
                MarkViewDirty(); 
            }
            else this.OnCloseView();
        }


        /// <summary>Open a script</summary>
        public bool OnOpenScript(string appname, string canvasName)     
        {
            if (this.IsNameOfCurrentApp(appname)) return false; 
            string path = this.apps[appname] as string; if (path == null) return false;

            OnCloseView();

            MaxApp app = this.CreateApp(appname, path, null);

            bool result = app.Open(path, true);
            if (!result)  app.Close(false);   

            // It is important to mark the view not dirty as the last step of app
            // script open, both in framework and in max, or changes could be lost 
            this.MarkViewNotDirty(true);   
            return result;
        }


        /// <summary>Register an app file on project file open</summary>
        public bool OnProjectRegisterScript(string appname, string fullpath, bool active)     
        {
            if (appname == null || fullpath == null) return false;        
            if (this.apps.Contains(appname)) return false;

            this.apps.Add(appname, fullpath);     // Add to project app table
                                                  // Add to explorer
            MaxProjectEventArgs args = new MaxProjectEventArgs   
                (MaxProjectEventArgs.MaxEventTypes.AddScript, appname, fullpath);
            args.Active = active;

            RaiseProjectActivity(this, args);
            return true;
        }


        /// <summary>Initialize intial script on project file open</summary>
        public bool OnProjectOpenScript(string appname, string trigger)     
        {
            string path = this.apps[appname] as string; if (path == null) return false; 

            MaxApp app  = this.CreateApp(appname, path, null);

            bool result = null != app.OnOpening(path, trigger);

            if (result)     // Notify framework of the open script 
                RaiseProjectActivity(this, new MaxProjectEventArgs                                              
                    (MaxProjectEventArgs.MaxEventTypes.OpenScript, appname, path));

            MarkViewNotDirty(false);
                                                        
            return result;      
        }


        /// <summary>Instantiate a MaxApp object</summary>
        /// <remarks>We use this method to ensure the current view is set immediately</remarks>
        MaxApp CreateApp(string name, string path, string trigger)
        {
            MaxApp app = trigger == null? new MaxApp(name): new MaxApp(name, trigger);
            string appfilePath = path == null? app.AppfilePath: path;
            view.Set(ViewTypes.App, name, appfilePath, app);
            return app;
        }


        /// <summary>Close an application script</summary>
        public void OnCloseScript(string appname)
        {
            bool isCurrentApp = this.IsNameOfCurrentApp(appname);   
            if (!isCurrentApp) return;    
            string path = this.apps[appname] as string; if (path == null) return;

            MaxProjectEventArgs args = new MaxProjectEventArgs
                (MaxProjectEventArgs.MaxEventTypes.CloseScript, appname, currentApp.AppfilePath);

            this.MarkIfAbandoned(args);

            manager.ClearTabs();
            view.Clear();
      
            RaiseProjectActivity(this, args);
        }


        /// <summary>Remove an application script from current project</summary>
        public void OnRemoveScript(string appname)
        {
            // To do both here and in OnDeleteScript following -- don't fire explorer
            // events or delete files here -- notify framework as in OnCloseScript
            // above, and framework can do the explorer and file maintenance
            string path = this.apps[appname] as string; if (path == null) return;

            bool isCurrentApp = this.IsNameOfCurrentApp(appname);    

            this.apps.Remove(appname);            // Remove from app table
            // Remove from explorer
            RaiseProjectActivity(this, new MaxProjectEventArgs
                (MaxProjectEventArgs.MaxEventTypes.RemoveScript, appname, path));

            if (isCurrentApp) 
            { 
                manager.ClearTabs();
                view.Clear();
            }
        }


        /// <summary>Remove application script from project and delete script file</summary>
        public void OnDeleteScript(string appname)
        {
            string path = this.apps[appname] as string; if (path == null) return;

            this.OnRemoveScript(appname);

            Utl.SafeDelete(path);
        }


        /// <summary>Rename script including script source file</summary>
        public bool RenameScript(string oldname, string newname)
        {
            MaxApp app = apps[oldname] as MaxApp; 
            if  (app == null) return false;
            if  (!this.CanRenameApp(newname, oldname)) return false;

            bool result = app.Rename(oldname, newname);
            if  (!result) return false;

            apps.Remove(oldname);
            apps.Add(newname, app);
      
            return true;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Installer script activity 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Add new installer file and open editor</summary>
        public void OnAddNewInstaller(string path)
        {
            OnCloseView();

            string name     = Const.InstallerFileName;
            string fullpath = Utl.GetInstallerFilePath(ProjectPath);

            MaxInstallerEditorTab editor = installer.Create(name, fullpath, true);

            if  (editor.New(fullpath, false))        
                MarkViewDirty();      
            else view.Clear(); 
        }


        /// <summary>Add existing installer file and open editor</summary>
        public void OnAddExistingInstaller(string path)
        {
            OnCloseView(); 

            string name = Const.InstallerFileName;

            MaxInstallerEditorTab editor = installer.Create(name, path, false);  
       
            if  (editor.New(path, true))        
                MarkViewDirty();      
            else view.Clear();    
        }


        /// <summary>Reopen installer</summary>
        public bool OnOpenInstaller(string appname)     
        {
            // Installer is presumably already registered with project
            if (view.IsInstaller || installer.path == null) return false;    

            OnCloseView();

            MaxInstallerEditorTab editor = installer.Create(installer.name, installer.path, false);

            return editor.Open(installer.name, installer.path);   
        }


        /// <summary>Close the installer</summary> 
        /// <remarks>Invoked prior GoTo some other view</remarks>           .
        public void OnCloseInstaller(string installername)     
        {
            installer.Close(false);
      
            MaxProjectEventArgs args = new MaxProjectEventArgs
                (MaxProjectEventArgs.MaxEventTypes.CloseInstaller, installername, null);

            this.MarkIfAbandoned(args);                   

            manager.ClearTabs();
            view.Clear();
      
            RaiseProjectActivity(this, args);
        }


        /// <summary>Register an installer file on project file open</summary>
        public bool OnProjectRegisterInstaller(string name, string fullpath, bool active)     
        {
            if (name == null || fullpath == null) return false;        

            installer.Set(name, fullpath);
            // Add to explorer
            MaxProjectEventArgs args = new MaxProjectEventArgs   
                (MaxProjectEventArgs.MaxEventTypes.AddInstaller, name, fullpath);
            args.Active = active;

            RaiseProjectActivity(this, args);
            return true;
        }


        /// <summary>Initialize installer on project file open</summary>
        public MaxInstallerEditorTab OnProjectOpenInstaller()     
        {
            // Presumably OnProjectRegisterInstaller was invoked previously
            if (installer.path == null) return null; 

            MaxInstallerEditorTab editor = installer.Create(installer.name, installer.path, false);

            if (!editor.OnOpening(installer.path)) return null;
      
            installer.editor = editor;
                                            // Notify framework of the open installer
            RaiseProjectActivity(this, new MaxProjectEventArgs                                              
                (MaxProjectEventArgs.MaxEventTypes.OpenInstaller, installer.name, installer.path));
                                                        
            return editor;         
        }


        /// <summary>Remove installer from current project</summary>
        public void OnRemoveInstaller()
        {
            if (installer.path == null) return;
                                            // Remove from explorer
            RaiseProjectActivity(this, new MaxProjectEventArgs
                (MaxProjectEventArgs.MaxEventTypes.RemoveInstaller, installer.name, installer.path));

            installer.Clear();

            if (view.IsInstaller) 
            {   
                view.Clear();
                manager.ClearTabs();
            }
        }


        /// <summary>Remove installer from project and delete its file</summary>
        public void OnDeleteInstaller()
        {
            string path = installer.path; if (path == null) return;

            this.OnRemoveInstaller();

            Utl.SafeDelete(path);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Locales script activity 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Add new locales file and open editor</summary>
        public void OnAddNewLocales(string path)
        {
            OnCloseView();

            string name = Const.LocalesFileName;
            string fullpath = Utl.GetLocalesFilePath(ProjectPath);

            MaxLocalizationEditorTab editor = localeInfo.Create(name, fullpath, true);

            if (editor.New(fullpath, false))
                MarkViewDirty();
            else view.Clear();
        }


        /// <summary>Add existing locales file and open editor</summary>
        public void OnAddExistingLocales(string path)
        {
            OnCloseView();

            string name = Const.LocalesFileName;

            MaxLocalizationEditorTab editor = localeInfo.Create(name, path, false);

            if (editor.New(path, true))
                MarkViewDirty();
            else view.Clear();
        }


        /// <summary>Reopen locales</summary>
        public bool OnOpenLocales(string appname)
        {
            // Installer is presumably already registered with project
            if (view.IsLocales || localeInfo.path == null) return false;

            OnCloseView();

            MaxLocalizationEditorTab editor = localeInfo.Create(localeInfo.name, localeInfo.path, false);

            return editor.Open(localeInfo.name, localeInfo.path);
        }


        /// <summary>Close the locales</summary> 
        /// <remarks>Invoked prior GoTo some other view</remarks>           .
        public void OnCloseLocales(string localesname)
        {
            localeInfo.Close(false);

            MaxProjectEventArgs args = new MaxProjectEventArgs
                (MaxProjectEventArgs.MaxEventTypes.CloseLocaleEd, localesname, null);

            this.MarkIfAbandoned(args);

            manager.ClearTabs();
            view.Clear();

            RaiseProjectActivity(this, args);
        }


        /// <summary>Register an locales file on project file open</summary>
        public bool OnProjectRegisterLocales(string name, string fullpath, bool active)
        {
            if (name == null || fullpath == null) return false;

            localeInfo.Set(name, fullpath);
            // Add to explorer
            MaxProjectEventArgs args = new MaxProjectEventArgs
                (MaxProjectEventArgs.MaxEventTypes.AddLocaleEd, name, fullpath);
            args.Active = active;

            RaiseProjectActivity(this, args);
            return true;
        }


        /// <summary>Initialize locales on project file open</summary>
        public MaxLocalizationEditorTab OnProjectOpenLocales()
        {
            // Presumably OnProjectRegisterInstaller was invoked previously
            if (localeInfo.path == null) return null;

            MaxLocalizationEditorTab editor = localeInfo.Create(localeInfo.name, localeInfo.path, false);

            if (!editor.OnOpening(localeInfo.path)) return null;

            localeInfo.editor = editor;
            // Notify framework of the open installer
            RaiseProjectActivity(this, new MaxProjectEventArgs
                (MaxProjectEventArgs.MaxEventTypes.OpenLocaleEd, localeInfo.name, localeInfo.path));

            return editor;
        }


        /// <summary>Remove locales from current project</summary>
        public void OnRemoveLocales()
        {
            if (localeInfo.path == null) return;
            // Remove from explorer
            RaiseProjectActivity(this, new MaxProjectEventArgs
                (MaxProjectEventArgs.MaxEventTypes.RemoveLocaleEd, localeInfo.name, localeInfo.path));

            localeInfo.Clear();

            if (view.IsLocales)
            {
                view.Clear();
                manager.ClearTabs();
            }
        }


        /// <summary>Remove locales from project and delete its file</summary>
        public void OnDeleteLocales()
        {
            string path = localeInfo.path; if (path == null) return;

            this.OnRemoveLocales();

            Utl.SafeDelete(path);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Database script activity 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Add new database script file and open editor</summary>
        public void OnAddNewDatabase(string dbname)
        {
            OnCloseView();

            string path = Utl.GetDatabaseFilePath(MaxProject.ProjectPath, dbname);

            MaxDbScriptEditorTab editor = this.CreateDatabaseScript(dbname, path, true);

            if (editor.New(path, false)) 
            {   // Simultaneous dbs note: we will index editor, not path   
                databases.Add(dbname, path);
                MarkViewDirty();
            }                 
            else view.Clear(); 
        }

        /// <summary>Add existing database script file and open editor</summary>
        public void OnAddExistingDatabase(string path)
        {
            string dbname = Path.GetFileNameWithoutExtension(path);

            OnCloseView(); 

            MaxDbScriptEditorTab editor = this.CreateDatabaseScript(dbname, path, false);  
 
            if  (editor.New(path, true)) 
            {
                editor.IsNew = false;
                // Simultaneous dbs note: we will index editor, not path   
                databases.Add(dbname, path);
                MarkViewDirty();      
            }
            else view.Clear();    
        }


        /// <summary>Reopen database script</summary>
        public bool OnOpenDatabase(string dbname)     
        {
            // Database is presumably already registered with project
            string path = this.databases[dbname] as string; if (path == null) return false;

            OnCloseView();

            MaxDbScriptEditorTab editor = this.CreateDatabaseScript(dbname, path, false);

            return editor.Open(dbname, path);   
        }


        /// <summary>Close an database script</summary>
        public void OnCloseDatabase(string databasename)     
        {
            MaxDbScriptEditorTab database = view.CurrentDatabase;
            if  (database == null) return;

            database.CloseEditor(false);
      
            MaxProjectEventArgs args = new MaxProjectEventArgs
                (MaxProjectEventArgs.MaxEventTypes.CloseDatabase, view.Name, view.FilePath);

            this.MarkIfAbandoned(args);                 

            manager.ClearTabs();
            view.Clear();
      
            RaiseProjectActivity(this, args);
        }


        /// <summary>Register a database script on project file open</summary>
        public bool OnProjectRegisterDatabase(string dbname, string fullpath, bool active)     
        {
            if (dbname == null || fullpath == null) return false;  
            if (this.databases.Contains(dbname))    return false;  

            this.databases.Add(dbname, fullpath);        
                                            // Add to explorer
            MaxProjectEventArgs args = new MaxProjectEventArgs   
                (MaxProjectEventArgs.MaxEventTypes.AddDatabase, dbname, fullpath);
            args.Active = active;

            RaiseProjectActivity(this, args);
            return true;
        }


        /// <summary>Initialize database script on project file open</summary>
        public MaxDbScriptEditorTab OnProjectOpenDatabase(string dbname, string fullpath)     
        {
            // Presumably OnProjectRegisterDatabase was invoked previously
            if (this.databases[dbname] == null) return null;

            MaxDbScriptEditorTab editor = CreateDatabaseScript(dbname, fullpath, false);

            if (!editor.OnOpening(fullpath)) return null;      
                                            // Notify framework
            RaiseProjectActivity(this, new MaxProjectEventArgs                                              
                (MaxProjectEventArgs.MaxEventTypes.OpenDatabase, dbname, fullpath));
                                                        
            return editor;         
        }


        /// <summary>Remove database script from current project</summary>
        public void OnRemoveDatabase(string dbname)
        {
            string path = this.databases[dbname] as string; if (path == null) return;      
                                            // Remove from explorer
            RaiseProjectActivity(this, new MaxProjectEventArgs
                (MaxProjectEventArgs.MaxEventTypes.RemoveDatabase, dbname, path));

            this.databases.Remove(dbname);

            if (view.IsDatabase) 
            {   
                view.Clear();
                manager.ClearTabs();
            }
            else MarkDirty();
        }


        /// <summary>Remove database script from project and delete its file</summary>
        public void OnDeleteDatabase(string dbname)
        {
            string path = this.databases[dbname] as string; if (path == null) return;      

            this.OnRemoveDatabase(dbname);

            Utl.SafeDelete(path);
        }


        /// <summary>Instantiate a MaxDbScriptEditorTab object</summary>
        /// <remarks>We use this method to ensure the current view is set immediately</remarks>
        MaxDbScriptEditorTab CreateDatabaseScript(string name, string path, bool isnew)
        {
            MaxDbScriptEditorTab db = new MaxDbScriptEditorTab(name, path, isnew);
            string filePath = path == null? db.FilePath: path;
            view.Set(ViewTypes.Database, name, filePath, db);
            return db;
        }



        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Voice rec resource file activity 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Reopen voice rec resource file</summary>
        public bool OnOpenVoiceRecResource(string filepath)     
        {
            // File is presumably already registered with project
            // string path = this.databases[dbname] as string; if (path == null) return false;

            OnCloseView();

            string filename = Path.GetFileName(filepath);
            MaxVoiceRecFileEditorTab editor = this.CreateVoiceRecFile(filename, filepath, false);

            return editor.Open(filename, filepath);   
        }


        /// <summary>Close a voice rec file</summary>
        public void OnCloseVoiceRecResource(string databasename)     
        {
            MaxVoiceRecFileEditorTab vrresx = view.CurrentVoiceRecFile;
            if  (vrresx == null) return;

            vrresx.CloseEditor(false);
      
            MaxProjectEventArgs args = new MaxProjectEventArgs
                (MaxProjectEventArgs.MaxEventTypes.CloseVrResx, view.Name, view.FilePath);

            this.MarkIfAbandoned(args);                 

            manager.ClearTabs();
            view.Clear();
      
            RaiseProjectActivity(this, args);
        }


        /// <summary>Instantiate a MaxVoiceRecFileEditorTab object</summary>
        /// <remarks>We use this method to ensure the current view is set immediately</remarks>
        MaxVoiceRecFileEditorTab CreateVoiceRecFile(string name, string path, bool isnew)
        {
            MaxVoiceRecFileEditorTab vr = new MaxVoiceRecFileEditorTab(name, path, isnew);
            string filePath = path == null? vr.FilePath: path;
            view.Set(ViewTypes.VrResx, name, filePath, vr);
            return vr;
        }



        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Media file activity 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Add media file to project</summary>
        public void OnAddMediaFile(string type, string path)
        {
            // We don't bother to return a project activity event, framework has
            // already added file to explorer. The dirty event is all we need.
            MarkDirty();      
        }


        /// <summary>Remove media file from project</summary>
        public void OnRemoveMediaFile(string type, string path)
        {
            // We don't bother to return a project activity event, framework has
            // already removed file from explorer. The dirty event is all we need.
            MarkDirty();
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Voice recognition resource file activity 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Add voice rec resource file to project</summary>
        public void OnAddVoiceRecResource(string type, string path)
        {
            MarkDirty();      
        }


        /// <summary>Remove voice rec resource file from project</summary>
        public void OnRemoveVoiceRecResource(string type, string path)
        {
            MarkDirty();
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // TTS Text file activity 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Add TTS ext file to project</summary>
        public void OnAddTtsTextFile(string type, string path)
        {
            MarkDirty();      
        }


        /// <summary>Remove TTS text file from project</summary>
        public void OnRemoveTtsTextFile(string type, string path)
        {
            MarkDirty();
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Application script activity support
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Dispose of current app prior to project Close</summary>
        /// <returns>State of current app prior to disposal</returns>
        /// <remarks>We must remove the app file stub if it has never been explicitly
        /// saved, since framework will not delete a project directory if it
        /// contains any app files</remarks>
        private AppDisposeInfo DisposeProject()       
        {
            AppDisposeInfo info = new AppDisposeInfo();
            if  (view.IsClear) return info;
            info.appName     = view.Name;
            info.appfilePath = view.FilePath;
      
            info.isCanceled = !this.CanClose(false);
            if  (info.isCanceled) return info;

            info.isAbandoned = view.ViewObject != null 
                && view.ViewObject.IsNew && view.ViewObject.SaveCount == 0;

            info.isExisting  = !view.ViewObject.IsNew;

            if  (info.isAbandoned) 
            {
                // MSC: A 'dirty' correction.  Since serialization is based off of what the user
                // sees, we will need to clear out a database node, in the case that is 
                // a new database file newly added to the project, but not saved to the project
                this.OnRemoveDatabase(view.Name);
                Utl.SafeDelete(info.appfilePath);
            }
            else if(info.isExisting && view.ViewObject.SaveCount == 0)
            {
                // MSC: A 'dirty' correction.  Since serialization is based off of what the user
                // sees, we will need to clear out a database node, in the case that is 
                // an existing database file newly added to the project, but not saved to the project
                this.OnRemoveDatabase(view.Name);
            }
            else this.SaveIDE(false);

            this.Clear();

            return info;
        }


        /// <summary>Determine whether current app can be closed</summary>
        public bool CanCloseCurrentApp()
        {
            if ( currentApp == null) return false;
            if (!view.Dirty)         return true;

            switch(Utl.PromptSaveChangesTo(currentApp.AppName, Const.AppSaveDlgTitle))
            {
               case DialogResult.Cancel: 
                    return false;

               case DialogResult.Yes:    
                    currentApp.Save(); 
                    break;
            }

            return true;
        }


        /// <summary>Actions on shutdown of framework</summary>
        /// <remarks>Either delete abandoned view file or save IDE config</remarks>
        public void OnFrameworkShutdownNotification(MaxUserInputEventArgs e) 
        {
            MaxFrameworkEventArgs args = new MaxFrameworkEventArgs
                (MaxFrameworkEventArgs.MaxEventTypes.Shutdown);
                                             
            if  (!view.IsClear && view.ViewObject.IsNew && view.ViewObject.SaveCount == 0) 
                                                 
                 Utl.SafeDelete(view.FilePath);
             
            else this.SaveIDE(false);             // Save tbx config if necessary
                                                  // Notify framework we're done
            this.RaiseFrameworkActivity(this, args);
        }


        /// <summary>State of current app prior to disposal</summary>
        public class AppDisposeInfo
        {
            public bool   isExisting;
            public bool   isCanceled;
            public bool   isAbandoned;
            public bool   isClosed;
            public bool   isDirty;
            public string appName;
            public string appfilePath;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Canvas activity  
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        // During transition from max single script (project == app) version, we
        // removed some handlers here, with callers invoking app directly. Others
        // we retained, since (a) a canvas could conceivably not be an app, and so
        // we wish to retain the ability to redirect here; (b) some are framework
        // menu requests forwarded by event layer, and we'd prefer not to have the
        // event layer access the app directly.

        /// <summary>Handle framework (menu/explorer) event to rename canvas</summary>
        public void OnRenameCanvas(string canvasname, string newname)
        {                                  
            currentApp.OnRenameCanvas(canvasname, newname, true);
        }


        /// <summary>Rename and remap a canvas</summary>
        public bool RenameCanvas(string oldname, string newname)
        {
            return currentApp.RenameCanvas(oldname, newname);
        }


        /// <summary>Handle framework (menu/explorer) event to remove canvas</summary>
        public void OnRemoveCanvas(string canvasname)
        {
            currentApp.OnRemoveCanvas(canvasname);  
        }


        /// <summary>Invoked after canvas removed to notify framework</summary>
        public void OnCanvasRemoved(string canvasname)
        {
            currentApp.OnCanvasRemoved(canvasname);
        }


        /// <summary>Handle framework (menu/explorer) event to rename node</summary>
        public void OnRenameNode(string canvasname, long nodeID)     
        {
            currentApp.OnRenameNode(canvasname, nodeID);
        }


        /// <summary>Handle framework (menu/explorer) event to remove node</summary>
        public void OnRemoveNode(string canvasname, long nodeID)    
        {
            currentApp.OnRemoveNode(canvasname, nodeID);
        }


        /// <summary>Invoked via event from framework to alter scale factor</summary>
        public void OnZoomRequest(int factor)
        {
            MaxCanvas canvas = this.CurrentCanvas(); if (canvas == null) return;

            canvas.View.DocScale = (factor == 0)? 1.0F: canvas.View.DocScale + (factor * 0.2F);
        }


        /// <summary>Invoked via event from framework to toggle grid</summary>
        public void OnGridRequest(int onoff)
        {
            MaxCanvas canvas = this.CurrentCanvas();
            if (canvas != null) canvas.ShowGrid(onoff != 0);
        }



        /// <summary>Invoked via event from framework to toggle variables tray</summary>
        public void OnViewTray(int onoff)
        {
            MaxCanvas canvas = this.CurrentCanvas() as MaxFunctionCanvas;
            if (canvas != null) canvas.VtrayManager.Show(onoff != 0);
        }



        public void OnPageSetupRequest()
        {
            new Metreos.Max.Framework.MaxPageSetupDlg().ShowDialog();            
        }



        /// <summary>Invoked via event from framework to print current canvas</summary>
        public void OnPrintRequest()
        {
            MaxCanvas canvas = this.CurrentCanvas(); if (canvas == null) return;
            bool result = false;

            try { canvas.View.Print(); result = true; } catch { }

            if (!result)
                 Utl.ShowPrintFailedDlg();
        }



        /// <summary>Invoked via event from framework to display print preview</summary>
        public void OnPrintPreviewRequest()
        {
            MaxCanvas canvas = this.CurrentCanvas();
            if (canvas != null)
            {
                PrintDocument pd = new PrintDocument();

                // Portrait or Landscape mode?
                pd.DefaultPageSettings.Landscape = Config.LandscapeMode;

                // What's the margin of the page?
                pd.DefaultPageSettings.Margins = Config.PageMargins;

                pd.DocumentName = MaxProject.ProjectName + " - " + this.CurrentCanvas().CanvasName;

                // Want to fit into one page?
                if (Config.OnePagePerView)
                {
                    // Fit the view into one page by calculating print scale
                    SizeF docsize = canvas.View.PrintDocumentSize;
                    if (docsize.Width > 1 || docsize.Height > 1) 
                    {
                        Rectangle b = pd.DefaultPageSettings.Bounds;
                        Margins m = pd.DefaultPageSettings.Margins;
                        float w = b.Width - (m.Left + m.Right);
                        float h = b.Height - (m.Top + m.Bottom);
                        float ratio = Math.Min(w/docsize.Width, h/docsize.Height);

                        // No reason to enlarge it.
                        if (ratio > 1)
                            ratio = 1;

                        canvas.View.PrintScale = ratio;
                    }
                }
                
                canvas.View.PrintPreview();
            }
        }


        /// <summary>Handle properties show request from framework</summary>
        public void OnFrameworkPropertiesRequest(MaxUserInputEventArgs args)
        {
            string entityName = args.UserInput1;
            int    entityType = args.UserInt1;

            switch(entityType)
            {
               case 0:
                    if (entityName == ProjectName)
                        PmProxy.ShowProperties(this, this.PmObjectType);
                    break;
            }
        }


        /// <summary>Retrieve canvas via selected toolbox tab</summary>
        private MaxCanvas GetCanvasForSelectedTab()
        {
            return (manager.SelectedTab == null)? null:
                    manager.SelectedTab.Control as MaxCanvas;
        }

        /// <summary>Handle Edit menu Copy request from framework</summary>
        public void OnEditCopy()
        {
            MaxCanvas canvas = this.GetCanvasForSelectedTab();
            if (canvas != null) canvas.View.EditCopy();
        }

        /// <summary>Handle Edit menu Paste request from framework</summary>
        public void OnEditPaste()
        {
            MaxCanvas canvas = this.GetCanvasForSelectedTab();
            if (canvas != null) canvas.View.EditPaste();    
        }

        /// <summary>Handle Edit menu Cut request from framework</summary>
        public void OnEditCut()
        {
            MaxCanvas canvas = this.GetCanvasForSelectedTab();
            if (canvas != null) canvas.View.EditCut();   
        }

        /// <summary>Handle Edit menu Delete/Del key forward from framework</summary>
        public void OnEditDelete()
        {
            MaxTabContent content = (manager.SelectedTab == null)? null:
                manager.SelectedTab.Control as MaxTabContent;
            if (content != null) content.OnEditDelete();     
        }

        /// <summary>Handle Edit menu Undo request from framework</summary>
        public void OnEditUndo()
        {
            MaxCanvas canvas = this.GetCanvasForSelectedTab();
            if (canvas != null) canvas.View.Undo();  
        }

        /// <summary>Handle Edit menu Redo request from framework</summary>
        public void OnEditRedo()
        {
            MaxCanvas canvas = this.GetCanvasForSelectedTab();
            if (canvas != null) canvas.View.Redo();   
        }

        /// <summary>Handle Edit menu Select All request from framework</summary>
        public void OnEditSelectAll()
        {
            MaxCanvas canvas = this.GetCanvasForSelectedTab();
            if (canvas != null) canvas.View.SelectAll();  
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Tools/Options changes  
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Actions on change of Tools/Options/Canvas/Wait for motion</summary>
        public void OnOptionWaitMotionChanged(MaxMenuEventArgs e) 
        {
            MaxApp app = currentApp; if (app == null) return;

            foreach(object content in app.Canvases.Values)
            {
                MaxCanvas canvas = content as MaxCanvas; if (canvas == null) continue;        
                if  (e.Checked)
                    canvas.View.SetBeginLinkingOnMouseMove();
                else canvas.View.SetBeginLinkingOnMouseDown();  
            }
        } 


        /// <summary>Actions on change of Tools/Options/Graphs/Large ports</summary>
        public void OnOptionLargePortsChanged(MaxMenuEventArgs e)
        {
            MaxApp app = currentApp; if (app == null) return;  

            foreach(object content in app.Canvases.Values)
            {
                MaxCanvas canvas = content as MaxCanvas; if (canvas == null) continue;

                MaxDocument doc = canvas.View.Document as MaxDocument;
                doc.SkipsUndoManager = true;

                try
                {
                    foreach(GoObject xgo in doc)
                    {
                        IMaxNode x = xgo as IMaxNode; 
                        if (x != null && (x is MaxIconicNode  || x is MaxIconicMultiTextNode)) 
                        {    
                            GoPort port = x.NodePort as GoPort; if (port == null) continue;
                            port.Size = e.Checked? Const.portSizeLarge: Const.portSizeNormal;
                            port.InvalidateViews();
                        }
                    }

                }   // try
                catch 
                { 
                }
                finally
                {
                    doc.SkipsUndoManager = false;
                }
            }
        }    

 
        /// <summary>Actions on change of Tools/Options/Graphs/Visible ports</summary>
        public void OnOptionVisiblePortsChanged(MaxMenuEventArgs e)
        {
            // Since we now show ports only when mouse is over the node,  
            // we have disabled here the showing of all ports. However the code 
            // has been left here in case we need to revert.
            MaxApp app = currentApp; if (app == null) return;

            foreach(object content in app.Canvases.Values)
            {
                MaxCanvas canvas = content as MaxCanvas; if (canvas == null) continue;

                MaxDocument doc = canvas.View.Document as MaxDocument;
                doc.SkipsUndoManager = true;

                try
                {
                    foreach(GoObject xgo in doc)
                    {
                        IMaxNode x = xgo as IMaxNode; 
                        if (x != null && (x is MaxIconicNode || x is MaxIconicMultiTextNode)) 
                        {    
                            GoPort port = x.NodePort as GoPort;  
                            if  (port == null || x is MaxRecumbentVariableNode) continue;

                            if  (e.Checked)
                            {
                                #if(false)
                                port.Style = GoPortStyle.Ellipse; 
                                port.Brush = Const.portBrush;
                                port.Pen   = Const.portPen;
                                #endif
                            }
                            else
                            {
                                port.Style = GoPortStyle.None;
                                port.Brush = null;
                                port.Pen   = null;
                            }
                  
                            port.InvalidateViews();             
                        }
                    }
                }   // try
                catch { }
                finally
                {
                    doc.SkipsUndoManager = false;
                }
            }
        } 


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Utility methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Register project as dirty both here and in framework</summary>
        public void MarkDirty() 
        {
            dirty = true;
            RaiseProjectActivity(this, ProjectDirtyEventArgs);
        }   


        /// <summary>Register project as dirty both here and in framework</summary>
        public void MarkDirty(bool ok) 
        {
            if (ok) RaiseProjectActivity(this, ProjectDirtyEventArgs);
            dirty = ok;
        }   


        /// <summary>Register project as not dirty both here and in framework</summary>
        public void MarkNotDirty(bool notify)  
        {
            if (dirty && notify) RaiseProjectActivity(this, ProjectNotDirtyEventArgs);
            dirty = false;
        } 


        /// <summary>Register view as dirty both here and in framework</summary>
        public void MarkViewDirty() 
        {
            MarkDirty();
            if  (!view.Dirty) RaiseProjectActivity(this, ViewDirtyEventArgs);
            view.Dirty = true;
        }   


        /// <summary>Register view as dirty both here and in framework</summary>
        public void MarkViewDirty(bool ok) 
        {
            MarkDirty(ok);
            if (ok) RaiseProjectActivity(this, ViewDirtyEventArgs);
            view.Dirty = ok;
        }   


        /// <summary>Register view as not dirty both here and in framework</summary>
        public void MarkViewNotDirty(bool notify)  
        {
            MarkNotDirty(notify);
            if  (view.Dirty && notify) RaiseProjectActivity(this, ViewNotDirtyEventArgs);
            view.Dirty = false;
        } 


        /// <summary>Determine if OK to open a new project</summary>
        private bool CanOpenNew()
        {
            return this.CanClose(true);
        }

        /// <summary>Determine if OK to close this project</summary>
        private bool CanClose(bool clear)
        {
            if (this.dirty)
            {
                switch(Utl.PromptSaveChangesTo(projectName, Const.AppSaveDlgTitle))
                {
                    case DialogResult.Cancel: return false;
                    case DialogResult.Yes:    this.Save(); break;
                }
            }

            if (clear) this.Clear();
            return true;
        }


        /// <summary>Determine if OK to name an app with specified name</summary>
        public bool CanNameApp(string name)
        {
            // We'll probably need some other tests here as well
            return Utl.IsValidAppName(name);
        }


        /// <summary>Determine if name passed is name of current app</summary>
        public bool IsNameOfCurrentApp(string appname)
        {
            return view.IsCurrentView(ViewTypes.App, appname);
        }


        /// <summary>Determine if OK to rename app to this name</summary>
        public bool CanRenameApp(string name, string oldname)
        {
            return name != null && name != oldname;
        }


        /// <summary>Notify framework to unhide properties</summary>
        public void ShowPropertiesWindow() 
        {
            RaiseProjectActivity(this, PropertiesEventArgs);
        }


        /// <summary>Enable or disable editing for all canvases</summary>
        public void EnableEditing(bool enable)
        {
            MaxApp app = MaxProject.CurrentApp; if (app == null) return;

            foreach(object x in app.Canvases.Values) 
            {     
                MaxFunctionCanvas canvas = x as MaxFunctionCanvas; 
                if (canvas != null) canvas.EnableEditing(enable);        
            }
        }


        /// <summary>Determine if multiple nodes selected, and if optional node is a member</summary>
        public bool IsMultipleNodeSelection(GoView view, IMaxNode node)  
        {
            GoSelection selection = view.Selection; if (selection == null) return false;
            int  n = 0, m = 0;

            foreach(object x in selection) 
            {
                if (x is IMaxNode) n++;
                if (node != null && x == node) m++;
            }

            bool result = (n < 2)? false: (node == null)? true: m > 0;
          
            return result;         
        }


        /// <summary>Clear project state</summary> 
        public void Clear()    
        {
            configSequence = 0;
            if (manager != null)
                manager.ClearTabs();

            this.apps.Clear();
            this.databases.Clear();
            view.Clear();
            this.dirty = false;
        }

        public void ClearProperties()
        {
            this.CreateProperties(null);
        }

        /// <summary>Format and save project name and project file full path</summary> 
        public void SetPathInfo(string name, string folder)      
        {
            string filename = name.IndexOf(Const.dot) == -1? name + Const.maxProjectFileExtension: name;
            projectFolder = folder;
            projectPath = folder + Const.bslash + filename;
            projectName = Utl.StripFileExtension(filename);
        }


        /// <summary>Format and save project name and project file full path</summary>
        public void SetPathInfo(string fullpath)      
        {
            projectPath   = fullpath;
            projectFolder = Utl.StripPathFilespec(fullpath);
            projectName   = Utl.StripPathFolderPlusExtension(fullpath);     
        }


        /// <summary>Set project state to no project open</summary>
        public void MarkProjectClosed()
        {
            this.SetPathInfo(null);
            this.Clear();
        }

                                              
        /// <summary>Return canvas hosted by current tab</summary>
        public MaxCanvas CurrentCanvas()
        {
            Crownwood.Magic.Controls.TabPage tabpage = manager.SelectedTab;
            return tabpage.Control as MaxCanvas;
        }


        /// <summary>Fire event to framework if framework is listening</summary>
        public void ConditionalRaiseFrameworkActivity(object sender, MaxFrameworkEventArgs e)
        {
            bool fireEvent 
                = e.MaxEventType == MaxFrameworkEventArgs.MaxEventTypes.Shutdown? true:
                  !MaxManager.ShuttingDown;

            if  (fireEvent) RaiseFrameworkActivity(sender, e);
        }


        /// <summary>Test signal from framework</summary>
        public void OnTestRequest()
        {
        }


        /// <summary>Return array of app's global variables</summary>
        public IMaxNode[] GetGlobalVariables()
        {
            return currentApp == null? new IMaxNode[0]: currentApp.GetGlobalVariables();
        }


        /// <summary>Debug display of undo manager state</summary>
        public static void ShowUndoManagerState(string text)
        {
            #if(false)  // Compile out since we don't usually need it
            MaxCanvas canvas = MaxProject.currentCanvas;
            if (canvas == null) return;
            MaxUndoManager undoManager = canvas.View.Document.UndoManager as MaxUndoManager;
            int ccount = 0, ecount = 0;

            Utl.Trace(String.Empty); Utl.Trace("--- undo mgr stack " + text + " ---");
            if (undoManager.AllEdits.Count == 0)
            {
                Utl.Trace("undo stack is empty");
            }
            else foreach(object x in undoManager.AllEdits)
            {
                GoUndoManagerCompoundEdit compound = x as GoUndoManagerCompoundEdit;
                if (compound == null) continue;
                ecount = 0;
                Utl.Trace(((++ccount).ToString()) + " Compound " + compound.PresentationName); 

                foreach(IGoUndoableEdit edit in compound.AllEdits)
                {
                    Utl.Trace(Const.spsp + ((++ecount).ToString()) + " edit " +  edit.ToString()); 
                }
            }

            Utl.Trace(String.Empty); 
            #endif
        }
       

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Convenience properties
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private static MaxApp currentApp   
        {
            get { return view.CurrentApp;  } 
            set 
            {
              if  (value == null) view.Clear(); 
              else view.Set(value.AppName, value); 
            } 
        } 

        public static MaxCanvas  currentCanvas
        {
            get {return MaxManager.Instance.CurrentTabContent() as MaxCanvas; }
        }  
        public static MaxApp     CurrentApp { get { return view.CurrentApp;     } } 
        public static MaxAppTree AppCanvas  { get { return currentApp.AppCanvas;} }

        public static void CancelMouse()
        {
            MaxCanvas canvas = MaxProject.currentCanvas;
            if (canvas != null) canvas.View.DoCancelMouse();
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Other properties  
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static string    CurrentViewName   { get { return view.Name;     } }
        public static ViewTypes CurrentViewType   { get { return view.ViewType; } } 

        /// <summary>Installer info wrapper</summary>
        public class  InstallerInfo 
        {
            public string name; public string path; 
            public MaxInstallerEditorTab editor;

            public void   Set(string n, string p) { name = n; path = p; }
            public void   Set(string n, string p, MaxInstallerEditorTab e) 
            { name = n; path = p; editor = e; }

            public void   Clear() { name = path = null; editor = null; }

            public void   Close(bool notify) 
            { 
                if (editor != null) editor.CloseEditor(notify); 
            }

            public bool InProject() { return path != null; }

            public MaxInstallerEditorTab Create(string name, string path, bool isnew)
            { 
                MaxInstallerEditorTab editor = new MaxInstallerEditorTab(name, path, isnew);
                Set(name, path, editor);
                MaxProject.View.Set(ViewTypes.Installer, name, path, editor);      
                return editor;
            }
        }

        /// <summary>Locales info wrapper</summary>
        public class LocalesInfo
        {
            public string name; public string path;
            public MaxLocalizationEditorTab editor;

            public void Set(string n, string p) { name = n; path = p; }
            public void Set(string n, string p, MaxLocalizationEditorTab e)
            { name = n; path = p; editor = e; }

            public void Clear() { name = path = null; editor = null; }

            public void Close(bool notify)
            {
                if (editor != null) editor.CloseEditor(notify);
            }

            public bool InProject() { return path != null; }

            public MaxLocalizationEditorTab Create(string name, string path, bool isnew)
            {
                MaxLocalizationEditorTab editor = new MaxLocalizationEditorTab(name, path, isnew);
                Set(name, path, editor);
                MaxProject.View.Set(ViewTypes.Locales, name, path, editor);
                return editor;
            }
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Interface implementations
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -        

        #region MaxSelectableObject Members

        public System.ComponentModel.PropertyDescriptorCollection MaxProperties
        {
            get { return this.properties; }   
        }

        /// <summary>Ask properties manager to create this object's properties</summary>                
        public PropertyDescriptorCollection CreateProperties(PropertyGrid.Core.PackageElement pe) 
        {
            MaxPropertiesManager propertiesManager = PmProxy.PropertiesManager;

            CreatePropertiesArgs args = new
                CreatePropertiesArgs(this, pe, this.PmObjectType);

            this.properties = propertiesManager.ConstructProperties(args);
            return this.properties;
        } 

        public Framework.Satellite.Property.DataTypes.Type PmObjectType {get{return pmObjectType;}}

        public void OnPropertiesChangeRaised(MaxProperty[] properties) { }  

        #endregion

        #region MaxObject Members

        public Metreos.Max.Core.ObjectTypes MaxObjectType
        {
            get { return ObjectTypes.Project; }
        }

        public string ObjectDisplayName { get { return Const.ProjectObjectDisplayName + MaxProject.ProjectName; } }

        public void MaxSerialize(XmlTextWriter writer)
        {
            // serializer.Serialize(writer);
        }

        #endregion

        #region ICustomTypeDescriptor Members
      
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }
      
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }
      
        EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }
      
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }
      
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
      
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }
      
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        { 
            return GetProperties();
        }
      
        public PropertyDescriptorCollection GetProperties()
        {
            return this.MaxProperties; // CRITICAL PART        
        }
      
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }
      
        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }
      
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }
      
        public string GetClassName()
        {
            TypeDescriptor.GetClassName(this, true);
            return null;
        }
      
        #endregion

    }  // class MaxProject

}      // namespace
