using System;  
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Xml;
using Metreos.Max.Core;
using Metreos.Max.Drawing;
using Metreos.Max.Framework;
using Metreos.Max.GlobalEvents;
using Metreos.Max.Core.Package;
using Metreos.Max.Core.Tool;
using Metreos.Max.Framework.Satellite.Property;
using Northwoods.Go;


namespace Metreos.Max.Manager
{
    public enum ViewTypes { None, App, Installer, Database, Locales, Media, VrResx }
  

    /// <summary>Structure describing state of current project view</summary>
    /// <remarks>Project view indicates what is currently open in the tab bank,
    /// whether nothing, an app script, an installer, or database scripts</remarks>
    public class MaxProjectView
    {
        public  MaxProjectView(MaxProject parent) { project = parent; }
        private MaxProject project;
        private ViewTypes  type;
        private string name;
        private string path;
        private bool   dirty;

        private IMaxViewType          viewObject;
        private MaxApp                currentApp;
        private MaxInstallerEditorTab installer;
        private MaxDbScriptEditorTab currentDatabase;
        private MaxLocalizationEditorTab locales;
        private MaxVoiceRecFileEditorTab currentVrResx;
        private Crownwood.Magic.Controls.TabPage tab;

        public  bool IsApp       { get { return type == ViewTypes.App ;       } }
        public  bool IsInstaller { get { return type == ViewTypes.Installer;  } }
        public  bool IsDatabase  { get { return type == ViewTypes.Database; } }
        public  bool IsLocales   { get { return type == ViewTypes.Locales; } }
        public  bool IsClear     { get { return type == ViewTypes.None; } }    
        public  bool Dirty       { get { return dirty; } set { dirty = value; } }
        public  Crownwood.Magic.Controls.TabPage CurrentTab 
        { get { return tab; } set { tab = value; } }

        public string       Name       { get { return name; } set { if (name != null) name = value; } }
        public string       FilePath   { get { return path; } set { path = value; } }
        public IMaxViewType ViewObject { get { return viewObject; } }
        public ViewTypes    ViewType   { get { return type; } }

        public void Clear()      
        { 
            type  = ViewTypes.None; 
            name  = path = null;
            tab   = null; 
            dirty = false; 
            currentApp = null;
            installer  = null;
            locales = null;
            viewObject = null;
            currentVrResx = null;
            currentDatabase = null;
        }


        /// <summary>Establish or clear app name</summary>
        public void Set(string viewname, MaxApp app)
        {
            Set(ViewTypes.App, viewname, app);
        }


        /// <summary>Establish or clear view name</summary>
        public void Set(ViewTypes newtype, string viewname, string viewfilepath, object viewObject)
        {       
            Set(newtype, viewname, viewObject);
            path = viewfilepath;
        }


        /// <summary>Establish or clear view name, and set title bar text accordingly</summary>
        public void Set(ViewTypes newtype, string viewname, object viewObj)
        {
            Clear();
            if (viewname == null || viewObj == null) return;  
            name = viewname;     
            type = newtype;

            switch(type)
            {  
               case ViewTypes.App:       
                    currentApp = viewObj as MaxApp; 
                    viewObject = currentApp;
                    break;

               case ViewTypes.Installer: 
                    installer  = viewObj as MaxInstallerEditorTab; 
                    viewObject = installer;
                    break;

                case ViewTypes.Locales:
                    locales = viewObj as MaxLocalizationEditorTab;
                    viewObject = locales;
                    break;

               case ViewTypes.Database:  
                    currentDatabase = viewObj as MaxDbScriptEditorTab;  
                    viewObject = currentDatabase;
                    break;

               case ViewTypes.VrResx:
                    currentVrResx = viewObj as MaxVoiceRecFileEditorTab;  
                    viewObject = currentVrResx;
                    break;
            }        
        }


        /// <summary>Save current view</summary>
        public bool Save()
        {        
            switch(type)
            {
               case ViewTypes.App:       return currentApp.Save();
               case ViewTypes.Installer: return installer.Save();
               case ViewTypes.Locales:   return locales.Save();
               case ViewTypes.Database:  return currentDatabase.Save();
               case ViewTypes.VrResx:    return currentVrResx.Save(); 
            } 
            return false;       
        }


        /// <summary>Determine if name passed is name of current view</summary>
        public bool IsCurrentView(ViewTypes thistype, string thisname)
        {
            return (thistype == type && thisname != null && name != null && name == thisname);
        }

        /// <summary>Get view as app, or establish current view as app</summary>
        public MaxApp CurrentApp 
        {
            get { return currentApp; }
            set 
            {
                currentApp = value; 
                name = currentApp == null? null: currentApp.AppName; 
                type = currentApp == null? ViewTypes.None: ViewTypes.App; 
            }
        }


        /// <summary>Get view as installer, or establish current view as installer</summary>
        private MaxInstallerEditorTab Installer  
        {
            get { return installer; } 
            set 
            {
                installer = value;
                name = installer == null? null: installer.Text;
                type = installer == null? ViewTypes.None: ViewTypes.Installer;
            }
        }

        /// <summary>Get view as installer, or establish current view as installer</summary>
        private MaxLocalizationEditorTab Locales
        {
            get { return locales; }
            set
            {
                locales = value;
                name = locales == null ? null : locales.Text;
                type = locales == null ? ViewTypes.None : ViewTypes.Locales;
            }
        }
  
        /// <summary>Get view as database, or establish current view as database</summary>
        public MaxDbScriptEditorTab CurrentDatabase 
        {
            get { return currentDatabase; }
            set 
            {
                currentDatabase = value; 
                name = currentDatabase == null? null: currentDatabase.Text;
                type = currentDatabase == null? ViewTypes.None: ViewTypes.Database;
            }
        }

        /// <summary>Get view as database, or establish current view as database</summary>
        public MaxVoiceRecFileEditorTab CurrentVoiceRecFile 
        {
            get { return currentVrResx; }
            set 
            {
                currentVrResx = value; 
                name = currentVrResx == null? null: currentVrResx.Text;
                type = currentVrResx == null? ViewTypes.None: ViewTypes.VrResx;
            }
        }

        /// <summary>Convenience attribute for backwards compatibility</summary>
        public string CurrentAppName { get { return type == ViewTypes.App? name: null; } }

    } // class MaxProjectView

}   // namespace
