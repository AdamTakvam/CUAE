using System;

namespace Metreos.Interfaces
{
    public abstract class IManagement
    {
        public enum Commands
        {
            LogIn,
            RefreshConfiguration,
            EnableProvider,
            DisableProvider,
            InvokeExtension,
            InstallProvider,
            UninstallProvider,
            ReloadProvider,
            InstallApplication,
            UpdateApplication,
            EnableApplication,
            DisableApplication,
            UninstallApplication,
            AddMediaServer,
            RemoveMediaServer,
            GetProvisioningStatus,
            DisableApplicationInstallation,
            EnableApplicationInstallation,
            GetApps,

            // TM commands
            PrintDiags,
            EndAllCalls,
            ClearCallTable,
            ClearCrgCache,

            // Secret weapons
            GarbageCollect
        }

        public abstract class ParameterNames
        {
            public const string TYPE        = "ComponentType";      // Used for routing the message internally
            public const string NAME        = "ComponentName";      // Used for routing the message internally
            public const string APP_NAME    = "ApplicationName";
            public const string EXT_NAME    = "ExtensionName";
            public const string USERNAME    = "Username";
            public const string PASSWORD    = "Password";
        }
    }
}
