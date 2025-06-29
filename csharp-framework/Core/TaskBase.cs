using System;
using System.Diagnostics;
using System.Threading;

using Metreos.LoggingFramework;
using Metreos.Interfaces;

namespace Metreos.Core
{
    /// <summary>
    /// Abstract base class for a "task". Tasks are, essentially, 
    /// runnable objects. They contain a thread, have a name, and
    /// are capable of writing log messgaes to the logger.
    /// 
    /// Derived classes implement Run() which will be kicked off by
    /// the thread entry point, InternalRunHook, when Start() is called.
    /// </summary>
    public abstract class TaskBase : Loggable
    {
        /// <summary>The primary execution thread.</summary>
        private Thread taskThread;
        
        /// <summary>The friendly name for this task.</summary>
        private string name;

        /// <summary>The type of this task</summary>
        private IConfig.ComponentType componentType;        

        /// <summary>Get the name of this task.</summary>
        public string Name { get { return name; } }

        /// <summary>Get the type of this task.</summary>
        public IConfig.ComponentType Type { get { return componentType; } }

        /// <summary>
        /// Determine whether this task's thread is currently alive.
        /// </summary>
        public bool IsThreadAlive
        {
            get { return this.taskThread.IsAlive; }
        }

		/// <summary>
		/// Refreshes configurable TaskBase items (i.e. the log)
		/// </summary>
		protected void RefreshConfiguration(TraceLevel newLevel)
		{
			base.SetLogLevel(newLevel);
		}

        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="taskName">The name of the task.</param>
        public TaskBase(IConfig.ComponentType type, string taskName, string displayName, TraceLevel traceLevel)
            : base(traceLevel, displayName)
        {
            this.componentType = type;
            this.name = taskName;

            taskThread = new Thread(new ThreadStart(this.InternalRunHook));
            taskThread.IsBackground = true;
            taskThread.Name = taskName;
        }

        /// <summary>Start the task.</summary>
        public virtual bool Start()
        {
            // Kick off thread execution.
            taskThread.Start();

            return true;
        }

        /// <summary>Cleanup this tasks resources.</summary>
        public override void Dispose()
        {            
            if(taskThread.IsAlive)
            {
                taskThread.Abort();
            }
        }

        /// <summary>User defined thread execution method.</summary>
        protected abstract void Run();

        /// <summary>
        /// Thread entry point. Simple hook used to log thread IDs and entry/exits.
        /// </summary>
        private void InternalRunHook()
        {
            log.Write(TraceLevel.Verbose, "Thread beginning: " + Thread.CurrentThread.ManagedThreadId);

            this.Run();

            log.Write(TraceLevel.Verbose, "Thread exiting: " + Thread.CurrentThread.ManagedThreadId);
        }
    }
}
