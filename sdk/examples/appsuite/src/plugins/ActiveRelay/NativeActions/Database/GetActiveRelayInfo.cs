using System;
using System.Data;
using System.Diagnostics;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Utilities;

namespace Metreos.Native.ActiveRelay
{
	/// <summary>
	/// Retrieves ActiveRelay infor for specified userId
	/// </summary>
    [PackageDecl("Metreos.Native.ActiveRelay")]
    public class GetActiveRelayInfo : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("User ID", true)]
        public uint UserId { set { userId = value; } }
        private uint userId;

        [ResultDataField("The routing guid of the application handling the Active Relay call")]
        public string AppRoutingGuid { get { return appRoutingGuid; } }
        private string appRoutingGuid;
        
        [ResultDataField("DateTime when this call was initially handled by Active Relay")]
        public DateTime TimeStamp { get { return timeStamp; } }
        private DateTime timeStamp;

        [ResultDataField("The number of the calling party")]
        public string FromNumber { get { return fromNumber; } }
        private string fromNumber;

        [ResultDataField("The destination party number")]
        public string ToNumber { get { return toNumber; } }
        private string toNumber;

        [ResultDataField("'true' indicates that the call was already swapped")]
        public bool WasSwapped { get { return wasSwapped; } }
        private bool wasSwapped;
 
        public GetActiveRelayInfo()
        {
            Clear();
        }
 
        public void Clear()
        {
            timeStamp           = DateTime.MinValue;
            appRoutingGuid      = null;
            userId              = 0;
            fromNumber          = null;
            toNumber            = null;
            wasSwapped          = false;
        }

        public void Reset()
        {
            fromNumber          = String.Empty;
            toNumber            = String.Empty;
            appRoutingGuid      = String.Empty;
        }

        public bool ValidateInput()
        {
            return (userId < ActiveRelay.StandardPrimaryKeySeed) ? false : true;
        }
 
        [ReturnValue(typeof(Result), "NoRecord indicates this device/user has no record.  Failure indicates database failure or critical inconsistency.")]
        [Action("GetActiveRelayInfo", false, "Retrieves AR Info", "Retrieves Active Relay call information specific to specified user")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.SELECT, ActiveRelay.TableName);
            builder.fieldNames.Add(ActiveRelay.Id);
            builder.fieldNames.Add(ActiveRelay.RoutingGuid);
            builder.fieldNames.Add(ActiveRelay.TimeStamp);
            builder.fieldNames.Add(ActiveRelay.FromNumber);
            builder.fieldNames.Add(ActiveRelay.ToNumber);
            builder.fieldNames.Add(ActiveRelay.WasSwapped);
            builder.where[ActiveRelay.UserId] = userId;
            
            string returnValue = Result.Failure.ToString();
            IDbConnection connection = null;
            connection = ActiveRelay.GetConnection(sessionData, ActiveRelay.ArDbConnectionName, ActiveRelay.ArDbConnectionString);
            
            try
            {
                if (connection.State == ConnectionState.Broken || connection.State == ConnectionState.Closed)
                    connection.Open();                
            }
            catch (Exception e)
            {
                log.Write(TraceLevel.Warning, "Could not open database at {0}.\n" + "Error Message: {1}", 
                    ActiveRelay.ArDbConnectionString, e.Message);
                Reset();
                if (connection != null)
                    connection.Close();
                return IApp.VALUE_FAILURE;
            }

            try
            {
                string select = builder.ToString();

                DataTable table = ActiveRelay.ExecuteQuery(select, connection);
                
                if (table == null || table.Rows.Count == 0)
                {
                    log.Write(TraceLevel.Warning, "{0}{1}", "Found no active ActiveRelay call records for UserId: ", userId);
                    Reset();
                    returnValue = Result.NoRecord.ToString();
                }
                else if (table.Rows.Count > 1)
                {
                    log.Write(TraceLevel.Warning, "Found multiple active ActiveRelay call records for UserId: {0}.  Choosing most recent.", userId);
                    
                    DataRow row = CleanupRecords(table, connection);
                    
                    if(ExtractToResultData(row) == true)    returnValue = IApp.VALUE_SUCCESS;
                    else                                    returnValue = IApp.VALUE_FAILURE;
                }
                else
                {
                    DataRow onlyRow = table.Rows[0];
                    if(ExtractToResultData(onlyRow) == true)    returnValue = IApp.VALUE_SUCCESS;
                    else                                        returnValue = IApp.VALUE_FAILURE;
                }
            }
            catch (Exception e)
            {
                log.Write(TraceLevel.Warning, "Error encountered in the GetActiveRelayInfo method, using UserId: {0}\n"+
                    "Error message: {1}", userId, e.Message);
                Reset();
                returnValue = IApp.VALUE_FAILURE;
            }

            if (connection != null)
                connection.Close();
            return returnValue;
        }

        /// <summary>
        ///     Removes all old records from ActiveRelay database for a given user, returning valid record
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        protected DataRow CleanupRecords(DataTable table, IDbConnection connection)
        {
            Debug.Assert(table.Rows.Count > 0, "Record clean utility method invoked incorrectly");

            if(table.Rows.Count == 1)   return table.Rows[0];

            DateTime mostRecent = FindMostRecentRecord(table);

            DataRow mostRecentRow = null;
            foreach(DataRow row in table.Rows)
            {
                object point = row[ActiveRelay.TimeStamp];
                if(Convert.IsDBNull(point))  DeleteRecord(row, connection);

                DateTime datetime = Convert.ToDateTime(point);
                if(datetime == mostRecent)  mostRecentRow = row;
                else                        DeleteRecord(row, connection);
            }

            return mostRecentRow;
        }

        protected void DeleteRecord(DataRow row, IDbConnection connection)
        {
            int id = Convert.ToInt32(row[ActiveRelay.Id]);

            SqlBuilder deleteRow = new SqlBuilder(SqlBuilder.Method.DELETE, ActiveRelay.TableName);
            deleteRow.where[ActiveRelay.Id] = id;

            string delete = deleteRow.ToString();
            try
            {
                ActiveRelay.ExecuteNonQuery(delete, connection);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Unable to delete duplicate record for user {0}.  Exception following:\n {1}", 
                    id, Exceptions.FormatException(e));
            }
        }

        protected DateTime FindMostRecentRecord(DataTable table)
        {
            DateTime mostRecent = DateTime.MinValue;

            foreach(DataRow row in table.Rows)
            {
                object point = row[ActiveRelay.TimeStamp];
                if(Convert.IsDBNull(point))  continue;

                DateTime datetime = Convert.ToDateTime(point);

                if(datetime.Subtract(mostRecent) > TimeSpan.Zero)
                    mostRecent = datetime;
            }

            return mostRecent;
        }

        protected bool ExtractToResultData(DataRow row)
        { 
            appRoutingGuid      = row[ActiveRelay.RoutingGuid] as string;
            fromNumber          = row[ActiveRelay.FromNumber] as string;
            toNumber            = row[ActiveRelay.ToNumber] as string;
            wasSwapped          = Convert.ToBoolean(row[ActiveRelay.WasSwapped]);
            
            if ( ! Convert.IsDBNull(row[ActiveRelay.TimeStamp]) )
                timeStamp = (DateTime) (row[ActiveRelay.TimeStamp]);
                
            if (fromNumber == null)
                fromNumber = string.Empty;

            if (toNumber == null)
                toNumber = string.Empty;

            if (appRoutingGuid == null)
            {
                Reset();
                return false;
            }

            return true;
        }

        protected enum Result
        {
            Success,  // One record for this device
            NoRecord, // No records for this device/user
            Failure // Db failure
        }
    }
}
