using System;

namespace DbTest
{
	public abstract class IConfig
	{
        #region Public Constants
        public const string APPSERVER_VERSION          = "1.1";
        
        public abstract class ConfigFileSettings
        {
            public const string Filename                = "AppServer.config";
            public const string FrameworkDir            = "FrameworkDirectory";
            public const string MessageQueueProvider    = "MessageQueueProvider";

            public abstract class DefaultValues
            {
                public const string MessageQueueProvider = "Metreos.Messaging.MetreosMessageQueueProvider";
            }
        }

        public abstract class AppServerDirectoryNames
        {
            // Samoa
            public const string APPS            = "Applications";
            public const string DATABASES       = "Databases";
            public const string DEPLOY          = "Deploy";
            public const string LIBS            = "Libs";
            public const string LOGS            = "Logs";
            public const string LOG_SINKS       = "LogSinks";
            public const string PROVIDERS       = "Providers";

            // FunctionalTest
            public const string TESTS           = "FunctionalTests";
        }

        public abstract class AppDirectoryNames
        {
            // Installed application subdirectory structure
            public const string DB              = "Databases";
            public const string MEDIA_FILES		= "MediaFiles";
            public const string EMBEDDED_CODE   = "EmbeddedCode";
            public const string ACTIONS         = "NativeActions";
            public const string TYPES           = "NativeTypes";
            public const string SCRIPTS         = "Scripts";
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

        public abstract class CoreComponentNames
        {
            public const string APP_SERVER  = "ApplicationServer";
            public const string ARE         = "ApplicationEnvironment";
            public const string ASSEMBLER   = "Assembler";
            public const string ROUTER      = "Router";
            public const string LOGGER      = "Logger";
            public const string OAM         = "OAM";
            public const string PROV_MANAGER = "ProviderManager";
            public const string APP_MANAGER = "AppManager";
            public const string TASK        = "Task";
            public const string TEST        = "FunctionalTest";
        }
        #endregion

        #region Internal Constants
        internal abstract class Tables
        {
            public const string COMPONENTS          = "mce_components";
            public const string PROV_EXTS           = "mce_provider_extensions";
            public const string PROV_EXT_PARAMS     = "mce_provider_extensions_parameters";
            public const string METADATA            = "mce_meta_data";
            public const string EVENT_LOG           = "mce_event_log";
            public const string APP_SCRIPTS         = "mce_application_scripts";
            public const string LICENSE             = "mce_license";
            public const string COMPONENT_GROUPS    = "mce_component_groups";
            public const string APP_PARTITIONS      = "mce_application_partitions";
            public const string FORMAT_TYPES        = "mce_format_types";
            public const string CONFIG_ENTRY_METAS  = "mce_config_entry_metas";
            public const string CONFIG_ENTRIES      = "mce_config_entries";
            public const string CONFIG_VALUES       = "mce_config_values";
            public const string USERS               = "mce_users";
            public const string USERS_ACL_LIST      = "mce_users_acl_list";
            public const string COMPONENT_GRP_MEMBERS   = "mce_component_group_members";
            public const string FORMAT_TYPE_ENUM_VALS   = "mce_format_type_enum_values";
            public const string APP_SCRIPT_TRIG_PARAMS  = "mce_application_script_trigger_parameters";
        }

        internal abstract class Keys
        {
            public const string COMPONENTS          = "mce_components_id";
            public const string PROV_EXTS           = "mce_provider_extensions_id";
            public const string PROV_EXT_PARAMS     = "mce_provider_extensions_parameters_id";
            public const string METADATA            = "mce_meta_data_id";
            public const string EVENT_LOG           = "mce_event_log_id";
            public const string APP_SCRIPTS         = "mce_application_scripts_id";
            public const string LICENSE             = "mce_license_id";
            public const string COMPONENT_GROUPS    = "mce_component_groups_id";
            public const string APP_PARTITIONS      = "mce_application_partitions_id";
            public const string FORMAT_TYPES        = "mce_format_types_id";
            public const string CONFIG_ENTRY_METAS  = "mce_config_entry_metas_id";
            public const string CONFIG_ENTRIES      = "mce_config_entries_id";
            public const string CONFIG_VALUES       = "mce_config_values_id";
            public const string USERS               = "mce_users_id";
            public const string FORMAT_TYPE_ENUM_VALS   = "mce_format_type_enum_values_id";
            public const string APP_SCRIPT_TRIG_PARAMS  = "mce_application_script_trigger_parameters_id";
        }

        internal abstract class Fields
        {
            public const string NAME                = "name";
            public const string DISPLAY_NAME        = "display_name";
            public const string TYPE                = "type";
            public const string STATUS              = "status";
            public const string VERSION             = "version";
            public const string DESCRIPTION         = "description";
            public const string WAIT_COMPLETION     = "wait_for_completion";
            public const string AUTHOR              = "author";
            public const string COPYRIGHT           = "copyright";
            public const string AUTHOR_URL          = "author_url";
            public const string SUPPORT_URL         = "support_url";
            public const string MESSAGE_ID          = "message_id";
            public const string CREATED_TS          = "created_timestamp";
            public const string UPDATED_TS          = "updated_timestamp";
            public const string USES_CALL_CONTROL   = "uses_call_control";
            public const string USES_MEDIA          = "uses_media_control";
            public const string LICENSE_DATA        = "license_data";
            public const string ACTIVE              = "active";
            public const string EVENT_TYPE          = "event_type";
            public const string ENABLED             = "enabled";
            public const string ORDINAL             = "ordinal";
            public const string KEY                 = "key";
            public const string VALUE               = "value";
            public const string FAILOVER_GROUP_ID   = "mce_failover_group_id";
            public const string ALARM_GROUP_ID      = "mce_alarm_group_id";
            public const string MIN_VALUE           = "min_value";
            public const string MAX_VALUE           = "max_value";
            public const string READ_ONLY           = "read_only";
            public const string COMPONENT_TYPE      = "component_type";
            public const string USERNAME            = "username";
            public const string PASSWORD            = "password";
            public const string CREATOR_ID          = "creator_mce_users_id";
            public const string ACCESS_LEVEL        = "access_level";
            public const string CALL_GROUP_ID       = "mce_call_route_group_id";
            public const string MEDIA_GROUP_ID      = "mce_media_resource_group_id";
        }
        #endregion

        #region Data Structures
        [Serializable]
        public class ConfigValue
        {
            public string ID;
            public string name;
            public object Value;
            public string description;
            public string format;
            public string enumValues;
            public int minValue = 0;
            public int maxValue = 0;
            public bool diagnostic = false;
            public bool requiresRestart = true;
            public bool readOnly = false;

            public ConfigValue() {}

            public ConfigValue(string name, object Value, string description, string format, bool requiresRestart)
                : this(name, Value, description, 0, 0, format, false, requiresRestart, false) {}

            public ConfigValue(string name, object Value, string description, int minValue,
                int maxValue, string format, bool diagnostic, bool requiresRestart,
                bool readOnly)
            {
                this.name = name;
                this.Value = Value;
                this.description = description;
                this.minValue = minValue;
                this.maxValue = maxValue;
                this.diagnostic = diagnostic;
                this.requiresRestart = requiresRestart;
                this.readOnly = readOnly;
                this.format = format;
            }

            public string Name  
            {
                get  { return name; }  set  { name = value; } 
            }
            public object _Value  
            {
                get  { return Value; }	set  { Value = value; } 
            }
            public string Description
            {
                get  { return description; }  set  { description = value; }
            }
            public int MinValue  
            {
                get  { return minValue; }  set  { minValue = value; } 
            }		
            public int MaxValue  
            {
                get  { return maxValue; }  set  { maxValue = value; } 
            }		
            public string Format  
            {
                get  { return format; }  set  { format = value; }
            }						
            public bool Diagnostic  
            {
                get  { return diagnostic; }	 set  { diagnostic = value; } 
            }		
            public bool RequiresRestart  
            {
                get  { return requiresRestart; }  set  { requiresRestart = value; } 
            }			
            public bool ReadOnly  
            {
                get  { return readOnly; }  set  { readOnly = value; } 
            }			
        }

        [Serializable]
        public class ComponentInfo
        {
            public string ID;
            public string name;
            public ComponentType componentType;
            public string description;
            public Status status;
            public string version;
            public string path;

            public double Version  
            {
                get
                { 
                    try { return double.Parse(version); }
                    catch(Exception) { return 0; }
                }
                set { version = value.ToString(); }
            }

            public Uri Address
            {
                get
                {
                    try { return new Uri(path); }
                    catch(Exception) { return null; }
                }
                set { path = value.ToString(); }
            }
        }

        [Serializable]
        public class Statistic
        {
            public string ID;
            public string name;
            public int Value;
            public string description;
        }

        [Serializable]
        public class Extension
        {
            public string ID;
            public string name;
            public string description;

            // Array of ExtensionParam objects
            public ArrayList parameters;

            // Some properties
            public string Name  
            {
                get  { return name; }  set  { name = value; } 
            }

            public string Description
            {
                get  { return description; }  set  { description = value; } 
            }

            // Constructors
            public Extension()
                : this(null, null) {}

            public Extension(string name) 
                : this(name, null) {}

            public Extension(string name, string description)
            {
                this.name = name;
                this.description = description;

                parameters = new ArrayList();
            }
        }

        [Serializable]
        public class ExtensionParam
        {
            public string ID;
            public string name;
            public string type;
            public string description;
            public string Value;

            public ExtensionParam() 
                :this (null, null, null, null) {}

            public ExtensionParam(string name, string type, string description)
                : this(name, type, description, null) {}

            public ExtensionParam(string name, string type, string description, string Value)
            {
                this.name = name;
                this.type = type;
                this.description = description;
                this.Value = Value;
            }
        }

        [Serializable]
        public class UserInfo
        {
            public string ID;
            public string name;
            public string password;
            public AccessLevel accessLevel = AccessLevel.Normal;
            public bool connected = false;
            public string createdByUsersID;
            public DateTime dateCreated;
            public DateTime dateLastUpdated;
        }
        #endregion

        #region Enumerations

        // Note that the 'Unspecified' values are present only for coding reasons
        //   they should never be inserted into the database.

        public enum ComponentType 
        {
            Unspecified     = -1,
            Core            = 0,
            Application     = 1,
            Provider        = 2,
            MediaServer     = 3,
            IPT_Server      = 4,
            SMTP_Manager    = 5,
            SNMP_Manager    = 6
        }

        public enum Status
        {
            Unspecified     = -1,
            Disabled        = 0,
            Disabled_Error  = 1,
            Enabled_Running = 2,       
            Enabled_Stopped = 3
        }

        public enum AccessLevel
        {
            Unspecified     = -1,
            Administrator   = 0,  // Modify anything
            Restricted      = 1,  // View only
            Normal          = 2   // Modify only certain component groups
        }

        public enum LogMsgType
        {
            Unspecified     = -1,
            Alarm           = 0,
            Audit           = 1,
            Security        = 2,
        }

        public enum TriggerParamsType
        {
            Unspecified     = -1,
            Literal         = 0,
            Regex           = 1
        }

        #endregion
	}
}
