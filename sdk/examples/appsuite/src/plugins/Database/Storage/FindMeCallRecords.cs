using System;
using System.Data;
using System.Collections;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Method = Metreos.Utilities.SqlBuilder.Method;
using FindMeCallRecordsTable = Metreos.ApplicationSuite.Storage.SqlConstants.Tables.FindMeCallRecords;

namespace Metreos.ApplicationSuite.Storage
{
	/// <summary>
	///     Provides data access to the FindMeCallRecords table
	/// </summary>
	public class FindMeCallRecords : DbTable
	{
		public FindMeCallRecords(IDbConnection connection, LogWriter log, string applicationName, string partitionName, bool allowWrite)
			: base(connection, log, applicationName, partitionName, allowWrite) { }

		#region WriteFindMeCallRecord

		/// <summary>
		///     Creates a FindMe call record with start time and other values
		/// </summary>
		/// <param name="callId">
		///     CallId of the CallRecord.  Specify 0 or negative for no user 
		/// </param>
		/// <param name="originNumber">
		///     The originating number.  Can be null
		/// </param>
		/// <param name="destinationNumber">
		///     The destination number. Can be null
		/// </param>
		/// <param name="typeOfCall">
		///     Type of FindMe call. See Storage.ActiveRelayCallTypes
		/// </param>
		/// <param name="endReason">
		///     End reason for the call. See Storage.EndReason
		/// </param>
		/// <returns></returns>
		public bool WriteFindMeCallRecord(
			uint callRecordsId, 
			string originNumber, 
			string destinationNumber,
			Storage.ActiveRelayCallTypes typeOfCall,
			Storage.EndReason endReason)
		{
			bool success = false;

            if (destinationNumber == null || destinationNumber == string.Empty)
                destinationNumber = "Unknown";

			SqlBuilder builder = new SqlBuilder(Method.INSERT, FindMeCallRecordsTable.TableName);
			builder.AddFieldValue(FindMeCallRecordsTable.CallRecordsId, callRecordsId);
			builder.AddFieldValue(FindMeCallRecordsTable.From, originNumber);
			builder.AddFieldValue(FindMeCallRecordsTable.To, destinationNumber);
			builder.AddFieldValue(FindMeCallRecordsTable.Type, (uint) typeOfCall);
			builder.AddFieldValue(FindMeCallRecordsTable.EndReason, (int) endReason);

			WriteResultContainer result = ExecuteCommand(builder);

			if(result.result == WriteResult.Success)
			{
				success = true;
			}
			else if(result.result == WriteResult.DbFailure)
			{
				log.Write(TraceLevel.Info, 
					"Error encountered in the WriteFindMeCallRecord method\n" +
					"Error message: {0}", result.e.Message);
				success = false;
			}
			else if(result.result == WriteResult.PublisherDown)
			{
				log.Write(DbTable.PublisherDown, DbTable.PublisherIsDownMessage("WriteFindMeCallRecord"));
				success = false;
			}
               
			return success;
		}

		#endregion
	
    } // class FindMeCallRecords
} // namespace
