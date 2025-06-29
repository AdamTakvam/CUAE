using System;
using System.Threading;

using Metreos.Interfaces;

namespace Metreos.AppServer.ARE
{
	/// <summary>Generates short script IDs (used for logging only)</summary>
	/// <remarks>This class is threadsafe</remarks>
	public sealed class ScriptIdFactory
	{
        private long id;

        /// <summary>IDs are only unique for any given instance of this factory</summary>
		public ScriptIdFactory()
		{
			this.id = 0;
		}

        /// <summary>Generates a new ID</summary>
        /// <returns>64-bit ID</returns>
        public long GenerateId()
        {
            return Interlocked.Increment(ref id);  // Handles rollover
        }
	}
}
