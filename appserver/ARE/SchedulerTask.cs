using System;
using System.IO;
using System.Data;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Serialization;

using Metreos.Core;
using Metreos.Utilities;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Core.ConfigData;
using Metreos.AppArchiveCore.Xml;
using Metreos.ApplicationFramework;
using Metreos.ApplicationFramework.ActionParameters;
using Metreos.ApplicationFramework.Collections;
using Metreos.ApplicationFramework.Actions;

using Metreos.Configuration;
using Metreos.AppServer.ARE.Collections;

namespace Metreos.AppServer.ARE
{
	public sealed class SchedulerTask : PrimaryTaskBase
	{
        public readonly ManualResetEvent startupComplete;
        public readonly ManualResetEvent shutdownComplete;

        private MessageQueueWriter routerQ = null;
		private MessageQueueWriter appManQ = null;
        private MessageQueueWriter telManQ = null;

        private readonly Scheduler scheduler;
        private readonly Repository repository;
        private readonly ScriptIdFactory scriptIdFactory;

        private readonly SessionDataCollection sessions;

        #region Initialize/Startup/Shutdown/Refresh
		public SchedulerTask(Repository repository, TraceLevel logLevel)
            : base(IConfig.ComponentType.Core,
                IConfig.CoreComponentNames.ARE,
                AppEnvironment.DisplayName,
                logLevel,
                Config.Instance)
		{
            Assertion.Check(repository != null, "Cannot create SchedulerTask with null repository reference");

            this.repository = repository;

            this.scriptIdFactory = new ScriptIdFactory();

            this.scheduler = new Scheduler(log);
            this.scheduler.providerActionHandler = new HandleProviderActionDelegate(this.HandleProviderAction);
            this.scheduler.endSessionHandler = new HandleEndSessionDelegate(this.HandleEndSession);
            this.scheduler.enableApplication = new VoidDelegate(EnableApplication);
            this.scheduler.recycleScriptHandler = new RecycleScriptDelegate(HandleRecycleScript);
            this.scheduler.sendNoHandler = new SendNoHandlerDelegate(SendNoHandler);
            this.scheduler.repostForwardedEvents = new ScriptInfoDelegate(RepostForwardedEvents);
            this.scheduler.sendDestructorEvent = new SendDestructorEventDelegate(SendDestructorEvent);

            // Debugging
			this.scheduler.hitBreakpointHandler = new ScriptInfoDelegate(this.HandleHitBreakpoint);
			this.scheduler.detailedRespHandler = new HandleDebugResponseDelegate(this.SendDetailedResp);
            this.scheduler.stopDebuggingHandler = new HandleStopDebuggingDelegate(this.SendStopDebugging);

            this.sessions = new SessionDataCollection();

            this.startupComplete = new ManualResetEvent(false);
            this.shutdownComplete = new ManualResetEvent(false);
		}

		public void Initialize(MessageQueueWriter routerQ, MessageQueueWriter telManQ)
		{
            Assertion.Check(routerQ != null, "routerQ is null in SchedulerTask.Initialize()");
            Assertion.Check(telManQ != null, "telManQ is null in SchedulerTask.Initialize()");

            this.routerQ = routerQ;
            this.telManQ = telManQ;
		}

        protected override System.Diagnostics.TraceLevel GetLogLevel()
        {
            return (TraceLevel)configUtility.GetEntryValue(IConfig.ComponentType.Core, 
                IConfig.CoreComponentNames.ARE, IConfig.Entries.Names.LOG_LEVEL);
        }

        protected override void RefreshConfiguration(string proxy)
        {
            scheduler.RefreshConfiguration();
        }

        protected override void OnStartup()
        {
            // Deserialize string table XML and 
            //  create list of valid locales for this app
            PopulateLocaleData();

            scheduler.Startup();

            if(repository.ConstructorScriptName == null)
            {
                // Enable the app
                log.Write(TraceLevel.Info, "No constructor found");
                EnableApplication();
            }
            else
            {
                TriggerConstructor();                
            }

            startupComplete.Set();
        }

        private void TriggerConstructor()
        {
            // The event is fabricated here and sent internally 
            //   because the Router's rules would not route the event properly
            //   due to multiple handlers for the same event.
            EventMessage eventMsg = base.CreateEventMessage(IEvents.Contruction, EventMessage.EventType.Triggering, System.Guid.NewGuid().ToString());
            eventMsg.ScriptName = repository.ConstructorScriptName;
            eventMsg.PartitionName = Database.DefaultValues.PARTITION_NAME;

            // Get default locale config
            string locale = null;
            try
            {
                locale = configUtility.GetEntryValue(IConfig.ComponentType.Core,
                    IConfig.CoreComponentNames.APP_MANAGER, IConfig.Entries.Names.DEFAULT_LOCALE) as string;
                eventMsg.Culture = new System.Globalization.CultureInfo(locale);
            }
            catch
            {
                log.Write(TraceLevel.Warning, "Default locale is invalid: " + locale);
                eventMsg.Culture = new System.Globalization.CultureInfo("en-US");
            }

            log.Write(TraceLevel.Info, "Executing constructor...");
            PostMessage(eventMsg);
        }

        /// <summary>Gets list of locales defined in the application</summary>
        /// <remarks>
        /// The list of valid locales is defined by the union of the locales 
        /// in the string table and the media locale directories.
        /// </remarks>
        public void PopulateLocaleData()
        {
            // Store locale data in a static place so all components have easy access
            AppEnvironment.AppMetaData.Locales = new List<string>();

            // Parse locales from string table
            string stXml = configUtility.GetEntryValue(IConfig.ComponentType.Application, AppEnvironment.AppMetaData.Name,
                IConfig.Entries.Names.STRING_TABLE) as string;
            if(stXml != null)
            {
                try
                {
                    System.IO.TextReader stXmlReader = new StringReader(stXml);
                    XmlSerializer serializer = new XmlSerializer(typeof(LocaleTableType));
                    LocaleTableType lt = (LocaleTableType) serializer.Deserialize(stXmlReader);

                    if (lt != null &&
                        lt.Locales != null &&
                        lt.Locales.Locale != null)
                    {
                        for(int i=0; i < lt.Locales.Locale.Length; i++)
                        {
                            Locale l = lt.Locales.Locale[i];

                            if(l.PromptInfos != null && l.PromptInfos.Length > 0)
                            {
                                Hashtable lTable = new Hashtable();

                                for(int x=0; x < l.PromptInfos.Length; x++)
                                {
                                    PromptInfo pi = l.PromptInfos[x];
                                    if(pi != null && pi.Value != null)
                                    {
                                        // string name, localized value
                                        lTable.Add(pi.@ref, pi.Value.Data);
                                    }
                                }

                                // locale name, table of localized strings
                                AppEnvironment.AppMetaData.StringTable.Add(l.name, lTable);
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Warning, "Could not parse string table for '{0}': {1}",
                        AppEnvironment.AppMetaData.Name, e.Message);
                }
            }

            // Build a list of valid locales from the resources defined in the app
            if(AppEnvironment.AppMetaData.StringTable != null)
            {
                AppEnvironment.AppMetaData.Locales.AddRange(AppEnvironment.AppMetaData.StringTable.GetLocales());
            }

            //Get media locale directory names
            DirectoryInfo appDir = Config.ApplicationDir;
            DirectoryInfo[] sDirs = appDir.GetDirectories(AppEnvironment.AppMetaData.Name);
            if(sDirs == null || sDirs.Length != 1)
            {
                log.Write(TraceLevel.Error, "Internal Error: Could not locate application directory for: " + AppEnvironment.AppMetaData.Name);
            }
            else
            {
                sDirs = sDirs[0].GetDirectories(AppEnvironment.AppMetaData.Version);
                if(sDirs == null || sDirs.Length != 1)
                {
                    log.Write(TraceLevel.Error, "Internal Error: Could not locate application directory for: " + AppEnvironment.AppMetaData.Name);
                }
                else
                {
                    sDirs = sDirs[0].GetDirectories(IConfig.AppDirectoryNames.MEDIA_FILES);
                    if(sDirs == null || sDirs.Length != 1)
                    {
                        log.Write(TraceLevel.Error, "Internal Error: Could not locate application media directory for: " + AppEnvironment.AppMetaData.Name);
                    }
                    else
                    {
                        sDirs = sDirs[0].GetDirectories();
                        foreach(DirectoryInfo dir in sDirs)
                        {
                            if(!AppEnvironment.AppMetaData.Locales.Contains(dir.Name))
                                AppEnvironment.AppMetaData.Locales.Add(dir.Name);
                        }
                    }
                }
            }

            // Save the list
            Metreos.Core.ConfigData.ConfigEntry cEntry = new Metreos.Core.ConfigData.ConfigEntry(IConfig.Entries.Names.LOCALE_LIST,
                AppEnvironment.AppMetaData.Locales, null, IConfig.StandardFormat.Array, false);
            configUtility.AddEntry(IConfig.ComponentType.Application, AppEnvironment.AppMetaData.Name, cEntry, true);
        }

        protected override void OnShutdown()
        {
            // If there are other apps still loaded, 
            //   reverse PrimaryTaskBase's status change for this component
            ComponentInfo[] apps = configUtility.GetComponents(IConfig.ComponentType.Application);
            if(apps != null && apps.Length > 1)
            {
                configUtility.UpdateStatus(IConfig.ComponentType.Core, IConfig.CoreComponentNames.ARE, 
                    IConfig.Status.Enabled_Running);
            }

            // Let the Router know we're going away
            CommandMessage msg = messageUtility.CreateCommandMessage(IConfig.CoreComponentNames.ROUTER, 
                ICommands.UNINSTALL_APP);
            msg.AddField(ICommands.Fields.APP_NAME, AppEnvironment.AppMetaData.Name);
            routerQ.PostMessage(msg);

            scheduler.Shutdown();

    		sessions.Clear();   // Closes all session resources

            shutdownComplete.Set();
        }

        public override void Dispose()
        {
            scheduler.Dispose();
            base.Dispose();
        }

        public string GetDiagMessage()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            try
            {
                sb.Append("App Name: ");
                sb.Append(AppEnvironment.AppMetaData.Name);
                sb.Append("\r\nActive Sessions: ");
                sb.Append(this.sessions.Count.ToString());

                if(sessions.Count > 0)
                {
                    sb.Append(" (");
                    foreach(SessionData sData in sessions)
                    {
                        sb.Append(sData.InstanceId.ToString());
                        sb.Append(", ");
                    }
                    sb.Remove(sb.Length-2, 2); // nix the last comma
                    sb.Append(")");
                }

                sb.Append("\r\n\r\n");

                sb.Append("Script Name  Running  Idle\r\n");
                sb.Append("-----------  -------  ----\r\n");

                foreach(string scriptName in this.repository.GetScriptNames())
                {
                    if(scriptName.Length > 11)
                        sb.Append(scriptName.Substring(0, 11));
                    else
                        sb.Append(scriptName.PadRight(11));
                    sb.Append("  ");

                    int numInstances = this.scheduler.DiagNumRunningInstances(scriptName);
                    sb.Append(numInstances.ToString().PadRight(7));
                    sb.Append("  ");

                    int numIdle = this.repository.GetNumIdle(scriptName);
                    sb.Append(numIdle.ToString().PadRight(4));
                    sb.Append("\r\n");
                }
            }
            catch(Exception e)
            {
                sb.Append("Exception caught while generating log message: " + e.Message);
            }

            return sb.ToString();
        }

        #endregion

		#region Message Distributor
        protected override bool HandleMessage(InternalMessage message)
        {
            try
            {
                if(message is EventMessage)
                {
                    EventMessage eventMsg = message as EventMessage;
                    switch(eventMsg.Type)
                    {
                        case EventMessage.EventType.Triggering:
                            HandleTriggeringEvent(eventMsg);
                            return true;
                        case EventMessage.EventType.AsyncCallback:
                        case EventMessage.EventType.NonTriggering:
                            if(!scheduler.HandleNonTriggeringEvent(eventMsg))
                                routerQ.PostMessage(eventMsg);  // Post-back if a 'Forward' is in process
                            return true;
                    }
                }
                else if(message is CommandMessage)
                {
                    CommandMessage cMsg = message as CommandMessage;
                    switch(cMsg.MessageId)
                    {
                        case ICommands.START_DEBUGGING:
                            HandleStartDebugging(cMsg);
                            break;
                        case ICommands.SET_BREAKPOINT:
                            HandleSetBreakpoint(cMsg);
                            break;
                        case ICommands.CLEAR_BREAKPOINT:
                            HandleClearBreakpoint(cMsg);
                            break;
                        case ICommands.BREAK:
                            HandleBreak(cMsg);
                            break;
                        case ICommands.STOP_DEBUGGING:
                            HandleStopDebugging(cMsg);
                            break;
                        case ICommands.EXEC_ACTION:
                            HandleExecuteAction(cMsg);
                            break;
                        case ICommands.RUN:
                            HandleRun(cMsg);
                            break;
                        case ICommands.UPDATE_VALUE:
                            HandleUpdateValue(cMsg);
                            break;
                        case ICommands.GET_BREAKPOINTS:
                            HandleGetBreakpoints(cMsg);
                            break;
                    }
                }
                else if(message.MessageId == IActions.NoHandler)
                {
                    EventMessage unhandledEvent = message[IActions.Fields.InnerMsg] as EventMessage;
                    if (unhandledEvent != null && 
                        unhandledEvent.MessageId == IEvents.Destruction)
                    { 
                        scheduler.TerminateScript(unhandledEvent.RoutingGuid, null);
                    }
                }
                else if(message is ResponseMessage)
                {
                    ResponseMessage respMsg = message as ResponseMessage;

                    if(respMsg.Source != IConfig.CoreComponentNames.APP_MANAGER)
                    {
                        // Send it to the scheduler
                        scheduler.HandleResponse(respMsg);
                    }
                    else
                    {
                        // Debug client response. Do something?
                    }

                    return true;
                }

                return false;
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "SchedulerTask threw an unhandled exception:\n" + e);
                throw e;
            }
        }
		#endregion

        #region Event Handling
        private void HandleTriggeringEvent(EventMessage eventMsg)
        {
            if(eventMsg.RoutingGuid == null)
            {
                log.Write(TraceLevel.Error, "Cannot handle event with no routing GUID specified: " + eventMsg);
                SendNoHandler(eventMsg);
                return;
            }

            if(eventMsg.ScriptName == null)
            {
                log.Write(TraceLevel.Error, "Cannot handle event with no script specified: " + eventMsg);
                SendNoHandler(eventMsg);
                return;
            }

            if(eventMsg.PartitionName == null)
            {
                log.Write(TraceLevel.Error, "Cannot handle event with no partition name specified: " + eventMsg);
                SendNoHandler(eventMsg);
                return;
            }

            if(eventMsg.SessionGuid == null)
                eventMsg.SessionGuid = eventMsg.RoutingGuid;

            ScriptData script = repository.GetScript(eventMsg.ScriptName);

			if(script == null)
            {
                log.Write(TraceLevel.Warning, "System Overloaded: Could not create script instance. Rejecting...");
                SendNoHandler(eventMsg);
                return;
            }

            // Get some session state
            SessionData data = new SessionData(eventMsg.AppName, eventMsg.PartitionName, eventMsg.Culture, scriptIdFactory.GenerateId());

            // Open internal databases
            if(AppEnvironment.AppMetaData.Databases != null)
            {
                foreach(string dbName in AppEnvironment.AppMetaData.Databases)
                {
                    string dsn = Database.FormatDSN(dbName, configUtility.DatabaseHost, configUtility.DatabasePort,
                        configUtility.DatabaseUsername, configUtility.DatabasePassword, true);

                    try
                    {
                        IDbConnection dbConn = Database.CreateConnection(Database.DbType.mysql, dsn);
                        data.DbConnections.Add(dbName, dbConn);
                    }
                    catch(Exception e)
                    {
                        log.Write(TraceLevel.Error, "Could not create database '{0}'. Error: {1}", dbName, e.Message);
                        SendNoHandler(eventMsg);
                        return;
                    }
                }
            }

            sessions.Add(eventMsg.SessionGuid, data);

			if(scheduler.StartScript(script, data, eventMsg) == false)
			{
				SendNoHandler(eventMsg);
			}
        }

        private void SendNoHandler(EventMessage msg)
        {
            SendNoHandler(msg, false);
        }

        private void SendNoHandler(EventMessage msg, bool sessionActive)
        {
            if(msg.SourceQueue == null || msg.SuppressNoHandler == true)
                return;

            string actionGuid = msg.RoutingGuid + ".0";

            ActionMessage noHandlerMsg = messageUtility.CreateActionMessage(IActions.NoHandler, actionGuid, 
                msg.AppName, msg.ScriptName, msg.PartitionName);
            noHandlerMsg.AddField(IActions.Fields.InnerMsg, msg);
            noHandlerMsg.AddField(IActions.Fields.SessionActive, sessionActive);

            msg.SourceQueue.PostMessage(noHandlerMsg);
        }
        #endregion

        #region Action Handling

        public void HandleProviderAction(RuntimeScriptInfo scriptInfo, ProviderAction pAction, string actionGuid)
        {
            Assertion.Check(routerQ != null, "Router queue is null.");
            Assertion.Check(scriptInfo != null, "Script info is null.");
            Assertion.Check(scriptInfo.sessionData != null, "Script session is null.");
            Assertion.Check(pAction != null, "Provider action is null");

            ActionMessage msg = messageUtility.CreateActionMessage(pAction.name, actionGuid, AppEnvironment.AppMetaData.Name, 
                scriptInfo.ScriptName, scriptInfo.sessionData.PartitionName);
            
			msg.SessionGuid = scriptInfo.routingGuid;
            msg.Locale = scriptInfo.sessionData.Culture;

            foreach(ActionParamBase aParam in pAction.actionParams)
            {
                if(aParam.Value != null)
                {                    
                    if(string.Compare(aParam.name, ICommands.Fields.USER_DATA, true) == 0)
                    {
                        msg.UserData = aParam.Value;
                    }
                    else
                    {
                        msg.AddField(aParam.name, aParam.Value);
                    }
                }
                else
                {
                    // Is this what we really want?
                    scriptInfo.log.Write(TraceLevel.Warning, "Provider action '{0}' parameter '{1}' has a null value", 
                        pAction.name, aParam.name);
                    msg.AddField(aParam.name, null);
                }
            }

            // Route Metreos.CallControl actions directly to the telephony manager
            if(msg.MessageId.StartsWith(ICallControl.NAMESPACE))
                telManQ.PostMessage(msg);
            else
                routerQ.PostMessage(msg);
        }

        public void SendDestructorEvent(string routingGuid, IApp.DestructorCodes errorCode, string errorText)
        {
            EventMessage dEvent = base.CreateEventMessage(IEvents.Destruction, EventMessage.EventType.NonTriggering, routingGuid);
            dEvent.AddField(IEvents.Fields.ErrorCode, errorCode);
            dEvent.AddField(IEvents.Fields.ErrorText, errorText == null ? errorCode.ToString() : errorText);
            scheduler.HandleNonTriggeringEvent(dEvent);
        }

		public void HandleRecycleScript(ScriptData script, string routingGuid)
		{
			repository.Recycle(script);

			CommandMessage msg = 
				messageUtility.CreateCommandMessage(IConfig.CoreComponentNames.ROUTER, ICommands.SCRIPT_ENDED);
			msg.AddField(ICommands.Fields.ROUTING_GUID, routingGuid);
            msg.AddField(ICommands.Fields.SCRIPT_NAME, script.name);

			routerQ.PostMessage(msg);
            telManQ.PostMessage(msg);
		}

        public void EnableApplication()
        {
            CommandMessage cmd = base.CreateCommandMessage(IConfig.CoreComponentNames.ROUTER, ICommands.ENABLE_APP);
            cmd.AddField(ICommands.Fields.APP_NAME, AppEnvironment.AppMetaData.Name);
            cmd.SourceQueue = null;                                     // Not interested in a response
            routerQ.PostMessage(cmd);
        }

        public void HandleEndSession(string sessionGuid)
        {
            lock(sessions)
            {
                SessionData sData = sessions[sessionGuid];
            
                if(sData != null)
                {
                    if(sData.DbConnections != null)
                        sData.DbConnections.Clear();

                    sessions.Remove(sessionGuid);
                }
            }

            CommandMessage msg = 
                messageUtility.CreateCommandMessage(IConfig.CoreComponentNames.ROUTER, ICommands.SESSION_ENDED);
            msg.AddField(ICommands.Fields.SESSION_GUID, sessionGuid);
            msg.AddField(ICommands.Fields.APP_NAME, AppEnvironment.AppMetaData.Name);
            routerQ.PostMessage(msg);
        }

        public void RepostForwardedEvents(RuntimeScriptInfo scriptInfo)
        {
            if(scriptInfo.unhandledEvents.Count > 0)
            {
                foreach(EventMessage eMsg in scriptInfo.unhandledEvents)
                {
                    routerQ.PostMessage(eMsg);
                }
                scriptInfo.unhandledEvents.Clear();
            }
        }

        #endregion

		#region Debug Command Handling
		
        #region Incoming Commands

        private void HandleStartDebugging(CommandMessage cMsg)
        {
            string scriptName = cMsg.Destination;
            Assertion.Check(scriptName != null, "Script name is null in StopDebugging message");

            string transId = cMsg[ICommands.Fields.TRANS_ID] as string;
            Assertion.Check(transId != null, "Transaction ID is null in StopDebugging message");

            // Snag a writer to the app manager
            if(appManQ == null)
            {
                appManQ = cMsg.SourceQueue;
            }

            ArrayList fields = new ArrayList();

            if(repository.Exists(scriptName) == false)
            {
                string failReason = String.Format("Script '{0}' does not exist in application '{0}'",
                    scriptName, AppEnvironment.AppMetaData.Name);
                fields.Add(new Field(ICommands.Fields.FAIL_REASON, failReason));
                cMsg.SendResponse(IApp.VALUE_FAILURE, fields, false);
                return;
            }

            scheduler.Debug_Start(scriptName);
            cMsg.SendResponse(IApp.VALUE_SUCCESS, fields, false);
        }

        private void HandleBreak(CommandMessage cMsg)
        {
            string scriptName = cMsg.Destination;
            Assertion.Check(scriptName != null, "Script name is null in ExecuteAction message");

            string transId = cMsg[ICommands.Fields.TRANS_ID] as string;
            Assertion.Check(transId != null, "Transaction ID is null in ExecuteAction message");

            ArrayList fields = new ArrayList();

            string failReason = null;
            if(scheduler.Debug_Break(scriptName, out failReason) == false)
            {
                fields.Add(new Field(ICommands.Fields.FAIL_REASON, failReason));
                cMsg.SendResponse(IApp.VALUE_FAILURE, fields, false);
            }

            // Success response is sent by Scheduler
        }

		private void HandleSetBreakpoint(CommandMessage cMsg)
		{
            string failReason = null;

			string scriptName = cMsg.Destination;
			Assertion.Check(scriptName != null, "Script name is null in SetBreakpoint message");

			string transId = cMsg[ICommands.Fields.TRANS_ID] as string;
			Assertion.Check(transId != null, "Transaction ID is null in SetBreakpoint message");

            ArrayList fields = new ArrayList();

			string actionId = cMsg[ICommands.Fields.DEBUG_ACTION_ID] as string;
            if((actionId == null) || (actionId == String.Empty))
            {
                actionId = repository.GetFirstActionId(scriptName);

                if(actionId == null)
                {
                    failReason = String.Format("Script '{0}' does not exist in application '{1}'", scriptName, AppEnvironment.AppMetaData.Name);
                    log.Write(TraceLevel.Warning, failReason);
                    fields.Add(new Field(ICommands.Fields.FAIL_REASON, failReason));
                    cMsg.SendResponse(IApp.VALUE_FAILURE, fields, false);
                    return;
                }
            }

			if(scheduler.Debug_SetBreakpoint(scriptName, actionId, out failReason) == false)
			{
				fields.Add(new Field(ICommands.Fields.FAIL_REASON, failReason));
				cMsg.SendResponse(IApp.VALUE_FAILURE, fields, false);
				return;
			}

			cMsg.SendResponse(IApp.VALUE_SUCCESS, fields, false);
		}

        private void HandleClearBreakpoint(CommandMessage cMsg)
        {
            string failReason = null;

            string scriptName = cMsg.Destination;
            Assertion.Check(scriptName != null, "Script name is null in SetBreakpoint message");

            string transId = cMsg[ICommands.Fields.TRANS_ID] as string;
            Assertion.Check(transId != null, "Transaction ID is null in SetBreakpoint message");

            ArrayList fields = new ArrayList();

            string actionId = cMsg[ICommands.Fields.DEBUG_ACTION_ID] as string;
            if(actionId == null)
            {
                failReason = String.Format("No action ID specified in ClearBreakpoint command");
                log.Write(TraceLevel.Warning, failReason);
                fields.Add(new Field(ICommands.Fields.FAIL_REASON, failReason));
                cMsg.SendResponse(IApp.VALUE_FAILURE, fields, false);
                return;
            }

            if(scheduler.Debug_ClearBreakpoint(scriptName, actionId, out failReason) == false)
            {
                fields.Add(new Field(ICommands.Fields.FAIL_REASON, failReason));
                cMsg.SendResponse(IApp.VALUE_FAILURE, fields, false);
                return;
            }

            cMsg.SendResponse(IApp.VALUE_SUCCESS, fields, false);
        }

        private void HandleGetBreakpoints(CommandMessage cMsg)
        {
            string scriptName = cMsg.Destination;
            Assertion.Check(scriptName != null, "Script name is null in GetBreakpoints message");

            string transId = cMsg[ICommands.Fields.TRANS_ID] as string;
            Assertion.Check(transId != null, "Transaction ID is null in GetBreakpoints message");

            ArrayList fields = new ArrayList();

            string failReason = null;
            StringCollection breakpoints = scheduler.Debug_GetBreakpoints(scriptName, out failReason);
            Stack breakStack = new Stack();

            if(breakpoints != null)
            {
                // Convert to Stack for two reasons:
                //   1. StringCollection is not binary serializable
                //   2. A Stack member (ripe for exploitation) already exists in DebugResponse message
                foreach(string actionId in breakpoints)
                {
                    breakStack.Push(actionId);
                }

                fields.Add(new Field(ICommands.Fields.ACTION_STACK, breakStack));
            }
            else
            {
                fields.Add(new Field(ICommands.Fields.FAIL_REASON, failReason));
            }

            cMsg.SendResponse(IApp.VALUE_SUCCESS, fields, false);
        }

        private void HandleRun(CommandMessage cMsg)
        {
            string scriptName = cMsg.Destination;
            Assertion.Check(scriptName != null, "Script name is null in ExecuteAction message");

            string transId = cMsg[ICommands.Fields.TRANS_ID] as string;
            Assertion.Check(transId != null, "Transaction ID is null in ExecuteAction message");

            string actionId = cMsg[ICommands.Fields.DEBUG_ACTION_ID] as string;

            string failReason = null;
            if(scheduler.Debug_Run(scriptName, actionId, cMsg, out failReason) == false)
            {
                ArrayList fields = new ArrayList();
                fields.Add(new Field(ICommands.Fields.FAIL_REASON, failReason));
                cMsg.SendResponse(IApp.VALUE_FAILURE, fields, false);
            }

            // Success response is sent later
        }

		private void HandleExecuteAction(CommandMessage cMsg)
		{
			string scriptName = cMsg.Destination;
			Assertion.Check(scriptName != null, "Script name is null in ExecuteAction message");

			string transId = cMsg[ICommands.Fields.TRANS_ID] as string;
			Assertion.Check(transId != null, "Transaction ID is null in ExecuteAction message");

			string actionId = cMsg[ICommands.Fields.DEBUG_ACTION_ID] as string;

			Assertion.Check(cMsg.Contains(ICommands.Fields.STEP_INTO), 
                "No 'StepInto' or 'StepOver' specifier in ExecuteAction message in SchedulerTask");
			bool stepInto = (bool)cMsg[ICommands.Fields.STEP_INTO];

			string failReason = null;
			if(scheduler.Debug_ExecuteAction(scriptName, actionId, stepInto, cMsg, out failReason) == false)
			{
				ArrayList fields = new ArrayList();
				fields.Add(new Field(ICommands.Fields.FAIL_REASON, failReason));
				cMsg.SendResponse(IApp.VALUE_FAILURE, fields, false);
			}

			// Success response is sent later
		}

		private void HandleStopDebugging(CommandMessage cMsg)
		{
			string scriptName = cMsg.Destination;
			Assertion.Check(scriptName != null, "Script name is null in StopDebugging message");

			string transId = cMsg[ICommands.Fields.TRANS_ID] as string;
			Assertion.Check(transId != null, "Transaction ID is null in StopDebugging message");

			ArrayList fields = new ArrayList();

			string failReason = null;
			if(scheduler.Debug_Stop(scriptName, out failReason) == false)
			{
				fields.Add(new Field(ICommands.Fields.FAIL_REASON, failReason));
				cMsg.SendResponse(IApp.VALUE_FAILURE, fields, false);
				return;
			}

			cMsg.SendResponse(IApp.VALUE_SUCCESS, fields, false);
		}

        private void HandleUpdateValue(CommandMessage cMsg)
        {
            string scriptName = cMsg.Destination;
            Assertion.Check(scriptName != null, "Script name is null in UpdateValue message");

            string transId = cMsg[ICommands.Fields.TRANS_ID] as string;
            Assertion.Check(transId != null, "Transaction ID is null in UpdateValue message");

            string varName = cMsg[ICommands.Fields.VAR_NAME] as String;
            Assertion.Check(transId != null, "Variable name is null in UpdateValue message");

            string varValue = cMsg[ICommands.Fields.VAR_VALUE] as String;

            ArrayList fields = new ArrayList();
            
            string failReason = null;
            if(scheduler.Debug_UpdateValue(scriptName, varName, varValue, out failReason) == false)
            {
                fields.Add(new Field(ICommands.Fields.FAIL_REASON, failReason));
                cMsg.SendResponse(IApp.VALUE_FAILURE, fields, false);
                return;
            }

            cMsg.SendResponse(IApp.VALUE_SUCCESS, fields, false);
        }
        #endregion

        #region Outbound Commands

        public void HandleHitBreakpoint(RuntimeScriptInfo scriptInfo)
		{
			//if(appManQ == null) { return; }
			Assertion.Check(appManQ != null, "App Manager queue writer is null on HitBreakpoint");

			string actionId = scriptInfo.currFunction.CurrentElementId;

			Hashtable funcVars;
			Hashtable scriptVars;
			BuildDebugVarTables(scriptInfo, out funcVars, out scriptVars);

			CommandMessage cMsg = CreateCommandMessage(IConfig.CoreComponentNames.APP_MANAGER, ICommands.HIT_BREAKPOINT);
            cMsg.AddField(ICommands.Fields.APP_NAME, AppEnvironment.AppMetaData.Name);
			cMsg.AddField(ICommands.Fields.DEBUG_ACTION_ID, actionId);
			cMsg.AddField(ICommands.Fields.SCRIPT_NAME, scriptInfo.ScriptName);
			cMsg.AddField(ICommands.Fields.FUNCTION_VARS, funcVars);
			cMsg.AddField(ICommands.Fields.SCRIPT_VARS, scriptVars);
			cMsg.AddField(ICommands.Fields.SESSION_DATA, scriptInfo.sessionData);
            cMsg.AddField(ICommands.Fields.ACTION_STACK, scriptInfo.actionStack);

            try
            {
                appManQ.PostMessage(cMsg);
            }
            catch
            {
                log.Write(TraceLevel.Warning, "Could not serialize variable values for {0}:{1}",
                    AppEnvironment.AppMetaData.Name, scriptInfo.ScriptName);
                cMsg.RemoveField(ICommands.Fields.FUNCTION_VARS);
                cMsg.RemoveField(ICommands.Fields.SCRIPT_VARS);

                appManQ.PostMessage(cMsg);
            }
		}

        public void SendStopDebugging(RuntimeScriptInfo scriptInfo, string errorDescription)
        {
            Assertion.Check(appManQ != null, "App Manager queue writer is null on StopDebugging");

            CommandMessage cMsg = CreateCommandMessage(IConfig.CoreComponentNames.APP_MANAGER, ICommands.STOP_DEBUGGING);
            cMsg.AddField(ICommands.Fields.APP_NAME, AppEnvironment.AppMetaData.Name);
            cMsg.AddField(ICommands.Fields.SCRIPT_NAME, scriptInfo.script.name);
            cMsg.AddField(ICommands.Fields.DEBUG_ACTION_ID, scriptInfo.currFunction.CurrentElementId);
            cMsg.AddField(ICommands.Fields.FAIL_REASON, errorDescription);
            cMsg.AddField(ICommands.Fields.ACTION_STACK, scriptInfo.actionStack);

            appManQ.PostMessage(cMsg);
        }
        #endregion

        #region Outbound Responses

        public void SendDetailedResp(RuntimeScriptInfo scriptInfo, string nextActionId)
		{
			Assertion.Check(scriptInfo.debugCommandMsg != null, "Trying to send Detailed response without a corresponding command");

			CommandMessage cMsg = scriptInfo.debugCommandMsg;

			Hashtable funcVars;
			Hashtable scriptVars;
			BuildDebugVarTables(scriptInfo, out funcVars, out scriptVars);

			ArrayList fields = new ArrayList();
			//fields.Add(new Field(ICommands.Fields.DEBUG_ACTION_ID, nextActionId));
			fields.Add(new Field(ICommands.Fields.ACTION_RESULT, scriptInfo.currFunction.LastReturnValue));
			fields.Add(new Field(ICommands.Fields.FUNCTION_VARS, funcVars));
			fields.Add(new Field(ICommands.Fields.SCRIPT_VARS, scriptVars));
			fields.Add(new Field(ICommands.Fields.SESSION_DATA, scriptInfo.sessionData));
		    fields.Add(new Field(ICommands.Fields.ACTION_STACK, scriptInfo.actionStack));

			cMsg.SendResponse(IApp.VALUE_SUCCESS, fields, false);

            scriptInfo.debugCommandMsg = null;
		}
        #endregion

        #region Debug Message Helper Methods
		// The primary purpose of this function is to circumvent some nasty serialization issues.
		//   Even still, some variables (like StringDictionary) are simply not serializable.
		private void BuildDebugVarTables(RuntimeScriptInfo scriptInfo, out Hashtable funcVars, out Hashtable scriptVars)
		{
			funcVars = new Hashtable();
			scriptVars = new Hashtable();
			
            foreach(DictionaryEntry de in scriptInfo.currFunction.function.variables)
            {
				string name = de.Key as string;
				Variable var = de.Value as Variable;
				Assertion.Check(var != null, "A non-Variable entry was encountered in the function variable table");

                if(IsSerializable(var.Value))                
                {
                    funcVars.Add(name, var.Value);
                }
                else
                {
                    funcVars.Add(name, var.Value.ToString());
                }
			}

            foreach(DictionaryEntry de in scriptInfo.script.variables)
			{
				string name = de.Key as string;
				Variable var = de.Value as Variable;
				Assertion.Check(var != null, "A non-Variable entry was encountered in the script variable table");

                if(IsSerializable(var.Value))
                {
                    scriptVars.Add(name, var.Value);
                }
                else
                {
                    scriptVars.Add(name, var.Value.ToString());
                }
			}
		}

        private StringCollection serializableTypes = new StringCollection();

        private bool IsSerializable(object obj)
        {
            // This fuction determines whether an object is serializable 
            //   considering the possibility of a serializable object containing non-serializable
            //   objects to an arbitrary nesting depth. Once a type has been identified as
            //   serializable, it is added to a list in order to avoid infinite recursion.
            if(obj.GetType().IsSerializable || obj is System.Runtime.Serialization.ISerializable)
            {
                bool serializable = true;

                foreach(System.Reflection.FieldInfo fInfo in obj.GetType().GetFields())
                {
                    object fieldObj = fInfo.GetValue(obj);
                    if(serializableTypes.Contains(fieldObj.GetType().FullName) == false)
                    {
                        serializableTypes.Add(fieldObj.GetType().FullName);
                        serializable &= IsSerializable(fieldObj);
                        if(serializable == false)
                            serializableTypes.Remove(fieldObj.GetType().FullName);
                    }
                }

                if(obj is IEnumerable)
                {
                    IEnumerator e = ((IEnumerable)obj).GetEnumerator();
                    while(e.MoveNext())
                    {
                        if(e is IDictionaryEnumerator)
                        {
                            object enumObj = ((IDictionaryEnumerator)e).Key;
                            if(serializableTypes.Contains(enumObj.GetType().FullName) == false)
                            {
                                serializableTypes.Add(enumObj.GetType().FullName);
                                serializable &= IsSerializable(enumObj);
                                if(serializable == false)
                                    serializableTypes.Remove(enumObj.GetType().FullName);
                            }

                            enumObj = ((IDictionaryEnumerator)e).Value;
                            if(serializableTypes.Contains(enumObj.GetType().FullName) == false)
                            {
                                serializableTypes.Add(enumObj.GetType().FullName);
                                serializable &= IsSerializable(enumObj);
                                if(serializable == false)
                                    serializableTypes.Remove(enumObj.GetType().FullName);
                            }
                        }
                        else
                        {
                            object enumObj = e.Current;
                            if(serializableTypes.Contains(enumObj.GetType().FullName) == false)
                            {
                                serializableTypes.Add(enumObj.GetType().FullName);
                                serializable &= IsSerializable(enumObj);
                                if(serializable == false)
                                    serializableTypes.Remove(enumObj.GetType().FullName);
                            }
                        }
                    }
                }

                return serializable;
            }

            return false;
        }
        #endregion

		#endregion
	}
}
