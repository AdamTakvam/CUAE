using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary> Writes a new recording stop record. </summary>
	public class WriteRecordingStop : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The type of the media file being saved.")]
        public  uint RecordingsId { get { return recordingsId; } set { recordingsId = value; } }
        private uint recordingsId;

        [ActionParamField("The type of the media file being saved.")]
        public  string Filepath { get { return filepath; } set { filepath = value; } }
        private string filepath;

		public WriteRecordingStop()
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
            filepath        = null;
        }

        [Action("WriteRecordingStop", false, "Write Session Stop", "Writes a new session stop record.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(Recordings recordingsDbAccess = new Recordings(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {

                bool success = recordingsDbAccess.WriteRecordingStop(recordingsId, filepath);
            
                if(success) return IApp.VALUE_SUCCESS;
                else        return IApp.VALUE_FAILURE;  
            }
        }

        #endregion
	}
}
