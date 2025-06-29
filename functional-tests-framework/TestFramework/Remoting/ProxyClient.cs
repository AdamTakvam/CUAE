using System;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

using Metreos.Messaging;

namespace Metreos.Samoa.FunctionalTestFramework
{
    public delegate void ActionMessageDelegate(ActionMessage im);
    public delegate void CommandMessageDelegate(CommandMessage im);
    public delegate void ClientEventDelegate(ClientEvents eventType);
    public delegate bool CreateComponentDelegate(string testname);
    public delegate void ShutdownDelegate();

	/// <summary>
	/// Summary description for ProxyClient.
	/// </summary>
	public class ProxyClient : MarshalByRefObject, IProxyClient
	{   
        public static event ActionMessageDelegate ReceivedSignal;

        /// <summary>The client proxy object is requesting to log</summary>
        public static event CommandMessageDelegate RequestForLog;

        // One FTF can potentially control multiple FTP.  It is important then that the FTF is returned, 
        // on every signal, its own index so it knows which FTP is sending down a signal
        private int serverId;
        public int ServerId { get { return serverId; } set { serverId = value; } } 

		public ProxyClient()
		{
			
		}
        
        [OneWay]
        public void SignalReceived(ActionMessage im)
        {
            if(ReceivedSignal == null)
            {
                CommandMessage log = Utilities.CreateLog("Client remoting object received signal to process, but there are no tests suscribed to the signal.", TraceLevel.Warning);
                Utilities.RequestForLogIterator(log, RequestForLog);
                return;
            }

            System.Delegate[] clients = ReceivedSignal.GetInvocationList() as System.Delegate[];

            try
            {
                foreach(ActionMessageDelegate receivedSignal in clients)
                {
                    try
                    {
                        receivedSignal(im);
                    }
                    catch
                    {
                        CommandMessage log = Utilities.CreateLog("A test has disconnected ungracefully. Removing client from the ReceivedSignal broadcast event.", TraceLevel.Warning);
                        Utilities.RequestForLogIterator(log, RequestForLog);
                        ReceivedSignal -= receivedSignal;
                    }
                }    
            }
            catch
            {
                CommandMessage log = Utilities.CreateLog("Unable to fire a signal received event.  The Functional Test Base is not hooked up to th Sginal Received event.", TraceLevel.Error);
                Utilities.RequestForLogIterator(log, RequestForLog);
            }  
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

	}

    public enum ClientEvents
    {
        startTest,
        endTest,
        clientDown
    }
}
