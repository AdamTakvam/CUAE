using System;
using Metreos.Max.Core;
using Metreos.Interfaces;   

namespace Metreos.Max.Framework
{
    /// <summary>
    ///     Constants only used for the build process 
    /// </summary>
    public abstract class ConstructionConst
    {
        public const int    MAXIMUM_FUNCTION_NODES = 10000;
        public const string enumPosition = "Enumerator not positioned correctly";
        public const string codingError  = "Coding error";
        public const int    DEFAULT_LOOP_COUNT = 1;
  
        // Xml Constants
        public const string xmlEltFinal = "final";
        public const string xmlAttrLabelTypeBasic = "Basic";
        public const string xmlAttrLabelTypeLabeled = "Labeled";
        public const string xmlAttrLog = "log";

        // Generated Node Strings
        public const string GENERATED_FUNCTION_PREPEND = "Loop";
        public const string underscore = "_";
        public const string OUTSIDE_ALL_FUNCTIONS = ".@#$%OutsideAnyLoop.#@$#";

        public const string DESCRIPTION_NOT_IMPLEMENTED = "Description not implemented";

        // Temporary directory used to extract the package.
        public const string extractionTempDir = "ext";

        // Serialization of Properties constants
        public const string xmlProperties = "Properties";

        public const string ACTION_PARAM_TYPE = "ActionParameterProperty";
        public const string RESULT_DATA_VAR   = "ResultDataProperty";

        // Metreos Action Matching
        public const string METREOS_LOG_WRITE = "Metreos.Native.Log.Write";
        public const string METREOS_LOG_PARAM_MESSAGE  = "Message";
        public const string METREOS_LOG_PARAM_LOGLEVEL = "LogLevel";
    
        // Automatic Logging Const
        public const string LOG_EXIT = "exit";
        public const string LOG_ENTRY = "entry";
        public const string LOG_NEXT_CONDITION = "default";

        // Anticpating Max Default Action
        public const string UNSPECIFIED_LINK_TEXT = "default";

        // Message Box errors from Deployment

        // THESE NEXT TWO PERTAIN TO IF AN APP IS ALREADY LOADED  
        public const string QUERY_APPLOADED_ALREADY  = "Application already installed"; // Caption
        public const string UNINSTALL_ABOUT_TO_OCCUR  
            = "The package will be reinstalled or upgraded";
    
        // Build messages 
        public static string buildStarted = "---- Build Started ----" + System.Environment.NewLine;
        public const string innerException = "---- Inner Exception ----";
        public const string constructingApplication = "Building ...";
        public const string packagingApplication    = "Packaging ...";
        public const string assemblingApplication   = "Assembling ...";

        public static string ConstructingScript(string scriptName)
        {
            return "Building '" + scriptName + "' ...";
        }

        public static string BuildEndedForScript(string scriptName, bool success)
        {
            return "Build " + (success ? "succeeded" : "failed") + " for '" + scriptName + "'";
        }

        public static string PreparingScript(string scriptName)
        {
            return "Preparing to build '" + scriptName + "' ...";
        }

        // Miscellaneous constants 
        public const string IN          = "in";
        public const string comma       = ",";
        public const string openParen   = "(";
        public const string closeParen  = ")";
        public const string NODE_NAME   = "name";
        public const string NODE_ID     = "id";
    }
}
