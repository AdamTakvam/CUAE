using System;

namespace Metreos.Interfaces
{
	/// <summary>
	/// Summary description for IMmsManagement.
	/// </summary>
    public abstract class IMmsManagement
    {
        public enum Commands
        {
            Start,
            Stop,
            UpdateConfig,
            GetStatus,
            ProvisionMedia
        }

        public abstract class ParameterNames
        {
            public const string UNC_PATH        = "UncPath";
            public const string FILENAME        = "Filename";
            public const string DATA            = "Data";
        }
	}
}
