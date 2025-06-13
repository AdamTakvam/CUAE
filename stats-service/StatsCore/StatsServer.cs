using System;
using System.IO;
using System.Diagnostics;
using System.Xml.Serialization;

using Metreos.Interfaces;
using Metreos.Configuration;
using Metreos.Core.IPC.Xml;
using Metreos.Core.IPC.Flatmaps;
using Metreos.LoggingFramework;
using Metreos.LogSinks;
using Metreos.Utilities;

using Metreos.Stats.Alarms;

namespace Metreos.Stats
{
    public class StatsServer : Loggable, IDisposable
    {
        public const string Name = "StatsServer";

        private readonly Config config;

        /// <summary>Listens for connections from management client (mceadmin)</summary>
        private readonly IpcXmlServer mgmtServer;

        /// <summary>Listens for the connection from SNMP agent</summary>
        private readonly IpcFlatmapServer snmpServer;

        /// <summary>Listens for connections from stats clients</summary>
        private readonly IpcFlatmapServer statsServer;

        /// <summary>Sends alarms</summary>
        private readonly AlarmAgent alarmAgent;

        /// <summary>Polls stats periodically and stores in logarithmic database</summary>
        private readonly Archiver archiver;
        
        /// <summary>TraceListener for writing to Log Service</summary>
        private readonly LogServerSink logServerSink;

        /// <summary>In-memory table of current statistics data</summary>
        private readonly StatTable stats;

        private readonly object refreshLock;

        #region Construction/RefreshConfig/Dispose

        public StatsServer()
            : base(TraceLevel.Verbose, Name)
        {
            this.config = Config.Instance;
            this.logServerSink = CreateLogSink();

            this.alarmAgent = new AlarmAgent();
            this.stats = new StatTable();
            this.refreshLock = new object();

            // Create mgmt server socket
            this.mgmtServer = new IpcXmlServer(Name, IStats.MgmtListener.Defaults.Port, true, log.LogLevel);
            this.mgmtServer.OnMessageReceived += new IpcXmlServer.OnMessageReceivedDelegate(this.OnMgmtMessageReceived);
            this.mgmtServer.Start();

            // Create stats server socket
            this.statsServer = new IpcFlatmapServer(Name, IStats.StatsListener.Defaults.Port, true, log.LogLevel);
            this.statsServer.OnMessageReceived += new IpcFlatmapServer.OnMessageReceivedDelegate(this.OnStatsMessageReceived);
            this.statsServer.OnNewConnection += new Metreos.Core.Sockets.NewConnectionDelegate(OnStatsNewConnection);
            this.statsServer.Start();

            // Create SNMP agent server socket
            this.snmpServer = new IpcFlatmapServer(Name, IStats.SnmpListener.Defaults.Port, true, log.LogLevel);
            this.snmpServer.OnMessageReceived += new IpcFlatmapServer.OnMessageReceivedDelegate(this.OnSnmpAgentMessageReceived);
            this.snmpServer.OnNewConnection += new Metreos.Core.Sockets.NewConnectionDelegate(OnSnmpAgentNewConnection);
            this.snmpServer.Start();

            // Create archiver
            this.archiver = new Archiver();
            this.archiver.GetStats = new GetStatsDelegate( delegate() { return stats; } );
            this.archiver.Start();
        }

        private void RefreshConfiguration()
        {
            // One at a time, please...
            lock(refreshLock)
            {
                logServerSink.RefreshConfiguration();
                alarmAgent.RefreshConfiguration();
            }
        }

        public override void Dispose()
        {
            archiver.Stop();
            archiver.Dispose();

            mgmtServer.Stop();
            mgmtServer.Dispose();

            statsServer.Stop();
            statsServer.Dispose();

            snmpServer.Stop();
            snmpServer.Dispose();

            alarmAgent.Dispose();

            logServerSink.Dispose();

            base.Dispose();
        }
        #endregion

        #region IPC Message from Stats Client

        private void OnStatsNewConnection(int socketId, string remoteHost)
        {
            log.Write(TraceLevel.Info, "Stats client connected: " + socketId);
        }

        /// <summary>Handles IPC messages from Stats Client</summary>
        /// <param name="receiveInterface"></param>
        /// <param name="messageType"></param>
        /// <param name="message"></param>
        private void OnStatsMessageReceived(int socketId, string receiveInterface, int messageType, FlatmapList message)
        {
            log.Write(TraceLevel.Verbose, "Received IPC message from " + receiveInterface);

            if(messageType == IStats.StatsListener.Messages.SetStatistic)
            {
                int oid = 0;
                object oidObj = message.Find(IStats.StatsListener.Messages.Fields.Oid, 1).dataValue;
                if(oidObj == null)
                {
                    SendStatNack(socketId, "No OID specified");
                    return;
                }

                try { oid = Convert.ToInt32(oidObj); }
                catch 
                {
                    SendStatNack(socketId, "Invalid OID: " + oidObj);
                    return; 
                }

                object statValueObj = message.Find(IStats.StatsListener.Messages.Fields.Value, 1).dataValue;
                if(statValueObj == null)
                {
                    SendStatNack(socketId, "No stat value specified");
                    return;
                }

                long statValue;
                try { statValue = Convert.ToInt64(statValueObj); }
                catch
                {
                    SendStatNack(socketId, "Invalid statistic value: " + statValueObj);
                    return;
                }

                stats.Add(oid, statValue);

                // Send ACK
                statsServer.Write(socketId, IStats.StatsListener.Messages.StatAck, message);
            }
            else if(messageType == IStats.StatsListener.Messages.NewEvent)
            {
                string guid = null;
                try
                {
                    guid = alarmAgent.RaiseAlarm(message);
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Error raising alarm: " + Exceptions.FormatException(e));                    
                }

                FlatmapList flatmap = new FlatmapList();
                if(guid != null)
                {
                    flatmap.Add(IStats.StatsListener.Messages.Fields.Guid, guid);
                    log.Write(TraceLevel.Verbose, "Sent ACK to client for alarm GUID: " + guid);
                }

                statsServer.Write(socketId, IStats.StatsListener.Messages.EventAccepted, flatmap);
            }
            else if(messageType == IStats.StatsListener.Messages.ClearEvent)
            {
                string guid = null;
                try
                {
                    guid = alarmAgent.ClearAlarm(message);
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Error clearing alarm: " + Exceptions.FormatException(e));
                }

                FlatmapList flatmap = new FlatmapList();
                if(guid != null)
                {
                    flatmap.Add(IStats.StatsListener.Messages.Fields.Guid, guid);
                    log.Write(TraceLevel.Verbose, "Sent ACK to client for cleared alarm GUID: " + guid);
                }

                statsServer.Write(socketId, IStats.StatsListener.Messages.EventCleared, flatmap);
            }
        }
        #endregion

        #region IPC Message from SNMP Agent

        private void OnSnmpAgentNewConnection(int socketId, string remoteHost)
        {
            log.Write(TraceLevel.Info, "SNMP Agent connected: " + socketId);
        }

        /// <summary>Handles IPC messages from SNMP Agent</summary>
        /// <param name="receiveInterface"></param>
        /// <param name="messageType"></param>
        /// <param name="message"></param>
        private void OnSnmpAgentMessageReceived(int socketId, string receiveInterface, int messageType, FlatmapList message)
        {
            log.Write(TraceLevel.Verbose, "Got stat request from SNMP agent for OID: {0}", messageType);

            long Value = stats.GetCurrentValue(messageType);

            //New flatmaplist
            FlatmapList oMessage = new FlatmapList();
            oMessage.Add((uint)messageType, Value);   // messageType?  - APC
            //oMessage.Add(IStats.SnmpListener.Messages.Fields.Value, Value);

            snmpServer.Write(socketId, messageType, oMessage);

            log.Write(TraceLevel.Verbose, "Sent response to SNMP Agent for OID {0}: {1}", messageType, Value);
        }
        #endregion

        #region IPC Message from Management Client

        /// <summary>Handles IPC messages from Management Client</summary>
        /// <param name="receiveInterface"></param>
        /// <param name="messageType"></param>
        /// <param name="message"></param>
        private void OnMgmtMessageReceived(int socketId, string receiveInterface, string dataStr)
        {
            commandType command = null;
            try
            {
                XmlSerializer commandSerializer = new XmlSerializer(typeof(commandType));
                StringReader reader = new StringReader(dataStr);
                command = (commandType) commandSerializer.Deserialize(reader);
            }
            catch
            {
                log.Write(TraceLevel.Warning, "Received invalid command:\n" + dataStr);
                return;
            }

            if(log.LogLevel == TraceLevel.Info)
                log.Write(TraceLevel.Info, "Got management command: " + command.name);
            else if(log.LogLevel == TraceLevel.Verbose)
                log.Write(TraceLevel.Verbose, "Got management command:\r\n" + dataStr);

            switch(command.name)
            {
                case ICommands.REFRESH_CONFIG:
                    // Send response first because Refresh can possibly take a long time
                    SendMgmtResponse(socketId, IConfig.Result.Success, null);
                    RefreshConfiguration();
                    break;
                case IStats.MgmtListener.Commands.GenerateGraph:
                    HandleGenerateGraph(socketId, command);
                    break;
                case IStats.MgmtListener.Commands.GetStatistic:
                    int oid;
                    string oidStr = command[IStats.MgmtListener.Commands.Parameters.Oid];
                    try 
                    {
                        oid = int.Parse(oidStr); 
                    }
                    catch
                    {
                        SendMgmtResponse(socketId, IConfig.Result.Failure, "Invalid OID: " + oidStr);
                        return;
                    }
                    SendMgmtResponse(socketId, IConfig.Result.Success, Convert.ToString(stats.GetCurrentValue(oid)));
                    break;
                default:
                    SendMgmtResponse(socketId, IConfig.Result.Failure, null);
                    break;
            }         
        }

        private void HandleGenerateGraph(int socketId, commandType command)
        {
            // Get OID of stat being requested
            string oid = command[IStats.MgmtListener.Commands.Parameters.Oid];
            if(oid == null || oid == String.Empty)
            {
                string error = "No OID specified in GenerateGraph request.";
                log.Write(TraceLevel.Error, error);
                SendMgmtResponse(socketId, IConfig.Result.Failure, error);
                return;
            }

            // See if they requested a horizontal line (license value)
            long lineVal = Convert.ToInt64(command[IStats.MgmtListener.Commands.Parameters.HLine]);

            // Are they requesting a standard interval?
            string intervalStr = command[IStats.MgmtListener.Commands.Parameters.Interval];
            if(intervalStr != null && intervalStr != String.Empty)
            {
                IStats.MgmtListener.Commands.Interval interval;
                try
                {
                    interval = (IStats.MgmtListener.Commands.Interval)
                        Enum.Parse(typeof(IStats.MgmtListener.Commands.Interval), intervalStr, true);
                }
                catch
                {
                    string error = "Invalid interval specified in GenerateGraph request: " + intervalStr;
                    log.Write(TraceLevel.Error, error);
                    SendMgmtResponse(socketId, IConfig.Result.Failure, error);
                    return;
                }

                string imgPath;
                try
                {
                    archiver.GenerateGraph(oid, interval, lineVal, out imgPath);
                    SendMgmtResponse(socketId, IConfig.Result.Success, imgPath);
                }
                catch(ApplicationException e)
                {
                    SendMgmtResponse(socketId, IConfig.Result.Failure, e.Message);
                }
            }
            else
            {
                DateTime startTime;
                DateTime endTime;
                try
                {
                    startTime = GetDateTime(command, IStats.MgmtListener.Commands.Parameters.StartTime);
                    endTime = GetDateTime(command, IStats.MgmtListener.Commands.Parameters.EndTime);
                }
                catch(Exception e)
                {
                    SendMgmtResponse(socketId, IConfig.Result.Failure, e.Message);
                    return;
                }

                string imgPath;
                try
                {
                    archiver.GenerateGraph(oid, startTime, endTime, lineVal, out imgPath);
                    SendMgmtResponse(socketId, IConfig.Result.Success, imgPath);
                }
                catch(ApplicationException e)
                {
                    SendMgmtResponse(socketId, IConfig.Result.Failure, e.Message);
                }
            }
        }

        private DateTime GetDateTime(commandType command, string paramName)
        {
            DateTime time;
            string timeStr = Convert.ToString(command[paramName]);
            if(timeStr == null)
                return DateTime.Now;

            try { time = DateTime.Parse(timeStr); }
            catch { throw new ApplicationException(paramName + " is invalid"); }

            return time;
        }

        private void SendMgmtResponse(int socketId, IConfig.Result resultType, string failureMessage)
        {
            responseType response = new responseType();
            response.type = resultType;
            response.Value = failureMessage;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            StringWriter writer = new StringWriter(sb);
            XmlSerializer responseSerializer = new XmlSerializer(typeof(responseType));
            responseSerializer.Serialize(writer, response);

            if(mgmtServer.Write(socketId, sb.ToString()) == false)
                log.Write(TraceLevel.Warning, "Failed to send response to management command. Socket is closed");
            else
                log.Write(TraceLevel.Info, "Sent '{0}' response to management client", resultType.ToString());
        }
        #endregion

        #region Private helpers

        private LogServerSink CreateLogSink()
        {
            ushort port = Config.LogService.ListenPort;
            if(port == 0)
                port = IServerLog.Default_Port;

            // Self-Registers with TraceLister subsystem
            LogServerSink lss = new LogServerSink(Name, port, GetLogServerLevel());
            lss.GetLogLevel = new LogLevelQueryDelegate(GetLogServerLevel);
            return lss;
        }

        private TraceLevel GetLogServerLevel()
        {
            return Config.StatsService.LogLevel;
        }

        private void SendStatNack(int socketId, string errorText)
        {
            FlatmapList map = new FlatmapList();
            map.Add(IStats.StatsListener.Messages.Fields.ErrorText, errorText);
            statsServer.Write(socketId, IStats.StatsListener.Messages.StatNack, map);
        }
        #endregion

        #region Test

        private TimerManager testTimers;

        public void StartTestData()
        {
            StopTestData();
            testTimers = new TimerManager("Test data", new WakeupDelegate(GenerateTestData), null, 1, 1);
            testTimers.Add(0);
        }

        public void StopTestData()
        {
            if(testTimers != null)
                testTimers.Shutdown();
        }

        public void TestGenerateGraph(IStats.MgmtListener.Commands.Interval interval)
        {
            string path = null;
            try
            {
                long lineVal = stats.GetCurrentValue(Convert.ToInt32(Archiver.Consts.Stats[1][0]));
                archiver.GenerateGraph(Archiver.Consts.Stats[1][0], interval, lineVal, out path);
            }
            catch(ApplicationException e)
            {
                log.Write(TraceLevel.Error, e.Message);
            }
        }

        public void TestGenerateGraph(TimeSpan time)
        {
            string path = null;
            DateTime endTime = DateTime.UtcNow;
            DateTime startTime = endTime - time;

            try
            {
                long lineVal = stats.GetCurrentValue(Convert.ToInt32(Archiver.Consts.Stats[0][0]));
                archiver.GenerateGraph(Archiver.Consts.Stats[0][0], startTime, endTime, lineVal, out path);
            }
            catch(ApplicationException e)
            {
                log.Write(TraceLevel.Error, e.Message);
            }
        }

        public void PrintStats()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for(int i=0; i<Archiver.Consts.Stats.Length; i++)
            {
                string statId = Archiver.Consts.Stats[i][0];
                sb.AppendLine(String.Format("{0} ({1}) = {2}", statId, Archiver.Consts.Stats[i][1], stats.GetCurrentValue(Convert.ToInt32(statId))));
            }
            log.Write(TraceLevel.Info, sb.ToString());
        }

        private long GenerateTestData(TimerHandle handle, object state)
        {
            log.Write(TraceLevel.Verbose, "Updating test data");

            for(int i=0; i<Archiver.Consts.Stats.Length; i++)
            {
                string statId = Archiver.Consts.Stats[i][0];
                System.Threading.Thread.Sleep(DateTime.Now.Minute);
                long val = DateTime.Now.Millisecond;
                stats.Add(Convert.ToInt32(statId), val); 
            }

            return Convert.ToInt64(Archiver.Consts.PollInterval.TotalMilliseconds);
        }

        #endregion
    }
}
