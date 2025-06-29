using System;
using System.Diagnostics;
using System.Collections;

namespace Metreos.AppServer.TelephonyManager
{
	internal sealed class CallTable
	{
		/// <summary>Table of all outstanding calls</summary>
        /// <remarks>Routing GUID (string) -> ArrayList of CallInfo objects</remarks>
        private readonly Hashtable callTable;

		/// <summary>Table of all calls currently executing a state map</summary>
		/// <remarks>Call ID (long) -> CallInfo (object)</remarks>
		private readonly Hashtable activeCalls;

        public int NumGuids { get { return callTable.Count; } }

        public int NumActiveCalls { get { return activeCalls.Count; } }

		public int Count 
        { 
            get 
            { 
                int count = 0;
                lock(callTable.SyncRoot)
                {
                    foreach(ArrayList calls in callTable.Values)
                    {
                        count += calls.Count;
                    }
                }
                return count;
            } 
        }

		public CallTable()
		{
            this.callTable = Hashtable.Synchronized(new Hashtable());
			this.activeCalls = Hashtable.Synchronized(new Hashtable());
		}

        public bool AddCall(CallInfo cInfo)
        {
            if(cInfo.RoutingGuid == null) { return false; }

			ArrayList calls = callTable[cInfo.RoutingGuid] as ArrayList;

			if(calls == null)
			{
				calls = ArrayList.Synchronized(new ArrayList());
				callTable[cInfo.RoutingGuid] = calls;
			}

            calls.Add(cInfo);
			SetActive(cInfo);
            return true;
        }

        public bool Contains(string routingGuid, long callId)
        {
            return GetCallInfo(routingGuid, callId) != null;
        }

        public CallInfo GetCallInfo(string routingGuid, long callId)
        {
            ArrayList calls = GetCalls(routingGuid);
            if(calls == null) { return null; }

            lock(calls.SyncRoot)
            {
                foreach(CallInfo cInfo in calls)
                {
                    if(cInfo.CallId == callId)
                        return cInfo;
                }
            }
            return null;
        }

        /// <summary>Gets a CallInfo object by ID</summary>
		/// <remarks>Heavy-weight method. Use the other overload if you can.</remarks>
        public CallInfo GetCallInfo(long callId)
        {
            ArrayList calls = GetAllCalls();

            lock(calls.SyncRoot)
            {
                foreach(CallInfo cInfo in calls)
                {
                    if(cInfo.CallId == callId)
                        return cInfo;
                }
            }
            return null;
        }

        public CallInfo GetMediaCallInfo(string routingGuid, uint connectionId)
        {
            ArrayList calls = GetCalls(routingGuid);
            if(calls == null) { return null; }

            lock(calls.SyncRoot)
            {
                foreach(CallInfo cInfo in calls)
                {
                    if(cInfo.ConnectionId == connectionId)
                        return cInfo;
                }
            }
            return null;
        }

        /// <summary>Gets a CallInfo object by ID</summary>
        /// <remarks>Heavy-weight method. Use the other overload if you can.</remarks>
        public CallInfo GetMediaCallInfo(uint connectionId)
        {
            ArrayList calls = GetAllCalls();

            lock(calls.SyncRoot)
            {
                foreach(CallInfo cInfo in calls)
                {
                    if(cInfo.ConnectionId == connectionId)
                        return cInfo;
                }
            }
            return null;
        }

        public ArrayList GetCalls(string routingGuid)
        {
            return callTable[routingGuid] as ArrayList;
        }

        public ArrayList GetAllCalls()
        {
            ArrayList calls = ArrayList.Synchronized(new ArrayList());

            lock(callTable.SyncRoot)
            {
                foreach(DictionaryEntry de in callTable)
                {
                    calls.AddRange(de.Value as ArrayList);
                }
            }
            return calls;
        }

		public ArrayList GetActiveCalls()
		{
			lock(activeCalls.SyncRoot)
			{
				return new ArrayList(activeCalls.Values);
			}
		}

        public bool ChangeActionGuid(string oldGuid, string newGuid)
        {
            lock(callTable.SyncRoot)
            {
                if(callTable.Contains(oldGuid) == false)
                    return false;

                ArrayList calls = callTable[oldGuid] as ArrayList;
                callTable.Remove(oldGuid);
                
                // Update routing GUID in each call object
                if(calls != null)
                {
                    lock(calls.SyncRoot)
                    {
                        foreach(CallInfo cInfo in calls)
                        {
                            cInfo.RoutingGuid = newGuid;
                        }
                    }
                }

                callTable[newGuid] = calls;
            }
            return true;
        }

		public void SetInactive(long callId)
		{
			this.activeCalls.Remove(callId);
		}

		public void SetActive(CallInfo cInfo)
		{
			activeCalls[cInfo.CallId] = cInfo;
		}

        public void RemoveCall(string routingGuid, long callId)
        {
            ArrayList calls = GetCalls(routingGuid);
            if(calls == null) { return; }

            // Lock callTable early to avoid deadlock
            lock(callTable.SyncRoot)
            {
                lock(calls.SyncRoot)
                {
                    foreach(CallInfo cInfo in calls)
                    {
                        if(cInfo.CallId == callId)
                        {
                            calls.Remove(cInfo);
                            if(calls.Count == 0)
                                callTable.Remove(routingGuid);
                            return;
                        }
                    }
                }
            }

			activeCalls.Remove(callId);
        }

        public void RemoveCalls(string routingGuid)
        {
            callTable.Remove(routingGuid);
        }

        public void Clear()
        {
            callTable.Clear();
        }
	}
}
