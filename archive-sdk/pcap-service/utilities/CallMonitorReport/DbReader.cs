using System;
using System.Data;
using System.Diagnostics;
using System.Collections;

using MySql.Data.MySqlClient;

using Metreos.Utilities;


namespace CallMonitorReport
{
	/// <summary>
	/// Helper class for database access
	/// </summary>
	public class DbReader
	{
		private IDbConnection db;               // db connection
		private string databaseHostname;        // db hostname
		private string databaseName;            // db name
		private string databaseUsername;        // db user name
		private string databasePassword;        // db password
		private ushort databasePort;            // db port

        private const string START_TIME_OF_DAY = "00:00:00";
        private const string END_TIME_OF_DAY = "23:59:59";

		public DbReader(string databaseHostname,
						string databaseName,
						string databaseUsername,
						string databasePassword,
						ushort databasePort)
		{
			this.databaseHostname = databaseHostname;
			this.databaseName = databaseName;
			this.databaseUsername = databaseUsername;
			this.databasePassword = databasePassword;
			this.databasePort = databasePort;

			db = DatabaseConnect(databaseName);
		}

		/// <summary>
		/// Destructor
		/// </summary>
		~DbReader()
		{
			CloseDatabase();
		}

        public bool IsDbConnected()
        {
            return db != null ? true : false;
        }

		/// <summary>
		/// Close database connection
		/// </summary>
		public void CloseDatabase()
		{
			if(db != null)
			{
				db.Close();
				db = null;
			}
		}

		/// <summary>
		/// Connects to the specified database.
		/// </summary>
		/// <param name="name">Database name</param>
		/// <returns>MySQL connection</returns>
		public IDbConnection DatabaseConnect(string name)
		{
			if (name == null) { return null; }

			string dsn = Database.FormatDSN(name, databaseHostname, databasePort, databaseUsername, databasePassword, true);
			IDbConnection newDb = new MySqlConnection(dsn);
            
            // Open() throws an exception on failure.
            try
            {
                newDb.Open();
            }
            catch
            {
                newDb = null;
            }

			return newDb;
		}

        public DataTable GetMonitoredCallsByTimeInterval(string fromDate, string toDate)
        {
            string sqlQuery = "SELECT `mc_did_number`, `mc_government_agent_number`, `mc_customer_number`, `mc_insurance_agent_number`, `mc_monitored_sid`, `mc_start_monitor_timestamp`" +
                                " FROM `monitored_calls` WHERE `mc_start_monitor_timestamp` BETWEEN " +
                                "'" + AdjustDateTime(fromDate, START_TIME_OF_DAY) + "' AND '" + AdjustDateTime(toDate, END_TIME_OF_DAY) + "' ORDER BY `mc_start_monitor_timestamp`"; 

            DataTable data = ExecuteQuery(sqlQuery);

            return data;
        }

        public ArrayList GetDIDsByTimeInterval(string fromDate, string toDate)
        {
            string sqlQuery = "SELECT DISTINCT `mc_did_number`" +
                " FROM `monitored_calls` WHERE `mc_start_monitor_timestamp` BETWEEN " +
                "'" + AdjustDateTime(fromDate, START_TIME_OF_DAY) + "' AND '" + AdjustDateTime(toDate, END_TIME_OF_DAY) + "' ORDER BY `mc_did_number`"; 

            DataTable data = ExecuteQuery(sqlQuery);

            if (data == null || data.Rows.Count == 0)
                return null;

            ArrayList DIDs = new ArrayList();
            for (int i=0; i<data.Rows.Count; i++)
            {
                string s = data.Rows[i]["mc_did_number"].ToString().Trim();
                if (s.Length > 0)
                    DIDs.Add(s);
            }

            return DIDs;
        }

        public int GetNumCallsByDID(string did, string fromDate, string toDate)
        {
            int numCalls = 0;

            string sqlQuery = "SELECT COUNT(`mc_did_number`)" +
                " FROM `monitored_calls` WHERE `mc_did_number` = '" + did + "' AND `mc_start_monitor_timestamp` BETWEEN " +
                "'" + AdjustDateTime(fromDate, START_TIME_OF_DAY) + "' AND '" + AdjustDateTime(toDate, END_TIME_OF_DAY) + "'"; 

            DataTable data = ExecuteQuery(sqlQuery);

            if (data == null || data.Rows.Count == 0)
                return 0;

            numCalls = Convert.ToInt32(data.Rows[0][0].ToString());

            return numCalls;
        }

        public string GetFirstLogDate()
        {
            string sqlQuery = "SELECT `mc_start_monitor_timestamp`" +
                " FROM `monitored_calls` ORDER BY `mc_start_monitor_timestamp`";

            DataTable data = ExecuteQuery(sqlQuery);

            if (data == null || data.Rows.Count == 0)
                return null;

            DateTime d = Convert.ToDateTime(data.Rows[0][0].ToString());
            return d.ToString("yyyy-MM-dd");
        }

        private DataTable ExecuteQuery(string sqlQuery)
        {
            DataTable data = null;
            lock(db)
            {
                try
                {
                    using(IDbCommand command = db.CreateCommand())
                    {
                        command.CommandText = sqlQuery;

                        using(IDataReader reader = command.ExecuteReader())
                        {
                            data = Database.GetDataTable(reader);
                        }
                    }
                }
                catch
                {
                    return null;
                }
            }

            return data;
        }

        private string AdjustDateTime(string ds, string time)
        {
            DateTime d = DateTime.Parse(ds);            
            return d.ToString("yyyy-MM-dd") + " " + time;
        }
	}
}
