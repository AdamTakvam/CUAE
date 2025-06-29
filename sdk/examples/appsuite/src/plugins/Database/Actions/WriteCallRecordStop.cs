using System;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary> Closes out an existing call detail record. </summary>
	public class WriteCallRecordStop : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("ID of the call to close out.", true)]
        public  uint CallRecordsId { set { callRecordsId = value; } }
        private uint callRecordsId;

        [ActionParamField("Reason the call was ended.", true)]
        public  Storage.EndReason EndReason { set { endReason = value; } }
        private Storage.EndReason endReason;

        public WriteCallRecordStop()
		{
            Clear();
		}

        #region INativeAction Implementation

        public bool ValidateInput()
        {
            //if(callRecordsId == new uint()) return false;
            //if(endReason     == Storage.EndReason.Invalid) return false;
            return true;
        }

        public void Clear()
        {
            callRecordsId = new uint();
            endReason     = Storage.EndReason.Invalid;
        }

        [Action("WriteCallRecordStop", false, "Write Call Record Stop", "Closes out an existing call detail record.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            if(callRecordsId == new uint())
            {
                log.Write(TraceLevel.Warning, "WriteCallRecordStop:  invalid call record ID");
                return IApp.VALUE_FAILURE;
            }

            using(CallRecords callRecordsDbAccess = new CallRecords(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                bool success = callRecordsDbAccess.WriteStop((int) callRecordsId, endReason);

                if(success) return IApp.VALUE_SUCCESS;
                else        return IApp.VALUE_FAILURE;
            }
        }

        #endregion
	}
}
