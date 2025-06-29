using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Method = Metreos.Utilities.SqlBuilder.Method;
using IntercomTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.IntercomGroupMembers;
using UsersTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.Users;

namespace Metreos.ApplicationSuite.Storage
{
    /// <summary>
    ///     Provides data access to the Devices table, and any information which stem from that
    /// </summary>
    public class IntercomMembers : DbTable
    {
        public IntercomMembers(IDbConnection connection, LogWriter log, string applicationName, string partitionName, bool allowWrite)
            : base(connection, log, applicationName, partitionName, allowWrite) {}

        #region GetIntercomGroupsForUser
        /// <summary>
        ///     Retrieve the intercom group IDs for a specific user ID.
        /// </summary>
        public string[] GetIntercomGroupsForUser(uint userId)
        {
            SqlBuilder builder = new SqlBuilder(Method.SELECT, IntercomTable.TableName);
            builder.fieldNames.Add(IntercomTable.IntercomGroupId);
            builder.where[IntercomTable.UserId] = userId;

            ArrayList intercomGroupIds = new ArrayList();

            AdvancedReadResultContainer result = ExecuteEasyQuery(builder);

            if(result.result == ReadResult.Success)
            {
                DataTable table = result.results;
            
                if ( (table == null) || (table.Rows.Count == 0) )
                    return null;

                foreach (DataRow row in table.Rows)
                {
                    if (Convert.IsDBNull(row[IntercomTable.IntercomGroupId]) == false)
                    {
                        uint intercomGroupId = (uint)row[IntercomTable.IntercomGroupId];
                        if (intercomGroupId != 0)
                            intercomGroupIds.Add(intercomGroupId.ToString());
                    }
                }

                if(intercomGroupIds.Count > 0) return (string[]) intercomGroupIds.ToArray(typeof(string));
                    else                           return null;
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetIntercomGroupsForUser method, using user Id: '{0}'\n" +
                    "Error message: {1}", userId, result.e.Message);
                return null;
            }
        }
        #endregion

        #region GetUsersBelongingToIntercomGroup
        /// <summary>
        ///     Retrieve the users that belong to a specific intercom group.
        /// </summary>
        public string[] GetUsersBelongingToIntercomGroup(uint intercomGroupId, bool activeUsersOnly)
        {
            string sqlString = null;

            // if we're only interested in users with account status of Active, use this special JOIN query
            if (activeUsersOnly)
            {
                string sqlFormatString = "SELECT a.{0} from {1} a INNER JOIN {2} b ON a.{0}=b.{0} WHERE ((b.{3}={4}) AND (a.{5}={6}));";
                sqlString = string.Format(sqlFormatString, new object[] {IntercomTable.UserId, IntercomTable.TableName, UsersTable.TableName,
                                                                            UsersTable.Status, (int) UserStatus.Ok, IntercomTable.IntercomGroupId,
                                                                            intercomGroupId});
            }
            else
            {
                SqlBuilder builder = new SqlBuilder(Method.SELECT, IntercomTable.TableName);
                builder.fieldNames.Add(IntercomTable.UserId);
                builder.where[IntercomTable.IntercomGroupId] = intercomGroupId;
            }

            ArrayList userIds = new ArrayList();

            AdvancedReadResultContainer result = ExecuteEasyQuery(sqlString);

            if(result.result == ReadResult.Success)
            {
                DataTable table = result.results;

                if ( (table == null) || (table.Rows.Count == 0) )
                    return null;

                foreach (DataRow row in table.Rows)
                {
                    if (Convert.IsDBNull(row[IntercomTable.UserId]) == false)
                    {
                        uint userId = Convert.ToUInt32(row[IntercomTable.UserId]);
                        if (userId != 0)
                            userIds.Add(userId.ToString());
                    }
                }
                
                if(userIds.Count > 0) return (string[]) userIds.ToArray(typeof(string));
                else                  return null;
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetUsersBelongingToIntercomGroup method, using intercom group Id: '{0}'\n" +
                    "Error message: {1}", intercomGroupId, result.e.Message);
                return null;
            }
        }
        #endregion
    }
}
