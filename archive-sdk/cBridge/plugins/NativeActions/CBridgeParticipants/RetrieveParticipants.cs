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
	/// Retrieves participants associated with given lineId and timestamp, and callId (if specified)
	/// </summary>
	[PackageDecl("Metreos.Native.CBridge")]
	public class RetrieveParticipants : INativeAction
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

        [ActionParamField("IsRecorded", false)]
        public bool IsRecorded { set { isRecorded = value; isRecordedSet = true;} }
        private bool isRecorded;
		private bool isRecordedSet = false;

        [ActionParamField("IsModerator", false)]
        public bool IsModerator { set { isModerator = value; isModeratorSet = true; } }
        private bool isModerator;
        private bool isModeratorSet = false;

        [ActionParamField("IsMuted", false)]
        public bool IsMuted { set { isMuted = value; isMutedSet = true; } }
        private bool isMuted;
        private bool isMutedSet = false;

        [ResultDataField("Collection of matching records")]
        public BridgeParticipants Participants { get { return participants; } }
        private BridgeParticipants participants;
 
		public RetrieveParticipants()
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
 
		[Action("RetrieveParticipants", false, "Retrieves a BridgeParticipants collection of participants", "If callId is given, only matches the records with that callId, optionally matching the Is* conditionals")]
		[ReturnValue(typeof(Const.ConferenceReturnValues), "Valid values are: success, failure, NotFound")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
		{            
			participants = new BridgeParticipants();

            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.SELECT, CBridgeParticipantsTable.TableName);
            builder.where[CBridgeParticipantsTable.LineId] = lineId;
            builder.where[CBridgeParticipantsTable.TimeStamp] = timestamp;

			if (callId != null)
			    builder.where[CBridgeParticipantsTable.CallId] = callId;
			
            if (isModeratorSet)
                builder.where[CBridgeParticipantsTable.IsModerator] = isModerator;

            if (isRecordedSet)
                builder.where[CBridgeParticipantsTable.IsRecorded] = isRecorded;
            
            if (isMutedSet)
                builder.where[CBridgeParticipantsTable.IsMuted] = isMuted;

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
                DataTable table = DbInteraction.ExecuteQuery(builder.ToString(), connection);
                
                if (table == null || table.Rows.Count == 0)
                {
                    return ReturnValues.NotFound.ToString();
                }

                foreach (DataRow row in table.Rows)
                {
                    Participant participant = new Participant();
                    participant.From = Convert.ToString(row[CBridgeParticipantsTable.FromNumber]);
                    participant.IsModerator = Convert.ToBoolean(row[CBridgeParticipantsTable.IsModerator]);
                    participant.IsMuted = Convert.ToBoolean(row[CBridgeParticipantsTable.IsMuted]);
                    participant.IsRecorded = Convert.ToBoolean(row[CBridgeParticipantsTable.IsRecorded]);
                    participant.LineId = lineId;
                    participant.CallId = Convert.ToString(row[CBridgeParticipantsTable.CallId]);
                    participant.Timestamp = timestamp;
                    participants.Add(participant);
                }       

                return ReturnValues.success.ToString();
			}
			catch (Exception e)
			{
				object[] msgArray = new object[2] { lineId, e.Message } ;
				log.Write(TraceLevel.Error, "Error encountered in the RetrieveParticipants method, using lineId: {0}\n"+
					"Error message: {1}", msgArray);
				return ReturnValues.failure.ToString();
			}
		}
	}
}
*/