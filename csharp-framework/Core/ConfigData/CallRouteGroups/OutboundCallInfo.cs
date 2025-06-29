using System;
using System.Net;

using Metreos.Messaging.MediaCaps;

namespace Metreos.Core.ConfigData.CallRouteGroups
{
	/// <summary>Everything a provider needs to know to make an outbound call</summary>
	/// <remarks>This object exists to aid providers in supporting CRG failover</remarks>
	[Serializable]
	public class OutboundCallInfo
	{
        private long callId;
        public long CallId 
        { 
            get { return callId; }
            set { callId = value; }
        }

        private string appName;
        public string AppName 
        { 
            get { return appName; }
            set { appName = value; }
        }

        private string partName;
        public string PartitionName 
        { 
            get { return partName; }
            set { partName = value; }
        }

        private CallRouteGroup crg;
        public CallRouteGroup RouteGroup 
        { 
            get { return crg; }
            set { crg = value; }
        }

        private string to;
        public string To 
        { 
            get { return to; }
            set { to = value; }
        }

        private string from;
        public string From 
        { 
            get { return from; }
            set { from = value; }
        }

        private string displayName;
        public string DisplayName 
        { 
            get { return displayName; }
            set { displayName = value; }
        }

        private IPEndPoint rxAddr;
        public IPEndPoint RxAddr 
        { 
            get { return rxAddr; }
            set { rxAddr = value; }
        }

        private MediaCapsField caps;
        public MediaCapsField Caps 
        { 
            get { return caps; }
            set { caps = value; }
        }

        private bool isP2P;
        public bool IsPeerToPeer 
        { 
            get { return isP2P; }
            set { isP2P = value; }
        }

        /// <summary>Used to keep track of iterations through the DeviceNames collection</summary>
        /// <remarks>This property must be maintained entirely by the consumer. 
        /// Its value will not be updated or modified in any way by this class.</remarks>
        public uint DeviceIndex 
        { 
            get { return deviceIndex; } 
            set { deviceIndex = value; }
        }
        private uint deviceIndex = 0;

        public OutboundCallInfo(long callId, string appName, string partName, CrgMember[] members, 
            string to, string from, string displayName, MediaCapsField caps, IPEndPoint rxAddr, bool isP2P)
		{
            this.callId = callId;
            this.appName = appName;
            this.partName = partName;
            this.to = to;
            this.from = from;
            this.displayName = displayName;
            this.caps = caps;
            this.rxAddr = rxAddr;
            this.isP2P = isP2P;

            this.crg = new CallRouteGroup(members);
        }

        public OutboundCallInfo()
        {
        }

        protected OutboundCallInfo(OutboundCallInfo callInfo)
        {
            this.callId = callInfo.callId;
            this.appName = callInfo.appName;
            this.partName = callInfo.partName;
            this.to = callInfo.to;
            this.from = callInfo.from;
            this.displayName = callInfo.displayName;
            this.caps = callInfo.caps;
            this.isP2P = callInfo.isP2P;

            this.crg = callInfo.crg;
        }
	}
}
