using System;

namespace Metreos.Interfaces
{
	/// <summary>
	/// Fields used in response messages
	/// </summary>
	public abstract class IResponses
	{
        public const string TIMEOUT             = "timeout";

        public const string STARTUP_COMPLETE    = "StartupComplete";
        public const string STARTUP_FAILED      = "StartupFailed";
        public const string SHUTDOWN_COMPLETE   = "ShutdownComplete";
        public const string SHUTDOWN_FAILED     = "ShutdownFailed";
        public const string RELOAD_COMPLETE     = "ReloadComplete";
        public const string RELOAD_FAILED       = "ReloadFailed";
        public const string REFRESH_COMPLETE    = "RefreshComplete";
        public const string PONG                = "PONG";

        public abstract class Fields
        {
            public const string EXT             = "Extension";
        }
	}
}
