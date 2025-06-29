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
	public class SaveConferenceError : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("IP address of conference host", true)]
        public uint ConferenceId { set { conferenceId = value; } }
        private uint conferenceId;

        [ActionParamField("Error", true)]
        public string Error { set { error = value; } }
        private string error;

        public SaveConferenceError()
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
            error = null;
        }

        [Action("SaveConferenceError", false, "Save Conference Error", "Adds a conference error to database")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            IDbConnection db = sessionData.DbConnections[IDatabase.DB_NAME];             
            if(db == null)
            {
                log.Write(TraceLevel.Error, "Could not open application database: " + IDatabase.DB_NAME);
                return IApp.VALUE_FAILURE;
            }

            bool notPreviouslyOpen = IDatabase.Open(db);

            // Insert error
            Utils.SqlBuilder sql = new Utils.SqlBuilder(Utils.SqlBuilder.Method.INSERT, IDatabase.ERRORS_TABLE);
            sql.AddFieldValue(IDatabase.CONF_ID, conferenceId);
            sql.AddFieldValue(IDatabase.ERROR, error);

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
                log.Write(TraceLevel.Error, "Could not execute database query. Error was: " + e.Message);
                IDatabase.Close(db, notPreviouslyOpen, log);
                return IApp.VALUE_FAILURE;
            }

            IDatabase.Close(db, notPreviouslyOpen, log);
            return IApp.VALUE_SUCCESS;
        }

	}
}
