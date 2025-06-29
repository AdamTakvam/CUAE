using System;
using System.Threading;


namespace Metreos.MMSTestTool.Sessions
{
	public class MediaServerInfo
	{
		public MediaServerResources resources = new MediaServerResources();

		/// <summary>
		/// Timer fired if we don't hear from the media server have a period
		/// of time defined by MsHeartbeatTimeout.
		/// </summary>
		public Timer msHeartbeatTimeout;

		/// <summary>
		/// Timer fired if the media server does not acknowledge our connect
		/// message soon enough. This will cause us to send another connect
		/// message.
		/// </summary>
		public Timer msConnectTimeout;

		/// <summary>
		/// True if we have established a connection to the media server.
		/// </summary>
		public volatile bool connectedToServer;

		/// <summary>
		/// Locks the processing of server connect messages.
		/// </summary>
		public object processingConnectLock = new object();

		/// <summary>
		/// The number of attempts the provider has made to connect to the
		/// media server.
		/// </summary>
		public string mmsConnectAttempts;

		/// <summary>
		/// The transaction ID of the last connect attempt;
		/// </summary>
		public string lastConnectAttemptTransactionId;

		/// <summary>
		/// The ID returned to us by the media server that is used
		/// to identify our unique connection to it.
		/// </summary>
		public string clientId;

		/// <summary>
		/// Indicates whether client ID is required in messages
		/// sent to the media server.
		/// </summary>
		public bool useClientId = false;

		/// <summary>
		/// Indicates whether serverId is required in messages sent to the media server
		/// </summary>
		public bool useServerId = false;
				
		/// <summary>
		/// The server ID that has been assigned to this media server.
		/// Valid server IDs are 1-255.  If operating in single media
		/// server mode, the ID of 0 should be used.
		/// </summary>
		public string serverId = "0";

		//server parameters
		/// <summary>
		/// The heartbeat interval
		/// </summary>
		public string heartbeatInterval;
		public int msHeartbeatTimeoutValue; 
		public int msConnectTimeoutValue;
		
		/// <summary>
		/// the heartbeat payload, typically media resources.
		/// </summary>
		public string heartbeatPayload;
		

		/// <summary>
		/// The IP address of the media server.
		/// </summary>
		public string mediaServerIpAddress;

	}
}
