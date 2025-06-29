using System;
using System.Data;
using System.Diagnostics;
using Metreos.ApplicationFramework;
using Metreos.Utilities;
using MySql.Data.MySqlClient;

namespace Metreos.Native.ActiveRelay
{
    /// <summary>
    /// Enumerations used to determine the state of a call. 
    /// See Remarks section.
    /// </summary>
    /// <remarks>
    /// Used by ActiveRelay to explicitly determine whether an operation
    /// about to be performed with regards to a call is still valid.
    /// For example, a call is answered while the 'Call Timeout' timer fires.
    /// The handler for the timer is queued up behind the MakeCall_Complete handler.
    /// MakeCall_Complete checks to see that the call had not already been hung up,
    /// and then sets the 'CONNECTED' state on the call. When the timeout timer handler
    /// executes, it sees that the call is connected and does not hang it up. This should
    /// probably make its way into the runtime at some point. If any of these names are changed,
    /// applications will have to be adjusted accordingly.
    /// </remarks>
    public enum CallState
    {
        NONE,
        DIAL_PENDING,
        RING_OUT,
        RING_IN,
        CONNECTED,
        ENDED
    }

    public class ActiveRelay
    {
        #region Database constants. 
        public const string ArDbConnectionName = "ActiveRelay";
        public const string ArDbConnectionString = "Server=localhost;User ID=root;Password=metreos;Database=ActiveRelay";
        public const string Id = "ar_active_calls_id";
        public const string RoutingGuid = "ar_routing_guid";
        public const string TableName = "ar_active_calls";
        public const string UserId = "as_user_id";
        public const string TimeStamp = "ar_call_timestamp";
        public const string FromNumber = "ar_call_from";
        public const string ToNumber = "ar_call_to";
        public const string WasSwapped = "ar_was_swapped";

        /// <summary>
        ///     All tables in Application Suite use primary keys seeded at 1
        /// </summary>
        public const int StandardPrimaryKeySeed = 1;
        #endregion

        // Methods
        public ActiveRelay()
        {
        }
 
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
