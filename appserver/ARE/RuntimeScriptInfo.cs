using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Core;
using Metreos.Messaging;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;

namespace Metreos.AppServer.ARE
{
	/// <summary>Runtime script metadata</summary>
	public class RuntimeScriptInfo
	{
        public enum RunState
        {
            Idle,
            Running,
            WaitingForResponse,
            WaitingForEvent,
			DebugBreak,
            Forwarded,
            Sleeping
        }

        public readonly LogWriter log;

        public ScriptData script;
        public SessionData sessionData;
        public string routingGuid = null;

        // REFACTOR: This should be a config item
        private bool diagStateChanges = false;

        private volatile RunState state;
        public RunState State 
        {
            get { return state; }
            set 
            {
                if(diagStateChanges)
                    log.Write(TraceLevel.Verbose, "Changing state to: {0}", value);

                state = value;
            }
        }

        public volatile object StateLock;

        public long currTimeout = 0;

        // Stack of RuntimeFunctionInfo
        public readonly Stack callStack;
        public RuntimeFunctionInfo currFunction; 

        // Events waiting to be processed
        public readonly Queue unhandledEvents;

        // Response to current action
        public object ResponseLock;
        private ResponseMessage response;
        public ResponseMessage Response
        {
            get { lock(ResponseLock) { return response; } }
            set { lock(ResponseLock) { response = value; } }
        }

        public string ScriptName { get { return script != null ? script.name : null; } }
        
        public bool Running { get { return script != null; } }

        #region Debugging info

        public enum StepType { None, Into, Over }

        private bool debugging = false;
        public bool Debugging
        {
            get { return debugging; }
            set 
            {
                debugging = value;
                if(debugging == false)
                {
                    breakpointActionIds.Clear();
                    actionStack.Clear();
                    debugCommandMsg = null;
                    debugStep = StepType.None;
                    debugBreak = false;
                }
            }
        }

        // Auto-reset property
        private StepType debugStep = StepType.None;
        public StepType DebugStep
        {
            set { debugStep = value; }
            get 
            {
                StepType oldValue = debugStep;
                debugStep = StepType.None;
                return oldValue; 
            }
        }

        // Auto-reset property
        private bool debugBreak = false;
        public bool DebugBreak
        {
            set { debugBreak = value; }
            get
            {
                bool oldValue = debugBreak;
                debugBreak = false;
                return debugBreak;
            }
        }

        public readonly StringCollection breakpointActionIds;
        public CommandMessage debugCommandMsg;
        public readonly Stack actionStack;
        
        #endregion

		public RuntimeScriptInfo(LogWriter log)
		{
            this.log = log;

            this.currFunction = new RuntimeFunctionInfo();
            this.unhandledEvents = Queue.Synchronized(new Queue());
            this.callStack = new Stack();
			this.breakpointActionIds = new StringCollection();
            this.actionStack = new Stack();
            this.state = RunState.Idle;
            this.StateLock = new object();
            this.ResponseLock = new object();
		}

        public void Clear()
        {
            callStack.Clear();
            breakpointActionIds.Clear();

            if(sessionData != null)
            {
                sessionData.Clear();
                sessionData = null;
            }

            if(currFunction != null)
            {
                currFunction.Clear();
                currFunction = null;
            }

            script = null;
        }
	}
}
