using System;

namespace Metreos.MmsTester.Interfaces
{
	/// <summary>
	/// Summary description for IMessaging.
	/// </summary>
	public abstract class IMessaging
	{
        // REFACTOR:
        public const string MACHINE_NAME = "BART";

        public const string SOURCE = "source";
		public const string CLIENT_NAME = "clientName";
        public const string MEDIA_SERVER_NAME = "mediaServerName";
        public const string ADAPTER_DISPLAY_NAME = "adapterDisplayName";
        public const string CLIENT_DISPLAY_NAME = "clientDisplayName";
        public const string ADAPTER_GUID = "adapterGuid";
        public const string CLIENT_GUID = "clientGuid";
        public const string MEDIA_SERVER_HANDLE = "mediaServerHandle";
        public const string TEST_TOOL_NAME = "mms-test-tool";
        public const string TEST_MACHINE_NAME = "machineName";
        public const string TEST_QUEUE_NAME = "queueName";

        

        // System Commands
        public const string INITIALIZE_ADAPTER = "Initialize Adapter";
        public const string START_ADAPTER = "Start Adapter";
        public const string LINK_ADAPTER_TO_CLIENT = "Link Adapter";
        public const string START_CLIENT = "Start Client";

        // Media Server Commands
        public const string CONNECT_TO_MEDIASERVER = "Connect to Media Server";
        public const string CONNECT_TO_CONFERENCE = "Connect to Conference";
        public const string DISCONNECT_FROM_CONFERENCE = "Disconect from Conference";
        public const string PLAY_TO_CONFERENCE = "Play to Conference";

        public const string REQUEST_MEDIA_SERVER_INFO = "Request Media Server Info";

        // boolean replacements
        public const string SUCCESS = "success";
        public const string FAILURE = "failure";

        // basic utils
        public const string NUM_CONNECTIONS = "Number of Connections";
        public const string NUM_CONFERENCES = "Number of Conferences";
        public const string CONNECTION_NUM = "ConnectionNumber";
        public const string CONFERENCE_NUM = "ConferenceNumber";

	}
}
