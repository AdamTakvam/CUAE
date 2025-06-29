using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Reflection;
using System.IO;

using Metreos.Utilities;
using Metreos.Configuration;


namespace MigrationCore
{
    public class Driver : IDisposable
    {

        private Dictionary<string, IMigrationDefinition> migrations;
        private IDbConnection dbObj;
        private string currentVersion;
        private string rollbackVersion;
        private LogWriter logger;

        public int LogLevel
        {
            set
            {
                this.logger.LogLevel = value;
            }
        }

        #region Methods

        public Driver()
        {
            // Read from the configuration file
            Hashtable configs = Metreos.Configuration.AppConfig.GetTable();
            string dbName = configs[Configuration.DATABASE_NAME_CONFIG].ToString();
            string dbHost = configs[Configuration.DATABASE_HOST_CONFIG].ToString();
            ushort dbPort = ushort.Parse(configs[Configuration.DATABASE_PORT_CONFIG].ToString());
            string dbUser = configs[Configuration.DATABASE_USER_CONFIG].ToString();
            string dbPasswd = configs[Configuration.DATABASE_PASSWD_CONFIG].ToString();

            // Create the database object
            string dsn = Database.FormatDSN(dbName, dbHost, dbPort, dbUser, dbPasswd, true);
            this.dbObj = Database.CreateConnection(Database.DbType.mysql, dsn);
            this.dbObj.Open();

            this.currentVersion = this.GetCurrentVersion();

            if (this.currentVersion.Trim() == String.Empty)
            { 
                throw new Exception("Could not retrieve the current database version.  Either the version number is not defined or there was a failure connecting to the database.");
            }

            this.rollbackVersion = this.GetRollbackVersion();

            this.logger = new LogWriter(System.DateTime.Now.ToString(Configuration.FILE_TIMESTAMP_FORMAT));
            this.migrations = new Dictionary<string, IMigrationDefinition>();
        }

        public void Dispose()
        {
            this.dbObj.Close();
            this.logger.Dispose();
        }

        public bool Upgrade()
        {
            return Upgrade(null);
        }

        public bool Upgrade(string toVersion)
        {
            if (!this.LoadDefinitions())
            {
                return false;
            }

            ArrayList migrationObjects = this.DeterminePath(this.currentVersion, toVersion);

            if (migrationObjects.Count == 0)
            {
                return true;
            }

            string finalVersion = this.currentVersion;
            foreach (MigrationDefinition migObjExec in migrationObjects)
            {
                using (IDbTransaction transObj = this.dbObj.BeginTransaction())
                {
                    try
                    {
                        this.logger.WriteLine();
                        this.logger.WriteLine("Upgrading database v{0} => v{1}...", migObjExec.CurrentVersion, migObjExec.NextVersion);
                        migObjExec.Logger = this.logger;
                        migObjExec.Transaction = transObj;
                        migObjExec.DoUpgrade();
                        transObj.Commit();
                        finalVersion = migObjExec.NextVersion;
                        this.logger.WriteLine("Version upgrade successful");
                    }
                    catch (Exception ex)
                    {
                        transObj.Rollback();
                        this.logger.WriteLine("FAILED: Database upgrade v{0} => v{1}", migObjExec.CurrentVersion, migObjExec.NextVersion);
                        this.logger.WriteLine("EXCEPTION THROWN: {0}", ex.ToString());
                        this.logger.WriteLine();
                        this.SetNewVersion(finalVersion);
                        return false;
                    }
                }
            }

            this.SetNewVersion(finalVersion);
            return true;
        }


        public bool Rollback()
        {
            return this.Rollback(this.rollbackVersion);
        }

        public bool Rollback(string toVersion)
        {
            string finalVersion = this.currentVersion;
            if (toVersion.Trim().Length == 0)
            {
                throw new Exception("No version defined for rollback or rollback already performed.");
            }
            if (!this.LoadDefinitions())
            {
                return false;
            }
            ArrayList migrationObjects = this.DeterminePath(toVersion, this.currentVersion);
            migrationObjects.Reverse();
            foreach (MigrationDefinition migObjExec in migrationObjects)
            {
                using (IDbTransaction transObj = this.dbObj.BeginTransaction())
                {
                    try
                    {
                        this.logger.WriteLine();
                        this.logger.WriteLine("Rolling back database v{0} => v{1}...", migObjExec.NextVersion, migObjExec.CurrentVersion);
                        migObjExec.Logger = this.logger;
                        migObjExec.Transaction = transObj;
                        migObjExec.DoRollback();
                        transObj.Commit();
                        finalVersion = migObjExec.CurrentVersion;
                        this.logger.WriteLine("Version roll back was successful");
                    }
                    catch (Exception ex)
                    {
                        transObj.Rollback();
                        this.logger.WriteLine();
                        this.logger.WriteLine("FAILED: Database roll back v{0} => v{1}", migObjExec.NextVersion, migObjExec.CurrentVersion);
                        this.logger.WriteLine("EXCEPTION THROWN: {0}\n", ex.ToString());
                        this.SetNewVersion(finalVersion, toVersion);
                        return false;
                    }
                }
            }

            this.SetNewVersion(toVersion, null);
            return true;                        
        }

        #endregion


        #region Private Methods

        private bool LoadDefinitions()
        {
            // Grab the Migration definition class library
            // Create a dictionary of Migration classes and the version from which they update
            Assembly migDefsAssembly = Assembly.LoadFrom(Configuration.MIGRATION_DEFINITION_ASSEMBLY);
            System.Type[] definitionTypes = migDefsAssembly.GetExportedTypes();

            foreach (System.Type defType in definitionTypes)
            {
                object[] verAttribs = defType.GetCustomAttributes(typeof(MigrationCore.VersionAttribute), false);
                if (1 == verAttribs.Length)
                {
                    MigrationCore.VersionAttribute attrib = verAttribs[0] as MigrationCore.VersionAttribute;
                    IMigrationDefinition migDef = (IMigrationDefinition) defType.InvokeMember("", BindingFlags.CreateInstance, null, defType, new Object[] { Database.DbType.mysql, this.dbObj });
                    if (this.migrations.ContainsKey(attrib.Version))
                    {
                        this.logger.WriteLine("ERROR: There is more than one migration definition for version {0}", attrib.Version);
                        return false;
                    }
                    this.migrations.Add(attrib.Version, migDef);
                }
            }

            if (this.migrations.Count == 0)
            {
                this.logger.WriteLine("There are no migration classes defined.");
                return false;
            }

            return true;
        }

        private ArrayList DeterminePath(string fromVersion, string toVersion)
        {
            // Check to see that a class exists to start the upgrade from this version
            if (!this.migrations.ContainsKey(fromVersion))
            {
                Exception ex = new Exception("No migration path is defined for starting version.");
                ex.Data.Add(Configuration.DISPLAY_NO_EXCEPTION_KEY, 1);
                throw ex;
            }

            // Iterate through the classes and determine a chain that will become the upgrade path
            ArrayList migrationObjects = new ArrayList();

            IMigrationDefinition migrationObj;
            string versionIt = fromVersion;
            bool readClasses = true;
            while (readClasses)
            {
                if (!migrations.TryGetValue(versionIt, out migrationObj))
                {
                    readClasses = false;
                }
                else
                {
                    if (!migrationObj.IsReady())
                    {
                        readClasses = false;
                    }
                    else
                    {
                        migrationObjects.Add(migrationObj);
                        versionIt = migrationObj.NextVersion;
                        if (toVersion != null && toVersion == versionIt)
                        {
                            readClasses = false;
                        }
                    }
                }
            }

            return migrationObjects;
        }

        private string GetCurrentVersion()
        {
            DataTable value = Metreos.Utilities.Database.SystemConfigs.Select(this.dbObj, Metreos.Interfaces.IConfig.SystemConfigs.DATABASE_VERSION);
            return value.Rows[0][Database.Fields.VALUE].ToString();
        }

        private string GetRollbackVersion()
        {
            DataTable value = Metreos.Utilities.Database.SystemConfigs.Select(this.dbObj, Metreos.Interfaces.IConfig.SystemConfigs.ROLLBACK_VERSION);
            return value.Rows[0][Database.Fields.VALUE].ToString();
        }

        private int SetNewVersion(string newVersion)
        {
            return this.SetNewVersion(newVersion, this.currentVersion);
        }

        private int SetNewVersion(string newVersion, string oldVersion)
        {
            if (newVersion != oldVersion)
            {
                if (Metreos.Utilities.Database.SystemConfigs.Update(this.dbObj, Metreos.Interfaces.IConfig.SystemConfigs.DATABASE_VERSION, newVersion) == 0)
                {
                    return 0;
                }
                return Metreos.Utilities.Database.SystemConfigs.Update(this.dbObj, Metreos.Interfaces.IConfig.SystemConfigs.ROLLBACK_VERSION, oldVersion);
            }
            else
            {
                return 0;
            }
        }

        public bool UseDryRunDatabase()
        {
            DataTable tables = null;
            StringDictionary create_statements = new StringDictionary();

            string main_db = this.dbObj.Database;
            string dry_run_db = String.Format("{0}_dry_run", this.dbObj.Database);

            this.logger.WriteLine("Creating a temporary database for a test run...");
            using (IDbCommand comm = this.dbObj.CreateCommand())
            {
                comm.CommandText = String.Format("CREATE DATABASE IF NOT EXISTS {0}", dry_run_db);
                comm.ExecuteNonQuery();
            }

            // Get table list
            using (IDbCommand comm = this.dbObj.CreateCommand())
            {
                comm.CommandText = "SHOW TABLES";
                using (IDataReader reader = comm.ExecuteReader())
                {
                    tables = Metreos.Utilities.Database.GetDataTable(reader);
                }
            }

            // Get table create statements
            for (int x=0; x < tables.Rows.Count; x++)
            {
                using (IDbCommand comm = this.dbObj.CreateCommand())
                {
                    comm.CommandText = String.Format("SHOW CREATE TABLE {0}", tables.Rows[x][0]);
                    using (IDataReader reader = comm.ExecuteReader())
                    {
                        DataTable result = Metreos.Utilities.Database.GetDataTable(reader);
                        create_statements.Add(result.Rows[0][0].ToString(), result.Rows[0][1].ToString());
                    }
                }
            }

            // Change database and make copies
            this.dbObj.ChangeDatabase(dry_run_db);
            foreach (DictionaryEntry de in create_statements)
            {
                using (IDbCommand comm = this.dbObj.CreateCommand())
                {
                    comm.CommandText = de.Value.ToString();
                    comm.ExecuteNonQuery();
                }

                using (IDbCommand comm = this.dbObj.CreateCommand())
                {
                    comm.CommandText = String.Format("INSERT INTO `{0}` SELECT * FROM `{1}`.`{0}`", de.Key.ToString(), main_db);
                    comm.ExecuteNonQuery();
                }
            }

            return true;
        }

        public bool DropDryRunDatabase()
        {
            Hashtable configs = Metreos.Configuration.AppConfig.GetTable();
            string dry_run_db = this.dbObj.Database;
            string main_db = configs[Configuration.DATABASE_NAME_CONFIG].ToString();

            if (dry_run_db != main_db)
            {
                this.logger.WriteLine();
                this.logger.WriteLine("Dropping temporary database...");
                this.dbObj.ChangeDatabase(main_db);
                using (IDbCommand comm = this.dbObj.CreateCommand())
                {
                    comm.CommandText = String.Format("DROP DATABASE {0}", dry_run_db);
                    comm.ExecuteNonQuery();
                }
            }
            return true;
        }

        #endregion

    }
}
