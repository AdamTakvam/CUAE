using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary> Associates scheduled conference id with call record. </summary>
	public class SchedConfIdIntoCallRecord : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;
        
        [ActionParamField("ID of the scheduled conference record to associate to.", true)]
        public  uint ScheduledConferenceId { set { scheduledConferenceId = value; } }
        private uint scheduledConferenceId;

        [ActionParamField("ID of the call record to associate with.", true)]
        public  uint CallRecordsId { set { callRecordsId = value; } }
        private uint callRecordsId;

		public SchedConfIdIntoCallRecord()
		{
            Clear();
		}

        public bool ValidateInput()
        {
            if(scheduledConferenceId    == new uint()) return false;
            if(callRecordsId            == new uint()) return false;

            return true;
        }

        public void Clear()
        {
            scheduledConferenceId   = new uint();
            callRecordsId           = new uint();
        }

        [Action("SchedConfIdIntoCallRecord", false, "SchedConfId Into CallRecord", "Associates scheduled conference id with call record.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(CallRecords callRecordsDbAccess = new CallRecords(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {

                bool success = callRecordsDbAccess.AssociateScheduledConferenceId(
                    (int) callRecordsId, 
                    (int) scheduledConferenceId);

                if(success) return IApp.VALUE_SUCCESS;
                else        return IApp.VALUE_FAILURE;
            }
        }
	}
}
