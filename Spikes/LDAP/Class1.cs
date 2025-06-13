using System;
using System.Collections;
using System.DirectoryServices;
using Metreos.LdapNet;

namespace LDAP
{
    class Class1
    {
        [STAThread]
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("Usage: LDAP.exe <username>");
                return;
            }
            
            Console.WriteLine("Searching for devices associated with: {0}", args[0]);

            string profile = GetUserProfileString(args[0]);

            if(profile == null)
            {
                Console.WriteLine("No user with username: {0}", args[0]);
                return;
            }

            Console.WriteLine("Found: {0}", profile);

            string[] devices = GetUserDevices(profile);

            if(devices != null)
            {
                foreach(string s in devices)
                {
                    Console.WriteLine("Device: {0}", s);
                }
            }
            else
            {
                Console.WriteLine("User has no controlled devices");
            }
        }

        static string[] GetAttributeValues(string searchBase, string attribute)
        {
            string[] retValues = null;
            string[] attributesToReturn = new string[] { attribute };

            LdapClient c = new LdapClient("192.168.1.250", 8404, true, false);
            c.ldap_simple_bind_s("cn=Directory Manager,o=cisco.com", "metreos");

            LdapResult res;

            try
            {
                int count = c.ldap_search_ext_s(searchBase, 
                    Metreos.LdapNet.Misc.LDAPSearchScope.LDAPSCOPE_BASE,
                    "objectClass=*",        /* search filter */
                    attributesToReturn,     /* attribs to return, empty for all */
                    false,                  /* return attrsonly? */
                    60,                     /* allow 60 secs for the search */
                    0,                      /* 0 == no size limit on returned entries */
                    out res);

                foreach(LdapEntry entry in res)
                {
                    foreach(LdapAttribute attr in entry)
                    {
                        if(attr.Name == attribute)
                        {
                            retValues = attr.StringValues;
                            break;
                        }
                    }
                }
            }
            catch(Metreos.LdapNet.Exceptions.LDAPException)
            {
                retValues = null;
            }
            finally
            {
                c.ldap_unbind();
                c.Dispose();
            }

            return retValues;
        }

        static string GetUserProfileString(string username)
        {
            string searchBase = String.Format("cn={0}, ou=Users, o=cisco.com", username);
            string[] profile = null;

            profile = GetAttributeValues(searchBase, "ciscoatUserProfileString");

            if(profile == null) { return null; }

            return profile[0];
        }
        
        static string[] GetUserDevices(string profile)
        {
            string[] devices = null;
            string[] ccnProfile = null;

            ccnProfile = GetAttributeValues(profile, "ciscoatAppProfile");

            if(ccnProfile == null) { return null; }

            devices = GetAttributeValues(ccnProfile[0], "ciscoCCNatControlDevices");

            if(devices == null) { return null; }

            return devices;
        }
    }
}
