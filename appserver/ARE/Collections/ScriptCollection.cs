using System;
using System.Collections;
using System.Collections.Specialized;

using Metreos.ApplicationFramework;
using Metreos.Utilities;

namespace Metreos.AppServer.ARE.Collections
{
	public class ScriptCollection: IEnumerable
	{
        /// <summary>Collection of all script instances</summary>
        /// <remarks>routing GUID (string) -> RuntimeScriptInfo</remarks>
        private readonly IDictionary allScripts;

        /// <summary>Subset of scripts which are currently executing a function</summary>
        /// <remarks>routing GUID (string) -> RuntimeScriptInfo</remarks>
        private readonly IDictionary runningScripts;

        public int TotalCount { get { return allScripts.Count; } }
        public int RunningCount { get { return runningScripts.Count; } }

        public RuntimeScriptInfo this[string routingGuid] { get { return GetScriptInfo(routingGuid); } }

		public ScriptCollection()
		{
            this.allScripts = ReportingDict.Wrap("Scheduler.allScripts", Hashtable.Synchronized( new Hashtable() ) );
            this.runningScripts = ReportingDict.Wrap("Scheduler.runningScripts", Hashtable.Synchronized( new Hashtable() ) );
		}

        /// <summary>Adds a new script instance and sets it active</summary>
        /// <param name="script">Script instance metadata</param>
        public void Add(RuntimeScriptInfo script)
        {
			this.allScripts.Add(script.routingGuid, script);
            this.runningScripts.Add(script.routingGuid, script);
		}

        /// <summary>Sets the specified script instance active.</summary>
        /// <remarks>This causes the scheduler to begin executing the current action in the current function.</remarks>
        /// <param name="routingGuid">Instance identifier</param>
        /// <returns>success</returns>
        public bool SetActive(string routingGuid)
        {
            RuntimeScriptInfo scriptInfo = GetScriptInfo(routingGuid);
            if(scriptInfo == null)
                return false;
            
            lock(runningScripts.SyncRoot)
            {
                if(!this.runningScripts.Contains(routingGuid))
                    this.runningScripts.Add(routingGuid, scriptInfo);
            }
            return true;
        }

        /// <summary>Sets the specified script instance inactive.</summary>
        /// <remarks>This causes the script to be removed from the set of scripts given to the scheduler thread for execution</remarks>
        /// <param name="routingGuid">Instance identifier</param>
        public void SetInactive(string routingGuid)
        {
            this.runningScripts.Remove(routingGuid);
        }

        public bool Contains(string routingGuid)
        {
            return allScripts.Contains(routingGuid);
        }

        public bool IsActive(string routingGuid)
        {
            return this.runningScripts.Contains(routingGuid);
        }

        public RuntimeScriptInfo GetScriptInfo(string routingGuid)
        {
            return allScripts[routingGuid] as RuntimeScriptInfo;
        }

        public ArrayList GetActiveScripts()
        {
            lock(this.runningScripts.SyncRoot)
            {
                return new ArrayList(this.runningScripts.Values);
            }
        }

        public ArrayList GetAllScripts()
        {
            lock(this.allScripts.SyncRoot)
            {
                return new ArrayList(this.allScripts.Values);
            }
        }

        public void Remove(string routingGuid)
		{
			allScripts.Remove(routingGuid);
            runningScripts.Remove(routingGuid);
        }

        public void Clear()
        {
            allScripts.Clear();
            runningScripts.Clear();
		}

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return new ArrayList(allScripts.Values).GetEnumerator();
		}

		#endregion
	}
}
