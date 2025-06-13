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
	/// Updates the record for a conference participant(s)
	/// </summary>
	[PackageDecl("Metreos.Native.CBridge")]
	public class UpdateParticipants : INativeAction
	{
		public LogWriter Log { get { return log; } set { log = value; } } 
		private LogWriter log;

		[ActionParamField("LineId", true)]
		public string LineId { set { lineId = value; } }
		private string lineId;

		[ActionParamField("From", true)]
		public string From { set { from = value; } }
		private string from;

		[ActionParamField("CallId", false)]
		public string CallId { set { callId = value; } }
		private string callId;

		[ActionParamField("Timestamp", true)]
		public DateTime Timestamp { set { timestamp = value; } }
		private DateTime timestamp;

		[ActionParamField("IsRecorded", true)]
		public bool IsRecorded { set { isRecorded = value; } }
		private bool isRecorded = false;
		
		[ActionParamField("IsModerator", true)]
		public bool IsModerator { set { isModerator = value; } }
		private bool isModerator = false;

		[ActionParamField("IsMuted", true)]
		public bool IsMuted { set { isMuted = value; } }
		private bool isMuted = false;
 
		public UpdateParticipants()
		{
			Clear();
		}
 
		public void Clear()
		{
			lineId = from = callId = null;
		}

		public bool ValidateInput()
		{
			return true;
		}
 
		[Action("UpdateParticipants", false, "Updates participant record", "Updates record for participant with given lineId, timestamp, and callId (if specified)")]
		[ReturnValue(typeof(Const.ConferenceReturnValues), "Valid values are: success, failure, NotFound")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
		{            
			if (lineId == null)
                return ReturnValues.failure.ToString();

            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.UPDATE, CBridgeParticipantsTable.TableName);
			builder.AddFieldValue(CBridgeParticipantsTable.TimeStamp, timestamp);
			builder.AddFieldValue(CBridgeParticipantsTable.LineId, lineId);
			builder.AddFieldValue(CBridgeParticipantsTable.FromNumber, from);
			builder.AddFieldValue(CBridgeParticipantsTable.IsRecorded, isRecorded);
			builder.AddFieldValue(CBridgeParticipantsTable.IsModerator, isModerator);
			builder.AddFieldValue(CBridgeParticipantsTable.IsMuted, isMuted);
            if (callId != null)
            {
                builder.AddFieldValue(CBridgeParticipantsTable.CallId, callId);
                builder.where[CBridgeParticipantsTable.CallId] = callId;
            }

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
				log.Write(TraceLevel.Error, "Error encountered in the UpdateParticipants method, using lineId: {0}\n"+
					"Error message: {1}", msgArray);
				return ReturnValues.failure.ToString();
			}
		}
	}
}
*/