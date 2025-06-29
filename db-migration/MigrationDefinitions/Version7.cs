using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Metreos.Utilities;
using Metreos.Utilities.DbBuilders;


// Version 2.2.1 to 2.3.0

namespace MigrationDefinitions
{
    [MigrationCore.Version ("7")]
    public class Version7 : MigrationCore.MigrationDefinition
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
            public const string SERVICES                    = "mce_services";
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


        public Version7(Database.DbType dbtype, ref IDbConnection dbObj)
            : base(dbtype, ref dbObj)
        {
            this.ready = true;
            this.currentVersion = "7";
        }

        public override bool DoUpgrade()
        {
            this.DropMsmqConnectionType();
            this.DropOldServices();
            this.DropOldSystemConfigs();
            this.DropComponent("PCapServiceProvider", 3);
            this.DropComponent("SccpProxyProvider", 3);

            this.SetIpcConnectionType();
            this.AddMediaPasswordSysconfig();
            this.AddNewServices();

            return true;
        }

        public override bool DoRollback()
        {
            return true;
        }


        #region Actions

        private void SetIpcConnectionType()
        {
            // Set all media servers to use IPC
            int meta_id = 38;

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, TableNames.FORMAT_TYPES);
            sql.fieldNames.Add(ColumnNames.FORMAT_TYPES_ID);
            sql.where.Add("name", "ConnectionType");
            DataTable rsx = this.ExecuteQuery(sql.ToString());
            uint format_id = (uint) rsx.Rows[0][ColumnNames.FORMAT_TYPES_ID];

            // Set media servers to use IPC
            SqlBuilder csql = new SqlBuilder(SqlBuilder.Method.SELECT, TableNames.COMPONENTS);
            csql.fieldNames.Add(ColumnNames.COMPONENTS_ID);
            csql.where.Add("type", 4);
            DataTable rs2 = this.ExecuteQuery(csql.ToString());
            for (int x = 0; x < rs2.Rows.Count; x++)
            {
                SqlBuilder dsql = new SqlBuilder(SqlBuilder.Method.SELECT, TableNames.ENTRIES);
                dsql.where.Add(ColumnNames.COMPONENTS_ID, rs2.Rows[x][ColumnNames.COMPONENTS_ID]);
                dsql.where.Add(ColumnNames.META_ENTRIES_ID, meta_id);
                DataTable drs = this.ExecuteQuery(dsql.ToString());
                uint entry_id = (uint) drs.Rows[0][ColumnNames.ENTRIES_ID];

                SqlBuilder esql = new SqlBuilder(SqlBuilder.Method.REPLACE, TableNames.VALUES);
                esql.AddFieldValue(ColumnNames.ENTRIES_ID, entry_id);
                esql.AddFieldValue("value", "IPC");
                this.ExecuteNonQuery(esql.ToString());
            }
        }

        private void DropMsmqConnectionType()
        {
            // Get rid of the MSMQ connection type for media servers
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, TableNames.FORMAT_TYPES);
            sql.fieldNames.Add(ColumnNames.FORMAT_TYPES_ID);
            sql.where.Add("name", "ConnectionType");
            DataTable rsx = this.ExecuteQuery(sql.ToString());
            uint format_id = (uint) rsx.Rows[0][ColumnNames.FORMAT_TYPES_ID];

            SqlBuilder asql = new SqlBuilder(SqlBuilder.Method.DELETE, "mce_format_type_enum_values");
            asql.where.Add(ColumnNames.FORMAT_TYPES_ID, format_id);
            asql.where.Add("value", "MSMQ");
            this.ExecuteNonQuery(asql.ToString());
        }

        private void DropOldServices()
        {
            // Delete services
            this.DropService("MetreosSMIServer");
            this.DropService("MetreosRtpRelayService");
            this.DropService("MetreosPCapService");
        }

        private void AddNewServices()
        {
            // Add new services
            SqlBuilder sftps = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.SERVICES);
            sftps.AddFieldValue("name", "SftpServerService");
            sftps.AddFieldValue("display_name", "SFTP Server");
            sftps.AddFieldValue("all_use", 1);
            sftps.AddFieldValue("description", "Secure file transfer server");
            this.ExecuteNonQuery(sftps.ToString());
        }

        private void AddMediaPasswordSysconfig()
        {
            // Add local media server password
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_system_configs");
            sql.AddFieldValue("name", "media_provisioning_password");
            this.ExecuteNonQuery(sql.ToString());
        }

        private void DropOldSystemConfigs()
        {
            // Delete unnecessary system configs
            this.DropSystemConfig("system_friendly_name");
            this.DropSystemConfig("time_zone");
            this.DropSystemConfig("ntp_enabled");
            this.DropSystemConfig("ntp_servers");
            this.DropSystemConfig("ntp_pollinterval");
            this.DropSystemConfig("ntp_maxposcorrection");
            this.DropSystemConfig("ntp_maxnegcorrection");
        }

        #endregion


        #region Helper Methods

        private void DropService(string name)
        {
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, TableNames.SERVICES);
            sql.where.Add("name", name);
            this.ExecuteNonQuery(sql.ToString());
        }

        private void DropSystemConfig(string name)
        {
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, "mce_system_configs");
            sql.where.Add("name", name);
            this.ExecuteNonQuery(sql.ToString());
        }

        #endregion

    }

}
