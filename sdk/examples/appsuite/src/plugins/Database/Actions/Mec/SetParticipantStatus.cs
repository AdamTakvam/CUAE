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
    /// <summary>Set participant status</summary>
    public class SetParticipantStatus : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("ParticipantId", true)]
        public int ParticipantId { set { participantId = value; } }
        private int participantId;

        [ActionParamField("Status", true)]
        public MecParticipantStatus Status { set { status = value; } }
        private MecParticipantStatus status;


        public SetParticipantStatus()
        {
            Clear();
        }
 
        public void Clear()
        {
            participantId = 0;
            status = MecParticipantStatus.None;
        }
 
        public bool ValidateInput()
        {
            return true;
        }
 
        [ReturnValue(typeof(ResultValues), "")]
        [Action("SetParticipantStatus", false, "Success/Failure", "Sets the status for participant")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(MecParticipants participants = new MecParticipants(
                      sessionData.DbConnections[SqlConstants.MecConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {

                ResultValues result = participants.SetParticipantStatus(participantId, status);
                return result.ToString();
            }
        }
    }
}
