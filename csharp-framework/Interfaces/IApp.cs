using System;

namespace Metreos.Interfaces
{
	/// <summary>
	/// Various fields which appear in Application XML
	/// </summary>
	public abstract class IApp
	{
        public enum DestructorCodes
        {
            Normal      = 0,
            Forwarded   = 1,
            Shutdown    = 2,
            Exception   = 3,
            ScriptError = 4,
            Timeout     = 5            
        }

        // FIELD_ constants are special field names in the XML
        public const string FIELD_RETURN_VALUE      = "returnValue";
        public const string FIELD_TIMEOUT           = "timeout";
        public const string FIELD_APP_NAME          = "applicationName"; // used internally

        // NAME_ constants are legal values of "name=" fields (others may exist)
        public const string NAME_FUNCTION_NAME      = "functionName";
		public const string NAME_TO_GUID			= "toGuid";
		public const string NAME_FROM_GUID			= "fromGuid";
		public const string NAME_EVENT_NAME			= "eventName";
        public const string NAME_LOOP_INDEX         = "loopIndex"; 
        public const string NAME_LOOP_ENUM          = "loopEnum";
        public const string NAME_LOOP_DICT_ENUM     = "loopDictEnum";

        public const string TYPE_ACTION_PROVIDER    = "provider";
        public const string TYPE_ACTION_NATIVE      = "native";

        public const string TYPE_PARAM_VARIABLE     = "variable";
        public const string TYPE_PARAM_CSHARP       = "csharp";

        // VALUE_ constants are special values for special field names (isn't that special?)
        public const string VALUE_DEFAULT           = "default";
        public const string VALUE_SUCCESS           = "success";
        public const string VALUE_FAILURE           = "failure";
		public const string VALUE_TIMEOUT			= "timeout"; // internal use only

        // RESULT_ constants are appended to action names to make async callback names
        public const string RESULT_COMPLETE         = "Complete";
        public const string RESULT_FAILED           = "Failed";

		// MSG_FIELD_ constants appear only in internal messaging
		public const string MSG_FIELD_FINAL			= "final";
    }
}
