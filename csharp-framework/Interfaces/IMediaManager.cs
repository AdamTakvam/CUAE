using System;

namespace Metreos.Interfaces
{
	public abstract class IMediaManager
	{
        public abstract class Commands
        {
            public const string ProvisionMedia  = "ProvisionMedia";
            public const string GetStatus       = "GetStatus";
        }

        public abstract class Fields
        {
            // ProvisionMedia fields
            public const string MediaDir        = "MediaDirectory";
            public const string AppName         = "ApplicationName";

            // GetStatus response fields
            public const string Progress        = "Progress";
            public const string Error           = "Error";
        }
	}
}
