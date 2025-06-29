using System;
using System.Collections;

using Metreos.Samoa.Core;
using Metreos.MmsTester.Interfaces;

namespace Metreos.MmsTester.Conduit
{
	/// <summary>
	/// Provides the interface to the external world for the Media Server Test Tool
	/// </summary>
	public class Conduit
	{
        Hashtable mediaServerCommandMap = new Hashtable();
        Hashtable systemCommandMap = new Hashtable();
        Hashtable testConfigMap = new Hashtable();
        Hashtable unsolicitedEvents = new Hashtable();
        Hashtable mediaServerInfoRequests = new Hashtable();

		public Conduit()
		{
			
		}

        #region Commands From External Source

        public bool SendMediaServerCommand(InternalMessage im, IConduit.VisualDelegate outsideSourceCallback)
        {
            if(mediaServerCommandMap.ContainsKey(im.MessageId))
            {
                IConduit.ConduitDelegate whichFunction = (IConduit.ConduitDelegate) mediaServerCommandMap[im.MessageId];
            
                // Calls the necessary function, which returns an InternalMessage to the visual tool
                if(!outsideSourceCallback( whichFunction(im) ))
                {
                    return false;
                }

                return true;
            }
            else
            {
                return true;
            }
        }

        public bool SendSystemCommand(InternalMessage im, IConduit.VisualDelegate outsideSourceCallback)
        {
            if(systemCommandMap.ContainsKey(im.MessageId))
            {
                IConduit.ConduitDelegate whichFunction = (IConduit.ConduitDelegate) systemCommandMap[im.MessageId];
            
                // Calls the necessary function, which returns an InternalMessage to the visual tool
                if(!outsideSourceCallback( whichFunction(im) ))
                {
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SendTestConfigurationCommand(InternalMessage im, IConduit.VisualDelegate outsideSourceCallback)
        {
            if(testConfigMap.ContainsKey(im.MessageId))
            {
                IConduit.ConduitDelegate whichFunction = (IConduit.ConduitDelegate) testConfigMap[im.MessageId];
            
                // Calls the necessary function, which returns an InternalMessage to the visual tool
                if(!outsideSourceCallback( whichFunction(im) ))
                {
                    return false;
                }

                return true;
            }
            else
            {
                return true;
            }
        }

        public bool RequestMediaServerTable(InternalMessage im, IConduit.VisualDelegate outsideSourceCallback)
        {
            if(mediaServerInfoRequests.ContainsKey(im.MessageId))
            {
                IConduit.ConduitDelegate whichFunction = (IConduit.ConduitDelegate) mediaServerInfoRequests[im.MessageId];
            
                // Calls the necessary function, which returns an InternalMessage to the visual tool
                if(!outsideSourceCallback( whichFunction(im) ))
                {
                    return false;
                }

                return true;
            }
            else
            {
                return true;
            }
        }
        #endregion Commands From External Source

        #region Commands From Internal Source

        public bool SendMediaServerUpdate(InternalMessage im)
        {
            //im.MessageId
            return true;
        }

        #endregion Commands From Internal Source

        #region Registering Events

        public void RegisterMediaServerEvents(IConduit.ConduitDelegate callback, string messageType)
        {
            mediaServerCommandMap[messageType] = callback;
        }

        public void RegisterSystemEvents(IConduit.ConduitDelegate callback, string messageType)
        {
            systemCommandMap[messageType] = callback;
        }

        public void RegisterTestConfigEvents(IConduit.ConduitDelegate callback, string messageType)
        {
            testConfigMap[messageType] = callback; 
        }

        public void RegisterUnsolicitedEvents(IConduit.ConduitDelegate callback, string messageType)
        {
            if(unsolicitedEvents.ContainsKey(messageType))
            {
                IConduit.ConduitDelegate callbackList = (IConduit.ConduitDelegate) unsolicitedEvents[messageType];
                callbackList += callback;
            }
            else
            {
                unsolicitedEvents[messageType] = callback;
            }   
            
        }

        public void RegisterMediaServerInfoRequests(IConduit.ConduitDelegate callback, string messageType)
        {
            mediaServerInfoRequests[messageType] = callback;
        }

        #endregion Registering Events
    }

}
