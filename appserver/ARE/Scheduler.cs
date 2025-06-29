using System;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;

using Reflect=System.Reflection;
using Utils=Metreos.Utilities;
using AppCollections=Metreos.ApplicationFramework.Collections;
using AppXml=Metreos.AppArchiveCore.Xml;

using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Configuration;
using Metreos.AppServer.ARE.Collections;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.ApplicationFramework.Loops;
using Metreos.ApplicationFramework.Actions;
using Metreos.ApplicationFramework.ActionParameters;
using Metreos.ApplicationFramework.ResultData;
using Metreos.Utilities;

namespace Metreos.AppServer.ARE
{
    public delegate void VoidDelegate();
    public delegate void HandleProviderActionDelegate(RuntimeScriptInfo scriptInfo, ProviderAction pAction, string actionGuid);
    public delegate void HandleEndSessionDelegate(string sessionGuid);
    public delegate void RecycleScriptDelegate(ScriptData script, string routingGuid);
    public delegate void ScriptInfoDelegate(RuntimeScriptInfo scriptInfo);
    public delegate void HandleDebugResponseDelegate(RuntimeScriptInfo scriptInfo, string nextActionId);
    public delegate void HandleStopDebuggingDelegate(RuntimeScriptInfo scriptInfo, string errorDescription);
    public delegate void SendNoHandlerDelegate(EventMessage msg, bool sessionActive);
    public delegate void SendDestructorEventDelegate(string routingGuid, IApp.DestructorCodes errorCode, string errorText);

    public sealed class Scheduler : IDisposable
	{
        #region Constants

        private abstract class Consts
        {
            // Default number of threads to initialize the pool with
            public const int InitThreadPoolSize         = 5;
            public const int InitSleepPoolSize          = 1;
            public const int MaxSleepPoolSize           = 3;

            public const int DefaultActionTimeout       = 5;  // secs
            public const int DefaultShutdownTimeout     = 30; // secs

            public const string MaxThreadsConfig        = "MaxThreads";
            public const string ShutdownTimeoutConfig   = "AppShutdownTimeout";
        }
        #endregion

        // SchedulerTask callbacks
        public HandleProviderActionDelegate providerActionHandler = null;
        public HandleEndSessionDelegate endSessionHandler = null;
        public VoidDelegate enableApplication = null;
        public RecycleScriptDelegate recycleScriptHandler = null;
        public SendNoHandlerDelegate sendNoHandler = null;
        public ScriptInfoDelegate repostForwardedEvents = null;
        public SendDestructorEventDelegate sendDestructorEvent = null;

        // Debug callbacks
		public ScriptInfoDelegate hitBreakpointHandler = null;
		public HandleDebugResponseDelegate detailedRespHandler = null;
        public HandleStopDebuggingDelegate stopDebuggingHandler = null;

        // Script scheduler core collection
        private readonly ScriptCollection scripts;

		// List of scripts to debug next time they start
		private readonly IDictionary debugScriptTable;

        // The execution thread pool
        private readonly Utils.ThreadPool threadPool;

        // Sleep timers
        private TimerManager timerManager;

        // The action execution delegate
        private readonly Utils.WorkRequestDelegate executeCurrentAction;

        // Configuration database
        private readonly Config configUtility;

        private readonly Thread schedulerThread;
        private volatile bool shutdownRequested;

        /// <summary>The generic log writer</summary>
        /// <remarks>Each script has its own, more specific, log writer</remarks>
        private readonly LogWriter gLog;

        #region Startup/Shutdown/Refresh/Dispose
        public Scheduler(LogWriter log)
		{
            Utils.Assertion.Check(log != null, "Cannot create Scheduler with null log writer");

            this.gLog = log;
            this.configUtility = Config.Instance;

            this.scripts = new ScriptCollection();
			this.debugScriptTable = ReportingDict.Wrap( "Scheduler.debugScriptTable", new Hashtable() );

            // Create thread pool
			int maxThreads = 0;
			try
			{
				maxThreads = (int)configUtility.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.ARE, Consts.MaxThreadsConfig);
			}
			catch {}
			finally
			{
				if(maxThreads < Consts.InitThreadPoolSize)
				{
					log.Write(TraceLevel.Warning, "Invalid value for ApplicationEnvironment ThreadPool size. Using default: " + 
                        Consts.InitThreadPoolSize);
					maxThreads = Consts.InitThreadPoolSize;
				}
			}

            threadPool = new Utils.ThreadPool(Consts.InitThreadPoolSize, maxThreads, "ARE Thread Pool");
            threadPool.Priority = System.Threading.ThreadPriority.AboveNormal;
            threadPool.MessageLogged += new LogDelegate(log.Write);
            threadPool.NewThreadTrigger = 400;

            executeCurrentAction = new Utils.WorkRequestDelegate(ExecuteCurrentAction);

            // Create scheduler thread
            this.schedulerThread = new Thread(new ThreadStart(Run));
            this.schedulerThread.IsBackground = true;
            this.schedulerThread.Name = "Application Scheduler";
            this.schedulerThread.Priority = ThreadPriority.AboveNormal;

			ReportingDict.Interval = 0;
		}

        public void Startup()
        {
            gLog.LogName = AppEnvironment.AppMetaData.Name;

            timerManager = new TimerManager(AppEnvironment.AppMetaData.Name + " Sleep Timers",
                new WakeupDelegate(OnSleepTimerFire), null, Consts.InitSleepPoolSize, Consts.MaxSleepPoolSize);

            threadPool.Start();

            shutdownRequested = false;
            schedulerThread.Start();
        }

        public void Shutdown()
        {
            // Cancel all pending sleep timers
            timerManager.Shutdown();

            // Complete all sleep actions and call instance destructors
            foreach(RuntimeScriptInfo scriptInfo in this.scripts.GetAllScripts())
            {
                HandleEndScript(scriptInfo, IApp.DestructorCodes.Shutdown, null);
            }

            lock(scripts)
            {
                if(scripts.TotalCount > 0)
                {
                    int shutdownTimeoutSecs = 0;
                    try
                    {
                        shutdownTimeoutSecs = (int) configUtility.GetEntryValue(IConfig.ComponentType.Core, IConfig.CoreComponentNames.ARE, Consts.ShutdownTimeoutConfig);
                    }
                    catch { }
                    finally
                    {
                        if(shutdownTimeoutSecs == 0)
                            shutdownTimeoutSecs = Consts.DefaultShutdownTimeout;

                        TimeSpan shutdownTimeout = new TimeSpan(0, 0, shutdownTimeoutSecs);

                        // Will be pulsed either when number of scripts is zero
                        //  or the shutdown timer fires
                        Monitor.Wait(scripts, shutdownTimeout);
                    }
                }

                scripts.Clear();
            }

            // Wait up to 5 seconds for the Scheduler thread to finish
            gLog.Write(TraceLevel.Info, "Stopping scheduler thread...");
            shutdownRequested = true;
            if(schedulerThread.Join(new TimeSpan(0, 0, 2)) == false)
            {
                gLog.Write(TraceLevel.Warning, "Scheduler thread did not shutdown gracefully. Aborting...");
                schedulerThread.Abort();
            }

            if(threadPool.StopAndWait(2000) == false)
            {
                gLog.Write(TraceLevel.Warning, "Application thread pool did not shutdown gracefully");
                threadPool.Stop();
            }

            gLog.Write(TraceLevel.Info, "Scheduler shutdown complete");
        }

        public void RefreshConfiguration()
        {
        }

        public void Dispose()
        {
    		debugScriptTable.Clear();            
			scripts.Clear();
        }
        #endregion

        #region Event Handlers

        public bool StartScript(ScriptData script, SessionData sessionData, EventMessage eventMsg)
        {
//			long t0 = HPTimer.Now();

            Utils.Assertion.Check(script != null, "Cannot queue null script into scheduler");
            Utils.Assertion.Check(sessionData != null, "Cannot start script with null sessionData");

            // Create script instance-specific log writer
            string logName = String.Format("{0}-{1}", script.name, sessionData.InstanceId);
            LogWriter scriptLog = new LogWriter(this.gLog.LogLevel, logName);

//			scriptLog.Write(TraceLevel.Warning, "starting script {0} checkpoint 1: {1}",
//				eventMsg.ScriptName, HPTimer.MillisSince( t0 ));

			// Initialize script-level variables
			if(InitializeScriptVariables(script, sessionData.PartitionName, sessionData, scriptLog) == false)
			{
				scriptLog.Write(TraceLevel.Error, "Script-level variables could not be initialized for '{0}'. Script will not execute.", 
					script.name);
				return false;
			}

//			scriptLog.Write(TraceLevel.Warning, "starting script {0} checkpoint 2: {1}",
//				eventMsg.ScriptName, HPTimer.MillisSince( t0 ));
			
			// Get a handle to the triggering event handler
            EventInfo eInfo = script.handledEvents.GetTriggeringEvent();

            if(eInfo == null)
            {
                scriptLog.Write(TraceLevel.Error, "Script '{0}' has no triggering event handler.", script.name);
                return false;
			}

//			scriptLog.Write(TraceLevel.Warning, "starting script {0} checkpoint 3: {1}",
//				eventMsg.ScriptName, HPTimer.MillisSince( t0 ));

            Function trigHandler = script.functions[eInfo.functionId] as Function;

            if(trigHandler == null)
            {
                scriptLog.Write(TraceLevel.Error, "Error locating triggering event handler '{0}' in script '{1}'",
                    eInfo.functionId, script.name);
                return false;
			}

//			scriptLog.Write(TraceLevel.Warning, "starting script {0} checkpoint 4: {1}",
//				eventMsg.ScriptName, HPTimer.MillisSince( t0 ));

            RuntimeScriptInfo scriptInfo = null;
            
            // Check to see if we want to debug this one
            if(debugScriptTable.Contains(script.name))
            {
                scriptInfo = debugScriptTable[script.name] as RuntimeScriptInfo;
                scriptInfo.Debugging = true;
                scriptInfo.log.LogName = logName;
                debugScriptTable.Remove(script.name);
            }
            else   
            {
                scriptInfo = new RuntimeScriptInfo(scriptLog);
            }

            scriptInfo.script = script;
            scriptInfo.sessionData = sessionData;
            scriptInfo.routingGuid = eventMsg.RoutingGuid;
            scriptInfo.currFunction.function = trigHandler;
            scriptInfo.State = RuntimeScriptInfo.RunState.Idle;
            scriptInfo.currFunction.CurrentElementId = trigHandler.firstActionId;

//			scriptLog.Write(TraceLevel.Warning, "starting script {0} checkpoint 5: {1}",
//				eventMsg.ScriptName, HPTimer.MillisSince( t0 ));

            if(InitializeFunctionParamVariables(eInfo.functionId, scriptInfo, trigHandler, eventMsg) == false)
            {
                scriptLog.Write(TraceLevel.Error, "Not all variables could be initialized in {0}:{1}. Script will not execute.", 
                    script.name, eInfo.functionId);
                return false;
            }

            if(gLog.LogLevel == TraceLevel.Verbose)
                scriptLog.Write(TraceLevel.Info, "Starting script '{0}' (i={1})", 
                    scriptInfo.routingGuid, this.scripts.TotalCount);
            else
                scriptLog.Write(TraceLevel.Info, "Starting script (i={0})", this.scripts.TotalCount);

			scripts.Add(scriptInfo);

//			scriptLog.Write(TraceLevel.Warning, "started script {0} in {1}", eventMsg.ScriptName, HPTimer.MillisSince( t0 ));

			return true;
        }

        public void HandleResponse(ResponseMessage msg)
        {
            RuntimeScriptInfo scriptInfo = scripts[msg.RoutingGuid];
            if(scriptInfo == null)
            {
                gLog.Write(TraceLevel.Warning, "Received a '{0}' response, but script is already gone: {1}",
                    msg.MessageId, msg.RoutingGuid);
                return;
            }

            if(msg.InResponseTo == IActions.Forward)
            {
                // Special case: 'Forward' request has finished being processed;
                //   the forwarded instance can now die.
                scriptInfo.log.Write(TraceLevel.Info, "Forward completed successfully");
                if(scriptInfo.currFunction == null ||
                    !scriptInfo.currFunction.function.IsDestructor)
                {
                    TerminateScript(scriptInfo, "Forward completed");
                }
                return;
            }
            
            if(msg.InResponseToActionGuid == null)
            {
                scriptInfo.log.Write(TraceLevel.Error, "Received malformed response:\n" + msg);
                return;
            }

            lock(scriptInfo.ResponseLock)
            {
                if( scriptInfo.Response != null && 
                    scriptInfo.Response.InResponseToActionGuid == msg.InResponseToActionGuid)
                {
                    scriptInfo.log.Write(TraceLevel.Warning, "Duplicate response received for: {0} ({1})", 
                        msg.InResponseTo, msg.InResponseToActionGuid);
                    return;
                }

                if(scriptInfo.currFunction.CurrentElementId != Utils.ActionGuid.GetActionId(msg.InResponseToActionGuid))
                {
                    scriptInfo.log.Write(TraceLevel.Warning, "Received response out of context: {0} ({1})",
                        msg.InResponseTo, msg.InResponseToActionGuid);
                    return;
                }

                scriptInfo.Response = msg;
            }
        }

        /// <summary>Locates the destination instance and adds the event to its queue</summary>
        /// <param name="msg">Non-triggering event</param>
        /// <returns>Whether or not the event was handled. If false, repost the event for handling later</returns>
        public bool HandleNonTriggeringEvent(EventMessage msg)
        {
            gLog.Write(TraceLevel.Info, "Enqueuing non-triggering event: " + msg.MessageId);

            RuntimeScriptInfo scriptInfo = scripts[msg.RoutingGuid];
            if(scriptInfo == null)
            {
                gLog.Write(TraceLevel.Warning, "Discarding non-triggering event '{0}' for ended script instance: {1}",
                    msg.MessageId, msg.RoutingGuid);
                return true;
            }

            if(scriptInfo.State == RuntimeScriptInfo.RunState.Forwarded)
                return false;

            scriptInfo.unhandledEvents.Enqueue(msg);
            
            if(!scripts.SetActive(scriptInfo.routingGuid))
            {
                gLog.Write(TraceLevel.Warning, "Discarding non-triggering event '{0}' for ended script instance: {1}",
                    msg.MessageId, msg.RoutingGuid);
            }
            return true;
        }
        #endregion

        #region Scheduler Thread
        private void Run()
        {
            try
            {
                while(!shutdownRequested)
                {
                    Thread.Sleep(1);

                    // Execute one action for each active script
                    foreach(RuntimeScriptInfo scriptInfo in scripts.GetActiveScripts())
                    {
                        //scriptInfo.log.Write(TraceLevel.Info, "action={0} ({1}), state={2}",
                        //    ((ActionBase) scriptInfo.currFunction.CurrentElement).name,
                        //    scriptInfo.currFunction.CurrentElementId, scriptInfo.State);

                        lock(scriptInfo.StateLock)
                        {
                            switch(scriptInfo.State)
                            {
                                case RuntimeScriptInfo.RunState.Running:
                                case RuntimeScriptInfo.RunState.DebugBreak:
                                case RuntimeScriptInfo.RunState.Forwarded:
                                case RuntimeScriptInfo.RunState.Sleeping:
                                    // nothing to do
                                    break;
                                case RuntimeScriptInfo.RunState.WaitingForResponse:
                                    if(EvaluateResponse(scriptInfo) == true)
                                    {
                                        scriptInfo.State = RuntimeScriptInfo.RunState.Running;
                                        threadPool.PostRequest(executeCurrentAction, scriptInfo);
                                    }
                                    break;
                                case RuntimeScriptInfo.RunState.WaitingForEvent:
                                    if(EvaluateNonTriggeringEvent(scriptInfo) == true)
                                    {
                                        scriptInfo.State = RuntimeScriptInfo.RunState.Running;
                                        threadPool.PostRequest(executeCurrentAction, scriptInfo);
                                    }
                                    break;
                                case RuntimeScriptInfo.RunState.Idle:
                                    scriptInfo.State = RuntimeScriptInfo.RunState.Running;
                                    threadPool.PostRequest(executeCurrentAction, scriptInfo);
                                    break;
                                default:
                                    gLog.Write(TraceLevel.Warning, "Script '{0}' unknown state: {1}",
                                        scriptInfo.routingGuid, scriptInfo.State);
                                    break;
                            }
                        }
                    }
                }
            }
            catch(ThreadAbortException)
            {
                // Do nothing
            }
            catch(Exception e)
            {
                gLog.Write(TraceLevel.Error, "Scheduler threw an unhandled exception:\n" + e);
                throw e;
            }

            gLog.Write(TraceLevel.Info, "Scheduler thread ended normally");
        }

        private bool EvaluateResponse(RuntimeScriptInfo scriptInfo)
        {
			// Is this expensive? Why not calculate it once and save it? - wert
            // It's a string concatenation. I guess we could... *shrug*  - APC
            string actionGuid = Utils.ActionGuid.Create(scriptInfo.routingGuid, scriptInfo.currFunction.CurrentElementId);

            bool success = false;

            lock(scriptInfo.ResponseLock)
            {
                if(scriptInfo.Response == null || scriptInfo.Response.InResponseToActionGuid != actionGuid)
                {
                    // The response has yet arrived. Have we timed out?
                    if(scriptInfo.currTimeout < HPTimer.Now())
                    {
                        Assertion.Check(scriptInfo.currTimeout > 0, "Provider action timeout is 0");

                        scriptInfo.log.Write(TraceLevel.Warning, "Action '{0}' timed-out waiting for a provider response",
                            scriptInfo.currFunction.CurrentElementId);

                        // Handle provider action timeout
                        return FinalizeAction(IApp.VALUE_TIMEOUT, scriptInfo);
                    }

                    scriptInfo.Response = null;  // Discard out-of-order response
                    return false;
                }

                scriptInfo.log.Write(TraceLevel.Verbose, "Got {0} response for '{1}' ({2})", 
                    scriptInfo.Response.MessageId, scriptInfo.Response.InResponseTo, scriptInfo.currFunction.CurrentElementId);

                // Evaluate result data
                ActionBase oldAction = scriptInfo.currFunction.CurrentElement as ActionBase;

                foreach(DictionaryEntry de in oldAction.resultData)
                {
                    string fieldName = de.Key as String;
                    ResultDataBase rData = de.Value as ResultDataBase;

                    Utils.Assertion.Check(rData != null, "ResultData object is null");

                    if(fieldName == null)
                        fieldName = IApp.FIELD_RETURN_VALUE;

                    // Throw an error if a ResultData value cannot be assigned
                    //   but tolerate it if this is some sort of failure response
                    if(!scriptInfo.Response.Contains(fieldName))
                    {
                        if(scriptInfo.Response.MessageId == IApp.VALUE_SUCCESS)
                        {
                            string error = String.Format("Could not resolve ResultData field '{0}' in action '{1}'",
                                fieldName, scriptInfo.currFunction.CurrentElementId);
                            scriptInfo.log.Write(TraceLevel.Error, error);
                            HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, error);
                            return false;
                        }
                        continue;
                    }

                    object responseValue = scriptInfo.Response[fieldName];

                    VariableResultData vrData = rData as VariableResultData;
                    if(vrData != null)
                    {
                        if(AssignVariableValue(scriptInfo, vrData.varName, responseValue) == false)
                        {
                            string error = String.Format("Could not assign {0}={1} in result of action '{2}'",
                                vrData.varName == null ? "<null>" : vrData.varName, responseValue == null ? "<null>" : responseValue,
                                scriptInfo.currFunction.CurrentElementId);
                            scriptInfo.log.Write(TraceLevel.Error, error);
                            HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, error);
                            return false;
                        }
                    }
                    else
                    {
                        string error = "User code result data not yet supported.";
                        scriptInfo.log.Write(TraceLevel.Error, error);
                        HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, error);
                        return false;
                    }
                }
                
                success = FinalizeAction(scriptInfo.Response.MessageId, scriptInfo);

                // Be sure to null the response so we don't get the wrong idea in a looping scenario
                scriptInfo.Response = null;
            }
            return success;
        }

        private bool EvaluateNonTriggeringEvent(RuntimeScriptInfo scriptInfo)
        {
            if(scriptInfo.unhandledEvents.Count == 0)
            {
                scripts.SetInactive(scriptInfo.routingGuid);
                return false;
            }

            EventMessage newEvent = scriptInfo.unhandledEvents.Dequeue() as EventMessage;
            if(newEvent == null)
            {
                scriptInfo.log.Write(TraceLevel.Error, "Internal Error: Invalid item type in unhandled events queue");
                return false;
            }

            // Locate the handler for this event
            string functionId = scriptInfo.script.handledEvents.GetHandler(newEvent);

            if(functionId == null)
            {
                if(newEvent.MessageId != IEvents.Destruction)
                {
                    scriptInfo.log.Write(TraceLevel.Warning, "No handler registered for '{0}' event",
                        newEvent.MessageId, scriptInfo.ScriptName);

                    Utils.Assertion.Check(sendNoHandler != null, "sendNoHandler delegate not hooked in Scheduler");
                    sendNoHandler(newEvent, true);

                    // Recurse until we either find an event we can handle or run out of events
                    return EvaluateNonTriggeringEvent(scriptInfo);
                }
                else
                {
                    TerminateScript(scriptInfo, null);
                    return false;
                }
            }

            Function newFunction = scriptInfo.script.functions[functionId] as Function;

            if(newFunction == null)
            {
                scriptInfo.log.Write(TraceLevel.Error, "Error locating non-triggering event handler '{0}'", functionId);
                return false;
            }

            if(InitializeFunctionParamVariables(functionId, scriptInfo, newFunction, newEvent) == false)
            {
                HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, "Not all function parameter variables could be initialized");
                return false;
            }

            scriptInfo.currFunction = new RuntimeFunctionInfo();
            scriptInfo.currFunction.function = newFunction;
            scriptInfo.currFunction.CurrentElementId = newFunction.firstActionId;
            return true;
        }

        private void ExecuteCurrentAction(object state)
        {
            RuntimeScriptInfo scriptInfo = state as RuntimeScriptInfo;
            if(scriptInfo == null) { return; }

            ScriptElementBase currElement = scriptInfo.currFunction.CurrentElement;
            if(currElement == null)
            {
                string error = String.Format("Could not locate element '{0}'", scriptInfo.currFunction.CurrentElementId);
                scriptInfo.log.Write(TraceLevel.Error, error);
                HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, error);
                return;
            }

            ActionBase currAction = currElement as ActionBase;
            if(currAction == null)
            {
                // If one has an element which is not an action, one has a loop.
                //      ~ Ancient Chinese Proverb
                Loop loop = currElement as Loop;
                Utils.Assertion.Check(loop != null, "We encountered an unknown species of element deep within script " + scriptInfo.ScriptName);

                if(HandleLoop(scriptInfo, loop) == false)
                {
                    string error = String.Format("Error entering loop '{0}'", scriptInfo.currFunction.CurrentElementId);
                    scriptInfo.log.Write(TraceLevel.Error, error);
                    HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, error);
                    return;
                }

                // Recurse until we get to a non-loop action
                ExecuteCurrentAction(scriptInfo);
                return;
            }

            // Handle script debugging
            if(scriptInfo.Debugging)
            {
                // Send debug response, if applicable
                if(scriptInfo.debugCommandMsg != null)
                {
                    Utils.Assertion.Check(detailedRespHandler != null, "Detailed response handler not connected");
                    detailedRespHandler(scriptInfo, scriptInfo.currFunction.CurrentElementId);
                    scriptInfo.debugCommandMsg = null;
                }
                else if(scriptInfo.breakpointActionIds.Contains(scriptInfo.currFunction.CurrentElementId) ||  // Breakpoint
                    (scriptInfo.DebugBreak == true) ||                                                        // Break command
                    (scriptInfo.currFunction.BreakOnNextAction == true) ||                                    // StepOver function
                    (scriptInfo.DebugStep != RuntimeScriptInfo.StepType.None))                                // Other stepping
                {
                    // Break before executing this action?
                    Utils.Assertion.Check(hitBreakpointHandler != null, "HitBreakpoint handler not connected in scheduler");
                    hitBreakpointHandler(scriptInfo);
                    
                    lock(scriptInfo.StateLock)
                    {
                        scriptInfo.State = RuntimeScriptInfo.RunState.DebugBreak;
                    }
                    return;
                }

                // Add this action to action stack
                scriptInfo.actionStack.Push(scriptInfo.currFunction.CurrentElementId);
            }

            scriptInfo.log.Write(TraceLevel.Verbose, "Executing action '{0}' ({1})",
                currAction.name, scriptInfo.currFunction.CurrentElementId);

            ProviderAction pAction = currAction as ProviderAction;
            if(pAction != null)
            {
                ExecuteProviderAction(pAction, scriptInfo);
                return;
            }

            NativeAction nAction = currAction as NativeAction;
            if(nAction != null)
            {
                ExecuteNativeAction(nAction, scriptInfo);
                return;
            }

            UserCodeAction uAction = currAction as UserCodeAction;
            if(uAction != null)
            {
                ExecuteUserCodeAction(uAction, scriptInfo);
                return;
            }

            scriptInfo.log.Write(TraceLevel.Error, "Inexplicable error trying to execute action '{0}'",
                scriptInfo.currFunction.CurrentElementId);
        }

        private bool HandleLoop(RuntimeScriptInfo scriptInfo, Loop loop)
        {
            // Are we repeating the current loop, 
            //   or are we entering a new loop?
            if(scriptInfo.currFunction.InLoop == true)
            {
                RuntimeLoopInfo currLoopInfo = scriptInfo.currFunction.CurrentLoop;
                if(currLoopInfo.loop.Equals(loop))
                {
                    // We're repeating, so increment the enumerator and check boundaries
                    if(loop.loopCount.enumType == LoopCountBase.EnumerationType.Int)
                    {
                        currLoopInfo.loopIndex++;
                        if(currLoopInfo.loopIndex < currLoopInfo.loopLimit)
                        {
                            scriptInfo.currFunction.CurrentElementId = loop.firstActionId;
                        }
                        else
                        {
                            scriptInfo.currFunction.ExitLoop();

                            string error = null;
                            string nextActionId = 
                                GetNextActionId(scriptInfo, scriptInfo.currFunction.LastReturnValue, out error);
                            if(nextActionId == null)
                            {
                                scriptInfo.log.Write(TraceLevel.Error, error);
                                HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, error);
                            }
                            else
                            {
                                scriptInfo.currFunction.CurrentElementId = nextActionId;
                            }
                        }
                    }
                    else
                    {
                        IEnumerator e = null;
                        if(loop.loopCount.enumType == LoopCountBase.EnumerationType.Enum)
                        {
                            e = currLoopInfo.loopEnum;
                        }
                        if(loop.loopCount.enumType == LoopCountBase.EnumerationType.DictEnum)
                        {
                            e = currLoopInfo.loopDictEnum;
                        }

                        if(e.MoveNext() == true)
                        {
                            scriptInfo.currFunction.CurrentElementId = loop.firstActionId;
                        }
                        else
                        {
                            scriptInfo.currFunction.ExitLoop();

                            string error = null;
                            string nextActionId = 
                                GetNextActionId(scriptInfo, scriptInfo.currFunction.LastReturnValue, out error);
                            if(nextActionId == null)
                            {
                                scriptInfo.log.Write(TraceLevel.Error, error);
                                HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, error);
                            }
                            else
                            {
                                scriptInfo.currFunction.CurrentElementId = nextActionId;
                            }
                        }
                    }
                    return true;
                }
            }

            RuntimeLoopInfo loopInfo = CreateRuntimeLoopInfo(scriptInfo, loop);

            if(loopInfo == null)
            {
                return false;
            }

            scriptInfo.currFunction.EnterLoop(loopInfo);
            scriptInfo.currFunction.CurrentElementId = loop.firstActionId;
            return true;
        }

        private void ExecuteProviderAction(ProviderAction action, RuntimeScriptInfo scriptInfo)
        {
            Utils.Assertion.Check(providerActionHandler != null, "SchedulerTask did not hook the Scheduler.actionHandler delegate");

            if(EvaluateCurrentActionParameters(scriptInfo) == false)
            {
                HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, "Not all parameters could be evaluated for this action");
                return;
            }

            // Is it an ApplicationControl action?
            switch(action.name)
            {
                case IActions.EndScript:
                    HandleEndScript(scriptInfo, IApp.DestructorCodes.Normal, null);
                    break;
                case IActions.EndSession:
                    HandleEndScript(scriptInfo, IApp.DestructorCodes.Normal, null);
                    break;
                case IActions.EndFunction:
                    HandleEndFunction(scriptInfo);
                    break;
                case IActions.CallFunction:
                    HandleCallFunction(action, scriptInfo);
                    break;
                case IActions.ConstructionComplete:
                    HandleConstructionComplete(action, scriptInfo);
                    break;
                case IActions.SetSessionData:
                    HandleSetSessionData(action, scriptInfo);
                    break;
                case IActions.ChangeLocale:
                    HandleChangeLocale(action, scriptInfo);
                    break;
                case IActions.Sleep:
                    HandleSleep(action, scriptInfo);
                    break;
                case IActions.Forward:
                    HandleForwardScript(action, scriptInfo);
                    break;
                default:
                    // Set time at which this action should give up waiting for a response
                    if(action.timeout > 0)
                        scriptInfo.currTimeout = HPTimer.AddTime(HPTimer.Now(), 0, action.timeout, 0);
                    else
                        scriptInfo.currTimeout = HPTimer.AddTime(HPTimer.Now(), 0, Consts.DefaultActionTimeout, 0);

                    lock(scriptInfo.StateLock)
                    {
                        scriptInfo.State = RuntimeScriptInfo.RunState.WaitingForResponse;
                    }

                    // Send to router
                    string actionGuid = Utils.ActionGuid.Create(scriptInfo.routingGuid, scriptInfo.currFunction.CurrentElementId);
                    providerActionHandler(scriptInfo, action, actionGuid);
                    break;
            }
        }

        private void ExecuteNativeAction(NativeAction action, RuntimeScriptInfo scriptInfo)
        {
            action.actionInstance.Log = scriptInfo.log;

            if(EvaluateCurrentActionParameters(scriptInfo) == false)
            {
                HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, "Not all parameters could be evaluated for this action");
                return;
            }

			if(AssignNativeActionParams(action.actionInstance, scriptInfo) == false)
			{
                HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, "Parameter type mismatch");
				return;
			}

            string returnValue = null;
            try
            {
			    if(action.actionInstance.ValidateInput() == false)
                {
                    string error = String.Format("Failed to execute native action {0}, id {1}. Error: Invalid input parameters.",
                        action.name, scriptInfo.currFunction.CurrentElementId);
                    scriptInfo.log.Write(TraceLevel.Error, error);
                    HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, error);
                    return;
                }

				returnValue = action.actionInstance.Execute(scriptInfo.sessionData, configUtility);
			}
			catch(Exception e)
			{
                string error = String.Format("Failed to execute native action {0}, id {1}. Error: {1}",
                    action.name, scriptInfo.currFunction.CurrentElementId, Utils.Exceptions.FormatException(e));
				scriptInfo.log.Write(TraceLevel.Error, error);
				HandleEndScript(scriptInfo, IApp.DestructorCodes.Exception, error);
				return;
			}

            // Evaluate result data
            Hashtable results = GetNativeActionResultData(action.actionInstance);

            foreach(DictionaryEntry de in action.resultData)
            {
                string fieldName = de.Key as String;
                ResultDataBase rData = de.Value as ResultDataBase;

                Utils.Assertion.Check(rData != null, "ResultData object is null");

                if(fieldName == null)
                {
                    fieldName = IApp.FIELD_RETURN_VALUE;
                }

                if(results.Contains(fieldName) == false)
                {
                    string error = String.Format("Could not resolve ResultData field '{0}' in action '{1}'",
                        fieldName, scriptInfo.currFunction.CurrentElementId);
                    scriptInfo.log.Write(TraceLevel.Error, error);
                    HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, error);
                    return;
                }

                object resultValue = results[fieldName];

                VariableResultData vrData = rData as VariableResultData;
                if(vrData != null)
                {
                    if(AssignVariableValue(scriptInfo, vrData.varName, resultValue) == false)
                    {
                        string error = String.Format("Could not assign {0}={1} in result of action '{2}'",
                            vrData.varName == null ? "<null>" : vrData.varName, resultValue == null ? "<null>" : resultValue,
                            scriptInfo.currFunction.CurrentElementId);
                        scriptInfo.log.Write(TraceLevel.Error, error);
                        HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, error);
                        return;
                    }
                }
                else
                {
                    string error = "User code result data not yet supported.";
                    scriptInfo.log.Write(TraceLevel.Error, error);
                    HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, error);
                    return;
                }
            }

            // Clear any sludge from the action properties
            try
            {
                action.actionInstance.Clear();
                action.actionInstance.Log = null;
            }
            catch(Exception e)
            {
                scriptInfo.log.Write(TraceLevel.Warning, "Exception thrown by Clear() method of native action {0}. Error: {1}",
                    scriptInfo.currFunction.CurrentElementId, e.Message);
            }

			FinalizeAction(returnValue, scriptInfo);
        }

        private void ExecuteUserCodeAction(UserCodeAction action, RuntimeScriptInfo scriptInfo)
        {
            // UserCode action have no parameters, but we have to figure out the function signature
            // So what do we do? No, no, we don't freak out. We use reflection.
            System.Reflection.ParameterInfo[] parameters = action.userCode.GetParameters();
            object[] arguments = new object[parameters.Length];

            for(int i=0; i<parameters.Length; i++)
            {
                System.Reflection.ParameterInfo parameter = parameters[i];

				object argValue = null;
                if(parameter.ParameterType.Name == typeof(SessionData).Name)
                {
                    argValue = scriptInfo.sessionData;
                }
                else if(parameter.ParameterType.Name == typeof(LogWriter).Name) 
                { 
                    argValue = scriptInfo.log;
                }
                else if(parameter.Name == IApp.NAME_LOOP_INDEX)
                {
                    if(scriptInfo.currFunction.InLoop)
                    {
                        argValue = scriptInfo.currFunction.CurrentLoop.loopIndex;
                    }
                    else
                    {
                        argValue = 0;
                    }
                }
                else if(parameter.Name == IApp.NAME_LOOP_ENUM)
                {
                    if(scriptInfo.currFunction.InLoop)
                    {
                        argValue = scriptInfo.currFunction.CurrentLoop.loopEnum;
                    }
                    else
                    {
                        argValue = null;
                    }
                }
                else if(parameter.Name == IApp.NAME_LOOP_DICT_ENUM)
                {
                    if(scriptInfo.currFunction.InLoop)
                    {
                        argValue = scriptInfo.currFunction.CurrentLoop.loopDictEnum;
                    }
                    else
                    {
                        argValue = null;
                    }
                }
                else
                {
                    argValue = GetVariableValue(scriptInfo, parameter.Name);
                }

                arguments[i] = argValue;
            }

            string returnValue = null;
            try
            {
                returnValue = action.userCode.Invoke(this, arguments) as string;
            }
            catch(Exception e)
            {
                string error = String.Format("Unhandled exception detected inside user code action '{0}': '{1}'",
                    scriptInfo.currFunction.CurrentElementId, Utils.Exceptions.FormatException(e));
                scriptInfo.log.Write(TraceLevel.Error, error);
                HandleEndScript(scriptInfo, IApp.DestructorCodes.Exception, error);
                return;
            }

            if(returnValue == null)
            {
                scriptInfo.log.Write(TraceLevel.Warning, "No value returned from user code action '{0}'. Using default.",
                    scriptInfo.currFunction.CurrentElementId);
                returnValue = IApp.VALUE_DEFAULT;
            }

            // Save any ref or out parameters
            for(int i=0; i<parameters.Length; i++)
            {
				// Don't try to save back the SessionData or log even if they specified 'ref'
				if(parameters[i].ParameterType.Name == typeof(SessionData).Name) { continue; }
				if(parameters[i].ParameterType.Name == typeof(LogWriter).Name) { continue; }

                if(parameters[i].ParameterType.IsByRef)
                {
                    if(AssignVariableValue(scriptInfo, parameters[i].Name, arguments[i]) == false)
                    {
                        scriptInfo.log.Write(TraceLevel.Warning, "Unable to persist any changes to variable '{0}' made in user code action '{1}'",
                            parameters[i].Name, scriptInfo.currFunction.CurrentElementId);
                    }
                }
            }

			FinalizeAction(returnValue, scriptInfo);
        }

		private bool FinalizeAction(string returnValue, RuntimeScriptInfo scriptInfo)
		{
            lock(scriptInfo.StateLock)
            {
                // Move along...
                string error = null;
                string prevActionId = scriptInfo.currFunction.CurrentElementId;
                string nextActionId = GetNextActionId(scriptInfo, returnValue, out error);
                if(nextActionId == null)
                {
                    scriptInfo.log.Write(TraceLevel.Error, error);
                    HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, error);
                    return false;
                }
                else
                {
                    scriptInfo.currFunction.CurrentElementId = nextActionId;
                    scriptInfo.State = RuntimeScriptInfo.RunState.Idle;
                }

                return true;
            }
		}

        private void FinalizeAction_DebugOnly(RuntimeScriptInfo scriptInfo, bool callFunction, bool final)
        {
            Utils.Assertion.Check(scriptInfo != null, "scriptInfo is null in FinalizeAction_DebugOnly");
            
            if((callFunction == false) && (final == false)) { return; }
            if(scriptInfo.Debugging == false) { return; }
            
            string currActionId = scriptInfo.currFunction.CurrentElementId;
            ActionBase currAction = scriptInfo.currFunction.CurrentElement as ActionBase;

            if(currAction == null) { return; }
                
            // Send debug response, if applicable
            if(scriptInfo.debugCommandMsg != null)
            {
                Utils.Assertion.Check(detailedRespHandler != null, "Detailed response handler not connected");
                detailedRespHandler(scriptInfo, currActionId);
                scriptInfo.debugCommandMsg = null;
            }

            if(callFunction == false) { return; }

            // Set breakpoint at next action if we're stepping
            RuntimeScriptInfo.StepType debugStep = scriptInfo.DebugStep;
            if(debugStep != RuntimeScriptInfo.StepType.None)
            {
                if(debugStep == RuntimeScriptInfo.StepType.Into)
                {
                    scriptInfo.breakpointActionIds.Add(currActionId);
                }
                else
                {
                    RuntimeFunctionInfo prevFunction = scriptInfo.callStack.Peek() as RuntimeFunctionInfo;
                    prevFunction.BreakOnNextAction = true;
                }
            }
        }
        #endregion

        #region ApplicationControl action handlers

        private void HandleForwardScript(ProviderAction action, RuntimeScriptInfo scriptInfo)
        {
            // Put instance in Forwarded state
            HandleEndScript(scriptInfo, IApp.DestructorCodes.Forwarded, null);

            // Send Forward action to router
            string actionGuid = Utils.ActionGuid.Create(scriptInfo.routingGuid, scriptInfo.currFunction.CurrentElementId);
            providerActionHandler(scriptInfo, action, actionGuid);

            // Repost all unhandled events back to Router
            repostForwardedEvents(scriptInfo);
        }

		private void HandleEndScript(RuntimeScriptInfo scriptInfo, IApp.DestructorCodes exitCode, string errorDescription)
		{
            if(scriptInfo.currFunction.function.IsDestructor)
            {
                TerminateScript(scriptInfo, errorDescription);
            }
            else
            {
                // Clear event queue then add destructor event
                scriptInfo.unhandledEvents.Clear();
                sendDestructorEvent(scriptInfo.routingGuid, exitCode, errorDescription);
                scripts.SetActive(scriptInfo.routingGuid);
            }

            lock(scriptInfo.StateLock)
            {
                if(exitCode == IApp.DestructorCodes.Forwarded)
                    scriptInfo.State = RuntimeScriptInfo.RunState.Forwarded;
                else
                    scriptInfo.State = RuntimeScriptInfo.RunState.WaitingForEvent;
            }
        }

        public void TerminateScript(string routingGuid, string errorDescription)
        {
            RuntimeScriptInfo scriptInfo = this.scripts[routingGuid];
            if (scriptInfo != null &&
                scriptInfo.State != RuntimeScriptInfo.RunState.Forwarded)
            {
                // If script is forwarded, it will be terminated when router replies to the forward action
                TerminateScript(scriptInfo, errorDescription);
            }
        }

        private void TerminateScript(RuntimeScriptInfo scriptInfo, string errorDescription)
        {
			Utils.Assertion.Check(endSessionHandler != null, "SchedulerTask's end session handler not hooked up");

			endSessionHandler(scriptInfo.routingGuid);

            if(scriptInfo.Debugging)
            {
                FinalizeAction_DebugOnly(scriptInfo, false, true);

                Utils.Assertion.Check(stopDebuggingHandler != null, "Stop Debugging delegate not connected in Scheduler");
                stopDebuggingHandler(scriptInfo, errorDescription);
            }

            lock(scripts)
            {
                scripts.Remove(scriptInfo.routingGuid);
                if(scripts.TotalCount == 0)
                    Monitor.Pulse(scripts);  // Release the wait on shutdown, if applicable
            }

            if(scriptInfo.log.LogLevel == TraceLevel.Verbose)
                scriptInfo.log.Write(TraceLevel.Info, "Script exited normally: {0}",
                    scriptInfo.routingGuid);
            else
                scriptInfo.log.Write(TraceLevel.Info, "Script exited normally");

            recycleScriptHandler(scriptInfo.script, scriptInfo.routingGuid);

            scriptInfo.Clear();
		}

        private void HandleEndFunction(RuntimeScriptInfo scriptInfo)
        {
            if(scriptInfo.callStack.Count == 0)
            {
                if(scriptInfo.Debugging)
                {
                    scriptInfo.actionStack.Clear();
                }

                lock(scriptInfo.StateLock)
                {
                    scriptInfo.State = RuntimeScriptInfo.RunState.WaitingForEvent;
                }
                return;
            }

            if(scriptInfo.Debugging)
            {
                // Remove all actions from this function from the action stack
                string actionId = scriptInfo.actionStack.Pop() as string;
                while(scriptInfo.currFunction.function.elements.Contains(actionId))
                {
                    actionId = scriptInfo.actionStack.Pop() as string;
                }
            }

            ActionBase efAction = scriptInfo.currFunction.CurrentElement as ActionBase;

            object returnValueObj = efAction.actionParams[IActions.Fields.ReturnValue];
            if(returnValueObj == null)
            {
                scriptInfo.log.Write(TraceLevel.Warning, "No return value specified for EndFunction action '{0}'. Using default...",
                    scriptInfo.currFunction.CurrentElementId);
                returnValueObj = IApp.VALUE_DEFAULT;
            }

            string returnValue = returnValueObj.ToString();

            scriptInfo.currFunction = scriptInfo.callStack.Pop() as RuntimeFunctionInfo;

            // Handle result data
            ActionBase cfAction = scriptInfo.currFunction.CurrentElement as ActionBase;

            foreach(DictionaryEntry de in cfAction.resultData)
            {
                string fieldName = de.Key as String;
                ResultDataBase rData = de.Value as ResultDataBase;

                Utils.Assertion.Check(rData != null, "ResultData object is null");

                if(fieldName == null)
                {
                    fieldName = IApp.FIELD_RETURN_VALUE;
                }

                object paramValue = efAction.actionParams[fieldName];
                if(paramValue == null)
                {
                    string error = String.Format("Could not resolve ResultData field '{0}' in action '{1}'",
                        fieldName, scriptInfo.currFunction.CurrentElementId);
                    scriptInfo.log.Write(TraceLevel.Error, error);
                    HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, error);
                    return;
                }

                VariableResultData vrData = rData as VariableResultData;
                if(vrData != null)
                {
                    if(AssignVariableValue(scriptInfo, vrData.varName, paramValue) == false)
                    {
                        string error = String.Format("Could not assign {0}={1} in result of action '{2}'",
                            vrData.varName == null ? "<null>" : vrData.varName, paramValue,
                            scriptInfo.currFunction.CurrentElementId);
                        scriptInfo.log.Write(TraceLevel.Error, error);
                        HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, error);
                        return;
                    }
                }
                else
                {
                    string error = "User code result data not yet supported.";
                    scriptInfo.log.Write(TraceLevel.Error, error);
                    HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, error);
                    return;
                }
            }

            FinalizeAction(returnValue, scriptInfo);
        }

        private void HandleCallFunction(ProviderAction action, RuntimeScriptInfo scriptInfo)
        {
            string functionName = action.actionParams[IActions.Fields.FunctionName] as string;

            if(functionName == null)
            {
                string error = String.Format("No function name specified in {0} action: {1}",
                    IActions.CallFunction, scriptInfo.currFunction.CurrentElementId);
                scriptInfo.log.Write(TraceLevel.Error, error);
                HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, error);
                return;
            }

            Function newFunction = scriptInfo.script.functions[functionName] as Function;

            if(newFunction == null)
            {
                string error = String.Format("Could not find function '{0}'", functionName);
                scriptInfo.log.Write(TraceLevel.Error, error);
                HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, error);
                return;
            }

            if(InitializeFunctionParamVariables(functionName, scriptInfo, newFunction, action) == false)
            {
                HandleEndScript(scriptInfo, IApp.DestructorCodes.ScriptError, "Not all parameters could be initialized for function call");
                return;
            }

            lock(scriptInfo.StateLock)
            {
                scriptInfo.callStack.Push(scriptInfo.currFunction);
                scriptInfo.currFunction = new RuntimeFunctionInfo();
                scriptInfo.currFunction.function = newFunction;
                scriptInfo.currFunction.CurrentElementId = newFunction.firstActionId;
                scriptInfo.State = RuntimeScriptInfo.RunState.Idle;
            }

            FinalizeAction_DebugOnly(scriptInfo, true, false);
        }

        private void HandleConstructionComplete(ProviderAction action, RuntimeScriptInfo scriptInfo)
        {
            bool success = false;
            if(action.actionParams.Contains(IActions.Fields.Success))
                success = (bool) ParseTo(typeof(bool), action.actionParams[IActions.Fields.Success]);

            if(success)
            {
                enableApplication();
                FinalizeAction(IApp.VALUE_SUCCESS, scriptInfo);
            }
            else
            {
                FinalizeAction(IApp.VALUE_FAILURE, scriptInfo);
            }           
        }

        private void HandleSetSessionData(ProviderAction action, RuntimeScriptInfo scriptInfo)
        {
            foreach(ActionParamBase aParam in action.actionParams)
            {
                scriptInfo.sessionData.CustomData[aParam.name] = aParam.Value;
            }
			
			FinalizeAction(IApp.VALUE_SUCCESS, scriptInfo);
        }

        private void HandleChangeLocale(ProviderAction action, RuntimeScriptInfo scriptInfo)
        {
            string newLocale = Convert.ToString(action.actionParams[IActions.Fields.Locale]);

            if (newLocale != null && 
                newLocale != String.Empty &&
                AppEnvironment.AppMetaData.Locales.Contains(newLocale) &&
                scriptInfo.sessionData.ChangeCulture(newLocale))
            {
                bool resetStrings = false;
                if(action.actionParams.Contains(IActions.Fields.ResetStrings))
                    resetStrings = (bool) ParseTo(typeof(bool), action.actionParams[IActions.Fields.ResetStrings]);

                if(resetStrings)
                {
                    // Enumerate global variables 
                    foreach(Variable var in scriptInfo.script.variables.Values)
                    {
                        // If they initialize from locale table, re-init them to new locale
                        if(var.InitWithType == Variable.InitWithTypes.Locale)
                            var.Assign(GetValueFromStringTable(var, scriptInfo.sessionData.Culture.IetfLanguageTag));
                    }
                }

                FinalizeAction(IApp.VALUE_SUCCESS, scriptInfo);
            }
            else
            {
                FinalizeAction(IApp.VALUE_FAILURE, scriptInfo);
            }
        }

        private void HandleSleep(ProviderAction action, RuntimeScriptInfo scriptInfo)
        {
            long sleepTime = Convert.ToInt64(action.actionParams[IActions.Fields.SleepTime]);
            if(sleepTime < 15)  // 15ms is the shortest sleep which would be noticable
            {
                Thread.Sleep(0);   // This will be auto-adjusted to the length of the OS scheduler interval
                FinalizeAction(IApp.VALUE_SUCCESS, scriptInfo);
            }
            else
            {
                lock(scriptInfo.StateLock)
                {
                    scriptInfo.State = RuntimeScriptInfo.RunState.Sleeping;
                    scripts.SetInactive(scriptInfo.routingGuid);
                    timerManager.Add(sleepTime, scriptInfo);
                }
            }
        }

        private long OnSleepTimerFire(TimerHandle timer, object scriptInfoObj)
        { 
            RuntimeScriptInfo scriptInfo  = scriptInfoObj as RuntimeScriptInfo;
            if(scriptInfo == null)
            {
                gLog.Write(TraceLevel.Error, "Internal Error: Sleep timer fired with no runtime metadata");
            }
            else
            {
                FinalizeAction(IApp.VALUE_SUCCESS, scriptInfo);
                scripts.SetActive(scriptInfo.routingGuid);
            }

            return 0;  // Don't reschedule (not periodic)
        }

        #endregion

		#region Debug Subsystem

        public bool Debug_SetBreakpoint(string scriptName, string actionId, out string failReason)
		{
			Utils.Assertion.Check(scriptName != null, "ScriptName is null in SetBreakpoint");
			Utils.Assertion.Check((actionId != null) && (actionId != String.Empty), "No action ID specified in Scheduler.Debug_SetBreakpoint");

			failReason = String.Empty;

			RuntimeScriptInfo scriptInfo = GetDebugInstance(scriptName);
			if(scriptInfo == null) 
			{
				failReason = String.Format("No instances of '{0}' are currently being debugged", scriptName);
				gLog.Write(TraceLevel.Warning, failReason);
				return false; 
			}

			// Is action ID valid?
            // This is a best-effort attempt at validation since the script is not present 
            //   until after execution has begun.
            if(scriptInfo.Running == true)
            {
                bool found = false;
                string currElementId = null;
                IDictionaryEnumerator func_enum = scriptInfo.script.functions.GetEnumerator();
                while(func_enum.MoveNext())
                {
                    Function function = func_enum.Value as Function;
                    if(function == null)
                    {
                        gLog.Write(TraceLevel.Error, "Internal Error: Non-Function object found in script's function table on SetBreakpoint: {0}",
                            scriptInfo.ScriptName);
                        return false;
                    }

                    if(found == true) { break; }

                    IDictionaryEnumerator element_enum = function.elements.GetEnumerator();
                    while(element_enum.MoveNext())
                    {
                        currElementId = element_enum.Key as string;
                        Utils.Assertion.Check(currElementId != null, "Function element has no ID");

                        if(currElementId == actionId)
                        {
                            if(element_enum.Value is ActionBase)
                            {
                                found = true;
                                break;
                            }
                            else
                            {
                                failReason = String.Format("A breakpoint cannot be set on a loop in script '{0}' ({1})", scriptName, actionId);
                                gLog.Write(TraceLevel.Warning, failReason);
                                return false; 
                            }
                        }
                    }
                }

                if(found == false)
                {
                    failReason = String.Format("Action ID '{0}' does not exist in script '{1}'", actionId, scriptName);
                    gLog.Write(TraceLevel.Warning, failReason);
                    return false; 
                }
            }

			if(scriptInfo.breakpointActionIds.Contains(actionId) == false)
			{
				scriptInfo.breakpointActionIds.Add(actionId);
			}

			return true;
		}

        public bool Debug_ClearBreakpoint(string scriptName, string actionId, out string failReason)
        {
            Utils.Assertion.Check(scriptName != null, "ScriptName is null in SetBreakpoint");
            Utils.Assertion.Check((actionId != null) && (actionId != String.Empty), "No action ID specified in Scheduler.Debug_SetBreakpoint");

            failReason = String.Empty;

            RuntimeScriptInfo scriptInfo = GetDebugInstance(scriptName);
            if(scriptInfo == null) 
            {
                failReason = String.Format("No instances of '{0}' are currently being debugged", scriptName);
                gLog.Write(TraceLevel.Warning, failReason);
                return false; 
            }

            scriptInfo.breakpointActionIds.Remove(actionId);
            return true;
        }

        public bool Debug_Break(string scriptName, out string failReason)
        {
            Utils.Assertion.Check(scriptName != null, "ScriptName is null in Debug_Break");

            failReason = String.Empty;

            RuntimeScriptInfo scriptInfo = GetDebugInstance(scriptName);
            if(scriptInfo == null) 
            {
                failReason = String.Format("No instances of '{0}' are currently being debugged", scriptName);
                gLog.Write(TraceLevel.Warning, failReason);
                return false; 
            }

            if(scriptInfo.Running == false)
            {
                failReason = String.Format("No debug instances of '{0}' are currently running", scriptName);
                gLog.Write(TraceLevel.Warning, failReason);
                return false;
            }
            
            scriptInfo.DebugBreak = true;

            Utils.Assertion.Check(detailedRespHandler != null, "Detailed response handler is null sending Break response");
            detailedRespHandler(scriptInfo, scriptInfo.currFunction.CurrentElementId);
            return true;
        }

        public bool Debug_ExecuteAction(string scriptName, string actionId, bool stepInto, CommandMessage cMsg, out string failReason)
		{
			Utils.Assertion.Check(scriptName != null, "ScriptName is null in Debug_ExecuteAction");
			Utils.Assertion.Check(actionId != null, "Action ID is null on Debug_ExecuteAction");

			failReason = String.Empty;

			RuntimeScriptInfo scriptInfo = GetDebugInstance(scriptName);
			if(scriptInfo == null) 
			{
				failReason = String.Format("No instances of '{0}' are currently being debugged", scriptName);
				gLog.Write(TraceLevel.Warning, failReason);
				return false; 
			}

            if(scriptInfo.Running == false)
            {
                failReason = String.Format("No debug instances of '{0}' are currently running", scriptName);
                gLog.Write(TraceLevel.Warning, failReason);
                return false;
            }

			// Are they manipulating the instruction pointer?
			if(scriptInfo.currFunction.CurrentElementId != actionId)
			{
				gLog.Write(TraceLevel.Info, "Debug client has modified the instruction pointer in '{0}'", scriptName);
				scriptInfo.currFunction.CurrentElementId = actionId;
			}

            lock(scriptInfo.StateLock)
            {
                scriptInfo.DebugStep = stepInto ? RuntimeScriptInfo.StepType.Into : RuntimeScriptInfo.StepType.Over;
                scriptInfo.debugCommandMsg = cMsg;
                scriptInfo.State = RuntimeScriptInfo.RunState.Idle;
            }

			return true;
		}

        public bool Debug_Run(string scriptName, string actionId, CommandMessage cMsg, out string failReason)
        {
            Utils.Assertion.Check(scriptName != null, "ScriptName is null in Debug_Run");
            Utils.Assertion.Check(actionId != null, "Action ID is null on Debug_Run");

            failReason = String.Empty;

            RuntimeScriptInfo scriptInfo = GetDebugInstance(scriptName);
            if(scriptInfo == null) 
            {
                failReason = String.Format("No instances of '{0}' are currently being debugged", scriptName);
                gLog.Write(TraceLevel.Warning, failReason);
                return false; 
            }

            if(scriptInfo.Running == false)
            {
                failReason = String.Format("No debug instances of '{0}' are currently running", scriptName);
                gLog.Write(TraceLevel.Warning, failReason);
                return false;
            }

            // Are they manipulating the instruction pointer?
            if(scriptInfo.currFunction.CurrentElementId != actionId)
            {
                gLog.Write(TraceLevel.Info, "Debug client has modified the instruction pointer in '{0}'", scriptName);
                scriptInfo.currFunction.CurrentElementId = actionId;
            }

            lock(scriptInfo.StateLock)
            {
                scriptInfo.DebugStep = RuntimeScriptInfo.StepType.None;
                scriptInfo.debugCommandMsg = cMsg;
                scriptInfo.State = RuntimeScriptInfo.RunState.Idle;
            }

            return true;
        }

        public void Debug_Start(string scriptName)
        {
            Utils.Assertion.Check(scriptName != null, "ScriptName is null in Debug_Start");

            if(debugScriptTable.Contains(scriptName) == false)
            {
                debugScriptTable[scriptName] = new RuntimeScriptInfo(new LogWriter(this.gLog.LogLevel, scriptName));
            }
        }

        public bool Debug_Stop(string scriptName, out string failReason)
		{
			Utils.Assertion.Check(scriptName != null, "ScriptName is null in Debug_Stop");
			
			failReason = String.Empty;

			RuntimeScriptInfo scriptInfo = GetDebugInstance(scriptName);
			if(scriptInfo == null) 
			{
                // Let it slide
				return true; 
			}

            lock(scriptInfo.StateLock)
            {
                scriptInfo.Debugging = false;

                if(scriptInfo.Running == false)
                {
                    this.debugScriptTable.Remove(scriptName);
                }
                else if(scriptInfo.State == RuntimeScriptInfo.RunState.DebugBreak)
                {
                    // Change app state so it runs to completion
                    scriptInfo.State = RuntimeScriptInfo.RunState.Idle;
                }
            }

			return true;
		}

        public bool Debug_UpdateValue(string scriptName, string varName, object varValue, out string failReason)
        {
            Utils.Assertion.Check(scriptName != null, "ScriptName is null in Debug_UpdateValue");
			
            failReason = String.Empty;

            RuntimeScriptInfo scriptInfo = GetDebugInstance(scriptName);
            if(scriptInfo == null) 
            {
                failReason = String.Format("No instances of '{0}' are currently being debugged", scriptName);
                gLog.Write(TraceLevel.Warning, failReason);
                return false; 
            }

            if(scriptInfo.Running == false)
            {
                failReason = String.Format("No debug instances of '{0}' are currently running", scriptName);
                gLog.Write(TraceLevel.Warning, failReason);
                return false;
            }

            return AssignVariableValue(scriptInfo, varName, varValue);
        }

        public StringCollection Debug_GetBreakpoints(string scriptName, out string failReason)
        {
            Utils.Assertion.Check(scriptName != null, "ScriptName is null in Debug_GetBreakpoints");

            failReason = String.Empty;

            RuntimeScriptInfo scriptInfo = GetDebugInstance(scriptName);
            if(scriptInfo == null) 
            {
                failReason = String.Format("No instances of '{0}' are currently being debugged", scriptName);
                gLog.Write(TraceLevel.Warning, failReason);
                return null; 
            }

            return scriptInfo.breakpointActionIds;
        }

		private RuntimeScriptInfo GetDebugInstance(string scriptName)
		{
			Utils.Assertion.Check(scriptName != null, "ScriptName is null in GetDebugInstance");

            foreach (RuntimeScriptInfo scriptInfo in scripts)
            {
                if (scriptInfo.ScriptName == scriptName && scriptInfo.Debugging)
                    return scriptInfo;
            }

            // Get the runtime info for the expected debug instance
			return (RuntimeScriptInfo) debugScriptTable[scriptName];
		}

		#endregion

        #region Helper Functions

        #region Variable Initialization

        private bool InitializeScriptVariables(ScriptData script, string partitionName, 
            SessionData sessionData, LogWriter scriptLog)
		{
//			long t0 = HPTimer.Now();

			IDictionary cEntries = configUtility.GetEntries(
                IConfig.ComponentType.Application, AppEnvironment.AppMetaData.Name, partitionName);

//			scriptLog.Write(TraceLevel.Warning, "init script vars {0} checkpoint 1: {1}",
//				script.name, HPTimer.NsSince( t0 ));

//			long t1 = HPTimer.Now();
			foreach(DictionaryEntry de in script.variables)
			{
				string varName = (string) de.Key;
				Variable var = (Variable) de.Value;

				//scriptLog.Write(TraceLevel.Verbose, "initing script variable {0}", varName);

				Utils.Assertion.Check(varName != null, "Variable name is null");
				Utils.Assertion.Check(var != null, "Variable is null");

//				scriptLog.Write(TraceLevel.Warning, "init script vars {0} checkpoint 2: {1}",
//					script.name, HPTimer.NsSince( t0 ));

				object Value = null;

				if(var.InitWith != null)
				{
                    if(var.InitWithType == Variable.InitWithTypes.Locale)
                        Value = GetValueFromStringTable(var, sessionData.Culture.IetfLanguageTag);
                    else
                        Value = GetValueFromConfigList(var, cEntries);
				}
				
                if(Value == null)
                {
                    // If there is no configured value, use default
                    // If there is also no default, just leave the variable in its initial state
                    if(var.DefaultValue != null)
                        Value = var.DefaultValue;
                }

                if(Value != null)
                {
                    try
                    {
                        if(var.Assign(Value) == false)
                        {
                            scriptLog.Write(TraceLevel.Error, "Could not initialize script-level variable {0}={1} in script (2)",
                                varName, Value.ToString(), script.name);
                            return false;
                        }
                    }
                    catch(Exception e)
                    {
                        scriptLog.Write(TraceLevel.Error, "Could not initialize script-level variable {0}={1} in script (2): {3}",
                            varName, Value.ToString(), script.name, e.Message);
                        return false;
                    }
                }
			}

			return true;
		}

        private bool InitializeFunctionParamVariables(string functionName, RuntimeScriptInfo scriptInfo, 
            Function function, EventMessage eventMsg)
        {
            Hashtable incomingParams = CollectionsUtil.CreateCaseInsensitiveHashtable();

            ArrayList fields = eventMsg.Fields;
            foreach(Field field in fields)
            {
                incomingParams[field.Name] = field.Value;
            }

            incomingParams[ICommands.Fields.ROUTING_GUID] = eventMsg.RoutingGuid;
            incomingParams[ICommands.Fields.SESSION_GUID] = eventMsg.SessionGuid;
            incomingParams[ICommands.Fields.USER_DATA]    = eventMsg.UserData;

            return InitializeFunctionParamVariables(functionName, scriptInfo, function, incomingParams);
        }

        private bool InitializeFunctionParamVariables(string functionName, RuntimeScriptInfo scriptInfo, 
            Function function, ActionBase action)
        {
            Hashtable incomingParams = CollectionsUtil.CreateCaseInsensitiveHashtable();

            foreach(ActionParamBase aParam in action.actionParams)
            {
                incomingParams[aParam.name] = aParam.Value;
            }

            return InitializeFunctionParamVariables(functionName, scriptInfo, function, incomingParams);
        }

        /// <summary>This function is only called from the overloads.</summary>
        private bool InitializeFunctionParamVariables(string functionName, RuntimeScriptInfo scriptInfo, 
            Function function, Hashtable incomingParams)
        {
            Utils.Assertion.Check(function != null, "Function is null");
            Utils.Assertion.Check(function.variables != null, "Function variable collection should not be null");

            if(function.variables.Count == 0)
                return true;

            function.variables.Reset(scriptInfo.log);

            foreach(DictionaryEntry de in function.variables)
            {
                string varName = de.Key as String;
                Variable var = de.Value as Variable;

                Utils.Assertion.Check(varName != null, "Variable name is null");
                Utils.Assertion.Check(var != null, "Variable is null");

                object paramValue = null;

                if(var.InitWith != null)
                {
                    paramValue = incomingParams[var.InitWith];
                    if(paramValue != null)
                    {
                        if(var.InitType == Variable.InitTypes.Value)
                        {
                            if(paramValue is string)
                            {
                                //paramValue = paramValue as string; nothing need be done!
                            }
                            else if(paramValue is ICloneable)
                            {
                                paramValue = ((ICloneable) paramValue).Clone();
                            }
                            else
                            {
                                scriptInfo.log.Write(TraceLevel.Warning, "The incoming value for function parameter '{0}' in function '{1}' does not implement ICloneable. Therefore, it cannot be passed by value, as specified. Passing by reference.",
                                    varName, functionName);
                            }
                        }
                    }
                }

                if(paramValue == null)
                {
                    if(var.DefaultValue != null)
                    {
                        paramValue = var.DefaultValue;
                    }
                    else if(var.InitWith != null)
                    {
                        scriptInfo.log.Write(TraceLevel.Error, "Required function parameter {0}({1}) not found",
                            functionName, var.InitWith);
                        return false;
                    }
                    else
                    {
                        continue;
                    }
                }

                try
                {
					if(var.Assign(paramValue) == false)
					{
						scriptInfo.log.Write(TraceLevel.Error, "Could not assign function parameter {0}({1})={2}",
                            functionName, var.InitWith, paramValue.ToString());
						return false;
					}
                }
                catch(Exception e)
                {
                    scriptInfo.log.Write(TraceLevel.Error, "Could not assign function parameter {0}({1})={2}: {3}",
                        functionName, var.InitWith, paramValue.ToString(), e.Message);
                    return false;
                }
            }

            return true;
        }

        #region InitWith resolution

        /// <summary>Gets value to initialize a variable with from the config list provided</summary>
        /// <param name="var">Variable to be initialized</param>
        /// <param name="cEntries">All config entry-value pairs for this application</param>
        /// <returns>Config value</returns>
        /// <remarks>Variable.InitWith must be set prior to calling this method</remarks>
        private object GetValueFromConfigList(Variable var, IDictionary cEntries)
        {
            if (var.InitWith == null)
                return null;

            Metreos.Core.ConfigData.ConfigEntry cEntry =
						(Metreos.Core.ConfigData.ConfigEntry) cEntries[var.InitWith];

            if(cEntry == null)
                return null;

            if ((cEntry.Value == null) && 
                (var.DefaultValue == null) && 
                (cEntry.formatType.Custom == false))
            {
                // They have a config entry, but no value. AND
                // They have no default specified in the app. AND
                // It's a standard collection. Let's be nice and initialize it for them.
                // How could this happen? Consider an optional dial plan hashtable.
                IConfig.StandardFormat configFormat = 
                            Metreos.Core.ConfigData.FormatType.ParseToStandardFormat(cEntry.formatType.name);
                switch(configFormat)
                {
                    case IConfig.StandardFormat.Array:
                        return new ArrayList();
                    case IConfig.StandardFormat.DataTable:
                        return new System.Data.DataTable();
                    case IConfig.StandardFormat.HashTable:
                        return new Hashtable();
                }
            }

            return cEntry.Value;
        }

        /// <summary>Initializes a variable from the locale string table</summary>
        /// <param name="var">Variable to be initialized</param>
        /// <returns>localized string</returns>
        /// <remarks>Variable.InitWith must be set prior to calling this method</remarks>
        private string GetValueFromStringTable(Variable var, string locale)
        {
            if (var.InitWith == null ||
                var.InitWithType != Variable.InitWithTypes.Locale ||
                AppEnvironment.AppMetaData.StringTable == null)
                return null;

            return AppEnvironment.AppMetaData.StringTable.GetTableValue(var.InitWith, locale);
        }

        #endregion

        #region Get/Set variable values

        private object GetVariableValue(RuntimeScriptInfo scriptInfo, string variableName)
        {
            // Search for function-scope variable
            Variable var = scriptInfo.currFunction.function.variables[variableName];

            if(var == null)
            {
                // Search for app-scope variable
                var = scriptInfo.script.variables[variableName];

                if(var == null)
                {
                    return null;
                }
            }

            return var.Value;
        }

        private bool AssignVariableValue(RuntimeScriptInfo scriptInfo, string variableName, object Value)
        {
            // Search for function-scope variable
            Variable var = scriptInfo.currFunction.function.variables[variableName];

            if(var == null)
            {
                // Search for app-scope variable
                var = scriptInfo.script.variables[variableName];

                if(var == null)
                    return false;
            }

            try
            {
                return var.Assign(Value);
            }
            catch(Exception e)
            {
                scriptInfo.log.Write(TraceLevel.Error, "Could not assign {0}={1}: {2}",
                    variableName, Value == null ? "null" : Value.ToString(), e.Message);
                return false;
            }
        }
        #endregion
        #endregion

        #region Action parameter value assignment

        private bool EvaluateCurrentActionParameters(RuntimeScriptInfo scriptInfo)
        {
            ActionBase action = scriptInfo.currFunction.CurrentElement as ActionBase;
            Utils.Assertion.Check(action != null, "Current action is invalid.");

            foreach(ActionParamBase aParamBase in action.actionParams)
            {
                LiteralActionParam lParam = aParamBase as LiteralActionParam;
                if(lParam != null)
                {
                    lParam.Value = lParam.literalValue;
                    continue;
                }

                VariableActionParam vParam = aParamBase as VariableActionParam;
                if(vParam != null)
                {
                    vParam.Value = GetVariableValue(scriptInfo, vParam.variableName);
                    continue;
                }

                UserCodeActionParam uParam = aParamBase as UserCodeActionParam;
                if(uParam != null)
                {
                    // object Execute(VariableCollection functionVariables, VariableCollection scriptVariables, 
                    //                SessionData sessionData, int loopIndex, IEnumerator loopEnum, 
                    //                IDictionaryEnumerator loopDictEnum)
                    object[] parameters = new object[6];
                    parameters[0] = scriptInfo.currFunction.function.variables;
                    parameters[1] = scriptInfo.script.variables;
                    parameters[2] = scriptInfo.sessionData;

                    if(scriptInfo.currFunction.InLoop)
                    {
                        parameters[3] = scriptInfo.currFunction.CurrentLoop.loopIndex;
                        parameters[4] = scriptInfo.currFunction.CurrentLoop.loopEnum;
                        parameters[5] = scriptInfo.currFunction.CurrentLoop.loopDictEnum;
                    }
                    else
                    {
                        parameters[3] = 0;
                        parameters[4] = null;
                        parameters[5] = null;
                    }

                    try
                    {
                        uParam.Value = uParam.userCode.Invoke(null, parameters);
                    }
                    catch(Exception e)
                    {
                        scriptInfo.log.Write(TraceLevel.Error, "Could not evaluate C# parameter '{0}' in action '{1}': {2}", 
                            uParam.name, scriptInfo.currFunction.CurrentElementId, e.InnerException.Message);
                        return false; 
                    }
                }
            }

            return true;
        }

		private bool AssignNativeActionParams(INativeAction nativeAction, RuntimeScriptInfo scriptInfo)
		{
			ActionBase currAction = scriptInfo.currFunction.CurrentElement as ActionBase;
			Utils.Assertion.Check(currAction != null, "Current action is invalid.");

			Reflect.PropertyInfo[] props = nativeAction.GetType().GetProperties();

            // Make a copy of the parameters, so the remainder can be passed as custom params
            AppCollections.ActionParamCollection customParams = new AppCollections.ActionParamCollection();
            foreach(ActionParamBase aParamBase in currAction.actionParams)
            {
                customParams.Add(aParamBase);
            }
            Reflect.PropertyInfo customPropInfo = null;

			foreach(Reflect.PropertyInfo pInfo in props)
			{
				if(pInfo.CanWrite == false) { continue; }

                if(pInfo.PropertyType == typeof(AppCollections.ActionParamCollection))
                {
                    if(customPropInfo != null)
                    {
                        scriptInfo.log.Write(TraceLevel.Error, "Native action has more than one property of type ActionParamCollection: {0}",
                            scriptInfo.currFunction.CurrentElementId);
                        return false;
                    }

                    customPropInfo = pInfo;
                    continue;
                }

				object[] attrs = pInfo.GetCustomAttributes(false);
				if(attrs == null) { continue; }
				if(attrs.Length != 1) { continue; }

				ActionParamFieldAttribute paramAttr = attrs[0] as ActionParamFieldAttribute;
				if(paramAttr == null) { continue; }

                if(!currAction.actionParams.Contains(pInfo.Name) && paramAttr.mandatory)
                {
                    scriptInfo.log.Write(TraceLevel.Error, "Mandatory parameter '{0}' missing in: {1}",
                        pInfo.Name, scriptInfo.currFunction.CurrentElementId);
                    return false;
                }

                customParams.Remove(pInfo.Name);

                if(currAction.actionParams.Contains(pInfo.Name))
                {
                    object Value = currAction.actionParams[pInfo.Name];
                    try
                    {
                        Value = ParseTo(pInfo.PropertyType, Value);
                        pInfo.SetValue(nativeAction, Value, null);
                    }
                    catch(Exception e)
                    {
                        scriptInfo.log.Write(TraceLevel.Error, "Could not assign {0}={1} in {2}. Reason: {3}",
                            pInfo.Name, Value != null ? Value.ToString() : "NULL", 
                            scriptInfo.currFunction.CurrentElementId, e.Message);
                        return false;
                    }
                }
			}

            if(customPropInfo != null)
            {
                try { customPropInfo.SetValue(nativeAction, customParams, null); }
                catch(Exception e)
                {
                    scriptInfo.log.Write(TraceLevel.Error, "Could not assign {0}={1} in {2}. Reason: {3}",
                        customPropInfo.Name, "[custom parameters]", 
                        scriptInfo.currFunction.CurrentElementId, e.Message);
                    return false;
                }
            }

			return true;
        }
        #endregion

        #region Loops

        private RuntimeLoopInfo CreateRuntimeLoopInfo(RuntimeScriptInfo scriptInfo, Loop loop)
        {
            Utils.Assertion.Check(loop != null, "Loop is null");

            RuntimeLoopInfo loopInfo = new RuntimeLoopInfo();
            loopInfo.id = scriptInfo.currFunction.CurrentElementId;
            loopInfo.loop = loop;
                    
            switch(loop.loopCount.enumType)
            {
                case LoopCountBase.EnumerationType.Int:
                    loopInfo.loopIndex = 0;

                    LiteralLoopCount litLoopCount = loop.loopCount as LiteralLoopCount;
                    if(litLoopCount != null)
                    {
                        loopInfo.loopLimit = litLoopCount.limit;
                    }

                    VariableLoopCount vLoopCount = loop.loopCount as VariableLoopCount;
                    if(vLoopCount != null)
                    {
                        object loopLimitObj = GetVariableValue(scriptInfo, vLoopCount.variableName);
                        if(loopLimitObj == null) 
                        {
                            scriptInfo.log.Write(TraceLevel.Error, "Could not resolve loop count variable '{0}' in loop '{1}'",
                                vLoopCount.variableName, scriptInfo.currFunction.CurrentElementId);
                            return null; 
                        }

                        try
                        {
                            loopInfo.loopLimit = int.Parse(loopLimitObj.ToString());
                        } 
                        catch 
                        {
                            scriptInfo.log.Write(TraceLevel.Error, "Loop count variable {0}({1}) in loop '{2}' is not an integer value",
                                vLoopCount.variableName, loopLimitObj.ToString(), scriptInfo.currFunction.CurrentElementId);
                            return null; 
                        }
                    }

                    UserCodeLoopCount uLoopCount = loop.loopCount as UserCodeLoopCount;
                    if(uLoopCount != null)
                    {
                        object userValue = GetUserCodeLoopCountValue(scriptInfo, uLoopCount);
                        if(userValue == null)
                        {
                            scriptInfo.log.Write(TraceLevel.Error, "C# loop count in loop '{0}' returned null", 
                                scriptInfo.currFunction.CurrentElementId);
                            return null; 
                        }

                        try
                        {
                            loopInfo.loopLimit = int.Parse(userValue.ToString());
                        }
                        catch
                        {
                            scriptInfo.log.Write(TraceLevel.Error, "C# loop count ({0}) in loop '{1}' cannot be resolved to an integer value", 
                                userValue.ToString(), scriptInfo.currFunction.CurrentElementId);
                            return null;
                        }
                    }
                    return loopInfo;

                case LoopCountBase.EnumerationType.Enum:
                    if(loop.loopCount is LiteralLoopCount)
                    {
                        scriptInfo.log.Write(TraceLevel.Error, "Integer loop counts are not enumerable (loop={0})",
                            scriptInfo.currFunction.CurrentElementId);
                        return null;
                    }

                    vLoopCount = loop.loopCount as VariableLoopCount;
                    if(vLoopCount != null)
                    {
                        object loopEnumObj = GetVariableValue(scriptInfo, vLoopCount.variableName);
                        if(loopEnumObj == null) 
                        {
                            scriptInfo.log.Write(TraceLevel.Error, "Could not resolve loop count variable '{0}' in loop '{1}'",
                                vLoopCount.variableName, scriptInfo.currFunction.CurrentElementId);
                            return null; 
                        }

                        if(loopEnumObj is IEnumerable)
                        {
                            loopInfo.loopEnum = ((IEnumerable)loopEnumObj).GetEnumerator();
                        }
                        else
                        {
                            scriptInfo.log.Write(TraceLevel.Error, "Variable '{0}' (type={1}) specified as the count in loop '{2}' does not implement System.IEnumerable",
                                vLoopCount.variableName, loopEnumObj.GetType().ToString(), scriptInfo.currFunction.CurrentElementId);
                            return null;
                        }
                    }

                    uLoopCount = loop.loopCount as UserCodeLoopCount;
                    if(uLoopCount != null)
                    {
                        object userValue = GetUserCodeLoopCountValue(scriptInfo, uLoopCount);
                        if(userValue == null)
                        {
                            scriptInfo.log.Write(TraceLevel.Error, "C# loop count in loop '{0}' returned null", 
                                scriptInfo.currFunction.CurrentElementId);
                            return null; 
                        }

                        if(userValue is IEnumerable)
                        {
                            loopInfo.loopEnum = ((IEnumerable)userValue).GetEnumerator();
                        }
                        else
                        {
                            scriptInfo.log.Write(TraceLevel.Error, "User code result type '{0}' specified as the count in loop '{1}' does not implement System.IEnumerable",
                                userValue.GetType().ToString(), scriptInfo.currFunction.CurrentElementId);
                            return null;
                        }
                    }

                    loopInfo.loopEnum.MoveNext();
                    return loopInfo;

                case LoopCountBase.EnumerationType.DictEnum:
                    if(loop.loopCount is LiteralLoopCount)
                    {
                        scriptInfo.log.Write(TraceLevel.Error, "Integer loop counts are not enumerable (loop={0})",
                            scriptInfo.currFunction.CurrentElementId);
                        return null;
                    }

                    vLoopCount = loop.loopCount as VariableLoopCount;
                    if(vLoopCount != null)
                    {
                        object loopEnumObj = GetVariableValue(scriptInfo, vLoopCount.variableName);
                        if(loopEnumObj == null) 
                        {
                            scriptInfo.log.Write(TraceLevel.Error, "Could not resolve loop count variable '{0}' in loop '{1}'",
                                vLoopCount.variableName, scriptInfo.currFunction.CurrentElementId);
                            return null; 
                        }

                        if(loopEnumObj is IEnumerable)
                        {
                            IEnumerator enumerator = ((IEnumerable)loopEnumObj).GetEnumerator();
                            if(enumerator is IDictionaryEnumerator)
                            {
                                loopInfo.loopDictEnum = enumerator as IDictionaryEnumerator;
                            }
                            else
                            {
                                scriptInfo.log.Write(TraceLevel.Error, "Variable '{0}' (type={1}) specified as the count in loop '{2}' implements System.IEnumerable but does not return an IDictionaryEnumerator",
                                    vLoopCount.variableName, loopEnumObj.GetType().ToString(), scriptInfo.currFunction.CurrentElementId);
                                return null;
                            }
                        }
                        else
                        {
                            scriptInfo.log.Write(TraceLevel.Error, "Variable '{0}' (type={1}) specified as the count in loop '{2}' does not implement System.IEnumerable",
                                vLoopCount.variableName, loopEnumObj.GetType().ToString(), scriptInfo.currFunction.CurrentElementId);
                            return null;
                        }
                    }

                    uLoopCount = loop.loopCount as UserCodeLoopCount;
                    if(uLoopCount != null)
                    {
                        object userValue = GetUserCodeLoopCountValue(scriptInfo, uLoopCount);
                        if(userValue == null)
                        {
                            scriptInfo.log.Write(TraceLevel.Error, "C# loop count in loop '{0}' returned null", 
                                scriptInfo.currFunction.CurrentElementId);
                            return null; 
                        }

                        if(userValue is IEnumerable)
                        {
                            IEnumerator enumerator = ((IEnumerable)userValue).GetEnumerator();
                            if(enumerator is IDictionaryEnumerator)
                            {
                                loopInfo.loopDictEnum = enumerator as IDictionaryEnumerator;
                            }
                            else
                            {
                                scriptInfo.log.Write(TraceLevel.Error, "User code result type '{0}' specified as the count in loop '{2}' implements System.IEnumerable but does not return an IDictionaryEnumerator",
                                    userValue.GetType().ToString(), scriptInfo.currFunction.CurrentElementId);
                                return null;
                            }
                        }
                        else
                        {
                            scriptInfo.log.Write(TraceLevel.Error, "User code result type '{0}' specified as the count in loop '{1}' does not implement System.IEnumerable",
                                userValue.GetType().ToString(), scriptInfo.currFunction.CurrentElementId);
                            return null;
                        }
                    }

                    loopInfo.loopDictEnum.MoveNext();
                    return loopInfo;
            }

            return null;
        }

        private object GetUserCodeLoopCountValue(RuntimeScriptInfo scriptInfo, UserCodeLoopCount uLoopCount)
        {
            // object Execute(VariableCollection functionVariables, VariableCollection scriptVariables, 
            //                SessionData sessionData);
            object[] parameters = new object[3];
            parameters[0] = scriptInfo.currFunction.function.variables;
            parameters[1] = scriptInfo.script.variables;
            parameters[2] = scriptInfo.sessionData;

            object userValue = null;
            try
            {
                userValue = uLoopCount.userCode.Invoke(null, parameters);
            }
            catch(Exception e)
            {
                scriptInfo.log.Write(TraceLevel.Error, "Could not evaluate C# loop count in loop '{0}' : {1}", 
                    scriptInfo.currFunction.CurrentElementId, e.InnerException.Message);
                return null; 
            }

            return userValue;
        }
        #endregion

        #region The uncategorizable misfits

        private string GetNextActionId(RuntimeScriptInfo scriptInfo, string returnValue, out string error)
        {
            error = null;
            string nextActionId = null;
            scriptInfo.currFunction.LastReturnValue = returnValue;

            ActionBase oldAction = scriptInfo.currFunction.CurrentElement as ActionBase;

            if(oldAction != null)
            {
                nextActionId = oldAction.nextActions[returnValue];

                if(nextActionId == null)
                {
                    nextActionId = oldAction.nextActions[IApp.VALUE_DEFAULT];

                    if(nextActionId == null)
                    {
                        error = String.Format("Could not locate next action (script={0}, action={1}, response={2})",
                            scriptInfo.ScriptName, scriptInfo.currFunction.CurrentElementId, returnValue);
                        return null;
                    }
                }
            }
            else
            {
                Loop loop = scriptInfo.currFunction.CurrentElement as Loop;

                Utils.Assertion.Check(loop != null, "Encountered unknown element");

                nextActionId = loop.nextActions[returnValue];

                if(nextActionId == null)
                {
                    nextActionId = loop.nextActions[IApp.VALUE_DEFAULT];

                    if(nextActionId == null)
                    {
                        error = String.Format("Could not locate next action (script={0}, loop={1}, response={2})",
                            scriptInfo.ScriptName, scriptInfo.currFunction.CurrentElementId, returnValue);
                        return null;
                    }
                }
            }

            return nextActionId;
        }

        private object ParseTo(Type type, object Value)
        {
            if(Value == null) { return null; }
            if(type == Value.GetType()) { return Value; }
            if(type.IsAssignableFrom(Value.GetType())) { return Value; }

            object newVal = null;
            if(type.IsEnum)
            {
                try
                {
                    newVal = Enum.Parse(type, Value.ToString(), true);
                }
                catch { }
            }
            else if(type == typeof(string))
            {
                newVal = Value.ToString();
            }
            else
            {
                try
                {
                    newVal = Convert.ChangeType(Value, type);
                }
                catch { }

                if(newVal == null)
                {
                    try
                    {
                        newVal = type.InvokeMember("Parse", Reflect.BindingFlags.InvokeMethod, null, null,
                            new object[] { Value });
                    }
                    catch { }
                }
            }

            return newVal != null ? newVal : Value;
        }

        private Hashtable GetNativeActionResultData(INativeAction action)
        {
            Hashtable hash = new Hashtable();

            Reflect.PropertyInfo[] props = action.GetType().GetProperties();

            foreach(Reflect.PropertyInfo pInfo in props)
            {
                if(pInfo.CanRead)
                {
                    hash[pInfo.Name] = pInfo.GetValue(action, null);
                }
            }

            return hash;
        }
        #endregion
        #endregion

        #region Diagnostic Output

        public int DiagNumRunningInstances(string scriptName)
        {
            int i=0;

            foreach(RuntimeScriptInfo scriptInfo in scripts)
            {
                if(scriptInfo.ScriptName == scriptName)
                    i++;
            }

            return i;
        }
        #endregion
	}
}
