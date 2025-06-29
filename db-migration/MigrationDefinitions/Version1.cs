using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Metreos.Utilities;
using Metreos.Utilities.DbBuilders;


namespace MigrationDefinitions
{
    /// <summary>
    /// Migration from DB version 1 (MCE 2.1.0 to 2.1.1)
    /// </summary>
    [MigrationCore.Version ("1")]
    public class Version1 : MigrationCore.MigrationDefinition
    {

        #region Constants

        private abstract class TableNames
        {
            public const string COMPONENTS          = "mce_components";
            public const string COMPONENT_GROUPS    = "mce_component_groups";
            public const string ENTRIES             = "mce_config_entries";
            public const string META_ENTRIES        = "mce_config_entry_metas";
            public const string VALUES              = "mce_config_values";
        }

        private abstract class ColumnNames
        {
            public const string COMPONENTS_ID       = "mce_components_id";
            public const string COMPONENT_GROUPS_ID = "mce_component_groups_id";
            public const string ENTRIES_ID          = "mce_config_entries_id";
            public const string META_ENTRIES_ID     = "mce_config_entry_metas_id";
            public const string VALUES_ID           = "mce_config_values_id";
        }

        #endregion


        public Version1(Database.DbType dbtype, ref IDbConnection dbObj)
            : base(dbtype, ref dbObj)
        {
            this.ready = true;
            this.currentVersion = "1";
        }

        public override bool DoUpgrade()
        {
            this.AddDefaultGroupCol();
            this.AddDisplayNameCol();
            this.AddMediaServerPasswords();
            this.AddLoggerDiagnostic();
            
            return true;
        }

        public override bool DoRollback()
        {
            throw new Exception("Roll back from this database version is not supported.");
        }


        #region Actions

        private void AddDefaultGroupCol()
        {
            // Add default_group column to component group table

            IDbTableModifier tbl = this.GetTableModifier(TableNames.COMPONENT_GROUPS);
            tbl.AddColumn(this.GetColumnBuilder("default_group", ColType.TINYINT).SetLength(1).SetDefault("0").
                AddModifier(ColModifier.NOTNULL).AddModifier(ColModifier.UNSIGNED).PlaceAfterColumn("component_type"));
            this.ExecuteNonQuery(tbl.ToString());

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, TableNames.COMPONENT_GROUPS);
            sql.AddFieldValue("default_group", 1);
            sql.AddCondition(new SqlCompareCondition(ColumnNames.COMPONENT_GROUPS_ID, SqlCompareCondition.Comparator.LTE, 9));
            this.ExecuteNonQuery(sql.ToString());
        }

        private void AddDisplayNameCol()
        {
            // Add display name column to component table

            IDbTableModifier tbl = this.GetTableModifier(TableNames.COMPONENTS);
            tbl.AddColumn(this.GetColumnBuilder("display_name", ColType.VARCHAR).SetLength(128).SetDefault("").
                AddModifier(ColModifier.NOTNULL).PlaceAfterColumn("name"));
            this.ExecuteNonQuery(tbl.ToString());

            AddDisplayName("ApplicationServer", "Application Server");
            AddDisplayName("AppManager", "Application Manager");
            AddDisplayName("ApplicationEnvironment", "Application Environment");
            AddDisplayName("ProviderManager", "Provider Manager");
            AddDisplayName("Management", "Management Interface");
            AddDisplayName("TelephonyManager", "Telephony Manager");
            AddDisplayName("LogServer", "Logging Service");
        }

        private void AddMediaServerPasswords()
        {
            // Add password config to media servers

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.META_ENTRIES);
            sql.AddFieldValue(ColumnNames.META_ENTRIES_ID, 37);
            sql.AddFieldValue("name", "MetreosReserved_Password");
            sql.AddFieldValue("display_name", "Password");
            sql.AddFieldValue("description", "Password to access the Media Server");
            sql.AddFieldValue("component_type", 4);
            sql.AddFieldValue("mce_format_types_id", 9);
            this.ExecuteNonQuery(sql.ToString());

            SqlBuilder sql2 = new SqlBuilder(SqlBuilder.Method.UPDATE, TableNames.META_ENTRIES);
            sql2.AddFieldValue("read_only", 1);
            sql2.where.Add(ColumnNames.META_ENTRIES_ID, 7);
            this.ExecuteNonQuery(sql2.ToString());

            // Set default passwords

            SqlBuilder sql3 = new SqlBuilder(SqlBuilder.Method.SELECT, TableNames.COMPONENTS);
            sql3.fieldNames.Add(ColumnNames.COMPONENTS_ID);
            sql3.where.Add("type", 4);
            DataTable rs = this.ExecuteQuery(sql3.ToString());

            for (int x = 0; x < rs.Rows.Count; x++)
            {
                SqlBuilder sqlz = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.ENTRIES);
                sqlz.AddFieldValue(ColumnNames.COMPONENTS_ID, rs.Rows[x][ColumnNames.COMPONENTS_ID]);
                sqlz.AddFieldValue(ColumnNames.META_ENTRIES_ID, 37);
                this.ExecuteNonQuery(sqlz.ToString());
                int id = this.GetLastId();

                SqlBuilder sqly = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.VALUES);
                sqly.AddFieldValue(ColumnNames.ENTRIES_ID, id);
                sqly.AddFieldValue("value", "metreos");
                this.ExecuteNonQuery(sqly.ToString());
            }

        }

        private void AddLoggerDiagnostic()
        {
            // Add diagnostic config for Logger
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.META_ENTRIES);
            sql.AddFieldValue("name", "EnableLoggerQueueDiag");
            sql.AddFieldValue("display_name", "Enable Logger Queue Diagnostics");
            sql.AddFieldValue("description", "If enabled, queue size and object generation will be output in log messages");
            sql.AddFieldValue("component_type", 0);
            sql.AddFieldValue("mce_format_types_id", 2);
            this.ExecuteNonQuery(sql.ToString());
            int id = this.GetLastId();

            SqlBuilder sqlb = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.ENTRIES);
            sqlb.AddFieldValue(ColumnNames.COMPONENTS_ID, 6);
            sqlb.AddFieldValue(ColumnNames.META_ENTRIES_ID, id);
            this.ExecuteNonQuery(sqlb.ToString());
            id = this.GetLastId();

            SqlBuilder sqlc = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.VALUES);
            sqlc.AddFieldValue(ColumnNames.ENTRIES_ID, id);
            sqlc.AddFieldValue("value", "false");
            this.ExecuteNonQuery(sqlc.ToString());
        }

        #endregion


        #region Private Helper Methods

        private void AddDisplayName(string name, string display_name)
        {
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, TableNames.COMPONENTS);
            sql.AddFieldValue("display_name", display_name);
            sql.where.Add("name", name);
            this.ExecuteNonQuery(sql.ToString());
        }

        #endregion

    }

}
