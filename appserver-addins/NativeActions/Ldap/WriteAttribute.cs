using System;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Core;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework.Collections;

using Novell.Directory.Ldap;

using Package = Metreos.Interfaces.PackageDefinitions.Ldap.Actions.WriteAttribute;

namespace Metreos.Native.Ldap
{
    /// <summary>
    ///     Defines an interface to write to an attribute of an entry in an LDAP directory
    /// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.Ldap.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.Ldap.Globals.PACKAGE_DESCRIPTION)]
    public class WriteAttribute : INativeAction
    {
        protected enum ReturnValues
        {
            failure,
            success,
            ConnectionFailure,
            AuthenticationFailure,
            WriteFailure
        }

        public enum WriteType
        {
            Add,
            Delete,
            Replace
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

        [ActionParamField(Package.Params.Dn.DISPLAY, Package.Params.Dn.DESCRIPTION, true, Package.Params.Dn.DEFAULT)]
        public string Dn { set { dn = value; } }
        private string dn;

        [ActionParamField(Package.Params.Type.DISPLAY, Package.Params.Type.DESCRIPTION, true, Package.Params.Type.DEFAULT)]
        public WriteType Type { set { type = value; } }
        private WriteType type;

        [ActionParamField(Package.Params.AttributeName.DISPLAY, Package.Params.AttributeName.DESCRIPTION, true, Package.Params.AttributeName.DEFAULT)]
        public string AttributeName { set { attributeName = value; } }
        private string attributeName;

        [ActionParamField(Package.Params.Value.DISPLAY, Package.Params.Value.DESCRIPTION, false, Package.Params.Value.DEFAULT)]
        public  string Value { set { singleValue = value; } }
        private string singleValue;

        [ActionParamField(Package.Params.Values.DISPLAY, Package.Params.Values.DESCRIPTION, false, Package.Params.Values.DEFAULT)]
        public StringCollection Values { set { values = value; } }
        private StringCollection values;

        [ActionParamField(Package.Params.Version.DISPLAY, Package.Params.Version.DESCRIPTION, false, Package.Params.Version.DEFAULT)]
        public int Version { set { version = value; } }
        private int version;

        [ResultDataField(Package.Results.ErrorCode.DISPLAY, Package.Results.ErrorCode.DESCRIPTION)]
        public int ErrorCode { get { return errorCode; } }
        private int errorCode;

		[ResultDataField(Package.Results.ErrorMessage.DISPLAY, Package.Results.ErrorMessage.DESCRIPTION)]
		public string ErrorMessage { get { return errorMessage; } }
		private string errorMessage;

        public WriteAttribute()
        {
            Clear();
        }

        [ReturnValue(typeof(ReturnValues), "Success/Failure, InvalidUsernamePin, LdapFailure")]
        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility config)
        {
            ReturnValues returnValue = WriteLdap();
   
            return returnValue.ToString();  
        } 

        protected ReturnValues WriteLdap()
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
                    return ReturnValues.AuthenticationFailure;
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Unable to bind to LDAP server");
                    log.Write(TraceLevel.Error, "General authentication error: " + e);
					return ReturnValues.failure; // not an authentication error necessarily--maybe TCP connection whacked at this moment...
				}
               
                try
                {
                    int modificationType = ConvertWriteEnumToLdapWriteType(type);
                    LdapAttribute writeAttribute = null;
                    if(values == null && singleValue != null)
                    {
                        writeAttribute = new LdapAttribute(attributeName, singleValue);
                    }
                    else if(singleValue == null)
                    {
                        writeAttribute = new LdapAttribute(attributeName);
                    }
                    else
                    {
                        string[] writeValues = new string[values.Count];
                        values.CopyTo(writeValues, 0);
                        writeAttribute = new LdapAttribute(attributeName, writeValues);
                    }
                    LdapModification writeCommand = new LdapModification(modificationType, writeAttribute);
                    con.Modify(dn, writeCommand);
                }
                catch(LdapException e)
                {
                    log.Write(TraceLevel.Error, "Unable to write to LDAP server");
                    log.Write(TraceLevel.Error, "LDAP write error: " + e.LdapErrorMessage);
                    log.Write(TraceLevel.Error, "LDAP error code: " + GetErrorCode(e));
                    errorCode = GetErrorInt(e);
					errorMessage = e.LdapErrorMessage;
                    return ReturnValues.WriteFailure;
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Unable to write to LDAP server");
                    log.Write(TraceLevel.Error, "General write error: " + e);
					return ReturnValues.failure; // not an authentication error necessarily--maybe TCP connection whacked at this moment...
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
            username        = null;
            password        = null;
            ldapHost        = null;
            singleValue     = null;
            values          = null;
            dn              = null;
            type            = WriteType.Replace;
            ldapPort        = 0;
            errorCode       = 0;
			errorMessage    = null;
            version         = 3;
        }

        private int ConvertWriteEnumToLdapWriteType(WriteType type)
        {
            switch(type)
            {
                case WriteType.Add:
                    return LdapModification.ADD;

                case WriteType.Delete:
                    return LdapModification.DELETE;

                case WriteType.Replace:
                    return LdapModification.REPLACE;

                default:
                    return LdapModification.REPLACE;
            }
        }

    }
}
