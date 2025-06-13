using System;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization;
using Metreos.Max.Core;
using Metreos.Max.Manager;
using Metreos.Max.Drawing;
using Metreos.Max.Framework;
using Metreos.Max.GlobalEvents;
using Metreos.Max.Resources.Images;


 
namespace Metreos.Max.Drawing
{
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxLocalizationEditorTab 
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>A datagrid editor canvas to be hosted in a tab frame</summary>
    public class MaxLocalizationEditorTab: MaxTabContent, IMaxViewType 
    {
        // Construction

        public MaxLocalizationEditorTab(string name, string path, bool isnew): base(name) 
        {
            this.canvasType = CanvasTypes.Text;

            this.filename = name;   
            
            this.ed = new MaxLocalizationEditor(this);

            this.filepath = path;

            this.Init();  
            // pmObjectType = Framework.Satellite.Property.DataTypes.Type.Installer;

            if (!isnew) return;

            ed.Save(this.filepath);           
        }

        public void Init()
        {                                                       
            this.Controls.Add(ed); 
      
            RaiseFrameworkActivity += OutboundHandlers.FrameworkActivityCallback;      
            RaiseCanvasActivity    += OutboundHandlers.CanvasActivityProxy;

            // this.CreateProperties(null);
        }

        // Properties

        protected MaxLocalizationEditor ed; 
        protected Crownwood.Magic.Controls.TabPage tab; 
        protected string filename;     
        public    string FileName   { get { return filename; } } 
        protected string filepath;     
        public    string FilePath   { get { return filepath; } } 
        protected int    saveCount;
        public    int    SaveCount  { get { return saveCount;} } 
        protected bool   isNewFile;
        public    bool   IsNew      { get { return isNewFile; } set { isNewFile = value; } }

        public int ImageIndex()
        {
            return MaxImageIndex.framework16x16IndexLocaleEd;
        }

        public MaxProjectEventArgs.MaxEventTypes EventArgsAddType()
        { 
            return MaxProjectEventArgs.MaxEventTypes.AddLocaleEd;
        }

        public MaxProjectEventArgs.MaxEventTypes EventArgsOpenType()
        { 
            return MaxProjectEventArgs.MaxEventTypes.OpenLocaleEd;
        }

        public MaxProjectEventArgs.MaxEventTypes EventArgsCloseType()
        { 
            return MaxProjectEventArgs.MaxEventTypes.CloseLocaleEd;
        }

        public MaxProjectEventArgs.MaxEventTypes EventArgsRenameType()
        { 
            return MaxProjectEventArgs.MaxEventTypes.RenameLocaleEd;
        }
        // Events

        public event GlobalEvents.MaxFrameworkActivityHandler RaiseFrameworkActivity;
        public event GlobalEvents.MaxCanvasActivityHandler    RaiseCanvasActivity;

        // Commands
        public bool New(string path, bool exists)
        {
            this.tab = MaxManager.Instance.AddTab(filename, this, this.ImageIndex());
            MaxProject.View.CurrentTab = tab;
            this.filepath = path;
 
            MaxProjectEventArgs.MaxEventTypes eventType = this.EventArgsAddType();
       
            MaxProjectEventArgs args = new MaxProjectEventArgs(eventType, filename, filepath);
            args.Active = true;

            if (exists)                            
            {                                     
                ed.NoNotify = true;               // Adding existing file to project

                ed.Open(path);
      
                ed.NoNotify = false;
            }
            else 
            {
                ed.Save(path);
        
                // Load a default configuration template into the installer editor
                //ed.InitializeConfig();
            }
            // We register with the file system initially, but this does not count  
            // as a user save when it comes time to determine if file was abandoned. 
      
            SignalProjectActivity(this, args); 
 
            this.isNewFile = true;
            return true;
        }


        public bool Open(string name, string path)   
        {                               
            this.filepath = path;
            this.filename = name;
            RaiseFrameworkActivity(this, MaxProject.SuspendLayoutEventArgs);

            Crownwood.Magic.Controls.TabPage tab 
                = MaxManager.Instance.AddTab(name, this, this.ImageIndex());
            MaxProject.View.CurrentTab = tab;
      
            MaxProjectEventArgs.MaxEventTypes eventType = this.EventArgsOpenType();
           
            MaxProjectEventArgs eventArgs = new MaxProjectEventArgs(eventType, name, path);

            ed.NoNotify = true;                   // Suppress text change event

            bool result = ed.Open(path);  

            ed.NoNotify = false;    
           
            eventArgs.Result = result? 
                MaxProjectEventArgs.MaxResults.OK: MaxProjectEventArgs.MaxResults.Error;
          
            SignalProjectActivity(this, eventArgs);

            RaiseFrameworkActivity(this, MaxProject.ResumeLayoutEventArgs);

            SignalProjectActivity (this, MaxProject.ViewNotDirtyEventArgs);

            this.Dirty = false;
            return result;
        }


        public bool CloseEditor(bool notify)
        {
            // remove handlers, so that events don't duplicate
            MaxProjectEventArgs.MaxEventTypes eventType = this.EventArgsCloseType();      

            MaxProjectEventArgs args = new MaxProjectEventArgs(eventType, this.filename);

            args.Result = this.CanClose()?   
                MaxProjectEventArgs.MaxResults.OK: MaxProjectEventArgs.MaxResults.Error;         

            if (notify) SignalProjectActivity(this, args);

            return args.Result == MaxProjectEventArgs.MaxResults.OK;
        }


        public bool SaveAs(string path)
        {
            bool result = ed.Save(path);

            if (result)
            {   
                SignalProjectActivity(this, MaxProject.ViewNotDirtyEventArgs);
                this.saveCount++;
            }

            return result;
        } 


        public bool Save()
        {
            return this.SaveAs(filepath);    
        } 


        /// <summary>Rename the object and its file</summary>
        public bool Rename(string oldname, string newname)
        {
            bool   result  = false;               // First rename source file
            string oldpath = filepath; 
            string ext     = Path.GetExtension(filepath);   
            string folder  = Utl.StripPathFilespec(oldpath);
            string newpath = folder + Const.bslash + newname + ext;
            try  { File.Move(oldpath, newpath); result = true; } 
            catch{ }
            if   (!result) return false; 

            this.filename = newname;
            this.filepath = newpath; 
            // Notify framework
            MaxProjectEventArgs.MaxEventTypes eventType = this.EventArgsRenameType();                                                     
            MaxProjectEventArgs args = new MaxProjectEventArgs(eventType);
            args.NewName = newname;
            args.OldName = oldname;
            args.ScriptPath = newpath;

            SignalProjectActivity(this, args);
     
            return true;
        } 


        // Support methods

        /// <summary>If dirty, prompt to save and save if confirmed</summary>
        public bool CanClose()
        {
            if (this.Dirty)
                switch(Utl.PromptSaveChangesTo(this.filename, Const.FileSaveDlgTitle))
                {
                    case DialogResult.Cancel: return false;
                    case DialogResult.Yes:    this.Save(); break;
                }
       
            return true;
        }


        /// <summary>Initailize editor in preparation for opening file</summary>
        public bool OnOpening(string path)
        {
            this.filepath = path;
            // this.CreateProperties(null);           
            return true;
        }
    } // class MaxLocalizationEditorTab 
}   // namespace