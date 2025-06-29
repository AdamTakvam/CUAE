namespace Metreos.MMSTestTool
{
    /// <summary>
    /// Holds constants used throughout the program. 
    /// </summary>
    public abstract class Constants
    {
        #region Command line constants
        public const string CL_PREFIX     = "--";
        public const string CL_PARAM_HELP = "help";
        public const string CL_PARAM_DEF = "definitions";
        public const string CL_PARAM_FILE = "file";
        public const string CL_XML_READ_DONE = "Done reading XML Command descriptions.";
        public const string CL_XML_READ_START = "Reading XML Command descriptions...";
        #endregion
    
        #region Parser constants
        public const string PARSER_EXEC_BEGIN = "Parser execution commencing...";
        public const string PARSER_EXEC_END = "Parser execution Complete.";
        #endregion

        #region Script Level constants
        public const string SLO_TIMEOUT = "commandTimeout";
        #endregion
            
        #region Session Manager Constants
        public const string SM_EXEC_BEGIN = "Session Manager execution commencing...";
        public const string SM_EXEC_END = "Session Manager execution Complete.";
        #endregion

        public const string FIXTURE_STRING = "Fixture";
        public const string SCRIPT_STRING = "Script";
        public const string COMMAND_STRING = "Command";
        public const string TAB = "     ";
        public const string NOT_EXECUTED = "Not yet Executed";

        #region GUI Constants
        public const string GUI_COMMAND_DESCRIPTION = "xmlCommandDescription";
        
        public static readonly Microsoft.Win32.RegistryKey regMain = Microsoft.Win32.Registry.LocalMachine;
        public static readonly string regMetKey = "Software\\" + System.Windows.Forms.Application.CompanyName;
        public static readonly string regMMSTesterKey = System.Windows.Forms.Application.ProductName;
        public const string regDescriptionKey = GUI_COMMAND_DESCRIPTION;
        #endregion

    }
}