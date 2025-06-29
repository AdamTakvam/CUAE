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
    /// Enables/Disables specified FindMe numbers for a user for use with active relay. 
    /// </summary>
    public class SetFindMeStatus : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The user ID", true)]
        public  uint UserId { set { userId = value; } }
        private uint userId;

        [ActionParamField("FindMe number filter, in SQL format. This field is used to match desired rows using the \"phone_number LIKE <filter>\" Use % to specify ALL.", false)]
        public string Filter { set { filter = value; } }
        private string filter;

        [ActionParamField("The new value of the ar_enabled field. Default: false", false)]
        public bool NewValue { set { newValue = value; } }
        public bool newValue;

        public SetFindMeStatus()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            return !(userId < SqlConstants.StandardPrimaryKeySeed);
        }

        public void Clear()
        {
            userId = 0;
            filter = string.Empty;
            newValue = false;
        }

        [Action("SetFindMeStatus", false, "SetFindMeStatus", "Enables/Disables specified FindMe numbers for a user for use with active relay.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(ExternalNumbers externNumsDbAccess = new ExternalNumbers(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                bool success = externNumsDbAccess.SetFindMeStatus(userId, filter, newValue);
                return success ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
            }
        }
    }
}