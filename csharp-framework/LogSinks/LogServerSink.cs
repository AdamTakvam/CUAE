using System;
using System.Diagnostics;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.Core.IPC.Flatmaps.LogServer;

namespace Metreos.LogSinks
{
	/// <summary>
	///     Creates a log file and writes to it, using the LoggerSink base class 
	/// </summary>
	public sealed class LogServerSink : LoggerSinkBase
	{
		private readonly LogClient logClient;

        public LogServerSink(string serviceName)
            : this(serviceName, 0, TraceLevel.Verbose) { }

		/// <summary>
		///     A sink to log to a file
		/// </summary>
		/// <param name="name"> The name of the log folder </param>
		public LogServerSink(string serviceName, ushort serverPort, TraceLevel initLogLevel)
            : base(initLogLevel)
		{
			if(serverPort == 0)
				serverPort = IServerLog.Default_Port;

            logClient = new LogClient(serviceName, serverPort);
		}
 
		public override void Dispose()
		{
			logClient.Dispose();
		}

		/// <summary>
		///     Invoked by Logger to write a log message to this sink.  
		///     Will attempt to log to the log server until successful or shutdown on 
		///     our end
		/// </summary>
		/// <param name="timeStamp">
		///     The time when the event that this message represents occurred.
		/// </param>
		/// <param name="errorLevel"> 
		///     The error level of the message.
		/// </param>
		/// <param name="message">
		///     The message to write.
		/// </param>
		public override void LoggerWriteCallback(DateTime timeStamp, TraceLevel errorLevel, string message)
		{
			logClient.WriteLog(timeStamp, errorLevel, message);
		}
	}
}