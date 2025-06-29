using System;
using System.Data;
using System.Diagnostics;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using SC=Metreos.Native.ScheduledConference.ScheduledConference;
using Method = Metreos.Utilities.SqlBuilder.Method;
using SchedConfFilesTable = Metreos.Native.ScheduledConference.ScheduledConference.ScheduledConferencesFiles;
using Metreos.Utilities;

namespace Metreos.Native.ScheduledConference
{
    public enum RemoveFromQueueReturnValues
    {
        Success, 
        Failure,
        NoNextFile
    }

    /// <summary>Removes a filename-queue entry, and returns the next one to play</summary>
    public class RemoveFromQueue: INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The key of this queue item", true)]
        public  uint QueueId { set { queueId = value; } }
        private uint queueId;

        [ActionParamField("Participant conference PIN.", true)]
        public  uint Pin { set { pin = value; } }
        private uint pin;

        [ResultDataField("The next key of this queue item")]
        public  uint NextQueueId { get { return nextQueueId; } }
        private uint nextQueueId;

        [ResultDataField("The type of the next announcement")]
        public  uint NextType { get { return nextType; } }
        private uint nextType;

        [ResultDataField("The filename of the next announcement. Is String.Empty if not defined")]
        public  string NextFilename { get { return nextFilename; } }
        private string nextFilename;
        
        public RemoveFromQueue()
        {
            Clear();
        }

        #region INativeAction Implementation

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            nextFilename = String.Empty;
            pin = queueId = nextQueueId = nextType = 0;			 
        }

        [ReturnValue(typeof(RemoveFromQueueReturnValues), "NoNextFile means that the queue is empty")]
        [Action("RemoveFromQueue", false, "Remove From Queue", "Removes a file record from the queue, returning the next one to play")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            bool alreadyOpen = false;
            IDbConnection connection = null;
            try
            {
                connection = SC.GetConnection(sessionData, ScheduledConference.ScDbConnectionName, ScheduledConference.ScDbConnectionString);
                if(connection.State == ConnectionState.Open)
                {
                    alreadyOpen = true;
                }
                else
                {
                    connection.Open();
                }
            }
            catch (Exception e)
            {
                object[] msgArray = new object[2] { SC.ScDbConnectionString, e.Message } ;
                log.Write(TraceLevel.Warning, "Could not open database at {0}.\n" + "Error Message: {1}", msgArray);
                return AddToQueueReturnValues.Failure.ToString();
            }

            RemoveFromQueueReturnValues result = RemoveFromQueueWork(
                connection,
                pin, 
                queueId,
                out nextFilename,
                out nextQueueId,
                out nextType);

            try
            {
                if(!alreadyOpen)
                {
                    connection.Close();
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Could not close database connection. " + Exceptions.FormatException(e));
            }

            return result.ToString();
        }

        #endregion

        #region RemoveFromQueue
        
        public RemoveFromQueueReturnValues RemoveFromQueueWork(
            IDbConnection connection,
            uint pin,
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

            string query = builder.ToString();

            try
            {
                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the RemoveFromQueue method\n" +
                    "Error message: {0}", e.Message);
                return RemoveFromQueueReturnValues.Failure;
            }

            builder = new SqlBuilder(Method.SELECT, SchedConfFilesTable.TableName);
            builder.fieldNames.Add(SchedConfFilesTable.Type);
            builder.fieldNames.Add(SchedConfFilesTable.Filename);
            builder.fieldNames.Add(SchedConfFilesTable.Time);
            builder.fieldNames.Add(SchedConfFilesTable.Id);

            builder.where[SchedConfFilesTable.Pin] = pin;

            query = builder.ToString(); 
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
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the AddToQueue method. Unable to add to the queue\n" +
                    "Error message: {0}", e.Message);
                return RemoveFromQueueReturnValues.Failure;
            }

            return RemoveFromQueueReturnValues.Success;
        }

        #endregion
    }
}
