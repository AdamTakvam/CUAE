using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Metreos.Utilities;
using Metreos.Utilities.DbBuilders;


// Version 2.3.0 to 2.3.1

namespace MigrationDefinitions
{
    [MigrationCore.Version("8")]
    public class Version8 : MigrationCore.MigrationDefinition
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


        public Version8(Database.DbType dbtype, ref IDbConnection dbObj)
            : base(dbtype, ref dbObj)
        {
            this.ready = true;
            this.currentVersion = "8";
        }

        public override bool DoUpgrade()
        {
            this.DropRtpRelay();
            this.RemoveLoggerConfigs();
            this.AddMonitoredCtiMetaConfigs();
            this.UpdateServiceDisplayNames();
            this.UpdateConfigMetas();
            this.IncreaseSipRegistrarLength();

            return true;
        }

        public override bool DoRollback()
        {
            return true;
        }


        #region Actions

        private void DropRtpRelay()
        {
            // Get rid of the RTP component
            int rtp_type = 6;

            this.DropComponent("RtpRelay", rtp_type);
            SqlBuilder dropmetas = new SqlBuilder(SqlBuilder.Method.DELETE, TableNames.META_ENTRIES);
            dropmetas.where.Add("component_type", rtp_type);
            this.ExecuteNonQuery(dropmetas.ToString());
        }

        private void RemoveLoggerConfigs()
        {
            // Delete unneccessary Logger configs
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, TableNames.VALUES);
            sql.AddCondition(new SqlBetweenCondition(ColumnNames.VALUES_ID, 15, 19));
            this.ExecuteNonQuery(sql.ToString());

            SqlBuilder asql = new SqlBuilder(SqlBuilder.Method.DELETE, TableNames.ENTRIES);
            asql.AddCondition(new SqlBetweenCondition(ColumnNames.ENTRIES_ID, 15, 19));
            this.ExecuteNonQuery(asql.ToString());

            SqlBuilder bsql = new SqlBuilder(SqlBuilder.Method.DELETE, TableNames.META_ENTRIES);
            bsql.AddCondition(new SqlBetweenCondition(ColumnNames.META_ENTRIES_ID, 110, 113));
            this.ExecuteNonQuery(bsql.ToString());
        }

        private void AddMonitoredCtiMetaConfigs()
        {
            // Add meta configs for new Monitored CTI Device Pool type
            this.AddConfigMeta(50, "MetreosReserved_PrimaryCtiManagerId", "Primary CTI Manager", "Primary CTI Manager for the Monitored CTI Devices", true, 107, 3);
            this.AddConfigMeta(51, "MetreosReserved_SecondaryCtiManagerId", "Secondary CTI Manager", "Secondary CTI Manager for the Monitored CTI Devices", false, 107, 3);
            this.AddConfigMeta(52, "MetreosReserved_Username", "Username", "Username to allow monitoring of the Monitored CTI Devices", true, 107, 1);
            this.AddConfigMeta(53, "MetreosReserved_Password", "Password", "Password for monitoring the Monitored CTI Devices", true, 107, 9);
        }

        private void UpdateServiceDisplayNames()
        {
            // Update service names
            this.ChangeServiceDisplayName("MediaServerService", "Media Engine");
            this.ChangeServiceDisplayName("MetreosAppServerService", "Application Server");
            this.ChangeServiceDisplayName("MetreosWatchDog", "Watchdog Server");

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, "mce_services");
            sql.AddFieldValue("description", "Media engine");
            sql.where.Add("name", "MediaServerService");
            this.ExecuteNonQuery(sql.ToString());
        }

        private void UpdateConfigMetas()
        {
            // Update configuration descriptions
            this.UpdateMetaConfigDisplay(7, "Address", "Address to the Media Engine");
            this.UpdateMetaConfigDisplay(10, "Address", "Address to the H.323 Gateway");
            this.UpdateMetaConfigDisplay(21, "Address", "Address to the SNMP manager");
            this.UpdateMetaConfigDisplay(37, "Password", "Password to access the Media Engine");
            this.UpdateMetaConfigDisplay(38, "Connection Type", "Connection method to the Media Engine");
            this.UpdateMetaConfigDisplay(43, "Registrar Address", "Address of the registrar the trunk interface uses");
        }

        private void IncreaseSipRegistrarLength()
        {
            // Update SIP domain registrar fiel length
            IDbTableModifier sipdomain = this.GetTableModifier("mce_sip_domains");
            sipdomain.ChangeColumn("primary_registrar", 
                this.GetColumnBuilder("primary_registrar", ColType.VARCHAR).SetLength(128).AddModifier(ColModifier.NOTNULL));
            sipdomain.ChangeColumn("secondary_registrar",
                this.GetColumnBuilder("secondary_registrar", ColType.VARCHAR).SetLength(128));
            this.ExecuteNonQuery(sipdomain.ToString());
        }

        #endregion


        #region Private Helpers

        private void AddConfigMeta(int id, string name, string display, string description, bool required, int component_type, int format_type)
        {
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.META_ENTRIES);
            sql.AddFieldValue(ColumnNames.META_ENTRIES_ID, id);
            sql.AddFieldValue("name", name);
            sql.AddFieldValue("display_name", display);
            sql.AddFieldValue("description", description);
            sql.AddFieldValue("required", (int) (required ? 1 : 0));
            sql.AddFieldValue("component_type", component_type);
            sql.AddFieldValue(ColumnNames.FORMAT_TYPES_ID, format_type);
            this.ExecuteNonQuery(sql.ToString());
        }

        private void ChangeServiceDisplayName(string keyname, string display_name)
        {
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, "mce_services");
            sql.AddFieldValue("display_name", display_name);
            sql.where.Add("name", keyname);
            this.ExecuteNonQuery(sql.ToString());
        }

        private void UpdateMetaConfigDisplay(int id, string display_name, string description)
        {
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, TableNames.META_ENTRIES);
            sql.AddFieldValue("display_name", display_name);
            sql.AddFieldValue("description", description);
            sql.where.Add(ColumnNames.META_ENTRIES_ID, id);
            this.ExecuteNonQuery(sql.ToString());
        }

        #endregion
    }

}
