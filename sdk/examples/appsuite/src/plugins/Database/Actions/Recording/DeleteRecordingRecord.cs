using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary> Deletes a recording record. </summary>
	public class DeleteRecordingRecord : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The type of the media file being saved.")]
        public  uint RecordingsId { get { return recordingsId; } set { recordingsId = value; } }
        private uint recordingsId;

		public DeleteRecordingRecord()
		{
            Clear();
		}

        #region INativeAction Implementation

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            recordingsId    = 0;
        }

        [Action("DeleteRecordingRecord", false, "Delete Recording Record", "Deletes a recording record.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(Recordings recordingsDbAccess = new Recordings(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {

                bool success = recordingsDbAccess.DeleteRecording(recordingsId);
            
                if(success) return IApp.VALUE_SUCCESS;
                else        return IApp.VALUE_FAILURE;  
            }
        }

        #endregion
	}
}
