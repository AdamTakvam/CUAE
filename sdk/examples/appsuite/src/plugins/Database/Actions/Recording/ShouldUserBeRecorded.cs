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
    ///     Determines if a user should be recorded and if he/she 
    ///     should be aware of it 
    /// </summary>
	public class ShouldUserBeRecorded : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("ID of the User to check recording status of.", true)]
        public  uint UsersId { set { usersId = value; } }
        private uint usersId;

        [ResultDataField("Indicates whether or not the user shold be recorded")]
        public bool Record { get { return record; } }
        private bool record;

        [ResultDataField("Indicates whether or not the user shold be aware if they are to be aware of being recorded")]
        public bool RecordingVisible { get { return recordingVisible; } }
        private bool recordingVisible;

		public ShouldUserBeRecorded()
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
            usersId = 0;
            record = false;
            recordingVisible = false;
        }

        [Action("ShouldUserBeRecorded", false, "Should User Be Recorded", "Determines if a user should be recorded and if he/she should be aware of it ")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(Users usersDbAccess = new Users(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {

                bool success = usersDbAccess.ShouldUserBeRecorded(usersId, out record, out recordingVisible);
            
                if(success) return IApp.VALUE_SUCCESS;
                else        return IApp.VALUE_FAILURE;
            }
        }

        #endregion
	}
}
