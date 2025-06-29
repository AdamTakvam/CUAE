using System;
using System.Data;
using System.Diagnostics;
using System.Collections;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Utilities;
using ReturnValues = Metreos.Applications.cBarge.Const.ConferenceReturnValues;

namespace Metreos.Applications.cBarge
{
    /// <summary>
    /// Retrieves the record for the newest conference associated with a line
    /// </summary>
    public class GetBargeRecords : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("DirectoryNumber", true)]
        public string DirectoryNumber { set { directoryNumber = value; } }
        private string directoryNumber;

        [ActionParamField("Only match records with this lineInstance", false)]
        public int LineInstance
        {
            set
            {
                lineInstance = value;
                lineInstanceSet = true;
            }
        }
        private int lineInstance;
        private bool lineInstanceSet = false;

        [ResultDataField("DataTable of matching records, sorted by timestamp, ascending")]
        public DataTable CallRecordsTable { get { return callRecordsTable; } }
        private DataTable callRecordsTable;

        public GetBargeRecords()
        {
            Clear();
        }
 
        public void Clear()
        {
        }

        public void Reset()
        {
        }

        public bool ValidateInput()
        {
            return true;
        }
 
        [Action("GetBargeRecords", false, "Retrieves barge records", "Retrieves barge records associated with given line")]
        [ReturnValue(typeof(Const.ConferenceReturnValues), "Valid values are: success, failure, NotFound")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {            
            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.SELECT, CBargeRecords.TableName);
            builder.appendSemicolon = false;
            builder.where[CBargeRecords.DirectoryNumber] = directoryNumber;
            if (lineInstanceSet)
                builder.where[CBargeRecords.LineInstance] = lineInstance;
            string sortOrder = "DESC";

            string sqlQuery = builder.ToString();
            sqlQuery += " ORDER BY " + CBargeRecords.TimeStamp + " " + sortOrder + ";";

            IDbConnection connection = null;
            try
            {
                connection = DbInteraction.GetConnection(sessionData, Const.CbDbConnectionName, Const.CbDbConnectionString);
            }
            catch (Exception e)
            {
                object[] msgArray = new object[2] { Const.CbDbConnectionString, e.Message } ;
                log.Write(TraceLevel.Warning, "Could not open database at {0}.\n" + "Error Message: {1}", msgArray);
                return ReturnValues.failure.ToString();
            }
            try
            {
                callRecordsTable = DbInteraction.ExecuteQuery(sqlQuery, connection);
                
                if ((callRecordsTable == null) || (callRecordsTable.Rows.Count == 0))
                {
                    return ReturnValues.NotFound.ToString();
                }
                
                return ReturnValues.success.ToString();
            }
            catch (Exception e)
            {
                object[] msgArray = new object[2] { directoryNumber, e.Message } ;
                log.Write(TraceLevel.Error, "Error encountered in the GetBargeRecords method, using directoryNumber: {0}\n"+
                    "Error message: {1}", msgArray);
                return ReturnValues.failure.ToString();
            }
        }
    }
}
