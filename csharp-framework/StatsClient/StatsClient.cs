using System;
using System.Net;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Core.IPC.Flatmaps;
using Metreos.Interfaces;

namespace Metreos.Stats
{
    public delegate void AlarmAckDelegate(string guid);
    public delegate void StatAckDelegate(int oid);
    public delegate void StatNackDelegate(string errorText);

	/// <summary>Sends alarms and stats to the alarm service</summary>
	public sealed class StatsClient : IDisposable
	{
        public event AlarmAckDelegate OnAlarmAck;
        public event StatAckDelegate OnStatAck;
        public event StatNackDelegate OnStatNack;

		private readonly IpcFlatmapClient client;		        // TCP connection to alarm service
		
        #region Singleton interface

        private static volatile StatsClient instance = null;
		private static Object newInstanceSync = new Object();

        public static StatsClient Instance
		{
			get
			{
				lock(newInstanceSync)                           // Grab the instance lock
				{
					if(instance == null)                        // Has it already been created?
					{
                        instance = new StatsClient();           // Create the singleton instance
					}
				}

				return instance;
			}
		}

        private StatsClient()
        {
            IPEndPoint agentEP = new IPEndPoint(IStats.StatsListener.Defaults.Interface, IStats.StatsListener.Defaults.Port);
            client = new IpcFlatmapClient(agentEP);
            client.onFlatmapMessageReceived += new OnFlatmapMessageReceivedDelegate(OnIpcMessageReceived);
            client.Start();
        }

        public void Dispose()
        {
            client.Close();
            client.Dispose();

            instance = null;
        }
		#endregion

		/// <summary>Trigger a new alarm</summary>
		/// <param name="severity">severity level</param>
		/// <param name="msg">code message with embedded error code</param>
		public string TriggerAlarm(IConfig.Severity severity, uint alarmCode, 
			string description, params object[] args)
		{
            if(severity == IConfig.Severity.Unspecified || alarmCode == 0 || description == null)
				return null;

			if(args != null && args.Length > 0)
			{
				description = String.Format(description, args);
			}

			string timeStamp = DateTime.Now.ToString(IStats.TimestampFormat);
            string guid = Metreos.Core.ConfigData.AlarmData.CreateGuid(alarmCode, description, severity);

            FlatmapList map = new FlatmapList();
            map.Add(IStats.StatsListener.Messages.Fields.AlarmCode, alarmCode.ToString());
            map.Add(IStats.StatsListener.Messages.Fields.Description, description);
            map.Add(IStats.StatsListener.Messages.Fields.Severity, severity.ToString());
            map.Add(IStats.StatsListener.Messages.Fields.Timestamp, timeStamp);
            map.Add(IStats.StatsListener.Messages.Fields.Guid, guid);

            if(!client.Write(IStats.StatsListener.Messages.NewEvent, map))
                return null;

            return guid;
		}

		/// <summary>Clear alarm entry by GUID</summary>
		/// <param name="guid"></param>
		public bool ClearAlarm(string guid)
		{
			if(guid == null || guid == String.Empty)
				return false;

			DateTime d = DateTime.Now;
			
			string timeStamp = d.ToString(IStats.TimestampFormat);

            FlatmapList map = new FlatmapList();
            map.Add(IStats.StatsListener.Messages.Fields.Guid, guid);
            map.Add(IStats.StatsListener.Messages.Fields.ClearedTimestamp, timeStamp);

            return client.Write(IStats.StatsListener.Messages.ClearEvent, map);
        }

        public void SetStatistic(int oid, object statValue)
        {
            FlatmapList map = new FlatmapList();
            map.Add(IStats.StatsListener.Messages.Fields.Oid, oid);
            map.Add(IStats.StatsListener.Messages.Fields.Value, statValue);

            client.Write(IStats.StatsListener.Messages.SetStatistic, map);
        }

		#region Incoming message handler

		public void OnIpcMessageReceived(IpcFlatmapClient client, int messageType, FlatmapList message)
		{
            if(messageType == IStats.StatsListener.Messages.EventAccepted)
			{
                if(OnAlarmAck != null)
                {
                    string guid = message.Find(IStats.StatsListener.Messages.Fields.Guid, 1).dataValue as string;
                    if(guid != null)
                        OnAlarmAck(guid);
                }
			}
            else if(messageType == IStats.StatsListener.Messages.StatAck)
            {
                if(OnStatAck != null)
                {
                    int oid = Convert.ToInt32(message.Find(IStats.StatsListener.Messages.Fields.Oid, 1).dataValue);
                    OnStatAck(oid);
                }
            }
            else if(messageType == IStats.StatsListener.Messages.StatNack)
            {
                if(OnStatNack != null)
                {
                    string errorText = message.Find(IStats.StatsListener.Messages.Fields.ErrorText, 1).dataValue as string;
                    OnStatNack(errorText);
                }
            }
		}
		#endregion
    }
}
