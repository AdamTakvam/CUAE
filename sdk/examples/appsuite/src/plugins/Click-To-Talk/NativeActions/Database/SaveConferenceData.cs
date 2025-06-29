using System;
using System.Data;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Utils=Metreos.Utilities;

namespace Metreos.Native.ClickToTalk
{
	/// <summary>Saves conference information to application-specific database</summary>
	public class SaveConferenceData : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("IP address of conference host", false)]
        public string HostIP { set { host = IDatabase.FormatHostIP(value); } }
        private string host;

        [ActionParamField("Friendly name for host", false)]
        public string HostDescription { set { hostDesc = value; } }
        private string hostDesc;

        [ActionParamField("Indicates whether or not to record the conference", false)]
        public bool Record { set { record = value; } }
        private bool record;

        [ActionParamField("Email address to mail recorded conference to", false)]
        public string Email { set { email = value; } }
        private string email;

        [ActionParamField("Cisco IP Phone username for host's phone", false)]
        public string HostUsername { set { username = value; } }
        private string username;

        [ActionParamField("Cisco IP Phone password for host's phone", false)]
        public string HostPassword { set { password = value; } }
        private string password;

        [ResultDataField("ConferenceId")]
        public uint ResultData { get { return resultData; } }
        private uint resultData;

        public SaveConferenceData()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            if((host == null) || (host == "")) { return false; }

            if(email == null) { email = ""; }
            if(hostDesc == null) { hostDesc = ""; }
            if(username == null) { username = ""; }
            if(password == null) { password = ""; }

            return true;
        }

        public void Clear()
        {
            host = null;
            hostDesc = null;
            record = false;
            email = null;
            username = null;
            password = null;
            resultData = 0;
        }

        [Action("SaveConferenceData", false, "Save Conference Data", "Adds conference metadata to database")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            IDbConnection db = sessionData.DbConnections[IDatabase.DB_NAME];
            if(db == null)
            {
                log.Write(TraceLevel.Error, "Could not open application database: " + IDatabase.DB_NAME);
                return IApp.VALUE_FAILURE;
            }

            bool notPreviouslyOpen = IDatabase.Open(db);

            RemoveStaleConferences(log, db, host);
 
            Utils.SqlBuilder sql = new Utils.SqlBuilder(Utils.SqlBuilder.Method.INSERT, IDatabase.CONF_TABLE);
            sql.AddFieldValue(IDatabase.HOST, host);
            sql.AddFieldValue(IDatabase.HOST_DESC, hostDesc);
            sql.AddFieldValue(IDatabase.HOST_USER, username);
            sql.AddFieldValue(IDatabase.HOST_PASS, password);
            sql.AddFieldValue(IDatabase.RECORD, record.ToString());
            sql.AddFieldValue(IDatabase.RECORD_END, "false");
            sql.AddFieldValue(IDatabase.EMAIL, email);

            try
            {
                using(IDbCommand command = db.CreateCommand())
                {
                    command.CommandText = sql.ToString();
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Could not execute database command. Error was: " + e.Message);
                IDatabase.Close(db, notPreviouslyOpen, log);
                return IApp.VALUE_FAILURE;
            }

            try
            {
                using(IDbCommand command = db.CreateCommand())
                {
                    command.CommandText = IDatabase.GetLastInsertId;
                    resultData = Convert.ToUInt32(command.ExecuteScalar());
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Could not execute database command. Error was: " + e.Message);
                IDatabase.Close(db, notPreviouslyOpen, log);
                return IApp.VALUE_FAILURE;
            }

            IDatabase.Close(db, notPreviouslyOpen, log);
            return IApp.VALUE_SUCCESS;
        }
        
        private static void RemoveStaleConferences(LogWriter log, IDbConnection db, string host)
        {
            if(db == null) { return; }

            Utils.SqlBuilder sql = new Utils.SqlBuilder(Utils.SqlBuilder.Method.SELECT, IDatabase.CONF_TABLE);
            
            sql.where.Add(IDatabase.HOST, host);

            DataTable resultTable;
            try
            {
                using(IDbCommand command = db.CreateCommand())
                {
                    command.CommandText = sql.ToString();
                    using(IDataReader reader = command.ExecuteReader())
                    {
                        resultTable = Utils.Database.GetDataTable(reader);
                    }
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Could not execute database query ({0}). Error was: {1}", sql.ToString(), e.Message);
                return;
            }

            if(resultTable != null)
            {
                DataRowCollection originalResults = resultTable.Rows;

                foreach(DataRow currRow in originalResults)
                {
                    DeleteConference(db, log, Convert.ToInt32(currRow[IDatabase.ID]), host);
                }
            }
        }

        private static void DeleteConference(IDbConnection db, LogWriter log, int confId, string host)
        {
            // Delete from Conferences
            Utils.SqlBuilder sql = new Utils.SqlBuilder(Utils.SqlBuilder.Method.DELETE, IDatabase.CONF_TABLE);
            sql.where.Add(IDatabase.ID, confId);

            try
            {
                using(IDbCommand command = db.CreateCommand())
                {
                    command.CommandText = sql.ToString();
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Could not execute database command ({0}). Error was: {1}", sql.ToString(), e);
            }

            // Delete from Callees
            sql = new Utils.SqlBuilder(Utils.SqlBuilder.Method.DELETE, IDatabase.CALLEE_TABLE);
            sql.where.Add(IDatabase.CONF_ID, confId);

            try
            {
                using(IDbCommand command = db.CreateCommand())
                {
                    command.CommandText = sql.ToString();
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Could not execute database command ({0}). Error was: {1}", sql.ToString(), e);
            }

            // Delete from Errors
            sql = new Utils.SqlBuilder(Utils.SqlBuilder.Method.DELETE, IDatabase.ERRORS_TABLE);
            sql.where.Add(IDatabase.CONF_ID, confId);

            try
            {
                using(IDbCommand command = db.CreateCommand())
                {
                    command.CommandText = sql.ToString();
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Could not execute database command ({0}). Error was: {1}", sql.ToString(), e);
            }
        }
	}
}
