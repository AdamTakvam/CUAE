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
    /// Retrieves the phone number that the user designated as his/her corporate number.
    /// </summary>
    public class GetCorporateNumberForUser : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The user ID", true)]
        public  uint UserId { set { userId = value; } }
        private uint userId;

        [ResultDataField("Corporate number.")]
        public String Number { get { return number; } }
        private String number;

        public GetCorporateNumberForUser()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            return !(userId < SqlConstants.StandardPrimaryKeySeed);
        }

        public void Clear()
        {
            number = null;
            userId = 0;
        }

        [ReturnValue(typeof(ExternalNumbers.GetCorpNumForUserResults), "Result of the operation: Success, Failure, or NoNumberDefined")]
        [Action("GetCorporateNumberForUser", false, "GetCorporateNumberForUser", "Retrieves the phone number that the user designated as his/her corporate number.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(ExternalNumbers externNumDbAccess = new ExternalNumbers(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                return externNumDbAccess.GetCorporateNumberForUser(userId, out number).ToString();
            }
        }
    }
}