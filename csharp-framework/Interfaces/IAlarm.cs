using System;
using System.Diagnostics;
using Metreos.Interfaces;

namespace Metreos.Interfaces
{
	/// <summary>
	/// Alarm service interfaces.
	/// </summary>
	public abstract class IAlarm
	{
		public const int ALARM_NEW_EVENT			= 1000;		// cell sends a new event to agent
		public const int ALARM_CLEAR_EVENT			= 1001;		// cell asks agent to clear event
		public const int ALARM_EVENT_CLEARED		= 1002;		// agent tells cell that event has been cleared
		public const int ALARM_EVENT_ACCEPTED		= 1003;		// agent tells cell that event has been accepted

		public const int ALARM_ERROR_CODE			= 2001;		// error code
		public const int ALARM_SEVERITY				= 2002;		// severity
		public const int ALARM_DESCRIPTION			= 2003;		// description	
		public const int ALARM_TIMESTAMP			= 2004;		// time stamp
		public const int ALARM_GUID					= 2005;		// guid
        public const int ALARM_CLEAR_TIMESTAMP      = 2006;     // time stamp when alarm cleared

		public enum STATUS
		{
			UNSPECIFIED		= 0,			// UNKNOWN
			TRIGGERED		= 1,			// EVENT HAS BEEN SENT FROM CELL TO AGENT
			CONFIRMED		= 2,			// AGENT CONFIRMED ADDED IT INTO DB
			CLEARED			= 3,			// EVENT HAS BEEN RESOLVED
			REMOVED			= 4				// EVENT HAS BEEN REMOVED FROM DB
		}

		public abstract class DEFAULT_VALUES
		{
			// DB
			public const string DEFAULT_DATABASENAME         = "MCE";
			public const string DEFAULT_DATABASEHOSTNAME     = "localhost";
			public const ushort DEFAULT_DATABASEPORT         = 3306;
			public const string DEFAULT_DATABASEUSERNAME     = "root";
			public const string DEFAULT_DATABASEPASSWORD     = "metreos";

			// EVENT UDP SOCKET
			public const string DEFAULT_EVENTSERVER			= "localhost";
			public const ushort DEFAULT_EVENTPORT			= 9200;

			// BROADCAST UDP SOCKET
			public const string DEFAULT_BROADCASTIP			= "224.5.6.7";
			public const ushort DEFAULT_BROADCASTPORT		= 9300;

			// DUMMY VALUES IF NO SMTP GROUP
			public const string DEFAULT_RECIPIENT			= "recipient";
			public const string DEFAULT_SENDER				= "sender";
			public const string DEFAULT_SERVER				= "server";
			public const string DEFAULT_USERNAME			= "username";
			public const string DEFAULT_PASSWORD			= "password";
			public const ushort DEFAULT_PORT				= 25;
			public const IConfig.Severity DEFAULT_SMTPLEVEL	= IConfig.Severity.Unspecified;

			// DUMMY VALUES IF NO SNMP GROUP
			public const string DEFAULT_MANAGERIP			= "localhost";
			public const IConfig.Severity DEFAULT_SNMPLEVEL	= IConfig.Severity.Unspecified;
		}
	}
}
