using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary> Writes a new recording start record. </summary>
	public class WriteRecordingStart : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The type of the media file being saved.", true)]
        public  MediaFileType MediaType { get { return mediaType; } set { mediaType = value; } }
        private MediaFileType mediaType;

        [ActionParamField("The call record ID associated with this recording.", true)]
        public  uint CallRecordsId { get { return callRecordsId; } set { callRecordsId = value; } }
        private uint callRecordsId;
        
        [ActionParamField("The user ID associated with this recording.", true)]
        public  uint UsersId { get { return usersId; } set { usersId = value; } }
        private uint usersId;

        [ResultDataField("ID of the new recording record.")]
        public  uint RecordingsId { get{ return recordingsId; } set { recordingsId = value; } }
        private uint recordingsId;

		public WriteRecordingStart()
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
            usersId         = 0;
            recordingsId    = 0;
            callRecordsId   = 0;
            mediaType       = MediaFileType.Other;
        }

        [Action("WriteRecordingStart", false, "Write Session Start", "Writes a new session start record.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(Recordings recordingsDbAccess = new Recordings(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {

                bool success = recordingsDbAccess.WriteRecordingStart(callRecordsId, usersId, mediaType, out recordingsId);

                // Check that a valid session Record Id is given to us
                if(recordingsId == 0)
                {
                    success = false;  
                }
            
                if(success) return IApp.VALUE_SUCCESS;
                else        return IApp.VALUE_FAILURE;  
            }
        }

        #endregion
	}
}
