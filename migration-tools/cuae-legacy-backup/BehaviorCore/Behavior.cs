using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;


namespace BehaviorCore
{
    public class Behavior : IDisposable
    {
        #region Members

        private IDbConnection dbConn;
        private string softwareVersion;
        private string firmwareVersion;
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
			get
			{
				return this.destPath;
			}
        }

		public string CuaePath
		{
			set
			{
				this.cuaePath = value;
			}
		}

        #endregion


        #region Methods

        public Behavior()
        {
            this.workPath = Config.PARENT_PATH + "\\work";
            this.cuaePath = Config.DEFAULT_CUAE_PATH;
            this.destPath = Config.PARENT_PATH + "\\Backups";

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
			StringCollection current_dbs = this.GetDatabaseNames();
			StringCollection add_dbs = new StringCollection();
			for (int x = 0; x < args.Length; x++)
			{
				if (args[x].StartsWith("+"))
				{
					string tbl = args[x].Substring(1);
					if (!current_dbs.Contains(tbl))
					{
						Console.WriteLine("There is no database named '{0}'", tbl);
						return false;
					}
					add_dbs.Add(tbl);
				}
			}
			
			// Do a few checks to make sure all is kosher for a backup
            this.GetSystemVersions();

            string main_version = Utils.GetMainVersion(this.softwareVersion);
			
			Actions.Database db_action = new Actions.Database(this.workPath);
			foreach (string tbl in add_dbs)
			{
				db_action.AddDatabase(tbl);
			}
			actions.Add(db_action);
			
			actions.Add(new Actions.Applications(this.workPath, this.cuaePath));
            actions.Add(new Actions.MmsConfig(this.workPath, this.cuaePath));

            if (!this.CheckSystem())
            {
				Console.WriteLine("The following problem(s) were found.  If a backup is created, the backup file will lack certain properties.");
				foreach (string error in this.errors)
                {
                    Console.WriteLine("    - {0}", error);
                }
                Console.WriteLine("Would you like to continue? [Y/N] (Default:N) ");
                string response = Console.ReadLine();
                if (!response.Trim().ToLower().StartsWith("y"))
                {
                    return false;
                }
            }

            // Build the backup package
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

        private void GetSystemVersions()
        {
            IDbCommand comm = this.dbConn.CreateCommand();
            comm.CommandText = "SELECT value FROM mce_system_configs WHERE name = 'software_version'";
            this.softwareVersion = (string) comm.ExecuteScalar();
            comm.Dispose();

            comm = this.dbConn.CreateCommand();
            comm.CommandText = "SELECT value FROM mce_system_configs WHERE name = 'firmware_version'";
            this.firmwareVersion = (string) comm.ExecuteScalar();
            comm.Dispose();
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
				}
			}

			return tables;
		}

        #endregion

    }
}
