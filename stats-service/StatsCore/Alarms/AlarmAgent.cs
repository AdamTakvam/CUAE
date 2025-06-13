using System;
using System.Diagnostics;
using System.Collections;
using System.Net;
using System.Management;

using Metreos.Utilities;
using Metreos.Configuration;
using Metreos.LoggingFramework;
using Metreos.Core.IPC.Flatmaps;
using Metreos.Core.ConfigData;
using Metreos.Interfaces;

namespace Metreos.Stats.Alarms
{
	/// <summary>Sends alarms via AlarmSender implementations</summary>
	internal class AlarmAgent : Loggable, IDisposable
	{
        public const string Name = "AlarmAgent";
  	
        private readonly Config config;         

        /// <summary>AlarmSender implementations</summary>
        private readonly ArrayList alarmSenders;

        /// <summary>Set of alarm codes the admin has chosen to ignore</summary>
        private ArrayList ignoredAlarms;
		
        #region Constructor/RefreshConfig/Dispose

		public AlarmAgent() 
            : base(TraceLevel.Verbose, Name)
		{
			this.config = Config.Instance;
            this.alarmSenders = new ArrayList();

            RefreshConfiguration();

			log.Write(TraceLevel.Info, "Alarm Agent started");
		}

        public void RefreshConfiguration()
        {
            // Create list of ignored alarms
            ignoredAlarms = GetIgnoredAlarms();

            // Dispose of alarm senders
            foreach(AlarmSenderBase sender in alarmSenders)
            {
                sender.Dispose();
            }
            alarmSenders.Clear();

            try
            {
                alarmSenders.Add(new EMailSender(log));
            }
            catch(SenderNotConfiguredException e)
            {
                if(e.InnerException != null)
                    log.Write(TraceLevel.Info, "{0} ({1})", e.Message, e.InnerException.Message);
                else
                    log.Write(TraceLevel.Info, e.Message);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Failed to create SMTP sender: " + Exceptions.FormatException(e));
            }

            try
            {
                alarmSenders.Add(new SnmpTrapSender(log));
            }
            catch(SenderNotConfiguredException e)
            {
                if(e.InnerException != null)
                    log.Write(TraceLevel.Info, "{0} ({1})", e.Message, e.InnerException.Message);
                else
                    log.Write(TraceLevel.Info, e.Message);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Failed to create SNMP sender: " + Exceptions.FormatException(e));
            }
        }

        private ArrayList GetIgnoredAlarms()
        {
            ArrayList ignoreList = new ArrayList();

            SortedList oidHash = config.GetOIDs();
            foreach(DictionaryEntry de in oidHash)
            {
                uint oid = Convert.ToUInt32(de.Key);
                SnmpOid data = de.Value as SnmpOid;

                if(data.Ignore)
                    ignoreList.Add(oid);
            }
            return ignoreList;
        }

		public override void Dispose()
		{
            // Dispose of alarm senders
            foreach(AlarmSenderBase sender in alarmSenders)
            {
                sender.Dispose();
            }
            alarmSenders.Clear();

            base.Dispose();
		}
        #endregion

        #region Alarm Functions

        public string RaiseAlarm(FlatmapList message)
        {
            string alarmCodeStr = message.Find(IStats.StatsListener.Messages.Fields.AlarmCode, 1).dataValue as string;
            uint alarmCode = Convert.ToUInt32(alarmCodeStr);
            string description = message.Find(IStats.StatsListener.Messages.Fields.Description, 1).dataValue as string;

            // Check if this alarm is marked 'ignored'
            if(ignoredAlarms.Contains(alarmCode))
            {
                log.Write(TraceLevel.Verbose, "Ignored alarm {0}: {1}", alarmCode, description);
                return null;
            }

            // Parse severity
            String s = message.Find(IStats.StatsListener.Messages.Fields.Severity, 1).dataValue as string;
            IConfig.Severity severity = IConfig.Severity.Unspecified;
            severity = (IConfig.Severity) Enum.Parse(typeof(IConfig.Severity), s, true);

            string timeStamp = message.Find(IStats.StatsListener.Messages.Fields.Timestamp, 1).dataValue as string;

            AlarmData data = new AlarmData(alarmCode, description, timeStamp, severity);
            if(!config.InsertAlarm(data.AlarmCode, data.Guid, data.Description, data.Severity))
                throw new ApplicationException("Failed to insert alarm data into database");

            SendAlarms(data, false);

            return data.Guid;
        }

        public string ClearAlarm(FlatmapList message)
        {
            // Clean alarm from DB
            string guid = message.Find(IStats.StatsListener.Messages.Fields.Guid, 1).dataValue as string;
            string recoveredTS = message.Find(IStats.StatsListener.Messages.Fields.ClearedTimestamp, 1).dataValue as string;

            AlarmData data = config.ClearAlarm(guid, recoveredTS);

            SendAlarms(data, true);

            return guid;
        }

        private void SendAlarms(AlarmData data, bool cleared)
        {
            foreach(AlarmSenderBase alarmSender in alarmSenders)
            {
                alarmSender.SendAlarm(data, cleared);
            }
        }
        #endregion		
	}
}
