using System;
using Metreos.ApplicationFramework;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.PackageGeneratorCore.Attributes;


namespace Metreos.Native.RemoteAgent
{
    /// <summary>
    /// Updates a call state record in the RemoteAgent.agent_records table.
    /// </summary>
    public class UpdateAgentRecord : INativeAction
    {
        [ActionParamField("Primary key of the record to update.", true)]
        public uint AgentRecordId { set { agentRecordId = value; } }
        private uint agentRecordId;

        [ActionParamField("Specifies whether the current call is being recorded.", true)]
        public bool IsRecorded { set { isRecorded = value; } }
        private bool isRecorded;

        public UpdateAgentRecord()
        {
            Clear(); 
        }

        #region INativeAction Members
        
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [Action("UpdateAgentRecord", false, "UpdateAgentRecord", "Updates a call state record in the RemoteAgent.agent_records table.")]
        public string Execute(SessionData sessionData, Metreos.Core.IConfigUtility configUtility)
        {
            bool success = DatabaseInteraction.UpdateAgentRecord(log, sessionData, agentRecordId, isRecorded);
            return success ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
        }

        public void Clear()
        {
            return;
        }

        public bool ValidateInput()
        {
            return true;
        }

        #endregion
    }
}
