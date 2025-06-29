using System;
using System.Diagnostics;

using Metreos.LoggingFramework;

namespace Metreos.LogSinks
{
    public sealed class EventLogSink : LoggerSinkBase
    {
		private EventLog eventLog;

        public EventLogSink()
            : this(new EventLog("Application"), TraceLevel.Error) {}

        public EventLogSink(EventLog eLog, TraceLevel initLogLevel)
            : base(initLogLevel)
        {
            this.eventLog = eLog;
        }

		/// <summary>
		/// Write entry to the event log.
		/// </summary>
		/// <param name="timeStamp">The time when the event that this message represents occurred.</param>
		/// <param name="errorLevel">Error level.</param>
		/// <param name="message">Text.</param>
        public override void LoggerWriteCallback(DateTime timeStamp, TraceLevel errorLevel, string message)
        {
            try
            {
                if(errorLevel == TraceLevel.Error)
                {
                    eventLog.WriteEntry(timeStamp.ToString(ILog.ShortTimestampFormat) + " " + message,
						EventLogEntryType.Error);
                }
            }
            catch
            {
                // Catch the exception and don't barf.  Unfortunately,
                // we can't do anything else right now.
            }
        }

        public override void Dispose() 
        {
            logger.UnregisterLoggerSink(this);
            logger.RefreshConfiguration();
        }
    }
}
