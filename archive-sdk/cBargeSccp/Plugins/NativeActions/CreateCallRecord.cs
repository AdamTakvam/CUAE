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
	/// Creates the record for the newest conference associated with a line, returns time stamp
	/// </summary>
	public class CreateCallRecord : INativeAction
	{
		public LogWriter Log { get { return log; } set { log = value; } } 
		private LogWriter log;

		[ActionParamField("CallReference", true)]
		public int CallReference { set { callReference = value; } }
		private int callReference;

        [ActionParamField("CallInstance", true)]
        public int CallInstance { set { callInstance = value; } }
        private int callInstance;

        [ActionParamField("LineInstance", true)]
        public int LineInstance { set { lineInstance = value; } }
        private int lineInstance;

        [ActionParamField("DirectoryNumber", true)]
        public string DirectoryNumber { set { directoryNumber = value; } }
        private string directoryNumber;

        [ActionParamField("RoutingGuid", true)]
        public string RoutingGuid { set { routingGuid = value; } }
        private string routingGuid;

        [ActionParamField("Sid of the device to be associated with this call record", true)]
        public string Sid { set { sid = value; } }
        private string sid;

        [ActionParamField("MmsId on which the conference resides", true)]
        public uint MmsId { set { mmsId = value; } }
        private uint mmsId;

        [ActionParamField("ConferenceId", true)]
        public string ConferenceId { set { conferenceId = value; } }
        private string conferenceId;

        [ActionParamField("WriteBargeRecord", true)]
        public bool WriteBargeRecord { set { writeBargeRecord = value; } }
        private bool writeBargeRecord;

		[ResultDataField("The timestamp of this conference")]
		public DateTime Timestamp { get { return timestamp; } }
		private DateTime timestamp;

		public CreateCallRecord()
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
 
		[Action("CreateCallRecord", false, "Creates call record", "Creates call record, associates it with CallReference, DirectoryNumber, RoutingGuid, and a time stamp")]
		public string Execute(SessionData sessionData, IConfigUtility configUtility)
		{            
			timestamp = DateTime.Now;
            string dnLi = directoryNumber + ":" + lineInstance.ToString();
			SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.INSERT, CBargeCallRecords.TableName);
            builder.AddFieldValue(CBargeCallRecords.DirectoryNumber, directoryNumber);
            builder.AddFieldValue(CBargeCallRecords.CallReference, callReference);
            builder.AddFieldValue(CBargeCallRecords.Sid, sid);
			builder.AddFieldValue(CBargeCallRecords.TimeStamp, timestamp);
			builder.AddFieldValue(CBargeCallRecords.RoutingGuid, routingGuid);
            builder.AddFieldValue(CBargeCallRecords.CallInstance, callInstance);
            builder.AddFieldValue(CBargeCallRecords.LineInstance, lineInstance);
            builder.AddFieldValue(CBargeCallRecords.MmsId, mmsId);
            builder.AddFieldValue(CBargeCallRecords.ConferenceId, conferenceId);
            builder.AddFieldValue(CBargeCallRecords.DnLi, dnLi);

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
                if ((affectedRows > 0) && writeBargeRecord)
                {
                    builder = new SqlBuilder(SqlBuilder.Method.UPDATE, CBargeRecords.TableName);
                    builder.AddFieldValue(CBargeRecords.ConferenceId, conferenceId);
                    builder.AddFieldValue(CBargeRecords.NumberParticipants, CBargeRecords.DefaultParticipants);
                    builder.AddFieldValue(CBargeRecords.MmsId, mmsId);
                    builder.AddFieldValue(CBargeRecords.TimeStamp, timestamp);
                    builder.AddFieldValue(CBargeRecords.DirectoryNumber, directoryNumber);
                    builder.AddFieldValue(CBargeRecords.LineInstance, lineInstance);
                    builder.AddFieldValue(CBargeRecords.DnLi, dnLi);
                    builder.where[CBargeRecords.DirectoryNumber] = directoryNumber;
                    builder.where[CBargeRecords.LineInstance] = lineInstance;
                    builder.where[CBargeRecords.DnLi] = dnLi;


                    affectedRows = DbInteraction.ExecuteNonQuery(builder.ToString(), connection);
                    // The update may have failed if the record has never been created before, in which case we
                    // should try inserting a new one. 
                    if (affectedRows == 0)
                    {
                        builder.method = SqlBuilder.Method.INSERT;
                        builder.where.Clear();
                        affectedRows = DbInteraction.ExecuteNonQuery(builder.ToString(), connection);
                    }
                }

                return (affectedRows != 0) ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
			}
			catch (Exception e)
			{
                object[] msgArray = new object[2] { directoryNumber, e.Message } ;
				log.Write(TraceLevel.Error, "Error encountered in the CreateCallRecord method, using directoryNumber: {0}\n"+
					"Error message: {1}", msgArray);
				return IApp.VALUE_FAILURE;
			}
		}
	}
}
