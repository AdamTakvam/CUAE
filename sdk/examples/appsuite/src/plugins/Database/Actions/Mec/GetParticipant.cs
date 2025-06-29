using System;
using System.Data;
using System.Diagnostics;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Utilities;
using Metreos.ApplicationSuite.Storage;
using ResultValues = Metreos.ApplicationSuite.Storage.MecParticipants.ResultValuesParticipant;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>Gets all data for a participant record
    /// </summary>
    public class GetParticipant : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("ParticipantId", true)]
        public int ParticipantId { set { participantId = value; } }
        private int participantId;

        [ResultDataField()]
        public int ConferenceId { get { return conferenceId; } }
        private int conferenceId;

        [ResultDataField()]
        public string MmsConferenceId { get { return mmsConferenceId; } }
        private string mmsConferenceId;
        
        [ResultDataField()]
        public string CallId { get { return callId; } }
        private string callId;

        [ResultDataField()]
        public bool IsHost { get { return isHost; } }
        private bool isHost;

        [ResultDataField()]
        public int Status { get { return status; } }
        private int status;

        [ResultDataField()]
        public DateTime FirstConnected { get { return firstConnected; } }
        private DateTime firstConnected;

        [ResultDataField()]
        public DateTime LastConnected { get { return lastConnected; } }
        private DateTime lastConnected;

        [ResultDataField()]
        public DateTime Disconnected { get { return disconnected; } }
        private DateTime disconnected;

        
        public GetParticipant()
        {
            Clear();
        }
 
        public void Clear()
        {
            conferenceId = 0;
            mmsConferenceId = null;
            callId = null;
            isHost = false;
            status = 0;
            firstConnected = DateTime.MinValue;
            lastConnected = DateTime.MinValue;
            disconnected = DateTime.MinValue;
        }
 
        public bool ValidateInput()
        {
            return true;
        }
 
        [ReturnValue(typeof(ResultValues), "")]
        [Action("GetParticipant", false, "Success/Failure", "Gets all data for a specified participant record")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(MecParticipants participants = new MecParticipants(
                      sessionData.DbConnections[SqlConstants.MecConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {

                ResultValues result = participants.GetParticipant(participantId, out conferenceId, out mmsConferenceId,
                    out callId, out isHost, out status, out firstConnected, out lastConnected, out disconnected);

                return result.ToString();
            }
        }
    }
}
