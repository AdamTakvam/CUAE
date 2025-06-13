using System;
using Metreos.ApplicationFramework;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.PackageGeneratorCore.Attributes;


namespace Metreos.Native.RemoteAgent
{
    /// <summary>
    /// Removes a call state record with the given AgentRecordId from the RemoteAgent.agent_records table.
    /// </summary>
    public class RemoveAgentRecord : INativeAction
    {
        [ActionParamField("AgentRecordId of the record to delete.", true)]
        public uint AgentRecordId { set { agentRecordId = value; } }
        private uint agentRecordId;

        public RemoveAgentRecord()
        {
            Clear();
        }

        #region INativeAction Members
        
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [Action("RemoveAgentRecord", false, "RemoveAgentRecord", "Removes a call state record with the given AgentRecordId from the RemoteAgent.agent_records table.")]
        public string Execute(SessionData sessionData, Metreos.Core.IConfigUtility configUtility)
        {
            bool success = DatabaseInteraction.RemoveAgentRecord(log, sessionData, agentRecordId);
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
