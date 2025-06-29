using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Metreos.Utilities;
using Metreos.Utilities.DbBuilders;


namespace MigrationDefinitions
{
    /// <summary>
    /// Migration from DB version 4 (MCE 2.1.3 to 2.1.4)
    /// </summary>
    [MigrationCore.Version("4")]
    public class Version4 : MigrationCore.MigrationDefinition
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


        public Version4(Database.DbType dbtype, ref IDbConnection dbObj)
            : base(dbtype, ref dbObj)
        {
            this.ready = true;
            this.currentVersion = "4";
        }

        public override bool DoUpgrade()
        {
            this.AddPrintDiagExtension("Router");
            this.AddPrintDiagExtension("AppManager");
            this.AddPrintDiagExtension("ApplicationEnvironment");
            this.AddPrintDiagExtension("ProviderManager");

            this.AddComponentExtension("TelephonyManager", "ClearCrgCache", "Clears the call route group cache");
            this.AddComponentExtension("TelephonyManager", "ClearCallTable", "Clears the call table");
            this.AddComponentExtension("TelephonyManager", "ClearCrgCache", "Terminates all calls in the call table gracefully");
            this.AddComponentExtension("TelephonyManager", "EndAllCalls", "Prints diagnostic information to logging");
            this.AddPrintDiagExtension("TelephonyManager");

            this.AddOutputDiagCallTable();
            return true;
        }

        public override bool DoRollback()
        {
            throw new Exception("Roll back from this database version is not supported.");
        }


        #region Actions

        private void AddOutputDiagCallTable()
        {
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.META_ENTRIES);
            sql.AddFieldValue("name", "OutputDiagnosticCallTable");
            sql.AddFieldValue("display_Name", "Enable Output Call Table Diagnostic");
            sql.AddFieldValue("description", "If enabled, the call manager will occasionally output diagnostics about its call table");
            sql.AddFieldValue("mce_format_types_id", 2);
            this.ExecuteNonQuery(sql.ToString());
            int meta_id = this.GetLastId();

            SqlBuilder subsql = new SqlBuilder(SqlBuilder.Method.SELECT, TableNames.COMPONENTS);
            subsql.fieldNames.Add(ColumnNames.COMPONENTS_ID);
            subsql.where.Add("name", "TelephonyManager");
            subsql.where.Add("type", 1);

            SqlBuilder asql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.ENTRIES);
            asql.AddFieldValue(ColumnNames.COMPONENTS_ID, this.MakeSubquery(subsql));
            asql.AddFieldValue(ColumnNames.META_ENTRIES_ID, meta_id);
            this.ExecuteNonQuery(asql.ToString());
            int entry_id = this.GetLastId();

            SqlBuilder bsql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.VALUES);
            bsql.AddFieldValue(ColumnNames.ENTRIES_ID, entry_id);
            bsql.AddFieldValue("value", "false");
            this.ExecuteNonQuery(bsql.ToString());
        }

        #endregion


        #region Private Helpers

        private void AddComponentExtension(string component, string extension, string description)
        {
            SqlBuilder subsql = new SqlBuilder(SqlBuilder.Method.SELECT, TableNames.COMPONENTS);
            subsql.fieldNames.Add(ColumnNames.COMPONENTS_ID);
            subsql.where.Add("name", component);
            subsql.where.Add("type", 1);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_provider_extensions");
            sql.AddFieldValue("name", extension);
            sql.AddFieldValue("description", description);
            sql.AddFieldValue("wait_for_completion", "0");
            sql.AddFieldValue(ColumnNames.COMPONENTS_ID, this.MakeSubquery(subsql));
            this.ExecuteNonQuery(sql.ToString());
        }

        private void AddPrintDiagExtension(string component)
        {
            this.AddComponentExtension(component, "PrintDiags", "Prints diagnostic information to logging");
        }

        #endregion

    }

}
