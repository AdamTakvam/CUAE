using System;
using System.Data;
using System.Diagnostics;
using System.Collections;

using Metreos.Core;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework.Collections;

using Novell.Directory.Ldap;

using Package = Metreos.Interfaces.PackageDefinitions.Ldap.Actions.AuthenticateUser;

namespace Metreos.Native.Ldap
{
    /// <summary>
    ///     Defines an interface to make bind a user against LDAP
    /// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.Ldap.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.Ldap.Globals.PACKAGE_DESCRIPTION)]
    public class AuthenticateUser : INativeAction
    {
        protected enum ReturnValues
        {
            failure,
            success,
            ConnectionFailure,
            //AuthenticationFailure, // Doesn't make sense to have this path, unlike other LDAP actions.  success should be returned if bind fails or succeeds.
        }

        private LogWriter log;
        public LogWriter Log { set { log = value; } }
 
        [ActionParamField(Package.Params.LdapServerHost.DISPLAY, Package.Params.LdapServerHost.DESCRIPTION, true, Package.Params.LdapServerHost.DEFAULT)]
        public  string LdapServerHost { set { ldapHost = value; } }
        private string ldapHost;

        [ActionParamField(Package.Params.LdapServerPort.DISPLAY, Package.Params.LdapServerPort.DESCRIPTION, false, Package.Params.LdapServerPort.DEFAULT)]
        public  uint LdapServerPort { set { ldapPort = value; } } 
        private uint ldapPort;

        [ActionParamField(Package.Params.Username.DISPLAY, Package.Params.Username.DESCRIPTION, false, Package.Params.Username.DEFAULT)]
        public  string Username{ set { username = value; } }
        private string username;

        [ActionParamField(Package.Params.Password.DISPLAY, Package.Params.Password.DESCRIPTION, false, Package.Params.Password.DEFAULT)]
        public  string Password { set { password = value; } } 
        private string password;

        [ActionParamField(Package.Params.Version.DISPLAY, Package.Params.Version.DESCRIPTION, false, Package.Params.Version.DEFAULT)]
        public int Version { set { version = value; } }
        private int version;

        [ResultDataField(Package.Results.Authenticated.DISPLAY, Package.Results.Authenticated.DESCRIPTION)]
        public bool Authenticated { get { return authenticated; } }
        private bool authenticated;

        [ResultDataField(Package.Results.ErrorCode.DISPLAY, Package.Results.ErrorCode.DESCRIPTION)]
        public int ErrorCode { get { return errorCode; } }
        private int errorCode;

		[ResultDataField(Package.Results.ErrorMessage.DISPLAY, Package.Results.ErrorMessage.DESCRIPTION)]
		public string ErrorMessage { get { return errorMessage; } }
		private string errorMessage;

        public AuthenticateUser()
        {
            Clear();
        }

        [ReturnValue(typeof(ReturnValues), "Success/Failure, InvalidUsernamePin, LdapFailure")]
        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility config)
        {
            ReturnValues returnValue = BindLdap();
   
            return returnValue.ToString();
        } 

        protected ReturnValues BindLdap()
        {
            LdapConnection con = null;

            try
            {
                try
                {
                    con = new LdapConnection();
                    con.Connect(ldapHost, (int)ldapPort);
                }
                catch(LdapException e)
                {
                    log.Write(TraceLevel.Error, "Unable to connect to LDAP server");
                    log.Write(TraceLevel.Error, "LDAP connection error: " + e.LdapErrorMessage);
                    log.Write(TraceLevel.Error, "LDAP error code: " + GetErrorCode(e));
                    errorCode = GetErrorInt(e);
					errorMessage = e.LdapErrorMessage;
                    return ReturnValues.ConnectionFailure;
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Unable to connect to LDAP server");
                    log.Write(TraceLevel.Error, "General connection error: " + e);
                    return ReturnValues.ConnectionFailure;
                }

                try
                {
                    con.Bind(version, username, password);
                }
                catch(LdapException e)
                {
                    log.Write(TraceLevel.Error, "Unable to bind to LDAP server");
                    log.Write(TraceLevel.Error, "LDAPB authentication error: " + e.LdapErrorMessage);
                    log.Write(TraceLevel.Error, "LDAP error code: " + GetErrorCode(e));
                    errorCode = GetErrorInt(e);
					errorMessage = e.LdapErrorMessage;
                    return ReturnValues.success; 
					// this is a success because the bind operation was engaged correctly--it's just that it failed to authenticate.
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Unable to bind to LDAP server");
                    log.Write(TraceLevel.Error, "General authentication error: " + e);
                    return ReturnValues.failure;
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Generic error: " + e);
                return ReturnValues.failure;
            }
            finally
            {
                if( (con != null) &&
                    (con.Connected == true))
                {
                    con.Disconnect();
                }
            }

			authenticated = true;
            return ReturnValues.success;
        }

        public bool ValidateInput()
        {
            return true;
        }

        public string GetErrorCode(LdapException e)
        {
            try
            {
                return int.Parse(e.Message).ToString();
            }
            catch
            {
                return "Unknown";
            }
        }

        public int GetErrorInt(LdapException e)
        {
            try
            {
                return int.Parse(e.Message);
            }
            catch
            {
                return 0;
            }
        }

        public void Clear()
        {
			authenticated   = false;
            username        = null;
            password        = null;
            ldapHost        = null;
            ldapPort        = 0;
            errorCode       = 0;
			errorMessage    = null;
            version         = 3;
        }
    }
}
