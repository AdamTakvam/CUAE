using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Method = Metreos.Utilities.SqlBuilder.Method;
using MecParticipantsTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.MecParticipants;
namespace Metreos.ApplicationSuite.Storage
{
    /// <summary>
    ///     Provides data access to the MceParticipants table, and any information which stem from that
    /// </summary>
    public class MecParticipants : DbTable
    {
        public enum ResultValuesParticipant
        {
            Success,
            NoRecord,
            Failure
        }

        public enum ResultValuesAddParticipant
        {
            Success,
            Failure
        }

        public MecParticipants(IDbConnection connection, LogWriter log, string applicationName, string partitionName, bool allowWrite)
            : base(connection, log, applicationName, partitionName, allowWrite) { }

        #region GetParticipant
        /// <summary>
        /// Returns all data for a given participant record
        /// </summary>
        public ResultValuesParticipant GetParticipant(
            int id, 
            out int conferenceId, 
            out string mmsConnectionId, 
            out string callId,
            out bool isHost,
            out int status,
            out DateTime firstConnected,
            out DateTime lastConnected,
            out DateTime disconnected)
        {
            conferenceId = 0;
            mmsConnectionId = null;
            callId = null;
            isHost = false;
            status = 0;
            firstConnected = DateTime.MinValue;
            lastConnected = DateTime.MinValue;
            disconnected = DateTime.MinValue;

            SqlBuilder builder = new SqlBuilder(Method.SELECT, MecParticipantsTable.TableName);
            builder.fieldNames.Add(MecParticipantsTable.ConferenceId);
            builder.fieldNames.Add(MecParticipantsTable.MmsConnectionId);
            builder.fieldNames.Add(MecParticipantsTable.CallId);
            builder.fieldNames.Add(MecParticipantsTable.IsHost);
            builder.fieldNames.Add(MecParticipantsTable.Status);
            builder.fieldNames.Add(MecParticipantsTable.FirstConnected);
            builder.fieldNames.Add(MecParticipantsTable.LastConnected);
            builder.fieldNames.Add(MecParticipantsTable.Disconnected);

            builder.where[MecParticipantsTable.Id] = id;

            ResultValuesParticipant returnValue;

            AdvancedReadResultContainer result = ExecuteEasyQuery(builder);

            if(result.result == ReadResult.Success)
            {
                DataTable results = result.results;
            
                if(results == null || results.Rows.Count < 1)
                {
                    log.Write(TraceLevel.Error, "No record was found for given participant of id '{0}'.\n",id);
                    returnValue = ResultValuesParticipant.NoRecord;
                }
                else
                {
                    DataRow firstResult = results.Rows[0];
                    conferenceId = Convert.ToInt32(firstResult[MecParticipantsTable.ConferenceId]);
                    mmsConnectionId = Convert.ToString(firstResult[MecParticipantsTable.MmsConnectionId]);
                    callId = Convert.ToString(firstResult[MecParticipantsTable.CallId]);
                    isHost = Convert.ToBoolean(firstResult[MecParticipantsTable.IsHost]);
                    status = Convert.ToInt32(firstResult[MecParticipantsTable.Status]);
                    
                    object obj = firstResult[MecParticipantsTable.FirstConnected];
                    if(obj == DBNull.Value) { firstConnected = DateTime.MinValue; }
                    else                    { firstConnected = Convert.ToDateTime(obj); }

                    obj = firstResult[MecParticipantsTable.LastConnected];
                    if(obj == DBNull.Value) { lastConnected = DateTime.MinValue; }
                    else                    { lastConnected = Convert.ToDateTime(obj); }

                    obj = firstResult[MecParticipantsTable.Disconnected];
                    if(obj == DBNull.Value) { disconnected = DateTime.MinValue; }
                    else                    { disconnected = Convert.ToDateTime(obj); }
                    returnValue = ResultValuesParticipant.Success;
                }
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetParticipantInfo method, using Id '{0}'\n" + 
                    "Error message: {1}", id, result.e.Message );
                returnValue = ResultValuesParticipant.Failure;
            }

            return returnValue;
        }
        #endregion

        #region SetParticipantStatus
        /// <summary>
        /// Sets status for a given participant record
        /// </summary>
        public ResultValuesParticipant SetParticipantStatus(
            int id, 
            MecParticipantStatus status)
        {
            SqlBuilder builder = new SqlBuilder(Method.UPDATE, MecParticipantsTable.TableName);
            builder.AddFieldValue(MecParticipantsTable.Status, (int)status);

            // Keep up with timestamps for certain status codes
            if(status == MecParticipantStatus.Online)
            {
                // User is coming online for first time, so also set first_connected and last_connected, 
                builder.AddFieldValue(MecParticipantsTable.FirstConnected, new SqlBuilder.PreformattedValue("NOW()"));
                builder.AddFieldValue(MecParticipantsTable.LastConnected, new SqlBuilder.PreformattedValue("NOW()"));
            }
            else if(status == MecParticipantStatus.Disconnected)
            {
                // User is leaving for good, set disconnected time
                builder.AddFieldValue(MecParticipantsTable.Disconnected, new SqlBuilder.PreformattedValue("NOW()"));
            }
            
            builder.where[MecParticipantsTable.Id] = id;

            ResultValuesParticipant returnValue = ResultValuesParticipant.Failure; 

            WriteResultContainer result = ExecuteCommand(builder);
 
            if(result.result == WriteResult.Success)
            {
                if(result.rowsAffected > 0)
                {
                    returnValue = ResultValuesParticipant.Success; 
                }
                else
                {
                    returnValue = ResultValuesParticipant.NoRecord;
                }
            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the SetParticipantStatus method, using Id '{0}'\n" + 
                    "Error message: {1}", id, result.e.Message );
                returnValue = ResultValuesParticipant.Failure;
            }
            else if(result.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("SetParticipantStatus")); 
                returnValue = ResultValuesParticipant.Failure;
            }

            return returnValue;
        }
        #endregion

        #region AddParticipant

        public ResultValuesAddParticipant AddParticipant(
            int conferenceId, 
            string mmsConnectionId, 
            string callId, 
            string phoneNumber, 
            string description,
            bool isHost,
            out int participantId)
        {
            participantId = 0;

            SqlBuilder builder = new SqlBuilder(Method.INSERT, MecParticipantsTable.TableName);
            builder.fieldNames.Add(MecParticipantsTable.ConferenceId);
            builder.fieldNames.Add(MecParticipantsTable.MmsConnectionId);
            builder.fieldNames.Add(MecParticipantsTable.CallId);
            builder.fieldNames.Add(MecParticipantsTable.IsHost);
            builder.fieldNames.Add(MecParticipantsTable.Status);
            builder.fieldNames.Add(MecParticipantsTable.PhoneNumber);
            builder.fieldNames.Add(MecParticipantsTable.Description);

            builder.fieldValues.Add(conferenceId);
            builder.fieldValues.Add(mmsConnectionId);
            builder.fieldValues.Add(callId);
            builder.fieldValues.Add(isHost);
            builder.fieldValues.Add((int)MecParticipantStatus.Connecting);
            builder.fieldValues.Add(phoneNumber);
            builder.fieldValues.Add(description);
           
            ResultValuesAddParticipant returnValue = ResultValuesAddParticipant.Failure;

            ReadResultContainer result = ExecuteScalar(builder);

            if(result.result == ReadResult.Success)
            {
                participantId = Convert.ToInt32(result.scalar);

                if(participantId < 1)
                {
                    log.Write(TraceLevel.Error, "Unable to create participant record");
                    returnValue = ResultValuesAddParticipant.Failure;
                }
                else
                {
                    returnValue = ResultValuesAddParticipant.Success;
                }
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the AddParticipant method\n"+ 
                    "Error message: {0}", result.e.Message );
                returnValue = ResultValuesAddParticipant.Failure;
            }

            return returnValue;
        }


        #endregion
    }
}
