using System;
using System.Collections;
using System.Diagnostics;

using Metreos.Interfaces;
using Metreos.Messaging;

namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>
	/// Summary description for FunctionTestProxy.
	/// </summary>
	public class FunctionalTestProxy : MarshalByRefObject, IProxyServer
	{
        /// <summary>The client is attempting to trigger an application</summary>
        public static event CommandMessageDelegate TriggerAppFromClient;

        /// <summary>The client is sending an event to an appliction</summary>
        public static event CommandMessageDelegate SendEventFromClient;

        /// <summary>The client is responding to a FTF.Signal action in a script</summary>
        public static event CommandMessageDelegate SendResponseFromClient;

        /// <summary>The server proxy object is requesting to log</summary>
        public static event CommandMessageDelegate RequestForLog;

        public static event CommandMessageDelegate UpdateConfigRequest;

        public static event CommandMessageDelegate UpdateScriptParameterRequest; 

        public static event CommandMessageDelegate UpdateCallRouteGroupRequest;

        public static event CommandMessageDelegate UpdateMediaRouteGroupRequest;

        public static event CreateComponentDelegate RequestForCreateComponentGroup;

        public static event CommandMessageDelegate CreatePartitionRequest;

        public static event CommandMessageDelegate CreatePartitionConfigRequest;

        public event ShutdownDelegate Shutdown;

        /// <summary>Key = clientId, Value = ProxyClient</summary>
        private SortedList clients;
        private Hashtable  clientRegisteredSignals;
        private Hashtable  signalsOfClients;

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

        public string ServerName { get { return serverName; } set { serverName = value; } }
        private string serverName;

        #region ServerCommands

        public void SendSignalToTestBase(ActionMessage im)
        {
            string signalName = IActions.NoHandler;
            if(im.MessageId != IActions.NoHandler)
            {
                //Decide which test this belongs to based on the signal name.
                signalName = im[Constants.FIELD_SIGNAL_NAME] as string;
            
                if(signalName == null)
                {
                    CommandMessage log = Utilities.CreateLog(Constants.FIELD_SIGNAL_NAME + " field not present in signal message.", TraceLevel.Error);
                    Utilities.RequestForLogIterator(log, RequestForLog);
                    return;
                }
            }
            try
            {
                int clientId = (int) signalsOfClients[signalName];
                IProxyClient client = clients[clientId] as IProxyClient;
                // Slip in the serverId, important in case one FTF is controlling multiple FTP
                im.AddField(Constants.ServerId, client.ServerId);
                client.SignalReceived(im);

            }
            catch(NullReferenceException)
            {
                RequestForLog(Utilities.CreateLog("Signal " + signalName + " is not a registered signal.  Check the installer.", TraceLevel.Error));
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
            clients[++clientIdCounter] = client;

            return clientIdCounter;
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
            signalsOfClients.Clear();
            // Check that one of the new signals isn't already being used.
            lock(signalCheckLock)
            {
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
                    CommandMessage im = Utilities.CreateLog("Register occuring twice for app. Overwriting old messages", TraceLevel.Warning);
                    Utilities.RequestForLogIterator(im, RequestForLog);
                }

                clientRegisteredSignals[id] = signals;
            }

            return true;
        }

        public void UpdateConfigValue(CommandMessage im)
        {
            if(UpdateConfigRequest != null)
            {
                UpdateConfigRequest(im);
            }
            else
            {
                CommandMessage log = Utilities.CreateLog("Unable to update configuration. UpdateConfigRequest in server remoting object is not hooked up to the Functional Test Provider.", TraceLevel.Error);
                Utilities.RequestForLogIterator(log, RequestForLog);
            }
        }

        public void UpdateScriptParameter(CommandMessage im)
        {
            if(UpdateScriptParameterRequest != null)
            {
                UpdateScriptParameterRequest(im);
            }
            else
            {
                CommandMessage log = Utilities.CreateLog("Unable to update script paramater. UpdateScriptParameterRequest in server remoting object is not hooked up to the Functional Test Provider.", TraceLevel.Error);
                Utilities.RequestForLogIterator(log, RequestForLog);
            }
        }

        public void UpdateCallRouteGroup(CommandMessage im)
        {
            if(UpdateCallRouteGroupRequest != null)
            {
                UpdateCallRouteGroupRequest(im);
            }
            else
            {
                CommandMessage log = Utilities.CreateLog("Unable to update call route group. UpdateCallRouteGroup in server remoting object is not hooked up to the Functional Test Provider.", TraceLevel.Error);
                Utilities.RequestForLogIterator(log, RequestForLog);
            }
        }

        public void UpdateMediaRouteGroup(CommandMessage im)
        {
            if(UpdateMediaRouteGroupRequest != null)
            {
                UpdateMediaRouteGroupRequest(im);
            }
            else
            {
                CommandMessage log = Utilities.CreateLog("Unable to update media route group. UpdateMediaRouteGroup in server remoting object is not hooked up to the Functional Test Provider.", TraceLevel.Error);
                Utilities.RequestForLogIterator(log, RequestForLog);
            }
        } 

        public void CreatePartition(CommandMessage im)
        {
            if(CreatePartitionRequest != null)
            {
                CreatePartitionRequest(im);
            }
            else
            {
                CommandMessage log = Utilities.CreateLog("Unable to create partition. CreatePartitionRequest in server remoting object is not hooked up to the Functional Test Provider.", TraceLevel.Error);
                Utilities.RequestForLogIterator(log, RequestForLog);
            }
        }

        public void CreatePartitionConfig(CommandMessage im)
        {
            if(CreatePartitionConfigRequest != null)
            {
                CreatePartitionConfigRequest(im);
            }
            else
            {
                CommandMessage log = Utilities.CreateLog("Unable to create partition. CreatePartitionConfigRequest in server remoting object is not hooked up to the Functional Test Provider.", TraceLevel.Error);
                Utilities.RequestForLogIterator(log, RequestForLog);
            }
        }

        public void SendTrigger(CommandMessage im)
        {
            if(TriggerAppFromClient != null)
            {
                TriggerAppFromClient(im);
            }
            else
            {
                CommandMessage log = Utilities.CreateLog("Unable to send trigger. TriggerHandler in server remoting object is not hooked up to the Functional Test Provider.", TraceLevel.Error);
                Utilities.RequestForLogIterator(log, RequestForLog);
            }
        }
        
        public void SendEvent(CommandMessage im)
        {
            if(SendEventFromClient != null)
            {
                SendEventFromClient(im);
            }
            else
            {
                CommandMessage log = Utilities.CreateLog("Unable to send event.  " + 
                    "EventHandler in server remoting object is not hooked up to the Functional Test Provider.", TraceLevel.Error);
                
                Utilities.RequestForLogIterator(log, RequestForLog);
            }
        }

        public void SendResponse(CommandMessage im)
        {
            if(SendResponseFromClient != null)
            {
                SendResponseFromClient(im);
            }
            else
            {
                CommandMessage log = Utilities.CreateLog("Unable to send response.  ResponseHandler in server remoting object is not hooked up to the Functional Test Provider.", TraceLevel.Error);
                Utilities.RequestForLogIterator(log, RequestForLog);
            }
        }

        public bool CreateComponentGroup(string testname)
        {
            return RequestForCreateComponentGroup(testname);
        }

        #endregion
	}
}
