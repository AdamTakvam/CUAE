using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>Validates and logs in a user who authenticated via HTTP</summary>
	public class WebLogin : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("Username", true)]
        public  string Username { set { username = value; } }
        private string username;

        [ActionParamField("Password", true)]
        public  string Password { set { password = value; } }
        private string password;

        [ActionParamField("IP Address of the user being authenticated", true)]
        public  string IpAddress { set { ipAddress = value; } }
        private string ipAddress;

        [ResultDataField("User ID")]
        public  uint UserId { get { return userId; } }
        private uint userId;

        [ResultDataField("Authentication Record ID")]
        public  uint AuthenticationRecordId { get { return authRecordId; } }
        private uint authRecordId;
         
        [ResultDataField("Authentication Result")]
        public  AuthenticationResult AuthenticationResult { get { return authenticationResult; } }
        private AuthenticationResult authenticationResult;
        
        public WebLogin()
		{
            Clear();
		}

        public bool ValidateInput()
        {
            if(ipAddress == null || ipAddress.Length == 0)      { return false; }
            return true;
        }

        public void Clear()
        {
            username = null;
            password = null;
            ipAddress = null;
            userId = 0;
            authenticationResult = AuthenticationResult.failure;
        }

        [Action("WebLogin", false, "Web Login", "Validates and logs in a user who authenticated via HTTP")]
        [ReturnValue(typeof(AuthenticationResult), "'true' if true, 'false' if false")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(Users usersDbAccess = new Users(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                bool success = usersDbAccess.ValidateWebLogin(
                    username, 
                    password, 
                    ipAddress, 
                    out userId, 
                    out authenticationResult,
                    out authRecordId);

                // Unable to get a valid user ID
                if(userId < SqlConstants.StandardPrimaryKeySeed)
                {
                    userId  = 0; // Cast to uint in UserId property must not throw exception
                    success = false;
                }

                // Unable to obtain a valid authentication record ID
                if(authRecordId < SqlConstants.StandardPrimaryKeySeed)
                {
                    authRecordId    = 0; // Cast to uint in AuthRecordId property must not throw exception
                    success         = false;
                }

                if(success)     return IApp.VALUE_SUCCESS; 
                else            return authenticationResult.ToString();
            }
        }
	}
}
