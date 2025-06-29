using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary> Closes out an existing session record. </summary>
	public class WriteSessionStop : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("ID of the session record to close out.", true)]
        public  uint SessionRecordsId { set { sessionRecordsId = value; } }
        private uint sessionRecordsId;

		public WriteSessionStop()
		{
            Clear();
		}

        #region INativeAction Implementation

        public bool ValidateInput()
        {
            if(sessionRecordsId == new uint()) return false;

            return true;
        }

        public void Clear()
        {
            sessionRecordsId = new uint();
        }

        [Action("WriteSessionStop", false, "Write Session Stop", "Closes out an existing session record.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(SessionRecords sessionRecordsDbAccess = new SessionRecords(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                bool success = sessionRecordsDbAccess.WriteCallSessionStop(sessionRecordsId);

                // Check that a valid session Record Id is given to us
                if(sessionRecordsId < SqlConstants.StandardPrimaryKeySeed)
                {
                    sessionRecordsId = 0; // Cast to uint in SessionRecordsId property must not throw exception
                    success = false;  
                }
            
                if(success) return IApp.VALUE_SUCCESS;
                else        return IApp.VALUE_FAILURE;
            }
        }

        #endregion
	}
}
