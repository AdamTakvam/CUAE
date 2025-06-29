using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Globalization;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Method = Metreos.Utilities.SqlBuilder.Method;
using SessionRecordsTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.SessionRecords;
using UsersTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.Users;
using AuthenticationTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.AuthenticationRecords;

namespace Metreos.ApplicationSuite.Storage
{
	/// <summary>
	///     Provides data access to the SessionRecords table, or any information which stems from SessionRecords table
	/// </summary>
	public class SessionRecords : DbTable
	{
        public SessionRecords(IDbConnection connection, LogWriter log, string applicationName, string partitionName, bool allowWrite)
            : base(connection, log, applicationName, partitionName, allowWrite) { }

        #region ReturnValues
        public enum WriteSessionStartResultValues
        {
            success,
            TooManyConcurrentLogins,
            failure
        }
        #endregion
        
        #region WriteCallSessionStart

        /// <summary>
        ///     Creates a start session record
        /// </summary>
        /// <param name="authRecordId">
        ///     The authentication record id to associate with this session.
        ///     Pass 0 for no authentication record.
        /// </param>
        /// <param name="sessionId"> 
        ///     The id of the session record just created.  -1 if failure to create record
        /// </param>
        /// <returns> 
        ///     <c>true</c> if the record was able to be created, otherwise <c>false</c>
        /// </returns>
        public WriteSessionStartResultValues WriteCallSessionStart(uint authRecordId, out uint sessionId)
        {
            bool success = true;
            uint usersId;
            int numConcurrentSessions;
            int maxConcurrentSessions;
            sessionId = 0;

            if(authRecordId == 0)   return WriteSessionStartResultValues.failure;

            // Check first that this user has not exceeded maximum sessions
            SqlBuilder innerAuthTable = new SqlBuilder(Method.SELECT, AuthenticationTable.TableName);
            innerAuthTable.fieldNames.Add(AuthenticationTable.UserId);
            innerAuthTable.where[AuthenticationTable.Id] = authRecordId;
            
            SqlBuilder outerUserTable = new SqlBuilder(Method.SELECT, UsersTable.TableName);
            outerUserTable.fieldNames.Add(UsersTable.Id);
            outerUserTable.fieldNames.Add(UsersTable.MaxConcurrentSessions);
            outerUserTable.fieldNames.Add(UsersTable.NumConcurrentSessions);
            outerUserTable.where[UsersTable.Id] = innerAuthTable;

            AdvancedReadResultContainer result = ExecuteEasyQuery(outerUserTable);
            
            if(result.result == ReadResult.Success)
            {
                DataTable results = result.results;

                if(results != null && results.Rows.Count == 1)
                {
                    DataRow row = results.Rows[0];

                    usersId                 = Convert.ToUInt32(row[UsersTable.Id]);
                    maxConcurrentSessions   = Convert.ToInt32(row[UsersTable.MaxConcurrentSessions]);
                    numConcurrentSessions   = Convert.ToInt32(row[UsersTable.NumConcurrentSessions]);

                    // maxconcurrentsessions == 0 indicates that there is no limit
                    if(maxConcurrentSessions != 0 && numConcurrentSessions >= maxConcurrentSessions)
                    {
                        return WriteSessionStartResultValues.TooManyConcurrentLogins;
                    }
                }
                else if(results != null && results.Rows.Count > 1)
                {
                    log.Write(TraceLevel.Error, 
                        "Duplication encountered when attempting to WriteCallSessionStart " +
                        "with an authRecordsId of '{0}'", authRecordId);
                    return WriteSessionStartResultValues.failure;
                }
                else
                {
                    log.Write(TraceLevel.Error, 
                        "No user found in authentication records in WriteCallSessionStart " +
                        "with an authRecordsId of '{0}'", authRecordId);
                    return WriteSessionStartResultValues.failure;
                }
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the WriteCallSessionStart method\n" +
                    "Error message: {0}", result.e.Message);

                return WriteSessionStartResultValues.failure;
            }                
        
            // Increment number of current sessions
            SqlBuilder incrementSessionsBuilder = new SqlBuilder(Method.UPDATE, UsersTable.TableName);
            incrementSessionsBuilder.AddFieldValue(UsersTable.NumConcurrentSessions, numConcurrentSessions + 1);
            incrementSessionsBuilder.where[UsersTable.Id] = usersId;

            WriteResultContainer writeResult = ExecuteCommand(incrementSessionsBuilder);
            
            if(writeResult.result == WriteResult.Success)
            {
                // Do nothing.  
            }
            else if(writeResult.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the WriteCallSessionStart method\n" +
                    "Error message: {0}", result.e.Message);

                return WriteSessionStartResultValues.failure;
            }
            else if(writeResult.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("WriteCallSessionStart"));

                return WriteSessionStartResultValues.failure;
            }


            SqlBuilder builder = new SqlBuilder(Method.INSERT, SessionRecordsTable.TableName);

            // Format authRecordId;
            object authRecordIdInput = authRecordId == 0 ? Convert.DBNull : authRecordId;

            builder.AddFieldValue(SessionRecordsTable.Start, DateTime.Now);
            builder.AddFieldValue(SessionRecordsTable.AuthenticationRecordId, authRecordIdInput);

            writeResult = ExecuteCommand(builder);

            if(writeResult.result == WriteResult.Success)
            {
                sessionId = writeResult.lastInsertId;
                success = sessionId != 0;
            }
            else if(writeResult.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the WriteCallSessionStart method\n" +
                    "Error message: {0}", result.e.Message);

                success = false;
            
                SqlBuilder decrementSessionsBuilder = new SqlBuilder(Method.UPDATE, UsersTable.TableName);
                decrementSessionsBuilder.AddFieldValue(UsersTable.NumConcurrentSessions, numConcurrentSessions);
                decrementSessionsBuilder.where[UsersTable.Id] = usersId;
            
                writeResult = ExecuteCommand(decrementSessionsBuilder);

                if(writeResult.result == WriteResult.Success)
                {

                }
                else if(writeResult.result == WriteResult.DbFailure)
                {
                    log.Write(TraceLevel.Error, 
                        "Error encountered in the WriteCallSessionStart method\n" +
                        "Error message: {0}", writeResult.e.Message);

                    return WriteSessionStartResultValues.failure;
                }
            }
            else if(writeResult.result == WriteResult.PublisherDown)
            {
                // Not possible to reach this point.  Just being explicit.
            }

            if(success) return WriteSessionStartResultValues.success;
            else        return WriteSessionStartResultValues.failure;
        }

        #endregion

        #region WriteCallSessionStop

        /// <summary>
        ///     Creates a stop session record
        /// </summary>
        /// <param name="sessionId">
        ///     The id of the session record.
        /// </param>
        /// <returns> 
        ///     <c>true</c> if the record was able to be updated, otherwise <c>false</c>
        /// </returns>
        public bool WriteCallSessionStop(uint sessionId)
        {
            if(sessionId == 0)   return false;

            bool success = true;

            String decrementCommand = String.Format("IF({0} > 0, {0} - 1, 0)", UsersTable.NumConcurrentSessions);

            // First decrement numConcurrentLogins
            SqlBuilder innerInnerSessionTable = new SqlBuilder(Method.SELECT, SessionRecordsTable.TableName);
            innerInnerSessionTable.fieldNames.Add(SessionRecordsTable.AuthenticationRecordId);
            innerInnerSessionTable.where[SessionRecordsTable.Id] = sessionId;

            SqlBuilder innerAuthTable = new SqlBuilder(Method.SELECT, AuthenticationTable.TableName);
            innerAuthTable.fieldNames.Add(AuthenticationTable.UserId);
            innerAuthTable.where[AuthenticationTable.Id] = innerInnerSessionTable;
            
            SqlBuilder outerUserTable = new SqlBuilder(Method.UPDATE, UsersTable.TableName);
            outerUserTable.AddFieldValue(UsersTable.NumConcurrentSessions, 
                new SqlBuilder.PreformattedValue(decrementCommand));
            outerUserTable.where[UsersTable.Id] = innerAuthTable;

            WriteResultContainer result = ExecuteCommand(outerUserTable);
 
            if(result.result == WriteResult.Success)
            {
                int numAffected = result.rowsAffected;

                if(numAffected > 1)
                {
                    log.Write(TraceLevel.Error, 
                        "Duplication encountered when attempting to WriteCallSessionStop " +
                        "with a sessionId of '{0}'", sessionId);

                    success &= false;
                }
                else if(numAffected == 0)
                {
                    log.Write(TraceLevel.Error, 
                        "No user found in authentication records in WriteCallSessionStop " +
                        "with an sessionId of '{0}'", sessionId);

                    success &= false;
                }
            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the WriteCallSessionStop method\n" +
                    "Error message: {0}", result.e.Message);
                success &= false;
            }
            else if(result.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("WriteCallSessionStop"));
                success &= false;
            }

            SqlBuilder builder = new SqlBuilder(Method.UPDATE, SessionRecordsTable.TableName);
            builder.AddFieldValue(SessionRecordsTable.End, DateTime.Now);
            builder.where[SessionRecordsTable.Id] = sessionId;

            result = ExecuteCommand(builder);
 
            if(result.result == WriteResult.Success)
            {
                int affected = result.rowsAffected;

                // At least one row must be affected by this
                // for this to have been considered a success
                success = affected > 0;
            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the WriteCallSessionStop method\n" +
                    "Error message: {0}", result.e.Message);
                success = false;
            }
            else if(result.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("WriteCallSessionStop"));
                success = false;
            }

            return success;
        }

        #endregion
	}
}
