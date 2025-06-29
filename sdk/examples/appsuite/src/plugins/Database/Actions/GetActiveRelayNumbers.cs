using System;
using System.Data;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>
    /// Retrieves list of external numbers that are enabled for active relay functionality
    /// </summary>
    public class GetActiveRelayNumbers : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The user ID", true)]
        public  uint UserId { set { userId = value; } }
        private uint userId;

        [ActionParamField("If set to true, FindMe numbers that have been blacklisted will not be returned in this table. Default: false", false)]
        public bool ExcludeBlacklisted { set { excludeBlacklisted = value; } }
        private bool excludeBlacklisted;
        
        [ResultDataField("DataTable of external ActiveRelay numbers for a user.")]
        public DataTable NumberTable { get { return numberTable; } }
        private DataTable numberTable;

        [ResultDataField("ActiveRelay Transfer Number associated with the user")]
        public string TransferNumber { get { return transferNumber; } } 
        private string transferNumber;

        public GetActiveRelayNumbers()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            return !(userId < SqlConstants.StandardPrimaryKeySeed);
        }

        public void Clear()
        {
            numberTable = null;
            transferNumber = null;
            userId = 0;
            excludeBlacklisted = false;
        }

        [Action("GetActiveRelayNumbers", false, "Retrieves DataTable of external numbers", "Retrieves DataTable of external numbers that are enabled for use with Active Relay and are associated with the provided userId")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(Users usersDbAccess = new Users(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)
                      ))
            {
                bool success;
                success = usersDbAccess.GetActiveRelayNumbers(userId, out numberTable, excludeBlacklisted);

                // retrieve the user's AR transfer number.
                if (success && (usersDbAccess.GetTransferNumberForUser(userId, out transferNumber) == false))
                {
                    DataRow[] matchedRows = numberTable.Select(string.Format("{0} = {1}", SqlConstants.Tables.ExternalNumbers.IsCorporate, "true"));
                    if (matchedRows.Length == 1)
                    {
                        transferNumber = matchedRows[0][SqlConstants.Tables.ExternalNumbers.PhoneNumber] as string;
                        if (transferNumber == null)
                            transferNumber = string.Empty;
                    }
                }

                if(success)                 return IApp.VALUE_SUCCESS;
                else
                {
                    numberTable = new DataTable();
                    return IApp.VALUE_FAILURE;
                }
            }
        }
    }
}