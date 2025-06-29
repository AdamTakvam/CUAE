using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Method = Metreos.Utilities.SqlBuilder.Method;
using IntercomGroupsTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.IntercomGroups;

namespace Metreos.ApplicationSuite.Storage
{
    /// <summary>
    ///     Provides data access to the Devices table, and any information which stem from that
    /// </summary>
    public class IntercomGroups : DbTable
    {
        public IntercomGroups(IDbConnection connection, LogWriter log, string applicationName, string partitionName, bool allowWrite)
            : base(connection, log, applicationName, partitionName, allowWrite) {}

        #region GetIntercomGroup
        /// <summary>
        ///     Retrieve the intercom group IDs for a specific user ID.
        /// </summary>
        public bool GetIntercomGroup(
            uint intercomGroupsId,
            out string name, 
            out bool isEnabled, 
            out bool isTalkbackEnabled, 
            out bool isPrivate)
        {
            name = null;
            isEnabled = isTalkbackEnabled = isPrivate = false;

            SqlBuilder builder = new SqlBuilder(Method.SELECT, IntercomGroupsTable.TableName);
            builder.fieldNames.Add(IntercomGroupsTable.Id);
            builder.fieldNames.Add(IntercomGroupsTable.IsEnabled);
            builder.fieldNames.Add(IntercomGroupsTable.IsPrivate);
            builder.fieldNames.Add(IntercomGroupsTable.IsTalkbackEnabled);
            builder.fieldNames.Add(IntercomGroupsTable.Name);
            builder.where[IntercomGroupsTable.Id] = intercomGroupsId;

            AdvancedReadResultContainer result = ExecuteEasyQuery(builder);

            if(result.result == ReadResult.Success)
            {
                DataTable table = result.results;
                
                if ( (table == null) || (table.Rows.Count == 0) )
                    return false;

                if(table.Rows.Count > 1)
                {
                    log.Write(TraceLevel.Warning, "{0}{1}{2}", "Found multiple intercom groups using Id '", intercomGroupsId, "'!" );
                    return false;
                }

                if(table.Rows.Count == 1)
                {
                    DataRow row = table.Rows[0];
                    if (Convert.IsDBNull(row[IntercomGroupsTable.IsEnabled]) == false)
                    {
                        isEnabled = Convert.ToBoolean(row[IntercomGroupsTable.IsEnabled]);
                    }

                    if (Convert.IsDBNull(row[IntercomGroupsTable.IsPrivate]) == false)
                    {
                        isPrivate = Convert.ToBoolean(row[IntercomGroupsTable.IsPrivate]);
                    }

                    if (Convert.IsDBNull(row[IntercomGroupsTable.IsTalkbackEnabled]) == false)
                    {
                        isTalkbackEnabled = Convert.ToBoolean(row[IntercomGroupsTable.IsTalkbackEnabled]);
                    }

                    if(Convert.IsDBNull(row[IntercomGroupsTable.Name]) == false)
                    {
                        name = row[IntercomGroupsTable.Name] as string;
                    }
                }

                return (table.Rows.Count == 0) ? false : true;
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetIntercomGroup method, using intercom groups Id: '{0}'\n" +
                    "Error message: {1}", intercomGroupsId, result.e.Message);
                return false;
            }
        }
        #endregion
    }
}
