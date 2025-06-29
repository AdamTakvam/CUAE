using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Diagnostics;

using Metreos.Interfaces;

namespace Metreos.Utilities
{
    /// <summary>
    /// This class privides a primitive database abstraction layer 
    /// specifically for the MCE config database. Aside from the 
    /// table, key and field definitions, each of the classes 
    /// represent a single table and contain methods for relevant 
    /// SQL operations which can be performed on that table.
    /// </summary>
	public abstract class Database
	{
        #region Database Constants

        /// <summary>Table names used in the MCE config database</summary>
        public abstract class Tables
        {
            public const string COMPONENTS          = "mce_components";
            public const string PROV_EXTS           = "mce_provider_extensions";
            public const string PROV_EXT_PARAMS     = "mce_provider_extensions_parameters";
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
            public const string TRIG_PARAM_VALUES   = "mce_trigger_parameter_values";
            public const string CM_DEVICES          = "mce_call_manager_devices";
            public const string CM_CLUSTERS         = "mce_call_manager_clusters";
            public const string CM_CLUSTER_SUBS     = "mce_call_manager_cluster_subscribers";
            public const string CM_CLUSTER_MEMBERS  = "mce_call_manager_cluster_members";
            public const string CM_CTI_MANAGERS     = "mce_call_manager_cluster_cti_managers";
            public const string SIP_DOMAINS         = "mce_sip_domains";
            public const string SIP_DEVICES         = "mce_ietf_sip_devices";
            public const string SIP_DOMAIN_MEMBERS  = "mce_sip_domain_members";
            public const string SIP_PROXIES         = "mce_sip_outbound_proxies";
            public const string SERVICES			= "mce_services";
            public const string SNMP_MIB_DEFS       = "mce_snmp_mib_defs";
            public const string SYSTEM_CONFIGS      = "mce_system_configs";
            public const string COMPONENT_GRP_MEMBERS   = "mce_component_group_members";
            public const string FORMAT_TYPE_ENUM_VALS   = "mce_format_type_enum_values";
            public const string APP_SCRIPT_TRIG_PARAMS  = "mce_application_script_trigger_parameters";
        }

        /// <summary>Primary and foreign key names used in the MCE config database</summary>
        public abstract class Keys
        {
            public const string COMPONENTS          = "mce_components_id";
            public const string PROV_EXTS           = "mce_provider_extensions_id";
            public const string PROV_EXT_PARAMS     = "mce_provider_extensions_parameters_id";
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
            public const string TRIG_PARAM_VALUES   = "mce_trigger_parameter_values_id";
            public const string CM_DEVICES          = "mce_call_manager_devices_id";
            public const string CM_CLUSTERS         = "mce_call_manager_clusters_id";
            public const string CM_CLUSTER_SUBS     = "mce_call_manager_cluster_subscribers_id";
            public const string CM_CTI_MANAGERS     = "mce_call_manager_cluster_cti_managers_id";
            public const string SIP_DOMAINS         = "mce_sip_domains_id";
            public const string SIP_DEVICES         = "mce_ietf_sip_devices_id";
            public const string SIP_PROXIES         = "mce_sip_outbound_proxies_id";
            public const string SERVICES			= "mce_services_id";
            public const string SNMP_MIB_DEFS       = "mce_snmp_mib_defs_id";
            public const string FORMAT_TYPE_ENUM_VALS   = "mce_format_type_enum_values_id";
            public const string APP_SCRIPT_TRIG_PARAMS  = "mce_application_script_trigger_parameters_id";
        }

        /// <summary>Field names used in the MCE config database</summary>
        public abstract class Fields
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
            public const string MESSAGE             = "message";
            public const string DATA                = "data";
            public const string SEVERITY            = "severity";
            public const string CREATED_TS          = "created_timestamp";
            public const string UPDATED_TS          = "updated_timestamp";
            public const string RECOVERED_TS        = "recovered_timestamp";
            public const string USES_CALL_CONTROL   = "uses_call_control";
            public const string USES_MEDIA          = "uses_media_control";
            public const string LICENSE_DATA        = "license_data";
            public const string ACTIVE              = "active";
            public const string EVENT_TYPE          = "event_type";
            public const string ENABLED             = "enabled";
			public const string ORDINAL				= "ordinal";
            public const string ORDINAL_ROW         = "ordinal_row";
            public const string KEY_COLUMN          = "key_column";
            public const string VALUE               = "value";
            public const string MIN_VALUE           = "min_value";
            public const string MAX_VALUE           = "max_value";
            public const string READ_ONLY           = "read_only";
            public const string REQUIRED            = "required";
            public const string COMPONENT_TYPE      = "component_type";
            public const string USERNAME            = "username";
            public const string PASSWORD            = "password";
            public const string ACCESS_LEVEL        = "access_level";
            public const string IP_ADDRESS          = "ip_address";
            public const string PUBLISHER_IP        = "publisher_ip_address";
            public const string PUBLISHER_USERNAME  = "publisher_username";
            public const string PUBLISHER_PASSWORD  = "publisher_password";
            public const string SNMP_COMMUNITY      = "snmp_community";
            public const string DEVICE_NAME         = "device_name";
            public const string DEVICE_TYPE         = "device_type";
            public const string DIR_NUMBER          = "directory_number";
			public const string USER_STOPPED		= "user_stopped";
            public const string PREF_CODEC          = "preferred_codec";
            public const string EARLY_MEDIA         = "use_early_media";
            public const string LOCALE              = "locale";
            public const string DOMAIN_NAME         = "domain_name";
            public const string REGISTRAR           = "primary_registrar";
            public const string REGISTRAR2          = "secondary_registrar";
            public const string OID                 = "oid";
            public const string DATA_TYPE           = "data_type";
            public const string IGNORE              = "ignore";
            public const string APPSERVER_USE       = "app_server_use";
            
            // Special foreign keys
            public const string FAILOVER_GROUP_ID   = "mce_failover_group_id";
            public const string ALARM_GROUP_ID      = "mce_alarm_group_id";
            public const string CREATOR_ID          = "creator_mce_users_id";
            public const string CALL_GROUP_ID       = "mce_call_route_group_id";
            public const string MEDIA_GROUP_ID      = "mce_media_resource_group_id";

            // Stored procedures
            public const string LAST_INSERT_ID      = "LAST_INSERT_ID()";
            public const string MYSQL_NOW           = "NOW()";
        }

        /// <summary>Static key values of the predefined component groups</summary>
        public abstract class ComponentGroupIds
        {
            public const uint CORE                  = 1;
            public const uint APPLICATIONS          = 2;
            public const uint PROVIDERS             = 3;
            public const uint MEDIA_SERVERS         = 4;
            public const uint IPT_SERVERS           = 5;
            public const uint ALARMS                = 6;
        }

        /// <summary>Miscellaneous default definitions</summary>
        public abstract class DefaultValues
        {
            public const string PARTITION_NAME      = "Default";
            public const string PARTITION_DESC      = "Automatically generated partition";
            public const string PREF_CODEC          = IConfig.PreferredCodec.g711u_20;
            public const uint MEDIA_RES_GROUP_ID    = 4;
            public const uint ALARM_GROUP_ID        = 5;
            public const uint SCCP_GROUP_ID         = 6;
            public const uint H323_GROUP_ID         = 7;
            public const uint SIP_GROUP_ID          = 8;
            public const uint CTI_GROUP_ID          = 9;
            public const uint TEST_GROUP_ID         = 10;
        }
        #endregion

        #region Low-Level Database Access Methods

        #region Components
        public abstract class Components
        {
            public static uint Insert(IDbConnection db, string name, string displayName, IConfig.ComponentType type, 
                IConfig.Status status, string author, string copyright, string authorUrl,
                string supportUrl, string description, string version)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if(name == null) { return 0; }
                if(type == IConfig.ComponentType.Unspecified) { return 0; }
                if(status == IConfig.Status.Unspecified) { return 0; }

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.COMPONENTS);
                sql.AddFieldValue(Fields.NAME, name);
                sql.AddFieldValue(Fields.TYPE, type);
                sql.AddFieldValue(Fields.STATUS, status);
                sql.AddFieldValue(Fields.CREATED_TS, new SqlBuilder.PreformattedValue(Fields.MYSQL_NOW));
                sql.AddFieldValue(Fields.VERSION, version != null ? version : "1.0");
                if(displayName != null) sql.AddFieldValue(Fields.DISPLAY_NAME, displayName);
                if(author != null)      sql.AddFieldValue(Fields.AUTHOR, author);
                if(copyright != null)   sql.AddFieldValue(Fields.COPYRIGHT, copyright);
                if(authorUrl != null)   sql.AddFieldValue(Fields.AUTHOR_URL, authorUrl);
                if(supportUrl != null)  sql.AddFieldValue(Fields.SUPPORT_URL, supportUrl);
                if(description != null) sql.AddFieldValue(Fields.DESCRIPTION, description);

                Utilities.ExecuteCommand(db, sql.ToString());

                return Utilities.GetLastInsertId(db, Tables.COMPONENTS, Keys.COMPONENTS);
            }

            public static DataTable Select(IDbConnection db, uint id)
            {
                return Select(db, id, null, IConfig.ComponentType.Unspecified);
            }

            public static DataTable Select(IDbConnection db, string name, IConfig.ComponentType type)
            {
                return Select(db, 0, name, type);
            }

            public static DataTable Select(IDbConnection db, uint id, string name, IConfig.ComponentType type)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.COMPONENTS);
                if(id != 0)                                     sql.where.Add(Keys.COMPONENTS, id);
                if(name != null)                                sql.where.Add(Fields.NAME, name);
                if(type != IConfig.ComponentType.Unspecified)   sql.where.Add(Fields.TYPE, type);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }

            public static int Update(IDbConnection db, uint id, string displayName, string version,
                string description, string author, string copyright, IConfig.Status status)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, Tables.COMPONENTS);
                if(displayName != null)     sql.AddFieldValue(Fields.DISPLAY_NAME, displayName);
                if(version != null)         sql.AddFieldValue(Fields.VERSION, version);
                if(description != null)     sql.AddFieldValue(Fields.DESCRIPTION, description);
                if(author != null)          sql.AddFieldValue(Fields.AUTHOR, author);
                if(copyright != null)       sql.AddFieldValue(Fields.COPYRIGHT, copyright);
                if(status != IConfig.Status.Unspecified)    
                    sql.AddFieldValue(Fields.STATUS, status);

                sql.where.Add(Keys.COMPONENTS, id);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }

            public static int Delete(IDbConnection db, uint id)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, Tables.COMPONENTS);
                sql.where.Add(Keys.COMPONENTS, id);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }
        }
        #endregion

        #region ComponentGroups
        public abstract class ComponentGroups
        {
            public static uint Insert(IDbConnection db, uint alarmGroupId, uint failoverGroupId, uint componentType,
                string name, string description)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.COMPONENT_GROUPS);
                if(failoverGroupId != 0)    sql.AddFieldValue(Fields.FAILOVER_GROUP_ID, failoverGroupId);
                if(alarmGroupId != 0)       sql.AddFieldValue(Fields.ALARM_GROUP_ID, alarmGroupId);
                if(componentType != 0)      sql.AddFieldValue(Fields.COMPONENT_TYPE, componentType);
                if(description != null)     sql.AddFieldValue(Fields.DESCRIPTION, description);
                if(name != null)            sql.AddFieldValue(Fields.NAME, name);

                Utilities.ExecuteCommand(db, sql.ToString());

                return Utilities.GetLastInsertId(db, Tables.COMPONENT_GROUPS, Keys.COMPONENT_GROUPS);
            }

            public static DataTable Select(IDbConnection db, uint id, string name)
            {
                return Select(db, id, name, IConfig.ComponentType.Unspecified);
            }

            public static DataTable Select(IDbConnection db, uint id, string name, IConfig.ComponentType componentType)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.COMPONENT_GROUPS);
                if(id != 0)         sql.where.Add(Keys.COMPONENT_GROUPS, id);
                if(name != null)    sql.where.Add(Fields.NAME, name);
                if(componentType != IConfig.ComponentType.Unspecified)
                                    sql.where.Add(Fields.COMPONENT_TYPE, componentType);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }

            public static int Update(IDbConnection db, uint id, uint failoverGroupId, uint alarmGroupId, 
                string name, string description)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, Tables.COMPONENT_GROUPS);
                if(failoverGroupId != 0)    sql.AddFieldValue(Fields.FAILOVER_GROUP_ID, failoverGroupId);
                if(alarmGroupId != 0)       sql.AddFieldValue(Fields.ALARM_GROUP_ID, alarmGroupId);
                if(name != null)            sql.AddFieldValue(Fields.NAME, name);
                if(description != null)     sql.AddFieldValue(Fields.DESCRIPTION, description);

                sql.where.Add(Keys.COMPONENT_GROUPS, id);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }

            public static int Delete(IDbConnection db, uint id)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, Tables.COMPONENT_GROUPS);
                sql.where.Add(Keys.COMPONENT_GROUPS, id);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }
        }
        #endregion

        #region ComponentGroupMembers
        public abstract class ComponentGroupMembers
        {
            public static void Insert(IDbConnection db, uint componentsId, uint componentGroupsId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.COMPONENT_GRP_MEMBERS);
                sql.AddFieldValue(Keys.COMPONENTS, componentsId);
                sql.AddFieldValue(Keys.COMPONENT_GROUPS, componentGroupsId);

                Utilities.ExecuteCommand(db, sql.ToString());
            }

            public static DataTable Select(IDbConnection db, uint componentsId, uint componentGroupsId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.COMPONENT_GRP_MEMBERS);
                if(componentsId != 0)      sql.where.Add(Keys.COMPONENTS, componentsId);
                if(componentGroupsId != 0) sql.where.Add(Keys.COMPONENT_GROUPS, componentGroupsId);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }

            public static int Delete(IDbConnection db, uint componentsId, uint componentGroupsId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, Tables.COMPONENT_GRP_MEMBERS);
                if(componentsId != 0)      sql.where.Add(Keys.COMPONENTS, componentsId);
                if(componentGroupsId != 0) sql.where.Add(Keys.COMPONENT_GROUPS, componentGroupsId);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }
        }
        #endregion

        #region ConfigEntries
        public abstract class ConfigEntries
        {
            public static uint Insert(IDbConnection db, uint configEntriesMetasId, uint componentsId, 
                uint appPartitionsId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if((componentsId == 0) && (appPartitionsId == 0)) { return 0; }

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.CONFIG_ENTRIES);
                sql.AddFieldValue(Keys.CONFIG_ENTRY_METAS, configEntriesMetasId);
                if(componentsId != 0)      sql.AddFieldValue(Keys.COMPONENTS, componentsId);
                if(appPartitionsId != 0)   sql.AddFieldValue(Keys.APP_PARTITIONS, appPartitionsId);

                Utilities.ExecuteCommand(db, sql.ToString());

                return Utilities.GetLastInsertId(db, Tables.CONFIG_ENTRIES, Keys.CONFIG_ENTRIES);
            }

            public static DataTable Select(IDbConnection db, uint componentsId, uint appPartitionsId)
            {
                return Select(db, componentsId, appPartitionsId, 0);
            }

            public static DataTable Select(IDbConnection db, uint componentsId, uint appPartitionsId,
				uint configEntriesMetasId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.CONFIG_ENTRIES);
                if(componentsId != 0)           sql.where.Add(Keys.COMPONENTS, componentsId);
                if(appPartitionsId != 0)        sql.where.Add(Keys.APP_PARTITIONS, appPartitionsId);
                if(configEntriesMetasId != 0)   sql.where.Add(Keys.CONFIG_ENTRY_METAS, configEntriesMetasId);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }

			/// <summary>
			/// Tres turbo select with join to get the named config variables.
			/// </summary>
			/// <param name="db"></param>
			/// <param name="componentType"></param>
			/// <param name="componentName"></param>
			/// <param name="partitionName"></param>
			/// <returns></returns>
			public static DataTable Select( IDbConnection db, IConfig.ComponentType componentType,
				string componentName, string partitionName )
			{
				String s = String.Format(
					"SELECT x.mce_config_entries_id, x.mce_config_entry_metas_id,"+
					" y.name, x.mce_components_id, x.mce_application_partitions_id"+
					" FROM mce_config_entries as x inner join mce_config_entry_metas as y"+
					" on x.mce_config_entry_metas_id = y.mce_config_entry_metas_id"+
					" WHERE x.mce_application_partitions_id=(SELECT mce_application_partitions_id"+
					" FROM mce_application_partitions WHERE mce_components_id=(SELECT mce_components_id"+
					" FROM mce_components WHERE type={0} AND name='{1}') AND name='{2}')"+
					" union"+
					" SELECT x.mce_config_entries_id, x.mce_config_entry_metas_id,"+
					" y.name, x.mce_components_id, x.mce_application_partitions_id"+
					" FROM mce_config_entries as x inner join mce_config_entry_metas as y"+
					" on x.mce_config_entry_metas_id = y.mce_config_entry_metas_id"+
					" WHERE x.mce_components_id=(SELECT mce_components_id FROM mce_components"+
					" WHERE type={0} AND name='{1}')",
					(uint) componentType, componentName, partitionName );

				//Console.WriteLine( "big query = {0}", s );

				return Utilities.ExecuteQuery(db, s);
			}

            public static int Delete(IDbConnection db, uint id)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, Tables.CONFIG_ENTRIES);
                sql.where.Add(Keys.CONFIG_ENTRIES, id);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }
        }
        #endregion

        #region ConfigValues
        public abstract class ConfigValues
        {
            public static int Insert(IDbConnection db, uint configEntriesId, string Value, int ordinal, string key)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if(Value == null) { return 0; }

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.CONFIG_VALUES);
                sql.AddFieldValue(Keys.CONFIG_ENTRIES, configEntriesId);
                sql.AddFieldValue(Fields.VALUE, Value);
                if(ordinal != -1)   sql.AddFieldValue(Fields.ORDINAL_ROW, ordinal);
                if(key != null)     sql.AddFieldValue(Fields.KEY_COLUMN, key);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }

            public static DataTable Select(IDbConnection db, uint configEntriesId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.CONFIG_VALUES);
                if(configEntriesId != 0)   sql.where.Add(Keys.CONFIG_ENTRIES, configEntriesId);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }

            public static int Update(IDbConnection db, uint id, string Value, uint ordinal, string key)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, Tables.CONFIG_VALUES);
                if(ordinal != 0)   sql.AddFieldValue(Fields.ORDINAL_ROW, ordinal);
                if(key != null)     sql.AddFieldValue(Fields.KEY_COLUMN, key);
                if(Value != null)     sql.AddFieldValue(Fields.VALUE, Value);

                sql.where.Add(Keys.CONFIG_VALUES, id);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }

            public static int Delete(IDbConnection db, uint configEntriesId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, Tables.CONFIG_VALUES);
                sql.where.Add(Keys.CONFIG_ENTRIES, configEntriesId);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }
        }
        #endregion

        #region ConfigEntryMetas
        public abstract class ConfigEntryMetas
        {
            public static uint Insert(IDbConnection db, bool readOnly, bool required, uint formatTypesId, 
                string name, string displayName, string description, double minValue, bool minValueSpecified, 
                double maxValue, bool maxValueSpecified)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if(name == null) { return 0; }

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.CONFIG_ENTRY_METAS);
                sql.AddFieldValue(Fields.REQUIRED, required);
                sql.AddFieldValue(Fields.READ_ONLY, readOnly);
                sql.AddFieldValue(Keys.FORMAT_TYPES, formatTypesId);
                sql.AddFieldValue(Fields.NAME, name);
                if(displayName != null) sql.AddFieldValue(Fields.DISPLAY_NAME, displayName);
                if(description != null) sql.AddFieldValue(Fields.DESCRIPTION, description);
                if(minValueSpecified)   sql.AddFieldValue(Fields.MIN_VALUE, minValue);
                if(maxValueSpecified)   sql.AddFieldValue(Fields.MAX_VALUE, maxValue);

                Utilities.ExecuteCommand(db, sql.ToString());

                return Utilities.GetLastInsertId(db, Tables.CONFIG_ENTRY_METAS, Keys.CONFIG_ENTRY_METAS);
            }

            public static uint Insert(IDbConnection db, bool readOnly, bool required, uint formatTypesId, 
                string name, string displayName, string description, int minValue, bool minValueSpecified, 
                int maxValue, bool maxValueSpecified)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if(name == null) { return 0; }

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.CONFIG_ENTRY_METAS);
                sql.AddFieldValue(Fields.REQUIRED, required);
                sql.AddFieldValue(Fields.READ_ONLY, readOnly);
                sql.AddFieldValue(Keys.FORMAT_TYPES, formatTypesId);
                sql.AddFieldValue(Fields.NAME, name);
                if(displayName != null) sql.AddFieldValue(Fields.DISPLAY_NAME, displayName);
                if(description != null) sql.AddFieldValue(Fields.DESCRIPTION, description);
                if(minValueSpecified)   sql.AddFieldValue(Fields.MIN_VALUE, minValue);
                if(maxValueSpecified)   sql.AddFieldValue(Fields.MAX_VALUE, maxValue);

                Utilities.ExecuteCommand(db, sql.ToString());

                return Utilities.GetLastInsertId(db, Tables.CONFIG_ENTRY_METAS, Keys.CONFIG_ENTRY_METAS);
            }

            public static DataTable Select(IDbConnection db, uint id, uint formatTypesId, 
                IConfig.ComponentType componentType, string name)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.CONFIG_ENTRY_METAS);
                if(id != 0)            sql.where.Add(Keys.CONFIG_ENTRY_METAS, id);
                if(formatTypesId != 0) sql.where.Add(Keys.FORMAT_TYPES, formatTypesId);
                if(name != null)       sql.where.Add(Fields.NAME, name);
                if(componentType != IConfig.ComponentType.Unspecified) 
                    sql.where.Add(Fields.COMPONENT_TYPE, componentType);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }

            public static int Update(IDbConnection db, uint id, uint formatTypesId, string name, 
                uint componentType, string displayName, string description, int minValue, bool minValueSpecified, 
                int maxValue, bool maxValueSpecified, bool readOnly, bool readOnlySpecified, bool required,
                bool requiredSpecified)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, Tables.CONFIG_ENTRY_METAS);
                if(requiredSpecified)   sql.AddFieldValue(Fields.REQUIRED, required);
                if(readOnlySpecified)   sql.AddFieldValue(Fields.READ_ONLY, readOnly);
                if(formatTypesId != 0)  sql.AddFieldValue(Keys.FORMAT_TYPES, formatTypesId);
                if(name != null)        sql.AddFieldValue(Fields.NAME, name);
                if(componentType != 0)  sql.AddFieldValue(Fields.COMPONENT_TYPE, componentType);
                if(displayName != null) sql.AddFieldValue(Fields.DISPLAY_NAME, displayName);
                if(description != null) sql.AddFieldValue(Fields.DESCRIPTION, description);
                if(minValueSpecified)   sql.AddFieldValue(Fields.MIN_VALUE, minValue);
                if(maxValueSpecified)   sql.AddFieldValue(Fields.MAX_VALUE, maxValue);

                sql.where.Add(Keys.CONFIG_ENTRY_METAS, id);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }

            public static int Delete(IDbConnection db, uint id)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                // Can't remove standard metas
                if(id < 100)
                    return 0;

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, Tables.CONFIG_ENTRY_METAS);
                sql.where.Add(Keys.CONFIG_ENTRY_METAS, id);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }
        }
        #endregion

        #region FormatTypes
        public abstract class FormatTypes
        {
            public static uint Insert(IDbConnection db, string name, string description, uint componentsId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if(name == null) { return 0; }

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.FORMAT_TYPES);
                sql.AddFieldValue(Fields.NAME, name);
                if(componentsId != 0)  sql.AddFieldValue(Keys.COMPONENTS, componentsId);
                if(description != null) sql.AddFieldValue(Fields.DESCRIPTION, description);

                Utilities.ExecuteCommand(db, sql.ToString());

                return Utilities.GetLastInsertId(db, Tables.FORMAT_TYPES, Keys.FORMAT_TYPES);
            }

            public static DataTable Select(IDbConnection db, uint id, uint componentId, string name)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.FORMAT_TYPES);
                if(id != 0)            sql.where.Add(Keys.FORMAT_TYPES, id);
                if(componentId != 0)   sql.where.Add(Keys.COMPONENTS, componentId);
                if(name != null)        sql.where.Add(Fields.NAME, name);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }

            public static int Delete(IDbConnection db, uint id, uint componentsId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, Tables.FORMAT_TYPES);
                if(id != 0)             sql.where.Add(Keys.FORMAT_TYPES, id);
                if(componentsId != 0)   sql.where.Add(Keys.COMPONENTS, componentsId);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }
        }
        #endregion

        #region FormatTypeEnumValues
        public abstract class FormatTypeEnumValues
        {
            public static uint Insert(IDbConnection db, uint formatTypesId, string Value)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if(Value == null) { return 0; }

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.FORMAT_TYPE_ENUM_VALS);
                sql.AddFieldValue(Keys.FORMAT_TYPES, formatTypesId);
                sql.AddFieldValue(Fields.VALUE, Value);

                Utilities.ExecuteCommand(db, sql.ToString());

                return Utilities.GetLastInsertId(db, Tables.FORMAT_TYPE_ENUM_VALS, Keys.FORMAT_TYPE_ENUM_VALS);
            }

            public static DataTable Select(IDbConnection db, uint formatTypesId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.FORMAT_TYPE_ENUM_VALS);
                if(formatTypesId != 0) sql.where.Add(Keys.FORMAT_TYPES, formatTypesId);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }

            public static int Delete(IDbConnection db, uint id, uint formatTypesId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, Tables.FORMAT_TYPE_ENUM_VALS);
                if(id != 0)             sql.where.Add(Keys.FORMAT_TYPE_ENUM_VALS, id);
                if(formatTypesId != 0)  sql.where.Add(Keys.FORMAT_TYPES, formatTypesId);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }
        }
        #endregion

        #region AppScripts
        public abstract class AppScripts
        {
            public static uint Insert(IDbConnection db, uint componentsId, string name, string eventType, 
                bool usesCC, bool usesMedia)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if(name == null) { return 0; }
                if(eventType == null) { return 0; }

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.APP_SCRIPTS);
                sql.AddFieldValue(Keys.COMPONENTS, componentsId);
                sql.AddFieldValue(Fields.NAME, name);
                sql.AddFieldValue(Fields.EVENT_TYPE, eventType);
                sql.AddFieldValue(Fields.USES_CALL_CONTROL, usesCC);
                sql.AddFieldValue(Fields.USES_MEDIA, usesMedia);

                Utilities.ExecuteCommand(db, sql.ToString());

                return Utilities.GetLastInsertId(db, Tables.APP_SCRIPTS, Keys.APP_SCRIPTS);
            }

            public static DataTable Select(IDbConnection db, uint componentsId, string name)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.APP_SCRIPTS);
                if(componentsId != 0)  sql.where.Add(Keys.COMPONENTS, componentsId);
                if(name != null)        sql.where.Add(Fields.NAME, name);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }

            public static int Delete(IDbConnection db, uint id, uint componentsId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, Tables.APP_SCRIPTS);
                if(id != 0)             sql.where.Add(Keys.APP_SCRIPTS, id);
                if(componentsId != 0)   sql.where.Add(Keys.COMPONENTS, componentsId);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }
        }
        #endregion

        #region AppScriptTriggerParams
        public abstract class AppScriptTriggerParams
        {
            public static uint Insert(IDbConnection db, string name, uint appScriptsId, uint appPartitionsId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if(name == null) { return 0; }
                if((appScriptsId == 0) || (appPartitionsId == 0)) { return 0; }

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.APP_SCRIPT_TRIG_PARAMS);
                sql.AddFieldValue(Fields.NAME, name);
                sql.AddFieldValue(Keys.APP_SCRIPTS, appScriptsId);
                sql.AddFieldValue(Keys.APP_PARTITIONS, appPartitionsId);

                Utilities.ExecuteCommand(db, sql.ToString());

                return Utilities.GetLastInsertId(db, Tables.APP_SCRIPT_TRIG_PARAMS, Keys.APP_SCRIPT_TRIG_PARAMS);
            }

            public static DataTable Select(IDbConnection db, uint appScriptsId, uint appPartitionsId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.APP_SCRIPT_TRIG_PARAMS);
                if(appScriptsId != 0)      sql.where.Add(Keys.APP_SCRIPTS, appScriptsId);
                if(appPartitionsId != 0)   sql.where.Add(Keys.APP_PARTITIONS, appPartitionsId);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }

            public static int Update(IDbConnection db, uint id, string name, string Value)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, Tables.APP_SCRIPT_TRIG_PARAMS);
                if(name != null)    sql.AddFieldValue(Fields.NAME, name);
                if(Value != null)   sql.AddFieldValue(Fields.VALUE, Value);

                sql.where.Add(Keys.APP_SCRIPT_TRIG_PARAMS, id);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }

            public static int Delete(IDbConnection db, uint id, uint appScriptsId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, Tables.APP_SCRIPT_TRIG_PARAMS);
                if(id != 0)             sql.where.Add(Keys.APP_SCRIPT_TRIG_PARAMS, id);
                if(appScriptsId != 0)   sql.where.Add(Keys.APP_SCRIPTS, appScriptsId);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }
        }
        #endregion

        #region TriggerParamValues
        public abstract class TriggerParamValues
        {
            public static uint Insert(IDbConnection db, string Value, uint trigParamId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if((Value == null) || (trigParamId == 0)) { return 0; }

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.TRIG_PARAM_VALUES);
                sql.AddFieldValue(Fields.VALUE, Value);
                sql.AddFieldValue(Keys.APP_SCRIPT_TRIG_PARAMS, trigParamId);
                
                Utilities.ExecuteCommand(db, sql.ToString());

                return Utilities.GetLastInsertId(db, Tables.TRIG_PARAM_VALUES, Keys.TRIG_PARAM_VALUES);
            }

            public static DataTable Select(IDbConnection db, uint trigParamId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.TRIG_PARAM_VALUES);
                if(trigParamId != 0)      sql.where.Add(Keys.APP_SCRIPT_TRIG_PARAMS, trigParamId);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }

            public static int Delete(IDbConnection db, uint id, uint paramId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, Tables.TRIG_PARAM_VALUES);
                if(id != 0)         sql.where.Add(Keys.TRIG_PARAM_VALUES, id);
                if(paramId != 0)    sql.where.Add(Keys.APP_SCRIPT_TRIG_PARAMS, paramId);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }
        }
        #endregion

        #region AppPartitions
        public abstract class AppPartitions
        {
            public static uint Insert(IDbConnection db, uint componentsId, string name, bool enabled, 
                uint alarmGroupId, uint callRouteGroupId, uint mediaResouceGroupId, string description,
                string locale, string preferredCodec, bool useEarlyMedia)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if(name == null) { return 0; }

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.APP_PARTITIONS);
                sql.AddFieldValue(Fields.NAME, name);
                sql.AddFieldValue(Fields.CREATED_TS, new SqlBuilder.PreformattedValue(Fields.MYSQL_NOW));
                sql.AddFieldValue(Fields.ENABLED, enabled);
                sql.AddFieldValue(Keys.COMPONENTS, componentsId);
                sql.AddFieldValue(Fields.ALARM_GROUP_ID, alarmGroupId);
                sql.AddFieldValue(Fields.EARLY_MEDIA, useEarlyMedia);
                if(callRouteGroupId != 0)      sql.AddFieldValue(Fields.CALL_GROUP_ID, callRouteGroupId);
                if(mediaResouceGroupId != 0)   sql.AddFieldValue(Fields.MEDIA_GROUP_ID, mediaResouceGroupId);
                if(description != null)        sql.AddFieldValue(Fields.DESCRIPTION, description);
                if(locale != null)             sql.AddFieldValue(Fields.LOCALE, locale);
                if(preferredCodec != null)     sql.AddFieldValue(Fields.PREF_CODEC, preferredCodec);

                Utilities.ExecuteCommand(db, sql.ToString());

                return Utilities.GetLastInsertId(db, Tables.APP_PARTITIONS, Keys.APP_PARTITIONS);
            }

            public static DataTable Select(IDbConnection db, uint componentsId, string partitionName)
            {
                return Select(db, componentsId, partitionName, false, false);
            }

            public static DataTable Select(IDbConnection db, uint componentsId, string partitionName, 
                bool enabled, bool enabledSpecified)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.APP_PARTITIONS);
                if(componentsId != 0)       sql.where.Add(Keys.COMPONENTS, componentsId);
                if(partitionName != null)   sql.where.Add(Fields.NAME, partitionName);
                if(enabledSpecified)        sql.where.Add(Fields.ENABLED, enabled);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }

            public static int Update(IDbConnection db, uint id, string name, bool enabled, bool enabledSpecified,
                uint alarmGroupId, uint callRouteGroupId, uint mediaResouceGroupId, string description, string locale)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, Tables.APP_PARTITIONS);
                if(name != null)                sql.AddFieldValue(Fields.NAME, name);
                if(enabledSpecified)            sql.AddFieldValue(Fields.ENABLED, enabled);
                if(alarmGroupId != 0)           sql.AddFieldValue(Fields.ALARM_GROUP_ID, alarmGroupId);
                if(callRouteGroupId != 0)       sql.AddFieldValue(Fields.CALL_GROUP_ID, callRouteGroupId);
                if(mediaResouceGroupId != 0)    sql.AddFieldValue(Fields.MEDIA_GROUP_ID, mediaResouceGroupId);
                if(description != null)         sql.AddFieldValue(Fields.DESCRIPTION, description);
                if(locale != null)              sql.AddFieldValue(Fields.LOCALE, locale);

                sql.where.Add(Keys.APP_PARTITIONS, id);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }

            public static int Delete(IDbConnection db, uint id, uint componentsId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, Tables.APP_PARTITIONS);
                if(id != 0)             sql.where.Add(Keys.APP_PARTITIONS, id);
                if(componentsId != 0)    sql.where.Add(Keys.COMPONENTS, componentsId);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }
        }
        #endregion

        #region CallManagerDevices

        public abstract class CallManagerDevices
        {
            public static DataTable Select(IDbConnection db, uint componentsId, string name, 
                IConfig.DeviceType type, string dn, IConfig.Status status)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.CM_DEVICES);
                if(componentsId != 0)   sql.where.Add(Keys.COMPONENTS, componentsId);
                if(name != null)        sql.where.Add(Fields.DEVICE_NAME, name);
                if(dn != null)          sql.where.Add(Fields.DIR_NUMBER, dn);
                if(status != IConfig.Status.Unspecified)
                                        sql.where.Add(Fields.STATUS, status);
                if(type != IConfig.DeviceType.Unspecified)
                                        sql.where.Add(Fields.DEVICE_TYPE, type);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }

			public static bool Update(IDbConnection db, IConfig.DeviceType devType, IConfig.Status status, string dn)
			{
				return Update(db, 0, devType, status, dn);
			}

			public static bool Update(IDbConnection db, uint devicesId, IConfig.Status status, string dn)
			{
				return Update(db, devicesId, IConfig.DeviceType.Unspecified, status, dn);
			}

			private static bool Update(IDbConnection db, uint devicesId, IConfig.DeviceType devType, 
				IConfig.Status status, string dn)
			{
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

				if(devicesId == 0 && devType == IConfig.DeviceType.Unspecified)
					return false;

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, Tables.CM_DEVICES);
                if(status != IConfig.Status.Unspecified)
                                        sql.AddFieldValue(Fields.STATUS, status);
                if(dn != null)          sql.AddFieldValue(Fields.DIR_NUMBER, dn);

                if(devicesId != 0)		sql.where.Add(Keys.CM_DEVICES, devicesId);
				if(devType != IConfig.DeviceType.Unspecified)
										sql.where.Add(Fields.DEVICE_TYPE, devType);

                return Utilities.ExecuteCommand(db, sql.ToString()) != 0;
            }
        }

        #endregion

        #region CallManagerClusterMembers

        public abstract class CallManagerClusterMembers
        {
            public static DataTable Select(IDbConnection db, uint componentsId, uint callManagerClustersId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.CM_CLUSTER_MEMBERS);
                if(componentsId != 0)           sql.where.Add(Keys.COMPONENTS, componentsId);
                if(callManagerClustersId != 0)  sql.where.Add(Keys.CM_CLUSTERS, callManagerClustersId);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }
        }

        #endregion

        #region CallManagerClusters

        public abstract class CallManagerClusters
        {
            public static DataTable Select(IDbConnection db, uint callManagerClustersId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.CM_CLUSTERS);
                if(callManagerClustersId != 0)  sql.where.Add(Keys.CM_CLUSTERS, callManagerClustersId);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }
        }

        #endregion

        #region CallManagerClusterSubscribers

        public abstract class CallManagerClusterSubscribers
        {
            public static DataTable Select(IDbConnection db, uint callManagerClusterSubscribersId, uint callManagerClustersId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.CM_CLUSTER_SUBS);
                if(callManagerClusterSubscribersId != 0)    
                    sql.where.Add(Keys.CM_CLUSTER_SUBS, callManagerClusterSubscribersId);
                if(callManagerClustersId != 0)  
                    sql.where.Add(Keys.CM_CLUSTERS, callManagerClustersId);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }
        }

        #endregion

        #region CtiManagers

        public abstract class CtiManagers
        {
            public static DataTable Select(IDbConnection db, uint ctiManagersId, uint clusterId,
                string name, string address)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.CM_CTI_MANAGERS);
                if(ctiManagersId != 0)  sql.where.Add(Keys.CM_CTI_MANAGERS, ctiManagersId);
                if(clusterId != 0)      sql.where.Add(Keys.CM_CLUSTERS, clusterId);
                if(name != null)        sql.where.Add(Fields.NAME, name);
                if(address != null)     sql.where.Add(Fields.IP_ADDRESS, address);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }
        }

        #endregion

        #region SipDomains

        public abstract class SipDomains
        {
            public static DataTable Select(IDbConnection db, uint sipDomainsId, string domainName)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.SIP_DOMAINS);
                if(sipDomainsId != 0)       sql.where.Add(Keys.SIP_DOMAINS, sipDomainsId);
                if(domainName != null)      sql.where.Add(Fields.DOMAIN_NAME, domainName);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }
        }

        #endregion

        #region IetfSipDevices

        public abstract class IetfSipDevices
        {
            public static DataTable Select(IDbConnection db, uint componentsId, string username, 
                IConfig.Status status)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.SIP_DEVICES);
                if(componentsId != 0)   sql.where.Add(Keys.COMPONENTS, componentsId);
                if(username != null)    sql.where.Add(Fields.USERNAME, username);
                if(status != IConfig.Status.Unspecified)
                    sql.where.Add(Fields.STATUS, status);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }

            private static bool Update(IDbConnection db, uint devicesId, IConfig.Status status)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if(devicesId == 0)
                    return false;

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, Tables.SIP_DEVICES);
                if(status != IConfig.Status.Unspecified)
                    sql.AddFieldValue(Fields.STATUS, status);
                if(devicesId != 0)		sql.where.Add(Keys.CM_DEVICES, devicesId);

                return Utilities.ExecuteCommand(db, sql.ToString()) != 0;
            }
        }

        #endregion

        #region SipDomainMembers

        public abstract class SipDomainMembers
        {
            public static DataTable Select(IDbConnection db, uint componentsId, uint sipDomainsId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.SIP_DOMAIN_MEMBERS);
                if(componentsId != 0)   sql.where.Add(Keys.COMPONENTS, componentsId);
                if(sipDomainsId != 0)   sql.where.Add(Keys.SIP_DOMAINS, sipDomainsId);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }
        }

        #endregion

        #region SipProxies

        public abstract class SipProxies
        {
            public static DataTable Select(IDbConnection db, uint sipProxiesId, uint domainId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.SIP_PROXIES);
                if(sipProxiesId != 0)       sql.where.Add(Keys.SIP_PROXIES, sipProxiesId);
                if(domainId != 0)           sql.where.Add(Keys.SIP_DOMAINS, domainId);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }
        }

        #endregion

        #region Users
        public abstract class Users
        {
            public static uint Insert(IDbConnection db, string username, uint creatorId, 
                IConfig.AccessLevel accessLevel, string password)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if((username == null) || (accessLevel == IConfig.AccessLevel.Unspecified)) { return 0; }

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.USERS);
                sql.AddFieldValue(Fields.USERNAME, username);
                sql.AddFieldValue(Fields.CREATED_TS,  new SqlBuilder.PreformattedValue(Fields.MYSQL_NOW));
                sql.AddFieldValue(Fields.CREATOR_ID, creatorId);
                sql.AddFieldValue(Fields.ACCESS_LEVEL, accessLevel);
                if(password != null)    sql.AddFieldValue(Fields.PASSWORD, password);

                Utilities.ExecuteCommand(db, sql.ToString());

                return Utilities.GetLastInsertId(db, Tables.USERS, Keys.USERS);
            }

            public static DataTable Select(IDbConnection db, string username, IConfig.AccessLevel accessLevel)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.USERS);
                if(username != null)    sql.where.Add(Fields.USERNAME, username);
                if(accessLevel != IConfig.AccessLevel.Unspecified)
                    sql.where.Add(Fields.ACCESS_LEVEL, accessLevel);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }

            public static int Update(IDbConnection db, uint id, IConfig.AccessLevel accessLevel, string password)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, Tables.USERS);
                if(accessLevel != IConfig.AccessLevel.Unspecified)
                    sql.AddFieldValue(Fields.ACCESS_LEVEL, accessLevel);
                if(password != null)    sql.AddFieldValue(Fields.PASSWORD, password);

                sql.where.Add(Keys.USERS, id);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }

            public static int Delete(IDbConnection db, uint id)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, Tables.USERS);
                sql.where.Add(Keys.USERS, id);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }
        }
        #endregion

        #region UsersAclList
        public abstract class UsersAclList
        {
            public static void Insert(IDbConnection db, uint usersId, uint componentGroupsId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.USERS_ACL_LIST);
                sql.AddFieldValue(Keys.USERS, usersId);
                sql.AddFieldValue(Keys.COMPONENT_GROUPS, componentGroupsId);

                Utilities.ExecuteCommand(db, sql.ToString());
            }

            public static DataTable Select(IDbConnection db, uint usersId, uint componentGroupsId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.USERS_ACL_LIST);
                if(usersId != 0)           sql.where.Add(Keys.USERS, usersId);
                if(componentGroupsId != 0) sql.where.Add(Keys.COMPONENT_GROUPS, componentGroupsId);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }

            public static int Delete(IDbConnection db, uint usersId, uint componentGroupsId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, Tables.USERS_ACL_LIST);
                if(usersId != 0)           sql.where.Add(Keys.USERS, usersId);
                if(componentGroupsId != 0) sql.where.Add(Keys.COMPONENT_GROUPS, componentGroupsId);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }
        }
        #endregion

        #region ProviderExtensions
        public abstract class ProviderExtensions
        {
            public static uint Insert(IDbConnection db, uint componentsId, string name, bool sync, string description)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if(name == null) { return 0; }

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.PROV_EXTS);
                sql.AddFieldValue(Keys.COMPONENTS, componentsId);
                sql.AddFieldValue(Fields.NAME, name);
                sql.AddFieldValue(Fields.WAIT_COMPLETION, sync);
                if(description != null) sql.AddFieldValue(Fields.DESCRIPTION, description);

                Utilities.ExecuteCommand(db, sql.ToString());

                return Utilities.GetLastInsertId(db, Tables.PROV_EXTS, Keys.PROV_EXTS);
            }

            public static DataTable Select(IDbConnection db, uint componentsId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.PROV_EXTS);
                if(componentsId != 0)  sql.where.Add(Keys.COMPONENTS, componentsId);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }

            public static int Delete(IDbConnection db, uint id, uint componentsId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, Tables.PROV_EXTS);
                if(id != 0)             sql.where.Add(Keys.PROV_EXTS, id);
                if(componentsId != 0)   sql.where.Add(Keys.COMPONENTS, componentsId);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }
        }
        #endregion

        #region ProviderExtensionParams
        public abstract class ProviderExtensionParams
        {
            public static uint Insert(IDbConnection db, uint provExtsId, uint formatTypesId, string name, string description)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if(name == null) { return 0; }

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.PROV_EXT_PARAMS);
                sql.AddFieldValue(Keys.PROV_EXTS, provExtsId);
                sql.AddFieldValue(Keys.FORMAT_TYPES, formatTypesId);
                sql.AddFieldValue(Fields.NAME, name);
                if(description != null) sql.AddFieldValue(Fields.DESCRIPTION, description);

                Utilities.ExecuteCommand(db, sql.ToString());

                return Utilities.GetLastInsertId(db, Tables.PROV_EXT_PARAMS, Keys.PROV_EXT_PARAMS);
            }

            public static DataTable Select(IDbConnection db, uint provExtsId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.PROV_EXT_PARAMS);
                if(provExtsId != 0)    sql.where.Add(Keys.PROV_EXTS, provExtsId);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }

            public static int Delete(IDbConnection db, uint id, uint provExtsId)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, Tables.PROV_EXT_PARAMS);
                if(id != 0)         sql.where.Add(Keys.PROV_EXT_PARAMS, id);
                if(provExtsId != 0) sql.where.Add(Keys.PROV_EXTS, provExtsId);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }
        }
        #endregion

        #region EventLog
        public abstract class EventLog
        {
            public static uint Insert(IDbConnection db, IConfig.LogMsgType type, uint messageId, 
                string message, string data, IConfig.Severity severity)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if(type == IConfig.LogMsgType.Unspecified) { return 0; }

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.EVENT_LOG);
                sql.AddFieldValue(Fields.TYPE, type);
                sql.AddFieldValue(Fields.MESSAGE_ID, messageId);
                sql.AddFieldValue(Fields.CREATED_TS,  new SqlBuilder.PreformattedValue(Fields.MYSQL_NOW));
                if(message != null)     sql.AddFieldValue(Fields.MESSAGE, message);
                if(data != null)        sql.AddFieldValue(Fields.DATA, data);
                if(severity != IConfig.Severity.Unspecified)
                    sql.AddFieldValue(Fields.SEVERITY, severity);

                Utilities.ExecuteCommand(db, sql.ToString());

                return Utilities.GetLastInsertId(db, Tables.EVENT_LOG, Keys.EVENT_LOG);
            }

            public static int SetRecovered(IDbConnection db, uint id, string recoveredTS)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, Tables.EVENT_LOG);

                if(recoveredTS == null)
                    sql.AddFieldValue(Fields.RECOVERED_TS, new SqlBuilder.PreformattedValue(Fields.MYSQL_NOW));
                else
                    sql.AddFieldValue(Fields.RECOVERED_TS, recoveredTS);

				sql.AddFieldValue(Fields.STATUS, IConfig.EventStatus.Resolved);

				// Archive this alarm by obscuring the GUID
				sql.AddFieldValue(Fields.DATA, "System Recovered");

                sql.where.Add(Keys.EVENT_LOG, id);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }

            public static DataTable Select(IDbConnection db, IConfig.LogMsgType type, string message, bool onlyUnrecovered)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.EVENT_LOG);
                if(message != null)                         sql.where.Add(Fields.MESSAGE, message);
                if(onlyUnrecovered == true)                 sql.where.Add(Fields.RECOVERED_TS, null);
                if(type != IConfig.LogMsgType.Unspecified)  sql.where.Add(Fields.TYPE, type);
         
                return Utilities.ExecuteQuery(db, sql.ToString());
            }

            public static int Delete(IDbConnection db, uint id)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, Tables.EVENT_LOG);
                sql.where.Add(Keys.EVENT_LOG, id);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }

			public static uint InsertAlarm(	IDbConnection db, 
											uint messageId, 
											string message, 
											string guid, 
											IConfig.Severity severity)
			{
				Debug.Assert(db != null, "Database connection is null");
				Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

				SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.EVENT_LOG);
				sql.AddFieldValue(Fields.TYPE, IConfig.LogMsgType.Alarm);
				sql.AddFieldValue(Fields.MESSAGE_ID, messageId);
				sql.AddFieldValue(Fields.CREATED_TS,  new SqlBuilder.PreformattedValue(Fields.MYSQL_NOW));
				if(message != null)     sql.AddFieldValue(Fields.MESSAGE, message);
				sql.AddFieldValue(Fields.DATA, guid);
				if(severity != IConfig.Severity.Unspecified)
					sql.AddFieldValue(Fields.SEVERITY, severity);
				sql.AddFieldValue(Fields.STATUS, IConfig.EventStatus.Open);

				Utilities.ExecuteCommand(db, sql.ToString());

				return Utilities.GetLastInsertId(db, Tables.EVENT_LOG, Keys.EVENT_LOG);
			}

			public static DataTable SelectAlarmByGuid(IDbConnection db, string guid)
			{
				Debug.Assert(db != null, "Database connection is null");
				Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

				SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.EVENT_LOG);
				if(guid != null)        sql.where.Add(Fields.DATA, guid);
         
				return Utilities.ExecuteQuery(db, sql.ToString());
			}
        }
        #endregion

        #region License
        public abstract class License
        {
            public static uint Insert(IDbConnection db, string data, bool active)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if(data == null) { return 0; }

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.LICENSE);
                sql.AddFieldValue(Fields.LICENSE_DATA, data);
                sql.AddFieldValue(Fields.ACTIVE, active);
                sql.AddFieldValue(Fields.CREATED_TS,  new SqlBuilder.PreformattedValue(Fields.MYSQL_NOW));

                Utilities.ExecuteCommand(db, sql.ToString());

                return Utilities.GetLastInsertId(db, Tables.LICENSE, Keys.LICENSE);
            }

            public static DataTable Select(IDbConnection db, bool active, bool activeSpecified)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.LICENSE);
                if(activeSpecified) sql.where.Add(Fields.ACTIVE, active);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }

            public static int Delete(IDbConnection db, uint id)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, Tables.LICENSE);
                sql.where.Add(Keys.LICENSE, id);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }
        }
        #endregion

		#region Services
		public abstract class Services
		{
            public static uint Insert(IDbConnection db, string serviceName, string displayName, string description)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if((serviceName == null) || (serviceName == String.Empty))
                    return 0;

                if((displayName == null) || (displayName == String.Empty))
                    displayName = serviceName;

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, Tables.SERVICES);
                sql.AddFieldValue(Fields.NAME, serviceName);
                sql.AddFieldValue(Fields.DISPLAY_NAME, displayName);
                sql.AddFieldValue(Fields.DESCRIPTION, description);
                sql.AddFieldValue(Fields.APPSERVER_USE, 1);

                Utilities.ExecuteCommand(db, sql.ToString());

                return Utilities.GetLastInsertId(db, Tables.SERVICES, Keys.SERVICES);
            }

			public static DataTable Select(IDbConnection db)
			{
				Debug.Assert(db != null, "Database connection is null");
				Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

				SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.SERVICES);

				return Utilities.ExecuteQuery(db, sql.ToString());
			}

            public static int Update(IDbConnection db, uint id, bool userStopped)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, Tables.SERVICES);
                sql.AddFieldValue(Fields.USER_STOPPED, userStopped ? 1 : 0);
                sql.where.Add(Keys.SERVICES, id);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }

            public static int Delete(IDbConnection db, uint id, string displayName)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if(id == 0 && displayName == null)
                    return 0;

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, Tables.SERVICES);
                if(id != 0)             sql.where.Add(Keys.COMPONENTS, id);
                if(displayName != null) sql.where.Add(Fields.DISPLAY_NAME, displayName);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }
		}
		#endregion

        #region SnmpMibDefs

        public abstract class SnmpMibDefs
        {
            public static DataTable Select(IDbConnection db, IConfig.SnmpOidType type)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.SNMP_MIB_DEFS);
                if(type != IConfig.SnmpOidType.Unspecified)
                    sql.where.Add(Fields.TYPE, type);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }
        }
        #endregion

        #region SystemConfigs
        public abstract class SystemConfigs
        {
            public static DataTable Select(IDbConnection db, string valueName)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if(valueName == null)
                    return null;

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Tables.SYSTEM_CONFIGS);
                sql.where.Add(Fields.NAME, valueName);

                return Utilities.ExecuteQuery(db, sql.ToString());
            }

            public static int Update(IDbConnection db, string name, string value)
            {
                Debug.Assert(db != null, "Database connection is null");
                Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

                if (name == null || name == String.Empty)
                    return 0;

                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, Tables.SYSTEM_CONFIGS);
                sql.AddFieldValue(Fields.VALUE, value);

                sql.where.Add(Fields.NAME, name);

                return Utilities.ExecuteCommand(db, sql.ToString());
            }

            public static int Clear(IDbConnection db, string name)
            {
                return Update(db, name, null);
            }
        }
        #endregion

        #region Private Utilities
        internal abstract class Utilities
        {
            public static uint GetLastInsertId(IDbConnection db, string tableName, string keyName)
            {
                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, tableName);
                sql.fieldNames.Add(keyName);
                sql.where.Add(keyName, new SqlBuilder.PreformattedValue(Fields.LAST_INSERT_ID));

                DataTable data = Utilities.ExecuteQuery(db, sql.ToString());
                return (uint)data.Rows[0][keyName];
            }

            public static int ExecuteCommand(IDbConnection db, string sqlCommand)
            {
                lock(db)
                {
                    try
                    {
                        using(IDbCommand command = db.CreateCommand())
                        {
                            command.CommandText = sqlCommand;
                            return command.ExecuteNonQuery();
                        }
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }

            public static DataTable ExecuteQuery(IDbConnection db, string sqlQuery)
			{
				long t0;

				if (QueryLogger != null)
					t0 = HPTimer.Now();
				else
					t0 = 0;
                
				try
				{
					lock (db)
					{
						using (IDbCommand command = db.CreateCommand())
						{
							command.CommandText = sqlQuery;

							using (IDataReader reader = command.ExecuteReader())
							{
								return Database.GetDataTable(reader);
							}
						}
					}
				}
				catch (Exception e)
				{
					if (QueryException != null)
						QueryException(sqlQuery, e);
					return null;
				}
				finally
				{
					if (QueryLogger != null)
						QueryLogger(sqlQuery, HPTimer.NsSince( t0 ));
				}
            }
        }
        #endregion
        #endregion

        #region Helper Methods
        public static DataTable GetDataTable(IDataReader data)
        {
            bool abortFlag = false;
            return GetDataTable(data, false, ref abortFlag);
        }

        public static DataTable GetDataTable(IDataReader data, bool throttle, ref bool abortFlag)
        {
			DataTable table = new DataTable();

			bool firstPass = true;

			while(data.Read())
			{
				DataRow row = table.NewRow();

				for(int i=0; i<data.FieldCount; i++)
				{
                    if(abortFlag)
                        return null;

                    if(throttle)
                        System.Threading.Thread.Sleep(1);

					if(firstPass)
					{
						table.Columns.Add(data.GetName(i), data.GetFieldType(i));
					}

                    try
                    {
                        row[data.GetName(i)] = data.GetValue(i);
                    }
                    catch { /* SQLite sucks bigtime */ }
				}

				table.Rows.Add(row);
				firstPass = false;
			}

            // Return null if the reader had no data
            if(firstPass) { return null; }

			return table;
		}

        /// <summary>Creates a connection string for a MySQL database</summary>
        /// <param name="dbName">The database name</param>
        /// <param name="host">The server address</param>
        /// <param name="port">The server port (default: 3306)</param>
        /// <param name="username">Database user</param>
        /// <param name="password">Database user's password</param>
        /// <param name="enablePooling">Indicates that the SQL connector should pool connections</param>
        /// <param name="connectionTimeout">Number of seconds before connection times out</param>
        /// <returns>Connection string</returns>
        public static string FormatDSN(string dbName, string host, ushort port, string username, 
            string password, bool enablePooling)
        {
            return FormatDSN(dbName, host, port, username, password, enablePooling, 0);
        }

        /// <summary>Creates a connection string for a MySQL database</summary>
        /// <param name="dbName">The database name</param>
        /// <param name="host">The server address</param>
        /// <param name="port">The server port (default: 3306)</param>
        /// <param name="username">Database user</param>
        /// <param name="password">Database user's password</param>
        /// <param name="enablePooling">Indicates that the SQL connector should pool connections</param>
        /// <param name="connectionTimeout">Number of seconds before connection times out</param>
        /// <returns>Connection string</returns>
        public static string FormatDSN(string dbName, string host, ushort port, string username, 
            string password, bool enablePooling, uint connectionTimeout)
        {
            //"DATABASE=SiteTracker;Driver=mysql;SERVER=localhost;UID=hamu;PWD
            //=naptra;PORT=3306;OPTION=131072;STMT=;"
            StringBuilder dsn = new StringBuilder();
            
            if(dbName == null) { return null; }

            dsn.Append("database=");
            dsn.Append(dbName);
            
            if(host != null)
            {
                dsn.Append("; server=");
                dsn.Append(host);
            }
            if(port != 0)
            {
                dsn.Append("; port=");
                dsn.Append(port);
            }
            if(username != null)
            {
                dsn.Append("; uid=");
                dsn.Append(username);
            }
            if(password != null)
            {
                dsn.Append("; pwd=");
                dsn.Append(password);
            }

            if(enablePooling)
            {
                dsn.Append("; pooling=true");
            }
            else
            {
                dsn.Append("; pooling=false");
                dsn.Append("; connection lifetime=0");
            }

            if(connectionTimeout > 0)
            {
                dsn.Append("; connection timeout=");
                dsn.Append(connectionTimeout);
            }

            return dsn.ToString();
        }

        /// <summary>
        ///     Creates a connection to the specified database, using the specified DSN
        /// </summary>
        /// <param name="dbType">
        ///     The type of database to use when using a connection
        /// </param>
        /// <param name="dsn">
        ///     The DSN to connect with
        /// </param>
        /// <exception cref="Exception">
        ///     No exceptions are caught by this method.  Please provide your own 
        ///     exception handling.
        /// </exception>
        /// <returns>
        ///     The connection to the database
        /// </returns>
        public static IDbConnection CreateConnection(DbType dbType, string dsn)
        {
            IDbConnection connection = null;
            switch(dbType)
            {
                case DbType.oracle:
                    connection = new System.Data.OracleClient.OracleConnection(dsn);
                    break;
                case DbType.sqlserver:
                    connection = new System.Data.SqlClient.SqlConnection(dsn);
                    break;
                case DbType.mysql:
                    connection = new MySql.Data.MySqlClient.MySqlConnection(dsn);
                    break;
                default:
                    connection = new System.Data.Odbc.OdbcConnection(dsn);
                    break;
            }

            return connection;
        }

        public enum DbType
        {
            oracle,
            sqlserver,
            mysql,
            odbc
        }
		#endregion

		public static QueryLoggerDelegate QueryLogger;
		public static QueryExceptionDelegate QueryException;
	}

	public delegate void QueryLoggerDelegate(String query, long nanos);

	public delegate void QueryExceptionDelegate(String query, Exception e);
}
