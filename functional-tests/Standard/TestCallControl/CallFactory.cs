using System;
using Codecs = Metreos.Interfaces.IMediaControl.Codecs;

namespace Metreos.FunctionalTests
{
	/// <summary>
	/// Summary description for CallFactory.
	/// </summary>
	public class CallFactory
	{
        long callId;
        int to;
        int from;
        byte txIp;
        ushort txPort;
        const string baseIp = "10.1.14.";
        private int callDuration;
        private object creationLock;
//        int rxCodec;
//        int rxFramesize;
//        int txCodec;
//        int txFramesize;

		public CallFactory(int callDuration)
		{
            creationLock = new object();
            callId = 0;
            to = 2000;
            from = 2000;
            txIp = 0;
            txPort = 0;
            this.callDuration = callDuration;
		}

        public CallInfo Create()
        {
            lock(creationLock)
            {
                unchecked // Rollover allowed
                {
                    callId++;
                    from++;
                    txIp++;
                    txPort++;
                }
                // Stagger to from from so that the difference is obvious
                to = from + 1;
            }
            return new CallInfo(callId, to.ToString(), from.ToString(), baseIp + txIp, txPort, callDuration);
        }
	}

    public class CallInfo
    {
        public long   CallId { get { return callId; } }
        public string To { get { return to; } }
        public string From { get { return from; } }
        public string TxIp { get { return txIp; } }
        public uint   TxPort { get { return txPort; } }
        public string TxControlIp { get { return txIp; } }
        public uint   TxControlPort { get { return unchecked ( txPort + 1 ); } }
        public Codecs RxCodec { get { return Codecs.G711u; } }
        public uint   RxFramesize { get { return 20; } }
        public Codecs TxCodec { get { return Codecs.G711u; } }
        public uint   TxFramesize { get { return 20; } }
        public int    CallLength { get { return callLength; } } 

        private long callId;
        private string to;
        private string from;
        private string txIp;
        private uint txPort;
        private int callLength;

        public CallInfo(long callId, string to, string from, string txIp, uint txPort, int callLength)
        {
            this.callId = callId;
            this.to = to;
            this.from = from;
            this.txIp = txIp;
            this.txPort = txPort;

            this.callLength = callLength;
        }
    }
}
