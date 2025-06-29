using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationSuite.Storage;
using ReturnValues = Metreos.ApplicationSuite.Storage.ScheduledConferences.GetConferenceReturnValues;

namespace Metreos.ApplicationSuite.Actions
{
	/// <summary>Get conference properties given (participant) ID</summary>
	public class GetConference : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("Participant conference PIN.", true)]
		public  uint ConferencePin { set { conferencePin = value; } }
		private uint conferencePin;

		[ResultDataField("Media server ID.")]
		public	uint MmsId { get { return mmsId; } }
		private uint mmsId;
		
		[ResultDataField("Media server conference ID.")]
		public	uint MmsConfId	{ get { return mmsConfId; } }
		private uint mmsConfId;
		
		[ResultDataField("Date/time of conference.")]
		public	DateTime ScheduledTime	{ get { return scheduledTime; } }
		private DateTime scheduledTime;
		
		[ResultDataField("Expected duration in minutes.")]
		public	uint DurationMinutes { get { return durationMinutes; } }
		private uint durationMinutes;

		[ResultDataField("Expected number of participants.")]
		public	uint Participants { get { return participants; } }
		private uint participants;
        
        [ResultDataField("Indicates whether a user is the host")]
        public  bool IsHost { get { return isHost; } }
        private bool isHost;

        [ResultDataField("The key to this entry in the scheduled conference table")]
        public  uint ScheduledConferenceId { get { return scheduledConferenceId; } }
        private uint scheduledConferenceId;

        [ResultDataField("Conference pin assigned to the host")]
        public  uint HostConferencePin { get { return hostConferencePin; } }
        private uint hostConferencePin;

		public GetConference()
		{
			Clear();
		}

		#region INativeAction Implementation

		public bool ValidateInput()
		{
			return true;
		}

		public void Clear()
		{
            isHost = false;
            scheduledTime = DateTime.MinValue;
			conferencePin  = scheduledConferenceId = mmsId = mmsConfId = durationMinutes = participants = 0;			 
		}

        [ReturnValue(typeof(ReturnValues), "")]
		[Action("GetConference", false, "Get Conference", "Retrieves conference by ID.")]
		public string Execute(SessionData sessionData, IConfigUtility configUtility)
		{
            using(ScheduledConferences schedConf = new ScheduledConferences(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {

                ReturnValues result = schedConf.GetConferenceInfo(
                    conferencePin, 
                    out scheduledConferenceId, 
                    out mmsId,
                    out mmsConfId,
                    out scheduledTime,
                    out durationMinutes,
                    out participants,
                    out isHost,
                    out hostConferencePin);

                return result.ToString();
            }
        }

		#endregion
	}	// class GetConference
}
