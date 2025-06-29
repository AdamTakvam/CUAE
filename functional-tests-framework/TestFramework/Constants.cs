using System;

namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>
	/// Summary description for Constants.
	/// </summary>
	public abstract class Constants
	{
        public enum CallRouteGroupTypes
        {
            H323,
            CTI,
            SCCP,
            SIP,
            Test
        }
        public const string FUNCTIONAL_TEST_PROVIDER_QUEUE = "FunctionalTestProvider";

        // Settings constants
        public const string FrameworkDir = @"x:\Build\Framework\1.0";
        public const string CompiledTestsFolder = @"x:\Build\FunctionalTest\FunctionalTests";
        public const string CompiledMaxTestsFolder = @"x:\Build\FunctionalTest\CompiledTests";
        public const string DefaultVersion = "1.0";
        public const string DefaultPassword = "metreos";
        public const string DefaultUsername = "Administrator";
        public const string DefaultSamoaIp = "127.0.0.1";
        public const string DefaultSamoaPort = "8120";
        public const string DefaultTestClientPort = "8142";
        public const string DefaultTestSettingPath = "TestSettings.txt";
        public const string DefaultPollTimes = "60";
        public const string CallManagerIp = "192.168.1.250";
        public const int    DefaultStartRange = 69000;
        public const int    DefaultEndRange = 69999;
        public const bool   DefaultNeverUninstall = false;

        public const string NEW_LINE                            = "\r\n";
        public const string SPACE                               = " ";
        public const string packageExtension                    = ".mca";
        public const string maxSlnFileExtension                 = ".max";
        public const string FUNCTIONAL_TESTER_MACHINE_NAME      = "machineName";
        public const string @namespace                          = "Metreos.Providers.FunctionalTest";
        public const string secondaryNamespace                  = "Metreos.FunctionalTest";
        public const string FTProvider                          = "FTProvider";
        public const string TEST_SCRIPT_NAME                    = "testScriptName";
        public const string UNIQUE_EVENT_PARAM                  = "uniqueEventParam";
        public const string ServerId                            = "ServerId";

        /// <summary>
        /// Used by tests to register signals with the provider. When a signal
        /// that has been registered is received from an application, it is passed to the
        /// test for processing.
        /// </summary>
        public const string MSG_REGISTER_SIGNAL     = @namespace + ".RegisterSignal";
        public const string EVENT_TRIGGER_APP       = @namespace + ".TriggerApp";
        public const string EVENT_SEND_EVENT        = @namespace + ".Event";
        public const string EVENT_SEND_RESPONSE     = @namespace + ".SendResponse";
        public const string ACTION_SIGNAL           = @namespace + ".Signal";
        public const string SECONDARY_ACTION_SIGNAL = secondaryNamespace + ".Signal";
        public const string FIELD_SIGNAL_NAME       = "signalName";
        public const string FIELD_EVENT_NAME        = "eventName";
        public const string FIELD_RESPONSE_CODE     = "responseCode";
		public const string FIELD_RESULT_VAR		= "resultVar";
        public const string HTML_FOOTER             = "</body></html>";
        public const string PROVIDER_QUEUE_NAME     = "provider.functionaltestprovider";
		public const string CISCO_EXECUTE_PATH      = "/CGI/Execute";
        public const string LOG_MESSAGE             = "message";
        public const string LOG_LEVEL               = "level";

        // OAM constants
        public const int CLIENT_PORT                = 8020;
        public const int COMMAND_TIMEOUT            = 15000;
        public const string FILE_NAME               = "Metreos.FunctionalTests.OamMessenger.exe";
        public const string TEST_NATIVE_TYPE        = "Metreos.Types.TestString";
        public const string DEFAULT_TYPE_STRING     = "defaultString";
        public const string ASSIGNED_TYPE_STRING    = "assignedString";
        public const string STARTUP_COMPLETE        = "Startup Complete";

        // Remoting Constants
        public const int serverPort                     = 8220;
        public const int clientPort                     = 8320;
        public const int FTBServerPort                  = 8420;
        public const string serverProxyUri              = "serverProxy";
        public const string clientProxyUri              = "clientProxy";
        public const string componentType               = "componentType";
        public const string componentName               = "componentName";
        public const string configName                  = "configName";
        public const string configValue                 = "configValue";
        public const string configDescription           = "configDescription";
        public const string configFormatName            = "configFormatName";
        public const string appName                     = "appName";
        public const string scriptName                  = "scriptName";
        public const string partitionName               = "partitionName";
        public const string paramName                   = "paramName";
        public const string paramValue                  = "paramValue";
        public const string @value                      = "value";
        public const string enabled                     = "enabled";
        public const string callRouteType               = "callRouteType";
        public const string mediaRouteType              = "mediaRouteType";

        // Unconst'ed AppServer consts
        public const string DefaultMediaGroup           = "Default";

        public enum MessageList
        {
            LogIn,
            GetUsers,
            LogOut,
            GetApps,
            GetAppInstances,
            GetNumAppInstances,
            InstallApp,
            InstallLibrary,
            UpdateApp,
            EnableApp,
            DisableApp,
            UninstallApp,
            KillAppInstance,
            ReloadApp,
            GetRequiredVariables,
            GetProviders,
            InstallProvider,
            UninstallProvider,
            DisableProvider,
            EnableProvider,
            ReloadProvider,
            GetConfig,
            UpdateConfig,
            Initialize,     //Unique to test
            ShutdownProcess //Unique to test
        }

        // GUI ELEMENTS
        public const string ABORT_TEST = "Abort";
        public const int unknownTestSuccess = 2;
        public const int previousTestSuccess = 0;
        public const int previousTestFailure = 1;
        public const int group              = 3;
        public const int groupFailed        = 4;
        public const int go                 = 5;

        // Error Reporting
        public const string FULL_EXCEPTION_REPORT = "Full exception is:";
        public const string ASSEMBLY_LOAD_ERROR = "assemblies could not be loaded.";

        // Empty Provider 
        public const string EMPTY_PROVIDER_NAME = "Empty Provider";
        public const string EMPTY_PROVIDER_NS = "Metreos.Providers.EmptyProvider";
        public const string EMPTY_PROVIDER_VERSION = "1.0";
        public const string EMPTY_PROVIDER_DESC = "Meaningless provider used for testing.";

        // Usage
        public const string usage = @"
Usage
---------------------------------------------------------------------------------
-count: [int | optional] The FTConsole will iterate the specified number of times.
       ----- Default = 1 ----- 

-results: [folder | optional] Output folder for results.xml. 
       ----- Default = currentDirectory -----

-dllDir: [folder | optional] Folder to search for compiled functional tests.  
       ----- Default = x:\Build\FunctionalTest\FunctionalTests -----

-maxTestsDir: [folder | optional] Folder to search for MAX sln files.
       ----- Default = x:\Build\FunctionalTest\CompiledTestDir -----

-ip: [ip address | optional] IP Address of machine running Functional Test Provider.
       ----- Default = 127.0.0.1 -----

-port: [port : optional] Port of machine running Functional Test Provider.
       ----- Default = 8120 -----

-testport: [port : optional] Port of machine running Functional Test Runtime.
       ----- Default = 8140 -----
";
	}
}
