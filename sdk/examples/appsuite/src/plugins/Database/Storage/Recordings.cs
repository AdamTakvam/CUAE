using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Globalization;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Method = Metreos.Utilities.SqlBuilder.Method;
using RecordingsTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.Recordings;

namespace Metreos.ApplicationSuite.Storage
{
    /// <summary>
    ///     Provides data access to the recordings table
    /// </summary>
    public class Recordings : DbTable
    {
        public Recordings(IDbConnection connection, LogWriter log, string applicationName, string partitionName, bool allowWrite)
            : base(connection, log, applicationName, partitionName, allowWrite) { }

        #region WriteRecordingStart

        /// <summary>
        ///     Creates a recording record, saving filetype, saving a start time,
        ///     and returning a key to the record
        /// </summary>
        /// <param name="mediaType"> The type of media file pointed to by this record </param>
        /// <param name="recordingId"> Key to the record </param>
        /// <returns> <c>true</c> if the record could be created, otherwise <c>false</c> </returns>
        public bool WriteRecordingStart(uint callRecordsId, uint usersId, MediaFileType mediaType, out uint recordingId)
        {
            recordingId = 0;
            int mediaTypeDb = (int) mediaType;
            bool success = false;

            if(callRecordsId == 0)  return false;
            if(usersId  == 0)       return false;

            SqlBuilder builder = new SqlBuilder(Method.INSERT, RecordingsTable.TableName);
            builder.AddFieldValue(RecordingsTable.CallRecordId, callRecordsId);
            builder.AddFieldValue(RecordingsTable.UserId, usersId);
            builder.AddFieldValue(RecordingsTable.Start, DateTime.Now);
            builder.AddFieldValue(RecordingsTable.Type, mediaTypeDb);

            WriteResultContainer result = ExecuteCommand(builder);

            if(result.result == WriteResult.Success)
            {
                recordingId = result.lastInsertId;
                success = true;
            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the WriteRecordingStart method\n" +
                    "Error message: {0}", result.e.Message);
                success = false;
            }
            else if(result.result == WriteResult.PublisherDown)
            {
                log.Write(PublisherDown, DbTable.PublisherIsDownMessage("WriteRecordingStart"));
                success = false;
            }

            return success;
        }

        #endregion

        #region WriteRecordingStop

        /// <summary>
        ///     Ends a recording record, by associating a filepath to the recording and marking
        ///     the end time
        /// </summary>
        /// <param name="recordingId"> The key to the record to end </param>
        /// <param name="filepath"> The path of the media file </param>
        /// <returns> <c>true</c> if the record could be updated, otherwise <c>false</c> </returns>
        public bool WriteRecordingStop(uint recordingId, string url)
        {
            bool success = false;
            if(recordingId == 0)    return false;

            SqlBuilder builder = new SqlBuilder(Method.UPDATE, RecordingsTable.TableName);
            builder.AddFieldValue(RecordingsTable.End, DateTime.Now);
            builder.AddFieldValue(RecordingsTable.Url, url);
            builder.where[RecordingsTable.Id] = recordingId;

            WriteResultContainer result = ExecuteCommand(builder);

            if(result.result == WriteResult.Success)
            {
                success = true;
            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the WriteRecordingStop method\n" +
                    "Error message: {0}", result.e.Message);
                success = false;
            }
            else if(result.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("WriteRecordingStop"));
                success = false;
            }

            return success;
        }

        #endregion

        #region DeleteRecording

        /// <summary>
        ///     Deletes a recording record
        /// </summary>
        /// <param name="recordingId"> The key to the record to end </param>
        /// <returns> <c>true</c> if the record could be deleted, otherwise <c>false</c> </returns>
        public bool DeleteRecording(uint recordingId)
        {
            bool success = false;
            if(recordingId == 0)    return false;

            SqlBuilder builder = new SqlBuilder(Method.DELETE, RecordingsTable.TableName);
            builder.where[RecordingsTable.Id] = recordingId;

            WriteResultContainer result = ExecuteCommand(builder);
 
            if(result.result == WriteResult.Success)
            {
                success = true;
            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the DeleteRecordingRecord method\n" +
                    "Error message: {0}", result.e.Message);
                success = false;
            }
            else if(result.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("DeleteRecording"));
                success = false;
            }

            return success;
        }

        #endregion
    }
}
