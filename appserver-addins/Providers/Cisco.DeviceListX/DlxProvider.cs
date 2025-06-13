using System;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Core;
using Metreos.Core.ConfigData;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.ProviderFramework;
using Metreos.Utilities;

using Package = Metreos.Interfaces.PackageDefinitions.CiscoDeviceListX;

namespace Metreos.Providers.CiscoDeviceListX
{
    [ProviderDecl(Package.Globals.PACKAGE_NAME)]
    [PackageDecl(Package.Globals.NAMESPACE, Package.Globals.PACKAGE_DESCRIPTION)]
    public class CiscoDeviceListX : ProviderBase
    { 
        // Number of milliseconds to wait for refresh thread to yield
        private const int REFRESH_THREAD_WAIT   = 15000;   

        private abstract class DbNames
        {
            public const string TABLE_NAME          = "device_info";
            public const string FIELD_TYPE          = "type";
            public const string FIELD_NAME          = "name";
            public const string FIELD_DESCRIPTION   = "description";
            public const string FIELD_SS            = "search_space";
            public const string FIELD_POOL          = "pool";
            public const string FIELD_IP            = "ip";
            public const string FIELD_STATUS        = "status";
            public const string FIELD_CCMIP         = "ccmip";
        }

        // Definitions
        private const string DB_NAME            = "CiscoDeviceListX";

        // Config value names
        private const string INTERVAL_NAME      = "PollInterval";

        // Default values
        private const int DEFAULT_INTERVAL          = 120;               // minutes  
        private const string DEFAULT_STATUS         = "0";
        private const string DEFAULT_POOL           = "Unknown";
        private const string DEFAULT_TYPE           = "1"; //unknown
        
        // SQL create
        private const string SQL_CREATE = @"CREATE TABLE device_info (
            type            int(2),
            name            varchar(100) NOT NULL default '',
            description     varchar(255) NOT NULL default '',
            search_space    varchar(255) NOT NULL default '',
            pool            varchar(255) NOT NULL default '',
            ip              varchar(255) NOT NULL default '',
            status          varchar(2)  NOT NULL default '',
            ccmip           varchar(100) NOT NULL default '',
            PRIMARY KEY (name, ccmip)
            );";

        // variables
        private ArrayList servers;          // DeviceListXReader objects
        private Timer refreshTimer;
        private AsyncAction manualRefreshAction;
        private TimeSpan interval;        
        private ManualResetEvent refreshThreadDormant;
        private bool shutdownFlag = false;
        private volatile bool started = false;
        
        public CiscoDeviceListX(IConfigUtility configUtility) 
            : base(typeof(CiscoDeviceListX), Package.Globals.DISPLAY_NAME, configUtility) 
        {
            // Set our certificate policy
            ServicePointManager.ServerCertificateValidationCallback = 
                new System.Net.Security.RemoteCertificateValidationCallback(MetreosCertificatePolicy.ValidateCertificate);

            this.servers = ArrayList.Synchronized(new ArrayList());
            refreshThreadDormant = new ManualResetEvent(true);
        }

        protected override bool Initialize(out ConfigEntry[] configItems, out Extension[] extensions)
        {
            // Declare actions
            this.messageCallbacks.Add(Package.Actions.Refresh_Sync.FULLNAME, 
                new HandleMessageDelegate(this.OnRefresh));

            // Declare config settings
            configItems = new ConfigEntry[1];
            configItems[0] = new ConfigEntry(INTERVAL_NAME, "Poll Interval", DEFAULT_INTERVAL, 
                "CallManager poll interval (min)", IConfig.StandardFormat.Number, true);
            
            // Declare refresh as an OAM extension
            extensions = new Extension[1];
            extensions[0] = new Extension(Package.Actions.Refresh_Sync.FULLNAME, "Refreshes the DeviceListX cache");

            if(configUtility.DatabaseExists(DB_NAME) == false)
            {
                try
                {
                    configUtility.DatabaseCreate(DB_NAME);
                    using(IDbConnection dbConn = configUtility.DatabaseConnect(DB_NAME))
                    {
                        using(IDbCommand command = dbConn.CreateCommand())
                        {
                            command.CommandText = SQL_CREATE; 
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch(Exception e) 
                {
                    log.Write(TraceLevel.Error, "Database Error: " + e.Message);
                    return false;
                }
            }

            return true;
        }

        protected override void OnStartup()
        {
            this.RegisterNamespace();

            started = true;
            OnRefresh(null);
        }

        protected override void OnShutdown()
        {
            shutdownFlag = true;
            started      = false;

            if(refreshTimer != null)
            {
                refreshTimer.Dispose();
                refreshTimer = null;
            }

            if(refreshThreadDormant != null)
            {
                if(refreshThreadDormant.WaitOne(REFRESH_THREAD_WAIT, false) == false)
                {
                    log.Write(TraceLevel.Error, "Refresh thread did not stop gracefully.");
                }
            }
        }

        protected override void HandleNoHandler(ActionMessage noHandlerAction, EventMessage originalEvent)
        {
            log.Write(TraceLevel.Warning, "{0} event was not handled", originalEvent.MessageId); 
        }

        public override void Cleanup()
        {
            base.Cleanup();
        }

        protected override void RefreshConfiguration()
        {
            // Start the refresh timer
            interval = new TimeSpan(0, Convert.ToInt32(this.GetConfigValue(INTERVAL_NAME)), 0);

            if(interval.TotalMinutes < 1)
            {
                log.Write(TraceLevel.Warning, 
                    "Poll interval must be at least 1 minute. Setting to default: " + DEFAULT_INTERVAL);
                interval = new TimeSpan(0, DEFAULT_INTERVAL, 0);
            }

            if(started)
                OnRefresh(null);
        }

        // The refresh action can be either sync or async.
        // Using dummy functions for documentation.
        [Action(Package.Actions.Refresh_Async.FULLNAME, false, Package.Actions.Refresh_Async.DISPLAY, Package.Actions.Refresh_Async.DESCRIPTION, true)]
        private void Dummy1() {}

        [Event(Package.Events.Refresh_Complete.FULLNAME, null, Package.Events.Refresh_Complete.DISPLAY, Package.Events.Refresh_Complete.DESCRIPTION, true)]
        private void Dummy2() {}

        [Event(Package.Events.Refresh_Failed.FULLNAME, null, Package.Events.Refresh_Failed.DISPLAY, Package.Events.Refresh_Failed.DESCRIPTION, false)]
        private void Dummy3() {}

        [Action(Package.Actions.Refresh_Sync.FULLNAME, false, Package.Actions.Refresh_Sync.DISPLAY, Package.Actions.Refresh_Sync.DESCRIPTION, false)]
        protected void OnRefresh(ActionBase action)
        {
            Hashtable oldClusters = new Hashtable();

            foreach(DeviceListXReader reader in servers)
            {
                oldClusters.Add(reader.Cluster.PublisherIP, reader);
            }

            servers.Clear();

            // Get the list of publisher addresses
            CallManagerCluster[] clusters = configUtility.GetCallManagerClusters();
            if((clusters == null) || (clusters.Length == 0)) 
            { 
                log.Write(TraceLevel.Warning, "No CallManager clusters configured. DeviceListX cache disabled");
            }
            else
            {
                Hashtable newClusters = new Hashtable();

                foreach(CallManagerCluster cluster in clusters)
                {
                    if(!cluster.IsValid())
                        continue;

                    if (cluster.Version >= 5.0) 
                    {
                        SNMPReader snmpReader = new SNMPReader(log, cluster);
                        servers.Add(snmpReader);
                    }
                    else
                    {
                        HttpDlxReader httpReader = new HttpDlxReader(log, cluster);
                        servers.Add(httpReader);
                    }

                    if(!oldClusters.Contains(cluster.PublisherIP))
                    {
                        log.Write(TraceLevel.Info, "Adding '{0} v{1}' to DeviceListX cache list", 
                            cluster.PublisherIP, cluster.Version);
                    }

                    newClusters.Add(cluster.PublisherIP, cluster);
                }

                // Log the clusters which were removed
                foreach(IPAddress addr in oldClusters.Keys)
                {
                    if(!newClusters.Contains(addr))
                    {
						DeviceListXReader oldReader = oldClusters[addr] as DeviceListXReader;
						if (oldReader == null)
						{
							log.Write(TraceLevel.Warning, "Could not remove reader object.");
							continue;
						}

                        CallManagerCluster c = oldReader.Cluster as CallManagerCluster;
                        log.Write(TraceLevel.Info, "Removing '{0} v{1}' from DeviceListX cache list", 
                            c.PublisherIP, c.Version);
                    }
                }
            }

            if(refreshTimer == null)
            {
                // Start refresh right away (Startup)
                refreshTimer = 
                    new Timer(new TimerCallback(RefreshDeviceInformation), null, TimeSpan.Zero, interval);
            }
            else if(action == null)
            {
                // Wait until next interval because many refreshes could occur while configuring system.
                refreshTimer.Change(interval, interval);
            }
            else
            {
                log.Write(TraceLevel.Info, "Application forced Cisco device cache refresh.");
                if(refreshThreadDormant.WaitOne(10, false))
                {
                    refreshTimer.Change(TimeSpan.Zero, interval);
                }
                else
                {
                    log.Write(TraceLevel.Warning, "Another refresh is currently executing. The manual refresh request has been dropped.");
                }
                action.SendResponse(true);

                if(action is AsyncAction)
                {
                    manualRefreshAction = action as AsyncAction;
                }
            }
        }

        protected void RefreshDeviceInformation(object state)
        {
            refreshThreadDormant.Reset();

            IDbConnection dbConn = configUtility.DatabaseConnect(DB_NAME);
            if(dbConn == null)
            {
                log.Write(TraceLevel.Error, "Could not open database");
                refreshThreadDormant.Set();
                return;
            }

            using(dbConn)
            {
                log.Write(TraceLevel.Info, "Device list cache refresh starting.");
           
                ArrayList exemptionList = new ArrayList();
                Hashtable deviceLists = new Hashtable();
                foreach(DeviceListXReader dlxReader in new ArrayList(servers))
                {
                    if(shutdownFlag) 
                    {
                        refreshThreadDormant.Set();
                        return; 
                    }

                    if (dlxReader.Initialize())
                    {
                        if (dlxReader.RetrieveDeviceList(ref shutdownFlag))
                        {
                            deviceLists.Add(dlxReader.ClusterIP, dlxReader.Data);
                            continue;
                        }
                    }

                    exemptionList.Add(dlxReader.ClusterIP);
                }

                if(((deviceLists.Count == 0) || (servers.Count == exemptionList.Count)) && !shutdownFlag)
                {
                    if(servers.Count > 0)
                        log.Write(TraceLevel.Error, "No DeviceListX data retreived");

                    refreshThreadDormant.Set();
                    return; 
                }

                log.Write(TraceLevel.Verbose, "Fetching existing device data");

                // Get existing keys
                // Device name -> Device info
                Hashtable entries = GetKeyList(exemptionList);

                if(shutdownFlag)
                {
                    refreshThreadDormant.Set();
                    return;
                }

                if(entries != null)
                {
                    log.Write(TraceLevel.Verbose, "Determining changes to the data");

                    IDictionaryEnumerator dataIterator = deviceLists.GetEnumerator();

                    while(dataIterator.MoveNext())
                    {
                        string ccmIP = dataIterator.Key as string;
                        DeviceList data = dataIterator.Value as DeviceList;

                        if(data.Items == null) { continue; }

                        // Remove matching keys from both lists
                        for(int i=0; i<data.Items.Length; i++)
                        {
                            DeviceListDevice DeviceListXDevice = data.Items[i];
                            // ccmip is not initialized at this point for individual deserialized devices
                            // i.e., one can not do device.ID or device.ccmIP
                            string deviceID = DeviceListXDevice.MakeID(ccmIP); 

                            if(entries.Contains(deviceID))
                            {
                                // Make sure none of the fields have been updated
                                DeviceListDevice dbDeviceRecord = (DeviceListDevice) entries[deviceID];
                    
                                // Remove from delete list
                                entries.Remove(deviceID);

                                if( (dbDeviceRecord.type        == DeviceListXDevice.type)        &&
                                    (dbDeviceRecord.description == DeviceListXDevice.description) &&
                                    (dbDeviceRecord.css         == DeviceListXDevice.css)         &&
                                    (dbDeviceRecord.pool        == DeviceListXDevice.pool)        &&
                                    (dbDeviceRecord.ip          == DeviceListXDevice.ip)          &&
                                    (dbDeviceRecord.status      == DeviceListXDevice.status))
                                {
                                    // Remove from insert/replace list
                                    data.Items[i] = null;
                                }
                            }

                            if(shutdownFlag)
                            {
                                refreshThreadDormant.Set();
                                return; 
                            }
                        }
                    }

                    // Delete any existing keys that 
                    //   were not returned from the last poll
                    DeleteByName(entries.Keys);
                }

                log.Write(TraceLevel.Verbose, "Creating SQL commands");

                // Insert/update data in the database
                StringCollection inserts = CreateSqlInsert(deviceLists);
                
                if(shutdownFlag) 
                {
                    refreshThreadDormant.Set();
                    return; 
                }

                log.Write(TraceLevel.Verbose, "Entering DB write loop");

                if(inserts != null)
                {
                    using(IDbCommand command = dbConn.CreateCommand())
                    {
                        for(int i=0; i<inserts.Count; i++)
                        {
                            Thread.Sleep(1);   // throttle

                            try 
                            {
                                command.CommandText = inserts[i];
                                command.ExecuteNonQuery();
                            }
                            catch(Exception e) 
                            {
                                log.Write(TraceLevel.Error, "Error updating database: " + e.Message);
                                break;
                            }

                            if(shutdownFlag) 
                            {
                                refreshThreadDormant.Set();
                                return; 
                            }
                        }
                    }            
                }

                if(manualRefreshAction != null)
                {
                    palWriter.PostMessage(manualRefreshAction.CreateAsyncCallback(true));
                    manualRefreshAction = null;
                }

                log.Write(TraceLevel.Info, "Device list cache refresh complete.");

                refreshThreadDormant.Set();
            }
        }

        private void DeleteByName(ICollection ids)
        {
            if(ids == null) { return; }
            if(ids.Count == 0) { return; }

            SqlBuilder removeCmd = new SqlBuilder(SqlBuilder.Method.DELETE);
            removeCmd.table = DbNames.TABLE_NAME;

            IEnumerator de = ids.GetEnumerator();

            using(IDbConnection dbConn = configUtility.DatabaseConnect(DB_NAME))
            {
                while(de.MoveNext())
                {
                    string ccmIP;
                    string deviceName;
                    if(DeviceListDevice.SplitID(de.Current as String, out ccmIP, out deviceName))
                    {
                        removeCmd.where.Clear();
                        removeCmd.where.Add(DbNames.FIELD_NAME, deviceName);
                        removeCmd.where.Add(DbNames.FIELD_CCMIP, ccmIP);

                        try
                        {
                            using(IDbCommand command = dbConn.CreateCommand())
                            {
                                command.CommandText = removeCmd.ToString();
                                command.ExecuteNonQuery();
                            }
                        }
                        catch(Exception e)
                        {
                            log.Write(TraceLevel.Error, 
                                "Could not remove old entry ({0}) from database. Error: " + e.Message, de.Current);
                        }
                    }
                    else
                    {
                        log.Write(TraceLevel.Error, 
                            "Could not remove old entry ({0}) from database.  The key was invalid");
                    }
                }
            }
        }

        private Hashtable GetKeyList(ArrayList exemptionList)
        {
            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.SELECT);
            sql.table = DbNames.TABLE_NAME;
            sql.appendSemicolon = false;

            string query = sql.ToString();
            if(exemptionList.Count > 0)
            {
                // SqlBuilder does not support != WHERE clauses

                StringBuilder exemptionSql = new StringBuilder(query);
                exemptionSql.Append(" WHERE ");
                // Don't pull db records for CCMs that are exempt
                foreach(string ccmIP in exemptionList)
                {
                    exemptionSql.AppendFormat("{0} != '{1}' AND ", DbNames.FIELD_CCMIP, ccmIP);
                }

                exemptionSql.Remove(exemptionSql.Length-5, 5);

                query = exemptionSql.ToString();
            }                   


            DataTable table = null;
            try 
            {
                using(IDbConnection dbConn = configUtility.DatabaseConnect(DB_NAME))
                {
                    using(IDbCommand command = dbConn.CreateCommand())
                    {
                        command.CommandText = query;
                        IDataReader reader = command.ExecuteReader();
                        table = Database.GetDataTable(reader, true, ref shutdownFlag);
                        reader.Close();
                    }
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Could not retrieve key list from database. Error: " + e.Message);
                return null;
            }

            if((table == null) || (table.Rows.Count == 0))
            {
                log.Write(TraceLevel.Info, "No keys found in database during periodic check.");
                return null;
            }

            DataRowCollection rows = table.Rows;

            Hashtable entries = new Hashtable();
            DeviceListDevice device = null;

            for(int i=0; i<rows.Count; i++)
            {
                DataRow row = rows[i];
                device = new DeviceListDevice();
                device.type         = row[DbNames.FIELD_TYPE].ToString();
                device.name         = (string) row[DbNames.FIELD_NAME];
                device.description  = (string) row[DbNames.FIELD_DESCRIPTION];
                device.css          = (string) row[DbNames.FIELD_SS];
                device.pool         = (string) row[DbNames.FIELD_POOL];
                device.ip           = (string) row[DbNames.FIELD_IP];
                device.status       = (string) row[DbNames.FIELD_STATUS];
                device.ccmIP        = (string) row[DbNames.FIELD_CCMIP]; 
                
                try 
                { 
                    entries.Add(device.ID, device); 
                }
                catch 
                {
                    log.Write(TraceLevel.Error, "DeviceListX database corrupt. Multiple entries encountered with the same name.");
                    break;
                }
            }

            return entries;
        }

        private StringCollection CreateSqlInsert(Hashtable deviceLists)
        {
            // Verify parameter
            if(deviceLists == null)     { return null; }
            if(deviceLists.Count == 0)  { return null; }

            SqlBuilder sql = new SqlBuilder(SqlBuilder.Method.REPLACE);
            sql.table = DbNames.TABLE_NAME;
            sql.fieldNames.Add(DbNames.FIELD_TYPE);
            sql.fieldNames.Add(DbNames.FIELD_NAME);
            sql.fieldNames.Add(DbNames.FIELD_DESCRIPTION);
            sql.fieldNames.Add(DbNames.FIELD_SS);
            sql.fieldNames.Add(DbNames.FIELD_POOL);
            sql.fieldNames.Add(DbNames.FIELD_IP);
            sql.fieldNames.Add(DbNames.FIELD_STATUS);
            sql.fieldNames.Add(DbNames.FIELD_CCMIP);

            StringCollection commands = new StringCollection();

            IDictionaryEnumerator deviceIterator = deviceLists.GetEnumerator();
            while(deviceIterator.MoveNext())
            {
                string ccmIP = deviceIterator.Key as string;
                DeviceList devices = deviceIterator.Value as DeviceList;

                if(devices == null || devices.Items == null)
                    continue;

                for(int i=0; i<devices.Items.Length; i++)
                {
                    Thread.Sleep(1);  // throttle

                    if(shutdownFlag)
                        return null; 

                    DeviceListDevice device = devices.Items[i];

                    if(device == null || device.name == null) { continue; }
                    
                    if(device.description == null)
                        device.description = string.Empty;
                    if(device.pool == null)
                        device.pool        = DEFAULT_POOL;
                    if(device.ip == null)
                        device.ip          = string.Empty;
                    if(device.css == null)
                        device.css         = string.Empty;
                    if(device.status == null)
                        device.status      = DEFAULT_STATUS;
                    if(device.type == null)
                        device.type        = DEFAULT_TYPE;

                    sql.fieldValues.Clear();
                    sql.fieldValues.Add(device.type);
                    sql.fieldValues.Add(device.name);
                    sql.fieldValues.Add(device.description);
                    sql.fieldValues.Add(device.css);
                    sql.fieldValues.Add(device.pool);
                    sql.fieldValues.Add(device.ip);
                    sql.fieldValues.Add(device.status);
                    sql.fieldValues.Add(ccmIP);

                    commands.Add(sql.ToString());
                }
            }

            return commands;
        }
	}
}
