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
	/// Retrieves RemoteAgent information associated with specified device.
	/// </summary>
	public class GetRemoteAgentByDevice : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("DeviceName of the device we're retrieving the information for.", true)]
        public string DeviceName 
        { 
            set { deviceName = value; } 
        }
        private string deviceName;

        [ResultDataField("Id of RemoteAgent DB record")]
        public uint RemoteAgentId 
        { 
            get { return remoteAgentId; } 
        }
        private uint remoteAgentId;

        [ResultDataField("UserId of user associated with retrieved record")]
        public uint UserId 
        { 
            get { return userId; } 
        }
        private uint userId;

        [ResultDataField("RemoteAgent User Level for user associated with record")]
        public uint UserLevel 
        { 
            get { return userLevel; } 
        }
        private uint userLevel;

        [ResultDataField("The external number that RemoteAgent is to dial.")]
        public string ExternalNumber 
        { 
            get { return externalNumber; } 
        }
        private string externalNumber;

        public GetRemoteAgentByDevice()
        {
            Clear();
        }

        [Action("GetRemoteAgentByDevice", false, "GetRemoteAgentByDevice", "Retrieves RemoteAgent information based on provided device name.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(RemoteAgents remoteAgentsDbAccess = new RemoteAgents(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {

                bool success = remoteAgentsDbAccess.GetRemoteAgentByDevice(deviceName, out remoteAgentId, out userId, out externalNumber, out userLevel);
                return success ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;                                                
            }
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
