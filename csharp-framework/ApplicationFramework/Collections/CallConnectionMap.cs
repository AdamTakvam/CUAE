using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;

namespace Metreos.ApplicationFramework.Collections
{
	/// <summary>
	/// Holds strings as keys and values, but not necessarily in a one-to-one fashion
	/// Keys and values can be added individually and only connected when specifically instructed.
	/// </summary>
	[Serializable]
	public class CallConnectionMap
	{
        private StringCollection callIds;
		private StringCollection connectionIds;
		private Hashtable mappings;

        public StringEnumerator CallIds { get { return callIds.GetEnumerator(); } }

        public StringEnumerator ConnectionIds { get { return connectionIds.GetEnumerator(); } }

		public CallConnectionMap()
		{
			callIds = new StringCollection();
			connectionIds = new StringCollection();
            mappings = new Hashtable();
		}

		public bool Add(string callId, string connectionId)
		{
			if((callId == null) || (connectionId == null)) { return false; }

			AddCallId(callId);
			AddConnectionId(connectionId);
			return Connect(callId, connectionId);
		}

		public void AddCallId(string callId)
		{
			if(callId == null) { return; }

			if(!callIds.Contains(callId))
			{
				callIds.Add(callId);
			}
		}

		public void AddConnectionId(string connectionId)
		{
			if(connectionId == null) { return; }

			if(!connectionIds.Contains(connectionId))
			{
				connectionIds.Add(connectionId);
			}
		}

		public bool Connect(string callId, string connectionId)
		{
			if((callId == null) || (connectionId == null)) { return false; }

			if(mappings.ContainsKey(callId)) { return false; }
			if(!callIds.Contains(callId)) { return false; }
			if(!connectionIds.Contains(connectionId)) { return false; }

			mappings[callId] = connectionId;
			return true;
		}

		public bool Disconnect(string callId)
		{
			if(callId == null) { return false; }
			if(!mappings.ContainsKey(callId)) { return false; }

			mappings.Remove(callId);
			return true;
		}

		public void RemoveCallId(string callId)
		{
			if(callId == null) { return; }

			callIds.Remove(callId);
			Disconnect(callId);
		}

		public void RemoveConnectionId(string connectionId)
		{
			if(connectionId == null) { return; }

			connectionIds.Remove(connectionId);
			
			string callId = GetCallId(connectionId);
			if(callId != null)
			{
				Disconnect(callId);
			}
		}

        public string GetCallId(string connectionId)
        {
			if(connectionId == null) { return null; }

            foreach(DictionaryEntry de in mappings)
			{
				string callId = de.Key as string;
				string connId = de.Value as string;

				Debug.Assert(callId != null, "callId is null in CallConnectionMap");
				Debug.Assert(connId != null, "connectionId is null in CallConnectionMap");

				if(connId == connectionId)
				{
					return callId;
				}
			}
			return null;
        }

        public string GetConnectionId(string callId)
        {
            if(callId == null) { return null; }

			return mappings[callId] as string;
        }

        public void Clear()
        {
			callIds.Clear();
			connectionIds.Clear();
            mappings.Clear();
        }
	}
}
