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
	public class GetConferenceData : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The unique ID of this conference", false)]
        public uint ConferenceId { set { conferenceId = value; } }
        private uint conferenceId;

        [ResultDataField("Conference metadata")]
        public DataRow ResultData { get { return resultData; } }
        private DataRow resultData;

        public GetConferenceData()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            if(conferenceId == 0) { return false; }
            return true;
        }

        public void Clear()
        {
            conferenceId = 0;
        }

        [Action("GetConferenceData", false, "Get Conference Data", "Gets metadata about this conference")]      
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            IDbConnection db = sessionData.DbConnections[IDatabase.DB_NAME];
            if(db == null)
            {
                log.Write(TraceLevel.Error, "Could not open application database: " + IDatabase.DB_NAME);
                return IApp.VALUE_FAILURE;
            }

            bool notPreviouslyOpen = IDatabase.Open(db);

            if(GetConferenceObject(conferenceId, db, log, out resultData) == false)
            {
                IDatabase.Close(db, notPreviouslyOpen, log);
                return IApp.VALUE_FAILURE;
            }

            IDatabase.Close(db, notPreviouslyOpen, log);
            return IApp.VALUE_SUCCESS;
        }

        public static bool GetConferenceObject(uint conferenceId,
            IDbConnection db, LogWriter log, out DataRow resultData)
        {
            resultData = null;
            if(log == null) { return false; }

            if(db == null)
            {
                log.Write(TraceLevel.Error, "Could not open application database: " + IDatabase.DB_NAME);
                return false;
            }

            Utils.SqlBuilder sql = new Utils.SqlBuilder(Utils.SqlBuilder.Method.SELECT, IDatabase.CONF_TABLE);
            sql.where.Add(IDatabase.ID, conferenceId);


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
                return false;
            }

            if((resultTable == null) || (resultTable.Rows.Count == 0))
            {
                log.Write(TraceLevel.Error, "No open conference found for conferenceId: " + conferenceId);
                return false;
            }


            resultData = resultTable.Rows[0];
            return true;
        }
	}
}
