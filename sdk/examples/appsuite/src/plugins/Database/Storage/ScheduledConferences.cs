using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Globalization;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Method = Metreos.Utilities.SqlBuilder.Method;
using SchedConfTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.ScheduledConferences;
using SchedConfFilesTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.ScheduledConferencesFiles;

namespace Metreos.ApplicationSuite.Storage
{
    /// <summary>
    ///     Provides data access to the scheduled conferences table
    /// </summary>
    public class ScheduledConferences : DbTable
    {
        public ScheduledConferences(IDbConnection connection, LogWriter log, string applicationName, string partitionName, bool allowWrite)
            : base(connection, log, applicationName, partitionName, allowWrite) { }

        public enum IsConferenceReadyToStartReturnValues
        {
            Yes,
            No,
            Expired,
            Failure
        }

        public enum GetConferenceReturnValues
        {
            success,
            NoConference,
            failure
        }

        public enum RemoveFromQueueReturnValues
        {
            Failure,
            Success,
            NoNextFile
        }

        public enum AddToQueueReturnValues
        {
            Success,
            Failure,
            Queued,
            PlayFile
        }

        #region GetConferenceInfo

        /// <summary>
        ///     Retrieves all conference data
        /// </summary>
        /// <param name="conferencePin">
        ///     The pin the user entered by the user to get into the conference
        /// </param>
        /// <param name="scheduledConferenceId">
        ///     The key to the ScheduledConference ID table.
        /// </param>
        /// <param name="mmsId">
        ///     The media server ID
        /// </param>
        /// <param name="mmsConfId">
        ///     The conference ID returned from the media server
        /// </param>
        /// <param name="scheduledTime">
        ///     The scheduled time for the conference to start
        /// </param>
        /// <param name="durationMinutes">
        ///     The length in minutes that this conference can last for
        /// </param>
        /// <param name="numParticipants">
        ///     The number of participants expected for this conference
        /// </param>
        /// <param name="isHost">
        ///     <c>true</c> if the conferencePin specified is the host pin, otherwise <c>false</c>
        /// </param>
        /// <returns>
        ///     success if everything went well, failure if a database error occurred, or
        ///     NoConference if no conference was found with the specified conferencePin
        /// </returns>
        public GetConferenceReturnValues GetConferenceInfo(
            uint conferencePin, 
            out uint scheduledConferenceId,
            out uint mmsId, 
            out uint mmsConfId, 
            out DateTime scheduledTime,
            out uint durationMinutes,
            out uint numParticipants,
            out bool isHost,
            out uint hostConferencePin)
        {
            scheduledConferenceId = 0;
            mmsId = 0;
            mmsConfId = 0;
            scheduledTime = DateTime.MinValue;
            durationMinutes = 0;
            numParticipants = 0;
            hostConferencePin = 0;
            isHost = false;

            SqlBuilder builder = new SqlBuilder(Method.SELECT, SchedConfTable.TableName);
            builder.fieldNames.Add(SchedConfTable.Id);
            builder.fieldNames.Add(SchedConfTable.HostConfId);
            builder.fieldNames.Add(SchedConfTable.MmsConfId);
            builder.fieldNames.Add(SchedConfTable.MmsId);
            builder.fieldNames.Add(SchedConfTable.NumParticipants);
            builder.fieldNames.Add(SchedConfTable.ParticipantConfId);
            builder.fieldNames.Add(SchedConfTable.ScheduledTimestamp);
            builder.fieldNames.Add(SchedConfTable.DurationMinutes);
            builder.appendSemicolon = false;

            string whereClause = String.Format(
                " WHERE ({0} = {1}) OR ({2} = {1})",
                SchedConfTable.HostConfId,
                conferencePin,
                SchedConfTable.ParticipantConfId);

            string query = String.Format("{0} {1}", builder, whereClause);  

            AdvancedReadResultContainer result = ExecuteEasyQuery(query);

            if(result.result == ReadResult.Success)
            {
                DataTable results = result.results;

                DataRow row = null;
                if(results != null && results.Rows.Count == 1)
                {
                    row = results.Rows[0];
                    scheduledConferenceId   = Convert.ToUInt32(row[SchedConfTable.Id]);
                    mmsId                   = Convert.ToUInt32(row[SchedConfTable.MmsId]);
                    mmsConfId               = Convert.ToUInt32(row[SchedConfTable.MmsConfId]);
                    scheduledTime           = Convert.ToDateTime(row[SchedConfTable.ScheduledTimestamp]);
                    durationMinutes         = Convert.ToUInt32(row[SchedConfTable.DurationMinutes]);
                    numParticipants         = Convert.ToUInt32(row[SchedConfTable.NumParticipants]);
                    hostConferencePin       = Convert.ToUInt32(row[SchedConfTable.HostConfId]);

                    isHost = hostConferencePin == conferencePin;
                }
                else if(results != null && results.Rows.Count > 1)
                {
                    log.Write(TraceLevel.Error, "Multiple conferences found with confirmation pin of '{0}'", conferencePin);
                    return GetConferenceReturnValues.failure;
                }
                else
                {
                    return GetConferenceReturnValues.NoConference;
                }
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetConferenceInfo method, using confirmation pin of '{0}'\n" +
                    "Error message: {1}", conferencePin, result.e.Message);
                return GetConferenceReturnValues.failure;
            }

            return GetConferenceReturnValues.success;
        }

        #endregion

        #region IsConferenceReadyToStart

        /// <summary>
        ///     Indicates if a conference is allowed to start
        /// </summary>
        /// <param name="conferencePin">
        ///     The pin the user entered by the user to get into the conference
        /// </param>
        /// <param name="toleranceMinutes">
        ///     The amount of minutes that the system will tolerate to allow a 
        ///     conference to start before the scheduled time
        /// </param>
        /// <returns>
        ///     true if the conference is ready to start, false if not, failure if the
        ///     database failed
        /// </returns>
        /* NOT ACTUALLY USED
         * 
        public IsConferenceReadyToStartReturnValues IsConferenceReadyToStart(
            uint conferencePin, int toleranceMinutes)
        {
            SqlBuilder builder = new SqlBuilder(Method.SELECT, SchedConfTable.TableName);
            builder.fieldNames.Add(SchedConfTable.ScheduledTimestamp);
            
            string whereClause = String.Format(
                " WHERE ({0} = {1}) OR ({2} = {1})",
                SchedConfTable.HostConfId,
                conferencePin,
                SchedConfTable.ParticipantConfId);

            string query = String.Format("{0} {1}", builder, whereClause);  

            try
            {
                DataTable results = null;
                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    using(IDataReader reader = command.ExecuteReader())
                    {
                        results = Database.GetDataTable(reader);
                    }
                }

                DataRow row = null;
                if(results != null && results.Rows.Count == 1)
                {
                    row                     = results.Rows[0];                 
                    DateTime scheduledTime  = Convert.ToDateTime(row[SchedConfTable.ScheduledTimestamp]);
                    
                    TimeSpan difference = DateTime.Now.Subtract(scheduledTime.AddMinutes(-toleranceMinutes));
                    if(difference < TimeSpan.Zero)  return IsConferenceReadyToStartReturnValues.True;
                    else                            return IsConferenceReadyToStartReturnValues.False;
                }
                else if(results != null && results.Rows.Count > 1)
                {
                    log.Write(TraceLevel.Error, "Multiple conferences found with confirmation pin of '{0}'", conferencePin);
                    return IsConferenceReadyToStartReturnValues.Failure;
                }
                else
                {
                    log.Write(TraceLevel.Error, "No conference found with confirmation pin of '{0}'", conferencePin);
                    return IsConferenceReadyToStartReturnValues.Failure;
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the IsConferenceReadyToStart method, using confirmation pin of '{0}'\n" +
                    "Error message: {1}", conferencePin, e.Message);
                return IsConferenceReadyToStartReturnValues.Failure;
            }
        }
        */
        #endregion

        #region UpdateConference

        /// <summary>
        ///     Updates the conference with values not available when initially reserved via 
        ///     the web or other means.  These values are available once the first participant
        ///     has entered the conference.
        /// </summary>
        /// <param name="scheduledConferenceId">
        ///     The key of the Scheduled Conferences table
        /// </param>
        /// <param name="mmsId">
        ///     The media server ID
        /// </param>
        /// <param name="mmsConfId">
        ///     The conference ID returned by the media server
        /// </param>
        /// <returns>
        ///     success if the conference could be updated, otherwise failure, indicating
        ///     no conference exists or the database failed
        /// </returns>
        public bool UpdateConference(uint scheduledConferenceId, uint mmsId, uint mmsConfId)
        {
            SqlBuilder builder = new SqlBuilder(Method.UPDATE, SchedConfTable.TableName);
            builder.AddFieldValue(SchedConfTable.MmsId, mmsId);
            builder.AddFieldValue(SchedConfTable.MmsConfId, mmsConfId);
            builder.where[SchedConfTable.Id] = scheduledConferenceId;

            WriteResultContainer result = ExecuteCommand(builder);
 
            if(result.result == WriteResult.Success)
            {
                int numAffected = result.rowsAffected;

                if(numAffected == 1)
                {
                    return true;
                }
                else if(numAffected > 1)
                {
                    log.Write(TraceLevel.Error, 
                        "Duplicate conferences found with a Scheduled Conference Id of '{0}'", 
                        scheduledConferenceId);
                    return false;
                }
                else
                {
                    log.Write(TraceLevel.Error, 
                        "No conference found with a Scheduled Conference Id of '{0}'", 
                        scheduledConferenceId);
                    return false;
                }
            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the UpdateConference method, using Scheduled Conference Id of '{0}'\n" +
                    "Error message: {1}", scheduledConferenceId, result.e.Message);
                return false;
            }
            else if(result.result == WriteResult.PublisherDown)
            {
			    log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("UpdateConference"));
                return false;
            }

            return false;
        }

        #endregion

        #region AddToQueue
        
        public AddToQueueReturnValues AddToQueue(
            uint scheduledConferenceId, 
            uint announcementType,
            string filename,
            out uint queueId)
        {
            queueId = 0;

            // First find if there are any records in the sched conf file queue table for this 
            // conference id
            SqlBuilder builder = new SqlBuilder(Method.SELECT, SchedConfFilesTable.TableName);
            builder.where[SchedConfFilesTable.ScheduledConferenceId] = scheduledConferenceId;

            string query = builder.ToString();

            bool recordFound = false;

            AdvancedReadResultContainer result = ExecuteEasyQuery(builder);

            if(result.result == ReadResult.Success)
            {
                DataTable results = result.results;

                if(results != null && results.Rows.Count > 0 )
                {
                    recordFound = true;
                }
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the AddToQueue method\n" +
                    "Error message: {0}", result.e.Message);
                return AddToQueueReturnValues.Failure;
            }

            // Regardless of the presence of other records, we need to add a record for this file
            // Depending on if we found any records, we flag 'playing' on the file

            builder = new SqlBuilder(Method.INSERT, SchedConfFilesTable.TableName);
            builder.fieldNames.Add(SchedConfFilesTable.ScheduledConferenceId);
            builder.fieldNames.Add(SchedConfFilesTable.Type);
            builder.fieldNames.Add(SchedConfFilesTable.Filename);

            builder.fieldValues.Add(scheduledConferenceId);
            builder.fieldValues.Add(announcementType);
            builder.fieldValues.Add(filename);

            WriteResultContainer writeResult = ExecuteCommand(builder);
 
            if(writeResult.result == WriteResult.Success)
            {
                queueId = writeResult.lastInsertId;
            }
            else if(writeResult.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the AddToQueue method. Unable to add to the queue\n" +
                    "Error message: {0}", writeResult.e.Message);
                return AddToQueueReturnValues.Failure;
            }
            else if(writeResult.result == WriteResult.PublisherDown)
            {
			    log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("AddToQueue"));
                return AddToQueueReturnValues.Failure;
            }

            if(recordFound)
            {
                return AddToQueueReturnValues.Queued;
            }
            else
            {
                return AddToQueueReturnValues.PlayFile;
            }
        }

        #endregion

        #region RemoveFromQueue
        
        public RemoveFromQueueReturnValues RemoveFromQueue(
            uint scheduledConferenceId,
            uint queueId,
            out string nextFilename,
            out uint nextQueueId,
            out uint nextType
            )
        {
            nextFilename = String.Empty;
            nextQueueId = 0;
            nextType = 0;

            // Delete the record from the queue
            SqlBuilder builder = new SqlBuilder(Method.DELETE, SchedConfFilesTable.TableName);
            builder.where[SchedConfFilesTable.Id] = queueId;

            WriteResultContainer result = ExecuteCommand(builder);
 
            if(result.result == WriteResult.Success)
            {

            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the RemoveFromQueue method\n" +
                    "Error message: {0}", result.e.Message);
                return RemoveFromQueueReturnValues.Failure;
            }
            else if(result.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("RemoveFromQueue"));
                return RemoveFromQueueReturnValues.Failure;
            }

            builder = new SqlBuilder(Method.SELECT, SchedConfFilesTable.TableName);
            builder.fieldNames.Add(SchedConfFilesTable.Type);
            builder.fieldNames.Add(SchedConfFilesTable.Filename);
            builder.fieldNames.Add(SchedConfFilesTable.Time);
            builder.fieldNames.Add(SchedConfFilesTable.Id);

            builder.where[SchedConfFilesTable.ScheduledConferenceId] = scheduledConferenceId;

            AdvancedReadResultContainer readResult = ExecuteEasyQuery(builder);

            if(readResult.result == ReadResult.Success)
            {
                DataTable results = readResult.results;

                if(results == null || results.Rows.Count == 0)
                {
                    return RemoveFromQueueReturnValues.NoNextFile;
                }

                DateTime earliest = DateTime.MinValue;
                foreach(DataRow row in results.Rows)
                {
                    DateTime rowTime = Convert.ToDateTime(row[SchedConfFilesTable.Time]);

                    if(rowTime > earliest)
                    {
                        nextFilename = Convert.ToString(row[SchedConfFilesTable.Filename]);
                        nextQueueId = Convert.ToUInt32(row[SchedConfFilesTable.Id]);
                        nextType = Convert.ToUInt32(row[SchedConfFilesTable.Type]);
                    }
                }
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the AddToQueue method. Unable to add to the queue\n" +
                    "Error message: {0}", readResult.e.Message);
                return RemoveFromQueueReturnValues.Failure;
            }

            return RemoveFromQueueReturnValues.Success;
        }

        #endregion
    }
}
