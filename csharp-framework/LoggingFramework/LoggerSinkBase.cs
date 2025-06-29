using System;
using System.Diagnostics;

namespace Metreos.LoggingFramework
{
    public delegate TraceLevel LogLevelQueryDelegate();

    /// <summary>Base class for all logger sinks</summary>
	public abstract class LoggerSinkBase : IDisposable
	{
        protected Logger logger = Logger.Instance;
        protected readonly TraceLevel initLogLevel;

        public LogLevelQueryDelegate GetLogLevel;

        public LoggerSinkBase(TraceLevel initLogLevel)
        {
            this.initLogLevel = initLogLevel;
            
            logger.RegisterLoggerSink(this);

            RefreshConfiguration();
        }

        protected void SubscribeToLoggerEvents(TraceLevel level)
        {
            if(level >= TraceLevel.Verbose)
            {
                logger.verboseMessageSink += new LoggerWriteDelegate(this.LoggerWriteCallback);
            }

            if(level >= TraceLevel.Info)
            {
                logger.infoMessageSink += new LoggerWriteDelegate(this.LoggerWriteCallback);
            }
            
            if(level >= TraceLevel.Warning)
            {
                logger.warningMessageSink += new LoggerWriteDelegate(this.LoggerWriteCallback);
            }
            
            if(level >= TraceLevel.Error)
            {
                logger.errorMessageSink += new LoggerWriteDelegate(this.LoggerWriteCallback);
            }
		}

        public virtual void RefreshConfiguration()
        {
            TraceLevel logLevel = initLogLevel;
            if(GetLogLevel != null)
                logLevel = GetLogLevel();

            SubscribeToLoggerEvents(logLevel);
        }

        public abstract void LoggerWriteCallback(DateTime timeStamp, TraceLevel errorLevel, string message);

        public abstract void Dispose();
	}
}
