using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Method = Metreos.Utilities.SqlBuilder.Method;
using RegistrationsTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.Registrations;
namespace Metreos.ApplicationSuite.Storage
{
    /// <summary>
    ///     Provides data access to the Devices table, and any information which stem from that
    /// </summary>
    public class Registrations : DbTable
    {
        public Registrations(IDbConnection connection, LogWriter log, string applicationName, string partitionName, bool allowWrite)
            : base(connection, log, applicationName, partitionName, allowWrite) { }

        #region WriteRegistrationStart
        /// <summary>
        /// This method writes a new registration record, and also provides the key to that record
        /// </summary>
        public bool WriteRegistrationStart(string sid, string ccmAddress, out int id)
        {
            id = 0;
            bool success = false;

            SqlBuilder builder = new SqlBuilder(Method.INSERT, RegistrationsTable.TableName);
            builder.fieldNames.Add(RegistrationsTable.Sid);
            builder.fieldNames.Add(RegistrationsTable.CcmAddress);
            builder.fieldNames.Add(RegistrationsTable.StartTime);
            builder.fieldValues.Add(sid);
            builder.fieldValues.Add(ccmAddress);
            SqlBuilder.PreformattedValue now = new Metreos.Utilities.SqlBuilder.PreformattedValue("NOW()");
            builder.fieldValues.Add(now);

            WriteResultContainer result = ExecuteCommand(builder);

            if(result.result == WriteResult.Success)
            {
                int rowsAffected = result.rowsAffected;

                if(rowsAffected < 1)
                {
                    log.Write(TraceLevel.Error, "No record was written for WriteRegistrationStart.\n" +
                        "Sid '{0}'\n" + 
                        "CcmAddress '{1}'.",
                        sid, ccmAddress);
                    success = false;
                }
                else
                {
                    id = (int) result.lastInsertId;

                    if(id < 1)
                    {
                        log.Write(TraceLevel.Error, "LAST_INSERT_ID() method failed to return valid ID after successful INSERT");
                        success = false;
                    }
                    else
                    {
                        success = true;
                    }
                }
            }
            else if(result.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the WriteRegistrationStart method, using Sid '{0}' and CcmAddress '{1}'\n" +
                    "Error message: {2}", sid, ccmAddress, result.e.Message );
                success = false;
            }
            else if(result.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown,
                    DbTable.PublisherIsDownMessage("WriteRegistrationStart"));
                success = false;
            }

            return success;
        }
        #endregion

        #region WriteRegistrationStop
        /// <summary>
        /// This method writes a new registration record, and also provides the key to that record
        /// </summary>
        public bool WriteRegistrationStop(int id, int numRingIn, int numRingOut, int numBusy, int numConnected)
        {
            bool success = false;

            SqlBuilder builder = new SqlBuilder(Method.UPDATE, RegistrationsTable.TableName);
            builder.fieldNames.Add(RegistrationsTable.NumRingIn);
            builder.fieldNames.Add(RegistrationsTable.NumRingOut);
            builder.fieldNames.Add(RegistrationsTable.NumBusy);
            builder.fieldNames.Add(RegistrationsTable.NumConnected);
            builder.fieldNames.Add(RegistrationsTable.EndTime);
            builder.fieldValues.Add(numRingIn);
            builder.fieldValues.Add(numRingOut);
            builder.fieldValues.Add(numBusy);
            builder.fieldValues.Add(numConnected);
            SqlBuilder.PreformattedValue now = new Metreos.Utilities.SqlBuilder.PreformattedValue("NOW()");
            builder.fieldValues.Add(now);
            builder.where[RegistrationsTable.Id] = id;

            WriteResultContainer writeResult = ExecuteCommand(builder);
 
            if(writeResult.result == WriteResult.Success)
            {
                if(writeResult.rowsAffected < 1)
                {
                    log.Write(TraceLevel.Error, "No record was updated for WriteRegistrationStop.\n" +
                        "NumRingIn    '{0}'\n" + 
                        "NumRingOut   '{1}'\n" + 
                        "NumBusy      '{2}'\n" + 
                        "NumConnected '{3}'\n" + 
                        "Id           '{4}'", 
                        numRingIn, numRingOut, numBusy, numConnected, id);
                    success = false;
                }
                else
                {  
                    success = true;
                }
            }
            else if(writeResult.result == WriteResult.DbFailure)
            {
                log.Write(TraceLevel.Error, 
                    "Error encountered in the WriteRegistrationStart method, using\n" + 
                    "NumRingIn    '{0}'\n" + 
                    "NumRingOut   '{1}'\n" + 
                    "NumBusy      '{2}'\n" + 
                    "NumConnected '{3}'\n" + 
                    "Id           '{4}'\n", 
                    "Error message '{5}'",
                    numRingIn, numRingOut, numBusy, numConnected, id, writeResult.e.Message );
                success = false;
            }
            else if(writeResult.result == WriteResult.PublisherDown)
            {
                log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("WriteRegistrationStart"));
                success = false;
            }

            return success;
        }
        #endregion
  
    }
}
