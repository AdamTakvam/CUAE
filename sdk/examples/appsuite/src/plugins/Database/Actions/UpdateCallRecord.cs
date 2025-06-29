using System;
using System.Collections;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;
using CallRecordsTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.CallRecords;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary> Updates a call detail record. </summary>
    public class UpdateCallRecord : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The ID of the call record.", true)]
        public  uint CallRecordsId { set { callRecordsId = value; } }
        private uint callRecordsId;

        [ActionParamField("Originating number.", false)]
        public  string OriginNumber { set { originNumber = value; originNumberSpecified = true; } }
        private string originNumber;
        private bool   originNumberSpecified;

        [ActionParamField("Destination number.", false)]
        public  string DestinationNumber { set { destinationNumber = value; destinationNumberSpecified = true; } }
        private string destinationNumber;
        private bool   destinationNumberSpecified;

        [ActionParamField("ID of the session containing this call.", false)]
        public  uint SessionRecordsId { set { sessionRecordsId = value; sessionRecordsIdSpecified = true; } }
        private uint sessionRecordsId;
        private bool sessionRecordsIdSpecified;

        [ActionParamField("ID of the user placing this call.", false)]
        public  uint UserId { set { userId = value; userIdSpecified = true; } }
        private uint userId;
        private bool userIdSpecified;

        [ActionParamField("ID of an authentication record for this call.", false)]
        public  uint AuthRecordsId { set { authRecordsId = value; authRecordsIdSpecified = true; } }
        private uint authRecordsId;
        private bool authRecordsIdSpecified;

        public UpdateCallRecord()
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
            callRecordsId           = 0;
            originNumber            = null;
            destinationNumber       = null;
            sessionRecordsId        = 0;
            userId                  = 0;
            authRecordsId           = 0;
            authRecordsIdSpecified = userIdSpecified = sessionRecordsIdSpecified = destinationNumberSpecified = originNumberSpecified = false;
        }

        [Action("UpdateCallRecord", false, "Write Call Record Start", "Writes a new call detail record.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(CallRecords callRecordsDbAccess = new CallRecords(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                bool success = false;
                Hashtable updateFields = new Hashtable();

                if (originNumberSpecified)
                    updateFields.Add(CallRecordsTable.OriginNumber, originNumber);
                if (destinationNumberSpecified)
                    updateFields.Add(CallRecordsTable.DestinationNumber, destinationNumber);
                if (sessionRecordsIdSpecified)
                    updateFields.Add(CallRecordsTable.SessionRecordsId, sessionRecordsId);
                if (userIdSpecified)
                    updateFields.Add(CallRecordsTable.UserId, userId);
                if (authRecordsIdSpecified)
                    updateFields.Add(CallRecordsTable.AuthRecordsId, authRecordsId);

                if (updateFields.Count == 0)
                    log.Write(TraceLevel.Info, "UpdateCallRecord action was invoked, but no field values were specified.");
                else
                    success = callRecordsDbAccess.UpdateCallRecord(callRecordsId, updateFields);

                if(success) return IApp.VALUE_SUCCESS;
                else        return IApp.VALUE_FAILURE;
            }
        }

        #endregion
    }	// class UpdateCallRecord
}
