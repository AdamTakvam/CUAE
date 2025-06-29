using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Specialized;

using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;

using Metreos.Configuration;

namespace Metreos.AppServer.TelephonyManager
{
    public class TelManager : PrimaryTaskBase
    {
        private StateEngine stateEngine;

        #region Construction/Startup/Refresh/Shutdown/Dispose

        public TelManager()
            : base(IConfig.ComponentType.Core, 
                IConfig.CoreComponentNames.TEL_MANAGER, 
                IConfig.CoreComponentNames.TEL_MANAGER, 
                Config.TelephonyManager.LogLevel,
                Config.Instance)
        {
            stateEngine = new StateEngine(configUtility);
            stateEngine.GetQueueSize = new ScalarQueryDelegate(GetQueueSize);
        }

        protected override void OnStartup()
        {
            RefreshConfiguration(null);

            if(stateEngine.Start(Config.TmScriptsDir.FullName, log) == false)
            {
                throw new StartupFailedException("State Engine failed to start");
            }
        }

        protected override void RefreshConfiguration(string proxy)
        {
            stateEngine.EnableSandbox = Config.TelephonyManager.SandboxEnabled;
			stateEngine.EnableDiags = Config.TelephonyManager.DiagsEnabled;
        }

        protected override TraceLevel GetLogLevel()
        {
            return Config.TelephonyManager.LogLevel;
        }

        protected override void OnShutdown()
        {
            stateEngine.Stop();
        }

        internal int GetQueueSize()
        {
            return base.taskQueue.Length;
        }
        #endregion

        #region Internal Message Distributor

        protected override bool HandleMessage(InternalMessage message)
        {
            // Handle special commands
            CommandMessage cMsg = message as CommandMessage;
            ActionMessage aMsg = message as ActionMessage;
            if(cMsg != null)
            {
                switch(cMsg.MessageId)
                {
                    case ICommands.SCRIPT_ENDED:
                        HandleEndScript(cMsg);
                        return true;
                    case ICommands.REGISTER_PROV_NAMESPACE:
                        HandleRegisterProvider(cMsg);
                        return true;
                    case ICommands.UNREGISTER_PROV_NAMESPACE:
                        HandleUnregisterProvider(cMsg);
                        return true;
                    case ICommands.CLEAR_CALL_TABLE:
                        HandleClearCallTable();
                        return true;
                    case ICommands.PRINT_DIAGS:
                        HandlePrintDiags();
                        return true;
                    case ICommands.END_ALL_CALLS:
                        HandleEndAllCalls();
                        return true;
                    case ICommands.CLEAR_CRG_CACHE:
                        HandleClearCrgCache();
                        return true;
                }
            }
            else if(aMsg != null && aMsg.MessageId == IActions.Forward)
            {
                string destGuid = aMsg[IActions.Fields.ToGuid] as String;
                if(destGuid == null)
                    log.Write(TraceLevel.Error, "No destination routing GUID specified in Forward action");
                else if(stateEngine.ChangeActionGuid(aMsg.RoutingGuid, destGuid) == false)
                    log.Write(TraceLevel.Info, "Ignoring Forward action for Routing GUID '{0}' since it has not placed or answered any calls.", aMsg.RoutingGuid);
                else
                    log.Write(TraceLevel.Info, "Routing GUID '{0}' changed to '{1}'", aMsg.RoutingGuid, destGuid);

                aMsg.SendResponse(IApp.VALUE_SUCCESS);
                return true;
            }
            else
            {
                stateEngine.EnqueueMessage(message);
                return true;
            }

            return false;
        }
        #endregion

        #region Special Message Handlers
        
        private void HandleEndScript(CommandMessage msg)
        {
            string routingGuid = msg[ICommands.Fields.ROUTING_GUID] as string;
            Assertion.Check(routingGuid != null, "Routing GUID is null in HandleEndScript");

            string scriptName = msg[ICommands.Fields.SCRIPT_NAME] as String;

            stateEngine.HandleEndScript(routingGuid, scriptName);
        }

        private void HandleRegisterProvider(CommandMessage msg)
        {
            if(msg.Contains(ICommands.Fields.CC_PROTOCOL) == false)
            {
                log.Write(TraceLevel.Error, "Received call control protocol provider registration with no protocol specified. (from: {0})",
                    msg.Source);
                return;
            }

            string ns = msg[ICommands.Fields.PROVIDER_NAMESPACE] as String;
            IConfig.ComponentType protocol = 
                (IConfig.ComponentType)Convert.ToUInt32(msg[ICommands.Fields.CC_PROTOCOL]);

            log.Write(TraceLevel.Verbose, "Registering provider namespace '{0}' to handle protocol type: {1}",
                ns, protocol.ToString());

            stateEngine.RegisterNamespace(protocol, ns);
        }

        private void HandleUnregisterProvider(CommandMessage msg)
        {
            StringCollection nss = msg[ICommands.Fields.PROVIDER_NAMESPACE] as StringCollection;
            if(nss != null)
                stateEngine.UnregisterNamespace(nss);
        }

        private void HandleClearCallTable()
        {
            stateEngine.ClearCallTable();
        }

        private void HandlePrintDiags()
        {
            stateEngine.PrintDiags();
        }

        private void HandleEndAllCalls()
        {
            stateEngine.EndAllCalls();
        }

        private void HandleClearCrgCache()
        {
            stateEngine.ClearCrgCache();
        }

        #endregion
    }
}
