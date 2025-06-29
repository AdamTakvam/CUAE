using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Remoting;

using Channels=System.Runtime.Remoting.Channels;
using HttpChannels=System.Runtime.Remoting.Channels.Http;
using TcpChannels=System.Runtime.Remoting.Channels.Tcp;
using FTF=Metreos.Samoa.FunctionalTestFramework;

using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.LoggingFramework;

namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>Handles the state of remoting with the Functional Test Provider.</summary>
	public class TestCommunicator
	{
        #region Singleton

        public static TestCommunicator Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new TestCommunicator();
                }

                return instance;
            }
        }

        private static TestCommunicator instance;

        private TestCommunicator()
	    {
    		
	    }

        #endregion

        public IProxyServer[] Servers { get { return ftpServers; } }
        private ProxyClient client;
        
        private LogWriter log;
        private Settings settings;

        private IProxyServer[] ftpServers; // IProxyServer
        private TcpChannels.TcpChannel[] ftpChannels;

        /// <summary>Sets up the remoting channels and gets a reference
        ///  to the server object hosted by the Funct Test Provider</summary>
        public void Initialize(LogWriter log, Settings settings)
        {
            this.settings = settings;
            this.log = log;

            Reconnect();
        }

        public bool Reconnect()
        {
            try
            {
                Cleanup();

                ArrayList ftpServersList = new ArrayList();
                ArrayList tcpChannelsList = new ArrayList();
         
                log.Write(TraceLevel.Verbose, "Connecting to {0} Application Servers", settings.AppServerIps.Length);

                for(int i = 0; i < settings.AppServerIps.Length; i++)
                {
                    log.Write(TraceLevel.Info, "Connecting to AppServer {0} via remoting", settings.AppServerIps[i]);

                    ListDictionary channelProperties = new ListDictionary();
                    channelProperties.Add("port", 0);
                    channelProperties.Add("name", "FunctionalTestRuntime" + i);
                    IDictionary props = new Hashtable(); 
                    props["typeFilterLevel"] = "Full"; 

                    ListDictionary serverChannelProperties = new ListDictionary();
                    serverChannelProperties.Add("port", Constants.FTBServerPort);
                    serverChannelProperties.Add("name", "FunctionalTestRuntimeServer" + i);

                    Channels.BinaryServerFormatterSinkProvider serverProvider = 
                        new Channels.BinaryServerFormatterSinkProvider();
                    serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

                    TcpChannels.TcpChannel ftpChannel = new TcpChannels.TcpChannel(channelProperties,
                        new Channels.BinaryClientFormatterSinkProvider(), serverProvider);
            
                    if(Channels.ChannelServices.GetChannel(ftpChannel.ChannelName) == null )
                    {
                        try
                        {
                            Channels.ChannelServices.RegisterChannel(ftpChannel, false);
                        }                
                        catch(Exception e)
                        {
                            string errorMsg = "Unable to register client channel.  Full exception is: " + e.ToString();

                            log.Write(TraceLevel.Error, errorMsg);
                    
                            return false;
                        }
                    }

                    tcpChannelsList.Add(ftpChannel);

                    IProxyServer server = Activator.GetObject(typeof(IProxyServer), 
                        Utilities.GetServerUri(settings.AppServerIps[i])) as IProxyServer;
                    server.ServerName = settings.AppServerIps[i];
                    ftpServersList.Add(server);          

                    log.Write(TraceLevel.Info, "Connected to AppServer {0} via remoting", settings.AppServerIps[i]);
                }

                ftpChannels = new TcpChannels.TcpChannel[tcpChannelsList.Count];
                ftpServers = new IProxyServer[ftpServersList.Count];

                tcpChannelsList.CopyTo(ftpChannels);
            
                int count = 0; 
                foreach(IProxyServer server in ftpServersList)
                {
                    ftpServers[count++] = server; 
                }

                return true;
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Failure encountered in connect-via-remoting method.  {0}", Exceptions.FormatException(e));
                return false;
            }
        }

        /// <summary>Registers an individual test with the server object hosted by the Functional Test Provider</summary>
        public bool RegisterTest(FunctionalTestBase test)
        {
            if(!HookUpClientEvents(test))       
            {
                return false;
            }

            if(!RegisterClientViaRemoting(test))
            {
                return false;
            }

            if(!RegisterSignals(test))
            {
                return false;
            }

            return true;
        }

        private bool HookUpClientEvents(FunctionalTestBase test)
        {
            try
            {
                ProxyClient.ReceivedSignal += test.ReceivedSignalEvent;
                ProxyClient.RequestForLog +=  test.FulfillLogEvent;
            }
            catch(Exception e)
            {
                string errorMsg = "Unable to attach to static broadcast event.  Full exception is: " + e.ToString();

                log.Write(TraceLevel.Error, errorMsg);

                return false;
            }

            return true;
        }

        private bool RegisterClientViaRemoting(FunctionalTestBase test)
        {
            if (ftpServers == null || ftpServers.Length == 0)
            {
                return false;
            }
            else
            {   
                bool success = true;
                int i = 0; // Order is used to index servers
                foreach(IProxyServer server in ftpServers)
                {
                    bool individualSuccess = RegisterClient(i, server, test);
                    
                    if(individualSuccess)
                    {
                        log.Write(TraceLevel.Info, "Registered the FTF client to server {0}", server.ServerName);
                    }
                    else
                    {
                        log.Write(TraceLevel.Info, "Unable to register the FTF client to server {0}", server.ServerName);
                    }

                    success &= individualSuccess;

                    i++;
                }
                return success;
            }
        }

        private bool RegisterClient(int serverId, IProxyServer server, FunctionalTestBase test)
        {
            client = new ProxyClient();
            client.ServerId = serverId;

            try
            {
                test.ClientIds[server.ServerName] = server.RegisterTest(client);
            }
            catch(Exception e)
            {
                string errorMsg = "Unable to register test with the server.  Full exception is: " + e.ToString();

                log.Write(TraceLevel.Error, errorMsg);

                return false;
            }

            return true;
        }

        /// <summary>Registers the signals for a test with the server object hosted by the Functional Test Provider</summary>
        private bool RegisterSignals(FunctionalTestBase test)
        {
            CallbackLink[] signals = test.GetCallbacks();

            if(signals == null)             return true;
            if(signals.Length == 0)         return true;

            foreach(CallbackLink callback in signals)
            {
                System.Diagnostics.Debug.Assert(callback.signal != null, "signalName can not be null");
            }

            string[] signalNames = new string[signals.Length];

            int i = 0;
            foreach(CallbackLink callback in signals)
            {
                test.SignalCallbacks[callback.signal] = callback.callback;
                signalNames[i++] = callback.signal;
            }

            try
            {
                foreach(IProxyServer ftpServer in ftpServers)
                {
                    int clientId;
                    if(test.ClientIds.Contains(ftpServer.ServerName))
                    {
                        clientId = (int) test.ClientIds[ftpServer.ServerName];
                        ftpServer.RegisterSignals(clientId, signalNames);
                    }
                    else
                    {
                        System.Diagnostics.Debug.Assert(true, "No client ID Found when registering signals.  Coding error.");
                    }
                }
            }
            catch(Exception e)
            {
                string errorMsg = "Unable to register signals with Functional Test Provider via remoting.  Full exception is: " + e.ToString();
                log.Write(TraceLevel.Error, errorMsg);

                return false;
            }

            return true;
        }

        /// <summary>Removes the knowledge of a test from the server object hosted by the Functional Test Provider</summary>
        public void UnregisterTest(FunctionalTestBase test)
        {
            try
            {
                ProxyClient.ReceivedSignal -= test.ReceivedSignalEvent;
            }
            catch {}

            try
            {
                ProxyClient.RequestForLog -= test.FulfillLogEvent;
            }
            catch {}

            try
            {
                foreach(IProxyServer ftpServer in ftpServers)
                {
                    ftpServer.UnregisterTest((int)test.ClientIds[ftpServer.ServerName]);
                }
            }
            catch {}
        }

        public bool CreateComponentGroup(string testname)
        {
            if(ftpServers == null) return false;

            bool success = true;
            foreach(IProxyServer ftpServer in ftpServers)
            {
                success &= ftpServer.CreateComponentGroup(testname);
            }
            return success;
        }

        public bool CreatePartition(string appName, string partitionName, 
            FunctionalTestFramework.Constants.CallRouteGroupTypes callRouteType, string mediaGroupName, 
            bool isEnabled)
        {
            CommandMessage message = new CommandMessage();
            message.AddField(FTF.Constants.appName, appName);
            message.AddField(FTF.Constants.partitionName, partitionName);
            message.AddField(FTF.Constants.callRouteType, callRouteType);
            message.AddField(FTF.Constants.mediaRouteType, mediaGroupName);
            message.AddField(FTF.Constants.enabled, isEnabled);

            if(ftpServers == null) return false;

            bool success = true;
            foreach(IProxyServer ftpServer in ftpServers)
            {
                ftpServer.CreatePartition(message);
            }
            return success;
        }

        public bool CreatePartitionConfig(string appName, string partitionName, 
            string configName, string newValue)
        {
            CommandMessage message = new CommandMessage();
            message.AddField(FTF.Constants.appName, appName);
            message.AddField(FTF.Constants.partitionName, partitionName);
            message.AddField(FTF.Constants.configName, configName);
            message.AddField(FTF.Constants.@value, newValue);

            if(ftpServers == null) return false;

            bool success = true;
            foreach(IProxyServer ftpServer in ftpServers)
            {
                ftpServer.CreatePartitionConfig(message);
            }
            return success;
        }


        public void Cleanup()
        {
            if(ftpChannels == null) return;

            foreach(TcpChannels.TcpChannel channel in ftpChannels)
            {
                try
                {
                    Channels.ChannelServices.UnregisterChannel(channel);
                }
                catch{}
            }

            ftpChannels = null;
        }
	}
}