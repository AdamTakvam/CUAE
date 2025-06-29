using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Globalization;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Method = Metreos.Utilities.SqlBuilder.Method;
using SessionRecordsTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.SessionRecords;
using CallRecordsTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.CallRecords;

namespace Metreos.ApplicationSuite.Storage
{
	/// <summary>
	///     Provides data access to the SessionRecords table, or any information which stems from SessionRecords table
	/// </summary>
	public class CallRecords : DbTable
	{
        public CallRecords(IDbConnection connection, LogWriter log, string applicationName, string partitionName, bool allowWrite)
            : base(connection, log, applicationName, partitionName, allowWrite) { }

        #region WriteCallRecordStart

        /// <summary>
        ///     Creates a call record with start time and other values
        /// </summary>
        /// <param name="userId">
        ///     UserId of the CallRecord.  Specify 0 or negative for no user 
        /// </param>
        /// <param name="sessionRecordId">
        ///     Associate Session. Specifiy 0 or negative for no session
        /// </param>
        /// <param name="originNumber">
        ///     The originating number.  Can be null
        /// </param>
        /// <param name="destinationNumber">
        ///     The destination number. Can be null
        /// </param>
        /// <param name="callRecordId"> 
        ///     The ID for this new record 
        /// </param>
        /// <param name="authRecordsId">
        ///     The ID for the authentication record associated with this call. 0 for none.
        /// </param>
        /// <returns></returns>
        public bool WriteStart(
            uint userId, 
            uint sessionRecordId, 
            string originNumber, 
            string destinationNumber, 
            string scriptName,
            uint authRecordsId,
            out uint callRecordId)
        {
            bool success = false;
            callRecordId = 0;

            // Format inputs
            object userIdInput              = userId;
            if(userId == 0)   
            {
                userIdInput                 = null;
            }
            object sessionRecordIdInput     = sessionRecordId;
            if(sessionRecordId == 0) 
            {
                sessionRecordIdInput        = null;
            }
            object originNumberInput        = originNumber == null || originNumber.Length == 0 
                ? null 
                : originNumber;
            object destinationNumberInput   = destinationNumber == null || destinationNumber.Length == 0 
                ? null
                : destinationNumber;

            object authRecordsIdInput       = authRecordsId;
            if(authRecordsId == 0)  
            {
                authRecordsIdInput          = null;
            }

            SqlBuilder builder = new SqlBuilder(Method.INSERT, CallRecordsTable.TableName);
            builder.AddFieldValue(CallRecordsTable.UserId, userIdInput);
            builder.AddFieldValue(CallRecordsTable.SessionRecordsId, sessionRecordIdInput);
            builder.AddFieldValue(CallRecordsTable.OriginNumber, originNumberInput);
            builder.AddFieldValue(CallRecordsTable.DestinationNumber, destinationNumberInput);
            builder.AddFieldValue(CallRecordsTable.ScriptName, scriptName);
            builder.AddFieldValue(CallRecordsTable.PartitionName, partitionName);
            builder.AddFieldValue(CallRecordsTable.ApplicationName, applicationName);
            builder.AddFieldValue(CallRecordsTable.AuthRecordsId, authRecordsIdInput);
            builder.AddFieldValue(CallRecordsTable.Start, DateTime.Now);

            WriteResultContainer result = ExecuteCommand(builder);

            if(result.result == WriteResult.Success)
            {
                callRecordId = result.lastInsertId;

                success = callRecordId != 0;
            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the WriteCallRecordStart method\n" +
                    "Error message: {0}", result.e.Message);
                success = false;
            }
            else if(result.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("WriteStart"));
                success = false;
            }
               
            return success;
        }

        #endregion

        #region UpdateCallRecord

        /// <summary>
        /// Updates an existing call record.
        /// </summary>
        /// <param name="callRecordId">id of the call record to update</param>
        /// <param name="updateFields">Hashtable of column name -> new value</param>
        /// <returns>'true' for success, 'false' otherwise</returns>
        public bool UpdateCallRecord(uint callRecordId, Hashtable updateFields)
        {
            bool success = false;
            if (callRecordId < SqlConstants.StandardPrimaryKeySeed || updateFields == null || updateFields.Count == 0)
                return success;

            SqlBuilder builder = new SqlBuilder(Method.UPDATE, CallRecordsTable.TableName);
            foreach (string s in updateFields.Keys)
            {
                builder.AddFieldValue(s, updateFields[s]);
            }
            builder.where[CallRecordsTable.Id] = callRecordId;

            WriteResultContainer result = ExecuteCommand(builder);

            if(result.result == WriteResult.Success)
                success = true;
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Info, 
                    "Error encountered in the UpdateCallRecord method\n" +
                    "Error message: {0}", result.e.Message);
            }
            else if(result.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("UpdateCallRecord"));
            }
               
            return success;
        } // UpdateCallRecord

        #endregion

        #region WriteCallRecordStop

        /// <summary>
        ///     End a call record
        /// </summary>
        /// <param name="callRecordId">
        ///     The id of the call record
        /// </param>
        /// <param name="reason">
        ///     The reason the call ended
        /// </param>
        /// <returns>
        ///     <c>true</c> if the call record could be marked as done, otherwise <c>false</c>
        /// </returns>
        public bool WriteStop(int callRecordId, EndReason reason)
        {
            bool success = false;

            SqlBuilder builder = new SqlBuilder(Method.UPDATE, CallRecordsTable.TableName);
            builder.AddFieldValue(CallRecordsTable.End, DateTime.Now);
            builder.AddFieldValue(CallRecordsTable.EndReason, ((int)reason));
            builder.where[CallRecordsTable.Id] = callRecordId;

            WriteResultContainer result = ExecuteCommand(builder);
 
            if(result.result == WriteResult.Success)
            {
                int resultsAffected = result.rowsAffected;
                success = resultsAffected != 0;
            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the WriteCallRecordStop method\n" +
                    "Error message: {0}", result.e.Message);
                success = false;
            }
            else if(result.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("WriteStop"));
                success = false;
            }

            return success;
        }

        #endregion

        #region AssociateScheduledConferenceId

        /// <summary>
        ///     Associate a Scheduled Conference ID with a call record
        /// </summary>
        /// <param name="callRecordId">
        ///     ID of the call record
        /// </param>
        /// <param name="scheduledRecordId">
        ///     ID of the scheduled conference record
        /// </param>
        /// <returns>
        ///     <c>true</c> if the call record could be associated with the scheduled conference record,
        ///     otherwise <c>false</c>
        /// </returns>
        public bool AssociateScheduledConferenceId(int callRecordId, int scheduledRecordId)
        {
            bool success = false;

            SqlBuilder builder = new SqlBuilder(Method.UPDATE, CallRecordsTable.TableName);
            builder.fieldNames.Add(CallRecordsTable.ScheduledConfId);

            // Format values
            object scheduledConferenceIdInput = scheduledRecordId;
            if(scheduledRecordId == 0) scheduledConferenceIdInput = null;

            builder.fieldValues.Add(scheduledConferenceIdInput);
            builder.where[CallRecordsTable.Id] = callRecordId; 

            WriteResultContainer result = ExecuteCommand(builder);
 
            if(result.result == WriteResult.Success)
            {
                int resultsAffected = result.rowsAffected;
                success = resultsAffected != 0;
            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the AssociateScheduledConferenceId method\n" +
                    "Error message: {0}", result.e.Message);
                success = false;
            }
            else if(result.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("AssociateScheduledConferenceId"));
				success = false;
            }

            return success;
        }

        #endregion

        #region AssociateRecordingsId

        /// <summary>
        ///     Associate a Recordings ID with a call record
        /// </summary>
        /// <param name="callRecordId">
        ///     ID of the call record
        /// </param>
        /// <param name="recordingsId">
        ///     ID of the recordings record
        /// </param>
        /// <returns>
        ///     <c>true</c> if the call record could be associated with the recordings record,
        ///     otherwise <c>false</c>
        /// </returns>
        public bool AssociateRecordingsId(uint callRecordId, uint recordingsId)
        {
            bool success = false;

            // Format values
            object recordingsIdInput = recordingsId;
            if(recordingsId == 0) recordingsIdInput = null;

            SqlBuilder builder = new SqlBuilder(Method.UPDATE, CallRecordsTable.TableName);
            builder.AddFieldValue(CallRecordsTable.RecordingsId, recordingsIdInput);
            builder.where[CallRecordsTable.Id] = callRecordId; 

            WriteResultContainer result = ExecuteCommand(builder);
 
            if(result.result == WriteResult.Success)
            {
                int resultsAffected = result.rowsAffected;
                success = resultsAffected != 0;
            }
            else if(result.result == WriteResult.DbFailure)
            {
                
                log.Write(TraceLevel.Error, 
                    "Error encountered in the AssociateRecordingsId method\n" +
                    "Error message: {0}", result.e.Message);
                success = false;
            }
            else if(result.result == WriteResult.PublisherDown)
            {
			    log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("AssociateRecordingsId"));
                success = false;
            }

            return success;
        }

        #endregion
	}
}
