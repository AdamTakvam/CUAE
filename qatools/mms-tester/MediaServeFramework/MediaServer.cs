using System;

namespace Metreos.MmsTester.MediaServerFramework
{
	/// <summary>
	/// Encapsulates the functionality and limitiations of the media server
	/// </summary>
	public class MediaServer
	{
        public MediaServerInformationTable mediaServerData;

        public int guid;

		public MediaServer( string mediaServerMachineName, string mediaServerQueueName, int numberOfTotalPossibleConnections, int guid )
		{
			mediaServerData =  new MediaServerInformationTable(mediaServerMachineName, mediaServerQueueName, numberOfTotalPossibleConnections);
		
            this.guid = guid;
        }
	}
}
