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
using ReturnValues = Metreos.Native.CBridge.Const.ConferenceReturnValues;

namespace Metreos.Native.CBridge
{
	/// <summary>
	/// Retrieves the record for the newest conference associated with a line
	/// </summary>
    [PackageDecl("Metreos.Native.CBridge")]
    public class GetConference : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("LineId", true)]
        public string LineId { set { lineId = value; } }
        private string lineId;
 
        [ResultDataField("The timestamp of the most recent conference")]
        public long Timestamp { get { return timestamp; } }
        private long timestamp;
        
        [ResultDataField("'true' if this conference is being recorded, false otherwise")]
        public bool IsRecorded { get { return isRecorded; } }
        private bool isRecorded;

        [ResultDataField("The routingGuid of the application handling this conference")]
        public string RoutingGuid { get { return routingGuid; } }
        private string routingGuid;

		private const string SqlString = "SELECT * FROM {0} WHERE {1}={2} ORDER BY {3} DESC LIMIT 1";

        public GetConference()
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
 
        [Action("GetConference", false, "Retrieves Conference info", "Retrieves conference record associated with given LineId")]
		[ReturnValue(typeof(Const.ConferenceReturnValues), "Valid values are: success, failure, NotFound")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {            
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
				
				string sqlQuery = string.Format(SqlString, new string[] { CBridgeTable.TableName, CBridgeTable.LineId, lineId, CBridgeTable.TimeStamp} );
				DataTable table = DbInteraction.ExecuteQuery(sqlQuery, connection);
                
                if (table == null || table.Rows.Count == 0)
                {
                    return ReturnValues.NotFound.ToString();
                }

                DataRow row = table.Rows[0];
                timestamp = Convert.ToInt64(row[CBridgeTable.TimeStamp]);
                isRecorded = Convert.ToBoolean (row[CBridgeTable.IsRecorded]);
                routingGuid = Convert.ToString(row[CBridgeTable.RoutingGuid]);

				return ReturnValues.success.ToString();
            }
            catch (Exception e)
            {
                object[] msgArray = new object[2] { lineId, e.Message } ;
                log.Write(TraceLevel.Error, "Error encountered in the GetConference method, using lineId: {0}\n"+
                                              "Error message: {1}", msgArray);
                return ReturnValues.failure.ToString();
            }
        }
    }
}
