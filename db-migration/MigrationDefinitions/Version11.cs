using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Metreos.Utilities;
using Metreos.Utilities.DbBuilders;


// Add secure connection to SMTP Managers

namespace MigrationDefinitions
{
    [MigrationCore.Version("11")]
    public class Version11 : MigrationCore.MigrationDefinition
    {

        private const int CONFIG_META_ID = 54;

        public Version11(Database.DbType dbtype, ref IDbConnection dbObj)
            : base(dbtype, ref dbObj)
        {
            this.ready = true;
            this.currentVersion = "11";
        }

        public override bool DoUpgrade()
        {
            // Add config meta

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_config_entry_metas");
            sql.AddFieldValue("mce_config_entry_metas_id", CONFIG_META_ID);
            sql.AddFieldValue("name", "MetreosReserved_SecureConnection");
            sql.AddFieldValue("display_name", "Secure Connnection");
            sql.AddFieldValue("description", "Connect to SMTP Server securely");
            sql.AddFieldValue("component_type", 50);
            sql.AddFieldValue("mce_format_types_id", 2);
            this.ExecuteNonQuery(sql.ToString());

            // If an existing SMTP manager exists, add an entry
            SqlBuilder findsmtp = new SqlBuilder(SqlBuilder.Method.SELECT, "mce_components");
            findsmtp.where.Add("type", 50);
            DataTable rs = this.ExecuteQuery(findsmtp.ToString());

            if (rs.Rows.Count > 0)
            {
                uint id = (uint) rs.Rows[0]["mce_components_id"];

                SqlBuilder entrysql = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_config_entries");
                entrysql.AddFieldValue("mce_components_id", id);
                entrysql.AddFieldValue("mce_config_entry_metas_id", CONFIG_META_ID);
                this.ExecuteNonQuery(entrysql.ToString());

                int entry_id = this.GetLastId();

                SqlBuilder valuesql = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_config_values");
                valuesql.AddFieldValue("mce_config_entries_id", entry_id);
                valuesql.AddFieldValue("value", "false");
                this.ExecuteNonQuery(valuesql.ToString());
            }

            return true;
        }

        public override bool DoRollback()
        {
            // Delete entry
            SqlBuilder findsmtp = new SqlBuilder(SqlBuilder.Method.SELECT, "mce_components");
            findsmtp.where.Add("type", 50);
            DataTable rs = this.ExecuteQuery(findsmtp.ToString());

            if (rs.Rows.Count > 0)
            {
                uint id = (uint) rs.Rows[0]["mce_components_id"];

                SqlBuilder find_entry = new SqlBuilder(SqlBuilder.Method.SELECT, "mce_config_entries");
                find_entry.where.Add("mce_components_id", id);
                find_entry.where.Add("mce_config_entry_metas_id", CONFIG_META_ID);
                DataTable entry_rs = this.ExecuteQuery(find_entry.ToString());
                uint entry_id = (uint) entry_rs.Rows[0]["mce_config_entries_id"];

                SqlBuilder del_values = new SqlBuilder(SqlBuilder.Method.DELETE, "mce_config_values");
                del_values.where.Add("mce_config_entries_id", entry_id);
                this.ExecuteNonQuery(del_values.ToString());

                SqlBuilder del_entry = new SqlBuilder(SqlBuilder.Method.DELETE, "mce_config_entries");
                del_entry.where.Add("mce_config_entries_id", entry_id);
                this.ExecuteNonQuery(del_entry.ToString());
            }

            // Delete config meta
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, "mce_config_entry_metas");
            sql.where.Add("mce_config_entry_metas_id", 54);
            this.ExecuteNonQuery(sql.ToString());
            return true;
        }
    }
}