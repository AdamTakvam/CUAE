using System;
using System.Collections;

namespace Metreos.MmsTester.MediaServerFramework
{
	/// <summary>
	/// Summary description for MediaServerManager.
	/// </summary>
	public class MediaServerManager
	{
        public ArrayList mediaServers;

        // Constructor for just one Media Server
		public MediaServerManager(string mediaServerMachineName, string mediaServerQueueName, int numberOfTotalPossibleConnections)
		{
            
            mediaServers = new ArrayList();
            mediaServers.Add(new MediaServer(mediaServerMachineName, mediaServerQueueName, numberOfTotalPossibleConnections, 0));

		}
	}
}
