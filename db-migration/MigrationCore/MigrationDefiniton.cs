using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Metreos.Utilities;
using Metreos.Utilities.DbBuilders;

namespace MigrationCore
{

    /// <summary>
    /// MigrationDefinition is the base class from which the migration definitions classes inherit.
    /// It provides the core initialization actions and some helper methods...
    /// </summary>
    public abstract class MigrationDefinition : IMigrationDefinition
    {

        #region Members

        protected bool ready;
        protected IDbConnection dbObj;
        protected IDbTransaction transObj;
        protected string currentVersion;
        protected string nextVersion;
        protected LogWriter logger;

        private Database.DbType dbType;

        #endregion


        #region Properties

        public string CurrentVersion
        {
            get
            {
                return this.currentVersion;
            }
        }

        public string NextVersion
        {
            get
            {
                if (this.nextVersion == null)
                {
                    try
                    {
                        char[] delimiters = { '.' };
                        string[] versionBits = this.currentVersion.Split(delimiters);
                        switch (versionBits.Length)
                        {
                            case 1:
                                int majorVersion = Convert.ToInt32(versionBits[0]);
                                majorVersion++;
                                this.nextVersion = majorVersion.ToString();
                                break;
                            case 2:
                                int minorVersion = Convert.ToInt32(versionBits[1]);
                                minorVersion++;
                                this.nextVersion = versionBits[0] + "." + minorVersion.ToString();
                                break;
                            default:
                                break;
                        }
                    }
                    catch
                    {
                        this.nextVersion = null;
                    }
                }

                return this.nextVersion;
            }
        }

        public IDbTransaction Transaction
        {
            set
            {
                this.transObj = value;
            }
        }

        public LogWriter Logger
        {
            set
            {
                this.logger = value;
            }
        }

        #endregion


        #region Methods

        protected MigrationDefinition(Database.DbType type, ref IDbConnection dbObj)
        {
            this.ready = false;
            this.dbObj = dbObj;
            this.transObj = null;
            this.currentVersion = null;
            this.nextVersion = null;
            this.dbType = type;
            this.logger = null;
        }

        protected MigrationDefinition(ref IDbConnection dbObj)
            : this(Database.DbType.mysql, ref dbObj)
        { }

        public bool IsReady()
        {
            return this.ready;
        }

        public abstract bool DoUpgrade();

        public abstract bool DoRollback();

        #endregion


        #region DbBuilder Retrievers

        protected IDbColumnBuilder GetColumnBuilder(string name, ColType colType)
        {
            switch (this.dbType)
            {
                case Database.DbType.mysql:
                    return new MysqlColumnBuilder(name, colType);
                default:
                    throw new Exception("Database type not yet supported");
            }
        }

        protected IDbTableBuilder GetTableBuilder(string name)
        {
            switch (this.dbType)
            {
                case Database.DbType.mysql:
                    return new MysqlTableBuilder(name);
                default:
                    throw new Exception("Database type not yet supported");
            }
        }

        protected IDbTableModifier GetTableModifier(string name)
        {
            switch (this.dbType)
            {
                case Database.DbType.mysql:
                    return new MysqlTableModifier(name);
                default:
                    throw new Exception("Database type not yet supported");
            }
        }

        #endregion


        #region DbBuilder Helpers

        protected IDbColumnBuilder CreateIdColumn(string name)
        {
            IDbColumnBuilder id = this.GetColumnBuilder(name, ColType.INT).SetLength(10);
            id.AddModifier(ColModifier.UNSIGNED);
            id.AddModifier(ColModifier.NOTNULL);
            id.AddModifier(ColModifier.AUTOINCREMENT);
            return id;
        }

        protected SqlBuilder.PreformattedValue MakeSubquery(SqlBuilder sqlObj)
        {
            string sql = sqlObj.ToString();
            sql = sql.Remove(sql.LastIndexOf(';'),1);
            return new SqlBuilder.PreformattedValue(new StringBuilder().AppendFormat("({0})", sql).ToString());
        }

        protected void DropComponent(string name, int type)
        {
            SqlBuilder comp_sql = new SqlBuilder(SqlBuilder.Method.SELECT, Database.Tables.COMPONENTS);
            comp_sql.fieldNames.Add(Database.Keys.COMPONENTS);
            comp_sql.where.Add(Database.Fields.NAME, name);
            comp_sql.where.Add(Database.Fields.TYPE, type);
            DataTable rs = this.ExecuteQuery(comp_sql.ToString());

            if (rs.Rows.Count > 0)
            {
                uint comp_id = (uint)rs.Rows[0][Database.Keys.COMPONENTS];

                SqlBuilder ent_sql = new SqlBuilder(SqlBuilder.Method.SELECT, Database.Tables.CONFIG_ENTRIES);
                ent_sql.where.Add(Database.Keys.COMPONENTS, comp_id);
                DataTable entries = this.ExecuteQuery(ent_sql.ToString());

                for (int x = 0; x < entries.Rows.Count; x++)
                {
                    SqlBuilder delvalues = new SqlBuilder(SqlBuilder.Method.DELETE, Database.Tables.CONFIG_VALUES);
                    delvalues.where.Add(Database.Keys.CONFIG_ENTRIES, entries.Rows[x][Database.Keys.CONFIG_ENTRIES]);
                    this.ExecuteNonQuery(delvalues.ToString());

                    SqlBuilder delmetas = new SqlBuilder(SqlBuilder.Method.DELETE, Database.Tables.CONFIG_ENTRY_METAS);
                    delmetas.where.Add(Database.Keys.CONFIG_ENTRY_METAS, entries.Rows[x][Database.Keys.CONFIG_ENTRY_METAS]);
                    delmetas.where.Add(Database.Fields.COMPONENT_TYPE, 0);
                    this.ExecuteNonQuery(delmetas.ToString());
                }

                SqlBuilder delentries = new SqlBuilder(SqlBuilder.Method.DELETE, Database.Tables.CONFIG_ENTRIES);
                delentries.where.Add(Database.Keys.COMPONENTS, comp_id);
                this.ExecuteNonQuery(delentries.ToString());

                SqlBuilder delcomponent = new SqlBuilder(SqlBuilder.Method.DELETE, Database.Tables.COMPONENTS);
                delcomponent.where.Add(Database.Keys.COMPONENTS, comp_id);
                this.ExecuteNonQuery(delcomponent.ToString());
            }

        }

        protected ArrayList GetTableList()
        {
            ArrayList tables = new ArrayList();
            DataTable rs = this.ExecuteQuery("SHOW TABLES");
            for (int x = 0; x < rs.Rows.Count; x++)
            {
                tables.Add(rs.Rows[x][0]);
            }
            return tables;
        }

        #endregion


        #region SQL Execution Methods

        protected int ExecuteNonQuery(string sql)
        {
            int rowsAffected;

            lock (this.dbObj)
            {
                try
                {
                    using (IDbCommand command = this.dbObj.CreateCommand())
                    {
                        command.Transaction = this.transObj;
                        command.CommandText = sql;
                        rowsAffected = command.ExecuteNonQuery();
                        this.logger.WriteLine(LogLevels.VERBOSE, "[  EXECUTE  ] {0}", sql);
                        return rowsAffected;
                    }
                }
                catch
                {
                    this.logger.WriteLine("[--FAILURE--] {0}", sql);
                    throw;
                }
            }
        }

        protected DataTable ExecuteQuery(string sql)
        {
            lock (this.dbObj)
            {
                try
                {
                    using (IDbCommand command = this.dbObj.CreateCommand())
                    {
                        command.Transaction = this.transObj;
                        command.CommandText = sql;
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            DataTable resultSet = Metreos.Utilities.Database.GetDataTable(reader);
                            this.logger.WriteLine(LogLevels.VERBOSE, "[  EXECUTE  ] {0}", sql);
                            if (resultSet == null)
                            {
                                return new DataTable();
                            }
                            else
                            {
                                return resultSet;
                            }
                        }
                    }
                }
                catch
                {
                    this.logger.WriteLine("[--FAILURE--] {0}", sql);
                    throw;
                }
            }
        }

        protected int GetLastId()
        {
            // Tread light here for DB compatibility.  This is by no means universal.
            // When we start supporting other DBs, we may have to rethink this.
            DataTable result = this.ExecuteQuery("SELECT LAST_INSERT_ID() AS id");
            if (result.Rows.Count > 0)
            {
                return (int)(Int64)result.Rows[0]["id"];
            }
            else
                throw new Exception("Could not retrieve the last inserted id");
        }

        #endregion

    }

}
