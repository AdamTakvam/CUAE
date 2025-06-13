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
using ReturnValues = Metreos.Native.CBarge.Const.ConferenceReturnValues;

namespace Metreos.Native.CBarge
{
	/// <summary>
	/// Retrieves the record for the newest conference associated with a line
	/// </summary>
    [PackageDecl("Metreos.Native.CBarge")]
    public class GetCallRecords : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("LineId", true)]
        public string LineId { set { lineId = value; } }
        private string lineId;

        [ActionParamField("Only match records with this routing guid", false)]
        public string RoutingGuid 
        { 
            set 
            { 
                routingGuid = value; 
                routingGuidSet = true; 
                // temp kludge for testing
                Log.Write(TraceLevel.Info, "TEST: GetCallRecords: routing guid being set");
            }
        }
        private string routingGuid;
        private bool routingGuidSet = false;

        [ActionParamField("Only match records with this callId", false)]
        public long CallId
        {
            set
            {
                callId = value;
                callIdSet = true;
                // temp kludge for testing
                Log.Write(TraceLevel.Info, "TEST: GetCallRecords: DeviceName being set");
            }
        }
        private long callId;
        private bool callIdSet = false;

        [ActionParamField("Only match records with this device name", false)]
        public string DeviceName 
        { 
            set 
            { 
                deviceName = value;
                deviceNameSet = true;
                // temp kludge for testing
                Log.Write(TraceLevel.Info, "TEST: GetCallRecords: DeviceName being set");
            } 
        }
        private string deviceName;
        private bool deviceNameSet = false;

        [ActionParamField("Only match records with this barge routing guid", false)]
        public string BargeRoutingGuid 
        { 
            set 
            { 
                bargeRoutingGuid = value; 
                bargeRoutingGuidSet = true; 
                // temp kludge for testing
                Log.Write(TraceLevel.Info, "TEST: GetCallRecords: barge routing guid being set");
            }
        }
        private string bargeRoutingGuid;
        private bool bargeRoutingGuidSet = false;

 
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
            builder.where[CBargeCallRecords.LineId] = lineId;

            if (routingGuidSet)
                builder.where[CBargeCallRecords.RoutingGuid] = routingGuid;
            if (callIdSet)
                builder.where[CBargeCallRecords.CallId] = callId;
            if (deviceNameSet)
                builder.where[CBargeCallRecords.DeviceName] = deviceName;
            if (bargeRoutingGuidSet)
                builder.where[CBargeCallRecords.BargeRoutingGuid] = bargeRoutingGuid;

            string sqlQuery = builder.ToString();
            sqlQuery += " ORDER BY " + CBargeCallRecords.TimeStamp + " ASC;";

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
                object[] msgArray = new object[2] { lineId, e.Message } ;
                log.Write(TraceLevel.Error, "Error encountered in the GetCallRecords method, using lineId: {0}\n"+
                                              "Error message: {1}", msgArray);
                return ReturnValues.failure.ToString();
            }
        }
    }
}
