using System;
using System.Data;
using System.Collections;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework.Collections;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>
    ///     Validates and logs in a user who authenticated via an IP phone application
    /// </summary>
    [PackageDecl("Metreos.ApplicationSuite.Actions")]
    public class PhoneLogin : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("Account Code", true)]
        public  uint AccountCode { set { accountCode = value; } }
        private uint accountCode;

        [ActionParamField("PIN", true)]
        public  uint Pin { set { pin = value; } }
        private uint pin;

        [ActionParamField("The directory number or phone number of the user being authenticated", true)]
        public  string UserPhoneNumber { set { phoneNumber = value; } }
        private string phoneNumber;

        [ResultDataField("User ID")]
        public  uint UserId { get { return userId; } }
        private uint userId;

        [ResultDataField("Authentication Record ID")]
        public  uint AuthenticationRecordId { get { return authRecordId; } }
        private uint authRecordId;
         
        [ResultDataField("True if pin change required")]
        public  bool PinChangeRequired { get { return pinChangeRequired; } }
        private bool pinChangeRequired;

        [ResultDataField("Authentication Result")]
        public  AuthenticationResult AuthenticationResult { get { return authenticationResult; } }
        private AuthenticationResult authenticationResult;

        public PhoneLogin() 
        {
            Clear();
        }

        public bool ValidateInput()
        {
            if(phoneNumber == null || phoneNumber.Length == 0)  { return false; }          
            return true;
        }

        public void Clear()
        {
            pin                 = 0;
            userId              = 0;
            accountCode         = 0;
            authRecordId        = 0;
            phoneNumber         = null;
            pinChangeRequired   = false;
            authenticationResult = AuthenticationResult.failure;
        }

        [Action("PhoneLogin", false, "Phone Login", "Validates and logs in a user who authenticated via an IP phone application")]
        [ReturnValue(typeof(AuthenticationResult), "Result of the authentication operation.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            using(Users usersDbAccess = new Users(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                bool success = usersDbAccess.ValidatePhoneLogin(
                    (int) accountCode, 
                    (int) pin, 
                    phoneNumber, 
                    out userId, 
                    out authenticationResult,
                    out authRecordId, 
                    out pinChangeRequired);

                // Unable to get a valid user ID
                if(userId < SqlConstants.StandardPrimaryKeySeed)
                {
                    userId  = 0;    // Cast to uint in UserId property must not throw exception
                    success = false;
                }

                // Unable to obtain a valid authentication record ID
                if(authRecordId < SqlConstants.StandardPrimaryKeySeed)
                {
                    authRecordId    = 0;    // Cast to uint in AuthRecordId property must not throw exception
                    success         = false;
                }

                if(success)     return AuthenticationResult.success.ToString();
                else            return authenticationResult.ToString();
            }
        }
    }
}
