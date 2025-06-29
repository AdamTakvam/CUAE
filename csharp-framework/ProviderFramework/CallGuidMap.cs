using System;
using System.Collections;

namespace Metreos.ProviderFramework
{
	/// <summary>Table of call ID to Routing GUID</summary>
	public class CallGuidMap
	{
        Hashtable callTable;

        public string this[long callId] { get { return callTable[callId] as string; } }

		public CallGuidMap()
		{
			callTable = Hashtable.Synchronized(new Hashtable());
		}

        public void Add(long callId, string routingGuid)
        {
            callTable[callId] = routingGuid;
        }

        public bool Contains(string callId)
        {
            return callTable.Contains(callId);
        }

        public void Remove(long callId)
        {
            callTable.Remove(callId);
        }

        public void Clear()
        {
            callTable.Clear();
        }
	}
}
