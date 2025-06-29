using System;
using System.Threading;

using Metreos.Interfaces;

namespace Metreos.Configuration
{
	/// <summary>Generates unique call IDs</summary>
	public sealed class CallIdFactory : MarshalByRefObject, ICallIdFactory
	{
        #region Singleton Interface

        private static CallIdFactory instance = null;
        private static object instanceLock = new object();

        public static CallIdFactory Instance
        {
            get
            {
                lock(instanceLock)
                {
                    if(instance == null)
                    {
                        instance = new CallIdFactory();
                    }
                    return instance;
                }
            }
        }

        #endregion

        private long callId;

		private CallIdFactory()
		{
			callId = 1000000;
		}

        public long GenerateCallId()
        {
            return Interlocked.Increment(ref callId);  // Handles rollover
        }

        #region MashalByRefObject Implementation

        public override object InitializeLifetimeService()
        {
            return null;
        }

        #endregion
	}
}
