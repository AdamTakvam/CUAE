using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>
    /// RRetrieves RemoteAgent User Level for given userId.
    /// </summary>
    public class GetRemoteAgentLevel : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("UserId of user whose RemoteAgent User Level we wish to know.", true)]
        public uint UserId 
        { 
            set { userId = value; } 
        }
        private uint userId;

        [ResultDataField("RemoteAgent User Level for user associated with record.")]
        public uint UserLevel 
        { 
            get { return userLevel; } 
        }
        private uint userLevel;

        public GetRemoteAgentLevel()
        {
            Clear();
        }

        [Action("GetRemoteAgentLevel", false, "GetRemoteAgentLevel", "Retrieves RemoteAgent User Level for given userId.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            RemoteAgents remoteAgentsDbAccess = new RemoteAgents(
                sessionData.DbConnections[SqlConstants.DbConnectionName],
                log,
                sessionData.AppName,
                sessionData.PartitionName,
                DbTable.DetermineAllowWrite(sessionData.CustomData));

            bool success = remoteAgentsDbAccess.GetRemoteAgentLevel(userId, out userLevel);
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
    }
}
