using System;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Specialized;

namespace Metreos.Messaging
{
    [Serializable]
    public class EventMessage : InternalMessage
    {
        public enum EventType
        {
            Triggering,
            NonTriggering,
            AsyncCallback
        }

        private EventType eventType;
        public EventType Type 
        { 
            get { return eventType; } 
            set { eventType = value; }
        }

        private string appName;
        public string AppName
        {
            get { return appName; }
            set { appName = value; }
        }

        private string scriptName;
        public string ScriptName
        {
            get { return scriptName; }
            set { scriptName = value; }
        }

        private object userData;
        public object UserData 
        {
            get { return userData; }
            set 
            { 
                if(eventType == EventType.AsyncCallback) 
                {
                    userData = value;
                }
            }
        }

        private string sessionGuid;
        public string SessionGuid
        {
            get { return sessionGuid; }
            set 
            { 
                if(eventType == EventType.Triggering)
                {
                    sessionGuid = value;
                }
            }
        }

        private string partitionName;
        public string PartitionName
        {
            get { return partitionName; }
            set { partitionName = value; }
        }

        private CultureInfo culture;
        public CultureInfo Culture
        {
            get { return culture; }
            set { culture = value; }
        }

        private bool suppressNoHandler = false;
        public bool SuppressNoHandler
        {
            get { return suppressNoHandler; }
            set { suppressNoHandler = value; }
        }

        public override bool IsComplete
        {
            get
            {
                if((base.IsComplete) && (routingGuid != null)) { return true; }
                return false;
            }
        }

		public EventMessage(EventType eventType, string routingGuid)
		{
            Debug.Assert(routingGuid != null, "Cannot create EventMessage with a null routing GUID");

            this.eventType = eventType;
            this.routingGuid = routingGuid;
		}

        public override string ToString()
        {
            StringDictionary memberHash = new StringDictionary();
            memberHash.Add("EventType", eventType.ToString());
            memberHash.Add("Routing GUID", routingGuid == null ? "unspecified" : routingGuid);
            memberHash.Add("UserData", userData == null ? "unspecified" : "specified");
            memberHash.Add("Session GUID", sessionGuid == null ? "unspecified" : sessionGuid);
            memberHash.Add("Suppress 'NoHandler'", suppressNoHandler.ToString());
            
            return ToString("EventMessage", memberHash);
        }
	}
}
