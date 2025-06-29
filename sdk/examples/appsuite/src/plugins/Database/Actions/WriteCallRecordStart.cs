using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary> Writes a new call detail record. </summary>
	public class WriteCallRecordStart : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ResultDataField("The ID of the new call record.")]
        public  uint CallRecordsId { get { return callRecordsId; } }
        private uint callRecordsId;

        [ActionParamField("Originating number.", true)]
        public  string OriginNumber { set { originNumber = value; } }
        private string originNumber;

        [ActionParamField("Destination number.", true)]
        public  string DestinationNumber { set { destinationNumber = value; } }
        private string destinationNumber;

        [ActionParamField("ID of the session containing this call.", false)]
        public  uint SessionRecordsId { set { sessionRecordsId = value; } }
        private uint sessionRecordsId;

        [ActionParamField("ID of the user placing this call.", false)]
        public  uint UserId { set { userId = value; } }
        private uint userId;

        [ActionParamField("ID of an authentication record for this call.", false)]
        public  uint AuthRecordsId { set { authRecordsId = value; } }
        private uint authRecordsId;

        public WriteCallRecordStart()
        {
            Clear();
        }

        #region INativeAction Implementation

        public bool ValidateInput()
        {
            if((originNumber == null)      || (originNumber.Length == 0))       return false;
            if((destinationNumber == null) || (destinationNumber.Length == 0))  return false;

            return true;
        }

        public void Clear()
        {
            callRecordsId           = 0;
            originNumber            = null;
            destinationNumber       = null;
            sessionRecordsId        = 0;
            userId                  = 0;
            authRecordsId           = 0;
        }

        [Action("WriteCallRecordStart", false, "Write Call Record Start", "Writes a new call detail record.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(Users userDbAccess = new Users(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                userDbAccess.StartCall(userId);
            }

            using(CallRecords callRecordsDbAccess = new CallRecords(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                string scriptName = log.LogName;
                if (scriptName == null || scriptName == string.Empty)
                    scriptName = "UNAVAILABLE";
                else
                {
                    int indexOfDash = scriptName.LastIndexOf('-');
                    if (indexOfDash > 0)
                        scriptName = scriptName.Substring(0, indexOfDash);
                }


                bool success = callRecordsDbAccess.WriteStart(
                    userId,
                    sessionRecordsId,
                    originNumber,
                    destinationNumber, 
                    scriptName,
                    authRecordsId,
                    out callRecordsId);

                // Check that a valid call record is given to us
                if(callRecordsId < SqlConstants.StandardPrimaryKeySeed)
                {
                    callRecordsId = 0; // Cast to uint in CallRecordsId property must not throw exception
                    success = false;
                }
            
                if(success) return IApp.VALUE_SUCCESS;
                else        return IApp.VALUE_FAILURE;
            }
        }

        #endregion
	}	// class WriteCallRecordStart
}
