using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Metreos.Utilities;
using Metreos.Utilities.DbBuilders;


namespace MigrationDefinitions
{
    /// <summary>
    /// Migration from DB version 2 (MCE 2.1.1 to 2.1.2)
    /// </summary>
    [MigrationCore.Version("2")]
    public class Version2 : MigrationCore.MigrationDefinition
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


        public Version2(Database.DbType dbtype, ref IDbConnection dbObj)
            : base(dbtype, ref dbObj)
        {
            this.ready = true;
            this.currentVersion = "2";
        }

        public override bool DoUpgrade()
        {
            this.CorrectMisnamedConfig();
            this.AddPreferredCodec();
            this.AddMediaConnectionType();
            this.AddCrgOrdinals();
            // No SCCP Proxy Provider?
            return true;
        }

        public override bool DoRollback()
        {
            throw new Exception("Roll back from this database version is not supported.");
        }


        #region Actions

        private void AddMediaConnectionType()
        {
            // Add ConnectionType to media servers
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.FORMAT_TYPES);
            sql.AddFieldValue(ColumnNames.COMPONENTS_ID, null);
            sql.AddFieldValue("name", "ConnectionType");
            this.ExecuteNonQuery(sql.ToString());
            int format_id = this.GetLastId();

            SqlBuilder asql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.META_ENTRIES);
            asql.AddFieldValue(ColumnNames.META_ENTRIES_ID, 38);
            asql.AddFieldValue("name", "MetreosReserved_ConnectionType");
            asql.AddFieldValue("display_name", "Connection Type");
            asql.AddFieldValue("description", "Connection method to the Media Server");
            asql.AddFieldValue("component_type", 4);
            asql.AddFieldValue(ColumnNames.FORMAT_TYPES_ID, format_id);
            this.ExecuteNonQuery(asql.ToString());
        }

        private void CorrectMisnamedConfig()
        {
            // Misnamed config from 2.1.0

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, TableNames.META_ENTRIES);
            sql.AddFieldValue("name", "MetreosReserved_DefaultTypeOfService");
            sql.where.Add(ColumnNames.META_ENTRIES_ID, 33);
            this.ExecuteNonQuery(sql.ToString());
        }

        private void AddPreferredCodec()
        {
            // Add preferred codec column to application partitions

            IDbColumnBuilder col = this.GetColumnBuilder("preferred_codec", ColType.VARCHAR).SetLength(128).SetDefault("").
                AddModifier(ColModifier.NOTNULL).PlaceAfterColumn("created_timestamp");

            IDbTableModifier tbl = this.GetTableModifier("mce_application_partitions").AddColumn(col);
            this.ExecuteNonQuery(tbl.ToString());
        }

        private void AddCrgOrdinals()
        {
            // Ordinal column & update call route group members
            IDbColumnBuilder col = this.GetColumnBuilder("ordinal", ColType.INT).SetLength(10).SetDefault("0").
                AddModifier(ColModifier.UNSIGNED).AddModifier(ColModifier.NOTNULL).PlaceAfterColumn(ColumnNames.COMPONENT_GROUPS_ID);

            IDbTableModifier tbl = this.GetTableModifier(TableNames.COMPONENT_GROUP_MEMBERS).AddColumn(col);
            this.ExecuteNonQuery(tbl.ToString());

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, TableNames.COMPONENT_GROUPS);
            sql.fieldNames.Add(ColumnNames.COMPONENT_GROUPS_ID);
            sql.AddCondition(new SqlBetweenCondition("component_type", 100, 150));
            DataTable rs = this.ExecuteQuery(sql.ToString());

            for (int x = 0; x < rs.Rows.Count; x++)
            {
                SqlBuilder sql2 = new SqlBuilder(SqlBuilder.Method.SELECT, TableNames.COMPONENT_GROUP_MEMBERS);
                sql2.fieldNames.Add(ColumnNames.COMPONENTS_ID);
                sql2.fieldNames.Add(ColumnNames.COMPONENT_GROUPS_ID);
                sql2.where.Add(ColumnNames.COMPONENT_GROUPS_ID, rs.Rows[x][ColumnNames.COMPONENT_GROUPS_ID]);
                sql2.order.Add("ordinal", "ASC");
                sql2.order.Add(ColumnNames.COMPONENTS_ID, "ASC");
                DataTable rs2 = this.ExecuteQuery(sql2.ToString());

                for (int y = 0; y < rs2.Rows.Count; y++)
                {
                    SqlBuilder sqlu = new SqlBuilder(SqlBuilder.Method.UPDATE, TableNames.COMPONENT_GROUP_MEMBERS);
                    sqlu.AddFieldValue("ordinal", y);
                    sqlu.where.Add(ColumnNames.COMPONENTS_ID, rs2.Rows[y][ColumnNames.COMPONENTS_ID]);
                    sqlu.where.Add(ColumnNames.COMPONENT_GROUPS_ID, rs2.Rows[y][ColumnNames.COMPONENT_GROUPS_ID]);
                    this.ExecuteNonQuery(sqlu.ToString());
                }
            }
        }

        #endregion

    }

}
