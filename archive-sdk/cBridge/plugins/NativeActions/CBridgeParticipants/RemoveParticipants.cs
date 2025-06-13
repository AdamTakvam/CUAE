/*
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
    /// Removes the record for conference participant(s)
    /// </summary>
    [PackageDecl("Metreos.Native.CBridge")]
    public class RemoveParticipants : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("LineId", true)]
        public string LineId { set { lineId = value; } }
        private string lineId;

        [ActionParamField("CallId", false)]
        public string CallId { set { callId = value; } }
        private string callId;

        [ActionParamField("Timestamp", true)]
        public DateTime Timestamp { set { timestamp = value; } }
        private DateTime timestamp;
 
        public RemoveParticipants()
        {
            Clear();
        }
 
        public void Clear()
        {
            lineId = callId = null;
        }

        public bool ValidateInput()
        {
            return true;
        }
 
        [Action("RemoveParticipants", false, "Removes participant record(s)", "If callId is not specified, all participants associated with line and timestamp are removed.")]
        [ReturnValue(typeof(Const.ConferenceReturnValues), "Valid values are: success, failure, NotFound")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {            
            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.DELETE, CBridgeParticipantsTable.TableName);
            builder.where[CBridgeParticipantsTable.LineId] = lineId;
            builder.where[CBridgeParticipantsTable.TimeStamp] = timestamp;
            
            if (callId != null)
                builder.where[CBridgeParticipantsTable.CallId] = callId;
            
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
                object[] msgArray = new object[2] { lineId, e.Message } ;
                log.Write(TraceLevel.Error, "Error encountered in the RemoveParticipants method, using lineId: {0}\n"+
                    "Error message: {1}", msgArray);
                return ReturnValues.failure.ToString();
            }
        }
    }
}
*/