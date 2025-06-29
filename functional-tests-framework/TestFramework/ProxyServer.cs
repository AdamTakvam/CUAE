using System;
using System.Collections;
using System.Diagnostics;

using Metreos.Messaging;

namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>
	/// Summary description for FunctionTestProxy.
	/// </summary>
	public class FunctionalTestProxy : MarshalByRefObject, IProxyServer
	{
        /// <summary>The client is attempting to trigger an application</summary>
        public static event InternalMessageDelegate TriggerAppFromClient;

        /// <summary>The client is sending an event to an appliction</summary>
        public static event InternalMessageDelegate SendEventFromClient;

        /// <summary>The client is responding to a FTF.Signal action in a script</summary>
        public static event InternalMessageDelegate SendResponseFromClient;

        /// <summary>The server proxy object is requesting to log</summary>
        public static event InternalMessageDelegate RequestForLog;

        public event ShutdownDelegate Shutdown;

        /// <summary>Key = clientId, Value = ProxyClient</summary>
        private SortedList clients;
        private Hashtable clientRegisteredSignals;
        private Hashtable signalsOfClients;

        private static object registerSignalLock = new object();
        private static object signalCheckLock = new object();

        private static int clientIdCounter = 0;

		public FunctionalTestProxy()
		{     
            clients = new SortedList();

            clientRegisteredSignals = new Hashtable();

            signalsOfClients = new Hashtable();
		}

        public override object InitializeLifetimeService()
        {
            return null;
        }


        #region ServerCommands

        public void SendSignalToTestBase(InternalMessage im)
        {
            //Decide which test this belongs to based on the signal name.
            string signalName = im[Constants.FIELD_SIGNAL_NAME] as string;
            if(signalName == null)
            {
                InternalMessage log = Utilities.CreateLog(Constants.FIELD_SIGNAL_NAME + " field not present in signal message.", TraceLevel.Error);
                Utilities.RequestForLogIterator(log, RequestForLog);
                return;
            }

            try
            {
                int clientId = (int) signalsOfClients[signalName];
                IProxyClient client = clients[clientId] as IProxyClient;
                client.SignalReceived(im);

            }
            catch(NullReferenceException)
            {
                RequestForLog(Utilities.CreateLog("Unable to locate the signal " + signalName + "in the registered signals hash.  Check the installer.", TraceLevel.Error));
                return;
            }
            catch(Exception e)
            {
                RequestForLog(Utilities.CreateLog("The client signal " + signalName + " could not be processed. Full exception is: " + e.ToString(), TraceLevel.Error));
                return;
            }
        }

        /// <summary>Send shutdown to all clients</summary>
        public void SendShutdown()
        {
            if(Shutdown == null)
            {
                return;
            }
            
            ShutdownDelegate[] allClientShutdownHandlers = Shutdown.GetInvocationList() as ShutdownDelegate[];
            
            foreach(ShutdownDelegate shutdownDelegate in allClientShutdownHandlers)
            {
                try
                {
                    shutdownDelegate();
                }
                catch
                {

                }
            }
        }

        #endregion

        #region ClientCommands

        /// <summary>Called by the client to register the client with the server</summary>
        /// <returns>An ID from the server</returns>
        public int RegisterTest(ProxyClient client)
        {
            clients[clientIdCounter] = client;

            return clientIdCounter++;
        }

        public void UnregisterTest(int id)
        {
            clients.Remove(id);
            
            string[] signalsForClient = clientRegisteredSignals[id] as string[];

            clientRegisteredSignals.Remove(id);

            foreach(string signal in signalsForClient)
                signalsOfClients.Remove(signal);
        }

        public bool RegisterSignals(int id, string[] signals)
        {
            // Check that one of the new signals isn't already being used.
            lock(signalCheckLock)
            {
                foreach(string signal in signals)
                {
                    if(signalsOfClients.Contains(signal))
                    {
                        InternalMessage log = Utilities.CreateLog("Signal has already been registered. " +  
                            "Duplicate signals not allowed. Deregistering test.", TraceLevel.Warning);

                        Utilities.RequestForLogIterator(log, RequestForLog);
                        UnregisterTest(id);
                        return false;
                    }  
                } 

                // Signals are verified not duplicate
                foreach(string signal in signals)
                    signalsOfClients[signal] = id;
            }
            

            lock(registerSignalLock)
            {
                if(clientRegisteredSignals.Contains(id))
                {
                    // Strange. Why would a test need to register twice? Log warning
                    clientRegisteredSignals.Remove(id);
                    InternalMessage im = Utilities.CreateLog("Register occuring twice for app. Overwriting old messages", TraceLevel.Warning);
                    Utilities.RequestForLogIterator(im, RequestForLog);
                }

                clientRegisteredSignals[id] = signals;
            }

            return true;
        }

        public void SendTrigger(InternalMessage im)
        {
            if(TriggerAppFromClient != null)
            {
                TriggerAppFromClient(im);
            }
            else
            {
                InternalMessage log = Utilities.CreateLog("Unable to send trigger. TriggerHandler in server remoting object is not hooked up to the Functional Test Provider.", TraceLevel.Error);
                Utilities.RequestForLogIterator(log, RequestForLog);
            }
        }
        
        public void SendEvent(InternalMessage im)
        {
            if(SendEventFromClient != null)
            {
                SendEventFromClient(im);
            }
            else
            {
                InternalMessage log = Utilities.CreateLog("Unable to send event.  " + 
                    "EventHandler in server remoting object is not hooked up to the Functional Test Provider.", TraceLevel.Error);
                
                Utilities.RequestForLogIterator(log, RequestForLog);
            }
        }

        public void SendResponse(InternalMessage im)
        {
            if(SendResponseFromClient != null)
            {
                SendResponseFromClient(im);
            }
            else
            {
                InternalMessage log = Utilities.CreateLog("Unable to send response.  ResponseHandler in server remoting object is not hooked up to the Functional Test Provider.", TraceLevel.Error);
                Utilities.RequestForLogIterator(log, RequestForLog);
            }
        }

        #endregion
	}
}
