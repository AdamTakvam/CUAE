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
    /// Update the value of the barge_routing_guid field, associating that instance of the barging script with the call record
    /// </summary>
    [PackageDecl("Metreos.Applications.cBarge")]
    public class BindBargeToRecord : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("DirectoryNumber with which to associate barge script routingGuid", true)]
        public string DirectoryNumber { set { directoryNumber = value; } }
        private string directoryNumber;

        [ActionParamField("Sid of the device with which to associate self", true)]
        public string Sid { set { sid = value; } }
        private string sid;

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
 
        [Action("BindBargeToRecord", false, "Bind barge script routing guid to call record associated with given directoryNumber, sid, and routingGuid", "Bind barge script routing guid to call record associated with given directoryNumber, sid, and routingGuid")]
        [ReturnValue(typeof(Const.ConferenceReturnValues), "Valid values are: success, failure, NotFound")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {            
            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.UPDATE, CBargeCallRecords.TableName);
            builder.AddFieldValue(CBargeCallRecords.BargeRoutingGuid, bargeRoutingGuid);
            builder.where[CBargeCallRecords.DirectoryNumber] = directoryNumber;
            builder.where[CBargeCallRecords.RoutingGuid] = routingGuid;
            builder.where[CBargeCallRecords.Sid] = sid;
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
                object[] msgArray = new object[3] { directoryNumber, routingGuid, e.Message } ;
                log.Write(TraceLevel.Error, "Error encountered in the BindBargeToRecord method, using directoryNumber: {0} and routingGuid: {1}\n"+
                    "Error message: {2}", msgArray);
                return ReturnValues.failure.ToString();
            }
        }
    }
}