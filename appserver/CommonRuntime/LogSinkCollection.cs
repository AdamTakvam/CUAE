using System;
using System.Collections;

using Metreos.LoggingFramework;

namespace Metreos.AppServer.CommonRuntime
{
	public class LogSinkCollection : IEnumerable
	{
        private readonly ArrayList sinks;

        public int Count { get { return sinks.Count; } }

        public LoggerSinkBase this[int index] { get { return sinks[index] as LoggerSinkBase; } }

		public LogSinkCollection()
		{
            sinks = new ArrayList();
		}

        public void Add(LoggerSinkBase logSink)
        {
            sinks.Add(logSink);
        }

        public void Remove(LoggerSinkBase logSink)
        {
            sinks.Remove(logSink);
        }

        public void Clear()
        {
            sinks.Clear();
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return sinks.GetEnumerator();
        }

        #endregion
    }
}
