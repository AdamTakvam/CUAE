using System;
using System.Data;
using Metreos.Utilities;

namespace Metreos.ApplicationSuite.Storage
{
	/// <summary>
	/// Summary description for DbInteraction.
	/// </summary>
	public class DbInteraction
	{
		public DbInteraction()
		{}

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
	}
}
