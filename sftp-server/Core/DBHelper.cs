using System;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Collections;

using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;

namespace Metreos.SftpServer
{
	/// <summary>Config database abstraction</summary>
	public class DBHelper
	{
        #region Constants

        private abstract class Consts
        {
            public const string AudioDir    = "MediaServer\\Audio";
            public const string GrammarDir  = "MediaServer\\Grammar";
            public const string DeployDir   = "AppServer\\Deploy";
        }
        #endregion

        private static object dbConnectLock = new object();

        private readonly string dbName;
        private readonly string host;
        private readonly ushort port;
        private readonly string username;
        private readonly string password;
        private readonly string audioUser;
        private readonly string grammarUser;

		public DBHelper(string dbName, string host, ushort port, string username, string password, 
            string audioUser, string grammarUser)
		{
            Assertion.Check(host != null, "Cannot connect to database with no host specified");
            Assertion.Check(port > 1024, "Cannot connect to database with invalid port specified");
            Assertion.Check(username != null, "Cannot connect to database with no username specified");
            Assertion.Check(password != null, "Cannot connect to database with no password specified");

            this.dbName = dbName;
            this.host = host;
            this.port = port;
            this.username = username;
            this.password = password;
            this.audioUser = audioUser;
            this.grammarUser = grammarUser;
		}

        // Password must be MD5 encoded by the client
        public bool IsAuthorized(string username, string password)
        {
            using(IDbConnection db = OpenDB())
            {
                string storedPass = null;
                if(username == audioUser || username == grammarUser)
                {
                    DataTable sysTable = Database.SystemConfigs.Select(db, IConfig.SystemConfigs.MEDIA_PASSWORD);
                    if(sysTable == null || sysTable.Rows.Count != 1)
                        return false;
                
                    storedPass = Convert.ToString(sysTable.Rows[0][Database.Fields.VALUE]);
                }
                else  // deploy user
                {
                    DataTable userTable = Database.Users.Select(db, username, IConfig.AccessLevel.Unspecified);
                    if(userTable == null || userTable.Rows.Count != 1)
                        return false;
                
                    storedPass = Convert.ToString(userTable.Rows[0][Database.Fields.PASSWORD]);
                }

                return storedPass == password;
            }
        }

        public DirectoryInfo GetHomeDirectory(string username)
        {
            string relDir;

            if(username == audioUser)
                relDir = Consts.AudioDir;
            else if(username == grammarUser)
                relDir = Consts.GrammarDir;
            else 
                relDir = Consts.DeployDir;

            DirectoryInfo homeDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            homeDir = homeDir.Parent;

            string[] subDirs = relDir.Split('\\');
            foreach(string subDir in subDirs)
            {
                foreach(DirectoryInfo subDirInfo in homeDir.GetDirectories(subDir))
                {
                    if(subDir == subDirInfo.Name)
                        homeDir = subDirInfo;
                }
            }

            return homeDir;
        }

        #region Test

        /// <summary>Verifies DB connectivity</summary>
        /// <returns>Whether or not a DB connection could be made</returns>
        public bool Test()
        {
            IDbConnection db = OpenDB();
            if(db == null)
                return false;

            db.Dispose();
            return true;
        }
        #endregion

        #region Private methods

        private IDbConnection OpenDB()
        {
            // One at a time please. No pushing.
            lock(dbConnectLock)
            {
                string dsn = Database.FormatDSN(dbName, host, port, username, password, true);
                if(dsn == null)
                    return null;

                IDbConnection newDb = Database.CreateConnection(Database.DbType.mysql, dsn);

                try { newDb.Open(); }
                catch(Exception e)
                {
                    Trace.Write(String.Format("Failed to connect to config database '{0}@{1}:{2} ({3})'. Error: {4}",
                        username, host, port, dbName, e.Message), TraceLevel.Error.ToString());
                    return null;
                }
                return newDb;
            }
        }
        #endregion
	}
}
