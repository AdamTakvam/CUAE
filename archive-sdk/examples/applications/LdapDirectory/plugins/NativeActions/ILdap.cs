using System;
using System.Diagnostics;

namespace Metreos.Native.LdapDirectory
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public abstract class ILdap
	{
        // Native Action Package declaration for this suite of actions
        public const string PACKAGE_DECL= "Metreos.Native.LdapDirectory";
        public const string PACKAGE_DESC= "Encapsulates common LDAP interactions."; 

        // Native Types Package declaration for this suite of types
        public const string TYPES_PACKAGE_DECL = "Metreos.Types.LdapDirectory";
        public const string TYPES_PACKAGE_DESC = "Types created to encapsulate LDAP interaction in an efficient manner.";
        
        #region Search Action

        // Search Action
        public const string ACTION_SEARCH = "Search";
        public const string ACTION_SEARCH_DESC = "Performs query against an LDAP server.";
        public const string ACTION_SEARCH_DISPLAY_NAME = "Search";

        public const string PARAM_HOSTNAME = "hostname";
        public const string PARAM_HOST_PORT = "hostPort";
        public const string PARAM_USERNAME = "username";
        public const string PARAM_PASSWORD = "password";
        public const string PARAM_BASE = "searchBase";
        public const string PARAM_FILTER = "searchFilter";
        public const string PARAM_FIRST_NAME = "firstNameAttr";
        public const string PARAM_LAST_NAME = "lastNameAttr";
        public const string PARAM_TELE_ATTR = "telephoneAttr";
        public const string PARAM_UNKNOWN_TELEPHONE_NUM = "unknownNumber";

        public const string PARAM_USERNAME_DESC = "Username to use for binding.";
        public const string PARAM_HOSTNAME_DESC = "The URI indicating the location of the LDAP server.";
        public const string PARAM_HOST_PORT_DESC = "The port of the LDAP server.";
        public const string PARAM_PASSWORD_DESC = "Password to use for binding.";
        public const string PARAM_BASE_DESC = "The base element of the search.";
        public const string PARAM_FILTER_DESC = "The filter to use for this search.";
        public const string PARAM_FIRST_NAME_DESC = "The attribute used to represent first names.";
        public const string PARAM_LAST_NAME_DESC = "The attribute used to represent last names.";
        public const string PARAM_TELE_ATTR_DESC = "The attribute used to represent the telephone number.";
        public const string PARAM_UNKNOWN_TELEPHONE_NUM_DESC = "The telephone number to supply for a person with no defined telephone number."; // Most likely the operator for the company.

        #endregion

        #region ResultSet Type

        public const string TYPE_RESULT_SET = "ResultSet";
        public const string TYPE_RESULT_SET_DESC = "Results of the search";

        #endregion

        public const int DEF_LDAP_PORT = 389;
        public const string OPERATOR = "0";
        public static string CreateMandatoryParamMissingError(string param, string packageName, string actionName)
        {
            return String.Format("Mandatory parameter {0} missing in action {1}.{2}", param, packageName, actionName);
        }

        public static bool IsParamDefined(string param)
        {
             return param != null ? (param != String.Empty ? true : false ) : false;
        }  
	}
}
