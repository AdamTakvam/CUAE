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
	public class GetCallees : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The unique ID of this conference", false)]
        public uint ConferenceID { set { confId = value; } }
        private uint confId;

        [ResultDataField("All non-host participants in this conference")]
        public DataRowCollection ResultData { get { return resultData; } }
        private DataRowCollection resultData;

        public GetCallees()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            if(confId == 0) { return false; }
            return true;
        }

        public void Clear()
        {
            confId = 0;
            resultData = null;
        }

        [Action("GetCallees", false, "Get Callees", "Gets list of callees to invite to this conference")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            IDbConnection db = sessionData.DbConnections[IDatabase.DB_NAME];
            if(db == null)
            {
                log.Write(TraceLevel.Error, "Could not open application database: " + IDatabase.DB_NAME);
                return IApp.VALUE_FAILURE;
            }

            bool notPreviouslyOpen = IDatabase.Open(db);

            Utils.SqlBuilder sql = new Utils.SqlBuilder(Utils.SqlBuilder.Method.SELECT, IDatabase.CALLEE_TABLE);
            sql.where.Add(IDatabase.CONF_ID, confId);

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
                IDatabase.Close(db, notPreviouslyOpen, log);
                return IApp.VALUE_FAILURE;
            }

            if(resultTable == null)
            {
                log.Write(TraceLevel.Error, "Failed to retrieve callee list from database.", sql.ToString());
                IDatabase.Close(db, notPreviouslyOpen, log);
                return IApp.VALUE_FAILURE;
            }

            resultData = resultTable.Rows;
            IDatabase.Close(db, notPreviouslyOpen, log);
            return IApp.VALUE_SUCCESS;
        }

	}
}
