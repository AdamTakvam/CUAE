using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Globalization;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Method = Metreos.Utilities.SqlBuilder.Method;
using LdapServersTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.LdapServers;

namespace Metreos.ApplicationSuite.Storage
{
    /// <summary>
    ///     Provides data access to the ldap server table
    /// </summary>
    public class LdapServers : DbTable
    {
        public LdapServers(DbTable host)
            : base(host) { }

        public LdapServers(IDbConnection connection, LogWriter log, string applicationName, string partitionName, bool allowWrite)
            : base(connection, log, applicationName, partitionName, allowWrite) { }

        #region GetLdapServer

        /// <summary>
        ///     Gets all information about an Ldap Server
        /// </summary>
        /// <param name="mediaType"> The ldap server key</param>
        /// <returns> <c>true</c> if the LDAP server could be found, otherwise <c>false</c> </returns>
        public bool GetLdapServer(uint id, out string hostname, out ushort port, out bool secureConnect, out string baseDn, out string userDn, out string password)
        {
            hostname = null;
            port = 0;
            secureConnect = false;
            baseDn = null;
            userDn = null;
            password = null;
            bool success = false;

            if(id == 0) return false;

            SqlBuilder builder = new SqlBuilder(Method.SELECT, LdapServersTable.TableName);
            builder.fieldNames.Add(LdapServersTable.Hostname);
            builder.fieldNames.Add(LdapServersTable.Port);
            builder.fieldNames.Add(LdapServersTable.SecureConnect);
            builder.fieldNames.Add(LdapServersTable.BaseDn);
            builder.fieldNames.Add(LdapServersTable.UserDn);
            builder.fieldNames.Add(LdapServersTable.Password);
            builder.where[LdapServersTable.Id] = id;

            ReadResultContainer result = ExecuteQuery(builder);

            if(result.result == ReadResult.Success)
            {
                using(IDataReader reader = result.reader)
                {
                    if(reader.Read())
                    {
                        hostname = reader[LdapServersTable.Hostname] as string;
                        port = Convert.ToUInt16(reader[LdapServersTable.Port]);
                        secureConnect = Convert.ToBoolean(reader[LdapServersTable.SecureConnect]);
                        object baseDnEntry = reader[LdapServersTable.BaseDn];
                        baseDn = Convert.IsDBNull(baseDnEntry) ? null : baseDnEntry as string;
                        object userDnEntry = reader[LdapServersTable.UserDn];
                        userDn = Convert.IsDBNull(userDnEntry) ? null : userDnEntry as string;
                        object passwordEntry = reader[LdapServersTable.Password];
                        password = Convert.IsDBNull(passwordEntry) ? null : passwordEntry as string;
 
                        if(!reader.Read())
                        {
                            success = true;
                        }
                        else
                        {
                            log.Write(TraceLevel.Error, "Database integrity compromised.  Multiple Ldap Servers of the same ID found.");
                        }
                    }
                    else
                    {
                        log.Write(TraceLevel.Error, "Unable to find LdapServer of id {0}", id);
                    }
                }
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetLdapServer method\n" +
                    "Error message: {0}", result.e.Message);
                success = false;
            }

            return success;
        }

        #endregion

    }
}
