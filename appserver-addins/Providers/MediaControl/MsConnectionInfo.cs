using System;

namespace Metreos.Providers.MediaControl
{
    /// <summary>
    /// Container class to hold information about a given media server connection.
    /// </summary>
    public sealed class MsConnectionInfo
    {
        /// <summary>
        /// Media server connection ID.
        /// </summary>
        public string connectionId;

        /// <summary>
        /// The IP address that the media server is using to receive media on.
        /// </summary>
        public string localIpAddress;

        /// <summary>
        /// The port that the media server is using to receive media on.
        /// </summary>
        public string localPort;

        /// <summary>
        /// The remote IP address that the media server is sending media to.
        /// </summary>
        public string remoteIpAddress;

        /// <summary>
        /// The remote port that the media server is sending media to.
        /// </summary>
        public string remotePort;

        /// <summary>
        /// The ID of the conference that this connection belongs to.
        /// Can be null.
        /// </summary>
        public string conferenceId;
    }
}
