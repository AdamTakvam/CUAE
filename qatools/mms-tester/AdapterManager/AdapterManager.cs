using System;
using System.Collections;

using Metreos.MmsTester.Custom.Adapters;
using Metreos.Samoa.Core;
using Metreos.MmsTester.AdapterFramework;
using Metreos.MmsTester.Interfaces;

namespace Metreos.MmsTester.AdapterManager
{
	/// <summary>
	/// Maintians the adapter(s) for a client
	/// </summary>
	public class AdapterManager
	{
        private AdapterProvider adapterProvider;
        public Hashtable runningAdapters;
        private Conduit.Conduit conduit;
        
        // Written adapters

		public AdapterManager(AdapterProvider adapterProvider, Conduit.Conduit conduit)
		{
            this.adapterProvider = adapterProvider;
			runningAdapters = new Hashtable();

            //lookupAdapter = new Hashtable();
            //lookupAdapter[IAdapterTypes.MMS_MQ_ADAPTER_DISPLAY_NAME] = IAdapterTypes.MMS_MQ_ADAPTER;

            this.conduit = conduit;

            RegisterAllEvents();
		}

        public InternalMessage RequestAdapter(InternalMessage im)
        {
            string displayName;
            string guid;

            im.GetFieldByName(IMessaging.ADAPTER_DISPLAY_NAME, out displayName);
            im.GetFieldByName(IMessaging.ADAPTER_GUID, out guid);
            
            AdapterBase adapter = null;

            switch (displayName)
            {
                case IAdapterTypes.MMS_MQ_ADAPTER_DISPLAY_NAME:
                    adapter = new MmsMqAdapter(displayName + guid, "verbose");
                    break;
            }
            
            runningAdapters[displayName + guid] = adapter;

            InternalMessage success = new InternalMessage();
            success.MessageId = IMessaging.SUCCESS;
            return success;
        }

        public InternalMessage StartAdapter(InternalMessage im)
        {
            string displayName;
            string guid;
            string mediaServerHandle;

            im.GetFieldByName(IMessaging.ADAPTER_DISPLAY_NAME, out displayName);
            im.GetFieldByName(IMessaging.ADAPTER_GUID, out guid);
            im.GetFieldByName(IMessaging.MEDIA_SERVER_HANDLE, out mediaServerHandle);

            string key = displayName + guid;

            AdapterBase startThisAdapter = (AdapterBase) runningAdapters[key];

            if(startThisAdapter.Initialize(mediaServerHandle))
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

        public void StopAdapter(InternalMessage im)
        {
            string displayName;
            string guid;

            im.GetFieldByName(IMessaging.ADAPTER_DISPLAY_NAME, out displayName);
            im.GetFieldByName(IMessaging.ADAPTER_GUID, out guid);

            string key = displayName + guid;

            AdapterBase stopThisAdapter = (AdapterBase) runningAdapters[key];

            stopThisAdapter.SignalShutdown();
        }

        public InternalMessage RestartAdapter(InternalMessage im)
        {
            string displayName;
            string guid;
            string mediaServerHandle;

            im.GetFieldByName(IMessaging.ADAPTER_DISPLAY_NAME, out displayName);
            im.GetFieldByName(IMessaging.ADAPTER_GUID, out guid);
            im.GetFieldByName(IMessaging.MEDIA_SERVER_HANDLE, out mediaServerHandle);


            string key = displayName + guid;

            AdapterBase restartThisAdapter = (AdapterBase) runningAdapters[key];

            if(restartThisAdapter.Restart(mediaServerHandle))
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

        public void RegisterAllEvents()
        {
            conduit.RegisterSystemEvents(new IConduit.ConduitDelegate( this.RequestAdapter ), IMessaging.INITIALIZE_ADAPTER);
            conduit.RegisterSystemEvents(new IConduit.ConduitDelegate( this.StartAdapter ), IMessaging.START_ADAPTER);
        }
	}
}
