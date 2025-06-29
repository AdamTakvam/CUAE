using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.IO;

using ICSharpCode.SharpZipLib.Zip;
using Metreos.Utilities;
using Metreos.Interfaces;

namespace BehaviorCore
{
    public class Behavior : IDisposable
    {
        #region Members

        private IDbConnection dbConn;
        private string softwareVersion;
        private string firmwareVersion;
        private string releaseType;
        private string workPath;
        private string cuaePath;
        private string destPath;
        private ArrayList actions;
        private StringCollection errors;

        #endregion


        #region Properties
        
        public IDbConnection DbConnection
        {
            set
            {
                this.dbConn = value;
            }
        }

        public string DestinationPath
        {
            set
            {
                this.destPath = value;
            }

            get
            {
                return this.destPath;
            }
        }

        #endregion


        #region Methods

        public Behavior()
        {
            this.workPath = Config.PARENT_PATH + "\\work";
            this.cuaePath = Config.DEFAULT_CUAE_PATH;
            this.destPath = Config.DEFAULT_CUAE_PATH + "\\Backups";

            if (!Directory.Exists(this.workPath))
            {
                Directory.CreateDirectory(this.workPath);
            }
            this.actions = new ArrayList();
            this.errors = new StringCollection();
        }

        public void Dispose()
        {
            Directory.Delete(this.workPath,true);
        }

        public bool CreateBackup(string[] args)
        {

            // See what additional databases they want to backup

            Console.WriteLine("Checking database connection and databases...");
            StringCollection current_dbs = this.GetDatabaseNames();
            StringCollection add_dbs = new StringCollection();
            for (int x = 0; x < args.Length; x++)
            {
                if (args[x].StartsWith("+"))
                {
                    string tbl = args[x].Substring(1);
                    if (!current_dbs.Contains(tbl))
                    {
                        Console.WriteLine();
                        Console.WriteLine("ERROR: There is no database named '{0}'", tbl);
                        return false;
                    }
                    add_dbs.Add(tbl);
                }
            }


            // Get the CUAE version and load the proper actions

            this.GetSystemVersions();

            string main_version = Utils.GetMainVersion(this.softwareVersion);

            Actions.Database db_action = new Actions.Database(this.workPath);
            foreach (string tbl in add_dbs)
            {
                db_action.AddDatabase(tbl);
            }
            actions.Add(db_action);

            actions.Add(new Actions.Applications(this.workPath, this.cuaePath));
            if (String.Compare(main_version, "2.4.0") >= 0)
            {
                actions.Add(new Actions.Licenses(this.workPath, this.cuaePath));
                actions.Add(new Actions.RRD(this.workPath, this.cuaePath));
            }
            actions.Add(new Actions.MmsConfig(this.workPath, this.cuaePath));


            // Do a few checks to make sure all is kosher for a backup

            Console.WriteLine("Checking the system for backup items...");

            if (!this.CheckSystem())
            {
                Console.WriteLine();
                Console.WriteLine("The following problem(s) were found.  If a backup is created, the backup file will lack certain properties.");
                foreach (string error in this.errors)
                {
                    Console.WriteLine("    - {0}", error);
                }
                Console.WriteLine();
                Console.WriteLine("Would you like to continue? [Y/N] (Default:N) ");
                char response = Console.ReadKey(true).KeyChar;
                if (!(response == 'Y' || response == 'y'))
                {
                    return false;
                }
                Console.WriteLine();
            }


            // Build the backup package

            Console.WriteLine("Performing backup and creating backup file...");
            this.CreateMetaDataFile();
            foreach (Action a in this.actions)
            {
                if (a.CanRun)
                {
                    if (!a.Backup())
                        throw new Exception(String.Format("{0} action failed", a.GetType().ToString()));
                }
            }
            this.CreateZipFile();

            return true;
        }

        public bool Restore(string filename)
        {
            
            // Setup the package file for restore

            Console.WriteLine("Reading package...");
            this.GetSystemVersions();
            this.ExtractZipFile(filename);
            StringDictionary metadata = this.ReadMetaDataFile();


            // Make sure this is the package the user wanted

            Console.WriteLine();
            Console.WriteLine("Details on the backup package:");
            Console.WriteLine("    From Machine: {0}", metadata["MachineName"]);
            Console.WriteLine("    From Version: {0}", metadata["CuaeVersion"]);
            Console.WriteLine("    Date: {0}", metadata["Date"]);
            Console.WriteLine("Do you want to restore this backup? [Y/N] (Default:N)");
            char response = Console.ReadKey(true).KeyChar;
            if (!(response == 'Y' || response == 'y'))
            {
                return false;
            }


            // Load up the restore actions

            Console.WriteLine();
            Console.WriteLine("Checking package contents...");
            string main_package_version = Utils.GetMainVersion(metadata["CuaeVersion"]);
            this.actions.Add(new Actions.Database(this.workPath));
            this.actions.Add(new Actions.Applications(this.workPath, this.cuaePath));
            if (String.Compare(main_package_version, "2.4.0") >= 0)
            {
                this.actions.Add(new Actions.Licenses(this.workPath, this.cuaePath));
                this.actions.Add(new Actions.MmsConfig(this.workPath, this.cuaePath));
                this.actions.Add(new Actions.RRD(this.workPath, this.cuaePath));
            }


            // Check the package and notify the user of anything strange

            if (!this.CheckPackage())
            {
                Console.WriteLine();
                Console.WriteLine("The following issue(s) were found with the package.");
                foreach (string error in this.errors)
                {
                    Console.WriteLine("    - {0}", error);
                }
                Console.WriteLine("Would you like to continue? [Y/N] (Default:N) ");
                response = Console.ReadKey(true).KeyChar;
                if (!(response == 'Y' || response == 'y'))
                {
                    return false;
                }
            }


            // Do the restore
            Console.WriteLine();
            Console.WriteLine("Performing the restore...");
            foreach (Action a in this.actions)
            {
                if (a.CanRun)
                {
                    if (!a.Restore())
                        throw new Exception(String.Format("{0} action failed", a.GetType().ToString()));
                }
            }
            if (String.Compare(main_package_version, "2.4.0") < 0)
            {
                this.SetDatabaseVersion(Utils.GetDbVersionForSoftware(main_package_version));
            }
            this.EnableRedeployMedia();
            this.ReplaceSystemVersions();

            return true;
        }

        #endregion


        #region Helper Methods

        private void CreateZipFile()
        {
            string filename = String.Format("{0}-{1}.cuae", System.Environment.MachineName, System.DateTime.Now.ToString("yyyyMMdd-HHmmss"));
            FastZip zfile = new FastZip();
            zfile.CreateEmptyDirectories = true;
            zfile.CreateZip(filename, this.workPath, true, String.Empty);

            if (!Directory.Exists(this.destPath))
                Directory.CreateDirectory(this.destPath);
            File.Move(filename, this.destPath + "\\" + filename);
        }

        private void ExtractZipFile(string filename)
        {
            try
            {
                FastZip zfile = new FastZip();
                zfile.CreateEmptyDirectories = true;
                if (!Directory.Exists(this.workPath))
                    Directory.CreateDirectory(this.workPath);
                zfile.ExtractZip(filename, this.workPath, String.Empty);
            }
            catch
            {
                throw new Exception("Failed to open the package file.  Perhaps it is not a valid backup package?");
            }
        }

        private void GetSystemVersions()
        {
            string query_pattern = "SELECT value FROM mce_system_configs WHERE name = '{0}'";

            using (IDbCommand comm = this.dbConn.CreateCommand())
            {
                comm.CommandText = String.Format(query_pattern, "software_version");
                this.softwareVersion = (string) comm.ExecuteScalar();
            }

            using (IDbCommand comm = this.dbConn.CreateCommand())
            {
                comm.CommandText = String.Format(query_pattern, "firmware_version");
                this.firmwareVersion = (string) comm.ExecuteScalar();
            }

            using (IDbCommand comm = this.dbConn.CreateCommand())
            {
                comm.CommandText = String.Format(query_pattern, "release_type");
                this.releaseType = (string) comm.ExecuteScalar();
            }
        }

        private void ReplaceSystemVersions()
        {
            using (IDbCommand comm = this.dbConn.CreateCommand())
            {
                comm.CommandText = String.Format("UPDATE mce_system_configs SET value = '{0}' where name = 'software_version'", this.softwareVersion);
                comm.ExecuteNonQuery();
            }

            using (IDbCommand comm = this.dbConn.CreateCommand())
            {
                comm.CommandText = String.Format("UPDATE mce_system_configs SET value = '{0}' where name = 'firmware_version'", this.firmwareVersion);
                comm.ExecuteNonQuery();
            }

            using (IDbCommand comm = this.dbConn.CreateCommand())
            {
                comm.CommandText = String.Format("UPDATE mce_system_configs SET value = '{0}' where name = 'release_type'", this.releaseType);
                comm.ExecuteNonQuery();
            }
        }

        private void SetDatabaseVersion(string version)
        {
            using (IDbCommand comm = this.dbConn.CreateCommand())
            {
                comm.CommandText = String.Format("INSERT mce_system_configs SET name='database_version', value = '{0}'", version);
                comm.ExecuteNonQuery();
            }

            using (IDbCommand comm = this.dbConn.CreateCommand())
            {
                comm.CommandText = "INSERT mce_system_configs SET name='rollback_version'";
                comm.ExecuteNonQuery();
            }
        }

        private bool CreateMetaDataFile()
        {
            StringDictionary data = new StringDictionary();
            data.Add("MachineName", System.Environment.MachineName);
            data.Add("Date", System.DateTime.Now.ToString());
            data.Add("CuaeVersion", this.softwareVersion);

            StreamWriter sw = new StreamWriter(this.workPath + "\\" + Config.METADATA_FILE_NAME);
            foreach (DictionaryEntry entry in data)
            {
                sw.WriteLine(entry.Key + ":" + entry.Value);
            }
            sw.Close();
            return true;
        }

        private StringDictionary ReadMetaDataFile()
        {
            StringDictionary data = new StringDictionary();
            string meta_file = this.workPath + "\\" + Config.METADATA_FILE_NAME;

            if (!File.Exists(meta_file))
                throw new Exception("The package does not have any metadata associated with it.");

            StreamReader sr = new StreamReader(meta_file);
            while (!sr.EndOfStream)
            {
                string buffer = sr.ReadLine().Trim();
                if (buffer.Length > 0)
                {
                    string[] values = buffer.Split(new char[] { ':' }, 2);
                    if (values.Length == 2)
                        data.Add(values[0], values[1]);
                }
            }
            sr.Close();
            return data;
        }

        private StringCollection GetDatabaseNames()
        {
            StringCollection tables = new StringCollection();
            using (IDbCommand comm = this.dbConn.CreateCommand())
            {
                comm.CommandText = "SHOW DATABASES";
                using (IDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string tbl = reader.GetString(0);
                        if (tbl != "mysql")
                            tables.Add(tbl);
                    }
                    reader.Close();
                }
            }

            return tables;
        }

        private bool CheckSystem()
        {
            this.errors.Clear();
            foreach (Action a in this.actions)
            {
                if (!a.CheckSystem())
                    this.errors.Add(a.Error);
            }
            return (this.errors.Count == 0);
        }

        private bool CheckPackage()
        {
            this.errors.Clear();
            foreach (Action a in this.actions)
            {
                if (!a.CheckPackage())
                    this.errors.Add(a.Error);
            }
            return (this.errors.Count == 0);
        }

        private bool EnableRedeployMedia()
        {
            uint has_media_id = 0;

            using (IDbCommand comm = this.dbConn.CreateCommand())
            {
                SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Database.Tables.CONFIG_ENTRY_METAS);
                sql.fieldNames.Add(Database.Keys.CONFIG_ENTRY_METAS);
                sql.where.Add(Database.Fields.NAME, "HasMedia");
                comm.CommandText = sql.ToString();
                object hmid = comm.ExecuteScalar();
                if (hmid != null)
                    has_media_id = (uint) hmid;
            }

            if (has_media_id > 0)
            {
                using (IDbCommand comm = this.dbConn.CreateCommand())
                {
                    SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT, Database.Tables.COMPONENTS);
                    sql.fieldNames.Add(Database.Keys.COMPONENTS);
                    sql.where.Add(Database.Fields.TYPE, IConfig.ComponentType.MediaServer);
                    comm.CommandText = sql.ToString();
                    IDataReader reader = comm.ExecuteReader();

                    ArrayList components = new ArrayList();
                    while (reader.Read())
                    {
                        components.Add(reader.GetValue(0));
                    }
                    reader.Close();

                    foreach (uint comp_id in components)
                    {
                        DataTable rs = Database.ConfigEntries.Select(this.dbConn, comp_id, 0, has_media_id);
                        if (rs != null && rs.Rows.Count > 0)
                        {
                            uint entry_id = (uint) rs.Rows[0][Database.Keys.CONFIG_ENTRIES];
                            Database.ConfigValues.Delete(this.dbConn, entry_id);
                            Database.ConfigEntries.Delete(this.dbConn, entry_id);
                        }
                    }
                }
            }

            return true;
        }

        #endregion

    }
}
