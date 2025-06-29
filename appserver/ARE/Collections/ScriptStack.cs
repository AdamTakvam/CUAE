using System;
using System.Threading;
using System.Diagnostics;
using System.Collections;

using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;

namespace Metreos.AppServer.ARE.Collections
{
    public sealed class ScriptStack
    {
        private LogWriter log;
        private string scriptName;
        private ScriptData masterScript;
        private Stack stack;

        public int Count { get { return stack.Count; } }

        public object SyncRoot { get { return stack.SyncRoot; } }

        public ScriptData MasterScript { get { return masterScript; } }

        public ScriptStack(string scriptName, ScriptData masterScript, LogWriter log) 
        {
            this.log = log;
            this.scriptName = scriptName;
            this.masterScript = masterScript;

            this.stack = Stack.Synchronized(new Stack());

            CloneScript();
        }

        /// <summary>Returns a reset script to the pool for reuse</summary>
        /// <param name="script">A reset script instance</param>
        public void Push(ScriptData script)
        {
            lock(SyncRoot)
            {
                stack.Push(script);

                // Notify anyone waiting that a new script is available
                Monitor.Pulse(SyncRoot);
            }
        }

        /// <summary>Returns a clean script instance and initiates a new copy</summary>
        public ScriptData Pop()
        {
            lock(SyncRoot)
            {
//                log.Write(TraceLevel.Verbose, "looking for script {0}, {1} copies in repository. From {2}",
//					scriptName, Count, Environment.StackTrace);

				// NOTE: there is no guaranty that the requests for scripts
				// will be processed in order. this might lead to an unlucky
				// caller being denied even though there is plenty of capacity
				// in the system for manufacturing new scripts.

				long now = Metreos.Utilities.HPTimer.Now();
				long t1 = now + 5000000000L;
				while (Count == 0 && now < t1)
				{
					int delay = (int)((t1-now)/1000000);
					if (delay <= 0)
						delay = 1;
					Monitor.Wait( SyncRoot, delay );
					now = Metreos.Utilities.HPTimer.Now();
				}

				if (Count == 0)
				{
					log.Write(TraceLevel.Error, "looking for script {0}, never got a copy", scriptName);
					return null;
				}

				ScriptData sd = (ScriptData) stack.Pop();
				
				if (Count == 0)
					ThreadPool.QueueUserWorkItem(new WaitCallback(CloneScript));
				
//				log.Write(TraceLevel.Verbose, "finally got a copy of script {0}",
//					scriptName);

				return sd;
            }
        }

        /// <summary>Makes a copy of the current script and add it to the stack</summary>
        private void CloneScript()
        {
			// Cloning is a heavy-weight operation
//			log.Write(TraceLevel.Verbose, "making a copy of script {0}", scriptName);
			ScriptData sd = masterScript.Clone();
//			log.Write(TraceLevel.Verbose, "made a copy of script {0}", scriptName);
			Push(sd);
        }

        /// <summary>Makes a copy of the current script and add it to the stack</summary>
        /// <remarks>
        /// This overload is only to make the WaitCallBack delegate happy
        /// </remarks>
        private void CloneScript(object unused)
        {
            CloneScript();
        }
	}
}
