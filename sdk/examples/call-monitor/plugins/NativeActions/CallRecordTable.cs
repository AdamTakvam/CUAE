using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Xml.Serialization;

using Metreos.Utilities;
using Metreos.LoggingFramework;

namespace Metreos.Native.CallMonitor
{
	/// <summary> 
	///     Replicates call records being stored to the database using .NET
	///     XML serialization to enable serialization of database record equivalent
	///     to file 
	/// </summary>
	[Serializable()]
	public class CallRecordTable
	{
        public static XmlSerializer Seri = new XmlSerializer(typeof(CallRecordTable));
        private static Hashtable fileLocks = new Hashtable();

		public CallRecordTable() {}

        public Record[] Records;

        #region Methods for saving xml to file, and retrieving from file
        public static CallRecordTable RetrieveRecords(string filepath, LogWriter log)
        {
            CallRecordTable record = null;

            if(File.Exists(filepath))
            {
                FileStream stream = null;

                try
                {
                    stream = File.Open(filepath, FileMode.Open, FileAccess.Read);
                    record = (CallRecordTable) Seri.Deserialize(stream);
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, 
                        "The call record table does not meet the current schema, " +
                        "and could not be deserialized.\n" + Exceptions.FormatException(e));
                }
                finally
                {
                    stream.Close();
                    stream = null;
                }
            }

            return record;
        }

        public static bool WriteRecords(string filepath, CallRecordTable record, LogWriter log)
        {
            bool success = false;

            if(record != null && record.Records != null)
            {
                success = SerializeFileWrite(filepath, record, log);
            }

            return success;
        }
        #endregion

        #region Methods for saving to database

        /// <summary>
        ///     Attempts to write all records to the specified database, returning
        ///     any records which failed to write to the database in a new CallRecordTable
        /// </summary>
        public static CallRecordTable WriteToDatabase(string dsn, CallRecordTable records, LogWriter log)
        {
            CallRecordTable errors = null;
            ArrayList failedWrites = new ArrayList();

            if(records != null && records.Records != null)
            {
                using(IDbConnection connection = Database.CreateConnection(Database.DbType.mysql, dsn))
                {
                    bool canWrite = false;
                    try
                    {
                        connection.Open();
                        canWrite = true;
                    }
                    catch(Exception e)
                    {
                        log.Write(TraceLevel.Error, "Unable to open a connection to the database.\n  " +
                            Exceptions.FormatException(e));
                        errors = records;
                    }

                    if(canWrite)
                    {
                        foreach(Record record in records.Records)
                        {
                            SqlBuilder sqlBuilder = new SqlBuilder(SqlBuilder.Method.INSERT, CallRecordDatabase.TableName);
                            sqlBuilder.fieldNames.Add(CallRecordDatabase.CustomerNumber);
                            sqlBuilder.fieldNames.Add(CallRecordDatabase.DidNumber);
                            sqlBuilder.fieldNames.Add(CallRecordDatabase.GovernmentAgentNumber);
                            sqlBuilder.fieldNames.Add(CallRecordDatabase.InsuranceAgentNumber);
                            sqlBuilder.fieldNames.Add(CallRecordDatabase.MonitoredSid);
                            sqlBuilder.fieldNames.Add(CallRecordDatabase.StartMonitorTime);

                            sqlBuilder.fieldValues.Add(record.customerNumber);
                            sqlBuilder.fieldValues.Add(record.did);
                            sqlBuilder.fieldValues.Add(record.governmentAgentNumber);
                            sqlBuilder.fieldValues.Add(record.insuranceAgentNumber);
                            sqlBuilder.fieldValues.Add(record.monitoredSid);

                            DateTime startTime = DateTime.MinValue;
                            try
                            {
                                startTime = DateTime.ParseExact(record.startMonitorTime, "yyyy-MM-dd HH.mm.ss:fff", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                            }
                            catch { }
                            sqlBuilder.fieldValues.Add(record.startMonitorTime);

                            try
                            {
                                using(IDbCommand command = connection.CreateCommand())
                                {
                                    command.CommandText = sqlBuilder.ToString();
                                    command.ExecuteNonQuery();
                                }
                            }
                            catch(Exception e)
                            {
                                log.Write(TraceLevel.Error, 
                                    "Unable to write indivdual record to database.\n  " + Exceptions.FormatException(e));

                                failedWrites.Add(record);
                            }
                        }
                    }
                }

                if(failedWrites.Count != 0)
                {
                    errors = new CallRecordTable();
                    errors.Records = new Record[failedWrites.Count];
                    failedWrites.CopyTo(errors.Records);
                }
            }

            return errors;
        }

        #endregion

        #region Serialize File Writing
        /// <summary>
        ///     Based on filepath (which is an imperfect key to a file), serialize file 
        ///     writes, since multiple instances of the same native action may be occuring
        ///     at once; both writing to the same file.
        /// </summary>
        public static bool SerializeFileWrite(string filepath, CallRecordTable callRecords, LogWriter log)
        {
            FileStream stream = null;
            bool success = false;
            lock(string.Intern(filepath))
            {
                if(callRecords != null && callRecords.Records != null)
                {
                    try
                    {
                        // Prepare file stream for writing
                        stream = File.Create(filepath);
                    }
                    catch(Exception e)
                    {
                        log.Write(TraceLevel.Error, 
                            "Unable to create the record file at '{0}' upon request to log a record.\n" + 
                            Exceptions.FormatException(e), filepath);
                    }

                    if(stream != null)
                    {
                        try
                        {
                            Seri.Serialize(stream, callRecords);
                            success = true;
                        }
                        catch(Exception e)
                        {
                            log.Write(TraceLevel.Error, 
                                "Unable to write records to the record file at '{0}'." 
                                + filepath + "\n" + Exceptions.FormatException(e));
                        }
                        finally
                        {
                            stream.Close();
                            stream = null;
                        }
                    }
                }
            }
            return success;
        }
        #endregion
	}

    [Serializable()]
    public class Record
    {
        public Record() {}
        [XmlAttributeAttribute("governmentAgentNumber")]
        public string governmentAgentNumber;

        [XmlAttributeAttribute("did")]
        public string did;

        [XmlAttributeAttribute("insuranceAgentNumber")]
        public string insuranceAgentNumber;

        [XmlAttributeAttribute("customerNumber")]
        public string customerNumber;

        [XmlAttributeAttribute("monitoredSid")]
        public string monitoredSid;

        [XmlAttributeAttribute("startMonitorTime")]
        public string startMonitorTime;
    }


}
