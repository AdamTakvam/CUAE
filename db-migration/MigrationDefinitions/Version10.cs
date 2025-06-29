using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Metreos.Utilities;
using Metreos.Utilities.DbBuilders;


// Add new SNMP MIBs

namespace MigrationDefinitions
{
    [MigrationCore.Version("10")]
    public class Version10 : MigrationCore.MigrationDefinition
    {

        public Version10(Database.DbType dbtype, ref IDbConnection dbObj)
            : base(dbtype, ref dbObj)
        {
            this.ready = true;
            this.currentVersion = "10";
        }

        public override bool DoUpgrade()
        {
            this.AddSnmpMibDefs();
            this.UpdateSnmpMibDefs();
            return true;
        }

        public override bool DoRollback()
        {
            this.DeleteSnmpMibDefs();
            return true;
        }


        #region Actions


        private void AddSnmpMibDefs()
        {
            this.AddSnmpMibDef(246, 1, "MsOutOfConf", "Out of conferences", 0, 0);
            this.AddSnmpMibDef(247, 1, "MsConfHW", "Conferences high water", 0, 0);
            this.AddSnmpMibDef(248, 1, "MsConfLW", "Conferences low water", 0, 0);

            this.AddSnmpMibDef(1246, 1, "ClearedMsOutOfConf", "Alarm Cleared: Out of conferences", 0, 0);
            this.AddSnmpMibDef(1247, 1, "ClearedMsConfHW", "Alarm Cleared: Conferences high water", 0, 0);
            this.AddSnmpMibDef(1248, 1, "ClearedMsConfLW", "Alarm Cleared: Conferences low water", 0, 0);
            
            this.AddSnmpMibDef(2106, 2, "StatMsConfSlots", "Number of conference slots in use", 1, 0);
            this.AddSnmpMibDef(2107, 2, "StatMsConf", "Number of conferences in use", 1, 0);
        }

        private void UpdateSnmpMibDefs()
        {
            this.UpdateSnmpMibDef(240, 1, "MsOutOfConfRes", "Out of conference resources for service instance", 0, 0);
            this.UpdateSnmpMibDef(241, 1, "MsConfResHW", "Conference resources for service instance high water", 0, 0);
            this.UpdateSnmpMibDef(242, 1, "MsConfResLW", "Conference resources for service instance low water", 0, 0);

            this.UpdateSnmpMibDef(1240, 1, "ClearedMsOutOfConfRes", "Alarm Cleared: Out of conference resources for service instance", 0, 0);
            this.UpdateSnmpMibDef(1241, 1, "ClearedMsConfResHW", "Alarm Cleared: Conference resources for service instance high water", 0, 0);
            this.UpdateSnmpMibDef(1242, 1, "ClearedMsConfResLW", "Alarm Cleared: Conference resources for service instance low water", 0, 0);

            this.UpdateSnmpMibDef(2103, 2, "StatMsConfRes", "Number of conference resources in use", 1, 0);
        }

        private void DeleteSnmpMibDefs()
        {
            this.DeleteSnmpMibDef(246);
            this.DeleteSnmpMibDef(247);
            this.DeleteSnmpMibDef(248);

            this.DeleteSnmpMibDef(1246);
            this.DeleteSnmpMibDef(1247);
            this.DeleteSnmpMibDef(1248);

            this.DeleteSnmpMibDef(2106);
            this.DeleteSnmpMibDef(2107);
        }

        #endregion


        #region Helper Methods

        private void AddSnmpMibDef(int oid, int type, string name, string description, int data_type, int ignore)
        {
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_snmp_mib_defs");
            sql.AddFieldValue("oid", oid);
            sql.AddFieldValue("type", type);
            sql.AddFieldValue("name", name);
            sql.AddFieldValue("description", description);
            sql.AddFieldValue("data_type", data_type);
            sql.AddFieldValue("`ignore`", ignore);
            this.ExecuteNonQuery(sql.ToString());
        }

        private void UpdateSnmpMibDef(int oid, int type, string name, string description, int data_type, int ignore)
        {
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, "mce_snmp_mib_defs");
            sql.where.Add("oid", oid);
            sql.AddFieldValue("type", type);
            sql.AddFieldValue("name", name);
            sql.AddFieldValue("description", description);
            sql.AddFieldValue("data_type", data_type);
            sql.AddFieldValue("`ignore`", ignore);
            this.ExecuteNonQuery(sql.ToString());
        }

        private void DeleteSnmpMibDef(int oid)
        {
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, "mce_snmp_mib_defs");
            sql.where.Add("oid", oid);
            this.ExecuteNonQuery(sql.ToString());
        }

        #endregion

    }

}
