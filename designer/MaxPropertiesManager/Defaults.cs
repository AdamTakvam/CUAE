using System;
using Metreos.ApplicationFramework.ScriptXml;


namespace Metreos.Max.Framework.Satellite.Property
{  

    public abstract class Defaults
    {
        public const string MetreosExit         = "Metreos.ApplicationControl.Exit";
        public const string MetreosExitFunction = "Metreos.ApplicationControl.ExitFunction";
        public static string blank  = " ";
        public static char   dot    = '.';
        public static string USER_DATA_DEFAULT  = "none";
        public static string HANDLER_ID_DEFAULT = "none";
        public static string TYPE  = "String";
        public static int maxAddRemComboWidth = 300;

        // Actions
        public static string DEFAULT = "Default";
        public static string USER_FRIENDLY_DEFAULT = "default";

        // Logging
        public static DataTypes.OnOff OVERALL_LOGGING = DataTypes.OnOff.On;

        public static DataTypes.OnOff INDIVIDUAL_LOGGING = DataTypes.OnOff.Off;

        public static System.Diagnostics.TraceLevel TRACE_LEVEL = System.Diagnostics.TraceLevel.Info;
        public static string TRACE_LEVEL_DESCRIPTION_BASE = "Choose the level of logging for this action\ncurrently: ";
        public static string TRACE_LEVEL_VERBOSE        = "\"Verbose\" is selected";
        public static string TRACE_LEVEL_INFO           = "\"Info\" is selected";
        public static string TRACE_LEVEL_WARNING        = "\"Warning\" is selected";
        public static string TRACE_LEVEL_ERROR          = "\"Error\" is selected";
        public static string ON_OFF_DESCRIPTION_BASE    = "Turn logging on or off\nCurrently: ";
        public static string ON_OFF_IS_ON               = "\"On\" is selected";
        public static string ON_OFF_IS_OFF              = "\"Off\" is selected";

        public static string LOG_ON_ENTRY_DESCRIPTION   = "Text to be logged immediately before the action executes";
        public static string LOG_ON_EXIT_DESCRIPTION    = "Text to be logged immediately after the action executes";
        public static string LOG_ON_SUCCESS_DESCRIPTION = "Text to be logged after the action executes, if the action result is success";
        public static string LOG_ON_FAILURE_DESCRIPTION = "Text to be logged after the action executes, if the action result is failure";
        public static string LOG_ON_TIMEOUT_DESCRIPTION = "Text to be logged after the action executes, if the action result is timeout";
        public static string LOG_ON_DEFAULT_DESCRIPTION = "Text to be logged after the action executes, if the action result is default";

        public static string USER_DATA_ACTION_PROPERTY_DESC = "Determines which function will handle the AsyncCallbacks of this function";
        public static string HANDLER_ID_PROPERTY_DESC = "Unique tag associated with each event handler instance";
        public static string RETURN_VALUE_DESC = "All applicable return values for this action";

        // Events
        public static DataTypes.RegexType REGEX         = DataTypes.RegexType.String;
        public static DataTypes.EventParamType EVENT_PARAM_TYPE = DataTypes.EventParamType.literal;
        public static string EVENT_PARAM_TYPE_DESC      = "Choose the matching value type of this parameter";
        public static string USER_DATA_DESC             = "Used internally to determine which function handles which event";

        // Link Styles
        public static DataTypes.LinkStyle LINK_STYLE    = DataTypes.LinkStyle.Bezier;

        // Looping
        public static string LOOP_COUNT = "1";
        public static string LOOP_COUNT_DESCRIPTION = "Number of times the loop is to execute";
        public static string INDEX_NAME_DESCRIPTION = "The name with which to access the loop count";
        public static DataTypes.LoopIterateType LOOP_ITERATE_TYPE = DataTypes.LoopIterateType.@int;

        // Start Node
        public static string START_NODE_DESCRIPTION     = "The entry point for the function";

        // Label
        public static string LABEL_DESCRIPTION          = "Represents either end of a 'go to' construct";
        // jld added
        public static string VARIABLETOOL_DESCRIPTION   = "A script variable";
		
        // User Type and Loop Type
        public static string BASE_DESCRIPTION           = "Choose the type of value this element represents\ncurrently: ";
        public static string STRING_DESCRIPTION         = "Literal string.";
        public static string INTEGER_DESCRIPTION        = "Literal integer.";
        public static string CSHARP_DESCRIPTION         = "C# resolvable string.";
        public static string VARIABLE_DESCRIPTION       = "A variable within this element's scope.";
        public static paramType LOOP_TYPE               = paramType.literal;

        // Loop Iteratate Type
        public static string ITERATE_BASE_DESCRIPTION   = "Choose the iterator type for this loop\ncurrently:";
        public static string INT_DESCRIPTION            = "An integer value.  The loop will iterate 'int' number of times";
        public static string ENUM_DESCRIPTION           = "An enumerator.  The loop will iterate through the collection.";
        public static string DICT_ENUM_DESCRIPTION      = "A dictionary enumerator.  The loop will iterate through the dictionary collection.";
        public static loopCountEnumType LOOP_ITERATE_TYPE_METREOS = loopCountEnumType.@int;

        // Trigger Property
        public static string TRIGGERING_PROPERTY_BASE_DESCRIPTION = "Determines the type of function this event can invoke.  ";
        public static string TRIGGERING_DESCRIPTION               = "This event can only be used to initiate an application.";
        public static string NONTRIGGERING_DESCRIPTION            = "This event can only be used to invoke a function in a running application.";
        public static string HYBRID_DESCRIPTION                   = "This event can be used to either initiate an application, or invoke a function in a running application.";
    
        // AppCanvas Property
        public static string SCRIPT_INSTANCING_TYPE_MULTI   = "Multiple instances of this application can be loaded simultaneously";
        public static string SCRIPT_INSTANCING_TYPE_SINGLE  = "Only one instance of this application can be loaded at any time";
        public static string SCRIPT_TYPE_MASTER_DESC        = "Does not require provisioning from another script to trigger.";
        public static string SCRIPT_TYPE_SLAVE_DESC         = "Requires provisioning from another script to trigger.";

        public static string VERSION = "1.0";
        public static string APP_DISPLAY_NAME_META_DESC = "Application display name.  Optional.";
        public static string APP_DESCRIPTION_META_DESC  = "Application description.  Optional.";
        public static string APP_COMPANY_META_DESC      = "Application company name.  Optional.";
        public static string APP_AUTHOR_META_DESC       = "Application author name.  Optional.";
        public static string APP_COPYRIGHT_META_DESC    = "Application copyright statement.  Optional.";
        public static string APP_TRADEMARK_META_DESC    = "Application trademark statement.  Optional.";
        public static string APP_VERSION_META_DESC      = "Application version.  Optional.";

        // Variable Property
        public const string VARIABLE_TYPE_DESCRIPTION   = "Define the type of the variable.";
        public const string DEFAULT_INIT_WITH_DESC      = "The variable will be set to this value if initialization from configuration failed or was not specified.";
        public const string CONFIG_INIT_WITH_DESC       = "The variable will be set to this value, if specified and if a valid initial value.";

        // Reference Property
        public const string REFERENCE_TYPE_REFERENCE_DESC     = "This variable will contain a reference.";
        public const string REFERENCE_TYPE_VALUE_DESC         = "This variable will contain a value.";
        public const DataTypes.ReferenceType REFERENCE_TYPE   = DataTypes.ReferenceType.reference;
        public const parameterTypeType REFERENCE_TYPE_METREOS = parameterTypeType.reference;

        // Code Property
        public const string INITIAL_USER_CODE           = "\npublic static string Execute()\n{" +
            "\n\t// TODO: add parameters with same name and type as variables" +
            "\n\t// TODO: add function body\n\n}\n";
        public const  string USER_CODE_DESCRIPTION       = "Custom user code.";
        public static string CUSTOM_CODE_PROPERTY_FILLER = String.Empty;
        public const  string BASE_LANGUAGE_DESCRIPTION   = "Specify the language to be used in this custom code:\n";
        public const  string CSHARP_LANGUAGE_DESCRIPTION = "Current language is C#";
        public const  languageType USER_CODE_LANGUAGE_METREOS = languageType.csharp;
        public const  DataTypes.AllowableLanguages USER_CODE_LANGUAGE = DataTypes.AllowableLanguages.csharp;

        // Media File Property 
        public const  string MEDIA_FILE_DESCRIPTION  = "Choose a media file.";
        public const  string MediaServerProviderNs   = "Metreos.MediaControl";
        public const  string PlayAnnouncement        = "Play";
        public const  string PLAY_ANN_FILE_CHOOSE    = "Prompt";

        // Complex Type Property
        public const string COMPLEX_TYPE_DESCRIPTION = "A complex type defined by a web service.";

        // Xml Doc Prepend
        public const string xmlDocPrepend = @"<?xml version=""1.0"" encoding=""utf-8"" ?>";

        // Serialization Xml const
        public const string xmlEltProperties = "Properties";
    
        // Action Node Serialization
        public const string xmlEltActionParameter = "ap";
        public const string xmlEltResultData    = "rd";
        public const string xmlEltLogging       = "log";
        public const string xmlEltTimeout       = "timeout";
        public const string xmlAttrType         = "type";
        public const string xmlAttrFinal        = "final";
        public const string xmlAttrOn           = "on";
        public const string xmlAttrCondition    = "condition";
        public const string xmlAttrLevel        = "level";
        public const string xmlAttrLog          = "log";
  
        // Event node Serialization
        public const string xmlEltEventParameter = "ep";
        public const string xmlAttrField        = "field";

        // Function node serialization
        public const string xmlAttrName         = "name";
    
        // Link node serialization
        public const string xmlAttrText         = "text";
        public const string xmlAttrStyle        = "style";
  
        // Loop node serialization
        public const string xmlAttrIndexName    = "index";
        public const string xmlAttrLoopIteratorType = "iteratorType";
    
        // Variable node serialization
        public const string xmlAttrInitWith         = "initWith";
        public const string xmlAttrDefaultInitWith  = "defaultInitWith";
        public const string xmlAttrRefType          = "refType";

        // Script serialization
        public const string xmlAttrInstanceType = "instanceType";

        // Project serialization
        public const string xmlAttrDescription  = "desc";
        public const string xmlAttrCompany      = "company";
        public const string xmlAttrAuthor       = "author";
        public const string xmlAttrCopyright    = "copyright";
        public const string xmlAttrTrademark    = "trademark";
        public const string xmlAttrVersion      = "version";
        public const string xmlEltUsing         = "using";

        // User Code serialization
        public const string xmlAttrLanguage     = "language";
    
        // Deserialization Errors
        public const string ERROR_DESERIALIZE_ACTION_NODE   = "Could not deserialize action node.";
        public const string ERROR_DESERIALIZE_EVENT_NODE    = "Could not deserialize event node.";
        public const string ERROR_DESERIALIZE_FUNCTION_NODE = "Could not deserialize function node.";
        public const string ERROR_DESERIALIZE_LINK_NODE     = "Could not deserialize link.";
        public const string ERROR_DESERIALIZE_LOOP_NODE     = "Could not deserialize loop node.";
        public const string ERROR_DESERIALIZE_VARIABLE_NODE = "Could not deserialize variable node.";
        public const string ERROR_DESERIALIZE_SCRIPT        = "Could not deserialize script properties.";
        public const string ERROR_DESERIALIZE_PROJECT       = "Could not deserialize project properties.";
        public const string ERROR_DESERIALIZE_USER_CODE     = "Could not deserialize user code node.";

        // Image Names
        public const string OnImage = "on." ;

        // Property Grid Description manipulation
        public const string REQUIRED_PARAM_APPEND = "(required)";
        public const string GUARANTEED_PARAM_APPEND = "(guaranteed)";

        // Messages for user message box
        public const string noActionParamName         = "Specify a parameter name.";
        public const string invalidActionParamName    = "Specify a valid parameter name.";
        public const string duplicateActionParamName  = "Duplicate names not allowed.";
        public const string noLogEvent                = "Specify a log event name.";
        public const string noResultParam             = "Specify a result variable name.";
        public const string noReplicateParam          = "Specify a parameter to replicate.";

        // Prepends for Config property types
        // locale types
        public const string LocalePrepend = "Locale";
        public const string ConfigPrepend = "Config";
    }
}
