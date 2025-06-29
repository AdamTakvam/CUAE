using System;
using System.Data;
using System.Diagnostics;
using Metreos.Utilities;
using Metreos.LoggingFramework;
namespace Metreos.Native.ClickToTalk
{
	/// <summary>
	/// Constants for interfacing with the Database actions
	/// </summary>
	public abstract class IDatabase
	{
        // Parameters
        public const string NAME        = "Name";
        public const string ADDRESS     = "Address";
        public const string CONF_ID     = "ConferencesID";
        public const string HOST        = "HostIP";
        public const string HOST_DESC   = "HostDescription";
        public const string HOST_USER   = "HostUsername";
        public const string HOST_PASS   = "HostPassword";
        public const string RECORD      = "Record";
        public const string RECORD_END  = "RecordEnded";
        public const string RECORD_ID   = "RecordConnectionId";
        public const string EMAIL       = "Email";
        public const string ID          = "ID";
        public const string ERROR       = "Error";
        public const string TIMESTAMP   = "TimeStamp";

        // Internal definitions
        internal const string APP_NAME      = "ClickToTalk";
        internal const string APP_VERSION   = "1.0";
        internal const string DB_NAME       = "ClickToTalk";

        internal const string CALLEE_TABLE  = "Callees";
        internal const string CONF_TABLE    = "Conferences";
        internal const string ERRORS_TABLE  = "Errors";

        public const string GetLastInsertId =
            "SELECT LAST_INSERT_ID()";

        public static string FormatHostIP(string host)
        {
            if(host == null) { return null; }

            // Make host all pretty and stuff *hehe*
            string[] hostBits = host.Split(':');

            return hostBits == null ? null : hostBits[0];
        }

        public static void Close(IDbConnection connection, bool notPreviouslyOpen, LogWriter log)
        {
            try
            {
                if(notPreviouslyOpen)
                {
                    connection.Close();
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Could not close database connection. " + Exceptions.FormatException(e));
            }
        }

        public static bool Open(IDbConnection connection)
        {
            if(connection.State == ConnectionState.Open)
            {
                return false;
            }
            else
            {
                connection.Open();
                return true;
            }
        }
	}
}
