using System;

namespace Metreos.MmsTester.Core
{
	/// <summary>
	/// Encapsulates the functionality and limitiations of the media server
	/// </summary>
	public class MediaServer
	{
        public MediaServerInformationTable mediaServerData;

		public MediaServer( string mediaServerMachineName, string mediaServerQueueName, int numberOfTotalPossibleConnections )
		{
			mediaServerData =  new MediaServerInformationTable(mediaServerMachineName, mediaServerQueueName, numberOfTotalPossibleConnections);
		}
	}
}
