using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Metreos.Utilities;
using Metreos.Utilities.DbBuilders;


namespace MigrationDefinitions
{
    /// <summary>
    /// Migration from DB version 6 (MCE 2.2.0 to 2.2.1)
    /// </summary>
    [MigrationCore.Version("6")]
    public class Version6 : MigrationCore.MigrationDefinition
    {

        public Version6(Database.DbType dbtype, ref IDbConnection dbObj)
            : base(dbtype, ref dbObj)
        {
            this.ready = true;
            this.currentVersion = "6";
        }

        public override bool DoUpgrade()
        {
            // Add default preferred codec
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, "mce_application_partitions");
            sql.AddFieldValue("preferred_codec", "G.711u_20ms");
            sql.where.Add("preferred_codec", "");
            this.ExecuteNonQuery(sql.ToString());
            return true;
        }

        public override bool DoRollback()
        {
            throw new Exception("Roll back from this database version is not supported.");
        }

    }

}
