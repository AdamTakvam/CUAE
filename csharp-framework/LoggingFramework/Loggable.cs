using System;
using System.Diagnostics;

namespace Metreos.LoggingFramework
{
	/// <summary>
	/// Base class for elements that would like to write log messages.
	/// This is a utility class. Other classes that want to write 
	/// log messages that can not (for whatever reason) inherit from
	/// Loggable, may just use a LogWriter.
	/// </summary>
	public abstract class Loggable : MarshalByRefObject, IDisposable
	{
        protected readonly LogWriter log;

        public TraceLevel LogLevel
        {
            get { return log.LogLevel; }
        }

		public Loggable(TraceLevel logLevel, string name)
		{
            // Use an abbreviation of the log name, based on at most the first
            // N uppercase letters. Pad to the right with spaces.
            const int ShortTaskNameLength = 3;
            string shortTaskName = string.Empty;
            foreach (char c in name)
            {
                if (char.IsUpper(c))
                {
                    shortTaskName += c;
                }
            }
            this.log = new LogWriter(logLevel,
                shortTaskName.PadRight(ShortTaskNameLength).Substring(0, ShortTaskNameLength));
		}

        public void SetLogLevel(TraceLevel logLevel)
        {
            log.LogLevel = logLevel;
        }

        /// <summary>
        /// Make sure this object has an infinite lifetime in terms of the remote garbage
        /// collector.
        /// </summary>
        /// <returns>Always returns null.</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }

        public virtual void Dispose()
        {
            log.Dispose();
        }
	}
}
