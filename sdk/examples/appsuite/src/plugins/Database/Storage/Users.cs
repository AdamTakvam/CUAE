using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Method = Metreos.Utilities.SqlBuilder.Method;
using UserTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.Users;
using CallTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.CallRecords;
using DeviceTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.Devices;
using AuthenticationTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.AuthenticationRecords;
using SessionRecordsTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.SessionRecords;
using ActiveRelayFilterNumbersTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.ActiveRelayFilterNumbers;
using ExternNumTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.ExternalNumbers;
using Novell.Directory.Ldap;

namespace Metreos.ApplicationSuite.Storage
{
	/// <summary>
	///     Provides data access to the users table, and any information which stem from that
	/// </summary>
	public class Users : DbTable
	{
        public const string CISCO_USER_PRO_ATTR         = "ciscoatUserProfile";
        public const string CISCO_USER_APP_PRO_ATTR     = "ciscoatAppProfile";
        public const string CISCO_PIN_ATTR              = "ciscoCCNatPIN";
        public const string STANDARD_PASSWORD           = "userPassword"; // Should not be unencrypted, this may not be useful
        
        // retrieves the current system DATETIME
        public const string MYSQL_FUNCTION_NOW          = "NOW()";
        
        // MySQL SYSTEM timezone
        public const string MYSQL_TIMEZONE_SYSTEM       = "SYSTEM";

        // MySQL GMT timezone
        public const string MYSQL_TIMEZONE_GMT          = "GMT";

        // 0 is current datetime, 1 is from_tz, 2 is to_tz. Used by some of the TZ-related functions in this class.
        public const string CONVERT_TZ_FUNCTION_STRING  = "CONVERT_TZ(({0}), ({1}), ({2}))";

        // This MySQL function will return the span of time between {1} and {0}
        // example from MySQL manual:
        /*
         * mysql> SELECT TIMEDIFF('1997-12-31 23:59:59.000001',
         *      ->                 '1997-12-30 01:01:01.000002');
         *      -> '46:58:57.999999'
         */
        public const string TIMEDIFF_FUNCTION_STRING    = "TIMEDIFF(({0}), ({1}))";

 		public Users(IDbConnection connection, LogWriter log, string applicationName, string partitionName, bool allowWrite)
            : base(connection, log, applicationName, partitionName, allowWrite) { }
        
        #region ReturnValues
        public enum GetUserReturnValues
        {
            success,
            NoUser,
            failure
        }

        public enum ChangePinReturnValues
        {
            success,
            InvalidPin,
            failure
        }
        #endregion

        #region ValidatePhoneLogin

        /// <summary>
        ///     Checks for validity of AccountCode and Pin, and creates a authentication record
        /// </summary>
        /// <param name="accountCode">
        ///     User AccountCode 
        /// </param>
        /// <param name="pin"> 
        ///     User PIN 
        /// </param>
        /// <param name="directoryNumber"> 
        ///     The 'from' number dialed. Used in authentication record keeping. Use a negative number for unknown 
        /// </param>
        /// <param name="userId"> 
        ///     The User Id for this action 
        /// </param>
        /// <param name="result"> 
        ///     The status of the login attempt.  Only needed on a return of <c>false</c> 
        /// </param>
        /// <param name="authRecordId">
        /// A reference to the authentication record.
        /// -1 indicates unable to enter into authentication table
        /// </param>
        /// <returns> 
        ///     <c>true</c> if valid, <c>false</c> otherwise 
        /// </returns>
        public bool ValidatePhoneLogin(
            int accountCode, 
            int pin, 
            string originDirectoryNumber, 
            out uint userId, 
            out AuthenticationResult result, 
            out uint authRecordId,
            out bool pinChangeRequired)
        {
            uint dbPin = 0;
            authRecordId = 0;
            userId = 0;
            result = AuthenticationResult.success;
            pinChangeRequired = false;
        
            SqlBuilder builder = new SqlBuilder(Method.SELECT, UserTable.TableName);
            builder.fieldNames.Add(UserTable.Id);
            builder.fieldNames.Add(UserTable.Status);
            builder.fieldNames.Add(UserTable.PinChangeRequired);
            builder.fieldNames.Add(UserTable.Pin);
            builder.fieldNames.Add(UserTable.ExternalAuthEnabled);
            builder.fieldNames.Add(UserTable.ExternalAuthDn);
            builder.fieldNames.Add(UserTable.LdapServersId);
            builder.where[UserTable.AccountCode] = accountCode;

            AdvancedReadResultContainer readResult = ExecuteEasyQuery(builder);

            if(readResult.result == ReadResult.Success)
            {
                DataTable results = readResult.results;

                if((results != null) && (results.Rows.Count >= 0))
                {
                    // We found an account code, figure out if the pin matches.
                    DataRow resultRow;
                    bool multipleNonDeletedRows;
                    if(IsOneNonDeletedRow(results, out resultRow, out multipleNonDeletedRows))
                    {
                        userId            = Convert.ToUInt32(resultRow[SqlConstants.Tables.Users.Id]);
                        UserStatus status = (UserStatus) Convert.ToUInt32(resultRow[UserTable.Status]);
                        pinChangeRequired = Convert.ToBoolean(resultRow[UserTable.PinChangeRequired]);
                        dbPin             = Convert.ToUInt32(resultRow[UserTable.Pin]);
                        bool externalAuthEnabled = Convert.ToBoolean(resultRow[UserTable.ExternalAuthEnabled]);

                        bool validPin = dbPin == pin;
                        if(externalAuthEnabled)
                        {
                            object externalAuthDnEntry = resultRow[UserTable.ExternalAuthDn];
                            string externalAuthDn = Convert.IsDBNull(externalAuthDnEntry) ? null : externalAuthDnEntry as string;
                            uint ldapServersId = Convert.ToUInt32(resultRow[UserTable.LdapServersId]);
                            validPin = AuthenticateAccountCodeUsingLdap(ldapServersId, externalAuthDn, pin.ToString());
                        }
                    
                        // Check that status is ok
                        if(status == UserStatus.Ok && validPin)
                        {
                            result = AuthenticationResult.success;
                            SuccessfulLogin(userId);
                        }
                        else if(!validPin)
                        {
                            result = AuthenticationResult.InvalidAccountCodeOrPin;

                            FailedLogin(userId);

                            // We only check for lock when status is Ok
                            if(UserStatus.Ok == status)
                            {
                                CheckIfAccountShouldBeLocked(userId);
                            }
                        }
                        else // Non-ok user
                        {
                            switch(status)
                            {
                                case UserStatus.Locked:
                                    if(validPin)        result = CheckForLockoutDurationExpiration(userId);
                                    else                result = AuthenticationResult.NotAllowedDueLocked;

                                    if(result == AuthenticationResult.success) SuccessfulLogin(userId);
                                    break;

                                case UserStatus.Disabled:
                                    result = AuthenticationResult.NotAllowedDueToDisabled;
                                    break;

                                case UserStatus.Deleted:
                                    result = AuthenticationResult.NotAllowedDueToDeleted;
                                    break;
                            }

                            // We increment # failed logins, but do not
                            // check to update status to locked
                            FailedLogin(userId);
                        }
                    }
                    else if(multipleNonDeletedRows)
                    {
                        log.Write(TraceLevel.Error, 
                            "Duplicate user encountered with AccountCode '{0}'", 
                            accountCode);
                        result = AuthenticationResult.failure;
                    }
                    else
                    {
                        // All existing users matching this username must be deleted at this point.
                        result = AuthenticationResult.NotAllowedDueToDeleted;
                    }
                }
                else
                {
                    // No account code found.
                    result = AuthenticationResult.InvalidAccountCodeOrPin;
                }
            }
            else
            {
                result = AuthenticationResult.failure;
                log.Write(TraceLevel.Error, 
                    "Error encountered in the ValidatePhoneLogin method, using AccountCode '{0}' and PIN '{1}'.\n" +
                    "Error message: {2}", accountCode, pin, readResult.e.Message);
            }

            // Log this authentication record
            SqlBuilder authBuilder = new SqlBuilder(Method.INSERT, AuthenticationTable.TableName);
            authBuilder.fieldNames.Add(AuthenticationTable.UserId);
            authBuilder.fieldNames.Add(AuthenticationTable.Status);
            authBuilder.fieldNames.Add(AuthenticationTable.DirectoryNumber);
            authBuilder.fieldNames.Add(AuthenticationTable.Username);
            authBuilder.fieldNames.Add(AuthenticationTable.Pin);
            authBuilder.fieldNames.Add(AuthenticationTable.ApplicationName);
            authBuilder.fieldNames.Add(AuthenticationTable.PartitionName);

            // Format Inputs
            object userIdInput = userId;
            if(userId == 0) 
            {
                userIdInput = null;
            }

            object directoryNumberInput = originDirectoryNumber == null || originDirectoryNumber.Length == 0 
                ? null 
                : originDirectoryNumber;
            
            authBuilder.fieldValues.Add(userIdInput);
            authBuilder.fieldValues.Add(result);
            authBuilder.fieldValues.Add(directoryNumberInput);
            authBuilder.fieldValues.Add(accountCode);
            authBuilder.fieldValues.Add(pin);
            authBuilder.fieldValues.Add(applicationName);
            authBuilder.fieldValues.Add(partitionName);

            WriteResultContainer writeResult = ExecuteCommand(authBuilder);

            if(writeResult.result == WriteResult.Success)
            {
                authRecordId = writeResult.lastInsertId;
            }
            else if(writeResult.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the ValidatePhoneLogin method in attempting to record authentication.\n" +
                    "Error message: {0}", writeResult.e.Message);

            }
            else if(writeResult.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("ValidatePhoneLogin"));
            }

            return userId > 0 && result == AuthenticationResult.success;
        }

        /// <summary>
        ///     Assumes that the UserStatus is currently Ok. 
        /// </summary>
        protected void CheckIfAccountShouldBeLocked(uint userId)
        {
            // Check that failed logins threshold has been exceeded, exempting a lockout threshold of 0
            string statusCheck = String.Format("IF({0} >= {1} && {1} <> 0, {2}, {3} )", 
                UserTable.NumFailedLogins, 
                UserTable.LockoutThreshold, 
                (int)UserStatus.Locked,
                UserTable.Status);

            SqlBuilder checkFailedLoginThreshold = new SqlBuilder(Method.UPDATE, UserTable.TableName);
            checkFailedLoginThreshold.AddFieldValue(UserTable.Status, new SqlBuilder.PreformattedValue(statusCheck));
            checkFailedLoginThreshold.where[UserTable.Id] = userId;

            WriteResultContainer result = ExecuteCommand(checkFailedLoginThreshold);
 
            if(result.result == WriteResult.Success)
            {

            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the ValidatePhoneLogin method in attempting to check/update user status based on failed login attempts.\n" +
                    "Error message: {0}", result.e.Message);
            }
            else if(result.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("CheckIfAccountShouldBeLocked"));
            }
        }

        /// <summary>
        ///     Reset failed logins to 0.
        ///     Mark last used
        ///     Set account status to Ok
        /// </summary>
        /// <param name="userId"></param>
        protected void SuccessfulLogin(uint userId)
        {
            SqlBuilder successfulLogin = new SqlBuilder(Method.UPDATE, UserTable.TableName);
            successfulLogin.AddFieldValue(UserTable.NumFailedLogins, 0);
            successfulLogin.AddFieldValue(UserTable.LastUsed, DateTime.Now);
            successfulLogin.AddFieldValue(UserTable.Status, (int)UserStatus.Ok);
            successfulLogin.where[UserTable.Id] = userId;

            WriteResultContainer result = ExecuteCommand(successfulLogin);
 
            if(result.result == WriteResult.Success)
            {

            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the SuccessfulLogin method in attempting to handle a successful login.\n" +
                    "Error message: {0}", result.e.Message);
            }
            else if(result.result == WriteResult.PublisherDown)
            {
				log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("SuccessfulLogin"));
            }
        }

        /// <summary>
        ///     Increment failed logins.
        ///     Mark last lockout
        /// </summary>
        /// <param name="userId"></param>
        protected void FailedLogin(uint userId)
        {
            // Increment number of failed login attempts
            string incrementCommand = String.Format("{0} + 1", UserTable.NumFailedLogins);

            SqlBuilder incrementFailedLoginAttempts = new SqlBuilder(Method.UPDATE, UserTable.TableName);
            incrementFailedLoginAttempts.AddFieldValue(
                UserTable.NumFailedLogins, 
                new SqlBuilder.PreformattedValue(incrementCommand));
            incrementFailedLoginAttempts.AddFieldValue(UserTable.LastLockout, DateTime.Now);
            incrementFailedLoginAttempts.where[UserTable.Id] = userId;

            WriteResultContainer result = ExecuteCommand(incrementFailedLoginAttempts);
 
            if(result.result == WriteResult.Success)
            {

            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the ValidatePhoneLogin method in attempting to increment failed logins.\n" +
                    "Error message: {0}", result.e.Message);
            }
            else if(result.result == WriteResult.PublisherDown)
            {
				log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("FailedLogin"));
            }
        }

        /// <summary>
        ///     Method assumes accountCode/pin is valid, checks for lockout duration expiration
        /// </summary>
        /// <param name="userId"> The ID of the user </param>
        /// <returns> An Authentication Result status </returns>
        protected AuthenticationResult CheckForLockoutDurationExpiration(uint userId)
        {
            AuthenticationResult result = AuthenticationResult.NotAllowedDueLocked;

            SqlBuilder builder = new SqlBuilder(Method.SELECT, UserTable.TableName);
            builder.fieldNames.Add(UserTable.LastLockout);
            builder.fieldNames.Add(UserTable.LockoutDuration);
            builder.where[UserTable.Id] = userId;

            AdvancedReadResultContainer readResults = ExecuteEasyQuery(builder);

            if(readResults.result == ReadResult.Success)
            {
                DataTable results = readResults.results;

                if(results != null && results.Rows.Count == 1)
                {
                    DateTime lastLockout = Convert.ToDateTime(results.Rows[0][UserTable.LastLockout]);
                    TimeSpan lockoutDuration = (TimeSpan) results.Rows[0][UserTable.LockoutDuration];
                    
                    // lockout duration of zero indicates that the user can only be unlocked by admin
                    if(lockoutDuration != TimeSpan.Zero && DateTime.Now.Subtract(lockoutDuration) > lastLockout)
                    {
                        result = AuthenticationResult.success;
                    }
                }
                else if(results != null && results.Rows.Count > 1)
                {
                    log.Write(TraceLevel.Error, 
                        "Duplicate user encountered in check for lockout duration.");
                }
                else
                {
                    log.Write(TraceLevel.Error, 
                        "No user encountered in check for lockout duration.");                     
                }
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the ValidatePhoneLogin method in attempting to check for lockout duration expiration.\n" +
                    "Error message: {0}",  readResults.e.Message);
                result = AuthenticationResult.NotAllowedDueLocked;
            }

            return result;
        }

        #endregion

        #region ValidateWebLogin

        /// <summary>
        ///     Checks for validity of Username / Password, and creates an authentication record
        /// </summary>
        /// <param name="username"> 
        ///     Username 
        /// </param>
        /// <param name="password">
        ///     Password 
        /// </param>
        /// <param name="originIpAddress">
        ///     IP Address of the device logging in.  Use <c>null</c> to indicate unknown
        /// </param>
        /// <param name="authRecordId">
        /// A reference to the authentication record.
        /// -1 indicates unable to enter into authentication table
        /// </param>
        /// <param name="userId"> The UserId for the given parameters.  -1 indicates invalid user </param>
        /// <param name="result"> The status of the login attempt.  Only needed on a return of <c>false</c> </param>
        /// <returns> <c>true</c> if valid, <c>false</c> otherwise </returns>
        public bool ValidateWebLogin(
            string username,
            string password, 
            string originIpAddress, 
            out uint userId, 
            out AuthenticationResult result,
            out uint authRecordId)
        {
            string dbPassword;
            authRecordId = 0;
            userId = 0;
            result = AuthenticationResult.success;
        
            SqlBuilder builder = new SqlBuilder(Method.SELECT, UserTable.TableName);
            builder.fieldNames.Add(UserTable.Id);
            builder.fieldNames.Add(UserTable.Status);
            builder.fieldNames.Add(UserTable.ExternalAuthEnabled);
            builder.fieldNames.Add(UserTable.ExternalAuthDn);
            builder.fieldNames.Add(UserTable.LdapServersId);
            builder.fieldNames.Add(UserTable.Password);
            builder.where[UserTable.Username] = username;
            
            AdvancedReadResultContainer readResult = ExecuteEasyQuery(builder);

            if(readResult.result == ReadResult.Success)
            {
                DataTable resultTable = readResult.results;

                if((resultTable != null) && (resultTable.Rows.Count >= 0))
                {
                    DataRow resultRow;
                    bool multipleNonDeletedRows;
                    if(IsOneNonDeletedRow(resultTable, out resultRow, out multipleNonDeletedRows))
                    {
                        userId = Convert.ToUInt32(resultRow[SqlConstants.Tables.Users.Id]);
                        UserStatus status = (UserStatus) Convert.ToUInt32(resultRow[UserTable.Status]);
                        dbPassword = Convert.ToString(resultRow[UserTable.Password]);
                        bool externalAuthEnabled = Convert.ToBoolean(resultRow[UserTable.ExternalAuthEnabled]);

                        bool validPass = password == dbPassword;
                        if(externalAuthEnabled)
                        {
                            object externalAuthDnEntry = resultRow[UserTable.ExternalAuthDn];
                            string externalAuthDn = Convert.IsDBNull(externalAuthDnEntry) ? null : externalAuthDnEntry as string;
                            uint ldapServersId = Convert.ToUInt32(resultRow[UserTable.LdapServersId]);
                            validPass = AuthenticateUsernameUsingLdap(ldapServersId, externalAuthDn, password);
                        }

                        // Check that status is ok
                        if(status == UserStatus.Ok && validPass)
                        {
                            result = AuthenticationResult.success;
                            SuccessfulLogin(userId);
                        }
                        else if(!validPass)
                        {
                            result = AuthenticationResult.InvalidAccountCodeOrPin;

                            FailedLogin(userId);

                            // We only check for lock when status is Ok
                            if(UserStatus.Ok == status)
                            {
                                CheckIfAccountShouldBeLocked(userId);
                            }
                        }
                        else // Non-ok user
                        {
                            switch(status)
                            {
                                case UserStatus.Locked:
                                    if(validPass)               result = CheckForLockoutDurationExpiration(userId);
                                    else                        result = AuthenticationResult.NotAllowedDueLocked;

                                    if(result == AuthenticationResult.success) SuccessfulLogin(userId);
                                    break;

                                case UserStatus.Disabled:
                                    result = AuthenticationResult.NotAllowedDueToDisabled;
                                    break;

                                case UserStatus.Deleted:
                                    result = AuthenticationResult.NotAllowedDueToDeleted;
                                    break;
                            }

                        
                            // We increment # failed logins, but do not
                            // check to update status to locked
                            FailedLogin(userId);
                        }
                    }
                    else if(multipleNonDeletedRows)
                    {
                        log.Write(TraceLevel.Error, 
                            "Duplicate user encountered with Username '{0}' and Password '{1}'.", 
                            username, password);
                        result = AuthenticationResult.failure;
                    }
                    else
                    {
                        // All existing users matching this username must be deleted at this point.
                        result = AuthenticationResult.NotAllowedDueToDeleted;
                    }
                }
                else
                {
                    result = AuthenticationResult.InvalidAccountCodeOrPin;
                }

            }
            else
            {
                result = AuthenticationResult.failure;
                log.Write(TraceLevel.Error, 
                    "Error encountered in the ValidateWebLogin method, using Username '{0}'.\n" +
                    "Error message: {2}", username, readResult.e.Message);
            }

            // Log this authentication record
            // Format Inputs
            object userIdInput = userId;
            if(userId == 0)
            {
                userIdInput = null;
            }                 

            SqlBuilder authBuilder = new SqlBuilder(Method.INSERT, AuthenticationTable.TableName);
            authBuilder.AddFieldValue(AuthenticationTable.UserId, userIdInput);
            authBuilder.AddFieldValue(AuthenticationTable.Status, result);
            authBuilder.AddFieldValue(AuthenticationTable.IpAddress, originIpAddress);
            authBuilder.AddFieldValue(AuthenticationTable.Username, username);
            authBuilder.AddFieldValue(AuthenticationTable.Pin, null);
            authBuilder.AddFieldValue(AuthenticationTable.ApplicationName, applicationName);
            authBuilder.AddFieldValue(AuthenticationTable.PartitionName, partitionName);

            WriteResultContainer writeResult = ExecuteCommand(authBuilder);

            if(writeResult.result == WriteResult.Success)
            {
                authRecordId = writeResult.lastInsertId;
            }
            else if(writeResult.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the ValidateWebLogin method in attempting to record authentication.\n" +
                    "Error message: {0}", writeResult.e.Message);
            }
            else if(writeResult.result == WriteResult.PublisherDown)
            {
			    log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("ValidateWebLogin"));
            }

            return (userId > 0) && (result == AuthenticationResult.success);
        }

        protected bool AuthenticateUsernameUsingLdap(uint ldapServersId, string externalAuthDn, string userPass)
        {
            bool success = false;
            string ldapServerName;
            ushort port;
            bool secureConnect;
            string baseDn;
            string userDn;
            string ldapPassword;

            LdapServers ldapServersTable = new LdapServers(this);
            if(ldapServersTable.GetLdapServer(ldapServersId, out ldapServerName, out port, out secureConnect, out baseDn, out userDn, out ldapPassword))
            {
                LdapConnection con = null;

                try
                {
                    con = new LdapConnection();
                    con.Connect(ldapServerName, (int)port);
                    con.Bind(externalAuthDn, userPass);
                    success = true;
                }
                catch(LdapException e) 
                {  
                    log.Write(TraceLevel.Error, "LDAP QueryUsers error message: " + e.LdapErrorMessage);
                }
                catch(Exception e) 
                { 
                    log.Write(TraceLevel.Error, "LDAP QueryUsers failed: " + e.Message);
                }
                finally
                {
                    if( (con != null) &&
                        (con.Connected == true))
                    {
                        con.Disconnect();
                    }
                }

                try
                {
                    con = new LdapConnection();
                    con.Connect(ldapServerName, (int)port);
                    con.Bind(userDn, ldapPassword);
                    
                    if(!success) // Try compare pass to userPass attr
                    {
                        success = ValideAgainstLdapPassword(con, ldapServerName, port, externalAuthDn, userPass);
                    }
                }
                catch(LdapException e) 
                {  
                    log.Write(TraceLevel.Error, "LDAP Password comparison error message: " + e.LdapErrorMessage);
                }
                catch(Exception e) 
                { 
                    log.Write(TraceLevel.Error, "LDAP Password comparison failed: " + e.Message);
                }
                finally
                {
                    if( (con != null) &&
                        (con.Connected == true))
                    {
                        con.Disconnect();
                    }
                }   
            }

            return success;
        }

        protected bool AuthenticateAccountCodeUsingLdap(uint ldapServersId, string externalAuthDn, string userPin)
        {
            bool success = false;
            string ldapServerName;
            ushort port;
            bool secureConnect;
            string baseDn;
            string userDn;
            string ldapPassword;

            LdapServers ldapServersTable = new LdapServers(this);
            if(ldapServersTable.GetLdapServer(ldapServersId, out ldapServerName, out port, out secureConnect, out baseDn, out userDn, out ldapPassword))
            {
                string cisocUserProfileLink = null;
                string ciscoAppProfileLink = null;
                LdapConnection con = null;
                try
                {
                    con = new LdapConnection();
                    con.Connect(ldapServerName, (int)port);
                    con.Bind(userDn, ldapPassword);

                    LdapSearchResults results = con.Search(externalAuthDn,
                        LdapConnection.SCOPE_SUB,
                        "(objectClass=*)",
                        new string[] { CISCO_USER_PRO_ATTR },
                        false);

                    try
                    {
                        // There should be one user with this dn
                        if(results.hasMore())
                        {
                            LdapEntry entry = results.next(); 
                            LdapAttribute ciscoUserAttr = entry.getAttribute(CISCO_USER_PRO_ATTR);
        
                            if(ciscoUserAttr != null)
                            {
                                cisocUserProfileLink = ciscoUserAttr.StringValue;
                            }
                        }
                        else
                        {
                            log.Write(TraceLevel.Error, "No results when searching for " + CISCO_USER_PRO_ATTR + 
                                ".  Assuming LDAP data integrity issue for user " + userDn + ".");
                        }
                    }
                    catch(Exception e)
                    {
                        log.Write(TraceLevel.Error,
                            "Exception occurred in parsing results of search for " + CISCO_USER_PRO_ATTR +
                            ".  Assuming LDAP data integrity issue for user " + userDn + ".");
                        throw e;
                    }

                    if(cisocUserProfileLink != null)
                    {
                        results = con.Search(cisocUserProfileLink,
                            LdapConnection.SCOPE_SUB,
                            "(objectClass=*)",
                            new string[] { CISCO_USER_APP_PRO_ATTR },
                            false);

                        try
                        {
                            // There should be one entry with this dn
                            if(results.hasMore())
                            {
                                LdapEntry entry = results.next(); 
                                LdapAttribute ciscoAppAttr = entry.getAttribute(CISCO_USER_APP_PRO_ATTR);
    
                                if(ciscoAppAttr != null)
                                {
                                    ciscoAppProfileLink = ciscoAppAttr.StringValue;
                                }
                            }
                            else
                            {
                                log.Write(TraceLevel.Error, "No results when searching for " + CISCO_USER_APP_PRO_ATTR + 
                                    ".  Assuming LDAP data integrity issue for user " + userDn + ".");
                            }
                        }
                        catch(Exception e)
                        {
                            log.Write(TraceLevel.Error,
                                "Exception occurred in parsing results of search for " + CISCO_USER_APP_PRO_ATTR +
                                ".  Assuming LDAP data integrity issue for user " + userDn + ".");
                            throw e;
                        }
                    }

                    if(ciscoAppProfileLink != null)
                    {
                        if(BindUser(ldapServerName, port, ciscoAppProfileLink, userPin) ||
                            ValideAgainstLdapPin(con, ldapServerName, port, ciscoAppProfileLink, userPin))
                        {
                            success = true;
                        }
                    }
                }
                catch(LdapException e) 
                {  
                    log.Write(TraceLevel.Error, "LDAP QueryUsers error message: " + e.LdapErrorMessage);
                }
                catch(Exception e) 
                { 
                    log.Write(TraceLevel.Error, "LDAP QueryUsers failed: " + e.Message);
                }
                finally
                {
                    if( (con != null) &&
                        (con.Connected == true))
                    {
                        con.Disconnect();
                    }
                }
            }
            
            return success;
        }

       
        /// <summary>
        /// Authenticates a user against an LDAP directory by performing
        /// an LDAP bind operation against the user's DN.
        /// </summary>
        /// <param name="ldapServer">The LDAP server IP address.</param>
        /// <param name="ldapPortStr">The LDAP server port number.</param>
        /// <param name="authDn">The DN to perform the bind operation 
        /// against.</param>
        /// <param name="password">The password to use in the bind
        /// operation.</param>
        /// <returns>True if authentication was successful, 
        /// false otherwise.</returns>
        private bool BindUser(string ldapServer, int port, string ciscoAppProfileDn, string password)
        {
            LdapConnection con = null;
            bool success = false;
            try
            {
                con = new LdapConnection();
                con.Connect(ldapServer, (int)port);
                con.Bind(ciscoAppProfileDn, password);             
                success = true;
            }
            catch(LdapException e) 
            {   
                log.Write(TraceLevel.Error, "LDAP Authentication error message: " + e.LdapErrorMessage);
                log.Write(TraceLevel.Error, "LDAP Authentication error code: " + e.ResultCode);
            }
            catch(Exception e) 
            { 
                log.Write(TraceLevel.Error, "LDAP Authentication failed: " + e.Message);
            }
            finally
            {
                if( (con != null) &&
                    (con.Connected == true))
                {
                    con.Disconnect();
                }
            }

            return success;
        }

        private bool ValideAgainstLdapPin(LdapConnection con, string ldapServer, int port, string ciscoAppProfileDn, string password)
        {                   
            bool valid = false;
            LdapSearchResults results = con.Search(
                ciscoAppProfileDn,
                LdapConnection.SCOPE_SUB,  
                "(objectClass=*)",          // Don't filter out anything.
                new string[] { CISCO_PIN_ATTR },
                false);

            LdapEntry entry = null;
 
            if(results.hasMore())
            {
                entry = results.next();
                LdapAttribute ldapPin = entry.getAttribute(CISCO_PIN_ATTR);

                if(ldapPin != null)
                {
                    valid = ldapPin.StringValue == password;
                }
            }

            return valid;
        }


        private bool ValideAgainstLdapPassword(LdapConnection con, string ldapServer, int port, string dn, string password)
        {                   
            bool valid = false;
            LdapSearchResults results = con.Search(
                dn,
                LdapConnection.SCOPE_SUB,  
                "(objectClass=*)",          // Don't filter out anything.
                new string[] { STANDARD_PASSWORD },
                false);

            LdapEntry entry = null;
 
            if(results.hasMore())
            {
                entry = results.next();
                LdapAttribute ldapPassword = entry.getAttribute(STANDARD_PASSWORD);

                if(ldapPassword != null)
                {
                    valid = ldapPassword.StringValue == password;
                }
            }

            return valid;
        }

        #endregion

        #region GetAccountStatus
        /// <summary>
        ///     If successful, will return the status of the user.  
        ///     Everytime we retrieve userstatus from the db, we need to check to see 
        ///     if the user's lockout threshold has been elapsed, and if so, 
        ///     unlock the user and reset the account
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="status">The status found for the user</param>
        /// <returns><c>true</c> if the user status could be retrieved, <c>false</c> if not</returns>
        public bool GetUserStatus(uint userId, out uint status)
        {
            status = 0;
            SqlBuilder builder = new SqlBuilder(Method.SELECT, UserTable.TableName);
            builder.fieldNames.Add(UserTable.Status);
            builder.where[UserTable.Id] = userId;

            AdvancedReadResultContainer result = ExecuteEasyQuery(builder);

            if(result.result == ReadResult.Success)
            {
                DataTable table = result.results;

                if (table.Rows.Count < 1)
                    return false;

                DataRow row = table.Rows[0];
                status = Convert.ToUInt32(row[UserTable.Status]);
                
                // check for lockout expiration
                // You might ask, why treat this user as if they successfully logged in, yet 
                // you were only checking their status?  When an application needs a status,
                // its most probably working in an 'assumed-logged in' mode, such as Active Relay
                // when it attempts to forward the call.  This assumption could bite us eventually,
                // and require some rework for more complex authentication rules, but for now its fine
                if(status == (uint)UserStatus.Locked)
                {
                    AuthenticationResult lockoutCheck = CheckForLockoutDurationExpiration(userId);

                    // If the lockout has become unlocked, reset the user
                    if(lockoutCheck == AuthenticationResult.success)
                    {
                        SuccessfulLogin(userId);
                        status = (uint)UserStatus.Ok;
                    }
                }
                return (status == 0) ? false : true;
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetAccountStatus method with user Id: {0}\n" +
                    "Error message: {1}", userId, result.e.Message);
                return false;
            }
        }
        #endregion

        #region GetAllDevices

        /// <summary>
        ///     Retreives all devices for a user
        /// </summary>
        /// <param name="userId"> The id of the user </param>
        /// <returns> Returns an array of MacAddress, with the first element as the primary device.
        /// If no devices found or error, returns <c>null</c></returns>
        public string[] GetAllDevices(uint userId)
        {
            if(userId == 0) return null;

            ArrayList deviceList = new ArrayList();

            SqlBuilder builder = new SqlBuilder(Method.SELECT, DeviceTable.TableName);
            builder.fieldNames.Add(DeviceTable.MacAddress);
            builder.fieldNames.Add(DeviceTable.IsPrimaryDevice);
            builder.where[DeviceTable.UserId] = userId;

            AdvancedReadResultContainer result = ExecuteEasyQuery(builder);

            if(result.result == ReadResult.Success)
            {
                DataTable results = result.results;

                if(results == null) return null; 

                foreach(DataRow row in results.Rows)
                {
                    string macAddress   = Convert.ToString(row[DeviceTable.MacAddress]);
                    bool isPrimary      = Convert.ToBoolean(row[DeviceTable.IsPrimaryDevice]);
                    
                    if(isPrimary) deviceList.Insert(0, macAddress);
                    else          deviceList.Add(macAddress);
                }
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetAllDevices method, using userId '{0}'\n" +
                    "Error message: {1}", userId, result.e.Message);
            }

            if(deviceList.Count > 0)    return (string[]) deviceList.ToArray(typeof(string));
            else                        return null;
        }

        #endregion

        #region GetPrimaryDevice

        /// <summary>
        ///     Retreives the primary device for a user, <c>null</c> if one could not be found
        /// </summary>
        /// <param name="userId"> The id of the user </param>
        /// <returns> Returns the primary device MacAddress </returns>
        public string GetPrimaryDevice(int userId)
        {
            SqlBuilder builder = new SqlBuilder(Method.SELECT, DeviceTable.TableName);
            builder.fieldNames.Add(DeviceTable.MacAddress);
            builder.where[DeviceTable.UserId] = userId;
            builder.where[DeviceTable.IsPrimaryDevice] = true;

            string primaryMacAddress = null;

            ReadResultContainer result = ExecuteScalar(builder);

            if(result.result == ReadResult.Success)
            {
                primaryMacAddress = result.scalar as string;
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetPrimaryDevice method, using userId '{0}'\n" +
                    "Error message: {2}", userId, result.e.Message);
            }

            return primaryMacAddress;
        }

        #endregion

        #region GetPrimaryDeviceIdByUser
        /// <summary>
        /// This method returns the device
        /// </summary>
        public bool GetPrimaryDeviceIdByUser(uint userId, out uint deviceId)
        {
            deviceId = 0;

            SqlBuilder builder = new SqlBuilder(Method.SELECT, DeviceTable.TableName);
            builder.fieldNames.Add(DeviceTable.Id);
            builder.where[DeviceTable.UserId] = userId;
            builder.where[DeviceTable.IsPrimaryDevice] = true;

            AdvancedReadResultContainer result = ExecuteEasyQuery(builder);

            if(result.result == ReadResult.Success)
            {
                DataTable table = result.results;

                if ( (table == null) || (table.Rows.Count == 0))
                    return false;

                if (table.Rows.Count > 1)
                {
                    object[] msgArray = new object[3] { "Found multiple primary devices for user Id: '", 
                                                          userId, "' !" } ;
                    log.Write(TraceLevel.Error, "{0}{1}{2}", msgArray);
                    return false;
                } 

                DataRow row = table.Rows[0];

                if ( Convert.IsDBNull(row[DeviceTable.Id]) )
                    return false;

                deviceId = Convert.ToUInt32(row[DeviceTable.Id]);
                return ( deviceId < SqlConstants.StandardPrimaryKeySeed ) ? false : true;
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetPrimaryDeviceIdByUser method, using user Id: '{0}'\n" +
                    "Error message: {1}", userId, result.e.Message);
                return false;
            }
                
        }
        #endregion

        #region GetExternalNumbersForUser
        /// <summary>
        /// This method is used to return the list of external numbers associated with the specified
        /// user id.
        /// </summary>
        /// <param name="applications">Hashtable specifying which applications we are interested in</param>
        /// <returns></returns>
        public bool GetExternalNumbersForUser(uint userId, out StringCollection numbers, Hashtable applications)
        {
            // ExternalNumbers has fields such as 'ar_enabled'. Those fields specify whether a given number
            // is to be used by a specific application. For example, if 'ar_enabled' is set to 'true' for 
            // a specific entry in the ExternalNumbers table, then that number may be used by the 
            // ActiveRelay application. The table needs to be in the following format:
            // The key should be a string equivalent to the name of the field we wish to specify, such as
            // 'ar_enabled', and the value should be a boolean.
            // Thus, if we pass in a hashtable containing 'ar_enabled' as the key, and 'true' for the value
            // associated with that key, this function will only return numbers out of the ExternalNumbers
            // table that are associated with the given userId and are enabled to work with ActiveRelay.
            // If 'applications' is null, no testing will be done, and an overloaded version of
            // GetExternalNumbersForUser that does not take the 'applications' parameter is defined below.
            
            numbers = null;
            if (userId < SqlConstants.StandardPrimaryKeySeed)
                return false;

            SqlBuilder builder = new SqlBuilder(Method.SELECT, ExternNumTable.TableName);
            builder.fieldNames.Add(ExternNumTable.PhoneNumber);
            builder.where[ExternNumTable.UserId] = userId;
            
            if ( (applications != null) && (applications.Count > 0) )
            {
                try
                {
                    bool fieldValue;
                    foreach (string fieldName in applications.Keys)
                    {
                        fieldValue = Convert.ToBoolean(applications[fieldName]);
                        builder.where[fieldName] = fieldValue;
                    }
                }
                catch (Exception e)
                {
                    log.Write(TraceLevel.Warning, 
                        "Error encountered in the GetExternalNumbersForUser method while processing application " +
                        "permissions, using user Id: '{0}'\n" +
                        "Error message: {1}", userId, e.Message);
                    return false;
                }
            }

            AdvancedReadResultContainer result = ExecuteEasyQuery(builder);

            if(result.result == ReadResult.Success)
            {
                DataTable table = result.results;

                if ( (table == null) || (table.Rows.Count == 0) )
                    return false;

                numbers = ExternalNumbers.FormatExternalNumbers(table);
                return (numbers.Count == 0) ? false : true;
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetExternalNumbersForUser method, using user Id: '{0}'\n" +
                    "Error message: {1}", userId, result.e.Message );
                return false;
            }
        }

        /// <summary>
        /// This method is used to return the list of external numbers associated with the specified
        /// user id, without checking application permissions.
        /// </summary>
        public bool GetExternalNumbersForUser(uint userId, out StringCollection numbers)
        {
            return GetExternalNumbersForUser(userId, out numbers, null);
        }
        #endregion

        #region ChangePin

        /// <summary>
        ///     Updates a pin for a given user
        /// </summary>
        /// <param name="userId">
        ///     The ID of the user 
        /// </param>
        /// <param name="pin"> 
        ///     The new pin
        /// </param>
        /// <returns>
        ///     success, failure for database error, InvalidPin for a pin that is
        ///     not a unsigned integer
        /// </returns>
        public ChangePinReturnValues ChangePin(uint userId, string pin)

        {
            if(userId == 0)
            {
                return ChangePinReturnValues.failure;
            }

            if(!ValidatePin(pin))
            {
                return ChangePinReturnValues.InvalidPin;
            }

            SqlBuilder builder = new SqlBuilder(Method.UPDATE, UserTable.TableName);
            builder.AddFieldValue(UserTable.Pin, pin);
            builder.AddFieldValue(UserTable.PinChangeRequired, 0);
            builder.where[UserTable.Id] = userId;

            WriteResultContainer result = ExecuteCommand(builder);
 
            if(result.result == WriteResult.Success)
            {
                int numAffected = result.rowsAffected;

                // In these two error conditions, do we return failure or InvalidPin?
                // After all, the database didn't fail, we really just want to alert
                // the administrator that the database is getting out of whack;
                // not give the App Developer grief by handling every minute condition
                if(numAffected > 1)
                {
                    log.Write(TraceLevel.Error, "Duplicate user found when attempting to change a pin for userId " +
                        "{0}", userId);
                }
                else if(numAffected == 0)
                {
                    log.Write(TraceLevel.Error, "No user found when attempting to change a pin for userId " +
                        "{0}", userId);
                }
            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the ChangePin method, using userId '{0}'\n" +
                    "Error message: {1}", userId, result.e.Message);
                return ChangePinReturnValues.failure;
            }
            else if(result.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("ChangePin"));
                return ChangePinReturnValues.failure;
            }

            return ChangePinReturnValues.success;
        }

        public bool ValidatePin(string pinAsString)
        {
            bool success = true;

            if(pinAsString == null || pinAsString.Length == 0)  success &= false;

            uint pin;
            try
            {
                pin = uint.Parse(pinAsString);
                success &= true;
            }
            catch { success &= false; }

            return success;
        }

        #endregion

        #region GetActiveRelayNumbers
		
		public bool GetActiveRelayNumbers(uint userId, out DataTable table, bool excludeBlacklist)
		{
			return GetActiveRelayNumbers(userId, out table, excludeBlacklist, false);
		}

        /// <summary>
        /// This method is used to return the DataTable of AR-enabled external numbers associated with the specified
        /// user id.
        /// </summary>
        /// <param name="applications">DataTable specifying which applications we are interested in</param>
        /// <returns></returns>
        public bool GetActiveRelayNumbers(uint userId, out DataTable table, bool excludeBlacklist, bool includeDisabled)
        {
            table = null;
            if (userId < SqlConstants.StandardPrimaryKeySeed)
                return false;

            SqlBuilder builder = new SqlBuilder(Method.SELECT, ExternNumTable.TableName);
            builder.where[ExternNumTable.UserId] = userId;
			if (includeDisabled == false) 
			{
				builder.where[ExternNumTable.ArEnabled] = true;
			}	
            
            if (excludeBlacklist)
                builder.where[ExternNumTable.IsBlacklisted] = false;
                        
            AdvancedReadResultContainer result = ExecuteEasyQuery(builder);

            if(result.result == ReadResult.Success)
            {
                table = result.results;
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetActiveRelayNumbers method, using user Id: '{0}'\n" +
                    "Error message: {1}", userId, result.e.Message);
            }

            if ( (table == null) || (table.Rows.Count == 0) )
                return false;

            return true;
        }
        #endregion

		#region GetActiveRelayFiltersForUser

		public bool GetActiveRelayFiltersForUser(
			uint userId, 
			out DataTable filters,
            bool includeAdmin)
		{
			bool success = false;
			filters = null;

			SqlBuilder builder = new SqlBuilder(Method.SELECT, ActiveRelayFilterNumbersTable.TableName);
            builder.appendSemicolon = false;
			builder.fieldNames.Add(ActiveRelayFilterNumbersTable.Number);
			builder.fieldNames.Add(ActiveRelayFilterNumbersTable.Type);
            
            string selectString = builder.ToString();
            string userWhereSubclause = string.Format("{0}={1}", ActiveRelayFilterNumbersTable.UserId, userId);
            string adminWhereSubclause = ActiveRelayFilterNumbersTable.UserId + "  IS NULL";

            if (includeAdmin)
                selectString = string.Format("{0} WHERE (({1}) OR ({2}));", selectString, userWhereSubclause, adminWhereSubclause);
            else
                selectString = string.Format("{0} WHERE ({1});", selectString, userWhereSubclause);

			AdvancedReadResultContainer result = ExecuteEasyQuery(selectString);
			if(result.result == ReadResult.Success)
			{
				filters = result.results;
                    
				if (filters == null )
					success = false;
				else
					success = true;
			}
			else
			{
				log.Write(TraceLevel.Error, 
					"Error encountered in the GetActiveRelayFiltersForUser method, using userId: " + userId);
				success = false;
			}
               
			return success;
		}

		#endregion

        #region GetTransferNumberForUser
        /// <summary>
        /// This method is used to return the static transfer number associated with the provided user account
        /// </summary>
        public bool GetTransferNumberForUser(uint userId, out string transferNumber)
        {
            bool success = false;
            transferNumber = string.Empty;
            if (userId < SqlConstants.StandardPrimaryKeySeed)
                return success;

            SqlBuilder builder = new SqlBuilder(Method.SELECT, UserTable.TableName);
            builder.fieldNames.Add(UserTable.ArTransferNumber);
            builder.where[UserTable.Id] = userId;
                        
            ReadResultContainer result = ExecuteScalar(builder);

            if(result.result == ReadResult.Success)
            {
                if ( ! Convert.IsDBNull(result.scalar))
                {
                    transferNumber = result.scalar as string;
                    if (transferNumber == null || transferNumber == string.Empty)
                        transferNumber = string.Empty;
                    else
                        success = true;
                }
            }
            else
            {
                log.Write(TraceLevel.Warning, 
                    "Error encountered in the GetTransferNumberForUser method, using user Id: '{0}'\n" +
                    "Error message: {1}", userId, result.e.Message);
            }

            return success;
        }
        #endregion

        #region ShouldUserBeRecorded

        /// <summary>
        ///     Determines if a user should be recorded, and if the user should be aware of it
        /// </summary>
        /// <param name="userId">
        ///     The ID of the user
        /// </param>
        /// <param name="record">
        ///     <c>true</c> if the user should be recorded, otherwise <c>false</c>
        /// </param>
        /// <param name="recordingVisible">
        ///     <c>true</c> if the user should be aware of being recorded, otherwise <c>false</c>
        /// </param>
        /// <returns>
        ///     <c>true</c> if the user was able to be identified in the database, otherwise
        ///     <c>false</c>
        /// </returns>
        public bool ShouldUserBeRecorded(uint userId, out bool record, out bool recordingVisible)
        {
            record = false;
            recordingVisible = false;
            if(userId == 0) return false;
            
            SqlBuilder builder = new SqlBuilder(Method.SELECT, UserTable.TableName);
            builder.fieldNames.Add(UserTable.Record);
            builder.fieldNames.Add(UserTable.RecordingVisible);
            builder.fieldNames.Add(UserTable.Status);
            builder.where[UserTable.Id] = userId;

            bool success = true;

            AdvancedReadResultContainer result = ExecuteEasyQuery(builder);

            if(result.result == ReadResult.Success)
            {
                DataTable results = result.results;

                DataRow row = null;
                if(results != null && results.Rows.Count == 1)
                {
                    row = results.Rows[0];
                }
                else if(results != null && results.Rows.Count > 1)
                {
                    row = results.Rows[0];
                    log.Write(TraceLevel.Error, "Duplicate user found when attempting to check recording settings for userId " +
                        "{0}", userId);
                }
                else
                {
                    log.Write(TraceLevel.Error, "No user found when attempting to check recording settings for userId " +
                        "{0}", userId);
                }

                success            &= true;
                record              = Convert.ToBoolean(row[UserTable.Record]);
                recordingVisible    = Convert.ToBoolean(row[UserTable.RecordingVisible]);
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the ShouldUserBeRecorded method, using userId '{0}'\n" +
                    "Error message: {1}", userId, result.e.Message);
                success &= false;
            }

            return success;
        }

            
        #endregion

        #region GetAllUserProperties
//
//        public bool GetUserProperties(
//            uint userId, 
//            out string username, 
//            out uint accountCode, 
//            out uint pin,
//            out string firstName,
//            out string lastName,
//            out string email,
//            out uint status,
//            out DateTime created,
//            out DateTime lastUsed,
//            out int lockoutThreshold,
//            out TimeSpan lockoutDuration,
//            out DateTime lastLockout,
//            out int failedLogins,
//            out int currentActionSessions,
//            out int maxConcurrentSessions,
//            out bool pinChangeRequired,
//            out bool externalAuthEnabled,
//            out string externalAuthDn,
//            out int gmtOffset)
//        {
//            SqlBuilder builder = new SqlBuilder(Method.SELECT, UserTable.TableName);
//            builder.fieldNames.Add(UserTable.Username);
//            builder.fieldNames.Add(UserTable.AccountCode);
//            builder.fieldNames.Add(UserTable.Pin);
//            builder.fieldNames.Add(UserTable.FirstName);
//            builder.fieldNames.Add(UserTable.
        #endregion

        #region GetUsername
    
        public string GetUsername(uint userId)
        {
            if(userId == 0) return null;

            SqlBuilder builder = new SqlBuilder(Method.SELECT, UserTable.TableName);
            builder.fieldNames.Add(UserTable.Username);
            builder.where[UserTable.Id] = userId;
            
            ReadResultContainer result = ExecuteScalar(builder);

            if(result.result == ReadResult.Success)
            {
                return Convert.ToString(result.scalar);
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetUsername method, using userId '{0}'\n" +
                    "Error message: {1}", userId, result.e.Message);
                return null;
            }
        }
        #endregion

        #region GetUserByUsername
    
        public Users.GetUserReturnValues GetUserByUsername(string username, out uint userId, out UserStatus userStatus)
        {
            userId = 0;
            userStatus = UserStatus.Ok;
            Users.GetUserReturnValues returnValue = Users.GetUserReturnValues.NoUser;

            if(username == String.Empty || username == null) return returnValue;

            SqlBuilder builder = new SqlBuilder(Method.SELECT, UserTable.TableName);
            builder.fieldNames.Add(UserTable.Id);
            builder.fieldNames.Add(UserTable.Status);
            builder.where[UserTable.Username] = username;
			builder.where[UserTable.Status] = (int) UserStatus.Ok;
            
            AdvancedReadResultContainer result = ExecuteEasyQuery(builder);

            if(result.result == ReadResult.Success)
            {
                if(result.results == null || result.results.Rows.Count == 0)
                {
                    returnValue = Users.GetUserReturnValues.NoUser;
                }
                else
                {
                    userId = Convert.ToUInt32(result.results.Rows[0][UserTable.Id]);
                    userStatus = (UserStatus) Convert.ToInt32(result.results.Rows[0][UserTable.Status]);
                    returnValue = Users.GetUserReturnValues.success;
                }
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetUserByUsername method, using username '{0}'\n" +
                    "Error message: {1}", username, result.e.Message);
                returnValue = Users.GetUserReturnValues.failure;
            }

            return returnValue;
        }
        #endregion

        #region GetEmail
        
        public string GetEmail(uint userId)
        {
            if(userId < SqlConstants.StandardPrimaryKeySeed) return null;

            SqlBuilder builder = new SqlBuilder(Method.SELECT, UserTable.TableName);
            builder.fieldNames.Add(UserTable.Email);
            builder.where[UserTable.Id] = userId;

            ReadResultContainer result = ExecuteScalar(builder);

            if(result.result == ReadResult.Success)
            {
                return Convert.ToString(result.scalar);
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetEmail method, using userId '{0}'\n" +
                    "Error message: {1}", userId, result.e.Message);
                return null;
            }
        }
        #endregion

        #region GetAccountCode
        public bool GetAccountCode(uint userId, out uint accountCode)
        {
            accountCode = 0;
            SqlBuilder builder = new SqlBuilder(Method.SELECT, UserTable.TableName);
            builder.fieldNames.Add(UserTable.AccountCode);
            builder.where[UserTable.Id] = userId;

            AdvancedReadResultContainer result = ExecuteEasyQuery(builder);

            if(result.result == ReadResult.Success)
            {
                DataTable table = result.results;
                DataRow row = table.Rows[0];
                accountCode = Convert.ToUInt32(row[UserTable.AccountCode]);
                return true;
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetAccountCode method with user Id: {0}\n" +
                    "Error message: {1}", userId, result.e.Message);
                return false;
            }
        }
        #endregion

        #region GetUserOffset
        /// <summary>
        /// Returns a TimeSpan that corresponds to the difference in between between the user's TZ and GMT, with relation to GMT
        /// ie, a user account that's in US/Central will most of the time return: -6:00:00 
        /// </summary>
        /// <param name="userId">account Id</param>
        /// <param name="offset">TimeSpan result</param>
        /// <returns></returns>
        public bool GetUserOffset(uint userId, out TimeSpan offset)
        {
            bool success = false;
            offset = TimeSpan.Zero;

            // SQL query to retrieve time_zone field for user...  
            SqlBuilder userTZQuery = new SqlBuilder(Method.SELECT, UserTable.TableName);
            userTZQuery.fieldNames.Add(UserTable.TimeZone);
            userTZQuery.where[UserTable.Id] = userId;
            userTZQuery.appendSemicolon = false;

            // using above query, construct the CONVERT_TZ query that converts system time to user time
            SqlBuilder innerUser = new SqlBuilder(Method.SELECT, SqlConstants.Tables.Dual.TableName);
            innerUser.fieldNames.Add(string.Format(CONVERT_TZ_FUNCTION_STRING, MYSQL_FUNCTION_NOW, 
                "'" + MYSQL_TIMEZONE_SYSTEM + "'", userTZQuery.ToString()));
            innerUser.appendSemicolon = false;
            
            // construct query to convert system time to GMT 
            SqlBuilder innerGmt = new SqlBuilder(Method.SELECT, SqlConstants.Tables.Dual.TableName);
            innerGmt.fieldNames.Add(string.Format(CONVERT_TZ_FUNCTION_STRING, MYSQL_FUNCTION_NOW,
                "'" + MYSQL_TIMEZONE_SYSTEM + "'", "'" + MYSQL_TIMEZONE_GMT + "'"));
            innerGmt.appendSemicolon = false;
            
            // using the outerUser and outerGMT queries constructed above, construct the final query that invokes the 
            // TimeDiff function that will return a span between the user's current time and GMT time. 
            SqlBuilder query = new SqlBuilder(Method.SELECT, SqlConstants.Tables.Dual.TableName);
            query.fieldNames.Add(string.Format(TIMEDIFF_FUNCTION_STRING, innerUser.ToString(), innerGmt.ToString()));
            
            ReadResultContainer result = ExecuteScalar(query);
            if (result.result == ReadResult.Success)
            {
                try
                {
                    offset = (TimeSpan) result.scalar;
                    success = true;
                }
                catch
                {
                    log.Write(TraceLevel.Warning, "GetUserOffset method could not obtain GMT offset for user Id: " + userId);
                }
            }
            else
            {
                log.Write(TraceLevel.Warning, "Database read error encountered in the GetUserOffset for user Id: " + userId);
            }

            return success;
        }
        #endregion

        #region GetUserTimeZone
        /// <summary>
        /// Retrieves the TimeZone string for a particular user account.
        /// </summary>
        /// <param name="userId">userId of the user whose Time Zone we wish to retrieve</param>
        /// <param name="timeZone">The user's Time Zone string will be written to this variable.</param>
        /// <returns></returns>
        public bool GetUserTimeZone(uint userId, out string timeZone)
        {
            bool success = false;
            timeZone = string.Empty;

            SqlBuilder builder = new SqlBuilder(Method.SELECT, UserTable.TableName);
            builder.fieldNames.Add(UserTable.TimeZone);
            builder.where[UserTable.Id] = userId;

            ReadResultContainer result = ExecuteScalar(builder);

            if (result.result == ReadResult.Success)
            {
                try
                {
                    timeZone = result.scalar as string;
                    success = (timeZone == null || timeZone == string.Empty) ? false : true;
                }
                catch 
                {
                    log.Write(TraceLevel.Warning, "Error encountered in the GetUserTimeZone method for user Id: " + userId);
                }
            }
            else
            {
                log.Write(TraceLevel.Warning, "Database read error encountered in the GetUserTimeZone method for user Id: " + userId);
            }
            
            return success;
        }
        #endregion

        #region GetUserCurrentDateTime
        // Retrieves the current DateTime for a specific user.
        public bool GetUserCurrentDateTime(uint userId, out DateTime dateTime)
        {
            bool success = false;
            dateTime = DateTime.Now;
              
            SqlBuilder inner = new SqlBuilder(Method.SELECT, UserTable.TableName);
            inner.fieldNames.Add(UserTable.TimeZone);
            inner.where[UserTable.Id] = userId;
            inner.appendSemicolon = false;

            SqlBuilder outer = new SqlBuilder(Method.SELECT, SqlConstants.Tables.Dual.TableName);
            outer.fieldNames.Add(string.Format(CONVERT_TZ_FUNCTION_STRING, MYSQL_FUNCTION_NOW, 
                                                            "'" + MYSQL_TIMEZONE_SYSTEM + "'", inner.ToString()));

            ReadResultContainer result = ExecuteScalar(outer);

            if (result.result == ReadResult.Success)
            {
                try
                {
                    dateTime = (DateTime) result.scalar;
                    success = true;
                }
                catch
                {
                    log.Write(TraceLevel.Warning, "GetUserCurrentDateTime method could not obtain current DateTime for user Id: " + userId);
                }
            }
            else
            {
                log.Write(TraceLevel.Warning, "Database read error encountered in the GetUserCurrentDateTime for user Id: " + userId);
            }

            return success;
        }
        #endregion
               
        #region IsOneNonDeletedRow
        /// <summary>
        ///     Search through all returned users, determining if one and only one
        ///     user is non-deleted.  (Deleted users stay in the as_users table)
        /// </summary>
        protected bool IsOneNonDeletedRow(DataTable results, out DataRow nonDeletedUser, out bool multipleNonDeletedUsers)
        {
            nonDeletedUser = null;
            multipleNonDeletedUsers = false;
            bool success = false;
            
            if(results != null)
            {
                foreach(DataRow row in results.Rows)
                {
                    UserStatus status = (UserStatus) Convert.ToUInt32(row[UserTable.Status]);

                    if(status != UserStatus.Deleted)
                    {
                        if(nonDeletedUser != null)
                        {
                            // Found more than one non-deleted user! Failure
                            nonDeletedUser = null;
                            multipleNonDeletedUsers = true;
                            success = false;
                            break;
                        }

                        nonDeletedUser = row;
                        success = true;
                    }
                }
            }
            
            return success;
        }
        #endregion

		#region IsSessionAllowed
		/// <summary>
		///     Returns true is number of concurrent sessions < max number sessions
		/// </summary>
		public bool IsSessionAllowed(uint userId, out bool sessionAllowed)
		{
			sessionAllowed = false;
			bool success = false;

			SqlBuilder builder = new SqlBuilder(Method.SELECT, UserTable.TableName);
			builder.fieldNames.Add(UserTable.NumConcurrentSessions);
			builder.fieldNames.Add(UserTable.MaxConcurrentSessions);
			builder.where[UserTable.Id] = userId;

			AdvancedReadResultContainer result = ExecuteEasyQuery(builder);

			if (result.result == ReadResult.Success)
			{
				try
				{
					int currentSessions = Convert.ToInt32(result.results.Rows[0][UserTable.NumConcurrentSessions]);
					int maxSessions = Convert.ToInt32(result.results.Rows[0][UserTable.MaxConcurrentSessions]);
					if (maxSessions > 0)
						sessionAllowed = currentSessions < maxSessions;
					else
						sessionAllowed = true;

					success = true;
				}
				catch 
				{
					log.Write(TraceLevel.Warning, "Error encountered in the IsSessionAllowed method for user Id: " + userId);
				}
			}
			else
			{
				log.Write(TraceLevel.Warning, "Database read error encountered in the IsSessionAllowed method for user Id: " + userId);
			}
            
			return success;

		}
		#endregion

        #region StartCall
        /// <summary>
        ///     Starts a call log for a user, which results in an increment of the 'placed_calls' field
        /// </summary>
        public bool StartCall(uint userId)
        {
            bool success;

            SqlBuilder builder = new SqlBuilder(Method.UPDATE, UserTable.TableName);
            builder.AddFieldValue(UserTable.PlacedCalls, new SqlBuilder.PreformattedValue(UserTable.PlacedCalls + " + 1"));
            builder.where[UserTable.Id] = userId;

            WriteResultContainer result = ExecuteCommand(builder.ToString());

            if(result.result == WriteResult.Success)
            {
                success = true;
            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the StartCall method, using userId '{0}'\n" +
                    "Error message: {1}", userId, result.e.Message);
                success = false;
            }
            else // (result.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("StartCall"));
                success = false;
            }

            return success;
        }
        #endregion

        #region EndCall
        /// <summary>
        ///     Starts a call log for a user, which results in an increment of the 'placed_calls' field
        /// </summary>
        public bool EndCall(uint userId, uint callId)
        {
            bool success;

            string empheralDuration = "duration";
            string query = String.Format(
                "SELECT {0} - {1} as {2}, {3} WHERE {4} = {5}",
                CallTable.End,
                CallTable.Start,
                empheralDuration,
                CallTable.EndReason,
                CallTable.Id,
                callId);

            AdvancedReadResultContainer readResult = ExecuteEasyQuery(query);

            int duration = 0;
            EndReason endReason = EndReason.Unreachable;

            if(readResult.result == ReadResult.Success)
            {
                if(readResult.results == null || readResult.results.Rows.Count == 0)
                {
                    log.Write(TraceLevel.Error, "No call record of ID {0} found.  Not incrementing user account information with call count and call duration", callId);
                    success = false;
                }
                else
                {
                    duration = Convert.ToInt32(readResult.results.Rows[0]["duration"]);
                    endReason = (EndReason) Convert.ToInt32(readResult.results.Rows[0][CallTable.EndReason]);
                    success = true;
                }
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the EndCall method, using call ID '{0}'\n" +
                    "Error message: {1}", callId, readResult.e.Message);
                success = false;
            }

            if(success == false) return false;

            if(endReason == EndReason.Normal)
            {
                SqlBuilder builder = new SqlBuilder(Method.UPDATE, UserTable.TableName);
                builder.AddFieldValue(UserTable.SuccessfulCalls, new SqlBuilder.PreformattedValue(UserTable.SuccessfulCalls + " + 1"));
                builder.AddFieldValue(UserTable.TotalCallTime, new SqlBuilder.PreformattedValue(UserTable.TotalCallTime + " + " + duration));
                builder.where[UserTable.Id] = userId;

                WriteResultContainer result = ExecuteCommand(builder.ToString());

                if(result.result == WriteResult.Success)
                {
                    success = true;
                }
                else if(result.result == WriteResult.DbFailure)
                {
                    log.Write(TraceLevel.Error, 
                        "Error encountered in the EndCall method, using userId '{0}'\n" +
                        "Error message: {1}", userId, result.e.Message);
                    success = false;
                }
                else // (result.result == WriteResult.PublisherDown)
                {
                    log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("EndCall"));
                    success = false;
                }
            }
           
            return success;
        }
        #endregion
    } 
}
