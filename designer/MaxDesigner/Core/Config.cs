using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Metreos.Max.Drawing;
using Northwoods.Go;

using Metreos.Max.Framework.Satellite.Property;

// We compare against const items often in this module. 
// Suppress the unreachable code warning 
#pragma warning disable 0429 // CS0429: Unreachable expression code detected


namespace Metreos.Max.Core
{
    /// <summary>Max Designer global configuration entries</summary>
    public sealed class Config
    {
        #region singleton
        private Config() {}
        private static Config instance;
        public  static Config Instance
        { get 
          { if (instance == null)
            {   instance = new Config();
                instance.Init();
            }
            return instance;
          }
        }
        #endregion

        private void Init()
        {
            recentFileList = new Framework.MaxRecentFileList(); 
            recentFileList.Load(); 
        }


        /// <summary>Get framework directory from registry</summary>
        public static string FrameworkDirectory
        { 
            get { return GetRegLoc(Const.FrameworkSubkey); }
            set { SetRegLoc(Const.FrameworkSubkey, value); }
        }


        /// <summary>Get framework version from registry</summary>
        public static string FrameworkVer
        { 
            get { return GetRegLoc(Const.FrameworkVerSubkey); }
            set { SetRegLoc(Const.FrameworkVerSubkey, value); }
        }


        /// <summary>Get app server IP address from registry</summary>
        public static string AppServerIP
        { 
            get { return GetRegLoc(Const.AppServerIpSubkey); }
            set { SetRegLoc(Const.AppServerIpSubkey, value); }
        }


        /// <summary>Get app server port from registry</summary>
        public static string AppServerPort
        { 
            get { return GetRegLoc(Const.AppServerPortSubkey); }
            set { SetRegLoc(Const.AppServerPortSubkey, value); }
        }


        /// <summary>Get app server admin username from registry</summary>
        public static string AppServerAdminUser
        { 
            get { return GetRegLoc(Const.AdminUserSubkey); }
            set { SetRegLoc(Const.AdminUserSubkey, value); }
        }


        /// <summary>Get app server admin password from registry</summary>
        public static string AppServerAdminPass
        { 
            get { return GetRegLoc(Const.AdminPassSubkey); }
            set { SetRegLoc(Const.AdminPassSubkey, value); }
        }

        public static string AppDeploymentUser
        {
            get { return GetRegLoc(Const.AppDepUsernamekey); }
            set { SetRegLoc(Const.AppDepUsernamekey, value); }
        }

        public static string AppDeploymentPass
        {
            get { return GetRegLoc(Const.AppDepPasswordkey); }
            set { SetRegLoc(Const.AppDepPasswordkey, value); }
        }

        public static string OpenSshPort
        {
            get { return GetRegLoc(Const.SshPortkey); }
            set { SetRegLoc(Const.SshPortkey, value); }
        }

        /// <summary>Get SSH Time Out from Registry</summary>
        public static string SshTimeOut
        {
            get { return GetRegLoc(Const.SshTimeOutkey); }
            set { SetRegLoc(Const.SshTimeOutkey, value); }
        }

        /// <summary>Get debugger ipc port from registry</summary>
        public static string DebuggerPort
        { 
            get { return GetRegLoc(Const.DebuggerPortSubkey); }
            set { SetRegLoc(Const.DebuggerPortSubkey, value); }
        }


        /// <summary>Get remote console port from registry</summary>
        public static string ConsolePort
        { 
            get { return GetRegLoc(Const.ConsolePortSubkey); }
            set { SetRegLoc(Const.ConsolePortSubkey, value); }
        }


        /// <summary>Get window placement from registry</summary>
        public static string WindowPlacement
        { 
            get { return GetRegLoc(Const.PlacementSubkey); }
            set { SetRegLoc(Const.PlacementSubkey, value); }
        }


        /// <summary>Get MRU max entry count from registry</summary>
        public static string MaxMRU
        { 
            get { return GetRegOpt(Const.MaxMruSubkey); }
            set { SetRegOpt(Const.MaxMruSubkey, value); }
        }


        /// <summary>Get whether to load most recent project on max boot from registry</summary>
        public static string LoadOnStartup
        { 
            get { return GetRegOpt(Const.LoadOnStartupSubkey); }
            set { SetRegOpt(Const.LoadOnStartupSubkey, value); }
        }


        /// <summary>Get whether to copy media files to project folder from registry</summary>
        public static string CopyMediaFilesLocal
        { 
            get { return GetRegOpt(Const.CopyMediaLocalSubkey); }
            set { SetRegOpt(Const.CopyMediaLocalSubkey, value); }
        }


        /// <summary>Get whether to show display names for actions as defined in packages</summary>
        public static string ShowDisplayNames
        {
            get { return GetRegOpt(Const.DisplayNamesSubkey); }
            set { SetRegOpt(Const.DisplayNamesSubkey, value); }
        }

        /// <summary>Get whether to copy media files to project folder from registry</summary>
        public static string DefaultPropertyType
        { 
            get { return GetRegOpt(Const.DefPropTypeSubkey); }
            set { SetRegOpt(Const.DefPropTypeSubkey, value); }
        }


        /// <summary>Get link style from registry</summary>
        public static string LinkStyle
        { 
            get { return GetRegOpt(Const.LinkStyleSubkey); }
            set { SetRegOpt(Const.LinkStyleSubkey, value); }
        }


        /// <summary>Get link width from registry</summary>
        public static string LinkWidth
        { 
            get { return GetRegOpt(Const.LinkWidthSubkey); }
            set { SetRegOpt(Const.LinkWidthSubkey, value); }
        }

        /// <summary>Get link orthogonality from registry</summary>
        public static string LinkOrtho
        { 
            get { return GetRegOpt(Const.LinkOrthoSubkey); }
            set { SetRegOpt(Const.LinkOrthoSubkey, value); }
        }

        /// <summary>Get link unconditional switch from registry</summary>
        public static string LinkUnconditional
        { 
            get { return GetRegOpt(Const.LinkUnconSubkey); }
            set { SetRegOpt(Const.LinkUnconSubkey, value); }
        }


        /// <summary>Get grid width from registry</summary>
        public static string GridWidth
        {  
            get { return GetRegOpt(Const.GridWidthSubkey); }
            set { SetRegOpt(Const.GridWidthSubkey, value); }
        }


        /// <summary>Get grid height from registry</summary>
        public static string GridHeight
        { 
            get { return GetRegOpt(Const.GridHeightSubkey); }
            set { SetRegOpt(Const.GridHeightSubkey, value); }
        }


        /// <summary>Get grid snap switch from registry</summary>
        public static string GridSnap
        { 
            get { return GetRegOpt(Const.GridSnapSubkey); }
            set { SetRegOpt(Const.GridSnapSubkey, value); }
        }


        /// <summary>Get warnings as error switch from registry</summary>
        public static string WarnAsError
        { 
            get { return GetRegOpt(Const.WarnAsErrorSubkey); }
            set { SetRegOpt(Const.WarnAsErrorSubkey, value); }
        }


        /// <summary>Get CallFunction as list node switch from registry</summary>
        public static string CallAsList
        { 
            get { return GetRegOpt(Const.CallAsListSubkey); }
            set { SetRegOpt(Const.CallAsListSubkey, value); }
        }


        /// <summary>Get 'start link on mouse motion' switch from registry</summary>
        public static string PortMotion
        { 
            get { return GetRegOpt(Const.PortMotionSubkey); }
            set { SetRegOpt(Const.PortMotionSubkey, value); }
        }


        /// <summary>Get large ports switch from registry</summary>
        public static string PortLarge
        { 
            get { return GetRegOpt(Const.PortLargeSubkey); }
            set { SetRegOpt(Const.PortLargeSubkey, value); }
        }


        /// <summary>Get visible ports switch from registry</summary>
        public static string PortVisible
        { 
            get { return GetRegOpt(Const.PortVisibleSubkey); }
            set { SetRegOpt(Const.PortVisibleSubkey, value); }
        }


        /// <summary>Get auto new script dialog switch from registry</summary>
        public static string SuppressAutoNewScript
        { 
            get { return GetRegOpt(Const.SuppressAutoNewScript); }
            set { SetRegOpt(Const.SuppressAutoNewScript, value); }
        }


        /// <summary>Get auto new script dialog switch from registry</summary>
        public static string SuppressAutoNewProject
        { 
            get { return GetRegOpt(Const.SuppressAutoNewProject); }
            set { SetRegOpt(Const.SuppressAutoNewProject, value); }
        }


        /// <summary>Get remote console background switch from registry</summary>
        public static string ConsoleBkgnd
        { 
            get { return GetRegDbg(Const.ConsoleBkgndSubkey); }
            set { SetRegDbg(Const.ConsoleBkgndSubkey, value); }
        }


        /// <summary>Get registry item from Metreos Location key</summary>
        public static string GetRegLoc(string subkey)
        {
            return Reg.GetStringValue(Const.RegistryLocationKey, subkey);
        }


        /// <summary>Set Metreos Location key registry item</summary>
        public static void SetRegLoc(string subkey, string value)
        {
            Reg.SetStringValue(Const.RegistryLocationKey, subkey, value);
        }


        /// <summary>Get registry item from Metreos Options key</summary>
        public static string GetRegOpt(string subkey)
        {
            return Reg.GetStringValue(Const.RegistryOptionsKey, subkey);
        }


        /// <summary>Set Metreos Options key registry item</summary>
        public static void SetRegOpt(string subkey, string value)
        {
            Reg.SetStringValue(Const.RegistryOptionsKey, subkey, value);
        }


        /// <summary>Get registry item from Metreos Debug key</summary>
        public static string GetRegDbg(string subkey)
        {
            return Reg.GetStringValue(Const.RegistryDebugKey, subkey);
        }


        /// <summary>Set Metreos Debug key registry item</summary>
        public static void SetRegDbg(string subkey, string value)
        {
            Reg.SetStringValue(Const.RegistryDebugKey, subkey, value);
        }

        /// <summary>Get registry item from Metreos PrintSetup key</summary>
        public static string GetRegPrintSetup(string subkey)
        {
            return Reg.GetStringValue(Const.RegistryPrintSetupKey, subkey);
        }

        /// <summary>Set Metreos PrintSetup key registry item</summary>
        public static void SetRegPrintSetup(string subkey, string value)
        {
            Reg.SetStringValue(Const.RegistryPrintSetupKey, subkey, value);
        }

        /// <summary>Get full path to System Packages directory</summary>
        public static string PackagesFolder
        {
            get { return FrameworkDirectory + Const.bslash + FrameworkVersion + Const.packagesSubdirectory; }
        }


        /// <summary>Get configured or default framework version</summary>
        public static string FrameworkVersion
        { get 
          {   string s = FrameworkVer;  
              return s == null? Const.defaultFrameworkVersion: s; 
          }      
          set { FrameworkVer = value; }
        }

        public  const string DefaultAppServerIP   = Const.localhost;
        public  const string DefaultAppServerPort = "8120";

        public  const string DefaultDebuggerPort  = "8130";
        public  const string DefaultConsolePort   = "8140";

        public  const string DefaultAdminUser     = "Administrator";
        public  const string DefaultAdminPass     = "Metreos";

        public  const string DefaultSshPort       = "22";
        public const string DefaultSshTimeOut     = "10";

        public static readonly string DefaultInstallerName = Const.Installer;
        public static readonly string DefaultLocalesName = Const.Locales;

        public  const  int recentFileListSizeDefault = 4;
        public  const  int recentFileListSizeMax     = 12;

        public  static int RecentFileListSize 
        { get 
          {   string s = MaxMRU; int n = Utl.atoi(s);
              return n == 0? recentFileListSizeDefault: n; 
          }      
          set { MaxMRU = value.ToString(); }
        }

        public static bool LoadLastSavedOnStartup
        {
            get { string s = LoadOnStartup; return s != null && s == Const.sone; }
            set { LoadOnStartup = value? Const.sone: Const.szero; }
        }

        public static bool CopyMediaLocal
        {
            get { string s = CopyMediaFilesLocal; return s != null && s == Const.sone; }
            set { CopyMediaFilesLocal = value? Const.sone: Const.szero; }
        }
     
        public static bool UseDisplayNames
        {
            get { string s = ShowDisplayNames; return s == null? defaultDisplayNames: s != Const.szero; }
            set { ShowDisplayNames = value? Const.sone: Const.szero; }
        }

        public static DataTypes.UserVariableType DefPropertyType
        {   get 
            {   string s = DefaultPropertyType; 
                return IsValidDefPropertyType(s)? 
                    (DataTypes.UserVariableType) Enum.Parse(typeof(DataTypes.UserVariableType), s, true): 
                    Const.defPropTypeStr;
            }                   
            set 
            {
                DefaultPropertyType = value.ToString();
            }
        }

        private static bool IsValidDefPropertyType(string s)
        {
            return s != null && Enum.IsDefined(typeof(DataTypes.UserVariableType), s);
        }

        public static bool ConsoleBackground
        {
            get { string s = ConsoleBkgnd; return s != null && s == Const.sone; }
            set { ConsoleBkgnd = value? Const.sone: Const.szero; }
        }

        private static Framework.MaxRecentFileList recentFileList;
        public  static Framework.MaxRecentFileList RecentFiles { get { return recentFileList; } }

        public static LinkStyles UserLinkStyle 
        {
          get 
          {   string x = Config.LinkStyle;
              LinkStyles style = x == null? LinkStyles.None:
                  (LinkStyles)System.Enum.Parse(typeof(LinkStyles), x, true); 
              if (style == LinkStyles.None) style = defaultLinkStyle;
              return style;  
          }
        }

        public static string AdminUser
        {
          get
          {   string x  = Config.AppServerAdminUser;
              return x == null || x.Length < 1? DefaultAdminUser: x;
          }
        }


        public static string AdminPassword
        {
          get
          {   string x  = Config.AppServerAdminPass;
              return x == null || x.Length < 1? DefaultAdminPass: x;
          }
          set
          {
              string x = value == null || value.Length < 1? DefaultAdminPass: value;
              string s = Metreos.Utilities.Security.EncryptPassword(x);     
              Config.AppServerAdminPass = s;         
          }
        }


        public static string AppServerIpEx
        {
            get
            {
                string x  = Config.AppServerIP;
                return x == null || x.Length < 1? Config.DefaultAppServerIP: x;
            }
        }


        public static string AppServerPortEx
        {
            get
            {
                string x  = Config.AppServerPort;
                return x == null || x.Length < 1? Config.DefaultAppServerPort: x;
            }
        }


        public static string DebuggerPortEx
        {
            get
            {
                string x  = Config.DebuggerPort;
                return x == null || x.Length < 1? Config.DefaultDebuggerPort: x;
            }
        }


        public static string ConsolePortEx
        {
            get
            {
                string x  = Config.ConsolePort;
                return x == null || x.Length < 1? Config.DefaultConsolePort: x;
            }
        }


        public static string SshPort
        {
          get
          {
              string x  = Config.OpenSshPort;
              return x == null || x.Length < 1? Config.DefaultSshPort: x;
          }
        }


        public static bool UserLinkOrtho 
        {
          get
          {
              string x  = Config.LinkOrtho;
              return x == null? true: x != Const.szero;
          }
        }


        public static int UserLinkWidth 
        {
          get
          {
              string  x  = Config.LinkWidth;
              int n = x == null? 0: Utl.atoi(x);   
              return n < 1 || n > 3? defaultLinkWidth: n;
          }
        }


        public static int GridCellWidth
        {
          get
          {
              int gridW = Utl.atoi(Config.GridWidth);
              return (gridW < 4 || gridW > 32)? Config.defaultGridCellWidth: gridW;
          }
        }

        public static int GridCellHeight
        {
          get
          {
              int gridH = Utl.atoi(Config.GridHeight);
              return (gridH < 4 || gridH > 32)? Config.defaultGridCellHeight: gridH;
          }
        }

        public static bool SnapToGrid
        {
          get
          {
              string x  = Config.GridSnap;
              return x == null || x != Const.szero;
          }
        }

        public static bool WarningsAsError
        {
          get
          {
              string x  = Config.WarnAsError;
              return x != null && x == Const.sone;
          }
        }

        public static bool ShowCallAsList
        {
          get
          {
              string x  = Config.CallAsList;
              return x == null?  defaultCallAsList: x != Const.szero;
          }
        }

        public static bool WaitForPortMotion
        {
          get
          {
              string x  = Config.PortMotion;
              return x == null?  defaultPortMotion: x != Const.szero;
          }
        }

        public static bool LargePorts
        {
          get
          {
              string x  = Config.PortLarge;
              return x == null?  defaultPortLarge: x != Const.szero;
          }
        }

        public static bool VisiblePorts
        {
          get
          {
              string x  = Config.PortVisible;
              return x == null?  defaultPortVisible: x != Const.szero;
          }
        }


        public static bool SuppressNewScriptDialog
        {
            get
            {
                string x  = Config.SuppressAutoNewScript;
                return x == null?  defaultSuppressNewScript: 
                       defaultSuppressNewScript? x == Const.sone: x != Const.szero;
            }
        }


        public static bool SuppressNewProjectDialog
        {
            get
            {
                string x  = Config.SuppressAutoNewProject;
                return x == null?  defaultSuppressNewProject: 
                    defaultSuppressNewProject? x == Const.sone: x != Const.szero;
            }
        }


        public static readonly LinkStyles defaultLinkStyle = LinkStyles.Bezier;
        public const bool defaultLinkOrtho = true;
        public const int  defaultLinkWidth = 1;

        public const bool defaultCallAsList = true;

        public const bool defaultPortMotion  = false;
        public const bool defaultPortLarge   = true;
        public const bool defaultPortVisible = true;
        public const bool defaultSuppressNewScript  = false;
        public const bool defaultSuppressNewProject = false;

        public const bool defaultDisplayNames = true;

        public static readonly System.Text.Encoding MaxEncoding 
            = new System.Text.UTF8Encoding();  

        public static readonly Color linkColorDark       = Color.Black;  
        public static readonly Color linkColorNormal     = Color.SlateGray;        
        public static readonly Color linkColorLight      = Color.LightSlateGray;
        public static readonly Color linkColorExtraLight = Color.FromArgb(170,187,204);
        public static readonly Color linkHighlight       = Color.GreenYellow; 
 
        public static readonly Color selectionPrimary    = Color.FromArgb(186,248,124); 
        public static readonly Color selectionSecondary  = Color.Lavender; 
        public static readonly Color selectionNoFocus    = selectionPrimary; // Color.Gainsboro;

        public static readonly Color debugBreakHaloColor = Color.FromArgb(64,255,0,0);
        public const  int    debugBreakHaloWidth   = 5;

        public const  int    defaultGridCellWidth  = 8;
        public const  int    defaultGridCellHeight = 8;

        public  const float  textEditorFontSize    = 9.5F;

        public  const float  outputWindowFontSize  = 8.75F;
        public  const int    outputWindowFontGray  = 80;
        public  const bool   outputWindowWordWrap  = false;
        public  const int    outputWindowMaxLines  = 400;
        public  const int    outputWindowSaveLines = 160;

        public  const int    installerTabStop = 16; // pixels
        public  const int    databasesTabStop = 16;

        public  const int    minPixelsInToolboxDrag = 8;

        public static bool   defaultShowExplorerDetail = false;
        public static bool   ShowExplorerDetail { get { return defaultShowExplorerDetail; } }

        public static bool   ConstrainingCoordinates = true;
        public const  int    minNodeX  = 32;
        public const  int    minNodeY  = 32;
        public static readonly float minNodeXf = Convert.ToSingle(minNodeX);
        public static readonly float minNodeYf = Convert.ToSingle(minNodeY);

        public const  int traySensitivityTenths  = 4;     // tsecs before tray appears
        public const  int trayLagTenths          = 16;    // tsecs before tray hides
        public const  int initialTrayLagTenths   = 48;    // tsecs before hides on first open
        public const  int trayProximityThreshold = 5;     // node this close wakes tray

        public const  int annotationShowDelayMs  = 900;   // msecs before annotation appears
        public const  int annotationHideDelayMs  = 700;   // msecs before annotation disappears

        public static readonly Font  canvasFont = SystemInformation.MenuFont;
        public static readonly Color canvasTextColor = Color.SlateGray;
        public const  float          canvasFontSize  = 10.5F;
        public const  float          trayFontSize    = 9.5F;
        public const  int            minTrayHeight   = 30;

        public const  int minMaxSaveWidth  = 400;
        public const  int minMaxSaveHeight = 300;

        public static readonly SizeF InitialLoopFrameSize = new SizeF(120,120);

        public static bool EnableOverviewDragZoom = false;

        public static bool EnableUnconditionalLinks
        {
          get
          {
              string x  = Config.LinkUnconditional;
              return x != null && x == Const.sone;
          }
        }

        public static bool ExpandAppTreeOnLoad = false;

        public static bool ShowTabIcons = true;

        public static bool EnableTestMenu = false;

        public static bool EnableCallFunctionNode = true;

        public static bool StubCalledFunctions = false;

        public static bool VariableNamesCaseSensitive = true;

        public static bool RemoveMissingReferences = false;

        public static bool IsLinksRelinkable = false;

        public static void MaxPortConfig(GoPort port)
        {
            port.Brush  = null;
            port.Pen    = new Pen(Color.Transparent);
            port.Style  = GoPortStyle.None;
            port.FromSpot = 64; port.ToSpot = 256;
        } 

        // Print Setup related value accessors
        public static string Orientation
        { 
            get { return GetRegPrintSetup(Const.PrintSetup.OrientationSubKey); }
            set { SetRegPrintSetup(Const.PrintSetup.OrientationSubKey, value); }
        }

        public static string FitToPage
        { 
            get { return GetRegPrintSetup(Const.PrintSetup.FitToPageSubKey); }
            set { SetRegPrintSetup(Const.PrintSetup.FitToPageSubKey, value); }
        }

        public static string MarginL
        { 
            get { return GetRegPrintSetup(Const.PrintSetup.MarginLSubKey); }
            set { SetRegPrintSetup(Const.PrintSetup.MarginLSubKey, value); }
        }

        public static string MarginT
        { 
            get { return GetRegPrintSetup(Const.PrintSetup.MarginTSubKey); }
            set { SetRegPrintSetup(Const.PrintSetup.MarginTSubKey, value); }
        }

        public static string MarginR
        { 
            get { return GetRegPrintSetup(Const.PrintSetup.MarginRSubKey); }
            set { SetRegPrintSetup(Const.PrintSetup.MarginRSubKey, value); }
        }

        public static string MarginB
        { 
            get { return GetRegPrintSetup(Const.PrintSetup.MarginBSubKey); }
            set { SetRegPrintSetup(Const.PrintSetup.MarginBSubKey, value); }
        }

        public static bool LandscapeMode
        {
            get { return Orientation == null || Orientation.CompareTo(Const.PrintSetup.Portrait) != 0 ? true : false; }
            set { Orientation = value == true ? Const.PrintSetup.Landscape : Const.PrintSetup.Portrait; }
        }

        public static bool OnePagePerView
        {
            get { return FitToPage == null || FitToPage.CompareTo(Const.PrintSetup.FitToPage) == 0 ? true : false; }
            set { FitToPage = value == true ? Const.PrintSetup.FitToPage : Const.PrintSetup.NotFitToPage; }
        }

        public static Margins PageMargins
        {
            get 
            { 
                Margins m = new Margins();
                try
                {                    
                    m.Left = MarginL == null ? Convert.ToInt32(Const.PrintSetup.DefaultMarginL) : Convert.ToInt32(MarginL);
                    m.Right = MarginR == null ? Convert.ToInt32(Const.PrintSetup.DefaultMarginR) : Convert.ToInt32(MarginR);
                    m.Bottom = MarginB == null ? Convert.ToInt32(Const.PrintSetup.DefaultMarginB) : Convert.ToInt32(MarginB);
                    m.Top = MarginT == null ? Convert.ToInt32(Const.PrintSetup.DefaultMarginT) : Convert.ToInt32(MarginT);
                }
                catch
                {
                    m.Left = Convert.ToInt32(Const.PrintSetup.DefaultMarginL);
                    m.Right = Convert.ToInt32(Const.PrintSetup.DefaultMarginR);
                    m.Top = Convert.ToInt32(Const.PrintSetup.DefaultMarginT);
                    m.Bottom = Convert.ToInt32(Const.PrintSetup.DefaultMarginB);
                }
                return m;
            }

            set 
            { 
                MarginL = value.Left.ToString();
                MarginR = value.Right.ToString();
                MarginT = value.Top.ToString();
                MarginB = value.Bottom.ToString();    
            }
        }
    } // class Config

}   // namespace

