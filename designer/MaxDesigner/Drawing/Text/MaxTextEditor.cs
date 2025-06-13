using System;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Metreos.Max.Manager;
using Metreos.Max.Drawing;
using Metreos.Max.Framework;
using Metreos.Max.GlobalEvents;
using Metreos.Max.Resources.Images;


 
namespace Metreos.Max.Drawing
{

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxTextEditorTab 
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>A text editor canvas to be hosted in a tab frame</summary>
    public abstract class MaxTextEditorTab: MaxTabContent, IMaxViewType 
    {
        // Construction

        public MaxTextEditorTab(string name): base(name) 
        {
            this.canvasType = CanvasTypes.Text;

            this.filename = name;                          
        }

        public void Init()
        {                                                                   
            ed.Font      = new Font(FontFamily.GenericMonospace, Config.textEditorFontSize);
            ed.BackColor = Const.ColorMaxBackground;
            ed.Dock      = DockStyle.Fill; 
            ed.WordWrap  = false;
            ed.BorderStyle = BorderStyle.None;

            this.Controls.Add(ed);
      
            RaiseFrameworkActivity += OutboundHandlers.FrameworkActivityCallback;      
            RaiseCanvasActivity    += OutboundHandlers.CanvasActivityProxy;

            // this.CreateProperties(null);
        }


        // Properties

        protected MaxTextEditor ed; 
        protected Crownwood.Magic.Controls.TabPage tab; 
        protected string filename;     
        public    string FileName   { get { return filename; } } 
        protected string filepath;     
        public    string FilePath   { get { return filepath; } } 
        protected int    saveCount;
        public    int    SaveCount  { get { return saveCount;} } 
        protected bool   isNewFile;
        public    bool   IsNew      { get { return isNewFile; } set { isNewFile = value; } }

        public abstract int[] TabStops();
        public abstract int   ImageIndex();
        public abstract MaxProjectEventArgs.MaxEventTypes EventArgsAddType();
        public abstract MaxProjectEventArgs.MaxEventTypes EventArgsOpenType();
        public abstract MaxProjectEventArgs.MaxEventTypes EventArgsCloseType();
        public abstract MaxProjectEventArgs.MaxEventTypes EventArgsRenameType();


        // Events

        public event GlobalEvents.MaxFrameworkActivityHandler RaiseFrameworkActivity;
        public event GlobalEvents.MaxCanvasActivityHandler    RaiseCanvasActivity;


        // Commands
  
        public bool New(string path, bool exists)
        {
            this.tab = MaxManager.Instance.AddTab(filename, this, this.ImageIndex());
            MaxProject.View.CurrentTab = tab;
            this.filepath = path;
            ed.AcceptsTab = true;              
 
            MaxProjectEventArgs.MaxEventTypes eventType = this.EventArgsAddType();
       
            MaxProjectEventArgs args = new MaxProjectEventArgs(eventType, filename, filepath);
            args.Active = true;

            if (exists)                            
            {                                     
                ed.NoNotify = true;               // Adding existing file to project
                 
                if (ed.Open(path)) ed.SetTabs(this.TabStops());

                ed.NoNotify = false;
            }
            else ed.Save(path);
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

            if (result) ed.SetTabs(this.TabStops());
           
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
            bool   result  = false;         // First rename source file
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
   
    } // class MaxTextEditorTab 


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxDbScriptEditorTab 
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>An installer text editor canvas to be hosted in a tab frame</summary>
    public class MaxDbScriptEditorTab: MaxTextEditorTab 
    {
        public MaxDbScriptEditorTab(string name, string path, bool isnew): base(name) 
        {
            this.ed = new MaxDbScriptEditor(this);

            this.filepath = path;
            this.Init();  
            // pmObjectType = Framework.Satellite.Property.DataTypes.Type.Database;

            if (!isnew) return;
            ed.AppendText(Const.InitialDatabaseText);

            ed.SetTabs(this.TabStops());
    
            ed.Save(this.filepath); 
        }

        public override int[] TabStops()
        {
            int t = Config.databasesTabStop;
            return new int[] { t,t,t,t,t,t,t,t };
        }

        public override int ImageIndex()
        {
            return MaxImageIndex.framework16x16IndexDbScripts;
        }

        public override MaxProjectEventArgs.MaxEventTypes EventArgsAddType()
        { 
            return MaxProjectEventArgs.MaxEventTypes.AddDatabase;
        } 

        public override MaxProjectEventArgs.MaxEventTypes EventArgsOpenType()
        { 
            return MaxProjectEventArgs.MaxEventTypes.OpenDatabase;
        } 

        public override MaxProjectEventArgs.MaxEventTypes EventArgsCloseType()
        { 
            return MaxProjectEventArgs.MaxEventTypes.CloseDatabase;
        } 

        public override MaxProjectEventArgs.MaxEventTypes EventArgsRenameType()
        { 
            return MaxProjectEventArgs.MaxEventTypes.RenameDatabase;
        }
    }  


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxDbScriptEditor 
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxDbScriptEditor: MaxTextEditor
    {
        public MaxDbScriptEditor(MaxTabContent tab): base(tab) 
        { 
        }
    }  



    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxVoiceRecFileEditorTab 
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>An installer text editor canvas to be hosted in a tab frame</summary>
    public class MaxVoiceRecFileEditorTab: MaxTextEditorTab 
    {
        public MaxVoiceRecFileEditorTab(string name, string path, bool isnew): base(name) 
        {
            this.ed = new MaxVoiceRecFileEditor(this);

            this.filepath = path;
            this.Init();  

            if (!isnew) return;
            // ed.AppendText(Const.InitialVoiceRecFileText);

            ed.SetTabs(this.TabStops());
    
            ed.Save(this.filepath); 
        }

        public override int[] TabStops()
        {
            int t = Config.databasesTabStop;
            return new int[] { t,t,t,t,t,t,t,t };
        }

        public override int ImageIndex()
        {
            return MaxImageIndex.framework16x16IndexVrResources;
        }

        public override MaxProjectEventArgs.MaxEventTypes EventArgsAddType()
        { 
            return MaxProjectEventArgs.MaxEventTypes.AddVrResx;
        } 

        public override MaxProjectEventArgs.MaxEventTypes EventArgsOpenType()
        { 
            return MaxProjectEventArgs.MaxEventTypes.OpenVrResx;
        } 

        public override MaxProjectEventArgs.MaxEventTypes EventArgsCloseType()
        { 
            return MaxProjectEventArgs.MaxEventTypes.CloseVrResx;
        } 

        public override MaxProjectEventArgs.MaxEventTypes EventArgsRenameType()
        { 
            return MaxProjectEventArgs.MaxEventTypes.None; // Rename not implemented
        }
    }  


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxVoiceRecFileEditor 
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxVoiceRecFileEditor: MaxTextEditor
    {
        public MaxVoiceRecFileEditor(MaxTabContent tab): base(tab) 
        { 
        }
    }  




    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxTextEditor 
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxTextEditor: RichTextBox
    {
        private MaxTabContent host;
        private bool noNotify;
        public  bool NoNotify { set { noNotify = value; } }

        public  MaxTextEditor(MaxTabContent tab) 
        { 
            this.host = tab;  
        }


        /// <summary>Open file and load into editor</summary>
        public bool Open(string path)
        {
            bool result = false;
            try 
            {
//              CSCsi81266
//              this.LoadFile(path, RichTextBoxStreamType.RichText); 
                StreamReader sqlReader = new StreamReader(path, System.Text.Encoding.UTF8);
                this.Text = sqlReader.ReadToEnd();
                sqlReader.Close();
                result = true; 
            }
            catch 
            {
                try { this.LoadFile(path, RichTextBoxStreamType.PlainText); result = true; }
                catch { }
            }

            if (!noNotify) host.SignalNotDirty();
            return result;
        }


        /// <summary>Save editor file</summary>
        public bool Save(string path)
        {
            bool result = false;
            try 
            {
//              CSCsi81266
//              this.SaveFile(path, RichTextBoxStreamType.RichText); 
                System.IO.StreamWriter streamWriter = new StreamWriter(path, false, System.Text.Encoding.UTF8);
                streamWriter.Write(this.Text);
                streamWriter.Close();
                result = true; }
            catch { }

            host.SignalNotDirty();
            return result;
        }


        /// <summary>Set tab stops to indicated values</summary>
        public void SetTabs(int[] tabstops)
        {
            bool oldNoNotify = this.noNotify;
            this.noNotify = true;
            this.AcceptsTab = true;    
            this.SelectAll();                   
            this.SelectionTabs = tabstops;
            this.Select(0,0);
            this.noNotify = oldNoNotify;
        }


        /// <summary>Monitor change to document text</summary>
        protected override void OnTextChanged(EventArgs e)
        {
            if (!host.Dirty && !noNotify) 
                host.SignalDirty();
        }

    } // class MaxTextEditor

}   // namespace




