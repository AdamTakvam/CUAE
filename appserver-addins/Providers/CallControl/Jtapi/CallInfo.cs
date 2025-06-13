using System;
using System.Threading;
using System.Net;

using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Core.ConfigData.CallRouteGroups;

namespace Metreos.CallControl.JTapi
{
    public enum CallState
    {
        Init,               // Inbound, Outbound, 3rd Party
        Ringing,            // Inbound
        Answered,           // Inbound
        PendingOutbound,    // Outbound
        Active,             // Inbound, Outbound, 3rd Party
        Held,               // 3rd Party
        InUse               // 3rd Party
    }

    public enum CallDirection
    {
        Inbound,
        Outbound
    }

	public class CallInfo : OutboundCallInfo
	{
		public const int MAKE_CALL_TIMEOUT  = 500; // ms

        /// <summary>Connection to JTapi service</summary>
        private readonly JTapiProxy proxy;
        public JTapiProxy Proxy { get { return proxy; } }

		private readonly CallDirection direction;
		public CallDirection Direction { get { return direction; } }

		private readonly object callLock;
		public object CallLock { get { return callLock; } }

        private readonly string stackCallId;
        public string StackCallId { get { return stackCallId; } }

        private CallState state;
        public CallState State
        {
            get { return state; }
            set { state = value; }
        }

        private bool mediaEstablished = false;
        public bool MediaEstablished
        {
            get { return mediaEstablished; }
            set { mediaEstablished = value; }
        }

        private string errorMsg;
        public string ErrorMessage
        {
            get { return errorMsg; }
            set { errorMsg = value; }
        }

        public bool Error { get { return errorMsg != null; } }

        private ICallControl.EndReason endReason;
        public ICallControl.EndReason EndReason
        {
            get { return endReason; }
            set { endReason = value; } 
        }

        // For third-party calls only
        private string routingGuid;
        public string RoutingGuid 
        { 
            get { return routingGuid; } 
            set { routingGuid = value; }
        }

        private IMediaControl.Codecs codec = IMediaControl.Codecs.Unspecified;
        public IMediaControl.Codecs Codec
        {
            get { return codec; }
            set { codec = value; }
        }

        private uint framesize;
        public uint Framesize
        {
            get { return framesize; } 
            set { framesize = value; }
        }

        private string txIP;
        public string TxIP
        {
            get { return txIP; }
            set { txIP = value; }
        }

        private uint txPort;
        public uint TxPort
        {
            get { return txPort; }
            set { txPort = value; }
        }

        private string rxIP;
        public string RxIP
        {
            get
            {
                if(RxAddr != null)
                {
                    return RxAddr.Address.ToString();
                }
                return null;
            }
            set { rxIP = value; }
        }

        private uint rxPort;
        public uint RxPort
        {
            get
            {
                if(RxAddr != null)
                {
                    return (uint) RxAddr.Port;
                }
                return 0;
            }
            set { rxPort = value; }
        }

        // Method used to set the RxAddr in the OutboundCallInfo.
        // Preferred instead of storing local copies of the IP
        // address string and port.
        public void SetRxAddress(string address, uint port)
        {
            // Create an IPAddress object using the IP Address string
            // and create an IP Endpoint for the combination of IP
            // address and port and store it in the OutboundCallInfo.
            if(address != null)
            {
                IPAddress ipAddress = IPAddress.Parse(address);
                IPEndPoint ipEndpoint = new IPEndPoint(ipAddress, (Int32) port);
                this.RxAddr = ipEndpoint;
            }
        }

        // Method to initialize the Outbound call parameters in the CallInfo
        // object. (which derives from OutboundCallInfo). This is used when 
        // the CallInfo object is created (the default constructor of the 
        // base class will be called) and we want to set the data for the
        // outbound calls.
        public void initializeOutboundCallInfo(OutboundCallInfo callInfo)
        {
            AppName = callInfo.AppName;
            PartitionName = callInfo.PartitionName;
            DisplayName = callInfo.DisplayName;
            Caps = callInfo.Caps;
            IsPeerToPeer = callInfo.IsPeerToPeer;
            RxAddr = callInfo.RxAddr;
            RouteGroup = callInfo.RouteGroup;
        }

        public CallInfo(JTapiProxy proxy, long callId, string to, string from, string stackCallId,
            OutboundCallInfo outCallInfo, CallDirection direction)
        {
            Assertion.Check(proxy != null, "proxy is null in CallInfo");

            this.proxy = proxy;
            this.CallId = callId;
            this.To = to;
            this.From = from;

            this.stackCallId = stackCallId;
            this.direction = direction;

            if(outCallInfo != null)
            {
                initializeOutboundCallInfo(outCallInfo);
            }

            this.state = CallState.Init;
            this.callLock = new object();
        }
	}
}
