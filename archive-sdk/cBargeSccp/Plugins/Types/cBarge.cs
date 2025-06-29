using System;
using System.Data;
using System.Diagnostics;
using Metreos.ApplicationFramework;
using Metreos.Utilities;
using MySql.Data.MySqlClient;

namespace Metreos.Applications.cBarge
{
	#region Database constants
	public abstract class Const
	{
		public const string CbDbConnectionName = "cBarge";
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
		public const string DirectoryNumber = "directory_number";
        public const string Sid = "sid";
        public const string CallReference = "call_reference";
        public const string CallInstance = "call_instance";
        public const string MmsId = "mms_id";
        public const string ConferenceId = "conference_id";
        public const string LineInstance = "line_instance";
		public const string TimeStamp = "cb_timestamp";
        public const string DnLi = "dn_li";
        public const string RoutingGuid = "routing_guid";
        public const string BargeRoutingGuid = "barge_routing_guid";
	}

    public abstract class CBargeRecords
    {
        // Conferences table constants. 
        public const string TableName = "cbarge_records";   
        public const string Id = "cbarge_id";
		public const string DirectoryNumber = "directory_number";
        public const string LineInstance = "line_instance";
        public const string MmsId = "mms_id";
        public const string ConferenceId = "conference_id";
        public const string NumberParticipants = "num_participants";
        public const string DnLi = "dn_li";
        public const string TimeStamp = "cb_timestamp";

        public const int DefaultParticipants = 0;
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
