using System;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>
    /// Retrieves list of external numbers for a user, possibly checking for application settings
    /// </summary>
    public class GetExternalNumbersForUser : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The user ID", true)]
        public  uint UserId { set { userId = value; } }
        private uint userId;

        [ActionParamField("hashtable of user application settings to check against. If none provided, all external numbers for a user are returned", false)]
        public Hashtable AppSettings { set { appSettings = value; } }
        private Hashtable appSettings;

        [ResultDataField("List of external numbers for a user.")]
        public StringCollection NumberList { get { return numberList; } }
        private StringCollection numberList;

        public GetExternalNumbersForUser()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            return !(userId < SqlConstants.StandardPrimaryKeySeed);
        }

        public void Clear()
        {
            appSettings = null;
            numberList = null;
            userId = 0;
        }

        [Action("GetExternalNumbersForUser", false, "Retrieves list of external numbers", "Retrieves list of external numbers for a user, possibly checking for application settings")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(Users usersDbAccess = new Users(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                bool success;
                success = usersDbAccess.GetExternalNumbersForUser(userId, out numberList, appSettings);

                if(success)                 return IApp.VALUE_SUCCESS;
                else
                {
                    numberList = new StringCollection();
                    return IApp.VALUE_FAILURE;
                }
            }
        }
    }
}