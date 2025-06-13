using System;

namespace CallMonitorReport
{
	/// <summary>
	/// Summary description for DIDCallStat.
	/// </summary>
	public class DIDCallStat
	{
        private string _did;
        private int _numCalls;

        public string did { get { return _did; } set { _did = value; } }
        public int numCalls { get { return _numCalls; } set { _numCalls = value; } }

		public DIDCallStat(string s, int n)
		{
            this.did = s;
            this.numCalls = n;
		}
	}
}
