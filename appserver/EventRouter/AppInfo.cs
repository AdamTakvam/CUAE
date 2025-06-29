using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Messaging;
using Metreos.Utilities;

namespace Metreos.AppServer.EventRouter
{
	internal sealed class AppInfo
	{
        private MessageQueueWriter appQ;
        public MessageQueueWriter AppQ { get { return appQ; } }

        // Collection of master trigger IDs
        private StringCollection masterTriggerIds;
        public StringCollection MasterTriggerIds { get { return masterTriggerIds; } }

        // SessionGuid -> StringCollection of slave trigger IDs
        private Hashtable sessionTriggers;
        
        public IEnumerator SessionGuids
        {
            get { return sessionTriggers != null ? sessionTriggers.Keys.GetEnumerator() : null; }
        }

        public AppInfo(MessageQueueWriter appQ)
        {
            Assertion.Check(appQ != null, "Cannot create AppInfo with null MessageQueueWriter.");

            this.masterTriggerIds = new StringCollection();
            this.sessionTriggers = new Hashtable();
            this.appQ = appQ;
        }

        public void AddMasterTriggerId(string triggerId)
        {
            if(masterTriggerIds != null)
            {
                masterTriggerIds.Add(triggerId);
            }
        }

        public void AddSlaveTriggerId(string sessionGuid, string triggerId)
        {
            if(sessionTriggers != null)
            {
                StringCollection sc = sessionTriggers[sessionGuid] as StringCollection;

                if(sc == null)
                {
                    sc = new StringCollection();
                    sessionTriggers[sessionGuid] = sc;
                }

                sc.Add(triggerId);
            }
        }

        public StringCollection GetSlaveTriggerIds(string sessionGuid)
        {
            if(sessionTriggers != null)
            {
                return sessionTriggers[sessionGuid] as StringCollection;
            }

            return null;
        }

        public void RemoveSession(string sessionGuid)
        {
            if(sessionTriggers != null)
            {
                sessionTriggers.Remove(sessionGuid);
            }
        }

        public void Clear()
        {
            if(appQ != null)
            {
                appQ.Dispose();
                appQ = null;
            }

            if(masterTriggerIds != null)
            {
                masterTriggerIds.Clear();
                masterTriggerIds = null;
            }

            if(sessionTriggers != null)
            {
                sessionTriggers.Clear();
                sessionTriggers = null;
            }
        }
    }
}
