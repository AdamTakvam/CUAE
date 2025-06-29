// Controls logging excessive time spent processing queues.
//#define DEBUG_PROCESS_QUEUE_TIMING

using System;
using System.Collections;
using System.Threading;
using System.Diagnostics;

using Metreos.Utilities;
using Metreos.LoggingFramework;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Represents a mechanism that processes events on a queue via a thread
	/// pool.
	/// </summary>
	public class EventProcessor
	{
		/// <summary>
		/// Creates thread pool that processes events coming off of the event
		/// queue.
		/// </summary>
		/// <param name="log">Object through which log entries are generated.</param>
		public EventProcessor(LogWriter log)
		{
		}
	}
}
