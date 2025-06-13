using System;
using System.Data;
using System.Diagnostics;
using Metreos.ApplicationFramework;
using Metreos.Utilities;
using MySql.Data.MySqlClient;

namespace Metreos.Native.CBarge
{
	#region Database constants
	public abstract class Const
	{
		public const string CbDbConnectionName = "cbarge";
		public const string CbDbConnectionString = "Server=localhost;User ID=root;Password=metreos;Database=cbarge";
		public const int StandardPrimaryKeySeed = 1;
		
		#region ReturnValues
		public enum ConferenceReturnValues
		{
			success,
			failure,
			NotFound
		}
		#endregion
	}

	public abstract class CBargeCallRecords
	{
		// Conferences table constants. 
		public const string TableName = "cbarge_call_records";
		public const string Id = "cbarge_id";
		public const string LineId = "line_id";
        public const string DeviceName = "device_name";
        public const string CallId = "call_id";
		public const string TimeStamp = "cb_timestamp";
        public const string RoutingGuid = "routing_guid";
        public const string BargeRoutingGuid = "barge_routing_guid";
	}

	#endregion

	public class DbInteraction
	{
		/// <summary>
        /// Returns the IDbConnection associated with the passed in connection name. The connection is retrieved from the
        /// given SessionData object. 
        /// </summary>
        public static IDbConnection GetConnection(SessionData sessionData, string connectionName, string connectionString)
        {
            IDbConnection connection = sessionData.DbConnections[connectionName];

            return connection;
        }
 
        public static DataTable ExecuteQuery(string query, IDbConnection connection)
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                using (IDataReader reader = command.ExecuteReader())
                {
                    return Database.GetDataTable(reader);
                }
            }
        }
 
        public static int ExecuteNonQuery(string query, IDbConnection connection)
        {
            int rowsAffected = 0;
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                rowsAffected = command.ExecuteNonQuery();
            }
            return rowsAffected;
        }
    }
}
