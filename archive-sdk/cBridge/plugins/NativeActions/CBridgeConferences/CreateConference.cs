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

namespace Metreos.Native.CBridge
{
	/// <summary>
	/// Creates the record for the newest conference associated with a line, returns time stamp
	/// </summary>
	[PackageDecl("Metreos.Native.CBridge")]
	public class CreateConference : INativeAction
	{
		public LogWriter Log { get { return log; } set { log = value; } } 
		private LogWriter log;

		[ActionParamField("LineId", true)]
		public string LineId { set { lineId = value; } }
		private string lineId;

		[ActionParamField("IsRecorded", false)]
		public bool IsRecorded { set { isRecorded = value; } }
		private bool isRecorded = false;

        [ActionParamField("RoutingGuid", true)]
        public string RoutingGuid { set { routingGuid = value; } }
        private string routingGuid;

		[ResultDataField("The timestamp of this conference")]
		public long Timestamp { get { return timestamp; } }
		private long timestamp;

		public CreateConference()
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
 
		[Action("CreateConference", false, "Creates Conference record", "Creates Conference record, associates it with LineId and a time stamp")]
		public string Execute(SessionData sessionData, IConfigUtility configUtility)
		{            
			timestamp = System.DateTime.Now.Ticks;
			SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.INSERT, CBridgeTable.TableName);
			builder.AddFieldValue(CBridgeTable.LineId, lineId);
			builder.AddFieldValue(CBridgeTable.TimeStamp, timestamp);
			builder.AddFieldValue(CBridgeTable.RoutingGuid, routingGuid);
            builder.AddFieldValue(CBridgeTable.IsRecorded, isRecorded);

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
				log.Write(TraceLevel.Error, "Error encountered in the CreateConference method, using lineId: {0}\n"+
					"Error message: {1}", msgArray);
				return IApp.VALUE_FAILURE;
			}
		}
	}
}
