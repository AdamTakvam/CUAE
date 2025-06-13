using System;
using Metreos.ApplicationFramework;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.PackageGeneratorCore.Attributes;


namespace Metreos.Native.RemoteAgent
{
    /// <summary>
    /// Retrieves the RemoteAgent call record for the specified directory number.
    /// </summary>
    public class RetrieveAgentRecord : INativeAction
    {
        [ActionParamField("Directory number of the agent whose current call record we wish to retrieve.", true)]
        public string AgentDN { set { agentDN = value; } }
        private string agentDN;

        [ResultDataField("Primary key for the returned record.")]
        public uint AgentRecordId { get { return agentRecordId; } }
        private uint agentRecordId;

        [ResultDataField("RoutingGuid of the script handling this particular call.")]
        public string RoutingGuid { get { return routingGuid; } }
        private string routingGuid;

        [ResultDataField("Boolean that specifies whether the call in question is being recorded.")]
        public bool IsRecorded { get { return isRecorded; } }
        private bool isRecorded;

        public RetrieveAgentRecord()
        {
            Clear();
        }

        #region INativeAction Members
        
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [Action("RetrieveAgentRecord", false, "RetrieveAgentRecord", "Retrieves the RemoteAgent call record for the specified directory number.")]
        public string Execute(SessionData sessionData, Metreos.Core.IConfigUtility configUtility)
        {
            bool success = DatabaseInteraction.RetrieveAgentRecord(log, sessionData, agentDN, out agentRecordId, out routingGuid, out isRecorded);
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
