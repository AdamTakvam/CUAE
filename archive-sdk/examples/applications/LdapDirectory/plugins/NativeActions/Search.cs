using System;
using System.Diagnostics;
using System.Collections;

using Novell.Directory.Ldap;
using Novell.Directory.Ldap.Utilclass;
using Metreos.LoggingFramework;
using Metreos.Interfaces;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework;
using Metreos.LdapDirectory.Common;

namespace Metreos.Native.LdapDirectory
{
    /// <summary> Searches against LDAP, returning a list of users and telephone numbers </summary>
    [PackageDecl(ILdap.PACKAGE_DECL, ILdap.PACKAGE_DESC)]
    public class Search : INativeAction
    {
        [ActionParamField(ILdap.PARAM_USERNAME_DESC, false)]
        public  string Username { set { username = value; } }
        private string username;

        [ActionParamField(ILdap.PARAM_PASSWORD_DESC, false)]
        public  string Password { set { password = value; } }
        private string password;

        [ActionParamField(ILdap.PARAM_HOSTNAME_DESC, true)]
        public  string Hostname { set { hostname = value; } }
        private string hostname;

        [ActionParamField(ILdap.PARAM_HOST_PORT_DESC, false)]
        public  ushort Hostport { set { hostport = value; } }
        private ushort hostport;

        [ActionParamField(ILdap.PARAM_BASE_DESC, true)]
        public  string SearchBase { set { searchBase = value; } }
        private string searchBase;

        [ActionParamField(ILdap.PARAM_FILTER_DESC, true)]
        public  string SearchFilter { set { searchFilter = value; } }
        private string searchFilter;

        [ActionParamField(ILdap.PARAM_FIRST_NAME_DESC, true)]
        public  string FirstNameAttr { set { firstNameAttr = value; } }
        private string firstNameAttr;

        [ActionParamField(ILdap.PARAM_LAST_NAME_DESC, true)]
        public  string LastNameAttr { set { lastNameAttr = value; } }
        private string lastNameAttr;

        [ActionParamField(ILdap.PARAM_TELE_ATTR, true)]
        public  string TelephoneAttr { set { telephoneAttr = value; } }
        private string telephoneAttr;

        [ActionParamField(ILdap.PARAM_UNKNOWN_TELEPHONE_NUM, false)]
        public  string UnknownNumber { set { unknownNumber = value; } }
        private string unknownNumber;

        [ResultDataField(ILdap.TYPE_RESULT_SET_DESC)]
        public LdapResultSortedList  ResultData { get { return resultData; } }
        private LdapResultSortedList resultData;

        [Action(ILdap.ACTION_SEARCH, true, ILdap.ACTION_SEARCH_DISPLAY_NAME, ILdap.ACTION_SEARCH_DESC)]
        public string Execute(LogWriter log, SessionData sessionData, IConfigUtility configUtility)
        { 
            // Clamp unknown number if undefined
            unknownNumber = ILdap.IsParamDefined(unknownNumber) ? unknownNumber : ILdap.OPERATOR;
  
            // The decision to bind is based on the presence of a username.
            bool attemptBind = ILdap.IsParamDefined(username);

            LdapConnection connection = null;
            LdapResultSortedList sortedResultSet = new LdapResultSortedList();

            // Binding not working on Windows using Novell.Directory.Ldap.dll
            if(! PrepareConnection(hostname, hostport, username, password, false, out connection, log) )
            {
                resultData = sortedResultSet;
                return IApp.VALUE_FAILURE;
            } 

            bool queryResult = PerformQuery(connection, 
                searchBase, searchFilter, firstNameAttr, lastNameAttr,
                telephoneAttr, unknownNumber, out sortedResultSet, log);

            if(queryResult)
            {
                resultData = sortedResultSet;
                return IApp.VALUE_SUCCESS;
  
            }
            else
            {
                resultData = sortedResultSet;
                return IApp.VALUE_FAILURE;
            }
        }
        
        public void Clear()
        {
            username = null;
            password = null;
            hostname = null;
            searchBase = null;
            searchFilter = null;
            firstNameAttr = null;
            lastNameAttr = null;
            telephoneAttr = null;
            resultData = null;
        }

        public bool ValidateInput()
        {
            return true;
        }

        /// <summary>Creates a connection to an LDAP server, optionally binding if needed.</summary>
        /// <param name="username">Username in LDAP format. Ex: 'cn=Directory Manager dc=metreos,dc=com'</param>
        /// <param name="connection"></param>
        /// <returns>Success on connect</returns>
        private bool PrepareConnection(string hostname, int port,
            string username, string password, bool attemptBind, 
            out LdapConnection connection, LogWriter log)
        {
            // Create connection
            connection = new LdapConnection();

            if(! Connect(connection, hostname, port) )
            {
                log.Write(TraceLevel.Error, "Unable to connect to the LDAP server.");
                return false;
            }

            if(attemptBind)
            {
                if(! Bind(connection, username, password) )
                {
                    log.Write(TraceLevel.Error, "Unable to bind to the connection.");
                    return false;
                }
            }

            return true;
        }

        private bool Connect(LdapConnection connection, string hostname, int port)
        {
            try
            {
                connection.Connect(hostname, port);
            
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool Bind(LdapConnection connection, string username, string password)
        {
            try
            {
                connection.Bind(3, username, password);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Disconnect();
                connection = null;
            }
        }
        
        // Performs a query, populating the sortedlist with attributes and telephone numbers
        private bool PerformQuery(LdapConnection connection,
            string searchBase, string searchFilter, string firstNameAttr, string lastNameAttr,
            string teleAttr, string unknownNumber, out LdapResultSortedList sortedResultSet, 
            LogWriter log)
        {
            LdapCompareAttributeCollection nameSearch = new LdapCompareAttributeCollection();
            nameSearch.Add(new LdapCompareAttribute(lastNameAttr));
            nameSearch.Add(new LdapCompareAttribute(firstNameAttr));

            LdapResultComparer comparer = new LdapResultComparer(nameSearch);

            sortedResultSet = new LdapResultSortedList(comparer, 32, firstNameAttr, lastNameAttr);

            try
            {
                LdapSearchResults results = connection.Search(searchBase,
                    LdapConnection.SCOPE_SUB, searchFilter, null, false);
                                            
                while(results.hasMore())
                {
                    LdapEntry entry;

                    try
                    {
                        entry = results.next();

                        LdapAttributeSet allAttributes = entry.getAttributeSet();

                        // The comparing mechanism is currently using a metreos defined class;
                        // this could all be done with Novell classes, but at the cost of
                        // completely reimplementing how serialization occurs of the SortedList, which
                        // is a non-issue in 0.6 anyway.
                        LdapAttributeCollection allNativeImplAttributes = new LdapAttributeCollection();

                        foreach(Novell.Directory.Ldap.LdapAttribute attribute in allAttributes)
                        {
                            Metreos.LdapDirectory.Common.LdapAttribute nativeImplAttribute
                                = new Metreos.LdapDirectory.Common.LdapAttribute
                                (attribute.Name, attribute.StringValueArray);
                            
                            allNativeImplAttributes.Add(nativeImplAttribute);
                        }

                        string telephoneNumber = unknownNumber;

                        // If the number can be found in the result, use that.
                        // If not, use the default unknown number.
                        string tele = allNativeImplAttributes[teleAttr].Value;
                        if(tele != NullLdapAttribute.UndefinedValue)
                        {
                            telephoneNumber = tele;
                        }

                        // This result should have at least one of the two names that will be pushed
                        // to the phone (or even if they were there, then they should at least be valued)
                        if(allNativeImplAttributes[lastNameAttr].Value == NullLdapAttribute.UndefinedValue
                            && allNativeImplAttributes[firstNameAttr].Value == NullLdapAttribute.UndefinedValue)
                        {
                            // Discard entry
                            continue;
                        }
                        else
                        {
                            // Store the telephone number for this person
                            sortedResultSet.Add(allNativeImplAttributes, telephoneNumber);
                        }
                    }
                    catch(Exception e)
                    {
                        if(e is LdapException)
                        {
                            bool fatalServerError = true;

                            LdapException ldapException = e as LdapException;

                            switch(ldapException.ResultCode)
                            {
                                case LdapException.FILTER_ERROR:

                                    log.Write(TraceLevel.Error, "The filter used in the query is not valid.");
                                    break;

                                case LdapException.SIZE_LIMIT_EXCEEDED:
                                    
                                    // TODO: should perhaps send signal to admin that this app
                                    // can not function ideally due to size limit of LDAP response 
                                    // occurring, indicating he should 
                                    // A.  Increase response size on LDAP server.
                                    //
                                    // ??? B.  Get an Application Developre to create a solution based on his server 
                                    //     type, and what it can support. (there isn't a 
                                    //     one-size-fits-all paging system for LDAP and it's flavors,
                                    //     it seems -- MSC).
                                    // 
                                    // Maybe make an indication at the bottom of the listing on the phone 
                                    // essentially showing a "Rest of list unshowable..."
                                    // Better the user be aggravated, but realize the problem is well defined
                                    // rather then the user just be aggravated and shaking their heads?
                                    log.Write(TraceLevel.Error, 
                                        "The number of entries to return exceeded the server limit.");

                                    fatalServerError = false;
                                    break;

                                default:
                                    break;
                            }

                            if(fatalServerError)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            log.Write(TraceLevel.Error, "A non-LDAP Exception was generated while parsing results of" +
                                " the search.  The exception was: " + System.Environment.NewLine + e.Message);
                            return false;
                        }
                    }
                }
            }
            catch(LdapException e)
            {
                // TODO: investigate possible exceptions this can generate, to see if something more helpful 
                // can be logged.
                log.Write(TraceLevel.Error, "A LDAP exception was generated in performing a search.  The exception was: "
                    + System.Environment.NewLine + e.Message);

                return false;
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "A non-LDAP exception was generated in performing a search.  The exception was: "
                    + System.Environment.NewLine + e.Message);
                
                return false;
            }
            finally
            {
                connection.Disconnect();
            }

            return true;
        }
    }
}
