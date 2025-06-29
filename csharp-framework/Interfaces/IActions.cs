using System;

namespace Metreos.Interfaces
{
	/// <summary>Definitions of internally-handled actions.</summary>
	public abstract class IActions
	{
        public const string AppControlNamespace     = "Metreos.ApplicationControl";
		public const string SendEvent	            = AppControlNamespace + ".SendEvent";
		public const string Forward		            = AppControlNamespace + ".Forward";
        public const string EndScript               = AppControlNamespace + ".EndScript";
        public const string EndSession              = AppControlNamespace + ".EndSession";
        public const string EndFunction             = AppControlNamespace + ".EndFunction";
        public const string CallFunction            = AppControlNamespace + ".CallFunction";
        public const string EnableScript            = AppControlNamespace + ".EnableScript";
        public const string ConstructionComplete    = AppControlNamespace + ".ConstructionComplete";
        public const string SetSessionData          = AppControlNamespace + ".SetSessionData";
        public const string ChangeLocale            = AppControlNamespace + ".ChangeLocale";
        public const string Sleep                   = AppControlNamespace + ".Sleep";

        public const string IntActionNamespace      = "Metreos.InternalActions";
        public const string NoHandler               = IntActionNamespace + ".NoHandler";
        public const string Ping                    = IntActionNamespace + ".ProviderPing";

        // This is sent to a provider when the response timeout expires
        public const string CancelOperation         = IntActionNamespace + ".CancelOperation";

        public abstract class Fields
        {
            public const string InnerMsg        = "InnerMessage";
            public const string ToGuid          = "ToGuid";
            public const string DestGuid        = "DestinationGuid";
            public const string FunctionName    = "FunctionName";
            public const string ReturnValue     = "ReturnValue";
            public const string Locale          = "Locale";
            public const string ResetStrings    = "ResetStrings";
            public const string Success         = "Success";
            public const string SleepTime       = "SleepTime";
            public const string SessionActive   = "SessionActive";
        }
	}
}
