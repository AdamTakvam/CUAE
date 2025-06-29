using System;
using System.Collections;

using Metreos.Samoa.Core;

namespace Metreos.MmsTester.Core
{
	/// <summary>
	/// Upkeeps the system resources available to the test.
	/// </summary>
	public class ResourceProvider
	{
        // private static Hashtable availableMediaServers;
        Conduit.Conduit conduit;

		public ResourceProvider(Conduit.Conduit conduit)
		{
			this.conduit = conduit;
		}

        // REFACTOR:  must handle multiple media servers in future.
        // Can't at the moment due to accessible configuration information
        public static bool PopMediaServer(string handle, out string machineName, out string queueName)
        {
            //not using handle yet, because there aren't multiple media servers

            machineName = null;
            queueName = null;

            machineName = Config.Providers.MediaServer.RemoteMachine;
            queueName = Config.Providers.MediaServer.RemoteQueue;

           return true;
        }
	}
}
