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
    /// Update the value of the barge_routing_guid field, associating that instance of the barging script with the call record
    /// </summary>
    [PackageDecl("Metreos.Native.CBarge")]
    public class BindBargeToRecord : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("LineId with which to associate barge script routingGuid", true)]
        public string LineId { set { lineId = value; } }
        private string lineId;

        [ActionParamField("DeviceName of the device with which to associate self", true)]
        public string DeviceName { set { deviceName = value; } }
        private string deviceName;

        [ActionParamField("RoutingGuid of the script with which to associate barge script routingGuid", true)]
        public string RoutingGuid { set { routingGuid = value; } }
        private string routingGuid;

        [ActionParamField("The timestamp of the conference whose record we're updating", true)]
        public DateTime Timestamp { set { timestamp = value; } }
        private DateTime timestamp;

        [ActionParamField("RoutingGuid of the script handling barges for this application", true)]
        public string BargeRoutingGuid { set { bargeRoutingGuid = value; } }
        private string bargeRoutingGuid;

        public BindBargeToRecord()
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
 
        [Action("BindBargeToRecord", false, "Bind barge script routing guid to call record associated with given lineId, deviceName, and routingGuid", "Bind barge script routing guid to call record associated with given lineId, deviceName, and routingGuid")]
        [ReturnValue(typeof(Const.ConferenceReturnValues), "Valid values are: success, failure, NotFound")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {            
            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.UPDATE, CBargeCallRecords.TableName);
            builder.AddFieldValue(CBargeCallRecords.BargeRoutingGuid, bargeRoutingGuid);
            builder.where[CBargeCallRecords.LineId] = lineId;
            builder.where[CBargeCallRecords.RoutingGuid] = routingGuid;
            builder.where[CBargeCallRecords.DeviceName] = deviceName;
            builder.where[CBargeCallRecords.TimeStamp] = timestamp;

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
                object[] msgArray = new object[3] { lineId, routingGuid, e.Message } ;
                log.Write(TraceLevel.Error, "Error encountered in the BindBargeToRecord method, using lineId: {0} and routingGuid: {1}\n"+
                    "Error message: {2}", msgArray);
                return ReturnValues.failure.ToString();
            }
        }
    }
}