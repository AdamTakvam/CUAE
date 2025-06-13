using System;


namespace Metreos.Max.Framework.Satellite.Property
{
  /// <summary>
  /// Used to hold the names of properties and categories.
  /// </summary>
  public abstract class DataTypes
  {
    /// <summary>
    /// The type of object to be displayed in the Property Grid.
    /// </summary>
    public const string PACKAGE_NAME                  = "Package Name";
    public const string EVENT_NAME                    = "Event Name";
    public const string ACTION_NAME                   = "Action Name";
    public const string ACTION_TYPE                   = "Type";
    public const string TRIGGER_NAME                  = "Type of Event";
    public const string RETURN_VALUE                  = "returnValue";
    public const string EXIT_FUNC_RETURN_VALUE_CATEGORY = "Return Value";
    public const string ACTION_PARAMETERS_CATEGORY    = "Action Parameters";
    public const string EVENT_CATEGORY                = "Event Parameters";
    public const string EVENT_TYPE                    = "__Event Type__";
    public const string COUNT                         = "Count";
    public const string DESCRIPTION                   = "Description";
    public const string ALLOW_CUSTOM_PARAMETERS       = "Allow Custom Parameters";
    public const string RETURN_VALUE_CATEGORY         = "Return Value";
    public const string LOGGING                       = "Logging";
    public const string LOGGING_GROWABLE              = "Additional logging";
    public const string RESULT_DATA_CATEGORY          = "Result Data";
    public const string COMMENT_TEXT                  = "Text";
    public const string FINAL                         = "Final";
    public const string ASYNC_CALLBACK                = "AsyncCallback";
    public const string UNNAMED                       = "Unnamed";
    public const string USERTYPE                      = "User Type";
    public const string LOOP_TYPE_NAME                = "Loop Type";
    public const string LOOP_ITERATE_TYPE_NAME        = "Loop Iteration Type";
    public const string TRACELEVEL                    = "Trace Level";
    public const string LOG_ON_ENTRY                  = "entry";
    public const string LOG_ON_EXIT                   = "exit";
    public const string LOG_ON_SUCCESS                = "success";
    public const string LOG_ON_FAILURE                = "failure";
    public const string LOG_ON_TIMEOUT                = "timeout";
    public const string LOG_ON_DEFAULT                = "default";
    public const string LOG_ON_YES                    = "yes";
    public const string LOG_ON_NO                     = "no";
    public const string LOG_ON_TRUE                   = "true";
    public const string LOG_ON_FALSE                  = "false";
    public const string INDEX_NAME                    = "Index Name";
    public const string REGEX_TYPE                    = "Match Type";
    public const string BASIC_PROPERTIES              = "Basic Properties";
    public const string LINKSTYLE                     = "Link Style";
    public const string LINKSTYLE_DESCRIPTION         = "Line Style determines the visual appearance and behavior of the line";
    public const string LINKSTYLE_CATEGORY            = "Link Style Properties";
    public const string FUNCTION_NAME                 = "Function Name";
    public const string SCRIPT_VERSION                = "Version";
    public const string VARIABLE_TYPE                 = "Type";
    public const string LOCAL_VAR_INIT_WITH           = "InitializeWith";
    public const string VARIABLE_TYPES                = "Required Type";
    public const string LABEL_NAME                    = "Label Name";
    public const string VARIABLE                      = "Variable Name";  // jld added
    public const string GENERIC_EVENT_DESCRIPTION     = "An event";
    public const string GENERIC_PACKAGE_DESCRIPTION   = "A package";
    public const string RESULT_DATA                   = "Result Data";
    public const string STYLE_PROPERTY_DESCRIPTION    = "Select the desired style of this link";
    public const string RESULT_DATA_DESCRIPTION       = "A result data parameter";
    public const string USER_TYPE_DESCRIPTION         = "Select the type of value you would like to pass in";
    public const string CUSTOM_ACTION_PARAMETERS      = "Custom Action Parameters";
    public const string RESULT_DATA_GROWABLE_NAME     = "Result Data";
    public const string RETURN_VALUE_DESCRIPTION      = "The return value of this function.";
    public const string NAME                          = "Name";
    public const int FIRST_NAMING_INDEX               = 1;
    public const bool UPDATE_ON_INDIVIDUAL_PROPERTY_CHANGE = false;
    public const string APP_DISPLAY_NAME_META         = "Display Name";
    public const string APP_DESCRIPTION_META          = "Description";
    public const string APP_COMPANY_META              = "Company";
    public const string APP_AUTHOR_META               = "Author";
    public const string APP_COPYRIGHT_META            = "Copyright";
    public const string APP_TRADEMARK_META            = "Trademark";
    public const string APP_VERSION_META              = "Version";
    public const string SCRIPT_TYPE_NAME              = "Script Type";
    public const string SCRIPT_INSTANCING_TYPE_NAME   = "Script Instance Type";
    public const string SCRIPT_DESCRIPTION            = "Description";
    public const string USINGS                        = "Usings";
    public const string USING                         = "Using";
    public const string USER_CODE                     = "Code";
    public const string DEFAULT_INIT_WITH             = "DefaultValue";
    public const string CONFIG_INIT_WITH              = "Config";
    public const string REFERENCE_TYPE_NAME           = "Reference Type";
    public const string LANGUAGE                      = "Language";
    public const string EVENT_PARAM_TYPE              = "Type";
    public const string TIMEOUT_ACTIONPARAM_NAME    = "timeout";

    public enum Type
    {
      /// <summary>A read-only package typically found in the ToolBox window</summary>
      Package,
      /// <summary>A read-only member of a package typically found in the ToolBox window</summary>
      Action,
      /// <summary>A read-only membor of a package typically found in the ToolBox window</summary>
      Event,
      /// <summary>A containing data link between actions</summary>
      Link,
      /// <summary>The parameters of an event.</summary>
      EventParameters,
      /// <summary>Symbol found at the application canvas level</summary>
      Function,
      /// <summary>An action found on the canvas</summary>
      ActionInstance,
      /// <summary>An event found on the canvas</summary>
      EventInstance,
      /// <summary>Function canvas entry point</summary>
      StartNode,
      /// <summary>A GoTo label node enabling link discontinuity</summary>
      Label,
      /// <summary>Specific override for an exit action found on the canvas</summary>
      ExitAction,
      /// <summary>A sticky-note-styled comment node</summary> 
      Comment,
			/// <summary>A container for other nodes indicating iteration</summary>
      Loop,
      /// <summary>A node representing the local variable type</summary>
      LocalVariable,
      /// <summary>A node representing the global variable type</summary>
      GlobalVariable,
      /// <summary>A custom code snippet action</summary>
      Code,
      /// <summary>The one and only project</summary>
      Project,
      /// <summary>A samoa script</summary>
      Script,
      /// <summary>Null type placeholder</summary>
      Nothing 
    }

    /// <summary>Enumerates the types of elements found in package</summary>
    public enum TypeOfOccurrence
    {
      /// <summary>Native or provider action</summary>
      Action,
      /// <summary>A provider event</summary>
      Event
    }
  
    /// <summary>Enumerates the various styles available for a link</summary>
    public enum LinkStyle
    {
      /// <summary>Curved link style</summary>
      Bezier,
      /// <summary>Straight lines and 90 degree corners link style</summary>
      HardEdged
    }

    /// <summary>Enumerates the various types for a variable a user can define</summary>
    /// <remarks>Mimics the types available in the Application Server ARE</remarks>
    public enum UserVariableType
    {
      /// <summary>A constant string entered by the user</summary>
      literal,
      /// <summary>A variable found within the current scope of the function</summary>
      variable,
      /// <summary>Custom csharp code that ultimately results in an expected value</summary>
      csharp
    }

    /// <summary>Enumerates the various types for a loop Count</summary>
    /// <remarks>Mimics the values possible in the Application Server ARE</remarks>
    public enum LoopType
    {
      literal,
      variable,
      csharp
    }

    /// <summary>Enumerates the various types of iteration controller, whether
    ///          it be an integer describing the number of times to loop, or use 
    ///          an enum or dictionary enum to controll the number of iterations</summary>
    public enum LoopIterateType
    {
      @int,
      @enum,
      dictEnum
    }

    /// <summary>Enumerates the two types of event parameters</summary>
    /// <remarks>Mimics the values possible in the Application Server ARE</remarks>
    public enum EventParamType
    {
      literal,
      variable
    }

    /// <summary>Enumerates the possible values for an event parameter</summary>
    /// <remarks>Mimics the values possible in the Application Server ARE</remarks>
    public enum RegexType
    {
      /// <summary>Matches a direct 1:1 string</summary>
      String,
      /// <summary>Uses a regex to match</summary>
      Regex
    }

    /// <summary>The values possible for logging</summary>
    public enum ReturnValues
    {
      Yes,
      No, 
      Success,
      Default,
      Failure,
      Timeout,
      True,
      False
    }

    /// <summary>Indicates if logging is on or off</summary>
    public enum OnOff
    {
      On,
      Off
    }

    /// <summary>The two reference-type values for local variables</summary>
    /// <remarks>Mimics the values of the Application Server ARE for script types</remarks>
    public enum ReferenceType
    {
      reference,
      @value,
    }

    /// <summary>The languages which can be compiled into custom user code</summary>
    /// <remarks>Mimics the values of the Application Server ARE for allowed languages</remarks>
    public enum AllowableLanguages
    {
      csharp
    }
  }
}
