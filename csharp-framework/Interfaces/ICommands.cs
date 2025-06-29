using System;

namespace Metreos.Interfaces
{
	/// <summary>Fields used in command messages</summary>
	public abstract class ICommands
    {
        // Router
        public const string REGISTER_SCRIPT             = "RegisterScript";
        public const string SCRIPT_ENDED                = "ScriptEnded";
        public const string SESSION_ENDED               = "SessionEnded";
        public const string ENABLE_APP		            = "EnableAplication";
        public const string DISABLE_APP		            = "DisableApplication";

        // ProviderManager
        public const string REGISTER_PROV_NAMESPACE     = "RegisterProviderNamespace";
        public const string UNREGISTER_PROV_NAMESPACE   = "UnregisterProviderNamespace";
        public const string DISABLE_PROVIDER			= "DisableProvider";
        public const string ENABLE_PROVIDER				= "EnableProvider";
        public const string INSTALL_PROVIDER			= "InstallProvider";
        public const string UNINSTALL_PROVIDER			= "UninstallProvider";
        public const string RELOAD_PROVIDER				= "ReloadProvider";
        public const string REFRESH_PROVIDERS           = "RefreshProviders";
        public const string UNHANDLED_EXCEPTION         = "UnhandledException";

        // AppManager
        public const string INSTALL_APP                 = "InstallApplication";
        public const string UNINSTALL_APP               = "UninstallApplication";
        public const string UPDATE_APP                  = "UpdateApplication";
        public const string RELOAD_APP		            = "ReloadApplication";
        public const string DISABLE_APP_INSTALL         = "DisableApplicationInstallation";
        public const string ENABLE_APP_INSTALL          = "EnableApplicationInstallation";

		// Debugging
        public const string START_DEBUGGING             = "StartDebugging";
		public const string SET_BREAKPOINT				= "SetBreakpoint";
        public const string CLEAR_BREAKPOINT            = "ClearBreakpoint";
		public const string STOP_DEBUGGING				= "StopDebugging";
		public const string EXEC_ACTION					= "ExecuteAction";
        public const string RUN                         = "Run";
        public const string UPDATE_VALUE                = "UpdateValue";
        public const string BREAK                       = "Break";
		public const string HIT_BREAKPOINT				= "HitBreakpoint";
        public const string GET_BREAKPOINTS             = "GetBreakpoints";

        // Telephony Manager
        public const string CLEAR_CALL_TABLE            = "ClearCallTable";
        public const string PRINT_DIAGS                 = "PrintDiags";
        public const string END_ALL_CALLS               = "EndAllCalls";
        public const string CLEAR_CRG_CACHE             = "ClearCrgCache";

        // Any Task
        public const string STARTUP                     = "Startup";
        public const string SHUTDOWN                    = "Shutdown";
        public const string SET_LOG_LEVEL               = "SetLogLevel";
        public const string REFRESH_CONFIG		        = "RefreshConfiguration";

        public abstract class Fields
        {
            public const string LICENSE_MANAGER     = "LicenseManager";
            public const string PROVIDER_NAME		= "ProviderName";
            public const string PROVIDER_NAMESPACE  = "providerNamespace";

            public const string ACTION_GUID         = "ActionGuid";  // For test framework only
			public const string SESSION_GUID        = "SessionGuid";
			public const string ROUTING_GUID        = "RoutingGuid";
            public const string FILENAME            = "Filename";
            public const string APP_NAME            = "ApplicationName";
            public const string APP_QUEUE           = "ApplicationQueueWriter";
            public const string SCRIPT_NAME		    = "ScriptName";
            public const string EVENT_NAME          = "EventName";
            public const string PARTITION_NAME      = "PartitionName";
            public const string CULTURE             = "Culture";
            public const string NUM_HITS            = "NumHits";
            public const string ENABLED             = "Enabled";
            public const string TRANS_ID			= "TransactionID";
            public const string USER_DATA           = "UserData";
            public const string HANDLER_ID          = "HandlerId";
            public const string CC_PROTOCOL         = "CallControlProtocol";
            public const string EXCEPTION           = "Exception";

			public const string IMMEDIATELY         = "immediately";
			public const string FAIL_REASON         = "reason";
			public const string FAIL_CODE           = "reasonCode";
            public const string LOG_LEVEL           = "logLevel";

            // Debug stuff
			public const string DEBUG_ACTION_ID		= "DebugCurrentActionId";
			public const string ACTION_RESULT		= "DebugActionResult";
			public const string FUNCTION_VARS		= "DebugFunctionVariables";
			public const string SCRIPT_VARS			= "DebugScriptVariables";
			public const string SESSION_DATA		= "DebugSessionData";
			public const string ACTION_STACK		= "DebugActionStack";
			public const string STEP_INTO			= "DebugStepInto";
            public const string VAR_NAME            = "VariableName";
            public const string VAR_VALUE           = "VariableValue";
        }
	}
}
