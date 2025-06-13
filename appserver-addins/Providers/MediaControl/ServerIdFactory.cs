using System;
using System.Threading;

namespace Metreos.MediaControl
{
	/// <summary>Generates unique media server IDs</summary>
	public abstract class ServerIdFactory
	{
        private abstract class Consts
        {
            public const int SeedValue = 0;  // Numbering will start at 1
        }

        private static int serverId = Consts.SeedValue;

        public static uint GenerateId()
        {
            return Convert.ToUInt32(Interlocked.Increment(ref serverId));
        }
	}
}
