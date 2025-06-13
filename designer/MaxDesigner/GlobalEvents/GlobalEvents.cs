using System;
using System.Globalization;
using Metreos.Max.Core;
using Metreos.Max.Drawing;



namespace Metreos.Max.GlobalEvents
{
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Prototypes
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    // Events from MAX via event layer, destined for framework

    public delegate void MaxFrameworkActivityHandler(object sender, MaxFrameworkEventArgs e); 
    public delegate void MaxProjectActivityHandler  (object sender, MaxProjectEventArgs e); 
    public delegate void MaxFocusActivityHandler    (object sender, MaxFocusEventArgs e); 
    public delegate void MaxNodeActivityHandler     (object sender, MaxNodeEventArgs e);  
    public delegate void MaxCanvasActivityHandler   (object sender, MaxCanvasEventArgs e);  
    public delegate void MaxTabActivityHandler      (object sender, MaxCanvasTabEventArgs e); 
    public delegate void MaxOutputWindowHandler     (object sender, MaxOutputWindowEventArgs e); 
    public delegate void MaxMenuOutputHandler       (object sender, MaxMenuOutputEventArgs e); 
    public delegate void MaxStatusBarOutputHandler  (object sender, MaxStatusBarOutputEventArgs e); 

    // Events from framework, such as satellites or menus, destined for MAX

    //  Satellite window events, such as output, closing, menu selection, etc
    public delegate void MaxPropertiesActivityHandler(object sender, MaxPropertiesEventArgs e);
    public delegate void MaxToolboxActivityHandler   (object sender, MaxToolboxEventArgs e);
    public delegate void MaxExplorerActivityHandler  (object sender, MaxExplorerEventArgs e);
    // Menu input
    public delegate void MaxMenuActivityHandler(object sender, MaxMenuEventArgs e);
    // Prompted input
    public delegate void MaxUserInputHandler   (object sender, MaxUserInputEventArgs e);
    // Tab change requests use MaxTabActivityHandler from above
    // Max start request uses MaxFrameworkActivityHandler from above


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // EventArgs derivatives: inbound events
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxPropertiesEventArgs: EventArgs
    {
        public enum MaxEventTypes { None, Changing, Changed }

        public MaxPropertiesEventArgs(MaxEventTypes type, Object properties)
        {
            maxEventType = type; propertyDescriptors = properties;
        }

        public MaxPropertiesEventArgs(MaxEventTypes type)
        {
            maxEventType = type; 
        }

        protected MaxEventTypes maxEventType;
        public    MaxEventTypes MaxEventType { get { return maxEventType;  } }
    
        protected Object propertyDescriptors;
        public    Object PropertyDescriptors 
        { get { return propertyDescriptors; }  set { propertyDescriptors = value; } }    
    }


    public class MaxToolboxEventArgs: EventArgs
    {
        public enum MaxEventTypes { None, ToolGroupRequest, ToolGroupReply }

        public MaxToolboxEventArgs(MaxEventTypes type, Object obj)
        {
            eventType = type; payload = obj;
        }

        protected MaxEventTypes eventType;
        public    MaxEventTypes MaxEventType 
        { get { return eventType; } set { eventType = value; } }

        protected Object payload;
        public    Object Payload  { get { return payload; } set { payload = value; } }
    }


    public class MaxExplorerEventArgs: EventArgs
    {
        public enum MaxEventTypes { None, ToBeDefined }
        // We can probably lose this class
        public MaxExplorerEventArgs(MaxEventTypes type, Object obj)
        {
            maxEventType = type; toBeDefined = obj;
        }

        protected MaxEventTypes maxEventType;
        public    MaxEventTypes MaxEventType { get { return maxEventType;  } }

        protected Object toBeDefined;
        public    Object ToBeDefined { get { return toBeDefined; } }
    }


    public class MaxMenuEventArgs: EventArgs    
    {
        public enum MaxEventTypes 
        {
            None, EditUndo, EditRedo, EditCut, EditCopy, EditPaste, EditDelete, 
            EditSelectAll,  EditGoTo,
            OptWaitMotion,  OptLargePorts, OptVisiblePorts
        }

        public MaxMenuEventArgs(MaxEventTypes type)
        {      
            maxEventType = type; 
        }

        public MaxMenuEventArgs(MaxEventTypes type, bool b)
        {      
            maxEventType = type; isChecked = b;
        }

        protected bool isChecked;
        public    bool Checked               { get { return isChecked;   } }
        protected MaxEventTypes maxEventType;
        public    MaxEventTypes MaxEventType { get { return maxEventType;} }
    }  


    public class MaxUserInputEventArgs: EventArgs
    {
        public enum MaxEventTypes 
        {
            None, NewProject, OpenProject, SaveProject, CloseProject, CloseFile,
            AddNewScript, AddExistingScript, OpenScript, RemoveScript, 
            SaveFile, ShowProperties, FocusCanvas, FocusTray,
            AddNewInstaller, AddExistingInstaller, OpenInstaller, RemoveInstaller,
            AddNewLocales, AddExistingLocales, OpenLocales, RemoveLocales,
            AddNewDatabase, AddExistingDatabase, OpenDatabase, RemoveDatabase,
            AddNewMediaFile,   AddExistingMediaFile,   RemoveMediaFile,
            AddNewTtsTextFile, AddExistingTtsTextFile, OpenTtsText,   RemoveTtsText,
            AddNewVrResxFile,  AddExistingVrResxFile,  OpenVrResx,    RemoveVrResxFile,
            RenameCanvas, RenameNode, RemoveCanvas, RemoveNode, GoToNode,
            Zoom, Grid, Tray, Test, Print, PrintPreview, Build, Deploy, Shutdown, PageSet
        }

        public MaxUserInputEventArgs(MaxEventTypes type, string user1)
        {
            maxEventType = type; this.userInput1 = user1;
        }

        public MaxUserInputEventArgs(MaxEventTypes type, string user1, string user2)
        {
            maxEventType = type; this.userInput1 = user1; this.userInput2 = user2;
        }

        public MaxUserInputEventArgs(MaxEventTypes type, string user1, long long1)
        {
            maxEventType = type; this.userInput1 = user1; this.userLong1 = long1;
        }

        public MaxUserInputEventArgs(MaxEventTypes type, int user1)
        {
            maxEventType = type; this.userInt1 = user1; 
        }

        public MaxUserInputEventArgs(MaxEventTypes type, string user1, int int1)
        {
            maxEventType = type; this.userInput1 = user1; this.userInt1 = int1; 
        }

        protected MaxEventTypes maxEventType;
        public    MaxEventTypes MaxEventType { get { return maxEventType;  } }
 
        protected string userInput1;
        public    string UserInput1 { get { return userInput1;  } }
        protected string userInput2;
        public    string UserInput2 { get { return userInput2;  } }
        protected long   userLong1;
        public    long   UserLong1  { get { return userLong1;   } }
        protected int    userInt1;
        public    int    UserInt1   { get { return userInt1;    } }
    }



    public class MaxFocusEventArgs: EventArgs
    {
        public enum MaxEventTypes { None, GotFocus, LostFocus }

        public MaxFocusEventArgs(MaxEventTypes type, MaxSelectableObject obj)
        {
            maxEventType = type; objectWithFocus = obj;
        }

        protected MaxEventTypes maxEventType;
        public    MaxEventTypes MaxEventType { get { return maxEventType;  } }
 
        protected MaxSelectableObject objectWithFocus;
        public    MaxSelectableObject ObjectWithFocus { get { return objectWithFocus;  } }
    }


    public class MaxCanvasTabEventArgs: EventArgs
    {
        public enum MaxEventTypes 
        { 
            None, ReOpen, Close, Add, Clear, GoTo, Toggle, Rename, Remove
        }

        public MaxCanvasTabEventArgs(MaxEventTypes type)
        {
            maxEventType = type; tabpage = null;
        }

        public MaxCanvasTabEventArgs(MaxEventTypes type, string tabname)
        {
            maxEventType = type; this.tabname = tabname; tabpage = null;
        }

        public MaxCanvasTabEventArgs(MaxEventTypes type, Crownwood.Magic.Controls.TabPage page)
        {
            maxEventType = type; tabpage = page;
        }

        public MaxCanvasTabEventArgs(MaxEventTypes type, string tabname, Crownwood.Magic.Controls.TabPage page)
        {
            maxEventType = type; this.tabname = tabname; tabpage = page;
        }

        public MaxCanvasTabEventArgs(MaxEventTypes type, string tabname, string oldname, long id)
        {
            maxEventType = type; this.tabname = tabname; this.oldname = oldname; nodeID = id;
        }

        protected MaxEventTypes maxEventType;
        public    MaxEventTypes MaxEventType { get { return maxEventType;  } }

        protected string tabname;
        public    string TabName { get { return tabname; }  set { tabname = value;} }  

        protected string oldname;
        public    string OldName { get { return oldname; } }  

        protected long   nodeID;
        public    long   NodeID  { get { return nodeID; } }  
    
        protected Crownwood.Magic.Controls.TabPage tabpage;
        public    Crownwood.Magic.Controls.TabPage TabPage{ get { return tabpage; } }    
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // EventArgs derivatives: outbound events
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxProjectEventArgs: EventArgs
    {
        public enum MaxEventTypes 
        {
            None,  New, Open, Close, Save, SaveAs, Dirty, NotDirty, AppDirty, AppNotDirty, 
            AddScript, OpenScript, CloseScript, RemoveScript, RenameScript, Build, Properties,
            AddInstaller, OpenInstaller, CloseInstaller, RemoveInstaller, RenameInstaller,
            AddLocales,   OpenLocales,   CloseLocales,   RemoveLocales,   RenameLocales,
            AddDatabase,  OpenDatabase,  CloseDatabase,  RemoveDatabase,  RenameDatabase, 
            AddLocaleEd,  OpenLocaleEd,  CloseLocaleEd,  RemoveLocaleEd,  RenameLocaleEd,
            AddVrResx,    RemoveVrResx,  OpenVrResx,     CloseVrResx,
            AddMedia,     RemoveMedia,   AddReference
        }

        public enum MaxResults { None, OK, OkRemove, Error }

        public MaxProjectEventArgs(MaxEventTypes type, MaxResults res, string path, string tempfile)
        {
            maxEventType = type; result = res; projectPath = path; tempFilePath = tempfile;
        }

        public MaxProjectEventArgs(MaxEventTypes type, MaxResults res, string path)
        {
            maxEventType = type; result = res; projectPath = path; 
        }

        public MaxProjectEventArgs(MaxEventTypes type, string path)
        {
            maxEventType = type; projectPath = path; result = MaxResults.OK;
        }

        public MaxProjectEventArgs(MaxEventTypes type)
        {
            maxEventType = type; result = MaxResults.OK;
        }

        public MaxProjectEventArgs(MaxEventTypes type, string name, string path)
        {
            maxEventType = type; result = MaxResults.OK;
            ScriptName = name; ScriptPath = path; 
        }

        public MaxProjectEventArgs(MaxEventTypes type, string name, string path, string trigger)
        {
            maxEventType = type; result = MaxResults.OK;
            ScriptName = name; ScriptPath = path; Trigger = trigger;
        }

        public MaxProjectEventArgs(MaxEventTypes type, string name, string path, CultureInfo locale)
        {
            maxEventType = type; result = MaxResults.OK;
            ScriptName = name; ScriptPath = path; MediaLocale = locale;
        }

        public void SetOK()     { result = MaxResults.OK; }

        protected MaxEventTypes maxEventType;
        public    MaxEventTypes MaxEventType { get { return maxEventType; } }

        protected MaxResults result;
        public    MaxResults Result   { get { return result;} set { result = value; } }

        public    string ScriptName   { get { return tempFilePath;} set { tempFilePath = value;} }
        public    string ScriptPath   { get { return projectPath; } set { projectPath  = value;} }
        public    bool   Active       { get { return intA != 0;   } set { intA = value? 1: 0;  } }
        public    bool   Invalid      { get { return intA != 0;   } set { intA = value? 1: 0;  } }
        public    string Trigger      { get { return stringA;     } set { stringA      = value;} } 
        public    string OldName      { get { return tempFilePath;} set { tempFilePath = value;} }
        public    string NewName      { get { return projectPath; } set { projectPath = value; } }
        public    CultureInfo MediaLocale  { get { return mediaLocale; } set { mediaLocale = value; } }
        protected string stringA;  
        protected int    intA;     

        protected string tempFilePath;
        public    string TempFilePath { get { return tempFilePath;} }
 
        protected string projectPath;
        public    string ProjectPath  { get { return projectPath; } }

        protected CultureInfo mediaLocale;
    }



    public class MaxFrameworkEventArgs: EventArgs
    {
        public enum MaxEventTypes 
        { 
            None, Start, Started, Shutdown, SuspendLayout, ResumeLayout, IdeModified
        }

        public MaxFrameworkEventArgs(MaxEventTypes type)
        {
            maxEventType = type;  
        }

        public enum MaxResults      { None, OK, Error, Saving }
        protected MaxResults result;
        public    MaxResults Result { get { return result;} set { result = value;} }

        protected MaxEventTypes maxEventType;
        public    MaxEventTypes MaxEventType 
        { get { return maxEventType; } set { maxEventType = value; } }
    }


    public class MaxCanvasEventArgs: EventArgs    
    {
        public enum MaxEventTypes { None, Add, Remove, Rename, Tray }

        public MaxCanvasEventArgs(MaxEventTypes type, string aname, string cname, bool isprime)
        {
            maxEventType = type; appName = aname; canvasName = cname; 
            isPrimary = isprime; isActive = true;
            result = MaxResults.OK;
        }

        protected string appName;  
        public    string AppName     { get { return appName;     } }

        protected string canvasName; // Same as app name, if primary
        public    string CanvasName  { get { return canvasName;  } }

        protected bool isPrimary;
        public    bool IsPrimary     { get { return isPrimary; } }
        public    bool IsTrayVisible { get { return isPrimary; } set { isPrimary = value;} }

        protected MaxEventTypes maxEventType;
        public    MaxEventTypes MaxEventType { get { return maxEventType;} }

        protected bool isActive;
        public    bool IsActive      { get { return isActive; } set { isActive = value;} }

        protected string newName;
        public    string NewName     { get { return newName;  } set { newName = value; } }

        public enum MaxResults       { None, OK, Error }
        protected   MaxResults result;
        public      MaxResults Result{ get { return result;   } set { result = value;  } }
        public void SetError()       { result = MaxResults.Error; }
    }



    public class MaxNodeEventArgs: EventArgs     
    {
        public enum MaxEventTypes { None, Add, Remove, Rename }

        public MaxNodeEventArgs(MaxEventTypes etype, Max.Drawing.NodeTypes ntype, 
        string name, string group, long id)
        {
            maxEventType = etype; nodeType = ntype; nodeName = name; nodeID = id;
            groupName = group; result = MaxResults.OK;
        }

        public MaxNodeEventArgs(MaxEventTypes etype, Max.Drawing.NodeTypes ntype, 
        string name, string group, string cname, long id)
        {
            maxEventType = etype; nodeType = ntype; nodeName  = name; 
            canvasName   = cname; nodeID   = id;    groupName = group;
            result = MaxResults.OK;
        }

        public MaxNodeEventArgs(MaxEventTypes etype, string name, string cname) 
        { 
            maxEventType = etype; nodeName = name; canvasName = cname;
            result = MaxResults.OK;
        }
                                                    
        public MaxNodeEventArgs(MaxEventTypes etype, string aname, string cname, long id) 
        { 
            maxEventType = etype; appName = aname; canvasName = cname; nodeID = id;
            result = MaxResults.OK;
        }

        public MaxNodeEventArgs(MaxEventTypes etype, string appname,
        Max.Drawing.NodeTypes ntype, string name, string group, string cname, long id)
        {
            maxEventType = etype; nodeType = ntype; nodeName  = name; 
            canvasName   = cname; nodeID   = id;    groupName = group;
            appName      = appname; result = MaxResults.OK;
        }

        protected MaxEventTypes maxEventType;
        public    MaxEventTypes MaxEventType { get { return maxEventType; } }

        protected NodeTypes nodeType;
        public    NodeTypes NodeType { get { return nodeType;  } }

        protected long nodeID;
        public    long NodeID        { get { return nodeID;    } }

        protected string nodeName;
        public    string NodeName    { get { return nodeName;  } }

        protected string groupName;
        public    string GroupName   { get { return groupName; } }

        protected string canvasName;
        public    string CanvasName  { get { return canvasName;} }

        protected string appName;
        public    string AppName     { get { return appName;   } }

        public enum MaxResults       { None, OK, Error }
        protected   MaxResults result;
        public      MaxResults Result{ get { return result; } set { result = value;  } }
        public void SetError()       { result = MaxResults.Error; }
    }


    public class MaxMenuOutputEventArgs: EventArgs
    {
        public enum MaxEventTypes 
        {
            None, Selected, CanUndo, CanRedo, CanPaste, CanDelete, CanSelectAll,
            IsTrayShown, IsFunctionCanvas,  
        }
    
        public MaxMenuOutputEventArgs(MaxEventTypes type) 
        {
                eventType = type; 
        }
        public MaxMenuOutputEventArgs(MaxEventTypes type, bool tf) 
        {
                eventType = type; val = tf; 
        }
        public MaxMenuOutputEventArgs(MaxEventTypes type, bool tf, bool ro) 
        {
                eventType = type; val = tf; readOnly = ro;
        }

        protected bool val;
        public    bool Value    { get { return val;      } }
        protected bool readOnly;
        public    bool ReadOnly { get { return readOnly; } }
        protected MaxEventTypes eventType;
        public    MaxEventTypes MaxEventType { get { return eventType; } }
    }  



    public class MaxOutputWindowEventArgs: EventArgs
    {
        public enum MaxEventTypes 
        { 
            None, WriteLine, Clear
        }

        public MaxOutputWindowEventArgs(string text)
        {
            maxEventType = MaxEventTypes.WriteLine; this.text = text;
        }

        public MaxOutputWindowEventArgs(MaxOutputWindowEventArgs.MaxEventTypes type)
        {
            maxEventType = type; this.text = String.Empty;
        }

        protected MaxEventTypes maxEventType;
        public    MaxEventTypes MaxEventType { get { return maxEventType;  } }
    
        protected string text;
        public    string Text { get { return text; } }    
    }


    public class MaxStatusBarOutputEventArgs: EventArgs
    {
        public enum MaxEventTypes 
        { 
            None, WriteLine
        }

        public MaxStatusBarOutputEventArgs(string text)
        {
            maxEventType = MaxEventTypes.WriteLine; this.text = text;
        }

        protected MaxEventTypes maxEventType;
        public    MaxEventTypes MaxEventType { get { return maxEventType;  } }
    
        protected string text;
        public    string Text { get { return text; } }   
    }

} // namespace
