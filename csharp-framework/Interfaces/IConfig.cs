using System;

namespace Metreos.Interfaces
{
	public abstract class IConfig
	{
        #region Constants
        public const string APPSERVER_VERSION          = "2.0";

        public abstract class ConfigFileSettings
        {
            public const string FILENAME                = "AppServer.config";
            public const string FW_DIR                  = "FrameworkDirectory";

            // Database
            public const string DB_NAME                 = "DatabaseName";
            public const string DB_HOST                 = "DatabaseHostname";
            public const string DB_PORT                 = "DatabasePort";
            public const string DB_USERNAME             = "DatabaseUsername";
			public const string DB_PASSWORD             = "DatabasePassword";

            public const string SFTP_LISTEN_PORT        = "SftpListenPort";
            public const string SFTP_GRAMMAR_USER       = "GrammarUsername";
            public const string SFTP_MEDIA_USER         = "MediaUsername";

			public const string DEVELOPER_MODE          = "DeveloperMode";

            public const string ROUTER_DISCARD_THRESHOLD    = "RouterDiscardTriggeringEventsQueueThreshold";
            public const string MQ_HIGHWATER_WARN_THRESHOLD = "MessageQueueHighWaterWarningThreshold";

            public abstract class DefaultValues
            {
                public const string DB_NAME             = "MCE";
                public const string DB_HOST             = "localhost";
                public const ushort DB_PORT             = 3306;
                public const string MQ_PROVIDER         = "Metreos.Messaging.MetreosMessageQueueProvider";
				public const string DEVELOPER_MODE      = "False";
                public const int SFTP_LISTEN_PORT       = 22;
            }
        }

        public abstract class AppServerDirectoryNames
        {
            // AppServer
            public const string ROOT            = "AppServer";
            public const string APPS            = "Applications";
            public const string CACHE           = "Cache";
            public const string DEPLOY          = "Deploy";
            public const string LIBS            = "Libs";
            public const string PROVIDERS       = "Providers";
            public const string TM_SCRIPTS      = "TmScripts";

            // FunctionalTest
            public const string TESTS           = "FunctionalTests";
        }

        public abstract class AppDirectoryNames
        {
            // Installed application subdirectory structure
            public const string MEDIA_FILES		= "MediaFiles";
            public const string VOICE_REC_FILES = "VoiceRecFiles";
            public const string EMBEDDED_CODE   = "EmbeddedCode";
            public const string ACTIONS         = "NativeActions";
            public const string TYPES           = "NativeTypes";
            public const string SCRIPTS         = "Scripts";
            public const string LIBS            = "Libs";
            public const string DATABASES       = "Databases";  // For packaging only
        }

        public abstract class FwDirectoryNames
        {
            // Framework subdirectory structure
            public const string CORE        = "CoreAssemblies";
            public const string ACTIONS     = "NativeActions";
            public const string TYPES       = "NativeTypes";
            public const string PACKAGES    = "Packages";
            public const string TOOLS       = "Tools";
        }

        public abstract class ProvDirectoryNames
        {
            // Provider package structure
            public const string Service         = "Service";
            public const string Docs            = "Docs";
            public const string Resources       = "Resources";
            public const string Web             = "Web";
        }

        public abstract class CoreComponentNames
        {
            public const string APP_SERVER          = "ApplicationServer";
            public const string ARE                 = "ApplicationEnvironment";
            public const string ASSEMBLER           = "Assembler";
            public const string ROUTER              = "Router";
            public const string LOGGER              = "Logger";
            public const string PROV_MANAGER        = "ProviderManager";
            public const string APP_MANAGER         = "AppManager";
            public const string MEDIA_MANAGER       = "MediaManager";
            public const string TEL_MANAGER         = "TelephonyManager";
            public const string SMTP_MANAGER        = "SmtpManager";
            public const string SNMP_MANAGER        = "SnmpManager";
            public const string MANAGEMENT          = "Management";
            public const string LOG_SERVER          = "LogServer";
            public const string CLUSTER_INTERFACE   = "ClusterInterface";
            public const string LICENSE_MANAGER     = "LicenseManager";
        }

        public abstract class StandardProviders
        {
            public abstract class MediaControlProvider
            {
                public static string NAME       = "MediaControlProvider";
                public const string NAMESPACE   = "Metreos.MediaControl";
            }

            public abstract class CallControlProvider
            {
                public const string NAMESPACE   = "Metreos.CallControl";
            }
        }

        /// <summary>Set of valid values for the app partition preferred codec field</summary>
        public abstract class PreferredCodec
        {
            public const string g711u_20    = "G.711u_20ms";
            public const string g711u_30    = "G.711u_30ms";
            public const string g711a_20    = "G.711a_20ms";
            public const string g711a_30    = "G.711a_30ms";
            public const string g723_30     = "G.723.1_30ms";
            public const string g723_60     = "G.723.1_60ms";
            public const string g729_20     = "G.729a_20ms";
            public const string g729_30     = "G.729a_30ms";
            public const string g729_40     = "G.729a_40ms";
        }

        public abstract class SystemConfigs
        {
            public const string PARENT_ADDR         = "redundancy_master_ip";
            public const string HB_INTERVAL         = "redundancy_master_heartbeat";
            public const string STANDBY_ADDR        = "redundancy_standby_ip";
            public const string SYNCH_TIMEOUT       = "redundancy_standby_startup_sync_time";
            public const string MAX_MISSED_HBS      = "redundancy_master_max_missed_heartbeat";
            public const string MEDIA_PASSWORD      = "media_provisioning_password";
            public const string DATABASE_VERSION    = "database_version";
            public const string ROLLBACK_VERSION    = "rollback_version";
            public const string OID_ROOT            = "oid_root";
        }

        /// <summary>Reserved config entry names in the MCE config database</summary>
        public abstract class Entries
        {
            public abstract class Names
            {
                public const string LOG_LEVEL           = "MetreosReserved_LogLevel";
                public const string ADDRESS             = "MetreosReserved_Address";    // Media servers
                public const string IP_ADDRESS          = "MetreosReserved_IPAddress";  // H.323
                public const string EMAIL_TO            = "MetreosReserved_EmailRecipient";
                public const string EMAIL_FROM          = "MetreosReserved_EmailSender";
                public const string EMAIL_SERVER        = "MetreosReserved_EmailServer";
                public const string EMAIL_USER          = "MetreosReserved_EmailUsername";
                public const string EMAIL_PASSWORD      = "MetreosReserved_EmailPassword";
                public const string EMAIL_PORT  		= "MetreosReserved_EmailPort";
                public const string ALARM_TRIGGER_LEVEL	= "MetreosReserved_TriggerLevel";
                public const string SNMP_MANAGER		= "MetreosReserved_ManagerIp";
                public const string PRIMARY_CCM_SUB     = "MetreosReserved_PrimarySubscriberId";
                public const string SECONDARY_CCM_SUB   = "MetreosReserved_SecondarySubscriberId";
                public const string TERTIARY_CCM_SUB    = "MetreosReserved_TertiarySubscriberId";
                public const string QUATERNARY_CCM_SUB  = "MetreosReserved_QuaternarySubscriberId";
                public const string SRST_CCM_SUB        = "MetreosReserved_SRST";
                public const string PRIMARY_CTI_ID      = "MetreosReserved_PrimaryCtiManagerId";
                public const string SECONDARY_CTI_ID    = "MetreosReserved_SecondaryCtiManagerId";
                public const string USERNAME            = "MetreosReserved_Username";
                public const string PASSWORD            = "MetreosReserved_Password";
                public const string CONNECTION_TYPE     = "MetreosReserved_ConnectionType";
                public const string PROXY_ID            = "MetreosReserved_OutboundProxyId";
                public const string USE_SSL             = "MetreosReserved_SecureConnection";
                public const string SERVER_NAME         = "ServerName";
                public const string SHUTDOWN_TIMEOUT    = "ShutdownTimeout";
                public const string STARTUP_TIMEOUT     = "StartupTimeout";
                public const string DEFAULT_LOCALE      = "DefaultLocale";
                public const string MAX_THREADS         = "MaxThreads";
                public const string ACTION_TIMEOUT      = "DefaultActionTimeout";
                public const string LOG_LEVEL_DV        = "DebugViewLoggerLevel";
                public const string LOG_LEVEL_CONS      = "ConsoleLoggerLevel";
                public const string LOG_LEVEL_EVENT     = "EventLoggerLevel";
                public const string LOG_LEVEL_FILE      = "FileLoggerLevel";
                public const string LOG_LEVEL_TCP       = "TcpLoggerLevel";
                public const string LOG_LEVEL_SERVER_SINK = "LogServerSinkLevel";
                public const string LOG_FILE_LINES      = "MaxFileLogLines";
                public const string LOG_MAXFILES	    = "MaxFiles";
                public const string LOG_FILE_PATH   	= "FilePath";
                public const string LOG_ENABLE_Q_DIAGS  = "EnableLoggerQueueDiag";
                public const string LOG_TCP_PORT        = "TcpLoggerPort";
                public const string DEBUG_PORT          = "DebugListenPort";
                public const string MGMT_PORT           = "ManagementPort";
                public const string LISTEN_PORT         = "ListenPort";
                public const string DIAG_CALL_TABLE     = "OutputDiagnosticCallTable";
                public const string SANDBOX_ENABLED     = "SandboxEnabled";

                // App configs
                public const string STRING_TABLE        = "StringTable";
                public const string LOCALE_LIST         = "LocaleList";

                // Media server config
                public const string HAS_MEDIA           = "HasMedia";

                public const string LIC_OVERAGE_TABLE   = "LicenseOverageTable";
            }

            public abstract class DisplayNames
            {
                public const string LOG_LEVEL           = "Log Level";
            }

            public abstract class Descriptions
            {
                public const string LOG_LEVEL           = "Filters all debug output below specified level";
            }
        }

        public abstract class LicenseKeys
        {
            public const string SecureBlackBox = "0411B2220FAADE7BCBF1A6E3E1AB4D90135845F61FE2B8FD95F2BCFF5C0806664C08B657ECDE4775835CB08349DC77D7A49F2BE8435C8262A14991F88E06CA1CEC657AF057D1951EEFDE346A8B99D9CF0817939C38919A528E926015C16DE07F76698BCFFC5F0CBF9B6E62C5E37494FB595D18C8EDF16B05F8C196F7186A1062063DA1782E0EFC2C51D349A7BE16BB66C7DDC75325B961C24EFB3FCC4162037DD0029E511D2C0AC72B3BBF45B27C573933DD277A8F37E77CA6967FBD6B9E4A5EB6A1D1430D60F2A010740E1F510D3EBF0859987C42804E1A7B45335454A5CFF2A424F0B6E143544F57A514807B5C46C6F9E665D94C3ECBBD2DB0F49B67A0AFE5";
        }
        #endregion

        #region Enumerations

        public enum ComponentType 
        {
            Unspecified         = 0,
            Core                = 1,
            Application         = 2,
            Provider            = 3,
            MediaServer         = 4,
            LogServer           = 5,
            SMTP_Manager        = 50,   // 50-99: Alarm servers
            SNMP_Manager        = 51,
            SCCP_DevicePool     = 100,  // 100-149: IPT servers
            H323_Gateway        = 101,
            Cisco_SIP_DevicePool= 102,
            CTI_DevicePool      = 103,    // JTAPI phone
            CTI_RoutePoint      = 104,    // JTAPI gateway
            SIP_Trunk           = 105,    // SIP trunk
            IETF_SIP_DevicePool = 106,    // SIP endpoint
            CTI_Monitored       = 107,    // JTAPI 3rd party
            Test                = 149     // Test CC provider
        }

        public enum DeviceType 
        {
            Unspecified         = 0,
            Sccp                = 1,
            CtiPort         	= 2,
            RoutePoint      	= 3,
            CiscoSip            = 4,
            SipTrunk            = 5,
            CtiMonitored        = 6,
            IetfSip             = 100  // Only used inside AppServer (not in DB)
        }

        public enum SipDomainType
        {
            Unspecified         = 0,
            Cisco               = 1,
            IETF                = 2
        }

        public enum SnmpOidType
        { 
            Unspecified         = 0,
            Alarm               = 1,
            Statistic           = 2
        }

        public enum SnmpSyntax
        {
            Unspecified         = 0,
            Integer32           = 1,
            DisplayString       = 2
        }

        public enum Status
        {
            Unspecified     = 0,
            Disabled        = 1,
            Disabled_Error  = 2,
            Enabled_Running = 3,       
            Enabled_Stopped = 4
        }

        public enum FailoverStatus : ushort
        {
            Normal          = 0,
            Failover        = 1,
            Failback        = 2
        }

        public enum StandardFormat
        {
            String          = 1,
            Bool            = 2,
            Number          = 3,
            DateTime        = 4,
            IP_Address      = 5,
            Array           = 6,
            HashTable       = 7,
            DataTable       = 8,
            Password        = 9,
            TraceLevel      = 100,
			TriggerLevel	= 101
        }

        public enum AccessLevel
        {
            Unspecified     = 0,
            Administrator   = 1,    // Modify anything
            Restricted      = 2,    // View only
            Normal          = 3     // Modify only certain component groups
        }

        public enum LogMsgType
        {
            Unspecified     = 0,
            Alarm           = 1,
            Audit           = 2,
            Security        = 3
        }

        public enum Severity
        {
            Unspecified     = 0,
            Red             = 1,
            Yellow          = 2,
            Green           = 3     // Recovered
        }

		public enum EventStatus		// Used by alarm server
		{
			Unspecified		= 0,
			Open			= 1,	// The event is open
			Acknowledged	= 2,	// The event has been reviewed
			Resolved		= 3		// The event has been resolved
		}

        public enum Result
        { 
            ArgumentError,
            Failure,
            Success,
            ServerError,
            NotFound,
            NotAuthorized,
            NotImplemented
        }

        #endregion
	}
}
