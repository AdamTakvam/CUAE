using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Northwoods.Go;
using Metreos.Max.Drawing;
using Metreos.Interfaces;
using Metreos.Max.Framework.Satellite.Property;
using Metreos.Max.Core.Tool;
 

namespace Metreos.Max.Core
{
    /// <summary>Global constants</summary>
    public sealed class Const 
    {
        #region singleton
        private static readonly Const instance = new Const();
        public  static Const Instance { get { return instance; }}
        private Const() {} 
        #endregion

        public  const string dot      = ".";
        public  const string qmark    = "?";
        public  const string asterisk = "*";
        public  const string blank    = " ";
        public  const string dash     = "-";
        public  const string colon    = ":";
        public  const string bslash   = "\\";
        public  const string slash    = "/";
        public  const string dquote   = "\"";
        public  const string squote   = "'";
        public  const string lbrack   = "[";
        public  const string rbrack   = "]";
        public  const string lparen   = "(";
        public  const string rparen   = ")";
        public  const string atsign   = "@";
        public  const string tilde    = "~";
        public  const string crlf     = "\n";
        public  const string newline  = "\n";
        public  const string beep     = "\a";
        public  const string szero    = "0";
        public  const string sone     = "1";
        public  const string emptystr = "";
        public  const string semicol  = "\x003b";

        public  const char   cspace   = ' ';
        public  const char   cslash   = '/';
        public  const char   cdot     = '.';

        public  const string bdashb   = " - ";
        public  const string dotb     = ". ";
        public  const string blbrack  = " [";
        public  const string bsquote  = " '";
        public  const string blparen  = " (";
        public  const string on       = "On";
        public  const string spsp     = "  ";
        public  const string colonsp  = ": ";
        public  const string commasp  = ", ";
        public  const string doublesp = "\n\n";
        public  const string user32   = "user32.dll";  
        public  const string kernel32 = "kernel32.dll";
        public  const string shlwapi  = "Shlwapi.dll";
        public  const string AppServerProcessName = "AppServer";

        public  static bool  IsNativeIDE { get { return true; } }

        public  const string productName = "Cisco Unified Application Designer";
        public  const string dialogTitle = "Cisco Unified Application Designer";

        public  static readonly Microsoft.Win32.RegistryKey regMainKey 
            = Microsoft.Win32.Registry.LocalMachine;
        public  static readonly string regMetKey = "Software\\" + Application.CompanyName;
        public  static readonly string regMaxKey =  Application.ProductName;
        public  const string regRflKey = "Recent";
        public  const string regLocKey = "Location";
        public  const string regOptKey = "Options";
        public  const string regDbgKey = "Debug";

        public  const string defaultPackagesFolderName       = "\\Packages";
        public  const string defaultPackagesSubdirectory     = "\\1.0\\Packages";
        public  const string packagesSubdirectory            = "\\Packages";
        public  const string defaultFrameworkFolderName      = "\\Samoa";
        public  const string defaultFrameworkVersion         = "1.0";
        public  const string defaultAppVersion               = "1.0";
        public  const string defaultLocalLibrariesFolderName = "\\Local";
        public  const string projectTempFileName             = "\\project.tmp";
        public  const string projectDbFolderName             = "\\db";
        public  const string projectBinFolderName            = "\\bin";
        public  const string projectObjFolderName            = "\\obj";
        public  const string projectMedFolderName            = "\\med";
        public  const string projectBakFolderName            = "\\bak";

        public  const string AppServerIpSubkey     = "AppServerIP";
        public  const string AppServerPortSubkey   = "AppServerPort";
        public  const string ConsolePortSubkey     = "ConsolePort";
        public  const string DebuggerPortSubkey    = "DebuggerPort";
        public  const string AdminUserSubkey       = "AdminUser";
        public  const string SshTimeOutkey         = "SshTimeOut";
        public  const string AdminPassSubkey       = "~";
        public  const string PackagesSubkey        = "PackagesFolder";
        public  const string FrameworkSubkey       = "FrameworkFolder";
        public  const string FrameworkVerSubkey    = "FrameworkVersion";
        public  const string LocalLibrariesSubkey  = "LocalLibrariesFolder";
        public  const string PlacementSubkey       = "WindowPlacement";
        public  const string DefPropTypeSubkey     = "DefaultPropertyType";
        public  const string SuppressAutoNewScript = "SuppressAutoNewScript";
        public  const string SuppressAutoNewProject= "SuppressAutoNewProject";
        public  const string AppDepUsernamekey     = "AppDepUsername";
        public  const string AppDepPasswordkey     = "AppDepPassword";
        public  const string SshPortkey            = "SshPort";
        public  const string EncryptionKey         = "xs2fyG0aD651Fcc"; 
        public  const string defaultProjectName    = "Application Designer Project";

        public  const string MaxMruSubkey          = "MaxMru";
        public  const string LoadOnStartupSubkey   = "LoadOnStartup";
        public  const string CopyMediaLocalSubkey  = "CopyMediaLocal";
        public  const string LinkStyleSubkey       = "LinkStyle";
        public  const string LinkWidthSubkey       = "LinkWidth";
        public  const string LinkOrthoSubkey       = "LinkOrtho";
        public  const string LinkUnconSubkey       = "LinkUncon";
        public  const string GridWidthSubkey       = "GridWidth";
        public  const string GridHeightSubkey      = "GridHeight";
        public  const string GridSnapSubkey        = "GridSnap";
        public  const string WarnAsErrorSubkey     = "WarnAsError";
        public  const string CallAsListSubkey      = "CallAsList";
        public  const string PortMotionSubkey      = "PortMotion";
        public  const string PortLargeSubkey       = "PortLarge";
        public  const string PortVisibleSubkey     = "PortVisible";
        public  const string DisplayNamesSubkey    = "DisplayNames";

        public  const string ConsoleBkgndSubkey    = "ConsoleBackground";

        public  const string cmdlineParamBuild     = "b";

        public  const int    DefaultClientWidth    = 1000;
        public  const int    DefaultClientHeight   = 750;
        public  const int    retcodeCmdLineError   = -1;
        public  const int    retcodeBuildError     = -2;

        public  const string LinkObjectDisplayName    = "Link ";
        public  const string AnnotObjectDisplayName   = "Annotation ";
        public  const string LoopObjectDisplayName    = "Loop ";
        public  const string CommentObjectDisplayName = "Comment ";
        public  const string LabelObjectDisplayName   = "Label ";
        public  const string ScriptObjectDisplayName  = "Script ";
        public  const string ProjectObjectDisplayName = "Project ";

        public  const string localhost = "127.0.0.1";
        public  const string ipzero    = "0.0.0.0";

        public  const string CurrentProjectFileVersion = "0.2";
        public  static readonly float CurrentProjectFileVersionF 
            = Convert.ToSingle(CurrentProjectFileVersion);
        public  static readonly float LowestSupportedProjectFileVersionF = 0.2F;

        public  const string CurrentAppFileVersion = "0.8"; // 1016
        public  static readonly float CurrentAppFileVersionF 
            = Convert.ToSingle(CurrentAppFileVersion);
        public  static readonly float LowestSupportedAppFileVersionF = 0.2F;
        public  static bool  IsPriorPt7(float f) { return f < 0.699F; }
        public  static bool  IsPriorVersion08(float f) { return f > 0.009F && f < 0.799F; }

        public  const  string CannotOpenScriptMsg = "Cannot open script";
        public  const  string MissingAuthItemsMsg 
            = "The following authentication entries are missing or invalid:\n";
        public  const  string DefaultAuthItemsMsg 
            = "Please enter credentials for the server to which you want to deploy this application:";
        public  const  string PasswordAsterisks = "*******";
        public  const  string missingIP   = "IP address";
        public  const  string missingUser = "user name";
        public  const  string missingPass = "password";
        public  const  string missingPort = "port";
        public  const  string badUsername = "User Name format is invalid as entered"; 
        public  const  string badPassword = "Password format is invalid as entered"; 
        public  const  string badIpAddr   = "IP Address is invalid as entered"; 
        public  const  string badPort     = "Port number is invalid as entered";
        public  const  string badTime     = "SSH Connection Time Out is invalid as entered";
        public  const  string badEntryDlgTitle = "Invalid Entry"; 
        public const string badProjectNameMsg 
            = "Project name may contain only letters, digits, and underscore characters";
        public const string badProjectPathMsg 
            = "Project location directory path is invalid as entered";
        public static readonly char[] invalidPasswordCharacters = new char[] 
          { ' ', '*', '<', '>', '.', ',', '\\', '"', ';', '|', '/', '=', '+', '(', ')', '`', '\''} ;

        public  static string UnsupportedVersionMsg(string path, string version)
        {
            return "File '" + path + "'\n is unsupported version " + version;
        }

        public static string References  = "References";
        public static string Reference   = "Reference";
        public static string Installer   = "Installer";
        public static string Locales     = "Locales";
        public static string Databases   = "Databases";
        public static string Media       = "Media";
        public static string Grammar     = "Grammar";
        public static string VrResource  = "Voice Recognition Resources";
        public static string TtsText     = "TTS text";

        public static string AppLiteral  = "Application ";
        public static string Database    = "Database ";
        public static string MediaFile   = "Media file ";
        public static string GrammarFile = "Grammar file ";
        public static string VrResxFile  = "VR resource file ";
        public static string TtsTextFile = "TTS text file ";

        public static string InstallerFileName = Installer;
        public static string LocalesFileName   = Locales;
        public const string addremDefaultDescr = "Description: not specified.";
        public  const string UnknownServiceName   = "Unknown";

        public  const string CanvasClipDataFormat = "MaxDesigner.Canvas";

        public  const string AppIconNamespacePath      = "MaxDesigner.Resources.max.ico";
        public  const string maxPackagesWildcard       = "*.xml";
        public  const string maxPackageExtension       = ".xml";
        public  const string maxAssemblyExtension      = ".dll";
        public  const string tempfileExtension         = ".tmp";
        public  const string maxProjectFileExtension   = ".max";
        public  const string maxScriptFileExtension    = ".app";
        public  const string maxIdeFileExtension       = ".ide";
        public  const string maxToolboxFileExtension   = ".tbx";
        public  const string maxBuildFileExtension     = ".xml";
        public  const string maxDeployFileExtension    = ".mca";
        public  const string VoxFileExtension          = ".vox";
        public  const string WavFileExtension          = ".wav";
        public  const string WsdlFileExtension         = ".wsdl";
        public  const string XsdFileExtension          = ".xsd";
        public  const string GrammarFileExtension      = ".grxml";
        public  const string UserdictFileExtension     = ".userdict";
        public  const string maxInstallerFileExtension = ".installer";
        public  const string maxLocalesFileExtension   = ".locale";
        public  const string maxDatabaseFileExtension  = ".sql";
        public  const string BuildErrorsFilename       = "errors.dat";
        public  const string CompileErrorsFilename     = "compileerrors.dat";
        public  const string MaxProjectFilter   = "Designer Project Files (*.max)|*.max|All files (*.*)|*.*";
        public  const string MaxScriptFilter    = "Designer Script Files (*.app)|*.app|All files (*.*)|*.*";
        public  const string MaxInstallerFilter = "Designer Installer Files (*.installer)|*.installer|All files (*.*)|*.*";
        public  const string MaxLocalesFilter   = "Designer Locale Files (*.locale)|*.locale|All files (*.*)|*.*";
        public  const string MaxDatabaseFilter  = "Designer Database Script Files (*.sql)|*.sql|All files (*.*)|*.*";
        public  const string MaxAudioFilter     = "Designer Audio Files (*.vox;*.wav)|*.vox;*.wav|All files (*.*)|*.*";
        public  const string MaxVrResourceFilter= "Voice recognition resources (*.grxml;*.xml;*.*)|*.*|All files (*.*)|*.*";
        public const  string MaxReferenceFilter = "Custom Type Libraries (*.xml;*.dll)|*.xml;*.dll|All files (*.*)|*.*";
        public  const string MaxPackageFilter   = "Designer Package Files (*.xml;*.dll)|*.xml;*.dll|All files (*.*)|*.*";
        public  const string MaxWsdlFilter      = "Web Services Description Files (*.wsdl)|*.wsdl|All files (*.*)|*.*";

        public static long nodeID = System.DateTime.Now.Ticks;
        public long   NextNodeID  { get { return ++nodeID == 0? ++nodeID: nodeID; } }

        public  const float  ErrValXY = -4096;
        public  const long   ErrValNodeID = 0;
        public  const int    ErrValTreeLevel = -1;
    
        // Default length of time a message will stay on the status bar
        public const int statusBarMessageDurationMs = 10000;
        public const int VariablesGroupHideDelayMs  = 2000;

        public const string stockPackageName        = "Metreos.StockTools";
        public const string metreosTypesPrepend     = "Metreos.Types";
        public const string defaultStockToolGroup   =  stockPackageName;
        public const string stubPackageDescription  = "Internal stub";
        public const string packageLiteralUC        = "Package";
        public const string NameOfCallFunction      = "CallFunction";

        public const string defaultCodeToolName     = "CustomCode";
        public const string defaultVariableToolName = "Variable";
        public const string defaultFunctionToolName = "Function";
        public const string defaultCommentToolName  = "Comment";
        public const string defaultAnnotToolName    = "Annotation";
        public const string defaultStartToolName    = "Start";
        public const string defaultLoopToolName     = "Loop";
        public const string defaultLabelToolName    = "Label";
        public const string defaultNullToolName     = "Null";

        public const string defaultCodeToolDesc     = "User-created custom code";
        public const string defaultVariableToolDesc = "Global or local variable";
        public const string defaultFunctionToolDesc = "A user-defined function";
        public const string defaultCommentToolDesc  = "A sticky-note style comment";
        public const string defaultAnnotToolDesc    = "A sticky note attached to a node";
        public const string defaultStartToolDesc    = "Marks the entry point of a function";
        public const string defaultLoopToolDesc     = "Container describing a programmatic loop";
        public const string defaultLabelToolDesc    = "A named program branch point";
        public const string defaultNullToolDesc     = "Placeholder tool";

        public const string defaultProjectFilename  = "project";
        public const string defaultAppName          = "script";
        public const string defaultDatabaseName     = "database";
        public const string defaultNodeName         = "Script Node";
        public const string defaultFunctionName     = "function";
        public const string defaultVariableName     = "variable";
        public const string defaultConfigName       = "configItem";
        public const string variablesGroupName      = "Variables";

        public const string initialActionLinkLabelText = "default";

        public const string methodArgs
         = @"(\s*)((ref|out)?)(\s*)((params)?)(\s*)([a-zA-Z_0-9.]+)(\s+)((?<name>\w+))(\s*)";

        public const string variablesTrayText = "(tray empty)";
        public static readonly PointF trayVariableNodeLoc = new PointF(0,0);

        public static readonly PointF EditPasteNodeOffset = new PointF(32,32);

        public static readonly PointF ExplorerProjectNodeIconPos = new PointF(5,0);
        public static readonly PointF ExplorerProjectNodeTextPos = new PointF(24,0);

        public static readonly Point AppTreeOffset    = new Point(0,2);
        public const int    AppTreeItemHeight         = 19;
        public const string AppTreeRootFolderName     = "Events and Functions";
        public const string AppTreeVariableFolderName = "Variables";

        public const string toolsOptionsTabNameGeneral    = "General";
        public const string toolsOptionsTabNameGraphs     = "Graphs";
        public const string toolsOptionsTabNameAppServer  = "Connectivity";
        public const string toolsOptionsTabNameBuild      = "Build";
        public const string toolsOptionsTabNameDebugger   = "Debug";

        public const string debugWatchTypeNameString      = "String";
        public const string debugWatchTypeNameInteger     = "Integer";
        public const string debugWatchTypeNameLong        = "Long";
        public const string debugWatchTypeNameDouble      = "Double";
        public const string debugWatchTypeNameComplex     = "Complex";
        public const string debugWatchTypeValueComplex    = "[complex type]";
        public const string debugWatchNullValue           = "[undefined value]";

        public const int    debugServerPingTimeoutMs = 3500;
        public const int    debugActivityIntervalMs  = 12500;

        public const string scopeGlobal = "Global";
        public const string scopeLocal  = "Local";
        public const DataTypes.UserVariableType defPropTypeStr = DataTypes.UserVariableType.literal;
        public const DataTypes.UserVariableType defPropTypeVar = DataTypes.UserVariableType.variable;
   
        public static readonly Color ColorMaxBackground   = Color.WhiteSmoke;
        public static readonly Color ColorDebugBackground = Color.FromArgb(245,245,255);  
        public static readonly Color ExtralightSlateGray  = Color.FromArgb(0x9f,0xaf,0xbf);
        public static readonly Color linkColorDark        = Color.Black;  
        public static readonly Color linkColorNormal      = Color.SlateGray;          
        public static readonly Color linkColorLight       = Color.LightSlateGray;
        public static readonly Color linkColorExtraLight  = ExtralightSlateGray;
        public static readonly Color linkLabelColor       = Color.SlateGray;
        public static readonly Color ColorNodeItemBack    = Color.FromArgb(241,241,242);
        public static readonly Color ColorNodeItemText    = Color.FromArgb(0x70,0x80,0x90);
        public static readonly Color ColorNodeItemBorder  = Color.FromArgb(0x7a,0x8a,0x9a); 
        public static readonly Color DialogMessageBlue    = Color.FromArgb(96,96,240);

        public const int breakpointIconOffsetX = 10;
        public const int breakpointIconOffsetY = 10;

        public static string buttonTextWizNext   = "Next >>";
        public static string buttonTextWizFinish = "Finish";

        public const string calledFunctionPlaceholderText = "<function name?>";
        public const string debuggerPingIdKey = "ping";

        public const string InitialDatabaseText = "\nCREATE TABLE sample" 
            + "\n(\n  samplekey  INT PRIMARY KEY,\n  sampledata VARCHAR(25)\n)" + semicol;

        public static string UpgradedMsg(string appname) 
        {
            return "Script '" + appname + "' has been upgraded from an earlier version." +
                "\nThis project file will be updated and saved prior to opening."; 
        }

        public const string UpgradedDlgTitle  = "Application Designer Upgrade";

        public const string ConsoleStoppedMsg = "Console is stopped";
                                                                         
        public static string ExplorerProjectString(string name)       
        {
            return "Project '" + name + "'";    
        }

        public static string LinkLabelUnconditional = "unconditional";

        public static string[] DefaultActionLinkLabelChoices = new string[]
            { "default"};

        public  static string UserDataPath      // User data directory incl version
        {
            get { return Application.LocalUserAppDataPath; } 
        }

        public  static string UserDataRoot      // User data directory above version
        {
            get { return Directory.GetParent(UserDataPath).FullName; } 
        }

        public static string DefaultPackagesFolder 
        {
            get { return UserDataRoot + defaultPackagesFolderName; } 
        }

        public static string DefaultFrameworkFolder 
        {
            get { return UserDataRoot + defaultFrameworkFolderName; } 
        }

        public static string DefaultLocalLibrariesFolder
        {
            get { return UserDataRoot + defaultLocalLibrariesFolderName; } 
        }

        private static readonly string regLocation 
            = regMetKey + bslash + regMaxKey + bslash + regLocKey;
        public  static string RegistryLocationKey { get { return regLocation; } }

        private static readonly string regDebug 
            = regMetKey + bslash + regMaxKey + bslash + regDbgKey;
        public  static string RegistryDebugKey    { get { return regDebug; } }

        private static readonly string regOptions 
            = regMetKey + bslash + regMaxKey + bslash + regOptKey;
        public  static string RegistryOptionsKey  { get { return regOptions; } }

        private static readonly string regRecentProjects 
            = regMetKey + bslash + regMaxKey + bslash + regRflKey;
        public  static string RegistryRecentProjectsKey { get { return regRecentProjects; } }

        private static readonly string regPrintSetup 
            = regMetKey + bslash + regMaxKey + bslash + PrintSetup.PrintSetupKey;
        public  static string RegistryPrintSetupKey { get { return regPrintSetup; } }

        private static string appTitleDirty;
        private static string appTitleNormal;
        public  static string AppTitleDirty  { get { return appTitleDirty;  } }
        public  static string AppTitleNormal { get { return appTitleNormal; } }
        private static string productNameDash = productName + bdashb;
        private static string projectTitleString;
        public  const  string projectSavingStatusMsg = "Saving ...";
        public  const  string projectSavedStatusMsg  = "Saved";

        public  const  string toolsOptionsDlgTitle = "Options";
        public  static readonly Size toolsOptionsControlSize = new Size(395,289);

        public static void MakeMaxTitle(string projectName)
        {
            if (projectName == null)
            {
                appTitleNormal = productName;
                appTitleDirty  = productName;
            }
            else
            {
                appTitleNormal = productNameDash + projectName;
                appTitleDirty  = appTitleNormal  + asterisk;
                projectTitleString = appTitleNormal;
            }
        }


        public static void MakeMaxTitle(string projectName, string appName)
        {
            if (projectName == null || appName == null)
                MakeMaxTitle(projectName);
            else
            {
                appTitleNormal = projectTitleString + blbrack + appName + rbrack;
                appTitleDirty  = appTitleNormal + asterisk;
            }
        }

        public static string GetPromptSaveChangesTo(string project)
        {
            return promptSaveChangesTo + project + qmark;
        }

        public static string MsgPackageLoaded(string name, int count, string path)
        {            
            return "Loaded package " + DisplayName(name) + blparen + count + cpfrom + path;
        }

        public static string MsgPackageUnloaded(string name, int count, string path)
        {
            return "Unloaded package " + DisplayName(name) + blparen + count + cpfrom + path;
        }

        public static string MsgNativeTypeLoaded(string name, int count, string path)
        {
            return "Loaded native types from " + DisplayName(name) + blparen + count + cpfrom + path;
        }

        public static string MsgNativeTypesUnloaded(string name, int count, string path)
        {
            return "Unloaded native types from " + DisplayName(name) + blparen + count + cpfrom + path;
        }

        public static string DisplayName(string name)
        {
            return name == null || name == String.Empty || name.StartsWith(Const.blank)? unnamed: name;
        }

        public static string CouldNotDetermineRefMsg(string exception)
        {
           return "Could not determine reference count for selected node\n" + exception;
        }

        public const string CouldNotDetermineRefDlgTitle = "Delete Complex Node";

        public const string unnamed = "unnamed";
        public const string cpfrom  = ") from ";

        public static string DebuggerConnectionLostMsg(string ip, string port)
        {
            StringBuilder s = new StringBuilder("Connection lost to remote server ");
            s.Append(ip); s.Append(Const.slash); s.Append(port);
            s.Append(". Debug session ended.");
            return s.ToString();
        }


        #region OrphanHanderMsg
        #if(false)
        public const string OprphaningFuncWarning = "Orphaned Function Warning";

        public static string OrphanHanderMsg(IMaxNode deletenode, string funcname)
        {
            StringBuilder s = new StringBuilder("Deleting the ");
            s.Append(deletenode.NodeName); 
            s.Append(" node will render function ");
            s.Append(funcname);
            s.Append(" unreachable,\nsince the only references to that function");
            s.Append(" remaining in the script exist within\nthat function.");
            s.Append(" Should you later wish to delete any of those remaining nodes,");
            s.Append("\nyou must re-insert another reference to that function, such as another\n");
            s.Append(deletenode.NodeName);
            s.Append(" node, outside of function ");
            s.Append(funcname);                                                                                                  
            return s.ToString();
        }
        #endif
        #endregion

        public static readonly Point FunctionCanvasStartNodeDropPoint 
            = new System.Drawing.Point(32,32);

        public static readonly Point HandlerStubOffset 
            = new System.Drawing.Point(63,-16);  

        public static readonly Point AsyncActionHandlerStubOffset 
            = new System.Drawing.Point(102,40);

        public static readonly Point      point00   = new System.Drawing.Point(0,0);
        public static readonly RectangleF nullRect  = new RectangleF(point00, size0x0);

        public static readonly SizeF portSizeNormal = new SizeF(8,8);    
        public static readonly SizeF portSizeLarge  = new SizeF(10,10);                                                   
        public static readonly Brush portBrush = new SolidBrush(Color.FromArgb(160,0x2f,0x4f,0x4f)); 
        public static readonly Pen   portPen   = Pens.White; 

        public const  string NodeRenameDialogTitle = "Rename Node";
        public static string CannotRenameMsg(bool isFunction) 
        {
            return (isFunction? "Function": "Node") 
                + " cannot be assigned the name specified.\nPlease choose a different name.";
        }

        public  const string canvasDeleteMsg        = " will be deleted from the project.";
        private const string variableDeleteMsg      = " will be removed from the project";
        private const string canvasLiteral          = "Canvas ";
        private const string functionLiteral        = "Function ";
        private const string variableLiteral        = "Variable ";
        private const string canvasesLiteral        = "Canvas(es) and Handler(s) ";
        public  const string missingEventDlgTitle   = "Event Not Found";
        public  const string installPkgDlgTitle     = "Install Package";
        public  const string canvasDeleteDlgTitle   = "Delete Canvas";
        public  const string handlerDeleteDlgTitle  = "Delete Event Handlers";
        public  const string removeTabDlgTitle      = "Remove Toolbox Tab";
        public  const string removeToolDlgTitle     = "Remove Toolbox Item";
        public  const string packageBrowseDlgTitle  = "Browse for Package";
        public  const string maxShutdownDlgTitle    = "Closing Application Designer";
        public  const string ReplaceTriggerDlgTitle = "Replace Triggering Event";
        public  const string RenameThandlerDlgTitle = "Rename Trigger Handler";
        public  const string RenameHandlerDlgTitle  = "Rename Async Action Handler";
        public  const string RenameFunctionDlgTitle = "Rename Function";
        public  const string InvalidConfItemDlgTitle= "Invalid Config Item";
        public  const string NewConfItemDlgTitle    = "New Configuration Item";
        public  const string NewConfValueDlgTitle   = "New Configuration Value";
        public  const string DuplicateResxNameMsg   = "Resource file name must be unique";
        public  const string DuplicateResxDlgTitle  = "Duplicate Resource Name";
        public  const string treenodeRemovalErrMsg  = " could not be removed from app tree";
        public  const string LocalizableEditorTopLeftCell = "String ID";
    
        public  const string ShowCallsDlgTitle      = "Show Calls";
        public  const string ShowReferencesDlgTitle = "Show References";
        public  const string ShowCallsToLabel       = "Calls to ";
        public  const string ActionsReferencingLabel= "References to ";
        public  const string ProjectFolderText      = "Project folder ";

        public static string RenameHandlerAtTreeMsg(bool isDelete)
        { 
            return "Handler has multiple references and must be " + (isDelete? "deleted": "renamed")
                + " at its specific action";
        }

        public  const string missingEventMsg 
            = "No event found in any accessible package\nwith the name ";
    
        public static string GetBuildCompleteMessage(int ecount, int wcount)
        {
            if (ecount < 0 || wcount < 0)
                return "Errors encountered during build";
            else if (ecount == 0)
                return "Build successful";
            else
            {
                string errormsg   = ecount == 1? ecount + " error ":   ecount + " errors, ";                       
                string warningmsg = wcount == 1? wcount + " warning ": wcount + " warnings ";              
                return errormsg + warningmsg + "encountered during build";
            }
        }

        public static string GetDeployCompleteMessage(int ecount)
        {
            return ecount  < 0? "Errors encountered during deploy": 
                   ecount == 0? "Deployment successful": 
                   ecount + " errors encountered during deploy";
        }

        public static string CantUndoWarning 
            = "Note that content which may exist in deleted function(s)\n"
            + "will not be restored with any subsequent Undo command."; 

        public static string DeleteCanvasMessage(string canvasname)
        { 
            return canvasLiteral + canvasname + canvasDeleteMsg 
                + doublesp + CantUndoWarning;
        } 

        public static string DeleteCanvasesMessage(string canvasnames)
        { 
            return canvasesLiteral + crlf + canvasnames + canvasDeleteMsg.Substring(1) 
                 + doublesp + CantUndoWarning;
        } 

        public static string DeleteFunctionMessage(string funcname)
        { 
            return functionLiteral + funcname + canvasDeleteMsg 
                + doublesp + CantUndoWarning;
        } 

        public static string DeleteVariableMessage(string varname)
        {
            return variableLiteral + varname + variableDeleteMsg;
        }

        public static string TreenodeRemovalErrorMessage(MaxAppTreeNode node)
        { 
            return dquote + node.Text + dquote + treenodeRemovalErrMsg;
        }

        public static string MissingEventErrorMessage(string name)
        { 
            return missingEventMsg + dquote + name + dquote;
        }

        public static string LoadedTypesPackagesMessage(int packagecount)
        {
            return "Loaded " + packagecount + " native type packages.";
        }

        public static string LoadedPackagesMessage(int packagecount)
        { 
            return "Loaded " + packagecount + " packages";
        } 

        public static string AlreadyExistsMessage(string path)
        { 
            return path + " already exists.\nDo you want to replace it?";
        }

        public static string AlreadyInstalledMessage(string type, string path)
        { 
            string name = type == null? path: type + blank + path;
            return name + " is already installed.\nDo you want to replace it?";
        }    

        public static string CouldNotSetBreakpointsMsg(int errcount, int totcount)
        { 
            return errcount + " of " + totcount + " breakpoints could not be set";
        }    

        public static string DuplicateDatabaseMessage(string dbname)
        {
            return Const.squote + dbname + Const.squote + Const.blank + "database creation script already exists.";
        }

        private const string functionDeleteMsg1         = "References to this function will be removed from ";
        private const string functionDeleteMsg2         = " action(s)";
        public  const string functionDeleteDlgTitle     = "Remove Function";
        public  const string variableDeleteDlgTitle     = "Remove Variable";
        public  const string unsolicitedEventDeleteDlgTitle = "Delete Unsolicited Event";
        public  const string MalformedAssemblyTitle     = "Malformed Assembly";
        public  const string FrameworkCorruptedTitle    = "Framework Version Mismatch";
        public  const string GenericErrorTitle          = "Unexpected Error";

        public  static string functionDeleteMsg(int n)
        { 
            return functionDeleteMsg1 + n + functionDeleteMsg2;
        }

        public  static string unsolicitedEventDeleteMsg(string name, int n)
        { 
            return "Selected event, handler " + name + ", and " + n + " node(s), will be deleted";
        } 

        public  static string deleteOrphanedFunctionMsg(string name, int n)
        { 
            return "Function '" + name + "' has no remaining references.\n"
                + "Delete " + name + " and " + n + " node(s)?";
        }  

        public  static string deletePopulatedFunctionMsg(string name, int n)
        { 
            return "Function canvas '" + name + "' and " + n + " node(s), will be deleted.\n\n"
                  + CantUndoWarning;
        } 

        public static string deleteNativeActionsToolboxMsg(string name, int n)
        {
            System.Diagnostics.Debug.Assert(n > 0, "Invalid dialog usage for 0 toolbox entries");

            if(n > 1) 
                return "Selected action package '" + name + "' and " + n + " toolbox entries, will be deleted";            
            else
                return "Selected action package '" + name + "' and 1 toolbox entry, will be deleted";
        }

        public static string deleteNativeActionsToolboxTitle(int n)
        {
            System.Diagnostics.Debug.Assert(n > 0, "Invalid dialog usage for 0 toolbox entries");

            if(n > 1)   return  "Delete Toolbox Entries";
            else        return  "Delete Toolbox Entry";
        }

        public static string BadProjectFileMsg(string path)
        {
            return path + "\ncould not be opened";
        }

        public static string BadScriptFileMsg(string path)
        {
            return path + "\nis not a valid Application Designer script file";
        }

        public const string CouldNotPrintScriptMsg 
            = "Script print request failed. Please verify selected printer and retry."; 

        public const string CouldNotPrintScriptDlgTitle = "Print"; 

        private const string toolsDisabledMsg 
            = "Associated toolbox entries, if any, have been disabled.";

        private const string multipleTypesRestrictionMsg
            = "Native actions, provider actions, and native types must reside in separate libraries.";

        public static string MissingPackagesBlurb = "Listed package(s) is/are used by the project "
            + " but either a reference is missing, the referenced file was not found, "
            + " or the package is not installed. " + toolsDisabledMsg;

        public static string MissingTypePackagesBlurb = "Listed native types references(s) is/are used by the project "
            + " but either the accompanying native types package could not be found, "
            + " or the file was not a valid native types package.";

        public static string OnlyActionsTitle = "Native action, provider action, or event not found";

        public static string OnlyActionsDesc = "Only libraries containing native actions, provider actions, or events"
            + " can be be added in this context.";

        public const string DuplicateDatabaseTitle = "Duplicate Database";

        public const string outOfSync 
          = "This version of Application Designer is not compatible with the installed server framework."
          + "\nYou must reinstall Application Designer to correct the problem.";

        public const string outOfSyncDlg = "Version Error";

        public const string deployFailed = "The deployment process was unsuccessful.";
        public const string deployFailedDlg = "Deployment Failed";
        public const string deployFailedMsg = "Deployment failed";

        public static string PackageLoadErrMsg(string name, string path)
        {
            return "Package " + name + " could not be installed. Ensure that\n"  
                + path + "\ncontains a valid package description";
        }

        public static string NoPackageName = "The chosen package contains no name";

        public static string CouldNotAttachDebugger(string ip, string port)
        { 
            StringBuilder s = new StringBuilder("Could not attach debugger on configured server ");
            s.Append(ip); s.Append(Const.slash); s.Append(port); s.Append(Const.dot);
            s.Append("\nPlease verify that the configured server is started and available.");
            return s.ToString();
        }

        public static string CouldNotStartDebugger(string ip, string port, string script)
        { 
          StringBuilder s = new StringBuilder("Could not start a remote debug session with configured server ");
          s.Append(ip); s.Append(Const.slash); s.Append(port);
          s.Append(" script '"); s.Append(script); s.Append("'. ");
          s.Append("\nPlease verify that your script is deployed to,");
          s.Append(" and that connectivity exists to, the configured server.");
          return s.ToString();
        }

        public static string MalformedAssemblyDesc(params string[] typesFound)
        {
            if(typesFound == null || typesFound.Length < 2) return multipleTypesRestrictionMsg;

            StringBuilder sb = new StringBuilder("Assembly contains multiple types: ");
            int i = 0;
            foreach(string type in typesFound)
            {
                sb.Append(Const.squote);
                sb.Append(type);
                sb.Append(Const.squote);

                if (typesFound.Length > 2 && i < typesFound.Length - 2) sb.Append(", ");
                else 
                if (i < typesFound.Length - 1) sb.Append(" and ");    
  
                i++;
            }

            sb.Append(Const.dot + System.Environment.NewLine);
            sb.Append(multipleTypesRestrictionMsg);
            return sb.ToString();
        }

        public const  string DeleteLoopTitle = "Delete Loop";

        public static string DeleteLoopMsg(int nodecount)
        {
            return String.Format("Selected loop and {0} nodes will be deleted", nodecount);
        }

        public static string FrameworkCorruptedDesc(string errorMessage)
        {
            return errorMessage;
        }

        public static string GenericErrorDesc(Exception e)
        {
            return "An unexpected error occurred.\n " + e.Message;
        }
 
        public static string FailedAssemblyPath(string path, string scriptName)
        {
            return String.Format("Source code in error for script '{0}' can be found at" + 
                   System.Environment.NewLine + "\t{1}", scriptName, path);
        }

        public const  string CmdlinePathErrMsg    = "project path not valid";
        public const  string CmdlineSlashErrMsg   = "prepended slash expected"; 
        public const  string CmdlineNoPathMsg     = "no path supplied for build";
        public static string CmdlineErrMsg(int n, string s) 
        {
            return "\nMax command line arg[" + n + "]: " + s + crlf; 
        }

        public const  string NewAppDlgTitle       = "New Application Script";
        public const  string OpenAppDlgTitle      = "Open Application Script";
        public const  string NewDatabaseDlgTitle  = "New Database Script";
        public const  string DatabaseScriptDescr  = "Database Script '";
        public const  string DatabaseScriptPrompt = "Database Script Name";

        public const  string DbgFuncNameColHdr    = "Function";
        public const  string DbgActionNameColHdr  = "Action";
        public const  string DbgActionIdColHdr    = "ID";
        public const  string DbgVarScopeColHdr    = "Scope";
        public const  string DbgVarNameColHdr     = "Name";
        public const  string DbgVarTypeColHdr     = "Type";
        public const  string DbgVarValueColHdr    = "Value";

        public const  string CtbNameColHdr        = "Action";
        public const  string CtbNamespaceColHdr   = "Package";
        public const  string CtbDirectoryColHdr   = "Directory";
        public const  string SystemPackagesName   = "System Packages";

        public const  string WsdlServiceColHdr    = "Service";
        public const  string WsdlMethodColHdr     = "Method";

        public const  string NewToolboxTabTextPrefix = "New Tab ";
        public const  string AppComponentsTabName    = "Application Components";
        public const  string AppControlPackageName   = IActions.AppControlNamespace;

        public const  string WillBeRemovedProject    = " will be removed from project";
        public const  string RemoveFromProjectTitle  = "Remove from Project";

        public const  string ReplaceDbScript      = "Replace Existing Database Script";
        public const  string DbScriptWriteWarning = "Overwrite an existing database script for database";

        public const  string DependencyNotFound    = "Dependency not found";

        public const string NoTriggersMsgA 
            = "No triggering event exists in any currently resident package\n";
        public const string NoTriggersMsgB 
            = "No triggering event was specified\n";
        public const string NoTriggersMsgC 
            = "A triggering event could not be created\n";
        public const string ProjectCannnotStartMsg 
            = "An Application Designer project cannot be initiated\n";

        public const  string UrlNotResolvedMsg     = "URL could not be resolved";
        public const  string EmptyWebServiceMsg    = "WSDL file defines no methods";
        public const string InvalidWebServiceMsg  = "WSDL file is invalid"; // use BuildInvalidWSMsg whenever possible
        public const  string WsdlParseErrorMsg     = "WSDL file parse error"; // use BuildWsdlParseErrorMsg whenever possible
		public const  string WsdlXmlLoadError      = "Unable to load the WSDL as XML";
		public const  string XsdXmlLoadError       = "Unable to load the XSD as XML";
		public const  string DuplicateSvcNameMsg   = "Service name is in use.\nPlease choose a different name.";
        public const  string InvalidTabNameMsg     = "Toolbox tab name missing or invalid";
        public const  string InvalidServiceNameMsg = "Web service name missing or invalid";
        public const  string MissingWsdlExeMsg     = "Web service compiler not found.\n  Please download the Microsoft .NET 2.0 SDK, and place wsdl.exe in the path.";
        public const  string ToolboxTabAddErrorMsg = "Tab could not be added to toolbox";
        public const  string ToolboxToolsErrorMsg  = "No tools were added to toolbox"; 
        public const  string InvalidUrlErrorMsg    = "Invalid URL or path";
        public const  string UrlUnreachableMsg     = "URL could not be found";
        public const  string HttpCommErrorMsg      = "Error occurred downloading WSDL file";
        public const  string WriteTempFileErrorMsg = "Could not save WSDL file to the local machine";
        public const  string FileNotFoundMsg       = "File not found";
        public const  string NoTriggersMsg = NoTriggersMsgA + ProjectCannnotStartMsg;
        public const  string NoTriggerMsg  = NoTriggersMsgB + ProjectCannnotStartMsg;
        public const  string ErrTriggerMsg = NoTriggersMsgC + CannotWriteScriptFileMsg;
        public const  string NoTriggersDlgTitle = "Resident Packages Search Result";
        public const  string TriggersDlgCaption = "Choose a Triggering Event";
        public const  string AddNodeToLoopDlgTitle = "Add Node To Loop"; 
        public const  string PackageInstallErrorDlgTitle = "Package Install Error"; 
        public const  string IdeSerializationErrorMsg    = "IDE could not be serialized";  
        public const  string UndefinedConfigItemMsg     = "Undefined Name for configuration item";
        public const  string DuplicateConfigItemMsg     = "Duplicate Name for configuration item";
        public const  string MinBoundaryMsg             = "The Minimum value must be less than the Maximum value";
        public const  string MaxBoundaryMsg             = "The Maximum value must be greater than the Minimum value";
        public const  string NewConfItemMsg             = "Define new item:";
        public const  string NewConfValueMsg            = "Define new value:";
        public const  string NullSerializationStreamMsg = "No stream open for project file";
        public const  string CannotWriteProjectFileMsg  = "A new project file was not created";
        public const  string CannotWriteScriptFileMsg   = "The script could not be instantiated";
        public const  string InstallerFileQ             = "Installer file '";
        public const  string DatabaseScript             = "Database script '";
        public const  string MediaFileQ                 = "Media file '";
        public const  string AlreadyDeployingMsg        = "Concurrent deployments are not allowed";

        public const  string NameOfMaxAsyncActionNode   = "MaxAsyncActionNode";
        public const  string NameOfMaxCallNode          = "MaxCallNode";
        public const  float  MultiTextOffsetX           = 18F;
        public const  float  MultiTextOffsetY           = 16F;
        public const  float  LoopOffsetX                = 18F;
        public const  float  LoopOffsetY                = 16F;
        public const  float  CommentOffsetX             = 18F;
        public const  float  CommentOffsetY             = 16F;

		public static string BuildInvalidWSMsg(string errorLogPath) 
		{ 
			return InvalidWebServiceMsg + dot + newline + "Check " + errorLogPath + " for more information.";
		}

		public static string BuildWsdlParseErrorMsg(string errorLogPath) 
		{ 
			return WsdlParseErrorMsg + dot + newline + "Check " + errorLogPath + " for more information.";
		}

        public static  string ProxiedCountMsg(int n, int m)
        {
            return n + " of " + m + " methods were proxied to the local machine.";
        }

        public static  string ToolboxAddCountMsg(int n, string tabname)
        {
            return n + " web service methods were added to toolbox tab " + tabname + dot;
        }

        public static  string ToolboxAddErrorMsg(string tab, string name)
        {
            return "Tool " + name + " could not be added to toolbox tab " + tab;
        }

        public static  string ReferenceAddErrorMsg(string name)
        {
            return "Reference " + name + " could not be added to project";
        }

        public static string ProjectFileTempPath(string projectFolder)
        {
            return projectFolder + Const.projectTempFileName;
        }

        public static string ProjectFileWriteErrorMsg(string errmsg)
        { 
            return errmsg + newline + CannotWriteProjectFileMsg;
        }

        public static string CouldNotInsertNodeMsg(string canvas, string node)
        {
            return "Could not insert node " + canvas + dot + node;
        }

        public static string CannotNameAppMsgA(string name, string filetype)
        {
            string ft = filetype == null? "Application script '": filetype;
            return ft + name + "' already exists in the project folder";
        }

        public static string CannotNameAppMsgB(string name)
        {
            return "'" + name + "' is not a valid application script name";
        }

        public static string CannotCopyMsgA(string sourcePath, string destPath)
        {
            return "Cannot copy " + sourcePath + "\nto " + destPath;
        }

        public static string CannotOpenAppMsgA(string name)
        {
            return "Application script '" + name + "' already exists in the project";
        }

        private static string NotFoundAndCreatedSegment = "\nwas not found and was created";

        public  static string MissingInstallerMsg(string path)
        {
            return "Project installer " + path + NotFoundAndCreatedSegment;
        }

        public static string MissingLocalesMsg(string path)
        {
            return "Project locale definitions " + path + NotFoundAndCreatedSegment;
        }

        public static string MissingDatabaseMsg(string path)
        {
            return "Database script " + path + NotFoundAndCreatedSegment;
        }

        private const string notFoundRemoved = "was not found and was removed from project";

        public static string MissingAppMsg(string name)
        {
            return "Application script file " + name + newline + notFoundRemoved;
        }

        public static string MissingMediaMsg(string name)
        {
            return "Media file " + name + blank + notFoundRemoved;
        }


        public static string MissingVrResxMsg(string name)
        {
            return "Voice rec resource file " + name + blank + notFoundRemoved;
        }

        public static string MissingReference(string path)
        {
            return "Reference " + path + newline + notFoundRemoved;
        }

        public static string DeleteToolboxTabMsg(string name, int count)
        {
            return "Tab '" + name + "' and " + count + " tool(s) will be removed from toolbox";
        }

        public static string DeleteToolboxItemMsg(string name)
        {
            return "Tool '" + name + "' will be removed from toolbox";
        }

        public static string MissingPackageMsg()
        {
            return "Package source library (.dll) or package (.xml) not found." 
                + "\n\nCurrently, both dll and package must reside in the same directory.";
        }

        public static string ReferenceExistsMsg(string name)
        {
            return "A reference to '" + name + "' already exists in the project";
        }

        public static string CantDeleteFunctionMsg(string fname)
        {
            return "Function " + fname + cantremovenow + ", since it" + containsrefs + dot;
        }

        private const string containsrefs  = " contains references to other functions";
        private const string cantremovenow = " cannot be removed at this time";

        public static string CantDeleteHandlerMsg(string aname, string fname, IMaxNode recursiveNode)
        {
            StringBuilder s = new StringBuilder("Function ");
            s.Append(fname); s.Append(Const.containsrefs);
            s.Append(", and so cannot be deleted.");
            s.Append("\n\nSince function '");
            s.Append(fname);
            s.Append("' is an event handler for async action '");
            s.Append(aname);
            s.Append("',\nand would be deleted with the action, the action");
            s.Append(Const.cantremovenow);
            s.Append(Const.dot);

            if (recursiveNode == null) return s.ToString();

            s.Append("\n\nTo delete the self-referential node ");
            s.Append(recursiveNode.NodeName);
            s.Append(" from function");
            s.Append(fname);
            s.Append(",\nsuch that the problematic references to the function can be removed,");
            s.Append("\nyou can (temporarily) add a reference (CallFunction or async action) to the function,");
            s.Append(" from outside the function.");            
            return s.ToString();
        }

        public static string CouldNotDemoteTreeNodeMsg(string funcname)
        {
            return "Could not demote handler since function " + funcname + " does not exist";
        }

        public const string CantRenameReferencedFunctionMsg 
            = "Function may not be renamed since it is referenced by other actions";

        public static string BadLoopNodeTypeMsg(string type)
        {
            return "Nodes of type '" + type + "' may not belong to a loop";
        }

        public const string InitialAnnotationText   
            = "Please enter descriptive text\r\nfor this node's annotation";

        public const string UndoRedoFailureMsg
            = "Undo or Redo operation could not be executed";
        public const string UndoRedoDlgTitle = "Undo/Redo";

        public static readonly System.Drawing.Size  size32x32 = new System.Drawing.Size(32,32);
        public static readonly System.Drawing.Size  size16x16 = new System.Drawing.Size(16,16);
        public static readonly System.Drawing.Size  size0x0   = new System.Drawing.Size (0,0);
        public static readonly System.Drawing.SizeF size1x1   = new System.Drawing.SizeF(1,1);

        public const  string PromptPackagesFolderLocation  = "Location of Packages Folder";
        public const  string PromptFrameworkFolderLocation = "Location of Framework Directory";
        public const  string PromptLocalLibrariesFolderLocation = "Location of Local Libraries Folder";

        private const string aboutA = productName + " Version " + BuildConst.Version;
        private const string aboutB = "\nCopyright © 2002-7 Cisco Systems Inc. All rights reserved.";
        public static string AboutMsg { get { return aboutA + aboutB; } }

        private const string noPkgsMsgA 
            = "No action/event packages could be loaded, thus no tools are available.";
        private const string noPkgsMsgB 
            = "\n\nEnsure that the configured Packages Folder location is correct, "
            + "\nand that the folder contains packages.";
        public  static string NoPkgsMsg { get { return noPkgsMsgA + noPkgsMsgB; } }

        public  const string toolboxDropObjectClassString 
            = "Metreos.Max.Framework.Satellite.Toolbox.MaxDropObject";
        public  const string canvasDropObjectClassString 
            = "Metreos.Max.Drawing.MaxSelection";
        public  static readonly Type toolboxDropObjectClassType 
            = typeof(Metreos.Max.Framework.Satellite.Toolbox.MaxDropObject);
  
        public  const int WM_PAINT            = 0xf;
        public  const int WM_HSCROLL          = 0x114;
        public  const int OCM_NOTIFY          = 0x204e;
        public  const int SB_LINEDOWN         = 1;
        public  const int WH_KEYBOARD         = 2;
        public  const int EM_SCROLL           = 0xb5;
        public  const int EM_GETLINECOUNT     = 0xba;
        public  const int EM_LINESCROLL       = 0xb6;
        public  const int EM_GETMODIFY        = 0xb8;
        public  const int EM_SETMODIFY        = 0xb9;
        public  const int WM_LBUTTONDBLCLK    = 0x203;
        public  const int WS_HSCROLL          = 0x00100000;
        public  const int WM_USER             = 0x0400;
        public  const int FILE_ATTRIBUTE_DIRECTORY = 0x10;
        public  const int FILE_ATTRIBUTE_NORMAL    = 0x80;
        public  const int WM_MAX_SVCWIZ_PANEL_LOADED = WM_USER + 0x50;
        public  const int MAX_PATH = 260;

        public  const int LinkUserFlagRemove  = 0x1000;

        private const  string outputWindowTitle   = "Output";
        public  static string OutputWindowTitle       { get { return outputWindowTitle; } }
        private const  string propertiesWindowTitle = "Properties";
        public  static string PropertiesWindowTitle   { get { return propertiesWindowTitle; } }  
        private const  string explorerWindowTitle = "Explorer";
        public  static string ExplorerWindowTitle     { get { return explorerWindowTitle; } }
        private const  string overviewWindowTitle = "Overview";
        public  static string OverviewWindowTitle     { get { return overviewWindowTitle; } }  
        private const  string toolboxWindowTitle  = "Toolbox";
        public  static string ToolboxWindowTitle      { get { return toolboxWindowTitle; } } 
        public  static string BreakpointsWindowTitle   = "Breakpoints";
        public  static string WatchWindowTitle         = "Watch";
        public  static string CallStackWindowTitle     = "Call Stack";
        public  static string RemoteConsoleWindowTitle = "Remote Console";
 
        private const  string projectOpenDlgTitle = "Open Application Designer Project";
        public  static string ProjectOpenDlgTitle     { get { return projectOpenDlgTitle; } }  
        private const  string projectSaveAsMsgTitle = "Save Project As";
        public  static string ProjectSaveAsMsgTitle   { get { return projectSaveAsMsgTitle; } } 
        public  const  string projectSaveDlgTitle = "Save Application Designer Project";
        public  static string ProjectSaveDlgTitle     { get { return projectSaveDlgTitle; } } 
        public  const  string AppOpenDlgTitle     = "Open Application Script";
        public  const  string AppAddExistDlgTitle = "Add Existing Application Script";
        public  const  string AppSaveDlgTitle     = "Save Application Script";
        public  const  string FileSaveDlgTitle    = "Save File";
        public  const  string InstalOpenDlgTitle  = "Open Installer";
        public  const  string FileNotFoundDlgTitle= "File Not Found";
        public  const  string PackageNotFoundDlgTitle= "Native Types Package Not Found.";
        public  const  string InstalAddExistDlgTitle = "Add Existing Installer";
        public  const  string LocalesAddExistDlgTitle= "Add Existing Locales";
        public  const  string InstalSaveDlgTitle    = "Save Installer";
        public  const  string DbaseNewDlgTitle      = "New Database Script";
        public  const  string DbaseOpenDlgTitle     = "Open Database Script";
        public  const  string DbaseAddExistDlgTitle = "Add Existing Database Script";
        public  const  string DbaseSaveDlgTitle     = "Save Database Script";
        public  const  string AddAudioDlgTitle      = "Add Audio File";
        public  const  string AddVrResourceDlgTitle = "Add Grammar or Other Voice Recognition Resource";
        public  const  string TtsTextNewDlgTitle    = "New TTS Text";
        public  const  string TtsTextOpenDlgTitle   = "Open TTS Text";
        public  const  string TtsTextAddExistDlgTitle = "Add Existing TTS Text File";
        public  const  string TtsTextSaveDlgTitle   = "Save TTS Textt";
        public  const  string AddReferenceDlgTitle  = "Add Reference";
        public  const  string AddWebServiceTitle    = "Add Web Service";
        public  const  string promptSaveChangesTo   = "Save changes to ";
        public  const  string WsdlBrowseDlgTitle    = "Browse for Web Service Description";
        public  const  string ChooseFolder          = "Choose a folder for the project"; 
        public  const  string ChooseDbFolder        = "Choose a folder for the database script"; 
        public  const  string DeployInitiatedMsg    = "Deploying ...";  
        public  const  string DeployCanceledMsg     = "Deployment canceled";
        public  const  string BuildErrorOccurred    = "Could not build project";
        public  const  string MoreErrorInfo         = "More information can be found in the '" + BuildErrorsFilename + 
            "' file located in the 'obj' directory of the project";
        public  const  string ErrorFileDeclaration  = "Full error information as follows:";
        public  const  string ErrorSource           = "Source:  ";
        public  const  string ErrorMessage          = "Message:  ";
        public  const  string ErrorStackTrace       = "Stack Trace:  ";
        public  const  string ErrorDateTime         = "Date:  ";
        public  const  string PackagerMessage       = "Packager Message: ";

        public  const  string InvalidInstallerDropDown = "[Invalid Installer File]";
        public  const  string InvalidLocalesDropDown = "[Invalid Locales File]";

        public  const  string WarnDeleteLastLocale      = "Removing all locales will cause all strings (rows) to dissappear.  Continue?";
        public  const  string WarnDeleteLastLocaleTitle = "Remove Last Locale";
        public  const  string LocaleDefined             = "Locale already defined";
        public  const  string LocaleDefinedTitle        = "Duplicate Locale";
        public  const  string LocalizableStringDefined  = "The specified string name already exists";
        public  const  string LocalizableStringDefinedTitle = "Duplicate String";

        public  const  string DefaultCanvasName = "Default";
        public  static string DefaultAppName { get { return DefaultCanvasName; } }
        public  static CultureInfo DefaultLocale = new CultureInfo("en-US");
        public  const  string AppTreeTriggerSubstring = " (trigger): ";

        // keys by which we can identify a key-value pair in a property grid collection
        public  const string PmVariableName  = "Variable Name";
        public  const string PmLabelName     = "Label Name";
        public  const string PmLoopTypeName  = "Loop Type";
        public  const string PmLoopCountName = "Count";
        public  const string PmCommentText   = "Text";
        public  const string PmFunctionName  = IActions.Fields.FunctionName;
        public  const string PmHandlerId     = ICommands.Fields.HANDLER_ID;

        public  static string[] csharpKeywords = new string[] 
        {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", 
            "class", "const", "continue", "decimal", "default", "delegate", "do", "double",
            "else", "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", 
            "for", "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", 
            "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override", 
            "params", "private","protected", "public", "readonly", "ref", "return", "sbyte", "sealed", 
            "short", "sizeof", "stackalloc", "static", "string", "struct","switch", "this", "throw", 
            "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", 
            "virtual", "void", "volatile", "while"
        };

        public  static string[] areKeywords = new string[] 
        {
           "session", "event", "loopIndex", "loopEnum", "loopDictEnum" 
        };

        public  const string DefaultLoopContainerText = "loop 1x";
        public  const string LoopContainerTextPrefix  = "loop ";
        public  const string LoopContainerTextSuffixConstant   = "x";
        public  const string LoopContainerTextSuffixVariable   = "(var)";
        public  const string LoopContainerTextSuffixExpression = "(expr)";

        public  const string NameCodeTool     = "MaxCodeTool";
        public  const string NameVariableTool = "MaxVariableTool";
        public  const string NameFunctionTool = "MaxFunctionTool";
        public  const string NameStartTool    = "MaxStartTool";
        public  const string NameCommentTool  = "MaxCommentTool";
        public  const string NameAnnotTool    = "MaxAnnotationTool";
        public  const string NameLoopTool     = "MaxLoopTool";
        public  const string NameLabelTool    = "MaxLabelTool";
        public  const string NameCode         = "CustomCode";
        public  const string NameVariable     = "Variable";
        public  const string NameStart        = "Start";
        public  const string NameComment      = "Comment";
        public  const string NameAnnotation   = "Annotation";
        public  const string NameLoop         = "Loop";
        public  const string NameLabel        = "Label";
        public  const string DefaultToolGroup = "Tool Group ";

        public  const string EndTranMsgNodeInserted = "node inserted";
        public  const string EndTranMsgLinkInserted = "link inserted";
        public  const string EndTranMsgLinkRemoved  = "link removed";
        public  const string EndTranMsgSelInserted  = "selection inserted";
        public  const string EndTranMsgSelDeleted   = "selection deleted";    

        public  const string menuFile   = "&File"; 
        public  const string menuEdit   = "&Edit"; 
        public  const string menuView   = "&View"; 
        public  const string menuProject= "&Project"; 
        public  const string menuBuild  = "&Build";
        public  const string menuDebug  = "&Debug"; 
        public  const string menuTools  = "&Tools"; 
        public  const string menuWindow = "&Window"; 
        public  const string menuHelp   = "&Help"; 

        public  const string menuGenericProperties = "&Properties";
        public  const string menuGenericRename     = "&Rename";
        public  const string menuGenericRenameA    = "&Remove";
        public  const string menuGenericRenameB    = "Remo&ve";
        public  const string menuGenericOpen       = "&Open";
        public  const string menuGenericClose      = "&Close";
        public  const string menuGenericGoTo       = "&Go To";
        public  const string menuGenericDelete     = "&Delete";
        public  const string menuGenericGoToDef    = "&Go To Definition";
        public  const string menuGenericAddNewItem = "&Add New Item";

        public  const string menuFileNew         = "&New Project";
        public  const string menuFileOpen        = "&Open Project";
        public  const string menuFileClose       = "&Close Project";
        public  const string menuFileAddScript   = "Add Scrip&t";
        public  const string menuFileAddInstal   = "Add &Installer";
        public  const string menuFileAddLocales  = "Add &Locales";
        public  const string menuFileAddResource = "Add R&esource";
        public  const string menuFileAddDbScript = "Add &Database";
        public  const string menuFileAddMedia    = "Add &Media";
        public  const string menuFileAddVrResx   = "Add &Voice Recognition Resource ...";
        public  const string menuFileAddTtsText  = "Add TTS &Text";
        public  const string menuFileCloseFile   = "Close &File";
        public  const string menuFileSave        = "&Save";
        public  const string menuFileSaveAs      = "Save &As";
        public  const string menuFileSaveAll     = "Save A&ll";
        public  const string menuFileSetup       = "Page Set&up ...";
        public  const string menuFilePrint       = "&Print ...";
        public  const string menuFilePreview     = "Print Pre&view";
        public  const string menuFileRecent      = "&Recent Projects";
        public  const string menuFileExit        = "E&xit";

        public const string  menuFileAddScriptNew     = "&New Script ...";
        public const string  menuFileAddScriptExist   = "&Existing Script ...";
        public const string  menuFileAddInstalNew     = "&New Installer ...";
        public const string  menuFileAddInstalExist   = "&Existing Installer ...";
        public const string  menuFileAddLocalesNew    = "&New Locale Editor ...";
        public const string  menuFileAddLocalesExist  = "&Existing Locale Editor ...";
        public const string  menuFileAddDbScriptNew   = "&New Database Script ...";
        public const string  menuFileAddDbScriptExist = "&Existing Database Script ...";
        public const string  menuFileAddMediaExist    = "&Audio File ...";
        public const string  menuFileAddTtsTextNew    = "&New TTS Text File ...";
        public const string  menuFileAddTtsTextExist  = "&Existing TTS Text File ...";

        public  const string menuEditUndo   = "&Undo";
        public  const string menuEditRedo   = "&Redo";
        public  const string menuEditCut    = "Cu&t";
        public  const string menuEditCopy   = "&Copy";
        public  const string menuEditPaste  = "&Paste";
        public  const string menuEditSelAll = "Select &All";

        public  const string menuViewZoom     = "&Zoom";
        public  const string menuViewZoomIn   = "&In";
        public  const string menuViewZoomOut  = "&Out";
        public  const string menuViewZoomNorm = "&Normal";
        public  const string menuViewGrid     = "&Grid";
        public  const string menuViewProps    = "Properties &Window";
        public  const string menuViewCanvas   = "&Canvas Only";
        public  const string menuViewToolbox  = "&Toolbox";
        public  const string menuViewExplorer = "Ex&plorer";
        public  const string menuViewOutput   = "&Output";
        public  const string menuViewOverview = "Ove&rview";
        public  const string menuViewTray     = "&Variables Tray";
        public  const string menuViewConsole  = "Remote Co&nsole";

        public  const string menuProjectProperties   = "&Properties ...";
        public  const string menuProjectAddReference = "Add &Reference ...";
        public  const string menuProjectAddWebService= "Add Web Ser&vice ...";
        public  const string menuProjectRemWebService= "Re&move Web Service";
        public  const string menuBuildBuildProject   = "&Build Project";  
        public  const string menuBuildDeploy         = "&Deploy";

        public  const string menuDebugWindows        = "&Windows ...";
        public  const string menuDebugStart          = "&Start Debugging";
        public  const string menuDebugStop           = "&Stop Debugging";
        public  const string menuDebugContinue       = "&Contin&ue";
        public  const string menuDebugBreak          = "B&reak";
        public  const string menuDebugStepInto       = "Step &Into";
        public  const string menuDebugStepOver       = "Step &Over";
        public  const string menuDebugNewBkpt        = "New &Breakpoint";
        public  const string menuDebugToggleBkpt     = "&Toggle Breakpoint";
        public  const string menuDebugClearBkpts     = "&Clear All Breakpoints";
        public  const string menuDebugDisableBkpts   = "&Disable All Breakpoints";
        public  const string menuDebugStartConsole   = "Start Remote Co&nsole";
        public  const string menuDebugStopConsole    = "Stop Remote Co&nsole";

        public  const string menuDebugWindowsBkpts   = "&Breakpoints";
        public  const string menuDebugWindowsWatch   = "&Watch";
        public  const string menuDebugWindowsCalls   = "&Call Stack";
        public  const string menuDebugWindowsConsole = "&Remote Console";

        public  const string menuNodeInsertBkpt      = "Insert B&reakpoint";
        public  const string menuNodeRemoveBkpt      = "Remo&ve Breakpoint";
        public  const string menuNodeEnableBkpt      = "Enab&le Breakpoint";
        public  const string menuNodeDisableBkpt     = "&Disable Breakpoint";

        public  const string menuToolsAddTab    = "Add Toolbox &Tab";
        public  const string menuToolsAddRemove = "Add/Remove Toolbox &Items";
        public  const string menuToolsOptions   = "&Options ...";  
        public  const string menuToolsTest      = "Te&st ..."; 
        public  const string menuToolsPkgs      = "&Packages Folder ...";
        public  const string menuToolsFwk       = "&Framework Folder ...";
        public  const string menuToolsLibs      = "&Local Libraries Folder ...";

        public  const string menuConsoleInvertColors = "&Invert Colors";
        public  const string menuConsoleStart   = "&Start";
        public  const string menuConsoleStop    = "Sto&p";

        public  const string menuWindowCloseAll = "C&lose All";
        public  const string menuWindowWindows  = "&Windows ...";

        public  const string menuHelpAbout  = "&About Cisco Unified Application Designer ...";

        public  const string menuDockDock   = "Doc&kable";
        public  const string menuDockHide   = "&Hide";
        public  const string menuDockFloat  = "&Floating";
        public  const string menuDockAuto   = "&Auto Hide";

        public  const string menuTabDelete  = "&Delete Tab";
        public  const string menuTabRename  = "&Rename Tab";
        public  const string menuTabAddRem  = "Add/Remove &Items ...";
        public  const string menuTabAdd     = "&Add Tab";
        public  const string menuTabShowAll = "&Show All Tabs";
        public  const string menuTabMoveUp  = "Move &Up";
        public  const string menuTabMoveDn  = "Move Do&wn";

        public  const string menuToolDelete = "&Delete Tool";
        public  const string menuToolRename = "&Rename Tool";
        public  const string menuToolAddRem = "Add/Remove &Items ...";

        public  const string menuAppTreeTriggerReplace = "Replace &Trigger";
        public  const string menuAppTreeHandlerRename  = "&Rename Handler";
        public  const string menuAppTreeHandlerReplace = "Replace E&vent";
        public  const string menuAppTreeHandlerDelete  = "&Delete Handler";
        public  const string menuAppTreeHandlerGoTo    = "&Go To Handler";
        public  const string menuAppTreeFunctionDelete = "&Delete Function";
        public  const string menuAppTreeFunctionCalls  = "Show Ca&lls";
        public  const string menuAppTreeFunctionRefs   = "Show Re&ferences";
        public  const string menuAppTreeFunctionParams = "Show Para&meters";

        public  const string menuLinkLinkStyle         = "Link &Style";
        public  const string menuLinkLinkOrthogonal    = "Se&gmented";
        public  const string menuLinkLinkCurve         = "&Curve";
        public  const string menuLinkLinkLine          = "&Line";
        public  const string menuLinkLinkRounded       = "&Bevel";

        public  const string menuNodeRenameHandler     = "&Rename Function";
        public  const string menuNodeGoToHandler       = "&Go To Function";

        public  const string menuNodeAnnotation        = "&Annotation ...";
        public  const string menuNodeAddAnnotation     = "&Add";
        public  const string menuNodeEditAnnotation    = "&Edit";
        public  const string menuNodeShowAnnotation    = "&Show";
        public  const string menuNodeHideAnnotation    = "&Hide";
        public  const string menuNodeDeleteAnnotation  = "&Delete";
        public  const string menuNodeCopyCode          = "Copy &Code";

        public  const string menuOutputGoToNode = "&Go To Error Node ...";
        public  const string menuOutputClearAll = "Clear Al&l";        

        public  const string xmlEltProject      = "MaxProject";
        public  const string xmlEltMaxIDE       = "MaxIDE";
        public  const string xmlEltInnerLayout  = "InnerConfig"; 
        public  const string xmlEltDocking      = "DockingConfig";
        public  const string xmlEltClipboard    = "MaxClipboardA";
        public  const string xmlEltApplication  = "Application";
        public  const string xmlEltCustomTypes  = "CustomInstallerTypes";
        public  const string xmlEltCustomType   = "CustomInstallerType";
        public  const string xmlEltCustomValue  = "Value";
        public  const string xmlEltProperties   = "Properties";
        public  const string xmlEltPackages     = "Packages";
        public  const string xmlEltTriggers     = "Triggers";
        public  const string xmlEltTrigger      = "trigger";
        public  const string xmlEltToolbox      = "Toolbox";
        public  const string xmlEltInclude      = "Include";
        public  const string xmlEltScripts      = "Scripts";
        public  const string xmlEltAnnotation   = "annot";
        public  const string xmlEltTools        = "MaxTools";
        public  const string xmlEltFiles        = "Files";
        public  const string xmlEltFile         = "File";
        public  const string xmlEltLayout       = "layout";
        public  const string xmlEltPackage      = "package";
        public  const string xmlEltTboxTab      = "sidetab";
        public  const string xmlEltTboxItem     =  Const.xmlEltTboxTab + "item";
        public  const string xmlEltToolgroup    = "toolgroup";
        public  const string xmlEltTool         = "tool";
        public  const string xmlEltItems        = "items";
        public  const string xmlEltItem         = "item";
        public  const string xmlEltGlobal       = "global";
        public  const string xmlEltOutline      = "outline";
        public  const string xmlEltVariables    = "variables";
        public  const string xmlEltFunction     = "function";  
        public  const string xmlEltCanvas       = "canvas";
        public  const string xmlEltLinkTo       = "linkto";
        public  const string xmlEltTreeNode     = "treenode";
        public  const string xmlEltSelection    = "selection";
        public  const string xmlEltReferences   = "references";
        public  const string xmlEltCalls        = "calls";
        public  const string xmlEltRef          = "ref";
        public  const string xmlEltNode         = "node";
        public  const string xmlEltAp           = "ap";
        public  const string xmlEltUsing        = "using";
        public  const string xmlEltWsdlDef      = "definitions";
        public  const string xmlEltWsdlImport   = "import";
        public  const string xmlEltXsdSchema    = "schema";
        public  const string xmlEltXsdImport    = "import";

        public  const string xmlAttrName        = "name";
        public  const string xmlAttrType        = "type";
        public  const string xmlAttrNodeType    = "Type";
        public  const string xmlAttrPackage     = "package";
        public  const string xmlAttrRelPath     = "relpath";
        public  const string xmlAttrSubType     = "subtype";
        public  const string xmlAttrRefType     = "reftype";
        public  const string xmlAttrLocale      = "locale";
        public  const string xmlAttrCurrent     = "current";
        public  const string xmlAttrAll         = "all";
        public  const string xmlAttrShow        = "show";
        public  const string xmlAttrHandler     = "handler";
        public  const string xmlAttrCount       = "count";
        public  const string xmlAttrID          = "id";
        public  const string xmlAttrVID         = "vid";
        public  const string xmlAttrX           = "x";
        public  const string xmlAttrY           = "y";
        public  const string xmlAttrWidth       = "cx";
        public  const string xmlAttrHeight      = "cy";
        public  const string xmlAttrMidX        = "mx";
        public  const string xmlAttrMidY        = "my";
        public  const string xmlAttrLeft        = "left";
        public  const string xmlAttrTop         = "top";
        public  const string xmlAttrText        = "text";
        public  const string xmlAttrActionID    = "actid";
        public  const string xmlAttrStyle       = "style";
        public  const string xmlAttrOrthogonal  = "ortho";
        public  const string xmlAttrEntry       = "entry";
        public  const string xmlAttrExit        = "exit";
        public  const string xmlAttrLabelType   = "labeltype";
        public  const string xmlAttrActiveTab   = "activetab";
        public  const string xmlAttrGroup       = "group";
        public  const string xmlAttrVersion     = "version";
        public  const string xmlAttrSingleton   = "single";
        public  const string xmlAttrTrigger     = "trigger";
        public  const string xmlAttrFunction    = "function";
        public  const string xmlAttrEvent       = "event";
        public  const string xmlAttrLevel       = "level";
        public  const string xmlAttrLabel       = "label";
        public  const string xmlAttrGrid        = "grid";
        public  const string xmlAttrPort        = "port";
        public  const string xmlAttrVarsY       = "varsy";
        public  const string xmlAttrPath        = "path";
        public  const string xmlAttrClass       = "class";
        public  const string xmlAttrFromPort    = "fromport";
        public  const string xmlAttrStartNode   = "startnode";
        public  const string xmlAttrTreeNode    = "treenode";
        public  const string xmlAttrAppNode     = "appnode";
        public  const string xmlAttrHandlerFor  = "handlerfor";  
        public  const string xmlAttrDescription = "description";
        public  const string xmlAttrContainer   = "container";
        public  const string xmlAttrInternalName= "internalName";
        public  const string xmlAttrWsdlLocation= "location";
        public  const string xmlAttrXsdLocation = "schemaLocation";

        public  const string xmlValFileSubtypeIDE     = "ide";
        public  const string xmlValFileSubtypeApp     = "app";
        public  const string xmlValFileSubtypeDbase   = "database";
        public  const string xmlValFileSubtypeInstall = "installer";
        public  const string xmlValFileSubtypeMedia   = "media";
        public  const string xmlValFileSubtypeVrResx  = "voicerec";
        public  const string xmlValFileSubtypeRef     = "ref";
        public  const string xmlValFileSubtypeLocales = "locale";
        public  const string xmlValRefTypeNativeAct   = "nativeAction";
        public  const string xmlValRefTypeNativeType  = "nativeType";
        public  const string xmlValRefTypeProvider    = "provider";
        public  const string xmlValRefTypeOther       = "other";
        public  const string xmlValNodeTypeAction     = "action";
        public  const string xmlValNodeTypeFunction   = "function";
        public  const string xmlValFunctionName       = "functionName";
        public  const string xmlValNodeTypeEvent      = "event";
        public  const string xmlValNodeTypeVariable   = "variable";
        public  const string xmlValNodeTypeLoop       = "loop";
        public  const string xmlValNodeTypeStart      = "start";
        public  const string xmlValNodeTypeComment    = "comment";
        public  const string xmlValNodeTypeLabel      = "label";
        public  const string xmlValBoolTrue           = "true";
        public  const string xmlValBoolFalse          = "false";
        public  const string xmlValLabelTypeTarget    = "target";
        public  const string xmlValLabelTypeSource    = "source";
        public  const string xmlValLinkStyleCurve     = "curve";
        public  const string xmlValLinkStyleLine      = "line";
        public  const string xmlValLinkStyleBevel     = "bevel";
        public  const string xmlValNode               = "node";
        public  const string xmlValNodeEvh            = "evh";
        public  const string xmlValNodeFun            = "fun";
        public  const string xmlValNodeVar            = "var";
        public  const string xmlNewline               = "\r\n";  

        public  const string xpathLink                = "::";
        public  const string xpathAttribute           = "attribute";
        public  const string xpathChild               = "child";
        public  const string xpathPath                = "/";
        public  const string xpathSpecifierOpen       = "[";
        public  const string xpathSpecifierClose      = "']";
        public  const string xpathEquals              = " = '";
        public  const string xpathAnd                 = "' and ";
        public  const string xpathOr                  = "' or ";
        public  const string arg0                     = "{0}";
        public  const string xpathGlobal              = xpathChild + xpathLink + xmlEltGlobal;
        public  const string xpathCanvas              = xpathChild + xpathLink + xmlEltCanvas;
        public  const string xpathGlobalVariables     = xpathChild + xpathLink + 
            xmlEltGlobal + xpathPath + 
            xpathChild + xpathLink + xmlEltVariables +
            xpathPath + xpathChild + xpathLink + xmlEltTreeNode;

        public  const string xpathLocalVariables      = xpathChild + xpathLink + xmlEltNode + xpathSpecifierOpen + 
            xpathAttribute + xpathLink + xmlAttrType + xpathEquals + 
            defaultVariableToolName + xpathSpecifierClose;
        public  const string xpathProperties          = xpathChild + xpathLink + xmlEltProperties;
        public  const string xpathProjectUsings       = xpathChild + xpathLink + xmlEltProperties+ xpathPath + 
            xpathChild + xpathLink + xmlEltUsing;
        public  const string xpathTreeNode            = xpathChild + xpathLink + xmlEltOutline + xpathPath + 
            xpathChild + xpathLink + xmlEltTreeNode + xpathPath + 
            xpathChild + xpathLink + xmlEltNode + xpathSpecifierOpen + 
            xpathAttribute + xpathLink + xmlAttrType + xpathEquals + 
            xmlValNodeTypeFunction + xpathAnd + xpathAttribute + xpathLink + xmlAttrName + 
            xpathEquals + arg0 + xpathSpecifierClose;

        public  const string xpathTreeNodeEvent       = xpathChild + xpathLink + xmlEltNode + 
            xpathSpecifierOpen + xpathAttribute + xpathLink + 
            xmlAttrType + xpathEquals + xmlValNodeTypeEvent + 
            xpathSpecifierClose;
        public  const string xpathTreeNodeEventAbs    = xpathChild + xpathLink + xmlEltGlobal + xpathPath + 
            xpathChild + xpathLink + xmlEltOutline + xpathPath + 
            xpathChild + xpathLink + xmlEltTreeNode + xpathPath + 
            xpathTreeNodeEvent;
        public  const string xpathEventParameters     = xpathChild + xpathLink + xmlEltProperties + 
            xpathPath + xpathChild + xpathLink + Defaults.xmlEltEventParameter;
        public  const string xpathLoop                = xpathChild + xpathLink + xmlEltNode + xpathSpecifierOpen +
            xpathAttribute + xpathLink + xmlAttrType +
            xpathEquals + defaultLoopToolName + xpathSpecifierClose;
        public  const string xpathLinkto              = xpathChild + xpathLink + xmlEltLinkTo;
        public  static string xpathActionLabelLoop    = xpathChild + xpathLink + xmlEltNode + xpathSpecifierOpen +
            xpathAttribute + xpathLink + xmlAttrType + xpathEquals + 
            MaxTool.ToolTypes.Action + xpathOr + xpathAttribute + xpathLink + 
            xmlAttrType + xpathEquals + MaxTool.ToolTypes.Label + xpathOr + xpathAttribute + 
            xpathLink + xmlAttrType + xpathEquals + MaxTool.ToolTypes.Loop + xpathSpecifierClose;
        public  const string xpathStart               = xpathChild + xpathLink + xmlEltNode + xpathSpecifierOpen +
            xpathAttribute + xpathLink + xmlAttrType + xpathEquals + 
            defaultStartToolName + xpathSpecifierClose;
        public  const string xpathLabel               = xpathChild + xpathLink + xmlEltNode + 
            xpathSpecifierOpen + xpathAttribute + xpathLink + 
            xmlAttrType + xpathEquals + defaultLabelToolName + xpathSpecifierClose;
        public  static string xpathAction             = xpathChild + xpathLink + xmlEltNode + xpathSpecifierOpen + 
            xpathAttribute + xpathLink + xmlAttrType + xpathEquals +
            MaxTool.ToolTypes.Action + xpathSpecifierClose;
        public  const string xpathLoopWithId          = xpathChild + xpathLink + xmlEltNode + xpathSpecifierOpen + 
            xpathAttribute + xpathLink + xmlAttrID + xpathEquals + arg0 + 
            xpathAnd + xpathAttribute + xpathLink + xmlAttrType + 
            xpathEquals + defaultLoopToolName + xpathSpecifierClose;
        public static string xpathActionAbs           = xpathChild + xpathLink + xmlEltCanvas + xpathPath + xpathAction;

        // print setup related values
        public abstract class PrintSetup
        {
            public const string PrintSetupKey       = "PrintSetup";
            public const string OrientationSubKey   = "Orientation";
            public const string FitToPageSubKey     = "FitToPage";
            public const string MarginLSubKey       = "MarginL";
            public const string MarginTSubKey       = "MarginT";
            public const string MarginRSubKey       = "MarginR";
            public const string MarginBSubKey       = "MarginB";

            public const string DefaultOrientation  = "Landscape";     // Landscape or Portrait
            public const string DefaultFitToPage    = "True";          // True or False
            public const string DefaultMarginL      = "25";            // 1/100 inches
            public const string DefaultMarginT      = "25";            // 1/100 inches
            public const string DefaultMarginR      = "25";            // 1/100 inches
            public const string DefaultMarginB      = "25";            // 1/100 inches

            public const string Landscape           = "Landscape";
            public const string Portrait            = "Portrait";
            public const string FitToPage           = "True";
            public const string NotFitToPage        = "False";
        }

        public abstract class Localization
        {
            // Localization related configs
            public const string LocaleEditorBasics =
           @"The Localization Editor allows one to define localizable strings. 
A localizable string is a string which has  different values based on the 
current locale of the application script.

Associated with every partition of an application is a current locale. 
A global variable initialized with a localizable string will automatically
have the value of the string defined for that particular locale.

To use a localizable string within a script, one must associate the string 
with a global variable.  This is done by choosing the name of the 
localizable string item in the dropdown of the Config property of the 
global variable.";

            public const string LocaleEditorUsage =
             @"To use this editor, one must define new locales for your application by 
selecting the 'Add Locale' button.  When 1 or more locales are defined,
one can then define any number of localizable strings by selecting
'Add String'.  

Right-click on the row header or column header to rename or remove 
a particular locale or string.

Note: to enter a newline within a cell, you must use Shift + Enter.";

            public const string RenameContextItemName = "Rename";
            public const string RemoveContextItemName = "Remove";
        }

    } // class Const
}   // namespace
