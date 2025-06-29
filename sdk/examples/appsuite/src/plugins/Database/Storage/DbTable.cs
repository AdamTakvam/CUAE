using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Globalization;

using Metreos.LoggingFramework;

namespace Metreos.ApplicationSuite.Storage
{
    #region Read/Write API Enum/Structs
    public enum ReadResult
    {
        Success,
        DbFailure,
    }

    public enum WriteResult
    {
        Success,
        DbFailure,
        PublisherDown
    }

    public struct ReadResultContainer
    {
        public ReadResult result;
        public Exception e;
        public IDataReader reader;
        public object scalar;
    }

    public struct AdvancedReadResultContainer
    {
        public ReadResult result;
        public Exception e;
        public DataTable results;
    }


    public struct WriteResultContainer
    {
        public WriteResult result;
        public Exception e;
        public uint lastInsertId;
        public int rowsAffected;
    }
    #endregion

	/// <summary>
	///     Defines the base class of all table-based database interaction classes.
	/// </summary>
	public abstract class DbTable : IDisposable
	{
        /// <summary> 
        ///     Connection is private because derived classes do not need to replicate pub/sub writing
        ///     logic all over the place.  Changing this allows for possibility of future code 
        ///     to not perserve pub/sub understanding in every db action.
        /// </summary>
        private IDbConnection connection;
        protected LogWriter log;
        protected string applicationName;
        protected string partitionName;
        protected bool connectionOpened;
        protected bool allowWrite;
        public const TraceLevel PublisherDown = TraceLevel.Warning;

        public DbTable(DbTable table) 
            : this(table.connection, table.log, table.applicationName, table.partitionName, table.allowWrite) { }

        public DbTable(IDbConnection connection, LogWriter log, string applicationName, string partitionName, bool allowWrite)
        {
            Debug.Assert(connection != null, "A null connection is not allowed in the Application Suite database utility");
            Debug.Assert(log != null, "A null logger is not allowed in the Application Suite database utility");
            Debug.Assert(applicationName != null && applicationName.Length != 0, "A null or empty application name is not allowed in the Application Suite database utility");
            Debug.Assert(partitionName != null && partitionName.Length != 0, "A null or empty partition name is not allowed in the Application Sutide database utility");

            this.log                = log;
            this.connection         = connection;
            this.applicationName    = applicationName;
            this.partitionName      = partitionName;
            this.connectionOpened   = true;
            this.allowWrite         = allowWrite;

            PrepareDbConnection();
        }	

        protected virtual void PrepareDbConnection()
        {
            try
            {
                switch (connection.State)
                {
                    case ConnectionState.Broken : connection.Close(); connection.Open(); break;
                    case ConnectionState.Closed : connection.Open(); break;
                    default                     : this.connectionOpened = false; break;
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, 
                    "Unable to open database connection in the application suite database utility.  Exception: " +
                    Metreos.Utilities.Exceptions.FormatException(e));
            }
        }

        protected ReadResultContainer ExecuteScalar(Metreos.Utilities.SqlBuilder builder)
        {
            return ExecuteScalar(builder.ToString());
        }

        protected ReadResultContainer ExecuteScalar(string scalar)
        {
            ReadResultContainer scalarResult = new ReadResultContainer();
            try
            {
                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = scalar;
                    scalarResult.scalar = command.ExecuteScalar();
                }
            }
            catch(Exception e)
            {
                scalarResult.result = ReadResult.DbFailure;
                scalarResult.e = e;
            }

            return scalarResult;
        }

        protected AdvancedReadResultContainer ExecuteEasyQuery(Metreos.Utilities.SqlBuilder builder)
        {
            return ExecuteEasyQuery(builder.ToString());
        }

        protected AdvancedReadResultContainer ExecuteEasyQuery(string select)
        {
            AdvancedReadResultContainer queryResult = new AdvancedReadResultContainer();

            try
            {
                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = select;
                    using(IDataReader reader = command.ExecuteReader())
                    {
                        queryResult.results = Metreos.Utilities.Database.GetDataTable(reader);
                        queryResult.result = ReadResult.Success;
                    }
                }
            }
            catch(Exception e)
            {
                queryResult.result = ReadResult.DbFailure;
                queryResult.e = e;
            }
            
            return queryResult;
        }

        protected ReadResultContainer ExecuteQuery(Metreos.Utilities.SqlBuilder builder)
        {
            return ExecuteQuery(builder.ToString());
        }

        protected ReadResultContainer ExecuteQuery(string select)
        {
            ReadResultContainer queryResult = new ReadResultContainer();

            try
            {
                using(IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = select;
                    queryResult.reader = command.ExecuteReader();
                    queryResult.result = ReadResult.Success;
                }
            }
            catch(Exception e)
            {
                queryResult.result = ReadResult.DbFailure;
                queryResult.e = e;
            }
            
            return queryResult;
        }

        protected WriteResultContainer ExecuteCommand(Metreos.Utilities.SqlBuilder builder)
        {
            return ExecuteCommand(builder.ToString(), builder.method == Metreos.Utilities.SqlBuilder.Method.INSERT);
        }

        protected WriteResultContainer ExecuteCommand(string command)
        {
            return ExecuteCommand(command, command.StartsWith("INSERT"));
        }

        protected WriteResultContainer ExecuteCommand(string commandText, bool isInsert)
        {
            WriteResultContainer commandResult = new WriteResultContainer();

            if(allowWrite)
            {
                try
                {
                    using(IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = commandText;
                        commandResult.rowsAffected = command.ExecuteNonQuery();

                        if(isInsert)
                        {
                            commandResult.lastInsertId = GetLastAutoId(command);
                        }
                    }
                }
                catch(Exception e)
                {
                    commandResult.e = e;
                    commandResult.result = WriteResult.DbFailure;
                }
            }
            else
            {
                commandResult.result = WriteResult.PublisherDown;
            }

            return commandResult;
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
        protected uint GetLastAutoId(IDbCommand command)
        {
            return GetLastAutoId(command, 0);
        }

        protected uint GetLastAutoId(IDbCommand command, uint defaultValue)
        {
            try
            {
                command.CommandText = SqlConstants.GetLastInsertId;
                return Convert.ToUInt32(command.ExecuteScalar());
            }
            catch
            {
                return defaultValue;
            }
        }

        public static bool DetermineAllowWrite(Hashtable sessionData)
        {
            bool allowWrite = true;
            if(sessionData.Contains(SqlConstants.AllowDBWriteName))
            {
                allowWrite = (bool) sessionData[SqlConstants.AllowDBWriteName];
            }

            // A debug assert almost makes sense here, but when writing this,
            // we did not have the time to update all applications
            return allowWrite;
        }

        public static string PublisherIsDownMessage(string methodName)
        {
            return "Publisher is down in attempting to execute " + methodName;
        }
        #region IDisposable Members

        public virtual void Dispose()
        {
            if(connection != null && connectionOpened == true)
            {
                try
                {
                    connection.Close();
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, 
                        "Unable to close database connection in disposing application database utility.  Exception: " + 
                        Metreos.Utilities.Exceptions.FormatException(e));
                }
            }
        }

        #endregion
    }
}
