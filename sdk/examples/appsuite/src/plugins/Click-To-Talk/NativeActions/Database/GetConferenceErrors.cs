using System;
using System.Data;
using System.Diagnostics;
using System.Collections.Specialized;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Utils=Metreos.Utilities;

namespace Metreos.Native.ClickToTalk
{
	/// <summary>Retrieves conference information to application-specific database</summary>
	public class GetConferenceErrors : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("IP address of conference host", true)]
        public uint ConferenceId { set { conferenceId = value; } }
        private uint conferenceId;

        [ResultDataField("Conference errors")]
        public StringCollection ResultData { get { return resultData; } }
        private StringCollection resultData;

        public GetConferenceErrors()
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
            resultData = null;
            conferenceId = 0;
        }

        [Action("GetConferenceErrors", false, "Get Conference Errors", "Gets list of errors that ocurred during this conference")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            IDbConnection db = sessionData.DbConnections[IDatabase.DB_NAME];             
            if(db == null)
            {
                log.Write(TraceLevel.Error, "Could not open application database: " + IDatabase.DB_NAME);
                return IApp.VALUE_FAILURE;
            }

            bool notPreviouslyOpen = IDatabase.Open(db);

            Utils.SqlBuilder sql = new Utils.SqlBuilder(Utils.SqlBuilder.Method.SELECT, IDatabase.ERRORS_TABLE);
            sql.fieldNames.Add(IDatabase.ERROR);
            sql.where.Add(IDatabase.CONF_ID, conferenceId);
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
                log.Write(TraceLevel.Error, "Could not execute database query. Error was: " + e.Message);
                IDatabase.Close(db, notPreviouslyOpen, log);
                return IApp.VALUE_FAILURE;
            }

            resultData = new StringCollection();

            if(resultTable != null)
            {
                foreach(DataRow row in resultTable.Rows)
                {
                    resultData.Add(row[IDatabase.ERROR] as string);
                }
            }

            IDatabase.Close(db, notPreviouslyOpen, log);
            return IApp.VALUE_SUCCESS;
        }

	}
}
