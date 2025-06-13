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
    /// Update the value of the barge_routing_guid field
    /// </summary>
    [PackageDecl("Metreos.Native.CBarge")]
    public class UpdateCallRecord : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("LineId of the line for whom we're updating this call record", true)]
        public string LineId { set { lineId = value; } }
        private string lineId;

        [ActionParamField("New RoutingGuid of the script that is handling call whose record we're updating", true)]
        public string RoutingGuid { set { routingGuid = value; } }
        private string routingGuid;

        [ActionParamField("The timestamp of the conference whose record we're updating", true)]
        public DateTime Timestamp { set { timestamp = value; } }
        private DateTime timestamp;

        [ActionParamField("New primary callId to be associated with this call record", false)]
        public long CallId
        {
            set
            {
                callId = value;
                callIdSet = true;
                // temp kludge for testing
                Log.Write(TraceLevel.Info, "TEST: UpdateCallRecord: DeviceName being set");
            }
        }
        private long callId;
        private bool callIdSet = false;

        [ActionParamField("New DeviceName of the device to be associated with this call record", false)]
        public string DeviceName 
        { 
            set 
            { 
                deviceName = value;
                deviceNameSet = true;
                // temp kludge for testing
                Log.Write(TraceLevel.Info, "TEST: UpdateCallRecord: DeviceName being set");
            } 
        }
        private string deviceName;
        private bool deviceNameSet = false;

        [ActionParamField("New RoutingGuid of the script handling barges for this application", false)]
        public string BargeRoutingGuid 
        { 
            set 
            { 
                bargeRoutingGuid = value; 
                bargeRoutingGuidSet = true; 
                // temp kludge for testing
                Log.Write(TraceLevel.Info, "TEST: UpdateCallRecord: barge routing guid being set");
            }
        }
        private string bargeRoutingGuid;
        private bool bargeRoutingGuidSet = false;


        public UpdateCallRecord()
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
 
        [Action("UpdateCallRecord", false, "Updates the call record associated with specified lineId, TimeStamp,  and routingGuid", "Updates the call record associated with specified lineId, TimeStamp, and routingGuid")]
        [ReturnValue(typeof(Const.ConferenceReturnValues), "Valid values are: success, failure, NotFound")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {            
            // warning, this action may return failure if none of the updatable fields are specified
            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.UPDATE, CBargeCallRecords.TableName);
            if (bargeRoutingGuidSet)
                builder.AddFieldValue(CBargeCallRecords.BargeRoutingGuid, bargeRoutingGuid);
            if (deviceNameSet)
                builder.AddFieldValue(CBargeCallRecords.DeviceName, deviceName);
            if (callIdSet)
                builder.AddFieldValue(CBargeCallRecords.CallId, callId);
            builder.where[CBargeCallRecords.LineId] = lineId;
            builder.where[CBargeCallRecords.RoutingGuid] = routingGuid;
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
                log.Write(TraceLevel.Error, "Error encountered in the UpdateCallRecord method, using lineId: {0} and routingGuid: {1}\n"+
                    "Error message: {2}", msgArray);
                return ReturnValues.failure.ToString();
            }
        }
    }
}