using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Metreos.Utilities;
using Metreos.Utilities.DbBuilders;


namespace MigrationDefinitions
{
    /// <summary>
    /// Migration from DB version 3 (MCE 2.1.2 to 2.1.3)
    /// </summary>
    [MigrationCore.Version("3")]
    public class Version3 : MigrationCore.MigrationDefinition
    {

        public Version3(Database.DbType dbtype, ref IDbConnection dbObj)
            : base(dbtype, ref dbObj)
        {
            this.ready = true;
            this.currentVersion = "3";
        }

        public override bool DoUpgrade()
        {
            this.AddHasMedia();
            this.AddConnectionTypeEntires();
            return true;
        }

        public override bool DoRollback()
        {
            throw new Exception("Roll back from this database version is not supported.");
        }


        #region Actions

        private void AddHasMedia()
        {
            // Add HasMedia for media servers

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_config_entry_metas");
            sql.AddFieldValue("mce_config_entry_metas_id", 39);
            sql.AddFieldValue("name", "HasMedia");
            sql.AddFieldValue("display_Name", "Has Media");
            sql.AddFieldValue("required", 0);
            sql.AddFieldValue("description", "Indicates that media has been provisioned to this server");
            sql.AddFieldValue("component_type", 4);
            sql.AddFieldValue("mce_format_types_id", 2);
            this.ExecuteNonQuery(sql.ToString());
        }

        private void AddConnectionTypeEntires()
        {
            // Add ConnectionType entires to pre-existing media servers
            int meta_id = 38;

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, "mce_components");
            sql.fieldNames.Add("mce_components_id");
            sql.where.Add("type", 4);
            DataTable rs = this.ExecuteQuery(sql.ToString());

            for (int x = 0; x < rs.Rows.Count; x++)
            {
                uint msid = (uint) rs.Rows[x]["mce_components_id"];

                SqlBuilder asql = new SqlBuilder(SqlBuilder.Method.SELECT, "mce_config_entries");
                asql.fieldNames.Add("mce_config_entries_id");
                asql.where.Add("mce_components_id", msid);
                asql.where.Add("mce_config_entry_metas_id", meta_id);
                DataTable rs2 = this.ExecuteQuery(asql.ToString());

                if (rs2.Rows.Count == 0)
                {
                    SqlBuilder bsql = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_config_entries");
                    bsql.AddFieldValue("mce_components_id", msid);
                    bsql.AddFieldValue("mce_config_entry_metas_id", meta_id);
                    this.ExecuteNonQuery(bsql.ToString());

                    int entry_id = this.GetLastId();

                    SqlBuilder csql = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_config_values");
                    csql.AddFieldValue("mce_config_entries_id", entry_id);
                    csql.AddFieldValue("value", "IPC");
                }
            }
        }

        #endregion

    }

}
