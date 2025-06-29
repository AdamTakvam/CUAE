using System;
using System.IO;
using System.Net;
using System.Data;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Xml.Serialization;
using System.Configuration;

using Metreos.Core;
using Metreos.Core.ConfigData;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;

namespace Metreos.Configuration
{
    /// <summary>
    /// Maintains configuration data for the application server.
    /// Config is a singleton, and an instance can only be retrieved
    /// using the static Instance property.
    /// </summary>
	public class Config : MarshalByRefObject, IConfigUtility
	{
        #region Constants

        private abstract class Consts
        {
            public const uint StdConfigMetaIdMax    = 125;
            public const uint StdFormatIdMax        = 100;
        }

        #endregion

        #region Failover state info
        private enum FailFilter : ushort
        {
            Invalid = 0,
            None    = 1,
            Local   = 2,
            Parent  = 3,
            Both    = 4
        }

        private const string ReplicationDatabaseName = "mce_standby";

        /// <summary>Lookup table: parent state, standby state -> FailFilter</summary>
        private static ushort[,] failStateTable;

        /// <summary>Our status as a cluster parent</summary>
        public static IConfig.FailoverStatus ParentFailoverStatus
        {
            get { return parentfailoverStatus;}
            set { parentfailoverStatus = value; }
        }
        private static IConfig.FailoverStatus parentfailoverStatus = IConfig.FailoverStatus.Normal;

        /// <summary>Our status as a cluster standby</summary>
        public static IConfig.FailoverStatus StandbyFailoverStatus
        {
            get { return standbyfailoverStatus;}
            set { standbyfailoverStatus = value; }
        }
        private static IConfig.FailoverStatus standbyfailoverStatus = IConfig.FailoverStatus.Normal;
        #endregion

        #region Static Constructor

        static Config()
        {
            BuildFailStateTable();
        }
        #endregion

		#region Singleton interface

        private LogWriter log;
        private static object dbConnectLock = new object();

		private static volatile Config instance = null;
		private static Object newInstanceSync = new Object();

		public static Config Instance
		{
			get
			{
				lock(newInstanceSync)                           // Grab the instance lock
				{
					if(instance == null)                        // Has it already been created?
					{                                           // Create the singleton instance
						instance = new Config();
					}
				}

				return instance;
			}
		}

		private Config()
		{
			log = new LogWriter(TraceLevel.Verbose, typeof(Config).Name);
		}

		#endregion

        #region Test

        /// <summary>Verifies DB connectivity</summary>
        /// <returns>Whether or not a DB connection could be made</returns>
        public bool Test()
        {
            IDbConnection db = OpenAppServerDb();
            if(db == null)
                return false;

            db.Dispose();
            return true;
        }
        #endregion

        #region Exception tracking

        /// <summary>Last exception thrown from a child AppDomain (provider or application)</summary>
        /// <remarks>
        /// The purpose of this is to overcome an immensely stupid decision made by Microsoft
        /// in .NET 2.0. By saving the exception here, we can safely ignore it when it gets
        /// rethrown in the default AppDomain. With the help of the "legacyUnhandledExceptionPolicy"
        /// flag in app.config, we can make this thing act in a sane manner.
        /// </remarks>
        public Exception LastChildDomainException
        {
            get { return lastChildDomainException; }
            set { lastChildDomainException = value; }
        }
        private Exception lastChildDomainException;
        #endregion

		#region Config Value Property Accessors

		// Stuff we can find logically
		public static string RootPath { get { return AppDomain.CurrentDomain.BaseDirectory; } }
		public static DirectoryInfo ApplicationDir { get { return GetDirectoryInfo(IConfig.AppServerDirectoryNames.APPS); } }
		public static DirectoryInfo AppDeployDir { get { return GetDirectoryInfo(IConfig.AppServerDirectoryNames.DEPLOY); } }
        public static DirectoryInfo CacheDir { get { return GetDirectoryInfo(IConfig.AppServerDirectoryNames.CACHE); } }
		public static DirectoryInfo CoreLibDir { get { return GetDirectoryInfo(IConfig.AppServerDirectoryNames.LIBS); } }
		public static DirectoryInfo TmScriptsDir { get { return GetDirectoryInfo(IConfig.AppServerDirectoryNames.TM_SCRIPTS); } }
		public static DirectoryInfo NativeActionsDir { get { return GetDirectoryInfo(IConfig.FwDirectoryNames.ACTIONS); } }
		public static DirectoryInfo NativeTypesDir { get { return GetDirectoryInfo(IConfig.FwDirectoryNames.TYPES); } }
		public static DirectoryInfo ProviderDir { get { return GetDirectoryInfo(IConfig.AppServerDirectoryNames.PROVIDERS); } }

		// Config file settings
		public string DatabaseName { get { return AppConfig.GetEntry(IConfig.ConfigFileSettings.DB_NAME); } }
        public string DatabaseHost { get { return AppConfig.GetEntry(IConfig.ConfigFileSettings.DB_HOST); } }
		public ushort DatabasePort { get { return GetDatabasePort(); } }
        public string DatabaseUsername { get { return AppConfig.GetEntry(IConfig.ConfigFileSettings.DB_USERNAME); } }
        public string DatabasePassword { get { return AppConfig.GetEntry(IConfig.ConfigFileSettings.DB_PASSWORD); } }
		public DirectoryInfo FrameworkDir { get { return FindFrameworkDirectory(); } }
        public DirectoryInfo FrameworkVersionDir { get { return FindFrameworkVersionDirectory(); } }
		public string MessageQueueProvider { get { return IConfig.ConfigFileSettings.DefaultValues.MQ_PROVIDER; } }
		
        public bool DeveloperMode
        {
            get
            {
                String s = AppConfig.GetEntry(IConfig.ConfigFileSettings.DEVELOPER_MODE);
                return Boolean.Parse(s != null ? s : IConfig.ConfigFileSettings.DefaultValues.DEVELOPER_MODE);
            }
        }

        public abstract class LicenseManager
        {
            public static TraceLevel LogLevel { get { return (TraceLevel)Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.LICENSE_MANAGER, IConfig.Entries.Names.LOG_LEVEL); } }
        }

        public abstract class ProviderManager
        { 
            public static TraceLevel LogLevel { get { return (TraceLevel)Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.PROV_MANAGER, IConfig.Entries.Names.LOG_LEVEL); } }
            public static uint ShutdownTimeout { get { return Convert.ToUInt32(Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.PROV_MANAGER, IConfig.Entries.Names.SHUTDOWN_TIMEOUT)); } }
            public static uint StartupTimeout { get { return Convert.ToUInt32(Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.PROV_MANAGER, IConfig.Entries.Names.STARTUP_TIMEOUT)); } }
        }
        
        public abstract class ApplicationServer
        {
            public static TraceLevel LogLevel { get { return (TraceLevel)Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.APP_SERVER, IConfig.Entries.Names.LOG_LEVEL); } }
            public static string ServerName { get { return Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.APP_SERVER, IConfig.Entries.Names.SERVER_NAME) as string; } }
        }
        
        public abstract class Router
        {
            public static TraceLevel LogLevel { get { return (TraceLevel)Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.ROUTER, IConfig.Entries.Names.LOG_LEVEL); } }
            public static uint ActionTimeout { get { return Convert.ToUInt32(Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.ROUTER, IConfig.Entries.Names.ACTION_TIMEOUT)); } }
        }

        public abstract class Logger
        {			
            public static TraceLevel TcpLoggerLevel { get { return (TraceLevel)Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.LOGGER, IConfig.Entries.Names.LOG_LEVEL_TCP); } }
            public static TraceLevel LogServerLevel { get { return (TraceLevel)Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.LOGGER, IConfig.Entries.Names.LOG_LEVEL_SERVER_SINK); } }
            public static ushort TcpLoggerPort { get { return Convert.ToUInt16(Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.LOGGER, IConfig.Entries.Names.LOG_TCP_PORT)); } }
        }
        
        public abstract class AppManager
        {
            public static TraceLevel LogLevel { get { return (TraceLevel)Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.APP_MANAGER, IConfig.Entries.Names.LOG_LEVEL); } }
            public static ushort DebugListenPort { get { return Convert.ToUInt16(Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.APP_MANAGER, IConfig.Entries.Names.DEBUG_PORT)); } }
        }

        public abstract class TelephonyManager
        {
            public static TraceLevel LogLevel { get { return (TraceLevel)Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.TEL_MANAGER, IConfig.Entries.Names.LOG_LEVEL); } }
            public static bool SandboxEnabled { get { return (bool)Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.TEL_MANAGER, IConfig.Entries.Names.SANDBOX_ENABLED); } }
            public static bool DiagsEnabled { get { return (bool)Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.TEL_MANAGER, IConfig.Entries.Names.DIAG_CALL_TABLE); } }
        }

        public abstract class Management
        {
            public static TraceLevel LogLevel { get { return (TraceLevel)Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.MANAGEMENT, IConfig.Entries.Names.LOG_LEVEL); } }
            public static ushort ManagementPort { get { return Convert.ToUInt16(Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.MANAGEMENT, IConfig.Entries.Names.MGMT_PORT)); } }
        }

        public abstract class ClusterInterface
        {
            public static TraceLevel LogLevel { get { return (TraceLevel)Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.CLUSTER_INTERFACE, IConfig.Entries.Names.LOG_LEVEL); } }
            public static int StartupSyncTimeout { get { return Convert.ToInt32(Instance.GetSystemConfigValue(IConfig.SystemConfigs.SYNCH_TIMEOUT)); } }
            public static int HeartbeatInterval { get { return Convert.ToInt32(Instance.GetSystemConfigValue(IConfig.SystemConfigs.HB_INTERVAL)); } }
            public static int MaxMissedHeartbeats { get { return Convert.ToInt32(Instance.GetSystemConfigValue(IConfig.SystemConfigs.MAX_MISSED_HBS)); } }
            public static IPAddress StandbyAddr 
            { 
                get 
                {
                    try 
                    { 
                        IPAddress addr = IpUtility.ResolveHostname(Instance.GetSystemConfigValue(IConfig.SystemConfigs.STANDBY_ADDR)); 
                        if(addr.Equals(IPAddress.Any))
                            return IPAddress.None;
                        else
                            return addr;
                    }
                    catch { return IPAddress.None; }
                } 
            }
            public static IPAddress ParentAddr
            { 
                get 
                {
                    try 
                    { 
                        IPAddress addr = IpUtility.ResolveHostname(Instance.GetSystemConfigValue(IConfig.SystemConfigs.PARENT_ADDR)); 
                        if(addr.Equals(IPAddress.Any))
                            return IPAddress.None;
                        else
                            return addr;
                    }
                    catch { return IPAddress.None; }
                } 
            }

            public static void RemoveParentAddr()
            {
                Instance.RemoveSystemConfigValue(IConfig.SystemConfigs.PARENT_ADDR);
            }
        }

        public abstract class SmtpManager
        {
            public static string Recipient { get { return Instance.GetEntryValue(IConfig.ComponentType.SMTP_Manager, IConfig.CoreComponentNames.SMTP_MANAGER, IConfig.Entries.Names.EMAIL_TO) as string; } }
            public static string Sender { get { return Instance.GetEntryValue(IConfig.ComponentType.SMTP_Manager, IConfig.CoreComponentNames.SMTP_MANAGER, IConfig.Entries.Names.EMAIL_FROM) as string; } }
            public static string Server { get { return Instance.GetEntryValue(IConfig.ComponentType.SMTP_Manager, IConfig.CoreComponentNames.SMTP_MANAGER, IConfig.Entries.Names.EMAIL_SERVER) as string; } }
            public static ushort Port { get { return Convert.ToUInt16(Instance.GetEntryValue(IConfig.ComponentType.SMTP_Manager, IConfig.CoreComponentNames.SMTP_MANAGER, IConfig.Entries.Names.EMAIL_PORT)); } }
            public static string Username { get { return Instance.GetEntryValue(IConfig.ComponentType.SMTP_Manager, IConfig.CoreComponentNames.SMTP_MANAGER, IConfig.Entries.Names.EMAIL_USER) as string; } }
            public static string Password { get { return Instance.GetEntryValue(IConfig.ComponentType.SMTP_Manager, IConfig.CoreComponentNames.SMTP_MANAGER, IConfig.Entries.Names.EMAIL_PASSWORD) as string; } }
            public static bool UseSSL 
            { 
                get 
                { 
                    string s = Instance.GetEntryValue(IConfig.ComponentType.SMTP_Manager, IConfig.CoreComponentNames.SMTP_MANAGER, IConfig.Entries.Names.USE_SSL) as string;
                    return s != null ? bool.Parse(s) : false;
                } 
            }
            public static IConfig.Severity TriggerLevel 
            { 
                get 
                { 
                    string levelStr = Instance.GetEntryValue(IConfig.ComponentType.SMTP_Manager,
                        IConfig.CoreComponentNames.SMTP_MANAGER, IConfig.Entries.Names.ALARM_TRIGGER_LEVEL) as string;
                    IConfig.Severity level = IConfig.Severity.Unspecified;
                    try { level = (IConfig.Severity)Enum.Parse(typeof(IConfig.Severity), levelStr, true); }
                    catch {}

                    return level == IConfig.Severity.Unspecified ? IConfig.Severity.Yellow : level;
                } 
            }
        }

        public abstract class SnmpManager
        {
            public static IPAddress ServerAddr { get { return Instance.GetEntryValue(IConfig.ComponentType.SNMP_Manager, IConfig.CoreComponentNames.SNMP_MANAGER, IConfig.Entries.Names.SNMP_MANAGER) as IPAddress; } }
            public static IConfig.Severity TriggerLevel 
            { 
                get 
                { 
                    string levelStr = Instance.GetEntryValue(IConfig.ComponentType.SNMP_Manager,
                        IConfig.CoreComponentNames.SNMP_MANAGER, IConfig.Entries.Names.ALARM_TRIGGER_LEVEL) as string;
                    IConfig.Severity level = IConfig.Severity.Unspecified;
                    try { level = (IConfig.Severity)Enum.Parse(typeof(IConfig.Severity), levelStr, true); }
                    catch {}

                    return level == IConfig.Severity.Unspecified ? IConfig.Severity.Yellow : level;
                } 
            }
        }

        public abstract class StatsService
        {
            public static TraceLevel LogLevel { get { return (TraceLevel)Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.LOGGER, IConfig.Entries.Names.LOG_LEVEL_SERVER_SINK); } }
        }

        public abstract class LogService
        {
            public static ushort ListenPort { get { return Convert.ToUInt16(Instance.GetEntryValue(IConfig.ComponentType.LogServer, IConfig.CoreComponentNames.LOG_SERVER, IConfig.Entries.Names.LISTEN_PORT)); } }
            public static string FilePath { get { return Instance.GetEntryValue(IConfig.ComponentType.LogServer, IConfig.CoreComponentNames.LOG_SERVER, IConfig.Entries.Names.LOG_FILE_PATH, null) as string; } }
            public static uint NumFiles { get { return Convert.ToUInt32(Instance.GetEntryValue(IConfig.ComponentType.LogServer, IConfig.CoreComponentNames.LOG_SERVER, IConfig.Entries.Names.LOG_MAXFILES, null)); } }
            public static uint NumLinesPerFile { get { return Convert.ToUInt32(Instance.GetEntryValue(IConfig.ComponentType.LogServer, IConfig.CoreComponentNames.LOG_SERVER, IConfig.Entries.Names.LOG_FILE_LINES, null)); } }
        }

        public abstract class Watchdog
        {
            public static TraceLevel LogLevel { get { return (TraceLevel)Instance.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.LOGGER, IConfig.Entries.Names.LOG_LEVEL_SERVER_SINK); } }
        }
        #endregion

        #region Config Value Management

        #region Get Entries
        public object GetEntryValue(IConfig.ComponentType componentType, string componentName, string valueName)
        {
            return GetEntryValue(componentType, componentName, valueName, null);
        }

        public object GetEntryValue(IConfig.ComponentType componentType, string componentName, 
            string valueName, string partitionName)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                return GetEntryValue(db, componentType, componentName, valueName, partitionName);
            }
        }

        private object GetEntryValue(IDbConnection db, IConfig.ComponentType componentType, string componentName, 
            string valueName, string partitionName)
        {
            try
            {
                ConfigEntry cEntry = GetEntry(db, componentType, componentName, valueName, partitionName);
                return cEntry != null ? cEntry.Value : null;
            }
            catch {}

            return null;
        }

        public ConfigEntry GetEntry(IConfig.ComponentType componentType, string componentName,
			string valueName)
        {
            return GetEntry(componentType, componentName, valueName, null);
        }

        public IDictionary GetEntries(IConfig.ComponentType componentType, string componentName, 
            string partitionName)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                return GetEntries(db, componentType, componentName, partitionName);
            }
        }

        private IDictionary GetEntries(IDbConnection db, IConfig.ComponentType componentType, string componentName, 
            string partitionName)
        {
            if (componentType == IConfig.ComponentType.Unspecified || 
                componentName == null)
                return null;

            IDictionary configEntries = new Hashtable();

			DataTable data = Database.ConfigEntries.Select( db, componentType,
				componentName, partitionName );

			if (data != null && data.Rows.Count > 0)
			{
				for (int i = 0; i < data.Rows.Count; i++)
				{
					DataRow configEntryRow = data.Rows[i];

					uint id = (uint) configEntryRow[Database.Keys.CONFIG_ENTRIES];
					uint metaId = (uint) configEntryRow[Database.Keys.CONFIG_ENTRY_METAS];
					string valueName = (string) configEntryRow[Database.Fields.NAME];
					uint componentId = (uint) configEntryRow[Database.Keys.COMPONENTS];
					uint partitionId = (uint) configEntryRow[Database.Keys.APP_PARTITIONS];

//						log.Write( TraceLevel.Verbose,
//							"Configuring config {0} meta {1} name {2} comp {3} part {4}",
//							id, metaId, valueName, componentId, partitionId );

					// Partition entries trump component entries. You don't know which
					// order they will come in.
					if (!configEntries.Contains( valueName ) || partitionId != 0)
						configEntries[valueName] = PopulateConfigEntry(db, id, metaId);
				}
			}
			return configEntries;
		}

        public ConfigEntry GetEntry(IConfig.ComponentType componentType, string componentName, 
            string valueName, string partitionName)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                return GetEntry(db, componentType, componentName, valueName, partitionName);
            }
        }

        private ConfigEntry GetEntry(IDbConnection db, IConfig.ComponentType componentType, string componentName, 
            string valueName, string partitionName)
        {
            if ((componentType == IConfig.ComponentType.Unspecified) || 
                (componentName == null) ||
                (valueName == null))  
                return null;

            // There are three levels here, each superceding the previous: group, component, partition
            // Start with the most specific and work up 'til we find the value
            uint componentId = GetComponentId(db, componentType, componentName);

            DataRow configEntryRow = null;
            if((componentType == IConfig.ComponentType.Application) && (partitionName != null))
            {
                configEntryRow = GetPartitionEntry(db, componentId, partitionName, valueName);
                if(configEntryRow != null)
				{
					uint id = (uint) configEntryRow[Database.Keys.CONFIG_ENTRIES];
					uint metaId = (uint) configEntryRow[Database.Keys.CONFIG_ENTRY_METAS];
					return PopulateConfigEntry(db, id, metaId);
				}
            }

            configEntryRow = GetComponentEntry(db, componentId, valueName);
            if(configEntryRow != null)
			{
				uint id = (uint) configEntryRow[Database.Keys.CONFIG_ENTRIES];
				uint metaId = (uint) configEntryRow[Database.Keys.CONFIG_ENTRY_METAS];
				return PopulateConfigEntry(db, id, metaId);
			}
            return null;
        }

		public uint GetComponentId( IDbConnection db, IConfig.ComponentType componentType, string componentName )
		{
			DataTable data = Database.Components.Select(db, componentName, componentType);
			if (data == null || data.Rows.Count != 1)
				return uint.MaxValue;
			return (uint)data.Rows[0][Database.Keys.COMPONENTS];
		}

        private DataRow GetPartitionEntry(IDbConnection db, uint componentId, string partitionName,
			string valueName)
        {
            uint partitionId = GetPartitionId( db, componentId, partitionName );
			if (partitionId == uint.MaxValue)
				return null;

			return GetPartitionEntry( db, partitionId, valueName );
        }

		private uint GetPartitionId( IDbConnection db, uint componentId, string partitionName )
		{
			if (partitionName == null)
				return uint.MaxValue;

			DataTable data = Database.AppPartitions.Select(db, componentId, partitionName);
			if (data == null || data.Rows.Count != 1)
				return uint.MaxValue;

			return (uint) data.Rows[0][Database.Keys.APP_PARTITIONS];
		}

		private DataRow GetPartitionEntry( IDbConnection db, uint partitionId, string valueName )
		{
			DataTable data = Database.ConfigEntries.Select(db, 0, partitionId);
			if(data == null || data.Rows.Count == 0)
				return null;

			int n = data.Rows.Count;
			for (int i=0; i<n; i++)
			{
				uint metasId = (uint)data.Rows[i][Database.Keys.CONFIG_ENTRY_METAS];

				DataTable mdata = Database.ConfigEntryMetas.Select(db, metasId, 0, IConfig.ComponentType.Unspecified, null);
				if (mdata == null || mdata.Rows.Count == 0)
					continue;

				if (valueName.Equals( (string) mdata.Rows[0][Database.Fields.NAME] ))
					return data.Rows[i];
			}

			return null;
		}

        private DataRow GetComponentEntry(IDbConnection db, uint componentId, string valueName)
        {
            DataTable data = Database.ConfigEntries.Select(db, componentId, 0);
            if((data == null) || (data.Rows.Count == 0)) { return null; }

            for(int i=0; i<data.Rows.Count; i++)
            {
                uint metasId = (uint)data.Rows[i][Database.Keys.CONFIG_ENTRY_METAS];
                DataTable mdata = Database.ConfigEntryMetas.Select(db, metasId, 0, IConfig.ComponentType.Unspecified, null);
                if((mdata == null) || (mdata.Rows.Count == 0)) { continue; }

                if(valueName == mdata.Rows[0][Database.Fields.NAME] as string)
                {
                    return data.Rows[i];
                }
            }

            return null;
        }

        private ConfigEntry PopulateConfigEntry(IDbConnection db, uint id, uint metaId)
        {
            ConfigEntry cEntry = new ConfigEntry();
            cEntry.ID = id;
			cEntry.metaID = metaId;

            // Get metadata
            DataTable metaTable = Database.ConfigEntryMetas.Select(db, cEntry.metaID, 0, IConfig.ComponentType.Unspecified, null);
            if((metaTable == null) || (metaTable.Rows.Count == 0)) 
            { 
                log.Write(TraceLevel.Error, "Database Error: No metadata found for config entry: " + cEntry.ID);
                return null;
            }

            DataRow metaRow = metaTable.Rows[0];
            if(metaRow[Database.Fields.COMPONENT_TYPE] == Convert.DBNull)
            {
                cEntry.componentType = IConfig.ComponentType.Unspecified;
            }
            else
            {
                uint cTypeVal = (uint)metaRow[Database.Fields.COMPONENT_TYPE];
                cEntry.componentType = (IConfig.ComponentType)cTypeVal;
            }

            cEntry.name = metaRow[Database.Fields.NAME] as string;
            cEntry.displayName = metaRow[Database.Fields.DISPLAY_NAME] as string;
            cEntry.description = metaRow[Database.Fields.DESCRIPTION] as string;
            if(metaRow[Database.Fields.READ_ONLY] != Convert.DBNull)
                cEntry.readOnly = (sbyte)metaRow[Database.Fields.READ_ONLY] == 1;
            if(metaRow[Database.Fields.MIN_VALUE] != Convert.DBNull)
                cEntry.minValue = (int)metaRow[Database.Fields.MIN_VALUE];
            if(metaRow[Database.Fields.MAX_VALUE] != Convert.DBNull)
                cEntry.maxValue = (int)metaRow[Database.Fields.MAX_VALUE];

            // Get format type info
            uint formatId = (uint)metaRow[Database.Keys.FORMAT_TYPES];
            DataTable formatTable = Database.FormatTypes.Select(db, formatId, 0, null);
            if((formatTable == null) || (formatTable.Rows.Count == 0)) 
            { 
                log.Write(TraceLevel.Error, "Database Error: Format ID is invalid for config item: " + cEntry.name); 
                return null;
            }
            
            DataRow formatRow = formatTable.Rows[0];
            cEntry.formatType = new FormatType();
            string formatName = formatRow[Database.Fields.NAME] as string;
            
            bool standardFormat = true;
            IConfig.StandardFormat format = IConfig.StandardFormat.String;
            try { format = (IConfig.StandardFormat) Enum.Parse(typeof(IConfig.StandardFormat), formatName, true); }
            catch { standardFormat = false; }

            if(standardFormat)
            {
                cEntry.formatType.InitStandardFormat(format);
            }
            else
            {
                cEntry.formatType.ID = (uint)formatRow[Database.Keys.FORMAT_TYPES];
                cEntry.formatType.name = formatName;
                cEntry.formatType.description = formatRow[Database.Fields.DESCRIPTION] as string;

                // Get custom enum values
                DataTable enumTable = Database.FormatTypeEnumValues.Select(db, cEntry.formatType.ID);
                if((enumTable == null) || (enumTable.Rows.Count == 0)) 
                {
                    log.Write(TraceLevel.Error, "Database Error: Custom format has no enumerable values: " + cEntry.formatType.name);
                    return null;
                }

                if(cEntry.formatType.enumValues == null) { cEntry.formatType.enumValues = new StringCollection(); }

                foreach(DataRow row in enumTable.Rows)
                {
                    cEntry.formatType.enumValues.Add((string)row[Database.Fields.VALUE]);    
                }
            }

			try
			{
				// Load up the value(s)
				DataTable valueTable = Database.ConfigValues.Select(db, cEntry.ID);

				// Initialize collections
				switch(format)
				{
					case IConfig.StandardFormat.Array:
						cEntry.Value = PopulateArrayListFromConfig(valueTable);
						break;
					case IConfig.StandardFormat.HashTable:
						cEntry.Value = PopulateHashtableFromConfig(valueTable);
						break;
					case IConfig.StandardFormat.DataTable:
						cEntry.Value = PopulateDataTableFromConfig(valueTable);
						break;
                    default:
                        if(valueTable != null && valueTable.Rows.Count > 0)
                            cEntry.ParseValue(valueTable.Rows[0][Database.Fields.VALUE]);
                        else
                            cEntry.Value = null;
                        break;
				}
			}
			catch(Exception e)
			{
				log.Write(TraceLevel.Error, "Could not load configuration entry '{0}': {1}", cEntry.name, e.Message);
				return null;
			}

            return cEntry;
        }

		private DataTable PopulateDataTableFromConfig(DataTable valueTable)
		{
			if((valueTable == null) || (valueTable.Rows.Count == 0)) 
				return null;

			ArrayList columns = new ArrayList();
			DataTable table = new DataTable();

			// Get a list of all the column names
			foreach(DataRow row in valueTable.Rows)
			{
				string columnName = Convert.ToString(row[Database.Fields.KEY_COLUMN]);
				if(columnName != null && columnName != String.Empty)
				{
					if(!columns.Contains(columnName))
						columns.Add(columnName);
				}
			}

			// Initialize the DataTable with the column names
			foreach(string columnName in columns)
			{
				table.Columns.Add(columnName);
			}

			// Populate by selecting all values for each unique row name
			string expPrefix = Database.Fields.ORDINAL_ROW + " = ";
			ArrayList rowNames = new ArrayList();

			for(int i=0; i<valueTable.Rows.Count; i++)
			{
				string rowName = Convert.ToString(valueTable.Rows[i][Database.Fields.ORDINAL_ROW]);
				if(!rowNames.Contains(rowName))
				{
					rowNames.Add(rowName);
					DataRow[] currRowData = valueTable.Select(expPrefix + rowName);
					
					DataRow newDataRow = table.NewRow();
					foreach(DataRow row in currRowData)
					{
						string columnName = Convert.ToString(row[Convert.ToString(Database.Fields.KEY_COLUMN)]);
						newDataRow[columnName] = row[Database.Fields.VALUE];
					}
					table.Rows.Add(newDataRow);    // Confused yet?  ;)
				}
			}

			return table;
		}

		private ArrayList PopulateArrayListFromConfig(DataTable valueTable)
		{
			ArrayList array = null;

			if((valueTable != null) && (valueTable.Rows.Count > 0)) 
			{
				object[] objArray = new object[valueTable.Rows.Count];

				// Ensure that the entries are in order
				foreach(DataRow row in valueTable.Rows)
				{
					objArray[Convert.ToUInt32(row[Database.Fields.ORDINAL_ROW])] = row[Database.Fields.VALUE];
				}

				array = new ArrayList(objArray);
			}

			return array;
		}

		private Hashtable PopulateHashtableFromConfig(DataTable valueTable)
		{
			Hashtable hash = null;
			if((valueTable != null) && (valueTable.Rows.Count > 0)) 
			{
				hash = new Hashtable();

				foreach(DataRow row in valueTable.Rows)
				{
					hash[Convert.ToString(row[Database.Fields.KEY_COLUMN])] = row[Database.Fields.VALUE];
				}
			}

			return hash;
		}

        #endregion

        #region Add Entries
        public bool AddEntry(IConfig.ComponentType componentType, string componentName, ConfigEntry cEntry)
        {
            return AddEntry(componentType, componentName, cEntry, true);
        }

        public bool AddEntry(IConfig.ComponentType componentType, string componentName, 
            ConfigEntry cEntry, bool overwrite)
        {
            if ((componentType == IConfig.ComponentType.Unspecified) || 
                (componentName == null) ||
                (cEntry == null))  
            { return false; }

            using(IDbConnection db = OpenAppServerDb())
            {
                DataTable data = Database.Components.Select(db, componentName, componentType);
                if((data == null) || (data.Rows.Count == 0)) { return false; }

                using(IDbTransaction trans = db.BeginTransaction())
                {
                    // There are three levels here, each superceding the previous: group, component, partition
                    // Use the most specific criteria specified
                    uint componentId = (uint)data.Rows[0][Database.Keys.COMPONENTS];

                    // Create the meta-data entry and format types, then figure out what to link them to
                    uint formatTypesId = cEntry.formatType.ID;
                    if(formatTypesId == 0)
                    {
                        // Custom format type
                        data = Database.FormatTypes.Select(db, 0, componentId, cEntry.formatType.name);
                        if((data != null) && (data.Rows.Count == 1))
                        {
                            formatTypesId = (uint)data.Rows[0][Database.Keys.FORMAT_TYPES];
                        }
                        else if((data == null) || (data.Rows.Count == 0))
                        {
                            formatTypesId = InsertCustomFormatType(db, cEntry.formatType, componentId);
                        }
                        else // database is hosed
                        {
                            trans.Rollback();
                            log.Write(TraceLevel.Error, "Database Error: Multiple format types found with the same name");
                            return false;
                        }

                        cEntry.formatType.ID = formatTypesId;  // So they can refer to it by ID next time
                    }

                    if(cEntry.metaID == 0)
                    {
                        // See if this is a standard entry (config_entry_metas_id < 100) 
                        //   or a pre-existing entry for this component or application partition
                        DataTable metas = Database.ConfigEntryMetas.Select(db, 0, 0, IConfig.ComponentType.Unspecified, cEntry.name);
                        DataTable entries = Database.ConfigEntries.Select(db, componentId, 0, 0);

                        ArrayList validMetaIds = new ArrayList();
                        if(entries != null)
                        {
                            foreach(DataRow row in entries.Rows)
                            {
                                validMetaIds.Add((uint)row[Database.Keys.CONFIG_ENTRY_METAS]);
                            }
                        }
                    
                        if((metas != null) && (metas.Rows.Count > 0))
                        {
                            // Filter out any results which are not related to this component and have IDs above 99
                            for(int i=0; i<metas.Rows.Count; i++)
                            {
                                DataRow row = metas.Rows[i];

                                uint metaId = (uint)row[Database.Keys.CONFIG_ENTRY_METAS];
                                if((validMetaIds.Contains(metaId) == false) && (metaId >= 100))
                                {
                                    row.Delete();
                                    i--;             // I don't want to hear it. If you only knew the hell I went through 
                                    //   to get this god-forsaken DataTable to play nice...
                                }
                            }

                            if(metas.Rows.Count > 1)
                            {
                                trans.Rollback();
                                log.Write(TraceLevel.Error, "Database Error: Multiple config entry metas found with the same name in the preloaded ID range (0-100)");
                                return false;
                            }
                        }

                        if((metas != null) && (metas.Rows.Count == 1))
                        {
                            // We got it, so use it
                            cEntry.metaID = (uint)metas.Rows[0][Database.Keys.CONFIG_ENTRY_METAS];
                        }
                        else
                        {
                            // Create a new config_entry_metas entry
                            if((cEntry.minValue != 0) || (cEntry.maxValue != 0))
                            {
                                cEntry.metaID = Database.ConfigEntryMetas.Insert(db, cEntry.readOnly, cEntry.required, (uint)formatTypesId, 
                                    cEntry.name, cEntry.displayName, cEntry.description, cEntry.minValue, true, cEntry.maxValue, true);
                            }
                            else
                            {
                                cEntry.metaID = Database.ConfigEntryMetas.Insert(db, cEntry.readOnly, cEntry.required, (uint)formatTypesId, 
                                    cEntry.name, cEntry.displayName, cEntry.description, 0, false, 0, false);
                            }
                        }
                    }

                    // Insert the config entry
                    data = Database.ConfigEntries.Select(db, componentId, 0, cEntry.metaID);
                    if((data != null) && (data.Rows.Count == 1))
                    {
                        cEntry.ID = (uint) data.Rows[0][Database.Keys.CONFIG_ENTRIES];
                    }
                    else
                    {                        
                        cEntry.ID = Database.ConfigEntries.Insert(db, cEntry.metaID, componentId, 0);
                    }

                    // Retrieve any existing values
                    DataTable currentValues = Database.ConfigValues.Select(db, cEntry.ID);

                    // Delete any existing values
                    data = Database.ConfigValues.Select(db, cEntry.ID);
                    if(data != null)
                    {
                        Database.ConfigValues.Delete(db, cEntry.ID);
                    }

                    // Insert the new value
                    if(cEntry.formatType.name == IConfig.StandardFormat.Array.ToString())
                    {
                        IEnumerable list = ConstructArrayValue(currentValues,
                            cEntry.Value as IEnumerable, overwrite);
                        if(list != null)
                        {
                            int i=0;
                            foreach(object obj in list)
                            {
                                Database.ConfigValues.Insert(db, cEntry.ID, Convert.ToString(obj), i, null);
                                i++;
                            }
                        }
                    }
                    else if(cEntry.formatType.name == IConfig.StandardFormat.HashTable.ToString())
                    {
                        IDictionary hash = ConstructDictionaryValue(currentValues, 
                            cEntry.Value as IDictionary, overwrite);

                        if(hash != null)
                        {
                            foreach(DictionaryEntry de in hash)
                            {
                                Database.ConfigValues.Insert(db, cEntry.ID, 
                                    de.Value == null ? null : de.Value.ToString() , -1, de.Key as string);  
                            }
                        }
                    }
                    else if(cEntry.formatType.name == IConfig.StandardFormat.DataTable.ToString())
                    {
                        // TODO: Handle data tables
                    }
                    else if(cEntry.Value != null || currentValues != null)
                    {
                        object newValue = ConstructSingleEntryValue(currentValues, 
                            cEntry.Value, overwrite);

                        Database.ConfigValues.Insert(db, cEntry.ID, 
                            newValue == null ? null : newValue.ToString(), -1, null);
                    }

                    trans.Commit();
                }
            }
            return true;
        }

        /// <summary>
        ///     Determines if the database has a value already in it for a simple entry,
        ///     returning that value if so, or defaulting to the default value otherwise
        /// </summary>
        /// <param name="currentValues">
        ///     DataTable representing the query for the value of the config entry 
        /// </param>
        /// <param name="defaultValue">
        ///     The value to default to, in the absence of data in the currentValues DataTable
        /// </param>
        /// <returns>
        ///     The value in the database if found, otherwise the default value
        /// </returns>
        private object ConstructSingleEntryValue(DataTable currentValues, 
            object defaultValue, bool overwrite)
        {
            if(!overwrite && currentValues != null && currentValues.Rows.Count > 0)
            {
                return currentValues.Rows[0][Database.Fields.VALUE];
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     Determines if the database has a value already in it for an ArrayList,
        ///     returning that value if so, or defaulting to the default value otherwise
        /// </summary>
        /// <param name="currentValues">
        ///     DataTable representing the query for the value of the config entry
        /// </param>
        /// <param name="defaultValue">
        ///     The value to default to, in the absence of data in the currentValues DataTable
        /// </param>
        /// <returns>
        ///     The ArrayList in the database if found, otherwise the default value
        /// </returns>
        private IEnumerable ConstructArrayValue(DataTable currentValues, 
            IEnumerable defaultValue, bool overwrite)
        {
            if(!overwrite && currentValues != null && currentValues.Rows.Count > 0)
            {  
                SortedList reconstructed = new SortedList();
                foreach(DataRow row in currentValues.Rows)
                {
                    int index           = Convert.ToInt32(row[Database.Fields.ORDINAL_ROW]);
                    object foundValue   = row[Database.Fields.VALUE];
                    
                    reconstructed[index] = foundValue;
                }

                return new ArrayList(reconstructed.GetValueList());
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     Determines if the database has a value already in it for an Hashtable,
        ///     returning that value if so, or defaulting to the default value otherwise
        /// </summary>
        /// <param name="currentValues">
        ///     DataTable representing the query for the value of the config entry
        /// </param>
        /// <param name="defaultValue">
        ///     The value to default to, in the absence of data in the currentValues DataTable
        /// </param>
        /// <returns>
        ///     The Hashtable in the database if found, otherwise the default value
        /// </returns>
        private IDictionary ConstructDictionaryValue(DataTable currentValues, 
            IDictionary defaultValue, bool overwrite)
        {
            if(!overwrite && currentValues != null && currentValues.Rows.Count > 0)
            { 
                Hashtable reconstructed = new Hashtable();
                foreach(DataRow row in currentValues.Rows)
                {
                    object foundKey     = row[Database.Fields.KEY_COLUMN];
                    object foundValue   = row[Database.Fields.VALUE]; 
                    reconstructed[foundKey] = foundValue;
                }

                return reconstructed;
            }
            else
            {
                return defaultValue;
            }
        }

        private uint InsertCustomFormatType(IDbConnection db, FormatType formatType, uint componentId)
        {
            if((formatType == null) || (formatType.enumValues == null)) { return 0; }

            // Create new format type
            uint formatTypesId = Database.FormatTypes.Insert(db, formatType.name, 
                formatType.description, componentId);

            // Insert enumeration values
            foreach(string enumValue in formatType.enumValues)
            {
                Database.FormatTypeEnumValues.Insert(db, (uint)formatTypesId, enumValue);
            }

            return formatTypesId;
        }
        #endregion

        #region Update Entries

        public bool UpdateEntryMeta(IConfig.ComponentType componentType, string componentName, string valueName,
            string displayName, string description, bool readOnly, bool required)
        {
            return UpdateEntryMeta(componentType, componentName, valueName, displayName, description, 0,
                false, 0, false, readOnly, true, required, true);
        }

        public bool UpdateEntryMeta(IConfig.ComponentType componentType, string componentName, string valueName,
            string displayName, string description, int minValue, int maxValue, bool readOnly, bool required)
        {
            return UpdateEntryMeta(componentType, componentName, valueName, displayName, description, minValue,
                true, maxValue, true, readOnly, true, required, true);
        }

        public bool UpdateEntryMeta(IConfig.ComponentType componentType, string componentName, string valueName,
            string displayName, string description, int minValue, bool minValueSpecified, int maxValue, 
            bool maxValueSpecified, bool readOnly, bool readOnlySpecified, bool required, bool requiredSpecified)
        {
            if ((componentType == IConfig.ComponentType.Unspecified) || 
                (componentName == null) ||
                (valueName == null))  
            { return false; }

            using(IDbConnection db = OpenAppServerDb())
            {
                ConfigEntry cEntry = GetEntry(componentType, componentName, valueName);
                if(cEntry == null) { return false; }

                int rowsAffected = Database.ConfigEntryMetas.Update(db, cEntry.metaID, 0, null, 0, displayName, 
                    description, minValue, minValueSpecified, maxValue, maxValueSpecified, readOnly, 
                    readOnlySpecified, required, requiredSpecified);
                return rowsAffected > 0;
            }
        }

        #endregion

        #region Remove Entries
        public bool RemoveEntry(IConfig.ComponentType componentType, string componentName, string valueName,
            string partitionName)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                if(partitionName != null)
                {
                    ConfigEntry entry = GetEntry(componentType, componentName, valueName, partitionName);
                    if(entry == null) { return true; }

                    Database.ConfigEntries.Delete(db, entry.ID);
                    Database.ConfigValues.Delete(db, entry.ID);

                    RemoveEntryMeta(db, entry.metaID);
                }
                else
                {
                    // If partitionName is not specified, remove the entry from all partitions
                    uint componentId = GetComponentId(db, componentType, componentName);

                    DataTable data = Database.ConfigEntries.Select(db, componentId, 0);
                    if(data == null)
                        return false;

                    uint metaId = 0;

                    foreach(DataRow row in data.Rows)
                    {
                        uint metasId = (uint)row[Database.Keys.CONFIG_ENTRY_METAS];
                        DataTable mdata = Database.ConfigEntryMetas.Select(db, metasId, 0, IConfig.ComponentType.Unspecified, null);
                        if((mdata == null) || (mdata.Rows.Count != 1)) { continue; }

                        if(valueName == mdata.Rows[0][Database.Fields.NAME] as string)
                        {
                            metaId = (uint)mdata.Rows[0][Database.Keys.CONFIG_ENTRY_METAS];
                            break;
                        }
                    }

                    if(metaId > 0)
                    {
                        DataTable deadEntries = Database.ConfigEntries.Select(db, componentId, 0, metaId);
                        foreach(DataRow entryRow in deadEntries.Rows)
                        {
                            uint entryId = (uint)entryRow[Database.Keys.CONFIG_ENTRIES];                            
                            Database.ConfigEntries.Delete(db, entryId);
                            Database.ConfigValues.Delete(db, entryId);
                        }

                        RemoveEntryMeta(db, metaId);
                    }
                }
            }
            return true;
        }

        private void RemoveEntryMeta(IDbConnection db, uint metaId)
        {
            if(metaId < Consts.StdConfigMetaIdMax)
                return;

            DataTable metaTable = Database.ConfigEntryMetas.Select(db, metaId, 0, IConfig.ComponentType.Unspecified, null);
            if(metaTable == null || metaTable.Rows.Count != 1)
                return;

            // Only delete the meta if no other entries are using it
            DataTable otherEntries = Database.ConfigEntries.Select(db, 0, 0, metaId);
            if(otherEntries != null && otherEntries.Rows.Count > 0)
                return;
            
            Database.ConfigEntryMetas.Delete(db, metaId);

            // Remote the format type if it's non-standard and no other meta is using it
            uint formatTypeId = (uint)metaTable.Rows[0][Database.Keys.FORMAT_TYPES];
            if(formatTypeId < Consts.StdFormatIdMax)
                return;

            otherEntries = Database.ConfigEntryMetas.Select(db, 0, formatTypeId, IConfig.ComponentType.Unspecified, null);
            if(otherEntries != null && otherEntries.Rows.Count > 0)
                return;

            // Nuke the format and any enum values
            Database.FormatTypes.Delete(db, formatTypeId, 0);
            Database.FormatTypeEnumValues.Delete(db, 0, formatTypeId);
        }

        private void RemoveEntries(IDbConnection db, uint partitionId, uint componentId)
        {
            DataTable data = Database.ConfigEntries.Select(db, componentId, partitionId);
            if((data == null) || (data.Rows.Count == 0)) { return; } 
        
            foreach(DataRow row in data.Rows)
            {
                uint entryId = Convert.ToUInt32(row[Database.Keys.CONFIG_ENTRIES]);
                Database.ConfigEntries.Delete(db, entryId);
                Database.ConfigValues.Delete(db, entryId);

                uint metaId = Convert.ToUInt32(row[Database.Keys.CONFIG_ENTRY_METAS]);
                RemoveEntryMeta(db, metaId);
            }
        }
        #endregion

        #endregion

        #region Component Management

        public bool AddComponent(ComponentInfo cInfo, string locale)
        {
            if ((cInfo.type == IConfig.ComponentType.Unspecified) ||
                (cInfo.name == null))
            { return false; }

            using(IDbConnection db = OpenAppServerDb())
            {
                // Make sure a component with this name and type doesn't already exist.
                if(Database.Components.Select(db, cInfo.name, cInfo.type) != null)
                {
                    if(RemoveComponent(cInfo.type, cInfo.name) == false)
                    {
                        return false;
                    }
                }

                using(IDbTransaction trans = db.BeginTransaction())
                {
                    // Add component
                    cInfo.ID = Database.Components.Insert(db, cInfo.name, cInfo.displayName, cInfo.type, cInfo.status, 
                        cInfo.author, cInfo.copyright, cInfo.authorUrl, cInfo.supportUrl, cInfo.description, cInfo.version);

                    if(cInfo.ID == 0) { return false; }

                    // Determine group, if not specified
                    DataTable data;
                    if ((cInfo.groups == null) || 
                        (cInfo.groups.Length == 0) ||
                        ((cInfo.groups[0].ID == 0) && (cInfo.groups[0].name == null)))
                    {
                        // Find group by type (best-effort only)
                        data = Database.ComponentGroups.Select(db, 0, null, cInfo.type);
                        if((data == null) || (data.Rows.Count != 1)) { return false; }

                        cInfo.groups = new ComponentGroup[1];
                        cInfo.groups[0] = PopulateComponentGroup(data.Rows[0]);
                    }
                    else if((cInfo.groups[0].ID == 0) && (cInfo.groups[0].name != null))
                    {
                        // Find group by name
                        data = Database.ComponentGroups.Select(db, 0, cInfo.groups[0].name);
                        if((data == null) || (data.Rows.Count != 1)) { return false; }

                        cInfo.groups[0] = PopulateComponentGroup(data.Rows[0]);
                    }

                    // Add group mapping
                    Database.ComponentGroupMembers.Insert(db, cInfo.ID, cInfo.groups[0].ID);

                    // Add preloaded config items
                    data = Database.ConfigEntryMetas.Select(db, 0, 0, cInfo.type, null);
                    if(data != null)
                    {
                        foreach(DataRow row in data.Rows)
                        {
                            Database.ConfigEntries.Insert(db, (uint)row[Database.Keys.CONFIG_ENTRY_METAS], cInfo.ID, 0);
                        }
                    }

                    // Add default app partition
                    if(cInfo.type == IConfig.ComponentType.Application)
                    {
                        uint partitionId = Database.AppPartitions.Insert(db, cInfo.ID, 
                            Database.DefaultValues.PARTITION_NAME, 
                            true,
                            Database.DefaultValues.ALARM_GROUP_ID, 
                            Database.DefaultValues.H323_GROUP_ID, 
                            Database.DefaultValues.MEDIA_RES_GROUP_ID, 
                            Database.DefaultValues.PARTITION_DESC,
                            locale,
                            Database.DefaultValues.PREF_CODEC,
                            false);
                    }

                    trans.Commit();
                }
            }
            return true;
        }

        public bool UpdateComponent(IConfig.ComponentType type, string name, string displayName, string version,
            string description, string author, string copyright)
        {
            return UpdateComponent(type, name, displayName, version, description, author, copyright, 
                IConfig.Status.Unspecified);
        }

        private bool UpdateComponent(IConfig.ComponentType type, string name, string displayName, string version,
            string description, string author, string copyright, IConfig.Status status)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                DataTable data = Database.Components.Select(db, name, type);
                if((data == null) || (data.Rows.Count != 1)) { return false; }

                uint componentId = (uint)data.Rows[0][Database.Keys.COMPONENTS];

                int affected = Database.Components.Update(db, componentId, displayName, version, 
                    description, author, copyright, status);
                return affected != 0;
            }
        }

        public bool RemoveComponent(IConfig.ComponentType type, string name)
        {
            if((type == IConfig.ComponentType.Core) ||
                (name == null)) 
            { return false; }

            using(IDbConnection db = OpenAppServerDb())
            {
                DataTable data = Database.Components.Select(db, name, type);
                if((data == null) || (data.Rows.Count == 0)) { return false; }

                using(IDbTransaction trans = db.BeginTransaction())
                {
                    uint componentId = (uint)data.Rows[0][Database.Keys.COMPONENTS];

                    // Remove component
                    Database.Components.Delete(db, componentId);
                    // Remove component group mapping
                    Database.ComponentGroupMembers.Delete(db, componentId, 0);

                    // Remove application specific stuff
                    if(type == IConfig.ComponentType.Application)
                    {
                        // Remove scripts, trigger params, and partitions
                        data = Database.AppScripts.Select(db, componentId, null);
                        if(data != null)
                        {
                            foreach(DataRow row in data.Rows)
                            {
                                DataTable paramTable = Database.AppScriptTriggerParams.Select(db, (uint)row[Database.Keys.APP_SCRIPTS], 0);
                                if(paramTable != null && paramTable.Rows.Count > 0)
                                {
                                    foreach(DataRow paramRow in paramTable.Rows)
                                    {
                                        Database.TriggerParamValues.Delete(db, 0, (uint)paramRow[Database.Keys.APP_SCRIPT_TRIG_PARAMS]);
                                    }
                                }
                                Database.AppScriptTriggerParams.Delete(db, 0, (uint)row[Database.Keys.APP_SCRIPTS]);
                            }
            
                            Database.AppScripts.Delete(db, 0, componentId);
                        }

                        data = Database.AppPartitions.Select(db, componentId, null);
                        if(data != null)
                        {
                            // Remove partition config entries
                            foreach(DataRow row in data.Rows)
                            {
                                uint partitionId = (uint)row[Database.Keys.APP_PARTITIONS];
                                RemoveEntries(db, partitionId, 0);
                            }

                            Database.AppPartitions.Delete(db, 0, componentId);
                        }
                    }
                    else if(type == IConfig.ComponentType.Provider)
                    {
                        // Remove extensions
                        data = Database.ProviderExtensions.Select(db, componentId);
                        if(data != null)
                        {
                            foreach(DataRow row in data.Rows)
                            {
                                Database.ProviderExtensionParams.Delete(db, 0, (uint)row[Database.Keys.PROV_EXTS]);
                            }

                            Database.ProviderExtensions.Delete(db, 0, componentId);
                        }
                    }

                    // Remove component config items
                    RemoveEntries(db, 0, componentId);

                    // Remove format types
                    data = Database.FormatTypes.Select(db, 0, componentId, null);
                    if(data != null)
                    {
                        foreach(DataRow row in data.Rows)
                        {
                            Database.FormatTypeEnumValues.Delete(db, 0, (uint)row[Database.Keys.FORMAT_TYPES]);
                        }

                        Database.FormatTypes.Delete(db, 0, componentId);
                    }

                    trans.Commit();
                }
            }
            return true;
        }

        public ComponentInfo[] GetComponents(string groupName)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                DataTable data = Database.ComponentGroups.Select(db, 0, groupName);
                if((data == null) || (data.Rows.Count != 1)) { return null; }

                uint groupId = (uint)data.Rows[0][Database.Keys.COMPONENT_GROUPS];
                data = Database.ComponentGroupMembers.Select(db, 0, groupId);
                if((data == null) || (data.Rows.Count == 0)) { return null; } 

                ComponentInfo[] components = new ComponentInfo[data.Rows.Count];

                DataTable compTable;
                for(int i=0; i<data.Rows.Count; i++)
                {
                    uint componentId = (uint)data.Rows[i][Database.Keys.COMPONENTS];
                    compTable = Database.Components.Select(db, componentId);

                    if((compTable != null) && (compTable.Rows.Count == 1))
                    {
                        components[i] = PopulateComponentInfo(db, compTable.Rows[0]);
                    }
                }

                return components;
            }
        }

        public ComponentInfo[] GetComponents(IConfig.ComponentType type)
        {
            FailFilter filter = FailFilter.Local;

            if( type == IConfig.ComponentType.CTI_DevicePool ||
                type == IConfig.ComponentType.CTI_RoutePoint ||
                type == IConfig.ComponentType.H323_Gateway ||
                type == IConfig.ComponentType.SCCP_DevicePool ||
                type == IConfig.ComponentType.Cisco_SIP_DevicePool ||
                type == IConfig.ComponentType.IETF_SIP_DevicePool ||
                type == IConfig.ComponentType.SIP_Trunk)
            {
                filter = GetFailFilter();
            }

            // Consider how the fail filter affects the result set
            switch(filter)
            {
                case FailFilter.Local:
                    using(IDbConnection db = OpenAppServerDb())
                        return GetComponents(db, type);
                case FailFilter.Parent:
                    using(IDbConnection db = OpenParentDb())
                        return GetComponents(db, type);
                case FailFilter.Both:
                    ArrayList compArray = new ArrayList();
                    ComponentInfo[] components = null;
                    using(IDbConnection db = OpenAppServerDb())
                    {
                        components = GetComponents(db, type);
                        if(components != null && components.Length > 0)
                            compArray.AddRange(components);
                    }
                    using(IDbConnection db = OpenParentDb())
                    {
                        components = GetComponents(db, type);
                        if(components != null && components.Length > 0)
                            compArray.AddRange(components);
                    }
                    components = new ComponentInfo[compArray.Count];
                    compArray.CopyTo(components);
                    return components;
            }
            return null;
        }

        private ComponentInfo[] GetComponents(IDbConnection db, IConfig.ComponentType type)
        {
            DataTable data = Database.Components.Select(db, null, type);
            if((data == null) || (data.Rows.Count == 0)) { return null; }

            ComponentInfo[] components = new ComponentInfo[data.Rows.Count];

            for(int i=0; i<data.Rows.Count; i++)
            {
                components[i] = PopulateComponentInfo(db, data.Rows[i]);
            }

            return components;
        }

        public ComponentInfo GetComponentInfo(IConfig.ComponentType type, string name)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                DataTable data = Database.Components.Select(db, name, type);
                if((data == null) || (data.Rows.Count != 1)) { return null; }

                return PopulateComponentInfo(db, data.Rows[0]);
            }
        }

        private ComponentInfo PopulateComponentInfo(IDbConnection db, DataRow componentRow)
        {
            if(componentRow == null) { return null; }

            // Mandatory fields
            ComponentInfo cInfo = new ComponentInfo();
            cInfo.ID = Convert.ToUInt32(componentRow[Database.Keys.COMPONENTS]);
            cInfo.name = componentRow[Database.Fields.NAME] as string;
            cInfo.displayName = componentRow[Database.Fields.DISPLAY_NAME] as string;
            cInfo.version = componentRow[Database.Fields.VERSION] as string;
            cInfo.created = Convert.ToDateTime(componentRow[Database.Fields.CREATED_TS]);
            uint statVal = Convert.ToUInt32(componentRow[Database.Fields.STATUS]);
            cInfo.status = (IConfig.Status)statVal;
            uint typeVal = Convert.ToUInt32(componentRow[Database.Fields.TYPE]);
            cInfo.type = (IConfig.ComponentType)typeVal;
            cInfo.replicatedDb = db.Database != DatabaseName;

            // Optional fields
            cInfo.author = componentRow[Database.Fields.AUTHOR] as string;
            cInfo.authorUrl = componentRow[Database.Fields.AUTHOR_URL] as string;
            cInfo.copyright = componentRow[Database.Fields.COPYRIGHT] as string;
            cInfo.description = componentRow[Database.Fields.DESCRIPTION] as string;
            cInfo.supportUrl = componentRow[Database.Fields.SUPPORT_URL] as string;

            // Group info
            DataTable data = Database.ComponentGroupMembers.Select(db, cInfo.ID, 0);
            if(data != null)
            {
                cInfo.groups = new ComponentGroup[data.Rows.Count];

                DataTable groupData;
                for(int i=0; i<data.Rows.Count; i++)
                {
                    groupData = Database.ComponentGroups.Select(db, (uint)data.Rows[i][Database.Keys.COMPONENT_GROUPS], null);
                    if((groupData != null) && (groupData.Rows.Count == 1))
                    {
                        cInfo.groups[i] = PopulateComponentGroup(groupData.Rows[0]);
                    }
                    else
                    {
                        // ComponentGroupMembers table is screwed up
                    }
                }
            }
            
            return cInfo;
        }

        public IConfig.Status GetStatus(IConfig.ComponentType type, string name)
        {
            if((type == IConfig.ComponentType.Unspecified) ||
                (name == null))
            { return IConfig.Status.Unspecified; }

            using(IDbConnection db = OpenAppServerDb())
            {
                DataTable data = Database.Components.Select(db, name, type);
                if((data == null) || (data.Rows.Count != 1)) { return IConfig.Status.Unspecified; }

                IConfig.Status status = IConfig.Status.Unspecified;
                if(data.Rows[0][Database.Fields.STATUS] != Convert.DBNull)
                {
                    uint statVal = (uint)data.Rows[0][Database.Fields.STATUS];
                    status = (IConfig.Status)statVal;
                }

                return status;
            }
        }

        public bool UpdateStatus(IConfig.ComponentType type, string name, IConfig.Status status)
        {
            return UpdateComponent(type, name, null, null, null, null, null, status);
        }
        #endregion

        #region Component Group Access

        public ComponentInfo[] GetCallRouteGroup(string appName, string partitionName)
        {
            if(appName == null) { return null; }
            
            if(partitionName == null)
            {
                partitionName = Database.DefaultValues.PARTITION_NAME;
            }

            using(IDbConnection db = OpenAppServerDb())
            {
                ComponentInfo cInfo = GetComponentInfo(IConfig.ComponentType.Application, appName);
                if(cInfo == null) { return null; }

                DataTable partTable = Database.AppPartitions.Select(db, cInfo.ID, partitionName);
                if((partTable == null) || (partTable.Rows.Count == 0)) { return null; }

                uint groupId = Convert.ToUInt32(partTable.Rows[0][Database.Fields.CALL_GROUP_ID]);
                if(groupId == 0) { return null; }

                DataTable groupMembers = Database.ComponentGroupMembers.Select(db, 0, groupId);
                if(groupMembers == null || groupMembers.Rows.Count == 0) { return null; }

				// Sort the rows by ordinal
				int ordinal = 0;
				Hashtable orderedRows = new Hashtable();
				for(int i=0; i<groupMembers.Rows.Count; i++)
				{
					DataRow row = groupMembers.Rows[i];
					ordinal = Convert.ToInt32(row[Database.Fields.ORDINAL]);
					orderedRows[ordinal] = row;
				}

				ComponentInfo[] cInfos = new ComponentInfo[orderedRows.Count];

				ordinal = 0;
				for(int i=0; i<orderedRows.Count; i++)
				{
					// Account for gaps in the ordinals
					DataRow row = null;
					while(row == null)
					{
						row = orderedRows[ordinal] as DataRow;
						ordinal++;
					}

                    uint componentId = Convert.ToUInt32(row[Database.Keys.COMPONENTS]);

                    DataTable compTable = Database.Components.Select(db, componentId, null, IConfig.ComponentType.Unspecified);
                    if(compTable == null || compTable.Rows.Count != 1)
                    {
                        // Get group name
                        DataTable groupInfo = Database.ComponentGroups.Select(db, groupId, null);
                        if(groupInfo == null || groupInfo.Rows.Count == 0)
                            log.Write(TraceLevel.Error, "Call route group information is corrupt in the database.");
                        else
                            log.Write(TraceLevel.Error, "Call route group {0} contains invalid entries in the database",
                                groupInfo.Rows[0][Database.Fields.NAME]);
                        return null;
                    }

                    cInfos[i] = PopulateComponentInfo(db, compTable.Rows[0]);
                }

                return cInfos;
            }
        }

        public int GetCallRouteGroupType(string appName, string partitionName)
        {
            if(appName == null) { return 0; }
            
            if(partitionName == null)
            {
                partitionName = Database.DefaultValues.PARTITION_NAME;
            }

            using(IDbConnection db = OpenAppServerDb())
            {
                ComponentInfo cInfo = GetComponentInfo(IConfig.ComponentType.Application, appName);
                if(cInfo == null) { return 0; }

                DataTable partTable = Database.AppPartitions.Select(db, cInfo.ID, partitionName);
                if((partTable == null) || (partTable.Rows.Count == 0)) { return 0; }

                uint groupId = Convert.ToUInt32(partTable.Rows[0][Database.Fields.CALL_GROUP_ID]);
                if(groupId == 0) { return 0; }

                DataTable groupTable = Database.ComponentGroups.Select(db, groupId, null, IConfig.ComponentType.Unspecified);
                if((groupTable == null) || (groupTable.Rows.Count != 1)) { return 0; }

                return Convert.ToInt32(groupTable.Rows[0][Database.Fields.COMPONENT_TYPE]);
            }
        }

        public ComponentInfo[] GetMediaResourceGroup(string appName, string partitionName)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                return GetMediaResourceGroup(db, appName, partitionName, false);
            }
        }

        public ComponentInfo[] GetFailoverMRG(string appName, string partitionName)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                return GetMediaResourceGroup(db, appName, partitionName, true);
            }
        }

        private ComponentInfo[] GetMediaResourceGroup(IDbConnection db, string appName, 
            string partitionName, bool failover)
        {
            if(appName == null) { return null; }
            
            if(partitionName == null)
                partitionName = Database.DefaultValues.PARTITION_NAME;
            
            ComponentInfo cInfo = GetComponentInfo(IConfig.ComponentType.Application, appName);
            if(cInfo == null) { return null; }

            DataTable partTable = Database.AppPartitions.Select(db, cInfo.ID, partitionName);
            if((partTable == null) || (partTable.Rows.Count == 0)) { return null; }

            uint groupId = Convert.ToUInt32(partTable.Rows[0][Database.Fields.MEDIA_GROUP_ID]);
            if(groupId == 0) { return null; }

            DataTable groupTable = Database.ComponentGroups.Select(db, groupId, null, IConfig.ComponentType.Unspecified);
            if((groupTable == null) || (groupTable.Rows.Count == 0)) { return null; }

            if(failover)
            {
                uint failoverGroupId = Convert.ToUInt32(groupTable.Rows[0][Database.Fields.FAILOVER_GROUP_ID]);
                if(failoverGroupId == 0) { return null; }

                groupTable = Database.ComponentGroups.Select(db, failoverGroupId, null, IConfig.ComponentType.Unspecified);
                if((groupTable == null) || (groupTable.Rows.Count == 0)) { return null; }
            }

            ComponentGroup cGroup = PopulateComponentGroup(groupTable.Rows[0]);
            if(cGroup == null) { return null; }

            DataTable groupMembers = Database.ComponentGroupMembers.Select(db, 0, cGroup.ID);
            if(groupMembers == null) { return null; }

            int i = 0;
            ComponentInfo[] mediaServers = new ComponentInfo[groupMembers.Rows.Count];

            foreach(DataRow row in groupMembers.Rows)
            {
                uint componentId = Convert.ToUInt32(row[Database.Keys.COMPONENTS]);
                if(componentId == 0)
                {
                    log.Write(TraceLevel.Error, 
                        "Invalid component ID '0' encountered while fetching the members of the media resource group for {0}:{1}",
                        appName, partitionName);
                    return null;
                }

                DataTable compTable = Database.Components.Select(db, componentId, null, IConfig.ComponentType.Unspecified);
                if((compTable == null) || (compTable.Rows.Count != 1))
                {
                    log.Write(TraceLevel.Error, 
                        "Invalid component ID '{0}' encountered while fetching the members of the media resource group for {1}:{2}",
                        componentId.ToString(), appName, partitionName);
                    return null;
                }

                // Sanity
                uint compType = Convert.ToUInt32(compTable.Rows[0][Database.Fields.TYPE]);
                if(compType != (uint)IConfig.ComponentType.MediaServer)
                {
                    log.Write(TraceLevel.Error, 
                        "Non-media server component '{0}' encountered while fetching the members of the media resource group for {1}:{2}",
                        compType, appName, partitionName);
                    return null;
                }

                mediaServers[i] = PopulateComponentInfo(db, compTable.Rows[0]);
                i++;
            }

            return mediaServers;
        }

        private ComponentGroup PopulateComponentGroup(DataRow groupRow)
        {
            if(groupRow == null) { return null; }

            ComponentGroup cGroup = new ComponentGroup();
   
            // Mandatory fields
            cGroup = new ComponentGroup();
            cGroup.ID = (uint)groupRow[Database.Keys.COMPONENT_GROUPS];
            cGroup.name = (string)groupRow[Database.Fields.NAME];
            uint typeVal = (uint)groupRow[Database.Fields.COMPONENT_TYPE];
            cGroup.componentType = (IConfig.ComponentType)typeVal;

            // Optional fields
            cGroup.description = groupRow[Database.Fields.DESCRIPTION] as string;
            if(groupRow[Database.Fields.ALARM_GROUP_ID] != Convert.DBNull)
            {
                cGroup.alarmGroupID = (uint)groupRow[Database.Fields.ALARM_GROUP_ID];
            }
            if(groupRow[Database.Fields.FAILOVER_GROUP_ID] != Convert.DBNull)
            {
                cGroup.failoverGroupID = (uint)groupRow[Database.Fields.FAILOVER_GROUP_ID];
            }

            return cGroup;
        }

        #endregion

        #region CallManager Device Access

        public DeviceInfo[] GetSccpDevices(ComponentInfo iptServerInfo)
        {
            return GetDevices(iptServerInfo, IConfig.DeviceType.Sccp);
        }

        public DeviceInfo[] GetCtiDevices(ComponentInfo iptServerInfo)
        {
            if(iptServerInfo.type == IConfig.ComponentType.CTI_DevicePool)
                return GetDevices(iptServerInfo, IConfig.DeviceType.CtiPort);
            else if(iptServerInfo.type == IConfig.ComponentType.CTI_RoutePoint)
                return GetDevices(iptServerInfo, IConfig.DeviceType.RoutePoint);
            else if(iptServerInfo.type == IConfig.ComponentType.CTI_Monitored)
                return GetDevices(iptServerInfo, IConfig.DeviceType.CtiMonitored);
            return null;
        }

        private DeviceInfo[] GetDevices(ComponentInfo cInfo, IConfig.DeviceType type)
        {
            if(cInfo == null)
                return null;

            ArrayList devs = new ArrayList();

            IDbConnection db = null;
            if(cInfo.replicatedDb)
                db = OpenParentDb();
            else
                db = OpenAppServerDb();

            using(db)
            {
                string clusterName = null;
                string clusterVersion = null;
                if(!GetClusterInfo(db, cInfo, out clusterVersion, out clusterName))
                    return null;

                DataTable devTable = Database.CallManagerDevices.Select(db, cInfo.ID, null,
                    type, null, IConfig.Status.Unspecified);
                if(devTable == null || devTable.Rows.Count == 0) { return null; }

                foreach(DataRow row in devTable.Rows)
                {
                    DeviceInfo dInfo = PopulateDeviceInfo(db, row, cInfo, clusterVersion, clusterName);
                    if(dInfo != null)
                    {
                        devs.Add(dInfo);
                    }
                }
            }

            if(devs.Count == 0) { return null; }

            DeviceInfo[] devArray = new DeviceInfo[devs.Count];
            devs.CopyTo(devArray);
            return devArray;
        }

        private bool GetClusterInfo(IDbConnection db, ComponentInfo cInfo, out string version, out string name)
        {
            version = null;
            name = null;

            // Get version of server that this pool belongs to
            DataTable clusterMembers = Database.CallManagerClusterMembers.Select(db, cInfo.ID, 0);
            if(clusterMembers == null || clusterMembers.Rows.Count == 0)
            {
                log.Write(TraceLevel.Warning, "Device Pool '{0}' is not associated with an IPT server", cInfo.name);
                return false;
            }

            uint clusterId = Convert.ToUInt32(clusterMembers.Rows[0][Database.Keys.CM_CLUSTERS]);
            if(clusterId == 0)
            {
                log.Write(TraceLevel.Warning, "Device Pool '{0}' is not associated with an IPT server", cInfo.name);
                return false;
            }

            DataTable cluster = Database.CallManagerClusters.Select(db, clusterId);
            if(cluster == null || cluster.Rows.Count == 0)
            {
                log.Write(TraceLevel.Error, "Internal Error: {0} contains an association " + 
                    "between component {1} ({2}) and non-existant {3} record {4}", 
                    Database.Tables.CM_CLUSTER_MEMBERS, cInfo.ID, cInfo.name, Database.Tables.CM_CLUSTERS, clusterId);
                return false;
            }

            version = Convert.ToString(cluster.Rows[0][Database.Fields.VERSION]);
            name = Convert.ToString(cluster.Rows[0][Database.Fields.NAME]);
            return true;
        }

        public bool UpdateDeviceStatus(string deviceName, IConfig.DeviceType type, IConfig.Status status)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
				if(deviceName == null)
				{
					return Database.CallManagerDevices.Update(db, type, status, null);
				}
				else
				{
					DataTable devTable = Database.CallManagerDevices.Select(db, 0, deviceName, type, null, IConfig.Status.Unspecified);
					if(devTable == null || devTable.Rows.Count == 0) { return false; }

					if(devTable.Rows.Count > 1)
					{
						log.Write(TraceLevel.Error, "Detected multiple devices with the same name and type in configuration database: {0}({1})",
							deviceName, type.ToString());
						return false;
					}

					uint devicesId = Convert.ToUInt32(devTable.Rows[0][Database.Keys.CM_DEVICES]);
					return Database.CallManagerDevices.Update(db, devicesId, status, null);
				}
            }
        }

        public bool SetDirectoryNumber(string deviceName, IConfig.DeviceType type, string dn)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
				if(deviceName == null)
				{
					return Database.CallManagerDevices.Update(db, type, IConfig.Status.Unspecified, dn);
				}
				else
				{
					DataTable devTable = Database.CallManagerDevices.Select(db, 0, deviceName, type, null, IConfig.Status.Unspecified);
					if(devTable == null || devTable.Rows.Count == 0) { return false; }

					if(devTable.Rows.Count > 1)
					{
						log.Write(TraceLevel.Error, "Detected multiple devices with the same name and type in configuration database: {0}({1})",
							deviceName, type.ToString());
						return false;
					}

					uint devicesId = Convert.ToUInt32(devTable.Rows[0][Database.Keys.CM_DEVICES]);
					return Database.CallManagerDevices.Update(db, devicesId, IConfig.Status.Unspecified, dn);
				}
            }
        }
 
        private DeviceInfo PopulateDeviceInfo(IDbConnection db, DataRow deviceRow, ComponentInfo cInfo, 
            string clusterVersion, string clusterName)
        {
            if(deviceRow == null) { return null; }

            string name = Convert.ToString(deviceRow[Database.Fields.DEVICE_NAME]);
            if(name == null || name == String.Empty)
            {
                log.Write(TraceLevel.Error, "Call route group '{0}' contains a device with no name.", cInfo.name);
                return null;
            }

            IConfig.DeviceType type = (IConfig.DeviceType)Convert.ToUInt32(deviceRow[Database.Fields.DEVICE_TYPE]);
            IConfig.Status status = (IConfig.Status)Convert.ToUInt32(deviceRow[Database.Fields.STATUS]);
            string dn = deviceRow[Database.Fields.DIR_NUMBER] as string;

            if( type == IConfig.DeviceType.CtiPort || 
                type == IConfig.DeviceType.RoutePoint || 
                type == IConfig.DeviceType.CtiMonitored)
            {
                return PopulateCtiDeviceInfo(db, cInfo, name, dn, type, status, clusterVersion, clusterName);
            }
            else if(type == IConfig.DeviceType.Sccp)
            {
                return PopulateSccpDeviceInfo(db, cInfo, name, dn, type, status, clusterVersion, clusterName);
            }
            else
            {
                log.Write(TraceLevel.Warning, "Device '{0}' has no type specified", name);
            }

            return null;
        }

        private DeviceInfo PopulateCtiDeviceInfo(IDbConnection db, ComponentInfo cInfo, string name,
            string dn, IConfig.DeviceType type, IConfig.Status status, string clusterVersion, string clusterName)
        {
            string username = Convert.ToString(this.GetEntryValue(db, cInfo.type, cInfo.name, 
                IConfig.Entries.Names.USERNAME, null));
            string password = Convert.ToString(this.GetEntryValue(db, cInfo.type, cInfo.name,
                IConfig.Entries.Names.PASSWORD, null));
            
            if(username == null || username == String.Empty)
            {
                log.Write(TraceLevel.Error, "Device '{0}' has no username associated with it.", name);
                return null;
            }

            uint ctiManagerId1 = Convert.ToUInt32(this.GetEntryValue(db, cInfo.type, cInfo.name,
                IConfig.Entries.Names.PRIMARY_CTI_ID, null));
            uint ctiManagerId2 = Convert.ToUInt32(this.GetEntryValue(db, cInfo.type, cInfo.name,
                IConfig.Entries.Names.SECONDARY_CTI_ID, null));

            if(ctiManagerId1 == 0)
            {
                log.Write(TraceLevel.Error, "Device '{0}' has no CTI Manager associated with it.", name);
                return null;
            }

            IPAddress[] ctiManagerAddrs = null;
            if(ctiManagerId2 != 0)
            {
                ctiManagerAddrs = new IPAddress[2];
            }
            else
            {
                ctiManagerAddrs = new IPAddress[1];
            }

            try
            {
                DataTable ctiTable = Database.CtiManagers.Select(db, ctiManagerId1, 0, null, null);
                if(ctiTable == null || ctiTable.Rows.Count != 1)
                {
                    log.Write(TraceLevel.Error, "Internal Error: Device '{0}' is associated with a non-existant CTI manager (ID={1})",
                        name, ctiManagerId1);
                    return null;
                }

                string ctiAddrStr = Convert.ToString(ctiTable.Rows[0][Database.Fields.IP_ADDRESS]);
                ctiManagerAddrs[0] = IpUtility.ResolveHostname(ctiAddrStr);
                if(ctiManagerAddrs[0] == null)
                {
                    log.Write(TraceLevel.Error, "Could not resolve CTI manager address: " + ctiAddrStr);
                    return null;
                }

                if(ctiManagerId2 != 0 && ctiManagerAddrs.Length == 2)
                {
                    ctiTable = Database.CtiManagers.Select(db, ctiManagerId2, 0, null, null);
                    if(ctiTable == null || ctiTable.Rows.Count != 1)
                    {
                        log.Write(TraceLevel.Warning, "Internal Error: Device '{0}' is associated with a non-existant backup CTI manager (ID={1})",
                            name, ctiManagerId2);
                        ctiManagerAddrs = new IPAddress[] { ctiManagerAddrs[0] };
                    }
                    else
                        ctiManagerAddrs[1] = IpUtility.ResolveHostname(Convert.ToString(ctiTable.Rows[0][Database.Fields.IP_ADDRESS]));
                }

                return new DeviceInfo(name, dn, type, status, clusterVersion, clusterName, ctiManagerAddrs, 
                    username, password);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Device '{0}' references an invalid CTI Manager address.", name);
                return null;
            }
        }

        private DeviceInfo PopulateSccpDeviceInfo(IDbConnection db, ComponentInfo cInfo, string name,
            string dn, IConfig.DeviceType type, IConfig.Status status, string clusterVersion, string clusterName)
        {
            // Populate data for SCCP devices
            ArrayList subs = new ArrayList();
                
            subs.Add(Convert.ToUInt32(this.GetEntryValue(db, cInfo.type, cInfo.name,
                IConfig.Entries.Names.PRIMARY_CCM_SUB, null)));
            subs.Add(Convert.ToUInt32(this.GetEntryValue(db, cInfo.type, cInfo.name,
                IConfig.Entries.Names.SECONDARY_CCM_SUB, null)));
            subs.Add(Convert.ToUInt32(this.GetEntryValue(db, cInfo.type, cInfo.name,
                IConfig.Entries.Names.TERTIARY_CCM_SUB, null)));
            subs.Add(Convert.ToUInt32(this.GetEntryValue(db, cInfo.type, cInfo.name,
                IConfig.Entries.Names.QUATERNARY_CCM_SUB, null)));

            if(Convert.ToUInt32(subs[0]) == 0)
            {
                log.Write(TraceLevel.Error, "Device '{0}' has no CCM subscriber associated with it.", name);
                return null;
            }

            ArrayList subIPs = new ArrayList();

            foreach(uint subId in subs)
            {
                if(subId == 0)
                    break;

                IPAddress addr = GetSubscriberAddress(db, subId);
                if(addr == null)
                {
                    log.Write(TraceLevel.Error, "Device '{0}' references an invalid CCM subscriber address.", name);
                    continue;
                }

                subIPs.Add(addr);
            }

            if(subIPs.Count == 0)
                return null;

            IPAddress[] subscriberAddrs = new IPAddress[subIPs.Count];
            subIPs.CopyTo(subscriberAddrs);

            DeviceInfo dInfo = new DeviceInfo(name, dn, type, status, clusterVersion, clusterName, subscriberAddrs, null, null);

            // Check for SRST server
            uint srstId = Convert.ToUInt32(this.GetEntryValue(db, cInfo.type, cInfo.name,
                IConfig.Entries.Names.SRST_CCM_SUB, null));
            if(srstId > 0)
            {
                IPAddress addr = GetSubscriberAddress(db, srstId);
                if(addr == null)
                {
                    log.Write(TraceLevel.Error, "Device '{0}' references an invalid SRST address.", name);
                }
                else
                {
                    dInfo.FailoverServerAddrs = new IPAddress[1];
                    dInfo.FailoverServerAddrs[0] = addr;
                }
            }

            return dInfo;
        }

        private IPAddress GetSubscriberAddress(IDbConnection db, uint subscriberId)
        {
            if(subscriberId == 0)
                return null;

            DataTable subTable = Database.CallManagerClusterSubscribers.Select(db, subscriberId, 0);
            if(subTable == null || subTable.Rows.Count != 1)
                return null;

            string addrStr = Convert.ToString(subTable.Rows[0][Database.Fields.IP_ADDRESS]);
            IPAddress addr = IpUtility.ResolveHostname(addrStr);
            if(addr == null)
                log.Write(TraceLevel.Warning, "Failed to resolve address: " + addrStr);
            
            return addr;
        }

        #endregion

        #region SIP Device Access

        /// <summary>For use by SIP provider and TM</summary>
        public SipDeviceInfo[] GetSipDevices(ComponentInfo iptServerInfo)
        {
            IDbConnection db = null;
            if(iptServerInfo.replicatedDb)
                db = OpenParentDb();
            else
                db = OpenAppServerDb();

            using(db)
            {
                // Lookup the SIP domain for this pool
                DataTable dmTable = Database.SipDomainMembers.Select(db, iptServerInfo.ID, 0);
                if(dmTable == null || dmTable.Rows.Count != 1)
                {
                    log.Write(TraceLevel.Warning, "Device Pool '{0}' is not associated with a SIP domain", iptServerInfo.name);
                    return null;
                }

                uint domainsId = Convert.ToUInt32(dmTable.Rows[0][Database.Keys.SIP_DOMAINS]);
                DataTable dTable = Database.SipDomains.Select(db, domainsId, null);
                if(dTable == null || dTable.Rows.Count != 1)
                {
                    log.Write(TraceLevel.Warning, "Device Pool '{0}' is not associated with a SIP domain", iptServerInfo.name);
                    return null;
                }

                // Extract the domain information
                string domainName = dTable.Rows[0][Database.Fields.DOMAIN_NAME] as string;
            
                IPAddress regAddr1 = IpUtility.ResolveHostname(dTable.Rows[0][Database.Fields.REGISTRAR] as string);
                if(regAddr1 == null)
                {
                    log.Write(TraceLevel.Warning, "SIP domain has no registrar configured: " + domainName);
                    return null;
                }
                
                IPAddress regAddr2 = IpUtility.ResolveHostname(dTable.Rows[0][Database.Fields.REGISTRAR2] as string);

                IPAddress[] addrs;
                if(regAddr2 != null)
                {
                    addrs = new IPAddress[2];
                    addrs[0] = regAddr1;
                    addrs[1] = regAddr2;
                }
                else
                {
                    addrs = new IPAddress[1];
                    addrs[0] = regAddr1;
                }

                // Figure out what kind of SIP devices these are and go from there
                IConfig.SipDomainType sipType = 
                    (IConfig.SipDomainType)Convert.ToUInt32(dTable.Rows[0][Database.Fields.TYPE]);

                if(sipType == IConfig.SipDomainType.Cisco)
                    return GetCiscoSipDevices(db, iptServerInfo, domainName, addrs);
                else if(sipType == IConfig.SipDomainType.IETF)
                    return GetIetfSipDevices(db, iptServerInfo, domainName, addrs);
                else
                    log.Write(TraceLevel.Error, "No type specified for SIP domain: " + domainName);

                return null;
            }
        }

        public SipDomainInfo GetSipDomainInfo(string domainName)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                DataTable domainTable = Database.SipDomains.Select(db, 0, domainName);
                if(domainTable == null || domainTable.Rows.Count != 1)
                    return null;

                DataRow dRow = domainTable.Rows[0];

                uint domainId = Convert.ToUInt32(dRow[Database.Keys.SIP_DOMAINS]);

                // Extract the domain information
                IPAddress regAddr1 = IpUtility.ResolveHostname(dRow[Database.Fields.REGISTRAR] as string);
                if(regAddr1 == null)
                {
                    log.Write(TraceLevel.Warning, "SIP domain has no registrar configured: " + domainName);
                    return null;
                }

                IPAddress regAddr2 = IpUtility.ResolveHostname(dRow[Database.Fields.REGISTRAR2] as string);

                IConfig.SipDomainType type = 
                    (IConfig.SipDomainType) Convert.ToUInt32(dRow[Database.Fields.TYPE]);
                
                if(type == IConfig.SipDomainType.Unspecified)
                    log.Write(TraceLevel.Error, "No type specified for SIP domain: " + domainName);

                IPAddress proxyAddr = null;
                DataTable proxyTable = Database.SipProxies.Select(db, 0, domainId);
                if(proxyTable != null && proxyTable.Rows.Count > 0)
                {
                    string proxyAddrStr = proxyTable.Rows[0][Database.Fields.IP_ADDRESS] as string;
                    try
                    {
                        proxyAddr = IPAddress.Parse(proxyAddrStr);
                    }
                    catch
                    {
                        log.Write(TraceLevel.Warning, "Invalid proxy address specified for SIP domain '{0}': {1}", domainName, proxyAddrStr);
                    }
                }

                return new SipDomainInfo(domainName, type, regAddr1, regAddr2, proxyAddr);
            }
        }

        private SipDeviceInfo[] GetCiscoSipDevices(IDbConnection db, ComponentInfo iptServerInfo, 
            string domainName, IPAddress[] addrs)
        {
            DataTable devTable = Database.CallManagerDevices.Select(db, iptServerInfo.ID, null, 
                IConfig.DeviceType.Unspecified, null, IConfig.Status.Unspecified);
            if(devTable == null || devTable.Rows.Count == 0)
                return null;

            ArrayList devs = new ArrayList();

            foreach(DataRow row in devTable.Rows)
            {
                SipDeviceInfo dInfo = PopulateCiscoSipDeviceInfo(db, row, iptServerInfo, domainName, addrs);
                if(dInfo != null)
                    devs.Add(dInfo);
            }

            if(devs.Count == 0) { return null; }

            SipDeviceInfo[] devArray = new SipDeviceInfo[devs.Count];
            devs.CopyTo(devArray);
            return devArray;
        }

        private SipDeviceInfo[] GetIetfSipDevices(IDbConnection db, ComponentInfo iptServerInfo, 
            string domainName, IPAddress[] addrs)
        {
            DataTable devTable = Database.IetfSipDevices.Select(db, iptServerInfo.ID, null, 
                IConfig.Status.Unspecified);
            if(devTable == null || devTable.Rows.Count == 0)
                return null;

            ArrayList devs = new ArrayList();

            foreach(DataRow row in devTable.Rows)
            {
                SipDeviceInfo dInfo = PopulateIetfSipDeviceInfo(db, row, iptServerInfo, domainName, addrs);
                if(dInfo != null)
                    devs.Add(dInfo);
            }

            if(devs.Count == 0) { return null; }

            SipDeviceInfo[] devArray = new SipDeviceInfo[devs.Count];
            devs.CopyTo(devArray);
            return devArray;
        }

        private SipDeviceInfo PopulateCiscoSipDeviceInfo(IDbConnection db, DataRow row, ComponentInfo cInfo,
            string domainName, IPAddress[] addrs)
        {
            IConfig.DeviceType dType = (IConfig.DeviceType)Convert.ToUInt32(row[Database.Fields.DEVICE_TYPE]);
            if( dType != IConfig.DeviceType.CiscoSip && 
                dType != IConfig.DeviceType.SipTrunk)
            {
                log.Write(TraceLevel.Warning, "Non-Cisco-SIP device found in call route group: " + cInfo.name);
                return null;
            }

            string deviceName = row[Database.Fields.DEVICE_NAME] as string;
            IConfig.Status status = (IConfig.Status)Convert.ToUInt32(row[Database.Fields.STATUS]);
            string dn = Convert.ToString(row[Database.Fields.DIR_NUMBER]);

            if(dType == IConfig.DeviceType.CiscoSip)
            {
                IPAddress proxyAddr = null;
                uint proxyId = Convert.ToUInt32(GetEntryValue(IConfig.ComponentType.Cisco_SIP_DevicePool, 
                    cInfo.name, IConfig.Entries.Names.PROXY_ID));
                if(proxyId > 0)
                {
                    DataTable pTable = Database.SipProxies.Select(db, proxyId, 0);
                    if(pTable != null && pTable.Rows.Count == 1)
                    {
                        proxyAddr = IpUtility.ResolveHostname(pTable.Rows[0][Database.Fields.IP_ADDRESS] as string);
                        if(proxyAddr == null)
                        {
                            log.Write(TraceLevel.Warning, "Address for default outbound proxy for domain '{0}' is invalid", domainName);
                        }
                    }
                }

                string username = GetEntryValue(IConfig.ComponentType.Cisco_SIP_DevicePool, 
                    cInfo.name, IConfig.Entries.Names.USERNAME) as string;
                string password = GetEntryValue(IConfig.ComponentType.Cisco_SIP_DevicePool, 
                    cInfo.name, IConfig.Entries.Names.PASSWORD) as string;

                return new SipDeviceInfo(deviceName, domainName, dn, dType, status, addrs, proxyAddr, username, password);
            }
            
            // Otherwise, it's a Cisco SIP trunk so we have everything we need already
            return new SipDeviceInfo(deviceName, domainName, dn, dType, status, addrs, null, null, null);
        }

        private SipDeviceInfo PopulateIetfSipDeviceInfo(IDbConnection db, DataRow row, ComponentInfo cInfo,
            string domainName, IPAddress[] addrs)
        {
            string username = row[Database.Fields.USERNAME] as string;
            string password = row[Database.Fields.PASSWORD] as string;
            IConfig.Status status = (IConfig.Status)Convert.ToUInt32(row[Database.Fields.STATUS]);        

            IPAddress proxyAddr = null;
            uint proxyId = Convert.ToUInt32(GetEntryValue(IConfig.ComponentType.IETF_SIP_DevicePool, 
                cInfo.name, IConfig.Entries.Names.PROXY_ID));
            if(proxyId > 0)
            {
                DataTable pTable = Database.SipProxies.Select(db, proxyId, 0);
                if(pTable != null && pTable.Rows.Count == 1)
                {
                    proxyAddr = IpUtility.ResolveHostname(pTable.Rows[0][Database.Fields.IP_ADDRESS] as string);
                    if(proxyAddr == null) 
                    {
                        log.Write(TraceLevel.Warning, "Address for default outbound proxy for domain '{0}' is invalid", domainName);
                    }
                }
            }

            string fqUsername = String.Format("{0}@{1}", username, domainName);

            return new SipDeviceInfo(fqUsername, domainName, fqUsername, IConfig.DeviceType.IetfSip, 
                status, addrs, proxyAddr, username, password);
        }

        #endregion

        #region Test Utilities
    
        public bool Test_RemoveScriptTriggerParam(string appName, string scriptName, string partitionName, string paramName)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                ComponentInfo cInfo = GetComponentInfo(IConfig.ComponentType.Application, appName);
                if(cInfo == null) { return false; }

                DataTable scriptTable = Database.AppScripts.Select(db, cInfo.ID, scriptName);
                if((scriptTable == null) || (scriptTable.Rows.Count != 1)) { return false; }

                uint scriptId = 0;
                try
                {
                    scriptId = (uint)scriptTable.Rows[0][Database.Keys.APP_SCRIPTS];
                    if(scriptId == 0) { return false; }
                }
                catch { return false; }

                DataTable pTable = Database.AppPartitions.Select(db, cInfo.ID, partitionName);
                if((pTable == null) || (pTable.Rows.Count != 1)) { return false; }

                uint pId = 0;
                try
                {
                    pId = (uint)pTable.Rows[0][Database.Keys.APP_PARTITIONS];
                    if(pId == 0) { return false; }
                }
                catch { return false; }

                DataTable trigParams = Database.AppScriptTriggerParams.Select(db, scriptId, pId);
                if(trigParams == null || trigParams.Rows.Count == 0) { return false; }

                uint paramId = 0;
                foreach(DataRow row in trigParams.Rows)
                {   
                    if(row[Database.Fields.NAME].ToString() == paramName)
                    {
                        paramId = Convert.ToUInt32(row[Database.Keys.APP_SCRIPT_TRIG_PARAMS]);
                        break;
                    }
                }

                if(paramId == 0) { return true; } 

                DataTable trigParamValues = Database.TriggerParamValues.Select(db, paramId);
                foreach(DataRow row in trigParamValues.Rows)
                {
                    uint trigParamValueId = Convert.ToUInt32(row[Database.Keys.TRIG_PARAM_VALUES]);

                    Database.TriggerParamValues.Delete(db, trigParamValueId, paramId);
                }

                Database.AppScriptTriggerParams.Delete(db, paramId, scriptId);              
            }

            return true;
        }

        /// <summary> Currently used exclusively by the Functional Test Framework </summary>
        public bool Test_CreateCallRouteGroup(string testname, string routeGroupName)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                DataTable data = Database.ComponentGroups.Select(db, 0, routeGroupName, IConfig.ComponentType.Unspecified);

                uint testGroupId;
                if(data == null)
                {
                    testGroupId = Database.ComponentGroups.Insert
                        (db, 0, 0, (uint) IConfig.ComponentType.Test, routeGroupName, "Functional Test created Call Route Group");

                    // Create a fake IPT server to put in the group
                    uint serverId = Database.Components.Insert(db, "Test Server", "Test Server", IConfig.ComponentType.Test, 
                        IConfig.Status.Enabled_Running, null, null, null, null, "Dummy server for test", "2.0");

                    Database.ComponentGroupMembers.Insert(db, serverId, testGroupId);
                }
                else
                {
                    testGroupId = (uint) data.Rows[0][Database.Keys.COMPONENT_GROUPS];
                }

                ComponentInfo cInfo = GetComponentInfo(IConfig.ComponentType.Application, testname);
                if(cInfo == null) { return false; }

                DataTable partTable = Database.AppPartitions.Select(db, cInfo.ID, Database.DefaultValues.PARTITION_NAME);
                if((partTable == null) || (partTable.Rows.Count == 0)) { return false; }

                uint appPartitionId = (uint) partTable.Rows[0][Database.Keys.APP_PARTITIONS];

                Database.AppPartitions.Update(db, appPartitionId, null, false, false, 0, testGroupId, 0, null, null);

                return true;
            }
        }

        public bool Test_CreateMediaResourceGroup(string testname, string mediaGroupName)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                // Find or create the group
                DataTable data = Database.ComponentGroups.Select(db, 0, mediaGroupName, IConfig.ComponentType.Unspecified);

                uint testGroupId;
                if(data == null)
                {
                    testGroupId = Database.ComponentGroups.Insert
                        (db, 0, 0, (uint) IConfig.ComponentType.Test, mediaGroupName, "Functional Test created Media Resource Group");
                }
                else
                {
                    testGroupId = (uint) data.Rows[0][Database.Keys.COMPONENT_GROUPS];
                }

                // Add all available media servers to the group
                ComponentInfo[] mediaServers = GetComponents(IConfig.ComponentType.MediaServer);
                if(mediaServers == null || mediaServers.Length == 0)
                    log.Write(TraceLevel.Warning, "No media servers found to add to test media resource group");
                else
                {
                    foreach(ComponentInfo serverInfo in mediaServers)
                    {
                        if(Database.ComponentGroupMembers.Select(db, serverInfo.ID, testGroupId) == null)
                            Database.ComponentGroupMembers.Insert(db, serverInfo.ID, testGroupId);
                    }
                }

                // Link the default partition of the test app to it
                ComponentInfo cInfo = GetComponentInfo(IConfig.ComponentType.Application, testname);
                if(cInfo == null) { return false; }

                DataTable partTable = Database.AppPartitions.Select(db, cInfo.ID, Database.DefaultValues.PARTITION_NAME);
                if((partTable == null) || (partTable.Rows.Count == 0)) { return false; }

                uint appPartitionId = (uint) partTable.Rows[0][Database.Keys.APP_PARTITIONS];

                Database.AppPartitions.Update(db, appPartitionId, null, false, false, 0, 0, testGroupId, null, null);

                return true;
            }
        }

        #endregion

        #region Application Script Management

        public bool AddScript(string appName, string scriptName, string eventType)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                ComponentInfo cInfo = GetComponentInfo(IConfig.ComponentType.Application, appName);
                if(cInfo == null) { return false; }

                // If the script is already present, don't do anything with it
                if(Database.AppScripts.Select(db, cInfo.ID, scriptName) != null) 
                    return false; 

                if(Database.AppScripts.Insert(db, cInfo.ID, scriptName, eventType, true, true) == 0) 
                    return false; 
            }

            return true;
        }

        public bool ScriptExists(string appName, string scriptName)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                ComponentInfo cInfo = GetComponentInfo(IConfig.ComponentType.Application, appName);
                if(cInfo == null) { return false; }

                DataTable scriptTable = Database.AppScripts.Select(db, cInfo.ID, scriptName);
                if((scriptTable == null) || (scriptTable.Rows.Count != 1)) { return false; }
            }

            return true;
        }

        public bool AddScriptTriggerParam(string appName, string scriptName, string partitionName, string paramName, object Value)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                ComponentInfo cInfo = GetComponentInfo(IConfig.ComponentType.Application, appName);
                if(cInfo == null) { return false; }

                DataTable scriptTable = Database.AppScripts.Select(db, cInfo.ID, scriptName);
                if((scriptTable == null) || (scriptTable.Rows.Count != 1)) { return false; }

                uint scriptId = 0;
                try
                {
                    scriptId = (uint)scriptTable.Rows[0][Database.Keys.APP_SCRIPTS];
                    if(scriptId == 0) { return false; }
                }
                catch { return false; }

                DataTable pTable = Database.AppPartitions.Select(db, cInfo.ID, partitionName);
                if((pTable == null) || (pTable.Rows.Count != 1)) { return false; }

                uint pId = 0;
                try
                {
                    pId = (uint)pTable.Rows[0][Database.Keys.APP_PARTITIONS];
                    if(pId == 0) { return false; }
                }
                catch { return false; }

                uint paramId = Database.AppScriptTriggerParams.Insert(db, paramName, scriptId, pId);
                if(paramId == 0) { return false; }

                if(Value == null)
                {
                    Database.TriggerParamValues.Insert(db, Convert.DBNull.ToString(), paramId);
                }
                else if(Value is String)  // Be careful, String implements IEnumerable!
                {
                    Database.TriggerParamValues.Insert(db, Value as String, paramId);
                }
                else if(Value is IEnumerable)
                {
                    IEnumerable eValue = Value as IEnumerable;
                    IEnumerator e = eValue.GetEnumerator();
                    while(e.MoveNext())
                    {
                        if(e.Current == null)
                        {
                            // I'm not 100% certain if this is really what we should do in this case
                            Database.TriggerParamValues.Insert(db, Convert.DBNull.ToString(), paramId);
                        }
                        else
                        {
                            Database.TriggerParamValues.Insert(db, e.Current.ToString(), paramId);
                        }
                    }
                }
                else
                {
                    Database.TriggerParamValues.Insert(db, Value.ToString(), paramId);
                }
            }

            return true;
        }

        public TriggerInfo[] GetAppTriggerInfo(string appName)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                ComponentInfo cInfo = GetComponentInfo(IConfig.ComponentType.Application, appName);
                if(cInfo == null) { return null; }

                DataTable scriptTable = Database.AppScripts.Select(db, cInfo.ID, null);
                if((scriptTable == null) || (scriptTable.Rows.Count == 0)) 
                {
                    log.Write(TraceLevel.Error, "Data Inconsistent: No master scripts in database for application: " + appName);
                    return null; 
                }

                DataTable partTable = Database.AppPartitions.Select(db, cInfo.ID, null, true, true);
                if((partTable == null) || (partTable.Rows.Count == 0))
                {
                    log.Write(TraceLevel.Error, "Data Inconsistent: No partitions in database for application: " + appName);
                    return null;
                }

                bool enabled = IsAppEnabled(appName);

                TriggerInfo[] tInfos = new TriggerInfo[scriptTable.Rows.Count * partTable.Rows.Count];  // Yep, you read it right, math

                // Here's the deal: We've got a two-dimensional data set (partitions/scripts) from which
                //   we're extracting one-dimensional data (trigger params). So the formula to find the index
                //   into the one dimensional array (tInfos) in terms of partitions and scripts is given by:
                //   tInfoIndex = (partitionIndex * scriptCount) + scriptIndex

                for(int partitionIndex = 0; partitionIndex < partTable.Rows.Count; partitionIndex++)
                {
                    DataRow partRow = partTable.Rows[partitionIndex];
                    if(partRow == null)
                    {
                        log.Write(TraceLevel.Error, "Data inconsistent accessing partition information for {0}", appName);
                        return null;
                    }

                    AppPartitionInfo pInfo = PopulateAppPartitionInfo(appName, partRow);
                    if(pInfo == null)
                        return null;

                    uint partitionId = Convert.ToUInt32(partRow[Database.Keys.APP_PARTITIONS]);

                    for(int scriptIndex = 0; scriptIndex < scriptTable.Rows.Count; scriptIndex++)
                    {
                        DataRow scriptRow = scriptTable.Rows[scriptIndex];
                        TriggerInfo tInfo = new TriggerInfo();

                        tInfo.appName = appName;
                        tInfo.partitionName = pInfo.Name;
                        tInfo.culture = pInfo.Culture;
                        tInfo.enabled = pInfo.Enabled;
                        tInfo.scriptName = scriptRow[Database.Fields.NAME] as string;
                        tInfo.eventName = scriptRow[Database.Fields.EVENT_TYPE] as string;

                        uint scriptId = (uint) scriptRow[Database.Keys.APP_SCRIPTS]; 
                        DataTable paramTable = Database.AppScriptTriggerParams.Select(db, scriptId, partitionId);
                        
                        if((paramTable != null) && (paramTable.Rows.Count > 0)) 
                        {
                            foreach(DataRow paramRow in paramTable.Rows)
                            {
                                string paramName = paramRow[Database.Fields.NAME] as String;

                                DataTable valueTable = Database.TriggerParamValues.Select(db, (uint)paramRow[Database.Keys.APP_SCRIPT_TRIG_PARAMS]);
                                if((valueTable == null) || (valueTable.Rows.Count == 0)) 
                                {
                                    tInfo.eventParams.Add(paramName, null);
                                }
                                else if(valueTable.Rows.Count == 1)
                                {
                                    tInfo.eventParams.Add(paramName, valueTable.Rows[0][Database.Fields.VALUE] as string);
                                }
                                else
                                {
                                    StringCollection values = new StringCollection();
                                    foreach(DataRow valueRow in valueTable.Rows)
                                    {
                                        values.Add(valueRow[Database.Fields.VALUE] as string);
                                    }
                                    tInfo.eventParams.Add(paramName, values);
                                }
                            }
                        }

                        tInfos[(partitionIndex * scriptTable.Rows.Count) + scriptIndex] = tInfo;
                    }
                }

                return tInfos;
            }
        }

        public bool IsAppEnabled(string appName)
        {
            IConfig.Status status = this.GetStatus(IConfig.ComponentType.Application, appName);
            if( status == IConfig.Status.Enabled_Running ||
                status == IConfig.Status.Enabled_Stopped)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region App Script Partitions

        public AppPartitionInfo GetPartitionInfo(string appName, string partitionName)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                ComponentInfo cInfo = GetComponentInfo(IConfig.ComponentType.Application, appName);
                if(cInfo == null) { return null; }

                DataTable pTable = Database.AppPartitions.Select(db, cInfo.ID, partitionName);
                if((pTable == null) || (pTable.Rows.Count != 1)) { return null; }

                uint pId = 0;
                try
                {
                    pId = (uint)pTable.Rows[0][Database.Keys.APP_PARTITIONS];
                    if(pId == 0) { return null; }
                }
                catch { return null; }
                
                return PopulateAppPartitionInfo(appName, pTable.Rows[0]);
            }
        }

        private AppPartitionInfo PopulateAppPartitionInfo(string appName, DataRow pRow)
        {
            string name = Convert.ToString(pRow[Database.Fields.NAME]);
            DateTime created = Convert.ToDateTime(pRow[Database.Fields.CREATED_TS]);
            string description = Convert.ToString(pRow[Database.Fields.DESCRIPTION]);
            bool enabled = Convert.ToBoolean(pRow[Database.Fields.ENABLED]);
            bool earlyMedia = Convert.ToBoolean(pRow[Database.Fields.EARLY_MEDIA]);

            string cultureStr = pRow[Database.Fields.LOCALE] as string;
            System.Globalization.CultureInfo culture = null;
            try
            {
                culture = new System.Globalization.CultureInfo(cultureStr);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Locale of {0}->{1} is invalid: {2}",
                    appName, name, cultureStr);
                return null;
            }

            // Convert codec string to ICallControl enum
            IMediaControl.Codecs preferredCodec = IMediaControl.Codecs.Unspecified;
            uint preferredFramesize = 0;
            string prefCodecStr = Convert.ToString(pRow[Database.Fields.PREF_CODEC]);
            switch(prefCodecStr)
            {
                case IConfig.PreferredCodec.g711a_20:
                    preferredCodec = IMediaControl.Codecs.G711a;
                    preferredFramesize = 20;
                    break;
                case IConfig.PreferredCodec.g711a_30:
                    preferredCodec = IMediaControl.Codecs.G711a;
                    preferredFramesize = 30;
                    break;
                case IConfig.PreferredCodec.g711u_20:
                    preferredCodec = IMediaControl.Codecs.G711u;
                    preferredFramesize = 20;
                    break;
                case IConfig.PreferredCodec.g711u_30:
                    preferredCodec = IMediaControl.Codecs.G711u;
                    preferredFramesize = 30;
                    break;
                case IConfig.PreferredCodec.g723_30:
                    preferredCodec = IMediaControl.Codecs.G723;
                    preferredFramesize = 30;
                    break;
                case IConfig.PreferredCodec.g723_60:
                    preferredCodec = IMediaControl.Codecs.G723;
                    preferredFramesize = 60;
                    break;
                case IConfig.PreferredCodec.g729_20:
                    preferredCodec = IMediaControl.Codecs.G729;
                    preferredFramesize = 20;
                    break;
                case IConfig.PreferredCodec.g729_30:
                    preferredCodec = IMediaControl.Codecs.G729;
                    preferredFramesize = 30;
                    break;
                case IConfig.PreferredCodec.g729_40:
                    preferredCodec = IMediaControl.Codecs.G729;
                    preferredFramesize = 40;
                    break;
            }

            return new AppPartitionInfo(name, created, description, enabled, earlyMedia, culture, preferredCodec, preferredFramesize);
        }

        #endregion

        #region Provider Extensions

        public bool AddExtension(string providerName, Extension ext)
        {
            if((ext == null) || (ext.name == null)) { return false; }

            using(IDbConnection db = OpenAppServerDb())
            {
                DataTable data = Database.Components.Select(db, providerName, IConfig.ComponentType.Provider);
                if((data == null) || (data.Rows.Count != 1)) { return false; }

                uint componentId = (uint)data.Rows[0][Database.Keys.COMPONENTS];

                // Check to see if it's already there
                DataTable existingExts = Database.ProviderExtensions.Select(db, componentId);
                if(existingExts != null)
                {
                    foreach(DataRow row in existingExts.Rows)
                    {
                        string existingName = row[Database.Fields.NAME] as string;
                        if(ext.name == existingName)
                        {
                            // It's already there, so bail out
                            return true;
                        }
                    }
                }

                using(IDbTransaction trans = db.BeginTransaction())
                {
                    ext.ID = Database.ProviderExtensions.Insert(db, componentId, ext.name, ext.synchronous, ext.description);
                    if(ext.ID == 0) 
                    {
                        trans.Rollback();
                        return false; 
                    }

                    if(ext.parameters != null)
                    {
                        // Add parameters
                        for(int i=0; i<ext.parameters.Length; i++)
                        {
                            // Get format type ID
                            uint formatTypesId = 0;
                            if(ext.parameters[i].type.Custom)
                            {
                                data = Database.FormatTypes.Select(db, 0, componentId, ext.parameters[i].type.name);
                                if((data == null) || (data.Rows.Count == 0))
                                {
                                    formatTypesId = InsertCustomFormatType(db, ext.parameters[i].type, componentId);
                                }
                                else
                                {
                                    formatTypesId = (uint)data.Rows[0][Database.Keys.FORMAT_TYPES];
                                }
                            }
                            else
                            {
                                data = Database.FormatTypes.Select(db, 0, 0, ext.parameters[i].type.name);
                                if((data == null) || (data.Rows.Count != 1)) 
                                {
                                    trans.Rollback();
                                    return false;
                                }

                                formatTypesId = (uint)data.Rows[0][Database.Keys.FORMAT_TYPES];
                            }

                            ext.parameters[i].ID = formatTypesId;

                            Database.ProviderExtensionParams.Insert(db, ext.ID, ext.parameters[i].ID, 
                                ext.parameters[i].name, ext.parameters[i].description);
                        }
                    }

                    trans.Commit();
                }
            }

            return true;
        }

        public void RemoveExtensions(string providerName)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                uint compId = GetComponentId(db, IConfig.ComponentType.Provider, providerName);
                if(compId > 0)
                    Database.ProviderExtensions.Delete(db, 0, compId);
            }
        }
        #endregion

        #region Call Manager clusters

        public CallManagerCluster[] GetCallManagerClusters()
        {
            DataTable clusterData = null;
            ArrayList array = new ArrayList();

            using(IDbConnection db = OpenAppServerDb())
            {
                clusterData = Database.CallManagerClusters.Select(db, 0);
            }

            if((clusterData == null) || (clusterData.Rows.Count == 0)) { return null; }

            foreach(DataRow row in clusterData.Rows)
            {
                uint id = Convert.ToUInt32(row[Database.Keys.CM_CLUSTERS]);
                string name = row[Database.Fields.NAME] as string;
                string description = row[Database.Fields.DESCRIPTION] as string;
                string publisherUsername = row[Database.Fields.PUBLISHER_USERNAME] as string;
                string publisherPassword = row[Database.Fields.PUBLISHER_PASSWORD] as string;
                string snmpCommunity = row[Database.Fields.SNMP_COMMUNITY] as string;

                string publisherIPStr = row[Database.Fields.PUBLISHER_IP] as string;
                IPAddress publisherIP = null;

                publisherIP = IpUtility.ResolveHostname(publisherIPStr);
                if(publisherIP == null)
                {
                    log.Write(TraceLevel.Warning, "Invalid CallManager cluster address (name={0} addr={1}). Please enter a valid address",
                        name, publisherIPStr);
                    continue;
                } // publisher IP try/catch

                double version = 0;
                try { version = Convert.ToDouble(row[Database.Fields.VERSION]); }
                catch
                {
                    string versionStr = Convert.ToString(row[Database.Fields.VERSION]);
                    log.Write(TraceLevel.Warning, "Invalid CallManager cluster version (name={0} version={1}). Please enter a normal decimal value",
                        name, versionStr);
                    continue;
                } // version try/catch

                // read in and parse subscriber information.
                ArrayList subscribers = new ArrayList();
                DataTable subscriberData;
                using(IDbConnection db = OpenAppServerDb())
                {
                    subscriberData = Database.CallManagerClusterSubscribers.Select(db, 0, id);
                }

                if (subscriberData != null && subscriberData.Rows.Count > 0)
                {
                    foreach (DataRow subRow in subscriberData.Rows)
                    {
                        uint subId = Convert.ToUInt32(subRow[Database.Keys.CM_CLUSTER_SUBS]);
                        string subName = subRow[Database.Fields.NAME] as string;
                        string subscriberIPstr = subRow[Database.Fields.IP_ADDRESS] as string;
                        IPAddress subscriberIPAddr = null;

                        subscriberIPAddr = IpUtility.ResolveHostname(subscriberIPstr); 
                        if(subscriberIPAddr == null)
                        {
                            log.Write(TraceLevel.Warning, "Invalid address specified for subscriber '{0}' in cluster '{1}'. Skipping.", subName, name);
                            continue;
                        }

                        subscribers.Add(new CallManagerSubscriber(subId, id, subName, subscriberIPAddr));

                    } // subscriber data foreach block
                } // if block

                CallManagerCluster cluster = new CallManagerCluster(id, name, publisherIP, 
                    (CallManagerSubscriber[]) subscribers.ToArray(typeof(CallManagerSubscriber)),
                    publisherUsername, publisherPassword, snmpCommunity, version, description);

                if(!cluster.IsValid())
                    log.Write(TraceLevel.Error, "Invalid cluster configuration:\r\n" + cluster);
                else                
                    array.Add(cluster);
            } // cluster data foreach block

            return (CallManagerCluster[]) array.ToArray(typeof(CallManagerCluster));
        }

        #endregion

        #region User Validation

        public IConfig.AccessLevel ValidateUser(string username, string password, string componentGroupName)
        {
            if((username == null) || (password == null)) { return IConfig.AccessLevel.Restricted; }

            using(IDbConnection db = OpenAppServerDb())
            {
                DataTable data = Database.Users.Select(db, username, IConfig.AccessLevel.Unspecified);
                if((data == null) || (data.Rows.Count != 1)) { return IConfig.AccessLevel.Restricted; }

                string storedPass = data.Rows[0][Database.Fields.PASSWORD] as string;
                if(password != storedPass) { return IConfig.AccessLevel.Restricted; }

                uint accessVal = (uint)data.Rows[0][Database.Fields.ACCESS_LEVEL];
                IConfig.AccessLevel access = (IConfig.AccessLevel)accessVal;

                if((componentGroupName != null) && (access != IConfig.AccessLevel.Administrator))
                {
                    uint usersId = (uint)data.Rows[0][Database.Keys.USERS];

                    data = Database.ComponentGroups.Select(db, 0, componentGroupName);
                    if((data == null) || (data.Rows.Count != 1)) { return IConfig.AccessLevel.Restricted; }

                    uint componentGroupId = (uint)data.Rows[0][Database.Keys.COMPONENT_GROUPS];

                    data = Database.UsersAclList.Select(db, usersId, componentGroupId);
                    if((data == null) || (data.Rows.Count != 1)) { return IConfig.AccessLevel.Restricted; }
                }

                return access;
            }
        }

        #endregion

        #region Service Parameters

        public void SetUserStoppedFlag(bool userStopped)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                Database.Services.Update(db, 1, userStopped);
            }
        }

        public ArrayList GetServicesInfo()
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                DataTable data = Database.Services.Select(db);
                if((data == null) || (data.Rows.Count == 0)) 
                    return null;

                ArrayList services = new ArrayList();

                foreach(DataRow row in data.Rows)
                {
                    string name = row[Database.Fields.NAME] as string;
                    string displayName = row[Database.Fields.DISPLAY_NAME] as string;
                    bool enabled = (Convert.ToSByte(row[Database.Fields.ENABLED]) > 0) ? true : false;
                    bool userStopped = (Convert.ToSByte(row[Database.Fields.USER_STOPPED])) > 0 ? true : false;

                    services.Add(new ServiceInfo(name, displayName, enabled, userStopped));				
                }
                return services;
            }
        }

        public bool AddService(string name, string displayName, string description)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                return Database.Services.Insert(db, name, displayName, description) != 0;
            }
        }

        public bool RemoveService(string displayName)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                return Database.Services.Delete(db, 0, displayName) != 0;
            }
        }

        #endregion

        #region System Configs

        public string GetSystemConfigValue(string valueName)
        {
            if(valueName == null || valueName == String.Empty)
                return null;

            using(IDbConnection db = OpenAppServerDb())
            {
                DataTable cTable = Database.SystemConfigs.Select(db, valueName);
                if(cTable == null || cTable.Rows.Count != 1)
                    return null;

                return Convert.ToString(cTable.Rows[0][Database.Fields.VALUE]);
            }
        }

        public void RemoveSystemConfigValue(string valueName)
        {
            if(valueName == null || valueName == String.Empty)
                return;

            using(IDbConnection db = OpenAppServerDb())
            {
                Database.SystemConfigs.Clear(db, valueName);
            }
        }

        #endregion

        #region Alarm Management
        /// <summary>
        /// Insert one new Alarm into DB
        /// </summary>
        /// <param name="data">Alarm data info</param>
        /// <returns></returns>
        public bool InsertAlarm(uint code, string guid, string description, IConfig.Severity severity)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                // Don't insert duplicate alarms
                DataTable dt = Database.EventLog.SelectAlarmByGuid(db, guid);
                if(dt != null)
                {
                    uint id = Convert.ToUInt32(dt.Rows[0][Database.Keys.EVENT_LOG]);
                    Database.EventLog.Delete(db, id);
                }

                return Database.EventLog.InsertAlarm(db, code, description, guid, severity) != 0;
            }
        }

        /// <summary>
        /// Clear alarm from DB by setting flag and timestamp.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public AlarmData ClearAlarm(string guid, string recoveredTS)
        {
            using(IDbConnection db = OpenAppServerDb())
            {
                DataTable dt = Database.EventLog.SelectAlarmByGuid(db, guid);
                if((dt == null) || (dt.Rows.Count != 1))
                    return null;

                DataRow alarmRow = dt.Rows[0];

                // Mark it recovered in the DB
                uint id = Convert.ToUInt32(alarmRow[Database.Keys.EVENT_LOG]);
                Database.EventLog.SetRecovered(db, id, recoveredTS);

                AlarmData data = PopulateAlarmData(alarmRow);
                data.SetRecovered(recoveredTS);
                return data;
            }
        }

        private AlarmData PopulateAlarmData(DataRow alarmRow)
        {
            uint alarmCode = Convert.ToUInt32(alarmRow[Database.Fields.MESSAGE_ID]);
            string description = Convert.ToString(alarmRow[Database.Fields.MESSAGE]);
            string createdTS = Convert.ToString(alarmRow[Database.Fields.CREATED_TS]);

            string sevStr = Convert.ToString(alarmRow[Database.Fields.SEVERITY]);
            IConfig.Severity severity = IConfig.Severity.Unspecified;
            try { severity = (IConfig.Severity) Enum.Parse(typeof(IConfig.Severity), sevStr, true); }
            catch { }

            return new AlarmData(alarmCode, description, createdTS, severity);
        }

        /// <summary>Returns a hash of SNMP OID info objects</summary>
        /// <returns>low-order number of OID (string) -> SnmpOid (object)</returns>
        public SortedList GetOIDs()
        {
            SortedList oids = new SortedList(new Metreos.Utilities.Collections.IntValueComparer());

            using(IDbConnection db = OpenAppServerDb())
            {
                DataTable oidTable = Database.SnmpMibDefs.Select(db, IConfig.SnmpOidType.Unspecified);
                if(oidTable != null && oidTable.Rows.Count > 0)
                {
                    foreach(DataRow row in oidTable.Rows)
                    {
                        string oid = Convert.ToString(row[Database.Fields.OID]);
                        string name = Convert.ToString(row[Database.Fields.NAME]);
                        string description = Convert.ToString(row[Database.Fields.DESCRIPTION]);
                        IConfig.SnmpOidType type = (IConfig.SnmpOidType) Convert.ToInt32(row[Database.Fields.TYPE]);
                        IConfig.SnmpSyntax data_type = (IConfig.SnmpSyntax) Convert.ToInt32(row[Database.Fields.DATA_TYPE]);
                        int ignoreInt = Convert.ToInt32(row[Database.Fields.IGNORE]);

                        oids.Add(oid, new SnmpOid(name, description, type, data_type, ignoreInt != 0));
                    }
                }
            }
            return oids;
        }

        public string GetOidRoot()
        {
            string root = String.Empty;

            using(IDbConnection db = OpenAppServerDb())
            {
                DataTable dt = Database.SystemConfigs.Select(db, IConfig.SystemConfigs.OID_ROOT);
                if(dt != null && dt.Rows.Count > 0)
                {
                    root = Convert.ToString(dt.Rows[0][Database.Fields.VALUE]);
                }
            }
            return root;
        }

        #endregion

        #region Public Helper Functions

        /// <summary>Checks for the presense of a database</summary>
        /// <param name="name">Database name</param>
        /// <returns>True if it exists</returns>
        public bool DatabaseExists(string name)
        {
            if(name == null) { return false; }

            string dsn = Database.FormatDSN(name, DatabaseHost, DatabasePort, DatabaseUsername, DatabasePassword, true);

            try
            {
                using(IDbConnection testDb = Database.CreateConnection(Database.DbType.mysql, dsn))
                {
                    testDb.Open();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Connects to the specified database. This method must not be called from child AppDomains.
        /// </summary>
        /// <param name="name">Database name</param>
        /// <returns>MySQL connection</returns>
        public IDbConnection DatabaseConnect(string name)
        {
            if (name == null) { return null; }

            lock(dbConnectLock)
            {
                string dsn = Database.FormatDSN(name, DatabaseHost, DatabasePort, DatabaseUsername, DatabasePassword, true);
                if(dsn == null)
                    return null;

                IDbConnection newDb = Database.CreateConnection(Database.DbType.mysql, dsn);

                try { newDb.Open(); }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Failed to connect to internal database '{0}'. Error: {1}",
                        name, e.Message);
                    return null;
                }
                return newDb;
            }
        }

        /// <summary>Drops a database</summary>
        /// <param name="name">Database name</param>
        public void DatabaseDrop(string name)
        {
            if ((name == null) || (String.Compare(name, DatabaseName, true) == 0)) 
                return;

            using(IDbConnection db = OpenAppServerDb())
            {
                using(IDbCommand command = db.CreateCommand())
                {
                    command.CommandText = "DROP DATABASE " + name + ";";
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>Creates a database</summary>
        /// <param name="name">Database name</param>
        /// <returns>True if successful</returns>
        public bool DatabaseCreate(string name)
        {
            if ((name == null) || (String.Compare(name, DatabaseName, true) == 0)) 
                return false;

            using(IDbConnection db = OpenAppServerDb())
            {
                using(IDbCommand command = db.CreateCommand())
                {
                    command.CommandText = "CREATE DATABASE " + name + ";";
                    command.ExecuteNonQuery();
                }
            }
            return true;
        }

        #endregion

        #region Private Helper Functions

        private IDbConnection OpenAppServerDb()
        {
            return this.DatabaseConnect(DatabaseName);
        }

        private IDbConnection OpenParentDb()
        {
            return this.DatabaseConnect(ReplicationDatabaseName);
        }

        private static DirectoryInfo GetDirectoryInfo(string directoryName)
        {
            if(directoryName == null) { return null; }

            string fullPath = Path.Combine(RootPath, directoryName);

            try
            {
                return new DirectoryInfo(fullPath);
            }
            catch 
            {
                return null;
            }
        }

        /// <summary>Uses the failover state table to determine the set of devices to return</summary>
        /// <remarks>See: http://wiki.metreos.com/confluence/display/DEV/Failover+state+table for details</remarks>
        private FailFilter GetFailFilter()
        {
            FailFilter filter = (FailFilter)failStateTable[(ushort)ParentFailoverStatus, (ushort)StandbyFailoverStatus];

            if(filter == FailFilter.Invalid)
            {
                log.Write(TraceLevel.Error, "Cluster parent status is invalid: {0}. Resetting.",
                    ParentFailoverStatus.ToString());
                ParentFailoverStatus = IConfig.FailoverStatus.Normal;

                filter = (FailFilter)failStateTable[(ushort)ParentFailoverStatus, (ushort)StandbyFailoverStatus];
            }
            return filter;
        }

        /// <summary>Builds the constant failover state table</summary>
        /// <remarks>See: http://wiki.metreos.com/confluence/display/DEV/Failover+state+table for details</remarks>
        private static void BuildFailStateTable()
        {
            if(failStateTable != null)
                return;

            failStateTable = new ushort[3,3];
            failStateTable[(ushort)IConfig.FailoverStatus.Normal, (ushort)IConfig.FailoverStatus.Normal] = (ushort)FailFilter.Local;
            failStateTable[(ushort)IConfig.FailoverStatus.Normal, (ushort)IConfig.FailoverStatus.Failover] = (ushort)FailFilter.Both;
            failStateTable[(ushort)IConfig.FailoverStatus.Normal, (ushort)IConfig.FailoverStatus.Failback] = (ushort)FailFilter.Local;
            failStateTable[(ushort)IConfig.FailoverStatus.Failover, (ushort)IConfig.FailoverStatus.Normal] = (ushort)FailFilter.Invalid;
            failStateTable[(ushort)IConfig.FailoverStatus.Failover, (ushort)IConfig.FailoverStatus.Failover] = (ushort)FailFilter.Invalid;
            failStateTable[(ushort)IConfig.FailoverStatus.Failover, (ushort)IConfig.FailoverStatus.Failback] = (ushort)FailFilter.Invalid;
            failStateTable[(ushort)IConfig.FailoverStatus.Failback, (ushort)IConfig.FailoverStatus.Normal] = (ushort)FailFilter.None;
            failStateTable[(ushort)IConfig.FailoverStatus.Failback, (ushort)IConfig.FailoverStatus.Failover] = (ushort)FailFilter.Parent;
            failStateTable[(ushort)IConfig.FailoverStatus.Failback, (ushort)IConfig.FailoverStatus.Failback] = (ushort)FailFilter.None;
        }

        #region Config File Access
        private ushort GetDatabasePort()
        {
            try
            {
                return ushort.Parse(AppConfig.GetEntry(IConfig.ConfigFileSettings.DB_PORT));
            }
            catch
            {
                return 0;
            }
        }

        private DirectoryInfo FindFrameworkDirectory()
        {
            // Is it parallel to the appserver execute directory?
            DirectoryInfo searchDir = new DirectoryInfo(RootPath);
            searchDir = searchDir.Parent;
            DirectoryInfo[] dirs = searchDir.GetDirectories("Framework");
            if(dirs == null) { return null; }
            if(dirs.Length != 1) { return null; }
            return dirs[0];
        }

        private DirectoryInfo FindFrameworkVersionDirectory()
        {
            DirectoryInfo fwDir = FrameworkDir;

            // Navigate to latest version dir
            float version = 0;
            DirectoryInfo[] dirs = fwDir.GetDirectories();
            foreach(DirectoryInfo dir in dirs)
            {
                try
                {
                    float currVer = float.Parse(dir.Name);
                    if(currVer > version)
                    {
                        version = currVer;
                        fwDir = dir;
                    }
                }
                catch { }
            }
            return fwDir;
        }

        #endregion

        #endregion

		#region MashalByRefObject Implementation

		public override object InitializeLifetimeService()
		{
			return null;
		}

		#endregion

        #region XML header support

        public bool ShouldIncludeXMLHeader()
        {
            string includeXMLHeaderStr = Metreos.Configuration.AppConfig.GetEntry("IncludeXMLHeader");
            bool includeXMLHeader = true;
            try { includeXMLHeader = System.Convert.ToBoolean(includeXMLHeaderStr); }
            catch { }
            return includeXMLHeader;
        }

        #endregion
    }   
}
