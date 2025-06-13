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

using Package = Metreos.Interfaces.PackageDefinitions.Ldap.Actions.Query;

namespace Metreos.Native.Ldap
{
    /// <summary>
    ///     Defines an interface to make generic LDAP queries
    /// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.Ldap.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.Ldap.Globals.PACKAGE_DESCRIPTION)]
    public class Query : INativeAction
    {
        protected enum ReturnValues
        {
            failure,
            success,
            ConnectionFailure,
            AuthenticationFailure,
            SearchFailure,
            NoResults,
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

        [ActionParamField(Package.Params.BaseDn.DISPLAY, Package.Params.BaseDn.DESCRIPTION, false, Package.Params.BaseDn.DEFAULT)]
        public string BaseDn { set { baseDn = value; } }
        private string baseDn;

        [ActionParamField(Package.Params.SearchFilter.DISPLAY, Package.Params.SearchFilter.DESCRIPTION, false, Package.Params.SearchFilter.DEFAULT)]
        public string SearchFilter { set { searchFilter = value; } }
        private string searchFilter;

        [ActionParamField("Specify explicit attributes to return only", false)]
        public string[] Attributes { set { attributes = value; } }
        private string[] attributes;

        [ActionParamField(Package.Params.Version.DISPLAY, Package.Params.Version.DESCRIPTION, false, Package.Params.Version.DEFAULT)]
        public int Version { set { version = value; } }
        private int version;

        [ResultDataField(Package.Results.SearchResults.DISPLAY, Package.Results.SearchResults.DESCRIPTION)]
        public DataTable SearchResults { get { return searchResults; } }
        private DataTable searchResults;

        [ResultDataField(Package.Results.ErrorCode.DISPLAY, Package.Results.ErrorCode.DESCRIPTION)]
        public int ErrorCode { get { return errorCode; } }
        private int errorCode;

		[ResultDataField(Package.Results.ErrorMessage.DISPLAY, Package.Results.ErrorMessage.DESCRIPTION)]
		public string ErrorMessage { get { return errorMessage; } }
		private string errorMessage;

        public Query()
        {
            Clear();
        }

        [ReturnValue(typeof(ReturnValues), "Success/Failure, InvalidUsernamePin, LdapFailure, NoResults")]
        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility config)
        {
            ReturnValues returnValue = QueryLdap();
   
            return returnValue.ToString();
           
        } 

        protected ReturnValues QueryLdap()
        {
            searchResults = new DataTable();
            LdapConnection con = null;
            LdapSearchResults results = null;

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
                    log.Write(TraceLevel.Error, "LDAP authentication error: " + e.LdapErrorMessage);
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
                    results = con.Search(
                        baseDn, 
                        LdapConnection.SCOPE_SUB,   // Search the base DN and all children.
                        searchFilter,
                        attributes,
                        false);
                }
                catch(LdapException e)
                {
                    log.Write(TraceLevel.Error, "Unable to search LDAP server");
                    log.Write(TraceLevel.Error, "LDAP search error: " + e.LdapErrorMessage);
                    log.Write(TraceLevel.Error, "LDAP error code: " + GetErrorCode(e));
                    errorCode = GetErrorInt(e);
					errorMessage = e.LdapErrorMessage;
                    return ReturnValues.SearchFailure;
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Unable to search LDAP server");
                    log.Write(TraceLevel.Error, "General search error: " + e);
                    return ReturnValues.failure; // not an authentication error necessarily--maybe TCP connection whacked at this moment...
                }

                LdapEntry entry = null;
                LdapAttributeSet attributeSet = null;
                IEnumerator attrEnum = null;
                LdapAttribute attr = null;

                try
                {
                    searchResults.BeginLoadData();

                    while(results.hasMore())
                    {
                        DataRow row = searchResults.NewRow();

                        entry = results.next();
                        attributeSet = entry.getAttributeSet();

                        attrEnum = attributeSet.GetEnumerator();
                        while(attrEnum.MoveNext())
                        {
                            attr = (LdapAttribute)attrEnum.Current;

                            if(searchResults.Columns.Contains(attr.Name) == false)
                            {
                                searchResults.Columns.Add(attr.Name, typeof(string));
                            }

                            row[attr.Name] = attr.StringValue;
                        }

                        searchResults.Rows.Add(row);
                    }
                }

                catch(LdapException e)
                {
                    log.Write(TraceLevel.Error, "Unable to parse LDAP search results");
                    log.Write(TraceLevel.Error, "LDAP search error: " + e.LdapErrorMessage);
                    log.Write(TraceLevel.Error, "LDAP error code: " + GetErrorCode(e));
                    errorCode = GetErrorInt(e);
					errorMessage = e.LdapErrorMessage;
                    return ReturnValues.SearchFailure;
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Unable to parse LDAP search results");
                    log.Write(TraceLevel.Error, "General search error: " + e);
                    return ReturnValues.failure;
                }

                if(searchResults == null || searchResults.Rows.Count == 0)
                {
                    return ReturnValues.NoResults;
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Generic error: " + e);
                return ReturnValues.failure;
            }
            finally
            {
                if(searchResults != null)
                {
                    searchResults.EndLoadData();
                }

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
            attributes      = null;
            searchFilter    = null;
            baseDn          = null;
			errorMessage    = null;
            searchResults   = new DataTable();
            baseDn          = String.Empty;
            ldapPort        = 0;
            errorCode       = 0;
            version         = 3;
        }
    }
}
