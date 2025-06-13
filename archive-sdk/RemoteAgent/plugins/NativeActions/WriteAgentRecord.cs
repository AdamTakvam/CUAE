using System;
using Metreos.ApplicationFramework;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.PackageGeneratorCore.Attributes;


namespace Metreos.Native.RemoteAgent
{
	/// <summary>
	/// Creates a call state record in the RemoteAgent.agent_records table.
	/// </summary>
    public class WriteAgentRecord : INativeAction
    {
        [ActionParamField("Directory number of the agent.", true)]
        public string AgentDN { set { agentDN = value; } }
        private string agentDN;
        
        [ActionParamField("RoutingGuid of the script handling this particular call.", true)]
        public string RoutingGuid { set { routingGuid = value; } }
        private string routingGuid;

        [ActionParamField("Specifies whether the current call is being recorded.", true)]
        public bool IsRecorded { set { isRecorded = value; } }
        private bool isRecorded;

        [ResultDataField("Primary key for the newly created record.")]
        public uint AgentRecordId { get { return agentRecordId; } }
        private uint agentRecordId;

		public WriteAgentRecord()
		{
            Clear();
        }

        #region INativeAction Members
        
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [Action("WriteAgentRecord", false, "WriteAgentRecord", "Creates a call state record in the RemoteAgent.agent_records table.")]
        public string Execute(SessionData sessionData, Metreos.Core.IConfigUtility configUtility)
        {
            bool success = DatabaseInteraction.WriteAgentRecord(log, sessionData, agentDN, routingGuid, isRecorded, out agentRecordId);
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
