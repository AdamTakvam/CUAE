using System;
using System.Collections;

using Metreos.MmsTester.Core;
using Metreos.MmsTester.Custom.Clients;
using Metreos.MmsTester.AdapterFramework;
using Metreos.MmsTester.AdapterManager;
using Metreos.Samoa.Core;
using Metreos.MmsTester.ClientFramework;
using Metreos.MmsTester.Interfaces;

namespace Metreos.MmsTester.ClientManager
{
	/// <summary>
	/// Manages the various clients, as well as providing a direct exposure to the conduit
	/// </summary>
	public class ClientRouter
	{
        private ClientProvider clientProvider;
        private Hashtable runningClients;
        private Conduit.Conduit conduit;
        private AdapterManager.AdapterManager adapterManager;

		public ClientRouter(AdapterManager.AdapterManager adapterManager, ClientProvider clientProvider, Conduit.Conduit conduit)
		{
            this.adapterManager = adapterManager;
            this.clientProvider = clientProvider;

		    runningClients = new Hashtable();	

            this.conduit = conduit;

            RegisterAllEvents();
		}

        /// <summary>
        /// System command message
        /// </summary>
        public InternalMessage StartClient(InternalMessage im)
        {
            string displayName;
            string guid;
            string mediaServerMachineName;
            string mediaServerQueueName;
            string mediaServerHandle;

            im.GetFieldByName(IMessaging.CLIENT_DISPLAY_NAME, out displayName);
            im.GetFieldByName(IMessaging.CLIENT_GUID, out guid);

            im.GetFieldByName(IMessaging.TEST_MACHINE_NAME, out mediaServerMachineName);
            im.GetFieldByName(IMessaging.TEST_QUEUE_NAME, out mediaServerQueueName);
            im.GetFieldByName(IMessaging.MEDIA_SERVER_HANDLE, out mediaServerHandle);
            
            ClientBase requestClient = null;

            switch(displayName)
            {
                case IClientTypes.AS_EMULATOR_DISPLAY_NAME:
                    requestClient = new ApplicationServerEmulator(mediaServerMachineName, mediaServerQueueName, mediaServerHandle, 32, conduit);
                    break;
            }
            
            runningClients[displayName + guid] = requestClient;


            InternalMessage success = new InternalMessage();
            success.MessageId = IMessaging.SUCCESS;

            return success;     
        }

        /// <summary>
        /// MediaServer command message
        /// </summary>
        /// <param name="im"></param>
        /// <param name="outsideSourceCallback"></param>
        /// <returns></returns>
        public InternalMessage RouteToClient(InternalMessage im)
        {
            string displayName;
            string guid;

            im.GetFieldByName(IMessaging.CLIENT_DISPLAY_NAME, out displayName);
            im.GetFieldByName(IMessaging.CLIENT_GUID, out guid);

            ClientBase client = (ClientBase) runningClients[displayName + guid];

            if(client.Send(im))
            {

                InternalMessage success = new InternalMessage();
                success.MessageId = IMessaging.SUCCESS;

                return success;
            }
            else
            {
                InternalMessage failure = new InternalMessage();
                failure.MessageId = IMessaging.FAILURE;

                return failure;
            }

        }

        public InternalMessage AssociateAdapter(InternalMessage im)
        {

            string clientDisplayName;
            string clientGuid;

            string adapterDisplayName;
            string adapterGuid;
           
            im.GetFieldByName(IMessaging.ADAPTER_DISPLAY_NAME, out adapterDisplayName);
            im.GetFieldByName(IMessaging.ADAPTER_GUID, out adapterGuid);

            im.GetFieldByName(IMessaging.CLIENT_DISPLAY_NAME, out clientDisplayName);
            im.GetFieldByName(IMessaging.CLIENT_GUID, out clientGuid);
            
            ClientBase client = (ClientBase) runningClients[clientDisplayName + clientGuid];

            AdapterBase adapter = (AdapterBase) adapterManager.runningAdapters[adapterDisplayName + adapterGuid];
            if(client.AssociateAdapter(adapter))
            {
                InternalMessage success = new InternalMessage();
                success.MessageId = IMessaging.SUCCESS;

                return success;
            }
            else
            {
                InternalMessage failure = new InternalMessage();
                failure.MessageId = IMessaging.FAILURE;
                
                return failure;
            }
        }

        public InternalMessage RequestMediaServerInfo(InternalMessage im)
        {
            string clientDisplayName;
            string clientGuid;

            im.GetFieldByName(IMessaging.CLIENT_DISPLAY_NAME, out clientDisplayName);
            im.GetFieldByName(IMessaging.CLIENT_GUID, out clientGuid);

            ClientBase client = (ClientBase) runningClients[clientDisplayName + clientGuid];

            return client.RequestMediaServerInfo(im);
        }

        public void RegisterAllEvents()
        {
            conduit.RegisterSystemEvents(new IConduit.ConduitDelegate( this.StartClient ), IMessaging.START_CLIENT);
            conduit.RegisterSystemEvents(new IConduit.ConduitDelegate( this.AssociateAdapter ), IMessaging.LINK_ADAPTER_TO_CLIENT);
            conduit.RegisterMediaServerEvents(new IConduit.ConduitDelegate( this.RouteToClient ), IMessaging.CONNECT_TO_MEDIASERVER);
            conduit.RegisterMediaServerEvents(new IConduit.ConduitDelegate( this.RouteToClient ), MmsProtocol.MSG_MS_CONNECT);
            conduit.RegisterMediaServerEvents(new IConduit.ConduitDelegate( this.RouteToClient ), MmsProtocol.MSG_MS_PLAY_ANN);
            conduit.RegisterMediaServerEvents(new IConduit.ConduitDelegate( this.RouteToClient ), MmsProtocol.MSG_MS_DISCONNECT);
            conduit.RegisterMediaServerEvents(new IConduit.ConduitDelegate( this.RouteToClient ), IMessaging.CONNECT_TO_CONFERENCE);
            conduit.RegisterMediaServerInfoRequests(new IConduit.ConduitDelegate( this.RequestMediaServerInfo ), IMessaging.REQUEST_MEDIA_SERVER_INFO);
        }
	}
}
