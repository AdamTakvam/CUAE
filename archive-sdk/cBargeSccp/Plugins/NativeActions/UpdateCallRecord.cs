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
using MySql.Data.Types;

namespace Metreos.Applications.cBarge
{
    /// <summary>
    /// Updates the record with given directory number, line instance, and call reference.
    /// </summary>
    public class UpdateCallRecord : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("DirectoryNumber", true)]
        public string DirectoryNumber { set { directoryNumber = value; } }
        private string directoryNumber;

        [ActionParamField("CallReference", true)]
        public int CallReference { set { callReference = value; } }
        private int callReference;

        [ActionParamField("UpdateBargeRecord", true)]
        public bool UpdateBargeRecord { set { updateBargeRecord = value; } }
        private bool updateBargeRecord;

        [ActionParamField("If the UPDATE fails, and this parameter is true, we will try to INSERT a new record.", false)]
        public bool InsertOnUpdateFail { set { insertOnUpdateFail = value; } }
        private bool insertOnUpdateFail;

        [ActionParamField("CallInstance", false)]
        public int CallInstance { set { callInstance = value; callInstanceSet = true; } }
        private int callInstance;
        private bool callInstanceSet;

        [ActionParamField("LineInstance", true)]
        public int LineInstance { set { lineInstance = value; } }
        private int lineInstance;

        [ActionParamField("RoutingGuid", false)]
        public string RoutingGuid { set { routingGuid = value; routingGuidSet = true; } }
        private string routingGuid;
        private bool routingGuidSet;

        [ActionParamField("Sid of the device to be associated with this call record", false)]
        public string Sid { set { sid = value; sidSet = true; } }
        private string sid;
        private bool sidSet;

        [ActionParamField("MmsId on which the conference resides", false)]
        public uint MmsId { set { mmsId = value; mmsIdSet = true; } }
        private uint mmsId;
        private bool mmsIdSet;

        [ActionParamField("ConferenceId", false)]
        public string ConferenceId { set { conferenceId = value; conferenceIdSet = true; } }
        private string conferenceId;
        private bool conferenceIdSet;

        [ResultDataField("The timestamp of this conference")]
        public DateTime Timestamp { get { return timestamp; } }
        private DateTime timestamp;

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
 
        [Action("UpdateCallRecord", false, "Updates call record", "Updates call record with given directory number, line instance, and call reference, and optionally updates barge record.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {            
            timestamp = DateTime.Now;
            string dnLi = directoryNumber + ":" + lineInstance.ToString();
            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.UPDATE, CBargeCallRecords.TableName);
            builder.AddFieldValue(CBargeCallRecords.DirectoryNumber, directoryNumber);
            builder.AddFieldValue(CBargeCallRecords.CallReference, callReference);
            builder.AddFieldValue(CBargeCallRecords.TimeStamp, timestamp);
            builder.AddFieldValue(CBargeCallRecords.LineInstance, lineInstance);
            builder.AddFieldValue(CBargeCallRecords.DnLi, dnLi);
            if (sidSet)
                builder.AddFieldValue(CBargeCallRecords.Sid, sid);
            if (routingGuidSet)
                builder.AddFieldValue(CBargeCallRecords.RoutingGuid, routingGuid);
            if (callInstanceSet)
                builder.AddFieldValue(CBargeCallRecords.CallInstance, callInstance);
            if (mmsIdSet)
                builder.AddFieldValue(CBargeCallRecords.MmsId, mmsId);
            if (conferenceIdSet)
                builder.AddFieldValue(CBargeCallRecords.ConferenceId, conferenceId);
            builder.where[CBargeCallRecords.CallReference] = callReference;
            // the following two lines are not really necessary but I'm leaving them here in case I'm 
            // not seeing something minor.
            builder.where[CBargeCallRecords.DirectoryNumber] = directoryNumber;
            builder.where[CBargeCallRecords.LineInstance] = lineInstance;
            builder.where[CBargeCallRecords.DnLi] = dnLi;

            IDbConnection connection = null;
            try
            {
                connection = DbInteraction.GetConnection(sessionData, Const.CbDbConnectionName, Const.CbDbConnectionString);
            }
            catch (Exception e)
            {
                object[] msgArray = new object[2] { Const.CbDbConnectionString, e.Message } ;
                log.Write(TraceLevel.Warning, "Could not open database at {0}.\n" + "Error Message: {1}", msgArray);
                return IApp.VALUE_FAILURE;
            }
            try
            {
                int affectedRows = DbInteraction.ExecuteNonQuery(builder.ToString(), connection);
                if (affectedRows == 0)
                {
                    if (insertOnUpdateFail)
                    {
                        builder.method = SqlBuilder.Method.INSERT;
                        builder.where.Clear();
                        affectedRows = DbInteraction.ExecuteNonQuery(builder.ToString(), connection);
                    }
                }
                if ((affectedRows != 0) && updateBargeRecord)
                {
                    builder = new SqlBuilder(SqlBuilder.Method.UPDATE, CBargeRecords.TableName);
                    builder.AddFieldValue(CBargeRecords.DirectoryNumber, directoryNumber);
                    builder.AddFieldValue(CBargeRecords.LineInstance, lineInstance);
                    builder.AddFieldValue(CBargeRecords.TimeStamp, timestamp);
                    builder.AddFieldValue(CBargeRecords.DnLi, dnLi);
                    if (conferenceIdSet)
                        builder.AddFieldValue(CBargeRecords.ConferenceId, conferenceId);
                    if (mmsIdSet)
                        builder.AddFieldValue(CBargeRecords.MmsId, mmsId);
                    builder.where[CBargeRecords.DirectoryNumber] = directoryNumber;
                    builder.where[CBargeRecords.LineInstance] = lineInstance;
                    builder.where[CBargeRecords.DnLi] = dnLi;

                    affectedRows = DbInteraction.ExecuteNonQuery(builder.ToString(), connection);
                }

                return (affectedRows != 0) ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
            }
            catch (Exception e)
            {
                object[] msgArray = new object[2] { directoryNumber, e.Message } ;
                log.Write(TraceLevel.Error, "Error encountered in the UpdateCallRecord method, using directoryNumber: {0}\n"+
                    "Error message: {1}", msgArray);
                return IApp.VALUE_FAILURE;
            }
        }
    }
}
