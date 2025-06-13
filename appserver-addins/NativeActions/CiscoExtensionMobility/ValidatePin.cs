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
using Metreos.Types.CiscoExtensionMobility;

using Novell.Directory.Ldap;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoExtensionMobility.Actions.ValidatePin;

namespace Metreos.Native.CiscoExtensionMobility
{
    /// <summary>
    ///     Validates a user's pin against Cisco LDAP
    /// </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.CiscoExtensionMobility.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.CiscoExtensionMobility.Globals.PACKAGE_DESCRIPTION)]
    public class ValidatePin : INativeAction
    {
        public abstract class Consts
        {
            public const string OC_CISCO_USER_ATTR      = "ciscoocUser";
            public const string OC_PERSON_ATTR          = "person";
            public const string CISCO_USER_PRO_ATTR     = "ciscoatUserProfile";
            public const string CISCO_USER_APP_PRO_ATTR = "ciscoatAppProfile";
            public const string EMPLOYEE_NUM_ATTR       = "employeeNumber";
            public const string MAIL_ATTR               = "mail";
            public const string CISCO_PIN_ATTR          = "ciscoCCNatPIN";

            public const string EMPLOYEE_NUM_COL        = "Employee Number";
            public const string ACCOUNT_NAME_COL        = "Network Account ID";
            public const string FIRST_NAME_COL          = "First Name";
            public const string LAST_NAME_COL           = "Last Name";
            public const string MAIL_COL                = "Email Address";
            public const string CISCO_APP_PROFILE_COL   = "Cisco Profile DN";
            public const string DEF_BASE_SEARCH_DIR     = "ou=Users,o=cisco.com";
            public const string STANDARD_CISCO_USERNAME = "cn";

            public const string SEARCH_FILTER   =
                "(&(objectClass={0})"           +
                "({1}={2}))";                  
                // Commented out because of emergency BearingPoint reaction.
                // TODO; investigate more robust solution for this
//                "(sn={3})"                      +
//                "(givenName={4}))";
        }

        protected enum ReturnValues
        {
            success,
            failure,
            InvalidUsernamePin
        }

        private LogWriter log;
        public LogWriter Log { set { log = value; } }
            

        [ActionParamField(Package.Params.Username.DISPLAY, Package.Params.Username.DESCRIPTION, true, Package.Params.Username.DEFAULT)]
        public  string Username { set { username = value; } }
        private string username;

        [ActionParamField(Package.Params.Pin.DISPLAY, Package.Params.Pin.DESCRIPTION, true, Package.Params.Pin.DEFAULT)]
        public  string Pin { set { pin = value; } }
        private string pin;

        [ActionParamField(Package.Params.CustomUserAttr.DISPLAY, Package.Params.CustomUserAttr.DESCRIPTION, false, Package.Params.CustomUserAttr.DEFAULT)]
        public  string CustomUserAttr {set { customUserAttr = value; } }
        private string customUserAttr;

        [ActionParamField(Package.Params.LdapServerHost.DISPLAY, Package.Params.LdapServerHost.DESCRIPTION, true, Package.Params.LdapServerHost.DEFAULT)]
        public  string LdapServerHost { set { ldapHost = value; } }
        private string ldapHost;

        [ActionParamField(Package.Params.LdapServerPort.DISPLAY, Package.Params.LdapServerPort.DESCRIPTION, true, Package.Params.LdapServerPort.DEFAULT)]
        public  uint LdapServerPort { set { ldapPort = value; } } 
        private uint ldapPort;

        [ActionParamField(Package.Params.LdapUsername.DISPLAY, Package.Params.LdapUsername.DESCRIPTION, true, Package.Params.LdapUsername.DEFAULT)]
        public  string LdapUsername{ set { ldapUsername = value; } }
        private string ldapUsername;

        [ActionParamField(Package.Params.LdapPassword.DISPLAY, Package.Params.LdapPassword.DESCRIPTION, true, Package.Params.LdapPassword.DEFAULT)]
        public  string LdapPassword { set { ldapPassword = value; } } 
        private string ldapPassword;

        [ActionParamField(Package.Params.LdapBaseDn.DISPLAY, Package.Params.LdapBaseDn.DESCRIPTION, false, Package.Params.LdapBaseDn.DEFAULT)]
        public string LdapBaseDn { set { ldapBaseDn = value; } }
        private string ldapBaseDn;

        public ValidatePin()
        {
            Clear();
        }

        [ReturnValue(typeof(ReturnValues), "Success/Failure, and InvalidPin")]
        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility config)
        {
            DataTable results = null;
            ReturnValues returnValue = QueryUsers(
                ldapBaseDn, ldapHost, ldapPort, 
                ldapUsername, ldapPassword, username, null, null, customUserAttr, out results);
          
            if(returnValue != ReturnValues.success)
            {
                return returnValue.ToString();
            }

            DataRow result = results.Rows[0];
            string accountName = result[Consts.CISCO_APP_PROFILE_COL] as string;

            if(accountName == null)
            {
                return ReturnValues.InvalidUsernamePin.ToString();
            }

            returnValue = AuthenticateUser(ldapHost, ldapPort, accountName, pin, log);

            if(returnValue != ReturnValues.success)
            {
                // Try one last time to authenticate
                string foundPin = SearchAppProfileForPin(ldapUsername, ldapPassword, ldapHost, ldapPort, accountName);
                if(pin == foundPin)
                {
                    return ReturnValues.success.ToString();
                }
            }

            return returnValue.ToString();
           
        } 

        /// <summary>
        /// Queries an LDAP server for all users that match certain
        /// criteria.
        /// </summary>
        /// <param name="searchBaseDN">The base DN of the search. This 
        /// acts as the "root" node of the search.</param>
        /// <param name="ldapServer">The LDAP server IP address.</param>
        /// <param name="ldapPortStr">The LDAP server port number.</param>
        /// <param name="ldapUsernameDN">The username to login to the LDAP 
        /// server with.  This must be a fully qualified LDAP user DN.</param>
        /// <param name="ldapPassword">The password to use to login.</param>
        /// <param name="commonName">The common name search filter to use.
        /// If null or an empty string are specified, '*' will be used.</param>
        /// <param name="surName">The surname to filter the search on.
        /// If null or an empty string are specified '*' will be used.</param>
        /// <param name="givenName">The given name to filter the search on.
        /// If null or an empty string are specified '*' will be used.</param>
        protected ReturnValues QueryUsers(
            string searchBaseDN,
            string ldapServer,
            uint ldapPort,
            string ldapUsernameDN,
            string ldapPassword,
            string commonName,
            string surName,
            string givenName,
            string customUserAttr,
            out DataTable tableResult)
        {
            tableResult = new DataTable();
            LdapConnection con = null;

            string[] attributesToReturn 
                = new string[] { Consts.EMPLOYEE_NUM_ATTR, Consts.CISCO_USER_PRO_ATTR, Consts.MAIL_ATTR, "cn", "sn", "givenName" };

            try
            {
                con = new LdapConnection();
                con.Connect(ldapServer, (int)ldapPort);
                con.Bind(ldapUsernameDN, ldapPassword);
                
                string searchFilter = BuildSearchFilter(commonName, surName, givenName, customUserAttr);

                log.Write(TraceLevel.Info, "Search filter is: " + searchFilter);

                log.Write(TraceLevel.Info, "Searching for QueryUsers");

                LdapSearchResults results = con.Search(
                    searchBaseDN, 
                    LdapConnection.SCOPE_SUB,   // Search the base DN and all children.
                    searchFilter,
                    attributesToReturn,
                    false);

                LdapEntry entry = null;
                LdapAttributeSet attributeSet = null;
                IEnumerator attrEnum = null;
                LdapAttribute attr = null;

                tableResult.Columns.Add(Consts.ACCOUNT_NAME_COL);
                tableResult.Columns.Add(Consts.LAST_NAME_COL);
                tableResult.Columns.Add(Consts.FIRST_NAME_COL);
                tableResult.Columns.Add(Consts.EMPLOYEE_NUM_COL);
                tableResult.Columns.Add(Consts.MAIL_COL);
                tableResult.Columns.Add(Consts.CISCO_APP_PROFILE_COL);

                tableResult.BeginLoadData();

                string subSearchBaseDN = null;
                string ciscoAppProfileAttrValue = null;

                while(results.hasMore())
                {
                    DataRow row = tableResult.NewRow();

                    entry = results.next();
                    attributeSet = entry.getAttributeSet();

                    attrEnum = attributeSet.GetEnumerator();
                    while(attrEnum.MoveNext())
                    {
                        attr = (LdapAttribute)attrEnum.Current;

                        switch(attr.Name)
                        {
                            case "cn":
                                row[Consts.ACCOUNT_NAME_COL] = attr.StringValue;
                                break;
                                
                            case "sn":
                                row[Consts.LAST_NAME_COL] = attr.StringValue;
                                break;

                            case "givenName":
                                row[Consts.FIRST_NAME_COL] = attr.StringValue;
                                break;

                            case Consts.EMPLOYEE_NUM_ATTR:
                                row[Consts.EMPLOYEE_NUM_COL] = attr.StringValue;
                                break;

                            case Consts.MAIL_ATTR:
                                row[Consts.MAIL_COL] = attr.StringValue;
                                break;

                            case Consts.CISCO_USER_PRO_ATTR:
                                subSearchBaseDN = attr.StringValue;
                                break;
                        }
                    }

                    // Retrieve the user's CCN application profile string. 
                    // This is what will be used to authenticate the user
                    // against his individual CallManager PIN code.
                    ciscoAppProfileAttrValue 
                        = GetCiscoAppProfileFromUserProfile(subSearchBaseDN, con);

                    row[Consts.CISCO_APP_PROFILE_COL] = ciscoAppProfileAttrValue;

                    tableResult.Rows.Add(row);
                }
            }
            catch(LdapException e) 
            {  
                log.Write(TraceLevel.Error, "LDAP QueryUsers error message: " + e.LdapErrorMessage);
                return ReturnValues.failure;
            }
            catch(Exception e) 
            { 
                log.Write(TraceLevel.Error, "LDAP QueryUsers failed: " + e.Message);
                return ReturnValues.failure;
            }
            finally
            {
                tableResult.EndLoadData();

                if( (con != null) &&
                    (con.Connected == true))
                {
                    con.Disconnect();
                }
            }

            if(tableResult == null || tableResult.Rows.Count == 0)
            {
                return ReturnValues.InvalidUsernamePin;
            }

            return ReturnValues.success;
        }

        /// <summary>
        /// Retrieves the value of the ciscoatAppProfile LDAP attribute from
        /// a user's CCN profile.
        /// </summary>
        /// <param name="userBaseDN">The base DN of the user whose profile
        /// is being retrieved.</param>
        /// <param name="con">The LDAP connection to perform the query on.</param>
        /// <returns>A string containing the value of the ciscoatAppProfile
        /// attribute.  If no attribute is found or the query fails, null
        /// is returned.</returns>
        protected string GetCiscoAppProfileFromUserProfile(
            string userBaseDN,
            LdapConnection con)
        {
            if(userBaseDN == null) { return null; };

            string ciscoAppProfileAttrValue = null;
            string[] attributesToReturn = new string[] { Consts.CISCO_USER_APP_PRO_ATTR };

            try
            {
                LdapSearchResults results = con.Search(
                    userBaseDN,
                    LdapConnection.SCOPE_BASE,  // Only search the specified DN, not its children.
                    "(objectClass=*)",          // Don't filter out anything.
                    attributesToReturn,
                    false);

                LdapEntry entry = null;
                LdapAttributeSet attributeSet = null;
                IEnumerator attrEnum = null;
                LdapAttribute attr = null;

                while(results.hasMore())
                {
                    entry = results.next();
                    attributeSet = entry.getAttributeSet();
                    attrEnum = attributeSet.GetEnumerator();
                    while(attrEnum.MoveNext())
                    {
                        attr = (LdapAttribute)attrEnum.Current;

                        if(attr.Name == Consts.CISCO_USER_APP_PRO_ATTR)
                        {
                            ciscoAppProfileAttrValue = attr.StringValue;
                            break;
                        }
                    }
                }
            }
            catch(LdapException e) 
            {  
                log.Write(TraceLevel.Error, "LDAP GetCiscoAppProfileFromUserProfile error message: " + e.LdapErrorMessage);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "LDAP GetCiscoAppProfileFromUserProfile failed: " + e.Message);
            }

            return ciscoAppProfileAttrValue;
        }

        protected string SearchAppProfileForPin(
            string ldapUsername, 
            string ldapPassword,
            string ldapServer,
            uint ldapPort,
            string userBaseDN)
        {
            if(userBaseDN == null) { return null; };

            string ciscoAppProfileAttrValue = null;
            string[] attributesToReturn = new string[] { Consts.CISCO_PIN_ATTR };

            LdapConnection con = null;

            try 
            {
                con = new LdapConnection();
                con.Connect(ldapServer, (int)ldapPort);
                con.Bind(ldapUsername, ldapPassword);
                   
                LdapSearchResults results = con.Search(
                    userBaseDN,
                    LdapConnection.SCOPE_BASE,  // Only search the specified DN, not its children.
                    "(objectClass=*)",          // Don't filter out anything.
                    attributesToReturn,
                    false);

                LdapEntry entry = null;
                LdapAttributeSet attributeSet = null;
                IEnumerator attrEnum = null;
                LdapAttribute attr = null;

                while(results.hasMore())
                {
                    entry = results.next();
                    attributeSet = entry.getAttributeSet();
                    attrEnum = attributeSet.GetEnumerator();
                    while(attrEnum.MoveNext()) 
                    {
                        attr = (LdapAttribute)attrEnum.Current;

                        if(attr.Name == Consts.CISCO_PIN_ATTR) 
                        {
                            ciscoAppProfileAttrValue = attr.StringValue;
                            break;
                        }
                    }
                }
            }
            catch(LdapException e)
            {  
                log.Write(TraceLevel.Error, "LDAP SearchAppProfileForPin error message: " + e.LdapErrorMessage);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "LDAP SearchAppProfileForPin failed: " + e.Message);
            }
            finally
            {

                if( (con != null) &&
                    (con.Connected == true)) 
                {
                    con.Disconnect();
                }
            }

            return ciscoAppProfileAttrValue;
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
        /// <returns>True if authentication was successfull, 
        /// false otherwise.</returns>
        protected ReturnValues AuthenticateUser(
            string ldapServer,
            uint ldapPort,
            string authDn,
            string password, 
            LogWriter log)
        {
            LdapConnection con = null;

            try
            {
                con = new LdapConnection();
                con.Connect(ldapServer, (int)ldapPort);
                con.Bind(authDn, password);             
            }
            catch(LdapException e) 
            {   
                log.Write(TraceLevel.Error, "LDAP Authentication error message: " + e.LdapErrorMessage);
                log.Write(TraceLevel.Error, "LDAP Authentication error code: " + e.ResultCode);
                return ReturnValues.InvalidUsernamePin;
            }
            catch(Exception e) 
            { 
                log.Write(TraceLevel.Error, "LDAP Authentication failed: " + e.Message);
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

        /// <summary>
        /// Constructs a search filter string for an LDAP query.
        /// </summary>
        /// <remarks>
        /// The format of the generated string will be:
        ///    (& (objectClass=person)
        ///       (objectClass=ciscoocUser)
        ///       (cn={User Specified})
        ///       (sn={User Specified})
        ///       (givenName={User Specified}))
        /// 
        /// NOTE: Formatted for readability only.
        /// </remarks>
        /// <param name="commonName">The filter criteria for the 
        /// common name (cn) filter.</param>
        /// <param name="surName">The filter criteria for the surname
        /// (sn) filter.</param>
        /// <param name="givenName">The filter criteria for the
        /// given name (givenName) filter.</param>
        /// <param name="customUserAttr">Specify null or empty string to 
        /// perform default Cisco lookup (username is stored in the 'cn' attribute), 
        /// otherwise, an overridden attribute will be used to find username</param>
        /// <returns>A properly formatted LDAP search filter.</returns>
        protected static string BuildSearchFilter(
            string commonName,
            string surName,
            string givenName,
            string customUserAttr)
        {
            SanitizeFilterCriteria(ref commonName);
            SanitizeFilterCriteria(ref surName);
            SanitizeFilterCriteria(ref givenName);

            string userAttr = Consts.STANDARD_CISCO_USERNAME;
            if(customUserAttr != null && customUserAttr != String.Empty) 
            {
                userAttr = customUserAttr;
            }

            return String.Format(Consts.SEARCH_FILTER, Consts.OC_PERSON_ATTR,
                userAttr, commonName, surName, givenName);
        }

        /// <summary>
        /// Sanitize an individual search filter attribute by checking
        /// whether it is null or an empty string. If so, replace it
        /// with a '*' indicating that it should match on everything.
        /// </summary>
        /// <param name="filter">The attribute filter string.</param>
        protected static void SanitizeFilterCriteria(ref string filter)
        {
            if((filter == null) || (filter == String.Empty)) 
            {
                filter = "*";
            }
        }

        public bool ValidateInput()
        {
            if (ldapBaseDn == null)
                ldapBaseDn = Consts.DEF_BASE_SEARCH_DIR;

            return true;
        }

        public void Clear()
        {
            customUserAttr = null;
            username    = null;
            pin         = null;
            ldapHost    = null;
            ldapBaseDn  = null;
            ldapPort    = 0;
        }
    }
}
