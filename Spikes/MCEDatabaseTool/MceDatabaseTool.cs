using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Metreos.Interfaces;
using Metreos.Utilities;
using DbFields = Metreos.Utilities.Database.Fields;
using DbKeys = Metreos.Utilities.Database.Keys;

namespace Metreos.Database
{
    public delegate void Log (string message);

    public interface MceDatabaseToolInterface
    {
        event Log Output;
        event Log Error;
    }

	public class MceDatabaseTool : MceDatabaseToolInterface
	{
        public event Log Output;
        public event Log Error;

        private IDbConnection dbConnection;
        private string dsn;

        #region Constructors
        public MceDatabaseTool() : this("mce", "127.0.0.1", 3306, "root", "metreos") { }

		public MceDatabaseTool(string dbName, string host, ushort port, string username, string password)
	    {
            dsn = Metreos.Utilities.Database.FormatDSN(dbName, host, port, username, password);			
        
            bool creationSuccess = CreateDatabase();

            if(!creationSuccess)
            {
                LogError("Unable to connect to database");
            }
        }

        #endregion

        #region Methods

        public bool DeleteAllUsers()
        {
            CheckoutConnection();

            try
            {
                using(IDbCommand command = dbConnection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM mce_users WHERE mce_users_id != 1";
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                LogError(e.ToString());
                return false;
            }

            CheckinConnection();

            return true;

        }
        public bool CreateUser(
            string name, 
            string password, 
            int creatorId, 
            DateTime createdTimeStamp, 
            DateTime updatedTimeStamp, 
            IConfig.AccessLevel accessLevel)
        {
            CheckoutConnection();

            try
            {
                using(IDbCommand command = dbConnection.CreateCommand())
                {
                    SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.INSERT, "mce_users");
                    sql.fieldNames.Add(DbFields.USERNAME);
                    sql.fieldNames.Add(DbFields.PASSWORD);
                    sql.fieldNames.Add(DbFields.CREATOR_ID);
                    sql.fieldNames.Add(DbFields.CREATED_TS);
                    sql.fieldNames.Add(DbFields.UPDATED_TS);
                    sql.fieldNames.Add(DbFields.ACCESS_LEVEL);
                    sql.fieldValues.Add(name);
                    sql.fieldValues.Add(password);
                    sql.fieldValues.Add(creatorId);
                    sql.fieldValues.Add(createdTimeStamp);
                    sql.fieldValues.Add(updatedTimeStamp);
                    sql.fieldValues.Add(accessLevel);

                    command.CommandText = sql.ToString();
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                LogError(e.ToString());
                return false;
            }

            CheckinConnection();

            return true;
        }

        public bool CreateRandomUsers(int numUsers)
        {
            bool allUsersMade = true;

            for(int i = 0; i < numUsers; i++)
            {
                string name = System.IO.Path.GetFileNameWithoutExtension(new System.IO.FileInfo(System.IO.Path.GetTempFileName()).Name);
                string password = Metreos.Utilities.Security.EncryptPassword("metreos");
                int creatorId = 1; // Administrator
                DateTime createdTimeStamp = DateTime.Now;
                DateTime updatedTimeStamp = DateTime.Now;
                IConfig.AccessLevel access;
                
                if(0 == i % 2)access = IConfig.AccessLevel.Administrator;
                else access = IConfig.AccessLevel.Normal;

                if(!CreateUser(name, password, creatorId, createdTimeStamp, updatedTimeStamp, access))
                {
                    LogError(String.Format("Unable to create user {0}", i));
                    allUsersMade = false;
                }
            }

            return allUsersMade;
        }
        #endregion

        #region Database Utility
        private bool CheckoutConnection()
        {
            try
            {
                dbConnection.Open();
            }
            catch(Exception e)
            {
                LogError(e.ToString());
                return false;
            }
            
            return true;
        }

        private void CheckinConnection()
        {
            if(dbConnection != null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }
        }
        #endregion


        #region Initialization/DeInitialization Routines
        private bool CreateDatabase()
        {
            try
            {
                dbConnection = new MySqlConnection(dsn);
            }
            catch(Exception e)
            {
                LogError(e.ToString());
                return false;
            }

            return true;

        }
        #endregion
        
        #region Interface MceDatabaseTool Interface
        public void LogOutput(string message)
        {
            if(Output != null)
            {
                Output(message);
            }
        }

        public void LogError(string message)
        {
            if(Error != null)
            {
                Error(message);
            }
        }
        #endregion
    }
}
