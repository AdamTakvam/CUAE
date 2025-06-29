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
    /// Retrieves the user that is associated with the specified DN, provided that the given DN
    /// is his primary DN
    /// </summary>
    public class GetUserByPrimaryDN : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The primary DN for a user", true)]
        public string DirectoryNumber { set { dn = value; } }
        private string dn;

        [ResultDataField("The user id associated with specified directory number")]
        public uint UserId { get { return userId; } }
        private uint userId;

        [ResultDataField("Account code associated with user")]
        public uint AccountCode { get { return accountCode; } }
        private uint accountCode;

        [ResultDataField("The status of the user")]
        public string UserStatus { get { return userStatus; } }
        private string userStatus;

        public GetUserByPrimaryDN()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            return (dn == null) ? false : true;
        }

        public void Clear()
        {
            dn = null;
            userStatus = string.Empty;
            userId = 0;
        }

        [Action("GetUserByPrimaryDN", false, "Retrieves user based on DN", "Retrieves the user that is associated with the specified DN, provided that the given DN is his primary DN")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(DirectoryNumbers dirNumDbAccess = new DirectoryNumbers(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                using(Devices devicesDbAcces = new Devices(
                          sessionData.DbConnections[SqlConstants.DbConnectionName], 
                          log,
                          sessionData.AppName,
                          sessionData.PartitionName,
                          DbTable.DetermineAllowWrite(sessionData.CustomData)))
                {
                    using(Users usersDbAccess = new Users(
                              sessionData.DbConnections[SqlConstants.DbConnectionName],
                              log,
                              sessionData.AppName,
                              sessionData.PartitionName,
                              DbTable.DetermineAllowWrite(sessionData.CustomData)))
                    {
                        bool success;
                        uint deviceId;
                        uint status;

                        success = dirNumDbAccess.GetDeviceByPrimaryDN(dn, out deviceId);
                        if (success)
                        {
                            if (devicesDbAcces.GetUserForDevice(deviceId, out userId))
                            {
                                if (usersDbAccess.GetUserStatus(userId, out status))
                                {
                                    try
                                    {
                                        userStatus = Enum.GetName(typeof(UserStatus), status);
                                        success = usersDbAccess.GetAccountCode(userId, out accountCode);
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

                        if(success)                 return IApp.VALUE_SUCCESS;
                        else                        return IApp.VALUE_FAILURE;
                    }
                }
            }
        }
    }
}
