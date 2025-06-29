using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Core.Sockets;
using Metreos.Core.IPC.Xml;
using Metreos.Core.ConfigData;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Configuration;

namespace Metreos.AppServer.Management
{
	public sealed class ManagementInterface : PrimaryTaskBase
	{
        #region Authentication info class

        private sealed class AuthInfo
        {
            public string RemoteHost { get { return remoteHost; } }
            private readonly string remoteHost;

            public bool Authenticated 
            { 
                get { return authenticated; } 
                set { authenticated = value; } 
            }
            private bool authenticated;

            public AuthInfo(string remoteHost)
            {
                this.remoteHost = remoteHost;
            }
        }
        #endregion

        MessageQueueWriter routerQ;
        MessageQueueWriter appManagerQ;
        MessageQueueWriter mediaManagerQ;
        MessageQueueWriter telManQ;
        MessageQueueWriter appServerQ;
        MessageQueueWriter clusterQ;

        private readonly XmlSerializer commandSerializer = new XmlSerializer(typeof(commandType));
        private readonly XmlSerializer responseSerializer = new XmlSerializer(typeof(responseType));

        private readonly IpcXmlServer ipcServer;
        private readonly Hashtable authTable;

        /// <summary>Stats/Alarms client connection</summary>
        private Metreos.Stats.StatsClient statsClient;

		public ManagementInterface()
            : base(IConfig.ComponentType.Core, 
                IConfig.CoreComponentNames.MANAGEMENT, 
                IConfig.CoreComponentNames.MANAGEMENT, 
                Config.Management.LogLevel, 
                Config.Instance)
		{
            this.authTable = new Hashtable();
            this.statsClient = Metreos.Stats.StatsClient.Instance;

            this.ipcServer = new IpcXmlServer(typeof(ManagementInterface).Name, Config.Management.ManagementPort, 
                false, Config.Management.LogLevel);
            this.ipcServer.OnNewConnection += new NewConnectionDelegate(OnNewConnection);
            this.ipcServer.OnCloseConnection += new CloseConnectionDelegate(OnCloseConnection);
            this.ipcServer.OnMessageReceived += new IpcXmlServer.OnMessageReceivedDelegate(OnMessageReceived);
		}

        #region Startup/Refresh/Shutdown

        protected override void OnStartup()
        {
            this.routerQ = MessageQueueFactory.GetQueueWriter(IConfig.CoreComponentNames.ROUTER);
            this.appManagerQ = MessageQueueFactory.GetQueueWriter(IConfig.CoreComponentNames.APP_MANAGER);
            this.mediaManagerQ = MessageQueueFactory.GetQueueWriter(IConfig.CoreComponentNames.MEDIA_MANAGER);
            this.telManQ = MessageQueueFactory.GetQueueWriter(IConfig.CoreComponentNames.TEL_MANAGER);
            this.appServerQ = MessageQueueFactory.GetQueueWriter(IConfig.CoreComponentNames.APP_SERVER);
            this.clusterQ = MessageQueueFactory.GetQueueWriter(IConfig.CoreComponentNames.CLUSTER_INTERFACE);

            ipcServer.Start();

            log.Write(TraceLevel.Info, "Management server started on port " + ipcServer.ListenPort);
        }

        protected override void RefreshConfiguration(string proxy)
        {
            ipcServer.LogLevel = Config.Management.LogLevel;
        }

        protected override TraceLevel GetLogLevel()
        {
            return Config.Management.LogLevel;
        }

        protected override void OnShutdown()
        {
            this.ipcServer.Stop();
            this.authTable.Clear();
        }
        #endregion

        #region Handler for messages received from within AppServer

        protected override bool HandleMessage(InternalMessage message)
        {
            ResponseMessage response = message as ResponseMessage;
            if(response != null)
            {
                try 
                { 
                    int socketId = Convert.ToInt32(message[ICommands.Fields.TRANS_ID]);
                    string failureMessage = message.RemoveField(ICommands.Fields.FAIL_REASON) as string;

                    if (response.MessageId == IApp.VALUE_SUCCESS ||
                        response.MessageId == IResponses.REFRESH_COMPLETE)
                    {
                        SendSuccessResponse(socketId, message);
                    }
                    else if(response.MessageId == IApp.VALUE_FAILURE)
                    {
                        SendFailureResponse(socketId, IConfig.Result.ServerError, failureMessage);
                    }
                    else
                    {
                        IConfig.Result result = (IConfig.Result) Enum.Parse(typeof(IConfig.Result), message.MessageId, true);
                        SendResponse(socketId, result, failureMessage, null, null);
                    }
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Unable to process component response:\n" + message + "\nError: " + e.Message);
                }

                return true;
            }

            return false;
        }
        #endregion

        #region Handlers for external connection events

        private void OnNewConnection(int socketId, string remoteHost)
        {
            log.Write(TraceLevel.Info, "Management client connected (From: {0}, ID: {1})", 
                remoteHost, socketId.ToString());

            // Rip off port portion of address
            int i = remoteHost.IndexOf(":");
            if(i != -1)
                remoteHost = remoteHost.Substring(0, i);

            // Auto-authenticate localhost connections
            if(remoteHost == IPAddress.Loopback.ToString())
                Authenticate(socketId, remoteHost);
        }

        private void OnCloseConnection(int socketId)
        {
            log.Write(TraceLevel.Info, "Management client disconnected (ID: {0})", socketId.ToString());
            Deauthenticate(socketId);
        }

        private void OnMessageReceived(int socketId, string remoteHost, string dataStr)
        {
            commandType command = null;

            try
            {
                StringReader reader = new StringReader(dataStr);
                command = (commandType) commandSerializer.Deserialize(reader);
            }
            catch
            {
                log.Write(TraceLevel.Warning, "Received invalid command:\n" + dataStr);
                return;
            }

            IManagement.Commands commandName;
            try { commandName = (IManagement.Commands) Enum.Parse(typeof(IManagement.Commands), command.name, true); }
            catch
            {
                log.Write(TraceLevel.Error, "Received unknown command: " + command.name);
                return;
            }

            if(log.LogLevel == TraceLevel.Info)
                log.Write(TraceLevel.Info, "Got management command: " + command.name);
            else if(log.LogLevel == TraceLevel.Verbose)
                log.Write(TraceLevel.Verbose, "Got management command:\r\n" + dataStr);

            // Enforce authentication
            if(commandName == IManagement.Commands.LogIn)
            {
                HandleLogin(socketId, remoteHost, 
                    command[IManagement.ParameterNames.USERNAME] as string,
                    command[IManagement.ParameterNames.PASSWORD] as string);
                return;
            }
            else if(!IsAuthenticated(socketId, remoteHost))
            {
                log.Write(TraceLevel.Warning, "Connection from '{0}' attempting to execute management command '{1}' without authorization",
                    remoteHost, commandName);
                SendFailureResponse(socketId, IConfig.Result.NotAuthorized, "Not authorized");
                return;
            }

            // Handle management commands
            switch(commandName)
            {
                case IManagement.Commands.AddMediaServer:
                    AddMediaServer(socketId);
                    break;
                case IManagement.Commands.DisableApplication:
                    DisableApplication(socketId, command[IManagement.ParameterNames.NAME] as string);
                    break;
                case IManagement.Commands.DisableProvider:
                    DisableProvider(socketId, command[IManagement.ParameterNames.NAME] as string);
                    break;
                case IManagement.Commands.EnableApplication:
                    EnableApplication(socketId, command[IManagement.ParameterNames.NAME] as string);
                    break;
                case IManagement.Commands.EnableProvider:
                    EnableProvider(socketId, command[IManagement.ParameterNames.NAME] as string);
                    break;
                case IManagement.Commands.GetApps:
                    GetApps(socketId);
                    break;
                case IManagement.Commands.InstallApplication:
                    InstallApplication(socketId, command[IManagement.ParameterNames.NAME] as string, 
                        command[IManagement.ParameterNames.APP_NAME]);
                    break;
                case IManagement.Commands.UpdateApplication:
                    UpdateApplication(socketId, command[IManagement.ParameterNames.NAME] as string, 
                        command[IManagement.ParameterNames.APP_NAME]);
                    break;
                case IManagement.Commands.InvokeExtension:
                    string extName = command[IManagement.ParameterNames.EXT_NAME] as string;
                    InvokeExtension(socketId, extName, command.GetParameters());
                    break;
                case IManagement.Commands.RefreshConfiguration:
                    HandleRefreshConfig(socketId, command[IManagement.ParameterNames.TYPE] as string,
                        command[IManagement.ParameterNames.NAME] as string, 
                        command[IManagement.ParameterNames.APP_NAME] as string);
                    break;
                case IManagement.Commands.RemoveMediaServer:
                    HandleRefreshConfig(socketId, IConfig.ComponentType.Provider, 
                        IConfig.StandardProviders.MediaControlProvider.NAME, null);
                    break;
                case IManagement.Commands.UninstallApplication:
                    UninstallApplication(socketId, command[IManagement.ParameterNames.NAME] as string);
                    break;
                case IManagement.Commands.InstallProvider:
                    InstallProvider(socketId, command[IManagement.ParameterNames.NAME] as string);
                    break;
                case IManagement.Commands.UninstallProvider:
                    UninstallProvider(socketId, command[IManagement.ParameterNames.NAME] as string);
                    break;
                case IManagement.Commands.ReloadProvider:
                    ReloadProvider(socketId, command[IManagement.ParameterNames.NAME] as string);
                    break;
                case IManagement.Commands.GetProvisioningStatus:
                    GetProvisioningStatus(socketId, command[IManagement.ParameterNames.APP_NAME] as string);
                    break;
                case IManagement.Commands.EnableApplicationInstallation:
                    EnableApplicationInstallation(socketId);
                    break;
                case IManagement.Commands.DisableApplicationInstallation:
                    DisableApplicationInstallation(socketId);
                    break;
                case IManagement.Commands.ClearCallTable:
                    ClearCallTable(socketId);
                    break;
                case IManagement.Commands.PrintDiags:
                    PrintDiags(socketId, command[IManagement.ParameterNames.NAME] as string);
                    break;
                case IManagement.Commands.EndAllCalls:
                    EndAllCalls(socketId);
                    break;
                case IManagement.Commands.ClearCrgCache:
                    ClearCrgCache(socketId);
                    break;
                case IManagement.Commands.GarbageCollect:
                    GarbageCollect(socketId);
                    break;
            }
        }
        #endregion

        #region Authentication methods

        private void Authenticate(int socketId, string remoteHost)
        {
            AuthInfo aInfo = new AuthInfo(remoteHost);
            aInfo.Authenticated = true;
            authTable[socketId] = aInfo;
        }

        private void Deauthenticate(int socketId)
        {
            authTable.Remove(socketId);
        }

        private bool IsAuthenticated(int socketId, string remoteHost)
        {
            AuthInfo aInfo = authTable[socketId] as AuthInfo;
            if (aInfo != null && 
                aInfo.RemoteHost == remoteHost && 
                aInfo.Authenticated == true)
                return true;

            return false;
        }

        private void HandleLogin(int socketId, string remoteHost, string username, string password)
        {
            IConfig.AccessLevel access = Config.Instance.ValidateUser(username, password, null);

            if(access == IConfig.AccessLevel.Unspecified || access == IConfig.AccessLevel.Restricted)
            {
                SendFailureResponse(socketId, IConfig.Result.NotAuthorized, "Invalid username or password");

                // Trigger alarm
                statsClient.TriggerAlarm(IConfig.Severity.Yellow, IStats.AlarmCodes.AppServer.MgmtLoginFailure,
                    IStats.AlarmCodes.AppServer.Descriptions.MgmtLoginFailure, username, remoteHost);
            }
            else
            {
                SendSuccessResponse(socketId);
                Authenticate(socketId, remoteHost);

                // Trigger alarm
                statsClient.TriggerAlarm(IConfig.Severity.Yellow, IStats.AlarmCodes.AppServer.MgmtLoginSuccess,
                    IStats.AlarmCodes.AppServer.Descriptions.MgmtLoginSuccess, username, remoteHost);
            }
        }
        #endregion

        #region Management command handlers

        private void AddMediaServer(int socketId)
        {
            HandleRefreshConfig(socketId, IConfig.ComponentType.Provider, IConfig.StandardProviders.MediaControlProvider.NAME, null);
            HandleRefreshConfig(socketId, IConfig.ComponentType.Core, IConfig.CoreComponentNames.MEDIA_MANAGER, null);
        }

        private void DisableApplication(int socketId, string appName)
        {
            CommandMessage iCommand = base.CreateCommandMessage(IConfig.CoreComponentNames.ROUTER, ICommands.DISABLE_APP);
            iCommand.AddField(ICommands.Fields.APP_NAME, appName);
            iCommand.AddField(ICommands.Fields.TRANS_ID, socketId);
            routerQ.PostMessage(iCommand);
        }

        private void DisableProvider(int socketId, string providerName)
        {
            CommandMessage iCommand = base.CreateCommandMessage(IConfig.CoreComponentNames.PROV_MANAGER,
                ICommands.DISABLE_PROVIDER);
            iCommand.AddField(ICommands.Fields.PROVIDER_NAME, providerName);
            iCommand.AddField(ICommands.Fields.TRANS_ID, socketId);
            routerQ.PostMessage(iCommand);
        }

        private void EnableApplication(int socketId, string appName)
        {
            CommandMessage iCommand = base.CreateCommandMessage(IConfig.CoreComponentNames.ROUTER,
                ICommands.ENABLE_APP);
            iCommand.AddField(ICommands.Fields.APP_NAME, appName);
            iCommand.AddField(ICommands.Fields.TRANS_ID, socketId);
            routerQ.PostMessage(iCommand);
        }

        private void EnableProvider(int socketId, string providerName)
        {
            CommandMessage iCommand = base.CreateCommandMessage(IConfig.CoreComponentNames.PROV_MANAGER,
                ICommands.ENABLE_PROVIDER);
            iCommand.AddField(ICommands.Fields.PROVIDER_NAME, providerName);
            iCommand.AddField(ICommands.Fields.TRANS_ID, socketId);
            routerQ.PostMessage(iCommand);
        }

        private void InstallApplication(int socketId, string appFilename, string appName)
        {
            if(appFilename == null)
            {
                SendFailureResponse(socketId, IConfig.Result.ArgumentError, "Missing application data");
            }

            CommandMessage msg = messageUtility.CreateCommandMessage(IConfig.CoreComponentNames.APP_MANAGER, 
                ICommands.INSTALL_APP);
            msg.AddField(ICommands.Fields.FILENAME, appFilename);
            msg.AddField(ICommands.Fields.APP_NAME, appName);
            msg.AddField(ICommands.Fields.TRANS_ID, socketId);
            appManagerQ.PostMessage(msg);     
        }

        private void UpdateApplication(int socketId, string appFilename, string appName)
        {
            if(appFilename == null)
            {
                SendFailureResponse(socketId, IConfig.Result.ArgumentError, "Missing application data");
            }

            CommandMessage msg = messageUtility.CreateCommandMessage(IConfig.CoreComponentNames.APP_MANAGER, 
                ICommands.UPDATE_APP);
            msg.AddField(ICommands.Fields.FILENAME, appFilename);
            msg.AddField(ICommands.Fields.APP_NAME, appName);
            msg.AddField(ICommands.Fields.TRANS_ID, socketId);
            appManagerQ.PostMessage(msg);     
        }

        private void UninstallApplication(int socketId, string appName)
        {
            CommandMessage iCommand = base.CreateCommandMessage(IConfig.CoreComponentNames.APP_MANAGER, 
                ICommands.UNINSTALL_APP);
            iCommand.AddField(ICommands.Fields.APP_NAME, appName);
            iCommand.AddField(ICommands.Fields.TRANS_ID, socketId);
            appManagerQ.PostMessage(iCommand);
        }

        private void InstallProvider(int socketId, string providerName)
        {
            CommandMessage iCommand = base.CreateCommandMessage(IConfig.CoreComponentNames.PROV_MANAGER, 
                ICommands.INSTALL_PROVIDER);
            iCommand.AddField(ICommands.Fields.PROVIDER_NAME, providerName);
            iCommand.AddField(ICommands.Fields.TRANS_ID, socketId);
            routerQ.PostMessage(iCommand);
        }

        private void UninstallProvider(int socketId, string providerName)
        {
            CommandMessage iCommand = base.CreateCommandMessage(IConfig.CoreComponentNames.PROV_MANAGER, 
                ICommands.UNINSTALL_PROVIDER);
            iCommand.AddField(ICommands.Fields.PROVIDER_NAME, providerName);
            iCommand.AddField(ICommands.Fields.TRANS_ID, socketId);
            routerQ.PostMessage(iCommand);
        }

        private void ReloadProvider(int socketId, string providerName)
        {
            CommandMessage iCommand = base.CreateCommandMessage(IConfig.CoreComponentNames.PROV_MANAGER, 
                ICommands.RELOAD_PROVIDER);
            iCommand.AddField(ICommands.Fields.PROVIDER_NAME, providerName);
            iCommand.AddField(ICommands.Fields.TRANS_ID, socketId);
            routerQ.PostMessage(iCommand);
        }

        private void InvokeExtension(int socketId, string extName, StringDictionary parameters)
        {
            ActionMessage action = messageUtility.CreateActionMessage(extName, 
                "Provider error: A provider must not send a response in an extension handler.");
            action.SourceType = IConfig.ComponentType.Application; // Masquerade as an application
                    
            foreach(DictionaryEntry de in parameters)
            {
                action.AddField(de.Key as string, de.Value as string);
            }

            routerQ.PostMessage(action);
            SendSuccessResponse(socketId);
        }

        private void GetApps(int socketId)
        {
            ComponentInfo[] appInfos = configUtility.GetComponents(IConfig.ComponentType.Application);
            SendResponse(socketId, IConfig.Result.Success, null, appInfos, null);
        }

        private void GetProvisioningStatus(int socketId, string appName)
        {
            CommandMessage iCommand = base.CreateCommandMessage(IConfig.CoreComponentNames.MEDIA_MANAGER,
                IMediaManager.Commands.GetStatus);
            iCommand.AddField(IMediaManager.Fields.AppName, appName);
            iCommand.AddField(ICommands.Fields.TRANS_ID, socketId);
            mediaManagerQ.PostMessage(iCommand);
        }

        private void EnableApplicationInstallation(int socketId)
        {
            CommandMessage iCommand = base.CreateCommandMessage(IConfig.CoreComponentNames.APP_MANAGER,
                ICommands.ENABLE_APP_INSTALL);
            appManagerQ.PostMessage(iCommand);
            SendSuccessResponse(socketId);  // This command cannot fail
        }

        private void DisableApplicationInstallation(int socketId)
        {
            CommandMessage iCommand = base.CreateCommandMessage(IConfig.CoreComponentNames.APP_MANAGER,
                ICommands.DISABLE_APP_INSTALL);
            appManagerQ.PostMessage(iCommand);
            SendSuccessResponse(socketId);
        }

        private void ClearCallTable(int socketId)
        {
            CommandMessage iCommand = base.CreateCommandMessage(IConfig.CoreComponentNames.TEL_MANAGER,
                ICommands.CLEAR_CALL_TABLE);
            telManQ.PostMessage(iCommand);
            SendSuccessResponse(socketId);
        }

        private void PrintDiags(int socketId, string componentName)
        {
            CommandMessage iCommand = base.CreateCommandMessage(componentName,
                ICommands.PRINT_DIAGS);

            switch(componentName)
            {
                case IConfig.CoreComponentNames.ARE:
                case IConfig.CoreComponentNames.APP_MANAGER:
                    appManagerQ.PostMessage(iCommand);
                    break;
                case IConfig.CoreComponentNames.ROUTER:
                case IConfig.CoreComponentNames.PROV_MANAGER:
                    routerQ.PostMessage(iCommand);
                    break;
                case IConfig.CoreComponentNames.CLUSTER_INTERFACE:
                    clusterQ.PostMessage(iCommand);
                    break;
                default:
                    iCommand.Destination = IConfig.CoreComponentNames.TEL_MANAGER;
                    telManQ.PostMessage(iCommand);
                    break;
            }

            SendSuccessResponse(socketId);
        }

        private void EndAllCalls(int socketId)
        {
            CommandMessage iCommand = base.CreateCommandMessage(IConfig.CoreComponentNames.TEL_MANAGER,
                ICommands.END_ALL_CALLS);
            telManQ.PostMessage(iCommand);
            SendSuccessResponse(socketId);
        }

        private void ClearCrgCache(int socketId)
        {
            CommandMessage iCommand = base.CreateCommandMessage(IConfig.CoreComponentNames.TEL_MANAGER,
                ICommands.CLEAR_CRG_CACHE);
            telManQ.PostMessage(iCommand);
            SendSuccessResponse(socketId);
        }

        private void GarbageCollect(int socketId)
        {
            log.Write(TraceLevel.Warning, "Garbage collector invoked manually");
            GC.Collect();
            SendSuccessResponse(socketId);
        }

        private void HandleRefreshConfig(int socketId, string componentType, string componentName, string appName)
        {
            if((componentName == null) || (componentType == null))
            {
                SendFailureResponse(socketId, IConfig.Result.ArgumentError, "Missing component name or type");
                return;
            }

            IConfig.ComponentType cType;
            try { cType = (IConfig.ComponentType)Enum.Parse(typeof(IConfig.ComponentType), componentType, true); }
            catch 
            { 
                SendFailureResponse(socketId, IConfig.Result.ArgumentError, "Invalid component type: " + componentType); 
                return;
            }

            HandleRefreshConfig(socketId, cType, componentName, appName);
        }

        private void HandleRefreshConfig(int socketId, IConfig.ComponentType cType, string componentName, string appName)
        {
            if(componentName == null)
            {
                SendFailureResponse(socketId, IConfig.Result.ArgumentError, "Missing component name");
                return;
            }

            CommandMessage iCommand = base.CreateCommandMessage(componentName, ICommands.REFRESH_CONFIG);
            iCommand.AddField(ICommands.Fields.TRANS_ID, socketId);

            if(cType == IConfig.ComponentType.Core)
            {
                switch(componentName)
                {
                    case IConfig.CoreComponentNames.ARE:
                    case IConfig.CoreComponentNames.APP_MANAGER:                
                        appManagerQ.PostMessage(iCommand);
                        break;
                    case IConfig.CoreComponentNames.MEDIA_MANAGER:
                        mediaManagerQ.PostMessage(iCommand);
                        break;
                    case IConfig.CoreComponentNames.APP_SERVER:
                    case IConfig.CoreComponentNames.LOGGER:
                        appServerQ.PostMessage(iCommand);
                        break;
                    case IConfig.CoreComponentNames.LICENSE_MANAGER:
                        iCommand.Destination = ICommands.Fields.LICENSE_MANAGER;
                        routerQ.PostMessage(iCommand);
                        break;
                    case IConfig.CoreComponentNames.PROV_MANAGER:
                    case IConfig.CoreComponentNames.ROUTER:
                        // Tunnel the application name through the RefreshConfig proxy parameter
                        if(appName != null)
                            iCommand.Destination = ICommands.Fields.APP_NAME + ":" + appName;
                        routerQ.PostMessage(iCommand);
                        break;
                    case IConfig.CoreComponentNames.TEL_MANAGER:
                        this.telManQ.PostMessage(iCommand);
                        break;
                    case IConfig.CoreComponentNames.CLUSTER_INTERFACE:
                        this.clusterQ.PostMessage(iCommand);
                        break;
                    case IConfig.CoreComponentNames.MANAGEMENT:
                        this.PostMessage(iCommand);
                        break;
                    default:
                        SendFailureResponse(socketId, IConfig.Result.ArgumentError, "Invalid core component: " + componentName);
                        break;
                }
            }
            else if(cType == IConfig.ComponentType.Provider)
            {
                routerQ.PostMessage(iCommand);
            }
            else if(cType == IConfig.ComponentType.Application)
            {
                appManagerQ.PostMessage(iCommand);
            }
            else
            {
                SendFailureResponse(socketId, IConfig.Result.ArgumentError, "Invalid component type: " + cType);
                return;
            }
        }
        #endregion

        #region External response senders

        private void SendSuccessResponse(int socketId)
        {
            SendSuccessResponse(socketId, null);
        }

        private void SendSuccessResponse(int socketId, InternalMessage msg)
        {
            StringCollection resultList = null;

            if(msg != null)
            {
                resultList = new StringCollection();
                foreach(Field field in msg.Fields)
                {
                    resultList.Add(String.Format("{0}: {1}", field.Name, field.Value));
                }
            }
            SendResponse(socketId, IConfig.Result.Success, null, null, resultList);
        }

        private void SendFailureResponse(int socketId, IConfig.Result failureType, string failureMessage)
        {
            SendResponse(socketId, failureType, failureMessage, null, null);
        }

        private void SendResponse(int socketId, IConfig.Result resultType, string failureMessage, 
            ComponentInfo[] cInfo, StringCollection resultList)
        {
            responseType response = new responseType();
            response.type = resultType;
            response.Value = failureMessage;
            response.componentInfo = cInfo;

            if(resultList != null && resultList.Count > 0)
            {
                response.resultList = new string[resultList.Count];
                resultList.CopyTo(response.resultList, 0);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            StringWriter writer = new StringWriter(sb);
            responseSerializer.Serialize(writer, response);
            
            if(ipcServer.Write(socketId, sb.ToString()) == false)
                log.Write(TraceLevel.Warning, "Failed to send response to management command. Socket is closed");
            else if(base.LogLevel == TraceLevel.Verbose)
                log.Write(TraceLevel.Verbose, "Sent response to client ({0}):\n{1}",
                    socketId, sb.ToString());
            else
                log.Write(TraceLevel.Info, "Sent '{0}' response", resultType.ToString());
        }
        #endregion
	}
}
