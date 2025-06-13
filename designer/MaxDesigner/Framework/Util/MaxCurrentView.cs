using System;
using Metreos.Max.Core;


namespace Metreos.Max.Framework
{
    /// <summary>Structure describing state of current project view</summary>
    public class CurrentView
    {
        public  MaxMain   main;
        private MaxMain.ViewTypes type;
        private string name;
        private string path;
        private bool   dirty;
        public  bool   Dirty      
        {
            get { return dirty; } 
            set 
            {   dirty = value; 
                if (dirty) Const.MakeMaxTitle(MaxMain.ProjectFilename, name);
                main.Text = dirty? Const.AppTitleDirty: Const.AppTitleNormal;
            } 
        }
        public  bool   IsApp      { get { return type == MaxMain.ViewTypes.App;        } }
        public  bool  IsInstaller { get { return type == MaxMain.ViewTypes.Installer; } }
        public  bool  IsLocales   { get { return type == MaxMain.ViewTypes.Locales; } }
        public  bool   IsDatabase { get { return type == MaxMain.ViewTypes.Database; } }
        public  bool   IsVrResx   { get { return type == MaxMain.ViewTypes.VrResx;     } }
        public  bool   IsClear    { get { return type == MaxMain.ViewTypes.None;       } }

        public  void   Clear()    { name = path = null; dirty = false; type = 0; }
        public  string Name       { get { return name; } set { name = value;   } }
        public  MaxMain.ViewTypes ViewType{ get { return type; } }

        /// <summary>Establish or clear view name; set title bar accordingly</summary>
        public void Set(string viewname)
        {
            Set(type, viewname);
        }

        /// <summary>Establish or clear view name/path; set title bar accordingly</summary>
        public void Set(MaxMain.ViewTypes newtype, string viewname, string viewfilepath)
        {
            path = viewfilepath;
            Set(newtype, viewname);
        }

        /// <summary>Establish or clear view name; set title bar accordingly</summary>
        public void Set(MaxMain.ViewTypes newtype, string viewname)
        {
            if (viewname == null || type != newtype) Clear();
            if (viewname != null)   type  = newtype;
            name = viewname;
            Const.MakeMaxTitle(MaxMain.ProjectFilename, name);
        }

        /// <summary>Convenience attribute</summary>
        public string CurrentAppName { get { return type == MaxMain.ViewTypes.App? name: null; } }
    

        /// <summary>Delete backing file</summary>
        public void Delete()
        {
            Unload();
            Utl.SafeDelete(path);
            Clear();
        }

        /// <summary>Unload explorer entry</summary>
        public void Unload()
        {
            switch(type)
            {
                case MaxMain.ViewTypes.App:         main.Explorer.RemoveApp(name);      break;
                case MaxMain.ViewTypes.Installer:   main.Explorer.RemoveInstaller();    break;
                case MaxMain.ViewTypes.Locales:     main.Explorer.RemoveLocales();      break;
                case MaxMain.ViewTypes.Database:    main.Explorer.RemoveDatabase(name); break;
            }
        }
    }  // class CurrentView
}    // namespace
