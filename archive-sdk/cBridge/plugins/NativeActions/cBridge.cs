using System;
using System.Data;
using System.Diagnostics;
using Metreos.ApplicationFramework;
using Metreos.Utilities;
using MySql.Data.MySqlClient;

namespace Metreos.Native.CBridge
{
	#region Database constants
	public abstract class Const
	{
		public const string CbDbConnectionName = "cbridge";
		public const string CbDbConnectionString = "Server=localhost;User ID=root;Password=metreos;Database=cbridge";
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

	public abstract class CBridgeTable
	{
		// Conferences table constants. 
		public const string TableName = "cbridge_conferences";
		public const string Id = "cbridge_id";
		public const string LineId = "line_id";
		public const string TimeStamp = "cb_timestamp";
        public const string RoutingGuid = "routingGuid";
		public const string IsRecorded  = "is_recorded";
	}

	public abstract class CBridgeParticipantsTable
	{
		// Participants table constants
		public const string TableName = "cbridge_conference_participants";
		public const string Id = "cbridge_participants_id";
		public const string LineId = "line_id";
		public const string FromNumber = "from_number";
		public const string CallId = "callId";
		public const string TimeStamp = "timestamp";
		public const string IsRecorded  = "is_recorded";
		public const string IsModerator  = "is_moderator";
		public const string IsMuted  = "is_muted";
	}
	#endregion

	public class DbInteraction
	{
		/// <summary>
        /// Returns the IDbConnection associated with the passed in connection name. The connection is retrieved from the
        /// given SessionData object. If the connection does not exist, it will be established. 
        /// </summary>
        public static IDbConnection GetConnection(SessionData sessionData, string connectionName, string connectionString)
        {
            IDbConnection connection = sessionData.DbConnections[connectionName];
            /*
			if (connection == null)
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                sessionData.DbConnections.Add(connectionName, connection);
                return connection;
            }
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
			*/
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
