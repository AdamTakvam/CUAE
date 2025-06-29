using System;

namespace Metreos.MmsTester.Interfaces
{
	/// <summary>
	/// Summary description for IClientTypes.
	/// </summary>
	public abstract class IClientTypes
	{
        // Look up information for adapter assemblies
        public const string AS_EMULATOR_DISPLAY_NAME = "Application Server Emulator";
        public const string AS_EMULATOR = "Metreos.MmsTester.Custom.Clients.ApplicationServerEmulator";

        public const string CONFIGURATOR_DISPLAY_NAME = "Configurator";
        public const string CONFIGURATOR = "Metreos.MmsTester.Custom.Clients.Configurator";

        public const string USER_DRIVEN_COMMANDS_DISPLAY_NAME = "Simple Commands";
        public const string USER_DRIVEN_COMMAND = "Metreos.MmsTester.Custom.VisualInterfaces.SimpleCommands";
	}
}
