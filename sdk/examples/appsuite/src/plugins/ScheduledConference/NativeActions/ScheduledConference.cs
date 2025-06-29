using System;
using System.Data;
using System.Diagnostics;
using Metreos.ApplicationFramework;
using Metreos.Utilities;
using MySql.Data.MySqlClient;

namespace Metreos.Native.ScheduledConference
{
    public class ScheduledConference
    {
        #region Database Constants
        public const string ScDbConnectionName = "ScheduledConference";
        public const string ScDbConnectionString = "Server=localhost;User ID=root;Password=metreos;Database=ScheduledConference";
        public const string Id = "sc_participants_id";
        public const string ConferencePin = "sc_participants_conference_pin";
        public const string TableName = "sc_participants";
        public const string ParticipantCount = "sc_participants_count";

        public class ScheduledConferencesFiles    
        {
            public const string TableName           = "sc_files";
            public const string Id                  = "sc_files_id";
            public const string Pin                 = "pinId";
            public const string Time                = "time";
            public const string Type                = "type";
            public const string Filename            = "filename";
        }

        public const string GetLastInsertId = "SELECT LAST_INSERT_ID()";

        /// <summary>
        ///     All tables in Application Suite use primary keys seeded at 1
        /// </summary>
        public const int StandardPrimaryKeySeed = 1;

        public const string CountUpdateString = "UPDATE sc_participants SET sc_participants_count=sc_participants_count{0}1 WHERE sc_participants_conference_pin={1}";
        #endregion

        #region ReturnValues
        public enum ReturnValues
        {
            success,
            failure,
            NoSuchPin,
            @true,
            @false
        }
        #endregion
        
        // Methods
        public ScheduledConference()
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

        /// <summary>
        ///     Gets the last auto_increment id that MySql
        /// </summary>
        /// <remarks>
        ///     MySql specific!
        /// </remarks>
        /// <param name="command">
        ///     The command to execute
        /// </param>
        /// <returns>
        ///     The integer key, defaulting to <code>SqlConstants.DefaultOutIntegerValue</code>
        /// </returns>
        public static uint GetLastAutoId(IDbCommand command)
        {
            return GetLastAutoId(command, 0);
        }

        public static uint GetLastAutoId(IDbCommand command, uint defaultValue)
        {
            try
            {
                command.CommandText = GetLastInsertId;
                return Convert.ToUInt32(command.ExecuteScalar());
            }
            catch
            {
                return defaultValue;
            }
        }

    }
}
