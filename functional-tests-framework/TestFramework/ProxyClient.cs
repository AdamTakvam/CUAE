using System;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

using Metreos.Messaging;

namespace Metreos.Samoa.FunctionalTestFramework
{
    public delegate void InternalMessageDelegate(InternalMessage im);
    public delegate void ClientEventDelegate(ClientEvents eventType);
    public delegate void ShutdownDelegate();

	/// <summary>
	/// Summary description for ProxyClient.
	/// </summary>
	public class ProxyClient : MarshalByRefObject, IProxyClient
	{   
        public static event InternalMessageDelegate ReceivedSignal;

        /// <summary>The client proxy object is requesting to log</summary>
        public static event InternalMessageDelegate RequestForLog;

		public ProxyClient()
		{
			
		}
        
        [OneWay]
        public void SignalReceived(InternalMessage im)
        {
            if(ReceivedSignal == null)
            {
                InternalMessage log = Utilities.CreateLog("Client remoting object received signal to process, but there are no tests suscribed to the signal.", TraceLevel.Warning);
                Utilities.RequestForLogIterator(log, RequestForLog);
                return;
            }

            System.Delegate[] clients = ReceivedSignal.GetInvocationList() as System.Delegate[];

            try
            {
                foreach(InternalMessageDelegate receivedSignal in clients)
                {
                    try
                    {
                        receivedSignal(im);
                    }
                    catch
                    {
                        InternalMessage log = Utilities.CreateLog("A test has disconnected ungracefully. Removing client from the ReceivedSignal broadcast event.", TraceLevel.Warning);
                        Utilities.RequestForLogIterator(log, RequestForLog);
                        ReceivedSignal -= receivedSignal;
                    }
                }    
            }
            catch
            {
                InternalMessage log = Utilities.CreateLog("Unable to fire a signal received event.  The Functional Test Base is not hooked up to th Sginal Received event.", TraceLevel.Error);
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
