using System;

namespace Metreos.MmsTester.Core
{
	/// <summary>
	/// Summary description for ResourcePool.
	/// </summary>
	public class ResourcePool
	{
        public const int SESSION_TIMEOUT = 180;
        public const int COMMAND_TIMEOUT = 5000;

        public const int MAX_NUMBER_CONNECTIONS = 32;

        public const bool OVERFLOW_CONNECTIONS_ALLOWED = false;
		public ResourcePool()
		{
			
		}
	}
}
