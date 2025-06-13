using System;
using System.Threading;

using Metreos.Interfaces;
using Metreos.Core;
using Metreos.Core.ConfigData;
using Metreos.Messaging.MediaCaps;

namespace Metreos.CallControl.Sip
{
    public enum CallState
    {
		Error,				// there is error for the call
        Init,               // Inbound, Outbound
		Trying,				// Proxy server is trying 100
        Ringing,            // Response code 180
		Fowarding,			// Response code 181
		Queued,				// Response code 182
		InProgress,			// Response code 183
        Answered,           // Inbound
        PendingOutbound,    // Outbound
        Active,             // Inbound, Outbound
		ReInviting,			// ReInviting
		OnHold,				// onhold
    }

    public enum CallDirection
    {
        Inbound,
        Outbound
    }

	public class CallInfo
	{
		public const int MAKE_CALL_TIMEOUT  = 500; // ms

		private CallDirection direction;
		public CallDirection Direction { get { return direction; } }

		private long callId;
		public long CallId { get { return callId; } }

		private object callLock;
		public object CallLock { get { return callLock; } }

		private string stackCallId;
		public string StackCallId 
		{ 
			get { return stackCallId; } 
			set { stackCallId = value; }
		}

		private string deviceDomain;
		public string DeviceDomain
		{
			get { return deviceDomain; }
		}

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

		private string rxIp;
		public string RxIp
		{
			get { return rxIp; }
			set { rxIp = value; }
		}

		private int rxPort;
		public int RxPort
		{
			get { return rxPort; }
			set { rxPort = value; }
		}

		private string txIp;
		public string TxIp
		{
			get { return txIp; }
			set { txIp = value; }
		}

		private int txPort;
		public int TxPort
		{
			get { return txPort; }
			set { txPort = value; }
		}

		private IMediaControl.Codecs codec;
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

		private MediaCapsField caps;
		public MediaCapsField  MediaCaps
		{
			get { return caps; }
			set { caps = value; }
		}

		public CallInfo(long callId, string stackCallId, CallDirection direction, string deviceDomain)
		{
            this.callId = callId;
            this.stackCallId = stackCallId;
            this.direction = direction;
			this.deviceDomain = deviceDomain;
            this.state = CallState.Init;
			this.callLock = new object();

			this.codec = IMediaControl.Codecs.Unspecified;
		}

		public override bool Equals(Object o)
		{
			if (o == null || GetType() != o.GetType())
				return false;
			
			CallInfo co = (CallInfo) o;
			return callId == co.callId;
		}

		public override int GetHashCode()
		{
			return (int) callId;
		}

		public bool IsMediaInfoComplete()
		{
			return (txIp != null && txIp.Length > 0 && txPort != 0 &&
					rxIp != null && rxIp.Length > 0 && rxPort != 0);
		}
	}
}
