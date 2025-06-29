using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Method = Metreos.Utilities.SqlBuilder.Method;
using MecConferencesTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.MecConferences;
namespace Metreos.ApplicationSuite.Storage
{
    /// <summary>
    ///     Provides data access to the MecConferences table, and any information which stem from that
    /// </summary>
    public class MecConferences : DbTable
    {
        public enum ResultValues
        {
            Success,
            Failure
        }

        public enum ResultConferenceUpdate
        {
            Success,
            NoRecord,
            Failure
        }

        public MecConferences(IDbConnection connection, LogWriter log, string applicationName, string partitionName, bool allowWrite)
            : base(connection, log, applicationName, partitionName, allowWrite) { }

        #region CreateConferenceRecord
        /// <summary>
        /// Creates a conference record
        /// </summary>
        public ResultValues CreateConferenceRecord(
            string sessionId, 
            out int id)
        {
            id = 0;
            
            SqlBuilder builder = new SqlBuilder(Method.INSERT, MecConferencesTable.TableName);
            builder.fieldNames.Add(MecConferencesTable.ConfSessionId);
            builder.fieldValues.Add(sessionId);
            ResultValues returnValue = ResultValues.Success;

            WriteResultContainer result = ExecuteCommand(builder);

            if(result.result == WriteResult.Success)
            {
                id = (int) result.lastInsertId;

                if(id < 1)
                {
                    log.Write(TraceLevel.Error, "Unable to create conference record using sessionId = '{0}'", sessionId);
                    returnValue = ResultValues.Failure;
                }
                else
                {
                    returnValue = ResultValues.Success;
                }
            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the CreateConferenceRecord method, using sessionId = '{0}'\n" + 
                    "Error message: {1}", sessionId, result.e.Message );
                returnValue = ResultValues.Failure;
            }
            else if(result.result == WriteResult.PublisherDown)
            {
                // no meaning for MEC.  Whatever, I'll just have this case too
                returnValue = ResultValues.Failure;
            }

            return returnValue;
        }
        #endregion

        #region StartConference
        /// <summary>
        /// Sets a conference record to starting conditions
        /// </summary>
        public ResultConferenceUpdate StartConference(
            int conferenceId, 
            string mmsConferenceId)
        {
            SqlBuilder builder = new SqlBuilder(Method.UPDATE, MecConferencesTable.TableName);
            builder.AddFieldValue(MecConferencesTable.Start, new SqlBuilder.PreformattedValue("NOW()"));
            builder.AddFieldValue(MecConferencesTable.MmsConferenceId, mmsConferenceId);

            builder.where[MecConferencesTable.Id] = conferenceId;

            ResultConferenceUpdate returnValue = ResultConferenceUpdate.Success;

            WriteResultContainer result = ExecuteCommand(builder);

            if(result.result == WriteResult.Success)
            {
                if(result.rowsAffected < 1)
                {
                    log.Write(TraceLevel.Error, "Unable to update conference to starting conditions for conferenceId = '{0}'", conferenceId);
                    returnValue = ResultConferenceUpdate.NoRecord;
                }
                else
                {
                    returnValue = ResultConferenceUpdate.Success;
                }
            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the StartConference method, using conferenceId = '{0}'\n" + 
                    "Error message: {1}", conferenceId, result.e.Message );
                returnValue = ResultConferenceUpdate.Failure;
            }
            else if(result.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("StartConference"));
                returnValue = ResultConferenceUpdate.Failure;
            }

            return returnValue;
        }
        #endregion

        #region EndConference
        /// <summary>
        /// Sets a conference record to ending conditions
        /// </summary>
        public ResultConferenceUpdate EndConference(
            int conferenceId)
        {
            SqlBuilder builder = new SqlBuilder(Method.UPDATE, MecConferencesTable.TableName);
            builder.AddFieldValue(MecConferencesTable.End, new SqlBuilder.PreformattedValue("NOW()"));

            builder.where[MecConferencesTable.Id] = conferenceId;

            ResultConferenceUpdate returnValue = ResultConferenceUpdate.Success;

            WriteResultContainer result = ExecuteCommand(builder);
 
            if(result.result == WriteResult.Success)
            {
                if(result.rowsAffected < 1)
                {
                    log.Write(TraceLevel.Error, "Unable to update conference to ending conditions for conferenceId = '{0}'", conferenceId);
                    returnValue = ResultConferenceUpdate.NoRecord;
                }
                else
                {
                    returnValue = ResultConferenceUpdate.Success;
                }
            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the EndConference method, using conferenceId = '{0}'\n" + 
                    "Error message: {1}", conferenceId, result.e.Message );
                returnValue = ResultConferenceUpdate.Failure;

            }
            else if(result.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("EndConference"));
                returnValue = ResultConferenceUpdate.Failure;
            }

            return returnValue;
        }
        #endregion
    }
}