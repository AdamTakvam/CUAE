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
    public enum AddToQueueReturnValues
    {
        Queued,
        PlayFile,
        Failure
    }

    /// <summary>Adds a file to the queue</summary>
    public class AddToQueue: INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("ScheduledConference record ID.", true)]
        public  uint Pin { set { pin = value; } }
        private uint pin;

        [ActionParamField("1 for entry into conference, 2 for exiting conference", true)]
        public	uint AnnouncementType { set { announcementType = value; } }
        private uint announcementType;
		
        [ActionParamField("Additional file to queue with incoming call", false)]
        public	string Filename { set { filename = value; } }
        private string filename;
        
        [ResultDataField("The key of this queue item")]
        public  uint QueueId { get { return queueId; } }
        private uint queueId;

        public AddToQueue()
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
            filename = String.Empty;
            pin = announcementType = queueId = 0;			 
        }

        [ReturnValue(typeof(AddToQueueReturnValues), "PlayFile means that the queue was empty and no files were being played, so proceed with play.  The file will be added to the queue")]
        [Action("AddToQueue", false, "Add To Queue", "Adds to a queue of outstanding filenames to play, unless the a file is not playing currently")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            if(filename == null) filename = String.Empty;

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
                log.Write(TraceLevel.Error, "Could not open database at {0}.\n" + "Error Message: {1}", msgArray);
                return AddToQueueReturnValues.Failure.ToString();
            }

            AddToQueueReturnValues result = AddToQueueWork(
                connection,
                pin, 
                announcementType, 
                filename,
                out queueId);

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

        #region AddToQueue
        
        public AddToQueueReturnValues AddToQueueWork(
            IDbConnection connection,
            uint pin, 
            uint announcementType,
            string filename,
            out uint queueId)
        {
            queueId = 0;

            // First find if there are any records in the sched conf file queue table for this 
            // conference id
            SqlBuilder builder = new SqlBuilder(Method.SELECT, SchedConfFilesTable.TableName);
            builder.where[SchedConfFilesTable.Pin] = pin;

            string query = builder.ToString();

            bool recordFound = false;
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

                if(results != null && results.Rows.Count > 0 )
                {
                    recordFound = true;
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the AddToQueue method\n" +
                    "Error message: {0}", e.Message);
                return AddToQueueReturnValues.Failure;
            }

            // Regardless of the presence of other records, we need to add a record for this file
            // Depending on if we found any records, we flag 'playing' on the file

            builder = new SqlBuilder(Method.INSERT, SchedConfFilesTable.TableName);
            builder.fieldNames.Add(SchedConfFilesTable.Pin);
            builder.fieldNames.Add(SchedConfFilesTable.Type);
            builder.fieldNames.Add(SchedConfFilesTable.Filename);

            builder.fieldValues.Add(pin);
            builder.fieldValues.Add(announcementType);
            builder.fieldValues.Add(filename);

            query = builder.ToString();
            try
            {
                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.ExecuteNonQuery();                    
                }

                using(IDbCommand command = connection.CreateCommand())
                {
                    queueId = SC.GetLastAutoId(command);
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the AddToQueue method. Unable to add to the queue\n" +
                    "Error message: {0}", e.Message);
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
    }
}
