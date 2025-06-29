using System;
using System.Data;
using System.Diagnostics;

using MySql.Data.MySqlClient;
using Metreos.DatabaseScraper.Common;
using Metreos.Core;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.PackageXml;
using Metreos.ApplicationFramework.Collections;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Interfaces;
using Metreos.Utilities;

namespace NativeActions
{
	/// <summary> 
	///     Query the database without communicating with the DatabaseScraper provider 
	/// </summary>
    /// <remarks>
    ///     This action is implemented here, as well as in the native actions project, to contrast
    ///     the differences between a provider and native action 
    /// </remarks>
	[PackageDecl("Metreos.Native.DatabaseScraper")]
	public class Query : INativeAction
	{
        #region Constants
        // ------------------
        // Database Settings
        // ------------------
        // For the purpose of this example provider, we will use static database settings.
        public const string DatabaseName        = "legacydb";
        public const string DatabaseAddress     = "localhost";
        public const int    DatabasePort        = 3306;
        public const string DatabaseUsername    = "root";
        public const string DatabasePassword    = "metreos";
        public const string ErrorsTable         = "errors";
        public const string TimeColumn          = "time";
        public const string DescriptionColumn   = "description";
        #endregion

        [ActionParamField("The earliest time to search for errors", false)]
        public DateTime StartTime { set { startTime = value; } }
        private DateTime startTime;

        [ActionParamField("The latest time to search for errors", false)]
        public DateTime EndTime { set { endTime = value; } }
        private DateTime endTime;

        public LogWriter Log { set { log = value; } }
        private LogWriter log;

        [ResultDataField("Errors that fall in the specified range")]
        public ErrorDataCollection Data { get { return errors; } }
        private ErrorDataCollection errors;
		
         public Query()
		{
			Clear();
		}

        /// <summary>
        ///     Since a native action is reused throughout the life of a script,
        ///     we use Clear to clear the native action back to its starting state
        /// </summary>
        public void Clear()
        {
            // If the user does not specify a start or end time, then these defaults
            // we be still in effect once the Execute action begins. 
            startTime = DateTime.MinValue;
            endTime = DateTime.MaxValue;
            errors = new ErrorDataCollection();
        }

        public bool ValidateInput()
        {
            // Check for wrong inputs
            if(startTime > endTime)
            {
                log.Write(TraceLevel.Error, "The Query action requires that the start time be after the end time.");
                return false;
            }

            return true;
        }

        //[Action(Name,  AllowCustomParams, DisplayName, Description)]
        [Action("Query", false, "Query", "Queries the specified time frame for errors")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            bool success = true;

            string dsn = Database.FormatDSN(
                DatabaseName, 
                DatabaseAddress, 
                DatabasePort, 
                DatabaseUsername, 
                DatabasePassword, 
                true);

            try
            {
                using(IDbConnection connection = new MySqlConnection(dsn))
                {
                    connection.Open();

                    using(IDbCommand command = connection.CreateCommand())
                    {                        
                        // Select all rows that fall between the start and end range
                        command.CommandText = String.Format(
                            "SELECT {0}, {1} FROM {2} " + 
                            "WHERE {1} > STR_TO_DATE('{3}', GET_FORMAT(DATETIME, 'USA')) " + 
                            "AND {1} < STR_TO_DATE('{4}', GET_FORMAT(DATETIME, 'USA'))",
                            DescriptionColumn,
                            TimeColumn,
                            ErrorsTable,
                            startTime.ToString("yyyy-MM-dd HH.mm.ss"),
                            endTime.ToString("yyyy-MM-dd HH.mm.ss"));

                        log.Write(TraceLevel.Info, command.CommandText);

                        using(IDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                string description = reader[DescriptionColumn] as string;
                                DateTime time = Convert.ToDateTime(reader[TimeColumn]);
                                errors.Add(new ErrorData(description, time));
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Unable to connect to the database.  Full exception is: " +
                    Metreos.Utilities.Exceptions.FormatException(e));

                success = false;
            }

            if(success)
            {
                return IApp.VALUE_SUCCESS;
            }
            else
            {
                return IApp.VALUE_FAILURE;
            }
        }
	}
}
