using System;
using System.Diagnostics;

using Metreos.Interfaces;

namespace Metreos.LoggingFramework
{
    [Serializable]
    public class LogWriter : IDisposable
    {
        /// <summary>Log level of this task.</summary>
        protected TraceLevel logLevel;

        protected string logName;

        public LogWriter()
        {
            logName = "None";
            logLevel = TraceLevel.Error;
        }

        public LogWriter(TraceLevel logLevel, string logName)
        {
            this.logLevel = logLevel;
            this.logName = logName;
        }

        public TraceLevel LogLevel
        {
            get { return logLevel; }
            set { logLevel = value; }
        }

        public string LogName
        {
            get { return logName; }
            set { logName = value; }
        }

        public void Write(TraceLevel messageLevel, string message)
        {
            if(messageLevel <= this.logLevel)
            {
                Trace.Write(logName + " " + message, messageLevel.ToString());
            }
        }

        public void Write(TraceLevel messageLevel, string message, params object[] args)
        {
            if(messageLevel <= this.logLevel)
            {
                Write(messageLevel, String.Format(message, args));
            }
        }

        public void WriteIf(bool test, TraceLevel messageLevel, string message)
        {
            if(test)
            {
                Write(messageLevel, message);
            }
        }

        public void WriteIf(bool test, TraceLevel messageLevel, string message, params object[] args)
        {
            if(test)
            {
                Write(messageLevel, message, args);
            }
        }

        /// <summary>
        /// Writes the specified message at the specified level to the log sinks 
        ///   regardless of the log level of this component.
        /// </summary>
        /// <param name="messageLevel">Severity</param>
        /// <param name="message">Message</param>
        public void ForceWrite(TraceLevel messageLevel, string message)
        {
            Trace.Write(logName + " " + message, messageLevel.ToString());
        }

        /// <summary>
        /// Writes the specified message at the specified level to the log sinks 
        ///   regardless of the log level of this component.
        /// </summary>
        /// <param name="messageLevel">Severity</param>
        /// <param name="message">Format</param>
        /// <param name="args">Arguments</param>
        public void ForceWrite(TraceLevel messageLevel, string message, params object[] args)
        {
            string fmtMsg = String.Format(message, args);
            Trace.Write(logName + " " + fmtMsg, messageLevel.ToString());
        }

        public virtual void Dispose()
        { 
        }
    }
}
