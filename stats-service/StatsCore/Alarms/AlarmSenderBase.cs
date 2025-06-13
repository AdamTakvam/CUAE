using System;

using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Configuration;
using Metreos.LoggingFramework;
using Metreos.Core.ConfigData;

namespace Metreos.Stats.Alarms
{
    public class SenderNotConfiguredException : ApplicationException
    {
        public SenderNotConfiguredException(string message)
            : this(message, null) { }

        public SenderNotConfiguredException(string message, Exception innerException)
            : base (message, innerException) {}
    }

    public abstract class AlarmSenderBase : IDisposable
    {
        protected readonly LogWriter log;

        public abstract void SendAlarm(AlarmData data, bool cleared);

        public abstract void Dispose();

        public AlarmSenderBase(LogWriter log)
        {
            if(log == null)
                throw new ArgumentException("Cannot create AlarmSender with null log", "log");

            this.log = log;
        }
    }
}
