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
	/// <summary>Retrieves conference information to application-specific database</summary>
	[PackageDecl("Metreos.Native.ClickToTalk", "Click-To-Talk database access actions")]
	public class DeleteConferenceData : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("IP address of conference host", true)]
        public uint ConferenceId { set { conferenceId = value; } }
        private uint conferenceId;

        public DeleteConferenceData() 
        {
            Clear();
        }

        public bool ValidateInput()
        {
            if(conferenceId == 0) return false;
            return true;
        }

        public void Clear()
        {
            conferenceId = 0;
        }

        [Action("DeleteConferenceData", false, "Delete Conference Data", "Deletes all data related to specified conference")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {            
            IDbConnection db = sessionData.DbConnections[IDatabase.DB_NAME];
            if(db == null)
            {
                log.Write(TraceLevel.Error, "Could not open application database: " + IDatabase.DB_NAME);
                return IApp.VALUE_FAILURE;
            }

            bool notPreviouslyOpen = IDatabase.Open(db);

            // Delete entry from Conferences table
            Utils.SqlBuilder sql = new Utils.SqlBuilder(Utils.SqlBuilder.Method.DELETE, IDatabase.CONF_TABLE);
            sql.where.Add(IDatabase.ID, conferenceId);

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

            // Delete entries from Callees table
            sql = new Utils.SqlBuilder(Utils.SqlBuilder.Method.DELETE, IDatabase.CALLEE_TABLE);
            sql.where.Add(IDatabase.CONF_ID, conferenceId);

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
        
            // Delete entries from Errors table
            sql = new Utils.SqlBuilder(Utils.SqlBuilder.Method.DELETE, IDatabase.ERRORS_TABLE);
            sql.where.Add(IDatabase.CONF_ID, conferenceId);

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

            IDatabase.Close(db, notPreviouslyOpen, log);
            return IApp.VALUE_SUCCESS;
        }

	}
}
