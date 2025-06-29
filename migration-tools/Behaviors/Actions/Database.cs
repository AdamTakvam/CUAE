using System;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Diagnostics;


namespace BehaviorCore.Actions
{
    class Database : Action
    {
        private string work_path;
        private StringCollection databases;

        public Database(string workPath)
        {
            this.work_path = workPath + "\\Databases";
            this.databases = new StringCollection();
            this.databases.Add("mce");
        }

        public void AddDatabase(string dbname)
        {
            this.databases.Add(dbname);
        }

        public override bool CheckSystem()
        {
            if (!File.Exists(Config.MYSQLDUMP_PATH))
            {
                this.SetError(String.Format("mysqldump is not found at the the location {0}.  No database backups will be performed.", Config.MYSQLDUMP_PATH));
                return false;
            }
            return true;
        }

        public override bool CheckPackage()
        {
            if (!Directory.Exists(this.work_path))
            {
                this.SetError("There are no databases in the package to restore.");
                return false;
            }
            return true;
        }

        public override bool Backup()
        {
            Directory.CreateDirectory(this.work_path);
            foreach (string db_name in this.databases)
            {
                Console.WriteLine("Backing up database '{0}'...", db_name);
                string sql_file = String.Format("{0}\\{1}.sql", this.work_path, db_name);
                StringBuilder args = new StringBuilder().AppendFormat(" -u {0} -p{1} --add-drop-table --single-transaction --databases {2}", Config.DEFAULT_DB_USER, Config.DEFAULT_DB_PASSWORD, db_name);
                int exit_code = this.ExecuteCmd(Config.MYSQLDUMP_PATH, args.ToString(), sql_file);

                if (!File.Exists(sql_file))
                    throw new Exception(String.Format("Backup of the database {0} failed.",db_name));
            }
            return true;
        }

        public override bool Restore()
        {
            string[] files = Directory.GetFiles(this.work_path);
            for (int x = 0; x < files.Length; x++)
            {
                if (Path.GetExtension(files[x]) == ".sql")
                {
                    string db_name = Path.GetFileNameWithoutExtension(files[x]);
                    StringBuilder drop = new StringBuilder().AppendFormat(" -u {0} -p{1} -e \"DROP DATABASE IF EXISTS {2}\"", Config.DEFAULT_DB_USER, Config.DEFAULT_DB_PASSWORD, db_name);
                    StringBuilder args = new StringBuilder().AppendFormat(" -u {0} -p{1} -e \"source {2}\"", Config.DEFAULT_DB_USER, Config.DEFAULT_DB_PASSWORD, files[x]);
                    Console.WriteLine("Restoring database '{0}'...", db_name);

                    if (this.ExecuteCmd(Config.MYSQL_EXE_PATH, drop.ToString(), null) == 0)
                    {
                        int exit_code = this.ExecuteCmd(Config.MYSQL_EXE_PATH, args.ToString(), null);
                        if (exit_code != 0)
                            throw new Exception(String.Format("Restoration of database {0} failed.", db_name));
                    }
                    else
                    {
                        throw new Exception(String.Format("Could not drop the old {0} database.", db_name));
                    }
                }
            }
            return true;
        }

        private int ExecuteCmd(string cmd, string args, string outfile)
        {
            System.Diagnostics.Process proc = new Process();
            proc.StartInfo.FileName = cmd;
            proc.StartInfo.Arguments = args;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            if (outfile != null)
            {
                proc.StartInfo.RedirectStandardOutput = true;
            }
            proc.Start();
            if (outfile != null)
            {
                StreamWriter file = new StreamWriter(outfile);
                file.Write(proc.StandardOutput.ReadToEnd());
                file.Close();
            }
            proc.WaitForExit();
            int exit_code = proc.ExitCode;
            proc.Close();
            return exit_code;
        }
    }
}
