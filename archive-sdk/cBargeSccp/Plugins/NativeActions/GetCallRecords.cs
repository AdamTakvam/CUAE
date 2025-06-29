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
    [PackageDecl("Metreos.Applications.cBarge")]
    public class GetCallRecords : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("DirectoryNumber", true)]
        public string DirectoryNumber { set { directoryNumber = value; } }
        private string directoryNumber;

        [ActionParamField("Only match records with this routing guid", false)]
        public string RoutingGuid 
        { 
            set 
            { 
                routingGuid = value; 
                routingGuidSet = true; 
            }
        }
        private string routingGuid;
        private bool routingGuidSet = false;

        [ActionParamField("Only match records with this callReference", false)]
        public int CallReference
        {
            set
            {
                callReference = value;
                callReferenceSet = true;
            }
        }
        private int callReference;
        private bool callReferenceSet = false;

        [ActionParamField("Only match records with this callInstance", false)]
        public int CallInstance
        {
            set
            {
                callInstance = value;
                callInstanceSet = true;
            }
        }
        private int callInstance;
        private bool callInstanceSet = false;

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

        [ActionParamField("Only match records with this device name", false)]
        public string Sid 
        { 
            set 
            { 
                sid = value;
                sidSet = true;
            } 
        }
        private string sid;
        private bool sidSet = false;

        [ActionParamField("Only match records with this barge routing guid", false)]
        public string BargeRoutingGuid 
        { 
            set 
            { 
                bargeRoutingGuid = value; 
                bargeRoutingGuidSet = true; 
            }
        }
        private string bargeRoutingGuid;
        private bool bargeRoutingGuidSet = false;

        [ActionParamField("Only match records with this conferenceId", false)]
        public string ConferenceId 
        { 
            set 
            { 
                conferenceId = value; 
                conferenceIdSet = true; 
            }
        }
        private string conferenceId;
        private bool conferenceIdSet = false;

        [ActionParamField("ASC or DESC, ASC by default", false)]
        public string SortOrder 
        { 
            set 
            { 
                sortOrder = value; 
                sortOrderSet = true; 
            }
        }
        private string sortOrder;
        private bool sortOrderSet = false;

        [ResultDataField("DataTable of matching records, sorted by timestamp, ascending")]
        public DataTable CallRecordsTable { get { return callRecordsTable; } }
        private DataTable callRecordsTable;

        public GetCallRecords()
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
 
        [Action("GetCallRecords", false, "Retrieves Call records", "Retrieves call records associated with given criteria")]
		[ReturnValue(typeof(Const.ConferenceReturnValues), "Valid values are: success, failure, NotFound")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {            
            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.SELECT, CBargeCallRecords.TableName);
            builder.appendSemicolon = false;
            builder.where[CBargeCallRecords.DirectoryNumber] = directoryNumber;

            if (routingGuidSet)
                builder.where[CBargeCallRecords.RoutingGuid] = routingGuid;
            if (callReferenceSet)
                builder.where[CBargeCallRecords.CallReference] = callReference;
            if (sidSet)
                builder.where[CBargeCallRecords.Sid] = sid;
            if (bargeRoutingGuidSet)
                builder.where[CBargeCallRecords.BargeRoutingGuid] = bargeRoutingGuid;
            if (callInstanceSet)
                builder.where[CBargeCallRecords.CallInstance] = callInstance;
            if (lineInstanceSet)
                builder.where[CBargeCallRecords.LineInstance] = lineInstance;
            if (conferenceIdSet)
                builder.where[CBargeCallRecords.ConferenceId] = conferenceId;

            if (!sortOrderSet)
                sortOrder = "ASC";

            string sqlQuery = builder.ToString();
            sqlQuery += " ORDER BY " + CBargeCallRecords.TimeStamp + " " + sortOrder + ";";

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
                log.Write(TraceLevel.Error, "Error encountered in the GetCallRecords method, using directoryNumber: {0}\n"+
                                              "Error message: {1}", msgArray);
                return ReturnValues.failure.ToString();
            }
        }
    }
}
