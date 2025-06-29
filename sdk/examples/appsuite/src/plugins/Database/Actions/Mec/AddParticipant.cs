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
using ResultValues = Metreos.ApplicationSuite.Storage.MecParticipants.ResultValuesAddParticipant;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>Adds a new participant
    /// </summary>
    public class AddParticipant : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("ConferenceId", true)]
        public int ConferenceId { set { conferenceId = value; } }
        private int conferenceId;

        [ActionParamField("MmsConnectionId", true)]
        public string MmsConnectionId { set { mmsConnectionId = value; } }
        private string mmsConnectionId;

        [ActionParamField("CallId", true)]
        public string CallId { set { callId = value; } }
        private string callId;

        [ActionParamField("PhoneNumber", true)]
        public string PhoneNumber { set { phoneNumber = value; } }
        private string phoneNumber;
        
        [ActionParamField("Description", true)]
        public string Description { set { description = value; } }
        private string description;

        [ActionParamField("IsHost", true)]
        public bool IsHost { set { isHost = value; } }
        private bool isHost;

        [ResultDataField()]
        public int ParticipantId { get { return participantId; } }
        private int participantId;

        public AddParticipant()
        {
            Clear();
        }
 
        public void Clear()
        {
            conferenceId = 0;
            mmsConnectionId = null;
            callId = null;
            phoneNumber = null;
            description = null;
            isHost = false;
            participantId = 0;
        }
 
        public bool ValidateInput()
        {
            return true;
        }
 
        [ReturnValue(typeof(ResultValues), "")]
        [Action("AddParticipant", false, "Success/Failure", "Gets all data for a specified participant record")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(MecParticipants participants = new MecParticipants(
                      sessionData.DbConnections[SqlConstants.MecConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {

                ResultValues result = participants.AddParticipant(conferenceId, mmsConnectionId, callId, phoneNumber, description, isHost, out participantId);
                return result.ToString();
            }
        }
    }
}
