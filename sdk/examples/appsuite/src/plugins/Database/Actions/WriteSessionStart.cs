using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using ReturnValues = Metreos.ApplicationSuite.Storage.SessionRecords.WriteSessionStartResultValues;
using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary> Writes a new session start record. </summary>
	public class WriteSessionStart : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ResultDataField("The ID of the new session record.")]
        public  uint SessionRecordsId { get { return sessionRecordsId; } }
        private uint sessionRecordsId;

        [ActionParamField("ID of the authentication record attached to this session.", true)]
        public  uint AuthRecordsId { set { authRecordsId = value; } }
        private uint authRecordsId;

		public WriteSessionStart()
		{
            Clear();
		}

        #region INativeAction Implementation

        public bool ValidateInput()
        {
            if(authRecordsId == 0) return false;

            return true;
        }

        public void Clear()
        {
            sessionRecordsId    = 0;
            authRecordsId       = 0;
        }

        [Action("WriteSessionStart", false, "Write Session Start", "Writes a new session start record.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(SessionRecords sessionRecordsDbAccess = new SessionRecords(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                ReturnValues result = sessionRecordsDbAccess.WriteCallSessionStart(authRecordsId, out sessionRecordsId);

                // Check that a valid session Record Id is given to us
                if(sessionRecordsId < SqlConstants.StandardPrimaryKeySeed)
                {
                    sessionRecordsId = 0; // Cast to uint in SessionRecordsId property must not throw exception
                
                }
            
                return result.ToString();
            }
        }

        #endregion
	}
}
