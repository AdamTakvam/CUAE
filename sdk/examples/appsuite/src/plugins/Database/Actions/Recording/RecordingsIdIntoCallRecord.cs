using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary> Associates recordingsId with call record. </summary>
	public class RecordingsIdIntoCallRecord : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("ID of the recording record to associate to.", true)]
        public  uint RecordingsId { set { recordingsId = value; } }
        private uint recordingsId;

        [ActionParamField("ID of the call record to associate with.", true)]
        public  uint CallRecordsId { set { callRecordsId = value; } }
        private uint callRecordsId;

		public RecordingsIdIntoCallRecord()
		{
            Clear();
		}

        #region INativeAction Implementation

        public bool ValidateInput()
        {
            if(recordingsId  == 0) return false;
            if(callRecordsId == 0) return false;

            return true;
        }

        public void Clear()
        {
            recordingsId  = 0;
            callRecordsId = 0;
        }

        [Action("RecordingsIdIntoCallRecord", false, "RecordingsId Into CallRecord", "Associates recordingsId with call record.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(CallRecords callRecordsDbAccess = new CallRecords(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {

                bool success = callRecordsDbAccess.AssociateRecordingsId(
                    callRecordsId, 
                    recordingsId);

                if(success) return IApp.VALUE_SUCCESS;
                else        return IApp.VALUE_FAILURE;
            }
        }

        #endregion
	}
}
