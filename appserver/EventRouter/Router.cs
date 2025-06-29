using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Core;
using Metreos.Utilities;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Core.ConfigData;
using Metreos.ApplicationFramework;

using Metreos.Configuration;
using PMan=Metreos.AppServer.ProviderManager;
using Metreos.AppServer.EventRouter.Collections;

namespace Metreos.AppServer.EventRouter
{
	public sealed class Router : PrimaryTaskBase
	{
        private new Config configUtility;

        private AppTable appTable;
        private RouteTable routeTable;
        private TriggerTable triggerTable;
		private ForwardingTable forwardingTable;
        private MessageQueueWriter tmQ;

        private PMan.ProviderManager providerManager;

        private LicenseManager licenseManager;

        private readonly int discardThreshold;

        #region Constructor/Startup/Shutdown/Refresh Config

		public Router()
            : base(IConfig.ComponentType.Core, 
                IConfig.CoreComponentNames.ROUTER, 
                IConfig.CoreComponentNames.ROUTER, 
                Config.Router.LogLevel,
                Config.Instance)
		{
            configUtility = Config.Instance;

            appTable = new AppTable();
            routeTable = new RouteTable();
            triggerTable = new TriggerTable();
			forwardingTable = new ForwardingTable();

            providerManager = new PMan.ProviderManager();
            licenseManager = new LicenseManager();
            discardThreshold = Int32.Parse(AppConfig.GetEntry(IConfig.ConfigFileSettings.ROUTER_DISCARD_THRESHOLD));

            RefreshConfiguration(null);
		}
        
        protected override void RefreshConfiguration(string proxy)
        {
            if(proxy != null)
            {
                // If the management console updated an application script's triggering info,
                //  the app name will be tunnelled in the proxy field as "ApplicationName:<app name>"                
                if(proxy.StartsWith(ICommands.Fields.APP_NAME + ":"))
                {
                    string appName = proxy.Substring(ICommands.Fields.APP_NAME.Length + 1);
                    RefreshTriggerInfo(appName, null);
                }
                else if(proxy.StartsWith(ICommands.Fields.LICENSE_MANAGER))
                {
                    licenseManager.RefreshConfiguration(false);
                }
                else
                {
                    providerManager.RefreshConfiguration(proxy);
                }
            }
        }

        protected override void OnStartup() 
        {
            // startup the license manager synchronously
            licenseManager.Startup();
            providerManager.Startup();
            configUtility.UpdateStatus(Type, Name, IConfig.Status.Enabled_Running);
            configUtility.UpdateStatus(Type, IConfig.CoreComponentNames.PROV_MANAGER, IConfig.Status.Enabled_Running);

            tmQ = MessageQueueFactory.GetQueueWriter(IConfig.CoreComponentNames.TEL_MANAGER);
            Assertion.Check(tmQ != null, "Router: Could not obtain a queue writer for the Telephony Manager");
        }

        protected override void OnShutdown()
        {
            licenseManager.Shutdown();
            providerManager.Shutdown();
            configUtility.UpdateStatus(Type, Name, IConfig.Status.Enabled_Stopped);
            configUtility.UpdateStatus(Type, IConfig.CoreComponentNames.PROV_MANAGER, IConfig.Status.Enabled_Stopped);
        }

        protected override TraceLevel GetLogLevel()
        {
            return Config.Router.LogLevel;
        }

        private void HandlePrintDiags()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("[Router Diagnostics]\r\n");
            sb.Append("Triggers:      [] = Disabled\r\n");
            if(this.triggerTable.Count > 0)
            {
                foreach(TriggerInfo tInfo in this.triggerTable)
                {
                    if(!tInfo.enabled)
                        sb.Append("[");

                    sb.Append(tInfo.appName);
                    sb.Append(":");
                    sb.Append(tInfo.scriptName);
                    sb.Append(":");
                    sb.Append(tInfo.partitionName);
                    sb.Append(" -> ");
                    if(tInfo.eventName != null)
                    {
                        sb.Append(tInfo.eventName);
                        sb.Append("(");
                
                        if(tInfo.eventParams != null && tInfo.eventParams.Count > 0)
                        {
                            foreach(EventParam eParam in tInfo.eventParams)
                            {
                                sb.Append(eParam.name);
                                sb.Append("=");

                                StringCollection pValues = eParam.Value as StringCollection;
                                if(pValues != null)
                                {
                                    foreach(string val in pValues)
                                    {
                                        sb.Append(val);
                                        sb.Append(", ");
                                    }
                                    sb.Remove(sb.Length-2, 2);  // Remove trailing comma
                                }
                                else
                                {
                                    sb.Append(eParam.Value.ToString());
                                }
                                
                                sb.Append("; ");
                            }
                            sb.Remove(sb.Length-2, 2);  // Remove trailing semicolon
                        }

                        sb.Append(")");
                    }
                    else
                    {
                        sb.Append("ALL");
                    }

                    if(!tInfo.enabled)
                        sb.Append("]");

                    sb.Append("\r\n");
                }
            }
            else
            {
                sb.Append("(None)\r\n");
            }

            log.ForceWrite(TraceLevel.Info, sb.ToString());
        }

        public override void Dispose()
        {
            if(appTable != null)
            {
                appTable.Clear();
                appTable = null;
            }

            if(routeTable != null)
            {
                routeTable.Clear();
                routeTable = null;
            }

            if(triggerTable != null)
            {
                triggerTable.Clear();
                triggerTable = null;
            }

            if(providerManager != null)
            {
                providerManager.Dispose();
                providerManager = null;
            }
			
			if(forwardingTable != null)
			{
				forwardingTable.Clear();
				forwardingTable = null;
			}

            base.Dispose();
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
        #endregion

        #region Internal Message Distributor

        protected override bool HandleMessage(InternalMessage message)
        {
            EventMessage evtMsg = message as EventMessage;
            if(evtMsg != null)
            {
                if(evtMsg.IsComplete == false)
                {
                    log.Write(TraceLevel.Error, "Received malformed event message:\n" + evtMsg);
                    SendNoHandler(evtMsg);
                    return true;
                }

                // If our queue backs up stop processing triggering events
                if(evtMsg.Type == EventMessage.EventType.Triggering && taskQueue.Length > discardThreshold)
                {
                    if (!evtMsg.SuppressNoHandler)
                        SendNoHandler(evtMsg);

                    log.Write(TraceLevel.Error, 
                        "Discarding triggering event, queue high water alert: q={0}, msg={1}, src={2}", 
                        taskQueue.Length, evtMsg.MessageId, evtMsg.Source);
                    
                    return true;
                }

                // Check if this routing GUID is being forwarded
                string tmpRoutingGuid = forwardingTable[evtMsg.RoutingGuid];
                if(tmpRoutingGuid != null)
                {
                    evtMsg.RoutingGuid = tmpRoutingGuid;
                    
                    // tmpRoutingGuid not used for anything except
                    // rewriting the msg's routing guid if forwarded.
                }

                // Divert call control and media digit events to Telephony Manager
                if((evtMsg.MessageId.StartsWith(ICallControl.NAMESPACE) ||
                    evtMsg.MessageId == IMediaControl.Events.GotMediaDigits) &&
                    evtMsg.Source != IConfig.CoreComponentNames.TEL_MANAGER)
                {
                    long time = HPTimer.Now();
                    tmQ.PostMessage(message);
                    time = HPTimer.MillisSince(time);
                    if(time > 32)
                        log.Write(TraceLevel.Warning, "DIAG: tmQ.PostMessage: time={0}, msg={1}, src={2}", time, evtMsg.MessageId, evtMsg.Source);
                    return true;
                }

                switch(evtMsg.Type)
                {
                    case EventMessage.EventType.Triggering:
                        HandleTriggeringEvent(evtMsg);
                        return true;
                    case EventMessage.EventType.AsyncCallback:
                    case EventMessage.EventType.NonTriggering:
                        HandleNonTriggeringEvent(evtMsg);
                        return true;
                }
                return true;
            }

            ActionMessage actMsg = message as ActionMessage;
            if(actMsg != null)
            {
                switch(actMsg.MessageId)
                {
                    case IActions.Forward:
                        HandleForwardAction(actMsg);
                        return true;
                    case IActions.SendEvent:
                        HandleSendEventAction(actMsg);
                        return true;
                    default:
                        providerManager.RouteAction(actMsg);
                        return true;
                }
            }

            ResponseMessage respMsg = message as ResponseMessage;
            if(respMsg != null)
            {
                if(respMsg.Destination == IConfig.CoreComponentNames.PROV_MANAGER)
                {
                    providerManager.HandleResponseMessage(respMsg);
                }
                else
                {
                    RouteResponse(respMsg);
                }
                return true;
            }

            CommandMessage cmdMsg = message as CommandMessage;
            if(cmdMsg != null)
            {
                if(cmdMsg.Destination == IConfig.CoreComponentNames.PROV_MANAGER)
                {
                    return providerManager.HandleCommandMessage(cmdMsg);
                }
                
                switch(cmdMsg.MessageId)
                {
                    case ICommands.REGISTER_SCRIPT:
                        HandleRegisterScript(cmdMsg);
                        return true;
                    case ICommands.SCRIPT_ENDED:
                        HandleScriptEnded(cmdMsg);
                        return true;
                    case ICommands.SESSION_ENDED:
                        HandleSessionEnded(cmdMsg);
                        return true;
                    case ICommands.DISABLE_APP:
						if((HandleDisableApp(cmdMsg) == true) && (cmdMsg.SourceQueue != null))
						{
							cmdMsg.SendResponse(IApp.VALUE_SUCCESS, null, false);
						}
						else if(cmdMsg.SourceQueue != null)
						{
							ArrayList fields = new ArrayList();
							fields.Add(new Field(ICommands.Fields.FAIL_REASON, "Unknown application: " + cmdMsg));
							cmdMsg.SendResponse(IApp.VALUE_FAILURE, fields, false);
						}
						return true;

                    case ICommands.ENABLE_APP:
						if((HandleEnableApp(cmdMsg) == true) && (cmdMsg.SourceQueue != null))
						{
							cmdMsg.SendResponse(IApp.VALUE_SUCCESS, null, false);
						}
						else if(cmdMsg.SourceQueue != null)
						{
							ArrayList fields = new ArrayList();
							fields.Add(new Field(ICommands.Fields.FAIL_REASON, "Unknown application: " + cmdMsg));
							cmdMsg.SendResponse(IApp.VALUE_FAILURE, fields, false);
						}
						return true;

                    case ICommands.UNINSTALL_APP:
						if((HandleUninstallApp(cmdMsg) == true) && (cmdMsg.SourceQueue != null))
						{
							cmdMsg.SendResponse(IApp.VALUE_SUCCESS, null, false);
						}
						else if(cmdMsg.SourceQueue != null)
						{
							ArrayList fields = new ArrayList();
							fields.Add(new Field(ICommands.Fields.FAIL_REASON, "Unknown application: " + cmdMsg));
							cmdMsg.SendResponse(IApp.VALUE_FAILURE, fields, false);
						}
						return true;

                    case ICommands.PRINT_DIAGS:
                        HandlePrintDiags();
                        return true;
                }
            }

            return false;
        }
        #endregion

        #region Handle Special Actions
        
        private void HandleForwardAction(ActionMessage msg)
        {
			string srcGuid = ActionGuid.GetRoutingGuid(msg.ActionGuid);
			Assertion.Check(srcGuid != null, "Could not infer routing GUID from action GUID '{0}' in Forward action", msg.ActionGuid);

			string destGuid = msg[IActions.Fields.ToGuid] as String;
			if(destGuid == null)
			{
				log.Write(TraceLevel.Error, "No destination routing GUID specified in Forward action");
				return;
			}

			forwardingTable.Add(srcGuid, destGuid);

            // Proxy this message on to TM, so it can update its info
            tmQ.PostMessage(msg);
        }

        private void HandleSendEventAction(ActionMessage msg)
        {
			// the Guid that the event will be sent to
            string destinationGuid = null;
            
            string eventName = msg[ICommands.Fields.EVENT_NAME] as string;
			if(eventName == null)
			{
				log.Write(TraceLevel.Error, "No event name specified in SendEvent action");
				msg.SendResponse(IApp.VALUE_FAILURE);
				return;
			}
	
            string triggeredScriptGuid = Guid.NewGuid().ToString();
            EventMessage eMsg = base.CreateEventMessage(eventName, EventMessage.EventType.Triggering, triggeredScriptGuid);

			ArrayList fields = msg.Fields;
			for(int i=0; i<fields.Count; i++)
			{
				Field field = fields[i] as Field;
				eMsg.AddField(field.Name, field.Value);
			}

			string routingGuid = msg[IActions.Fields.ToGuid] as string;
			if(routingGuid != null)
			{
				// This is a non-triggering event
				eMsg.RoutingGuid = routingGuid;
				eMsg.Type = EventMessage.EventType.NonTriggering;

                if(HandleNonTriggeringEvent(eMsg, true) == false)
                {
                    log.Write(TraceLevel.Error, "Unable to route non-triggering event for SendEvent action:\n" + eMsg);
                    msg.SendResponse(IApp.VALUE_FAILURE);
                    return;
                }
                else
                {
                    destinationGuid = routingGuid;
                }
			}
			else
			{
                if(HandleTriggeringEvent(eMsg, true) == false)
                {
                    log.Write(TraceLevel.Error, "Unable to route triggering event for SendEvent action:\n" + eMsg);
                    msg.SendResponse(IApp.VALUE_FAILURE);
                    return;
                }
                else
                {
                    destinationGuid = triggeredScriptGuid;
                }
			}

            // append the destination guid to the SendResponse.
            ArrayList responseFields = new ArrayList();
            responseFields.Add(new Field(IActions.Fields.DestGuid, destinationGuid));
            msg.SendResponse(IApp.VALUE_SUCCESS, responseFields, true);
        }

        #endregion

        #region Action/Event Routing

		private void HandleTriggeringEvent(EventMessage msg)
		{
			HandleTriggeringEvent(msg, false);
		}

        private bool HandleTriggeringEvent(EventMessage msg, bool suppressNoHandler)
        {
            // check to see if script is allowed to run  
            if (!(licenseManager.IsScriptAllowedToRun()))
            {
                if(!suppressNoHandler)
                {
                    SendNoHandler(msg);
                }
    
                return false;
            }
            
            // Get matching trigger info
            TriggerInfo tInfo = triggerTable.GetTriggerInfo(msg);

            if(tInfo == null)
            {
                if(log.LogLevel == TraceLevel.Verbose)
                    log.Write(TraceLevel.Verbose, "No handler registered for incoming event:\r\n" + msg);
                else
                    log.Write(TraceLevel.Info, "No handler registered for incoming event: " + msg.MessageId);
				
				if(!suppressNoHandler) 
				{ 
					SendNoHandler(msg); 
				}
                return false;
            }

            // Specify the recipient
            msg.AppName = tInfo.appName;
            msg.ScriptName = tInfo.scriptName;
            msg.PartitionName = tInfo.partitionName;
            msg.Culture = tInfo.culture;

            // Add the session GUID
            if(tInfo.IsMaster == false)
            {
                tInfo.DecrementNumHits();

                if(tInfo.Expired)
                {
                    triggerTable.Remove(tInfo.ID);
                }

                msg.SessionGuid = tInfo.sessionGuid;
            }
            else
            {
                msg.SessionGuid = msg.RoutingGuid;
            }

            // Get the queue writer
            AppInfo appInfo = appTable[tInfo.appName];
            
            if(appInfo == null)
            {
                log.Write(TraceLevel.Error, "Found a trigger with no corresponding app for:\n" + msg);

                if(!suppressNoHandler) 
                { 
                    SendNoHandler(msg); 
                }
                return false;
            }

            // Record the route info
            routeTable[msg.RoutingGuid] = tInfo.appName;

            // Send
            appInfo.AppQ.PostMessage(msg);

            //increment the number of running scripts
            licenseManager.IncrementNumberOfRunningInstances();

			return true;
        }

		private void HandleNonTriggeringEvent(EventMessage msg)
		{
			HandleNonTriggeringEvent(msg, false);
		}

		private bool HandleNonTriggeringEvent(EventMessage msg, bool suppressNoHandler)
        {
            if(msg.AppName == null)
            {
                msg.AppName = routeTable[msg.RoutingGuid];

                if(msg.AppName == null)
                {
                    log.Write(TraceLevel.Info, "No handler registered for nontriggering event: " + msg.MessageId);

                    if(!suppressNoHandler)
                        SendNoHandler(msg);

                    return false;
                }
            }

            AppInfo appInfo = appTable[msg.AppName];

            if(appInfo == null)
            {
                log.Write(TraceLevel.Info, "No handler registered for nontriggering event: " + msg.MessageId);

				if(!suppressNoHandler)
					SendNoHandler(msg);

                return false;
            }

            appInfo.AppQ.PostMessage(msg);
			return true;
        }

        private void RouteResponse(ResponseMessage respMsg)
        {
            string appName = routeTable[respMsg.RoutingGuid];
            if(appName == null)
            {
                log.Write(TraceLevel.Warning, "Cannot route response to application:\n{0}", respMsg);
                return;
            }

            AppInfo appInfo = appTable[appName];
            if(appInfo == null)
            {
                log.Write(TraceLevel.Warning, "Cannot route response to null application: {0}\n{1}", appName, respMsg);
                return;
            }

            appInfo.AppQ.PostMessage(respMsg);
        }

        private void SendNoHandler(EventMessage msg)
        {
            if(msg.SourceQueue == null) { return; }

            string actionGuid = String.Format("{0}.{1}", msg.RoutingGuid, "0");

            ActionMessage noHandlerMsg = messageUtility.CreateActionMessage(IActions.NoHandler, actionGuid, 
                String.Empty, String.Empty, String.Empty);
            noHandlerMsg.AddField(IActions.Fields.InnerMsg, msg);
            noHandlerMsg.AddField(IActions.Fields.SessionActive, false);

            long time = HPTimer.Now();
            msg.SourceQueue.PostMessage(noHandlerMsg);
            time = HPTimer.MillisSince(time);
            if(time > 32)
                log.Write(TraceLevel.Warning, "DIAG: SendNoHandler: time={0}, msg={1}, src={2}", time, msg.MessageId, msg.Source);
        }
        #endregion

        #region Application/Script Commands
        private void HandleRegisterScript(CommandMessage msg)
        {
            MessageQueueWriter appQ = msg.RemoveField(ICommands.Fields.APP_QUEUE) as MessageQueueWriter;
            Assertion.Check(appQ != null, "No app message queue specified in first RegisterScript message:\n" + msg);

            // Populate trigger info structure
            TriggerInfo tInfo = new TriggerInfo();
            tInfo.appName = msg.RemoveField(ICommands.Fields.APP_NAME) as String;
            tInfo.scriptName = msg.RemoveField(ICommands.Fields.SCRIPT_NAME) as String;
            tInfo.partitionName = msg.RemoveField(ICommands.Fields.PARTITION_NAME) as String;
            tInfo.eventName = msg.RemoveField(ICommands.Fields.EVENT_NAME) as String;
            tInfo.sessionGuid = msg.RemoveField(ICommands.Fields.SESSION_GUID) as String;
            tInfo.enabled = Convert.ToBoolean(msg.RemoveField(ICommands.Fields.ENABLED));

            AppPartitionInfo pInfo = configUtility.GetPartitionInfo(tInfo.appName, tInfo.partitionName);
            if(pInfo == null)
                return;

            tInfo.culture = pInfo.Culture;

            log.Write(TraceLevel.Verbose, "Got register script request for {0}:{1}",
                tInfo.appName, tInfo.scriptName);

            AppInfo appInfo = appTable[tInfo.appName];
            if(appInfo == null)
            {
                appInfo = new AppInfo(appQ);
                appTable[tInfo.appName] = appInfo;
            }

            // If trigger information is present in the database, use it instead of the stuff specified
            if(configUtility.ScriptExists(tInfo.appName, tInfo.scriptName))
            {
                log.Write(TraceLevel.Verbose, "Preserving existing triggering criteria for extant script {0}:{1}",
                    tInfo.appName, tInfo.scriptName);

                RefreshTriggerInfo(tInfo.appName, tInfo.scriptName);
                return;
            }

            log.Write(TraceLevel.Verbose, "Building trigger tables for new script {0}:{1}",
                tInfo.appName, tInfo.scriptName);

            if(msg.Contains(ICommands.Fields.NUM_HITS))
            {
                try
                {
                    tInfo.NumHits = Convert.ToInt32(msg.RemoveField(ICommands.Fields.NUM_HITS));
                }
                catch(Exception) {}
            }
            
            ArrayList fields = msg.Fields;          // Potentially heavy-weight operation
            for(int i=0; i<fields.Count; i++)
            {
                Field field = fields[i] as Field;
                if(field != null)
                {
                    object Value = field.Value;
                    if(Value != null)
                    {
                        tInfo.eventParams.Add(field.Name, Value);
                    }
                }
            }

            Assertion.Check(tInfo.IsComplete, "RegisterScript is missing required fields:\n" + msg);

            // Check to see if the script already exists
            string id = triggerTable.GetId(tInfo.appName, tInfo.scriptName, tInfo.partitionName);
            if(id != null)
            {
                triggerTable.Remove(id);
            }

            // Add to trigger table
            tInfo.ID = triggerTable.Add(tInfo);

            // Add trigger ID to app table
            if(tInfo.IsMaster)
            {
                appInfo.AddMasterTriggerId(tInfo.ID);

                // Add master script trigger info to database
                if(configUtility.AddScript(tInfo.appName, tInfo.scriptName, tInfo.eventName))
                {
                    foreach(EventParam eParam in tInfo.eventParams)
                    {
                        configUtility.AddScriptTriggerParam(tInfo.appName, tInfo.scriptName, Utilities.Database.DefaultValues.PARTITION_NAME,
                            eParam.name, eParam.Value);
                    }
                }
            }
            else
            {
                appInfo.AddSlaveTriggerId(tInfo.sessionGuid, tInfo.ID);
            }
        }

        private void RefreshTriggerInfo(string appName, string scriptName)
        {
            // Get current app info
            AppInfo appInfo = appTable[appName];
            if(appInfo == null)
            {
                log.Write(TraceLevel.Warning, "Ignoring trigger changes for not loaded application: " + appName);
                return;
            }

            if(scriptName != null)
            {
                string[] triggerIds = triggerTable.GetIds(appName, scriptName);
                
                if(triggerIds != null)
                {
                    foreach(string triggerId in triggerIds)
                    {
                        triggerTable.Remove(triggerId);
                        appInfo.MasterTriggerIds.Remove(triggerId);
                    }
                }
            }
            else
            {
                // Clear out all its triggers
                triggerTable.Remove(appInfo.MasterTriggerIds);
                appInfo.MasterTriggerIds.Clear();
            }

            // Get new trigger info from database
            TriggerInfo[] triggers = configUtility.GetAppTriggerInfo(appName);
            if(triggers == null)
            {
                log.Write(TraceLevel.Error, "No trigger(s) registered for application: " + appName);
                return;
            }

            // Store new triggers
            foreach(TriggerInfo tInfo in triggers)
            {
                if(scriptName == null || (scriptName == tInfo.scriptName))
                {
                    tInfo.ID = triggerTable.Add(tInfo);
                    appInfo.AddMasterTriggerId(tInfo.ID);
                }
            }
        }

        private void HandleScriptEnded(CommandMessage msg)
        {
            string routingGuid = msg[ICommands.Fields.ROUTING_GUID] as string;

			forwardingTable.Remove(routingGuid);

            if(routingGuid != null)
            {
                routeTable.Remove(routingGuid);
            }

            licenseManager.DecrementNumberOfRunningInstances();
        }

        private void HandleSessionEnded(CommandMessage msg)
        {
            string appName = msg[ICommands.Fields.APP_NAME] as string;
            string sessionGuid = msg[ICommands.Fields.SESSION_GUID] as string;

            HandleSessionEnded(appName, sessionGuid);
        }

        private void HandleSessionEnded(string appName, string sessionGuid)
        {
            if((appName != null) && (sessionGuid != null))
            {
                AppInfo appInfo = appTable[appName];

                if(appInfo != null)
                {
                    StringCollection ids = appInfo.GetSlaveTriggerIds(sessionGuid);
                    
                    triggerTable.Remove(ids);

                    appInfo.RemoveSession(sessionGuid);
                }
            }
        }

        private bool HandleUninstallApp(CommandMessage msg)
        {
            string appName = msg[ICommands.Fields.APP_NAME] as string;

            if(appName != null)
            {
                AppInfo appInfo = appTable[appName];

                if(appInfo != null)
                {
                    // End all sessions
                    StringCollection sessionGuids = new StringCollection();
                    IEnumerator e = appInfo.SessionGuids;
                    while(e.MoveNext())
                    {
                        string sessionGuid = e.Current as string;
                        Assertion.Check(sessionGuid != null, "Unexpected element returned by AppInfo.SessionGuids");

                        sessionGuids.Add(sessionGuid);
                    }

                    foreach(string sessionGuid in sessionGuids)
                    {
                        HandleSessionEnded(appName, sessionGuid);
                    }
                    sessionGuids.Clear();

                    // Remove master script trigger info
                    triggerTable.Remove(appInfo.MasterTriggerIds);
                    
                    // Remove app from app table
                    appTable.Remove(appName);
                }
				return true;
            }

			return false;
        }

        private bool HandleDisableApp(CommandMessage msg)
        {
            string appName = msg[ICommands.Fields.APP_NAME] as string;

            if(appName != null)
            {
                configUtility.UpdateStatus(IConfig.ComponentType.Application, appName, IConfig.Status.Disabled);

                AppInfo appInfo = appTable[appName];
                
                if(appInfo != null)
                {
                    foreach(string triggerId in appInfo.MasterTriggerIds)
                    {
                        TriggerInfo tInfo = triggerTable[triggerId];
                        if(tInfo != null)
                        {
                            tInfo.enabled = false;
                        }
                    }
                }
				return true;
            }

			return false;
        }

        private bool HandleEnableApp(CommandMessage msg)
        {
            string appName = msg[ICommands.Fields.APP_NAME] as string;
            
            if(appName != null)
            {
                configUtility.UpdateStatus(IConfig.ComponentType.Application, appName, IConfig.Status.Enabled_Running);

                AppInfo appInfo = appTable[appName];
                
                if(appInfo != null)
                {
                    foreach(string triggerId in appInfo.MasterTriggerIds)
                    {
                        TriggerInfo tInfo = triggerTable[triggerId];
                        if(tInfo != null)
                        {
                            tInfo.enabled = true;
                        }
                    }
                }
				return true;
            }

			return false;
        }
        #endregion
	}
}
