using System;
using System.Diagnostics;

using Metreos.Interfaces;

namespace Metreos.Core.ConfigData
{
	/// <summary>An alarm and its metadata</summary>
	public class AlarmData
	{
		private readonly string guid;
		public string Guid { get { return guid; } }

        private uint alarmCode;
        public uint AlarmCode { get { return alarmCode; } }

		private readonly string description;
		public string Description { get { return description; } }

		private readonly string createdTimeStamp;
		public string CreatedTimeStamp { get { return createdTimeStamp; } }

        private readonly IConfig.Severity severity;
        public IConfig.Severity Severity { get { return severity; } }

		private string recoveredTimeStamp = null;
		public string RecoveredTimeStamp { get { return recoveredTimeStamp; } }

        public AlarmData(uint alarmCode, string description, 
            string createdTimeStamp, IConfig.Severity severity)
		{
            this.alarmCode = alarmCode;
			this.description = description;
			this.createdTimeStamp = createdTimeStamp;
			this.severity = severity;

			this.guid = CreateGuid(alarmCode, description, severity);
		}

        public void SetRecovered(string recoveredTS)
        {
            // Only apply the change if this alarm has never before been recovered
            //   and the proposed timestamp is reasonably valid
            if (recoveredTimeStamp == null &&
                recoveredTS != null)
            {
                // Change the alarm code to the cleared version of the triggered one
                alarmCode += IStats.AlarmClearedIdOffset;

                this.recoveredTimeStamp = recoveredTS;
            }
        }

		public override bool Equals(object obj)
		{
			AlarmData alarm = obj as AlarmData;
			if(alarm != null)
				return Guid == alarm.Guid;
			return false;
		}

		public override int GetHashCode()
		{
			return guid.GetHashCode();
		}

        #region Static methods

        public static string CreateGuid(uint alarmCode, string description, IConfig.Severity severity)
        {
            return String.Format("{0}.{1}.{2}", alarmCode, description, severity.ToString());
        }
        #endregion
	}
}
