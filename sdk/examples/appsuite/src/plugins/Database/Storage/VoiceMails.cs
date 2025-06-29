using System;
using System.Data;
using System.Collections;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Metreos.ApplicationSuite.Types;
using Method = Metreos.Utilities.SqlBuilder.Method;
using VoiceMailsTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.VoiceMails;
using Storage = Metreos.ApplicationSuite.Storage;


namespace Metreos.ApplicationSuite.Storage
{
    /// <summary>
    ///     Provides data access to the VoiceMail records table
    /// </summary>
    public class VoiceMails : DbTable
    {
        public VoiceMails(IDbConnection connection, LogWriter log, string applicationName, string partitionName, bool allowWrite)
            : base(connection, log, applicationName, partitionName, allowWrite) { }
        
        public static bool StatusStringToUInt(string msgStatus, out uint messageStatus)
        {
            bool success = false;
            try
            {
                messageStatus = Convert.ToUInt32( Enum.Parse(typeof(MessageStatus), msgStatus, true) );
                success = true;
            }
            catch
            {
                messageStatus = 0;
            }
            
            return success;
        }

        public static bool StatusUIntToString(uint msgStatus, out string messageStatus)
        {
            bool success = false;
            try
            {
                messageStatus = Enum.GetName(typeof(MessageStatus), msgStatus);
                success = true;
            }
            catch
            {
                messageStatus = null;
            }

            return success;
        }

        #region GetVoiceMailRecords
        public bool GetVoiceMailRecords(uint userId, out VoiceMailRecordCollection recordCollection, string msgStatus)
        {
            recordCollection = null;
            uint messageStatus;

            if ( ! VoiceMails.StatusStringToUInt(msgStatus, out messageStatus) )
            {
                log.Write(TraceLevel.Error, "Could not convert specified string to corresponding enum in the " +
                    "GetVoiceMailRecords method.");
                return false;
            }

            SqlBuilder builder = new SqlBuilder(Method.SELECT, VoiceMailsTable.TableName);
            builder.where[VoiceMailsTable.UserId] = userId;
            builder.where[VoiceMailsTable.Status] = messageStatus;

            AdvancedReadResultContainer result = ExecuteEasyQuery(builder);

            if(result.result == ReadResult.Success)
            {
                DataTable table = result.results;

                if (table.Rows.Count == 0)
                {
                    log.Write(TraceLevel.Warning, "{0}{1}{2}", "Did not find any VoiceMail records for user Id: '", 
                        userId, "' !");
                    return false;
                }
                
                recordCollection = new VoiceMailRecordCollection();

                foreach (DataRow row in table.Rows)
                {
                    uint recordId = Convert.ToUInt32(row[VoiceMailsTable.Id]);
                    uint uId = Convert.ToUInt32(row[VoiceMailsTable.UserId]);
                    uint status = Convert.ToUInt32(row[VoiceMailsTable.Status]);
                    uint length = Convert.ToUInt32(row[VoiceMailsTable.Length]);
                    string filename = Convert.ToString(row[VoiceMailsTable.Filename]);
                    DateTime timeStamp = Convert.ToDateTime(row[VoiceMailsTable.TimeStamp]);

                    // recordCollection[recordId] = new VoiceMailRecord(recordId, uId, filename, status, length, timeStamp);
                }

                return true;
            }
            else
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the GetVoiceMailRecords method, using user Id: '{0}'\n" +
                    "Error message: {1}", userId, result.e.Message);
      
                return false;
            }
        }
        #endregion

        #region InsertRecordIntoDatabase
        public bool InsertRecordIntoDatabase(VoiceMailRecord record)
        {
            SqlBuilder builder = new SqlBuilder(Method.INSERT, VoiceMailsTable.TableName);
            builder.AddFieldValue(VoiceMailsTable.UserId, record.UserId);
            builder.AddFieldValue(VoiceMailsTable.Status, record.Status);
            builder.AddFieldValue(VoiceMailsTable.Filename, record.Filename);
            builder.AddFieldValue(VoiceMailsTable.TimeStamp, record.TimeStamp);
            builder.AddFieldValue(VoiceMailsTable.Length, record.Length);

            WriteResultContainer result = ExecuteCommand(builder);
 
            if(result.result == WriteResult.Success)
            {
                return true;
            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the InsertRecordIntoDatabase method, for record with filename: '{0}'\n" +
                    "Error message: {1}", record.Filename, result.e.Message);
                return false;
            }
            else if(result.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("InsertRecordIntoDatabase"));
                return false;
            }

            return false;
        }
        #endregion
    }
}