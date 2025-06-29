using System;

namespace Metreos.Applications.cBarge
{
	/// <summary>
	/// This class contains values that describe a unique endpoint in an active call.
	/// </summary>
	public class EndPoint
	{

        /// <summary>
        /// MMS ConnectionId associated with this endpoint
        /// </summary>
        public string ConnectionId
        {
            get { return connectionId; }
            set { connectionId = value; }
        }
        private string connectionId;
        
        /// <summary>
        /// Status of the Rx media channel for this endpoint, WITH RESPECT TO THE MEDIA SERVER connection:
        /// </summary>
        public bool RxOpen
        {
            get { return rxOpen; }
            set { rxOpen = value; }
        }
        private bool rxOpen;

        /// <summary>
        /// Status of the Tx media channel for this endpoint, WITH RESPECT TO THE MEDIA SERVER connection:
        /// </summary>
        public bool TxOpen
        {
            get { return txOpen; }
            set { txOpen = value; }
        }
        private bool txOpen;


        //NOTE: when finalizing the app, consider making this an enum

        /// <summary>
        /// IP address on which this endpoint is receiving RTP data
        /// </summary>
        public string RxIP
        {
            get { return rxIP; }
            set { rxIP = value; }
        }
        private string rxIP;
        
        /// <summary>
        /// Port on which this endpoint is receiving RTP data
        /// </summary>
        public uint RxPort
        {
            get { return rxPort; }
            set { rxPort = value; }
        }
        private uint rxPort;
        
        /// <summary>
        /// IP to which this endpoint is transmitting RTP data
        /// </summary>
        public string TxIP
        {
            get { return txIP; }
            set { txIP = value; }
        }
        private string txIP;
        
        /// <summary>
        /// Port to which this endpoint is transmitting RTP data
        /// </summary>
        public uint TxPort
        {
            get { return txPort; }
            set { txPort = value; }
        }
        private uint txPort;
        
        public enum Status
        {
        }

        public EndPoint()
		{
            connectionId = rxIP = txIP = "0";
            rxOpen = txOpen = false;
            rxPort = txPort = 0;
        }

	}
}
