using System;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.Utilities;

namespace Metreos.AppServer.ARE.Collections
{
	public sealed class SessionDataCollection
	{
        // Session GUID -> SessionData
        private readonly IDictionary sessions;

		public int Count { get { return sessions.Count; } }

        public SessionData this[string sessionGuid]
        {
            get { return sessions[sessionGuid] as SessionData; }
        }

		public SessionDataCollection()
		{
            sessions = ReportingDict.Wrap( "SessionDataCollection.sessions", Hashtable.Synchronized(new Hashtable()) );
		}

        public void Add(string sessionGuid, SessionData sessionData)
        {
            lock(sessions.SyncRoot)
            {
                if(!sessions.Contains(sessionGuid))
                    sessions.Add(sessionGuid, sessionData);
            }
        }

        public void Remove(string sessionGuid)
        {
            sessions.Remove(sessionGuid);
        }

        public bool Contains(string sessionGuid)
        {
            return sessions.Contains(sessionGuid);
        }

        public IEnumerator GetEnumerator()
        {
            return new ArrayList(sessions.Values).GetEnumerator();
        }

        public void Clear()
        {
            foreach(SessionData data in this)
            {
                data.Clear();
            }

            sessions.Clear();
        }
	}
}
