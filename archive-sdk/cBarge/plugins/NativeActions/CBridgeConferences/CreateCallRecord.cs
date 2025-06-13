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

namespace Metreos.Native.CBarge
{
	/// <summary>
	/// Creates the record for the newest conference associated with a line, returns time stamp
	/// </summary>
	[PackageDecl("Metreos.Native.CBarge")]
	public class CreateCallRecord : INativeAction
	{
		public LogWriter Log { get { return log; } set { log = value; } } 
		private LogWriter log;

		[ActionParamField("CallId", true)]
		public long CallId { set { callId = value; } }
		private long callId;

        [ActionParamField("LineId", true)]
        public string LineId { set { lineId = value; } }
        private string lineId;

        [ActionParamField("RoutingGuid", true)]
        public string RoutingGuid { set { routingGuid = value; } }
        private string routingGuid;

        [ActionParamField("New DeviceName of the device to be associated with this call record", true)]
        public string DeviceName { set { deviceName = value; } }
        private string deviceName;

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
 
		[Action("CreateCallRecord", false, "Creates call record", "Creates call record, associates it with CallId, LineId, RoutingGuid, and a time stamp")]
		public string Execute(SessionData sessionData, IConfigUtility configUtility)
		{            
			timestamp = DateTime.Now;
			SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.INSERT, CBargeCallRecords.TableName);
			builder.AddFieldValue(CBargeCallRecords.LineId, lineId);
            builder.AddFieldValue(CBargeCallRecords.CallId, callId);
            builder.AddFieldValue(CBargeCallRecords.DeviceName, deviceName);
			builder.AddFieldValue(CBargeCallRecords.TimeStamp, timestamp);
			builder.AddFieldValue(CBargeCallRecords.RoutingGuid, routingGuid);

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
                
				return (affectedRows == 0) ? IApp.VALUE_FAILURE : IApp.VALUE_SUCCESS;
			}
			catch (Exception e)
			{
				object[] msgArray = new object[2] { lineId, e.Message } ;
				log.Write(TraceLevel.Error, "Error encountered in the CreateCallRecord method, using lineId: {0}\n"+
					"Error message: {1}", msgArray);
				return IApp.VALUE_FAILURE;
			}
		}
	}
}
