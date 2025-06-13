using System;
using System.Text;
using System.Data;
using System.Diagnostics;

using Metreos.Utilities;

namespace DbTest.ConfigDb
{
    #region Components
    public abstract class Components
    {
        public static int Insert(IDbConnection db, string name, IConfig.ComponentType type, 
            IConfig.Status status, string version)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            if(name == null) { return 0; }
            if(type == IConfig.ComponentType.Unspecified) { return 0; }
            if(status == IConfig.Status.Unspecified) { return 0; }

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, IConfig.Tables.COMPONENTS);
            sql.AddFieldValue(IConfig.Fields.NAME, name);
            sql.AddFieldValue(IConfig.Fields.TYPE, ((int)type).ToString());     // e.g. "1"
            sql.AddFieldValue(IConfig.Fields.STATUS, ((int)status).ToString());
            sql.AddFieldValue(IConfig.Fields.VERSION, version != null ? version : "1.0");

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static DataTable Select(IDbConnection db, string name, IConfig.ComponentType type)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, IConfig.Tables.COMPONENTS);
            if(name != null)                                sql.where.Add(IConfig.Fields.NAME, name);
            if(type != IConfig.ComponentType.Unspecified)   sql.where.Add(IConfig.Fields.TYPE, ((int)type).ToString());

            return Utilities.ExecuteQuery(db, sql.ToString());
        }

        public static int Update(IDbConnection db, uint id, string name, IConfig.ComponentType type,
            IConfig.Status status, string version)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, IConfig.Tables.COMPONENTS);
            if(name != null)                                sql.AddFieldValue(IConfig.Fields.NAME, name);
            if(type != IConfig.ComponentType.Unspecified)   sql.AddFieldValue(IConfig.Fields.TYPE, ((int)type).ToString());
            if(status != IConfig.Status.Unspecified)        sql.AddFieldValue(IConfig.Fields.STATUS, ((int)status).ToString());
            if(version != null)                             sql.AddFieldValue(IConfig.Fields.VERSION, version);

            sql.where.Add(IConfig.Keys.COMPONENTS, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static int Delete(IDbConnection db, uint id)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, IConfig.Tables.COMPONENTS);
            sql.where.Add(IConfig.Keys.COMPONENTS, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }
    }
    #endregion

    #region MetaData
    public abstract class MetaData
    {
        public static int Insert(IDbConnection db, uint componentsId, string author, string copyright, 
            ulong createdTS, string author_url, string support_url, string description)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, IConfig.Tables.METADATA);
            sql.AddFieldValue(IConfig.Keys.COMPONENTS, componentsId.ToString());
            sql.AddFieldValue(IConfig.Fields.CREATED_TS, createdTS.ToString());
            if(author != null)      sql.AddFieldValue(IConfig.Fields.AUTHOR, author);
            if(copyright != null)   sql.AddFieldValue(IConfig.Fields.COPYRIGHT, copyright);
            if(author_url != null)  sql.AddFieldValue(IConfig.Fields.AUTHOR_URL, author_url);
            if(support_url != null)  sql.AddFieldValue(IConfig.Fields.SUPPORT_URL, support_url);
            if(description != null) sql.AddFieldValue(IConfig.Fields.DESCRIPTION, description);

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static DataTable Select(IDbConnection db, int componentsId)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, IConfig.Tables.METADATA);
            if(componentsId != -1)  sql.where.Add(IConfig.Keys.COMPONENTS, componentsId.ToString());

            return Utilities.ExecuteQuery(db, sql.ToString());
        }

        public static int Update(IDbConnection db, uint id, string author, string copyright, 
            string author_url, string support_url, string description)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, IConfig.Tables.METADATA);
            if(author != null)      sql.AddFieldValue(IConfig.Fields.AUTHOR, author);
            if(copyright != null)   sql.AddFieldValue(IConfig.Fields.COPYRIGHT, copyright);
            if(author_url != null)  sql.AddFieldValue(IConfig.Fields.AUTHOR_URL, author_url);
            if(support_url != null)  sql.AddFieldValue(IConfig.Fields.SUPPORT_URL, support_url);
            if(description != null) sql.AddFieldValue(IConfig.Fields.DESCRIPTION, description);

            sql.where.Add(IConfig.Keys.METADATA, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static int Delete(IDbConnection db, uint id)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, IConfig.Tables.METADATA);
            sql.where.Add(IConfig.Keys.METADATA, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }
    }
    #endregion

    #region ComponentGroups
    public abstract class ComponentGroups
    {
        public static int Insert(IDbConnection db, int alarmGroupId, int failoverGroupId, int componentType,
            string name, string description)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, IConfig.Tables.COMPONENT_GROUPS);
            if(failoverGroupId != -1)   sql.AddFieldValue(IConfig.Fields.FAILOVER_GROUP_ID, failoverGroupId.ToString());
            if(alarmGroupId != -1)      sql.AddFieldValue(IConfig.Fields.ALARM_GROUP_ID, alarmGroupId.ToString());
            if(componentType != -1)     sql.AddFieldValue(IConfig.Fields.COMPONENT_TYPE, componentType.ToString());
            if(description != null)     sql.AddFieldValue(IConfig.Fields.DESCRIPTION, description);
            if(name != null)            sql.AddFieldValue(IConfig.Fields.NAME, name);

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static DataTable Select(IDbConnection db, int id)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, IConfig.Tables.COMPONENT_GROUPS);
            if(id != -1)    sql.where.Add(IConfig.Keys.COMPONENT_GROUPS, id.ToString());

            return Utilities.ExecuteQuery(db, sql.ToString());
        }

        public static int Update(IDbConnection db, uint id, int failoverGroupId, int alarmGroupId, 
            string name, string description)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, IConfig.Tables.COMPONENT_GROUPS);
            if(failoverGroupId != -1)   sql.AddFieldValue(IConfig.Fields.FAILOVER_GROUP_ID, failoverGroupId.ToString());
            if(alarmGroupId != -1)      sql.AddFieldValue(IConfig.Fields.ALARM_GROUP_ID, alarmGroupId.ToString());
            if(name != null)            sql.AddFieldValue(IConfig.Fields.NAME, name);
            if(description != null)     sql.AddFieldValue(IConfig.Fields.DESCRIPTION, description);

            sql.where.Add(IConfig.Keys.COMPONENT_GROUPS, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static int Delete(IDbConnection db, uint id)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, IConfig.Tables.COMPONENT_GROUPS);
            sql.where.Add(IConfig.Keys.COMPONENT_GROUPS, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }
    }
    #endregion

    #region ComponentGroupMembers
    public abstract class ComponentGroupMembers
    {
        public static int Insert(IDbConnection db, uint componentsId, uint componentGroupsId)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, IConfig.Tables.COMPONENT_GRP_MEMBERS);
            sql.AddFieldValue(IConfig.Keys.COMPONENTS, componentsId.ToString());
            sql.AddFieldValue(IConfig.Keys.COMPONENT_GROUPS, componentGroupsId.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static DataTable Select(IDbConnection db, int componentsId, int componentGroupsId)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, IConfig.Tables.COMPONENT_GRP_MEMBERS);
            if(componentsId != -1)      sql.where.Add(IConfig.Keys.COMPONENTS, componentsId.ToString());
            if(componentGroupsId != -1) sql.where.Add(IConfig.Keys.COMPONENT_GROUPS, componentGroupsId.ToString());

            return Utilities.ExecuteQuery(db, sql.ToString());
        }

        public static int Delete(IDbConnection db, int componentsId, int componentGroupsId)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, IConfig.Tables.COMPONENT_GRP_MEMBERS);
            if(componentsId != -1)      sql.where.Add(IConfig.Keys.COMPONENTS, componentsId.ToString());
            if(componentGroupsId != -1) sql.where.Add(IConfig.Keys.COMPONENT_GROUPS, componentGroupsId.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }
    }
    #endregion

    #region ConfigEntries
    public abstract class ConfigEntries
    {
        public static int Insert(IDbConnection db, uint conifgEntriesMetasId, int componentsId, 
            int componentGroupsId, int appPartitionsId)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            if((componentsId == -1) && (componentGroupsId == -1) && (appPartitionsId == -1)) { return 0; }

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, IConfig.Tables.CONFIG_ENTRIES);
            sql.AddFieldValue(IConfig.Keys.CONFIG_ENTRY_METAS, conifgEntriesMetasId.ToString());
            if(componentsId != -1)      sql.AddFieldValue(IConfig.Keys.COMPONENTS, componentsId.ToString());
            if(componentGroupsId != -1) sql.AddFieldValue(IConfig.Keys.COMPONENT_GROUPS, componentGroupsId.ToString());
            if(appPartitionsId != -1)   sql.AddFieldValue(IConfig.Keys.APP_PARTITIONS, appPartitionsId.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static DataTable Select(IDbConnection db, int componentsId, 
            int componentGroupsId, int appPartitionsId)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, IConfig.Tables.CONFIG_ENTRIES);
            if(componentsId != -1)      sql.where.Add(IConfig.Keys.COMPONENTS, componentsId.ToString());
            if(appPartitionsId != -1)   sql.where.Add(IConfig.Keys.APP_PARTITIONS, appPartitionsId.ToString());
            if(componentGroupsId != -1) sql.where.Add(IConfig.Keys.COMPONENT_GROUPS, componentGroupsId.ToString());

            return Utilities.ExecuteQuery(db, sql.ToString());
        }

        public static int Delete(IDbConnection db, uint id)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, IConfig.Tables.CONFIG_ENTRIES);
            sql.where.Add(IConfig.Keys.CONFIG_ENTRIES, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }
    }
    #endregion

    #region ConfigValues
    public abstract class ConfigValues
    {
        public static int Insert(IDbConnection db, uint configEntriesId, string Value, int ordinal, string key)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            if(Value == null) { return 0; }

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, IConfig.Tables.CONFIG_VALUES);
            sql.AddFieldValue(IConfig.Keys.CONFIG_ENTRIES, configEntriesId.ToString());
            sql.AddFieldValue(IConfig.Fields.VALUE, Value);
            if(ordinal != -1)   sql.AddFieldValue(IConfig.Fields.ORDINAL, ordinal.ToString());
            if(key != null)     sql.AddFieldValue(IConfig.Fields.KEY, key);

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static DataTable Select(IDbConnection db, int configEntriesId)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, IConfig.Tables.CONFIG_VALUES);
            if(configEntriesId != -1)   sql.where.Add(IConfig.Keys.CONFIG_ENTRIES, configEntriesId.ToString());

            return Utilities.ExecuteQuery(db, sql.ToString());
        }

        public static int Update(IDbConnection db, uint id, string Value, int ordinal, string key)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, IConfig.Tables.CONFIG_VALUES);
            if(ordinal != -1)   sql.AddFieldValue(IConfig.Fields.ORDINAL, ordinal.ToString());
            if(key != null)     sql.AddFieldValue(IConfig.Fields.KEY, key);
            if(Value != null)     sql.AddFieldValue(IConfig.Fields.VALUE, Value);

            sql.where.Add(IConfig.Keys.CONFIG_VALUES, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static int Delete(IDbConnection db, uint id)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, IConfig.Tables.CONFIG_VALUES);
            sql.where.Add(IConfig.Keys.CONFIG_VALUES, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }
    }
    #endregion

    #region ConfigEntryMetas
    public abstract class ConfigEntryMetas
    {
        public static int Insert(IDbConnection db, bool readOnly, uint formatTypesId, string name, 
            IConfig.ComponentType componentType, string displayName, string description, int minValue, 
            bool minValueSpecified, int maxValue, bool maxValueSpecified)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            if(name == null) { return 0; }

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, IConfig.Tables.CONFIG_ENTRY_METAS);
            sql.AddFieldValue(IConfig.Fields.READ_ONLY, readOnly ? "1" : "0");
            sql.AddFieldValue(IConfig.Keys.FORMAT_TYPES, formatTypesId.ToString());
            sql.AddFieldValue(IConfig.Fields.NAME, name);
            if(displayName != null) sql.AddFieldValue(IConfig.Fields.DISPLAY_NAME, displayName);
            if(description != null) sql.AddFieldValue(IConfig.Fields.DESCRIPTION, description);
            if(minValueSpecified)   sql.AddFieldValue(IConfig.Fields.MIN_VALUE, minValue.ToString());
            if(maxValueSpecified)   sql.AddFieldValue(IConfig.Fields.MAX_VALUE, maxValue.ToString());
            if(componentType != IConfig.ComponentType.Unspecified)
                                    sql.AddFieldValue(IConfig.Fields.COMPONENT_TYPE, ((int)componentType).ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static DataTable Select(IDbConnection db, int id, IConfig.ComponentType componentType)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, IConfig.Tables.CONFIG_ENTRY_METAS);
            if(id != -1)            sql.where.Add(IConfig.Keys.CONFIG_ENTRY_METAS, id.ToString());
            if(componentType != IConfig.ComponentType.Unspecified) 
                                    sql.where.Add(IConfig.Fields.COMPONENT_TYPE, ((int)componentType).ToString());

            return Utilities.ExecuteQuery(db, sql.ToString());
        }

        public static int Update(IDbConnection db, uint id, int formatTypesId, string name, 
            int componentType, string displayName, string description, int minValue, bool minValueSpecified, 
            int maxValue, bool maxValueSpecified, bool readOnly, bool readOnlySpecified)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, IConfig.Tables.CONFIG_ENTRY_METAS);
            if(readOnlySpecified)   sql.AddFieldValue(IConfig.Fields.READ_ONLY, readOnly ? "1" : "0");
            if(formatTypesId != -1) sql.AddFieldValue(IConfig.Keys.FORMAT_TYPES, formatTypesId.ToString());
            if(name != null)        sql.AddFieldValue(IConfig.Fields.NAME, name);
            if(componentType != -1) sql.AddFieldValue(IConfig.Fields.COMPONENT_TYPE, componentType.ToString());
            if(displayName != null) sql.AddFieldValue(IConfig.Fields.DISPLAY_NAME, displayName);
            if(description != null) sql.AddFieldValue(IConfig.Fields.DESCRIPTION, description);
            if(minValueSpecified)   sql.AddFieldValue(IConfig.Fields.MIN_VALUE, minValue.ToString());
            if(maxValueSpecified)   sql.AddFieldValue(IConfig.Fields.MAX_VALUE, maxValue.ToString());

            sql.where.Add(IConfig.Keys.CONFIG_ENTRY_METAS, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static int Delete(IDbConnection db, uint id)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, IConfig.Tables.CONFIG_ENTRY_METAS);
            sql.where.Add(IConfig.Keys.CONFIG_ENTRY_METAS, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }
    }
    #endregion

    #region FormatTypes
    public abstract class FormatTypes
    {
        public static int Insert(IDbConnection db, string name, string description, int componentsId)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            if(name == null) { return 0; }

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, IConfig.Tables.FORMAT_TYPES);
            sql.AddFieldValue(IConfig.Fields.NAME, name);
            if(componentsId != -1)  sql.AddFieldValue(IConfig.Keys.COMPONENTS, componentsId.ToString());
            if(description != null) sql.AddFieldValue(IConfig.Fields.DESCRIPTION, description);

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static DataTable Select(IDbConnection db, int id, string name)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, IConfig.Tables.FORMAT_TYPES);
            if(id != -1)        sql.where.Add(IConfig.Keys.FORMAT_TYPES, id.ToString());
            if(name != null)    sql.where.Add(IConfig.Fields.NAME, name);

            return Utilities.ExecuteQuery(db, sql.ToString());
        }

        public static int Delete(IDbConnection db, uint id)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, IConfig.Tables.FORMAT_TYPES);
            sql.where.Add(IConfig.Keys.FORMAT_TYPES, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }
    }
    #endregion

    #region FormatTypeEnumValues
    public abstract class FormatTypeEnumValues
    {
        public static int Insert(IDbConnection db, uint formatTypesId, string Value)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            if(Value == null) { return 0; }

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, IConfig.Tables.FORMAT_TYPE_ENUM_VALS);
            sql.AddFieldValue(IConfig.Keys.FORMAT_TYPES, formatTypesId.ToString());
            sql.AddFieldValue(IConfig.Fields.VALUE, Value);

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static DataTable Select(IDbConnection db, int formatTypesId)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, IConfig.Tables.FORMAT_TYPE_ENUM_VALS);
            if(formatTypesId != -1) sql.where.Add(IConfig.Keys.FORMAT_TYPES, formatTypesId.ToString());

            return Utilities.ExecuteQuery(db, sql.ToString());
        }

        public static int Delete(IDbConnection db, uint id)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, IConfig.Tables.FORMAT_TYPE_ENUM_VALS);
            sql.where.Add(IConfig.Keys.FORMAT_TYPE_ENUM_VALS, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }
    }
    #endregion

    #region AppScripts
    public abstract class AppScripts
    {
        public static int Insert(IDbConnection db, uint componentsId, string name, string eventType, 
            bool usesCC, bool usesMedia)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            if(name == null) { return 0; }
            if(eventType == null) { return 0; }

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, IConfig.Tables.APP_SCRIPTS);
            sql.AddFieldValue(IConfig.Keys.COMPONENTS, componentsId.ToString());
            sql.AddFieldValue(IConfig.Fields.NAME, name);
            sql.AddFieldValue(IConfig.Fields.EVENT_TYPE, eventType);
            sql.AddFieldValue(IConfig.Fields.USES_CALL_CONTROL, usesCC ? "1" : "0");
            sql.AddFieldValue(IConfig.Fields.USES_MEDIA, usesMedia ? "1" : "0");

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static DataTable Select(IDbConnection db, int componentsId, string name)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, IConfig.Tables.APP_SCRIPTS);
            if(componentsId != -1)  sql.where.Add(IConfig.Keys.COMPONENTS, componentsId.ToString());
            if(name != null)        sql.where.Add(IConfig.Fields.NAME, name);

            return Utilities.ExecuteQuery(db, sql.ToString());
        }

        public static int Delete(IDbConnection db, uint id)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, IConfig.Tables.APP_SCRIPTS);
            sql.where.Add(IConfig.Keys.APP_SCRIPTS, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }
    }
    #endregion

    #region AppScriptTriggerParams
    public abstract class AppScriptTriggerParams
    {
        public static int Insert(IDbConnection db, string name, string Value, IConfig.TriggerParamsType type, 
            int appScriptsId, int appPartitionsId)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            if((name == null) || (Value == null) || (type == IConfig.TriggerParamsType.Unspecified)) { return 0; }
            if((appScriptsId == -1) && (appPartitionsId == -1)) { return 0; }

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, IConfig.Tables.APP_SCRIPT_TRIG_PARAMS);
            sql.AddFieldValue(IConfig.Fields.NAME, name);
            sql.AddFieldValue(IConfig.Fields.VALUE, Value);
            sql.AddFieldValue(IConfig.Fields.TYPE, ((int)type).ToString());
            if(appScriptsId != -1)      sql.AddFieldValue(IConfig.Keys.APP_SCRIPTS, appScriptsId.ToString());
            if(appPartitionsId != -1)   sql.AddFieldValue(IConfig.Keys.APP_PARTITIONS, appPartitionsId.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static DataTable Select(IDbConnection db, int appScriptsId, int appPartitionsId)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, IConfig.Tables.APP_SCRIPT_TRIG_PARAMS);
            if(appScriptsId != -1)      sql.where.Add(IConfig.Keys.APP_SCRIPTS, appScriptsId.ToString());
            if(appPartitionsId != -1)   sql.where.Add(IConfig.Keys.APP_PARTITIONS, appPartitionsId.ToString());

            return Utilities.ExecuteQuery(db, sql.ToString());
        }

        public static int Update(IDbConnection db, uint id, string name, string Value, IConfig.TriggerParamsType type)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, IConfig.Tables.APP_SCRIPT_TRIG_PARAMS);
            if(name != null)    sql.AddFieldValue(IConfig.Fields.NAME, name);
            if(Value != null)   sql.AddFieldValue(IConfig.Fields.VALUE, Value);
            if(type != IConfig.TriggerParamsType.Unspecified)
                                sql.AddFieldValue(IConfig.Fields.TYPE, ((int)type).ToString());

            sql.where.Add(IConfig.Keys.APP_SCRIPT_TRIG_PARAMS, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static int Delete(IDbConnection db, uint id)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, IConfig.Tables.APP_SCRIPT_TRIG_PARAMS);
            sql.where.Add(IConfig.Keys.APP_SCRIPT_TRIG_PARAMS, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }
    }
    #endregion

    #region AppPartitions
    public abstract class AppPartitions
    {
        public static int Insert(IDbConnection db, uint componentsId, string name, ulong createdTS, bool enabled, 
            uint alarmGroupId, int callRouteGroupId, int mediaResouceGroupId, string description)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            if(name == null) { return 0; }

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, IConfig.Tables.APP_PARTITIONS);
            sql.AddFieldValue(IConfig.Fields.NAME, name);
            sql.AddFieldValue(IConfig.Fields.CREATED_TS, createdTS.ToString());
            sql.AddFieldValue(IConfig.Fields.ENABLED, enabled ? "1" : "0");
            sql.AddFieldValue(IConfig.Keys.COMPONENTS, componentsId.ToString());
            sql.AddFieldValue(IConfig.Fields.ALARM_GROUP_ID, alarmGroupId.ToString());
            if(callRouteGroupId != -1)      sql.AddFieldValue(IConfig.Fields.CALL_GROUP_ID, callRouteGroupId.ToString());
            if(mediaResouceGroupId != -1)   sql.AddFieldValue(IConfig.Fields.MEDIA_GROUP_ID, mediaResouceGroupId.ToString());
            if(description != null)         sql.AddFieldValue(IConfig.Fields.DESCRIPTION, description);

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static DataTable Select(IDbConnection db, int componentsId)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, IConfig.Tables.APP_PARTITIONS);
            if(componentsId != -1)  sql.where.Add(IConfig.Keys.COMPONENTS, componentsId.ToString());

            return Utilities.ExecuteQuery(db, sql.ToString());
        }

        public static int Update(IDbConnection db, uint id, string name, bool enabled, bool enabledSpecified,
            int alarmGroupId, int callRouteGroupId, int mediaResouceGroupId, string description)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, IConfig.Tables.APP_PARTITIONS);
            if(name != null)                sql.AddFieldValue(IConfig.Fields.NAME, name);
            if(enabledSpecified)            sql.AddFieldValue(IConfig.Fields.ENABLED, enabled ? "1" : "0");
            if(alarmGroupId != -1)          sql.AddFieldValue(IConfig.Fields.ALARM_GROUP_ID, alarmGroupId.ToString());
            if(callRouteGroupId != -1)      sql.AddFieldValue(IConfig.Fields.CALL_GROUP_ID, callRouteGroupId.ToString());
            if(mediaResouceGroupId != -1)   sql.AddFieldValue(IConfig.Fields.MEDIA_GROUP_ID, mediaResouceGroupId.ToString());
            if(description != null)         sql.AddFieldValue(IConfig.Fields.DESCRIPTION, description);

            sql.where.Add(IConfig.Keys.APP_PARTITIONS, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static int Delete(IDbConnection db, uint id)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, IConfig.Tables.APP_PARTITIONS);
            sql.where.Add(IConfig.Keys.APP_PARTITIONS, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }
    }
    #endregion

    #region Users
    public abstract class Users
    {
        public static int Insert(IDbConnection db, string username, uint creatorId, ulong createdTS, 
            IConfig.AccessLevel accessLevel, string password)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            if((username == null) || (accessLevel == IConfig.AccessLevel.Unspecified)) { return 0; }

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, IConfig.Tables.USERS);
            sql.AddFieldValue(IConfig.Fields.USERNAME, username);
            sql.AddFieldValue(IConfig.Fields.CREATED_TS, createdTS.ToString());
            sql.AddFieldValue(IConfig.Fields.CREATOR_ID, creatorId.ToString());
            sql.AddFieldValue(IConfig.Fields.ACCESS_LEVEL, ((int)accessLevel).ToString());
            if(password != null)    sql.AddFieldValue(IConfig.Fields.PASSWORD, password);

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static DataTable Select(IDbConnection db, string username, IConfig.AccessLevel accessLevel)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, IConfig.Tables.USERS);
            if(username != null)    sql.where.Add(IConfig.Fields.USERNAME, username);
            if(accessLevel != IConfig.AccessLevel.Unspecified)
                                    sql.where.Add(IConfig.Fields.ACCESS_LEVEL, ((int)accessLevel).ToString());

            return Utilities.ExecuteQuery(db, sql.ToString());
        }

        public static int Update(IDbConnection db, uint id, IConfig.AccessLevel accessLevel, string password)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.UPDATE, IConfig.Tables.USERS);
            if(accessLevel != IConfig.AccessLevel.Unspecified)
                                    sql.AddFieldValue(IConfig.Fields.ACCESS_LEVEL, ((int)accessLevel).ToString());
            if(password != null)    sql.AddFieldValue(IConfig.Fields.PASSWORD, password);

            sql.where.Add(IConfig.Keys.USERS, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static int Delete(IDbConnection db, uint id)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, IConfig.Tables.USERS);
            sql.where.Add(IConfig.Keys.USERS, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }
    }
    #endregion

    #region UsersAclList
    public abstract class UsersAclList
    {
        public static int Insert(IDbConnection db, uint usersId, uint componentGroupsId)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, IConfig.Tables.USERS_ACL_LIST);
            sql.AddFieldValue(IConfig.Keys.USERS, usersId.ToString());
            sql.AddFieldValue(IConfig.Keys.COMPONENT_GROUPS, componentGroupsId.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static DataTable Select(IDbConnection db, int usersId, int componentGroupsId)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, IConfig.Tables.USERS_ACL_LIST);
            if(usersId != -1)           sql.where.Add(IConfig.Keys.USERS, usersId.ToString());
            if(componentGroupsId != -1) sql.where.Add(IConfig.Keys.COMPONENT_GROUPS, componentGroupsId.ToString());

            return Utilities.ExecuteQuery(db, sql.ToString());
        }

        public static int Delete(IDbConnection db, int usersId, int componentGroupsId)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, IConfig.Tables.USERS_ACL_LIST);
            if(usersId != -1)           sql.where.Add(IConfig.Keys.USERS, usersId.ToString());
            if(componentGroupsId != -1) sql.where.Add(IConfig.Keys.COMPONENT_GROUPS, componentGroupsId.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }
    }
    #endregion

    #region ProviderExtensions
    public abstract class ProviderExtensions
    {
        public static int Insert(IDbConnection db, uint componentsId, string name, bool sync, string description)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            if(name == null) { return 0; }

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, IConfig.Tables.PROV_EXTS);
            sql.AddFieldValue(IConfig.Keys.COMPONENTS, componentsId.ToString());
            sql.AddFieldValue(IConfig.Fields.NAME, name);
            sql.AddFieldValue(IConfig.Fields.WAIT_COMPLETION, sync ? "1" : "0");
            if(description != null) sql.AddFieldValue(IConfig.Fields.DESCRIPTION, description);

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static DataTable Select(IDbConnection db, int componentsId)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, IConfig.Tables.PROV_EXTS);
            if(componentsId != -1)  sql.where.Add(IConfig.Keys.COMPONENTS, componentsId.ToString());

            return Utilities.ExecuteQuery(db, sql.ToString());
        }

        public static int Delete(IDbConnection db, uint id)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, IConfig.Tables.PROV_EXTS);
            sql.where.Add(IConfig.Keys.PROV_EXTS, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }
    }
    #endregion

    #region ProviderExtensionParams
    public abstract class ProviderExtensionParams
    {
        public static int Insert(IDbConnection db, uint provExtsId, uint formatTypesId, string name, string description)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            if(name == null) { return 0; }

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, IConfig.Tables.PROV_EXT_PARAMS);
            sql.AddFieldValue(IConfig.Keys.PROV_EXTS, provExtsId.ToString());
            sql.AddFieldValue(IConfig.Keys.FORMAT_TYPES, formatTypesId.ToString());
            sql.AddFieldValue(IConfig.Fields.NAME, name);
            if(description != null) sql.AddFieldValue(IConfig.Fields.DESCRIPTION, description);

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static DataTable Select(IDbConnection db, int provExtsId)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, IConfig.Tables.PROV_EXT_PARAMS);
            if(provExtsId != -1)    sql.where.Add(IConfig.Keys.PROV_EXTS, provExtsId.ToString());

            return Utilities.ExecuteQuery(db, sql.ToString());
        }

        public static int Delete(IDbConnection db, uint id)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, IConfig.Tables.PROV_EXT_PARAMS);
            sql.where.Add(IConfig.Keys.PROV_EXT_PARAMS, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }
    }
    #endregion

    #region EventLog
    public abstract class EventLog
    {
        public static int Insert(IDbConnection db, IConfig.LogMsgType type, uint messageId, 
            ulong createdTS, string description)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            if(type == IConfig.LogMsgType.Unspecified) { return 0; }

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, IConfig.Tables.EVENT_LOG);
            sql.AddFieldValue(IConfig.Fields.TYPE, ((int)type).ToString());
            sql.AddFieldValue(IConfig.Fields.MESSAGE_ID, messageId.ToString());
            sql.AddFieldValue(IConfig.Fields.CREATED_TS, createdTS.ToString());
            if(description != null) sql.AddFieldValue(IConfig.Fields.DESCRIPTION, description);

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static DataTable Select(IDbConnection db, IConfig.LogMsgType type)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, IConfig.Tables.EVENT_LOG);
            if(type != IConfig.LogMsgType.Unspecified)  
                                    sql.where.Add(IConfig.Fields.TYPE, ((int)type).ToString());

            return Utilities.ExecuteQuery(db, sql.ToString());
        }

        public static int Delete(IDbConnection db, uint id)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, IConfig.Tables.EVENT_LOG);
            sql.where.Add(IConfig.Keys.EVENT_LOG, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }
    }
    #endregion

    #region License
    public abstract class License
    {
        public static int Insert(IDbConnection db, string data, ulong createdTS, bool active)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            if(data == null) { return 0; }

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, IConfig.Tables.LICENSE);
            sql.AddFieldValue(IConfig.Fields.LICENSE_DATA, data);
            sql.AddFieldValue(IConfig.Fields.ACTIVE, active ? "1" : "0");
            sql.AddFieldValue(IConfig.Fields.CREATED_TS, createdTS.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }

        public static DataTable Select(IDbConnection db, bool active, bool activeSpecified)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, IConfig.Tables.LICENSE);
            if(activeSpecified) sql.where.Add(IConfig.Fields.ACTIVE, active ? "1" : "0");

            return Utilities.ExecuteQuery(db, sql.ToString());
        }

        public static int Delete(IDbConnection db, uint id)
        {
            Debug.Assert(db != null, "Database connection is null");
            Debug.Assert(db.State == ConnectionState.Open, "Database in inaccessible state: " + db.State);

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.DELETE, IConfig.Tables.LICENSE);
            sql.where.Add(IConfig.Keys.LICENSE, id.ToString());

            return Utilities.ExecuteCommand(db, sql.ToString());
        }
    }
    #endregion

    #region -- Private Utilities
    internal abstract class Utilities
    {
        public static int ExecuteCommand(IDbConnection db, string sqlCommand)
        {
            lock(db)
            {
                using(IDbCommand command = db.CreateCommand())
                {
                    command.CommandText = sqlCommand;
                    return command.ExecuteNonQuery();
                }
            }
        }

        public static DataTable ExecuteQuery(IDbConnection db, string sqlQuery)
        {
            lock(db)
            {
                using(IDbCommand command = db.CreateCommand())
                {
                    command.CommandText = sqlQuery;

                    using(IDataReader reader = command.ExecuteReader())
                    {
                        return Database.GetDataTable(reader);
                    }
                }
            }
        }
    }
    #endregion
}
