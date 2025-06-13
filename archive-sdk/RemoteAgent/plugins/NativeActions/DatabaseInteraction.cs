using System;
using System.Data;
using Metreos.ApplicationFramework;
using Metreos.Utilities;
using Metreos.LoggingFramework;

namespace Metreos.Native.RemoteAgent
{
    /// <summary>
    /// This class facilitates interaction between the native actions and the database.
    /// </summary>
    public abstract class DatabaseInteraction
    {
        /// <summary>
        /// This class contains a number of constants related to the RemoteAgent database table.
        /// </summary>
        public abstract class Constants
        {
            public const string TableName       = "agent_records";
            public const string LastInsertId    = "SELECT LAST_INSERT_ID()";
            public const string ConnectionName  = "RemoteAgent";
            public const string Id              = "agent_records_id";
            public const string AgentDN         = "agent_dn";
            public const string RoutingGuid     = "routing_guid";
            public const string IsRecorded      = "is_recorded";
        }

        public DatabaseInteraction()
        {}

        /// <summary>
        /// Returns the IDbConnection associated with the passed in connection name. The connection is retrieved from the
        /// given SessionData object. If the connection does not exist, 'null' is returned
        /// </summary>
        /// <param name="sessionData">SessionData object from which to pull out the connection</param>
        /// <param name="connectionName">name of the connection we want</param>
        /// <returns></returns>
        public static IDbConnection GetConnection(SessionData sessionData, string connectionName)
        {
            IDbConnection connection = sessionData.DbConnections[connectionName];
            return connection;
        }
 
        /// <summary>
        /// Executes the specified query (ie, 'SELECT') string on the specified connection
        /// </summary>
        /// <param name="query">SQL query to execute</param>
        /// <param name="connection">Database connection object</param>
        /// <returns>A System.Data.DataTable that contains the result of the query</returns>
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

        /// <summary>
        /// Executes the specified query on the specified connection, and returns the element in the first column of the first row.
        /// </summary>
        /// <param name="query">SQL query to execute</param>
        /// <param name="connection">Database connection object</param>
        /// <returns>The element in the first column of the first row</returns>
        public static object ExecuteScalar(string query, IDbConnection connection)
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                return command.ExecuteScalar();
            }
        }
 
        /// <summary>
        /// Executes the specified non-query (ie, 'UPDATE') string on the specified connection
        /// </summary>
        /// <param name="query">SQL command to execute</param>
        /// <param name="connection">Database connection object</param>
        /// <returns>The number of rows affected by the command</returns>
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

        public static bool WriteAgentRecord(LogWriter log, SessionData sessionData, string agentDN, string routingGuid, bool isRecorded, out uint agentRecordId)
        {
            agentRecordId = 0;
            if (agentDN == string.Empty || agentDN == null)
                return false;
            if (routingGuid == string.Empty || routingGuid == null)
                return false;
            
            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.INSERT, Constants.TableName);
            builder.AddFieldValue(Constants.AgentDN, agentDN);
            builder.AddFieldValue(Constants.RoutingGuid, routingGuid);
            builder.AddFieldValue(Constants.IsRecorded, isRecorded);

            IDbConnection conn = null;
            
            try
            {
                conn = GetConnection(sessionData, Constants.ConnectionName);

                conn.Open();
                int rowsAffected = ExecuteNonQuery(builder.ToString(), conn);
                
                agentRecordId = Convert.ToUInt32(ExecuteScalar(Constants.LastInsertId, conn));
                conn.Close();

                return rowsAffected > 0;
            }
            catch (Exception e)
            {
                log.Write(System.Diagnostics.TraceLevel.Verbose, "An exception occured in the WriteAgentRecord method. Exception message is: " + e.Message);
                if (conn != null)
                    conn.Close();
                return false;
            }
        }

        public static bool UpdateAgentRecord(LogWriter log, SessionData sessionData, uint agentRecordId, bool isRecorded)
        {
            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.UPDATE, Constants.TableName);
            builder.where[Constants.Id] = agentRecordId;
            builder.AddFieldValue(Constants.IsRecorded, isRecorded);
            
            IDbConnection conn = null;
            int rowsAffected = 0;

            try 
            {
                conn = GetConnection(sessionData, Constants.ConnectionName);
                conn.Open();

                rowsAffected = ExecuteNonQuery(builder.ToString(), conn);
            }
            catch (Exception e)
            {
                log.Write(System.Diagnostics.TraceLevel.Verbose, "An exception occured in the UpdateAgentRecord method. Exception message is: " + e.Message);
            }

            if (conn != null)
                conn.Close();

            return rowsAffected > 0;
        }


        public static bool RemoveAgentRecord(LogWriter log, SessionData sessionData, uint agentRecordId)
        {
            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.DELETE, Constants.TableName);
            builder.where[Constants.Id] = agentRecordId;
            IDbConnection conn = null;
            
            try
            {
                conn = GetConnection(sessionData, Constants.ConnectionName);

                conn.Open();
                int rowsAffected = ExecuteNonQuery(builder.ToString(), conn);
                conn.Close();

                return rowsAffected > 0 ? true : false;
            }
            catch (Exception e)
            {
                log.Write(System.Diagnostics.TraceLevel.Verbose, "An exception occured in the RemoveAgentRecord method. Exception message is: " + e.Message);
                if (conn != null)
                    conn.Close();
                return false;
            }
        }

        public static bool RetrieveAgentRecord(LogWriter log, SessionData sessionData, string agentDN, out uint agentRecordId, out string routingGuid, out bool isRecorded)
        {
            agentRecordId = 0;
            isRecorded = false;
            routingGuid = string.Empty;

            if (agentDN == string.Empty || agentDN == null)
                return false;
            
            SqlBuilder builder = new SqlBuilder(SqlBuilder.Method.SELECT, Constants.TableName);
            builder.fieldNames.Add(Constants.Id);
            builder.fieldNames.Add(Constants.RoutingGuid);
            builder.fieldNames.Add(Constants.IsRecorded);
            builder.where[Constants.AgentDN] = agentDN;
            IDbConnection conn = null;

            try
            {
                conn = GetConnection(sessionData, Constants.ConnectionName);

                conn.Open();
                DataTable table = ExecuteQuery(builder.ToString(), conn);
                if (table.Rows.Count == 0)
                    return false;

                DataRow row = table.Rows[0];
                agentRecordId = Convert.ToUInt32(row[Constants.Id]);
                routingGuid = row[Constants.RoutingGuid] as string;
                isRecorded = Convert.ToBoolean(row[Constants.IsRecorded]);
                conn.Close();

                if (routingGuid == null || routingGuid == string.Empty)
                {
                    // SMA-476, booo
                    routingGuid = string.Empty;
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                log.Write(System.Diagnostics.TraceLevel.Verbose, "An exception occured in the RetrieveAgentRecord method. Exception message is: " + e.Message);
                if (conn != null)
                    conn.Close();
                return false;
            }        
        }
    }
}
