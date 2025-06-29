using System;
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
    /// Retrieves the e-mail address associated with a user. 
    /// </summary>
    public class GetEmailAddress : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("UserId of user whose E-mail address we wish to retrieve", true)]
        public uint UserId { set { userId = value; } }
        private uint userId;

        [ResultDataField("The primary E-mail address for the specified user id")]
        public string EmailAddress{ get { return emailAddress; } }
        private string emailAddress;

        public GetEmailAddress()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            userId = 0;
            emailAddress = null;
        }

        [Action("GetEmailAddress", false, "Retrieves the e-mail address associated with a user.", "Retrieves the e-mail address associated with a user.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(Users usersDbAccess = new Users(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                try
                {
                    emailAddress = usersDbAccess.GetEmail(userId);
                    return (emailAddress == null) ? IApp.VALUE_FAILURE : IApp.VALUE_SUCCESS;
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, 
                        "Error encountered in the GetUserByPrimaryDN action, using user Id: {0}\n" +
                        "Error message: {1}", userId, e.Message);
                    return IApp.VALUE_FAILURE;
                }
            }
        }
    }
}
