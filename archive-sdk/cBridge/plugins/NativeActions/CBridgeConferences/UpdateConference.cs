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
    /// Updates the database conference record with specified lineId and timestamp, setting the
    /// cb_timestamp to System.DateTime.Now, and returns that value.
    /// </summary>
    [PackageDecl("Metreos.Native.CBridge")]
    public class UpdateConference : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("LineId", true)]
        public string LineId { set { lineId = value; } }
        private string lineId;

        [ActionParamField("Timestamp", true)]
        public long Timestamp { set { timestamp = value; } }
        private long timestamp;

        [ActionParamField("RoutingGuid", true)]
        public string RoutingGuid { set { routingGuid = value; } }
        private string routingGuid;

        [ActionParamField("SleepTime", false)]
        public int SleepTime { set { sleepTime = value; } }
        private int sleepTime = 101;

        [ResultDataField("The new timestamp of this conference")]
        public long NewTimestamp { get { return newTimestamp; } }
        private long newTimestamp;

        public UpdateConference()
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
 
        [Action("UpdateConference", false, "Updates conference record", "Updates the database conference record with specified lineId and timestamp, setting the cb_timestamp to System.DateTime.Now, and returns that value.")]
        [ReturnValue(typeof(Const.ConferenceReturnValues), "Valid values are: success, failure, NotFound")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {            
            System.Threading.Thread.Sleep(sleepTime);
            newTimestamp = DateTime.Now.Ticks;
            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.UPDATE, CBridgeTable.TableName);
            builder.AddFieldValue(CBridgeTable.TimeStamp, newTimestamp);
            builder.where[CBridgeTable.LineId] = lineId;
            builder.where[CBridgeTable.TimeStamp] = timestamp;
            builder.where[CBridgeTable.RoutingGuid] = routingGuid;

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
                int affectedRows = DbInteraction.ExecuteNonQuery(builder.ToString(), connection);
                
                return (affectedRows == 0) ? ReturnValues.NotFound.ToString() : ReturnValues.success.ToString();
            }
            catch (Exception e)
            {
                object[] msgArray = new object[3] { lineId, timestamp, e.Message } ;
                log.Write(TraceLevel.Error, "Error encountered in the UpdateConference method, using lineId: {0} and timestamp: {1}\n"+
                    "Error message: {2}", msgArray);
                return ReturnValues.failure.ToString();
            }
        }
    }
}
