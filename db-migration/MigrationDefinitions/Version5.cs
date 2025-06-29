using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Metreos.Utilities;
using Metreos.Utilities.DbBuilders;


namespace MigrationDefinitions
{
    /// <summary>
    /// Migration from DB version 5 (MCE 2.1.4 to 2.2.0)
    /// </summary>
    [MigrationCore.Version("5")]
    public class Version5 : MigrationCore.MigrationDefinition
    {

        #region Constants

        private abstract class TableNames
        {
            public const string COMPONENTS                  = "mce_components";
            public const string COMPONENT_GROUPS            = "mce_component_groups";
            public const string ENTRIES                     = "mce_config_entries";
            public const string META_ENTRIES                = "mce_config_entry_metas";
            public const string VALUES                      = "mce_config_values";
            public const string COMPONENT_GROUP_MEMBERS     = "mce_component_group_members";
            public const string FORMAT_TYPES                = "mce_format_types";
        }

        private abstract class ColumnNames
        {
            public const string COMPONENTS_ID               = "mce_components_id";
            public const string COMPONENT_GROUPS_ID         = "mce_component_groups_id";
            public const string ENTRIES_ID                  = "mce_config_entries_id";
            public const string META_ENTRIES_ID             = "mce_config_entry_metas_id";
            public const string VALUES_ID                   = "mce_config_values_id";
            public const string COMPONENT_GROUP_MEMBERS_ID  = "mce_component_group_members_id";
            public const string FORMAT_TYPES_ID             = "mce_format_types_id";
        }

        #endregion


        public Version5(Database.DbType dbtype, ref IDbConnection dbObj)
            : base(dbtype, ref dbObj)
        {
            this.ready = true;
            this.currentVersion = "5";
        }

        public override bool DoUpgrade()
        {
            this.UseInnoDB();
            
            this.AddSipTables();
            this.AddSipTrunkConfigMeta();
            this.AddSipPoolConfigMetas();
            this.AddSipGroup();

            this.AddSccpDevicePoolConfigMetas();
            this.AddClusterInterface();
            this.AddEarlyMedia();
            this.AddSnmpCommunityToCMClusters();
            this.AddIpcConnectionType();
            this.AddNewServices();
            this.AddRedundancyConfigs();

            return true;
        }

        public override bool DoRollback()
        {
            throw new Exception("Roll back from this database version is not supported.");
        }


        #region Actions

        private void UseInnoDB()
        {
            // Set tables to use InnoDB engine
            string template = "ALTER TABLE {0} ENGINE=InnoDB";

            ArrayList tables = this.GetTableList();
            foreach (string tbl in tables)
            {
                this.ExecuteNonQuery(new StringBuilder().AppendFormat(template, tbl).ToString());
            }
        }

        private void AddSipTables()
        {
            // SIP Domains
            IDbTableBuilder domain_tbl = this.GetTableBuilder("mce_sip_domains");
            domain_tbl.Columns.Add(this.CreateIdColumn("mce_sip_domains_id"));
            domain_tbl.Columns.Add(this.GetColumnBuilder("domain_name", ColType.VARCHAR).SetLength(128).AddModifier(ColModifier.NOTNULL));
            domain_tbl.Columns.Add(this.GetColumnBuilder("primary_registrar", ColType.VARCHAR).SetLength(16).AddModifier(ColModifier.NOTNULL));
            domain_tbl.Columns.Add(this.GetColumnBuilder("secondary_registrar", ColType.VARCHAR).SetLength(16));
            domain_tbl.Columns.Add(this.GetColumnBuilder("type", ColType.INT).SetDefault("0").AddModifier(ColModifier.UNSIGNED).AddModifier(ColModifier.NOTNULL));
            domain_tbl.PrimaryKey = "mce_sip_domains_id";
            this.ExecuteNonQuery(domain_tbl.ToString());

            // SIP domain membership
            IDbTableBuilder domain_members_tbl = this.GetTableBuilder("mce_sip_domain_members");
            domain_members_tbl.Columns.Add(this.GetColumnBuilder("mce_sip_domains_id", ColType.INT).AddModifier(ColModifier.UNSIGNED).AddModifier(ColModifier.NOTNULL));
            domain_members_tbl.Columns.Add(this.GetColumnBuilder(ColumnNames.COMPONENTS_ID, ColType.INT).AddModifier(ColModifier.UNSIGNED).AddModifier(ColModifier.NOTNULL));
            domain_members_tbl.PrimaryKey = "mce_sip_domains_id, mce_components_id";
            this.ExecuteNonQuery(domain_members_tbl.ToString());

            // SIP proxies
            IDbTableBuilder proxy_tbl = this.GetTableBuilder("mce_sip_outbound_proxies");
            proxy_tbl.Columns.Add(this.CreateIdColumn("mce_sip_outbound_proxies_id"));
            proxy_tbl.Columns.Add(this.GetColumnBuilder("mce_sip_domains_id", ColType.INT).AddModifier(ColModifier.UNSIGNED).AddModifier(ColModifier.NOTNULL));
            proxy_tbl.Columns.Add(this.GetColumnBuilder("ip_address", ColType.VARCHAR).SetLength(16).AddModifier(ColModifier.NOTNULL));
            proxy_tbl.PrimaryKey = "mce_sip_outbound_proxies_id";
            this.ExecuteNonQuery(proxy_tbl.ToString());

            // Add standard SIP devices table
            IDbTableBuilder devices_tbl = this.GetTableBuilder("mce_ietf_sip_devices");
            devices_tbl.Columns.Add(this.CreateIdColumn("mce_ietf_sip_devices_id"));
            devices_tbl.Columns.Add(this.GetColumnBuilder(ColumnNames.COMPONENTS_ID, ColType.INT).AddModifier(ColModifier.UNSIGNED).
                AddModifier(ColModifier.NOTNULL).SetDefault("0"));
            devices_tbl.Columns.Add(this.GetColumnBuilder("username", ColType.VARCHAR).SetLength(128).SetDefault("").AddModifier(ColModifier.NOTNULL));
            devices_tbl.Columns.Add(this.GetColumnBuilder("password", ColType.VARCHAR).SetLength(255).SetDefault("").AddModifier(ColModifier.NOTNULL));
            devices_tbl.Columns.Add(this.GetColumnBuilder("status", ColType.INT).SetDefault("0").AddModifier(ColModifier.UNSIGNED).AddModifier(ColModifier.NOTNULL));
            devices_tbl.PrimaryKey = "mce_ietf_sip_devices_id";
            this.ExecuteNonQuery(devices_tbl.ToString());
        }

        private void AddEarlyMedia()
        {
            IDbTableModifier tbl = this.GetTableModifier("mce_application_partitions");
            tbl.AddColumn(this.GetColumnBuilder("use_early_media", ColType.TINYINT).SetLength(1).SetDefault("0").
                AddModifier(ColModifier.UNSIGNED).AddModifier(ColModifier.NOTNULL).PlaceAfterColumn("preferred_codec"));
            this.ExecuteNonQuery(tbl.ToString());
        }

        private void AddNewServices()
        {
            this.AddService("MetreosSipStack", "Sip Stack", "SIP stack");
            this.AddService("MetreosJTAPIStack_CCM-4-2", "JTAPI Stack CCM-4-2", "JTAPI CCM-4-2");
            this.AddService("MetreosJTAPIStack_CCM-5-0", "JTAPI Stack CCM-5-0", "JTAPI CCM-5-0");
        }

        private void AddIpcConnectionType()
        {
            // Check for existing IPC type
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, TableNames.FORMAT_TYPES);
            sql.fieldNames.Add(ColumnNames.FORMAT_TYPES_ID);
            sql.where.Add("name", "ConnectionType");
            DataTable rsx = this.ExecuteQuery(sql.ToString());
            uint format_id = (uint)rsx.Rows[0][ColumnNames.FORMAT_TYPES_ID];

            SqlBuilder asql = new SqlBuilder(SqlBuilder.Method.SELECT, "mce_format_type_enum_values");
            asql.fieldNames.Add("COUNT(*) as count");
            asql.where.Add("value", "IPC");
            asql.where.Add(ColumnNames.FORMAT_TYPES_ID, format_id);
            DataTable rs = this.ExecuteQuery(asql.ToString());

            if ((Int64)rs.Rows[0]["count"] == 0)
            {
                // Add IPC type
                SqlBuilder bsql = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_format_type_enum_values");
                bsql.AddFieldValue("value", "IPC");
                bsql.AddFieldValue(ColumnNames.FORMAT_TYPES_ID, format_id);
                this.ExecuteNonQuery(bsql.ToString());
            }
        }

        private void AddClusterInterface()
        {
            // Add Cluster Interface components
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.COMPONENTS);
            sql.AddFieldValue("name", "ClusterInterface");
            sql.AddFieldValue("display_name", "Cluster Interface");
            sql.AddFieldValue("type", 1);
            sql.AddFieldValue("status", 4);
            sql.AddFieldValue("version", "2.2");
            sql.AddFieldValue("description", "Cluster Interface");
            this.ExecuteNonQuery(sql.ToString());

            int cluster_id = this.GetLastId();

            // Add logging config entry
            SqlBuilder asql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.ENTRIES);
            asql.AddFieldValue(ColumnNames.COMPONENTS_ID, cluster_id);
            asql.AddFieldValue(ColumnNames.META_ENTRIES_ID, 1);
            this.ExecuteNonQuery(asql.ToString());

            int cluster_entry_id = this.GetLastId();

            SqlBuilder bsql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.VALUES);
            bsql.AddFieldValue(ColumnNames.ENTRIES_ID, cluster_entry_id);
            bsql.AddFieldValue("value", "Warning");
            this.ExecuteNonQuery(bsql.ToString());

            // Add extenstion
            SqlBuilder csql = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_provider_extensions");
            csql.AddFieldValue(ColumnNames.COMPONENTS_ID, cluster_id);
            csql.AddFieldValue("name", "PrintDiags");
            csql.AddFieldValue("description", "Prints diagnostic information to logging");
            csql.AddFieldValue("wait_for_completion", 0);
            csql.ToString();
            this.ExecuteNonQuery(csql.ToString());
        }

        private void AddSipGroup()
        {
            // Add default SIP group
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.COMPONENT_GROUPS);
            sql.AddFieldValue("name", "Default SIP");
            sql.AddFieldValue("component_type", 102);
            sql.AddFieldValue("default_group", 1);
            sql.AddFieldValue("description", "Default SIP resource group");
            sql.AddFieldValue("mce_alarm_group_id", 6);
            this.ExecuteNonQuery(sql.ToString());
        }

        private void AddSnmpCommunityToCMClusters()
        {
            // Add SNMP community field to call manager table
            IDbTableModifier tblmod = this.GetTableModifier("mce_call_manager_clusters");
            tblmod.AddColumn(this.GetColumnBuilder("snmp_community", ColType.VARCHAR).SetLength(128).SetDefault("").AddModifier(ColModifier.NOTNULL));
            this.ExecuteNonQuery(tblmod.ToString());
        }

        private void AddSipPoolConfigMetas()
        {
            // Add IETF/Cisco SIP Device Pool configs
            this.AddConfigMeta(40, "MetreosReserved_Username", "Username", "Username to allow monitoring the SIP Device Pool", true, 102, 1);
            this.AddConfigMeta(41, "MetreosReserved_Password", "Password", "Password for monitoring the SIP Device Pool", true, 102, 9);
            this.AddConfigMeta(42, "MetreosReserved_OutboundProxyId", "Outbound Proxy", "Outbound proxy for the SIP Device Pool", true, 102, 3);

            this.AddConfigMeta(47, "MetreosReserved_Username", "Username", "Username to allow monitoring the SIP Device Pool", true, 106, 1);
            this.AddConfigMeta(48, "MetreosReserved_Password", "Password", "Password for monitoring the SIP Device Pool", true, 106, 9);
            this.AddConfigMeta(49, "MetreosReserved_OutboundProxyId", "Outbound Proxy", "Outbound proxy for the SIP Device Pool", true, 106, 3);
        }

        private void AddSccpDevicePoolConfigMetas()
        {
            // Add SCCP Device Pool configs
            this.AddConfigMeta(44, "MetreosReserved_TertiarySubscriberId", "Tertiary Subscriber", "Tertiary subscriber for the SCCP Device Pool", false, 100, 3);
            this.AddConfigMeta(45, "MetreosReserved_QuaternarySubscriberId", "Quaternary Subscriber", "Quaternary subscriber for the SCCP Device Pool", false, 100, 3);
            this.AddConfigMeta(46, "MetreosReserved_SRST", "SRST", "Subscriber assigned as SRST for the SCCP Device Pool", false, 100, 3);
        }

        private void AddSipTrunkConfigMeta()
        {
            // Add SIP Domain Trunk Interface config
            this.AddConfigMeta(43, "MetreosReserved_RegistrarIpAddress", "Registrar Address", "Address of the registrar the trunk interface uses", true, 105, 1);
        }

        private void AddRedundancyConfigs()
        {
            this.AddSystemConfig("redundancy_master_ip", "");
            this.AddSystemConfig("redundancy_master_heartbeat", "5");
            this.AddSystemConfig("redundancy_master_max_missed_heartbeat", "2");
            this.AddSystemConfig("redundancy_standby_ip", "");
            this.AddSystemConfig("redundancy_standby_startup_sync_time", "5");
        }

        #endregion


        #region Helper Methods

        private void AddConfigMeta(int id, string name, string display, string description, bool required, int component_type, int format_type)
        {
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.META_ENTRIES);
            sql.AddFieldValue(ColumnNames.META_ENTRIES_ID, id);
            sql.AddFieldValue("name", name);
            sql.AddFieldValue("display_name", display);
            sql.AddFieldValue("description", description);
            sql.AddFieldValue("required", (int)(required ? 1 : 0));
            sql.AddFieldValue("component_type", component_type);
            sql.AddFieldValue(ColumnNames.FORMAT_TYPES_ID, format_type);
            this.ExecuteNonQuery(sql.ToString());
        }

        private void AddSystemConfig(string name, string value)
        {
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_system_configs");
            sql.AddFieldValue("name", name);
            sql.AddFieldValue("value", value);
            this.ExecuteNonQuery(sql.ToString());
        }

        private void AddService(string name, string display, string description)
        {
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_services");
            sql.AddFieldValue("name", name);
            sql.AddFieldValue("display_name", display);
            sql.AddFieldValue("description", description);
            sql.AddFieldValue("app_server_use", 1);
            this.ExecuteQuery(sql.ToString());
        }

        #endregion
    }

}
