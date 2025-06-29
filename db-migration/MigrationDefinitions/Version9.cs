using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Metreos.Utilities;
using Metreos.Utilities.DbBuilders;


// Version 2.3.1 to 2.4.0 (Dev)

namespace MigrationDefinitions
{
    [MigrationCore.Version("9")]
    public class Version9 : MigrationCore.MigrationDefinition
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


        public Version9(Database.DbType dbtype, ref IDbConnection dbObj)
            : base(dbtype, ref dbObj)
        {
            this.ready = true;
            this.currentVersion = "9";
        }

        public override bool DoUpgrade()
        {
            this.UseUtf8();
            this.DropAlarmService();
            this.DropOldTables();
            this.AlterConfigValueType();

            this.AddAppShutdownTimeoutConfig();
            this.AddOidRootSysConfig();
            this.AddLicenseManager();
            this.AddNewServices();
            this.AddAppLocales();
            this.AddSnmpMibDefs();

            this.UpdateCoreComponentMetadata();
            this.UpdateServicesMetadata();

            return true;
        }

        public override bool DoRollback()
        {
            return true;
        }


        #region Actions

        private void UseUtf8()
        {
            // Switch existing tables to use UTF-8

            ArrayList tables = this.GetTableList();
            foreach (string tbl in tables)
            {
                this.ConvertTableToUtf8(tbl);
            }
        }

        private void DropOldTables()
        {
            // Kill these tables
            this.ExecuteNonQuery(this.GetTableModifier("mce_net_interfaces").Drop().ToString());
            this.ExecuteNonQuery(this.GetTableModifier("mce_net_interface_ips").Drop().ToString());
            this.ExecuteNonQuery(this.GetTableModifier("mce_net_interface_addresses").Drop().ToString());
            this.ExecuteNonQuery(this.GetTableModifier("mce_license").Drop().ToString());
        }

        private void DropAlarmService()
        {
            // Kill this service
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, "mce_services");
            sql.where.Add("name", "MetreosAlarmServer");
            this.ExecuteNonQuery(sql.ToString());
        }

        private void AlterConfigValueType()
        {
            // Make configs a text type
            IDbTableModifier tbl = this.GetTableModifier(TableNames.VALUES);
            tbl.ChangeColumn("value", this.GetColumnBuilder("value", ColType.TEXT).AddModifier(ColModifier.NOTNULL));
            this.ExecuteNonQuery(tbl.ToString());
        }

        private void AddOidRootSysConfig()
        {
            // Add oid system config
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_system_configs");
            sql.AddFieldValue("name", "oid_root");
            sql.AddFieldValue("value", "1.3.6.1.4.1.22720.1.");
            this.ExecuteNonQuery(sql.ToString());
        }

        private void AddNewServices()
        {
            // Add new controllable services
            this.AddService("MetreosJTAPIStack_CCM-5-1", "JTAPI Stack CCM-5-1", "JTAPI for CallManager 5.1");
            this.AddService("MetreosJTAPIStack_CCM-6-0", "JTAPI Stack CUCM-6-0", "JTAPI for Unified Communications Manager 6.0");
            this.AddService("CuaeStatsServer", "Stats Server", "Maintains usage statistics and sends out alarms");
            this.AddService("MetreosPresenceStack", "Presence Stack", "SIP Presence Stack");
        }

        private void AddAppShutdownTimeoutConfig()
        {
            // Remove Monster Interval/Add Shutdown TImeout config
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.META_ENTRIES);
            sql.AddFieldValue("name", "AppShutdownTimeout");
            sql.AddFieldValue("display_name", "Shutdown Timeout");
            sql.AddFieldValue("min_value", 5);
            sql.AddFieldValue("max_value", 120);
            sql.AddFieldValue("required", 1);
            sql.AddFieldValue("description", "Interval in seconds to wait for applications to shut down gracefully");
            sql.AddFieldValue(ColumnNames.FORMAT_TYPES_ID, 3);
            this.ExecuteNonQuery(sql.ToString());

            int meta_id = this.GetLastId();

            SqlBuilder asql = new SqlBuilder(SqlBuilder.Method.UPDATE, TableNames.ENTRIES);
            asql.AddFieldValue(ColumnNames.META_ENTRIES_ID, meta_id);
            asql.where.Add(ColumnNames.ENTRIES_ID, 10);
            this.ExecuteNonQuery(asql.ToString());

            SqlBuilder bsql = new SqlBuilder(SqlBuilder.Method.UPDATE, TableNames.VALUES);
            bsql.AddFieldValue("value", "30");
            bsql.where.Add(ColumnNames.ENTRIES_ID, 10);
            this.ExecuteNonQuery(bsql.ToString());

            SqlBuilder csql = new SqlBuilder(SqlBuilder.Method.DELETE, TableNames.META_ENTRIES);
            csql.where.Add("name", "MonsterInterval");
            csql.where.Add("display_name", "GC Interval");
            this.ExecuteNonQuery(csql.ToString());
        }

        private void AddLicenseManager()
        {
            // Add License Manager component
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.COMPONENTS);
            sql.AddFieldValue("name", "LicenseManager");
            sql.AddFieldValue("display_name", "License Manager");
            sql.AddFieldValue("type", 1);
            sql.AddFieldValue("status", 4);
            sql.AddFieldValue("version", "2.4.0");
            sql.AddFieldValue("copyright", "2005-2007 Cisco Systems, Inc.");
            sql.AddFieldValue("description", "License Manager");
            sql.AddFieldValue("author", "Cisco Systems, Inc.");
            sql.AddFieldValue("author_url", "http://www.cisco.com");
            sql.AddFieldValue("support_url", "http://www.cisco.com");
            this.ExecuteNonQuery(sql.ToString());

            int comp_id = this.GetLastId();
            
            SqlBuilder asql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.ENTRIES);
            asql.AddFieldValue(ColumnNames.COMPONENTS_ID, comp_id);
            asql.AddFieldValue(ColumnNames.META_ENTRIES_ID, 1);
            this.ExecuteNonQuery(asql.ToString());

            int entry_id = this.GetLastId();
            SqlBuilder bsql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.VALUES);
            bsql.AddFieldValue(ColumnNames.ENTRIES_ID, entry_id);
            bsql.AddFieldValue("value", "Warning");
            this.ExecuteNonQuery(bsql.ToString());
        }

        private void AddAppLocales()
        {
            // Add application partition field
            IDbTableModifier apppart = this.GetTableModifier("mce_application_partitions");
            apppart.AddColumn(this.GetColumnBuilder("locale", ColType.VARCHAR).SetLength(128).SetDefault("en-US").
                AddModifier(ColModifier.NOTNULL).PlaceAfterColumn("preferred_codec"));
            this.ExecuteNonQuery(apppart.ToString());

            // Add locale format
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.FORMAT_TYPES);
            sql.AddFieldValue("name", "Locale");
            this.ExecuteNonQuery(sql.ToString());

            int format_id = this.GetLastId();

            // Add AppMan meta config
            SqlBuilder asql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.META_ENTRIES);
            asql.AddFieldValue("name", "DefaultLocale");
            asql.AddFieldValue("display_name", "Default Locale");
            asql.AddFieldValue("description", "Locale which will be applied to all newly-installed applications by default");
            asql.AddFieldValue(ColumnNames.FORMAT_TYPES_ID, format_id);
            this.ExecuteNonQuery(asql.ToString());

            int meta_id = this.GetLastId();

            // Add AppMan config
            SqlBuilder aasql = new SqlBuilder(SqlBuilder.Method.SELECT, TableNames.COMPONENTS);
            aasql.fieldNames.Add(ColumnNames.COMPONENTS_ID);
            aasql.where.Add("name", "AppManager");
            aasql.where.Add("type", 1);
            DataTable rs = this.ExecuteQuery(aasql.ToString());

            uint appman_id = (uint) rs.Rows[0][ColumnNames.COMPONENTS_ID];

            SqlBuilder bsql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.ENTRIES);
            bsql.AddFieldValue(ColumnNames.META_ENTRIES_ID, meta_id);
            bsql.AddFieldValue(ColumnNames.COMPONENTS_ID, appman_id);
            this.ExecuteNonQuery(bsql.ToString());

            int config_id = this.GetLastId();

            SqlBuilder csql = new SqlBuilder(SqlBuilder.Method.INSERT, TableNames.VALUES);
            csql.AddFieldValue(ColumnNames.ENTRIES_ID, config_id);
            csql.AddFieldValue("value", "en-US");
            this.ExecuteNonQuery(csql.ToString());

            // Add format enums
            this.AddLocale(format_id, "af-ZA");
            this.AddLocale(format_id, "ar-AE");
            this.AddLocale(format_id, "ar-BH");
            this.AddLocale(format_id, "ar-DZ");
            this.AddLocale(format_id, "ar-EG");
            this.AddLocale(format_id, "ar-IQ");
            this.AddLocale(format_id, "ar-JO");
            this.AddLocale(format_id, "ar-KW");
            this.AddLocale(format_id, "ar-LB");
            this.AddLocale(format_id, "ar-LY");
            this.AddLocale(format_id, "ar-MA");
            this.AddLocale(format_id, "ar-OM");
            this.AddLocale(format_id, "ar-QA");
            this.AddLocale(format_id, "ar-SA");
            this.AddLocale(format_id, "ar-SY");
            this.AddLocale(format_id, "ar-TN");
            this.AddLocale(format_id, "ar-YE");
            this.AddLocale(format_id, "az-Cyrl-AZ");
            this.AddLocale(format_id, "az-Latn-AZ");
            this.AddLocale(format_id, "be-BY");
            this.AddLocale(format_id, "bg-BG");
            this.AddLocale(format_id, "bs-Latn-BA");
            this.AddLocale(format_id, "ca-ES");
            this.AddLocale(format_id, "cs-CZ");
            this.AddLocale(format_id, "cy-GB");
            this.AddLocale(format_id, "da-DK");
            this.AddLocale(format_id, "de-AT");
            this.AddLocale(format_id, "de-CH");
            this.AddLocale(format_id, "de-DE");
            this.AddLocale(format_id, "de-LI");
            this.AddLocale(format_id, "de-LU");
            this.AddLocale(format_id, "div-MV");
            this.AddLocale(format_id, "el-GR");
            this.AddLocale(format_id, "en-029");
            this.AddLocale(format_id, "en-AU");
            this.AddLocale(format_id, "en-BZ");
            this.AddLocale(format_id, "en-CA");
            this.AddLocale(format_id, "en-GB");
            this.AddLocale(format_id, "en-IE");
            this.AddLocale(format_id, "en-JM");
            this.AddLocale(format_id, "en-NZ");
            this.AddLocale(format_id, "en-PH");
            this.AddLocale(format_id, "en-TT");
            this.AddLocale(format_id, "en-US");
            this.AddLocale(format_id, "en-ZA");
            this.AddLocale(format_id, "en-ZW");
            this.AddLocale(format_id, "es-AR");
            this.AddLocale(format_id, "es-BO");
            this.AddLocale(format_id, "es-CL");
            this.AddLocale(format_id, "es-CO");
            this.AddLocale(format_id, "es-CR");
            this.AddLocale(format_id, "es-DO");
            this.AddLocale(format_id, "es-EC");
            this.AddLocale(format_id, "es-ES");
            this.AddLocale(format_id, "es-GT");
            this.AddLocale(format_id, "es-HN");
            this.AddLocale(format_id, "es-MX");
            this.AddLocale(format_id, "es-NI");
            this.AddLocale(format_id, "es-PA");
            this.AddLocale(format_id, "es-PE");
            this.AddLocale(format_id, "es-PR");
            this.AddLocale(format_id, "es-PY");
            this.AddLocale(format_id, "es-SV");
            this.AddLocale(format_id, "es-UY");
            this.AddLocale(format_id, "es-VE");
            this.AddLocale(format_id, "et-EE");
            this.AddLocale(format_id, "eu-ES");
            this.AddLocale(format_id, "fa-IR");
            this.AddLocale(format_id, "fi-FI");
            this.AddLocale(format_id, "fo-FO");
            this.AddLocale(format_id, "fr-BE");
            this.AddLocale(format_id, "fr-CA");
            this.AddLocale(format_id, "fr-CH");
            this.AddLocale(format_id, "fr-FR");
            this.AddLocale(format_id, "fr-LU");
            this.AddLocale(format_id, "fr-MC");
            this.AddLocale(format_id, "gl-ES");
            this.AddLocale(format_id, "gu-IN");
            this.AddLocale(format_id, "he-IL");
            this.AddLocale(format_id, "hi-IN");
            this.AddLocale(format_id, "hr-BA");
            this.AddLocale(format_id, "hr-HR");
            this.AddLocale(format_id, "hu-HU");
            this.AddLocale(format_id, "hy-AM");
            this.AddLocale(format_id, "id-ID");
            this.AddLocale(format_id, "is-IS");
            this.AddLocale(format_id, "it-CH");
            this.AddLocale(format_id, "it-IT");
            this.AddLocale(format_id, "ja-JP");
            this.AddLocale(format_id, "ka-GE");
            this.AddLocale(format_id, "kk-KZ");
            this.AddLocale(format_id, "kn-IN");
            this.AddLocale(format_id, "kok-IN");
            this.AddLocale(format_id, "ko-KR");
            this.AddLocale(format_id, "ky-KG");
            this.AddLocale(format_id, "lt-LT");
            this.AddLocale(format_id, "lv-LV");
            this.AddLocale(format_id, "mi-NZ");
            this.AddLocale(format_id, "mk-MK");
            this.AddLocale(format_id, "mn-MN");
            this.AddLocale(format_id, "mr-IN");
            this.AddLocale(format_id, "ms-BN");
            this.AddLocale(format_id, "ms-MY");
            this.AddLocale(format_id, "mt-MT");
            this.AddLocale(format_id, "nb-NO");
            this.AddLocale(format_id, "nl-BE");
            this.AddLocale(format_id, "nl-NL");
            this.AddLocale(format_id, "nn-NO");
            this.AddLocale(format_id, "ns-ZA");
            this.AddLocale(format_id, "pa-IN");
            this.AddLocale(format_id, "pl-PL");
            this.AddLocale(format_id, "pt-BR");
            this.AddLocale(format_id, "pt-PT");
            this.AddLocale(format_id, "quz-BO");
            this.AddLocale(format_id, "quz-EC");
            this.AddLocale(format_id, "quz-PE");
            this.AddLocale(format_id, "ro-RO");
            this.AddLocale(format_id, "ru-RU");
            this.AddLocale(format_id, "sa-IN");
            this.AddLocale(format_id, "se-FI");
            this.AddLocale(format_id, "se-NO");
            this.AddLocale(format_id, "se-SE");
            this.AddLocale(format_id, "sk-SK");
            this.AddLocale(format_id, "sl-SI");
            this.AddLocale(format_id, "sma-NO");
            this.AddLocale(format_id, "sma-SE");
            this.AddLocale(format_id, "smj-NO");
            this.AddLocale(format_id, "smj-SE");
            this.AddLocale(format_id, "smn-FI");
            this.AddLocale(format_id, "sms-FI");
            this.AddLocale(format_id, "sq-AL");
            this.AddLocale(format_id, "sr-Cyrl-BA");
            this.AddLocale(format_id, "sr-Cyrl-SP");
            this.AddLocale(format_id, "sr-Latn-BA");
            this.AddLocale(format_id, "sr-Latn-SP");
            this.AddLocale(format_id, "sv-FI");
            this.AddLocale(format_id, "sv-SE");
            this.AddLocale(format_id, "sw-KE");
            this.AddLocale(format_id, "syr-SY");
            this.AddLocale(format_id, "ta-IN");
            this.AddLocale(format_id, "te-IN");
            this.AddLocale(format_id, "th-TH");
            this.AddLocale(format_id, "tn-ZA");
            this.AddLocale(format_id, "tr-TR");
            this.AddLocale(format_id, "tt-RU");
            this.AddLocale(format_id, "uk-UA");
            this.AddLocale(format_id, "ur-PK");
            this.AddLocale(format_id, "uz-Cyrl-UZ");
            this.AddLocale(format_id, "uz-Latn-UZ");
            this.AddLocale(format_id, "vi-VN");
            this.AddLocale(format_id, "xh-ZA");
            this.AddLocale(format_id, "zh-CN");
            this.AddLocale(format_id, "zh-HK");
            this.AddLocale(format_id, "zh-MO");
            this.AddLocale(format_id, "zh-SG");
            this.AddLocale(format_id, "zh-TW");
            this.AddLocale(format_id, "zu-ZA");
        }

        private void AddSnmpMibDefs()
        {
            // Create SNMP Mib Table
            IDbTableBuilder tbl = this.GetTableBuilder("mce_snmp_mib_defs");
            tbl.Columns.Add(this.CreateIdColumn("mce_snmp_mib_defs_id"));
            tbl.Columns.Add(this.GetColumnBuilder("oid", ColType.VARCHAR).SetLength(64).AddModifier(ColModifier.NOTNULL));
            tbl.Columns.Add(this.GetColumnBuilder("type", ColType.INT).SetLength(10).SetDefault("0").AddModifier(ColModifier.NOTNULL));
            tbl.Columns.Add(this.GetColumnBuilder("name", ColType.VARCHAR).SetLength(64).AddModifier(ColModifier.NOTNULL));
            tbl.Columns.Add(this.GetColumnBuilder("description", ColType.VARCHAR).SetLength(256).AddModifier(ColModifier.NOTNULL));
            tbl.Columns.Add(this.GetColumnBuilder("data_type", ColType.INT).SetLength(10).SetDefault("0").AddModifier(ColModifier.NOTNULL));
            tbl.Columns.Add(this.GetColumnBuilder("ignore", ColType.TINYINT).SetLength(1).SetDefault("0").
                AddModifier(ColModifier.NOTNULL).AddModifier(ColModifier.UNSIGNED));
            tbl.PrimaryKey = "mce_snmp_mib_defs_id";
            this.ExecuteNonQuery(tbl.ToString());

            // Create definitions
            this.AddSnmpMibDef(1, 100, 1, "ServiceUnavailable", "A CUAE Service is not available.", 0, 0);
            this.AddSnmpMibDef(2, 101, 1, "MediaServerUnavailable", "A Media Server is not available.", 0, 0);
            this.AddSnmpMibDef(3, 102, 1, "OutOfMemory", "CUAE is running out of memory.", 0, 0);
            this.AddSnmpMibDef(4, 200, 1, "MsCompromised", "Media server compromised", 0, 0);
            this.AddSnmpMibDef(5, 201, 1, "MsUnexpectedCond", "Unexpected condition", 0, 0);
            this.AddSnmpMibDef(6, 202, 1, "MsErrorShutdown", "Media server unscheduled shutdown", 0, 0);
            this.AddSnmpMibDef(7, 203, 1, "MsNoResource", "Resource type not deployed on this server this.AddSnmpMibDef(e.g. no voice rec)", 0, 0);
            this.AddSnmpMibDef(8, 210, 1, "MsOutOfRTP", "Out of connections this.AddSnmpMibDef(G.711)", 0, 0);
            this.AddSnmpMibDef(9, 211, 1, "MsRtpHW", "High water connections", 0, 0);
            this.AddSnmpMibDef(10, 212, 1, "MsRtpLW", "Low water connections", 0, 0);
            this.AddSnmpMibDef(11, 220, 1, "MsOutOfVoice", "Out of voice", 0, 0);
            this.AddSnmpMibDef(12, 221, 1, "MsVoiceHW", "High water voice", 0, 0);
            this.AddSnmpMibDef(13, 222, 1, "MsVoiceLW", "Low water voice", 0, 0);
            this.AddSnmpMibDef(14, 230, 1, "MsOutOfErtp", "Out of low bitrate", 0, 0);
            this.AddSnmpMibDef(15, 231, 1, "MsErtpHW", "High water low bitrate", 0, 0);
            this.AddSnmpMibDef(16, 232, 1, "MsErtpLW", "Low water low bitrate", 0, 0);
            this.AddSnmpMibDef(17, 240, 1, "MsOutOfConf", "Out of conference resources for service instance", 0, 0);
            this.AddSnmpMibDef(18, 241, 1, "MsConfHW", "Conference resources for service instance high water", 0, 0);
            this.AddSnmpMibDef(19, 242, 1, "MsConfLW", "Conference resources for service instance low water", 0, 0);
            this.AddSnmpMibDef(20, 243, 1, "MsOutOfConfSlots", "Out of conference slots for conference", 0, 0);
            this.AddSnmpMibDef(21, 244, 1, "MsConfSlotsHW", "Conference slots for conference high water", 0, 0);
            this.AddSnmpMibDef(22, 245, 1, "MsConfSlotsLW", "Conference slots for conference low water", 0, 0);
            this.AddSnmpMibDef(23, 250, 1, "MsOutOfTts", "Out of TTS ports this.AddSnmpMibDef(request fails)", 0, 0);
            this.AddSnmpMibDef(24, 251, 1, "MsOutOfTtsQueued", "Out of TTS ports this.AddSnmpMibDef(request queues)", 0, 0);
            this.AddSnmpMibDef(25, 252, 1, "MsTtsHW", "High water TTS", 0, 0);
            this.AddSnmpMibDef(26, 253, 1, "MsTtsLW", "Low water TTS", 0, 0);
            this.AddSnmpMibDef(27, 260, 1, "MsOutOfVoiceRec", "Out of voice rec resources this.AddSnmpMibDef(request fails)", 0, 0);
            this.AddSnmpMibDef(28, 261, 1, "MsVoiceRecHW", "High water voice rec", 0, 0);
            this.AddSnmpMibDef(29, 262, 1, "MsVoiceRecLW", "Low water voice rec", 0, 0);
            this.AddSnmpMibDef(30, 300, 1, "ServerErrorShutdown", "CUAE Server shutdown unexpectedly", 0, 0);
            this.AddSnmpMibDef(31, 301, 1, "ServerStartupFailed", "CUAE Server failed to start", 0, 0);
            this.AddSnmpMibDef(32, 302, 1, "AppLoadFailed", "Application failed to load", 0, 0);
            this.AddSnmpMibDef(33, 303, 1, "ProviderLoadFailed", "Provider failed to load", 0, 0);
            this.AddSnmpMibDef(34, 304, 1, "AppReloaded", "Application reloaded due to failure", 0, 0);
            this.AddSnmpMibDef(35, 305, 1, "ProviderReloaded", "Provider reloaded due to failure", 0, 0);
            this.AddSnmpMibDef(36, 306, 1, "MediaDeployFailed", "Media deploy failure", 0, 0);
            this.AddSnmpMibDef(37, 310, 1, "AppSessionHW", "High water application sessions", 0, 0);
            this.AddSnmpMibDef(38, 311, 1, "AppSessionLW", "Low water application sessions", 0, 0);
            this.AddSnmpMibDef(39, 320, 1, "MgmtLoginFailure", "Management login failure", 0, 0);
            this.AddSnmpMibDef(40, 321, 1, "MgmtLoginSuccess", "Management login success", 0, 0);
            this.AddSnmpMibDef(41, 400, 1, "AppSessionsExceeded", "Number of licensed application sessions exceeded.", 0, 0);
            this.AddSnmpMibDef(42, 401, 1, "AppSessionsExceededFinal", "Number of licensed application sessions exceeded; licenses are now strictly enforced.", 0, 0);
            this.AddSnmpMibDef(43, 402, 1, "AppSessionDenied", "An attempt has been made to exceed the maximum number of licensed application sessions.", 0, 0);
            this.AddSnmpMibDef(44, 403, 1, "VoiceExceeded", "Number of licensed voice resources exceeded.", 0, 0);
            this.AddSnmpMibDef(45, 404, 1, "VoiceExceededFinal", "Number of licensed voice resources exceeded; licenses are now strictly enforced.", 0, 0);
            this.AddSnmpMibDef(46, 405, 1, "VoiceDenied", "An attempt has been made to exceed the maximum number of licensed voice resources.", 0, 0);
            this.AddSnmpMibDef(47, 406, 1, "RtpExceeded", "Number of licensed RTP resources exceeded.", 0, 0);
            this.AddSnmpMibDef(48, 407, 1, "RtpExceededFinal", "Number of licensed RTP resources exceeded; licenses are now strictly enforced.", 0, 0);
            this.AddSnmpMibDef(49, 408, 1, "RtpDenied", "An attempt has been made to exceed the maximum number of licensed RTP resources.", 0, 0);
            this.AddSnmpMibDef(50, 409, 1, "ErtpExceeded", "Number of licensed enhanced RTP resources exceeded.", 0, 0);
            this.AddSnmpMibDef(51, 410, 1, "ErtpExceededFinal", "Number of licensed enhanced RTP resources exceeded; licenses are now strictly enforced.", 0, 0);
            this.AddSnmpMibDef(52, 411, 1, "ErtpDenied", "An attempt has been made to exceed the maximum number of licensed enhanced RTP resources.", 0, 0);
            this.AddSnmpMibDef(53, 412, 1, "ConfExceeded", "Number of licensed conference resources exceeded.", 0, 0);
            this.AddSnmpMibDef(54, 413, 1, "ConfExceededFinal", "Number of licensed conference resources exceeded; licenses are now strictly enforced.", 0, 0);
            this.AddSnmpMibDef(55, 414, 1, "ConfDenied", "An attempt has been made to exceed the maximum number of licensed conference resources.", 0, 0);
            this.AddSnmpMibDef(56, 415, 1, "SpeechExceeded", "Number of licensed speech integration resources exceeded.", 0, 0);
            this.AddSnmpMibDef(57, 416, 1, "SpeechExceededFinal", "Number of licensed speech integration resources exceeded; licenses are now strictly enforced.", 0, 0);
            this.AddSnmpMibDef(58, 417, 1, "SpeechDenied", "An attempt has been made to exceed the maximum number of licensed speech integration resources.", 0, 0);
            this.AddSnmpMibDef(59, 418, 1, "TtsExceeded", "Number of licensed TTS resources exceeded.", 0, 0);
            this.AddSnmpMibDef(60, 419, 1, "TtsExceededFinal", "Number of licensed TTS resources exceeded; licenses are now strictly enforced.", 0, 0);
            this.AddSnmpMibDef(61, 420, 1, "TtsDenied", "An attempt has been made to exceed the maximum number of licensed TTS resources.", 0, 0);

            this.AddSnmpMibDef(1001, 1100, 1, "ClearedServiceUnavailable", "Alarm Cleared: A CUAE Service is not available.", 0, 0);
            this.AddSnmpMibDef(1002, 1101, 1, "ClearedMediaServerUnavailable", "Alarm Cleared: A Media Server is not available.", 0, 0);
            this.AddSnmpMibDef(1003, 1102, 1, "ClearedOutOfMemory", "Alarm Cleared: CUAE is running out of memory.", 0, 0);
            this.AddSnmpMibDef(1004, 1200, 1, "ClearedMsCompromised", "Alarm Cleared: Media server compromised", 0, 0);
            this.AddSnmpMibDef(1005, 1201, 1, "ClearedMsUnexpectedCond", "Alarm Cleared: Unexpected condition", 0, 0);
            this.AddSnmpMibDef(1006, 1202, 1, "ClearedMsErrorShutdown", "Alarm Cleared: Media server unscheduled shutdown", 0, 0);
            this.AddSnmpMibDef(1007, 1203, 1, "ClearedMsNoResource", "Alarm Cleared: Resource type not deployed on this server this.AddSnmpMibDef(e.g. no voice rec)", 0, 0);
            this.AddSnmpMibDef(1008, 1210, 1, "ClearedMsOutOfRTP", "Alarm Cleared: Out of connections this.AddSnmpMibDef(G.711)", 0, 0);
            this.AddSnmpMibDef(1009, 1211, 1, "ClearedMsRtpHW", "Alarm Cleared: High water connections", 0, 0);
            this.AddSnmpMibDef(1010, 1212, 1, "ClearedMsRtpLW", "Alarm Cleared: Low water connections", 0, 0);
            this.AddSnmpMibDef(1011, 1220, 1, "ClearedMsOutOfVoice", "Alarm Cleared: Out of voice", 0, 0);
            this.AddSnmpMibDef(1012, 1221, 1, "ClearedMsVoiceHW", "Alarm Cleared: High water voice", 0, 0);
            this.AddSnmpMibDef(1013, 1222, 1, "ClearedMsVoiceLW", "Alarm Cleared: Low water voice", 0, 0);
            this.AddSnmpMibDef(1014, 1230, 1, "ClearedMsOutOfErtp", "Alarm Cleared: Out of low bitrate", 0, 0);
            this.AddSnmpMibDef(1015, 1231, 1, "ClearedMsErtpHW", "Alarm Cleared: High water low bitrate", 0, 0);
            this.AddSnmpMibDef(1016, 1232, 1, "ClearedMsErtpLW", "Alarm Cleared: Low water low bitrate", 0, 0);
            this.AddSnmpMibDef(1017, 1240, 1, "ClearedMsOutOfConf", "Alarm Cleared: Out of conference resources for service instance", 0, 0);
            this.AddSnmpMibDef(1018, 1241, 1, "ClearedMsConfHW", "Alarm Cleared: Conference resources for service instance high water", 0, 0);
            this.AddSnmpMibDef(1019, 1242, 1, "ClearedMsConfLW", "Alarm Cleared: Conference resources for service instance low water", 0, 0);
            this.AddSnmpMibDef(1020, 1243, 1, "ClearedMsOutOfConfSlots", "Alarm Cleared: Out of conference slots for conference", 0, 0);
            this.AddSnmpMibDef(1021, 1244, 1, "ClearedMsConfSlotsHW", "Alarm Cleared: Conference slots for conference high water", 0, 0);
            this.AddSnmpMibDef(1022, 1245, 1, "ClearedMsConfSlotsLW", "Alarm Cleared: Conference slots for conference low water", 0, 0);
            this.AddSnmpMibDef(1023, 1250, 1, "ClearedMsOutOfTts", "Alarm Cleared: Out of TTS ports this.AddSnmpMibDef(request fails)", 0, 0);
            this.AddSnmpMibDef(1024, 1251, 1, "ClearedMsOutOfTtsQueued", "Alarm Cleared: Out of TTS ports this.AddSnmpMibDef(request queues)", 0, 0);
            this.AddSnmpMibDef(1025, 1252, 1, "ClearedMsTtsHW", "Alarm Cleared: High water TTS", 0, 0);
            this.AddSnmpMibDef(1026, 1253, 1, "ClearedMsTtsLW", "Alarm Cleared: Low water TTS", 0, 0);
            this.AddSnmpMibDef(1027, 1260, 1, "ClearedMsOutOfVoiceRec", "Alarm Cleared: Out of voice rec resources this.AddSnmpMibDef(request fails)", 0, 0);
            this.AddSnmpMibDef(1028, 1261, 1, "ClearedMsVoiceRecHW", "Alarm Cleared: High water voice rec", 0, 0);
            this.AddSnmpMibDef(1029, 1262, 1, "ClearedMsVoiceRecLW", "Alarm Cleared: Low water voice rec", 0, 0);
            this.AddSnmpMibDef(1030, 1300, 1, "ClearedServerErrorShutdown", "Alarm Cleared: CUAE Server shutdown unexpectedly", 0, 0);
            this.AddSnmpMibDef(1031, 1301, 1, "ClearedServerStartupFailed", "Alarm Cleared: CUAE Server failed to start", 0, 0);
            this.AddSnmpMibDef(1032, 1302, 1, "ClearedAppLoadFailed", "Alarm Cleared: Application failed to load", 0, 0);
            this.AddSnmpMibDef(1033, 1303, 1, "ClearedProviderLoadFailed", "Alarm Cleared: Provider failed to load", 0, 0);
            this.AddSnmpMibDef(1034, 1304, 1, "ClearedAppReloaded", "Alarm Cleared: Application reloaded due to failure", 0, 0);
            this.AddSnmpMibDef(1035, 1305, 1, "ClearedProviderReloaded", "Alarm Cleared: Provider reloaded due to failure", 0, 0);
            this.AddSnmpMibDef(1036, 1306, 1, "ClearedMediaDeployFailed", "Alarm Cleared: Media deploy failure", 0, 0);
            this.AddSnmpMibDef(1037, 1310, 1, "ClearedAppSessionHW", "Alarm Cleared: High water application sessions", 0, 0);
            this.AddSnmpMibDef(1038, 1311, 1, "ClearedAppSessionLW", "Alarm Cleared: Low water application sessions", 0, 0);
            this.AddSnmpMibDef(1039, 1320, 1, "ClearedMgmtLoginFailure", "Alarm Cleared: Management login failure", 0, 0);
            this.AddSnmpMibDef(1040, 1321, 1, "ClearedMgmtLoginSuccess", "Alarm Cleared: Management login success", 0, 0);
            this.AddSnmpMibDef(1041, 1400, 1, "ClearedAppSessionsExceeded", "Alarm Cleared: Number of licensed application sessions exceeded.", 0, 0);
            this.AddSnmpMibDef(1042, 1401, 1, "ClearedAppSessionsExceededFinal", "Alarm Cleared: Number of licensed application sessions exceeded; licenses are now strictly enforced.", 0, 0);
            this.AddSnmpMibDef(1043, 1402, 1, "ClearedAppSessionDenied", "Alarm Cleared: An attempt has been made to exceed the maximum number of licensed application sessions.", 0, 0);
            this.AddSnmpMibDef(1044, 1403, 1, "ClearedVoiceExceeded", "Alarm Cleared: Number of licensed voice resources exceeded.", 0, 0);
            this.AddSnmpMibDef(1045, 1404, 1, "ClearedVoiceExceededFinal", "Alarm Cleared: Number of licensed voice resources exceeded; licenses are now strictly enforced.", 0, 0);
            this.AddSnmpMibDef(1046, 1405, 1, "ClearedVoiceDenied", "Alarm Cleared: An attempt has been made to exceed the maximum number of licensed voice resources.", 0, 0);
            this.AddSnmpMibDef(1047, 1406, 1, "ClearedRtpExceeded", "Alarm Cleared: Number of licensed RTP resources exceeded.", 0, 0);
            this.AddSnmpMibDef(1048, 1407, 1, "ClearedRtpExceededFinal", "Alarm Cleared: Number of licensed RTP resources exceeded; licenses are now strictly enforced.", 0, 0);
            this.AddSnmpMibDef(1049, 1408, 1, "ClearedRtpDenied", "Alarm Cleared: An attempt has been made to exceed the maximum number of licensed RTP resources.", 0, 0);
            this.AddSnmpMibDef(1050, 1409, 1, "ClearedErtpExceeded", "Alarm Cleared: Number of licensed enhanced RTP resources exceeded.", 0, 0);
            this.AddSnmpMibDef(1051, 1410, 1, "ClearedErtpExceededFinal", "Alarm Cleared: Number of licensed enhanced RTP resources exceeded; licenses are now strictly enforced.", 0, 0);
            this.AddSnmpMibDef(1052, 1411, 1, "ClearedErtpDenied", "Alarm Cleared: An attempt has been made to exceed the maximum number of licensed enhanced RTP resources.", 0, 0);
            this.AddSnmpMibDef(1053, 1412, 1, "ClearedConfExceeded", "Alarm Cleared: Number of licensed conference resources exceeded.", 0, 0);
            this.AddSnmpMibDef(1054, 1413, 1, "ClearedConfExceededFinal", "Alarm Cleared: Number of licensed conference resources exceeded; licenses are now strictly enforced.", 0, 0);
            this.AddSnmpMibDef(1055, 1414, 1, "ClearedConfDenied", "Alarm Cleared: An attempt has been made to exceed the maximum number of licensed conference resources.", 0, 0);
            this.AddSnmpMibDef(1056, 1415, 1, "ClearedSpeechExceeded", "Alarm Cleared: Number of licensed speech integration resources exceeded.", 0, 0);
            this.AddSnmpMibDef(1057, 1416, 1, "ClearedSpeechExceededFinal", "Alarm Cleared: Number of licensed speech integration resources exceeded; licenses are now strictly enforced.", 0, 0);
            this.AddSnmpMibDef(1058, 1417, 1, "ClearedSpeechDenied", "Alarm Cleared: An attempt has been made to exceed the maximum number of licensed speech integration resources.", 0, 0);
            this.AddSnmpMibDef(1059, 1418, 1, "ClearedTtsExceeded", "Alarm Cleared: Number of licensed TTS resources exceeded.", 0, 0);
            this.AddSnmpMibDef(1060, 1419, 1, "ClearedTtsExceededFinal", "Alarm Cleared: Number of licensed TTS resources exceeded; licenses are now strictly enforced.", 0, 0);
            this.AddSnmpMibDef(1061, 1420, 1, "ClearedTtsDenied", "Alarm Cleared: An attempt has been made to exceed the maximum number of licensed TTS resources.", 0, 0);

            this.AddSnmpMibDef(2001, 2000, 2, "StatCPULoad", "CPU load %", 1, 0);
            this.AddSnmpMibDef(2002, 2001, 2, "StatServerMemory", "CUAE Server memory usage", 1, 0);
            this.AddSnmpMibDef(2003, 2002, 2, "StatMsMemory", "Media Engine memory usage", 1, 0);
            this.AddSnmpMibDef(2004, 2010, 2, "StatAppSessions", "Number of active application sessions", 1, 0);
            this.AddSnmpMibDef(2005, 2011, 2, "StatActiveCalls", "Number of active calls", 1, 0);
            this.AddSnmpMibDef(2006, 2020, 2, "StatRouterMsgs", "Router: Messages/sec", 1, 0);
            this.AddSnmpMibDef(2007, 2021, 2, "StatRouterEvents", "Router: Events/sec", 1, 0);
            this.AddSnmpMibDef(2008, 2022, 2, "StatRouterActions", "Router: Actions/sec", 1, 0);
            this.AddSnmpMibDef(2009, 2100, 2, "StatMsVoice", "Number of voice resources in use", 1, 0);
            this.AddSnmpMibDef(2010, 2101, 2, "StatMsRtp", "Number of RTP resources in use", 1, 0);
            this.AddSnmpMibDef(2011, 2102, 2, "StatMsErtp", "Number of enhanced RTP resources in use", 1, 0);
            this.AddSnmpMibDef(2012, 2103, 2, "StatMsConf", "Number of conference resources in use", 1, 0);
            this.AddSnmpMibDef(2013, 2104, 2, "StatMsSpeech", "Number of speech integration resources in use", 1, 0);
            this.AddSnmpMibDef(2014, 2105, 2, "StatMsTts", "Number of TTS resources in use", 1, 0);
        }

        private void UpdateCoreComponentMetadata()
        {
            // Update core component meta data
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, TableNames.COMPONENTS);
            sql.AddFieldValue("version", "2.4.0");
            sql.AddFieldValue("copyright", "2002-2007 Cisco Systems, Inc.");
            sql.AddFieldValue("author", "Cisco Systems, Inc.");
            sql.AddFieldValue("author_url", "http://www.cisco.com");
            sql.AddFieldValue("support_url", "http://www.cisco.com");
            sql.AddCondition(new SqlLogicalCondition(SqlLogicalCondition.LogicalOp.OR,
                new SqlCompareCondition("type", SqlCompareCondition.Comparator.EQUAL, 1),
                new SqlCompareCondition("type", SqlCompareCondition.Comparator.EQUAL, 5)));
            this.ExecuteNonQuery(sql.ToString());
        }

        private void UpdateServicesMetadata()
        {
            // Update service descriptions
            this.SetServiceMetadata("MetreosH323Stack", "H.323 Stack", "H.323 stack");
            this.SetServiceMetadata("MetreosLogServer", "Log Server", "Maintains logs of services");
            this.SetServiceMetadata("MetreosSipStack", "SIP Stack", "SIP Stack");
            this.SetServiceMetadata("SftpServerService", "SFTP Server", "Secure file transfer server");
            this.SetServiceMetadata("MetreosJTAPIStack_CCM-3-3", "JTAPI Stack CCM-3-3", "JTAPI for CallManager 3.3");
            this.SetServiceMetadata("MetreosJTAPIStack_CCM-4-0", "JTAPI Stack CCM-4-0", "JTAPI for CallManager 4.0");
            this.SetServiceMetadata("MetreosJTAPIStack_CCM-4-1", "JTAPI Stack CCM-4-1", "JTAPI for CallManager 4.1");
            this.SetServiceMetadata("MetreosJTAPIStack_CCM-4-2", "JTAPI Stack CCM-4-2", "JTAPI for CallManager 4.2");
            this.SetServiceMetadata("MetreosJTAPIStack_CCM-5-0", "JTAPI Stack CCM-5-0", "JTAPI for CallManager 5.0");
        }

        #endregion


        #region Helper Methods

        private void ConvertTableToUtf8(string table)
        {
            this.ExecuteNonQuery(new StringBuilder().AppendFormat("ALTER TABLE {0} CONVERT TO CHARACTER SET utf8", table).ToString());
        }

        private void AddService(string name, string display_name, string description)
        {
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_services");
            sql.AddFieldValue("name", name);
            sql.AddFieldValue("display_name", display_name);
            sql.AddFieldValue("description", description);
            this.ExecuteNonQuery(sql.ToString());
        }

        private void SetServiceMetadata(string name, string display_name, string description)
        {
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, "mce_services");
            sql.AddFieldValue("display_name", display_name);
            sql.AddFieldValue("description", description);
            sql.where.Add("name", name);
            this.ExecuteNonQuery(sql.ToString());
        }

        private void AddLocale(int format_id, string locale)
        {
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_format_type_enum_values");
            sql.AddFieldValue(ColumnNames.FORMAT_TYPES_ID, format_id);
            sql.AddFieldValue("value", locale);
            this.ExecuteNonQuery(sql.ToString());
        }

        private void AddSnmpMibDef(int id, int oid, int type, string name, string description, int data_type, int ignore)
        {
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_snmp_mib_defs");
            sql.AddFieldValue("mce_snmp_mib_defs_id", id);
            sql.AddFieldValue("oid", oid);
            sql.AddFieldValue("type", type);
            sql.AddFieldValue("name", name);
            sql.AddFieldValue("description", description);
            sql.AddFieldValue("data_type", data_type);
            sql.AddFieldValue("`ignore`", ignore);
            this.ExecuteNonQuery(sql.ToString());
        }

        #endregion

    }

}
