using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;

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

        public IProxyServer Server { get { return ftpServer; } }

        private IProxyServer ftpServer;
        private TcpChannel ftpChannel;
        private ProxyClient client;
        
        private CommonTypes.OutputLine outputLine;
        private Settings settings;

        /// <summary>Sets up the remoting channels and gets a reference
        ///  to the server object hosted by the Funct Test Provider</summary>
        public void Initialize(CommonTypes.OutputLine outputLine, Settings settings)
        {
            this.settings = settings;
            this.outputLine = outputLine;

            ListDictionary channelProperties = new ListDictionary();
            channelProperties.Add("port", 0);
            channelProperties.Add("name", "FunctionalTestRuntime");
            IDictionary props = new Hashtable(); 
            props["typeFilterLevel"] = "Full"; 

            ListDictionary serverChannelProperties = new ListDictionary();
            serverChannelProperties.Add("port", Constants.FTBServerPort);
            serverChannelProperties.Add("name", "FunctionalTestRuntimeServer");

            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

            ftpChannel = new TcpChannel(channelProperties,
                new BinaryClientFormatterSinkProvider(), serverProvider);
            
            if(ChannelServices.GetChannel(ftpChannel.ChannelName) == null )
            {
                try
                {
                    ChannelServices.RegisterChannel(ftpChannel);
                }                
                catch(Exception e)
                {
                    string errorMsg = "Unable to register client channel.  Full exception is: " + e.ToString();

                    outputLine(errorMsg);
                    
                    return;
                }
            }

            ftpServer = Activator.GetObject(typeof(IProxyServer), 
                Utilities.GetServerUri(settings.SamoaIP)) as IProxyServer;          
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

                outputLine(errorMsg);

                return false;
            }

            return true;
        }

        private bool RegisterClientViaRemoting(FunctionalTestBase test)
        {
            if (ftpServer == null)
            {
                return false;
            }
            else
            {   
                return RegisterClient(test);
            }
        }

        private bool RegisterClient(FunctionalTestBase test)
        {       
               
            client = new ProxyClient();
            
            try
            {
                test.ClientId = ftpServer.RegisterTest(client);
            }
            catch(Exception e)
            {
                string errorMsg = "Unable to register test with the server.  Full exception is: " + e.ToString();

                outputLine(errorMsg);

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
                ftpServer.RegisterSignals(test.ClientId, signalNames);
            }
            catch(Exception e)
            {
                string errorMsg = "Unable to register signals with Functional Test Provider via remoting.  Full exception is: " + e.ToString();
                outputLine(errorMsg);

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
            catch
            {
            }

            try
            {
                ProxyClient.RequestForLog -= test.FulfillLogEvent;
            }
            catch
            {
            }

            try
            {
                ftpServer.UnregisterTest(test.ClientId);
            }
            catch
            {
            }
        }

        public void Cleanup()
        {
            ChannelServices.UnregisterChannel(ftpChannel);
        }
	}
}
