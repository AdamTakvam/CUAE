using System;

namespace Metreos.RecordAgent
{
	/// <summary>
	/// Summary description for CallData.
	/// </summary>
	public class CallData
	{
        public CallData()
        {
            this.callIdentifier = 0;
            this.callState = 0;
            this.callType = 0;
            this.callerDN = null;
            this.calleeDN = null;
            this.callerName = null;
            this.calleeName = null;
        }

        public CallData(uint callIdentifier, uint callState, uint callType, 
            string callerDN, string callerName, string calleeDN, string calleeName)
        {
            this.callIdentifier = callIdentifier;
            this.callState = callState;
            this.callType = callType;
            this.callerDN = callerDN;
            this.calleeDN = calleeDN;
            this.callerName = callerName;
            this.calleeName = calleeName;
        }

        private uint callIdentifier;
        public uint CallIdentifier { get { return callIdentifier; } set { callIdentifier = value; } }

        private uint callState;
        public uint CallState { get { return callState; } set { callState = value; } }

        private uint callType;
        public uint CallType { get { return callType; } set { callType = value; } }

        private string callerDN;
        public string CallerDN { get { return callerDN; } set { callerDN = value; } }

        private string calleeDN;
        public string CalleeDN { get { return calleeDN; } set { calleeDN = value; } }

        private string callerName;
        public string CallerName { get { return callerName; } set { callerName = value; } }

        private string calleeName;
        public string CalleeName { get { return calleeName; } set { calleeName = value; } }

        public CallData Clone() { return (CallData)MemberwiseClone(); }
    }
}
