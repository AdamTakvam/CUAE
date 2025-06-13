using System;
using System.Collections;

using Metreos.Utilities.Collections;

namespace Metreos.CallControl.H323
{
	public class CallIdMap : IEnumerable
	{
        /// <summary>Stack call ID -> AppServer call ID</summary>
        private TwoWayHash callMap;

        /// <summary>Short-term memory of calls gone by</summary>
        /// <remarks>
        /// Hack for supressing errors in race conditions.
        /// Two entries per call (AppServer call ID & stack call ID)
        /// </remarks>
        private BoundedCollection callMorgue;

        public long this[string stackCallId] 
        { 
            get { return GetAppServerCallId(stackCallId); } 
        }

        public string this[long callId]
        {
            get { return GetStackCallId(callId); }
        }

		public CallIdMap(int morgueSize)
		{
            callMorgue = new BoundedCollection(morgueSize);
            callMap = new TwoWayHash();
		}

        public void Add(long callId, string stackCallId)
        {
            callMap.Add(callId, stackCallId);
        }

        public void Remove(long callId)
        {
            lock(callMorgue.SyncRoot)
            {
                callMorgue.Add(callId);

                string stackCallId = GetStackCallId(callId);
                if(stackCallId != null)
                    callMorgue.Add(stackCallId);
                
                callMap.RemoveByKey(callId);
            }
        }

        public bool IsRecentlyEnded(long callId)
        {
            return callMorgue.Contains(callId);
        }

        public bool IsRecentlyEnded(string stackCallId)
        {
            return callMorgue.Contains(stackCallId);
        }

        public string GetStackCallId(long callId)
        {
            return Convert.ToString(callMap.GetByKey(callId));
        }

        public long GetAppServerCallId(string stackCallId)
        {
            return Convert.ToInt64(callMap.GetByValue(stackCallId));
        }

        public void Clear()
        {
            callMap.Clear();
            callMorgue.Clear();
        }

        public object SyncRoot { get { return callMap.SyncRoot; } }

        public IEnumerator GetEnumerator()
        {
            return callMap.GetEnumerator();
        }
	}
}
