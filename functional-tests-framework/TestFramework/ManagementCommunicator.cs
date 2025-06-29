using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

using Metreos.LoggingFramework;
using Metreos.AppArchiveCore;
using Metreos.Interfaces;
using Metreos.Utilities;
using Metreos.Core.IPC;
using Metreos.Core.IPC.Xml;
using Metreos.Core.IPC.Sftp;
using Metreos.Core.ConfigData;


namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>Handles the state of remoting with the Functional Test Provider.</summary>
	public class ManagementCommunicator
	{
        #region Singleton Impl

        public static ManagementCommunicator Instance { get { return instance; } }

        private static ManagementCommunicator instance = new ManagementCommunicator();

        private ManagementCommunicator() 
        {
            this.cmdBuilder = new StringBuilder();
            this.cmdWriter = new StringWriter(cmdBuilder);
        }

        #endregion

        private LogWriter log;
        private Settings settings;
        private responseType lastResponse;

        private StringBuilder cmdBuilder;
        private StringWriter cmdWriter;

        private XmlSerializer serializer = new XmlSerializer(typeof(commandType));
        private XmlSerializer deserializer = new XmlSerializer(typeof(responseType));
        private XmlDocument cdataMaker = new XmlDocument();
        private volatile bool connected;
        private volatile int connectCount;
        private object connectLock;
        private ArrayList clients;
        private Hashtable connectedMap;

        /// <summary>Sets up the remoting channels and gets a reference
        ///  to the server object hosted by the Funct Test Provider</summary>
        public void Initialize(LogWriter log, Settings settings)
        {
            this.log            = log;
            this.settings       = settings;
            this.connected      = false;    
            this.clients        = new ArrayList();
            this.connectCount   = 0;
            this.connectLock    = new object();
            this.connectedMap   = new Hashtable();
            
            Connect();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout">Seconds</param>
        /// <returns></returns>
        public bool Connect()
        {
            bool success = true;
            for(int i = 0; i < settings.AppServerIps.Length; i++)
            {
                string appServerIp = settings.AppServerIps[i];
                IpcXmlClient xmlClient = new IpcXmlClient();
                xmlClient.onConnect += new OnConnectDelegate(OnConnect);
                xmlClient.onXmlMessageReceived += new OnXmlMessageReceivedDelegate(OnMessageReceived);
                xmlClient.onClose += new OnCloseDelegate(OnClose);
				xmlClient.RemoteEp = new IPEndPoint(IPAddress.Parse(appServerIp), int.Parse(settings.SamoaPort));
				xmlClient.Start();

                success = false;
                clients.Add(xmlClient);
                connectedMap[xmlClient] = false;
            }

            // Start out the status of the ManagementClient right here,
            // if all appservers could be connected to
            // This connected status should stay in sync
            // as the OnClose/OnConnected events fire 
            return success;
        }

        public bool Reconnect()
        {
            lock(connectLock)
            {
                return connected;
            }
        }

        public bool DisableApplication(int serverId, string applicationName)
        {
            // Disable the application
            commandType disableCommand = new commandType();
            disableCommand.name = IManagement.Commands.DisableApplication.ToString();
            disableCommand.param = new paramType[1];

            paramType appName = new paramType();
            appName.name = IManagement.ParameterNames.NAME;
            appName.Value = applicationName;

            disableCommand.param[0] = appName;

            return SendCommand(serverId, disableCommand, IErrors.disableApp);
        }

        public bool UninstallApplication(int serverId, string applicationName)
        {
            // Uninstall the application
            commandType uninstallCommand = new commandType();
            uninstallCommand.name = IManagement.Commands.UninstallApplication.ToString();
            uninstallCommand.param = new paramType[1];

            paramType appName = new paramType();
            appName.name = IManagement.ParameterNames.NAME;
            appName.Value = applicationName;

            uninstallCommand.param[0] = appName;

            return SendCommand(serverId, uninstallCommand, IErrors.appWillNotUninstall);
        }

        public bool GetAllApplications(int serverId, out ComponentInfo[] allApplications)
        {
            allApplications = null;
            
            commandType getApps = new commandType();
            getApps.name = IManagement.Commands.GetApps.ToString(); 

            if(SendCommand(serverId, getApps, IErrors.getApps))
            {
                // This is not 100% thread-safe, but I think it'll slide in this case  ;)
                allApplications = lastResponse.componentInfo;
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Logins in to the Application Server    
        /// </summary>
        /// <param name="username">
        ///     Username
        /// </param>
        /// <param name="password">
        ///     Un-encrypted password
        /// </param>
        /// <returns>
        ///     Success if the login could be made, otherwise false
        /// </returns>
        public bool Login(int serverId, string username, string password)
        {    
            password = Metreos.Utilities.Security.EncryptPassword(password);

            commandType login = new Metreos.Core.IPC.Xml.commandType();
            login.name = IManagement.Commands.LogIn.ToString();
            login.param = new paramType[2];

            paramType usernameParam = new paramType();
            usernameParam.name = IManagement.ParameterNames.USERNAME;
            usernameParam.Value = username;

            paramType passwordParam = new paramType();
            passwordParam.name = IManagement.ParameterNames.PASSWORD;
            passwordParam.Value = password;

            login.param[0] = usernameParam;
            login.param[1] = passwordParam;

            return SendCommand(serverId, login, IErrors.loginFailure);
        }

        public bool RefreshApplicationConfiguration(string appName)
        {
            return RefreshApplicationConfiguration(0, appName);
        }

        public bool RefreshApplicationConfiguration(int serverId, string appName)
        {
            commandType refresh = new commandType();
            refresh.name = IManagement.Commands.RefreshConfiguration.ToString();
            refresh.param = new paramType[3];

            paramType type = new paramType();
            type.name = IManagement.ParameterNames.TYPE;
            type.Value = IConfig.ComponentType.Core.ToString();
            
            paramType name = new paramType();
            name.name = IManagement.ParameterNames.NAME;
            name.Value = IConfig.CoreComponentNames.ROUTER;

            paramType applicationName = new paramType();
            applicationName.name = IManagement.ParameterNames.APP_NAME;
            applicationName.Value = appName;

            refresh.param[0] = type;
            refresh.param[1] = name;
            refresh.param[2] = applicationName;

            bool success = SendCommand(serverId, refresh, IErrors.refreshFailure);

            Thread.Sleep(2000);

            return success;
        }

        public bool CiscoDeviceListXRefresh(int serverId, string appName)
        {
            commandType refresh = new commandType();
            refresh.name = IManagement.Commands.InvokeExtension.ToString();
            refresh.param = new paramType[1];

            paramType extensionNameParam = new paramType();
            extensionNameParam.name = IManagement.ParameterNames.EXT_NAME;
            extensionNameParam.Value = "Metreos.Providers.CiscoDeviceListX.Refresh";

            refresh.param[0] = extensionNameParam;

            return SendCommand(serverId, refresh, IErrors.refreshFailure);
        }

        private bool SendCommand(int serverId, object cmd, string failMsg)
        {
            lock(this)
            {
                try
                {
                    lastResponse = null;
                    cmdBuilder.Length = 0;

                    serializer.Serialize(cmdWriter, cmd);
                    (clients[serverId] as IpcXmlClient).Write(cmdBuilder.ToString());

                    Monitor.Wait(this, Constants.COMMAND_TIMEOUT);
                }
                catch
                {
                    log.Write(TraceLevel.Error, IErrors.inaccessible);
                    return false;
                }

                if(lastResponse != null && lastResponse.type != IConfig.Result.Success)
                {
                    log.Write(TraceLevel.Error, failMsg);
                    return false;
                }
            }

            return true;
        }

        public void Cleanup()
        {
            connected = false;
            foreach(IpcXmlClient client in clients)
            {
                try { client.Close(); }
                catch { }
            }

            clients.Clear();
        }

        private void OnConnectionClosed()
        {
            lock(connectLock)
            {
                connectCount--;
                connected = false;
            }
        }

        private void OnConnect(IpcClient client, bool reconnect)
        {
            lock(connectLock)
            {
                connectedMap[client] = true;
                this.connected = CheckConnectMap();
            }
        }

        private void OnMessageReceived(IpcXmlClient client, string message)
        {
            lock(this)
            {
                try
                {
                    StringReader sr = new StringReader(message);
                    lastResponse = (responseType) deserializer.Deserialize(sr);
                }
                catch
                {
                    lastResponse = null;
                }

                Monitor.Pulse(this);
            }
        }

        private void OnClose(IpcClient ipcClient, Exception e)
        {
            lock(connectLock)
            {
                connectedMap[ipcClient] = false;
                this.connected = CheckConnectMap();
            }
        }

        private bool CheckConnectMap()
        {
            bool allConnected = true;
            foreach(bool status in this.connectedMap.Values)
            {
                allConnected &= status;
            }
            return allConnected;
        }
    }
}
